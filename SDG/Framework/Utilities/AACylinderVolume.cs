using System;
using UnityEngine;

namespace SDG.Framework.Utilities
{
	public struct AACylinderVolume : IShapeVolume
	{
		public AACylinderVolume(Vector3 newCenter, float newRadius, float newHeight)
		{
			this.center = newCenter;
			this.radius = newRadius;
			this.height = newHeight;
		}

		public bool containsPoint(Vector3 point)
		{
			float num = this.height / 2f;
			if (point.y > this.center.y - num && point.y < this.center.y + num)
			{
				float num2 = this.radius * this.radius;
				return (new Vector2(point.x, point.z) - new Vector2(this.center.x, this.center.z)).sqrMagnitude < num2;
			}
			return false;
		}

		public Bounds worldBounds
		{
			get
			{
				float num = this.radius * 2f;
				return new Bounds(this.center, new Vector3(num, this.height, num));
			}
		}

		public float internalVolume
		{
			get
			{
				return this.height * 3.14159274f * this.radius * this.radius;
			}
		}

		public float surfaceArea
		{
			get
			{
				return 3.14159274f * this.radius * this.radius;
			}
		}

		public Vector3 center;

		public float radius;

		public float height;
	}
}
