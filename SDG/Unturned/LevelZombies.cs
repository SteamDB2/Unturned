using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	public class LevelZombies
	{
		public static Transform models
		{
			get
			{
				return LevelZombies._models;
			}
		}

		public static List<ZombieSpawnpoint>[] zombies
		{
			get
			{
				return LevelZombies._zombies;
			}
		}

		public static List<ZombieSpawnpoint>[,] spawns
		{
			get
			{
				return LevelZombies._spawns;
			}
		}

		public static void setEnabled(bool isEnabled)
		{
			if (LevelZombies.spawns == null)
			{
				return;
			}
			for (byte b = 0; b < Regions.WORLD_SIZE; b += 1)
			{
				for (byte b2 = 0; b2 < Regions.WORLD_SIZE; b2 += 1)
				{
					for (int i = 0; i < LevelZombies.spawns[(int)b, (int)b2].Count; i++)
					{
						LevelZombies.spawns[(int)b, (int)b2][i].setEnabled(isEnabled);
					}
				}
			}
		}

		public static void addTable(string name)
		{
			if (LevelZombies.tables.Count == 255)
			{
				return;
			}
			LevelZombies.tables.Add(new ZombieTable(name));
		}

		public static void removeTable()
		{
			LevelZombies.tables.RemoveAt((int)EditorSpawns.selectedZombie);
			for (byte b = 0; b < Regions.WORLD_SIZE; b += 1)
			{
				for (byte b2 = 0; b2 < Regions.WORLD_SIZE; b2 += 1)
				{
					List<ZombieSpawnpoint> list = new List<ZombieSpawnpoint>();
					for (int i = 0; i < LevelZombies.spawns[(int)b, (int)b2].Count; i++)
					{
						ZombieSpawnpoint zombieSpawnpoint = LevelZombies.spawns[(int)b, (int)b2][i];
						if (zombieSpawnpoint.type == EditorSpawns.selectedZombie)
						{
							Object.Destroy(zombieSpawnpoint.node.gameObject);
						}
						else
						{
							if (zombieSpawnpoint.type > EditorSpawns.selectedZombie)
							{
								ZombieSpawnpoint zombieSpawnpoint2 = zombieSpawnpoint;
								zombieSpawnpoint2.type -= 1;
							}
							list.Add(zombieSpawnpoint);
						}
					}
					LevelZombies._spawns[(int)b, (int)b2] = list;
				}
			}
			EditorSpawns.selectedZombie = 0;
			if ((int)EditorSpawns.selectedZombie < LevelZombies.tables.Count)
			{
				EditorSpawns.zombieSpawn.GetComponent<Renderer>().material.color = LevelZombies.tables[(int)EditorSpawns.selectedZombie].color;
			}
		}

		public static void addSpawn(Vector3 point)
		{
			byte b;
			byte b2;
			if (!Regions.tryGetCoordinate(point, out b, out b2))
			{
				return;
			}
			if ((int)EditorSpawns.selectedZombie >= LevelZombies.tables.Count)
			{
				return;
			}
			LevelZombies.spawns[(int)b, (int)b2].Add(new ZombieSpawnpoint(EditorSpawns.selectedZombie, point));
		}

		public static void removeSpawn(Vector3 point, float radius)
		{
			radius *= radius;
			for (byte b = 0; b < Regions.WORLD_SIZE; b += 1)
			{
				for (byte b2 = 0; b2 < Regions.WORLD_SIZE; b2 += 1)
				{
					List<ZombieSpawnpoint> list = new List<ZombieSpawnpoint>();
					for (int i = 0; i < LevelZombies.spawns[(int)b, (int)b2].Count; i++)
					{
						ZombieSpawnpoint zombieSpawnpoint = LevelZombies.spawns[(int)b, (int)b2][i];
						if ((zombieSpawnpoint.point - point).sqrMagnitude < radius)
						{
							Object.Destroy(zombieSpawnpoint.node.gameObject);
						}
						else
						{
							list.Add(zombieSpawnpoint);
						}
					}
					LevelZombies._spawns[(int)b, (int)b2] = list;
				}
			}
		}

		public static void load()
		{
			LevelZombies._models = new GameObject().transform;
			LevelZombies.models.name = "Zombies";
			LevelZombies.models.parent = Level.spawns;
			LevelZombies.models.tag = "Logic";
			LevelZombies.models.gameObject.layer = LayerMasks.LOGIC;
			LevelZombies.tables = new List<ZombieTable>();
			if (ReadWrite.fileExists(Level.info.path + "/Spawns/Zombies.dat", false, false))
			{
				Block block = ReadWrite.readBlock(Level.info.path + "/Spawns/Zombies.dat", false, false, 0);
				byte b = block.readByte();
				if (b > 3 && b < 5)
				{
					block.readSteamID();
				}
				if (b > 2)
				{
					byte b2 = block.readByte();
					for (byte b3 = 0; b3 < b2; b3 += 1)
					{
						Color newColor = block.readColor();
						string newName = block.readString();
						bool flag = block.readBoolean();
						ushort newHealth = block.readUInt16();
						byte newDamage = block.readByte();
						byte newLootIndex = block.readByte();
						ushort newLootID;
						if (b > 6)
						{
							newLootID = block.readUInt16();
						}
						else
						{
							newLootID = 0;
						}
						uint newXP;
						if (b > 7)
						{
							newXP = block.readUInt32();
						}
						else if (flag)
						{
							newXP = 40u;
						}
						else
						{
							newXP = 3u;
						}
						float newRegen = 10f;
						if (b > 5)
						{
							newRegen = block.readSingle();
						}
						string newDifficultyGUID = string.Empty;
						if (b > 8)
						{
							newDifficultyGUID = block.readString();
						}
						ZombieSlot[] array = new ZombieSlot[4];
						byte b4 = block.readByte();
						for (byte b5 = 0; b5 < b4; b5 += 1)
						{
							List<ZombieCloth> list = new List<ZombieCloth>();
							float newChance = block.readSingle();
							byte b6 = block.readByte();
							for (byte b7 = 0; b7 < b6; b7 += 1)
							{
								ushort num = block.readUInt16();
								if ((ItemAsset)Assets.find(EAssetType.ITEM, num) != null)
								{
									list.Add(new ZombieCloth(num));
								}
							}
							array[(int)b5] = new ZombieSlot(newChance, list);
						}
						LevelZombies.tables.Add(new ZombieTable(array, newColor, newName, flag, newHealth, newDamage, newLootIndex, newLootID, newXP, newRegen, newDifficultyGUID));
					}
				}
				else
				{
					byte b8 = block.readByte();
					for (byte b9 = 0; b9 < b8; b9 += 1)
					{
						Color newColor2 = block.readColor();
						string newName2 = block.readString();
						byte newLootIndex2 = block.readByte();
						ZombieSlot[] array2 = new ZombieSlot[4];
						byte b10 = block.readByte();
						for (byte b11 = 0; b11 < b10; b11 += 1)
						{
							List<ZombieCloth> list2 = new List<ZombieCloth>();
							float newChance2 = block.readSingle();
							byte b12 = block.readByte();
							for (byte b13 = 0; b13 < b12; b13 += 1)
							{
								ushort num2 = block.readUInt16();
								if ((ItemAsset)Assets.find(EAssetType.ITEM, num2) != null)
								{
									list2.Add(new ZombieCloth(num2));
								}
							}
							array2[(int)b11] = new ZombieSlot(newChance2, list2);
						}
						LevelZombies.tables.Add(new ZombieTable(array2, newColor2, newName2, false, 100, 15, newLootIndex2, 0, 5u, 10f, string.Empty));
					}
				}
			}
			LevelZombies._spawns = new List<ZombieSpawnpoint>[(int)Regions.WORLD_SIZE, (int)Regions.WORLD_SIZE];
			for (byte b14 = 0; b14 < Regions.WORLD_SIZE; b14 += 1)
			{
				for (byte b15 = 0; b15 < Regions.WORLD_SIZE; b15 += 1)
				{
					LevelZombies.spawns[(int)b14, (int)b15] = new List<ZombieSpawnpoint>();
				}
			}
			if (Level.isEditor)
			{
				if (ReadWrite.fileExists(Level.info.path + "/Spawns/Animals.dat", false, false))
				{
					River river = new River(Level.info.path + "/Spawns/Animals.dat", false);
					byte b16 = river.readByte();
					if (b16 > 0)
					{
						for (byte b17 = 0; b17 < Regions.WORLD_SIZE; b17 += 1)
						{
							for (byte b18 = 0; b18 < Regions.WORLD_SIZE; b18 += 1)
							{
								ushort num3 = river.readUInt16();
								for (ushort num4 = 0; num4 < num3; num4 += 1)
								{
									byte newType = river.readByte();
									Vector3 newPoint = river.readSingleVector3();
									LevelZombies.spawns[(int)b17, (int)b18].Add(new ZombieSpawnpoint(newType, newPoint));
								}
							}
						}
					}
					river.closeRiver();
				}
				else
				{
					for (byte b19 = 0; b19 < Regions.WORLD_SIZE; b19 += 1)
					{
						for (byte b20 = 0; b20 < Regions.WORLD_SIZE; b20 += 1)
						{
							LevelZombies.spawns[(int)b19, (int)b20] = new List<ZombieSpawnpoint>();
							if (ReadWrite.fileExists(string.Concat(new object[]
							{
								Level.info.path,
								"/Spawns/Animals_",
								b19,
								"_",
								b20,
								".dat"
							}), false, false))
							{
								River river2 = new River(string.Concat(new object[]
								{
									Level.info.path,
									"/Spawns/Animals_",
									b19,
									"_",
									b20,
									".dat"
								}), false);
								byte b21 = river2.readByte();
								if (b21 > 0)
								{
									ushort num5 = river2.readUInt16();
									for (ushort num6 = 0; num6 < num5; num6 += 1)
									{
										byte newType2 = river2.readByte();
										Vector3 newPoint2 = river2.readSingleVector3();
										LevelZombies.spawns[(int)b19, (int)b20].Add(new ZombieSpawnpoint(newType2, newPoint2));
									}
									river2.closeRiver();
								}
							}
						}
					}
				}
			}
			else if (Provider.isServer)
			{
				LevelZombies._zombies = new List<ZombieSpawnpoint>[LevelNavigation.bounds.Count];
				for (int i = 0; i < LevelZombies.zombies.Length; i++)
				{
					LevelZombies.zombies[i] = new List<ZombieSpawnpoint>();
				}
				if (ReadWrite.fileExists(Level.info.path + "/Spawns/Animals.dat", false, false))
				{
					River river3 = new River(Level.info.path + "/Spawns/Animals.dat", false);
					byte b22 = river3.readByte();
					if (b22 > 0)
					{
						for (byte b23 = 0; b23 < Regions.WORLD_SIZE; b23 += 1)
						{
							for (byte b24 = 0; b24 < Regions.WORLD_SIZE; b24 += 1)
							{
								ushort num7 = river3.readUInt16();
								for (ushort num8 = 0; num8 < num7; num8 += 1)
								{
									byte newType3 = river3.readByte();
									Vector3 vector = river3.readSingleVector3();
									byte b25;
									if (LevelNavigation.tryGetBounds(vector, out b25) && LevelNavigation.checkNavigation(vector))
									{
										LevelZombies.zombies[(int)b25].Add(new ZombieSpawnpoint(newType3, vector));
									}
								}
							}
						}
					}
					river3.closeRiver();
				}
				else
				{
					for (byte b26 = 0; b26 < Regions.WORLD_SIZE; b26 += 1)
					{
						for (byte b27 = 0; b27 < Regions.WORLD_SIZE; b27 += 1)
						{
							if (ReadWrite.fileExists(string.Concat(new object[]
							{
								Level.info.path,
								"/Spawns/Animals_",
								b26,
								"_",
								b27,
								".dat"
							}), false, false))
							{
								River river4 = new River(string.Concat(new object[]
								{
									Level.info.path,
									"/Spawns/Animals_",
									b26,
									"_",
									b27,
									".dat"
								}), false);
								byte b28 = river4.readByte();
								if (b28 > 0)
								{
									ushort num9 = river4.readUInt16();
									for (ushort num10 = 0; num10 < num9; num10 += 1)
									{
										byte newType4 = river4.readByte();
										Vector3 vector2 = river4.readSingleVector3();
										byte b29;
										if (LevelNavigation.tryGetBounds(vector2, out b29) && LevelNavigation.checkNavigation(vector2))
										{
											LevelZombies.zombies[(int)b29].Add(new ZombieSpawnpoint(newType4, vector2));
										}
									}
									river4.closeRiver();
								}
							}
						}
					}
				}
			}
		}

		public static void save()
		{
			Block block = new Block();
			block.writeByte(LevelZombies.SAVEDATA_TABLE_VERSION);
			block.writeByte((byte)LevelZombies.tables.Count);
			byte b = 0;
			while ((int)b < LevelZombies.tables.Count)
			{
				ZombieTable zombieTable = LevelZombies.tables[(int)b];
				block.writeColor(zombieTable.color);
				block.writeString(zombieTable.name);
				block.writeBoolean(zombieTable.isMega);
				block.writeUInt16(zombieTable.health);
				block.writeByte(zombieTable.damage);
				block.writeByte(zombieTable.lootIndex);
				block.writeUInt16(zombieTable.lootID);
				block.writeUInt32(zombieTable.xp);
				block.writeSingle(zombieTable.regen);
				block.writeString(zombieTable.difficultyGUID);
				block.write((byte)zombieTable.slots.Length);
				byte b2 = 0;
				while ((int)b2 < zombieTable.slots.Length)
				{
					ZombieSlot zombieSlot = zombieTable.slots[(int)b2];
					block.writeSingle(zombieSlot.chance);
					block.writeByte((byte)zombieSlot.table.Count);
					byte b3 = 0;
					while ((int)b3 < zombieSlot.table.Count)
					{
						ZombieCloth zombieCloth = zombieSlot.table[(int)b3];
						block.writeUInt16(zombieCloth.item);
						b3 += 1;
					}
					b2 += 1;
				}
				b += 1;
			}
			ReadWrite.writeBlock(Level.info.path + "/Spawns/Zombies.dat", false, false, block);
			River river = new River(Level.info.path + "/Spawns/Animals.dat", false);
			river.writeByte(LevelZombies.SAVEDATA_SPAWN_VERSION);
			for (byte b4 = 0; b4 < Regions.WORLD_SIZE; b4 += 1)
			{
				for (byte b5 = 0; b5 < Regions.WORLD_SIZE; b5 += 1)
				{
					List<ZombieSpawnpoint> list = LevelZombies.spawns[(int)b4, (int)b5];
					river.writeUInt16((ushort)list.Count);
					ushort num = 0;
					while ((int)num < list.Count)
					{
						ZombieSpawnpoint zombieSpawnpoint = list[(int)num];
						river.writeByte(zombieSpawnpoint.type);
						river.writeSingleVector3(zombieSpawnpoint.point);
						num += 1;
					}
				}
			}
			river.closeRiver();
		}

		public static readonly byte SAVEDATA_TABLE_VERSION = 9;

		public static readonly byte SAVEDATA_SPAWN_VERSION = 1;

		private static Transform _models;

		public static List<ZombieTable> tables;

		private static List<ZombieSpawnpoint>[] _zombies;

		private static List<ZombieSpawnpoint>[,] _spawns;
	}
}
