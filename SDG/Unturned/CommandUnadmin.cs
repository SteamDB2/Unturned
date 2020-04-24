using System;
using Steamworks;

namespace SDG.Unturned
{
	public class CommandUnadmin : Command
	{
		public CommandUnadmin(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("UnadminCommandText");
			this._info = this.localization.format("UnadminInfoText");
			this._help = this.localization.format("UnadminHelpText");
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
			SteamAdminlist.unadmin(csteamID);
			CommandWindow.Log(this.localization.format("UnadminText", new object[]
			{
				csteamID
			}));
		}
	}
}
