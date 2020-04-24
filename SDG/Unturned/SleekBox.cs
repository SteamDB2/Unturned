using System;

namespace SDG.Unturned
{
	public class SleekBox : SleekLabel
	{
		public SleekBox()
		{
			base.init();
			this.backgroundTint = ESleekTint.BACKGROUND;
			this.fontStyle = 1;
			this.fontAlignment = 4;
			this.fontSize = SleekRender.FONT_SIZE;
			this.calculateContent();
		}

		public override void draw(bool ignoreCulling)
		{
			if (!this.isHidden)
			{
				SleekRender.drawBox(base.frame, base.backgroundColor);
				SleekRender.drawLabel(base.frame, this.fontStyle, this.fontAlignment, this.fontSize, this.content2, base.foregroundColor, this.content);
			}
			base.drawChildren(ignoreCulling);
		}
	}
}
