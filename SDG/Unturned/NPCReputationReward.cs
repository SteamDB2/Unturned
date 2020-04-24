using System;

namespace SDG.Unturned
{
	public class NPCReputationReward : INPCReward
	{
		public NPCReputationReward(int newValue, string newText) : base(newText)
		{
			this.value = newValue;
		}

		public int value { get; protected set; }

		public override void grantReward(Player player, bool shouldSend)
		{
			if (shouldSend)
			{
				player.skills.askRep(this.value);
			}
			else
			{
				player.skills.modRep(this.value);
			}
		}

		public override string formatReward(Player player)
		{
			if (string.IsNullOrEmpty(this.text))
			{
				this.text = PlayerNPCQuestUI.localization.read("Reward_Reputation");
			}
			string text = this.value.ToString();
			if (this.value > 0)
			{
				text = "+" + text;
			}
			return string.Format(this.text, text);
		}
	}
}
