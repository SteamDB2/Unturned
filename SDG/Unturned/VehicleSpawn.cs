using System;

namespace SDG.Unturned
{
	public class VehicleSpawn
	{
		public VehicleSpawn(ushort newVehicle)
		{
			this._vehicle = newVehicle;
		}

		public ushort vehicle
		{
			get
			{
				return this._vehicle;
			}
		}

		private ushort _vehicle;
	}
}
