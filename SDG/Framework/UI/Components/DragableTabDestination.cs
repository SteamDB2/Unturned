using System;
using SDG.Framework.UI.Sleek2;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SDG.Framework.UI.Components
{
	public class DragableTabDestination : MonoBehaviour, IDropHandler, IEventSystemHandler
	{
		public event TabDockedHandler tabDocked;

		public void OnDrop(PointerEventData eventData)
		{
			if (Sleek2DragManager.item is Sleek2WindowTab)
			{
				Sleek2DragManager.dropped = true;
				Vector2 vector;
				RectTransformUtility.ScreenPointToLocalPointInRectangle(base.transform as RectTransform, eventData.position, eventData.pressEventCamera, ref vector);
				this.triggerTabDocked(Sleek2DragManager.item as Sleek2WindowTab, vector.x);
			}
		}

		protected virtual void triggerTabDocked(Sleek2WindowTab tab, float offset)
		{
			if (this.tabDocked != null)
			{
				this.tabDocked(this.dock, tab, offset);
			}
		}

		public Sleek2WindowDock dock;
	}
}
