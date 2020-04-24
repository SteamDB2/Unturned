using System;
using SDG.Framework.Devkit;
using UnityEngine;

namespace SDG.Unturned
{
	public class NPCVehicleReward : INPCReward
	{
		public NPCVehicleReward(ushort newID, string newSpawnpoint, string newText) : base(newText)
		{
			this.id = newID;
			this.spawnpoint = newSpawnpoint;
		}

		public ushort id { get; protected set; }

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
				Debug.LogError("Failed to find NPC vehicle reward spawnpoint: " + this.spawnpoint);
				return;
			}
			VehicleManager.spawnVehicle(this.id, spawnpoint.transform.position, spawnpoint.transform.rotation);
		}

		public override string formatReward(Player player)
		{
			if (string.IsNullOrEmpty(this.text))
			{
				this.text = PlayerNPCQuestUI.localization.read("Reward_Vehicle");
			}
			VehicleAsset vehicleAsset = Assets.find(EAssetType.VEHICLE, this.id) as VehicleAsset;
			string arg;
			if (vehicleAsset != null)
			{
				arg = string.Concat(new string[]
				{
					"<color=",
					Palette.hex(ItemTool.getRarityColorUI(vehicleAsset.rarity)),
					">",
					vehicleAsset.vehicleName,
					"</color>"
				});
			}
			else
			{
				arg = "?";
			}
			return string.Format(this.text, arg);
		}

		public override Sleek createUI(Player player)
		{
			string text = this.formatReward(player);
			if (string.IsNullOrEmpty(text))
			{
				return null;
			}
			if (!(Assets.find(EAssetType.VEHICLE, this.id) is VehicleAsset))
			{
				return null;
			}
			SleekBox sleekBox = new SleekBox();
			sleekBox.sizeOffset_Y = 30;
			sleekBox.sizeScale_X = 1f;
			sleekBox.add(new SleekLabel
			{
				positionOffset_X = 5,
				sizeOffset_X = -10,
				sizeScale_X = 1f,
				sizeScale_Y = 1f,
				fontAlignment = 3,
				foregroundTint = ESleekTint.NONE,
				isRich = true,
				text = text
			});
			return sleekBox;
		}
	}
}
