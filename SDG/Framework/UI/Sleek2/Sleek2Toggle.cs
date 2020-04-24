using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SDG.Framework.UI.Sleek2
{
	public class Sleek2Toggle : Sleek2Image
	{
		public Sleek2Toggle()
		{
			base.gameObject.name = "Toggle";
			base.imageComponent.sprite = Resources.Load<Sprite>("Sprites/UI/Button_Background");
			base.imageComponent.type = 1;
			this.toggleComponent = base.gameObject.AddComponent<Toggle>();
			this.toggleComponent.onValueChanged.AddListener(new UnityAction<bool>(this.handleToggleValueChanged));
			this.checkmark = new Sleek2Image();
			this.checkmark.name = "Checkmark";
			this.checkmark.transform.anchorMin = new Vector2(0f, 0f);
			this.checkmark.transform.anchorMax = new Vector2(1f, 1f);
			this.checkmark.transform.offsetMin = new Vector2(5f, 5f);
			this.checkmark.transform.offsetMax = new Vector2(-5f, -5f);
			this.checkmark.imageComponent.sprite = Resources.Load<Sprite>("Sprites/UI/Checkmark");
			this.checkmark.imageComponent.color = Sleek2Config.darkTextColor;
			this.toggleComponent.graphic = this.checkmark.imageComponent;
			this.addElement(this.checkmark);
		}

		public event ToggleToggledHandler toggled;

		public Toggle toggleComponent { get; protected set; }

		public Sleek2Image checkmark { get; protected set; }

		protected virtual void triggerToggled(bool isOn)
		{
			if (this.toggled != null)
			{
				this.toggled(this, isOn);
			}
		}

		protected virtual void handleToggleValueChanged(bool isOn)
		{
			this.triggerToggled(isOn);
		}
	}
}
