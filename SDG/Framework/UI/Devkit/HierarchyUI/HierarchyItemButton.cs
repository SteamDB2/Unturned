using System;
using SDG.Framework.Devkit;
using SDG.Framework.Translations;
using SDG.Framework.UI.Sleek2;
using UnityEngine;

namespace SDG.Framework.UI.Devkit.HierarchyUI
{
	public class HierarchyItemButton : Sleek2ImageButton
	{
		public HierarchyItemButton(IDevkitHierarchyItem newItem)
		{
			this.item = newItem;
			base.transform.anchorMin = new Vector2(0f, 1f);
			base.transform.anchorMax = new Vector2(1f, 1f);
			base.transform.pivot = new Vector2(0.5f, 1f);
			base.transform.sizeDelta = new Vector2(0f, 30f);
			Sleek2TranslatedLabel sleek2TranslatedLabel = new Sleek2TranslatedLabel();
			sleek2TranslatedLabel.transform.reset();
			if (this.item is Object)
			{
				sleek2TranslatedLabel.translation = new TranslatedTextFallback((this.item as Object).name);
			}
			else
			{
				sleek2TranslatedLabel.translation = this.item.GetType().getTranslatedNameText();
			}
			sleek2TranslatedLabel.translation.format();
			sleek2TranslatedLabel.textComponent.color = Sleek2Config.darkTextColor;
			this.addElement(sleek2TranslatedLabel);
		}

		public IDevkitHierarchyItem item { get; protected set; }
	}
}
