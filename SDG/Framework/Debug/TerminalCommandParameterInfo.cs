using System;

namespace SDG.Framework.Debug
{
	public class TerminalCommandParameterInfo
	{
		public TerminalCommandParameterInfo(string newName, string newDescription, Type newType, object newDefaultValue)
		{
			this.name = newName;
			this.description = newDescription;
			this.type = newType;
			this.defaultValue = newDefaultValue;
		}

		public string name { get; protected set; }

		public string description { get; protected set; }

		public Type type { get; protected set; }

		public object defaultValue { get; protected set; }
	}
}
