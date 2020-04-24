using System;
using UnityEngine;

namespace SDG.Framework.Utilities
{
	public interface IShapeVolume
	{
		bool containsPoint(Vector3 point);

		Bounds worldBounds { get; }

		float internalVolume { get; }

		float surfaceArea { get; }
	}
}
