using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class VehicleAsset : Asset
	{
		public VehicleAsset(Bundle bundle, Data data, Local localization, ushort id) : base(bundle, data, localization, id)
		{
			if (id < 200 && !bundle.hasResource && !data.has("Bypass_ID_Limit"))
			{
				throw new NotSupportedException("ID < 200");
			}
			this._vehicleName = localization.format("Name");
			this._vehicle = (GameObject)bundle.load("Vehicle");
			if (this.vehicle == null)
			{
				throw new NotSupportedException("Missing vehicle gameobject");
			}
			this._clip = (GameObject)bundle.load("Clip");
			if (data.has("Engine"))
			{
				this._engine = (EEngine)Enum.Parse(typeof(EEngine), data.readString("Engine"), true);
				if (this.engine == EEngine.BOAT)
				{
					if (this.vehicle.transform.FindChild("Buoyancy") == null)
					{
						this._engine = EEngine.CAR;
					}
				}
				else if (this.engine != EEngine.CAR && this.vehicle.transform.FindChild("Rotors") == null)
				{
					this._engine = EEngine.CAR;
				}
			}
			else
			{
				this._engine = EEngine.CAR;
			}
			if (data.has("Rarity"))
			{
				this._rarity = (EItemRarity)Enum.Parse(typeof(EItemRarity), data.readString("Rarity"), true);
			}
			else
			{
				this._rarity = EItemRarity.COMMON;
			}
			this._hasHeadlights = (this.vehicle.transform.FindChild("Headlights") != null);
			this._hasSirens = (this.vehicle.transform.FindChild("Sirens") != null);
			this._hasHook = (this.vehicle.transform.FindChild("Hook") != null);
			this._hasZip = data.has("Zip");
			this.isReclined = data.has("Reclined");
			this._hasCrawler = data.has("Crawler");
			this._hasLockMouse = data.has("LockMouse");
			this._hasTraction = data.has("Traction");
			this._hasSleds = data.has("Sleds");
			this._ignition = (AudioClip)bundle.load("Ignition");
			this._horn = (AudioClip)bundle.load("Horn");
			if (this.clip == null)
			{
				Assets.errors.Add(this.vehicleName + " is missing collision data. Highly recommended to fix.");
			}
			if (this.vehicle != null && this.vehicle.transform.FindChild("Seats") == null)
			{
				Assets.errors.Add(this.vehicleName + " vehicle is missing seats.");
			}
			if (this.clip != null && this.clip.transform.FindChild("Seats") == null)
			{
				Assets.errors.Add(this.vehicleName + " clip is missing seats.");
			}
			if (data.has("Pitch_Idle"))
			{
				this._pitchIdle = data.readSingle("Pitch_Idle");
			}
			else
			{
				this._pitchIdle = 0.5f;
				AudioSource component = this.vehicle.GetComponent<AudioSource>();
				if (component != null)
				{
					AudioClip clip = component.clip;
					if (clip != null)
					{
						if (clip.name == "Engine_Large")
						{
							this._pitchIdle = 0.625f;
						}
						else if (clip.name == "Engine_Small")
						{
							this._pitchIdle = 0.75f;
						}
					}
					else
					{
						Assets.errors.Add(this.vehicleName + " missing engine audio!");
					}
				}
			}
			if (data.has("Pitch_Drive"))
			{
				this._pitchDrive = data.readSingle("Pitch_Drive");
			}
			else
			{
				this._pitchDrive = 0.05f;
				AudioSource component2 = this.vehicle.GetComponent<AudioSource>();
				if (component2 != null)
				{
					AudioClip clip2 = component2.clip;
					if (clip2 != null)
					{
						if (clip2.name == "Engine_Large")
						{
							this._pitchDrive = 0.025f;
						}
						else if (clip2.name == "Engine_Small")
						{
							this._pitchDrive = 0.075f;
						}
					}
					else
					{
						Assets.errors.Add(this.vehicleName + " missing engine audio!");
					}
				}
			}
			this._speedMin = data.readSingle("Speed_Min");
			this._speedMax = data.readSingle("Speed_Max") * 1.25f;
			this._steerMin = data.readSingle("Steer_Min");
			this._steerMax = data.readSingle("Steer_Max") * 0.75f;
			this._brake = data.readSingle("Brake");
			this._lift = data.readSingle("Lift");
			this._fuelMin = data.readUInt16("Fuel_Min");
			this._fuelMax = data.readUInt16("Fuel_Max");
			this._fuel = data.readUInt16("Fuel");
			this._healthMin = data.readUInt16("Health_Min");
			this._healthMax = data.readUInt16("Health_Max");
			this._health = data.readUInt16("Health");
			this._explosion = data.readUInt16("Explosion");
			if (data.has("Exit"))
			{
				this._exit = data.readSingle("Exit");
			}
			else
			{
				this._exit = 2f;
			}
			if (data.has("Cam_Follow_Distance"))
			{
				this._camFollowDistance = data.readSingle("Cam_Follow_Distance");
			}
			else
			{
				this._camFollowDistance = 5.5f;
			}
			this._camDriverOffset = data.readSingle("Cam_Driver_Offset");
			if (data.has("Bumper_Multiplier"))
			{
				this._bumperMultiplier = data.readSingle("Bumper_Multiplier");
			}
			else
			{
				this._bumperMultiplier = 1f;
			}
			if (data.has("Passenger_Explosion_Armor"))
			{
				this._passengerExplosionArmor = data.readSingle("Passenger_Explosion_Armor");
			}
			else
			{
				this._passengerExplosionArmor = 1f;
			}
			if (this.engine == EEngine.HELICOPTER)
			{
				this._sqrDelta = Mathf.Pow(this.speedMax * 0.125f, 2f);
			}
			else
			{
				this._sqrDelta = Mathf.Pow(this.speedMax * 0.1f, 2f);
			}
			this._turrets = new TurretInfo[(int)data.readByte("Turrets")];
			byte b = 0;
			while ((int)b < this.turrets.Length)
			{
				TurretInfo turretInfo = new TurretInfo();
				turretInfo.seatIndex = data.readByte("Turret_" + b + "_Seat_Index");
				turretInfo.itemID = data.readUInt16("Turret_" + b + "_Item_ID");
				turretInfo.yawMin = data.readSingle("Turret_" + b + "_Yaw_Min");
				turretInfo.yawMax = data.readSingle("Turret_" + b + "_Yaw_Max");
				turretInfo.pitchMin = data.readSingle("Turret_" + b + "_Pitch_Min");
				turretInfo.pitchMax = data.readSingle("Turret_" + b + "_Pitch_Max");
				turretInfo.useAimCamera = !data.has("Turret_" + b + "_Ignore_Aim_Camera");
				turretInfo.aimOffset = data.readSingle("Turret_" + b + "_Aim_Offset");
				this._turrets[(int)b] = turretInfo;
				b += 1;
			}
			this._isVulnerable = !data.has("Invulnerable");
			this.canTiresBeDamaged = !data.has("Tires_Invulnerable");
			if (data.has("Air_Turn_Responsiveness"))
			{
				this.airTurnResponsiveness = data.readSingle("Air_Turn_Responsiveness");
			}
			else
			{
				this.airTurnResponsiveness = 2f;
			}
			if (data.has("Air_Steer_Min"))
			{
				this.airSteerMin = data.readSingle("Air_Steer_Min");
			}
			else
			{
				this.airSteerMin = this.steerMin;
			}
			if (data.has("Air_Steer_Max"))
			{
				this.airSteerMax = data.readSingle("Air_Steer_Max");
			}
			else
			{
				this.airSteerMax = this.steerMax;
			}
			bundle.unload();
			this._shouldVerifyHash = !data.has("Bypass_Hash_Verification");
		}

		public bool shouldVerifyHash
		{
			get
			{
				return this._shouldVerifyHash;
			}
		}

		public string vehicleName
		{
			get
			{
				return this._vehicleName;
			}
		}

		public EEngine engine
		{
			get
			{
				return this._engine;
			}
		}

		public EItemRarity rarity
		{
			get
			{
				return this._rarity;
			}
		}

		public GameObject vehicle
		{
			get
			{
				return this._vehicle;
			}
		}

		public GameObject clip
		{
			get
			{
				return this._clip;
			}
		}

		public AudioClip ignition
		{
			get
			{
				return this._ignition;
			}
		}

		public AudioClip horn
		{
			get
			{
				return this._horn;
			}
		}

		public float pitchIdle
		{
			get
			{
				return this._pitchIdle;
			}
		}

		public float pitchDrive
		{
			get
			{
				return this._pitchDrive;
			}
		}

		public float speedMin
		{
			get
			{
				return this._speedMin;
			}
		}

		public float speedMax
		{
			get
			{
				return this._speedMax;
			}
		}

		public float steerMin
		{
			get
			{
				return this._steerMin;
			}
		}

		public float steerMax
		{
			get
			{
				return this._steerMax;
			}
		}

		public float brake
		{
			get
			{
				return this._brake;
			}
		}

		public float lift
		{
			get
			{
				return this._lift;
			}
		}

		public ushort fuelMin
		{
			get
			{
				return this._fuelMin;
			}
		}

		public ushort fuelMax
		{
			get
			{
				return this._fuelMax;
			}
		}

		public ushort fuel
		{
			get
			{
				return this._fuel;
			}
		}

		public ushort healthMin
		{
			get
			{
				return this._healthMin;
			}
		}

		public ushort healthMax
		{
			get
			{
				return this._healthMax;
			}
		}

		public ushort health
		{
			get
			{
				return this._health;
			}
		}

		public ushort explosion
		{
			get
			{
				return this._explosion;
			}
		}

		public bool hasHeadlights
		{
			get
			{
				return this._hasHeadlights;
			}
		}

		public bool hasSirens
		{
			get
			{
				return this._hasSirens;
			}
		}

		public bool hasHook
		{
			get
			{
				return this._hasHook;
			}
		}

		public bool hasZip
		{
			get
			{
				return this._hasZip;
			}
		}

		public bool isReclined { get; protected set; }

		public bool hasCrawler
		{
			get
			{
				return this._hasCrawler;
			}
		}

		public bool hasLockMouse
		{
			get
			{
				return this._hasLockMouse;
			}
		}

		public bool hasTraction
		{
			get
			{
				return this._hasTraction;
			}
		}

		public bool hasSleds
		{
			get
			{
				return this._hasSleds;
			}
		}

		public float exit
		{
			get
			{
				return this._exit;
			}
		}

		public float sqrDelta
		{
			get
			{
				return this._sqrDelta;
			}
		}

		public float camFollowDistance
		{
			get
			{
				return this._camFollowDistance;
			}
		}

		public float camDriverOffset
		{
			get
			{
				return this._camDriverOffset;
			}
		}

		public float bumperMultiplier
		{
			get
			{
				return this._bumperMultiplier;
			}
		}

		public float passengerExplosionArmor
		{
			get
			{
				return this._passengerExplosionArmor;
			}
		}

		public TurretInfo[] turrets
		{
			get
			{
				return this._turrets;
			}
		}

		public bool isVulnerable
		{
			get
			{
				return this._isVulnerable;
			}
		}

		public bool canTiresBeDamaged { get; protected set; }

		public float airTurnResponsiveness { get; protected set; }

		public float airSteerMin { get; protected set; }

		public float airSteerMax { get; protected set; }

		public override EAssetType assetCategory
		{
			get
			{
				return EAssetType.VEHICLE;
			}
		}

		protected bool _shouldVerifyHash;

		protected string _vehicleName;

		protected EEngine _engine;

		protected EItemRarity _rarity;

		protected GameObject _vehicle;

		protected GameObject _clip;

		protected AudioClip _ignition;

		protected AudioClip _horn;

		protected float _pitchIdle;

		protected float _pitchDrive;

		protected float _speedMin;

		protected float _speedMax;

		protected float _steerMin;

		protected float _steerMax;

		protected float _brake;

		protected float _lift;

		protected ushort _fuelMin;

		protected ushort _fuelMax;

		protected ushort _fuel;

		protected ushort _healthMin;

		protected ushort _healthMax;

		protected ushort _health;

		protected ushort _explosion;

		protected bool _hasHeadlights;

		protected bool _hasSirens;

		protected bool _hasHook;

		protected bool _hasZip;

		protected bool _hasCrawler;

		protected bool _hasLockMouse;

		protected bool _hasTraction;

		protected bool _hasSleds;

		protected float _exit;

		protected float _sqrDelta;

		protected float _camFollowDistance;

		protected float _camDriverOffset;

		protected float _bumperMultiplier;

		protected float _passengerExplosionArmor;

		protected TurretInfo[] _turrets;

		protected bool _isVulnerable;
	}
}
