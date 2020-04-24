using System;
using System.Collections.Generic;

namespace SDG.Framework.Debug
{
	public class TerminalLogCategory
	{
		public TerminalLogCategory(string newInternalName, string newDisplayName, bool defaultIsVisible = true)
		{
			this.isVisible = defaultIsVisible;
			this.internalName = newInternalName;
			this.displayName = newDisplayName;
			this.messages = new List<TerminalLogMessage>();
		}

		public bool isVisible { get; set; }

		public string internalName { get; protected set; }

		public string displayName { get; protected set; }

		public List<TerminalLogMessage> messages { get; protected set; }
	}
}
