using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class ItemFuelAsset : ItemAsset
	{
		public ItemFuelAsset(Bundle bundle, Data data, Local localization, ushort id) : base(bundle, data, localization, id)
		{
			this._use = (AudioClip)bundle.load("Use");
			this._fuel = data.readUInt16("Fuel");
			this.fuelState = BitConverter.GetBytes(this.fuel);
			bundle.unload();
		}

		public AudioClip use
		{
			get
			{
				return this._use;
			}
		}

		public ushort fuel
		{
			get
			{
				return this._fuel;
			}
		}

		public override byte[] getState(EItemOrigin origin)
		{
			byte[] array = new byte[2];
			if (origin == EItemOrigin.ADMIN)
			{
				array[0] = this.fuelState[0];
				array[1] = this.fuelState[1];
			}
			return array;
		}

		public override string getContext(string desc, byte[] state)
		{
			ushort num = BitConverter.ToUInt16(state, 0);
			desc += PlayerDashboardInventoryUI.localization.format("Fuel", new object[]
			{
				((int)((float)num / (float)this.fuel * 100f)).ToString()
			});
			desc += "\n\n";
			return desc;
		}

		protected AudioClip _use;

		protected ushort _fuel;

		private byte[] fuelState;
	}
}
