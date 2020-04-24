using System;

namespace SDG.Framework.Translations
{
	public class TranslatedTextArg0Container : ITranslatedTextArgContainer
	{
		public TranslatedTextArg0Container(object newArg0)
		{
			this.arg0 = newArg0;
		}

		public string format(string text)
		{
			return string.Format(text, this.arg0);
		}

		public object arg0;
	}
}
