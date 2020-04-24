using System;
using SDG.Framework.UI.Sleek2;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SDG.Framework.UI.Components
{
	public class DragablePopoutContainer : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IEventSystemHandler
	{
		public void OnPointerDown(PointerEventData eventData)
		{
			if (Sleek2DragManager.isDragging)
			{
				return;
			}
			if (this.target == null)
			{
				return;
			}
			if (eventData.button != null)
			{
				return;
			}
			RectTransform rectTransform = this.target.parent as RectTransform;
			if (rectTransform == null)
			{
				return;
			}
			if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, ref this.offset))
			{
				return;
			}
			this.offset.x = (this.offset.x + rectTransform.rect.size.x * rectTransform.pivot.x) / rectTransform.rect.size.x;
			this.offset.y = (this.offset.y + rectTransform.rect.size.y * rectTransform.pivot.y) / rectTransform.rect.size.y;
			this.target.SetAsLastSibling();
		}

		public void OnBeginDrag(PointerEventData eventData)
		{
			if (Sleek2DragManager.isDragging)
			{
				return;
			}
			if (this.target == null)
			{
				return;
			}
			if (eventData.button != null)
			{
				return;
			}
			this.min = this.target.anchorMin;
			this.max = this.target.anchorMax;
			Sleek2DragManager.isDragging = true;
			this.isDragging = true;
		}

		public void OnDrag(PointerEventData eventData)
		{
			if (!this.isDragging)
			{
				return;
			}
			RectTransform rectTransform = this.target.parent as RectTransform;
			if (rectTransform == null)
			{
				return;
			}
			Vector2 vector;
			if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, ref vector))
			{
				return;
			}
			vector.x = (vector.x + rectTransform.rect.size.x * rectTransform.pivot.x) / rectTransform.rect.size.x;
			vector.y = (vector.y + rectTransform.rect.size.y * rectTransform.pivot.y) / rectTransform.rect.size.y;
			Vector2 vector2 = vector - this.offset;
			vector2.x = Mathf.Clamp(vector2.x, -this.min.x, 1f - this.max.x);
			vector2.y = Mathf.Clamp(vector2.y, -this.min.y, 1f - this.max.y);
			this.target.anchorMin = this.min + vector2;
			this.target.anchorMax = this.max + vector2;
		}

		public void OnEndDrag(PointerEventData eventData)
		{
			if (!this.isDragging)
			{
				return;
			}
			Sleek2DragManager.isDragging = false;
			this.isDragging = false;
		}

		private void OnDisable()
		{
			if (this.isDragging)
			{
				Sleek2DragManager.isDragging = false;
				this.isDragging = false;
			}
		}

		private void Reset()
		{
			this.target = (base.transform as RectTransform);
		}

		public RectTransform target;

		private bool isDragging;

		private Vector2 offset;

		private Vector2 min;

		private Vector2 max;
	}
}
