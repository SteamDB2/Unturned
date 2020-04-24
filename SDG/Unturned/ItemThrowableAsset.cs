using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class ItemThrowableAsset : ItemWeaponAsset
	{
		public ItemThrowableAsset(Bundle bundle, Data data, Local localization, ushort id) : base(bundle, data, localization, id)
		{
			this._use = (AudioClip)bundle.load("Use");
			this._throwable = (GameObject)bundle.load("Throwable");
			this._explosion = data.readUInt16("Explosion");
			this._isExplosive = data.has("Explosive");
			this._isFlash = data.has("Flash");
			this._isSticky = data.has("Sticky");
			bundle.unload();
		}

		public AudioClip use
		{
			get
			{
				return this._use;
			}
		}

		public GameObject throwable
		{
			get
			{
				return this._throwable;
			}
		}

		public ushort explosion
		{
			get
			{
				return this._explosion;
			}
		}

		public bool isExplosive
		{
			get
			{
				return this._isExplosive;
			}
		}

		public bool isFlash
		{
			get
			{
				return this._isFlash;
			}
		}

		public bool isSticky
		{
			get
			{
				return this._isSticky;
			}
		}

		public override bool isDangerous
		{
			get
			{
				return this.isExplosive;
			}
		}

		protected AudioClip _use;

		protected GameObject _throwable;

		private ushort _explosion;

		private bool _isExplosive;

		private bool _isFlash;

		private bool _isSticky;
	}
}
