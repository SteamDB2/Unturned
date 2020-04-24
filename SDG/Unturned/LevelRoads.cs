using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SDG.Framework.Debug;
using SDG.Framework.Landscapes;
using UnityEngine;

namespace SDG.Unturned
{
	public class LevelRoads
	{
		[TerminalCommandMethod("build.roads", "re-create road meshes")]
		public static void buildRoadsCommand()
		{
			LevelRoads.bakeRoads();
		}

		public static Transform models
		{
			get
			{
				return LevelRoads._models;
			}
		}

		public static RoadMaterial[] materials
		{
			get
			{
				return LevelRoads._materials;
			}
		}

		public static void setEnabled(bool isEnabled)
		{
			for (int i = 0; i < LevelRoads.roads.Count; i++)
			{
				LevelRoads.roads[i].setEnabled(isEnabled);
			}
		}

		public static Transform addRoad(Vector3 point)
		{
			LevelRoads.roads.Add(new Road(EditorRoads.selected));
			return LevelRoads.roads[LevelRoads.roads.Count - 1].addVertex(0, point);
		}

		[Obsolete]
		public static void removeRoad(Transform select)
		{
			for (int i = 0; i < LevelRoads.roads.Count; i++)
			{
				for (int j = 0; j < LevelRoads.roads[i].paths.Count; j++)
				{
					if (LevelRoads.roads[i].paths[j].vertex == select)
					{
						LevelRoads.roads[i].remove();
						LevelRoads.roads.RemoveAt(i);
						return;
					}
				}
			}
		}

		public static void removeRoad(Road road)
		{
			for (int i = 0; i < LevelRoads.roads.Count; i++)
			{
				if (LevelRoads.roads[i] == road)
				{
					LevelRoads.roads[i].remove();
					LevelRoads.roads.RemoveAt(i);
					return;
				}
			}
		}

		public static RoadMaterial getRoadMaterial(Transform road)
		{
			if (road == null || road.parent == null)
			{
				return null;
			}
			for (int i = 0; i < LevelRoads.roads.Count; i++)
			{
				if (LevelRoads.roads[i].road == road || LevelRoads.roads[i].road == road.parent)
				{
					return LevelRoads.materials[(int)LevelRoads.roads[i].material];
				}
			}
			return null;
		}

		public static Road getRoad(Transform target, out int vertexIndex, out int tangentIndex)
		{
			vertexIndex = -1;
			tangentIndex = -1;
			for (int i = 0; i < LevelRoads.roads.Count; i++)
			{
				Road road = LevelRoads.roads[i];
				for (int j = 0; j < road.paths.Count; j++)
				{
					RoadPath roadPath = road.paths[j];
					if (roadPath.vertex == target)
					{
						vertexIndex = j;
						return road;
					}
					if (roadPath.tangents[0] == target)
					{
						vertexIndex = j;
						tangentIndex = 0;
						return road;
					}
					if (roadPath.tangents[1] == target)
					{
						vertexIndex = j;
						tangentIndex = 1;
						return road;
					}
				}
			}
			return null;
		}

		public static void bakeRoads()
		{
			for (int i = 0; i < LevelRoads.roads.Count; i++)
			{
				LevelRoads.roads[i].updatePoints();
			}
			LevelRoads.buildMeshes();
		}

