using System;

namespace SDG.Unturned
{
	public class NPCShortFlagReward : INPCReward
	{
		public NPCShortFlagReward(ushort newID, short newValue, ENPCModificationType newModificationType, string newText) : base(newText)
		{
			this.id = newID;
			this.value = newValue;
			this.modificationType = newModificationType;
		}

		public ushort id { get; protected set; }

		public virtual short value { get; protected set; }

		public ENPCModificationType modificationType { get; protected set; }

		public override void grantReward(Player player, bool shouldSend)
		{
			if (this.modificationType == ENPCModificationType.ASSIGN)
			{
				if (shouldSend)
				{
					player.quests.sendSetFlag(this.id, this.value);
				}
				else
				{
					player.quests.setFlag(this.id, this.value);
				}
			}
			else
			{
				short num;
				player.quests.getFlag(this.id, out num);
				if (this.modificationType == ENPCModificationType.INCREMENT)
				{
					num += this.value;
				}
				else if (this.modificationType == ENPCModificationType.DECREMENT)
				{
					num -= this.value;
				}
				if (shouldSend)
				{
					player.quests.sendSetFlag(this.id, num);
				}
				else
				{
					player.quests.setFlag(this.id, num);
				}
			}
		}
	}
}
