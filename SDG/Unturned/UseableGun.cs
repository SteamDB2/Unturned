using System;
using System.Collections.Generic;
using SDG.Framework.Utilities;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	public class UseableGun : Useable
	{
		public bool isAiming { get; protected set; }

		[SteamCall]
		public void askFiremode(CSteamID steamID, byte id)
		{
			if (base.channel.checkOwner(steamID) && Provider.isServer)
			{
				if (base.player.equipment.isBusy)
				{
					return;
				}
				if (this.isFired)
				{
					return;
				}
				if (this.isReloading || this.isHammering || this.needsRechamber)
				{
					return;
				}
				if (base.player.equipment.asset == null)
				{
					return;
				}
				if (id == 0)
				{
					if (((ItemGunAsset)base.player.equipment.asset).hasSafety)
					{
						this.firemode = (EFiremode)id;
					}
				}
				else if (id == 1)
				{
					if (((ItemGunAsset)base.player.equipment.asset).hasSemi)
					{
						this.firemode = (EFiremode)id;
					}
				}
				else if (id == 2)
				{
					if (((ItemGunAsset)base.player.equipment.asset).hasAuto)
					{
						this.firemode = (EFiremode)id;
					}
				}
				else if (id == 3 && ((ItemGunAsset)base.player.equipment.asset).hasBurst)
				{
					this.firemode = (EFiremode)id;
				}
				base.player.equipment.state[11] = (byte)this.firemode;
				base.player.equipment.sendUpdateState();
				EffectManager.sendEffect(8, EffectManager.SMALL, base.transform.position);
			}
		}

		public void askInteractGun()
		{
			if (base.player.equipment.isBusy)
			{
				return;
			}
			if (this.isFired)
			{
				return;
			}
			if (this.isReloading || this.isHammering || this.needsRechamber)
			{
				return;
			}
			if (this.thirdAttachments.tacticalAsset == null)
			{
				return;
			}
			if (this.thirdAttachments.tacticalAsset.isMelee)
			{
				if (!this.isSprinting && (!base.player.movement.isSafe || !base.player.movement.isSafeInfo.noWeapons) && this.firemode != EFiremode.SAFETY)
				{
					this.isJabbing = true;
				}
			}
			else
			{
				this.interact = !this.interact;
				base.player.equipment.state[12] = ((!this.interact) ? 0 : 1);
				base.player.equipment.sendUpdateState();
				EffectManager.sendEffect(8, EffectManager.SMALL, base.transform.position);
			}
		}

		private void project(Vector3 origin, Vector3 direction)
		{
			if (this.sound != null)
			{
				if (((ItemGunAsset)base.player.equipment.asset).action == EAction.String)
				{
					this.sound.maxDistance = 16f;
				}
				else if (((ItemGunAsset)base.player.equipment.asset).action == EAction.Rocket)
				{
					this.sound.maxDistance = 64f;
				}
				else
				{
					this.sound.maxDistance = 512f;
				}
				if (this.thirdAttachments.barrelAsset != null && this.thirdAttachments.barrelAsset.isSilenced && base.player.equipment.state[16] > 0)
				{
					this.sound.clip = this.thirdAttachments.barrelAsset.shoot;
					this.sound.volume = this.thirdAttachments.barrelAsset.volume;
					this.sound.maxDistance *= 0.5f;
				}
				else
				{
					this.sound.clip = ((ItemGunAsset)base.player.equipment.asset).shoot;
					this.sound.volume = 1f;
				}
				this.sound.pitch = Random.Range(0.975f, 1.025f);
				this.sound.PlayOneShot(this.sound.clip);
			}
			if (this.thirdAttachments.barrelModel == null || !this.thirdAttachments.barrelAsset.isBraked || base.player.equipment.state[16] == 0)
			{
				if (this.firstMuzzleEmitter != null && base.player.look.perspective == EPlayerPerspective.FIRST && !((ItemGunAsset)base.player.equipment.asset).isTurret)
				{
					this.firstMuzzleEmitter.Emit(1);
					this.firstMuzzleEmitter.GetComponent<Light>().enabled = true;
					if (this.firstFakeLight != null)
					{
						this.firstFakeLight.GetComponent<Light>().enabled = true;
					}
				}
				if (this.thirdMuzzleEmitter != null && (!base.channel.isOwner || base.player.look.perspective == EPlayerPerspective.THIRD || ((ItemGunAsset)base.player.equipment.asset).isTurret))
				{
					this.thirdMuzzleEmitter.Emit(1);
					this.thirdMuzzleEmitter.GetComponent<Light>().enabled = true;
				}
			}
			Transform transform = Object.Instantiate<GameObject>(((ItemGunAsset)base.player.equipment.asset).projectile).transform;
			transform.name = "Projectile";
			transform.parent = Level.effects;
			transform.position = origin;
			transform.rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(90f, 0f, 0f);
			transform.GetComponent<Rigidbody>().AddForce(direction * ((ItemGunAsset)base.player.equipment.asset).ballisticForce);
			if (base.channel.isOwner && transform.GetComponent<AudioSource>() != null)
			{
				transform.GetComponent<AudioSource>().maxDistance = 512f;
			}
			Rocket rocket = transform.gameObject.AddComponent<Rocket>();
			if (Provider.isServer)
			{
				rocket.killer = base.channel.owner.playerID.steamID;
				rocket.range = ((ItemGunAsset)base.player.equipment.asset).range;
				rocket.playerDamage = ((ItemGunAsset)base.player.equipment.asset).playerDamageMultiplier.damage;
				rocket.zombieDamage = ((ItemGunAsset)base.player.equipment.asset).zombieDamageMultiplier.damage;
				rocket.animalDamage = ((ItemGunAsset)base.player.equipment.asset).animalDamageMultiplier.damage;
				rocket.barricadeDamage = ((ItemGunAsset)base.player.equipment.asset).barricadeDamage;
				rocket.structureDamage = ((ItemGunAsset)base.player.equipment.asset).structureDamage;
				rocket.vehicleDamage = ((ItemGunAsset)base.player.equipment.asset).vehicleDamage;
				rocket.resourceDamage = ((ItemGunAsset)base.player.equipment.asset).resourceDamage;
				rocket.objectDamage = ((ItemGunAsset)base.player.equipment.asset).objectDamage;
				rocket.explosion = ((ItemGunAsset)base.player.equipment.asset).explosion;
				rocket.ignoreTransform = base.transform;
			}
			Object.Destroy(transform.gameObject, 30f);
			this.lastShot = Time.realtimeSinceStartup;
		}

		[SteamCall]
		public void askProject(CSteamID steamID, Vector3 origin, Vector3 direction)
		{
			if (base.channel.checkServer(steamID) && base.player.equipment.isEquipped)
			{
				this.project(origin, direction);
			}
		}

		private void trace(Vector3 pos, Vector3 dir)
		{
			if (this.tracerEmitter == null)
			{
				return;
			}
			if (this.thirdAttachments.barrelModel != null && this.thirdAttachments.barrelAsset.isBraked && base.player.equipment.state[16] > 0)
			{
				return;
			}
			this.tracerEmitter.transform.position = pos;
			this.tracerEmitter.transform.rotation = Quaternion.LookRotation(dir);
			this.tracerEmitter.Emit(1);
		}

		private void shoot()
		{
			if (this.sound != null)
			{
				if (((ItemGunAsset)base.player.equipment.asset).action == EAction.String)
				{
					this.sound.maxDistance = 16f;
				}
				else if (((ItemGunAsset)base.player.equipment.asset).action == EAction.Rocket)
				{
					this.sound.maxDistance = 64f;
				}
				else
				{
					this.sound.maxDistance = 512f;
				}
				if (this.thirdAttachments.barrelAsset != null && this.thirdAttachments.barrelAsset.isSilenced && base.player.equipment.state[16] > 0)
				{
					this.sound.clip = this.thirdAttachments.barrelAsset.shoot;
					this.sound.volume = this.thirdAttachments.barrelAsset.volume;
					this.sound.maxDistance *= 0.5f;
				}
				else
				{
					this.sound.clip = ((ItemGunAsset)base.player.equipment.asset).shoot;
					this.sound.volume = 1f;
				}
				this.sound.pitch = Random.Range(0.975f, 1.025f);
				this.sound.PlayOneShot(this.sound.clip);
			}
			if (((ItemGunAsset)base.player.equipment.asset).action == EAction.Trigger || ((ItemGunAsset)base.player.equipment.asset).action == EAction.Minigun)
			{
				if (this.firstShellEmitter != null && base.player.look.perspective == EPlayerPerspective.FIRST && !((ItemGunAsset)base.player.equipment.asset).isTurret)
				{
					this.firstShellEmitter.Emit(1);
				}
				if (this.thirdShellEmitter != null && (!base.channel.isOwner || base.player.look.perspective == EPlayerPerspective.THIRD || ((ItemGunAsset)base.player.equipment.asset).isTurret))
				{
					this.thirdShellEmitter.Emit(1);
				}
			}
			if (this.thirdAttachments.barrelModel == null || !this.thirdAttachments.barrelAsset.isBraked || base.player.equipment.state[16] == 0)
			{
				if (this.firstMuzzleEmitter != null && base.player.look.perspective == EPlayerPerspective.FIRST && !((ItemGunAsset)base.player.equipment.asset).isTurret)
				{
					this.firstMuzzleEmitter.Emit(1);
					this.firstMuzzleEmitter.GetComponent<Light>().enabled = true;
					if (this.firstFakeLight != null)
					{
						this.firstFakeLight.GetComponent<Light>().enabled = true;
					}
				}
				if (this.thirdMuzzleEmitter != null && (!base.channel.isOwner || base.player.look.perspective == EPlayerPerspective.THIRD || ((ItemGunAsset)base.player.equipment.asset).isTurret))
				{
					this.thirdMuzzleEmitter.Emit(1);
					this.thirdMuzzleEmitter.GetComponent<Light>().enabled = true;
				}
			}
			if (!base.channel.isOwner)
			{
				if (((ItemGunAsset)base.player.equipment.asset).range < 32f)
				{
					this.trace(base.player.look.aim.position + base.player.look.aim.forward * 32f, base.player.look.aim.forward);
				}
				else
				{
					this.trace(base.player.look.aim.position + base.player.look.aim.forward * Random.Range(32f, Mathf.Min(64f, ((ItemGunAsset)base.player.equipment.asset).range)), base.player.look.aim.forward);
				}
			}
			this.lastShot = Time.realtimeSinceStartup;
			if (((ItemGunAsset)base.player.equipment.asset).action == EAction.Bolt || ((ItemGunAsset)base.player.equipment.asset).action == EAction.Pump)
			{
				this.needsRechamber = true;
			}
			if (this.thirdAttachments.barrelAsset != null && this.thirdAttachments.barrelAsset.durability > 0)
			{
				if (this.thirdAttachments.barrelAsset.durability > base.player.equipment.state[16])
				{
					base.player.equipment.state[16] = 0;
				}
				else
				{
					byte[] state = base.player.equipment.state;
					int num = 16;
					state[num] -= this.thirdAttachments.barrelAsset.durability;
				}
				if (base.channel.isOwner || Provider.isServer)
				{
					base.player.equipment.updateState();
				}
			}
		}

		[SteamCall]
		public void askShoot(CSteamID steamID)
		{
			if (base.channel.checkServer(steamID) && base.player.equipment.isEquipped)
			{
				this.shoot();
			}
		}

		private void fire()
		{
			float num = (float)base.player.equipment.quality / 100f;
			if (this.thirdAttachments.magazineAsset == null)
			{
				return;
			}
			this.ammo -= 1;
			if (base.channel.isOwner && this.ammo == 0)
			{
				PlayerUI.message(EPlayerMessage.RELOAD, string.Empty);
			}
			if (!this.isAiming)
			{
				base.player.equipment.uninspect();
			}
			if (Provider.isServer)
			{
				if (((ItemGunAsset)base.player.equipment.asset).action != EAction.String)
				{
					base.player.equipment.state[10] = this.ammo;
					base.player.equipment.updateState();
				}
				base.channel.send("askShoot", ESteamCall.NOT_OWNER, base.transform.position, EffectManager.INSANE, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[0]);
				this.lastShot = Time.realtimeSinceStartup;
				if (((ItemGunAsset)base.player.equipment.asset).action == EAction.Bolt || ((ItemGunAsset)base.player.equipment.asset).action == EAction.Pump)
				{
					this.needsRechamber = true;
				}
				if (this.thirdAttachments.barrelAsset == null || !this.thirdAttachments.barrelAsset.isSilenced || base.player.equipment.state[16] == 0)
				{
					AlertTool.alert(base.transform.position, ((ItemGunAsset)base.player.equipment.asset).alertRadius);
				}
				if (Provider.modeConfigData.Items.Has_Durability && base.player.equipment.quality > 0 && Random.value < ((ItemWeaponAsset)base.player.equipment.asset).durability)
				{
					if (base.player.equipment.quality > ((ItemWeaponAsset)base.player.equipment.asset).wear)
					{
						PlayerEquipment equipment = base.player.equipment;
						equipment.quality -= ((ItemWeaponAsset)base.player.equipment.asset).wear;
					}
					else
					{
						base.player.equipment.quality = 0;
					}
					base.player.equipment.sendUpdateQuality();
				}
			}
			if (base.channel.isOwner)
			{
				float num2 = (num >= 0.5f) ? 1f : (1f + (1f - num * 2f));
				if (this.isAiming)
				{
					num2 *= Mathf.Lerp(1f, ((ItemGunAsset)base.player.equipment.asset).spreadAim, (float)this.aimAccuracy / 10f);
				}
				num2 *= 1f - base.player.skills.mastery(0, 1) * 0.5f;
				if (this.thirdAttachments.sightAsset != null && this.isAiming)
				{
					num2 *= Mathf.Lerp(1f, this.thirdAttachments.sightAsset.spread, (float)this.aimAccuracy / 10f);
				}
				if (this.thirdAttachments.tacticalAsset != null && this.interact)
				{
					num2 *= this.thirdAttachments.tacticalAsset.spread;
				}
				if (this.thirdAttachments.gripAsset != null && (!this.thirdAttachments.gripAsset.isBipod || base.player.stance.stance == EPlayerStance.PRONE))
				{
					num2 *= this.thirdAttachments.gripAsset.spread;
				}
				if (this.thirdAttachments.barrelAsset != null)
				{
					num2 *= this.thirdAttachments.barrelAsset.spread;
				}
				if (this.thirdAttachments.magazineAsset != null)
				{
					num2 *= this.thirdAttachments.magazineAsset.spread;
				}
				if (base.player.stance.stance == EPlayerStance.CROUCH)
				{
					num2 *= UseableGun.SPREAD_CROUCH;
				}
				else if (base.player.stance.stance == EPlayerStance.PRONE)
				{
					num2 *= UseableGun.SPREAD_PRONE;
				}
				if (!base.player.look.isCam && base.player.look.perspective == EPlayerPerspective.THIRD)
				{
					RaycastHit raycastHit;
					PhysicsUtility.raycast(new Ray(MainCamera.instance.transform.position, MainCamera.instance.transform.forward), out raycastHit, 512f, RayMasks.DAMAGE_CLIENT, 0);
					if (raycastHit.transform != null)
					{
						if (Vector3.Dot(raycastHit.point - base.player.look.aim.position, MainCamera.instance.transform.forward) > 0f)
						{
							base.player.look.aim.rotation = Quaternion.LookRotation(raycastHit.point - base.player.look.aim.position);
						}
					}
					else
					{
						base.player.look.aim.rotation = Quaternion.LookRotation(MainCamera.instance.transform.position + MainCamera.instance.transform.forward * 512f - base.player.look.aim.position);
					}
				}
				if (((ItemGunAsset)base.player.equipment.asset).projectile == null)
				{
					byte pellets = this.thirdAttachments.magazineAsset.pellets;
					for (byte b = 0; b < pellets; b += 1)
					{
						Vector3 vector = base.player.look.aim.forward;
						vector += base.player.look.aim.right * Random.Range(-((ItemGunAsset)base.player.equipment.asset).spreadHip, ((ItemGunAsset)base.player.equipment.asset).spreadHip) * num2;
						vector += base.player.look.aim.up * Random.Range(-((ItemGunAsset)base.player.equipment.asset).spreadHip, ((ItemGunAsset)base.player.equipment.asset).spreadHip) * num2;
						vector.Normalize();
						BulletInfo bulletInfo = new BulletInfo();
						bulletInfo.pos = base.player.look.aim.position;
						bulletInfo.dir = vector;
						bulletInfo.pellet = b;
						bulletInfo.quality = num;
						bulletInfo.barrelAsset = this.thirdAttachments.barrelAsset;
						bulletInfo.magazineAsset = this.thirdAttachments.magazineAsset;
						this.bullets.Add(bulletInfo);
						int num3;
						if (Provider.provider.statisticsService.userStatisticsService.getStatistic("Accuracy_Shot", out num3))
						{
							Provider.provider.statisticsService.userStatisticsService.setStatistic("Accuracy_Shot", num3 + 1);
						}
					}
				}
				else
				{
					Vector3 forward = base.player.look.aim.forward;
					Ray ray;
					ray..ctor(base.player.look.aim.position, forward);
					RaycastInfo raycastInfo = DamageTool.raycast(ray, 512f, RayMasks.DAMAGE_CLIENT);
					if (raycastInfo.transform != null)
					{
						base.player.input.sendRaycast(raycastInfo);
					}
					Vector3 vector2 = base.player.look.aim.position;
					RaycastHit raycastHit2;
					if (!PhysicsUtility.raycast(new Ray(vector2, forward), out raycastHit2, 1f, RayMasks.DAMAGE_SERVER, 0))
					{
						vector2 += forward;
					}
					this.project(vector2, forward);
					int num4;
					if (Provider.provider.statisticsService.userStatisticsService.getStatistic("Accuracy_Shot", out num4))
					{
						Provider.provider.statisticsService.userStatisticsService.setStatistic("Accuracy_Shot", num4 + 1);
					}
				}
				float num5 = Random.Range(((ItemGunAsset)base.player.equipment.asset).recoilMin_x, ((ItemGunAsset)base.player.equipment.asset).recoilMax_x) * ((num >= 0.5f) ? 1f : (1f + (1f - num * 2f)));
				float num6 = Random.Range(((ItemGunAsset)base.player.equipment.asset).recoilMin_y, ((ItemGunAsset)base.player.equipment.asset).recoilMax_y) * ((num >= 0.5f) ? 1f : (1f + (1f - num * 2f)));
				float num7 = Random.Range(((ItemGunAsset)base.player.equipment.asset).shakeMin_x, ((ItemGunAsset)base.player.equipment.asset).shakeMax_x);
				float num8 = Random.Range(((ItemGunAsset)base.player.equipment.asset).shakeMin_y, ((ItemGunAsset)base.player.equipment.asset).shakeMax_y);
				float num9 = Random.Range(((ItemGunAsset)base.player.equipment.asset).shakeMin_z, ((ItemGunAsset)base.player.equipment.asset).shakeMax_z);
				num5 *= 1f - base.player.skills.mastery(0, 1) * 0.5f;
				num6 *= 1f - base.player.skills.mastery(0, 1) * 0.5f;
				if (this.thirdAttachments.sightAsset != null)
				{
					if (this.isAiming && ((ItemGunAsset)base.player.equipment.asset).useRecoilAim && this.thirdAttachments.sightAsset.zoom < 45f)
					{
						num5 *= Mathf.Lerp(1f, this.thirdAttachments.sightAsset.zoom / 90f * ((ItemGunAsset)base.player.equipment.asset).recoilAim, (float)this.aimAccuracy / 10f * ((base.player.look.perspective != EPlayerPerspective.FIRST) ? 0.5f : 1f));
						num6 *= Mathf.Lerp(1f, this.thirdAttachments.sightAsset.zoom / 90f * ((ItemGunAsset)base.player.equipment.asset).recoilAim, (float)this.aimAccuracy / 10f * ((base.player.look.perspective != EPlayerPerspective.FIRST) ? 0.5f : 1f));
					}
					num5 *= this.thirdAttachments.sightAsset.recoil_x;
					num6 *= this.thirdAttachments.sightAsset.recoil_y;
					num7 *= this.thirdAttachments.sightAsset.shake;
					num8 *= this.thirdAttachments.sightAsset.shake;
					num9 *= this.thirdAttachments.sightAsset.shake;
				}
				if (this.thirdAttachments.tacticalAsset != null && this.interact)
				{
					num5 *= this.thirdAttachments.tacticalAsset.recoil_x;
					num6 *= this.thirdAttachments.tacticalAsset.recoil_y;
					num7 *= this.thirdAttachments.tacticalAsset.shake;
					num8 *= this.thirdAttachments.tacticalAsset.shake;
					num9 *= this.thirdAttachments.tacticalAsset.shake;
				}
				if (this.thirdAttachments.gripAsset != null && (!this.thirdAttachments.gripAsset.isBipod || base.player.stance.stance == EPlayerStance.PRONE))
				{
					num5 *= this.thirdAttachments.gripAsset.recoil_x;
					num6 *= this.thirdAttachments.gripAsset.recoil_y;
					num7 *= this.thirdAttachments.gripAsset.shake;
					num8 *= this.thirdAttachments.gripAsset.shake;
					num9 *= this.thirdAttachments.gripAsset.shake;
				}
				if (this.thirdAttachments.barrelAsset != null)
				{
					num5 *= this.thirdAttachments.barrelAsset.recoil_x;
					num6 *= this.thirdAttachments.barrelAsset.recoil_y;
					num7 *= this.thirdAttachments.barrelAsset.shake;
					num8 *= this.thirdAttachments.barrelAsset.shake;
					num9 *= this.thirdAttachments.barrelAsset.shake;
				}
				if (this.thirdAttachments.magazineAsset != null)
				{
					num5 *= this.thirdAttachments.magazineAsset.recoil_x;
					num6 *= this.thirdAttachments.magazineAsset.recoil_y;
					num7 *= this.thirdAttachments.magazineAsset.shake;
					num8 *= this.thirdAttachments.magazineAsset.shake;
					num9 *= this.thirdAttachments.magazineAsset.shake;
				}
				if (base.player.stance.stance == EPlayerStance.CROUCH)
				{
					num5 *= UseableGun.RECOIL_CROUCH;
					num6 *= UseableGun.RECOIL_CROUCH;
					num7 *= UseableGun.SHAKE_CROUCH;
					num8 *= UseableGun.SHAKE_CROUCH;
					num9 *= UseableGun.SHAKE_CROUCH;
				}
				else if (base.player.stance.stance == EPlayerStance.PRONE)
				{
					num5 *= UseableGun.RECOIL_PRONE;
					num6 *= UseableGun.RECOIL_PRONE;
					num7 *= UseableGun.SHAKE_PRONE;
					num8 *= UseableGun.SHAKE_PRONE;
					num9 *= UseableGun.SHAKE_PRONE;
				}
				base.player.look.recoil(num5, num6, ((ItemGunAsset)base.player.equipment.asset).recover_x, ((ItemGunAsset)base.player.equipment.asset).recover_y);
				base.player.animator.shake(num7, num8, num9);
				this.updateInfo();
				if (((ItemGunAsset)base.player.equipment.asset).projectile == null)
				{
					this.shoot();
				}
			}
			if (Provider.isServer)
			{
				if (!base.channel.isOwner && this.thirdAttachments.barrelAsset != null && this.thirdAttachments.barrelAsset.durability > 0)
				{
					if (this.thirdAttachments.barrelAsset.durability > base.player.equipment.state[16])
					{
						base.player.equipment.state[16] = 0;
					}
					else
					{
						byte[] state = base.player.equipment.state;
						int num10 = 16;
						state[num10] -= this.thirdAttachments.barrelAsset.durability;
					}
					base.player.equipment.updateState();
				}
				if (((ItemGunAsset)base.player.equipment.asset).projectile == null)
				{
					byte pellets2 = this.thirdAttachments.magazineAsset.pellets;
					for (byte b2 = 0; b2 < pellets2; b2 += 1)
					{
						BulletInfo bulletInfo2;
						if (base.channel.isOwner)
						{
							bulletInfo2 = this.bullets[this.bullets.Count - (int)pellets2 + (int)b2];
						}
						else
						{
							bulletInfo2 = new BulletInfo();
							bulletInfo2.pos = base.player.look.aim.position;
							bulletInfo2.pellet = b2;
							bulletInfo2.quality = num;
							bulletInfo2.barrelAsset = this.thirdAttachments.barrelAsset;
							bulletInfo2.magazineAsset = this.thirdAttachments.magazineAsset;
							this.bullets.Add(bulletInfo2);
						}
						if (this.thirdAttachments.magazineAsset != null && this.thirdAttachments.magazineAsset.isExplosive)
						{
							if (((ItemGunAsset)base.player.equipment.asset).action == EAction.String)
							{
								base.player.equipment.state[8] = 0;
								base.player.equipment.state[9] = 0;
								base.player.equipment.state[10] = 0;
								base.player.equipment.state[17] = 0;
								base.player.equipment.sendUpdateState();
							}
						}
						else if (((ItemGunAsset)base.player.equipment.asset).action == EAction.String)
						{
							if (base.player.equipment.state[17] > 0)
							{
								if (base.player.equipment.state[17] > this.thirdAttachments.magazineAsset.stuck)
								{
									byte[] state2 = base.player.equipment.state;
									int num11 = 17;
									state2[num11] -= this.thirdAttachments.magazineAsset.stuck;
								}
								else
								{
									base.player.equipment.state[17] = 0;
								}
								bulletInfo2.dropID = this.thirdAttachments.magazineID;
								bulletInfo2.dropAmount = base.player.equipment.state[10];
								bulletInfo2.dropQuality = base.player.equipment.state[17];
							}
							base.player.equipment.state[8] = 0;
							base.player.equipment.state[9] = 0;
							base.player.equipment.state[10] = 0;
							base.player.equipment.sendUpdateState();
						}
					}
				}
				else
				{
					if (base.player.input.hasInputs())
					{
						InputInfo input = base.player.input.getInput(false);
						if (input != null)
						{
							base.player.look.aim.LookAt(input.point);
						}
					}
					if (this.ammo == 0)
					{
						base.player.equipment.state[8] = 0;
						base.player.equipment.state[9] = 0;
						base.player.equipment.state[10] = 0;
						base.player.equipment.sendUpdateState();
					}
					if (!base.channel.isOwner)
					{
						Vector3 vector3 = base.player.look.aim.position;
						Vector3 forward2 = base.player.look.aim.forward;
						RaycastHit raycastHit3;
						if (!PhysicsUtility.raycast(new Ray(vector3, forward2), out raycastHit3, 1f, RayMasks.DAMAGE_SERVER, 0))
						{
							vector3 += forward2;
						}
						this.project(vector3, forward2);
						base.channel.send("askProject", ESteamCall.NOT_OWNER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[]
						{
							base.player.look.aim.position,
							base.player.look.aim.forward
						});
					}
					base.player.life.markAggressive(false, true);
				}
			}
		}

		private void jab()
		{
			if (Provider.isServer)
			{
				AlertTool.alert(base.transform.position, 8f);
			}
			if (base.channel.isOwner)
			{
				base.player.animator.fling(0f, 0f, 0.8f);
				base.player.playSound((AudioClip)Resources.Load("Guns/Jab"), 0.5f);
				int num;
				if (Provider.provider.statisticsService.userStatisticsService.getStatistic("Accuracy_Shot", out num))
				{
					Provider.provider.statisticsService.userStatisticsService.setStatistic("Accuracy_Shot", num + 1);
				}
				Ray ray;
				ray..ctor(base.player.look.aim.position, base.player.look.aim.forward);
				RaycastInfo raycastInfo = DamageTool.raycast(ray, 2f, RayMasks.DAMAGE_CLIENT);
				if (raycastInfo.player != null && !base.player.quests.isMemberOfSameGroupAs(raycastInfo.player) && Provider.isPvP)
				{
					if (Provider.provider.statisticsService.userStatisticsService.getStatistic("Accuracy_Hit", out num))
					{
						Provider.provider.statisticsService.userStatisticsService.setStatistic("Accuracy_Hit", num + 1);
					}
					if (raycastInfo.limb == ELimb.SKULL && Provider.provider.statisticsService.userStatisticsService.getStatistic("Headshots", out num))
					{
						Provider.provider.statisticsService.userStatisticsService.setStatistic("Headshots", num + 1);
					}
					PlayerUI.hitmark(0, raycastInfo.point, false, (raycastInfo.limb != ELimb.SKULL) ? EPlayerHit.ENTITIY : EPlayerHit.CRITICAL);
				}
				else if (raycastInfo.zombie != null || raycastInfo.animal != null)
				{
					if (Provider.provider.statisticsService.userStatisticsService.getStatistic("Accuracy_Hit", out num))
					{
						Provider.provider.statisticsService.userStatisticsService.setStatistic("Accuracy_Hit", num + 1);
					}
					if (raycastInfo.limb == ELimb.SKULL && Provider.provider.statisticsService.userStatisticsService.getStatistic("Headshots", out num))
					{
						Provider.provider.statisticsService.userStatisticsService.setStatistic("Headshots", num + 1);
					}
					PlayerUI.hitmark(0, raycastInfo.point, false, (raycastInfo.limb != ELimb.SKULL) ? EPlayerHit.ENTITIY : EPlayerHit.CRITICAL);
				}
				base.player.input.sendRaycast(raycastInfo);
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
				if ((input.point - base.player.look.aim.position).sqrMagnitude > 36f)
				{
					return;
				}
				DamageTool.impact(input.point, input.normal, input.material, input.type != ERaycastInfoType.NONE && input.type != ERaycastInfoType.OBJECT);
				EPlayerKill eplayerKill = EPlayerKill.NONE;
				uint num2 = 0u;
				float num3 = 1f;
				num3 *= 1f + base.channel.owner.player.skills.mastery(0, 0) * 0.5f;
				if (input.type == ERaycastInfoType.PLAYER)
				{
					if (input.player != null && !base.player.quests.isMemberOfSameGroupAs(input.player) && Provider.isPvP)
					{
						DamageTool.damage(input.player, EDeathCause.MELEE, input.limb, base.channel.owner.playerID.steamID, input.direction, UseableGun.DAMAGE_PLAYER_MULTIPLIER, num3, true, out eplayerKill);
					}
				}
				else if (input.type == ERaycastInfoType.ZOMBIE)
				{
					if (input.zombie != null)
					{
						DamageTool.damage(input.zombie, input.limb, input.direction, UseableGun.DAMAGE_ZOMBIE_MULTIPLIER, num3, true, out eplayerKill, out num2);
						if (base.player.movement.nav != 255)
						{
							input.zombie.alert(base.transform.position, true);
						}
					}
				}
				else if (input.type == ERaycastInfoType.ANIMAL && input.animal != null)
				{
					DamageTool.damage(input.animal, input.limb, input.direction, UseableGun.DAMAGE_ANIMAL_MULTIPLIER, num3, out eplayerKill, out num2);
					input.animal.alertPoint(base.transform.position, true);
				}
				if (input.type != ERaycastInfoType.PLAYER && input.type != ERaycastInfoType.ZOMBIE && input.type != ERaycastInfoType.ANIMAL && !base.player.life.isAggressor)
				{
					float num4 = 2f + Provider.modeConfigData.Players.Ray_Aggressor_Distance;
					num4 *= num4;
					float num5 = Provider.modeConfigData.Players.Ray_Aggressor_Distance;
					num5 *= num5;
					Vector3 forward = base.player.look.aim.forward;
					for (int i = 0; i < Provider.clients.Count; i++)
					{
						if (Provider.clients[i] != base.channel.owner)
						{
							Player player = Provider.clients[i].player;
							if (!(player == null))
							{
								Vector3 vector = player.look.aim.position - base.player.look.aim.position;
								Vector3 vector2 = Vector3.Project(vector, forward);
								if (vector2.sqrMagnitude < num4 && (vector2 - vector).sqrMagnitude < num5)
								{
									base.player.life.markAggressive(false, true);
								}
							}
						}
					}
				}
				if (Level.info.type == ELevelType.HORDE)
				{
					if (input.zombie != null)
					{
						if (input.limb == ELimb.SKULL)
						{
							base.player.skills.askPay(10u);
						}
						else
						{
							base.player.skills.askPay(5u);
						}
					}
					if (eplayerKill == EPlayerKill.ZOMBIE)
					{
						if (input.limb == ELimb.SKULL)
						{
							base.player.skills.askPay(50u);
						}
						else
						{
							base.player.skills.askPay(25u);
						}
					}
				}
				else
				{
					if (eplayerKill == EPlayerKill.PLAYER)
					{
						base.player.sendStat(EPlayerStat.KILLS_PLAYERS);
						if (Level.info.type == ELevelType.ARENA)
						{
							base.player.skills.askPay(100u);
						}
					}
					else if (eplayerKill == EPlayerKill.ZOMBIE)
					{
						base.player.sendStat(EPlayerStat.KILLS_ZOMBIES_NORMAL);
					}
					else if (eplayerKill == EPlayerKill.MEGA)
					{
						base.player.sendStat(EPlayerStat.KILLS_ZOMBIES_MEGA);
					}
					else if (eplayerKill == EPlayerKill.ANIMAL)
					{
						base.player.sendStat(EPlayerStat.KILLS_ANIMALS);
					}
					else if (eplayerKill == EPlayerKill.RESOURCE)
					{
						base.player.sendStat(EPlayerStat.FOUND_RESOURCES);
					}
					if (num2 > 0u)
					{
						base.player.skills.askPay(num2);
					}
				}
			}
		}

		private void ballistics()
		{
			if (((ItemGunAsset)base.player.equipment.asset).projectile != null || this.bullets == null)
			{
				return;
			}
			if (base.channel.isOwner)
			{
				for (int i = 0; i < this.bullets.Count; i++)
				{
					BulletInfo bulletInfo = this.bullets[i];
					byte pellets = bulletInfo.magazineAsset.pellets;
					if (base.channel.isOwner)
					{
						EPlayerHit eplayerHit = EPlayerHit.NONE;
						if (pellets > 1)
						{
							this.hitmarkerIndex = (int)bulletInfo.pellet;
						}
						else if (OptionsSettings.hitmarker)
						{
							this.hitmarkerIndex++;
							if (this.hitmarkerIndex >= PlayerLifeUI.hitmarkers.Length)
							{
								this.hitmarkerIndex = 0;
							}
						}
						else
						{
							this.hitmarkerIndex = 0;
						}
						Ray ray;
						ray..ctor(bulletInfo.pos, bulletInfo.dir);
						RaycastInfo raycastInfo = DamageTool.raycast(ray, (!Provider.modeConfigData.Gameplay.Ballistics) ? ((ItemGunAsset)base.player.equipment.asset).range : ((ItemGunAsset)base.player.equipment.asset).ballisticTravel, RayMasks.DAMAGE_CLIENT);
						if (raycastInfo.player != null && ((ItemGunAsset)base.player.equipment.asset).playerDamageMultiplier.damage > 1f && !base.player.quests.isMemberOfSameGroupAs(raycastInfo.player) && Provider.isPvP)
						{
							if (eplayerHit != EPlayerHit.CRITICAL)
							{
								eplayerHit = ((raycastInfo.limb != ELimb.SKULL) ? EPlayerHit.ENTITIY : EPlayerHit.CRITICAL);
							}
							PlayerUI.hitmark(this.hitmarkerIndex, raycastInfo.point, pellets > 1, (raycastInfo.limb != ELimb.SKULL) ? EPlayerHit.ENTITIY : EPlayerHit.CRITICAL);
						}
						else if ((raycastInfo.zombie != null && ((ItemGunAsset)base.player.equipment.asset).zombieDamageMultiplier.damage > 1f) || (raycastInfo.animal != null && ((ItemGunAsset)base.player.equipment.asset).animalDamageMultiplier.damage > 1f))
						{
							if (eplayerHit != EPlayerHit.CRITICAL)
							{
								eplayerHit = ((raycastInfo.limb != ELimb.SKULL) ? EPlayerHit.ENTITIY : EPlayerHit.CRITICAL);
							}
							PlayerUI.hitmark(this.hitmarkerIndex, raycastInfo.point, pellets > 1, (raycastInfo.limb != ELimb.SKULL) ? EPlayerHit.ENTITIY : EPlayerHit.CRITICAL);
						}
						else if (raycastInfo.transform != null && raycastInfo.transform.CompareTag("Barricade") && ((ItemGunAsset)base.player.equipment.asset).barricadeDamage > 1f)
						{
							InteractableDoorHinge component = raycastInfo.transform.GetComponent<InteractableDoorHinge>();
							if (component != null)
							{
								raycastInfo.transform = component.transform.parent.parent;
							}
							ushort id;
							if (ushort.TryParse(raycastInfo.transform.name, out id))
							{
								ItemBarricadeAsset itemBarricadeAsset = (ItemBarricadeAsset)Assets.find(EAssetType.ITEM, id);
								if (itemBarricadeAsset != null && (itemBarricadeAsset.isVulnerable || ((ItemWeaponAsset)base.player.equipment.asset).isInvulnerable))
								{
									if (eplayerHit == EPlayerHit.NONE)
									{
										eplayerHit = EPlayerHit.BUILD;
									}
									PlayerUI.hitmark(this.hitmarkerIndex, raycastInfo.point, pellets > 1, EPlayerHit.BUILD);
								}
							}
						}
						else if (raycastInfo.transform != null && raycastInfo.transform.CompareTag("Structure") && ((ItemGunAsset)base.player.equipment.asset).structureDamage > 1f)
						{
							ushort id2;
							if (ushort.TryParse(raycastInfo.transform.name, out id2))
							{
								ItemStructureAsset itemStructureAsset = (ItemStructureAsset)Assets.find(EAssetType.ITEM, id2);
								if (itemStructureAsset != null && (itemStructureAsset.isVulnerable || ((ItemWeaponAsset)base.player.equipment.asset).isInvulnerable))
								{
									if (eplayerHit == EPlayerHit.NONE)
									{
										eplayerHit = EPlayerHit.BUILD;
									}
									PlayerUI.hitmark(this.hitmarkerIndex, raycastInfo.point, pellets > 1, EPlayerHit.BUILD);
								}
							}
						}
						else if (raycastInfo.vehicle != null && !raycastInfo.vehicle.isDead && ((ItemGunAsset)base.player.equipment.asset).vehicleDamage > 1f)
						{
							if (raycastInfo.vehicle.asset != null && (raycastInfo.vehicle.asset.isVulnerable || ((ItemWeaponAsset)base.player.equipment.asset).isInvulnerable))
							{
								if (eplayerHit == EPlayerHit.NONE)
								{
									eplayerHit = EPlayerHit.BUILD;
								}
								PlayerUI.hitmark(this.hitmarkerIndex, raycastInfo.point, pellets > 1, EPlayerHit.BUILD);
							}
						}
						else if (raycastInfo.transform != null && raycastInfo.transform.CompareTag("Resource") && ((ItemGunAsset)base.player.equipment.asset).resourceDamage > 1f)
						{
							byte x;
							byte y;
							ushort index;
							if (ResourceManager.tryGetRegion(raycastInfo.transform, out x, out y, out index))
							{
								ResourceSpawnpoint resourceSpawnpoint = ResourceManager.getResourceSpawnpoint(x, y, index);
								if (resourceSpawnpoint != null && !resourceSpawnpoint.isDead && resourceSpawnpoint.asset.bladeID == ((ItemWeaponAsset)base.player.equipment.asset).bladeID)
								{
									if (eplayerHit == EPlayerHit.NONE)
									{
										eplayerHit = EPlayerHit.BUILD;
									}
									PlayerUI.hitmark(this.hitmarkerIndex, raycastInfo.point, pellets > 1, EPlayerHit.BUILD);
								}
							}
						}
						else if (raycastInfo.transform != null && ((ItemGunAsset)base.player.equipment.asset).objectDamage > 1f)
						{
							InteractableObjectRubble component2 = raycastInfo.transform.GetComponent<InteractableObjectRubble>();
							if (component2 != null)
							{
								raycastInfo.section = component2.getSection(raycastInfo.collider.transform);
								if (!component2.isSectionDead(raycastInfo.section) && (component2.asset.rubbleIsVulnerable || ((ItemWeaponAsset)base.player.equipment.asset).isInvulnerable))
								{
									if (eplayerHit == EPlayerHit.NONE)
									{
										eplayerHit = EPlayerHit.BUILD;
									}
									PlayerUI.hitmark(this.hitmarkerIndex, raycastInfo.point, pellets > 1, EPlayerHit.BUILD);
								}
							}
						}
						if (Provider.modeConfigData.Gameplay.Ballistics)
						{
							if (bulletInfo.steps > 0 || ((ItemGunAsset)base.player.equipment.asset).ballisticSteps <= 1)
							{
								if (((ItemGunAsset)base.player.equipment.asset).ballisticTravel < 32f)
								{
									this.trace(bulletInfo.pos + bulletInfo.dir * 32f, bulletInfo.dir);
								}
								else
								{
									this.trace(bulletInfo.pos + bulletInfo.dir * Random.Range(32f, ((ItemGunAsset)base.player.equipment.asset).ballisticTravel), bulletInfo.dir);
								}
							}
						}
						else if (((ItemGunAsset)base.player.equipment.asset).range < 32f)
						{
							this.trace(ray.origin + ray.direction * 32f, ray.direction);
						}
						else
						{
							this.trace(ray.origin + ray.direction * Random.Range(32f, Mathf.Min(64f, ((ItemGunAsset)base.player.equipment.asset).range)), ray.direction);
						}
						if (base.player.input.isRaycastInvalid(raycastInfo))
						{
							float num = ((ItemGunAsset)base.player.equipment.asset).ballisticDrop;
							if (bulletInfo.barrelAsset != null)
							{
								num *= bulletInfo.barrelAsset.ballisticDrop;
							}
							bulletInfo.pos += bulletInfo.dir * ((ItemGunAsset)base.player.equipment.asset).ballisticTravel;
							BulletInfo bulletInfo2 = bulletInfo;
							bulletInfo2.dir.y = bulletInfo2.dir.y - num;
							bulletInfo.dir.Normalize();
						}
						else
						{
							if (eplayerHit != EPlayerHit.NONE)
							{
								int num2;
								if (Provider.provider.statisticsService.userStatisticsService.getStatistic("Accuracy_Hit", out num2))
								{
									Provider.provider.statisticsService.userStatisticsService.setStatistic("Accuracy_Hit", num2 + 1);
								}
								if (eplayerHit == EPlayerHit.CRITICAL && Provider.provider.statisticsService.userStatisticsService.getStatistic("Headshots", out num2))
								{
									Provider.provider.statisticsService.userStatisticsService.setStatistic("Headshots", num2 + 1);
								}
							}
							base.player.input.sendRaycast(raycastInfo);
							bulletInfo.steps = 254;
						}
					}
				}
			}
			if (Provider.isServer)
			{
				while (this.bullets.Count > 0)
				{
					BulletInfo bulletInfo3 = this.bullets[0];
					byte pellets2 = bulletInfo3.magazineAsset.pellets;
					if (!base.player.input.hasInputs())
					{
						break;
					}
					InputInfo input = base.player.input.getInput(true);
					if (input == null)
					{
						break;
					}
					if (!base.channel.isOwner)
					{
						if (Provider.modeConfigData.Gameplay.Ballistics)
						{
							if ((input.point - bulletInfo3.pos).magnitude > ((ItemGunAsset)base.player.equipment.asset).ballisticTravel * (float)((long)(bulletInfo3.steps + 1) + (long)((ulong)PlayerInput.SAMPLES)) + 4f)
							{
								this.bullets.RemoveAt(0);
								continue;
							}
						}
						else if ((input.point - base.player.look.aim.position).sqrMagnitude > Mathf.Pow(((ItemGunAsset)base.player.equipment.asset).range + 4f, 2f))
						{
							break;
						}
					}
					if (input.material != EPhysicsMaterial.NONE)
					{
						if (bulletInfo3.magazineAsset != null && bulletInfo3.magazineAsset.impact != 0)
						{
							DamageTool.impact(input.point, input.normal, bulletInfo3.magazineAsset.impact, base.channel.owner.playerID.steamID, base.transform.position);
						}
						else
						{
							DamageTool.impact(input.point, input.normal, input.material, input.type != ERaycastInfoType.NONE && input.type != ERaycastInfoType.OBJECT, base.channel.owner.playerID.steamID, base.transform.position);
						}
					}
					EPlayerKill eplayerKill = EPlayerKill.NONE;
					uint num3 = 0u;
					float num4 = 1f;
					num4 *= ((bulletInfo3.quality >= 0.5f) ? 1f : (0.5f + bulletInfo3.quality));
					if (input.type == ERaycastInfoType.PLAYER)
					{
						if (input.player != null && !base.player.quests.isMemberOfSameGroupAs(input.player) && Provider.isPvP)
						{
							DamageTool.damage(input.player, EDeathCause.GUN, input.limb, base.channel.owner.playerID.steamID, input.direction * Mathf.Ceil((float)pellets2 / 2f), ((ItemGunAsset)base.player.equipment.asset).playerDamageMultiplier, num4, true, out eplayerKill);
						}
					}
					else if (input.type == ERaycastInfoType.ZOMBIE)
					{
						if (input.zombie != null)
						{
							DamageTool.damage(input.zombie, input.limb, input.direction * Mathf.Ceil((float)pellets2 / 2f), ((ItemGunAsset)base.player.equipment.asset).zombieDamageMultiplier, num4, true, out eplayerKill, out num3);
							if (base.player.movement.nav != 255)
							{
								input.zombie.alert(base.transform.position, true);
							}
						}
					}
					else if (input.type == ERaycastInfoType.ANIMAL)
					{
						if (input.animal != null)
						{
							DamageTool.damage(input.animal, input.limb, input.direction * Mathf.Ceil((float)pellets2 / 2f), ((ItemGunAsset)base.player.equipment.asset).animalDamageMultiplier, num4, out eplayerKill, out num3);
							input.animal.alertPoint(base.transform.position, true);
						}
					}
					else if (input.type == ERaycastInfoType.VEHICLE)
					{
						if (input.vehicle != null && input.vehicle.asset != null && (input.vehicle.asset.isVulnerable || ((ItemWeaponAsset)base.player.equipment.asset).isInvulnerable))
						{
							DamageTool.damage(input.vehicle, true, input.point, false, ((ItemGunAsset)base.player.equipment.asset).vehicleDamage, num4, true, out eplayerKill);
						}
					}
					else if (input.type == ERaycastInfoType.BARRICADE)
					{
						ushort id3;
						if (input.transform != null && input.transform.CompareTag("Barricade") && ushort.TryParse(input.transform.name, out id3))
						{
							ItemBarricadeAsset itemBarricadeAsset2 = (ItemBarricadeAsset)Assets.find(EAssetType.ITEM, id3);
							if (itemBarricadeAsset2 != null && (itemBarricadeAsset2.isVulnerable || ((ItemWeaponAsset)base.player.equipment.asset).isInvulnerable))
							{
								DamageTool.damage(input.transform, false, ((ItemGunAsset)base.player.equipment.asset).barricadeDamage, num4, out eplayerKill);
							}
						}
					}
					else if (input.type == ERaycastInfoType.STRUCTURE)
					{
						ushort id4;
						if (input.transform != null && input.transform.CompareTag("Structure") && ushort.TryParse(input.transform.name, out id4))
						{
							ItemStructureAsset itemStructureAsset2 = (ItemStructureAsset)Assets.find(EAssetType.ITEM, id4);
							if (itemStructureAsset2 != null && (itemStructureAsset2.isVulnerable || ((ItemWeaponAsset)base.player.equipment.asset).isInvulnerable))
							{
								DamageTool.damage(input.transform, false, input.direction * Mathf.Ceil((float)pellets2 / 2f), ((ItemGunAsset)base.player.equipment.asset).structureDamage, num4, out eplayerKill);
							}
						}
					}
					else if (input.type == ERaycastInfoType.RESOURCE)
					{
						byte x2;
						byte y2;
						ushort index2;
						if (input.transform != null && input.transform.CompareTag("Resource") && ResourceManager.tryGetRegion(input.transform, out x2, out y2, out index2))
						{
							ResourceSpawnpoint resourceSpawnpoint2 = ResourceManager.getResourceSpawnpoint(x2, y2, index2);
							if (resourceSpawnpoint2 != null && !resourceSpawnpoint2.isDead && resourceSpawnpoint2.asset.bladeID == ((ItemWeaponAsset)base.player.equipment.asset).bladeID)
							{
								DamageTool.damage(input.transform, input.direction * Mathf.Ceil((float)pellets2 / 2f), ((ItemGunAsset)base.player.equipment.asset).resourceDamage, num4, 1f, out eplayerKill, out num3);
							}
						}
					}
					else if (input.type == ERaycastInfoType.OBJECT && input.transform != null && input.section < 255)
					{
						InteractableObjectRubble component3 = input.transform.GetComponent<InteractableObjectRubble>();
						if (component3 != null && !component3.isSectionDead(input.section) && (component3.asset.rubbleIsVulnerable || ((ItemWeaponAsset)base.player.equipment.asset).isInvulnerable))
						{
							DamageTool.damage(input.transform, input.direction, input.section, ((ItemGunAsset)base.player.equipment.asset).objectDamage, num4, out eplayerKill, out num3);
						}
					}
					if (input.type != ERaycastInfoType.PLAYER && input.type != ERaycastInfoType.ZOMBIE && input.type != ERaycastInfoType.ANIMAL && !base.player.life.isAggressor)
					{
						float num5 = ((ItemGunAsset)base.player.equipment.asset).range + Provider.modeConfigData.Players.Ray_Aggressor_Distance;
						num5 *= num5;
						float num6 = Provider.modeConfigData.Players.Ray_Aggressor_Distance;
						num6 *= num6;
						Vector3 normalized = (bulletInfo3.pos - base.player.look.aim.position).normalized;
						for (int j = 0; j < Provider.clients.Count; j++)
						{
							if (Provider.clients[j] != base.channel.owner)
							{
								Player player = Provider.clients[j].player;
								if (!(player == null))
								{
									Vector3 vector = player.look.aim.position - base.player.look.aim.position;
									Vector3 vector2 = Vector3.Project(vector, normalized);
									if (vector2.sqrMagnitude < num5 && (vector2 - vector).sqrMagnitude < num6)
									{
										base.player.life.markAggressive(false, true);
									}
								}
							}
						}
					}
					if (Level.info.type == ELevelType.HORDE)
					{
						if (input.zombie != null)
						{
							if (input.limb == ELimb.SKULL)
							{
								base.player.skills.askPay(10u);
							}
							else
							{
								base.player.skills.askPay(5u);
							}
						}
						if (eplayerKill == EPlayerKill.ZOMBIE)
						{
							if (input.limb == ELimb.SKULL)
							{
								base.player.skills.askPay(50u);
							}
							else
							{
								base.player.skills.askPay(25u);
							}
						}
					}
					else
					{
						if (eplayerKill == EPlayerKill.PLAYER)
						{
							base.player.sendStat(EPlayerStat.KILLS_PLAYERS);
							if (Level.info.type == ELevelType.ARENA)
							{
								base.player.skills.askPay(100u);
							}
						}
						else if (eplayerKill == EPlayerKill.ZOMBIE)
						{
							base.player.sendStat(EPlayerStat.KILLS_ZOMBIES_NORMAL);
						}
						else if (eplayerKill == EPlayerKill.MEGA)
						{
							base.player.sendStat(EPlayerStat.KILLS_ZOMBIES_MEGA);
						}
						else if (eplayerKill == EPlayerKill.ANIMAL)
						{
							base.player.sendStat(EPlayerStat.KILLS_ANIMALS);
						}
						else if (eplayerKill == EPlayerKill.RESOURCE)
						{
							base.player.sendStat(EPlayerStat.FOUND_RESOURCES);
						}
						if (num3 > 0u)
						{
							base.player.skills.askPay(num3);
						}
					}
					Vector3 point = input.point + input.normal * 0.25f;
					if (bulletInfo3.magazineAsset != null && bulletInfo3.magazineAsset.isExplosive)
					{
						EffectManager.sendEffect(bulletInfo3.magazineAsset.explosion, EffectManager.MEDIUM, point);
						DamageTool.explode(point, bulletInfo3.magazineAsset.range, EDeathCause.SPLASH, base.channel.owner.playerID.steamID, bulletInfo3.magazineAsset.playerDamage, bulletInfo3.magazineAsset.zombieDamage, bulletInfo3.magazineAsset.animalDamage, bulletInfo3.magazineAsset.barricadeDamage, bulletInfo3.magazineAsset.structureDamage, bulletInfo3.magazineAsset.vehicleDamage, bulletInfo3.magazineAsset.resourceDamage, bulletInfo3.magazineAsset.objectDamage, EExplosionDamageType.CONVENTIONAL, 32f, true);
					}
					if (bulletInfo3.dropID != 0)
					{
						ItemManager.dropItem(new Item(bulletInfo3.dropID, bulletInfo3.dropAmount, bulletInfo3.dropQuality), point, false, Dedicator.isDedicated, false);
					}
					this.bullets.RemoveAt(0);
				}
			}
			if (base.player.equipment.asset != null)
			{
				if (Provider.modeConfigData.Gameplay.Ballistics)
				{
					for (int k = this.bullets.Count - 1; k >= 0; k--)
					{
						BulletInfo bulletInfo4 = this.bullets[k];
						BulletInfo bulletInfo5 = bulletInfo4;
						bulletInfo5.steps += 1;
						if (bulletInfo4.steps >= ((ItemGunAsset)base.player.equipment.asset).ballisticSteps)
						{
							this.bullets.RemoveAt(k);
						}
					}
				}
				else
				{
					this.bullets.Clear();
				}
			}
		}

		[SteamCall]
		public void askAttachSight(CSteamID steamID, byte page, byte x, byte y)
		{
			if (base.channel.checkOwner(steamID) && Provider.isServer)
			{
				if (base.player.equipment.isBusy)
				{
					return;
				}
				if (this.isFired)
				{
					return;
				}
				if (this.isReloading || this.isHammering || this.needsRechamber)
				{
					return;
				}
				if (base.player.equipment.asset == null)
				{
					return;
				}
				if (!((ItemGunAsset)base.player.equipment.asset).hasSight)
				{
					return;
				}
				Item item = null;
				if (this.thirdAttachments.sightAsset != null)
				{
					item = new Item(this.thirdAttachments.sightID, false, base.player.equipment.state[13]);
				}
				if (page != 255)
				{
					byte index = base.player.inventory.getIndex(page, x, y);
					if (index != 255)
					{
						ItemJar item2 = base.player.inventory.getItem(page, index);
						ItemCaliberAsset itemCaliberAsset = (ItemCaliberAsset)Assets.find(EAssetType.ITEM, item2.item.id);
						if (itemCaliberAsset == null)
						{
							return;
						}
						if (itemCaliberAsset.calibers.Length != 0)
						{
							bool flag = false;
							byte b = 0;
							while ((int)b < itemCaliberAsset.calibers.Length)
							{
								byte b2 = 0;
								while ((int)b2 < ((ItemGunAsset)base.player.equipment.asset).attachmentCalibers.Length)
								{
									if (itemCaliberAsset.calibers[(int)b] == ((ItemGunAsset)base.player.equipment.asset).attachmentCalibers[(int)b2])
									{
										flag = true;
										break;
									}
									b2 += 1;
								}
								b += 1;
							}
							if (!flag)
							{
								return;
							}
						}
						Buffer.BlockCopy(BitConverter.GetBytes(item2.item.id), 0, base.player.equipment.state, 0, 2);
						base.player.equipment.state[13] = item2.item.quality;
						base.player.inventory.removeItem(page, index);
						if (item != null)
						{
							base.player.inventory.forceAddItem(item, true);
						}
						base.player.equipment.sendUpdateState();
						EffectManager.sendEffect(8, EffectManager.SMALL, base.transform.position);
						return;
					}
				}
				if (item != null)
				{
					base.player.inventory.forceAddItem(item, true);
				}
				base.player.equipment.state[0] = 0;
				base.player.equipment.state[1] = 0;
				base.player.equipment.sendUpdateState();
				EffectManager.sendEffect(8, EffectManager.SMALL, base.transform.position);
			}
		}

		[SteamCall]
		public void askAttachTactical(CSteamID steamID, byte page, byte x, byte y)
		{
			if (base.channel.checkOwner(steamID) && Provider.isServer)
			{
				if (base.player.equipment.isBusy)
				{
					return;
				}
				if (this.isFired)
				{
					return;
				}
				if (this.isReloading || this.isHammering || this.needsRechamber)
				{
					return;
				}
				if (!((ItemGunAsset)base.player.equipment.asset).hasTactical)
				{
					return;
				}
				if (base.player.equipment.asset == null)
				{
					return;
				}
				Item item = null;
				if (this.thirdAttachments.tacticalAsset != null)
				{
					item = new Item(this.thirdAttachments.tacticalID, false, base.player.equipment.state[14]);
				}
				if (page != 255)
				{
					byte index = base.player.inventory.getIndex(page, x, y);
					if (index != 255)
					{
						ItemJar item2 = base.player.inventory.getItem(page, index);
						ItemCaliberAsset itemCaliberAsset = (ItemCaliberAsset)Assets.find(EAssetType.ITEM, item2.item.id);
						if (itemCaliberAsset == null)
						{
							return;
						}
						if (itemCaliberAsset.calibers.Length != 0)
						{
							bool flag = false;
							byte b = 0;
							while ((int)b < itemCaliberAsset.calibers.Length)
							{
								byte b2 = 0;
								while ((int)b2 < ((ItemGunAsset)base.player.equipment.asset).attachmentCalibers.Length)
								{
									if (itemCaliberAsset.calibers[(int)b] == ((ItemGunAsset)base.player.equipment.asset).attachmentCalibers[(int)b2])
									{
										flag = true;
										break;
									}
									b2 += 1;
								}
								b += 1;
							}
							if (!flag)
							{
								return;
							}
						}
						Buffer.BlockCopy(BitConverter.GetBytes(item2.item.id), 0, base.player.equipment.state, 2, 2);
						base.player.equipment.state[14] = item2.item.quality;
						base.player.inventory.removeItem(page, index);
						if (item != null)
						{
							base.player.inventory.forceAddItem(item, true);
						}
						base.player.equipment.sendUpdateState();
						EffectManager.sendEffect(8, EffectManager.SMALL, base.transform.position);
						return;
					}
				}
				if (item != null)
				{
					base.player.inventory.forceAddItem(item, true);
				}
				base.player.equipment.state[2] = 0;
				base.player.equipment.state[3] = 0;
				base.player.equipment.sendUpdateState();
				EffectManager.sendEffect(8, EffectManager.SMALL, base.transform.position);
			}
		}

		[SteamCall]
		public void askAttachGrip(CSteamID steamID, byte page, byte x, byte y)
		{
			if (base.channel.checkOwner(steamID) && Provider.isServer)
			{
				if (base.player.equipment.isBusy)
				{
					return;
				}
				if (this.isFired)
				{
					return;
				}
				if (this.isReloading || this.isHammering || this.needsRechamber)
				{
					return;
				}
				if (base.player.equipment.asset == null)
				{
					return;
				}
				if (!((ItemGunAsset)base.player.equipment.asset).hasGrip)
				{
					return;
				}
				Item item = null;
				if (this.thirdAttachments.gripAsset != null)
				{
					item = new Item(this.thirdAttachments.gripID, false, base.player.equipment.state[15]);
				}
				if (page != 255)
				{
					byte index = base.player.inventory.getIndex(page, x, y);
					if (index != 255)
					{
						ItemJar item2 = base.player.inventory.getItem(page, index);
						ItemCaliberAsset itemCaliberAsset = (ItemCaliberAsset)Assets.find(EAssetType.ITEM, item2.item.id);
						if (itemCaliberAsset == null)
						{
							return;
						}
						if (itemCaliberAsset.calibers.Length != 0)
						{
							bool flag = false;
							byte b = 0;
							while ((int)b < itemCaliberAsset.calibers.Length)
							{
								byte b2 = 0;
								while ((int)b2 < ((ItemGunAsset)base.player.equipment.asset).attachmentCalibers.Length)
								{
									if (itemCaliberAsset.calibers[(int)b] == ((ItemGunAsset)base.player.equipment.asset).attachmentCalibers[(int)b2])
									{
										flag = true;
										break;
									}
									b2 += 1;
								}
								b += 1;
							}
							if (!flag)
							{
								return;
							}
						}
						Buffer.BlockCopy(BitConverter.GetBytes(item2.item.id), 0, base.player.equipment.state, 4, 2);
						base.player.equipment.state[15] = item2.item.quality;
						base.player.inventory.removeItem(page, index);
						if (item != null)
						{
							base.player.inventory.forceAddItem(item, true);
						}
						base.player.equipment.sendUpdateState();
						EffectManager.sendEffect(8, EffectManager.SMALL, base.transform.position);
						return;
					}
				}
				if (item != null)
				{
					base.player.inventory.forceAddItem(item, true);
				}
				base.player.equipment.state[4] = 0;
				base.player.equipment.state[5] = 0;
				base.player.equipment.sendUpdateState();
				EffectManager.sendEffect(8, EffectManager.SMALL, base.transform.position);
			}
		}

		[SteamCall]
		public void askAttachBarrel(CSteamID steamID, byte page, byte x, byte y)
		{
			if (base.channel.checkOwner(steamID) && Provider.isServer)
			{
				if (base.player.equipment.isBusy)
				{
					return;
				}
				if (this.isFired)
				{
					return;
				}
				if (this.isReloading || this.isHammering || this.needsRechamber)
				{
					return;
				}
				if (base.player.equipment.asset == null)
				{
					return;
				}
				if (!((ItemGunAsset)base.player.equipment.asset).hasBarrel)
				{
					return;
				}
				Item item = null;
				if (this.thirdAttachments.barrelAsset != null)
				{
					item = new Item(this.thirdAttachments.barrelID, false, base.player.equipment.state[16]);
				}
				if (page != 255)
				{
					byte index = base.player.inventory.getIndex(page, x, y);
					if (index != 255)
					{
						ItemJar item2 = base.player.inventory.getItem(page, index);
						ItemCaliberAsset itemCaliberAsset = (ItemCaliberAsset)Assets.find(EAssetType.ITEM, item2.item.id);
						if (itemCaliberAsset == null)
						{
							return;
						}
						if (itemCaliberAsset.calibers.Length != 0)
						{
							bool flag = false;
							byte b = 0;
							while ((int)b < itemCaliberAsset.calibers.Length)
							{
								byte b2 = 0;
								while ((int)b2 < ((ItemGunAsset)base.player.equipment.asset).attachmentCalibers.Length)
								{
									if (itemCaliberAsset.calibers[(int)b] == ((ItemGunAsset)base.player.equipment.asset).attachmentCalibers[(int)b2])
									{
										flag = true;
										break;
									}
									b2 += 1;
								}
								b += 1;
							}
							if (!flag)
							{
								return;
							}
						}
						Buffer.BlockCopy(BitConverter.GetBytes(item2.item.id), 0, base.player.equipment.state, 6, 2);
						base.player.equipment.state[16] = item2.item.quality;
						base.player.inventory.removeItem(page, index);
						if (item != null)
						{
							base.player.inventory.forceAddItem(item, true);
						}
						base.player.equipment.sendUpdateState();
						EffectManager.sendEffect(8, EffectManager.SMALL, base.transform.position);
						return;
					}
				}
				if (item != null)
				{
					base.player.inventory.forceAddItem(item, true);
				}
				base.player.equipment.state[6] = 0;
				base.player.equipment.state[7] = 0;
				base.player.equipment.sendUpdateState();
				EffectManager.sendEffect(8, EffectManager.SMALL, base.transform.position);
			}
		}

		[SteamCall]
		public void askAttachMagazine(CSteamID steamID, byte page, byte x, byte y)
		{
			if (base.channel.checkOwner(steamID) && Provider.isServer)
			{
				if (base.player.equipment.isBusy)
				{
					return;
				}
				if (this.isFired)
				{
					return;
				}
				if (this.isReloading || this.isHammering || this.needsRechamber)
				{
					return;
				}
				if (base.player.equipment.asset == null)
				{
					return;
				}
				Item item = null;
				if (this.thirdAttachments.magazineAsset != null && ((((ItemGunAsset)base.player.equipment.asset).action != EAction.Pump && ((ItemGunAsset)base.player.equipment.asset).action != EAction.Rail && ((ItemGunAsset)base.player.equipment.asset).action != EAction.String && ((ItemGunAsset)base.player.equipment.asset).action != EAction.Rocket && ((ItemGunAsset)base.player.equipment.asset).action != EAction.Break && !((ItemGunAsset)base.player.equipment.asset).deleteEmptyMagazines && !this.thirdAttachments.magazineAsset.deleteEmpty) || this.ammo > 0))
				{
					item = new Item(this.thirdAttachments.magazineID, base.player.equipment.state[10], base.player.equipment.state[17]);
				}
				if (page != 255)
				{
					byte index = base.player.inventory.getIndex(page, x, y);
					if (index != 255)
					{
						ItemJar item2 = base.player.inventory.getItem(page, index);
						ItemCaliberAsset itemCaliberAsset = (ItemCaliberAsset)Assets.find(EAssetType.ITEM, item2.item.id);
						if (itemCaliberAsset == null)
						{
							return;
						}
						if (itemCaliberAsset.calibers.Length != 0)
						{
							bool flag = false;
							byte b = 0;
							while ((int)b < itemCaliberAsset.calibers.Length)
							{
								byte b2 = 0;
								while ((int)b2 < ((ItemGunAsset)base.player.equipment.asset).magazineCalibers.Length)
								{
									if (itemCaliberAsset.calibers[(int)b] == ((ItemGunAsset)base.player.equipment.asset).magazineCalibers[(int)b2])
									{
										flag = true;
										break;
									}
									b2 += 1;
								}
								b += 1;
							}
							if (!flag)
							{
								return;
							}
						}
						bool flag2 = this.ammo == 0;
						this.ammo = item2.item.amount;
						Buffer.BlockCopy(BitConverter.GetBytes(item2.item.id), 0, base.player.equipment.state, 8, 2);
						base.player.equipment.state[10] = item2.item.amount;
						base.player.equipment.state[17] = item2.item.quality;
						base.player.inventory.removeItem(page, index);
						if (item != null)
						{
							base.player.inventory.forceAddItem(item, true);
						}
						base.player.equipment.sendUpdateState();
						base.channel.send("askReload", ESteamCall.ALL, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[]
						{
							flag2 && ((ItemGunAsset)base.player.equipment.asset).hammer != null
						});
						EffectManager.sendEffect(8, EffectManager.SMALL, base.transform.position);
						return;
					}
				}
				if (item != null)
				{
					base.player.inventory.forceAddItem(item, true);
				}
				base.player.equipment.state[8] = 0;
				base.player.equipment.state[9] = 0;
				base.player.equipment.state[10] = 0;
				base.player.equipment.sendUpdateState();
				base.channel.send("askReload", ESteamCall.ALL, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[]
				{
					((ItemGunAsset)base.player.equipment.asset).hammer != null
				});
				EffectManager.sendEffect(8, EffectManager.SMALL, base.transform.position);
			}
		}

		private void hammer()
		{
			base.player.equipment.isBusy = true;
			this.isHammering = true;
			this.startedHammer = Time.realtimeSinceStartup;
			float num = 1f;
			num += base.player.skills.mastery(0, 2) * 0.5f;
			if (this.thirdAttachments.magazineAsset != null)
			{
				num *= this.thirdAttachments.magazineAsset.speed;
			}
			base.player.playSound(((ItemGunAsset)base.player.equipment.asset).hammer, num, 0.05f);
			this.updateAnimationSpeeds(num);
			base.player.animator.play("Hammer", false);
		}

		[SteamCall]
		public void askReload(CSteamID steamID, bool newHammer)
		{
			if (base.channel.checkServer(steamID) && base.player.equipment.isEquipped)
			{
				if (this.isAiming)
				{
					this.isAiming = false;
					this.stopAim();
				}
				if (this.isAttaching)
				{
					this.isAttaching = false;
					this.stopAttach();
				}
				this.isShooting = false;
				this.isSprinting = false;
				base.player.equipment.isBusy = true;
				this.needsHammer = newHammer;
				this.isReloading = true;
				this.startedReload = Time.realtimeSinceStartup;
				float num = 1f;
				num += base.player.skills.mastery(0, 2) * 0.5f;
				if (this.thirdAttachments.magazineAsset != null)
				{
					num *= this.thirdAttachments.magazineAsset.speed;
				}
				base.player.playSound(((ItemGunAsset)base.player.equipment.asset).reload, num, 0.05f);
				this.updateAnimationSpeeds(num);
				base.player.animator.play("Reload", false);
				this.needsUnplace = true;
				this.needsReplace = true;
				if (((ItemGunAsset)base.player.equipment.asset).action == EAction.Break)
				{
					this.needsUnload = true;
				}
			}
		}

		[SteamCall]
		public void askAimStart(CSteamID steamID)
		{
			if (base.channel.checkServer(steamID) && base.player.equipment.isEquipped)
			{
				this.startAim();
			}
		}

		[SteamCall]
		public void askAimStop(CSteamID steamID)
		{
			if (base.channel.checkServer(steamID) && base.player.equipment.isEquipped)
			{
				this.stopAim();
			}
		}

		public override void startPrimary()
		{
			if (!this.isShooting && !this.isReloading && !this.isHammering && !this.isSprinting && !this.isAttaching && !this.needsRechamber && this.firemode != EFiremode.SAFETY && !base.player.equipment.isBusy)
			{
				if (((ItemGunAsset)base.player.equipment.asset).action == EAction.String)
				{
					if (this.thirdAttachments.nockHook != null || this.isAiming)
					{
						this.isShooting = true;
					}
				}
				else if (((ItemGunAsset)base.player.equipment.asset).action == EAction.Minigun)
				{
					if (this.isAiming)
					{
						this.isShooting = true;
					}
				}
				else
				{
					this.isShooting = true;
				}
			}
		}

		public override void stopPrimary()
		{
			if (this.isShooting)
			{
				this.isShooting = false;
			}
		}

		public override void startSecondary()
		{
			if (!this.isAiming && !this.isReloading && !this.isHammering && !this.isSprinting && !this.isAttaching && !this.needsRechamber)
			{
				this.isAiming = true;
				this.startAim();
				if (Provider.isServer)
				{
					base.channel.send("askAimStart", ESteamCall.NOT_OWNER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[0]);
				}
			}
		}

		public override void stopSecondary()
		{
			if (this.isAiming)
			{
				if (((ItemGunAsset)base.player.equipment.asset).action == EAction.Minigun && this.isShooting)
				{
					this.isShooting = false;
				}
				this.isAiming = false;
				this.stopAim();
				if (Provider.isServer)
				{
					base.channel.send("askAimStop", ESteamCall.NOT_OWNER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[0]);
				}
			}
		}

		public override bool canInspect
		{
			get
			{
				return !this.isShooting && !this.isReloading && !this.isHammering && !this.isSprinting && !this.isAttaching && !this.isAiming && !this.needsRechamber;
			}
		}

		public override void equip()
		{
			this.lastShot = float.MaxValue;
			if (!Dedicator.isDedicated)
			{
				if (base.channel.isOwner)
				{
					this.sound = base.player.gameObject.AddComponent<AudioSource>();
				}
				else
				{
					this.sound = base.player.equipment.thirdModel.gameObject.AddComponent<AudioSource>();
				}
				this.sound.clip = null;
				this.sound.spatialBlend = 1f;
				this.sound.rolloffMode = 2;
				this.sound.SetCustomCurve(0, (Resources.Load("Guns/Rolloff") as GameObject).GetComponent<AudioSource>().GetCustomCurve(0));
				this.sound.volume = 1f;
				this.sound.playOnAwake = false;
			}
			if (base.channel.isOwner)
			{
				this.firstAttachments = base.player.equipment.firstModel.gameObject.GetComponent<Attachments>();
				this.firstMinigunBarrel = this.firstAttachments.transform.FindChild("Model_1");
				if (this.firstAttachments.rope != null)
				{
					this.firstAttachments.rope.gameObject.SetActive(true);
				}
				if (this.firstAttachments.ejectHook != null && ((ItemGunAsset)base.player.equipment.asset).action != EAction.String && ((ItemGunAsset)base.player.equipment.asset).action != EAction.Rocket)
				{
					EffectAsset effectAsset = (EffectAsset)Assets.find(EAssetType.EFFECT, ((ItemGunAsset)base.player.equipment.asset).shell);
					if (effectAsset != null)
					{
						Transform transform = EffectManager.Instantiate(effectAsset.effect).transform;
						transform.name = "Emitter";
						transform.parent = this.firstAttachments.ejectHook;
						transform.localPosition = Vector3.zero;
						transform.localRotation = ((!base.channel.owner.hand) ? Quaternion.identity : Quaternion.Euler(0f, 180f, 0f));
						transform.tag = "Viewmodel";
						transform.gameObject.layer = LayerMasks.VIEWMODEL;
						this.firstShellEmitter = transform.GetComponent<ParticleSystem>();
					}
				}
				if (this.firstAttachments.barrelHook != null)
				{
					EffectAsset effectAsset2 = (EffectAsset)Assets.find(EAssetType.EFFECT, ((ItemGunAsset)base.player.equipment.asset).muzzle);
					if (effectAsset2 != null)
					{
						Transform transform2 = EffectManager.Instantiate(effectAsset2.effect).transform;
						transform2.name = "Emitter";
						transform2.parent = this.firstAttachments.barrelHook;
						transform2.localPosition = Vector3.zero;
						transform2.localRotation = Quaternion.identity;
						transform2.tag = "Viewmodel";
						transform2.gameObject.layer = LayerMasks.VIEWMODEL;
						this.firstMuzzleEmitter = transform2.GetComponent<ParticleSystem>();
						this.firstMuzzleEmitter.main.simulationSpace = 0;
					}
				}
				if (((ItemGunAsset)base.player.equipment.asset).isTurret)
				{
					base.player.animator.viewOffset = Vector3.up;
				}
				base.player.animator.driveOffset = Vector3.zero;
			}
			this.thirdAttachments = base.player.equipment.thirdModel.gameObject.GetComponent<Attachments>();
			this.thirdMinigunBarrel = this.thirdAttachments.transform.FindChild("Model_1");
			if (!Dedicator.isDedicated && this.thirdMinigunBarrel != null && ((ItemGunAsset)base.player.equipment.asset).action == EAction.Minigun)
			{
				if (base.channel.isOwner)
				{
					this.whir = base.player.gameObject.AddComponent<AudioSource>();
				}
				else
				{
					this.whir = base.player.equipment.thirdModel.gameObject.AddComponent<AudioSource>();
				}
				this.whir.clip = ((ItemGunAsset)base.player.equipment.asset).minigun;
				this.whir.spatialBlend = 1f;
				this.whir.rolloffMode = 1;
				this.whir.minDistance = 1f;
				this.whir.maxDistance = 16f;
				this.whir.volume = 0f;
				this.whir.playOnAwake = false;
				this.whir.loop = true;
				this.whir.Play();
			}
			if (this.thirdAttachments.ejectHook != null && ((ItemGunAsset)base.player.equipment.asset).action != EAction.String && ((ItemGunAsset)base.player.equipment.asset).action != EAction.Rocket)
			{
				EffectAsset effectAsset3 = (EffectAsset)Assets.find(EAssetType.EFFECT, ((ItemGunAsset)base.player.equipment.asset).shell);
				if (effectAsset3 != null)
				{
					Transform transform3 = EffectManager.Instantiate(effectAsset3.effect).transform;
					transform3.name = "Emitter";
					transform3.parent = ((!((ItemGunAsset)base.player.equipment.asset).isTurret) ? this.thirdAttachments.ejectHook : Level.effects);
					transform3.localPosition = Vector3.zero;
					transform3.localRotation = ((!base.channel.owner.hand) ? Quaternion.identity : Quaternion.Euler(0f, 180f, 0f));
					this.thirdShellEmitter = transform3.GetComponent<ParticleSystem>();
				}
			}
			if (this.thirdAttachments.barrelHook != null)
			{
				EffectAsset effectAsset4 = (EffectAsset)Assets.find(EAssetType.EFFECT, ((ItemGunAsset)base.player.equipment.asset).muzzle);
				if (effectAsset4 != null)
				{
					Transform transform4 = EffectManager.Instantiate(effectAsset4.effect).transform;
					transform4.name = "Emitter";
					transform4.parent = ((!((ItemGunAsset)base.player.equipment.asset).isTurret) ? this.thirdAttachments.barrelHook : Level.effects);
					transform4.localPosition = Vector3.zero;
					transform4.localRotation = Quaternion.identity;
					this.thirdMuzzleEmitter = transform4.GetComponent<ParticleSystem>();
				}
				if (base.channel.isOwner && effectAsset4 != null)
				{
					this.firstFakeLight = Object.Instantiate<GameObject>(effectAsset4.effect).transform;
					this.firstFakeLight.name = "Emitter";
					this.firstFakeLight.parent = Level.effects;
				}
			}
			this.ammo = base.player.equipment.state[10];
			this.firemode = (EFiremode)base.player.equipment.state[11];
			this.interact = (base.player.equipment.state[12] == 1);
			this.updateAttachments();
			this.startedReload = float.MaxValue;
			this.startedHammer = float.MaxValue;
			if (base.channel.isOwner)
			{
				if (this.firemode == EFiremode.SAFETY)
				{
					PlayerUI.message(EPlayerMessage.SAFETY, string.Empty);
				}
				else if (this.ammo == 0)
				{
					PlayerUI.message(EPlayerMessage.RELOAD, string.Empty);
				}
				base.player.animator.lockView();
				base.player.animator.getAnimationSample("Aim_Start", 1f);
				if (this.firstAttachments.sightHook != null)
				{
					this.sightOffset = base.player.animator.view.InverseTransformPoint(this.firstAttachments.sightHook.position);
				}
				else
				{
					this.sightOffset = Vector3.zero;
				}
				if (this.firstAttachments.viewHook != null)
				{
					this.viewOffset = base.player.animator.view.InverseTransformPoint(this.firstAttachments.viewHook.position);
				}
				else
				{
					this.viewOffset = Vector3.zero;
				}
				if (this.firstAttachments.aimHook != null)
				{
					Vector3 vector = this.firstAttachments.aimHook.localPosition + this.firstAttachments.aimHook.parent.localPosition;
					this.scopeOffset = new Vector3(vector.x, vector.z, vector.y);
				}
				else if (((ItemGunAsset)base.player.equipment.asset).hasSight)
				{
					this.scopeOffset = new Vector3(0f, 0.01f, -0.04f);
				}
				else
				{
					this.scopeOffset = Vector3.zero;
				}
				if (this.firstAttachments.reticuleHook != null)
				{
					this.reticuleOffset = this.firstAttachments.reticuleHook.localPosition;
				}
				else
				{
					this.reticuleOffset = Vector3.zero;
				}
				base.player.animator.unlockView();
				this.localization = Localization.read("/Player/Useable/PlayerUseableGun.dat");
				if (this.icons != null)
				{
					this.icons.unload();
				}
				this.icons = Bundles.getBundle("/Bundles/Textures/Player/Icons/Useable/PlayerUseableGun/PlayerUseableGun.unity3d");
				if (((ItemGunAsset)base.player.equipment.asset).hasSight)
				{
					this.sightButton = new SleekButtonIcon((Texture2D)this.icons.load("Sight"));
					this.sightButton.sizeOffset_X = 50;
					this.sightButton.sizeOffset_Y = 50;
					this.sightButton.onClickedButton = new ClickedButton(this.onClickedSightHookButton);
					PlayerUI.container.add(this.sightButton);
					this.sightButton.isVisible = false;
				}
				if (((ItemGunAsset)base.player.equipment.asset).hasTactical)
				{
					this.tacticalButton = new SleekButtonIcon((Texture2D)this.icons.load("Tactical"));
					this.tacticalButton.sizeOffset_X = 50;
					this.tacticalButton.sizeOffset_Y = 50;
					this.tacticalButton.onClickedButton = new ClickedButton(this.onClickedTacticalHookButton);
					PlayerUI.container.add(this.tacticalButton);
					this.tacticalButton.isVisible = false;
				}
				if (((ItemGunAsset)base.player.equipment.asset).hasGrip)
				{
					this.gripButton = new SleekButtonIcon((Texture2D)this.icons.load("Grip"));
					this.gripButton.sizeOffset_X = 50;
					this.gripButton.sizeOffset_Y = 50;
					this.gripButton.onClickedButton = new ClickedButton(this.onClickedGripHookButton);
					PlayerUI.container.add(this.gripButton);
					this.gripButton.isVisible = false;
				}
				if (((ItemGunAsset)base.player.equipment.asset).hasBarrel)
				{
					this.barrelButton = new SleekButtonIcon((Texture2D)this.icons.load("Barrel"));
					this.barrelButton.sizeOffset_X = 50;
					this.barrelButton.sizeOffset_Y = 50;
					this.barrelButton.onClickedButton = new ClickedButton(this.onClickedBarrelHookButton);
					PlayerUI.container.add(this.barrelButton);
					this.barrelButton.isVisible = false;
					this.barrelQualityLabel = new SleekLabel();
					this.barrelQualityLabel.positionOffset_Y = -30;
					this.barrelQualityLabel.positionScale_Y = 1f;
					this.barrelQualityLabel.sizeOffset_Y = 30;
					this.barrelQualityLabel.sizeScale_X = 1f;
					this.barrelQualityLabel.fontAlignment = 6;
					this.barrelQualityLabel.foregroundTint = ESleekTint.NONE;
					this.barrelQualityLabel.fontSize = 10;
					this.barrelButton.add(this.barrelQualityLabel);
					this.barrelQualityLabel.isVisible = false;
					this.barrelQualityImage = new SleekImageTexture();
					this.barrelQualityImage.positionOffset_X = -15;
					this.barrelQualityImage.positionOffset_Y = -15;
					this.barrelQualityImage.positionScale_X = 1f;
					this.barrelQualityImage.positionScale_Y = 1f;
					this.barrelQualityImage.sizeOffset_X = 10;
					this.barrelQualityImage.sizeOffset_Y = 10;
					this.barrelQualityImage.texture = (Texture2D)PlayerDashboardInventoryUI.icons.load("Quality_1");
					this.barrelButton.add(this.barrelQualityImage);
					this.barrelQualityImage.isVisible = false;
				}
				this.magazineButton = new SleekButtonIcon((Texture2D)this.icons.load("Magazine"));
				this.magazineButton.sizeOffset_X = 50;
				this.magazineButton.sizeOffset_Y = 50;
				this.magazineButton.onClickedButton = new ClickedButton(this.onClickedMagazineHookButton);
				PlayerUI.container.add(this.magazineButton);
				this.magazineButton.isVisible = false;
				this.magazineQualityLabel = new SleekLabel();
				this.magazineQualityLabel.positionOffset_Y = -30;
				this.magazineQualityLabel.positionScale_Y = 1f;
				this.magazineQualityLabel.sizeOffset_Y = 30;
				this.magazineQualityLabel.sizeScale_X = 1f;
				this.magazineQualityLabel.fontAlignment = 6;
				this.magazineQualityLabel.foregroundTint = ESleekTint.NONE;
				this.magazineQualityLabel.fontSize = 10;
				this.magazineButton.add(this.magazineQualityLabel);
				this.magazineQualityLabel.isVisible = false;
				this.magazineQualityImage = new SleekImageTexture();
				this.magazineQualityImage.positionOffset_X = -15;
				this.magazineQualityImage.positionOffset_Y = -15;
				this.magazineQualityImage.positionScale_X = 1f;
				this.magazineQualityImage.positionScale_Y = 1f;
				this.magazineQualityImage.sizeOffset_X = 10;
				this.magazineQualityImage.sizeOffset_Y = 10;
				this.magazineQualityImage.texture = (Texture2D)PlayerDashboardInventoryUI.icons.load("Quality_1");
				this.magazineButton.add(this.magazineQualityImage);
				this.magazineQualityImage.isVisible = false;
				this.icons.unload();
				this.infoBox = new SleekBox();
				this.infoBox.positionOffset_Y = -70;
				this.infoBox.positionScale_X = 0.7f;
				this.infoBox.positionScale_Y = 1f;
				this.infoBox.sizeOffset_Y = 70;
				this.infoBox.sizeScale_X = 0.3f;
				PlayerLifeUI.container.add(this.infoBox);
				this.ammoLabel = new SleekLabel();
				this.ammoLabel.sizeScale_X = 0.35f;
				this.ammoLabel.sizeScale_Y = 1f;
				this.ammoLabel.fontSize = 24;
				this.infoBox.add(this.ammoLabel);
				this.firemodeLabel = new SleekLabel();
				this.firemodeLabel.positionOffset_Y = 5;
				this.firemodeLabel.positionScale_X = 0.35f;
				this.firemodeLabel.sizeScale_X = 0.65f;
				this.firemodeLabel.sizeScale_Y = 0.5f;
				this.infoBox.add(this.firemodeLabel);
				this.attachLabel = new SleekLabel();
				this.attachLabel.positionOffset_Y = -5;
				this.attachLabel.positionScale_X = 0.35f;
				this.attachLabel.positionScale_Y = 0.5f;
				this.attachLabel.sizeScale_X = 0.65f;
				this.attachLabel.sizeScale_Y = 0.5f;
				this.infoBox.add(this.attachLabel);
				this.updateInfo();
			}
			base.player.animator.play("Equip", true);
			if (base.player.channel.isOwner)
			{
				PlayerUI.disableDot();
				PlayerStance stance = base.player.stance;
				stance.onStanceUpdated = (StanceUpdated)Delegate.Combine(stance.onStanceUpdated, new StanceUpdated(this.updateCrosshair));
				PlayerLook look = base.player.look;
				look.onPerspectiveUpdated = (PerspectiveUpdated)Delegate.Combine(look.onPerspectiveUpdated, new PerspectiveUpdated(this.onPerspectiveUpdated));
			}
			if ((base.channel.isOwner || Provider.isServer) && ((ItemGunAsset)base.player.equipment.asset).projectile == null)
			{
				this.bullets = new List<BulletInfo>();
			}
			this.aimAccuracy = 0;
			this.steadyAccuracy = 0u;
			this.canSteady = true;
			this.swayTime = Time.time;
		}

		public override void dequip()
		{
			if (this.infoBox != null)
			{
				if (this.sightButton != null)
				{
					PlayerUI.container.remove(this.sightButton);
				}
				if (this.tacticalButton != null)
				{
					PlayerUI.container.remove(this.tacticalButton);
				}
				if (this.gripButton != null)
				{
					PlayerUI.container.remove(this.gripButton);
				}
				if (this.barrelButton != null)
				{
					PlayerUI.container.remove(this.barrelButton);
				}
				if (this.magazineButton != null)
				{
					PlayerUI.container.remove(this.magazineButton);
				}
				if (this.rangeLabel != null)
				{
					this.rangeLabel.parent.remove(this.rangeLabel);
				}
				PlayerLifeUI.container.remove(this.infoBox);
			}
			base.player.updateSpot(false);
			if (base.channel.isOwner || Provider.isServer)
			{
				base.player.movement.multiplier = 1f;
			}
			if (base.channel.isOwner)
			{
				if (((ItemGunAsset)base.player.equipment.asset).isTurret)
				{
					base.player.animator.viewOffset = Vector3.zero;
				}
				base.player.animator.driveOffset = Vector3.zero;
				if (this.sound != null)
				{
					Object.Destroy(this.sound);
				}
				if (this.whir != null)
				{
					Object.Destroy(this.whir);
				}
				if (this.laser != null)
				{
					Object.Destroy(this.laser.gameObject);
				}
				if (this.isAiming)
				{
					this.stopAim();
				}
				PlayerUI.isLocked = false;
				if (this.isAttaching)
				{
					PlayerLifeUI.open();
				}
				if (base.player.movement.getVehicle() == null)
				{
					PlayerUI.enableDot();
				}
				PlayerUI.disableCrosshair();
				base.player.look.disableScope();
				PlayerStance stance = base.player.stance;
				stance.onStanceUpdated = (StanceUpdated)Delegate.Remove(stance.onStanceUpdated, new StanceUpdated(this.updateCrosshair));
				PlayerLook look = base.player.look;
				look.onPerspectiveUpdated = (PerspectiveUpdated)Delegate.Remove(look.onPerspectiveUpdated, new PerspectiveUpdated(this.onPerspectiveUpdated));
				if (this.firstFakeLight != null)
				{
					Object.Destroy(this.firstFakeLight.gameObject);
				}
				if (this.firstFakeLight_0 != null)
				{
					Object.Destroy(this.firstFakeLight_0.gameObject);
				}
				if (this.firstFakeLight_1 != null)
				{
					Object.Destroy(this.firstFakeLight_1.gameObject);
				}
			}
			if (this.tracerEmitter != null)
			{
				EffectManager.Destroy(this.tracerEmitter.gameObject);
				this.tracerEmitter = null;
			}
			if (this.firstMuzzleEmitter != null)
			{
				EffectManager.Destroy(this.firstMuzzleEmitter.gameObject);
				this.firstMuzzleEmitter = null;
			}
			if (this.firstShellEmitter != null)
			{
				EffectManager.Destroy(this.firstShellEmitter.gameObject);
				this.firstShellEmitter = null;
			}
			if (this.thirdMuzzleEmitter != null)
			{
				EffectManager.Destroy(this.thirdMuzzleEmitter.gameObject);
				this.thirdMuzzleEmitter = null;
			}
			if (this.thirdShellEmitter != null)
			{
				EffectManager.Destroy(this.thirdShellEmitter.gameObject);
				this.thirdShellEmitter = null;
			}
		}

		public override void tick()
		{
			if (base.channel.isOwner && this.firstAttachments.rope != null)
			{
				this.firstAttachments.rope.SetPosition(0, this.firstAttachments.leftHook.position);
				if (this.firstAttachments.nockHook != null)
				{
					if (this.firstAttachments.magazineModel != null && this.firstAttachments.magazineModel.gameObject.activeSelf)
					{
						this.firstAttachments.rope.SetPosition(1, this.firstAttachments.nockHook.position);
					}
					else
					{
						this.firstAttachments.rope.SetPosition(1, this.firstAttachments.restHook.position);
					}
				}
				else if (this.isAiming)
				{
					this.firstAttachments.rope.SetPosition(1, base.player.equipment.firstRightHook.position);
				}
				else if ((this.isAttaching || this.isSprinting || base.player.equipment.isInspecting) && this.firstAttachments.magazineModel != null && this.firstAttachments.magazineModel.gameObject.activeSelf)
				{
					this.firstAttachments.rope.SetPosition(1, this.firstAttachments.restHook.position);
				}
				else
				{
					this.firstAttachments.rope.SetPosition(1, this.firstAttachments.leftHook.position);
				}
				this.firstAttachments.rope.SetPosition(2, this.firstAttachments.rightHook.position);
			}
			if (!base.player.equipment.isEquipped)
			{
				return;
			}
			if ((double)(Time.realtimeSinceStartup - this.lastShot) > 0.05)
			{
				if (this.firstMuzzleEmitter != null)
				{
					this.firstMuzzleEmitter.GetComponent<Light>().enabled = false;
				}
				if (this.thirdMuzzleEmitter != null)
				{
					this.thirdMuzzleEmitter.GetComponent<Light>().enabled = false;
				}
				if (this.firstFakeLight != null)
				{
					this.firstFakeLight.GetComponent<Light>().enabled = false;
				}
			}
			if ((base.player.stance.stance == EPlayerStance.SPRINT && base.player.movement.isMoving) || this.firemode == EFiremode.SAFETY)
			{
				if (!this.isShooting && !this.isSprinting && !this.isReloading && !this.isHammering && !this.isAttaching && !this.isAiming && !this.needsRechamber)
				{
					this.isSprinting = true;
					base.player.animator.play("Sprint_Start", false);
				}
			}
			else if (this.isSprinting)
			{
				this.isSprinting = false;
				base.player.animator.play("Sprint_Stop", false);
			}
			if (base.channel.isOwner)
			{
				if (Input.GetKeyUp(ControlsSettings.attach) && this.isAttaching)
				{
					this.isAttaching = false;
					base.player.animator.play("Attach_Stop", false);
					this.stopAttach();
				}
				if (Input.GetKeyDown(ControlsSettings.tactical))
				{
					this.fireTacticalInput = true;
				}
				if (!PlayerUI.window.showCursor)
				{
					if (Input.GetKeyDown(ControlsSettings.attach) && !this.isShooting && !this.isAttaching && !this.isSprinting && !this.isReloading && !this.isHammering && !this.isAiming && !this.needsRechamber)
					{
						this.isAttaching = true;
						base.player.animator.play("Attach_Start", false);
						this.updateAttach();
						this.startAttach();
					}
					if (Input.GetKeyDown(ControlsSettings.reload) && !this.isShooting && !this.isReloading && !this.isHammering && !this.isSprinting && !this.isAttaching && !this.isAiming && !this.needsRechamber)
					{
						this.magazineSearch = base.player.inventory.search(EItemType.MAGAZINE, ((ItemGunAsset)base.player.equipment.asset).magazineCalibers);
						if (this.magazineSearch.Count > 0)
						{
							byte b = 0;
							byte b2 = byte.MaxValue;
							byte b3 = 0;
							while ((int)b3 < this.magazineSearch.Count)
							{
								if (this.magazineSearch[(int)b3].jar.item.amount > b)
								{
									b = this.magazineSearch[(int)b3].jar.item.amount;
									b2 = b3;
								}
								b3 += 1;
							}
							if (b2 != 255)
							{
								base.channel.send("askAttachMagazine", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[]
								{
									this.magazineSearch[(int)b2].page,
									this.magazineSearch[(int)b2].jar.x,
									this.magazineSearch[(int)b2].jar.y
								});
							}
						}
					}
					if (Input.GetKeyDown(ControlsSettings.firemode) && !this.isAiming)
					{
						if (this.firemode == EFiremode.SAFETY)
						{
							if (((ItemGunAsset)base.player.equipment.asset).hasSemi)
							{
								base.channel.send("askFiremode", ESteamCall.SERVER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
								{
									1
								});
							}
							else if (((ItemGunAsset)base.player.equipment.asset).hasAuto)
							{
								base.channel.send("askFiremode", ESteamCall.SERVER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
								{
									2
								});
							}
							else if (((ItemGunAsset)base.player.equipment.asset).hasBurst)
							{
								base.channel.send("askFiremode", ESteamCall.SERVER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
								{
									3
								});
							}
							PlayerUI.message(EPlayerMessage.NONE, string.Empty);
						}
						else if (this.firemode == EFiremode.SEMI)
						{
							if (((ItemGunAsset)base.player.equipment.asset).hasAuto)
							{
								base.channel.send("askFiremode", ESteamCall.SERVER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
								{
									2
								});
							}
							else if (((ItemGunAsset)base.player.equipment.asset).hasBurst)
							{
								base.channel.send("askFiremode", ESteamCall.SERVER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
								{
									3
								});
							}
							else if (((ItemGunAsset)base.player.equipment.asset).hasSafety)
							{
								base.channel.send("askFiremode", ESteamCall.SERVER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
								{
									0
								});
								PlayerUI.message(EPlayerMessage.SAFETY, string.Empty);
							}
						}
						else if (this.firemode == EFiremode.AUTO)
						{
							if (((ItemGunAsset)base.player.equipment.asset).hasBurst)
							{
								base.channel.send("askFiremode", ESteamCall.SERVER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
								{
									3
								});
							}
							else if (((ItemGunAsset)base.player.equipment.asset).hasSafety)
							{
								base.channel.send("askFiremode", ESteamCall.SERVER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
								{
									0
								});
								PlayerUI.message(EPlayerMessage.SAFETY, string.Empty);
							}
							else if (((ItemGunAsset)base.player.equipment.asset).hasSemi)
							{
								base.channel.send("askFiremode", ESteamCall.SERVER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
								{
									1
								});
							}
						}
						else if (this.firemode == EFiremode.BURST)
						{
							if (((ItemGunAsset)base.player.equipment.asset).hasSafety)
							{
								base.channel.send("askFiremode", ESteamCall.SERVER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
								{
									0
								});
								PlayerUI.message(EPlayerMessage.SAFETY, string.Empty);
							}
							else if (((ItemGunAsset)base.player.equipment.asset).hasSemi)
							{
								base.channel.send("askFiremode", ESteamCall.SERVER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
								{
									1
								});
							}
							else if (((ItemGunAsset)base.player.equipment.asset).hasAuto)
							{
								base.channel.send("askFiremode", ESteamCall.SERVER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
								{
									2
								});
							}
						}
					}
				}
				if (this.isAttaching)
				{
					if (this.sightButton != null)
					{
						if (base.player.look.perspective == EPlayerPerspective.FIRST && !((ItemGunAsset)base.player.equipment.asset).isTurret)
						{
							Vector2 vector = base.player.animator.view.GetComponent<Camera>().WorldToScreenPoint(this.firstAttachments.sightHook.position + this.firstAttachments.sightHook.up * 0.05f + this.firstAttachments.sightHook.forward * 0.05f);
							vector.x -= PlayerUI.window.frame.x;
							vector.y -= PlayerUI.window.frame.y;
							this.sightButton.positionOffset_X = (int)(vector.x - 25f);
							this.sightButton.positionOffset_Y = (int)((float)Screen.height - vector.y - 25f);
							this.sightButton.positionScale_X = 0f;
							this.sightButton.positionScale_Y = 0f;
						}
						else
						{
							this.sightButton.positionOffset_X = -25;
							this.sightButton.positionOffset_Y = -25;
							this.sightButton.positionScale_X = 0.667f;
							this.sightButton.positionScale_Y = 0.75f;
						}
					}
					if (this.tacticalButton != null)
					{
						if (base.player.look.perspective == EPlayerPerspective.FIRST && !((ItemGunAsset)base.player.equipment.asset).isTurret)
						{
							Vector2 vector2 = base.player.animator.view.GetComponent<Camera>().WorldToScreenPoint(this.firstAttachments.tacticalHook.position);
							vector2.x -= PlayerUI.window.frame.x;
							vector2.y -= PlayerUI.window.frame.y;
							this.tacticalButton.positionOffset_X = (int)(vector2.x - 25f);
							this.tacticalButton.positionOffset_Y = (int)((float)Screen.height - vector2.y - 25f);
							this.tacticalButton.positionScale_X = 0f;
							this.tacticalButton.positionScale_Y = 0f;
						}
						else
						{
							this.tacticalButton.positionOffset_X = -25;
							this.tacticalButton.positionOffset_Y = -25;
							this.tacticalButton.positionScale_X = 0.5f;
							this.tacticalButton.positionScale_Y = 0.25f;
						}
					}
					if (this.gripButton != null)
					{
						if (base.player.look.perspective == EPlayerPerspective.FIRST && !((ItemGunAsset)base.player.equipment.asset).isTurret)
						{
							Vector2 vector3 = base.player.animator.view.GetComponent<Camera>().WorldToScreenPoint(this.firstAttachments.gripHook.position + this.firstAttachments.gripHook.forward * -0.05f);
							vector3.x -= PlayerUI.window.frame.x;
							vector3.y -= PlayerUI.window.frame.y;
							this.gripButton.positionOffset_X = (int)(vector3.x - 25f);
							this.gripButton.positionOffset_Y = (int)((float)Screen.height - vector3.y - 25f);
							this.gripButton.positionScale_X = 0f;
							this.gripButton.positionScale_Y = 0f;
						}
						else
						{
							this.gripButton.positionOffset_X = -25;
							this.gripButton.positionOffset_Y = -25;
							this.gripButton.positionScale_X = 0.75f;
							this.gripButton.positionScale_Y = 0.25f;
						}
					}
					if (this.barrelButton != null)
					{
						if (base.player.look.perspective == EPlayerPerspective.FIRST && !((ItemGunAsset)base.player.equipment.asset).isTurret)
						{
							Vector2 vector4 = base.player.animator.view.GetComponent<Camera>().WorldToScreenPoint(this.firstAttachments.barrelHook.position + this.firstAttachments.barrelHook.up * 0.05f);
							vector4.x -= PlayerUI.window.frame.x;
							vector4.y -= PlayerUI.window.frame.y;
							this.barrelButton.positionOffset_X = (int)(vector4.x - 25f);
							this.barrelButton.positionOffset_Y = (int)((float)Screen.height - vector4.y - 25f);
							this.barrelButton.positionScale_X = 0f;
							this.barrelButton.positionScale_Y = 0f;
						}
						else
						{
							this.barrelButton.positionOffset_X = -25;
							this.barrelButton.positionOffset_Y = -25;
							this.barrelButton.positionScale_X = 0.25f;
							this.barrelButton.positionScale_Y = 0.25f;
						}
					}
					if (this.magazineButton != null)
					{
						if (base.player.look.perspective == EPlayerPerspective.FIRST && !((ItemGunAsset)base.player.equipment.asset).isTurret)
						{
							Vector2 vector5 = base.player.animator.view.GetComponent<Camera>().WorldToScreenPoint(this.firstAttachments.magazineHook.position + this.firstAttachments.magazineHook.forward * -0.1f);
							vector5.x -= PlayerUI.window.frame.x;
							vector5.y -= PlayerUI.window.frame.y;
							this.magazineButton.positionOffset_X = (int)(vector5.x - 25f);
							this.magazineButton.positionOffset_Y = (int)((float)Screen.height - vector5.y - 25f);
							this.magazineButton.positionScale_X = 0f;
							this.magazineButton.positionScale_Y = 0f;
						}
						else
						{
							this.magazineButton.positionOffset_X = -25;
							this.magazineButton.positionOffset_Y = -25;
							this.magazineButton.positionScale_X = 0.334f;
							this.magazineButton.positionScale_Y = 0.75f;
						}
					}
				}
				if (this.rangeLabel != null)
				{
					if (PlayerLifeUI.scopeOverlay.isVisible)
					{
						this.rangeLabel.positionOffset_X = -300;
						this.rangeLabel.positionOffset_Y = 100;
						this.rangeLabel.positionScale_X = 0.5f;
						this.rangeLabel.positionScale_Y = 0.5f;
						this.rangeLabel.fontAlignment = 2;
						if (this.rangeLabel.parent != PlayerUI.window)
						{
							PlayerLifeUI.container.remove(this.rangeLabel);
							PlayerUI.window.add(this.rangeLabel);
						}
					}
					else
					{
						Vector2 vector6;
						if (base.player.look.perspective == EPlayerPerspective.FIRST)
						{
							vector6 = base.player.animator.view.GetComponent<Camera>().WorldToScreenPoint(this.firstAttachments.lightHook.position);
						}
						else
						{
							vector6 = MainCamera.instance.WorldToScreenPoint(this.thirdAttachments.lightHook.position);
						}
						vector6.x -= PlayerUI.window.frame.x;
						vector6.y -= PlayerUI.window.frame.y;
						this.rangeLabel.positionOffset_X = (int)(vector6.x - 100f);
						this.rangeLabel.positionOffset_Y = (int)((float)Screen.height - vector6.y - 15f);
						this.rangeLabel.positionScale_X = 0f;
						this.rangeLabel.positionScale_Y = 0f;
						this.rangeLabel.fontAlignment = 4;
						if (this.rangeLabel.parent != PlayerLifeUI.container)
						{
							PlayerUI.window.remove(this.rangeLabel);
							PlayerLifeUI.container.add(this.rangeLabel);
						}
					}
					this.rangeLabel.isVisible = true;
				}
			}
			if (this.needsRechamber && Time.realtimeSinceStartup - this.lastShot > 0.25f && !this.isAiming)
			{
				this.needsRechamber = false;
				this.lastRechamber = Time.realtimeSinceStartup;
				this.needsEject = true;
				this.hammer();
			}
			if (this.needsEject && Time.realtimeSinceStartup - this.lastRechamber > 0.45f)
			{
				this.needsEject = false;
				if (this.firstShellEmitter != null && base.player.look.perspective == EPlayerPerspective.FIRST && !((ItemGunAsset)base.player.equipment.asset).isTurret)
				{
					this.firstShellEmitter.Emit(1);
				}
				if (this.thirdShellEmitter != null && (!base.channel.isOwner || base.player.look.perspective == EPlayerPerspective.THIRD || ((ItemGunAsset)base.player.equipment.asset).isTurret))
				{
					this.thirdShellEmitter.Emit(1);
				}
			}
			if (this.needsUnload && Time.realtimeSinceStartup - this.startedReload > 0.5f)
			{
				this.needsUnload = false;
				if (this.firstShellEmitter != null && base.player.look.perspective == EPlayerPerspective.FIRST && !((ItemGunAsset)base.player.equipment.asset).isTurret)
				{
					this.firstShellEmitter.Emit((int)((ItemGunAsset)base.player.equipment.asset).ammoMax);
				}
				if (this.thirdShellEmitter != null && (!base.channel.isOwner || base.player.look.perspective == EPlayerPerspective.THIRD || ((ItemGunAsset)base.player.equipment.asset).isTurret))
				{
					this.thirdShellEmitter.Emit((int)((ItemGunAsset)base.player.equipment.asset).ammoMax);
				}
			}
			if (this.needsUnplace && Time.realtimeSinceStartup - this.startedReload > this.reloadTime * ((ItemGunAsset)base.player.equipment.asset).unplace)
			{
				this.needsUnplace = false;
				if (base.channel.isOwner && this.firstAttachments.magazineModel != null)
				{
					this.firstAttachments.magazineModel.gameObject.SetActive(false);
				}
				if (this.thirdAttachments.magazineModel != null)
				{
					this.thirdAttachments.magazineModel.gameObject.SetActive(false);
				}
			}
			if (this.needsReplace && Time.realtimeSinceStartup - this.startedReload > this.reloadTime * ((ItemGunAsset)base.player.equipment.asset).replace)
			{
				this.needsReplace = false;
				if (base.channel.isOwner && this.firstAttachments.magazineModel != null)
				{
					this.firstAttachments.magazineModel.gameObject.SetActive(true);
				}
				if (this.thirdAttachments.magazineModel != null)
				{
					this.thirdAttachments.magazineModel.gameObject.SetActive(true);
				}
			}
			if (this.isReloading && Time.realtimeSinceStartup - this.startedReload > this.reloadTime)
			{
				this.isReloading = false;
				if (this.needsHammer)
				{
					this.hammer();
				}
				else
				{
					base.player.equipment.isBusy = false;
				}
			}
			if (this.isHammering && Time.realtimeSinceStartup - this.startedHammer > this.hammerTime)
			{
				this.isHammering = false;
				base.player.equipment.isBusy = false;
			}
		}

		public override void simulate(uint simulation, bool inputSteady)
		{
			if (!this.canSteady && !inputSteady && base.player.life.oxygen > 10)
			{
				this.canSteady = true;
			}
			if (this.isAiming && this.thirdAttachments.sightAsset != null && this.thirdAttachments.sightAsset.zoom < 45f && base.player.life.oxygen > 0 && this.canSteady && inputSteady)
			{
				if (this.steadyAccuracy < 4u)
				{
					this.steadyAccuracy += 1u;
				}
				base.player.life.askSuffocate(5 - base.player.skills.skills[0][5].level / 2);
				if (base.player.life.oxygen == 0)
				{
					this.canSteady = false;
				}
			}
			else if (this.steadyAccuracy > 0u)
			{
				this.steadyAccuracy -= 1u;
			}
			if (base.channel.isOwner && base.player.equipment.isEquipped && this.fireTacticalInput)
			{
				if (!this.isReloading && !this.isHammering && !this.needsRechamber && this.thirdAttachments.tacticalAsset != null)
				{
					if (this.thirdAttachments.tacticalAsset.isMelee)
					{
						if (!this.isSprinting && (!base.player.movement.isSafe || !base.player.movement.isSafeInfo.noWeapons) && this.firemode != EFiremode.SAFETY)
						{
							if (!Provider.isServer)
							{
								this.isJabbing = true;
							}
							base.player.input.keys[8] = true;
						}
					}
					else if (this.thirdAttachments.tacticalAsset.isLight || this.thirdAttachments.tacticalAsset.isLaser || this.thirdAttachments.tacticalAsset.isRangefinder)
					{
						base.player.input.keys[8] = true;
					}
				}
				this.fireTacticalInput = false;
			}
			if (Provider.isServer && base.player.input.keys[8])
			{
				this.askInteractGun();
			}
		}

		private void tockShoot(uint clock)
		{
			if (this.firemode == EFiremode.SAFETY)
			{
				this.bursts = 0;
				this.isShooting = false;
				return;
			}
			if (this.isReloading || this.isHammering || base.player.stance.stance == EPlayerStance.SPRINT || this.isAttaching)
			{
				this.bursts = 0;
				this.isShooting = false;
				return;
			}
			if (this.firemode == EFiremode.SEMI)
			{
				this.isShooting = false;
			}
			if (this.firemode == EFiremode.BURST && this.isShooting)
			{
				this.bursts += ((ItemGunAsset)base.player.equipment.asset).bursts;
				this.isShooting = false;
			}
			if ((ulong)(clock - this.lastFire) > (ulong)((long)(((ItemGunAsset)base.player.equipment.asset).firerate - ((this.thirdAttachments.tacticalAsset == null) ? 0 : this.thirdAttachments.tacticalAsset.firerate))))
			{
				if (this.bursts > 0)
				{
					this.bursts--;
				}
				if (this.ammo > 0)
				{
					this.isFired = true;
					this.lastFire = clock;
					base.player.equipment.isBusy = true;
					this.fire();
				}
				else
				{
					if (Provider.isServer)
					{
						EffectManager.sendEffect(8, EffectManager.SMALL, base.transform.position);
					}
					this.bursts = 0;
					this.isShooting = false;
				}
			}
		}

		private void tockJab(uint clock)
		{
			this.isJabbing = false;
			if (clock - this.lastJab > 25u)
			{
				this.lastJab = clock;
				this.jab();
			}
		}

		public override void tock(uint clock)
		{
			if (this.isShooting || this.bursts > 0)
			{
				this.tockShoot(clock);
			}
			if (this.isJabbing)
			{
				this.tockJab(clock);
			}
			this.ballistics();
			if (this.isFired && clock - this.lastFire > 6u)
			{
				this.isFired = false;
				base.player.equipment.isBusy = false;
			}
			if (this.isAiming)
			{
				if (this.aimAccuracy < 10)
				{
					this.aimAccuracy += 1;
				}
			}
			else if (this.aimAccuracy > 0)
			{
				this.aimAccuracy -= 1;
			}
		}

		public override void updateState(byte[] newState)
		{
			this.ammo = newState[10];
			this.firemode = (EFiremode)newState[11];
			this.interact = (newState[12] == 1);
			if (base.channel.isOwner)
			{
				this.firstAttachments.updateAttachments(newState, true);
			}
			this.thirdAttachments.updateAttachments(newState, false);
			this.updateAttachments();
			if (base.channel.isOwner)
			{
				if (this.firstAttachments.aimHook != null)
				{
					Vector3 vector = this.firstAttachments.aimHook.localPosition + this.firstAttachments.aimHook.parent.localPosition;
					this.scopeOffset = new Vector3(vector.x, vector.z, vector.y);
				}
				else if (((ItemGunAsset)base.player.equipment.asset).hasSight)
				{
					this.scopeOffset = new Vector3(0f, 0.01f, -0.04f);
				}
				else
				{
					this.scopeOffset = Vector3.zero;
				}
				if (this.firstAttachments.reticuleHook != null)
				{
					this.reticuleOffset = this.firstAttachments.reticuleHook.localPosition;
				}
				else
				{
					this.reticuleOffset = Vector3.zero;
				}
			}
			if (this.infoBox != null)
			{
				if (this.isAttaching)
				{
					this.updateAttach();
				}
				this.updateInfo();
			}
		}

		private void updateAnimationSpeeds(float speed)
		{
			base.player.animator.setAnimationSpeed("Reload", speed);
			this.reloadTime = base.player.animator.getAnimationLength("Reload");
			this.reloadTime = Mathf.Max(this.reloadTime, ((ItemGunAsset)base.player.equipment.asset).reloadTime * speed);
			base.player.animator.setAnimationSpeed("Hammer", speed);
			this.hammerTime = base.player.animator.getAnimationLength("Hammer");
			base.player.animator.setAnimationSpeed("Scope", speed);
			this.hammerTime = Mathf.Max(this.hammerTime, ((ItemGunAsset)base.player.equipment.asset).hammerTime * speed);
		}

		private void updateAttachments()
		{
			if (base.channel.isOwner)
			{
				if (this.firstAttachments.tacticalAsset != null)
				{
					if (this.firstAttachments.tacticalAsset.isLaser)
					{
						if (!this.wasLaser)
						{
							PlayerUI.message(EPlayerMessage.LASER, string.Empty);
						}
						this.wasLaser = true;
					}
					else
					{
						this.wasLaser = false;
					}
					if (this.firstAttachments.tacticalAsset.isLight)
					{
						if (!this.wasLight)
						{
							PlayerUI.message(EPlayerMessage.LIGHT, string.Empty);
						}
						this.wasLight = true;
					}
					else
					{
						this.wasLight = false;
					}
					if (this.firstAttachments.tacticalAsset.isRangefinder)
					{
						if (!this.wasRange)
						{
							PlayerUI.message(EPlayerMessage.RANGEFINDER, string.Empty);
						}
						this.wasRange = true;
					}
					else
					{
						this.wasRange = false;
					}
					if (this.firstAttachments.tacticalAsset.isMelee)
					{
						if (!this.wasBayonet)
						{
							PlayerUI.message(EPlayerMessage.BAYONET, string.Empty);
						}
						this.wasBayonet = true;
					}
					else
					{
						this.wasBayonet = false;
					}
				}
				else
				{
					this.wasLaser = false;
					this.wasLight = false;
					this.wasRange = false;
					this.wasBayonet = false;
				}
				if (this.firstAttachments.tacticalAsset != null && this.firstAttachments.tacticalAsset.isLaser && this.interact)
				{
					if (this.laser == null)
					{
						this.laser = ((GameObject)Object.Instantiate(Resources.Load("Guns/Laser"))).transform;
						this.laser.name = "Laser";
						this.laser.parent = Level.effects;
						this.laser.position = Vector3.zero;
						this.laser.rotation = Quaternion.identity;
					}
				}
				else if (this.laser != null)
				{
					Object.Destroy(this.laser.gameObject);
					this.laser = null;
				}
				if (this.firstAttachments.tacticalAsset != null && this.firstAttachments.tacticalAsset.isRangefinder && this.interact)
				{
					if (this.rangeLabel == null)
					{
						this.rangeLabel = new SleekLabel();
						this.rangeLabel.sizeOffset_X = 200;
						this.rangeLabel.sizeOffset_Y = 30;
						this.rangeLabel.foregroundTint = ESleekTint.NONE;
						PlayerLifeUI.container.add(this.rangeLabel);
						this.rangeLabel.isVisible = false;
					}
				}
				else if (this.rangeLabel != null)
				{
					this.rangeLabel.parent.remove(this.rangeLabel);
					this.rangeLabel = null;
				}
				if (this.firstFakeLight_0 != null)
				{
					Object.Destroy(this.firstFakeLight_0.gameObject);
					this.firstFakeLight_0 = null;
				}
				if (this.thirdAttachments.lightHook != null)
				{
					Transform transform = this.thirdAttachments.lightHook.FindChild("Light");
					if (transform != null)
					{
						this.firstFakeLight_0 = Object.Instantiate<GameObject>(transform.gameObject).transform;
						this.firstFakeLight_0.name = "Emitter";
						this.firstFakeLight_0.parent = Level.effects;
					}
				}
				if (this.firstFakeLight_1 != null)
				{
					Object.Destroy(this.firstFakeLight_1.gameObject);
					this.firstFakeLight_1 = null;
				}
				if (this.thirdAttachments.light2Hook != null)
				{
					Transform transform2 = this.thirdAttachments.light2Hook.FindChild("Light");
					if (transform2 != null)
					{
						this.firstFakeLight_1 = Object.Instantiate<GameObject>(transform2.gameObject).transform;
						this.firstFakeLight_1.name = "Emitter";
						this.firstFakeLight_1.parent = Level.effects;
					}
				}
			}
			if (this.firstMuzzleEmitter != null)
			{
				if (this.firstAttachments.barrelModel != null)
				{
					this.firstMuzzleEmitter.transform.localPosition = Vector3.up * 0.25f;
				}
				else
				{
					this.firstMuzzleEmitter.transform.localPosition = Vector3.zero;
				}
			}
			if (this.thirdMuzzleEmitter != null)
			{
				if (this.thirdAttachments.barrelModel != null)
				{
					this.thirdMuzzleEmitter.transform.localPosition = Vector3.up * 0.25f;
				}
				else
				{
					this.thirdMuzzleEmitter.transform.localPosition = Vector3.zero;
				}
			}
			if (this.thirdAttachments.magazineAsset != null && this.thirdAttachments.magazineAsset.tracer != this.tracerID)
			{
				if (this.tracerEmitter != null)
				{
					EffectManager.Destroy(this.tracerEmitter.gameObject);
					this.tracerEmitter = null;
				}
				this.tracerID = this.thirdAttachments.magazineAsset.tracer;
				EffectAsset effectAsset = (EffectAsset)Assets.find(EAssetType.EFFECT, this.thirdAttachments.magazineAsset.tracer);
				if (effectAsset != null)
				{
					Transform transform3 = EffectManager.Instantiate(effectAsset.effect).transform;
					transform3.name = "Tracer";
					transform3.parent = Level.effects;
					transform3.localPosition = Vector3.zero;
					transform3.localRotation = Quaternion.identity;
					this.tracerEmitter = transform3.GetComponent<ParticleSystem>();
				}
			}
			if (this.isReloading)
			{
				if (base.channel.isOwner && this.firstAttachments.magazineModel != null)
				{
					this.firstAttachments.magazineModel.gameObject.SetActive(this.needsUnplace || !this.needsReplace);
				}
				if (this.thirdAttachments.magazineModel != null)
				{
					this.thirdAttachments.magazineModel.gameObject.SetActive(this.needsUnplace || !this.needsReplace);
				}
			}
			if (!Dedicator.isDedicated && this.thirdAttachments.tacticalAsset != null)
			{
				if (this.thirdAttachments.tacticalAsset.isLight || this.thirdAttachments.tacticalAsset.isLaser)
				{
					if (base.channel.isOwner && this.firstAttachments.lightHook != null)
					{
						this.firstAttachments.lightHook.gameObject.SetActive(this.interact);
					}
					if (this.thirdAttachments.lightHook != null)
					{
						this.thirdAttachments.lightHook.gameObject.SetActive(this.interact);
					}
					if (this.firstFakeLight_0 != null)
					{
						this.firstFakeLight_0.gameObject.SetActive(this.interact);
					}
				}
				else if (this.thirdAttachments.tacticalAsset.isRangefinder)
				{
					if (base.channel.isOwner && this.firstAttachments.lightHook != null)
					{
						this.firstAttachments.lightHook.gameObject.SetActive(this.inRange && this.interact);
						this.firstAttachments.light2Hook.gameObject.SetActive(!this.inRange && this.interact);
					}
					if (base.channel.isOwner && this.thirdAttachments.lightHook != null)
					{
						this.thirdAttachments.lightHook.gameObject.SetActive(this.inRange && this.interact);
						this.thirdAttachments.light2Hook.gameObject.SetActive(!this.inRange && this.interact);
					}
					if (this.firstFakeLight_0 != null)
					{
						this.firstFakeLight_0.gameObject.SetActive(this.inRange && this.interact);
					}
					if (this.firstFakeLight_1 != null)
					{
						this.firstFakeLight_1.gameObject.SetActive(!this.inRange && this.interact);
					}
				}
			}
			base.player.updateSpot(this.thirdAttachments.tacticalAsset != null && this.thirdAttachments.tacticalAsset.isLight && this.interact);
			if (base.channel.isOwner)
			{
				if (this.firstAttachments.sightAsset != null)
				{
					this.zoom = this.firstAttachments.sightAsset.zoom;
					if (this.firstAttachments.scopeHook != null)
					{
						base.player.look.enableScope(this.zoom, this.firstAttachments.sightAsset.vision);
						if (GraphicsSettings.scopeQuality == EGraphicQuality.OFF)
						{
							this.firstAttachments.scopeHook.GetComponent<Renderer>().enabled = false;
						}
						else
						{
							this.firstAttachments.scopeHook.GetComponent<Renderer>().enabled = true;
						}
						this.firstAttachments.scopeHook.GetComponent<Renderer>().sharedMaterial.mainTexture = base.player.look.scopeCamera.targetTexture;
						this.firstAttachments.scopeHook.gameObject.SetActive(true);
						if (base.channel.owner.hand)
						{
							Vector3 localScale = this.firstAttachments.scopeHook.localScale;
							localScale.x *= -1f;
							this.firstAttachments.scopeHook.localScale = localScale;
						}
					}
					else
					{
						base.player.look.disableScope();
					}
				}
				else
				{
					this.zoom = 90f;
					base.player.look.disableScope();
				}
				this.updateCrosshair();
			}
		}

		private void updateCrosshair()
		{
			this.crosshair = ((ItemGunAsset)base.player.equipment.asset).spreadHip;
			this.crosshair *= 1f - base.player.skills.mastery(0, 1) * 0.5f;
			if (this.thirdAttachments.tacticalAsset != null && this.interact)
			{
				this.crosshair *= this.thirdAttachments.tacticalAsset.spread;
			}
			if (this.thirdAttachments.gripAsset != null && (!this.thirdAttachments.gripAsset.isBipod || base.player.stance.stance == EPlayerStance.PRONE))
			{
				this.crosshair *= this.thirdAttachments.gripAsset.spread;
			}
			if (this.thirdAttachments.barrelAsset != null)
			{
				this.crosshair *= this.thirdAttachments.barrelAsset.spread;
			}
			if (this.thirdAttachments.magazineAsset != null)
			{
				this.crosshair *= this.thirdAttachments.magazineAsset.spread;
			}
			if (base.player.stance.stance == EPlayerStance.SPRINT)
			{
				this.crosshair *= UseableGun.SPREAD_SPRINT;
			}
			else if (base.player.stance.stance == EPlayerStance.CROUCH)
			{
				this.crosshair *= UseableGun.SPREAD_CROUCH;
			}
			else if (base.player.stance.stance == EPlayerStance.PRONE)
			{
				this.crosshair *= UseableGun.SPREAD_PRONE;
			}
			if (this.isAiming)
			{
				if (base.player.look.perspective == EPlayerPerspective.FIRST)
				{
					this.crosshair *= ((ItemGunAsset)base.player.equipment.asset).spreadAim + 0.2f;
				}
				else
				{
					this.crosshair *= 0.5f;
				}
			}
			PlayerUI.updateCrosshair(this.crosshair);
			if ((!((ItemGunAsset)base.player.equipment.asset).isTurret && ((ItemGunAsset)base.player.equipment.asset).action != EAction.Minigun && ((this.isAiming && base.player.look.perspective == EPlayerPerspective.FIRST && (((ItemGunAsset)base.player.equipment.asset).action != EAction.String || this.thirdAttachments.sightHook != null)) || this.isAttaching)) || (base.player.movement.getVehicle() != null && base.player.look.perspective != EPlayerPerspective.FIRST))
			{
				PlayerUI.disableCrosshair();
			}
			else
			{
				PlayerUI.enableCrosshair();
			}
		}

		private void updateAttach()
		{
			if (this.sightButton != null)
			{
				this.sightSearch = base.player.inventory.search(EItemType.SIGHT, ((ItemGunAsset)base.player.equipment.asset).attachmentCalibers);
				if (this.sightJars != null)
				{
					this.sightButton.remove(this.sightJars);
				}
				this.sightJars = new SleekJars(100f, this.sightSearch);
				this.sightJars.sizeScale_X = 1f;
				this.sightJars.sizeScale_Y = 1f;
				this.sightJars.onClickedJar = new ClickedJar(this.onClickedSightJar);
				this.sightButton.add(this.sightJars);
				if (this.thirdAttachments.sightAsset != null)
				{
					this.sightButton.backgroundTint = ESleekTint.NONE;
					this.sightButton.foregroundTint = ESleekTint.NONE;
					this.sightButton.backgroundColor = ItemTool.getRarityColorUI(this.thirdAttachments.sightAsset.rarity);
					this.sightButton.foregroundColor = this.sightButton.backgroundColor;
					this.sightButton.tooltip = this.thirdAttachments.sightAsset.itemName;
					this.sightButton.iconImage.backgroundColor = this.sightButton.backgroundColor;
					this.sightButton.iconImage.backgroundTint = ESleekTint.NONE;
				}
				else
				{
					this.sightButton.backgroundTint = ESleekTint.BACKGROUND;
					this.sightButton.foregroundTint = ESleekTint.FOREGROUND;
					this.sightButton.backgroundColor = Color.white;
					this.sightButton.foregroundColor = this.sightButton.backgroundColor;
					this.sightButton.tooltip = this.localization.format("Sight_Hook_Tooltip");
					this.sightButton.iconImage.backgroundColor = Color.white;
					this.sightButton.iconImage.backgroundTint = ESleekTint.FOREGROUND;
				}
			}
			if (this.tacticalButton != null)
			{
				this.tacticalSearch = base.player.inventory.search(EItemType.TACTICAL, ((ItemGunAsset)base.player.equipment.asset).attachmentCalibers);
				if (this.tacticalJars != null)
				{
					this.tacticalButton.remove(this.tacticalJars);
				}
				this.tacticalJars = new SleekJars(100f, this.tacticalSearch);
				this.tacticalJars.sizeScale_X = 1f;
				this.tacticalJars.sizeScale_Y = 1f;
				this.tacticalJars.onClickedJar = new ClickedJar(this.onClickedTacticalJar);
				this.tacticalButton.add(this.tacticalJars);
				if (this.thirdAttachments.tacticalAsset != null)
				{
					this.tacticalButton.backgroundTint = ESleekTint.NONE;
					this.tacticalButton.foregroundTint = ESleekTint.NONE;
					this.tacticalButton.backgroundColor = ItemTool.getRarityColorUI(this.thirdAttachments.tacticalAsset.rarity);
					this.tacticalButton.foregroundColor = this.tacticalButton.backgroundColor;
					this.tacticalButton.tooltip = this.thirdAttachments.tacticalAsset.itemName;
					this.tacticalButton.iconImage.backgroundColor = this.tacticalButton.backgroundColor;
					this.tacticalButton.iconImage.backgroundTint = ESleekTint.NONE;
				}
				else
				{
					this.tacticalButton.backgroundTint = ESleekTint.BACKGROUND;
					this.tacticalButton.foregroundTint = ESleekTint.FOREGROUND;
					this.tacticalButton.backgroundColor = Color.white;
					this.tacticalButton.foregroundColor = this.tacticalButton.backgroundColor;
					this.tacticalButton.tooltip = this.localization.format("Tactical_Hook_Tooltip");
					this.tacticalButton.iconImage.backgroundColor = Color.white;
					this.tacticalButton.iconImage.backgroundTint = ESleekTint.FOREGROUND;
				}
			}
			if (this.gripButton != null)
			{
				this.gripSearch = base.player.inventory.search(EItemType.GRIP, ((ItemGunAsset)base.player.equipment.asset).attachmentCalibers);
				if (this.gripJars != null)
				{
					this.gripButton.remove(this.gripJars);
				}
				this.gripJars = new SleekJars(100f, this.gripSearch);
				this.gripJars.sizeScale_X = 1f;
				this.gripJars.sizeScale_Y = 1f;
				this.gripJars.onClickedJar = new ClickedJar(this.onClickedGripJar);
				this.gripButton.add(this.gripJars);
				if (this.thirdAttachments.gripAsset != null)
				{
					this.gripButton.backgroundTint = ESleekTint.NONE;
					this.gripButton.foregroundTint = ESleekTint.NONE;
					this.gripButton.backgroundColor = ItemTool.getRarityColorUI(this.thirdAttachments.gripAsset.rarity);
					this.gripButton.foregroundColor = this.gripButton.backgroundColor;
					this.gripButton.tooltip = this.thirdAttachments.gripAsset.itemName;
					this.gripButton.iconImage.backgroundColor = this.gripButton.backgroundColor;
					this.gripButton.iconImage.backgroundTint = ESleekTint.NONE;
				}
				else
				{
					this.gripButton.backgroundTint = ESleekTint.BACKGROUND;
					this.gripButton.foregroundTint = ESleekTint.FOREGROUND;
					this.gripButton.backgroundColor = Color.white;
					this.gripButton.foregroundColor = Color.white;
					this.gripButton.tooltip = this.localization.format("Grip_Hook_Tooltip");
					this.gripButton.iconImage.backgroundColor = Color.white;
					this.gripButton.iconImage.backgroundTint = ESleekTint.FOREGROUND;
				}
			}
			if (this.barrelButton != null)
			{
				this.barrelSearch = base.player.inventory.search(EItemType.BARREL, ((ItemGunAsset)base.player.equipment.asset).attachmentCalibers);
				if (this.barrelJars != null)
				{
					this.barrelButton.remove(this.barrelJars);
				}
				this.barrelJars = new SleekJars(100f, this.barrelSearch);
				this.barrelJars.sizeScale_X = 1f;
				this.barrelJars.sizeScale_Y = 1f;
				this.barrelJars.onClickedJar = new ClickedJar(this.onClickedBarrelJar);
				this.barrelButton.add(this.barrelJars);
				if (this.thirdAttachments.barrelAsset != null)
				{
					this.barrelButton.backgroundTint = ESleekTint.NONE;
					this.barrelButton.foregroundTint = ESleekTint.NONE;
					this.barrelButton.backgroundColor = ItemTool.getRarityColorUI(this.thirdAttachments.barrelAsset.rarity);
					this.barrelButton.foregroundColor = this.barrelButton.backgroundColor;
					this.barrelButton.tooltip = this.thirdAttachments.barrelAsset.itemName;
					this.barrelButton.iconImage.backgroundColor = this.barrelButton.backgroundColor;
					this.barrelButton.iconImage.backgroundTint = ESleekTint.NONE;
				}
				else
				{
					this.barrelButton.backgroundTint = ESleekTint.BACKGROUND;
					this.barrelButton.foregroundTint = ESleekTint.FOREGROUND;
					this.barrelButton.backgroundColor = Color.white;
					this.barrelButton.foregroundColor = Color.white;
					this.barrelButton.tooltip = this.localization.format("Barrel_Hook_Tooltip");
					this.barrelButton.iconImage.backgroundColor = Color.white;
					this.barrelButton.iconImage.backgroundTint = ESleekTint.FOREGROUND;
				}
				if (this.thirdAttachments.barrelAsset != null && this.thirdAttachments.barrelAsset.showQuality)
				{
					this.barrelQualityImage.backgroundColor = ItemTool.getQualityColor((float)base.player.equipment.state[16] / 100f);
					this.barrelQualityImage.foregroundColor = this.barrelQualityImage.backgroundColor;
					this.barrelQualityLabel.text = base.player.equipment.state[16] + "%";
					this.barrelQualityLabel.backgroundColor = this.barrelQualityImage.backgroundColor;
					this.barrelQualityLabel.foregroundColor = this.barrelQualityImage.backgroundColor;
					this.barrelQualityLabel.isVisible = true;
					this.barrelQualityImage.isVisible = true;
				}
				else
				{
					this.barrelQualityLabel.isVisible = false;
					this.barrelQualityImage.isVisible = false;
				}
			}
			if (this.magazineButton != null)
			{
				this.magazineSearch = base.player.inventory.search(EItemType.MAGAZINE, ((ItemGunAsset)base.player.equipment.asset).magazineCalibers);
				if (this.magazineJars != null)
				{
					this.magazineButton.remove(this.magazineJars);
				}
				this.magazineJars = new SleekJars(100f, this.magazineSearch);
				this.magazineJars.sizeScale_X = 1f;
				this.magazineJars.sizeScale_Y = 1f;
				this.magazineJars.onClickedJar = new ClickedJar(this.onClickedMagazineJar);
				this.magazineButton.add(this.magazineJars);
				if (this.thirdAttachments.magazineAsset != null)
				{
					this.magazineButton.backgroundTint = ESleekTint.NONE;
					this.magazineButton.foregroundTint = ESleekTint.NONE;
					this.magazineButton.backgroundColor = ItemTool.getRarityColorUI(this.thirdAttachments.magazineAsset.rarity);
					this.magazineButton.foregroundColor = this.magazineButton.backgroundColor;
					this.magazineButton.tooltip = this.thirdAttachments.magazineAsset.itemName;
					this.magazineButton.iconImage.backgroundColor = this.magazineButton.backgroundColor;
					this.magazineButton.iconImage.backgroundTint = ESleekTint.NONE;
				}
				else
				{
					this.magazineButton.backgroundTint = ESleekTint.BACKGROUND;
					this.magazineButton.foregroundTint = ESleekTint.FOREGROUND;
					this.magazineButton.backgroundColor = Color.white;
					this.magazineButton.foregroundColor = Color.white;
					this.magazineButton.tooltip = this.localization.format("Magazine_Hook_Tooltip");
					this.magazineButton.iconImage.backgroundColor = Color.white;
					this.magazineButton.iconImage.backgroundTint = ESleekTint.FOREGROUND;
				}
				if (this.thirdAttachments.magazineAsset != null && this.thirdAttachments.magazineAsset.showQuality)
				{
					this.magazineQualityImage.backgroundColor = ItemTool.getQualityColor((float)base.player.equipment.state[17] / 100f);
					this.magazineQualityImage.foregroundColor = this.magazineQualityImage.backgroundColor;
					this.magazineQualityLabel.text = base.player.equipment.state[17] + "%";
					this.magazineQualityLabel.backgroundColor = this.magazineQualityImage.backgroundColor;
					this.magazineQualityLabel.foregroundColor = this.magazineQualityImage.backgroundColor;
					this.magazineQualityLabel.isVisible = true;
					this.magazineQualityImage.isVisible = true;
				}
				else
				{
					this.magazineQualityLabel.isVisible = false;
					this.magazineQualityImage.isVisible = false;
				}
			}
		}

		private void updateInfo()
		{
			if (this.ammo == 0)
			{
				this.ammoLabel.foregroundColor = Palette.COLOR_R;
			}
			else
			{
				this.ammoLabel.foregroundColor = Color.white;
			}
			this.ammoLabel.text = this.localization.format("Ammo", new object[]
			{
				this.ammo,
				(int)((this.thirdAttachments.magazineAsset == null) ? 0 : this.thirdAttachments.magazineAsset.amount)
			});
			if (this.firemode == EFiremode.SAFETY)
			{
				this.firemodeLabel.text = this.localization.format("Firemode", new object[]
				{
					this.localization.format("Safety"),
					ControlsSettings.firemode
				});
			}
			else if (this.firemode == EFiremode.SEMI)
			{
				this.firemodeLabel.text = this.localization.format("Firemode", new object[]
				{
					this.localization.format("Semi"),
					ControlsSettings.firemode
				});
			}
			else if (this.firemode == EFiremode.AUTO)
			{
				this.firemodeLabel.text = this.localization.format("Firemode", new object[]
				{
					this.localization.format("Auto"),
					ControlsSettings.firemode
				});
			}
			else if (this.firemode == EFiremode.BURST)
			{
				this.firemodeLabel.text = this.localization.format("Firemode", new object[]
				{
					this.localization.format("Burst"),
					ControlsSettings.firemode
				});
			}
			this.attachLabel.text = this.localization.format("Attach", new object[]
			{
				(this.thirdAttachments.magazineAsset == null) ? this.localization.format("None") : this.thirdAttachments.magazineAsset.itemName,
				ControlsSettings.attach
			});
			if (this.thirdAttachments.magazineAsset != null)
			{
				this.attachLabel.foregroundColor = ItemTool.getRarityColorUI(this.thirdAttachments.magazineAsset.rarity);
				this.attachLabel.foregroundTint = ESleekTint.NONE;
			}
			else
			{
				this.attachLabel.foregroundColor = Color.white;
				this.attachLabel.foregroundTint = ESleekTint.FOREGROUND;
			}
		}

		private void onPerspectiveUpdated(EPlayerPerspective newPerspective)
		{
			this.updateCrosshair();
			if (newPerspective == EPlayerPerspective.THIRD)
			{
				if (this.isAiming)
				{
					PlayerUI.updateScope(false);
					base.player.look.enableZoom(OptionsSettings.view * 0.8f);
					base.player.look.disableOverlay();
				}
				else
				{
					base.player.look.disableZoom();
				}
				if (((ItemGunAsset)base.player.equipment.asset).action != EAction.Minigun)
				{
					if (base.player.movement.getVehicle() != null)
					{
						PlayerUI.disableCrosshair();
					}
					else
					{
						PlayerUI.enableCrosshair();
					}
				}
			}
			else if (this.isAiming)
			{
				if (GraphicsSettings.scopeQuality == EGraphicQuality.OFF && PlayerLifeUI.scopeImage.texture != null)
				{
					PlayerUI.updateScope(true);
					base.player.look.enableZoom(this.zoom * 2f);
					base.player.look.enableOverlay();
				}
				else
				{
					base.player.look.disableZoom();
				}
				if (((ItemGunAsset)base.player.equipment.asset).action != EAction.Minigun)
				{
					PlayerUI.disableCrosshair();
				}
			}
			else
			{
				base.player.look.disableZoom();
				if (((ItemGunAsset)base.player.equipment.asset).action != EAction.Minigun)
				{
					PlayerUI.enableCrosshair();
				}
			}
		}

		private void onClickedSightHookButton(SleekButton button)
		{
			base.channel.send("askAttachSight", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[]
			{
				255,
				255,
				255
			});
		}

		private void onClickedTacticalHookButton(SleekButton button)
		{
			base.channel.send("askAttachTactical", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[]
			{
				255,
				255,
				255
			});
		}

		private void onClickedGripHookButton(SleekButton button)
		{
			base.channel.send("askAttachGrip", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[]
			{
				255,
				255,
				255
			});
		}

		private void onClickedBarrelHookButton(SleekButton button)
		{
			base.channel.send("askAttachBarrel", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[]
			{
				255,
				255,
				255
			});
		}

		private void onClickedMagazineHookButton(SleekButton button)
		{
			base.channel.send("askAttachMagazine", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[]
			{
				255,
				255,
				255
			});
		}

		private void onClickedSightJar(SleekJars jars, int index)
		{
			base.channel.send("askAttachSight", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[]
			{
				this.sightSearch[index].page,
				this.sightSearch[index].jar.x,
				this.sightSearch[index].jar.y
			});
		}

		private void onClickedTacticalJar(SleekJars jars, int index)
		{
			base.channel.send("askAttachTactical", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[]
			{
				this.tacticalSearch[index].page,
				this.tacticalSearch[index].jar.x,
				this.tacticalSearch[index].jar.y
			});
		}

		private void onClickedGripJar(SleekJars jars, int index)
		{
			base.channel.send("askAttachGrip", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[]
			{
				this.gripSearch[index].page,
				this.gripSearch[index].jar.x,
				this.gripSearch[index].jar.y
			});
		}

		private void onClickedBarrelJar(SleekJars jars, int index)
		{
			base.channel.send("askAttachBarrel", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[]
			{
				this.barrelSearch[index].page,
				this.barrelSearch[index].jar.x,
				this.barrelSearch[index].jar.y
			});
		}

		private void onClickedMagazineJar(SleekJars jars, int index)
		{
			base.channel.send("askAttachMagazine", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[]
			{
				this.magazineSearch[index].page,
				this.magazineSearch[index].jar.x,
				this.magazineSearch[index].jar.y
			});
		}

		private void startAim()
		{
			if (base.channel.isOwner || Provider.isServer)
			{
				base.player.movement.multiplier = 0.75f;
			}
			if (base.channel.isOwner)
			{
				base.player.animator.multiplier = 0.1f;
				base.player.animator.pref = 0f;
				if (!((ItemGunAsset)base.player.equipment.asset).isTurret && ((ItemGunAsset)base.player.equipment.asset).action != EAction.Minigun)
				{
					if (GraphicsSettings.scopeQuality == EGraphicQuality.OFF && this.firstAttachments.sightModel != null && this.firstAttachments.scopeHook != null && this.firstAttachments.scopeHook.FindChild("Reticule") != null)
					{
						Texture mainTexture = this.firstAttachments.scopeHook.FindChild("Reticule").GetComponent<Renderer>().sharedMaterial.mainTexture;
						if (mainTexture.width <= 64)
						{
							PlayerLifeUI.scopeImage.positionOffset_X = -mainTexture.width / 2;
							PlayerLifeUI.scopeImage.positionOffset_Y = -mainTexture.height / 2;
							PlayerLifeUI.scopeImage.positionScale_X = 0.5f;
							PlayerLifeUI.scopeImage.positionScale_Y = 0.5f;
							PlayerLifeUI.scopeImage.sizeOffset_X = mainTexture.width;
							PlayerLifeUI.scopeImage.sizeOffset_Y = mainTexture.height;
							PlayerLifeUI.scopeImage.sizeScale_X = 0f;
							PlayerLifeUI.scopeImage.sizeScale_Y = 0f;
						}
						else
						{
							PlayerLifeUI.scopeImage.positionOffset_X = 0;
							PlayerLifeUI.scopeImage.positionOffset_Y = 0;
							PlayerLifeUI.scopeImage.positionScale_X = 0f;
							PlayerLifeUI.scopeImage.positionScale_Y = 0f;
							PlayerLifeUI.scopeImage.sizeOffset_X = 0;
							PlayerLifeUI.scopeImage.sizeOffset_Y = 0;
							PlayerLifeUI.scopeImage.sizeScale_X = 1f;
							PlayerLifeUI.scopeImage.sizeScale_Y = 1f;
						}
						PlayerLifeUI.scopeImage.texture = mainTexture;
						if (this.firstAttachments.aimHook.parent.FindChild("Reticule") != null)
						{
							PlayerLifeUI.scopeImage.backgroundColor = OptionsSettings.criticalHitmarkerColor;
						}
						else
						{
							PlayerLifeUI.scopeImage.backgroundColor = Color.white;
						}
						base.player.animator.viewOffset = Vector3.up;
					}
					else
					{
						PlayerLifeUI.scopeImage.texture = null;
						if (this.firstAttachments.aimHook != null || this.firstAttachments.viewHook == null)
						{
							base.player.animator.viewOffset = this.sightOffset + this.scopeOffset;
						}
						else
						{
							base.player.animator.viewOffset = this.viewOffset;
						}
					}
				}
				if (((ItemGunAsset)base.player.equipment.asset).isTurret)
				{
					base.player.animator.driveOffset = Vector3.up;
				}
				base.player.look.sensitivity = this.zoom / 90f;
				if (((ItemGunAsset)base.player.equipment.asset).isTurret || ((ItemGunAsset)base.player.equipment.asset).action == EAction.Minigun)
				{
					base.player.look.enableZoom(this.zoom);
				}
				else if (base.player.look.perspective == EPlayerPerspective.FIRST)
				{
					if (GraphicsSettings.scopeQuality == EGraphicQuality.OFF && PlayerLifeUI.scopeImage.texture != null)
					{
						PlayerUI.updateScope(true);
						base.player.look.enableZoom(this.zoom * 2f);
						base.player.look.enableOverlay();
					}
				}
				else if (base.player.look.perspective == EPlayerPerspective.THIRD)
				{
					base.player.look.enableZoom(OptionsSettings.view * 0.8f);
				}
				this.updateCrosshair();
				PlayerGroupUI.container.isVisible = false;
			}
			base.player.playSound(((ItemGunAsset)base.player.equipment.asset).aim);
			this.isMinigunSpinning = true;
			base.player.animator.play("Aim_Start", false);
		}

		private void stopAim()
		{
			if (base.channel.isOwner || Provider.isServer)
			{
				base.player.movement.multiplier = 1f;
			}
			if (base.channel.isOwner)
			{
				if (!((ItemGunAsset)base.player.equipment.asset).isTurret)
				{
					base.player.animator.viewOffset = Vector3.zero;
				}
				base.player.animator.driveOffset = Vector3.zero;
				base.player.animator.viewSway = Vector3.zero;
				base.player.animator.multiplier = 1f;
				base.player.animator.pref = 1f;
				PlayerUI.updateScope(false);
				base.player.look.sensitivity = 1f;
				base.player.look.disableZoom();
				base.player.look.disableOverlay();
				this.updateCrosshair();
				PlayerGroupUI.container.isVisible = true;
			}
			this.isMinigunSpinning = false;
			base.player.animator.play("Aim_Stop", false);
		}

		private void startAttach()
		{
			PlayerUI.isLocked = true;
			PlayerLifeUI.close();
			if (this.sightButton != null)
			{
				this.sightButton.isVisible = true;
			}
			if (this.tacticalButton != null)
			{
				this.tacticalButton.isVisible = true;
			}
			if (this.gripButton != null)
			{
				this.gripButton.isVisible = true;
			}
			if (this.barrelButton != null)
			{
				this.barrelButton.isVisible = true;
			}
			if (this.magazineButton != null)
			{
				this.magazineButton.isVisible = true;
			}
			this.updateCrosshair();
		}

		private void stopAttach()
		{
			PlayerUI.isLocked = false;
			PlayerLifeUI.open();
			if (this.sightButton != null)
			{
				this.sightButton.isVisible = false;
			}
			if (this.tacticalButton != null)
			{
				this.tacticalButton.isVisible = false;
			}
			if (this.gripButton != null)
			{
				this.gripButton.isVisible = false;
			}
			if (this.barrelButton != null)
			{
				this.barrelButton.isVisible = false;
			}
			if (this.magazineButton != null)
			{
				this.magazineButton.isVisible = false;
			}
			this.updateCrosshair();
		}

		private void Update()
		{
			if (!Dedicator.isDedicated)
			{
				ItemGunAsset itemGunAsset = base.player.equipment.asset as ItemGunAsset;
				if (itemGunAsset != null && itemGunAsset.action == EAction.Minigun)
				{
					if (this.isMinigunSpinning)
					{
						this.minigunSpeed = Mathf.Lerp(this.minigunSpeed, 1f, 8f * Time.deltaTime);
					}
					else
					{
						this.minigunSpeed = Mathf.Lerp(this.minigunSpeed, 0f, 2f * Time.deltaTime);
					}
					this.minigunDistance += this.minigunSpeed * 720f * Time.deltaTime;
					if (this.firstMinigunBarrel != null)
					{
						this.firstMinigunBarrel.localRotation = Quaternion.Euler(0f, this.minigunDistance, 0f);
					}
					if (this.thirdMinigunBarrel != null)
					{
						this.thirdMinigunBarrel.localRotation = Quaternion.Euler(0f, this.minigunDistance, 0f);
					}
					if (this.whir != null)
					{
						this.whir.volume = this.minigunSpeed;
						this.whir.pitch = Mathf.Lerp(0.75f, 1f, this.minigunSpeed);
					}
				}
			}
			if (base.player.movement.getVehicle() != null && base.player.movement.getVehicle().passengers[(int)base.player.movement.getSeat()].turret != null)
			{
				Transform turretAim = base.player.movement.getVehicle().passengers[(int)base.player.movement.getSeat()].turretAim;
				if (turretAim != null)
				{
					Transform transform = turretAim.FindChild("Barrel");
					Transform transform2 = turretAim.FindChild("Eject");
					if (this.thirdMuzzleEmitter != null && transform != null)
					{
						this.thirdMuzzleEmitter.transform.position = transform.position;
						this.thirdMuzzleEmitter.transform.rotation = transform.rotation;
					}
					if (this.thirdShellEmitter != null && transform2 != null)
					{
						this.thirdShellEmitter.transform.position = transform2.position;
						this.thirdShellEmitter.transform.rotation = transform2.rotation;
					}
				}
			}
			if (base.channel.isOwner)
			{
				if (this.laser != null)
				{
					if (base.player.look.perspective == EPlayerPerspective.FIRST)
					{
						if (!base.player.look.isCam && PhysicsUtility.raycast(new Ray(base.player.look.aim.position, base.player.look.aim.forward), out this.contact, 256f, RayMasks.BLOCK_LASER, 0))
						{
							this.laser.position = this.contact.point + base.player.look.aim.forward * -0.05f;
							this.laser.gameObject.SetActive(true);
						}
						else
						{
							this.laser.gameObject.SetActive(false);
						}
					}
					else if (base.player.look.perspective == EPlayerPerspective.THIRD)
					{
						RaycastHit raycastHit;
						if (!base.player.look.isCam && PhysicsUtility.raycast(new Ray(MainCamera.instance.transform.position, MainCamera.instance.transform.forward), out raycastHit, 512f, RayMasks.DAMAGE_CLIENT, 0))
						{
							if (PhysicsUtility.raycast(new Ray(base.player.look.aim.position, (raycastHit.point - base.player.look.aim.position).normalized), out this.contact, 256f, RayMasks.BLOCK_LASER, 0))
							{
								this.laser.position = this.contact.point + base.player.look.aim.forward * -0.05f;
								this.laser.gameObject.SetActive(true);
							}
							else
							{
								this.laser.gameObject.SetActive(false);
							}
						}
						else
						{
							this.laser.gameObject.SetActive(false);
						}
					}
				}
				else if (this.firstAttachments != null && this.firstAttachments.tacticalAsset != null && this.firstAttachments.tacticalAsset.isRangefinder)
				{
					bool flag = false;
					if (base.player.look.perspective == EPlayerPerspective.FIRST)
					{
						flag = PhysicsUtility.raycast(new Ray(base.player.look.aim.position, base.player.look.aim.forward), out this.contact, ((ItemGunAsset)base.player.equipment.asset).range, RayMasks.BLOCK_LASER, 0);
					}
					else if (base.player.look.perspective == EPlayerPerspective.THIRD)
					{
						RaycastHit raycastHit2;
						flag = (PhysicsUtility.raycast(new Ray(MainCamera.instance.transform.position, MainCamera.instance.transform.forward), out raycastHit2, 512f, RayMasks.DAMAGE_CLIENT, 0) && PhysicsUtility.raycast(new Ray(base.player.look.aim.position, (raycastHit2.point - base.player.look.aim.position).normalized), out this.contact, ((ItemGunAsset)base.player.equipment.asset).range, RayMasks.BLOCK_LASER, 0));
					}
					if (this.rangeLabel != null)
					{
						if (this.inRange)
						{
							if (OptionsSettings.metric)
							{
								this.rangeLabel.text = (int)this.contact.distance + " m";
							}
							else
							{
								this.rangeLabel.text = (int)MeasurementTool.MtoYd(this.contact.distance) + " yd";
							}
						}
						else if (OptionsSettings.metric)
						{
							this.rangeLabel.text = "? m";
						}
						else
						{
							this.rangeLabel.text = "? yd";
						}
						this.rangeLabel.backgroundColor = ((!this.inRange) ? Palette.COLOR_R : Palette.COLOR_G);
						this.rangeLabel.foregroundColor = this.rangeLabel.backgroundColor;
					}
					if (flag != this.inRange)
					{
						this.inRange = flag;
						this.firstAttachments.lightHook.gameObject.SetActive(this.inRange && this.interact);
						this.firstAttachments.light2Hook.gameObject.SetActive(!this.inRange && this.interact);
						this.thirdAttachments.lightHook.gameObject.SetActive(this.inRange && this.interact);
						this.thirdAttachments.light2Hook.gameObject.SetActive(!this.inRange && this.interact);
					}
				}
				if (this.firstFakeLight != null && this.thirdAttachments.barrelHook != null)
				{
					this.firstFakeLight.position = this.thirdAttachments.barrelHook.position + this.thirdAttachments.barrelHook.up * 0.25f;
				}
				if (this.firstFakeLight_0 != null && this.thirdAttachments.lightHook != null)
				{
					this.firstFakeLight_0.position = this.thirdAttachments.lightHook.position;
					if (this.firstFakeLight_0.gameObject.activeSelf != (base.player.look.perspective == EPlayerPerspective.FIRST && this.thirdAttachments.lightHook.gameObject.activeSelf))
					{
						this.firstFakeLight_0.gameObject.SetActive(base.player.look.perspective == EPlayerPerspective.FIRST && this.thirdAttachments.lightHook.gameObject.activeSelf);
					}
				}
				if (this.firstFakeLight_1 != null && this.thirdAttachments.light2Hook != null)
				{
					this.firstFakeLight_1.position = this.thirdAttachments.light2Hook.position;
					if (this.firstFakeLight_1.gameObject.activeSelf != (base.player.look.perspective == EPlayerPerspective.FIRST && this.thirdAttachments.light2Hook.gameObject.activeSelf))
					{
						this.firstFakeLight_1.gameObject.SetActive(base.player.look.perspective == EPlayerPerspective.FIRST && this.thirdAttachments.light2Hook.gameObject.activeSelf);
					}
				}
				this.swayTime += Time.deltaTime * (1f - this.steadyAccuracy / 4f);
				if (this.isAiming && this.firstAttachments.sightAsset != null)
				{
					float num = (1f - this.firstAttachments.sightAsset.zoom / 90f) * 1.25f;
					num *= 1f - base.player.skills.mastery(0, 5) * 0.5f;
					if (this.thirdAttachments != null && this.thirdAttachments.gripAsset != null && (!this.thirdAttachments.gripAsset.isBipod || base.player.stance.stance == EPlayerStance.PRONE))
					{
						num *= this.thirdAttachments.gripAsset.sway;
					}
					if (base.player.stance.stance == EPlayerStance.CROUCH)
					{
						num *= UseableGun.SWAY_CROUCH;
					}
					else if (base.player.stance.stance == EPlayerStance.PRONE)
					{
						num *= UseableGun.SWAY_PRONE;
					}
					base.player.animator.viewSway = Vector3.Lerp(base.player.animator.viewSway, new Vector3(Mathf.Sin(0.75f * this.swayTime) * num, Mathf.Sin(1f * this.swayTime) * num, 0f), Time.deltaTime * 4f);
				}
				else
				{
					base.player.animator.viewSway = Vector3.Lerp(base.player.animator.viewSway, Vector3.zero, Time.deltaTime * 4f);
				}
				if (this.firstAttachments.reticuleHook != null && this.firstAttachments.sightAsset != null && this.firstAttachments.sightAsset.isHolographic)
				{
					this.firstAttachments.reticuleHook.localPosition = this.reticuleOffset;
					Plane plane;
					plane..ctor(this.firstAttachments.reticuleHook.forward, this.firstAttachments.reticuleHook.position);
					float num2;
					plane.Raycast(new Ray(base.player.animator.view.position, base.player.animator.view.forward), ref num2);
					Vector3 vector = base.player.animator.view.position + base.player.animator.view.forward * num2;
					Vector3 vector2 = this.firstAttachments.reticuleHook.parent.InverseTransformPoint(vector);
					this.firstAttachments.reticuleHook.localPosition = Vector3.Lerp(this.firstAttachments.reticuleHook.localPosition, vector2, (float)this.aimAccuracy / 10f);
				}
			}
		}

		private static readonly PlayerDamageMultiplier DAMAGE_PLAYER_MULTIPLIER = new PlayerDamageMultiplier(40f, 0.6f, 0.6f, 0.8f, 1.1f);

		private static readonly ZombieDamageMultiplier DAMAGE_ZOMBIE_MULTIPLIER = new ZombieDamageMultiplier(40f, 0.3f, 0.3f, 0.6f, 1.1f);

		private static readonly AnimalDamageMultiplier DAMAGE_ANIMAL_MULTIPLIER = new AnimalDamageMultiplier(40f, 0.3f, 0.6f, 1.1f);

		private static readonly float RECOIL_CROUCH = 0.85f;

		private static readonly float RECOIL_PRONE = 0.7f;

		private static readonly float SPREAD_SPRINT = 1.25f;

		private static readonly float SPREAD_CROUCH = 0.85f;

		private static readonly float SPREAD_PRONE = 0.7f;

		private static readonly float SHAKE_CROUCH = 0.85f;

		private static readonly float SHAKE_PRONE = 0.7f;

		private static readonly float SWAY_CROUCH = 0.85f;

		private static readonly float SWAY_PRONE = 0.7f;

		private Local localization;

		private Bundle icons;

		private SleekButtonIcon sightButton;

		private SleekJars sightJars;

		private SleekButtonIcon tacticalButton;

		private SleekJars tacticalJars;

		private SleekButtonIcon gripButton;

		private SleekJars gripJars;

		private SleekButtonIcon barrelButton;

		private SleekLabel barrelQualityLabel;

		private SleekImageTexture barrelQualityImage;

		private SleekJars barrelJars;

		private SleekButtonIcon magazineButton;

		private SleekLabel magazineQualityLabel;

		private SleekImageTexture magazineQualityImage;

		private SleekJars magazineJars;

		private SleekLabel rangeLabel;

		private SleekBox infoBox;

		private SleekLabel ammoLabel;

		private SleekLabel firemodeLabel;

		private SleekLabel attachLabel;

		private Attachments firstAttachments;

		private ParticleSystem firstShellEmitter;

		private ParticleSystem firstMuzzleEmitter;

		private Transform firstFakeLight;

		private Transform firstFakeLight_0;

		private Transform firstFakeLight_1;

		private Attachments thirdAttachments;

		private ParticleSystem thirdShellEmitter;

		private ParticleSystem thirdMuzzleEmitter;

		private float minigunSpeed;

		private float minigunDistance;

		private Transform firstMinigunBarrel;

		private Transform thirdMinigunBarrel;

		private ushort tracerID;

		private ParticleSystem tracerEmitter;

		private AudioSource sound;

		private AudioSource whir;

		private Vector3 sightOffset;

		private Vector3 viewOffset;

		private Vector3 scopeOffset;

		private Vector3 reticuleOffset;

		private bool isShooting;

		private bool isJabbing;

		private bool isMinigunSpinning;

		private bool isSprinting;

		private bool isReloading;

		private bool isHammering;

		private bool isAttaching;

		private float lastShot;

		private float lastRechamber;

		private uint lastFire;

		private uint lastJab;

		private bool isFired;

		private int bursts;

		private byte aimAccuracy;

		private uint steadyAccuracy;

		private bool canSteady;

		private float swayTime;

		private List<BulletInfo> bullets;

		private float startedReload;

		private float startedHammer;

		private float reloadTime;

		private float hammerTime;

		private bool needsHammer;

		private bool needsRechamber;

		private bool needsEject;

		private bool needsUnload;

		private bool needsUnplace;

		private bool needsReplace;

		private bool interact;

		private byte ammo;

		private EFiremode firemode;

		private List<InventorySearch> sightSearch;

		private List<InventorySearch> tacticalSearch;

		private List<InventorySearch> gripSearch;

		private List<InventorySearch> barrelSearch;

		private List<InventorySearch> magazineSearch;

		private float zoom;

		private float crosshair;

		private Transform laser;

		private bool wasLaser;

		private bool wasLight;

		private bool wasRange;

		private bool wasBayonet;

		private bool inRange;

		private bool fireTacticalInput;

		private RaycastHit contact;

		private int hitmarkerIndex;
	}
}
