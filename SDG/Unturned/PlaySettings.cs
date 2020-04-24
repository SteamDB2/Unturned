using System;

namespace SDG.Unturned
{
	public class PlaySettings
	{
		public static void load()
		{
			if (ReadWrite.fileExists("/Play.dat", true))
			{
				Block block = ReadWrite.readBlock("/Play.dat", true, 0);
				if (block != null)
				{
					byte b = block.readByte();
					if (b > 1)
					{
						PlaySettings.connectIP = block.readString();
						PlaySettings.connectPort = block.readUInt16();
						PlaySettings.connectPassword = block.readString();
						if (b > 3)
						{
							PlaySettings.serversName = block.readString();
						}
						else
						{
							PlaySettings.serversName = string.Empty;
						}
						PlaySettings.serversPassword = block.readString();
						PlaySettings.singleplayerMode = (EGameMode)block.readByte();
						if (b < 8)
						{
							PlaySettings.singleplayerMode = EGameMode.NORMAL;
						}
						if (b > 10)
						{
							PlaySettings.matchmakingMode = (EGameMode)block.readByte();
						}
						else
						{
							PlaySettings.matchmakingMode = EGameMode.NORMAL;
						}
						if (b < 7)
						{
							PlaySettings.singleplayerCheats = false;
						}
						else
						{
							PlaySettings.singleplayerCheats = block.readBoolean();
						}
						if (b > 4)
						{
							PlaySettings.singleplayerMap = block.readString();
							PlaySettings.editorMap = block.readString();
						}
						else
						{
							PlaySettings.singleplayerMap = string.Empty;
							PlaySettings.editorMap = string.Empty;
						}
						if (b > 10)
						{
							PlaySettings.matchmakingMap = block.readString();
						}
						else
						{
							PlaySettings.matchmakingMap = string.Empty;
						}
						if (b > 5)
						{
							PlaySettings.isVR = block.readBoolean();
							if (b < 9)
							{
								PlaySettings.isVR = false;
							}
						}
						else
						{
							PlaySettings.isVR = false;
						}
						if (b < 10)
						{
							PlaySettings.singleplayerCategory = ESingleplayerMapCategory.OFFICIAL;
						}
						else
						{
							PlaySettings.singleplayerCategory = (ESingleplayerMapCategory)block.readByte();
						}
						return;
					}
				}
			}
			PlaySettings.connectIP = "127.0.0.1";
			PlaySettings.connectPort = 27015;
			PlaySettings.connectPassword = string.Empty;
			PlaySettings.serversName = string.Empty;
			PlaySettings.serversPassword = string.Empty;
			PlaySettings.singleplayerMode = EGameMode.NORMAL;
			PlaySettings.matchmakingMode = EGameMode.NORMAL;
			PlaySettings.singleplayerCheats = false;
			PlaySettings.singleplayerMap = string.Empty;
			PlaySettings.matchmakingMap = string.Empty;
			PlaySettings.editorMap = string.Empty;
			PlaySettings.singleplayerCategory = ESingleplayerMapCategory.OFFICIAL;
		}

		public static void save()
		{
			Block block = new Block();
			block.writeByte(PlaySettings.SAVEDATA_VERSION);
			block.writeString(PlaySettings.connectIP);
			block.writeUInt16(PlaySettings.connectPort);
			block.writeString(PlaySettings.connectPassword);
			block.writeString(PlaySettings.serversName);
			block.writeString(PlaySettings.serversPassword);
			block.writeByte((byte)PlaySettings.singleplayerMode);
			block.writeByte((byte)PlaySettings.matchmakingMode);
			block.writeBoolean(PlaySettings.singleplayerCheats);
			block.writeString(PlaySettings.singleplayerMap);
			block.writeString(PlaySettings.matchmakingMap);
			block.writeString(PlaySettings.editorMap);
			block.writeBoolean(PlaySettings.isVR);
			block.writeByte((byte)PlaySettings.singleplayerCategory);
			ReadWrite.writeBlock("/Play.dat", true, block);
		}

		public static readonly byte SAVEDATA_VERSION = 11;

		public static string connectIP;

		public static ushort connectPort;

		public static string connectPassword;

		public static string serversName;

		public static string serversPassword;

		public static EGameMode singleplayerMode;

		public static EGameMode matchmakingMode;

		public static bool singleplayerCheats;

		public static string singleplayerMap;

		public static string matchmakingMap;

		public static string editorMap;

		public static bool isVR;

		public static ESingleplayerMapCategory singleplayerCategory;
	}
}
