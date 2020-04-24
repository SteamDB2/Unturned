using System;
using System.Runtime.CompilerServices;
using Steamworks;

namespace SDG.Unturned
{
	public class SaveManager : SteamCaller
	{
		public static void save()
		{
			if (Level.info.type == ELevelType.SURVIVAL)
			{
				for (int i = 0; i < Provider.clients.Count; i++)
				{
					if (Provider.clients[i].model != null)
					{
						Player component = Provider.clients[i].model.GetComponent<Player>();
						component.save();
					}
				}
				VehicleManager.save();
				BarricadeManager.save();
				StructureManager.save();
				ObjectManager.save();
				LightingManager.save();
				GroupManager.save();
			}
			if (Dedicator.isDedicated)
			{
				SteamWhitelist.save();
				SteamBlacklist.save();
				SteamAdminlist.save();
			}
		}

		private static void onServerShutdown()
		{
			if (Provider.isServer && Level.isLoaded)
			{
				SaveManager.save();
			}
		}

		private static void onServerDisconnected(CSteamID steamID)
		{
			if (Provider.isServer && Level.isLoaded)
			{
				Player player = PlayerTool.getPlayer(steamID);
				if (player != null)
				{
					player.save();
				}
			}
		}

		private void Start()
		{
			Delegate onServerShutdown = Provider.onServerShutdown;
			if (SaveManager.<>f__mg$cache0 == null)
			{
				SaveManager.<>f__mg$cache0 = new Provider.ServerShutdown(SaveManager.onServerShutdown);
			}
			Provider.onServerShutdown = (Provider.ServerShutdown)Delegate.Combine(onServerShutdown, SaveManager.<>f__mg$cache0);
			Delegate onServerDisconnected = Provider.onServerDisconnected;
			if (SaveManager.<>f__mg$cache1 == null)
			{
				SaveManager.<>f__mg$cache1 = new Provider.ServerDisconnected(SaveManager.onServerDisconnected);
			}
			Provider.onServerDisconnected = (Provider.ServerDisconnected)Delegate.Combine(onServerDisconnected, SaveManager.<>f__mg$cache1);
		}

		[CompilerGenerated]
		private static Provider.ServerShutdown <>f__mg$cache0;

		[CompilerGenerated]
		private static Provider.ServerDisconnected <>f__mg$cache1;
	}
}
