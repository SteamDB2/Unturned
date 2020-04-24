using System;
using Steamworks;

namespace SDG.Unturned
{
	public class CommandDecay : Command
	{
		public CommandDecay(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("DecayCommandText");
			this._info = this.localization.format("DecayInfoText");
			this._help = this.localization.format("DecayHelpText");
		}

		protected override void execute(CSteamID executorID, string parameter)
		{
			string[] componentsFromSerial = Parser.getComponentsFromSerial(parameter, '/');
			if (componentsFromSerial.Length != 2)
			{
				CommandWindow.LogError(this.localization.format("InvalidParameterErrorText"));
				return;
			}
			uint num;
			if (!uint.TryParse(componentsFromSerial[0], out num))
			{
				CommandWindow.LogError(this.localization.format("InvalidNumberErrorText", new object[]
				{
					parameter
				}));
				return;
			}
			uint num2;
			if (!uint.TryParse(componentsFromSerial[1], out num2))
			{
				CommandWindow.LogError(this.localization.format("InvalidNumberErrorText", new object[]
				{
					parameter
				}));
				return;
			}
			CommandWindow.Log(this.localization.format("DecayText"));
		}
	}
}
