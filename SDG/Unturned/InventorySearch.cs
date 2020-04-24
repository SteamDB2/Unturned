using System;

namespace SDG.Unturned
{
	public class InventorySearch
	{
		public InventorySearch(byte newPage, ItemJar newJar)
		{
			this._page = newPage;
			this._jar = newJar;
		}

		public byte page
		{
			get
			{
				return this._page;
			}
		}

		public ItemJar jar
		{
			get
			{
				return this._jar;
			}
		}

		private byte _page;

		private ItemJar _jar;
	}
}
