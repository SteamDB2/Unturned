using System;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	public class LightingManager : SteamCaller
	{
		public static float day
		{
			get
			{
				return LightingManager.time / LightingManager.cycle;
			}
		}

		public static uint cycle
		{
			get
			{
				return LightingManager._cycle;
			}
			set
			{
				LightingManager._offset = Provider.time - (uint)(LightingManager.day * value);
				LightingManager._cycle = value;
				if (Provider.isServer)
				{
					LightingManager.manager.updateLighting();
					LightingManager.manager.channel.send("tellLightingCycle", ESteamCall.OTHERS, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
					{
						LightingManager.cycle
					});
				}
			}
		}

		public static uint time
		{
			get
			{
				return LightingManager._time;
			}
			set
			{
				value %= LightingManager.cycle;
				LightingManager._offset = Provider.time - value;
				LightingManager._time = value;
				LightingManager.manager.updateLighting();
				LightingManager.manager.channel.send("tellLightingOffset", ESteamCall.OTHERS, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
				{
					LightingManager.offset
				});
			}
		}

		public static uint offset
		{
			get
			{
				return LightingManager._offset;
			}
		}

		public static bool hasRain
		{
			get
			{
				return LightingManager._hasRain;
			}
		}

		public static bool hasSnow
		{
			get
			{
				return LightingManager._hasSnow;
			}
		}

		public static bool isFullMoon
		{
			get
			{
				return LightingManager._isFullMoon;
			}
			set
			{
				if (value != LightingManager.isFullMoon)
				{
					LightingManager._isFullMoon = value;
					if (LightingManager.onMoonUpdated != null)
					{
						LightingManager.onMoonUpdated(LightingManager.isFullMoon);
					}
				}
			}
		}

		public static bool isDaytime
		{
			get
			{
				return LightingManager.day < LevelLighting.bias;
			}
		}

		public static bool isNighttime
		{
			get
			{
				return !LightingManager.isDaytime;
			}
		}

		[SteamCall]
		public void tellLighting(CSteamID steamID, uint serverTime, uint newCycle, uint newOffset, byte moon, byte wind, byte rain, byte snow)
		{
			if (base.channel.checkServer(steamID))
			{
				Provider.time = serverTime;
				LightingManager._cycle = newCycle;
				LightingManager._offset = newOffset;
				this.updateLighting();
				LevelLighting.moon = moon;
				LightingManager.isCycled = (LightingManager.day > LevelLighting.bias);
				LightingManager.isFullMoon = (LightingManager.isCycled && LevelLighting.moon == 2);
				if (LightingManager.onDayNightUpdated != null)
				{
					LightingManager.onDayNightUpdated(LightingManager.isDaytime);
				}
				LevelLighting.wind = (float)wind * 2f;
				LevelLighting.rainyness = (ELightingRain)rain;
				LevelLighting.snowyness = (ELightingSnow)snow;
				if (LightingManager.onRainUpdated != null)
				{
					LightingManager.onRainUpdated(LevelLighting.rainyness);
				}
				if (LightingManager.onSnowUpdated != null)
				{
					LightingManager.onSnowUpdated(LevelLighting.snowyness);
				}
				Level.isLoadingLighting = false;
			}
		}

		[SteamCall]
		public void askLighting(CSteamID steamID)
		{
			base.channel.send("tellLighting", steamID, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
			{
				Provider.time,
				LightingManager.cycle,
				LightingManager.offset,
				LevelLighting.moon,
				MeasurementTool.angleToByte(LevelLighting.wind),
				(byte)LevelLighting.rainyness,
				(byte)LevelLighting.snowyness
			});
		}

		[SteamCall]
		public void tellLightingCycle(CSteamID steamID, uint newScale)
		{
			if (base.channel.checkServer(steamID))
			{
				LightingManager._offset = Provider.time - (uint)(LightingManager.day * newScale);
				LightingManager._cycle = newScale;
				this.updateLighting();
			}
		}

		[SteamCall]
		public void tellLightingOffset(CSteamID steamID, uint newCycle)
		{
			if (base.channel.checkServer(steamID))
			{
				LightingManager._offset = newCycle;
				this.updateLighting();
			}
		}

		[SteamCall]
		public void tellLightingWind(CSteamID steamID, byte newWind)
		{
			if (base.channel.checkServer(steamID))
			{
				LevelLighting.wind = (float)newWind;
			}
		}

		[SteamCall]
		public void tellLightingRain(CSteamID steamID, byte newRain)
		{
			if (base.channel.checkServer(steamID))
			{
				LevelLighting.rainyness = (ELightingRain)newRain;
				if (LightingManager.onRainUpdated != null)
				{
					LightingManager.onRainUpdated(LevelLighting.rainyness);
				}
			}
		}

		[SteamCall]
		public void tellLightingSnow(CSteamID steamID, byte newSnow)
		{
			if (base.channel.checkServer(steamID))
			{
				LevelLighting.snowyness = (ELightingSnow)newSnow;
				if (LightingManager.onSnowUpdated != null)
				{
					LightingManager.onSnowUpdated(LevelLighting.snowyness);
				}
			}
		}

		private void onClientConnected()
		{
			if (Level.info.type != ELevelType.SURVIVAL)
			{
				return;
			}
			base.channel.send("askLighting", ESteamCall.SERVER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[0]);
		}

		private void updateLighting()
		{
			LightingManager._time = (Provider.time - LightingManager.offset) % LightingManager.cycle;
			if (Provider.isServer && Time.time - LightingManager.lastWind > LightingManager.windDelay)
			{
				LightingManager.windDelay = (float)Random.Range(45, 75);
				LightingManager.lastWind = Time.time;
				LevelLighting.wind = (float)Random.Range(0, 360);
				LightingManager.manager.channel.send("tellLightingWind", ESteamCall.OTHERS, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
				{
					MeasurementTool.angleToByte(LevelLighting.wind)
				});
			}
			if (LightingManager.day > LevelLighting.bias)
			{
				if (!LightingManager.isCycled)
				{
					LightingManager.isCycled = true;
					if (LevelLighting.moon < LevelLighting.MOON_CYCLES - 1)
					{
						LevelLighting.moon += 1;
						LightingManager.isFullMoon = (LevelLighting.moon == 2);
					}
					else
					{
						LevelLighting.moon = 0;
						LightingManager.isFullMoon = false;
					}
					if (LightingManager.onDayNightUpdated != null)
					{
						LightingManager.onDayNightUpdated(false);
					}
				}
			}
			else if (LightingManager.isCycled)
			{
				LightingManager.isCycled = false;
				LightingManager.isFullMoon = false;
				if (LightingManager.onDayNightUpdated != null)
				{
					LightingManager.onDayNightUpdated(true);
				}
			}
			if (!Dedicator.isDedicated)
			{
				LevelLighting.time = LightingManager.day;
			}
		}

		private void onLevelLoaded(int level)
		{
			if (level > Level.SETUP)
			{
				LightingManager.onMoonUpdated = null;
				LightingManager.lastRain = Time.realtimeSinceStartup;
				LightingManager.lastSnow = Time.realtimeSinceStartup;
				if (Level.info != null && Level.info.type != ELevelType.SURVIVAL)
				{
					LightingManager._cycle = 3600u;
					LightingManager._offset = 0u;
					if (Level.info.type == ELevelType.HORDE)
					{
						LightingManager._time = (uint)((LevelLighting.bias + (1f - LevelLighting.bias) / 2f) * LightingManager.cycle);
						LightingManager._isFullMoon = true;
					}
					else if (Level.info.type == ELevelType.ARENA)
					{
						LightingManager._time = (uint)(LevelLighting.transition * LightingManager.cycle);
						LightingManager._isFullMoon = false;
					}
					LightingManager.windDelay = (float)Random.Range(45, 75);
					LevelLighting.wind = (float)Random.Range(0, 360);
					LevelLighting.rainyness = ELightingRain.NONE;
					LevelLighting.snowyness = ELightingSnow.NONE;
					Level.isLoadingLighting = false;
					if (!Dedicator.isDedicated)
					{
						LevelLighting.time = LightingManager.day;
						LevelLighting.moon = 2;
					}
					return;
				}
				LightingManager._cycle = 3600u;
				LightingManager._time = 0u;
				LightingManager._offset = 0u;
				LightingManager._isFullMoon = false;
				LightingManager.isCycled = false;
				if (LightingManager.onDayNightUpdated != null)
				{
					LightingManager.onDayNightUpdated(true);
				}
				LightingManager.windDelay = (float)Random.Range(45, 75);
				LevelLighting.wind = (float)Random.Range(0, 360);
				if (Provider.isServer)
				{
					LightingManager.load();
					this.updateLighting();
					Level.isLoadingLighting = false;
				}
			}
		}

		private void Update()
		{
			if (!Level.isLoaded || Level.info == null)
			{
				return;
			}
			if (Level.isEditor)
			{
				LevelLighting.updateLighting();
			}
			else if (Level.info.type == ELevelType.SURVIVAL)
			{
				this.updateLighting();
			}
			if (Provider.isServer)
			{
				if (LevelLighting.canRain)
				{
					if (!LightingManager.hasRain)
					{
						LightingManager.rainFrequency = (uint)(Random.Range(Provider.modeConfigData.Events.Rain_Frequency_Min, Provider.modeConfigData.Events.Rain_Frequency_Max) * LightingManager.cycle * LevelLighting.rainFreq);
						LightingManager.rainDuration = (uint)(Random.Range(Provider.modeConfigData.Events.Rain_Duration_Min, Provider.modeConfigData.Events.Rain_Duration_Max) * LightingManager.cycle * LevelLighting.rainDur);
						LightingManager._hasRain = true;
						LightingManager.lastRain = Time.realtimeSinceStartup;
					}
					switch (LevelLighting.rainyness)
					{
					case ELightingRain.NONE:
						if (LightingManager.rainFrequency > 0u)
						{
							if (Time.realtimeSinceStartup - LightingManager.lastRain > 1f)
							{
								LightingManager.rainFrequency -= 1u;
								LightingManager.lastRain = Time.realtimeSinceStartup;
							}
						}
						else
						{
							LightingManager.manager.channel.send("tellLightingRain", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
							{
								1
							});
							LightingManager.lastRain = Time.realtimeSinceStartup;
						}
						break;
					case ELightingRain.PRE_DRIZZLE:
						if (Time.realtimeSinceStartup - LightingManager.lastRain > 20f)
						{
							LightingManager.manager.channel.send("tellLightingRain", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
							{
								2
							});
							LightingManager.lastRain = Time.realtimeSinceStartup;
						}
						break;
					case ELightingRain.DRIZZLE:
						if (LightingManager.rainDuration > 0u)
						{
							if (Time.realtimeSinceStartup - LightingManager.lastRain > 1f)
							{
								LightingManager.rainDuration -= 1u;
								LightingManager.lastRain = Time.realtimeSinceStartup;
							}
						}
						else
						{
							LightingManager.manager.channel.send("tellLightingRain", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
							{
								3
							});
							LightingManager.lastRain = Time.realtimeSinceStartup;
						}
						break;
					case ELightingRain.POST_DRIZZLE:
						if (Time.realtimeSinceStartup - LightingManager.lastRain > 20f)
						{
							LightingManager.manager.channel.send("tellLightingRain", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
							{
								0
							});
							LightingManager._hasRain = false;
						}
						break;
					}
				}
				if (LevelLighting.canSnow)
				{
					if (!LightingManager.hasSnow)
					{
						LightingManager.snowFrequency = (uint)(Random.Range(Provider.modeConfigData.Events.Snow_Frequency_Min, Provider.modeConfigData.Events.Snow_Frequency_Max) * LightingManager.cycle * LevelLighting.snowFreq);
						LightingManager.snowDuration = (uint)(Random.Range(Provider.modeConfigData.Events.Snow_Duration_Min, Provider.modeConfigData.Events.Snow_Duration_Max) * LightingManager.cycle * LevelLighting.snowDur);
						LightingManager._hasSnow = true;
						LightingManager.lastSnow = Time.realtimeSinceStartup;
					}
					switch (LevelLighting.snowyness)
					{
					case ELightingSnow.NONE:
						if (LightingManager.snowFrequency > 0u)
						{
							if (Time.realtimeSinceStartup - LightingManager.lastSnow > 1f)
							{
								LightingManager.snowFrequency -= 1u;
								LightingManager.lastSnow = Time.realtimeSinceStartup;
							}
						}
						else
						{
							LightingManager.manager.channel.send("tellLightingSnow", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
							{
								1
							});
							LightingManager.lastSnow = Time.realtimeSinceStartup;
						}
						break;
					case ELightingSnow.PRE_BLIZZARD:
						if (Time.realtimeSinceStartup - LightingManager.lastSnow > 20f)
						{
							LightingManager.manager.channel.send("tellLightingSnow", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
							{
								2
							});
							LightingManager.lastSnow = Time.realtimeSinceStartup;
						}
						break;
					case ELightingSnow.BLIZZARD:
						if (LightingManager.snowDuration > 0u)
						{
							if (Time.realtimeSinceStartup - LightingManager.lastSnow > 1f)
							{
								LightingManager.snowDuration -= 1u;
								LightingManager.lastSnow = Time.realtimeSinceStartup;
							}
						}
						else
						{
							LightingManager.manager.channel.send("tellLightingSnow", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
							{
								3
							});
							LightingManager.lastSnow = Time.realtimeSinceStartup;
						}
						break;
					case ELightingSnow.POST_BLIZZARD:
						if (Time.realtimeSinceStartup - LightingManager.lastSnow > 20f)
						{
							LightingManager.manager.channel.send("tellLightingSnow", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
							{
								0
							});
							LightingManager._hasSnow = false;
						}
						break;
					}
				}
			}
		}

		private void Start()
		{
			LightingManager.manager = this;
			Level.onLevelLoaded = (LevelLoaded)Delegate.Combine(Level.onLevelLoaded, new LevelLoaded(this.onLevelLoaded));
			Provider.onClientConnected = (Provider.ClientConnected)Delegate.Combine(Provider.onClientConnected, new Provider.ClientConnected(this.onClientConnected));
		}

		public static void load()
		{
			if (LevelSavedata.fileExists("/Lighting.dat"))
			{
				River river = LevelSavedata.openRiver("/Lighting.dat", true);
				byte b = river.readByte();
				if (b > 0)
				{
					LightingManager._cycle = river.readUInt32();
					LightingManager._time = river.readUInt32();
					if (b > 1)
					{
						LightingManager.rainFrequency = river.readUInt32();
						LightingManager.rainDuration = river.readUInt32();
						LightingManager._hasRain = river.readBoolean();
						LevelLighting.rainyness = (ELightingRain)river.readByte();
					}
					else
					{
						LightingManager._hasRain = false;
						LevelLighting.rainyness = ELightingRain.NONE;
					}
					if (b > 2)
					{
						LightingManager.snowFrequency = river.readUInt32();
						LightingManager.snowDuration = river.readUInt32();
						LightingManager._hasSnow = river.readBoolean();
						LevelLighting.snowyness = (ELightingSnow)river.readByte();
					}
					else
					{
						LightingManager._hasSnow = false;
						LevelLighting.snowyness = ELightingSnow.NONE;
					}
					LightingManager._offset = Provider.time - LightingManager.time;
					return;
				}
			}
			LightingManager._time = (uint)(LightingManager.cycle * LevelLighting.transition);
			LightingManager._offset = Provider.time - LightingManager.time;
			LightingManager._hasRain = false;
			LevelLighting.rainyness = ELightingRain.NONE;
			LightingManager._hasSnow = false;
			LevelLighting.snowyness = ELightingSnow.NONE;
		}

		public static void save()
		{
			River river = LevelSavedata.openRiver("/Lighting.dat", false);
			river.writeByte(LightingManager.SAVEDATA_VERSION);
			river.writeUInt32(LightingManager.cycle);
			river.writeUInt32(LightingManager.time);
			river.writeUInt32(LightingManager.rainFrequency);
			river.writeUInt32(LightingManager.rainDuration);
			river.writeBoolean(LightingManager.hasRain);
			river.writeByte((byte)LevelLighting.rainyness);
			river.writeUInt32(LightingManager.snowFrequency);
			river.writeUInt32(LightingManager.snowDuration);
			river.writeBoolean(LightingManager.hasSnow);
			river.writeByte((byte)LevelLighting.snowyness);
		}

		public static readonly byte SAVEDATA_VERSION = 3;

		public static DayNightUpdated onDayNightUpdated;

		public static MoonUpdated onMoonUpdated;

		public static RainUpdated onRainUpdated;

		public static SnowUpdated onSnowUpdated;

		private static LightingManager manager;

		private static uint _cycle;

		private static uint _time;

		private static uint _offset;

		public static uint rainFrequency;

		public static uint rainDuration;

		private static bool _hasRain;

		private static float lastRain;

		public static uint snowFrequency;

		public static uint snowDuration;

		private static bool _hasSnow;

		private static float lastSnow;

		private static bool isCycled;

		private static bool _isFullMoon;

		private static float lastWind;

		private static float windDelay;
	}
}
