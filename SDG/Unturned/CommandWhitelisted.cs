using System;
using Steamworks;

namespace SDG.Unturned
{
	public class CommandWhitelisted : Command
	{
		public CommandWhitelisted(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("WhitelistedCommandText");
			this._info = this.localization.format("WhitelistedInfoText");
			this._help = this.localization.format("WhitelistedHelpText");
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
			Provider.isWhitelisted = true;
			CommandWindow.Log(this.localization.format("WhitelistedText"));
		}
	}
}
