using System;
using SDG.Provider;
using SDG.Provider.Services.Achievements;
using SDG.Provider.Services.Browser;
using SDG.Provider.Services.Cloud;
using SDG.Provider.Services.Community;
using SDG.Provider.Services.Multiplayer;
using SDG.Provider.Services.Statistics;
using SDG.Provider.Services.Store;
using SDG.Provider.Services.Translation;
using SDG.Provider.Services.Web;
using SDG.SteamworksProvider.Services.Achievements;
using SDG.SteamworksProvider.Services.Browser;
using SDG.SteamworksProvider.Services.Cloud;
using SDG.SteamworksProvider.Services.Community;
using SDG.SteamworksProvider.Services.Multiplayer;
using SDG.SteamworksProvider.Services.Statistics;
using SDG.SteamworksProvider.Services.Store;
using SDG.SteamworksProvider.Services.Translation;
using SDG.SteamworksProvider.Services.Web;
using Steamworks;

namespace SDG.SteamworksProvider
{
	public class SteamworksProvider : IProvider
	{
		public SteamworksProvider(SteamworksAppInfo newAppInfo)
		{
			this.appInfo = newAppInfo;
			this.constructServices();
		}

		public IAchievementsService achievementsService { get; protected set; }

		public IBrowserService browserService { get; protected set; }

		public ICloudService cloudService { get; protected set; }

		public ICommunityService communityService { get; protected set; }

		public TempSteamworksEconomy economyService { get; protected set; }

		public TempSteamworksMatchmaking matchmakingService { get; protected set; }

		public IMultiplayerService multiplayerService { get; protected set; }

		public IStatisticsService statisticsService { get; protected set; }

		public IStoreService storeService { get; protected set; }

		public ITranslationService translationService { get; protected set; }

		public IWebService webService { get; protected set; }

		public TempSteamworksWorkshop workshopService { get; protected set; }

		public void intialize()
		{
			if (!this.appInfo.isDedicated)
			{
				if (SteamAPI.RestartAppIfNecessary((AppId_t)this.appInfo.id))
				{
					throw new Exception("Restarting app from Steam.");
				}
				if (!SteamAPI.Init())
				{
					throw new Exception("Steam API initialization failed.");
				}
			}
			this.initializeServices();
		}

		public void update()
		{
			GameServer.RunCallbacks();
			if (!this.appInfo.isDedicated)
			{
				SteamAPI.RunCallbacks();
			}
			this.updateServices();
		}

		public void shutdown()
		{
			if (!this.appInfo.isDedicated)
			{
				SteamAPI.Shutdown();
			}
			this.shutdownServices();
		}

		private void constructServices()
		{
			this.achievementsService = new SteamworksAchievementsService();
			this.economyService = new TempSteamworksEconomy(this.appInfo);
			this.multiplayerService = new SteamworksMultiplayerService(this.appInfo);
			this.statisticsService = new SteamworksStatisticsService();
			this.webService = new SteamworksWebService();
			this.workshopService = new TempSteamworksWorkshop(this.appInfo);
			if (!this.appInfo.isDedicated)
			{
				this.browserService = new SteamworksBrowserService();
				this.cloudService = new SteamworksCloudService();
				this.communityService = new SteamworksCommunityService();
				this.matchmakingService = new TempSteamworksMatchmaking();
				this.storeService = new SteamworksStoreService(this.appInfo);
				this.translationService = new SteamworksTranslationService();
			}
		}

		private void initializeServices()
		{
			this.achievementsService.initialize();
			this.multiplayerService.initialize();
			this.statisticsService.initialize();
			this.webService.initialize();
			if (!this.appInfo.isDedicated)
			{
				this.browserService.initialize();
				this.cloudService.initialize();
				this.communityService.initialize();
				this.storeService.initialize();
				this.translationService.initialize();
			}
		}

		private void updateServices()
		{
			this.achievementsService.update();
			this.multiplayerService.update();
			this.statisticsService.update();
			this.webService.update();
			if (!this.appInfo.isDedicated)
			{
				this.browserService.update();
				this.cloudService.update();
				this.communityService.update();
				this.storeService.update();
				this.translationService.update();
			}
		}

		private void shutdownServices()
		{
			this.achievementsService.shutdown();
			this.multiplayerService.shutdown();
			this.statisticsService.shutdown();
			this.webService.shutdown();
			if (!this.appInfo.isDedicated)
			{
				this.browserService.shutdown();
				this.cloudService.shutdown();
				this.communityService.shutdown();
				this.storeService.shutdown();
				this.translationService.shutdown();
			}
		}

		private SteamworksAppInfo appInfo;
	}
}
