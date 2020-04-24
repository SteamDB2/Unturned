using System;
using SDG.Framework.Debug;
using SDG.Framework.Devkit;
using SDG.Framework.IO.FormattedFiles;

namespace SDG.Unturned
{
	public class ZombieDifficultyAsset : Asset, IDevkitAssetSpawnable
	{
		public ZombieDifficultyAsset()
		{
			this.construct();
		}

		public ZombieDifficultyAsset(Bundle bundle, Local localization, byte[] hash) : base(bundle, localization, hash)
		{
			this.construct();
		}

		public void devkitAssetSpawn()
		{
		}

		protected override void readAsset(IFormattedFileReader reader)
		{
			base.readAsset(reader);
			this.Crawler_Chance = reader.readValue<float>("Crawler_Chance");
			this.Sprinter_Chance = reader.readValue<float>("Sprinter_Chance");
			this.Flanker_Chance = reader.readValue<float>("Flanker_Chance");
			this.Burner_Chance = reader.readValue<float>("Burner_Chance");
			this.Acid_Chance = reader.readValue<float>("Acid_Chance");
			this.Boss_Electric_Chance = reader.readValue<float>("Boss_Electric_Chance");
			this.Boss_Wind_Chance = reader.readValue<float>("Boss_Wind_Chance");
			this.Boss_Fire_Chance = reader.readValue<float>("Boss_Fire_Chance");
		}

		protected override void writeAsset(IFormattedFileWriter writer)
		{
			base.writeAsset(writer);
			writer.writeValue<float>("Crawler_Chance", this.Crawler_Chance);
			writer.writeValue<float>("Sprinter_Chance", this.Sprinter_Chance);
			writer.writeValue<float>("Flanker_Chance", this.Flanker_Chance);
			writer.writeValue<float>("Burner_Chance", this.Burner_Chance);
			writer.writeValue<float>("Acid_Chance", this.Acid_Chance);
			writer.writeValue<float>("Boss_Electric_Chance", this.Boss_Electric_Chance);
			writer.writeValue<float>("Boss_Wind_Chance", this.Boss_Wind_Chance);
			writer.writeValue<float>("Boss_Fire_Chance", this.Boss_Fire_Chance);
		}

		protected virtual void construct()
		{
		}

		[Inspectable("#SDG::Asset.Zombie_Difficulty.Crawler_Chance.Name", null)]
		public float Crawler_Chance;

		[Inspectable("#SDG::Asset.Zombie_Difficulty.Sprinter_Chance.Name", null)]
		public float Sprinter_Chance;

		[Inspectable("#SDG::Asset.Zombie_Difficulty.Flanker_Chance.Name", null)]
		public float Flanker_Chance;

		[Inspectable("#SDG::Asset.Zombie_Difficulty.Burner_Chance.Name", null)]
		public float Burner_Chance;

		[Inspectable("#SDG::Asset.Zombie_Difficulty.Acid_Chance.Name", null)]
		public float Acid_Chance;

		[Inspectable("#SDG::Asset.Zombie_Difficulty.Boss_Electric_Chance.Name", null)]
		public float Boss_Electric_Chance;

		[Inspectable("#SDG::Asset.Zombie_Difficulty.Boss_Wind_Chance.Name", null)]
		public float Boss_Wind_Chance;

		[Inspectable("#SDG::Asset.Zombie_Difficulty.Boss_Fire_Chance.Name", null)]
		public float Boss_Fire_Chance;
	}
}
