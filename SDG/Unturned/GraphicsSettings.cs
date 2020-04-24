using System;
using System.Collections.Generic;
using HighlightingSystem;
using SDG.Framework.Debug;
using SDG.Framework.Foliage;
using UnityEngine;
using UnityEngine.PostProcessing;
using UnityStandardAssets.ImageEffects;

namespace SDG.Unturned
{
	public class GraphicsSettings
	{
		[TerminalCommandProperty("gfx.uncap_landmarks", "unlimited landmark render distance", false)]
		public static bool uncapLandmarks
		{
			get
			{
				return GraphicsSettings._uncapLandmarks;
			}
			set
			{
				GraphicsSettings._uncapLandmarks = value;
				GraphicsSettings.apply();
				TerminalUtility.printCommandPass("Set uncap_landmarks to: " + GraphicsSettings.uncapLandmarks);
			}
		}

		public static PostProcessingProfile mainProfile { get; protected set; }

		public static PostProcessingProfile viewProfile { get; protected set; }

		public static float effect
		{
			get
			{
				if (GraphicsSettings.effectQuality == EGraphicQuality.ULTRA)
				{
					return Random.Range(GraphicsSettings.EFFECT_ULTRA - 16f, GraphicsSettings.EFFECT_ULTRA + 16f);
				}
				if (GraphicsSettings.effectQuality == EGraphicQuality.HIGH)
				{
					return Random.Range(GraphicsSettings.EFFECT_HIGH - 8f, GraphicsSettings.EFFECT_HIGH + 8f);
				}
				if (GraphicsSettings.effectQuality == EGraphicQuality.MEDIUM)
				{
					return Random.Range(GraphicsSettings.EFFECT_MEDIUM - 4f, GraphicsSettings.EFFECT_MEDIUM + 4f);
				}
				if (GraphicsSettings.effectQuality == EGraphicQuality.LOW)
				{
					return Random.Range(GraphicsSettings.EFFECT_LOW - 2f, GraphicsSettings.EFFECT_LOW + 2f);
				}
				return 0f;
			}
		}

		public static GraphicsSettingsResolution resolution
		{
			get
			{
				return GraphicsSettings.graphicsSettingsData.Resolution;
			}
			set
			{
				GraphicsSettings.graphicsSettingsData.Resolution = value;
				GraphicsSettings.changeResolution = true;
			}
		}

		public static bool fullscreen
		{
			get
			{
				return GraphicsSettings.graphicsSettingsData.IsFullscreenEnabled;
			}
			set
			{
				GraphicsSettings.graphicsSettingsData.IsFullscreenEnabled = value;
				GraphicsSettings.changeResolution = true;
			}
		}

		public static bool buffer
		{
			get
			{
				return GraphicsSettings.graphicsSettingsData.IsVSyncEnabled;
			}
			set
			{
				GraphicsSettings.graphicsSettingsData.IsVSyncEnabled = value;
			}
		}

		public static EAntiAliasingType antiAliasingType
		{
			get
			{
				return GraphicsSettings.graphicsSettingsData.AntiAliasingType5;
			}
			set
			{
				GraphicsSettings.graphicsSettingsData.AntiAliasingType5 = value;
			}
		}

		public static EAnisotropicFilteringMode anisotropicFilteringMode
		{
			get
			{
				return GraphicsSettings.graphicsSettingsData.AnisotropicFilteringMode;
			}
			set
			{
				GraphicsSettings.graphicsSettingsData.AnisotropicFilteringMode = value;
			}
		}

		public static bool bloom
		{
			get
			{
				return GraphicsSettings.graphicsSettingsData.IsBloomEnabled;
			}
			set
			{
				GraphicsSettings.graphicsSettingsData.IsBloomEnabled = value;
			}
		}

		public static bool chromaticAberration
		{
			get
			{
				return GraphicsSettings.graphicsSettingsData.IsChromaticAberrationEnabled;
			}
			set
			{
				GraphicsSettings.graphicsSettingsData.IsChromaticAberrationEnabled = value;
			}
		}

		public static bool filmGrain
		{
			get
			{
				return GraphicsSettings.graphicsSettingsData.IsFilmGrainEnabled;
			}
			set
			{
				GraphicsSettings.graphicsSettingsData.IsFilmGrainEnabled = value;
			}
		}

		public static bool clouds
		{
			get
			{
				return !Level.isVR && GraphicsSettings.graphicsSettingsData.IsCloudEnabled;
			}
			set
			{
				GraphicsSettings.graphicsSettingsData.IsCloudEnabled = value;
			}
		}

		public static bool blend
		{
			get
			{
				return GraphicsSettings.graphicsSettingsData.IsNiceBlendEnabled;
			}
			set
			{
				GraphicsSettings.graphicsSettingsData.IsNiceBlendEnabled = value;
			}
		}

		public static bool fog
		{
			get
			{
				return GraphicsSettings.graphicsSettingsData.IsFogEnabled;
			}
			set
			{
				GraphicsSettings.graphicsSettingsData.IsFogEnabled = value;
			}
		}

		public static bool grassDisplacement
		{
			get
			{
				return GraphicsSettings.graphicsSettingsData.IsGrassDisplacementEnabled;
			}
			set
			{
				GraphicsSettings.graphicsSettingsData.IsGrassDisplacementEnabled = value;
			}
		}

		public static bool foliageFocus
		{
			get
			{
				return GraphicsSettings.graphicsSettingsData.IsFoliageFocusEnabled;
			}
			set
			{
				GraphicsSettings.graphicsSettingsData.IsFoliageFocusEnabled = value;
			}
		}

