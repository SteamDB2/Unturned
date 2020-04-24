using System;

namespace SDG.Framework.Translations
{
	public class TranslationLeaf
	{
		public TranslationLeaf(Translation newTranslation, TranslationBranch newBranch)
		{
			this.translation = newTranslation;
			this.branch = newBranch;
		}

		public Translation translation { get; protected set; }

		public TranslationBranch branch { get; protected set; }

		public TranslationReference getReferenceTo()
		{
			return this.branch.getReferenceTo();
		}

		public string text
		{
			get
			{
				return this._text;
			}
			set
			{
				if (this.text == value)
				{
					return;
				}
				string text = this.text;
				this._text = value;
				this.triggerTextChanged(text, this.text);
			}
		}

		public int version
		{
			get
			{
				return this._version;
			}
			set
			{
				if (this.version == value)
				{
					return;
				}
				int version = this.version;
				this._version = value;
				this.triggerVersionChanged(version, this.version);
			}
		}

		public event TranslationLeafTextChangedHandler textChanged;

		public event TranslationLeafVersionChangedHandler versionChanged;

		protected void triggerTextChanged(string oldText, string newText)
		{
			if (this.textChanged != null)
			{
				this.textChanged(this, oldText, newText);
			}
		}

		protected void triggerVersionChanged(int oldVersion, int newVersion)
		{
			if (this.versionChanged != null)
			{
				this.versionChanged(this, oldVersion, newVersion);
			}
		}

		protected string _text;

		protected int _version;
	}
}
