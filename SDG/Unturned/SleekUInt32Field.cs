using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class SleekUInt32Field : SleekBox
	{
		public SleekUInt32Field()
		{
			base.init();
			this.state = 0u;
			this.fontStyle = 1;
			this.fontAlignment = 4;
			this.fontSize = SleekRender.FONT_SIZE;
			this.calculateContent();
		}

		public uint state
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
			uint num;
			if (text != base.text && uint.TryParse(text, out num))
			{
				this._state = num;
				if (this.onTypedUInt32 != null)
				{
					this.onTypedUInt32(this, num);
				}
			}
			base.text = text;
			base.drawChildren(ignoreCulling);
		}

		public TypedUInt32 onTypedUInt32;

		private uint _state;
	}
}
