using System;

namespace SDG.Unturned
{
	public class ItemOpticAsset : ItemAsset
	{
		public ItemOpticAsset(Bundle bundle, Data data, Local localization, ushort id) : base(bundle, data, localization, id)
		{
			this._zoom = 90f / (float)data.readByte("Zoom");
			bundle.unload();
		}

		public float zoom
		{
			get
			{
				return this._zoom;
			}
		}

		private float _zoom;
	}
}
