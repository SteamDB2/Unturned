using System;
using System.Collections.Generic;

namespace SDG.Unturned
{
	public class VendorSelling : VendorElement
	{
		public VendorSelling(byte newIndex, ushort newID, uint newCost, INPCCondition[] newConditions) : base(newIndex, newID, newCost, newConditions)
		{
		}

		public bool canBuy(Player player)
		{
			return player.skills.experience >= base.cost;
		}

		public void buy(Player player)
		{
			player.inventory.forceAddItem(new Item(base.id, EItemOrigin.ADMIN), false, false);
			player.skills.askSpend(base.cost);
		}

		public void format(Player player, out ushort total)
		{
			VendorSelling.search.Clear();
			player.inventory.search(VendorSelling.search, base.id, false, true);
			total = 0;
			byte b = 0;
			while ((int)b < VendorSelling.search.Count)
			{
				total += (ushort)VendorSelling.search[(int)b].jar.item.amount;
				b += 1;
			}
		}

		private static List<InventorySearch> search = new List<InventorySearch>();
	}
}
