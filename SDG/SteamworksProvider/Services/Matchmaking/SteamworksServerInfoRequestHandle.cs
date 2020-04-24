using System;
using SDG.Provider.Services.Matchmaking;
using SDG.SteamworksProvider.Services.Multiplayer;
using Steamworks;

namespace SDG.SteamworksProvider.Services.Matchmaking
{
	public class SteamworksServerInfoRequestHandle : IServerInfoRequestHandle
	{
		public SteamworksServerInfoRequestHandle(ServerInfoRequestReadyCallback newCallback)
		{
			this.callback = newCallback;
		}

		public void onServerResponded(gameserveritem_t server)
		{
			SteamworksServerInfo newServerInfo = new SteamworksServerInfo(server);
			SteamworksServerInfoRequestResult result = new SteamworksServerInfoRequestResult(newServerInfo);
			this.triggerCallback(result);
			this.cleanupQuery();
			SteamworksMatchmakingService.serverInfoRequestHandles.Remove(this);
		}

		public void onServerFailedToRespond()
		{
			SteamworksServerInfoRequestResult result = new SteamworksServerInfoRequestResult(null);
			this.triggerCallback(result);
			this.cleanupQuery();
			SteamworksMatchmakingService.serverInfoRequestHandles.Remove(this);
		}

		public void triggerCallback(IServerInfoRequestResult result)
		{
			if (this.callback == null)
			{
				return;
			}
			this.callback(this, result);
		}

		private void cleanupQuery()
		{
			SteamMatchmakingServers.CancelServerQuery(this.query);
			this.query = HServerQuery.Invalid;
		}

		public HServerQuery query;

		public ISteamMatchmakingPingResponse pingResponse;

		private ServerInfoRequestReadyCallback callback;
	}
}
