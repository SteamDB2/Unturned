using System;
using SDG.Framework.IO.Streams;
using SDG.Provider.Services.Economy;
using Steamworks;

namespace SDG.SteamworksProvider.Services.Economy
{
	public class SteamworksEconomyItemDefinition : IEconomyItemDefinition, INetworkStreamable
	{
		public SteamworksEconomyItemDefinition(SteamItemDef_t newSteamItemDef)
		{
			this.steamItemDef = newSteamItemDef;
		}

		public SteamItemDef_t steamItemDef { get; protected set; }

		public void readFromStream(NetworkStream networkStream)
		{
			this.steamItemDef = (SteamItemDef_t)networkStream.readInt32();
		}

		public void writeToStream(NetworkStream networkStream)
		{
			networkStream.writeInt32((int)this.steamItemDef);
		}

		public string getPropertyValue(string key)
		{
			uint num = 1024u;
			string result;
			SteamInventory.GetItemDefinitionProperty(this.steamItemDef, key, ref result, ref num);
			return result;
		}
	}
}
