using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	public class LevelNodes
	{
		public static Transform models
		{
			get
			{
				return LevelNodes._models;
			}
		}

		public static List<Node> nodes
		{
			get
			{
				return LevelNodes._nodes;
			}
		}

		public static void setEnabled(bool isEnabled)
		{
			if (LevelNodes.nodes == null)
			{
				return;
			}
			for (int i = 0; i < LevelNodes.nodes.Count; i++)
			{
				LevelNodes.nodes[i].setEnabled(isEnabled);
			}
		}

		public static Transform addNode(Vector3 point, ENodeType type)
		{
			if (type == ENodeType.LOCATION)
			{
				LevelNodes.nodes.Add(new LocationNode(point));
			}
			else if (type == ENodeType.SAFEZONE)
			{
				LevelNodes.nodes.Add(new SafezoneNode(point));
			}
			else if (type == ENodeType.PURCHASE)
			{
				LevelNodes.nodes.Add(new PurchaseNode(point));
			}
			else if (type == ENodeType.ARENA)
			{
				LevelNodes.nodes.Add(new ArenaNode(point));
			}
			else if (type == ENodeType.DEADZONE)
			{
				LevelNodes.nodes.Add(new DeadzoneNode(point));
			}
			else if (type == ENodeType.AIRDROP)
			{
				LevelNodes.nodes.Add(new AirdropNode(point));
			}
			else if (type == ENodeType.EFFECT)
			{
				LevelNodes.nodes.Add(new EffectNode(point));
			}
			return LevelNodes.nodes[LevelNodes.nodes.Count - 1].model;
		}

		public static void removeNode(Transform select)
		{
			for (int i = 0; i < LevelNodes.nodes.Count; i++)
			{
				if (LevelNodes.nodes[i].model == select)
				{
					LevelNodes.nodes[i].remove();
					LevelNodes.nodes.RemoveAt(i);
					return;
				}
			}
		}

		public static Node getNode(Transform select)
		{
			for (int i = 0; i < LevelNodes.nodes.Count; i++)
			{
				if (LevelNodes.nodes[i].model == select)
				{
					return LevelNodes.nodes[i];
				}
			}
			return null;
		}

		public static void load()
		{
			LevelNodes._models = new GameObject().transform;
			LevelNodes.models.name = "Nodes";
			LevelNodes.models.parent = Level.level;
			LevelNodes.models.tag = "Logic";
			LevelNodes.models.gameObject.layer = LayerMasks.LOGIC;
			LevelNodes._nodes = new List<Node>();
			if (ReadWrite.fileExists(Level.info.path + "/Environment/Nodes.dat", false, false))
			{
				River river = new River(Level.info.path + "/Environment/Nodes.dat", false);
				byte b = river.readByte();
				if (b > 0)
				{
					ushort num = (ushort)river.readByte();
					for (ushort num2 = 0; num2 < num; num2 += 1)
					{
						Vector3 vector = river.readSingleVector3();
						ENodeType enodeType = (ENodeType)river.readByte();
						if (enodeType == ENodeType.LOCATION)
						{
							string newName = river.readString();
							LevelNodes.nodes.Add(new LocationNode(vector, newName));
						}
						else if (enodeType == ENodeType.SAFEZONE)
						{
							float newRadius = river.readSingle();
							bool newHeight = false;
							if (b > 1)
							{
								newHeight = river.readBoolean();
							}
							bool newNoWeapons = true;
							if (b > 4)
							{
								newNoWeapons = river.readBoolean();
							}
							bool newNoBuildables = true;
							if (b > 4)
							{
								newNoBuildables = river.readBoolean();
							}
							LevelNodes.nodes.Add(new SafezoneNode(vector, newRadius, newHeight, newNoWeapons, newNoBuildables));
						}
						else if (enodeType == ENodeType.PURCHASE)
						{
							float newRadius2 = river.readSingle();
							ushort newID = river.readUInt16();
							uint newCost = river.readUInt32();
							LevelNodes.nodes.Add(new PurchaseNode(vector, newRadius2, newID, newCost));
						}
						else if (enodeType == ENodeType.ARENA)
						{
							float newRadius3 = river.readSingle();
							LevelNodes.nodes.Add(new ArenaNode(vector, newRadius3));
						}
						else if (enodeType == ENodeType.DEADZONE)
						{
							float newRadius4 = river.readSingle();
							LevelNodes.nodes.Add(new DeadzoneNode(vector, newRadius4));
						}
						else if (enodeType == ENodeType.AIRDROP)
						{
							ushort num3 = river.readUInt16();
							byte b2;
							byte b3;
							if (SpawnTableTool.resolve(num3) == 0 && Regions.tryGetCoordinate(vector, out b2, out b3))
							{
								Assets.errors.Add(string.Concat(new object[]
								{
									Level.info.name,
									" airdrop references invalid spawn table ",
									num3,
									" at (",
									b2,
									", ",
									b3,
									")!"
								}));
							}
							LevelNodes.nodes.Add(new AirdropNode(vector, num3));
						}
						else if (enodeType == ENodeType.EFFECT)
						{
							byte newShape = 0;
							if (b > 2)
							{
								newShape = river.readByte();
							}
							float newRadius5 = river.readSingle();
							Vector3 newBounds = Vector3.one;
							if (b > 2)
							{
								newBounds = river.readSingleVector3();
							}
							ushort newID2 = river.readUInt16();
							bool newNoWater = river.readBoolean();
							bool newNoLighting = false;
							if (b > 3)
							{
								newNoLighting = river.readBoolean();
							}
							LevelNodes.nodes.Add(new EffectNode(vector, (ENodeShape)newShape, newRadius5, newBounds, newID2, newNoWater, newNoLighting));
						}
					}
				}
				river.closeRiver();
			}
		}

		public static void save()
		{
			River river = new River(Level.info.path + "/Environment/Nodes.dat", false);
			river.writeByte(LevelNodes.SAVEDATA_VERSION);
			byte b = 0;
			ushort num = 0;
			while ((int)num < LevelNodes.nodes.Count)
			{
				if (LevelNodes.nodes[(int)num].type != ENodeType.LOCATION || ((LocationNode)LevelNodes.nodes[(int)num]).name.Length > 0)
				{
					b += 1;
				}
				num += 1;
			}
			river.writeByte(b);
			byte b2 = 0;
			while ((int)b2 < LevelNodes.nodes.Count)
			{
				if (LevelNodes.nodes[(int)b2].type != ENodeType.LOCATION || ((LocationNode)LevelNodes.nodes[(int)b2]).name.Length > 0)
				{
					river.writeSingleVector3(LevelNodes.nodes[(int)b2].point);
					river.writeByte((byte)LevelNodes.nodes[(int)b2].type);
					if (LevelNodes.nodes[(int)b2].type == ENodeType.LOCATION)
					{
						river.writeString(((LocationNode)LevelNodes.nodes[(int)b2]).name);
					}
					else if (LevelNodes.nodes[(int)b2].type == ENodeType.SAFEZONE)
					{
						river.writeSingle(((SafezoneNode)LevelNodes.nodes[(int)b2]).radius);
						river.writeBoolean(((SafezoneNode)LevelNodes.nodes[(int)b2]).isHeight);
						river.writeBoolean(((SafezoneNode)LevelNodes.nodes[(int)b2]).noWeapons);
						river.writeBoolean(((SafezoneNode)LevelNodes.nodes[(int)b2]).noBuildables);
					}
					else if (LevelNodes.nodes[(int)b2].type == ENodeType.PURCHASE)
					{
						river.writeSingle(((PurchaseNode)LevelNodes.nodes[(int)b2]).radius);
						river.writeUInt16(((PurchaseNode)LevelNodes.nodes[(int)b2]).id);
						river.writeUInt32(((PurchaseNode)LevelNodes.nodes[(int)b2]).cost);
					}
					else if (LevelNodes.nodes[(int)b2].type == ENodeType.ARENA)
					{
						river.writeSingle(((ArenaNode)LevelNodes.nodes[(int)b2]).radius);
					}
					else if (LevelNodes.nodes[(int)b2].type == ENodeType.DEADZONE)
					{
						river.writeSingle(((DeadzoneNode)LevelNodes.nodes[(int)b2]).radius);
					}
					else if (LevelNodes.nodes[(int)b2].type == ENodeType.AIRDROP)
					{
						river.writeUInt16(((AirdropNode)LevelNodes.nodes[(int)b2]).id);
					}
					else if (LevelNodes.nodes[(int)b2].type == ENodeType.EFFECT)
					{
						river.writeByte((byte)((EffectNode)LevelNodes.nodes[(int)b2]).shape);
						river.writeSingle(((EffectNode)LevelNodes.nodes[(int)b2]).radius);
						river.writeSingleVector3(((EffectNode)LevelNodes.nodes[(int)b2]).bounds);
						river.writeUInt16(((EffectNode)LevelNodes.nodes[(int)b2]).id);
						river.writeBoolean(((EffectNode)LevelNodes.nodes[(int)b2]).noWater);
						river.writeBoolean(((EffectNode)LevelNodes.nodes[(int)b2]).noLighting);
					}
				}
				b2 += 1;
			}
			river.closeRiver();
		}

		public static readonly byte SAVEDATA_VERSION = 5;

		private static Transform _models;

		private static List<Node> _nodes;
	}
}
