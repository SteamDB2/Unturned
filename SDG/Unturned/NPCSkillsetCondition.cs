using System;

namespace SDG.Unturned
{
	public class NPCSkillsetCondition : NPCLogicCondition
	{
		public NPCSkillsetCondition(EPlayerSkillset newSkillset, ENPCLogicType newLogicType, string newText) : base(newLogicType, newText, false)
		{
			this.skillset = newSkillset;
		}

		public EPlayerSkillset skillset { get; protected set; }

		public override bool isConditionMet(Player player)
		{
			return base.doesLogicPass<EPlayerSkillset>(player.channel.owner.skillset, this.skillset);
		}
	}
}
