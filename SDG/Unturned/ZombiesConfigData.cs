using System;

namespace SDG.Unturned
{
	public class ZombiesConfigData
	{
		public ZombiesConfigData(EGameMode mode)
		{
			this.Respawn_Day_Time = 360f;
			this.Respawn_Night_Time = 30f;
			this.Respawn_Beacon_Time = 0f;
			switch (mode)
			{
			case EGameMode.EASY:
				this.Spawn_Chance = 0.2f;
				this.Loot_Chance = 0.55f;
				this.Crawler_Chance = 0f;
				this.Sprinter_Chance = 0f;
				this.Flanker_Chance = 0f;
				this.Burner_Chance = 0f;
				this.Acid_Chance = 0f;
				break;
			case EGameMode.NORMAL:
				this.Spawn_Chance = 0.25f;
				this.Loot_Chance = 0.5f;
				this.Crawler_Chance = 0.15f;
				this.Sprinter_Chance = 0.15f;
				this.Flanker_Chance = 0.025f;
				this.Burner_Chance = 0.025f;
				this.Acid_Chance = 0.025f;
				break;
			case EGameMode.HARD:
				this.Spawn_Chance = 0.3f;
				this.Loot_Chance = 0.3f;
				this.Crawler_Chance = 0.125f;
				this.Sprinter_Chance = 0.175f;
				this.Flanker_Chance = 0.05f;
				this.Burner_Chance = 0.05f;
				this.Acid_Chance = 0.05f;
				break;
			default:
				this.Spawn_Chance = 1f;
				this.Loot_Chance = 0f;
				this.Crawler_Chance = 0f;
				this.Sprinter_Chance = 0f;
				this.Flanker_Chance = 0f;
				this.Burner_Chance = 0f;
				this.Acid_Chance = 0f;
				break;
			}
			this.Boss_Electric_Chance = 0f;
			this.Boss_Wind_Chance = 0f;
			this.Boss_Fire_Chance = 0f;
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
			this.Beacon_Experience_Multiplier = 1f;
			this.Full_Moon_Experience_Multiplier = 2f;
			this.Min_Drops = 1u;
			this.Max_Drops = 1u;
			this.Min_Mega_Drops = 5u;
			this.Max_Mega_Drops = 5u;
			this.Min_Boss_Drops = 8u;
			this.Max_Boss_Drops = 10u;
			this.Slow_Movement = (mode == EGameMode.EASY);
			this.Can_Stun = (mode != EGameMode.HARD);
		}

		public float Spawn_Chance;

		public float Loot_Chance;

		public float Crawler_Chance;

		public float Sprinter_Chance;

		public float Flanker_Chance;

		public float Burner_Chance;

		public float Acid_Chance;

		public float Boss_Electric_Chance;

		public float Boss_Wind_Chance;

		public float Boss_Fire_Chance;

		public float Respawn_Day_Time;

		public float Respawn_Night_Time;

		public float Respawn_Beacon_Time;

		public float Damage_Multiplier;

		public float Armor_Multiplier;

		public float Beacon_Experience_Multiplier;

		public float Full_Moon_Experience_Multiplier;

		public uint Min_Drops;

		public uint Max_Drops;

		public uint Min_Mega_Drops;

		public uint Max_Mega_Drops;

		public uint Min_Boss_Drops;

		public uint Max_Boss_Drops;

		public bool Slow_Movement;

		public bool Can_Stun;
	}
}
