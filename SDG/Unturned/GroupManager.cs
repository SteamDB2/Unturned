using System;
using System.Collections.Generic;
using Steamworks;

namespace SDG.Unturned
{
	public class GroupManager : SteamCaller
	{
		public static GroupManager instance
		{
			get
			{
				return GroupManager.manager;
			}
		}

		public static event GroupInfoReadyHandler groupInfoReady;

		public static CSteamID generateUniqueGroupID()
		{
			CSteamID result = GroupManager.availableGroupID;
			GroupManager.availableGroupID = new CSteamID(result.m_SteamID + 1UL);
			return result;
		}

		public static GroupInfo addGroup(CSteamID groupID, string name)
		{
			GroupInfo groupInfo = new GroupInfo(groupID, name, 0u);
			GroupManager.knownGroups.Add(groupID, groupInfo);
			return groupInfo;
		}

		public static void removeGroup(CSteamID groupID)
		{
			GroupManager.knownGroups.Remove(groupID);
		}

		public static GroupInfo getGroupInfo(CSteamID groupID)
		{
			GroupInfo result = null;
			GroupManager.knownGroups.TryGetValue(groupID, out result);
			return result;
		}

		public static GroupInfo getOrAddGroup(CSteamID groupID, string name, out bool wasCreated)
		{
			wasCreated = false;
			GroupInfo groupInfo = GroupManager.getGroupInfo(groupID);
			if (groupInfo == null)
			{
				groupInfo = GroupManager.addGroup(groupID, name);
				wasCreated = true;
			}
			return groupInfo;
		}

		private static void triggerGroupInfoReady(GroupInfo group)
		{
			GroupInfoReadyHandler groupInfoReadyHandler = GroupManager.groupInfoReady;
			if (groupInfoReadyHandler != null)
			{
				groupInfoReadyHandler(group);
			}
		}

		public static void sendGroupInfo(CSteamID steamID, GroupInfo group)
		{
			GroupManager.manager.channel.send("tellGroupInfo", steamID, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
			{
				group.groupID,
				group.name,
				group.members
			});
		}

		public static void sendGroupInfo(GroupInfo group)
		{
			foreach (SteamPlayer steamPlayer in Provider.clients)
			{
				if (steamPlayer.player.quests.isMemberOfGroup(group.groupID))
				{
					GroupManager.sendGroupInfo(steamPlayer.playerID.steamID, group);
				}
			}
		}

		[SteamCall]
		public void tellGroupInfo(CSteamID steamID, CSteamID groupID, string name, uint members)
		{
			if (!base.channel.checkServer(steamID))
			{
				return;
			}
			GroupInfo groupInfo = GroupManager.getGroupInfo(groupID);
			if (groupInfo == null)
			{
				groupInfo = new GroupInfo(groupID, name, members);
				GroupManager.knownGroups.Add(groupInfo.groupID, groupInfo);
			}
			else
			{
				groupInfo.name = name;
				groupInfo.members = members;
			}
			GroupManager.triggerGroupInfoReady(groupInfo);
		}

		private void onLevelLoaded(int level)
		{
			if (level > Level.SETUP)
			{
				GroupManager.availableGroupID = new CSteamID(1UL);
				GroupManager.knownGroups = new Dictionary<CSteamID, GroupInfo>();
				if (Provider.isServer)
				{
					GroupManager.load();
				}
			}
		}

		private void Start()
		{
			GroupManager.manager = this;
			Level.onLevelLoaded = (LevelLoaded)Delegate.Combine(Level.onLevelLoaded, new LevelLoaded(this.onLevelLoaded));
		}

		public static void load()
		{
			if (LevelSavedata.fileExists("/Groups.dat"))
			{
				River river = LevelSavedata.openRiver("/Groups.dat", true);
				byte b = river.readByte();
				if (b > 0)
				{
					GroupManager.availableGroupID = river.readSteamID();
					if (b > 1)
					{
						int num = river.readInt32();
						for (int i = 0; i < num; i++)
						{
							CSteamID csteamID = river.readSteamID();
							string newName = river.readString();
							uint num2 = river.readUInt32();
							if (num2 > 0u)
							{
								GroupManager.knownGroups.Add(csteamID, new GroupInfo(csteamID, newName, num2));
							}
						}
					}
				}
			}
		}

		public static void save()
		{
			River river = LevelSavedata.openRiver("/Groups.dat", false);
			river.writeByte(GroupManager.SAVEDATA_VERSION);
			river.writeSteamID(GroupManager.availableGroupID);
			Dictionary<CSteamID, GroupInfo>.ValueCollection values = GroupManager.knownGroups.Values;
			river.writeInt32(values.Count);
			foreach (GroupInfo groupInfo in values)
			{
				river.writeSteamID(groupInfo.groupID);
				river.writeString(groupInfo.name);
				river.writeUInt32(groupInfo.members);
			}
		}

		public static readonly byte SAVEDATA_VERSION = 2;

		private static GroupManager manager;

		private static CSteamID availableGroupID;

		private static Dictionary<CSteamID, GroupInfo> knownGroups;
	}
}
