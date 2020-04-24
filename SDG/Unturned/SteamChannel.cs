using System;
using System.Collections.Generic;
using System.Reflection;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	public class SteamChannel : MonoBehaviour
	{
		public SteamChannelMethod[] calls
		{
			get
			{
				return this._calls;
			}
		}

		public bool checkServer(CSteamID steamID)
		{
			return steamID == Provider.server;
		}

		public bool checkOwner(CSteamID steamID)
		{
			return this.owner != null && steamID == this.owner.playerID.steamID;
		}

		public void receive(CSteamID steamID, byte[] packet, int offset, int size)
		{
			if (SteamChannel.onTriggerReceive != null)
			{
				SteamChannel.onTriggerReceive(this, steamID, packet, offset, size);
			}
			if (size < 2)
			{
				return;
			}
			int num = (int)packet[offset + 1];
			if (num < 0 || num >= this.calls.Length)
			{
				return;
			}
			ESteamPacket esteamPacket = (ESteamPacket)packet[offset];
			if (esteamPacket == ESteamPacket.UPDATE_VOICE && size < 5)
			{
				return;
			}
			if (esteamPacket == ESteamPacket.UPDATE_UNRELIABLE_CHUNK_BUFFER || esteamPacket == ESteamPacket.UPDATE_RELIABLE_CHUNK_BUFFER)
			{
				SteamPacker.openRead(offset + 2, packet);
				this.calls[num].method.Invoke(this.calls[num].component, new object[]
				{
					steamID
				});
				SteamPacker.closeRead();
			}
			else if (this.calls[num].types.Length > 0)
			{
				if (esteamPacket == ESteamPacket.UPDATE_VOICE)
				{
					SteamChannel.voice[0] = steamID;
					SteamChannel.voice[1] = packet;
					SteamChannel.voice[2] = (int)BitConverter.ToUInt16(packet, offset + 2);
					this.calls[num].method.Invoke(this.calls[num].component, SteamChannel.voice);
				}
				else
				{
					object[] objects = SteamPacker.getObjects(steamID, offset, 2, packet, this.calls[num].types);
					if (objects != null)
					{
						this.calls[num].method.Invoke(this.calls[num].component, objects);
					}
				}
			}
			else
			{
				this.calls[num].method.Invoke(this.calls[num].component, null);
			}
		}

		public object read(Type type)
		{
			return SteamPacker.read(type);
		}

		public object[] read(Type type_0, Type type_1)
		{
			return SteamPacker.read(type_0, type_1);
		}

		public object[] read(Type type_0, Type type_1, Type type_2)
		{
			return SteamPacker.read(type_0, type_1, type_2);
		}

		public object[] read(Type type_0, Type type_1, Type type_2, Type type_3)
		{
			return SteamPacker.read(type_0, type_1, type_2, type_3);
		}

		public object[] read(Type type_0, Type type_1, Type type_2, Type type_3, Type type_4)
		{
			return SteamPacker.read(type_0, type_1, type_2, type_3, type_4);
		}

		public object[] read(Type type_0, Type type_1, Type type_2, Type type_3, Type type_4, Type type_5)
		{
			return SteamPacker.read(type_0, type_1, type_2, type_3, type_4, type_5);
		}

		public object[] read(Type type_0, Type type_1, Type type_2, Type type_3, Type type_4, Type type_5, Type type_6)
		{
			return SteamPacker.read(type_0, type_1, type_2, type_3, type_4, type_5, type_6);
		}

		public object[] read(params Type[] types)
		{
			return SteamPacker.read(types);
		}

		public void write(object objects)
		{
			SteamPacker.write(objects);
		}

		public void write(object object_0, object object_1)
		{
			SteamPacker.write(object_0, object_1);
		}

		public void write(object object_0, object object_1, object object_2)
		{
			SteamPacker.write(object_0, object_1, object_2);
		}

		public void write(object object_0, object object_1, object object_2, object object_3)
		{
			SteamPacker.write(object_0, object_1, object_2, object_3);
		}

		public void write(object object_0, object object_1, object object_2, object object_3, object object_4)
		{
			SteamPacker.write(object_0, object_1, object_2, object_3, object_4);
		}

		public void write(object object_0, object object_1, object object_2, object object_3, object object_4, object object_5)
		{
			SteamPacker.write(object_0, object_1, object_2, object_3, object_4, object_5);
		}

		public void write(object object_0, object object_1, object object_2, object object_3, object object_4, object object_5, object object_6)
		{
			SteamPacker.write(object_0, object_1, object_2, object_3, object_4, object_5, object_6);
		}

		public void write(params object[] objects)
		{
			SteamPacker.write(objects);
		}

		public int step
		{
			get
			{
				return SteamPacker.step;
			}
			set
			{
				SteamPacker.step = value;
			}
		}

		public bool useCompression
		{
			get
			{
				return SteamPacker.useCompression;
			}
			set
			{
				SteamPacker.useCompression = value;
			}
		}

		public bool longBinaryData
		{
			get
			{
				return SteamPacker.longBinaryData;
			}
			set
			{
				SteamPacker.longBinaryData = value;
			}
		}

		public void openWrite()
		{
			SteamPacker.openWrite(2);
		}

		public void closeWrite(string name, CSteamID steamID, ESteamPacket type)
		{
			if (!Provider.isChunk(type))
			{
				Debug.LogError("Failed to stream non chunk.");
				return;
			}
			int call = this.getCall(name);
			if (call == -1)
			{
				return;
			}
			int size;
			byte[] packet;
			this.getPacket(type, call, out size, out packet);
			if (this.isOwner && steamID == Provider.client)
			{
				this.receive(Provider.client, packet, 0, size);
			}
			else if (Provider.isServer && steamID == Provider.server)
			{
				this.receive(Provider.server, packet, 0, size);
			}
			else
			{
				Provider.send(steamID, type, packet, size, this.id);
			}
		}

		public void closeWrite(string name, ESteamCall mode, byte bound, ESteamPacket type)
		{
			if (!Provider.isChunk(type))
			{
				Debug.LogError("Failed to stream non chunk.");
				return;
			}
			int call = this.getCall(name);
			if (call == -1)
			{
				return;
			}
			int size;
			byte[] packet;
			this.getPacket(type, call, out size, out packet);
			this.send(mode, bound, type, size, packet);
		}

		public void closeWrite(string name, ESteamCall mode, byte x, byte y, byte area, ESteamPacket type)
		{
			if (!Provider.isChunk(type))
			{
				Debug.LogError("Failed to stream non chunk.");
				return;
			}
			int call = this.getCall(name);
			if (call == -1)
			{
				return;
			}
			int size;
			byte[] packet;
			this.getPacket(type, call, out size, out packet);
			this.send(mode, x, y, area, type, size, packet);
		}

		public void closeWrite(string name, ESteamCall mode, ESteamPacket type)
		{
			if (!Provider.isChunk(type))
			{
				Debug.LogError("Failed to stream non chunk.");
				return;
			}
			int call = this.getCall(name);
			if (call == -1)
			{
				return;
			}
			int size;
			byte[] packet;
			this.getPacket(type, call, out size, out packet);
			this.send(mode, type, size, packet);
		}

		public void send(string name, CSteamID steamID, ESteamPacket type, params object[] arguments)
		{
			int call = this.getCall(name);
			if (call == -1)
			{
				return;
			}
			int size;
			byte[] packet;
			this.getPacket(type, call, out size, out packet, arguments);
			if (this.isOwner && steamID == Provider.client)
			{
				this.receive(Provider.client, packet, 0, size);
			}
			else if (Provider.isServer && steamID == Provider.server)
			{
				this.receive(Provider.server, packet, 0, size);
			}
			else
			{
				Provider.send(steamID, type, packet, size, this.id);
			}
		}

		public void sendAside(string name, CSteamID steamID, ESteamPacket type, params object[] arguments)
		{
			int call = this.getCall(name);
			if (call == -1)
			{
				return;
			}
			int size;
			byte[] packet;
			this.getPacket(type, call, out size, out packet, arguments);
			for (int i = 0; i < Provider.clients.Count; i++)
			{
				if (Provider.clients[i].playerID.steamID != steamID)
				{
					Provider.send(Provider.clients[i].playerID.steamID, type, packet, size, this.id);
				}
			}
		}

		public void send(ESteamCall mode, byte bound, ESteamPacket type, int size, byte[] packet)
		{
			if (mode == ESteamCall.SERVER)
			{
				if (Provider.isServer)
				{
					this.receive(Provider.server, packet, 0, size);
				}
				else
				{
					Provider.send(Provider.server, type, packet, size, this.id);
				}
			}
			else if (mode == ESteamCall.ALL)
			{
				if (!Provider.isServer)
				{
					Provider.send(Provider.server, type, packet, size, this.id);
				}
				for (int i = 0; i < Provider.clients.Count; i++)
				{
					if (Provider.clients[i].playerID.steamID != Provider.client && Provider.clients[i].player != null && Provider.clients[i].player.movement.bound == bound)
					{
						Provider.send(Provider.clients[i].playerID.steamID, type, packet, size, this.id);
					}
				}
				if (Provider.isServer)
				{
					this.receive(Provider.server, packet, 0, size);
				}
				else
				{
					this.receive(Provider.client, packet, 0, size);
				}
			}
			else if (mode == ESteamCall.OTHERS)
			{
				if (!Provider.isServer)
				{
					Provider.send(Provider.server, type, packet, size, this.id);
				}
				for (int j = 0; j < Provider.clients.Count; j++)
				{
					if (Provider.clients[j].playerID.steamID != Provider.client && Provider.clients[j].player != null && Provider.clients[j].player.movement.bound == bound)
					{
						Provider.send(Provider.clients[j].playerID.steamID, type, packet, size, this.id);
					}
				}
			}
			else if (mode == ESteamCall.OWNER)
			{
				if (this.isOwner)
				{
					this.receive(this.owner.playerID.steamID, packet, 0, size);
				}
				else
				{
					Provider.send(this.owner.playerID.steamID, type, packet, size, this.id);
				}
			}
			else if (mode == ESteamCall.NOT_OWNER)
			{
				if (!Provider.isServer)
				{
					Provider.send(Provider.server, type, packet, size, this.id);
				}
				for (int k = 0; k < Provider.clients.Count; k++)
				{
					if (Provider.clients[k].playerID.steamID != this.owner.playerID.steamID && Provider.clients[k].player != null && Provider.clients[k].player.movement.bound == bound)
					{
						Provider.send(Provider.clients[k].playerID.steamID, type, packet, size, this.id);
					}
				}
			}
			else if (mode == ESteamCall.CLIENTS)
			{
				for (int l = 0; l < Provider.clients.Count; l++)
				{
					if (Provider.clients[l].playerID.steamID != Provider.client && Provider.clients[l].player != null && Provider.clients[l].player.movement.bound == bound)
					{
						Provider.send(Provider.clients[l].playerID.steamID, type, packet, size, this.id);
					}
				}
				if (Provider.isClient)
				{
					this.receive(Provider.client, packet, 0, size);
				}
			}
			else if (mode == ESteamCall.PEERS)
			{
				for (int m = 0; m < Provider.clients.Count; m++)
				{
					if (Provider.clients[m].playerID.steamID != Provider.client && Provider.clients[m].player != null && Provider.clients[m].player.movement.bound == bound)
					{
						Provider.send(Provider.clients[m].playerID.steamID, type, packet, size, this.id);
					}
				}
			}
		}

		public void send(string name, ESteamCall mode, byte bound, ESteamPacket type, byte[] bytes, int length)
		{
			int call = this.getCall(name);
			if (call == -1)
			{
				return;
			}
			int size;
			byte[] packet;
			this.getPacket(type, call, out size, out packet, bytes, length);
			this.send(mode, bound, type, size, packet);
		}

		public void send(string name, ESteamCall mode, byte bound, ESteamPacket type, params object[] arguments)
		{
			int call = this.getCall(name);
			if (call == -1)
			{
				return;
			}
			int size;
			byte[] packet;
			this.getPacket(type, call, out size, out packet, arguments);
			this.send(mode, bound, type, size, packet);
		}

		public void send(ESteamCall mode, byte x, byte y, byte area, ESteamPacket type, int size, byte[] packet)
		{
			if (mode == ESteamCall.SERVER)
			{
				if (Provider.isServer)
				{
					this.receive(Provider.server, packet, 0, size);
				}
				else
				{
					Provider.send(Provider.server, type, packet, size, this.id);
				}
			}
			else if (mode == ESteamCall.ALL)
			{
				if (!Provider.isServer)
				{
					Provider.send(Provider.server, type, packet, size, this.id);
				}
				for (int i = 0; i < Provider.clients.Count; i++)
				{
					if (Provider.clients[i].playerID.steamID != Provider.client && Provider.clients[i].player != null && Regions.checkArea(x, y, Provider.clients[i].player.movement.region_x, Provider.clients[i].player.movement.region_y, area))
					{
						Provider.send(Provider.clients[i].playerID.steamID, type, packet, size, this.id);
					}
				}
				if (Provider.isServer)
				{
					this.receive(Provider.server, packet, 0, size);
				}
				else
				{
					this.receive(Provider.client, packet, 0, size);
				}
			}
			else if (mode == ESteamCall.OTHERS)
			{
				if (!Provider.isServer)
				{
					Provider.send(Provider.server, type, packet, size, this.id);
				}
				for (int j = 0; j < Provider.clients.Count; j++)
				{
					if (Provider.clients[j].playerID.steamID != Provider.client && Provider.clients[j].player != null && Regions.checkArea(x, y, Provider.clients[j].player.movement.region_x, Provider.clients[j].player.movement.region_y, area))
					{
						Provider.send(Provider.clients[j].playerID.steamID, type, packet, size, this.id);
					}
				}
			}
			else if (mode == ESteamCall.OWNER)
			{
				if (this.isOwner)
				{
					this.receive(this.owner.playerID.steamID, packet, 0, size);
				}
				else
				{
					Provider.send(this.owner.playerID.steamID, type, packet, size, this.id);
				}
			}
			else if (mode == ESteamCall.NOT_OWNER)
			{
				if (!Provider.isServer)
				{
					Provider.send(Provider.server, type, packet, size, this.id);
				}
				for (int k = 0; k < Provider.clients.Count; k++)
				{
					if (Provider.clients[k].playerID.steamID != this.owner.playerID.steamID && Provider.clients[k].player != null && Regions.checkArea(x, y, Provider.clients[k].player.movement.region_x, Provider.clients[k].player.movement.region_y, area))
					{
						Provider.send(Provider.clients[k].playerID.steamID, type, packet, size, this.id);
					}
				}
			}
			else if (mode == ESteamCall.CLIENTS)
			{
				for (int l = 0; l < Provider.clients.Count; l++)
				{
					if (Provider.clients[l].playerID.steamID != Provider.client && Provider.clients[l].player != null && Regions.checkArea(x, y, Provider.clients[l].player.movement.region_x, Provider.clients[l].player.movement.region_y, area))
					{
						Provider.send(Provider.clients[l].playerID.steamID, type, packet, size, this.id);
					}
				}
				if (Provider.isClient)
				{
					this.receive(Provider.client, packet, 0, size);
				}
			}
			else if (mode == ESteamCall.PEERS)
			{
				for (int m = 0; m < Provider.clients.Count; m++)
				{
					if (Provider.clients[m].playerID.steamID != Provider.client && Provider.clients[m].player != null && Regions.checkArea(x, y, Provider.clients[m].player.movement.region_x, Provider.clients[m].player.movement.region_y, area))
					{
						Provider.send(Provider.clients[m].playerID.steamID, type, packet, size, this.id);
					}
				}
			}
		}

		public void send(string name, ESteamCall mode, byte x, byte y, byte area, ESteamPacket type, byte[] bytes, int length)
		{
			int call = this.getCall(name);
			if (call == -1)
			{
				return;
			}
			int size;
			byte[] packet;
			this.getPacket(type, call, out size, out packet, bytes, length);
			this.send(mode, x, y, area, type, size, packet);
		}

		public void send(string name, ESteamCall mode, byte x, byte y, byte area, ESteamPacket type, params object[] arguments)
		{
			int call = this.getCall(name);
			if (call == -1)
			{
				return;
			}
			int size;
			byte[] packet;
			this.getPacket(type, call, out size, out packet, arguments);
			this.send(mode, x, y, area, type, size, packet);
		}

		public void send(ESteamCall mode, ESteamPacket type, int size, byte[] packet)
		{
			if (mode == ESteamCall.SERVER)
			{
				if (Provider.isServer)
				{
					this.receive(Provider.server, packet, 0, size);
				}
				else
				{
					Provider.send(Provider.server, type, packet, size, this.id);
				}
			}
			else if (mode == ESteamCall.ALL)
			{
				if (!Provider.isServer)
				{
					Provider.send(Provider.server, type, packet, size, this.id);
				}
				for (int i = 0; i < Provider.clients.Count; i++)
				{
					if (Provider.clients[i].playerID.steamID != Provider.client)
					{
						Provider.send(Provider.clients[i].playerID.steamID, type, packet, size, this.id);
					}
				}
				if (Provider.isServer)
				{
					this.receive(Provider.server, packet, 0, size);
				}
				else
				{
					this.receive(Provider.client, packet, 0, size);
				}
			}
			else if (mode == ESteamCall.OTHERS)
			{
				if (!Provider.isServer)
				{
					Provider.send(Provider.server, type, packet, size, this.id);
				}
				for (int j = 0; j < Provider.clients.Count; j++)
				{
					if (Provider.clients[j].playerID.steamID != Provider.client)
					{
						Provider.send(Provider.clients[j].playerID.steamID, type, packet, size, this.id);
					}
				}
			}
			else if (mode == ESteamCall.OWNER)
			{
				if (this.isOwner)
				{
					this.receive(this.owner.playerID.steamID, packet, 0, size);
				}
				else
				{
					Provider.send(this.owner.playerID.steamID, type, packet, size, this.id);
				}
			}
			else if (mode == ESteamCall.NOT_OWNER)
			{
				if (!Provider.isServer)
				{
					Provider.send(Provider.server, type, packet, size, this.id);
				}
				for (int k = 0; k < Provider.clients.Count; k++)
				{
					if (Provider.clients[k].playerID.steamID != this.owner.playerID.steamID)
					{
						Provider.send(Provider.clients[k].playerID.steamID, type, packet, size, this.id);
					}
				}
			}
			else if (mode == ESteamCall.CLIENTS)
			{
				for (int l = 0; l < Provider.clients.Count; l++)
				{
					if (Provider.clients[l].playerID.steamID != Provider.client)
					{
						Provider.send(Provider.clients[l].playerID.steamID, type, packet, size, this.id);
					}
				}
				if (Provider.isClient)
				{
					this.receive(Provider.client, packet, 0, size);
				}
			}
			else if (mode == ESteamCall.PEERS)
			{
				for (int m = 0; m < Provider.clients.Count; m++)
				{
					if (Provider.clients[m].playerID.steamID != Provider.client)
					{
						Provider.send(Provider.clients[m].playerID.steamID, type, packet, size, this.id);
					}
				}
			}
		}

		public void send(string name, ESteamCall mode, ESteamPacket type, params object[] arguments)
		{
			if (SteamChannel.onTriggerSend != null)
			{
				SteamChannel.onTriggerSend(this.owner, name, mode, type, arguments);
			}
			int call = this.getCall(name);
			if (call == -1)
			{
				return;
			}
			int size;
			byte[] packet;
			this.getPacket(type, call, out size, out packet, arguments);
			this.send(mode, type, size, packet);
		}

		public void send(string name, ESteamCall mode, ESteamPacket type, byte[] bytes, int length)
		{
			int call = this.getCall(name);
			if (call == -1)
			{
				return;
			}
			int size;
			byte[] packet;
			this.getPacket(type, call, out size, out packet, bytes, length);
			this.send(mode, type, size, packet);
		}

		public void send(ESteamCall mode, Vector3 point, float radius, ESteamPacket type, int size, byte[] packet)
		{
			radius *= radius;
			if (mode == ESteamCall.SERVER)
			{
				if (Provider.isServer)
				{
					this.receive(Provider.server, packet, 0, size);
				}
				else
				{
					Provider.send(Provider.server, type, packet, size, this.id);
				}
			}
			else if (mode == ESteamCall.ALL)
			{
				if (!Provider.isServer)
				{
					Provider.send(Provider.server, type, packet, size, this.id);
				}
				for (int i = 0; i < Provider.clients.Count; i++)
				{
					if (Provider.clients[i].playerID.steamID != Provider.client && Provider.clients[i].player != null && (Provider.clients[i].player.transform.position - point).sqrMagnitude < radius)
					{
						Provider.send(Provider.clients[i].playerID.steamID, type, packet, size, this.id);
					}
				}
				if (Provider.isServer)
				{
					this.receive(Provider.server, packet, 0, size);
				}
				else
				{
					this.receive(Provider.client, packet, 0, size);
				}
			}
			else if (mode == ESteamCall.OTHERS)
			{
				if (!Provider.isServer)
				{
					Provider.send(Provider.server, type, packet, size, this.id);
				}
				for (int j = 0; j < Provider.clients.Count; j++)
				{
					if (Provider.clients[j].playerID.steamID != Provider.client && Provider.clients[j].player != null && (Provider.clients[j].player.transform.position - point).sqrMagnitude < radius)
					{
						Provider.send(Provider.clients[j].playerID.steamID, type, packet, size, this.id);
					}
				}
			}
			else if (mode == ESteamCall.OWNER)
			{
				if (this.isOwner)
				{
					this.receive(this.owner.playerID.steamID, packet, 0, size);
				}
				else
				{
					Provider.send(this.owner.playerID.steamID, type, packet, size, this.id);
				}
			}
			else if (mode == ESteamCall.NOT_OWNER)
			{
				if (!Provider.isServer)
				{
					Provider.send(Provider.server, type, packet, size, this.id);
				}
				for (int k = 0; k < Provider.clients.Count; k++)
				{
					if (Provider.clients[k].playerID.steamID != this.owner.playerID.steamID && Provider.clients[k].player != null && (Provider.clients[k].player.transform.position - point).sqrMagnitude < radius)
					{
						Provider.send(Provider.clients[k].playerID.steamID, type, packet, size, this.id);
					}
				}
			}
			else if (mode == ESteamCall.CLIENTS)
			{
				for (int l = 0; l < Provider.clients.Count; l++)
				{
					if (Provider.clients[l].playerID.steamID != Provider.client && Provider.clients[l].player != null && (Provider.clients[l].player.transform.position - point).sqrMagnitude < radius)
					{
						Provider.send(Provider.clients[l].playerID.steamID, type, packet, size, this.id);
					}
				}
				if (Provider.isClient)
				{
					this.receive(Provider.client, packet, 0, size);
				}
			}
			else if (mode == ESteamCall.PEERS)
			{
				for (int m = 0; m < Provider.clients.Count; m++)
				{
					if (Provider.clients[m].playerID.steamID != Provider.client && Provider.clients[m].player != null && (Provider.clients[m].player.transform.position - point).sqrMagnitude < radius)
					{
						Provider.send(Provider.clients[m].playerID.steamID, type, packet, size, this.id);
					}
				}
			}
		}

		public void sendVoice(string name, ESteamCall mode, Vector3 point, float radius, ESteamPacket type, byte[] bytes, int length)
		{
			int call = this.getCall(name);
			if (call == -1)
			{
				return;
			}
			int size;
			byte[] packet;
			this.getPacketVoice(type, call, out size, out packet, bytes, length);
			this.send(mode, point, radius, type, size, packet);
		}

		public void send(string name, ESteamCall mode, Vector3 point, float radius, ESteamPacket type, params object[] arguments)
		{
			int call = this.getCall(name);
			if (call == -1)
			{
				return;
			}
			int size;
			byte[] packet;
			this.getPacket(type, call, out size, out packet, arguments);
			this.send(mode, point, radius, type, size, packet);
		}

		public void build()
		{
			List<SteamChannelMethod> list = new List<SteamChannelMethod>();
			Component[] components = base.GetComponents(typeof(Component));
			for (int i = 0; i < components.Length; i++)
			{
				MemberInfo[] members = components[i].GetType().GetMembers();
				for (int j = 0; j < members.Length; j++)
				{
					if (members[j].MemberType == MemberTypes.Method)
					{
						MethodInfo methodInfo = (MethodInfo)members[j];
						if (methodInfo.GetCustomAttributes(typeof(SteamCall), true).Length > 0)
						{
							ParameterInfo[] parameters = methodInfo.GetParameters();
							Type[] array = new Type[parameters.Length];
							for (int k = 0; k < parameters.Length; k++)
							{
								array[k] = parameters[k].ParameterType;
							}
							list.Add(new SteamChannelMethod(components[i], methodInfo, array));
						}
					}
				}
			}
			this._calls = list.ToArray();
			if (this.calls.Length > 235)
			{
				CommandWindow.LogError(base.name + " approaching 255 methods!");
			}
		}

		public void setup()
		{
			Provider.openChannel(this);
		}

		public void getPacket(ESteamPacket type, int index, out int size, out byte[] packet)
		{
			packet = SteamPacker.closeWrite(out size);
			packet[0] = (byte)type;
			packet[1] = (byte)index;
		}

		public void getPacket(ESteamPacket type, int index, out int size, out byte[] packet, byte[] bytes, int length)
		{
			size = 4 + length;
			packet = bytes;
			packet[0] = (byte)type;
			packet[1] = (byte)index;
			byte[] bytes2 = BitConverter.GetBytes((ushort)length);
			packet[2] = bytes2[0];
			packet[3] = bytes2[1];
		}

		public void getPacketVoice(ESteamPacket type, int index, out int size, out byte[] packet, byte[] bytes, int length)
		{
			size = 5 + length;
			packet = bytes;
			packet[0] = (byte)type;
			packet[1] = (byte)index;
			byte[] bytes2 = BitConverter.GetBytes((ushort)length);
			packet[2] = bytes2[0];
			packet[3] = bytes2[1];
		}

		public void getPacket(ESteamPacket type, int index, out int size, out byte[] packet, params object[] arguments)
		{
			packet = SteamPacker.getBytes(2, out size, arguments);
			packet[0] = (byte)type;
			packet[1] = (byte)index;
		}

		public int getCall(string name)
		{
			for (int i = 0; i < this.calls.Length; i++)
			{
				if (this.calls[i].method.Name == name)
				{
					return i;
				}
			}
			CommandWindow.LogError("Failed to find a method named: " + name);
			return -1;
		}

		private void Awake()
		{
			this.build();
		}

		private void OnDestroy()
		{
			if (this.id != 0)
			{
				Provider.closeChannel(this);
			}
		}

		private static object[] voice = new object[3];

		private SteamChannelMethod[] _calls;

		public int id;

		public SteamPlayer owner;

		public bool isOwner;

		public static TriggerReceive onTriggerReceive;

		public static TriggerSend onTriggerSend;
	}
}