		public static void load()
		{
			LevelRoads._models = new GameObject().transform;
			LevelRoads.models.name = "Roads";
			LevelRoads.models.parent = Level.level;
			LevelRoads.models.tag = "Logic";
			LevelRoads.models.gameObject.layer = LayerMasks.LOGIC;
			if (ReadWrite.fileExists(Level.info.path + "/Environment/Roads.unity3d", false, false))
			{
				Bundle bundle = Bundles.getBundle(Level.info.path + "/Environment/Roads.unity3d", false);
				Object[] array = bundle.load();
				bundle.unload();
				LevelRoads._materials = new RoadMaterial[array.Length];
				for (int i = 0; i < LevelRoads.materials.Length; i++)
				{
					LevelRoads.materials[i] = new RoadMaterial((Texture2D)array[i]);
				}
			}
			else
			{
				LevelRoads._materials = new RoadMaterial[0];
			}
			LevelRoads.roads = new List<Road>();
			if (ReadWrite.fileExists(Level.info.path + "/Environment/Roads.dat", false, false))
			{
				River river = new River(Level.info.path + "/Environment/Roads.dat", false);
				byte b = river.readByte();
				if (b > 0)
				{
					byte b2 = river.readByte();
					for (byte b3 = 0; b3 < b2; b3 += 1)
					{
						if ((int)b3 >= LevelRoads.materials.Length)
						{
							break;
						}
						LevelRoads.materials[(int)b3].width = river.readSingle();
						LevelRoads.materials[(int)b3].height = river.readSingle();
						LevelRoads.materials[(int)b3].depth = river.readSingle();
						if (b > 1)
						{
							LevelRoads.materials[(int)b3].offset = river.readSingle();
						}
						LevelRoads.materials[(int)b3].isConcrete = river.readBoolean();
					}
				}
				river.closeRiver();
			}
			if (ReadWrite.fileExists(Level.info.path + "/Environment/Paths.dat", false, false))
			{
				River river2 = new River(Level.info.path + "/Environment/Paths.dat", false);
				byte b4 = river2.readByte();
				if (b4 > 1)
				{
					ushort num = river2.readUInt16();
					for (ushort num2 = 0; num2 < num; num2 += 1)
					{
						ushort num3 = river2.readUInt16();
						byte newMaterial = river2.readByte();
						bool newLoop = b4 > 2 && river2.readBoolean();
						List<RoadJoint> list = new List<RoadJoint>();
						for (ushort num4 = 0; num4 < num3; num4 += 1)
						{
							Vector3 vertex = river2.readSingleVector3();
							Vector3[] array2 = new Vector3[2];
							if (b4 > 2)
							{
								array2[0] = river2.readSingleVector3();
								array2[1] = river2.readSingleVector3();
							}
							ERoadMode mode;
							if (b4 > 2)
							{
								mode = (ERoadMode)river2.readByte();
							}
							else
							{
								mode = ERoadMode.FREE;
							}
							float offset;
							if (b4 > 4)
							{
								offset = river2.readSingle();
							}
							else
							{
								offset = 0f;
							}
							bool ignoreTerrain = b4 > 3 && river2.readBoolean();
							RoadJoint item = new RoadJoint(vertex, array2, mode, offset, ignoreTerrain);
							list.Add(item);
						}
						if (b4 < 3)
						{
							for (ushort num5 = 0; num5 < num3; num5 += 1)
							{
								RoadJoint roadJoint = list[(int)num5];
								if (num5 == 0)
								{
									roadJoint.setTangent(0, (roadJoint.vertex - list[(int)(num5 + 1)].vertex).normalized * 2.5f);
									roadJoint.setTangent(1, (list[(int)(num5 + 1)].vertex - roadJoint.vertex).normalized * 2.5f);
								}
								else if (num5 == num3 - 1)
								{
									roadJoint.setTangent(0, (list[(int)(num5 - 1)].vertex - roadJoint.vertex).normalized * 2.5f);
									roadJoint.setTangent(1, (roadJoint.vertex - list[(int)(num5 - 1)].vertex).normalized * 2.5f);
								}
								else
								{
									roadJoint.setTangent(0, (list[(int)(num5 - 1)].vertex - roadJoint.vertex).normalized * 2.5f);
									roadJoint.setTangent(1, (list[(int)(num5 + 1)].vertex - roadJoint.vertex).normalized * 2.5f);
								}
							}
						}
						LevelRoads.roads.Add(new Road(newMaterial, newLoop, list));
					}
				}
				else if (b4 > 0)
				{
					byte b5 = river2.readByte();
					for (byte b6 = 0; b6 < b5; b6 += 1)
					{
						byte b7 = river2.readByte();
						byte newMaterial2 = river2.readByte();
						List<RoadJoint> list2 = new List<RoadJoint>();
						for (byte b8 = 0; b8 < b7; b8 += 1)
						{
							Vector3 vertex2 = river2.readSingleVector3();
							Vector3[] tangents = new Vector3[2];
							ERoadMode mode2 = ERoadMode.FREE;
							RoadJoint item2 = new RoadJoint(vertex2, tangents, mode2, 0f, false);
							list2.Add(item2);
						}
						for (byte b9 = 0; b9 < b7; b9 += 1)
						{
							RoadJoint roadJoint2 = list2[(int)b9];
							if (b9 == 0)
							{
								roadJoint2.setTangent(0, (roadJoint2.vertex - list2[(int)(b9 + 1)].vertex).normalized * 2.5f);
								roadJoint2.setTangent(1, (list2[(int)(b9 + 1)].vertex - roadJoint2.vertex).normalized * 2.5f);
							}
							else if (b9 == b7 - 1)
							{
								roadJoint2.setTangent(0, (list2[(int)(b9 - 1)].vertex - roadJoint2.vertex).normalized * 2.5f);
								roadJoint2.setTangent(1, (roadJoint2.vertex - list2[(int)(b9 - 1)].vertex).normalized * 2.5f);
							}
							else
							{
								roadJoint2.setTangent(0, (list2[(int)(b9 - 1)].vertex - roadJoint2.vertex).normalized * 2.5f);
								roadJoint2.setTangent(1, (list2[(int)(b9 + 1)].vertex - roadJoint2.vertex).normalized * 2.5f);
							}
						}
						LevelRoads.roads.Add(new Road(newMaterial2, false, list2));
					}
				}
				river2.closeRiver();
			}
			if (LevelGround.terrain != null)
			{
				LevelRoads.buildMeshes();
			}
			if (!LevelRoads.isListeningForLandscape)
			{
				LevelRoads.isListeningForLandscape = true;
				if (LevelRoads.<>f__mg$cache0 == null)
				{
					LevelRoads.<>f__mg$cache0 = new LandscapeLoadedHandler(LevelRoads.handleLandscapeLoaded);
				}
				Landscape.loaded += LevelRoads.<>f__mg$cache0;
			}
		}

