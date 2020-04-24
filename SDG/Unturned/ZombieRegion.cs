using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	public class ZombieRegion
	{
		public ZombieRegion(byte newNav)
		{
			this._zombies = new List<Zombie>();
			this.nav = newNav;
			this.updates = 0;
			this.respawnZombieIndex = 0;
			this.alive = 0;
			this.isNetworked = false;
			this.lastMega = -1000f;
			this.hasMega = false;
		}

		public List<Zombie> zombies
		{
			get
			{
				return this._zombies;
			}
		}

		public byte nav { get; protected set; }

		public bool hasBeacon
		{
			get
			{
				return this._hasBeacon;
			}
			set
			{
				if (value != this._hasBeacon)
				{
					this._hasBeacon = value;
					if (this.onHyperUpdated != null)
					{
						this.onHyperUpdated(this.isHyper);
					}
				}
			}
		}

		public bool isHyper
		{
			get
			{
				return LightingManager.isFullMoon || this.hasBeacon;
			}
		}

		public void UpdateRegion()
		{
			if (this.bossZombie == null)
			{
				return;
			}
			bool flag = false;
			bool flag2 = false;
			for (int i = 0; i < Provider.clients.Count; i++)
			{
				SteamPlayer steamPlayer = Provider.clients[i];
				if (!(steamPlayer.player == null) && !(steamPlayer.player.movement == null) && !(steamPlayer.player.life == null) && !steamPlayer.player.life.isDead)
				{
					if (steamPlayer.player.movement.bound == this.nav)
					{
						flag = true;
					}
					if (steamPlayer.player.movement.nav == this.nav)
					{
						flag2 = true;
					}
					if (flag && flag2)
					{
						break;
					}
				}
			}
			if (flag)
			{
				if (this.bossZombie.isDead)
				{
					this.bossZombie = null;
					if (flag2)
					{
						this.UpdateBoss();
					}
				}
			}
			else
			{
				EPlayerKill eplayerKill;
				uint num;
				this.bossZombie.askDamage(50000, Vector3.up, out eplayerKill, out num, false, false);
			}
		}

		public void UpdateBoss()
		{
			if (this.bossZombie != null)
			{
				return;
			}
			for (int i = 0; i < Provider.clients.Count; i++)
			{
				SteamPlayer steamPlayer = Provider.clients[i];
				if (!(steamPlayer.player == null) && !(steamPlayer.player.movement == null) && !(steamPlayer.player.life == null) && !steamPlayer.player.life.isDead)
				{
					if (steamPlayer.player.movement.nav == this.nav)
					{
						for (int j = 0; j < steamPlayer.player.quests.questsList.Count; j++)
						{
							PlayerQuest playerQuest = steamPlayer.player.quests.questsList[j];
							if (playerQuest != null && playerQuest.asset != null)
							{
								for (int k = 0; k < playerQuest.asset.conditions.Length; k++)
								{
									NPCZombieKillsCondition npczombieKillsCondition = playerQuest.asset.conditions[k] as NPCZombieKillsCondition;
									if (npczombieKillsCondition != null)
									{
										if (npczombieKillsCondition.nav == this.nav && npczombieKillsCondition.spawn && !npczombieKillsCondition.isConditionMet(steamPlayer.player))
										{
											Zombie zombie = null;
											for (int l = 0; l < this.zombies.Count; l++)
											{
												Zombie zombie2 = this.zombies[l];
												if (zombie2 != null && zombie2.isDead)
												{
													zombie = zombie2;
													break;
												}
											}
											if (zombie == null)
											{
												for (int m = 0; m < this.zombies.Count; m++)
												{
													Zombie zombie3 = this.zombies[m];
													if (zombie3 != null && !zombie3.isHunting)
													{
														zombie = zombie3;
														break;
													}
												}
											}
											if (zombie == null)
											{
												zombie = this.zombies[Random.Range(0, this.zombies.Count)];
											}
											Vector3 position = zombie.transform.position;
											if (zombie.isDead)
											{
												for (int n = 0; n < 10; n++)
												{
													ZombieSpawnpoint zombieSpawnpoint = LevelZombies.zombies[(int)this.nav][Random.Range(0, LevelZombies.zombies[(int)this.nav].Count)];
													if (SafezoneManager.checkPointValid(zombieSpawnpoint.point))
													{
														break;
													}
													position = zombieSpawnpoint.point;
													position.y += 0.1f;
												}
											}
											this.bossZombie = zombie;
											this.bossZombie.sendRevive(this.bossZombie.type, (byte)npczombieKillsCondition.zombie, this.bossZombie.shirt, this.bossZombie.pants, this.bossZombie.hat, this.bossZombie.gear, position, Random.Range(0f, 360f));
										}
									}
								}
							}
						}
					}
				}
			}
		}

		private void onMoonUpdated(bool isFullMoon)
		{
			if (this.onHyperUpdated != null)
			{
				this.onHyperUpdated(this.isHyper);
			}
		}

		public void destroy()
		{
			ushort num = 0;
			while ((int)num < this.zombies.Count)
			{
				Object.Destroy(this.zombies[(int)num].gameObject);
				num += 1;
			}
			this.zombies.Clear();
			this.hasMega = false;
		}

		public void init()
		{
			LightingManager.onMoonUpdated = (MoonUpdated)Delegate.Combine(LightingManager.onMoonUpdated, new MoonUpdated(this.onMoonUpdated));
		}

		public HyperUpdated onHyperUpdated;

		public ZombieLifeUpdated onZombieLifeUpdated;

		private List<Zombie> _zombies;

		public ushort updates;

		public ushort respawnZombieIndex;

		public int alive;

		public bool isNetworked;

		public float lastMega;

		public bool hasMega;

		private bool _hasBeacon;

		public bool isRadioactive;

		private Zombie bossZombie;
	}
}
