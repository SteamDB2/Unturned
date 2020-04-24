using System;
using SDG.Framework.UI.Sleek2;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SDG.Framework.UI.Components
{
	[AddComponentMenu("UI/Separator")]
	public class Separator : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerUpHandler, IEventSystemHandler
	{
		public bool aActive
		{
			get
			{
				return this._aActive;
			}
			set
			{
				if (this.aActive != value)
				{
					this._aActive = value;
					this.updateVisuals();
				}
			}
		}

		public bool bActive
		{
			get
			{
				return this._bActive;
			}
			set
			{
				if (this.bActive != value)
				{
					this._bActive = value;
					this.updateVisuals();
				}
			}
		}

		private void updateVisuals()
		{
			if (this.a == null || this.b == null)
			{
				return;
			}
			RectTransform rectTransform = base.transform as RectTransform;
			if (this.aActive != this.a.gameObject.activeSelf)
			{
				this.a.gameObject.SetActive(this.aActive);
			}
			if (this.bActive != this.b.gameObject.activeSelf)
			{
				this.b.gameObject.SetActive(this.bActive);
			}
			Image component = rectTransform.GetComponent<Image>();
			if (component != null && component.enabled != (this.aActive && this.bActive))
			{
				component.enabled = (this.aActive && this.bActive);
			}
			this.value = Mathf.Clamp(this.value, this.min, this.max);
			float num = this.value;
			float num2 = this.padding;
			if (!this.aActive)
			{
				num = 0f;
				num2 = 0f;
			}
			else if (!this.bActive)
			{
				num = 1f;
				num2 = 0f;
			}
			this.tracker.Clear();
			Separator.EDirection edirection = this.direction;
			if (edirection != Separator.EDirection.HORIZONTAL)
			{
				if (edirection == Separator.EDirection.VERTICAL)
				{
					this.tracker.Add(this, rectTransform, 16128);
					this.tracker.Add(this, this.a, 65286);
					this.tracker.Add(this, this.b, 65286);
					rectTransform.anchorMin = new Vector2(0f, num);
					rectTransform.anchorMax = new Vector2(1f, num);
					rectTransform.sizeDelta = new Vector2(0f, this.width);
					this.a.anchorMin = Vector2.zero;
					this.a.anchorMax = new Vector2(1f, num);
					this.a.sizeDelta = new Vector2(0f, -num2);
					this.a.anchoredPosition = Vector2.zero;
					this.a.pivot = new Vector2(0.5f, 0f);
					this.b.anchorMin = new Vector2(0f, num);
					this.b.anchorMax = Vector2.one;
					this.b.sizeDelta = new Vector2(0f, -num2);
					this.b.anchoredPosition = new Vector2(0f, num2);
					this.b.pivot = new Vector2(0.5f, 0f);
				}
			}
			else
			{
				this.tracker.Add(this, rectTransform, 16128);
				this.tracker.Add(this, this.a, 65286);
				this.tracker.Add(this, this.b, 65286);
				rectTransform.anchorMin = new Vector2(num, 0f);
				rectTransform.anchorMax = new Vector2(num, 1f);
				rectTransform.sizeDelta = new Vector2(this.width, 0f);
				this.a.anchorMin = Vector2.zero;
				this.a.anchorMax = new Vector2(num, 1f);
				this.a.sizeDelta = new Vector2(-num2, 0f);
				this.a.anchoredPosition = Vector2.zero;
				this.a.pivot = new Vector2(0f, 0.5f);
				this.b.anchorMin = new Vector2(num, 0f);
				this.b.anchorMax = Vector2.one;
				this.b.sizeDelta = new Vector2(-num2, 0f);
				this.b.anchoredPosition = new Vector2(num2, 0f);
				this.b.pivot = new Vector2(0f, 0.5f);
			}
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			if (Sleek2DragManager.isDragging)
			{
				return;
			}
			Sleek2Pointer.cursor = ((this.direction != Separator.EDirection.HORIZONTAL) ? Resources.Load<Texture2D>("UI/Cursors/Cursor_Vertical") : Resources.Load<Texture2D>("UI/Cursors/Cursor_Horizontal"));
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
			RectTransform rectTransform = base.transform as RectTransform;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, ref this.offset);
			this.isCursor = true;
		}

		public void OnBeginDrag(PointerEventData eventData)
		{
			if (Sleek2DragManager.isDragging)
			{
				return;
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
			RectTransform rectTransform = base.transform.parent as RectTransform;
			if (rectTransform == null)
			{
				return;
			}
			Vector2 vector;
			if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, ref vector))
			{
				return;
			}
			Separator.EDirection edirection = this.direction;
			if (edirection != Separator.EDirection.HORIZONTAL)
			{
				if (edirection == Separator.EDirection.VERTICAL)
				{
					this.value = (vector.y - this.offset.y + rectTransform.rect.size.y * rectTransform.pivot.y) / rectTransform.rect.size.y;
				}
			}
			else
			{
				this.value = (vector.x - this.offset.x + rectTransform.rect.size.x * rectTransform.pivot.x) / rectTransform.rect.size.x;
			}
			this.updateVisuals();
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
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			this.isCursor = false;
		}

		private void Start()
		{
			this.updateVisuals();
		}

		private void OnValidate()
		{
			this.updateVisuals();
		}

		private void OnDisable()
		{
			this.tracker.Clear();
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

		public Separator.EDirection direction;

		public float min;

		public float max;

		public float value;

		public float width;

		public float padding;

		private bool isDragging;

		private bool isCursor;

		private bool isHovering;

		[SerializeField]
		private bool _aActive = true;

		[SerializeField]
		private bool _bActive = true;

		public RectTransform a;

		public RectTransform b;

		private DrivenRectTransformTracker tracker;

		private Vector2 offset;

		public enum EDirection
		{
			HORIZONTAL,
			VERTICAL
		}
	}
}
