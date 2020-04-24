using System;
using System.Collections;
using System.Collections.Generic;
using SDG.Framework.Devkit;
using SDG.Framework.Devkit.Transactions;
using SDG.Framework.Water;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

namespace SDG.Unturned
{
	public class Level : MonoBehaviour
	{
		public static ushort border
		{
			get
			{
				if (Level.info == null)
				{
					return 1;
				}
				if (Level.info.size == ELevelSize.TINY)
				{
					return Level.TINY_BORDER;
				}
				if (Level.info.size == ELevelSize.SMALL)
				{
					return Level.SMALL_BORDER;
				}
				if (Level.info.size == ELevelSize.MEDIUM)
				{
					return Level.MEDIUM_BORDER;
				}
				if (Level.info.size == ELevelSize.LARGE)
				{
					return Level.LARGE_BORDER;
				}
				if (Level.info.size == ELevelSize.INSANE)
				{
					return Level.INSANE_BORDER;
				}
				return 0;
			}
		}

		public static ushort size
		{
			get
			{
				if (Level.info == null)
				{
					return 8;
				}
				if (Level.info.size == ELevelSize.TINY)
				{
					return Level.TINY_SIZE;
				}
				if (Level.info.size == ELevelSize.SMALL)
				{
					return Level.SMALL_SIZE;
				}
				if (Level.info.size == ELevelSize.MEDIUM)
				{
					return Level.MEDIUM_SIZE;
				}
				if (Level.info.size == ELevelSize.LARGE)
				{
					return Level.LARGE_SIZE;
				}
				if (Level.info.size == ELevelSize.INSANE)
				{
					return Level.INSANE_SIZE;
				}
				return 0;
			}
		}

		public static ushort vehicles
		{
			get
			{
				if (Level.info.size == ELevelSize.TINY)
				{
					return Level.TINY_VEHICLES;
				}
				if (Level.info.size == ELevelSize.SMALL)
				{
					return Level.SMALL_VEHICLES;
				}
				if (Level.info.size == ELevelSize.MEDIUM)
				{
					return Level.MEDIUM_VEHICLES;
				}
				if (Level.info.size == ELevelSize.LARGE)
				{
					return Level.LARGE_VEHICLES;
				}
				if (Level.info.size == ELevelSize.INSANE)
				{
					return Level.INSANE_VEHICLES;
				}
				return 0;
			}
		}

		public static ushort animals
		{
			get
			{
				if (Level.info.size == ELevelSize.TINY)
				{
					return Level.TINY_ANIMALS;
				}
				if (Level.info.size == ELevelSize.SMALL)
				{
					return Level.SMALL_ANIMALS;
				}
				if (Level.info.size == ELevelSize.MEDIUM)
				{
					return Level.MEDIUM_ANIMALS;
				}
				if (Level.info.size == ELevelSize.LARGE)
				{
					return Level.LARGE_ANIMALS;
				}
				if (Level.info.size == ELevelSize.INSANE)
				{
					return Level.INSANE_ANIMALS;
				}
				return 0;
			}
		}

		public static float fog
		{
			get
			{
				if (Level.info != null)
				{
					if (Level.info.size == ELevelSize.TINY)
					{
						return Level.TINY_FOG;
					}
					if (Level.info.size == ELevelSize.SMALL)
					{
						return Level.SMALL_FOG;
					}
					if (Level.info.size == ELevelSize.MEDIUM)
					{
						return Level.MEDIUM_FOG;
					}
					if (Level.info.size == ELevelSize.LARGE)
					{
						return Level.LARGE_FOG;
					}
					if (Level.info.size == ELevelSize.INSANE)
					{
						return Level.INSANE_FOG;
					}
				}
				return 1f;
			}
		}

		public static bool checkSafeIncludingClipVolumes(Vector3 point)
		{
			if (Level.info != null && !Level.info.configData.Use_Legacy_Clip_Borders)
			{
				return !PlayerClipVolumeUtility.isPointInsideVolume(point);
			}
			return point.x > (float)(-Level.size / 2 + Level.border) && point.y > 0f && point.z > (float)(-Level.size / 2 + Level.border) && point.x < (float)(Level.size / 2 - Level.border) && point.y < Level.HEIGHT && point.z < (float)(Level.size / 2 - Level.border);
		}

		public static bool checkSafe(Vector3 point)
		{
			return (Level.info != null && !Level.info.configData.Use_Legacy_Clip_Borders) || (point.x > (float)(-Level.size / 2 + Level.border) && point.y > 0f && point.z > (float)(-Level.size / 2 + Level.border) && point.x < (float)(Level.size / 2 - Level.border) && point.y < Level.HEIGHT && point.z < (float)(Level.size / 2 - Level.border));
		}

		public static bool checkLevel(Vector3 point)
		{
			return point.x > (float)(-Level.size / 2) && point.y > 0f && point.z > (float)(-Level.size / 2) && point.x < (float)(Level.size / 2) && point.y < Level.HEIGHT && point.z < (float)(Level.size / 2);
		}

		public static event LevelLoadingStepHandler loadingSteps;

		public static LevelInfo info
		{
			get
			{
				return Level._info;
			}
		}

		public static Transform level
		{
			get
			{
				return Level._level;
			}
		}

		public static Transform roots
		{
			get
			{
				return Level._roots;
			}
		}

		public static Transform clips
		{
			get
			{
				return Level._clips;
			}
		}

		public static Transform effects
		{
			get
			{
				return Level._effects;
			}
		}

		public static Transform spawns
		{
			get
			{
				return Level._spawns;
			}
		}

		public static Transform editing
		{
			get
			{
				return Level._editing;
			}
		}

		public static bool isInitialized
		{
			get
			{
				return Level._isInitialized;
			}
		}

		public static bool isEditor
		{
			get
			{
				return Level._isEditor;
			}
		}

		public static bool isExiting { get; protected set; }

		public static bool isDevkit
		{
			get
			{
				return Level._isDevkit;
			}
		}

		public static bool isVR
		{
			get
			{
				return PlaySettings.isVR && Level.isEditor;
			}
		}

