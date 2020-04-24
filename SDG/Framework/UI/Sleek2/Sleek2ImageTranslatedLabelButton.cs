using System;
using UnityEngine;

namespace SDG.Framework.UI.Sleek2
{
	public class Sleek2ImageTranslatedLabelButton : Sleek2ImageButton
	{
		public Sleek2ImageTranslatedLabelButton()
		{
			this.label = new Sleek2TranslatedLabel();
			this.label.transform.reset();
			this.label.textComponent.color = Sleek2Config.darkTextColor;
			this.addElement(this.label);
		}

		public Sleek2TranslatedLabel label { get; protected set; }
	}
}
