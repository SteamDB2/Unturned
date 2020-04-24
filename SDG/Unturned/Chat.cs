using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class Chat
	{
		public Chat(SteamPlayer newPlayer, EChatMode newMode, Color newColor, string newSpeaker, string newText)
		{
			this.player = newPlayer;
			this.mode = newMode;
			this.color = newColor;
			this.speaker = newSpeaker;
			this.text = newText;
		}

		public SteamPlayer player;

		public EChatMode mode;

		public Color color;

		public string speaker;

		public string text;
	}
}
