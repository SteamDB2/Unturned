using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class Bumper : MonoBehaviour
	{
		public void init(InteractableVehicle newVehicle)
		{
			this.vehicle = newVehicle;
		}

		private void OnTriggerEnter(Collider other)
		{
			if (Provider.isServer)
			{
				if (other.isTrigger)
				{
					return;
				}
				if (other.CompareTag("Debris"))
				{
					return;
				}
				float num = Mathf.Clamp(this.vehicle.speed * this.vehicle.asset.bumperMultiplier, 0f, 10f);
				if (num < 3f)
				{
					return;
				}
				if (other.transform.parent.CompareTag("Vehicle"))
				{
					return;
				}
				if (other.transform.CompareTag("Player"))
				{
					if (Provider.isPvP && this.vehicle.isDriven)
					{
						Player player = DamageTool.getPlayer(other.transform);
						if (player != null && player.movement.getVehicle() == null && !this.vehicle.passengers[0].player.player.quests.isMemberOfSameGroupAs(player))
						{
							EPlayerKill eplayerKill;
							DamageTool.damage(player, EDeathCause.ROADKILL, ELimb.SPINE, this.vehicle.passengers[0].player.playerID.steamID, base.transform.forward, Bumper.DAMAGE_PLAYER, num, out eplayerKill);
							EffectManager.sendEffect(5, EffectManager.SMALL, other.transform.position + other.transform.up, -base.transform.forward);
							this.vehicle.askDamage(2, true);
						}
					}
				}
				else if (other.transform.CompareTag("Agent"))
				{
					Zombie zombie = DamageTool.getZombie(other.transform);
					if (zombie != null)
					{
						EPlayerKill eplayerKill2;
						uint num2;
						DamageTool.damage(zombie, base.transform.forward, Bumper.DAMAGE_ZOMBIE, num, out eplayerKill2, out num2);
						EffectManager.sendEffect((!zombie.isRadioactive) ? 5 : 95, EffectManager.SMALL, other.transform.position + other.transform.up, -base.transform.forward);
						this.vehicle.askDamage(2, true);
					}
					else
					{
						Animal animal = DamageTool.getAnimal(other.transform);
						if (animal != null)
						{
							EPlayerKill eplayerKill3;
							uint num3;
							DamageTool.damage(animal, base.transform.forward, Bumper.DAMAGE_ANIMAL, num, out eplayerKill3, out num3);
							EffectManager.sendEffect(5, EffectManager.SMALL, other.transform.position + other.transform.up, -base.transform.forward);
							this.vehicle.askDamage(2, true);
						}
					}
				}
				else
				{
					if (!other.transform.CompareTag("Barricade"))
					{
						if (!other.transform.CompareTag("Structure"))
						{
							if (other.transform.CompareTag("Resource"))
							{
								DamageTool.impact(base.transform.position + base.transform.forward * ((BoxCollider)base.transform.GetComponent<Collider>()).size.z / 2f, -base.transform.forward, DamageTool.getMaterial(base.transform.position, other.transform, other.GetComponent<Collider>()), true);
								EPlayerKill eplayerKill4;
								uint num4;
								ResourceManager.damage(other.transform, base.transform.forward, Bumper.DAMAGE_RESOURCE, num, 1f, out eplayerKill4, out num4);
								this.vehicle.askDamage((ushort)(Bumper.DAMAGE_VEHICLE * num), true);
							}
							else
							{
								InteractableObjectRubble componentInParent = other.transform.GetComponentInParent<InteractableObjectRubble>();
								if (componentInParent != null)
								{
									EPlayerKill eplayerKill5;
									uint num5;
									DamageTool.damage(componentInParent.transform, base.transform.forward, componentInParent.getSection(other.transform), Bumper.DAMAGE_OBJECT, num, out eplayerKill5, out num5);
									if (Time.realtimeSinceStartup - this.lastDamageImpact > 0.2f)
									{
										this.lastDamageImpact = Time.realtimeSinceStartup;
										DamageTool.impact(base.transform.position + base.transform.forward * ((BoxCollider)base.transform.GetComponent<Collider>()).size.z / 2f, -base.transform.forward, DamageTool.getMaterial(base.transform.position, other.transform, other.GetComponent<Collider>()), true);
										this.vehicle.askDamage((ushort)(Bumper.DAMAGE_VEHICLE * num), true);
									}
								}
								else if (Time.realtimeSinceStartup - this.lastDamageImpact > 0.2f)
								{
									ObjectAsset asset = LevelObjects.getAsset(other.transform);
									if (asset != null && !asset.isSoft)
									{
										this.lastDamageImpact = Time.realtimeSinceStartup;
										DamageTool.impact(base.transform.position + base.transform.forward * ((BoxCollider)base.transform.GetComponent<Collider>()).size.z / 2f, -base.transform.forward, DamageTool.getMaterial(base.transform.position, other.transform, other.GetComponent<Collider>()), true);
										this.vehicle.askDamage((ushort)(Bumper.DAMAGE_VEHICLE * num), true);
									}
								}
							}
						}
					}
					if (!this.vehicle.isDead && !other.transform.CompareTag("Border") && ((this.vehicle.asset.engine == EEngine.PLANE && this.vehicle.speed > 20f) || (this.vehicle.asset.engine == EEngine.HELICOPTER && this.vehicle.speed > 10f)))
					{
						this.vehicle.askDamage(20000, false);
					}
				}
			}
		}

		private static readonly float DAMAGE_PLAYER = 10f;

		private static readonly float DAMAGE_ZOMBIE = 15f;

		private static readonly float DAMAGE_ANIMAL = 15f;

		private static readonly float DAMAGE_OBJECT = 30f;

		private static readonly float DAMAGE_VEHICLE = 8f;

		private static readonly float DAMAGE_RESOURCE = 85f;

		private InteractableVehicle vehicle;

		private float lastDamageImpact;
	}
}
