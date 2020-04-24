using System;
using System.Collections.Generic;

namespace SDG.Unturned
{
	public class VehicleTier
	{
		public VehicleTier(List<VehicleSpawn> newTable, string newName, float newChance)
		{
			this._table = newTable;
			this.name = newName;
			this.chance = newChance;
		}

		public List<VehicleSpawn> table
		{
			get
			{
				return this._table;
			}
		}

		public void addVehicle(ushort id)
		{
			if (this.table.Count == 255)
			{
				return;
			}
			byte b = 0;
			while ((int)b < this.table.Count)
			{
				if (this.table[(int)b].vehicle == id)
				{
					return;
				}
				b += 1;
			}
			this.table.Add(new VehicleSpawn(id));
		}

		public void removeVehicle(byte index)
		{
			this.table.RemoveAt((int)index);
		}

		private List<VehicleSpawn> _table;

		public string name;

		public float chance;
	}
}
