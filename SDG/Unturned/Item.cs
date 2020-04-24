using System;

namespace SDG.Unturned
{
	public class Item
	{
		public Item(ushort newID, bool full) : this(newID, (!full) ? EItemOrigin.WORLD : EItemOrigin.ADMIN)
		{
		}

		public Item(ushort newID, EItemOrigin origin)
		{
			this._id = newID;
			ItemAsset itemAsset = (ItemAsset)Assets.find(EAssetType.ITEM, this.id);
			if (itemAsset == null)
			{
				this.state = new byte[0];
				return;
			}
			if (origin == EItemOrigin.WORLD && !Provider.modeConfigData.Items.Has_Durability)
			{
				origin = EItemOrigin.CRAFT;
			}
			if (origin != EItemOrigin.WORLD)
			{
				this.amount = itemAsset.amount;
				this.quality = 100;
			}
			else
			{
				this.amount = itemAsset.count;
				this.quality = itemAsset.quality;
			}
			this.state = itemAsset.getState(origin);
		}

		public Item(ushort newID, bool full, byte newQuality) : this(newID, (!full) ? EItemOrigin.WORLD : EItemOrigin.ADMIN, newQuality)
		{
		}

		public Item(ushort newID, EItemOrigin origin, byte newQuality)
		{
			this._id = newID;
			this.quality = newQuality;
			ItemAsset itemAsset = (ItemAsset)Assets.find(EAssetType.ITEM, this.id);
			if (itemAsset == null)
			{
				this.state = new byte[0];
				return;
			}
			if (origin == EItemOrigin.WORLD && !Provider.modeConfigData.Items.Has_Durability)
			{
				origin = EItemOrigin.CRAFT;
			}
			if (origin != EItemOrigin.WORLD)
			{
				this.amount = itemAsset.amount;
			}
			else
			{
				this.amount = itemAsset.count;
			}
			this.state = itemAsset.getState(origin);
		}

		public Item(ushort newID, byte newAmount, byte newQuality)
		{
			this._id = newID;
			this.amount = newAmount;
			this.quality = newQuality;
			ItemAsset itemAsset = (ItemAsset)Assets.find(EAssetType.ITEM, this.id);
			if (itemAsset == null)
			{
				this.state = new byte[0];
				return;
			}
			this.state = itemAsset.getState();
		}

		public Item(ushort newID, byte newAmount, byte newQuality, byte[] newState)
		{
			this._id = newID;
			this.amount = newAmount;
			this.quality = newQuality;
			this.state = newState;
		}

		public ushort id
		{
			get
			{
				return this._id;
			}
		}

		public byte durability
		{
			get
			{
				return this.quality;
			}
			set
			{
				this.quality = value;
			}
		}

		public byte[] metadata
		{
			get
			{
				return this.state;
			}
			set
			{
				this.state = value;
			}
		}

		public override string ToString()
		{
			return string.Concat(new object[]
			{
				this.id,
				" ",
				this.amount,
				" ",
				this.quality,
				" ",
				this.state.Length
			});
		}

		private ushort _id;

		public byte amount;

		public byte quality;

		public byte[] state;
	}
}
