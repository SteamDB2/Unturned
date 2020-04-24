using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	public class SleekItems : Sleek
	{
		public SleekItems(byte newPage)
		{
			this._page = newPage;
			this._items = new List<SleekItem>();
			base.init();
			this.grid = new SleekGrid();
			this.grid.sizeScale_X = 1f;
			this.grid.sizeScale_Y = 1f;
			this.grid.texture = (Texture2D)PlayerDashboardInventoryUI.icons.load("Grid_Free");
			this.grid.onClickedGrid = new ClickedGrid(this.onClickedGrid);
			this.grid.backgroundTint = ESleekTint.FOREGROUND;
			base.add(this.grid);
		}

		public byte page
		{
			get
			{
				return this._page;
			}
		}

		public byte width
		{
			get
			{
				return this._width;
			}
		}

		public byte height
		{
			get
			{
				return this._height;
			}
		}

		public List<SleekItem> items
		{
			get
			{
				return this._items;
			}
		}

		public override void draw(bool ignoreCulling)
		{
			base.drawChildren(ignoreCulling);
		}

		public void resize(byte newWidth, byte newHeight)
		{
			this._width = newWidth;
			this._height = newHeight;
			base.sizeOffset_X = (int)(this.width * 50);
			base.sizeOffset_Y = (int)(this.height * 50);
		}

		public void clear()
		{
			this.items.Clear();
			base.remove();
			base.add(this.grid);
		}

		public void updateItem(byte index, ItemJar jar)
		{
			this.items[(int)index].updateItem(jar);
		}

		public void addItem(ItemJar jar)
		{
			SleekItem sleekItem = new SleekItem(jar);
			sleekItem.positionOffset_X = (int)(jar.x * 50);
			sleekItem.positionOffset_Y = (int)(jar.y * 50);
			sleekItem.onClickedItem = new ClickedItem(this.onClickedItem);
			sleekItem.onDraggedItem = new DraggedItem(this.onDraggedItem);
			base.add(sleekItem);
			this.items.Add(sleekItem);
		}

		public void removeItem(ItemJar jar)
		{
			for (int i = 0; i < this.items.Count; i++)
			{
				if (this.items[i].positionOffset_X == (int)(jar.x * 50) && this.items[i].positionOffset_Y == (int)(jar.y * 50))
				{
					base.remove(this.items[i]);
					this.items.RemoveAt(i);
					return;
				}
			}
		}

		private void onClickedItem(SleekItem item)
		{
			if (this.onSelectedItem != null)
			{
				this.onSelectedItem(this.page, (byte)(item.positionOffset_X / 50), (byte)(item.positionOffset_Y / 50));
			}
		}

		private void onDraggedItem(SleekItem item)
		{
			if (this.onGrabbedItem != null)
			{
				this.onGrabbedItem(this.page, (byte)(item.positionOffset_X / 50), (byte)(item.positionOffset_Y / 50), item);
			}
		}

		private void onClickedGrid(SleekGrid grid)
		{
			byte x = (byte)((PlayerUI.window.mouse_x + PlayerUI.window.frame.x - (float)base.positionOffset_X - base.parent.frame.x + ((SleekScrollBox)base.parent).state.x) / 50f);
			byte y = (byte)((PlayerUI.window.mouse_y + PlayerUI.window.frame.y - (float)base.positionOffset_Y - base.parent.frame.y + ((SleekScrollBox)base.parent).state.y) / 50f);
			if (this.onPlacedItem != null)
			{
				this.onPlacedItem(this.page, x, y);
			}
		}

		public SelectedItem onSelectedItem;

		public GrabbedItem onGrabbedItem;

		public PlacedItem onPlacedItem;

		private SleekGrid grid;

		private byte _page;

		private byte _width;

		private byte _height;

		private List<SleekItem> _items;
	}
}
