using System;

namespace SDG.Framework.Debug
{
	[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
	public class TerminalCommandParameterAttribute : Attribute
	{
		public TerminalCommandParameterAttribute(string newName, string newDescription)
		{
			this.name = newName;
			this.description = newDescription;
		}

		public string name;

		public string description;
	}
}
