using System;

namespace SDG.Unturned
{
	public class ItemCaliberAsset : ItemAsset
	{
		public ItemCaliberAsset(Bundle bundle, Data data, Local localization, ushort id) : base(bundle, data, localization, id)
		{
			this._calibers = new ushort[(int)data.readByte("Calibers")];
			byte b = 0;
			while ((int)b < this.calibers.Length)
			{
				this._calibers[(int)b] = data.readUInt16("Caliber_" + b);
				b += 1;
			}
			this._recoil_x = data.readSingle("Recoil_X");
			if ((double)this.recoil_x < 0.01)
			{
				this._recoil_x = 1f;
			}
			this._recoil_y = data.readSingle("Recoil_Y");
			if ((double)this.recoil_y < 0.01)
			{
				this._recoil_y = 1f;
			}
			this._spread = data.readSingle("Spread");
			if ((double)this.spread < 0.01)
			{
				this._spread = 1f;
			}
			this._sway = data.readSingle("Sway");
			if ((double)this.sway < 0.01)
			{
				this._sway = 1f;
			}
			this._shake = data.readSingle("Shake");
			if ((double)this.shake < 0.01)
			{
				this._shake = 1f;
			}
			this._damage = data.readSingle("Damage");
			if ((double)this.damage < 0.01)
			{
				this._damage = 1f;
			}
			this._firerate = data.readByte("Firerate");
			this._isPaintable = data.has("Paintable");
		}

		public ushort[] calibers
		{
			get
			{
				return this._calibers;
			}
		}

		public float recoil_x
		{
			get
			{
				return this._recoil_x;
			}
		}

		public float recoil_y
		{
			get
			{
				return this._recoil_y;
			}
		}

		public float spread
		{
			get
			{
				return this._spread;
			}
		}

		public float sway
		{
			get
			{
				return this._sway;
			}
		}

		public float shake
		{
			get
			{
				return this._shake;
			}
		}

		public float damage
		{
			get
			{
				return this._damage;
			}
		}

		public byte firerate
		{
			get
			{
				return this._firerate;
			}
		}

		public bool isPaintable
		{
			get
			{
				return this._isPaintable;
			}
		}

		private ushort[] _calibers;

		private float _recoil_x;

		private float _recoil_y;

		private float _spread;

		private float _sway;

		private float _shake;

		private float _damage;

		private byte _firerate;

		protected bool _isPaintable;
	}
}
