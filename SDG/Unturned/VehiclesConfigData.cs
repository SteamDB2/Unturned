using System;

namespace SDG.Unturned
{
	public class VehiclesConfigData
	{
		public VehiclesConfigData(EGameMode mode)
		{
			this.Has_Battery_Chance = 0.8f;
			this.Min_Battery_Charge = 0.5f;
			this.Max_Battery_Charge = 0.75f;
			switch (mode)
			{
			case EGameMode.EASY:
				this.Has_Battery_Chance = 1f;
				this.Min_Battery_Charge = 0.8f;
				this.Max_Battery_Charge = 1f;
				this.Has_Tire_Chance = 1f;
				break;
			case EGameMode.NORMAL:
				this.Has_Battery_Chance = 0.8f;
				this.Min_Battery_Charge = 0.5f;
				this.Max_Battery_Charge = 0.75f;
				this.Has_Tire_Chance = 0.85f;
				break;
			case EGameMode.HARD:
				this.Has_Battery_Chance = 0.25f;
				this.Min_Battery_Charge = 0.1f;
				this.Max_Battery_Charge = 0.3f;
				this.Has_Tire_Chance = 0.7f;
				break;
			default:
				this.Has_Battery_Chance = 1f;
				this.Min_Battery_Charge = 1f;
				this.Max_Battery_Charge = 1f;
				this.Has_Tire_Chance = 1f;
				break;
			}
			this.Respawn_Time = 300f;
			this.Armor_Multiplier = 1f;
		}

		public float Has_Battery_Chance;

		public float Min_Battery_Charge;

		public float Max_Battery_Charge;

		public float Has_Tire_Chance;

		public float Respawn_Time;

		public float Armor_Multiplier;
	}
}
