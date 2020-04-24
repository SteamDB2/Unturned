using System;

namespace SDG.Provider.Services.Economy
{
	public class EconomyRequestResult : IEconomyRequestResult
	{
		public EconomyRequestResult(EEconomyRequestState newEconomyRequestState, IEconomyItem[] newItems)
		{
			this.economyRequestState = newEconomyRequestState;
			this.items = newItems;
		}

		public EEconomyRequestState economyRequestState { get; protected set; }

		public IEconomyItem[] items { get; protected set; }
	}
}
