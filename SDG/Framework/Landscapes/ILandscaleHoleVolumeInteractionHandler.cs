using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Framework.Landscapes
{
	public interface ILandscaleHoleVolumeInteractionHandler
	{
		bool landscapeHoleAutoIgnoreTerrainCollision { get; }

		void landscapeHoleBeginCollision(LandscapeHoleVolume volume, List<TerrainCollider> terrainColliders);

		void landscapeHoleEndCollision(LandscapeHoleVolume volume, List<TerrainCollider> terrainColliders);
	}
}
