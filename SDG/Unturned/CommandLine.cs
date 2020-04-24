using System;
using System.Collections.Generic;

namespace SDG.Unturned
{
	public class CommandLine
	{
		public static bool tryGetConnect(string line, out uint ip, out ushort port, out string pass)
		{
			ip = 0u;
			port = 0;
			pass = string.Empty;
			int num = line.ToLower().IndexOf("+connect ");
			if (num == -1)
			{
				return false;
			}
			int num2 = line.IndexOf(':', num + 9);
			string text = line.Substring(num + 9, num2 - num - 9);
			if (Parser.checkIP(text))
			{
				ip = Parser.getUInt32FromIP(text);
			}
			else if (!uint.TryParse(text, out ip))
			{
				return false;
			}
			int num3 = line.IndexOf(' ', num2 + 1);
			if (num3 == -1)
			{
				if (!ushort.TryParse(line.Substring(num2 + 1, line.Length - num2 - 1), out port))
				{
					return false;
				}
				port -= port % 3;
				int num4 = line.ToLower().IndexOf("+password ");
				if (num4 != -1)
				{
					pass = line.Substring(num4 + 10, line.Length - num4 - 10);
				}
				return true;
			}
			else
			{
				if (!ushort.TryParse(line.Substring(num2 + 1, num3 - num2 - 1), out port))
				{
					return false;
				}
				port -= port % 3;
				int num5 = line.ToLower().IndexOf("+password ");
				if (num5 != -1)
				{
					pass = line.Substring(num5 + 10, line.Length - num5 - 10);
				}
				return true;
			}
		}

		public static bool tryGetLobby(string line, out ulong lobby)
		{
			lobby = 0UL;
			int num = line.ToLower().IndexOf("+connect_lobby ");
			if (num == -1)
			{
				return false;
			}
			int num2 = line.IndexOf(' ', num + 15);
			if (num2 == -1)
			{
				return ulong.TryParse(line.Substring(num + 15, line.Length - num - 15), out lobby);
			}
			return ulong.TryParse(line.Substring(num + 15, num2 - num - 15), out lobby);
		}

		public static bool tryGetLanguage(out string local, out string path)
		{
			local = string.Empty;
			path = string.Empty;
			string[] commandLineArgs = Environment.GetCommandLineArgs();
			for (int i = 0; i < commandLineArgs.Length; i++)
			{
				if (commandLineArgs[i].Substring(0, 1) == "+")
				{
					local = commandLineArgs[i].Substring(1, commandLineArgs[i].Length - 1);
					if (Provider.provider.workshopService.ugc != null)
					{
						for (int j = 0; j < Provider.provider.workshopService.ugc.Count; j++)
						{
							SteamContent steamContent = Provider.provider.workshopService.ugc[j];
							if (steamContent.type == ESteamUGCType.LOCALIZATION && ReadWrite.folderExists(steamContent.path + "/" + local, false))
							{
								path = steamContent.path + "/";
								return true;
							}
						}
					}
					if (ReadWrite.folderExists("/Localization/" + local))
					{
						path = ReadWrite.PATH + "/Localization/";
						return true;
					}
				}
			}
			return false;
		}

		public static bool tryGetServer(out ESteamServerVisibility visibility, out string id)
		{
			visibility = ESteamServerVisibility.LAN;
			id = string.Empty;
			string commandLine = Environment.CommandLine;
			int num = commandLine.ToLower().IndexOf("+secureserver", StringComparison.OrdinalIgnoreCase);
			if (num != -1)
			{
				visibility = ESteamServerVisibility.Internet;
				id = commandLine.Substring(num + 14, commandLine.Length - num - 14);
				return !(id == "Singleplayer");
			}
			int num2 = commandLine.ToLower().IndexOf("+insecureserver", StringComparison.OrdinalIgnoreCase);
			if (num2 != -1)
			{
				visibility = ESteamServerVisibility.Internet;
				id = commandLine.Substring(num2 + 16, commandLine.Length - num2 - 16);
				return !(id == "Singleplayer");
			}
			int num3 = commandLine.ToLower().IndexOf("+internetserver", StringComparison.OrdinalIgnoreCase);
			if (num3 != -1)
			{
				visibility = ESteamServerVisibility.Internet;
				id = commandLine.Substring(num3 + 16, commandLine.Length - num3 - 16);
				return !(id == "Singleplayer");
			}
			int num4 = commandLine.ToLower().IndexOf("+lanserver", StringComparison.OrdinalIgnoreCase);
			if (num4 != -1)
			{
				visibility = ESteamServerVisibility.LAN;
				id = commandLine.Substring(num4 + 11, commandLine.Length - num4 - 11);
				return !(id == "Singleplayer");
			}
			return false;
		}

		public static bool tryGetVR()
		{
			string commandLine = Environment.CommandLine;
			int num = commandLine.ToLower().IndexOf("-vr");
			return num != -1;
		}

		public static string[] getCommands()
		{
			string[] commandLineArgs = Environment.GetCommandLineArgs();
			List<string> list = new List<string>();
			if (CommandLine.onGetCommands != null)
			{
				CommandLine.onGetCommands(list);
			}
			bool flag = false;
			for (int i = 0; i < commandLineArgs.Length; i++)
			{
				if (commandLineArgs[i].Substring(0, 1) == "+")
				{
					flag = true;
				}
				else if (commandLineArgs[i].Substring(0, 1) == "-")
				{
					list.Add(commandLineArgs[i].Substring(1, commandLineArgs[i].Length - 1));
					flag = false;
				}
				else if (list.Count > 0 && !flag)
				{
					List<string> list2;
					int index;
					(list2 = list)[index = list.Count - 1] = list2[index] + " " + commandLineArgs[i];
				}
			}
			return list.ToArray();
		}

		public static GetCommands onGetCommands;
	}
}
