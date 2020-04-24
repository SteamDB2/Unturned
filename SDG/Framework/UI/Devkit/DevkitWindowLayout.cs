using System;
using System.IO;
using SDG.Framework.IO;
using SDG.Framework.IO.FormattedFiles;
using SDG.Framework.IO.FormattedFiles.KeyValueTables;
using SDG.Framework.UI.Sleek2;

namespace SDG.Framework.UI.Devkit
{
	public class DevkitWindowLayout
	{
		public static void load(string name)
		{
			DevkitWindowLayout.wasLoaded = true;
			DevkitWindowManager.resetLayout();
			string path = IOUtility.rootPath + "/Cloud/Layouts/" + name + ".layout";
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
				IFormattedFileReader formattedFileReader = new KeyValueTableReader(streamReader);
				formattedFileReader.readKey("Root");
				DevkitWindowManager.partition.read(formattedFileReader);
				int num = formattedFileReader.readArrayLength("Containers");
				for (int i = 0; i < num; i++)
				{
					formattedFileReader.readArrayIndex(i);
					IFormattedFileReader formattedFileReader2 = formattedFileReader.readObject();
					if (formattedFileReader2 != null)
					{
						Type type = formattedFileReader2.readValue<Type>("Type");
						if (type != null)
						{
							Sleek2PopoutContainer sleek2PopoutContainer = DevkitWindowManager.addContainer(type);
							if (sleek2PopoutContainer != null)
							{
								formattedFileReader2.readKey("Container");
								sleek2PopoutContainer.read(formattedFileReader2);
							}
						}
					}
				}
			}
		}

		public static void save(string name)
		{
			if (!DevkitWindowLayout.wasLoaded)
			{
				return;
			}
			string path = IOUtility.rootPath + "/Cloud/Layouts/" + name + ".layout";
			string directoryName = Path.GetDirectoryName(path);
			if (!Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			using (StreamWriter streamWriter = new StreamWriter(path))
			{
				IFormattedFileWriter formattedFileWriter = new KeyValueTableWriter(streamWriter);
				formattedFileWriter.writeKey("Root");
				DevkitWindowManager.partition.write(formattedFileWriter);
				formattedFileWriter.writeKey("Containers");
				formattedFileWriter.beginArray();
				for (int i = 0; i < DevkitWindowManager.containers.Count; i++)
				{
					formattedFileWriter.beginObject();
					Sleek2PopoutContainer sleek2PopoutContainer = DevkitWindowManager.containers[i];
					formattedFileWriter.writeValue<Type>("Type", sleek2PopoutContainer.GetType());
					formattedFileWriter.writeValue<Sleek2PopoutContainer>("Container", sleek2PopoutContainer);
					formattedFileWriter.endObject();
				}
				formattedFileWriter.endArray();
			}
		}

		protected static bool wasLoaded;
	}
}
