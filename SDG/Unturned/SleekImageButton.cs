using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class SleekImageButton : Sleek
	{
		public SleekImageButton()
		{
			base.init();
		}

		public override void draw(bool ignoreCulling)
		{
			if (!this.isHidden)
			{
				if (SleekRender.drawImageButton(base.frame, this.texture, base.backgroundColor))
				{
					if (this.onClickedImage != null)
					{
						this.onClickedImage(this);
					}
					if (!this.isHeld)
					{
						this.isHeld = true;
						if (this.onClickImageStarted != null)
						{
							this.onClickImageStarted(this);
						}
					}
				}
				else if (this.isHeld)
				{
					this.isHeld = false;
					if (this.onClickImageStopped != null)
					{
						this.onClickImageStopped(this);
					}
				}
			}
			base.drawChildren(ignoreCulling);
		}

		public ClickedImage onClickedImage;

		public ClickImageStarted onClickImageStarted;

		public ClickImageStopped onClickImageStopped;

		public Texture texture;

		private bool isHeld;
	}
}
