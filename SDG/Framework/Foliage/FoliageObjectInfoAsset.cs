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
	public class FoliageObjectInfoAsset : FoliageInfoAsset, IDevkitAssetSpawnable
	{
		public FoliageObjectInfoAsset()
		{
			this.resetObject();
		}

		public FoliageObjectInfoAsset(Bundle bundle, Local localization, byte[] hash) : base(bundle, localization, hash)
		{
			this.resetObject();
		}

		public void devkitAssetSpawn()
		{
		}

		public override void bakeFoliage(FoliageBakeSettings bakeSettings, IFoliageSurface surface, Bounds bounds, float surfaceWeight, float collectionWeight)
		{
			if (!bakeSettings.bakeObjects)
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
					List<LevelObject> list = LevelObjects.objects[(int)b, (int)b2];
					foreach (LevelObject levelObject in list)
					{
						if (this.obj.isReferenceTo(levelObject.asset))
						{
							if (volume.containsPoint(levelObject.transform.position))
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
			ObjectAsset objectAsset = Assets.find<ObjectAsset>(this.obj);
			if (objectAsset == null)
			{
				return;
			}
			LevelObjects.addDevkitObject(objectAsset.GUID, position, rotation, scale, (!clearWhenBaked) ? ELevelObjectPlacementOrigin.PAINTED : ELevelObjectPlacementOrigin.GENERATED);
		}

		protected override bool isPositionValid(Vector3 position)
		{
			return FoliageVolumeUtility.isPointValid(position, false, false, true);
		}

		protected override void readAsset(IFormattedFileReader reader)
		{
			base.readAsset(reader);
			this.obj = reader.readValue<AssetReference<ObjectAsset>>("Object");
			if (reader.containsKey("Obstruction_Radius"))
			{
				this.obstructionRadius = reader.readValue<float>("Obstruction_Radius");
			}
		}

		protected override void writeAsset(IFormattedFileWriter writer)
		{
			base.writeAsset(writer);
			writer.writeValue<AssetReference<ObjectAsset>>("Object", this.obj);
			writer.writeValue<float>("Obstruction_Radius", this.obstructionRadius);
		}

		protected virtual void resetObject()
		{
			this.obstructionRadius = 4f;
		}

		[Inspectable("#SDG::Asset.Foliage.Info.Object.Name", null)]
		public AssetReference<ObjectAsset> obj;

		[Inspectable("#SDG::Asset.Foliage.Info.Obstruction_Radius.Name", null)]
		public float obstructionRadius;
	}
}
