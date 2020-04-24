using System;
using SDG.Framework.IO.FormattedFiles;
using UnityEngine;

namespace SDG.Framework.Foliage
{
	public struct FoliageCoord : IFormattedFileReadable, IFormattedFileWritable, IEquatable<FoliageCoord>
	{
		public FoliageCoord(int new_x, int new_y)
		{
			this.x = new_x;
			this.y = new_y;
		}

		public FoliageCoord(Vector3 position)
		{
			this.x = Mathf.FloorToInt(position.x / FoliageSystem.TILE_SIZE);
			this.y = Mathf.FloorToInt(position.z / FoliageSystem.TILE_SIZE);
		}

		public void read(IFormattedFileReader reader)
		{
			reader = reader.readObject();
			this.x = reader.readValue<int>("X");
			this.y = reader.readValue<int>("Y");
		}

		public void write(IFormattedFileWriter writer)
		{
			writer.beginObject();
			writer.writeValue<int>("X", this.x);
			writer.writeValue<int>("Y", this.y);
			writer.endObject();
		}

		public static bool operator ==(FoliageCoord a, FoliageCoord b)
		{
			return a.x == b.x && a.y == b.y;
		}

		public static bool operator !=(FoliageCoord a, FoliageCoord b)
		{
			return !(a == b);
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			FoliageCoord foliageCoord = (FoliageCoord)obj;
			return this.x == foliageCoord.x && this.y == foliageCoord.y;
		}

		public override int GetHashCode()
		{
			return this.x ^ this.y;
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

		public bool Equals(FoliageCoord other)
		{
			return this.x == other.x && this.y == other.y;
		}

		public static FoliageCoord ZERO = new FoliageCoord(0, 0);

		public int x;

		public int y;
	}
}
