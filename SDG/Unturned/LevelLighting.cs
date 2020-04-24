using System;
using SDG.Framework.Debug;
using SDG.Framework.Devkit;
using SDG.Framework.Landscapes;
using SDG.Framework.Water;
using UnityEngine;

namespace SDG.Unturned
{
	public class LevelLighting
	{
		public static bool enableUnderwaterEffects
		{
			get
			{
				return LevelLighting._enableUnderwaterEffects;
			}
			set
			{
				LevelLighting._enableUnderwaterEffects = value;
				TerminalUtility.printCommandPass("Set enable_underwater_effects to: " + LevelLighting.enableUnderwaterEffects);
			}
		}

		private static float fogHeight
		{
			get
			{
				if (Level.info.configData.Use_Legacy_Fog_Height)
				{
					return -128f;
				}
				return -Landscape.TILE_HEIGHT / 2f - 128f;
			}
		}

		private static float fogSize
		{
			get
			{
				if (Level.info.configData.Use_Legacy_Fog_Height)
				{
					return 128f;
				}
				return Landscape.TILE_HEIGHT;
			}
		}

		public static float azimuth
		{
			get
			{
				return LevelLighting._azimuth;
			}
			set
			{
				LevelLighting._azimuth = value;
				LevelLighting.updateLighting();
			}
		}

		public static float transition
		{
			get
			{
				return LevelLighting._transition;
			}
		}

		public static float bias
		{
			get
			{
				return LevelLighting._bias;
			}
			set
			{
				LevelLighting._bias = value;
				if (LevelLighting.bias < 1f - LevelLighting.bias)
				{
					LevelLighting._transition = LevelLighting.bias / 2f * LevelLighting.fade;
				}
				else
				{
					LevelLighting._transition = (1f - LevelLighting.bias) / 2f * LevelLighting.fade;
				}
				LevelLighting.updateLighting();
			}
		}

		public static float fade
		{
			get
			{
				return LevelLighting._fade;
			}
			set
			{
				LevelLighting._fade = value;
				if (LevelLighting.bias < 1f - LevelLighting.bias)
				{
					LevelLighting._transition = LevelLighting.bias / 2f * LevelLighting.fade;
				}
				else
				{
					LevelLighting._transition = (1f - LevelLighting.bias) / 2f * LevelLighting.fade;
				}
				LevelLighting.updateLighting();
			}
		}

		public static float time
		{
			get
			{
				return LevelLighting._time;
			}
			set
			{
				LevelLighting._time = value;
				LevelLighting.updateLighting();
			}
		}

		public static float wind
		{
			get
			{
				return LevelLighting._wind;
			}
			set
			{
				LevelLighting._wind = value;
			}
		}

		public static float christmasyness { get; private set; }

		public static float blizzardyness { get; private set; }

		public static float mistyness { get; private set; }

		public static float drizzlyness { get; private set; }

		public static byte[] hash
		{
			get
			{
				return LevelLighting._hash;
			}
		}

		public static LightingInfo[] times
		{
			get
			{
				return LevelLighting._times;
			}
		}

		public static float seaLevel
		{
			get
			{
				return LevelLighting._seaLevel;
			}
			set
			{
				LevelLighting._seaLevel = value;
				LevelLighting.updateSea();
			}
		}

		public static float snowLevel
		{
			get
			{
				return LevelLighting._snowLevel;
			}
			set
			{
				LevelLighting._snowLevel = value;
			}
		}

		public static event LevelLighting.IsSeaChangedHandler isSeaChanged;

		public static bool isSea
		{
			get
			{
				return LevelLighting._isSea;
			}
			protected set
			{
				if (LevelLighting.isSea == value)
				{
					return;
				}
				LevelLighting._isSea = value;
				if (LevelLighting.isSeaChanged != null)
				{
					LevelLighting.isSeaChanged(LevelLighting.isSea);
				}
			}
		}

		public static AudioSource effectAudio
		{
			get
			{
				return LevelLighting._effectAudio;
			}
		}

		public static AudioSource dayAudio
		{
			get
			{
				return LevelLighting._dayAudio;
			}
		}

		public static AudioSource nightAudio
		{
			get
			{
				return LevelLighting._nightAudio;
			}
		}

		public static AudioSource waterAudio
		{
			get
			{
				return LevelLighting._waterAudio;
			}
		}

		public static AudioSource windAudio
		{
			get
			{
				return LevelLighting._windAudio;
			}
		}

		public static AudioSource belowAudio
		{
			get
			{
				return LevelLighting._belowAudio;
			}
		}

		public static AudioSource rainAudio
		{
			get
			{
				return LevelLighting._rainAudio;
			}
		}

		public static bool isSkyboxReflectionEnabled
		{
			get
			{
				return LevelLighting._isSkyboxReflectionEnabled;
			}
			set
			{
				LevelLighting._isSkyboxReflectionEnabled = value;
				LevelLighting.updateSkyboxReflections();
			}
		}

		public static Transform bubbles
		{
			get
			{
				return LevelLighting._bubbles;
			}
		}

		public static Transform snow
		{
			get
			{
				return LevelLighting._snow;
			}
		}

		public static Transform rain
		{
			get
			{
				return LevelLighting._rain;
			}
		}

		public static WindZone windZone
		{
			get
			{
				return LevelLighting._windZone;
			}
		}

		public static Transform clouds
		{
			get
			{
				return LevelLighting._clouds;
			}
		}

		public static Material sea
		{
			get
			{
				return LevelLighting._sea;
			}
		}

		public static PlanarReflection reflection
		{
			get
			{
				return LevelLighting._reflection;
			}
		}

		public static byte moon
		{
			get
			{
				return LevelLighting._moon;
			}
			set
			{
				LevelLighting._moon = value;
				if (!Dedicator.isDedicated && (int)LevelLighting.moon < LevelLighting.moons.Length && LevelLighting.moonFlare != null)
				{
					LevelLighting.moonFlare.GetComponent<Renderer>().sharedMaterial = LevelLighting.moons[(int)LevelLighting.moon];
				}
			}
		}

		public static void setEnabled(bool isEnabled)
		{
			LevelLighting.sun.GetComponent<Light>().enabled = isEnabled;
		}

		public static bool isPositionSnowy(Vector3 position)
		{
			return Level.info != null && Level.info.configData.Use_Legacy_Snow_Height && LevelLighting.snowLevel > 0.01f && position.y > LevelLighting.snowLevel * Level.TERRAIN;
		}

		public static bool isPositionUnderwater(Vector3 position)
		{
			return Level.info != null && Level.info.configData.Use_Legacy_Water && LevelLighting.seaLevel < 0.99f && position.y < LevelLighting.seaLevel * Level.TERRAIN;
		}

		public static float getWaterSurfaceElevation()
		{
			return LevelLighting.seaLevel * Level.TERRAIN;
		}

		public static Vector4 getSeaVector(string name)
		{
			return LevelLighting.sea.GetVector(name);
		}

		public static void setSeaVector(string name, Vector4 vector)
		{
			LevelLighting.sea.SetVector(name, vector);
			foreach (WaterVolume waterVolume in WaterSystem.volumes)
			{
				if (!(waterVolume.sea == null))
				{
					waterVolume.sea.SetVector(name, vector);
				}
			}
		}

		public static Color getSeaColor(string name)
		{
			return LevelLighting.sea.GetColor(name);
		}

		public static void setSeaColor(string name, Color color)
		{
			LevelLighting.sea.SetColor(name, color);
			foreach (WaterVolume waterVolume in WaterSystem.volumes)
			{
				if (!(waterVolume.sea == null))
				{
					waterVolume.sea.SetColor(name, color);
				}
			}
		}

		public static float getSeaFloat(string name)
		{
			return LevelLighting.sea.GetFloat(name);
		}

		public static void setSeaFloat(string name, float value)
		{
			LevelLighting.sea.SetFloat(name, value);
			foreach (WaterVolume waterVolume in WaterSystem.volumes)
			{
				if (!(waterVolume.sea == null))
				{
					waterVolume.sea.SetFloat(name, value);
				}
			}
		}

