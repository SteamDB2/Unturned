using System;

namespace SDG.Unturned
{
	public class ItemFarmAsset : ItemBarricadeAsset
	{
		public ItemFarmAsset(Bundle bundle, Data data, Local localization, ushort id) : base(bundle, data, localization, id)
		{
			this._growth = data.readUInt32("Growth");
			this._grow = data.readUInt16("Grow");
		}

		public uint growth
		{
			get
			{
				return this._growth;
			}
		}

		public ushort grow
		{
			get
			{
				return this._grow;
			}
		}

		protected uint _growth;

		protected ushort _grow;
	}
}
