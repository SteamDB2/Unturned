using System;

namespace SDG.Unturned
{
	public class LevelInfoConfigData
	{
		public LevelInfoConfigData()
		{
			this.Creators = new string[0];
			this.Collaborators = new string[0];
			this.Thanks = new string[0];
			this.Item = 0;
			this.Feedback = null;
			this.Status = EMapStatus.NONE;
			this.Load_From_Resources = false;
			this.Asset = AssetReference<LevelAsset>.invalid;
			this.Allow_Underwater_Features = false;
			this.Terrain_Snow_Sparkle = false;
			this.Use_Legacy_Clip_Borders = true;
			this.Use_Legacy_Ground = true;
			this.Use_Legacy_Water = true;
			this.Use_Legacy_Objects = true;
			this.Use_Legacy_Snow_Height = true;
			this.Use_Legacy_Fog_Height = true;
			this.Use_Legacy_Oxygen_Height = true;
			this.Use_Rain_Volumes = false;
			this.Use_Snow_Volumes = false;
			this.Is_Aurora_Borealis_Visible = false;
			this.Snow_Affects_Temperature = true;
			this.Has_Atmosphere = true;
			this.Has_Discord_Rich_Presence = false;
			this.Gravity = -9.81f;
			this.Category = ESingleplayerMapCategory.MISC;
			this.Visible_In_Matchmaking = false;
		}

		public string[] Creators;

		public string[] Collaborators;

		public string[] Thanks;

		public int Item;

		public string Feedback;

		public EMapStatus Status;

		public bool Load_From_Resources;

		public AssetReference<LevelAsset> Asset;

		public bool Allow_Underwater_Features;

		public bool Terrain_Snow_Sparkle;

		public bool Use_Legacy_Clip_Borders;

		public bool Use_Legacy_Ground;

		public bool Use_Legacy_Water;

		public bool Use_Legacy_Objects;

		public bool Use_Legacy_Snow_Height;

		public bool Use_Legacy_Fog_Height;

		public bool Use_Legacy_Oxygen_Height;

		public bool Use_Rain_Volumes;

		public bool Use_Snow_Volumes;

		public bool Is_Aurora_Borealis_Visible;

		public bool Snow_Affects_Temperature;

		public bool Has_Atmosphere;

		public bool Has_Discord_Rich_Presence;

		public float Gravity;

		public ESingleplayerMapCategory Category;

		public bool Visible_In_Matchmaking;
	}
}
