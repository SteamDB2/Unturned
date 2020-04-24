using System;

namespace SDG.Unturned
{
	public class ItemsConfigData
	{
		public ItemsConfigData(EGameMode mode)
		{
			this.Despawn_Dropped_Time = 600f;
			this.Despawn_Natural_Time = 900f;
			switch (mode)
			{
			case EGameMode.EASY:
				this.Spawn_Chance = 0.35f;
				this.Respawn_Time = 30f;
				this.Quality_Full_Chance = 0.1f;
				this.Quality_Multiplier = 1f;
				this.Gun_Bullets_Full_Chance = 0.1f;
				this.Gun_Bullets_Multiplier = 1f;
				this.Magazine_Bullets_Full_Chance = 0.1f;
				this.Magazine_Bullets_Multiplier = 1f;
				this.Crate_Bullets_Full_Chance = 0.1f;
				this.Crate_Bullets_Multiplier = 1f;
				break;
			case EGameMode.NORMAL:
				this.Spawn_Chance = 0.35f;
				this.Respawn_Time = 45f;
				this.Quality_Full_Chance = 0.1f;
				this.Quality_Multiplier = 1f;
				this.Gun_Bullets_Full_Chance = 0.05f;
				this.Gun_Bullets_Multiplier = 0.25f;
				this.Magazine_Bullets_Full_Chance = 0.05f;
				this.Magazine_Bullets_Multiplier = 0.5f;
				this.Crate_Bullets_Full_Chance = 0.05f;
				this.Crate_Bullets_Multiplier = 1f;
				break;
			case EGameMode.HARD:
				this.Spawn_Chance = 0.15f;
				this.Respawn_Time = 60f;
				this.Quality_Full_Chance = 0.01f;
				this.Quality_Multiplier = 1f;
				this.Gun_Bullets_Full_Chance = 0.025f;
				this.Gun_Bullets_Multiplier = 0.1f;
				this.Magazine_Bullets_Full_Chance = 0.025f;
				this.Magazine_Bullets_Multiplier = 0.25f;
				this.Crate_Bullets_Full_Chance = 0.025f;
				this.Crate_Bullets_Multiplier = 0.75f;
				break;
			default:
				this.Spawn_Chance = 1f;
				this.Respawn_Time = 1000000f;
				this.Quality_Full_Chance = 1f;
				this.Quality_Multiplier = 1f;
				this.Gun_Bullets_Full_Chance = 1f;
				this.Gun_Bullets_Multiplier = 1f;
				this.Magazine_Bullets_Full_Chance = 1f;
				this.Magazine_Bullets_Multiplier = 1f;
				this.Crate_Bullets_Full_Chance = 1f;
				this.Crate_Bullets_Multiplier = 1f;
				break;
			}
			if (mode != EGameMode.EASY)
			{
				this.Has_Durability = true;
			}
			else
			{
				this.Has_Durability = false;
			}
		}

		public float Spawn_Chance;

		public float Despawn_Dropped_Time;

		public float Despawn_Natural_Time;

		public float Respawn_Time;

		public float Quality_Full_Chance;

		public float Quality_Multiplier;

		public float Gun_Bullets_Full_Chance;

		public float Gun_Bullets_Multiplier;

		public float Magazine_Bullets_Full_Chance;

		public float Magazine_Bullets_Multiplier;

		public float Crate_Bullets_Full_Chance;

		public float Crate_Bullets_Multiplier;

		public bool Has_Durability;
	}
}
