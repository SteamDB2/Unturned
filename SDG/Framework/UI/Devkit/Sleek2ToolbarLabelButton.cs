using System;
using SDG.Framework.UI.Sleek2;
using UnityEngine;

namespace SDG.Framework.UI.Devkit
{
	public class Sleek2ToolbarLabelButton : Sleek2ImageLabelButton
	{
		public Sleek2ToolbarLabelButton()
		{
			base.transform.sizeDelta = new Vector2(0f, (float)Sleek2Config.bodyHeight);
			base.imageComponent.sprite = Resources.Load<Sprite>("Sprites/UI/Hover_Background");
			base.label.textComponent.color = Sleek2Config.lightTextColor;
		}
	}
}
