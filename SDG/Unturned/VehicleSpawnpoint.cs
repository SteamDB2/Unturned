﻿using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class VehicleSpawnpoint
	{
		public VehicleSpawnpoint(byte newType, Vector3 newPoint, float newAngle)
		{
			this.type = newType;
			this._point = newPoint;
			this._angle = newAngle;
			if (Level.isEditor)
			{
				this._node = ((GameObject)Object.Instantiate(Resources.Load("Edit/Vehicle"))).transform;
				this.node.name = this.type.ToString();
				this.node.position = this.point;
				this.node.rotation = Quaternion.Euler(0f, this.angle, 0f);
				this.node.parent = LevelVehicles.models;
				this.node.GetComponent<Renderer>().material.color = LevelVehicles.tables[(int)this.type].color;
				this.node.FindChild("Arrow").GetComponent<Renderer>().material.color = LevelVehicles.tables[(int)this.type].color;
			}
		}

		public Vector3 point
		{
			get
			{
				return this._point;
			}
		}

		public float angle
		{
			get
			{
				return this._angle;
			}
		}

		public Transform node
		{
			get
			{
				return this._node;
			}
		}

		public void setEnabled(bool isEnabled)
		{
			this.node.transform.gameObject.SetActive(isEnabled);
		}

		public byte type;

		private Vector3 _point;

		private float _angle;

		private Transform _node;
	}
}
