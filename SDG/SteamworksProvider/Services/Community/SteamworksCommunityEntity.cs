using System;
using SDG.Framework.IO.Streams;
using SDG.Provider.Services.Community;
using Steamworks;

namespace SDG.SteamworksProvider.Services.Community
{
	public class SteamworksCommunityEntity : ICommunityEntity, INetworkStreamable
	{
		public SteamworksCommunityEntity(CSteamID newSteamID)
		{
			this.steamID = newSteamID;
		}

		public bool isValid
		{
			get
			{
				return this.steamID.IsValid();
			}
		}

		public CSteamID steamID { get; protected set; }

		public void readFromStream(NetworkStream networkStream)
		{
			this.steamID = (CSteamID)networkStream.readUInt64();
		}

		public void writeToStream(NetworkStream networkStream)
		{
			networkStream.writeUInt64((ulong)this.steamID);
		}

		public static readonly SteamworksCommunityEntity INVALID = new SteamworksCommunityEntity(CSteamID.Nil);
	}
}
