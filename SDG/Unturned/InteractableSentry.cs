using System;
using System.Collections.Generic;
using SDG.Framework.Utilities;
using UnityEngine;

namespace SDG.Unturned
{
	public class InteractableSentry : InteractableStorage
	{
		private void trace(Vector3 pos, Vector3 dir)
		{
			if (this.tracerEmitter == null)
			{
				return;
			}
			if (this.attachments.barrelModel != null && this.attachments.barrelAsset.isBraked && this.displayItem.state[16] > 0)
			{
				return;
			}
			this.tracerEmitter.transform.position = pos;
			this.tracerEmitter.transform.rotation = Quaternion.LookRotation(dir);
			this.tracerEmitter.Emit(1);
		}

		public void shoot()
		{
			this.lastAlert = Time.realtimeSinceStartup;
			if (!Dedicator.isDedicated)
			{
				if (this.sound != null)
				{
					if (this.attachments.barrelAsset != null && this.attachments.barrelAsset.isSilenced && this.displayItem.state[16] > 0)
					{
						this.sound.clip = this.attachments.barrelAsset.shoot;
						this.sound.volume = this.attachments.barrelAsset.volume;
					}
					else
					{
						this.sound.clip = ((ItemGunAsset)this.displayAsset).shoot;
						this.sound.volume = 1f;
					}
					this.sound.pitch = Random.Range(0.975f, 1.025f);
					this.sound.PlayOneShot(this.sound.clip);
				}
				if (((ItemGunAsset)this.displayAsset).action == EAction.Trigger && this.shellEmitter != null)
				{
					this.shellEmitter.Emit(1);
				}
				if ((this.attachments.barrelModel == null || !this.attachments.barrelAsset.isBraked || this.displayItem.state[16] == 0) && this.muzzleEmitter != null)
				{
					this.muzzleEmitter.Emit(1);
					this.muzzleEmitter.GetComponent<Light>().enabled = true;
				}
				if (this.aimTransform != null)
				{
					if (((ItemGunAsset)this.displayAsset).range < 32f)
					{
						this.trace(this.aimTransform.position + this.aimTransform.forward * 32f, this.aimTransform.forward);
					}
					else
					{
						this.trace(this.aimTransform.position + this.aimTransform.forward * Random.Range(32f, Mathf.Min(64f, ((ItemGunAsset)this.displayAsset).range)), this.aimTransform.forward);
					}
				}
			}
			this.lastShot = Time.realtimeSinceStartup;
			if (this.attachments.barrelAsset != null && this.attachments.barrelAsset.durability > 0)
			{
				if (this.attachments.barrelAsset.durability > this.displayItem.state[16])
				{
					this.displayItem.state[16] = 0;
				}
				else
				{
					byte[] state = this.displayItem.state;
					int num = 16;
					state[num] -= this.attachments.barrelAsset.durability;
				}
			}
		}

		public void alert(float newYaw, float newPitch)
		{
			this.targetYaw = newYaw;
			this.targetPitch = newPitch;
			this.lastAlert = Time.realtimeSinceStartup;
		}

		public override void updateState(Asset asset, byte[] state)
		{
			this.sentryMode = ((ItemSentryAsset)asset).sentryMode;
			this.yawTransform = base.transform.FindChild("Yaw");
			if (this.yawTransform != null)
			{
				this.pitchTransform = this.yawTransform.FindChild("Pitch");
				if (this.pitchTransform != null)
				{
					this.aimTransform = this.pitchTransform.FindChild("Aim");
					Transform transform = this.aimTransform.FindChild("Spot");
					if (transform != null)
					{
						this.spotGameObject = transform.gameObject;
					}
				}
			}
			Transform transform2 = base.transform.FindChildRecursive("On");
			if (transform2 != null)
			{
				this.onGameObject = transform2.gameObject;
			}
			Transform transform3 = base.transform.FindChildRecursive("On_Model");
			if (transform3 != null)
			{
				this.onModelGameObject = transform3.gameObject;
				this.onMaterial = this.onModelGameObject.GetComponent<Renderer>().material;
			}
			Transform transform4 = base.transform.FindChildRecursive("Off");
			if (transform4 != null)
			{
				this.offGameObject = transform4.gameObject;
			}
			Transform transform5 = base.transform.FindChildRecursive("Off_Model");
			if (transform5 != null)
			{
				this.offModelGameObject = transform5.gameObject;
				this.offMaterial = this.offModelGameObject.GetComponent<Renderer>().material;
			}
			this.isAlert = false;
			this.lastAlert = 0f;
			this.targetYaw = base.transform.localRotation.eulerAngles.y;
			this.yaw = this.targetYaw;
			this.targetPitch = 0f;
			this.pitch = this.targetPitch;
			base.updateState(asset, state);
		}

