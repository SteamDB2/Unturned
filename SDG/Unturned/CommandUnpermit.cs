using System;
using Steamworks;

namespace SDG.Unturned
{
	public class CommandUnpermit : Command
	{
		public CommandUnpermit(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("UnpermitCommandText");
			this._info = this.localization.format("UnpermitInfoText");
			this._help = this.localization.format("UnpermitHelpText");
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
			if (!SteamWhitelist.unwhitelist(csteamID))
			{
				CommandWindow.LogError(this.localization.format("NoPlayerErrorText", new object[]
				{
					csteamID
				}));
				return;
			}
			CommandWindow.Log(this.localization.format("UnpermitText", new object[]
			{
				csteamID
			}));
		}
	}
}
