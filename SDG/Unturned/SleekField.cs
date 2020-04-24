using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class SleekField : SleekBox
	{
		public SleekField()
		{
			base.init();
			this.replace = ' ';
			this.fontStyle = 1;
			this.fontAlignment = 4;
			this.fontSize = SleekRender.FONT_SIZE;
			this.maxLength = 16;
			this.calculateContent();
		}

		protected override void calculateContent()
		{
			this.content = new GUIContent(string.Empty, base.tooltip);
		}

		public override void draw(bool ignoreCulling)
		{
			SleekRender.drawBox(base.frame, base.backgroundColor, this.content);
			if (this.control != null && this.control.Length > 0)
			{
				GUI.SetNextControlName(this.control);
			}
			string text;
			if (this.replace != ' ')
			{
				text = SleekRender.drawField(base.frame, this.fontStyle, this.fontAlignment, this.fontSize, base.backgroundColor, base.foregroundColor, base.text, this.maxLength, this.hint, this.replace);
			}
			else
			{
				text = SleekRender.drawField(base.frame, this.fontStyle, this.fontAlignment, this.fontSize, base.backgroundColor, base.foregroundColor, base.text, this.maxLength, this.hint, this.multiline);
			}
			if (text != base.text && this.onTyped != null)
			{
				this.onTyped(this, text);
			}
			base.text = text;
			if (this.control != null && this.control.Length > 0 && GUI.GetNameOfFocusedControl() == this.control && Event.current.isKey && Event.current.type == 5)
			{
				if (Event.current.keyCode == 27)
				{
					if (this.onEscaped != null)
					{
						this.onEscaped(this);
					}
					GUI.FocusControl(string.Empty);
				}
				else if (Event.current.keyCode == 13 && this.onEntered != null)
				{
					this.onEntered(this);
				}
			}
			base.drawChildren(ignoreCulling);
		}

		public Escaped onEscaped;

		public Entered onEntered;

		public Typed onTyped;

		public char replace;

		public string hint;

		public string control;

		public bool multiline;

		public int maxLength;
	}
}
