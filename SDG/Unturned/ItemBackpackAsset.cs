using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class ItemBackpackAsset : ItemBagAsset
	{
		public ItemBackpackAsset(Bundle bundle, Data data, Local localization, ushort id) : base(bundle, data, localization, id)
		{
			if (!Dedicator.isDedicated)
			{
				this._backpack = (GameObject)bundle.load("Backpack");
			}
			bundle.unload();
		}

		public GameObject backpack
		{
			get
			{
				return this._backpack;
			}
		}

		protected GameObject _backpack;
	}
}
