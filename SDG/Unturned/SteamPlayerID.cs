using System;
using Steamworks;

namespace SDG.Unturned
{
	public class SteamPlayerID
	{
		public SteamPlayerID(CSteamID newSteamID, byte newCharacterID, string newPlayerName, string newCharacterName, string newNickName, CSteamID newGroup)
		{
			this._steamID = newSteamID;
			this.characterID = newCharacterID;
			this._playerName = newPlayerName;
			this._characterName = newCharacterName;
			this.nickName = newNickName;
			this.group = newGroup;
		}

		public CSteamID steamID
		{
			get
			{
				return this._steamID;
			}
		}

		private string streamerName
		{
			get
			{
				if (Provider.streamerNames != null && Provider.streamerNames.Count > 0)
				{
					return Provider.streamerNames[(int)(this.steamID.m_SteamID % (ulong)((long)Provider.streamerNames.Count))];
				}
				return string.Empty;
			}
		}

		public string playerName
		{
			get
			{
				if (OptionsSettings.streamer && this.steamID != Provider.user)
				{
					return this.streamerName;
				}
				return this._playerName;
			}
		}

		public string characterName
		{
			get
			{
				if (OptionsSettings.streamer && this.steamID != Provider.user)
				{
					return this.streamerName;
				}
				return this._characterName;
			}
			set
			{
				this._characterName = value;
			}
		}

		public override string ToString()
		{
			return string.Concat(new object[]
			{
				this.steamID,
				" ",
				this.characterID,
				" ",
				this.playerName
			});
		}

		public static bool operator ==(SteamPlayerID playerID_0, SteamPlayerID playerID_1)
		{
			return playerID_0.steamID == playerID_1.steamID;
		}

		public static bool operator !=(SteamPlayerID playerID_0, SteamPlayerID playerID_1)
		{
			return !(playerID_0 == playerID_1);
		}

		public static string operator +(SteamPlayerID playerID, string text)
		{
			return playerID.steamID + text;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		private CSteamID _steamID;

		public byte characterID;

		private string _playerName;

		private string _characterName;

		public string nickName;

		public CSteamID group;
	}
}
