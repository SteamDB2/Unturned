using System;

namespace SDG.Unturned
{
	public class ItemCloudAsset : ItemAsset
	{
		public ItemCloudAsset(Bundle bundle, Data data, Local localization, ushort id) : base(bundle, data, localization, id)
		{
			this._gravity = data.readSingle("Gravity");
			bundle.unload();
		}

		public float gravity
		{
			get
			{
				return this._gravity;
			}
		}

		private float _gravity;
	}
}
