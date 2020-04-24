using System;

namespace SDG.Framework.Debug
{
	public interface ITerminalParameterParser
	{
		object parse(string input);
	}
}
