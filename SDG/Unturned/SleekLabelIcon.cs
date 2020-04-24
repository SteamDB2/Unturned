using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class SleekLabelIcon : SleekLabel
	{
		public SleekLabelIcon(Texture2D newIcon)
		{
			base.init();
			this.iconImage = new SleekImageTexture();
			this.iconImage.positionOffset_X = 5;
			this.iconImage.positionOffset_Y = -10;
			this.iconImage.positionScale_Y = 0.5f;
			this.iconImage.sizeOffset_X = 20;
			this.iconImage.sizeOffset_Y = 20;
			this.iconImage.texture = newIcon;
			base.add(this.iconImage);
			this.fontStyle = 1;
			this.fontAlignment = 4;
			this.fontSize = SleekRender.FONT_SIZE;
			this.calculateContent();
		}

		public Texture2D icon
		{
			set
			{
				this.iconImage.texture = value;
			}
		}

		private SleekImageTexture iconImage;
	}
}
