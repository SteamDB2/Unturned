using System;
using System.Collections.Generic;

namespace SDG.Unturned
{
	public class InventorySearchQualityDescendingComparator : IComparer<InventorySearch>
	{
		public int Compare(InventorySearch a, InventorySearch b)
		{
			return (int)(b.jar.item.quality - a.jar.item.quality);
		}
	}
}
