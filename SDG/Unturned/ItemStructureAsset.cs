using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class ItemStructureAsset : ItemAsset
	{
		public ItemStructureAsset(Bundle bundle, Data data, Local localization, ushort id) : base(bundle, data, localization, id)
		{
			if (!Dedicator.isDedicated)
			{
				this._structure = (GameObject)bundle.load("Structure");
			}
			this._clip = (GameObject)bundle.load("Clip");
			this._nav = (GameObject)bundle.load("Nav");
			this._use = (AudioClip)bundle.load("Use");
			this._construct = (EConstruct)Enum.Parse(typeof(EConstruct), data.readString("Construct"), true);
			this._health = data.readUInt16("Health");
			this._range = data.readSingle("Range");
			this._explosion = data.readUInt16("Explosion");
			this._isVulnerable = data.has("Vulnerable");
			this._isRepairable = !data.has("Unrepairable");
			this._proofExplosion = data.has("Proof_Explosion");
			this._isUnpickupable = data.has("Unpickupable");
			this._isSalvageable = !data.has("Unsalvageable");
			this._isSaveable = !data.has("Unsaveable");
			bundle.unload();
		}

		public GameObject structure
		{
			get
			{
				return this._structure;
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

		public EConstruct construct
		{
			get
			{
				return this._construct;
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

		public ushort explosion
		{
			get
			{
				return this._explosion;
			}
		}

		public bool isVulnerable
		{
			get
			{
				return this._isVulnerable;
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

		protected GameObject _structure;

		protected GameObject _clip;

		protected GameObject _nav;

		protected AudioClip _use;

		protected EConstruct _construct;

		protected ushort _health;

		protected float _range;

		protected ushort _explosion;

		protected bool _isVulnerable;

		protected bool _isRepairable;

		protected bool _proofExplosion;

		protected bool _isUnpickupable;

		protected bool _isSalvageable;

		protected bool _isSaveable;
	}
}
