using System;
using UnityEngine;
using UnityEngine.UI;

namespace SDG.Framework.UI.Sleek2
{
	public class Sleek2Container : Sleek2Element
	{
		public Sleek2Container()
		{
			base.name = "Container";
			Sleek2Image sleek2Image = new Sleek2Image();
			sleek2Image.name = "Header";
			sleek2Image.transform.anchorMin = new Vector2(0f, 1f);
			sleek2Image.transform.anchorMax = new Vector2(1f, 1f);
			sleek2Image.transform.pivot = new Vector2(0f, 1f);
			sleek2Image.transform.sizeDelta = new Vector2(0f, (float)Sleek2Config.bodyHeight);
			sleek2Image.imageComponent.sprite = Resources.Load<Sprite>("Sprites/UI/Toolbar_Background");
			sleek2Image.imageComponent.type = 1;
			this.headerPanel = sleek2Image;
			this.addElement(sleek2Image);
			Sleek2Mask sleek2Mask = new Sleek2Mask();
			sleek2Mask.name = "Body";
			sleek2Mask.transform.anchorMin = new Vector2(0f, 0f);
			sleek2Mask.transform.anchorMax = new Vector2(1f, 1f);
			sleek2Mask.transform.pivot = new Vector2(0f, 1f);
			sleek2Mask.transform.offsetMin = new Vector2(0f, 0f);
			sleek2Mask.transform.offsetMax = new Vector2(0f, (float)(-(float)Sleek2Config.bodyHeight));
			this.bodyPanel = sleek2Mask;
			this.addElement(sleek2Mask);
			this.backgroundImageComponent = base.gameObject.AddComponent<Image>();
			this.backgroundImageComponent.sprite = Resources.Load<Sprite>("Sprites/UI/Background");
			this.backgroundImageComponent.type = 1;
		}

		public Sleek2Element headerPanel { get; protected set; }

		public Sleek2Element bodyPanel { get; protected set; }

		public Image backgroundImageComponent { get; protected set; }
	}
}