		public static bool isLoading
		{
			get
			{
				if (Provider.isConnected)
				{
					return Level.isLoadingContent || Level.isLoadingLighting || Level.isLoadingVehicles || Level.isLoadingBarricades || Level.isLoadingStructures || Level.isLoadingArea;
				}
				return Level.isEditor && Level.isLoadingContent;
			}
		}

		public static bool isLoaded
		{
			get
			{
				return Level._isLoaded;
			}
		}

		public static byte[] hash
		{
			get
			{
				return Level._hash;
			}
		}

		public static void setEnabled(bool isEnabled)
		{
			Level.clips.gameObject.SetActive(isEnabled);
		}

		public static void add(string name, ELevelSize size, ELevelType type)
		{
			if (!ReadWrite.folderExists("/Maps/" + name))
			{
				ReadWrite.createFolder("/Maps/" + name);
				Block block = new Block();
				block.writeByte(Level.SAVEDATA_VERSION);
				block.writeSteamID(Provider.client);
				block.writeByte((byte)size);
				block.writeByte((byte)type);
				ReadWrite.writeBlock("/Maps/" + name + "/Level.dat", false, block);
				ReadWrite.copyFile("/Bundles/Level/Charts.unity3d", "/Maps/" + name + "/Charts.unity3d");
				ReadWrite.copyFile("/Bundles/Level/Details.unity3d", "/Maps/" + name + "/Terrain/Details.unity3d");
				ReadWrite.copyFile("/Bundles/Level/Details.dat", "/Maps/" + name + "/Terrain/Details.dat");
				ReadWrite.copyFile("/Bundles/Level/Materials.unity3d", "/Maps/" + name + "/Terrain/Materials.unity3d");
				ReadWrite.copyFile("/Bundles/Level/Materials.dat", "/Maps/" + name + "/Terrain/Materials.dat");
				ReadWrite.copyFile("/Bundles/Level/Resources.dat", "/Maps/" + name + "/Terrain/Resources.dat");
				ReadWrite.copyFile("/Bundles/Level/Lighting.dat", "/Maps/" + name + "/Environment/Lighting.dat");
				ReadWrite.copyFile("/Bundles/Level/Roads.unity3d", "/Maps/" + name + "/Environment/Roads.unity3d");
				ReadWrite.copyFile("/Bundles/Level/Roads.dat", "/Maps/" + name + "/Environment/Roads.dat");
				ReadWrite.copyFile("/Bundles/Level/Ambience.unity3d", "/Maps/" + name + "/Environment/Ambience.unity3d");
				if (Level.onLevelsRefreshed != null)
				{
					Level.onLevelsRefreshed();
				}
			}
		}

		public static void remove(string name)
		{
			ReadWrite.deleteFolder("/Maps/" + name);
			if (Level.onLevelsRefreshed != null)
			{
				Level.onLevelsRefreshed();
			}
		}

		public static void save()
		{
			LevelObjects.save();
			LevelLighting.save();
			LevelGround.save();
			LevelRoads.save();
			if (!Level.isVR)
			{
				LevelNavigation.save();
				LevelNodes.save();
				LevelItems.save();
				LevelPlayers.save();
				LevelZombies.save();
				LevelVehicles.save();
				LevelAnimals.save();
				LevelVisibility.save();
			}
			Editor.save();
		}

		public static void edit(LevelInfo newInfo, bool Devkit)
		{
			Level._isEditor = true;
			Level._isDevkit = Devkit;
			Level.isExiting = false;
			Level._info = newInfo;
			LoadingUI.updateScene();
			SceneManager.LoadScene("Game");
			Provider.updateRichPresence();
			DevkitTransactionManager.resetTransactions();
		}

		public static void load(LevelInfo newInfo)
		{
			Level._isEditor = false;
			Level._isDevkit = false;
			Level.isExiting = false;
			Level._info = newInfo;
			LoadingUI.updateScene();
			SceneManager.LoadScene("Game");
			if (!Dedicator.isDedicated)
			{
				string text = Level.info.name.ToLower();
				if (text != null)
				{
					if (!(text == "germany"))
					{
						if (!(text == "hawaii"))
						{
							if (!(text == "pei"))
							{
								if (!(text == "russia"))
								{
									if (!(text == "yukon"))
									{
										if (text == "washington")
										{
											Provider.provider.achievementsService.setAchievement("Washington");
										}
									}
									else
									{
										Provider.provider.achievementsService.setAchievement("Yukon");
									}
								}
								else
								{
									Provider.provider.achievementsService.setAchievement("Russia");
								}
							}
							else
							{
								Provider.provider.achievementsService.setAchievement("PEI");
							}
						}
						else
						{
							Provider.provider.achievementsService.setAchievement("Hawaii");
						}
					}
					else
					{
						Provider.provider.achievementsService.setAchievement("Peaks");
					}
				}
			}
			Provider.updateRichPresence();
			DevkitTransactionManager.resetTransactions();
		}

		public static void loading()
		{
			SceneManager.LoadScene("Loading");
		}

		public static void exit()
		{
			if (Level.onLevelExited != null)
			{
				Level.onLevelExited();
			}
			if (!Level.isEditor && Player.player != null && PlayerUI.window != null && Level.info != null && PlayerUI.window.totalTime > 60f)
			{
				int fpsMin = PlayerUI.window.fpsMin;
				int fpsMax = PlayerUI.window.fpsMax;
				int num = (int)((float)PlayerUI.window.totalFrames / PlayerUI.window.totalTime);
				string value = (!Level.info.canAnalyticsTrack) ? "Workshop" : Level.info.name;
				Dictionary<string, object> dictionary = new Dictionary<string, object>
				{
					{
						"FPS_Min",
						fpsMin
					},
					{
						"FPS_Max",
						fpsMax
					},
					{
						"FPS_Avg",
						num
					},
					{
						"Map",
						value
					},
					{
						"Network",
						Provider.clients.Count > 1
					}
				};
				Analytics.CustomEvent("Perf", dictionary);
			}
			Level._isEditor = false;
			Level._isDevkit = false;
			Level.isExiting = true;
			Level._info = null;
			LoadingUI.updateScene();
			SceneManager.LoadScene("Menu");
			Provider.updateRichPresence();
			DevkitTransactionManager.resetTransactions();
		}

