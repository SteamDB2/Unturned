using System;

namespace SDG.Unturned
{
	public class SleekButton : SleekLabel
	{
		public SleekButton()
		{
			base.init();
			this.backgroundTint = ESleekTint.BACKGROUND;
			this.fontStyle = 1;
			this.fontAlignment = 4;
			this.fontSize = SleekRender.FONT_SIZE;
			this.calculateContent();
			this.isClickable = true;
		}

		public override void draw(bool ignoreCulling)
		{
			if (!this.isHidden)
			{
				if (this.isClickable)
				{
					if (SleekRender.drawButton(base.frame, base.backgroundColor) && this.onClickedButton != null)
					{
						this.onClickedButton(this);
					}
				}
				else
				{
					SleekRender.drawBox(base.frame, base.backgroundColor);
				}
				SleekRender.drawLabel(base.frame, this.fontStyle, this.fontAlignment, this.fontSize, this.content2, base.foregroundColor, this.content);
			}
			base.drawChildren(ignoreCulling);
		}

		public ClickedButton onClickedButton;

		public bool isClickable;
	}
}
