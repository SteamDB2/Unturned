using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class SpawnTableTool
	{
		public static ushort resolve(ushort id)
		{
			SpawnAsset spawnAsset = (SpawnAsset)Assets.find(EAssetType.SPAWN, id);
			if (spawnAsset == null)
			{
				return 0;
			}
			bool flag;
			spawnAsset.resolve(out id, out flag);
			if (flag)
			{
				id = SpawnTableTool.resolve(id);
			}
			return id;
		}

		private static bool isVariantItemTier(ItemTier tier)
		{
			if (tier.table.Count < 6)
			{
				return false;
			}
			ItemAsset itemAsset = (ItemAsset)Assets.find(EAssetType.ITEM, tier.table[0].item);
			if (itemAsset == null)
			{
				return false;
			}
			int num = itemAsset.itemName.IndexOf(" ");
			if (num <= 0)
			{
				return false;
			}
			string text = itemAsset.itemName.Substring(num + 1);
			if (text.Length <= 1)
			{
				Debug.LogError(itemAsset.itemName + " name has a trailing space!");
				return false;
			}
			for (int i = 1; i < tier.table.Count; i++)
			{
				ItemAsset itemAsset2 = (ItemAsset)Assets.find(EAssetType.ITEM, tier.table[i].item);
				if (!itemAsset2.itemName.Contains(text))
				{
					return false;
				}
			}
			tier.name = text;
			return true;
		}

		private static bool isVariantVehicleTier(VehicleTier tier)
		{
			if (tier.table.Count < 6)
			{
				return false;
			}
			VehicleAsset vehicleAsset = (VehicleAsset)Assets.find(EAssetType.VEHICLE, tier.table[0].vehicle);
			if (vehicleAsset == null)
			{
				return false;
			}
			int num = vehicleAsset.vehicleName.IndexOf(" ");
			if (num <= 0)
			{
				return false;
			}
			string text = vehicleAsset.vehicleName.Substring(num + 1);
			if (text.Length <= 1)
			{
				Debug.LogError(vehicleAsset.vehicleName + " name has a trailing space!");
				return false;
			}
			for (int i = 1; i < tier.table.Count; i++)
			{
				VehicleAsset vehicleAsset2 = (VehicleAsset)Assets.find(EAssetType.VEHICLE, tier.table[i].vehicle);
				if (!vehicleAsset2.vehicleName.Contains(text))
				{
					return false;
				}
			}
			tier.name = text;
			return true;
		}

		private static void exportItems(string path, Data spawnsData, ref ushort id, bool isLegacy)
		{
			for (int i = 0; i < LevelItems.tables.Count; i++)
			{
				ItemTable itemTable = LevelItems.tables[i];
				if (itemTable.tableID == 0)
				{
					itemTable.tableID = id;
					spawnsData.writeString(id.ToString(), Level.info.name + "_" + itemTable.name);
					Data data = new Data();
					data.writeString("Type", "Spawn");
					Data data2 = data;
					string key = "ID";
					ushort value;
					id = (value = id) + 1;
					data2.writeUInt16(key, value);
					if (ReadWrite.fileExists(string.Concat(new string[]
					{
						"/Bundles/Spawns/Items/",
						itemTable.name,
						"/",
						itemTable.name,
						".dat"
					}), false, true))
					{
						Data data3 = ReadWrite.readData(string.Concat(new string[]
						{
							"/Bundles/Spawns/Items/",
							itemTable.name,
							"/",
							itemTable.name,
							".dat"
						}), false, true);
						data.writeInt32("Tables", 1);
						data.writeUInt16("Table_0_Spawn_ID", data3.readUInt16("ID"));
						data.writeInt32("Table_0_Weight", 100);
					}
					else
					{
						data.writeInt32("Tables", 1);
						data.writeUInt16("Table_0_Spawn_ID", id);
						data.writeInt32("Table_0_Weight", 100);
						spawnsData.writeString(id.ToString(), itemTable.name);
						Data data4 = new Data();
						data4.writeString("Type", "Spawn");
						Data data5 = data4;
						string key2 = "ID";
						id = (value = id) + 1;
						data5.writeUInt16(key2, value);
						if (isLegacy)
						{
							if (itemTable.tiers.Count > 1)
							{
								float num = float.MaxValue;
								for (int j = 0; j < itemTable.tiers.Count; j++)
								{
									ItemTier itemTier = itemTable.tiers[j];
									if (itemTier.chance < num)
									{
										num = itemTier.chance;
									}
								}
								int num2 = Mathf.CeilToInt(10f / num);
								data4.writeInt32("Tables", itemTable.tiers.Count);
								for (int k = 0; k < itemTable.tiers.Count; k++)
								{
									ItemTier itemTier2 = itemTable.tiers[k];
									bool flag = SpawnTableTool.isVariantItemTier(itemTier2);
									if (flag && ReadWrite.fileExists(string.Concat(new string[]
									{
										"/Bundles/Spawns/Items/",
										itemTier2.name,
										"/",
										itemTier2.name,
										".dat"
									}), false, true))
									{
										Data data6 = ReadWrite.readData(string.Concat(new string[]
										{
											"/Bundles/Spawns/Items/",
											itemTier2.name,
											"/",
											itemTier2.name,
											".dat"
										}), false, true);
										data4.writeUInt16("Table_" + k + "_Spawn_ID", data6.readUInt16("ID"));
										data4.writeInt32("Table_" + k + "_Weight", (int)(itemTier2.chance * (float)num2));
									}
									else if (flag && ReadWrite.fileExists(string.Concat(new string[]
									{
										path,
										"/Items/",
										itemTier2.name,
										"/",
										itemTier2.name,
										".dat"
									}), false, false))
									{
										Data data7 = ReadWrite.readData(string.Concat(new string[]
										{
											path,
											"/Items/",
											itemTier2.name,
											"/",
											itemTier2.name,
											".dat"
										}), false, false);
										data4.writeUInt16("Table_" + k + "_Spawn_ID", data7.readUInt16("ID"));
										data4.writeInt32("Table_" + k + "_Weight", (int)(itemTier2.chance * (float)num2));
									}
									else
									{
										data4.writeUInt16("Table_" + k + "_Spawn_ID", id);
										data4.writeInt32("Table_" + k + "_Weight", (int)(itemTier2.chance * (float)num2));
										if (flag)
										{
											spawnsData.writeString(id.ToString(), itemTier2.name);
										}
										else
										{
											spawnsData.writeString(id.ToString(), itemTable.name + "_" + itemTier2.name);
										}
										Data data8 = new Data();
										data8.writeString("Type", "Spawn");
										Data data9 = data8;
										string key3 = "ID";
										id = (value = id) + 1;
										data9.writeUInt16(key3, value);
										data8.writeInt32("Tables", itemTier2.table.Count);
										for (int l = 0; l < itemTier2.table.Count; l++)
										{
											ItemSpawn itemSpawn = itemTier2.table[l];
											data8.writeUInt16("Table_" + l + "_Asset_ID", itemSpawn.item);
											data8.writeInt32("Table_" + l + "_Weight", 10);
										}
										if (flag)
										{
											ReadWrite.writeData(string.Concat(new string[]
											{
												path,
												"/Items/",
												itemTier2.name,
												"/",
												itemTier2.name,
												".dat"
											}), false, false, data8);
										}
										else
										{
											ReadWrite.writeData(string.Concat(new string[]
											{
												path,
												"/Items/",
												itemTable.name,
												"_",
												itemTier2.name,
												"/",
												itemTable.name,
												"_",
												itemTier2.name,
												".dat"
											}), false, false, data8);
										}
									}
								}
							}
							else
							{
								ItemTier itemTier3 = itemTable.tiers[0];
								data4.writeInt32("Tables", itemTier3.table.Count);
								for (int m = 0; m < itemTier3.table.Count; m++)
								{
									ItemSpawn itemSpawn2 = itemTier3.table[m];
									data4.writeUInt16("Table_" + m + "_Asset_ID", itemSpawn2.item);
									data4.writeInt32("Table_" + m + "_Weight", 10);
								}
							}
						}
						ReadWrite.writeData(string.Concat(new string[]
						{
							path,
							"/Items/",
							itemTable.name,
							"/",
							itemTable.name,
							".dat"
						}), false, false, data4);
					}
					ReadWrite.writeData(string.Concat(new string[]
					{
						path,
						"/Items/",
						Level.info.name,
						"_",
						itemTable.name,
						"/",
						Level.info.name,
						"_",
						itemTable.name,
						".dat"
					}), false, false, data);
				}
			}
		}

		private static void exportVehicles(string path, Data spawnsData, ref ushort id, bool isLegacy)
		{
			for (int i = 0; i < LevelVehicles.tables.Count; i++)
			{
				VehicleTable vehicleTable = LevelVehicles.tables[i];
				if (vehicleTable.tableID == 0)
				{
					vehicleTable.tableID = id;
					spawnsData.writeString(id.ToString(), Level.info.name + "_" + vehicleTable.name);
					Data data = new Data();
					data.writeString("Type", "Spawn");
					Data data2 = data;
					string key = "ID";
					ushort value;
					id = (value = id) + 1;
					data2.writeUInt16(key, value);
					if (ReadWrite.fileExists(string.Concat(new string[]
					{
						"/Bundles/Spawns/Vehicles/",
						vehicleTable.name,
						"/",
						vehicleTable.name,
						".dat"
					}), false, true))
					{
						Data data3 = ReadWrite.readData(string.Concat(new string[]
						{
							"/Bundles/Spawns/Vehicles/",
							vehicleTable.name,
							"/",
							vehicleTable.name,
							".dat"
						}), false, true);
						data.writeInt32("Tables", 1);
						data.writeUInt16("Table_0_Spawn_ID", data3.readUInt16("ID"));
						data.writeInt32("Table_0_Weight", 100);
					}
					else
					{
						data.writeInt32("Tables", 1);
						data.writeUInt16("Table_0_Spawn_ID", id);
						data.writeInt32("Table_0_Weight", 100);
						spawnsData.writeString(id.ToString(), vehicleTable.name);
						Data data4 = new Data();
						data4.writeString("Type", "Spawn");
						Data data5 = data4;
						string key2 = "ID";
						id = (value = id) + 1;
						data5.writeUInt16(key2, value);
						if (isLegacy)
						{
							if (vehicleTable.tiers.Count > 1)
							{
								float num = float.MaxValue;
								for (int j = 0; j < vehicleTable.tiers.Count; j++)
								{
									VehicleTier vehicleTier = vehicleTable.tiers[j];
									if (vehicleTier.chance < num)
									{
										num = vehicleTier.chance;
									}
								}
								int num2 = Mathf.CeilToInt(10f / num);
								data4.writeInt32("Tables", vehicleTable.tiers.Count);
								for (int k = 0; k < vehicleTable.tiers.Count; k++)
								{
									VehicleTier vehicleTier2 = vehicleTable.tiers[k];
									bool flag = SpawnTableTool.isVariantVehicleTier(vehicleTier2);
									if (flag && ReadWrite.fileExists(string.Concat(new string[]
									{
										"/Bundles/Spawns/Vehicles/",
										vehicleTier2.name,
										"/",
										vehicleTier2.name,
										".dat"
									}), false, true))
									{
										Data data6 = ReadWrite.readData(string.Concat(new string[]
										{
											"/Bundles/Spawns/Vehicles/",
											vehicleTier2.name,
											"/",
											vehicleTier2.name,
											".dat"
										}), false, true);
										data4.writeUInt16("Table_" + k + "_Spawn_ID", data6.readUInt16("ID"));
										data4.writeInt32("Table_" + k + "_Weight", (int)(vehicleTier2.chance * (float)num2));
									}
									else if (flag && ReadWrite.fileExists(string.Concat(new string[]
									{
										path,
										"/Vehicles/",
										vehicleTier2.name,
										"/",
										vehicleTier2.name,
										".dat"
									}), false, false))
									{
										Data data7 = ReadWrite.readData(string.Concat(new string[]
										{
											path,
											"/Vehicles/",
											vehicleTier2.name,
											"/",
											vehicleTier2.name,
											".dat"
										}), false, false);
										data4.writeUInt16("Table_" + k + "_Spawn_ID", data7.readUInt16("ID"));
										data4.writeInt32("Table_" + k + "_Weight", (int)(vehicleTier2.chance * (float)num2));
									}
									else
									{
										data4.writeUInt16("Table_" + k + "_Spawn_ID", id);
										data4.writeInt32("Table_" + k + "_Weight", (int)(vehicleTier2.chance * (float)num2));
										if (flag)
										{
											spawnsData.writeString(id.ToString(), vehicleTier2.name);
										}
										else
										{
											spawnsData.writeString(id.ToString(), vehicleTable.name + "_" + vehicleTier2.name);
										}
										Data data8 = new Data();
										data8.writeString("Type", "Spawn");
										Data data9 = data8;
										string key3 = "ID";
										id = (value = id) + 1;
										data9.writeUInt16(key3, value);
										data8.writeInt32("Tables", vehicleTier2.table.Count);
										for (int l = 0; l < vehicleTier2.table.Count; l++)
										{
											VehicleSpawn vehicleSpawn = vehicleTier2.table[l];
											data8.writeUInt16("Table_" + l + "_Asset_ID", vehicleSpawn.vehicle);
											data8.writeInt32("Table_" + l + "_Weight", 10);
										}
										if (flag)
										{
											ReadWrite.writeData(string.Concat(new string[]
											{
												path,
												"/Vehicles/",
												vehicleTier2.name,
												"/",
												vehicleTier2.name,
												".dat"
											}), false, false, data8);
										}
										else
										{
											ReadWrite.writeData(string.Concat(new string[]
											{
												path,
												"/Vehicles/",
												vehicleTable.name,
												"_",
												vehicleTier2.name,
												"/",
												vehicleTable.name,
												"_",
												vehicleTier2.name,
												".dat"
											}), false, false, data8);
										}
									}
								}
							}
							else
							{
								VehicleTier vehicleTier3 = vehicleTable.tiers[0];
								data4.writeInt32("Tables", vehicleTier3.table.Count);
								for (int m = 0; m < vehicleTier3.table.Count; m++)
								{
									VehicleSpawn vehicleSpawn2 = vehicleTier3.table[m];
									data4.writeUInt16("Table_" + m + "_Asset_ID", vehicleSpawn2.vehicle);
									data4.writeInt32("Table_" + m + "_Weight", 10);
								}
							}
						}
						ReadWrite.writeData(string.Concat(new string[]
						{
							path,
							"/Vehicles/",
							vehicleTable.name,
							"/",
							vehicleTable.name,
							".dat"
						}), false, false, data4);
					}
					ReadWrite.writeData(string.Concat(new string[]
					{
						path,
						"/Vehicles/",
						Level.info.name,
						"_",
						vehicleTable.name,
						"/",
						Level.info.name,
						"_",
						vehicleTable.name,
						".dat"
					}), false, false, data);
				}
			}
		}

		private static void exportZombies(string path, Data spawnsData, ref ushort id, bool isLegacy)
		{
			for (int i = 0; i < LevelZombies.tables.Count; i++)
			{
				ZombieTable zombieTable = LevelZombies.tables[i];
				if (zombieTable.lootID == 0 && (int)zombieTable.lootIndex < LevelItems.tables.Count)
				{
					zombieTable.lootID = LevelItems.tables[(int)zombieTable.lootIndex].tableID;
				}
			}
		}

		private static void exportAnimals(string path, Data spawnsData, ref ushort id, bool isLegacy)
		{
			for (int i = 0; i < LevelAnimals.tables.Count; i++)
			{
				AnimalTable animalTable = LevelAnimals.tables[i];
				if (animalTable.tableID == 0)
				{
					animalTable.tableID = id;
					spawnsData.writeString(id.ToString(), Level.info.name + "_" + animalTable.name);
					Data data = new Data();
					data.writeString("Type", "Spawn");
					Data data2 = data;
					string key = "ID";
					ushort value;
					id = (value = id) + 1;
					data2.writeUInt16(key, value);
					if (ReadWrite.fileExists(string.Concat(new string[]
					{
						"/Bundles/Spawns/Animals/",
						animalTable.name,
						"/",
						animalTable.name,
						".dat"
					}), false, true))
					{
						Data data3 = ReadWrite.readData(string.Concat(new string[]
						{
							"/Bundles/Spawns/Animals/",
							animalTable.name,
							"/",
							animalTable.name,
							".dat"
						}), false, true);
						data.writeInt32("Tables", 1);
						data.writeUInt16("Table_0_Spawn_ID", data3.readUInt16("ID"));
						data.writeInt32("Table_0_Weight", 100);
					}
					else
					{
						data.writeInt32("Tables", 1);
						data.writeUInt16("Table_0_Spawn_ID", id);
						data.writeInt32("Table_0_Weight", 100);
						spawnsData.writeString(id.ToString(), animalTable.name);
						Data data4 = new Data();
						data4.writeString("Type", "Spawn");
						Data data5 = data4;
						string key2 = "ID";
						id = (value = id) + 1;
						data5.writeUInt16(key2, value);
						if (isLegacy)
						{
							if (animalTable.tiers.Count > 1)
							{
								float num = float.MaxValue;
								for (int j = 0; j < animalTable.tiers.Count; j++)
								{
									AnimalTier animalTier = animalTable.tiers[j];
									if (animalTier.chance < num)
									{
										num = animalTier.chance;
									}
								}
								int num2 = Mathf.CeilToInt(10f / num);
								data4.writeInt32("Tables", animalTable.tiers.Count);
								for (int k = 0; k < animalTable.tiers.Count; k++)
								{
									AnimalTier animalTier2 = animalTable.tiers[k];
									data4.writeUInt16("Table_" + k + "_Spawn_ID", id);
									data4.writeInt32("Table_" + k + "_Weight", (int)(animalTier2.chance * (float)num2));
									spawnsData.writeString(id.ToString(), animalTable.name + "_" + animalTier2.name);
									Data data6 = new Data();
									data6.writeString("Type", "Spawn");
									Data data7 = data6;
									string key3 = "ID";
									id = (value = id) + 1;
									data7.writeUInt16(key3, value);
									data6.writeInt32("Tables", animalTier2.table.Count);
									for (int l = 0; l < animalTier2.table.Count; l++)
									{
										AnimalSpawn animalSpawn = animalTier2.table[l];
										data6.writeUInt16("Table_" + l + "_Asset_ID", animalSpawn.animal);
										data6.writeInt32("Table_" + l + "_Weight", 10);
									}
									ReadWrite.writeData(string.Concat(new string[]
									{
										path,
										"/Animals/",
										animalTable.name,
										"_",
										animalTier2.name,
										"/",
										animalTable.name,
										"_",
										animalTier2.name,
										".dat"
									}), false, false, data6);
								}
							}
							else
							{
								AnimalTier animalTier3 = animalTable.tiers[0];
								data4.writeInt32("Tables", animalTier3.table.Count);
								for (int m = 0; m < animalTier3.table.Count; m++)
								{
									AnimalSpawn animalSpawn2 = animalTier3.table[m];
									data4.writeUInt16("Table_" + m + "_Asset_ID", animalSpawn2.animal);
									data4.writeInt32("Table_" + m + "_Weight", 10);
								}
							}
						}
						ReadWrite.writeData(string.Concat(new string[]
						{
							path,
							"/Animals/",
							animalTable.name,
							"/",
							animalTable.name,
							".dat"
						}), false, false, data4);
					}
					ReadWrite.writeData(string.Concat(new string[]
					{
						path,
						"/Animals/",
						Level.info.name,
						"_",
						animalTable.name,
						"/",
						Level.info.name,
						"_",
						animalTable.name,
						".dat"
					}), false, false, data);
				}
			}
		}

		public static void export(ushort id, bool isLegacy)
		{
			string text = Level.info.path;
			if (isLegacy)
			{
				text += "/Exported_Legacy_Spawn_Tables";
			}
			else
			{
				text += "/Exported_Proxy_Spawn_Tables";
			}
			if (ReadWrite.folderExists(text, false))
			{
				ReadWrite.deleteFolder(text, false);
			}
			Data data = new Data();
			data.writeString("ID", "Spawn");
			SpawnTableTool.exportItems(text, data, ref id, isLegacy);
			SpawnTableTool.exportVehicles(text, data, ref id, isLegacy);
			SpawnTableTool.exportZombies(text, data, ref id, isLegacy);
			SpawnTableTool.exportAnimals(text, data, ref id, isLegacy);
			data.isCSV = true;
			ReadWrite.writeData(text + "/IDs.csv", false, false, data);
		}
	}
}