		public static bool exists(string name)
		{
			if (ReadWrite.folderExists("/Maps/" + name))
			{
				return true;
			}
			if (Provider.provider.workshopService.ugc != null)
			{
				for (int i = 0; i < Provider.provider.workshopService.ugc.Count; i++)
				{
					SteamContent steamContent = Provider.provider.workshopService.ugc[i];
					if (steamContent.type == ESteamUGCType.MAP && ReadWrite.folderExists(steamContent.path + "/" + name, false))
					{
						return true;
					}
				}
			}
			else
			{
				string[] folders = ReadWrite.getFolders("/Bundles/Workshop/Maps");
				for (int j = 0; j < folders.Length; j++)
				{
					if (ReadWrite.folderExists(folders[j] + "/" + name, false))
					{
						return true;
					}
				}
				string[] folders2 = ReadWrite.getFolders(ServerSavedata.directory + "/" + Provider.serverID + "/Workshop/Maps");
				for (int k = 0; k < folders2.Length; k++)
				{
					if (ReadWrite.folderExists(folders2[k] + "/" + name, false))
					{
						return true;
					}
				}
				if (ReadWrite.folderExists(string.Concat(new string[]
				{
					ServerSavedata.directory,
					"/",
					Provider.serverID,
					"/Maps/",
					name
				})))
				{
					return true;
				}
			}
			return false;
		}

		public static byte[] getLevelHash(string path)
		{
			return Level.getLevelHash(path, true);
		}

		public static byte[] getLevelHash(string path, bool usePath)
		{
			if (ReadWrite.fileExists(path + "/Level.dat", false, usePath))
			{
				Block block = ReadWrite.readBlock(path + "/Level.dat", false, usePath, 1);
				return block.getHash();
			}
			return new byte[20];
		}

		public static LevelInfo getLevel(string name)
		{
			if (ReadWrite.folderExists("/Maps/" + name))
			{
				return Level.getLevel("/Maps/" + name, true);
			}
			if (Provider.provider.workshopService.ugc != null)
			{
				for (int i = 0; i < Provider.provider.workshopService.ugc.Count; i++)
				{
					SteamContent steamContent = Provider.provider.workshopService.ugc[i];
					if (steamContent.type == ESteamUGCType.MAP && ReadWrite.folderExists(steamContent.path + "/" + name, false))
					{
						return Level.getLevel(steamContent.path + "/" + name, false);
					}
				}
			}
			else
			{
				string[] folders = ReadWrite.getFolders("/Bundles/Workshop/Maps");
				for (int j = 0; j < folders.Length; j++)
				{
					if (ReadWrite.folderExists(folders[j] + "/" + name, false))
					{
						return Level.getLevel(folders[j] + "/" + name, false);
					}
				}
				string[] folders2 = ReadWrite.getFolders(ServerSavedata.directory + "/" + Provider.serverID + "/Workshop/Maps");
				for (int k = 0; k < folders2.Length; k++)
				{
					if (ReadWrite.folderExists(folders2[k] + "/" + name, false))
					{
						return Level.getLevel(folders2[k] + "/" + name, false);
					}
				}
				if (ReadWrite.folderExists(string.Concat(new string[]
				{
					ServerSavedata.directory,
					"/",
					Provider.serverID,
					"/Maps/",
					name
				})))
				{
					return Level.getLevel(string.Concat(new string[]
					{
						ServerSavedata.directory,
						"/",
						Provider.serverID,
						"/Maps/",
						name
					}), true);
				}
			}
			return null;
		}

		public static LevelInfo getLevel(string path, bool usePath)
		{
			if (ReadWrite.fileExists(path + "/Level.dat", false, usePath))
			{
				Block block = ReadWrite.readBlock(path + "/Level.dat", false, usePath, 0);
				byte b = block.readByte();
				bool newEditable = block.readSteamID() == Provider.client || ReadWrite.fileExists(path + "/.unlocker", false, usePath);
				ELevelSize newSize = (ELevelSize)block.readByte();
				ELevelType newType = ELevelType.SURVIVAL;
				if (b > 1)
				{
					newType = (ELevelType)block.readByte();
				}
				LevelInfoConfigData levelInfoConfigData;
				if (ReadWrite.fileExists(path + "/Config.json", false, usePath))
				{
					try
					{
						levelInfoConfigData = ReadWrite.deserializeJSON<LevelInfoConfigData>(path + "/Config.json", false, usePath);
					}
					catch
					{
						levelInfoConfigData = null;
					}
					if (levelInfoConfigData == null)
					{
						levelInfoConfigData = new LevelInfoConfigData();
					}
				}
				else
				{
					levelInfoConfigData = new LevelInfoConfigData();
				}
				return new LevelInfo((!usePath) ? path : (ReadWrite.PATH + path), ReadWrite.folderName(path), newSize, newType, newEditable, levelInfoConfigData);
			}
			return null;
		}

		private static bool doesLevelPassFilter(LevelInfo levelInfo, ESingleplayerMapCategory categoryFilter)
		{
			if (categoryFilter == ESingleplayerMapCategory.ALL)
			{
				return true;
			}
			if (categoryFilter == ESingleplayerMapCategory.EDITABLE)
			{
				if (!levelInfo.isEditable)
				{
					return false;
				}
			}
			else if (categoryFilter == ESingleplayerMapCategory.WORKSHOP)
			{
				if (!levelInfo.isFromWorkshop)
				{
					return false;
				}
			}
			else if (categoryFilter == ESingleplayerMapCategory.MATCHMAKING)
			{
				if (!levelInfo.configData.Visible_In_Matchmaking)
				{
					return false;
				}
			}
			else if (categoryFilter != levelInfo.configData.Category)
			{
				return false;
			}
			return true;
		}

