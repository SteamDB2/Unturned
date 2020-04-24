using System;
using SDG.Framework.Foliage;
using SDG.Framework.Utilities;
using UnityEngine;

namespace SDG.Unturned
{
	public class PlayerLook : PlayerCaller
	{
		private float heightLook
		{
			get
			{
				if (base.player.stance.stance == EPlayerStance.DRIVING || base.player.stance.stance == EPlayerStance.SITTING)
				{
					return PlayerLook.HEIGHT_LOOK_SIT;
				}
				if (base.player.stance.stance == EPlayerStance.STAND || base.player.stance.stance == EPlayerStance.SPRINT || base.player.stance.stance == EPlayerStance.CLIMB || base.player.stance.stance == EPlayerStance.SWIM || base.player.stance.stance == EPlayerStance.DRIVING || base.player.stance.stance == EPlayerStance.SITTING)
				{
					return PlayerLook.HEIGHT_LOOK_STAND;
				}
				if (base.player.stance.stance == EPlayerStance.CROUCH)
				{
					return PlayerLook.HEIGHT_LOOK_CROUCH;
				}
				if (base.player.stance.stance == EPlayerStance.PRONE)
				{
					return PlayerLook.HEIGHT_LOOK_PRONE;
				}
				return 0f;
			}
		}

		private float heightCamera
		{
			get
			{
				if (base.player.stance.stance == EPlayerStance.DRIVING || base.player.stance.stance == EPlayerStance.SITTING)
				{
					return PlayerLook.HEIGHT_CAMERA_SIT;
				}
				if (base.player.stance.stance == EPlayerStance.STAND || base.player.stance.stance == EPlayerStance.SPRINT || base.player.stance.stance == EPlayerStance.CLIMB || base.player.stance.stance == EPlayerStance.SWIM || base.player.stance.stance == EPlayerStance.DRIVING || base.player.stance.stance == EPlayerStance.SITTING)
				{
					return PlayerLook.HEIGHT_CAMERA_STAND;
				}
				if (base.player.stance.stance == EPlayerStance.CROUCH)
				{
					return PlayerLook.HEIGHT_CAMERA_CROUCH;
				}
				if (base.player.stance.stance == EPlayerStance.PRONE)
				{
					return PlayerLook.HEIGHT_CAMERA_PRONE;
				}
				return 0f;
			}
		}

		public Camera characterCamera
		{
			get
			{
				return this._characterCamera;
			}
		}

		public Camera scopeCamera
		{
			get
			{
				return this._scopeCamera;
			}
		}

		public Camera highlightCamera
		{
			get
			{
				return this._highlightCamera;
			}
		}

		public bool isScopeActive
		{
			get
			{
				return this._isScopeActive;
			}
		}

		public Transform aim
		{
			get
			{
				return this._aim;
			}
		}

		public float pitch
		{
			get
			{
				return this._pitch;
			}
		}

		public float yaw
		{
			get
			{
				return this._yaw;
			}
		}

		public float look_x
		{
			get
			{
				return this._look_x;
			}
		}

		public float look_y
		{
			get
			{
				return this._look_y;
			}
		}

		public float orbitPitch
		{
			get
			{
				return this._orbitPitch;
			}
		}

		public float orbitYaw
		{
			get
			{
				return this._orbitYaw;
			}
		}

		public bool isCam
		{
			get
			{
				return this.isOrbiting || this.isTracking || this.isLocking || this.isFocusing;
			}
		}

		public EPlayerPerspective perspective
		{
			get
			{
				return this._perspective;
			}
		}

		public void updateScope(EGraphicQuality quality)
		{
			if (quality == EGraphicQuality.OFF)
			{
				this.scopeCamera.targetTexture = null;
			}
			else if (quality == EGraphicQuality.LOW)
			{
				this.scopeCamera.targetTexture = (RenderTexture)Resources.Load("RenderTextures/Scope_Low");
			}
			else if (quality == EGraphicQuality.MEDIUM)
			{
				this.scopeCamera.targetTexture = (RenderTexture)Resources.Load("RenderTextures/Scope_Medium");
			}
			else if (quality == EGraphicQuality.HIGH)
			{
				this.scopeCamera.targetTexture = (RenderTexture)Resources.Load("RenderTextures/Scope_High");
			}
			else if (quality == EGraphicQuality.ULTRA)
			{
				this.scopeCamera.targetTexture = (RenderTexture)Resources.Load("RenderTextures/Scope_Ultra");
			}
			this.scopeCamera.enabled = (this.isScopeActive && this.scopeCamera.targetTexture != null && this.scopeVision == ELightingVision.NONE);
			if (base.player.equipment.asset != null && base.player.equipment.asset.type == EItemType.GUN)
			{
				base.player.equipment.useable.updateState(base.player.equipment.state);
			}
		}

		public void enableScope(float zoom, ELightingVision vision)
		{
			this.scopeCamera.fieldOfView = zoom;
			this._isScopeActive = true;
			this.scopeVision = vision;
			this.scopeCamera.enabled = (this.scopeCamera.targetTexture != null && this.scopeVision == ELightingVision.NONE);
		}

		public void disableScope()
		{
			this.scopeCamera.enabled = false;
			this._isScopeActive = false;
			this.scopeVision = ELightingVision.NONE;
		}

		public void enableOverlay()
		{
			if (this.scopeVision == ELightingVision.NONE)
			{
				return;
			}
			if (this.scopeCamera.targetTexture != null)
			{
				return;
			}
			this.enableVision();
			this.isOverlayActive = true;
		}

