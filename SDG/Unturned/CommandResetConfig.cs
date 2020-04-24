using System;
using Steamworks;

namespace SDG.Unturned
{
	public class CommandResetConfig : Command
	{
		public CommandResetConfig(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("ResetConfigCommandText");
			this._info = this.localization.format("ResetConfigInfoText");
			this._help = this.localization.format("ResetConfigHelpText");
		}

		protected override void execute(CSteamID executorID, string parameter)
		{
			Provider.resetConfig();
			CommandWindow.Log(this.localization.format("ResetConfigText"));
		}
	}
}
