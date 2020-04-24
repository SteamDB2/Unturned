using System;
using Steamworks;

namespace SDG.Unturned
{
	public class CommandGameMode : Command
	{
		public CommandGameMode(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("GameModeCommandText");
			this._info = this.localization.format("GameModeInfoText");
			this._help = this.localization.format("GameModeHelpText");
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
			Provider.selectedGameModeName = parameter;
			CommandWindow.Log(this.localization.format("GameModeText", new object[]
			{
				parameter
			}));
		}
	}
}
