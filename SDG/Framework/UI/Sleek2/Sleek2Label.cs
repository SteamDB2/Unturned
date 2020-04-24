using System;
using UnityEngine;
using UnityEngine.UI;

namespace SDG.Framework.UI.Sleek2
{
	public class Sleek2Label : Sleek2Element
	{
		public Sleek2Label()
		{
			base.gameObject.name = "Label";
			this.textComponent = base.gameObject.AddComponent<Text>();
			this.textComponent.font = Sleek2Label.defaultLabelFont;
			this.textComponent.alignment = 4;
			this.textComponent.color = Sleek2Config.lightTextColor;
			this.textComponent.fontSize = Sleek2Config.bodyFontSize;
			this.shadowComponent = base.gameObject.AddComponent<Shadow>();
			this.shadowComponent.effectColor = new Color(0f, 0f, 0f, 0.5f);
		}

		public static Font defaultLabelFont
		{
			get
			{
				if (Sleek2Label._defaultLabelFont == null)
				{
					Sleek2Label._defaultLabelFont = Resources.GetBuiltinResource<Font>("Arial.ttf");
				}
				return Sleek2Label._defaultLabelFont;
			}
		}

		public Text textComponent { get; protected set; }

		public Shadow shadowComponent { get; protected set; }

		protected static Font _defaultLabelFont;
	}
}