		public void setPerspective(EPlayerPerspective newPerspective)
		{
			this._perspective = newPerspective;
			if (this.perspective == EPlayerPerspective.FIRST)
			{
				MainCamera.instance.transform.parent = base.player.first;
				MainCamera.instance.transform.localPosition = Vector3.up * this.eyes;
				this.isOrbiting = false;
				this.isTracking = false;
				this.isLocking = false;
				this.isFocusing = false;
			}
			else
			{
				MainCamera.instance.transform.parent = base.player.transform;
			}
			GraphicsSettings.mainProfile.chromaticAberration.enabled = (this.perspective == EPlayerPerspective.THIRD && GraphicsSettings.chromaticAberration);
			GraphicsSettings.mainProfile.grain.enabled = (this.perspective == EPlayerPerspective.THIRD && GraphicsSettings.filmGrain);
			GraphicsSettings.viewProfile.chromaticAberration.enabled = (this.perspective == EPlayerPerspective.FIRST && GraphicsSettings.chromaticAberration);
			GraphicsSettings.viewProfile.grain.enabled = (this.perspective == EPlayerPerspective.FIRST && GraphicsSettings.filmGrain);
			if (this.onPerspectiveUpdated != null)
			{
				this.onPerspectiveUpdated(this.perspective);
			}
		}

		private void enableVision()
		{
			this.tempVision = LevelLighting.vision;
			LevelLighting.vision = this.scopeVision;
			LevelLighting.updateLighting();
			LevelLighting.updateLocal();
			PlayerLifeUI.updateGrayscale();
		}

		public void disableOverlay()
		{
			if (this.perspective != EPlayerPerspective.FIRST)
			{
				this.tempVision = ELightingVision.NONE;
			}
			if (!this.isOverlayActive)
			{
				return;
			}
			this.disableVision();
			this.isOverlayActive = false;
		}

		private void disableVision()
		{
			LevelLighting.vision = this.tempVision;
			LevelLighting.updateLighting();
			LevelLighting.updateLocal();
			PlayerLifeUI.updateGrayscale();
			this.tempVision = ELightingVision.NONE;
		}

		public void enableZoom(float zoom)
		{
			this.fov = zoom;
			this.isZoomed = true;
		}

		public void disableZoom()
		{
			this.fov = 0f;
			this.isZoomed = false;
		}

		public void updateRot()
		{
			if (this.pitch < 0f)
			{
				this.angle = 0;
			}
			else if (this.pitch > 180f)
			{
				this.angle = 180;
			}
			else
			{
				this.angle = (byte)this.pitch;
			}
			this.rot = MeasurementTool.angleToByte(this.yaw);
		}

		public void updateLook()
		{
			this.sensitivity = 1f;
			this._pitch = 90f;
			this._yaw = base.transform.localRotation.eulerAngles.y;
			this.updateRot();
			if (base.channel.isOwner && this.perspective == EPlayerPerspective.FIRST)
			{
				MainCamera.instance.transform.localRotation = Quaternion.Euler(this.pitch - 90f, 0f, 0f);
				MainCamera.instance.transform.localPosition = Vector3.up * this.eyes;
			}
		}

		public void recoil(float x, float y, float h, float v)
		{
			this._yaw += x;
			this._pitch -= y;
			this.recoil_x += x * h;
			this.recoil_y += y * v;
			if ((double)(Time.realtimeSinceStartup - this.lastRecoil) < 0.2)
			{
				this.recoil_x *= 0.6f;
				this.recoil_y *= 0.6f;
			}
			this.lastRecoil = Time.realtimeSinceStartup;
		}

		public void simulate(float look_x, float look_y, float delta)
		{
			this._pitch = look_y;
			this._yaw = look_x;
			this.checkPitch();
			this.updateRot();
			if (base.player.stance.stance == EPlayerStance.DRIVING || base.player.stance.stance == EPlayerStance.SITTING)
			{
				base.transform.localRotation = Quaternion.identity;
			}
			else
			{
				base.transform.localRotation = Quaternion.Euler(0f, this.yaw, 0f);
			}
			if (base.player.movement.getVehicle() != null && base.player.movement.getVehicle().passengers[(int)base.player.movement.getSeat()].turret != null)
			{
				Passenger passenger = base.player.movement.getVehicle().passengers[(int)base.player.movement.getSeat()];
				if (passenger.turretYaw != null)
				{
					passenger.turretYaw.localRotation = passenger.rotationYaw * Quaternion.Euler(0f, this.yaw, 0f);
				}
				if (passenger.turretPitch != null)
				{
					passenger.turretPitch.localRotation = passenger.rotationPitch * Quaternion.Euler(this.pitch - 90f, 0f, 0f);
				}
			}
			this.updateAim(delta);
		}

