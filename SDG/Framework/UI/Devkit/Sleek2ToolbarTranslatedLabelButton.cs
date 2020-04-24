using System;
using SDG.Framework.UI.Sleek2;
using UnityEngine;

namespace SDG.Framework.UI.Devkit
{
	public class Sleek2ToolbarTranslatedLabelButton : Sleek2ImageTranslatedLabelButton
	{
		public Sleek2ToolbarTranslatedLabelButton()
		{
			base.transform.sizeDelta = new Vector2(0f, (float)Sleek2Config.bodyHeight);
			base.imageComponent.sprite = Resources.Load<Sprite>("Sprites/UI/Hover_Background");
			base.label.textComponent.color = Sleek2Config.lightTextColor;
		}
	}
}