		public static void updateLighting()
		{
			if (LevelLighting.sun == null || LevelLighting.sea == null)
			{
				return;
			}
			float num = 0f;
			LevelLighting.setSeaVector("_WorldLightDir", LevelLighting.sunFlare.forward);
			LightingInfo lightingInfo;
			LightingInfo lightingInfo2;
			if (LevelLighting.time < LevelLighting.bias)
			{
				LevelLighting.sun.rotation = Quaternion.Euler(LevelLighting.time / LevelLighting.bias * 180f, LevelLighting.azimuth, 0f);
				if (LevelLighting.time < LevelLighting.transition)
				{
					LevelLighting.dayVolume = Mathf.Lerp(0.5f, 1f, LevelLighting.time / LevelLighting.transition);
					LevelLighting.nightVolume = Mathf.Lerp(0.5f, 0f, LevelLighting.time / LevelLighting.transition);
					lightingInfo = LevelLighting.times[0];
					lightingInfo2 = LevelLighting.times[1];
					num = LevelLighting.time / LevelLighting.transition;
					LevelLighting.setSeaColor("_Foam", Color.Lerp(LevelLighting.FOAM_DAWN, LevelLighting.FOAM_MIDDAY, LevelLighting.time / LevelLighting.transition));
					LevelLighting.setSeaFloat("_Shininess", Mathf.Lerp(LevelLighting.SPECULAR_DAWN, LevelLighting.SPECULAR_MIDDAY, LevelLighting.time / LevelLighting.transition));
					RenderSettings.reflectionIntensity = Mathf.Lerp(LevelLighting.REFLECTION_DAWN, LevelLighting.REFLECTION_MIDDAY, LevelLighting.time / LevelLighting.transition);
				}
				else if (LevelLighting.time < LevelLighting.bias - LevelLighting.transition)
				{
					LevelLighting.dayVolume = 1f;
					LevelLighting.nightVolume = 0f;
					lightingInfo = null;
					lightingInfo2 = LevelLighting.times[1];
					LevelLighting.setSeaColor("_Foam", LevelLighting.FOAM_MIDDAY);
					LevelLighting.setSeaFloat("_Shininess", LevelLighting.SPECULAR_MIDDAY);
					RenderSettings.reflectionIntensity = LevelLighting.REFLECTION_MIDDAY;
				}
				else
				{
					LevelLighting.dayVolume = Mathf.Lerp(1f, 0.5f, (LevelLighting.time - LevelLighting.bias + LevelLighting.transition) / LevelLighting.transition);
					LevelLighting.nightVolume = Mathf.Lerp(0f, 0.5f, (LevelLighting.time - LevelLighting.bias + LevelLighting.transition) / LevelLighting.transition);
					lightingInfo = LevelLighting.times[1];
					lightingInfo2 = LevelLighting.times[2];
					num = (LevelLighting.time - LevelLighting.bias + LevelLighting.transition) / LevelLighting.transition;
					LevelLighting.setSeaColor("_Foam", Color.Lerp(LevelLighting.FOAM_MIDDAY, LevelLighting.FOAM_DUSK, (LevelLighting.time - LevelLighting.bias + LevelLighting.transition) / LevelLighting.transition));
					LevelLighting.setSeaFloat("_Shininess", Mathf.Lerp(LevelLighting.SPECULAR_MIDDAY, LevelLighting.SPECULAR_DUSK, (LevelLighting.time - LevelLighting.bias + LevelLighting.transition) / LevelLighting.transition));
					RenderSettings.reflectionIntensity = Mathf.Lerp(LevelLighting.REFLECTION_MIDDAY, LevelLighting.REFLECTION_DUSK, (LevelLighting.time - LevelLighting.bias + LevelLighting.transition) / LevelLighting.transition);
				}
				LevelLighting.updateStars(1f);
				LevelLighting.auroraBorealisTargetIntensity = 0f;
			}
			else
			{
				LevelLighting.sun.rotation = Quaternion.Euler(180f + (LevelLighting.time - LevelLighting.bias) / (1f - LevelLighting.bias) * 180f, LevelLighting.azimuth, 0f);
				if (LevelLighting.time < LevelLighting.bias + LevelLighting.transition)
				{
					LevelLighting.dayVolume = Mathf.Lerp(0.5f, 0f, (LevelLighting.time - LevelLighting.bias) / LevelLighting.transition);
					LevelLighting.nightVolume = Mathf.Lerp(0.5f, 1f, (LevelLighting.time - LevelLighting.bias) / LevelLighting.transition);
					lightingInfo = LevelLighting.times[2];
					lightingInfo2 = LevelLighting.times[3];
					num = (LevelLighting.time - LevelLighting.bias) / LevelLighting.transition;
					LevelLighting.setSeaColor("_Foam", Color.Lerp(LevelLighting.FOAM_DUSK, LevelLighting.FOAM_MIDNIGHT, (LevelLighting.time - LevelLighting.bias) / LevelLighting.transition));
					LevelLighting.setSeaFloat("_Shininess", Mathf.Lerp(LevelLighting.SPECULAR_DUSK, LevelLighting.SPECULAR_MIDNIGHT, (LevelLighting.time - LevelLighting.bias) / LevelLighting.transition));
					RenderSettings.reflectionIntensity = Mathf.Lerp(LevelLighting.REFLECTION_DUSK, LevelLighting.REFLECTION_MIDNIGHT, (LevelLighting.time - LevelLighting.bias) / LevelLighting.transition);
					LevelLighting.updateStars(Mathf.Lerp(1f, 0.05f, num));
					LevelLighting.auroraBorealisTargetIntensity = 0f;
				}
				else if (LevelLighting.time < 1f - LevelLighting.transition)
				{
					LevelLighting.dayVolume = 0f;
					LevelLighting.nightVolume = 1f;
					lightingInfo = null;
					lightingInfo2 = LevelLighting.times[3];
					LevelLighting.setSeaColor("_Foam", LevelLighting.FOAM_MIDNIGHT);
					LevelLighting.setSeaFloat("_Shininess", LevelLighting.SPECULAR_MIDNIGHT);
					RenderSettings.reflectionIntensity = LevelLighting.REFLECTION_MIDNIGHT;
					LevelLighting.updateStars(0.05f);
					LevelLighting.auroraBorealisTargetIntensity = 1f;
				}
				else
				{
					LevelLighting.dayVolume = Mathf.Lerp(0f, 0.5f, (LevelLighting.time - 1f + LevelLighting.transition) / LevelLighting.transition);
					LevelLighting.nightVolume = Mathf.Lerp(1f, 0.5f, (LevelLighting.time - 1f + LevelLighting.transition) / LevelLighting.transition);
					lightingInfo = LevelLighting.times[3];
					lightingInfo2 = LevelLighting.times[0];
					num = (LevelLighting.time - 1f + LevelLighting.transition) / LevelLighting.transition;
					LevelLighting.setSeaColor("_Foam", Color.Lerp(LevelLighting.FOAM_MIDNIGHT, LevelLighting.FOAM_DAWN, (LevelLighting.time - 1f + LevelLighting.transition) / LevelLighting.transition));
					LevelLighting.setSeaFloat("_Shininess", Mathf.Lerp(LevelLighting.SPECULAR_MIDNIGHT, LevelLighting.SPECULAR_DAWN, (LevelLighting.time - 1f + LevelLighting.transition) / LevelLighting.transition));
					RenderSettings.reflectionIntensity = Mathf.Lerp(LevelLighting.REFLECTION_MIDNIGHT, LevelLighting.REFLECTION_DAWN, (LevelLighting.time - 1f + LevelLighting.transition) / LevelLighting.transition);
					LevelLighting.updateStars(Mathf.Lerp(0.05f, 1f, num));
					LevelLighting.auroraBorealisTargetIntensity = 0f;
				}
			}
			if (lightingInfo == null)
			{
				LevelLighting.sunFlare.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.Lerp(Color.white, lightingInfo2.colors[0], 0.5f));
				LevelLighting.sun.GetComponent<Light>().color = lightingInfo2.colors[0];
				LevelLighting.sun.GetComponent<Light>().intensity = lightingInfo2.singles[0];
				LevelLighting.sun.GetComponent<Light>().shadowStrength = lightingInfo2.singles[3] * (1f - LevelLighting.drizzlyness * 0.8f);
				LevelLighting.setSeaColor("_BaseColor", lightingInfo2.colors[1]);
				LevelLighting.setSeaColor("_ReflectionColor", lightingInfo2.colors[1]);
				RenderSettings.ambientSkyColor = lightingInfo2.colors[6];
				RenderSettings.ambientEquatorColor = lightingInfo2.colors[7];
				RenderSettings.ambientGroundColor = lightingInfo2.colors[8];
				LevelLighting.skyboxSky = lightingInfo2.colors[3];
				LevelLighting.skyboxEquator = lightingInfo2.colors[4];
				LevelLighting.skyboxGround = lightingInfo2.colors[5];
				LevelLighting.skyboxClouds = lightingInfo2.colors[9];
				LevelLighting.rainColor = lightingInfo2.colors[11];
				LevelLighting.raysColor = lightingInfo2.colors[10];
				LevelLighting.raysIntensity = lightingInfo2.singles[4] * 4f;
				if (MainCamera.instance != null)
				{
					GlobalFog component = MainCamera.instance.GetComponent<GlobalFog>();
					if (component != null)
					{
						if (LevelLighting.seaLevel < 0.99f)
						{
							component.height = LevelLighting.seaLevel * Level.TERRAIN + LevelLighting.fogHeight + lightingInfo2.singles[1] * LevelLighting.fogSize;
						}
						else
						{
							component.height = LevelLighting.fogHeight + lightingInfo2.singles[1] * LevelLighting.fogSize;
						}
						component.globalFogColor = lightingInfo2.colors[2];
					}
				}
				LevelLighting.clouds.GetComponent<ParticleSystem>().emission.rateOverTime = lightingInfo2.singles[2];
			}
			else
			{
				LevelLighting.sunFlare.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.Lerp(Color.white, Color.Lerp(lightingInfo.colors[0], lightingInfo2.colors[0], num), 0.5f));
				LevelLighting.sun.GetComponent<Light>().color = Color.Lerp(lightingInfo.colors[0], lightingInfo2.colors[0], num);
				LevelLighting.sun.GetComponent<Light>().intensity = Mathf.Lerp(lightingInfo.singles[0], lightingInfo2.singles[0], num);
				LevelLighting.sun.GetComponent<Light>().shadowStrength = Mathf.Lerp(lightingInfo.singles[3], lightingInfo2.singles[3], num) * (1f - LevelLighting.drizzlyness * 0.8f);
				LevelLighting.setSeaColor("_BaseColor", Color.Lerp(lightingInfo.colors[1], lightingInfo2.colors[1], num));
				LevelLighting.setSeaColor("_ReflectionColor", Color.Lerp(lightingInfo.colors[1], lightingInfo2.colors[1], num));
				RenderSettings.ambientSkyColor = Color.Lerp(lightingInfo.colors[6], lightingInfo2.colors[6], num);
				RenderSettings.ambientEquatorColor = Color.Lerp(lightingInfo.colors[7], lightingInfo2.colors[7], num);
				RenderSettings.ambientGroundColor = Color.Lerp(lightingInfo.colors[8], lightingInfo2.colors[8], num);
				LevelLighting.skyboxSky = Color.Lerp(lightingInfo.colors[3], lightingInfo2.colors[3], num);
				LevelLighting.skyboxEquator = Color.Lerp(lightingInfo.colors[4], lightingInfo2.colors[4], num);
				LevelLighting.skyboxGround = Color.Lerp(lightingInfo.colors[5], lightingInfo2.colors[5], num);
				LevelLighting.skyboxClouds = Color.Lerp(lightingInfo.colors[9], lightingInfo2.colors[9], num);
				LevelLighting.rainColor = Color.Lerp(lightingInfo.colors[11], lightingInfo2.colors[11], num);
				LevelLighting.raysColor = Color.Lerp(lightingInfo.colors[10], lightingInfo2.colors[10], num);
				LevelLighting.raysIntensity = Mathf.Lerp(lightingInfo.singles[4], lightingInfo2.singles[4], num) * 4f;
				if (MainCamera.instance != null)
				{
					GlobalFog component2 = MainCamera.instance.GetComponent<GlobalFog>();
					if (component2 != null)
					{
						if (LevelLighting.seaLevel < 0.99f)
						{
							component2.height = LevelLighting.seaLevel * Level.TERRAIN + LevelLighting.fogHeight + Mathf.Lerp(lightingInfo.singles[1], lightingInfo2.singles[1], num) * LevelLighting.fogSize;
						}
						else
						{
							component2.height = LevelLighting.fogHeight + Mathf.Lerp(lightingInfo.singles[1], lightingInfo2.singles[1], num) * LevelLighting.fogSize;
						}
						component2.globalFogColor = Color.Lerp(lightingInfo.colors[2], lightingInfo2.colors[2], num);
					}
				}
				LevelLighting.clouds.GetComponent<ParticleSystem>().emission.rateOverTime = Mathf.Lerp(lightingInfo.singles[2], lightingInfo2.singles[2], num);
			}
			if (LevelLighting.localBlendingFog && MainCamera.instance != null)
			{
				GlobalFog component3 = MainCamera.instance.GetComponent<GlobalFog>();
				if (component3 != null)
				{
					component3.height = Mathf.Lerp(component3.height, LevelLighting.localFogHeight, LevelLighting.localFogBlend);
					component3.globalFogColor = Color.Lerp(component3.globalFogColor, LevelLighting.localFogColor, LevelLighting.localFogBlend);
				}
			}
			if (LevelLighting.localBlendingLight)
			{
				LevelLighting.setSeaColor("_Foam", Color.Lerp(LevelLighting.getSeaColor("_Foam"), Color.black, LevelLighting.localLightingBlend * LevelLighting.PITCH_DARK_WATER_BLEND));
				LevelLighting.setSeaFloat("_Shininess", Mathf.Lerp(LevelLighting.getSeaFloat("_Shininess"), 0f, LevelLighting.localLightingBlend * LevelLighting.PITCH_DARK_WATER_BLEND));
				LevelLighting.setSeaColor("_BaseColor", Color.Lerp(LevelLighting.getSeaColor("_BaseColor"), Color.black, LevelLighting.localLightingBlend * LevelLighting.PITCH_DARK_WATER_BLEND));
				LevelLighting.setSeaColor("_ReflectionColor", Color.Lerp(LevelLighting.getSeaColor("_ReflectionColor"), Color.black, LevelLighting.localLightingBlend * LevelLighting.PITCH_DARK_WATER_BLEND));
				LevelLighting.sun.GetComponent<Light>().color = Color.Lerp(LevelLighting.sun.GetComponent<Light>().color, Color.black, LevelLighting.localLightingBlend);
				LevelLighting.sun.GetComponent<Light>().intensity = Mathf.Lerp(LevelLighting.sun.GetComponent<Light>().intensity, 0f, LevelLighting.localLightingBlend);
				LevelLighting.sun.GetComponent<Light>().shadowStrength = Mathf.Lerp(LevelLighting.sun.GetComponent<Light>().shadowStrength, 0f, LevelLighting.localLightingBlend);
				RenderSettings.ambientSkyColor = Color.Lerp(RenderSettings.ambientSkyColor, Color.black, LevelLighting.localLightingBlend);
				RenderSettings.ambientEquatorColor = Color.Lerp(RenderSettings.ambientEquatorColor, Color.black, LevelLighting.localLightingBlend);
				RenderSettings.ambientGroundColor = Color.Lerp(RenderSettings.ambientGroundColor, Color.black, LevelLighting.localLightingBlend);
				RenderSettings.ambientMode = 1;
				LevelLighting.skyboxSky = Color.Lerp(LevelLighting.skyboxSky, Color.black, LevelLighting.localLightingBlend);
				LevelLighting.skyboxEquator = Color.Lerp(LevelLighting.skyboxEquator, Color.black, LevelLighting.localLightingBlend);
				LevelLighting.skyboxGround = Color.Lerp(LevelLighting.skyboxGround, Color.black, LevelLighting.localLightingBlend);
				LevelLighting.skyboxClouds = Color.Lerp(LevelLighting.skyboxClouds, Color.black, LevelLighting.localLightingBlend);
				LevelLighting.rainColor = Color.Lerp(LevelLighting.rainColor, Color.black, LevelLighting.localLightingBlend);
			}
			LevelLighting.setSeaColor("_SpecularColor", LevelLighting.sun.GetComponent<Light>().color);
			if (LevelLighting.vision == ELightingVision.MILITARY)
			{
				LevelLighting.setSeaColor("_BaseColor", LevelLighting.NIGHTVISION_MILITARY);
				LevelLighting.setSeaColor("_ReflectionColor", LevelLighting.NIGHTVISION_MILITARY);
				RenderSettings.ambientSkyColor = LevelLighting.NIGHTVISION_MILITARY;
				RenderSettings.ambientEquatorColor = LevelLighting.NIGHTVISION_MILITARY;
				RenderSettings.ambientGroundColor = LevelLighting.NIGHTVISION_MILITARY;
				RenderSettings.ambientMode = 1;
				LevelLighting.skyboxSky = LevelLighting.NIGHTVISION_MILITARY;
				LevelLighting.skyboxEquator = LevelLighting.NIGHTVISION_MILITARY;
				LevelLighting.skyboxGround = LevelLighting.NIGHTVISION_MILITARY;
				LevelLighting.skyboxClouds = LevelLighting.NIGHTVISION_MILITARY;
			}
			else if (LevelLighting.vision == ELightingVision.CIVILIAN)
			{
				LevelLighting.setSeaColor("_BaseColor", LevelLighting.NIGHTVISION_CIVILIAN);
				LevelLighting.setSeaColor("_ReflectionColor", LevelLighting.NIGHTVISION_CIVILIAN);
				RenderSettings.ambientSkyColor = LevelLighting.NIGHTVISION_CIVILIAN;
				RenderSettings.ambientEquatorColor = LevelLighting.NIGHTVISION_CIVILIAN;
				RenderSettings.ambientGroundColor = LevelLighting.NIGHTVISION_CIVILIAN;
				RenderSettings.ambientMode = 1;
				LevelLighting.skyboxSky = LevelLighting.NIGHTVISION_CIVILIAN;
				LevelLighting.skyboxEquator = LevelLighting.NIGHTVISION_CIVILIAN;
				LevelLighting.skyboxGround = LevelLighting.NIGHTVISION_CIVILIAN;
				LevelLighting.skyboxClouds = LevelLighting.NIGHTVISION_CIVILIAN;
			}
			if ((LevelLighting.vision == ELightingVision.MILITARY || LevelLighting.vision == ELightingVision.CIVILIAN) && LevelLighting.localBlendingLight)
			{
				RenderSettings.ambientSkyColor = Color.Lerp(RenderSettings.ambientSkyColor, Color.black, LevelLighting.localLightingBlend / 2f);
				RenderSettings.ambientEquatorColor = Color.Lerp(RenderSettings.ambientSkyColor, Color.black, LevelLighting.localLightingBlend / 2f);
				RenderSettings.ambientGroundColor = Color.Lerp(RenderSettings.ambientSkyColor, Color.black, LevelLighting.localLightingBlend / 2f);
				LevelLighting.skyboxSky = Color.Lerp(LevelLighting.skyboxSky, Color.black, LevelLighting.localLightingBlend / 2f);
				LevelLighting.skyboxEquator = Color.Lerp(LevelLighting.skyboxEquator, Color.black, LevelLighting.localLightingBlend / 2f);
				LevelLighting.skyboxGround = Color.Lerp(LevelLighting.skyboxGround, Color.black, LevelLighting.localLightingBlend / 2f);
				LevelLighting.skyboxClouds = Color.Lerp(LevelLighting.skyboxClouds, Color.black, LevelLighting.localLightingBlend / 2f);
			}
			if (Time.time - LevelLighting.lastSkyboxUpdate > 3f)
			{
				LevelLighting.lastSkyboxUpdate = Time.time;
				LevelLighting.skyboxUpdated = false;
			}
			if (MainCamera.instance != null)
			{
				if (LevelLighting.vision == ELightingVision.MILITARY)
				{
					GlobalFog component4 = MainCamera.instance.GetComponent<GlobalFog>();
					if (component4 != null)
					{
						component4.height = 2048f;
						component4.globalFogColor = LevelLighting.NIGHTVISION_MILITARY;
					}
				}
				else if (LevelLighting.vision == ELightingVision.CIVILIAN)
				{
					GlobalFog component5 = MainCamera.instance.GetComponent<GlobalFog>();
					if (component5 != null)
					{
						component5.height = 2048f;
						component5.globalFogColor = LevelLighting.NIGHTVISION_CIVILIAN;
					}
				}
				SunShafts component6 = MainCamera.instance.GetComponent<SunShafts>();
				if (component6 != null)
				{
					component6.sunTransform = LevelLighting.sunFlare;
					component6.sunColor = LevelLighting.raysColor;
					component6.sunShaftIntensity = LevelLighting.raysIntensity * LevelLighting.raysMultiplier;
				}
				LevelLighting.reflection.clearColor = MainCamera.instance.backgroundColor;
				if (Player.player != null)
				{
					Player.player.look.scopeCamera.backgroundColor = MainCamera.instance.backgroundColor;
					GlobalFog component7 = MainCamera.instance.GetComponent<GlobalFog>();
					if (component7 != null)
					{
						GlobalFog component8 = Player.player.look.scopeCamera.GetComponent<GlobalFog>();
						if (component8 != null)
						{
							component8.height = component7.height;
							component8.globalFogColor = component7.globalFogColor;
						}
					}
				}
			}
			LevelLighting.stars.rotation = Quaternion.Euler(Time.realtimeSinceStartup * 0.01f, Time.realtimeSinceStartup * 0.01f, Time.realtimeSinceStartup * 0.01f);
		}

		public static void load(ushort size)
		{
			LevelLighting.vision = ELightingVision.NONE;
			LevelLighting.isSea = false;
			LevelLighting.isSnow = false;
			LevelLighting.christmasyness = 0f;
			LevelLighting.blizzardyness = 0f;
			LevelLighting.mistyness = 0f;
			LevelLighting.drizzlyness = 0f;
			LevelLighting.raysMultiplier = 1f;
			LevelLighting.localEffectNode = null;
			LevelLighting.localPlayingEffect = false;
			LevelLighting.localBlendingLight = false;
			LevelLighting.localLightingBlend = 1f;
			LevelLighting.localBlendingFog = false;
			LevelLighting.localFogBlend = 1f;
			LevelLighting.rainBlend = 1f;
			LevelLighting.snowBlend = 1f;
			LevelLighting.auroraBorealisCurrentIntensity = 0f;
			LevelLighting.auroraBorealisTargetIntensity = 0f;
			LevelLighting.currentAudioVolume = 0f;
			LevelLighting.targetAudioVolume = 0f;
			LevelLighting.nextAudioVolumeChangeTime = -1f;
			if (!Dedicator.isDedicated)
			{
				LevelLighting.skybox = (Material)Object.Instantiate(Resources.Load("Level/Skybox"));
				RenderSettings.skybox = LevelLighting.skybox;
				LevelLighting.lighting = ((GameObject)Object.Instantiate(Resources.Load("Level/Lighting"))).transform;
				LevelLighting.lighting.name = "Lighting";
				LevelLighting.lighting.position = Vector3.zero;
				LevelLighting.lighting.rotation = Quaternion.identity;
				LevelLighting.lighting.parent = Level.level;
				LevelLighting.sun = LevelLighting.lighting.FindChild("Sun");
				LevelLighting.sunFlare = LevelLighting.sun.FindChild("Flare_Sun");
				LevelLighting.moonFlare = LevelLighting.sun.FindChild("Flare_Moon");
				LevelLighting.stars = LevelLighting.lighting.FindChild("Stars");
				LevelLighting._bubbles = LevelLighting.lighting.FindChild("Bubbles");
				LevelLighting._snow = LevelLighting.lighting.FindChild("Snow");
				LevelLighting._rain = LevelLighting.lighting.FindChild("Rain");
				LevelLighting._clouds = LevelLighting.lighting.FindChild("Clouds");
				LevelLighting._windZone = LevelLighting.lighting.FindChild("WindZone").GetComponent<WindZone>();
				LevelLighting.reflectionCamera = LevelLighting.lighting.FindChild("Reflection").GetComponent<Camera>();
				LevelLighting.reflectionMap = new Cubemap(16, 5, false);
				LevelLighting.reflectionMap.name = "Skybox_Reflection";
				LevelLighting.reflectionMapVision = new Cubemap(16, 5, false);
				LevelLighting.reflectionMapVision.name = "Skybox_Reflection_Vision";
				RenderSettings.defaultReflectionMode = 1;
				LevelLighting.reflectionIndex = 0;
				LevelLighting.reflectionIndexVision = 0;
				LevelLighting.isReflectionBuilding = false;
				LevelLighting.isReflectionBuildingVision = false;
				LevelLighting.mist = LevelLighting.clouds.GetComponent<Renderer>().material;
				LevelLighting.puddles = LevelLighting.lighting.GetComponent<Rain>();
				LevelLighting.auroraBorealisTransform = LevelLighting.lighting.FindChild("Aurora_Borealis");
				LevelLighting.auroraBorealisTransform.gameObject.SetActive(Level.info.configData.Is_Aurora_Borealis_Visible);
				LevelLighting.auroraBorealisMaterial = LevelLighting.auroraBorealisTransform.GetComponent<MeshRenderer>().material;
				LevelLighting.moons = new Material[(int)LevelLighting.MOON_CYCLES];
				for (int i = 0; i < LevelLighting.moons.Length; i++)
				{
					LevelLighting.moons[i] = (Material)Resources.Load("Flares/Moon_" + i);
				}
				LevelLighting._effectAudio = LevelLighting.lighting.FindChild("Effect").GetComponent<AudioSource>();
				LevelLighting._dayAudio = LevelLighting.lighting.FindChild("Day").GetComponent<AudioSource>();
				LevelLighting._nightAudio = LevelLighting.lighting.FindChild("Night").GetComponent<AudioSource>();
				LevelLighting._waterAudio = LevelLighting.lighting.FindChild("Water").GetComponent<AudioSource>();
				LevelLighting._windAudio = LevelLighting.lighting.FindChild("Wind").GetComponent<AudioSource>();
				LevelLighting._belowAudio = LevelLighting.lighting.FindChild("Below").GetComponent<AudioSource>();
				LevelLighting._rainAudio = LevelLighting.lighting.FindChild("Rain").GetComponent<AudioSource>();
				if (ReadWrite.fileExists(Level.info.path + "/Environment/Ambience.unity3d", false, false))
				{
					Bundle bundle = Bundles.getBundle(Level.info.path + "/Environment/Ambience.unity3d", false);
					LevelLighting.dayAudio.clip = (AudioClip)bundle.load("Day");
					LevelLighting.dayAudio.Play();
					LevelLighting.nightAudio.clip = (AudioClip)bundle.load("Night");
					LevelLighting.nightAudio.Play();
					LevelLighting.waterAudio.clip = (AudioClip)bundle.load("Water");
					LevelLighting.waterAudio.Play();
					LevelLighting.windAudio.clip = (AudioClip)bundle.load("Wind");
					LevelLighting.windAudio.Play();
					LevelLighting.belowAudio.clip = (AudioClip)bundle.load("Below");
					LevelLighting.belowAudio.Play();
					LevelLighting.rainAudio.clip = (AudioClip)bundle.load("Rain");
					LevelLighting.rainAudio.Play();
					bundle.unload();
				}
				LevelLighting.water = ((GameObject)Object.Instantiate(Resources.Load("Level/Water"))).transform;
				LevelLighting.water.name = "Water";
				LevelLighting.water.parent = Level.level;
				LevelLighting.water.transform.rotation = Quaternion.identity;
				LevelLighting.water.transform.localScale = new Vector3((float)(size * 2) / 100f, 1f, (float)(size * 2) / 100f);
				LevelLighting._sea = LevelLighting.water.GetChild(0).GetChild(0).GetComponent<Renderer>().material;
				for (int j = 0; j < LevelLighting.water.childCount; j++)
				{
					Transform child = LevelLighting.water.GetChild(j);
					for (int k = 0; k < LevelLighting.water.childCount; k++)
					{
						Transform child2 = child.GetChild(k);
						child2.GetComponent<Renderer>().material = LevelLighting.sea;
					}
				}
				LevelLighting._reflection = LevelLighting.water.GetComponent<PlanarReflection>();
			}
			if (ReadWrite.fileExists(Level.info.path + "/Environment/Lighting.dat", false, false))
			{
				Block block = ReadWrite.readBlock(Level.info.path + "/Environment/Lighting.dat", false, false, 0);
				byte b = block.readByte();
				LevelLighting._azimuth = block.readSingle();
				LevelLighting._bias = block.readSingle();
				LevelLighting._fade = block.readSingle();
				LevelLighting._time = block.readSingle();
				LevelLighting.moon = block.readByte();
				if (b >= 5)
				{
					LevelLighting._seaLevel = block.readSingle();
					LevelLighting._snowLevel = block.readSingle();
					if (b > 6)
					{
						LevelLighting.canRain = block.readBoolean();
					}
					else
					{
						LevelLighting.canRain = false;
					}
					if (b > 10)
					{
						LevelLighting.canSnow = block.readBoolean();
					}
					else
					{
						LevelLighting.canSnow = false;
					}
					if (b < 8)
					{
						LevelLighting.rainFreq = 1f;
						LevelLighting.rainDur = 1f;
					}
					else
					{
						LevelLighting.rainFreq = block.readSingle();
						LevelLighting.rainDur = block.readSingle();
					}
					if (b < 11)
					{
						LevelLighting.snowFreq = 1f;
						LevelLighting.snowDur = 1f;
					}
					else
					{
						LevelLighting.snowFreq = block.readSingle();
						LevelLighting.snowDur = block.readSingle();
					}
					LevelLighting._times = new LightingInfo[4];
					for (int l = 0; l < LevelLighting.times.Length; l++)
					{
						Color[] array = new Color[12];
						float[] array2 = new float[5];
						if (b > 9)
						{
							for (int m = 0; m < array.Length; m++)
							{
								array[m] = block.readColor();
							}
							for (int n = 0; n < array2.Length; n++)
							{
								array2[n] = block.readSingle();
							}
						}
						else if (b > 8)
						{
							for (int num = 0; num < array.Length - 1; num++)
							{
								array[num] = block.readColor();
							}
							array[11] = array[3];
							for (int num2 = 0; num2 < array2.Length; num2++)
							{
								array2[num2] = block.readSingle();
							}
						}
						else
						{
							if (b >= 6)
							{
								for (int num3 = 0; num3 < array.Length - 1; num3++)
								{
									array[num3] = block.readColor();
								}
							}
							else
							{
								for (int num4 = 0; num4 < array.Length - 2; num4++)
								{
									array[num4] = block.readColor();
								}
								array[9] = array[2];
							}
							for (int num5 = 0; num5 < array2.Length - 1; num5++)
							{
								array2[num5] = block.readSingle();
							}
							array[10] = array[0];
							array2[4] = 0.25f;
						}
						LightingInfo lightingInfo = new LightingInfo(array, array2);
						LevelLighting.times[l] = lightingInfo;
					}
				}
				else
				{
					LevelLighting._times = new LightingInfo[4];
					for (int num6 = 0; num6 < LevelLighting.times.Length; num6++)
					{
						Color[] newColors = new Color[12];
						float[] newSingles = new float[5];
						LightingInfo lightingInfo2 = new LightingInfo(newColors, newSingles);
						LevelLighting.times[num6] = lightingInfo2;
					}
					LevelLighting.times[0].colors[3] = block.readColor();
					LevelLighting.times[1].colors[3] = block.readColor();
					LevelLighting.times[2].colors[3] = block.readColor();
					LevelLighting.times[3].colors[3] = block.readColor();
					LevelLighting.times[0].colors[4] = LevelLighting.times[0].colors[3];
					LevelLighting.times[1].colors[4] = LevelLighting.times[1].colors[3];
					LevelLighting.times[2].colors[4] = LevelLighting.times[2].colors[3];
					LevelLighting.times[3].colors[4] = LevelLighting.times[3].colors[3];
					LevelLighting.times[0].colors[5] = LevelLighting.times[0].colors[3];
					LevelLighting.times[1].colors[5] = LevelLighting.times[1].colors[3];
					LevelLighting.times[2].colors[5] = LevelLighting.times[2].colors[3];
					LevelLighting.times[3].colors[5] = LevelLighting.times[3].colors[3];
					LevelLighting.times[0].colors[6] = block.readColor();
					LevelLighting.times[1].colors[6] = block.readColor();
					LevelLighting.times[2].colors[6] = block.readColor();
					LevelLighting.times[3].colors[6] = block.readColor();
					LevelLighting.times[0].colors[7] = LevelLighting.times[0].colors[6];
					LevelLighting.times[1].colors[7] = LevelLighting.times[1].colors[6];
					LevelLighting.times[2].colors[7] = LevelLighting.times[2].colors[6];
					LevelLighting.times[3].colors[7] = LevelLighting.times[3].colors[6];
					LevelLighting.times[0].colors[8] = LevelLighting.times[0].colors[6];
					LevelLighting.times[1].colors[8] = LevelLighting.times[1].colors[6];
					LevelLighting.times[2].colors[8] = LevelLighting.times[2].colors[6];
					LevelLighting.times[3].colors[8] = LevelLighting.times[3].colors[6];
					LevelLighting.times[0].colors[2] = block.readColor();
					LevelLighting.times[1].colors[2] = block.readColor();
					LevelLighting.times[2].colors[2] = block.readColor();
					LevelLighting.times[3].colors[2] = block.readColor();
					LevelLighting.times[0].colors[0] = block.readColor();
					LevelLighting.times[1].colors[0] = block.readColor();
					LevelLighting.times[2].colors[0] = block.readColor();
					LevelLighting.times[3].colors[0] = block.readColor();
					LevelLighting.times[0].singles[0] = block.readSingle();
					LevelLighting.times[1].singles[0] = block.readSingle();
					LevelLighting.times[2].singles[0] = block.readSingle();
					LevelLighting.times[3].singles[0] = block.readSingle();
					LevelLighting.times[0].singles[1] = block.readSingle();
					LevelLighting.times[1].singles[1] = block.readSingle();
					LevelLighting.times[2].singles[1] = block.readSingle();
					LevelLighting.times[3].singles[1] = block.readSingle();
					LevelLighting.times[0].singles[2] = block.readSingle();
					LevelLighting.times[1].singles[2] = block.readSingle();
					LevelLighting.times[2].singles[2] = block.readSingle();
					LevelLighting.times[3].singles[2] = block.readSingle();
					LevelLighting.times[0].singles[3] = block.readSingle();
					LevelLighting.times[1].singles[3] = block.readSingle();
					LevelLighting.times[2].singles[3] = block.readSingle();
					LevelLighting.times[3].singles[3] = block.readSingle();
					if (b > 2)
					{
						LevelLighting._seaLevel = block.readSingle();
					}
					else
					{
						LevelLighting._seaLevel = block.readSingle() / 2f;
					}
					if (b > 1)
					{
						LevelLighting._snowLevel = block.readSingle();
					}
					else
					{
						LevelLighting._snowLevel = 0f;
					}
					LevelLighting.canRain = false;
					LevelLighting.canSnow = false;
					LevelLighting.times[0].colors[1] = block.readColor();
					LevelLighting.times[1].colors[1] = block.readColor();
					LevelLighting.times[2].colors[1] = block.readColor();
					LevelLighting.times[3].colors[1] = block.readColor();
				}
				LevelLighting._hash = block.getHash();
			}
			else
			{
				LevelLighting._azimuth = 0.2f;
				LevelLighting._bias = 0.5f;
				LevelLighting._fade = 1f;
				LevelLighting._time = LevelLighting.bias / 2f;
				LevelLighting.moon = 0;
				LevelLighting._seaLevel = 1f;
				LevelLighting._snowLevel = 0f;
				LevelLighting.canRain = true;
				LevelLighting.canSnow = false;
				LevelLighting.rainFreq = 1f;
				LevelLighting.rainDur = 1f;
				LevelLighting.snowFreq = 1f;
				LevelLighting.snowDur = 1f;
				LevelLighting._times = new LightingInfo[4];
				for (int num7 = 0; num7 < LevelLighting.times.Length; num7++)
				{
					Color[] newColors2 = new Color[12];
					float[] newSingles2 = new float[5];
					LightingInfo lightingInfo3 = new LightingInfo(newColors2, newSingles2);
					LevelLighting.times[num7] = lightingInfo3;
				}
				LevelLighting._hash = new byte[20];
			}
			if (LevelLighting.bias < 1f - LevelLighting.bias)
			{
				LevelLighting._transition = LevelLighting.bias / 2f * LevelLighting.fade;
			}
			else
			{
				LevelLighting._transition = (1f - LevelLighting.bias) / 2f * LevelLighting.fade;
			}
			LevelLighting.times[0].colors[1].a = 0.25f;
			LevelLighting.times[1].colors[1].a = 0.5f;
			LevelLighting.times[2].colors[1].a = 0.75f;
			LevelLighting.times[3].colors[1].a = 0.9f;
			LevelLighting.init = false;
		}

		public static void save()
		{
			Block block = new Block();
			block.writeByte(LevelLighting.SAVEDATA_VERSION);
			block.writeSingle(LevelLighting.azimuth);
			block.writeSingle(LevelLighting.bias);
			block.writeSingle(LevelLighting.fade);
			block.writeSingle(LevelLighting.time);
			block.writeByte(LevelLighting.moon);
			block.writeSingle(LevelLighting.seaLevel);
			block.writeSingle(LevelLighting.snowLevel);
			block.writeBoolean(LevelLighting.canRain);
			block.writeBoolean(LevelLighting.canSnow);
			block.writeSingle(LevelLighting.rainFreq);
			block.writeSingle(LevelLighting.rainDur);
			block.writeSingle(LevelLighting.snowFreq);
			block.writeSingle(LevelLighting.snowDur);
			for (int i = 0; i < LevelLighting.times.Length; i++)
			{
				LightingInfo lightingInfo = LevelLighting.times[i];
				for (int j = 0; j < lightingInfo.colors.Length; j++)
				{
					block.writeColor(lightingInfo.colors[j]);
				}
				for (int k = 0; k < lightingInfo.singles.Length; k++)
				{
					block.writeSingle(lightingInfo.singles[k]);
				}
			}
			ReadWrite.writeBlock(Level.info.path + "/Environment/Lighting.dat", false, false, block);
		}

		public static void updateClouds()
		{
			LevelLighting.clouds.GetComponent<ParticleSystem>().Stop();
			LevelLighting.clouds.GetComponent<ParticleSystem>().Play();
		}

		private static void updateSea()
		{
			if (Level.info.configData.Use_Legacy_Water)
			{
				if (LevelLighting.seaLevel < 0.99f)
				{
					LevelLighting.water.position = new Vector3(0f, LevelLighting.seaLevel * Level.TERRAIN, 0f);
					LevelLighting.bubbles.gameObject.SetActive(true);
					LevelLighting.water.gameObject.SetActive(true);
				}
				else
				{
					LevelLighting.bubbles.gameObject.SetActive(false);
					LevelLighting.water.gameObject.SetActive(false);
				}
			}
			else
			{
				LevelLighting.bubbles.gameObject.SetActive(true);
				LevelLighting.water.gameObject.SetActive(false);
			}
			LevelLighting.bubbles.GetComponent<ParticleSystem>().Play();
		}

		public static void updateLocal()
		{
			LevelLighting.updateLocal(LevelLighting.localPoint, LevelLighting.localWindOverride, LevelLighting.localEffectNode);
		}

		public static void updateLocal(Vector3 point, float windOverride, IAmbianceNode effectNode)
		{
			LevelLighting.localPoint = point;
			LevelLighting.localWindOverride = windOverride;
			if (effectNode != LevelLighting.localEffectNode)
			{
				if (effectNode != null)
				{
					if (LevelLighting.localEffectNode == null || effectNode.id != LevelLighting.localEffectNode.id)
					{
						EffectAsset effectAsset = (EffectAsset)Assets.find(EAssetType.EFFECT, effectNode.id);
						if (effectAsset != null && effectAsset.effect != null)
						{
							AudioSource component = effectAsset.effect.GetComponent<AudioSource>();
							if (component != null)
							{
								LevelLighting.effectAudio.clip = component.clip;
								LevelLighting.effectAudio.Play();
								LevelLighting.localPlayingEffect = true;
							}
							else
							{
								LevelLighting.localPlayingEffect = false;
							}
						}
						else
						{
							LevelLighting.localPlayingEffect = false;
						}
					}
				}
				else
				{
					LevelLighting.localPlayingEffect = false;
				}
			}
			LevelLighting.localEffectNode = effectNode;
			if (LevelLighting.localEffectNode != null && LevelLighting.localEffectNode.noLighting && !Level.isEditor)
			{
				LevelLighting.localLightingBlend = Mathf.Lerp(LevelLighting.localLightingBlend, 1f, 0.25f * Time.deltaTime);
				LevelLighting.localBlendingLight = true;
			}
			else
			{
				LevelLighting.localLightingBlend = Mathf.Lerp(LevelLighting.localLightingBlend, 0f, 0.25f * Time.deltaTime);
				if (LevelLighting.localLightingBlend < 0.01f)
				{
					LevelLighting.localLightingBlend = 0f;
					LevelLighting.localBlendingLight = false;
				}
			}
			AmbianceVolume ambianceVolume = LevelLighting.localEffectNode as AmbianceVolume;
			if (ambianceVolume != null && ambianceVolume.overrideFog)
			{
				LevelLighting.localFogBlend = Mathf.Lerp(LevelLighting.localFogBlend, 1f, 0.05f * Time.deltaTime);
				LevelLighting.localBlendingFog = true;
				LevelLighting.localFogColor = ambianceVolume.fogColor;
				LevelLighting.localFogHeight = ambianceVolume.fogHeight;
			}
			else
			{
				LevelLighting.localFogBlend = Mathf.Lerp(LevelLighting.localFogBlend, 0f, 0.125f * Time.deltaTime);
				if (LevelLighting.localFogBlend < 0.01f)
				{
					LevelLighting.localFogBlend = 0f;
					LevelLighting.localBlendingFog = false;
				}
			}
			if (Level.info != null && Level.info.configData.Use_Rain_Volumes)
			{
				if (ambianceVolume != null && ambianceVolume.canRain)
				{
					LevelLighting.rainBlend = Mathf.Lerp(LevelLighting.rainBlend, 1f, 0.1f * Time.deltaTime);
				}
				else
				{
					LevelLighting.rainBlend = Mathf.Lerp(LevelLighting.rainBlend, 0f, 0.1f * Time.deltaTime);
					if (LevelLighting.rainBlend < 0.01f)
					{
						LevelLighting.rainBlend = 0f;
					}
				}
			}
			else
			{
				LevelLighting.rainBlend = 1f;
			}
			float num = 1f - (LevelLighting.snowLevel * Level.TERRAIN - point.y) / 32f;
			if (Level.info != null && Level.info.configData.Use_Snow_Volumes)
			{
				if (ambianceVolume != null && ambianceVolume.canSnow)
				{
					LevelLighting.snowBlend = Mathf.Lerp(LevelLighting.snowBlend, 1f, 0.1f * Time.deltaTime);
				}
				else if (LevelLighting.snowLevel > 0.01f && Level.info.configData.Use_Legacy_Snow_Height)
				{
					LevelLighting.snowBlend = Mathf.Lerp(LevelLighting.snowBlend, num, 0.1f * Time.deltaTime);
				}
				else
				{
					LevelLighting.snowBlend = Mathf.Lerp(LevelLighting.snowBlend, 0f, 0.1f * Time.deltaTime);
					if (LevelLighting.snowBlend < 0.01f)
					{
						LevelLighting.snowBlend = 0f;
					}
				}
			}
			else if (LevelLighting.snowLevel > 0.01f && Level.info != null && Level.info.configData.Use_Legacy_Snow_Height)
			{
				LevelLighting.snowBlend = Mathf.Lerp(LevelLighting.snowBlend, num, 0.1f * Time.deltaTime);
			}
			else
			{
				LevelLighting.snowBlend = 0f;
			}
			if (!LevelLighting.init)
			{
				LevelLighting.init = true;
				LevelLighting.updateSea();
				LevelLighting.updateLighting();
				LevelLighting.clouds.GetComponent<ParticleSystem>().Play();
				LevelLighting.bubbles.GetComponent<ParticleSystem>().Play();
				LevelLighting.snow.GetComponent<ParticleSystem>().Play();
				LevelLighting.rain.GetComponent<ParticleSystem>().Play();
			}
			LevelLighting.lighting.position = point;
			switch (LevelLighting.snowyness)
			{
			case ELightingSnow.NONE:
				LevelLighting.christmasyness = Mathf.Lerp(LevelLighting.christmasyness, 0f, 0.5f * Time.deltaTime);
				LevelLighting.blizzardyness = Mathf.Lerp(LevelLighting.blizzardyness, 0f, 0.5f * Time.deltaTime);
				break;
			case ELightingSnow.PRE_BLIZZARD:
				LevelLighting.christmasyness = Mathf.Lerp(LevelLighting.christmasyness, 0.5f, 0.2f * Time.deltaTime);
				LevelLighting.blizzardyness = Mathf.Lerp(LevelLighting.blizzardyness, 0f, 0.5f * Time.deltaTime);
				break;
			case ELightingSnow.BLIZZARD:
				LevelLighting.christmasyness = Mathf.Lerp(LevelLighting.christmasyness, 0.5f, 0.5f * Time.deltaTime);
				LevelLighting.blizzardyness = Mathf.Lerp(LevelLighting.blizzardyness, LevelLighting.snowBlend, 0.2f * Time.deltaTime);
				break;
			case ELightingSnow.POST_BLIZZARD:
				LevelLighting.christmasyness = Mathf.Lerp(LevelLighting.christmasyness, 0f, 0.2f * Time.deltaTime);
				LevelLighting.blizzardyness = Mathf.Lerp(LevelLighting.blizzardyness, 0f, 0.2f * Time.deltaTime);
				break;
			}
			switch (LevelLighting.rainyness)
			{
			case ELightingRain.NONE:
				LevelLighting.mistyness = Mathf.Lerp(LevelLighting.mistyness, 0f, 0.5f * Time.deltaTime);
				LevelLighting.drizzlyness = Mathf.Lerp(LevelLighting.drizzlyness, 0f, 0.5f * Time.deltaTime);
				break;
			case ELightingRain.PRE_DRIZZLE:
				LevelLighting.mistyness = Mathf.Lerp(LevelLighting.mistyness, 0.5f, 0.2f * Time.deltaTime);
				LevelLighting.drizzlyness = Mathf.Lerp(LevelLighting.drizzlyness, 0f, 0.5f * Time.deltaTime);
				break;
			case ELightingRain.DRIZZLE:
				LevelLighting.mistyness = Mathf.Lerp(LevelLighting.mistyness, 0.5f, 0.5f * Time.deltaTime);
				LevelLighting.drizzlyness = Mathf.Lerp(LevelLighting.drizzlyness, LevelLighting.rainBlend * (1f - LevelLighting.blizzardyness), 0.2f * Time.deltaTime);
				break;
			case ELightingRain.POST_DRIZZLE:
				LevelLighting.mistyness = Mathf.Lerp(LevelLighting.mistyness, 0f, 0.2f * Time.deltaTime);
				LevelLighting.drizzlyness = Mathf.Lerp(LevelLighting.drizzlyness, 0f, 0.2f * Time.deltaTime);
				break;
			}
			LevelLighting.skybox.SetColor("_SkyColor", LevelLighting.skyboxSky);
			LevelLighting.skybox.SetColor("_EquatorColor", LevelLighting.skyboxEquator);
			LevelLighting.skybox.SetColor("_GroundColor", LevelLighting.skyboxGround);
			Color color = Color.Lerp(new Color(0.8f, 0.8f, 0.8f), Color.black, LevelLighting.mistyness);
			color = Color.Lerp(color, Color.white, LevelLighting.christmasyness);
			Color color2 = Color.Lerp(LevelLighting.skyboxClouds, Color.black, LevelLighting.mistyness);
			color2 = Color.Lerp(color2, Color.white, LevelLighting.christmasyness);
			LevelLighting.mist.SetColor("_Color", color);
			LevelLighting.mist.SetColor("_RimColor", color2);
			if (MainCamera.instance != null)
			{
				MainCamera.instance.backgroundColor = LevelLighting.skyboxSky;
			}
			float num2 = WaterUtility.getWaterSurfaceElevation(point);
			if (!LevelLighting.enableUnderwaterEffects)
			{
				num2 = -1024f;
			}
			if (LevelLighting.enableUnderwaterEffects && WaterUtility.isPointUnderwater(point))
			{
				LevelLighting.waterAudio.volume = 0f;
				LevelLighting.belowAudio.volume = 1f;
				RenderSettings.fogColor = LevelLighting.sea.GetColor("_BaseColor");
				RenderSettings.fogDensity = 0.075f;
				if (MainCamera.instance != null)
				{
					MainCamera.instance.backgroundColor = RenderSettings.fogColor;
				}
				if (!LevelLighting.isSea)
				{
					RenderSettings.skybox = null;
					if (MainCamera.instance != null)
					{
						LevelLighting.areFXAllowed = false;
						SunShafts component2 = MainCamera.instance.GetComponent<SunShafts>();
						if (component2 != null)
						{
							component2.enabled = false;
						}
						LevelLighting.raysMultiplier = 0f;
						GlobalFog component3 = MainCamera.instance.GetComponent<GlobalFog>();
						if (component3 != null)
						{
							component3.enabled = false;
						}
						if (Player.player != null)
						{
							GlobalFog component4 = Player.player.look.scopeCamera.GetComponent<GlobalFog>();
							if (component4 != null)
							{
								component4.enabled = false;
							}
						}
					}
				}
				LevelLighting.isSea = true;
			}
			else
			{
				if (point.y < num2 + 8f && (LevelLighting.localEffectNode == null || !LevelLighting.localEffectNode.noWater))
				{
					LevelLighting.waterAudio.volume = Mathf.Lerp(0f, 0.25f, 1f - (point.y - num2) / 8f);
					LevelLighting.belowAudio.volume = 0f;
				}
				else
				{
					LevelLighting.waterAudio.volume = 0f;
					LevelLighting.belowAudio.volume = 0f;
				}
				if (LevelLighting.isSea)
				{
					RenderSettings.skybox = LevelLighting.skybox;
					RenderSettings.fogDensity = 0f;
					if (MainCamera.instance != null)
					{
						LevelLighting.areFXAllowed = true;
						SunShafts component5 = MainCamera.instance.GetComponent<SunShafts>();
						if (component5 != null)
						{
							component5.enabled = (GraphicsSettings.sunShaftsQuality != EGraphicQuality.OFF);
						}
						LevelLighting.raysMultiplier = 1f;
						GlobalFog component6 = MainCamera.instance.GetComponent<GlobalFog>();
						if (component6 != null)
						{
							component6.enabled = GraphicsSettings.fog;
						}
						if (Player.player != null)
						{
							GlobalFog component7 = Player.player.look.scopeCamera.GetComponent<GlobalFog>();
							if (component7 != null)
							{
								component7.enabled = GraphicsSettings.fog;
							}
						}
					}
				}
				LevelLighting.isSea = false;
			}
			Color color3 = LevelLighting.sunFlare.GetComponent<Renderer>().material.GetColor("_Color");
			color3.a = 1f - Mathf.Max(LevelLighting.blizzardyness, LevelLighting.drizzlyness);
			LevelLighting.sunFlare.GetComponent<Renderer>().material.SetColor("_Color", color3);
			Color color4 = LevelLighting.moonFlare.GetComponent<Renderer>().material.GetColor("_Color");
			color4.a = color3.a;
			LevelLighting.moonFlare.GetComponent<Renderer>().material.SetColor("_Color", color4);
			if (!LevelLighting.isSea)
			{
				RenderSettings.fogColor = LevelLighting.skyboxSky;
				RenderSettings.fogDensity = Mathf.Pow(Mathf.Max(LevelLighting.blizzardyness, LevelLighting.drizzlyness), 3f) * 0.005f;
			}
			if (MainCamera.instance != null && RenderSettings.fogDensity < 0.0299f)
			{
				LevelLighting.raysMultiplier = 1f - Mathf.Max(LevelLighting.blizzardyness, LevelLighting.drizzlyness);
				GlobalFog component8 = MainCamera.instance.GetComponent<GlobalFog>();
				if (component8 != null)
				{
					component8.globalDensity = 0.005f * (1f - Mathf.Max(LevelLighting.blizzardyness, LevelLighting.drizzlyness));
				}
				if (Player.player != null)
				{
					GlobalFog component9 = Player.player.look.scopeCamera.GetComponent<GlobalFog>();
					if (component9 != null)
					{
						component9.globalDensity = 0.005f * (1f - Mathf.Max(LevelLighting.blizzardyness, LevelLighting.drizzlyness));
					}
				}
			}
			LevelLighting.auroraBorealisCurrentIntensity = Mathf.Clamp01(Mathf.Lerp(LevelLighting.auroraBorealisCurrentIntensity, Mathf.Min(1f - LevelLighting.blizzardyness, LevelLighting.auroraBorealisTargetIntensity), 0.1f * Time.deltaTime));
			LevelLighting.updateAuroraBorealis(LevelLighting.auroraBorealisCurrentIntensity);
			if (LevelLighting.blizzardyness > 0.01f)
			{
				LevelLighting.windAudio.volume = Mathf.Lerp(windOverride, 1f, LevelLighting.blizzardyness);
				LevelLighting.snow.GetComponent<ParticleSystem>().emission.rateOverTime = Mathf.Pow(LevelLighting.blizzardyness, 2f) * 1024f;
				LevelLighting.snow.GetComponent<ParticleSystem>().main.startColor = LevelLighting.rainColor;
				LevelLighting.skybox.SetColor("_SkyColor", LevelLighting.skyboxSky);
				LevelLighting.skybox.SetColor("_EquatorColor", Color.Lerp(LevelLighting.skyboxEquator, LevelLighting.skyboxSky, LevelLighting.blizzardyness));
				LevelLighting.skybox.SetColor("_GroundColor", Color.Lerp(LevelLighting.skyboxGround, LevelLighting.skyboxSky, LevelLighting.blizzardyness));
				if (MainCamera.instance != null && RenderSettings.fogDensity < 0.0299f && LevelLighting.blizzardyness > 0.99f)
				{
					if (LevelLighting.areFXAllowed)
					{
						SunShafts component10 = MainCamera.instance.GetComponent<SunShafts>();
						if (component10 != null)
						{
							component10.enabled = false;
						}
					}
					LevelLighting.areFXAllowed = false;
				}
				LevelLighting.isSnow = true;
			}
			else
			{
				LevelLighting.windAudio.volume = windOverride;
				LevelLighting.snow.GetComponent<ParticleSystem>().emission.rateOverTime = 0f;
				if (LevelLighting.isSnow && MainCamera.instance != null)
				{
					LevelLighting.areFXAllowed = true;
					SunShafts component11 = MainCamera.instance.GetComponent<SunShafts>();
					if (component11 != null)
					{
						component11.enabled = (GraphicsSettings.sunShaftsQuality != EGraphicQuality.OFF);
					}
				}
				LevelLighting.isSnow = false;
			}
			Shader.SetGlobalColor("_AlphaParticleLightingColor", LevelLighting.rainColor);
			if (LevelLighting.drizzlyness > 0.01f)
			{
				LevelLighting.rain.GetComponent<ParticleSystem>().emission.rateOverTime = Mathf.Pow(LevelLighting.drizzlyness, 2f) * 2048f;
				LevelLighting.skybox.SetColor("_SkyColor", LevelLighting.skyboxSky);
				LevelLighting.skybox.SetColor("_EquatorColor", Color.Lerp(LevelLighting.skyboxEquator, LevelLighting.skyboxSky, LevelLighting.drizzlyness));
				LevelLighting.skybox.SetColor("_GroundColor", Color.Lerp(LevelLighting.skyboxGround, LevelLighting.skyboxSky, LevelLighting.drizzlyness));
			}
			else
			{
				LevelLighting.rain.GetComponent<ParticleSystem>().emission.rateOverTime = 0f;
			}
			if (LevelLighting.puddles != null)
			{
				if (LevelLighting.rainyness == ELightingRain.DRIZZLE)
				{
					LevelLighting.puddles.Water_Level = Mathf.Lerp(LevelLighting.puddles.Water_Level, LevelLighting.drizzlyness * 0.75f, 0.2f * Time.deltaTime);
				}
				else
				{
					LevelLighting.puddles.Water_Level = Mathf.Lerp(LevelLighting.puddles.Water_Level, 0f, 0.025f * Time.deltaTime);
				}
				LevelLighting.puddles.Intensity = LevelLighting.drizzlyness * 2f;
			}
			if (Time.time > LevelLighting.nextAudioVolumeChangeTime)
			{
				LevelLighting.nextAudioVolumeChangeTime = Time.time + (float)Random.Range(15, 60);
				LevelLighting.targetAudioVolume = Random.Range(LevelLighting.AUDIO_MIN, LevelLighting.AUDIO_MAX);
			}
			LevelLighting.currentAudioVolume = Mathf.Lerp(LevelLighting.currentAudioVolume, LevelLighting.targetAudioVolume, 0.1f * Time.deltaTime);
			LevelLighting.effectAudio.volume = Mathf.Lerp(LevelLighting.effectAudio.volume, (float)((!LevelLighting.localPlayingEffect) ? 0 : 1), (!Level.isEditor) ? (0.5f * Time.deltaTime) : 1f);
			LevelLighting.rainAudio.volume = Mathf.Lerp(LevelLighting.rainAudio.volume, LevelLighting.drizzlyness * (1f - LevelLighting.effectAudio.volume), 0.5f * Time.deltaTime);
			LevelLighting.dayAudio.volume = Mathf.Lerp(LevelLighting.dayAudio.volume, LevelLighting.dayVolume * LevelLighting.currentAudioVolume * (1f - LevelLighting.waterAudio.volume * 4f) * (1f - LevelLighting.belowAudio.volume) * (1f - LevelLighting.windAudio.volume) * (1f - LevelLighting.rainAudio.volume) * (1f - LevelLighting.effectAudio.volume), 0.5f * Time.deltaTime);
			LevelLighting.nightAudio.volume = Mathf.Lerp(LevelLighting.nightAudio.volume, LevelLighting.nightVolume * LevelLighting.currentAudioVolume * (1f - LevelLighting.waterAudio.volume * 4f) * (1f - LevelLighting.belowAudio.volume) * (1f - LevelLighting.windAudio.volume) * (1f - LevelLighting.rainAudio.volume) * (1f - LevelLighting.effectAudio.volume), 0.5f * Time.deltaTime);
			LevelLighting.snow.rotation = Quaternion.Slerp(LevelLighting.snow.rotation, Quaternion.Euler(45f, LevelLighting.wind, 0f), 0.5f * Time.deltaTime);
			LevelLighting.snow.position = point + LevelLighting.snow.forward * -32f;
			LevelLighting.rain.rotation = Quaternion.Slerp(LevelLighting.rain.rotation, Quaternion.Euler(75f, LevelLighting.wind, 0f), 0.5f * Time.deltaTime);
			LevelLighting.rain.position = point + LevelLighting.rain.forward * -32f;
			LevelLighting.windZone.transform.rotation = Quaternion.Slerp(LevelLighting.windZone.transform.rotation, Quaternion.Euler(0f, LevelLighting.wind, 0f), 0.5f * Time.deltaTime);
			LevelLighting.windZone.windMain = Mathf.Lerp(LevelLighting.windZone.windMain, (!LevelLighting.isSnow) ? 0.15f : 0.8f, 0.5f * Time.deltaTime);
			point.y = Mathf.Min(point.y - 16f, num2 - 32f);
			LevelLighting.bubbles.position = point;
			if (!LevelLighting.skyboxUpdated)
			{
				LevelLighting.skyboxUpdated = true;
				if (LevelLighting.vision != ELightingVision.CIVILIAN && LevelLighting.vision != ELightingVision.MILITARY && !LevelLighting.localBlendingLight)
				{
					if (Provider.preferenceData != null && Provider.preferenceData.Graphics.Use_Skybox_Ambience)
					{
						RenderSettings.ambientMode = 0;
						DynamicGI.UpdateEnvironment();
					}
					else
					{
						RenderSettings.ambientMode = 1;
					}
				}
				LevelLighting.isReflectionBuilding = true;
				LevelLighting.isReflectionBuildingVision = true;
			}
			LevelLighting.updateSkyboxReflections();
		}

		private static void renderSkyboxReflection(Cubemap target, ref int index, ref bool isBuilding)
		{
			if (!isBuilding)
			{
				return;
			}
			if (target == null || LevelLighting.reflectionCamera == null)
			{
				return;
			}
			int num = 1 << index;
			index++;
			if (index > 5)
			{
				index = 0;
				isBuilding = false;
			}
			LevelLighting.reflectionCamera.RenderToCubemap(target, num);
		}

		public static void updateSkyboxReflections()
		{
			if (LevelLighting.isSkyboxReflectionEnabled)
			{
				if (LevelLighting.vision == ELightingVision.NONE)
				{
					LevelLighting.renderSkyboxReflection(LevelLighting.reflectionMap, ref LevelLighting.reflectionIndex, ref LevelLighting.isReflectionBuilding);
					RenderSettings.customReflection = LevelLighting.reflectionMap;
				}
				else
				{
					LevelLighting.renderSkyboxReflection(LevelLighting.reflectionMapVision, ref LevelLighting.reflectionIndexVision, ref LevelLighting.isReflectionBuildingVision);
					RenderSettings.customReflection = LevelLighting.reflectionMapVision;
				}
			}
			else
			{
				RenderSettings.customReflection = null;
			}
		}

		private static void updateStars(float cutoff)
		{
			if (!Level.info.configData.Has_Atmosphere)
			{
				cutoff = 0.05f;
			}
			LevelLighting.stars.GetComponent<Renderer>().material.SetFloat("_Cutoff", cutoff);
		}

		private static void updateAuroraBorealis(float intensity)
		{
			LevelLighting.auroraBorealisMaterial.SetFloat("_Intensity", intensity);
		}

		private static bool _enableUnderwaterEffects = true;

		public static readonly byte SAVEDATA_VERSION = 11;

		public static readonly byte MOON_CYCLES = 5;

		public static readonly float CLOUDS = 2f;

		public static readonly float AUDIO_MIN = 0.075f;

		public static readonly float AUDIO_MAX = 0.15f;

		private static readonly Color FOAM_DAWN = new Color(0.125f, 0f, 0f, 0f);

		private static readonly Color FOAM_MIDDAY = new Color(0.25f, 0f, 0f, 0f);

		private static readonly Color FOAM_DUSK = new Color(0.05f, 0f, 0f, 0f);

		private static readonly Color FOAM_MIDNIGHT = new Color(0.01f, 0f, 0f, 0f);

		private static readonly float SPECULAR_DAWN = 5f;

		private static readonly float SPECULAR_MIDDAY = 50f;

		private static readonly float SPECULAR_DUSK = 5f;

		private static readonly float SPECULAR_MIDNIGHT = 50f;

		private static readonly float PITCH_DARK_WATER_BLEND = 0.9f;

		private static readonly float REFLECTION_DAWN = 0.75f;

		private static readonly float REFLECTION_MIDDAY = 0.75f;

		private static readonly float REFLECTION_DUSK = 0.5f;

		private static readonly float REFLECTION_MIDNIGHT = 0.5f;

		private static readonly Color NIGHTVISION_MILITARY = new Color(0f, 1f, 0f, 0f);

		private static readonly Color NIGHTVISION_CIVILIAN = new Color(0.25f, 0.25f, 0.25f, 0f);

		private static float _azimuth;

		private static float _transition;

		private static float _bias;

		private static float _fade;

		private static float _time;

		private static float _wind;

		public static ELightingRain rainyness;

		public static ELightingSnow snowyness;

		private static byte[] _hash;

		private static LightingInfo[] _times;

		private static float _seaLevel;

		private static float _snowLevel;

		public static float rainFreq;

		public static float rainDur;

		public static float snowFreq;

		public static float snowDur;

		public static bool canRain;

		public static bool canSnow;

		public static ELightingVision vision;

		protected static bool _isSea;

		private static bool isSnow;

		private static Material skybox;

		private static Material mist;

		private static Transform lighting;

		private static Rain puddles;

		private static Transform auroraBorealisTransform;

		private static Material auroraBorealisMaterial;

		private static float auroraBorealisCurrentIntensity;

		private static float auroraBorealisTargetIntensity;

		private static Color skyboxSky;

		private static Color skyboxEquator;

		private static Color skyboxGround;

		private static Color skyboxClouds;

		private static bool skyboxUpdated;

		private static float lastSkyboxUpdate;

		private static Color rainColor;

		private static Color raysColor;

		private static float raysIntensity;

		private static float raysMultiplier;

		public static Transform sun;

		private static Transform sunFlare;

		private static Transform moonFlare;

		private static AudioSource _effectAudio;

		private static AudioSource _dayAudio;

		private static AudioSource _nightAudio;

		private static AudioSource _waterAudio;

		private static AudioSource _windAudio;

		private static AudioSource _belowAudio;

		private static AudioSource _rainAudio;

		private static float currentAudioVolume;

		private static float targetAudioVolume;

		private static float nextAudioVolumeChangeTime;

		private static float dayVolume;

		private static float nightVolume;

		private static Transform stars;

		private static Camera reflectionCamera;

		private static Cubemap reflectionMap;

		private static Cubemap reflectionMapVision;

		private static int reflectionIndex;

		private static int reflectionIndexVision;

		private static bool isReflectionBuilding;

		private static bool isReflectionBuildingVision;

		private static bool _isSkyboxReflectionEnabled;

		private static Transform _bubbles;

		private static Transform _snow;

		private static Transform _rain;

		private static WindZone _windZone;

		private static Transform _clouds;

		private static Material _sea;

		private static PlanarReflection _reflection;

		public static bool areFXAllowed = true;

		private static Transform water;

		private static Material[] moons;

		private static byte _moon;

		private static bool init;

		private static Vector3 localPoint;

		private static float localWindOverride;

		private static IAmbianceNode localEffectNode;

		private static bool localPlayingEffect;

		private static bool localBlendingLight;

		private static float localLightingBlend;

		private static bool localBlendingFog;

		private static float localFogBlend;

		private static Color localFogColor;

		private static float localFogHeight;

		private static float rainBlend;

		private static float snowBlend;

		public delegate void IsSeaChangedHandler(bool isSea);
	}
}
