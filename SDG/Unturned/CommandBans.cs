using System;
using Steamworks;

namespace SDG.Unturned
{
	public class CommandBans : Command
	{
		public CommandBans(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("BansCommandText");
			this._info = this.localization.format("BansInfoText");
			this._help = this.localization.format("BansHelpText");
		}

		protected override void execute(CSteamID executorID, string parameter)
		{
			if (!Dedicator.isDedicated)
			{
				return;
			}
			if (SteamBlacklist.list.Count == 0)
			{
				CommandWindow.LogError(this.localization.format("NoBansErrorText"));
				return;
			}
			CommandWindow.Log(this.localization.format("BansText"));
			for (int i = 0; i < SteamBlacklist.list.Count; i++)
			{
				SteamBlacklistID steamBlacklistID = SteamBlacklist.list[i];
				CommandWindow.Log(this.localization.format("BanNameText", new object[]
				{
					steamBlacklistID.playerID
				}));
				CommandWindow.Log(this.localization.format("BanJudgeText", new object[]
				{
					steamBlacklistID.judgeID
				}));
				CommandWindow.Log(this.localization.format("BanStatusText", new object[]
				{
					steamBlacklistID.reason,
					steamBlacklistID.duration,
					steamBlacklistID.getTime()
				}));
			}
		}
	}
}
