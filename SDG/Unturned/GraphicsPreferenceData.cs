using System;

namespace SDG.Unturned
{
	public class GraphicsPreferenceData
	{
		public GraphicsPreferenceData()
		{
			this.Use_Skybox_Ambience = false;
			this.Use_Lens_Dirt = true;
		}

		public bool Use_Skybox_Ambience;

		public bool Use_Lens_Dirt;
	}
}
