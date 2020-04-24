using System;
using UnityEngine;

namespace SDG.Framework.Landscapes
{
	public struct HeightmapCoord : IEquatable<HeightmapCoord>
	{
		public HeightmapCoord(int new_x, int new_y)
		{
			this.x = new_x;
			this.y = new_y;
		}

		public HeightmapCoord(LandscapeCoord tileCoord, Vector3 worldPosition)
		{
			this.x = Mathf.Clamp(Mathf.RoundToInt((worldPosition.z - (float)tileCoord.y * Landscape.TILE_SIZE) / Landscape.TILE_SIZE * (float)Landscape.HEIGHTMAP_RESOLUTION_MINUS_ONE), 0, Landscape.HEIGHTMAP_RESOLUTION_MINUS_ONE);
			this.y = Mathf.Clamp(Mathf.RoundToInt((worldPosition.x - (float)tileCoord.x * Landscape.TILE_SIZE) / Landscape.TILE_SIZE * (float)Landscape.HEIGHTMAP_RESOLUTION_MINUS_ONE), 0, Landscape.HEIGHTMAP_RESOLUTION_MINUS_ONE);
		}

		public static bool operator ==(HeightmapCoord a, HeightmapCoord b)
		{
			return a.x == b.x && a.y == b.y;
		}

		public static bool operator !=(HeightmapCoord a, HeightmapCoord b)
		{
			return !(a == b);
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			HeightmapCoord heightmapCoord = (HeightmapCoord)obj;
			return this.x == heightmapCoord.x && this.y == heightmapCoord.y;
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

		public bool Equals(HeightmapCoord other)
		{
			return this.x == other.x && this.y == other.y;
		}

		public int x;

		public int y;
	}
}