		private void checkPitch()
		{
			if (base.player.stance.stance == EPlayerStance.DRIVING || base.player.stance.stance == EPlayerStance.SITTING)
			{
				if (base.player.movement.getVehicle() != null && base.player.movement.getVehicle().passengers[(int)base.player.movement.getSeat()].turret != null)
				{
					Passenger passenger = base.player.movement.getVehicle().passengers[(int)base.player.movement.getSeat()];
					if (this.pitch < passenger.turret.pitchMin)
					{
						this._pitch = passenger.turret.pitchMin;
					}
					else if (this.pitch > passenger.turret.pitchMax)
					{
						this._pitch = passenger.turret.pitchMax;
					}
				}
				else if (this.pitch < PlayerLook.MIN_ANGLE_SIT)
				{
					this._pitch = PlayerLook.MIN_ANGLE_SIT;
				}
				else if (this.pitch > PlayerLook.MAX_ANGLE_SIT)
				{
					this._pitch = PlayerLook.MAX_ANGLE_SIT;
				}
			}
			else if (base.player.stance.stance == EPlayerStance.STAND || base.player.stance.stance == EPlayerStance.SPRINT)
			{
				if (this.pitch < PlayerLook.MIN_ANGLE_STAND)
				{
					this._pitch = PlayerLook.MIN_ANGLE_STAND;
				}
				else if (this.pitch > PlayerLook.MAX_ANGLE_STAND)
				{
					this._pitch = PlayerLook.MAX_ANGLE_STAND;
				}
			}
			else if (base.player.stance.stance == EPlayerStance.CLIMB)
			{
				if (this.pitch < PlayerLook.MIN_ANGLE_CLIMB)
				{
					this._pitch = PlayerLook.MIN_ANGLE_CLIMB;
				}
				else if (this.pitch > PlayerLook.MAX_ANGLE_CLIMB)
				{
					this._pitch = PlayerLook.MAX_ANGLE_CLIMB;
				}
			}
			else if (base.player.stance.stance == EPlayerStance.SWIM)
			{
				if (this.pitch < PlayerLook.MIN_ANGLE_SWIM)
				{
					this._pitch = PlayerLook.MIN_ANGLE_SWIM;
				}
				else if (this.pitch > PlayerLook.MAX_ANGLE_SWIM)
				{
					this._pitch = PlayerLook.MAX_ANGLE_SWIM;
				}
			}
			else if (base.player.stance.stance == EPlayerStance.CROUCH)
			{
				if (this.pitch < PlayerLook.MIN_ANGLE_CROUCH)
				{
					this._pitch = PlayerLook.MIN_ANGLE_CROUCH;
				}
				else if (this.pitch > PlayerLook.MAX_ANGLE_CROUCH)
				{
					this._pitch = PlayerLook.MAX_ANGLE_CROUCH;
				}
			}
			else if (base.player.stance.stance == EPlayerStance.PRONE)
			{
				if (this.pitch < PlayerLook.MIN_ANGLE_PRONE)
				{
					this._pitch = PlayerLook.MIN_ANGLE_PRONE;
				}
				else if (this.pitch > PlayerLook.MAX_ANGLE_PRONE)
				{
					this._pitch = PlayerLook.MAX_ANGLE_PRONE;
				}
			}
		}

		public void updateAim(float delta)
		{
			if (base.player.movement.getVehicle() != null && base.player.movement.getVehicle().passengers[(int)base.player.movement.getSeat()].turret != null && base.player.movement.getVehicle().passengers[(int)base.player.movement.getSeat()].turret.useAimCamera)
			{
				Passenger passenger = base.player.movement.getVehicle().passengers[(int)base.player.movement.getSeat()];
				if (passenger.turretAim != null)
				{
					this.aim.position = passenger.turretAim.position;
					this.aim.rotation = passenger.turretAim.rotation;
				}
			}
			else
			{
				this.aim.localPosition = Vector3.Lerp(this.aim.localPosition, Vector3.up * this.heightLook, 4f * delta);
				if (base.player.stance.stance == EPlayerStance.SITTING || base.player.stance.stance == EPlayerStance.DRIVING)
				{
					this.aim.parent.localRotation = Quaternion.Euler(0f, this.yaw, 0f);
				}
				else
				{
					this.aim.parent.localRotation = Quaternion.Lerp(this.aim.parent.localRotation, Quaternion.Euler(0f, 0f, (float)base.player.animator.lean * HumanAnimator.LEAN), 4f * delta);
				}
				this.aim.localRotation = Quaternion.Euler(this.pitch - 90f + base.player.animator.viewSway.x, base.player.animator.viewSway.y, 0f);
			}
		}