		public static LevelInfo[] getLevels(ESingleplayerMapCategory categoryFilter)
		{
			List<LevelInfo> list = new List<LevelInfo>();
			string[] folders = ReadWrite.getFolders("/Maps");
			for (int i = 0; i < folders.Length; i++)
			{
				LevelInfo level = Level.getLevel(folders[i], false);
				if (level != null && !(level.name.ToLower() == "tutorial"))
				{
					if (Level.doesLevelPassFilter(level, categoryFilter))
					{
						list.Add(level);
					}
				}
			}
			if (Provider.provider.workshopService.ugc != null)
			{
				for (int j = 0; j < Provider.provider.workshopService.ugc.Count; j++)
				{
					SteamContent steamContent = Provider.provider.workshopService.ugc[j];
					if (steamContent.type == ESteamUGCType.MAP)
					{
						LevelInfo level2 = Level.getLevel(ReadWrite.folderFound(steamContent.path, false), false);
						if (level2 != null)
						{
							level2.isFromWorkshop = true;
							if (Level.doesLevelPassFilter(level2, categoryFilter))
							{
								list.Add(level2);
							}
						}
					}
				}
			}
			else
			{
				string[] folders2 = ReadWrite.getFolders("/Bundles/Workshop/Maps");
				for (int k = 0; k < folders2.Length; k++)
				{
					LevelInfo level3 = Level.getLevel(folders2[k], false);
					if (level3 != null)
					{
						level3.isFromWorkshop = true;
						if (Level.doesLevelPassFilter(level3, categoryFilter))
						{
							list.Add(level3);
						}
					}
				}
				string[] folders3 = ReadWrite.getFolders(ServerSavedata.directory + "/" + Provider.serverID + "/Workshop/Maps");
				for (int l = 0; l < folders3.Length; l++)
				{
					LevelInfo level4 = Level.getLevel(folders3[l], false);
					if (level4 != null)
					{
						level4.isFromWorkshop = true;
						if (Level.doesLevelPassFilter(level4, categoryFilter))
						{
							list.Add(level4);
						}
					}
				}
				folders = ReadWrite.getFolders(ServerSavedata.directory + "/" + Provider.serverID + "/Maps");
				for (int m = 0; m < folders.Length; m++)
				{
					LevelInfo level5 = Level.getLevel(folders[m], false);
					if (level5 != null && !(level5.name.ToLower() == "tutorial"))
					{
						level5.isFromWorkshop = true;
						if (Level.doesLevelPassFilter(level5, categoryFilter))
						{
							list.Add(level5);
						}
					}
				}
			}
			return list.ToArray();
		}

		public static void mapify()
		{
			RenderTexture temporary = RenderTexture.GetTemporary((int)Level.size, (int)Level.size, 32);
			temporary.name = "Texture";
			Level.mapper.GetComponent<Camera>().targetTexture = temporary;
			bool fog = RenderSettings.fog;
			AmbientMode ambientMode = RenderSettings.ambientMode;
			Color ambientSkyColor = RenderSettings.ambientSkyColor;
			Color ambientEquatorColor = RenderSettings.ambientEquatorColor;
			Color ambientGroundColor = RenderSettings.ambientGroundColor;
			float heightmapPixelError = 0f;
			float heightmapPixelError2 = 0f;
			float basemapDistance = 0f;
			float basemapDistance2 = 0f;
			if (LevelGround.terrain != null)
			{
				heightmapPixelError = LevelGround.terrain.heightmapPixelError;
				heightmapPixelError2 = LevelGround.terrain2.heightmapPixelError;
				basemapDistance = LevelGround.terrain.basemapDistance;
				basemapDistance2 = LevelGround.terrain2.basemapDistance;
			}
			float lodBias = QualitySettings.lodBias;
			float @float = LevelLighting.sea.GetFloat("_Shininess");
			Color color = LevelLighting.sea.GetColor("_SpecularColor");
			ERenderMode renderMode = GraphicsSettings.renderMode;
			GraphicsSettings.renderMode = ERenderMode.FORWARD;
			GraphicsSettings.apply();
			RenderSettings.fog = false;
			RenderSettings.ambientMode = 1;
			RenderSettings.ambientSkyColor = Palette.AMBIENT;
			RenderSettings.ambientEquatorColor = Palette.AMBIENT;
			RenderSettings.ambientGroundColor = Palette.AMBIENT;
			if (LevelGround.terrain != null)
			{
				LevelGround.terrain.heightmapPixelError = 1f;
				LevelGround.terrain2.heightmapPixelError = 1f;
				LevelGround.terrain.basemapDistance = 8192f;
				LevelGround.terrain2.basemapDistance = 8192f;
			}
			LevelLighting.sea.SetFloat("_Shininess", 500f);
			LevelLighting.sea.SetColor("_SpecularColor", Color.black);
			QualitySettings.lodBias = float.MaxValue;
			for (byte b = 0; b < Regions.WORLD_SIZE; b += 1)
			{
				for (byte b2 = 0; b2 < Regions.WORLD_SIZE; b2 += 1)
				{
					if (!LevelObjects.regions[(int)b, (int)b2])
					{
						List<LevelObject> list = LevelObjects.objects[(int)b, (int)b2];
						for (int i = 0; i < list.Count; i++)
						{
							list[i].enableCollision();
							list[i].enableVisual();
							list[i].disableSkybox();
						}
					}
					if (!LevelGround.regions[(int)b, (int)b2])
					{
						List<ResourceSpawnpoint> list2 = LevelGround.trees[(int)b, (int)b2];
						for (int j = 0; j < list2.Count; j++)
						{
							list2[j].enable();
						}
					}
				}
			}
			Level.mapper.GetComponent<Camera>().Render();
			for (byte b3 = 0; b3 < Regions.WORLD_SIZE; b3 += 1)
			{
				for (byte b4 = 0; b4 < Regions.WORLD_SIZE; b4 += 1)
				{
					if (!LevelObjects.regions[(int)b3, (int)b4])
					{
						List<LevelObject> list3 = LevelObjects.objects[(int)b3, (int)b4];
						for (int k = 0; k < list3.Count; k++)
						{
							list3[k].disableCollision();
							list3[k].disableVisual();
							if (list3[k].isLandmarkQualityMet)
							{
								list3[k].enableSkybox();
							}
						}
					}
					if (!LevelGround.regions[(int)b3, (int)b4])
					{
						List<ResourceSpawnpoint> list4 = LevelGround.trees[(int)b3, (int)b4];
						for (int l = 0; l < list4.Count; l++)
						{
							list4[l].disable();
						}
					}
				}
			}
			GraphicsSettings.renderMode = renderMode;
			GraphicsSettings.apply();
			RenderSettings.fog = fog;
			RenderSettings.ambientMode = ambientMode;
			RenderSettings.ambientSkyColor = ambientSkyColor;
			RenderSettings.ambientEquatorColor = ambientEquatorColor;
			RenderSettings.ambientGroundColor = ambientGroundColor;
			if (LevelGround.terrain != null)
			{
				LevelGround.terrain.heightmapPixelError = heightmapPixelError;
				LevelGround.terrain2.heightmapPixelError = heightmapPixelError2;
				LevelGround.terrain.basemapDistance = basemapDistance;
				LevelGround.terrain2.basemapDistance = basemapDistance2;
			}
			LevelLighting.sea.SetFloat("_Shininess", @float);
			LevelLighting.sea.SetColor("_SpecularColor", color);
			QualitySettings.lodBias = lodBias;
			RenderTexture.active = temporary;
			Texture2D texture2D = new Texture2D(temporary.width, temporary.height);
			texture2D.name = "Mapify";
			texture2D.hideFlags = 61;
			texture2D.filterMode = 2;
			texture2D.ReadPixels(new Rect(0f, 0f, (float)temporary.width, (float)temporary.height), 0, 0);
			for (int m = 0; m < texture2D.width; m++)
			{
				for (int n = 0; n < texture2D.height; n++)
				{
					Color pixel = texture2D.GetPixel(m, n);
					if (pixel.a < 1f)
					{
						pixel.a = 1f;
						texture2D.SetPixel(m, n, pixel);
					}
				}
			}
			texture2D.Apply();
			byte[] bytes = texture2D.EncodeToPNG();
			ReadWrite.writeBytes(Level.info.path + "/Map.png", false, false, bytes);
			Object.DestroyImmediate(texture2D);
		}

