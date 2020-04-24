using System;
using UnityEngine;

namespace SDG.Framework.Foliage
{
	public struct FoliagePreviewSample
	{
		public FoliagePreviewSample(Vector3 newPosition, Color newColor)
		{
			this.position = newPosition;
			this.color = newColor;
		}

		public Vector3 position;

		public Color color;
	}
}