		public override void refreshDisplay()
		{
			base.refreshDisplay();
			this.hasWeapon = false;
			this.attachments = null;
			this.sound = null;
			this.destroyEffects();
			if (this.spotGameObject != null)
			{
				this.spotGameObject.SetActive(false);
			}
			if (this.displayAsset == null || this.displayAsset.type != EItemType.GUN || ((ItemGunAsset)this.displayAsset).action == EAction.String || ((ItemGunAsset)this.displayAsset).action == EAction.Rocket)
			{
				return;
			}
			this.hasWeapon = true;
			this.attachments = this.displayModel.gameObject.GetComponent<Attachments>();
			this.interact = (this.displayItem.state[12] == 1);
			if (!Dedicator.isDedicated)
			{
				this.sound = this.displayModel.gameObject.AddComponent<AudioSource>();
				this.sound.clip = null;
				this.sound.spatialBlend = 1f;
				this.sound.rolloffMode = 1;
				this.sound.volume = 1f;
				this.sound.minDistance = 8f;
				this.sound.maxDistance = 256f;
				this.sound.playOnAwake = false;
			}
			if (this.attachments.ejectHook != null && ((ItemGunAsset)this.displayAsset).action != EAction.String && ((ItemGunAsset)this.displayAsset).action != EAction.Rocket)
			{
				EffectAsset effectAsset = null;
				if (((ItemGunAsset)this.displayAsset).action == EAction.Pump || ((ItemGunAsset)this.displayAsset).action == EAction.Break)
				{
					effectAsset = (EffectAsset)Assets.find(EAssetType.EFFECT, 33);
				}
				else if (((ItemGunAsset)this.displayAsset).action != EAction.Rail)
				{
					effectAsset = (EffectAsset)Assets.find(EAssetType.EFFECT, 1);
				}
				if (effectAsset != null)
				{
					Transform transform = EffectManager.Instantiate(effectAsset.effect).transform;
					transform.name = "Emitter";
					transform.parent = this.attachments.ejectHook;
					transform.localPosition = Vector3.zero;
					transform.localRotation = Quaternion.identity;
					this.shellEmitter = transform.GetComponent<ParticleSystem>();
				}
			}
			if (this.attachments.barrelHook != null)
			{
				EffectAsset effectAsset2 = (EffectAsset)Assets.find(EAssetType.EFFECT, ((ItemGunAsset)this.displayAsset).muzzle);
				if (effectAsset2 != null)
				{
					Transform transform2 = EffectManager.Instantiate(effectAsset2.effect).transform;
					transform2.name = "Emitter";
					transform2.parent = this.attachments.barrelHook;
					transform2.localPosition = Vector3.zero;
					transform2.localRotation = Quaternion.identity;
					this.muzzleEmitter = transform2.GetComponent<ParticleSystem>();
				}
			}
			if (this.muzzleEmitter != null)
			{
				if (this.attachments.barrelModel != null)
				{
					this.muzzleEmitter.transform.localPosition = Vector3.up * 0.25f;
				}
				else
				{
					this.muzzleEmitter.transform.localPosition = Vector3.zero;
				}
			}
			if (this.attachments.magazineAsset != null)
			{
				EffectAsset effectAsset3 = (EffectAsset)Assets.find(EAssetType.EFFECT, this.attachments.magazineAsset.tracer);
				if (effectAsset3 != null)
				{
					Transform transform3 = EffectManager.Instantiate(effectAsset3.effect).transform;
					transform3.name = "Tracer";
					transform3.parent = Level.effects;
					transform3.localPosition = Vector3.zero;
					transform3.localRotation = Quaternion.identity;
					this.tracerEmitter = transform3.GetComponent<ParticleSystem>();
				}
			}
			if (!Dedicator.isDedicated)
			{
				if (this.attachments.tacticalAsset != null && (this.attachments.tacticalAsset.isLight || this.attachments.tacticalAsset.isLaser) && this.attachments.lightHook != null)
				{
					this.attachments.lightHook.gameObject.SetActive(this.interact);
				}
				if (this.spotGameObject != null)
				{
					this.spotGameObject.SetActive(this.attachments.tacticalAsset != null && this.attachments.tacticalAsset.isLight && this.interact);
				}
			}
			this.fireTime = (float)(((ItemGunAsset)this.displayAsset).firerate - ((this.attachments.tacticalAsset == null) ? 0 : this.attachments.tacticalAsset.firerate));
			this.fireTime /= 50f;
			this.fireTime *= 3.33f;
		}

