using System;

namespace SDG.Unturned
{
	public struct GUIDTableIndex
	{
		public GUIDTableIndex(ushort index)
		{
			this.index = index;
		}

		public static implicit operator GUIDTableIndex(ushort value)
		{
			return new GUIDTableIndex(value);
		}

		public static explicit operator ushort(GUIDTableIndex value)
		{
			return value.index;
		}

		public static GUIDTableIndex invalid = new GUIDTableIndex(ushort.MaxValue);

		public ushort index;
	}
}
