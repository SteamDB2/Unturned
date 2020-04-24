using System;
using System.Collections.Generic;

namespace SDG.Framework.Debug
{
	public class Terminal
	{
		[TerminalCommandProperty("terminal.max_messages", "how many logs to hold per-category before deleting", 25)]
		public static uint maxMessages
		{
			get
			{
				return Terminal._maxMessages;
			}
			set
			{
				Terminal._maxMessages = value;
				TerminalUtility.printCommandPass("Set max_messages to: " + Terminal.maxMessages);
			}
		}

		[TerminalCommandProperty("terminal.pass_color", "hex color of success messages", "#00ff00")]
		public static string passColor
		{
			get
			{
				return Terminal._passColor;
			}
			set
			{
				Terminal._passColor = value;
				TerminalUtility.printCommandPass("Set pass_color to: " + Terminal.passColor);
			}
		}

		[TerminalCommandProperty("terminal.fail_color", "hex color of failure messages", "#ff0000")]
		public static string failColor
		{
			get
			{
				return Terminal._failColor;
			}
			set
			{
				Terminal._failColor = value;
				TerminalUtility.printCommandPass("Set fail_color to: " + Terminal.failColor);
			}
		}

		[TerminalCommandProperty("terminal.highlight_color", "hex color of match highlight", "#ffff00")]
		public static string highlightColor
		{
			get
			{
				return Terminal._highlightColor;
			}
			set
			{
				Terminal._highlightColor = value;
				TerminalUtility.printCommandPass("Set highlight_color to: " + Terminal.highlightColor);
			}
		}

		public static event TerminalCategoryVisibilityChanged onCategoryVisibilityChanged;

		public static event TerminalCategoriesCleared onCategoriesCleared;

		public static event TerminalCategoryCleared onCategoryCleared;

		public static event TerminalCategoryAdded onCategoryAdded;

		public static event TerminalMessageRemoved onMessageRemoved;

		public static event TerminalMessageAdded onMessageAdded;

		public static TerminalParameterParserRegistry parserRegistry { get; protected set; }

		public static IList<TerminalCommand> getCommands()
		{
			return Terminal.commands.Values;
		}

		public static IList<TerminalLogCategory> getLogs()
		{
			return Terminal.logs.Values;
		}

		public static void toggleCategoryVisibility(string internalCategory, bool newIsVisible)
		{
			TerminalLogCategory terminalLogCategory;
			if (!Terminal.logs.TryGetValue(internalCategory, out terminalLogCategory))
			{
				return;
			}
			terminalLogCategory.isVisible = newIsVisible;
			if (Terminal.onCategoryVisibilityChanged != null)
			{
				Terminal.onCategoryVisibilityChanged(terminalLogCategory);
			}
		}

		public static void clearAll()
		{
			Terminal.logs.Clear();
			if (Terminal.onCategoriesCleared != null)
			{
				Terminal.onCategoriesCleared();
			}
		}

		public static void clearCategory(string internalCategory)
		{
			TerminalLogCategory terminalLogCategory;
			if (!Terminal.logs.TryGetValue(internalCategory, out terminalLogCategory))
			{
				return;
			}
			terminalLogCategory.messages.Clear();
			if (Terminal.onCategoryCleared != null)
			{
				Terminal.onCategoryCleared(terminalLogCategory);
			}
		}

		public static void registerCommand(TerminalCommand command)
		{
			if (command == null || command.method == null || command.parameters == null)
			{
				return;
			}
			Terminal.commands.Add(command.method.command, command);
		}

		public static void print(string internalMessage, string displayMessage, string internalCategory, string displayCategory, bool defaultIsVisible = true)
		{
			if (displayMessage == null)
			{
				displayMessage = internalMessage;
			}
			if (displayCategory == null)
			{
				displayCategory = internalCategory;
			}
			TerminalLogCategory terminalLogCategory;
			if (!Terminal.logs.TryGetValue(internalCategory, out terminalLogCategory))
			{
				terminalLogCategory = new TerminalLogCategory(internalCategory, displayCategory, defaultIsVisible);
				Terminal.logs.Add(internalCategory, terminalLogCategory);
				if (Terminal.onCategoryAdded != null)
				{
					Terminal.onCategoryAdded(terminalLogCategory);
				}
			}
			TerminalLogMessage terminalLogMessage = default(TerminalLogMessage);
			terminalLogMessage.category = terminalLogCategory;
			terminalLogMessage.timestamp = DateTime.Now.Ticks;
			terminalLogMessage.internalText = internalMessage;
			terminalLogMessage.displayText = displayMessage;
			if ((long)terminalLogCategory.messages.Count >= (long)((ulong)Terminal.maxMessages))
			{
				TerminalLogMessage message = terminalLogCategory.messages[0];
				terminalLogCategory.messages.RemoveAtFast(0);
				if (Terminal.onMessageRemoved != null)
				{
					Terminal.onMessageRemoved(message, terminalLogCategory);
				}
			}
			terminalLogCategory.messages.Add(terminalLogMessage);
			if (Terminal.onMessageAdded != null)
			{
				Terminal.onMessageAdded(terminalLogMessage, terminalLogCategory);
			}
		}

		public static void initialize()
		{
			if (Terminal.parserRegistry == null)
			{
				Terminal.parserRegistry = new TerminalParameterParserRegistry();
			}
		}

		private static uint _maxMessages = 25u;

		private static string _passColor = "#00ff00";

		private static string _failColor = "#ff0000";

		private static string _highlightColor = "#ffff00";

		private static SortedList<string, TerminalCommand> commands = new SortedList<string, TerminalCommand>();

		private static SortedList<string, TerminalLogCategory> logs = new SortedList<string, TerminalLogCategory>();
	}
}
