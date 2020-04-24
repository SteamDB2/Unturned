using System;

namespace SDG.Unturned
{
	public class NPCEventReward : INPCReward
	{
		public NPCEventReward(string newID, string newText) : base(newText)
		{
			this.id = newID;
		}

		public string id { get; protected set; }

		public override void grantReward(Player player, bool shouldSend)
		{
			NPCEventManager.triggerEventTriggered(this.id);
		}
	}
}
