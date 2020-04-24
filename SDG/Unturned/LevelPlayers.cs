using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	public class LevelPlayers
	{
		public static Transform models
		{
			get
			{
				return LevelPlayers._models;
			}
		}

		public static List<PlayerSpawnpoint> spawns
		{
			get
			{
				return LevelPlayers._spawns;
			}
		}

		public static void setEnabled(bool isEnabled)
		{
			for (int i = 0; i < LevelPlayers.spawns.Count; i++)
			{
				LevelPlayers.spawns[i].setEnabled(isEnabled);
			}
		}

		public static bool checkCanBuild(Vector3 point)
		{
			for (int i = 0; i < LevelPlayers.spawns.Count; i++)
			{
				PlayerSpawnpoint playerSpawnpoint = LevelPlayers.spawns[i];
				if ((playerSpawnpoint.point - point).sqrMagnitude < 256f)
				{
					return false;
				}
			}
			return true;
		}

		public static void addSpawn(Vector3 point, float angle, bool isAlt)
		{
			LevelPlayers.spawns.Add(new PlayerSpawnpoint(point, angle, isAlt));
		}

		public static void removeSpawn(Vector3 point, float radius)
		{
			radius *= radius;
			List<PlayerSpawnpoint> list = new List<PlayerSpawnpoint>();
			for (int i = 0; i < LevelPlayers.spawns.Count; i++)
			{
				PlayerSpawnpoint playerSpawnpoint = LevelPlayers.spawns[i];
				if ((playerSpawnpoint.point - point).sqrMagnitude < radius)
				{
					Object.Destroy(playerSpawnpoint.node.gameObject);
				}
				else
				{
					list.Add(playerSpawnpoint);
				}
			}
			LevelPlayers._spawns = list;
		}

		public static List<PlayerSpawnpoint> getRegSpawns()
		{
			List<PlayerSpawnpoint> list = new List<PlayerSpawnpoint>();
			for (int i = 0; i < LevelPlayers.spawns.Count; i++)
			{
				PlayerSpawnpoint playerSpawnpoint = LevelPlayers.spawns[i];
				if (!playerSpawnpoint.isAlt)
				{
					list.Add(playerSpawnpoint);
				}
			}
			return list;
		}

		public static List<PlayerSpawnpoint> getAltSpawns()
		{
			List<PlayerSpawnpoint> list = new List<PlayerSpawnpoint>();
			for (int i = 0; i < LevelPlayers.spawns.Count; i++)
			{
				PlayerSpawnpoint playerSpawnpoint = LevelPlayers.spawns[i];
				if (playerSpawnpoint.isAlt)
				{
					list.Add(playerSpawnpoint);
				}
			}
			return list;
		}

		public static PlayerSpawnpoint getSpawn(bool isAlt)
		{
			List<PlayerSpawnpoint> list = (!isAlt) ? LevelPlayers.getRegSpawns() : LevelPlayers.getAltSpawns();
			if (list.Count == 0)
			{
				return new PlayerSpawnpoint(new Vector3(0f, 256f, 0f), 0f, isAlt);
			}
			return list[Random.Range(0, list.Count)];
		}

		public static void load()
		{
			LevelPlayers._models = new GameObject().transform;
			LevelPlayers.models.name = "Players";
			LevelPlayers.models.parent = Level.spawns;
			LevelPlayers.models.tag = "Logic";
			LevelPlayers.models.gameObject.layer = LayerMasks.LOGIC;
			LevelPlayers._spawns = new List<PlayerSpawnpoint>();
			if (ReadWrite.fileExists(Level.info.path + "/Spawns/Players.dat", false, false))
			{
				River river = new River(Level.info.path + "/Spawns/Players.dat", false);
				byte b = river.readByte();
				if (b > 1 && b < 3)
				{
					river.readSteamID();
				}
				int num = 0;
				int num2 = 0;
				byte b2 = river.readByte();
				for (int i = 0; i < (int)b2; i++)
				{
					Vector3 point = river.readSingleVector3();
					float angle = (float)(river.readByte() * 2);
					bool flag = false;
					if (b > 3)
					{
						flag = river.readBoolean();
					}
					if (flag)
					{
						num2++;
					}
					else
					{
						num++;
					}
					LevelPlayers.addSpawn(point, angle, flag);
				}
				river.closeRiver();
			}
		}

		public static void save()
		{
			River river = new River(Level.info.path + "/Spawns/Players.dat", false);
			river.writeByte(LevelPlayers.SAVEDATA_VERSION);
			river.writeByte((byte)LevelPlayers.spawns.Count);
			for (int i = 0; i < LevelPlayers.spawns.Count; i++)
			{
				PlayerSpawnpoint playerSpawnpoint = LevelPlayers.spawns[i];
				river.writeSingleVector3(playerSpawnpoint.point);
				river.writeByte(MeasurementTool.angleToByte(playerSpawnpoint.angle));
				river.writeBoolean(playerSpawnpoint.isAlt);
			}
			river.closeRiver();
		}

		public static readonly byte SAVEDATA_VERSION = 4;

		private static Transform _models;

		private static List<PlayerSpawnpoint> _spawns;
	}
}
