using System;
using Steamworks;

namespace SDG.Unturned
{
	public class CommandKick : Command
	{
		public CommandKick(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("KickCommandText");
			this._info = this.localization.format("KickInfoText");
			this._help = this.localization.format("KickHelpText");
		}

		protected override void execute(CSteamID executorID, string parameter)
		{
			if (!Dedicator.isDedicated)
			{
				return;
			}
			if (!Provider.isServer)
			{
				CommandWindow.LogError(this.localization.format("NotRunningErrorText"));
				return;
			}
			string[] componentsFromSerial = Parser.getComponentsFromSerial(parameter, '/');
			if (componentsFromSerial.Length != 1 && componentsFromSerial.Length != 2)
			{
				CommandWindow.LogError(this.localization.format("InvalidParameterErrorText"));
				return;
			}
			SteamPlayer steamPlayer;
			if (!PlayerTool.tryGetSteamPlayer(componentsFromSerial[0], out steamPlayer))
			{
				CommandWindow.LogError(this.localization.format("NoPlayerErrorText", new object[]
				{
					componentsFromSerial[0]
				}));
				return;
			}
			if (componentsFromSerial.Length == 1)
			{
				Provider.kick(steamPlayer.playerID.steamID, this.localization.format("KickTextReason"));
			}
			else if (componentsFromSerial.Length == 2)
			{
				Provider.kick(steamPlayer.playerID.steamID, componentsFromSerial[1]);
			}
			CommandWindow.Log(this.localization.format("KickText", new object[]
			{
				steamPlayer.playerID.playerName
			}));
		}
	}
}
