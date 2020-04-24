using System;

namespace SDG.Unturned
{
	public class PlayersConfigData
	{
		public PlayersConfigData(EGameMode mode)
		{
			this.Health_Regen_Min_Food = 90u;
			this.Health_Regen_Min_Water = 90u;
			this.Health_Regen_Ticks = 60u;
			this.Food_Damage_Ticks = 15u;
			this.Water_Damage_Ticks = 20u;
			this.Virus_Infect = 50u;
			this.Virus_Use_Ticks = 125u;
			this.Virus_Damage_Ticks = 25u;
			this.Leg_Regen_Ticks = 750u;
			this.Bleed_Damage_Ticks = 10u;
			this.Bleed_Regen_Ticks = 750u;
			if (mode != EGameMode.EASY)
			{
				if (mode != EGameMode.HARD)
				{
					this.Food_Use_Ticks = 300u;
					this.Water_Use_Ticks = 270u;
				}
				else
				{
					this.Food_Use_Ticks = 250u;
					this.Water_Use_Ticks = 220u;
				}
			}
			else
			{
				this.Food_Use_Ticks = 350u;
				this.Water_Use_Ticks = 320u;
			}
			switch (mode)
			{
			case EGameMode.EASY:
				this.Experience_Multiplier = 1.5f;
				break;
			case EGameMode.NORMAL:
				this.Experience_Multiplier = 1f;
				break;
			case EGameMode.HARD:
				this.Experience_Multiplier = 1.5f;
				break;
			default:
				this.Experience_Multiplier = 10f;
				break;
			}
			if (mode != EGameMode.EASY)
			{
				if (mode != EGameMode.HARD)
				{
					this.Detect_Radius_Multiplier = 1f;
				}
				else
				{
					this.Detect_Radius_Multiplier = 1.25f;
				}
			}
			else
			{
				this.Detect_Radius_Multiplier = 0.5f;
			}
			this.Ray_Aggressor_Distance = 8f;
			this.Armor_Multiplier = 1f;
			this.Lose_Skills_PvP = 0.75f;
			this.Lose_Skills_PvE = 0.75f;
			this.Lose_Items_PvP = 1f;
			this.Lose_Items_PvE = 1f;
			this.Lose_Clothes_PvP = true;
			this.Lose_Clothes_PvE = true;
			this.Can_Hurt_Legs = true;
			if (mode != EGameMode.EASY)
			{
				this.Can_Break_Legs = true;
				this.Can_Start_Bleeding = true;
			}
			else
			{
				this.Can_Break_Legs = false;
				this.Can_Start_Bleeding = false;
			}
			if (mode != EGameMode.HARD)
			{
				this.Can_Fix_Legs = true;
				this.Can_Stop_Bleeding = true;
			}
			else
			{
				this.Can_Fix_Legs = false;
				this.Can_Stop_Bleeding = false;
			}
		}

		public uint Health_Regen_Min_Food;

		public uint Health_Regen_Min_Water;

		public uint Health_Regen_Ticks;

		public uint Food_Use_Ticks;

		public uint Food_Damage_Ticks;

		public uint Water_Use_Ticks;

		public uint Water_Damage_Ticks;

		public uint Virus_Infect;

		public uint Virus_Use_Ticks;

		public uint Virus_Damage_Ticks;

		public uint Leg_Regen_Ticks;

		public uint Bleed_Damage_Ticks;

		public uint Bleed_Regen_Ticks;

		public float Armor_Multiplier;

		public float Experience_Multiplier;

		public float Detect_Radius_Multiplier;

		public float Ray_Aggressor_Distance;

		public float Lose_Skills_PvP;

		public float Lose_Skills_PvE;

		public float Lose_Items_PvP;

		public float Lose_Items_PvE;

		public bool Lose_Clothes_PvP;

		public bool Lose_Clothes_PvE;

		public bool Can_Hurt_Legs;

		public bool Can_Break_Legs;

		public bool Can_Fix_Legs;

		public bool Can_Start_Bleeding;

		public bool Can_Stop_Bleeding;
	}
}
