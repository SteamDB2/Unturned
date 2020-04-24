using System;

namespace SDG.Unturned
{
	public class ItemLibraryAsset : ItemBarricadeAsset
	{
		public ItemLibraryAsset(Bundle bundle, Data data, Local localization, ushort id) : base(bundle, data, localization, id)
		{
			this._capacity = data.readUInt32("Capacity");
			this._tax = data.readByte("Tax");
		}

		public uint capacity
		{
			get
			{
				return this._capacity;
			}
		}

		public byte tax
		{
			get
			{
				return this._tax;
			}
		}

		public override byte[] getState(EItemOrigin origin)
		{
			return new byte[20];
		}

		protected uint _capacity;

		protected byte _tax;
	}
}
