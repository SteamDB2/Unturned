using System;
using UnityEngine;

namespace SDG.Framework.UI.Sleek2
{
	public class Sleek2ToolbarButton : Sleek2HoverDropdownButton
	{
		public Sleek2ToolbarButton()
		{
			base.transform.anchorMin = Vector2.zero;
			base.transform.anchorMax = new Vector2(0f, 1f);
			base.transform.pivot = new Vector2(0f, 0.5f);
			base.transform.sizeDelta = new Vector2((float)Sleek2Config.tabWidth, 0f);
			this.label = new Sleek2Label();
			this.label.transform.anchorMin = Vector2.zero;
			this.label.transform.anchorMax = Vector2.one;
			this.label.transform.sizeDelta = Vector2.zero;
			this.addElement(this.label);
		}

		public Sleek2Label label { get; protected set; }
	}
}
