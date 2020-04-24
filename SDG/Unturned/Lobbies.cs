using System;
using System.Runtime.CompilerServices;
using SDG.Framework.Debug;
using Steamworks;

namespace SDG.Unturned
{
	public static class Lobbies
	{
		static Lobbies()
		{
			if (Lobbies.<>f__mg$cache0 == null)
			{
				Lobbies.<>f__mg$cache0 = new CallResult<LobbyCreated_t>.APIDispatchDelegate(Lobbies.onLobbyCreated);
			}
			Lobbies.lobbyCreated = CallResult<LobbyCreated_t>.Create(Lobbies.<>f__mg$cache0);
			if (Lobbies.<>f__mg$cache1 == null)
			{
				Lobbies.<>f__mg$cache1 = new Callback<LobbyEnter_t>.DispatchDelegate(Lobbies.onLobbyEnter);
			}
			Lobbies.lobbyEnter = Callback<LobbyEnter_t>.Create(Lobbies.<>f__mg$cache1);
			if (Lobbies.<>f__mg$cache2 == null)
			{
				Lobbies.<>f__mg$cache2 = new Callback<GameLobbyJoinRequested_t>.DispatchDelegate(Lobbies.onGameLobbyJoinRequested);
			}
			Lobbies.gameLobbyJoinRequested = Callback<GameLobbyJoinRequested_t>.Create(Lobbies.<>f__mg$cache2);
			if (Lobbies.<>f__mg$cache3 == null)
			{
				Lobbies.<>f__mg$cache3 = new Callback<PersonaStateChange_t>.DispatchDelegate(Lobbies.onPersonaStateChange);
			}
			Lobbies.personaStateChange = Callback<PersonaStateChange_t>.Create(Lobbies.<>f__mg$cache3);
			if (Lobbies.<>f__mg$cache4 == null)
			{
				Lobbies.<>f__mg$cache4 = new Callback<LobbyGameCreated_t>.DispatchDelegate(Lobbies.onLobbyGameCreated);
			}
			Lobbies.lobbyGameCreated = Callback<LobbyGameCreated_t>.Create(Lobbies.<>f__mg$cache4);
			if (Lobbies.<>f__mg$cache5 == null)
			{
				Lobbies.<>f__mg$cache5 = new Callback<LobbyChatUpdate_t>.DispatchDelegate(Lobbies.onLobbyChatUpdate);
			}
			Lobbies.lobbyChatUpdate = Callback<LobbyChatUpdate_t>.Create(Lobbies.<>f__mg$cache5);
		}

		public static bool canOpenInvitations
		{
			get
			{
				return SteamUtils.IsOverlayEnabled();
			}
		}

		public static bool isHost { get; private set; }

		public static bool inLobby { get; private set; }

		public static CSteamID currentLobby { get; private set; }

		private static void onLobbyCreated(LobbyCreated_t callback, bool io)
		{
			Terminal.print(string.Concat(new object[]
			{
				"Lobby created: ",
				callback.m_eResult,
				" ",
				callback.m_ulSteamIDLobby,
				" ",
				io
			}), null, Provider.STEAM_IC, Provider.STEAM_DC, true);
			Lobbies.isHost = true;
		}

		private static void onLobbyEnter(LobbyEnter_t callback)
		{
			Terminal.print(string.Concat(new object[]
			{
				"Lobby entered: ",
				callback.m_bLocked,
				" ",
				callback.m_ulSteamIDLobby,
				" ",
				callback.m_EChatRoomEnterResponse,
				" ",
				callback.m_rgfChatPermissions
			}), null, Provider.STEAM_IC, Provider.STEAM_DC, true);
			Lobbies.inLobby = true;
			Lobbies.currentLobby = new CSteamID(callback.m_ulSteamIDLobby);
			Lobbies.triggerLobbiesRefreshed();
			Lobbies.triggerLobbiesEntered();
		}

		private static void onGameLobbyJoinRequested(GameLobbyJoinRequested_t callback)
		{
			Terminal.print(string.Concat(new object[]
			{
				"Lobby join requested: ",
				callback.m_steamIDLobby,
				" ",
				callback.m_steamIDFriend
			}), null, Provider.STEAM_IC, Provider.STEAM_DC, true);
			if (Provider.isConnected)
			{
				return;
			}
			Lobbies.joinLobby(callback.m_steamIDLobby);
		}

		private static void onPersonaStateChange(PersonaStateChange_t callback)
		{
			if (Lobbies.currentLobby == CSteamID.Nil)
			{
				return;
			}
			Lobbies.triggerLobbiesRefreshed();
		}

		private static void onLobbyGameCreated(LobbyGameCreated_t callback)
		{
			if (callback.m_ulSteamIDLobby != Lobbies.currentLobby.m_SteamID)
			{
				return;
			}
			Terminal.print(string.Concat(new object[]
			{
				"Lobby game created: ",
				callback.m_ulSteamIDLobby,
				" ",
				callback.m_unIP,
				" ",
				callback.m_usPort,
				" ",
				callback.m_ulSteamIDGameServer
			}), null, Provider.STEAM_IC, Provider.STEAM_DC, true);
			Provider.provider.matchmakingService.connect(new SteamConnectionInfo(callback.m_unIP, callback.m_usPort, string.Empty));
			Provider.provider.matchmakingService.autoJoinServerQuery = true;
		}

