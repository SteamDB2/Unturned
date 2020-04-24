using System;
using Steamworks;

namespace SDG.Unturned
{
	public class CommandPermits : Command
	{
		public CommandPermits(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("PermitsCommandText");
			this._info = this.localization.format("PermitsInfoText");
			this._help = this.localization.format("PermitsHelpText");
		}

		protected override void execute(CSteamID executorID, string parameter)
		{
			if (!Dedicator.isDedicated)
			{
				return;
			}
			if (SteamWhitelist.list.Count == 0)
			{
				CommandWindow.LogError(this.localization.format("NoPermitsErrorText"));
				return;
			}
			CommandWindow.Log(this.localization.format("PermitsText"));
			for (int i = 0; i < SteamWhitelist.list.Count; i++)
			{
				SteamWhitelistID steamWhitelistID = SteamWhitelist.list[i];
				CommandWindow.Log(this.localization.format("PermitNameText", new object[]
				{
					steamWhitelistID.steamID,
					steamWhitelistID.tag
				}));
				CommandWindow.Log(this.localization.format("PermitJudgeText", new object[]
				{
					steamWhitelistID.judgeID
				}));
			}
		}
	}
}
