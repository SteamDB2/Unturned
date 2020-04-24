using System;

namespace SDG.Unturned
{
	public class PlayerSavedata
	{
		public static void writeData(SteamPlayerID playerID, string path, Data data)
		{
			if (PlayerSavedata.hasSync)
			{
				ReadWrite.writeData(string.Concat(new object[]
				{
					"/Sync/",
					playerID.steamID,
					"_",
					playerID.characterID,
					"/",
					Level.info.name,
					path
				}), false, data);
			}
			else
			{
				ServerSavedata.writeData(string.Concat(new object[]
				{
					"/Players/",
					playerID.steamID,
					"_",
					playerID.characterID,
					"/",
					Level.info.name,
					path
				}), data);
			}
		}

		public static Data readData(SteamPlayerID playerID, string path)
		{
			if (PlayerSavedata.hasSync)
			{
				return ReadWrite.readData(string.Concat(new object[]
				{
					"/Sync/",
					playerID.steamID,
					"_",
					playerID.characterID,
					"/",
					Level.info.name,
					path
				}), false);
			}
			return ServerSavedata.readData(string.Concat(new object[]
			{
				"/Players/",
				playerID.steamID,
				"_",
				playerID.characterID,
				"/",
				Level.info.name,
				path
			}));
		}

		public static void writeBlock(SteamPlayerID playerID, string path, Block block)
		{
			if (PlayerSavedata.hasSync)
			{
				ReadWrite.writeBlock(string.Concat(new object[]
				{
					"/Sync/",
					playerID.steamID,
					"_",
					playerID.characterID,
					"/",
					Level.info.name,
					path
				}), false, block);
			}
			else
			{
				ServerSavedata.writeBlock(string.Concat(new object[]
				{
					"/Players/",
					playerID.steamID,
					"_",
					playerID.characterID,
					"/",
					Level.info.name,
					path
				}), block);
			}
		}

		public static Block readBlock(SteamPlayerID playerID, string path, byte prefix)
		{
			if (PlayerSavedata.hasSync)
			{
				return ReadWrite.readBlock(string.Concat(new object[]
				{
					"/Sync/",
					playerID.steamID,
					"_",
					playerID.characterID,
					"/",
					Level.info.name,
					path
				}), false, prefix);
			}
			return ServerSavedata.readBlock(string.Concat(new object[]
			{
				"/Players/",
				playerID.steamID,
				"_",
				playerID.characterID,
				"/",
				Level.info.name,
				path
			}), prefix);
		}

		public static River openRiver(SteamPlayerID playerID, string path, bool isReading)
		{
			if (PlayerSavedata.hasSync)
			{
				return new River(string.Concat(new object[]
				{
					"/Sync/",
					playerID.steamID,
					"_",
					playerID.characterID,
					"/",
					Level.info.name,
					path
				}), true, false, isReading);
			}
			return ServerSavedata.openRiver(string.Concat(new object[]
			{
				"/Players/",
				playerID.steamID,
				"_",
				playerID.characterID,
				"/",
				Level.info.name,
				path
			}), isReading);
		}

		public static void deleteFile(SteamPlayerID playerID, string path)
		{
			if (PlayerSavedata.hasSync)
			{
				ReadWrite.deleteFile(string.Concat(new object[]
				{
					"/Sync/",
					playerID.steamID,
					"_",
					playerID.characterID,
					"/",
					Level.info.name,
					path
				}), false);
			}
			else
			{
				ServerSavedata.deleteFile(string.Concat(new object[]
				{
					"/Players/",
					playerID.steamID,
					"_",
					playerID.characterID,
					"/",
					Level.info.name,
					path
				}));
			}
		}

		public static bool fileExists(SteamPlayerID playerID, string path)
		{
			if (PlayerSavedata.hasSync)
			{
				return ReadWrite.fileExists(string.Concat(new object[]
				{
					"/Sync/",
					playerID.steamID,
					"_",
					playerID.characterID,
					"/",
					Level.info.name,
					path
				}), false);
			}
			return ServerSavedata.fileExists(string.Concat(new object[]
			{
				"/Players/",
				playerID.steamID,
				"_",
				playerID.characterID,
				"/",
				Level.info.name,
				path
			}));
		}

		public static void deleteFolder(SteamPlayerID playerID)
		{
			if (PlayerSavedata.hasSync)
			{
				for (byte b = 0; b < Customization.FREE_CHARACTERS + Customization.PRO_CHARACTERS; b += 1)
				{
					if (ReadWrite.folderExists(string.Concat(new object[]
					{
						"/Sync/",
						playerID.steamID,
						"_",
						playerID.characterID
					}), false))
					{
						ReadWrite.deleteFolder(string.Concat(new object[]
						{
							"/Sync/",
							playerID.steamID,
							"_",
							playerID.characterID
						}), false);
					}
				}
			}
			else
			{
				for (byte b2 = 0; b2 < Customization.FREE_CHARACTERS + Customization.PRO_CHARACTERS; b2 += 1)
				{
					if (ServerSavedata.folderExists(string.Concat(new object[]
					{
						"/Players/",
						playerID.steamID,
						"_",
						playerID.characterID
					})))
					{
						ServerSavedata.deleteFolder(string.Concat(new object[]
						{
							"/Players/",
							playerID.steamID,
							"_",
							playerID.characterID
						}));
					}
				}
			}
		}

		public static bool hasSync;
	}
}
