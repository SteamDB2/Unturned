using System;
using System.Collections.Generic;
using Steamworks;

namespace SDG.Unturned
{
	public class PlayerCrafting : PlayerCaller
	{
		private bool stripAttachments(byte page, ItemJar jar)
		{
			ItemAsset itemAsset = (ItemAsset)Assets.find(EAssetType.ITEM, jar.item.id);
			if (itemAsset != null && itemAsset.type == EItemType.GUN && jar.item.state != null && jar.item.state.Length == 18)
			{
				if (((ItemGunAsset)itemAsset).hasSight)
				{
					ushort num = BitConverter.ToUInt16(jar.item.state, 0);
					if (num != 0 && num != ((ItemGunAsset)itemAsset).sightID)
					{
						base.player.inventory.forceAddItem(new Item(num, false, jar.item.state[13]), true);
						jar.item.state[0] = 0;
						jar.item.state[1] = 0;
						jar.item.state[13] = 0;
					}
				}
				if (((ItemGunAsset)itemAsset).hasTactical)
				{
					ushort num2 = BitConverter.ToUInt16(jar.item.state, 2);
					if (num2 != 0)
					{
						base.player.inventory.forceAddItem(new Item(num2, false, jar.item.state[14]), true);
						jar.item.state[2] = 0;
						jar.item.state[3] = 0;
						jar.item.state[14] = 0;
					}
				}
				if (((ItemGunAsset)itemAsset).hasGrip)
				{
					ushort num3 = BitConverter.ToUInt16(jar.item.state, 4);
					if (num3 != 0)
					{
						base.player.inventory.forceAddItem(new Item(num3, false, jar.item.state[15]), true);
						jar.item.state[4] = 0;
						jar.item.state[5] = 0;
						jar.item.state[15] = 0;
					}
				}
				if (((ItemGunAsset)itemAsset).hasBarrel)
				{
					ushort num4 = BitConverter.ToUInt16(jar.item.state, 6);
					if (num4 != 0)
					{
						base.player.inventory.forceAddItem(new Item(num4, false, jar.item.state[16]), true);
						jar.item.state[6] = 0;
						jar.item.state[7] = 0;
						jar.item.state[16] = 0;
					}
				}
				ushort num5 = BitConverter.ToUInt16(jar.item.state, 8);
				if (num5 != 0 && jar.item.state[10] > 0)
				{
					base.player.inventory.forceAddItem(new Item(num5, jar.item.state[10], jar.item.state[17]), true);
					jar.item.state[8] = 0;
					jar.item.state[9] = 0;
					jar.item.state[10] = 0;
					jar.item.state[17] = 0;
				}
				return true;
			}
			return false;
		}

		public void removeItem(byte page, ItemJar jar)
		{
			base.player.inventory.removeItem(page, base.player.inventory.getIndex(page, jar.x, jar.y));
			this.stripAttachments(page, jar);
		}

		[SteamCall]
		public void askStripAttachments(CSteamID steamID, byte page, byte x, byte y)
		{
			if (base.channel.checkOwner(steamID) && Provider.isServer)
			{
				if (page < PlayerInventory.SLOTS || page >= PlayerInventory.PAGES - 1)
				{
					return;
				}
				if (base.player.equipment.checkSelection(page, x, y))
				{
					if (base.player.equipment.isBusy)
					{
						return;
					}
					base.player.equipment.dequip();
				}
				byte index = base.player.inventory.getIndex(page, x, y);
				if (index == 255)
				{
					return;
				}
				ItemJar item = base.player.inventory.getItem(page, index);
				if (item == null)
				{
					return;
				}
				if (!this.stripAttachments(page, item))
				{
					return;
				}
				base.player.inventory.sendUpdateInvState(page, x, y, item.item.state);
			}
		}

		public void sendStripAttachments(byte page, byte x, byte y)
		{
			base.channel.send("askStripAttachments", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[]
			{
				page,
				x,
				y
			});
		}

		[SteamCall]
		public void tellCraft(CSteamID steamID)
		{
			if (base.channel.checkServer(steamID) && this.onCraftingUpdated != null)
			{
				this.onCraftingUpdated();
			}
		}

