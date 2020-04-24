using System;

namespace SDG.Unturned
{
	public class NPCExperienceCondition : NPCLogicCondition
	{
		public NPCExperienceCondition(uint newExperience, ENPCLogicType newLogicType, string newText, bool newShouldReset) : base(newLogicType, newText, newShouldReset)
		{
			this.experience = newExperience;
		}

		public uint experience { get; protected set; }

		public override bool isConditionMet(Player player)
		{
			return base.doesLogicPass<uint>(player.skills.experience, this.experience);
		}

		public override void applyCondition(Player player, bool shouldSend)
		{
			if (!this.shouldReset)
			{
				return;
			}
			if (shouldSend)
			{
				player.skills.askSpend(this.experience);
			}
			else
			{
				player.skills.modXp2(this.experience);
			}
		}

		public override string formatCondition(Player player)
		{
			if (string.IsNullOrEmpty(this.text))
			{
				this.text = PlayerNPCQuestUI.localization.read("Condition_Experience");
			}
			return string.Format(this.text, player.skills.experience, this.experience);
		}
	}
}
