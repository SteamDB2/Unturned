using System;
using System.Collections.Generic;

namespace SDG.Unturned
{
	public class AnimalTier
	{
		public AnimalTier(List<AnimalSpawn> newTable, string newName, float newChance)
		{
			this._table = newTable;
			this.name = newName;
			this.chance = newChance;
		}

		public List<AnimalSpawn> table
		{
			get
			{
				return this._table;
			}
		}

		public void addAnimal(ushort id)
		{
			if (this.table.Count == 255)
			{
				return;
			}
			byte b = 0;
			while ((int)b < this.table.Count)
			{
				if (this.table[(int)b].animal == id)
				{
					return;
				}
				b += 1;
			}
			this.table.Add(new AnimalSpawn(id));
		}

		public void removeAnimal(byte index)
		{
			this.table.RemoveAt((int)index);
		}

		private List<AnimalSpawn> _table;

		public string name;

		public float chance;
	}
}
