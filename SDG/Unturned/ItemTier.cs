using System;
using System.Collections.Generic;

namespace SDG.Unturned
{
	public class ItemTier
	{
		public ItemTier(List<ItemSpawn> newTable, string newName, float newChance)
		{
			this._table = newTable;
			this.name = newName;
			this.chance = newChance;
		}

		public List<ItemSpawn> table
		{
			get
			{
				return this._table;
			}
		}

		public void addItem(ushort id)
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
			this.table.Add(new ItemSpawn(id));
		}

		public void removeItem(byte index)
		{
			this.table.RemoveAt((int)index);
		}

		private List<ItemSpawn> _table;

		public string name;

		public float chance;
	}
}
