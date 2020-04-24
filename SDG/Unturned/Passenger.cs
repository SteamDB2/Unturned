using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class Passenger
	{
		public Passenger(Transform newSeat, Transform newObj, Transform newTurretYaw, Transform newTurretPitch, Transform newTurretAim)
		{
			this._seat = newSeat;
			this._obj = newObj;
			this._turretYaw = newTurretYaw;
			this._turretPitch = newTurretPitch;
			this._turretAim = newTurretAim;
			if (this.turretYaw != null)
			{
				this.rotationYaw = this.turretYaw.localRotation;
			}
			if (this.turretPitch != null)
			{
				this.rotationPitch = this.turretPitch.localRotation;
			}
		}

		public Transform seat
		{
			get
			{
				return this._seat;
			}
		}

		public Transform obj
		{
			get
			{
				return this._obj;
			}
		}

		public Quaternion rotationYaw { get; private set; }

		public Transform turretYaw
		{
			get
			{
				return this._turretYaw;
			}
		}

		public Quaternion rotationPitch { get; private set; }

		public Transform turretPitch
		{
			get
			{
				return this._turretPitch;
			}
		}

		public Transform turretAim
		{
			get
			{
				return this._turretAim;
			}
		}

		public SteamPlayer player;

		public TurretInfo turret;

		private Transform _seat;

		private Transform _obj;

		private Transform _turretYaw;

		private Transform _turretPitch;

		private Transform _turretAim;

		public byte[] state;
	}
}
