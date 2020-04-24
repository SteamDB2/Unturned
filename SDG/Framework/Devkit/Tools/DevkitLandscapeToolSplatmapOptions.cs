using System;
using System.IO;
using SDG.Framework.Debug;
using SDG.Framework.IO;
using SDG.Framework.IO.FormattedFiles;
using SDG.Framework.IO.FormattedFiles.KeyValueTables;
using SDG.Unturned;

namespace SDG.Framework.Devkit.Tools
{
	public class DevkitLandscapeToolSplatmapOptions : IFormattedFileReadable, IFormattedFileWritable
	{
		static DevkitLandscapeToolSplatmapOptions()
		{
			DevkitLandscapeToolSplatmapOptions._instance = new DevkitLandscapeToolSplatmapOptions();
			DevkitLandscapeToolSplatmapOptions.load();
		}

		public static DevkitLandscapeToolSplatmapOptions instance
		{
			get
			{
				return DevkitLandscapeToolSplatmapOptions._instance;
			}
		}

		[TerminalCommandProperty("landscape.tool.paint_sensitivity", "multiplier for paint brush delta", 1)]
		public static float paintSensitivity
		{
			get
			{
				return DevkitLandscapeToolSplatmapOptions._paintSensitivity;
			}
			set
			{
				DevkitLandscapeToolSplatmapOptions._paintSensitivity = value;
				TerminalUtility.printCommandPass("Set paint_sensitivity to: " + DevkitLandscapeToolSplatmapOptions.paintSensitivity);
			}
		}

		public static void load()
		{
			DevkitLandscapeToolSplatmapOptions.wasLoaded = true;
			string path = IOUtility.rootPath + "/Cloud/Splatmap.tool";
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
				DevkitLandscapeToolSplatmapOptions.instance.read(reader);
			}
		}

		public static void save()
		{
			if (!DevkitLandscapeToolSplatmapOptions.wasLoaded)
			{
				return;
			}
			string path = IOUtility.rootPath + "/Cloud/Splatmap.tool";
			string directoryName = Path.GetDirectoryName(path);
			if (!Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			using (StreamWriter streamWriter = new StreamWriter(path))
			{
				IFormattedFileWriter formattedFileWriter = new KeyValueTableWriter(streamWriter);
				formattedFileWriter.writeValue<DevkitLandscapeToolSplatmapOptions>(DevkitLandscapeToolSplatmapOptions.instance);
			}
		}

		public void read(IFormattedFileReader reader)
		{
			this.brushRadius = reader.readValue<float>("Brush_Radius");
			this.brushFalloff = reader.readValue<float>("Brush_Falloff");
			this.brushStrength = reader.readValue<float>("Brush_Strength");
			this.useWeightTarget = reader.readValue<bool>("Use_Weight_Target");
			this.weightTarget = reader.readValue<float>("Weight_Target");
			this.maxPreviewSamples = reader.readValue<uint>("Max_Preview_Samples");
			this.smoothMethod = reader.readValue<DevkitLandscapeTool.EDevkitLandscapeToolSplatmapSmoothMethod>("Smooth_Method");
			this.previewMethod = reader.readValue<DevkitLandscapeTool.EDevkitLandscapeToolSplatmapPreviewMethod>("Preview_Method");
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

		public void write(IFormattedFileWriter writer)
		{
			writer.writeValue<float>("Brush_Radius", this.brushRadius);
			writer.writeValue<float>("Brush_Falloff", this.brushFalloff);
			writer.writeValue<float>("Brush_Strength", this.brushStrength);
			writer.writeValue<bool>("Use_Weight_Target", this.useWeightTarget);
			writer.writeValue<float>("Weight_Target", this.weightTarget);
			writer.writeValue<uint>("Max_Preview_Samples", this.maxPreviewSamples);
			writer.writeValue<DevkitLandscapeTool.EDevkitLandscapeToolSplatmapSmoothMethod>("Smooth_Method", this.smoothMethod);
			writer.writeValue<DevkitLandscapeTool.EDevkitLandscapeToolSplatmapPreviewMethod>("Preview_Method", this.previewMethod);
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

		private static DevkitLandscapeToolSplatmapOptions _instance;

		protected static float _paintSensitivity = 1f;

		[Inspectable("#SDG::Devkit.Landscape_Tool.Splatmap.Brush.Radius", null)]
		public float brushRadius = 16f;

		[Inspectable("#SDG::Devkit.Landscape_Tool.Splatmap.Brush.Falloff", null)]
		public float brushFalloff = 0.5f;

		[Inspectable("#SDG::Devkit.Landscape_Tool.Splatmap.Brush.Strength", null)]
		public float brushStrength = 1f;

		[Inspectable("#SDG::Devkit.Landscape_Tool.Splatmap.Use_Weight_Target", null)]
		public bool useWeightTarget;

		[Inspectable("#SDG::Devkit.Landscape_Tool.Splatmap.Weight_Target", null)]
		public float weightTarget;

		[Inspectable("#SDG::Devkit.Landscape_Tool.Splatmap.Max_Preview_Samples", null)]
		public uint maxPreviewSamples = 64u;

		[Inspectable("#SDG::Devkit.Landscape_Tool.Splatmap.Smooth_Method", null)]
		public DevkitLandscapeTool.EDevkitLandscapeToolSplatmapSmoothMethod smoothMethod;

		[Inspectable("#SDG::Devkit.Landscape_Tool.Splatmap.Preview_Method", null)]
		public DevkitLandscapeTool.EDevkitLandscapeToolSplatmapPreviewMethod previewMethod;

		[Inspectable("#SDG::Devkit.Landscape_Tool.Splatmap.Use_Auto_Slope", null)]
		public bool useAutoSlope;

		[Inspectable("#SDG::Devkit.Landscape_Tool.Splatmap.Auto_Min_Angle_Begin", null)]
		public float autoMinAngleBegin = 50f;

		[Inspectable("#SDG::Devkit.Landscape_Tool.Splatmap.Auto_Min_Angle_End", null)]
		public float autoMinAngleEnd = 70f;

		[Inspectable("#SDG::Devkit.Landscape_Tool.Splatmap.Auto_Max_Angle_Begin", null)]
		public float autoMaxAngleBegin = 90f;

		[Inspectable("#SDG::Devkit.Landscape_Tool.Splatmap.Auto_Max_Angle_End", null)]
		public float autoMaxAngleEnd = 90f;

		[Inspectable("#SDG::Devkit.Landscape_Tool.Splatmap.Use_Auto_Foundation", null)]
		public bool useAutoFoundation;

		[Inspectable("#SDG::Devkit.Landscape_Tool.Splatmap.Auto_Ray_Radius", null)]
		public float autoRayRadius;

		[Inspectable("#SDG::Devkit.Landscape_Tool.Splatmap.Auto_Ray_Length", null)]
		public float autoRayLength;

		[Inspectable("#SDG::Devkit.Landscape_Tool.Splatmap.Auto_Ray_Mask", null)]
		public ERayMask autoRayMask;

		protected static bool wasLoaded;
	}
}
