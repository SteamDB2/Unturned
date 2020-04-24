using System;
using Steamworks;

namespace SDG.Unturned
{
	public class CommandPassword : Command
	{
		public CommandPassword(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("PasswordCommandText");
			this._info = this.localization.format("PasswordInfoText");
			this._help = this.localization.format("PasswordHelpText");
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
			if (parameter.Length == 0)
			{
				Provider.serverPassword = string.Empty;
				CommandWindow.Log(this.localization.format("DisableText"));
				return;
			}
			Provider.serverPassword = parameter;
			CommandWindow.Log(this.localization.format("PasswordText", new object[]
			{
				parameter
			}));
		}
	}
}
