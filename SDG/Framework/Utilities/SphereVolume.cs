using System;
using UnityEngine;

namespace SDG.Framework.Utilities
{
	public struct SphereVolume : IShapeVolume
	{
		public SphereVolume(Vector3 newCenter, float newRadius)
		{
			this.center = newCenter;
			this.radius = newRadius;
		}

		public bool containsPoint(Vector3 point)
		{
			if (Mathf.Abs(point.x - this.center.x) >= this.radius)
			{
				return false;
			}
			if (Mathf.Abs(point.y - this.center.y) >= this.radius)
			{
				return false;
			}
			if (Mathf.Abs(point.z - this.center.z) >= this.radius)
			{
				return false;
			}
			float num = this.radius * this.radius;
			return (point - this.center).sqrMagnitude < num;
		}

		public Bounds worldBounds
		{
			get
			{
				float num = this.radius * 2f;
				return new Bounds(this.center, new Vector3(num, num, num));
			}
		}

		public float internalVolume
		{
			get
			{
				return 4.18879032f * this.radius * this.radius * this.radius;
			}
		}

		public float surfaceArea
		{
			get
			{
				return 12.566371f * this.radius * this.radius;
			}
		}

		public Vector3 center;

		public float radius;
	}
}
