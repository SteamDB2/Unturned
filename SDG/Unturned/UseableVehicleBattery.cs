using System;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	public class UseableVehicleBattery : Useable
	{
		private bool isUseable
		{
			get
			{
				return Time.realtimeSinceStartup - this.startedUse > this.useTime;
			}
		}

		private bool isReplaceable
		{
			get
			{
				return Time.realtimeSinceStartup - this.startedUse > this.useTime * 0.75f;
			}
		}

		private void replace()
		{
			this.startedUse = Time.realtimeSinceStartup;
			this.isUsing = true;
			base.player.animator.play("Use", false);
			if (!Dedicator.isDedicated)
			{
				base.player.playSound(((ItemToolAsset)base.player.equipment.asset).use);
			}
			if (Provider.isServer)
			{
				AlertTool.alert(base.transform.position, 8f);
			}
		}

		[SteamCall]
		public void askReplace(CSteamID steamID)
		{
			if (base.channel.checkServer(steamID) && base.player.equipment.isEquipped)
			{
				this.replace();
			}
		}

		private bool fire()
		{
			if (base.channel.isOwner)
			{
				Ray ray;
				ray..ctor(base.player.look.aim.position, base.player.look.aim.forward);
				RaycastInfo raycastInfo = DamageTool.raycast(ray, 3f, RayMasks.DAMAGE_CLIENT);
				if (raycastInfo.vehicle == null || !raycastInfo.vehicle.isBatteryReplaceable)
				{
					return false;
				}
				base.player.input.sendRaycast(raycastInfo);
			}
			if (Provider.isServer)
			{
				if (!base.player.input.hasInputs())
				{
					return false;
				}
				InputInfo input = base.player.input.getInput(true);
				if (input == null)
				{
					return false;
				}
				if ((input.point - base.player.look.aim.position).sqrMagnitude > 49f)
				{
					return false;
				}
				if (input.type != ERaycastInfoType.VEHICLE)
				{
					return false;
				}
				if (input.vehicle == null || !input.vehicle.isBatteryReplaceable)
				{
					return false;
				}
				this.isReplacing = true;
				this.vehicle = input.vehicle;
			}
			return true;
		}

		public override void startPrimary()
		{
			if (base.player.equipment.isBusy)
			{
				return;
			}
			if (this.isUseable && this.fire())
			{
				base.player.equipment.isBusy = true;
				this.startedUse = Time.realtimeSinceStartup;
				this.isUsing = true;
				this.replace();
				if (Provider.isServer)
				{
					base.channel.send("askReplace", ESteamCall.NOT_OWNER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[0]);
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
			if (this.isReplacing && this.isReplaceable)
			{
				this.isReplacing = false;
				if (this.vehicle != null && this.vehicle.isBatteryReplaceable)
				{
					this.vehicle.replaceBattery(base.player, base.player.equipment.quality);
					this.vehicle = null;
				}
				base.player.equipment.useStepA();
			}
			if (this.isUsing && this.isUseable)
			{
				base.player.equipment.isBusy = false;
				this.isUsing = false;
				base.player.equipment.useStepB();
			}
		}

		private float startedUse;

		private float useTime;

		private bool isUsing;

		private bool isReplacing;

		private InteractableVehicle vehicle;
	}
}
