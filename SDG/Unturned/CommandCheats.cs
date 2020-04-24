using System;
using Steamworks;

namespace SDG.Unturned
{
	public class CommandCheats : Command
	{
		public CommandCheats(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("CheatsCommandText");
			this._info = this.localization.format("CheatsInfoText");
			this._help = this.localization.format("CheatsHelpText");
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
			Provider.hasCheats = true;
			CommandWindow.Log(this.localization.format("CheatsText"));
		}
	}
}
