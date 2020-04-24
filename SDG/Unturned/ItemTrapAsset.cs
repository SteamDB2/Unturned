using System;

namespace SDG.Unturned
{
	public class ItemTrapAsset : ItemBarricadeAsset
	{
		public ItemTrapAsset(Bundle bundle, Data data, Local localization, ushort id) : base(bundle, data, localization, id)
		{
			this._range2 = data.readSingle("Range2");
			this._playerDamage = data.readSingle("Player_Damage");
			this._zombieDamage = data.readSingle("Zombie_Damage");
			this._animalDamage = data.readSingle("Animal_Damage");
			this._barricadeDamage = data.readSingle("Barricade_Damage");
			this._structureDamage = data.readSingle("Structure_Damage");
			this._vehicleDamage = data.readSingle("Vehicle_Damage");
			this._resourceDamage = data.readSingle("Resource_Damage");
			if (data.has("Object_Damage"))
			{
				this._objectDamage = data.readSingle("Object_Damage");
			}
			else
			{
				this._objectDamage = this.resourceDamage;
			}
			this._explosion2 = data.readUInt16("Explosion2");
			this._isBroken = data.has("Broken");
			this._isExplosive = data.has("Explosive");
			this.damageTires = data.has("Damage_Tires");
		}

		public float range2
		{
			get
			{
				return this._range2;
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

		public ushort explosion2
		{
			get
			{
				return this._explosion2;
			}
		}

		public bool isBroken
		{
			get
			{
				return this._isBroken;
			}
		}

		public bool isExplosive
		{
			get
			{
				return this._isExplosive;
			}
		}

		public bool damageTires { get; protected set; }

		protected float _range2;

		protected float _playerDamage;

		protected float _zombieDamage;

		protected float _animalDamage;

		protected float _barricadeDamage;

		protected float _structureDamage;

		protected float _vehicleDamage;

		protected float _resourceDamage;

		protected float _objectDamage;

		private ushort _explosion2;

		protected bool _isBroken;

		protected bool _isExplosive;
	}
}
