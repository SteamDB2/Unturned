using System;
using System.Reflection;

namespace SDG.Framework.Debug
{
	public class TerminalCommandMethodInfo
	{
		public TerminalCommandMethodInfo(string newCommand, string newDescription, MethodInfo newInfo)
		{
			this.command = newCommand;
			this.description = newDescription;
			this.info = newInfo;
		}

		public string command { get; protected set; }

		public string description { get; protected set; }

		public MethodInfo info { get; protected set; }
	}
}
