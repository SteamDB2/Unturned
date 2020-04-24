using System;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	public class CommandKill : Command
	{
		public CommandKill(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("KillCommandText");
			this._info = this.localization.format("KillInfoText");
			this._help = this.localization.format("KillHelpText");
		}

		protected override void execute(CSteamID executorID, string parameter)
		{
			if (!Provider.isServer)
			{
				CommandWindow.LogError(this.localization.format("NotRunningErrorText"));
				return;
			}
			SteamPlayer steamPlayer;
			if (!PlayerTool.tryGetSteamPlayer(parameter, out steamPlayer))
			{
				CommandWindow.LogError(this.localization.format("NoPlayerErrorText", new object[]
				{
					parameter
				}));
				return;
			}
			if (steamPlayer.player != null)
			{
				EPlayerKill eplayerKill;
				steamPlayer.player.life.askDamage(101, Vector3.up * 101f, EDeathCause.KILL, ELimb.SKULL, executorID, out eplayerKill);
			}
			CommandWindow.Log(this.localization.format("KillText", new object[]
			{
				steamPlayer.playerID.playerName
			}));
		}
	}
}
