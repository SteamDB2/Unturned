using System;

namespace SDG.Framework.Translations
{
	public class TranslatedTextArg0Arg1Container : ITranslatedTextArgContainer
	{
		public TranslatedTextArg0Arg1Container(object newArg0, object newArg1)
		{
			this.arg0 = newArg0;
			this.arg1 = newArg1;
		}

		public string format(string text)
		{
			return string.Format(text, this.arg0, this.arg1);
		}

		public object arg0;

		public object arg1;
	}
}
