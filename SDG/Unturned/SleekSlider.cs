using System;

namespace SDG.Unturned
{
	public class SleekSlider : Sleek
	{
		public SleekSlider()
		{
			base.init();
			this.backgroundTint = ESleekTint.BACKGROUND;
			this.orientation = ESleekOrientation.VERTICAL;
			this.size = 0.25f;
		}

		public float state
		{
			get
			{
				return this._state;
			}
			set
			{
				this._state = value;
				this.scroll = this.state * (1f - this.size);
			}
		}

		public override void draw(bool ignoreCulling)
		{
			float num = SleekRender.drawSlider(base.frame, this.orientation, this.scroll, this.size, base.backgroundColor);
			if (num != this.scroll)
			{
				this._state = num / (1f - this.size);
				if (this.state < 0f)
				{
					this.state = 0f;
				}
				else if (this.state > 1f)
				{
					this.state = 1f;
				}
				if (this.onDragged != null)
				{
					this.onDragged(this, this.state);
				}
			}
			this.scroll = num;
			base.drawChildren(ignoreCulling);
		}

		public Dragged onDragged;

		public ESleekOrientation orientation;

		public float size;

		private float scroll;

		private float _state;
	}
}
