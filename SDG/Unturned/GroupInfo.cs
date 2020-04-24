using System;
using Steamworks;

namespace SDG.Unturned
{
	public class GroupInfo
	{
		public GroupInfo(CSteamID newGroupID, string newName, uint newMembers)
		{
			this.groupID = newGroupID;
			this.name = newName;
			this.members = newMembers;
		}

		public CSteamID groupID { get; private set; }

		public string name;

		public uint members;
	}
}
