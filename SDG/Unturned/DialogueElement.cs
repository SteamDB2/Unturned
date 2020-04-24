using System;

namespace SDG.Unturned
{
	public class DialogueElement
	{
		public DialogueElement(byte newIndex, INPCCondition[] newConditions, INPCReward[] newRewards)
		{
			this.index = newIndex;
			this.conditions = newConditions;
			this.rewards = newRewards;
		}

		public byte index { get; protected set; }

		public INPCCondition[] conditions { get; protected set; }

		public INPCReward[] rewards { get; protected set; }

		public bool areConditionsMet(Player player)
		{
			if (this.conditions != null)
			{
				for (int i = 0; i < this.conditions.Length; i++)
				{
					if (!this.conditions[i].isConditionMet(player))
					{
						return false;
					}
				}
			}
			return true;
		}

		public void applyConditions(Player player, bool shouldSend)
		{
			if (this.conditions != null)
			{
				for (int i = 0; i < this.conditions.Length; i++)
				{
					this.conditions[i].applyCondition(player, shouldSend);
				}
			}
		}

		public void grantRewards(Player player, bool shouldSend)
		{
			if (this.rewards != null)
			{
				for (int i = 0; i < this.rewards.Length; i++)
				{
					this.rewards[i].grantReward(player, shouldSend);
				}
			}
		}
	}
}
