using System;
using UnityEngine;

namespace SDG.Framework.Landscapes
{
	public struct LandscapeBounds
	{
		public LandscapeBounds(LandscapeCoord newMin, LandscapeCoord newMax)
		{
			this.min = newMin;
			this.max = newMax;
		}

		public LandscapeBounds(Bounds worldBounds)
		{
			this.min = new LandscapeCoord(worldBounds.min);
			this.max = new LandscapeCoord(worldBounds.max);
		}

		public override string ToString()
		{
			return string.Concat(new object[]
			{
				'[',
				this.min.ToString(),
				", ",
				this.max.ToString(),
				']'
			});
		}

		public LandscapeCoord min;

		public LandscapeCoord max;
	}
}
