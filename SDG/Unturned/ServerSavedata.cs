using System;

namespace SDG.Unturned
{
	public class ServerSavedata
	{
		public static string directory
		{
			get
			{
				if (Dedicator.isDedicated)
				{
					return "/Servers";
				}
				return "/Worlds";
			}
		}

		public static void serializeJSON<T>(string path, T instance)
		{
			ReadWrite.serializeJSON<T>(ServerSavedata.directory + "/" + Provider.serverID + path, false, instance);
		}

		public static T deserializeJSON<T>(string path)
		{
			return ReadWrite.deserializeJSON<T>(ServerSavedata.directory + "/" + Provider.serverID + path, false);
		}

		public static void writeData(string path, Data data)
		{
			ReadWrite.writeData(ServerSavedata.directory + "/" + Provider.serverID + path, false, data);
		}

		public static Data readData(string path)
		{
			return ReadWrite.readData(ServerSavedata.directory + "/" + Provider.serverID + path, false);
		}

		public static void writeBlock(string path, Block block)
		{
			ReadWrite.writeBlock(ServerSavedata.directory + "/" + Provider.serverID + path, false, block);
		}

		public static Block readBlock(string path, byte prefix)
		{
			return ReadWrite.readBlock(ServerSavedata.directory + "/" + Provider.serverID + path, false, prefix);
		}

		public static River openRiver(string path, bool isReading)
		{
			return new River(ServerSavedata.directory + "/" + Provider.serverID + path, true, false, isReading);
		}

		public static void deleteFile(string path)
		{
			ReadWrite.deleteFile(ServerSavedata.directory + "/" + Provider.serverID + path, false);
		}

		public static bool fileExists(string path)
		{
			return ReadWrite.fileExists(ServerSavedata.directory + "/" + Provider.serverID + path, false);
		}

		public static void createFolder(string path)
		{
			ReadWrite.createFolder(ServerSavedata.directory + "/" + Provider.serverID + path);
		}

		public static void deleteFolder(string path)
		{
			ReadWrite.deleteFolder(ServerSavedata.directory + "/" + Provider.serverID + path);
		}

		public static bool folderExists(string path)
		{
			return ReadWrite.folderExists(ServerSavedata.directory + "/" + Provider.serverID + path);
		}
	}
}
