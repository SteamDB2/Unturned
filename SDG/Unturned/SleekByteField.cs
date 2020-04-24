using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class SleekByteField : SleekBox
	{
		public SleekByteField()
		{
			base.init();
			this.state = 0;
			this.fontStyle = 1;
			this.fontAlignment = 4;
			this.fontSize = SleekRender.FONT_SIZE;
			this.calculateContent();
		}

		public byte state
		{
			get
			{
				return this._state;
			}
			set
			{
				this._state = value;
				base.text = this.state.ToString();
			}
		}

		protected override void calculateContent()
		{
			this.content = new GUIContent(string.Empty, base.tooltip);
		}

		public override void draw(bool ignoreCulling)
		{
			SleekRender.drawBox(base.frame, base.backgroundColor, this.content);
			string text = SleekRender.drawField(base.frame, this.fontStyle, this.fontAlignment, this.fontSize, base.backgroundColor, base.foregroundColor, base.text, 3, false);
			byte b;
			if (text != base.text && byte.TryParse(text, out b))
			{
				this._state = b;
				if (this.onTypedByte != null)
				{
					this.onTypedByte(this, b);
				}
			}
			base.text = text;
			base.drawChildren(ignoreCulling);
		}

		public TypedByte onTypedByte;

		private byte _state;
	}
}
