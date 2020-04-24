using System;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	public class CommandSlay : Command
	{
		public CommandSlay(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("SlayCommandText");
			this._info = this.localization.format("SlayInfoText");
			this._help = this.localization.format("SlayHelpText");
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
			P2PSessionState_t p2PSessionState_t;
			uint ip;
			if (SteamGameServerNetworking.GetP2PSessionState(steamPlayer.playerID.steamID, ref p2PSessionState_t))
			{
				ip = p2PSessionState_t.m_nRemoteIP;
			}
			else
			{
				ip = 0u;
			}
			if (componentsFromSerial.Length == 1)
			{
				SteamBlacklist.ban(steamPlayer.playerID.steamID, ip, executorID, this.localization.format("SlayTextReason"), SteamBlacklist.PERMANENT);
			}
			else if (componentsFromSerial.Length == 2)
			{
				SteamBlacklist.ban(steamPlayer.playerID.steamID, ip, executorID, componentsFromSerial[1], SteamBlacklist.PERMANENT);
			}
			if (steamPlayer.player != null)
			{
				EPlayerKill eplayerKill;
				steamPlayer.player.life.askDamage(101, Vector3.up * 101f, EDeathCause.KILL, ELimb.SKULL, executorID, out eplayerKill);
			}
			CommandWindow.Log(this.localization.format("SlayText", new object[]
			{
				steamPlayer.playerID.playerName
			}));
		}
	}
}
