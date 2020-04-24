using System;

namespace SDG.Unturned
{
	public class NPCQuestReward : INPCReward
	{
		public NPCQuestReward(ushort newID, string newText) : base(newText)
		{
			this.id = newID;
		}

		public ushort id { get; protected set; }

		public override void grantReward(Player player, bool shouldSend)
		{
			if (shouldSend)
			{
				player.quests.sendAddQuest(this.id);
			}
			else
			{
				player.quests.addQuest(this.id);
			}
		}
	}
}
