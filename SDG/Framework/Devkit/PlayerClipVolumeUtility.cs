using System;
using UnityEngine;

namespace SDG.Framework.Devkit
{
	public class PlayerClipVolumeUtility
	{
		public static bool isPointInsideVolume(Vector3 point)
		{
			PlayerClipVolume playerClipVolume;
			return PlayerClipVolumeUtility.isPointInsideVolume(point, out playerClipVolume);
		}

		public static bool isPointInsideVolume(Vector3 point, out PlayerClipVolume volume)
		{
			for (int i = 0; i < PlayerClipVolumeSystem.volumes.Count; i++)
			{
				volume = (PlayerClipVolumeSystem.volumes[i] as PlayerClipVolume);
				if (!(volume == null))
				{
					if (PlayerClipVolumeUtility.isPointInsideVolume(volume, point))
					{
						return true;
					}
				}
			}
			volume = null;
			return false;
		}

		public static bool isPointInsideVolume(PlayerClipVolume volume, Vector3 point)
		{
			Vector3 vector = volume.transform.InverseTransformPoint(point);
			return Mathf.Abs(vector.x) < 0.5f && Mathf.Abs(vector.y) < 0.5f && Mathf.Abs(vector.z) < 0.5f;
		}
	}
}
