using System;
using SDG.Framework.Debug;
using SDG.Framework.IO.FormattedFiles;

namespace SDG.Unturned
{
	public class LevelAsset : Asset
	{
		public LevelAsset()
		{
			this.supportedGameModes = new InspectableList<TypeReference<GameMode>>();
		}

		public LevelAsset(Bundle bundle, Local localization, byte[] hash) : base(bundle, localization, hash)
		{
			this.supportedGameModes = new InspectableList<TypeReference<GameMode>>();
			bundle.unload();
		}

		public LevelAsset(Bundle bundle, Data data, Local localization, ushort id) : base(bundle, data, localization, id)
		{
			this.supportedGameModes = new InspectableList<TypeReference<GameMode>>();
			bundle.unload();
		}

		protected override void readAsset(IFormattedFileReader reader)
		{
			base.readAsset(reader);
			this.defaultGameMode = reader.readValue<TypeReference<GameMode>>("Default_Game_Mode");
			int num = reader.readArrayLength("Supported_Game_Modes");
			for (int i = 0; i < num; i++)
			{
				this.supportedGameModes.Add(reader.readValue<TypeReference<GameMode>>(i));
			}
		}

		protected override void writeAsset(IFormattedFileWriter writer)
		{
			base.writeAsset(writer);
			writer.writeValue<TypeReference<GameMode>>("Default_Game_Mode", this.defaultGameMode);
			writer.beginArray("Supported_Game_Modes");
			foreach (TypeReference<GameMode> value in this.supportedGameModes)
			{
				writer.writeValue<TypeReference<GameMode>>(value);
			}
			writer.endArray();
		}

		[Inspectable("#SDG::Asset.Level.Default_Game_Mode.Name", null)]
		public TypeReference<GameMode> defaultGameMode;

		[Inspectable("#SDG::Asset.Level.Supported_Game_Modes.Name", null)]
		public InspectableList<TypeReference<GameMode>> supportedGameModes;
	}
}
