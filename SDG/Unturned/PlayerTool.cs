using System;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	public class PlayerTool
	{
		private static string getRepKey(int rep)
		{
			string result = string.Empty;
			if (rep <= -200)
			{
				result = "Villain";
			}
			else if (rep <= -100)
			{
				result = "Bandit";
			}
			else if (rep <= -33)
			{
				result = "Gangster";
			}
			else if (rep <= -8)
			{
				result = "Outlaw";
			}
			else if (rep < 0)
			{
				result = "Thug";
			}
			else if (rep >= 200)
			{
				result = "Paragon";
			}
			else if (rep >= 100)
			{
				result = "Sheriff";
			}
			else if (rep >= 33)
			{
				result = "Deputy";
			}
			else if (rep >= 8)
			{
				result = "Constable";
			}
			else if (rep > 0)
			{
				result = "Vigilante";
			}
			else if (rep == 0)
			{
				result = "Neutral";
			}
			return result;
		}

		public static Texture2D getRepTexture(int rep)
		{
			return (Texture2D)Resources.Load("Reputation/" + PlayerTool.getRepKey(rep));
		}

		public static string getRepTitle(int rep)
		{
			return PlayerDashboardInformationUI.localization.format("Rep", new object[]
			{
				PlayerDashboardInformationUI.localization.format("Rep_" + PlayerTool.getRepKey(rep)),
				rep
			});
		}

		public static Color getRepColor(int rep)
		{
			if (rep == 0)
			{
				return Color.white;
			}
			if (rep < 0)
			{
				float num = (float)Mathf.Min(Mathf.Abs(rep), 200) / 200f;
				if (num < 0.5f)
				{
					return Color.Lerp(Color.white, Palette.COLOR_Y, num * 2f);
				}
				return Color.Lerp(Palette.COLOR_Y, Palette.COLOR_R, (num - 0.5f) * 2f);
			}
			else
			{
				if (rep > 0)
				{
					float num2 = (float)Mathf.Min(Mathf.Abs(rep), 200) / 200f;
					return Color.Lerp(Color.white, Palette.COLOR_G, num2);
				}
				return Color.white;
			}
		}

		public static void getPlayersInRadius(Vector3 center, float sqrRadius, List<Player> result)
		{
			for (int i = 0; i < Provider.clients.Count; i++)
			{
				Player player = Provider.clients[i].player;
				if (!(player == null))
				{
					if ((player.transform.position - center).sqrMagnitude < sqrRadius)
					{
						result.Add(player);
					}
				}
			}
		}

		public static SteamPlayer[] getSteamPlayers()
		{
			return Provider.clients.ToArray();
		}

		public static SteamPlayer getSteamPlayer(string name)
		{
			for (int i = 0; i < Provider.clients.Count; i++)
			{
				if (NameTool.checkNames(name, Provider.clients[i].playerID.playerName) || NameTool.checkNames(name, Provider.clients[i].playerID.characterName))
				{
					return Provider.clients[i];
				}
			}
			return null;
		}

		public static SteamPlayer getSteamPlayer(ulong steamID)
		{
			for (int i = 0; i < Provider.clients.Count; i++)
			{
				if (Provider.clients[i].playerID.steamID.m_SteamID == steamID)
				{
					return Provider.clients[i];
				}
			}
			return null;
		}

		public static SteamPlayer getSteamPlayer(CSteamID steamID)
		{
			for (int i = 0; i < Provider.clients.Count; i++)
			{
				if (Provider.clients[i].playerID.steamID == steamID)
				{
					return Provider.clients[i];
				}
			}
			return null;
		}

		public static Transform getPlayerModel(CSteamID steamID)
		{
			SteamPlayer steamPlayer = PlayerTool.getSteamPlayer(steamID);
			if (steamPlayer != null && steamPlayer.model != null)
			{
				return steamPlayer.model;
			}
			return null;
		}

		public static Player getPlayer(CSteamID steamID)
		{
			SteamPlayer steamPlayer = PlayerTool.getSteamPlayer(steamID);
			if (steamPlayer != null && steamPlayer.player != null)
			{
				return steamPlayer.player;
			}
			return null;
		}

		public static Transform getPlayerModel(string name)
		{
			SteamPlayer steamPlayer = PlayerTool.getSteamPlayer(name);
			if (steamPlayer != null && steamPlayer.model != null)
			{
				return steamPlayer.model;
			}
			return null;
		}

		public static Player getPlayer(string name)
		{
			SteamPlayer steamPlayer = PlayerTool.getSteamPlayer(name);
			if (steamPlayer != null && steamPlayer.player != null)
			{
				return steamPlayer.player;
			}
			return null;
		}

		public static bool tryGetSteamPlayer(string input, out SteamPlayer player)
		{
			player = null;
			ulong steamID;
			if (ulong.TryParse(input, out steamID))
			{
				player = PlayerTool.getSteamPlayer(steamID);
				return player != null;
			}
			player = PlayerTool.getSteamPlayer(input);
			return player != null;
		}

		public static bool tryGetSteamID(string input, out CSteamID steamID)
		{
			steamID = CSteamID.Nil;
			ulong num;
			if (ulong.TryParse(input, out num))
			{
				steamID..ctor(num);
				return true;
			}
			SteamPlayer steamPlayer = PlayerTool.getSteamPlayer(input);
			if (steamPlayer != null)
			{
				steamID = steamPlayer.playerID.steamID;
				return true;
			}
			return false;
		}
	}
}
