using System;
using UnityEngine;

namespace SDG.Unturned
{
	public delegate void Chatted(SteamPlayer player, EChatMode mode, ref Color chatted, string text, ref bool isVisible);
}
