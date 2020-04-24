using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace SDG.Unturned
{
	public class PlayerGroupUI
	{
		public PlayerGroupUI()
		{
			PlayerGroupUI._container = new Sleek();
			PlayerGroupUI.container.sizeScale_X = 1f;
			PlayerGroupUI.container.sizeScale_Y = 1f;
			PlayerUI.container.add(PlayerGroupUI.container);
			PlayerGroupUI._groups = new List<SleekLabel>();
			for (int i = 0; i < Provider.clients.Count; i++)
			{
				PlayerGroupUI.addGroup(Provider.clients[i]);
			}
			if (PlayerGroupUI.<>f__mg$cache0 == null)
			{
				PlayerGroupUI.<>f__mg$cache0 = new Provider.EnemyConnected(PlayerGroupUI.onEnemyConnected);
			}
			Provider.onEnemyConnected = PlayerGroupUI.<>f__mg$cache0;
			if (PlayerGroupUI.<>f__mg$cache1 == null)
			{
				PlayerGroupUI.<>f__mg$cache1 = new Provider.EnemyDisconnected(PlayerGroupUI.onEnemyDisconnected);
			}
			Provider.onEnemyDisconnected = PlayerGroupUI.<>f__mg$cache1;
		}

		public static Sleek container
		{
			get
			{
				return PlayerGroupUI._container;
			}
		}

		public static List<SleekLabel> groups
		{
			get
			{
				return PlayerGroupUI._groups;
			}
		}

		private static void addGroup(SteamPlayer player)
		{
			SleekLabel sleekLabel = new SleekLabel();
			sleekLabel.sizeOffset_X = 200;
			sleekLabel.sizeOffset_Y = 30;
			if (string.IsNullOrEmpty(player.playerID.nickName))
			{
				sleekLabel.text = player.playerID.characterName;
			}
			else
			{
				sleekLabel.text = player.playerID.nickName;
			}
			PlayerGroupUI.container.add(sleekLabel);
			sleekLabel.isVisible = false;
			PlayerGroupUI.groups.Add(sleekLabel);
		}

		private static void onEnemyConnected(SteamPlayer player)
		{
			PlayerGroupUI.addGroup(player);
		}

		private static void onEnemyDisconnected(SteamPlayer player)
		{
			for (int i = 0; i < Provider.clients.Count; i++)
			{
				if (Provider.clients[i] == player)
				{
					PlayerGroupUI.container.remove(PlayerGroupUI.groups[i]);
					PlayerGroupUI.groups.RemoveAt(i);
				}
			}
		}

		private static Sleek _container;

		private static List<SleekLabel> _groups;

		[CompilerGenerated]
		private static Provider.EnemyConnected <>f__mg$cache0;

		[CompilerGenerated]
		private static Provider.EnemyDisconnected <>f__mg$cache1;
	}
}
