using System;
using SDG.Framework.UI.Sleek2;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SDG.Framework.UI.Components
{
	public class DragableTab : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IEventSystemHandler
	{
		public event PopoutTabHandler popoutTab;

		public void OnPointerDown(PointerEventData eventData)
		{
			if (Sleek2DragManager.isDragging)
			{
				return;
			}
			this.offset = new Vector2(eventData.position.x - this.target.position.x, eventData.position.y - this.target.position.y);
		}

		public void OnBeginDrag(PointerEventData eventData)
		{
			if (Sleek2DragManager.isDragging)
			{
				return;
			}
			if (this.target == null || this.source == null)
			{
				return;
			}
			if (eventData.button != null)
			{
				return;
			}
			this.origin = this.target.localPosition;
			this.blockCanvas = this.target.gameObject.getOrAddComponent<Canvas>();
			this.blockCanvas.overrideSorting = true;
			this.blockCanvas.sortingOrder = 30000;
			Sleek2DragManager.isDragging = true;
			this.isDragging = true;
			Sleek2DragManager.item = this.source;
		}

		public void OnDrag(PointerEventData eventData)
		{
			if (!this.isDragging)
			{
				return;
			}
			Vector2 position = eventData.position;
			position.x = Mathf.Clamp(position.x, 0f, (float)Screen.width);
			position.y = Mathf.Clamp(position.y, 0f, (float)Screen.height);
			this.target.position = position - this.offset;
		}

		public void OnEndDrag(PointerEventData eventData)
		{
			if (!this.isDragging)
			{
				return;
			}
			if (!Sleek2DragManager.dropped)
			{
				this.target.localPosition = this.origin;
				this.triggerPopoutTab(eventData.position);
			}
			Object.Destroy(this.blockCanvas);
			Sleek2DragManager.isDragging = false;
			this.isDragging = false;
			Sleek2DragManager.item = null;
		}

		protected void triggerPopoutTab(Vector2 position)
		{
			if (this.popoutTab != null)
			{
				this.popoutTab(this, position);
			}
		}

		protected void OnDisable()
		{
			if (this.isDragging)
			{
				this.target.localPosition = this.origin;
				Object.Destroy(this.blockCanvas);
				Sleek2DragManager.isDragging = false;
				this.isDragging = false;
				Sleek2DragManager.item = null;
			}
		}

		private void Reset()
		{
			this.target = (base.transform as RectTransform);
		}

		public RectTransform target;

		public object source;

		private bool isDragging;

		private Vector2 offset;

		private Vector3 origin;

		private Canvas blockCanvas;
	}
}
