using System;

namespace SDG.Unturned
{
	public class ItemClothingAsset : ItemAsset
	{
		public ItemClothingAsset(Bundle bundle, Data data, Local localization, ushort id) : base(bundle, data, localization, id)
		{
			if (this.isPro)
			{
				this._armor = 1f;
			}
			else
			{
				this._armor = data.readSingle("Armor");
				if ((double)this.armor < 0.01)
				{
					this._armor = 1f;
				}
				this._proofWater = data.has("Proof_Water");
				this._proofFire = data.has("Proof_Fire");
			}
		}

		public float armor
		{
			get
			{
				return this._armor;
			}
		}

		public override bool showQuality
		{
			get
			{
				return true;
			}
		}

		public bool proofWater
		{
			get
			{
				return this._proofWater;
			}
		}

		public bool proofFire
		{
			get
			{
				return this._proofFire;
			}
		}

		protected float _armor;

		private bool _proofWater;

		private bool _proofFire;
	}
}
