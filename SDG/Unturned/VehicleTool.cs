using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class VehicleTool
	{
		public static bool giveVehicle(Player player, ushort id)
		{
			VehicleAsset vehicleAsset = (VehicleAsset)Assets.find(EAssetType.VEHICLE, id);
			if (vehicleAsset != null)
			{
				Vector3 vector = player.transform.position + player.transform.forward * 6f;
				RaycastHit raycastHit;
				Physics.Raycast(vector + Vector3.up * 16f, Vector3.down, ref raycastHit, 32f, RayMasks.BLOCK_VEHICLE);
				if (raycastHit.collider != null)
				{
					vector.y = raycastHit.point.y + 16f;
				}
				VehicleManager.spawnVehicle(id, vector, player.transform.rotation);
				return true;
			}
			return false;
		}
	}
}
