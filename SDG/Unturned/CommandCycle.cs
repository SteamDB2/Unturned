using System;
using Steamworks;

namespace SDG.Unturned
{
	public class CommandCycle : Command
	{
		public CommandCycle(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("CycleCommandText");
			this._info = this.localization.format("CycleInfoText");
			this._help = this.localization.format("CycleHelpText");
		}

		protected override void execute(CSteamID executorID, string parameter)
		{
			uint num;
			if (!uint.TryParse(parameter, out num))
			{
				CommandWindow.LogError(this.localization.format("InvalidNumberErrorText", new object[]
				{
					parameter
				}));
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
			LightingManager.cycle = num;
			CommandWindow.Log(this.localization.format("CycleText", new object[]
			{
				num
			}));
		}
	}
}
