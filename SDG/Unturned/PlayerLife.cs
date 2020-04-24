using System;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;
using UnityEngine.Analytics;

namespace SDG.Unturned
{
	public class PlayerLife : PlayerCaller
	{
		public event Hurt onHurt;

		public bool wasPvPDeath { get; private set; }

		public static EDeathCause deathCause
		{
			get
			{
				return PlayerLife._deathCause;
			}
		}

		public static ELimb deathLimb
		{
			get
			{
				return PlayerLife._deathLimb;
			}
		}

		public static CSteamID deathKiller
		{
			get
			{
				return PlayerLife._deathKiller;
			}
		}

		public bool isAggressor
		{
			get
			{
				return Time.realtimeSinceStartup - this.lastTimeAggressive < PlayerLife.COMBAT_COOLDOWN;
			}
		}

		public void markAggressive(bool force, bool spreadToGroup = true)
		{
			if (force || Time.realtimeSinceStartup - this.lastTimeAggressive < PlayerLife.COMBAT_COOLDOWN)
			{
				this.lastTimeAggressive = Time.realtimeSinceStartup;
			}
			else if (this.recentKiller == CSteamID.Nil || Time.realtimeSinceStartup - this.lastTimeTookDamage > PlayerLife.COMBAT_COOLDOWN)
			{
				this.lastTimeAggressive = Time.realtimeSinceStartup;
			}
			if (spreadToGroup && base.player.quests.isMemberOfAGroup)
			{
				for (int i = 0; i < Provider.clients.Count; i++)
				{
					if (Provider.clients[i].playerID.steamID != base.channel.owner.playerID.steamID && base.player.quests.isMemberOfSameGroupAs(Provider.clients[i].player) && Provider.clients[i].player != null)
					{
						Provider.clients[i].player.life.markAggressive(force, false);
					}
				}
			}
		}

		public bool isDead
		{
			get
			{
				return this._isDead;
			}
		}

		public byte health
		{
			get
			{
				return this._health;
			}
		}

		public byte food
		{
			get
			{
				return this._food;
			}
		}

		public byte water
		{
			get
			{
				return this._water;
			}
		}

		public byte virus
		{
			get
			{
				return this._virus;
			}
		}

		public byte vision
		{
			get
			{
				return this._vision;
			}
		}

		public uint warmth
		{
			get
			{
				return this._warmth;
			}
		}

		public byte stamina
		{
			get
			{
				return this._stamina;
			}
		}

		public byte oxygen
		{
			get
			{
				return this._oxygen;
			}
		}

		public bool isBleeding
		{
			get
			{
				return this._isBleeding;
			}
		}

		public bool isBroken
		{
			get
			{
				return this._isBroken;
			}
		}

		public EPlayerTemperature temperature
		{
			get
			{
				return this._temperature;
			}
		}

		public float lastRespawn
		{
			get
			{
				return this._lastRespawn;
			}
		}

		public float lastDeath
		{
			get
			{
				return this._lastDeath;
			}
		}

		[SteamCall]
		public void tellDeath(CSteamID steamID, byte newCause, byte newLimb, CSteamID newKiller)
		{
			if (base.channel.checkServer(steamID))
			{
				PlayerLife._deathCause = (EDeathCause)newCause;
				PlayerLife._deathLimb = (ELimb)newLimb;
				PlayerLife._deathKiller = newKiller;
				if (base.channel.isOwner)
				{
					int num;
					if (Provider.provider.statisticsService.userStatisticsService.getStatistic("Deaths_Players", out num))
					{
						Provider.provider.statisticsService.userStatisticsService.setStatistic("Deaths_Players", num + 1);
					}
					if (Level.info != null && Time.realtimeSinceStartup - this.lastAlive > 5f)
					{
						string value = PlayerLife.deathCause.ToString();
						float num2 = Time.realtimeSinceStartup - this.lastAlive;
						string value2 = (!Level.info.canAnalyticsTrack) ? "Workshop" : Level.info.name;
						Dictionary<string, object> dictionary = new Dictionary<string, object>
						{
							{
								"Cause",
								value
							},
							{
								"Lifespan",
								num2
							},
							{
								"Map",
								value2
							},
							{
								"Network",
								Provider.clients.Count > 1
							}
						};
						Analytics.CustomEvent("Death", dictionary);
					}
				}
			}
		}

		[SteamCall]
		public void tellDead(CSteamID steamID, Vector3 newRagdoll)
		{
			if (base.channel.checkServer(steamID))
			{
				this._isDead = true;
				this._lastDeath = Time.realtimeSinceStartup;
				this.ragdoll = newRagdoll;
				if (!Dedicator.isDedicated)
				{
					RagdollTool.ragdollPlayer(base.transform.position, base.transform.rotation, base.player.animator.thirdSkeleton, this.ragdoll, base.player.clothing);
				}
				if (this.onLifeUpdated != null)
				{
					this.onLifeUpdated(this.isDead);
				}
				if (PlayerLife.onPlayerLifeUpdated != null)
				{
					PlayerLife.onPlayerLifeUpdated(base.player);
				}
			}
		}

		[SteamCall]
		public void tellRevive(CSteamID steamID, Vector3 position, byte angle)
		{
			if (base.channel.checkServer(steamID))
			{
				this._isDead = false;
				this._lastRespawn = Time.realtimeSinceStartup;
				base.player.askTeleport(steamID, position, angle);
				if (this.onLifeUpdated != null)
				{
					this.onLifeUpdated(this.isDead);
				}
				if (PlayerLife.onPlayerLifeUpdated != null)
				{
					PlayerLife.onPlayerLifeUpdated(base.player);
				}
			}
		}

		[SteamCall]
		public void tellLife(CSteamID steamID, byte newHealth, byte newFood, byte newWater, byte newVirus, bool newBleeding, bool newBroken)
		{
			if (base.channel.checkServer(steamID))
			{
				Player.isLoadingLife = false;
			}
			this.tellHealth(steamID, newHealth);
			this.tellFood(steamID, newFood);
			this.tellWater(steamID, newWater);
			this.tellVirus(steamID, newVirus);
			this.tellBleeding(steamID, newBleeding);
			this.tellBroken(steamID, newBroken);
			if (base.channel.checkServer(steamID))
			{
				this._stamina = 100;
				this._oxygen = 100;
				this._vision = 0;
				this._warmth = 0u;
				this._temperature = EPlayerTemperature.NONE;
				this.wasWarm = false;
				this.wasCovered = false;
				if (this.onVisionUpdated != null)
				{
					this.onVisionUpdated(false);
				}
				if (this.onStaminaUpdated != null)
				{
					this.onStaminaUpdated(this.stamina);
				}
				if (this.onOxygenUpdated != null)
				{
					this.onOxygenUpdated(this.oxygen);
				}
				if (this.onTemperatureUpdated != null)
				{
					this.onTemperatureUpdated(this.temperature);
				}
				this.lastAlive = Time.realtimeSinceStartup;
			}
		}

