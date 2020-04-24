using System;

namespace SDG.Unturned
{
	public class NPCBoolFlagReward : INPCReward
	{
		public NPCBoolFlagReward(ushort newID, bool newValue, string newText) : base(newText)
		{
			this.id = newID;
			this.value = newValue;
		}

		public ushort id { get; protected set; }

		public bool value { get; protected set; }

		public override void grantReward(Player player, bool shouldSend)
		{
			if (shouldSend)
			{
				player.quests.sendSetFlag(this.id, (!this.value) ? 0 : 1);
			}
			else
			{
				player.quests.setFlag(this.id, (!this.value) ? 0 : 1);
			}
		}
	}
}
