using System;
using System.Collections;
using UnityEngine;

namespace SDG.Unturned
{
	public class VolumeTeleporter : MonoBehaviour
	{
		private IEnumerator teleport()
		{
			yield return new WaitForSeconds(3f);
			if (this.target != null && this.playerTeleported != null && !this.playerTeleported.life.isDead)
			{
				this.playerTeleported.sendTeleport(this.target.position, MeasurementTool.angleToByte(this.target.rotation.eulerAngles.y));
				if (this.playerTeleported.equipment.isSelected)
				{
					this.playerTeleported.equipment.dequip();
				}
				this.playerTeleported.equipment.canEquip = true;
			}
			this.playerTeleported = null;
			yield break;
		}

		private void OnTriggerEnter(Collider other)
		{
			bool flag;
			if (!Dedicator.isDedicated && !string.IsNullOrEmpty(this.achievement) && other.transform.CompareTag("Player") && other.transform == Player.player.transform && Provider.provider.achievementsService.getAchievement(this.achievement, out flag) && !flag)
			{
				Provider.provider.achievementsService.setAchievement(this.achievement);
			}
			if (Provider.isServer && other.transform.CompareTag("Player") && this.playerTeleported == null)
			{
				EffectManager.sendEffect(this.teleportEffect, 16f, this.effectHook.position);
				this.playerTeleported = DamageTool.getPlayer(other.transform);
				base.StartCoroutine("teleport");
			}
		}

		public string achievement;

		public Transform target;

		public ushort teleportEffect;

		public Transform effectHook;

		private Player playerTeleported;
	}
}