		[SteamCall]
		public void askLife(CSteamID steamID)
		{
			if (Provider.isServer)
			{
				if (base.channel.checkOwner(steamID))
				{
					base.channel.send("tellLife", steamID, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
					{
						this.health,
						this.food,
						this.water,
						this.virus,
						this.isBleeding,
						this.isBroken
					});
				}
				else if (this.isDead)
				{
					base.channel.send("tellDead", steamID, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
					{
						this.ragdoll
					});
				}
			}
		}

		[SteamCall]
		public void tellHealth(CSteamID steamID, byte newHealth)
		{
			if (base.channel.checkServer(steamID))
			{
				this._health = newHealth;
				if (this.onHealthUpdated != null)
				{
					this.onHealthUpdated(this.health);
				}
				if (newHealth < this.lastHealth - 3 && this.onDamaged != null)
				{
					this.onDamaged(this.lastHealth - newHealth);
				}
				this.lastHealth = newHealth;
			}
		}

		[SteamCall]
		public void tellFood(CSteamID steamID, byte newFood)
		{
			if (base.channel.checkServer(steamID))
			{
				this._food = newFood;
				if (this.onFoodUpdated != null)
				{
					this.onFoodUpdated(this.food);
				}
			}
		}

		[SteamCall]
		public void tellWater(CSteamID steamID, byte newWater)
		{
			if (base.channel.checkServer(steamID))
			{
				this._water = newWater;
				if (this.onWaterUpdated != null)
				{
					this.onWaterUpdated(this.water);
				}
			}
		}

		[SteamCall]
		public void tellVirus(CSteamID steamID, byte newVirus)
		{
			if (base.channel.checkServer(steamID))
			{
				this._virus = newVirus;
				if (this.onVirusUpdated != null)
				{
					this.onVirusUpdated(this.virus);
				}
			}
		}

		[SteamCall]
		public void tellBleeding(CSteamID steamID, bool newBleeding)
		{
			if (base.channel.checkServer(steamID))
			{
				this._isBleeding = newBleeding;
				if (this.onBleedingUpdated != null)
				{
					this.onBleedingUpdated(this.isBleeding);
				}
			}
		}

		[SteamCall]
		public void tellBroken(CSteamID steamID, bool newBroken)
		{
			if (base.channel.checkServer(steamID))
			{
				this._isBroken = newBroken;
				if (this.onBrokenUpdated != null)
				{
					this.onBrokenUpdated(this.isBroken);
				}
			}
		}

		public void askDamage(byte amount, Vector3 newRagdoll, EDeathCause newCause, ELimb newLimb, CSteamID newKiller, out EPlayerKill kill)
		{
			kill = EPlayerKill.NONE;
			if (base.player.movement.isSafe && base.player.movement.isSafeInfo.noWeapons)
			{
				return;
			}
			this.doDamage(amount, newRagdoll, newCause, newLimb, newKiller, out kill);
		}

