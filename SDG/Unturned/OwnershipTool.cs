using System;
using Steamworks;

namespace SDG.Unturned
{
	internal class OwnershipTool
	{
		public static bool checkToggle(ulong player, ulong group)
		{
			return !Dedicator.isDedicated && OwnershipTool.checkToggle(Provider.client, player, Player.player.quests.groupID, group);
		}

		public static bool checkToggle(CSteamID player_0, ulong player_1, CSteamID group_0, ulong group_1)
		{
			return (Provider.isServer && !Dedicator.isDedicated) || player_0.m_SteamID == player_1 || (group_0 != CSteamID.Nil && group_0.m_SteamID == group_1);
		}
	}
}
