using System;
using SDG.Framework.Landscapes;
using UnityEngine;

namespace SDG.Unturned
{
	public class PhysicsTool
	{
		public static EPhysicsMaterial checkMaterial(Vector3 point)
		{
			if (LevelGround.terrain == null)
			{
				AssetReference<LandscapeMaterialAsset> reference;
				if (Landscape.getSplatmapMaterial(point, out reference))
				{
					LandscapeMaterialAsset landscapeMaterialAsset = Assets.find<LandscapeMaterialAsset>(reference);
					if (landscapeMaterialAsset != null)
					{
						return landscapeMaterialAsset.physicsMaterial;
					}
				}
				return EPhysicsMaterial.NONE;
			}
			GroundMaterial material = LevelGround.getMaterial(point);
			if (material.isGrassy_0 || material.isGrassy_1 || material.isFlowery_0 || material.isFlowery_1)
			{
				return EPhysicsMaterial.FOLIAGE_STATIC;
			}
			if (material.isRocky)
			{
				return EPhysicsMaterial.GRAVEL_STATIC;
			}
			if (material.isRoad)
			{
				return EPhysicsMaterial.CONCRETE_STATIC;
			}
			if (material.isSnowy)
			{
				return EPhysicsMaterial.SNOW_STATIC;
			}
			return EPhysicsMaterial.CONCRETE_STATIC;
		}

		public static bool isMaterialDynamic(EPhysicsMaterial material)
		{
			switch (material)
			{
			case EPhysicsMaterial.CLOTH_DYNAMIC:
				return true;
			case EPhysicsMaterial.TILE_DYNAMIC:
				return true;
			case EPhysicsMaterial.CONCRETE_DYNAMIC:
				return true;
			case EPhysicsMaterial.FLESH_DYNAMIC:
				return true;
			case EPhysicsMaterial.GRAVEL_DYNAMIC:
				return true;
			case EPhysicsMaterial.METAL_DYNAMIC:
				return true;
			case EPhysicsMaterial.WOOD_DYNAMIC:
				return true;
			}
			return false;
		}

		public static EPhysicsMaterial checkMaterial(Collider collider)
		{
			if (collider.sharedMaterial == null)
			{
				return EPhysicsMaterial.NONE;
			}
			string text = collider.sharedMaterial.name.ToString();
			switch (text)
			{
			case "Cloth":
				return EPhysicsMaterial.CLOTH_STATIC;
			case "Cloth_Dynamic":
				return EPhysicsMaterial.CLOTH_DYNAMIC;
			case "Cloth_Static":
				return EPhysicsMaterial.CLOTH_STATIC;
			case "Tile":
				return EPhysicsMaterial.TILE_STATIC;
			case "Tile_Dynamic":
				return EPhysicsMaterial.TILE_DYNAMIC;
			case "Tile_Static":
				return EPhysicsMaterial.TILE_STATIC;
			case "Concrete":
				return EPhysicsMaterial.CONCRETE_STATIC;
			case "Concrete_Dynamic":
				return EPhysicsMaterial.CONCRETE_DYNAMIC;
			case "Concrete_Static":
				return EPhysicsMaterial.CONCRETE_STATIC;
			case "Flesh":
				return EPhysicsMaterial.FLESH_DYNAMIC;
			case "Flesh_Dynamic":
				return EPhysicsMaterial.FLESH_DYNAMIC;
			case "Flesh_Static":
				return EPhysicsMaterial.FLESH_DYNAMIC;
			case "Gravel":
				return EPhysicsMaterial.GRAVEL_STATIC;
			case "Gravel_Dynamic":
				return EPhysicsMaterial.GRAVEL_DYNAMIC;
			case "Gravel_Static":
				return EPhysicsMaterial.GRAVEL_STATIC;
			case "Metal":
				return EPhysicsMaterial.METAL_STATIC;
			case "Metal_Dynamic":
				return EPhysicsMaterial.METAL_DYNAMIC;
			case "Metal_Static":
				return EPhysicsMaterial.METAL_STATIC;
			case "Metal_Slip":
				return EPhysicsMaterial.METAL_SLIP;
			case "Wood":
				return EPhysicsMaterial.WOOD_STATIC;
			case "Wood_Dynamic":
				return EPhysicsMaterial.WOOD_DYNAMIC;
			case "Wood_Static":
				return EPhysicsMaterial.WOOD_STATIC;
			case "Foliage":
				return EPhysicsMaterial.FOLIAGE_STATIC;
			case "Foliage_Dynamic":
				return EPhysicsMaterial.FOLIAGE_DYNAMIC;
			case "Foliage_Static":
				return EPhysicsMaterial.FOLIAGE_STATIC;
			case "Water":
				return EPhysicsMaterial.WATER_STATIC;
			case "Water_Dynamic":
				return EPhysicsMaterial.WATER_STATIC;
			case "Water_Static":
				return EPhysicsMaterial.WATER_STATIC;
			case "Snow":
				return EPhysicsMaterial.SNOW_STATIC;
			case "Snow_Dynamic":
				return EPhysicsMaterial.SNOW_STATIC;
			case "Snow_Static":
				return EPhysicsMaterial.SNOW_STATIC;
			case "Ice":
				return EPhysicsMaterial.ICE_STATIC;
			case "Ice_Dynamic":
				return EPhysicsMaterial.ICE_STATIC;
			case "Ice_Static":
				return EPhysicsMaterial.ICE_STATIC;
			}
			return EPhysicsMaterial.NONE;
		}
	}
}
