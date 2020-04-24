using System;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	public class AnimalManager : SteamCaller
	{
		public static List<Animal> animals
		{
			get
			{
				return AnimalManager._animals;
			}
		}

		public static List<PackInfo> packs
		{
			get
			{
				return AnimalManager._packs;
			}
		}

		public static List<Animal> tickingAnimals
		{
			get
			{
				return AnimalManager._tickingAnimals;
			}
		}

		public static void getAnimalsInRadius(Vector3 center, float sqrRadius, List<Animal> result)
		{
			if (AnimalManager.animals == null)
			{
				return;
			}
			for (int i = 0; i < AnimalManager.animals.Count; i++)
			{
				Animal animal = AnimalManager.animals[i];
				if ((animal.transform.position - center).sqrMagnitude < sqrRadius)
				{
					result.Add(animal);
				}
			}
		}

		[SteamCall]
		public void tellAnimalAlive(CSteamID steamID, ushort index, Vector3 newPosition, byte newAngle)
		{
			if (base.channel.checkServer(steamID))
			{
				if ((int)index >= AnimalManager.animals.Count)
				{
					return;
				}
				AnimalManager.animals[(int)index].tellAlive(newPosition, newAngle);
			}
		}

		[SteamCall]
		public void tellAnimalDead(CSteamID steamID, ushort index, Vector3 newRagdoll)
		{
			if (base.channel.checkServer(steamID))
			{
				if ((int)index >= AnimalManager.animals.Count)
				{
					return;
				}
				AnimalManager.animals[(int)index].tellDead(newRagdoll);
			}
		}

		[SteamCall]
		public void tellAnimalStates(CSteamID steamID)
		{
			if (base.channel.checkServer(steamID))
			{
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
					if ((int)((ushort)array[0]) >= AnimalManager.animals.Count)
					{
						break;
					}
					AnimalManager.animals[(int)((ushort)array[0])].tellState((Vector3)array[1], (byte)array[2]);
				}
				base.channel.useCompression = false;
			}
		}

		[SteamCall]
		public void askAnimalStartle(CSteamID steamID, ushort index)
		{
			if (base.channel.checkServer(steamID))
			{
				if ((int)index >= AnimalManager.animals.Count)
				{
					return;
				}
				AnimalManager.animals[(int)index].askStartle();
			}
		}

		[SteamCall]
		public void askAnimalAttack(CSteamID steamID, ushort index)
		{
			if (base.channel.checkServer(steamID))
			{
				if ((int)index >= AnimalManager.animals.Count)
				{
					return;
				}
				AnimalManager.animals[(int)index].askAttack();
			}
		}

		[SteamCall]
		public void askAnimalPanic(CSteamID steamID, ushort index)
		{
			if (base.channel.checkServer(steamID))
			{
				if ((int)index >= AnimalManager.animals.Count)
				{
					return;
				}
				AnimalManager.animals[(int)index].askPanic();
			}
		}

		[SteamCall]
		public void tellAnimals(CSteamID steamID)
		{
			if (base.channel.checkServer(steamID))
			{
				ushort num = (ushort)base.channel.read(Types.UINT16_TYPE);
				for (int i = 0; i < (int)num; i++)
				{
					object[] array = base.channel.read(Types.UINT16_TYPE, Types.VECTOR3_TYPE, Types.BYTE_TYPE, Types.BOOLEAN_TYPE);
					this.addAnimal((ushort)array[0], (Vector3)array[1], (float)((byte)array[2] * 2), (bool)array[3]);
				}
			}
		}

		[SteamCall]
		public void askAnimals(CSteamID steamID)
		{
			base.channel.openWrite();
			base.channel.write((ushort)AnimalManager.animals.Count);
			for (int i = 0; i < AnimalManager.animals.Count; i++)
			{
				Animal animal = AnimalManager.animals[i];
				base.channel.write(animal.id, animal.transform.position, MeasurementTool.angleToByte(animal.transform.rotation.eulerAngles.y), animal.isDead);
			}
			base.channel.closeWrite("tellAnimals", steamID, ESteamPacket.UPDATE_RELIABLE_CHUNK_BUFFER);
		}

		public static void sendAnimalAlive(Animal animal, Vector3 newPosition, byte newAngle)
		{
			AnimalManager.manager.channel.send("tellAnimalAlive", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
			{
				animal.index,
				newPosition,
				newAngle
			});
		}

		public static void sendAnimalDead(Animal animal, Vector3 newRagdoll)
		{
			AnimalManager.manager.channel.send("tellAnimalDead", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
			{
				animal.index,
				newRagdoll
			});
		}

		public static void sendAnimalStartle(Animal animal)
		{
			AnimalManager.manager.channel.send("askAnimalStartle", ESteamCall.ALL, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[]
			{
				animal.index
			});
		}

		public static void sendAnimalAttack(Animal animal)
		{
			AnimalManager.manager.channel.send("askAnimalAttack", ESteamCall.ALL, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[]
			{
				animal.index
			});
		}

		public static void sendAnimalPanic(Animal animal)
		{
			AnimalManager.manager.channel.send("askAnimalPanic", ESteamCall.ALL, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[]
			{
				animal.index
			});
		}

		public static void dropLoot(Animal animal)
		{
			if (animal.asset.rewardID != 0)
			{
				int num = Random.Range((int)animal.asset.rewardMin, (int)(animal.asset.rewardMax + 1));
				for (int i = 0; i < num; i++)
				{
					ushort num2 = SpawnTableTool.resolve(animal.asset.rewardID);
					if (num2 != 0)
					{
						ItemManager.dropItem(new Item(num2, EItemOrigin.NATURE), animal.transform.position, false, Dedicator.isDedicated, true);
					}
				}
			}
			else
			{
				if (animal.asset.meat != 0)
				{
					int num3 = Random.Range(2, 5);
					for (int j = 0; j < num3; j++)
					{
						ItemManager.dropItem(new Item(animal.asset.meat, EItemOrigin.NATURE), animal.transform.position, false, Dedicator.isDedicated, true);
					}
				}
				if (animal.asset.pelt != 0)
				{
					int num4 = Random.Range(2, 5);
					for (int k = 0; k < num4; k++)
					{
						ItemManager.dropItem(new Item(animal.asset.pelt, EItemOrigin.NATURE), animal.transform.position, false, Dedicator.isDedicated, true);
					}
				}
			}
		}

		private Animal addAnimal(ushort id, Vector3 point, float angle, bool isDead)
		{
			AnimalAsset animalAsset = (AnimalAsset)Assets.find(EAssetType.ANIMAL, id);
			if (animalAsset != null)
			{
				Transform transform;
				if (Dedicator.isDedicated)
				{
					transform = Object.Instantiate<GameObject>(animalAsset.dedicated).transform;
				}
				else if (Provider.isServer)
				{
					transform = Object.Instantiate<GameObject>(animalAsset.server).transform;
				}
				else
				{
					transform = Object.Instantiate<GameObject>(animalAsset.client).transform;
				}
				transform.name = id.ToString();
				transform.parent = LevelAnimals.models;
				transform.position = point;
				transform.rotation = Quaternion.Euler(0f, angle, 0f);
				Animal animal = transform.gameObject.AddComponent<Animal>();
				animal.index = (ushort)AnimalManager.animals.Count;
				animal.id = id;
				animal.isDead = isDead;
				AnimalManager.animals.Add(animal);
				return animal;
			}
			return null;
		}

		public static Animal getAnimal(ushort index)
		{
			if ((int)index >= AnimalManager.animals.Count)
			{
				return null;
			}
			return AnimalManager.animals[(int)index];
		}

		private void respawnAnimals()
		{
			if ((int)AnimalManager.respawnPackIndex >= AnimalManager.packs.Count)
			{
				AnimalManager.respawnPackIndex = (ushort)(AnimalManager.packs.Count - 1);
			}
			PackInfo packInfo = AnimalManager.packs[(int)AnimalManager.respawnPackIndex];
			AnimalManager.respawnPackIndex += 1;
			if ((int)AnimalManager.respawnPackIndex >= AnimalManager.packs.Count)
			{
				AnimalManager.respawnPackIndex = 0;
			}
			if (packInfo == null)
			{
				return;
			}
			for (int i = 0; i < packInfo.animals.Count; i++)
			{
				Animal animal = packInfo.animals[i];
				if (animal == null || !animal.isDead || Time.realtimeSinceStartup - animal.lastDead < Provider.modeConfigData.Animals.Respawn_Time)
				{
					return;
				}
			}
			List<AnimalSpawnpoint> list = new List<AnimalSpawnpoint>();
			for (int j = 0; j < packInfo.spawns.Count; j++)
			{
				list.Add(packInfo.spawns[j]);
			}
			for (int k = 0; k < packInfo.animals.Count; k++)
			{
				Animal animal2 = packInfo.animals[k];
				if (!(animal2 == null))
				{
					int index = Random.Range(0, list.Count);
					AnimalSpawnpoint animalSpawnpoint = list[index];
					list.RemoveAt(index);
					Vector3 point = animalSpawnpoint.point;
					point.y += 0.1f;
					animal2.sendRevive(point, Random.Range(0f, 360f));
				}
			}
		}

		private void onLevelLoaded(int level)
		{
			if (level > Level.SETUP)
			{
				this.seq = 0u;
				AnimalManager._animals = new List<Animal>();
				AnimalManager._packs = null;
				AnimalManager.updates = 0;
				AnimalManager.tickIndex = 0;
				AnimalManager._tickingAnimals = new List<Animal>();
				if (Provider.isServer && LevelAnimals.spawns.Count > 0)
				{
					AnimalManager._packs = new List<PackInfo>();
					for (int i = 0; i < LevelAnimals.spawns.Count; i++)
					{
						AnimalSpawnpoint animalSpawnpoint = LevelAnimals.spawns[i];
						int num = -1;
						for (int j = AnimalManager.packs.Count - 1; j >= 0; j--)
						{
							List<AnimalSpawnpoint> spawns = AnimalManager.packs[j].spawns;
							for (int k = 0; k < spawns.Count; k++)
							{
								AnimalSpawnpoint animalSpawnpoint2 = spawns[k];
								if ((animalSpawnpoint2.point - animalSpawnpoint.point).sqrMagnitude < 256f)
								{
									if (num == -1)
									{
										spawns.Add(animalSpawnpoint);
									}
									else
									{
										List<AnimalSpawnpoint> spawns2 = AnimalManager.packs[num].spawns;
										for (int l = 0; l < spawns2.Count; l++)
										{
											spawns.Add(spawns2[l]);
										}
										AnimalManager.packs.RemoveAtFast(num);
									}
									num = j;
									break;
								}
							}
						}
						if (num == -1)
						{
							PackInfo packInfo = new PackInfo();
							packInfo.spawns.Add(animalSpawnpoint);
							AnimalManager.packs.Add(packInfo);
						}
					}
					List<AnimalManager.ValidAnimalSpawnsInfo> list = new List<AnimalManager.ValidAnimalSpawnsInfo>();
					for (int m = 0; m < AnimalManager.packs.Count; m++)
					{
						PackInfo packInfo2 = AnimalManager.packs[m];
						List<AnimalSpawnpoint> list2 = new List<AnimalSpawnpoint>();
						for (int n = 0; n < packInfo2.spawns.Count; n++)
						{
							list2.Add(packInfo2.spawns[n]);
						}
						list.Add(new AnimalManager.ValidAnimalSpawnsInfo
						{
							spawns = list2,
							pack = packInfo2
						});
					}
					while (AnimalManager.animals.Count < (int)Level.animals && list.Count > 0)
					{
						int index = Random.Range(0, list.Count);
						AnimalManager.ValidAnimalSpawnsInfo validAnimalSpawnsInfo = list[index];
						int index2 = Random.Range(0, validAnimalSpawnsInfo.spawns.Count);
						AnimalSpawnpoint animalSpawnpoint3 = validAnimalSpawnsInfo.spawns[index2];
						validAnimalSpawnsInfo.spawns.RemoveAt(index2);
						if (validAnimalSpawnsInfo.spawns.Count == 0)
						{
							list.RemoveAt(index);
						}
						Vector3 point = animalSpawnpoint3.point;
						point.y += 0.1f;
						ushort id;
						if (validAnimalSpawnsInfo.pack.animals.Count > 0)
						{
							id = validAnimalSpawnsInfo.pack.animals[0].id;
						}
						else
						{
							id = LevelAnimals.getAnimal(animalSpawnpoint3);
						}
						Animal animal = this.addAnimal(id, point, Random.Range(0f, 360f), false);
						if (animal != null)
						{
							animal.pack = validAnimalSpawnsInfo.pack;
							validAnimalSpawnsInfo.pack.animals.Add(animal);
						}
					}
					for (int num2 = AnimalManager.packs.Count - 1; num2 >= 0; num2--)
					{
						PackInfo packInfo3 = AnimalManager.packs[num2];
						if (packInfo3.animals.Count <= 0)
						{
							AnimalManager.packs.RemoveAt(num2);
						}
					}
				}
			}
		}

		private void onClientConnected()
		{
			base.channel.send("askAnimals", ESteamCall.SERVER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[0]);
		}

		private void OnDrawGizmos()
		{
			if (AnimalManager.packs == null)
			{
				return;
			}
			for (int i = 0; i < AnimalManager.packs.Count; i++)
			{
				PackInfo packInfo = AnimalManager.packs[i];
				if (packInfo != null && packInfo.spawns != null && packInfo.animals != null)
				{
					Vector3 averageSpawnPoint = packInfo.getAverageSpawnPoint();
					Vector3 averageAnimalPoint = packInfo.getAverageAnimalPoint();
					Vector3 wanderDirection = packInfo.getWanderDirection();
					Gizmos.color = Color.gray;
					for (int j = 0; j < packInfo.spawns.Count; j++)
					{
						AnimalSpawnpoint animalSpawnpoint = packInfo.spawns[j];
						if (animalSpawnpoint != null)
						{
							Gizmos.DrawLine(averageSpawnPoint, animalSpawnpoint.point);
						}
					}
					Gizmos.color = Color.yellow;
					Gizmos.DrawLine(averageSpawnPoint, averageAnimalPoint);
					for (int k = 0; k < packInfo.animals.Count; k++)
					{
						Animal animal = packInfo.animals[k];
						if (!(animal == null))
						{
							Gizmos.color = ((!animal.isDead) ? Color.green : Color.red);
							Gizmos.DrawLine(averageAnimalPoint, animal.transform.position);
							if (!animal.isDead)
							{
								Gizmos.color = Color.magenta;
								Gizmos.DrawLine(animal.transform.position, animal.target);
							}
						}
					}
					Gizmos.color = Color.cyan;
					Gizmos.DrawLine(averageAnimalPoint, averageAnimalPoint + wanderDirection * 4f);
				}
			}
		}

		private void Update()
		{
			if (!Provider.isServer || !Level.isLoaded)
			{
				return;
			}
			if (LevelAnimals.spawns == null || LevelAnimals.spawns.Count == 0 || AnimalManager.animals == null || AnimalManager.animals.Count == 0)
			{
				return;
			}
			if (AnimalManager.tickingAnimals == null)
			{
				return;
			}
			int num;
			int num2;
			if (Dedicator.isDedicated)
			{
				if (AnimalManager.tickIndex >= AnimalManager.tickingAnimals.Count)
				{
					AnimalManager.tickIndex = 0;
				}
				num = AnimalManager.tickIndex;
				num2 = num + 25;
				if (num2 >= AnimalManager.tickingAnimals.Count)
				{
					num2 = AnimalManager.tickingAnimals.Count;
				}
				AnimalManager.tickIndex = num2;
			}
			else
			{
				num = 0;
				num2 = AnimalManager.tickingAnimals.Count;
			}
			for (int i = num2 - 1; i >= num; i--)
			{
				Animal animal = AnimalManager.tickingAnimals[i];
				if (animal == null)
				{
					Debug.LogError("Missing animal " + i);
				}
				else
				{
					animal.tick();
				}
			}
			if (Dedicator.isDedicated && Time.realtimeSinceStartup - AnimalManager.lastTick > Provider.UPDATE_TIME)
			{
				AnimalManager.lastTick += Provider.UPDATE_TIME;
				if (Time.realtimeSinceStartup - AnimalManager.lastTick > Provider.UPDATE_TIME)
				{
					AnimalManager.lastTick = Time.realtimeSinceStartup;
				}
				base.channel.useCompression = true;
				this.seq += 1u;
				for (int j = 0; j < Provider.clients.Count; j++)
				{
					SteamPlayer steamPlayer = Provider.clients[j];
					if (steamPlayer != null && !(steamPlayer.player == null))
					{
						base.channel.openWrite();
						base.channel.write(this.seq);
						ushort num3 = 0;
						int step = base.channel.step;
						base.channel.write(num3);
						int num4 = 0;
						for (int k = 0; k < AnimalManager.animals.Count; k++)
						{
							if (num3 >= 64)
							{
								break;
							}
							Animal animal2 = AnimalManager.animals[k];
							if (!(animal2 == null) && animal2.isUpdated)
							{
								if ((animal2.transform.position - steamPlayer.player.transform.position).sqrMagnitude > 331776f)
								{
									if ((ulong)(this.seq % 8u) == (ulong)((long)num4))
									{
										base.channel.write(animal2.index, animal2.transform.position, MeasurementTool.angleToByte(animal2.transform.rotation.eulerAngles.y));
										num3 += 1;
									}
									num4++;
								}
								else
								{
									base.channel.write(animal2.index, animal2.transform.position, MeasurementTool.angleToByte(animal2.transform.rotation.eulerAngles.y));
									num3 += 1;
								}
							}
						}
						if (num3 != 0)
						{
							int step2 = base.channel.step;
							base.channel.step = step;
							base.channel.write(num3);
							base.channel.step = step2;
							base.channel.closeWrite("tellAnimalStates", steamPlayer.playerID.steamID, ESteamPacket.UPDATE_UNRELIABLE_CHUNK_BUFFER);
						}
					}
				}
				base.channel.useCompression = false;
				for (int l = 0; l < AnimalManager.animals.Count; l++)
				{
					Animal animal3 = AnimalManager.animals[l];
					if (!(animal3 == null))
					{
						animal3.isUpdated = false;
					}
				}
			}
			this.respawnAnimals();
		}

		private void Start()
		{
			AnimalManager.manager = this;
			Level.onLevelLoaded = (LevelLoaded)Delegate.Combine(Level.onLevelLoaded, new LevelLoaded(this.onLevelLoaded));
			Provider.onClientConnected = (Provider.ClientConnected)Delegate.Combine(Provider.onClientConnected, new Provider.ClientConnected(this.onClientConnected));
		}

		private static AnimalManager manager;

		private static List<Animal> _animals;

		private static List<PackInfo> _packs;

		private static int tickIndex;

		private static List<Animal> _tickingAnimals;

		public static ushort updates;

		private static ushort respawnPackIndex;

		private static float lastTick;

		private uint seq;

		private class ValidAnimalSpawnsInfo
		{
			public List<AnimalSpawnpoint> spawns;

			public PackInfo pack;
		}
	}
}
