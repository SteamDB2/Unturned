using System;
using UnityEngine;

namespace SDG.Framework.UI.Sleek2
{
	public class Sleek2ImageLabelButton : Sleek2ImageButton
	{
		public Sleek2ImageLabelButton()
		{
			this.label = new Sleek2Label();
			this.label.transform.reset();
			this.label.textComponent.color = Sleek2Config.darkTextColor;
			this.addElement(this.label);
		}

		public Sleek2Label label { get; protected set; }
	}
}
