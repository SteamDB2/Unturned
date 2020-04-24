using System;

namespace SDG.Unturned
{
	public class AnimalsConfigData
	{
		public AnimalsConfigData(EGameMode mode)
		{
			this.Respawn_Time = 180f;
			if (mode != EGameMode.EASY)
			{
				if (mode != EGameMode.HARD)
				{
					this.Damage_Multiplier = 1f;
					this.Armor_Multiplier = 1f;
				}
				else
				{
					this.Damage_Multiplier = 1.5f;
					this.Armor_Multiplier = 0.75f;
				}
			}
			else
			{
				this.Damage_Multiplier = 0.75f;
				this.Armor_Multiplier = 1.25f;
			}
		}

		public float Respawn_Time;

		public float Damage_Multiplier;

		public float Armor_Multiplier;
	}
}
