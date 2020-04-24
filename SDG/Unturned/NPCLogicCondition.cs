using System;

namespace SDG.Unturned
{
	public class NPCLogicCondition : INPCCondition
	{
		public NPCLogicCondition(ENPCLogicType newLogicType, string newText, bool newShouldReset) : base(newText, newShouldReset)
		{
			this.logicType = newLogicType;
		}

		public ENPCLogicType logicType { get; protected set; }

		protected bool doesLogicPass<T>(T a, T b) where T : IComparable
		{
			int num = a.CompareTo(b);
			switch (this.logicType)
			{
			case ENPCLogicType.LESS_THAN:
				return num < 0;
			case ENPCLogicType.LESS_THAN_OR_EQUAL_TO:
				return num <= 0;
			case ENPCLogicType.EQUAL:
				return num == 0;
			case ENPCLogicType.NOT_EQUAL:
				return num != 0;
			case ENPCLogicType.GREATER_THAN_OR_EQUAL_TO:
				return num >= 0;
			case ENPCLogicType.GREATER_THAN:
				return num > 0;
			default:
				return false;
			}
		}
	}
}
