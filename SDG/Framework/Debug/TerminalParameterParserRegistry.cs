using System;
using System.Collections.Generic;

namespace SDG.Framework.Debug
{
	public class TerminalParameterParserRegistry
	{
		public object parse(Type type, string input)
		{
			ITerminalParameterParser terminalParameterParser;
			if (this.parsers.TryGetValue(type, out terminalParameterParser))
			{
				return terminalParameterParser.parse(input);
			}
			return null;
		}

		public void add(Type type, ITerminalParameterParser parser)
		{
			this.parsers.Add(type, parser);
		}

		public void remove(Type type)
		{
			this.parsers.Remove(type);
		}

		private Dictionary<Type, ITerminalParameterParser> parsers = new Dictionary<Type, ITerminalParameterParser>();
	}
}
