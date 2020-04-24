using System;
using SDG.Framework.Translations;
using SDG.Framework.UI.Sleek2;
using UnityEngine.UI;

namespace SDG.Framework.UI.Devkit.TranslationUI
{
	public class TranslationNamespaceButton : Sleek2ImageLabelButton
	{
		public TranslationNamespaceButton(Translation newTranslation)
		{
			base.name = "Namespace";
			this.translation = newTranslation;
			base.label.textComponent.text = this.translation.ns;
			this.layoutComponent = base.gameObject.AddComponent<LayoutElement>();
			this.layoutComponent.preferredHeight = (float)Sleek2Config.bodyHeight;
		}

		public Translation translation { get; protected set; }

		public LayoutElement layoutComponent { get; protected set; }
	}
}
