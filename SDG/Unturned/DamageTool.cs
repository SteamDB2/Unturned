using System;
using System.Collections.Generic;
using SDG.Framework.Utilities;
using SDG.Framework.Water;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	public class DamageTool
	{
		public static ELimb getLimb(Transform limb)
		{
			if (limb.CompareTag("Player") || limb.CompareTag("Enemy") || limb.CompareTag("Zombie") || limb.CompareTag("Animal"))
			{
				string name = limb.name;
				switch (name)
				{
				case "Left_Foot":
					return ELimb.LEFT_FOOT;
				case "Left_Leg":
					return ELimb.LEFT_LEG;
				case "Right_Foot":
					return ELimb.RIGHT_FOOT;
				case "Right_Leg":
					return ELimb.RIGHT_LEG;
				case "Left_Hand":
					return ELimb.LEFT_HAND;
				case "Left_Arm":
					return ELimb.LEFT_ARM;
				case "Right_Hand":
					return ELimb.RIGHT_HAND;
				case "Right_Arm":
					return ELimb.RIGHT_ARM;
				case "Left_Back":
					return ELimb.LEFT_BACK;
				case "Right_Back":
					return ELimb.RIGHT_BACK;
				case "Left_Front":
					return ELimb.LEFT_FRONT;
				case "Right_Front":
					return ELimb.RIGHT_FRONT;
				case "Spine":
					return ELimb.SPINE;
				case "Skull":
					return ELimb.SKULL;
				}
			}
			return ELimb.SPINE;
		}

		public static Player getPlayer(Transform limb)
		{
			Player player = null;
			if (limb.CompareTag("Player"))
			{
				player = limb.GetComponent<Player>();
			}
			else
			{
				string name = limb.name;
				switch (name)
				{
				case "Left_Foot":
					player = limb.parent.parent.parent.parent.parent.GetComponent<Player>();
					break;
				case "Left_Leg":
					player = limb.parent.parent.parent.parent.GetComponent<Player>();
					break;
				case "Right_Foot":
					player = limb.parent.parent.parent.parent.parent.GetComponent<Player>();
					break;
				case "Right_Leg":
					player = limb.parent.parent.parent.parent.GetComponent<Player>();
					break;
				case "Left_Hand":
					player = limb.parent.parent.parent.parent.parent.parent.GetComponent<Player>();
					break;
				case "Left_Arm":
					player = limb.parent.parent.parent.parent.parent.GetComponent<Player>();
					break;
				case "Right_Hand":
					player = limb.parent.parent.parent.parent.parent.parent.GetComponent<Player>();
					break;
				case "Right_Arm":
					player = limb.parent.parent.parent.parent.parent.GetComponent<Player>();
					break;
				case "Spine":
					player = limb.parent.parent.parent.GetComponent<Player>();
					break;
				case "Skull":
					player = limb.parent.parent.parent.parent.GetComponent<Player>();
					break;
				}
			}
			if (player != null && player.life.isDead)
			{
				player = null;
			}
			return player;
		}

		public static Zombie getZombie(Transform limb)
		{
			Zombie zombie = null;
			if (limb.CompareTag("Agent"))
			{
				zombie = limb.GetComponent<Zombie>();
			}
			else
			{
				string name = limb.name;
				switch (name)
				{
				case "Left_Foot":
					zombie = limb.parent.parent.parent.parent.parent.GetComponent<Zombie>();
					break;
				case "Left_Leg":
					zombie = limb.parent.parent.parent.parent.GetComponent<Zombie>();
					break;
				case "Right_Foot":
					zombie = limb.parent.parent.parent.parent.parent.GetComponent<Zombie>();
					break;
				case "Right_Leg":
					zombie = limb.parent.parent.parent.parent.GetComponent<Zombie>();
					break;
				case "Left_Hand":
					zombie = limb.parent.parent.parent.parent.parent.parent.GetComponent<Zombie>();
					break;
				case "Left_Arm":
					zombie = limb.parent.parent.parent.parent.parent.GetComponent<Zombie>();
					break;
				case "Right_Hand":
					zombie = limb.parent.parent.parent.parent.parent.parent.GetComponent<Zombie>();
					break;
				case "Right_Arm":
					zombie = limb.parent.parent.parent.parent.parent.GetComponent<Zombie>();
					break;
				case "Spine":
					zombie = limb.parent.parent.parent.GetComponent<Zombie>();
					break;
				case "Skull":
					zombie = limb.parent.parent.parent.parent.GetComponent<Zombie>();
					break;
				}
			}
			if (zombie != null && zombie.isDead)
			{
				zombie = null;
			}
			return zombie;
		}

		public static Animal getAnimal(Transform limb)
		{
			Animal animal = null;
			if (limb.CompareTag("Agent"))
			{
				animal = limb.GetComponent<Animal>();
			}
			else
			{
				string name = limb.name;
				if (name != null)
				{
					if (!(name == "Left_Back"))
					{
						if (!(name == "Right_Back"))
						{
							if (!(name == "Left_Front"))
							{
								if (!(name == "Right_Front"))
								{
									if (!(name == "Spine"))
									{
										if (name == "Skull")
										{
											animal = limb.parent.parent.parent.parent.GetComponent<Animal>();
										}
									}
									else
									{
										animal = limb.parent.parent.parent.GetComponent<Animal>();
									}
								}
								else
								{
									animal = limb.parent.parent.parent.parent.GetComponent<Animal>();
								}
							}
							else
							{
								animal = limb.parent.parent.parent.parent.GetComponent<Animal>();
							}
						}
						else
						{
							animal = limb.parent.parent.parent.GetComponent<Animal>();
						}
					}
					else
					{
						animal = limb.parent.parent.parent.GetComponent<Animal>();
					}
				}
			}
			if (animal != null && animal.isDead)
			{
				animal = null;
			}
			return animal;
		}

		public static InteractableVehicle getVehicle(Transform model)
		{
			return model.GetComponent<InteractableVehicle>();
		}

		public static void damage(Player player, EDeathCause cause, ELimb limb, CSteamID killer, Vector3 direction, float damage, float times, out EPlayerKill kill)
		{
			if (player == null)
			{
				kill = EPlayerKill.NONE;
				return;
			}
			times *= Provider.modeConfigData.Players.Armor_Multiplier;
			byte b = (byte)(damage * times);
			player.life.askDamage(b, direction * (float)b, cause, limb, killer, out kill);
		}

		public static void damage(Player player, EDeathCause cause, ELimb limb, CSteamID killer, Vector3 direction, PlayerDamageMultiplier multiplier, float times, bool armor, out EPlayerKill kill)
		{
			if (player == null)
			{
				kill = EPlayerKill.NONE;
				return;
			}
			if (armor)
			{
				times *= multiplier.armor(limb, player);
			}
			DamageTool.damage(player, cause, limb, killer, direction, multiplier.multiply(limb), times, out kill);
		}

		public static void damage(Zombie zombie, Vector3 direction, float damage, float times, out EPlayerKill kill, out uint xp)
		{
			if (zombie == null)
			{
				kill = EPlayerKill.NONE;
				xp = 0u;
				return;
			}
			byte b = (byte)(damage * times);
			zombie.askDamage((ushort)b, direction * (float)b, out kill, out xp, true, true);
		}

		public static void damage(Zombie zombie, ELimb limb, Vector3 direction, ZombieDamageMultiplier multiplier, float times, bool armor, out EPlayerKill kill, out uint xp)
		{
			if (zombie == null)
			{
				kill = EPlayerKill.NONE;
				xp = 0u;
				return;
			}
			if (armor)
			{
				times *= multiplier.armor(limb, zombie);
				if ((double)Vector3.Dot(zombie.transform.forward, direction) > 0.5)
				{
					times *= 1.25f;
				}
			}
			DamageTool.damage(zombie, direction, multiplier.multiply(limb), times, out kill, out xp);
		}

		public static void damage(Animal animal, Vector3 direction, float damage, float times, out EPlayerKill kill, out uint xp)
		{
			if (animal == null)
			{
				kill = EPlayerKill.NONE;
				xp = 0u;
				return;
			}
			byte b = (byte)(damage * times);
			animal.askDamage(b, direction * (float)b, out kill, out xp);
		}

		public static void damage(Animal animal, ELimb limb, Vector3 direction, AnimalDamageMultiplier multiplier, float times, out EPlayerKill kill, out uint xp)
		{
			if (animal == null)
			{
				kill = EPlayerKill.NONE;
				xp = 0u;
				return;
			}
			DamageTool.damage(animal, direction, multiplier.multiply(limb), times, out kill, out xp);
		}

		public static void damage(InteractableVehicle vehicle, bool damageTires, Vector3 position, bool isRepairing, float vehicleDamage, float times, bool canRepair, out EPlayerKill kill)
		{
			kill = EPlayerKill.NONE;
			if (vehicle == null)
			{
				return;
			}
			if (isRepairing)
			{
				if (!vehicle.isExploded && !vehicle.isRepaired)
				{
					VehicleManager.repair(vehicle, vehicleDamage, times);
				}
			}
			else
			{
				if (!vehicle.isDead)
				{
					VehicleManager.damage(vehicle, vehicleDamage, times, canRepair);
				}
				if (damageTires && !vehicle.isExploded)
				{
					int hitTireIndex = vehicle.getHitTireIndex(position);
					if (hitTireIndex != -1)
					{
						vehicle.askDamageTire(hitTireIndex);
					}
				}
			}
		}

		public static void damage(Transform barricade, bool isRepairing, float barricadeDamage, float times, out EPlayerKill kill)
		{
			kill = EPlayerKill.NONE;
			if (barricade == null)
			{
				return;
			}
			if (isRepairing)
			{
				BarricadeManager.repair(barricade, barricadeDamage, times);
			}
			else
			{
				BarricadeManager.damage(barricade, barricadeDamage, times, true);
			}
		}

		public static void damage(Transform structure, bool isRepairing, Vector3 direction, float structureDamage, float times, out EPlayerKill kill)
		{
			kill = EPlayerKill.NONE;
			if (structure == null)
			{
				return;
			}
			if (isRepairing)
			{
				StructureManager.repair(structure, structureDamage, times);
			}
			else
			{
				StructureManager.damage(structure, direction, structureDamage, times, true);
			}
		}

		public static void damage(Transform resource, Vector3 direction, float resourceDamage, float times, float drops, out EPlayerKill kill, out uint xp)
		{
			if (resource == null)
			{
				kill = EPlayerKill.NONE;
				xp = 0u;
				return;
			}
			ResourceManager.damage(resource, direction, resourceDamage, times, drops, out kill, out xp);
		}

		public static void damage(Transform obj, Vector3 direction, byte section, float objectDamage, float times, out EPlayerKill kill, out uint xp)
		{
			if (obj == null)
			{
				kill = EPlayerKill.NONE;
				xp = 0u;
				return;
			}
			ObjectManager.damage(obj, direction, section, objectDamage, times, out kill, out xp);
		}

		public static void explode(Vector3 point, float damageRadius, EDeathCause cause, CSteamID killer, float playerDamage, float zombieDamage, float animalDamage, float barricadeDamage, float structureDamage, float vehicleDamage, float resourceDamage, float objectDamage, EExplosionDamageType damageType = EExplosionDamageType.CONVENTIONAL, float alertRadius = 32f, bool playImpactEffect = true)
		{
			DamageTool.explosionRangeComparator.point = point;
			float num = damageRadius * damageRadius;
			DamageTool.regionsInRadius.Clear();
			Regions.getRegionsInRadius(point, damageRadius, DamageTool.regionsInRadius);
			if (structureDamage > 0.5f)
			{
				DamageTool.structuresInRadius.Clear();
				StructureManager.getStructuresInRadius(point, num, DamageTool.regionsInRadius, DamageTool.structuresInRadius);
				DamageTool.structuresInRadius.Sort(DamageTool.explosionRangeComparator);
				for (int i = 0; i < DamageTool.structuresInRadius.Count; i++)
				{
					Transform transform = DamageTool.structuresInRadius[i];
					if (!(transform == null))
					{
						ushort id;
						if (ushort.TryParse(transform.name, out id))
						{
							ItemStructureAsset itemStructureAsset = (ItemStructureAsset)Assets.find(EAssetType.ITEM, id);
							if (itemStructureAsset != null && !itemStructureAsset.proofExplosion)
							{
								Vector3 vector = transform.transform.position - point;
								float magnitude = vector.magnitude;
								Vector3 vector2 = vector / magnitude;
								if (magnitude > 0.5f)
								{
									RaycastHit raycastHit;
									PhysicsUtility.raycast(new Ray(point, vector2), out raycastHit, magnitude - 0.5f, RayMasks.BLOCK_EXPLOSION, 0);
									if (raycastHit.transform != null && raycastHit.transform != transform.transform)
									{
										goto IL_154;
									}
								}
								StructureManager.damage(transform, vector.normalized, structureDamage, 1f - magnitude / damageRadius, true);
							}
						}
					}
					IL_154:;
				}
			}
			if (resourceDamage > 0.5f)
			{
				DamageTool.resourcesInRadius.Clear();
				ResourceManager.getResourcesInRadius(point, num, DamageTool.regionsInRadius, DamageTool.resourcesInRadius);
				DamageTool.resourcesInRadius.Sort(DamageTool.explosionRangeComparator);
				for (int j = 0; j < DamageTool.resourcesInRadius.Count; j++)
				{
					Transform transform2 = DamageTool.resourcesInRadius[j];
					if (!(transform2 == null))
					{
						Vector3 vector3 = transform2.transform.position - point;
						float magnitude2 = vector3.magnitude;
						Vector3 vector4 = vector3 / magnitude2;
						if (magnitude2 > 0.5f)
						{
							RaycastHit raycastHit;
							PhysicsUtility.raycast(new Ray(point, vector4), out raycastHit, magnitude2 - 0.5f, RayMasks.BLOCK_EXPLOSION, 0);
							if (raycastHit.transform != null && raycastHit.transform != transform2.transform)
							{
								goto IL_26D;
							}
						}
						EPlayerKill eplayerKill;
						uint num2;
						ResourceManager.damage(transform2, vector3.normalized, resourceDamage, 1f - magnitude2 / damageRadius, 1f, out eplayerKill, out num2);
					}
					IL_26D:;
				}
			}
			if (objectDamage > 0.5f)
			{
				DamageTool.objectsInRadius.Clear();
				ObjectManager.getObjectsInRadius(point, num, DamageTool.regionsInRadius, DamageTool.objectsInRadius);
				DamageTool.objectsInRadius.Sort(DamageTool.explosionRangeComparator);
				for (int k = 0; k < DamageTool.objectsInRadius.Count; k++)
				{
					Transform transform3 = DamageTool.objectsInRadius[k];
					if (!(transform3 == null))
					{
						InteractableObjectRubble component = transform3.GetComponent<InteractableObjectRubble>();
						if (!(component == null))
						{
							if (!component.asset.rubbleProofExplosion)
							{
								for (byte b = 0; b < component.getSectionCount(); b += 1)
								{
									Transform section = component.getSection(b);
									Vector3 vector5 = section.position - point;
									if (vector5.sqrMagnitude < num)
									{
										float magnitude3 = vector5.magnitude;
										Vector3 vector6 = vector5 / magnitude3;
										if (magnitude3 > 0.5f)
										{
											RaycastHit raycastHit;
											PhysicsUtility.raycast(new Ray(point, vector6), out raycastHit, magnitude3 - 0.5f, RayMasks.BLOCK_EXPLOSION, 0);
											if (raycastHit.transform != null && raycastHit.transform != transform3.transform)
											{
												goto IL_3CA;
											}
										}
										EPlayerKill eplayerKill;
										uint num2;
										ObjectManager.damage(transform3, vector5.normalized, b, objectDamage, 1f - magnitude3 / damageRadius, out eplayerKill, out num2);
									}
									IL_3CA:;
								}
							}
						}
					}
				}
			}
			if (barricadeDamage > 0.5f)
			{
				DamageTool.barricadesInRadius.Clear();
				BarricadeManager.getBarricadesInRadius(point, num, DamageTool.regionsInRadius, DamageTool.barricadesInRadius);
				BarricadeManager.getBarricadesInRadius(point, num, DamageTool.barricadesInRadius);
				DamageTool.barricadesInRadius.Sort(DamageTool.explosionRangeComparator);
				for (int l = 0; l < DamageTool.barricadesInRadius.Count; l++)
				{
					Transform transform4 = DamageTool.barricadesInRadius[l];
					if (!(transform4 == null))
					{
						Vector3 vector7 = transform4.transform.position - point;
						float magnitude4 = vector7.magnitude;
						Vector3 vector8 = vector7 / magnitude4;
						if (magnitude4 > 0.5f)
						{
							RaycastHit raycastHit;
							PhysicsUtility.raycast(new Ray(point, vector8), out raycastHit, magnitude4 - 0.5f, RayMasks.BLOCK_EXPLOSION, 0);
							if (raycastHit.transform != null && raycastHit.transform != transform4.transform)
							{
								goto IL_52A;
							}
						}
						ushort id2;
						if (ushort.TryParse(transform4.name, out id2))
						{
							ItemBarricadeAsset itemBarricadeAsset = (ItemBarricadeAsset)Assets.find(EAssetType.ITEM, id2);
							if (itemBarricadeAsset != null && !itemBarricadeAsset.proofExplosion)
							{
								BarricadeManager.damage(transform4, barricadeDamage, 1f - magnitude4 / damageRadius, true);
							}
						}
					}
					IL_52A:;
				}
			}
			if ((Provider.isPvP || damageType == EExplosionDamageType.ZOMBIE_ACID || damageType == EExplosionDamageType.ZOMBIE_FIRE || damageType == EExplosionDamageType.ZOMBIE_ELECTRIC) && playerDamage > 0.5f)
			{
				DamageTool.playersInRadius.Clear();
				PlayerTool.getPlayersInRadius(point, num, DamageTool.playersInRadius);
				for (int m = 0; m < DamageTool.playersInRadius.Count; m++)
				{
					Player player = DamageTool.playersInRadius[m];
					if (!(player == null) && !player.life.isDead)
					{
						if (damageType != EExplosionDamageType.ZOMBIE_FIRE || player.clothing.shirtAsset == null || !player.clothing.shirtAsset.proofFire || player.clothing.pantsAsset == null || !player.clothing.pantsAsset.proofFire)
						{
							Vector3 vector9 = player.transform.position - point;
							float magnitude5 = vector9.magnitude;
							Vector3 vector10 = vector9 / magnitude5;
							if (magnitude5 > 0.5f)
							{
								RaycastHit raycastHit;
								PhysicsUtility.raycast(new Ray(point, vector10), out raycastHit, magnitude5 - 0.5f, RayMasks.BLOCK_EXPLOSION, 0);
								if (raycastHit.transform != null && raycastHit.transform != player.transform)
								{
									goto IL_760;
								}
							}
							if (playImpactEffect)
							{
								EffectManager.sendEffect(5, EffectManager.SMALL, player.transform.position + Vector3.up, -vector10);
								EffectManager.sendEffect(5, EffectManager.SMALL, player.transform.position + Vector3.up, Vector3.up);
							}
							float num3 = 1f - Mathf.Pow(magnitude5 / damageRadius, 2f);
							if (player.movement.getVehicle() != null && player.movement.getVehicle().asset != null)
							{
								num3 *= player.movement.getVehicle().asset.passengerExplosionArmor;
							}
							EPlayerKill eplayerKill;
							DamageTool.damage(player, cause, ELimb.SPINE, killer, vector10, playerDamage, num3, out eplayerKill);
						}
					}
					IL_760:;
				}
			}
			if (damageType == EExplosionDamageType.ZOMBIE_FIRE || zombieDamage > 0.5f)
			{
				DamageTool.zombiesInRadius.Clear();
				ZombieManager.getZombiesInRadius(point, num, DamageTool.zombiesInRadius);
				for (int n = 0; n < DamageTool.zombiesInRadius.Count; n++)
				{
					Zombie zombie = DamageTool.zombiesInRadius[n];
					if (!(zombie == null) && !zombie.isDead)
					{
						if (damageType == EExplosionDamageType.ZOMBIE_FIRE)
						{
							if (zombie.speciality == EZombieSpeciality.NORMAL)
							{
								ZombieManager.sendZombieSpeciality(zombie, EZombieSpeciality.BURNER);
							}
						}
						else
						{
							Vector3 vector11 = zombie.transform.position - point;
							float magnitude6 = vector11.magnitude;
							Vector3 vector12 = vector11 / magnitude6;
							if (magnitude6 > 0.5f)
							{
								RaycastHit raycastHit;
								PhysicsUtility.raycast(new Ray(point, vector12), out raycastHit, magnitude6 - 0.5f, RayMasks.BLOCK_EXPLOSION, 0);
								if (raycastHit.transform != null && raycastHit.transform != zombie.transform)
								{
									goto IL_90E;
								}
							}
							if (playImpactEffect)
							{
								EffectManager.sendEffect((!zombie.isRadioactive) ? 5 : 95, EffectManager.SMALL, zombie.transform.position + Vector3.up, -vector12);
								EffectManager.sendEffect((!zombie.isRadioactive) ? 5 : 95, EffectManager.SMALL, zombie.transform.position + Vector3.up, Vector3.up);
							}
							EPlayerKill eplayerKill;
							uint num2;
							DamageTool.damage(zombie, vector12, zombieDamage, 1f - magnitude6 / damageRadius, out eplayerKill, out num2);
						}
					}
					IL_90E:;
				}
			}
			if (animalDamage > 0.5f)
			{
				DamageTool.animalsInRadius.Clear();
				AnimalManager.getAnimalsInRadius(point, num, DamageTool.animalsInRadius);
				for (int num4 = 0; num4 < DamageTool.animalsInRadius.Count; num4++)
				{
					Animal animal = DamageTool.animalsInRadius[num4];
					if (!(animal == null) && !animal.isDead)
					{
						Vector3 vector13 = animal.transform.position - point;
						float magnitude7 = vector13.magnitude;
						Vector3 vector14 = vector13 / magnitude7;
						if (magnitude7 > 0.5f)
						{
							RaycastHit raycastHit;
							PhysicsUtility.raycast(new Ray(point, vector14), out raycastHit, magnitude7 - 0.5f, RayMasks.BLOCK_EXPLOSION, 0);
							if (raycastHit.transform != null && raycastHit.transform != animal.transform)
							{
								goto IL_A6A;
							}
						}
						if (playImpactEffect)
						{
							EffectManager.sendEffect(5, EffectManager.SMALL, animal.transform.position + Vector3.up, -vector14);
							EffectManager.sendEffect(5, EffectManager.SMALL, animal.transform.position + Vector3.up, Vector3.up);
						}
						EPlayerKill eplayerKill;
						uint num2;
						DamageTool.damage(animal, vector14, animalDamage, 1f - magnitude7 / damageRadius, out eplayerKill, out num2);
					}
					IL_A6A:;
				}
			}
			if (vehicleDamage > 0.5f)
			{
				DamageTool.vehiclesInRadius.Clear();
				VehicleManager.getVehiclesInRadius(point, num, DamageTool.vehiclesInRadius);
				for (int num5 = 0; num5 < DamageTool.vehiclesInRadius.Count; num5++)
				{
					InteractableVehicle interactableVehicle = DamageTool.vehiclesInRadius[num5];
					if (!(interactableVehicle == null) && !interactableVehicle.isDead)
					{
						Vector3 vector15 = interactableVehicle.transform.position - point;
						float magnitude8 = vector15.magnitude;
						Vector3 vector16 = vector15 / magnitude8;
						if (magnitude8 > 0.5f)
						{
							RaycastHit raycastHit;
							PhysicsUtility.raycast(new Ray(point, vector16), out raycastHit, magnitude8 - 0.5f, RayMasks.BLOCK_EXPLOSION, 0);
							if (raycastHit.transform != null && raycastHit.transform != interactableVehicle.transform)
							{
								goto IL_B6C;
							}
						}
						VehicleManager.damage(interactableVehicle, vehicleDamage, 1f - magnitude8 / damageRadius, false);
					}
					IL_B6C:;
				}
			}
			AlertTool.alert(point, alertRadius);
		}

		public static EPhysicsMaterial getMaterial(Vector3 point, Transform transform, Collider collider)
		{
			if (WaterUtility.isPointUnderwater(point))
			{
				return EPhysicsMaterial.WATER_STATIC;
			}
			if (transform.CompareTag("Ground"))
			{
				return PhysicsTool.checkMaterial(point);
			}
			return PhysicsTool.checkMaterial(collider);
		}

		public static void impact(Vector3 point, Vector3 normal, EPhysicsMaterial material, bool forceDynamic)
		{
			DamageTool.impact(point, normal, material, forceDynamic, CSteamID.Nil, point);
		}

		public static void impact(Vector3 point, Vector3 normal, EPhysicsMaterial material, bool forceDynamic, CSteamID spectatorID, Vector3 spectatorPoint)
		{
			if (material == EPhysicsMaterial.NONE)
			{
				return;
			}
			ushort id = 0;
			if (material == EPhysicsMaterial.CLOTH_DYNAMIC || material == EPhysicsMaterial.TILE_DYNAMIC || material == EPhysicsMaterial.CONCRETE_DYNAMIC)
			{
				id = 38;
			}
			else if (material == EPhysicsMaterial.CLOTH_STATIC || material == EPhysicsMaterial.TILE_STATIC || material == EPhysicsMaterial.CONCRETE_STATIC)
			{
				id = ((!forceDynamic) ? 13 : 38);
			}
			else if (material == EPhysicsMaterial.FLESH_DYNAMIC)
			{
				id = 5;
			}
			else if (material == EPhysicsMaterial.GRAVEL_DYNAMIC)
			{
				id = 44;
			}
			else if (material == EPhysicsMaterial.GRAVEL_STATIC)
			{
				id = ((!forceDynamic) ? 14 : 44);
			}
			else if (material == EPhysicsMaterial.METAL_DYNAMIC)
			{
				id = 18;
			}
			else if (material == EPhysicsMaterial.METAL_STATIC || material == EPhysicsMaterial.METAL_SLIP)
			{
				id = ((!forceDynamic) ? 12 : 18);
			}
			else if (material == EPhysicsMaterial.WOOD_DYNAMIC)
			{
				id = 17;
			}
			else if (material == EPhysicsMaterial.WOOD_STATIC)
			{
				id = ((!forceDynamic) ? 2 : 17);
			}
			else if (material == EPhysicsMaterial.FOLIAGE_STATIC || material == EPhysicsMaterial.FOLIAGE_DYNAMIC)
			{
				id = 15;
			}
			else if (material == EPhysicsMaterial.SNOW_STATIC || material == EPhysicsMaterial.ICE_STATIC)
			{
				id = 41;
			}
			else if (material == EPhysicsMaterial.WATER_STATIC)
			{
				id = 16;
			}
			else if (material == EPhysicsMaterial.ALIEN_DYNAMIC)
			{
				id = 95;
			}
			DamageTool.impact(point, normal, id, spectatorID, spectatorPoint);
		}

		public static void impact(Vector3 point, Vector3 normal, ushort id, CSteamID spectatorID, Vector3 spectatorPoint)
		{
			if (id == 0)
			{
				return;
			}
			point += normal * Random.Range(0.04f, 0.06f);
			EffectManager.sendEffect(id, EffectManager.SMALL, point, normal);
			if (spectatorID != CSteamID.Nil && (spectatorPoint - point).sqrMagnitude >= EffectManager.SMALL * EffectManager.SMALL)
			{
				EffectManager.sendEffect(id, spectatorID, point, normal);
			}
		}

		public static RaycastInfo raycast(Ray ray, float range, int mask)
		{
			RaycastHit hit;
			PhysicsUtility.raycast(ray, out hit, range, mask, 0);
			RaycastInfo raycastInfo = new RaycastInfo(hit);
			raycastInfo.direction = ray.direction;
			if (hit.transform != null)
			{
				if (hit.transform.CompareTag("Enemy"))
				{
					raycastInfo.player = DamageTool.getPlayer(raycastInfo.transform);
				}
				if (hit.transform.CompareTag("Zombie"))
				{
					raycastInfo.zombie = DamageTool.getZombie(raycastInfo.transform);
				}
				if (hit.transform.CompareTag("Animal"))
				{
					raycastInfo.animal = DamageTool.getAnimal(raycastInfo.transform);
				}
				raycastInfo.limb = DamageTool.getLimb(raycastInfo.transform);
				if (hit.transform.CompareTag("Vehicle"))
				{
					raycastInfo.vehicle = DamageTool.getVehicle(raycastInfo.transform);
				}
				if (raycastInfo.zombie != null && raycastInfo.zombie.isRadioactive)
				{
					raycastInfo.material = EPhysicsMaterial.ALIEN_DYNAMIC;
				}
				else
				{
					raycastInfo.material = DamageTool.getMaterial(hit.point, hit.transform, hit.collider);
				}
			}
			return raycastInfo;
		}

		private static List<RegionCoordinate> regionsInRadius = new List<RegionCoordinate>(4);

		private static List<Player> playersInRadius = new List<Player>();

		private static List<Zombie> zombiesInRadius = new List<Zombie>();

		private static List<Animal> animalsInRadius = new List<Animal>();

		private static List<Transform> barricadesInRadius = new List<Transform>();

		private static List<Transform> structuresInRadius = new List<Transform>();

		private static List<InteractableVehicle> vehiclesInRadius = new List<InteractableVehicle>();

		private static List<Transform> resourcesInRadius = new List<Transform>();

		private static List<Transform> objectsInRadius = new List<Transform>();

		private static ExplosionRangeComparator explosionRangeComparator = new ExplosionRangeComparator();
	}
}
