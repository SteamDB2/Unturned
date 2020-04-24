using System;
using Steamworks;

namespace SDG.Unturned
{
	public class CommandVehicle : Command
	{
		public CommandVehicle(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("VehicleCommandText");
			this._info = this.localization.format("VehicleInfoText");
			this._help = this.localization.format("VehicleHelpText");
		}

		protected override void execute(CSteamID executorID, string parameter)
		{
			if (!Provider.isServer)
			{
				CommandWindow.LogError(this.localization.format("NotRunningErrorText"));
				return;
			}
			if (!Provider.hasCheats)
			{
				CommandWindow.LogError(this.localization.format("CheatsErrorText"));
				return;
			}
			string[] componentsFromSerial = Parser.getComponentsFromSerial(parameter, '/');
			if (componentsFromSerial.Length < 1 || componentsFromSerial.Length > 3)
			{
				CommandWindow.LogError(this.localization.format("InvalidParameterErrorText"));
				return;
			}
			bool flag = false;
			SteamPlayer steamPlayer;
			if (!PlayerTool.tryGetSteamPlayer(componentsFromSerial[0], out steamPlayer))
			{
				steamPlayer = PlayerTool.getSteamPlayer(executorID);
				if (steamPlayer == null)
				{
					CommandWindow.LogError(this.localization.format("NoPlayerErrorText", new object[]
					{
						componentsFromSerial[0]
					}));
					return;
				}
				flag = true;
			}
			ushort num;
			if (!ushort.TryParse(componentsFromSerial[(!flag) ? 1 : 0], out num))
			{
				CommandWindow.LogError(this.localization.format("InvalidVehicleIDErrorText", new object[]
				{
					componentsFromSerial[(!flag) ? 1 : 0]
				}));
				return;
			}
			if (!VehicleTool.giveVehicle(steamPlayer.player, num))
			{
				CommandWindow.LogError(this.localization.format("NoVehicleIDErrorText", new object[]
				{
					num
				}));
				return;
			}
			CommandWindow.Log(this.localization.format("VehicleText", new object[]
			{
				steamPlayer.playerID.playerName,
				num
			}));
		}
	}
}
