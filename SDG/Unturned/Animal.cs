using System;
using System.Collections.Generic;
using SDG.Framework.Water;
using UnityEngine;

namespace SDG.Unturned
{
	public class Animal : MonoBehaviour
	{
		public Vector3 target { get; private set; }

		private void updateTicking()
		{
			if (this.isFleeing || this.isWandering || this.isHunting)
			{
				if (this.isTicking)
				{
					return;
				}
				this.isTicking = true;
				AnimalManager.tickingAnimals.Add(this);
				this.lastTick = Time.time;
			}
			else
			{
				if (!this.isTicking)
				{
					return;
				}
				this.isTicking = false;
				AnimalManager.tickingAnimals.RemoveFast(this);
			}
		}

		public bool isFleeing
		{
			get
			{
				return this._isFleeing;
			}
		}

		public float lastDead
		{
			get
			{
				return this._lastDead;
			}
		}

		public AnimalAsset asset
		{
			get
			{
				return this._asset;
			}
		}

		public void askEat()
		{
			if (this.isDead)
			{
				return;
			}
			this.lastEat = Time.time;
			this.eatDelay = Random.Range(4f, 8f);
			this.isPlayingEat = true;
			if (!Dedicator.isDedicated)
			{
				this.animator.Play("Eat");
			}
		}

		public void askGlance()
		{
			if (this.isDead)
			{
				return;
			}
			this.lastGlance = Time.time;
			this.glanceDelay = Random.Range(4f, 8f);
			this.isPlayingGlance = true;
			if (!Dedicator.isDedicated)
			{
				this.animator.Play("Glance_" + Random.Range(0, 2));
			}
		}

		public void askStartle()
		{
			if (this.isDead)
			{
				return;
			}
			this.lastStartle = Time.time;
			this.isPlayingStartle = true;
			if (!Dedicator.isDedicated)
			{
				this.animator.Play("Startle");
			}
		}

		public void askAttack()
		{
			if (this.isDead)
			{
				return;
			}
			this.lastAttack = Time.time;
			this.isPlayingAttack = true;
			if (!Dedicator.isDedicated)
			{
				if (this.animator["Attack"] != null)
				{
					this.animator.Play("Attack");
				}
				if (this.asset != null && this.asset.roars != null && this.asset.roars.Length > 0 && Time.time - this.startedRoar > 1f)
				{
					this.startedRoar = Time.time;
					base.GetComponent<AudioSource>().pitch = Random.Range(0.9f, 1.1f);
					base.GetComponent<AudioSource>().PlayOneShot(this.asset.roars[Random.Range(0, this.asset.roars.Length)]);
				}
			}
		}

		public void askPanic()
		{
			if (this.isDead)
			{
				return;
			}
			if (!Dedicator.isDedicated && this.asset != null && this.asset.panics != null && this.asset.panics.Length > 0 && Time.time - this.startedPanic > 1f)
			{
				this.startedPanic = Time.time;
				base.GetComponent<AudioSource>().pitch = Random.Range(0.9f, 1.1f);
				base.GetComponent<AudioSource>().PlayOneShot(this.asset.panics[Random.Range(0, this.asset.panics.Length)]);
			}
		}

		public void askDamage(byte amount, Vector3 newRagdoll, out EPlayerKill kill, out uint xp)
		{
			kill = EPlayerKill.NONE;
			xp = 0u;
			if (amount == 0 || this.isDead)
			{
				return;
			}
			if (!this.isDead)
			{
				if ((ushort)amount >= this.health)
				{
					this.health = 0;
				}
				else
				{
					this.health -= (ushort)amount;
				}
				this.ragdoll = newRagdoll;
				if (this.health == 0)
				{
					kill = EPlayerKill.ANIMAL;
					if (this.asset != null)
					{
						xp = this.asset.rewardXP;
					}
					AnimalManager.dropLoot(this);
					AnimalManager.sendAnimalDead(this, this.ragdoll);
				}
				else if (this.asset != null && this.asset.panics != null && this.asset.panics.Length > 0)
				{
					AnimalManager.sendAnimalPanic(this);
				}
				this.lastRegen = Time.time;
			}
		}

