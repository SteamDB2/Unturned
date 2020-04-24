using System;
using Steamworks;

namespace SDG.Unturned
{
	public class CommandDay : Command
	{
		public CommandDay(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("DayCommandText");
			this._info = this.localization.format("DayInfoText");
			this._help = this.localization.format("DayHelpText");
		}

		protected override void execute(CSteamID executorID, string parameter)
		{
			if (!Provider.isServer)
			{
				CommandWindow.LogError(this.localization.format("NotRunningErrorText"));
				return;
			}
			if (Provider.isServer && Level.info.type == ELevelType.HORDE)
			{
				CommandWindow.LogError(this.localization.format("HordeErrorText"));
				return;
			}
			if (Provider.isServer && Level.info.type == ELevelType.ARENA)
			{
				CommandWindow.LogError(this.localization.format("ArenaErrorText"));
				return;
			}
			LightingManager.time = (uint)(LightingManager.cycle * LevelLighting.transition);
			CommandWindow.Log(this.localization.format("DayText"));
		}
	}
}
