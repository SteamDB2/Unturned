using System;

namespace SDG.Unturned
{
	public struct RegionCoordinate
	{
		public RegionCoordinate(byte x, byte y)
		{
			this.x = x;
			this.y = y;
		}

		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"(",
				this.x,
				", ",
				this.y,
				")"
			});
		}

		public byte x;

		public byte y;
	}
}
