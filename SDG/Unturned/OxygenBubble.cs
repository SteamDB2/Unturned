using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class OxygenBubble
	{
		public OxygenBubble(Transform newOrigin, float newSqrRadius)
		{
			this.origin = newOrigin;
			this.sqrRadius = newSqrRadius;
		}

		public Transform origin;

		public float sqrRadius;
	}
}
