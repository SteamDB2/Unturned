using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class RaycastInfo
	{
		public RaycastInfo(RaycastHit hit)
		{
			this.transform = hit.transform;
			this.collider = hit.collider;
			this.distance = hit.distance;
			this.point = hit.point;
			this.normal = hit.normal;
			this.section = byte.MaxValue;
		}

		public RaycastInfo(Transform hit)
		{
			this.transform = hit;
			this.point = hit.position;
			this.section = byte.MaxValue;
		}

		public Transform transform;

		public Collider collider;

		public float distance;

		public Vector3 point;

		public Vector3 direction;

		public Vector3 normal;

		public Player player;

		public Zombie zombie;

		public Animal animal;

		public ELimb limb;

		public EPhysicsMaterial material;

		public InteractableVehicle vehicle;

		public byte section;
	}
}
