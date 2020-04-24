using System;
using System.Collections;
using UnityEngine;

namespace SDG.Unturned
{
	public class GermanyTeleporterB : GermanyTeleporterA
	{
		protected override IEnumerator teleport()
		{
			yield return new WaitForSeconds(1f);
			if (this.target != null)
			{
				GermanyTeleporterA.nearbyPlayers.Clear();
				PlayerTool.getPlayersInRadius(base.transform.position, this.sqrRadius, GermanyTeleporterA.nearbyPlayers);
				for (int i = 0; i < GermanyTeleporterA.nearbyPlayers.Count; i++)
				{
					Player player = GermanyTeleporterA.nearbyPlayers[i];
					if (!player.life.isDead)
					{
						if (player.quests.getQuestStatus(248) == ENPCQuestStatus.COMPLETED)
						{
							player.sendTeleport(this.target.position, MeasurementTool.angleToByte(this.target.rotation.eulerAngles.y));
						}
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
			if ((player.transform.position - base.transform.position).sqrMagnitude > this.sqrBossRadius)
			{
				return;
			}
			if (player.quests.getQuestStatus(248) == ENPCQuestStatus.COMPLETED)
			{
				return;
			}
			player.quests.sendRemoveQuest(248);
		}

		private void onZombieLifeUpdated(Zombie zombie)
		{
			if (!zombie.isDead)
			{
				return;
			}
			if ((zombie.transform.position - base.transform.position).sqrMagnitude > this.sqrBossRadius)
			{
				return;
			}
			GermanyTeleporterA.nearbyPlayers.Clear();
			PlayerTool.getPlayersInRadius(base.transform.position, this.sqrBossRadius, GermanyTeleporterA.nearbyPlayers);
			for (int i = 0; i < GermanyTeleporterA.nearbyPlayers.Count; i++)
			{
				Player player = GermanyTeleporterA.nearbyPlayers[i];
				if (!player.life.isDead)
				{
					player.quests.sendRemoveQuest(248);
					player.quests.sendSetFlag(248, 1);
				}
			}
		}

		protected override void OnEnable()
		{
			base.OnEnable();
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
			this.region = ZombieManager.regions[this.navIndex];
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

		protected override void OnDisable()
		{
			base.OnDisable();
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

		public float sqrBossRadius;

		public int navIndex;

		private ZombieRegion region;

		private bool isListeningPlayer;

		private bool isListeningZombie;
	}
}
