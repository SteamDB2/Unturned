using System;

namespace SDG.Unturned
{
	public class NPCReputationCondition : NPCLogicCondition
	{
		public NPCReputationCondition(int newReputation, ENPCLogicType newLogicType, string newText) : base(newLogicType, newText, false)
		{
			this.reputation = newReputation;
		}

		public int reputation { get; protected set; }

		public override bool isConditionMet(Player player)
		{
			return base.doesLogicPass<int>(player.skills.reputation, this.reputation);
		}

		public override string formatCondition(Player player)
		{
			if (string.IsNullOrEmpty(this.text))
			{
				this.text = PlayerNPCQuestUI.localization.read("Condition_Reputation");
			}
			string text = player.skills.reputation.ToString();
			if (player.skills.reputation > 0)
			{
				text = "+" + text;
			}
			string text2 = this.reputation.ToString();
			if (this.reputation > 0)
			{
				text2 = "+" + text2;
			}
			return string.Format(this.text, text, text2);
		}
	}
}
