using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class PlayerSpawnpoint
	{
		public PlayerSpawnpoint(Vector3 newPoint, float newAngle, bool newIsAlt)
		{
			this._point = newPoint;
			this._angle = newAngle;
			this._isAlt = newIsAlt;
			if (Level.isEditor)
			{
				this._node = ((GameObject)Object.Instantiate(Resources.Load((!this.isAlt) ? "Edit/Player" : "Edit/Player_Alt"))).transform;
				this.node.name = "Player";
				this.node.position = this.point;
				this.node.rotation = Quaternion.Euler(0f, this.angle, 0f);
				this.node.parent = LevelPlayers.models;
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

		public bool isAlt
		{
			get
			{
				return this._isAlt;
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

		private Vector3 _point;

		private float _angle;

		private bool _isAlt;

		private Transform _node;
	}
}
