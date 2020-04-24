using System;
using System.IO;
using SDG.Framework.Debug;
using SDG.Framework.IO;
using SDG.Framework.IO.FormattedFiles;
using SDG.Framework.IO.FormattedFiles.KeyValueTables;
using SDG.Unturned;

namespace SDG.Framework.Devkit.Tools
{
	public class DevkitFoliageToolOptions : IFormattedFileReadable, IFormattedFileWritable
	{
		static DevkitFoliageToolOptions()
		{
			DevkitFoliageToolOptions._instance = new DevkitFoliageToolOptions();
			DevkitFoliageToolOptions.load();
		}

		public static DevkitFoliageToolOptions instance
		{
			get
			{
				return DevkitFoliageToolOptions._instance;
			}
		}

		[TerminalCommandProperty("foliage.tool.add_sensitivity", "multiplier for paint brush delta", 1)]
		public static float addSensitivity
		{
			get
			{
				return DevkitFoliageToolOptions._addSensitivity;
			}
			set
			{
				DevkitFoliageToolOptions._addSensitivity = value;
				TerminalUtility.printCommandPass("Set add_sensitivity to: " + DevkitFoliageToolOptions.addSensitivity);
			}
		}

		[TerminalCommandProperty("foliage.tool.remove_sensitivity", "multiplier for paint brush delta", 1)]
		public static float removeSensitivity
		{
			get
			{
				return DevkitFoliageToolOptions._removeSensitivity;
			}
			set
			{
				DevkitFoliageToolOptions._removeSensitivity = value;
				TerminalUtility.printCommandPass("Set remove_sensitivity to: " + DevkitFoliageToolOptions.removeSensitivity);
			}
		}

		public static void load()
		{
			DevkitFoliageToolOptions.wasLoaded = true;
			string path = IOUtility.rootPath + "/Cloud/Foliage.tool";
			string directoryName = Path.GetDirectoryName(path);
			if (!Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			if (!File.Exists(path))
			{
				return;
			}
			using (StreamReader streamReader = new StreamReader(path))
			{
				IFormattedFileReader reader = new KeyValueTableReader(streamReader);
				DevkitFoliageToolOptions.instance.read(reader);
			}
		}

		public static void save()
		{
			if (!DevkitFoliageToolOptions.wasLoaded)
			{
				return;
			}
			string path = IOUtility.rootPath + "/Cloud/Foliage.tool";
			string directoryName = Path.GetDirectoryName(path);
			if (!Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			using (StreamWriter streamWriter = new StreamWriter(path))
			{
				IFormattedFileWriter formattedFileWriter = new KeyValueTableWriter(streamWriter);
				formattedFileWriter.writeValue<DevkitFoliageToolOptions>(DevkitFoliageToolOptions.instance);
			}
		}

		public void read(IFormattedFileReader reader)
		{
			this.bakeInstancedMeshes = reader.readValue<bool>("Bake_Instanced_Meshes");
			this.bakeResources = reader.readValue<bool>("Bake_Resources");
			this.bakeObjects = reader.readValue<bool>("Bake_Objects");
			this.bakeClear = reader.readValue<bool>("Bake_Clear");
			this.bakeApplyScale = reader.readValue<bool>("Bake_Apply_Scale");
			this.brushRadius = reader.readValue<float>("Brush_Radius");
			this.brushFalloff = reader.readValue<float>("Brush_Falloff");
			this.brushStrength = reader.readValue<float>("Brush_Strength");
			this.densityTarget = reader.readValue<float>("Density_Target");
			this.surfaceMask = reader.readValue<ERayMask>("Surface_Mask");
			this.maxPreviewSamples = reader.readValue<uint>("Max_Preview_Samples");
		}

		public void write(IFormattedFileWriter writer)
		{
			writer.writeValue<bool>("Bake_Instanced_Meshes", this.bakeInstancedMeshes);
			writer.writeValue<bool>("Bake_Resources", this.bakeResources);
			writer.writeValue<bool>("Bake_Objects", this.bakeObjects);
			writer.writeValue<bool>("Bake_Clear", this.bakeClear);
			writer.writeValue<bool>("Bake_Apply_Scale", this.bakeApplyScale);
			writer.writeValue<float>("Brush_Radius", this.brushRadius);
			writer.writeValue<float>("Brush_Falloff", this.brushFalloff);
			writer.writeValue<float>("Brush_Strength", this.brushStrength);
			writer.writeValue<float>("Density_Target", this.densityTarget);
			writer.writeValue<ERayMask>("Surface_Mask", this.surfaceMask);
			writer.writeValue<uint>("Max_Preview_Samples", this.maxPreviewSamples);
		}

		private static DevkitFoliageToolOptions _instance;

		protected static float _addSensitivity = 1f;

		protected static float _removeSensitivity = 1f;

		[Inspectable("#SDG::Devkit.Foliage_Tool.Bake.Instanced_Meshes", null)]
		public bool bakeInstancedMeshes = true;

		[Inspectable("#SDG::Devkit.Foliage_Tool.Bake.Resources", null)]
		public bool bakeResources = true;

		[Inspectable("#SDG::Devkit.Foliage_Tool.Bake.Objects", null)]
		public bool bakeObjects = true;

		[Inspectable("#SDG::Devkit.Foliage_Tool.Bake.Clear", null)]
		public bool bakeClear;

		[Inspectable("#SDG::Devkit.Foliage_Tool.Bake.Apply_Scale", null)]
		public bool bakeApplyScale;

		[Inspectable("#SDG::Devkit.Foliage_Tool.Brush.Radius", null)]
		public float brushRadius = 16f;

		[Inspectable("#SDG::Devkit.Foliage_Tool.Brush.Falloff", null)]
		public float brushFalloff = 0.5f;

		[Inspectable("#SDG::Devkit.Foliage_Tool.Brush.Strength", null)]
		public float brushStrength = 0.05f;

		[Inspectable("#SDG::Devkit.Foliage_Tool.Density_Target", null)]
		public float densityTarget = 1f;

		[Inspectable("#SDG::Devkit.Foliage_Tool.Surface_Mask", null)]
		public ERayMask surfaceMask = ERayMask.LARGE | ERayMask.MEDIUM | ERayMask.SMALL | ERayMask.ENVIRONMENT | ERayMask.GROUND;

		[Inspectable("#SDG::Devkit.Foliage_Tool.Max_Preview_Samples", null)]
		public uint maxPreviewSamples = 64u;

		protected static bool wasLoaded;
	}
}
