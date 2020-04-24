using System;
using SDG.Provider.Services.Community;
using SDG.Provider.Services.Multiplayer;
using SDG.SteamworksProvider.Services.Community;
using Steamworks;

namespace SDG.SteamworksProvider.Services.Multiplayer
{
	public class SteamworksServerInfo : IServerInfo
	{
		public SteamworksServerInfo(gameserveritem_t server)
		{
			this.entity = new SteamworksCommunityEntity(server.m_steamID);
			this.name = server.GetServerName();
			this.players = (byte)server.m_nPlayers;
			this.capacity = (byte)server.m_nMaxPlayers;
			this.ping = server.m_nPing;
		}

		public ICommunityEntity entity { get; protected set; }

		public string name { get; protected set; }

		public byte players { get; protected set; }

		public byte capacity { get; protected set; }

		public int ping { get; protected set; }
	}
}
