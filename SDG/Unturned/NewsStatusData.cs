using System;

namespace SDG.Unturned
{
	public class NewsStatusData
	{
		public NewsStatusData()
		{
			this.Featured_Workshop = 0UL;
			this.Popular_Workshop_Trend_Days = 30u;
			this.Popular_Workshop_Carousel_Items = 3;
			this.Announcements_Count = 3;
		}

		public ulong Featured_Workshop;

		public uint Popular_Workshop_Trend_Days;

		public int Popular_Workshop_Carousel_Items;

		public int Announcements_Count;
	}
}