		private void doDamage(byte amount, Vector3 newRagdoll, EDeathCause newCause, ELimb newLimb, CSteamID newKiller, out EPlayerKill kill)
		{
			kill = EPlayerKill.NONE;
			if (amount == 0 || this.isDead)
			{
				return;
			}
			if (!this.isDead)
			{
				if (amount >= this.health)
				{
					this._health = 0;
				}
				else
				{
					this._health -= amount;
				}
				this.ragdoll = newRagdoll;
				base.channel.send("tellHealth", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
				{
					this.health
				});
				if (newCause == EDeathCause.GUN || newCause == EDeathCause.MELEE || newCause == EDeathCause.PUNCH || newCause == EDeathCause.ROADKILL || newCause == EDeathCause.GRENADE || newCause == EDeathCause.MISSILE || newCause == EDeathCause.CHARGE)
				{
					this.recentKiller = newKiller;
					this.lastTimeTookDamage = Time.realtimeSinceStartup;
					Player player = PlayerTool.getPlayer(this.recentKiller);
					if (player != null)
					{
						player.life.lastTimeCausedDamage = Time.realtimeSinceStartup;
						if (Time.realtimeSinceStartup - player.life.lastTimeAggressive < PlayerLife.COMBAT_COOLDOWN)
						{
							player.life.markAggressive(true, true);
						}
						else if ((player.life.recentKiller == CSteamID.Nil || Time.realtimeSinceStartup - player.life.lastTimeTookDamage > PlayerLife.COMBAT_COOLDOWN) && Time.realtimeSinceStartup - this.lastTimeCausedDamage > PlayerLife.COMBAT_COOLDOWN)
						{
							player.life.markAggressive(true, true);
						}
					}
				}
				if (this.health == 0)
				{
					if (this.recentKiller != CSteamID.Nil && this.recentKiller != base.channel.owner.playerID.steamID && Time.realtimeSinceStartup - this.lastTimeTookDamage < PlayerLife.COMBAT_COOLDOWN)
					{
						Player player2 = PlayerTool.getPlayer(this.recentKiller);
						if (player2 != null)
						{
							int num = Mathf.Abs(base.player.skills.reputation);
							num = Mathf.Clamp(num, 1, 25);
							if (player2.life.isAggressor)
							{
								num = -num;
							}
							player2.skills.askRep(num);
						}
					}
					kill = EPlayerKill.PLAYER;
					this.wasPvPDeath = (newCause == EDeathCause.GUN || newCause == EDeathCause.MELEE || newCause == EDeathCause.PUNCH || newCause == EDeathCause.ROADKILL || newCause == EDeathCause.GRENADE || newCause == EDeathCause.MISSILE || newCause == EDeathCause.CHARGE || newCause == EDeathCause.SENTRY);
					base.channel.send("tellDeath", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
					{
						(byte)newCause,
						(byte)newLimb,
						newKiller
					});
					base.channel.send("tellDead", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
					{
						this.ragdoll
					});
					if (this.spawnpoint == null || (newCause != EDeathCause.SUICIDE && newCause != EDeathCause.BREATH) || Time.realtimeSinceStartup - this.lastSuicide > 60f)
					{
						this.spawnpoint = LevelPlayers.getSpawn(false);
					}
					if (newCause == EDeathCause.SUICIDE || newCause == EDeathCause.BREATH)
					{
						this.lastSuicide = Time.realtimeSinceStartup;
					}
					if (CommandWindow.shouldLogDeaths)
					{
						if (newCause == EDeathCause.BLEEDING)
						{
							CommandWindow.Log(Provider.localization.format("Bleeding", new object[]
							{
								base.channel.owner.playerID.characterName,
								base.channel.owner.playerID.playerName
							}));
						}
						else if (newCause == EDeathCause.BONES)
						{
							CommandWindow.Log(Provider.localization.format("Bones", new object[]
							{
								base.channel.owner.playerID.characterName,
								base.channel.owner.playerID.playerName
							}));
						}
						else if (newCause == EDeathCause.FREEZING)
						{
							CommandWindow.Log(Provider.localization.format("Freezing", new object[]
							{
								base.channel.owner.playerID.characterName,
								base.channel.owner.playerID.playerName
							}));
						}
						else if (newCause == EDeathCause.BURNING)
						{
							CommandWindow.Log(Provider.localization.format("Burning", new object[]
							{
								base.channel.owner.playerID.characterName,
								base.channel.owner.playerID.playerName
							}));
						}
						else if (newCause == EDeathCause.FOOD)
						{
							CommandWindow.Log(Provider.localization.format("Food", new object[]
							{
								base.channel.owner.playerID.characterName,
								base.channel.owner.playerID.playerName
							}));
						}
						else if (newCause == EDeathCause.WATER)
						{
							CommandWindow.Log(Provider.localization.format("Water", new object[]
							{
								base.channel.owner.playerID.characterName,
								base.channel.owner.playerID.playerName
							}));
						}
						else if (newCause == EDeathCause.GUN || newCause == EDeathCause.MELEE || newCause == EDeathCause.PUNCH || newCause == EDeathCause.ROADKILL || newCause == EDeathCause.GRENADE || newCause == EDeathCause.MISSILE || newCause == EDeathCause.CHARGE || newCause == EDeathCause.SPLASH)
						{
							SteamPlayer steamPlayer = PlayerTool.getSteamPlayer(newKiller);
							string text;
							string text2;
							if (steamPlayer != null)
							{
								text = steamPlayer.playerID.characterName;
								text2 = steamPlayer.playerID.playerName;
							}
							else
							{
								text = "?";
								text2 = "?";
							}
							string text3 = string.Empty;
							if (newLimb == ELimb.LEFT_FOOT || newLimb == ELimb.LEFT_LEG || newLimb == ELimb.RIGHT_FOOT || newLimb == ELimb.RIGHT_LEG)
							{
								text3 = Provider.localization.format("Leg");
							}
							else if (newLimb == ELimb.LEFT_HAND || newLimb == ELimb.LEFT_ARM || newLimb == ELimb.RIGHT_HAND || newLimb == ELimb.RIGHT_ARM)
							{
								text3 = Provider.localization.format("Arm");
							}
							else if (newLimb == ELimb.SPINE)
							{
								text3 = Provider.localization.format("Spine");
							}
							else if (newLimb == ELimb.SKULL)
							{
								text3 = Provider.localization.format("Skull");
							}
							if (newCause == EDeathCause.GUN)
							{
								CommandWindow.Log(Provider.localization.format("Gun", new object[]
								{
									base.channel.owner.playerID.characterName,
									base.channel.owner.playerID.playerName,
									text3,
									text,
									text2
								}));
							}
							else if (newCause == EDeathCause.MELEE)
							{
								CommandWindow.Log(Provider.localization.format("Melee", new object[]
								{
									base.channel.owner.playerID.characterName,
									base.channel.owner.playerID.playerName,
									text3,
									text,
									text2
								}));
							}
							else if (newCause == EDeathCause.PUNCH)
							{
								CommandWindow.Log(Provider.localization.format("Punch", new object[]
								{
									base.channel.owner.playerID.characterName,
									base.channel.owner.playerID.playerName,
									text3,
									text,
									text2
								}));
							}
							else if (newCause == EDeathCause.ROADKILL)
							{
								CommandWindow.Log(Provider.localization.format("Roadkill", new object[]
								{
									base.channel.owner.playerID.characterName,
									base.channel.owner.playerID.playerName,
									text,
									text2
								}));
							}
							else if (newCause == EDeathCause.GRENADE)
							{
								CommandWindow.Log(Provider.localization.format("Grenade", new object[]
								{
									base.channel.owner.playerID.characterName,
									base.channel.owner.playerID.playerName,
									text,
									text2
								}));
							}
							else if (newCause == EDeathCause.MISSILE)
							{
								CommandWindow.Log(Provider.localization.format("Missile", new object[]
								{
									base.channel.owner.playerID.characterName,
									base.channel.owner.playerID.playerName,
									text,
									text2
								}));
							}
							else if (newCause == EDeathCause.CHARGE)
							{
								CommandWindow.Log(Provider.localization.format("Charge", new object[]
								{
									base.channel.owner.playerID.characterName,
									base.channel.owner.playerID.playerName,
									text,
									text2
								}));
							}
							else if (newCause == EDeathCause.SPLASH)
							{
								CommandWindow.Log(Provider.localization.format("Splash", new object[]
								{
									base.channel.owner.playerID.characterName,
									base.channel.owner.playerID.playerName,
									text,
									text2
								}));
							}
						}
						else if (newCause == EDeathCause.ZOMBIE)
						{
							CommandWindow.Log(Provider.localization.format("Zombie", new object[]
							{
								base.channel.owner.playerID.characterName,
								base.channel.owner.playerID.playerName
							}));
						}
						else if (newCause == EDeathCause.ANIMAL)
						{
							CommandWindow.Log(Provider.localization.format("Animal", new object[]
							{
								base.channel.owner.playerID.characterName,
								base.channel.owner.playerID.playerName
							}));
						}
						else if (newCause == EDeathCause.SUICIDE)
						{
							CommandWindow.Log(Provider.localization.format("Suicide", new object[]
							{
								base.channel.owner.playerID.characterName,
								base.channel.owner.playerID.playerName
							}));
						}
						else if (newCause == EDeathCause.INFECTION)
						{
							CommandWindow.Log(Provider.localization.format("Infection", new object[]
							{
								base.channel.owner.playerID.characterName,
								base.channel.owner.playerID.playerName
							}));
						}
						else if (newCause == EDeathCause.BREATH)
						{
							CommandWindow.Log(Provider.localization.format("Breath", new object[]
							{
								base.channel.owner.playerID.characterName,
								base.channel.owner.playerID.playerName
							}));
						}
						else if (newCause == EDeathCause.ZOMBIE)
						{
							CommandWindow.Log(Provider.localization.format("Zombie", new object[]
							{
								base.channel.owner.playerID.characterName,
								base.channel.owner.playerID.playerName
							}));
						}
						else if (newCause == EDeathCause.VEHICLE)
						{
							CommandWindow.Log(Provider.localization.format("Vehicle", new object[]
							{
								base.channel.owner.playerID.characterName,
								base.channel.owner.playerID.playerName
							}));
						}
						else if (newCause == EDeathCause.SHRED)
						{
							CommandWindow.Log(Provider.localization.format("Shred", new object[]
							{
								base.channel.owner.playerID.characterName,
								base.channel.owner.playerID.playerName
							}));
						}
						else if (newCause == EDeathCause.LANDMINE)
						{
							CommandWindow.Log(Provider.localization.format("Landmine", new object[]
							{
								base.channel.owner.playerID.characterName,
								base.channel.owner.playerID.playerName
							}));
						}
						else if (newCause == EDeathCause.ARENA)
						{
							CommandWindow.Log(Provider.localization.format("Arena", new object[]
							{
								base.channel.owner.playerID.characterName,
								base.channel.owner.playerID.playerName
							}));
						}
						else if (newCause == EDeathCause.SENTRY)
						{
							CommandWindow.Log(Provider.localization.format("Sentry", new object[]
							{
								base.channel.owner.playerID.characterName,
								base.channel.owner.playerID.playerName
							}));
						}
						else if (newCause == EDeathCause.ACID)
						{
							CommandWindow.Log(Provider.localization.format("Acid", new object[]
							{
								base.channel.owner.playerID.characterName,
								base.channel.owner.playerID.playerName
							}));
						}
						else if (newCause == EDeathCause.BOULDER)
						{
							CommandWindow.Log(Provider.localization.format("Boulder", new object[]
							{
								base.channel.owner.playerID.characterName,
								base.channel.owner.playerID.playerName
							}));
						}
						else if (newCause == EDeathCause.BURNER)
						{
							CommandWindow.Log(Provider.localization.format("Burner", new object[]
							{
								base.channel.owner.playerID.characterName,
								base.channel.owner.playerID.playerName
							}));
						}
						else if (newCause == EDeathCause.SPIT)
						{
							CommandWindow.Log(Provider.localization.format("Spit", new object[]
							{
								base.channel.owner.playerID.characterName,
								base.channel.owner.playerID.playerName
							}));
						}
						else if (newCause == EDeathCause.SPARK)
						{
							CommandWindow.Log(Provider.localization.format("Spark", new object[]
							{
								base.channel.owner.playerID.characterName,
								base.channel.owner.playerID.playerName
							}));
						}
					}
				}
				else if (Provider.modeConfigData.Players.Can_Start_Bleeding && amount >= 20 && !this.isBleeding)
				{
					this._isBleeding = true;
					this.lastBleeding = base.player.input.simulation;
					this.lastBleed = base.player.input.simulation;
					base.channel.send("tellBleeding", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
					{
						this.isBleeding
					});
				}
				if (this.onHurt != null)
				{
					this.onHurt(base.player, amount, newRagdoll, newCause, newLimb, newKiller);
				}
			}
		}

