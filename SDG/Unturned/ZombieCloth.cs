using System;

namespace SDG.Unturned
{
	public class ZombieCloth
	{
		public ZombieCloth(ushort newItem)
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
