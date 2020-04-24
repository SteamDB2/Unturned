using System;
using System.Collections.Generic;

namespace SDG.Unturned
{
	public class SteamPlayerGroupAscendingComparator : IComparer<SteamPlayer>
	{
		public int Compare(SteamPlayer a, SteamPlayer b)
		{
			if (b.player.quests.groupID.m_SteamID > a.player.quests.groupID.m_SteamID)
			{
				return 1;
			}
			if (b.player.quests.groupID.m_SteamID < a.player.quests.groupID.m_SteamID)
			{
				return -1;
			}
			return 0;
		}
	}
}
