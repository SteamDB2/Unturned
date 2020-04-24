using System;
using SDG.Provider.Services.Matchmaking;
using SDG.Provider.Services.Multiplayer;
using SDG.SteamworksProvider.Services.Multiplayer;

namespace SDG.SteamworksProvider.Services.Matchmaking
{
	public class SteamworksServerInfoRequestResult : IServerInfoRequestResult
	{
		public SteamworksServerInfoRequestResult(SteamworksServerInfo newServerInfo)
		{
			this.serverInfo = newServerInfo;
		}

		public IServerInfo serverInfo { get; protected set; }
	}
}
