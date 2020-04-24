using System;

namespace SDG.Unturned
{
	public class ItemGeneratorAsset : ItemBarricadeAsset
	{
		public ItemGeneratorAsset(Bundle bundle, Data data, Local localization, ushort id) : base(bundle, data, localization, id)
		{
			this._capacity = data.readUInt16("Capacity");
			this._wirerange = data.readSingle("Wirerange");
			this._burn = data.readSingle("Burn");
		}

		public ushort capacity
		{
			get
			{
				return this._capacity;
			}
		}

		public float wirerange
		{
			get
			{
				return this._wirerange;
			}
		}

		public float burn
		{
			get
			{
				return this._burn;
			}
		}

		public override byte[] getState(EItemOrigin origin)
		{
			return new byte[3];
		}

		protected ushort _capacity;

		protected float _wirerange;

		protected float _burn;
	}
}
