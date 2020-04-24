using System;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	public class StructureManager : SteamCaller
	{
		public static StructureManager instance
		{
			get
			{
				return StructureManager.manager;
			}
		}

		public static StructureRegion[,] regions { get; private set; }

		public static void getStructuresInRadius(Vector3 center, float sqrRadius, List<RegionCoordinate> search, List<Transform> result)
		{
			if (StructureManager.regions == null)
			{
				return;
			}
			for (int i = 0; i < search.Count; i++)
			{
				RegionCoordinate regionCoordinate = search[i];
				if (StructureManager.regions[(int)regionCoordinate.x, (int)regionCoordinate.y] != null)
				{
					for (int j = 0; j < StructureManager.regions[(int)regionCoordinate.x, (int)regionCoordinate.y].drops.Count; j++)
					{
						Transform model = StructureManager.regions[(int)regionCoordinate.x, (int)regionCoordinate.y].drops[j].model;
						if ((model.position - center).sqrMagnitude < sqrRadius)
						{
							result.Add(model);
						}
					}
				}
			}
		}

		public static void transformStructure(Transform structure, Vector3 point, float angle_x, float angle_y, float angle_z)
		{
			angle_x = (float)(Mathf.RoundToInt(angle_x / 2f) * 2);
			angle_y = (float)(Mathf.RoundToInt(angle_y / 2f) * 2);
			angle_z = (float)(Mathf.RoundToInt(angle_z / 2f) * 2);
			byte b;
			byte b2;
			ushort num;
			StructureRegion structureRegion;
			StructureDrop structureDrop;
			if (StructureManager.tryGetInfo(structure, out b, out b2, out num, out structureRegion, out structureDrop))
			{
				StructureManager.manager.channel.send("askTransformStructure", ESteamCall.SERVER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
				{
					b,
					b2,
					structureDrop.instanceID,
					point,
					MeasurementTool.angleToByte(angle_x),
					MeasurementTool.angleToByte(angle_y),
					MeasurementTool.angleToByte(angle_z)
				});
			}
		}

		[SteamCall]
		public void tellTransformStructure(CSteamID steamID, byte x, byte y, uint instanceID, Vector3 point, byte angle_x, byte angle_y, byte angle_z)
		{
			StructureRegion structureRegion;
			if (base.channel.checkServer(steamID) && StructureManager.tryGetRegion(x, y, out structureRegion))
			{
				if (!Provider.isServer && !structureRegion.isNetworked)
				{
					return;
				}
				StructureData structureData = null;
				StructureDrop structureDrop = null;
				ushort num = 0;
				while ((int)num < structureRegion.drops.Count)
				{
					if (structureRegion.drops[(int)num].instanceID == instanceID)
					{
						if (Provider.isServer)
						{
							structureData = structureRegion.structures[(int)num];
						}
						structureDrop = structureRegion.drops[(int)num];
						break;
					}
					num += 1;
				}
				if (structureDrop == null)
				{
					return;
				}
				structureDrop.model.position = point;
				structureDrop.model.rotation = Quaternion.Euler((float)(angle_x * 2), (float)(angle_y * 2), (float)(angle_z * 2));
				byte b;
				byte b2;
				if (Regions.tryGetCoordinate(point, out b, out b2) && (x != b || y != b2))
				{
					StructureRegion structureRegion2 = StructureManager.regions[(int)b, (int)b2];
					structureRegion.drops.RemoveAt((int)num);
					if (structureRegion2.isNetworked || Provider.isServer)
					{
						structureRegion2.drops.Add(structureDrop);
					}
					else if (!Provider.isServer)
					{
						Object.Destroy(structureDrop.model.gameObject);
					}
					if (Provider.isServer)
					{
						structureRegion.structures.RemoveAt((int)num);
						structureRegion2.structures.Add(structureData);
					}
				}
				if (Provider.isServer)
				{
					structureData.point = point;
					structureData.angle_x = angle_x;
					structureData.angle_y = angle_y;
					structureData.angle_z = angle_z;
				}
			}
		}

		[SteamCall]
		public void askTransformStructure(CSteamID steamID, byte x, byte y, uint instanceID, Vector3 point, byte angle_x, byte angle_y, byte angle_z)
		{
			StructureRegion structureRegion;
			if (Provider.isServer && StructureManager.tryGetRegion(x, y, out structureRegion))
			{
				Player player = PlayerTool.getPlayer(steamID);
				if (player == null)
				{
					return;
				}
				if (player.life.isDead)
				{
					return;
				}
				if (!player.channel.owner.isAdmin)
				{
					return;
				}
				StructureManager.manager.channel.send("tellTransformStructure", ESteamCall.ALL, x, y, StructureManager.STRUCTURE_REGIONS, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
				{
					x,
					y,
					instanceID,
					point,
					angle_x,
					angle_y,
					angle_z
				});
			}
		}

		[SteamCall]
		public void tellStructureHealth(CSteamID steamID, byte x, byte y, ushort index, byte hp)
		{
			StructureRegion structureRegion;
			if (base.channel.checkServer(steamID) && StructureManager.tryGetRegion(x, y, out structureRegion))
			{
				if (!Provider.isServer && !structureRegion.isNetworked)
				{
					return;
				}
				if ((int)index >= structureRegion.drops.Count)
				{
					return;
				}
				Interactable2HP component = structureRegion.drops[(int)index].model.GetComponent<Interactable2HP>();
				if (component != null)
				{
					component.hp = hp;
				}
			}
		}

		public static void salvageStructure(Transform structure)
		{
			byte b;
			byte b2;
			ushort num;
			StructureRegion structureRegion;
			if (StructureManager.tryGetInfo(structure, out b, out b2, out num, out structureRegion))
			{
				StructureManager.manager.channel.send("askSalvageStructure", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[]
				{
					b,
					b2,
					num
				});
			}
		}

		[SteamCall]
		public void askSalvageStructure(CSteamID steamID, byte x, byte y, ushort index)
		{
			StructureRegion structureRegion;
			if (Provider.isServer && StructureManager.tryGetRegion(x, y, out structureRegion))
			{
				Player player = PlayerTool.getPlayer(steamID);
				if (player == null)
				{
					return;
				}
				if (player.life.isDead)
				{
					return;
				}
				if ((int)index >= structureRegion.drops.Count)
				{
					return;
				}
				if (!OwnershipTool.checkToggle(player.channel.owner.playerID.steamID, structureRegion.structures[(int)index].owner, player.quests.groupID, structureRegion.structures[(int)index].group))
				{
					return;
				}
				ItemStructureAsset itemStructureAsset = (ItemStructureAsset)Assets.find(EAssetType.ITEM, structureRegion.structures[(int)index].structure.id);
				if (itemStructureAsset != null)
				{
					if (itemStructureAsset.isUnpickupable)
					{
						return;
					}
					if (structureRegion.structures[(int)index].structure.health == itemStructureAsset.health)
					{
						player.inventory.forceAddItem(new Item(structureRegion.structures[(int)index].structure.id, EItemOrigin.NATURE), true);
					}
					else if (itemStructureAsset.isSalvageable)
					{
						for (int i = 0; i < itemStructureAsset.blueprints.Count; i++)
						{
							Blueprint blueprint = itemStructureAsset.blueprints[i];
							if (blueprint.outputs.Length == 1 && blueprint.outputs[0].id == itemStructureAsset.id)
							{
								ushort id = blueprint.supplies[Random.Range(0, blueprint.supplies.Length)].id;
								player.inventory.forceAddItem(new Item(id, EItemOrigin.NATURE), true);
								break;
							}
						}
					}
				}
				structureRegion.structures.RemoveAt((int)index);
				StructureManager.manager.channel.send("tellTakeStructure", ESteamCall.ALL, x, y, StructureManager.STRUCTURE_REGIONS, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
				{
					x,
					y,
					index,
					(structureRegion.drops[(int)index].model.position - player.transform.position).normalized * 100f
				});
			}
		}

		public static void damage(Transform structure, Vector3 direction, float damage, float times, bool armor)
		{
			byte b;
			byte b2;
			ushort num;
			StructureRegion structureRegion;
			if (StructureManager.tryGetInfo(structure, out b, out b2, out num, out structureRegion) && !structureRegion.structures[(int)num].structure.isDead)
			{
				if (armor)
				{
					times *= Provider.modeConfigData.Structures.Armor_Multiplier;
				}
				ushort num2 = (ushort)(damage * times);
				structureRegion.structures[(int)num].structure.askDamage(num2);
				if (structureRegion.structures[(int)num].structure.isDead)
				{
					ItemStructureAsset itemStructureAsset = (ItemStructureAsset)Assets.find(EAssetType.ITEM, structureRegion.structures[(int)num].structure.id);
					if (itemStructureAsset != null && itemStructureAsset.explosion != 0)
					{
						EffectManager.sendEffect(itemStructureAsset.explosion, EffectManager.SMALL, structure.position + Vector3.down * StructureManager.HEIGHT);
					}
					structureRegion.structures.RemoveAt((int)num);
					StructureManager.manager.channel.send("tellTakeStructure", ESteamCall.ALL, b, b2, StructureManager.STRUCTURE_REGIONS, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
					{
						b,
						b2,
						num,
						direction * (float)num2
					});
				}
				else
				{
					for (int i = 0; i < Provider.clients.Count; i++)
					{
						if (Provider.clients[i].player != null && OwnershipTool.checkToggle(Provider.clients[i].playerID.steamID, structureRegion.structures[(int)num].owner, Provider.clients[i].player.quests.groupID, structureRegion.structures[(int)num].group) && Regions.checkArea(b, b2, Provider.clients[i].player.movement.region_x, Provider.clients[i].player.movement.region_y, StructureManager.STRUCTURE_REGIONS))
						{
							StructureManager.manager.channel.send("tellStructureHealth", Provider.clients[i].playerID.steamID, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[]
							{
								b,
								b2,
								num,
								(byte)Mathf.RoundToInt((float)structureRegion.structures[(int)num].structure.health / (float)structureRegion.structures[(int)num].structure.asset.health * 100f)
							});
						}
					}
				}
			}
		}

		public static void repair(Transform structure, float damage, float times)
		{
			byte b;
			byte b2;
			ushort num;
			StructureRegion structureRegion;
			if (StructureManager.tryGetInfo(structure, out b, out b2, out num, out structureRegion) && !structureRegion.structures[(int)num].structure.isDead && !structureRegion.structures[(int)num].structure.isRepaired)
			{
				ushort amount = (ushort)(damage * times);
				structureRegion.structures[(int)num].structure.askRepair(amount);
				for (int i = 0; i < Provider.clients.Count; i++)
				{
					if (Provider.clients[i].player != null && OwnershipTool.checkToggle(Provider.clients[i].playerID.steamID, structureRegion.structures[(int)num].owner, Provider.clients[i].player.quests.groupID, structureRegion.structures[(int)num].group) && Regions.checkArea(b, b2, Provider.clients[i].player.movement.region_x, Provider.clients[i].player.movement.region_y, StructureManager.STRUCTURE_REGIONS))
					{
						StructureManager.manager.channel.send("tellStructureHealth", Provider.clients[i].playerID.steamID, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[]
						{
							b,
							b2,
							num,
							(byte)Mathf.RoundToInt((float)structureRegion.structures[(int)num].structure.health / (float)structureRegion.structures[(int)num].structure.asset.health * 100f)
						});
					}
				}
			}
		}

		public static bool tryGetInfo(Transform structure, out byte x, out byte y, out ushort index, out StructureRegion region)
		{
			x = 0;
			y = 0;
			index = 0;
			region = null;
			if (StructureManager.tryGetRegion(structure, out x, out y, out region))
			{
				index = 0;
				while ((int)index < region.drops.Count)
				{
					if (structure == region.drops[(int)index].model)
					{
						return true;
					}
					index += 1;
				}
			}
			return false;
		}

		public static bool tryGetInfo(Transform structure, out byte x, out byte y, out ushort index, out StructureRegion region, out StructureDrop drop)
		{
			x = 0;
			y = 0;
			index = 0;
			region = null;
			drop = null;
			if (StructureManager.tryGetRegion(structure, out x, out y, out region))
			{
				index = 0;
				while ((int)index < region.drops.Count)
				{
					if (structure == region.drops[(int)index].model)
					{
						drop = region.drops[(int)index];
						return true;
					}
					index += 1;
				}
			}
			return false;
		}

		public static bool tryGetRegion(Transform structure, out byte x, out byte y, out StructureRegion region)
		{
			x = 0;
			y = 0;
			region = null;
			if (structure == null)
			{
				return false;
			}
			if (Regions.tryGetCoordinate(structure.position, out x, out y))
			{
				region = StructureManager.regions[(int)x, (int)y];
				return true;
			}
			return false;
		}

		public static bool tryGetRegion(byte x, byte y, out StructureRegion region)
		{
			region = null;
			if (Regions.checkSafe((int)x, (int)y))
			{
				region = StructureManager.regions[(int)x, (int)y];
				return true;
			}
			return false;
		}

		public static void dropStructure(Structure structure, Vector3 point, float angle_x, float angle_y, float angle_z, ulong owner, ulong group)
		{
			ItemStructureAsset itemStructureAsset = (ItemStructureAsset)Assets.find(EAssetType.ITEM, structure.id);
			if (itemStructureAsset != null)
			{
				Vector3 eulerAngles = Quaternion.Euler(-90f, angle_y, 0f).eulerAngles;
				angle_x = (float)(Mathf.RoundToInt(eulerAngles.x / 2f) * 2);
				angle_y = (float)(Mathf.RoundToInt(eulerAngles.y / 2f) * 2);
				angle_z = (float)(Mathf.RoundToInt(eulerAngles.z / 2f) * 2);
				byte b;
				byte b2;
				StructureRegion structureRegion;
				if (Regions.tryGetCoordinate(point, out b, out b2) && StructureManager.tryGetRegion(b, b2, out structureRegion))
				{
					StructureData structureData = new StructureData(structure, point, MeasurementTool.angleToByte(angle_x), MeasurementTool.angleToByte(angle_y), MeasurementTool.angleToByte(angle_z), owner, group, Provider.time);
					structureRegion.structures.Add(structureData);
					StructureManager.manager.channel.send("tellStructure", ESteamCall.ALL, b, b2, StructureManager.STRUCTURE_REGIONS, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
					{
						b,
						b2,
						structure.id,
						structureData.point,
						structureData.angle_x,
						structureData.angle_y,
						structureData.angle_z,
						owner,
						group,
						StructureManager.instanceCount += 1u
					});
				}
			}
		}

		[SteamCall]
		public void tellTakeStructure(CSteamID steamID, byte x, byte y, ushort index, Vector3 ragdoll)
		{
			StructureRegion structureRegion;
			if (base.channel.checkServer(steamID) && StructureManager.tryGetRegion(x, y, out structureRegion))
			{
				if (!Provider.isServer && !structureRegion.isNetworked)
				{
					return;
				}
				if ((int)index >= structureRegion.drops.Count)
				{
					return;
				}
				if (Dedicator.isDedicated || !GraphicsSettings.debris)
				{
					Object.Destroy(structureRegion.drops[(int)index].model.gameObject);
					structureRegion.drops[(int)index].model.position = Vector3.zero;
				}
				else
				{
					ItemStructureAsset itemStructureAsset = (ItemStructureAsset)Assets.find(EAssetType.ITEM, ushort.Parse(structureRegion.drops[(int)index].model.name));
					if (itemStructureAsset != null && itemStructureAsset.construct != EConstruct.FLOOR && itemStructureAsset.construct != EConstruct.ROOF && itemStructureAsset.construct != EConstruct.FLOOR_POLY && itemStructureAsset.construct != EConstruct.ROOF_POLY)
					{
						ragdoll.y += 8f;
						ragdoll.x += Random.Range(-16f, 16f);
						ragdoll.z += Random.Range(-16f, 16f);
						ragdoll *= 2f;
						structureRegion.drops[(int)index].model.parent = Level.effects;
						MeshCollider component = structureRegion.drops[(int)index].model.GetComponent<MeshCollider>();
						if (component != null)
						{
							component.convex = true;
						}
						structureRegion.drops[(int)index].model.tag = "Debris";
						structureRegion.drops[(int)index].model.gameObject.layer = LayerMasks.DEBRIS;
						Rigidbody rigidbody = structureRegion.drops[(int)index].model.gameObject.GetComponent<Rigidbody>();
						if (rigidbody == null)
						{
							rigidbody = structureRegion.drops[(int)index].model.gameObject.AddComponent<Rigidbody>();
						}
						rigidbody.useGravity = true;
						rigidbody.isKinematic = false;
						rigidbody.AddForce(ragdoll);
						rigidbody.drag = 0.5f;
						rigidbody.angularDrag = 0.1f;
						structureRegion.drops[(int)index].model.localScale *= 0.75f;
						Object.Destroy(structureRegion.drops[(int)index].model.gameObject, 8f);
						if (Provider.isServer)
						{
							Object.Destroy(structureRegion.drops[(int)index].model.FindChild("Nav").gameObject);
						}
						for (int i = 0; i < structureRegion.drops[(int)index].model.childCount; i++)
						{
							Transform child = structureRegion.drops[(int)index].model.GetChild(i);
							if (!(child == null))
							{
								if (child.CompareTag("Logic"))
								{
									Object.Destroy(child.gameObject);
								}
							}
						}
					}
					else
					{
						Object.Destroy(structureRegion.drops[(int)index].model.gameObject);
						structureRegion.drops[(int)index].model.position = Vector3.zero;
					}
				}
				structureRegion.drops.RemoveAt((int)index);
			}
		}

		[SteamCall]
		public void tellClearRegionStructures(CSteamID steamID, byte x, byte y)
		{
			if (base.channel.checkServer(steamID))
			{
				if (!Provider.isServer && !StructureManager.regions[(int)x, (int)y].isNetworked)
				{
					return;
				}
				StructureRegion structureRegion = StructureManager.regions[(int)x, (int)y];
				structureRegion.destroy();
			}
		}

		public static void askClearRegionStructures(byte x, byte y)
		{
			if (Provider.isServer)
			{
				if (!Regions.checkSafe((int)x, (int)y))
				{
					return;
				}
				StructureRegion structureRegion = StructureManager.regions[(int)x, (int)y];
				if (structureRegion.structures.Count > 0)
				{
					structureRegion.structures.Clear();
					StructureManager.manager.channel.send("tellClearRegionStructures", ESteamCall.ALL, x, y, StructureManager.STRUCTURE_REGIONS, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
					{
						x,
						y
					});
				}
			}
		}

		public static void askClearAllStructures()
		{
			if (Provider.isServer)
			{
				for (byte b = 0; b < Regions.WORLD_SIZE; b += 1)
				{
					for (byte b2 = 0; b2 < Regions.WORLD_SIZE; b2 += 1)
					{
						StructureManager.askClearRegionStructures(b, b2);
					}
				}
			}
		}

		private Transform spawnStructure(StructureRegion region, ushort id, Vector3 point, byte angle_x, byte angle_y, byte angle_z, byte hp, ulong owner, ulong group, uint instanceID)
		{
			if (id == 0)
			{
				return null;
			}
			ItemStructureAsset itemStructureAsset;
			try
			{
				itemStructureAsset = (ItemStructureAsset)Assets.find(EAssetType.ITEM, id);
			}
			catch
			{
				itemStructureAsset = null;
			}
			if (itemStructureAsset != null)
			{
				Transform structure = StructureTool.getStructure(id, hp, owner, group, itemStructureAsset);
				structure.parent = LevelStructures.models;
				structure.position = point;
				structure.rotation = Quaternion.Euler((float)(angle_x * 2), (float)(angle_y * 2), (float)(angle_z * 2));
				if (!Dedicator.isDedicated && (itemStructureAsset.construct == EConstruct.FLOOR || itemStructureAsset.construct == EConstruct.FLOOR_POLY))
				{
					LevelGround.bewilder(point);
				}
				region.drops.Add(new StructureDrop(structure, instanceID));
				StructureManager.structureColliders.Clear();
				structure.GetComponentsInChildren<Collider>(StructureManager.structureColliders);
				for (int i = 0; i < StructureManager.structureColliders.Count; i++)
				{
					if (StructureManager.structureColliders[i] is MeshCollider)
					{
						StructureManager.structureColliders[i].enabled = false;
					}
					if (StructureManager.structureColliders[i] is MeshCollider)
					{
						StructureManager.structureColliders[i].enabled = true;
					}
				}
				return structure;
			}
			if (!Provider.isServer)
			{
				Provider.connectionFailureInfo = ESteamConnectionFailureInfo.STRUCTURE;
				Provider.disconnect();
			}
			return null;
		}

		[SteamCall]
		public void tellStructure(CSteamID steamID, byte x, byte y, ushort id, Vector3 point, byte angle_x, byte angle_y, byte angle_z, ulong owner, ulong group, uint instanceID)
		{
			StructureRegion structureRegion;
			if (base.channel.checkServer(steamID) && StructureManager.tryGetRegion(x, y, out structureRegion))
			{
				if (!Provider.isServer && !structureRegion.isNetworked)
				{
					return;
				}
				this.spawnStructure(structureRegion, id, point, angle_x, angle_y, angle_z, 100, owner, group, instanceID);
			}
		}

		[SteamCall]
		public void tellStructures(CSteamID steamID)
		{
			if (base.channel.checkServer(steamID))
			{
				byte x = (byte)base.channel.read(Types.BYTE_TYPE);
				byte y = (byte)base.channel.read(Types.BYTE_TYPE);
				StructureRegion structureRegion;
				if (StructureManager.tryGetRegion(x, y, out structureRegion))
				{
					if ((byte)base.channel.read(Types.BYTE_TYPE) == 0)
					{
						if (structureRegion.isNetworked)
						{
							return;
						}
					}
					else if (!structureRegion.isNetworked)
					{
						return;
					}
					structureRegion.isNetworked = true;
					ushort num = (ushort)base.channel.read(Types.UINT16_TYPE);
					for (int i = 0; i < (int)num; i++)
					{
						object[] array = base.channel.read(new Type[]
						{
							Types.UINT16_TYPE,
							Types.VECTOR3_TYPE,
							Types.BYTE_TYPE,
							Types.BYTE_TYPE,
							Types.BYTE_TYPE,
							Types.UINT64_TYPE,
							Types.UINT64_TYPE,
							Types.UINT32_TYPE
						});
						ulong owner = (ulong)array[5];
						ulong group = (ulong)array[6];
						uint instanceID = (uint)array[7];
						byte hp = (byte)base.channel.read(Types.BYTE_TYPE);
						this.spawnStructure(structureRegion, (ushort)array[0], (Vector3)array[1], (byte)array[2], (byte)array[3], (byte)array[4], hp, owner, group, instanceID);
					}
					Level.isLoadingStructures = false;
				}
			}
		}

		[SteamCall]
		public void askStructures(CSteamID steamID, byte x, byte y)
		{
			StructureRegion structureRegion;
			if (StructureManager.tryGetRegion(x, y, out structureRegion))
			{
				if (structureRegion.structures.Count > 0)
				{
					byte b = 0;
					int i = 0;
					int j = 0;
					while (i < structureRegion.structures.Count)
					{
						int num = 0;
						while (j < structureRegion.structures.Count)
						{
							num += 38;
							j++;
							if (num > Block.BUFFER_SIZE / 2)
							{
								break;
							}
						}
						base.channel.openWrite();
						base.channel.write(x);
						base.channel.write(y);
						base.channel.write(b);
						base.channel.write((ushort)(j - i));
						while (i < j)
						{
							StructureData structureData = structureRegion.structures[i];
							base.channel.write(new object[]
							{
								structureData.structure.id,
								structureData.point,
								structureData.angle_x,
								structureData.angle_y,
								structureData.angle_z,
								structureData.owner,
								structureData.group,
								structureRegion.drops[i].instanceID
							});
							base.channel.write((byte)Mathf.RoundToInt((float)structureData.structure.health / (float)structureData.structure.asset.health * 100f));
							i++;
						}
						base.channel.closeWrite("tellStructures", steamID, ESteamPacket.UPDATE_RELIABLE_CHUNK_BUFFER);
						b += 1;
					}
				}
				else
				{
					base.channel.openWrite();
					base.channel.write(x);
					base.channel.write(y);
					base.channel.write(0);
					base.channel.write(0);
					base.channel.closeWrite("tellStructures", steamID, ESteamPacket.UPDATE_RELIABLE_CHUNK_BUFFER);
				}
			}
		}

		private static void updateActivity(StructureRegion region, CSteamID owner, CSteamID group)
		{
			ushort num = 0;
			while ((int)num < region.structures.Count)
			{
				StructureData structureData = region.structures[(int)num];
				if (OwnershipTool.checkToggle(owner, structureData.owner, group, structureData.group))
				{
					structureData.objActiveDate = Provider.time;
				}
				num += 1;
			}
		}

		private static void updateActivity(CSteamID owner, CSteamID group)
		{
			for (byte b = 0; b < Regions.WORLD_SIZE; b += 1)
			{
				for (byte b2 = 0; b2 < Regions.WORLD_SIZE; b2 += 1)
				{
					StructureRegion region = StructureManager.regions[(int)b, (int)b2];
					StructureManager.updateActivity(region, owner, group);
				}
			}
		}

		private void onLevelLoaded(int level)
		{
			if (level > Level.SETUP)
			{
				StructureManager.regions = new StructureRegion[(int)Regions.WORLD_SIZE, (int)Regions.WORLD_SIZE];
				for (byte b = 0; b < Regions.WORLD_SIZE; b += 1)
				{
					for (byte b2 = 0; b2 < Regions.WORLD_SIZE; b2 += 1)
					{
						StructureManager.regions[(int)b, (int)b2] = new StructureRegion();
					}
				}
				StructureManager.structureColliders = new List<Collider>();
				StructureManager.instanceCount = 0u;
				if (Provider.isServer)
				{
					StructureManager.load();
				}
			}
		}

		private void onRegionUpdated(Player player, byte old_x, byte old_y, byte new_x, byte new_y, byte step, ref bool canIncrementIndex)
		{
			if (step == 0)
			{
				for (byte b = 0; b < Regions.WORLD_SIZE; b += 1)
				{
					for (byte b2 = 0; b2 < Regions.WORLD_SIZE; b2 += 1)
					{
						if (Provider.isServer)
						{
							if (player.movement.loadedRegions[(int)b, (int)b2].isStructuresLoaded && !Regions.checkArea(b, b2, new_x, new_y, StructureManager.STRUCTURE_REGIONS))
							{
								player.movement.loadedRegions[(int)b, (int)b2].isStructuresLoaded = false;
							}
						}
						else if (player.channel.isOwner && StructureManager.regions[(int)b, (int)b2].isNetworked && !Regions.checkArea(b, b2, new_x, new_y, StructureManager.STRUCTURE_REGIONS))
						{
							StructureManager.regions[(int)b, (int)b2].destroy();
							StructureManager.regions[(int)b, (int)b2].isNetworked = false;
						}
					}
				}
			}
			if (step == 1 && Dedicator.isDedicated && Regions.checkSafe((int)new_x, (int)new_y))
			{
				for (int i = (int)(new_x - StructureManager.STRUCTURE_REGIONS); i <= (int)(new_x + StructureManager.STRUCTURE_REGIONS); i++)
				{
					for (int j = (int)(new_y - StructureManager.STRUCTURE_REGIONS); j <= (int)(new_y + StructureManager.STRUCTURE_REGIONS); j++)
					{
						if (Regions.checkSafe((int)((byte)i), (int)((byte)j)) && !player.movement.loadedRegions[i, j].isStructuresLoaded)
						{
							player.movement.loadedRegions[i, j].isStructuresLoaded = true;
							this.askStructures(player.channel.owner.playerID.steamID, (byte)i, (byte)j);
						}
					}
				}
			}
		}

		private void onPlayerCreated(Player player)
		{
			PlayerMovement movement = player.movement;
			movement.onRegionUpdated = (PlayerRegionUpdated)Delegate.Combine(movement.onRegionUpdated, new PlayerRegionUpdated(this.onRegionUpdated));
			if (Provider.isServer)
			{
				SteamPlayerID playerID = player.channel.owner.playerID;
				StructureManager.updateActivity(playerID.steamID, player.quests.groupID);
			}
		}

		private void Start()
		{
			StructureManager.manager = this;
			Level.onLevelLoaded = (LevelLoaded)Delegate.Combine(Level.onLevelLoaded, new LevelLoaded(this.onLevelLoaded));
			Player.onPlayerCreated = (PlayerCreated)Delegate.Combine(Player.onPlayerCreated, new PlayerCreated(this.onPlayerCreated));
		}

		public static void load()
		{
			bool flag = false;
			if (LevelSavedata.fileExists("/Structures.dat") && Level.info.type == ELevelType.SURVIVAL)
			{
				River river = LevelSavedata.openRiver("/Structures.dat", true);
				byte b = river.readByte();
				if (b > 3)
				{
					StructureManager.serverActiveDate = river.readUInt32();
				}
				else
				{
					StructureManager.serverActiveDate = Provider.time;
				}
				if (b > 1)
				{
					for (byte b2 = 0; b2 < Regions.WORLD_SIZE; b2 += 1)
					{
						for (byte b3 = 0; b3 < Regions.WORLD_SIZE; b3 += 1)
						{
							StructureRegion region = StructureManager.regions[(int)b2, (int)b3];
							StructureManager.loadRegion(b, river, region);
						}
					}
				}
				if (b < 6)
				{
					flag = true;
				}
			}
			else
			{
				flag = true;
			}
			if (flag && LevelObjects.buildables != null)
			{
				for (byte b4 = 0; b4 < Regions.WORLD_SIZE; b4 += 1)
				{
					for (byte b5 = 0; b5 < Regions.WORLD_SIZE; b5 += 1)
					{
						List<LevelBuildableObject> list = LevelObjects.buildables[(int)b4, (int)b5];
						if (list != null && list.Count != 0)
						{
							StructureRegion structureRegion = StructureManager.regions[(int)b4, (int)b5];
							for (int i = 0; i < list.Count; i++)
							{
								LevelBuildableObject levelBuildableObject = list[i];
								if (levelBuildableObject != null)
								{
									ItemStructureAsset itemStructureAsset = levelBuildableObject.asset as ItemStructureAsset;
									if (itemStructureAsset != null)
									{
										Vector3 eulerAngles = levelBuildableObject.rotation.eulerAngles;
										byte newAngle_X = MeasurementTool.angleToByte((float)(Mathf.RoundToInt(eulerAngles.x / 2f) * 2));
										byte newAngle_Y = MeasurementTool.angleToByte((float)(Mathf.RoundToInt(eulerAngles.y / 2f) * 2));
										byte newAngle_Z = MeasurementTool.angleToByte((float)(Mathf.RoundToInt(eulerAngles.z / 2f) * 2));
										Structure structure = new Structure(itemStructureAsset.id, itemStructureAsset.health, itemStructureAsset);
										StructureData structureData = new StructureData(structure, levelBuildableObject.point, newAngle_X, newAngle_Y, newAngle_Z, 0UL, 0UL, uint.MaxValue);
										structureRegion.structures.Add(structureData);
										StructureManager.manager.spawnStructure(structureRegion, structure.id, structureData.point, structureData.angle_x, structureData.angle_y, structureData.angle_z, (byte)Mathf.RoundToInt((float)structure.health / (float)itemStructureAsset.health * 100f), 0UL, 0UL, StructureManager.instanceCount += 1u);
									}
								}
							}
						}
					}
				}
			}
			Level.isLoadingStructures = false;
		}

		public static void save()
		{
			River river = LevelSavedata.openRiver("/Structures.dat", false);
			river.writeByte(StructureManager.SAVEDATA_VERSION);
			river.writeUInt32(Provider.time);
			for (byte b = 0; b < Regions.WORLD_SIZE; b += 1)
			{
				for (byte b2 = 0; b2 < Regions.WORLD_SIZE; b2 += 1)
				{
					StructureRegion region = StructureManager.regions[(int)b, (int)b2];
					StructureManager.saveRegion(river, region);
				}
			}
			river.closeRiver();
		}

		private static void loadRegion(byte version, River river, StructureRegion region)
		{
			ushort num = river.readUInt16();
			for (ushort num2 = 0; num2 < num; num2 += 1)
			{
				ushort num3 = river.readUInt16();
				ushort num4 = river.readUInt16();
				Vector3 vector = river.readSingleVector3();
				byte b = 0;
				if (version > 4)
				{
					b = river.readByte();
				}
				byte b2 = river.readByte();
				byte b3 = 0;
				if (version > 4)
				{
					b3 = river.readByte();
				}
				ulong num5 = 0UL;
				ulong num6 = 0UL;
				if (version > 2)
				{
					num5 = river.readUInt64();
					num6 = river.readUInt64();
				}
				uint newObjActiveDate;
				if (version > 3)
				{
					newObjActiveDate = river.readUInt32();
					if (Provider.time - StructureManager.serverActiveDate > Provider.modeConfigData.Structures.Decay_Time / 2u)
					{
						newObjActiveDate = Provider.time;
					}
				}
				else
				{
					newObjActiveDate = Provider.time;
				}
				ItemStructureAsset itemStructureAsset;
				try
				{
					itemStructureAsset = (ItemStructureAsset)Assets.find(EAssetType.ITEM, num3);
				}
				catch
				{
					itemStructureAsset = null;
				}
				if (itemStructureAsset != null)
				{
					if (version < 5)
					{
						Vector3 eulerAngles = Quaternion.Euler(-90f, (float)(b2 * 2), 0f).eulerAngles;
						b = MeasurementTool.angleToByte((float)(Mathf.RoundToInt(eulerAngles.x / 2f) * 2));
						b2 = MeasurementTool.angleToByte((float)(Mathf.RoundToInt(eulerAngles.y / 2f) * 2));
						b3 = MeasurementTool.angleToByte((float)(Mathf.RoundToInt(eulerAngles.z / 2f) * 2));
					}
					region.structures.Add(new StructureData(new Structure(num3, num4, itemStructureAsset), vector, b, b2, b3, num5, num6, newObjActiveDate));
					StructureManager.manager.spawnStructure(region, num3, vector, b, b2, b3, (byte)Mathf.RoundToInt((float)num4 / (float)itemStructureAsset.health * 100f), num5, num6, StructureManager.instanceCount += 1u);
				}
			}
		}

		private static void saveRegion(River river, StructureRegion region)
		{
			uint time = Provider.time;
			ushort num = 0;
			ushort num2 = 0;
			while ((int)num2 < region.structures.Count)
			{
				StructureData structureData = region.structures[(int)num2];
				if ((!Dedicator.isDedicated || Provider.modeConfigData.Structures.Decay_Time == 0u || time < structureData.objActiveDate || time - structureData.objActiveDate < Provider.modeConfigData.Structures.Decay_Time) && structureData.structure.asset.isSaveable)
				{
					num += 1;
				}
				num2 += 1;
			}
			river.writeUInt16(num);
			ushort num3 = 0;
			while ((int)num3 < region.structures.Count)
			{
				StructureData structureData2 = region.structures[(int)num3];
				if ((!Dedicator.isDedicated || Provider.modeConfigData.Structures.Decay_Time == 0u || time < structureData2.objActiveDate || time - structureData2.objActiveDate < Provider.modeConfigData.Structures.Decay_Time) && structureData2.structure.asset.isSaveable)
				{
					river.writeUInt16(structureData2.structure.id);
					river.writeUInt16(structureData2.structure.health);
					river.writeSingleVector3(structureData2.point);
					river.writeByte(structureData2.angle_x);
					river.writeByte(structureData2.angle_y);
					river.writeByte(structureData2.angle_z);
					river.writeUInt64(structureData2.owner);
					river.writeUInt64(structureData2.group);
					river.writeUInt32(structureData2.objActiveDate);
				}
				num3 += 1;
			}
		}

		public static readonly byte SAVEDATA_VERSION = 6;

		public static readonly byte STRUCTURE_REGIONS = 2;

		public static readonly float WALL = 3f;

		public static readonly float PILLAR = 3.1f;

		public static readonly float HEIGHT = 2.125f;

		private static StructureManager manager;

		private static List<Collider> structureColliders;

		private static uint instanceCount;

		private static uint serverActiveDate;
	}
}