		private void Update()
		{
			if (Provider.isServer && this.power != null && this.power.isWired)
			{
				Vector3 vector = base.transform.position + new Vector3(0f, 0.65f, 0f);
				if (Time.realtimeSinceStartup - this.lastScan > 0.1f)
				{
					this.lastScan = Time.realtimeSinceStartup;
					float num = 48f;
					if (this.hasWeapon)
					{
						num = Mathf.Min(num, ((ItemWeaponAsset)this.displayAsset).range);
					}
					float num2 = num * num;
					float num3 = num2;
					Player player = null;
					Zombie zombie = null;
					if (Provider.isPvP)
					{
						InteractableSentry.playersInRadius.Clear();
						PlayerTool.getPlayersInRadius(vector, num2, InteractableSentry.playersInRadius);
						for (int i = 0; i < InteractableSentry.playersInRadius.Count; i++)
						{
							Player player2 = InteractableSentry.playersInRadius[i];
							if (!(player2.channel.owner.playerID.steamID == base.owner) && !player2.quests.isMemberOfGroup(base.group))
							{
								if (!player2.life.isDead && player2.animator.gesture != EPlayerGesture.ARREST_START)
								{
									if ((!player2.movement.isSafe || !player2.movement.isSafeInfo.noWeapons) && player2.movement.canAddSimulationResultsToUpdates)
									{
										if (!(player != null) || player2.animator.gesture != EPlayerGesture.SURRENDER_START)
										{
											if (this.sentryMode != ESentryMode.FRIENDLY || Time.realtimeSinceStartup - player2.equipment.lastPunching <= 2f || (player2.equipment.isSelected && player2.equipment.asset != null && player2.equipment.asset.isDangerous))
											{
												float sqrMagnitude = (player2.look.aim.position - vector).sqrMagnitude;
												if (sqrMagnitude <= num3)
												{
													Vector3 vector2 = player2.look.aim.position - vector;
													float magnitude = vector2.magnitude;
													Vector3 vector3 = vector2 / magnitude;
													if (!(player2 != this.targetPlayer) || Vector3.Dot(vector3, this.aimTransform.forward) >= 0.5f)
													{
														if (magnitude > 0.025f)
														{
															RaycastHit raycastHit;
															PhysicsUtility.raycast(new Ray(vector, vector3), out raycastHit, magnitude - 0.025f, RayMasks.BLOCK_SENTRY, 0);
															if (raycastHit.transform != null && raycastHit.transform != base.transform)
															{
																goto IL_35F;
															}
															PhysicsUtility.raycast(new Ray(vector + vector3 * (magnitude - 0.025f), -vector3), out raycastHit, magnitude - 0.025f, RayMasks.DAMAGE_SERVER, 0);
															if (raycastHit.transform != null && raycastHit.transform != base.transform)
															{
																goto IL_35F;
															}
														}
														num3 = sqrMagnitude;
														player = player2;
													}
												}
											}
										}
									}
								}
							}
							IL_35F:;
						}
					}
					InteractableSentry.zombiesInRadius.Clear();
					ZombieManager.getZombiesInRadius(vector, num2, InteractableSentry.zombiesInRadius);
					for (int j = 0; j < InteractableSentry.zombiesInRadius.Count; j++)
					{
						Zombie zombie2 = InteractableSentry.zombiesInRadius[j];
						if (!zombie2.isDead && zombie2.isHunting)
						{
							Vector3 vector4 = zombie2.transform.position;
							switch (zombie2.speciality)
							{
							case EZombieSpeciality.NORMAL:
								vector4 += new Vector3(0f, 1.75f, 0f);
								break;
							case EZombieSpeciality.MEGA:
								vector4 += new Vector3(0f, 2.625f, 0f);
								break;
							case EZombieSpeciality.CRAWLER:
								vector4 += new Vector3(0f, 0.25f, 0f);
								break;
							case EZombieSpeciality.SPRINTER:
								vector4 += new Vector3(0f, 1f, 0f);
								break;
							}
							float sqrMagnitude2 = (vector4 - vector).sqrMagnitude;
							if (sqrMagnitude2 <= num3)
							{
								Vector3 vector5 = vector4 - vector;
								float magnitude2 = vector5.magnitude;
								Vector3 vector6 = vector5 / magnitude2;
								if (!(zombie2 != this.targetZombie) || Vector3.Dot(vector6, this.aimTransform.forward) >= 0.5f)
								{
									if (magnitude2 > 0.025f)
									{
										RaycastHit raycastHit2;
										PhysicsUtility.raycast(new Ray(vector, vector6), out raycastHit2, magnitude2 - 0.025f, RayMasks.BLOCK_SENTRY, 0);
										if (raycastHit2.transform != null && raycastHit2.transform != base.transform)
										{
											goto IL_5B4;
										}
										PhysicsUtility.raycast(new Ray(vector + vector6 * (magnitude2 - 0.025f), -vector6), out raycastHit2, magnitude2 - 0.025f, RayMasks.DAMAGE_SERVER, 0);
										if (raycastHit2.transform != null && raycastHit2.transform != base.transform)
										{
											goto IL_5B4;
										}
									}
									num3 = sqrMagnitude2;
									player = null;
									zombie = zombie2;
								}
							}
						}
						IL_5B4:;
					}
					if (player != this.targetPlayer || zombie != this.targetZombie)
					{
						this.targetPlayer = player;
						this.targetZombie = zombie;
						this.lastFire = Time.realtimeSinceStartup + 0.1f;
					}
				}
				if (this.targetPlayer != null)
				{
					ESentryMode esentryMode = this.sentryMode;
					if (esentryMode != ESentryMode.FRIENDLY && esentryMode != ESentryMode.NEUTRAL)
					{
						if (esentryMode == ESentryMode.HOSTILE)
						{
							this.isFiring = true;
						}
					}
					else
					{
						this.isFiring = (this.targetPlayer.animator.gesture != EPlayerGesture.SURRENDER_START);
					}
					this.isAiming = true;
				}
				else if (this.targetZombie != null)
				{
					this.isFiring = true;
					this.isAiming = true;
				}
				else
				{
					this.isFiring = false;
					this.isAiming = false;
				}
				if (this.isAiming && Time.realtimeSinceStartup - this.lastAim > Provider.UPDATE_TIME)
				{
					this.lastAim = Time.realtimeSinceStartup;
					Transform transform = null;
					Vector3 vector7 = Vector3.zero;
					if (this.targetPlayer != null)
					{
						transform = this.targetPlayer.transform;
						vector7 = this.targetPlayer.look.aim.position;
					}
					else if (this.targetZombie != null)
					{
						transform = this.targetZombie.transform;
						vector7 = this.targetZombie.transform.position;
						switch (this.targetZombie.speciality)
						{
						case EZombieSpeciality.NORMAL:
							vector7 += new Vector3(0f, 1.75f, 0f);
							break;
						case EZombieSpeciality.MEGA:
							vector7 += new Vector3(0f, 2.625f, 0f);
							break;
						case EZombieSpeciality.CRAWLER:
							vector7 += new Vector3(0f, 0.25f, 0f);
							break;
						case EZombieSpeciality.SPRINTER:
							vector7 += new Vector3(0f, 1f, 0f);
							break;
						}
					}
					if (transform != null)
					{
						float num4 = Mathf.Atan2(vector7.x - vector.x, vector7.z - vector.z) * 57.29578f;
						float num5 = Mathf.Sin((vector7.y - vector.y) / (vector7 - vector).magnitude) * 57.29578f;
						BarricadeManager.sendAlertSentry(base.transform, num4, num5);
					}
				}
				if (this.isFiring && this.hasWeapon && this.displayItem.state[10] > 0 && !this.isOpen && Time.realtimeSinceStartup - this.lastFire > this.fireTime)
				{
					this.lastFire += this.fireTime;
					if (Time.realtimeSinceStartup - this.lastFire > this.fireTime)
					{
						this.lastFire = Time.realtimeSinceStartup;
					}
					float num6 = (float)this.displayItem.quality / 100f;
					if (this.attachments.magazineAsset == null)
					{
						return;
					}
					byte[] state = this.displayItem.state;
					int num7 = 10;
					state[num7] -= 1;
					if (this.attachments.barrelAsset == null || !this.attachments.barrelAsset.isSilenced || this.displayItem.state[16] == 0)
					{
						AlertTool.alert(base.transform.position, 48f);
					}
					if (Provider.modeConfigData.Items.Has_Durability && this.displayItem.quality > 0 && Random.value < ((ItemWeaponAsset)this.displayAsset).durability)
					{
						if (this.displayItem.quality > ((ItemWeaponAsset)this.displayAsset).wear)
						{
							Item displayItem = this.displayItem;
							displayItem.quality -= ((ItemWeaponAsset)this.displayAsset).wear;
						}
						else
						{
							this.displayItem.quality = 0;
						}
					}
					float num8 = ((ItemGunAsset)this.displayAsset).spreadAim * ((num6 >= 0.5f) ? 1f : (1f + (1f - num6 * 2f)));
					if (this.attachments.tacticalAsset != null && this.interact)
					{
						num8 *= this.attachments.tacticalAsset.spread;
					}
					if (this.attachments.gripAsset != null)
					{
						num8 *= this.attachments.gripAsset.spread;
					}
					if (this.attachments.barrelAsset != null)
					{
						num8 *= this.attachments.barrelAsset.spread;
					}
					if (this.attachments.magazineAsset != null)
					{
						num8 *= this.attachments.magazineAsset.spread;
					}
					if (((ItemGunAsset)this.displayAsset).projectile == null)
					{
						BarricadeManager.sendShootSentry(base.transform);
						byte pellets = this.attachments.magazineAsset.pellets;
						for (byte b = 0; b < pellets; b += 1)
						{
							EPlayerKill eplayerKill = EPlayerKill.NONE;
							uint num9 = 0u;
							float num10 = 1f;
							num10 *= ((num6 >= 0.5f) ? 1f : (0.5f + num6));
							Transform transform2;
							float magnitude3;
							if (this.targetPlayer != null)
							{
								transform2 = this.targetPlayer.transform;
								magnitude3 = (transform2.position - base.transform.position).magnitude;
							}
							else
							{
								transform2 = this.targetZombie.transform;
								magnitude3 = (transform2.position - base.transform.position).magnitude;
							}
							float num11 = magnitude3 / ((ItemWeaponAsset)this.displayAsset).range;
							num11 = 1f - num11;
							num11 *= 1f - ((ItemGunAsset)this.displayAsset).spreadHip;
							num11 *= 0.75f;
							if (transform2 == null || Random.value > num11)
							{
								Vector3 vector8 = this.aimTransform.forward;
								vector8 += this.aimTransform.right * Random.Range(-((ItemGunAsset)this.displayAsset).spreadHip, ((ItemGunAsset)this.displayAsset).spreadHip) * num8;
								vector8 += this.aimTransform.up * Random.Range(-((ItemGunAsset)this.displayAsset).spreadHip, ((ItemGunAsset)this.displayAsset).spreadHip) * num8;
								vector8.Normalize();
								Ray ray;
								ray..ctor(this.aimTransform.position, vector8);
								RaycastInfo raycastInfo = DamageTool.raycast(ray, ((ItemWeaponAsset)this.displayAsset).range, RayMasks.DAMAGE_SERVER);
								if (!(raycastInfo.transform == null))
								{
									DamageTool.impact(raycastInfo.point, raycastInfo.normal, raycastInfo.material, raycastInfo.vehicle != null || raycastInfo.transform.CompareTag("Barricade") || raycastInfo.transform.CompareTag("Structure") || raycastInfo.transform.CompareTag("Resource"));
									if (raycastInfo.vehicle != null)
									{
										DamageTool.damage(raycastInfo.vehicle, false, Vector3.zero, false, ((ItemGunAsset)this.displayAsset).vehicleDamage, num10, true, out eplayerKill);
									}
									else if (raycastInfo.transform != null)
									{
										if (raycastInfo.transform.CompareTag("Barricade"))
										{
											ushort id;
											if (ushort.TryParse(raycastInfo.transform.name, out id))
											{
												ItemBarricadeAsset itemBarricadeAsset = (ItemBarricadeAsset)Assets.find(EAssetType.ITEM, id);
												if (itemBarricadeAsset != null && (itemBarricadeAsset.isVulnerable || ((ItemWeaponAsset)this.displayAsset).isInvulnerable))
												{
													DamageTool.damage(raycastInfo.transform, false, ((ItemGunAsset)this.displayAsset).barricadeDamage, num10, out eplayerKill);
												}
											}
										}
										else if (raycastInfo.transform.CompareTag("Structure"))
										{
											ushort id2;
											if (ushort.TryParse(raycastInfo.transform.name, out id2))
											{
												ItemStructureAsset itemStructureAsset = (ItemStructureAsset)Assets.find(EAssetType.ITEM, id2);
												if (itemStructureAsset != null && (itemStructureAsset.isVulnerable || ((ItemWeaponAsset)this.displayAsset).isInvulnerable))
												{
													DamageTool.damage(raycastInfo.transform, false, raycastInfo.direction * Mathf.Ceil((float)this.attachments.magazineAsset.pellets / 2f), ((ItemGunAsset)this.displayAsset).structureDamage, num10, out eplayerKill);
												}
											}
										}
										else if (raycastInfo.transform.CompareTag("Resource"))
										{
											byte x;
											byte y;
											ushort index;
											if (ResourceManager.tryGetRegion(raycastInfo.transform, out x, out y, out index))
											{
												ResourceSpawnpoint resourceSpawnpoint = ResourceManager.getResourceSpawnpoint(x, y, index);
												if (resourceSpawnpoint != null && !resourceSpawnpoint.isDead && resourceSpawnpoint.asset.bladeID == ((ItemWeaponAsset)this.displayAsset).bladeID)
												{
													DamageTool.damage(raycastInfo.transform, raycastInfo.direction * Mathf.Ceil((float)this.attachments.magazineAsset.pellets / 2f), ((ItemGunAsset)this.displayAsset).resourceDamage, num10, 1f, out eplayerKill, out num9);
												}
											}
										}
										else if (raycastInfo.section < 255)
										{
											InteractableObjectRubble component = raycastInfo.transform.GetComponent<InteractableObjectRubble>();
											if (component != null && !component.isSectionDead(raycastInfo.section) && (component.asset.rubbleIsVulnerable || ((ItemWeaponAsset)this.displayAsset).isInvulnerable))
											{
												DamageTool.damage(raycastInfo.transform, raycastInfo.direction, raycastInfo.section, ((ItemGunAsset)this.displayAsset).objectDamage, num10, out eplayerKill, out num9);
											}
										}
									}
								}
							}
							else
							{
								Vector3 vector9 = Vector3.zero;
								if (this.targetPlayer != null)
								{
									vector9 = this.targetPlayer.look.aim.position;
								}
								else if (this.targetZombie != null)
								{
									vector9 = this.targetZombie.transform.position;
									switch (this.targetZombie.speciality)
									{
									case EZombieSpeciality.NORMAL:
										vector9 += new Vector3(0f, 1.75f, 0f);
										break;
									case EZombieSpeciality.MEGA:
										vector9 += new Vector3(0f, 2.625f, 0f);
										break;
									case EZombieSpeciality.CRAWLER:
										vector9 += new Vector3(0f, 0.25f, 0f);
										break;
									case EZombieSpeciality.SPRINTER:
										vector9 += new Vector3(0f, 1f, 0f);
										break;
									}
								}
								DamageTool.impact(vector9, -this.aimTransform.forward, EPhysicsMaterial.FLESH_DYNAMIC, true);
								if (this.targetPlayer != null)
								{
									DamageTool.damage(this.targetPlayer, EDeathCause.SENTRY, ELimb.SPINE, base.owner, this.aimTransform.forward * Mathf.Ceil((float)this.attachments.magazineAsset.pellets / 2f), ((ItemGunAsset)this.displayAsset).playerDamageMultiplier, num10, true, out eplayerKill);
								}
								else if (this.targetZombie != null)
								{
									DamageTool.damage(this.targetZombie, ELimb.SPINE, this.aimTransform.forward * Mathf.Ceil((float)this.attachments.magazineAsset.pellets / 2f), ((ItemGunAsset)this.displayAsset).zombieDamageMultiplier, num10, true, out eplayerKill, out num9);
								}
							}
						}
					}
					base.rebuildState();
				}
			}
			bool flag = Time.realtimeSinceStartup - this.lastAlert < 1f;
			if (flag != this.isAlert)
			{
				this.isAlert = flag;
				if (!Dedicator.isDedicated)
				{
					if (this.isAlert)
					{
						EffectManager.effect(92, base.transform.position, Vector3.up);
					}
					else
					{
						EffectManager.effect(93, base.transform.position, Vector3.up);
					}
				}
				if (!this.isAlert)
				{
					this.targetYaw = base.transform.localRotation.eulerAngles.y;
				}
			}
			if (this.power != null)
			{
				if (this.power.isWired)
				{
					if (this.isAlert)
					{
						this.lastDrift = Time.realtimeSinceStartup;
						this.yaw = Mathf.LerpAngle(this.yaw, this.targetYaw, 4f * Time.deltaTime);
					}
					else
					{
						this.yaw = Mathf.LerpAngle(this.yaw, this.targetYaw + Mathf.Sin(Time.realtimeSinceStartup - this.lastDrift) * 60f, 4f * Time.deltaTime);
					}
					this.pitch = Mathf.LerpAngle(this.pitch, this.targetPitch, 4f * Time.deltaTime);
					this.yawTransform.rotation = Quaternion.Euler(-90f, 0f, this.yaw);
					this.pitchTransform.localRotation = Quaternion.Euler(0f, -90f, this.pitch);
				}
				if (!Dedicator.isDedicated)
				{
					if (this.onGameObject != null)
					{
						this.onGameObject.SetActive(this.isAlert && this.power.isWired);
					}
					if (this.onModelGameObject != null)
					{
						this.onModelGameObject.SetActive(this.isAlert);
						if (this.onMaterial != null)
						{
							this.onMaterial.SetColor("_EmissionColor", (!this.isAlert || !this.power.isWired) ? Color.black : this.onMaterial.color);
						}
					}
					if (this.offGameObject != null)
					{
						this.offGameObject.SetActive(!this.isAlert && this.power.isWired);
					}
					if (this.offModelGameObject != null)
					{
						this.offModelGameObject.SetActive(!this.isAlert);
						if (this.offMaterial != null)
						{
							this.offMaterial.SetColor("_EmissionColor", (this.isAlert || !this.power.isWired) ? Color.black : this.offMaterial.color);
						}
					}
					if ((double)(Time.realtimeSinceStartup - this.lastShot) > 0.05 && this.muzzleEmitter != null)
					{
						this.muzzleEmitter.GetComponent<Light>().enabled = false;
					}
				}
			}
		}

