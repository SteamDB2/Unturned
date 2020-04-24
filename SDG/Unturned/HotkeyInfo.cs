using System;

namespace SDG.Unturned
{
	public class HotkeyInfo
	{
		public HotkeyInfo()
		{
			this.id = 0;
			this.page = byte.MaxValue;
			this.x = byte.MaxValue;
			this.y = byte.MaxValue;
		}

		public ushort id;

		public byte page;

		public byte x;

		public byte y;
	}
}
