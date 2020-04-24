using System;
using UnityEngine;

namespace SDG.Framework.Landscapes
{
	public struct LandscapePreviewSample
	{
		public LandscapePreviewSample(Vector3 newPosition, float newWeight)
		{
			this.position = newPosition;
			this.weight = newWeight;
		}

		public Vector3 position;

		public float weight;
	}
}
