using System;
using SDG.Provider.Services;
using SDG.Provider.Services.Achievements;
using Steamworks;

namespace SDG.SteamworksProvider.Services.Achievements
{
	public class SteamworksAchievementsService : Service, IAchievementsService, IService
	{
		public bool getAchievement(string name, out bool has)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			return SteamUserStats.GetAchievement(name, ref has);
		}

		public bool setAchievement(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			bool result = SteamUserStats.SetAchievement(name);
			SteamUserStats.StoreStats();
			return result;
		}
	}
}