		public static EGraphicQuality landmarkQuality
		{
			get
			{
				return GraphicsSettings.graphicsSettingsData.LandmarkQuality;
			}
			set
			{
				GraphicsSettings.graphicsSettingsData.LandmarkQuality = value;
			}
		}

		public static bool ragdolls
		{
			get
			{
				return GraphicsSettings.graphicsSettingsData.IsRagdollsEnabled;
			}
			set
			{
				GraphicsSettings.graphicsSettingsData.IsRagdollsEnabled = value;
			}
		}

		public static bool debris
		{
			get
			{
				return GraphicsSettings.graphicsSettingsData.IsDebrisEnabled;
			}
			set
			{
				GraphicsSettings.graphicsSettingsData.IsDebrisEnabled = value;
			}
		}

		public static bool blast
		{
			get
			{
				return GraphicsSettings.graphicsSettingsData.IsBlastEnabled;
			}
			set
			{
				GraphicsSettings.graphicsSettingsData.IsBlastEnabled = value;
			}
		}

		public static bool puddle
		{
			get
			{
				return GraphicsSettings.graphicsSettingsData.IsPuddleEnabled;
			}
			set
			{
				GraphicsSettings.graphicsSettingsData.IsPuddleEnabled = value;
			}
		}

		public static bool glitter
		{
			get
			{
				return GraphicsSettings.graphicsSettingsData.IsGlitterEnabled;
			}
			set
			{
				GraphicsSettings.graphicsSettingsData.IsGlitterEnabled = value;
			}
		}

		public static bool triplanar
		{
			get
			{
				return GraphicsSettings.graphicsSettingsData.IsTriplanarMappingEnabled;
			}
			set
			{
				GraphicsSettings.graphicsSettingsData.IsTriplanarMappingEnabled = value;
			}
		}

		public static bool skyboxReflection
		{
			get
			{
				return GraphicsSettings.graphicsSettingsData.IsSkyboxReflectionEnabled;
			}
			set
			{
				GraphicsSettings.graphicsSettingsData.IsSkyboxReflectionEnabled = value;
			}
		}

		public static float distance
		{
			get
			{
				return GraphicsSettings.graphicsSettingsData.DrawDistance;
			}
			set
			{
				GraphicsSettings.graphicsSettingsData.DrawDistance = value;
			}
		}

		public static float landmarkDistance
		{
			get
			{
				return GraphicsSettings.graphicsSettingsData.LandmarkDistance;
			}
			set
			{
				GraphicsSettings.graphicsSettingsData.LandmarkDistance = value;
			}
		}

		public static EGraphicQuality effectQuality
		{
			get
			{
				return GraphicsSettings.graphicsSettingsData.EffectQuality;
			}
			set
			{
				GraphicsSettings.graphicsSettingsData.EffectQuality = value;
			}
		}

		public static EGraphicQuality foliageQuality
		{
			get
			{
				if (Level.isVR)
				{
					return EGraphicQuality.OFF;
				}
				return GraphicsSettings.graphicsSettingsData.FoliageQuality2;
			}
			set
			{
				GraphicsSettings.graphicsSettingsData.FoliageQuality2 = value;
			}
		}

		public static EGraphicQuality sunShaftsQuality
		{
			get
			{
				return GraphicsSettings.graphicsSettingsData.SunShaftsQuality;
			}
			set
			{
				GraphicsSettings.graphicsSettingsData.SunShaftsQuality = value;
			}
		}

		public static EGraphicQuality lightingQuality
		{
			get
			{
				if (Level.isVR && GraphicsSettings.graphicsSettingsData.LightingQuality == EGraphicQuality.ULTRA)
				{
					return EGraphicQuality.HIGH;
				}
				return GraphicsSettings.graphicsSettingsData.LightingQuality;
			}
			set
			{
				GraphicsSettings.graphicsSettingsData.LightingQuality = value;
			}
		}

		public static EGraphicQuality ambientOcclusionQuality
		{
			get
			{
				return GraphicsSettings.graphicsSettingsData.ScreenSpaceAmbientOcclusionQuality;
			}
			set
			{
				GraphicsSettings.graphicsSettingsData.ScreenSpaceAmbientOcclusionQuality = value;
			}
		}

		public static EGraphicQuality reflectionQuality
		{
			get
			{
				return GraphicsSettings.graphicsSettingsData.ScreenSpaceReflectionQuality;
			}
			set
			{
				GraphicsSettings.graphicsSettingsData.ScreenSpaceReflectionQuality = value;
			}
		}

		public static EGraphicQuality planarReflectionQuality
		{
			get
			{
				return GraphicsSettings.graphicsSettingsData.PlanarReflectionQuality;
			}
			set
			{
				GraphicsSettings.graphicsSettingsData.PlanarReflectionQuality = value;
			}
		}

		public static EGraphicQuality waterQuality
		{
			get
			{
				if (Level.isVR && GraphicsSettings.graphicsSettingsData.WaterQuality == EGraphicQuality.ULTRA)
				{
					return EGraphicQuality.HIGH;
				}
				return GraphicsSettings.graphicsSettingsData.WaterQuality;
			}
			set
			{
				GraphicsSettings.graphicsSettingsData.WaterQuality = value;
			}
		}

		public static EGraphicQuality scopeQuality
		{
			get
			{
				return GraphicsSettings.graphicsSettingsData.ScopeQuality2;
			}
			set
			{
				GraphicsSettings.graphicsSettingsData.ScopeQuality2 = value;
			}
		}

