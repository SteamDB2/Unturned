using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class ItemSightAsset : ItemCaliberAsset
	{
		public ItemSightAsset(Bundle bundle, Data data, Local localization, ushort id) : base(bundle, data, localization, id)
		{
			this._sight = (GameObject)bundle.load("Sight");
			if (data.has("Vision"))
			{
				this._vision = (ELightingVision)Enum.Parse(typeof(ELightingVision), data.readString("Vision"), true);
			}
			else
			{
				this._vision = ELightingVision.NONE;
			}
			if (data.has("Zoom"))
			{
				this._zoom = 90f / (float)data.readByte("Zoom");
			}
			else
			{
				this._zoom = 90f;
			}
			this._isHolographic = data.has("Holographic");
			bundle.unload();
		}

		public GameObject sight
		{
			get
			{
				return this._sight;
			}
		}

		public ELightingVision vision
		{
			get
			{
				return this._vision;
			}
		}

		public float zoom
		{
			get
			{
				return this._zoom;
			}
		}

		public bool isHolographic
		{
			get
			{
				return this._isHolographic;
			}
		}

		protected GameObject _sight;

		private ELightingVision _vision;

		private float _zoom;

		private bool _isHolographic;
	}
}
