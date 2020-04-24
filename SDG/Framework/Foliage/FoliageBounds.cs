using System;
using UnityEngine;

namespace SDG.Framework.Foliage
{
	public struct FoliageBounds
	{
		public FoliageBounds(FoliageCoord newMin, FoliageCoord newMax)
		{
			this.min = newMin;
			this.max = newMax;
		}

		public FoliageBounds(Bounds worldBounds)
		{
			this.min = new FoliageCoord(worldBounds.min);
			this.max = new FoliageCoord(worldBounds.max);
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

		public FoliageCoord min;

		public FoliageCoord max;
	}
}
