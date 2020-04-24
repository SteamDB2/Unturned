using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class ItemBarricadeAsset : ItemAsset
	{
		public ItemBarricadeAsset(Bundle bundle, Data data, Local localization, ushort id) : base(bundle, data, localization, id)
		{
			if (!Dedicator.isDedicated)
			{
				this._barricade = (GameObject)bundle.load("Barricade");
			}
			this._clip = (GameObject)bundle.load("Clip");
			this._nav = (GameObject)bundle.load("Nav");
			this._use = (AudioClip)bundle.load("Use");
			this._build = (EBuild)Enum.Parse(typeof(EBuild), data.readString("Build"), true);
			this._health = data.readUInt16("Health");
			this._range = data.readSingle("Range");
			this._radius = data.readSingle("Radius");
			this._offset = data.readSingle("Offset");
			this._explosion = data.readUInt16("Explosion");
			this._isLocked = data.has("Locked");
			this._isVulnerable = data.has("Vulnerable");
			this._bypassClaim = data.has("Bypass_Claim");
			this._isRepairable = !data.has("Unrepairable");
			this._proofExplosion = data.has("Proof_Explosion");
			this._isUnpickupable = data.has("Unpickupable");
			this._isSalvageable = !data.has("Unsalvageable");
			this._isSaveable = !data.has("Unsaveable");
			bundle.unload();
		}

		public GameObject barricade
		{
			get
			{
				return this._barricade;
			}
		}

		public GameObject clip
		{
			get
			{
				return this._clip;
			}
		}

		public GameObject nav
		{
			get
			{
				return this._nav;
			}
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
			if (this.build == EBuild.DOOR || this.build == EBuild.GATE || this.build == EBuild.SHUTTER || this.build == EBuild.HATCH)
			{
				return new byte[17];
			}
			if (this.build == EBuild.BED)
			{
				return new byte[8];
			}
			if (this.build == EBuild.FARM)
			{
				return new byte[4];
			}
			if (this.build == EBuild.TORCH || this.build == EBuild.CAMPFIRE || this.build == EBuild.OVEN || this.build == EBuild.SPOT || this.build == EBuild.SAFEZONE || this.build == EBuild.OXYGENATOR || this.build == EBuild.BARREL_RAIN || this.build == EBuild.CAGE)
			{
				return new byte[1];
			}
			if (this.build == EBuild.OIL)
			{
				return new byte[2];
			}
			if (this.build == EBuild.SIGN || this.build == EBuild.SIGN_WALL || this.build == EBuild.NOTE)
			{
				return new byte[17];
			}
			if (this.build == EBuild.STEREO)
			{
				return new byte[17];
			}
			if (this.build == EBuild.MANNEQUIN)
			{
				return new byte[73];
			}
			return new byte[0];
		}

		public EBuild build
		{
			get
			{
				return this._build;
			}
		}

		public ushort health
		{
			get
			{
				return this._health;
			}
		}

		public float range
		{
			get
			{
				return this._range;
			}
		}

		public float radius
		{
			get
			{
				return this._radius;
			}
		}

		public float offset
		{
			get
			{
				return this._offset;
			}
		}

		public ushort explosion
		{
			get
			{
				return this._explosion;
			}
		}

		public bool isLocked
		{
			get
			{
				return this._isLocked;
			}
		}

		public bool isVulnerable
		{
			get
			{
				return this._isVulnerable;
			}
		}

		public bool bypassClaim
		{
			get
			{
				return this._bypassClaim;
			}
		}

		public bool isRepairable
		{
			get
			{
				return this._isRepairable;
			}
		}

		public bool proofExplosion
		{
			get
			{
				return this._proofExplosion;
			}
		}

		public bool isUnpickupable
		{
			get
			{
				return this._isUnpickupable;
			}
		}

		public bool isSalvageable
		{
			get
			{
				return this._isSalvageable;
			}
		}

		public bool isSaveable
		{
			get
			{
				return this._isSaveable;
			}
		}

		public override bool isDangerous
		{
			get
			{
				return true;
			}
		}

		protected GameObject _barricade;

		protected GameObject _clip;

		protected GameObject _nav;

		protected AudioClip _use;

		protected EBuild _build;

		protected ushort _health;

		protected float _range;

		protected float _radius;

		protected float _offset;

		protected ushort _explosion;

		protected bool _isLocked;

		protected bool _isVulnerable;

		protected bool _bypassClaim;

		protected bool _isRepairable;

		protected bool _proofExplosion;

		protected bool _isUnpickupable;

		protected bool _isSalvageable;

		protected bool _isSaveable;
	}
}
