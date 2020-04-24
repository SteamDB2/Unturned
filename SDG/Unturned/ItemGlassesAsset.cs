using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class ItemGlassesAsset : ItemGearAsset
	{
		public ItemGlassesAsset(Bundle bundle, Data data, Local localization, ushort id) : base(bundle, data, localization, id)
		{
			if (!Dedicator.isDedicated)
			{
				this._glasses = (GameObject)bundle.load("Glasses");
			}
			if (data.has("Vision"))
			{
				this._vision = (ELightingVision)Enum.Parse(typeof(ELightingVision), data.readString("Vision"), true);
			}
			else
			{
				this._vision = ELightingVision.NONE;
			}
			this.isBlindfold = data.has("Blindfold");
			bundle.unload();
		}

		public GameObject glasses
		{
			get
			{
				return this._glasses;
			}
		}

		public ELightingVision vision
		{
			get
			{
				return this._vision;
			}
		}

		public bool isBlindfold { get; protected set; }

		public override byte[] getState(EItemOrigin origin)
		{
			if (this.vision != ELightingVision.NONE)
			{
				return new byte[]
				{
					1
				};
			}
			return new byte[0];
		}

		protected GameObject _glasses;

		private ELightingVision _vision;
	}
}
