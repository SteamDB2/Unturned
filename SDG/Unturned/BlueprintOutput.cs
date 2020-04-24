using System;

namespace SDG.Unturned
{
	public class BlueprintOutput
	{
		public BlueprintOutput(ushort newID, byte newAmount)
		{
			this._id = newID;
			this.amount = (ushort)newAmount;
		}

		public ushort id
		{
			get
			{
				return this._id;
			}
		}

		private ushort _id;

		public ushort amount;
	}
}
