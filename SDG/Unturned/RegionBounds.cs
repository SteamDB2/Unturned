using System;
using UnityEngine;

namespace SDG.Unturned
{
	public struct RegionBounds
	{
		public RegionBounds(RegionCoord newMin, RegionCoord newMax)
		{
			this.min = newMin;
			this.max = newMax;
		}

		public RegionBounds(Bounds worldBounds)
		{
			this.min = new RegionCoord(worldBounds.min);
			this.max = new RegionCoord(worldBounds.max);
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

		public RegionCoord min;

		public RegionCoord max;
	}
}