		private static void findChartHit(Vector3 pos, out EObjectChart chart, out RaycastHit hit)
		{
			Physics.Raycast(pos, Vector3.down, ref hit, Level.HEIGHT, RayMasks.CHART);
			chart = EObjectChart.NONE;
			ObjectAsset asset = LevelObjects.getAsset(hit.transform);
			if (asset != null)
			{
				chart = asset.chart;
			}
			if (chart == EObjectChart.IGNORE)
			{
				Level.findChartHit(hit.point + Vector3.down * 0.1f, out chart, out hit);
				return;
			}
		}

		public static void chartify()
		{
			Bundle bundle = Bundles.getBundle(Level.info.path + "/Charts.unity3d", false);
			if (bundle == null)
			{
				return;
			}
			Texture2D texture2D = (Texture2D)bundle.load("Height_Strip");
			Texture2D texture2D2 = (Texture2D)bundle.load("Layer_Strip");
			bundle.unload();
			if (texture2D == null || texture2D2 == null)
			{
				return;
			}
			Texture2D texture2D3 = new Texture2D((int)Level.size, (int)Level.size);
			texture2D3.name = "Chartify";
			texture2D3.hideFlags = 61;
			texture2D3.filterMode = 2;
			float num = Mathf.Clamp01(WaterSystem.worldSeaLevel / Level.TERRAIN);
			float num2 = 1f;
			for (byte b = 0; b < Regions.WORLD_SIZE; b += 1)
			{
				for (byte b2 = 0; b2 < Regions.WORLD_SIZE; b2 += 1)
				{
					if (!LevelObjects.regions[(int)b, (int)b2])
					{
						List<LevelObject> list = LevelObjects.objects[(int)b, (int)b2];
						for (int i = 0; i < list.Count; i++)
						{
							list[i].enableCollision();
							list[i].enableVisual();
						}
					}
					if (!LevelGround.regions[(int)b, (int)b2])
					{
						List<ResourceSpawnpoint> list2 = LevelGround.trees[(int)b, (int)b2];
						for (int j = 0; j < list2.Count; j++)
						{
							list2[j].enable();
						}
					}
				}
			}
			GameObject gameObject = new GameObject();
			gameObject.layer = LayerMasks.GROUND;
			for (int k = 0; k < texture2D3.width; k++)
			{
				for (int l = 0; l < texture2D3.height; l++)
				{
					Vector3 vector;
					vector..ctor((float)(-(float)Level.size) / 2f + (float)Level.border + (float)k / (float)texture2D3.width * ((float)Level.size - (float)Level.border * 2f), Level.HEIGHT, (float)(-(float)Level.size) / 2f + (float)Level.border + (float)l / (float)texture2D3.height * ((float)Level.size - (float)Level.border * 2f));
					EObjectChart eobjectChart;
					RaycastHit raycastHit;
					Level.findChartHit(vector, out eobjectChart, out raycastHit);
					Transform transform = raycastHit.transform;
					Vector3 point = raycastHit.point;
					if (transform == null)
					{
						transform = gameObject.transform;
						point = vector;
						point.y = LevelGround.getHeight(vector);
					}
					int num3 = transform.gameObject.layer;
					if (eobjectChart == EObjectChart.GROUND)
					{
						num3 = LayerMasks.GROUND;
					}
					else if (eobjectChart == EObjectChart.HIGHWAY)
					{
						num3 = 0;
					}
					else if (eobjectChart == EObjectChart.ROAD)
					{
						num3 = 1;
					}
					else if (eobjectChart == EObjectChart.STREET)
					{
						num3 = 2;
					}
					else if (eobjectChart == EObjectChart.PATH)
					{
						num3 = 3;
					}
					else if (eobjectChart == EObjectChart.LARGE)
					{
						num3 = LayerMasks.LARGE;
					}
					else if (eobjectChart == EObjectChart.MEDIUM)
					{
						num3 = LayerMasks.MEDIUM;
					}
					else if (eobjectChart == EObjectChart.CLIFF)
					{
						num3 = 4;
					}
					if (num3 == LayerMasks.ENVIRONMENT)
					{
						RoadMaterial roadMaterial = LevelRoads.getRoadMaterial(transform);
						if (roadMaterial != null)
						{
							if (!roadMaterial.isConcrete)
							{
								num3 = 3;
							}
							else if (roadMaterial.width > 8f)
							{
								num3 = 0;
							}
							else
							{
								num3 = 1;
							}
						}
					}
					Color pixel;
					if (eobjectChart == EObjectChart.WATER)
					{
						pixel = texture2D.GetPixel(0, 0);
					}
					else if (num3 == LayerMasks.GROUND)
					{
						if (WaterUtility.isPointUnderwater(point))
						{
							pixel = texture2D.GetPixel(0, 0);
						}
						else
						{
							float num4 = point.y / Level.TERRAIN;
							num4 = (num4 - num) / (num2 - num);
							num4 = Mathf.Clamp01(num4);
							pixel = texture2D.GetPixel((int)(num4 * (float)(texture2D.width - 1)) + 1, 0);
						}
					}
					else
					{
						pixel = texture2D2.GetPixel(num3, 0);
					}
					texture2D3.SetPixel(k, l, pixel);
				}
			}
			texture2D3.Apply();
			for (byte b3 = 0; b3 < Regions.WORLD_SIZE; b3 += 1)
			{
				for (byte b4 = 0; b4 < Regions.WORLD_SIZE; b4 += 1)
				{
					if (!LevelObjects.regions[(int)b3, (int)b4])
					{
						List<LevelObject> list3 = LevelObjects.objects[(int)b3, (int)b4];
						for (int m = 0; m < list3.Count; m++)
						{
							list3[m].disableCollision();
							list3[m].disableVisual();
						}
					}
					if (!LevelGround.regions[(int)b3, (int)b4])
					{
						List<ResourceSpawnpoint> list4 = LevelGround.trees[(int)b3, (int)b4];
						for (int n = 0; n < list4.Count; n++)
						{
							list4[n].disable();
						}
					}
				}
			}
			byte[] bytes = texture2D3.EncodeToPNG();
			ReadWrite.writeBytes(Level.info.path + "/Chart.png", false, false, bytes);
			Object.DestroyImmediate(texture2D3);
		}

