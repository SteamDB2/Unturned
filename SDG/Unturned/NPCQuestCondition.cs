using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class NPCQuestCondition : NPCLogicCondition
	{
		public NPCQuestCondition(ushort newID, ENPCQuestStatus newStatus, bool newIgnoreNPC, ENPCLogicType newLogicType, string newText, bool newShouldReset) : base(newLogicType, newText, newShouldReset)
		{
			this.id = newID;
			this.status = newStatus;
			this.ignoreNPC = newIgnoreNPC;
		}

		public ushort id { get; protected set; }

		public ENPCQuestStatus status { get; protected set; }

		public bool ignoreNPC { get; protected set; }

		public override bool isConditionMet(Player player)
		{
			return base.doesLogicPass<ENPCQuestStatus>(player.quests.getQuestStatus(this.id), this.status);
		}

		public override void applyCondition(Player player, bool shouldSend)
		{
			if (shouldSend)
			{
				Debug.LogError("Send quest complete not supported!");
				return;
			}
			if (!this.shouldReset)
			{
				return;
			}
			switch (this.status)
			{
			case ENPCQuestStatus.NONE:
				Debug.LogError("Reset none quest status? How should this work?");
				return;
			case ENPCQuestStatus.ACTIVE:
				player.quests.abandonQuest(this.id);
				return;
			case ENPCQuestStatus.READY:
				player.quests.completeQuest(this.id, this.ignoreNPC);
				return;
			case ENPCQuestStatus.COMPLETED:
				player.quests.removeFlag(this.id);
				return;
			default:
				return;
			}
		}
	}
}