		public static EGraphicQuality outlineQuality
		{
			get
			{
				return GraphicsSettings.graphicsSettingsData.OutlineQuality;
			}
			set
			{
				GraphicsSettings.graphicsSettingsData.OutlineQuality = value;
			}
		}

		public static EGraphicQuality boneQuality
		{
			get
			{
				return GraphicsSettings.graphicsSettingsData.BoneQuality;
			}
			set
			{
				GraphicsSettings.graphicsSettingsData.BoneQuality = value;
			}
		}

		public static EGraphicQuality terrainQuality
		{
			get
			{
				return GraphicsSettings.graphicsSettingsData.TerrainQuality;
			}
			set
			{
				GraphicsSettings.graphicsSettingsData.TerrainQuality = value;
			}
		}

		public static EGraphicQuality windQuality
		{
			get
			{
				return GraphicsSettings.graphicsSettingsData.WindQuality;
			}
			set
			{
				GraphicsSettings.graphicsSettingsData.WindQuality = value;
			}
		}

		public static ETreeGraphicMode treeMode
		{
			get
			{
				return GraphicsSettings.graphicsSettingsData.TreeMode;
			}
			set
			{
				GraphicsSettings.graphicsSettingsData.TreeMode = value;
			}
		}

		public static ERenderMode renderMode
		{
			get
			{
				return GraphicsSettings.graphicsSettingsData.RenderMode2;
			}
			set
			{
				GraphicsSettings.graphicsSettingsData.RenderMode2 = value;
			}
		}

		public static event GraphicsSettingsApplied graphicsSettingsApplied;

		public static void resize()
		{
			if (Application.isEditor)
			{
				return;
			}
			float num = (float)GraphicsSettings.resolution.Width / (float)GraphicsSettings.resolution.Height;
			if (num - 0.01f > GraphicsSettings.MAX_ASPECT_RATIO)
			{
				GraphicsSettings.resolution.Width = (int)((float)GraphicsSettings.resolution.Height * GraphicsSettings.MAX_ASPECT_RATIO);
			}
			if (GraphicsSettings.resolution.Width < 640 || GraphicsSettings.resolution.Height < 480)
			{
				GraphicsSettings.resolution = new GraphicsSettingsResolution(Screen.resolutions[Screen.resolutions.Length - 1]);
			}
			else if (GraphicsSettings.resolution.Width < Screen.resolutions[0].width || GraphicsSettings.resolution.Height < Screen.resolutions[0].height)
			{
				GraphicsSettings.resolution = new GraphicsSettingsResolution(Screen.resolutions[0]);
			}
			else if (GraphicsSettings.resolution.Width > Screen.resolutions[Screen.resolutions.Length - 1].width || GraphicsSettings.resolution.Height > Screen.resolutions[Screen.resolutions.Length - 1].height)
			{
				GraphicsSettings.resolution = new GraphicsSettingsResolution(Screen.resolutions[0]);
			}
			Screen.SetResolution(GraphicsSettings.resolution.Width, GraphicsSettings.resolution.Height, GraphicsSettings.fullscreen);
		}

