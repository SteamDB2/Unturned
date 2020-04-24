using System;
using Steamworks;

namespace SDG.Unturned
{
	public class CommandUnban : Command
	{
		public CommandUnban(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("UnbanCommandText");
			this._info = this.localization.format("UnbanInfoText");
			this._help = this.localization.format("UnbanHelpText");
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
				CommandWindow.LogError(this.localization.format("InvalidSteamIDErrorText", new object[]
				{
					parameter
				}));
				return;
			}
			if (!SteamBlacklist.unban(csteamID))
			{
				CommandWindow.LogError(this.localization.format("NoPlayerErrorText", new object[]
				{
					csteamID
				}));
				return;
			}
			CommandWindow.Log(this.localization.format("UnbanText", new object[]
			{
				csteamID
			}));
		}
	}
}
