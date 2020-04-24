using System;
using Steamworks;

namespace SDG.Unturned
{
	public class SteamAdminID
	{
		public SteamAdminID(CSteamID newPlayerID, CSteamID newJudgeID)
		{
			this._playerID = newPlayerID;
			this.judgeID = newJudgeID;
		}

		public CSteamID playerID
		{
			get
			{
				return this._playerID;
			}
		}

		private CSteamID _playerID;

		public CSteamID judgeID;
	}
}
