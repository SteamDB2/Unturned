using System;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	public class UseableFuel : Useable
	{
		private bool isUseable
		{
			get
			{
				return Time.realtimeSinceStartup - this.startedUse > this.useTime;
			}
		}

		private void glug()
		{
			this.startedUse = Time.realtimeSinceStartup;
			this.isUsing = true;
			base.player.animator.play("Use", false);
			if (!Dedicator.isDedicated)
			{
				base.player.playSound(((ItemFuelAsset)base.player.equipment.asset).use);
			}
			if (Provider.isServer)
			{
				AlertTool.alert(base.transform.position, 8f);
			}
		}

		[SteamCall]
		public void askGlug(CSteamID steamID)
		{
			if (base.channel.checkServer(steamID) && base.player.equipment.isEquipped)
			{
				this.glug();
			}
		}

		private bool fire(bool mode)
		{
			if (base.channel.isOwner)
			{
				Ray ray;
				ray..ctor(base.player.look.aim.position, base.player.look.aim.forward);
				RaycastInfo raycastInfo = DamageTool.raycast(ray, 3f, RayMasks.DAMAGE_CLIENT);
				if (raycastInfo.vehicle != null)
				{
					if (mode)
					{
						if (this.fuel == 0)
						{
							return false;
						}
						if (!raycastInfo.vehicle.isRefillable)
						{
							return false;
						}
					}
					else
					{
						if (this.fuel == ((ItemFuelAsset)base.player.equipment.asset).fuel)
						{
							return false;
						}
						if (!raycastInfo.vehicle.isSiphonable)
						{
							return false;
						}
					}
				}
				else
				{
					if (!(raycastInfo.transform != null))
					{
						return false;
					}
					InteractableGenerator component = raycastInfo.transform.GetComponent<InteractableGenerator>();
					InteractableOil component2 = raycastInfo.transform.GetComponent<InteractableOil>();
					InteractableTank component3 = raycastInfo.transform.GetComponent<InteractableTank>();
					InteractableObjectResource component4 = raycastInfo.transform.GetComponent<InteractableObjectResource>();
					if (component != null)
					{
						if (mode)
						{
							if (this.fuel == 0)
							{
								return false;
							}
							if (!component.isRefillable)
							{
								return false;
							}
						}
						else
						{
							if (this.fuel == ((ItemFuelAsset)base.player.equipment.asset).fuel)
							{
								return false;
							}
							if (!component.isSiphonable)
							{
								return false;
							}
						}
					}
					else if (!(component2 != null))
					{
						if (component3 != null)
						{
							if (component3.source != ETankSource.FUEL)
							{
								return false;
							}
							if (mode)
							{
								if (this.fuel == 0)
								{
									return false;
								}
								if (!component3.isRefillable)
								{
									return false;
								}
							}
							else
							{
								if (this.fuel == ((ItemFuelAsset)base.player.equipment.asset).fuel)
								{
									return false;
								}
								if (!component3.isSiphonable)
								{
									return false;
								}
							}
						}
						else
						{
							if (!(component4 != null))
							{
								return false;
							}
							if (component4.objectAsset.interactability != EObjectInteractability.FUEL)
							{
								return false;
							}
							if (mode)
							{
								if (this.fuel == 0)
								{
									return false;
								}
								if (component4.amount == component4.capacity)
								{
									return false;
								}
							}
							else
							{
								if (this.fuel == ((ItemFuelAsset)base.player.equipment.asset).fuel)
								{
									return false;
								}
								if (component4.amount == 0)
								{
									return false;
								}
							}
						}
					}
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
				if (input.type == ERaycastInfoType.VEHICLE)
				{
					if (input.vehicle == null)
					{
						return false;
					}
					if (mode)
					{
						if (this.fuel == 0)
						{
							return false;
						}
						if (!input.vehicle.isRefillable)
						{
							return false;
						}
						ushort num = (ushort)Mathf.Min((int)this.fuel, (int)(input.vehicle.asset.fuel - input.vehicle.fuel));
						input.vehicle.askFillFuel(num);
						this.fuel -= num;
					}
					else
					{
						if (this.fuel == ((ItemFuelAsset)base.player.equipment.asset).fuel)
						{
							return false;
						}
						if (!input.vehicle.isSiphonable)
						{
							return false;
						}
						ushort num2 = (ushort)Mathf.Min((int)input.vehicle.fuel, (int)(((ItemFuelAsset)base.player.equipment.asset).fuel - this.fuel));
						input.vehicle.askBurnFuel(num2);
						VehicleManager.sendVehicleFuel(input.vehicle, input.vehicle.fuel);
						this.fuel += num2;
					}
				}
				else if (input.type == ERaycastInfoType.BARRICADE)
				{
					if (input.transform == null || !input.transform.CompareTag("Barricade"))
					{
						return false;
					}
					InteractableGenerator component5 = input.transform.GetComponent<InteractableGenerator>();
					InteractableOil component6 = input.transform.GetComponent<InteractableOil>();
					InteractableTank component7 = input.transform.GetComponent<InteractableTank>();
					if (component5 != null)
					{
						if (mode)
						{
							if (this.fuel == 0)
							{
								return false;
							}
							if (!component5.isRefillable)
							{
								return false;
							}
							ushort num3 = (ushort)Mathf.Min((int)this.fuel, (int)(component5.capacity - component5.fuel));
							component5.askFill(num3);
							BarricadeManager.sendFuel(input.transform, component5.fuel);
							this.fuel -= num3;
						}
						else
						{
							if (this.fuel == ((ItemFuelAsset)base.player.equipment.asset).fuel)
							{
								return false;
							}
							if (!component5.isSiphonable)
							{
								return false;
							}
							ushort num4 = (ushort)Mathf.Min((int)component5.fuel, (int)(((ItemFuelAsset)base.player.equipment.asset).fuel - this.fuel));
							component5.askBurn(num4);
							BarricadeManager.sendFuel(input.transform, component5.fuel);
							this.fuel += num4;
						}
					}
					else if (component6 != null)
					{
						if (mode)
						{
							if (this.fuel == 0)
							{
								return false;
							}
							if (!component6.isRefillable)
							{
								return false;
							}
							ushort num5 = (ushort)Mathf.Min((int)this.fuel, (int)(component6.capacity - component6.fuel));
							component6.askFill(num5);
							BarricadeManager.sendOil(input.transform, component6.fuel);
							this.fuel -= num5;
						}
						else
						{
							if (this.fuel == ((ItemFuelAsset)base.player.equipment.asset).fuel)
							{
								return false;
							}
							if (!component6.isSiphonable)
							{
								return false;
							}
							ushort num6 = (ushort)Mathf.Min((int)component6.fuel, (int)(((ItemFuelAsset)base.player.equipment.asset).fuel - this.fuel));
							component6.askBurn(num6);
							BarricadeManager.sendOil(input.transform, component6.fuel);
							this.fuel += num6;
						}
					}
					else
					{
						if (!(component7 != null))
						{
							return false;
						}
						if (component7.source != ETankSource.FUEL)
						{
							return false;
						}
						if (mode)
						{
							if (this.fuel == 0)
							{
								return false;
							}
							if (!component7.isRefillable)
							{
								return false;
							}
							ushort num7 = (ushort)Mathf.Min((int)this.fuel, (int)(component7.capacity - component7.amount));
							BarricadeManager.updateTank(input.transform, component7.amount + num7);
							this.fuel -= num7;
						}
						else
						{
							if (this.fuel == ((ItemFuelAsset)base.player.equipment.asset).fuel)
							{
								return false;
							}
							if (!component7.isSiphonable)
							{
								return false;
							}
							ushort num8 = (ushort)Mathf.Min((int)component7.amount, (int)(((ItemFuelAsset)base.player.equipment.asset).fuel - this.fuel));
							BarricadeManager.updateTank(input.transform, component7.amount - num8);
							this.fuel += num8;
						}
					}
				}
				else if (input.type == ERaycastInfoType.OBJECT)
				{
					if (input.transform == null)
					{
						return false;
					}
					InteractableObjectResource component8 = input.transform.GetComponent<InteractableObjectResource>();
					if (component8 == null || component8.objectAsset.interactability != EObjectInteractability.FUEL)
					{
						return false;
					}
					if (mode)
					{
						if (this.fuel == 0)
						{
							return false;
						}
						if (!component8.isRefillable)
						{
							return false;
						}
						ushort num9 = (ushort)Mathf.Min((int)this.fuel, (int)(component8.capacity - component8.amount));
						ObjectManager.updateObjectResource(component8.transform, component8.amount + num9, true);
						this.fuel -= num9;
					}
					else
					{
						if (this.fuel == ((ItemFuelAsset)base.player.equipment.asset).fuel)
						{
							return false;
						}
						if (!component8.isSiphonable)
						{
							return false;
						}
						ushort num10 = (ushort)Mathf.Min((int)component8.amount, (int)(((ItemFuelAsset)base.player.equipment.asset).fuel - this.fuel));
						ObjectManager.updateObjectResource(component8.transform, component8.amount - num10, true);
						this.fuel += num10;
					}
				}
			}
			return true;
		}

		private void start(bool mode)
		{
			if (base.player.equipment.isBusy)
			{
				return;
			}
			if (this.isUseable && this.fire(mode))
			{
				if (Provider.isServer)
				{
					byte[] bytes = BitConverter.GetBytes(this.fuel);
					base.player.equipment.state[0] = bytes[0];
					base.player.equipment.state[1] = bytes[1];
					base.player.equipment.sendUpdateState();
				}
				base.player.equipment.isBusy = true;
				this.startedUse = Time.realtimeSinceStartup;
				this.isUsing = true;
				this.glug();
				if (Provider.isServer)
				{
					base.channel.send("askGlug", ESteamCall.NOT_OWNER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[0]);
				}
			}
		}

		public override void startPrimary()
		{
			this.start(true);
		}

		public override void startSecondary()
		{
			this.start(false);
		}

		public override void updateState(byte[] newState)
		{
			if (base.channel.isOwner)
			{
				this.fuel = BitConverter.ToUInt16(newState, 0);
				PlayerUI.message(EPlayerMessage.FUEL, ((int)((float)this.fuel / (float)((ItemFuelAsset)base.player.equipment.asset).fuel * 100f)).ToString());
			}
		}

		public override void equip()
		{
			if (base.channel.isOwner || Provider.isServer)
			{
				if (base.player.equipment.state.Length < 2)
				{
					base.player.equipment.state = ((ItemFuelAsset)base.player.equipment.asset).getState(EItemOrigin.ADMIN);
					base.player.equipment.updateState();
				}
				this.fuel = BitConverter.ToUInt16(base.player.equipment.state, 0);
			}
			base.player.animator.play("Equip", true);
			this.useTime = base.player.animator.getAnimationLength("Use");
			if (base.channel.isOwner)
			{
				PlayerUI.message(EPlayerMessage.FUEL, ((int)((float)this.fuel / (float)((ItemFuelAsset)base.player.equipment.asset).fuel * 100f)).ToString());
			}
		}

		public override void simulate(uint simulation, bool inputSteady)
		{
			if (this.isUsing && this.isUseable)
			{
				base.player.equipment.isBusy = false;
				this.isUsing = false;
			}
		}

		private float startedUse;

		private float useTime;

		private bool isUsing;

		private ushort fuel;
	}
}