		public static void apply()
		{
			if (GraphicsSettings.changeResolution)
			{
				GraphicsSettings.changeResolution = false;
				if (!Application.isEditor)
				{
					if (Provider.isConnected)
					{
						PlayerUI.rebuild();
					}
					else
					{
						MenuUI.rebuild();
					}
				}
			}
			if (LevelLighting.sun != null)
			{
				if (GraphicsSettings.lightingQuality == EGraphicQuality.ULTRA || GraphicsSettings.lightingQuality == EGraphicQuality.HIGH)
				{
					LevelLighting.sun.GetComponent<Light>().shadowNormalBias = 0f;
				}
				else
				{
					LevelLighting.sun.GetComponent<Light>().shadowNormalBias = 0.5f;
				}
			}
			QualitySettings.SetQualityLevel((int)((byte)GraphicsSettings.lightingQuality + 1), true);
			QualitySettings.vSyncCount = ((!GraphicsSettings.buffer) ? 0 : 1);
			EAnisotropicFilteringMode anisotropicFilteringMode = GraphicsSettings.anisotropicFilteringMode;
			if (anisotropicFilteringMode != EAnisotropicFilteringMode.DISABLED)
			{
				if (anisotropicFilteringMode != EAnisotropicFilteringMode.PER_TEXTURE)
				{
					if (anisotropicFilteringMode == EAnisotropicFilteringMode.FORCED_ON)
					{
						QualitySettings.anisotropicFiltering = 2;
					}
				}
				else
				{
					QualitySettings.anisotropicFiltering = 1;
				}
			}
			else
			{
				QualitySettings.anisotropicFiltering = 0;
			}
			float[] array = new float[32];
			array[LayerMasks.DEFAULT] = 0f;
			array[LayerMasks.TRANSPARENT_FX] = 0f;
			array[LayerMasks.IGNORE_RAYCAST] = 0f;
			array[3] = 0f;
			array[LayerMasks.WATER] = 4096f;
			array[LayerMasks.UI] = 0f;
			array[6] = 0f;
			array[7] = 0f;
			array[LayerMasks.LOGIC] = ((!Level.isEditor) ? 0f : (256f + GraphicsSettings.distance * 256f));
			array[LayerMasks.PLAYER] = 0f;
			array[LayerMasks.ENEMY] = 512f;
			array[LayerMasks.VIEWMODEL] = 0f;
			array[LayerMasks.DEBRIS] = 256f + GraphicsSettings.distance * 256f;
			array[LayerMasks.ITEM] = 32f + GraphicsSettings.distance * 32f;
			array[LayerMasks.RESOURCE] = 256f + GraphicsSettings.distance * 256f;
			array[LayerMasks.LARGE] = 256f + GraphicsSettings.distance * 256f;
			array[LayerMasks.MEDIUM] = 128f + GraphicsSettings.distance * 128f;
			array[LayerMasks.SMALL] = 32f + GraphicsSettings.distance * 32f;
			array[LayerMasks.SKY] = 16384f;
			array[LayerMasks.ENVIRONMENT] = 256f + GraphicsSettings.distance * 256f;
			array[LayerMasks.GROUND] = 4096f;
			array[LayerMasks.CLIP] = 0f;
			array[LayerMasks.NAVMESH] = ((!Level.isEditor) ? 0f : (256f + GraphicsSettings.distance * 256f));
			array[LayerMasks.ENTITY] = 256f + GraphicsSettings.distance * 256f;
			array[LayerMasks.AGENT] = 0f;
			array[LayerMasks.LADDER] = 0f;
			array[LayerMasks.VEHICLE] = 256f + GraphicsSettings.distance * 256f;
			array[LayerMasks.BARRICADE] = 256f + GraphicsSettings.distance * 256f;
			array[LayerMasks.STRUCTURE] = 256f + GraphicsSettings.distance * 256f;
			array[LayerMasks.TIRE] = 0f;
			array[LayerMasks.TRAP] = 256f + GraphicsSettings.distance * 256f;
			array[LayerMasks.GROUND2] = 4096f;
			if (GraphicsSettings.landmarkQuality >= EGraphicQuality.LOW)
			{
				if (GraphicsSettings.uncapLandmarks)
				{
					array[LayerMasks.LARGE] = 4096f;
				}
				else
				{
					array[LayerMasks.LARGE] += GraphicsSettings.landmarkDistance * 1536f;
				}
			}
			if (GraphicsSettings.landmarkQuality >= EGraphicQuality.MEDIUM)
			{
				if (GraphicsSettings.uncapLandmarks)
				{
					array[LayerMasks.RESOURCE] = 4096f;
				}
				else
				{
					array[LayerMasks.RESOURCE] += GraphicsSettings.landmarkDistance * 1536f;
				}
			}
			if (GraphicsSettings.landmarkQuality >= EGraphicQuality.ULTRA)
			{
				if (GraphicsSettings.uncapLandmarks)
				{
					array[LayerMasks.ENVIRONMENT] = 4096f;
				}
				else
				{
					array[LayerMasks.ENVIRONMENT] += GraphicsSettings.landmarkDistance * 1536f;
				}
			}
			if (!LevelObjects.shouldInstantlyLoad && !LevelGround.shouldInstantlyLoad)
			{
				for (byte b = 0; b < Regions.WORLD_SIZE; b += 1)
				{
					for (byte b2 = 0; b2 < Regions.WORLD_SIZE; b2 += 1)
					{
						if (LevelObjects.regions != null && !LevelObjects.regions[(int)b, (int)b2])
						{
							List<LevelObject> list = LevelObjects.objects[(int)b, (int)b2];
							for (int i = 0; i < list.Count; i++)
							{
								LevelObject levelObject = list[i];
								if (levelObject != null)
								{
									if (levelObject.isLandmarkQualityMet)
									{
										levelObject.enableSkybox();
									}
									else
									{
										levelObject.disableSkybox();
									}
								}
							}
						}
						if (LevelGround.regions != null && !LevelGround.regions[(int)b, (int)b2])
						{
							List<ResourceSpawnpoint> list2 = LevelGround.trees[(int)b, (int)b2];
							for (int j = 0; j < list2.Count; j++)
							{
								ResourceSpawnpoint resourceSpawnpoint = list2[j];
								if (resourceSpawnpoint != null)
								{
									if (GraphicsSettings.landmarkQuality >= EGraphicQuality.MEDIUM)
									{
										resourceSpawnpoint.enableSkybox();
									}
									else
									{
										resourceSpawnpoint.disableSkybox();
									}
								}
							}
						}
					}
				}
			}
			QualitySettings.lodBias = 1f + GraphicsSettings.distance * 3f;
			switch (GraphicsSettings.boneQuality)
			{
			case EGraphicQuality.LOW:
				QualitySettings.blendWeights = 1;
				break;
			case EGraphicQuality.MEDIUM:
				QualitySettings.blendWeights = 2;
				break;
			case EGraphicQuality.HIGH:
				QualitySettings.blendWeights = 4;
				break;
			default:
				QualitySettings.blendWeights = 1;
				break;
			}
			if (MainCamera.instance != null)
			{
				MainCamera.instance.renderingPath = ((GraphicsSettings.renderMode != ERenderMode.DEFERRED) ? 1 : 3);
				MainCamera.instance.hdr = (GraphicsSettings.renderMode == ERenderMode.DEFERRED);
				GlobalFog2 component = MainCamera.instance.GetComponent<GlobalFog2>();
				if (component != null)
				{
					component.enabled = (GraphicsSettings.renderMode == ERenderMode.DEFERRED);
				}
				AntialiasingModel.Settings settings = GraphicsSettings.mainProfile.antialiasing.settings;
				EAntiAliasingType antiAliasingType = GraphicsSettings.antiAliasingType;
				if (antiAliasingType != EAntiAliasingType.FXAA)
				{
					if (antiAliasingType != EAntiAliasingType.TAA)
					{
						GraphicsSettings.mainProfile.antialiasing.enabled = false;
						QualitySettings.antiAliasing = 0;
					}
					else
					{
						GraphicsSettings.mainProfile.antialiasing.enabled = true;
						settings.method = AntialiasingModel.Method.Taa;
						QualitySettings.antiAliasing = ((GraphicsSettings.renderMode != ERenderMode.DEFERRED) ? 0 : 8);
					}
				}
				else
				{
					GraphicsSettings.mainProfile.antialiasing.enabled = true;
					settings.method = AntialiasingModel.Method.Fxaa;
					QualitySettings.antiAliasing = ((GraphicsSettings.renderMode != ERenderMode.DEFERRED) ? 0 : 4);
				}
				GraphicsSettings.mainProfile.antialiasing.settings = settings;
				AmbientOcclusionModel.Settings settings2 = GraphicsSettings.mainProfile.ambientOcclusion.settings;
				GraphicsSettings.mainProfile.ambientOcclusion.enabled = (GraphicsSettings.renderMode == ERenderMode.DEFERRED && GraphicsSettings.ambientOcclusionQuality != EGraphicQuality.OFF);
				switch (GraphicsSettings.ambientOcclusionQuality)
				{
				case EGraphicQuality.LOW:
					settings2.sampleCount = AmbientOcclusionModel.SampleCount.Lowest;
					break;
				case EGraphicQuality.MEDIUM:
					settings2.sampleCount = AmbientOcclusionModel.SampleCount.Low;
					break;
				case EGraphicQuality.HIGH:
					settings2.sampleCount = AmbientOcclusionModel.SampleCount.Medium;
					break;
				case EGraphicQuality.ULTRA:
					settings2.sampleCount = AmbientOcclusionModel.SampleCount.High;
					break;
				}
				GraphicsSettings.mainProfile.ambientOcclusion.settings = settings2;
				ScreenSpaceReflectionModel.Settings settings3 = GraphicsSettings.mainProfile.screenSpaceReflection.settings;
				GraphicsSettings.mainProfile.screenSpaceReflection.enabled = (GraphicsSettings.reflectionQuality != EGraphicQuality.OFF);
				switch (GraphicsSettings.reflectionQuality)
				{
				case EGraphicQuality.LOW:
					settings3.reflection.reflectionQuality = ScreenSpaceReflectionModel.SSRResolution.Low;
					settings3.reflection.iterationCount = 100;
					settings3.reflection.stepSize = 4;
					break;
				case EGraphicQuality.MEDIUM:
					settings3.reflection.reflectionQuality = ScreenSpaceReflectionModel.SSRResolution.Low;
					settings3.reflection.iterationCount = 150;
					settings3.reflection.stepSize = 3;
					break;
				case EGraphicQuality.HIGH:
					settings3.reflection.reflectionQuality = ScreenSpaceReflectionModel.SSRResolution.High;
					settings3.reflection.iterationCount = 250;
					settings3.reflection.stepSize = 2;
					break;
				case EGraphicQuality.ULTRA:
					settings3.reflection.reflectionQuality = ScreenSpaceReflectionModel.SSRResolution.High;
					settings3.reflection.iterationCount = 600;
					settings3.reflection.stepSize = 1;
					break;
				}
				GraphicsSettings.mainProfile.screenSpaceReflection.settings = settings3;
				GraphicsSettings.mainProfile.bloom.enabled = GraphicsSettings.bloom;
				BloomModel.Settings settings4 = GraphicsSettings.mainProfile.bloom.settings;
				settings4.lensDirt.intensity = (float)((!Provider.preferenceData.Graphics.Use_Lens_Dirt) ? 0 : 3);
				GraphicsSettings.mainProfile.bloom.settings = settings4;
				SunShafts component2 = MainCamera.instance.GetComponent<SunShafts>();
				if (component2 != null)
				{
					if (GraphicsSettings.sunShaftsQuality == EGraphicQuality.LOW)
					{
						component2.resolution = 0;
					}
					else if (GraphicsSettings.sunShaftsQuality == EGraphicQuality.MEDIUM)
					{
						component2.resolution = 1;
					}
					else if (GraphicsSettings.sunShaftsQuality == EGraphicQuality.HIGH)
					{
						component2.resolution = 2;
					}
					if (LevelLighting.areFXAllowed)
					{
						component2.enabled = (GraphicsSettings.sunShaftsQuality != EGraphicQuality.OFF);
					}
				}
				if (LevelLighting.areFXAllowed)
				{
					GlobalFog component3 = MainCamera.instance.GetComponent<GlobalFog>();
					if (component3 != null)
					{
						component3.enabled = GraphicsSettings.fog;
					}
				}
				HighlightingRenderer component4 = MainCamera.instance.GetComponent<HighlightingRenderer>();
				if (component4 != null)
				{
					if (Level.isDevkit)
					{
						component4.downsampleFactor = 1;
						component4.iterations = 1;
						component4.blurMinSpread = 1f;
						component4.blurSpread = 1f;
						component4.blurIntensity = 1f;
					}
					else if (GraphicsSettings.outlineQuality == EGraphicQuality.LOW)
					{
						component4.downsampleFactor = 4;
						component4.iterations = 1;
						component4.blurMinSpread = 0.75f;
						component4.blurSpread = 0f;
						component4.blurIntensity = 0.25f;
					}
					else if (GraphicsSettings.outlineQuality == EGraphicQuality.MEDIUM)
					{
						component4.downsampleFactor = 4;
						component4.iterations = 2;
						component4.blurMinSpread = 0.5f;
						component4.blurSpread = 0.25f;
						component4.blurIntensity = 0.25f;
					}
					else if (GraphicsSettings.outlineQuality == EGraphicQuality.HIGH)
					{
						component4.downsampleFactor = 2;
						component4.iterations = 2;
						component4.blurMinSpread = 1f;
						component4.blurSpread = 0.5f;
						component4.blurIntensity = 0.25f;
					}
					else if (GraphicsSettings.outlineQuality == EGraphicQuality.ULTRA)
					{
						component4.downsampleFactor = 1;
						component4.iterations = 3;
						component4.blurMinSpread = 0.5f;
						component4.blurSpread = 0.5f;
						component4.blurIntensity = 0.25f;
					}
				}
				MainCamera.instance.layerCullDistances = array;
				MainCamera.instance.layerCullSpherical = true;
				if (Player.player != null)
				{
					Player.player.look.scopeCamera.layerCullDistances = array;
					Player.player.look.scopeCamera.layerCullSpherical = true;
					Player.player.look.scopeCamera.depthTextureMode = 1;
					Player.player.look.updateScope(GraphicsSettings.scopeQuality);
					Player.player.look.scopeCamera.GetComponent<GlobalFog>().enabled = GraphicsSettings.fog;
					Player.player.look.scopeCamera.renderingPath = ((GraphicsSettings.renderMode != ERenderMode.DEFERRED) ? 1 : 3);
					Player.player.look.scopeCamera.hdr = (GraphicsSettings.renderMode == ERenderMode.DEFERRED);
					component = Player.player.look.scopeCamera.GetComponent<GlobalFog2>();
					if (component != null)
					{
						component.enabled = (GraphicsSettings.renderMode == ERenderMode.DEFERRED);
					}
					if (LevelLighting.areFXAllowed)
					{
						GlobalFog component5 = Player.player.look.scopeCamera.GetComponent<GlobalFog>();
						if (component5 != null)
						{
							component5.enabled = GraphicsSettings.fog;
						}
					}
					GraphicsSettings.mainProfile.chromaticAberration.enabled = (Player.player.look.perspective == EPlayerPerspective.THIRD && GraphicsSettings.chromaticAberration);
					GraphicsSettings.mainProfile.grain.enabled = (Player.player.look.perspective == EPlayerPerspective.THIRD && GraphicsSettings.filmGrain);
					GraphicsSettings.viewProfile.chromaticAberration.enabled = (Player.player.look.perspective == EPlayerPerspective.FIRST && GraphicsSettings.chromaticAberration);
					GraphicsSettings.viewProfile.grain.enabled = (Player.player.look.perspective == EPlayerPerspective.FIRST && GraphicsSettings.filmGrain);
				}
				else
				{
					GraphicsSettings.mainProfile.chromaticAberration.enabled = GraphicsSettings.chromaticAberration;
					GraphicsSettings.mainProfile.grain.enabled = GraphicsSettings.filmGrain;
					GraphicsSettings.viewProfile.chromaticAberration.enabled = false;
					GraphicsSettings.viewProfile.grain.enabled = false;
				}
			}
			if (LevelGround.terrain != null)
			{
				Terrain terrain = LevelGround.terrain;
				Terrain terrain2 = LevelGround.terrain2;
				if (GraphicsSettings.blend)
				{
					ERenderMode renderMode = GraphicsSettings.renderMode;
					if (renderMode != ERenderMode.FORWARD)
					{
						if (renderMode != ERenderMode.DEFERRED)
						{
							terrain.materialTemplate = null;
							terrain2.materialTemplate = null;
							Debug.LogError("Unknown render mode: " + GraphicsSettings.renderMode);
						}
						else
						{
							terrain.materialTemplate = Resources.Load<Material>("Materials/Landscapes/Landscape_Deferred");
							terrain2.materialTemplate = Resources.Load<Material>("Materials/Landscapes/Landscape_Deferred");
						}
					}
					else
					{
						terrain.materialTemplate = Resources.Load<Material>("Materials/Landscapes/Landscape_Forward");
						terrain2.materialTemplate = Resources.Load<Material>("Materials/Landscapes/Landscape_Forward");
					}
					terrain.basemapDistance = 512f;
					terrain2.basemapDistance = 512f;
				}
				else
				{
					terrain.materialTemplate = Resources.Load<Material>("Materials/Landscapes/Landscape_Classic");
					terrain2.materialTemplate = Resources.Load<Material>("Materials/Landscapes/Landscape_Classic");
					terrain.basemapDistance = 256f;
					terrain2.basemapDistance = 256f;
				}
				switch (GraphicsSettings.terrainQuality)
				{
				case EGraphicQuality.LOW:
					terrain.heightmapPixelError = 64f;
					break;
				case EGraphicQuality.MEDIUM:
					terrain.heightmapPixelError = 32f;
					break;
				case EGraphicQuality.HIGH:
					terrain.heightmapPixelError = 16f;
					break;
				case EGraphicQuality.ULTRA:
					terrain.heightmapPixelError = 8f;
					break;
				}
				terrain2.heightmapPixelError = terrain.heightmapPixelError * 2f;
				if (GraphicsSettings.foliageQuality == EGraphicQuality.OFF)
				{
					terrain.detailObjectDensity = 0f;
					terrain.detailObjectDistance = 0f;
				}
				else if (GraphicsSettings.foliageQuality == EGraphicQuality.LOW)
				{
					terrain.detailObjectDensity = 0.25f;
					terrain.detailObjectDistance = 60f;
				}
				else if (GraphicsSettings.foliageQuality == EGraphicQuality.MEDIUM)
				{
					terrain.detailObjectDensity = 0.5f;
					terrain.detailObjectDistance = 120f;
				}
				else if (GraphicsSettings.foliageQuality == EGraphicQuality.HIGH)
				{
					terrain.detailObjectDensity = 0.75f;
					terrain.detailObjectDistance = 180f;
				}
				else if (GraphicsSettings.foliageQuality == EGraphicQuality.ULTRA)
				{
					terrain.detailObjectDensity = 1f;
					terrain.detailObjectDistance = 250f;
				}
				terrain.terrainData.wavingGrassAmount = 0.25f;
				terrain.terrainData.wavingGrassSpeed = 0.5f;
				terrain.terrainData.wavingGrassStrength = 1f;
			}
			switch (GraphicsSettings.foliageQuality)
			{
			case EGraphicQuality.OFF:
				FoliageSettings.enabled = false;
				FoliageSettings.drawDistance = 0;
				FoliageSettings.instanceDensity = 0f;
				FoliageSettings.drawFocusDistance = 0;
				FoliageSettings.focusDistance = 0f;
				break;
			case EGraphicQuality.LOW:
				FoliageSettings.enabled = true;
				FoliageSettings.drawDistance = 2;
				FoliageSettings.instanceDensity = 0.25f;
				FoliageSettings.drawFocusDistance = 1;
				FoliageSettings.focusDistance = 1024f;
				break;
			case EGraphicQuality.MEDIUM:
				FoliageSettings.enabled = true;
				FoliageSettings.drawDistance = 3;
				FoliageSettings.instanceDensity = 0.5f;
				FoliageSettings.drawFocusDistance = 2;
				FoliageSettings.focusDistance = 1024f;
				break;
			case EGraphicQuality.HIGH:
				FoliageSettings.enabled = true;
				FoliageSettings.drawDistance = 4;
				FoliageSettings.instanceDensity = 0.75f;
				FoliageSettings.drawFocusDistance = 3;
				FoliageSettings.focusDistance = 2048f;
				break;
			case EGraphicQuality.ULTRA:
				FoliageSettings.enabled = true;
				FoliageSettings.drawDistance = 5;
				FoliageSettings.instanceDensity = 1f;
				FoliageSettings.drawFocusDistance = 4;
				FoliageSettings.focusDistance = 2048f;
				break;
			default:
				FoliageSettings.enabled = false;
				FoliageSettings.drawDistance = 0;
				FoliageSettings.instanceDensity = 0f;
				FoliageSettings.drawFocusDistance = 0;
				FoliageSettings.focusDistance = 0f;
				Debug.LogError("Unknown foliage quality: " + GraphicsSettings.foliageQuality);
				break;
			}
			FoliageSettings.drawFocus = GraphicsSettings.foliageFocus;
			if (LevelLighting.sea != null)
			{
				Material sea = LevelLighting.sea;
				PlanarReflection reflection = LevelLighting.reflection;
				if (sea != null && reflection != null)
				{
					if (GraphicsSettings.waterQuality == EGraphicQuality.LOW)
					{
						sea.shader.maximumLOD = 201;
						Shader.EnableKeyword("WATER_EDGEBLEND_OFF");
						Shader.DisableKeyword("WATER_EDGEBLEND_ON");
						Shader.DisableKeyword("WATER_REFLECTIVE");
						Shader.EnableKeyword("WATER_SIMPLE");
						reflection.enabled = false;
					}
					else if (GraphicsSettings.waterQuality == EGraphicQuality.MEDIUM)
					{
						sea.shader.maximumLOD = 301;
						Shader.EnableKeyword("WATER_EDGEBLEND_OFF");
						Shader.DisableKeyword("WATER_EDGEBLEND_ON");
						Shader.DisableKeyword("WATER_REFLECTIVE");
						Shader.EnableKeyword("WATER_SIMPLE");
						reflection.enabled = false;
					}
					else if (GraphicsSettings.waterQuality == EGraphicQuality.HIGH)
					{
						sea.shader.maximumLOD = 501;
						if (SystemInfo.SupportsRenderTextureFormat(1))
						{
							Shader.EnableKeyword("WATER_EDGEBLEND_ON");
							Shader.DisableKeyword("WATER_EDGEBLEND_OFF");
						}
						else
						{
							Shader.EnableKeyword("WATER_EDGEBLEND_OFF");
							Shader.DisableKeyword("WATER_EDGEBLEND_ON");
						}
						Shader.DisableKeyword("WATER_REFLECTIVE");
						Shader.EnableKeyword("WATER_SIMPLE");
						reflection.enabled = false;
					}
					else if (GraphicsSettings.waterQuality == EGraphicQuality.ULTRA)
					{
						sea.shader.maximumLOD = 501;
						if (SystemInfo.SupportsRenderTextureFormat(1))
						{
							Shader.EnableKeyword("WATER_EDGEBLEND_ON");
							Shader.DisableKeyword("WATER_EDGEBLEND_OFF");
						}
						else
						{
							Shader.EnableKeyword("WATER_EDGEBLEND_OFF");
							Shader.DisableKeyword("WATER_EDGEBLEND_ON");
						}
						reflection.enabled = true;
					}
				}
			}
			if (LevelLighting.clouds != null)
			{
				LevelLighting.clouds.gameObject.SetActive(GraphicsSettings.clouds);
				LevelLighting.clouds.GetComponent<ParticleSystem>().Play();
			}
			if (LevelLighting.rain != null)
			{
				LevelLighting.rain.GetComponent<ParticleSystem>().collision.collidesWith = ((!GraphicsSettings.puddle) ? (RayMasks.LARGE | RayMasks.STRUCTURE) : (RayMasks.LARGE | RayMasks.STRUCTURE | RayMasks.GROUND));
				LevelLighting.rain.GetComponent<ParticleSystem>().subEmitters.enabled = GraphicsSettings.puddle;
			}
			LevelLighting.isSkyboxReflectionEnabled = GraphicsSettings.skyboxReflection;
			if (GraphicsSettings.windQuality > EGraphicQuality.OFF)
			{
				Shader.EnableKeyword("NICE_FOLIAGE_ON");
				Shader.EnableKeyword("GRASS_WIND_ON");
			}
			else
			{
				Shader.DisableKeyword("NICE_FOLIAGE_ON");
				Shader.DisableKeyword("GRASS_WIND_ON");
			}
			if (GraphicsSettings.windQuality > EGraphicQuality.LOW)
			{
				Shader.EnableKeyword("ENABLE_WIND");
			}
			else
			{
				Shader.DisableKeyword("ENABLE_WIND");
			}
			switch (GraphicsSettings.windQuality)
			{
			case EGraphicQuality.OFF:
				Shader.SetGlobalInt("_MaxWindQuality", 0);
				break;
			case EGraphicQuality.LOW:
				Shader.SetGlobalInt("_MaxWindQuality", 1);
				break;
			case EGraphicQuality.MEDIUM:
				Shader.SetGlobalInt("_MaxWindQuality", 2);
				break;
			case EGraphicQuality.HIGH:
				Shader.SetGlobalInt("_MaxWindQuality", 3);
				break;
			case EGraphicQuality.ULTRA:
				Shader.SetGlobalInt("_MaxWindQuality", 4);
				break;
			}
			if (GraphicsSettings.grassDisplacement)
			{
				Shader.EnableKeyword("GRASS_DISPLACEMENT_ON");
			}
			else
			{
				Shader.DisableKeyword("GRASS_DISPLACEMENT_ON");
			}
			if (Level.info != null && Level.info.configData != null && Level.info.configData.Terrain_Snow_Sparkle && GraphicsSettings.glitter)
			{
				Shader.EnableKeyword("IS_SNOWING");
			}
			else
			{
				Shader.DisableKeyword("IS_SNOWING");
			}
			if (GraphicsSettings.triplanar)
			{
				Shader.EnableKeyword("TRIPLANAR_MAPPING_ON");
			}
			else
			{
				Shader.DisableKeyword("TRIPLANAR_MAPPING_ON");
			}
			GraphicsSettings.planarReflectionNeedsUpdate = true;
			if (GraphicsSettings.graphicsSettingsApplied != null)
			{
				GraphicsSettings.graphicsSettingsApplied();
			}
		}

