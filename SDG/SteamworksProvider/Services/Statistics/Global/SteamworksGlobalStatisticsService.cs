using System;
using SDG.Provider.Services;
using SDG.Provider.Services.Statistics.Global;
using Steamworks;

namespace SDG.SteamworksProvider.Services.Statistics.Global
{
	public class SteamworksGlobalStatisticsService : Service, IGlobalStatisticsService, IService
	{
		public SteamworksGlobalStatisticsService()
		{
			this.globalStatsReceived = Callback<GlobalStatsReceived_t>.Create(new Callback<GlobalStatsReceived_t>.DispatchDelegate(this.onGlobalStatsReceived));
		}

		public event GlobalStatisticsRequestReady onGlobalStatisticsRequestReady;

		private void triggerGlobalStatisticsRequestReady()
		{
			if (this.onGlobalStatisticsRequestReady != null)
			{
				this.onGlobalStatisticsRequestReady();
			}
		}

		public bool getStatistic(string name, out long data)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			return SteamUserStats.GetGlobalStat(name, ref data);
		}

		public bool getStatistic(string name, out double data)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			return SteamUserStats.GetGlobalStat(name, ref data);
		}

		public bool requestStatistics()
		{
			SteamUserStats.RequestGlobalStats(0);
			return true;
		}

		private void onGlobalStatsReceived(GlobalStatsReceived_t callback)
		{
			if (callback.m_nGameID != (ulong)SteamUtils.GetAppID().m_AppId)
			{
				return;
			}
			this.triggerGlobalStatisticsRequestReady();
		}

		private Callback<GlobalStatsReceived_t> globalStatsReceived;
	}
}
