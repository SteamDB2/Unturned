using System;
using Steamworks;

namespace SDG.Unturned
{
	public class CommandTime : Command
	{
		public CommandTime(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("TimeCommandText");
			this._info = this.localization.format("TimeInfoText");
			this._help = this.localization.format("TimeHelpText");
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
			uint num;
			if (!uint.TryParse(parameter, out num))
			{
				CommandWindow.LogError(this.localization.format("InvalidNumberErrorText", new object[]
				{
					parameter
				}));
				return;
			}
			LightingManager.time = num;
			CommandWindow.Log(this.localization.format("TimeText", new object[]
			{
				num
			}));
		}
	}
}