		public IEnumerator init(int id)
		{
			if (!Level.isVR)
			{
				LevelNavigation.load();
			}
			LoadingUI.updateProgress(1f / Level.STEPS);
			yield return null;
			LevelObjects.load();
			LoadingUI.updateProgress(2f / Level.STEPS);
			yield return null;
			LevelLighting.load(Level.size);
			LoadingUI.updateProgress(3f / Level.STEPS);
			yield return null;
			LevelGround.load(Level.size);
			LoadingUI.updateProgress(4f / Level.STEPS);
			yield return null;
			LevelRoads.load();
			LoadingUI.updateProgress(5f / Level.STEPS);
			yield return null;
			if (!Level.isVR)
			{
				LevelNodes.load();
				LoadingUI.updateProgress(6f / Level.STEPS);
				yield return null;
				LevelItems.load();
				LoadingUI.updateProgress(7f / Level.STEPS);
				yield return null;
			}
			LevelPlayers.load();
			LoadingUI.updateProgress(8f / Level.STEPS);
			yield return null;
			if (!Level.isVR)
			{
				LevelZombies.load();
				LoadingUI.updateProgress(9f / Level.STEPS);
				yield return null;
				LevelVehicles.load();
				LoadingUI.updateProgress(10f / Level.STEPS);
				yield return null;
				LevelAnimals.load();
				LoadingUI.updateProgress(11f / Level.STEPS);
				yield return null;
			}
			LevelVisibility.load();
			LoadingUI.updateProgress(12f / Level.STEPS);
			yield return null;
			if (Level.loadingSteps != null)
			{
				Level.loadingSteps();
			}
			yield return null;
			LevelBarricades.load();
			yield return null;
			LevelStructures.load();
			Level._hash = Hash.combine(new byte[][]
			{
				Level.getLevelHash(Level.info.path),
				LevelGround.hash,
				LevelLighting.hash,
				LevelObjects.hash
			});
			Physics.gravity = new Vector3(0f, Level.info.configData.Gravity, 0f);
			yield return null;
			Resources.UnloadUnusedAssets();
			GC.Collect();
			yield return null;
			Level._editing = new GameObject().transform;
			Level.editing.name = "Editing";
			Level.editing.parent = Level.level;
			if (Level.isEditor)
			{
				Level.mapper = ((GameObject)Object.Instantiate(Resources.Load("Edit/Mapper"))).transform;
				Level.mapper.name = "Mapper";
				Level.mapper.parent = Level.editing;
				Level.mapper.position = new Vector3(0f, 1028f, 0f);
				Level.mapper.rotation = Quaternion.Euler(90f, 0f, 0f);
				Level.mapper.GetComponent<Camera>().orthographicSize = (float)(Level.size / 2 - Level.border);
				if (Level.isDevkit)
				{
					GameObject gameObject = Object.Instantiate<GameObject>(Resources.Load<GameObject>("Edit2/Editor"));
					if (gameObject != null)
					{
						gameObject.name = "Editor";
						gameObject.transform.parent = Level.editing;
					}
				}
				else
				{
					Transform transform = ((GameObject)Object.Instantiate(Resources.Load((!Level.isVR) ? "Edit/Editor" : "Edit/VR"))).transform;
					transform.name = "Editor";
					transform.parent = Level.editing;
					transform.tag = "Logic";
					transform.gameObject.layer = LayerMasks.LOGIC;
				}
			}
			yield return null;
			if (Level.onPrePreLevelLoaded != null)
			{
				Level.onPrePreLevelLoaded(id);
			}
			yield return null;
			if (Level.onPreLevelLoaded != null)
			{
				Level.onPreLevelLoaded(id);
			}
			yield return null;
			if (Level.onLevelLoaded != null)
			{
				Level.onLevelLoaded(id);
			}
			yield return null;
			if (Level.onPostLevelLoaded != null)
			{
				Level.onPostLevelLoaded(id);
			}
			yield return null;
			if (!Level.isEditor && Level.info != null && Level.info.hasTriggers)
			{
				Transform transform2 = null;
				string text = Level.info.name.ToLower();
				if (text != null)
				{
					if (!(text == "germany"))
					{
						if (!(text == "pei"))
						{
							if (!(text == "russia"))
							{
								if (text == "tutorial")
								{
									transform2 = ((GameObject)Object.Instantiate(Resources.Load("Level/Triggers_Tutorial"))).transform;
								}
							}
							else
							{
								transform2 = ((GameObject)Object.Instantiate(Resources.Load("Level/Triggers_Russia"))).transform;
							}
						}
						else
						{
							transform2 = ((GameObject)Object.Instantiate(Resources.Load("Level/Triggers_PEI"))).transform;
						}
					}
					else
					{
						transform2 = ((GameObject)Object.Instantiate(Resources.Load("Level/Triggers_Germany"))).transform;
					}
				}
				if (transform2 != null)
				{
					transform2.position = Vector3.zero;
					transform2.rotation = Quaternion.identity;
					transform2.name = "Triggers";
					transform2.parent = Level.clips;
				}
			}
			yield return null;
			Level._isLoaded = true;
			Level.isLoadingContent = false;
			yield break;
		}

