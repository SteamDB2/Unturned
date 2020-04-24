using System;

namespace SDG.Unturned
{
	public class VendorElement
	{
		public VendorElement(byte newIndex, ushort newID, uint newCost, INPCCondition[] newConditions)
		{
			this.index = newIndex;
			this.id = newID;
			this.cost = newCost;
			this.conditions = newConditions;
		}

		public byte index { get; protected set; }

		public ushort id { get; protected set; }

		public uint cost { get; protected set; }

		public INPCCondition[] conditions { get; protected set; }

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
	}
}
