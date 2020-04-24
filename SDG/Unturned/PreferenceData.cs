using System;

namespace SDG.Unturned
{
	public class PreferenceData
	{
		public PreferenceData()
		{
			this.Graphics = new GraphicsPreferenceData();
			this.Viewmodel = new ViewmodelPreferenceData();
		}

		public GraphicsPreferenceData Graphics;

		public ViewmodelPreferenceData Viewmodel;
	}
}
