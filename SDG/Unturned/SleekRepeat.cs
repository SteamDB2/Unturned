using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class SleekRepeat : SleekLabel
	{
		public SleekRepeat()
		{
			base.init();
			this.fontStyle = 1;
			this.fontAlignment = 4;
			this.fontSize = SleekRender.FONT_SIZE;
			this.calculateContent();
		}

		public override void draw(bool ignoreCulling)
		{
			if (!this.isHidden)
			{
				if (SleekRender.drawRepeat(base.frame, base.backgroundColor))
				{
					if (!this.isHeld)
					{
						this.isHeld = true;
						if (this.onStartedButton != null)
						{
							this.onStartedButton(this);
						}
					}
				}
				else if (Event.current.type == 7 && this.isHeld)
				{
					this.isHeld = false;
					if (this.onStoppedButton != null)
					{
						this.onStoppedButton(this);
					}
				}
				SleekRender.drawLabel(base.frame, this.fontStyle, this.fontAlignment, this.fontSize, this.content2, base.foregroundColor, this.content);
			}
			base.drawChildren(ignoreCulling);
		}

		public StartedButton onStartedButton;

		public StoppedButton onStoppedButton;

		private bool isHeld;
	}
}
