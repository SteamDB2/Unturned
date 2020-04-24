using System;
using System.Collections.Generic;

namespace SDG.Unturned
{
	public class InventorySearchQualityAscendingComparator : IComparer<InventorySearch>
	{
		public int Compare(InventorySearch a, InventorySearch b)
		{
			return (int)(a.jar.item.quality - b.jar.item.quality);
		}
	}
}
