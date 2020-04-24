using System;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	public class UseableFilter : Useable
	{
		private bool isUseable
		{
			get
			{
				return Time.realtimeSinceStartup - this.startedUse > this.useTime;
			}
		}

		private void filter()
		{
			base.player.animator.play("Use", false);
			if (!Dedicator.isDedicated)
			{
				base.player.playSound(((ItemFilterAsset)base.player.equipment.asset).use);
			}
			if (Provider.isServer)
			{
				AlertTool.alert(base.transform.position, 8f);
			}
		}

		[SteamCall]
		public void askFilter(CSteamID steamID)
		{
			if (base.channel.checkServer(steamID) && base.player.equipment.isEquipped)
			{
				this.filter();
			}
		}

		public override void startPrimary()
		{
			if (base.player.equipment.isBusy)
			{
				return;
			}
			if (base.player.clothing.maskAsset == null || !base.player.clothing.maskAsset.proofRadiation || base.player.clothing.maskQuality == 100)
			{
				return;
			}
			base.player.equipment.isBusy = true;
			this.startedUse = Time.realtimeSinceStartup;
			this.isUsing = true;
			if (Provider.isServer)
			{
				base.channel.send("askFilter", ESteamCall.NOT_OWNER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[0]);
			}
			this.filter();
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
					if (base.player.clothing.maskAsset != null && base.player.clothing.maskAsset.proofRadiation && base.player.clothing.maskQuality < 100)
					{
						base.player.equipment.use();
						base.player.clothing.maskQuality = 100;
						base.player.clothing.sendUpdateMaskQuality();
					}
					else
					{
						base.player.equipment.dequip();
					}
				}
			}
		}

		private float startedUse;

		private float useTime;

		private bool isUsing;
	}
}
