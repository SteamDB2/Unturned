using System;
using SDG.Framework.UI.Components;
using UnityEngine;
using UnityEngine.UI;

namespace SDG.Framework.UI.Sleek2
{
	public class Sleek2WindowTabDock : Sleek2Element
	{
		public Sleek2WindowTabDock()
		{
			base.name = "Tab_Dock";
			base.transform.anchorMin = new Vector2(0f, 1f);
			base.transform.anchorMax = new Vector2(1f, 1f);
			base.transform.pivot = new Vector2(0f, 1f);
			base.transform.sizeDelta = new Vector2(0f, (float)Sleek2Config.bodyHeight);
			this.imageComponent = base.gameObject.AddComponent<Image>();
			this.imageComponent.sprite = Resources.Load<Sprite>("Sprites/UI/Background");
			this.imageComponent.type = 1;
			this.destination = base.gameObject.AddComponent<DragableTabDestination>();
		}

		public Image imageComponent { get; protected set; }

		public DragableTabDestination destination { get; protected set; }
	}
}
