using System;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	public class SteamGroup
	{
		public SteamGroup(CSteamID newSteamID, string newName, Texture2D newIcon)
		{
			this._steamID = newSteamID;
			this._name = newName;
			this._icon = newIcon;
		}

		public CSteamID steamID
		{
			get
			{
				return this._steamID;
			}
		}

		public string name
		{
			get
			{
				return this._name;
			}
		}

		public Texture2D icon
		{
			get
			{
				return this._icon;
			}
		}

		private CSteamID _steamID;

		private string _name;

		private Texture2D _icon;
	}
}
