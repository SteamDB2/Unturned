using System;
using Steamworks;

namespace SDG.Unturned
{
	public class CommandVotify : Command
	{
		public CommandVotify(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("VotifyCommandText");
			this._info = this.localization.format("VotifyInfoText");
			this._help = this.localization.format("VotifyHelpText");
		}

		protected override void execute(CSteamID executorID, string parameter)
		{
			if (!Dedicator.isDedicated)
			{
				return;
			}
			string[] componentsFromSerial = Parser.getComponentsFromSerial(parameter, '/');
			if (componentsFromSerial.Length != 6)
			{
				CommandWindow.LogError(this.localization.format("InvalidParameterErrorText"));
				return;
			}
			bool voteAllowed;
			if (componentsFromSerial[0].ToLower() == "y")
			{
				voteAllowed = true;
			}
			else
			{
				if (!(componentsFromSerial[0].ToLower() == "n"))
				{
					CommandWindow.LogError(this.localization.format("InvalidBooleanErrorText", new object[]
					{
						componentsFromSerial[0]
					}));
					return;
				}
				voteAllowed = false;
			}
			float votePassCooldown;
			if (!float.TryParse(componentsFromSerial[1], out votePassCooldown))
			{
				CommandWindow.LogError(this.localization.format("InvalidNumberErrorText", new object[]
				{
					componentsFromSerial[1]
				}));
				return;
			}
			float voteFailCooldown;
			if (!float.TryParse(componentsFromSerial[2], out voteFailCooldown))
			{
				CommandWindow.LogError(this.localization.format("InvalidNumberErrorText", new object[]
				{
					componentsFromSerial[2]
				}));
				return;
			}
			float voteDuration;
			if (!float.TryParse(componentsFromSerial[3], out voteDuration))
			{
				CommandWindow.LogError(this.localization.format("InvalidNumberErrorText", new object[]
				{
					componentsFromSerial[3]
				}));
				return;
			}
			float votePercentage;
			if (!float.TryParse(componentsFromSerial[4], out votePercentage))
			{
				CommandWindow.LogError(this.localization.format("InvalidNumberErrorText", new object[]
				{
					componentsFromSerial[4]
				}));
				return;
			}
			byte votePlayers;
			if (!byte.TryParse(componentsFromSerial[5], out votePlayers))
			{
				CommandWindow.LogError(this.localization.format("InvalidNumberErrorText", new object[]
				{
					componentsFromSerial[5]
				}));
				return;
			}
			ChatManager.voteAllowed = voteAllowed;
			ChatManager.votePassCooldown = votePassCooldown;
			ChatManager.voteFailCooldown = voteFailCooldown;
			ChatManager.voteDuration = voteDuration;
			ChatManager.votePercentage = votePercentage;
			ChatManager.votePlayers = votePlayers;
			CommandWindow.Log(this.localization.format("VotifyText"));
		}
	}
}
