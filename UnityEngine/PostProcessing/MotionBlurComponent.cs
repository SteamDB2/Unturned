using System;
using System.Runtime.CompilerServices;
using UnityEngine.Rendering;

namespace UnityEngine.PostProcessing
{
	public sealed class MotionBlurComponent : PostProcessingComponentCommandBuffer<MotionBlurModel>
	{
		public MotionBlurComponent.ReconstructionFilter reconstructionFilter
		{
			get
			{
				if (this.m_ReconstructionFilter == null)
				{
					this.m_ReconstructionFilter = new MotionBlurComponent.ReconstructionFilter();
				}
				return this.m_ReconstructionFilter;
			}
		}

		public MotionBlurComponent.FrameBlendingFilter frameBlendingFilter
		{
			get
			{
				if (this.m_FrameBlendingFilter == null)
				{
					this.m_FrameBlendingFilter = new MotionBlurComponent.FrameBlendingFilter();
				}
				return this.m_FrameBlendingFilter;
			}
		}

		public override bool active
		{
			get
			{
				MotionBlurModel.Settings settings = base.model.settings;
				return base.model.enabled && ((settings.shutterAngle > 0f && this.reconstructionFilter.IsSupported()) || settings.frameBlending > 0f) && SystemInfo.graphicsDeviceType != 8 && !this.context.interrupted;
			}
		}

		public override string GetName()
		{
			return "Motion Blur";
		}

		public void ResetHistory()
		{
			if (this.m_FrameBlendingFilter != null)
			{
				this.m_FrameBlendingFilter.Dispose();
			}
			this.m_FrameBlendingFilter = null;
		}

		public override DepthTextureMode GetCameraFlags()
		{
			return 5;
		}

		public override CameraEvent GetCameraEvent()
		{
			return 18;
		}

		public override void OnEnable()
		{
			this.m_FirstFrame = true;
		}

