using System;
using System.Collections.Generic;
using SDG.Framework.Debug;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	public class ChatManager : SteamCaller
	{
		public static ChatManager instance
		{
			get
			{
				return ChatManager.manager;
			}
		}

		public static Chat[] chat
		{
			get
			{
				return ChatManager._chat;
			}
		}

		public static string replace(string text, int index, int count, char mask)
		{
			string text2 = text.Substring(0, index);
			for (int i = 0; i < count; i++)
			{
				text2 += mask;
			}
			return text2 + text.Substring(index + count, text.Length - index - count);
		}

		public static string filter(string text)
		{
			string text2 = text.ToLower();
			if (text.Length > 0)
			{
				bool flag = text.IndexOf(' ') != -1;
				for (int i = 0; i < ChatManager.SWEARS.Length; i++)
				{
					string text3 = ChatManager.SWEARS[i];
					int num = text2.IndexOf(text3, 0);
					while (num != -1)
					{
						if (!flag || ((num == 0 || !char.IsLetterOrDigit(text2[num - 1])) && (num == text2.Length - text3.Length || !char.IsLetterOrDigit(text2[num + text3.Length]))))
						{
							text2 = ChatManager.replace(text2, num, text3.Length, '#');
							text = ChatManager.replace(text, num, text3.Length, '#');
							num = text2.IndexOf(text3, num);
						}
						else
						{
							num = text2.IndexOf(text3, num + 1);
						}
					}
				}
			}
			return text;
		}

		public static void list(CSteamID steamID, EChatMode mode, Color color, string text)
		{
			text = text.Trim();
			if (OptionsSettings.filter)
			{
				text = ChatManager.filter(text);
			}
			if (OptionsSettings.streamer)
			{
				color = Color.white;
			}
			SteamPlayer steamPlayer = null;
			string text2;
			if (steamID == CSteamID.Nil)
			{
				text2 = Provider.localization.format("Say");
			}
			else
			{
				steamPlayer = PlayerTool.getSteamPlayer(steamID);
				if (steamPlayer == null)
				{
					return;
				}
				if (!OptionsSettings.chatText && steamPlayer.playerID.steamID != Provider.client)
				{
					return;
				}
				if (steamPlayer.player.quests.isMemberOfSameGroupAs(Player.player))
				{
					if (steamPlayer.playerID.nickName != string.Empty && steamPlayer.playerID.steamID != Provider.client)
					{
						text2 = steamPlayer.playerID.nickName;
					}
					else
					{
						text2 = steamPlayer.playerID.characterName;
					}
				}
				else
				{
					text2 = steamPlayer.playerID.characterName;
				}
			}
			for (int i = ChatManager.chat.Length - 1; i > 0; i--)
			{
				if (ChatManager.chat[i - 1] != null)
				{
					if (ChatManager.chat[i] == null)
					{
						ChatManager.chat[i] = new Chat(ChatManager.chat[i - 1].player, ChatManager.chat[i - 1].mode, ChatManager.chat[i - 1].color, ChatManager.chat[i - 1].speaker, ChatManager.chat[i - 1].text);
					}
					else
					{
						ChatManager.chat[i].player = ChatManager.chat[i - 1].player;
						ChatManager.chat[i].mode = ChatManager.chat[i - 1].mode;
						ChatManager.chat[i].color = ChatManager.chat[i - 1].color;
						ChatManager.chat[i].speaker = ChatManager.chat[i - 1].speaker;
						ChatManager.chat[i].text = ChatManager.chat[i - 1].text;
					}
				}
			}
			if (ChatManager.chat[0] == null)
			{
				ChatManager.chat[0] = new Chat(steamPlayer, mode, color, text2, text);
			}
			else
			{
				ChatManager.chat[0].player = steamPlayer;
				ChatManager.chat[0].mode = mode;
				ChatManager.chat[0].color = color;
				ChatManager.chat[0].speaker = text2;
				ChatManager.chat[0].text = text;
			}
			if (ChatManager.onListed != null)
			{
				ChatManager.onListed();
			}
		}

		public static bool process(SteamPlayer player, string text)
		{
			bool flag = false;
			bool result = true;
			string a = text.Substring(0, 1);
			if ((a == "@" || a == "/") && player.isAdmin)
			{
				flag = true;
				result = false;
			}
			if (ChatManager.onCheckPermissions != null)
			{
				ChatManager.onCheckPermissions(player, text, ref flag, ref result);
			}
			if (flag)
			{
				Commander.execute(player.playerID.steamID, text.Substring(1));
			}
			return result;
		}

		[SteamCall]
		public void tellVoteStart(CSteamID steamID, CSteamID origin, CSteamID target, byte votesNeeded)
		{
			if (base.channel.checkServer(steamID))
			{
				SteamPlayer steamPlayer = PlayerTool.getSteamPlayer(origin);
				if (steamPlayer == null)
				{
					return;
				}
				SteamPlayer steamPlayer2 = PlayerTool.getSteamPlayer(target);
				if (steamPlayer2 == null)
				{
					return;
				}
				ChatManager.needsVote = true;
				ChatManager.hasVote = false;
				if (ChatManager.onVotingStart != null)
				{
					ChatManager.onVotingStart(steamPlayer, steamPlayer2, votesNeeded);
				}
			}
		}

		[SteamCall]
		public void tellVoteUpdate(CSteamID steamID, byte voteYes, byte voteNo)
		{
			if (base.channel.checkServer(steamID) && ChatManager.onVotingUpdate != null)
			{
				ChatManager.onVotingUpdate(voteYes, voteNo);
			}
		}

		[SteamCall]
		public void tellVoteStop(CSteamID steamID, byte message)
		{
			if (base.channel.checkServer(steamID))
			{
				ChatManager.needsVote = false;
				if (ChatManager.onVotingStop != null)
				{
					ChatManager.onVotingStop((EVotingMessage)message);
				}
			}
		}

		[SteamCall]
		public void tellVoteMessage(CSteamID steamID, byte message)
		{
			if (base.channel.checkServer(steamID) && ChatManager.onVotingMessage != null)
			{
				ChatManager.onVotingMessage((EVotingMessage)message);
			}
		}

		[SteamCall]
		public void askVote(CSteamID steamID, bool vote)
		{
			if (Provider.isServer)
			{
				if (PlayerTool.getSteamPlayer(steamID) == null)
				{
					return;
				}
				if (!ChatManager.isVoting)
				{
					return;
				}
				if (ChatManager.votes.Contains(steamID))
				{
					return;
				}
				ChatManager.votes.Add(steamID);
				if (vote)
				{
					ChatManager.voteYes += 1;
				}
				else
				{
					ChatManager.voteNo += 1;
				}
				ChatManager.manager.channel.send("tellVoteUpdate", ESteamCall.CLIENTS, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
				{
					ChatManager.voteYes,
					ChatManager.voteNo
				});
			}
		}

		[SteamCall]
		public void askCallVote(CSteamID steamID, CSteamID target)
		{
			if (Provider.isServer)
			{
				if (ChatManager.isVoting)
				{
					return;
				}
				if (!ChatManager.voteAllowed)
				{
					ChatManager.manager.channel.send("tellVoteMessage", steamID, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
					{
						0
					});
					return;
				}
				SteamPlayer steamPlayer = PlayerTool.getSteamPlayer(steamID);
				if (steamPlayer == null || Time.realtimeSinceStartup < steamPlayer.nextVote)
				{
					ChatManager.manager.channel.send("tellVoteMessage", steamID, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
					{
						1
					});
					return;
				}
				SteamPlayer steamPlayer2 = PlayerTool.getSteamPlayer(target);
				if (steamPlayer2 == null || steamPlayer2.isAdmin)
				{
					return;
				}
				if (Provider.clients.Count < (int)ChatManager.votePlayers)
				{
					ChatManager.manager.channel.send("tellVoteMessage", steamID, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
					{
						2
					});
					return;
				}
				CommandWindow.Log(Provider.localization.format("Vote_Kick", new object[]
				{
					steamPlayer.playerID.characterName,
					steamPlayer.playerID.playerName,
					steamPlayer2.playerID.characterName,
					steamPlayer2.playerID.playerName
				}));
				ChatManager.lastVote = Time.realtimeSinceStartup;
				ChatManager.isVoting = true;
				ChatManager.voteYes = 0;
				ChatManager.voteNo = 0;
				ChatManager.votesPossible = (byte)Provider.clients.Count;
				ChatManager.votesNeeded = (byte)Mathf.Ceil((float)ChatManager.votesPossible * ChatManager.votePercentage);
				ChatManager.voteOrigin = steamPlayer;
				ChatManager.voteTarget = target;
				ChatManager.votes = new List<CSteamID>();
				P2PSessionState_t p2PSessionState_t;
				if (SteamGameServerNetworking.GetP2PSessionState(ChatManager.voteTarget, ref p2PSessionState_t))
				{
					ChatManager.voteIP = p2PSessionState_t.m_nRemoteIP;
				}
				else
				{
					ChatManager.voteIP = 0u;
				}
				ChatManager.manager.channel.send("tellVoteStart", ESteamCall.CLIENTS, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
				{
					steamID,
					target,
					ChatManager.votesNeeded
				});
			}
		}

		public static void sendVote(bool vote)
		{
			ChatManager.manager.channel.send("askVote", ESteamCall.SERVER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
			{
				vote
			});
		}

		public static void sendCallVote(CSteamID target)
		{
			ChatManager.manager.channel.send("askCallVote", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[]
			{
				target
			});
		}

		[SteamCall]
		public void tellChat(CSteamID steamID, CSteamID owner, byte mode, Color color, string text)
		{
			if (base.channel.checkServer(steamID))
			{
				ChatManager.list(owner, (EChatMode)mode, color, text);
			}
		}

		[SteamCall]
		public void askChat(CSteamID steamID, byte mode, string text)
		{
			if (Provider.isServer)
			{
				SteamPlayer steamPlayer = PlayerTool.getSteamPlayer(steamID);
				if (steamPlayer == null)
				{
					return;
				}
				if (Time.realtimeSinceStartup - steamPlayer.lastChat < ChatManager.chatrate)
				{
					return;
				}
				steamPlayer.lastChat = Time.realtimeSinceStartup;
				if (text.Length < 2)
				{
					return;
				}
				if (text.Length > ChatManager.LENGTH)
				{
					text = text.Substring(0, ChatManager.LENGTH);
				}
				text = text.Trim();
				if (mode == 0)
				{
					if (CommandWindow.shouldLogChat)
					{
						CommandWindow.Log(Provider.localization.format("Global", new object[]
						{
							steamPlayer.playerID.characterName,
							steamPlayer.playerID.playerName,
							text
						}));
					}
				}
				else if (mode == 1)
				{
					if (CommandWindow.shouldLogChat)
					{
						CommandWindow.Log(Provider.localization.format("Local", new object[]
						{
							steamPlayer.playerID.characterName,
							steamPlayer.playerID.playerName,
							text
						}));
					}
				}
				else
				{
					if (mode != 2)
					{
						return;
					}
					if (CommandWindow.shouldLogChat)
					{
						CommandWindow.Log(Provider.localization.format("Group", new object[]
						{
							steamPlayer.playerID.characterName,
							steamPlayer.playerID.playerName,
							text
						}));
					}
				}
				Color color = Color.white;
				if (steamPlayer.isAdmin && !Provider.hideAdmins)
				{
					color = Palette.ADMIN;
				}
				else if (steamPlayer.isPro)
				{
					color = Palette.PRO;
				}
				bool flag = true;
				if (ChatManager.onChatted != null)
				{
					ChatManager.onChatted(steamPlayer, (EChatMode)mode, ref color, text, ref flag);
				}
				if (ChatManager.process(steamPlayer, text) && flag)
				{
					if (mode == 0)
					{
						base.channel.send("tellChat", ESteamCall.OTHERS, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
						{
							steamID,
							mode,
							color,
							text
						});
					}
					else if (mode == 1)
					{
						base.channel.send("tellChat", ESteamCall.OTHERS, steamPlayer.player.transform.position, EffectManager.MEDIUM, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
						{
							steamID,
							mode,
							color,
							text
						});
					}
					else if (mode == 2 && steamPlayer.player.quests.groupID != CSteamID.Nil)
					{
						for (int i = 0; i < Provider.clients.Count; i++)
						{
							SteamPlayer steamPlayer2 = Provider.clients[i];
							if (steamPlayer2.player != null && steamPlayer.player.quests.isMemberOfSameGroupAs(steamPlayer2.player))
							{
								base.channel.send("tellChat", steamPlayer2.playerID.steamID, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
								{
									steamID,
									mode,
									color,
									text
								});
							}
						}
					}
				}
			}
		}

		[TerminalCommandMethod("chat.send", "broadcast message in chat")]
		public static void terminalChat([TerminalCommandParameter("message", "text to send")] string message)
		{
			ChatManager.sendChat(EChatMode.GLOBAL, message);
		}

		public static void sendChat(EChatMode mode, string text)
		{
			if (!Provider.isServer)
			{
				ChatManager.manager.channel.send("askChat", ESteamCall.SERVER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
				{
					(byte)mode,
					text
				});
			}
			else if (!Dedicator.isDedicated)
			{
				SteamPlayer steamPlayer = PlayerTool.getSteamPlayer(Provider.client);
				if (steamPlayer == null)
				{
					return;
				}
				Color color = (!Provider.isPro) ? Color.white : Palette.PRO;
				bool flag = true;
				if (ChatManager.onChatted != null)
				{
					ChatManager.onChatted(steamPlayer, mode, ref color, text, ref flag);
				}
				if (ChatManager.process(steamPlayer, text) && flag)
				{
					ChatManager.list(Provider.client, mode, color, text);
				}
			}
		}

		public static void say(CSteamID target, string text, Color color)
		{
			ChatManager.say(target, text, color, EChatMode.WELCOME);
		}

		public static void say(CSteamID target, string text, Color color, EChatMode mode)
		{
			if (Provider.isServer)
			{
				if (text.Length > ChatManager.LENGTH)
				{
					text = text.Substring(0, ChatManager.LENGTH);
				}
				if (Dedicator.isDedicated)
				{
					ChatManager.manager.channel.send("tellChat", target, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
					{
						CSteamID.Nil,
						(byte)mode,
						color,
						text
					});
				}
				else
				{
					ChatManager.list(CSteamID.Nil, mode, color, text);
				}
			}
		}

		public static void say(string text, Color color)
		{
			if (Provider.isServer)
			{
				if (text.Length > ChatManager.LENGTH)
				{
					text = text.Substring(0, ChatManager.LENGTH);
				}
				if (Dedicator.isDedicated)
				{
					ChatManager.manager.channel.send("tellChat", ESteamCall.OTHERS, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
					{
						CSteamID.Nil,
						4,
						color,
						text
					});
				}
				else
				{
					ChatManager.list(CSteamID.Nil, EChatMode.SAY, color, text);
				}
			}
		}

		private void onLevelLoaded(int level)
		{
			if (level > Level.SETUP)
			{
				for (int i = 0; i < ChatManager.chat.Length; i++)
				{
					ChatManager.chat[i] = null;
				}
			}
		}

		private void onServerConnected(CSteamID steamID)
		{
			if (Provider.isServer && ChatManager.welcomeText != string.Empty)
			{
				SteamPlayer steamPlayer = PlayerTool.getSteamPlayer(steamID);
				ChatManager.say(steamPlayer.playerID.steamID, string.Format(ChatManager.welcomeText, steamPlayer.playerID.characterName), ChatManager.welcomeColor);
			}
		}

		private void Update()
		{
			if (ChatManager.isVoting && (Time.realtimeSinceStartup - ChatManager.lastVote > ChatManager.voteDuration || ChatManager.voteYes >= ChatManager.votesNeeded || ChatManager.voteNo > ChatManager.votesPossible - ChatManager.votesNeeded))
			{
				ChatManager.isVoting = false;
				if (ChatManager.voteYes >= ChatManager.votesNeeded)
				{
					if (ChatManager.voteOrigin != null)
					{
						ChatManager.voteOrigin.nextVote = Time.realtimeSinceStartup + ChatManager.votePassCooldown;
					}
					CommandWindow.Log(Provider.localization.format("Vote_Pass"));
					ChatManager.manager.channel.send("tellVoteStop", ESteamCall.CLIENTS, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
					{
						3
					});
					SteamBlacklist.ban(ChatManager.voteTarget, ChatManager.voteIP, CSteamID.Nil, "you were vote kicked", SteamBlacklist.TEMPORARY);
				}
				else
				{
					if (ChatManager.voteOrigin != null)
					{
						ChatManager.voteOrigin.nextVote = Time.realtimeSinceStartup + ChatManager.voteFailCooldown;
					}
					CommandWindow.Log(Provider.localization.format("Vote_Fail"));
					ChatManager.manager.channel.send("tellVoteStop", ESteamCall.CLIENTS, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
					{
						4
					});
				}
			}
			if (ChatManager.needsVote && !ChatManager.hasVote)
			{
				if (Input.GetKeyDown(282))
				{
					ChatManager.needsVote = false;
					ChatManager.hasVote = true;
					ChatManager.sendVote(true);
				}
				else if (Input.GetKeyDown(283))
				{
					ChatManager.needsVote = false;
					ChatManager.hasVote = true;
					ChatManager.sendVote(false);
				}
			}
		}

		private void Start()
		{
			ChatManager.manager = this;
			ChatManager._chat = new Chat[16];
			Level.onLevelLoaded = (LevelLoaded)Delegate.Combine(Level.onLevelLoaded, new LevelLoaded(this.onLevelLoaded));
			Provider.onServerConnected = (Provider.ServerConnected)Delegate.Combine(Provider.onServerConnected, new Provider.ServerConnected(this.onServerConnected));
		}

		public static readonly int LENGTH = 90;

		public static Listed onListed;

		public static Chatted onChatted;

		public static CheckPermissions onCheckPermissions;

		public static VotingStart onVotingStart;

		public static VotingUpdate onVotingUpdate;

		public static VotingStop onVotingStop;

		public static VotingMessage onVotingMessage;

		public static string welcomeText = string.Empty;

		public static Color welcomeColor = Palette.SERVER;

		public static float chatrate = 0.25f;

		public static bool voteAllowed = false;

		public static float votePassCooldown = 5f;

		public static float voteFailCooldown = 60f;

		public static float voteDuration = 15f;

		public static float votePercentage = 0.75f;

		public static byte votePlayers = 3;

		private static float lastVote;

		private static bool isVoting;

		private static bool needsVote;

		private static bool hasVote;

		private static byte voteYes;

		private static byte voteNo;

		private static byte votesPossible;

		private static byte votesNeeded;

		private static SteamPlayer voteOrigin;

		private static CSteamID voteTarget;

		private static uint voteIP;

		private static List<CSteamID> votes;

		private static ChatManager manager;

		private static Chat[] _chat;

		public static readonly string[] SWEARS = new string[]
		{
			"bitch",
			"clit",
			"cunt",
			"dick",
			"pussy",
			"penis",
			"vagina",
			"fuck",
			"fucking",
			"fuckd",
			"fucked",
			"shit",
			"shiting",
			"shitting",
			"shat",
			"damn",
			"damned",
			"hell",
			"cock",
			"whore",
			"fag",
			"faggot",
			"nigger",
			"suka",
			"cuka",
			"cyka",
			"сука",
			"blyat",
			"блят",
			"блять"
		};
	}
}
