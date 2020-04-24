using System;
using SDG.Provider.Services;
using SDG.Provider.Services.Economy;
using SDG.Provider.Services.Store;
using SDG.SteamworksProvider.Services.Economy;
using SDG.Unturned;
using Steamworks;

namespace SDG.SteamworksProvider.Services.Store
{
	public class SteamworksStoreService : Service, IStoreService, IService
	{
		public SteamworksStoreService(SteamworksAppInfo newAppInfo)
		{
			this.appInfo = newAppInfo;
		}

		public bool canOpenStore
		{
			get
			{
				return SteamUtils.IsOverlayEnabled();
			}
		}

		public void open(IStorePackageID packageID)
		{
			SteamworksStorePackageID steamworksStorePackageID = (SteamworksStorePackageID)packageID;
			AppId_t appID = steamworksStorePackageID.appID;
			SteamFriends.ActivateGameOverlayToStore(appID, 0);
		}

		public void open(IEconomyItemDefinition itemDefinitionID)
		{
			SteamworksEconomyItemDefinition steamworksEconomyItemDefinition = (SteamworksEconomyItemDefinition)itemDefinitionID;
			SteamItemDef_t steamItemDef = steamworksEconomyItemDefinition.steamItemDef;
			SteamFriends.ActivateGameOverlayToWebPage(string.Concat(new object[]
			{
				"http://store.steampowered.com/itemstore/",
				this.appInfo.id,
				"/detail/",
				steamItemDef
			}));
		}

		public void open()
		{
			if (Provider.statusData.Stockpile.Has_New_Items)
			{
				SteamFriends.ActivateGameOverlayToWebPage("http://store.steampowered.com/itemstore/" + this.appInfo.id + "/browse/?filter=New");
			}
			else
			{
				SteamFriends.ActivateGameOverlayToWebPage("http://store.steampowered.com/itemstore/" + this.appInfo.id);
			}
		}

		private SteamworksAppInfo appInfo;
	}
}
