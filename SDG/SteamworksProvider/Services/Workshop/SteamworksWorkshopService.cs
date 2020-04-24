using System;
using SDG.Provider.Services;
using SDG.Provider.Services.Workshop;
using Steamworks;

namespace SDG.SteamworksProvider.Services.Workshop
{
	public class SteamworksWorkshopService : Service, IWorkshopService, IService
	{
		public bool canOpenWorkshop
		{
			get
			{
				return SteamUtils.IsOverlayEnabled();
			}
		}

		public void open(PublishedFileId_t id)
		{
			SteamFriends.ActivateGameOverlayToWebPage("http://steamcommunity.com/sharedfiles/filedetails/?id=" + id.m_PublishedFileId);
		}
	}
}
