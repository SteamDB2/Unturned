using System;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	public class UseableDetonator : Useable
	{
		private bool isUseable
		{
			get
			{
				return Time.realtimeSinceStartup - this.startedUse > this.useTime;
			}
		}

		private bool isDetonatable
		{
			get
			{
				return Time.realtimeSinceStartup - this.startedUse > this.useTime * 0.33f;
			}
		}

		private void plunge()
		{
			this.startedUse = Time.realtimeSinceStartup;
			this.isUsing = true;
			base.player.animator.play("Use", false);
			if (!Dedicator.isDedicated)
			{
				base.player.playSound(((ItemDetonatorAsset)base.player.equipment.asset).use);
			}
			if (Provider.isServer)
			{
				AlertTool.alert(base.transform.position, 8f);
			}
		}

		[SteamCall]
		public void askPlunge(CSteamID steamID)
		{
			if (base.channel.checkServer(steamID) && base.player.equipment.isEquipped)
			{
				this.plunge();
			}
		}

		public override void startPrimary()
		{
			if (base.player.equipment.isBusy)
			{
				return;
			}
			if (this.isUseable)
			{
				if (base.channel.isOwner)
				{
					for (int i = 0; i < this.charges.Count; i++)
					{
						InteractableCharge interactableCharge = this.charges[i];
						if (!(interactableCharge == null))
						{
							RaycastInfo info = new RaycastInfo(interactableCharge.transform);
							base.player.input.sendRaycast(info);
						}
					}
					this.charges.Clear();
				}
				if (Provider.isServer)
				{
					this.charges.Clear();
					if (base.player.input.hasInputs())
					{
						int inputCount = base.player.input.getInputCount();
						for (int j = 0; j < inputCount; j++)
						{
							InputInfo input = base.player.input.getInput(false);
							if (input != null)
							{
								if (input.type == ERaycastInfoType.BARRICADE)
								{
									if (!(input.transform == null) && input.transform.CompareTag("Barricade"))
									{
										InteractableCharge component = input.transform.GetComponent<InteractableCharge>();
										if (!(component == null))
										{
											if (!((!Dedicator.isDedicated) ? (!component.hasOwnership) : (!OwnershipTool.checkToggle(base.channel.owner.playerID.steamID, component.owner, base.player.quests.groupID, component.group))))
											{
												this.charges.Add(component);
											}
										}
									}
								}
							}
						}
					}
				}
				base.player.equipment.isBusy = true;
				this.startedUse = Time.realtimeSinceStartup;
				this.isUsing = true;
				this.isDetonating = true;
				this.plunge();
				if (Provider.isServer)
				{
					base.player.life.markAggressive(false, true);
					base.channel.send("askPlunge", ESteamCall.NOT_OWNER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[0]);
				}
			}
		}

		public override void startSecondary()
		{
			if (base.player.equipment.isBusy)
			{
				return;
			}
			if (base.channel.isOwner && !this.isUsing && this.target != null)
			{
				if (this.target.isSelected)
				{
					this.target.deselect();
					this.charges.Remove(this.target);
				}
				else
				{
					this.target.select();
					this.charges.Add(this.target);
					if (this.charges.Count > 8)
					{
						if (this.charges[0] != null)
						{
							this.charges[0].deselect();
						}
						this.charges.RemoveAt(0);
					}
				}
			}
		}

		public override void equip()
		{
			base.player.animator.play("Equip", true);
			this.useTime = base.player.animator.getAnimationLength("Use");
			if (base.channel.isOwner)
			{
				this.chargePoint = Vector3.zero;
				this.foundInRadius = new List<InteractableCharge>();
				this.chargesInRadius = new List<InteractableCharge>();
			}
			this.charges = new List<InteractableCharge>();
		}

		public override void dequip()
		{
			if (base.channel.isOwner)
			{
				for (int i = 0; i < this.chargesInRadius.Count; i++)
				{
					InteractableCharge interactableCharge = this.chargesInRadius[i];
					interactableCharge.unhighlight();
				}
			}
		}

		public override void simulate(uint simulation, bool inputSteady)
		{
			if (this.isDetonating && this.isDetonatable)
			{
				if (this.charges.Count > 0)
				{
					if (simulation - this.lastExplosion > 1u)
					{
						this.lastExplosion = simulation;
						InteractableCharge interactableCharge = this.charges[0];
						this.charges.RemoveAt(0);
						if (interactableCharge != null)
						{
							interactableCharge.detonate(base.channel.owner.playerID.steamID);
						}
					}
				}
				else
				{
					this.isDetonating = false;
				}
			}
			if (this.isUsing && this.isUseable && this.charges.Count == 0)
			{
				base.player.equipment.isBusy = false;
				this.isUsing = false;
			}
		}

		public override void tick()
		{
			if (base.channel.isOwner)
			{
				if ((base.transform.position - this.chargePoint).sqrMagnitude > 1f)
				{
					this.chargePoint = base.transform.position;
					this.foundInRadius.Clear();
					PowerTool.checkInteractables<InteractableCharge>(this.chargePoint, 64f, this.foundInRadius);
					for (int i = this.chargesInRadius.Count - 1; i >= 0; i--)
					{
						InteractableCharge interactableCharge = this.chargesInRadius[i];
						if (interactableCharge == null)
						{
							this.chargesInRadius.RemoveAtFast(i);
						}
						else if (!this.foundInRadius.Contains(interactableCharge))
						{
							interactableCharge.unhighlight();
							this.chargesInRadius.RemoveAtFast(i);
						}
					}
					for (int j = 0; j < this.foundInRadius.Count; j++)
					{
						InteractableCharge interactableCharge2 = this.foundInRadius[j];
						if (!(interactableCharge2 == null))
						{
							if (interactableCharge2.hasOwnership)
							{
								if (!this.chargesInRadius.Contains(interactableCharge2))
								{
									interactableCharge2.highlight();
									this.chargesInRadius.Add(interactableCharge2);
								}
							}
						}
					}
				}
				InteractableCharge interactableCharge3 = null;
				float num = 0.98f;
				for (int k = 0; k < this.chargesInRadius.Count; k++)
				{
					InteractableCharge interactableCharge4 = this.chargesInRadius[k];
					if (!(interactableCharge4 == null))
					{
						float num2 = Vector3.Dot((interactableCharge4.transform.position - MainCamera.instance.transform.position).normalized, MainCamera.instance.transform.forward);
						if (num2 > num)
						{
							interactableCharge3 = interactableCharge4;
							num = num2;
						}
					}
				}
				if (interactableCharge3 != this.target)
				{
					if (this.target != null)
					{
						this.target.untarget();
					}
					this.target = interactableCharge3;
					if (this.target != null)
					{
						this.target.target();
					}
				}
			}
		}

		private float startedUse;

		private float useTime;

		private uint lastExplosion;

		private bool isUsing;

		private bool isDetonating;

		private Vector3 chargePoint;

		private List<InteractableCharge> foundInRadius;

		private List<InteractableCharge> chargesInRadius;

		private List<InteractableCharge> charges;

		private InteractableCharge target;
	}
}
