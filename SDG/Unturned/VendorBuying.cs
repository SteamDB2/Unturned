using System;
using System.Collections.Generic;

namespace SDG.Unturned
{
	public class VendorBuying : VendorElement
	{
		public VendorBuying(byte newIndex, ushort newID, uint newCost, INPCCondition[] newConditions) : base(newIndex, newID, newCost, newConditions)
		{
		}

		public bool canSell(Player player)
		{
			ItemAsset itemAsset = Assets.find(EAssetType.ITEM, base.id) as ItemAsset;
			VendorBuying.search.Clear();
			player.inventory.search(VendorBuying.search, base.id, false, true);
			ushort num = 0;
			byte b = 0;
			while ((int)b < VendorBuying.search.Count)
			{
				num += (ushort)VendorBuying.search[(int)b].jar.item.amount;
				b += 1;
			}
			return num >= (ushort)itemAsset.amount;
		}

		public void sell(Player player)
		{
			ItemAsset itemAsset = Assets.find(EAssetType.ITEM, base.id) as ItemAsset;
			VendorBuying.search.Clear();
			player.inventory.search(VendorBuying.search, base.id, false, true);
			VendorBuying.search.Sort(VendorBuying.qualityAscendingComparator);
			ushort num = (ushort)itemAsset.amount;
			byte b = 0;
			while ((int)b < VendorBuying.search.Count)
			{
				InventorySearch inventorySearch = VendorBuying.search[(int)b];
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
			player.skills.askAward(base.cost);
		}

		public void format(Player player, out ushort total, out byte amount)
		{
			ItemAsset itemAsset = Assets.find(EAssetType.ITEM, base.id) as ItemAsset;
			VendorBuying.search.Clear();
			player.inventory.search(VendorBuying.search, base.id, false, true);
			total = 0;
			byte b = 0;
			while ((int)b < VendorBuying.search.Count)
			{
				total += (ushort)VendorBuying.search[(int)b].jar.item.amount;
				b += 1;
			}
			amount = itemAsset.amount;
		}

		private static InventorySearchQualityAscendingComparator qualityAscendingComparator = new InventorySearchQualityAscendingComparator();

		private static List<InventorySearch> search = new List<InventorySearch>();
	}
}
