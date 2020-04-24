using System;
using SDG.Framework.UI.Components;
using UnityEngine;

namespace SDG.Framework.UI.Sleek2
{
	public class Sleek2Titlebar : Sleek2Element
	{
		public Sleek2Titlebar()
		{
			base.name = "Titlebar";
			base.transform.reset();
			this.titleLabel = new Sleek2TranslatedLabel();
			this.titleLabel.transform.reset();
			this.titleLabel.transform.offsetMin = new Vector2(5f, 0f);
			this.titleLabel.transform.offsetMax = new Vector2((float)(-5 - Sleek2Config.bodyHeight), 0f);
			this.titleLabel.textComponent.alignment = 3;
			this.addElement(this.titleLabel);
			this.exitButton = new Sleek2ImageButton();
			this.exitButton.transform.anchorMin = Vector2.one;
			this.exitButton.transform.anchorMax = Vector2.one;
			this.exitButton.transform.pivot = Vector2.one;
			this.exitButton.transform.sizeDelta = new Vector2((float)Sleek2Config.bodyHeight, (float)Sleek2Config.bodyHeight);
			this.exitButton.imageComponent.sprite = Resources.Load<Sprite>("Sprites/UI/Exit");
			this.addElement(this.exitButton);
			this.dragableComponent = base.gameObject.AddComponent<DragablePopoutContainer>();
		}

		public Sleek2TranslatedLabel titleLabel { get; protected set; }

		public Sleek2ImageButton exitButton { get; protected set; }

		public DragablePopoutContainer dragableComponent { get; protected set; }
	}
}