		private void onDamaged(byte damage)
		{
			if (damage > 25)
			{
				damage = 25;
			}
			if ((double)Random.value < 0.5)
			{
				this.dodge -= (float)(2 * damage) * (1f - base.player.skills.mastery(1, 3) * 0.75f);
			}
			else
			{
				this.dodge += (float)(2 * damage) * (1f - base.player.skills.mastery(1, 3) * 0.75f);
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
			if (isDead)
			{
				PlayerLook.killcam = base.transform.rotation.eulerAngles.y;
			}
		}

		private void onSeated(bool isDriver, bool inVehicle, bool wasVehicle, InteractableVehicle oldVehicle, InteractableVehicle newVehicle)
		{
			if (!wasVehicle)
			{
				this._orbitPitch = 22.5f;
				this._orbitYaw = 0f;
			}
			if (Provider.cameraMode == ECameraMode.VEHICLE && this.perspective == EPlayerPerspective.THIRD && !isDriver)
			{
				this.setPerspective(EPlayerPerspective.FIRST);
			}
		}

		private void Update()
		{
			if (base.channel.isOwner)
			{
				if (base.channel.owner.isAdmin && this.perspective == EPlayerPerspective.THIRD && Input.GetKey(304))
				{
					if (Input.GetKeyDown(282))
					{
						this.isOrbiting = !this.isOrbiting;
						if (this.isOrbiting && !this.isTracking && !this.isLocking && !this.isFocusing)
						{
							this.isTracking = true;
						}
					}
					if (Input.GetKeyDown(283))
					{
						this.isTracking = !this.isTracking;
						if (this.isTracking)
						{
							this.isLocking = false;
							this.isFocusing = false;
						}
					}
					if (Input.GetKeyDown(284))
					{
						this.isLocking = !this.isLocking;
						if (this.isLocking)
						{
							this.isTracking = false;
							this.isFocusing = false;
							this.lockPosition = base.player.first.position;
						}
					}
					if (Input.GetKeyDown(285))
					{
						this.isFocusing = !this.isFocusing;
						if (this.isFocusing)
						{
							this.isTracking = false;
							this.isLocking = false;
							this.lockPosition = base.player.first.position;
						}
					}
					if (Input.GetKeyDown(286))
					{
						this.isSmoothing = !this.isSmoothing;
					}
					if (Input.GetKeyDown(287))
					{
						if (PlayerWorkzoneUI.active)
						{
							PlayerWorkzoneUI.close();
							PlayerLifeUI.open();
						}
						else
						{
							PlayerWorkzoneUI.open();
							PlayerLifeUI.close();
						}
					}
				}
				this.eyes = Mathf.Lerp(this.eyes, this.heightLook, 4f * Time.deltaTime);
				Camera instance = MainCamera.instance;
				if (!base.player.life.isDead && !PlayerUI.window.showCursor && Input.GetKeyDown(ControlsSettings.perspective) && (Provider.cameraMode == ECameraMode.BOTH || (Provider.cameraMode == ECameraMode.VEHICLE && base.player.stance.stance == EPlayerStance.DRIVING)))
				{
					EPlayerPerspective perspective;
					if (this.perspective == EPlayerPerspective.FIRST)
					{
						perspective = EPlayerPerspective.THIRD;
					}
					else
					{
						perspective = EPlayerPerspective.FIRST;
					}
					this.setPerspective(perspective);
				}
				if (this.isCam)
				{
					instance.fieldOfView = OptionsSettings.view;
				}
				else
				{
					instance.fieldOfView = Mathf.Lerp(instance.fieldOfView, (this.fov <= 1f) ? (OptionsSettings.view + (float)((base.player.stance.stance != EPlayerStance.SPRINT) ? 0 : 10)) : this.fov, 8f * Time.deltaTime);
				}
				this.highlightCamera.fieldOfView = instance.fieldOfView;
				this._look_x = 0f;
				this._look_y = 0f;
				if (!PlayerUI.window.showCursor && !this.isIgnoringInput)
				{
					if (this.isOrbiting)
					{
						if (!base.player.workzone.isBuilding || Input.GetKey(ControlsSettings.secondary))
						{
							this._orbitYaw += ControlsSettings.look * Input.GetAxis("mouse_x") * (float)this.warp_x;
							if (ControlsSettings.invert)
							{
								this._orbitPitch += ControlsSettings.look * Input.GetAxis("mouse_y") * (float)this.warp_y;
							}
							else
							{
								this._orbitPitch -= ControlsSettings.look * Input.GetAxis("mouse_y") * (float)this.warp_y;
							}
						}
					}
					else
					{
						if (this.perspective == EPlayerPerspective.FIRST || this.isTracking || this.isLocking || this.isFocusing)
						{
							this._look_x = ControlsSettings.look * Input.GetAxis("mouse_x") * (float)this.warp_x;
							this._look_y = ControlsSettings.look * -Input.GetAxis("mouse_y") * (float)this.warp_y;
						}
						if (Input.GetKey(ControlsSettings.rollLeft))
						{
							this._look_x = ((!(base.player.movement.getVehicle() != null)) ? -1f : (-base.player.movement.getVehicle().asset.airTurnResponsiveness));
						}
						else if (Input.GetKey(ControlsSettings.rollRight))
						{
							this._look_x = ((!(base.player.movement.getVehicle() != null)) ? 1f : base.player.movement.getVehicle().asset.airTurnResponsiveness);
						}
						if (Input.GetKey(ControlsSettings.pitchUp))
						{
							this._look_y = ((!(base.player.movement.getVehicle() != null)) ? -1f : (-base.player.movement.getVehicle().asset.airTurnResponsiveness));
						}
						else if (Input.GetKey(ControlsSettings.pitchDown))
						{
							this._look_y = ((!(base.player.movement.getVehicle() != null)) ? 1f : base.player.movement.getVehicle().asset.airTurnResponsiveness);
						}
						if (ControlsSettings.invertFlight)
						{
							this._look_y *= -1f;
						}
						if (base.player.movement.getVehicle() != null && this.perspective == EPlayerPerspective.THIRD)
						{
							this._orbitYaw += ControlsSettings.look * Input.GetAxis("mouse_x") * (float)this.warp_x;
							this._orbitYaw = this.orbitYaw % 360f;
						}
						else if (base.player.movement.getVehicle() == null || !base.player.movement.getVehicle().asset.hasLockMouse || !base.player.movement.getVehicle().isDriver)
						{
							this._yaw += ControlsSettings.look * ((this.perspective != EPlayerPerspective.FIRST) ? 1f : this.sensitivity) * Input.GetAxis("mouse_x") * (float)this.warp_x;
							this._yaw = this.yaw % 360f;
						}
						if (base.player.movement.getVehicle() != null && this.perspective == EPlayerPerspective.THIRD)
						{
							if (ControlsSettings.invert)
							{
								this._orbitPitch += ControlsSettings.look * Input.GetAxis("mouse_y") * (float)this.warp_y;
							}
							else
							{
								this._orbitPitch -= ControlsSettings.look * Input.GetAxis("mouse_y") * (float)this.warp_y;
							}
						}
						else if (base.player.movement.getVehicle() == null || !base.player.movement.getVehicle().asset.hasLockMouse || !base.player.movement.getVehicle().isDriver)
						{
							if (ControlsSettings.invert)
							{
								this._pitch += ControlsSettings.look * ((this.perspective != EPlayerPerspective.FIRST) ? 1f : this.sensitivity) * Input.GetAxis("mouse_y") * (float)this.warp_y;
							}
							else
							{
								this._pitch -= ControlsSettings.look * ((this.perspective != EPlayerPerspective.FIRST) ? 1f : this.sensitivity) * Input.GetAxis("mouse_y") * (float)this.warp_y;
							}
						}
					}
				}
				if (float.IsInfinity(this.yaw) || float.IsNaN(this.yaw))
				{
					this._yaw = 0f;
				}
				if (float.IsInfinity(this.pitch) || float.IsNaN(this.pitch))
				{
					this._pitch = 0f;
				}
				if (float.IsInfinity(this.orbitYaw) || float.IsNaN(this.orbitYaw))
				{
					this._orbitYaw = 0f;
				}
				if (float.IsInfinity(this.orbitPitch) || float.IsNaN(this.orbitPitch))
				{
					this._orbitPitch = 0f;
				}
				this._yaw -= Mathf.Lerp(0f, this.recoil_x, 4f * Time.deltaTime);
				this._pitch += Mathf.Lerp(0f, this.recoil_y, 4f * Time.deltaTime);
				this.recoil_x = Mathf.Lerp(this.recoil_x, 0f, 4f * Time.deltaTime);
				this.recoil_y = Mathf.Lerp(this.recoil_y, 0f, 4f * Time.deltaTime);
				this.dodge = Mathf.LerpAngle(this.dodge, 0f, 4f * Time.deltaTime);
				this.checkPitch();
				if (this.orbitPitch > 90f)
				{
					this._orbitPitch = 90f;
				}
				else if (this.orbitPitch < -90f)
				{
					this._orbitPitch = -90f;
				}
				PlayerLook._characterYaw = Mathf.Lerp(PlayerLook._characterYaw, PlayerLook.characterYaw + 180f, 4f * Time.deltaTime);
				this.characterCamera.transform.rotation = Quaternion.Euler(20f, PlayerLook._characterYaw, 0f);
				this.characterCamera.transform.position = base.player.character.position - this.characterCamera.transform.forward * 3.5f + Vector3.up * PlayerLook.characterHeight;
				if (base.player.life.isDead)
				{
					PlayerLook.killcam += -16f * Time.deltaTime;
					instance.transform.rotation = Quaternion.Lerp(instance.transform.rotation, Quaternion.Euler(32f, PlayerLook.killcam, 0f), 2f * Time.deltaTime);
				}
				else
				{
					if ((base.player.stance.stance == EPlayerStance.DRIVING || base.player.stance.stance == EPlayerStance.SITTING) && this.perspective == EPlayerPerspective.THIRD)
					{
						instance.transform.localRotation = Quaternion.Euler(this.orbitPitch, this.orbitYaw, 0f);
					}
					else if (base.player.stance.stance == EPlayerStance.DRIVING)
					{
						if (this.yaw < -160f)
						{
							this._yaw = -160f;
						}
						else if (this.yaw > 160f)
						{
							this._yaw = 160f;
						}
						instance.transform.localRotation = Quaternion.Euler(this.pitch - 90f, this.yaw / 10f, 0f);
						instance.transform.Rotate(base.transform.up, this.yaw, 0);
					}
					else if (base.player.stance.stance == EPlayerStance.SITTING)
					{
						if (base.player.movement.getVehicle() != null && base.player.movement.getVehicle().passengers[(int)base.player.movement.getSeat()].turret != null)
						{
							Passenger passenger = base.player.movement.getVehicle().passengers[(int)base.player.movement.getSeat()];
							if (this.yaw < passenger.turret.yawMin)
							{
								this._yaw = passenger.turret.yawMin;
							}
							else if (this.yaw > passenger.turret.yawMax)
							{
								this._yaw = passenger.turret.yawMax;
							}
						}
						else if (this.yaw < -90f)
						{
							this._yaw = -90f;
						}
						else if (this.yaw > 90f)
						{
							this._yaw = 90f;
						}
						instance.transform.localRotation = Quaternion.Euler(this.pitch - 90f + base.player.animator.viewSway.x, base.player.animator.viewSway.y, 0f);
						instance.transform.Rotate(base.transform.up, this.yaw, 0);
					}
					else
					{
						if (this.perspective == EPlayerPerspective.FIRST)
						{
							instance.transform.localRotation = Quaternion.Euler(this.pitch - 90f + base.player.animator.viewSway.x, base.player.animator.viewSway.y, this.dodge);
						}
						else
						{
							instance.transform.localRotation = Quaternion.Euler(this.pitch - 90f + base.player.animator.viewSway.x, base.player.animator.shoulder * -5f + base.player.animator.viewSway.y, 0f);
						}
						base.transform.localRotation = Quaternion.Euler(0f, this.yaw, 0f);
					}
					if (this.isCam)
					{
						if (this.isFocusing)
						{
							if (this.isSmoothing)
							{
								this.smoothRotation = Quaternion.Lerp(this.smoothRotation, Quaternion.LookRotation(base.player.first.position + Vector3.up - (this.lockPosition + this.orbitPosition)), 4f * Time.deltaTime);
								instance.transform.rotation = this.smoothRotation;
							}
							else
							{
								instance.transform.rotation = Quaternion.LookRotation(base.player.first.position + Vector3.up - (this.lockPosition + this.orbitPosition));
							}
						}
						else if (this.isSmoothing)
						{
							this.smoothRotation = Quaternion.Lerp(this.smoothRotation, Quaternion.Euler(this.orbitPitch, this.orbitYaw, 0f), 4f * Time.deltaTime);
							instance.transform.rotation = this.smoothRotation;
						}
						else
						{
							instance.transform.rotation = Quaternion.Euler(this.orbitPitch, this.orbitYaw, 0f);
						}
					}
				}
				if (base.player.life.isDead)
				{
					this.cam = instance.transform.forward * -4f;
					PhysicsUtility.raycast(new Ray(base.player.first.position + Vector3.up, this.cam), out this.hit, 4f, RayMasks.BLOCK_KILLCAM, 0);
					if (this.hit.transform != null)
					{
						this.cam = this.hit.point + this.cam.normalized * -0.5f;
					}
					else
					{
						this.cam = base.player.first.position + Vector3.up + this.cam;
					}
					instance.transform.position = this.cam;
				}
				else
				{
					if (this.isCam)
					{
						if (this.isLocking || this.isFocusing)
						{
							instance.transform.position = this.lockPosition + this.orbitPosition;
						}
						else if (this.isOrbiting || this.isTracking)
						{
							if (this.isSmoothing)
							{
								this.smoothPosition = Vector3.Lerp(this.smoothPosition, this.orbitPosition, 4f * Time.deltaTime);
								instance.transform.position = base.player.first.position + this.smoothPosition;
							}
							else
							{
								instance.transform.position = base.player.first.position + this.orbitPosition;
							}
						}
					}
					else if ((base.player.stance.stance == EPlayerStance.DRIVING || base.player.stance.stance == EPlayerStance.SITTING) && this.perspective == EPlayerPerspective.THIRD)
					{
						float num = base.player.movement.getVehicle().asset.camFollowDistance + Mathf.Abs(base.player.movement.getVehicle().spedometer) * 0.1f;
						this.cam = instance.transform.forward * -num;
						PhysicsUtility.raycast(new Ray(base.player.first.transform.position + Vector3.up * this.eyes, this.cam), out this.hit, num, RayMasks.BLOCK_VEHICLECAM, 0);
						if (this.hit.transform != null)
						{
							this.cam = this.hit.point + this.cam.normalized * -0.5f;
						}
						else
						{
							this.cam = base.player.first.transform.position + Vector3.up * this.eyes + this.cam;
						}
						PhysicsUtility.raycast(new Ray(this.cam, instance.transform.right), out this.hit, 0.5f, RayMasks.BLOCK_PLAYERCAM, 0);
						if (this.hit.transform != null)
						{
							this.cam = this.hit.point + instance.transform.right * -0.5f;
						}
						else
						{
							PhysicsUtility.raycast(new Ray(this.cam, -instance.transform.right), out this.hit, 0.5f, RayMasks.BLOCK_PLAYERCAM, 0);
							if (this.hit.transform != null)
							{
								this.cam = this.hit.point + instance.transform.right * 0.5f;
							}
						}
						PhysicsUtility.raycast(new Ray(this.cam, instance.transform.up), out this.hit, 0.5f, RayMasks.BLOCK_PLAYERCAM, 0);
						if (this.hit.transform != null)
						{
							this.cam = this.hit.point + instance.transform.up * -0.5f;
						}
						else
						{
							PhysicsUtility.raycast(new Ray(this.cam, -instance.transform.up), out this.hit, 0.5f, RayMasks.BLOCK_PLAYERCAM, 0);
							if (this.hit.transform != null)
							{
								this.cam = this.hit.point + instance.transform.up * 0.5f;
							}
						}
						instance.transform.position = this.cam;
					}
					else if (base.player.stance.stance == EPlayerStance.DRIVING)
					{
						if (this.yaw > 0f)
						{
							instance.transform.localPosition = Vector3.Lerp(instance.transform.localPosition, Vector3.up * (this.heightLook + base.player.movement.getVehicle().asset.camDriverOffset) - Vector3.left * this.yaw / 360f, 4f * Time.deltaTime);
						}
						else
						{
							instance.transform.localPosition = Vector3.Lerp(instance.transform.localPosition, Vector3.up * (this.heightLook + base.player.movement.getVehicle().asset.camDriverOffset) - Vector3.left * this.yaw / 240f, 4f * Time.deltaTime);
						}
					}
					else if (this.perspective == EPlayerPerspective.FIRST)
					{
						instance.transform.localPosition = Vector3.up * this.eyes;
					}
					else
					{
						if (Provider.modeConfigData.Gameplay.Allow_Shoulder_Camera)
						{
							this.cam = instance.transform.forward * -1.5f + instance.transform.up * 0.25f + instance.transform.right * base.player.animator.shoulder * 1f;
						}
						else
						{
							this.cam = instance.transform.forward * -1.5f + instance.transform.up * 0.5f + instance.transform.right * base.player.animator.shoulder2 * 0.5f;
						}
						PhysicsUtility.raycast(new Ray(base.player.first.position + Vector3.up * this.eyes, this.cam), out this.hit, 2f, RayMasks.BLOCK_PLAYERCAM, 0);
						if (this.hit.transform != null)
						{
							Vector3 normalized = this.cam.normalized;
							RaycastHit raycastHit;
							PhysicsUtility.raycast(new Ray(this.hit.point, -normalized), out raycastHit, 1f, RayMasks.BLOCK_PLAYERCAM, 0);
							if (raycastHit.transform != null)
							{
								this.cam = base.player.first.position + Vector3.up * this.eyes;
							}
							else
							{
								this.cam = this.hit.point + normalized * -0.5f;
							}
						}
						else
						{
							this.cam = base.player.first.position + Vector3.up * this.eyes + this.cam;
						}
						PhysicsUtility.raycast(new Ray(this.cam, instance.transform.right * Mathf.Sign(base.player.animator.shoulder)), out this.hit, 0.5f, RayMasks.BLOCK_PLAYERCAM, 0);
						if (this.hit.transform != null)
						{
							this.cam = this.hit.point + instance.transform.right * Mathf.Sign(base.player.animator.shoulder) * -0.5f;
						}
						PhysicsUtility.raycast(new Ray(this.cam, instance.transform.up), out this.hit, 0.5f, RayMasks.BLOCK_PLAYERCAM, 0);
						if (this.hit.transform != null)
						{
							this.cam = this.hit.point + instance.transform.up * -0.5f;
						}
						else
						{
							PhysicsUtility.raycast(new Ray(this.cam, -instance.transform.up), out this.hit, 0.5f, RayMasks.BLOCK_PLAYERCAM, 0);
							if (this.hit.transform != null)
							{
								this.cam = this.hit.point + instance.transform.up * 0.5f;
							}
						}
						instance.transform.position = this.cam;
					}
					PlayerLook.characterHeight = Mathf.Lerp(PlayerLook.characterHeight, this.heightCamera, 4f * Time.deltaTime);
				}
				if (base.player.movement.getVehicle() != null && base.player.movement.getVehicle().asset.engine == EEngine.PLANE && base.player.movement.getVehicle().spedometer > 16f)
				{
					LevelLighting.updateLocal(instance.transform.position, Mathf.Lerp(0f, 1f, (base.player.movement.getVehicle().spedometer - 16f) / 8f), base.player.movement.effectNode);
				}
				else if (base.player.movement.getVehicle() != null && base.player.movement.getVehicle().asset.engine == EEngine.HELICOPTER && base.player.movement.getVehicle().spedometer > 4f)
				{
					LevelLighting.updateLocal(instance.transform.position, Mathf.Lerp(0f, 1f, (base.player.movement.getVehicle().spedometer - 8f) / 8f), base.player.movement.effectNode);
				}
				else
				{
					LevelLighting.updateLocal(instance.transform.position, 0f, base.player.movement.effectNode);
				}
				base.player.animator.viewmodelLock.rotation = instance.transform.rotation;
				if (this.isScopeActive && this.scopeCamera.targetTexture != null && this.scopeVision != ELightingVision.NONE)
				{
					this.enableVision();
					this.scopeCamera.Render();
					this.disableVision();
				}
				if (base.player.movement.getVehicle() != null && base.player.movement.getVehicle().passengers[(int)base.player.movement.getSeat()].turret != null)
				{
					Passenger passenger2 = base.player.movement.getVehicle().passengers[(int)base.player.movement.getSeat()];
					if (passenger2.turretYaw != null)
					{
						passenger2.turretYaw.localRotation = passenger2.rotationYaw * Quaternion.Euler(0f, this.yaw, 0f);
					}
					if (passenger2.turretPitch != null)
					{
						passenger2.turretPitch.localRotation = passenger2.rotationPitch * Quaternion.Euler(this.pitch - 90f, 0f, 0f);
					}
					if (this.perspective == EPlayerPerspective.FIRST && base.player.movement.getVehicle().passengers[(int)base.player.movement.getSeat()].turret.useAimCamera)
					{
						instance.transform.position = passenger2.turretAim.position;
						instance.transform.rotation = passenger2.turretAim.rotation;
					}
				}
				if (FoliageSettings.drawFocus)
				{
					if (this.isZoomed || (this.isScopeActive && this.scopeCamera.targetTexture != null))
					{
						FoliageSystem.isFocused = true;
						RaycastHit raycastHit2;
						if (Physics.Raycast(MainCamera.instance.transform.position, MainCamera.instance.transform.forward, ref raycastHit2, FoliageSettings.focusDistance, RayMasks.FOLIAGE_FOCUS))
						{
							FoliageSystem.focusPosition = raycastHit2.point;
							if (this.scopeCamera.targetTexture != null)
							{
								FoliageSystem.focusCamera = this.scopeCamera;
							}
							else
							{
								FoliageSystem.focusCamera = MainCamera.instance;
							}
						}
					}
					else
					{
						FoliageSystem.isFocused = false;
					}
				}
			}
			else if (!Provider.isServer)
			{
				if (base.player.stance.stance == EPlayerStance.DRIVING || base.player.stance.stance == EPlayerStance.SITTING)
				{
					base.transform.localRotation = Quaternion.identity;
				}
				else
				{
					this._pitch = base.player.movement.snapshot.pitch;
					this._yaw = base.player.movement.snapshot.yaw;
					base.transform.localRotation = Quaternion.Euler(0f, this.yaw, 0f);
				}
				if (base.player.movement.getVehicle() != null && base.player.movement.getVehicle().passengers[(int)base.player.movement.getSeat()].turret != null)
				{
					Passenger passenger3 = base.player.movement.getVehicle().passengers[(int)base.player.movement.getSeat()];
					if (passenger3.turretYaw != null)
					{
						passenger3.turretYaw.localRotation = passenger3.rotationYaw * Quaternion.Euler(0f, base.player.movement.snapshot.yaw, 0f);
					}
					if (passenger3.turretPitch != null)
					{
						passenger3.turretPitch.localRotation = passenger3.rotationPitch * Quaternion.Euler(base.player.movement.snapshot.pitch - 90f, 0f, 0f);
					}
				}
			}
			if (!Dedicator.isDedicated)
			{
				this.updateAim(Time.deltaTime);
			}
		}

		private void Start()
		{
			this._aim = base.transform.FindChild("Aim").FindChild("Fire");
			this.updateLook();
			this.warp_x = 1;
			this.warp_y = 1;
			if (base.channel.isOwner)
			{
				if (Provider.cameraMode == ECameraMode.THIRD)
				{
					this._perspective = EPlayerPerspective.THIRD;
					MainCamera.instance.transform.parent = base.player.transform;
				}
				else
				{
					this._perspective = EPlayerPerspective.FIRST;
				}
				MainCamera.instance.fieldOfView = OptionsSettings.view;
				PlayerLook.characterHeight = 0f;
				PlayerLook._characterYaw = 180f;
				PlayerLook.characterYaw = 0f;
				this.dodge = 0f;
				if (base.player.character != null)
				{
					this._characterCamera = base.player.character.FindChild("Camera").GetComponent<Camera>();
				}
				this._scopeCamera = MainCamera.instance.transform.FindChild("Scope").GetComponent<Camera>();
				this.scopeCamera.layerCullDistances = MainCamera.instance.layerCullDistances;
				this.scopeCamera.layerCullSpherical = MainCamera.instance.layerCullSpherical;
				this.scopeCamera.fieldOfView = 10f;
				this._highlightCamera = MainCamera.instance.transform.FindChild("HighlightCamera").GetComponent<Camera>();
				this.highlightCamera.fieldOfView = MainCamera.instance.fieldOfView;
				LevelLighting.updateLighting();
				PlayerLife life = base.player.life;
				life.onVisionUpdated = (VisionUpdated)Delegate.Combine(life.onVisionUpdated, new VisionUpdated(this.onVisionUpdated));
				PlayerLife life2 = base.player.life;
				life2.onLifeUpdated = (LifeUpdated)Delegate.Combine(life2.onLifeUpdated, new LifeUpdated(this.onLifeUpdated));
				PlayerLife life3 = base.player.life;
				life3.onDamaged = (Damaged)Delegate.Combine(life3.onDamaged, new Damaged(this.onDamaged));
				PlayerMovement movement = base.player.movement;
				movement.onSeated = (Seated)Delegate.Combine(movement.onSeated, new Seated(this.onSeated));
			}
		}

		private static readonly float HEIGHT_LOOK_SIT = 1.6f;

		private static readonly float HEIGHT_LOOK_STAND = 1.75f;

		private static readonly float HEIGHT_LOOK_CROUCH = 1.2f;

		private static readonly float HEIGHT_LOOK_PRONE = 0.35f;

		private static readonly float HEIGHT_CAMERA_SIT = 0.7f;

		private static readonly float HEIGHT_CAMERA_STAND = 1.05f;

		private static readonly float HEIGHT_CAMERA_CROUCH = 0.95f;

		private static readonly float HEIGHT_CAMERA_PRONE = 0.3f;

		private static readonly float MIN_ANGLE_SIT = 60f;

		private static readonly float MAX_ANGLE_SIT = 120f;

		private static readonly float MIN_ANGLE_CLIMB = 45f;

		private static readonly float MAX_ANGLE_CLIMB = 100f;

		private static readonly float MIN_ANGLE_SWIM = 45f;

		private static readonly float MAX_ANGLE_SWIM = 135f;

		private static readonly float MIN_ANGLE_STAND;

		private static readonly float MAX_ANGLE_STAND = 180f;

		private static readonly float MIN_ANGLE_CROUCH = 20f;

		private static readonly float MAX_ANGLE_CROUCH = 160f;

		private static readonly float MIN_ANGLE_PRONE = 60f;

		private static readonly float MAX_ANGLE_PRONE = 120f;

		public PerspectiveUpdated onPerspectiveUpdated;

		private Camera _characterCamera;

		private Camera _scopeCamera;

		private Camera _highlightCamera;

		private bool _isScopeActive;

		private bool isOverlayActive;

		private ELightingVision scopeVision;

		private ELightingVision tempVision;

		private Transform _aim;

		private static float characterHeight;

		private static float _characterYaw;

		public static float characterYaw;

		private static float killcam;

		private int warp_x;

		private int warp_y;

		private float _pitch;

		private float _yaw;

		private float _look_x;

		private float _look_y;

		private float _orbitPitch;

		private float _orbitYaw;

		public Vector3 lockPosition;

		public Vector3 orbitPosition;

		public bool isOrbiting;

		public bool isTracking;

		public bool isLocking;

		public bool isFocusing;

		public bool isSmoothing;

		public bool isIgnoringInput;

		private Vector3 smoothPosition;

		private Quaternion smoothRotation;

		public byte angle;

		public byte rot;

		private float recoil_x;

		private float recoil_y;

		private float lastRecoil;

		private float lastTick;

		public byte lastAngle;

		public byte lastRot;

		private float dodge;

		private float fov;

		private float eyes;

		private RaycastHit hit;

		private Vector3 cam;

		public float sensitivity;

		private EPlayerPerspective _perspective;

		protected bool isZoomed;
	}
}
