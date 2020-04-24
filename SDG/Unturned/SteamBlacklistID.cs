using System;
using Steamworks;

namespace SDG.Unturned
{
	public class SteamBlacklistID
	{
		public SteamBlacklistID(CSteamID newPlayerID, uint newIP, CSteamID newJudgeID, string newReason, uint newDuration, uint newBanned)
		{
			this._playerID = newPlayerID;
			this._ip = newIP;
			this.judgeID = newJudgeID;
			this.reason = newReason;
			this.duration = newDuration;
			this.banned = newBanned;
		}

		public CSteamID playerID
		{
			get
			{
				return this._playerID;
			}
		}

		public uint ip
		{
			get
			{
				return this._ip;
			}
		}

		public bool isExpired
		{
			get
			{
				return Provider.time > this.banned + this.duration;
			}
		}

		public uint getTime()
		{
			return this.duration - (Provider.time - this.banned);
		}

		private CSteamID _playerID;

		private uint _ip;

		public CSteamID judgeID;

		public string reason;

		public uint duration;

		public uint banned;
	}
}
