using System;

namespace SDG.Unturned
{
	public class BlueprintSupply
	{
		public BlueprintSupply(ushort newID, bool newCritical, byte newAmount)
		{
			this._id = newID;
			this._isCritical = newCritical;
			this.amount = (ushort)newAmount;
			this.hasAmount = 0;
		}

		public ushort id
		{
			get
			{
				return this._id;
			}
		}

		public bool isCritical
		{
			get
			{
				return this._isCritical;
			}
		}

		private ushort _id;

		private bool _isCritical;

		public ushort amount;

		public ushort hasAmount;
	}
}
