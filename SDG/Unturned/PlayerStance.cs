using System;
using SDG.Framework.Water;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	public class PlayerStance : PlayerCaller
	{
		public EPlayerStance stance
		{
			get
			{
				return this._stance;
			}
			set
			{
				this._stance = value;
				if (value != this.changeStance)
				{
					if (this.stance == EPlayerStance.STAND || this.stance == EPlayerStance.SPRINT || this.stance == EPlayerStance.CLIMB || this.stance == EPlayerStance.SWIM || this.stance == EPlayerStance.DRIVING || this.stance == EPlayerStance.SITTING)
					{
						base.player.movement.setSize(EPlayerHeight.STAND);
					}
					else if (this.stance == EPlayerStance.CROUCH)
					{
						base.player.movement.setSize(EPlayerHeight.CROUCH);
					}
					else if (this.stance == EPlayerStance.PRONE)
					{
						base.player.movement.setSize(EPlayerHeight.PRONE);
					}
				}
				this.changeStance = this.stance;
			}
		}

		public float radius
		{
			get
			{
				if (base.player.movement.nav != 255 && ZombieManager.regions[(int)base.player.movement.nav].isHyper)
				{
					return 24f;
				}
				if (this.stance == EPlayerStance.DRIVING)
				{
					if (base.player.movement.getVehicle().sirensOn)
					{
						return PlayerStance.DETECT_FORWARD;
					}
					if (base.player.movement.getVehicle().speed > 0f)
					{
						return PlayerStance.DETECT_FORWARD * base.player.movement.getVehicle().speed / base.player.movement.getVehicle().asset.speedMax;
					}
					return PlayerStance.DETECT_BACKWARD * base.player.movement.getVehicle().speed / base.player.movement.getVehicle().asset.speedMin;
				}
				else
				{
					if (this.stance == EPlayerStance.SITTING)
					{
						return 0f;
					}
					if (this.stance == EPlayerStance.SPRINT)
					{
						return PlayerStance.DETECT_SPRINT * ((!base.player.movement.isMoving) ? 1f : PlayerStance.DETECT_MOVE);
					}
					if (this.stance == EPlayerStance.STAND || this.stance == EPlayerStance.SWIM)
					{
						float num = 1f - base.player.skills.mastery(1, 0) * 0.5f;
						return PlayerStance.DETECT_STAND * ((!base.player.movement.isMoving) ? 1f : PlayerStance.DETECT_MOVE) * num;
					}
					float num2 = 1f - base.player.skills.mastery(1, 0) * 0.75f;
					if (this.stance == EPlayerStance.CROUCH || this.stance == EPlayerStance.CLIMB)
					{
						return PlayerStance.DETECT_CROUCH * ((!base.player.movement.isMoving) ? 1f : PlayerStance.DETECT_MOVE) * num2;
					}
					if (this.stance == EPlayerStance.PRONE)
					{
						return PlayerStance.DETECT_PRONE * ((!base.player.movement.isMoving) ? 1f : PlayerStance.DETECT_MOVE) * num2;
					}
					return 0f;
				}
			}
		}

		public bool crouch
		{
			get
			{
				return this._crouch;
			}
		}

		public bool prone
		{
			get
			{
				return this._prone;
			}
		}

		public bool sprint
		{
			get
			{
				return this._sprint;
			}
		}

		public bool isSubmerged
		{
			get
			{
				return this._isSubmerged;
			}
		}

		private bool checkSpace(float height)
		{
			return Physics.OverlapSphereNonAlloc(base.transform.position + Vector3.up * height, PlayerStance.RADIUS, PlayerStance.checkColliders, RayMasks.BLOCK_STANCE, 1) == 0;
		}

		public void checkStance(EPlayerStance newStance)
		{
			this.checkStance(newStance, false);
		}

		public void checkStance(EPlayerStance newStance, bool all)
		{
			if (base.player.movement.getVehicle() != null && newStance != EPlayerStance.DRIVING && newStance != EPlayerStance.SITTING)
			{
				return;
			}
			if (newStance == this.stance)
			{
				return;
			}
			if ((newStance == EPlayerStance.PRONE || newStance == EPlayerStance.CROUCH) && (!base.player.movement.isGrounded || base.player.movement.fall > 0f))
			{
				return;
			}
			if (newStance == EPlayerStance.STAND && (this.stance == EPlayerStance.CROUCH || this.stance == EPlayerStance.PRONE))
			{
				if (Time.realtimeSinceStartup - this.lastStance <= PlayerStance.COOLDOWN)
				{
					return;
				}
				this.lastStance = Time.realtimeSinceStartup;
				if (!this.checkSpace(1.5f))
				{
					return;
				}
				if (!this.checkSpace(1f))
				{
					return;
				}
				if (!this.checkSpace(0.5f))
				{
					return;
				}
			}
			if (newStance == EPlayerStance.CROUCH && this.stance == EPlayerStance.PRONE)
			{
				if (Time.realtimeSinceStartup - this.lastStance <= PlayerStance.COOLDOWN)
				{
					return;
				}
				this.lastStance = Time.realtimeSinceStartup;
				if (!this.checkSpace(1f))
				{
					return;
				}
				if (!this.checkSpace(0.5f))
				{
					return;
				}
			}
			if (Provider.isServer)
			{
				if (base.player.animator.gesture == EPlayerGesture.INVENTORY_START)
				{
					if (newStance != EPlayerStance.STAND && newStance != EPlayerStance.SPRINT && newStance != EPlayerStance.CROUCH)
					{
						base.player.animator.sendGesture(EPlayerGesture.INVENTORY_STOP, false);
					}
				}
				else if (base.player.animator.gesture == EPlayerGesture.SURRENDER_START)
				{
					base.player.animator.sendGesture(EPlayerGesture.SURRENDER_STOP, true);
				}
				else if (base.player.animator.gesture == EPlayerGesture.REST_START)
				{
					base.player.animator.sendGesture(EPlayerGesture.REST_STOP, true);
				}
			}
			this.stance = newStance;
			if (this.onStanceUpdated != null)
			{
				this.onStanceUpdated();
			}
			if (Provider.isServer)
			{
				if (all)
				{
					base.channel.send("tellStance", ESteamCall.ALL, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[]
					{
						(byte)this.stance
					});
				}
				else
				{
					base.channel.send("tellStance", ESteamCall.NOT_OWNER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[]
					{
						(byte)this.stance
					});
				}
			}
		}

		[SteamCall]
		public void tellStance(CSteamID steamID, byte newStance)
		{
			if (base.channel.checkServer(steamID))
			{
				this.stance = (EPlayerStance)newStance;
				if (this.stance == EPlayerStance.CROUCH)
				{
					if (ControlsSettings.crouching == EControlMode.TOGGLE)
					{
						this._crouch = true;
						this._prone = false;
					}
				}
				else if (this.stance == EPlayerStance.PRONE && ControlsSettings.proning == EControlMode.TOGGLE)
				{
					this._crouch = false;
					this._prone = true;
				}
			}
		}

		[SteamCall]
		public void askStance(CSteamID steamID)
		{
			if (Provider.isServer)
			{
				base.channel.send("tellStance", steamID, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
				{
					(byte)this.stance
				});
			}
		}

		public void simulate(uint simulation, bool inputCrouch, bool inputProne, bool inputSprint)
		{
			this._isSubmerged = WaterUtility.isPointUnderwater(base.player.look.aim.position);
			if (this.stance == EPlayerStance.CLIMB || ((this.stance == EPlayerStance.STAND || this.stance == EPlayerStance.SPRINT || this.stance == EPlayerStance.SWIM) && !base.player.equipment.isBusy))
			{
				Physics.Raycast(base.transform.position + Vector3.up * 0.5f, base.transform.forward, ref this.ladder, 0.75f, RayMasks.LADDER_INTERACT);
				if (this.ladder.transform != null && this.ladder.transform.CompareTag("Ladder") && Mathf.Abs(Vector3.Dot(this.ladder.normal, this.ladder.transform.up)) > 0.9f)
				{
					if (this.stance != EPlayerStance.CLIMB)
					{
						Vector3 vector = new Vector3(this.ladder.transform.position.x, this.ladder.point.y - 0.5f, this.ladder.transform.position.z) + this.ladder.normal * 0.5f;
						if (!Physics.CapsuleCast(base.transform.position + new Vector3(0f, PlayerStance.RADIUS, 0f), base.transform.position + new Vector3(0f, PlayerMovement.HEIGHT_STAND - PlayerStance.RADIUS, 0f), PlayerStance.RADIUS, (vector - base.transform.position).normalized, (vector - base.transform.position).magnitude, RayMasks.BLOCK_LADDER, 1))
						{
							base.transform.position = vector;
							this.checkStance(EPlayerStance.CLIMB);
						}
					}
					if (this.stance == EPlayerStance.CLIMB)
					{
						return;
					}
				}
				else if (this.stance == EPlayerStance.CLIMB)
				{
					this.checkStance(EPlayerStance.STAND);
				}
			}
			bool flag = WaterUtility.isPointUnderwater(base.transform.position);
			if (WaterUtility.isPointUnderwater(base.transform.position + new Vector3(0f, 1.25f, 0f)))
			{
				if (this.stance != EPlayerStance.SWIM)
				{
					this.checkStance(EPlayerStance.SWIM);
				}
				return;
			}
			if (flag)
			{
				if (this.stance != EPlayerStance.STAND && this.stance != EPlayerStance.SPRINT)
				{
					this.checkStance(EPlayerStance.STAND);
				}
			}
			else if (this.stance == EPlayerStance.SWIM)
			{
				this.checkStance(EPlayerStance.STAND);
			}
			if (this.stance != EPlayerStance.CLIMB && this.stance != EPlayerStance.SITTING && this.stance != EPlayerStance.DRIVING)
			{
				if (inputCrouch != this.lastCrouch)
				{
					this.lastCrouch = inputCrouch;
					if (!flag)
					{
						if (inputCrouch)
						{
							this.checkStance(EPlayerStance.CROUCH);
						}
						else if (this.stance == EPlayerStance.CROUCH)
						{
							this.checkStance(EPlayerStance.STAND);
						}
					}
				}
				if (inputProne != this.lastProne)
				{
					this.lastProne = inputProne;
					if (!flag)
					{
						if (inputProne)
						{
							this.checkStance(EPlayerStance.PRONE);
						}
						else if (this.stance == EPlayerStance.PRONE)
						{
							this.checkStance(EPlayerStance.STAND);
						}
					}
				}
				if (inputSprint != this.lastSprint)
				{
					this.lastSprint = inputSprint;
					if (inputSprint)
					{
						if (this.stance == EPlayerStance.STAND && !base.player.life.isBroken && base.player.life.stamina > 0 && (double)base.player.movement.multiplier > 0.9 && base.player.movement.isMoving)
						{
							this.checkStance(EPlayerStance.SPRINT);
						}
					}
					else if (this.stance == EPlayerStance.SPRINT)
					{
						this.checkStance(EPlayerStance.STAND);
					}
				}
				if (this.stance == EPlayerStance.SPRINT && (base.player.life.isBroken || base.player.life.stamina == 0 || (double)base.player.movement.multiplier < 0.9 || !base.player.movement.isMoving))
				{
					this.checkStance(EPlayerStance.STAND);
				}
			}
			else
			{
				this.lastCrouch = false;
				this.lastProne = false;
				this.lastSprint = false;
			}
		}

		private void onLifeUpdated(bool isDead)
		{
			if (!isDead)
			{
				this.checkStance(EPlayerStance.STAND);
			}
		}

		private void Update()
		{
			if (base.channel.isOwner && !PlayerUI.window.showCursor)
			{
				if (!base.player.look.isOrbiting)
				{
					if (Input.GetKey(ControlsSettings.stance))
					{
						if (this.isHolding)
						{
							if (Time.realtimeSinceStartup - this.lastHold > 0.33f)
							{
								this._crouch = false;
								this._prone = true;
							}
						}
						else
						{
							this.isHolding = true;
							this.lastHold = Time.realtimeSinceStartup;
						}
					}
					else if (this.isHolding)
					{
						if (Time.realtimeSinceStartup - this.lastHold < 0.33f)
						{
							if (this.crouch)
							{
								this._crouch = false;
								this._prone = false;
							}
							else
							{
								this._crouch = true;
								this._prone = false;
							}
						}
						this.isHolding = false;
					}
					if (ControlsSettings.crouching == EControlMode.TOGGLE)
					{
						if (Input.GetKey(ControlsSettings.crouch) != this.flipCrouch)
						{
							this.flipCrouch = Input.GetKey(ControlsSettings.crouch);
							if (this.flipCrouch)
							{
								this._crouch = !this.crouch;
							}
						}
					}
					else
					{
						this._crouch = Input.GetKey(ControlsSettings.crouch);
						this.flipCrouch = this.crouch;
					}
					if (ControlsSettings.proning == EControlMode.TOGGLE)
					{
						if (Input.GetKey(ControlsSettings.prone) != this.flipProne)
						{
							this.flipProne = Input.GetKey(ControlsSettings.prone);
							if (this.flipProne)
							{
								this._prone = !this.prone;
							}
						}
					}
					else
					{
						this._prone = Input.GetKey(ControlsSettings.prone);
						this.flipProne = this.prone;
					}
					if (ControlsSettings.sprinting == EControlMode.TOGGLE)
					{
						if (Input.GetKey(ControlsSettings.sprint) != this.flipSprint)
						{
							this.flipSprint = Input.GetKey(ControlsSettings.sprint);
							if (this.flipSprint)
							{
								this._sprint = !this.sprint;
							}
						}
					}
					else
					{
						this._sprint = Input.GetKey(ControlsSettings.sprint);
						this.flipSprint = this.sprint;
					}
				}
				if ((this.stance == EPlayerStance.PRONE || this.stance == EPlayerStance.CROUCH) && Input.GetKey(ControlsSettings.jump))
				{
					this._crouch = false;
					this._prone = false;
				}
				if (this.stance == EPlayerStance.CLIMB || this.stance == EPlayerStance.SITTING || this.stance == EPlayerStance.DRIVING)
				{
					this._crouch = false;
					this._prone = false;
					this._sprint = false;
				}
				if (PlayerUI.window.showCursor)
				{
					this._sprint = false;
				}
			}
			if (Provider.isServer && (double)(Time.realtimeSinceStartup - this.lastDetect) > 0.1)
			{
				this.lastDetect = Time.realtimeSinceStartup;
				if (!base.player.life.isDead)
				{
					AlertTool.alert(base.player, base.transform.position, this.radius, this.stance != EPlayerStance.SPRINT && this.stance != EPlayerStance.DRIVING, base.player.look.aim.forward, base.player.isSpotOn);
				}
			}
		}

		private void Start()
		{
			this._stance = EPlayerStance.STAND;
			if (base.channel.isOwner || Provider.isServer)
			{
				this.lastStance = float.MinValue;
				PlayerLife life = base.player.life;
				life.onLifeUpdated = (LifeUpdated)Delegate.Combine(life.onLifeUpdated, new LifeUpdated(this.onLifeUpdated));
			}
			if (Provider.isServer && (!this.checkSpace(1.5f) || !this.checkSpace(1f) || !this.checkSpace(0.5f)))
			{
				this.stance = EPlayerStance.PRONE;
			}
			if (!Provider.isServer)
			{
				base.channel.send("askStance", ESteamCall.SERVER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[0]);
			}
		}

		private static Collider[] checkColliders = new Collider[1];

		public static readonly float COOLDOWN = 0.75f;

		public static readonly float RADIUS = 0.4f;

		public static readonly float DETECT_MOVE = 1.1f;

		public static readonly float DETECT_FORWARD = 48f;

		public static readonly float DETECT_BACKWARD = 24f;

		public static readonly float DETECT_SPRINT = 20f;

		public static readonly float DETECT_STAND = 12f;

		public static readonly float DETECT_CROUCH = 5f;

		public static readonly float DETECT_PRONE = 2f;

		public StanceUpdated onStanceUpdated;

		private EPlayerStance changeStance;

		private EPlayerStance _stance;

		private float lastStance;

		private float lastDetect;

		private float lastHold;

		private bool isHolding;

		private bool flipCrouch;

		private bool lastCrouch;

		private bool _crouch;

		private bool flipProne;

		private bool lastProne;

		private bool _prone;

		private bool flipSprint;

		private bool lastSprint;

		private bool _sprint;

		private bool _isSubmerged;

		private RaycastHit ladder;
	}
}
