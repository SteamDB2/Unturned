using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class SleekImageTexture : Sleek
	{
		public SleekImageTexture()
		{
			base.init();
		}

		public SleekImageTexture(Texture newTexture)
		{
			base.init();
			this.texture = newTexture;
		}

		public void updateTexture(Texture2D newTexture)
		{
			this.texture = newTexture;
		}

		public override void draw(bool ignoreCulling)
		{
			if (!this.isHidden)
			{
				if (this.isAngled)
				{
					SleekRender.drawAngledImageTexture(base.frame, this.texture, this.angle, base.backgroundColor);
				}
				else
				{
					SleekRender.drawImageTexture(base.frame, this.texture, base.backgroundColor);
				}
			}
			base.drawChildren(ignoreCulling);
		}

		public override void destroy()
		{
			if (this.shouldDestroyTexture && this.texture != null)
			{
				Object.DestroyImmediate(this.texture);
				this.texture = null;
			}
			base.destroyChildren();
		}

		public Texture texture;

		public float angle;

		public bool isAngled;

		public bool shouldDestroyTexture;
	}
}
