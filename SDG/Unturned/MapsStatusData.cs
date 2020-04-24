using System;

namespace SDG.Unturned
{
	public class MapsStatusData
	{
		public MapsStatusData()
		{
			this.Official = EMapStatus.NONE;
			this.Curated = EMapStatus.NONE;
			this.Misc = EMapStatus.NONE;
		}

		public EMapStatus Official;

		public EMapStatus Curated;

		public EMapStatus Misc;
	}
}
