using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class SleekSlot : Sleek
	{
		public SleekSlot(byte newPage)
		{
			this._page = newPage;
			base.init();
			base.sizeOffset_X = 250;
			base.sizeOffset_Y = 150;
			this.image = new SleekImageTexture();
			this.image.sizeScale_X = 1f;
			this.image.sizeScale_Y = 1f;
			this.image.texture = (Texture2D)PlayerDashboardInventoryUI.icons.load("Slot_" + this.page + "_Free");
			this.image.backgroundTint = ESleekTint.FOREGROUND;
			base.add(this.image);
		}

		public SleekItem item
		{
			get
			{
				return this._item;
			}
		}

		public byte page
		{
			get
			{
				return this._page;
			}
		}

		public override void draw(bool ignoreCulling)
		{
			base.drawChildren(ignoreCulling);
		}

		public void select()
		{
			if (this.onPlacedItem != null)
			{
				this.onPlacedItem(this.page, 0, 0);
			}
		}

		public void updateItem(ItemJar jar)
		{
			if (this.item == null)
			{
				return;
			}
			this.item.updateItem(jar);
		}

		public void applyItem(ItemJar jar)
		{
			if (this.item != null)
			{
				base.remove(this.item);
			}
			if (jar != null)
			{
				this._item = new SleekItem(jar);
				this.item.positionOffset_X = (int)(-jar.size_x * 25);
				this.item.positionOffset_Y = (int)(-jar.size_y * 25);
				this.item.positionScale_X = 0.5f;
				this.item.positionScale_Y = 0.5f;
				this.item.updateHotkey(this.page);
				this.item.onClickedItem = new ClickedItem(this.onClickedItem);
				this.item.onDraggedItem = new DraggedItem(this.onDraggedItem);
				base.add(this.item);
			}
		}

		private void onClickedItem(SleekItem item)
		{
			if (this.onSelectedItem != null)
			{
				this.onSelectedItem(this.page, 0, 0);
			}
		}

		private void onDraggedItem(SleekItem item)
		{
			if (this.onGrabbedItem != null)
			{
				this.onGrabbedItem(this.page, 0, 0, item);
			}
		}

		public SelectedItem onSelectedItem;

		public GrabbedItem onGrabbedItem;

		public PlacedItem onPlacedItem;

		private SleekImageTexture image;

		private SleekItem _item;

		private byte _page;
	}
}
