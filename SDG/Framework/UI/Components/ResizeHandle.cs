using System;
using System.Collections.Generic;
using SDG.Framework.UI.Sleek2;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SDG.Framework.UI.Components
{
	public class ResizeHandle : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerUpHandler, IEventSystemHandler
	{
		public void OnPointerEnter(PointerEventData eventData)
		{
			if (Sleek2DragManager.isDragging)
			{
				return;
			}
			if ((this.horizontalPosition && this.verticalPosition) || (this.horizontalSize && this.verticalSize))
			{
				Sleek2Pointer.cursor = Resources.Load<Texture2D>("UI/Cursors/Cursor_Diagonal_45");
			}
			else if ((this.horizontalPosition && this.verticalSize) || (this.horizontalSize && this.verticalPosition))
			{
				Sleek2Pointer.cursor = Resources.Load<Texture2D>("UI/Cursors/Cursor_Diagonal_135");
			}
			else if (this.horizontalPosition || this.horizontalSize)
			{
				Sleek2Pointer.cursor = Resources.Load<Texture2D>("UI/Cursors/Cursor_Horizontal");
			}
			else if (this.verticalPosition || this.verticalSize)
			{
				Sleek2Pointer.cursor = Resources.Load<Texture2D>("UI/Cursors/Cursor_Vertical");
			}
			Sleek2Pointer.hotspot = new Vector2(10f, 10f);
			this.isHovering = true;
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			if (Sleek2DragManager.isDragging || this.isCursor)
			{
				return;
			}
			Sleek2Pointer.cursor = null;
			this.isHovering = false;
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			if (Sleek2DragManager.isDragging)
			{
				return;
			}
			RectTransform rectTransform = this.targetTransform.parent as RectTransform;
			if (rectTransform == null)
			{
				return;
			}
			if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, ref this.offset))
			{
				return;
			}
			this.offset.x = this.offset.x - this.targetTransform.localPosition.x;
			if (this.horizontalSize)
			{
				this.offset.x = this.offset.x - this.targetTransform.rect.size.x;
			}
			this.offset.y = this.offset.y - this.targetTransform.localPosition.y;
			if (this.verticalSize)
			{
				this.offset.y = this.offset.y - this.targetTransform.rect.size.y;
			}
			this.isCursor = true;
		}

		public void OnBeginDrag(PointerEventData eventData)
		{
			if (Sleek2DragManager.isDragging)
			{
				return;
			}
			this.targetTransform.GetComponentsInChildren<LayoutGroup>(false, ResizeHandle.layoutGroups);
			for (int i = ResizeHandle.layoutGroups.Count - 1; i >= 0; i--)
			{
				if (!ResizeHandle.layoutGroups[i].enabled)
				{
					ResizeHandle.layoutGroups.RemoveAt(i);
				}
				else
				{
					ResizeHandle.layoutGroups[i].enabled = false;
				}
			}
			Sleek2DragManager.isDragging = true;
			this.isDragging = true;
		}

		public void OnDrag(PointerEventData eventData)
		{
			if (!this.isDragging)
			{
				return;
			}
			RectTransform rectTransform = this.targetTransform.parent as RectTransform;
			if (rectTransform == null)
			{
				return;
			}
			Vector2 vector;
			if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, ref vector))
			{
				return;
			}
			if (this.horizontalPosition)
			{
				this.targetTransform.anchorMin = new Vector2(Mathf.Clamp((vector.x - this.offset.x + rectTransform.rect.size.x * rectTransform.pivot.x) / rectTransform.rect.size.x, 0f, Mathf.Max(this.targetTransform.anchorMax.x - this.min, 0f)), this.targetTransform.anchorMin.y);
			}
			if (this.horizontalSize)
			{
				this.targetTransform.anchorMax = new Vector2(Mathf.Clamp((vector.x - this.offset.x + rectTransform.rect.size.x * rectTransform.pivot.x) / rectTransform.rect.size.x, Mathf.Min(this.targetTransform.anchorMin.x + this.min, 1f), 1f), this.targetTransform.anchorMax.y);
			}
			if (this.verticalPosition)
			{
				this.targetTransform.anchorMin = new Vector2(this.targetTransform.anchorMin.x, Mathf.Clamp((vector.y - this.offset.y + rectTransform.rect.size.y * rectTransform.pivot.y) / rectTransform.rect.size.y, 0f, Mathf.Max(this.targetTransform.anchorMax.y - this.min, 0f)));
			}
			if (this.verticalSize)
			{
				this.targetTransform.anchorMax = new Vector2(this.targetTransform.anchorMax.x, Mathf.Clamp((vector.y - this.offset.y + rectTransform.rect.size.y * rectTransform.pivot.y) / rectTransform.rect.size.y, Mathf.Min(this.targetTransform.anchorMin.y + this.min, 1f), 1f));
			}
		}

		public void OnEndDrag(PointerEventData eventData)
		{
			if (!this.isDragging)
			{
				return;
			}
			Sleek2DragManager.isDragging = false;
			this.isDragging = false;
			Sleek2Pointer.cursor = null;
			for (int i = 0; i < ResizeHandle.layoutGroups.Count; i++)
			{
				ResizeHandle.layoutGroups[i].enabled = true;
			}
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			this.isCursor = false;
			this.isHovering = false;
		}

		private void OnDisable()
		{
			if (this.isDragging)
			{
				Sleek2DragManager.isDragging = false;
				this.isDragging = false;
			}
			if (this.isCursor || this.isHovering)
			{
				this.isCursor = false;
				this.isHovering = false;
				Sleek2Pointer.cursor = null;
			}
		}

		private void Reset()
		{
			this.targetTransform = (base.transform as RectTransform);
		}

		private static List<LayoutGroup> layoutGroups = new List<LayoutGroup>();

		public RectTransform targetTransform;

		public bool horizontalPosition;

		public bool horizontalSize;

		public bool verticalPosition;

		public bool verticalSize;

		public float min = 0.1f;

		private Vector2 offset;

		private bool isDragging;

		private bool isCursor;

		private bool isHovering;
	}
}
