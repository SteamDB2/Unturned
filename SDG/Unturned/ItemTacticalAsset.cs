using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class ItemTacticalAsset : ItemCaliberAsset
	{
		public ItemTacticalAsset(Bundle bundle, Data data, Local localization, ushort id) : base(bundle, data, localization, id)
		{
			this._tactical = (GameObject)bundle.load("Tactical");
			this._isLaser = data.has("Laser");
			this._isLight = data.has("Light");
			this._isRangefinder = data.has("Rangefinder");
			this._isMelee = data.has("Melee");
			bundle.unload();
		}

		public GameObject tactical
		{
			get
			{
				return this._tactical;
			}
		}

		public bool isLaser
		{
			get
			{
				return this._isLaser;
			}
		}

		public bool isLight
		{
			get
			{
				return this._isLight;
			}
		}

		public bool isRangefinder
		{
			get
			{
				return this._isRangefinder;
			}
		}

		public bool isMelee
		{
			get
			{
				return this._isMelee;
			}
		}

		protected GameObject _tactical;

		private bool _isLaser;

		private bool _isLight;

		private bool _isRangefinder;

		private bool _isMelee;
	}
}