		public void sendRevive(Vector3 position, float angle)
		{
			AnimalManager.sendAnimalAlive(this, position, MeasurementTool.angleToByte(angle));
		}

		private bool checkTargetValid(Vector3 point)
		{
			if (!Level.checkSafeIncludingClipVolumes(point))
			{
				return false;
			}
			float height = LevelGround.getHeight(point);
			return !WaterUtility.isPointUnderwater(new Vector3(point.x, height - 1f, point.z));
		}

		private Vector3 getFleeTarget(Vector3 normal)
		{
			Vector3 vector = base.transform.position + normal * 64f + new Vector3(Random.Range(-8f, 8f), 0f, Random.Range(-8f, 8f));
			if (!this.checkTargetValid(vector))
			{
				vector = base.transform.position + normal * 32f + new Vector3(Random.Range(-8f, 8f), 0f, Random.Range(-8f, 8f));
				if (!this.checkTargetValid(vector))
				{
					vector = base.transform.position + normal * -32f + new Vector3(Random.Range(-8f, 8f), 0f, Random.Range(-8f, 8f));
					if (!this.checkTargetValid(vector))
					{
						vector = base.transform.position + normal * -16f + new Vector3(Random.Range(-8f, 8f), 0f, Random.Range(-8f, 8f));
					}
				}
			}
			return vector;
		}

		private void getWanderTarget()
		{
			Vector3 vector;
			if (this.isStuck)
			{
				vector = base.transform.position + new Vector3(Random.Range(-8f, 8f), 0f, Random.Range(-8f, 8f));
				if (!this.checkTargetValid(vector))
				{
					return;
				}
			}
			else if ((base.transform.position - this.pack.getAverageAnimalPoint()).sqrMagnitude > 256f)
			{
				vector = this.pack.getAverageAnimalPoint() + new Vector3(Random.Range(-8f, 8f), 0f, Random.Range(-8f, 8f));
			}
			else
			{
				Vector3 wanderDirection = this.pack.getWanderDirection();
				vector = base.transform.position + wanderDirection * Random.Range(6f, 8f) + new Vector3(Random.Range(-4f, 4f), 0f, Random.Range(-4f, 4f));
				if (!this.checkTargetValid(vector))
				{
					vector = base.transform.position - wanderDirection * Random.Range(6f, 8f) + new Vector3(Random.Range(-4f, 4f), 0f, Random.Range(-4f, 4f));
					if (!this.checkTargetValid(vector))
					{
						return;
					}
					this.pack.wanderAngle += Random.Range(160f, 200f);
				}
				else
				{
					this.pack.wanderAngle += Random.Range(-20f, 20f);
				}
			}
			this.target = vector;
			this.isWandering = true;
			this.updateTicking();
		}

		public bool checkAlert(Player newPlayer)
		{
			return this.player != newPlayer;
		}

		public void alertPlayer(Player newPlayer, bool sendToPack)
		{
			if (sendToPack)
			{
				for (int i = 0; i < this.pack.animals.Count; i++)
				{
					Animal animal = this.pack.animals[i];
					if (!(animal == null) && !(animal == this))
					{
						animal.alertPlayer(newPlayer, false);
					}
				}
			}
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
				this._isFleeing = false;
				this.isWandering = false;
				this.isHunting = true;
				this.updateTicking();
				this.lastStuck = Time.time;
				this.player = newPlayer;
			}
			else if ((newPlayer.transform.position - base.transform.position).sqrMagnitude < (this.player.transform.position - base.transform.position).sqrMagnitude)
			{
				this._isFleeing = false;
				this.isWandering = false;
				this.isHunting = true;
				this.updateTicking();
				this.player = newPlayer;
			}
		}

