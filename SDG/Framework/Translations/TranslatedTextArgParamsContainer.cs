using System;

namespace SDG.Framework.Translations
{
	public class TranslatedTextArgParamsContainer : ITranslatedTextArgContainer
	{
		public TranslatedTextArgParamsContainer(object[] newArgs)
		{
			this.args = newArgs;
		}

		public string format(string text)
		{
			return string.Format(text, this.args);
		}

		public object[] args;
	}
}
