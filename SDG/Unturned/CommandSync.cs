using System;
using Steamworks;

namespace SDG.Unturned
{
	public class CommandSync : Command
	{
		public CommandSync(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("SyncCommandText");
			this._info = this.localization.format("SyncInfoText");
			this._help = this.localization.format("SyncHelpText");
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
			PlayerSavedata.hasSync = true;
			CommandWindow.Log(this.localization.format("SyncText"));
		}
	}
}
