using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SDG.Unturned
{
	public class OptionsSettings
	{
		public static float fov
		{
			get
			{
				return OptionsSettings._fov;
			}
			set
			{
				OptionsSettings._fov = value;
				OptionsSettings._view = (float)OptionsSettings.MIN_FOV + (float)OptionsSettings.MAX_FOV * value;
			}
		}

		public static float view
		{
			get
			{
				return OptionsSettings._view;
			}
		}

		public static void apply()
		{
			if (!Level.isLoaded && MainCamera.instance != null && !Level.isVR && !Dedicator.isVR)
			{
				MainCamera.instance.fieldOfView = OptionsSettings.view;
			}
			if (SceneManager.GetActiveScene().buildIndex <= Level.MENU)
			{
				MenuConfigurationOptions.apply();
			}
			AudioListener.volume = OptionsSettings.volume;
			if (LevelLighting.dayAudio != null)
			{
				if (!LevelLighting.dayAudio.enabled && OptionsSettings.ambience)
				{
					LevelLighting.dayAudio.enabled = true;
					LevelLighting.dayAudio.Play();
				}
				else
				{
					LevelLighting.dayAudio.enabled = OptionsSettings.ambience;
				}
			}
			if (LevelLighting.nightAudio != null)
			{
				if (!LevelLighting.nightAudio.enabled && OptionsSettings.ambience)
				{
					LevelLighting.nightAudio.enabled = true;
					LevelLighting.nightAudio.Play();
				}
				else
				{
					LevelLighting.nightAudio.enabled = OptionsSettings.ambience;
				}
			}
		}

		public static void restoreDefaults()
		{
			OptionsSettings.music = true;
			OptionsSettings.timer = false;
			OptionsSettings.fov = 0.75f;
			OptionsSettings.volume = 1f;
			OptionsSettings.voice = 1f;
			OptionsSettings.debug = false;
			OptionsSettings.gore = true;
			OptionsSettings.filter = false;
			OptionsSettings.chatText = true;
			OptionsSettings.chatVoiceIn = true;
			OptionsSettings.chatVoiceOut = true;
			OptionsSettings.metric = true;
			OptionsSettings.talk = false;
			OptionsSettings.hints = true;
			OptionsSettings.ambience = true;
			OptionsSettings.proUI = true;
			OptionsSettings.hitmarker = false;
			OptionsSettings.streamer = false;
			OptionsSettings.featuredWorkshop = true;
			OptionsSettings.matchmakingShowAllMaps = false;
			OptionsSettings.minMatchmakingPlayers = 12;
			OptionsSettings.maxMatchmakingPing = 300;
			OptionsSettings.crosshairColor = Color.white;
			OptionsSettings.hitmarkerColor = Color.white;
			OptionsSettings.criticalHitmarkerColor = Color.red;
			OptionsSettings.cursorColor = Color.white;
			OptionsSettings.backgroundColor = new Color(0.9f, 0.9f, 0.9f);
			OptionsSettings.foregroundColor = new Color(0.9f, 0.9f, 0.9f);
			OptionsSettings.fontColor = new Color(0.9f, 0.9f, 0.9f);
		}

		public static void load()
		{
			OptionsSettings.restoreDefaults();
			if (ReadWrite.fileExists("/Options.dat", true))
			{
				Block block = ReadWrite.readBlock("/Options.dat", true, 0);
				if (block != null)
				{
					byte b = block.readByte();
					if (b > 2)
					{
						OptionsSettings.music = block.readBoolean();
						if (b < 20)
						{
							OptionsSettings.timer = false;
						}
						else
						{
							OptionsSettings.timer = block.readBoolean();
						}
						if (b < 10)
						{
							block.readBoolean();
						}
						if (b > 7)
						{
							OptionsSettings.fov = block.readSingle();
						}
						else
						{
							OptionsSettings.fov = block.readSingle() * 0.5f;
						}
						if (b < 24)
						{
							OptionsSettings.fov *= 1.5f;
							OptionsSettings.fov = Mathf.Clamp01(OptionsSettings.fov);
						}
						if (b > 4)
						{
							OptionsSettings.volume = block.readSingle();
						}
						else
						{
							OptionsSettings.volume = 1f;
						}
						if (b > 22)
						{
							OptionsSettings.voice = block.readSingle();
						}
						else
						{
							OptionsSettings.voice = 1f;
						}
						OptionsSettings.debug = block.readBoolean();
						OptionsSettings.gore = block.readBoolean();
						OptionsSettings.filter = block.readBoolean();
						OptionsSettings.chatText = block.readBoolean();
						if (b > 8)
						{
							OptionsSettings.chatVoiceIn = block.readBoolean();
						}
						else
						{
							OptionsSettings.chatVoiceIn = true;
						}
						OptionsSettings.chatVoiceOut = block.readBoolean();
						OptionsSettings.metric = block.readBoolean();
						if (b > 24)
						{
							OptionsSettings.talk = block.readBoolean();
						}
						else
						{
							OptionsSettings.talk = false;
						}
						if (b > 3)
						{
							OptionsSettings.hints = block.readBoolean();
						}
						else
						{
							OptionsSettings.hints = true;
						}
						if (b > 13)
						{
							OptionsSettings.ambience = block.readBoolean();
						}
						else
						{
							OptionsSettings.ambience = true;
						}
						if (b > 12)
						{
							OptionsSettings.proUI = block.readBoolean();
						}
						else
						{
							OptionsSettings.proUI = true;
						}
						if (b > 20)
						{
							OptionsSettings.hitmarker = block.readBoolean();
						}
						else
						{
							OptionsSettings.hitmarker = false;
						}
						if (b > 21)
						{
							OptionsSettings.streamer = block.readBoolean();
						}
						else
						{
							OptionsSettings.streamer = false;
						}
						if (b > 25)
						{
							OptionsSettings.featuredWorkshop = block.readBoolean();
						}
						else
						{
							OptionsSettings.featuredWorkshop = true;
						}
						if (b > 28)
						{
							OptionsSettings.matchmakingShowAllMaps = block.readBoolean();
						}
						else
						{
							OptionsSettings.matchmakingShowAllMaps = false;
						}
						if (b > 27)
						{
							OptionsSettings.minMatchmakingPlayers = block.readInt32();
						}
						else
						{
							OptionsSettings.minMatchmakingPlayers = 12;
						}
						if (b > 26)
						{
							OptionsSettings.maxMatchmakingPing = block.readInt32();
						}
						else
						{
							OptionsSettings.maxMatchmakingPing = 300;
						}
						if (b > 6)
						{
							OptionsSettings.crosshairColor = block.readColor();
							OptionsSettings.hitmarkerColor = block.readColor();
							OptionsSettings.criticalHitmarkerColor = block.readColor();
							OptionsSettings.cursorColor = block.readColor();
						}
						else
						{
							OptionsSettings.crosshairColor = Color.white;
							OptionsSettings.hitmarkerColor = Color.white;
							OptionsSettings.criticalHitmarkerColor = Color.red;
							OptionsSettings.cursorColor = Color.white;
						}
						if (b > 18)
						{
							OptionsSettings.backgroundColor = block.readColor();
							OptionsSettings.foregroundColor = block.readColor();
							OptionsSettings.fontColor = block.readColor();
						}
						else
						{
							OptionsSettings.backgroundColor = new Color(0.9f, 0.9f, 0.9f);
							OptionsSettings.foregroundColor = new Color(0.9f, 0.9f, 0.9f);
							OptionsSettings.fontColor = new Color(0.9f, 0.9f, 0.9f);
						}
						return;
					}
				}
			}
		}

		public static void save()
		{
			Block block = new Block();
			block.writeByte(OptionsSettings.SAVEDATA_VERSION);
			block.writeBoolean(OptionsSettings.music);
			block.writeBoolean(OptionsSettings.timer);
			block.writeSingle(OptionsSettings.fov);
			block.writeSingle(OptionsSettings.volume);
			block.writeSingle(OptionsSettings.voice);
			block.writeBoolean(OptionsSettings.debug);
			block.writeBoolean(OptionsSettings.gore);
			block.writeBoolean(OptionsSettings.filter);
			block.writeBoolean(OptionsSettings.chatText);
			block.writeBoolean(OptionsSettings.chatVoiceIn);
			block.writeBoolean(OptionsSettings.chatVoiceOut);
			block.writeBoolean(OptionsSettings.metric);
			block.writeBoolean(OptionsSettings.talk);
			block.writeBoolean(OptionsSettings.hints);
			block.writeBoolean(OptionsSettings.ambience);
			block.writeBoolean(OptionsSettings.proUI);
			block.writeBoolean(OptionsSettings.hitmarker);
			block.writeBoolean(OptionsSettings.streamer);
			block.writeBoolean(OptionsSettings.featuredWorkshop);
			block.writeBoolean(OptionsSettings.matchmakingShowAllMaps);
			block.writeInt32(OptionsSettings.minMatchmakingPlayers);
			block.writeInt32(OptionsSettings.maxMatchmakingPing);
			block.writeColor(OptionsSettings.crosshairColor);
			block.writeColor(OptionsSettings.hitmarkerColor);
			block.writeColor(OptionsSettings.criticalHitmarkerColor);
			block.writeColor(OptionsSettings.cursorColor);
			block.writeColor(OptionsSettings.backgroundColor);
			block.writeColor(OptionsSettings.foregroundColor);
			block.writeColor(OptionsSettings.fontColor);
			ReadWrite.writeBlock("/Options.dat", true, block);
		}

		public static readonly byte SAVEDATA_VERSION = 29;

		public static readonly byte MIN_FOV = 60;

		public static readonly byte MAX_FOV = 40;

		private static float _fov;

		private static float _view;

		public static float volume;

		public static float voice;

		public static bool debug;

		public static bool music;

		public static bool timer;

		public static bool gore;

		public static bool filter;

		public static bool chatText;

		public static bool chatVoiceIn;

		public static bool chatVoiceOut;

		public static bool metric;

		public static bool talk;

		public static bool hints;

		public static bool ambience;

		public static bool proUI;

		public static bool hitmarker;

		public static bool streamer;

		public static bool featuredWorkshop;

		public static bool matchmakingShowAllMaps;

		public static int minMatchmakingPlayers;

		public static int maxMatchmakingPing;

		public static Color crosshairColor;

		public static Color hitmarkerColor;

		public static Color criticalHitmarkerColor;

		public static Color cursorColor;

		public static Color backgroundColor;

		public static Color foregroundColor;

		public static Color fontColor;
	}
}