		public void alertPoint(Vector3 newPosition, bool sendToPack)
		{
			this.alertDirection((base.transform.position - newPosition).normalized, sendToPack);
		}

		public void alertDirection(Vector3 newDirection, bool sendToPack)
		{
			if (sendToPack)
			{
				for (int i = 0; i < this.pack.animals.Count; i++)
				{
					Animal animal = this.pack.animals[i];
					if (!(animal == null) && !(animal == this))
					{
						animal.alertDirection(newDirection, false);
					}
				}
			}
			if (this.isDead)
			{
				return;
			}
			if (this.isStuck)
			{
				return;
			}
			if (this.isHunting)
			{
				return;
			}
			if (!this.isFleeing)
			{
				AnimalManager.sendAnimalStartle(this);
			}
			this._isFleeing = true;
			this.isWandering = false;
			this.isHunting = false;
			this.updateTicking();
			this.target = this.getFleeTarget(newDirection);
		}

		private void stop()
		{
			this.isMoving = false;
			this.isRunning = false;
			this._isFleeing = false;
			this.isWandering = false;
			this.isHunting = false;
			this.updateTicking();
			this.isStuck = false;
			this.player = null;
			this.target = base.transform.position;
		}

		public void tellAlive(Vector3 newPosition, byte newAngle)
		{
			this.isDead = false;
			base.transform.position = newPosition;
			base.transform.rotation = Quaternion.Euler(0f, (float)(newAngle * 2), 0f);
			this.updateLife();
			this.updateStates();
			this.reset();
		}

		public void tellDead(Vector3 newRagdoll)
		{
			this.isDead = true;
			this._lastDead = Time.realtimeSinceStartup;
			this.updateLife();
			if (!Dedicator.isDedicated)
			{
				this.ragdoll = newRagdoll;
				RagdollTool.ragdollAnimal(base.transform.position, base.transform.rotation, this.skeleton, this.ragdoll, this.id);
			}
			if (Provider.isServer)
			{
				this.stop();
			}
		}

		public void tellState(Vector3 newPosition, byte newAngle)
		{
			this.lastUpdatePos = newPosition;
			this.lastUpdateAngle = (float)newAngle * 2f;
			if (this.nsb != null)
			{
				this.nsb.addNewSnapshot(new YawSnapshotInfo(newPosition, (float)newAngle * 2f));
			}
			if (this.isPlayingEat || this.isPlayingGlance)
			{
				this.isPlayingEat = false;
				this.isPlayingGlance = false;
				this.animator.Stop();
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
				base.GetComponent<Collider>().enabled = !this.isDead;
			}
		}

		public void updateStates()
		{
			this.lastUpdatePos = base.transform.position;
			this.lastUpdateAngle = base.transform.rotation.eulerAngles.y;
			if (this.nsb != null)
			{
				this.nsb.updateLastSnapshot(new YawSnapshotInfo(base.transform.position, base.transform.rotation.eulerAngles.y));
			}
		}

		private void reset()
		{
			this.target = base.transform.position;
			this.lastStartle = Time.time;
			this.lastWander = Time.time;
			this.lastStuck = Time.time;
			this.isPlayingEat = false;
			this.isPlayingGlance = false;
			this.isPlayingStartle = false;
			this.isMoving = false;
			this.isRunning = false;
			this._isFleeing = false;
			this.isWandering = false;
			this.isHunting = false;
			this.updateTicking();
			this.isStuck = false;
			this._asset = (AnimalAsset)Assets.find(EAssetType.ANIMAL, this.id);
			this.health = this.asset.health;
		}

