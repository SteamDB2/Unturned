using System;

namespace SDG.Unturned
{
	public class ItemBagAsset : ItemClothingAsset
	{
		public ItemBagAsset(Bundle bundle, Data data, Local localization, ushort id) : base(bundle, data, localization, id)
		{
			if (!this.isPro)
			{
				this._width = data.readByte("Width");
				this._height = data.readByte("Height");
			}
		}

		public byte width
		{
			get
			{
				return this._width;
			}
		}

		public byte height
		{
			get
			{
				return this._height;
			}
		}

		private byte _width;

		private byte _height;
	}
}
