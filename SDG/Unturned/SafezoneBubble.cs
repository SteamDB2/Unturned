using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class SafezoneBubble
	{
		public SafezoneBubble(Vector3 newOrigin, float newSqrRadius)
		{
			this.origin = newOrigin;
			this.sqrRadius = newSqrRadius;
		}

		public Vector3 origin;

		public float sqrRadius;
	}
}
