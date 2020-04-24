using System;
using UnityEngine.Rendering;

namespace UnityEngine.PostProcessing
{
	public sealed class AmbientOcclusionComponent : PostProcessingComponentCommandBuffer<AmbientOcclusionModel>
	{
		private AmbientOcclusionComponent.OcclusionSource occlusionSource
		{
			get
			{
				if (this.context.isGBufferAvailable && !base.model.settings.forceForwardCompatibility)
				{
					return AmbientOcclusionComponent.OcclusionSource.GBuffer;
				}
				if (base.model.settings.highPrecision && !this.context.isGBufferAvailable)
				{
					return AmbientOcclusionComponent.OcclusionSource.DepthTexture;
				}
				return AmbientOcclusionComponent.OcclusionSource.DepthNormalsTexture;
			}
		}

		private bool ambientOnlySupported
		{
			get
			{
				return this.context.isHdr && base.model.settings.ambientOnly && this.context.isGBufferAvailable && !base.model.settings.forceForwardCompatibility;
			}
		}

		public override bool active
		{
			get
			{
				return base.model.enabled && base.model.settings.intensity > 0f && !this.context.interrupted;
			}
		}

		public override DepthTextureMode GetCameraFlags()
		{
			DepthTextureMode depthTextureMode = 0;
			if (this.occlusionSource == AmbientOcclusionComponent.OcclusionSource.DepthTexture)
			{
				depthTextureMode |= 1;
			}
			if (this.occlusionSource != AmbientOcclusionComponent.OcclusionSource.GBuffer)
			{
				depthTextureMode |= 2;
			}
			return depthTextureMode;
		}

		public override string GetName()
		{
			return "Ambient Occlusion";
		}

		public override CameraEvent GetCameraEvent()
		{
			return (!this.ambientOnlySupported || this.context.profile.debugViews.IsModeActive(BuiltinDebugViewsModel.Mode.AmbientOcclusion)) ? 12 : 21;
		}

		public override void PopulateCommandBuffer(CommandBuffer cb)
		{
			AmbientOcclusionModel.Settings settings = base.model.settings;
			Material material = this.context.materialFactory.Get("Hidden/Post FX/Blit");
			Material material2 = this.context.materialFactory.Get("Hidden/Post FX/Ambient Occlusion");
			material2.shaderKeywords = null;
			material2.SetFloat(AmbientOcclusionComponent.Uniforms._Intensity, settings.intensity);
			material2.SetFloat(AmbientOcclusionComponent.Uniforms._Radius, settings.radius);
			material2.SetFloat(AmbientOcclusionComponent.Uniforms._Downsample, (!settings.downsampling) ? 1f : 0.5f);
			material2.SetInt(AmbientOcclusionComponent.Uniforms._SampleCount, (int)settings.sampleCount);
			int width = this.context.width;
			int height = this.context.height;
			int num = (!settings.downsampling) ? 1 : 2;
			int num2 = AmbientOcclusionComponent.Uniforms._OcclusionTexture1;
			cb.GetTemporaryRT(num2, width / num, height / num, 0, 1, 0, 1);
			cb.Blit(null, num2, material2, (int)this.occlusionSource);
			int occlusionTexture = AmbientOcclusionComponent.Uniforms._OcclusionTexture2;
			cb.GetTemporaryRT(occlusionTexture, width, height, 0, 1, 0, 1);
			cb.SetGlobalTexture(AmbientOcclusionComponent.Uniforms._MainTex, num2);
			cb.Blit(num2, occlusionTexture, material2, (this.occlusionSource != AmbientOcclusionComponent.OcclusionSource.GBuffer) ? 3 : 4);
			cb.ReleaseTemporaryRT(num2);
			num2 = AmbientOcclusionComponent.Uniforms._OcclusionTexture;
			cb.GetTemporaryRT(num2, width, height, 0, 1, 0, 1);
			cb.SetGlobalTexture(AmbientOcclusionComponent.Uniforms._MainTex, occlusionTexture);
			cb.Blit(occlusionTexture, num2, material2, 5);
			cb.ReleaseTemporaryRT(occlusionTexture);
			if (this.context.profile.debugViews.IsModeActive(BuiltinDebugViewsModel.Mode.AmbientOcclusion))
			{
				cb.SetGlobalTexture(AmbientOcclusionComponent.Uniforms._MainTex, num2);
				cb.Blit(num2, 2, material2, 8);
				this.context.Interrupt();
			}
			else if (this.ambientOnlySupported)
			{
				cb.SetRenderTarget(this.m_MRT, 2);
				cb.DrawMesh(GraphicsUtils.quad, Matrix4x4.identity, material2, 0, 7);
			}
			else
			{
				RenderTextureFormat renderTextureFormat = (!this.context.isHdr) ? 7 : 9;
				int tempRT = AmbientOcclusionComponent.Uniforms._TempRT;
				cb.GetTemporaryRT(tempRT, this.context.width, this.context.height, 0, 1, renderTextureFormat);
				cb.Blit(2, tempRT, material, 0);
				cb.SetGlobalTexture(AmbientOcclusionComponent.Uniforms._MainTex, tempRT);
				cb.Blit(tempRT, 2, material2, 6);
				cb.ReleaseTemporaryRT(tempRT);
			}
			cb.ReleaseTemporaryRT(num2);
		}

		private const string k_BlitShaderString = "Hidden/Post FX/Blit";

		private const string k_ShaderString = "Hidden/Post FX/Ambient Occlusion";

		private readonly RenderTargetIdentifier[] m_MRT = new RenderTargetIdentifier[]
		{
			10,
			2
		};

		private static class Uniforms
		{
			internal static readonly int _Intensity = Shader.PropertyToID("_Intensity");

			internal static readonly int _Radius = Shader.PropertyToID("_Radius");

			internal static readonly int _Downsample = Shader.PropertyToID("_Downsample");

			internal static readonly int _SampleCount = Shader.PropertyToID("_SampleCount");

			internal static readonly int _OcclusionTexture1 = Shader.PropertyToID("_OcclusionTexture1");

			internal static readonly int _OcclusionTexture2 = Shader.PropertyToID("_OcclusionTexture2");

			internal static readonly int _OcclusionTexture = Shader.PropertyToID("_OcclusionTexture");

			internal static readonly int _MainTex = Shader.PropertyToID("_MainTex");

			internal static readonly int _TempRT = Shader.PropertyToID("_TempRT");
		}

		private enum OcclusionSource
		{
			DepthTexture,
			DepthNormalsTexture,
			GBuffer
		}
	}
}
