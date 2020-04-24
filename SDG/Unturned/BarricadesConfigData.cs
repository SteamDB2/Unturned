using System;

namespace SDG.Unturned
{
	public class BarricadesConfigData
	{
		public BarricadesConfigData(EGameMode mode)
		{
			this.Decay_Time = 604800u;
			this.Armor_Multiplier = 1f;
		}

		public uint Decay_Time;

		public float Armor_Multiplier;
	}
}
