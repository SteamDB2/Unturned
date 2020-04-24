using System;
using System.Collections.Generic;
using SDG.Framework.Debug;
using SDG.Framework.Devkit;
using SDG.Framework.Landscapes;
using SDG.Framework.Water;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	public class PlayerMovement : PlayerCaller, ILandscaleHoleVolumeInteractionHandler
	{
		[TerminalCommandProperty("misc.dont_enable_this", "seriously, don't enable this unless Nelson suggested it", false)]
		public static bool forceTrustClient
		{
			get
			{
				return PlayerMovement._forceTrustClient;
			}
			set
			{
				PlayerMovement._forceTrustClient = value;
				TerminalUtility.printCommandPass("Set dont_enable_this to: " + PlayerMovement.forceTrustClient);
			}
		}

		public event PlayerNavChanged PlayerNavChanged;

		private void TriggerPlayerNavChanged(byte oldNav, byte newNav)
		{
			if (this.PlayerNavChanged == null)
			{
				return;
			}
			this.PlayerNavChanged(this, oldNav, newNav);
		}

		public LandscapeHoleVolume landscapeHoleVolume { get; protected set; }

		public bool landscapeHoleAutoIgnoreTerrainCollision
		{
			get
			{
				return true;
			}
		}

		public void landscapeHoleBeginCollision(LandscapeHoleVolume volume, List<TerrainCollider> terrainColliders)
		{
			this.landscapeHoleVolume = volume;
		}

		public void landscapeHoleEndCollision(LandscapeHoleVolume volume, List<TerrainCollider> terrainColliders)
		{
			if (volume == this.landscapeHoleVolume)
			{
				this.landscapeHoleVolume = null;
			}
		}

		public bool isGrounded
		{
			get
			{
				return this._isGrounded;
			}
		}

		public bool isSafe
		{
			get
			{
				return this._isSafe;
			}
			set
			{
				this._isSafe = value;
			}
		}

		public bool isRadiated
		{
			get
			{
				return this._isRadiated;
			}
			set
			{
				this._isRadiated = value;
			}
		}

		public PurchaseNode purchaseNode
		{
			get
			{
				return this._purchaseNode;
			}
			set
			{
				this._purchaseNode = value;
			}
		}

		public IAmbianceNode effectNode { get; private set; }

		public void setSize(EPlayerHeight newHeight)
		{
			if (newHeight == this.height)
			{
				return;
			}
			this.height = newHeight;
			this.applySize();
		}

		private void applySize()
		{
			float num;
			switch (this.height)
			{
			case EPlayerHeight.STAND:
				num = PlayerMovement.HEIGHT_STAND;
				break;
			case EPlayerHeight.CROUCH:
				num = PlayerMovement.HEIGHT_CROUCH;
				break;
			case EPlayerHeight.PRONE:
				num = PlayerMovement.HEIGHT_PRONE;
				break;
			default:
				num = 2f;
				break;
			}
			if ((base.channel.isOwner || Provider.isServer) && this.controller != null)
			{
				this.controller.height = num;
				this.controller.center = new Vector3(0f, num / 2f, 0f);
				if (this.wasSizeAppliedYet)
				{
					base.transform.localPosition += new Vector3(0f, 0.02f, 0f);
				}
				this.wasSizeAppliedYet = true;
			}
		}

		public bool isMoving
		{
			get
			{
				return this._isMoving;
			}
		}

		public float speed
		{
			get
			{
				if (base.player.stance.stance == EPlayerStance.SWIM)
				{
					return PlayerMovement.SPEED_SWIM * (1f + base.player.skills.mastery(0, 5) * 0.25f) * this._multiplier;
				}
				float num = 1f + base.player.skills.mastery(0, 4) * 0.25f;
				if (base.player.stance.stance == EPlayerStance.CLIMB)
				{
					return PlayerMovement.SPEED_CLIMB * num * this._multiplier;
				}
				if (base.player.stance.stance == EPlayerStance.SPRINT)
				{
					return PlayerMovement.SPEED_SPRINT * num * this._multiplier;
				}
				if (base.player.stance.stance == EPlayerStance.STAND)
				{
					return PlayerMovement.SPEED_STAND * num * this._multiplier;
				}
				if (base.player.stance.stance == EPlayerStance.CROUCH)
				{
					return PlayerMovement.SPEED_CROUCH * num * this._multiplier;
				}
				if (base.player.stance.stance == EPlayerStance.PRONE)
				{
					return PlayerMovement.SPEED_PRONE * num * this._multiplier;
				}
				return 0f;
			}
		}

		public Vector3 move
		{
			get
			{
				return this._move;
			}
		}

		public byte region_x
		{
			get
			{
				return this._region_x;
			}
		}

		public byte region_y
		{
			get
			{
				return this._region_y;
			}
		}

		public byte bound
		{
			get
			{
				return this._bound;
			}
		}

		public byte nav
		{
			get
			{
				return this._nav;
			}
		}

		public LoadedRegion[,] loadedRegions
		{
			get
			{
				return this._loadedRegions;
			}
		}

		public LoadedBound[] loadedBounds
		{
			get
			{
				return this._loadedBounds;
			}
		}

		public float fall { get; private set; }

		public Vector3 real
		{
			get
			{
				return this._real;
			}
		}

		public byte horizontal
		{
			get
			{
				return this._horizontal;
			}
		}

		public byte vertical
		{
			get
			{
				return this._vertical;
			}
		}

		public bool jump
		{
			get
			{
				return this._jump;
			}
		}

		public InteractableVehicle getVehicle()
		{
			return this.vehicle;
		}

		public byte getSeat()
		{
			return this.seat;
		}

		private void updateVehicle()
		{
			InteractableVehicle interactableVehicle = this.vehicle;
			this.vehicle = this.seatingVehicle;
			this.seat = this.seatingSeat;
			bool flag = this.vehicle != null && this.seat == 0;
			if (this.vehicle == null)
			{
				base.player.transform.parent = this.seatingTransform;
				base.player.askTeleport(Provider.server, this.seatingPosition, this.seatingAngle);
			}
			if (base.channel.isOwner)
			{
				bool flag2;
				if (flag && Level.info != null && Level.info.name.ToLower() != "tutorial" && Provider.provider.achievementsService.getAchievement("Wheel", out flag2) && !flag2)
				{
					Provider.provider.achievementsService.setAchievement("Wheel");
				}
				if (this.vehicle != null)
				{
					PlayerUI.disableDot();
					if (base.player.equipment.asset != null && base.player.equipment.asset.type == EItemType.GUN)
					{
						if (base.player.look.perspective == EPlayerPerspective.THIRD)
						{
							PlayerUI.disableCrosshair();
						}
						else
						{
							PlayerUI.enableCrosshair();
						}
					}
				}
				else if (base.player.equipment.asset != null && base.player.equipment.asset.type == EItemType.GUN)
				{
					PlayerUI.enableCrosshair();
				}
				else
				{
					PlayerUI.enableDot();
				}
			}
			if (base.channel.isOwner || Provider.isServer)
			{
				this.controller.enabled = (this.vehicle == null);
				if (this.vehicle != null)
				{
					if (flag)
					{
						base.player.stance.checkStance(EPlayerStance.DRIVING);
					}
					else
					{
						base.player.stance.checkStance(EPlayerStance.SITTING);
					}
				}
				else
				{
					base.player.stance.checkStance(EPlayerStance.STAND);
				}
			}
			if (base.channel.isOwner)
			{
				if (this.onSeated != null)
				{
					this.onSeated(flag, this.vehicle != null, interactableVehicle != null, interactableVehicle, this.vehicle);
				}
				if (flag && this.onVehicleUpdated != null)
				{
					this.onVehicleUpdated(!this.vehicle.isUnderwater && !this.vehicle.isDead, this.vehicle.fuel, this.vehicle.asset.fuel, this.vehicle.spedometer, this.vehicle.asset.speedMin, this.vehicle.asset.speedMax, this.vehicle.health, this.vehicle.asset.health, this.vehicle.batteryCharge);
				}
				if (this.vehicle != null)
				{
					if (flag)
					{
						if (interactableVehicle == null)
						{
							PlayerUI.message(EPlayerMessage.VEHICLE_EXIT, string.Empty);
						}
						else
						{
							PlayerUI.message(EPlayerMessage.VEHICLE_SWAP, string.Empty);
						}
					}
					else
					{
						PlayerUI.message(EPlayerMessage.VEHICLE_SWAP, string.Empty);
					}
				}
			}
			if (this.vehicle != null)
			{
				base.player.transform.parent = this.seatingTransform;
				base.player.transform.localPosition = this.seatingPosition;
				base.player.transform.localRotation = Quaternion.identity;
				base.player.look.updateLook();
				if ((base.channel.isOwner || Provider.isServer) && this.landscapeHoleVolume != null)
				{
					this.landscapeHoleVolume.endCollision(this.controller);
				}
			}
		}

		public void setVehicle(InteractableVehicle newVehicle, byte newSeat, Transform newSeatingTransform, Vector3 newSeatingPosition, byte newSeatingAngle, bool forceUpdate)
		{
			this.isSeating = true;
			this.seatingVehicle = newVehicle;
			this.seatingSeat = newSeat;
			this.seatingTransform = newSeatingTransform;
			this.seatingPosition = newSeatingPosition;
			this.seatingAngle = newSeatingAngle;
			if ((base.channel.isOwner || Provider.isServer) && !base.player.life.isDead && !forceUpdate)
			{
				return;
			}
			this.updateVehicle();
		}

		[SteamCall]
		public void tellRecov(CSteamID steamID, Vector3 newPosition, int newRecov)
		{
			if (base.channel.checkServer(steamID))
			{
				if (base.player.stance.stance == EPlayerStance.DRIVING || base.player.stance.stance == EPlayerStance.SITTING)
				{
					return;
				}
				this._real = newPosition;
				this.lastUpdatePos = base.transform.position;
				if (this.nsb != null)
				{
					this.nsb.updateLastSnapshot(new PitchYawSnapshotInfo(this.lastUpdatePos, base.player.look.pitch, base.player.look.yaw));
				}
				base.transform.localPosition = newPosition;
				this.isFrozen = true;
				base.player.input.recov = newRecov;
			}
		}

		public void tellState(Vector3 newPosition, byte newPitch, byte newYaw)
		{
			if (base.channel.isOwner)
			{
				return;
			}
			this.checkGround(newPosition);
			this.lastUpdatePos = newPosition;
			if (this.nsb != null)
			{
				this.nsb.addNewSnapshot(new PitchYawSnapshotInfo(newPosition, (float)newPitch, (float)newYaw * 2f));
			}
		}

		public void updateMovement()
		{
			this.lastUpdatePos = base.transform.localPosition;
			if (this.nsb != null)
			{
				this.nsb.updateLastSnapshot(new PitchYawSnapshotInfo(this.lastUpdatePos, base.player.look.pitch, base.player.look.yaw));
			}
			this._real = base.transform.position;
			if (base.channel.isOwner || Provider.isServer)
			{
				this.isRecovering = false;
				this.lastRecover = Time.realtimeSinceStartup;
				this.isFrozen = false;
			}
		}

		private void checkGround(Vector3 position)
		{
			this.material = EPhysicsMaterial.NONE;
			int num = RayMasks.BLOCK_COLLISION;
			LandscapeHoleVolume landscapeHoleVolume;
			if (LandscapeHoleUtility.isPointInsideHoleVolume(position, out landscapeHoleVolume))
			{
				num &= ~RayMasks.GROUND;
			}
			Physics.SphereCast(position + new Vector3(0f, 0.45f, 0f), 0.45f, Vector3.down, ref this.ground, 0.125f, num, 1);
			this._isGrounded = (this.ground.transform != null);
			if ((base.channel.isOwner || Provider.isServer) && this.controller.isGrounded)
			{
				this._isGrounded = true;
			}
			if (base.player.stance.stance == EPlayerStance.CLIMB || base.player.stance.stance == EPlayerStance.SWIM)
			{
				this._isGrounded = true;
			}
			if (base.player.stance.stance == EPlayerStance.CLIMB)
			{
				this.material = EPhysicsMaterial.TILE_STATIC;
			}
			else if (base.player.stance.stance == EPlayerStance.SWIM || WaterUtility.isPointUnderwater(base.transform.position))
			{
				this.material = EPhysicsMaterial.WATER_STATIC;
			}
			else if (this.ground.transform != null)
			{
				if (this.ground.transform.CompareTag("Ground"))
				{
					this.material = PhysicsTool.checkMaterial(base.transform.position);
				}
				else
				{
					this.material = PhysicsTool.checkMaterial(this.ground.collider);
				}
			}
		}

		private void onVisionUpdated(bool isViewing)
		{
			if (isViewing)
			{
				this.warp_x = (((double)Random.value >= 0.25) ? 1 : -1);
				this.warp_y = (((double)Random.value >= 0.25) ? 1 : -1);
			}
			else
			{
				this.warp_x = 1;
				this.warp_y = 1;
			}
		}

		private void onLifeUpdated(bool isDead)
		{
			byte b;
			Vector3 point;
			byte angle;
			if (isDead && this.vehicle != null && this.vehicle.forceRemovePlayer(out b, base.channel.owner.playerID.steamID, out point, out angle))
			{
				VehicleManager.sendExitVehicle(this.vehicle, b, point, angle, false);
			}
		}

		public void simulate()
		{
			this.updateRegionAndBound();
			if (base.channel.isOwner)
			{
				this.lastUpdatePos = base.transform.position;
			}
			if (this.isSeating)
			{
				this.isSeating = false;
				this.updateVehicle();
				return;
			}
		}

		public void simulate(uint simulation, int recov, bool inputBrake, Vector3 point, float angle_x, float angle_y, float angle_z, float speed, float physicsSpeed, int turn, float delta)
		{
			this.updateRegionAndBound();
			if (base.channel.isOwner)
			{
				this.lastUpdatePos = base.transform.position;
			}
			if (this.isSeating)
			{
				this.isSeating = false;
				this.updateVehicle();
				return;
			}
			if (base.player.stance.stance == EPlayerStance.DRIVING)
			{
				this.fell = base.transform.position.y;
				if (this.vehicle != null)
				{
					this.vehicle.simulate(simulation, recov, point, Quaternion.Euler(angle_x, angle_y, angle_z), speed, physicsSpeed, turn, delta);
				}
			}
		}

		public void simulate(uint simulation, bool inputJump, Vector3 point, float delta)
		{
			this.updateRegionAndBound();
			if (base.channel.isOwner)
			{
				this.lastUpdatePos = base.transform.position;
			}
			if (this.isSeating)
			{
				this.isSeating = false;
				this.updateVehicle();
				return;
			}
			if (this.isAllowed)
			{
				if ((point - base.transform.position).sqrMagnitude > 1f)
				{
					return;
				}
				this.isAllowed = false;
				this.fell = base.transform.position.y;
			}
			if (this.isRecovering)
			{
				if ((point - this.real).sqrMagnitude > 0.01f)
				{
					if (Time.realtimeSinceStartup - this.lastRecover > 5f)
					{
						this.lastRecover = Time.realtimeSinceStartup;
						base.channel.send("tellRecov", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
						{
							this.real,
							base.player.input.recov
						});
					}
					return;
				}
				this.isRecovering = false;
				return;
			}
			else
			{
				if (this.isFrozen)
				{
					this.isFrozen = false;
					return;
				}
				if (base.player.stance.stance == EPlayerStance.SITTING)
				{
					this._isMoving = false;
					this.fell = base.transform.position.y;
					return;
				}
				if (base.player.stance.stance == EPlayerStance.DRIVING)
				{
					this._isMoving = false;
					return;
				}
				this._isMoving = ((point - base.transform.position).sqrMagnitude > 0.25f);
				this.checkGround(base.transform.position);
				if (base.player.stance.stance == EPlayerStance.CLIMB || base.player.stance.stance == EPlayerStance.SWIM)
				{
					this.fell = base.transform.position.y;
				}
				else if (this.lastGrounded != this.isGrounded)
				{
					this.lastGrounded = this.isGrounded;
					if (this.isGrounded && this.onLanded != null)
					{
						this.onLanded(base.transform.position.y - this.fell);
					}
					this.fell = base.transform.position.y;
				}
				if (inputJump && this.isGrounded && !base.player.life.isBroken && (float)base.player.life.stamina >= 10f * (1f - base.player.skills.mastery(0, 6) * 0.5f) && (base.player.stance.stance == EPlayerStance.STAND || base.player.stance.stance == EPlayerStance.SPRINT))
				{
					base.player.life.askTire((byte)(10f * (1f - base.player.skills.mastery(0, 6) * 0.5f)));
				}
				if (Mathf.Pow(point.x - this.real.x, 2f) + Mathf.Pow(point.z - this.real.z, 2f) > Mathf.Pow(this.speed * delta, 2f) + 1f)
				{
					this.isRecovering = true;
					this.lastRecover = Time.realtimeSinceStartup;
					base.channel.send("tellRecov", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
					{
						this.real,
						base.player.input.recov
					});
					return;
				}
				if (point.y < this.real.y)
				{
					if (point.y - this.real.y < Physics.gravity.y * delta - 0.1f)
					{
						this.isRecovering = true;
						this.lastRecover = Time.realtimeSinceStartup;
						base.channel.send("tellPosition", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
						{
							this.real
						});
						return;
					}
				}
				else if (point.y - this.real.y > 9f * delta + 0.1f)
				{
					this.isRecovering = true;
					this.lastRecover = Time.realtimeSinceStartup;
					base.channel.send("tellPosition", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
					{
						this.real
					});
					return;
				}
				base.GetComponent<Rigidbody>().MovePosition(point);
				this._real = point;
				return;
			}
		}

		public void simulate(uint simulation, int recov, int input_x, int input_y, float look_x, float look_y, bool inputJump, Vector3 target, float delta)
		{
			this.updateRegionAndBound();
			if (base.channel.isOwner)
			{
				this.lastUpdatePos = base.transform.position;
			}
			if (this.isSeating)
			{
				this.isSeating = false;
				this.updateVehicle();
				if (!base.channel.isOwner)
				{
					return;
				}
			}
			if (this.isAllowed)
			{
				if (base.channel.isOwner)
				{
					this.isAllowed = false;
					this.fell = base.transform.position.y;
					this.fall = 0f;
					this.fall2 = 0f;
					return;
				}
				if ((target - base.transform.position).sqrMagnitude > 0.01f)
				{
					return;
				}
				this.isAllowed = false;
				this.fell = base.transform.position.y;
				this.fall = 0f;
				this.fall2 = 0f;
				return;
			}
			else if (Provider.isServer && this.isRecovering)
			{
				if (recov < base.player.input.recov)
				{
					if (Time.realtimeSinceStartup - this.lastRecover > 5f)
					{
						this.lastRecover = Time.realtimeSinceStartup;
						base.channel.send("tellRecov", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
						{
							this.real,
							base.player.input.recov
						});
					}
					return;
				}
				this.isRecovering = false;
				this.fall = 0f;
				this.fall2 = 0f;
				return;
			}
			else
			{
				if (this.isFrozen)
				{
					this.isFrozen = false;
					this.fall = 0f;
					this.fall2 = 0f;
					return;
				}
				this._move.x = (float)input_x;
				this._move.z = (float)input_y;
				if (base.player.stance.stance == EPlayerStance.SITTING)
				{
					this._isMoving = false;
					this.checkGround(base.transform.position);
					this.fell = base.transform.position.y;
					this.fall2 = 0f;
					if (this.getVehicle() != null && this.getVehicle().passengers[(int)this.getSeat()].turret != null && (Mathf.Abs((int)(base.player.look.lastAngle - base.player.look.angle)) > 1 || Mathf.Abs((int)(base.player.look.lastRot - base.player.look.rot)) > 1))
					{
						base.player.look.lastAngle = base.player.look.angle;
						base.player.look.lastRot = base.player.look.rot;
						if (this.canAddSimulationResultsToUpdates)
						{
							this.updates.Add(new PlayerStateUpdate(this.real, base.player.look.angle, base.player.look.rot));
						}
					}
					return;
				}
				if (base.player.stance.stance == EPlayerStance.DRIVING)
				{
					this._isMoving = false;
					this.checkGround(base.transform.position);
					this.fell = base.transform.position.y;
					this.fall2 = 0f;
					if (base.channel.isOwner)
					{
						this.vehicle.simulate(simulation, recov, input_x, input_y, look_x, look_y, inputJump, delta);
						if (this.onVehicleUpdated != null)
						{
							this.onVehicleUpdated(!this.vehicle.isUnderwater && !this.vehicle.isDead, this.vehicle.fuel, this.vehicle.asset.fuel, this.vehicle.speed, this.vehicle.asset.speedMin, this.vehicle.asset.speedMax, this.vehicle.health, this.vehicle.asset.health, this.vehicle.batteryCharge);
						}
					}
					return;
				}
				if (base.player.stance.stance == EPlayerStance.CLIMB)
				{
					this.fall = PlayerMovement.JUMP;
					this._isMoving = ((double)Mathf.Abs(this.move.x) > 0.1 || (double)Mathf.Abs(this.move.z) > 0.1);
					this.checkGround(base.transform.position);
					this.fell = base.transform.position.y;
					this.fall2 = 0f;
					this.direction = this.move.normalized * this.speed / 2f;
					this.controller.Move(Vector3.up * this.direction.z * delta);
				}
				else if (base.player.stance.stance == EPlayerStance.SWIM)
				{
					this._isMoving = ((double)Mathf.Abs(this.move.x) > 0.1 || (double)Mathf.Abs(this.move.z) > 0.1);
					this.checkGround(base.transform.position);
					this.fell = base.transform.position.y;
					this.fall2 = 0f;
					this.direction = this.move.normalized * this.speed * 1.5f;
					if (base.player.stance.isSubmerged || (base.player.look.pitch > 110f && (double)this.move.z > 0.1))
					{
						this.fall += Physics.gravity.y * delta / 7f;
						if (this.fall < Physics.gravity.y / 7f)
						{
							this.fall = Physics.gravity.y / 7f;
						}
						if (inputJump)
						{
							this.fall = PlayerMovement.SWIM;
						}
						this.controller.Move(base.player.look.aim.rotation * this.direction * delta + Vector3.up * this.fall * delta);
					}
					else
					{
						bool flag;
						float num;
						WaterUtility.getUnderwaterInfo(base.transform.position, out flag, out num);
						this.fall = (num - 1.275f - base.transform.position.y) / 8f;
						this.controller.Move(base.transform.rotation * this.direction * delta + Vector3.up * this.fall * delta);
					}
				}
				else
				{
					if (!base.channel.isOwner || !Level.isLoading)
					{
						this.fall += Physics.gravity.y * ((this.fall > 0f) ? 1f : this.gravity) * delta * 3f;
						if (this.fall < Physics.gravity.y * 2f * this.gravity)
						{
							this.fall = Physics.gravity.y * 2f * this.gravity;
						}
					}
					this._isMoving = ((double)Mathf.Abs(this.move.x) > 0.1 || (double)Mathf.Abs(this.move.z) > 0.1);
					this.checkGround(base.transform.position);
					if (this.lastGrounded != this.isGrounded)
					{
						this.lastGrounded = this.isGrounded;
						if (this.isGrounded && this.onLanded != null)
						{
							this.onLanded(base.transform.position.y - this.fell);
						}
						this.fell = base.transform.position.y;
					}
					if (inputJump && this.isGrounded && !base.player.life.isBroken && (float)base.player.life.stamina >= 10f * (1f - base.player.skills.mastery(0, 6) * 0.5f) && (base.player.stance.stance == EPlayerStance.STAND || base.player.stance.stance == EPlayerStance.SPRINT))
					{
						this.fall = PlayerMovement.JUMP * (1f + base.player.skills.mastery(0, 6) * 0.25f);
						base.player.life.askTire((byte)(10f * (1f - base.player.skills.mastery(0, 6) * 0.5f)));
					}
					if (this.isGrounded && this.ground.transform != null)
					{
						this.slope = Mathf.Lerp(this.slope, this.ground.normal.y, delta);
					}
					else
					{
						this.slope = Mathf.Lerp(this.slope, 1f, delta);
					}
					this._multiplier = Mathf.Lerp(this._multiplier, this.multiplier, delta);
					if (this.material == EPhysicsMaterial.ICE_STATIC)
					{
						this.direction = Vector3.Lerp(this.direction, base.transform.rotation * this.move.normalized * this.speed * this.slope * delta, delta);
					}
					else if (this.material == EPhysicsMaterial.METAL_SLIP)
					{
						float num2;
						if (this.slope < 0.75f)
						{
							num2 = 0f;
						}
						else
						{
							num2 = Mathf.Lerp(0f, 1f, (this.slope - 0.75f) * 4f);
						}
						this.direction = Vector3.Lerp(this.direction, base.transform.rotation * this.move.normalized * this.speed * this.slope * delta * 2f, (!this.isMoving) ? (0.5f * num2 * delta) : (2f * delta));
					}
					else
					{
						this.direction = base.transform.rotation * this.move.normalized * this.speed * this.slope * delta;
					}
					Vector3 vector = this.direction;
					if (this.isGrounded)
					{
						float num3 = Vector3.Angle(Vector3.up, this.ground.normal);
						if (num3 > 59f)
						{
							this.fall2 += 16f * delta;
							if (this.fall2 > 128f)
							{
								this.fall2 = 128f;
							}
							Vector3 vector2 = Vector3.Cross(Vector3.up, this.ground.normal);
							Vector3 vector3 = Vector3.Cross(vector2, this.ground.normal);
							vector += vector3 * this.fall2 * delta;
						}
						else
						{
							this.fall2 = 0f;
						}
					}
					vector += Vector3.up * this.fall * delta;
					this.controller.Move(vector);
				}
				if (!base.channel.isOwner && Provider.isServer)
				{
					bool flag2 = Dedicator.serverVisibility == ESteamServerVisibility.LAN || PlayerMovement.forceTrustClient;
					if (flag2)
					{
						base.transform.localPosition = target;
						this._real = target;
					}
					else
					{
						this._real = base.transform.localPosition;
						if ((target - this.real).sqrMagnitude > 0.01f)
						{
							this.isRecovering = true;
							this.lastRecover = Time.realtimeSinceStartup;
							base.player.input.recov++;
							base.channel.send("tellRecov", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
							{
								this.real,
								base.player.input.recov
							});
						}
					}
					if (this.updates != null && (Mathf.Abs((int)(base.player.look.lastAngle - base.player.look.angle)) > 1 || Mathf.Abs((int)(base.player.look.lastRot - base.player.look.rot)) > 1 || Mathf.Abs(this.lastUpdatePos.x - this.real.x) > Provider.UPDATE_DISTANCE || Mathf.Abs(this.lastUpdatePos.y - this.real.y) > Provider.UPDATE_DISTANCE || Mathf.Abs(this.lastUpdatePos.z - this.real.z) > Provider.UPDATE_DISTANCE))
					{
						base.player.look.lastAngle = base.player.look.angle;
						base.player.look.lastRot = base.player.look.rot;
						this.lastUpdatePos = this.real;
						if (this.canAddSimulationResultsToUpdates)
						{
							this.updates.Add(new PlayerStateUpdate(this.real, base.player.look.angle, base.player.look.rot));
						}
						else
						{
							this.updates.Add(new PlayerStateUpdate(Vector3.zero, 0, 0));
						}
					}
				}
				return;
			}
		}

		private void onPerspectiveUpdated(EPlayerPerspective newPerspective)
		{
			if (this.vehicle != null && base.player.equipment.asset != null && base.player.equipment.asset.type == EItemType.GUN)
			{
				if (newPerspective == EPlayerPerspective.THIRD)
				{
					PlayerUI.disableCrosshair();
				}
				else
				{
					PlayerUI.enableCrosshair();
				}
			}
		}

		private void Update()
		{
			if (this.nsb != null)
			{
				this.snapshot = (PitchYawSnapshotInfo)this.nsb.getCurrentSnapshot();
			}
			if (base.channel.isOwner)
			{
				if (!PlayerUI.window.showCursor && !LoadingUI.isBlocked)
				{
					this._jump = Input.GetKey(ControlsSettings.jump);
					if (this.getVehicle() != null)
					{
						if (Input.GetKeyDown(ControlsSettings.locker))
						{
							VehicleManager.sendVehicleLock();
						}
						if (Input.GetKeyDown(ControlsSettings.primary))
						{
							VehicleManager.sendVehicleHorn();
						}
						if (Input.GetKeyDown(ControlsSettings.secondary))
						{
							VehicleManager.sendVehicleHeadlights();
						}
						if (Input.GetKeyDown(ControlsSettings.other))
						{
							VehicleManager.sendVehicleBonus();
						}
					}
					if (this.getVehicle() != null && this.getVehicle().asset != null && (this.getVehicle().asset.engine == EEngine.PLANE || this.getVehicle().asset.engine == EEngine.HELICOPTER))
					{
						if (Input.GetKey(ControlsSettings.yawLeft))
						{
							this.input_x = -1;
						}
						else if (Input.GetKey(ControlsSettings.yawRight))
						{
							this.input_x = 1;
						}
						else
						{
							this.input_x = 0;
						}
						if (Input.GetKey(ControlsSettings.thrustIncrease))
						{
							this.input_y = 1;
						}
						else if (Input.GetKey(ControlsSettings.thrustDecrease))
						{
							this.input_y = -1;
						}
						else
						{
							this.input_y = 0;
						}
					}
					else
					{
						if (Input.GetKey(ControlsSettings.left))
						{
							this.input_x = -1;
						}
						else if (Input.GetKey(ControlsSettings.right))
						{
							this.input_x = 1;
						}
						else
						{
							this.input_x = 0;
						}
						if (Input.GetKey(ControlsSettings.up))
						{
							this.input_y = 1;
						}
						else if (Input.GetKey(ControlsSettings.down))
						{
							this.input_y = -1;
						}
						else
						{
							this.input_y = 0;
						}
					}
				}
				else
				{
					this._jump = false;
					this.input_x = 0;
					this.input_y = 0;
				}
				this.input_x *= this.warp_x;
				this.input_y *= this.warp_y;
				if (base.player.look.isOrbiting)
				{
					this._jump = false;
					this._horizontal = 1;
					this._vertical = 1;
				}
				else
				{
					this._horizontal = (byte)(this.input_x + 1);
					this._vertical = (byte)(this.input_y + 1);
				}
			}
			if (!Dedicator.isDedicated && Time.time - this.lastFootstep > 1.75f / this.speed)
			{
				this.lastFootstep = Time.time;
				if (!base.channel.isOwner)
				{
					this.checkGround(base.transform.position);
				}
				if (this.isGrounded && this.isMoving && base.player.stance.stance != EPlayerStance.PRONE && this.material != EPhysicsMaterial.NONE)
				{
					if (this.material == EPhysicsMaterial.WATER_STATIC)
					{
						if (base.player.stance.stance == EPlayerStance.SWIM)
						{
							base.player.playSound((AudioClip)Resources.Load("Sounds/Physics/Water/Footsteps/Swim"), 0.1f);
						}
						else
						{
							base.player.playSound((AudioClip)Resources.Load("Sounds/Physics/Water/Footsteps/Splash"), 0.2f);
						}
					}
					else
					{
						float num = 1f - base.player.skills.mastery(1, 0) * 0.75f;
						if (base.player.stance.stance == EPlayerStance.CROUCH)
						{
							num *= 0.5f;
						}
						num *= 0.2f;
						if (num > 0.01f)
						{
							if (this.material == EPhysicsMaterial.CLOTH_DYNAMIC || this.material == EPhysicsMaterial.CLOTH_STATIC)
							{
								base.player.playSound((AudioClip)Resources.Load("Sounds/Physics/Tile/Footsteps/Tile_" + Random.Range(0, 4)), num);
							}
							else if (this.material == EPhysicsMaterial.TILE_DYNAMIC || this.material == EPhysicsMaterial.TILE_STATIC)
							{
								base.player.playSound((AudioClip)Resources.Load("Sounds/Physics/Tile/Footsteps/Tile_" + Random.Range(0, 4)), num);
							}
							else if (this.material == EPhysicsMaterial.CONCRETE_DYNAMIC || this.material == EPhysicsMaterial.CONCRETE_STATIC)
							{
								base.player.playSound((AudioClip)Resources.Load("Sounds/Physics/Concrete/Footsteps/Concrete_" + Random.Range(0, 4)), num);
							}
							else if (this.material == EPhysicsMaterial.GRAVEL_DYNAMIC || this.material == EPhysicsMaterial.GRAVEL_STATIC)
							{
								base.player.playSound((AudioClip)Resources.Load("Sounds/Physics/Gravel/Footsteps/Gravel_" + Random.Range(0, 4)), num);
							}
							else if (this.material == EPhysicsMaterial.METAL_DYNAMIC || this.material == EPhysicsMaterial.METAL_STATIC || this.material == EPhysicsMaterial.METAL_SLIP)
							{
								base.player.playSound((AudioClip)Resources.Load("Sounds/Physics/Metal/Footsteps/Metal_" + Random.Range(0, 5)), num);
							}
							else if (this.material == EPhysicsMaterial.WOOD_DYNAMIC || this.material == EPhysicsMaterial.WOOD_STATIC)
							{
								base.player.playSound((AudioClip)Resources.Load("Sounds/Physics/Wood/Footsteps/Wood_" + Random.Range(0, 11)), num);
							}
							else if (this.material == EPhysicsMaterial.FOLIAGE_STATIC || this.material == EPhysicsMaterial.FOLIAGE_DYNAMIC)
							{
								base.player.playSound((AudioClip)Resources.Load("Sounds/Physics/Foliage/Footsteps/Foliage_" + Random.Range(0, 7)), num);
							}
							else if (this.material == EPhysicsMaterial.SNOW_STATIC || this.material == EPhysicsMaterial.ICE_STATIC)
							{
								base.player.playSound((AudioClip)Resources.Load("Sounds/Physics/Snow/Footsteps/Snow_" + Random.Range(0, 7)), num);
							}
						}
					}
				}
			}
			if (base.channel.isOwner)
			{
				if (base.player.look.isOrbiting && (!base.player.workzone.isBuilding || Input.GetKey(ControlsSettings.secondary)))
				{
					float num2 = 4f;
					if (Input.GetKey(ControlsSettings.modify))
					{
						num2 = 16f;
					}
					else if (Input.GetKey(ControlsSettings.other))
					{
						num2 = 1f;
					}
					base.player.look.orbitPosition += MainCamera.instance.transform.right * (float)this.input_x * Time.deltaTime * num2;
					base.player.look.orbitPosition += MainCamera.instance.transform.forward * (float)this.input_y * Time.deltaTime * num2;
					float num3 = 0f;
					if (Input.GetKey(ControlsSettings.ascend))
					{
						num3 = 1f;
					}
					else if (Input.GetKey(ControlsSettings.descend))
					{
						num3 = -1f;
					}
					base.player.look.orbitPosition += Vector3.up * num3 * Time.deltaTime * num2;
				}
				if (base.player.stance.stance == EPlayerStance.DRIVING || base.player.stance.stance == EPlayerStance.SITTING)
				{
					base.player.first.localPosition = Vector3.zero;
					base.player.third.localPosition = Vector3.zero;
					this.fell = base.transform.position.y;
					this.fall2 = 0f;
				}
				else
				{
					base.player.first.position = Vector3.Lerp(this.lastUpdatePos, base.transform.position, (Time.realtimeSinceStartup - base.player.input.tick) / PlayerInput.RATE);
					if (base.player.stance.stance == EPlayerStance.PRONE)
					{
						base.player.first.position += Vector3.down * 0.1f;
					}
					base.player.third.position = base.player.first.position;
				}
				base.player.look.aim.parent.transform.position = base.player.first.position;
				if (this.vehicle != null)
				{
					if ((base.transform.position - this.lastStatPos).sqrMagnitude > 1024f)
					{
						this.lastStatPos = base.transform.position;
					}
					else if (Time.realtimeSinceStartup - this.lastStatTime > 1f)
					{
						this.lastStatTime = Time.realtimeSinceStartup;
						if ((base.transform.position - this.lastStatPos).sqrMagnitude > 0.1f)
						{
							int num4;
							if (Provider.provider.statisticsService.userStatisticsService.getStatistic("Travel_Vehicle", out num4))
							{
								Provider.provider.statisticsService.userStatisticsService.setStatistic("Travel_Vehicle", num4 + (int)(base.transform.position - this.lastStatPos).magnitude);
							}
							this.lastStatPos = base.transform.position;
						}
					}
				}
				else if ((base.transform.position - this.lastStatPos).sqrMagnitude > 256f)
				{
					this.lastStatPos = base.transform.position;
				}
				else if (Time.realtimeSinceStartup - this.lastStatTime > 1f)
				{
					this.lastStatTime = Time.realtimeSinceStartup;
					if ((base.transform.position - this.lastStatPos).sqrMagnitude > 0.1f)
					{
						int num5;
						if (Provider.provider.statisticsService.userStatisticsService.getStatistic("Travel_Foot", out num5))
						{
							Provider.provider.statisticsService.userStatisticsService.setStatistic("Travel_Foot", num5 + (int)(base.transform.position - this.lastStatPos).magnitude);
						}
						this.lastStatPos = base.transform.position;
					}
				}
			}
			else if (!Provider.isServer)
			{
				if (base.player.stance.stance == EPlayerStance.SITTING || base.player.stance.stance == EPlayerStance.DRIVING)
				{
					this._isMoving = false;
					base.transform.localPosition = Vector3.zero;
				}
				else
				{
					if (Mathf.Abs(this.lastUpdatePos.x - base.transform.position.x) > 0.01f || Mathf.Abs(this.lastUpdatePos.y - base.transform.position.y) > 0.01f || Mathf.Abs(this.lastUpdatePos.z - base.transform.position.z) > 0.01f)
					{
						this._isMoving = true;
					}
					else
					{
						this._isMoving = false;
					}
					base.transform.localPosition = this.snapshot.pos;
				}
			}
			if (!base.channel.isOwner && base.player.third != null)
			{
				if (base.player.stance.stance == EPlayerStance.PRONE)
				{
					base.player.third.localPosition = new Vector3(0f, -0.1f, 0f);
				}
				else
				{
					base.player.third.localPosition = Vector3.zero;
				}
			}
		}

		private void updateRegionAndBound()
		{
			byte b;
			byte b2;
			if (Regions.tryGetCoordinate(base.transform.position, out b, out b2) && (b != this.region_x || b2 != this.region_y))
			{
				byte region_x = this.region_x;
				byte region_y = this.region_y;
				this._region_x = b;
				this._region_y = b2;
				this.updateRegionOld_X = region_x;
				this.updateRegionOld_Y = region_y;
				this.updateRegionNew_X = b;
				this.updateRegionNew_Y = b2;
				this.updateRegionIndex = 0;
			}
			if (this.updateRegionIndex < 6)
			{
				bool flag = true;
				if (this.onRegionUpdated != null)
				{
					this.onRegionUpdated(base.player, this.updateRegionOld_X, this.updateRegionOld_Y, this.updateRegionNew_X, this.updateRegionNew_Y, this.updateRegionIndex, ref flag);
				}
				if (flag)
				{
					this.updateRegionIndex += 1;
				}
			}
			byte b3;
			LevelNavigation.tryGetBounds(base.transform.position, out b3);
			if (b3 != this.bound)
			{
				byte bound = this.bound;
				this._bound = b3;
				if (this.onBoundUpdated != null)
				{
					this.onBoundUpdated(base.player, bound, b3);
				}
			}
			if (Provider.isServer)
			{
				byte b4;
				LevelNavigation.tryGetNavigation(base.transform.position, out b4);
				if (b4 != this.nav)
				{
					byte nav = this.nav;
					this._nav = b4;
					this.TriggerPlayerNavChanged(nav, b4);
				}
			}
			bool flag2 = false;
			bool flag3 = false;
			PurchaseNode purchaseNode = null;
			this.effectNode = null;
			this.isSafeInfo = null;
			this.inRain = !Level.info.configData.Use_Rain_Volumes;
			this.inSnow = LevelLighting.isPositionSnowy(base.transform.position);
			for (int i = 0; i < LevelNodes.nodes.Count; i++)
			{
				Node node = LevelNodes.nodes[i];
				if (node.type == ENodeType.SAFEZONE)
				{
					if (!flag2)
					{
						SafezoneNode safezoneNode = (SafezoneNode)node;
						if (safezoneNode.isHeight)
						{
							if (base.transform.position.y > safezoneNode.point.y)
							{
								flag2 = true;
							}
						}
						else if ((base.transform.position - safezoneNode.point).sqrMagnitude < safezoneNode.radius)
						{
							flag2 = true;
						}
						if (flag2)
						{
							this.isSafeInfo = safezoneNode;
						}
					}
				}
				else if (node.type == ENodeType.PURCHASE)
				{
					if (purchaseNode == null)
					{
						PurchaseNode purchaseNode2 = (PurchaseNode)node;
						if ((base.transform.position - purchaseNode2.point).sqrMagnitude < purchaseNode2.radius)
						{
							purchaseNode = purchaseNode2;
						}
					}
				}
				else if (node.type == ENodeType.DEADZONE)
				{
					if (!flag3)
					{
						DeadzoneNode deadzoneNode = (DeadzoneNode)node;
						if ((base.transform.position - deadzoneNode.point).sqrMagnitude < deadzoneNode.radius)
						{
							flag3 = true;
						}
					}
				}
				else if (node.type == ENodeType.EFFECT)
				{
					if (base.channel.isOwner)
					{
						if (this.effectNode == null)
						{
							EffectNode effectNode = (EffectNode)node;
							if (effectNode.shape == ENodeShape.SPHERE)
							{
								if ((base.transform.position - effectNode.point).sqrMagnitude < effectNode.radius)
								{
									this.effectNode = effectNode;
								}
							}
							else if (effectNode.shape == ENodeShape.BOX && Mathf.Abs(base.transform.position.x - effectNode.point.x) < effectNode.bounds.x && Mathf.Abs(base.transform.position.y - effectNode.point.y) < effectNode.bounds.y && Mathf.Abs(base.transform.position.z - effectNode.point.z) < effectNode.bounds.z)
							{
								this.effectNode = effectNode;
							}
						}
					}
				}
			}
			AmbianceVolume ambianceVolume;
			if (AmbianceUtility.isPointInsideVolume(base.transform.position, out ambianceVolume))
			{
				this.effectNode = ambianceVolume;
				if (!this.inRain && Level.info.configData.Use_Rain_Volumes)
				{
					this.inRain = ambianceVolume.canRain;
				}
				if (!this.inSnow && Level.info.configData.Use_Snow_Volumes)
				{
					this.inSnow = ambianceVolume.canSnow;
				}
			}
			this.inSnow &= (LevelLighting.snowyness == ELightingSnow.BLIZZARD);
			DeadzoneVolume deadzoneVolume;
			if (!flag3 && DeadzoneUtility.isPointInsideVolume(base.transform.position, out deadzoneVolume))
			{
				flag3 = true;
			}
			if (flag2 != this.isSafe)
			{
				this._isSafe = flag2;
				if (this.onSafetyUpdated != null)
				{
					this.onSafetyUpdated(this.isSafe);
				}
			}
			if (flag3 != this.isRadiated)
			{
				this._isRadiated = flag3;
				if (this.onRadiationUpdated != null)
				{
					this.onRadiationUpdated(this.isRadiated);
				}
			}
			if (purchaseNode != this.purchaseNode)
			{
				this._purchaseNode = purchaseNode;
				if (this.onPurchaseUpdated != null)
				{
					this.onPurchaseUpdated(this.purchaseNode);
				}
			}
		}

		private void Start()
		{
			this._multiplier = 1f;
			this.multiplier = 1f;
			this.gravity = 1f;
			this.slope = 1f;
			this._region_x = byte.MaxValue;
			this._region_y = byte.MaxValue;
			this._bound = byte.MaxValue;
			this._nav = byte.MaxValue;
			if (base.channel.isOwner || Provider.isServer)
			{
				this._loadedRegions = new LoadedRegion[(int)Regions.WORLD_SIZE, (int)Regions.WORLD_SIZE];
				for (byte b = 0; b < Regions.WORLD_SIZE; b += 1)
				{
					for (byte b2 = 0; b2 < Regions.WORLD_SIZE; b2 += 1)
					{
						this.loadedRegions[(int)b, (int)b2] = new LoadedRegion();
					}
				}
				this._loadedBounds = new LoadedBound[LevelNavigation.bounds.Count];
				byte b3 = 0;
				while ((int)b3 < LevelNavigation.bounds.Count)
				{
					this.loadedBounds[(int)b3] = new LoadedBound();
					b3 += 1;
				}
			}
			this.warp_x = 1;
			this.warp_y = 1;
			if (Provider.isServer || base.channel.isOwner)
			{
				this.controller = base.GetComponent<CharacterController>();
				PlayerLook look = base.player.look;
				look.onPerspectiveUpdated = (PerspectiveUpdated)Delegate.Combine(look.onPerspectiveUpdated, new PerspectiveUpdated(this.onPerspectiveUpdated));
			}
			if (Provider.isServer)
			{
				PlayerLife life = base.player.life;
				life.onVisionUpdated = (VisionUpdated)Delegate.Combine(life.onVisionUpdated, new VisionUpdated(this.onVisionUpdated));
				PlayerLife life2 = base.player.life;
				life2.onLifeUpdated = (LifeUpdated)Delegate.Combine(life2.onLifeUpdated, new LifeUpdated(this.onLifeUpdated));
			}
			else
			{
				this.nsb = new NetworkSnapshotBuffer(Provider.UPDATE_TIME, Provider.UPDATE_DELAY);
			}
			this.applySize();
			if (Dedicator.isDedicated)
			{
				base.gameObject.AddComponent<Rigidbody>();
				base.GetComponent<Rigidbody>().useGravity = false;
				base.GetComponent<Rigidbody>().isKinematic = true;
			}
			this.updateMovement();
			this.updates = new List<PlayerStateUpdate>();
			this.canAddSimulationResultsToUpdates = true;
			this.lastFootstep = Time.time;
		}

		private void OnDrawGizmos()
		{
			if (this.nsb == null)
			{
				return;
			}
			for (int i = 0; i < this.nsb.snapshots.Length; i++)
			{
				if (this.nsb.snapshots[i].timestamp <= 0.01f)
				{
					return;
				}
				PitchYawSnapshotInfo pitchYawSnapshotInfo = (PitchYawSnapshotInfo)this.nsb.snapshots[i].info;
				Gizmos.DrawLine(pitchYawSnapshotInfo.pos, pitchYawSnapshotInfo.pos + Vector3.up * 2f);
			}
		}

		private void OnDestroy()
		{
			this.updates = null;
		}

		public static readonly float HEIGHT_STAND = 2f;

		public static readonly float HEIGHT_CROUCH = 1.2f;

		public static readonly float HEIGHT_PRONE = 0.8f;

		private static bool _forceTrustClient;

		public Landed onLanded;

		public Seated onSeated;

		public VehicleUpdated onVehicleUpdated;

		public SafetyUpdated onSafetyUpdated;

		public RadiationUpdated onRadiationUpdated;

		public PurchaseUpdated onPurchaseUpdated;

		public PlayerRegionUpdated onRegionUpdated;

		public PlayerBoundUpdated onBoundUpdated;

		private static readonly float SPEED_CLIMB = 4.5f;

		private static readonly float SPEED_SWIM = 3f;

		private static readonly float SPEED_SPRINT = 7f;

		private static readonly float SPEED_STAND = 4.5f;

		private static readonly float SPEED_CROUCH = 2.5f;

		private static readonly float SPEED_PRONE = 1.5f;

		private static readonly float JUMP = 7f;

		private static readonly float SWIM = 3f;

		private CharacterController controller;

		public float _multiplier;

		public float multiplier;

		public float gravity;

		private float slope;

		private float fell;

		private bool lastGrounded;

		private float lastFootstep;

		private bool _isGrounded;

		private bool _isSafe;

		public SafezoneNode isSafeInfo;

		private bool _isRadiated;

		private PurchaseNode _purchaseNode;

		public bool inRain;

		public bool inSnow;

		private EPhysicsMaterial material;

		private RaycastHit ground;

		private EPlayerHeight height;

		private bool wasSizeAppliedYet;

		private bool _isMoving;

		private Vector3 _move;

		private byte _region_x;

		private byte _region_y;

		private byte _bound;

		private byte _nav;

		private byte updateRegionOld_X;

		private byte updateRegionOld_Y;

		private byte updateRegionNew_X;

		private byte updateRegionNew_Y;

		private byte updateRegionIndex;

		private LoadedRegion[,] _loadedRegions;

		private LoadedBound[] _loadedBounds;

		private Vector3 direction;

		private float fall2;

		private Vector3 _real;

		private Vector3 lastUpdatePos;

		public PitchYawSnapshotInfo snapshot;

		private NetworkSnapshotBuffer nsb;

		private float lastTick;

		private byte _horizontal;

		private byte _vertical;

		private int warp_x;

		private int warp_y;

		private int input_x;

		private int input_y;

		private bool _jump;

		private bool isRecovering;

		private float lastRecover;

		private bool isFrozen;

		public bool isAllowed;

		[Obsolete]
		public bool isUpdated;

		public List<PlayerStateUpdate> updates;

		public bool canAddSimulationResultsToUpdates;

		private bool isSeating;

		private InteractableVehicle seatingVehicle;

		private byte seatingSeat;

		private Transform seatingTransform;

		private Vector3 seatingPosition;

		private byte seatingAngle;

		private Vector3 lastStatPos;

		private float lastStatTime;

		private InteractableVehicle vehicle;

		private byte seat;
	}
}
