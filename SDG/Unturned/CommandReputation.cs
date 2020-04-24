using System;
using Steamworks;

namespace SDG.Unturned
{
	public class CommandReputation : Command
	{
		public CommandReputation(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("ReputationCommandText");
			this._info = this.localization.format("ReputationInfoText");
			this._help = this.localization.format("ReputationHelpText");
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
			if (componentsFromSerial.Length < 1 || componentsFromSerial.Length > 2)
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
			int num;
			if (!int.TryParse(componentsFromSerial[(!flag) ? 1 : 0], out num))
			{
				CommandWindow.LogError(this.localization.format("InvalidNumberErrorText", new object[]
				{
					componentsFromSerial[(!flag) ? 1 : 0]
				}));
				return;
			}
			steamPlayer.player.skills.askRep(num);
			string text = num.ToString();
			if (num > 0)
			{
				text = '+' + text;
			}
			CommandWindow.Log(this.localization.format("ReputationText", new object[]
			{
				steamPlayer.playerID.playerName,
				text
			}));
		}
	}
}
