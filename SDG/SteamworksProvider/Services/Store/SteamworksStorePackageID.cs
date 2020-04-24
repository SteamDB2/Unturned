using System;
using SDG.Provider.Services.Store;
using Steamworks;

namespace SDG.SteamworksProvider.Services.Store
{
	public class SteamworksStorePackageID : IStorePackageID
	{
		public SteamworksStorePackageID(uint appID)
		{
			this.appID = new AppId_t(appID);
		}

		public AppId_t appID { get; protected set; }
	}
}
