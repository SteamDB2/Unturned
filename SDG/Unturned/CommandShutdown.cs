using System;
using Steamworks;

namespace SDG.Unturned
{
	public class CommandShutdown : Command
	{
		public CommandShutdown(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("ShutdownCommandText");
			this._info = this.localization.format("ShutdownInfoText");
			this._help = this.localization.format("ShutdownHelpText");
		}

		protected override void execute(CSteamID executorID, string parameter)
		{
			if (!Dedicator.isDedicated)
			{
				return;
			}
			string[] componentsFromSerial = Parser.getComponentsFromSerial(parameter, '/');
			if (componentsFromSerial.Length > 1)
			{
				CommandWindow.LogError(this.localization.format("InvalidParameterErrorText"));
				return;
			}
			if (componentsFromSerial.Length == 0)
			{
				Provider.shutdown();
			}
			else
			{
				int timer;
				if (!int.TryParse(componentsFromSerial[0], out timer))
				{
					CommandWindow.LogError(this.localization.format("InvalidNumberErrorText", new object[]
					{
						parameter
					}));
					return;
				}
				Provider.shutdown(timer);
				CommandWindow.LogError(this.localization.format("ShutdownText", new object[]
				{
					parameter
				}));
			}
		}
	}
}
