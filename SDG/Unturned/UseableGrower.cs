using System;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	public class UseableGrower : Useable
	{
		private bool isUseable
		{
			get
			{
				return Time.realtimeSinceStartup - this.startedUse > this.useTime;
			}
		}

		private void grow()
		{
			this.startedUse = Time.realtimeSinceStartup;
			this.isUsing = true;
			base.player.animator.play("Use", false);
			if (!Dedicator.isDedicated)
			{
				base.player.playSound(((ItemGrowerAsset)base.player.equipment.asset).use);
			}
			if (Provider.isServer)
			{
				AlertTool.alert(base.transform.position, 8f);
			}
		}

		[SteamCall]
		public void askGrow(CSteamID steamID)
		{
			if (base.channel.checkServer(steamID) && base.player.equipment.isEquipped)
			{
				this.grow();
			}
		}

		private bool fire()
		{
			if (base.channel.isOwner)
			{
				Ray ray;
				ray..ctor(base.player.look.aim.position, base.player.look.aim.forward);
				RaycastInfo raycastInfo = DamageTool.raycast(ray, 3f, RayMasks.DAMAGE_CLIENT);
				if (raycastInfo.transform == null || !raycastInfo.transform.CompareTag("Barricade"))
				{
					return false;
				}
				InteractableFarm component = raycastInfo.transform.GetComponent<InteractableFarm>();
				if (component == null)
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
				if (input.type != ERaycastInfoType.BARRICADE)
				{
					return false;
				}
				if (input.transform == null || !input.transform.CompareTag("Barricade"))
				{
					return false;
				}
				this.farm = input.transform.GetComponent<InteractableFarm>();
				if (this.farm == null || this.farm.checkFarm())
				{
					return false;
				}
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
				this.grow();
				if (Provider.isServer)
				{
					base.channel.send("askGrow", ESteamCall.NOT_OWNER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[0]);
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
					if (this.farm != null && !this.farm.checkFarm())
					{
						BarricadeManager.updateFarm(this.farm.transform, 1u, true);
						base.player.equipment.use();
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

		private InteractableFarm farm;
	}
}
