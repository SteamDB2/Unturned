using System;
using System.Collections.Generic;

namespace SDG.Framework.Debug
{
	public class TerminalLogMessageTimestampComparer : IComparer<TerminalLogMessage>
	{
		public int Compare(TerminalLogMessage x, TerminalLogMessage y)
		{
			return x.timestamp.CompareTo(y.timestamp);
		}
	}
}
