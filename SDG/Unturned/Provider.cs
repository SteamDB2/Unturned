using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using BattlEye;
using SDG.Framework.Debug;
using SDG.Framework.IO.FormattedFiles;
using SDG.Framework.IO.FormattedFiles.KeyValueTables;
using SDG.Framework.Modules;
using SDG.Framework.Translations;
using SDG.Provider;
using SDG.Provider.Services.Community;
using SDG.Provider.Services.Multiplayer;
using SDG.Provider.Services.Multiplayer.Server;
using SDG.SteamworksProvider;
using SDG.SteamworksProvider.Services.Community;
using Steamworks;
using UnityEngine;
using UnityEngine.Analytics;

namespace SDG.Unturned
{
	public class Provider : MonoBehaviour
	{
		public static string APP_VERSION { get; protected set; }

		public static void takeScreenshot()
		{
			Application.CaptureScreenshot(ReadWrite.PATH + "/Screenshot.png", 2);
			ScreenshotHandle screenshotHandle = SteamScreenshots.AddScreenshotToLibrary(ReadWrite.PATH + "/Screenshot.png", null, Screen.width * 2, Screen.height * 2);
			Terminal.print("Screenshot handle: " + screenshotHandle, null, Provider.STEAM_IC, Provider.STEAM_DC, true);
			string text;
			if (Level.info != null)
			{
				Local local = Localization.tryRead(Level.info.path, false);
				if (local != null && local.has("Name"))
				{
					text = local.format("Name");
				}
				else
				{
					text = Level.info.name;
				}
			}
			else
			{
				text = "Misc";
			}
			SteamScreenshots.SetLocation(screenshotHandle, text);
			foreach (SteamPlayer steamPlayer in Provider.clients)
			{
				if (!(steamPlayer.player == null) && !steamPlayer.player.channel.isOwner)
				{
					Vector3 vector = MainCamera.instance.WorldToViewportPoint(steamPlayer.player.transform.position + Vector3.up);
					if (vector.x >= 0f && vector.x <= 1f && vector.y >= 0f && vector.y <= 1f)
					{
						SteamScreenshots.TagUser(screenshotHandle, steamPlayer.playerID.steamID);
					}
				}
			}
		}

		public static string language
		{
			get
			{
				return Provider._language;
			}
		}

		public static string path
		{
			get
			{
				return Provider._path;
			}
		}

		public static List<string> streamerNames { get; private set; }

		protected static void handleLanguageChanged(string oldLanguage, string newLanguage)
		{
			if (oldLanguage != "english")
			{
				Translator.unloadTranslations(oldLanguage);
			}
			if (newLanguage != "english")
			{
				Translator.loadTranslations(newLanguage);
			}
		}

		protected static void handleTranslationRegistered(string language, string ns)
		{
			if (Dedicator.isDedicated || Translator.isOriginLanguage(language) || Translator.isCurrentLanguage(language))
			{
				Translator.loadTranslation(language, ns);
			}
		}

		private static void battlEyeClientPrintMessage(string message)
		{
			Terminal.print(message, message, "BattlEye Client", "<color=yellow>BattlEye Client</color>", true);
		}

		private static void battlEyeClientRequestRestart(int reason)
		{
			if (reason == 0)
			{
				Provider._connectionFailureInfo = ESteamConnectionFailureInfo.BATTLEYE_BROKEN;
			}
			else if (reason == 1)
			{
				Provider._connectionFailureInfo = ESteamConnectionFailureInfo.BATTLEYE_UPDATE;
			}
			else
			{
				Provider._connectionFailureInfo = ESteamConnectionFailureInfo.BATTLEYE_UNKNOWN;
			}
			Provider.battlEyeHasRequiredRestart = true;
		}

		private static void battlEyeClientSendPacket(IntPtr packetHandle, int length)
		{
			Block.buffer[0] = 24;
			Marshal.Copy(packetHandle, Block.buffer, 1, length);
			Provider.send(Provider.server, ESteamPacket.BATTLEYE, Block.buffer, 1 + length, 0);
		}

		private static void battlEyeServerPrintMessage(string message)
		{
			for (int i = 0; i < Provider.clients.Count; i++)
			{
				SteamPlayer steamPlayer = Provider.clients[i];
				if (steamPlayer != null && !(steamPlayer.player == null))
				{
					if (steamPlayer.player.wantsBattlEyeLogs)
					{
						steamPlayer.player.sendTerminalRelay(message, "BattlEye Server", "<color=yellow>BattlEye Server</color>");
					}
				}
			}
			if (CommandWindow.shouldLogAnticheat)
			{
				CommandWindow.Log("BattlEye Server: " + message);
			}
		}

		private static void battlEyeServerKickPlayer(int playerID, string reason)
		{
			for (int i = 0; i < Provider.clients.Count; i++)
			{
				if (Provider.clients[i].channel == playerID)
				{
					if (reason.Length == 18 && reason.StartsWith("Global Ban #"))
					{
						ChatManager.say(Provider.clients[i].playerID.playerName + " got banned by BattlEye", Color.yellow);
					}
					Provider.kick(Provider.clients[i].playerID.steamID, "BattlEye: " + reason);
					return;
				}
			}
		}

		private static void battlEyeServerSendPacket(int playerID, IntPtr packetHandle, int length)
		{
			for (int i = 0; i < Provider.clients.Count; i++)
			{
				if (Provider.clients[i].channel == playerID)
				{
					Block.buffer[0] = 24;
					Marshal.Copy(packetHandle, Block.buffer, 1, length);
					Provider.send(Provider.clients[i].playerID.steamID, ESteamPacket.BATTLEYE, Block.buffer, 1 + length, Provider.clients[i].channel);
					return;
				}
			}
		}

		private static void handleDiscordReady()
		{
			Provider.isDiscordReady = true;
			Provider.updateRichPresence();
			Terminal.print("Ready", null, Provider.DISCORD_IC, Provider.DISCORD_DC, true);
		}

		private static void handleDiscordDisconnected(int errorCode, string message)
		{
			Terminal.print(string.Concat(new object[]
			{
				"Disconnected: [",
				errorCode,
				"] ",
				message
			}), null, Provider.DISCORD_IC, Provider.DISCORD_DC, true);
		}

		private static void handleDiscordError(int errorCode, string message)
		{
			Terminal.print(string.Concat(new object[]
			{
				"Error: [",
				errorCode,
				"] ",
				message
			}), null, Provider.DISCORD_IC, Provider.DISCORD_DC, true);
		}

		private static void handleDiscordJoin(string secret)
		{
			string text = Provider.discordDecryptSecret(secret);
			if (text.StartsWith("join_lobby"))
			{
				string[] array = text.Split(new char[]
				{
					':'
				});
				ulong num = ulong.Parse(array[1]);
				if (!Provider.isConnected)
				{
					Lobbies.joinLobby(new CSteamID(num));
				}
			}
			else if (text.StartsWith("join_server"))
			{
				string[] array2 = text.Split(new char[]
				{
					':'
				});
				uint newIP = uint.Parse(array2[1]);
				ushort newPort = ushort.Parse(array2[2]);
				if (!Provider.isConnected)
				{
					SteamConnectionInfo info = new SteamConnectionInfo(newIP, newPort, string.Empty);
					MenuPlayConnectUI.connect(info);
				}
			}
			Terminal.print("Join: " + secret + " " + text, null, Provider.DISCORD_IC, Provider.DISCORD_DC, true);
		}

		private static void handleDiscordSpectate(string secret)
		{
			Terminal.print("Spectate: " + secret, null, Provider.DISCORD_IC, Provider.DISCORD_DC, true);
		}

		public static void updateRichPresence()
		{
			if (Dedicator.isDedicated)
			{
				return;
			}
			Provider.updateSteamRichPresence();
			Provider.updateDiscordRichPresence();
		}

		private static void updateSteamRichPresence()
		{
			if (Level.info != null)
			{
				if (Level.isEditor)
				{
					Provider.provider.communityService.setStatus(Provider.localization.format("Rich_Presence_Editing", new object[]
					{
						Level.info.name
					}));
				}
				else
				{
					Provider.provider.communityService.setStatus(Provider.localization.format("Rich_Presence_Playing", new object[]
					{
						Level.info.name
					}));
				}
			}
			else if (Lobbies.inLobby)
			{
				Provider.provider.communityService.setStatus(Provider.localization.format("Rich_Presence_Lobby"));
			}
			else
			{
				Provider.provider.communityService.setStatus(Provider.localization.format("Rich_Presence_Menu"));
			}
		}

		private static void updateDiscordRichPresence()
		{
			if (!Provider.isDiscordReady)
			{
				return;
			}
			if (Level.info != null && Level.info.configData.Has_Discord_Rich_Presence)
			{
				Provider.discordRichPresence.largeImageKey = "map_" + Level.info.name.ToLower();
				Provider.discordRichPresence.largeImageText = Level.info.name;
			}
			else
			{
				Provider.discordRichPresence.largeImageKey = string.Empty;
				Provider.discordRichPresence.largeImageText = string.Empty;
			}
			if (Level.info != null)
			{
				Provider.discordRichPresence.details = Level.info.name;
				if (Level.isEditor)
				{
					Provider.discordRichPresence.state = "In the Editor";
				}
				else
				{
					Provider.discordRichPresence.state = "In a Game";
				}
			}
			else
			{
				Provider.discordRichPresence.details = "Menu";
				if (Lobbies.inLobby)
				{
					Provider.discordRichPresence.state = "In a Lobby";
				}
				else
				{
					Provider.discordRichPresence.state = "Hanging Around";
				}
			}
			string text;
			if (Lobbies.inLobby)
			{
				Provider.discordRichPresence.partyId = Lobbies.currentLobby.ToString();
				Provider.discordRichPresence.partySize = Lobbies.getLobbyMemberCount();
				Provider.discordRichPresence.partyMax = 24;
				text = "join_lobby:" + Lobbies.currentLobby.ToString();
			}
			else if (Provider.isConnected && !Provider.isServer)
			{
				Provider.discordRichPresence.partyId = Provider.server.ToString();
				Provider.discordRichPresence.partySize = Provider.currentServerInfo.players;
				Provider.discordRichPresence.partyMax = Provider.currentServerInfo.maxPlayers;
				text = string.Concat(new object[]
				{
					"join_server:",
					Provider.discordJoinIP,
					":",
					Provider.discordJoinPort
				});
			}
			else
			{
				Provider.discordRichPresence.partyId = string.Empty;
				Provider.discordRichPresence.partySize = 0;
				Provider.discordRichPresence.partyMax = 0;
				text = string.Empty;
			}
			Provider.discordRichPresence.joinSecret = ((!string.IsNullOrEmpty(text)) ? Provider.discordEncryptSecret(text) : string.Empty);
			Terminal.print("Rich Presence State: " + Provider.discordRichPresence.state, null, Provider.DISCORD_IC, Provider.DISCORD_DC, true);
			Terminal.print("Rich Presence Details: " + Provider.discordRichPresence.details, null, Provider.DISCORD_IC, Provider.DISCORD_DC, true);
			Terminal.print("Rich Presence Party ID: " + Provider.discordRichPresence.partyId, null, Provider.DISCORD_IC, Provider.DISCORD_DC, true);
			Terminal.print("Rich Presence Party Size: " + Provider.discordRichPresence.partySize, null, Provider.DISCORD_IC, Provider.DISCORD_DC, true);
			Terminal.print("Rich Presence Party Max: " + Provider.discordRichPresence.partyMax, null, Provider.DISCORD_IC, Provider.DISCORD_DC, true);
			Terminal.print("Rich Presence Join: " + text, null, Provider.DISCORD_IC, Provider.DISCORD_DC, true);
			Terminal.print("Rich Presence Join Secret: " + Provider.discordRichPresence.joinSecret, null, Provider.DISCORD_IC, Provider.DISCORD_DC, true);
			DiscordRpc.UpdatePresence(ref Provider.discordRichPresence);
		}

