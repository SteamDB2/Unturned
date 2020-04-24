using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class SleekButtonIcon : SleekButton
	{
		public SleekButtonIcon(Texture2D newIcon, int newSize)
		{
			base.init();
			this.backgroundTint = ESleekTint.BACKGROUND;
			this.iconImage = new SleekImageTexture();
			this.iconSize = newSize;
			this.iconScale = false;
			this.iconImage.positionOffset_X = 5;
			this.iconImage.positionOffset_Y = 5;
			this.iconImage.sizeOffset_X = this.iconSize;
			this.iconImage.sizeOffset_Y = this.iconSize;
			this.iconImage.texture = newIcon;
			base.add(this.iconImage);
			this.fontStyle = 1;
			this.fontAlignment = 4;
			this.fontSize = SleekRender.FONT_SIZE;
			this.calculateContent();
		}

		public SleekButtonIcon(Texture2D newIcon)
		{
			base.init();
			this.backgroundTint = ESleekTint.BACKGROUND;
			this.iconImage = new SleekImageTexture();
			this.iconSize = 0;
			this.iconScale = false;
			this.iconImage.positionOffset_X = 5;
			this.iconImage.positionOffset_Y = 5;
			this.iconImage.texture = newIcon;
			base.add(this.iconImage);
			if (this.iconImage.texture != null)
			{
				this.iconImage.sizeOffset_X = this.iconImage.texture.width;
				this.iconImage.sizeOffset_Y = this.iconImage.texture.height;
			}
			this.fontStyle = 1;
			this.fontAlignment = 4;
			this.fontSize = SleekRender.FONT_SIZE;
			this.calculateContent();
		}

		public SleekButtonIcon(Texture2D newIcon, bool newScale)
		{
			base.init();
			this.backgroundTint = ESleekTint.BACKGROUND;
			this.iconImage = new SleekImageTexture();
			this.iconSize = 0;
			this.iconScale = newScale;
			this.iconImage.positionOffset_X = 5;
			this.iconImage.positionOffset_Y = 5;
			if (this.iconScale)
			{
				this.iconImage.sizeOffset_X = -10;
				this.iconImage.sizeOffset_Y = -10;
				this.iconImage.sizeScale_X = 1f;
				this.iconImage.sizeScale_Y = 1f;
			}
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
				if (this.iconSize == 0 && !this.iconScale && this.iconImage.texture != null)
				{
					this.iconImage.sizeOffset_X = this.iconImage.texture.width;
					this.iconImage.sizeOffset_Y = this.iconImage.texture.height;
				}
			}
		}

		public SleekImageTexture iconImage;

		private int iconSize;

		private bool iconScale;
	}
}
