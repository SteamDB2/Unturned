using System;

namespace SDG.Unturned
{
	public class ObjectRegion
	{
		public ObjectRegion()
		{
			this.isNetworked = false;
			this.updateObjectIndex = 0;
		}

		public bool isNetworked;

		public ushort updateObjectIndex;
	}
}
