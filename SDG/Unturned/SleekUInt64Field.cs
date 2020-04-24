using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class SleekUInt64Field : SleekBox
	{
		public SleekUInt64Field()
		{
			base.init();
			this.state = 0UL;
			this.fontStyle = 1;
			this.fontAlignment = 4;
			this.fontSize = SleekRender.FONT_SIZE;
			this.calculateContent();
		}

		public ulong state
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
			ulong num;
			if (text != base.text && ulong.TryParse(text, out num))
			{
				this._state = num;
				if (this.onTypedUInt64 != null)
				{
					this.onTypedUInt64(this, num);
				}
			}
			base.text = text;
			base.drawChildren(ignoreCulling);
		}

		public TypedUInt64 onTypedUInt64;

		private ulong _state;
	}
}
