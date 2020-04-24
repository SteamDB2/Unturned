using System;
using System.Collections.Generic;
using Steamworks;

namespace SDG.Unturned
{
	public class SteamWhitelist
	{
		public static List<SteamWhitelistID> list
		{
			get
			{
				return SteamWhitelist._list;
			}
		}

		public static void whitelist(CSteamID steamID, string tag, CSteamID judgeID)
		{
			for (int i = 0; i < SteamWhitelist.list.Count; i++)
			{
				if (SteamWhitelist.list[i].steamID == steamID)
				{
					SteamWhitelist.list[i].tag = tag;
					SteamWhitelist.list[i].judgeID = judgeID;
					return;
				}
			}
			SteamWhitelist.list.Add(new SteamWhitelistID(steamID, tag, judgeID));
		}

		public static bool unwhitelist(CSteamID steamID)
		{
			for (int i = 0; i < SteamWhitelist.list.Count; i++)
			{
				if (SteamWhitelist.list[i].steamID == steamID)
				{
					if (Provider.isWhitelisted)
					{
						Provider.kick(steamID, "Removed from whitelist.");
					}
					SteamWhitelist.list.RemoveAt(i);
					return true;
				}
			}
			return false;
		}

		public static bool checkWhitelisted(CSteamID steamID)
		{
			for (int i = 0; i < SteamWhitelist.list.Count; i++)
			{
				if (SteamWhitelist.list[i].steamID == steamID)
				{
					return true;
				}
			}
			return false;
		}

		public static void load()
		{
			SteamWhitelist._list = new List<SteamWhitelistID>();
			if (ServerSavedata.fileExists("/Server/Whitelist.dat"))
			{
				River river = ServerSavedata.openRiver("/Server/Whitelist.dat", true);
				byte b = river.readByte();
				if (b > 1)
				{
					ushort num = river.readUInt16();
					for (ushort num2 = 0; num2 < num; num2 += 1)
					{
						CSteamID newSteamID = river.readSteamID();
						string newTag = river.readString();
						CSteamID newJudgeID = river.readSteamID();
						SteamWhitelistID item = new SteamWhitelistID(newSteamID, newTag, newJudgeID);
						SteamWhitelist.list.Add(item);
					}
				}
			}
		}

		public static void save()
		{
			River river = ServerSavedata.openRiver("/Server/Whitelist.dat", false);
			river.writeByte(SteamWhitelist.SAVEDATA_VERSION);
			river.writeUInt16((ushort)SteamWhitelist.list.Count);
			ushort num = 0;
			while ((int)num < SteamWhitelist.list.Count)
			{
				SteamWhitelistID steamWhitelistID = SteamWhitelist.list[(int)num];
				river.writeSteamID(steamWhitelistID.steamID);
				river.writeString(steamWhitelistID.tag);
				river.writeSteamID(steamWhitelistID.judgeID);
				num += 1;
			}
			river.closeRiver();
		}

		public static readonly byte SAVEDATA_VERSION = 2;

		private static List<SteamWhitelistID> _list;
	}
}
