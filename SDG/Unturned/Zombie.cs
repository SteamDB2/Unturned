using System;
using System.Collections.Generic;
using SDG.Framework.Utilities;
using SDG.Framework.Water;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	public class Zombie : MonoBehaviour
	{
		public byte move
		{
			get
			{
				return this._move;
			}
			set
			{
				this._move = value;
				this.moveAnim = "Move_" + this.move;
			}
		}

		public byte idle
		{
			get
			{
				return this._idle;
			}
			set
			{
				this._idle = value;
				this.idleAnim = "Idle_" + this.idle;
			}
		}

		private void updateTicking()
		{
			if (this.isHunting)
			{
				if (this.isTicking)
				{
					return;
				}
				this.isTicking = true;
				ZombieManager.tickingZombies.Add(this);
				this.lastTick = Time.time;
				if (this.speciality == EZombieSpeciality.FLANKER_FRIENDLY)
				{
					ZombieManager.sendZombieSpeciality(this, EZombieSpeciality.FLANKER_STALK);
				}
			}
			else
			{
				if (!this.isTicking)
				{
					return;
				}
				this.isTicking = false;
				if (this.isWandering)
				{
					this.isWandering = false;
					ZombieManager.wanderingCount--;
				}
				ZombieManager.tickingZombies.RemoveFast(this);
				if (this.speciality == EZombieSpeciality.FLANKER_STALK)
				{
					ZombieManager.sendZombieSpeciality(this, EZombieSpeciality.FLANKER_FRIENDLY);
				}
			}
		}

		public bool isHunting
		{
			get
			{
				return this._isHunting;
			}
			set
			{
				if (value != this.isHunting)
				{
					this._isHunting = value;
					this.updateTicking();
				}
			}
		}

		public float lastDead
		{
			get
			{
				return this._lastDead;
			}
		}

		public bool isHyper
		{
			get
			{
				return ZombieManager.regions[(int)this.bound].isHyper && this.speciality != EZombieSpeciality.BOSS_ALL;
			}
		}

		public bool isRadioactive
		{
			get
			{
				return ZombieManager.regions[(int)this.bound].isRadioactive;
			}
		}

		public bool isBoss
		{
			get
			{
				switch (this.speciality)
				{
				case EZombieSpeciality.BOSS_ELECTRIC:
				case EZombieSpeciality.BOSS_WIND:
				case EZombieSpeciality.BOSS_FIRE:
				case EZombieSpeciality.BOSS_MAGMA:
					return true;
				}
				return false;
			}
		}

		public bool isMega
		{
			get
			{
				return this.speciality == EZombieSpeciality.MEGA || this.isBoss || this.speciality == EZombieSpeciality.BOSS_ALL;
			}
		}

		private float attack
		{
			get
			{
				if (this.barricade != null)
				{
					return Zombie.ATTACK_BARRICADE * (float)((!this.isMega) ? 1 : 2);
				}
				if (this.vehicle != null)
				{
					return Zombie.ATTACK_VEHICLE * (float)((!this.isMega) ? 1 : 2);
				}
				if (this.drive != null)
				{
					return Zombie.ATTACK_VEHICLE * (float)((!this.isMega) ? 1 : 2);
				}
				return Zombie.ATTACK_PLAYER * ((!Dedicator.isDedicated || this.speciality != EZombieSpeciality.NORMAL) ? 1f : 0.5f) * (float)((!this.isMega) ? 1 : 2);
			}
		}

		public void tellAlive(byte newType, byte newSpeciality, byte newShirt, byte newPants, byte newHat, byte newGear, Vector3 newPosition, byte newAngle)
		{
			this.type = newType;
			this.speciality = (EZombieSpeciality)newSpeciality;
			this.shirt = newShirt;
			this.pants = newPants;
			this.hat = newHat;
			this.gear = newGear;
			this.isDead = false;
			ZombieManager.regions[(int)this.bound].alive++;
			base.transform.position = newPosition;
			base.transform.rotation = Quaternion.Euler(0f, (float)(newAngle * 2), 0f);
			this.updateLife();
			this.apply();
			this.updateEffects();
			this.updateVisibility(this.speciality != EZombieSpeciality.FLANKER_STALK, false);
			this.updateStates();
			if (Provider.isServer)
			{
				this.reset();
			}
		}

		public void tellDead(Vector3 newRagdoll)
		{
			this.isDead = true;
			ZombieManager.regions[(int)this.bound].alive--;
			if (ZombieManager.regions[(int)this.bound].hasBeacon && Provider.isServer)
			{
				BeaconManager.checkBeacon(this.bound).despawnAlive();
			}
			this._lastDead = Time.realtimeSinceStartup;
			this.updateLife();
			if (!Dedicator.isDedicated)
			{
				this.ragdoll = newRagdoll;
				RagdollTool.ragdollZombie(base.transform.position, base.transform.rotation, this.skeleton, this.ragdoll, this.type, this.shirt, this.pants, this.hat, this.gear, this.isMega);
				if (this.radiation != null && this.isRadioactive)
				{
					EffectManager.effect(94, this.radiation.position, Vector3.up);
				}
				if (this.burner != null && (this.speciality == EZombieSpeciality.BURNER || this.speciality == EZombieSpeciality.BOSS_FIRE || this.speciality == EZombieSpeciality.BOSS_MAGMA))
				{
					EffectManager.effect(119, this.burner.position, Vector3.up);
				}
			}
			if (Provider.isServer)
			{
				this.stop();
			}
		}

		public void tellState(Vector3 newPosition, byte newAngle)
		{
			this.lastUpdatedPos = newPosition;
			this.lastUpdatedAngle = (float)(newAngle * 2);
			if (this.nsb != null)
			{
				this.nsb.addNewSnapshot(new YawSnapshotInfo(newPosition, (float)(newAngle * 2)));
			}
		}

		public void tellSpeciality(EZombieSpeciality newSpeciality)
		{
			this.speciality = newSpeciality;
			this.updateEffects();
			this.updateVisibility(this.speciality != EZombieSpeciality.FLANKER_STALK, true);
		}

		public void askThrow()
		{
			if (this.isDead)
			{
				return;
			}
			this.lastSpecial = Time.time;
			this.isThrowingBoulder = true;
			this.isPlayingBoulder = true;
			if (!Dedicator.isDedicated)
			{
				this.animator.Play("Boulder_0");
				if (this.isMega)
				{
					base.GetComponent<AudioSource>().pitch = Random.Range(0.5f, 0.7f);
				}
				else
				{
					base.GetComponent<AudioSource>().pitch = Random.Range(0.9f, 1.1f);
				}
				if (this.isHyper)
				{
					base.GetComponent<AudioSource>().pitch *= 0.9f;
				}
				base.GetComponent<AudioSource>().PlayOneShot(ZombieManager.roars[Random.Range(0, 16)]);
			}
			this.boulderItem = ((GameObject)Object.Instantiate(Resources.Load("Characters/Mega_Boulder_Item"))).transform;
			this.boulderItem.name = "Boulder";
			this.boulderItem.parent = this.rightHook;
			this.boulderItem.localPosition = Vector3.zero;
			this.boulderItem.localRotation = Quaternion.Euler(0f, 0f, 90f);
			this.boulderItem.localScale = Vector3.one;
			Object.Destroy(this.boulderItem.gameObject, 2f);
		}

		public void askBoulder(Vector3 origin, Vector3 direction)
		{
			if (this.isDead)
			{
				return;
			}
			Transform transform = ((GameObject)Object.Instantiate(Resources.Load((!Dedicator.isDedicated) ? "Characters/Mega_Boulder_Projectile_Client" : "Characters/Mega_Boulder_Projectile_Server"))).transform;
			transform.name = "Boulder";
			transform.parent = Level.effects;
			transform.position = origin;
			transform.rotation = Quaternion.LookRotation(direction) * Quaternion.Euler((float)Random.Range(0, 2) * 180f, (float)Random.Range(0, 2) * 180f, (float)Random.Range(0, 2) * 180f);
			transform.localScale = Vector3.one * 1.75f;
			transform.GetComponent<Rigidbody>().AddForce(direction * 1500f);
			transform.GetComponent<Rigidbody>().AddRelativeTorque(Random.Range(-500f, 500f), Random.Range(-500f, 500f), Random.Range(-500f, 500f), 0);
			transform.FindChild("Trap").gameObject.AddComponent<Boulder>();
			Object.Destroy(transform.gameObject, 8f);
		}

		public void askSpit()
		{
			if (this.isDead)
			{
				return;
			}
			this.lastSpecial = Time.time;
			this.isSpittingAcid = true;
			this.isPlayingSpit = true;
			if (!Dedicator.isDedicated)
			{
				this.animator.Play("Acid_0");
			}
		}

		public void askAcid(Vector3 origin, Vector3 direction)
		{
			if (this.isDead)
			{
				return;
			}
			if (!Dedicator.isDedicated)
			{
				if (this.isMega)
				{
					base.GetComponent<AudioSource>().pitch = Random.Range(0.5f, 0.7f);
				}
				else
				{
					base.GetComponent<AudioSource>().pitch = Random.Range(0.9f, 1.1f);
				}
				if (this.isHyper)
				{
					base.GetComponent<AudioSource>().pitch *= 0.9f;
				}
				base.GetComponent<AudioSource>().PlayOneShot(ZombieManager.spits[Random.Range(0, 4)]);
			}
			Transform transform = ((GameObject)Object.Instantiate(Resources.Load((!Dedicator.isDedicated) ? "Characters/Acid_Projectile_Client" : "Characters/Acid_Projectile_Server"))).transform;
			transform.name = "Acid";
			transform.parent = Level.effects;
			transform.position = origin;
			transform.rotation = Quaternion.LookRotation(direction);
			transform.GetComponent<Rigidbody>().AddForce(direction * 1000f);
			transform.FindChild("Trap").gameObject.AddComponent<Acid>();
			Object.Destroy(transform.gameObject, 8f);
		}

		public void askCharge()
		{
			if (this.isDead)
			{
				return;
			}
			this.lastSpecial = Time.time;
			this.isChargingSpark = true;
			this.isPlayingCharge = true;
			if (!Dedicator.isDedicated)
			{
				this.animator.Play("Electric_0");
				if (this.sparkSystem != null)
				{
					this.sparkSystem.Play();
				}
			}
		}

		public void askSpark(Vector3 target)
		{
			if (this.isDead)
			{
				return;
			}
			Vector3 vector = target - this.sparkSystem.transform.position;
			Vector3 normalized = vector.normalized;
			Transform transform = EffectManager.effect(127, this.sparkSystem.transform.position + normalized * 2f, normalized);
			if (transform != null)
			{
				transform.GetComponent<ParticleSystem>().main.startLifetime = (vector.magnitude - 2f) / 128f;
			}
			EffectManager.effect(129, target, -normalized);
		}

		public void askStomp()
		{
			if (this.isDead)
			{
				return;
			}
			this.lastSpecial = Time.time;
			this.isStompingWind = true;
			this.isPlayingWind = true;
			if (!Dedicator.isDedicated)
			{
				this.animator.Play("Wind_0");
				EffectManager.effect(128, base.transform.position, Vector3.up);
			}
		}

		public void askBreath()
		{
			if (this.isDead)
			{
				return;
			}
			this.lastSpecial = Time.time;
			this.isBreathingFire = true;
			this.isPlayingFire = true;
			this.fireDamage = 0f;
			if (!Dedicator.isDedicated)
			{
				this.animator.Play("Fire_0");
				if (this.fireSystem != null)
				{
					this.fireSystem.emission.enabled = true;
				}
				if (this.fireAudio != null)
				{
					this.fireAudio.pitch = Random.Range(0.95f, 1.05f);
					this.fireAudio.Play();
				}
			}
		}

		public void askAttack(byte id)
		{
			if (this.isDead)
			{
				return;
			}
			this.lastAttack = Time.time;
			this.specialAttackDelay = Random.Range(2f, 4f);
			this.isPlayingAttack = true;
			if (!Dedicator.isDedicated)
			{
				this.animator.Play("Attack_" + id);
				if (this.isMega)
				{
					base.GetComponent<AudioSource>().pitch = Random.Range(0.5f, 0.7f);
				}
				else
				{
					base.GetComponent<AudioSource>().pitch = Random.Range(0.9f, 1.1f);
				}
				if (this.isHyper)
				{
					base.GetComponent<AudioSource>().pitch *= 0.9f;
				}
				base.GetComponent<AudioSource>().PlayOneShot(ZombieManager.roars[Random.Range(0, 16)]);
			}
			if (this.speciality == EZombieSpeciality.FLANKER_FRIENDLY || this.speciality == EZombieSpeciality.FLANKER_STALK)
			{
				this.updateVisibility(true, true);
			}
		}

		public void askStartle(byte id)
		{
			if (this.isDead)
			{
				return;
			}
			this.lastStartle = Time.time;
			this.specialStartleDelay = Random.Range(1f, 2f);
			this.isPlayingStartle = true;
			if (!Dedicator.isDedicated)
			{
				this.animator.Play("Startle_" + id);
				if (this.isMega)
				{
					base.GetComponent<AudioSource>().pitch = Random.Range(0.5f, 0.7f);
				}
				else
				{
					base.GetComponent<AudioSource>().pitch = Random.Range(0.9f, 1.1f);
				}
				if (this.isHyper)
				{
					base.GetComponent<AudioSource>().pitch *= 0.9f;
				}
				base.GetComponent<AudioSource>().PlayOneShot(ZombieManager.roars[Random.Range(0, 16)]);
			}
		}

		public void askStun(byte id)
		{
			if (this.isDead)
			{
				return;
			}
			this.lastStun = Time.time;
			this.isPlayingStun = true;
			if (!Dedicator.isDedicated)
			{
				this.animator.Play("Stun_" + id);
			}
		}

		public void askDamage(ushort amount, Vector3 newRagdoll, out EPlayerKill kill, out uint xp, bool trackKill = true, bool dropLoot = true)
		{
			kill = EPlayerKill.NONE;
			xp = 0u;
			if (amount == 0 || this.isDead)
			{
				return;
			}
			if (!this.isDead)
			{
				if (ZombieManager.regions[(int)this.bound].hasBeacon)
				{
					amount = (ushort)((byte)Mathf.CeilToInt((float)amount / ((float)Mathf.Max(1, BeaconManager.checkBeacon(this.bound).initialParticipants) * 1.5f)));
				}
				if (amount >= this.health)
				{
					this.health = 0;
				}
				else
				{
					this.health -= amount;
				}
				this.ragdoll = newRagdoll;
				if (this.health == 0)
				{
					if (this.isMega)
					{
						kill = EPlayerKill.MEGA;
					}
					else
					{
						kill = EPlayerKill.ZOMBIE;
					}
					xp = LevelZombies.tables[(int)this.type].xp;
					if (ZombieManager.regions[(int)this.bound].hasBeacon)
					{
						xp = (uint)(xp * Provider.modeConfigData.Zombies.Full_Moon_Experience_Multiplier);
					}
					else
					{
						if (LightingManager.isFullMoon)
						{
							xp = (uint)(xp * Provider.modeConfigData.Zombies.Full_Moon_Experience_Multiplier);
						}
						if (dropLoot)
						{
							ZombieManager.dropLoot(this);
						}
					}
					ZombieManager.sendZombieDead(this, this.ragdoll);
					if (this.isRadioactive)
					{
						DamageTool.explode(base.transform.position + new Vector3(0f, 0.25f, 0f), 2f, EDeathCause.ACID, CSteamID.Nil, 20f, 0f, 20f, 0f, 0f, 0f, 0f, 0f, EExplosionDamageType.ZOMBIE_ACID, 2f, true);
					}
					if (this.speciality == EZombieSpeciality.BURNER || this.speciality == EZombieSpeciality.BOSS_FIRE || this.speciality == EZombieSpeciality.BOSS_MAGMA)
					{
						DamageTool.explode(base.transform.position + new Vector3(0f, 0.25f, 0f), 4f, EDeathCause.BURNER, CSteamID.Nil, 40f, 0f, 40f, 0f, 0f, 0f, 0f, 0f, EExplosionDamageType.ZOMBIE_FIRE, 4f, true);
					}
					if (trackKill)
					{
						for (int i = 0; i < Provider.clients.Count; i++)
						{
							SteamPlayer steamPlayer = Provider.clients[i];
							if (!(steamPlayer.player == null) && !(steamPlayer.player.movement == null) && !(steamPlayer.player.life == null) && !steamPlayer.player.life.isDead)
							{
								if (steamPlayer.player.movement.bound == this.bound)
								{
									steamPlayer.player.quests.trackZombieKill(this);
								}
							}
						}
					}
				}
				else if (Provider.modeConfigData.Zombies.Can_Stun && amount > ((!this.isMega) ? 20 : 150))
				{
					this.stun();
				}
				this.lastRegen = Time.time;
			}
		}

		public void sendRevive(byte type, byte speciality, byte shirt, byte pants, byte hat, byte gear, Vector3 position, float angle)
		{
			ZombieManager.sendZombieAlive(this, type, speciality, shirt, pants, hat, gear, position, MeasurementTool.angleToByte(angle));
		}

		public bool checkAlert(Player newPlayer)
		{
			return this.player != newPlayer;
		}

		public void alert(Player newPlayer)
		{
			if (this.isDead)
			{
				return;
			}
			if (this.player == newPlayer)
			{
				return;
			}
			if (this.player == null)
			{
				if (!this.isHunting && !this.isLeaving)
				{
					if (this.speciality == EZombieSpeciality.CRAWLER)
					{
						float value = Random.value;
						if (value < 0.5f)
						{
							ZombieManager.sendZombieStartle(this, 3);
						}
						else
						{
							ZombieManager.sendZombieStartle(this, 6);
						}
					}
					else if (this.speciality == EZombieSpeciality.SPRINTER)
					{
						float value2 = Random.value;
						if (value2 < 0.5f)
						{
							ZombieManager.sendZombieStartle(this, 4);
						}
						else
						{
							ZombieManager.sendZombieStartle(this, 5);
						}
					}
					else
					{
						ZombieManager.sendZombieStartle(this, (byte)Random.Range(0, 3));
					}
				}
				this.isHunting = true;
				this.huntType = EHuntType.PLAYER;
				this.isPulled = true;
				this.lastPull = Time.time;
				if (this.isWandering)
				{
					this.isWandering = false;
					ZombieManager.wanderingCount--;
				}
				this.isLeaving = false;
				this.isMoving = false;
				this.isStuck = false;
				this.lastHunted = Time.time;
				this.lastStuck = Time.time;
				this.player = newPlayer;
				this.target.position = this.player.transform.position;
				this.seeker.canSearch = true;
				this.seeker.canMove = true;
				if (this.isMega)
				{
					this.path = EZombiePath.RUSH;
				}
				else if (this.speciality == EZombieSpeciality.FLANKER_FRIENDLY || this.speciality == EZombieSpeciality.FLANKER_STALK)
				{
					if ((double)Random.value < 0.5)
					{
						this.path = EZombiePath.LEFT_FLANK;
					}
					else
					{
						this.path = EZombiePath.RIGHT_FLANK;
					}
				}
				else if (this.player.agro % 3 == 0)
				{
					this.path = EZombiePath.RUSH;
				}
				else if ((double)Random.value < 0.5)
				{
					this.path = EZombiePath.LEFT;
				}
				else
				{
					this.path = EZombiePath.RIGHT;
				}
				this.player.agro++;
			}
			else if ((newPlayer.transform.position - base.transform.position).sqrMagnitude < (this.player.transform.position - base.transform.position).sqrMagnitude)
			{
				this.player.agro--;
				this.player = newPlayer;
				this.target.position = this.player.transform.position;
				if (this.isMega)
				{
					this.path = EZombiePath.RUSH;
				}
				else if (this.player.agro % 3 == 0)
				{
					this.path = EZombiePath.RUSH;
				}
				else if ((double)Random.value < 0.5)
				{
					this.path = EZombiePath.LEFT;
				}
				else
				{
					this.path = EZombiePath.RIGHT;
				}
				this.player.agro++;
			}
		}

		public void alert(Vector3 newPosition, bool isStartling)
		{
			if (this.isDead)
			{
				return;
			}
			if (this.player == null)
			{
				if (!this.isHunting)
				{
					if (isStartling)
					{
						if (this.speciality == EZombieSpeciality.CRAWLER)
						{
							float value = Random.value;
							if (value < 0.5f)
							{
								ZombieManager.sendZombieStartle(this, 3);
							}
							else
							{
								ZombieManager.sendZombieStartle(this, 6);
							}
						}
						else if (this.speciality == EZombieSpeciality.SPRINTER)
						{
							float value2 = Random.value;
							if (value2 < 0.5f)
							{
								ZombieManager.sendZombieStartle(this, 4);
							}
							else
							{
								ZombieManager.sendZombieStartle(this, 5);
							}
						}
						else
						{
							ZombieManager.sendZombieStartle(this, (byte)Random.Range(0, 3));
						}
						this.isPulled = true;
						this.lastPull = Time.time;
						if (this.isWandering)
						{
							this.isWandering = false;
							ZombieManager.wanderingCount--;
						}
					}
					this.isHunting = true;
					this.huntType = EHuntType.POINT;
					this.isLeaving = false;
					this.isMoving = false;
					this.isStuck = false;
					this.lastHunted = Time.time;
					this.lastStuck = Time.time;
					this.target.position = newPosition;
					this.seeker.canSearch = true;
					this.seeker.canMove = true;
				}
				else if ((newPosition - base.transform.position).sqrMagnitude < (this.target.position - base.transform.position).sqrMagnitude)
				{
					this.target.position = newPosition;
				}
			}
		}

		public void updateStates()
		{
			this.lastUpdatedPos = base.transform.position;
			this.lastUpdatedAngle = base.transform.rotation.eulerAngles.y;
			if (this.nsb != null)
			{
				this.nsb.updateLastSnapshot(new YawSnapshotInfo(base.transform.position, base.transform.rotation.eulerAngles.y));
			}
		}

		private void stop()
		{
			this.isMoving = false;
			this.isAttacking = false;
			this.isHunting = false;
			this.isStuck = false;
			this.isThrowRelocating = false;
			this.lastStuck = Time.time;
			if (this.player != null)
			{
				this.player.agro--;
			}
			this.player = null;
			this.barricade = null;
			this.structure = null;
			this.vehicle = null;
			this.drive = null;
			this.seeker.canSearch = false;
			this.seeker.canMove = false;
			this.target.position = base.transform.position;
			this.seeker.stop();
		}

		private void stun()
		{
			this.isStunned = true;
			this.isMoving = false;
			this.seeker.canMove = false;
			if (this.speciality == EZombieSpeciality.CRAWLER)
			{
				float value = Random.value;
				if (value < 0.33f)
				{
					ZombieManager.sendZombieStun(this, 5);
				}
				else if (value < 0.66f)
				{
					ZombieManager.sendZombieStun(this, 7);
				}
				else
				{
					ZombieManager.sendZombieStun(this, 8);
				}
			}
			else if (this.speciality == EZombieSpeciality.SPRINTER)
			{
				float value2 = Random.value;
				if (value2 < 0.33f)
				{
					ZombieManager.sendZombieStun(this, 6);
				}
				else if (value2 < 0.66f)
				{
					ZombieManager.sendZombieStun(this, 9);
				}
				else
				{
					ZombieManager.sendZombieStun(this, 10);
				}
			}
			else
			{
				ZombieManager.sendZombieStun(this, (byte)Random.Range(0, 5));
			}
		}

		private void leave(bool quick)
		{
			this.isLeaving = true;
			this.lastLeave = Time.time;
			if (quick)
			{
				this.leaveTime = Random.Range(0.5f, 1f);
			}
			else
			{
				this.leaveTime = Random.Range(3f, 6f);
			}
			this.leaveTo = base.transform.position - 16f * (this.target.position - base.transform.position).normalized + new Vector3(Random.Range(-8f, 8f), 0f, Random.Range(-8f, 8f));
			if (!LevelNavigation.checkNavigation(this.leaveTo))
			{
				this.leaveTo = base.transform.position + 16f * (this.target.position - base.transform.position).normalized + new Vector3(Random.Range(-8f, 8f), 0f, Random.Range(-8f, 8f));
			}
			if (!LevelNavigation.checkNavigation(this.leaveTo))
			{
				this.leaveTo = base.transform.position;
			}
			this.stop();
		}

		private void updateEffects()
		{
			if (Dedicator.isDedicated)
			{
				return;
			}
			if (this.burner != null)
			{
				this.burner.gameObject.SetActive(this.speciality == EZombieSpeciality.BURNER || this.speciality == EZombieSpeciality.BOSS_FIRE || this.speciality == EZombieSpeciality.BOSS_MAGMA);
			}
			if (this.acid != null)
			{
				this.acid.gameObject.SetActive(this.speciality == EZombieSpeciality.ACID);
			}
			if (this.electric != null)
			{
				this.electric.gameObject.SetActive(this.speciality == EZombieSpeciality.BOSS_ELECTRIC);
			}
			if (this.fireSystem != null)
			{
				this.fireSystem.emission.enabled = false;
			}
		}

		private void updateVisibility(bool newVisible, bool playEffect)
		{
			if (Dedicator.isDedicated)
			{
				return;
			}
			if (newVisible == this.isVisible)
			{
				return;
			}
			this.isVisible = newVisible;
			if (this.isVisible)
			{
				if (this.attachmentModel_0 != null && this.attachmentMaterial_0 != null)
				{
					Material material;
					HighlighterTool.rematerialize(this.attachmentModel_0, this.attachmentMaterial_0, out material);
				}
				if (this.attachmentModel_1 != null && this.attachmentMaterial_1 != null)
				{
					Material material2;
					HighlighterTool.rematerialize(this.attachmentModel_1, this.attachmentMaterial_1, out material2);
				}
				if (this.renderer_0 != null && this.skinMaterial != null)
				{
					this.renderer_0.sharedMaterial = this.skinMaterial;
				}
				if (this.renderer_1 != null && this.skinMaterial != null)
				{
					this.renderer_1.sharedMaterial = this.skinMaterial;
				}
				this.attachmentMaterial_0 = null;
				this.attachmentMaterial_1 = null;
				this.skinMaterial = null;
			}
			else
			{
				if (this.attachmentModel_0 != null)
				{
					HighlighterTool.rematerialize(this.attachmentModel_0, ZombieClothing.ghostMaterial, out this.attachmentMaterial_0);
				}
				if (this.attachmentModel_1 != null)
				{
					HighlighterTool.rematerialize(this.attachmentModel_1, ZombieClothing.ghostMaterial, out this.attachmentMaterial_1);
				}
				if (this.renderer_0 != null)
				{
					this.skinMaterial = this.renderer_0.sharedMaterial;
					this.renderer_0.sharedMaterial = ZombieClothing.ghostMaterial;
				}
				if (this.renderer_1 != null)
				{
					if (this.skinMaterial == null)
					{
						this.skinMaterial = this.renderer_1.sharedMaterial;
					}
					this.renderer_1.sharedMaterial = ZombieClothing.ghostMaterial;
				}
			}
			if (playEffect)
			{
				EffectManager.effect(118, this.radiation.position, Vector3.up);
			}
		}

		private void apply()
		{
			if (!Dedicator.isDedicated)
			{
				ZombieClothing.apply(this.animator.transform, this.isMega, this.renderer_0, this.renderer_1, this.type, this.shirt, this.pants, this.hat, this.gear, out this.attachmentModel_0, out this.attachmentModel_1);
				if (this.speciality == EZombieSpeciality.BOSS_MAGMA)
				{
					Material sharedMaterial = Resources.Load<Material>("Characters/Magma_Material");
					if (this.renderer_0 != null)
					{
						this.renderer_0.sharedMaterial = sharedMaterial;
					}
					if (this.renderer_1 != null)
					{
						this.renderer_1.sharedMaterial = sharedMaterial;
					}
				}
			}
			if (this.isMega)
			{
				if (!Dedicator.isDedicated)
				{
					base.GetComponent<AudioSource>().maxDistance = 64f;
					this.animator.transform.localScale = Vector3.one * Random.Range(1.45f, 1.55f);
				}
				if (Provider.isServer)
				{
					((CharacterController)base.GetComponent<Collider>()).radius = 0.75f;
					this.seeker.speed = 6f;
				}
			}
			else
			{
				if (!Dedicator.isDedicated)
				{
					base.GetComponent<AudioSource>().maxDistance = 32f;
					this.animator.transform.localScale = Vector3.one * Random.Range(0.95f, 1.05f);
				}
				if (Provider.isServer)
				{
					((CharacterController)base.GetComponent<Collider>()).radius = 0.4f;
					if (this.speciality == EZombieSpeciality.CRAWLER)
					{
						if (Provider.modeConfigData.Zombies.Slow_Movement)
						{
							this.seeker.speed = 2.5f;
						}
						else
						{
							this.seeker.speed = 3f;
						}
					}
					else if (this.speciality == EZombieSpeciality.SPRINTER)
					{
						if (Provider.modeConfigData.Zombies.Slow_Movement)
						{
							this.seeker.speed = 6f;
						}
						else
						{
							this.seeker.speed = 6.5f;
						}
					}
					else if (this.speciality == EZombieSpeciality.FLANKER_FRIENDLY || this.speciality == EZombieSpeciality.FLANKER_STALK)
					{
						if (Provider.modeConfigData.Zombies.Slow_Movement)
						{
							this.seeker.speed = 5.5f;
						}
						else
						{
							this.seeker.speed = 6f;
						}
					}
					else if (Provider.modeConfigData.Zombies.Slow_Movement)
					{
						this.seeker.speed = 4.5f;
					}
					else
					{
						this.seeker.speed = 5.5f;
					}
				}
			}
		}

		private void updateLife()
		{
			if (!Dedicator.isDedicated)
			{
				if (this.renderer_0 != null)
				{
					this.renderer_0.enabled = !this.isDead;
				}
				if (this.renderer_1 != null)
				{
					this.renderer_1.enabled = !this.isDead;
				}
				this.skeleton.gameObject.SetActive(!this.isDead);
				if (this.eyes != null)
				{
					this.eyes.gameObject.SetActive(this.isHyper);
				}
				if (this.radiation != null)
				{
					this.radiation.gameObject.SetActive(this.isRadioactive);
				}
			}
			base.GetComponent<Collider>().enabled = !this.isDead;
		}

		private void reset()
		{
			this.target.position = base.transform.position;
			this.lastTarget = Time.time;
			this.lastLeave = Time.time;
			this.lastRelocate = Time.time;
			this.lastSpecial = Time.time;
			this.lastAttack = Time.time;
			this.lastStartle = Time.time;
			this.lastStun = Time.time;
			this.lastStuck = Time.time;
			this.cameFrom = base.transform.position;
			this.isPulled = false;
			this.pullDelay = Random.Range(24f, 96f);
			this.specialStartleDelay = Random.Range(1f, 2f);
			this.specialAttackDelay = Random.Range(2f, 4f);
			this.specialUseDelay = Random.Range(4f, 8f);
			this.boulderThrowDelay = 8f;
			this.isThrowRelocating = false;
			this.isThrowingBoulder = false;
			this.isSpittingAcid = false;
			this.isChargingSpark = false;
			this.isStompingWind = false;
			this.isBreathingFire = false;
			this.isPlayingBoulder = false;
			this.isPlayingSpit = false;
			this.isPlayingCharge = false;
			this.isPlayingWind = false;
			this.isPlayingFire = false;
			this.isPlayingAttack = false;
			this.isPlayingStartle = false;
			this.isPlayingStun = false;
			this.isMoving = false;
			this.isAttacking = false;
			this.isHunting = false;
			this.isLeaving = false;
			this.isStunned = false;
			this.isStuck = false;
			this.leaveTo = base.transform.position;
			if (this.player != null)
			{
				this.player.agro--;
			}
			this.player = null;
			this.barricade = null;
			this.structure = null;
			this.vehicle = null;
			this.drive = null;
			this.seeker.canSearch = false;
			this.seeker.canMove = false;
			this.health = LevelZombies.tables[(int)this.type].health;
			if (this.speciality == EZombieSpeciality.CRAWLER)
			{
				this.health = (ushort)((float)this.health * 1.5f);
			}
			else if (this.speciality == EZombieSpeciality.SPRINTER)
			{
				this.health = (ushort)((float)this.health * 0.5f);
			}
			else if (this.speciality == EZombieSpeciality.BOSS_ALL || this.speciality == EZombieSpeciality.BOSS_MAGMA)
			{
				this.health = 12000;
			}
			else if (this.isBoss)
			{
				this.health = 6000;
			}
			if (Level.info.type == ELevelType.HORDE)
			{
				this.health += (ushort)(Mathf.Min(ZombieManager.waveIndex - 1, 20) * 10);
			}
			this.maxHealth = this.health;
		}

		public void tick()
		{
			float delta = Time.time - this.lastTick;
			this.lastTick = Time.time;
			this.lastPull = Time.time;
			if (this.isStunned)
			{
				return;
			}
			if (this.huntType == EHuntType.PLAYER)
			{
				if (this.player == null)
				{
					this.stop();
					return;
				}
			}
			else if (this.huntType == EHuntType.POINT && !this.isMoving && Time.time - this.lastHunted > 3f)
			{
				this.stop();
				return;
			}
			if (this.player != null)
			{
				if (this.player.life.isDead)
				{
					this.leave(false);
					return;
				}
				if (this.player.movement.nav == 255 || (this.player.stance.stance == EPlayerStance.SWIM && !WaterUtility.isPointUnderwater(base.transform.position)))
				{
					this.leave(true);
					return;
				}
			}
			if (this.vehicle != null && this.vehicle.isDead)
			{
				this.vehicle = null;
			}
			if (this.drive != null && this.drive.isDead)
			{
				this.drive = null;
			}
			if (this.isStuck && Time.time - this.lastStuck > 1f && this.barricade == null && this.structure == null && this.vehicle == null && this.drive == null)
			{
				Zombie.regionsInRadius.Clear();
				Regions.getRegionsInRadius(base.transform.position, 4f, Zombie.regionsInRadius);
				Zombie.structuresInRadius.Clear();
				StructureManager.getStructuresInRadius(base.transform.position, 16f, Zombie.regionsInRadius, Zombie.structuresInRadius);
				if (Zombie.structuresInRadius.Count > 0)
				{
					this.structure = Zombie.structuresInRadius[0];
				}
				else
				{
					Zombie.vehiclesInRadius.Clear();
					VehicleManager.getVehiclesInRadius(base.transform.position, 16f, Zombie.vehiclesInRadius);
					if (Zombie.vehiclesInRadius.Count > 0)
					{
						this.vehicle = Zombie.vehiclesInRadius[0];
					}
					else
					{
						Zombie.barricadesInRadius.Clear();
						BarricadeManager.getBarricadesInRadius(base.transform.position, 16f, Zombie.regionsInRadius, Zombie.barricadesInRadius);
						if (Zombie.barricadesInRadius.Count > 0)
						{
							this.barricade = Zombie.barricadesInRadius[0];
						}
					}
				}
			}
			float num;
			if (this.barricade != null)
			{
				num = Mathf.Pow(this.barricade.position.x - base.transform.position.x, 2f) + Mathf.Pow(this.barricade.position.z - base.transform.position.z, 2f);
				this.target.position = this.barricade.position;
				this.seeker.canTurn = false;
				this.seeker.targetDirection = this.barricade.position - base.transform.position;
			}
			else if (this.structure != null)
			{
				num = 0f;
				this.target.position = base.transform.position;
				this.seeker.canTurn = false;
				this.seeker.targetDirection = this.structure.position - base.transform.position;
			}
			else if (this.vehicle != null)
			{
				num = Mathf.Pow(this.vehicle.transform.position.x - base.transform.position.x, 2f) + Mathf.Pow(this.vehicle.transform.position.z - base.transform.position.z, 2f);
				this.target.position = this.vehicle.transform.position;
				this.seeker.canTurn = false;
				this.seeker.targetDirection = this.vehicle.transform.position - base.transform.position;
			}
			else if (this.player != null)
			{
				this.drive = this.player.movement.getVehicle();
				if (this.drive != null && this.drive.isDead)
				{
					this.drive = null;
				}
				if (this.drive != null)
				{
					num = Mathf.Pow(this.drive.transform.position.x - base.transform.position.x, 2f) + Mathf.Pow(this.drive.transform.position.z - base.transform.position.z, 2f);
					this.target.position = this.drive.transform.position;
					this.seeker.canTurn = false;
					this.seeker.targetDirection = this.drive.transform.position - base.transform.position;
				}
				else
				{
					num = Mathf.Pow(this.player.transform.position.x - base.transform.position.x, 2f) + Mathf.Pow(this.player.transform.position.z - base.transform.position.z, 2f);
					if (this.isThrowRelocating)
					{
						Vector3 vector = base.transform.position - this.player.transform.position;
						vector.y = 0f;
						this.target.position = this.player.transform.position + vector.normalized * 7f;
						this.seeker.canTurn = true;
					}
					else
					{
						this.target.position = this.player.transform.position;
						if (this.path == EZombiePath.LEFT_FLANK)
						{
							if (num > 100f)
							{
								this.seeker.canTurn = true;
								this.target.position += this.player.transform.right * 9f + this.player.transform.forward * -4f;
							}
							else if (num > 20f || Vector3.Dot((base.transform.position - this.player.transform.position).normalized, this.player.transform.forward) > 0f)
							{
								this.seeker.canTurn = true;
								this.target.position += this.player.transform.right * 3f + this.player.transform.forward * -3f;
							}
							else if (num > 4f)
							{
								this.seeker.canTurn = true;
								this.target.position -= this.player.transform.forward;
							}
							else
							{
								this.seeker.canTurn = false;
								this.seeker.targetDirection = this.player.transform.position - base.transform.position;
							}
						}
						else if (this.path == EZombiePath.RIGHT_FLANK)
						{
							if (num > 100f)
							{
								this.seeker.canTurn = true;
								this.target.position += this.player.transform.right * -9f + this.player.transform.forward * -4f;
							}
							else if (num > 20f || Vector3.Dot((base.transform.position - this.player.transform.position).normalized, this.player.transform.forward) > 0f)
							{
								this.seeker.canTurn = true;
								this.target.position += this.player.transform.right * -3f + this.player.transform.forward * -3f;
							}
							else if (num > 4f)
							{
								this.seeker.canTurn = true;
								this.target.position -= this.player.transform.forward;
							}
							else
							{
								this.seeker.canTurn = false;
								this.seeker.targetDirection = this.player.transform.position - base.transform.position;
							}
						}
						else if (this.path == EZombiePath.LEFT)
						{
							if (num > 4f)
							{
								this.seeker.canTurn = true;
								this.target.position -= base.transform.right;
							}
							else
							{
								this.seeker.canTurn = false;
								this.seeker.targetDirection = this.player.transform.position - base.transform.position;
							}
						}
						else if (this.path == EZombiePath.RIGHT)
						{
							if (num > 4f)
							{
								this.seeker.canTurn = true;
								this.target.position += base.transform.right;
							}
							else
							{
								this.seeker.canTurn = false;
								this.seeker.targetDirection = this.player.transform.position - base.transform.position;
							}
						}
						else if (this.path == EZombiePath.RUSH)
						{
							if (num > 4f)
							{
								this.seeker.canTurn = true;
								this.target.position -= base.transform.forward;
							}
							else
							{
								this.seeker.canTurn = false;
								this.seeker.targetDirection = this.player.transform.position - base.transform.position;
							}
						}
						if (!Dedicator.isDedicated && this.speciality == EZombieSpeciality.SPRINTER)
						{
							this.target.position -= base.transform.forward * 0.15f;
						}
					}
				}
			}
			else
			{
				num = Mathf.Pow(this.target.position.x - base.transform.position.x, 2f) + Mathf.Pow(this.target.position.z - base.transform.position.z, 2f);
				this.seeker.canTurn = true;
			}
			float num2 = Mathf.Abs(this.target.position.y - base.transform.position.y);
			this.isMoving = (num > 3f);
			if (!this.isWandering && num > 4096f && (this.player == null || !ZombieManager.regions[(int)this.bound].hasBeacon))
			{
				this.leave(false);
				return;
			}
			if (this.player != null || this.barricade != null || this.structure != null || this.vehicle != null || this.drive != null)
			{
				if (this.player != null && (this.speciality == EZombieSpeciality.MEGA || (this.speciality == EZombieSpeciality.BOSS_ALL && Random.value < 0.2f)) && Time.time - this.lastStartle > this.specialStartleDelay && Time.time - this.lastAttack > this.specialAttackDelay && Time.time - this.lastSpecial > this.boulderThrowDelay)
				{
					if (num < 20f)
					{
						if (this.isThrowRelocating)
						{
							if (Time.time - this.lastRelocate > 1.5f)
							{
								this.isThrowRelocating = false;
								this.lastSpecial = Time.time;
								this.boulderThrowDelay = Random.Range(8f, 12f);
							}
						}
						else
						{
							this.isThrowRelocating = true;
							this.lastRelocate = Time.time;
						}
					}
					else
					{
						this.isThrowRelocating = false;
						this.lastSpecial = Time.time;
						this.boulderThrowDelay = Random.Range(4f, 8f);
						this.seeker.canMove = false;
						ZombieManager.sendZombieThrow(this);
					}
				}
				else
				{
					this.isThrowRelocating = false;
				}
				if (this.player != null && (this.speciality == EZombieSpeciality.ACID || (this.speciality == EZombieSpeciality.BOSS_ALL && Random.value < 0.2f)) && Time.time - this.lastStartle > this.specialStartleDelay && Time.time - this.lastAttack > this.specialAttackDelay && Time.time - this.lastSpecial > this.specialUseDelay)
				{
					this.lastSpecial = Time.time;
					this.specialUseDelay = Random.Range(4f, 8f);
					this.seeker.canMove = false;
					ZombieManager.sendZombieSpit(this);
				}
				if (this.player != null && (this.speciality == EZombieSpeciality.BOSS_WIND || (this.speciality == EZombieSpeciality.BOSS_ALL && Random.value < 0.2f)) && Time.time - this.lastStartle > this.specialStartleDelay && Time.time - this.lastAttack > this.specialAttackDelay && Time.time - this.lastSpecial > this.specialUseDelay && (this.player.transform.position - base.transform.position).sqrMagnitude < 144f)
				{
					this.lastSpecial = Time.time;
					this.specialUseDelay = Random.Range(4f, 8f);
					this.seeker.canMove = false;
					ZombieManager.sendZombieStomp(this);
				}
				if (this.player != null && (this.speciality == EZombieSpeciality.BOSS_FIRE || this.speciality == EZombieSpeciality.BOSS_MAGMA || (this.speciality == EZombieSpeciality.BOSS_ALL && Random.value < 0.2f)) && Time.time - this.lastStartle > this.specialStartleDelay && Time.time - this.lastAttack > this.specialAttackDelay && Time.time - this.lastSpecial > this.specialUseDelay && (this.player.transform.position - base.transform.position).sqrMagnitude < 529f)
				{
					this.lastSpecial = Time.time;
					this.specialUseDelay = Random.Range(4f, 8f);
					this.seeker.canMove = false;
					ZombieManager.sendZombieBreath(this);
				}
				if (this.player != null && (this.speciality == EZombieSpeciality.BOSS_ELECTRIC || (this.speciality == EZombieSpeciality.BOSS_ALL && Random.value < 0.2f)) && Time.time - this.lastStartle > this.specialStartleDelay && Time.time - this.lastAttack > this.specialAttackDelay && Time.time - this.lastSpecial > this.specialUseDelay && (this.player.transform.position - base.transform.position).sqrMagnitude > 4f && (this.player.transform.position - base.transform.position).sqrMagnitude < 4096f)
				{
					this.lastSpecial = Time.time;
					this.specialUseDelay = Random.Range(4f, 8f);
					this.seeker.canMove = false;
					ZombieManager.sendZombieCharge(this);
				}
				if ((this.structure != null || num < this.attack) && num2 < ((!this.isHyper) ? 2f : 3.5f) * ((!this.isMega) ? 1f : 1.5f))
				{
					if (this.speciality == EZombieSpeciality.SPRINTER || Time.time - this.lastTarget > ((!Dedicator.isDedicated) ? 0.1f : 0.5f))
					{
						if (this.isAttacking)
						{
							if (Time.time - this.lastAttack > this.attackTime / 2f)
							{
								this.isAttacking = false;
								byte b = (byte)((float)LevelZombies.tables[(int)this.type].damage * ((!this.isHyper) ? 1f : 1.5f));
								b = (byte)((float)b * Provider.modeConfigData.Zombies.Damage_Multiplier);
								if (this.speciality == EZombieSpeciality.CRAWLER)
								{
									b = (byte)((float)b * 2f);
								}
								else if (this.speciality == EZombieSpeciality.SPRINTER)
								{
									b = (byte)((float)b * 0.75f);
								}
								if (this.structure != null)
								{
									StructureManager.damage(this.structure, (this.target.position - base.transform.position).normalized * (float)b, (float)b, 1f, true);
									if (this.structure == null || !this.structure.CompareTag("Structure"))
									{
										this.structure = null;
										this.isStuck = false;
										this.lastStuck = Time.time;
									}
								}
								else if (this.barricade != null)
								{
									InteractableDoorHinge component = this.barricade.GetComponent<InteractableDoorHinge>();
									if (component != null)
									{
										BarricadeManager.damage(this.barricade.parent.parent, (float)b, 1f, true);
									}
									else
									{
										BarricadeManager.damage(this.barricade, (float)b, 1f, true);
									}
								}
								else if (this.vehicle != null)
								{
									VehicleManager.damage(this.vehicle, (float)b, 1f, true);
								}
								else if (this.drive != null)
								{
									VehicleManager.damage(this.drive, (float)b, 1f, true);
								}
								else if (this.player != null)
								{
									if (this.player.skills.boost == EPlayerBoost.HARDENED)
									{
										b = (byte)((float)b * 0.75f);
									}
									if (this.isMega)
									{
										if (this.player.clothing.hat != 0)
										{
											ItemClothingAsset itemClothingAsset = (ItemClothingAsset)Assets.find(EAssetType.ITEM, this.player.clothing.hat);
											if (itemClothingAsset != null)
											{
												if (Provider.modeConfigData.Items.Has_Durability && this.player.clothing.hatQuality > 0)
												{
													PlayerClothing clothing = this.player.clothing;
													clothing.hatQuality -= 1;
													this.player.clothing.sendUpdateHatQuality();
												}
												float num3 = itemClothingAsset.armor + (1f - itemClothingAsset.armor) * (1f - (float)this.player.clothing.hatQuality / 100f);
												b = (byte)((float)b * num3);
											}
										}
										else if (this.player.clothing.vest != 0)
										{
											ItemClothingAsset itemClothingAsset2 = (ItemClothingAsset)Assets.find(EAssetType.ITEM, this.player.clothing.vest);
											if (itemClothingAsset2 != null)
											{
												if (Provider.modeConfigData.Items.Has_Durability && this.player.clothing.vestQuality > 0)
												{
													PlayerClothing clothing2 = this.player.clothing;
													clothing2.vestQuality -= 1;
													this.player.clothing.sendUpdateVestQuality();
												}
												float num4 = itemClothingAsset2.armor + (1f - itemClothingAsset2.armor) * (1f - (float)this.player.clothing.vestQuality / 100f);
												b = (byte)((float)b * num4);
											}
										}
										else if (this.player.clothing.shirt != 0)
										{
											ItemClothingAsset itemClothingAsset3 = (ItemClothingAsset)Assets.find(EAssetType.ITEM, this.player.clothing.shirt);
											if (itemClothingAsset3 != null)
											{
												if (Provider.modeConfigData.Items.Has_Durability && this.player.clothing.shirtQuality > 0)
												{
													PlayerClothing clothing3 = this.player.clothing;
													clothing3.shirtQuality -= 1;
													this.player.clothing.sendUpdateShirtQuality();
												}
												float num5 = itemClothingAsset3.armor + (1f - itemClothingAsset3.armor) * (1f - (float)this.player.clothing.shirtQuality / 100f);
												b = (byte)((float)b * num5);
											}
										}
									}
									else if (this.speciality == EZombieSpeciality.NORMAL)
									{
										if (this.player.clothing.vest != 0)
										{
											ItemClothingAsset itemClothingAsset4 = (ItemClothingAsset)Assets.find(EAssetType.ITEM, this.player.clothing.vest);
											if (itemClothingAsset4 != null)
											{
												if (Provider.modeConfigData.Items.Has_Durability && this.player.clothing.vestQuality > 0)
												{
													PlayerClothing clothing4 = this.player.clothing;
													clothing4.vestQuality -= 1;
													this.player.clothing.sendUpdateVestQuality();
												}
												float num6 = itemClothingAsset4.armor + (1f - itemClothingAsset4.armor) * (1f - (float)this.player.clothing.vestQuality / 100f);
												b = (byte)((float)b * num6);
											}
										}
										else if (this.player.clothing.shirt != 0)
										{
											ItemClothingAsset itemClothingAsset5 = (ItemClothingAsset)Assets.find(EAssetType.ITEM, this.player.clothing.shirt);
											if (itemClothingAsset5 != null)
											{
												if (Provider.modeConfigData.Items.Has_Durability && this.player.clothing.shirtQuality > 0)
												{
													PlayerClothing clothing5 = this.player.clothing;
													clothing5.shirtQuality -= 1;
													this.player.clothing.sendUpdateShirtQuality();
												}
												float num7 = itemClothingAsset5.armor + (1f - itemClothingAsset5.armor) * (1f - (float)this.player.clothing.shirtQuality / 100f);
												b = (byte)((float)b * num7);
											}
										}
									}
									else if (this.speciality == EZombieSpeciality.CRAWLER)
									{
										if (this.player.clothing.pants != 0)
										{
											ItemClothingAsset itemClothingAsset6 = (ItemClothingAsset)Assets.find(EAssetType.ITEM, this.player.clothing.pants);
											if (itemClothingAsset6 != null)
											{
												if (Provider.modeConfigData.Items.Has_Durability && this.player.clothing.pantsQuality > 0)
												{
													PlayerClothing clothing6 = this.player.clothing;
													clothing6.pantsQuality -= 1;
													this.player.clothing.sendUpdatePantsQuality();
												}
												float num8 = itemClothingAsset6.armor + (1f - itemClothingAsset6.armor) * (1f - (float)this.player.clothing.pantsQuality / 100f);
												b = (byte)((float)b * num8);
											}
										}
									}
									else if (this.speciality == EZombieSpeciality.SPRINTER)
									{
										if (this.player.clothing.vest != 0)
										{
											ItemClothingAsset itemClothingAsset7 = (ItemClothingAsset)Assets.find(EAssetType.ITEM, this.player.clothing.vest);
											if (itemClothingAsset7 != null)
											{
												if (Provider.modeConfigData.Items.Has_Durability && this.player.clothing.vestQuality > 0)
												{
													PlayerClothing clothing7 = this.player.clothing;
													clothing7.vestQuality -= 1;
													this.player.clothing.sendUpdateVestQuality();
												}
												float num9 = itemClothingAsset7.armor + (1f - itemClothingAsset7.armor) * (1f - (float)this.player.clothing.vestQuality / 100f);
												b = (byte)((float)b * num9);
											}
										}
										else if (this.player.clothing.shirt != 0)
										{
											ItemClothingAsset itemClothingAsset8 = (ItemClothingAsset)Assets.find(EAssetType.ITEM, this.player.clothing.shirt);
											if (itemClothingAsset8 != null)
											{
												if (Provider.modeConfigData.Items.Has_Durability && this.player.clothing.shirtQuality > 0)
												{
													PlayerClothing clothing8 = this.player.clothing;
													clothing8.shirtQuality -= 1;
													this.player.clothing.sendUpdateShirtQuality();
												}
												float num10 = itemClothingAsset8.armor + (1f - itemClothingAsset8.armor) * (1f - (float)this.player.clothing.shirtQuality / 100f);
												b = (byte)((float)b * num10);
											}
										}
										else if (this.player.clothing.pants != 0)
										{
											ItemClothingAsset itemClothingAsset9 = (ItemClothingAsset)Assets.find(EAssetType.ITEM, this.player.clothing.pants);
											if (itemClothingAsset9 != null)
											{
												if (Provider.modeConfigData.Items.Has_Durability && this.player.clothing.pantsQuality > 0)
												{
													PlayerClothing clothing9 = this.player.clothing;
													clothing9.pantsQuality -= 1;
													this.player.clothing.sendUpdatePantsQuality();
												}
												float num11 = itemClothingAsset9.armor + (1f - itemClothingAsset9.armor) * (1f - (float)this.player.clothing.pantsQuality / 100f);
												b = (byte)((float)b * num11);
											}
										}
									}
									EPlayerKill eplayerKill;
									this.player.life.askDamage(b, (this.target.position - base.transform.position).normalized * (float)b, EDeathCause.ZOMBIE, ELimb.SKULL, Provider.server, out eplayerKill);
									this.player.life.askInfect((byte)((float)(b / 3) * (1f - this.player.skills.mastery(1, 2) * 0.5f)));
								}
							}
						}
						else if (Time.time - this.lastAttack > 1f)
						{
							this.isAttacking = true;
							if (this.speciality == EZombieSpeciality.CRAWLER)
							{
								ZombieManager.sendZombieAttack(this, 5);
							}
							else if (this.speciality == EZombieSpeciality.SPRINTER)
							{
								ZombieManager.sendZombieAttack(this, (byte)Random.Range(6, 9));
							}
							else
							{
								ZombieManager.sendZombieAttack(this, (byte)Random.Range(0, 5));
							}
						}
					}
				}
				else
				{
					this.lastTarget = Time.time;
					this.isAttacking = false;
				}
			}
			if (this.seeker != null)
			{
				this.seeker.move(delta);
			}
		}

		private void Update()
		{
			if (this.isDead)
			{
				return;
			}
			if (Provider.isServer)
			{
				if (!this.isUpdated)
				{
					if (Mathf.Abs(this.lastUpdatedPos.x - base.transform.position.x) > Provider.UPDATE_DISTANCE || Mathf.Abs(this.lastUpdatedPos.y - base.transform.position.y) > Provider.UPDATE_DISTANCE || Mathf.Abs(this.lastUpdatedPos.z - base.transform.position.z) > Provider.UPDATE_DISTANCE || Mathf.Abs(this.lastUpdatedAngle - base.transform.rotation.eulerAngles.y) > 1f)
					{
						this.lastUpdatedPos = base.transform.position;
						this.lastUpdatedAngle = base.transform.rotation.eulerAngles.y;
						this.isUpdated = true;
						ZombieRegion zombieRegion = ZombieManager.regions[(int)this.bound];
						zombieRegion.updates += 1;
						this.isStuck = false;
						this.lastStuck = Time.time;
					}
					else if (this.isMoving)
					{
						this.isStuck = true;
					}
				}
				if (this.isPulled && Time.time - this.lastPull > this.pullDelay)
				{
					this.lastPull = Time.time;
					this.pullDelay = Random.Range(24f, 96f);
					if (!this.isLeaving && ZombieManager.canSpareWanderer)
					{
						float num = Random.value * 3.14159274f * 2f;
						float num2 = Random.Range(0.5f, 1f);
						this.isWandering = true;
						ZombieManager.wanderingCount++;
						this.isPulled = false;
						this.alert(this.cameFrom + new Vector3(Mathf.Cos(num) * num2, 0f, Mathf.Sin(num) * num2), false);
					}
				}
			}
			else
			{
				if (Mathf.Abs(this.lastUpdatedPos.x - base.transform.position.x) > 0.01f || Mathf.Abs(this.lastUpdatedPos.y - base.transform.position.y) > 0.01f || Mathf.Abs(this.lastUpdatedPos.z - base.transform.position.z) > 0.01f)
				{
					this.isMoving = true;
				}
				else
				{
					this.isMoving = false;
				}
				if (this.nsb != null)
				{
					YawSnapshotInfo yawSnapshotInfo = (YawSnapshotInfo)this.nsb.getCurrentSnapshot();
					base.transform.position = yawSnapshotInfo.pos;
					base.transform.rotation = Quaternion.Euler(0f, yawSnapshotInfo.yaw, 0f);
				}
			}
			if ((this.isThrowingBoulder || this.isSpittingAcid || this.isBreathingFire || this.isChargingSpark) && Provider.isServer && this.player != null)
			{
				Vector3 normalized = (this.player.transform.position - base.transform.position).normalized;
				normalized.y = 0f;
				Quaternion quaternion = Quaternion.LookRotation(normalized);
				if (Dedicator.isDedicated)
				{
					base.transform.rotation = quaternion;
				}
				else
				{
					base.transform.rotation = Quaternion.Lerp(base.transform.rotation, quaternion, 4f * Time.deltaTime);
				}
			}
			if (this.isThrowingBoulder && Time.time - this.lastSpecial > this.throwTime)
			{
				this.isThrowingBoulder = false;
				if (this.boulderItem != null)
				{
					Object.Destroy(this.boulderItem.gameObject);
				}
				if (Provider.isServer)
				{
					this.seeker.canMove = true;
					if (this.player != null)
					{
						Vector3 vector = this.player.transform.position - base.transform.position;
						float magnitude = vector.magnitude;
						vector += Vector3.up * magnitude * 0.1f;
						Vector3 direction = vector / magnitude;
						ZombieManager.sendZombieBoulder(this, base.transform.position + Vector3.up * base.transform.localScale.y * 1.9f, direction);
					}
					else
					{
						ZombieManager.sendZombieBoulder(this, base.transform.position + Vector3.up * base.transform.localScale.y * 1.9f, Vector3.forward);
					}
				}
			}
			if (this.isSpittingAcid && Time.time - this.lastSpecial > this.acidTime)
			{
				this.isSpittingAcid = false;
				if (Provider.isServer)
				{
					this.seeker.canMove = true;
					if (this.player != null)
					{
						Vector3 vector2 = this.player.transform.position - base.transform.position;
						float magnitude2 = vector2.magnitude;
						vector2 += Vector3.up * magnitude2 * 0.25f;
						Vector3 direction2 = vector2 / magnitude2;
						ZombieManager.sendZombieAcid(this, base.transform.position + Vector3.up * base.transform.localScale.y * 1.75f, direction2);
					}
					else
					{
						ZombieManager.sendZombieAcid(this, base.transform.position + Vector3.up * base.transform.localScale.y * 1.75f, Vector3.forward);
					}
				}
			}
			if (this.isChargingSpark && Time.time - this.lastSpecial > this.sparkTime)
			{
				this.isChargingSpark = false;
				if (Provider.isServer && this.player != null)
				{
					Vector3 vector3 = this.player.look.aim.position;
					Vector3 vector4 = vector3 - (base.transform.position + new Vector3(0f, 2f, 0f));
					RaycastHit raycastHit;
					if (PhysicsUtility.raycast(new Ray(base.transform.position + new Vector3(0f, 2f, 0f), vector4), out raycastHit, vector4.magnitude - 0.025f, RayMasks.BLOCK_SENTRY, 0))
					{
						vector3 = raycastHit.point + raycastHit.normal;
					}
					DamageTool.explode(vector3, 5f, EDeathCause.SPARK, CSteamID.Nil, 25f, 0f, 0f, 250f, 250f, 250f, 250f, 250f, EExplosionDamageType.ZOMBIE_ELECTRIC, 4f, true);
					ZombieManager.sendZombieSpark(this, vector3);
				}
			}
			if (this.isStompingWind && Time.time - this.lastSpecial > this.windTime)
			{
				this.isStompingWind = false;
				if (Provider.isServer)
				{
					this.seeker.canMove = true;
					DamageTool.explode(base.transform.position + new Vector3(0f, 1.5f, 0f), 10f, EDeathCause.BOULDER, CSteamID.Nil, 60f, 0f, 0f, 500f, 500f, 500f, 500f, 500f, EExplosionDamageType.ZOMBIE_ACID, 32f, true);
					EffectManager.sendEffect(52, EffectManager.MEDIUM, base.transform.position, Vector3.up);
				}
			}
			if (this.isBreathingFire)
			{
				if (Provider.isServer && this.isBreathingFire)
				{
					this.fireDamage += Time.deltaTime * 50f;
					if (this.fireDamage > 1f)
					{
						float playerDamage = this.fireDamage;
						float num3 = this.fireDamage * 10f;
						this.fireDamage = 0f;
						DamageTool.explode(base.transform.position + new Vector3(0f, 1.25f, 0f) + base.transform.forward * 3f, 2f, EDeathCause.BURNER, CSteamID.Nil, playerDamage, 0f, 0f, num3, num3, num3, num3, num3, EExplosionDamageType.ZOMBIE_FIRE, 4f, false);
						DamageTool.explode(base.transform.position + new Vector3(0f, 1.25f, 0f) + base.transform.forward * 7f, 3f, EDeathCause.BURNER, CSteamID.Nil, playerDamage, 0f, 0f, num3, num3, num3, num3, num3, EExplosionDamageType.ZOMBIE_FIRE, 4f, false);
						DamageTool.explode(base.transform.position + new Vector3(0f, 1.25f, 0f) + base.transform.forward * 12f, 4f, EDeathCause.BURNER, CSteamID.Nil, playerDamage, 0f, 0f, num3, num3, num3, num3, num3, EExplosionDamageType.ZOMBIE_FIRE, 4f, false);
					}
				}
				if (Time.time - this.lastSpecial > this.fireTime)
				{
					this.isBreathingFire = false;
					if (this.fireSystem != null)
					{
						this.fireSystem.emission.enabled = false;
					}
					if (Provider.isServer)
					{
						this.seeker.canMove = true;
					}
				}
			}
			if (this.isPlayingBoulder)
			{
				if (Time.time - this.lastSpecial > this.boulderTime)
				{
					this.isPlayingBoulder = false;
				}
			}
			else if (this.isPlayingSpit)
			{
				if (Time.time - this.lastSpecial > this.spitTime)
				{
					this.isPlayingSpit = false;
				}
			}
			else if (this.isPlayingCharge)
			{
				if (Time.time - this.lastSpecial > this.chargeTime)
				{
					this.isPlayingCharge = false;
					if (Provider.isServer)
					{
						this.seeker.canMove = true;
					}
				}
			}
			else if (this.isPlayingWind)
			{
				if (Time.time - this.lastSpecial > this.windTime)
				{
					this.isPlayingWind = false;
				}
			}
			else if (this.isPlayingFire)
			{
				if (Time.time - this.lastSpecial > this.fireTime)
				{
					this.isPlayingFire = false;
				}
			}
			else if (this.isPlayingAttack)
			{
				if (Time.time - this.lastAttack > this.attackTime)
				{
					if (this.speciality == EZombieSpeciality.FLANKER_FRIENDLY || this.speciality == EZombieSpeciality.FLANKER_STALK)
					{
						this.updateVisibility(false, true);
					}
					this.isPlayingAttack = false;
				}
			}
			else if (this.isPlayingStartle)
			{
				if (Time.time - this.lastStartle > this.startleTime)
				{
					this.isPlayingStartle = false;
				}
			}
			else if (this.isPlayingStun)
			{
				if (Time.time - this.lastStun > this.stunTime)
				{
					this.isPlayingStun = false;
				}
			}
			else if (!Dedicator.isDedicated)
			{
				if (this.isMoving && (!Provider.isServer || !this.isStuck))
				{
					if (this.speciality == EZombieSpeciality.CRAWLER)
					{
						this.animator.CrossFade("Move_4", CharacterAnimator.BLEND);
					}
					else if (this.speciality == EZombieSpeciality.SPRINTER)
					{
						this.animator.CrossFade("Move_5", CharacterAnimator.BLEND);
					}
					else
					{
						this.animator.CrossFade(this.moveAnim, CharacterAnimator.BLEND);
					}
				}
				else if (this.speciality == EZombieSpeciality.CRAWLER)
				{
					this.animator.CrossFade("Idle_3", CharacterAnimator.BLEND);
				}
				else if (this.speciality == EZombieSpeciality.SPRINTER)
				{
					this.animator.CrossFade("Idle_4", CharacterAnimator.BLEND);
				}
				else
				{
					this.animator.CrossFade(this.idleAnim, CharacterAnimator.BLEND);
				}
			}
			if (Provider.isServer && this.health < this.maxHealth && Time.time - this.lastRegen > LevelZombies.tables[(int)this.type].regen)
			{
				this.lastRegen = Time.time;
				this.health += 1;
			}
			if (!Dedicator.isDedicated && Time.time - this.lastGroan > this.groanDelay)
			{
				this.lastGroan = Time.time;
				if (this.isVisible)
				{
					if (this.isMega)
					{
						this.groanDelay = Random.Range(2f, 4f);
					}
					else
					{
						this.groanDelay = Random.Range(4f, 8f);
					}
					if (!this.isMoving)
					{
						if ((double)Random.value > 0.8)
						{
							if (this.isMega)
							{
								base.GetComponent<AudioSource>().pitch = Random.Range(0.5f, 0.7f);
							}
							else
							{
								base.GetComponent<AudioSource>().pitch = Random.Range(0.9f, 1.1f);
							}
							if (this.isHyper)
							{
								base.GetComponent<AudioSource>().pitch *= 0.9f;
							}
							AudioClip audioClip = ZombieManager.groans[Random.Range(0, 5)];
							base.GetComponent<AudioSource>().PlayOneShot(audioClip);
						}
					}
					else
					{
						if (this.isMega)
						{
							base.GetComponent<AudioSource>().pitch = Random.Range(0.5f, 0.7f);
						}
						else
						{
							base.GetComponent<AudioSource>().pitch = Random.Range(0.9f, 1.1f);
						}
						if (this.isHyper)
						{
							base.GetComponent<AudioSource>().pitch *= 0.9f;
						}
						AudioClip audioClip2 = ZombieManager.roars[Random.Range(0, 16)];
						base.GetComponent<AudioSource>().PlayOneShot(audioClip2);
					}
				}
			}
			if (Provider.isServer)
			{
				if (this.isStunned)
				{
					if (Time.time - this.lastStun <= 1f)
					{
						return;
					}
					this.lastTarget = Time.time;
					this.lastStuck = Time.time;
					this.isStunned = false;
					this.seeker.canMove = true;
				}
				if (this.isLeaving && Time.time - this.lastLeave > this.leaveTime)
				{
					this.alert(this.leaveTo, false);
					this.isLeaving = false;
				}
			}
		}

		private void onHyperUpdated(bool isHyper)
		{
			if (this.eyes != null)
			{
				this.eyes.gameObject.SetActive(isHyper);
			}
		}

		public void init()
		{
			this.awake();
			this.start();
		}

		private void start()
		{
			if (Provider.isServer)
			{
				this.seeker = base.GetComponent<AIPath>();
				this.target = base.transform.FindChild("Target");
				this.target.parent = LevelNavigation.models;
				this.seeker.target = this.target;
				this.seeker.canSmooth = !Dedicator.isDedicated;
				this.reset();
			}
			else
			{
				this.lastUpdatedPos = base.transform.position;
				this.lastUpdatedAngle = base.transform.rotation.eulerAngles.y;
				this.nsb = new NetworkSnapshotBuffer(Provider.UPDATE_TIME, Provider.UPDATE_DELAY);
			}
			this.lastGroan = Time.time + Random.Range(4f, 16f);
			if (this.isMega)
			{
				this.groanDelay = Random.Range(2f, 4f);
			}
			else
			{
				this.groanDelay = Random.Range(4f, 8f);
			}
			this.updateLife();
			this.apply();
			this.updateEffects();
			this.updateVisibility(this.speciality != EZombieSpeciality.FLANKER_STALK, false);
			this.updateStates();
			if (!Dedicator.isDedicated)
			{
				ZombieRegion zombieRegion = ZombieManager.regions[(int)this.bound];
				zombieRegion.onHyperUpdated = (HyperUpdated)Delegate.Combine(zombieRegion.onHyperUpdated, new HyperUpdated(this.onHyperUpdated));
			}
		}

		private void awake()
		{
			this.throwTime = 1f;
			this.acidTime = 1f;
			this.windTime = 0.9f;
			this.fireTime = 2.75f;
			this.chargeTime = 1.8f;
			this.sparkTime = 1.2f;
			if (Dedicator.isDedicated)
			{
				this.boulderTime = 1f;
				this.spitTime = 1f;
				this.attackTime = 0.5f;
				this.startleTime = 0.5f;
				this.stunTime = 0.5f;
			}
			else
			{
				this.animator = base.transform.FindChild("Character").GetComponent<Animation>();
				this.skeleton = this.animator.transform.FindChild("Skeleton");
				this.rightHook = this.skeleton.FindChild("Spine").FindChild("Right_Shoulder").FindChild("Right_Arm").FindChild("Right_Hand").FindChild("Right_Hook");
				this.renderer_0 = this.animator.transform.FindChild("Model_0").GetComponent<SkinnedMeshRenderer>();
				this.renderer_1 = this.animator.transform.FindChild("Model_1").GetComponent<SkinnedMeshRenderer>();
				this.eyes = this.skeleton.FindChild("Spine").FindChild("Skull").FindChild("Eyes");
				this.radiation = this.skeleton.FindChild("Spine").FindChild("Radiation");
				this.burner = this.skeleton.FindChild("Spine").FindChild("Burner");
				this.acid = this.skeleton.FindChild("Spine").FindChild("Skull").FindChild("Acid");
				this.electric = this.skeleton.FindChild("Spine").FindChild("Electric");
				this.sparkSystem = this.rightHook.FindChild("Spark").GetComponent<ParticleSystem>();
				this.fireSystem = this.skeleton.FindChild("Spine").FindChild("Skull").FindChild("Fire").GetComponent<ParticleSystem>();
				this.fireAudio = this.skeleton.FindChild("Spine").FindChild("Skull").FindChild("Fire").GetComponent<AudioSource>();
				this.boulderTime = this.animator["Boulder_0"].clip.length;
				this.spitTime = this.animator["Acid_0"].clip.length;
				this.attackTime = this.animator["Attack_0"].clip.length;
				this.startleTime = this.animator["Startle_0"].clip.length;
				this.stunTime = this.animator["Stun_0"].clip.length;
			}
		}

		private void OnDestroy()
		{
			if (Provider.isServer)
			{
				this.isHunting = false;
			}
			if (!Dedicator.isDedicated)
			{
				ZombieRegion zombieRegion = ZombieManager.regions[(int)this.bound];
				zombieRegion.onHyperUpdated = (HyperUpdated)Delegate.Remove(zombieRegion.onHyperUpdated, new HyperUpdated(this.onHyperUpdated));
			}
		}

		private static List<RegionCoordinate> regionsInRadius = new List<RegionCoordinate>(4);

		private static List<Transform> structuresInRadius = new List<Transform>();

		private static List<InteractableVehicle> vehiclesInRadius = new List<InteractableVehicle>();

		private static List<Transform> barricadesInRadius = new List<Transform>();

		private static readonly float ATTACK_BARRICADE = 16f;

		private static readonly float ATTACK_VEHICLE = 16f;

		private static readonly float ATTACK_PLAYER = 2f;

		private Transform skeleton;

		private Transform rightHook;

		private SkinnedMeshRenderer renderer_0;

		private SkinnedMeshRenderer renderer_1;

		private Transform eyes;

		private Transform radiation;

		private Transform burner;

		private Transform acid;

		private Transform electric;

		private ParticleSystem sparkSystem;

		private ParticleSystem fireSystem;

		private AudioSource fireAudio;

		private Material skinMaterial;

		private Transform attachmentModel_0;

		private Transform attachmentModel_1;

		private Material attachmentMaterial_0;

		private Material attachmentMaterial_1;

		public ushort id;

		public byte bound;

		public byte type;

		public EZombieSpeciality speciality;

		public byte shirt;

		public byte pants;

		public byte hat;

		public byte gear;

		private byte _move;

		private string moveAnim;

		private byte _idle;

		public string idleAnim;

		public bool isUpdated;

		private AIPath seeker;

		private Player player;

		private Transform barricade;

		private Transform structure;

		private InteractableVehicle vehicle;

		private InteractableVehicle drive;

		private Transform target;

		private Animation animator;

		private float lastHunted;

		private float lastTarget;

		private float lastLeave;

		private float lastRelocate;

		private float lastSpecial;

		private float lastAttack;

		private float lastStartle;

		private float lastStun;

		private float lastGroan;

		private float lastRegen;

		private float lastStuck;

		private Vector3 cameFrom;

		private bool isPulled;

		private float lastPull;

		private float pullDelay;

		private float groanDelay;

		private float leaveTime;

		private float throwTime;

		private float boulderTime;

		private float spitTime;

		private float acidTime;

		private float chargeTime;

		private float sparkTime;

		private float windTime;

		private float fireTime;

		private float attackTime;

		private float startleTime;

		private float stunTime;

		private bool isThrowRelocating;

		private bool isThrowingBoulder;

		private bool isSpittingAcid;

		private bool isChargingSpark;

		private bool isStompingWind;

		private bool isBreathingFire;

		private bool isPlayingBoulder;

		private bool isPlayingSpit;

		private bool isPlayingCharge;

		private bool isPlayingWind;

		private bool isPlayingFire;

		private bool isPlayingAttack;

		private bool isPlayingStartle;

		private bool isPlayingStun;

		private Vector3 lastUpdatedPos;

		private float lastUpdatedAngle;

		private NetworkSnapshotBuffer nsb;

		private bool isMoving;

		private bool isAttacking;

		private bool isVisible;

		private bool isWandering;

		private bool isTicking;

		private bool _isHunting;

		private EHuntType huntType;

		private bool isLeaving;

		private bool isStunned;

		private bool isStuck;

		private Vector3 leaveTo;

		private float _lastDead;

		public bool isDead;

		private ushort health;

		private ushort maxHealth;

		private Vector3 ragdoll;

		private EZombiePath path;

		private float specialStartleDelay;

		private float specialAttackDelay;

		private float specialUseDelay;

		private float boulderThrowDelay;

		private Transform boulderItem;

		private float fireDamage;

		private float lastTick;
	}
}
