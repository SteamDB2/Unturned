using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	public class LevelItems
	{
		public static Transform models
		{
			get
			{
				return LevelItems._models;
			}
		}

		public static List<ItemTable> tables
		{
			get
			{
				return LevelItems._tables;
			}
		}

		public static List<ItemSpawnpoint>[,] spawns
		{
			get
			{
				return LevelItems._spawns;
			}
		}

		public static void setEnabled(bool isEnabled)
		{
			if (LevelItems.spawns == null)
			{
				return;
			}
			for (byte b = 0; b < Regions.WORLD_SIZE; b += 1)
			{
				for (byte b2 = 0; b2 < Regions.WORLD_SIZE; b2 += 1)
				{
					for (int i = 0; i < LevelItems.spawns[(int)b, (int)b2].Count; i++)
					{
						LevelItems.spawns[(int)b, (int)b2][i].setEnabled(isEnabled);
					}
				}
			}
		}

		public static void addTable(string name)
		{
			if (LevelItems.tables.Count == 255)
			{
				return;
			}
			LevelItems.tables.Add(new ItemTable(name));
		}

		public static void removeTable()
		{
			LevelItems.tables.RemoveAt((int)EditorSpawns.selectedItem);
			for (byte b = 0; b < Regions.WORLD_SIZE; b += 1)
			{
				for (byte b2 = 0; b2 < Regions.WORLD_SIZE; b2 += 1)
				{
					List<ItemSpawnpoint> list = new List<ItemSpawnpoint>();
					for (int i = 0; i < LevelItems.spawns[(int)b, (int)b2].Count; i++)
					{
						ItemSpawnpoint itemSpawnpoint = LevelItems.spawns[(int)b, (int)b2][i];
						if (itemSpawnpoint.type == EditorSpawns.selectedItem)
						{
							Object.Destroy(itemSpawnpoint.node.gameObject);
						}
						else
						{
							if (itemSpawnpoint.type > EditorSpawns.selectedItem)
							{
								ItemSpawnpoint itemSpawnpoint2 = itemSpawnpoint;
								itemSpawnpoint2.type -= 1;
							}
							list.Add(itemSpawnpoint);
						}
					}
					LevelItems._spawns[(int)b, (int)b2] = list;
				}
			}
			for (int j = 0; j < LevelZombies.tables.Count; j++)
			{
				ZombieTable zombieTable = LevelZombies.tables[j];
				if (zombieTable.lootIndex > EditorSpawns.selectedItem)
				{
					ZombieTable zombieTable2 = zombieTable;
					zombieTable2.lootIndex -= 1;
				}
			}
			EditorSpawns.selectedItem = 0;
			if ((int)EditorSpawns.selectedItem < LevelItems.tables.Count)
			{
				EditorSpawns.itemSpawn.GetComponent<Renderer>().material.color = LevelItems.tables[(int)EditorSpawns.selectedItem].color;
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
			if ((int)EditorSpawns.selectedItem >= LevelItems.tables.Count)
			{
				return;
			}
			LevelItems.spawns[(int)b, (int)b2].Add(new ItemSpawnpoint(EditorSpawns.selectedItem, point));
		}

		public static void removeSpawn(Vector3 point, float radius)
		{
			radius *= radius;
			for (byte b = 0; b < Regions.WORLD_SIZE; b += 1)
			{
				for (byte b2 = 0; b2 < Regions.WORLD_SIZE; b2 += 1)
				{
					List<ItemSpawnpoint> list = new List<ItemSpawnpoint>();
					for (int i = 0; i < LevelItems.spawns[(int)b, (int)b2].Count; i++)
					{
						ItemSpawnpoint itemSpawnpoint = LevelItems.spawns[(int)b, (int)b2][i];
						if ((itemSpawnpoint.point - point).sqrMagnitude < radius)
						{
							Object.Destroy(itemSpawnpoint.node.gameObject);
						}
						else
						{
							list.Add(itemSpawnpoint);
						}
					}
					LevelItems._spawns[(int)b, (int)b2] = list;
				}
			}
		}

		public static ushort getItem(ItemSpawnpoint spawn)
		{
			return LevelItems.getItem(spawn.type);
		}

		public static ushort getItem(byte type)
		{
			return LevelItems.tables[(int)type].getItem();
		}

		public static void load()
		{
			LevelItems._models = new GameObject().transform;
			LevelItems.models.name = "Items";
			LevelItems.models.parent = Level.spawns;
			LevelItems.models.tag = "Logic";
			LevelItems.models.gameObject.layer = LayerMasks.LOGIC;
			if (Level.isEditor || Provider.isServer)
			{
				LevelItems._tables = new List<ItemTable>();
				LevelItems._spawns = new List<ItemSpawnpoint>[(int)Regions.WORLD_SIZE, (int)Regions.WORLD_SIZE];
				if (ReadWrite.fileExists(Level.info.path + "/Spawns/Items.dat", false, false))
				{
					Block block = ReadWrite.readBlock(Level.info.path + "/Spawns/Items.dat", false, false, 0);
					byte b = block.readByte();
					if (b > 1 && b < 3)
					{
						block.readSteamID();
					}
					byte b2 = block.readByte();
					for (byte b3 = 0; b3 < b2; b3 += 1)
					{
						Color newColor = block.readColor();
						string text = block.readString();
						ushort num;
						if (b > 3)
						{
							num = block.readUInt16();
							if (num != 0 && SpawnTableTool.resolve(num) == 0)
							{
								Assets.errors.Add(string.Concat(new object[]
								{
									Level.info.name,
									" item table \"",
									text,
									"\" references invalid spawn table ",
									num,
									"!"
								}));
							}
						}
						else
						{
							num = 0;
						}
						List<ItemTier> list = new List<ItemTier>();
						byte b4 = block.readByte();
						for (byte b5 = 0; b5 < b4; b5 += 1)
						{
							string newName = block.readString();
							float newChance = block.readSingle();
							List<ItemSpawn> list2 = new List<ItemSpawn>();
							byte b6 = block.readByte();
							for (byte b7 = 0; b7 < b6; b7 += 1)
							{
								ushort num2 = block.readUInt16();
								ItemAsset itemAsset = (ItemAsset)Assets.find(EAssetType.ITEM, num2);
								if (itemAsset != null && !itemAsset.isPro)
								{
									list2.Add(new ItemSpawn(num2));
								}
							}
							if (list2.Count > 0)
							{
								list.Add(new ItemTier(list2, newName, newChance));
							}
						}
						LevelItems.tables.Add(new ItemTable(list, newColor, text, num));
						if (!Level.isEditor)
						{
							LevelItems.tables[(int)b3].buildTable();
						}
					}
				}
				for (byte b8 = 0; b8 < Regions.WORLD_SIZE; b8 += 1)
				{
					for (byte b9 = 0; b9 < Regions.WORLD_SIZE; b9 += 1)
					{
						LevelItems.spawns[(int)b8, (int)b9] = new List<ItemSpawnpoint>();
					}
				}
				if (ReadWrite.fileExists(Level.info.path + "/Spawns/Jars.dat", false, false))
				{
					River river = new River(Level.info.path + "/Spawns/Jars.dat", false);
					byte b10 = river.readByte();
					if (b10 > 0)
					{
						for (byte b11 = 0; b11 < Regions.WORLD_SIZE; b11 += 1)
						{
							for (byte b12 = 0; b12 < Regions.WORLD_SIZE; b12 += 1)
							{
								ushort num3 = river.readUInt16();
								for (ushort num4 = 0; num4 < num3; num4 += 1)
								{
									byte newType = river.readByte();
									Vector3 newPoint = river.readSingleVector3();
									LevelItems.spawns[(int)b11, (int)b12].Add(new ItemSpawnpoint(newType, newPoint));
								}
							}
						}
					}
					river.closeRiver();
				}
				else
				{
					for (byte b13 = 0; b13 < Regions.WORLD_SIZE; b13 += 1)
					{
						for (byte b14 = 0; b14 < Regions.WORLD_SIZE; b14 += 1)
						{
							LevelItems.spawns[(int)b13, (int)b14] = new List<ItemSpawnpoint>();
							if (ReadWrite.fileExists(string.Concat(new object[]
							{
								Level.info.path,
								"/Spawns/Items_",
								b13,
								"_",
								b14,
								".dat"
							}), false, false))
							{
								River river2 = new River(string.Concat(new object[]
								{
									Level.info.path,
									"/Spawns/Items_",
									b13,
									"_",
									b14,
									".dat"
								}), false);
								byte b15 = river2.readByte();
								if (b15 > 0)
								{
									ushort num5 = river2.readUInt16();
									for (ushort num6 = 0; num6 < num5; num6 += 1)
									{
										byte newType2 = river2.readByte();
										Vector3 newPoint2 = river2.readSingleVector3();
										LevelItems.spawns[(int)b13, (int)b14].Add(new ItemSpawnpoint(newType2, newPoint2));
									}
								}
								river2.closeRiver();
							}
						}
					}
				}
			}
		}

		public static void save()
		{
			Block block = new Block();
			block.writeByte(LevelItems.SAVEDATA_VERSION);
			block.writeByte((byte)LevelItems.tables.Count);
			byte b = 0;
			while ((int)b < LevelItems.tables.Count)
			{
				ItemTable itemTable = LevelItems.tables[(int)b];
				block.writeColor(itemTable.color);
				block.writeString(itemTable.name);
				block.writeUInt16(itemTable.tableID);
				block.write((byte)itemTable.tiers.Count);
				byte b2 = 0;
				while ((int)b2 < itemTable.tiers.Count)
				{
					ItemTier itemTier = itemTable.tiers[(int)b2];
					block.writeString(itemTier.name);
					block.writeSingle(itemTier.chance);
					block.writeByte((byte)itemTier.table.Count);
					byte b3 = 0;
					while ((int)b3 < itemTier.table.Count)
					{
						ItemSpawn itemSpawn = itemTier.table[(int)b3];
						block.writeUInt16(itemSpawn.item);
						b3 += 1;
					}
					b2 += 1;
				}
				b += 1;
			}
			ReadWrite.writeBlock(Level.info.path + "/Spawns/Items.dat", false, false, block);
			River river = new River(Level.info.path + "/Spawns/Jars.dat", false);
			river.writeByte(LevelItems.SAVEDATA_VERSION);
			for (byte b4 = 0; b4 < Regions.WORLD_SIZE; b4 += 1)
			{
				for (byte b5 = 0; b5 < Regions.WORLD_SIZE; b5 += 1)
				{
					List<ItemSpawnpoint> list = LevelItems.spawns[(int)b4, (int)b5];
					river.writeUInt16((ushort)list.Count);
					ushort num = 0;
					while ((int)num < list.Count)
					{
						ItemSpawnpoint itemSpawnpoint = list[(int)num];
						river.writeByte(itemSpawnpoint.type);
						river.writeSingleVector3(itemSpawnpoint.point);
						num += 1;
					}
				}
			}
			river.closeRiver();
		}

		public static readonly byte SAVEDATA_VERSION = 4;

		private static Transform _models;

		private static List<ItemTable> _tables;

		private static List<ItemSpawnpoint>[,] _spawns;
	}
}
