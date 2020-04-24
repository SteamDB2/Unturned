using System;
using System.Collections.Generic;

namespace SDG.Unturned
{
	public class InventorySearchAmountAscendingComparator : IComparer<InventorySearch>
	{
		public int Compare(InventorySearch a, InventorySearch b)
		{
			return (int)(a.jar.item.amount - b.jar.item.amount);
		}
	}
}
