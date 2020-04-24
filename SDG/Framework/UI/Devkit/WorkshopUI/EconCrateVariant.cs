using System;

namespace SDG.Framework.UI.Devkit.WorkshopUI
{
	public class EconCrateVariant : EconVariant
	{
		public EconCrateVariant(int Effect, bool IsCommodity, bool IsGenerated, int Quality) : base(Quality)
		{
			this.Effect = Effect;
			this.IsCommodity = IsCommodity;
			this.IsGenerated = IsGenerated;
		}

		public int Effect;

		public bool IsCommodity;

		public bool IsGenerated;
	}
}
