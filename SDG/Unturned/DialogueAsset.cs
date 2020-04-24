using System;

namespace SDG.Unturned
{
	public class DialogueAsset : Asset
	{
		public DialogueAsset(Bundle bundle, Data data, Local localization, ushort id) : base(bundle, data, localization, id)
		{
			if (id < 2000 && !bundle.hasResource && !data.has("Bypass_ID_Limit"))
			{
				throw new NotSupportedException("ID < 2000");
			}
			this.messages = new DialogueMessage[(int)data.readByte("Messages")];
			byte b = 0;
			while ((int)b < this.messages.Length)
			{
				DialoguePage[] array = new DialoguePage[(int)data.readByte("Message_" + b + "_Pages")];
				byte b2 = 0;
				while ((int)b2 < array.Length)
				{
					string text = localization.format(string.Concat(new object[]
					{
						"Message_",
						b,
						"_Page_",
						b2
					}));
					text = ItemTool.filterRarityRichText(text);
					array[(int)b2] = new DialoguePage(text);
					b2 += 1;
				}
				byte[] array2 = new byte[(int)data.readByte("Message_" + b + "_Responses")];
				byte b3 = 0;
				while ((int)b3 < array2.Length)
				{
					array2[(int)b3] = data.readByte(string.Concat(new object[]
					{
						"Message_",
						b,
						"_Response_",
						b3
					}));
					b3 += 1;
				}
				ushort newPrev = data.readUInt16("Message_" + b + "_Prev");
				INPCCondition[] array3 = new INPCCondition[(int)data.readByte("Message_" + b + "_Conditions")];
				NPCTool.readConditions(data, localization, "Message_" + b + "_Condition_", array3);
				INPCReward[] array4 = new INPCReward[(int)data.readByte("Message_" + b + "_Rewards")];
				NPCTool.readRewards(data, localization, "Message_" + b + "_Reward_", array4);
				this.messages[(int)b] = new DialogueMessage(b, array, array2, newPrev, array3, array4);
				b += 1;
			}
			this.responses = new DialogueResponse[(int)data.readByte("Responses")];
			byte b4 = 0;
			while ((int)b4 < this.responses.Length)
			{
				byte[] array5 = new byte[(int)data.readByte("Response_" + b4 + "_Messages")];
				byte b5 = 0;
				while ((int)b5 < array5.Length)
				{
					array5[(int)b5] = data.readByte(string.Concat(new object[]
					{
						"Response_",
						b4,
						"_Message_",
						b5
					}));
					b5 += 1;
				}
				ushort newDialogue = data.readUInt16("Response_" + b4 + "_Dialogue");
				ushort newQuest = data.readUInt16("Response_" + b4 + "_Quest");
				ushort newVendor = data.readUInt16("Response_" + b4 + "_Vendor");
				string text2 = localization.format("Response_" + b4);
				text2 = ItemTool.filterRarityRichText(text2);
				INPCCondition[] array6 = new INPCCondition[(int)data.readByte("Response_" + b4 + "_Conditions")];
				NPCTool.readConditions(data, localization, "Response_" + b4 + "_Condition_", array6);
				INPCReward[] array7 = new INPCReward[(int)data.readByte("Response_" + b4 + "_Rewards")];
				NPCTool.readRewards(data, localization, "Response_" + b4 + "_Reward_", array7);
				this.responses[(int)b4] = new DialogueResponse(b4, array5, newDialogue, newQuest, newVendor, text2, array6, array7);
				b4 += 1;
			}
			bundle.unload();
		}

		public DialogueMessage[] messages { get; protected set; }

		public DialogueResponse[] responses { get; protected set; }

		public override EAssetType assetCategory
		{
			get
			{
				return EAssetType.NPC;
			}
		}

		public int getAvailableMessage(Player player)
		{
			for (int i = 0; i < this.messages.Length; i++)
			{
				DialogueMessage dialogueMessage = this.messages[i];
				if (dialogueMessage.areConditionsMet(player))
				{
					return i;
				}
			}
			return -1;
		}
	}
}
