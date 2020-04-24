using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class TemperatureBubble
	{
		public TemperatureBubble(Transform newOrigin, float newSqrRadius, EPlayerTemperature newTemperature)
		{
			this.origin = newOrigin;
			this.sqrRadius = newSqrRadius;
			this.temperature = newTemperature;
		}

		public Transform origin;

		public float sqrRadius;

		public EPlayerTemperature temperature;
	}
}
