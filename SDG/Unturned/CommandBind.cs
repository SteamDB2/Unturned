using System;
using Steamworks;

namespace SDG.Unturned
{
	public class CommandBind : Command
	{
		public CommandBind(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("BindCommandText");
			this._info = this.localization.format("BindInfoText");
			this._help = this.localization.format("BindHelpText");
		}

		protected override void execute(CSteamID executorID, string parameter)
		{
			if (!Dedicator.isDedicated)
			{
				return;
			}
			if (!Parser.checkIP(parameter))
			{
				CommandWindow.LogError(this.localization.format("InvalidIPErrorText", new object[]
				{
					parameter
				}));
				return;
			}
			if (Provider.isServer)
			{
				CommandWindow.LogError(this.localization.format("RunningErrorText"));
				return;
			}
			Provider.ip = Parser.getUInt32FromIP(parameter);
			CommandWindow.Log(this.localization.format("BindText", new object[]
			{
				parameter
			}));
		}
	}
}
