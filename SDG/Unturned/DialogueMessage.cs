using System;

namespace SDG.Unturned
{
	public class DialogueMessage : DialogueElement
	{
		public DialogueMessage(byte newID, DialoguePage[] newPages, byte[] newResponses, ushort newPrev, INPCCondition[] newConditions, INPCReward[] newRewards) : base(newID, newConditions, newRewards)
		{
			this.pages = newPages;
			this.responses = newResponses;
			this.prev = newPrev;
		}

		public DialoguePage[] pages { get; protected set; }

		public byte[] responses { get; protected set; }

		public ushort prev { get; protected set; }
	}
}
