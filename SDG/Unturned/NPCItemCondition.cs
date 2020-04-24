using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	public class NPCItemCondition : INPCCondition
	{
		public NPCItemCondition(ushort newID, ushort newAmount, string newText, bool newShouldReset) : base(newText, newShouldReset)
		{
			this.id = newID;
			this.amount = newAmount;
		}

		public ushort id { get; protected set; }

		public ushort amount { get; protected set; }

		public override bool isConditionMet(Player player)
		{
			NPCItemCondition.search.Clear();
			player.inventory.search(NPCItemCondition.search, this.id, false, true);
			ushort num = 0;
			byte b = 0;
			while ((int)b < NPCItemCondition.search.Count)
			{
				num += (ushort)NPCItemCondition.search[(int)b].jar.item.amount;
				b += 1;
			}
			return num >= this.amount;
		}

		public override void applyCondition(Player player, bool shouldSend)
		{
			if (!Provider.isServer)
			{
				return;
			}
			if (!this.shouldReset)
			{
				return;
			}
			NPCItemCondition.search.Clear();
			player.inventory.search(NPCItemCondition.search, this.id, false, true);
			NPCItemCondition.search.Sort(NPCItemCondition.qualityAscendingComparator);
			ushort num = this.amount;
			byte b = 0;
			while ((int)b < NPCItemCondition.search.Count)
			{
				InventorySearch inventorySearch = NPCItemCondition.search[(int)b];
				if (player.equipment.checkSelection(inventorySearch.page, inventorySearch.jar.x, inventorySearch.jar.y))
				{
					player.equipment.dequip();
				}
				if ((ushort)inventorySearch.jar.item.amount > num)
				{
					player.inventory.sendUpdateAmount(inventorySearch.page, inventorySearch.jar.x, inventorySearch.jar.y, (byte)((ushort)inventorySearch.jar.item.amount - num));
					break;
				}
				num -= (ushort)inventorySearch.jar.item.amount;
				player.inventory.sendUpdateAmount(inventorySearch.page, inventorySearch.jar.x, inventorySearch.jar.y, 0);
				player.crafting.removeItem(inventorySearch.page, inventorySearch.jar);
				if (inventorySearch.page < PlayerInventory.SLOTS)
				{
					player.equipment.sendSlot(inventorySearch.page);
				}
				if (num == 0)
				{
					break;
				}
				b += 1;
			}
		}

		public override string formatCondition(Player player)
		{
			if (string.IsNullOrEmpty(this.text))
			{
				this.text = PlayerNPCQuestUI.localization.format("Condition_Item");
			}
			ItemAsset itemAsset = Assets.find(EAssetType.ITEM, this.id) as ItemAsset;
			string arg;
			if (itemAsset != null)
			{
				arg = string.Concat(new string[]
				{
					"<color=",
					Palette.hex(ItemTool.getRarityColorUI(itemAsset.rarity)),
					">",
					itemAsset.itemName,
					"</color>"
				});
			}
			else
			{
				arg = "?";
			}
			NPCItemCondition.search.Clear();
			player.inventory.search(NPCItemCondition.search, this.id, false, true);
			return string.Format(this.text, NPCItemCondition.search.Count, this.amount, arg);
		}

		public override Sleek createUI(Player player, Texture2D icon)
		{
			string text = this.formatCondition(player);
			if (string.IsNullOrEmpty(text))
			{
				return null;
			}
			ItemAsset itemAsset = (ItemAsset)Assets.find(EAssetType.ITEM, this.id);
			if (itemAsset == null)
			{
				return null;
			}
			SleekBox sleekBox = new SleekBox();
			if (itemAsset.size_y == 1)
			{
				sleekBox.sizeOffset_Y = (int)(itemAsset.size_y * 50 + 10);
			}
			else
			{
				sleekBox.sizeOffset_Y = (int)(itemAsset.size_y * 25 + 10);
			}
			sleekBox.sizeScale_X = 1f;
			if (icon != null)
			{
				sleekBox.add(new SleekImageTexture(icon)
				{
					positionOffset_X = 5,
					positionOffset_Y = -10,
					positionScale_Y = 0.5f,
					sizeOffset_X = 20,
					sizeOffset_Y = 20
				});
			}
			SleekImageTexture sleekImageTexture = new SleekImageTexture();
			if (icon != null)
			{
				sleekImageTexture.positionOffset_X = 30;
			}
			else
			{
				sleekImageTexture.positionOffset_X = 5;
			}
			sleekImageTexture.positionOffset_Y = 5;
			if (itemAsset.size_y == 1)
			{
				sleekImageTexture.sizeOffset_X = (int)(itemAsset.size_x * 50);
				sleekImageTexture.sizeOffset_Y = (int)(itemAsset.size_y * 50);
			}
			else
			{
				sleekImageTexture.sizeOffset_X = (int)(itemAsset.size_x * 25);
				sleekImageTexture.sizeOffset_Y = (int)(itemAsset.size_y * 25);
			}
			sleekBox.add(sleekImageTexture);
			ItemTool.getIcon(this.id, 100, itemAsset.getState(false), itemAsset, sleekImageTexture.sizeOffset_X, sleekImageTexture.sizeOffset_Y, new ItemIconReady(sleekImageTexture.updateTexture));
			SleekLabel sleekLabel = new SleekLabel();
			if (icon != null)
			{
				sleekLabel.positionOffset_X = 35 + sleekImageTexture.sizeOffset_X;
				sleekLabel.sizeOffset_X = -40 - sleekImageTexture.sizeOffset_X;
			}
			else
			{
				sleekLabel.positionOffset_X = 10 + sleekImageTexture.sizeOffset_X;
				sleekLabel.sizeOffset_X = -15 - sleekImageTexture.sizeOffset_X;
			}
			sleekLabel.sizeScale_X = 1f;
			sleekLabel.sizeScale_Y = 1f;
			sleekLabel.fontAlignment = 3;
			sleekLabel.foregroundTint = ESleekTint.NONE;
			sleekLabel.isRich = true;
			sleekLabel.text = text;
			sleekBox.add(sleekLabel);
			return sleekBox;
		}

		private static InventorySearchQualityAscendingComparator qualityAscendingComparator = new InventorySearchQualityAscendingComparator();

		private static List<InventorySearch> search = new List<InventorySearch>();
	}
}
