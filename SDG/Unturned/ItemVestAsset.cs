using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class ItemVestAsset : ItemBagAsset
	{
		public ItemVestAsset(Bundle bundle, Data data, Local localization, ushort id) : base(bundle, data, localization, id)
		{
			if (!Dedicator.isDedicated)
			{
				this._vest = (GameObject)bundle.load("Vest");
			}
			bundle.unload();
		}

		public GameObject vest
		{
			get
			{
				return this._vest;
			}
		}

		protected GameObject _vest;
	}
}
