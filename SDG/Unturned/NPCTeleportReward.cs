using System;
using SDG.Framework.Devkit;
using UnityEngine;

namespace SDG.Unturned
{
	public class NPCTeleportReward : INPCReward
	{
		public NPCTeleportReward(string newSpawnpoint, string newText) : base(newText)
		{
			this.spawnpoint = newSpawnpoint;
		}

		public string spawnpoint { get; protected set; }

		public override void grantReward(Player player, bool shouldSend)
		{
			if (!Provider.isServer)
			{
				return;
			}
			Spawnpoint spawnpoint = SpawnpointSystem.getSpawnpoint(this.spawnpoint);
			if (spawnpoint == null)
			{
				Debug.LogError("Failed to find NPC teleport reward spawnpoint: " + this.spawnpoint);
				return;
			}
			player.sendTeleport(spawnpoint.transform.position, MeasurementTool.angleToByte(spawnpoint.transform.rotation.eulerAngles.y));
		}
	}
}
