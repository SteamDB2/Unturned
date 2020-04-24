using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class AnimalAsset : Asset
	{
		public AnimalAsset(Bundle bundle, Data data, Local localization, ushort id) : base(bundle, data, localization, id)
		{
			if (id < 50 && !bundle.hasResource && !data.has("Bypass_ID_Limit"))
			{
				throw new NotSupportedException("ID < 50");
			}
			this._animalName = localization.format("Name");
			this._client = (GameObject)bundle.load("Animal_Client");
			this._server = (GameObject)bundle.load("Animal_Server");
			this._dedicated = (GameObject)bundle.load("Animal_Dedicated");
			this._ragdoll = (GameObject)bundle.load("Ragdoll");
			if (this.client == null)
			{
				Assets.errors.Add(this.animalName + " is missing client data. Highly recommended to fix.");
			}
			if (this.server == null)
			{
				Assets.errors.Add(this.animalName + " is missing server data. Highly recommended to fix.");
			}
			if (this.dedicated == null)
			{
				Assets.errors.Add(this.animalName + " is missing dedicated data. Highly recommended to fix.");
			}
			if (this.ragdoll == null)
			{
				Assets.errors.Add(this.animalName + " is missing ragdoll data. Highly recommended to fix.");
			}
			this._speedRun = data.readSingle("Speed_Run");
			this._speedWalk = data.readSingle("Speed_Walk");
			this._behaviour = (EAnimalBehaviour)Enum.Parse(typeof(EAnimalBehaviour), data.readString("Behaviour"), true);
			this._health = data.readUInt16("Health");
			this._regen = data.readSingle("Regen");
			if (!data.has("Regen"))
			{
				this._regen = 10f;
			}
			this._damage = data.readByte("Damage");
			this._meat = data.readUInt16("Meat");
			this._pelt = data.readUInt16("Pelt");
			this._rewardID = data.readUInt16("Reward_ID");
			if (data.has("Reward_Min"))
			{
				this._rewardMin = data.readByte("Reward_Min");
			}
			else
			{
				this._rewardMin = 3;
			}
			if (data.has("Reward_Max"))
			{
				this._rewardMax = data.readByte("Reward_Max");
			}
			else
			{
				this._rewardMax = 4;
			}
			this._roars = new AudioClip[(int)data.readByte("Roars")];
			byte b = 0;
			while ((int)b < this.roars.Length)
			{
				this.roars[(int)b] = (AudioClip)bundle.load("Roar_" + b);
				b += 1;
			}
			this._panics = new AudioClip[(int)data.readByte("Panics")];
			byte b2 = 0;
			while ((int)b2 < this.panics.Length)
			{
				this.panics[(int)b2] = (AudioClip)bundle.load("Panic_" + b2);
				b2 += 1;
			}
			this._rewardXP = data.readUInt32("Reward_XP");
			bundle.unload();
		}

		public string animalName
		{
			get
			{
				return this._animalName;
			}
		}

		public GameObject client
		{
			get
			{
				return this._client;
			}
		}

		public GameObject server
		{
			get
			{
				return this._server;
			}
		}

		public GameObject dedicated
		{
			get
			{
				return this._dedicated;
			}
		}

		public GameObject ragdoll
		{
			get
			{
				return this._ragdoll;
			}
		}

		public float speedRun
		{
			get
			{
				return this._speedRun;
			}
		}

		public float speedWalk
		{
			get
			{
				return this._speedWalk;
			}
		}

		public EAnimalBehaviour behaviour
		{
			get
			{
				return this._behaviour;
			}
		}

		public ushort health
		{
			get
			{
				return this._health;
			}
		}

		public uint rewardXP
		{
			get
			{
				return this._rewardXP;
			}
		}

		public float regen
		{
			get
			{
				return this._regen;
			}
		}

		public byte damage
		{
			get
			{
				return this._damage;
			}
		}

		public ushort meat
		{
			get
			{
				return this._meat;
			}
		}

		public ushort pelt
		{
			get
			{
				return this._pelt;
			}
		}

		public byte rewardMin
		{
			get
			{
				return this._rewardMin;
			}
		}

		public byte rewardMax
		{
			get
			{
				return this._rewardMax;
			}
		}

		public ushort rewardID
		{
			get
			{
				return this._rewardID;
			}
		}

		public AudioClip[] roars
		{
			get
			{
				return this._roars;
			}
		}

		public AudioClip[] panics
		{
			get
			{
				return this._panics;
			}
		}

		public override EAssetType assetCategory
		{
			get
			{
				return EAssetType.ANIMAL;
			}
		}

		protected string _animalName;

		protected GameObject _client;

		protected GameObject _server;

		protected GameObject _dedicated;

		protected GameObject _ragdoll;

		protected float _speedRun;

		protected float _speedWalk;

		private EAnimalBehaviour _behaviour;

		protected ushort _health;

		protected uint _rewardXP;

		protected float _regen;

		protected byte _damage;

		protected ushort _meat;

		protected ushort _pelt;

		private byte _rewardMin;

		private byte _rewardMax;

		private ushort _rewardID;

		protected AudioClip[] _roars;

		protected AudioClip[] _panics;
	}
}
