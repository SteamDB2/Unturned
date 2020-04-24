using System;
using System.Collections.Generic;
using System.Text;
using SDG.Framework.Utilities;

namespace SDG.Framework.Debug
{
	public class TerminalUtility
	{
		public static List<TerminalCommand> filterCommands(string input)
		{
			TerminalUtility.filteredCommands.Clear();
			if (!string.IsNullOrEmpty(input))
			{
				IList<TerminalCommand> commands = Terminal.getCommands();
				for (int i = 0; i < commands.Count; i++)
				{
					TerminalCommand terminalCommand = commands[i];
					if (terminalCommand.method.command.Length >= input.Length)
					{
						bool flag = true;
						for (int j = 0; j < input.Length; j++)
						{
							if (terminalCommand.method.command[j] != input[j])
							{
								flag = false;
								break;
							}
						}
						if (flag)
						{
							TerminalUtility.filteredCommands.Add(terminalCommand);
						}
					}
				}
			}
			return TerminalUtility.filteredCommands;
		}

		public static List<string> splitArguments(string input)
		{
			TerminalUtility.commandArguments.Clear();
			if (!string.IsNullOrEmpty(input))
			{
				int num = 0;
				bool flag = false;
				bool flag2 = false;
				bool flag3 = false;
				for (int i = 0; i < input.Length; i++)
				{
					if (input[i] == '"')
					{
						flag = !flag;
						if (!flag)
						{
							flag2 = true;
						}
					}
					else if (input[i] == ' ' && !flag)
					{
						if (i > num)
						{
							if (flag2)
							{
								if (i > num + 2)
								{
									string item = input.Substring(num + 1, i - num - 2);
									TerminalUtility.commandArguments.Add(item);
								}
								flag2 = false;
							}
							else
							{
								string item2 = input.Substring(num, i - num);
								TerminalUtility.commandArguments.Add(item2);
							}
						}
						num = i + 1;
						if (i == input.Length - 1)
						{
							flag3 = true;
						}
					}
				}
				if (flag2)
				{
					if (input.Length - num > 2)
					{
						TerminalUtility.commandArguments.Add(input.Substring(num + 1, input.Length - num - 2));
					}
				}
				else if (input.Length - num > 0)
				{
					TerminalUtility.commandArguments.Add(input.Substring(num, input.Length - num));
				}
				if (flag3)
				{
					TerminalUtility.commandArguments.Add(string.Empty);
				}
			}
			return TerminalUtility.commandArguments;
		}

		public static void execute(string input, List<string> arguments = null, List<TerminalCommand> commands = null)
		{
			if (arguments == null)
			{
				if (string.IsNullOrEmpty(input))
				{
					return;
				}
				arguments = TerminalUtility.splitArguments(input);
			}
			if (commands == null && arguments.Count >= 1)
			{
				if (string.IsNullOrEmpty(input))
				{
					return;
				}
				commands = TerminalUtility.filterCommands(arguments[0]);
			}
			if (commands.Count == 1)
			{
				TerminalCommand terminalCommand = commands[0];
				if (arguments.Count == terminalCommand.parameters.Length + 1)
				{
					bool flag = true;
					object[] array = new object[terminalCommand.parameters.Length];
					for (int i = 0; i < array.Length; i++)
					{
						array[i] = Terminal.parserRegistry.parse(terminalCommand.parameters[i].type, arguments[i + 1]);
						if (array[i] == null)
						{
							TerminalUtility.printCommandFail(string.Concat(new string[]
							{
								"Unable to parse \"",
								arguments[i + 1],
								"\" as ",
								terminalCommand.parameters[i].type.Name.ToString().ToLower(),
								"!"
							}));
							flag = false;
							break;
						}
					}
					if (flag)
					{
						terminalCommand.method.info.Invoke(null, array);
					}
				}
				else
				{
					TerminalUtility.printCommandFail(string.Concat(new object[]
					{
						"Expected ",
						terminalCommand.parameters.Length,
						" argument(s), got ",
						arguments.Count - 1,
						"!"
					}));
				}
			}
			else
			{
				TerminalUtility.printCommandFail(string.Concat(new object[]
				{
					"Unable to determine intention of \"",
					input,
					"\" out of ",
					commands.Count,
					" commands!"
				}));
			}
		}

		public static void printCommandPass(string message)
		{
			StringBuilder instance = StringBuilderUtility.instance;
			instance.Append("<color=");
			instance.Append(Terminal.passColor);
			instance.Append(">");
			instance.Append(message);
			instance.Append("</color>");
			string displayMessage = instance.ToString();
			StringBuilder instance2 = StringBuilderUtility.instance;
			instance2.Append("<color=");
			instance2.Append(Terminal.passColor);
			instance2.Append(">Commands");
			instance2.Append("</color>");
			string displayCategory = instance2.ToString();
			Terminal.print(message, displayMessage, "Commands", displayCategory, true);
		}

		public static void printCommandFail(string message)
		{
			StringBuilder instance = StringBuilderUtility.instance;
			instance.Append("<color=");
			instance.Append(Terminal.failColor);
			instance.Append(">");
			instance.Append(message);
			instance.Append("</color>");
			string displayMessage = instance.ToString();
			StringBuilder instance2 = StringBuilderUtility.instance;
			instance2.Append("<color=");
			instance2.Append(Terminal.passColor);
			instance2.Append(">Commands");
			instance2.Append("</color>");
			string displayCategory = instance2.ToString();
			Terminal.print(message, displayMessage, "Commands", displayCategory, true);
		}

		private static List<TerminalCommand> filteredCommands = new List<TerminalCommand>();

		private static List<string> commandArguments = new List<string>();
	}
}
