using System;
using System.Collections.Generic;
using SDG.Framework.Debug;
using SDG.Framework.Devkit;
using SDG.Framework.IO.FormattedFiles;
using SDG.Framework.Utilities;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.Foliage
{
	public class FoliageResourceInfoAsset : FoliageInfoAsset, IDevkitAssetSpawnable
	{
		public FoliageResourceInfoAsset()
		{
			this.resetResource();
		}

		public FoliageResourceInfoAsset(Bundle bundle, Local localization, byte[] hash) : base(bundle, localization, hash)
		{
			this.resetResource();
		}

		public void devkitAssetSpawn()
		{
		}

		public override void bakeFoliage(FoliageBakeSettings bakeSettings, IFoliageSurface surface, Bounds bounds, float surfaceWeight, float collectionWeight)
		{
			if (!bakeSettings.bakeResources)
			{
				return;
			}
			if (bakeSettings.bakeClear)
			{
				return;
			}
			base.bakeFoliage(bakeSettings, surface, bounds, surfaceWeight, collectionWeight);
		}

		public override int getInstanceCountInVolume(IShapeVolume volume)
		{
			Bounds worldBounds = volume.worldBounds;
			RegionBounds regionBounds = new RegionBounds(worldBounds);
			int num = 0;
			for (byte b = regionBounds.min.x; b <= regionBounds.max.x; b += 1)
			{
				for (byte b2 = regionBounds.min.y; b2 <= regionBounds.max.y; b2 += 1)
				{
					List<ResourceSpawnpoint> list = LevelGround.trees[(int)b, (int)b2];
					foreach (ResourceSpawnpoint resourceSpawnpoint in list)
					{
						if (this.resource.isReferenceTo(resourceSpawnpoint.asset))
						{
							if (volume.containsPoint(resourceSpawnpoint.point))
							{
								num++;
							}
						}
					}
				}
			}
			return num;
		}

		protected override void addFoliage(Vector3 position, Quaternion rotation, Vector3 scale, bool clearWhenBaked)
		{
			ResourceAsset resourceAsset = Assets.find<ResourceAsset>(this.resource);
			if (resourceAsset == null)
			{
				return;
			}
			byte b = 0;
			while ((int)b < LevelGround.resources.Length)
			{
				GroundResource groundResource = LevelGround.resources[(int)b];
				if (groundResource.id == resourceAsset.id)
				{
					break;
				}
				b += 1;
			}
			LevelGround.addSpawn(position, b, clearWhenBaked);
		}

		protected override bool isPositionValid(Vector3 position)
		{
			if (!FoliageVolumeUtility.isPointValid(position, false, true, false))
			{
				return false;
			}
			int num = Physics.OverlapSphereNonAlloc(position, this.obstructionRadius, FoliageResourceInfoAsset.OBSTRUCTION_COLLIDERS, RayMasks.BLOCK_RESOURCE);
			for (int i = 0; i < num; i++)
			{
				ObjectAsset asset = LevelObjects.getAsset(FoliageResourceInfoAsset.OBSTRUCTION_COLLIDERS[i].transform);
				if (asset != null && !asset.isSnowshoe)
				{
					return false;
				}
			}
			return true;
		}

		protected override void readAsset(IFormattedFileReader reader)
		{
			base.readAsset(reader);
			this.resource = reader.readValue<AssetReference<ResourceAsset>>("Resource");
			if (reader.containsKey("Obstruction_Radius"))
			{
				this.obstructionRadius = reader.readValue<float>("Obstruction_Radius");
			}
		}

		protected override void writeAsset(IFormattedFileWriter writer)
		{
			base.writeAsset(writer);
			writer.writeValue<AssetReference<ResourceAsset>>("Resource", this.resource);
			writer.writeValue<float>("Obstruction_Radius", this.obstructionRadius);
		}

		protected virtual void resetResource()
		{
			this.obstructionRadius = 4f;
		}

		private static readonly Collider[] OBSTRUCTION_COLLIDERS = new Collider[16];

		[Inspectable("#SDG::Asset.Foliage.Info.Resource.Name", null)]
		public AssetReference<ResourceAsset> resource;

		[Inspectable("#SDG::Asset.Foliage.Info.Obstruction_Radius.Name", null)]
		public float obstructionRadius;
	}
}
