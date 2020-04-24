using System;
using Steamworks;

namespace SDG.Unturned
{
	public class SteamWhitelistID
	{
		public SteamWhitelistID(CSteamID newSteamID, string newTag, CSteamID newJudgeID)
		{
			this._steamID = newSteamID;
			this.tag = newTag;
			this.judgeID = newJudgeID;
		}

		public CSteamID steamID
		{
			get
			{
				return this._steamID;
			}
		}

		private CSteamID _steamID;

		public string tag;

		public CSteamID judgeID;
	}
}
