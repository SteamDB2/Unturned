using System;
using Steamworks;

namespace SDG.Unturned
{
	public class CommandPermit : Command
	{
		public CommandPermit(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("PermitCommandText");
			this._info = this.localization.format("PermitInfoText");
			this._help = this.localization.format("PermitHelpText");
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
			if (componentsFromSerial.Length != 2)
			{
				CommandWindow.LogError(this.localization.format("InvalidParameterErrorText"));
				return;
			}
			CSteamID csteamID;
			if (!PlayerTool.tryGetSteamID(componentsFromSerial[0], out csteamID))
			{
				CommandWindow.LogError(this.localization.format("InvalidSteamIDErrorText", new object[]
				{
					componentsFromSerial[0]
				}));
				return;
			}
			SteamWhitelist.whitelist(csteamID, componentsFromSerial[1], executorID);
			CommandWindow.Log(this.localization.format("PermitText", new object[]
			{
				csteamID,
				componentsFromSerial[1]
			}));
		}
	}
}
