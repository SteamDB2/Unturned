using System;
using SDG.Provider.Services;
using SDG.Provider.Services.Community;
using SDG.Provider.Services.Statistics.User;
using SDG.SteamworksProvider.Services.Community;
using Steamworks;

namespace SDG.SteamworksProvider.Services.Statistics.User
{
	public class SteamworksUserStatisticsService : Service, IUserStatisticsService, IService
	{
		public SteamworksUserStatisticsService()
		{
			this.userStatsReceivedCallback = Callback<UserStatsReceived_t>.Create(new Callback<UserStatsReceived_t>.DispatchDelegate(this.onUserStatsReceived));
		}

		public event UserStatisticsRequestReady onUserStatisticsRequestReady;

		private void triggerUserStatisticsRequestReady(ICommunityEntity entityID)
		{
			if (this.onUserStatisticsRequestReady != null)
			{
				this.onUserStatisticsRequestReady(entityID);
			}
		}

		public bool getStatistic(string name, out int data)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			return SteamUserStats.GetStat(name, ref data);
		}

		public bool setStatistic(string name, int data)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			bool result = SteamUserStats.SetStat(name, data);
			SteamUserStats.StoreStats();
			return result;
		}

		public bool getStatistic(string name, out float data)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			return SteamUserStats.GetStat(name, ref data);
		}

		public bool setStatistic(string name, float data)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			bool result = SteamUserStats.SetStat(name, data);
			SteamUserStats.StoreStats();
			return result;
		}

		public bool requestStatistics()
		{
			SteamUserStats.RequestCurrentStats();
			return true;
		}

		private void onUserStatsReceived(UserStatsReceived_t callback)
		{
			if (callback.m_nGameID != (ulong)SteamUtils.GetAppID().m_AppId)
			{
				return;
			}
			SteamworksCommunityEntity entityID = new SteamworksCommunityEntity(callback.m_steamIDUser);
			this.triggerUserStatisticsRequestReady(entityID);
		}

		private Callback<UserStatsReceived_t> userStatsReceivedCallback;
	}
}
