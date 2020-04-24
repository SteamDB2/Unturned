using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	public class LevelVehicles
	{
		public static Transform models
		{
			get
			{
				return LevelVehicles._models;
			}
		}

		public static List<VehicleTable> tables
		{
			get
			{
				return LevelVehicles._tables;
			}
		}

		public static List<VehicleSpawnpoint> spawns
		{
			get
			{
				return LevelVehicles._spawns;
			}
		}

		public static void setEnabled(bool isEnabled)
		{
			if (LevelVehicles.spawns == null)
			{
				return;
			}
			for (int i = 0; i < LevelVehicles.spawns.Count; i++)
			{
				LevelVehicles.spawns[i].setEnabled(isEnabled);
			}
		}

		public static void addTable(string name)
		{
			if (LevelVehicles.tables.Count == 255)
			{
				return;
			}
			LevelVehicles.tables.Add(new VehicleTable(name));
		}

		public static void removeTable()
		{
			LevelVehicles.tables.RemoveAt((int)EditorSpawns.selectedVehicle);
			List<VehicleSpawnpoint> list = new List<VehicleSpawnpoint>();
			for (int i = 0; i < LevelVehicles.spawns.Count; i++)
			{
				VehicleSpawnpoint vehicleSpawnpoint = LevelVehicles.spawns[i];
				if (vehicleSpawnpoint.type == EditorSpawns.selectedVehicle)
				{
					Object.Destroy(vehicleSpawnpoint.node.gameObject);
				}
				else
				{
					if (vehicleSpawnpoint.type > EditorSpawns.selectedVehicle)
					{
						VehicleSpawnpoint vehicleSpawnpoint2 = vehicleSpawnpoint;
						vehicleSpawnpoint2.type -= 1;
					}
					list.Add(vehicleSpawnpoint);
				}
			}
			LevelVehicles._spawns = list;
			EditorSpawns.selectedVehicle = 0;
			if ((int)EditorSpawns.selectedVehicle < LevelVehicles.tables.Count)
			{
				EditorSpawns.vehicleSpawn.GetComponent<Renderer>().material.color = LevelVehicles.tables[(int)EditorSpawns.selectedVehicle].color;
			}
		}

		public static void addSpawn(Vector3 point, float angle)
		{
			if ((int)EditorSpawns.selectedVehicle >= LevelVehicles.tables.Count)
			{
				return;
			}
			LevelVehicles.spawns.Add(new VehicleSpawnpoint(EditorSpawns.selectedVehicle, point, angle));
		}

		public static void removeSpawn(Vector3 point, float radius)
		{
			radius *= radius;
			List<VehicleSpawnpoint> list = new List<VehicleSpawnpoint>();
			for (int i = 0; i < LevelVehicles.spawns.Count; i++)
			{
				VehicleSpawnpoint vehicleSpawnpoint = LevelVehicles.spawns[i];
				if ((vehicleSpawnpoint.point - point).sqrMagnitude < radius)
				{
					Object.Destroy(vehicleSpawnpoint.node.gameObject);
				}
				else
				{
					list.Add(vehicleSpawnpoint);
				}
			}
			LevelVehicles._spawns = list;
		}

		public static ushort getVehicle(VehicleSpawnpoint spawn)
		{
			return LevelVehicles.getVehicle(spawn.type);
		}

		public static ushort getVehicle(byte type)
		{
			return LevelVehicles.tables[(int)type].getVehicle();
		}

		public static void load()
		{
			LevelVehicles._models = new GameObject().transform;
			LevelVehicles.models.name = "Vehicles";
			LevelVehicles.models.parent = Level.spawns;
			LevelVehicles.models.tag = "Logic";
			LevelVehicles.models.gameObject.layer = LayerMasks.LOGIC;
			if (Level.isEditor || Provider.isServer)
			{
				LevelVehicles._tables = new List<VehicleTable>();
				LevelVehicles._spawns = new List<VehicleSpawnpoint>();
				if (ReadWrite.fileExists(Level.info.path + "/Spawns/Vehicles.dat", false, false))
				{
					River river = new River(Level.info.path + "/Spawns/Vehicles.dat", false);
					byte b = river.readByte();
					if (b > 1 && b < 3)
					{
						river.readSteamID();
					}
					byte b2 = river.readByte();
					for (byte b3 = 0; b3 < b2; b3 += 1)
					{
						Color newColor = river.readColor();
						string text = river.readString();
						ushort num;
						if (b > 3)
						{
							num = river.readUInt16();
							if (num != 0 && SpawnTableTool.resolve(num) == 0)
							{
								Assets.errors.Add(string.Concat(new object[]
								{
									Level.info.name,
									" vehicle table \"",
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
						List<VehicleTier> list = new List<VehicleTier>();
						byte b4 = river.readByte();
						for (byte b5 = 0; b5 < b4; b5 += 1)
						{
							string newName = river.readString();
							float newChance = river.readSingle();
							List<VehicleSpawn> list2 = new List<VehicleSpawn>();
							byte b6 = river.readByte();
							for (byte b7 = 0; b7 < b6; b7 += 1)
							{
								ushort newVehicle = river.readUInt16();
								list2.Add(new VehicleSpawn(newVehicle));
							}
							list.Add(new VehicleTier(list2, newName, newChance));
						}
						LevelVehicles.tables.Add(new VehicleTable(list, newColor, text, num));
						if (!Level.isEditor)
						{
							LevelVehicles.tables[(int)b3].buildTable();
						}
					}
					ushort num2 = river.readUInt16();
					for (int i = 0; i < (int)num2; i++)
					{
						byte newType = river.readByte();
						Vector3 newPoint = river.readSingleVector3();
						float newAngle = (float)(river.readByte() * 2);
						LevelVehicles.spawns.Add(new VehicleSpawnpoint(newType, newPoint, newAngle));
					}
					river.closeRiver();
				}
			}
		}

		public static void save()
		{
			River river = new River(Level.info.path + "/Spawns/Vehicles.dat", false);
			river.writeByte(LevelVehicles.SAVEDATA_VERSION);
			river.writeByte((byte)LevelVehicles.tables.Count);
			byte b = 0;
			while ((int)b < LevelVehicles.tables.Count)
			{
				VehicleTable vehicleTable = LevelVehicles.tables[(int)b];
				river.writeColor(vehicleTable.color);
				river.writeString(vehicleTable.name);
				river.writeUInt16(vehicleTable.tableID);
				river.writeByte((byte)vehicleTable.tiers.Count);
				byte b2 = 0;
				while ((int)b2 < vehicleTable.tiers.Count)
				{
					VehicleTier vehicleTier = vehicleTable.tiers[(int)b2];
					river.writeString(vehicleTier.name);
					river.writeSingle(vehicleTier.chance);
					river.writeByte((byte)vehicleTier.table.Count);
					byte b3 = 0;
					while ((int)b3 < vehicleTier.table.Count)
					{
						VehicleSpawn vehicleSpawn = vehicleTier.table[(int)b3];
						river.writeUInt16(vehicleSpawn.vehicle);
						b3 += 1;
					}
					b2 += 1;
				}
				b += 1;
			}
			river.writeUInt16((ushort)LevelVehicles.spawns.Count);
			for (int i = 0; i < LevelVehicles.spawns.Count; i++)
			{
				VehicleSpawnpoint vehicleSpawnpoint = LevelVehicles.spawns[i];
				river.writeByte(vehicleSpawnpoint.type);
				river.writeSingleVector3(vehicleSpawnpoint.point);
				river.writeByte(MeasurementTool.angleToByte(vehicleSpawnpoint.angle));
			}
			river.closeRiver();
		}

		public static readonly byte SAVEDATA_VERSION = 4;

		private static Transform _models;

		private static List<VehicleTable> _tables;

		private static List<VehicleSpawnpoint> _spawns;
	}
}
