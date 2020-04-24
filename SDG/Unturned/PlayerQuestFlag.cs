using System;

namespace SDG.Unturned
{
	public class PlayerQuestFlag
	{
		public PlayerQuestFlag(ushort newID, short newValue)
		{
			this.id = newID;
			this.value = newValue;
		}

		public ushort id { get; private set; }

		public short value;
	}
}
