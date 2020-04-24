using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class ItemDetonatorAsset : ItemAsset
	{
		public ItemDetonatorAsset(Bundle bundle, Data data, Local localization, ushort id) : base(bundle, data, localization, id)
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

		protected AudioClip _use;
	}
}
