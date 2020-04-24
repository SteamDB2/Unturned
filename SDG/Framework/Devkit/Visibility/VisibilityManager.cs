using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using SDG.Framework.Debug;
using SDG.Framework.IO;
using SDG.Framework.IO.FormattedFiles;
using SDG.Framework.IO.FormattedFiles.KeyValueTables;
using SDG.Framework.Modules;

namespace SDG.Framework.Devkit.Visibility
{
	public static class VisibilityManager
	{
		static VisibilityManager()
		{
			VisibilityManager.groups.canInspectorAdd = false;
			VisibilityManager.groups.canInspectorRemove = false;
			VisibilityManager.savedGroups = new Dictionary<string, IVisibilityGroup>();
			if (VisibilityManager.<>f__mg$cache0 == null)
			{
				VisibilityManager.<>f__mg$cache0 = new ModulesInitializedHandler(VisibilityManager.handleModulesInitialized);
			}
			ModuleHook.onModulesInitialized += VisibilityManager.<>f__mg$cache0;
		}

		[Inspectable("#SDG::Groups", null)]
		public static InspectableList<IVisibilityGroup> groups { get; private set; } = new InspectableList<IVisibilityGroup>();

		public static event VisibilityManagerGroupRegisteredHandler groupRegistered;

		public static T registerVisibilityGroup<T>(T defaultGroup) where T : IVisibilityGroup
		{
			foreach (IVisibilityGroup visibilityGroup in VisibilityManager.groups)
			{
				if (visibilityGroup.internalName == defaultGroup.internalName)
				{
					VisibilityManager.triggerGroupRegistered(visibilityGroup);
					return (T)((object)visibilityGroup);
				}
			}
			IVisibilityGroup visibilityGroup2;
			if (VisibilityManager.savedGroups.TryGetValue(defaultGroup.internalName, out visibilityGroup2))
			{
				visibilityGroup2.displayName = defaultGroup.displayName;
				if (visibilityGroup2.GetType() == defaultGroup.GetType())
				{
					defaultGroup = (T)((object)visibilityGroup2);
				}
			}
			VisibilityManager.groups.Add(defaultGroup);
			VisibilityManager.triggerGroupRegistered(defaultGroup);
			return defaultGroup;
		}

		public static void load()
		{
			VisibilityManager.wasLoaded = true;
			string path = IOUtility.rootPath + "/Cloud/Visibility.config";
			if (!File.Exists(path))
			{
				return;
			}
			using (StreamReader streamReader = new StreamReader(path))
			{
				IFormattedFileReader formattedFileReader = new KeyValueTableReader(streamReader);
				int num = formattedFileReader.readArrayLength("Groups");
				for (int i = 0; i < num; i++)
				{
					formattedFileReader.readArrayIndex(i);
					IFormattedFileReader formattedFileReader2 = formattedFileReader.readObject();
					string text = formattedFileReader2.readValue<string>("Name");
					Type type = formattedFileReader2.readValue<Type>("Type");
					if (type != null)
					{
						IVisibilityGroup visibilityGroup = formattedFileReader2.readValue(type, "Group") as IVisibilityGroup;
						visibilityGroup.internalName = text;
						VisibilityManager.savedGroups.Add(text, visibilityGroup);
					}
				}
			}
		}

		public static void save()
		{
			if (!VisibilityManager.wasLoaded)
			{
				return;
			}
			string path = IOUtility.rootPath + "/Cloud/Visibility.config";
			string directoryName = Path.GetDirectoryName(path);
			if (!Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			using (StreamWriter streamWriter = new StreamWriter(path))
			{
				IFormattedFileWriter formattedFileWriter = new KeyValueTableWriter(streamWriter);
				formattedFileWriter.writeKey("Groups");
				formattedFileWriter.beginArray();
				foreach (IVisibilityGroup visibilityGroup in VisibilityManager.groups)
				{
					formattedFileWriter.beginObject();
					formattedFileWriter.writeValue("Name", visibilityGroup.internalName);
					formattedFileWriter.writeValue<Type>("Type", visibilityGroup.GetType());
					formattedFileWriter.writeValue<IVisibilityGroup>("Group", visibilityGroup);
					formattedFileWriter.endObject();
				}
				formattedFileWriter.endArray();
			}
		}

		private static void triggerGroupRegistered(IVisibilityGroup group)
		{
			VisibilityManagerGroupRegisteredHandler visibilityManagerGroupRegisteredHandler = VisibilityManager.groupRegistered;
			if (visibilityManagerGroupRegisteredHandler != null)
			{
				visibilityManagerGroupRegisteredHandler(group);
			}
		}

		private static void handleModulesInitialized()
		{
			VisibilityManager.load();
		}

		private static Dictionary<string, IVisibilityGroup> savedGroups;

		public static bool wasLoaded;

		[CompilerGenerated]
		private static ModulesInitializedHandler <>f__mg$cache0;
	}
}
