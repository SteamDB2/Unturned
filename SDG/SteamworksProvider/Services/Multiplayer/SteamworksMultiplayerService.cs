using System;
using SDG.Provider.Services;
using SDG.Provider.Services.Multiplayer;
using SDG.Provider.Services.Multiplayer.Client;
using SDG.Provider.Services.Multiplayer.Server;
using SDG.SteamworksProvider.Services.Multiplayer.Client;
using SDG.SteamworksProvider.Services.Multiplayer.Server;

namespace SDG.SteamworksProvider.Services.Multiplayer
{
	public class SteamworksMultiplayerService : IMultiplayerService, IService
	{
		public SteamworksMultiplayerService(SteamworksAppInfo newAppInfo)
		{
			this.appInfo = newAppInfo;
			this.serverMultiplayerService = new SteamworksServerMultiplayerService(this.appInfo);
			if (!this.appInfo.isDedicated)
			{
				this.clientMultiplayerService = new SteamworksClientMultiplayerService();
			}
		}

		public IClientMultiplayerService clientMultiplayerService { get; protected set; }

		public IServerMultiplayerService serverMultiplayerService { get; protected set; }

		public void initialize()
		{
			this.serverMultiplayerService.initialize();
			if (!this.appInfo.isDedicated)
			{
				this.clientMultiplayerService.initialize();
			}
		}

		public void update()
		{
			this.serverMultiplayerService.update();
			if (!this.appInfo.isDedicated)
			{
				this.clientMultiplayerService.update();
			}
		}

		public void shutdown()
		{
			this.serverMultiplayerService.shutdown();
			if (!this.appInfo.isDedicated)
			{
				this.clientMultiplayerService.shutdown();
			}
		}

		private SteamworksAppInfo appInfo;
	}
}
