using System;
using Steamworks;

namespace SDG.Unturned
{
	public class CommandSave : Command
	{
		public CommandSave(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("SaveCommandText");
			this._info = this.localization.format("SaveInfoText");
			this._help = this.localization.format("SaveHelpText");
		}

		protected override void execute(CSteamID executorID, string parameter)
		{
			SaveManager.save();
			CommandWindow.Log(this.localization.format("SaveText"));
		}
	}
}
