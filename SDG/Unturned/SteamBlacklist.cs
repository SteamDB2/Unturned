using System;
using System.Collections.Generic;
using Steamworks;

namespace SDG.Unturned
{
	public class SteamBlacklist
	{
		public static List<SteamBlacklistID> list
		{
			get
			{
				return SteamBlacklist._list;
			}
		}

		[Obsolete]
		public static void ban(CSteamID playerID, CSteamID judgeID, string reason, uint duration)
		{
			SteamBlacklist.ban(playerID, 0u, judgeID, reason, duration);
		}

		public static void ban(CSteamID playerID, uint ip, CSteamID judgeID, string reason, uint duration)
		{
			Provider.ban(playerID, reason, duration);
			for (int i = 0; i < SteamBlacklist.list.Count; i++)
			{
				if (SteamBlacklist.list[i].playerID == playerID)
				{
					SteamBlacklist.list[i].judgeID = judgeID;
					SteamBlacklist.list[i].reason = reason;
					SteamBlacklist.list[i].duration = duration;
					SteamBlacklist.list[i].banned = Provider.time;
					return;
				}
			}
			SteamBlacklist.list.Add(new SteamBlacklistID(playerID, ip, judgeID, reason, duration, Provider.time));
		}

		public static bool unban(CSteamID playerID)
		{
			for (int i = 0; i < SteamBlacklist.list.Count; i++)
			{
				if (SteamBlacklist.list[i].playerID == playerID)
				{
					SteamBlacklist.list.RemoveAt(i);
					return true;
				}
			}
			return false;
		}

		[Obsolete]
		public static bool checkBanned(CSteamID playerID, out SteamBlacklistID blacklistID)
		{
			return SteamBlacklist.checkBanned(playerID, 0u, out blacklistID);
		}

		public static bool checkBanned(CSteamID playerID, uint ip, out SteamBlacklistID blacklistID)
		{
			blacklistID = null;
			int i = 0;
			while (i < SteamBlacklist.list.Count)
			{
				if (SteamBlacklist.list[i].playerID == playerID || (SteamBlacklist.list[i].ip == ip && ip != 0u))
				{
					if (SteamBlacklist.list[i].isExpired)
					{
						SteamBlacklist.list.RemoveAt(i);
						return false;
					}
					blacklistID = SteamBlacklist.list[i];
					return true;
				}
				else
				{
					i++;
				}
			}
			return false;
		}

		public static void load()
		{
			SteamBlacklist._list = new List<SteamBlacklistID>();
			if (ServerSavedata.fileExists("/Server/Blacklist.dat"))
			{
				River river = ServerSavedata.openRiver("/Server/Blacklist.dat", true);
				byte b = river.readByte();
				if (b > 1)
				{
					ushort num = river.readUInt16();
					for (ushort num2 = 0; num2 < num; num2 += 1)
					{
						CSteamID newPlayerID = river.readSteamID();
						uint newIP;
						if (b > 2)
						{
							newIP = river.readUInt32();
						}
						else
						{
							newIP = 0u;
						}
						CSteamID newJudgeID = river.readSteamID();
						string newReason = river.readString();
						uint newDuration = river.readUInt32();
						uint newBanned = river.readUInt32();
						SteamBlacklistID steamBlacklistID = new SteamBlacklistID(newPlayerID, newIP, newJudgeID, newReason, newDuration, newBanned);
						if (!steamBlacklistID.isExpired)
						{
							SteamBlacklist.list.Add(steamBlacklistID);
						}
					}
				}
			}
		}

		public static void save()
		{
			River river = ServerSavedata.openRiver("/Server/Blacklist.dat", false);
			river.writeByte(SteamBlacklist.SAVEDATA_VERSION);
			river.writeUInt16((ushort)SteamBlacklist.list.Count);
			ushort num = 0;
			while ((int)num < SteamBlacklist.list.Count)
			{
				SteamBlacklistID steamBlacklistID = SteamBlacklist.list[(int)num];
				river.writeSteamID(steamBlacklistID.playerID);
				river.writeUInt32(steamBlacklistID.ip);
				river.writeSteamID(steamBlacklistID.judgeID);
				river.writeString(steamBlacklistID.reason);
				river.writeUInt32(steamBlacklistID.duration);
				river.writeUInt32(steamBlacklistID.banned);
				num += 1;
			}
			river.closeRiver();
		}

		public static readonly byte SAVEDATA_VERSION = 3;

		public static readonly uint PERMANENT = 31536000u;

		public static readonly uint TEMPORARY = 180u;

		private static List<SteamBlacklistID> _list;
	}
}
