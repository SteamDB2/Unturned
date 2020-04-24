using System;

namespace SDG.Unturned
{
	public class ObjectConfigData
	{
		public ObjectConfigData(EGameMode mode)
		{
			this.Binary_State_Reset_Multiplier = 1f;
			this.Fuel_Reset_Multiplier = 1f;
			this.Water_Reset_Multiplier = 1f;
			this.Resource_Reset_Multiplier = 1f;
			this.Rubble_Reset_Multiplier = 1f;
		}

		public float Binary_State_Reset_Multiplier;

		public float Fuel_Reset_Multiplier;

		public float Water_Reset_Multiplier;

		public float Resource_Reset_Multiplier;

		public float Rubble_Reset_Multiplier;
	}
}