		public void askHeal(byte amount, bool healBleeding, bool healBroken)
		{
			if (amount == 0 || this.isDead)
			{
				return;
			}
			if (!this.isDead)
			{
				if (amount >= 100 - this.health)
				{
					this._health = 100;
				}
				else
				{
					this._health += amount;
				}
				base.channel.send("tellHealth", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
				{
					this.health
				});
				if (this.isBleeding && healBleeding)
				{
					this._isBleeding = false;
					base.channel.send("tellBleeding", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
					{
						this.isBleeding
					});
				}
				if (this.isBroken && healBroken)
				{
					this._isBroken = false;
					base.channel.send("tellBroken", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
					{
						this.isBroken
					});
				}
			}
		}

		public void askStarve(byte amount)
		{
			if (amount == 0 || this.isDead)
			{
				return;
			}
			if (!this.isDead)
			{
				if (amount >= this.food)
				{
					this._food = 0;
				}
				else
				{
					this._food -= amount;
				}
				base.channel.send("tellFood", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
				{
					this.food
				});
			}
		}

		public void askEat(byte amount)
		{
			if (amount == 0 || this.isDead)
			{
				return;
			}
			if (!this.isDead)
			{
				if (amount >= 100 - this.food)
				{
					this._food = 100;
				}
				else
				{
					this._food += amount;
				}
				base.channel.send("tellFood", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
				{
					this.food
				});
			}
		}

		public void askDehydrate(byte amount)
		{
			if (amount == 0 || this.isDead)
			{
				return;
			}
			if (!this.isDead)
			{
				if (amount >= this.water)
				{
					this._water = 0;
				}
				else
				{
					this._water -= amount;
				}
				base.channel.send("tellWater", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
				{
					this.water
				});
			}
		}

		public void askDrink(byte amount)
		{
			if (amount == 0 || this.isDead)
			{
				return;
			}
			if (!this.isDead)
			{
				if (amount >= 100 - this.water)
				{
					this._water = 100;
				}
				else
				{
					this._water += amount;
				}
				base.channel.send("tellWater", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
				{
					this.water
				});
			}
		}

		public void askInfect(byte amount)
		{
			if (amount == 0 || this.isDead)
			{
				return;
			}
			if (!this.isDead)
			{
				if (amount >= this.virus)
				{
					this._virus = 0;
				}
				else
				{
					this._virus -= amount;
				}
				base.channel.send("tellVirus", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
				{
					this.virus
				});
			}
		}

		public void askRadiate(byte amount)
		{
			if (amount == 0 || this.isDead)
			{
				return;
			}
			if (!this.isDead)
			{
				if (amount >= this.virus)
				{
					this._virus = 0;
				}
				else
				{
					this._virus -= amount;
				}
				if (this.onVirusUpdated != null)
				{
					this.onVirusUpdated(this.virus);
				}
			}
		}

