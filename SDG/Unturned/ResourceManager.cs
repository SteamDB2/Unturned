using System;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	public class ResourceManager : SteamCaller
	{
		public static void getResourcesInRadius(Vector3 center, float sqrRadius, List<RegionCoordinate> search, List<Transform> result)
		{
			if (ResourceManager.regions == null)
			{
				return;
			}
			for (int i = 0; i < search.Count; i++)
			{
				RegionCoordinate regionCoordinate = search[i];
				if (ResourceManager.regions[(int)regionCoordinate.x, (int)regionCoordinate.y] != null)
				{
					for (int j = 0; j < LevelGround.trees[(int)regionCoordinate.x, (int)regionCoordinate.y].Count; j++)
					{
						ResourceSpawnpoint resourceSpawnpoint = LevelGround.trees[(int)regionCoordinate.x, (int)regionCoordinate.y][j];
						if (!(resourceSpawnpoint.model == null) && !resourceSpawnpoint.isDead)
						{
							if ((resourceSpawnpoint.point - center).sqrMagnitude < sqrRadius)
							{
								result.Add(resourceSpawnpoint.model);
							}
						}
					}
				}
			}
		}

		public static void damage(Transform resource, Vector3 direction, float damage, float times, float drop, out EPlayerKill kill, out uint xp)
		{
			xp = 0u;
			kill = EPlayerKill.NONE;
			byte b;
			byte b2;
			if (Regions.tryGetCoordinate(resource.position, out b, out b2))
			{
				List<ResourceSpawnpoint> list = LevelGround.trees[(int)b, (int)b2];
				ushort num = 0;
				while ((int)num < list.Count)
				{
					if (resource == list[(int)num].model)
					{
						if (!list[(int)num].isDead)
						{
							ushort num2 = (ushort)(damage * times);
							list[(int)num].askDamage(num2);
							if (list[(int)num].isDead)
							{
								kill = EPlayerKill.RESOURCE;
								ResourceAsset asset = list[(int)num].asset;
								if (list[(int)num].asset != null)
								{
									if (asset.explosion != 0)
									{
										if (asset.hasDebris)
										{
											EffectManager.sendEffect(asset.explosion, b, b2, ResourceManager.RESOURCE_REGIONS, resource.position + Vector3.up * 8f);
										}
										else
										{
											EffectManager.sendEffect(asset.explosion, b, b2, ResourceManager.RESOURCE_REGIONS, resource.position);
										}
									}
									if (asset.rewardID != 0)
									{
										direction.y = 0f;
										direction.Normalize();
										int num3 = (int)((float)Random.Range((int)asset.rewardMin, (int)(asset.rewardMax + 1)) * drop);
										for (int i = 0; i < num3; i++)
										{
											ushort num4 = SpawnTableTool.resolve(asset.rewardID);
											if (num4 != 0)
											{
												if (asset.hasDebris)
												{
													ItemManager.dropItem(new Item(num4, EItemOrigin.NATURE), resource.position + direction * (float)(2 + i) + new Vector3(0f, 2f, 0f), false, Dedicator.isDedicated, true);
												}
												else
												{
													ItemManager.dropItem(new Item(num4, EItemOrigin.NATURE), resource.position + new Vector3(Random.Range(-2f, 2f), 2f, Random.Range(-2f, 2f)), false, Dedicator.isDedicated, true);
												}
											}
										}
									}
									else
									{
										if (asset.log != 0)
										{
											int num5 = (int)((float)Random.Range(3, 7) * drop);
											for (int j = 0; j < num5; j++)
											{
												ItemManager.dropItem(new Item(asset.log, EItemOrigin.NATURE), resource.position + direction * (float)(2 + j * 2) + Vector3.up, false, Dedicator.isDedicated, true);
											}
										}
										if (asset.stick != 0)
										{
											int num6 = (int)((float)Random.Range(2, 5) * drop);
											for (int k = 0; k < num6; k++)
											{
												float num7 = Random.Range(0f, 6.28318548f);
												ItemManager.dropItem(new Item(asset.stick, EItemOrigin.NATURE), resource.position + new Vector3(Mathf.Sin(num7) * 3f, 1f, Mathf.Cos(num7) * 3f), false, Dedicator.isDedicated, true);
											}
										}
									}
									xp = asset.rewardXP;
								}
								ResourceManager.manager.channel.send("tellResourceDead", ESteamCall.ALL, b, b2, ResourceManager.RESOURCE_REGIONS, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
								{
									b,
									b2,
									num,
									direction * (float)num2
								});
							}
						}
						break;
					}
					num += 1;
				}
			}
		}

		public static void forage(Transform resource)
		{
			byte b;
			byte b2;
			if (Regions.tryGetCoordinate(resource.position, out b, out b2))
			{
				List<ResourceSpawnpoint> list = LevelGround.trees[(int)b, (int)b2];
				ushort num = 0;
				while ((int)num < list.Count)
				{
					if (resource == list[(int)num].model)
					{
						ResourceManager.manager.channel.send("askForage", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[]
						{
							b,
							b2,
							num
						});
						break;
					}
					num += 1;
				}
			}
		}

		[SteamCall]
		public void askForage(CSteamID steamID, byte x, byte y, ushort index)
		{
			if (Provider.isServer)
			{
				if (!Regions.checkSafe((int)x, (int)y))
				{
					return;
				}
				Player player = PlayerTool.getPlayer(steamID);
				if (player == null)
				{
					return;
				}
				if (player.life.isDead)
				{
					return;
				}
				List<ResourceSpawnpoint> list = LevelGround.trees[(int)x, (int)y];
				if ((int)index >= list.Count)
				{
					return;
				}
				if (list[(int)index].isDead)
				{
					return;
				}
				ResourceAsset resourceAsset = (ResourceAsset)Assets.find(EAssetType.RESOURCE, LevelGround.resources[(int)list[(int)index].type].id);
				if (resourceAsset == null || !resourceAsset.isForage)
				{
					return;
				}
				list[(int)index].askDamage(1);
				if (resourceAsset.explosion != 0)
				{
					EffectManager.sendEffect(resourceAsset.explosion, x, y, ResourceManager.RESOURCE_REGIONS, list[(int)index].point);
				}
				ushort num;
				if (resourceAsset.rewardID != 0)
				{
					num = SpawnTableTool.resolve(resourceAsset.rewardID);
				}
				else
				{
					num = resourceAsset.log;
				}
				if (num != 0)
				{
					player.inventory.forceAddItem(new Item(num, EItemOrigin.NATURE), true);
					if (Random.value < player.skills.mastery(2, 5))
					{
						player.inventory.forceAddItem(new Item(num, EItemOrigin.NATURE), true);
					}
				}
				player.sendStat(EPlayerStat.FOUND_PLANTS);
				player.skills.askPay(1u);
				ResourceManager.manager.channel.send("tellResourceDead", ESteamCall.ALL, x, y, ResourceManager.RESOURCE_REGIONS, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
				{
					x,
					y,
					index,
					Vector3.zero
				});
			}
		}

		[SteamCall]
		public void tellResourceDead(CSteamID steamID, byte x, byte y, ushort index, Vector3 ragdoll)
		{
			if (base.channel.checkServer(steamID))
			{
				if ((int)index >= LevelGround.trees[(int)x, (int)y].Count)
				{
					return;
				}
				if (!Provider.isServer && !ResourceManager.regions[(int)x, (int)y].isNetworked)
				{
					return;
				}
				LevelGround.trees[(int)x, (int)y][(int)index].kill(ragdoll);
			}
		}

		[SteamCall]
		public void tellResourceAlive(CSteamID steamID, byte x, byte y, ushort index)
		{
			if (base.channel.checkServer(steamID))
			{
				if ((int)index >= LevelGround.trees[(int)x, (int)y].Count)
				{
					return;
				}
				if (!Provider.isServer && !ResourceManager.regions[(int)x, (int)y].isNetworked)
				{
					return;
				}
				LevelGround.trees[(int)x, (int)y][(int)index].revive();
			}
		}

		[SteamCall]
		public void tellResources(CSteamID steamID, byte x, byte y, bool[] resources)
		{
			if (base.channel.checkServer(steamID))
			{
				if (!Regions.checkSafe((int)x, (int)y))
				{
					return;
				}
				if (ResourceManager.regions[(int)x, (int)y].isNetworked)
				{
					return;
				}
				ResourceManager.regions[(int)x, (int)y].isNetworked = true;
				ushort num = 0;
				while ((int)num < resources.Length)
				{
					if (resources[(int)num])
					{
						LevelGround.trees[(int)x, (int)y][(int)num].wipe();
					}
					else
					{
						LevelGround.trees[(int)x, (int)y][(int)num].revive();
					}
					num += 1;
				}
			}
		}

		[SteamCall]
		public void askResources(CSteamID steamID, byte x, byte y)
		{
			bool[] array = new bool[LevelGround.trees[(int)x, (int)y].Count];
			ushort num = 0;
			while ((int)num < array.Length)
			{
				array[(int)num] = LevelGround.trees[(int)x, (int)y][(int)num].isDead;
				num += 1;
			}
			base.channel.send("tellResources", steamID, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
			{
				x,
				y,
				array
			});
		}

		public static ResourceSpawnpoint getResourceSpawnpoint(byte x, byte y, ushort index)
		{
			if (!Regions.checkSafe((int)x, (int)y))
			{
				return null;
			}
			List<ResourceSpawnpoint> list = LevelGround.trees[(int)x, (int)y];
			if ((int)index >= list.Count)
			{
				return null;
			}
			return list[(int)index];
		}

		public static Transform getResource(byte x, byte y, ushort index)
		{
			ResourceSpawnpoint resourceSpawnpoint = ResourceManager.getResourceSpawnpoint(x, y, index);
			if (resourceSpawnpoint == null)
			{
				return null;
			}
			if (resourceSpawnpoint.model != null)
			{
				return resourceSpawnpoint.model;
			}
			return resourceSpawnpoint.stump;
		}

		public static bool tryGetRegion(Transform resource, out byte x, out byte y, out ushort index)
		{
			x = 0;
			y = 0;
			index = 0;
			if (Regions.tryGetCoordinate(resource.position, out x, out y))
			{
				List<ResourceSpawnpoint> list = LevelGround.trees[(int)x, (int)y];
				index = 0;
				while ((int)index < list.Count)
				{
					if (resource == list[(int)index].model || resource == list[(int)index].stump)
					{
						return true;
					}
					index += 1;
				}
			}
			return false;
		}

		private bool respawnResources()
		{
			if (LevelGround.trees[(int)ResourceManager.respawnResources_X, (int)ResourceManager.respawnResources_Y].Count > 0)
			{
				if ((int)ResourceManager.regions[(int)ResourceManager.respawnResources_X, (int)ResourceManager.respawnResources_Y].respawnResourceIndex >= LevelGround.trees[(int)ResourceManager.respawnResources_X, (int)ResourceManager.respawnResources_Y].Count)
				{
					ResourceManager.regions[(int)ResourceManager.respawnResources_X, (int)ResourceManager.respawnResources_Y].respawnResourceIndex = (ushort)(LevelGround.trees[(int)ResourceManager.respawnResources_X, (int)ResourceManager.respawnResources_Y].Count - 1);
				}
				ResourceSpawnpoint resourceSpawnpoint = LevelGround.trees[(int)ResourceManager.respawnResources_X, (int)ResourceManager.respawnResources_Y][(int)ResourceManager.regions[(int)ResourceManager.respawnResources_X, (int)ResourceManager.respawnResources_Y].respawnResourceIndex];
				if (resourceSpawnpoint.checkCanReset(Provider.modeConfigData.Objects.Resource_Reset_Multiplier))
				{
					ResourceManager.manager.channel.send("tellResourceAlive", ESteamCall.ALL, ResourceManager.respawnResources_X, ResourceManager.respawnResources_Y, ResourceManager.RESOURCE_REGIONS, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
					{
						ResourceManager.respawnResources_X,
						ResourceManager.respawnResources_Y,
						ResourceManager.regions[(int)ResourceManager.respawnResources_X, (int)ResourceManager.respawnResources_Y].respawnResourceIndex
					});
				}
				return false;
			}
			return true;
		}

		private void onLevelLoaded(int level)
		{
			if (level > Level.SETUP)
			{
				ResourceManager.regions = new ResourceRegion[(int)Regions.WORLD_SIZE, (int)Regions.WORLD_SIZE];
				for (byte b = 0; b < Regions.WORLD_SIZE; b += 1)
				{
					for (byte b2 = 0; b2 < Regions.WORLD_SIZE; b2 += 1)
					{
						ResourceManager.regions[(int)b, (int)b2] = new ResourceRegion();
					}
				}
				ResourceManager.respawnResources_X = 0;
				ResourceManager.respawnResources_Y = 0;
			}
		}

		private void onRegionUpdated(Player player, byte old_x, byte old_y, byte new_x, byte new_y, byte step, ref bool canIncrementIndex)
		{
			if (step == 0)
			{
				for (byte b = 0; b < Regions.WORLD_SIZE; b += 1)
				{
					for (byte b2 = 0; b2 < Regions.WORLD_SIZE; b2 += 1)
					{
						if (Provider.isServer)
						{
							if (player.movement.loadedRegions[(int)b, (int)b2].isResourcesLoaded && !Regions.checkArea(b, b2, new_x, new_y, ResourceManager.RESOURCE_REGIONS))
							{
								player.movement.loadedRegions[(int)b, (int)b2].isResourcesLoaded = false;
							}
						}
						else if (player.channel.isOwner && ResourceManager.regions[(int)b, (int)b2].isNetworked && !Regions.checkArea(b, b2, new_x, new_y, ResourceManager.RESOURCE_REGIONS))
						{
							ResourceManager.regions[(int)b, (int)b2].isNetworked = false;
						}
					}
				}
			}
			if (step == 3 && Dedicator.isDedicated && Regions.checkSafe((int)new_x, (int)new_y))
			{
				for (int i = (int)(new_x - ResourceManager.RESOURCE_REGIONS); i <= (int)(new_x + ResourceManager.RESOURCE_REGIONS); i++)
				{
					for (int j = (int)(new_y - ResourceManager.RESOURCE_REGIONS); j <= (int)(new_y + ResourceManager.RESOURCE_REGIONS); j++)
					{
						if (Regions.checkSafe((int)((byte)i), (int)((byte)j)) && !player.movement.loadedRegions[i, j].isResourcesLoaded)
						{
							player.movement.loadedRegions[i, j].isResourcesLoaded = true;
							this.askResources(player.channel.owner.playerID.steamID, (byte)i, (byte)j);
						}
					}
				}
			}
		}

		private void onPlayerCreated(Player player)
		{
			PlayerMovement movement = player.movement;
			movement.onRegionUpdated = (PlayerRegionUpdated)Delegate.Combine(movement.onRegionUpdated, new PlayerRegionUpdated(this.onRegionUpdated));
		}

		private void Update()
		{
			if (!Provider.isServer || !Level.isLoaded)
			{
				return;
			}
			bool flag = true;
			while (flag)
			{
				flag = this.respawnResources();
				ResourceRegion resourceRegion = ResourceManager.regions[(int)ResourceManager.respawnResources_X, (int)ResourceManager.respawnResources_Y];
				resourceRegion.respawnResourceIndex += 1;
				if ((int)ResourceManager.regions[(int)ResourceManager.respawnResources_X, (int)ResourceManager.respawnResources_Y].respawnResourceIndex >= LevelGround.trees[(int)ResourceManager.respawnResources_X, (int)ResourceManager.respawnResources_Y].Count)
				{
					ResourceManager.regions[(int)ResourceManager.respawnResources_X, (int)ResourceManager.respawnResources_Y].respawnResourceIndex = 0;
				}
				ResourceManager.respawnResources_X += 1;
				if (ResourceManager.respawnResources_X >= Regions.WORLD_SIZE)
				{
					ResourceManager.respawnResources_X = 0;
					ResourceManager.respawnResources_Y += 1;
					if (ResourceManager.respawnResources_Y >= Regions.WORLD_SIZE)
					{
						ResourceManager.respawnResources_Y = 0;
						flag = false;
					}
				}
			}
		}

		private void Start()
		{
			ResourceManager.manager = this;
			Level.onLevelLoaded = (LevelLoaded)Delegate.Combine(Level.onLevelLoaded, new LevelLoaded(this.onLevelLoaded));
			Player.onPlayerCreated = (PlayerCreated)Delegate.Combine(Player.onPlayerCreated, new PlayerCreated(this.onPlayerCreated));
		}

		public static readonly byte RESOURCE_REGIONS = 2;

		private static ResourceManager manager;

		private static ResourceRegion[,] regions;

		private static byte respawnResources_X;

		private static byte respawnResources_Y;
	}
}
