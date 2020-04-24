using System;

namespace SDG.Framework.Debug
{
	public struct TerminalLogMessage
	{
		public TerminalLogCategory category;

		public long timestamp;

		public string internalText;

		public string displayText;
	}
}
