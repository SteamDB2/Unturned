using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	public class ZombieSoulTeleporter : MonoBehaviour
	{
		private IEnumerator teleport()
		{
			yield return new WaitForSeconds(3f);
			if (this.target != null)
			{
				ZombieSoulTeleporter.nearbyPlayers.Clear();
				PlayerTool.getPlayersInRadius(base.transform.position, this.sqrRadius, ZombieSoulTeleporter.nearbyPlayers);
				for (int i = 0; i < ZombieSoulTeleporter.nearbyPlayers.Count; i++)
				{
					Player player = ZombieSoulTeleporter.nearbyPlayers[i];
					if (!player.life.isDead)
					{
						short num;
						short num2;
						if (player.quests.getFlag(211, out num) && num == 1 && player.quests.getFlag(212, out num2) && num2 == 1 && player.quests.getQuestStatus(213) != ENPCQuestStatus.COMPLETED)
						{
							player.quests.sendSetFlag(214, 0);
							player.quests.sendAddQuest(213);
							player.sendTeleport(this.targetBoss.position, MeasurementTool.angleToByte(this.targetBoss.rotation.eulerAngles.y));
						}
						else
						{
							player.sendTeleport(this.target.position, MeasurementTool.angleToByte(this.target.rotation.eulerAngles.y));
							if (player.equipment.isSelected)
							{
								player.equipment.dequip();
							}
							player.equipment.canEquip = false;
						}
					}
				}
			}
			yield break;
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
			EffectManager.sendEffect(this.collectEffect, 16f, zombie.transform.position + Vector3.up, (base.transform.position - zombie.transform.position + Vector3.up).normalized);
			this.soulsCollected += 1;
			if (this.soulsCollected >= this.soulsNeeded)
			{
				EffectManager.sendEffect(this.teleportEffect, 16f, base.transform.position);
				this.soulsCollected = 0;
				base.StartCoroutine("teleport");
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

		public Transform target;

		public Transform targetBoss;

		public float sqrRadius;

		public byte soulsNeeded;

		public ushort collectEffect;

		public ushort teleportEffect;

		private ZombieRegion region;

		private byte soulsCollected;

		private bool isListening;
	}
}