		public static void save()
		{
			River river = new River(Level.info.path + "/Environment/Roads.dat", false);
			river.writeByte(LevelRoads.SAVEDATA_ROADS_VERSION);
			river.writeByte((byte)LevelRoads.materials.Length);
			byte b = 0;
			while ((int)b < LevelRoads.materials.Length)
			{
				river.writeSingle(LevelRoads.materials[(int)b].width);
				river.writeSingle(LevelRoads.materials[(int)b].height);
				river.writeSingle(LevelRoads.materials[(int)b].depth);
				river.writeSingle(LevelRoads.materials[(int)b].offset);
				river.writeBoolean(LevelRoads.materials[(int)b].isConcrete);
				b += 1;
			}
			river.closeRiver();
			river = new River(Level.info.path + "/Environment/Paths.dat", false);
			river.writeByte(LevelRoads.SAVEDATA_PATHS_VERSION);
			ushort num = 0;
			ushort num2 = 0;
			while ((int)num2 < LevelRoads.roads.Count)
			{
				if (LevelRoads.roads[(int)num2].joints.Count > 1)
				{
					num += 1;
				}
				num2 += 1;
			}
			river.writeUInt16(num);
			byte b2 = 0;
			while ((int)b2 < LevelRoads.roads.Count)
			{
				List<RoadJoint> joints = LevelRoads.roads[(int)b2].joints;
				if (joints.Count > 1)
				{
					river.writeUInt16((ushort)joints.Count);
					river.writeByte(LevelRoads.roads[(int)b2].material);
					river.writeBoolean(LevelRoads.roads[(int)b2].isLoop);
					ushort num3 = 0;
					while ((int)num3 < joints.Count)
					{
						RoadJoint roadJoint = joints[(int)num3];
						river.writeSingleVector3(roadJoint.vertex);
						river.writeSingleVector3(roadJoint.getTangent(0));
						river.writeSingleVector3(roadJoint.getTangent(1));
						river.writeByte((byte)roadJoint.mode);
						river.writeSingle(roadJoint.offset);
						river.writeBoolean(roadJoint.ignoreTerrain);
						num3 += 1;
					}
				}
				b2 += 1;
			}
			river.closeRiver();
		}

		private static void buildMeshes()
		{
			for (int i = 0; i < LevelRoads.roads.Count; i++)
			{
				LevelRoads.roads[i].buildMesh();
			}
		}

		private static void handleLandscapeLoaded()
		{
			if (Level.isEditor)
			{
				LevelRoads.bakeRoads();
			}
			else
			{
				LevelRoads.buildMeshes();
			}
		}

		public static readonly byte SAVEDATA_ROADS_VERSION = 2;

		public static readonly byte SAVEDATA_PATHS_VERSION = 5;

		private static Transform _models;

		private static RoadMaterial[] _materials;

		private static List<Road> roads;

		private static bool isListeningForLandscape;

		[CompilerGenerated]
		private static LandscapeLoadedHandler <>f__mg$cache0;
	}
}
