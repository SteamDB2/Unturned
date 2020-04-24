using System;

namespace SDG.Unturned
{
	public class PlayerQuest
	{
		public PlayerQuest(ushort newID)
		{
			this.id = newID;
			this.asset = (Assets.find(EAssetType.NPC, this.id) as QuestAsset);
		}

		public ushort id { get; private set; }

		public QuestAsset asset { get; protected set; }
	}
}
