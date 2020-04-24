using System;
using SDG.Framework.UI.Sleek2;
using UnityEngine;

namespace SDG.Framework.UI.Devkit
{
	public class DevkitToolbarContainerButton : Sleek2ImageButton
	{
		public DevkitToolbarContainerButton(Type type)
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
			Sleek2PopoutContainer sleek2PopoutContainer = DevkitWindowManager.addContainer(this.type);
			sleek2PopoutContainer.transform.anchorMin = new Vector2(0.25f, 0.25f);
			sleek2PopoutContainer.transform.anchorMax = new Vector2(0.75f, 0.75f);
			base.handleButtonClick();
		}

		protected Type type;
	}
}
