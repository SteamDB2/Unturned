using System;
using Steamworks;

namespace SDG.Unturned
{
	public class CommandFilter : Command
	{
		public CommandFilter(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("FilterCommandText");
			this._info = this.localization.format("FilterInfoText");
			this._help = this.localization.format("FilterHelpText");
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
			Provider.filterName = true;
			CommandWindow.Log(this.localization.format("FilterText"));
		}
	}
}
