using System;
using UnityEngine;

namespace SDG.Framework.UI.Sleek2
{
	public class Sleek2DropdownButtonTemplate : Sleek2ImageButton
	{
		public Sleek2DropdownButtonTemplate()
		{
			base.transform.sizeDelta = new Vector2(0f, (float)Sleek2Config.bodyHeight);
			base.imageComponent.sprite = Resources.Load<Sprite>("Sprites/UI/Hover_Background");
			this.label = new Sleek2Label();
			this.label.transform.reset();
			this.addElement(this.label);
		}

		public Sleek2Label label { get; protected set; }
	}
}
