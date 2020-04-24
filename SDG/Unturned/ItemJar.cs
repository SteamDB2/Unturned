using System;

namespace SDG.Unturned
{
	public class ItemJar
	{
		public ItemJar(Item newItem)
		{
			this._item = newItem;
			ItemAsset itemAsset = (ItemAsset)Assets.find(EAssetType.ITEM, this.item.id);
			if (itemAsset == null)
			{
				return;
			}
			this.size_x = itemAsset.size_x;
			this.size_y = itemAsset.size_y;
		}

		public ItemJar(byte new_x, byte new_y, byte newRot, Item newItem)
		{
			this.x = new_x;
			this.y = new_y;
			this.rot = newRot;
			this._item = newItem;
			ItemAsset itemAsset = (ItemAsset)Assets.find(EAssetType.ITEM, this.item.id);
			if (itemAsset == null)
			{
				return;
			}
			this.size_x = itemAsset.size_x;
			this.size_y = itemAsset.size_y;
		}

		public Item item
		{
			get
			{
				return this._item;
			}
		}

		public byte x;

		public byte y;

		public byte rot;

		public byte size_x;

		public byte size_y;

		private Item _item;

		public InteractableItem interactableItem;
	}
}
