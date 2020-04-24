using System;
using Steamworks;

namespace SDG.Unturned
{
	public class CommandOwner : Command
	{
		public CommandOwner(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("OwnerCommandText");
			this._info = this.localization.format("OwnerInfoText");
			this._help = this.localization.format("OwnerHelpText");
		}

		protected override void execute(CSteamID executorID, string parameter)
		{
			if (!Dedicator.isDedicated)
			{
				return;
			}
			if (Provider.isServer)
			{
				CommandWindow.LogError(this.localization.format("RunningErrorText"));
				return;
			}
			CSteamID csteamID;
			if (!PlayerTool.tryGetSteamID(parameter, out csteamID))
			{
				CommandWindow.LogError(this.localization.format("InvalidSteamIDErrorText", new object[]
				{
					parameter
				}));
				return;
			}
			SteamAdminlist.ownerID = csteamID;
			CommandWindow.Log(this.localization.format("OwnerText", new object[]
			{
				csteamID
			}));
		}
	}
}
