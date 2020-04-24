using System;

namespace SDG.Framework.UI.Sleek2
{
	public class Sleek2DragManager
	{
		public static object item
		{
			get
			{
				return Sleek2DragManager._item;
			}
			set
			{
				if (Sleek2DragManager.item == value)
				{
					return;
				}
				Sleek2DragManager._item = value;
				Sleek2DragManager.dropped = false;
				Sleek2DragManager.triggerItemChanged();
			}
		}

		public static event Sleek2DragItemChangedHandler itemChanged;

		protected static void triggerItemChanged()
		{
			if (Sleek2DragManager.itemChanged != null)
			{
				Sleek2DragManager.itemChanged();
			}
		}

		protected static object _item;

		public static bool dropped;

		public static bool isDragging;
	}
}
