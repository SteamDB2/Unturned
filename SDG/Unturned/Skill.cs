using System;

namespace SDG.Unturned
{
	public class Skill
	{
		public Skill(byte newLevel, byte newMax, uint newCost, float newDifficulty)
		{
			this.level = newLevel;
			this.max = newMax;
			this._cost = newCost;
			this.difficulty = newDifficulty;
		}

		public float mastery
		{
			get
			{
				if (this.level == 0)
				{
					return 0f;
				}
				if (this.level >= this.max)
				{
					return 1f;
				}
				return (float)this.level / (float)this.max;
			}
		}

		public uint cost
		{
			get
			{
				return (uint)(this._cost * ((float)this.level * this.difficulty + 1f));
			}
		}

		public byte level;

		public byte max;

		private uint _cost;

		private float difficulty;
	}
}
