using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class ItemHatAsset : ItemGearAsset
	{
		public ItemHatAsset(Bundle bundle, Data data, Local localization, ushort id) : base(bundle, data, localization, id)
		{
			if (!Dedicator.isDedicated)
			{
				this._hat = (GameObject)bundle.load("Hat");
			}
			bundle.unload();
		}

		public GameObject hat
		{
			get
			{
				return this._hat;
			}
		}

		protected GameObject _hat;
	}
}
