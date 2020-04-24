using System;
using System.Runtime.CompilerServices;
using SDG.Framework.Debug;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	public class CommandWindow
	{
		public CommandWindow()
		{
			CommandWindow.input = new ConsoleInput();
			ConsoleInput input = CommandWindow.input;
			if (CommandWindow.<>f__mg$cache0 == null)
			{
				CommandWindow.<>f__mg$cache0 = new InputText(CommandWindow.onInputText);
			}
			input.onInputText = CommandWindow.<>f__mg$cache0;
			CommandWindow.output = new ConsoleOutput();
			if (CommandWindow.<>f__mg$cache1 == null)
			{
				CommandWindow.<>f__mg$cache1 = new Application.LogCallback(CommandWindow.onLogMessageReceived);
			}
			Application.logMessageReceived += CommandWindow.<>f__mg$cache1;
			Terminal.onMessageAdded += this.onMessageAdded;
		}

		public string title
		{
			get
			{
				return this._title;
			}
			set
			{
				this._title = value;
				if (CommandWindow.output != null)
				{
					CommandWindow.output.title = this.title;
				}
			}
		}

		public static ConsoleInput input { get; private set; }

		public static ConsoleOutput output { get; private set; }

		public static void Log(object text, ConsoleColor color)
		{
			if (CommandWindow.onCommandWindowOutputted != null)
			{
				CommandWindow.onCommandWindowOutputted(text, color);
			}
			if (CommandWindow.output == null)
			{
				Debug.Log(text);
				return;
			}
			Console.ForegroundColor = color;
			if (Console.CursorLeft != 0)
			{
				CommandWindow.input.clearLine();
			}
			Console.WriteLine(text);
			CommandWindow.input.redrawInputLine();
		}

		public static void Log(object text)
		{
			CommandWindow.Log(text, ConsoleColor.White);
		}

		public static void LogError(object text)
		{
			CommandWindow.Log(text, ConsoleColor.Red);
		}

		public static void LogWarning(object text)
		{
			CommandWindow.Log(text, ConsoleColor.Yellow);
		}

		private static void onLogMessageReceived(string text, string stack, LogType type)
		{
			if (type == 4)
			{
				CommandWindow.LogError(text + " - " + stack);
			}
		}

		private void onMessageAdded(TerminalLogMessage message, TerminalLogCategory category)
		{
			if (string.IsNullOrEmpty(message.internalText))
			{
				return;
			}
			CommandWindow.Log(category.internalName + ": " + message.internalText);
		}

		private static void onInputText(string command)
		{
			bool flag = true;
			if (CommandWindow.onCommandWindowInputted != null)
			{
				CommandWindow.onCommandWindowInputted(command, ref flag);
			}
			if (flag && !Commander.execute(CSteamID.Nil, command))
			{
				CommandWindow.LogError("?");
			}
		}

		public void update()
		{
			if (CommandWindow.input == null)
			{
				return;
			}
			CommandWindow.input.update();
		}

		public void shutdown()
		{
			if (CommandWindow.output == null)
			{
				return;
			}
			CommandWindow.output.shutdown();
		}

		public static CommandWindowInputted onCommandWindowInputted;

		public static CommandWindowOutputted onCommandWindowOutputted;

		private string _title;

		public static bool shouldLogChat = true;

		public static bool shouldLogJoinLeave = true;

		public static bool shouldLogDeaths = true;

		public static bool shouldLogAnticheat;

		[CompilerGenerated]
		private static InputText <>f__mg$cache0;

		[CompilerGenerated]
		private static Application.LogCallback <>f__mg$cache1;
	}
}
