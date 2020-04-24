using System;

namespace SDG.Unturned
{
	public class ItemSpawn
	{
		public ItemSpawn(ushort newItem)
		{
			this._item = newItem;
		}

		public ushort item
		{
			get
			{
				return this._item;
			}
		}

		private ushort _item;
	}
}
