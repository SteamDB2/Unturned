using System;
using UnityEngine;

namespace SDG.Framework.Devkit
{
	public class AmbianceUtility
	{
		public static bool isPointInsideVolume(Vector3 point, out AmbianceVolume volume)
		{
			for (int i = 0; i < AmbianceSystem.volumes.Count; i++)
			{
				volume = AmbianceSystem.volumes[i];
				if (AmbianceUtility.isPointInsideVolume(volume, point))
				{
					return true;
				}
			}
			volume = null;
			return false;
		}

		public static bool isPointInsideVolume(AmbianceVolume volume, Vector3 point)
		{
			Vector3 vector = volume.transform.InverseTransformPoint(point);
			return Mathf.Abs(vector.x) < 0.5f && Mathf.Abs(vector.y) < 0.5f && Mathf.Abs(vector.z) < 0.5f;
		}
	}
}
