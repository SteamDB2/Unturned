using System;

namespace SDG.Unturned
{
	public class NPCRandomItemReward : INPCReward
	{
		public NPCRandomItemReward(ushort newID, byte newAmount, string newText) : base(newText)
		{
			this.id = newID;
			this.amount = newAmount;
		}

		public ushort id { get; protected set; }

		public byte amount { get; protected set; }

		public override void grantReward(Player player, bool shouldSend)
		{
			if (!Provider.isServer)
			{
				return;
			}
			for (byte b = 0; b < this.amount; b += 1)
			{
				ushort num = SpawnTableTool.resolve(this.id);
				if (num != 0)
				{
					player.inventory.forceAddItem(new Item(num, EItemOrigin.CRAFT), false, false);
				}
			}
		}

		public override string formatReward(Player player)
		{
			if (string.IsNullOrEmpty(this.text))
			{
				this.text = PlayerNPCQuestUI.localization.read("Reward_Item_Random");
			}
			return string.Format(this.text, this.amount);
		}
	}
}
