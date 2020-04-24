using System;

namespace SDG.Unturned
{
	public class DialogueResponse : DialogueElement
	{
		public DialogueResponse(byte newID, byte[] newMessages, ushort newDialogue, ushort newQuest, ushort newVendor, string newText, INPCCondition[] newConditions, INPCReward[] newRewards) : base(newID, newConditions, newRewards)
		{
			this.messages = newMessages;
			this.dialogue = newDialogue;
			this.quest = newQuest;
			this.vendor = newVendor;
			this.text = newText;
		}

		public byte[] messages { get; protected set; }

		public ushort dialogue { get; protected set; }

		public ushort quest { get; protected set; }

		public ushort vendor { get; protected set; }

		public string text { get; protected set; }
	}
}
