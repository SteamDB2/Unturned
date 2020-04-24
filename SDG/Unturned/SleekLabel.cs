using System;
using System.Text.RegularExpressions;
using UnityEngine;

namespace SDG.Unturned
{
	public class SleekLabel : Sleek
	{
		public SleekLabel()
		{
			base.init();
			this.foregroundTint = ESleekTint.FONT;
			this.fontStyle = 1;
			this.fontAlignment = 4;
			this.fontSize = SleekRender.FONT_SIZE;
			this.calculateContent();
		}

		public string text
		{
			get
			{
				return this._text;
			}
			set
			{
				this._text = value;
				this.calculateContent();
			}
		}

		public string tooltip
		{
			get
			{
				return this._tooltip;
			}
			set
			{
				this._tooltip = value;
				this.calculateContent();
			}
		}

		protected virtual void calculateContent()
		{
			this.content = new GUIContent(this.text, this.tooltip);
			if (this.isRich)
			{
				this.content2 = new GUIContent(Regex.Replace(this.text, "</*color.*?>", string.Empty), Regex.Replace(this.tooltip, "<.*?>", string.Empty));
			}
			else
			{
				this.content2 = null;
			}
		}

		public override void draw(bool ignoreCulling)
		{
			SleekRender.drawLabel(base.frame, this.fontStyle, this.fontAlignment, this.fontSize, this.content2, base.foregroundColor, this.content);
			base.drawChildren(ignoreCulling);
		}

		private string _text = string.Empty;

		private string _tooltip = string.Empty;

		public FontStyle fontStyle;

		public TextAnchor fontAlignment;

		public int fontSize;

		public GUIContent content;

		protected GUIContent content2;
	}
}