		private static void onLobbyChatUpdate(LobbyChatUpdate_t callback)
		{
			if (callback.m_ulSteamIDLobby != Lobbies.currentLobby.m_SteamID)
			{
				return;
			}
			Terminal.print(string.Concat(new object[]
			{
				"Lobby chat update: ",
				callback.m_ulSteamIDLobby,
				" ",
				callback.m_ulSteamIDMakingChange,
				" ",
				callback.m_ulSteamIDUserChanged,
				" ",
				callback.m_rgfChatMemberStateChange
			}), null, Provider.STEAM_IC, Provider.STEAM_DC, true);
			Lobbies.triggerLobbiesRefreshed();
		}

		private static void triggerLobbiesRefreshed()
		{
			Provider.updateRichPresence();
			LobbiesRefreshedHandler lobbiesRefreshedHandler = Lobbies.lobbiesRefreshed;
			if (lobbiesRefreshedHandler != null)
			{
				lobbiesRefreshedHandler();
			}
		}

		private static void triggerLobbiesEntered()
		{
			LobbiesEnteredHandler lobbiesEnteredHandler = Lobbies.lobbiesEntered;
			if (lobbiesEnteredHandler != null)
			{
				lobbiesEnteredHandler();
			}
		}

		public static void createLobby()
		{
			Terminal.print("Create lobby", null, Provider.STEAM_IC, Provider.STEAM_DC, true);
			SteamAPICall_t steamAPICall_t = SteamMatchmaking.CreateLobby(0, 24);
			Lobbies.lobbyCreated.Set(steamAPICall_t, null);
		}

		public static void joinLobby(CSteamID newLobby)
		{
			if (Lobbies.inLobby)
			{
				Lobbies.leaveLobby();
			}
			Terminal.print("Join lobby: " + newLobby, null, Provider.STEAM_IC, Provider.STEAM_DC, true);
			SteamMatchmaking.JoinLobby(newLobby);
		}

		public static void linkLobby(uint ip, ushort port)
		{
			if (!Lobbies.isHost)
			{
				return;
			}
			Terminal.print(string.Concat(new object[]
			{
				"Link lobby: ",
				ip,
				" ",
				port
			}), null, Provider.STEAM_IC, Provider.STEAM_DC, true);
			SteamMatchmaking.SetLobbyGameServer(Lobbies.currentLobby, ip, port, CSteamID.Nil);
		}

		public static void leaveLobby()
		{
			if (!Lobbies.inLobby)
			{
				return;
			}
			Terminal.print("Leave lobby", null, Provider.STEAM_IC, Provider.STEAM_DC, true);
			Lobbies.isHost = false;
			Lobbies.inLobby = false;
			SteamMatchmaking.LeaveLobby(Lobbies.currentLobby);
		}

		public static int getLobbyMemberCount()
		{
			return SteamMatchmaking.GetNumLobbyMembers(Lobbies.currentLobby);
		}

		public static CSteamID getLobbyMemberByIndex(int index)
		{
			return SteamMatchmaking.GetLobbyMemberByIndex(Lobbies.currentLobby, index);
		}

		public static void openInvitations()
		{
			SteamFriends.ActivateGameOverlayInviteDialog(Lobbies.currentLobby);
		}

		public static LobbiesRefreshedHandler lobbiesRefreshed;

		public static LobbiesEnteredHandler lobbiesEntered;

		private static CallResult<LobbyCreated_t> lobbyCreated;

		private static Callback<LobbyEnter_t> lobbyEnter;

		private static Callback<GameLobbyJoinRequested_t> gameLobbyJoinRequested;

		private static Callback<PersonaStateChange_t> personaStateChange;

		private static Callback<LobbyGameCreated_t> lobbyGameCreated;

		private static Callback<LobbyChatUpdate_t> lobbyChatUpdate;

		[CompilerGenerated]
		private static CallResult<LobbyCreated_t>.APIDispatchDelegate <>f__mg$cache0;

		[CompilerGenerated]
		private static Callback<LobbyEnter_t>.DispatchDelegate <>f__mg$cache1;

		[CompilerGenerated]
		private static Callback<GameLobbyJoinRequested_t>.DispatchDelegate <>f__mg$cache2;

		[CompilerGenerated]
		private static Callback<PersonaStateChange_t>.DispatchDelegate <>f__mg$cache3;

		[CompilerGenerated]
		private static Callback<LobbyGameCreated_t>.DispatchDelegate <>f__mg$cache4;

		[CompilerGenerated]
		private static Callback<LobbyChatUpdate_t>.DispatchDelegate <>f__mg$cache5;
	}
}
