using System;

namespace SDG.Framework.Debug
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
	public class TerminalCommandMethodAttribute : Attribute
	{
		public TerminalCommandMethodAttribute(string newCommand, string newDescription)
		{
			this.command = newCommand;
			this.description = newDescription;
		}

		public string command;

		public string description;
	}
}
