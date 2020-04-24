using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class ItemPantsAsset : ItemBagAsset
	{
		public ItemPantsAsset(Bundle bundle, Data data, Local localization, ushort id) : base(bundle, data, localization, id)
		{
			if (!Dedicator.isDedicated)
			{
				this._pants = (Texture2D)bundle.load("Pants");
				this._emission = (Texture2D)bundle.load("Emission");
				this._metallic = (Texture2D)bundle.load("Metallic");
			}
			bundle.unload();
		}

		public Texture2D pants
		{
			get
			{
				return this._pants;
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

		protected Texture2D _pants;

		protected Texture2D _emission;

		protected Texture2D _metallic;
	}
}
