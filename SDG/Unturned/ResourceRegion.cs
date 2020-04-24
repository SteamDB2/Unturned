using System;

namespace SDG.Unturned
{
	public class ResourceRegion
	{
		public ResourceRegion()
		{
			this.isNetworked = false;
			this.respawnResourceIndex = 0;
		}

		public bool isNetworked;

		public ushort respawnResourceIndex;
	}
}
