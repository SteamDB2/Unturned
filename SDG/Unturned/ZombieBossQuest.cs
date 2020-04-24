using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	public class ZombieBossQuest : MonoBehaviour
	{
		private IEnumerator teleport()
		{
			yield return new WaitForSeconds(3f);
			if (this.target != null)
			{
				ZombieBossQuest.nearbyPlayers.Clear();
				PlayerTool.getPlayersInRadius(base.transform.position, this.sqrRadius, ZombieBossQuest.nearbyPlayers);
				for (int i = 0; i < ZombieBossQuest.nearbyPlayers.Count; i++)
				{
					Player player = ZombieBossQuest.nearbyPlayers[i];
					if (!player.life.isDead)
					{
						player.quests.sendRemoveQuest(213);
						player.quests.setFlag(213, 1);
						player.sendTeleport(this.target.position, MeasurementTool.angleToByte(this.target.rotation.eulerAngles.y));
					}
				}
			}
			yield break;
		}

		private void onPlayerLifeUpdated(Player player)
		{
			if (player == null || !player.life.isDead)
			{
				return;
			}
			if ((player.transform.position - base.transform.position).sqrMagnitude > this.sqrRadius)
			{
				return;
			}
			player.quests.sendRemoveQuest(213);
		}

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
			EffectManager.sendEffect(this.teleportEffect, 16f, zombie.transform.position + Vector3.up);
			base.StartCoroutine("teleport");
		}

		private void OnEnable()
		{
			if (!Provider.isServer)
			{
				return;
			}
			if (!this.isListeningPlayer)
			{
				PlayerLife.onPlayerLifeUpdated = (PlayerLifeUpdated)Delegate.Combine(PlayerLife.onPlayerLifeUpdated, new PlayerLifeUpdated(this.onPlayerLifeUpdated));
				this.isListeningPlayer = true;
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
			if (!this.isListeningZombie)
			{
				ZombieRegion zombieRegion = this.region;
				zombieRegion.onZombieLifeUpdated = (ZombieLifeUpdated)Delegate.Combine(zombieRegion.onZombieLifeUpdated, new ZombieLifeUpdated(this.onZombieLifeUpdated));
				this.isListeningZombie = true;
			}
		}

		private void OnDisable()
		{
			if (this.isListeningPlayer)
			{
				PlayerLife.onPlayerLifeUpdated = (PlayerLifeUpdated)Delegate.Remove(PlayerLife.onPlayerLifeUpdated, new PlayerLifeUpdated(this.onPlayerLifeUpdated));
				this.isListeningPlayer = false;
			}
			if (this.isListeningZombie && this.region != null)
			{
				ZombieRegion zombieRegion = this.region;
				zombieRegion.onZombieLifeUpdated = (ZombieLifeUpdated)Delegate.Remove(zombieRegion.onZombieLifeUpdated, new ZombieLifeUpdated(this.onZombieLifeUpdated));
				this.isListeningZombie = false;
			}
			this.region = null;
		}

		private static List<Player> nearbyPlayers = new List<Player>();

		public Transform target;

		public float sqrRadius;

		public ushort teleportEffect;

		private ZombieRegion region;

		private bool isListeningPlayer;

		private bool isListeningZombie;
	}
}