		public override void PopulateCommandBuffer(CommandBuffer cb)
		{
			if (this.m_FirstFrame)
			{
				this.m_FirstFrame = false;
				return;
			}
			Material material = this.context.materialFactory.Get("Hidden/Post FX/Motion Blur");
			Material material2 = this.context.materialFactory.Get("Hidden/Post FX/Blit");
			MotionBlurModel.Settings settings = base.model.settings;
			RenderTextureFormat renderTextureFormat = (!this.context.isHdr) ? 7 : 9;
			int tempRT = MotionBlurComponent.Uniforms._TempRT;
			cb.GetTemporaryRT(tempRT, this.context.width, this.context.height, 0, 0, renderTextureFormat);
			if (settings.shutterAngle > 0f && settings.frameBlending > 0f)
			{
				this.reconstructionFilter.ProcessImage(this.context, cb, ref settings, 2, tempRT, material);
				this.frameBlendingFilter.BlendFrames(cb, settings.frameBlending, tempRT, 2, material);
				this.frameBlendingFilter.PushFrame(cb, tempRT, this.context.width, this.context.height, material);
			}
			else if (settings.shutterAngle > 0f)
			{
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._MainTex, 2);
				cb.Blit(2, tempRT, material2, 0);
				this.reconstructionFilter.ProcessImage(this.context, cb, ref settings, tempRT, 2, material);
			}
			else if (settings.frameBlending > 0f)
			{
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._MainTex, 2);
				cb.Blit(2, tempRT, material2, 0);
				this.frameBlendingFilter.BlendFrames(cb, settings.frameBlending, tempRT, 2, material);
				this.frameBlendingFilter.PushFrame(cb, tempRT, this.context.width, this.context.height, material);
			}
			cb.ReleaseTemporaryRT(tempRT);
		}

		public override void OnDisable()
		{
			if (this.m_FrameBlendingFilter != null)
			{
				this.m_FrameBlendingFilter.Dispose();
			}
		}

		private MotionBlurComponent.ReconstructionFilter m_ReconstructionFilter;

		private MotionBlurComponent.FrameBlendingFilter m_FrameBlendingFilter;

		private bool m_FirstFrame = true;

		private static class Uniforms
		{
			internal static readonly int _VelocityScale = Shader.PropertyToID("_VelocityScale");

			internal static readonly int _MaxBlurRadius = Shader.PropertyToID("_MaxBlurRadius");

			internal static readonly int _VelocityTex = Shader.PropertyToID("_VelocityTex");

			internal static readonly int _Tile4RT = Shader.PropertyToID("_Tile4RT");

			internal static readonly int _MainTex = Shader.PropertyToID("_MainTex");

			internal static readonly int _Tile8RT = Shader.PropertyToID("_Tile8RT");

			internal static readonly int _TileMaxOffs = Shader.PropertyToID("_TileMaxOffs");

			internal static readonly int _TileMaxLoop = Shader.PropertyToID("_TileMaxLoop");

			internal static readonly int _TileVRT = Shader.PropertyToID("_TileVRT");

			internal static readonly int _NeighborMaxTex = Shader.PropertyToID("_NeighborMaxTex");

			internal static readonly int _LoopCount = Shader.PropertyToID("_LoopCount");

			internal static readonly int _TempRT = Shader.PropertyToID("_TempRT");

			internal static readonly int _History1LumaTex = Shader.PropertyToID("_History1LumaTex");

			internal static readonly int _History2LumaTex = Shader.PropertyToID("_History2LumaTex");

			internal static readonly int _History3LumaTex = Shader.PropertyToID("_History3LumaTex");

			internal static readonly int _History4LumaTex = Shader.PropertyToID("_History4LumaTex");

			internal static readonly int _History1ChromaTex = Shader.PropertyToID("_History1ChromaTex");

			internal static readonly int _History2ChromaTex = Shader.PropertyToID("_History2ChromaTex");

			internal static readonly int _History3ChromaTex = Shader.PropertyToID("_History3ChromaTex");

			internal static readonly int _History4ChromaTex = Shader.PropertyToID("_History4ChromaTex");

			internal static readonly int _History1Weight = Shader.PropertyToID("_History1Weight");

			internal static readonly int _History2Weight = Shader.PropertyToID("_History2Weight");

			internal static readonly int _History3Weight = Shader.PropertyToID("_History3Weight");

			internal static readonly int _History4Weight = Shader.PropertyToID("_History4Weight");
		}

		private enum Pass
		{
			VelocitySetup,
			TileMax4,
			TileMax2,
			TileMaxV,
			NeighborMax,
			Reconstruction,
			ReconstructionUnroll,
			FrameCompression,
			FrameBlendingChroma,
			FrameBlendingRaw
		}

		public class ReconstructionFilter
		{
			public ReconstructionFilter()
			{
				this.CheckTextureFormatSupport();
				this.m_UseUnrolledShader = SystemInfo.graphicsDeviceName.Contains("Adreno");
			}

			private void CheckTextureFormatSupport()
			{
				if (!SystemInfo.SupportsRenderTextureFormat(this.m_PackedRTFormat))
				{
					this.m_PackedRTFormat = 0;
				}
			}

			public bool IsSupported()
			{
				return SystemInfo.supportsMotionVectors;
			}

			public void ProcessImage(PostProcessingContext context, CommandBuffer cb, ref MotionBlurModel.Settings settings, RenderTargetIdentifier source, RenderTargetIdentifier destination, Material material)
			{
				int num = (int)(5f * (float)context.height / 100f);
				int num2 = ((num - 1) / 8 + 1) * 8;
				float num3 = settings.shutterAngle / 360f * 1.45f;
				cb.SetGlobalFloat(MotionBlurComponent.Uniforms._VelocityScale, num3);
				cb.SetGlobalFloat(MotionBlurComponent.Uniforms._MaxBlurRadius, (float)num);
				int velocityTex = MotionBlurComponent.Uniforms._VelocityTex;
				cb.GetTemporaryRT(velocityTex, context.width, context.height, 0, 0, this.m_PackedRTFormat);
				cb.Blit(null, velocityTex, material, 0);
				int tile4RT = MotionBlurComponent.Uniforms._Tile4RT;
				cb.GetTemporaryRT(tile4RT, context.width / 4, context.height / 4, 0, 0, this.m_VectorRTFormat);
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._MainTex, velocityTex);
				cb.Blit(velocityTex, tile4RT, material, 1);
				int tile8RT = MotionBlurComponent.Uniforms._Tile8RT;
				cb.GetTemporaryRT(tile8RT, context.width / 8, context.height / 8, 0, 0, this.m_VectorRTFormat);
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._MainTex, tile4RT);
				cb.Blit(tile4RT, tile8RT, material, 2);
				cb.ReleaseTemporaryRT(tile4RT);
				Vector2 vector = Vector2.one * ((float)num2 / 8f - 1f) * -0.5f;
				cb.SetGlobalVector(MotionBlurComponent.Uniforms._TileMaxOffs, vector);
				cb.SetGlobalFloat(MotionBlurComponent.Uniforms._TileMaxLoop, (float)((int)((float)num2 / 8f)));
				int tileVRT = MotionBlurComponent.Uniforms._TileVRT;
				cb.GetTemporaryRT(tileVRT, context.width / num2, context.height / num2, 0, 0, this.m_VectorRTFormat);
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._MainTex, tile8RT);
				cb.Blit(tile8RT, tileVRT, material, 3);
				cb.ReleaseTemporaryRT(tile8RT);
				int neighborMaxTex = MotionBlurComponent.Uniforms._NeighborMaxTex;
				int num4 = context.width / num2;
				int num5 = context.height / num2;
				cb.GetTemporaryRT(neighborMaxTex, num4, num5, 0, 0, this.m_VectorRTFormat);
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._MainTex, tileVRT);
				cb.Blit(tileVRT, neighborMaxTex, material, 4);
				cb.ReleaseTemporaryRT(tileVRT);
				cb.SetGlobalFloat(MotionBlurComponent.Uniforms._LoopCount, (float)Mathf.Clamp(settings.sampleCount / 2, 1, 64));
				cb.SetGlobalFloat(MotionBlurComponent.Uniforms._MaxBlurRadius, (float)num);
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._MainTex, source);
				cb.Blit(source, destination, material, (!this.m_UseUnrolledShader) ? 5 : 6);
				cb.ReleaseTemporaryRT(velocityTex);
				cb.ReleaseTemporaryRT(neighborMaxTex);
			}

			private RenderTextureFormat m_VectorRTFormat = 13;

			private RenderTextureFormat m_PackedRTFormat = 8;

			private bool m_UseUnrolledShader;
		}

		public class FrameBlendingFilter
		{
			public FrameBlendingFilter()
			{
				this.m_UseCompression = MotionBlurComponent.FrameBlendingFilter.CheckSupportCompression();
				this.m_RawTextureFormat = MotionBlurComponent.FrameBlendingFilter.GetPreferredRenderTextureFormat();
				this.m_FrameList = new MotionBlurComponent.FrameBlendingFilter.Frame[4];
			}

			public void Dispose()
			{
				foreach (MotionBlurComponent.FrameBlendingFilter.Frame frame in this.m_FrameList)
				{
					frame.Release();
				}
			}

			public void PushFrame(CommandBuffer cb, RenderTargetIdentifier source, int width, int height, Material material)
			{
				int frameCount = Time.frameCount;
				if (frameCount == this.m_LastFrameCount)
				{
					return;
				}
				int num = frameCount % this.m_FrameList.Length;
				if (this.m_UseCompression)
				{
					this.m_FrameList[num].MakeRecord(cb, source, width, height, material);
				}
				else
				{
					this.m_FrameList[num].MakeRecordRaw(cb, source, width, height, this.m_RawTextureFormat);
				}
				this.m_LastFrameCount = frameCount;
			}

			public void BlendFrames(CommandBuffer cb, float strength, RenderTargetIdentifier source, RenderTargetIdentifier destination, Material material)
			{
				float time = Time.time;
				MotionBlurComponent.FrameBlendingFilter.Frame frameRelative = this.GetFrameRelative(-1);
				MotionBlurComponent.FrameBlendingFilter.Frame frameRelative2 = this.GetFrameRelative(-2);
				MotionBlurComponent.FrameBlendingFilter.Frame frameRelative3 = this.GetFrameRelative(-3);
				MotionBlurComponent.FrameBlendingFilter.Frame frameRelative4 = this.GetFrameRelative(-4);
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._History1LumaTex, frameRelative.lumaTexture);
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._History2LumaTex, frameRelative2.lumaTexture);
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._History3LumaTex, frameRelative3.lumaTexture);
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._History4LumaTex, frameRelative4.lumaTexture);
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._History1ChromaTex, frameRelative.chromaTexture);
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._History2ChromaTex, frameRelative2.chromaTexture);
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._History3ChromaTex, frameRelative3.chromaTexture);
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._History4ChromaTex, frameRelative4.chromaTexture);
				cb.SetGlobalFloat(MotionBlurComponent.Uniforms._History1Weight, frameRelative.CalculateWeight(strength, time));
				cb.SetGlobalFloat(MotionBlurComponent.Uniforms._History2Weight, frameRelative2.CalculateWeight(strength, time));
				cb.SetGlobalFloat(MotionBlurComponent.Uniforms._History3Weight, frameRelative3.CalculateWeight(strength, time));
				cb.SetGlobalFloat(MotionBlurComponent.Uniforms._History4Weight, frameRelative4.CalculateWeight(strength, time));
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._MainTex, source);
				cb.Blit(source, destination, material, (!this.m_UseCompression) ? 9 : 8);
			}

			private static bool CheckSupportCompression()
			{
				return SystemInfo.SupportsRenderTextureFormat(16) && SystemInfo.supportedRenderTargetCount > 1;
			}

			private static RenderTextureFormat GetPreferredRenderTextureFormat()
			{
				RenderTextureFormat[] array = new RenderTextureFormat[3];
				RuntimeHelpers.InitializeArray(array, fieldof(<PrivateImplementationDetails>.$field-51A7A390CD6DE245186881400B18C9D822EFE240).FieldHandle);
				RenderTextureFormat[] array2 = array;
				foreach (RenderTextureFormat renderTextureFormat in array2)
				{
					if (SystemInfo.SupportsRenderTextureFormat(renderTextureFormat))
					{
						return renderTextureFormat;
					}
				}
				return 7;
			}

			private MotionBlurComponent.FrameBlendingFilter.Frame GetFrameRelative(int offset)
			{
				int num = (Time.frameCount + this.m_FrameList.Length + offset) % this.m_FrameList.Length;
				return this.m_FrameList[num];
			}

			private bool m_UseCompression;

			private RenderTextureFormat m_RawTextureFormat;

			private MotionBlurComponent.FrameBlendingFilter.Frame[] m_FrameList;

			private int m_LastFrameCount;

			private struct Frame
			{
				public float CalculateWeight(float strength, float currentTime)
				{
					if (Mathf.Approximately(this.m_Time, 0f))
					{
						return 0f;
					}
					float num = Mathf.Lerp(80f, 16f, strength);
					return Mathf.Exp((this.m_Time - currentTime) * num);
				}

				public void Release()
				{
					if (this.lumaTexture != null)
					{
						RenderTexture.ReleaseTemporary(this.lumaTexture);
					}
					if (this.chromaTexture != null)
					{
						RenderTexture.ReleaseTemporary(this.chromaTexture);
					}
					this.lumaTexture = null;
					this.chromaTexture = null;
				}

				public void MakeRecord(CommandBuffer cb, RenderTargetIdentifier source, int width, int height, Material material)
				{
					this.Release();
					this.lumaTexture = RenderTexture.GetTemporary(width, height, 0, 16);
					this.chromaTexture = RenderTexture.GetTemporary(width, height, 0, 16);
					this.lumaTexture.filterMode = 0;
					this.chromaTexture.filterMode = 0;
					if (this.m_MRT == null)
					{
						this.m_MRT = new RenderTargetIdentifier[2];
					}
					this.m_MRT[0] = this.lumaTexture;
					this.m_MRT[1] = this.chromaTexture;
					cb.SetGlobalTexture(MotionBlurComponent.Uniforms._MainTex, source);
					cb.SetRenderTarget(this.m_MRT, this.lumaTexture);
					cb.DrawMesh(GraphicsUtils.quad, Matrix4x4.identity, material, 0, 7);
					this.m_Time = Time.time;
				}

				public void MakeRecordRaw(CommandBuffer cb, RenderTargetIdentifier source, int width, int height, RenderTextureFormat format)
				{
					this.Release();
					this.lumaTexture = RenderTexture.GetTemporary(width, height, 0, format);
					this.lumaTexture.filterMode = 0;
					cb.SetGlobalTexture(MotionBlurComponent.Uniforms._MainTex, source);
					cb.Blit(source, this.lumaTexture);
					this.m_Time = Time.time;
				}

				public RenderTexture lumaTexture;

				public RenderTexture chromaTexture;

				private float m_Time;

				private RenderTargetIdentifier[] m_MRT;
			}
		}
	}
}
