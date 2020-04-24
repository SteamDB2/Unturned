using System;

namespace SDG.Framework.Translations
{
	public class TranslatedTextFallback : TranslatedText
	{
		public TranslatedTextFallback(string newFallback) : base(default(TranslationReference))
		{
			this.fallback = newFallback;
		}

		public override string format()
		{
			if (this.args != null)
			{
				this.text = this.args.format(this.fallback);
			}
			else
			{
				this.text = this.fallback;
			}
			this.triggerTranslatedTextChanged();
			return this.text;
		}

		public override void beginListening()
		{
		}

		public override void endListening()
		{
		}

		protected string fallback;
	}
}
