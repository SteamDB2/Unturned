using System;

namespace SDG.Unturned
{
	public class GameplayConfigData
	{
		public GameplayConfigData(EGameMode mode)
		{
			this.Repair_Level_Max = 3u;
			if (mode != EGameMode.HARD)
			{
				this.Hitmarkers = true;
				this.Crosshair = true;
			}
			else
			{
				this.Hitmarkers = false;
				this.Crosshair = false;
			}
			if (mode != EGameMode.EASY)
			{
				this.Ballistics = true;
			}
			else
			{
				this.Ballistics = false;
			}
			this.Chart = (mode == EGameMode.EASY);
			this.Group_Map = (mode != EGameMode.HARD);
			this.Group_HUD = true;
			this.Allow_Dynamic_Groups = true;
			this.Allow_Shoulder_Camera = true;
			this.Timer_Exit = 10u;
			this.Timer_Respawn = 10u;
			this.Timer_Home = 30u;
		}

		public uint Repair_Level_Max;

		public bool Hitmarkers;

		public bool Crosshair;

		public bool Ballistics;

		public bool Chart;

		public bool Group_Map;

		public bool Group_HUD;

		public bool Allow_Dynamic_Groups;

		public bool Allow_Shoulder_Camera;

		public uint Timer_Exit;

		public uint Timer_Respawn;

		public uint Timer_Home;
	}
}
