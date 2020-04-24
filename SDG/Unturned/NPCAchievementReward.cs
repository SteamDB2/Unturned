using System;

namespace SDG.Unturned
{
	public class NPCAchievementReward : INPCReward
	{
		public NPCAchievementReward(string newID, string newText) : base(newText)
		{
			this.id = newID;
		}

		public string id { get; protected set; }

		public override void grantReward(Player player, bool shouldSend)
		{
			if (!player.channel.isOwner)
			{
				return;
			}
			bool flag;
			if (Provider.provider.achievementsService.getAchievement(this.id, out flag) && !flag)
			{
				Provider.provider.achievementsService.setAchievement(this.id);
			}
		}
	}
}
