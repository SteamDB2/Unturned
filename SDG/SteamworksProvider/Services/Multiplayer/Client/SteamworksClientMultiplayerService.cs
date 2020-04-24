using System;
using System.IO;
using SDG.Provider.Services;
using SDG.Provider.Services.Community;
using SDG.Provider.Services.Multiplayer;
using SDG.Provider.Services.Multiplayer.Client;
using SDG.SteamworksProvider.Services.Community;
using Steamworks;

namespace SDG.SteamworksProvider.Services.Multiplayer.Client
{
	public class SteamworksClientMultiplayerService : Service, IClientMultiplayerService, IService
	{
		public SteamworksClientMultiplayerService()
		{
			this.buffer = new byte[1024];
			this.stream = new MemoryStream(this.buffer);
			this.reader = new BinaryReader(this.stream);
			this.writer = new BinaryWriter(this.stream);
			SteamworksClientMultiplayerService.p2pSessionRequest = Callback<P2PSessionRequest_t>.Create(new Callback<P2PSessionRequest_t>.DispatchDelegate(this.onP2PSessionRequest));
		}

		public IServerInfo serverInfo { get; protected set; }

		public bool isConnected { get; protected set; }

		public bool isAttempting { get; protected set; }

		public MemoryStream stream { get; protected set; }

		public BinaryReader reader { get; protected set; }

		public BinaryWriter writer { get; protected set; }

		public void connect(IServerInfo newServerInfo)
		{
			this.serverInfo = newServerInfo;
		}

		public void disconnect()
		{
		}

		public bool read(out ICommunityEntity entity, byte[] data, out ulong length, int channel)
		{
			entity = SteamworksCommunityEntity.INVALID;
			length = 0UL;
			uint num;
			if (!SteamNetworking.IsP2PPacketAvailable(ref num, channel) || (ulong)num > (ulong)((long)data.Length))
			{
				return false;
			}
			CSteamID newSteamID;
			if (!SteamNetworking.ReadP2PPacket(data, num, ref num, ref newSteamID, channel))
			{
				return false;
			}
			entity = new SteamworksCommunityEntity(newSteamID);
			length = (ulong)num;
			return true;
		}

		public void write(ICommunityEntity entity, byte[] data, ulong length)
		{
		}

		public void write(ICommunityEntity entity, byte[] data, ulong length, ESendMethod method, int channel)
		{
		}

		private void onP2PSessionRequest(P2PSessionRequest_t callback)
		{
			CSteamID steamIDRemote = callback.m_steamIDRemote;
			SteamNetworking.AcceptP2PSessionWithUser(steamIDRemote);
		}

		private byte[] buffer;

		private static Callback<P2PSessionRequest_t> p2pSessionRequest;
	}
}
