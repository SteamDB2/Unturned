using System;

namespace SDG.Unturned
{
	public class StockpileStatusData
	{
		public StockpileStatusData()
		{
			this.Has_New_Items = false;
			this.Featured_Item = 0;
		}

		public bool Has_New_Items;

		public int Featured_Item;
	}
}
