using System;

namespace SDG.Unturned
{
	public class ItemIconInfo
	{
		public ushort id;

		public ushort skin;

		public byte quality;

		public byte[] state;

		public ItemAsset itemAsset;

		public SkinAsset skinAsset;

		public int x;

		public int y;

		public bool scale;

		public ItemIconReady callback;
	}
}
