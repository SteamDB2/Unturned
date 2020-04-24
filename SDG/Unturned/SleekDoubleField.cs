using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class SleekDoubleField : SleekBox
	{
		public SleekDoubleField()
		{
			base.init();
			this.state = 0.0;
			this.fontStyle = 1;
			this.fontAlignment = 4;
			this.fontSize = SleekRender.FONT_SIZE;
			this.calculateContent();
		}

		public double state
		{
			get
			{
				return this._state;
			}
			set
			{
				this._state = value;
				base.text = this.state.ToString("F3");
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
			double num;
			if (text != base.text && double.TryParse(text, out num))
			{
				this._state = num;
				if (this.onTypedDouble != null)
				{
					this.onTypedDouble(this, num);
				}
			}
			base.text = text;
			base.drawChildren(ignoreCulling);
		}

		public TypedDouble onTypedDouble;

		private double _state;
	}
}
