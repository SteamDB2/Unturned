using System;
using System.Collections.Generic;
using SDG.Framework.Debug;
using SDG.Framework.Devkit;
using SDG.Framework.IO.FormattedFiles;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.Foliage
{
	public class FoliageInfoCollectionAsset : Asset, IDevkitAssetSpawnable
	{
		public FoliageInfoCollectionAsset()
		{
			this.elements = new List<FoliageInfoCollectionAsset.FoliageInfoCollectionElement>();
		}

		public FoliageInfoCollectionAsset(Bundle bundle, Local localization, byte[] hash) : base(bundle, localization, hash)
		{
			this.elements = new List<FoliageInfoCollectionAsset.FoliageInfoCollectionElement>();
		}

		public void devkitAssetSpawn()
		{
		}

		public virtual void bakeFoliage(FoliageBakeSettings bakeSettings, IFoliageSurface surface, Bounds bounds, float weight)
		{
			foreach (FoliageInfoCollectionAsset.FoliageInfoCollectionElement foliageInfoCollectionElement in this.elements)
			{
				FoliageInfoAsset foliageInfoAsset = Assets.find<FoliageInfoAsset>(foliageInfoCollectionElement.asset);
				if (foliageInfoAsset != null)
				{
					foliageInfoAsset.bakeFoliage(bakeSettings, surface, bounds, weight, foliageInfoCollectionElement.weight);
				}
			}
		}

		protected override void readAsset(IFormattedFileReader reader)
		{
			base.readAsset(reader);
			int num = reader.readArrayLength("Foliage");
			for (int i = 0; i < num; i++)
			{
				this.elements.Add(reader.readValue<FoliageInfoCollectionAsset.FoliageInfoCollectionElement>(i));
			}
		}

		protected override void writeAsset(IFormattedFileWriter writer)
		{
			base.writeAsset(writer);
			writer.beginArray("Foliage");
			for (int i = 0; i < this.elements.Count; i++)
			{
				writer.writeValue<FoliageInfoCollectionAsset.FoliageInfoCollectionElement>(this.elements[i]);
			}
			writer.endArray();
		}

		[Inspectable("#SDG::Asset.Foliage.Collection.Elements.Name", null)]
		public List<FoliageInfoCollectionAsset.FoliageInfoCollectionElement> elements;

		public class FoliageInfoCollectionElement : IFormattedFileReadable, IFormattedFileWritable
		{
			public FoliageInfoCollectionElement()
			{
				this.weight = 1f;
			}

			public virtual void read(IFormattedFileReader reader)
			{
				reader = reader.readObject();
				if (reader == null)
				{
					return;
				}
				this.asset = reader.readValue<AssetReference<FoliageInfoAsset>>("Asset");
				this.weight = reader.readValue<float>("Weight");
			}

			public virtual void write(IFormattedFileWriter writer)
			{
				writer.beginObject();
				writer.writeValue<AssetReference<FoliageInfoAsset>>("Asset", this.asset);
				writer.writeValue<float>("Weight", this.weight);
				writer.endObject();
			}

			[Inspectable("#SDG::Asset.Foliage.Collection.Element.Asset.Name", null)]
			public AssetReference<FoliageInfoAsset> asset;

			[Inspectable("#SDG::Asset.Foliage.Collection.Element.Weight.Name", null)]
			public float weight;
		}
	}
}
