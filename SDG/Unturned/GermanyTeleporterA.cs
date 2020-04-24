using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	public class GermanyTeleporterA : MonoBehaviour
	{
		protected virtual IEnumerator teleport()
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
						if (player.quests.getQuestStatus(248) != ENPCQuestStatus.COMPLETED)
						{
							player.quests.sendAddQuest(248);
						}
						player.sendTeleport(this.target.position, MeasurementTool.angleToByte(this.target.rotation.eulerAngles.y));
					}
				}
			}
			yield break;
		}

		protected virtual void handleEventTriggered(string id)
		{
			if (id != this.eventID)
			{
				return;
			}
			if (Time.realtimeSinceStartup - this.lastTeleport < 5f)
			{
				return;
			}
			this.lastTeleport = Time.realtimeSinceStartup;
			EffectManager.sendEffect(this.teleportEffect, 16f, base.transform.position);
			base.StartCoroutine("teleport");
		}

		protected virtual void OnEnable()
		{
			if (!Provider.isServer)
			{
				return;
			}
			if (!this.isListening)
			{
				NPCEventManager.eventTriggered += this.handleEventTriggered;
				this.isListening = true;
			}
		}

		protected virtual void OnDisable()
		{
			if (this.isListening)
			{
				NPCEventManager.eventTriggered -= this.handleEventTriggered;
				this.isListening = false;
			}
		}

		protected static List<Player> nearbyPlayers = new List<Player>();

		public Transform target;

		public float sqrRadius;

		public string eventID;

		public ushort teleportEffect;

		private float lastTeleport;

		private bool isListening;
	}
}
