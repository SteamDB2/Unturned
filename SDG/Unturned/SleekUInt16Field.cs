using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class SleekUInt16Field : SleekBox
	{
		public SleekUInt16Field()
		{
			base.init();
			this.state = 0;
			this.fontStyle = 1;
			this.fontAlignment = 4;
			this.fontSize = SleekRender.FONT_SIZE;
			this.calculateContent();
		}

		public ushort state
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
			string text = SleekRender.drawField(base.frame, this.fontStyle, this.fontAlignment, this.fontSize, base.backgroundColor, base.foregroundColor, base.text, 64, false);
			ushort num;
			if (text != base.text && ushort.TryParse(text, out num))
			{
				this._state = num;
				if (this.onTypedUInt16 != null)
				{
					this.onTypedUInt16(this, num);
				}
			}
			base.text = text;
			base.drawChildren(ignoreCulling);
		}

		public TypedUInt16 onTypedUInt16;

		private ushort _state;
	}
}
