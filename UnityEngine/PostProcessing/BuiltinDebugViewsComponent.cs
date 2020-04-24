using System;
using System.Collections.Generic;
using UnityEngine.Rendering;

namespace UnityEngine.PostProcessing
{
	public sealed class BuiltinDebugViewsComponent : PostProcessingComponentCommandBuffer<BuiltinDebugViewsModel>
	{
		public override bool active
		{
			get
			{
				return base.model.IsModeActive(BuiltinDebugViewsModel.Mode.Depth) || base.model.IsModeActive(BuiltinDebugViewsModel.Mode.Normals) || base.model.IsModeActive(BuiltinDebugViewsModel.Mode.MotionVectors);
			}
		}

		public override DepthTextureMode GetCameraFlags()
		{
			BuiltinDebugViewsModel.Mode mode = base.model.settings.mode;
			DepthTextureMode depthTextureMode = 0;
			if (mode != BuiltinDebugViewsModel.Mode.Normals)
			{
				if (mode != BuiltinDebugViewsModel.Mode.MotionVectors)
				{
					if (mode == BuiltinDebugViewsModel.Mode.Depth)
					{
						depthTextureMode |= 1;
					}
				}
				else
				{
					depthTextureMode |= 5;
				}
			}
			else
			{
				depthTextureMode |= 2;
			}
			return depthTextureMode;
		}

		public override CameraEvent GetCameraEvent()
		{
			return (base.model.settings.mode != BuiltinDebugViewsModel.Mode.MotionVectors) ? 12 : 18;
		}

		public override string GetName()
		{
			return "Builtin Debug Views";
		}

		public override void PopulateCommandBuffer(CommandBuffer cb)
		{
			BuiltinDebugViewsModel.Settings settings = base.model.settings;
			Material material = this.context.materialFactory.Get("Hidden/Post FX/Builtin Debug Views");
			material.shaderKeywords = null;
			if (this.context.isGBufferAvailable)
			{
				material.EnableKeyword("SOURCE_GBUFFER");
			}
			BuiltinDebugViewsModel.Mode mode = settings.mode;
			if (mode != BuiltinDebugViewsModel.Mode.Depth)
			{
				if (mode != BuiltinDebugViewsModel.Mode.Normals)
				{
					if (mode == BuiltinDebugViewsModel.Mode.MotionVectors)
					{
						this.MotionVectorsPass(cb);
					}
				}
				else
				{
					this.DepthNormalsPass(cb);
				}
			}
			else
			{
				this.DepthPass(cb);
			}
			this.context.Interrupt();
		}

		private void DepthPass(CommandBuffer cb)
		{
			Material material = this.context.materialFactory.Get("Hidden/Post FX/Builtin Debug Views");
			BuiltinDebugViewsModel.DepthSettings depth = base.model.settings.depth;
			cb.SetGlobalFloat(BuiltinDebugViewsComponent.Uniforms._DepthScale, 1f / depth.scale);
			cb.Blit(null, 2, material, 0);
		}

		private void DepthNormalsPass(CommandBuffer cb)
		{
			Material material = this.context.materialFactory.Get("Hidden/Post FX/Builtin Debug Views");
			cb.Blit(null, 2, material, 1);
		}

		private void MotionVectorsPass(CommandBuffer cb)
		{
			Material material = this.context.materialFactory.Get("Hidden/Post FX/Builtin Debug Views");
			BuiltinDebugViewsModel.MotionVectorsSettings motionVectors = base.model.settings.motionVectors;
			int num = BuiltinDebugViewsComponent.Uniforms._TempRT;
			cb.GetTemporaryRT(num, this.context.width, this.context.height, 0, 1);
			cb.SetGlobalFloat(BuiltinDebugViewsComponent.Uniforms._Opacity, motionVectors.sourceOpacity);
			cb.SetGlobalTexture(BuiltinDebugViewsComponent.Uniforms._MainTex, 2);
			cb.Blit(2, num, material, 2);
			if (motionVectors.motionImageOpacity > 0f && motionVectors.motionImageAmplitude > 0f)
			{
				int tempRT = BuiltinDebugViewsComponent.Uniforms._TempRT2;
				cb.GetTemporaryRT(tempRT, this.context.width, this.context.height, 0, 1);
				cb.SetGlobalFloat(BuiltinDebugViewsComponent.Uniforms._Opacity, motionVectors.motionImageOpacity);
				cb.SetGlobalFloat(BuiltinDebugViewsComponent.Uniforms._Amplitude, motionVectors.motionImageAmplitude);
				cb.SetGlobalTexture(BuiltinDebugViewsComponent.Uniforms._MainTex, num);
				cb.Blit(num, tempRT, material, 3);
				cb.ReleaseTemporaryRT(num);
				num = tempRT;
			}
			if (motionVectors.motionVectorsOpacity > 0f && motionVectors.motionVectorsAmplitude > 0f)
			{
				this.PrepareArrows();
				float num2 = 1f / (float)motionVectors.motionVectorsResolution;
				float num3 = num2 * (float)this.context.height / (float)this.context.width;
				cb.SetGlobalVector(BuiltinDebugViewsComponent.Uniforms._Scale, new Vector2(num3, num2));
				cb.SetGlobalFloat(BuiltinDebugViewsComponent.Uniforms._Opacity, motionVectors.motionVectorsOpacity);
				cb.SetGlobalFloat(BuiltinDebugViewsComponent.Uniforms._Amplitude, motionVectors.motionVectorsAmplitude);
				cb.DrawMesh(this.m_Arrows.mesh, Matrix4x4.identity, material, 0, 4);
			}
			cb.SetGlobalTexture(BuiltinDebugViewsComponent.Uniforms._MainTex, num);
			cb.Blit(num, 2);
			cb.ReleaseTemporaryRT(num);
		}

