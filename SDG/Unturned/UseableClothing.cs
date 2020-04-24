using System;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	public class UseableClothing : Useable
	{
		private bool isUseable
		{
			get
			{
				return Time.realtimeSinceStartup - this.startedUse > this.useTime;
			}
		}

		private void wear()
		{
			base.player.animator.play("Use", false);
			if (Provider.isServer)
			{
				AlertTool.alert(base.transform.position, 8f);
			}
		}

		[SteamCall]
		public void askWear(CSteamID steamID)
		{
			if (base.channel.checkServer(steamID) && base.player.equipment.isEquipped)
			{
				this.wear();
			}
		}

		public override void startPrimary()
		{
			if (base.player.equipment.isBusy)
			{
				return;
			}
			base.player.equipment.isBusy = true;
			this.startedUse = Time.realtimeSinceStartup;
			this.isUsing = true;
			if (Provider.isServer)
			{
				base.channel.send("askWear", ESteamCall.NOT_OWNER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[0]);
			}
			this.wear();
		}

		public override void equip()
		{
			base.player.animator.play("Equip", true);
			this.useTime = base.player.animator.getAnimationLength("Use");
		}

		public override void simulate(uint simulation, bool inputSteady)
		{
			if (this.isUsing && this.isUseable)
			{
				base.player.equipment.isBusy = false;
				this.isUsing = false;
				if (Provider.isServer)
				{
					EItemType type = base.player.equipment.asset.type;
					ushort itemID = base.player.equipment.itemID;
					byte quality = base.player.equipment.quality;
					byte[] state = base.player.equipment.state;
					base.player.equipment.use();
					if (type == EItemType.HAT)
					{
						base.player.clothing.askWearHat(itemID, quality, state, true);
					}
					else if (type == EItemType.SHIRT)
					{
						base.player.clothing.askWearShirt(itemID, quality, state, true);
					}
					else if (type == EItemType.PANTS)
					{
						base.player.clothing.askWearPants(itemID, quality, state, true);
					}
					else if (type == EItemType.BACKPACK)
					{
						base.player.clothing.askWearBackpack(itemID, quality, state, true);
					}
					else if (type == EItemType.VEST)
					{
						base.player.clothing.askWearVest(itemID, quality, state, true);
					}
					else if (type == EItemType.MASK)
					{
						base.player.clothing.askWearMask(itemID, quality, state, true);
					}
					else if (type == EItemType.GLASSES)
					{
						base.player.clothing.askWearGlasses(itemID, quality, state, true);
					}
				}
			}
		}

		private float startedUse;

		private float useTime;

		private bool isUsing;
	}
}
