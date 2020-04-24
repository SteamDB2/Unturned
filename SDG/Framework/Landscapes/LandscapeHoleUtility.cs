using System;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.Landscapes
{
	public class LandscapeHoleUtility
	{
		public static bool isPointInsideHoleVolume(Vector3 point)
		{
			LandscapeHoleVolume landscapeHoleVolume;
			return LandscapeHoleUtility.isPointInsideHoleVolume(point, out landscapeHoleVolume);
		}

		public static bool isPointInsideHoleVolume(Vector3 point, out LandscapeHoleVolume volume)
		{
			for (int i = 0; i < LandscapeHoleSystem.volumes.Count; i++)
			{
				volume = LandscapeHoleSystem.volumes[i];
				if (LandscapeHoleUtility.isPointInsideHoleVolume(volume, point))
				{
					return true;
				}
			}
			volume = null;
			return false;
		}

		public static bool isPointInsideHoleVolume(LandscapeHoleVolume volume, Vector3 point)
		{
			Vector3 vector = volume.transform.InverseTransformPoint(point);
			return Mathf.Abs(vector.x) < 0.5f && Mathf.Abs(vector.y) < 0.5f && Mathf.Abs(vector.z) < 0.5f;
		}

		public static bool doesRayIntersectHoleVolume(Ray ray, out RaycastHit hit, out LandscapeHoleVolume volume, float maxDistance)
		{
			for (int i = 0; i < LandscapeHoleSystem.volumes.Count; i++)
			{
				volume = LandscapeHoleSystem.volumes[i];
				if (LandscapeHoleUtility.doesRayIntersectHoleVolume(volume, ray, out hit, maxDistance))
				{
					return true;
				}
			}
			hit = default(RaycastHit);
			volume = null;
			return false;
		}

		public static bool doesRayIntersectHoleVolume(LandscapeHoleVolume volume, Ray ray, out RaycastHit hit, float maxDistance)
		{
			return volume.box.Raycast(ray, ref hit, maxDistance);
		}

		public static bool shouldRaycastIgnoreLandscape(Ray ray, float maxDistance)
		{
			RaycastHit raycastHit;
			LandscapeHoleVolume volume;
			RaycastHit raycastHit2;
			RaycastHit raycastHit3;
			return (LandscapeHoleUtility.doesRayIntersectHoleVolume(ray, out raycastHit, out volume, maxDistance) && Physics.Raycast(ray, ref raycastHit2, maxDistance, RayMasks.GROUND) && LandscapeHoleUtility.isPointInsideHoleVolume(volume, raycastHit2.point)) || (LandscapeHoleUtility.isPointInsideHoleVolume(ray.origin, out volume) && Physics.Raycast(ray, ref raycastHit3, maxDistance, RayMasks.GROUND) && LandscapeHoleUtility.isPointInsideHoleVolume(volume, raycastHit3.point));
		}

		public static void raycastIgnoreLandscapeIfNecessary(Ray ray, float maxDistance, ref int layerMask)
		{
			if (LandscapeHoleUtility.shouldRaycastIgnoreLandscape(ray, maxDistance))
			{
				layerMask &= ~RayMasks.GROUND;
			}
		}
	}
}
