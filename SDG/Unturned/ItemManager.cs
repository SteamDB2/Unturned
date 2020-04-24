using System;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	public class ItemManager : SteamCaller
	{
		public static ItemManager instance
		{
			get
			{
				return ItemManager.manager;
			}
		}

		public static ItemRegion[,] regions { get; private set; }

		public static void getItemsInRadius(Vector3 center, float sqrRadius, List<RegionCoordinate> search, List<InteractableItem> result)
		{
			if (ItemManager.regions == null)
			{
				return;
			}
			for (int i = 0; i < search.Count; i++)
			{
				RegionCoordinate regionCoordinate = search[i];
				if (ItemManager.regions[(int)regionCoordinate.x, (int)regionCoordinate.y] != null)
				{
					for (int j = 0; j < ItemManager.regions[(int)regionCoordinate.x, (int)regionCoordinate.y].drops.Count; j++)
					{
						ItemDrop itemDrop = ItemManager.regions[(int)regionCoordinate.x, (int)regionCoordinate.y].drops[j];
						if ((itemDrop.model.position - center).sqrMagnitude < sqrRadius)
						{
							result.Add(itemDrop.interactableItem);
						}
					}
				}
			}
		}

		public static void takeItem(Transform item, byte to_x, byte to_y, byte to_rot, byte to_page)
		{
			byte b;
			byte b2;
			if (Regions.tryGetCoordinate(item.position, out b, out b2))
			{
				ItemRegion itemRegion = ItemManager.regions[(int)b, (int)b2];
				for (int i = 0; i < itemRegion.drops.Count; i++)
				{
					if (itemRegion.drops[i].model == item)
					{
						ItemManager.manager.channel.send("askTakeItem", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[]
						{
							b,
							b2,
							itemRegion.drops[i].instanceID,
							to_x,
							to_y,
							to_rot,
							to_page
						});
						return;
					}
				}
			}
		}

		public static void dropItem(Item item, Vector3 point, bool playEffect, bool isDropped, bool wideSpread)
		{
			if (ItemManager.regions == null || ItemManager.manager == null)
			{
				return;
			}
			if (wideSpread)
			{
				point.x += Random.Range(-0.75f, 0.75f);
				point.z += Random.Range(-0.75f, 0.75f);
			}
			else
			{
				point.x += Random.Range(-0.125f, 0.125f);
				point.z += Random.Range(-0.125f, 0.125f);
			}
			byte b;
			byte b2;
			if (Regions.tryGetCoordinate(point, out b, out b2))
			{
				ItemAsset itemAsset = (ItemAsset)Assets.find(EAssetType.ITEM, item.id);
				if (itemAsset != null && !itemAsset.isPro)
				{
					if (playEffect)
					{
						EffectManager.sendEffect(6, EffectManager.SMALL, point);
					}
					if (point.y > 0f)
					{
						RaycastHit raycastHit;
						Physics.Raycast(point + Vector3.up, Vector3.down, ref raycastHit, Mathf.Min(point.y + 1f, Level.HEIGHT), RayMasks.BLOCK_ITEM);
						if (raycastHit.collider != null)
						{
							point.y = raycastHit.point.y;
						}
					}
					ItemData itemData = new ItemData(item, ItemManager.instanceCount += 1u, point, isDropped);
					ItemManager.regions[(int)b, (int)b2].items.Add(itemData);
					ItemManager.manager.channel.send("tellItem", ESteamCall.CLIENTS, b, b2, ItemManager.ITEM_REGIONS, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
					{
						b,
						b2,
						item.id,
						item.amount,
						item.quality,
						item.state,
						point,
						itemData.instanceID
					});
				}
			}
		}

		[SteamCall]
		public void tellTakeItem(CSteamID steamID, byte x, byte y, uint instanceID)
		{
			if (base.channel.checkServer(steamID))
			{
				if (!Provider.isServer && !ItemManager.regions[(int)x, (int)y].isNetworked)
				{
					return;
				}
				ItemRegion itemRegion = ItemManager.regions[(int)x, (int)y];
				ushort num = 0;
				while ((int)num < itemRegion.drops.Count)
				{
					if (itemRegion.drops[(int)num].instanceID == instanceID)
					{
						if (ItemManager.onItemDropRemoved != null)
						{
							ItemManager.onItemDropRemoved(itemRegion.drops[(int)num].model, itemRegion.drops[(int)num].interactableItem);
						}
						Object.Destroy(itemRegion.drops[(int)num].model.gameObject);
						itemRegion.drops.RemoveAt((int)num);
						return;
					}
					num += 1;
				}
			}
		}

		[SteamCall]
		public void askTakeItem(CSteamID steamID, byte x, byte y, uint instanceID, byte to_x, byte to_y, byte to_rot, byte to_page)
		{
			if (Provider.isServer)
			{
				if (!Regions.checkSafe((int)x, (int)y))
				{
					return;
				}
				Player player = PlayerTool.getPlayer(steamID);
				if (player == null)
				{
					return;
				}
				if (player.life.isDead)
				{
					return;
				}
				if (player.animator.gesture == EPlayerGesture.ARREST_START)
				{
					return;
				}
				ItemRegion itemRegion = ItemManager.regions[(int)x, (int)y];
				ushort num = 0;
				while ((int)num < itemRegion.items.Count)
				{
					if (itemRegion.items[(int)num].instanceID == instanceID)
					{
						if (Dedicator.isDedicated && (itemRegion.items[(int)num].point - player.transform.position).sqrMagnitude > 400f)
						{
							return;
						}
						bool flag;
						if (to_page == 255)
						{
							flag = player.inventory.tryAddItem(ItemManager.regions[(int)x, (int)y].items[(int)num].item, true);
						}
						else
						{
							flag = player.inventory.tryAddItem(ItemManager.regions[(int)x, (int)y].items[(int)num].item, to_x, to_y, to_page, to_rot);
						}
						if (flag)
						{
							if (!player.equipment.wasTryingToSelect && !player.equipment.isSelected)
							{
								player.animator.sendGesture(EPlayerGesture.PICKUP, true);
							}
							EffectManager.sendEffect(7, EffectManager.SMALL, ItemManager.regions[(int)x, (int)y].items[(int)num].point);
							ItemManager.regions[(int)x, (int)y].items.RemoveAt((int)num);
							player.sendStat(EPlayerStat.FOUND_ITEMS);
							base.channel.send("tellTakeItem", ESteamCall.CLIENTS, x, y, ItemManager.ITEM_REGIONS, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
							{
								x,
								y,
								instanceID
							});
						}
						else
						{
							player.sendMessage(EPlayerMessage.SPACE);
						}
						return;
					}
					else
					{
						num += 1;
					}
				}
			}
		}

		[SteamCall]
		public void tellClearRegionItems(CSteamID steamID, byte x, byte y)
		{
			if (base.channel.checkServer(steamID))
			{
				if (!Provider.isServer && !ItemManager.regions[(int)x, (int)y].isNetworked)
				{
					return;
				}
				ItemRegion itemRegion = ItemManager.regions[(int)x, (int)y];
				itemRegion.destroy();
			}
		}

		public static void askClearRegionItems(byte x, byte y)
		{
			if (Provider.isServer)
			{
				if (!Regions.checkSafe((int)x, (int)y))
				{
					return;
				}
				ItemRegion itemRegion = ItemManager.regions[(int)x, (int)y];
				if (itemRegion.items.Count > 0)
				{
					itemRegion.items.Clear();
					ItemManager.manager.channel.send("tellClearRegionItems", ESteamCall.CLIENTS, x, y, ItemManager.ITEM_REGIONS, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
					{
						x,
						y
					});
				}
			}
		}

		public static void askClearAllItems()
		{
			if (Provider.isServer)
			{
				for (byte b = 0; b < Regions.WORLD_SIZE; b += 1)
				{
					for (byte b2 = 0; b2 < Regions.WORLD_SIZE; b2 += 1)
					{
						ItemManager.askClearRegionItems(b, b2);
					}
				}
			}
		}

		private void spawnItem(byte x, byte y, ushort id, byte amount, byte quality, byte[] state, Vector3 point, uint instanceID)
		{
			ItemAsset itemAsset = (ItemAsset)Assets.find(EAssetType.ITEM, id);
			if (itemAsset != null)
			{
				Transform transform = new GameObject().transform;
				transform.name = id.ToString();
				transform.transform.parent = LevelItems.models;
				transform.transform.position = point;
				Transform item = ItemTool.getItem(id, 0, quality, state, false, itemAsset);
				item.parent = transform;
				InteractableItem interactableItem = item.gameObject.AddComponent<InteractableItem>();
				interactableItem.item = new Item(id, amount, quality, state);
				interactableItem.asset = itemAsset;
				item.position = point + Vector3.up * 0.75f;
				item.rotation = Quaternion.Euler((float)(-90 + Random.Range(-15, 15)), (float)Random.Range(0, 360), (float)Random.Range(-15, 15));
				item.gameObject.AddComponent<Rigidbody>();
				item.GetComponent<Rigidbody>().interpolation = 1;
				item.GetComponent<Rigidbody>().collisionDetectionMode = 0;
				item.GetComponent<Rigidbody>().drag = 0.5f;
				item.GetComponent<Rigidbody>().angularDrag = 0.1f;
				if (LevelObjects.loads[(int)x, (int)y] != -1)
				{
					item.GetComponent<Rigidbody>().useGravity = false;
					item.GetComponent<Rigidbody>().isKinematic = true;
				}
				ItemDrop item2 = new ItemDrop(transform, interactableItem, instanceID);
				ItemManager.regions[(int)x, (int)y].drops.Add(item2);
				if (ItemManager.onItemDropAdded != null)
				{
					ItemManager.onItemDropAdded(item, interactableItem);
				}
			}
		}

		[SteamCall]
		public void tellItem(CSteamID steamID, byte x, byte y, ushort id, byte amount, byte quality, byte[] state, Vector3 point, uint instanceID)
		{
			if (base.channel.checkServer(steamID))
			{
				if (!Regions.checkSafe((int)x, (int)y))
				{
					return;
				}
				if (!ItemManager.regions[(int)x, (int)y].isNetworked)
				{
					return;
				}
				this.spawnItem(x, y, id, amount, quality, state, point, instanceID);
			}
		}

		[SteamCall]
		public void tellItems(CSteamID steamID)
		{
			if (base.channel.checkServer(steamID))
			{
				byte b = (byte)base.channel.read(Types.BYTE_TYPE);
				byte b2 = (byte)base.channel.read(Types.BYTE_TYPE);
				if (!Regions.checkSafe((int)b, (int)b2))
				{
					return;
				}
				if ((byte)base.channel.read(Types.BYTE_TYPE) == 0)
				{
					if (ItemManager.regions[(int)b, (int)b2].isNetworked)
					{
						return;
					}
				}
				else if (!ItemManager.regions[(int)b, (int)b2].isNetworked)
				{
					return;
				}
				ItemManager.regions[(int)b, (int)b2].isNetworked = true;
				ushort num = (ushort)base.channel.read(Types.UINT16_TYPE);
				for (int i = 0; i < (int)num; i++)
				{
					object[] array = base.channel.read(Types.UINT16_TYPE, Types.BYTE_TYPE, Types.BYTE_TYPE, Types.BYTE_ARRAY_TYPE, Types.VECTOR3_TYPE, Types.UINT32_TYPE);
					this.spawnItem(b, b2, (ushort)array[0], (byte)array[1], (byte)array[2], (byte[])array[3], (Vector3)array[4], (uint)array[5]);
				}
			}
		}

		public void askItems(CSteamID steamID, byte x, byte y)
		{
			if (ItemManager.regions[(int)x, (int)y].items.Count > 0)
			{
				byte b = 0;
				int i = 0;
				int j = 0;
				while (i < ItemManager.regions[(int)x, (int)y].items.Count)
				{
					int num = 0;
					while (j < ItemManager.regions[(int)x, (int)y].items.Count)
					{
						num += 4 + ItemManager.regions[(int)x, (int)y].items[j].item.state.Length + 12 + 4;
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
						ItemData itemData = ItemManager.regions[(int)x, (int)y].items[i];
						base.channel.write(itemData.item.id, itemData.item.amount, itemData.item.quality, itemData.item.state, itemData.point, itemData.instanceID);
						i++;
					}
					base.channel.closeWrite("tellItems", steamID, ESteamPacket.UPDATE_RELIABLE_CHUNK_BUFFER);
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
				base.channel.closeWrite("tellItems", steamID, ESteamPacket.UPDATE_RELIABLE_CHUNK_BUFFER);
			}
		}

		private bool despawnItems()
		{
			if (Level.info == null || Level.info.type == ELevelType.ARENA)
			{
				return false;
			}
			if (ItemManager.regions[(int)ItemManager.despawnItems_X, (int)ItemManager.despawnItems_Y].items.Count > 0)
			{
				for (int i = 0; i < ItemManager.regions[(int)ItemManager.despawnItems_X, (int)ItemManager.despawnItems_Y].items.Count; i++)
				{
					if (Time.realtimeSinceStartup - ItemManager.regions[(int)ItemManager.despawnItems_X, (int)ItemManager.despawnItems_Y].items[i].lastDropped > ((!ItemManager.regions[(int)ItemManager.despawnItems_X, (int)ItemManager.despawnItems_Y].items[i].isDropped) ? Provider.modeConfigData.Items.Despawn_Natural_Time : Provider.modeConfigData.Items.Despawn_Dropped_Time))
					{
						uint instanceID = ItemManager.regions[(int)ItemManager.despawnItems_X, (int)ItemManager.despawnItems_Y].items[i].instanceID;
						ItemManager.regions[(int)ItemManager.despawnItems_X, (int)ItemManager.despawnItems_Y].items.RemoveAt(i);
						base.channel.send("tellTakeItem", ESteamCall.CLIENTS, ItemManager.despawnItems_X, ItemManager.despawnItems_Y, ItemManager.ITEM_REGIONS, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
						{
							ItemManager.despawnItems_X,
							ItemManager.despawnItems_Y,
							instanceID
						});
					}
				}
				return false;
			}
			return true;
		}

		private bool respawnItems()
		{
			if (Level.info == null || Level.info.type == ELevelType.ARENA)
			{
				return false;
			}
			if (LevelItems.spawns[(int)ItemManager.respawnItems_X, (int)ItemManager.respawnItems_Y].Count > 0)
			{
				if (Time.realtimeSinceStartup - ItemManager.regions[(int)ItemManager.respawnItems_X, (int)ItemManager.respawnItems_Y].lastRespawn > Provider.modeConfigData.Items.Respawn_Time && (float)ItemManager.regions[(int)ItemManager.respawnItems_X, (int)ItemManager.respawnItems_Y].items.Count < (float)LevelItems.spawns[(int)ItemManager.respawnItems_X, (int)ItemManager.respawnItems_Y].Count * Provider.modeConfigData.Items.Spawn_Chance)
				{
					ItemManager.regions[(int)ItemManager.respawnItems_X, (int)ItemManager.respawnItems_Y].lastRespawn = Time.realtimeSinceStartup;
					ItemSpawnpoint itemSpawnpoint = LevelItems.spawns[(int)ItemManager.respawnItems_X, (int)ItemManager.respawnItems_Y][Random.Range(0, LevelItems.spawns[(int)ItemManager.respawnItems_X, (int)ItemManager.respawnItems_Y].Count)];
					if (!SafezoneManager.checkPointValid(itemSpawnpoint.point))
					{
						return false;
					}
					ushort num = 0;
					while ((int)num < ItemManager.regions[(int)ItemManager.respawnItems_X, (int)ItemManager.respawnItems_Y].items.Count)
					{
						if ((ItemManager.regions[(int)ItemManager.respawnItems_X, (int)ItemManager.respawnItems_Y].items[(int)num].point - itemSpawnpoint.point).sqrMagnitude < 4f)
						{
							return false;
						}
						num += 1;
					}
					ushort item = LevelItems.getItem(itemSpawnpoint);
					ItemAsset itemAsset = (ItemAsset)Assets.find(EAssetType.ITEM, item);
					if (itemAsset != null)
					{
						Item item2 = new Item(item, EItemOrigin.WORLD);
						ItemData itemData = new ItemData(item2, ItemManager.instanceCount += 1u, itemSpawnpoint.point, false);
						ItemManager.regions[(int)ItemManager.respawnItems_X, (int)ItemManager.respawnItems_Y].items.Add(itemData);
						ItemManager.manager.channel.send("tellItem", ESteamCall.CLIENTS, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
						{
							ItemManager.respawnItems_X,
							ItemManager.respawnItems_Y,
							item2.id,
							item2.quality,
							item2.state,
							itemSpawnpoint.point,
							itemData.instanceID
						});
					}
					else
					{
						CommandWindow.LogError(string.Concat(new object[]
						{
							"Failed to respawn an item with ID ",
							item,
							" from type ",
							itemSpawnpoint.type,
							"!"
						}));
					}
				}
				return false;
			}
			return true;
		}

		private void generateItems(byte x, byte y)
		{
			if (Level.info == null || Level.info.type == ELevelType.ARENA)
			{
				return;
			}
			List<ItemData> list = new List<ItemData>();
			if (LevelItems.spawns[(int)x, (int)y].Count > 0)
			{
				List<ItemSpawnpoint> list2 = new List<ItemSpawnpoint>();
				for (int i = 0; i < LevelItems.spawns[(int)x, (int)y].Count; i++)
				{
					ItemSpawnpoint itemSpawnpoint = LevelItems.spawns[(int)x, (int)y][i];
					if (SafezoneManager.checkPointValid(itemSpawnpoint.point))
					{
						list2.Add(itemSpawnpoint);
					}
				}
				while ((float)list.Count < (float)LevelItems.spawns[(int)x, (int)y].Count * Provider.modeConfigData.Items.Spawn_Chance && list2.Count > 0)
				{
					int index = Random.Range(0, list2.Count);
					ItemSpawnpoint itemSpawnpoint2 = list2[index];
					list2.RemoveAt(index);
					ushort item = LevelItems.getItem(itemSpawnpoint2);
					ItemAsset itemAsset = (ItemAsset)Assets.find(EAssetType.ITEM, item);
					if (itemAsset != null)
					{
						Item newItem = new Item(item, EItemOrigin.WORLD);
						list.Add(new ItemData(newItem, ItemManager.instanceCount += 1u, itemSpawnpoint2.point, false));
					}
					else
					{
						CommandWindow.LogError(string.Concat(new object[]
						{
							"Failed to generate an item with ID ",
							item,
							" from type ",
							itemSpawnpoint2.type,
							"!"
						}));
					}
				}
			}
			for (int j = 0; j < ItemManager.regions[(int)x, (int)y].items.Count; j++)
			{
				if (ItemManager.regions[(int)x, (int)y].items[j].isDropped)
				{
					list.Add(ItemManager.regions[(int)x, (int)y].items[j]);
				}
			}
			ItemManager.regions[(int)x, (int)y].items = list;
		}

		private void onLevelLoaded(int level)
		{
			if (level > Level.SETUP)
			{
				ItemManager.regions = new ItemRegion[(int)Regions.WORLD_SIZE, (int)Regions.WORLD_SIZE];
				for (byte b = 0; b < Regions.WORLD_SIZE; b += 1)
				{
					for (byte b2 = 0; b2 < Regions.WORLD_SIZE; b2 += 1)
					{
						ItemManager.regions[(int)b, (int)b2] = new ItemRegion();
					}
				}
				ItemManager.clampedItems = new List<InteractableItem>();
				ItemManager.instanceCount = 0u;
				ItemManager.clampItemIndex = 0;
				ItemManager.despawnItems_X = 0;
				ItemManager.despawnItems_Y = 0;
				ItemManager.respawnItems_X = 0;
				ItemManager.respawnItems_Y = 0;
				if (Dedicator.isDedicated)
				{
					for (byte b3 = 0; b3 < Regions.WORLD_SIZE; b3 += 1)
					{
						for (byte b4 = 0; b4 < Regions.WORLD_SIZE; b4 += 1)
						{
							this.generateItems(b3, b4);
						}
					}
				}
			}
		}

		private void onRegionActivated(byte x, byte y)
		{
			if (ItemManager.regions != null && ItemManager.regions[(int)x, (int)y] != null)
			{
				for (int i = 0; i < ItemManager.regions[(int)x, (int)y].drops.Count; i++)
				{
					ItemDrop itemDrop = ItemManager.regions[(int)x, (int)y].drops[i];
					if (itemDrop != null && !(itemDrop.interactableItem == null))
					{
						Rigidbody component = itemDrop.interactableItem.GetComponent<Rigidbody>();
						if (!(component == null))
						{
							component.useGravity = true;
							component.isKinematic = false;
						}
					}
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
						if (player.channel.isOwner && ItemManager.regions[(int)b, (int)b2].isNetworked && !Regions.checkArea(b, b2, new_x, new_y, ItemManager.ITEM_REGIONS))
						{
							ItemManager.regions[(int)b, (int)b2].destroy();
							ItemManager.regions[(int)b, (int)b2].isNetworked = false;
						}
						if (Provider.isServer && player.movement.loadedRegions[(int)b, (int)b2].isItemsLoaded && !Regions.checkArea(b, b2, new_x, new_y, ItemManager.ITEM_REGIONS))
						{
							player.movement.loadedRegions[(int)b, (int)b2].isItemsLoaded = false;
						}
					}
				}
			}
			if (step == 5 && Provider.isServer && Regions.checkSafe((int)new_x, (int)new_y))
			{
				for (int i = (int)(new_x - ItemManager.ITEM_REGIONS); i <= (int)(new_x + ItemManager.ITEM_REGIONS); i++)
				{
					for (int j = (int)(new_y - ItemManager.ITEM_REGIONS); j <= (int)(new_y + ItemManager.ITEM_REGIONS); j++)
					{
						if (Regions.checkSafe((int)((byte)i), (int)((byte)j)) && !player.movement.loadedRegions[i, j].isItemsLoaded)
						{
							if (player.channel.isOwner)
							{
								this.generateItems((byte)i, (byte)j);
							}
							player.movement.loadedRegions[i, j].isItemsLoaded = true;
							this.askItems(player.channel.owner.playerID.steamID, (byte)i, (byte)j);
						}
					}
				}
			}
		}

		private void onPlayerCreated(Player player)
		{
			PlayerMovement movement = player.movement;
			movement.onRegionUpdated = (PlayerRegionUpdated)Delegate.Combine(movement.onRegionUpdated, new PlayerRegionUpdated(this.onRegionUpdated));
		}

		private void Update()
		{
			if (!Provider.isServer && ItemManager.clampedItems != null && ItemManager.clampedItems.Count > 0)
			{
				if (ItemManager.clampItemIndex >= ItemManager.clampedItems.Count)
				{
					ItemManager.clampItemIndex = 0;
				}
				InteractableItem interactableItem = ItemManager.clampedItems[ItemManager.clampItemIndex];
				if (interactableItem != null)
				{
					interactableItem.clampRange();
				}
				ItemManager.clampItemIndex++;
			}
			if (!Dedicator.isDedicated || !Level.isLoaded)
			{
				return;
			}
			bool flag = true;
			while (flag)
			{
				flag = this.despawnItems();
				ItemManager.despawnItems_X += 1;
				if (ItemManager.despawnItems_X >= Regions.WORLD_SIZE)
				{
					ItemManager.despawnItems_X = 0;
					ItemManager.despawnItems_Y += 1;
					if (ItemManager.despawnItems_Y >= Regions.WORLD_SIZE)
					{
						ItemManager.despawnItems_Y = 0;
						flag = false;
					}
				}
			}
			bool flag2 = true;
			while (flag2)
			{
				flag2 = this.respawnItems();
				ItemManager.respawnItems_X += 1;
				if (ItemManager.respawnItems_X >= Regions.WORLD_SIZE)
				{
					ItemManager.respawnItems_X = 0;
					ItemManager.respawnItems_Y += 1;
					if (ItemManager.respawnItems_Y >= Regions.WORLD_SIZE)
					{
						ItemManager.respawnItems_Y = 0;
						flag2 = false;
					}
				}
			}
		}

		private void Start()
		{
			ItemManager.manager = this;
			Level.onLevelLoaded = (LevelLoaded)Delegate.Combine(Level.onLevelLoaded, new LevelLoaded(this.onLevelLoaded));
			LevelObjects.onRegionActivated = (RegionActivated)Delegate.Combine(LevelObjects.onRegionActivated, new RegionActivated(this.onRegionActivated));
			Player.onPlayerCreated = (PlayerCreated)Delegate.Combine(Player.onPlayerCreated, new PlayerCreated(this.onPlayerCreated));
		}

		public static readonly byte ITEM_REGIONS = 1;

		public static ItemDropAdded onItemDropAdded;

		public static ItemDropRemoved onItemDropRemoved;

		private static ItemManager manager;

		public static List<InteractableItem> clampedItems;

		private static uint instanceCount;

		private static int clampItemIndex;

		private static byte despawnItems_X;

		private static byte despawnItems_Y;

		private static byte respawnItems_X;

		private static byte respawnItems_Y;
	}
}
