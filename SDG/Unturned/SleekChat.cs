using System;

namespace SDG.Unturned
{
	public class SleekChat : Sleek
	{
		public SleekChat()
		{
			base.init();
			this.avatarImage = new SleekImageTexture();
			this.avatarImage.positionOffset_Y = 4;
			this.avatarImage.sizeOffset_X = 32;
			this.avatarImage.sizeOffset_Y = 32;
			this.avatarImage.shouldDestroyTexture = true;
			base.add(this.avatarImage);
			this.repImage = new SleekImageTexture();
			this.repImage.positionOffset_X = 37;
			this.repImage.positionOffset_Y = 4;
			this.repImage.sizeOffset_X = 32;
			this.repImage.sizeOffset_Y = 32;
			base.add(this.repImage);
			this.msg = new SleekLabel();
			this.msg.positionOffset_X = 74;
			this.msg.sizeOffset_X = 400;
			this.msg.sizeOffset_Y = 40;
			this.msg.fontSize = 14;
			this.msg.fontAlignment = 0;
			this.msg.foregroundTint = ESleekTint.NONE;
			base.add(this.msg);
		}

		public SleekImageTexture avatarImage;

		public SleekImageTexture repImage;

		public SleekLabel msg;
	}
}