		[SteamCall]
		public void askCraft(CSteamID steamID, ushort id, byte index, bool force)
		{
			if (base.channel.checkOwner(steamID))
			{
				if (base.player.equipment.isBusy)
				{
					return;
				}
				ItemAsset itemAsset = (ItemAsset)Assets.find(EAssetType.ITEM, id);
				if (itemAsset != null)
				{
					if ((int)index >= itemAsset.blueprints.Count)
					{
						return;
					}
					Blueprint blueprint = itemAsset.blueprints[(int)index];
					if (blueprint.skill == EBlueprintSkill.REPAIR && (uint)blueprint.level > Provider.modeConfigData.Gameplay.Repair_Level_Max)
					{
						return;
					}
					if (!string.IsNullOrEmpty(blueprint.map) && !blueprint.map.Equals(Level.info.name, StringComparison.InvariantCultureIgnoreCase))
					{
						return;
					}
					if (blueprint.tool != 0 && base.player.inventory.has(blueprint.tool) == null)
					{
						return;
					}
					if (blueprint.skill != EBlueprintSkill.NONE)
					{
						bool flag = PowerTool.checkFires(base.transform.position, 16f);
						if ((blueprint.skill == EBlueprintSkill.CRAFT && base.player.skills.skills[2][1].level < blueprint.level) || (blueprint.skill == EBlueprintSkill.COOK && (!flag || base.player.skills.skills[2][3].level < blueprint.level)) || (blueprint.skill == EBlueprintSkill.REPAIR && base.player.skills.skills[2][7].level < blueprint.level))
						{
							return;
						}
					}
					bool flag2 = false;
					for (;;)
					{
						List<InventorySearch>[] array = new List<InventorySearch>[blueprint.supplies.Length];
						byte b = 0;
						while ((int)b < blueprint.supplies.Length)
						{
							BlueprintSupply blueprintSupply = blueprint.supplies[(int)b];
							List<InventorySearch> list = base.player.inventory.search(blueprintSupply.id, false, true);
							if (list.Count == 0)
							{
								return;
							}
							ushort num = 0;
							byte b2 = 0;
							while ((int)b2 < list.Count)
							{
								num += (ushort)list[(int)b2].jar.item.amount;
								b2 += 1;
							}
							if (num < blueprintSupply.amount && blueprint.type != EBlueprintType.AMMO)
							{
								return;
							}
							if (blueprint.type == EBlueprintType.AMMO)
							{
								list.Sort(PlayerCrafting.amountAscendingComparator);
							}
							else
							{
								list.Sort(PlayerCrafting.qualityAscendingComparator);
							}
							array[(int)b] = list;
							b += 1;
						}
						if (blueprint.type == EBlueprintType.REPAIR)
						{
							List<InventorySearch> list2 = base.player.inventory.search(itemAsset.id, false, false);
							byte b3 = byte.MaxValue;
							byte b4 = byte.MaxValue;
							byte b5 = 0;
							while ((int)b5 < list2.Count)
							{
								if (list2[(int)b5].jar.item.quality < b3)
								{
									b3 = list2[(int)b5].jar.item.quality;
									b4 = b5;
								}
								b5 += 1;
							}
							if (b4 == 255)
							{
								return;
							}
							InventorySearch inventorySearch = list2[(int)b4];
							if (base.player.equipment.checkSelection(inventorySearch.page, inventorySearch.jar.x, inventorySearch.jar.y))
							{
								base.player.equipment.dequip();
							}
							byte b6 = 0;
							while ((int)b6 < array.Length)
							{
								BlueprintSupply blueprintSupply2 = blueprint.supplies[(int)b6];
								List<InventorySearch> list3 = array[(int)b6];
								byte b7 = 0;
								while ((ushort)b7 < blueprintSupply2.amount)
								{
									InventorySearch inventorySearch2 = list3[(int)b7];
									if (base.player.equipment.checkSelection(inventorySearch2.page, inventorySearch2.jar.x, inventorySearch2.jar.y))
									{
										base.player.equipment.dequip();
									}
									this.removeItem(inventorySearch2.page, inventorySearch2.jar);
									if (inventorySearch2.page < PlayerInventory.SLOTS)
									{
										base.player.equipment.sendSlot(inventorySearch2.page);
									}
									b7 += 1;
								}
								b6 += 1;
							}
							base.player.inventory.sendUpdateQuality(inventorySearch.page, inventorySearch.jar.x, inventorySearch.jar.y, 100);
							if (itemAsset.type == EItemType.REFILL && inventorySearch.jar.item.state[0] == 3)
							{
								inventorySearch.jar.item.state[0] = 1;
								base.player.inventory.sendUpdateInvState(inventorySearch.page, inventorySearch.jar.x, inventorySearch.jar.y, inventorySearch.jar.item.state);
							}
							base.channel.send("tellCraft", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[0]);
						}
						else if (blueprint.type == EBlueprintType.AMMO)
						{
							List<InventorySearch> list4 = base.player.inventory.search(itemAsset.id, true, true);
							int num2 = -1;
							byte b8 = byte.MaxValue;
							byte b9 = 0;
							while ((int)b9 < list4.Count)
							{
								if ((int)list4[(int)b9].jar.item.amount > num2 && list4[(int)b9].jar.item.amount < itemAsset.amount)
								{
									num2 = (int)list4[(int)b9].jar.item.amount;
									b8 = b9;
								}
								b9 += 1;
							}
							if (b8 == 255)
							{
								return;
							}
							InventorySearch inventorySearch3 = list4[(int)b8];
							int num3 = (int)itemAsset.amount - num2;
							if (base.player.equipment.checkSelection(inventorySearch3.page, inventorySearch3.jar.x, inventorySearch3.jar.y))
							{
								base.player.equipment.dequip();
							}
							List<InventorySearch> list5 = array[0];
							byte b10 = 0;
							while ((int)b10 < list5.Count)
							{
								InventorySearch inventorySearch4 = list5[(int)b10];
								if (inventorySearch4.jar != inventorySearch3.jar)
								{
									if (base.player.equipment.checkSelection(inventorySearch4.page, inventorySearch4.jar.x, inventorySearch4.jar.y))
									{
										base.player.equipment.dequip();
									}
									if ((int)inventorySearch4.jar.item.amount > num3)
									{
										base.player.inventory.sendUpdateAmount(inventorySearch4.page, inventorySearch4.jar.x, inventorySearch4.jar.y, (byte)((int)inventorySearch4.jar.item.amount - num3));
										num3 = 0;
										break;
									}
									num3 -= (int)inventorySearch4.jar.item.amount;
									base.player.inventory.sendUpdateAmount(inventorySearch4.page, inventorySearch4.jar.x, inventorySearch4.jar.y, 0);
									if ((index == 0 && itemAsset.blueprints.Count > 1) || itemAsset.blueprints.Count == 1)
									{
										this.removeItem(inventorySearch4.page, inventorySearch4.jar);
										if (inventorySearch4.page < PlayerInventory.SLOTS)
										{
											base.player.equipment.sendSlot(inventorySearch4.page);
										}
									}
									if (num3 == 0)
									{
										break;
									}
								}
								b10 += 1;
							}
							base.player.inventory.sendUpdateAmount(inventorySearch3.page, inventorySearch3.jar.x, inventorySearch3.jar.y, (byte)((int)itemAsset.amount - num3));
							base.channel.send("tellCraft", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[0]);
						}
						else
						{
							byte b11 = 0;
							while ((int)b11 < array.Length)
							{
								BlueprintSupply blueprintSupply3 = blueprint.supplies[(int)b11];
								List<InventorySearch> list6 = array[(int)b11];
								byte b12 = 0;
								while ((ushort)b12 < blueprintSupply3.amount)
								{
									InventorySearch inventorySearch5 = list6[(int)b12];
									if (base.player.equipment.checkSelection(inventorySearch5.page, inventorySearch5.jar.x, inventorySearch5.jar.y))
									{
										base.player.equipment.dequip();
									}
									this.removeItem(inventorySearch5.page, inventorySearch5.jar);
									if (inventorySearch5.page < PlayerInventory.SLOTS)
									{
										base.player.equipment.sendSlot(inventorySearch5.page);
									}
									b12 += 1;
								}
								b11 += 1;
							}
							byte b13 = 0;
							while ((int)b13 < blueprint.outputs.Length)
							{
								BlueprintOutput blueprintOutput = blueprint.outputs[(int)b13];
								byte b14 = 0;
								while ((ushort)b14 < blueprintOutput.amount)
								{
									if (blueprint.transferState)
									{
										base.player.inventory.forceAddItem(new Item(blueprintOutput.id, array[0][0].jar.item.amount, array[0][0].jar.item.quality, array[0][0].jar.item.state), true);
									}
									else
									{
										base.player.inventory.forceAddItem(new Item(blueprintOutput.id, EItemOrigin.CRAFT), true);
									}
									b14 += 1;
								}
								b13 += 1;
							}
							base.channel.send("tellCraft", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[0]);
						}
						if (!flag2)
						{
							flag2 = true;
							base.player.sendStat(EPlayerStat.FOUND_CRAFTS);
							if (blueprint.build != 0)
							{
								EffectManager.sendEffect(blueprint.build, EffectManager.SMALL, base.transform.position);
								if (Provider.isServer)
								{
									AlertTool.alert(base.transform.position, 8f);
								}
							}
						}
						if (!force || blueprint.type == EBlueprintType.REPAIR || blueprint.type == EBlueprintType.AMMO)
						{
							return;
						}
					}
					return;
				}
			}
		}

		public void sendCraft(ushort id, byte index, bool force)
		{
			base.channel.send("askCraft", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[]
			{
				id,
				index,
				force
			});
		}

		private static InventorySearchQualityAscendingComparator qualityAscendingComparator = new InventorySearchQualityAscendingComparator();

		private static InventorySearchAmountAscendingComparator amountAscendingComparator = new InventorySearchAmountAscendingComparator();

		public CraftingUpdated onCraftingUpdated;
	}
}
