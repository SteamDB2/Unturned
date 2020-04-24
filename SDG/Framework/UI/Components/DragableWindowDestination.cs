using System;
using SDG.Framework.UI.Sleek2;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SDG.Framework.UI.Components
{
	public class DragableWindowDestination : MonoBehaviour, IDropHandler, IEventSystemHandler
	{
		public event WindowDockedHandler windowDocked;

		public void OnDrop(PointerEventData eventData)
		{
			if (Sleek2DragManager.item is Sleek2WindowTab)
			{
				RectTransform rectTransform = base.transform as RectTransform;
				Rect rect = rectTransform.rect;
				Vector2 vector;
				RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, ref vector);
				float num;
				ESleek2PartitionDirection esleek2PartitionDirection;
				if (vector.x < rect.width / 2f)
				{
					num = vector.x;
					esleek2PartitionDirection = ESleek2PartitionDirection.LEFT;
				}
				else
				{
					num = rect.width - vector.x;
					esleek2PartitionDirection = ESleek2PartitionDirection.RIGHT;
				}
				float num2;
				ESleek2PartitionDirection esleek2PartitionDirection2;
				if (vector.y < rect.height / 2f)
				{
					num2 = vector.y;
					esleek2PartitionDirection2 = ESleek2PartitionDirection.DOWN;
				}
				else
				{
					num2 = rect.height - vector.y;
					esleek2PartitionDirection2 = ESleek2PartitionDirection.UP;
				}
				ESleek2PartitionDirection esleek2PartitionDirection3;
				if (num < 64f || num2 < 64f)
				{
					if (num < num2)
					{
						esleek2PartitionDirection3 = esleek2PartitionDirection;
					}
					else
					{
						esleek2PartitionDirection3 = esleek2PartitionDirection2;
					}
				}
				else
				{
					esleek2PartitionDirection3 = ESleek2PartitionDirection.NONE;
				}
				if (esleek2PartitionDirection3 != ESleek2PartitionDirection.NONE)
				{
					Sleek2WindowTab sleek2WindowTab = Sleek2DragManager.item as Sleek2WindowTab;
					if (sleek2WindowTab.window.dock != this.dock || sleek2WindowTab.window.dock.windows.Count > 1)
					{
						Sleek2DragManager.dropped = true;
						this.triggerWindowDocked(sleek2WindowTab, esleek2PartitionDirection3);
					}
				}
			}
		}

		protected virtual void triggerWindowDocked(Sleek2WindowTab tab, ESleek2PartitionDirection direction)
		{
			if (this.windowDocked != null)
			{
				this.windowDocked(this.dock, tab, direction);
			}
		}

		public Sleek2WindowDock dock;
	}
}
