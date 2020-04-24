using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class ItemToolAsset : ItemAsset
	{
		public ItemToolAsset(Bundle bundle, Data data, Local localization, ushort id) : base(bundle, data, localization, id)
		{
			this._use = (AudioClip)bundle.load("Use");
			bundle.unload();
		}

		public AudioClip use
		{
			get
			{
				return this._use;
			}
		}

		public override bool isDangerous
		{
			get
			{
				return true;
			}
		}

		protected AudioClip _use;
	}
}
