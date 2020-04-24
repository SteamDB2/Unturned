using System;

namespace SDG.Unturned
{
	public class ItemStorageAsset : ItemBarricadeAsset
	{
		public ItemStorageAsset(Bundle bundle, Data data, Local localization, ushort id) : base(bundle, data, localization, id)
		{
			this._storage_x = data.readByte("Storage_X");
			if (this.storage_x < 1)
			{
				this._storage_x = 1;
			}
			this._storage_y = data.readByte("Storage_Y");
			if (this.storage_y < 1)
			{
				this._storage_y = 1;
			}
			this._isDisplay = data.has("Display");
		}

		public byte storage_x
		{
			get
			{
				return this._storage_x;
			}
		}

		public byte storage_y
		{
			get
			{
				return this._storage_y;
			}
		}

		public bool isDisplay
		{
			get
			{
				return this._isDisplay;
			}
		}

		public override byte[] getState(EItemOrigin origin)
		{
			if (this.isDisplay)
			{
				return new byte[21];
			}
			return new byte[17];
		}

		protected byte _storage_x;

		protected byte _storage_y;

		protected bool _isDisplay;
	}
}