		private void PrepareArrows()
		{
			int motionVectorsResolution = base.model.settings.motionVectors.motionVectorsResolution;
			int num = motionVectorsResolution * Screen.width / Screen.height;
			if (this.m_Arrows == null)
			{
				this.m_Arrows = new BuiltinDebugViewsComponent.ArrowArray();
			}
			if (this.m_Arrows.columnCount != num || this.m_Arrows.rowCount != motionVectorsResolution)
			{
				this.m_Arrows.Release();
				this.m_Arrows.BuildMesh(num, motionVectorsResolution);
			}
		}

		public override void OnDisable()
		{
			if (this.m_Arrows != null)
			{
				this.m_Arrows.Release();
			}
			this.m_Arrows = null;
		}

		private const string k_ShaderString = "Hidden/Post FX/Builtin Debug Views";

		private BuiltinDebugViewsComponent.ArrowArray m_Arrows;

		private static class Uniforms
		{
			internal static readonly int _DepthScale = Shader.PropertyToID("_DepthScale");

			internal static readonly int _TempRT = Shader.PropertyToID("_TempRT");

			internal static readonly int _Opacity = Shader.PropertyToID("_Opacity");

			internal static readonly int _MainTex = Shader.PropertyToID("_MainTex");

			internal static readonly int _TempRT2 = Shader.PropertyToID("_TempRT2");

			internal static readonly int _Amplitude = Shader.PropertyToID("_Amplitude");

			internal static readonly int _Scale = Shader.PropertyToID("_Scale");
		}

		private enum Pass
		{
			Depth,
			Normals,
			MovecOpacity,
			MovecImaging,
			MovecArrows
		}

		private class ArrowArray
		{
			public Mesh mesh { get; private set; }

			public int columnCount { get; private set; }

			public int rowCount { get; private set; }

			public void BuildMesh(int columns, int rows)
			{
				Vector3[] array = new Vector3[]
				{
					new Vector3(0f, 0f, 0f),
					new Vector3(0f, 1f, 0f),
					new Vector3(0f, 1f, 0f),
					new Vector3(-1f, 1f, 0f),
					new Vector3(0f, 1f, 0f),
					new Vector3(1f, 1f, 0f)
				};
				int num = 6 * columns * rows;
				List<Vector3> list = new List<Vector3>(num);
				List<Vector2> list2 = new List<Vector2>(num);
				for (int i = 0; i < rows; i++)
				{
					for (int j = 0; j < columns; j++)
					{
						Vector2 item;
						item..ctor((0.5f + (float)j) / (float)columns, (0.5f + (float)i) / (float)rows);
						for (int k = 0; k < 6; k++)
						{
							list.Add(array[k]);
							list2.Add(item);
						}
					}
				}
				int[] array2 = new int[num];
				for (int l = 0; l < num; l++)
				{
					array2[l] = l;
				}
				this.mesh = new Mesh
				{
					hideFlags = 52
				};
				this.mesh.SetVertices(list);
				this.mesh.SetUVs(0, list2);
				this.mesh.SetIndices(array2, 3, 0);
				this.mesh.UploadMeshData(true);
				this.columnCount = columns;
				this.rowCount = rows;
			}

			public void Release()
			{
				GraphicsUtils.Destroy(this.mesh);
				this.mesh = null;
			}
		}
	}
}