		private void destroyEffects()
		{
			if (this.tracerEmitter != null)
			{
				EffectManager.Destroy(this.tracerEmitter.gameObject);
				this.tracerEmitter = null;
			}
			if (this.muzzleEmitter != null)
			{
				EffectManager.Destroy(this.muzzleEmitter.gameObject);
				this.muzzleEmitter = null;
			}
			if (this.shellEmitter != null)
			{
				EffectManager.Destroy(this.shellEmitter.gameObject);
				this.shellEmitter = null;
			}
		}

		private void OnDestroy()
		{
			this.destroyEffects();
			if (this.onMaterial != null)
			{
				Object.DestroyImmediate(this.onMaterial);
			}
			if (this.offMaterial != null)
			{
				Object.DestroyImmediate(this.offMaterial);
			}
		}

		private static List<Player> playersInRadius = new List<Player>();

		private static List<Zombie> zombiesInRadius = new List<Zombie>();

		public InteractablePower power;

		private bool hasWeapon;

		private bool interact;

		private Attachments attachments;

		private AudioSource sound;

		private ParticleSystem shellEmitter;

		private ParticleSystem muzzleEmitter;

		private ParticleSystem tracerEmitter;

		private Transform yawTransform;

		private Transform pitchTransform;

		private Transform aimTransform;

		private GameObject onGameObject;

		private GameObject onModelGameObject;

		private Material onMaterial;

		private GameObject offGameObject;

		private GameObject offModelGameObject;

		private Material offMaterial;

		private GameObject spotGameObject;

		private Player targetPlayer;

		private Zombie targetZombie;

		private float targetYaw;

		private float yaw;

		private float targetPitch;

		private float pitch;

		private bool isAlert;

		private float lastAlert;

		private bool isFiring;

		private float lastFire;

		private float fireTime;

		private bool isAiming;

		private float lastAim;

		private float lastScan;

		private float lastDrift;

		private float lastShot;

		private ESentryMode sentryMode;
	}
}
