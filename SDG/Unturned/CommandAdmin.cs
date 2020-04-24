using System;
using Steamworks;

namespace SDG.Unturned
{
	public class CommandAdmin : Command
	{
		public CommandAdmin(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("AdminCommandText");
			this._info = this.localization.format("AdminInfoText");
			this._help = this.localization.format("AdminHelpText");
		}

		protected override void execute(CSteamID executorID, string parameter)
		{
			if (!Dedicator.isDedicated)
			{
				return;
			}
			if (!Provider.isServer)
			{
				CommandWindow.LogError(this.localization.format("NotRunningErrorText"));
				return;
			}
			CSteamID csteamID;
			if (!PlayerTool.tryGetSteamID(parameter, out csteamID))
			{
				CommandWindow.LogError(this.localization.format("NoPlayerErrorText", new object[]
				{
					parameter
				}));
				return;
			}
			SteamAdminlist.admin(csteamID, executorID);
			CommandWindow.Log(this.localization.format("AdminText", new object[]
			{
				csteamID
			}));
		}
	}
}
