using System;
using SDG.Framework.IO.Streams;
using SDG.Provider.Services.Economy;
using Steamworks;

namespace SDG.SteamworksProvider.Services.Economy
{
	public class SteamworksEconomyItem : IEconomyItem, INetworkStreamable
	{
		public SteamworksEconomyItem(SteamItemDetails_t newSteamItemDetail)
		{
			this.steamItemDetail = newSteamItemDetail;
			this.itemDefinitionID = new SteamworksEconomyItemDefinition(this.steamItemDetail.m_iDefinition);
			this.itemInstanceID = new SteamworksEconomyItemInstance(this.steamItemDetail.m_itemId);
		}

		public SteamItemDetails_t steamItemDetail { get; protected set; }

		public IEconomyItemDefinition itemDefinitionID { get; protected set; }

		public IEconomyItemInstance itemInstanceID { get; protected set; }

		public void readFromStream(NetworkStream networkStream)
		{
			this.itemDefinitionID.readFromStream(networkStream);
			this.itemInstanceID.readFromStream(networkStream);
		}

		public void writeToStream(NetworkStream networkStream)
		{
			this.itemDefinitionID.writeToStream(networkStream);
			this.itemInstanceID.writeToStream(networkStream);
		}
	}
}
