using System;
using SDG.Framework.Landscapes;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.Utilities
{
	public class PhysicsUtility
	{
		public static bool raycast(Ray ray, out RaycastHit hit, float maxDistance, int layerMask, QueryTriggerInteraction queryTriggerInteraction = 0)
		{
			if ((layerMask & RayMasks.GROUND) == RayMasks.GROUND)
			{
				LandscapeHoleUtility.raycastIgnoreLandscapeIfNecessary(ray, maxDistance, ref layerMask);
			}
			return Physics.Raycast(ray, ref hit, maxDistance, layerMask, queryTriggerInteraction);
		}
	}
}
