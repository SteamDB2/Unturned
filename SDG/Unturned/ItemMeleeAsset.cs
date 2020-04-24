using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class ItemMeleeAsset : ItemWeaponAsset
	{
		public ItemMeleeAsset(Bundle bundle, Data data, Local localization, ushort id) : base(bundle, data, localization, id)
		{
			this._use = (AudioClip)bundle.load("Use");
			this._strength = data.readSingle("Strength");
			this._weak = data.readSingle("Weak");
			if ((double)this.weak < 0.01)
			{
				this._weak = 0.5f;
			}
			this._strong = data.readSingle("Strong");
			if ((double)this.strong < 0.01)
			{
				this._strong = 0.33f;
			}
			this._stamina = data.readByte("Stamina");
			this._isRepair = data.has("Repair");
			this._isRepeated = data.has("Repeated");
			this._isLight = data.has("Light");
			if (data.has("Alert_Radius"))
			{
				this.alertRadius = data.readSingle("Alert_Radius");
			}
			else
			{
				this.alertRadius = 8f;
			}
			bundle.unload();
		}

		public AudioClip use
		{
			get
			{
				return this._use;
			}
		}

		public override byte[] getState(EItemOrigin origin)
		{
			if (this.isLight)
			{
				return new byte[]
				{
					1
				};
			}
			return new byte[0];
		}

		public float strength
		{
			get
			{
				return this._strength;
			}
		}

		public float weak
		{
			get
			{
				return this._weak;
			}
		}

		public float strong
		{
			get
			{
				return this._strong;
			}
		}

		public byte stamina
		{
			get
			{
				return this._stamina;
			}
		}

		public bool isRepair
		{
			get
			{
				return this._isRepair;
			}
		}

		public bool isRepeated
		{
			get
			{
				return this._isRepeated;
			}
		}

		public bool isLight
		{
			get
			{
				return this._isLight;
			}
		}

		public override bool showQuality
		{
			get
			{
				return true;
			}
		}

		public override bool isDangerous
		{
			get
			{
				return true;
			}
		}

		public float alertRadius { get; protected set; }

		protected AudioClip _use;

		private float _strength;

		private float _weak;

		private float _strong;

		private byte _stamina;

		private bool _isRepair;

		private bool _isRepeated;

		private bool _isLight;
	}
}
