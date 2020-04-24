using System;

namespace SDG.Framework.Debug
{
	public delegate void TerminalCommandVariableChanged<T>(TerminalCommandVariable<T> variable, T oldValue, T newValue);
}
