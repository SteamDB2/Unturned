using System;
using SDG.Framework.Translations;
using UnityEngine;
using UnityEngine.UI;

namespace SDG.Framework.UI.Components
{
	public class TranslatedLabel : MonoBehaviour
	{
		public TranslatedText translation
		{
			get
			{
				return this._translation;
			}
			set
			{
				if (this.translation == value)
				{
					return;
				}
				if (this.translation != null)
				{
					this.translation.changed -= this.handleTranslationChanged;
					this.translation.endListening();
				}
				this._translation = value;
				if (this.translation != null)
				{
					this.translation.changed += this.handleTranslationChanged;
				}
			}
		}

		protected virtual void handleTranslationChanged(TranslatedText translation, string text)
		{
			if (this.textComponent == null)
			{
				return;
			}
			this.textComponent.text = text;
		}

		protected void OnDestroy()
		{
			this.translation = null;
		}

		public Text textComponent;

		protected TranslatedText _translation;
	}
}
