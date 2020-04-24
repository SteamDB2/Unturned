using System;

namespace SDG.Unturned
{
	public class NPCFlagCondition : NPCLogicCondition
	{
		public NPCFlagCondition(ushort newID, bool newAllowUnset, ENPCLogicType newLogicType, string newText, bool newShouldReset) : base(newLogicType, newText, newShouldReset)
		{
			this.id = newID;
			this.allowUnset = newAllowUnset;
		}

		public ushort id { get; protected set; }

		public bool allowUnset { get; protected set; }
	}
}
