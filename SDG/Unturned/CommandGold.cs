using System;
using Steamworks;

namespace SDG.Unturned
{
	public class CommandGold : Command
	{
		public CommandGold(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("GoldCommandText");
			this._info = this.localization.format("GoldInfoText");
			this._help = this.localization.format("GoldHelpText");
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
			Provider.isGold = true;
			CommandWindow.Log(this.localization.format("GoldText"));
		}
	}
}
