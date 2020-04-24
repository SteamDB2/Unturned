using System;

namespace UnityEngine.PostProcessing
{
	public sealed class EyeAdaptationComponent : PostProcessingComponentRenderTexture<EyeAdaptationModel>
	{
		public override bool active
		{
			get
			{
				return base.model.enabled && SystemInfo.supportsComputeShaders && !this.context.interrupted;
			}
		}

		public override void OnEnable()
		{
			this.m_FirstFrame = true;
		}

		public override void OnDisable()
		{
			foreach (RenderTexture obj in this.m_AutoExposurePool)
			{
				GraphicsUtils.Destroy(obj);
			}
			if (this.m_HistogramBuffer != null)
			{
				this.m_HistogramBuffer.Release();
			}
			this.m_HistogramBuffer = null;
			if (this.m_DebugHistogram != null)
			{
				this.m_DebugHistogram.Release();
			}
			this.m_DebugHistogram = null;
		}

		private Vector4 GetHistogramScaleOffsetRes()
		{
			EyeAdaptationModel.Settings settings = base.model.settings;
			float num = (float)(settings.logMax - settings.logMin);
			float num2 = 1f / num;
			float num3 = (float)(-(float)settings.logMin) * num2;
			return new Vector4(num2, num3, Mathf.Floor((float)this.context.width / 2f), Mathf.Floor((float)this.context.height / 2f));
		}

		public Texture Prepare(RenderTexture source, Material uberMaterial)
		{
			EyeAdaptationModel.Settings settings = base.model.settings;
			if (this.m_EyeCompute == null)
			{
				this.m_EyeCompute = Resources.Load<ComputeShader>("Shaders/EyeHistogram");
			}
			Material material = this.context.materialFactory.Get("Hidden/Post FX/Eye Adaptation");
			if (this.m_HistogramBuffer == null)
			{
				this.m_HistogramBuffer = new ComputeBuffer(64, 4);
			}
			if (EyeAdaptationComponent.s_EmptyHistogramBuffer == null)
			{
				EyeAdaptationComponent.s_EmptyHistogramBuffer = new uint[64];
			}
			Vector4 histogramScaleOffsetRes = this.GetHistogramScaleOffsetRes();
			RenderTexture renderTexture = this.context.renderTextureFactory.Get((int)histogramScaleOffsetRes.z, (int)histogramScaleOffsetRes.w, 0, source.format, 0, 1, 1, "FactoryTempTexture");
			Graphics.Blit(source, renderTexture);
			if (this.m_AutoExposurePool[0] == null || !this.m_AutoExposurePool[0].IsCreated())
			{
				this.m_AutoExposurePool[0] = new RenderTexture(1, 1, 0, 14);
			}
			if (this.m_AutoExposurePool[1] == null || !this.m_AutoExposurePool[1].IsCreated())
			{
				this.m_AutoExposurePool[1] = new RenderTexture(1, 1, 0, 14);
			}
			this.m_HistogramBuffer.SetData(EyeAdaptationComponent.s_EmptyHistogramBuffer);
			int num = this.m_EyeCompute.FindKernel("KEyeHistogram");
			this.m_EyeCompute.SetBuffer(num, "_Histogram", this.m_HistogramBuffer);
			this.m_EyeCompute.SetTexture(num, "_Source", renderTexture);
			this.m_EyeCompute.SetVector("_ScaleOffsetRes", histogramScaleOffsetRes);
			this.m_EyeCompute.Dispatch(num, Mathf.CeilToInt((float)renderTexture.width / 16f), Mathf.CeilToInt((float)renderTexture.height / 16f), 1);
			this.context.renderTextureFactory.Release(renderTexture);
			settings.highPercent = Mathf.Clamp(settings.highPercent, 1.01f, 99f);
			settings.lowPercent = Mathf.Clamp(settings.lowPercent, 1f, settings.highPercent - 0.01f);
			material.SetBuffer("_Histogram", this.m_HistogramBuffer);
			material.SetVector(EyeAdaptationComponent.Uniforms._Params, new Vector4(settings.lowPercent * 0.01f, settings.highPercent * 0.01f, settings.minLuminance, settings.maxLuminance));
			material.SetVector(EyeAdaptationComponent.Uniforms._Speed, new Vector2(settings.speedDown, settings.speedUp));
			material.SetVector(EyeAdaptationComponent.Uniforms._ScaleOffsetRes, histogramScaleOffsetRes);
			material.SetFloat(EyeAdaptationComponent.Uniforms._ExposureCompensation, settings.exposureCompensation);
			if (this.m_FirstFrame || !Application.isPlaying)
			{
				this.m_CurrentAutoExposure = this.m_AutoExposurePool[0];
				Graphics.Blit(null, this.m_CurrentAutoExposure, material, 1);
				Graphics.Blit(this.m_AutoExposurePool[0], this.m_AutoExposurePool[1]);
			}
			else
			{
				int num2 = this.m_AutoExposurePingPing;
				RenderTexture renderTexture2 = this.m_AutoExposurePool[++num2 % 2];
				RenderTexture renderTexture3 = this.m_AutoExposurePool[++num2 % 2];
				Graphics.Blit(renderTexture2, renderTexture3, material, (int)settings.adaptationType);
				this.m_AutoExposurePingPing = (num2 + 1) % 2;
				this.m_CurrentAutoExposure = renderTexture3;
			}
			uberMaterial.EnableKeyword("EYE_ADAPTATION");
			uberMaterial.SetTexture(EyeAdaptationComponent.Uniforms._AutoExposure, this.m_CurrentAutoExposure);
			if (this.context.profile.debugViews.IsModeActive(BuiltinDebugViewsModel.Mode.EyeAdaptation))
			{
				if (this.m_DebugHistogram == null || !this.m_DebugHistogram.IsCreated())
				{
					this.m_DebugHistogram = new RenderTexture(256, 128, 0, 0)
					{
						filterMode = 0,
						wrapMode = 1
					};
				}
				material.SetFloat(EyeAdaptationComponent.Uniforms._DebugWidth, (float)this.m_DebugHistogram.width);
				Graphics.Blit(null, this.m_DebugHistogram, material, 2);
			}
			this.m_FirstFrame = false;
			return this.m_CurrentAutoExposure;
		}

