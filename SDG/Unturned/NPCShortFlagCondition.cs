using System;

namespace SDG.Unturned
{
	public class NPCShortFlagCondition : NPCFlagCondition
	{
		public NPCShortFlagCondition(ushort newID, short newValue, bool newAllowUnset, ENPCLogicType newLogicType, string newText, bool newShouldReset) : base(newID, newAllowUnset, newLogicType, newText, newShouldReset)
		{
			this.value = newValue;
		}

		public short value { get; protected set; }

		public override bool isConditionMet(Player player)
		{
			short a;
			if (player.quests.getFlag(base.id, out a))
			{
				return base.doesLogicPass<short>(a, this.value);
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
			short num;
			if (!player.quests.getFlag(base.id, out num))
			{
				if (base.allowUnset)
				{
					num = this.value;
				}
				else
				{
					num = 0;
				}
			}
			return string.Format(this.text, num, this.value);
		}
	}
}
