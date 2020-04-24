using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class INPCCondition
	{
		public INPCCondition(string newText, bool newShouldReset)
		{
			this.text = newText;
			this.shouldReset = newShouldReset;
		}

		public virtual bool isConditionMet(Player player)
		{
			return false;
		}

		public virtual void applyCondition(Player player, bool shouldSend)
		{
		}

		public virtual string formatCondition(Player player)
		{
			return null;
		}

		public virtual Sleek createUI(Player player, Texture2D icon)
		{
			string value = this.formatCondition(player);
			if (string.IsNullOrEmpty(value))
			{
				return null;
			}
			SleekBox sleekBox = new SleekBox();
			sleekBox.sizeOffset_Y = 30;
			sleekBox.sizeScale_X = 1f;
			if (icon != null)
			{
				sleekBox.add(new SleekImageTexture(icon)
				{
					positionOffset_X = 5,
					positionOffset_Y = 5,
					sizeOffset_X = 20,
					sizeOffset_Y = 20
				});
			}
			SleekLabel sleekLabel = new SleekLabel();
			if (icon != null)
			{
				sleekLabel.positionOffset_X = 30;
				sleekLabel.sizeOffset_X = -35;
			}
			else
			{
				sleekLabel.positionOffset_X = 5;
				sleekLabel.sizeOffset_X = -10;
			}
			sleekLabel.sizeScale_X = 1f;
			sleekLabel.sizeScale_Y = 1f;
			sleekLabel.fontAlignment = 3;
			sleekLabel.foregroundTint = ESleekTint.NONE;
			sleekLabel.isRich = true;
			sleekLabel.text = value;
			sleekBox.add(sleekLabel);
			return sleekBox;
		}

		protected string text;

		protected bool shouldReset;
	}
}
