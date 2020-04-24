using System;

namespace SDG.Unturned
{
	public class GraphicsSettingsData
	{
		public GraphicsSettingsData()
		{
			this.Resolution = new GraphicsSettingsResolution();
			this.IsFullscreenEnabled = false;
			this.IsVSyncEnabled = false;
			this.IsBloomEnabled = false;
			this.IsChromaticAberrationEnabled = false;
			this.IsFilmGrainEnabled = false;
			this.IsCloudEnabled = true;
			this.IsNiceBlendEnabled = true;
			this.IsFogEnabled = true;
			this.IsGrassDisplacementEnabled = false;
			this.IsFoliageFocusEnabled = false;
			this.IsRagdollsEnabled = true;
			this.IsDebrisEnabled = true;
			this.IsBlastEnabled = true;
			this.IsPuddleEnabled = true;
			this.IsGlitterEnabled = true;
			this.IsTriplanarMappingEnabled = true;
			this.IsSkyboxReflectionEnabled = false;
			this.DrawDistance = 1f;
			this.LandmarkDistance = 0f;
			this.AntiAliasingType5 = EAntiAliasingType.OFF;
			this.AnisotropicFilteringMode = EAnisotropicFilteringMode.FORCED_ON;
			this.EffectQuality = EGraphicQuality.MEDIUM;
			this.FoliageQuality2 = EGraphicQuality.OFF;
			this.SunShaftsQuality = EGraphicQuality.OFF;
			this.LightingQuality = EGraphicQuality.LOW;
			this.ScreenSpaceAmbientOcclusionQuality = EGraphicQuality.OFF;
			this.ScreenSpaceReflectionQuality = EGraphicQuality.OFF;
			this.PlanarReflectionQuality = EGraphicQuality.MEDIUM;
			this.WaterQuality = EGraphicQuality.LOW;
			this.ScopeQuality2 = EGraphicQuality.OFF;
			this.OutlineQuality = EGraphicQuality.LOW;
			this.BoneQuality = EGraphicQuality.MEDIUM;
			this.TerrainQuality = EGraphicQuality.MEDIUM;
			this.WindQuality = EGraphicQuality.OFF;
			this.TreeMode = ETreeGraphicMode.LEGACY;
			this.RenderMode2 = ERenderMode.FORWARD;
			this.LandmarkQuality = EGraphicQuality.OFF;
		}

		public GraphicsSettingsResolution Resolution { get; set; }

		public bool IsFullscreenEnabled { get; set; }

		public bool IsVSyncEnabled { get; set; }

		public bool IsBloomEnabled { get; set; }

		public bool IsChromaticAberrationEnabled { get; set; }

		public bool IsFilmGrainEnabled { get; set; }

		public bool IsCloudEnabled { get; set; }

		public bool IsNiceBlendEnabled { get; set; }

		public bool IsFogEnabled { get; set; }

		public float DrawDistance { get; set; }

		public float LandmarkDistance { get; set; }

		public EAntiAliasingType AntiAliasingType5 { get; set; }

		public EAnisotropicFilteringMode AnisotropicFilteringMode { get; set; }

		public EGraphicQuality EffectQuality { get; set; }

		public EGraphicQuality FoliageQuality2 { get; set; }

		public EGraphicQuality SunShaftsQuality { get; set; }

		public EGraphicQuality LightingQuality { get; set; }

		public EGraphicQuality ScreenSpaceAmbientOcclusionQuality { get; set; }

		public EGraphicQuality ScreenSpaceReflectionQuality { get; set; }

		public EGraphicQuality PlanarReflectionQuality { get; set; }

		public EGraphicQuality WaterQuality { get; set; }

		public EGraphicQuality ScopeQuality2 { get; set; }

		public EGraphicQuality OutlineQuality { get; set; }

		public EGraphicQuality BoneQuality { get; set; }

		public EGraphicQuality TerrainQuality { get; set; }

		public EGraphicQuality WindQuality { get; set; }

		public ETreeGraphicMode TreeMode { get; set; }

		public bool IsGrassDisplacementEnabled;

		public bool IsFoliageFocusEnabled;

		public bool IsRagdollsEnabled;

		public bool IsDebrisEnabled;

		public bool IsBlastEnabled;

		public bool IsPuddleEnabled;

		public bool IsGlitterEnabled;

		public bool IsTriplanarMappingEnabled;

		public bool IsSkyboxReflectionEnabled;

		public ERenderMode RenderMode2;

		public EGraphicQuality LandmarkQuality;
	}
}