		public void askDisinfect(byte amount)
		{
			if (amount == 0 || this.isDead)
			{
				return;
			}
			if (!this.isDead)
			{
				if (amount >= 100 - this.virus)
				{
					this._virus = 100;
				}
				else
				{
					this._virus += amount;
				}
				base.channel.send("tellVirus", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
				{
					this.virus
				});
			}
		}

		public void askTire(byte amount)
		{
			if (amount == 0 || this.isDead)
			{
				return;
			}
			if (!this.isDead)
			{
				this.lastTire = base.player.input.simulation;
				if (amount >= this.stamina)
				{
					this._stamina = 0;
				}
				else
				{
					this._stamina -= amount;
				}
				if (this.onStaminaUpdated != null)
				{
					this.onStaminaUpdated(this.stamina);
				}
			}
		}

		public void askRest(byte amount)
		{
			if (amount == 0 || this.isDead)
			{
				return;
			}
			if (!this.isDead)
			{
				if (amount >= 100 - this.stamina)
				{
					this._stamina = 100;
				}
				else
				{
					this._stamina += amount;
				}
				if (this.onStaminaUpdated != null)
				{
					this.onStaminaUpdated(this.stamina);
				}
			}
		}

		public void askView(byte amount)
		{
			if (amount == 0 || this.isDead)
			{
				return;
			}
			if (!this.isDead)
			{
				this.lastView = base.player.input.simulation;
				this._vision = (byte)Mathf.Max((int)this.vision, (int)amount);
				if (this.onVisionUpdated != null)
				{
					this.onVisionUpdated(true);
				}
			}
		}

		public void askWarm(uint amount)
		{
			if (amount == 0u || this.isDead)
			{
				return;
			}
			if (!this.isDead)
			{
				this._warmth += amount;
			}
		}

		public void askBlind(byte amount)
		{
			if (amount == 0 || this.isDead)
			{
				return;
			}
			if (!this.isDead)
			{
				if (amount >= this.vision)
				{
					this._vision = 0;
				}
				else
				{
					this._vision -= amount;
				}
				if (this.vision == 0 && this.onVisionUpdated != null)
				{
					this.onVisionUpdated(false);
				}
			}
		}

		public void askSuffocate(byte amount)
		{
			if (amount == 0 || this.isDead)
			{
				return;
			}
			if (!this.isDead)
			{
				this.lastSuffocate = base.player.input.simulation;
				if (amount >= this.oxygen)
				{
					this._oxygen = 0;
				}
				else
				{
					this._oxygen -= amount;
				}
				if (this.onOxygenUpdated != null)
				{
					this.onOxygenUpdated(this.oxygen);
				}
			}
		}

		public void askBreath(byte amount)
		{
			if (amount == 0 || this.isDead)
			{
				return;
			}
			if (!this.isDead)
			{
				if (amount >= 100 - this.oxygen)
				{
					this._oxygen = 100;
				}
				else
				{
					this._oxygen += amount;
				}
				if (this.onOxygenUpdated != null)
				{
					this.onOxygenUpdated(this.oxygen);
				}
			}
		}

		[SteamCall]
		public void askRespawn(CSteamID steamID, bool atHome)
		{
			if (this.spawnpoint == null)
			{
				return;
			}
			if (base.channel.checkOwner(steamID) && Provider.isServer && this.isDead)
			{
				if (atHome)
				{
					if ((!Provider.isServer || Dedicator.isDedicated) && Provider.isPvP)
					{
						if (Time.realtimeSinceStartup - this.lastDeath < Provider.modeConfigData.Gameplay.Timer_Home)
						{
							return;
						}
					}
					else if (Time.realtimeSinceStartup - this.lastRespawn < Provider.modeConfigData.Gameplay.Timer_Respawn)
					{
						return;
					}
				}
				else if (Time.realtimeSinceStartup - this.lastRespawn < Provider.modeConfigData.Gameplay.Timer_Respawn)
				{
					return;
				}
				this.sendRevive();
				Vector3 vector;
				byte b;
				if (!atHome || !BarricadeManager.tryGetBed(base.channel.owner.playerID.steamID, out vector, out b))
				{
					vector = this.spawnpoint.point;
					b = MeasurementTool.angleToByte(this.spawnpoint.angle);
				}
				vector += new Vector3(0f, 0.5f, 0f);
				base.channel.send("tellRevive", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
				{
					vector,
					b
				});
			}
		}

		[SteamCall]
		public void askSuicide(CSteamID steamID)
		{
			if (base.channel.checkOwner(steamID) && Provider.isServer && !this.isDead && ((Level.info != null && Level.info.type == ELevelType.SURVIVAL) || !base.player.movement.isSafe || !base.player.movement.isSafeInfo.noWeapons))
			{
				EPlayerKill eplayerKill;
				this.doDamage(100, Vector3.up * 10f, EDeathCause.SUICIDE, ELimb.SKULL, steamID, out eplayerKill);
			}
		}

		public void sendRevive()
		{
			this._health = 100;
			this._food = 100;
			this._water = 100;
			this._virus = 100;
			this._stamina = 100;
			this._oxygen = 100;
			this._vision = 0;
			this._warmth = 0u;
			this._isBleeding = false;
			this._isBroken = false;
			this._temperature = EPlayerTemperature.NONE;
			this.wasWarm = false;
			this.wasCovered = false;
			this.lastStarve = base.player.input.simulation;
			this.lastDehydrate = base.player.input.simulation;
			this.lastUncleaned = base.player.input.simulation;
			this.lastTire = base.player.input.simulation;
			this.lastRest = base.player.input.simulation;
			this.recentKiller = CSteamID.Nil;
			this.lastTimeAggressive = -100f;
			this.lastTimeTookDamage = -100f;
			this.lastTimeCausedDamage = -100f;
			base.channel.send("tellLife", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
			{
				this.health,
				this.food,
				this.water,
				this.virus,
				this.isBleeding,
				this.isBroken
			});
		}

		public void sendRespawn(bool atHome)
		{
			base.channel.send("askRespawn", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[]
			{
				atHome
			});
		}

		public void sendSuicide()
		{
			base.channel.send("askSuicide", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[0]);
		}

		public void init()
		{
			base.channel.send("askLife", ESteamCall.SERVER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[0]);
		}

