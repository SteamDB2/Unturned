using System;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	public class Barrier : MonoBehaviour
	{
		private void OnTriggerEnter(Collider other)
		{
			if (Provider.isServer && other.transform.CompareTag("Player"))
			{
				Player player = DamageTool.getPlayer(other.transform);
				if (player != null)
				{
					EPlayerKill eplayerKill;
					player.life.askDamage(101, Vector3.up * 10f, EDeathCause.SUICIDE, ELimb.SKULL, CSteamID.Nil, out eplayerKill);
				}
			}
		}
	}
}
