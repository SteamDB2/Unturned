using System;

namespace SDG.Framework.Translations
{
	public class TranslatedText
	{
		public TranslatedText(TranslationReference newReference)
		{
			this.args = null;
			this.text = null;
			this._reference = newReference;
			this.beginListening();
		}

		public TranslationReference reference
		{
			get
			{
				return this._reference;
			}
			set
			{
				if (this.reference == value)
				{
					return;
				}
				this.endListening();
				this._reference = value;
				this.beginListening();
				this.format();
				this.triggerTranslatedTextChanged();
			}
		}

		public event TranslatedTextChangedHandler changed;

		public virtual string format()
		{
			if (this.args != null)
			{
				this.text = this.args.format(Translator.translate(this.reference));
			}
			else
			{
				this.text = Translator.translate(this.reference);
			}
			this.triggerTranslatedTextChanged();
			return this.text;
		}

		public virtual string format(object arg0)
		{
			if (this.args == null)
			{
				this.args = new TranslatedTextArg0Container(arg0);
			}
			else
			{
				TranslatedTextArg0Container translatedTextArg0Container = this.args as TranslatedTextArg0Container;
				translatedTextArg0Container.arg0 = arg0;
			}
			return this.format();
		}

		public virtual string format(object arg0, object arg1)
		{
			if (this.args == null)
			{
				this.args = new TranslatedTextArg0Arg1Container(arg0, arg1);
			}
			else
			{
				TranslatedTextArg0Arg1Container translatedTextArg0Arg1Container = this.args as TranslatedTextArg0Arg1Container;
				translatedTextArg0Arg1Container.arg0 = arg0;
				translatedTextArg0Arg1Container.arg1 = arg1;
			}
			return this.format();
		}

		public virtual string format(object arg0, object arg1, object arg2)
		{
			if (this.args == null)
			{
				this.args = new TranslatedTextArg0Arg1Arg2Container(arg0, arg1, arg2);
			}
			else
			{
				TranslatedTextArg0Arg1Arg2Container translatedTextArg0Arg1Arg2Container = this.args as TranslatedTextArg0Arg1Arg2Container;
				translatedTextArg0Arg1Arg2Container.arg0 = arg0;
				translatedTextArg0Arg1Arg2Container.arg1 = arg1;
				translatedTextArg0Arg1Arg2Container.arg2 = arg2;
			}
			return this.format();
		}

		public virtual string format(params object[] args)
		{
			if (this.args == null)
			{
				this.args = new TranslatedTextArgParamsContainer(args);
			}
			else
			{
				TranslatedTextArgParamsContainer translatedTextArgParamsContainer = this.args as TranslatedTextArgParamsContainer;
				translatedTextArgParamsContainer.args = args;
			}
			return this.format();
		}

		protected virtual void handleLeafTextChanged(TranslationLeaf leaf, string oldText, string newText)
		{
			this.format();
		}

		protected virtual void handleLanguageChanged(string oldLanguage, string newLanguage)
		{
			TranslationLeaf leaf = Translator.getLeaf(oldLanguage, this.reference);
			if (leaf != null)
			{
				leaf.textChanged -= this.handleLeafTextChanged;
			}
			TranslationLeaf leaf2 = Translator.getLeaf(newLanguage, this.reference);
			if (leaf2 != null)
			{
				leaf2.textChanged += this.handleLeafTextChanged;
			}
			this.format();
		}

		protected virtual void triggerTranslatedTextChanged()
		{
			if (this.changed != null)
			{
				this.changed(this, this.text);
			}
		}

		public override string ToString()
		{
			return this.text;
		}

		public virtual void beginListening()
		{
			if (this.isListening)
			{
				return;
			}
			this.isListening = true;
			Translator.languageChanged += this.handleLanguageChanged;
			TranslationLeaf leaf = Translator.getLeaf(this.reference);
			if (leaf != null)
			{
				leaf.textChanged += this.handleLeafTextChanged;
			}
		}

		public virtual void endListening()
		{
			if (!this.isListening)
			{
				return;
			}
			this.isListening = false;
			Translator.languageChanged -= this.handleLanguageChanged;
			TranslationLeaf leaf = Translator.getLeaf(this.reference);
			if (leaf != null)
			{
				leaf.textChanged -= this.handleLeafTextChanged;
			}
		}

		protected TranslationReference _reference;

		protected bool isListening;

		protected string text;

		protected ITranslatedTextArgContainer args;
	}
}