		public void simulate(uint simulation)
		{
			if (Provider.isServer)
			{
				if (Level.info.type == ELevelType.SURVIVAL)
				{
					if (this.food > 0)
					{
						if (simulation - this.lastStarve > Provider.modeConfigData.Players.Food_Use_Ticks * (1f + base.player.skills.mastery(1, 6) * 0.25f) * ((!base.player.movement.inSnow) ? 1f : (0.5f + base.player.skills.mastery(1, 5) * 0.5f)))
						{
							this.lastStarve = simulation;
							this.askStarve(1);
						}
					}
					else if (simulation - this.lastStarve > Provider.modeConfigData.Players.Food_Damage_Ticks)
					{
						this.lastStarve = simulation;
						EPlayerKill eplayerKill;
						this.askDamage(1, Vector3.up, EDeathCause.FOOD, ELimb.SPINE, Provider.server, out eplayerKill);
					}
					if (this.water > 0)
					{
						if (simulation - this.lastDehydrate > Provider.modeConfigData.Players.Water_Use_Ticks * (1f + base.player.skills.mastery(1, 6) * 0.25f))
						{
							this.lastDehydrate = simulation;
							this.askDehydrate(1);
						}
					}
					else if (simulation - this.lastDehydrate > Provider.modeConfigData.Players.Water_Damage_Ticks)
					{
						this.lastDehydrate = simulation;
						EPlayerKill eplayerKill2;
						this.askDamage(1, Vector3.up, EDeathCause.WATER, ELimb.SPINE, Provider.server, out eplayerKill2);
					}
					if (this.virus == 0)
					{
						if (simulation - this.lastInfect > Provider.modeConfigData.Players.Virus_Damage_Ticks)
						{
							this.lastInfect = simulation;
							EPlayerKill eplayerKill3;
							this.askDamage(1, Vector3.up, EDeathCause.INFECTION, ELimb.SPINE, Provider.server, out eplayerKill3);
						}
					}
					else if ((uint)this.virus < Provider.modeConfigData.Players.Virus_Infect && simulation - this.lastUncleaned > Provider.modeConfigData.Players.Virus_Use_Ticks)
					{
						this.lastUncleaned = simulation;
						this.askInfect(1);
					}
				}
				if (this.isBleeding)
				{
					if (simulation - this.lastBleed > Provider.modeConfigData.Players.Bleed_Damage_Ticks)
					{
						this.lastBleed = simulation;
						EPlayerKill eplayerKill4;
						this.askDamage(1, Vector3.up, EDeathCause.BLEEDING, ELimb.SPINE, Provider.server, out eplayerKill4);
					}
				}
				else if (this.health < 100 && (uint)this.food > Provider.modeConfigData.Players.Health_Regen_Min_Food && (uint)this.water > Provider.modeConfigData.Players.Health_Regen_Min_Water && simulation - this.lastRegenerate > Provider.modeConfigData.Players.Health_Regen_Ticks * (1f - base.player.skills.mastery(1, 1) * 0.5f))
				{
					this.lastRegenerate = simulation;
					this.askHeal(1, false, false);
				}
				if (Provider.modeConfigData.Players.Can_Stop_Bleeding && this.isBleeding && simulation - this.lastBleeding > Provider.modeConfigData.Players.Bleed_Regen_Ticks * (1f - base.player.skills.mastery(1, 4) * 0.5f))
				{
					this._isBleeding = false;
					base.channel.send("tellBleeding", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
					{
						this.isBleeding
					});
				}
				if (Provider.modeConfigData.Players.Can_Fix_Legs && this.isBroken && simulation - this.lastBroken > Provider.modeConfigData.Players.Leg_Regen_Ticks * (1f - base.player.skills.mastery(1, 4) * 0.5f))
				{
					this._isBroken = false;
					base.channel.send("tellBroken", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
					{
						this.isBroken
					});
				}
			}
			if (base.channel.isOwner)
			{
				if (this.vision > 0 && simulation - this.lastView > 12u)
				{
					this.lastView = simulation;
					this.askBlind(1);
				}
				if (!this.isDead)
				{
					Provider.provider.economyService.updateInventory();
				}
			}
			if (base.channel.isOwner || Provider.isServer)
			{
				if (base.player.stance.stance == EPlayerStance.SPRINT && (ulong)(simulation - this.lastTire) > (ulong)((long)(1 + base.player.skills.skills[0][4].level)))
				{
					this.lastTire = simulation;
					this.askTire(1);
				}
				if (this.stamina < 100 && simulation - this.lastTire > 32f * (1f - base.player.skills.mastery(0, 3) * 0.5f) && simulation - this.lastRest > 1u)
				{
					this.lastRest = simulation;
					this.askRest((byte)(1f + base.player.skills.mastery(0, 3) * 2f));
				}
				bool flag = OxygenManager.checkPointBreathable(base.transform.position);
				bool flag2 = !base.player.stance.isSubmerged || flag;
				if (flag2)
				{
					if (flag)
					{
						if (this.oxygen < 100 && simulation - this.lastBreath > 4u)
						{
							this.lastBreath = simulation;
							this.askBreath(1);
						}
					}
					else
					{
						float num3;
						if (Level.info != null && Level.info.type == ELevelType.SURVIVAL)
						{
							if (Level.info.configData != null && Level.info.configData.Use_Legacy_Oxygen_Height)
							{
								float num = 0f;
								if (Level.info.configData.Use_Legacy_Water)
								{
									num = LevelLighting.getWaterSurfaceElevation();
								}
								float num2 = Mathf.Clamp01((base.transform.position.y - num) / (Level.HEIGHT - num));
								num3 = Mathf.Lerp(1f, -1f, num2);
							}
							else
							{
								num3 = 1f;
							}
						}
						else
						{
							num3 = 1f;
						}
						if (num3 > 0f)
						{
							if (this.oxygen < 100 && simulation - this.lastBreath > (uint)(1 + Mathf.CeilToInt(10f * (1f - num3))))
							{
								this.lastBreath = simulation;
								this.askBreath((byte)(1f + base.player.skills.mastery(0, 3) * 2f));
							}
						}
						else
						{
							num3 = -num3;
							if (this.oxygen > 0)
							{
								uint num4 = (uint)(1 + Mathf.CeilToInt((1f + (float)base.player.skills.skills[0][5].level) * 10f * (1f - num3)));
								if (base.player.clothing.backpackAsset != null && base.player.clothing.backpackAsset.proofWater && base.player.clothing.glassesAsset != null && base.player.clothing.glassesAsset.proofWater)
								{
									num4 *= 10u;
								}
								if (simulation - this.lastSuffocate > num4)
								{
									this.lastSuffocate = simulation;
									this.askSuffocate(1);
								}
							}
							else if (simulation - this.lastSuffocate > 10u)
							{
								this.lastSuffocate = simulation;
								if (Provider.isServer)
								{
									EPlayerKill eplayerKill5;
									this.doDamage(10, Vector3.up, EDeathCause.BREATH, ELimb.SPINE, Provider.server, out eplayerKill5);
								}
							}
						}
					}
				}
				else if (this.oxygen > 0)
				{
					uint num5 = (uint)(1 + base.player.skills.skills[0][5].level);
					if (base.player.clothing.backpackAsset != null && base.player.clothing.backpackAsset.proofWater && base.player.clothing.glassesAsset != null && base.player.clothing.glassesAsset.proofWater)
					{
						num5 *= 10u;
					}
					if (simulation - this.lastSuffocate > num5)
					{
						this.lastSuffocate = simulation;
						this.askSuffocate(1);
					}
				}
				else if (simulation - this.lastSuffocate > 10u)
				{
					this.lastSuffocate = simulation;
					if (Provider.isServer)
					{
						EPlayerKill eplayerKill6;
						this.doDamage(10, Vector3.up, EDeathCause.BREATH, ELimb.SPINE, Provider.server, out eplayerKill6);
					}
				}
				if (base.player.movement.isRadiated)
				{
					if (base.player.clothing.maskAsset != null && base.player.clothing.maskAsset.proofRadiation && base.player.clothing.maskQuality > 0)
					{
						if (simulation - this.lastRadiate > 30u)
						{
							this.lastRadiate = simulation;
							PlayerClothing clothing = base.player.clothing;
							clothing.maskQuality -= 1;
							base.player.clothing.updateMaskQuality();
						}
					}
					else if (this.virus > 0)
					{
						if (simulation - this.lastRadiate > 1u)
						{
							this.lastRadiate = simulation;
							this.askRadiate(1);
						}
					}
					else if (Provider.isServer && simulation - this.lastRadiate > 10u)
					{
						this.lastRadiate = simulation;
						EPlayerKill eplayerKill7;
						this.askDamage(10, Vector3.up, EDeathCause.INFECTION, ELimb.SPINE, Provider.server, out eplayerKill7);
					}
				}
				else
				{
					this.lastRadiate = simulation;
				}
				if (this.warmth > 0u)
				{
					this._warmth -= 1u;
				}
				bool proofFire = false;
				if (base.player.clothing.shirtAsset != null && base.player.clothing.shirtAsset.proofFire && base.player.clothing.pantsAsset != null && base.player.clothing.pantsAsset.proofFire)
				{
					proofFire = true;
				}
				EPlayerTemperature eplayerTemperature = this.temperature;
				EPlayerTemperature eplayerTemperature2 = TemperatureManager.checkPointTemperature(base.transform.position, proofFire);
				if (eplayerTemperature2 == EPlayerTemperature.ACID)
				{
					eplayerTemperature = EPlayerTemperature.ACID;
					if (Provider.isServer && simulation - this.lastBurn > 10u)
					{
						this.lastBurn = simulation;
						EPlayerKill eplayerKill8;
						this.askDamage(10, Vector3.up, EDeathCause.SPIT, ELimb.SPINE, Provider.server, out eplayerKill8);
					}
				}
				else if (eplayerTemperature2 == EPlayerTemperature.BURNING)
				{
					eplayerTemperature = EPlayerTemperature.BURNING;
					if (Provider.isServer && simulation - this.lastBurn > 10u)
					{
						this.lastBurn = simulation;
						EPlayerKill eplayerKill9;
						this.askDamage(10, Vector3.up, EDeathCause.BURNING, ELimb.SPINE, Provider.server, out eplayerKill9);
					}
					this.lastWarm = simulation;
					this.wasWarm = true;
				}
				else if (eplayerTemperature2 == EPlayerTemperature.WARM || this.warmth > 0u)
				{
					eplayerTemperature = EPlayerTemperature.WARM;
					this.lastWarm = simulation;
					this.wasWarm = true;
				}
				else if (base.player.movement.inSnow)
				{
					if (base.player.stance.stance == EPlayerStance.SWIM)
					{
						eplayerTemperature = EPlayerTemperature.FREEZING;
						if (Provider.isServer && simulation - this.lastFreeze > 25u)
						{
							this.lastFreeze = simulation;
							byte b = 8;
							if (base.player.clothing.shirt != 0 || base.player.clothing.vest != 0)
							{
								b -= 2;
							}
							if (base.player.clothing.pants != 0)
							{
								b -= 2;
							}
							if (base.player.clothing.hat != 0)
							{
								b -= 2;
							}
							EPlayerKill eplayerKill10;
							this.askDamage(b, Vector3.up, EDeathCause.FREEZING, ELimb.SPINE, Provider.server, out eplayerKill10);
						}
					}
					else if (Level.info != null && Level.info.configData.Snow_Affects_Temperature)
					{
						if (!this.wasWarm || simulation - this.lastWarm > 250f * (1f + base.player.skills.mastery(1, 5)))
						{
							bool flag3 = (base.player.movement.getVehicle() != null && !base.player.movement.getVehicle().asset.hasZip) || Physics.Raycast(base.transform.position + Vector3.up, Quaternion.Euler(45f, LevelLighting.wind, 0f) * -Vector3.forward, 32f, RayMasks.BLOCK_WIND);
							if (flag3)
							{
								eplayerTemperature = EPlayerTemperature.COVERED;
								this.lastCovered = simulation;
								this.wasCovered = true;
							}
							else
							{
								byte b2 = 1;
								if (base.player.clothing.shirt != 0 || base.player.clothing.vest != 0)
								{
									b2 += 1;
								}
								if (base.player.clothing.pants != 0)
								{
									b2 += 1;
								}
								if (base.player.clothing.hat != 0)
								{
									b2 += 1;
								}
								if (!this.wasCovered || (ulong)(simulation - this.lastCovered) > (ulong)((long)(50 * b2)))
								{
									eplayerTemperature = EPlayerTemperature.FREEZING;
									if (Provider.isServer && simulation - this.lastFreeze > 75u)
									{
										this.lastFreeze = simulation;
										byte b3 = 4;
										if (base.player.clothing.shirt != 0 || base.player.clothing.vest != 0)
										{
											b3 -= 1;
										}
										if (base.player.clothing.pants != 0)
										{
											b3 -= 1;
										}
										if (base.player.clothing.hat != 0)
										{
											b3 -= 1;
										}
										EPlayerKill eplayerKill11;
										this.askDamage(b3, Vector3.up, EDeathCause.FREEZING, ELimb.SPINE, Provider.server, out eplayerKill11);
									}
								}
								else
								{
									eplayerTemperature = EPlayerTemperature.COLD;
								}
							}
						}
						else
						{
							eplayerTemperature = EPlayerTemperature.COLD;
							this.lastCovered = simulation;
							this.wasCovered = true;
						}
					}
					else
					{
						eplayerTemperature = EPlayerTemperature.NONE;
					}
				}
				else
				{
					eplayerTemperature = EPlayerTemperature.NONE;
				}
				if (eplayerTemperature != this.temperature)
				{
					this._temperature = eplayerTemperature;
					if (this.onTemperatureUpdated != null)
					{
						this.onTemperatureUpdated(this.temperature);
					}
				}
			}
		}

