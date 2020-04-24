using System;
using Steamworks;

namespace SDG.Unturned
{
	public class CommandSpy : Command
	{
		public CommandSpy(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("SpyCommandText");
			this._info = this.localization.format("SpyInfoText");
			this._help = this.localization.format("SpyHelpText");
		}

		protected override void execute(CSteamID executorID, string parameter)
		{
			if (!Provider.isServer)
			{
				CommandWindow.LogError(this.localization.format("NotRunningErrorText"));
				return;
			}
			string[] componentsFromSerial = Parser.getComponentsFromSerial(parameter, '/');
			if (componentsFromSerial.Length != 1)
			{
				CommandWindow.LogError(this.localization.format("InvalidParameterErrorText"));
				return;
			}
			SteamPlayer steamPlayer;
			if (!PlayerTool.tryGetSteamPlayer(componentsFromSerial[0], out steamPlayer) || steamPlayer.player == null)
			{
				CommandWindow.LogError(this.localization.format("NoPlayerErrorText", new object[]
				{
					componentsFromSerial[0]
				}));
				return;
			}
			steamPlayer.player.sendScreenshot(executorID, null);
			CommandWindow.Log(this.localization.format("SpyText", new object[]
			{
				steamPlayer.playerID.playerName
			}));
		}
	}
}
