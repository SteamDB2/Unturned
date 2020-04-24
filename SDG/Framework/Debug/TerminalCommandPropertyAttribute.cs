using System;

namespace SDG.Framework.Debug
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public class TerminalCommandPropertyAttribute : Attribute
	{
		public TerminalCommandPropertyAttribute(string newCommand, string newDescription, object newDefaultValue)
		{
			this.command = newCommand;
			this.description = newDescription;
			this.defaultValue = newDefaultValue;
		}

		public string command;

		public string description;

		public object defaultValue;
	}
}
