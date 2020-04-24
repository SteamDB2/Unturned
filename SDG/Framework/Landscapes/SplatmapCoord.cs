using System;
using UnityEngine;

namespace SDG.Framework.Landscapes
{
	public struct SplatmapCoord : IEquatable<SplatmapCoord>
	{
		public SplatmapCoord(int new_x, int new_y)
		{
			this.x = new_x;
			this.y = new_y;
		}

		public SplatmapCoord(LandscapeCoord tileCoord, Vector3 worldPosition)
		{
			this.x = Mathf.Clamp(Mathf.FloorToInt((worldPosition.z - (float)tileCoord.y * Landscape.TILE_SIZE) / Landscape.TILE_SIZE * (float)Landscape.SPLATMAP_RESOLUTION), 0, Landscape.SPLATMAP_RESOLUTION_MINUS_ONE);
			this.y = Mathf.Clamp(Mathf.FloorToInt((worldPosition.x - (float)tileCoord.x * Landscape.TILE_SIZE) / Landscape.TILE_SIZE * (float)Landscape.SPLATMAP_RESOLUTION), 0, Landscape.SPLATMAP_RESOLUTION_MINUS_ONE);
		}

		public static bool operator ==(SplatmapCoord a, SplatmapCoord b)
		{
			return a.x == b.x && a.y == b.y;
		}

		public static bool operator !=(SplatmapCoord a, SplatmapCoord b)
		{
			return !(a == b);
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			SplatmapCoord splatmapCoord = (SplatmapCoord)obj;
			return this.x.Equals(splatmapCoord.x) && this.y.Equals(splatmapCoord.y);
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

		public bool Equals(SplatmapCoord other)
		{
			return this.x == other.x && this.y == other.y;
		}

		public int x;

		public int y;
	}
}
