using System;
using UnityEngine;

namespace SDG.Framework.Foliage
{
	public class FoliageVolumeUtility
	{
		public static bool isTileBakeable(FoliageTile tile)
		{
			if (FoliageVolumeSystem.additiveVolumes.Count > 0)
			{
				Vector3 center = tile.worldBounds.center;
				for (int i = 0; i < FoliageVolumeSystem.additiveVolumes.Count; i++)
				{
					FoliageVolume volume = FoliageVolumeSystem.additiveVolumes[i];
					if (FoliageVolumeUtility.isPointInsideVolume(volume, center))
					{
						return true;
					}
				}
				return false;
			}
			return true;
		}

		public static bool isPointValid(Vector3 point, bool instancedMeshes, bool resources, bool objects)
		{
			bool flag = false;
			if (FoliageVolumeSystem.additiveVolumes.Count > 0)
			{
				for (int i = 0; i < FoliageVolumeSystem.additiveVolumes.Count; i++)
				{
					FoliageVolume foliageVolume = FoliageVolumeSystem.additiveVolumes[i];
					if (!instancedMeshes || foliageVolume.instancedMeshes)
					{
						if (!resources || foliageVolume.resources)
						{
							if (!objects || foliageVolume.objects)
							{
								if (FoliageVolumeUtility.isPointInsideVolume(foliageVolume, point))
								{
									flag = true;
								}
							}
						}
					}
				}
			}
			else
			{
				flag = true;
			}
			if (!flag)
			{
				return false;
			}
			for (int j = 0; j < FoliageVolumeSystem.subtractiveVolumes.Count; j++)
			{
				FoliageVolume foliageVolume2 = FoliageVolumeSystem.subtractiveVolumes[j];
				if (!instancedMeshes || foliageVolume2.instancedMeshes)
				{
					if (!resources || foliageVolume2.resources)
					{
						if (!objects || foliageVolume2.objects)
						{
							if (FoliageVolumeUtility.isPointInsideVolume(foliageVolume2, point))
							{
								return false;
							}
						}
					}
				}
			}
			return true;
		}

		public static bool isPointInsideVolume(FoliageVolume volume, Vector3 point)
		{
			Vector3 vector = volume.transform.InverseTransformPoint(point);
			return Mathf.Abs(vector.x) < 0.5f && Mathf.Abs(vector.y) < 0.5f && Mathf.Abs(vector.z) < 0.5f;
		}
	}
}