		private void Awake()
		{
			if (Level.isInitialized)
			{
				Object.Destroy(base.gameObject);
				return;
			}
			Level._isInitialized = true;
			Object.DontDestroyOnLoad(base.gameObject);
			SceneManager.sceneLoaded += new UnityAction<Scene, LoadSceneMode>(this.onSceneLoaded);
		}

		private void onSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			if (scene.buildIndex == 3)
			{
				return;
			}
			if (scene.buildIndex == 2 && !Dedicator.isDedicated)
			{
				LoadingUI.loader.AddComponent<AudioListener>();
			}
			LevelLighting.areFXAllowed = true;
			if (scene.buildIndex > Level.SETUP && Level.info != null)
			{
				Level._level = new GameObject().transform;
				Level.level.name = Level.info.name;
				Level.level.tag = "Logic";
				Level.level.gameObject.layer = LayerMasks.LOGIC;
				Level._roots = new GameObject().transform;
				Level.roots.name = "Roots";
				Level.roots.parent = Level.level;
				Level._clips = new GameObject().transform;
				Level.clips.name = "Clips";
				Level.clips.parent = Level.level;
				Level.clips.tag = "Clip";
				Level.clips.gameObject.layer = LayerMasks.CLIP;
				if (Level.info.configData.Use_Legacy_Clip_Borders)
				{
					Transform transform = ((GameObject)Object.Instantiate(Resources.Load("Level/Cap"))).transform;
					transform.position = new Vector3(0f, -4f, 0f);
					transform.localScale = new Vector3((float)(Level.size - Level.border * 2 + Level.CLIP * 2), (float)(Level.size - Level.border * 2 + Level.CLIP * 2), 1f);
					transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
					transform.name = "Cap";
					transform.parent = Level.clips;
					transform = ((GameObject)Object.Instantiate(Resources.Load("Level/Cap"))).transform;
					transform.position = new Vector3(0f, Level.HEIGHT + 4f, 0f);
					transform.localScale = new Vector3((float)(Level.size - Level.border * 2 + Level.CLIP * 2), (float)(Level.size - Level.border * 2 + Level.CLIP * 2), 1f);
					transform.rotation = Quaternion.Euler(90f, 0f, 0f);
					transform.name = "Cap";
					transform.parent = Level.clips;
					transform = ((GameObject)Object.Instantiate(Resources.Load("Level/Cap"))).transform;
					transform.position = new Vector3((float)(Level.size / 2 - Level.border), Level.HEIGHT / 2f, (float)(Level.size / 2 - Level.border));
					transform.localScale = new Vector3((float)(Level.CLIP * 4), (float)(Level.CLIP * 4), 64f);
					transform.rotation = Quaternion.Euler(90f, 0f, 45f);
					transform.name = "Cap";
					transform.parent = Level.clips;
					transform = ((GameObject)Object.Instantiate(Resources.Load("Level/Cap"))).transform;
					transform.position = new Vector3((float)(-Level.size / 2 + Level.border), Level.HEIGHT / 2f, (float)(Level.size / 2 - Level.border));
					transform.localScale = new Vector3((float)(Level.CLIP * 4), (float)(Level.CLIP * 4), 64f);
					transform.rotation = Quaternion.Euler(90f, 0f, 45f);
					transform.name = "Cap";
					transform.parent = Level.clips;
					transform = ((GameObject)Object.Instantiate(Resources.Load("Level/Cap"))).transform;
					transform.position = new Vector3((float)(Level.size / 2 - Level.border), Level.HEIGHT / 2f, (float)(-Level.size / 2 + Level.border));
					transform.localScale = new Vector3((float)(Level.CLIP * 4), (float)(Level.CLIP * 4), 64f);
					transform.rotation = Quaternion.Euler(90f, 0f, 45f);
					transform.name = "Cap";
					transform.parent = Level.clips;
					transform = ((GameObject)Object.Instantiate(Resources.Load("Level/Cap"))).transform;
					transform.position = new Vector3((float)(-Level.size / 2 + Level.border), Level.HEIGHT / 2f, (float)(-Level.size / 2 + Level.border));
					transform.localScale = new Vector3((float)(Level.CLIP * 4), (float)(Level.CLIP * 4), 64f);
					transform.rotation = Quaternion.Euler(90f, 0f, 45f);
					transform.name = "Cap";
					transform.parent = Level.clips;
					Transform transform2 = ((GameObject)Object.Instantiate(Resources.Load((!Level.isEditor) ? "Level/Clip" : "Level/Wall"))).transform;
					transform2.position = new Vector3((float)(Level.size / 2 - Level.border), Level.HEIGHT / 8f, 0f);
					transform2.localScale = new Vector3((float)(Level.size - Level.border * 2), Level.HEIGHT / 4f, 1f);
					transform2.rotation = Quaternion.Euler(0f, -90f, 0f);
					transform2.name = "Clip";
					transform2.parent = Level.clips;
					if (Level.isEditor)
					{
						transform2.GetComponent<Renderer>().material.mainTextureScale = new Vector2((float)(Level.size - Level.border * 2) / 32f, 4f);
					}
					transform2 = ((GameObject)Object.Instantiate(Resources.Load((!Level.isEditor) ? "Level/Clip" : "Level/Wall"))).transform;
					transform2.position = new Vector3((float)(-Level.size / 2 + Level.border), Level.HEIGHT / 8f, 0f);
					transform2.localScale = new Vector3((float)(Level.size - Level.border * 2), Level.HEIGHT / 4f, 1f);
					transform2.rotation = Quaternion.Euler(0f, 90f, 0f);
					transform2.name = "Clip";
					transform2.parent = Level.clips;
					if (Level.isEditor)
					{
						transform2.GetComponent<Renderer>().material.mainTextureScale = new Vector2((float)(Level.size - Level.border * 2) / 32f, 4f);
					}
					transform2 = ((GameObject)Object.Instantiate(Resources.Load((!Level.isEditor) ? "Level/Clip" : "Level/Wall"))).transform;
					transform2.position = new Vector3(0f, Level.HEIGHT / 8f, (float)(Level.size / 2 - Level.border));
					transform2.localScale = new Vector3((float)(Level.size - Level.border * 2), Level.HEIGHT / 4f, 1f);
					transform2.rotation = Quaternion.Euler(0f, 180f, 0f);
					transform2.name = "Clip";
					transform2.parent = Level.clips;
					if (Level.isEditor)
					{
						transform2.GetComponent<Renderer>().material.mainTextureScale = new Vector2((float)(Level.size - Level.border * 2) / 32f, 4f);
					}
					transform2 = ((GameObject)Object.Instantiate(Resources.Load((!Level.isEditor) ? "Level/Clip" : "Level/Wall"))).transform;
					transform2.position = new Vector3(0f, Level.HEIGHT / 8f, (float)(-Level.size / 2 + Level.border));
					transform2.localScale = new Vector3((float)(Level.size - Level.border * 2), Level.HEIGHT / 4f, 1f);
					transform2.rotation = Quaternion.identity;
					transform2.name = "Clip";
					transform2.parent = Level.clips;
					if (Level.isEditor)
					{
						transform2.GetComponent<Renderer>().material.mainTextureScale = new Vector2((float)(Level.size - Level.border * 2) / 32f, 4f);
					}
				}
				Level._effects = new GameObject().transform;
				Level.effects.name = "Effects";
				Level.effects.parent = Level.level;
				Level.effects.tag = "Logic";
				Level.effects.gameObject.layer = LayerMasks.LOGIC;
				Level._spawns = new GameObject().transform;
				Level.spawns.name = "Spawns";
				Level.spawns.parent = Level.level;
				Level.spawns.tag = "Logic";
				Level.spawns.gameObject.layer = LayerMasks.LOGIC;
				base.StartCoroutine("init", scene.buildIndex);
			}
			else
			{
				Level.isLoadingLighting = true;
				Level.isLoadingVehicles = true;
				Level.isLoadingBarricades = true;
				Level.isLoadingStructures = true;
				Level.isLoadingContent = true;
				Level.isLoadingArea = true;
				Level._isLoaded = false;
				if (Level.onLevelLoaded != null)
				{
					Level.onLevelLoaded(scene.buildIndex);
				}
			}
			Resources.UnloadUnusedAssets();
			GC.Collect();
		}

