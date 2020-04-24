using System;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	public class PlayerManager : SteamCaller
	{
		[SteamCall]
		public void tellPlayerStates(CSteamID steamID)
		{
			if (base.channel.checkServer(steamID))
			{
				uint num = (uint)base.channel.read(Types.UINT32_TYPE);
				if (num <= this.seq)
				{
					return;
				}
				this.seq = num;
				base.channel.useCompression = true;
				ushort num2 = (ushort)base.channel.read(Types.UINT16_TYPE);
				for (ushort num3 = 0; num3 < num2; num3 += 1)
				{
					object[] array = base.channel.read(Types.INT32_TYPE, Types.VECTOR3_TYPE, Types.BYTE_TYPE, Types.BYTE_TYPE);
					int num4 = (int)array[0];
					int i = 0;
					while (i < Provider.clients.Count)
					{
						if (Provider.clients[i].channel == num4)
						{
							SteamPlayer steamPlayer = Provider.clients[i];
							if (steamPlayer == null || steamPlayer.player == null || steamPlayer.player.movement == null)
							{
								break;
							}
							steamPlayer.player.movement.tellState((Vector3)array[1], (byte)array[2], (byte)array[3]);
							break;
						}
						else
						{
							i++;
						}
					}
				}
				base.channel.useCompression = false;
			}
		}

		private void onLevelLoaded(int level)
		{
			if (level > Level.SETUP)
			{
				this.seq = 0u;
			}
		}

		private void Update()
		{
			if (!Provider.isServer || !Level.isLoaded)
			{
				return;
			}
			if (Dedicator.isDedicated && Time.realtimeSinceStartup - PlayerManager.lastTick > Provider.UPDATE_TIME)
			{
				PlayerManager.lastTick += Provider.UPDATE_TIME;
				if (Time.realtimeSinceStartup - PlayerManager.lastTick > Provider.UPDATE_TIME)
				{
					PlayerManager.lastTick = Time.realtimeSinceStartup;
				}
				base.channel.useCompression = true;
				this.seq += 1u;
				for (int i = 0; i < Provider.clients.Count; i++)
				{
					SteamPlayer steamPlayer = Provider.clients[i];
					if (steamPlayer != null && !(steamPlayer.player == null))
					{
						base.channel.openWrite();
						base.channel.write(this.seq);
						ushort num = 0;
						int step = base.channel.step;
						base.channel.write(num);
						int num2 = 0;
						for (int j = 0; j < Provider.clients.Count; j++)
						{
							if (num >= 64)
							{
								break;
							}
							SteamPlayer steamPlayer2 = Provider.clients[j];
							if (steamPlayer2 != null && !(steamPlayer2.player == null) && !(steamPlayer2.player.movement == null) && steamPlayer2.player.movement.updates != null && steamPlayer2.player.movement.updates.Count != 0)
							{
								if (j != i)
								{
									if ((steamPlayer2.player.transform.position - steamPlayer.player.transform.position).sqrMagnitude > 331776f)
									{
										if ((ulong)(this.seq % 8u) == (ulong)((long)num2))
										{
											PlayerStateUpdate playerStateUpdate = steamPlayer2.player.movement.updates[steamPlayer2.player.movement.updates.Count - 1];
											base.channel.write(steamPlayer2.channel, playerStateUpdate.pos, playerStateUpdate.angle, playerStateUpdate.rot);
											num += 1;
										}
										num2++;
									}
									else
									{
										for (int k = 0; k < steamPlayer2.player.movement.updates.Count; k++)
										{
											PlayerStateUpdate playerStateUpdate2 = steamPlayer2.player.movement.updates[k];
											base.channel.write(steamPlayer2.channel, playerStateUpdate2.pos, playerStateUpdate2.angle, playerStateUpdate2.rot);
										}
										num += (ushort)steamPlayer2.player.movement.updates.Count;
									}
								}
							}
						}
						if (num != 0)
						{
							int step2 = base.channel.step;
							base.channel.step = step;
							base.channel.write(num);
							base.channel.step = step2;
							base.channel.closeWrite("tellPlayerStates", steamPlayer.playerID.steamID, ESteamPacket.UPDATE_UNRELIABLE_CHUNK_BUFFER);
						}
					}
				}
				base.channel.useCompression = false;
				for (int l = 0; l < Provider.clients.Count; l++)
				{
					SteamPlayer steamPlayer3 = Provider.clients[l];
					if (steamPlayer3 != null && !(steamPlayer3.player == null) && !(steamPlayer3.player.movement == null) && steamPlayer3.player.movement.updates != null && steamPlayer3.player.movement.updates.Count != 0)
					{
						steamPlayer3.player.movement.updates.Clear();
					}
				}
			}
		}

		private void Start()
		{
			Level.onLevelLoaded = (LevelLoaded)Delegate.Combine(Level.onLevelLoaded, new LevelLoaded(this.onLevelLoaded));
		}

		[Obsolete]
		public static ushort updates;

		private static float lastTick;

		private uint seq;
	}
}
