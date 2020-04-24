using System;
using Steamworks;

namespace SDG.Unturned
{
	public class CommandPort : Command
	{
		public CommandPort(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("PortCommandText");
			this._info = this.localization.format("PortInfoText");
			this._help = this.localization.format("PortHelpText");
		}

		protected override void execute(CSteamID executorID, string parameter)
		{
			if (!Dedicator.isDedicated)
			{
				return;
			}
			ushort num;
			if (!ushort.TryParse(parameter, out num))
			{
				CommandWindow.LogError(this.localization.format("InvalidNumberErrorText", new object[]
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
			Provider.port = num;
			CommandWindow.Log(this.localization.format("PortText", new object[]
			{
				num
			}));
		}
	}
}
