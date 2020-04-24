using System;
using Steamworks;

namespace SDG.Unturned
{
	public class CommandPlayers : Command
	{
		public CommandPlayers(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("PlayersCommandText");
			this._info = this.localization.format("PlayersInfoText");
			this._help = this.localization.format("PlayersHelpText");
		}

		protected override void execute(CSteamID executorID, string parameter)
		{
			if (!Dedicator.isDedicated)
			{
				return;
			}
			if (Provider.clients.Count == 0)
			{
				CommandWindow.LogError(this.localization.format("NoPlayersErrorText"));
				return;
			}
			CommandWindow.Log(this.localization.format("PlayersText"));
			for (int i = 0; i < Provider.clients.Count; i++)
			{
				SteamPlayer steamPlayer = Provider.clients[i];
				CommandWindow.Log(this.localization.format("PlayerIDText", new object[]
				{
					steamPlayer.playerID.steamID,
					steamPlayer.playerID.playerName,
					steamPlayer.playerID.characterName,
					(int)(steamPlayer.ping * 1000f)
				}));
			}
		}
	}
}
