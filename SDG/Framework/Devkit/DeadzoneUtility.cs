using System;
using UnityEngine;

namespace SDG.Framework.Devkit
{
	public class DeadzoneUtility
	{
		public static bool isPointInsideVolume(Vector3 point, out DeadzoneVolume volume)
		{
			for (int i = 0; i < DeadzoneSystem.volumes.Count; i++)
			{
				volume = DeadzoneSystem.volumes[i];
				if (DeadzoneUtility.isPointInsideVolume(volume, point))
				{
					return true;
				}
			}
			volume = null;
			return false;
		}

		public static bool isPointInsideVolume(DeadzoneVolume volume, Vector3 point)
		{
			Vector3 vector = volume.transform.InverseTransformPoint(point);
			return Mathf.Abs(vector.x) < 0.5f && Mathf.Abs(vector.y) < 0.5f && Mathf.Abs(vector.z) < 0.5f;
		}
	}
}
