using System;
using Steamworks;

namespace SDG.Unturned
{
	public class CommandHideAdmins : Command
	{
		public CommandHideAdmins(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("HideAdminsCommandText");
			this._info = this.localization.format("HideAdminsInfoText");
			this._help = this.localization.format("HideAdminsHelpText");
		}

		protected override void execute(CSteamID executorID, string parameter)
		{
			if (!Dedicator.isDedicated)
			{
				return;
			}
			if (Provider.isServer)
			{
				CommandWindow.LogError(this.localization.format("RunningErrorText"));
				return;
			}
			Provider.hideAdmins = true;
			CommandWindow.Log(this.localization.format("HideAdminsText"));
		}
	}
}
