using System;
using SDG.Provider.Services;
using SDG.Provider.Services.Statistics;
using SDG.Provider.Services.Statistics.Global;
using SDG.Provider.Services.Statistics.User;
using SDG.SteamworksProvider.Services.Statistics.Global;
using SDG.SteamworksProvider.Services.Statistics.User;

namespace SDG.SteamworksProvider.Services.Statistics
{
	public class SteamworksStatisticsService : IStatisticsService, IService
	{
		public SteamworksStatisticsService()
		{
			this.userStatisticsService = new SteamworksUserStatisticsService();
			this.globalStatisticsService = new SteamworksGlobalStatisticsService();
		}

		public IUserStatisticsService userStatisticsService { get; protected set; }

		public IGlobalStatisticsService globalStatisticsService { get; protected set; }

		public void initialize()
		{
			this.userStatisticsService.initialize();
			this.globalStatisticsService.initialize();
		}

		public void update()
		{
			this.userStatisticsService.update();
			this.globalStatisticsService.update();
		}

		public void shutdown()
		{
			this.userStatisticsService.shutdown();
			this.globalStatisticsService.shutdown();
		}
	}
}
