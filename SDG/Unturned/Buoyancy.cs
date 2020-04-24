using System;
using System.Collections.Generic;
using SDG.Framework.Water;
using UnityEngine;

namespace SDG.Unturned
{
	public class Buoyancy : MonoBehaviour
	{
		private void FixedUpdate()
		{
			for (int i = 0; i < this.voxels.Count; i++)
			{
				Vector3 vector = base.transform.TransformPoint(this.voxels[i]);
				bool flag;
				float num;
				WaterUtility.getUnderwaterInfo(vector, out flag, out num);
				if (flag)
				{
					if (!Dedicator.isDedicated)
					{
						num += Mathf.Sin((vector.x + vector.z) * 8f + Time.time) * 0.1f;
					}
					if (vector.y - this.voxelHalfHeight < num)
					{
						Vector3 pointVelocity = this.rootRigidbody.GetPointVelocity(vector);
						Vector3 vector2 = -pointVelocity * Buoyancy.DAMPER * this.rootRigidbody.mass;
						Vector3 vector3 = vector2 + Mathf.Sqrt(Mathf.Clamp01((num - vector.y) / (2f * this.voxelHalfHeight) + 0.5f)) * this.localArchimedesForce;
						this.rootRigidbody.AddForceAtPosition(vector3, vector);
					}
				}
			}
		}

		private void Awake()
		{
			this.rootRigidbody = base.gameObject.GetComponentInParent<Rigidbody>();
			this.volumeCollider = base.GetComponent<Collider>();
			Vector3 position = base.transform.position;
			Quaternion rotation = base.transform.rotation;
			base.transform.position = Vector3.zero;
			base.transform.rotation = Quaternion.identity;
			Bounds bounds = this.volumeCollider.bounds;
			if (bounds.size.x < bounds.size.y)
			{
				this.voxelHalfHeight = bounds.size.x;
			}
			else
			{
				this.voxelHalfHeight = bounds.size.y;
			}
			if (bounds.size.z < this.voxelHalfHeight)
			{
				this.voxelHalfHeight = bounds.size.z;
			}
			this.voxelHalfHeight /= (float)(2 * this.slicesPerAxis);
			this.voxels = new List<Vector3>(this.slicesPerAxis * this.slicesPerAxis * this.slicesPerAxis);
			for (int i = 0; i < this.slicesPerAxis; i++)
			{
				for (int j = 0; j < this.slicesPerAxis; j++)
				{
					for (int k = 0; k < this.slicesPerAxis; k++)
					{
						float num = bounds.min.x + bounds.size.x / (float)this.slicesPerAxis * (0.5f + (float)i);
						float num2 = bounds.min.y + bounds.size.y / (float)this.slicesPerAxis * (0.5f + (float)j);
						float num3 = bounds.min.z + bounds.size.z / (float)this.slicesPerAxis * (0.5f + (float)k);
						Vector3 vector = base.transform.InverseTransformPoint(new Vector3(num, num2, num3));
						bool flag = true;
						for (int l = 0; l < Buoyancy.DIRECTIONS.Length; l++)
						{
							if (!this.volumeCollider.Raycast(new Ray(vector - Buoyancy.DIRECTIONS[l] * 1000f, Buoyancy.DIRECTIONS[l]), ref Buoyancy.insideHit, 1000f))
							{
								flag = false;
								break;
							}
						}
						if (flag)
						{
							this.voxels.Add(vector);
						}
					}
				}
			}
			if (this.voxels.Count == 0)
			{
				this.voxels.Add(bounds.center);
			}
			base.transform.position = position;
			base.transform.rotation = rotation;
			float num4 = this.rootRigidbody.mass / this.density;
			float num5 = Buoyancy.WATER_DENSITY * Mathf.Abs(Physics.gravity.y) * num4;
			this.localArchimedesForce = new Vector3(0f, num5, 0f) / (float)this.voxels.Count;
		}

		private static readonly float DAMPER = 0.1f;

		private static readonly float WATER_DENSITY = 1000f;

		private static readonly Vector3[] DIRECTIONS = new Vector3[]
		{
			Vector3.up,
			Vector3.down,
			Vector3.left,
			Vector3.right,
			Vector3.forward,
			Vector3.back
		};

		private static RaycastHit insideHit;

		public float density = 500f;

		public int slicesPerAxis = 2;

		private float voxelHalfHeight;

		private Vector3 localArchimedesForce;

		private List<Vector3> voxels;

		private Rigidbody rootRigidbody;

		private Collider volumeCollider;
	}
}
