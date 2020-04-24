using System;
using Steamworks;

namespace SDG.Unturned
{
	public delegate void TriggerReceive(SteamChannel channel, CSteamID steamID, byte[] packet, int offset, int size);
}
