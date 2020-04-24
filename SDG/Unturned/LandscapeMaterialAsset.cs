using System;
using SDG.Framework.Debug;
using SDG.Framework.Devkit;
using SDG.Framework.Foliage;
using SDG.Framework.IO.FormattedFiles;
using UnityEngine;

namespace SDG.Unturned
{
	public class LandscapeMaterialAsset : Asset, IDevkitAssetSpawnable
	{
		public LandscapeMaterialAsset()
		{
		}

		public LandscapeMaterialAsset(Bundle bundle, Local localization, byte[] hash) : base(bundle, localization, hash)
		{
		}

		public override EAssetType assetCategory
		{
			get
			{
				return EAssetType.NONE;
			}
		}

		public void devkitAssetSpawn()
		{
		}

		protected override void readAsset(IFormattedFileReader reader)
		{
			base.readAsset(reader);
			this.texture = reader.readValue<ContentReference<Texture2D>>("Texture");
			this.mask = reader.readValue<ContentReference<Texture2D>>("Mask");
			this.physicsMaterial = reader.readValue<EPhysicsMaterial>("Physics_Material");
			this.foliage = reader.readValue<AssetReference<FoliageInfoCollectionAsset>>("Foliage");
			this.useAutoSlope = reader.readValue<bool>("Use_Auto_Slope");
			this.autoMinAngleBegin = reader.readValue<float>("Auto_Min_Angle_Begin");
			this.autoMinAngleEnd = reader.readValue<float>("Auto_Min_Angle_End");
			this.autoMaxAngleBegin = reader.readValue<float>("Auto_Max_Angle_Begin");
			this.autoMaxAngleEnd = reader.readValue<float>("Auto_Max_Angle_End");
			this.useAutoFoundation = reader.readValue<bool>("Use_Auto_Foundation");
			this.autoRayRadius = reader.readValue<float>("Auto_Ray_Radius");
			this.autoRayLength = reader.readValue<float>("Auto_Ray_Length");
			this.autoRayMask = reader.readValue<ERayMask>("Auto_Ray_Mask");
		}

		protected override void writeAsset(IFormattedFileWriter writer)
		{
			base.writeAsset(writer);
			writer.writeValue<ContentReference<Texture2D>>("Texture", this.texture);
			writer.writeValue<ContentReference<Texture2D>>("Mask", this.mask);
			writer.writeValue<EPhysicsMaterial>("Physics_Material", this.physicsMaterial);
			writer.writeValue<AssetReference<FoliageInfoCollectionAsset>>("Foliage", this.foliage);
			writer.writeValue<bool>("Use_Auto_Slope", this.useAutoSlope);
			writer.writeValue<float>("Auto_Min_Angle_Begin", this.autoMinAngleBegin);
			writer.writeValue<float>("Auto_Min_Angle_End", this.autoMinAngleEnd);
			writer.writeValue<float>("Auto_Max_Angle_Begin", this.autoMaxAngleBegin);
			writer.writeValue<float>("Auto_Max_Angle_End", this.autoMaxAngleEnd);
			writer.writeValue<bool>("Use_Auto_Foundation", this.useAutoFoundation);
			writer.writeValue<float>("Auto_Ray_Radius", this.autoRayRadius);
			writer.writeValue<float>("Auto_Ray_Length", this.autoRayLength);
			writer.writeValue<ERayMask>("Auto_Ray_Mask", this.autoRayMask);
		}

		[Inspectable("#SDG::Asset.Landscape.Material.Texture.Name", null)]
		public ContentReference<Texture2D> texture;

		[Inspectable("#SDG::Asset.Landscape.Material.Mask.Name", null)]
		public ContentReference<Texture2D> mask;

		[Inspectable("#SDG::Asset.Landscape.Material.Physics_Material.Name", null)]
		public EPhysicsMaterial physicsMaterial;

		[Inspectable("#SDG::Asset.Landscape.Foliage.Name", null)]
		public AssetReference<FoliageInfoCollectionAsset> foliage;

		[Inspectable("#SDG::Asset.Landscape.Material.Use_Auto_Slope", null)]
		public bool useAutoSlope;

		[Inspectable("#SDG::Asset.Landscape.Material.Auto_Min_Angle_Begin", null)]
		public float autoMinAngleBegin;

		[Inspectable("#SDG::Asset.Landscape.Material.Auto_Min_Angle_End", null)]
		public float autoMinAngleEnd;

		[Inspectable("#SDG::Asset.Landscape.Material.Auto_Max_Angle_Begin", null)]
		public float autoMaxAngleBegin;

		[Inspectable("#SDG::Asset.Landscape.Material.Auto_Max_Angle_End", null)]
		public float autoMaxAngleEnd;

		[Inspectable("#SDG::Asset.Landscape.Material.Use_Auto_Foundation", null)]
		public bool useAutoFoundation;

		[Inspectable("#SDG::Asset.Landscape.Material.Auto_Ray_Radius", null)]
		public float autoRayRadius;

		[Inspectable("#SDG::Asset.Landscape.Material.Auto_Ray_Length", null)]
		public float autoRayLength;

		[Inspectable("#SDG::Asset.Landscape.Material.Auto_Ray_Mask", null)]
		public ERayMask autoRayMask;
	}
}
