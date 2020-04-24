using System;
using SDG.Provider.Services;
using SDG.Provider.Services.Browser;
using Steamworks;

namespace SDG.SteamworksProvider.Services.Browser
{
	public class SteamworksBrowserService : Service, IBrowserService, IService
	{
		public bool canOpenBrowser
		{
			get
			{
				return SteamUtils.IsOverlayEnabled();
			}
		}

		public void open(string url)
		{
			SteamFriends.ActivateGameOverlayToWebPage(url);
		}
	}
}
