using System;
using System.Collections.Generic;
using Steamworks;

namespace SDG.Unturned
{
	public class Commander
	{
		public static List<Command> commands { get; set; }

		public static void register(Command command)
		{
			int num = Commander.commands.BinarySearch(command);
			if (num < 0)
			{
				num = ~num;
			}
			Commander.commands.Insert(num, command);
		}

		public static void deregister(Command command)
		{
			Commander.commands.Remove(command);
		}

		public static bool execute(CSteamID executorID, string command)
		{
			string method = command;
			string parameter = string.Empty;
			int num = command.IndexOf(' ');
			if (num != -1)
			{
				method = command.Substring(0, num);
				parameter = command.Substring(num + 1, command.Length - num - 1);
			}
			for (int i = 0; i < Commander.commands.Count; i++)
			{
				if (Commander.commands[i].check(executorID, method, parameter))
				{
					return true;
				}
			}
			return false;
		}

		public static void init()
		{
			Commander.commands = new List<Command>();
			Commander.register(new CommandModules(Localization.read("/Server/ServerCommandModules.dat")));
			Commander.register(new CommandHelp(Localization.read("/Server/ServerCommandHelp.dat")));
			Commander.register(new CommandName(Localization.read("/Server/ServerCommandName.dat")));
			Commander.register(new CommandPort(Localization.read("/Server/ServerCommandPort.dat")));
			Commander.register(new CommandPassword(Localization.read("/Server/ServerCommandPassword.dat")));
			Commander.register(new CommandMaxPlayers(Localization.read("/Server/ServerCommandMaxPlayers.dat")));
			Commander.register(new CommandQueue(Localization.read("/Server/ServerCommandQueue.dat")));
			Commander.register(new CommandMap(Localization.read("/Server/ServerCommandMap.dat")));
			Commander.register(new CommandPvE(Localization.read("/Server/ServerCommandPvE.dat")));
			Commander.register(new CommandWhitelisted(Localization.read("/Server/ServerCommandWhitelisted.dat")));
			Commander.register(new CommandCheats(Localization.read("/Server/ServerCommandCheats.dat")));
			Commander.register(new CommandHideAdmins(Localization.read("/Server/ServerCommandHideAdmins.dat")));
			Commander.register(new CommandSync(Localization.read("/Server/ServerCommandSync.dat")));
			Commander.register(new CommandFilter(Localization.read("/Server/ServerCommandFilter.dat")));
			Commander.register(new CommandVotify(Localization.read("/Server/ServerCommandVotify.dat")));
			Commander.register(new CommandMode(Localization.read("/Server/ServerCommandMode.dat")));
			Commander.register(new CommandGameMode(Localization.read("/Server/ServerCommandGameMode.dat")));
			Commander.register(new CommandGold(Localization.read("/Server/ServerCommandGold.dat")));
			Commander.register(new CommandCamera(Localization.read("/Server/ServerCommandCamera.dat")));
			Commander.register(new CommandCycle(Localization.read("/Server/ServerCommandCycle.dat")));
			Commander.register(new CommandTime(Localization.read("/Server/ServerCommandTime.dat")));
			Commander.register(new CommandDay(Localization.read("/Server/ServerCommandDay.dat")));
			Commander.register(new CommandNight(Localization.read("/Server/ServerCommandNight.dat")));
			Commander.register(new CommandWeather(Localization.read("/Server/ServerCommandWeather.dat")));
			Commander.register(new CommandAirdrop(Localization.read("/Server/ServerCommandAirdrop.dat")));
			Commander.register(new CommandKick(Localization.read("/Server/ServerCommandKick.dat")));
			Commander.register(new CommandSpy(Localization.read("/Server/ServerCommandSpy.dat")));
			Commander.register(new CommandBan(Localization.read("/Server/ServerCommandBan.dat")));
			Commander.register(new CommandUnban(Localization.read("/Server/ServerCommandUnban.dat")));
			Commander.register(new CommandBans(Localization.read("/Server/ServerCommandBans.dat")));
			Commander.register(new CommandAdmin(Localization.read("/Server/ServerCommandAdmin.dat")));
			Commander.register(new CommandUnadmin(Localization.read("/Server/ServerCommandUnadmin.dat")));
			Commander.register(new CommandAdmins(Localization.read("/Server/ServerCommandAdmins.dat")));
			Commander.register(new CommandOwner(Localization.read("/Server/ServerCommandOwner.dat")));
			Commander.register(new CommandPermit(Localization.read("/Server/ServerCommandPermit.dat")));
			Commander.register(new CommandUnpermit(Localization.read("/Server/ServerCommandUnpermit.dat")));
			Commander.register(new CommandPermits(Localization.read("/Server/ServerCommandPermits.dat")));
			Commander.register(new CommandPlayers(Localization.read("/Server/ServerCommandPlayers.dat")));
			Commander.register(new CommandSay(Localization.read("/Server/ServerCommandSay.dat")));
			Commander.register(new CommandWelcome(Localization.read("/Server/ServerCommandWelcome.dat")));
			Commander.register(new CommandSlay(Localization.read("/Server/ServerCommandSlay.dat")));
			Commander.register(new CommandKill(Localization.read("/Server/ServerCommandKill.dat")));
			Commander.register(new CommandGive(Localization.read("/Server/ServerCommandGive.dat")));
			Commander.register(new CommandLoadout(Localization.read("/Server/ServerCommandLoadout.dat")));
			Commander.register(new CommandExperience(Localization.read("/Server/ServerCommandExperience.dat")));
			Commander.register(new CommandReputation(Localization.read("/Server/ServerCommandReputation.dat")));
			Commander.register(new CommandFlag(Localization.read("/Server/ServerCommandFlag.dat")));
			Commander.register(new CommandQuest(Localization.read("/Server/ServerCommandQuest.dat")));
			Commander.register(new CommandVehicle(Localization.read("/Server/ServerCommandVehicle.dat")));
			Commander.register(new CommandTeleport(Localization.read("/Server/ServerCommandTeleport.dat")));
			Commander.register(new CommandTimeout(Localization.read("/Server/ServerCommandTimeout.dat")));
			Commander.register(new CommandChatrate(Localization.read("/Server/ServerCommandChatrate.dat")));
			Commander.register(new CommandLog(Localization.read("/Server/ServerCommandLog.dat")));
			Commander.register(new CommandDebug(Localization.read("/Server/ServerCommandDebug.dat")));
			Commander.register(new CommandResetConfig(Localization.read("/Server/ServerCommandResetConfig.dat")));
			Commander.register(new CommandBind(Localization.read("/Server/ServerCommandBind.dat")));
			Commander.register(new CommandSave(Localization.read("/Server/ServerCommandSave.dat")));
			Commander.register(new CommandShutdown(Localization.read("/Server/ServerCommandShutdown.dat")));
		}
	}
}