		public void breakLegs()
		{
			this.lastBroken = base.player.input.simulation;
			if (!this.isBroken)
			{
				this._isBroken = true;
				base.channel.send("tellBroken", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
				{
					this.isBroken
				});
				EffectManager.sendEffect(31, EffectManager.SMALL, base.transform.position);
			}
		}

		private void onLanded(float fall)
		{
			if (fall < -5.5f && base.player.movement.gravity > 0.67f)
			{
				if (fall < -50f)
				{
					fall = -50f;
				}
				if (Provider.modeConfigData.Players.Can_Hurt_Legs)
				{
					EPlayerKill eplayerKill;
					this.askDamage((byte)(Mathf.Abs(fall) * (1f - base.player.skills.mastery(1, 4) * 0.75f)), Vector3.down, EDeathCause.BONES, ELimb.SPINE, Provider.server, out eplayerKill);
				}
				if (Provider.modeConfigData.Players.Can_Break_Legs)
				{
					this.breakLegs();
				}
			}
		}

		private void Start()
		{
			if (Provider.isServer)
			{
				PlayerMovement movement = base.player.movement;
				movement.onLanded = (Landed)Delegate.Combine(movement.onLanded, new Landed(this.onLanded));
				this.load();
			}
			base.Invoke("init", 0.1f);
		}

