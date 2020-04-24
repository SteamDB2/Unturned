using System;
using SDG.Framework.Debug;
using SDG.Framework.Devkit;
using SDG.Framework.IO.FormattedFiles;
using UnityEngine;

namespace SDG.Unturned
{
	public class MaterialPaletteAsset : Asset, IDevkitAssetSpawnable
	{
		public MaterialPaletteAsset()
		{
			this.materials = new InspectableList<ContentReference<Material>>();
		}

		public MaterialPaletteAsset(Bundle bundle, Local localization, byte[] hash) : base(bundle, localization, hash)
		{
			this.materials = new InspectableList<ContentReference<Material>>();
		}

		public void devkitAssetSpawn()
		{
		}

		protected override void readAsset(IFormattedFileReader reader)
		{
			base.readAsset(reader);
			int num = reader.readArrayLength("Materials");
			for (int i = 0; i < num; i++)
			{
				this.materials.Add(reader.readValue<ContentReference<Material>>(i));
			}
		}

		protected override void writeAsset(IFormattedFileWriter writer)
		{
			base.writeAsset(writer);
			writer.beginArray("Materials");
			for (int i = 0; i < this.materials.Count; i++)
			{
				writer.writeValue<ContentReference<Material>>(this.materials[i]);
			}
			writer.endArray();
		}

		[Inspectable("#SDG::Asset.Material_Palette.Materials.Name", null)]
		public InspectableList<ContentReference<Material>> materials;
	}
}
