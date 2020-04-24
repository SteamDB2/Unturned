using System;

namespace SDG.Unturned
{
	public delegate void TriggerSend(SteamPlayer player, string name, ESteamCall mode, ESteamPacket type, params object[] arguments);
}
