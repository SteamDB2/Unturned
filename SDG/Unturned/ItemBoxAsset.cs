using System;

namespace SDG.Unturned
{
	public class ItemBoxAsset : ItemAsset
	{
		public ItemBoxAsset(Bundle bundle, Data data, Local localization, ushort id) : base(bundle, data, localization, id)
		{
			this._generate = data.readInt32("Generate");
			this._destroy = data.readInt32("Destroy");
			this._drops = new int[data.readInt32("Drops")];
			for (int i = 0; i < this.drops.Length; i++)
			{
				this.drops[i] = data.readInt32("Drop_" + i);
			}
			bundle.unload();
		}

		public int generate
		{
			get
			{
				return this._generate;
			}
		}

		public int destroy
		{
			get
			{
				return this._destroy;
			}
		}

		public int[] drops
		{
			get
			{
				return this._drops;
			}
		}

		protected int _generate;

		protected int _destroy;

		protected int[] _drops;
	}
}
