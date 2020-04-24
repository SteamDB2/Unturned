using System;
using SDG.Framework.UI.Sleek2;
using UnityEngine;

namespace SDG.Framework.UI.Devkit
{
	public class DevkitToolbarWindowButton : Sleek2ImageButton
	{
		public DevkitToolbarWindowButton(Type type)
		{
			this.type = type;
			base.transform.sizeDelta = new Vector2(0f, (float)Sleek2Config.bodyHeight);
			base.imageComponent.sprite = Resources.Load<Sprite>("Sprites/UI/Hover_Background");
			this.label = new Sleek2Label();
			this.label.transform.reset();
			this.label.textComponent.text = type.Name;
			this.addElement(this.label);
		}

		public Sleek2Label label { get; protected set; }

		protected override void handleButtonClick()
		{
			Sleek2Window window = Activator.CreateInstance(this.type) as Sleek2Window;
			DevkitWindowManager.addWindow(window);
			base.handleButtonClick();
		}

		protected Type type;
	}
}
