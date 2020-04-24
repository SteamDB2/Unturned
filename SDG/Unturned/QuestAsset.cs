using System;

namespace SDG.Unturned
{
	public class QuestAsset : Asset
	{
		public QuestAsset(Bundle bundle, Data data, Local localization, ushort id) : base(bundle, data, localization, id)
		{
			if (id < 2000 && !bundle.hasResource && !data.has("Bypass_ID_Limit"))
			{
				throw new NotSupportedException("ID < 2000");
			}
			this.questName = localization.format("Name");
			this.questName = ItemTool.filterRarityRichText(this.questName);
			this.questDescription = localization.format("Description");
			this.questDescription = ItemTool.filterRarityRichText(this.questDescription);
			this.conditions = new INPCCondition[(int)data.readByte("Conditions")];
			NPCTool.readConditions(data, localization, "Condition_", this.conditions);
			this.rewards = new INPCReward[(int)data.readByte("Rewards")];
			NPCTool.readRewards(data, localization, "Reward_", this.rewards);
			bundle.unload();
		}

		public string questName { get; protected set; }

		public string questDescription { get; protected set; }

		public INPCCondition[] conditions { get; protected set; }

		public INPCReward[] rewards { get; protected set; }

		public override EAssetType assetCategory
		{
			get
			{
				return EAssetType.NPC;
			}
		}

		public bool areConditionsMet(Player player)
		{
			if (this.conditions != null)
			{
				for (int i = 0; i < this.conditions.Length; i++)
				{
					if (!this.conditions[i].isConditionMet(player))
					{
						return false;
					}
				}
			}
			return true;
		}

		public void applyConditions(Player player, bool shouldSend)
		{
			if (this.conditions != null)
			{
				for (int i = 0; i < this.conditions.Length; i++)
				{
					this.conditions[i].applyCondition(player, shouldSend);
				}
			}
		}

		public void grantRewards(Player player, bool shouldSend)
		{
			if (this.rewards != null)
			{
				for (int i = 0; i < this.rewards.Length; i++)
				{
					this.rewards[i].grantReward(player, shouldSend);
				}
			}
		}
	}
}
