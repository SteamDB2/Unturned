using System;
using SDG.Framework.IO.FormattedFiles;
using UnityEngine;

namespace SDG.Unturned
{
	public struct RegionCoord : IFormattedFileReadable, IFormattedFileWritable, IEquatable<RegionCoord>
	{
		public RegionCoord(byte new_x, byte new_y)
		{
			this.x = new_x;
			this.y = new_y;
		}

		public RegionCoord(Vector3 position)
		{
			Regions.tryGetCoordinate(position, out this.x, out this.y);
		}

		public void read(IFormattedFileReader reader)
		{
			reader = reader.readObject();
			this.x = reader.readValue<byte>("X");
			this.y = reader.readValue<byte>("Y");
		}

		public void write(IFormattedFileWriter writer)
		{
			writer.beginObject();
			writer.writeValue<byte>("X", this.x);
			writer.writeValue<byte>("Y", this.y);
			writer.endObject();
		}

		public static bool operator ==(RegionCoord a, RegionCoord b)
		{
			return a.x == b.x && a.y == b.y;
		}

		public static bool operator !=(RegionCoord a, RegionCoord b)
		{
			return !(a == b);
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			RegionCoord regionCoord = (RegionCoord)obj;
			return this.x == regionCoord.x && this.y == regionCoord.y;
		}

		public override int GetHashCode()
		{
			return (int)(this.x ^ this.y);
		}

		public override string ToString()
		{
			return string.Concat(new object[]
			{
				'(',
				this.x.ToString(),
				", ",
				this.y.ToString(),
				')'
			});
		}

		public bool Equals(RegionCoord other)
		{
			return this.x == other.x && this.y == other.y;
		}

		public static RegionCoord ZERO = new RegionCoord(0, 0);

		public byte x;

		public byte y;
	}
}
