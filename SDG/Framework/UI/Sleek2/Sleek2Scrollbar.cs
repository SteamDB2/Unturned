using System;
using UnityEngine;
using UnityEngine.UI;

namespace SDG.Framework.UI.Sleek2
{
	public class Sleek2Scrollbar : Sleek2Element
	{
		public Sleek2Scrollbar()
		{
			base.gameObject.name = "Scrollbar";
			this.imageComponent = base.gameObject.AddComponent<Image>();
			this.imageComponent.sprite = Resources.Load<Sprite>("Sprites/UI/Button_Background");
			this.imageComponent.type = 1;
			this.scrollbarComponent = base.gameObject.AddComponent<Scrollbar>();
			this.scrollbarComponent.direction = 2;
			this.handle = new Sleek2ScrollbarHandle();
			this.addElement(this.handle);
			this.handle.transform.reset();
			this.scrollbarComponent.handleRect = this.handle.transform;
		}

		public Image imageComponent { get; protected set; }

		public Scrollbar scrollbarComponent { get; protected set; }

		public Sleek2ScrollbarHandle handle { get; protected set; }
	}
}
