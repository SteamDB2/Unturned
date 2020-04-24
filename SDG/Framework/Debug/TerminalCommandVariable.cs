using System;
using SDG.Framework.Translations;

namespace SDG.Framework.Debug
{
	public class TerminalCommandVariable<T>
	{
		public TerminalCommandVariable(T defaultValue, TranslatedText changeText)
		{
			this.value = defaultValue;
			this.text = changeText;
		}

		public event TerminalCommandVariableChanged<T> changed;

		public T getValue()
		{
			return this.value;
		}

		public void setValue(T newValue, bool print = false)
		{
			T oldValue = this.value;
			this.value = newValue;
			if (print)
			{
				TerminalUtility.printCommandPass(this.text.format(this.value));
			}
			this.triggerChanged(oldValue, this.value);
		}

		protected virtual void triggerChanged(T oldValue, T newValue)
		{
			if (this.changed != null)
			{
				this.changed(this, oldValue, newValue);
			}
		}

		protected T value;

		protected TranslatedText text;
	}
}
