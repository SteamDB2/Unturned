using System;
using Steamworks;

namespace SDG.Unturned
{
	public class CommandArmor : Command
	{
		public CommandArmor(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("ArmorCommandText");
			this._info = this.localization.format("ArmorInfoText");
			this._help = this.localization.format("ArmorHelpText");
		}

		protected override void execute(CSteamID executorID, string parameter)
		{
			string[] componentsFromSerial = Parser.getComponentsFromSerial(parameter, '/');
			if (componentsFromSerial.Length != 2)
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
			CommandWindow.Log(this.localization.format("ArmorText"));
		}
	}
}
