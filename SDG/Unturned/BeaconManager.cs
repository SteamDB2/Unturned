using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	public class BeaconManager : MonoBehaviour
	{
		public static int getParticipants(byte nav)
		{
			int num = 0;
			for (int i = 0; i < Provider.clients.Count; i++)
			{
				SteamPlayer steamPlayer = Provider.clients[i];
				if (!(steamPlayer.player == null) && !(steamPlayer.player.movement == null) && !(steamPlayer.player.life == null) && !steamPlayer.player.life.isDead)
				{
					if (steamPlayer.player.movement.nav == nav)
					{
						num++;
					}
				}
			}
			return num;
		}

		public static InteractableBeacon checkBeacon(byte nav)
		{
			if (BeaconManager.beacons[(int)nav].Count > 0)
			{
				return BeaconManager.beacons[(int)nav][0];
			}
			return null;
		}

		public static void registerBeacon(byte nav, InteractableBeacon beacon)
		{
			if (!LevelNavigation.checkSafe(nav))
			{
				return;
			}
			BeaconManager.beacons[(int)nav].Add(beacon);
			if (BeaconManager.onBeaconUpdated != null)
			{
				BeaconManager.onBeaconUpdated(nav, BeaconManager.beacons[(int)nav].Count > 0);
			}
		}

		public static void deregisterBeacon(byte nav, InteractableBeacon beacon)
		{
			if (!LevelNavigation.checkSafe(nav))
			{
				return;
			}
			BeaconManager.beacons[(int)nav].Remove(beacon);
			if (BeaconManager.onBeaconUpdated != null)
			{
				BeaconManager.onBeaconUpdated(nav, BeaconManager.beacons[(int)nav].Count > 0);
			}
		}

		private void onLevelLoaded(int level)
		{
			if (LevelNavigation.bounds == null)
			{
				return;
			}
			BeaconManager.beacons = new List<InteractableBeacon>[LevelNavigation.bounds.Count];
			for (int i = 0; i < BeaconManager.beacons.Length; i++)
			{
				BeaconManager.beacons[i] = new List<InteractableBeacon>();
			}
		}

		private void Start()
		{
			Level.onPrePreLevelLoaded = (PrePreLevelLoaded)Delegate.Combine(Level.onPrePreLevelLoaded, new PrePreLevelLoaded(this.onLevelLoaded));
		}

		private static List<InteractableBeacon>[] beacons;

		public static BeaconUpdated onBeaconUpdated;
	}
}
