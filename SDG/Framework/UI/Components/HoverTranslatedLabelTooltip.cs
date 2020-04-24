using System;
using SDG.Framework.Translations;
using SDG.Framework.UI.Sleek2;
using UnityEngine;

namespace SDG.Framework.UI.Components
{
	public class HoverTranslatedLabelTooltip : HoverTooltip
	{
		protected override Sleek2Element triggerBeginTooltip()
		{
			Sleek2TranslatedLabel sleek2TranslatedLabel = new Sleek2TranslatedLabel();
			sleek2TranslatedLabel.transform.anchorMin = Vector2.zero;
			sleek2TranslatedLabel.transform.anchorMax = Vector2.zero;
			sleek2TranslatedLabel.transform.pivot = new Vector2(0f, 1f);
			sleek2TranslatedLabel.transform.sizeDelta = new Vector2(200f, 50f);
			sleek2TranslatedLabel.textComponent.alignment = 0;
			sleek2TranslatedLabel.translation = this.translation;
			sleek2TranslatedLabel.translation.format();
			DevkitCanvas.tooltip = sleek2TranslatedLabel;
			return sleek2TranslatedLabel;
		}

		protected override void triggerEndTooltip(Sleek2Element element)
		{
			Object.Destroy(element.gameObject);
			DevkitCanvas.tooltip = null;
		}

		public TranslatedText translation;
	}
}
