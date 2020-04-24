using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class ItemShirtAsset : ItemBagAsset
	{
		public ItemShirtAsset(Bundle bundle, Data data, Local localization, ushort id) : base(bundle, data, localization, id)
		{
			if (!Dedicator.isDedicated)
			{
				this._shirt = (Texture2D)bundle.load("Shirt");
				this._emission = (Texture2D)bundle.load("Emission");
				this._metallic = (Texture2D)bundle.load("Metallic");
			}
			this._ignoreHand = data.has("Ignore_Hand");
			bundle.unload();
		}

		public Texture2D shirt
		{
			get
			{
				return this._shirt;
			}
		}

		public Texture2D emission
		{
			get
			{
				return this._emission;
			}
		}

		public Texture2D metallic
		{
			get
			{
				return this._metallic;
			}
		}

		public bool ignoreHand
		{
			get
			{
				return this._ignoreHand;
			}
		}

		protected Texture2D _shirt;

		protected Texture2D _emission;

		protected Texture2D _metallic;

		protected bool _ignoreHand;
	}
}
