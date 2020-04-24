using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class ItemMagazineAsset : ItemCaliberAsset
	{
		public ItemMagazineAsset(Bundle bundle, Data data, Local localization, ushort id) : base(bundle, data, localization, id)
		{
			this._magazine = (GameObject)bundle.load("Magazine");
			this._pellets = data.readByte("Pellets");
			if (this.pellets < 1)
			{
				this._pellets = 1;
			}
			this._stuck = data.readByte("Stuck");
			this._range = data.readSingle("Range");
			this._playerDamage = data.readSingle("Player_Damage");
			this._zombieDamage = data.readSingle("Zombie_Damage");
			this._animalDamage = data.readSingle("Animal_Damage");
			this._barricadeDamage = data.readSingle("Barricade_Damage");
			this._structureDamage = data.readSingle("Structure_Damage");
			this._vehicleDamage = data.readSingle("Vehicle_Damage");
			this._resourceDamage = data.readSingle("Resource_Damage");
			this._explosion = data.readUInt16("Explosion");
			if (data.has("Object_Damage"))
			{
				this._objectDamage = data.readSingle("Object_Damage");
			}
			else
			{
				this._objectDamage = this.resourceDamage;
			}
			this._tracer = data.readUInt16("Tracer");
			this._impact = data.readUInt16("Impact");
			this._speed = data.readSingle("Speed");
			if (this.speed < 0.01f)
			{
				this._speed = 1f;
			}
			this._isExplosive = data.has("Explosive");
			this._deleteEmpty = data.has("Delete_Empty");
			bundle.unload();
		}

		public GameObject magazine
		{
			get
			{
				return this._magazine;
			}
		}

		public byte pellets
		{
			get
			{
				return this._pellets;
			}
		}

		public byte stuck
		{
			get
			{
				return this._stuck;
			}
		}

		public float range
		{
			get
			{
				return this._range;
			}
		}

		public float playerDamage
		{
			get
			{
				return this._playerDamage;
			}
		}

		public float zombieDamage
		{
			get
			{
				return this._zombieDamage;
			}
		}

		public float animalDamage
		{
			get
			{
				return this._animalDamage;
			}
		}

		public float barricadeDamage
		{
			get
			{
				return this._barricadeDamage;
			}
		}

		public float structureDamage
		{
			get
			{
				return this._structureDamage;
			}
		}

		public float vehicleDamage
		{
			get
			{
				return this._vehicleDamage;
			}
		}

		public float resourceDamage
		{
			get
			{
				return this._resourceDamage;
			}
		}

		public float objectDamage
		{
			get
			{
				return this._objectDamage;
			}
		}

		public ushort explosion
		{
			get
			{
				return this._explosion;
			}
		}

		public ushort tracer
		{
			get
			{
				return this._tracer;
			}
		}

		public ushort impact
		{
			get
			{
				return this._impact;
			}
		}

		public override bool showQuality
		{
			get
			{
				return this.stuck > 0;
			}
		}

		public float speed
		{
			get
			{
				return this._speed;
			}
		}

		public bool isExplosive
		{
			get
			{
				return this._isExplosive;
			}
		}

		public bool deleteEmpty
		{
			get
			{
				return this._deleteEmpty;
			}
		}

		protected GameObject _magazine;

		private byte _pellets;

		private byte _stuck;

		protected float _range;

		protected float _playerDamage;

		protected float _zombieDamage;

		protected float _animalDamage;

		protected float _barricadeDamage;

		protected float _structureDamage;

		protected float _vehicleDamage;

		protected float _resourceDamage;

		protected float _objectDamage;

		private ushort _explosion;

		private ushort _tracer;

		private ushort _impact;

		private float _speed;

		protected bool _isExplosive;

		private bool _deleteEmpty;
	}
}
