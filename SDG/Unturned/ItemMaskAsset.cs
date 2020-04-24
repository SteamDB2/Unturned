using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class ItemMaskAsset : ItemGearAsset
	{
		public ItemMaskAsset(Bundle bundle, Data data, Local localization, ushort id) : base(bundle, data, localization, id)
		{
			if (!Dedicator.isDedicated)
			{
				this._mask = (GameObject)bundle.load("Mask");
			}
			if (!this.isPro)
			{
				this._proofRadiation = data.has("Proof_Radiation");
				this._isEarpiece = data.has("Earpiece");
			}
			bundle.unload();
		}

		public GameObject mask
		{
			get
			{
				return this._mask;
			}
		}

		public bool proofRadiation
		{
			get
			{
				return this._proofRadiation;
			}
		}

		public bool isEarpiece
		{
			get
			{
				return this._isEarpiece;
			}
		}

		protected GameObject _mask;

		private bool _proofRadiation;

		private bool _isEarpiece;
	}
}
