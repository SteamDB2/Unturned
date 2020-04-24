using System;
using SDG.Provider.Services.Economy;
using Steamworks;

namespace SDG.SteamworksProvider.Services.Economy
{
	public class SteamworksEconomyRequestHandle : IEconomyRequestHandle
	{
		public SteamworksEconomyRequestHandle(SteamInventoryResult_t newSteamInventoryResult, EconomyRequestReadyCallback newEconomyRequestReadyCallback)
		{
			this.steamInventoryResult = newSteamInventoryResult;
			this.economyRequestReadyCallback = newEconomyRequestReadyCallback;
		}

		public SteamInventoryResult_t steamInventoryResult { get; protected set; }

		private EconomyRequestReadyCallback economyRequestReadyCallback { get; set; }

		public void triggerInventoryRequestReadyCallback(IEconomyRequestResult inventoryRequestResult)
		{
			if (this.economyRequestReadyCallback == null)
			{
				return;
			}
			this.economyRequestReadyCallback(this, inventoryRequestResult);
		}
	}
}
