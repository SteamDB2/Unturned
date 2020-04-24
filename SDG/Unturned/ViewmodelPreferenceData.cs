using System;

namespace SDG.Unturned
{
	public class ViewmodelPreferenceData
	{
		public ViewmodelPreferenceData()
		{
			this.Field_Of_View_Aim = 60f;
			this.Field_Of_View_Hip = 60f;
			this.Offset_Horizontal = 0f;
			this.Offset_Vertical = 0f;
			this.Offset_Depth = 0f;
		}

		public float Field_Of_View_Aim;

		public float Field_Of_View_Hip;

		public float Offset_Horizontal;

		public float Offset_Vertical;

		public float Offset_Depth;
	}
}
