using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class AnimalSpawnpoint
	{
		public AnimalSpawnpoint(byte newType, Vector3 newPoint)
		{
			this.type = newType;
			this._point = newPoint;
			if (Level.isEditor)
			{
				this._node = ((GameObject)Object.Instantiate(Resources.Load("Edit/Animal"))).transform;
				this.node.name = this.type.ToString();
				this.node.position = this.point;
				this.node.parent = LevelAnimals.models;
				this.node.GetComponent<Renderer>().material.color = LevelAnimals.tables[(int)this.type].color;
			}
		}

		public Vector3 point
		{
			get
			{
				return this._point;
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

		private Transform _node;
	}
}
