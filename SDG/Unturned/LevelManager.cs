using System;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	public class LevelManager : SteamCaller
	{
		private static int ARENA_PLAYERS
		{
			get
			{
				return (!Dedicator.isDedicated) ? 1 : 2;
			}
		}

		public static LevelManager instance
		{
			get
			{
				return LevelManager.manager;
			}
		}

		public static ELevelType levelType
		{
			get
			{
				return LevelManager._levelType;
			}
		}

		public static Vector3 arenaCenter
		{
			get
			{
				return LevelManager._arenaCenter;
			}
		}

		public static float arenaRadius
		{
			get
			{
				return LevelManager._arenaRadius;
			}
		}

		public static bool isPlayerInArena(Player player)
		{
			if (LevelManager.arenaState == EArenaState.CLEAR || LevelManager.arenaState == EArenaState.PLAY || LevelManager.arenaState == EArenaState.FINALE || LevelManager.arenaState == EArenaState.RESTART)
			{
				foreach (ArenaPlayer arenaPlayer in LevelManager.arenaPlayers)
				{
					if (arenaPlayer.steamPlayer != null && arenaPlayer.steamPlayer.player == player)
					{
						return true;
					}
				}
				return false;
			}
			return false;
		}

		private void findGroups()
		{
			LevelManager.nonGroups = 0;
			LevelManager.arenaGroups.Clear();
			for (int i = 0; i < Provider.clients.Count; i++)
			{
				SteamPlayer steamPlayer = Provider.clients[i];
				if (steamPlayer != null && !(steamPlayer.player == null) && !steamPlayer.player.life.isDead)
				{
					if (!steamPlayer.player.quests.isMemberOfAGroup)
					{
						LevelManager.nonGroups++;
					}
					else if (!LevelManager.arenaGroups.Contains(steamPlayer.player.quests.groupID))
					{
						LevelManager.arenaGroups.Add(steamPlayer.player.quests.groupID);
					}
				}
			}
		}

		private void updateGroups(SteamPlayer steamPlayer)
		{
			if (!steamPlayer.player.quests.isMemberOfAGroup)
			{
				LevelManager.nonGroups--;
			}
			else
			{
				for (int i = LevelManager.arenaPlayers.Count - 1; i >= 0; i--)
				{
					ArenaPlayer arenaPlayer = LevelManager.arenaPlayers[i];
					if (arenaPlayer.steamPlayer.player.quests.isMemberOfSameGroupAs(steamPlayer.player))
					{
						return;
					}
				}
				LevelManager.arenaGroups.Remove(steamPlayer.player.quests.groupID);
			}
		}

		private void arenaLobby()
		{
			this.findGroups();
			if (LevelManager.nonGroups + LevelManager.arenaGroups.Count < LevelManager.ARENA_PLAYERS)
			{
				if (LevelManager.arenaMessage != EArenaMessage.LOBBY)
				{
					base.channel.send("tellArenaMessage", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
					{
						0
					});
				}
				return;
			}
			LevelManager.arenaState = EArenaState.CLEAR;
		}

		private void arenaClear()
		{
			VehicleManager.askVehicleDestroyAll();
			BarricadeManager.askClearAllBarricades();
			StructureManager.askClearAllStructures();
			ItemManager.askClearAllItems();
			EffectManager.askEffectClearAll();
			ObjectManager.askClearAllObjects();
			LevelManager.arenaPlayers.Clear();
			Vector3 vector = Vector3.zero;
			float num = (float)Level.size / 2f;
			if (LevelManager.arenaNodes.Count > 0)
			{
				ArenaNode arenaNode = LevelManager.arenaNodes[Random.Range(0, LevelManager.arenaNodes.Count)];
				vector = arenaNode.point;
				vector.y = 0f;
				num = arenaNode.radius;
			}
			base.channel.send("tellArenaOrigin", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
			{
				vector,
				num,
				false
			});
			LevelManager.arenaState = EArenaState.WARMUP;
			base.channel.send("tellLevelTimer", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
			{
				5
			});
		}

		private void arenaWarmUp()
		{
			if (LevelManager.arenaMessage != EArenaMessage.WARMUP)
			{
				base.channel.send("tellArenaMessage", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
				{
					1
				});
			}
			if (LevelManager.countTimerMessages >= 0)
			{
				return;
			}
			this.findGroups();
			if (LevelManager.nonGroups + LevelManager.arenaGroups.Count < LevelManager.ARENA_PLAYERS)
			{
				LevelManager.arenaState = EArenaState.LOBBY;
			}
			else
			{
				LevelManager.arenaState = EArenaState.SPAWN;
			}
		}

		private void arenaSpawn()
		{
			for (byte b = 0; b < Regions.WORLD_SIZE; b += 1)
			{
				for (byte b2 = 0; b2 < Regions.WORLD_SIZE; b2 += 1)
				{
					if (LevelItems.spawns[(int)b, (int)b2].Count > 0)
					{
						for (int i = 0; i < LevelItems.spawns[(int)b, (int)b2].Count; i++)
						{
							ItemSpawnpoint itemSpawnpoint = LevelItems.spawns[(int)b, (int)b2][i];
							ushort item = LevelItems.getItem(itemSpawnpoint);
							if (item != 0)
							{
								Item item2 = new Item(item, EItemOrigin.ADMIN);
								ItemManager.dropItem(item2, itemSpawnpoint.point, false, false, false);
							}
						}
					}
				}
			}
			List<VehicleSpawnpoint> spawns = LevelVehicles.spawns;
			for (int j = 0; j < spawns.Count; j++)
			{
				VehicleSpawnpoint vehicleSpawnpoint = spawns[j];
				ushort vehicle = LevelVehicles.getVehicle(vehicleSpawnpoint);
				if (vehicle != 0)
				{
					Vector3 point = vehicleSpawnpoint.point;
					point.y += 1f;
					VehicleManager.spawnVehicle(vehicle, point, Quaternion.Euler(0f, vehicleSpawnpoint.angle, 0f));
				}
			}
			List<PlayerSpawnpoint> altSpawns = LevelPlayers.getAltSpawns();
			float num = LevelManager.arenaRadius - SafezoneNode.MIN_SIZE;
			num *= num;
			for (int k = altSpawns.Count - 1; k >= 0; k--)
			{
				PlayerSpawnpoint playerSpawnpoint = altSpawns[k];
				float num2 = Mathf.Pow(playerSpawnpoint.point.x - LevelManager.arenaCenter.x, 2f) + Mathf.Pow(playerSpawnpoint.point.z - LevelManager.arenaCenter.z, 2f);
				if (num2 > num)
				{
					altSpawns.RemoveAt(k);
				}
			}
			for (int l = 0; l < Provider.clients.Count; l++)
			{
				if (altSpawns.Count == 0)
				{
					break;
				}
				SteamPlayer steamPlayer = Provider.clients[l];
				if (steamPlayer != null && !(steamPlayer.player == null) && !steamPlayer.player.life.isDead)
				{
					int index = Random.Range(0, altSpawns.Count);
					PlayerSpawnpoint playerSpawnpoint2 = altSpawns[index];
					altSpawns.RemoveAt(index);
					ArenaPlayer arenaPlayer = new ArenaPlayer(steamPlayer);
					arenaPlayer.steamPlayer.player.life.sendRevive();
					arenaPlayer.steamPlayer.player.sendTeleport(playerSpawnpoint2.point, MeasurementTool.angleToByte(playerSpawnpoint2.angle));
					LevelManager.arenaPlayers.Add(arenaPlayer);
				}
			}
			LevelManager.arenaState = EArenaState.PLAY;
			base.channel.send("tellLevelNumber", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
			{
				(byte)LevelManager.arenaPlayers.Count
			});
		}

		private void arenaPlay()
		{
			if (LevelManager.arenaMessage != EArenaMessage.PLAY)
			{
				base.channel.send("tellArenaMessage", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
				{
					2
				});
			}
			if (LevelManager.nonGroups + LevelManager.arenaGroups.Count < LevelManager.ARENA_PLAYERS)
			{
				LevelManager.arenaState = EArenaState.FINALE;
				LevelManager.lastFinaleMessage = Time.realtimeSinceStartup;
				if (LevelManager.arenaPlayers.Count > 0)
				{
					ulong[] array = new ulong[LevelManager.arenaPlayers.Count];
					for (int i = 0; i < LevelManager.arenaPlayers.Count; i++)
					{
						array[i] = LevelManager.arenaPlayers[i].steamPlayer.playerID.steamID.m_SteamID;
					}
					LevelManager.arenaMessage = EArenaMessage.LOSE;
					base.channel.send("tellArenaPlayer", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
					{
						array,
						5
					});
				}
				else
				{
					base.channel.send("tellArenaMessage", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
					{
						6
					});
				}
			}
			else
			{
				for (int j = LevelManager.arenaPlayers.Count - 1; j >= 0; j--)
				{
					ArenaPlayer arenaPlayer = LevelManager.arenaPlayers[j];
					if (arenaPlayer.steamPlayer == null || arenaPlayer.steamPlayer.player == null)
					{
						ulong[] array2 = new ulong[]
						{
							arenaPlayer.steamPlayer.playerID.steamID.m_SteamID
						};
						base.channel.send("tellArenaPlayer", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
						{
							array2,
							4
						});
						LevelManager.arenaPlayers.RemoveAt(j);
						this.updateGroups(arenaPlayer.steamPlayer);
						base.channel.send("tellLevelNumber", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
						{
							(byte)LevelManager.arenaPlayers.Count
						});
					}
					else
					{
						if (Time.realtimeSinceStartup - arenaPlayer.lastAreaDamage > 1f)
						{
							float num = Mathf.Pow(arenaPlayer.steamPlayer.player.transform.position.x - LevelManager.arenaCenter.x, 2f) + Mathf.Pow(arenaPlayer.steamPlayer.player.transform.position.z - LevelManager.arenaCenter.z, 2f);
							if (num > LevelManager.arenaSqrRadius || LevelManager.arenaRadius < 1f)
							{
								EPlayerKill eplayerKill;
								arenaPlayer.steamPlayer.player.life.askDamage(10, Vector3.up * 10f, EDeathCause.ARENA, ELimb.SPINE, CSteamID.Nil, out eplayerKill);
								arenaPlayer.lastAreaDamage = Time.realtimeSinceStartup;
							}
						}
						if (arenaPlayer.hasDied)
						{
							ulong[] array3 = new ulong[]
							{
								arenaPlayer.steamPlayer.playerID.steamID.m_SteamID
							};
							base.channel.send("tellArenaPlayer", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
							{
								array3,
								3
							});
							LevelManager.arenaPlayers.RemoveAt(j);
							this.updateGroups(arenaPlayer.steamPlayer);
							base.channel.send("tellLevelNumber", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
							{
								(byte)LevelManager.arenaPlayers.Count
							});
						}
					}
				}
			}
		}

		private void arenaFinale()
		{
			if (Time.realtimeSinceStartup - LevelManager.lastFinaleMessage > 10f)
			{
				LevelManager.arenaState = EArenaState.RESTART;
			}
		}

		private void arenaRestart()
		{
			LevelManager.arenaState = EArenaState.INTERMISSION;
			base.channel.send("tellLevelTimer", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
			{
				15
			});
			for (int i = 0; i < LevelManager.arenaPlayers.Count; i++)
			{
				ArenaPlayer arenaPlayer = LevelManager.arenaPlayers[i];
				if (!arenaPlayer.hasDied && arenaPlayer.steamPlayer != null && !(arenaPlayer.steamPlayer.player == null))
				{
					arenaPlayer.steamPlayer.player.sendStat(EPlayerStat.ARENA_WINS);
				}
			}
			for (int j = 0; j < Provider.clients.Count; j++)
			{
				SteamPlayer steamPlayer = Provider.clients[j];
				if (steamPlayer != null && !(steamPlayer.player == null) && !steamPlayer.player.life.isDead && !steamPlayer.player.movement.isSafe)
				{
					EPlayerKill eplayerKill;
					steamPlayer.player.life.askDamage(101, Vector3.up * 101f, EDeathCause.ARENA, ELimb.SPINE, CSteamID.Nil, out eplayerKill);
				}
			}
		}

		private void arenaIntermission()
		{
			if (LevelManager.arenaMessage != EArenaMessage.INTERMISSION)
			{
				base.channel.send("tellArenaMessage", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
				{
					7
				});
			}
			if (LevelManager.countTimerMessages >= 0)
			{
				return;
			}
			LevelManager.arenaState = EArenaState.LOBBY;
		}

		private void arenaTick()
		{
			if (Time.realtimeSinceStartup > LevelManager.nextAreaModify)
			{
				LevelManager._arenaRadius = Mathf.Max(0.5f, LevelManager.arenaRadius - Time.deltaTime * Level.fog);
				LevelManager.arenaSqrRadius = LevelManager.arenaRadius * LevelManager.arenaRadius;
			}
			if (!Dedicator.isDedicated && LevelManager.arenaArea != null)
			{
				LevelManager.arenaArea.position = LevelManager.arenaCenter;
				LevelManager.arenaArea.localScale = new Vector3(LevelManager.arenaRadius, LevelManager.arenaRadius, Level.HEIGHT);
			}
			if (LevelManager.countTimerMessages >= 0 && Time.realtimeSinceStartup - LevelManager.lastTimerMessage > 1f)
			{
				if (LevelManager.onLevelNumberUpdated != null)
				{
					LevelManager.onLevelNumberUpdated(LevelManager.countTimerMessages);
				}
				LevelManager.lastTimerMessage = Time.realtimeSinceStartup;
				LevelManager.countTimerMessages--;
				if (LevelManager.arenaMessage == EArenaMessage.WARMUP && !Dedicator.isDedicated && MainCamera.instance != null && OptionsSettings.timer)
				{
					MainCamera.instance.GetComponent<AudioSource>().PlayOneShot(LevelManager.timer, 1f);
				}
			}
			if (Provider.isServer)
			{
				switch (LevelManager.arenaState)
				{
				case EArenaState.LOBBY:
					this.arenaLobby();
					break;
				case EArenaState.CLEAR:
					this.arenaClear();
					break;
				case EArenaState.WARMUP:
					this.arenaWarmUp();
					break;
				case EArenaState.SPAWN:
					this.arenaSpawn();
					break;
				case EArenaState.PLAY:
					this.arenaPlay();
					break;
				case EArenaState.FINALE:
					this.arenaFinale();
					break;
				case EArenaState.RESTART:
					this.arenaRestart();
					break;
				case EArenaState.INTERMISSION:
					this.arenaIntermission();
					break;
				}
			}
		}

		private void arenaInit()
		{
			LevelManager._arenaCenter = Vector3.zero;
			LevelManager._arenaRadius = 16384f;
			if (!Dedicator.isDedicated && !Level.isEditor)
			{
				LevelManager.arenaArea = ((GameObject)Object.Instantiate(Resources.Load("Level/Area"))).transform;
				LevelManager.arenaArea.name = "Area";
				LevelManager.arenaArea.localRotation = Quaternion.Euler(-90f, 0f, 0f);
				LevelManager.arenaArea.parent = Level.clips;
			}
			if (Provider.isServer)
			{
				LevelManager.arenaState = EArenaState.LOBBY;
				LevelManager.arenaGroups = new List<CSteamID>();
				LevelManager.arenaPlayers = new List<ArenaPlayer>();
				LevelManager.arenaNodes = new List<ArenaNode>();
				for (int i = 0; i < LevelNodes.nodes.Count; i++)
				{
					Node node = LevelNodes.nodes[i];
					if (node.type == ENodeType.ARENA)
					{
						LevelManager.arenaNodes.Add((ArenaNode)node);
					}
				}
			}
		}

		[SteamCall]
		public void tellArenaOrigin(CSteamID steamID, Vector3 newArenaCenter, float newArenaRadius, bool isPlaying)
		{
			if (base.channel.checkServer(steamID))
			{
				LevelManager._arenaCenter = newArenaCenter;
				LevelManager._arenaRadius = newArenaRadius;
				LevelManager.arenaSqrRadius = LevelManager.arenaRadius * LevelManager.arenaRadius;
				if (isPlaying)
				{
					LevelManager.nextAreaModify = 0f;
				}
				else
				{
					LevelManager.nextAreaModify = Time.realtimeSinceStartup + 6f;
				}
			}
		}

		[SteamCall]
		public void tellArenaMessage(CSteamID steamID, byte newArenaMessage)
		{
			if (base.channel.checkServer(steamID))
			{
				LevelManager.arenaMessage = (EArenaMessage)newArenaMessage;
				if (LevelManager.onArenaMessageUpdated != null)
				{
					LevelManager.onArenaMessageUpdated(LevelManager.arenaMessage);
				}
			}
		}

		[SteamCall]
		public void tellArenaPlayer(CSteamID steamID, ulong[] newPlayerIDs, byte newArenaMessage)
		{
			if (base.channel.checkServer(steamID) && LevelManager.onArenaPlayerUpdated != null)
			{
				LevelManager.onArenaPlayerUpdated(newPlayerIDs, (EArenaMessage)newArenaMessage);
			}
		}

		[SteamCall]
		public void tellLevelNumber(CSteamID steamID, byte newLevelNumber)
		{
			if (base.channel.checkServer(steamID))
			{
				LevelManager.countTimerMessages = -1;
				if (LevelManager.onLevelNumberUpdated != null)
				{
					LevelManager.onLevelNumberUpdated((int)newLevelNumber);
				}
			}
		}

		[SteamCall]
		public void tellLevelTimer(CSteamID steamID, byte newTimerCount)
		{
			if (base.channel.checkServer(steamID))
			{
				LevelManager.countTimerMessages = (int)newTimerCount;
			}
		}

		[SteamCall]
		public void askArenaState(CSteamID steamID)
		{
			base.channel.send("tellArenaOrigin", steamID, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
			{
				LevelManager.arenaCenter,
				LevelManager.arenaRadius,
				LevelManager.arenaState == EArenaState.PLAY
			});
			base.channel.send("tellArenaMessage", steamID, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
			{
				(byte)LevelManager.arenaMessage
			});
			if (LevelManager.countTimerMessages > 0)
			{
				base.channel.send("tellLevelTimer", steamID, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
				{
					(byte)LevelManager.countTimerMessages
				});
			}
			else
			{
				base.channel.send("tellLevelNumber", steamID, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
				{
					(byte)LevelManager.arenaPlayers.Count
				});
			}
		}

		public static bool hasAirdrop
		{
			get
			{
				return LevelManager._hasAirdrop;
			}
		}

		public static void airdrop(Vector3 point, ushort id)
		{
			if (id == 0)
			{
				return;
			}
			Vector3 vector = Vector3.zero;
			if (Random.value < 0.5f)
			{
				vector.x = (float)(Level.size / 2) * -Mathf.Sign(point.x);
				vector.z = (float)Random.Range(0, (int)(Level.size / 2)) * -Mathf.Sign(point.z);
			}
			else
			{
				vector.x = (float)Random.Range(0, (int)(Level.size / 2)) * -Mathf.Sign(point.x);
				vector.z = (float)(Level.size / 2) * -Mathf.Sign(point.z);
			}
			point.y = 0f;
			Vector3 normalized = (point - vector).normalized;
			vector += normalized * -2048f;
			float num = (point - vector).magnitude / 128f;
			vector.y = 1024f;
			LevelManager.manager.airdropSpawn(id, vector, normalized, num);
			LevelManager.manager.channel.send("tellAirdropState", ESteamCall.OTHERS, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
			{
				vector,
				normalized,
				num
			});
		}

		private void airdropTick()
		{
			for (int i = LevelManager.airdrops.Count - 1; i >= 0; i--)
			{
				AirdropInfo airdropInfo = LevelManager.airdrops[i];
				airdropInfo.state += airdropInfo.direction * 128f * Time.deltaTime;
				airdropInfo.delay -= Time.deltaTime;
				if (airdropInfo.model != null)
				{
					airdropInfo.model.position = airdropInfo.state;
				}
				if (airdropInfo.dropped)
				{
					if (Mathf.Abs(airdropInfo.state.x) > (float)(Level.size / 2 + 2048) || Mathf.Abs(airdropInfo.state.z) > (float)(Level.size / 2 + 2048))
					{
						if (airdropInfo.model != null)
						{
							Object.Destroy(airdropInfo.model.gameObject);
						}
						LevelManager.airdrops.RemoveAt(i);
					}
				}
				else if (airdropInfo.delay <= 0f)
				{
					airdropInfo.dropped = true;
					Transform transform;
					if (Dedicator.isDedicated)
					{
						transform = ((GameObject)Object.Instantiate(Resources.Load("Level/Carepackage_Server"))).transform;
					}
					else
					{
						transform = ((GameObject)Object.Instantiate(Resources.Load("Level/Carepackage_Client"))).transform;
					}
					transform.name = "Carepackage";
					transform.parent = Level.effects;
					transform.position = airdropInfo.state;
					transform.rotation = Quaternion.identity;
					if (Provider.isServer)
					{
						transform.GetComponent<Carepackage>().id = airdropInfo.id;
					}
					if (Dedicator.isDedicated)
					{
						LevelManager.airdrops.RemoveAt(i);
					}
				}
			}
			if (Provider.isServer && LevelManager.airdropNodes.Count > 0)
			{
				if (!LevelManager.hasAirdrop)
				{
					LevelManager.airdropFrequency = (uint)(Random.Range(Provider.modeConfigData.Events.Airdrop_Frequency_Min, Provider.modeConfigData.Events.Airdrop_Frequency_Max) * LightingManager.cycle);
					LevelManager._hasAirdrop = true;
					LevelManager.lastAirdrop = Time.realtimeSinceStartup;
				}
				if (LevelManager.airdropFrequency > 0u)
				{
					if (Time.realtimeSinceStartup - LevelManager.lastAirdrop > 1f)
					{
						LevelManager.airdropFrequency -= 1u;
						LevelManager.lastAirdrop = Time.realtimeSinceStartup;
					}
				}
				else
				{
					AirdropNode airdropNode = LevelManager.airdropNodes[Random.Range(0, LevelManager.airdropNodes.Count)];
					LevelManager.airdrop(airdropNode.point, airdropNode.id);
					LevelManager._hasAirdrop = false;
				}
			}
		}

		private void airdropInit()
		{
			LevelManager.lastAirdrop = Time.realtimeSinceStartup;
			LevelManager.airdrops = new List<AirdropInfo>();
			if (Provider.isServer)
			{
				LevelManager.airdropNodes = new List<AirdropNode>();
				for (int i = 0; i < LevelNodes.nodes.Count; i++)
				{
					Node node = LevelNodes.nodes[i];
					if (node.type == ENodeType.AIRDROP)
					{
						AirdropNode airdropNode = (AirdropNode)node;
						if (airdropNode.id != 0)
						{
							LevelManager.airdropNodes.Add(airdropNode);
						}
					}
				}
				LevelManager.load();
			}
		}

		private void airdropSpawn(ushort id, Vector3 state, Vector3 direction, float delay)
		{
			AirdropInfo airdropInfo = new AirdropInfo();
			airdropInfo.id = id;
			airdropInfo.state = state;
			airdropInfo.direction = direction;
			airdropInfo.delay = delay;
			airdropInfo.dropped = false;
			if (!Dedicator.isDedicated)
			{
				Transform transform = ((GameObject)Object.Instantiate(Resources.Load("Level/Dropship"))).transform;
				transform.name = "Dropship";
				transform.parent = Level.effects;
				transform.position = state;
				transform.rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(-90f, 180f, 0f);
				airdropInfo.model = transform;
			}
			LevelManager.airdrops.Add(airdropInfo);
		}

		[SteamCall]
		public void tellAirdropState(CSteamID steamID, Vector3 state, Vector3 direction, float delay)
		{
			if (base.channel.checkServer(steamID))
			{
				this.airdropSpawn(0, state, direction, delay);
			}
		}

		[SteamCall]
		public void askAirdropState(CSteamID steamID)
		{
			for (int i = 0; i < LevelManager.airdrops.Count; i++)
			{
				AirdropInfo airdropInfo = LevelManager.airdrops[i];
				base.channel.send("tellAirdropState", steamID, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
				{
					airdropInfo.state,
					airdropInfo.direction,
					airdropInfo.delay
				});
			}
		}

		private void onClientConnected()
		{
			if (Level.info.type == ELevelType.ARENA)
			{
				base.channel.send("askArenaState", ESteamCall.SERVER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[0]);
			}
			else if (Level.info.type == ELevelType.SURVIVAL)
			{
				base.channel.send("askAirdropState", ESteamCall.SERVER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[0]);
			}
		}

		private void onLevelLoaded(int level)
		{
			LevelManager.isInit = false;
			if (level > Level.SETUP && Level.info != null)
			{
				LevelManager.isInit = true;
				LevelManager._levelType = Level.info.type;
				if (LevelManager.levelType == ELevelType.ARENA)
				{
					this.arenaInit();
				}
				else if (LevelManager.levelType == ELevelType.SURVIVAL)
				{
					this.airdropInit();
				}
			}
		}

		private void Update()
		{
			if (!LevelManager.isInit)
			{
				return;
			}
			if (LevelManager.levelType == ELevelType.ARENA)
			{
				this.arenaTick();
			}
			else if (LevelManager.levelType == ELevelType.SURVIVAL)
			{
				this.airdropTick();
			}
		}

		private void Start()
		{
			LevelManager.manager = this;
			if (!Dedicator.isDedicated)
			{
				LevelManager.timer = (AudioClip)Resources.Load("Sounds/General/Timer");
			}
			Level.onLevelLoaded = (LevelLoaded)Delegate.Combine(Level.onLevelLoaded, new LevelLoaded(this.onLevelLoaded));
			Provider.onClientConnected = (Provider.ClientConnected)Delegate.Combine(Provider.onClientConnected, new Provider.ClientConnected(this.onClientConnected));
		}

		public static void load()
		{
			if (LevelSavedata.fileExists("/Events.dat"))
			{
				River river = LevelSavedata.openRiver("/Events.dat", true);
				byte b = river.readByte();
				if (b > 0)
				{
					LevelManager.airdropFrequency = river.readUInt32();
					LevelManager._hasAirdrop = river.readBoolean();
					return;
				}
			}
			LevelManager._hasAirdrop = false;
		}

		public static void save()
		{
			River river = LevelSavedata.openRiver("/Events.dat", false);
			river.writeByte(LevelManager.SAVEDATA_VERSION);
			river.writeUInt32(LevelManager.airdropFrequency);
			river.writeBoolean(LevelManager.hasAirdrop);
		}

		public static readonly byte SAVEDATA_VERSION = 1;

		private static LevelManager manager;

		private static bool isInit;

		private static ELevelType _levelType;

		private static AudioClip timer;

		private static float lastFinaleMessage;

		private static float lastTimerMessage;

		private static float nextAreaModify;

		private static int countTimerMessages;

		public static EArenaState arenaState;

		public static EArenaMessage arenaMessage;

		private static int nonGroups;

		public static List<CSteamID> arenaGroups;

		public static List<ArenaPlayer> arenaPlayers;

		private static List<ArenaNode> arenaNodes;

		private static Vector3 _arenaCenter;

		private static float _arenaRadius;

		private static float arenaSqrRadius;

		private static Transform arenaArea;

		public static ArenaMessageUpdated onArenaMessageUpdated;

		public static ArenaPlayerUpdated onArenaPlayerUpdated;

		public static LevelNumberUpdated onLevelNumberUpdated;

		private static List<AirdropNode> airdropNodes;

		private static List<AirdropInfo> airdrops;

		public static uint airdropFrequency;

		private static bool _hasAirdrop;

		private static float lastAirdrop;
	}
}
