using System;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	public class UseableArrestEnd : Useable
	{
		private bool isUseable
		{
			get
			{
				return Time.realtimeSinceStartup - this.startedUse > this.useTime;
			}
		}

		private void arrest()
		{
			base.player.animator.play("Use", false);
			if (!Dedicator.isDedicated)
			{
				base.player.playSound(((ItemArrestEndAsset)base.player.equipment.asset).use);
			}
		}

		[SteamCall]
		public void askArrest(CSteamID steamID)
		{
			if (base.channel.checkServer(steamID) && base.player.equipment.isEquipped)
			{
				this.arrest();
			}
		}

		public override void startPrimary()
		{
			if (base.player.equipment.isBusy)
			{
				return;
			}
			if (base.channel.isOwner)
			{
				Ray ray;
				ray..ctor(base.player.look.aim.position, base.player.look.aim.forward);
				RaycastInfo raycastInfo = DamageTool.raycast(ray, 3f, RayMasks.DAMAGE_CLIENT);
				if (raycastInfo.player != null && raycastInfo.player.animator.gesture == EPlayerGesture.ARREST_START)
				{
					base.player.input.sendRaycast(raycastInfo);
					if (!Provider.isServer)
					{
						base.player.equipment.isBusy = true;
						this.startedUse = Time.realtimeSinceStartup;
						this.isUsing = true;
						this.arrest();
					}
				}
			}
			if (Provider.isServer)
			{
				if (!base.player.input.hasInputs())
				{
					return;
				}
				InputInfo input = base.player.input.getInput(true);
				if (input == null)
				{
					return;
				}
				if (input.type == ERaycastInfoType.PLAYER && input.player != null)
				{
					this.enemy = input.player;
					base.player.equipment.isBusy = true;
					this.startedUse = Time.realtimeSinceStartup;
					this.isUsing = true;
					this.arrest();
					base.channel.send("askArrest", ESteamCall.NOT_OWNER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[0]);
				}
			}
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
					if (this.enemy != null && this.enemy.animator.gesture == EPlayerGesture.ARREST_START && this.enemy.animator.captorID == base.channel.owner.playerID.steamID && this.enemy.animator.captorItem == ((ItemArrestEndAsset)base.player.equipment.asset).recover)
					{
						this.enemy.animator.captorID = CSteamID.Nil;
						this.enemy.animator.captorStrength = 0;
						this.enemy.animator.sendGesture(EPlayerGesture.ARREST_STOP, true);
						base.player.inventory.forceAddItem(new Item(((ItemArrestEndAsset)base.player.equipment.asset).recover, EItemOrigin.NATURE), false);
					}
					base.player.equipment.dequip();
				}
			}
		}

		private float startedUse;

		private float useTime;

		private bool isUsing;

		private Player enemy;
	}
}