		public void load()
		{
			this._isDead = false;
			if (PlayerSavedata.fileExists(base.channel.owner.playerID, "/Player/Life.dat") && Level.info.type == ELevelType.SURVIVAL)
			{
				Block block = PlayerSavedata.readBlock(base.channel.owner.playerID, "/Player/Life.dat", 0);
				byte b = block.readByte();
				if (b > 1)
				{
					this._health = block.readByte();
					this._food = block.readByte();
					this._water = block.readByte();
					this._virus = block.readByte();
					this._stamina = 100;
					this._oxygen = 100;
					this._isBleeding = block.readBoolean();
					this._isBroken = block.readBoolean();
					this._temperature = EPlayerTemperature.NONE;
					this.wasWarm = false;
					this.wasCovered = false;
					return;
				}
			}
			this._health = 100;
			this._food = 100;
			this._water = 100;
			this._virus = 100;
			this._stamina = 100;
			this._oxygen = 100;
			this._isBleeding = false;
			this._isBroken = false;
			this._temperature = EPlayerTemperature.NONE;
			this.wasWarm = false;
			this.wasCovered = false;
			this.recentKiller = CSteamID.Nil;
			this.lastTimeAggressive = -100f;
			this.lastTimeTookDamage = -100f;
			this.lastTimeCausedDamage = -100f;
		}

		public void save()
		{
			if (base.player.life.isDead)
			{
				if (PlayerSavedata.fileExists(base.channel.owner.playerID, "/Player/Life.dat"))
				{
					PlayerSavedata.deleteFile(base.channel.owner.playerID, "/Player/Life.dat");
				}
			}
			else
			{
				Block block = new Block();
				block.writeByte(PlayerLife.SAVEDATA_VERSION);
				block.writeByte(this.health);
				block.writeByte(this.food);
				block.writeByte(this.water);
				block.writeByte(this.virus);
				block.writeBoolean(this.isBleeding);
				block.writeBoolean(this.isBroken);
				PlayerSavedata.writeBlock(base.channel.owner.playerID, "/Player/Life.dat", block);
			}
		}

		public static readonly byte SAVEDATA_VERSION = 2;

		private static readonly float COMBAT_COOLDOWN = 30f;

		public static PlayerLifeUpdated onPlayerLifeUpdated;

		public LifeUpdated onLifeUpdated;

		public HealthUpdated onHealthUpdated;

		public FoodUpdated onFoodUpdated;

		public WaterUpdated onWaterUpdated;

		public VirusUpdated onVirusUpdated;

		public StaminaUpdated onStaminaUpdated;

		public VisionUpdated onVisionUpdated;

		public OxygenUpdated onOxygenUpdated;

		public BleedingUpdated onBleedingUpdated;

		public BrokenUpdated onBrokenUpdated;

		public TemperatureUpdated onTemperatureUpdated;

		public Damaged onDamaged;

		private static EDeathCause _deathCause;

		private static ELimb _deathLimb;

		private static CSteamID _deathKiller;

		private CSteamID recentKiller;

		private float lastTimeAggressive;

		private float lastTimeTookDamage;

		private float lastTimeCausedDamage;

		private bool _isDead;

		private byte lastHealth;

		private byte _health;

		private byte _food;

		private byte _water;

		private byte _virus;

		private byte _vision;

		private uint _warmth;

		private byte _stamina;

		private byte _oxygen;

		private bool _isBleeding;

		private bool _isBroken;

		private EPlayerTemperature _temperature;

		private uint lastStarve;

		private uint lastDehydrate;

		private uint lastUncleaned;

		private uint lastView;

		private uint lastTire;

		private uint lastSuffocate;

		private uint lastRest;

		private uint lastBreath;

		private uint lastInfect;

		private uint lastBleed;

		private uint lastBleeding;

		private uint lastBroken;

		private uint lastFreeze;

		private uint lastWarm;

		private uint lastBurn;

		private uint lastCovered;

		private uint lastRegenerate;

		private uint lastRadiate;

		private bool wasWarm;

		private bool wasCovered;

		private float _lastRespawn;

		private float _lastDeath;

		private float lastSuicide;

		private float lastAlive;

		private Vector3 ragdoll;

		private PlayerSpawnpoint spawnpoint;
	}
}
