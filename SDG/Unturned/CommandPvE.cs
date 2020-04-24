using System;
using Steamworks;

namespace SDG.Unturned
{
	public class CommandPvE : Command
	{
		public CommandPvE(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("PvECommandText");
			this._info = this.localization.format("PvEInfoText");
			this._help = this.localization.format("PvEHelpText");
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
			Provider.isPvP = false;
			CommandWindow.Log(this.localization.format("PvEText"));
		}
	}
}
