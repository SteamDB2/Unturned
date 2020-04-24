using System;

namespace SDG.Unturned
{
	public class ItemTankAsset : ItemBarricadeAsset
	{
		public ItemTankAsset(Bundle bundle, Data data, Local localization, ushort id) : base(bundle, data, localization, id)
		{
			this._source = (ETankSource)Enum.Parse(typeof(ETankSource), data.readString("Source"), true);
			this._resource = data.readUInt16("Resource");
			this.resourceState = BitConverter.GetBytes(this.resource);
		}

		public ETankSource source
		{
			get
			{
				return this._source;
			}
		}

		public ushort resource
		{
			get
			{
				return this._resource;
			}
		}

		public override byte[] getState(EItemOrigin origin)
		{
			byte[] array = new byte[2];
			if (origin == EItemOrigin.ADMIN)
			{
				array[0] = this.resourceState[0];
				array[1] = this.resourceState[1];
			}
			return array;
		}

		protected ETankSource _source;

		protected ushort _resource;

		private byte[] resourceState;
	}
}