		private static string discordEncryptSecret(string plainText)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(plainText);
			byte[] bytes2 = new Rfc2898DeriveBytes(Provider.DISCORD_PASSWORD_HASH, Encoding.ASCII.GetBytes(Provider.DISCORD_SALT_KEY)).GetBytes(32);
			ICryptoTransform transform = new RijndaelManaged
			{
				Mode = CipherMode.CBC,
				Padding = PaddingMode.Zeros
			}.CreateEncryptor(bytes2, Encoding.ASCII.GetBytes(Provider.DISCORD_VI_KEY));
			byte[] inArray;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write))
				{
					cryptoStream.Write(bytes, 0, bytes.Length);
					cryptoStream.FlushFinalBlock();
					inArray = memoryStream.ToArray();
					cryptoStream.Close();
				}
				memoryStream.Close();
			}
			return Convert.ToBase64String(inArray);
		}

		private static string discordDecryptSecret(string encryptedText)
		{
			byte[] array = Convert.FromBase64String(encryptedText);
			byte[] bytes = new Rfc2898DeriveBytes(Provider.DISCORD_PASSWORD_HASH, Encoding.ASCII.GetBytes(Provider.DISCORD_SALT_KEY)).GetBytes(32);
			ICryptoTransform transform = new RijndaelManaged
			{
				Mode = CipherMode.CBC,
				Padding = PaddingMode.Zeros
			}.CreateDecryptor(bytes, Encoding.ASCII.GetBytes(Provider.DISCORD_VI_KEY));
			MemoryStream memoryStream = new MemoryStream(array);
			CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Read);
			byte[] array2 = new byte[array.Length];
			int count = cryptoStream.Read(array2, 0, array2.Length);
			memoryStream.Close();
			cryptoStream.Close();
			return Encoding.UTF8.GetString(array2, 0, count).TrimEnd("\0".ToCharArray());
		}

		public static uint bytesSent
		{
			get
			{
				return Provider._bytesSent;
			}
		}

		public static uint bytesReceived
		{
			get
			{
				return Provider._bytesReceived;
			}
		}

		public static uint packetsSent
		{
			get
			{
				return Provider._packetsSent;
			}
		}

		public static uint packetsReceived
		{
			get
			{
				return Provider._packetsReceived;
			}
		}

		public static SteamServerInfo currentServerInfo
		{
			get
			{
				return Provider._currentServerInfo;
			}
		}

		public static CSteamID server
		{
			get
			{
				return Provider._server;
			}
		}

		public static CSteamID client
		{
			get
			{
				return Provider._client;
			}
		}

		public static CSteamID user
		{
			get
			{
				return Provider._user;
			}
		}

		public static byte[] clientHash
		{
			get
			{
				return Provider._clientHash;
			}
		}

		public static string clientName
		{
			get
			{
				return Provider._clientName;
			}
		}

		public static List<SteamPlayer> clients
		{
			get
			{
				return Provider._clients;
			}
		}

		[Obsolete]
		public static List<SteamPlayer> players
		{
			get
			{
				return Provider.clients;
			}
		}

		public static bool isServer
		{
			get
			{
				return Provider._isServer;
			}
		}

		public static bool isClient
		{
			get
			{
				return Provider._isClient;
			}
		}

		public static bool isPro
		{
			get
			{
				return Provider._isPro;
			}
		}

		public static bool isConnected
		{
			get
			{
				return Provider._isConnected;
			}
		}

		public static bool isLoading
		{
			get
			{
				return Provider.isLoadingUGC;
			}
		}

		public static int channels
		{
			get
			{
				return Provider._channels;
			}
		}

		public static ESteamConnectionFailureInfo connectionFailureInfo
		{
			get
			{
				return Provider._connectionFailureInfo;
			}
			set
			{
				Provider._connectionFailureInfo = value;
			}
		}

		public static string connectionFailureReason
		{
			get
			{
				return Provider._connectionFailureReason;
			}
		}

		public static uint connectionFailureDuration
		{
			get
			{
				return Provider._connectionFailureDuration;
			}
		}

		public static List<SteamChannel> receivers
		{
			get
			{
				return Provider._receivers;
			}
		}

		public static void resetConnectionFailure()
		{
			Provider._connectionFailureInfo = ESteamConnectionFailureInfo.NONE;
			Provider._connectionFailureReason = string.Empty;
			Provider._connectionFailureDuration = 0u;
		}

		public static void openChannel(SteamChannel receiver)
		{
			if (Provider.receivers == null)
			{
				Provider.resetChannels();
				return;
			}
			Provider.receivers.Add(receiver);
			Provider._channels++;
		}

		public static void closeChannel(SteamChannel receiver)
		{
			for (int i = 0; i < Provider.receivers.Count; i++)
			{
				if (Provider.receivers[i].id == receiver.id)
				{
					Provider.receivers.RemoveAt(i);
					return;
				}
			}
		}

		private static void addPlayer(SteamPlayerID playerID, Vector3 point, byte angle, bool isPro, bool isAdmin, int channel, byte face, byte hair, byte beard, Color skin, Color color, bool hand, int shirtItem, int pantsItem, int hatItem, int backpackItem, int vestItem, int maskItem, int glassesItem, int[] skinItems, EPlayerSkillset skillset, string language, CSteamID lobbyID)
		{
			if (!Dedicator.isDedicated && playerID.steamID != Provider.client)
			{
				SteamFriends.SetPlayedWith(playerID.steamID);
			}
			if (playerID.steamID == Provider.client)
			{
				string value = skillset.ToString();
				int num = 0;
				int num2 = 0;
				if (shirtItem != 0)
				{
					num++;
					if (Provider.provider.economyService.getInventoryMythicID(shirtItem) != 0)
					{
						num2++;
					}
				}
				if (pantsItem != 0)
				{
					num++;
					if (Provider.provider.economyService.getInventoryMythicID(pantsItem) != 0)
					{
						num2++;
					}
				}
				if (hatItem != 0)
				{
					num++;
					if (Provider.provider.economyService.getInventoryMythicID(hatItem) != 0)
					{
						num2++;
					}
				}
				if (backpackItem != 0)
				{
					num++;
					if (Provider.provider.economyService.getInventoryMythicID(backpackItem) != 0)
					{
						num2++;
					}
				}
				if (vestItem != 0)
				{
					num++;
					if (Provider.provider.economyService.getInventoryMythicID(vestItem) != 0)
					{
						num2++;
					}
				}
				if (maskItem != 0)
				{
					num++;
					if (Provider.provider.economyService.getInventoryMythicID(maskItem) != 0)
					{
						num2++;
					}
				}
				if (glassesItem != 0)
				{
					num++;
					if (Provider.provider.economyService.getInventoryMythicID(glassesItem) != 0)
					{
						num2++;
					}
				}
				int num3 = skinItems.Length;
				for (int i = 0; i < skinItems.Length; i++)
				{
					if (Provider.provider.economyService.getInventoryMythicID(skinItems[i]) != 0)
					{
						num2++;
					}
				}
				Dictionary<string, object> dictionary = new Dictionary<string, object>
				{
					{
						"Ability",
						value
					},
					{
						"Cosmetics",
						num
					},
					{
						"Mythics",
						num2
					},
					{
						"Skins",
						num3
					}
				};
				Analytics.CustomEvent("Character", dictionary);
			}
			Transform transform = Provider.gameMode.getPlayerGameObject(playerID).transform;
			transform.position = point;
			transform.rotation = Quaternion.Euler(0f, (float)(angle * 2), 0f);
			Provider.clients.Add(new SteamPlayer(playerID, transform, isPro, isAdmin, channel, face, hair, beard, skin, color, hand, shirtItem, pantsItem, hatItem, backpackItem, vestItem, maskItem, glassesItem, skinItems, skillset, language, lobbyID));
			if (Provider.onEnemyConnected != null)
			{
				Provider.onEnemyConnected(Provider.clients[Provider.clients.Count - 1]);
			}
		}

		private static void removePlayer(byte index)
		{
			if (index < 0 || (int)index >= Provider.clients.Count)
			{
				Debug.LogError("Failed to find player: " + index);
				return;
			}
			if (Provider.battlEyeServerHandle != IntPtr.Zero && Provider.battlEyeServerRunData != null && Provider.battlEyeServerRunData.pfnChangePlayerStatus != null)
			{
				Provider.battlEyeServerRunData.pfnChangePlayerStatus.Invoke(Provider.clients[(int)index].channel, -1);
			}
			Provider.steam.StartCoroutine("close", Provider.clients[(int)index].playerID.steamID);
			if (Provider.onEnemyDisconnected != null)
			{
				Provider.onEnemyDisconnected(Provider.clients[(int)index]);
			}
			if (Provider.clients[(int)index].model != null)
			{
				Object.Destroy(Provider.clients[(int)index].model.gameObject);
			}
			Provider.clients.RemoveAt((int)index);
			if (Provider.pending.Count > 0 && Provider.clients.Count < (int)Provider.maxPlayers && Provider.pending[0].lastActive < 0f)
			{
				Provider.pending[0].lastActive = Time.realtimeSinceStartup;
				Provider.send(Provider.pending[0].playerID.steamID, ESteamPacket.VERIFY, new byte[]
				{
					3
				}, 1, 0);
			}
		}

		private static bool isInstant(ESteamPacket packet)
		{
			return packet == ESteamPacket.UPDATE_RELIABLE_INSTANT || packet == ESteamPacket.UPDATE_UNRELIABLE_INSTANT || packet == ESteamPacket.UPDATE_RELIABLE_INSTANT || packet == ESteamPacket.UPDATE_UNRELIABLE_INSTANT || packet == ESteamPacket.UPDATE_RELIABLE_CHUNK_INSTANT || packet == ESteamPacket.UPDATE_UNRELIABLE_CHUNK_INSTANT;
		}

		private static bool isUnreliable(ESteamPacket packet)
		{
			return packet == ESteamPacket.UPDATE_UNRELIABLE_BUFFER || packet == ESteamPacket.UPDATE_UNRELIABLE_INSTANT || packet == ESteamPacket.UPDATE_UNRELIABLE_CHUNK_BUFFER || packet == ESteamPacket.UPDATE_UNRELIABLE_CHUNK_INSTANT || packet == ESteamPacket.UPDATE_VOICE || packet == ESteamPacket.TICK || packet == ESteamPacket.TIME || packet == ESteamPacket.BATTLEYE;
		}

		public static bool isChunk(ESteamPacket packet)
		{
			return packet == ESteamPacket.UPDATE_UNRELIABLE_CHUNK_BUFFER || packet == ESteamPacket.UPDATE_RELIABLE_CHUNK_BUFFER || packet == ESteamPacket.UPDATE_UNRELIABLE_CHUNK_INSTANT || packet == ESteamPacket.UPDATE_RELIABLE_CHUNK_INSTANT;
		}

		private static bool isUpdate(ESteamPacket packet)
		{
			return packet == ESteamPacket.UPDATE_RELIABLE_BUFFER || packet == ESteamPacket.UPDATE_UNRELIABLE_BUFFER || packet == ESteamPacket.UPDATE_RELIABLE_INSTANT || packet == ESteamPacket.UPDATE_UNRELIABLE_INSTANT || packet == ESteamPacket.UPDATE_RELIABLE_CHUNK_BUFFER || packet == ESteamPacket.UPDATE_UNRELIABLE_CHUNK_BUFFER || packet == ESteamPacket.UPDATE_RELIABLE_CHUNK_INSTANT || packet == ESteamPacket.UPDATE_UNRELIABLE_CHUNK_INSTANT || packet == ESteamPacket.UPDATE_VOICE;
		}

		private static void resetChannels()
		{
			Provider._bytesSent = 0u;
			Provider._bytesReceived = 0u;
			Provider._packetsSent = 0u;
			Provider._packetsReceived = 0u;
			Provider._channels = 1;
			Provider._receivers = new List<SteamChannel>();
			SteamChannel[] array = Object.FindObjectsOfType<SteamChannel>();
			for (int i = 0; i < array.Length; i++)
			{
				Provider.openChannel(array[i]);
			}
			Provider._clients = new List<SteamPlayer>();
			Provider.pending = new List<SteamPending>();
		}

		private static void onLevelLoaded(int level)
		{
			if (level == 2)
			{
				Provider.isLoadingUGC = false;
				if (Provider.isConnected)
				{
					if (Provider.isServer)
					{
						if (Provider.isClient)
						{
							SteamPlayerID steamPlayerID = new SteamPlayerID(Provider.client, Characters.selected, Provider.clientName, Characters.active.name, Characters.active.nick, Characters.active.group);
							Vector3 point = Vector3.zero;
							byte angle;
							if (PlayerSavedata.fileExists(steamPlayerID, "/Player/Player.dat") && Level.info.type == ELevelType.SURVIVAL)
							{
								Block block = PlayerSavedata.readBlock(steamPlayerID, "/Player/Player.dat", 1);
								point = block.readSingleVector3() + new Vector3(0f, 0.5f, 0f);
								angle = block.readByte();
							}
							else
							{
								PlayerSpawnpoint spawn = LevelPlayers.getSpawn(false);
								point = spawn.point + new Vector3(0f, 0.5f, 0f);
								angle = (byte)(spawn.angle / 2f);
							}
							int inventoryItem = Provider.provider.economyService.getInventoryItem(Characters.active.packageShirt);
							int inventoryItem2 = Provider.provider.economyService.getInventoryItem(Characters.active.packagePants);
							int inventoryItem3 = Provider.provider.economyService.getInventoryItem(Characters.active.packageHat);
							int inventoryItem4 = Provider.provider.economyService.getInventoryItem(Characters.active.packageBackpack);
							int inventoryItem5 = Provider.provider.economyService.getInventoryItem(Characters.active.packageVest);
							int inventoryItem6 = Provider.provider.economyService.getInventoryItem(Characters.active.packageMask);
							int inventoryItem7 = Provider.provider.economyService.getInventoryItem(Characters.active.packageGlasses);
							int[] array = new int[Characters.packageSkins.Count];
							for (int i = 0; i < array.Length; i++)
							{
								array[i] = Provider.provider.economyService.getInventoryItem(Characters.packageSkins[i]);
							}
							Provider.addPlayer(steamPlayerID, point, angle, Provider.isPro, true, Provider.channels, Characters.active.face, Characters.active.hair, Characters.active.beard, Characters.active.skin, Characters.active.color, Characters.active.hand, inventoryItem, inventoryItem2, inventoryItem3, inventoryItem4, inventoryItem5, inventoryItem6, inventoryItem7, array, Characters.active.skillset, Translator.language, Lobbies.currentLobby);
							Lobbies.leaveLobby();
							Provider.updateRichPresence();
							if (Provider.onServerConnected != null)
							{
								Provider.onServerConnected(steamPlayerID.steamID);
							}
						}
					}
					else
					{
						byte b = 3;
						Provider.critMods.Clear();
						Provider.modBuilder.Length = 0;
						ModuleHook.getRequiredModules(Provider.critMods);
						for (int j = 0; j < Provider.critMods.Count; j++)
						{
							Provider.modBuilder.Append(Provider.critMods[j].config.Name);
							Provider.modBuilder.Append(",");
							Provider.modBuilder.Append(Provider.critMods[j].config.Version_Internal);
							if (j < Provider.critMods.Count - 1)
							{
								Provider.modBuilder.Append(";");
							}
						}
						int size;
						byte[] bytes = SteamPacker.getBytes(0, out size, new object[]
						{
							2,
							Characters.selected,
							Provider.clientName,
							Characters.active.name,
							Provider._serverPasswordHash,
							Level.hash,
							ReadWrite.appOut(),
							b,
							Provider.APP_VERSION,
							Provider.isPro,
							(float)Provider.currentServerInfo.ping / 1000f,
							Characters.active.nick,
							Characters.active.group,
							Characters.active.face,
							Characters.active.hair,
							Characters.active.beard,
							Characters.active.skin,
							Characters.active.color,
							Characters.active.hand,
							Characters.active.packageShirt,
							Characters.active.packagePants,
							Characters.active.packageHat,
							Characters.active.packageBackpack,
							Characters.active.packageVest,
							Characters.active.packageMask,
							Characters.active.packageGlasses,
							Characters.packageSkins.ToArray(),
							(byte)Characters.active.skillset,
							Provider.modBuilder.ToString(),
							Translator.language,
							Lobbies.currentLobby
						});
						Provider.send(Provider.server, ESteamPacket.CONNECT, bytes, size, 0);
					}
				}
			}
		}

		public static void connect(SteamServerInfo info, string password)
		{
			if (Provider.isConnected)
			{
				return;
			}
			Provider._currentServerInfo = info;
			Provider._isConnected = true;
			Provider.map = info.map;
			Provider.isPvP = info.isPvP;
			Provider.isWhitelisted = false;
			Provider.mode = info.mode;
			Provider.cameraMode = info.cameraMode;
			Provider.maxPlayers = (byte)info.maxPlayers;
			Provider.selectedGameModeName = info.gameMode;
			Provider._queuePosition = 0;
			Provider.resetChannels();
			GUIDTable.clear();
			Lobbies.linkLobby(info.ip, info.port);
			Provider._server = info.steamID;
			Provider._serverPassword = password;
			Provider._serverPasswordHash = Hash.SHA1(password);
			Provider._isClient = true;
			Provider.lastNet = Time.realtimeSinceStartup;
			Provider.offsetNet = 0f;
			Provider.pings = new float[4];
			Provider.lag((float)info.ping / 1000f);
			Provider.isTesting = true;
			Provider.isLoadingUGC = true;
			LoadingUI.updateScene();
			Provider.send(Provider.server, ESteamPacket.WORKSHOP, new byte[]
			{
				1
			}, 1, 0);
			List<SteamItemInstanceID_t> list = new List<SteamItemInstanceID_t>();
			if (Characters.active.packageShirt != 0UL)
			{
				list.Add((SteamItemInstanceID_t)Characters.active.packageShirt);
			}
			if (Characters.active.packagePants != 0UL)
			{
				list.Add((SteamItemInstanceID_t)Characters.active.packagePants);
			}
			if (Characters.active.packageHat != 0UL)
			{
				list.Add((SteamItemInstanceID_t)Characters.active.packageHat);
			}
			if (Characters.active.packageBackpack != 0UL)
			{
				list.Add((SteamItemInstanceID_t)Characters.active.packageBackpack);
			}
			if (Characters.active.packageVest != 0UL)
			{
				list.Add((SteamItemInstanceID_t)Characters.active.packageVest);
			}
			if (Characters.active.packageMask != 0UL)
			{
				list.Add((SteamItemInstanceID_t)Characters.active.packageMask);
			}
			if (Characters.active.packageGlasses != 0UL)
			{
				list.Add((SteamItemInstanceID_t)Characters.active.packageGlasses);
			}
			for (int i = 0; i < Characters.packageSkins.Count; i++)
			{
				ulong num = Characters.packageSkins[i];
				if (num != 0UL)
				{
					list.Add((SteamItemInstanceID_t)num);
				}
			}
			if (list.Count > 0)
			{
				SteamInventory.GetItemsByID(ref Provider.provider.economyService.wearingResult, list.ToArray(), (uint)list.Count);
			}
			Level.loading();
		}

		public static void launch()
		{
			if (!Level.exists(Provider.map))
			{
				Provider._connectionFailureInfo = ESteamConnectionFailureInfo.MAP;
				Provider.disconnect();
				return;
			}
			Level.load(Level.getLevel(Provider.map));
			Provider.loadGameMode();
		}

		private static void loadGameMode()
		{
			if (Level.info == null || Level.info.configData == null)
			{
				Provider.gameMode = new SurvivalGameMode();
				return;
			}
			LevelAsset levelAsset = Assets.find<LevelAsset>(Level.info.configData.Asset);
			if (levelAsset == null)
			{
				Provider.gameMode = new SurvivalGameMode();
				return;
			}
			Type type = levelAsset.defaultGameMode.type;
			if (!string.IsNullOrEmpty(Provider.selectedGameModeName))
			{
				foreach (TypeReference<GameMode> typeReference in levelAsset.supportedGameModes)
				{
					if (typeReference.assemblyQualifiedName.Contains(Provider.selectedGameModeName))
					{
						type = typeReference.type;
						break;
					}
				}
			}
			if (type == null)
			{
				Provider.gameMode = new SurvivalGameMode();
				return;
			}
			Provider.gameMode = (Activator.CreateInstance(type) as GameMode);
			if (Provider.gameMode == null)
			{
				Provider.gameMode = new SurvivalGameMode();
			}
		}

		private static void unloadGameMode()
		{
			Provider.gameMode = null;
			Provider.selectedGameModeName = null;
		}

		public static void singleplayer(EGameMode singleplayerMode, bool singleplayerCheats)
		{
			Provider._isConnected = true;
			Provider.resetChannels();
			GUIDTable.clear();
			Dedicator.serverVisibility = ESteamServerVisibility.LAN;
			Dedicator.serverID = "Singleplayer_" + Characters.selected;
			Commander.init();
			Provider.maxPlayers = 1;
			Provider.queueSize = 8;
			Provider.serverName = "Singleplayer #" + (int)(Characters.selected + 1);
			Provider.serverPassword = "Singleplayer";
			Provider.ip = 0u;
			Provider.port = 25000;
			Provider.lastNet = Time.realtimeSinceStartup;
			Provider.offsetNet = 0f;
			Provider.pings = new float[4];
			Provider.isPvP = true;
			Provider.isWhitelisted = false;
			Provider.hideAdmins = false;
			Provider.hasCheats = singleplayerCheats;
			Provider.filterName = false;
			Provider.mode = singleplayerMode;
			Provider.isGold = false;
			Provider.gameMode = null;
			Provider.selectedGameModeName = null;
			Provider.cameraMode = ECameraMode.BOTH;
			if (singleplayerMode != EGameMode.TUTORIAL)
			{
				PlayerInventory.skillsets = PlayerInventory.SKILLSETS_CLIENT;
			}
			Provider.lag(0f);
			SteamWhitelist.load();
			SteamBlacklist.load();
			SteamAdminlist.load();
			Provider._currentServerInfo = new SteamServerInfo(Provider.serverName, Provider.mode, false, false, false);
			if (ServerSavedata.fileExists("/Config.json"))
			{
				try
				{
					Provider._configData = ServerSavedata.deserializeJSON<ConfigData>("/Config.json");
				}
				catch
				{
					Provider._configData = null;
				}
				if (Provider.configData == null)
				{
					Provider._configData = new ConfigData();
				}
			}
			else
			{
				Provider._configData = new ConfigData();
			}
			switch (Provider.mode)
			{
			case EGameMode.EASY:
				Provider._modeConfigData = Provider.configData.Easy;
				break;
			case EGameMode.NORMAL:
				Provider._modeConfigData = Provider.configData.Normal;
				break;
			case EGameMode.HARD:
				Provider._modeConfigData = Provider.configData.Hard;
				break;
			default:
				Provider._modeConfigData = new ModeConfigData(Provider.mode);
				break;
			}
			Provider._time = SteamUtils.GetServerRealTime();
			Level.load(Level.getLevel(Provider.map));
			Provider.loadGameMode();
			Provider._server = Provider.user;
			Provider._client = Provider._server;
			Provider._clientHash = Hash.SHA1(Provider.client);
			Provider.lastNet = Time.realtimeSinceStartup;
			Provider.offsetNet = 0f;
			Provider._isServer = true;
			Provider._isClient = true;
			if (Provider.onServerHosted != null)
			{
				Provider.onServerHosted();
			}
		}

		public static void host()
		{
			Provider._isConnected = true;
			Provider.resetChannels();
			GUIDTable.clear();
			Provider.openGameServer();
			Provider._isServer = true;
			if (Provider.onServerHosted != null)
			{
				Provider.onServerHosted();
			}
		}

		public static void shutdown()
		{
			Provider.shutdown(0);
		}

		public static void shutdown(int timer)
		{
			Provider.countShutdownTimer = timer;
			Provider.lastTimerMessage = Time.realtimeSinceStartup;
		}

		public static void disconnect()
		{
			if (Provider.isServer)
			{
				if (Provider.configData != null && Provider.configData.Server.BattlEye_Secure && Provider.battlEyeServerHandle != IntPtr.Zero)
				{
					if (Provider.battlEyeServerRunData != null && Provider.battlEyeServerRunData.pfnExit != null)
					{
						Provider.battlEyeServerRunData.pfnExit.Invoke();
					}
					BEServer.dlclose(Provider.battlEyeServerHandle);
					Provider.battlEyeServerHandle = IntPtr.Zero;
				}
				if (Dedicator.isDedicated)
				{
					Provider.closeGameServer();
				}
				else if (Provider.onServerShutdown != null)
				{
					Provider.onServerShutdown();
				}
				if (Provider.isClient)
				{
					Provider._client = Provider.user;
					Provider._clientHash = Hash.SHA1(Provider.client);
				}
				Provider._isServer = false;
				Provider._isClient = false;
			}
			else if (Provider.isClient)
			{
				if (Provider.battlEyeClientHandle != IntPtr.Zero)
				{
					if (Provider.battlEyeClientRunData != null && Provider.battlEyeClientRunData.pfnExit != null)
					{
						Provider.battlEyeClientRunData.pfnExit.Invoke();
					}
					BEClient.dlclose(Provider.battlEyeClientHandle);
					Provider.battlEyeClientHandle = IntPtr.Zero;
				}
				SteamNetworking.CloseP2PSessionWithUser(Provider.server);
				for (int i = 0; i < Provider.clients.Count; i++)
				{
					SteamNetworking.CloseP2PSessionWithUser(Provider.clients[i].playerID.steamID);
				}
				SteamFriends.SetRichPresence("connect", string.Empty);
				Lobbies.leaveLobby();
				Provider.closeTicket();
				SteamUser.AdvertiseGame(CSteamID.Nil, 0u, 0);
				Provider._server = default(CSteamID);
				Provider._isServer = false;
				Provider._isClient = false;
			}
			if (Provider.onClientDisconnected != null)
			{
				Provider.onClientDisconnected();
			}
			Level.exit();
			Provider.unloadGameMode();
			Provider._isConnected = false;
			Provider.isTesting = false;
			Provider.isLoadingUGC = false;
			Provider.isLoadingInventory = true;
		}

		public static void sendGUIDTable(SteamPending player)
		{
			List<KeyValuePair<GUIDTableIndex, Guid>> list = GUIDTable.toList();
			if ((int)player.guidTableIndex < list.Count)
			{
				SteamPacker.block.reset(0);
				SteamPacker.block.writeByte(25);
				ushort num = (ushort)Mathf.Min(60, list.Count - (int)player.guidTableIndex);
				ushort num2 = player.guidTableIndex + num;
				SteamPacker.block.writeUInt16(num);
				while (player.guidTableIndex < num2)
				{
					SteamPacker.block.writeUInt16((ushort)list[(int)player.guidTableIndex].Key);
					GuidBuffer guidBuffer = new GuidBuffer(list[(int)player.guidTableIndex].Value);
					guidBuffer.Write(SteamPacker.block.block, SteamPacker.block.step);
					SteamPacker.block.step += 16;
					player.guidTableIndex += 1;
				}
				Provider.send(player.playerID.steamID, ESteamPacket.GUIDTABLE, SteamPacker.block.block, SteamPacker.block.step, 0);
			}
			else
			{
				Provider.accept(player);
			}
		}

		private static void onMappingAdded(GUIDTableIndex tableIndex, Guid GUID)
		{
			SteamPacker.block.reset(0);
			SteamPacker.block.writeByte(25);
			SteamPacker.block.writeUInt16(1);
			SteamPacker.block.writeUInt16((ushort)tableIndex);
			default(GuidBuffer).Write(SteamPacker.block.block, SteamPacker.block.step);
			SteamPacker.block.step += 16;
			for (int i = 0; i < Provider.clients.Count; i++)
			{
				Provider.send(Provider.clients[i].playerID.steamID, ESteamPacket.GUIDTABLE, SteamPacker.block.block, SteamPacker.block.step, 0);
			}
		}

		private static void handleServerReady()
		{
			if (Provider.isServerConnectedToSteam)
			{
				return;
			}
			Provider.isServerConnectedToSteam = true;
			CommandWindow.Log("Steam servers ready!");
			List<ulong> list;
			if (ServerSavedata.fileExists("/WorkshopDownloadIDs.json"))
			{
				try
				{
					list = ServerSavedata.deserializeJSON<List<ulong>>("/WorkshopDownloadIDs.json");
				}
				catch
				{
					list = null;
				}
				if (list == null)
				{
					list = new List<ulong>();
				}
			}
			else
			{
				list = new List<ulong>();
			}
			ServerSavedata.serializeJSON<List<ulong>>("/WorkshopDownloadIDs.json", list);
			DedicatedUGC.initialize();
			foreach (ulong id in list)
			{
				DedicatedUGC.registerItemInstallation(id);
			}
			CommandWindow.Log("Downloading " + list.Count + " workshop items...");
			DedicatedUGC.installNextItem();
		}

		private static void onDedicatedUGCInstalled()
		{
			if (Provider.isDedicatedUGCInstalled)
			{
				return;
			}
			Provider.isDedicatedUGCInstalled = true;
			Provider.apiWarningMessageHook = new SteamAPIWarningMessageHook_t(Provider.onAPIWarningMessage);
			SteamGameServerUtils.SetWarningMessageHook(Provider.apiWarningMessageHook);
			Provider._time = SteamGameServerUtils.GetServerRealTime();
			Level.load(Level.getLevel(Provider.map));
			Provider.loadGameMode();
			SteamGameServer.SetMaxPlayerCount((int)Provider.maxPlayers);
			SteamGameServer.SetServerName(Provider.serverName);
			SteamGameServer.SetPasswordProtected(Provider.serverPassword != string.Empty);
			SteamGameServer.SetMapName(Provider.map);
			if (Dedicator.isDedicated)
			{
				string[] folders = ReadWrite.getFolders("/Bundles/Workshop/Content");
				for (int i = 0; i < folders.Length; i++)
				{
					string s = ReadWrite.folderName(folders[i]);
					ulong item;
					if (ulong.TryParse(s, out item))
					{
						Provider.serverWorkshopFileIDs.Add(item);
					}
				}
				string[] folders2 = ReadWrite.getFolders(ServerSavedata.directory + "/" + Provider.serverID + "/Workshop/Content");
				for (int j = 0; j < folders2.Length; j++)
				{
					string s2 = ReadWrite.folderName(folders2[j]);
					ulong item2;
					if (ulong.TryParse(s2, out item2))
					{
						Provider.serverWorkshopFileIDs.Add(item2);
					}
				}
				string name = new DirectoryInfo(Level.info.path).Parent.Name;
				ulong item3;
				if (ulong.TryParse(name, out item3))
				{
					Provider.serverWorkshopFileIDs.Add(item3);
				}
				SteamGameServer.SetGameData(string.Concat(new string[]
				{
					(!(Provider.serverPassword != string.Empty)) ? "SSAP" : "PASS",
					",",
					(!Provider.configData.Server.VAC_Secure) ? "VAC_OFF" : "VAC_ON",
					",",
					Provider.APP_VERSION
				}));
				SteamGameServer.SetGameTags(string.Concat(new object[]
				{
					(!Provider.isPvP) ? "PVE" : "PVP",
					",GAMEMODE:",
					Provider.gameMode.GetType().Name,
					',',
					(!Provider.hasCheats) ? "STAEHC" : "CHEATS",
					',',
					Provider.mode.ToString(),
					",",
					Provider.cameraMode.ToString(),
					",",
					(Provider.serverWorkshopFileIDs.Count <= 0) ? "KROW" : "WORK",
					",",
					(!Provider.isGold) ? "YLNODLOG" : "GOLDONLY",
					",",
					(!Provider.configData.Server.BattlEye_Secure) ? "BATTLEYE_OFF" : "BATTLEYE_ON"
				}));
				SteamGameServer.SetKeyValue("Browser_Icon", Provider.configData.Browser.Icon);
				SteamGameServer.SetKeyValue("Browser_Desc_Hint", Provider.configData.Browser.Desc_Hint);
				int num = (Provider.configData.Browser.Desc_Full.Length - 1) / 120 + 1;
				int num2 = 0;
				SteamGameServer.SetKeyValue("Browser_Desc_Full_Count", num.ToString());
				for (int k = 0; k < Provider.configData.Browser.Desc_Full.Length; k += 120)
				{
					int num3 = 120;
					if (k + num3 > Provider.configData.Browser.Desc_Full.Length)
					{
						num3 = Provider.configData.Browser.Desc_Full.Length - k;
					}
					string text = Provider.configData.Browser.Desc_Full.Substring(k, num3);
					SteamGameServer.SetKeyValue("Browser_Desc_Full_Line_" + num2, text);
					num2++;
				}
				if (Provider.serverWorkshopFileIDs.Count > 0)
				{
					string text2 = string.Empty;
					for (int l = 0; l < Provider.serverWorkshopFileIDs.Count; l++)
					{
						if (text2.Length > 0)
						{
							text2 += ',';
						}
						text2 += Provider.serverWorkshopFileIDs[l];
					}
					int num4 = (text2.Length - 1) / 120 + 1;
					int num5 = 0;
					SteamGameServer.SetKeyValue("Browser_Workshop_Count", num4.ToString());
					for (int m = 0; m < text2.Length; m += 120)
					{
						int num6 = 120;
						if (m + num6 > text2.Length)
						{
							num6 = text2.Length - m;
						}
						string text3 = text2.Substring(m, num6);
						SteamGameServer.SetKeyValue("Browser_Workshop_Line_" + num5, text3);
						num5++;
					}
				}
				string text4 = string.Empty;
				Type type = Provider.modeConfigData.GetType();
				foreach (FieldInfo fieldInfo in type.GetFields())
				{
					object value = fieldInfo.GetValue(Provider.modeConfigData);
					Type type2 = value.GetType();
					foreach (FieldInfo fieldInfo2 in type2.GetFields())
					{
						object value2 = fieldInfo2.GetValue(value);
						if (text4.Length > 0)
						{
							text4 += ',';
						}
						if (value2 is bool)
						{
							text4 += ((!(bool)value2) ? "F" : "T");
						}
						else
						{
							text4 += value2;
						}
					}
				}
				int num8 = (text4.Length - 1) / 120 + 1;
				int num9 = 0;
				SteamGameServer.SetKeyValue("Browser_Config_Count", num8.ToString());
				for (int num10 = 0; num10 < text4.Length; num10 += 120)
				{
					int num11 = 120;
					if (num10 + num11 > text4.Length)
					{
						num11 = text4.Length - num10;
					}
					string text5 = text4.Substring(num10, num11);
					SteamGameServer.SetKeyValue("Browser_Config_Line_" + num9, text5);
					num9++;
				}
			}
			Provider._server = SteamGameServer.GetSteamID();
			Provider._client = Provider._server;
			Provider._clientHash = Hash.SHA1(Provider.client);
			if (Dedicator.isDedicated)
			{
				Provider._clientName = Provider.localization.format("Console");
			}
			Provider.lastNet = Time.realtimeSinceStartup;
			Provider.offsetNet = 0f;
		}

		public static void send(CSteamID steamID, ESteamPacket type, byte[] packet, int size, int channel)
		{
			if (!Provider.isConnected)
			{
				return;
			}
			Provider._bytesSent += (uint)size;
			Provider._packetsSent += 1u;
			if (Provider.isServer)
			{
				if (steamID == Provider.server || (Provider.isClient && steamID == Provider.client))
				{
					Provider.receiveServer(Provider.server, packet, 0, size, channel);
					return;
				}
				if (steamID.m_SteamID == 0UL)
				{
					Debug.LogError("Failed to send to invalid steam ID.");
					return;
				}
				if (Provider.isUnreliable(type))
				{
					if (!SteamGameServerNetworking.SendP2PPacket(steamID, packet, (uint)size, (!Provider.isInstant(type)) ? 0 : 1, channel))
					{
						Debug.LogError(string.Concat(new object[]
						{
							"Failed to send size ",
							size,
							" unreliable packet to ",
							steamID,
							"!"
						}));
					}
					return;
				}
				if (!SteamGameServerNetworking.SendP2PPacket(steamID, packet, (uint)size, (!Provider.isInstant(type)) ? 3 : 2, channel))
				{
					Debug.LogError(string.Concat(new object[]
					{
						"Failed to send size ",
						size,
						" reliable packet to ",
						steamID,
						"!"
					}));
				}
			}
			else
			{
				if (steamID == Provider.client)
				{
					Provider.receiveClient(Provider.client, packet, 0, size, channel);
					return;
				}
				if (steamID.m_SteamID == 0UL)
				{
					Debug.LogError("Failed to send to invalid steam ID.");
					return;
				}
				if (Provider.isUnreliable(type))
				{
					if (!SteamNetworking.SendP2PPacket(steamID, packet, (uint)size, (!Provider.isInstant(type)) ? 0 : 1, channel))
					{
						Debug.LogError(string.Concat(new object[]
						{
							"Failed to send size ",
							size,
							" unreliable packet to ",
							steamID,
							"!"
						}));
					}
					return;
				}
				if (!SteamNetworking.SendP2PPacket(steamID, packet, (uint)size, (!Provider.isInstant(type)) ? 3 : 2, channel))
				{
					Debug.LogError(string.Concat(new object[]
					{
						"Failed to send size ",
						size,
						" reliable packet to ",
						steamID,
						"!"
					}));
				}
			}
		}

		private static void receiveServer(CSteamID steamID, byte[] packet, int offset, int size, int channel)
		{
			Provider._bytesReceived += (uint)size;
			Provider._packetsReceived += 1u;
			if (!Dedicator.isDedicated)
			{
				return;
			}
			ESteamPacket esteamPacket = (ESteamPacket)packet[offset];
			if (Provider.isUpdate(esteamPacket))
			{
				if (steamID == Provider.server)
				{
					for (int i = 0; i < Provider.receivers.Count; i++)
					{
						if (Provider.receivers[i].id == channel)
						{
							Provider.receivers[i].receive(steamID, packet, offset, size);
							return;
						}
					}
					return;
				}
				for (int j = 0; j < Provider.clients.Count; j++)
				{
					if (Provider.clients[j].playerID.steamID == steamID)
					{
						for (int k = 0; k < Provider.receivers.Count; k++)
						{
							if (Provider.receivers[k].id == channel)
							{
								Provider.receivers[k].receive(steamID, packet, offset, size);
								return;
							}
						}
						return;
					}
				}
				return;
			}
			else
			{
				if (esteamPacket == ESteamPacket.WORKSHOP)
				{
					byte[] array = new byte[2 + Provider.serverWorkshopFileIDs.Count * 8];
					array[0] = 1;
					array[1] = (byte)Provider.serverWorkshopFileIDs.Count;
					byte b = 0;
					while ((int)b < Provider.serverWorkshopFileIDs.Count)
					{
						BitConverter.GetBytes(Provider.serverWorkshopFileIDs[(int)b]).CopyTo(array, (int)(2 + b * 8));
						b += 1;
					}
					Provider.send(steamID, ESteamPacket.WORKSHOP, array, array.Length, 0);
					return;
				}
				if (esteamPacket == ESteamPacket.TICK)
				{
					for (int l = 0; l < Provider.pending.Count; l++)
					{
						if (Provider.pending[l].playerID.steamID == steamID)
						{
							Provider.pending[l].lastNet = Time.realtimeSinceStartup;
							int size2;
							byte[] bytes = SteamPacker.getBytes(0, out size2, 14, Provider.net, (byte)l);
							Provider.send(steamID, ESteamPacket.TIME, bytes, size2, 0);
							return;
						}
					}
					for (int m = 0; m < Provider.clients.Count; m++)
					{
						if (Provider.clients[m].playerID.steamID == steamID)
						{
							int size3;
							byte[] bytes2 = SteamPacker.getBytes(0, out size3, 14, Provider.net);
							Provider.send(steamID, ESteamPacket.TIME, bytes2, size3, 0);
							return;
						}
					}
					return;
				}
				if (esteamPacket == ESteamPacket.TIME)
				{
					for (int n = 0; n < Provider.clients.Count; n++)
					{
						if (Provider.clients[n].playerID.steamID == steamID)
						{
							if (Provider.clients[n].lastPing > 0f)
							{
								Provider.clients[n].lastNet = Time.realtimeSinceStartup;
								Provider.clients[n].lag(Time.realtimeSinceStartup - Provider.clients[n].lastPing);
								Provider.clients[n].lastPing = -1f;
							}
							return;
						}
					}
					return;
				}
				if (esteamPacket == ESteamPacket.CONNECT)
				{
					for (int num = 0; num < Provider.pending.Count; num++)
					{
						if (Provider.pending[num].playerID.steamID == steamID)
						{
							Provider.reject(steamID, ESteamRejection.ALREADY_PENDING);
							return;
						}
					}
					for (int num2 = 0; num2 < Provider.clients.Count; num2++)
					{
						if (Provider.clients[num2].playerID.steamID == steamID)
						{
							Provider.reject(steamID, ESteamRejection.ALREADY_CONNECTED);
							return;
						}
					}
					object[] objects = SteamPacker.getObjects(steamID, offset, 0, packet, new Type[]
					{
						Types.BYTE_TYPE,
						Types.BYTE_TYPE,
						Types.STRING_TYPE,
						Types.STRING_TYPE,
						Types.BYTE_ARRAY_TYPE,
						Types.BYTE_ARRAY_TYPE,
						Types.BYTE_ARRAY_TYPE,
						Types.BYTE_TYPE,
						Types.STRING_TYPE,
						Types.BOOLEAN_TYPE,
						Types.SINGLE_TYPE,
						Types.STRING_TYPE,
						Types.STEAM_ID_TYPE,
						Types.BYTE_TYPE,
						Types.BYTE_TYPE,
						Types.BYTE_TYPE,
						Types.COLOR_TYPE,
						Types.COLOR_TYPE,
						Types.BOOLEAN_TYPE,
						Types.UINT64_TYPE,
						Types.UINT64_TYPE,
						Types.UINT64_TYPE,
						Types.UINT64_TYPE,
						Types.UINT64_TYPE,
						Types.UINT64_TYPE,
						Types.UINT64_TYPE,
						Types.UINT64_ARRAY_TYPE,
						Types.BYTE_TYPE,
						Types.STRING_TYPE,
						Types.STRING_TYPE,
						Types.STEAM_ID_TYPE
					});
					SteamPlayerID steamPlayerID = new SteamPlayerID(steamID, (byte)objects[1], (string)objects[2], (string)objects[3], (string)objects[11], (CSteamID)objects[12]);
					if ((string)objects[8] != Provider.APP_VERSION)
					{
						Provider.reject(steamID, ESteamRejection.WRONG_VERSION);
						return;
					}
					if (steamPlayerID.playerName.Length < 2)
					{
						Provider.reject(steamID, ESteamRejection.NAME_PLAYER_SHORT);
						return;
					}
					if (steamPlayerID.characterName.Length < 2)
					{
						Provider.reject(steamID, ESteamRejection.NAME_CHARACTER_SHORT);
						return;
					}
					if (steamPlayerID.playerName.Length > 32)
					{
						Provider.reject(steamID, ESteamRejection.NAME_PLAYER_LONG);
						return;
					}
					if (steamPlayerID.characterName.Length > 32)
					{
						Provider.reject(steamID, ESteamRejection.NAME_CHARACTER_LONG);
						return;
					}
					long num3;
					double num4;
					if (long.TryParse(steamPlayerID.playerName, out num3) || double.TryParse(steamPlayerID.playerName, out num4))
					{
						Provider.reject(steamID, ESteamRejection.NAME_PLAYER_NUMBER);
						return;
					}
					long num5;
					double num6;
					if (long.TryParse(steamPlayerID.characterName, out num5) || double.TryParse(steamPlayerID.characterName, out num6))
					{
						Provider.reject(steamID, ESteamRejection.NAME_CHARACTER_NUMBER);
						return;
					}
					if (Provider.filterName)
					{
						if (!NameTool.isValid(steamPlayerID.playerName))
						{
							Provider.reject(steamID, ESteamRejection.NAME_PLAYER_INVALID);
							return;
						}
						if (!NameTool.isValid(steamPlayerID.characterName))
						{
							Provider.reject(steamID, ESteamRejection.NAME_CHARACTER_INVALID);
							return;
						}
					}
					P2PSessionState_t p2PSessionState_t;
					uint num7;
					if (SteamGameServerNetworking.GetP2PSessionState(steamID, ref p2PSessionState_t))
					{
						num7 = p2PSessionState_t.m_nRemoteIP;
					}
					else
					{
						num7 = 0u;
					}
					SteamBlacklistID steamBlacklistID;
					if (SteamBlacklist.checkBanned(steamID, num7, out steamBlacklistID))
					{
						int size4;
						byte[] bytes3 = SteamPacker.getBytes(0, out size4, 9, steamBlacklistID.reason, steamBlacklistID.getTime());
						Provider.send(steamID, ESteamPacket.BANNED, bytes3, size4, 0);
						return;
					}
					bool flag = SteamWhitelist.checkWhitelisted(steamID);
					if (Provider.isWhitelisted && !flag)
					{
						Provider.reject(steamID, ESteamRejection.WHITELISTED);
						return;
					}
					if (Provider.clients.Count + 1 > (int)Provider.maxPlayers && Provider.pending.Count + 1 > (int)Provider.queueSize)
					{
						Provider.reject(steamID, ESteamRejection.SERVER_FULL);
						return;
					}
					byte[] array2 = (byte[])objects[4];
					if (array2.Length != 20)
					{
						Provider.reject(steamID, ESteamRejection.WRONG_PASSWORD);
						return;
					}
					byte[] array3 = (byte[])objects[5];
					if (array3.Length != 20)
					{
						Provider.reject(steamID, ESteamRejection.WRONG_HASH_LEVEL);
						return;
					}
					byte[] array4 = (byte[])objects[6];
					if (array4.Length != 20)
					{
						Provider.reject(steamID, ESteamRejection.WRONG_HASH_ASSEMBLY);
						return;
					}
					string text = (string)objects[28];
					ModuleDependency[] array5;
					if (string.IsNullOrEmpty(text))
					{
						array5 = new ModuleDependency[0];
					}
					else
					{
						string[] array6 = text.Split(new char[]
						{
							';'
						});
						array5 = new ModuleDependency[array6.Length];
						for (int num8 = 0; num8 < array5.Length; num8++)
						{
							string[] array7 = array6[num8].Split(new char[]
							{
								','
							});
							if (array7.Length == 2)
							{
								array5[num8] = new ModuleDependency();
								array5[num8].Name = array7[0];
								uint.TryParse(array7[1], out array5[num8].Version_Internal);
							}
						}
					}
					Provider.critMods.Clear();
					ModuleHook.getRequiredModules(Provider.critMods);
					bool flag2 = true;
					for (int num9 = 0; num9 < array5.Length; num9++)
					{
						bool flag3 = false;
						if (array5[num9] != null)
						{
							for (int num10 = 0; num10 < Provider.critMods.Count; num10++)
							{
								if (Provider.critMods[num10] != null && Provider.critMods[num10].config != null)
								{
									if (Provider.critMods[num10].config.Name == array5[num9].Name && Provider.critMods[num10].config.Version_Internal >= array5[num9].Version_Internal)
									{
										flag3 = true;
										break;
									}
								}
							}
						}
						if (!flag3)
						{
							flag2 = false;
							break;
						}
					}
					if (!flag2)
					{
						Provider.reject(steamID, ESteamRejection.CLIENT_MODULE_DESYNC);
						return;
					}
					bool flag4 = true;
					for (int num11 = 0; num11 < Provider.critMods.Count; num11++)
					{
						bool flag5 = false;
						if (Provider.critMods[num11] != null && Provider.critMods[num11].config != null)
						{
							for (int num12 = 0; num12 < array5.Length; num12++)
							{
								if (array5[num12] != null)
								{
									if (array5[num12].Name == Provider.critMods[num11].config.Name && array5[num12].Version_Internal >= Provider.critMods[num11].config.Version_Internal)
									{
										flag5 = true;
										break;
									}
								}
							}
						}
						if (!flag5)
						{
							flag4 = false;
							break;
						}
					}
					if (!flag4)
					{
						Provider.reject(steamID, ESteamRejection.SERVER_MODULE_DESYNC);
						return;
					}
					if (!(Provider.serverPassword == string.Empty) && !Hash.verifyHash(array2, Provider._serverPasswordHash))
					{
						Provider.reject(steamID, ESteamRejection.WRONG_PASSWORD);
						return;
					}
					if (!Hash.verifyHash(array3, Level.hash))
					{
						Provider.reject(steamID, ESteamRejection.WRONG_HASH_LEVEL);
						return;
					}
					if (!ReadWrite.appIn(array4, (byte)objects[7]))
					{
						Provider.reject(steamID, ESteamRejection.WRONG_HASH_ASSEMBLY);
						return;
					}
					if ((float)objects[10] < Provider.configData.Server.Max_Ping_Milliseconds / 1000f)
					{
						if (!Provider.isWhitelisted && flag)
						{
							if (Provider.pending.Count == 0)
							{
								Provider.pending.Add(new SteamPending(steamPlayerID, (bool)objects[9], (byte)objects[13], (byte)objects[14], (byte)objects[15], (Color)objects[16], (Color)objects[17], (bool)objects[18], (ulong)objects[19], (ulong)objects[20], (ulong)objects[21], (ulong)objects[22], (ulong)objects[23], (ulong)objects[24], (ulong)objects[25], (ulong[])objects[26], (EPlayerSkillset)((byte)objects[27]), (string)objects[29], (CSteamID)objects[30]));
								if (Provider.clients.Count < (int)Provider.maxPlayers && Provider.pending[0].lastActive < 0f)
								{
									Provider.pending[0].lastActive = Time.realtimeSinceStartup;
									Provider.send(steamID, ESteamPacket.VERIFY, new byte[]
									{
										3
									}, 1, 0);
								}
							}
							else
							{
								Provider.pending.Insert(1, new SteamPending(steamPlayerID, (bool)objects[9], (byte)objects[13], (byte)objects[14], (byte)objects[15], (Color)objects[16], (Color)objects[17], (bool)objects[18], (ulong)objects[19], (ulong)objects[20], (ulong)objects[21], (ulong)objects[22], (ulong)objects[23], (ulong)objects[24], (ulong)objects[25], (ulong[])objects[26], (EPlayerSkillset)((byte)objects[27]), (string)objects[29], (CSteamID)objects[30]));
							}
						}
						else
						{
							Provider.pending.Add(new SteamPending(steamPlayerID, (bool)objects[9], (byte)objects[13], (byte)objects[14], (byte)objects[15], (Color)objects[16], (Color)objects[17], (bool)objects[18], (ulong)objects[19], (ulong)objects[20], (ulong)objects[21], (ulong)objects[22], (ulong)objects[23], (ulong)objects[24], (ulong)objects[25], (ulong[])objects[26], (EPlayerSkillset)((byte)objects[27]), (string)objects[29], (CSteamID)objects[30]));
							if (Provider.pending.Count == 1 && Provider.clients.Count < (int)Provider.maxPlayers && Provider.pending[0].lastActive < 0f)
							{
								Provider.pending[0].lastActive = Time.realtimeSinceStartup;
								Provider.send(steamID, ESteamPacket.VERIFY, new byte[]
								{
									3
								}, 1, 0);
							}
						}
						return;
					}
					Provider.reject(steamID, ESteamRejection.PING);
					return;
				}
				else if (esteamPacket == ESteamPacket.AUTHENTICATE)
				{
					SteamPending steamPending = null;
					for (int num13 = 0; num13 < Provider.pending.Count; num13++)
					{
						if (Provider.pending[num13].playerID.steamID == steamID)
						{
							steamPending = Provider.pending[num13];
							break;
						}
					}
					if (steamPending == null)
					{
						Provider.reject(steamID, ESteamRejection.NOT_PENDING);
						return;
					}
					ushort num14 = BitConverter.ToUInt16(packet, 1);
					byte[] array8 = new byte[(int)num14];
					Buffer.BlockCopy(packet, 3, array8, 0, (int)num14);
					ushort num15 = BitConverter.ToUInt16(packet, (int)(3 + num14));
					byte[] array9 = new byte[(int)num15];
					Buffer.BlockCopy(packet, (int)(5 + num14), array9, 0, (int)num15);
					if (!Provider.verifyTicket(steamID, array8))
					{
						Provider.reject(steamID, ESteamRejection.AUTH_VERIFICATION);
						return;
					}
					if (steamPending.playerID.group == CSteamID.Nil)
					{
						steamPending.hasGroup = true;
					}
					else if (!SteamGameServer.RequestUserGroupStatus(steamPending.playerID.steamID, steamPending.playerID.group))
					{
						steamPending.playerID.group = CSteamID.Nil;
						steamPending.hasGroup = true;
					}
					int num16 = (int)(num15 / 12);
					SteamItemDetails_t[] array10 = new SteamItemDetails_t[num16];
					for (int num17 = 0; num17 < num16; num17++)
					{
						array10[num17].m_itemId.m_SteamItemInstanceID = BitConverter.ToUInt64(array9, num17 * 12);
						array10[num17].m_iDefinition.m_SteamItemDef = BitConverter.ToInt32(array9, num17 * 12 + 8);
					}
					steamPending.inventoryResult = SteamInventoryResult_t.Invalid;
					steamPending.inventoryDetails = array10;
					steamPending.inventoryDetailsReady();
					return;
				}
				else
				{
					if (esteamPacket == ESteamPacket.BATTLEYE)
					{
						if (Provider.battlEyeServerHandle != IntPtr.Zero && Provider.battlEyeServerRunData != null && Provider.battlEyeServerRunData.pfnReceivedPacket != null)
						{
							for (int num18 = 0; num18 < Provider.clients.Count; num18++)
							{
								if (Provider.clients[num18].playerID.steamID == steamID)
								{
									GCHandle gchandle = GCHandle.Alloc(packet, GCHandleType.Pinned);
									IntPtr intPtr = gchandle.AddrOfPinnedObject();
									if (IntPtr.Size == 4)
									{
										intPtr = new IntPtr(intPtr.ToInt32() + offset + 1);
									}
									else
									{
										intPtr = new IntPtr(intPtr.ToInt64() + (long)offset + 1L);
									}
									Provider.battlEyeServerRunData.pfnReceivedPacket.Invoke(Provider.clients[num18].channel, intPtr, size - offset - 1);
									gchandle.Free();
								}
							}
						}
						return;
					}
					Debug.LogError("Failed to handle message: " + esteamPacket);
					return;
				}
			}
		}

		private static void receiveClient(CSteamID steamID, byte[] packet, int offset, int size, int channel)
		{
			Provider._bytesReceived += (uint)size;
			Provider._packetsReceived += 1u;
			ESteamPacket esteamPacket = (ESteamPacket)packet[offset];
			if (Provider.isUpdate(esteamPacket))
			{
				for (int i = 0; i < Provider.receivers.Count; i++)
				{
					if (Provider.receivers[i].id == channel)
					{
						Provider.receivers[i].receive(steamID, packet, offset, size);
						return;
					}
				}
				return;
			}
			if (steamID == Provider.server)
			{
				if (esteamPacket == ESteamPacket.TICK)
				{
					Provider.send(Provider.server, ESteamPacket.TIME, new byte[]
					{
						14
					}, 1, 0);
					return;
				}
				if (esteamPacket == ESteamPacket.TIME)
				{
					if (Provider.lastPing > 0f)
					{
						object[] objects = SteamPacker.getObjects(steamID, offset, 0, packet, Types.BYTE_TYPE, Types.SINGLE_TYPE, Types.BYTE_TYPE);
						Provider.lastNet = Time.realtimeSinceStartup;
						Provider.offsetNet = (float)objects[1] + (Time.realtimeSinceStartup - Provider.lastPing) / 2f;
						if (Player.player == null)
						{
							Provider._queuePosition = (byte)objects[2];
							if (Provider.onQueuePositionUpdated != null)
							{
								Provider.onQueuePositionUpdated();
							}
						}
						Provider.lag(Time.realtimeSinceStartup - Provider.lastPing);
						Provider.lastPing = -1f;
					}
					return;
				}
				if (esteamPacket == ESteamPacket.SHUTDOWN)
				{
					Provider._connectionFailureInfo = ESteamConnectionFailureInfo.SHUTDOWN;
					Provider.disconnect();
					return;
				}
				if (esteamPacket == ESteamPacket.CONNECTED)
				{
					object[] objects2 = SteamPacker.getObjects(steamID, offset, 0, packet, new Type[]
					{
						Types.BYTE_TYPE,
						Types.STEAM_ID_TYPE,
						Types.BYTE_TYPE,
						Types.STRING_TYPE,
						Types.STRING_TYPE,
						Types.VECTOR3_TYPE,
						Types.BYTE_TYPE,
						Types.BOOLEAN_TYPE,
						Types.BOOLEAN_TYPE,
						Types.INT32_TYPE,
						Types.STEAM_ID_TYPE,
						Types.STRING_TYPE,
						Types.BYTE_TYPE,
						Types.BYTE_TYPE,
						Types.BYTE_TYPE,
						Types.COLOR_TYPE,
						Types.COLOR_TYPE,
						Types.BOOLEAN_TYPE,
						Types.INT32_TYPE,
						Types.INT32_TYPE,
						Types.INT32_TYPE,
						Types.INT32_TYPE,
						Types.INT32_TYPE,
						Types.INT32_TYPE,
						Types.INT32_TYPE,
						Types.INT32_ARRAY_TYPE,
						Types.BYTE_TYPE,
						Types.STRING_TYPE
					});
					Provider.addPlayer(new SteamPlayerID((CSteamID)objects2[1], (byte)objects2[2], (string)objects2[3], (string)objects2[4], (string)objects2[11], (CSteamID)objects2[10]), (Vector3)objects2[5], (byte)objects2[6], (bool)objects2[7], (bool)objects2[8], (int)objects2[9], (byte)objects2[12], (byte)objects2[13], (byte)objects2[14], (Color)objects2[15], (Color)objects2[16], (bool)objects2[17], (int)objects2[18], (int)objects2[19], (int)objects2[20], (int)objects2[21], (int)objects2[22], (int)objects2[23], (int)objects2[24], (int[])objects2[25], (EPlayerSkillset)((byte)objects2[26]), (string)objects2[27], CSteamID.Nil);
					return;
				}
				if (esteamPacket == ESteamPacket.DISCONNECTED)
				{
					Provider.removePlayer(packet[offset + 1]);
					return;
				}
				if (esteamPacket == ESteamPacket.WORKSHOP)
				{
					Provider.isTesting = false;
					Provider.provider.workshopService.installing = new List<PublishedFileId_t>();
					byte b = packet[offset + 1];
					for (byte b2 = 0; b2 < b; b2 += 1)
					{
						ulong num = BitConverter.ToUInt64(packet, offset + 2 + (int)(b2 * 8));
						PublishedFileId_t file = new PublishedFileId_t(num);
						ulong num2;
						string text;
						uint num3;
						if (SteamUGC.GetItemInstallInfo(file, ref num2, ref text, 1024u, ref num3))
						{
							if (Provider.provider.workshopService.ugc.Find((SteamContent x) => x.publishedFileID == file) == null)
							{
								Provider.provider.workshopService.installing.Add(file);
							}
						}
						else
						{
							Provider.provider.workshopService.installing.Add(file);
						}
					}
					Provider.provider.workshopService.installed = Provider.provider.workshopService.installing.Count;
					if (Provider.provider.workshopService.installed == 0)
					{
						Provider.launch();
					}
					else
					{
						SteamUGC.DownloadItem(Provider.provider.workshopService.installing[0], true);
					}
					return;
				}
				if (esteamPacket == ESteamPacket.VERIFY)
				{
					byte[] array = Provider.openTicket();
					if (array == null)
					{
						Provider._connectionFailureInfo = ESteamConnectionFailureInfo.AUTH_EMPTY;
						Provider.disconnect();
						return;
					}
					byte[] array2;
					uint num4;
					if (Provider.provider.economyService.wearingResult == SteamInventoryResult_t.Invalid)
					{
						array2 = new byte[0];
						num4 = 0u;
					}
					else
					{
						uint num5 = 0u;
						SteamItemDetails_t[] array3;
						if (SteamInventory.GetResultItems(Provider.provider.economyService.wearingResult, null, ref num5) && num5 > 0u)
						{
							array3 = new SteamItemDetails_t[num5];
							SteamInventory.GetResultItems(Provider.provider.economyService.wearingResult, array3, ref num5);
						}
						else
						{
							array3 = new SteamItemDetails_t[num5];
						}
						SteamInventory.DestroyResult(Provider.provider.economyService.wearingResult);
						Provider.provider.economyService.wearingResult = SteamInventoryResult_t.Invalid;
						array2 = new byte[num5 * 12u];
						int num6 = 0;
						while ((long)num6 < (long)((ulong)num5))
						{
							Buffer.BlockCopy(BitConverter.GetBytes(array3[num6].m_itemId.m_SteamItemInstanceID), 0, array2, num6 * 12, 8);
							Buffer.BlockCopy(BitConverter.GetBytes(array3[num6].m_iDefinition.m_SteamItemDef), 0, array2, num6 * 12 + 8, 4);
							num6++;
						}
						num4 = (uint)array2.Length;
					}
					byte[] array4 = new byte[(long)(5 + array.Length) + (long)((ulong)num4)];
					array4[0] = 4;
					Buffer.BlockCopy(BitConverter.GetBytes((ushort)array.Length), 0, array4, 1, 2);
					Buffer.BlockCopy(array, 0, array4, 3, array.Length);
					Buffer.BlockCopy(BitConverter.GetBytes((ushort)num4), 0, array4, 3 + array.Length, 2);
					Buffer.BlockCopy(array2, 0, array4, 5 + array.Length, (int)num4);
					Provider.send(Provider.server, ESteamPacket.AUTHENTICATE, array4, array4.Length, 0);
					return;
				}
				else
				{
					if (esteamPacket == ESteamPacket.ACCEPTED)
					{
						object[] objects3 = SteamPacker.getObjects(steamID, offset, 0, packet, new Type[]
						{
							Types.BYTE_TYPE,
							Types.UINT32_TYPE,
							Types.UINT16_TYPE,
							Types.BYTE_TYPE,
							Types.BOOLEAN_TYPE,
							Types.BOOLEAN_TYPE,
							Types.BOOLEAN_TYPE,
							Types.BOOLEAN_TYPE,
							Types.BOOLEAN_TYPE,
							Types.BOOLEAN_TYPE,
							Types.BOOLEAN_TYPE,
							Types.BOOLEAN_TYPE,
							Types.UINT16_TYPE,
							Types.UINT16_TYPE,
							Types.UINT16_TYPE
						});
						uint num7 = (uint)objects3[1];
						ushort num8 = (ushort)objects3[2];
						if (Provider.currentServerInfo != null && Provider.currentServerInfo.IsBattlEyeSecure)
						{
							string text2 = ReadWrite.PATH + "/BattlEye/BEClient_x64.so";
							if (!File.Exists(text2))
							{
								text2 = ReadWrite.PATH + "/BattlEye/BEClient.so";
							}
							if (!File.Exists(text2))
							{
								Provider._connectionFailureInfo = ESteamConnectionFailureInfo.KICKED;
								Provider._connectionFailureReason = "Missing BattlEye client library!";
								Debug.LogError(Provider.connectionFailureReason);
								Provider.disconnect();
								return;
							}
							try
							{
								Provider.battlEyeClientHandle = BEClient.dlopen(text2, 2);
								if (!(Provider.battlEyeClientHandle != IntPtr.Zero))
								{
									Provider._connectionFailureInfo = ESteamConnectionFailureInfo.KICKED;
									Provider._connectionFailureReason = "Failed to load BattlEye client library!";
									Debug.LogError(Provider.connectionFailureReason);
									Provider.disconnect();
									return;
								}
								IntPtr ptr = BEClient.dlsym(Provider.battlEyeClientHandle, "Init");
								BEClient.BEClientInitFn beclientInitFn = Marshal.GetDelegateForFunctionPointer(ptr, typeof(BEClient.BEClientInitFn)) as BEClient.BEClientInitFn;
								if (beclientInitFn == null)
								{
									BEClient.dlclose(Provider.battlEyeClientHandle);
									Provider.battlEyeClientHandle = IntPtr.Zero;
									Provider._connectionFailureInfo = ESteamConnectionFailureInfo.KICKED;
									Provider._connectionFailureReason = "Failed to get BattlEye client init delegate!";
									Debug.LogError(Provider.connectionFailureReason);
									Provider.disconnect();
									return;
								}
								uint ulAddress = (num7 & 255u) << 24 | (num7 & 65280u) << 8 | (num7 & 16711680u) >> 8 | (num7 & 4278190080u) >> 24;
								ushort usPort = (ushort)((int)(num8 & 255) << 8 | (int)((uint)(num8 & 65280) >> 8));
								Provider.battlEyeClientInitData = new BEClient.BECL_GAME_DATA();
								Provider.battlEyeClientInitData.pstrGameVersion = Provider.APP_NAME + " " + Provider.APP_VERSION;
								Provider.battlEyeClientInitData.ulAddress = ulAddress;
								Provider.battlEyeClientInitData.usPort = usPort;
								BEClient.BECL_GAME_DATA becl_GAME_DATA = Provider.battlEyeClientInitData;
								if (Provider.<>f__mg$cache0 == null)
								{
									Provider.<>f__mg$cache0 = new BEClient.BECL_GAME_DATA.PrintMessageFn(Provider.battlEyeClientPrintMessage);
								}
								becl_GAME_DATA.pfnPrintMessage = Provider.<>f__mg$cache0;
								BEClient.BECL_GAME_DATA becl_GAME_DATA2 = Provider.battlEyeClientInitData;
								if (Provider.<>f__mg$cache1 == null)
								{
									Provider.<>f__mg$cache1 = new BEClient.BECL_GAME_DATA.RequestRestartFn(Provider.battlEyeClientRequestRestart);
								}
								becl_GAME_DATA2.pfnRequestRestart = Provider.<>f__mg$cache1;
								BEClient.BECL_GAME_DATA becl_GAME_DATA3 = Provider.battlEyeClientInitData;
								if (Provider.<>f__mg$cache2 == null)
								{
									Provider.<>f__mg$cache2 = new BEClient.BECL_GAME_DATA.SendPacketFn(Provider.battlEyeClientSendPacket);
								}
								becl_GAME_DATA3.pfnSendPacket = Provider.<>f__mg$cache2;
								Provider.battlEyeClientRunData = new BEClient.BECL_BE_DATA();
								if (!beclientInitFn.Invoke(2, Provider.battlEyeClientInitData, Provider.battlEyeClientRunData))
								{
									BEClient.dlclose(Provider.battlEyeClientHandle);
									Provider.battlEyeClientHandle = IntPtr.Zero;
									Provider._connectionFailureInfo = ESteamConnectionFailureInfo.KICKED;
									Provider._connectionFailureReason = "Failed to call BattlEye client init!";
									Debug.LogError(Provider.connectionFailureReason);
									Provider.disconnect();
									return;
								}
							}
							catch (Exception ex)
							{
								Provider._connectionFailureInfo = ESteamConnectionFailureInfo.KICKED;
								Provider._connectionFailureReason = "Unhandled exception when loading BattlEye client library!";
								Debug.LogError(Provider.connectionFailureReason);
								Debug.LogException(ex);
								Provider.disconnect();
								return;
							}
						}
						Provider._modeConfigData = new ModeConfigData(Provider.mode);
						Provider.modeConfigData.Gameplay.Repair_Level_Max = (uint)((byte)objects3[3]);
						Provider.modeConfigData.Gameplay.Hitmarkers = (bool)objects3[4];
						Provider.modeConfigData.Gameplay.Crosshair = (bool)objects3[5];
						Provider.modeConfigData.Gameplay.Ballistics = (bool)objects3[6];
						Provider.modeConfigData.Gameplay.Chart = (bool)objects3[7];
						Provider.modeConfigData.Gameplay.Group_Map = (bool)objects3[8];
						Provider.modeConfigData.Gameplay.Group_HUD = (bool)objects3[9];
						Provider.modeConfigData.Gameplay.Allow_Dynamic_Groups = (bool)objects3[10];
						Provider.modeConfigData.Gameplay.Allow_Shoulder_Camera = (bool)objects3[11];
						Provider.modeConfigData.Gameplay.Timer_Exit = (uint)((ushort)objects3[12]);
						Provider.modeConfigData.Gameplay.Timer_Respawn = (uint)((ushort)objects3[13]);
						Provider.modeConfigData.Gameplay.Timer_Home = (uint)((ushort)objects3[14]);
						SteamUser.AdvertiseGame(Provider.server, num7, num8);
						if (OptionsSettings.streamer)
						{
							SteamFriends.SetRichPresence("connect", string.Empty);
						}
						else
						{
							SteamFriends.SetRichPresence("connect", string.Concat(new object[]
							{
								"+connect ",
								num7,
								":",
								num8
							}));
						}
						Lobbies.leaveLobby();
						Provider.favoriteIP = num7;
						Provider.favoritePort = num8;
						Provider._isFavorited = Provider.checkFavorite(Provider.favoriteIP, Provider.favoritePort);
						SteamMatchmaking.AddFavoriteGame(Provider.APP_ID, num7, num8, num8 + 1, 2u, SteamUtils.GetServerRealTime());
						Provider.discordJoinIP = num7;
						Provider.discordJoinPort = num8;
						Provider.updateRichPresence();
						if (Provider.onClientConnected != null)
						{
							Provider.onClientConnected();
						}
						return;
					}
					if (esteamPacket == ESteamPacket.REJECTED)
					{
						ESteamRejection esteamRejection = (ESteamRejection)packet[offset + 1];
						if (esteamRejection == ESteamRejection.WHITELISTED)
						{
							Provider._connectionFailureInfo = ESteamConnectionFailureInfo.WHITELISTED;
						}
						else if (esteamRejection == ESteamRejection.WRONG_PASSWORD)
						{
							Provider._connectionFailureInfo = ESteamConnectionFailureInfo.PASSWORD;
						}
						else if (esteamRejection == ESteamRejection.SERVER_FULL)
						{
							Provider._connectionFailureInfo = ESteamConnectionFailureInfo.FULL;
						}
						else if (esteamRejection == ESteamRejection.WRONG_HASH_LEVEL)
						{
							Provider._connectionFailureInfo = ESteamConnectionFailureInfo.HASH_LEVEL;
						}
						else if (esteamRejection == ESteamRejection.WRONG_HASH_ASSEMBLY)
						{
							Provider._connectionFailureInfo = ESteamConnectionFailureInfo.HASH_ASSEMBLY;
						}
						else if (esteamRejection == ESteamRejection.WRONG_VERSION)
						{
							Provider._connectionFailureInfo = ESteamConnectionFailureInfo.VERSION;
						}
						else if (esteamRejection == ESteamRejection.PRO_SERVER)
						{
							Provider._connectionFailureInfo = ESteamConnectionFailureInfo.PRO_SERVER;
						}
						else if (esteamRejection == ESteamRejection.PRO_CHARACTER)
						{
							Provider._connectionFailureInfo = ESteamConnectionFailureInfo.PRO_CHARACTER;
						}
						else if (esteamRejection == ESteamRejection.PRO_DESYNC)
						{
							Provider._connectionFailureInfo = ESteamConnectionFailureInfo.PRO_DESYNC;
						}
						else if (esteamRejection == ESteamRejection.PRO_APPEARANCE)
						{
							Provider._connectionFailureInfo = ESteamConnectionFailureInfo.PRO_APPEARANCE;
						}
						else if (esteamRejection == ESteamRejection.AUTH_VERIFICATION)
						{
							Provider._connectionFailureInfo = ESteamConnectionFailureInfo.AUTH_VERIFICATION;
						}
						else if (esteamRejection == ESteamRejection.AUTH_NO_STEAM)
						{
							Provider._connectionFailureInfo = ESteamConnectionFailureInfo.AUTH_NO_STEAM;
						}
						else if (esteamRejection == ESteamRejection.AUTH_LICENSE_EXPIRED)
						{
							Provider._connectionFailureInfo = ESteamConnectionFailureInfo.AUTH_LICENSE_EXPIRED;
						}
						else if (esteamRejection == ESteamRejection.AUTH_VAC_BAN)
						{
							Provider._connectionFailureInfo = ESteamConnectionFailureInfo.AUTH_VAC_BAN;
						}
						else if (esteamRejection == ESteamRejection.AUTH_ELSEWHERE)
						{
							Provider._connectionFailureInfo = ESteamConnectionFailureInfo.AUTH_ELSEWHERE;
						}
						else if (esteamRejection == ESteamRejection.AUTH_TIMED_OUT)
						{
							Provider._connectionFailureInfo = ESteamConnectionFailureInfo.AUTH_TIMED_OUT;
						}
						else if (esteamRejection == ESteamRejection.AUTH_USED)
						{
							Provider._connectionFailureInfo = ESteamConnectionFailureInfo.AUTH_USED;
						}
						else if (esteamRejection == ESteamRejection.AUTH_NO_USER)
						{
							Provider._connectionFailureInfo = ESteamConnectionFailureInfo.AUTH_NO_USER;
						}
						else if (esteamRejection == ESteamRejection.AUTH_PUB_BAN)
						{
							Provider._connectionFailureInfo = ESteamConnectionFailureInfo.AUTH_PUB_BAN;
						}
						else if (esteamRejection == ESteamRejection.AUTH_ECON_DESERIALIZE)
						{
							Provider._connectionFailureInfo = ESteamConnectionFailureInfo.AUTH_ECON_DESERIALIZE;
						}
						else if (esteamRejection == ESteamRejection.AUTH_ECON_VERIFY)
						{
							Provider._connectionFailureInfo = ESteamConnectionFailureInfo.AUTH_ECON_VERIFY;
						}
						else if (esteamRejection == ESteamRejection.ALREADY_CONNECTED)
						{
							Provider._connectionFailureInfo = ESteamConnectionFailureInfo.ALREADY_CONNECTED;
						}
						else if (esteamRejection == ESteamRejection.ALREADY_PENDING)
						{
							Provider._connectionFailureInfo = ESteamConnectionFailureInfo.ALREADY_PENDING;
						}
						else if (esteamRejection == ESteamRejection.LATE_PENDING)
						{
							Provider._connectionFailureInfo = ESteamConnectionFailureInfo.LATE_PENDING;
						}
						else if (esteamRejection == ESteamRejection.NOT_PENDING)
						{
							Provider._connectionFailureInfo = ESteamConnectionFailureInfo.NOT_PENDING;
						}
						else if (esteamRejection == ESteamRejection.NAME_PLAYER_SHORT)
						{
							Provider._connectionFailureInfo = ESteamConnectionFailureInfo.NAME_PLAYER_SHORT;
						}
						else if (esteamRejection == ESteamRejection.NAME_PLAYER_LONG)
						{
							Provider._connectionFailureInfo = ESteamConnectionFailureInfo.NAME_PLAYER_LONG;
						}
						else if (esteamRejection == ESteamRejection.NAME_PLAYER_INVALID)
						{
							Provider._connectionFailureInfo = ESteamConnectionFailureInfo.NAME_PLAYER_INVALID;
						}
						else if (esteamRejection == ESteamRejection.NAME_PLAYER_NUMBER)
						{
							Provider._connectionFailureInfo = ESteamConnectionFailureInfo.NAME_PLAYER_NUMBER;
						}
						else if (esteamRejection == ESteamRejection.NAME_CHARACTER_SHORT)
						{
							Provider._connectionFailureInfo = ESteamConnectionFailureInfo.NAME_CHARACTER_SHORT;
						}
						else if (esteamRejection == ESteamRejection.NAME_CHARACTER_LONG)
						{
							Provider._connectionFailureInfo = ESteamConnectionFailureInfo.NAME_CHARACTER_LONG;
						}
						else if (esteamRejection == ESteamRejection.NAME_CHARACTER_INVALID)
						{
							Provider._connectionFailureInfo = ESteamConnectionFailureInfo.NAME_CHARACTER_INVALID;
						}
						else if (esteamRejection == ESteamRejection.NAME_CHARACTER_NUMBER)
						{
							Provider._connectionFailureInfo = ESteamConnectionFailureInfo.NAME_CHARACTER_NUMBER;
						}
						else if (esteamRejection == ESteamRejection.PING)
						{
							Provider._connectionFailureInfo = ESteamConnectionFailureInfo.PING;
						}
						else if (esteamRejection == ESteamRejection.PLUGIN)
						{
							Provider._connectionFailureInfo = ESteamConnectionFailureInfo.PLUGIN;
						}
						else if (esteamRejection == ESteamRejection.CLIENT_MODULE_DESYNC)
						{
							Provider._connectionFailureInfo = ESteamConnectionFailureInfo.CLIENT_MODULE_DESYNC;
						}
						else if (esteamRejection == ESteamRejection.SERVER_MODULE_DESYNC)
						{
							Provider._connectionFailureInfo = ESteamConnectionFailureInfo.SERVER_MODULE_DESYNC;
						}
						Provider.disconnect();
						return;
					}
					if (esteamPacket == ESteamPacket.BANNED)
					{
						object[] objects4 = SteamPacker.getObjects(steamID, offset, 0, packet, Types.BYTE_TYPE, Types.STRING_TYPE, Types.UINT32_TYPE);
						Provider._connectionFailureInfo = ESteamConnectionFailureInfo.BANNED;
						Provider._connectionFailureReason = (string)objects4[1];
						Provider._connectionFailureDuration = (uint)objects4[2];
						Provider.disconnect();
						return;
					}
					if (esteamPacket == ESteamPacket.KICKED)
					{
						object[] objects5 = SteamPacker.getObjects(steamID, offset, 0, packet, Types.BYTE_TYPE, Types.STRING_TYPE);
						Provider._connectionFailureInfo = ESteamConnectionFailureInfo.KICKED;
						Provider._connectionFailureReason = (string)objects5[1];
						Provider.disconnect();
						return;
					}
					if (esteamPacket == ESteamPacket.ADMINED)
					{
						int num9 = (int)packet[offset + 1];
						if (num9 < 0 || num9 >= Provider.clients.Count)
						{
							Debug.LogError("Failed to find player at index " + num9 + ".");
							return;
						}
						Provider.clients[num9].isAdmin = true;
						return;
					}
					else if (esteamPacket == ESteamPacket.UNADMINED)
					{
						int num10 = (int)packet[offset + 1];
						if (num10 < 0 || num10 >= Provider.clients.Count)
						{
							Debug.LogError("Failed to find player at index " + num10 + ".");
							return;
						}
						Provider.clients[num10].isAdmin = false;
						return;
					}
					else
					{
						if (esteamPacket == ESteamPacket.BATTLEYE)
						{
							if (Provider.battlEyeClientHandle != IntPtr.Zero && Provider.battlEyeClientRunData != null && Provider.battlEyeClientRunData.pfnReceivedPacket != null)
							{
								GCHandle gchandle = GCHandle.Alloc(packet, GCHandleType.Pinned);
								IntPtr intPtr = gchandle.AddrOfPinnedObject();
								if (IntPtr.Size == 4)
								{
									intPtr = new IntPtr(intPtr.ToInt32() + offset + 1);
								}
								else
								{
									intPtr = new IntPtr(intPtr.ToInt64() + (long)offset + 1L);
								}
								Provider.battlEyeClientRunData.pfnReceivedPacket.Invoke(intPtr, size - offset - 1);
								gchandle.Free();
							}
							return;
						}
						if (esteamPacket == ESteamPacket.GUIDTABLE)
						{
							SteamPacker.block.reset(offset, packet);
							SteamPacker.block.readByte();
							ushort num11 = SteamPacker.block.readUInt16();
							for (ushort num12 = 0; num12 < num11; num12 += 1)
							{
								GUIDTableIndex index = SteamPacker.block.readUInt16();
								GuidBuffer guidBuffer = default(GuidBuffer);
								guidBuffer.Read(packet, SteamPacker.block.step);
								GUIDTable.addMapping(index, guidBuffer.GUID);
							}
							byte[] array5 = new byte[]
							{
								25
							};
							Provider.send(Provider.server, ESteamPacket.GUIDTABLE, array5, array5.Length, 0);
						}
						if (esteamPacket == ESteamPacket.GUIDTABLE)
						{
							for (int j = 0; j < Provider.pending.Count; j++)
							{
								if (Provider.pending[j].playerID.steamID == steamID)
								{
									Provider.sendGUIDTable(Provider.pending[j]);
									return;
								}
							}
						}
						Debug.LogError("Failed to handle message: " + esteamPacket);
					}
				}
			}
		}

		private static void listenServer(int channel)
		{
			ICommunityEntity communityEntity;
			ulong num;
			while (Provider.provider.multiplayerService.serverMultiplayerService.read(out communityEntity, Provider.buffer, out num, channel))
			{
				Provider.receiveServer(((SteamworksCommunityEntity)communityEntity).steamID, Provider.buffer, 0, (int)num, channel);
			}
		}

		private static void listenClient(int channel)
		{
			ICommunityEntity communityEntity;
			ulong num;
			while (Provider.provider.multiplayerService.clientMultiplayerService.read(out communityEntity, Provider.buffer, out num, channel))
			{
				Provider.receiveClient(((SteamworksCommunityEntity)communityEntity).steamID, Provider.buffer, 0, (int)num, channel);
			}
		}

		private static void listen()
		{
			if (!Provider.isConnected)
			{
				return;
			}
			if (Provider.isServer)
			{
				if (!Dedicator.isDedicated)
				{
					return;
				}
				if (!Level.isLoaded)
				{
					return;
				}
				Provider.listenServer(0);
				for (int i = 0; i < Provider.receivers.Count; i++)
				{
					Provider.listenServer(Provider.receivers[i].id);
				}
				if (Dedicator.isDedicated)
				{
					if (Time.realtimeSinceStartup - Provider.lastCheck > Provider.CHECKRATE)
					{
						Provider.lastCheck = Time.realtimeSinceStartup;
						for (int j = 0; j < Provider.clients.Count; j++)
						{
							if (Time.realtimeSinceStartup - Provider.clients[j].lastPing > 1f || Provider.clients[j].lastPing < 0f)
							{
								Provider.clients[j].lastPing = Time.realtimeSinceStartup;
								Provider.send(Provider.clients[j].playerID.steamID, ESteamPacket.TICK, new byte[]
								{
									13
								}, 1, 0);
							}
						}
					}
					for (int k = 0; k < Provider.clients.Count; k++)
					{
						if (Time.realtimeSinceStartup - Provider.clients[k].lastNet > Provider.configData.Server.Timeout_Game_Seconds || (Time.realtimeSinceStartup - Provider.clients[k].joined > Provider.configData.Server.Timeout_Game_Seconds && Provider.clients[k].ping > Provider.configData.Server.Max_Ping_Milliseconds / 1000f))
						{
							Provider.dismiss(Provider.clients[k].playerID.steamID);
						}
					}
					if (Provider.pending.Count > 0 && Provider.pending[0].lastActive > 0f && Time.realtimeSinceStartup - Provider.pending[0].lastActive > Provider.configData.Server.Timeout_Queue_Seconds)
					{
						Provider.reject(Provider.pending[0].playerID.steamID, ESteamRejection.LATE_PENDING);
					}
					if (Provider.pending.Count > 1)
					{
						for (int l = Provider.pending.Count - 1; l > 0; l--)
						{
							if (Time.realtimeSinceStartup - Provider.pending[l].lastNet > Provider.configData.Server.Timeout_Queue_Seconds)
							{
								Provider.reject(Provider.pending[l].playerID.steamID, ESteamRejection.LATE_PENDING);
							}
						}
					}
				}
			}
			else
			{
				Provider.listenClient(0);
				for (int m = 0; m < Provider.receivers.Count; m++)
				{
					Provider.listenClient(Provider.receivers[m].id);
				}
				if (Time.realtimeSinceStartup - Provider.lastCheck > Provider.CHECKRATE && (Time.realtimeSinceStartup - Provider.lastPing > 1f || Provider.lastPing < 0f))
				{
					Provider.lastCheck = Time.realtimeSinceStartup;
					Provider.lastPing = Time.realtimeSinceStartup;
					Provider.send(Provider.server, ESteamPacket.TICK, new byte[]
					{
						13
					}, 1, 0);
				}
				if (Provider.isLoadingUGC)
				{
					if (Provider.isTesting)
					{
						if (Time.realtimeSinceStartup - Provider.lastNet > (float)Provider.CLIENT_TIMEOUT)
						{
							Provider._connectionFailureInfo = ESteamConnectionFailureInfo.TIMED_OUT;
							Provider.disconnect();
						}
						return;
					}
					Provider.lastNet = Time.realtimeSinceStartup;
					return;
				}
				else
				{
					if (Level.isLoading)
					{
						Provider.lastNet = Time.realtimeSinceStartup;
						return;
					}
					if (Time.realtimeSinceStartup - Provider.lastNet > (float)Provider.CLIENT_TIMEOUT)
					{
						Provider._connectionFailureInfo = ESteamConnectionFailureInfo.TIMED_OUT;
						Provider.disconnect();
						return;
					}
					if (Provider.battlEyeHasRequiredRestart)
					{
						Provider.battlEyeHasRequiredRestart = false;
						Provider.disconnect();
						return;
					}
				}
			}
		}

		private static void onP2PSessionConnectFail(P2PSessionConnectFail_t callback)
		{
			Provider.dismiss(callback.m_steamIDRemote);
		}

		private static void handleValidateAuthTicketResponse(ValidateAuthTicketResponse_t callback)
		{
			bool flag = callback.m_eAuthSessionResponse == 0;
			if (Provider.onCheckValid != null)
			{
				Provider.onCheckValid(callback, ref flag);
			}
			if (flag)
			{
				SteamPending steamPending = null;
				for (int i = 0; i < Provider.pending.Count; i++)
				{
					if (Provider.pending[i].playerID.steamID == callback.m_SteamID)
					{
						steamPending = Provider.pending[i];
						break;
					}
				}
				if (steamPending == null)
				{
					for (int j = 0; j < Provider.clients.Count; j++)
					{
						if (Provider.clients[j].playerID.steamID == callback.m_SteamID)
						{
							return;
						}
					}
					Provider.reject(callback.m_SteamID, ESteamRejection.NOT_PENDING);
					return;
				}
				bool flag2 = SteamGameServer.UserHasLicenseForApp(steamPending.playerID.steamID, Provider.PRO_ID) != 1;
				if (Provider.isGold && !flag2)
				{
					Provider.reject(steamPending.playerID.steamID, ESteamRejection.PRO_SERVER);
					return;
				}
				if ((steamPending.playerID.characterID >= Customization.FREE_CHARACTERS && !flag2) || steamPending.playerID.characterID >= Customization.FREE_CHARACTERS + Customization.PRO_CHARACTERS)
				{
					Provider.reject(steamPending.playerID.steamID, ESteamRejection.PRO_CHARACTER);
					return;
				}
				if (!flag2 && steamPending.isPro)
				{
					Provider.reject(steamPending.playerID.steamID, ESteamRejection.PRO_DESYNC);
					return;
				}
				if (steamPending.face >= Customization.FACES_FREE + Customization.FACES_PRO || (!flag2 && steamPending.face >= Customization.FACES_FREE))
				{
					Provider.reject(steamPending.playerID.steamID, ESteamRejection.PRO_APPEARANCE);
					return;
				}
				if (steamPending.hair >= Customization.HAIRS_FREE + Customization.HAIRS_PRO || (!flag2 && steamPending.hair >= Customization.HAIRS_FREE))
				{
					Provider.reject(steamPending.playerID.steamID, ESteamRejection.PRO_APPEARANCE);
					return;
				}
				if (steamPending.beard >= Customization.BEARDS_FREE + Customization.BEARDS_PRO || (!flag2 && steamPending.beard >= Customization.BEARDS_FREE))
				{
					Provider.reject(steamPending.playerID.steamID, ESteamRejection.PRO_APPEARANCE);
					return;
				}
				if (!flag2)
				{
					if (!Customization.checkSkin(steamPending.skin))
					{
						Provider.reject(steamPending.playerID.steamID, ESteamRejection.PRO_APPEARANCE);
						return;
					}
					if (!Customization.checkColor(steamPending.color))
					{
						Provider.reject(steamPending.playerID.steamID, ESteamRejection.PRO_APPEARANCE);
						return;
					}
				}
				steamPending.assignedPro = flag2;
				steamPending.assignedAdmin = SteamAdminlist.checkAdmin(steamPending.playerID.steamID);
				steamPending.hasAuthentication = true;
				if (steamPending.canAcceptYet)
				{
					Provider.sendGUIDTable(steamPending);
				}
			}
			else if (callback.m_eAuthSessionResponse == 1)
			{
				Provider.reject(callback.m_SteamID, ESteamRejection.AUTH_NO_STEAM);
			}
			else if (callback.m_eAuthSessionResponse == 2)
			{
				Provider.reject(callback.m_SteamID, ESteamRejection.AUTH_LICENSE_EXPIRED);
			}
			else if (callback.m_eAuthSessionResponse == 3)
			{
				Provider.reject(callback.m_SteamID, ESteamRejection.AUTH_VAC_BAN);
			}
			else if (callback.m_eAuthSessionResponse == 4)
			{
				Provider.reject(callback.m_SteamID, ESteamRejection.AUTH_ELSEWHERE);
			}
			else if (callback.m_eAuthSessionResponse == 5)
			{
				Provider.reject(callback.m_SteamID, ESteamRejection.AUTH_TIMED_OUT);
			}
			else if (callback.m_eAuthSessionResponse == 6)
			{
				Provider.dismiss(callback.m_SteamID);
			}
			else if (callback.m_eAuthSessionResponse == 7)
			{
				Provider.reject(callback.m_SteamID, ESteamRejection.AUTH_USED);
			}
			else if (callback.m_eAuthSessionResponse == 8)
			{
				Provider.reject(callback.m_SteamID, ESteamRejection.AUTH_NO_USER);
			}
			else if (callback.m_eAuthSessionResponse == 9)
			{
				Provider.reject(callback.m_SteamID, ESteamRejection.AUTH_PUB_BAN);
			}
			else if (!flag)
			{
				Provider.reject(callback.m_SteamID, ESteamRejection.PLUGIN);
			}
		}

		private static void onValidateAuthTicketResponse(ValidateAuthTicketResponse_t callback)
		{
			Provider.handleValidateAuthTicketResponse(callback);
		}

		private static void handleClientGroupStatus(GSClientGroupStatus_t callback)
		{
			SteamPending steamPending = null;
			for (int i = 0; i < Provider.pending.Count; i++)
			{
				if (Provider.pending[i].playerID.steamID == callback.m_SteamIDUser)
				{
					steamPending = Provider.pending[i];
					break;
				}
			}
			if (steamPending == null)
			{
				Provider.reject(callback.m_SteamIDUser, ESteamRejection.NOT_PENDING);
				return;
			}
			if (!callback.m_bMember && !callback.m_bOfficer)
			{
				steamPending.playerID.group = CSteamID.Nil;
			}
			steamPending.hasGroup = true;
			if (steamPending.canAcceptYet)
			{
				Provider.sendGUIDTable(steamPending);
			}
		}

		private static void onClientGroupStatus(GSClientGroupStatus_t callback)
		{
			Provider.handleClientGroupStatus(callback);
		}

		public static byte maxPlayers
		{
			get
			{
				return Provider._maxPlayers;
			}
			set
			{
				Provider._maxPlayers = value;
				if (Provider.isServer)
				{
					SteamGameServer.SetMaxPlayerCount((int)Provider.maxPlayers);
				}
			}
		}

		public static byte queuePosition
		{
			get
			{
				return Provider._queuePosition;
			}
		}

		public static string serverName
		{
			get
			{
				return Provider._serverName;
			}
			set
			{
				Provider._serverName = value;
				if (Dedicator.commandWindow != null)
				{
					Dedicator.commandWindow.title = Provider.serverName;
				}
				if (Provider.isServer)
				{
					SteamGameServer.SetServerName(Provider.serverName);
				}
			}
		}

		public static string serverID
		{
			get
			{
				return Dedicator.serverID;
			}
			set
			{
				Dedicator.serverID = value;
			}
		}

		public static byte[] serverPasswordHash
		{
			get
			{
				return Provider._serverPasswordHash;
			}
		}

		public static string serverPassword
		{
			get
			{
				return Provider._serverPassword;
			}
			set
			{
				Provider._serverPassword = value;
				Provider._serverPasswordHash = Hash.SHA1(Provider.serverPassword);
				if (Provider.isServer)
				{
					SteamGameServer.SetPasswordProtected(Provider.serverPassword != string.Empty);
				}
			}
		}

		public static StatusData statusData
		{
			get
			{
				return Provider._statusData;
			}
		}

		public static PreferenceData preferenceData
		{
			get
			{
				return Provider._preferenceData;
			}
		}

		public static ConfigData configData
		{
			get
			{
				return Provider._configData;
			}
		}

		public static ModeConfigData modeConfigData
		{
			get
			{
				return Provider._modeConfigData;
			}
		}

		public static void resetConfig()
		{
			Provider._modeConfigData = new ModeConfigData(Provider.mode);
			EGameMode egameMode = Provider.mode;
			if (egameMode != EGameMode.EASY)
			{
				if (egameMode != EGameMode.NORMAL)
				{
					if (egameMode == EGameMode.HARD)
					{
						Provider.configData.Hard = Provider.modeConfigData;
					}
				}
				else
				{
					Provider.configData.Normal = Provider.modeConfigData;
				}
			}
			else
			{
				Provider.configData.Easy = Provider.modeConfigData;
			}
			ServerSavedata.serializeJSON<ConfigData>("/Config.json", Provider.configData);
		}

		public static void accept(SteamPending player)
		{
			Provider.accept(player.playerID, player.assignedPro, player.assignedAdmin, player.face, player.hair, player.beard, player.skin, player.color, player.hand, player.shirtItem, player.pantsItem, player.hatItem, player.backpackItem, player.vestItem, player.maskItem, player.glassesItem, player.skinItems, player.skillset, player.language, player.lobbyID);
		}

		public static void accept(SteamPlayerID playerID, bool isPro, bool isAdmin, byte face, byte hair, byte beard, Color skin, Color color, bool hand, int shirtItem, int pantsItem, int hatItem, int backpackItem, int vestItem, int maskItem, int glassesItem, int[] skinItems, EPlayerSkillset skillset, string language, CSteamID lobbyID)
		{
			bool flag = false;
			int num = 0;
			for (int i = 0; i < Provider.pending.Count; i++)
			{
				if (Provider.pending[i].playerID == playerID)
				{
					if (Provider.pending[i].inventoryResult != SteamInventoryResult_t.Invalid)
					{
						SteamGameServerInventory.DestroyResult(Provider.pending[i].inventoryResult);
						Provider.pending[i].inventoryResult = SteamInventoryResult_t.Invalid;
					}
					flag = true;
					num = i;
					Provider.pending.RemoveAt(i);
					break;
				}
			}
			if (!flag)
			{
				return;
			}
			if (isPro)
			{
				SteamGameServer.BUpdateUserData(playerID.steamID, playerID.playerName, 1u);
			}
			else
			{
				SteamGameServer.BUpdateUserData(playerID.steamID, playerID.playerName, 0u);
			}
			Vector3 vector = Vector3.zero;
			byte b;
			if (PlayerSavedata.fileExists(playerID, "/Player/Player.dat") && Level.info.type == ELevelType.SURVIVAL)
			{
				Block block = PlayerSavedata.readBlock(playerID, "/Player/Player.dat", 1);
				vector = block.readSingleVector3() + new Vector3(0f, 0.02f, 0f);
				b = block.readByte();
			}
			else
			{
				PlayerSpawnpoint spawn = LevelPlayers.getSpawn(false);
				vector = spawn.point + new Vector3(0f, 0.5f, 0f);
				b = (byte)(spawn.angle / 2f);
			}
			int channels = Provider.channels;
			Provider.addPlayer(playerID, vector, b, isPro, isAdmin, channels, face, hair, beard, skin, color, hand, shirtItem, pantsItem, hatItem, backpackItem, vestItem, maskItem, glassesItem, skinItems, skillset, language, lobbyID);
			int size;
			byte[] bytes;
			for (int j = 0; j < Provider.clients.Count; j++)
			{
				if (Provider.clients[j].playerID != playerID)
				{
					bytes = SteamPacker.getBytes(0, out size, new object[]
					{
						11,
						Provider.clients[j].playerID.steamID,
						Provider.clients[j].playerID.characterID,
						Provider.clients[j].playerID.playerName,
						Provider.clients[j].playerID.characterName,
						Provider.clients[j].model.transform.position,
						(byte)(Provider.clients[j].model.transform.rotation.eulerAngles.y / 2f),
						Provider.clients[j].isPro,
						Provider.clients[j].isAdmin && !Provider.hideAdmins,
						Provider.clients[j].channel,
						Provider.clients[j].playerID.group,
						Provider.clients[j].playerID.nickName,
						Provider.clients[j].face,
						Provider.clients[j].hair,
						Provider.clients[j].beard,
						Provider.clients[j].skin,
						Provider.clients[j].color,
						Provider.clients[j].hand,
						Provider.clients[j].shirtItem,
						Provider.clients[j].pantsItem,
						Provider.clients[j].hatItem,
						Provider.clients[j].backpackItem,
						Provider.clients[j].vestItem,
						Provider.clients[j].maskItem,
						Provider.clients[j].glassesItem,
						Provider.clients[j].skinItems,
						(byte)Provider.clients[j].skillset,
						Provider.clients[j].language
					});
				}
				else
				{
					bytes = SteamPacker.getBytes(0, out size, new object[]
					{
						11,
						Provider.clients[j].playerID.steamID,
						Provider.clients[j].playerID.characterID,
						Provider.clients[j].playerID.playerName,
						Provider.clients[j].playerID.characterName,
						Provider.clients[j].model.transform.position,
						(byte)(Provider.clients[j].model.transform.rotation.eulerAngles.y / 2f),
						Provider.clients[j].isPro,
						Provider.clients[j].isAdmin,
						Provider.clients[j].channel,
						Provider.clients[j].playerID.group,
						Provider.clients[j].playerID.nickName,
						Provider.clients[j].face,
						Provider.clients[j].hair,
						Provider.clients[j].beard,
						Provider.clients[j].skin,
						Provider.clients[j].color,
						Provider.clients[j].hand,
						Provider.clients[j].shirtItem,
						Provider.clients[j].pantsItem,
						Provider.clients[j].hatItem,
						Provider.clients[j].backpackItem,
						Provider.clients[j].vestItem,
						Provider.clients[j].maskItem,
						Provider.clients[j].glassesItem,
						Provider.clients[j].skinItems,
						(byte)Provider.clients[j].skillset,
						Provider.clients[j].language
					});
				}
				Provider.send(playerID.steamID, ESteamPacket.CONNECTED, bytes, size, 0);
			}
			bytes = SteamPacker.getBytes(0, out size, new object[]
			{
				6,
				SteamGameServer.GetPublicIP(),
				Provider.port,
				(byte)Provider.modeConfigData.Gameplay.Repair_Level_Max,
				Provider.modeConfigData.Gameplay.Hitmarkers,
				Provider.modeConfigData.Gameplay.Crosshair,
				Provider.modeConfigData.Gameplay.Ballistics,
				Provider.modeConfigData.Gameplay.Chart,
				Provider.modeConfigData.Gameplay.Group_Map,
				Provider.modeConfigData.Gameplay.Group_HUD,
				Provider.modeConfigData.Gameplay.Allow_Dynamic_Groups,
				Provider.modeConfigData.Gameplay.Allow_Shoulder_Camera,
				(ushort)Provider.modeConfigData.Gameplay.Timer_Exit,
				(ushort)Provider.modeConfigData.Gameplay.Timer_Respawn,
				(ushort)Provider.modeConfigData.Gameplay.Timer_Home
			});
			Provider.send(playerID.steamID, ESteamPacket.ACCEPTED, bytes, size, 0);
			if (Provider.battlEyeServerHandle != IntPtr.Zero && Provider.battlEyeServerRunData != null && Provider.battlEyeServerRunData.pfnAddPlayer != null && Provider.battlEyeServerRunData.pfnReceivedPlayerGUID != null)
			{
				P2PSessionState_t p2PSessionState_t;
				uint num2;
				ushort num3;
				if (SteamGameServerNetworking.GetP2PSessionState(playerID.steamID, ref p2PSessionState_t))
				{
					num2 = p2PSessionState_t.m_nRemoteIP;
					num3 = p2PSessionState_t.m_nRemotePort;
				}
				else
				{
					num2 = 0u;
					num3 = 0;
				}
				uint num4 = (num2 & 255u) << 24 | (num2 & 65280u) << 8 | (num2 & 16711680u) >> 8 | (num2 & 4278190080u) >> 24;
				ushort num5 = (ushort)((int)(num3 & 255) << 8 | (int)((uint)(num3 & 65280) >> 8));
				Provider.battlEyeServerRunData.pfnAddPlayer.Invoke(channels, num4, num5, playerID.playerName);
				ulong steamID = playerID.steamID.m_SteamID;
				GCHandle gchandle = GCHandle.Alloc(steamID, GCHandleType.Pinned);
				IntPtr intPtr = gchandle.AddrOfPinnedObject();
				Provider.battlEyeServerRunData.pfnReceivedPlayerGUID.Invoke(channels, intPtr, 8);
				gchandle.Free();
			}
			bytes = SteamPacker.getBytes(0, out size, new object[]
			{
				11,
				playerID.steamID,
				playerID.characterID,
				playerID.playerName,
				playerID.characterName,
				vector,
				b,
				isPro,
				isAdmin && !Provider.hideAdmins,
				channels,
				playerID.group,
				playerID.nickName,
				face,
				hair,
				beard,
				skin,
				color,
				hand,
				shirtItem,
				pantsItem,
				hatItem,
				backpackItem,
				vestItem,
				maskItem,
				glassesItem,
				skinItems,
				(byte)skillset
			});
			for (int k = 0; k < Provider.clients.Count; k++)
			{
				if (Provider.clients[k].playerID != playerID)
				{
					Provider.send(Provider.clients[k].playerID.steamID, ESteamPacket.CONNECTED, bytes, size, 0);
				}
			}
			if (Provider.onServerConnected != null)
			{
				Provider.onServerConnected(playerID.steamID);
			}
			if (CommandWindow.shouldLogJoinLeave)
			{
				CommandWindow.Log(Provider.localization.format("PlayerConnectedText", new object[]
				{
					playerID.steamID,
					playerID.playerName,
					playerID.characterName
				}));
			}
			if (Provider.pending.Count > 0 && num == 0 && Provider.clients.Count < (int)Provider.maxPlayers && Provider.pending[0].lastActive < 0f)
			{
				Provider.pending[0].lastActive = Time.realtimeSinceStartup;
				Provider.send(Provider.pending[0].playerID.steamID, ESteamPacket.VERIFY, new byte[]
				{
					3
				}, 1, 0);
			}
		}

		public static void reject(CSteamID steamID, ESteamRejection rejection)
		{
			for (int i = 0; i < Provider.pending.Count; i++)
			{
				if (Provider.pending[i].playerID.steamID == steamID)
				{
					if (rejection == ESteamRejection.AUTH_VAC_BAN)
					{
						ChatManager.say(Provider.pending[i].playerID.playerName + " was banned by VAC", Color.yellow);
					}
					else if (rejection == ESteamRejection.AUTH_PUB_BAN)
					{
						ChatManager.say(Provider.pending[i].playerID.playerName + " was banned by BattlEye", Color.yellow);
					}
					if (Provider.pending[i].inventoryResult != SteamInventoryResult_t.Invalid)
					{
						SteamGameServerInventory.DestroyResult(Provider.pending[i].inventoryResult);
						Provider.pending[i].inventoryResult = SteamInventoryResult_t.Invalid;
					}
					Provider.pending.RemoveAt(i);
					if (Provider.pending.Count > 0 && i == 0 && Provider.clients.Count < (int)Provider.maxPlayers && Provider.pending[0].lastActive < 0f)
					{
						Provider.pending[0].lastActive = Time.realtimeSinceStartup;
						Provider.send(Provider.pending[0].playerID.steamID, ESteamPacket.VERIFY, new byte[]
						{
							3
						}, 1, 0);
					}
					break;
				}
			}
			SteamGameServer.EndAuthSession(steamID);
			Provider.send(steamID, ESteamPacket.REJECTED, new byte[]
			{
				5,
				(byte)rejection
			}, 2, 0);
		}

		public static void kick(CSteamID steamID, string reason)
		{
			bool flag = false;
			for (int i = 0; i < Provider.clients.Count; i++)
			{
				if (Provider.clients[i].playerID.steamID == steamID)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				return;
			}
			int size;
			byte[] bytes = SteamPacker.getBytes(0, out size, 10, reason);
			Provider.send(steamID, ESteamPacket.KICKED, bytes, size, 0);
			if (Provider.onServerDisconnected != null)
			{
				Provider.onServerDisconnected(steamID);
			}
			SteamGameServer.EndAuthSession(steamID);
			for (int j = 0; j < Provider.clients.Count; j++)
			{
				if (Provider.clients[j].playerID.steamID == steamID)
				{
					Provider.removePlayer((byte)j);
					for (int k = 0; k < Provider.clients.Count; k++)
					{
						if (Provider.clients[k].playerID.steamID != steamID)
						{
							Provider.send(Provider.clients[k].playerID.steamID, ESteamPacket.DISCONNECTED, new byte[]
							{
								12,
								(byte)j
							}, 2, 0);
						}
					}
					break;
				}
			}
		}

		public static void ban(CSteamID steamID, string reason, uint duration)
		{
			bool flag = false;
			for (int i = 0; i < Provider.clients.Count; i++)
			{
				if (Provider.clients[i].playerID.steamID == steamID)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				return;
			}
			int size;
			byte[] bytes = SteamPacker.getBytes(0, out size, 9, reason, duration);
			Provider.send(steamID, ESteamPacket.BANNED, bytes, size, 0);
			SteamGameServer.EndAuthSession(steamID);
			for (int j = 0; j < Provider.clients.Count; j++)
			{
				if (Provider.clients[j].playerID.steamID == steamID)
				{
					if (Provider.onServerDisconnected != null)
					{
						Provider.onServerDisconnected(steamID);
					}
					Provider.removePlayer((byte)j);
					for (int k = 0; k < Provider.clients.Count; k++)
					{
						if (Provider.clients[k].playerID.steamID != steamID)
						{
							Provider.send(Provider.clients[k].playerID.steamID, ESteamPacket.DISCONNECTED, new byte[]
							{
								12,
								(byte)j
							}, 2, 0);
						}
					}
					break;
				}
			}
		}

		public static void dismiss(CSteamID steamID)
		{
			bool flag = false;
			for (int i = 0; i < Provider.clients.Count; i++)
			{
				if (Provider.clients[i].playerID.steamID == steamID)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				return;
			}
			if (Provider.onServerDisconnected != null)
			{
				Provider.onServerDisconnected(steamID);
			}
			SteamGameServer.EndAuthSession(steamID);
			for (int j = 0; j < Provider.clients.Count; j++)
			{
				if (Provider.clients[j].playerID.steamID == steamID)
				{
					if (CommandWindow.shouldLogJoinLeave)
					{
						CommandWindow.Log(Provider.localization.format("PlayerDisconnectedText", new object[]
						{
							Provider.clients[j].playerID.steamID,
							Provider.clients[j].playerID.playerName,
							Provider.clients[j].playerID.characterName
						}));
					}
					Provider.removePlayer((byte)j);
					for (int k = 0; k < Provider.clients.Count; k++)
					{
						if (Provider.clients[k].playerID.steamID != steamID)
						{
							Provider.send(Provider.clients[k].playerID.steamID, ESteamPacket.DISCONNECTED, new byte[]
							{
								12,
								(byte)j
							}, 2, 0);
						}
					}
					break;
				}
			}
		}

		private static bool verifyTicket(CSteamID steamID, byte[] ticket)
		{
			EBeginAuthSessionResult ebeginAuthSessionResult = SteamGameServer.BeginAuthSession(ticket, ticket.Length, steamID);
			return ebeginAuthSessionResult == 0;
		}

		private static void openGameServer()
		{
			if (Provider.isServer || Provider.isClient)
			{
				Debug.LogError("Failed to open game server: session already in progress.");
				return;
			}
			ESecurityMode esecurityMode = ESecurityMode.LAN;
			ESteamServerVisibility serverVisibility = Dedicator.serverVisibility;
			if (serverVisibility != ESteamServerVisibility.Internet)
			{
				if (serverVisibility == ESteamServerVisibility.LAN)
				{
					esecurityMode = ESecurityMode.LAN;
				}
			}
			else if (Provider.configData.Server.VAC_Secure)
			{
				esecurityMode = ESecurityMode.SECURE;
			}
			else
			{
				esecurityMode = ESecurityMode.INSECURE;
			}
			if (esecurityMode == ESecurityMode.INSECURE)
			{
				CommandWindow.LogWarning(Provider.localization.format("InsecureWarningText"));
			}
			try
			{
				Provider.provider.multiplayerService.serverMultiplayerService.open(Provider.ip, Provider.port, esecurityMode);
			}
			catch (Exception ex)
			{
				Debug.Log("Quit due to provider exception: " + ex.Message);
				Application.Quit();
				return;
			}
			if (Provider.configData != null && Provider.configData.Server.BattlEye_Secure && esecurityMode == ESecurityMode.SECURE)
			{
				string text = ReadWrite.PATH + "/BattlEye/BEServer_x64.so";
				if (!File.Exists(text))
				{
					text = ReadWrite.PATH + "/BattlEye/BEServer.so";
				}
				if (!File.Exists(text))
				{
					Debug.LogError("Missing BattlEye server library!");
					Application.Quit();
					return;
				}
				try
				{
					Provider.battlEyeServerHandle = BEServer.dlopen(text, 2);
					if (!(Provider.battlEyeServerHandle != IntPtr.Zero))
					{
						Debug.LogError("Failed to load BattlEye server library!");
						Application.Quit();
						return;
					}
					IntPtr ptr = BEServer.dlsym(Provider.battlEyeServerHandle, "Init");
					BEServer.BEServerInitFn beserverInitFn = Marshal.GetDelegateForFunctionPointer(ptr, typeof(BEServer.BEServerInitFn)) as BEServer.BEServerInitFn;
					if (beserverInitFn == null)
					{
						BEServer.dlclose(Provider.battlEyeServerHandle);
						Provider.battlEyeServerHandle = IntPtr.Zero;
						Debug.LogError("Failed to get BattlEye server init delegate!");
						Application.Quit();
						return;
					}
					Provider.battlEyeServerInitData = new BEServer.BESV_GAME_DATA();
					Provider.battlEyeServerInitData.pstrGameVersion = Provider.APP_NAME + " " + Provider.APP_VERSION;
					BEServer.BESV_GAME_DATA besv_GAME_DATA = Provider.battlEyeServerInitData;
					if (Provider.<>f__mg$cache3 == null)
					{
						Provider.<>f__mg$cache3 = new BEServer.BESV_GAME_DATA.PrintMessageFn(Provider.battlEyeServerPrintMessage);
					}
					besv_GAME_DATA.pfnPrintMessage = Provider.<>f__mg$cache3;
					BEServer.BESV_GAME_DATA besv_GAME_DATA2 = Provider.battlEyeServerInitData;
					if (Provider.<>f__mg$cache4 == null)
					{
						Provider.<>f__mg$cache4 = new BEServer.BESV_GAME_DATA.KickPlayerFn(Provider.battlEyeServerKickPlayer);
					}
					besv_GAME_DATA2.pfnKickPlayer = Provider.<>f__mg$cache4;
					BEServer.BESV_GAME_DATA besv_GAME_DATA3 = Provider.battlEyeServerInitData;
					if (Provider.<>f__mg$cache5 == null)
					{
						Provider.<>f__mg$cache5 = new BEServer.BESV_GAME_DATA.SendPacketFn(Provider.battlEyeServerSendPacket);
					}
					besv_GAME_DATA3.pfnSendPacket = Provider.<>f__mg$cache5;
					Provider.battlEyeServerRunData = new BEServer.BESV_BE_DATA();
					if (!beserverInitFn.Invoke(0, Provider.battlEyeServerInitData, Provider.battlEyeServerRunData))
					{
						BEServer.dlclose(Provider.battlEyeServerHandle);
						Provider.battlEyeServerHandle = IntPtr.Zero;
						Debug.LogError("Failed to call BattlEye server init!");
						Application.Quit();
						return;
					}
				}
				catch (Exception ex2)
				{
					Debug.LogError("Unhandled exception when loading BattlEye server library!");
					Debug.LogException(ex2);
					Application.Quit();
					return;
				}
			}
			CommandWindow.Log("Waiting for Steam servers...");
		}

		private static void closeGameServer()
		{
			if (!Provider.isServer)
			{
				Debug.LogError("Failed to close game server: no session in progress.");
				return;
			}
			if (Provider.onServerShutdown != null)
			{
				Provider.onServerShutdown();
			}
			Provider._isServer = false;
			Provider.provider.multiplayerService.serverMultiplayerService.close();
		}

		public static bool isFavorited
		{
			get
			{
				return Provider._isFavorited;
			}
		}

		public static bool checkFavorite(uint ip, ushort port)
		{
			bool result = false;
			for (int i = 0; i < SteamMatchmaking.GetFavoriteGameCount(); i++)
			{
				AppId_t appId_t;
				uint num;
				ushort num2;
				ushort num3;
				uint num4;
				uint num5;
				SteamMatchmaking.GetFavoriteGame(i, ref appId_t, ref num, ref num2, ref num3, ref num4, ref num5);
				if (appId_t == Provider.APP_ID && num == ip && num2 == port)
				{
					result = true;
					break;
				}
			}
			return result;
		}

		public static void updateFavorite(uint ip, ushort port, bool isFav)
		{
			if (isFav)
			{
				SteamMatchmaking.AddFavoriteGame(Provider.APP_ID, ip, port, port + 1, 1u, SteamUtils.GetServerRealTime());
			}
			else
			{
				SteamMatchmaking.RemoveFavoriteGame(Provider.APP_ID, ip, port, port + 1, 1u);
			}
		}

		public static void toggleFavorite()
		{
			if (Provider.isServer)
			{
				return;
			}
			if (Provider.isFavorited)
			{
				SteamMatchmaking.RemoveFavoriteGame(Provider.APP_ID, Provider.favoriteIP, Provider.favoritePort, Provider.favoritePort + 1, 1u);
				Provider._isFavorited = false;
			}
			else
			{
				SteamMatchmaking.AddFavoriteGame(Provider.APP_ID, Provider.favoriteIP, Provider.favoritePort, Provider.favoritePort + 1, 1u, SteamUtils.GetServerRealTime());
				Provider._isFavorited = true;
			}
		}

		private static void onPersonaStateChange(PersonaStateChange_t callback)
		{
			if (callback.m_nChangeFlags == 1 && callback.m_ulSteamID == Provider.client.m_SteamID)
			{
				Provider._clientName = SteamFriends.GetPersonaName();
			}
		}

		private static void onGameServerChangeRequested(GameServerChangeRequested_t callback)
		{
			if (Provider.isConnected)
			{
				return;
			}
			Terminal.print("onGameServerChangeRequested " + callback.m_rgchServer + " " + callback.m_rgchPassword, null, "Steam", "<color=#2784c6>Steam</color>", true);
			SteamConnectionInfo steamConnectionInfo = new SteamConnectionInfo(callback.m_rgchServer, callback.m_rgchPassword);
			Terminal.print(string.Concat(new object[]
			{
				steamConnectionInfo.ip,
				" ",
				Parser.getIPFromUInt32(steamConnectionInfo.ip),
				" ",
				steamConnectionInfo.port,
				" ",
				steamConnectionInfo.password
			}), null, "Steam", "<color=#2784c6>Steam</color>", true);
			MenuPlayConnectUI.connect(steamConnectionInfo);
		}

		private static void onGameRichPresenceJoinRequested(GameRichPresenceJoinRequested_t callback)
		{
			if (Provider.isConnected)
			{
				return;
			}
			Terminal.print("onGameRichPresenceJoinRequested " + callback.m_rgchConnect, null, "Steam", "<color=#2784c6>Steam</color>", true);
			uint newIP;
			ushort newPort;
			string newPassword;
			if (CommandLine.tryGetConnect(callback.m_rgchConnect, out newIP, out newPort, out newPassword))
			{
				SteamConnectionInfo steamConnectionInfo = new SteamConnectionInfo(newIP, newPort, newPassword);
				Terminal.print(string.Concat(new object[]
				{
					steamConnectionInfo.ip,
					" ",
					Parser.getIPFromUInt32(steamConnectionInfo.ip),
					" ",
					steamConnectionInfo.port,
					" ",
					steamConnectionInfo.password
				}), null, "Steam", "<color=#2784c6>Steam</color>", true);
				MenuPlayConnectUI.connect(steamConnectionInfo);
			}
		}

		public static float lastNet { get; private set; }

		public static float net
		{
			get
			{
				return Provider.offsetNet + Time.realtimeSinceStartup - Provider.lastNet;
			}
		}

		public static float ping
		{
			get
			{
				return Provider._ping;
			}
		}

		private static void lag(float value)
		{
			Provider._ping = value;
			for (int i = Provider.pings.Length - 1; i > 0; i--)
			{
				Provider.pings[i] = Provider.pings[i - 1];
				if (Provider.pings[i] > 0.001f)
				{
					Provider._ping += Provider.pings[i];
				}
			}
			Provider._ping /= (float)Provider.pings.Length;
			Provider.pings[0] = value;
		}

		private static byte[] openTicket()
		{
			if (Provider.ticketHandle != HAuthTicket.Invalid)
			{
				return null;
			}
			byte[] array = new byte[1024];
			uint num;
			Provider.ticketHandle = SteamUser.GetAuthSessionTicket(array, array.Length, ref num);
			if (num == 0u)
			{
				return null;
			}
			byte[] array2 = new byte[num];
			Buffer.BlockCopy(array, 0, array2, 0, (int)num);
			return array2;
		}

		private static void closeTicket()
		{
			if (Provider.ticketHandle == HAuthTicket.Invalid)
			{
				return;
			}
			SteamUser.CancelAuthTicket(Provider.ticketHandle);
			Provider.ticketHandle = HAuthTicket.Invalid;
		}

		public static IProvider provider { get; protected set; }

		public static bool isInitialized
		{
			get
			{
				return Provider._isInitialized;
			}
		}

		public static uint time
		{
			get
			{
				return Provider._time + (uint)(Time.realtimeSinceStartup - Provider.timeOffset);
			}
			set
			{
				Provider._time = value;
				Provider.timeOffset = (uint)Time.realtimeSinceStartup;
			}
		}

		public IEnumerator close(CSteamID steamID)
		{
			yield return new WaitForSeconds(0.1f);
			if (Provider.isServer)
			{
				SteamGameServerNetworking.CloseP2PSessionWithUser(steamID);
			}
			else
			{
				SteamNetworking.CloseP2PSessionWithUser(steamID);
			}
			yield break;
		}

		private void exit()
		{
			Application.Quit();
		}

		private static void onAPIWarningMessage(int severity, StringBuilder warning)
		{
			CommandWindow.LogWarning("Steam API Warning Message:");
			CommandWindow.LogWarning("Severity: " + severity);
			CommandWindow.LogWarning("Warning: " + warning);
		}

		private void updateDebug()
		{
			Provider.debugUpdates++;
			if (Time.realtimeSinceStartup - Provider.debugLastUpdate > 1f)
			{
				Provider.debugUPS = (int)((float)Provider.debugUpdates / (Time.realtimeSinceStartup - Provider.debugLastUpdate));
				Provider.debugLastUpdate = Time.realtimeSinceStartup;
				Provider.debugUpdates = 0;
			}
		}

		private void tickDebug()
		{
			Provider.debugTicks++;
			if (Time.realtimeSinceStartup - Provider.debugLastTick > 1f)
			{
				Provider.debugTPS = (int)((float)Provider.debugTicks / (Time.realtimeSinceStartup - Provider.debugLastTick));
				Provider.debugLastTick = Time.realtimeSinceStartup;
				Provider.debugTicks = 0;
			}
		}

		private IEnumerator downloadIcon()
		{
			WWW request = new WWW(Provider.iconQueryURL);
			yield return request;
			if (string.IsNullOrEmpty(request.error))
			{
				Texture2D texture = request.texture;
				texture.name = "WebImage";
				texture.filterMode = 2;
				if (Provider.onIconQueryRefreshed != null)
				{
					Provider.onIconQueryRefreshed(texture);
				}
			}
			yield break;
		}

		public static void refreshIcon(string url)
		{
			Provider.iconQueryURL = url;
			Provider.steam.StartCoroutine("downloadIcon");
		}

		private void Update()
		{
			if (!Provider.isInitialized)
			{
				return;
			}
			if (Provider.battlEyeClientHandle != IntPtr.Zero && Provider.battlEyeClientRunData != null && Provider.battlEyeClientRunData.pfnRun != null)
			{
				Provider.battlEyeClientRunData.pfnRun.Invoke();
			}
			if (Provider.configData != null && Provider.configData.Server.BattlEye_Secure && Provider.battlEyeServerHandle != IntPtr.Zero && Provider.battlEyeServerRunData != null && Provider.battlEyeServerRunData.pfnRun != null)
			{
				Provider.battlEyeServerRunData.pfnRun.Invoke();
			}
			if (Provider.isConnected)
			{
				Provider.listen();
			}
			this.updateDebug();
			Provider.provider.update();
			if (!Dedicator.isDedicated && Provider.hasDiscord)
			{
				DiscordRpc.RunCallbacks();
			}
			if (Provider.countShutdownTimer > 0)
			{
				if (Time.realtimeSinceStartup - Provider.lastTimerMessage > 1f)
				{
					Provider.lastTimerMessage = Time.realtimeSinceStartup;
					Provider.countShutdownTimer--;
					if (Provider.countShutdownTimer == 300 || Provider.countShutdownTimer == 60 || Provider.countShutdownTimer == 30 || Provider.countShutdownTimer == 15 || Provider.countShutdownTimer == 3 || Provider.countShutdownTimer == 2 || Provider.countShutdownTimer == 1)
					{
						ChatManager.say(Provider.localization.format("Shutdown", new object[]
						{
							Provider.countShutdownTimer
						}), ChatManager.welcomeColor);
					}
				}
			}
			else if (Provider.countShutdownTimer == 0)
			{
				Provider.countShutdownTimer = -1;
				for (int i = 0; i < Provider.clients.Count; i++)
				{
					SteamGameServer.EndAuthSession(Provider.clients[i].playerID.steamID);
					Provider.send(Provider.clients[i].playerID.steamID, ESteamPacket.SHUTDOWN, new byte[1], 1, 0);
				}
				base.Invoke("exit", 0.5f);
			}
		}

		private void FixedUpdate()
		{
			if (!Provider.isInitialized)
			{
				return;
			}
			this.tickDebug();
		}

		public void awake()
		{
			if (ReadWrite.fileExists("/Status.json", false, true))
			{
				try
				{
					Provider._statusData = ReadWrite.deserializeJSON<StatusData>("/Status.json", false, true);
				}
				catch
				{
					Provider._statusData = null;
				}
				if (Provider.statusData == null)
				{
					Provider._statusData = new StatusData();
				}
			}
			else
			{
				Provider._statusData = new StatusData();
			}
			SDG.Framework.Modules.Module moduleByName = ModuleHook.getModuleByName("Unturned");
			if (moduleByName != null)
			{
				Provider.APP_VERSION = moduleByName.config.Version;
			}
			else
			{
				Provider.APP_VERSION = "1.0.0.0";
			}
			Terminal.print("App: " + Provider.APP_NAME + " " + Provider.APP_VERSION, null, "Steam", "<color=#2784c6>Steam</color>", true);
			if (Provider.isInitialized)
			{
				Object.Destroy(base.gameObject);
				return;
			}
			Provider._isInitialized = true;
			Object.DontDestroyOnLoad(base.gameObject);
			Provider.steam = this;
			Delegate onLevelLoaded = Level.onLevelLoaded;
			if (Provider.<>f__mg$cache6 == null)
			{
				Provider.<>f__mg$cache6 = new LevelLoaded(Provider.onLevelLoaded);
			}
			Level.onLevelLoaded = (LevelLoaded)Delegate.Combine(onLevelLoaded, Provider.<>f__mg$cache6);
			if (Dedicator.isDedicated)
			{
				try
				{
					Provider.provider = new SteamworksProvider(new SteamworksAppInfo(Provider.APP_ID.m_AppId, Provider.APP_NAME, Provider.APP_VERSION, true));
					Provider.provider.intialize();
				}
				catch (Exception ex)
				{
					Debug.Log("Quit due to provider exception: " + ex.Message);
					Application.Quit();
					return;
				}
				if (!CommandLine.tryGetLanguage(out Provider._language, out Provider._path))
				{
					Provider._path = ReadWrite.PATH + "/Localization/";
					Provider._language = "English";
				}
				Provider.localization = Localization.read("/Server/ServerConsole.dat");
				if (Provider.<>f__mg$cache7 == null)
				{
					Provider.<>f__mg$cache7 = new Callback<P2PSessionConnectFail_t>.DispatchDelegate(Provider.onP2PSessionConnectFail);
				}
				Provider.p2pSessionConnectFail = Callback<P2PSessionConnectFail_t>.CreateGameServer(Provider.<>f__mg$cache7);
				if (Provider.<>f__mg$cache8 == null)
				{
					Provider.<>f__mg$cache8 = new Callback<ValidateAuthTicketResponse_t>.DispatchDelegate(Provider.onValidateAuthTicketResponse);
				}
				Provider.validateAuthTicketResponse = Callback<ValidateAuthTicketResponse_t>.CreateGameServer(Provider.<>f__mg$cache8);
				if (Provider.<>f__mg$cache9 == null)
				{
					Provider.<>f__mg$cache9 = new Callback<GSClientGroupStatus_t>.DispatchDelegate(Provider.onClientGroupStatus);
				}
				Provider.clientGroupStatus = Callback<GSClientGroupStatus_t>.CreateGameServer(Provider.<>f__mg$cache9);
				Provider._isPro = true;
				CommandWindow.Log(Provider.APP_VERSION);
				Provider.maxPlayers = 8;
				Provider.queueSize = 8;
				Provider.serverName = "Unturned";
				Provider.serverPassword = string.Empty;
				Provider.ip = 0u;
				Provider.port = 27015;
				Provider.map = "PEI";
				Provider.isPvP = true;
				Provider.isWhitelisted = false;
				Provider.hideAdmins = false;
				Provider.hasCheats = false;
				Provider.filterName = false;
				Provider.mode = EGameMode.NORMAL;
				Provider.isGold = false;
				Provider.gameMode = null;
				Provider.selectedGameModeName = null;
				Provider.cameraMode = ECameraMode.FIRST;
				Commander.init();
				SteamWhitelist.load();
				SteamBlacklist.load();
				SteamAdminlist.load();
				string[] commands = CommandLine.getCommands();
				for (int i = 0; i < commands.Length; i++)
				{
					Commander.execute(CSteamID.Nil, commands[i]);
				}
				if (ServerSavedata.fileExists("/Server/Commands.dat"))
				{
					FileStream fileStream = null;
					StreamReader streamReader = null;
					try
					{
						fileStream = new FileStream(ReadWrite.PATH + "/Servers/" + Provider.serverID + "/Server/Commands.dat", FileMode.Open, FileAccess.Read, FileShare.Read);
						streamReader = new StreamReader(fileStream);
						string command;
						while ((command = streamReader.ReadLine()) != null)
						{
							Commander.execute(CSteamID.Nil, command);
						}
					}
					finally
					{
						if (fileStream != null)
						{
							fileStream.Close();
						}
						if (streamReader != null)
						{
							streamReader.Close();
						}
					}
				}
				else
				{
					Data data = new Data();
					ServerSavedata.writeData("/Server/Commands.dat", data);
				}
				if (!ServerSavedata.folderExists("/Bundles"))
				{
					ServerSavedata.createFolder("/Bundles");
				}
				if (!ServerSavedata.folderExists("/Maps"))
				{
					ServerSavedata.createFolder("/Maps");
				}
				if (!ServerSavedata.folderExists("/Workshop/Content"))
				{
					ServerSavedata.createFolder("/Workshop/Content");
				}
				if (!ServerSavedata.folderExists("/Workshop/Maps"))
				{
					ServerSavedata.createFolder("/Workshop/Maps");
				}
				if (ServerSavedata.fileExists("/Config.json"))
				{
					try
					{
						Provider._configData = ServerSavedata.deserializeJSON<ConfigData>("/Config.json");
					}
					catch
					{
						Provider._configData = null;
					}
					if (Provider.configData == null)
					{
						Provider._configData = new ConfigData();
					}
				}
				else
				{
					Provider._configData = new ConfigData();
				}
				ServerSavedata.serializeJSON<ConfigData>("/Config.json", Provider.configData);
				switch (Provider.mode)
				{
				case EGameMode.EASY:
					Provider._modeConfigData = Provider.configData.Easy;
					break;
				case EGameMode.NORMAL:
					Provider._modeConfigData = Provider.configData.Normal;
					break;
				case EGameMode.HARD:
					Provider._modeConfigData = Provider.configData.Hard;
					break;
				default:
					Provider._modeConfigData = new ModeConfigData(Provider.mode);
					break;
				}
				if (Provider.<>f__mg$cacheA == null)
				{
					Provider.<>f__mg$cacheA = new GUIDTableMappingAddedHandler(Provider.onMappingAdded);
				}
				GUIDTable.mappingAdded += Provider.<>f__mg$cacheA;
				IServerMultiplayerService serverMultiplayerService = Provider.provider.multiplayerService.serverMultiplayerService;
				if (Provider.<>f__mg$cacheB == null)
				{
					Provider.<>f__mg$cacheB = new ServerMultiplayerServiceReadyHandler(Provider.handleServerReady);
				}
				serverMultiplayerService.ready += Provider.<>f__mg$cacheB;
				if (Provider.<>f__mg$cacheC == null)
				{
					Provider.<>f__mg$cacheC = new DedicatedUGCInstalledHandler(Provider.onDedicatedUGCInstalled);
				}
				DedicatedUGC.installed += Provider.<>f__mg$cacheC;
				return;
			}
			try
			{
				Provider.provider = new SteamworksProvider(new SteamworksAppInfo(Provider.APP_ID.m_AppId, Provider.APP_NAME, Provider.APP_VERSION, false));
				Provider.provider.intialize();
			}
			catch (Exception ex2)
			{
				Debug.Log("Quit due to provider exception: " + ex2.Message);
				Application.Quit();
				return;
			}
			Provider.apiWarningMessageHook = new SteamAPIWarningMessageHook_t(Provider.onAPIWarningMessage);
			SteamUtils.SetWarningMessageHook(Provider.apiWarningMessageHook);
			Provider._time = SteamUtils.GetServerRealTime();
			if (Provider.<>f__mg$cacheD == null)
			{
				Provider.<>f__mg$cacheD = new Callback<PersonaStateChange_t>.DispatchDelegate(Provider.onPersonaStateChange);
			}
			Provider.personaStateChange = Callback<PersonaStateChange_t>.Create(Provider.<>f__mg$cacheD);
			if (Provider.<>f__mg$cacheE == null)
			{
				Provider.<>f__mg$cacheE = new Callback<GameServerChangeRequested_t>.DispatchDelegate(Provider.onGameServerChangeRequested);
			}
			Provider.gameServerChangeRequested = Callback<GameServerChangeRequested_t>.Create(Provider.<>f__mg$cacheE);
			if (Provider.<>f__mg$cacheF == null)
			{
				Provider.<>f__mg$cacheF = new Callback<GameRichPresenceJoinRequested_t>.DispatchDelegate(Provider.onGameRichPresenceJoinRequested);
			}
			Provider.gameRichPresenceJoinRequested = Callback<GameRichPresenceJoinRequested_t>.Create(Provider.<>f__mg$cacheF);
			Provider._user = SteamUser.GetSteamID();
			Provider._client = Provider.user;
			Provider._clientHash = Hash.SHA1(Provider.client);
			Provider._clientName = SteamFriends.GetPersonaName();
			Provider.provider.statisticsService.userStatisticsService.requestStatistics();
			Provider.provider.statisticsService.globalStatisticsService.requestStatistics();
			Provider.provider.workshopService.refreshUGC();
			Provider.provider.workshopService.refreshPublished();
			Provider._isPro = SteamApps.BIsSubscribedApp(Provider.PRO_ID);
			Provider.isLoadingInventory = true;
			SteamInventory.GrantPromoItems(ref Provider.provider.economyService.promoResult);
			if (!CommandLine.tryGetLanguage(out Provider._language, out Provider._path))
			{
				string steamUILanguage = SteamUtils.GetSteamUILanguage();
				Provider._language = steamUILanguage.Substring(0, 1).ToUpper() + steamUILanguage.Substring(1, steamUILanguage.Length - 1).ToLower();
				bool flag = false;
				for (int j = 0; j < Provider.provider.workshopService.ugc.Count; j++)
				{
					SteamContent steamContent = Provider.provider.workshopService.ugc[j];
					if (steamContent.type == ESteamUGCType.LOCALIZATION && ReadWrite.folderExists(steamContent.path + "/" + steamUILanguage, false))
					{
						Provider._path = steamContent.path + "/";
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					Provider._path = ReadWrite.PATH + "/Localization/";
					if (!ReadWrite.folderExists("/Localization/" + Provider.language))
					{
						Provider._language = "English";
					}
				}
			}
			Provider.provider.economyService.loadTranslationEconInfo();
			Provider.localization = Localization.read("/Server/ServerConsole.dat");
			Provider.isDiscordReady = false;
			Provider.hasDiscord = (Environment.CommandLine.IndexOf("-Discord", StringComparison.OrdinalIgnoreCase) != -1);
			if (Provider.hasDiscord)
			{
				Provider.discordEventHandlers = default(DiscordRpc.EventHandlers);
				if (Provider.<>f__mg$cache10 == null)
				{
					Provider.<>f__mg$cache10 = new DiscordRpc.ReadyCallback(Provider.handleDiscordReady);
				}
				Provider.discordEventHandlers.readyCallback = Provider.<>f__mg$cache10;
				Delegate disconnectedCallback = Provider.discordEventHandlers.disconnectedCallback;
				if (Provider.<>f__mg$cache11 == null)
				{
					Provider.<>f__mg$cache11 = new DiscordRpc.DisconnectedCallback(Provider.handleDiscordDisconnected);
				}
				Provider.discordEventHandlers.disconnectedCallback = (DiscordRpc.DisconnectedCallback)Delegate.Combine(disconnectedCallback, Provider.<>f__mg$cache11);
				Delegate errorCallback = Provider.discordEventHandlers.errorCallback;
				if (Provider.<>f__mg$cache12 == null)
				{
					Provider.<>f__mg$cache12 = new DiscordRpc.ErrorCallback(Provider.handleDiscordError);
				}
				Provider.discordEventHandlers.errorCallback = (DiscordRpc.ErrorCallback)Delegate.Combine(errorCallback, Provider.<>f__mg$cache12);
				Delegate joinCallback = Provider.discordEventHandlers.joinCallback;
				if (Provider.<>f__mg$cache13 == null)
				{
					Provider.<>f__mg$cache13 = new DiscordRpc.JoinCallback(Provider.handleDiscordJoin);
				}
				Provider.discordEventHandlers.joinCallback = (DiscordRpc.JoinCallback)Delegate.Combine(joinCallback, Provider.<>f__mg$cache13);
				Delegate spectateCallback = Provider.discordEventHandlers.spectateCallback;
				if (Provider.<>f__mg$cache14 == null)
				{
					Provider.<>f__mg$cache14 = new DiscordRpc.SpectateCallback(Provider.handleDiscordSpectate);
				}
				Provider.discordEventHandlers.spectateCallback = (DiscordRpc.SpectateCallback)Delegate.Combine(spectateCallback, Provider.<>f__mg$cache14);
				DiscordRpc.Initialize("351821143981817856", ref Provider.discordEventHandlers, true);
			}
			Provider.updateRichPresence();
			Provider._configData = new ConfigData();
			Provider._modeConfigData = Provider.configData.Normal;
			if (ReadWrite.fileExists("/Preferences.json", false, true))
			{
				try
				{
					Provider._preferenceData = ReadWrite.deserializeJSON<PreferenceData>("/Preferences.json", false, true);
				}
				catch
				{
					Provider._preferenceData = null;
				}
				if (Provider.preferenceData == null)
				{
					Provider._preferenceData = new PreferenceData();
				}
			}
			else
			{
				Provider._preferenceData = new PreferenceData();
			}
			ReadWrite.serializeJSON<PreferenceData>("/Preferences.json", false, true, Provider.preferenceData);
			if (ReadWrite.fileExists("/StreamerNames.json", false, true))
			{
				try
				{
					Provider.streamerNames = ReadWrite.deserializeJSON<List<string>>("/StreamerNames.json", false, true);
				}
				catch
				{
					Provider.streamerNames = null;
				}
				if (Provider.streamerNames == null)
				{
					Provider.streamerNames = new List<string>();
				}
			}
			else
			{
				Provider.streamerNames = new List<string>();
			}
		}

		public void start()
		{
			if (Provider.<>f__mg$cache15 == null)
			{
				Provider.<>f__mg$cache15 = new TranslationRegisteredHandler(Provider.handleTranslationRegistered);
			}
			Translator.translationRegistered += Provider.<>f__mg$cache15;
			if (Dedicator.isDedicated)
			{
				if (Translator.isOriginLanguage(Translator.language) && !Translator.isOriginLanguage(Provider.language))
				{
					Translator.language = Provider.language;
				}
				Translator.registerTranslationDirectory(ReadWrite.PATH + "/Translations");
				return;
			}
			if (Translator.isOriginLanguage(Provider.language))
			{
				if (File.Exists(ReadWrite.PATH + "/Cloud/Translations.config"))
				{
					using (StreamReader streamReader = new StreamReader(ReadWrite.PATH + "/Cloud/Translations.config"))
					{
						IFormattedFileReader formattedFileReader = new KeyValueTableReader(streamReader);
						if (formattedFileReader != null)
						{
							string text = formattedFileReader.readValue<string>("Language");
							if (!string.IsNullOrEmpty(text))
							{
								Translator.language = text;
							}
						}
					}
				}
				else if (!Translator.isOriginLanguage(Provider.language))
				{
					Translator.language = Provider.language;
				}
			}
			Translator.registerTranslationDirectory(ReadWrite.PATH + "/Translations");
			if (Provider.<>f__mg$cache16 == null)
			{
				Provider.<>f__mg$cache16 = new LanguageChangedHandler(Provider.handleLanguageChanged);
			}
			Translator.languageChanged += Provider.<>f__mg$cache16;
			if (Provider.provider.workshopService.ugc != null && Provider.provider.workshopService.ugc != null)
			{
				for (int i = 0; i < Provider.provider.workshopService.ugc.Count; i++)
				{
					SteamContent steamContent = Provider.provider.workshopService.ugc[i];
					if (Directory.Exists(steamContent.path + "/Translations"))
					{
						Translator.registerTranslationDirectory(steamContent.path + "/Translations");
					}
					if (Directory.Exists(steamContent.path + "/Content"))
					{
						Assets.searchForAndLoadContent(steamContent.path + "/Content");
					}
				}
			}
		}

		private void OnApplicationQuit()
		{
			if (!Dedicator.isDedicated && !Translator.isOriginLanguage(Translator.language))
			{
				string path = ReadWrite.PATH + "/Cloud/Translations.config";
				string directoryName = Path.GetDirectoryName(path);
				if (!Directory.Exists(directoryName))
				{
					Directory.CreateDirectory(directoryName);
				}
				using (StreamWriter streamWriter = new StreamWriter(path))
				{
					IFormattedFileWriter formattedFileWriter = new KeyValueTableWriter(streamWriter);
					formattedFileWriter.writeKey("Language");
					formattedFileWriter.writeValue<string>(Translator.language);
				}
			}
			if (!Provider.isInitialized)
			{
				return;
			}
			if (!Dedicator.isDedicated && Provider.hasDiscord)
			{
				DiscordRpc.Shutdown();
			}
			if (!Provider.isServer && Provider.isPvP && Provider.clients.Count > 1 && Player.player != null && !Player.player.movement.isSafe && !Player.player.life.isDead)
			{
				Application.CancelQuit();
				return;
			}
			Provider.disconnect();
			Provider.provider.shutdown();
		}

		public static readonly string STEAM_IC = "Steam";

		public static readonly string STEAM_DC = "<color=#2784c6>Steam</color>";

		public static readonly AppId_t APP_ID = new AppId_t(304930u);

		public static readonly AppId_t PRO_ID = new AppId_t(306460u);

		public static readonly string APP_NAME = "Unturned";

		public static readonly string APP_AUTHOR = "Nelson Sexton";

		public static readonly int CLIENT_TIMEOUT = 30;

		private static readonly float CHECKRATE = 1f;

		private static string _language;

		private static string _path;

		public static Local localization;

		private static IntPtr battlEyeClientHandle = IntPtr.Zero;

		private static BEClient.BECL_GAME_DATA battlEyeClientInitData = null;

		private static BEClient.BECL_BE_DATA battlEyeClientRunData = null;

		private static bool battlEyeHasRequiredRestart = false;

		private static IntPtr battlEyeServerHandle = IntPtr.Zero;

		private static BEServer.BESV_GAME_DATA battlEyeServerInitData = null;

		private static BEServer.BESV_BE_DATA battlEyeServerRunData = null;

		public static string DISCORD_IC = "Discord";

		public static string DISCORD_DC = "<color=#7289da>Discord</color>";

		private static DiscordRpc.EventHandlers discordEventHandlers;

		private static DiscordRpc.RichPresence discordRichPresence;

		private static bool hasDiscord;

		private static bool isDiscordReady;

		private static uint discordJoinIP;

		private static ushort discordJoinPort;

		private static readonly string DISCORD_PASSWORD_HASH = "V_PX!mPbM8r&u+]+ziA:]4kA";

		private static readonly string DISCORD_SALT_KEY = "t]qL#3E%Et6KmP8:2+:u%d!M";

		private static readonly string DISCORD_VI_KEY = "h2_),}>_M-JVdW9b";

		private static uint _bytesSent;

		private static uint _bytesReceived;

		private static uint _packetsSent;

		private static uint _packetsReceived;

		private static SteamServerInfo _currentServerInfo;

		private static CSteamID _server;

		private static CSteamID _client;

		private static CSteamID _user;

		private static byte[] _clientHash;

		private static string _clientName;

		private static List<SteamPlayer> _clients;

		public static List<SteamPending> pending;

		private static bool _isServer;

		private static bool _isClient;

		private static bool _isPro;

		private static bool _isConnected;

		private static bool isTesting;

		public static List<ulong> serverWorkshopFileIDs = new List<ulong>();

		public static bool isLoadingUGC;

		public static bool isLoadingInventory;

		private static int _channels = 1;

		public static ESteamConnectionFailureInfo _connectionFailureInfo;

		private static string _connectionFailureReason;

		private static uint _connectionFailureDuration;

		private static List<SteamChannel> _receivers;

		private static byte[] buffer = new byte[Block.BUFFER_SIZE];

		private static List<SDG.Framework.Modules.Module> critMods = new List<SDG.Framework.Modules.Module>();

		private static StringBuilder modBuilder = new StringBuilder();

		private static int countShutdownTimer = -1;

		private static float lastTimerMessage;

		private static bool isServerConnectedToSteam;

		private static bool isDedicatedUGCInstalled;

		public static Provider.ServerConnected onServerConnected;

		public static Provider.ServerDisconnected onServerDisconnected;

		public static Provider.ServerHosted onServerHosted;

		public static Provider.ServerShutdown onServerShutdown;

		private static Callback<P2PSessionConnectFail_t> p2pSessionConnectFail;

		public static Provider.CheckValid onCheckValid;

		private static Callback<ValidateAuthTicketResponse_t> validateAuthTicketResponse;

		private static Callback<GSClientGroupStatus_t> clientGroupStatus;

		private static byte _maxPlayers;

		public static byte queueSize;

		private static byte _queuePosition;

		public static Provider.QueuePositionUpdated onQueuePositionUpdated;

		private static string _serverName;

		public static uint ip;

		public static ushort port;

		private static byte[] _serverPasswordHash;

		private static string _serverPassword;

		public static string map;

		public static bool isPvP;

		public static bool isWhitelisted;

		public static bool hideAdmins;

		public static bool hasCheats;

		public static bool filterName;

		public static EGameMode mode;

		public static bool isGold;

		public static GameMode gameMode;

		public static string selectedGameModeName;

		public static ECameraMode cameraMode;

		private static StatusData _statusData;

		private static PreferenceData _preferenceData;

		private static ConfigData _configData;

		private static ModeConfigData _modeConfigData;

		private static uint favoriteIP;

		private static ushort favoritePort;

		private static bool _isFavorited;

		public static Provider.ClientConnected onClientConnected;

		public static Provider.ClientDisconnected onClientDisconnected;

		public static Provider.EnemyConnected onEnemyConnected;

		public static Provider.EnemyDisconnected onEnemyDisconnected;

		private static Callback<PersonaStateChange_t> personaStateChange;

		private static Callback<GameServerChangeRequested_t> gameServerChangeRequested;

		private static Callback<GameRichPresenceJoinRequested_t> gameRichPresenceJoinRequested;

		private static HAuthTicket ticketHandle = HAuthTicket.Invalid;

		private static float lastCheck;

		private static float lastPing;

		private static float offsetNet;

		public static readonly float EPSILON = 0.01f;

		public static readonly float UPDATE_TIME = 0.08f;

		public static readonly float UPDATE_DELAY = 0.1f;

		public static readonly float UPDATE_DISTANCE = 0.01f;

		public static readonly uint UPDATES = 1u;

		public static readonly float LERP = 3f;

		private static float[] pings;

		private static float _ping;

		private static Provider steam;

		private static bool _isInitialized;

		private static uint timeOffset;

		private static uint _time;

		private static SteamAPIWarningMessageHook_t apiWarningMessageHook;

		private static int debugUpdates;

		public static int debugUPS;

		private static float debugLastUpdate;

		private static int debugTicks;

		public static int debugTPS;

		private static float debugLastTick;

		public static Provider.IconQueryRefreshed onIconQueryRefreshed;

		private static string iconQueryURL;

		[CompilerGenerated]
		private static BEClient.BECL_GAME_DATA.PrintMessageFn <>f__mg$cache0;

		[CompilerGenerated]
		private static BEClient.BECL_GAME_DATA.RequestRestartFn <>f__mg$cache1;

		[CompilerGenerated]
		private static BEClient.BECL_GAME_DATA.SendPacketFn <>f__mg$cache2;

		[CompilerGenerated]
		private static BEServer.BESV_GAME_DATA.PrintMessageFn <>f__mg$cache3;

		[CompilerGenerated]
		private static BEServer.BESV_GAME_DATA.KickPlayerFn <>f__mg$cache4;

		[CompilerGenerated]
		private static BEServer.BESV_GAME_DATA.SendPacketFn <>f__mg$cache5;

		[CompilerGenerated]
		private static LevelLoaded <>f__mg$cache6;

		[CompilerGenerated]
		private static Callback<P2PSessionConnectFail_t>.DispatchDelegate <>f__mg$cache7;

		[CompilerGenerated]
		private static Callback<ValidateAuthTicketResponse_t>.DispatchDelegate <>f__mg$cache8;

		[CompilerGenerated]
		private static Callback<GSClientGroupStatus_t>.DispatchDelegate <>f__mg$cache9;

		[CompilerGenerated]
		private static GUIDTableMappingAddedHandler <>f__mg$cacheA;

		[CompilerGenerated]
		private static ServerMultiplayerServiceReadyHandler <>f__mg$cacheB;

		[CompilerGenerated]
		private static DedicatedUGCInstalledHandler <>f__mg$cacheC;

		[CompilerGenerated]
		private static Callback<PersonaStateChange_t>.DispatchDelegate <>f__mg$cacheD;

		[CompilerGenerated]
		private static Callback<GameServerChangeRequested_t>.DispatchDelegate <>f__mg$cacheE;

		[CompilerGenerated]
		private static Callback<GameRichPresenceJoinRequested_t>.DispatchDelegate <>f__mg$cacheF;

		[CompilerGenerated]
		private static DiscordRpc.ReadyCallback <>f__mg$cache10;

		[CompilerGenerated]
		private static DiscordRpc.DisconnectedCallback <>f__mg$cache11;

		[CompilerGenerated]
		private static DiscordRpc.ErrorCallback <>f__mg$cache12;

		[CompilerGenerated]
		private static DiscordRpc.JoinCallback <>f__mg$cache13;

		[CompilerGenerated]
		private static DiscordRpc.SpectateCallback <>f__mg$cache14;

		[CompilerGenerated]
		private static TranslationRegisteredHandler <>f__mg$cache15;

		[CompilerGenerated]
		private static LanguageChangedHandler <>f__mg$cache16;

		public delegate void ServerConnected(CSteamID steamID);

		public delegate void ServerDisconnected(CSteamID steamID);

		public delegate void ServerHosted();

		public delegate void ServerShutdown();

		public delegate void CheckValid(ValidateAuthTicketResponse_t callback, ref bool isValid);

		public delegate void QueuePositionUpdated();

		public delegate void ClientConnected();

		public delegate void ClientDisconnected();

		public delegate void EnemyConnected(SteamPlayer player);

		public delegate void EnemyDisconnected(SteamPlayer player);

		public delegate void IconQueryRefreshed(Texture2D icon);
	}
}
