using System;
using Steamworks;

namespace SDG.Unturned
{
	public class CommandAirdrop : Command
	{
		public CommandAirdrop(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("AirdropCommandText");
			this._info = this.localization.format("AirdropInfoText");
			this._help = this.localization.format("AirdropHelpText");
		}

		protected override void execute(CSteamID executorID, string parameter)
		{
			if (!LevelManager.hasAirdrop)
			{
				return;
			}
			LevelManager.airdropFrequency = 0u;
			CommandWindow.Log(this.localization.format("AirdropText"));
		}
	}
}
