using System;
using SDG.Framework.UI.Sleek2;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.UI.Devkit
{
	public class DevkitToolbarHelpButton : Sleek2ImageButton
	{
		public DevkitToolbarHelpButton(string text, string url)
		{
			this.url = url;
			base.transform.sizeDelta = new Vector2(0f, (float)Sleek2Config.bodyHeight);
			base.imageComponent.sprite = Resources.Load<Sprite>("Sprites/UI/Hover_Background");
			this.label = new Sleek2Label();
			this.label.transform.reset();
			this.label.textComponent.text = text;
			this.addElement(this.label);
		}

		public Sleek2Label label { get; protected set; }

		protected override void handleButtonClick()
		{
			Provider.provider.browserService.open(this.url);
			Debug.Log("URL: " + this.url);
			base.handleButtonClick();
		}

		protected string url;
	}
}
