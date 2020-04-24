using System;
using SDG.Framework.Translations;
using SDG.Framework.UI.Components;

namespace SDG.Framework.UI.Sleek2
{
	public class Sleek2TranslatedLabel : Sleek2Label
	{
		public Sleek2TranslatedLabel()
		{
			base.name = "Label_Translated";
			this.translationComponent = base.gameObject.AddComponent<TranslatedLabel>();
			this.translationComponent.textComponent = base.textComponent;
		}

		public TranslatedText translation
		{
			get
			{
				return this.translationComponent.translation;
			}
			set
			{
				this.translationComponent.translation = value;
			}
		}

		protected TranslatedLabel translationComponent;
	}
}
