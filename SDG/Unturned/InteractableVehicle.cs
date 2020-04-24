using System;
using System.Collections.Generic;
using SDG.Framework.Devkit;
using SDG.Framework.Landscapes;
using SDG.Framework.Utilities;
using SDG.Framework.Water;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	public class InteractableVehicle : Interactable, ILandscaleHoleVolumeInteractionHandler
	{
		public event VehiclePassengersUpdated onPassengersUpdated;

		public event VehicleLockUpdated onLockUpdated;

		public event VehicleHeadlightsUpdated onHeadlightsUpdated;

		public event VehicleTaillightsUpdated onTaillightsUpdated;

		public event VehicleSirensUpdated onSirensUpdated;

		public event VehicleBatteryChangedHandler batteryChanged;

		public bool isEngineOn { get; protected set; }

		public bool hasBattery
		{
			get
			{
				return this.batteryCharge > 0;
			}
		}

		public bool isBatteryFull
		{
			get
			{
				return this.batteryCharge >= 10000;
			}
		}

		public bool landscapeHoleAutoIgnoreTerrainCollision
		{
			get
			{
				return true;
			}
		}

		public void landscapeHoleBeginCollision(LandscapeHoleVolume volume, List<TerrainCollider> terrainColliders)
		{
			foreach (Wheel wheel in this.tires)
			{
				if (!(wheel.wheel == null))
				{
					foreach (TerrainCollider terrainCollider in terrainColliders)
					{
						Physics.IgnoreCollision(wheel.wheel, terrainCollider, true);
					}
				}
			}
		}

		public void landscapeHoleEndCollision(LandscapeHoleVolume volume, List<TerrainCollider> terrainColliders)
		{
			foreach (Wheel wheel in this.tires)
			{
				if (!(wheel.wheel == null))
				{
					foreach (TerrainCollider terrainCollider in terrainColliders)
					{
						Physics.IgnoreCollision(wheel.wheel, terrainCollider, false);
					}
				}
			}
		}

		public bool canUseHorn
		{
			get
			{
				return Time.realtimeSinceStartup - this.horned > 0.5f && this.hasBattery;
			}
		}

		public bool canTurnOnLights
		{
			get
			{
				return this.hasBattery && !this.isUnderwater;
			}
		}

		public bool isRefillable
		{
			get
			{
				return this.fuel < this.asset.fuel && !this.isDriven && !this.isExploded;
			}
		}

		public bool isSiphonable
		{
			get
			{
				return this.fuel > 0 && !this.isDriven && !this.isExploded;
			}
		}

		public bool isRepaired
		{
			get
			{
				return this.health == this.asset.health;
			}
		}

		public bool isDriven
		{
			get
			{
				return this.passengers != null && this.passengers[0].player != null;
			}
		}

		public bool isDriver
		{
			get
			{
				return !Dedicator.isDedicated && this.checkDriver(Provider.client);
			}
		}

		public bool isEmpty
		{
			get
			{
				byte b = 0;
				while ((int)b < this.passengers.Length)
				{
					if (this.passengers[(int)b].player != null)
					{
						return false;
					}
					b += 1;
				}
				return true;
			}
		}

		public bool isDrowned
		{
			get
			{
				return this._isDrowned;
			}
		}

		public bool isUnderwater
		{
			get
			{
				if (this.waterCenterTransform != null)
				{
					return WaterUtility.isPointUnderwater(this.waterCenterTransform.position);
				}
				return WaterUtility.isPointUnderwater(base.transform.position + new Vector3(0f, 1f, 0f));
			}
		}

		public bool isBatteryReplaceable
		{
			get
			{
				return !this.isBatteryFull && !this.isDriven && !this.isExploded;
			}
		}

		public bool isTireReplaceable
		{
			get
			{
				return !this.isDriven && !this.isExploded && this.asset.canTiresBeDamaged;
			}
		}

		public bool isGoingToRespawn
		{
			get
			{
				return this.isExploded || this.isDrowned;
			}
		}

		public bool isAutoClearable
		{
			get
			{
				return this.isExploded || (this.isUnderwater && this.buoyancy == null) || (this.asset != null && this.asset.engine == EEngine.BOAT && this.fuel == 0);
			}
		}

		public float lastDead
		{
			get
			{
				return this._lastDead;
			}
		}

		public float lastUnderwater
		{
			get
			{
				return this._lastUnderwater;
			}
		}

		public float lastExploded
		{
			get
			{
				return this._lastExploded;
			}
		}

		public float slip
		{
			get
			{
				return this._slip;
			}
		}

		public bool isDead
		{
			get
			{
				return this.health == 0;
			}
		}

		public float factor
		{
			get
			{
				return this._factor;
			}
		}

		public float speed
		{
			get
			{
				return this._speed;
			}
		}

		public float physicsSpeed
		{
			get
			{
				return this._physicsSpeed;
			}
		}

		public float spedometer
		{
			get
			{
				return this._spedometer;
			}
		}

		public int turn
		{
			get
			{
				return this._turn;
			}
		}

		public float steer
		{
			get
			{
				return this._steer;
			}
		}

		public Transform sirens
		{
			get
			{
				return this._sirens;
			}
		}

		public bool sirensOn
		{
			get
			{
				return this._sirensOn;
			}
		}

		public Transform headlights
		{
			get
			{
				return this._headlights;
			}
		}

		public bool headlightsOn
		{
			get
			{
				return this._headlightsOn;
			}
		}

		public Transform taillights
		{
			get
			{
				return this._taillights;
			}
		}

		public bool taillightsOn
		{
			get
			{
				return this._taillightsOn;
			}
		}

		public CSteamID lockedOwner
		{
			get
			{
				return this._lockedOwner;
			}
		}

		public CSteamID lockedGroup
		{
			get
			{
				return this._lockedGroup;
			}
		}

		public bool isLocked
		{
			get
			{
				return this._isLocked;
			}
		}

		public VehicleAsset asset
		{
			get
			{
				return this._asset;
			}
		}

		public Passenger[] passengers
		{
			get
			{
				return this._passengers;
			}
		}

		public Passenger[] turrets
		{
			get
			{
				return this._turrets;
			}
		}

		public Wheel[] tires { get; protected set; }

		public void replaceBattery(Player player, byte quality)
		{
			this.giveBatteryItem(player);
			VehicleManager.sendVehicleBatteryCharge(this, (ushort)(quality * 100));
		}

		public void stealBattery(Player player)
		{
			if (!this.giveBatteryItem(player))
			{
				return;
			}
			VehicleManager.sendVehicleBatteryCharge(this, 0);
		}

		protected bool giveBatteryItem(Player player)
		{
			byte b = (byte)Mathf.FloorToInt((float)this.batteryCharge / 100f);
			if (b == 0)
			{
				return false;
			}
			Item item = new Item(1450, 1, b);
			player.inventory.forceAddItem(item, false);
			return true;
		}

		public byte tireAliveMask
		{
			get
			{
				int num = 0;
				byte b = 0;
				while ((int)b < Mathf.Min(8, this.tires.Length))
				{
					if (this.tires[(int)b].isAlive)
					{
						int num2 = 1 << (int)b;
						num |= num2;
					}
					b += 1;
				}
				return (byte)num;
			}
			set
			{
				byte b = 0;
				while ((int)b < Mathf.Min(8, this.tires.Length))
				{
					if (!(this.tires[(int)b].wheel == null))
					{
						int num = 1 << (int)b;
						this.tires[(int)b].isAlive = (((int)value & num) == num);
					}
					b += 1;
				}
			}
		}

		public void sendTireAliveMaskUpdate()
		{
			VehicleManager.sendVehicleTireAliveMask(this, this.tireAliveMask);
		}

		public void askRepairTire(int index)
		{
			if (index < 0)
			{
				return;
			}
			this.tires[index].askRepair();
		}

		public void askDamageTire(int index)
		{
			if (index < 0)
			{
				return;
			}
			if (this.asset != null && !this.asset.canTiresBeDamaged)
			{
				return;
			}
			this.tires[index].askDamage();
		}

		public int getHitTireIndex(Vector3 position)
		{
			for (int i = 0; i < this.tires.Length; i++)
			{
				WheelCollider wheelCollider = this.tires[i].wheel;
				if (!(wheelCollider == null))
				{
					if ((wheelCollider.transform.position - position).sqrMagnitude < wheelCollider.radius * wheelCollider.radius)
					{
						return i;
					}
				}
			}
			return -1;
		}

		public int getClosestAliveTireIndex(Vector3 position, bool isAlive)
		{
			int result = -1;
			float num = 16f;
			for (int i = 0; i < this.tires.Length; i++)
			{
				if (this.tires[i].isAlive == isAlive)
				{
					if (!(this.tires[i].wheel == null))
					{
						float sqrMagnitude = (this.tires[i].wheel.transform.position - position).sqrMagnitude;
						if (sqrMagnitude < num)
						{
							result = i;
							num = sqrMagnitude;
						}
					}
				}
			}
			return result;
		}

		public void askBurnFuel(ushort amount)
		{
			if (amount == 0 || this.isExploded)
			{
				return;
			}
			if (amount >= this.fuel)
			{
				this.fuel = 0;
			}
			else
			{
				this.fuel -= amount;
			}
		}

		public void askFillFuel(ushort amount)
		{
			if (amount == 0 || this.isExploded)
			{
				return;
			}
			if (amount >= this.asset.fuel - this.fuel)
			{
				this.fuel = this.asset.fuel;
			}
			else
			{
				this.fuel += amount;
			}
			VehicleManager.sendVehicleFuel(this, this.fuel);
		}

		public void askBurnBattery(ushort amount)
		{
			if (amount == 0 || this.isExploded)
			{
				return;
			}
			if (amount >= this.batteryCharge)
			{
				this.batteryCharge = 0;
			}
			else
			{
				this.batteryCharge -= amount;
			}
		}

		public void askChargeBattery(ushort amount)
		{
			if (amount == 0 || this.isExploded)
			{
				return;
			}
			if (amount >= 10000 - this.batteryCharge)
			{
				this.batteryCharge = 10000;
			}
			else
			{
				this.batteryCharge += amount;
			}
		}

		public void sendBatteryChargeUpdate()
		{
			VehicleManager.sendVehicleBatteryCharge(this, this.batteryCharge);
		}

		public void askDamage(ushort amount, bool canRepair)
		{
			if (amount == 0)
			{
				return;
			}
			if (this.isDead)
			{
				if (!canRepair)
				{
					this.explode();
				}
				return;
			}
			if (amount >= this.health)
			{
				this.health = 0;
			}
			else
			{
				this.health -= amount;
			}
			VehicleManager.sendVehicleHealth(this, this.health);
			if (this.isDead && !canRepair)
			{
				this.explode();
			}
		}

		public void askRepair(ushort amount)
		{
			if (amount == 0 || this.isExploded)
			{
				return;
			}
			if (amount >= this.asset.health - this.health)
			{
				this.health = this.asset.health;
			}
			else
			{
				this.health += amount;
			}
			VehicleManager.sendVehicleHealth(this, this.health);
		}

		private void explode()
		{
			base.GetComponent<Rigidbody>().AddForce(0f, 1024f, 0f);
			base.GetComponent<Rigidbody>().AddTorque(16f, 0f, 0f);
			DamageTool.explode(base.transform.position, 8f, EDeathCause.VEHICLE, CSteamID.Nil, 200f, 200f, 200f, 0f, 0f, 500f, 2000f, 500f, EExplosionDamageType.CONVENTIONAL, 32f, true);
			for (int i = 0; i < this.passengers.Length; i++)
			{
				Passenger passenger = this.passengers[i];
				if (this.passengers != null)
				{
					SteamPlayer player = passenger.player;
					if (player != null)
					{
						Player player2 = player.player;
						if (!(player2 == null))
						{
							if (!player2.life.isDead)
							{
								EPlayerKill eplayerKill;
								player2.life.askDamage(101, Vector3.up * 101f, EDeathCause.VEHICLE, ELimb.SPINE, CSteamID.Nil, out eplayerKill);
							}
						}
					}
				}
			}
			int num = Random.Range(3, 7);
			for (int j = 0; j < num; j++)
			{
				float num2 = Random.Range(0f, 6.28318548f);
				ItemManager.dropItem(new Item(67, EItemOrigin.NATURE), base.transform.position + new Vector3(Mathf.Sin(num2) * 3f, 1f, Mathf.Cos(num2) * 3f), false, Dedicator.isDedicated, true);
			}
			VehicleManager.sendVehicleExploded(this);
			EffectManager.sendEffect(this.asset.explosion, EffectManager.LARGE, base.transform.position);
		}

		public bool checkEnter(CSteamID enemyPlayer, CSteamID enemyGroup)
		{
			return !this.isHooked && ((Provider.isServer && !Dedicator.isDedicated) || !this.isLocked || enemyPlayer == this.lockedOwner || (this.lockedGroup != CSteamID.Nil && enemyGroup == this.lockedGroup));
		}

		public override bool checkUseable()
		{
			return !this.isExploded && this.checkEnter(Provider.client, Player.player.quests.groupID);
		}

		public override void use()
		{
			VehicleManager.enterVehicle(this);
		}

		public override bool checkHighlight(out Color color)
		{
			color = ItemTool.getRarityColorHighlight(this.asset.rarity);
			return true;
		}

		public override bool checkHint(out EPlayerMessage message, out string text, out Color color)
		{
			if (this.checkUseable())
			{
				message = EPlayerMessage.VEHICLE_ENTER;
				text = this.asset.vehicleName;
				color = ItemTool.getRarityColorUI(this.asset.rarity);
			}
			else
			{
				message = EPlayerMessage.LOCKED;
				text = string.Empty;
				color = Color.white;
			}
			return !this.isExploded;
		}

		public void updateVehicle()
		{
			this.lastUpdatedPos = base.transform.position;
			if (this.nsb != null)
			{
				this.nsb.updateLastSnapshot(new TransformSnapshotInfo(base.transform.position, base.transform.rotation));
			}
			this.real = base.transform.position;
			this.isRecovering = false;
			this.lastRecover = Time.realtimeSinceStartup;
			this.isFrozen = false;
		}

		public void updatePhysics()
		{
			if (this.checkDriver(Provider.client) || (Provider.isServer && !this.isDriven))
			{
				base.GetComponent<Rigidbody>().useGravity = true;
				base.GetComponent<Rigidbody>().isKinematic = false;
				this.isPhysical = true;
				if (!this.isExploded)
				{
					for (int i = 0; i < this.tires.Length; i++)
					{
						this.tires[i].isPhysical = true;
					}
					if (this.buoyancy != null)
					{
						this.buoyancy.gameObject.SetActive(true);
					}
				}
			}
			else
			{
				base.GetComponent<Rigidbody>().useGravity = false;
				base.GetComponent<Rigidbody>().isKinematic = true;
				this.isPhysical = false;
				for (int j = 0; j < this.tires.Length; j++)
				{
					this.tires[j].isPhysical = false;
				}
				if (this.buoyancy != null)
				{
					this.buoyancy.gameObject.SetActive(false);
				}
			}
			Transform transform = base.transform.FindChild("Cog");
			if (transform)
			{
				base.GetComponent<Rigidbody>().centerOfMass = transform.localPosition;
			}
			else
			{
				base.GetComponent<Rigidbody>().centerOfMass = new Vector3(0f, -0.25f, 0f);
			}
		}

		public void updateEngine()
		{
			this.tellTaillights(this.isDriven && this.canTurnOnLights);
			if (!Dedicator.isDedicated && this.sirens != null && this.sirens.GetComponent<AudioSource>() != null)
			{
				this.sirens.GetComponent<AudioSource>().enabled = this.isDriven;
			}
		}

		public void tellLocked(CSteamID owner, CSteamID group, bool locked)
		{
			this._lockedOwner = owner;
			this._lockedGroup = group;
			this._isLocked = locked;
			if (this.onLockUpdated != null)
			{
				this.onLockUpdated();
			}
		}

		public void tellSirens(bool on)
		{
			this._sirensOn = on;
			if (!Dedicator.isDedicated)
			{
				if (this.sirens != null)
				{
					this.sirens.gameObject.SetActive(this.sirensOn);
				}
				if (this.sirenMaterials != null)
				{
					for (int i = 0; i < this.sirenMaterials.Length; i++)
					{
						if (this.sirenMaterials[i] != null)
						{
							this.sirenMaterials[i].SetColor("_EmissionColor", Color.black);
						}
					}
				}
			}
			if (this.onSirensUpdated != null)
			{
				this.onSirensUpdated();
			}
		}

		public void tellHeadlights(bool on)
		{
			this._headlightsOn = on;
			if (!Dedicator.isDedicated)
			{
				if (this.headlights != null)
				{
					this.headlights.gameObject.SetActive(this.headlightsOn);
				}
				if (this.headlightsMaterial != null)
				{
					this.headlightsMaterial.SetColor("_EmissionColor", (!this.headlightsOn) ? Color.black : this.headlightsMaterial.color);
				}
			}
			if (this.onHeadlightsUpdated != null)
			{
				this.onHeadlightsUpdated();
			}
		}

		public void tellTaillights(bool on)
		{
			this._taillightsOn = on;
			if (!Dedicator.isDedicated)
			{
				if (this.taillights != null)
				{
					this.taillights.gameObject.SetActive(this.taillightsOn);
				}
				if (this.taillightsMaterial != null)
				{
					this.taillightsMaterial.SetColor("_EmissionColor", (!this.taillightsOn) ? Color.black : this.taillightsMaterial.color);
				}
				else if (this.taillightMaterials != null)
				{
					for (int i = 0; i < this.taillightMaterials.Length; i++)
					{
						if (this.taillightMaterials[i] != null)
						{
							this.taillightMaterials[i].SetColor("_EmissionColor", (!this.taillightsOn) ? Color.black : this.taillightMaterials[i].color);
						}
					}
				}
			}
			if (this.onTaillightsUpdated != null)
			{
				this.onTaillightsUpdated();
			}
		}

		public void tellHorn()
		{
			this.horned = Time.realtimeSinceStartup;
			if (!Dedicator.isDedicated && this.clipAudioSource != null)
			{
				this.clipAudioSource.pitch = 1f;
				this.clipAudioSource.PlayOneShot(this.asset.horn);
			}
			if (Provider.isServer)
			{
				AlertTool.alert(base.transform.position, 32f);
			}
		}

		public void tellFuel(ushort newFuel)
		{
			this.fuel = newFuel;
		}

		public void tellBatteryCharge(ushort newBatteryCharge)
		{
			this.batteryCharge = newBatteryCharge;
			if (this.batteryCharge == 0)
			{
				this.isEngineOn = false;
			}
			if (this.batteryChanged != null)
			{
				this.batteryChanged();
			}
		}

		public void tellExploded()
		{
			this.clearHooked();
			this.isExploded = true;
			this._lastExploded = Time.realtimeSinceStartup;
			if (this.sirensOn)
			{
				this.tellSirens(false);
			}
			if (this.headlightsOn)
			{
				this.tellHeadlights(false);
			}
			for (int i = 0; i < this.tires.Length; i++)
			{
				this.tires[i].isPhysical = false;
			}
			if (this.buoyancy != null)
			{
				this.buoyancy.gameObject.SetActive(false);
			}
			if (!Dedicator.isDedicated)
			{
				HighlighterTool.color(base.transform, new Color(0.25f, 0.25f, 0.25f));
				this.updateFires();
				for (int j = 0; j < this.tires.Length; j++)
				{
					if (!(this.tires[j].model == null))
					{
						this.tires[j].model.transform.parent = Level.effects;
						this.tires[j].model.GetComponent<Collider>().enabled = true;
						Rigidbody orAddComponent = this.tires[j].model.gameObject.getOrAddComponent<Rigidbody>();
						orAddComponent.interpolation = 1;
						orAddComponent.collisionDetectionMode = 0;
						orAddComponent.drag = 0.5f;
						orAddComponent.angularDrag = 0.1f;
						Object.Destroy(this.tires[j].model.gameObject, 8f);
						if (j % 2 == 0)
						{
							orAddComponent.AddForce(-this.tires[j].model.right * 512f + Vector3.up * 128f);
						}
						else
						{
							orAddComponent.AddForce(this.tires[j].model.right * 512f + Vector3.up * 128f);
						}
					}
				}
				if (this.rotors != null)
				{
					for (int k = 0; k < this.rotors.Length; k++)
					{
						Object.Destroy(this.rotors[k].prop.gameObject);
					}
				}
				if (this.exhausts != null)
				{
					for (int l = 0; l < this.exhausts.Length; l++)
					{
						this.exhausts[l].emission.rateOverTime = 0f;
					}
				}
				if (this.turrets != null)
				{
					for (int m = 0; m < this.turrets.Length; m++)
					{
						HighlighterTool.color(this.turrets[m].turretYaw, new Color(0.25f, 0.25f, 0.25f));
						HighlighterTool.color(this.turrets[m].turretPitch, new Color(0.25f, 0.25f, 0.25f));
					}
				}
			}
		}

		public void updateFires()
		{
			if (!Dedicator.isDedicated)
			{
				this.fire.gameObject.SetActive((this.isExploded || this.isDead) && !this.isUnderwater);
				this.smoke_0.gameObject.SetActive((this.isExploded || this.health < InteractableVehicle.HEALTH_0) && !this.isUnderwater);
				this.smoke_1.gameObject.SetActive((this.isExploded || this.health < InteractableVehicle.HEALTH_1) && !this.isUnderwater);
			}
		}

		public void tellHealth(ushort newHealth)
		{
			this.health = newHealth;
			if (this.isDead)
			{
				this._lastDead = Time.realtimeSinceStartup;
			}
			this.updateFires();
		}

		public void tellRecov(Vector3 newPosition, int newRecov)
		{
			this.lastTick = Time.realtimeSinceStartup;
			base.GetComponent<Rigidbody>().MovePosition(newPosition);
			this.isFrozen = true;
			base.GetComponent<Rigidbody>().useGravity = false;
			base.GetComponent<Rigidbody>().isKinematic = true;
			if (this.passengers[0] != null && this.passengers[0].player != null && this.passengers[0].player.player != null && this.passengers[0].player.player.input != null)
			{
				this.passengers[0].player.player.input.recov = newRecov;
			}
		}

		public void tellState(Vector3 newPosition, byte newAngle_X, byte newAngle_Y, byte newAngle_Z, byte newSpeed, byte newPhysicsSpeed, byte newTurn)
		{
			if (this.isDriver)
			{
				return;
			}
			this.lastTick = Time.realtimeSinceStartup;
			this.lastUpdatedPos = newPosition;
			if (this.nsb != null)
			{
				this.nsb.addNewSnapshot(new TransformSnapshotInfo(newPosition, Quaternion.Euler(MeasurementTool.byteToAngle2(newAngle_X), MeasurementTool.byteToAngle2(newAngle_Y), MeasurementTool.byteToAngle2(newAngle_Z))));
			}
			this._speed = (float)(newSpeed - 128);
			this._physicsSpeed = (float)(newPhysicsSpeed - 128);
			this._turn = (int)(newTurn - 1);
		}

		public bool checkDriver(CSteamID steamID)
		{
			return this.isDriven && this.passengers[0].player.playerID.steamID == steamID;
		}

		public void addPlayer(byte seat, CSteamID steamID)
		{
			SteamPlayer steamPlayer = PlayerTool.getSteamPlayer(steamID);
			if (steamPlayer != null)
			{
				this.passengers[(int)seat].player = steamPlayer;
				if (steamPlayer.player != null)
				{
					steamPlayer.player.movement.setVehicle(this, seat, this.passengers[(int)seat].seat, Vector3.zero, 0, false);
					if (this.passengers[(int)seat].turret != null)
					{
						steamPlayer.player.equipment.turretEquipClient();
						if (Provider.isServer)
						{
							steamPlayer.player.equipment.turretEquipServer(this.passengers[(int)seat].turret.itemID, this.passengers[(int)seat].state);
						}
					}
				}
				this.updatePhysics();
			}
			if (seat == 0)
			{
				this.isEngineOn = (this.hasBattery && !this.isUnderwater);
			}
			this.updateEngine();
			if (seat == 0 && this.fuel > 0 && !Dedicator.isDedicated && !this.isUnderwater)
			{
				if (this.clipAudioSource != null && this.isEngineOn)
				{
					this.clipAudioSource.pitch = Random.Range(0.9f, 1.1f);
					this.clipAudioSource.PlayOneShot(this.asset.ignition);
				}
				if (this.engineAudioSource != null)
				{
					this.engineAudioSource.pitch = 0.5f;
				}
			}
			if (this.onPassengersUpdated != null)
			{
				this.onPassengersUpdated();
			}
		}

		public void removePlayer(byte seat, Vector3 point, byte angle, bool forceUpdate)
		{
			if ((int)seat < this.passengers.Length)
			{
				SteamPlayer player = this.passengers[(int)seat].player;
				if (player.player != null)
				{
					if (this.passengers[(int)seat].turret != null)
					{
						player.player.equipment.turretDequipClient();
						if (Provider.isServer)
						{
							player.player.equipment.turretDequipServer();
						}
					}
					player.player.movement.setVehicle(null, 0, LevelPlayers.models, point, angle, forceUpdate);
				}
				this.passengers[(int)seat].player = null;
				this.updatePhysics();
				if (Provider.isServer)
				{
					VehicleManager.sendVehicleFuel(this, this.fuel);
					VehicleManager.sendVehicleBatteryCharge(this, this.batteryCharge);
				}
			}
			if (seat == 0)
			{
				this.isEngineOn = false;
			}
			this.updateEngine();
			if (seat == 0)
			{
				this.altSpeedInput = 0f;
				this.altSpeedOutput = 0f;
				if (!Dedicator.isDedicated)
				{
					if (this.engineAudioSource != null)
					{
						this.engineAudioSource.volume = 0f;
					}
					if (this.windZone != null)
					{
						this.windZone.windMain = 0f;
					}
				}
				for (int i = 0; i < this.tires.Length; i++)
				{
					this.tires[i].reset();
				}
			}
			if (this.onPassengersUpdated != null)
			{
				this.onPassengersUpdated();
			}
		}

		public void swapPlayer(byte fromSeat, byte toSeat)
		{
			if ((int)fromSeat < this.passengers.Length && (int)toSeat < this.passengers.Length)
			{
				SteamPlayer player = this.passengers[(int)fromSeat].player;
				if (player.player != null)
				{
					if (this.passengers[(int)fromSeat].turret != null)
					{
						player.player.equipment.turretDequipClient();
						if (Provider.isServer)
						{
							player.player.equipment.turretDequipServer();
						}
					}
					player.player.movement.setVehicle(this, toSeat, this.passengers[(int)toSeat].seat, Vector3.zero, 0, false);
					if (this.passengers[(int)toSeat].turret != null)
					{
						player.player.equipment.turretEquipClient();
						if (Provider.isServer)
						{
							player.player.equipment.turretEquipServer(this.passengers[(int)toSeat].turret.itemID, this.passengers[(int)toSeat].state);
						}
					}
				}
				this.passengers[(int)fromSeat].player = null;
				this.passengers[(int)toSeat].player = player;
				this.updatePhysics();
				if (Provider.isServer)
				{
					VehicleManager.sendVehicleFuel(this, this.fuel);
					VehicleManager.sendVehicleBatteryCharge(this, this.batteryCharge);
				}
			}
			if (toSeat == 0)
			{
				this.isEngineOn = (this.hasBattery && !this.isUnderwater);
			}
			if (fromSeat == 0)
			{
				this.isEngineOn = false;
			}
			this.updateEngine();
			if (fromSeat == 0)
			{
				this.altSpeedInput = 0f;
				this.altSpeedOutput = 0f;
				if (!Dedicator.isDedicated)
				{
					if (this.engineAudioSource != null)
					{
						this.engineAudioSource.volume = 0f;
					}
					if (this.windZone != null)
					{
						this.windZone.windMain = 0f;
					}
				}
				for (int i = 0; i < this.tires.Length; i++)
				{
					this.tires[i].reset();
				}
			}
			if (this.onPassengersUpdated != null)
			{
				this.onPassengersUpdated();
			}
		}

		public bool tryAddPlayer(out byte seat, Player player)
		{
			seat = byte.MaxValue;
			if (this.isExploded)
			{
				return false;
			}
			if (!this.isExitable)
			{
				return false;
			}
			byte b = (player.animator.gesture != EPlayerGesture.ARREST_START) ? 0 : 1;
			while ((int)b < this.passengers.Length)
			{
				if (this.passengers[(int)b] != null && this.passengers[(int)b].player == null)
				{
					seat = b;
					return true;
				}
				b += 1;
			}
			return false;
		}

		public bool forceRemovePlayer(out byte seat, CSteamID player, out Vector3 point, out byte angle)
		{
			seat = byte.MaxValue;
			point = Vector3.zero;
			angle = 0;
			byte b = 0;
			while ((int)b < this.passengers.Length)
			{
				if (this.passengers[(int)b] != null && this.passengers[(int)b].player != null && this.passengers[(int)b].player.playerID.steamID == player)
				{
					seat = b;
					this.getExit(seat, out point, out angle);
					return true;
				}
				b += 1;
			}
			return false;
		}

		public bool tryRemovePlayer(out byte seat, CSteamID player, out Vector3 point, out byte angle)
		{
			seat = byte.MaxValue;
			point = Vector3.zero;
			angle = 0;
			byte b = 0;
			while ((int)b < this.passengers.Length)
			{
				if (this.passengers[(int)b] != null && this.passengers[(int)b].player != null && this.passengers[(int)b].player.playerID.steamID == player)
				{
					seat = b;
					return this.getExit(seat, out point, out angle);
				}
				b += 1;
			}
			return false;
		}

		public bool trySwapPlayer(Player player, byte toSeat, out byte fromSeat)
		{
			fromSeat = byte.MaxValue;
			if ((int)toSeat >= this.passengers.Length)
			{
				return false;
			}
			if (player.animator.gesture == EPlayerGesture.ARREST_START && toSeat < 1)
			{
				return false;
			}
			byte b = 0;
			while ((int)b < this.passengers.Length)
			{
				if (this.passengers[(int)b] != null && this.passengers[(int)b].player != null && this.passengers[(int)b].player.player == player)
				{
					if (toSeat != b)
					{
						fromSeat = b;
						return this.passengers[(int)toSeat].player == null;
					}
					return false;
				}
				else
				{
					b += 1;
				}
			}
			return false;
		}

		public bool isExitable
		{
			get
			{
				Vector3 vector;
				byte b;
				return this.getExit(0, out vector, out b);
			}
		}

		protected bool isExitSafe(Vector3 point, Vector3 direction, float distance)
		{
			RaycastHit raycastHit;
			PhysicsUtility.raycast(new Ray(point, direction), out raycastHit, distance, RayMasks.BLOCK_EXIT, 0);
			return raycastHit.transform == null || (raycastHit.transform.IsChildOf(base.transform) && this.isExitSafe(raycastHit.point + direction * 0.01f, direction, distance - raycastHit.distance - 0.01f));
		}

		protected Vector3 getExitGroundPoint(Vector3 exitPoint)
		{
			RaycastHit raycastHit;
			PhysicsUtility.raycast(new Ray(exitPoint, Vector3.down), out raycastHit, 3f, RayMasks.BLOCK_EXIT, 0);
			if (raycastHit.transform != null)
			{
				exitPoint = raycastHit.point;
			}
			return exitPoint + new Vector3(0f, 0.5f, 0f);
		}

		protected bool getExitSidePoint(float side, ref Vector3 point)
		{
			float num = PlayerStance.RADIUS + 0.1f;
			float num2 = this.asset.exit + Mathf.Abs(this.speed) * 0.1f + num;
			if (this.isExitSafe(this.center.position + this.center.up, this.center.right * side, num2))
			{
				point = this.getExitGroundPoint(this.center.position + this.center.up + this.center.right * side * (num2 - num));
				return true;
			}
			side = -side;
			if (this.isExitSafe(this.center.position + this.center.up, this.center.right * side, num2))
			{
				point = this.getExitGroundPoint(this.center.position + this.center.up + this.center.right * side * (num2 - num));
				return true;
			}
			return false;
		}

		public bool getExit(byte seat, out Vector3 point, out byte angle)
		{
			point = this.center.position;
			angle = MeasurementTool.angleToByte(this.center.rotation.eulerAngles.y);
			if (seat % 2 == 0)
			{
				if (this.getExitSidePoint(-1f, ref point))
				{
					return true;
				}
			}
			else if (this.getExitSidePoint(1f, ref point))
			{
				return true;
			}
			float num = PlayerMovement.HEIGHT_STAND + 0.1f;
			float num2 = this.asset.exit + Mathf.Abs(this.speed) * 0.1f + num;
			if (this.isExitSafe(this.center.position, this.center.up, num2))
			{
				point = this.getExitGroundPoint(this.center.position + this.center.up * (num2 - num));
				return true;
			}
			if (this.isExitSafe(this.center.position, -this.center.up, num2))
			{
				point = this.getExitGroundPoint(this.center.position - this.center.up * (num2 - num));
				return true;
			}
			return true;
		}

		public void simulate(uint simulation, int recov, Vector3 point, Quaternion angle, float newSpeed, float newPhysicsSpeed, int newTurn, float delta)
		{
			if (this.isRecovering)
			{
				if (recov < this.passengers[0].player.player.input.recov)
				{
					if (Time.realtimeSinceStartup - this.lastRecover > 5f)
					{
						this.lastRecover = Time.realtimeSinceStartup;
						VehicleManager.sendVehicleRecov(this, this.real, this.passengers[0].player.player.input.recov);
					}
					return;
				}
				this.isRecovering = false;
				this.isFrozen = false;
			}
			bool flag = Dedicator.serverVisibility == ESteamServerVisibility.LAN || PlayerMovement.forceTrustClient;
			if (!flag)
			{
				if (this.asset.engine == EEngine.CAR)
				{
					if (Mathf.Pow(point.x - this.real.x, 2f) + Mathf.Pow(point.z - this.real.z, 2f) > ((this.fuel != 0) ? this.asset.sqrDelta : 0.5f))
					{
						this.isRecovering = true;
						this.lastRecover = Time.realtimeSinceStartup;
						this.passengers[0].player.player.input.recov++;
						VehicleManager.sendVehicleRecov(this, this.real, this.passengers[0].player.player.input.recov);
						return;
					}
					if (point.y - this.real.y > 1f)
					{
						this.isRecovering = true;
						this.lastRecover = Time.realtimeSinceStartup;
						this.passengers[0].player.player.input.recov++;
						VehicleManager.sendVehicleRecov(this, this.real, this.passengers[0].player.player.input.recov);
						return;
					}
				}
				else if (this.asset.engine == EEngine.BOAT)
				{
					if (Mathf.Pow(point.x - this.real.x, 2f) + Mathf.Pow(point.z - this.real.z, 2f) > ((!WaterUtility.isPointUnderwater(point + new Vector3(0f, -4f, 0f))) ? 0.5f : this.asset.sqrDelta))
					{
						this.isRecovering = true;
						this.lastRecover = Time.realtimeSinceStartup;
						this.passengers[0].player.player.input.recov++;
						VehicleManager.sendVehicleRecov(this, this.real, this.passengers[0].player.player.input.recov);
						return;
					}
					if (point.y - this.real.y > 0.25f)
					{
						this.isRecovering = true;
						this.lastRecover = Time.realtimeSinceStartup;
						this.passengers[0].player.player.input.recov++;
						VehicleManager.sendVehicleRecov(this, this.real, this.passengers[0].player.player.input.recov);
						return;
					}
				}
				else if (Mathf.Pow(point.x - this.real.x, 2f) + Mathf.Pow(point.z - this.real.z, 2f) > this.asset.sqrDelta)
				{
					this.isRecovering = true;
					this.lastRecover = Time.realtimeSinceStartup;
					this.passengers[0].player.player.input.recov++;
					VehicleManager.sendVehicleRecov(this, this.real, this.passengers[0].player.player.input.recov);
					return;
				}
			}
			if (this.asset.engine == EEngine.CAR)
			{
				if (simulation - this.lastBurnFuel > 5u)
				{
					this.lastBurnFuel = simulation;
					this.askBurnFuel(1);
				}
			}
			else if (simulation - this.lastBurnFuel > 2u)
			{
				this.lastBurnFuel = simulation;
				this.askBurnFuel(1);
			}
			this._speed = newSpeed;
			this._physicsSpeed = newSpeed;
			this._turn = newTurn;
			base.GetComponent<Rigidbody>().MovePosition(point);
			base.GetComponent<Rigidbody>().MoveRotation(angle);
			this.real = point;
			if (this.updates != null && (Mathf.Abs(this.lastUpdatedPos.x - this.real.x) > Provider.UPDATE_DISTANCE || Mathf.Abs(this.lastUpdatedPos.y - this.real.y) > Provider.UPDATE_DISTANCE || Mathf.Abs(this.lastUpdatedPos.z - this.real.z) > Provider.UPDATE_DISTANCE))
			{
				this.lastUpdatedPos = this.real;
				this.updates.Add(new VehicleStateUpdate(point, angle));
			}
		}

		public void clearHooked()
		{
			foreach (HookInfo hookInfo in this.hooked)
			{
				if (!(hookInfo.vehicle == null))
				{
					hookInfo.vehicle.isHooked = false;
				}
			}
			this.hooked.Clear();
		}

		public void useHook()
		{
			if (this.hooked.Count > 0)
			{
				this.clearHooked();
			}
			else
			{
				int num = Physics.OverlapSphereNonAlloc(this.hook.position, 3f, InteractableVehicle.grab, RayMasks.VEHICLE);
				for (int i = 0; i < num; i++)
				{
					InteractableVehicle vehicle = DamageTool.getVehicle(InteractableVehicle.grab[i].transform);
					if (!(vehicle == null) && !(vehicle == this) && vehicle.isEmpty && !vehicle.isHooked)
					{
						HookInfo hookInfo = new HookInfo();
						hookInfo.target = vehicle.transform;
						hookInfo.vehicle = vehicle;
						hookInfo.deltaPosition = this.hook.InverseTransformPoint(vehicle.transform.position);
						hookInfo.deltaRotation = Quaternion.FromToRotation(this.hook.forward, vehicle.transform.forward);
						this.hooked.Add(hookInfo);
						vehicle.isHooked = true;
					}
				}
			}
		}

		public void simulate(uint simulation, int recov, int input_x, int input_y, float look_x, float look_y, bool inputBrake, float delta)
		{
			if (this.isFrozen)
			{
				this.isFrozen = false;
				base.GetComponent<Rigidbody>().useGravity = true;
				base.GetComponent<Rigidbody>().isKinematic = false;
				return;
			}
			if (this.fuel == 0 || this.isUnderwater || this.isDead || !this.isEngineOn)
			{
				input_y = 0;
			}
			this._factor = Mathf.InverseLerp(0f, (this.speed >= 0f) ? this.asset.speedMax : this.asset.speedMin, this.speed);
			bool flag = false;
			for (int i = 0; i < this.tires.Length; i++)
			{
				this.tires[i].simulate((float)input_x, (float)input_y, inputBrake, delta);
				if (this.tires[i].isGrounded)
				{
					flag = true;
				}
			}
			switch (this.asset.engine)
			{
			case EEngine.CAR:
				if (flag)
				{
					base.GetComponent<Rigidbody>().AddForce(-base.transform.up * this.factor * 40f);
				}
				if (this.buoyancy != null)
				{
					float num = Mathf.Lerp(this.asset.steerMax, this.asset.steerMin, this.factor);
					this.speedTraction = Mathf.Lerp(this.speedTraction, (float)((!WaterUtility.isPointUnderwater(base.transform.position + new Vector3(0f, -1f, 0f))) ? 0 : 1), 4f * Time.deltaTime);
					if (input_y > 0)
					{
						this.altSpeedInput = Mathf.Lerp(this.altSpeedInput, this.asset.speedMax, delta / 4f);
					}
					else if (input_y < 0)
					{
						this.altSpeedInput = Mathf.Lerp(this.altSpeedInput, this.asset.speedMin, delta / 4f);
					}
					else
					{
						this.altSpeedInput = Mathf.Lerp(this.altSpeedInput, 0f, delta / 8f);
					}
					this.altSpeedOutput = this.altSpeedInput * this.speedTraction;
					Vector3 forward = base.transform.forward;
					forward.y = 0f;
					base.GetComponent<Rigidbody>().AddForce(forward.normalized * this.altSpeedOutput * 2f * this.speedTraction);
					base.GetComponent<Rigidbody>().AddRelativeTorque((float)input_y * -2.5f * this.speedTraction, (float)input_x * num / 8f * this.speedTraction, (float)input_x * -2.5f * this.speedTraction);
				}
				break;
			case EEngine.PLANE:
			{
				float num2 = Mathf.Lerp(this.asset.airSteerMax, this.asset.airSteerMin, this.factor);
				if (input_y > 0)
				{
					this.altSpeedInput = Mathf.Lerp(this.altSpeedInput, this.asset.speedMax, delta);
				}
				else if (input_y < 0)
				{
					this.altSpeedInput = Mathf.Lerp(this.altSpeedInput, 0f, delta / 8f);
				}
				else
				{
					this.altSpeedInput = Mathf.Lerp(this.altSpeedInput, 0f, delta / 16f);
				}
				this.altSpeedOutput = this.altSpeedInput;
				base.GetComponent<Rigidbody>().AddForce(base.transform.forward * this.altSpeedOutput * 2f);
				base.GetComponent<Rigidbody>().AddForce(Mathf.Lerp(0f, 1f, base.transform.InverseTransformDirection(base.GetComponent<Rigidbody>().velocity).z / this.asset.speedMax) * this.asset.lift * -Physics.gravity);
				if (this.tires.Length == 0 || (!this.tires[0].isGrounded && !this.tires[1].isGrounded))
				{
					base.GetComponent<Rigidbody>().AddRelativeTorque(Mathf.Clamp(look_y, -this.asset.airTurnResponsiveness, this.asset.airTurnResponsiveness) * num2, (float)input_x * this.asset.airTurnResponsiveness * num2 / 4f, Mathf.Clamp(look_x, -this.asset.airTurnResponsiveness, this.asset.airTurnResponsiveness) * -num2 / 2f);
				}
				if (this.tires.Length == 0 && input_y < 0)
				{
					base.GetComponent<Rigidbody>().AddForce(base.transform.forward * this.asset.speedMin * 4f);
				}
				break;
			}
			case EEngine.HELICOPTER:
			{
				float num3 = Mathf.Lerp(this.asset.steerMax, this.asset.steerMin, this.factor);
				if (input_y > 0)
				{
					this.altSpeedInput = Mathf.Lerp(this.altSpeedInput, this.asset.speedMax, delta / 4f);
				}
				else if (input_y < 0)
				{
					this.altSpeedInput = Mathf.Lerp(this.altSpeedInput, 0f, delta / 8f);
				}
				else
				{
					this.altSpeedInput = Mathf.Lerp(this.altSpeedInput, 0f, delta / 16f);
				}
				this.altSpeedOutput = this.altSpeedInput;
				base.GetComponent<Rigidbody>().AddForce(base.transform.up * this.altSpeedOutput * 3f);
				base.GetComponent<Rigidbody>().AddRelativeTorque(Mathf.Clamp(look_y, -2f, 2f) * num3, (float)input_x * num3 / 2f, Mathf.Clamp(look_x, -2f, 2f) * -num3 / 4f);
				break;
			}
			case EEngine.BOAT:
			{
				float num4 = Mathf.Lerp(this.asset.steerMax, this.asset.steerMin, this.factor);
				this.speedTraction = Mathf.Lerp(this.speedTraction, (float)((!WaterUtility.isPointUnderwater(base.transform.position + new Vector3(0f, -1f, 0f))) ? 0 : 1), 4f * Time.deltaTime);
				if (input_y > 0)
				{
					this.altSpeedInput = Mathf.Lerp(this.altSpeedInput, this.asset.speedMax, delta / 4f);
				}
				else if (input_y < 0)
				{
					this.altSpeedInput = Mathf.Lerp(this.altSpeedInput, this.asset.speedMin, delta / 4f);
				}
				else
				{
					this.altSpeedInput = Mathf.Lerp(this.altSpeedInput, 0f, delta / 8f);
				}
				this.altSpeedOutput = this.altSpeedInput * this.speedTraction;
				Vector3 forward2 = base.transform.forward;
				forward2.y = 0f;
				base.GetComponent<Rigidbody>().AddForce(forward2.normalized * this.altSpeedOutput * 4f * this.speedTraction);
				if (this.tires.Length == 0 || (!this.tires[0].isGrounded && !this.tires[1].isGrounded))
				{
					base.GetComponent<Rigidbody>().AddRelativeTorque((float)input_y * -10f * this.speedTraction, (float)input_x * num4 / 2f * this.speedTraction, (float)input_x * -5f * this.speedTraction);
				}
				break;
			}
			}
			if (this.asset.engine == EEngine.CAR)
			{
				this._speed = base.transform.InverseTransformDirection(base.GetComponent<Rigidbody>().velocity).z;
				this._physicsSpeed = this._speed;
			}
			else
			{
				this._speed = this.altSpeedOutput;
				this._physicsSpeed = base.transform.InverseTransformDirection(base.GetComponent<Rigidbody>().velocity).z;
			}
			this._turn = input_x;
			if (this.asset.engine == EEngine.CAR)
			{
				if (simulation - this.lastBurnFuel > 5u)
				{
					this.lastBurnFuel = simulation;
					this.askBurnFuel(1);
				}
			}
			else if (simulation - this.lastBurnFuel > 2u)
			{
				this.lastBurnFuel = simulation;
				this.askBurnFuel(1);
			}
			this.lastUpdatedPos = base.transform.position;
			if (this.nsb != null)
			{
				this.nsb.updateLastSnapshot(new TransformSnapshotInfo(base.transform.position, base.transform.rotation));
			}
		}

		private void Update()
		{
			if (this.asset == null)
			{
				return;
			}
			float deltaTime = Time.deltaTime;
			if (Provider.isServer && this.hooked != null)
			{
				for (int i = 0; i < this.hooked.Count; i++)
				{
					HookInfo hookInfo = this.hooked[i];
					if (hookInfo != null && !(hookInfo.target == null))
					{
						hookInfo.target.position = this.hook.TransformPoint(hookInfo.deltaPosition);
						hookInfo.target.rotation = this.hook.rotation * hookInfo.deltaRotation;
					}
				}
			}
			if (Dedicator.isDedicated)
			{
				if (this.isPhysical && this.updates != null && this.updates.Count == 0 && (Mathf.Abs(this.lastUpdatedPos.x - base.transform.position.x) > Provider.UPDATE_DISTANCE || Mathf.Abs(this.lastUpdatedPos.y - base.transform.position.y) > Provider.UPDATE_DISTANCE || Mathf.Abs(this.lastUpdatedPos.z - base.transform.position.z) > Provider.UPDATE_DISTANCE))
				{
					this.lastUpdatedPos = base.transform.position;
					this.updates.Add(new VehicleStateUpdate(base.transform.position, base.transform.rotation));
				}
			}
			else
			{
				this._steer = Mathf.Lerp(this.steer, (float)this.turn * this.asset.steerMax, 4f * deltaTime);
				this._spedometer = Mathf.Lerp(this.spedometer, this.speed, 4f * deltaTime);
				if (!this.isExploded)
				{
					if (this.isDriven)
					{
						this.fly += (this.spedometer + 8f) * 89f * Time.deltaTime;
					}
					this.spin += this.spedometer * 45f * Time.deltaTime;
					for (int j = 0; j < this.tires.Length; j++)
					{
						if (!(this.tires[j].model == null))
						{
							if (j < ((this.asset.engine != EEngine.CAR) ? 1 : 2) && !this.asset.hasCrawler)
							{
								this.tires[j].model.localRotation = Quaternion.Euler(this.spin, this.steer, 0f);
							}
							else
							{
								this.tires[j].model.localRotation = Quaternion.Euler(this.spin, 0f, 0f);
							}
						}
					}
					if (this.front != null)
					{
						this.front.localRotation = Quaternion.Euler(-90f, 180f, 0f);
						this.front.transform.Rotate(0f, 0f, this.steer, 1);
					}
					if (this.rotors != null)
					{
						for (int k = 0; k < this.rotors.Length; k++)
						{
							Rotor rotor = this.rotors[k];
							if (rotor == null || rotor.prop == null || rotor.material_0 == null || rotor.material_1 == null)
							{
								break;
							}
							rotor.prop.localRotation = rotor.rest * Quaternion.Euler(0f, this.fly, 0f);
							Color color = rotor.material_0.color;
							if (this.asset.engine == EEngine.PLANE)
							{
								color.a = Mathf.Lerp(1f, 0f, (this.spedometer - 16f) / 8f);
							}
							else
							{
								color.a = Mathf.Lerp(1f, 0f, (this.spedometer - 8f) / 8f);
							}
							rotor.material_0.color = color;
							color.a = (1f - color.a) * 0.25f;
							rotor.material_1.color = color;
						}
					}
					float num = Mathf.Max(0f, Mathf.InverseLerp(0f, this.asset.speedMax, this.physicsSpeed));
					if (this.exhausts != null)
					{
						for (int l = 0; l < this.exhausts.Length; l++)
						{
							this.exhausts[l].emission.rateOverTime = (float)this.exhausts[l].main.maxParticles * num;
						}
					}
					if (this.wheel != null)
					{
						this.wheel.transform.localRotation = this.rest;
						this.wheel.transform.Rotate(0f, -this.steer, 0f, 1);
					}
				}
				if (this.isDriven && !this.isUnderwater)
				{
					if (this.asset.engine == EEngine.HELICOPTER)
					{
						if (this.engineAudioSource != null)
						{
							this.engineAudioSource.pitch = Mathf.Lerp(this.engineAudioSource.pitch, 0.5f + Mathf.Abs(this.spedometer) * 0.03f, 2f * deltaTime);
							this.engineAudioSource.volume = Mathf.Lerp(this.engineAudioSource.volume, (this.fuel <= 0 || !this.isEngineOn) ? 0f : (0.25f + Mathf.Abs(this.spedometer) * 0.03f), 0.125f * deltaTime);
						}
						if (this.windZone != null)
						{
							this.windZone.windMain = Mathf.Lerp(this.windZone.windMain, (this.fuel <= 0) ? 0f : (Mathf.Abs(this.spedometer) * 0.1f), 0.125f * deltaTime);
						}
					}
					else if (this.engineAudioSource != null)
					{
						this.engineAudioSource.pitch = Mathf.Lerp(this.engineAudioSource.pitch, this.asset.pitchIdle + Mathf.Abs(this.spedometer) * this.asset.pitchDrive, 2f * deltaTime);
						this.engineAudioSource.volume = Mathf.Lerp(this.engineAudioSource.volume, (this.fuel <= 0 || !this.isEngineOn) ? 0f : 0.75f, 2f * deltaTime);
					}
				}
			}
			if (!Provider.isServer && !this.isPhysical && this.nsb != null)
			{
				TransformSnapshotInfo transformSnapshotInfo = (TransformSnapshotInfo)this.nsb.getCurrentSnapshot();
				base.GetComponent<Rigidbody>().MovePosition(transformSnapshotInfo.pos);
				base.GetComponent<Rigidbody>().MoveRotation(transformSnapshotInfo.rot);
			}
			if (this.headlightsOn && !this.canTurnOnLights)
			{
				this.tellHeadlights(false);
			}
			if (this.taillightsOn && !this.canTurnOnLights)
			{
				this.tellTaillights(false);
			}
			if (this.sirensOn && !this.canTurnOnLights)
			{
				this.tellSirens(false);
			}
			if (this.isUnderwater)
			{
				if (!this.isDrowned)
				{
					this._lastUnderwater = Time.realtimeSinceStartup;
					this._isDrowned = true;
					this.tellSirens(false);
					this.tellHeadlights(false);
					this.updateFires();
					if (!Dedicator.isDedicated)
					{
						if (this.engineAudioSource != null)
						{
							this.engineAudioSource.volume = 0f;
						}
						if (this.windZone != null)
						{
							this.windZone.windMain = 0f;
						}
					}
				}
			}
			else if (this._isDrowned)
			{
				this._isDrowned = false;
				this.updateFires();
			}
			if (this.isDriver)
			{
				if (!this.asset.hasTraction)
				{
					bool flag = LevelLighting.isPositionSnowy(base.transform.position);
					AmbianceVolume ambianceVolume;
					if (!flag && Level.info.configData.Use_Snow_Volumes && AmbianceUtility.isPointInsideVolume(base.transform.position, out ambianceVolume))
					{
						flag = ambianceVolume.canSnow;
					}
					flag &= (LevelLighting.snowyness == ELightingSnow.BLIZZARD);
					this._slip = Mathf.Lerp(this._slip, (float)((!flag) ? 0 : 1), Time.deltaTime * 0.05f);
				}
				else
				{
					this._slip = 0f;
				}
				if (this.tires != null)
				{
					for (int m = 0; m < this.tires.Length; m++)
					{
						if (this.tires[m] == null)
						{
							break;
						}
						this.tires[m].update(deltaTime);
					}
				}
			}
			if (Provider.isServer)
			{
				if (this.isDriven)
				{
					if (this.tires != null)
					{
						for (int n = 0; n < this.tires.Length; n++)
						{
							if (this.tires[n] == null)
							{
								break;
							}
							this.tires[n].checkForTraps();
						}
					}
				}
				else
				{
					this._speed = base.transform.InverseTransformDirection(base.GetComponent<Rigidbody>().velocity).z;
					this._physicsSpeed = this._speed;
					this._turn = 0;
					this.real = base.transform.position;
				}
				if (this.isDead && !this.isExploded && !this.isUnderwater && Time.realtimeSinceStartup - this.lastDead > InteractableVehicle.EXPLODE)
				{
					this.explode();
				}
			}
			if (!Provider.isServer && !this.isPhysical && Time.realtimeSinceStartup - this.lastTick > Provider.UPDATE_TIME * 2f)
			{
				this.lastTick = Time.realtimeSinceStartup;
				this._speed = 0f;
				this._physicsSpeed = 0f;
				this._turn = 0;
			}
			if (this.sirensOn && !Dedicator.isDedicated && Time.realtimeSinceStartup - this.lastWeeoo > 0.33f && this.sirens != null)
			{
				this.lastWeeoo = Time.realtimeSinceStartup;
				if (this.siren_0 != null)
				{
					this.siren_0.gameObject.SetActive(!this.siren_0.gameObject.activeSelf);
				}
				if (this.siren_1 != null)
				{
					this.siren_1.gameObject.SetActive(!this.siren_0.gameObject.activeSelf);
				}
				if (this.sirenMaterials != null)
				{
					if (this.sirenMaterials[0] != null)
					{
						this.sirenMaterials[0].SetColor("_EmissionColor", (!this.siren_0.gameObject.activeSelf) ? Color.black : this.sirenMaterials[0].color);
					}
					if (this.sirenMaterials[1] != null)
					{
						this.sirenMaterials[1].SetColor("_EmissionColor", (!this.siren_1.gameObject.activeSelf) ? Color.black : this.sirenMaterials[1].color);
					}
				}
			}
			this.batteryBuffer += deltaTime * 20f;
			ushort num2 = (ushort)Mathf.FloorToInt(this.batteryBuffer);
			this.batteryBuffer -= (float)num2;
			if (num2 > 0)
			{
				if (this.isEngineOn && this.isDriven)
				{
					this.askChargeBattery(num2);
				}
				else if (this.headlightsOn || this.sirensOn)
				{
					this.askBurnBattery(num2);
				}
			}
		}

		protected virtual void handleTireAliveChanged(Wheel wheel)
		{
			if (this.isPhysical)
			{
				base.GetComponent<Rigidbody>().WakeUp();
			}
		}

		public void init()
		{
			if (!Provider.isServer)
			{
				this.nsb = new NetworkSnapshotBuffer(Provider.UPDATE_TIME, Provider.UPDATE_DELAY);
			}
			this._asset = (VehicleAsset)Assets.find(EAssetType.VEHICLE, this.id);
			if (Provider.isServer)
			{
				if (this.fuel == 65535)
				{
					if (Provider.mode == EGameMode.TUTORIAL)
					{
						this.fuel = 0;
					}
					else
					{
						this.fuel = (ushort)Random.Range((int)this.asset.fuelMin, (int)this.asset.fuelMax);
					}
				}
				if (this.health == 65535)
				{
					this.health = (ushort)Random.Range((int)this.asset.healthMin, (int)this.asset.healthMax);
				}
				if (this.batteryCharge == 65535)
				{
					if (Random.value < Provider.modeConfigData.Vehicles.Has_Battery_Chance)
					{
						this.batteryCharge = (ushort)Random.Range(10000f * Provider.modeConfigData.Vehicles.Min_Battery_Charge, 10000f * Provider.modeConfigData.Vehicles.Max_Battery_Charge);
					}
					else
					{
						this.batteryCharge = 0;
					}
				}
			}
			if (!Dedicator.isDedicated)
			{
				this.fire = base.transform.FindChild("Fire");
				LightLODTool.applyLightLOD(this.fire);
				this.smoke_0 = base.transform.FindChild("Smoke_0");
				this.smoke_1 = base.transform.FindChild("Smoke_1");
				this._sirens = base.transform.FindChild("Sirens");
				LightLODTool.applyLightLOD(this.sirens);
				if (this.sirens != null)
				{
					this.siren_0 = this.sirens.FindChild("Siren_0");
					this.siren_1 = this.sirens.FindChild("Siren_1");
					this.sirenMaterials = new Material[2];
					Transform transform = base.transform.FindChild("Siren_0_Model");
					if (transform != null)
					{
						this.sirenMaterials[0] = transform.GetComponent<Renderer>().material;
					}
					Transform transform2 = base.transform.FindChild("Siren_1_Model");
					if (transform2 != null)
					{
						this.sirenMaterials[1] = transform2.GetComponent<Renderer>().material;
					}
				}
				this._headlights = base.transform.FindChild("Headlights");
				LightLODTool.applyLightLOD(this.headlights);
				Transform transform3 = base.transform.FindChild("Headlights_Model");
				if (transform3 != null)
				{
					this.headlightsMaterial = transform3.GetComponent<Renderer>().material;
				}
				this._taillights = base.transform.FindChild("Taillights");
				LightLODTool.applyLightLOD(this.taillights);
				Transform transform4 = base.transform.FindChild("Taillights_Model");
				if (transform4 != null)
				{
					this.taillightsMaterial = transform4.GetComponent<Renderer>().material;
				}
				else
				{
					InteractableVehicle.materials.Clear();
					for (int i = 0; i < 4; i++)
					{
						Transform transform5 = base.transform.FindChild("Taillight_" + i + "_Model");
						if (transform5 == null)
						{
							break;
						}
						InteractableVehicle.materials.Add(transform5.GetComponent<Renderer>().material);
					}
					if (InteractableVehicle.materials.Count > 0)
					{
						this.taillightMaterials = InteractableVehicle.materials.ToArray();
					}
				}
				if (this.asset.engine == EEngine.HELICOPTER && this.clipAudioSource != null)
				{
					this.windZone = this.clipAudioSource.gameObject.AddComponent<WindZone>();
					this.windZone.mode = 1;
					this.windZone.radius = 64f;
					this.windZone.windMain = 0f;
					this.windZone.windTurbulence = 0f;
					this.windZone.windPulseFrequency = 0f;
					this.windZone.windPulseMagnitude = 0f;
				}
			}
			this._sirensOn = false;
			this._headlightsOn = false;
			this._taillightsOn = false;
			this.waterCenterTransform = base.transform.FindChild("Water_Center");
			Transform transform6 = base.transform.FindChild("Seats");
			Transform transform7 = base.transform.FindChild("Objects");
			Transform transform8 = base.transform.FindChild("Turrets");
			this._passengers = new Passenger[transform6.childCount];
			for (int j = 0; j < this.passengers.Length; j++)
			{
				Transform newSeat = transform6.FindChild("Seat_" + j);
				Transform newObj = null;
				if (transform7 != null)
				{
					newObj = transform7.FindChild("Seat_" + j);
				}
				Transform transform9 = null;
				Transform newTurretPitch = null;
				Transform newTurretAim = null;
				if (transform8 != null)
				{
					Transform transform10 = transform8.FindChild("Turret_" + j);
					if (transform10 != null)
					{
						transform9 = transform10.FindChild("Yaw");
						if (transform9 != null)
						{
							Transform transform11 = transform9.FindChild("Seats");
							if (transform11 != null)
							{
								newSeat = transform11.FindChild("Seat_" + j);
							}
							Transform transform12 = transform9.FindChild("Objects");
							if (transform12 != null)
							{
								newObj = transform12.FindChild("Seat_" + j);
							}
							newTurretPitch = transform9.FindChild("Pitch");
						}
						newTurretAim = transform10.FindChildRecursive("Aim");
					}
				}
				this.passengers[j] = new Passenger(newSeat, newObj, transform9, newTurretPitch, newTurretAim);
			}
			this._turrets = new Passenger[this.asset.turrets.Length];
			for (int k = 0; k < this.turrets.Length; k++)
			{
				TurretInfo turretInfo = this.asset.turrets[k];
				if ((int)turretInfo.seatIndex < this.passengers.Length)
				{
					this.passengers[(int)turretInfo.seatIndex].turret = turretInfo;
					this._turrets[k] = this.passengers[(int)turretInfo.seatIndex];
				}
			}
			Transform transform13 = base.transform.FindChild("Tires");
			this.tires = new Wheel[transform13.childCount];
			for (int l = 0; l < transform13.childCount; l++)
			{
				Wheel wheel = new Wheel(this, (WheelCollider)transform13.FindChild("Tire_" + l).GetComponent<Collider>(), l < 2, l >= transform13.childCount - 2);
				wheel.reset();
				wheel.aliveChanged += this.handleTireAliveChanged;
				this.tires[l] = wheel;
			}
			this.buoyancy = base.transform.FindChild("Buoyancy");
			if (this.buoyancy != null)
			{
				for (int m = 0; m < this.buoyancy.childCount; m++)
				{
					Transform child = this.buoyancy.GetChild(m);
					child.gameObject.AddComponent<Buoyancy>().density = (float)(this.buoyancy.childCount * 500);
				}
			}
			this.hook = base.transform.FindChild("Hook");
			this.hooked = new List<HookInfo>();
			this.center = base.transform.FindChild("Center");
			if (this.center == null)
			{
				this.center = base.transform;
			}
			Transform transform14 = base.transform.FindChild("DepthMask");
			if (transform14 != null)
			{
				transform14.GetComponent<Renderer>().sharedMaterial = (Material)Resources.Load("Materials/DepthMask");
			}
			if (!Dedicator.isDedicated)
			{
				List<Wheel> list = new List<Wheel>(this.tires);
				Transform transform15 = base.transform.FindChild("Wheels");
				if (transform15 != null)
				{
					for (int n = 0; n < list.Count; n++)
					{
						int num = -1;
						float num2 = 16f;
						for (int num3 = 0; num3 < transform15.childCount; num3++)
						{
							Transform child2 = transform15.GetChild(num3);
							float sqrMagnitude = (list[n].wheel.transform.position - child2.position).sqrMagnitude;
							if (sqrMagnitude < num2)
							{
								num = num3;
								num2 = sqrMagnitude;
							}
						}
						if (num != -1)
						{
							list[n].model = transform15.GetChild(num);
						}
					}
					if (transform15.childCount != list.Count)
					{
						for (int num4 = 0; num4 < transform15.childCount; num4++)
						{
							Transform transform16 = transform15.GetChild(num4);
							for (int num5 = 0; num5 < this.tires.Length; num5++)
							{
								if (list[num5].model == transform16)
								{
									transform16 = null;
									break;
								}
							}
							if (!(transform16 == null))
							{
								list.Add(new Wheel(this, null, false, false)
								{
									model = transform16
								});
							}
						}
					}
				}
				this.tires = list.ToArray();
				this.wheel = base.transform.FindChild("Objects").FindChild("Steer");
				Transform transform17 = base.transform.FindChild("Rotors");
				if (transform17 != null)
				{
					this.rotors = new Rotor[transform17.childCount];
					for (int num6 = 0; num6 < transform17.childCount; num6++)
					{
						Transform child3 = transform17.GetChild(num6);
						Rotor rotor = new Rotor();
						rotor.prop = child3;
						rotor.material_0 = child3.FindChild("Model_0").GetComponent<Renderer>().material;
						rotor.material_1 = child3.FindChild("Model_1").GetComponent<Renderer>().material;
						rotor.rest = child3.localRotation;
						this.rotors[num6] = rotor;
					}
				}
				else
				{
					this.rotors = new Rotor[0];
				}
				Transform transform18 = base.transform.FindChild("Exhaust");
				if (transform18 != null)
				{
					this.exhausts = new ParticleSystem[transform18.childCount];
					for (int num7 = 0; num7 < transform18.childCount; num7++)
					{
						Transform child4 = transform18.GetChild(num7);
						this.exhausts[num7] = child4.GetComponent<ParticleSystem>();
					}
				}
				else
				{
					this.exhausts = new ParticleSystem[0];
				}
				this.rest = this.wheel.localRotation;
				this.front = base.transform.FindChild("Objects").FindChild("Front");
				this.tellFuel(this.fuel);
				this.tellHealth(this.health);
				this.tellBatteryCharge(this.batteryCharge);
			}
			if (this.isExploded)
			{
				this.tellExploded();
			}
			if (!Provider.isServer)
			{
				Object.Destroy(base.transform.FindChild("Nav").gameObject);
				Object.Destroy(base.transform.FindChild("Bumper").gameObject);
			}
		}

		private void Awake()
		{
			if (!Dedicator.isDedicated)
			{
				this.clipAudioSource = base.transform.FindChild("Sound").GetComponent<AudioSource>();
				this.engineAudioSource = base.GetComponent<AudioSource>();
				if (this.engineAudioSource != null)
				{
					this.engineAudioSource.maxDistance *= 2f;
				}
			}
		}

		private void Start()
		{
			Bumper bumper = base.transform.FindChild("Bumper").gameObject.AddComponent<Bumper>();
			bumper.init(this);
			this.updateVehicle();
			this.updatePhysics();
			this.updateEngine();
			this.updates = new List<VehicleStateUpdate>();
		}

		private void OnDestroy()
		{
			if (this.isExploded && !Dedicator.isDedicated)
			{
				HighlighterTool.destroyMaterials(base.transform);
				if (this.turrets != null)
				{
					for (int i = 0; i < this.turrets.Length; i++)
					{
						HighlighterTool.destroyMaterials(this.turrets[i].turretYaw);
						HighlighterTool.destroyMaterials(this.turrets[i].turretPitch);
					}
				}
			}
			if (this.headlightsMaterial != null)
			{
				Object.DestroyImmediate(this.headlightsMaterial);
			}
			if (this.taillightsMaterial != null)
			{
				Object.DestroyImmediate(this.taillightsMaterial);
			}
			else if (this.taillightMaterials != null)
			{
				for (int j = 0; j < this.taillightMaterials.Length; j++)
				{
					if (this.taillightMaterials[j] != null)
					{
						Object.DestroyImmediate(this.taillightMaterials[j]);
					}
				}
			}
			if (this.sirenMaterials != null)
			{
				for (int k = 0; k < this.sirenMaterials.Length; k++)
				{
					if (this.sirenMaterials[k] != null)
					{
						Object.DestroyImmediate(this.sirenMaterials[k]);
					}
				}
			}
			if (this.rotors == null)
			{
				return;
			}
			for (int l = 0; l < this.rotors.Length; l++)
			{
				if (this.rotors[l].material_0 != null)
				{
					Object.DestroyImmediate(this.rotors[l].material_0);
					this.rotors[l].material_0 = null;
				}
				if (this.rotors[l].material_1 != null)
				{
					Object.DestroyImmediate(this.rotors[l].material_1);
					this.rotors[l].material_1 = null;
				}
			}
		}

		private static Collider[] grab = new Collider[4];

		private static List<Material> materials = new List<Material>();

		private static readonly float EXPLODE = 4f;

		private static readonly ushort HEALTH_0 = 100;

		private static readonly ushort HEALTH_1 = 200;

		public uint instanceID;

		public ushort id;

		public ushort fuel;

		public ushort health;

		public ushort batteryCharge;

		private uint lastBurnFuel;

		private uint lastBurnBattery;

		private float horned;

		private bool _isDrowned;

		private float _lastDead;

		private float _lastUnderwater;

		private float _lastExploded;

		private float _slip;

		public bool isExploded;

		private float _factor;

		private float _speed;

		private float _physicsSpeed;

		private float _spedometer;

		private int _turn;

		private float spin;

		private float _steer;

		private float fly;

		private Rotor[] rotors;

		private ParticleSystem[] exhausts;

		private Transform wheel;

		private Transform front;

		private Quaternion rest;

		private Transform waterCenterTransform;

		private Transform fire;

		private Transform smoke_0;

		private Transform smoke_1;

		[Obsolete]
		public bool isUpdated;

		public List<VehicleStateUpdate> updates;

		private Transform _sirens;

		private Material[] sirenMaterials;

		private Transform siren_0;

		private Transform siren_1;

		private bool _sirensOn;

		private Transform _headlights;

		private Material headlightsMaterial;

		private bool _headlightsOn;

		private Transform _taillights;

		private Material taillightsMaterial;

		private Material[] taillightMaterials;

		private bool _taillightsOn;

		private CSteamID _lockedOwner;

		private CSteamID _lockedGroup;

		private bool _isLocked;

		private VehicleAsset _asset;

		public float lastSeat;

		private Passenger[] _passengers;

		private Passenger[] _turrets;

		public bool isHooked;

		private Transform buoyancy;

		private Transform hook;

		private List<HookInfo> hooked;

		private Transform center;

		private Vector3 lastUpdatedPos;

		private NetworkSnapshotBuffer nsb;

		private Vector3 real;

		private float lastTick;

		private float lastWeeoo;

		private AudioSource clipAudioSource;

		private AudioSource engineAudioSource;

		private WindZone windZone;

		private bool isRecovering;

		private float lastRecover;

		private bool isPhysical;

		private bool isFrozen;

		private float altSpeedInput;

		private float altSpeedOutput;

		private float speedTraction;

		private float batteryBuffer;
	}
}
