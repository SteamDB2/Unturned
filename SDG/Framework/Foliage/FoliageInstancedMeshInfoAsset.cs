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
	public class FoliageInstancedMeshInfoAsset : FoliageInfoAsset, IDevkitAssetSpawnable
	{
		public FoliageInstancedMeshInfoAsset()
		{
			this.resetInstancedMeshInfo();
		}

		public FoliageInstancedMeshInfoAsset(Bundle bundle, Local localization, byte[] hash) : base(bundle, localization, hash)
		{
			this.resetInstancedMeshInfo();
		}

		public void devkitAssetSpawn()
		{
		}

		public override void bakeFoliage(FoliageBakeSettings bakeSettings, IFoliageSurface surface, Bounds bounds, float surfaceWeight, float collectionWeight)
		{
			if (!bakeSettings.bakeInstancesMeshes)
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
			FoliageBounds foliageBounds = new FoliageBounds(worldBounds);
			int num = 0;
			for (int i = foliageBounds.min.x; i <= foliageBounds.max.x; i++)
			{
				for (int j = foliageBounds.min.y; j <= foliageBounds.max.y; j++)
				{
					FoliageCoord tileCoord = new FoliageCoord(i, j);
					FoliageTile tile = FoliageSystem.getTile(tileCoord);
					if (tile != null)
					{
						if (!tile.hasInstances)
						{
							tile.readInstancesOnThread();
						}
						FoliageInstanceList foliageInstanceList;
						if (tile.instances != null && tile.instances.TryGetValue(base.getReferenceTo<FoliageInstancedMeshInfoAsset>(), out foliageInstanceList))
						{
							foreach (List<Matrix4x4> list in foliageInstanceList.matrices)
							{
								foreach (Matrix4x4 matrix in list)
								{
									if (volume.containsPoint(matrix.GetPosition()))
									{
										num++;
									}
								}
							}
						}
					}
				}
			}
			return num;
		}

		protected override void addFoliage(Vector3 position, Quaternion rotation, Vector3 scale, bool clearWhenBaked)
		{
			FoliageSystem.addInstance(base.getReferenceTo<FoliageInstancedMeshInfoAsset>(), position, rotation, scale, clearWhenBaked);
		}

		protected override bool isPositionValid(Vector3 position)
		{
			return FoliageVolumeUtility.isPointValid(position, true, false, false);
		}

		protected override void readAsset(IFormattedFileReader reader)
		{
			base.readAsset(reader);
			this.mesh = reader.readValue<ContentReference<Mesh>>("Mesh");
			this.material = reader.readValue<ContentReference<Material>>("Material");
			if (reader.containsKey("Cast_Shadows"))
			{
				this.castShadows = reader.readValue<bool>("Cast_Shadows");
			}
			else
			{
				this.castShadows = false;
			}
			if (reader.containsKey("Tile_Dither"))
			{
				this.tileDither = reader.readValue<bool>("Tile_Dither");
			}
			else
			{
				this.tileDither = true;
			}
			if (reader.containsKey("Draw_Distance"))
			{
				this.drawDistance = reader.readValue<int>("Draw_Distance");
			}
			else
			{
				this.drawDistance = -1;
			}
		}

		protected override void writeAsset(IFormattedFileWriter writer)
		{
			base.writeAsset(writer);
			writer.writeValue<ContentReference<Mesh>>("Mesh", this.mesh);
			writer.writeValue<ContentReference<Material>>("Material", this.material);
			writer.writeValue<bool>("Cast_Shadows", this.castShadows);
			writer.writeValue<bool>("Tile_Dither", this.tileDither);
			writer.writeValue<int>("Draw_Distance", this.drawDistance);
		}

		protected virtual void resetInstancedMeshInfo()
		{
			this.tileDither = true;
			this.drawDistance = -1;
		}

		[Inspectable("#SDG::Asset.Foliage.Info.Mesh.Name", null)]
		public ContentReference<Mesh> mesh;

		[Inspectable("#SDG::Asset.Foliage.Info.Material.Name", null)]
		public ContentReference<Material> material;

		[Inspectable("#SDG::Asset.Foliage.Info.Cast_Shadows.Name", null)]
		public bool castShadows;

		[Inspectable("#SDG::Asset.Foliage.Info.Tile_Dither.Name", null)]
		public bool tileDither;

		[Inspectable("#SDG::Asset.Foliage.Info.Draw_Distance.Name", null)]
		public int drawDistance;
	}
}
