using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class ItemGripAsset : ItemCaliberAsset
	{
		public ItemGripAsset(Bundle bundle, Data data, Local localization, ushort id) : base(bundle, data, localization, id)
		{
			this._grip = (GameObject)bundle.load("Grip");
			this._isBipod = data.has("Bipod");
			bundle.unload();
		}

		public GameObject grip
		{
			get
			{
				return this._grip;
			}
		}

		public bool isBipod
		{
			get
			{
				return this._isBipod;
			}
		}

		protected GameObject _grip;

		private bool _isBipod;
	}
}
