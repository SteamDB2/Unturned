using System;

namespace UnityEngine.PostProcessing
{
	public sealed class VignetteComponent : PostProcessingComponentRenderTexture<VignetteModel>
	{
		public override bool active
		{
			get
			{
				return base.model.enabled && !this.context.interrupted;
			}
		}

		public override void Prepare(Material uberMaterial)
		{
			VignetteModel.Settings settings = base.model.settings;
			uberMaterial.SetColor(VignetteComponent.Uniforms._Vignette_Color, settings.color);
			if (settings.mode == VignetteModel.Mode.Classic)
			{
				uberMaterial.SetVector(VignetteComponent.Uniforms._Vignette_Center, settings.center);
				uberMaterial.EnableKeyword("VIGNETTE_CLASSIC");
				float num = (1f - settings.roundness) * 6f + settings.roundness;
				uberMaterial.SetVector(VignetteComponent.Uniforms._Vignette_Settings, new Vector3(settings.intensity * 3f, settings.smoothness * 5f, num));
			}
			else if (settings.mode == VignetteModel.Mode.Round)
			{
				uberMaterial.SetVector(VignetteComponent.Uniforms._Vignette_Center, settings.center);
				uberMaterial.EnableKeyword("VIGNETTE_ROUND");
				uberMaterial.SetVector(VignetteComponent.Uniforms._Vignette_Settings, new Vector3(settings.intensity * 3f, settings.smoothness * 5f, 1f));
			}
			else if (settings.mode == VignetteModel.Mode.Masked && settings.mask != null && settings.opacity > 0f)
			{
				uberMaterial.EnableKeyword("VIGNETTE_MASKED");
				uberMaterial.SetTexture(VignetteComponent.Uniforms._Vignette_Mask, settings.mask);
				uberMaterial.SetFloat(VignetteComponent.Uniforms._Vignette_Opacity, settings.opacity);
			}
		}

		private static class Uniforms
		{
			internal static readonly int _Vignette_Color = Shader.PropertyToID("_Vignette_Color");

			internal static readonly int _Vignette_Center = Shader.PropertyToID("_Vignette_Center");

			internal static readonly int _Vignette_Settings = Shader.PropertyToID("_Vignette_Settings");

			internal static readonly int _Vignette_Mask = Shader.PropertyToID("_Vignette_Mask");

			internal static readonly int _Vignette_Opacity = Shader.PropertyToID("_Vignette_Opacity");
		}
	}
}