		private void move(float delta)
		{
			Vector3 vector = this.target - base.transform.position;
			vector.y = 0f;
			Vector3 vector2 = vector;
			float magnitude = vector.magnitude;
			bool flag = magnitude > 0.75f;
			if (!Dedicator.isDedicated && flag && !this.isMoving)
			{
				if (this.isPlayingEat)
				{
					this.animator.Stop();
					this.isPlayingEat = false;
				}
				if (this.isPlayingGlance)
				{
					this.animator.Stop();
					this.isPlayingGlance = false;
				}
				if (this.isPlayingStartle)
				{
					this.animator.Stop();
					this.isPlayingStartle = false;
				}
			}
			this.isMoving = flag;
			this.isRunning = (this.isMoving && (this.isFleeing || this.isHunting));
			float num = Mathf.Clamp01(magnitude / 0.6f);
			Vector3 forward = base.transform.forward;
			float num2 = Vector3.Dot(vector.normalized, forward);
			float num3 = ((!this.isRunning) ? this.asset.speedWalk : this.asset.speedRun) * Mathf.Max(num2, 0.05f) * num;
			if (Time.deltaTime > 0f)
			{
				num3 = Mathf.Clamp(num3, 0f, magnitude / (Time.deltaTime * 2f));
			}
			vector = base.transform.forward * num3;
			vector.y = Physics.gravity.y * 2f;
			if (!this.isMoving)
			{
				vector.x = 0f;
				vector.z = 0f;
				if (!this.isStuck)
				{
					this._isFleeing = false;
					this.isWandering = false;
					this.updateTicking();
				}
			}
			else
			{
				Quaternion quaternion = base.transform.rotation;
				Quaternion quaternion2 = Quaternion.LookRotation(vector2);
				Vector3 eulerAngles = Quaternion.Slerp(quaternion, quaternion2, 8f * delta).eulerAngles;
				eulerAngles.z = 0f;
				eulerAngles.x = 0f;
				quaternion = Quaternion.Euler(eulerAngles);
				base.transform.rotation = quaternion;
			}
			this.controller.Move(vector * delta);
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
					if (Mathf.Abs(this.lastUpdatePos.x - base.transform.position.x) > Provider.UPDATE_DISTANCE || Mathf.Abs(this.lastUpdatePos.y - base.transform.position.y) > Provider.UPDATE_DISTANCE || Mathf.Abs(this.lastUpdatePos.z - base.transform.position.z) > Provider.UPDATE_DISTANCE || Mathf.Abs(this.lastUpdateAngle - base.transform.rotation.eulerAngles.y) > 1f)
					{
						this.lastUpdatePos = base.transform.position;
						this.lastUpdateAngle = base.transform.rotation.eulerAngles.y;
						this.isUpdated = true;
						AnimalManager.updates += 1;
						if (this.isStuck && Time.time - this.lastStuck > 0.5f)
						{
							this.isStuck = false;
							this.lastStuck = Time.time;
						}
					}
					else if (this.isMoving)
					{
						if (Time.time - this.lastStuck > 0.125f)
						{
							this.isStuck = true;
						}
					}
					else
					{
						this.isStuck = false;
						this.lastStuck = Time.time;
					}
				}
			}
			else
			{
				if (Mathf.Abs(this.lastUpdatePos.x - base.transform.position.x) > 0.01f || Mathf.Abs(this.lastUpdatePos.y - base.transform.position.y) > 0.01f || Mathf.Abs(this.lastUpdatePos.z - base.transform.position.z) > 0.01f)
				{
					if (!this.isMoving)
					{
						if (this.isPlayingEat)
						{
							this.animator.Stop();
							this.isPlayingEat = false;
						}
						if (this.isPlayingGlance)
						{
							this.animator.Stop();
							this.isPlayingGlance = false;
						}
						if (this.isPlayingStartle)
						{
							this.animator.Stop();
							this.isPlayingStartle = false;
						}
					}
					this.isMoving = true;
					this.isRunning = ((this.lastUpdatePos - base.transform.position).sqrMagnitude > 1f);
				}
				else
				{
					this.isMoving = false;
					this.isRunning = false;
				}
				if (this.nsb != null)
				{
					YawSnapshotInfo yawSnapshotInfo = (YawSnapshotInfo)this.nsb.getCurrentSnapshot();
					base.transform.position = yawSnapshotInfo.pos;
					base.transform.rotation = Quaternion.Euler(0f, yawSnapshotInfo.yaw, 0f);
				}
			}
			if (!Dedicator.isDedicated && !this.isMoving && !this.isPlayingEat && !this.isPlayingGlance && !this.isPlayingAttack)
			{
				if (Time.time - this.lastEat > this.eatDelay)
				{
					this.askEat();
				}
				else if (Time.time - this.lastGlance > this.glanceDelay)
				{
					this.askGlance();
				}
			}
			if (Provider.isServer)
			{
				if (this.isStuck)
				{
					if (Time.time - this.lastStuck > 0.75f)
					{
						this.lastStuck = Time.time;
						this.getWanderTarget();
					}
				}
				else if (!this.isFleeing && !this.isHunting)
				{
					if (Time.time - this.lastWander > this.wanderDelay)
					{
						this.lastWander = Time.time;
						this.wanderDelay = Random.Range(8f, 16f);
						this.getWanderTarget();
					}
				}
				else
				{
					this.lastWander = Time.time;
				}
			}
			if (this.isPlayingEat)
			{
				if (Time.time - this.lastEat > this.eatTime)
				{
					this.isPlayingEat = false;
				}
			}
			else if (this.isPlayingGlance)
			{
				if (Time.time - this.lastGlance > this.glanceTime)
				{
					this.isPlayingGlance = false;
				}
			}
			else if (this.isPlayingStartle)
			{
				if (Time.time - this.lastStartle > this.startleTime)
				{
					this.isPlayingStartle = false;
				}
			}
			else if (this.isPlayingAttack)
			{
				if (Time.time - this.lastAttack > this.attackTime)
				{
					this.isPlayingAttack = false;
				}
			}
			else if (!Dedicator.isDedicated)
			{
				if (this.isRunning)
				{
					this.animator.Play("Run");
				}
				else if (this.isMoving)
				{
					this.animator.Play("Walk");
				}
				else
				{
					this.animator.Play("Idle");
				}
			}
			if (Provider.isServer && this.health < this.asset.health && Time.time - this.lastRegen > this.asset.regen)
			{
				this.lastRegen = Time.time;
				this.health += 1;
			}
		}

		public void tick()
		{
			float delta = Time.time - this.lastTick;
			this.lastTick = Time.time;
			if (this.isHunting)
			{
				if (this.player != null && !this.player.life.isDead && this.player.stance.stance != EPlayerStance.SWIM)
				{
					this.target = this.player.transform.position;
					float num = Mathf.Pow(this.target.x - base.transform.position.x, 2f) + Mathf.Pow(this.target.z - base.transform.position.z, 2f);
					float num2 = Mathf.Abs(this.target.y - base.transform.position.y);
					if (num < (float)((!(this.player.movement.getVehicle() != null)) ? 5 : 19) && num2 < 2f)
					{
						if (Time.time - this.lastTarget > ((!Dedicator.isDedicated) ? 0.1f : 0.3f))
						{
							if (this.isAttacking)
							{
								if (Time.time - this.lastAttack > this.attackTime / 2f)
								{
									this.isAttacking = false;
									byte b = this.asset.damage;
									b = (byte)((float)b * Provider.modeConfigData.Animals.Damage_Multiplier);
									if (this.player.movement.getVehicle() != null)
									{
										VehicleManager.damage(this.player.movement.getVehicle(), (float)b, 1f, true);
									}
									else
									{
										EPlayerKill eplayerKill;
										this.player.life.askDamage(b, (this.target - base.transform.position).normalized * (float)b, EDeathCause.ANIMAL, ELimb.SKULL, Provider.server, out eplayerKill);
									}
								}
							}
							else if (Time.time - this.lastAttack > 1f)
							{
								this.isAttacking = true;
								AnimalManager.sendAnimalAttack(this);
							}
						}
					}
					else if (num > 4096f)
					{
						this.player = null;
						this.isHunting = false;
						this.updateTicking();
					}
					else
					{
						this.lastTarget = Time.time;
						this.isAttacking = false;
					}
				}
				else
				{
					this.player = null;
					this.isHunting = false;
					this.updateTicking();
				}
				this.lastWander = Time.time;
			}
			this.move(delta);
		}

		private void Start()
		{
			if (Provider.isServer)
			{
				this.controller = base.GetComponent<CharacterController>();
			}
			else
			{
				this.nsb = new NetworkSnapshotBuffer(Provider.UPDATE_TIME, Provider.UPDATE_DELAY);
			}
			this.reset();
			this.lastEat = Time.time + Random.Range(4f, 16f);
			this.lastGlance = Time.time + Random.Range(4f, 16f);
			this.lastWander = Time.time + Random.Range(8f, 32f);
			this.eatDelay = Random.Range(4f, 8f);
			this.glanceDelay = Random.Range(4f, 8f);
			this.wanderDelay = Random.Range(8f, 16f);
			this.updateLife();
			this.updateStates();
		}

		private void Awake()
		{
			if (Dedicator.isDedicated)
			{
				this.eatTime = 0.5f;
				this.glanceTime = 0.5f;
				this.startleTime = 0.5f;
				this.attackTime = 0.5f;
			}
			else
			{
				this.animator = base.transform.FindChild("Character").GetComponent<Animation>();
				this.skeleton = this.animator.transform.FindChild("Skeleton");
				if (this.animator.transform.FindChild("Model_0") != null)
				{
					this.renderer_0 = this.animator.transform.FindChild("Model_0").GetComponent<Renderer>();
				}
				if (this.animator.transform.FindChild("Model_1"))
				{
					this.renderer_1 = this.animator.transform.FindChild("Model_1").GetComponent<Renderer>();
				}
				this.eatTime = this.animator["Eat"].clip.length;
				this.glanceTime = this.animator["Glance_0"].clip.length;
				this.startleTime = this.animator["Startle"].clip.length;
				if (this.animator["Attack"] != null)
				{
					this.attackTime = this.animator["Attack"].clip.length;
				}
				else
				{
					this.attackTime = 0.5f;
				}
			}
		}

		private Animation animator;

		private Transform skeleton;

		private Renderer renderer_0;

		private Renderer renderer_1;

		private float lastEat;

		private float lastGlance;

		private float lastStartle;

		private float lastWander;

		private float lastStuck;

		private float lastTarget;

		private float lastAttack;

		private float lastRegen;

		private float eatTime;

		private float glanceTime;

		private float startleTime;

		private float attackTime;

		private float startedRoar;

		private float startedPanic;

		private float eatDelay;

		private float glanceDelay;

		private float wanderDelay;

		private bool isPlayingEat;

		private bool isPlayingGlance;

		private bool isPlayingStartle;

		private bool isPlayingAttack;

		private Player player;

		private Vector3 lastUpdatePos;

		private float lastUpdateAngle;

		private NetworkSnapshotBuffer nsb;

		private bool isMoving;

		private bool isRunning;

		private bool isTicking;

		private bool _isFleeing;

		private bool isWandering;

		private bool isHunting;

		private bool isStuck;

		private bool isAttacking;

		private float _lastDead;

		public bool isDead;

		public ushort index;

		public ushort id;

		public PackInfo pack;

		private ushort health;

		private Vector3 ragdoll;

		private AnimalAsset _asset;

		private CharacterController controller;

		public bool isUpdated;

		private float lastTick;
	}
}
