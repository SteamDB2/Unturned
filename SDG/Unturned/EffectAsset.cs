using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class EffectAsset : Asset
	{
		public EffectAsset(Bundle bundle, Data data, Local localization, ushort id) : base(bundle, data, localization, id)
		{
			if (id < 200 && !bundle.hasResource && !data.has("Bypass_ID_Limit"))
			{
				throw new NotSupportedException("ID < 200");
			}
			this._effect = (GameObject)bundle.load("Effect");
			if (this.effect == null)
			{
				throw new NotSupportedException("Missing effect gameobject");
			}
			this._gore = data.has("Gore");
			this._splatters = new GameObject[(int)data.readByte("Splatter")];
			for (int i = 0; i < this.splatters.Length; i++)
			{
				this.splatters[i] = (GameObject)bundle.load("Splatter_" + i);
			}
			this._splatter = data.readByte("Splatters");
			this._splatterLiquid = data.has("Splatter_Liquid");
			if (data.has("Splatter_Temperature"))
			{
				this._splatterTemperature = (EPlayerTemperature)Enum.Parse(typeof(EPlayerTemperature), data.readString("Splatter_Temperature"), true);
			}
			else
			{
				this._splatterTemperature = EPlayerTemperature.NONE;
			}
			this._splatterLifetime = data.readSingle("Splatter_Lifetime");
			if (data.has("Splatter_Lifetime_Spread"))
			{
				this._splatterLifetimeSpread = data.readSingle("Splatter_Lifetime_Spread");
			}
			else
			{
				this._splatterLifetimeSpread = 1f;
			}
			this._lifetime = data.readSingle("Lifetime");
			if (data.has("Lifetime_Spread"))
			{
				this._lifetimeSpread = data.readSingle("Lifetime_Spread");
			}
			else
			{
				this._lifetimeSpread = 4f;
			}
			this._isStatic = data.has("Static");
			if (data.has("Preload"))
			{
				this._preload = data.readByte("Preload");
			}
			else
			{
				this._preload = 1;
			}
			if (data.has("Splatter_Preload"))
			{
				this._splatterPreload = data.readByte("Splatter_Preload");
			}
			else
			{
				this._splatterPreload = (byte)(Mathf.CeilToInt((float)this.splatter / (float)this.splatters.Length) * (int)this.preload);
			}
			this._blast = data.readUInt16("Blast");
			bundle.unload();
		}

		public GameObject effect
		{
			get
			{
				return this._effect;
			}
		}

		public GameObject[] splatters
		{
			get
			{
				return this._splatters;
			}
		}

		public bool gore
		{
			get
			{
				return this._gore;
			}
		}

		public byte splatter
		{
			get
			{
				return this._splatter;
			}
		}

		public float splatterLifetime
		{
			get
			{
				return this._splatterLifetime;
			}
		}

		public float splatterLifetimeSpread
		{
			get
			{
				return this._splatterLifetimeSpread;
			}
		}

		public bool splatterLiquid
		{
			get
			{
				return this._splatterLiquid;
			}
		}

		public EPlayerTemperature splatterTemperature
		{
			get
			{
				return this._splatterTemperature;
			}
		}

		public byte splatterPreload
		{
			get
			{
				return this._splatterPreload;
			}
		}

		public float lifetime
		{
			get
			{
				return this._lifetime;
			}
		}

		public float lifetimeSpread
		{
			get
			{
				return this._lifetimeSpread;
			}
		}

		public bool isStatic
		{
			get
			{
				return this._isStatic;
			}
		}

		public byte preload
		{
			get
			{
				return this._preload;
			}
		}

		public ushort blast
		{
			get
			{
				return this._blast;
			}
		}

		public override EAssetType assetCategory
		{
			get
			{
				return EAssetType.EFFECT;
			}
		}

		protected GameObject _effect;

		protected GameObject[] _splatters;

		private bool _gore;

		private byte _splatter;

		private float _splatterLifetime;

		private float _splatterLifetimeSpread;

		private bool _splatterLiquid;

		private EPlayerTemperature _splatterTemperature;

		private byte _splatterPreload;

		private float _lifetime;

		private float _lifetimeSpread;

		private bool _isStatic;

		private byte _preload;

		private ushort _blast;
	}
}
