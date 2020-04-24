using System;

namespace SDG.Unturned
{
	public class NPCExperienceReward : INPCReward
	{
		public NPCExperienceReward(uint newValue, string newText) : base(newText)
		{
			this.value = newValue;
		}

		public uint value { get; protected set; }

		public override void grantReward(Player player, bool shouldSend)
		{
			if (shouldSend)
			{
				player.skills.askAward(this.value);
			}
			else
			{
				player.skills.modXp(this.value);
			}
		}

		public override string formatReward(Player player)
		{
			if (string.IsNullOrEmpty(this.text))
			{
				this.text = PlayerNPCQuestUI.localization.read("Reward_Experience");
			}
			string arg = "+" + this.value;
			return string.Format(this.text, arg);
		}
	}
}
