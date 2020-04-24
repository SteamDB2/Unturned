using System;
using System.IO;
using SDG.Provider.Services;
using SDG.Provider.Services.Community;
using SDG.Provider.Services.Multiplayer;
using SDG.Provider.Services.Multiplayer.Server;
using SDG.SteamworksProvider.Services.Community;
using Steamworks;

namespace SDG.SteamworksProvider.Services.Multiplayer.Server
{
	public class SteamworksServerMultiplayerService : Service, IServerMultiplayerService, IService
	{
		public SteamworksServerMultiplayerService(SteamworksAppInfo newAppInfo)
		{
			this.appInfo = newAppInfo;
			this.buffer = new byte[1024];
			this.stream = new MemoryStream(this.buffer);
			this.reader = new BinaryReader(this.stream);
			this.writer = new BinaryWriter(this.stream);
			SteamworksServerMultiplayerService.p2pSessionRequest = Callback<P2PSessionRequest_t>.CreateGameServer(new Callback<P2PSessionRequest_t>.DispatchDelegate(this.onP2PSessionRequest));
			SteamworksServerMultiplayerService.steamServersConnected = Callback<SteamServersConnected_t>.CreateGameServer(new Callback<SteamServersConnected_t>.DispatchDelegate(this.onSteamServersConnected));
		}

		public IServerInfo serverInfo { get; protected set; }

		public bool isHosting { get; protected set; }

		public MemoryStream stream { get; protected set; }

		public BinaryReader reader { get; protected set; }

		public BinaryWriter writer { get; protected set; }

		public event ServerMultiplayerServiceReadyHandler ready;

		public void open(uint ip, ushort port, ESecurityMode security)
		{
			if (this.isHosting)
			{
				return;
			}
			EServerMode eserverMode = 0;
			if (security != ESecurityMode.LAN)
			{
				if (security != ESecurityMode.SECURE)
				{
					if (security == ESecurityMode.INSECURE)
					{
						eserverMode = 2;
					}
				}
				else
				{
					eserverMode = 3;
				}
			}
			else
			{
				eserverMode = 1;
			}
			if (!GameServer.Init(ip, port + 2, port, port + 1, eserverMode, "1.0.0.0"))
			{
				throw new Exception("GameServer API initialization failed!");
			}
			SteamGameServer.SetDedicatedServer(this.appInfo.isDedicated);
			SteamGameServer.SetGameDescription(this.appInfo.name);
			SteamGameServer.SetProduct(this.appInfo.name);
			SteamGameServer.SetModDir(this.appInfo.name);
			SteamGameServer.LogOnAnonymous();
			SteamGameServer.EnableHeartbeats(true);
			this.isHosting = true;
		}

		public void close()
		{
			if (!this.isHosting)
			{
				return;
			}
			SteamGameServer.EnableHeartbeats(false);
			SteamGameServer.LogOff();
			GameServer.Shutdown();
			this.isHosting = false;
		}

		public bool read(out ICommunityEntity entity, byte[] data, out ulong length, int channel)
		{
			entity = SteamworksCommunityEntity.INVALID;
			length = 0UL;
			uint num;
			if (!SteamGameServerNetworking.IsP2PPacketAvailable(ref num, channel) || (ulong)num > (ulong)((long)data.Length))
			{
				return false;
			}
			CSteamID newSteamID;
			if (!SteamGameServerNetworking.ReadP2PPacket(data, num, ref num, ref newSteamID, channel))
			{
				return false;
			}
			entity = new SteamworksCommunityEntity(newSteamID);
			length = (ulong)num;
			return true;
		}

		public void write(ICommunityEntity entity, byte[] data, ulong length)
		{
			SteamworksCommunityEntity steamworksCommunityEntity = (SteamworksCommunityEntity)entity;
			CSteamID steamID = steamworksCommunityEntity.steamID;
			SteamGameServerNetworking.SendP2PPacket(steamID, data, (uint)length, 0, 0);
		}

		public void write(ICommunityEntity entity, byte[] data, ulong length, ESendMethod method, int channel)
		{
			SteamworksCommunityEntity steamworksCommunityEntity = (SteamworksCommunityEntity)entity;
			CSteamID steamID = steamworksCommunityEntity.steamID;
			switch (method)
			{
			case ESendMethod.RELIABLE:
				SteamGameServerNetworking.SendP2PPacket(steamID, data, (uint)length, 3, channel);
				return;
			case ESendMethod.RELIABLE_NODELAY:
				SteamGameServerNetworking.SendP2PPacket(steamID, data, (uint)length, 2, channel);
				return;
			case ESendMethod.UNRELIABLE:
				SteamGameServerNetworking.SendP2PPacket(steamID, data, (uint)length, 0, channel);
				return;
			case ESendMethod.UNRELIABLE_NODELAY:
				SteamGameServerNetworking.SendP2PPacket(steamID, data, (uint)length, 1, channel);
				return;
			default:
				return;
			}
		}

		private void onP2PSessionRequest(P2PSessionRequest_t callback)
		{
			CSteamID steamIDRemote = callback.m_steamIDRemote;
			SteamGameServerNetworking.AcceptP2PSessionWithUser(steamIDRemote);
		}

		private void onSteamServersConnected(SteamServersConnected_t callback)
		{
			if (this.ready != null)
			{
				this.ready();
			}
		}

		private byte[] buffer;

		private SteamworksAppInfo appInfo;

		private static Callback<P2PSessionRequest_t> p2pSessionRequest;

		private static Callback<SteamServersConnected_t> steamServersConnected;
	}
}
