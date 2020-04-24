using System;
using SDG.Framework.IO.Streams;
using SDG.Provider.Services.Economy;
using Steamworks;

namespace SDG.SteamworksProvider.Services.Economy
{
	public class SteamworksEconomyItemInstance : IEconomyItemInstance, INetworkStreamable
	{
		public SteamworksEconomyItemInstance(SteamItemInstanceID_t newSteamItemInstanceID)
		{
			this.steamItemInstanceID = newSteamItemInstanceID;
		}

		public SteamItemInstanceID_t steamItemInstanceID { get; protected set; }

		public void readFromStream(NetworkStream networkStream)
		{
			this.steamItemInstanceID = (SteamItemInstanceID_t)networkStream.readUInt64();
		}

		public void writeToStream(NetworkStream networkStream)
		{
			networkStream.writeUInt64((ulong)this.steamItemInstanceID);
		}
	}
}