		private static readonly float STEPS = 12f;

		public static readonly int SETUP;

		public static readonly int MENU = 1;

		public static readonly float HEIGHT = 512f;

		public static readonly float TERRAIN = 256f;

		public static readonly ushort CLIP = 8;

		public static readonly ushort TINY_BORDER = 16;

		public static readonly ushort SMALL_BORDER = 64;

		public static readonly ushort MEDIUM_BORDER = 64;

		public static readonly ushort LARGE_BORDER = 64;

		public static readonly ushort INSANE_BORDER = 128;

		public static readonly ushort TINY_SIZE = 512;

		public static readonly ushort SMALL_SIZE = 1024;

		public static readonly ushort MEDIUM_SIZE = 2048;

		public static readonly ushort LARGE_SIZE = 4096;

		public static readonly ushort INSANE_SIZE = 8192;

		public static readonly ushort TINY_VEHICLES = 4;

		public static readonly ushort SMALL_VEHICLES = 8;

		public static readonly ushort MEDIUM_VEHICLES = 24;

		public static readonly ushort LARGE_VEHICLES = 32;

		public static readonly ushort INSANE_VEHICLES = 64;

		public static readonly ushort TINY_ANIMALS = 4;

		public static readonly ushort SMALL_ANIMALS = 8;

		public static readonly ushort MEDIUM_ANIMALS = 24;

		public static readonly ushort LARGE_ANIMALS = 32;

		public static readonly ushort INSANE_ANIMALS = 64;

		public static readonly float TINY_FOG = 0.5f;

		public static readonly float SMALL_FOG = 1.5f;

		public static readonly float MEDIUM_FOG = 3f;

		public static readonly float LARGE_FOG = 4.5f;

		public static readonly float INSANE_FOG = 6f;

		public static readonly byte SAVEDATA_VERSION = 2;

		public static PrePreLevelLoaded onPrePreLevelLoaded;

		public static PreLevelLoaded onPreLevelLoaded;

		public static LevelLoaded onLevelLoaded;

		public static PostLevelLoaded onPostLevelLoaded;

		public static LevelsRefreshed onLevelsRefreshed;

		public static LevelExited onLevelExited;

		private static LevelInfo _info;

		private static Transform mapper;

		private static Transform _level;

		private static Transform _roots;

		private static Transform _clips;

		private static Transform _effects;

		private static Transform _spawns;

		private static Transform _editing;

		private static bool _isInitialized;

		private static bool _isEditor;

		protected static bool _isDevkit;

		public static bool isLoadingContent = true;

		public static bool isLoadingLighting = true;

		public static bool isLoadingVehicles = true;

		public static bool isLoadingBarricades = true;

		public static bool isLoadingStructures = true;

		public static bool isLoadingArea = true;

		private static bool _isLoaded;

		private static byte[] _hash;
	}
}
