using System;

namespace SDG.Unturned
{
	public class ItemGearAsset : ItemClothingAsset
	{
		public ItemGearAsset(Bundle bundle, Data data, Local localization, ushort id) : base(bundle, data, localization, id)
		{
			this._hasHair = data.has("Hair");
			this._hasBeard = data.has("Beard");
		}

		public bool hasHair
		{
			get
			{
				return this._hasHair;
			}
		}

		public bool hasBeard
		{
			get
			{
				return this._hasBeard;
			}
		}

		protected bool _hasHair;

		protected bool _hasBeard;
	}
}
