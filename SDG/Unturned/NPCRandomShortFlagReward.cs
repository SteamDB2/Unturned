using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class NPCRandomShortFlagReward : NPCShortFlagReward
	{
		public NPCRandomShortFlagReward(ushort newID, short newMinValue, short newMaxValue, ENPCModificationType newModificationType, string newText) : base(newID, 0, newModificationType, newText)
		{
			base.id = newID;
			this.minValue = newMinValue;
			this.maxValue = newMaxValue;
			base.modificationType = newModificationType;
		}

		public short minValue { get; protected set; }

		public short maxValue { get; protected set; }

		public override short value
		{
			get
			{
				return (short)Random.Range((int)this.minValue, (int)(this.maxValue + 1));
			}
			protected set
			{
			}
		}
	}
}
