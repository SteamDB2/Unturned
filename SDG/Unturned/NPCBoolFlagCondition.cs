using System;

namespace SDG.Unturned
{
	public class NPCBoolFlagCondition : NPCFlagCondition
	{
		public NPCBoolFlagCondition(ushort newID, bool newValue, bool newAllowUnset, ENPCLogicType newLogicType, string newText, bool newShouldReset) : base(newID, newAllowUnset, newLogicType, newText, newShouldReset)
		{
			this.value = newValue;
		}

		public bool value { get; protected set; }

		public override bool isConditionMet(Player player)
		{
			short num;
			if (player.quests.getFlag(base.id, out num))
			{
				return base.doesLogicPass<bool>(num == 1, this.value);
			}
			return base.allowUnset;
		}

		public override void applyCondition(Player player, bool shouldSend)
		{
			if (!this.shouldReset)
			{
				return;
			}
			if (shouldSend)
			{
				player.quests.sendRemoveFlag(base.id);
			}
			else
			{
				player.quests.removeFlag(base.id);
			}
		}

		public override string formatCondition(Player player)
		{
			if (string.IsNullOrEmpty(this.text))
			{
				return null;
			}
			return string.Format(this.text, (!this.isConditionMet(player)) ? 0 : 1);
		}
	}
}
