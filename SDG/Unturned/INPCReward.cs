using System;

namespace SDG.Unturned
{
	public class INPCReward
	{
		public INPCReward(string newText)
		{
			this.text = newText;
		}

		public virtual void grantReward(Player player, bool shouldSend)
		{
		}

		public virtual string formatReward(Player player)
		{
			return null;
		}

		public virtual Sleek createUI(Player player)
		{
			string value = this.formatReward(player);
			if (string.IsNullOrEmpty(value))
			{
				return null;
			}
			SleekBox sleekBox = new SleekBox();
			sleekBox.sizeOffset_Y = 30;
			sleekBox.sizeScale_X = 1f;
			sleekBox.add(new SleekLabel
			{
				positionOffset_X = 5,
				sizeOffset_X = -10,
				sizeScale_X = 1f,
				sizeScale_Y = 1f,
				fontAlignment = 3,
				foregroundTint = ESleekTint.NONE,
				isRich = true,
				text = value
			});
			return sleekBox;
		}

		protected string text;
	}
}
