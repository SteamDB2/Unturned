using System;
using SDG.Framework.Utilities;
using UnityEngine;

namespace SDG.Unturned
{
	public class Wheel
	{
		public Wheel(InteractableVehicle newVehicle, WheelCollider newWheel, bool newSteered, bool newPowered)
		{
			this._vehicle = newVehicle;
			this._wheel = newWheel;
			if (this.wheel != null)
			{
				this.sidewaysFriction = this.wheel.sidewaysFriction;
				this.forwardFriction = this.wheel.forwardFriction;
				this.wheel.forceAppPointDistance = 0f;
			}
			this._isSteered = newSteered;
			this._isPowered = newPowered;
			this.isAlive = true;
		}

		public InteractableVehicle vehicle
		{
			get
			{
				return this._vehicle;
			}
		}

		public WheelCollider wheel
		{
			get
			{
				return this._wheel;
			}
		}

		public bool isSteered
		{
			get
			{
				return this._isSteered;
			}
		}

		public bool isPowered
		{
			get
			{
				return this._isPowered;
			}
		}

		public bool isGrounded
		{
			get
			{
				return this._isGrounded;
			}
		}

		public bool isAlive
		{
			get
			{
				return this._isAlive;
			}
			set
			{
				if (this.isAlive == value)
				{
					return;
				}
				this._isAlive = value;
				if (this.model != null)
				{
					this.model.gameObject.SetActive(this.isAlive);
				}
				this.updateColliderEnabled();
				this.triggerAliveChanged();
			}
		}

		protected virtual void triggerAliveChanged()
		{
			if (this.aliveChanged != null)
			{
				this.aliveChanged(this);
			}
		}

		public bool isPhysical
		{
			get
			{
				return this._isPhysical;
			}
			set
			{
				this._isPhysical = value;
				this.updateColliderEnabled();
			}
		}

		public event WheelAliveChangedHandler aliveChanged;

		public void askRepair()
		{
			if (this.isAlive)
			{
				return;
			}
			this.isAlive = true;
			this.vehicle.sendTireAliveMaskUpdate();
		}

		public void askDamage()
		{
			if (!this.isAlive)
			{
				return;
			}
			this.isAlive = false;
			this.vehicle.sendTireAliveMaskUpdate();
			EffectManager.sendEffect(138, EffectManager.SMALL, this.wheel.transform.position, this.wheel.transform.up);
		}

		protected virtual void updateColliderEnabled()
		{
			if (this.wheel != null)
			{
				this.wheel.gameObject.SetActive(this.isPhysical && this.isAlive);
			}
		}

		public void reset()
		{
			this.direction = 0f;
			this.steer = 0f;
			this.speed = 0f;
			if (this.wheel != null)
			{
				this.wheel.steerAngle = 0f;
				this.wheel.motorTorque = 0f;
				this.wheel.brakeTorque = this.vehicle.asset.brake * 0.25f;
				this.sidewaysFriction.stiffness = 0.25f;
				this.wheel.sidewaysFriction = this.sidewaysFriction;
				this.forwardFriction.stiffness = 0.25f;
				this.wheel.forwardFriction = this.forwardFriction;
			}
		}

		public void simulate(float input_x, float input_y, bool inputBrake, float delta)
		{
			if (this.wheel == null)
			{
				return;
			}
			if (this.isSteered)
			{
				this.direction = input_x;
				this.steer = Mathf.Lerp(this.steer, Mathf.Lerp(this.vehicle.asset.steerMax, this.vehicle.asset.steerMin, this.vehicle.factor), 2f * delta);
			}
			if (this.isPowered)
			{
				if (input_y > 0f)
				{
					if (this.vehicle.asset.engine == EEngine.PLANE)
					{
						if (this.vehicle.speed < 0f)
						{
							this.speed = Mathf.Lerp(this.speed, this.vehicle.asset.speedMax / 2f, delta / 4f);
						}
						else
						{
							this.speed = Mathf.Lerp(this.speed, this.vehicle.asset.speedMax / 2f, delta / 8f);
						}
					}
					else if (this.vehicle.speed < 0f)
					{
						this.speed = Mathf.Lerp(this.speed, this.vehicle.asset.speedMax, 2f * delta);
					}
					else
					{
						this.speed = Mathf.Lerp(this.speed, this.vehicle.asset.speedMax, delta);
					}
				}
				else if (input_y < 0f)
				{
					if (this.vehicle.speed > 0f)
					{
						this.speed = Mathf.Lerp(this.speed, this.vehicle.asset.speedMin, 2f * delta);
					}
					else
					{
						this.speed = Mathf.Lerp(this.speed, this.vehicle.asset.speedMin, delta);
					}
				}
				else
				{
					this.speed = Mathf.Lerp(this.speed, 0f, delta);
				}
			}
			if (inputBrake)
			{
				this.speed = 0f;
				this.wheel.motorTorque = 0f;
				this.wheel.brakeTorque = this.vehicle.asset.brake * (1f - this.vehicle.slip * 0.5f);
			}
			else
			{
				this.wheel.brakeTorque = 0f;
			}
			RaycastHit raycastHit;
			this._isGrounded = PhysicsUtility.raycast(new Ray(this.wheel.transform.position, -this.wheel.transform.up), out raycastHit, this.wheel.suspensionDistance + this.wheel.radius, RayMasks.BLOCK_COLLISION, 0);
		}

