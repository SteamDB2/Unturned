using System;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	public class ZombieManager : SteamCaller
	{
		public static ZombieManager instance
		{
			get
			{
				return ZombieManager.manager;
			}
		}

		public static ZombieRegion[] regions
		{
			get
			{
				return ZombieManager._regions;
			}
		}

		public static List<Zombie> tickingZombies
		{
			get
			{
				return ZombieManager._tickingZombies;
			}
		}

		public static bool canSpareWanderer
		{
			get
			{
				return ZombieManager.wanderingCount < 8 && ZombieManager.tickingZombies.Count < 50;
			}
		}

		public static bool waveReady
		{
			get
			{
				return ZombieManager._waveReady;
			}
		}

		public static int waveIndex
		{
			get
			{
				return ZombieManager._waveIndex;
			}
		}

		public static int waveRemaining
		{
			get
			{
				return ZombieManager._waveRemaining;
			}
		}

		public static void getZombiesInRadius(Vector3 center, float sqrRadius, List<Zombie> result)
		{
			if (ZombieManager.regions == null)
			{
				return;
			}
			byte b;
			if (!LevelNavigation.tryGetNavigation(center, out b))
			{
				return;
			}
			if (ZombieManager.regions[(int)b] == null || ZombieManager.regions[(int)b].zombies == null)
			{
				return;
			}
			for (int i = 0; i < ZombieManager.regions[(int)b].zombies.Count; i++)
			{
				Zombie zombie = ZombieManager.regions[(int)b].zombies[i];
				if (!(zombie == null))
				{
					if ((zombie.transform.position - center).sqrMagnitude < sqrRadius)
					{
						result.Add(zombie);
					}
				}
			}
		}

		[SteamCall]
		public void tellBeacon(CSteamID steamID, byte reference, bool hasBeacon)
		{
			if (base.channel.checkServer(steamID))
			{
				if (ZombieManager.regions == null || (int)reference >= ZombieManager.regions.Length)
				{
					return;
				}
				if (!Provider.isServer && !ZombieManager.regions[(int)reference].isNetworked)
				{
					return;
				}
				ZombieManager.regions[(int)reference].hasBeacon = hasBeacon;
			}
		}

		[SteamCall]
		public void tellWave(CSteamID steamID, bool newWaveReady, int newWave)
		{
			if (base.channel.checkServer(steamID))
			{
				ZombieManager._waveReady = newWaveReady;
				ZombieManager._waveIndex = newWave;
				if (ZombieManager.onWaveUpdated != null)
				{
					ZombieManager.onWaveUpdated(ZombieManager.waveReady, ZombieManager.waveIndex);
				}
			}
		}

		[SteamCall]
		public void askWave(CSteamID steamID)
		{
			base.channel.send("tellWave", steamID, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
			{
				ZombieManager.waveReady,
				ZombieManager.waveIndex
			});
		}

		[SteamCall]
		public void tellZombieAlive(CSteamID steamID, byte reference, ushort id, byte newType, byte newSpeciality, byte newShirt, byte newPants, byte newHat, byte newGear, Vector3 newPosition, byte newAngle)
		{
			if (base.channel.checkServer(steamID))
			{
				if (!Provider.isServer && !ZombieManager.regions[(int)reference].isNetworked)
				{
					return;
				}
				if ((int)id >= ZombieManager.regions[(int)reference].zombies.Count)
				{
					return;
				}
				ZombieManager.regions[(int)reference].zombies[(int)id].tellAlive(newType, newSpeciality, newShirt, newPants, newHat, newGear, newPosition, newAngle);
			}
		}

		[SteamCall]
		public void tellZombieDead(CSteamID steamID, byte reference, ushort id, Vector3 newRagdoll)
		{
			if (base.channel.checkServer(steamID))
			{
				if (!Provider.isServer && !ZombieManager.regions[(int)reference].isNetworked)
				{
					return;
				}
				if ((int)id >= ZombieManager.regions[(int)reference].zombies.Count)
				{
					return;
				}
				ZombieManager.regions[(int)reference].zombies[(int)id].tellDead(newRagdoll);
			}
		}

		[SteamCall]
		public void tellZombieStates(CSteamID steamID)
		{
			if (base.channel.checkServer(steamID))
			{
				byte b = (byte)base.channel.read(Types.BYTE_TYPE);
				if (!Provider.isServer && !ZombieManager.regions[(int)b].isNetworked)
				{
					return;
				}
				uint num = (uint)base.channel.read(Types.UINT32_TYPE);
				if (num <= this.seq)
				{
					return;
				}
				this.seq = num;
				base.channel.useCompression = true;
				ushort num2 = (ushort)base.channel.read(Types.UINT16_TYPE);
				for (int i = 0; i < (int)num2; i++)
				{
					object[] array = base.channel.read(Types.UINT16_TYPE, Types.VECTOR3_TYPE, Types.BYTE_TYPE);
					if ((int)((ushort)array[0]) >= ZombieManager.regions[(int)b].zombies.Count)
					{
						break;
					}
					ZombieManager.regions[(int)b].zombies[(int)((ushort)array[0])].tellState((Vector3)array[1], (byte)array[2]);
				}
				base.channel.useCompression = false;
			}
		}

		[SteamCall]
		public void tellZombieSpeciality(CSteamID steamID, byte reference, ushort id, byte speciality)
		{
			if (base.channel.checkServer(steamID))
			{
				if (!Provider.isServer && !ZombieManager.regions[(int)reference].isNetworked)
				{
					return;
				}
				if ((int)id >= ZombieManager.regions[(int)reference].zombies.Count)
				{
					return;
				}
				ZombieManager.regions[(int)reference].zombies[(int)id].tellSpeciality((EZombieSpeciality)speciality);
			}
		}

		[SteamCall]
		public void askZombieThrow(CSteamID steamID, byte reference, ushort id)
		{
			if (base.channel.checkServer(steamID))
			{
				if (!Provider.isServer && !ZombieManager.regions[(int)reference].isNetworked)
				{
					return;
				}
				if ((int)id >= ZombieManager.regions[(int)reference].zombies.Count)
				{
					return;
				}
				ZombieManager.regions[(int)reference].zombies[(int)id].askThrow();
			}
		}

		[SteamCall]
		public void askZombieBoulder(CSteamID steamID, byte reference, ushort id, Vector3 origin, Vector3 direction)
		{
			if (base.channel.checkServer(steamID))
			{
				if (!Provider.isServer && !ZombieManager.regions[(int)reference].isNetworked)
				{
					return;
				}
				if ((int)id >= ZombieManager.regions[(int)reference].zombies.Count)
				{
					return;
				}
				ZombieManager.regions[(int)reference].zombies[(int)id].askBoulder(origin, direction);
			}
		}

		[SteamCall]
		public void askZombieSpit(CSteamID steamID, byte reference, ushort id)
		{
			if (base.channel.checkServer(steamID))
			{
				if (!Provider.isServer && !ZombieManager.regions[(int)reference].isNetworked)
				{
					return;
				}
				if ((int)id >= ZombieManager.regions[(int)reference].zombies.Count)
				{
					return;
				}
				ZombieManager.regions[(int)reference].zombies[(int)id].askSpit();
			}
		}

		[SteamCall]
		public void askZombieCharge(CSteamID steamID, byte reference, ushort id)
		{
			if (base.channel.checkServer(steamID))
			{
				if (!Provider.isServer && !ZombieManager.regions[(int)reference].isNetworked)
				{
					return;
				}
				if ((int)id >= ZombieManager.regions[(int)reference].zombies.Count)
				{
					return;
				}
				ZombieManager.regions[(int)reference].zombies[(int)id].askCharge();
			}
		}

		[SteamCall]
		public void askZombieStomp(CSteamID steamID, byte reference, ushort id)
		{
			if (base.channel.checkServer(steamID))
			{
				if (!Provider.isServer && !ZombieManager.regions[(int)reference].isNetworked)
				{
					return;
				}
				if ((int)id >= ZombieManager.regions[(int)reference].zombies.Count)
				{
					return;
				}
				ZombieManager.regions[(int)reference].zombies[(int)id].askStomp();
			}
		}

		[SteamCall]
		public void askZombieBreath(CSteamID steamID, byte reference, ushort id)
		{
			if (base.channel.checkServer(steamID))
			{
				if (!Provider.isServer && !ZombieManager.regions[(int)reference].isNetworked)
				{
					return;
				}
				if ((int)id >= ZombieManager.regions[(int)reference].zombies.Count)
				{
					return;
				}
				ZombieManager.regions[(int)reference].zombies[(int)id].askBreath();
			}
		}

		[SteamCall]
		public void askZombieAcid(CSteamID steamID, byte reference, ushort id, Vector3 origin, Vector3 direction)
		{
			if (base.channel.checkServer(steamID))
			{
				if (!Provider.isServer && !ZombieManager.regions[(int)reference].isNetworked)
				{
					return;
				}
				if ((int)id >= ZombieManager.regions[(int)reference].zombies.Count)
				{
					return;
				}
				ZombieManager.regions[(int)reference].zombies[(int)id].askAcid(origin, direction);
			}
		}

		[SteamCall]
		public void askZombieSpark(CSteamID steamID, byte reference, ushort id, Vector3 target)
		{
			if (base.channel.checkServer(steamID))
			{
				if (!Provider.isServer && !ZombieManager.regions[(int)reference].isNetworked)
				{
					return;
				}
				if ((int)id >= ZombieManager.regions[(int)reference].zombies.Count)
				{
					return;
				}
				ZombieManager.regions[(int)reference].zombies[(int)id].askSpark(target);
			}
		}

		[SteamCall]
		public void askZombieAttack(CSteamID steamID, byte reference, ushort id, byte attack)
		{
			if (base.channel.checkServer(steamID))
			{
				if (!Provider.isServer && !ZombieManager.regions[(int)reference].isNetworked)
				{
					return;
				}
				if ((int)id >= ZombieManager.regions[(int)reference].zombies.Count)
				{
					return;
				}
				ZombieManager.regions[(int)reference].zombies[(int)id].askAttack(attack);
			}
		}

		[SteamCall]
		public void askZombieStartle(CSteamID steamID, byte reference, ushort id, byte startle)
		{
			if (base.channel.checkServer(steamID))
			{
				if (!Provider.isServer && !ZombieManager.regions[(int)reference].isNetworked)
				{
					return;
				}
				if ((int)id >= ZombieManager.regions[(int)reference].zombies.Count)
				{
					return;
				}
				ZombieManager.regions[(int)reference].zombies[(int)id].askStartle(startle);
			}
		}

		[SteamCall]
		public void askZombieStun(CSteamID steamID, byte reference, ushort id, byte stun)
		{
			if (base.channel.checkServer(steamID))
			{
				if (!Provider.isServer && !ZombieManager.regions[(int)reference].isNetworked)
				{
					return;
				}
				if ((int)id >= ZombieManager.regions[(int)reference].zombies.Count)
				{
					return;
				}
				ZombieManager.regions[(int)reference].zombies[(int)id].askStun(stun);
			}
		}

		[SteamCall]
		public void tellZombies(CSteamID steamID)
		{
			if (base.channel.checkServer(steamID))
			{
				byte b = (byte)base.channel.read(Types.BYTE_TYPE);
				if (ZombieManager.regions[(int)b].isNetworked)
				{
					return;
				}
				ZombieManager.regions[(int)b].isNetworked = true;
				bool hasBeacon = (bool)base.channel.read(Types.BOOLEAN_TYPE);
				ushort num = (ushort)base.channel.read(Types.UINT16_TYPE);
				for (int i = 0; i < (int)num; i++)
				{
					object[] array = base.channel.read(new Type[]
					{
						Types.BYTE_TYPE,
						Types.BYTE_TYPE,
						Types.BYTE_TYPE,
						Types.BYTE_TYPE,
						Types.BYTE_TYPE,
						Types.BYTE_TYPE,
						Types.BYTE_TYPE,
						Types.BYTE_TYPE,
						Types.VECTOR3_TYPE,
						Types.BYTE_TYPE,
						Types.BOOLEAN_TYPE
					});
					this.addZombie(b, (byte)array[0], (byte)array[1], (byte)array[2], (byte)array[3], (byte)array[4], (byte)array[5], (byte)array[6], (byte)array[7], (Vector3)array[8], (float)((byte)array[9] * 2), (bool)array[10]);
				}
				ZombieManager.regions[(int)b].hasBeacon = hasBeacon;
			}
		}

		public void askZombies(CSteamID steamID, byte bound)
		{
			base.channel.openWrite();
			base.channel.write(bound);
			base.channel.write(ZombieManager.regions[(int)bound].hasBeacon);
			base.channel.write((ushort)ZombieManager.regions[(int)bound].zombies.Count);
			for (int i = 0; i < ZombieManager.regions[(int)bound].zombies.Count; i++)
			{
				Zombie zombie = ZombieManager.regions[(int)bound].zombies[i];
				base.channel.write(new object[]
				{
					zombie.type,
					(byte)zombie.speciality,
					zombie.shirt,
					zombie.pants,
					zombie.hat,
					zombie.gear,
					zombie.move,
					zombie.idle,
					zombie.transform.position,
					MeasurementTool.angleToByte(zombie.transform.rotation.eulerAngles.y),
					zombie.isDead
				});
			}
			base.channel.closeWrite("tellZombies", steamID, ESteamPacket.UPDATE_RELIABLE_CHUNK_BUFFER);
		}

		public static void sendZombieAlive(Zombie zombie, byte newType, byte newSpeciality, byte newShirt, byte newPants, byte newHat, byte newGear, Vector3 newPosition, byte newAngle)
		{
			ZombieManager.manager.channel.send("tellZombieAlive", ESteamCall.ALL, zombie.bound, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
			{
				zombie.bound,
				zombie.id,
				newType,
				newSpeciality,
				newShirt,
				newPants,
				newHat,
				newGear,
				newPosition,
				newAngle
			});
			if (ZombieManager.regions[(int)zombie.bound].onZombieLifeUpdated != null)
			{
				ZombieManager.regions[(int)zombie.bound].onZombieLifeUpdated(zombie);
			}
		}

		public static void sendZombieDead(Zombie zombie, Vector3 newRagdoll)
		{
			ZombieManager.manager.channel.send("tellZombieDead", ESteamCall.ALL, zombie.bound, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
			{
				zombie.bound,
				zombie.id,
				newRagdoll
			});
			if (ZombieManager.regions[(int)zombie.bound].onZombieLifeUpdated != null)
			{
				ZombieManager.regions[(int)zombie.bound].onZombieLifeUpdated(zombie);
			}
		}

		public static void sendZombieSpeciality(Zombie zombie, EZombieSpeciality speciality)
		{
			ZombieManager.manager.channel.send("tellZombieSpeciality", ESteamCall.ALL, zombie.bound, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[]
			{
				zombie.bound,
				zombie.id,
				(byte)speciality
			});
		}

		public static void sendZombieThrow(Zombie zombie)
		{
			ZombieManager.manager.channel.send("askZombieThrow", ESteamCall.ALL, zombie.bound, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[]
			{
				zombie.bound,
				zombie.id
			});
		}

		public static void sendZombieSpit(Zombie zombie)
		{
			ZombieManager.manager.channel.send("askZombieSpit", ESteamCall.ALL, zombie.bound, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[]
			{
				zombie.bound,
				zombie.id
			});
		}

		public static void sendZombieCharge(Zombie zombie)
		{
			ZombieManager.manager.channel.send("askZombieCharge", ESteamCall.ALL, zombie.bound, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[]
			{
				zombie.bound,
				zombie.id
			});
		}

		public static void sendZombieStomp(Zombie zombie)
		{
			ZombieManager.manager.channel.send("askZombieStomp", ESteamCall.ALL, zombie.bound, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[]
			{
				zombie.bound,
				zombie.id
			});
		}

		public static void sendZombieBreath(Zombie zombie)
		{
			ZombieManager.manager.channel.send("askZombieBreath", ESteamCall.ALL, zombie.bound, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[]
			{
				zombie.bound,
				zombie.id
			});
		}

		public static void sendZombieBoulder(Zombie zombie, Vector3 origin, Vector3 direction)
		{
			ZombieManager.manager.channel.send("askZombieBoulder", ESteamCall.ALL, zombie.bound, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[]
			{
				zombie.bound,
				zombie.id,
				origin,
				direction
			});
		}

		public static void sendZombieAcid(Zombie zombie, Vector3 origin, Vector3 direction)
		{
			ZombieManager.manager.channel.send("askZombieAcid", ESteamCall.ALL, zombie.bound, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[]
			{
				zombie.bound,
				zombie.id,
				origin,
				direction
			});
		}

		public static void sendZombieSpark(Zombie zombie, Vector3 target)
		{
			ZombieManager.manager.channel.send("askZombieSpark", ESteamCall.CLIENTS, zombie.bound, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[]
			{
				zombie.bound,
				zombie.id,
				target
			});
		}

		public static void sendZombieAttack(Zombie zombie, byte attack)
		{
			ZombieManager.manager.channel.send("askZombieAttack", ESteamCall.ALL, zombie.bound, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[]
			{
				zombie.bound,
				zombie.id,
				attack
			});
		}

		public static void sendZombieStartle(Zombie zombie, byte startle)
		{
			ZombieManager.manager.channel.send("askZombieStartle", ESteamCall.ALL, zombie.bound, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[]
			{
				zombie.bound,
				zombie.id,
				startle
			});
		}

		public static void sendZombieStun(Zombie zombie, byte stun)
		{
			ZombieManager.manager.channel.send("askZombieStun", ESteamCall.ALL, zombie.bound, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[]
			{
				zombie.bound,
				zombie.id,
				stun
			});
		}

		public static void dropLoot(Zombie zombie)
		{
			int num;
			if (zombie.isBoss || zombie.speciality == EZombieSpeciality.BOSS_ALL)
			{
				num = Random.Range((int)Provider.modeConfigData.Zombies.Min_Boss_Drops, (int)(Provider.modeConfigData.Zombies.Max_Boss_Drops + 1u));
			}
			else if (zombie.isMega)
			{
				num = Random.Range((int)Provider.modeConfigData.Zombies.Min_Mega_Drops, (int)(Provider.modeConfigData.Zombies.Max_Mega_Drops + 1u));
			}
			else
			{
				num = Random.Range((int)Provider.modeConfigData.Zombies.Min_Drops, (int)(Provider.modeConfigData.Zombies.Max_Drops + 1u));
			}
			if (LevelZombies.tables[(int)zombie.type].isMega)
			{
				ZombieManager.regions[(int)zombie.bound].lastMega = Time.realtimeSinceStartup;
				ZombieManager.regions[(int)zombie.bound].hasMega = false;
			}
			if (num > 1 || Random.value < Provider.modeConfigData.Zombies.Loot_Chance)
			{
				if (LevelZombies.tables[(int)zombie.type].lootID != 0)
				{
					for (int i = 0; i < num; i++)
					{
						ushort num2 = SpawnTableTool.resolve(LevelZombies.tables[(int)zombie.type].lootID);
						if (num2 != 0)
						{
							Item item = new Item(num2, EItemOrigin.WORLD);
							ItemManager.dropItem(item, zombie.transform.position, false, Dedicator.isDedicated, true);
						}
					}
				}
				else if ((int)LevelZombies.tables[(int)zombie.type].lootIndex < LevelItems.tables.Count)
				{
					for (int j = 0; j < num; j++)
					{
						ushort item2 = LevelItems.getItem(LevelZombies.tables[(int)zombie.type].lootIndex);
						if (item2 != 0)
						{
							Item item3 = new Item(item2, EItemOrigin.WORLD);
							ItemManager.dropItem(item3, zombie.transform.position, false, Dedicator.isDedicated, true);
						}
					}
				}
			}
		}

		public void addZombie(byte bound, byte type, byte speciality, byte shirt, byte pants, byte hat, byte gear, byte move, byte idle, Vector3 position, float angle, bool isDead)
		{
			Transform transform;
			if (Dedicator.isDedicated)
			{
				transform = ((GameObject)Object.Instantiate(Resources.Load("Characters/Zombie_Dedicated"))).transform;
			}
			else if (Provider.isServer)
			{
				transform = ((GameObject)Object.Instantiate(Resources.Load("Characters/Zombie_Server"))).transform;
			}
			else
			{
				transform = ((GameObject)Object.Instantiate(Resources.Load("Characters/Zombie_Client"))).transform;
			}
			transform.name = "Zombie";
			transform.parent = LevelZombies.models;
			transform.position = position;
			transform.rotation = Quaternion.Euler(0f, angle, 0f);
			Zombie component = transform.GetComponent<Zombie>();
			component.id = (ushort)ZombieManager.regions[(int)bound].zombies.Count;
			component.speciality = (EZombieSpeciality)speciality;
			component.bound = bound;
			component.type = type;
			component.shirt = shirt;
			component.pants = pants;
			component.hat = hat;
			component.gear = gear;
			component.move = move;
			component.idle = idle;
			component.isDead = isDead;
			component.init();
			if (!isDead)
			{
				ZombieManager.regions[(int)bound].alive++;
			}
			ZombieManager.regions[(int)bound].zombies.Add(transform.GetComponent<Zombie>());
		}

		public static Zombie getZombie(Vector3 point, ushort id)
		{
			byte b;
			if (!LevelNavigation.tryGetBounds(point, out b))
			{
				return null;
			}
			if ((int)id >= ZombieManager.regions[(int)b].zombies.Count)
			{
				return null;
			}
			if (ZombieManager.regions[(int)b].zombies[(int)id].isDead)
			{
				return null;
			}
			return ZombieManager.regions[(int)b].zombies[(int)id];
		}

		private static void generateZombieSpeciality(ref EZombieSpeciality speciality, byte bound, ZombieTable table)
		{
			float value = Random.value;
			AssetReference<ZombieDifficultyAsset> reference = AssetReference<ZombieDifficultyAsset>.invalid;
			if ((int)bound < LevelNavigation.flagData.Count && LevelNavigation.flagData[(int)bound].difficulty.isValid)
			{
				reference = LevelNavigation.flagData[(int)bound].difficulty;
			}
			else if (table.difficulty.isValid)
			{
				reference = table.difficulty;
			}
			if (reference.isValid)
			{
				ZombieDifficultyAsset zombieDifficultyAsset = Assets.find<ZombieDifficultyAsset>(reference);
				if (zombieDifficultyAsset != null)
				{
					if (value < zombieDifficultyAsset.Crawler_Chance)
					{
						speciality = EZombieSpeciality.CRAWLER;
					}
					else if (value < zombieDifficultyAsset.Crawler_Chance + zombieDifficultyAsset.Sprinter_Chance)
					{
						speciality = EZombieSpeciality.SPRINTER;
					}
					else if (value < zombieDifficultyAsset.Crawler_Chance + zombieDifficultyAsset.Sprinter_Chance + zombieDifficultyAsset.Flanker_Chance)
					{
						speciality = EZombieSpeciality.FLANKER_FRIENDLY;
					}
					else if (value < zombieDifficultyAsset.Crawler_Chance + zombieDifficultyAsset.Sprinter_Chance + zombieDifficultyAsset.Flanker_Chance + zombieDifficultyAsset.Burner_Chance)
					{
						speciality = EZombieSpeciality.BURNER;
					}
					else if (value < zombieDifficultyAsset.Crawler_Chance + zombieDifficultyAsset.Sprinter_Chance + zombieDifficultyAsset.Flanker_Chance + zombieDifficultyAsset.Burner_Chance + zombieDifficultyAsset.Acid_Chance)
					{
						speciality = EZombieSpeciality.ACID;
					}
					else if (value < zombieDifficultyAsset.Crawler_Chance + zombieDifficultyAsset.Sprinter_Chance + zombieDifficultyAsset.Flanker_Chance + zombieDifficultyAsset.Burner_Chance + zombieDifficultyAsset.Acid_Chance + zombieDifficultyAsset.Boss_Electric_Chance)
					{
						speciality = EZombieSpeciality.BOSS_ELECTRIC;
					}
					else if (value < zombieDifficultyAsset.Crawler_Chance + zombieDifficultyAsset.Sprinter_Chance + zombieDifficultyAsset.Flanker_Chance + zombieDifficultyAsset.Burner_Chance + zombieDifficultyAsset.Acid_Chance + zombieDifficultyAsset.Boss_Electric_Chance + zombieDifficultyAsset.Boss_Wind_Chance)
					{
						speciality = EZombieSpeciality.BOSS_WIND;
					}
					else if (value < zombieDifficultyAsset.Crawler_Chance + zombieDifficultyAsset.Sprinter_Chance + zombieDifficultyAsset.Flanker_Chance + zombieDifficultyAsset.Burner_Chance + zombieDifficultyAsset.Acid_Chance + zombieDifficultyAsset.Boss_Electric_Chance + zombieDifficultyAsset.Boss_Wind_Chance + zombieDifficultyAsset.Boss_Fire_Chance)
					{
						speciality = EZombieSpeciality.BOSS_FIRE;
					}
					return;
				}
			}
			if (value < Provider.modeConfigData.Zombies.Crawler_Chance)
			{
				speciality = EZombieSpeciality.CRAWLER;
			}
			else if (value < Provider.modeConfigData.Zombies.Crawler_Chance + Provider.modeConfigData.Zombies.Sprinter_Chance)
			{
				speciality = EZombieSpeciality.SPRINTER;
			}
			else if (value < Provider.modeConfigData.Zombies.Crawler_Chance + Provider.modeConfigData.Zombies.Sprinter_Chance + Provider.modeConfigData.Zombies.Flanker_Chance)
			{
				speciality = EZombieSpeciality.FLANKER_FRIENDLY;
			}
			else if (value < Provider.modeConfigData.Zombies.Crawler_Chance + Provider.modeConfigData.Zombies.Sprinter_Chance + Provider.modeConfigData.Zombies.Flanker_Chance + Provider.modeConfigData.Zombies.Burner_Chance)
			{
				speciality = EZombieSpeciality.BURNER;
			}
			else if (value < Provider.modeConfigData.Zombies.Crawler_Chance + Provider.modeConfigData.Zombies.Sprinter_Chance + Provider.modeConfigData.Zombies.Flanker_Chance + Provider.modeConfigData.Zombies.Burner_Chance + Provider.modeConfigData.Zombies.Acid_Chance)
			{
				speciality = EZombieSpeciality.ACID;
			}
			else if (value < Provider.modeConfigData.Zombies.Crawler_Chance + Provider.modeConfigData.Zombies.Sprinter_Chance + Provider.modeConfigData.Zombies.Flanker_Chance + Provider.modeConfigData.Zombies.Burner_Chance + Provider.modeConfigData.Zombies.Acid_Chance + Provider.modeConfigData.Zombies.Boss_Electric_Chance)
			{
				speciality = EZombieSpeciality.BOSS_ELECTRIC;
			}
			else if (value < Provider.modeConfigData.Zombies.Crawler_Chance + Provider.modeConfigData.Zombies.Sprinter_Chance + Provider.modeConfigData.Zombies.Flanker_Chance + Provider.modeConfigData.Zombies.Burner_Chance + Provider.modeConfigData.Zombies.Acid_Chance + Provider.modeConfigData.Zombies.Boss_Electric_Chance + Provider.modeConfigData.Zombies.Boss_Wind_Chance)
			{
				speciality = EZombieSpeciality.BOSS_WIND;
			}
			else if (value < Provider.modeConfigData.Zombies.Crawler_Chance + Provider.modeConfigData.Zombies.Sprinter_Chance + Provider.modeConfigData.Zombies.Flanker_Chance + Provider.modeConfigData.Zombies.Burner_Chance + Provider.modeConfigData.Zombies.Acid_Chance + Provider.modeConfigData.Zombies.Boss_Electric_Chance + Provider.modeConfigData.Zombies.Boss_Wind_Chance + Provider.modeConfigData.Zombies.Boss_Fire_Chance)
			{
				speciality = EZombieSpeciality.BOSS_FIRE;
			}
		}

		public void generateZombies(byte bound)
		{
			if (LevelNavigation.bounds.Count == 0 || LevelZombies.zombies.Length == 0 || LevelNavigation.bounds.Count != LevelZombies.zombies.Length)
			{
				return;
			}
			if (LevelZombies.zombies[(int)bound].Count > 0)
			{
				ZombieManager.regions[(int)bound].alive = 0;
				List<ZombieSpawnpoint> list = new List<ZombieSpawnpoint>();
				for (int i = 0; i < LevelZombies.zombies[(int)bound].Count; i++)
				{
					ZombieSpawnpoint zombieSpawnpoint = LevelZombies.zombies[(int)bound][i];
					if (SafezoneManager.checkPointValid(zombieSpawnpoint.point))
					{
						list.Add(zombieSpawnpoint);
					}
				}
				while (list.Count > 0)
				{
					if (Level.info.type == ELevelType.HORDE)
					{
						if (ZombieManager.regions[(int)bound].zombies.Count >= 40)
						{
							break;
						}
					}
					else if (ZombieManager.regions[(int)bound].zombies.Count >= Mathf.Min((int)LevelNavigation.flagData[(int)bound].maxZombies, Mathf.CeilToInt((float)LevelZombies.zombies[(int)bound].Count * Provider.modeConfigData.Zombies.Spawn_Chance)))
					{
						break;
					}
					int index = Random.Range(0, list.Count);
					ZombieSpawnpoint zombieSpawnpoint2 = list[index];
					list.RemoveAt(index);
					byte type = zombieSpawnpoint2.type;
					ZombieTable zombieTable = LevelZombies.tables[(int)type];
					if (this.canRegionSpawnZombiesFromTable(ZombieManager.regions[(int)bound], zombieTable))
					{
						EZombieSpeciality ezombieSpeciality = EZombieSpeciality.NORMAL;
						if (zombieTable.isMega)
						{
							ZombieManager.regions[(int)bound].lastMega = Time.realtimeSinceStartup;
							ZombieManager.regions[(int)bound].hasMega = true;
							ezombieSpeciality = EZombieSpeciality.MEGA;
						}
						else if (Level.info.type == ELevelType.SURVIVAL)
						{
							ZombieManager.generateZombieSpeciality(ref ezombieSpeciality, bound, zombieTable);
						}
						if (ZombieManager.regions[(int)bound].hasBeacon)
						{
							BeaconManager.checkBeacon(bound).spawnRemaining();
						}
						byte shirt = byte.MaxValue;
						if (zombieTable.slots[0].table.Count > 0 && Random.value < zombieTable.slots[0].chance)
						{
							shirt = (byte)Random.Range(0, zombieTable.slots[0].table.Count);
						}
						byte pants = byte.MaxValue;
						if (zombieTable.slots[1].table.Count > 0 && Random.value < zombieTable.slots[1].chance)
						{
							pants = (byte)Random.Range(0, zombieTable.slots[1].table.Count);
						}
						byte hat = byte.MaxValue;
						if (zombieTable.slots[2].table.Count > 0 && Random.value < zombieTable.slots[2].chance)
						{
							hat = (byte)Random.Range(0, zombieTable.slots[2].table.Count);
						}
						byte gear = byte.MaxValue;
						if (zombieTable.slots[3].table.Count > 0 && Random.value < zombieTable.slots[3].chance)
						{
							gear = (byte)Random.Range(0, zombieTable.slots[3].table.Count);
						}
						byte move = (byte)Random.Range(0, 4);
						byte idle = (byte)Random.Range(0, 3);
						Vector3 point = zombieSpawnpoint2.point;
						point.y += 0.1f;
						this.addZombie(bound, type, (byte)ezombieSpeciality, shirt, pants, hat, gear, move, idle, point, Random.Range(0f, 360f), !LevelNavigation.flagData[(int)bound].spawnZombies || Level.info.type == ELevelType.HORDE);
					}
				}
			}
		}

		private bool canRegionSpawnZombiesFromTable(ZombieRegion region, ZombieTable table)
		{
			if (region.hasBeacon)
			{
				return !table.isMega;
			}
			return !table.isMega || (!region.hasMega && Time.realtimeSinceStartup - region.lastMega > 600f);
		}

		public void respawnZombies()
		{
			if (Level.info.type == ELevelType.HORDE)
			{
				if (ZombieManager.waveRemaining > 0 || ZombieManager.regions[(int)ZombieManager.respawnZombiesBound].alive > 0)
				{
					ZombieManager.lastWave = Time.realtimeSinceStartup;
				}
				if (ZombieManager.waveRemaining == 0)
				{
					if (ZombieManager.regions[(int)ZombieManager.respawnZombiesBound].alive > 0)
					{
						return;
					}
					if (Time.realtimeSinceStartup - ZombieManager.lastWave <= 10f && ZombieManager.waveIndex != 0)
					{
						if (ZombieManager.waveReady)
						{
							ZombieManager._waveReady = false;
							base.channel.send("tellWave", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
							{
								ZombieManager.waveReady,
								ZombieManager.waveIndex
							});
						}
						return;
					}
					if (!ZombieManager.waveReady)
					{
						ZombieManager._waveReady = true;
						ZombieManager._waveIndex++;
						ZombieManager._waveRemaining = (int)Mathf.Ceil(Mathf.Pow((float)(ZombieManager.waveIndex + 5), 1.5f));
						base.channel.send("tellWave", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
						{
							ZombieManager.waveReady,
							ZombieManager.waveIndex
						});
					}
				}
			}
			if (!LevelNavigation.flagData[(int)ZombieManager.respawnZombiesBound].spawnZombies)
			{
				return;
			}
			if (ZombieManager.regions[(int)ZombieManager.respawnZombiesBound].zombies.Count > 0)
			{
				if (!Dedicator.isDedicated && !ZombieManager.regions[(int)ZombieManager.respawnZombiesBound].hasBeacon && Level.info.type != ELevelType.HORDE)
				{
					return;
				}
				if (ZombieManager.regions[(int)ZombieManager.respawnZombiesBound].hasBeacon && BeaconManager.checkBeacon(ZombieManager.respawnZombiesBound).getRemaining() == 0)
				{
					return;
				}
				if ((int)ZombieManager.regions[(int)ZombieManager.respawnZombiesBound].respawnZombieIndex >= ZombieManager.regions[(int)ZombieManager.respawnZombiesBound].zombies.Count)
				{
					ZombieManager.regions[(int)ZombieManager.respawnZombiesBound].respawnZombieIndex = (ushort)(ZombieManager.regions[(int)ZombieManager.respawnZombiesBound].zombies.Count - 1);
				}
				Zombie zombie = ZombieManager.regions[(int)ZombieManager.respawnZombiesBound].zombies[(int)ZombieManager.regions[(int)ZombieManager.respawnZombiesBound].respawnZombieIndex];
				ZombieRegion zombieRegion = ZombieManager.regions[(int)ZombieManager.respawnZombiesBound];
				zombieRegion.respawnZombieIndex += 1;
				if ((int)ZombieManager.regions[(int)ZombieManager.respawnZombiesBound].respawnZombieIndex >= ZombieManager.regions[(int)ZombieManager.respawnZombiesBound].zombies.Count)
				{
					ZombieManager.regions[(int)ZombieManager.respawnZombiesBound].respawnZombieIndex = 0;
				}
				float num = Provider.modeConfigData.Zombies.Respawn_Day_Time;
				if (ZombieManager.regions[(int)ZombieManager.respawnZombiesBound].hasBeacon)
				{
					num = Provider.modeConfigData.Zombies.Respawn_Beacon_Time;
				}
				else if (LightingManager.isFullMoon)
				{
					num = Provider.modeConfigData.Zombies.Respawn_Night_Time;
				}
				if (zombie.isDead && Time.realtimeSinceStartup - zombie.lastDead > num)
				{
					ZombieSpawnpoint zombieSpawnpoint = LevelZombies.zombies[(int)ZombieManager.respawnZombiesBound][Random.Range(0, LevelZombies.zombies[(int)ZombieManager.respawnZombiesBound].Count)];
					if (!SafezoneManager.checkPointValid(zombieSpawnpoint.point))
					{
						return;
					}
					ushort num2 = 0;
					while ((int)num2 < ZombieManager.regions[(int)ZombieManager.respawnZombiesBound].zombies.Count)
					{
						if (!ZombieManager.regions[(int)ZombieManager.respawnZombiesBound].zombies[(int)num2].isDead && (ZombieManager.regions[(int)ZombieManager.respawnZombiesBound].zombies[(int)num2].transform.position - zombieSpawnpoint.point).sqrMagnitude < 4f)
						{
							return;
						}
						num2 += 1;
					}
					byte b = zombieSpawnpoint.type;
					ZombieTable zombieTable = LevelZombies.tables[(int)b];
					if (this.canRegionSpawnZombiesFromTable(ZombieManager.regions[(int)ZombieManager.respawnZombiesBound], zombieTable))
					{
						EZombieSpeciality ezombieSpeciality = EZombieSpeciality.NORMAL;
						if ((!ZombieManager.regions[(int)ZombieManager.respawnZombiesBound].hasBeacon) ? zombieTable.isMega : (BeaconManager.checkBeacon(ZombieManager.respawnZombiesBound).getRemaining() == 1))
						{
							if (!zombieTable.isMega)
							{
								byte b2 = 0;
								while ((int)b2 < LevelZombies.tables.Count)
								{
									ZombieTable zombieTable2 = LevelZombies.tables[(int)b2];
									if (zombieTable2.isMega)
									{
										b = b2;
										zombieTable = zombieTable2;
										break;
									}
									b2 += 1;
								}
							}
							ZombieManager.regions[(int)ZombieManager.respawnZombiesBound].lastMega = Time.realtimeSinceStartup;
							ZombieManager.regions[(int)ZombieManager.respawnZombiesBound].hasMega = true;
							ezombieSpeciality = EZombieSpeciality.MEGA;
						}
						else if (Level.info.type == ELevelType.SURVIVAL)
						{
							ZombieManager.generateZombieSpeciality(ref ezombieSpeciality, ZombieManager.respawnZombiesBound, zombieTable);
						}
						if (ZombieManager.regions[(int)ZombieManager.respawnZombiesBound].hasBeacon)
						{
							BeaconManager.checkBeacon(ZombieManager.respawnZombiesBound).spawnRemaining();
						}
						byte shirt = byte.MaxValue;
						if (zombieTable.slots[0].table.Count > 0 && Random.value < zombieTable.slots[0].chance)
						{
							shirt = (byte)Random.Range(0, zombieTable.slots[0].table.Count);
						}
						byte pants = byte.MaxValue;
						if (zombieTable.slots[1].table.Count > 0 && Random.value < zombieTable.slots[1].chance)
						{
							pants = (byte)Random.Range(0, zombieTable.slots[1].table.Count);
						}
						byte hat = byte.MaxValue;
						if (zombieTable.slots[2].table.Count > 0 && Random.value < zombieTable.slots[2].chance)
						{
							hat = (byte)Random.Range(0, zombieTable.slots[2].table.Count);
						}
						byte gear = byte.MaxValue;
						if (zombieTable.slots[3].table.Count > 0 && Random.value < zombieTable.slots[3].chance)
						{
							gear = (byte)Random.Range(0, zombieTable.slots[3].table.Count);
						}
						Vector3 point = zombieSpawnpoint.point;
						point.y += 0.1f;
						zombie.sendRevive(b, (byte)ezombieSpeciality, shirt, pants, hat, gear, point, Random.Range(0f, 360f));
						if (Level.info.type == ELevelType.HORDE)
						{
							ZombieManager._waveRemaining--;
						}
					}
				}
			}
		}

		private void onBoundUpdated(Player player, byte oldBound, byte newBound)
		{
			if (player.channel.isOwner && LevelNavigation.checkSafe(oldBound) && ZombieManager.regions[(int)oldBound].isNetworked)
			{
				ZombieManager.regions[(int)oldBound].destroy();
				ZombieManager.regions[(int)oldBound].isNetworked = false;
			}
			if (Provider.isServer)
			{
				if (LevelNavigation.checkSafe(oldBound) && player.movement.loadedBounds[(int)oldBound].isZombiesLoaded)
				{
					player.movement.loadedBounds[(int)oldBound].isZombiesLoaded = false;
				}
				if (LevelNavigation.checkSafe(newBound) && !player.movement.loadedBounds[(int)newBound].isZombiesLoaded)
				{
					if (player.channel.isOwner)
					{
						this.generateZombies(newBound);
						ZombieManager.regions[(int)newBound].isNetworked = true;
					}
					else
					{
						this.askZombies(player.channel.owner.playerID.steamID, newBound);
					}
					player.movement.loadedBounds[(int)newBound].isZombiesLoaded = true;
				}
			}
		}

		private void onClientConnected()
		{
			if (Level.info.type == ELevelType.HORDE)
			{
				base.channel.send("askWave", ESteamCall.SERVER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[0]);
			}
		}

		private void onPlayerCreated(Player player)
		{
			PlayerMovement movement = player.movement;
			movement.onBoundUpdated = (PlayerBoundUpdated)Delegate.Combine(movement.onBoundUpdated, new PlayerBoundUpdated(this.onBoundUpdated));
		}

		private void onLevelLoaded(int level)
		{
			if (level > Level.MENU)
			{
				this.seq = 0u;
				if (LevelNavigation.bounds == null)
				{
					return;
				}
				ZombieManager._regions = new ZombieRegion[LevelNavigation.bounds.Count];
				byte b = 0;
				while ((int)b < ZombieManager.regions.Length)
				{
					ZombieManager.regions[(int)b] = new ZombieRegion(b);
					for (int i = 0; i < LevelNodes.nodes.Count; i++)
					{
						Node node = LevelNodes.nodes[i];
						if (node.type == ENodeType.DEADZONE)
						{
							DeadzoneNode deadzoneNode = (DeadzoneNode)node;
							Vector3 vector = LevelNavigation.bounds[(int)b].center - deadzoneNode.point;
							vector.y = 0f;
							if (vector.sqrMagnitude < deadzoneNode.radius)
							{
								ZombieManager.regions[(int)b].isRadioactive = true;
								break;
							}
						}
					}
					b += 1;
				}
				ZombieManager.wanderingCount = 0;
				ZombieManager.tickIndex = 0;
				ZombieManager._tickingZombies = new List<Zombie>();
				ZombieManager.respawnZombiesBound = 0;
				ZombieManager._waveReady = false;
				ZombieManager._waveIndex = 0;
				ZombieManager._waveRemaining = 0;
				ZombieManager.onWaveUpdated = null;
				if (Dedicator.isDedicated)
				{
					if (LevelNavigation.bounds.Count == 0 || LevelZombies.zombies.Length == 0 || LevelNavigation.bounds.Count != LevelZombies.zombies.Length)
					{
						return;
					}
					byte b2 = 0;
					while ((int)b2 < LevelNavigation.bounds.Count)
					{
						this.generateZombies(b2);
						b2 += 1;
					}
				}
			}
			if (level > Level.SETUP && !Dedicator.isDedicated)
			{
				ZombieClothing.build();
			}
		}

		private void onPostLevelLoaded(int level)
		{
			if (level > Level.MENU)
			{
				if (ZombieManager.regions == null)
				{
					return;
				}
				for (int i = 0; i < ZombieManager.regions.Length; i++)
				{
					ZombieManager.regions[i].init();
					if (Provider.isServer)
					{
						InteractableBeacon interactableBeacon = BeaconManager.checkBeacon((byte)i);
						if (interactableBeacon != null)
						{
							interactableBeacon.init(ZombieManager.regions[i].alive);
						}
						ZombieManager.regions[i].hasBeacon = (interactableBeacon != null);
					}
				}
			}
		}

		private void onBeaconUpdated(byte nav, bool hasBeacon)
		{
			if (!Provider.isServer)
			{
				return;
			}
			if (ZombieManager.regions == null || (int)nav >= ZombieManager.regions.Length)
			{
				return;
			}
			if (hasBeacon)
			{
				InteractableBeacon interactableBeacon = BeaconManager.checkBeacon(nav);
				interactableBeacon.init(ZombieManager.regions[(int)nav].alive);
			}
			ZombieManager.manager.channel.send("tellBeacon", ESteamCall.ALL, nav, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
			{
				nav,
				hasBeacon
			});
		}

		private void Update()
		{
			if (!Level.isLoaded)
			{
				return;
			}
			if (!Provider.isServer)
			{
				return;
			}
			if (LevelNavigation.bounds == null || LevelNavigation.bounds.Count == 0 || LevelZombies.zombies == null || LevelZombies.zombies.Length == 0 || LevelNavigation.bounds.Count != LevelZombies.zombies.Length)
			{
				return;
			}
			if (ZombieManager.regions == null || ZombieManager.tickingZombies == null)
			{
				return;
			}
			int num;
			int num2;
			if (Dedicator.isDedicated)
			{
				if (ZombieManager.tickIndex >= ZombieManager.tickingZombies.Count)
				{
					ZombieManager.tickIndex = 0;
				}
				num = ZombieManager.tickIndex;
				num2 = num + 50;
				if (num2 >= ZombieManager.tickingZombies.Count)
				{
					num2 = ZombieManager.tickingZombies.Count;
				}
				ZombieManager.tickIndex = num2;
			}
			else
			{
				num = 0;
				num2 = ZombieManager.tickingZombies.Count;
			}
			for (int i = num2 - 1; i >= num; i--)
			{
				Zombie zombie = ZombieManager.tickingZombies[i];
				if (zombie == null)
				{
					Debug.LogError("Missing zombie " + i);
				}
				else
				{
					zombie.tick();
				}
			}
			if (Time.realtimeSinceStartup - ZombieManager.lastTick > Provider.UPDATE_TIME)
			{
				ZombieManager.lastTick += Provider.UPDATE_TIME;
				if (Time.realtimeSinceStartup - ZombieManager.lastTick > Provider.UPDATE_TIME)
				{
					ZombieManager.lastTick = Time.realtimeSinceStartup;
				}
				byte b = 0;
				while ((int)b < ZombieManager.regions.Length)
				{
					ZombieManager.regions[(int)b].UpdateRegion();
					if (ZombieManager.regions[(int)b].updates > 0)
					{
						if (Dedicator.isDedicated)
						{
							base.channel.openWrite();
							base.channel.useCompression = true;
							base.channel.write(b);
							this.seq += 1u;
							base.channel.write(this.seq);
							base.channel.write(ZombieManager.regions[(int)b].updates);
							for (int j = 0; j < ZombieManager.regions[(int)b].zombies.Count; j++)
							{
								Zombie zombie2 = ZombieManager.regions[(int)b].zombies[j];
								if (zombie2.isUpdated)
								{
									zombie2.isUpdated = false;
									base.channel.write(zombie2.id, zombie2.transform.position, MeasurementTool.angleToByte(zombie2.transform.rotation.eulerAngles.y));
								}
							}
							ZombieManager.regions[(int)b].updates = 0;
							base.channel.useCompression = false;
							base.channel.closeWrite("tellZombieStates", ESteamCall.OTHERS, b, ESteamPacket.UPDATE_UNRELIABLE_CHUNK_BUFFER);
						}
						else
						{
							for (int k = 0; k < ZombieManager.regions[(int)b].zombies.Count; k++)
							{
								Zombie zombie3 = ZombieManager.regions[(int)b].zombies[k];
								if (zombie3.isUpdated)
								{
									zombie3.isUpdated = false;
								}
							}
							ZombieManager.regions[(int)b].updates = 0;
						}
					}
					b += 1;
				}
			}
			this.respawnZombies();
			ZombieManager.respawnZombiesBound += 1;
			if ((int)ZombieManager.respawnZombiesBound >= LevelZombies.zombies.Length)
			{
				ZombieManager.respawnZombiesBound = 0;
			}
		}

		private void Start()
		{
			ZombieManager.manager = this;
			Level.onLevelLoaded = (LevelLoaded)Delegate.Combine(Level.onLevelLoaded, new LevelLoaded(this.onLevelLoaded));
			Level.onPostLevelLoaded = (PostLevelLoaded)Delegate.Combine(Level.onPostLevelLoaded, new PostLevelLoaded(this.onPostLevelLoaded));
			Provider.onClientConnected = (Provider.ClientConnected)Delegate.Combine(Provider.onClientConnected, new Provider.ClientConnected(this.onClientConnected));
			Player.onPlayerCreated = (PlayerCreated)Delegate.Combine(Player.onPlayerCreated, new PlayerCreated(this.onPlayerCreated));
			BeaconManager.onBeaconUpdated = (BeaconUpdated)Delegate.Combine(BeaconManager.onBeaconUpdated, new BeaconUpdated(this.onBeaconUpdated));
			if (!Dedicator.isDedicated)
			{
				ZombieManager.roars = new AudioClip[16];
				for (int i = 0; i < ZombieManager.roars.Length; i++)
				{
					ZombieManager.roars[i] = (AudioClip)Resources.Load("Sounds/Zombies/Roars/Roar_" + i);
				}
				ZombieManager.groans = new AudioClip[5];
				for (int j = 0; j < ZombieManager.groans.Length; j++)
				{
					ZombieManager.groans[j] = (AudioClip)Resources.Load("Sounds/Zombies/Groans/Groan_" + j);
				}
				ZombieManager.spits = new AudioClip[4];
				for (int k = 0; k < ZombieManager.spits.Length; k++)
				{
					ZombieManager.spits[k] = (AudioClip)Resources.Load("Sounds/Zombies/Spits/Spit_" + k);
				}
			}
		}

		public static AudioClip[] roars;

		public static AudioClip[] groans;

		public static AudioClip[] spits;

		private static ZombieManager manager;

		private static ZombieRegion[] _regions;

		public static int wanderingCount;

		private static int tickIndex;

		private static List<Zombie> _tickingZombies;

		private static byte respawnZombiesBound;

		private static float lastWave;

		private static bool _waveReady;

		private static int _waveIndex;

		private static int _waveRemaining;

		private static float lastTick;

		public static WaveUpdated onWaveUpdated;

		private uint seq;
	}
}
