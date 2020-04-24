using System;
using Steamworks;

namespace SDG.Unturned
{
	public class CommandRain : Command
	{
		public CommandRain(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("RainCommandText");
			this._info = this.localization.format("RainInfoText");
			this._help = this.localization.format("RainHelpText");
		}

		protected override void execute(CSteamID executorID, string parameter)
		{
			string[] componentsFromSerial = Parser.getComponentsFromSerial(parameter, '/');
			if (componentsFromSerial.Length != 4)
			{
				CommandWindow.LogError(this.localization.format("InvalidParameterErrorText"));
				return;
			}
			float num;
			if (!float.TryParse(componentsFromSerial[0], out num))
			{
				CommandWindow.LogError(this.localization.format("InvalidNumberErrorText", new object[]
				{
					parameter
				}));
				return;
			}
			float num2;
			if (!float.TryParse(componentsFromSerial[1], out num2))
			{
				CommandWindow.LogError(this.localization.format("InvalidNumberErrorText", new object[]
				{
					parameter
				}));
				return;
			}
			float num3;
			if (!float.TryParse(componentsFromSerial[2], out num3))
			{
				CommandWindow.LogError(this.localization.format("InvalidNumberErrorText", new object[]
				{
					parameter
				}));
				return;
			}
			float num4;
			if (!float.TryParse(componentsFromSerial[3], out num4))
			{
				CommandWindow.LogError(this.localization.format("InvalidNumberErrorText", new object[]
				{
					parameter
				}));
				return;
			}
			CommandWindow.Log(this.localization.format("RainText"));
		}
	}
}
