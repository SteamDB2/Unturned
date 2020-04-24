using System;
using System.Collections.Generic;

namespace SDG.Unturned
{
	public class ZombieSlot
	{
		public ZombieSlot(float newChance, List<ZombieCloth> newTable)
		{
			this._table = newTable;
			this.chance = newChance;
		}

		public List<ZombieCloth> table
		{
			get
			{
				return this._table;
			}
		}

		public void addCloth(ushort id)
		{
			if (this.table.Count == 255)
			{
				return;
			}
			byte b = 0;
			while ((int)b < this.table.Count)
			{
				if (this.table[(int)b].item == id)
				{
					return;
				}
				b += 1;
			}
			this.table.Add(new ZombieCloth(id));
		}

		public void removeCloth(byte index)
		{
			this.table.RemoveAt((int)index);
		}

		private List<ZombieCloth> _table;

		public float chance;
	}
}
