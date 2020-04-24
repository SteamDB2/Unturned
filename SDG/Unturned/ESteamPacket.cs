﻿using System;

namespace SDG.Unturned
{
	public enum ESteamPacket
	{
		SHUTDOWN,
		WORKSHOP,
		CONNECT,
		VERIFY,
		AUTHENTICATE,
		REJECTED,
		ACCEPTED,
		ADMINED,
		UNADMINED,
		BANNED,
		KICKED,
		CONNECTED,
		DISCONNECTED,
		TICK,
		TIME,
		UPDATE_RELIABLE_BUFFER,
		UPDATE_UNRELIABLE_BUFFER,
		UPDATE_RELIABLE_INSTANT,
		UPDATE_UNRELIABLE_INSTANT,
		UPDATE_RELIABLE_CHUNK_BUFFER,
		UPDATE_UNRELIABLE_CHUNK_BUFFER,
		UPDATE_RELIABLE_CHUNK_INSTANT,
		UPDATE_UNRELIABLE_CHUNK_INSTANT,
		UPDATE_VOICE,
		BATTLEYE,
		GUIDTABLE
	}
}
