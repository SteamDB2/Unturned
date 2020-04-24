using System;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	public class CommandWelcome : Command
	{
		public CommandWelcome(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("WelcomeCommandText");
			this._info = this.localization.format("WelcomeInfoText");
			this._help = this.localization.format("WelcomeHelpText");
		}

		protected override void execute(CSteamID executorID, string parameter)
		{
			if (!Dedicator.isDedicated)
			{
				return;
			}
			string[] componentsFromSerial = Parser.getComponentsFromSerial(parameter, '/');
			if (componentsFromSerial.Length != 1 && componentsFromSerial.Length != 4)
			{
				CommandWindow.LogError(this.localization.format("InvalidParameterErrorText"));
				return;
			}
			ChatManager.welcomeText = componentsFromSerial[0];
			if (componentsFromSerial.Length == 1)
			{
				ChatManager.welcomeColor = Palette.SERVER;
			}
			else if (componentsFromSerial.Length == 4)
			{
				byte b;
				if (!byte.TryParse(componentsFromSerial[1], out b))
				{
					CommandWindow.LogError(this.localization.format("InvalidNumberErrorText", new object[]
					{
						componentsFromSerial[0]
					}));
					return;
				}
				byte b2;
				if (!byte.TryParse(componentsFromSerial[2], out b2))
				{
					CommandWindow.LogError(this.localization.format("InvalidNumberErrorText", new object[]
					{
						componentsFromSerial[1]
					}));
					return;
				}
				byte b3;
				if (!byte.TryParse(componentsFromSerial[3], out b3))
				{
					CommandWindow.LogError(this.localization.format("InvalidNumberErrorText", new object[]
					{
						componentsFromSerial[2]
					}));
					return;
				}
				ChatManager.welcomeColor = new Color((float)b / 255f, (float)b2 / 255f, (float)b3 / 255f);
			}
			CommandWindow.Log(this.localization.format("WelcomeText", new object[]
			{
				componentsFromSerial[0]
			}));
		}
	}
}
