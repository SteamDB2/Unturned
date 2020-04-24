using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class InteractableBeacon : MonoBehaviour, IManualOnDestroy
	{
		public void updateState(ItemBarricadeAsset asset)
		{
			this.asset = (ItemBeaconAsset)asset;
		}

		public bool isPlant
		{
			get
			{
				return base.transform.parent != null && base.transform.parent.CompareTag("Vehicle");
			}
		}

		public int initialParticipants { get; private set; }

		public void init(int amount)
		{
			if (this.wasInit)
			{
				return;
			}
			if (amount >= (int)this.asset.wave)
			{
				this.remaining = 0;
				this.alive = (int)this.asset.wave;
			}
			else
			{
				this.remaining = (int)this.asset.wave - amount;
				this.alive = amount;
			}
			this.wasInit = true;
		}

		public int getRemaining()
		{
			return this.remaining;
		}

		public void spawnRemaining()
		{
			if (this.remaining <= 0)
			{
				return;
			}
			this.remaining--;
			this.alive++;
		}

		public int getAlive()
		{
			return this.alive;
		}

		public void despawnAlive()
		{
			if (this.alive <= 0)
			{
				return;
			}
			this.alive--;
			if (this.remaining == 0 && this.alive == 0)
			{
				BarricadeManager.damage(base.transform, 10000f, 1f, false);
			}
		}

		private void Update()
		{
			if (!Provider.isServer)
			{
				return;
			}
			if (Time.realtimeSinceStartup - this.started < 3f)
			{
				return;
			}
			if (this.isRegistered)
			{
				for (int i = 0; i < Provider.clients.Count; i++)
				{
					SteamPlayer steamPlayer = Provider.clients[i];
					if (!(steamPlayer.player == null) && !(steamPlayer.player.movement == null) && !(steamPlayer.player.life == null) && !steamPlayer.player.life.isDead)
					{
						if (steamPlayer.player.movement.nav == this.nav)
						{
							return;
						}
					}
				}
			}
			BarricadeManager.damage(base.transform, 10000f, 1f, false);
		}

		private void Start()
		{
			this.started = Time.realtimeSinceStartup;
			Transform transform = base.transform.FindChild("Engine");
			if (transform != null)
			{
				transform.gameObject.SetActive(true);
			}
			if (!Provider.isServer)
			{
				return;
			}
			if (this.isRegistered)
			{
				return;
			}
			if (this.isPlant)
			{
				return;
			}
			if (!LevelNavigation.checkNavigation(base.transform.position))
			{
				return;
			}
			LevelNavigation.tryGetNavigation(base.transform.position, out this.nav);
			this.initialParticipants = BeaconManager.getParticipants(this.nav);
			BeaconManager.registerBeacon(this.nav, this);
			this.isRegistered = true;
		}

		public void ManualOnDestroy()
		{
			if (!Provider.isServer)
			{
				return;
			}
			if (!this.isRegistered)
			{
				return;
			}
			BeaconManager.deregisterBeacon(this.nav, this);
			this.isRegistered = false;
			if (!this.wasInit)
			{
				return;
			}
			if (this.remaining > 0 || this.alive > 0)
			{
				return;
			}
			for (int i = 0; i < Provider.clients.Count; i++)
			{
				if (Provider.clients[i].player != null && !Provider.clients[i].player.life.isDead && Provider.clients[i].player.movement.nav == this.nav)
				{
					Provider.clients[i].player.quests.trackHordeKill();
				}
			}
			int num = (int)this.asset.rewards;
			num *= Mathf.Max(1, this.initialParticipants);
			for (int j = 0; j < num; j++)
			{
				ushort num2 = SpawnTableTool.resolve(this.asset.rewardID);
				if (num2 != 0)
				{
					ItemManager.dropItem(new Item(num2, EItemOrigin.NATURE), base.transform.position, false, true, true);
				}
			}
		}

		private ItemBeaconAsset asset;

		private byte nav;

		private bool wasInit;

		private float started;

		private int remaining;

		private int alive;

		private bool isRegistered;
	}
}
