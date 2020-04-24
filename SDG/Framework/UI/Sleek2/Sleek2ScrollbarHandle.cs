using System;
using UnityEngine;
using UnityEngine.UI;

namespace SDG.Framework.UI.Sleek2
{
	public class Sleek2ScrollbarHandle : Sleek2Element
	{
		public Sleek2ScrollbarHandle()
		{
			base.gameObject.name = "Handle";
			this.imageComponent = base.gameObject.AddComponent<Image>();
			this.imageComponent.sprite = Resources.Load<Sprite>("Sprites/UI/Button_Background");
			this.imageComponent.type = 1;
		}

		public Image imageComponent { get; protected set; }
	}
}
