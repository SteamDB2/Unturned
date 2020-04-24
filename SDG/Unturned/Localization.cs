using System;
using System.Collections.Generic;
using System.IO;

namespace SDG.Unturned
{
	public class Localization
	{
		public static List<string> messages
		{
			get
			{
				return Localization._messages;
			}
		}

		public static Local tryRead(string path)
		{
			return Localization.tryRead(path, true);
		}

		public static Local tryRead(string path, bool usePath)
		{
			if (ReadWrite.fileExists(path + "/" + Provider.language + ".dat", false, usePath))
			{
				return new Local(ReadWrite.readData(path + "/" + Provider.language + ".dat", false, usePath));
			}
			if (ReadWrite.fileExists(path + "/English.dat", false, usePath))
			{
				return new Local(ReadWrite.readData(path + "/English.dat", false, usePath));
			}
			return new Local();
		}

		public static Local read(string path)
		{
			if (ReadWrite.fileExists(Provider.path + Provider.language + path, false, false))
			{
				return new Local(ReadWrite.readData(Provider.path + Provider.language + path, false, false));
			}
			return new Local();
		}

		private static void scanFile(string path)
		{
			Data data = ReadWrite.readData("/Localization/English/" + path, false, true);
			Data data2 = ReadWrite.readData(Provider.path + Provider.language + path, false, false);
			KeyValuePair<string, string>[] contents = data.getContents();
			KeyValuePair<string, string>[] contents2 = data2.getContents();
			Localization.keys.Clear();
			for (int i = 0; i < contents.Length; i++)
			{
				string key = contents[i].Key;
				bool flag = false;
				for (int j = 0; j < contents2.Length; j++)
				{
					string key2 = contents2[j].Key;
					if (key == key2)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					Localization.keys.Add(key);
				}
			}
			if (Localization.keys.Count > 0)
			{
				string text = string.Empty;
				for (int k = 0; k < Localization.keys.Count; k++)
				{
					if (k == 0)
					{
						text += Localization.keys[k];
					}
					else if (k == Localization.keys.Count - 1)
					{
						text = text + " and " + Localization.keys[k];
					}
					else
					{
						text = text + ", " + Localization.keys[k];
					}
				}
				Localization.messages.Add(string.Concat(new object[]
				{
					path,
					" has ",
					Localization.keys.Count,
					" new keys: ",
					text
				}));
			}
		}

		private static void scanFolder(string path)
		{
			string[] files = ReadWrite.getFiles("/Localization/English/" + path, true);
			string[] files2 = ReadWrite.getFiles(Provider.path + Provider.language + path, false);
			for (int i = 0; i < files.Length; i++)
			{
				string fileName = Path.GetFileName(files[i]);
				bool flag = false;
				for (int j = 0; j < files2.Length; j++)
				{
					string fileName2 = Path.GetFileName(files2[j]);
					if (fileName == fileName2)
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					Localization.scanFile(path + "/" + fileName);
				}
				else
				{
					Localization.messages.Add("New file \"" + fileName + "\" in " + path);
				}
			}
			string[] folders = ReadWrite.getFolders("/Localization/English/" + path, true);
			string[] folders2 = ReadWrite.getFolders(Provider.path + Provider.language + path, false);
			for (int k = 0; k < folders.Length; k++)
			{
				string fileName3 = Path.GetFileName(folders[k]);
				bool flag2 = false;
				for (int l = 0; l < folders2.Length; l++)
				{
					string fileName4 = Path.GetFileName(folders2[l]);
					if (fileName3 == fileName4)
					{
						flag2 = true;
						break;
					}
				}
				if (flag2)
				{
					Localization.scanFolder(path + "/" + fileName3);
				}
				else
				{
					Localization.messages.Add("New folder \"" + fileName3 + "\" in " + path);
				}
			}
		}

		public static void refresh()
		{
			if (Localization.messages == null)
			{
				Localization._messages = new List<string>();
			}
			else
			{
				Localization.messages.Clear();
			}
			Localization.scanFolder("/Player");
			Localization.scanFolder("/Menu");
			Localization.scanFolder("/Server");
			Localization.scanFolder("/Editor");
		}

		private static List<string> _messages;

		private static List<string> keys = new List<string>();
	}
}
