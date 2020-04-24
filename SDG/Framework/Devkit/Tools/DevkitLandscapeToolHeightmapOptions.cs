using System;
using System.IO;
using SDG.Framework.Debug;
using SDG.Framework.IO;
using SDG.Framework.IO.FormattedFiles;
using SDG.Framework.IO.FormattedFiles.KeyValueTables;

namespace SDG.Framework.Devkit.Tools
{
	public class DevkitLandscapeToolHeightmapOptions : IFormattedFileReadable, IFormattedFileWritable
	{
		static DevkitLandscapeToolHeightmapOptions()
		{
			DevkitLandscapeToolHeightmapOptions._instance = new DevkitLandscapeToolHeightmapOptions();
			DevkitLandscapeToolHeightmapOptions.load();
		}

		public static DevkitLandscapeToolHeightmapOptions instance
		{
			get
			{
				return DevkitLandscapeToolHeightmapOptions._instance;
			}
		}

		[TerminalCommandProperty("landscape.tool.adjust_sensitivity", "multiplier for adjust brush delta", 0.1f)]
		public static float adjustSensitivity
		{
			get
			{
				return DevkitLandscapeToolHeightmapOptions._adjustSensitivity;
			}
			set
			{
				DevkitLandscapeToolHeightmapOptions._adjustSensitivity = value;
				TerminalUtility.printCommandPass("Set adjust_sensitivity to: " + DevkitLandscapeToolHeightmapOptions.adjustSensitivity);
			}
		}

		[TerminalCommandProperty("landscape.tool.flatten_sensitivity", "multiplier for flatten brush delta", 1)]
		public static float flattenSensitivity
		{
			get
			{
				return DevkitLandscapeToolHeightmapOptions._flattenSensitivity;
			}
			set
			{
				DevkitLandscapeToolHeightmapOptions._flattenSensitivity = value;
				TerminalUtility.printCommandPass("Set flatten_sensitivity to: " + DevkitLandscapeToolHeightmapOptions.flattenSensitivity);
			}
		}

		public static void load()
		{
			DevkitLandscapeToolHeightmapOptions.wasLoaded = true;
			string path = IOUtility.rootPath + "/Cloud/Heightmap.tool";
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
				DevkitLandscapeToolHeightmapOptions.instance.read(reader);
			}
		}

		public static void save()
		{
			if (!DevkitLandscapeToolHeightmapOptions.wasLoaded)
			{
				return;
			}
			string path = IOUtility.rootPath + "/Cloud/Heightmap.tool";
			string directoryName = Path.GetDirectoryName(path);
			if (!Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			using (StreamWriter streamWriter = new StreamWriter(path))
			{
				IFormattedFileWriter formattedFileWriter = new KeyValueTableWriter(streamWriter);
				formattedFileWriter.writeValue<DevkitLandscapeToolHeightmapOptions>(DevkitLandscapeToolHeightmapOptions.instance);
			}
		}

		public void read(IFormattedFileReader reader)
		{
			this.brushRadius = reader.readValue<float>("Brush_Radius");
			this.brushFalloff = reader.readValue<float>("Brush_Falloff");
			this.brushStrength = reader.readValue<float>("Brush_Strength");
			this.flattenTarget = reader.readValue<float>("Flatten_Target");
			this.maxPreviewSamples = reader.readValue<uint>("Max_Preview_Samples");
			this.smoothMethod = reader.readValue<DevkitLandscapeTool.EDevkitLandscapeToolHeightmapSmoothMethod>("Smooth_Method");
		}

		public void write(IFormattedFileWriter writer)
		{
			writer.writeValue<float>("Brush_Radius", this.brushRadius);
			writer.writeValue<float>("Brush_Falloff", this.brushFalloff);
			writer.writeValue<float>("Brush_Strength", this.brushStrength);
			writer.writeValue<float>("Flatten_Target", this.flattenTarget);
			writer.writeValue<uint>("Max_Preview_Samples", this.maxPreviewSamples);
			writer.writeValue<DevkitLandscapeTool.EDevkitLandscapeToolHeightmapSmoothMethod>("Smooth_Method", this.smoothMethod);
		}

		private static DevkitLandscapeToolHeightmapOptions _instance;

		protected static float _adjustSensitivity = 0.1f;

		protected static float _flattenSensitivity = 1f;

		[Inspectable("#SDG::Devkit.Landscape_Tool.Heightmap.Brush.Radius", null)]
		public float brushRadius = 16f;

		[Inspectable("#SDG::Devkit.Landscape_Tool.Heightmap.Brush.Falloff", null)]
		public float brushFalloff = 0.5f;

		[Inspectable("#SDG::Devkit.Landscape_Tool.Heightmap.Brush.Strength", null)]
		public float brushStrength = 0.05f;

		[Inspectable("#SDG::Devkit.Landscape_Tool.Heightmap.Flatten_Target", null)]
		public float flattenTarget;

		[Inspectable("#SDG::Devkit.Landscape_Tool.Heightmap.Max_Preview_Samples", null)]
		public uint maxPreviewSamples = 64u;

		[Inspectable("#SDG::Devkit.Landscape_Tool.Heightmap.Smooth_Method", null)]
		public DevkitLandscapeTool.EDevkitLandscapeToolHeightmapSmoothMethod smoothMethod;

		protected static bool wasLoaded;
	}
}
