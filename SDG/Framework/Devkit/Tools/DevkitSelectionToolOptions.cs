using System;
using System.IO;
using SDG.Framework.Debug;
using SDG.Framework.IO;
using SDG.Framework.IO.FormattedFiles;
using SDG.Framework.IO.FormattedFiles.KeyValueTables;
using SDG.Unturned;

namespace SDG.Framework.Devkit.Tools
{
	public class DevkitSelectionToolOptions : IFormattedFileReadable, IFormattedFileWritable
	{
		static DevkitSelectionToolOptions()
		{
			DevkitSelectionToolOptions.load();
		}

		public static DevkitSelectionToolOptions instance
		{
			get
			{
				return DevkitSelectionToolOptions._instance;
			}
		}

		public static void load()
		{
			DevkitSelectionToolOptions.wasLoaded = true;
			string path = IOUtility.rootPath + "/Cloud/Selection.tool";
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
				DevkitSelectionToolOptions.instance.read(reader);
			}
		}

		public static void save()
		{
			if (!DevkitSelectionToolOptions.wasLoaded)
			{
				return;
			}
			string path = IOUtility.rootPath + "/Cloud/Selection.tool";
			string directoryName = Path.GetDirectoryName(path);
			if (!Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			using (StreamWriter streamWriter = new StreamWriter(path))
			{
				IFormattedFileWriter formattedFileWriter = new KeyValueTableWriter(streamWriter);
				formattedFileWriter.writeValue<DevkitSelectionToolOptions>(DevkitSelectionToolOptions.instance);
			}
		}

		public void read(IFormattedFileReader reader)
		{
			this.snapPosition = reader.readValue<float>("Snap_Position");
			this.snapRotation = reader.readValue<float>("Snap_Rotation");
			this.snapScale = reader.readValue<float>("Snap_Scale");
			this.surfaceOffset = reader.readValue<float>("Surface_Offset");
			this.surfaceAlign = reader.readValue<bool>("Surface_Align");
			this.localSpace = reader.readValue<bool>("Local_Space");
			this.lockHandles = reader.readValue<bool>("Lock_Handles");
			this.selectionMask = reader.readValue<ERayMask>("Selection_Mask");
		}

		public void write(IFormattedFileWriter writer)
		{
			writer.writeValue<float>("Snap_Position", this.snapPosition);
			writer.writeValue<float>("Snap_Rotation", this.snapRotation);
			writer.writeValue<float>("Snap_Scale", this.snapScale);
			writer.writeValue<float>("Surface_Offset", this.surfaceOffset);
			writer.writeValue<bool>("Surface_Align", this.surfaceAlign);
			writer.writeValue<bool>("Local_Space", this.localSpace);
			writer.writeValue<bool>("Lock_Handles", this.lockHandles);
			writer.writeValue<ERayMask>("Selection_Mask", this.selectionMask);
		}

		private static DevkitSelectionToolOptions _instance = new DevkitSelectionToolOptions();

		[Inspectable("#SDG::Devkit.Selection_Tool.Snap_Position", null)]
		public float snapPosition = 1f;

		[Inspectable("#SDG::Devkit.Selection_Tool.Snap_Rotation", null)]
		public float snapRotation = 15f;

		[Inspectable("#SDG::Devkit.Selection_Tool.Snap_Scale", null)]
		public float snapScale = 0.1f;

		[Inspectable("#SDG::Devkit.Selection_Tool.Surface_Offset", null)]
		public float surfaceOffset;

		[Inspectable("#SDG::Devkit.Selection_Tool.Surface_Align", null)]
		public bool surfaceAlign;

		[Inspectable("#SDG::Devkit.Selection_Tool.Local_Space", null)]
		public bool localSpace;

		[Inspectable("#SDG::Devkit.Selection_Tool.Lock_Handles", null)]
		public bool lockHandles;

		[Inspectable("#SDG::Devkit.Foliage_Tool.Selection_Mask", null)]
		public ERayMask selectionMask = ERayMask.LARGE | ERayMask.MEDIUM | ERayMask.SMALL | ERayMask.ENVIRONMENT | ERayMask.GROUND | ERayMask.CLIP | ERayMask.TRAP;

		public IDevkitSelectionToolInstantiationInfo instantiationInfo;

		protected static bool wasLoaded;
	}
}
