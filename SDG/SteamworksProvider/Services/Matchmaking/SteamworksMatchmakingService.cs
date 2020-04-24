using System;
using System.Collections.Generic;
using SDG.Provider.Services;
using SDG.Provider.Services.Matchmaking;
using Steamworks;

namespace SDG.SteamworksProvider.Services.Matchmaking
{
	public class SteamworksMatchmakingService : Service, IMatchmakingService, IService
	{
		public IServerInfoRequestHandle requestServerInfo(uint ip, ushort port, ServerInfoRequestReadyCallback callback)
		{
			SteamworksServerInfoRequestHandle steamworksServerInfoRequestHandle = new SteamworksServerInfoRequestHandle(callback);
			ISteamMatchmakingPingResponse steamMatchmakingPingResponse = new ISteamMatchmakingPingResponse(new ISteamMatchmakingPingResponse.ServerResponded(steamworksServerInfoRequestHandle.onServerResponded), new ISteamMatchmakingPingResponse.ServerFailedToRespond(steamworksServerInfoRequestHandle.onServerFailedToRespond));
			steamworksServerInfoRequestHandle.pingResponse = steamMatchmakingPingResponse;
			HServerQuery query = SteamMatchmakingServers.PingServer(ip, port + 1, steamMatchmakingPingResponse);
			steamworksServerInfoRequestHandle.query = query;
			SteamworksMatchmakingService.serverInfoRequestHandles.Add(steamworksServerInfoRequestHandle);
			return steamworksServerInfoRequestHandle;
		}

		public static List<SteamworksServerInfoRequestHandle> serverInfoRequestHandles;
	}
}
