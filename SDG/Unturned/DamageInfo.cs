using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class DamageInfo
	{
		public void update(RaycastHit hit)
		{
			this.transform = hit.transform;
			this.collider = hit.collider;
			this.distance = hit.distance;
			this.point = hit.point;
			this.normal = hit.normal;
		}

		public Transform transform;

		public Collider collider;

		public float distance;

		public Vector3 point;

		public Vector3 normal;

		public Player player;

		public Zombie zombie;

		public InteractableVehicle vehicle;
	}
}
