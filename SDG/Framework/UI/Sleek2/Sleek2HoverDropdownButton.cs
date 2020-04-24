using System;
using SDG.Framework.UI.Components;
using UnityEngine;
using UnityEngine.UI;

namespace SDG.Framework.UI.Sleek2
{
	public class Sleek2HoverDropdownButton : Sleek2Element
	{
		public Sleek2HoverDropdownButton()
		{
			base.gameObject.name = "Hover_Dropdown_Button";
			this.imageComponent = base.gameObject.AddComponent<Image>();
			this.imageComponent.sprite = Resources.Load<Sprite>("Sprites/UI/Hover_Background");
			this.imageComponent.type = 1;
			this.panel = new Sleek2HoverDropdown();
			this.addElement(this.panel);
			base.gameObject.AddComponent<Selectable>();
			this.dropdown = base.gameObject.AddComponent<HoverDropdownButton>();
			this.dropdown.dropdown = this.panel.transform;
		}

		public Image imageComponent { get; protected set; }

		public Sleek2HoverDropdown panel { get; protected set; }

		protected HoverDropdownButton dropdown;
	}
}