		public static void restoreDefaults()
		{
			bool isFullscreenEnabled = false;
			bool isVSyncEnabled = false;
			GraphicsSettingsResolution resolution = new GraphicsSettingsResolution();
			if (GraphicsSettings.graphicsSettingsData != null)
			{
				isFullscreenEnabled = GraphicsSettings.graphicsSettingsData.IsFullscreenEnabled;
				isVSyncEnabled = GraphicsSettings.graphicsSettingsData.IsVSyncEnabled;
				resolution = GraphicsSettings.graphicsSettingsData.Resolution;
			}
			GraphicsSettings.graphicsSettingsData = new GraphicsSettingsData();
			GraphicsSettings.graphicsSettingsData.IsFullscreenEnabled = isFullscreenEnabled;
			GraphicsSettings.graphicsSettingsData.IsVSyncEnabled = isVSyncEnabled;
			GraphicsSettings.graphicsSettingsData.Resolution = resolution;
			GraphicsSettings.apply();
		}

		public static void load()
		{
			if (GraphicsSettings.mainProfile == null)
			{
				GraphicsSettings.mainProfile = (PostProcessingProfile)Resources.Load("Profiles/Profile_Main");
			}
			if (GraphicsSettings.viewProfile == null)
			{
				GraphicsSettings.viewProfile = (PostProcessingProfile)Resources.Load("Profiles/Profile_View");
			}
			if (ReadWrite.fileExists("/Settings/Graphics.json", true))
			{
				try
				{
					GraphicsSettings.graphicsSettingsData = ReadWrite.deserializeJSON<GraphicsSettingsData>("/Settings/Graphics.json", true);
				}
				catch
				{
					GraphicsSettings.graphicsSettingsData = null;
				}
				if (GraphicsSettings.graphicsSettingsData == null)
				{
					GraphicsSettings.restoreDefaults();
				}
			}
			else
			{
				GraphicsSettings.restoreDefaults();
			}
			if (GraphicsSettings.graphicsSettingsData.EffectQuality == EGraphicQuality.OFF)
			{
				GraphicsSettings.graphicsSettingsData.EffectQuality = EGraphicQuality.MEDIUM;
			}
			if (!Application.isEditor && (GraphicsSettings.resolution.Width > Screen.resolutions[Screen.resolutions.Length - 1].width || GraphicsSettings.resolution.Height > Screen.resolutions[Screen.resolutions.Length - 1].height))
			{
				GraphicsSettings.resolution = new GraphicsSettingsResolution(Screen.resolutions[Screen.resolutions.Length - 1]);
			}
		}

		public static void save()
		{
			ReadWrite.serializeJSON<GraphicsSettingsData>("/Settings/Graphics.json", true, GraphicsSettings.graphicsSettingsData);
		}

		private static bool _uncapLandmarks;

		public static readonly float MAX_ASPECT_RATIO = 2.33333325f;

		private static readonly float EFFECT_ULTRA = 64f;

		private static readonly float EFFECT_HIGH = 48f;

		private static readonly float EFFECT_MEDIUM = 32f;

		private static readonly float EFFECT_LOW = 16f;

		public static bool planarReflectionNeedsUpdate;

		private static GraphicsSettingsData graphicsSettingsData;

		private static bool changeResolution;
	}
}