		public void update(float delta)
		{
			if (this.wheel == null)
			{
				return;
			}
			this.wheel.steerAngle = Mathf.Lerp(this.wheel.steerAngle, this.direction * this.steer, 4f * delta);
			if (this.vehicle.asset.hasSleds)
			{
				this.sidewaysFriction.stiffness = Mathf.Lerp(this.wheel.sidewaysFriction.stiffness, 0.25f, 4f * delta);
				this.forwardFriction.stiffness = Mathf.Lerp(this.wheel.forwardFriction.stiffness, 0.25f, 4f * delta);
			}
			else
			{
				this.sidewaysFriction.stiffness = Mathf.Lerp(this.wheel.sidewaysFriction.stiffness, 1f - this.vehicle.slip * 0.75f, 4f * delta);
				this.forwardFriction.stiffness = Mathf.Lerp(this.wheel.forwardFriction.stiffness, 2f - this.vehicle.slip * 1.5f, 4f * delta);
			}
			this.wheel.sidewaysFriction = this.sidewaysFriction;
			this.wheel.forwardFriction = this.forwardFriction;
			if (this.speed > 0f)
			{
				if (this.vehicle.speed < 0f)
				{
					this.wheel.motorTorque = Mathf.Lerp(this.wheel.motorTorque, this.speed, 4f * delta);
				}
				else if (this.vehicle.speed < this.vehicle.asset.speedMax)
				{
					this.wheel.motorTorque = Mathf.Lerp(this.wheel.motorTorque, this.speed, 2f * delta);
				}
				else
				{
					this.wheel.motorTorque = Mathf.Lerp(this.wheel.motorTorque, this.speed / 2f, 2f * delta);
				}
			}
			else if (this.vehicle.speed > 0f)
			{
				this.wheel.motorTorque = Mathf.Lerp(this.wheel.motorTorque, this.speed, 4f * delta);
			}
			else if (this.vehicle.speed > this.vehicle.asset.speedMin)
			{
				this.wheel.motorTorque = Mathf.Lerp(this.wheel.motorTorque, this.speed, 2f * delta);
			}
			else
			{
				this.wheel.motorTorque = Mathf.Lerp(this.wheel.motorTorque, this.speed / 2f, 2f * delta);
			}
		}

		public void checkForTraps()
		{
			if (this.wheel == null)
			{
				return;
			}
			if (!this.isAlive)
			{
				return;
			}
			if (Provider.isServer && this.vehicle.asset != null && this.vehicle.asset.canTiresBeDamaged)
			{
				RaycastHit raycastHit;
				Physics.Raycast(new Ray(this.wheel.transform.position, -this.wheel.transform.up), ref raycastHit, this.wheel.suspensionDistance + this.wheel.radius, RayMasks.BARRICADE);
				if (raycastHit.transform != null && raycastHit.transform.CompareTag("Barricade"))
				{
					InteractableTrapDamageTires component = raycastHit.transform.GetComponent<InteractableTrapDamageTires>();
					if (component != null)
					{
						this.askDamage();
					}
				}
			}
		}

		private InteractableVehicle _vehicle;

		private WheelCollider _wheel;

		public Transform model;

		private WheelFrictionCurve forwardFriction;

		private WheelFrictionCurve sidewaysFriction;

		private bool _isSteered;

		private bool _isPowered;

		private bool _isGrounded;

		protected bool _isAlive;

		private float direction;

		private float steer;

		private float speed;

		protected bool _isPhysical;
	}
}
