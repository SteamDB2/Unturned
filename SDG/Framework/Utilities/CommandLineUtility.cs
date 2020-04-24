using System;
using System.Collections.Generic;
using SDG.Framework.Debug;

namespace SDG.Framework.Utilities
{
	public class CommandLineUtility
	{
		public static List<List<string>> commands
		{
			get
			{
				if (CommandLineUtility._commands == null)
				{
					CommandLineUtility._commands = new List<List<string>>();
					string[] array = Environment.CommandLine.Split(CommandLineUtility.DELIMITERS);
					if (array.Length > 1)
					{
						for (int i = 1; i < array.Length; i++)
						{
							if (!string.IsNullOrEmpty(array[i]))
							{
								string input = array[i].Trim();
								List<string> collection = TerminalUtility.splitArguments(input);
								CommandLineUtility.commands.Add(new List<string>(collection));
							}
						}
					}
				}
				return CommandLineUtility._commands;
			}
		}

		protected static readonly char[] DELIMITERS = new char[]
		{
			'-',
			'+'
		};

		protected static List<List<string>> _commands;
	}
}
