using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	public class ZombieSoulFlag : MonoBehaviour
	{
		private void onZombieLifeUpdated(Zombie zombie)
		{
			if (!zombie.isDead)
			{
				return;
			}
			if ((zombie.transform.position - base.transform.position).sqrMagnitude > this.sqrRadius)
			{
				return;
			}
			ZombieSoulFlag.nearbyPlayers.Clear();
			PlayerTool.getPlayersInRadius(base.transform.position, this.sqrRadius, ZombieSoulFlag.nearbyPlayers);
			for (int i = 0; i < ZombieSoulFlag.nearbyPlayers.Count; i++)
			{
				Player player = ZombieSoulFlag.nearbyPlayers[i];
				if (!player.life.isDead)
				{
					short num;
					if (player.quests.getFlag(this.flagPlaced, out num) && num == 1)
					{
						EffectManager.sendEffect(this.collectEffect, player.channel.owner.playerID.steamID, zombie.transform.position + Vector3.up, (base.transform.position - zombie.transform.position + Vector3.up).normalized);
						short num2;
						player.quests.getFlag(this.flagKills, out num2);
						num2 += 1;
						player.quests.sendSetFlag(this.flagKills, num2);
						if (num2 >= (short)this.soulsNeeded)
						{
							EffectManager.sendEffect(this.teleportEffect, player.channel.owner.playerID.steamID, base.transform.position);
							player.quests.sendSetFlag(this.flagPlaced, 2);
						}
					}
				}
			}
		}

		private void OnEnable()
		{
			if (!Provider.isServer)
			{
				return;
			}
			if (this.region != null)
			{
				return;
			}
			byte b;
			if (LevelNavigation.tryGetBounds(base.transform.position, out b))
			{
				this.region = ZombieManager.regions[(int)b];
			}
			if (this.region == null)
			{
				return;
			}
			if (!this.isListening)
			{
				ZombieRegion zombieRegion = this.region;
				zombieRegion.onZombieLifeUpdated = (ZombieLifeUpdated)Delegate.Combine(zombieRegion.onZombieLifeUpdated, new ZombieLifeUpdated(this.onZombieLifeUpdated));
				this.isListening = true;
			}
		}

		private void OnDisable()
		{
			if (this.isListening && this.region != null)
			{
				ZombieRegion zombieRegion = this.region;
				zombieRegion.onZombieLifeUpdated = (ZombieLifeUpdated)Delegate.Remove(zombieRegion.onZombieLifeUpdated, new ZombieLifeUpdated(this.onZombieLifeUpdated));
				this.isListening = false;
			}
			this.region = null;
		}

		private static List<Player> nearbyPlayers = new List<Player>();

		public ushort flagPlaced;

		public ushort flagKills;

		public float sqrRadius;

		public byte soulsNeeded;

		public ushort collectEffect;

		public ushort teleportEffect;

		private ZombieRegion region;

		private bool isListening;
	}
}
