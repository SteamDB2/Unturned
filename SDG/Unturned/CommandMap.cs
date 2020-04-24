using System;
using Steamworks;

namespace SDG.Unturned
{
	public class CommandMap : Command
	{
		public CommandMap(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("MapCommandText");
			this._info = this.localization.format("MapInfoText");
			this._help = this.localization.format("MapHelpText");
		}

		protected override void execute(CSteamID executorID, string parameter)
		{
			if (!Dedicator.isDedicated)
			{
				return;
			}
			if (!Level.exists(parameter))
			{
				CommandWindow.LogError(this.localization.format("NoMapErrorText", new object[]
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
			Provider.map = parameter;
			CommandWindow.Log(this.localization.format("MapText", new object[]
			{
				parameter
			}));
		}
	}
}
