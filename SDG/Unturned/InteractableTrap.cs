using System;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	public class InteractableTrap : Interactable
	{
		public override void updateState(Asset asset, byte[] state)
		{
			this.range2 = ((ItemTrapAsset)asset).range2;
			this.playerDamage = ((ItemTrapAsset)asset).playerDamage;
			this.zombieDamage = ((ItemTrapAsset)asset).zombieDamage;
			this.animalDamage = ((ItemTrapAsset)asset).animalDamage;
			this.barricadeDamage = ((ItemTrapAsset)asset).barricadeDamage;
			this.structureDamage = ((ItemTrapAsset)asset).structureDamage;
			this.vehicleDamage = ((ItemTrapAsset)asset).vehicleDamage;
			this.resourceDamage = ((ItemTrapAsset)asset).resourceDamage;
			this.objectDamage = ((ItemTrapAsset)asset).objectDamage;
			this.explosion2 = ((ItemTrapAsset)asset).explosion2;
			this.isBroken = ((ItemTrapAsset)asset).isBroken;
			this.isExplosive = ((ItemTrapAsset)asset).isExplosive;
			if (((ItemTrapAsset)asset).damageTires)
			{
				base.transform.parent.getOrAddComponent<InteractableTrapDamageTires>();
			}
		}

		public override bool checkInteractable()
		{
			return false;
		}

		private void OnEnable()
		{
			this.lastActive = Time.realtimeSinceStartup;
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.isTrigger)
			{
				return;
			}
			if (Time.realtimeSinceStartup - this.lastActive < 0.25f)
			{
				return;
			}
			if (other.transform == base.transform.parent)
			{
				return;
			}
			if (Provider.isServer)
			{
				if (this.isExplosive)
				{
					if (other.transform.CompareTag("Player"))
					{
						if (Provider.isPvP && !other.transform.parent.CompareTag("Vehicle"))
						{
							EffectManager.sendEffect(this.explosion2, EffectManager.LARGE, base.transform.position);
							DamageTool.explode(base.transform.position, this.range2, EDeathCause.LANDMINE, CSteamID.Nil, this.playerDamage, this.zombieDamage, this.animalDamage, this.barricadeDamage, this.structureDamage, this.vehicleDamage, this.resourceDamage, this.objectDamage, EExplosionDamageType.CONVENTIONAL, 32f, true);
						}
					}
					else
					{
						EffectManager.sendEffect(this.explosion2, EffectManager.LARGE, base.transform.position);
						DamageTool.explode(base.transform.position, this.range2, EDeathCause.LANDMINE, CSteamID.Nil, this.playerDamage, this.zombieDamage, this.animalDamage, this.barricadeDamage, this.structureDamage, this.vehicleDamage, this.resourceDamage, this.objectDamage, EExplosionDamageType.CONVENTIONAL, 32f, true);
					}
				}
				else if (other.transform.CompareTag("Player"))
				{
					if (Provider.isPvP && !other.transform.parent.CompareTag("Vehicle"))
					{
						Player player = DamageTool.getPlayer(other.transform);
						if (player != null)
						{
							EPlayerKill eplayerKill;
							DamageTool.damage(player, EDeathCause.SHRED, ELimb.SPINE, CSteamID.Nil, Vector3.up, this.playerDamage, 1f, out eplayerKill);
							if (this.isBroken)
							{
								player.life.breakLegs();
							}
							EffectManager.sendEffect(5, EffectManager.SMALL, base.transform.position + Vector3.up, Vector3.down);
							BarricadeManager.damage(base.transform.parent, 5f, 1f, false);
						}
					}
				}
				else if (other.transform.CompareTag("Agent"))
				{
					Zombie zombie = DamageTool.getZombie(other.transform);
					if (zombie != null)
					{
						EPlayerKill eplayerKill2;
						uint num;
						DamageTool.damage(zombie, base.transform.forward, this.zombieDamage, 1f, out eplayerKill2, out num);
						EffectManager.sendEffect((!zombie.isRadioactive) ? 5 : 95, EffectManager.SMALL, base.transform.position + Vector3.up, Vector3.down);
						BarricadeManager.damage(base.transform.parent, (!zombie.isHyper) ? 5f : 10f, 1f, false);
					}
					else
					{
						Animal animal = DamageTool.getAnimal(other.transform);
						if (animal != null)
						{
							EPlayerKill eplayerKill3;
							uint num2;
							DamageTool.damage(animal, base.transform.forward, this.animalDamage, 1f, out eplayerKill3, out num2);
							EffectManager.sendEffect(5, EffectManager.SMALL, base.transform.position + Vector3.up, Vector3.down);
							BarricadeManager.damage(base.transform.parent, 5f, 1f, false);
						}
					}
				}
			}
		}

		private float range2;

		private float playerDamage;

		private float zombieDamage;

		private float animalDamage;

		private float barricadeDamage;

		private float structureDamage;

		private float vehicleDamage;

		private float resourceDamage;

		private float objectDamage;

		private ushort explosion2;

		private bool isBroken;

		private bool isExplosive;

		private float lastActive;
	}
}
