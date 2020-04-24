using System;
using System.Reflection;

namespace SDG.Framework.Debug
{
	public class TerminalCommand
	{
		public TerminalCommand(TerminalCommandMethodInfo newMethod, TerminalCommandParameterInfo[] newParameters, MethodInfo newCurrentValue = null)
		{
			this.method = newMethod;
			this.parameters = newParameters;
			this.currentValue = newCurrentValue;
		}

		public TerminalCommandMethodInfo method { get; protected set; }

		public TerminalCommandParameterInfo[] parameters { get; protected set; }

		public MethodInfo currentValue { get; protected set; }
	}
}
