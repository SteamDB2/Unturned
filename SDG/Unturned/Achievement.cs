using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class Achievement : MonoBehaviour
	{
		private void OnTriggerEnter(Collider other)
		{
			if (Dedicator.isDedicated || !other.transform.CompareTag("Player") || other.transform != Player.player.transform)
			{
				return;
			}
			bool flag;
			if (Provider.provider.achievementsService.getAchievement(base.transform.name, out flag) && !flag)
			{
				Provider.provider.achievementsService.setAchievement(base.transform.name);
			}
		}
	}
}
