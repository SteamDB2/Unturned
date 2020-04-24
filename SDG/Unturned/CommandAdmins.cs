using System;
using Steamworks;

namespace SDG.Unturned
{
	public class CommandAdmins : Command
	{
		public CommandAdmins(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("AdminsCommandText");
			this._info = this.localization.format("AdminsInfoText");
			this._help = this.localization.format("AdminsHelpText");
		}

		protected override void execute(CSteamID executorID, string parameter)
		{
			if (!Dedicator.isDedicated)
			{
				return;
			}
			if (SteamAdminlist.list.Count == 0)
			{
				CommandWindow.LogError(this.localization.format("NoAdminsErrorText"));
				return;
			}
			CommandWindow.Log(this.localization.format("AdminsText"));
			for (int i = 0; i < SteamAdminlist.list.Count; i++)
			{
				SteamAdminID steamAdminID = SteamAdminlist.list[i];
				CommandWindow.Log(this.localization.format("AdminNameText", new object[]
				{
					steamAdminID.playerID
				}));
				CommandWindow.Log(this.localization.format("AdminJudgeText", new object[]
				{
					steamAdminID.judgeID
				}));
			}
		}
	}
}