		public void OnGUI()
		{
			if (this.m_DebugHistogram == null || !this.m_DebugHistogram.IsCreated())
			{
				return;
			}
			Rect rect;
			rect..ctor(this.context.viewport.x * (float)Screen.width + 8f, 8f, (float)this.m_DebugHistogram.width, (float)this.m_DebugHistogram.height);
			GUI.DrawTexture(rect, this.m_DebugHistogram);
		}

		private ComputeShader m_EyeCompute;

		private ComputeBuffer m_HistogramBuffer;

		private readonly RenderTexture[] m_AutoExposurePool = new RenderTexture[2];

		private int m_AutoExposurePingPing;

		private RenderTexture m_CurrentAutoExposure;

		private RenderTexture m_DebugHistogram;

		private static uint[] s_EmptyHistogramBuffer;

		private bool m_FirstFrame = true;

		private const int k_HistogramBins = 64;

		private const int k_HistogramThreadX = 16;

		private const int k_HistogramThreadY = 16;

		private static class Uniforms
		{
			internal static readonly int _Params = Shader.PropertyToID("_Params");

			internal static readonly int _Speed = Shader.PropertyToID("_Speed");

			internal static readonly int _ScaleOffsetRes = Shader.PropertyToID("_ScaleOffsetRes");

			internal static readonly int _ExposureCompensation = Shader.PropertyToID("_ExposureCompensation");

			internal static readonly int _AutoExposure = Shader.PropertyToID("_AutoExposure");

			internal static readonly int _DebugWidth = Shader.PropertyToID("_DebugWidth");
		}
	}
}
