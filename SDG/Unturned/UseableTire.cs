using System;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	public class UseableTire : Useable
	{
		private bool isUseable
		{
			get
			{
				return Time.realtimeSinceStartup - this.startedUse > this.useTime;
			}
		}

		private bool isAttachable
		{
			get
			{
				return Time.realtimeSinceStartup - this.startedUse > this.useTime * 0.75f;
			}
		}

		private void attach()
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
		public void askAttach(CSteamID steamID)
		{
			if (base.channel.checkServer(steamID) && base.player.equipment.isEquipped)
			{
				this.attach();
			}
		}

		private bool fire()
		{
			if (base.channel.isOwner)
			{
				Ray ray;
				ray..ctor(base.player.look.aim.position, base.player.look.aim.forward);
				RaycastInfo raycastInfo = DamageTool.raycast(ray, 3f, RayMasks.DAMAGE_CLIENT);
				if (raycastInfo.vehicle == null || !raycastInfo.vehicle.isTireReplaceable)
				{
					return false;
				}
				int closestAliveTireIndex = raycastInfo.vehicle.getClosestAliveTireIndex(raycastInfo.point, ((ItemTireAsset)base.player.equipment.asset).mode == EUseableTireMode.REMOVE);
				if (closestAliveTireIndex == -1)
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
				if (input.vehicle == null || !input.vehicle.isTireReplaceable)
				{
					return false;
				}
				int closestAliveTireIndex2 = input.vehicle.getClosestAliveTireIndex(input.point, ((ItemTireAsset)base.player.equipment.asset).mode == EUseableTireMode.REMOVE);
				if (closestAliveTireIndex2 == -1)
				{
					return false;
				}
				this.isAttaching = true;
				this.vehicle = input.vehicle;
				this.vehicleWheelIndex = closestAliveTireIndex2;
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
				this.attach();
				if (Provider.isServer)
				{
					base.channel.send("askAttach", ESteamCall.NOT_OWNER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[0]);
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
			if (this.isAttaching && this.isAttachable)
			{
				this.isAttaching = false;
				if (this.vehicle != null && this.vehicle.isTireReplaceable && this.vehicleWheelIndex != -1)
				{
					if (((ItemTireAsset)base.player.equipment.asset).mode == EUseableTireMode.ADD)
					{
						if (!this.vehicle.tires[this.vehicleWheelIndex].isAlive)
						{
							this.vehicle.askRepairTire(this.vehicleWheelIndex);
						}
					}
					else if (this.vehicle.tires[this.vehicleWheelIndex].isAlive)
					{
						this.vehicle.askDamageTire(this.vehicleWheelIndex);
						base.player.inventory.forceAddItem(new Item(1451, true), false);
					}
					this.vehicle = null;
				}
				if (((ItemTireAsset)base.player.equipment.asset).mode == EUseableTireMode.ADD)
				{
					base.player.equipment.useStepA();
				}
			}
			if (this.isUsing && this.isUseable)
			{
				base.player.equipment.isBusy = false;
				this.isUsing = false;
				if (((ItemTireAsset)base.player.equipment.asset).mode == EUseableTireMode.ADD)
				{
					base.player.equipment.useStepB();
				}
			}
		}

		private float startedUse;

		private float useTime;

		private bool isUsing;

		private bool isAttaching;

		private InteractableVehicle vehicle;

		private int vehicleWheelIndex = -1;
	}
}
