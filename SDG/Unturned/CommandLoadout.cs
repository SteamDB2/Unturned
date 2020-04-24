using System;
using Steamworks;

namespace SDG.Unturned
{
	public class CommandLoadout : Command
	{
		public CommandLoadout(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("LoadoutCommandText");
			this._info = this.localization.format("LoadoutInfoText");
			this._help = this.localization.format("LoadoutHelpText");
		}

		protected override void execute(CSteamID executorID, string parameter)
		{
			string[] componentsFromSerial = Parser.getComponentsFromSerial(parameter, '/');
			if (componentsFromSerial.Length < 1)
			{
				CommandWindow.LogError(this.localization.format("InvalidParameterErrorText"));
				return;
			}
			byte b;
			if (!byte.TryParse(componentsFromSerial[0], out b) || (b != 255 && b > 10))
			{
				CommandWindow.LogError(this.localization.format("InvalidSkillsetIDErrorText", new object[]
				{
					componentsFromSerial[0]
				}));
				return;
			}
			ushort[] array = new ushort[componentsFromSerial.Length - 1];
			for (int i = 1; i < componentsFromSerial.Length; i++)
			{
				ushort num;
				if (!ushort.TryParse(componentsFromSerial[i], out num))
				{
					CommandWindow.LogError(this.localization.format("InvalidItemIDErrorText", new object[]
					{
						componentsFromSerial[i]
					}));
					return;
				}
				array[i - 1] = num;
			}
			if (b == 255)
			{
				PlayerInventory.loadout = array;
			}
			else
			{
				PlayerInventory.skillsets[(int)b] = array;
			}
			CommandWindow.Log(this.localization.format("LoadoutText", new object[]
			{
				b
			}));
		}
	}
}
