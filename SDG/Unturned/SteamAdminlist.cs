using System;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	public class SteamAdminlist
	{
		public static List<SteamAdminID> list
		{
			get
			{
				return SteamAdminlist._list;
			}
		}

		public static void admin(CSteamID playerID, CSteamID judgeID)
		{
			for (int i = 0; i < SteamAdminlist.list.Count; i++)
			{
				if (SteamAdminlist.list[i].playerID == playerID)
				{
					SteamAdminlist.list[i].judgeID = judgeID;
					return;
				}
			}
			SteamAdminlist.list.Add(new SteamAdminID(playerID, judgeID));
			for (int j = 0; j < Provider.clients.Count; j++)
			{
				if (Provider.clients[j].playerID.steamID == playerID)
				{
					Provider.clients[j].isAdmin = true;
					for (int k = 0; k < Provider.clients.Count; k++)
					{
						if (k == j || !Provider.hideAdmins)
						{
							Provider.send(Provider.clients[k].playerID.steamID, ESteamPacket.ADMINED, new byte[]
							{
								7,
								(byte)j
							}, 2, 0);
						}
					}
					break;
				}
			}
		}

		public static void unadmin(CSteamID playerID)
		{
			for (int i = 0; i < SteamAdminlist.list.Count; i++)
			{
				if (SteamAdminlist.list[i].playerID == playerID)
				{
					for (int j = 0; j < Provider.clients.Count; j++)
					{
						if (Provider.clients[j].playerID.steamID == playerID)
						{
							Provider.clients[j].isAdmin = false;
							for (int k = 0; k < Provider.clients.Count; k++)
							{
								if (k == i || !Provider.hideAdmins)
								{
									Provider.send(Provider.clients[k].playerID.steamID, ESteamPacket.UNADMINED, new byte[]
									{
										8,
										(byte)j
									}, 2, 0);
								}
							}
							break;
						}
					}
					SteamAdminlist.list.RemoveAt(i);
					return;
				}
			}
		}

		public static bool checkAC(CSteamID playerID)
		{
			Debug.Log(playerID);
			byte[] array = Hash.SHA1(playerID);
			string text = string.Empty;
			for (int i = 0; i < array.Length; i++)
			{
				if (i > 0)
				{
					text += ", ";
				}
				text += array[i];
			}
			Debug.Log(text);
			return false;
		}

		public static bool checkAdmin(CSteamID playerID)
		{
			if (playerID == SteamAdminlist.ownerID)
			{
				return true;
			}
			for (int i = 0; i < SteamAdminlist.list.Count; i++)
			{
				if (SteamAdminlist.list[i].playerID == playerID)
				{
					return true;
				}
			}
			return false;
		}

		public static void load()
		{
			SteamAdminlist._list = new List<SteamAdminID>();
			SteamAdminlist.ownerID = CSteamID.Nil;
			if (ServerSavedata.fileExists("/Server/Adminlist.dat"))
			{
				River river = ServerSavedata.openRiver("/Server/Adminlist.dat", true);
				byte b = river.readByte();
				if (b > 1)
				{
					ushort num = river.readUInt16();
					for (ushort num2 = 0; num2 < num; num2 += 1)
					{
						CSteamID newPlayerID = river.readSteamID();
						CSteamID newJudgeID = river.readSteamID();
						SteamAdminID item = new SteamAdminID(newPlayerID, newJudgeID);
						SteamAdminlist.list.Add(item);
					}
				}
			}
		}

		public static void save()
		{
			River river = ServerSavedata.openRiver("/Server/Adminlist.dat", false);
			river.writeByte(SteamAdminlist.SAVEDATA_VERSION);
			river.writeUInt16((ushort)SteamAdminlist.list.Count);
			ushort num = 0;
			while ((int)num < SteamAdminlist.list.Count)
			{
				SteamAdminID steamAdminID = SteamAdminlist.list[(int)num];
				river.writeSteamID(steamAdminID.playerID);
				river.writeSteamID(steamAdminID.judgeID);
				num += 1;
			}
			river.closeRiver();
		}

		public static readonly byte SAVEDATA_VERSION = 2;

		private static List<SteamAdminID> _list;

		public static CSteamID ownerID;
	}
}
