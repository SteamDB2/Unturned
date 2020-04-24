using System;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.Water
{
	public class WaterUtility
	{
		public static bool isPointInsideVolume(Vector3 point)
		{
			WaterVolume waterVolume;
			return WaterUtility.isPointInsideVolume(point, out waterVolume);
		}

		public static bool isPointInsideVolume(Vector3 point, out WaterVolume volume)
		{
			for (int i = 0; i < WaterSystem.volumes.Count; i++)
			{
				volume = WaterSystem.volumes[i];
				if (WaterUtility.isPointInsideVolume(volume, point))
				{
					return true;
				}
			}
			volume = null;
			return false;
		}

		public static float getWaterSurfaceElevation(WaterVolume volume, Vector3 point)
		{
			point.y += 1024f;
			Ray ray;
			ray..ctor(point, new Vector3(0f, -1f, 0f));
			RaycastHit raycastHit;
			if (volume.box.Raycast(ray, ref raycastHit, 2048f))
			{
				return raycastHit.point.y;
			}
			return 0f;
		}

		public static bool isPointInsideVolume(WaterVolume volume, Vector3 point)
		{
			Vector3 vector = volume.transform.InverseTransformPoint(point);
			return Mathf.Abs(vector.x) < 0.5f && Mathf.Abs(vector.y) < 0.5f && Mathf.Abs(vector.z) < 0.5f;
		}

		public static bool isPointUnderwater(Vector3 point)
		{
			return (Level.info != null && Level.info.configData.Use_Legacy_Water && LevelLighting.isPositionUnderwater(point)) || WaterUtility.isPointInsideVolume(point);
		}

		public static bool isPointUnderwater(Vector3 point, out WaterVolume volume)
		{
			if (Level.info != null && Level.info.configData.Use_Legacy_Water && LevelLighting.isPositionUnderwater(point))
			{
				volume = null;
				return true;
			}
			return WaterUtility.isPointInsideVolume(point, out volume);
		}

		public static float getWaterSurfaceElevation(Vector3 point)
		{
			bool flag = false;
			float num = -1024f;
			foreach (WaterVolume waterVolume in WaterSystem.volumes)
			{
				if (WaterUtility.isPointInsideVolume(waterVolume, point))
				{
					return WaterUtility.getWaterSurfaceElevation(waterVolume, point);
				}
				Ray ray;
				ray..ctor(point, new Vector3(0f, -1f, 0f));
				RaycastHit raycastHit;
				if (waterVolume.box.Raycast(ray, ref raycastHit, 2048f) && raycastHit.point.y > num)
				{
					num = raycastHit.point.y;
					flag = true;
				}
			}
			if (flag)
			{
				return num;
			}
			if (Level.info != null && Level.info.configData.Use_Legacy_Water)
			{
				return LevelLighting.getWaterSurfaceElevation();
			}
			return -1024f;
		}

		public static void getUnderwaterInfo(Vector3 point, out bool isUnderwater, out float surfaceElevation)
		{
			if (Level.info != null && Level.info.configData.Use_Legacy_Water)
			{
				isUnderwater = LevelLighting.isPositionUnderwater(point);
				surfaceElevation = LevelLighting.getWaterSurfaceElevation();
			}
			else
			{
				isUnderwater = false;
				surfaceElevation = -1024f;
			}
			WaterVolume volume;
			if (!isUnderwater && WaterUtility.isPointInsideVolume(point, out volume))
			{
				isUnderwater = true;
				surfaceElevation = WaterUtility.getWaterSurfaceElevation(volume, point);
			}
		}
	}
}
