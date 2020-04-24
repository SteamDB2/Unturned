using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SDG.Framework.UI.Sleek2
{
	public class Sleek2ImageButton : Sleek2Image
	{
		public Sleek2ImageButton()
		{
			base.gameObject.name = "Button";
			base.imageComponent.sprite = Resources.Load<Sprite>("Sprites/UI/Button_Background");
			base.imageComponent.type = 1;
			this.buttonComponent = base.gameObject.AddComponent<Button>();
			this.buttonComponent.onClick.AddListener(new UnityAction(this.handleButtonClick));
		}

		public event ButtonClickedHandler clicked;

		public Button buttonComponent { get; protected set; }

		protected virtual void triggerClicked()
		{
			if (this.clicked != null)
			{
				this.clicked(this);
			}
		}

		protected virtual void handleButtonClick()
		{
			this.triggerClicked();
		}
	}
}
