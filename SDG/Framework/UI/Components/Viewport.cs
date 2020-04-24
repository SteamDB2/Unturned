using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SDG.Framework.UI.Components
{
	public class Viewport : UIBehaviour, IPointerEnterHandler, IPointerExitHandler, IEventSystemHandler
	{
		public static Rect screenRect { get; protected set; }

		public static bool hasPointer
		{
			get
			{
				return Viewport._hasPointer;
			}
			protected set
			{
				if (Viewport.hasPointer == value)
				{
					return;
				}
				bool hasPointer = Viewport.hasPointer;
				Viewport._hasPointer = value;
				Viewport.triggerHasPointerChanged(hasPointer, Viewport.hasPointer);
			}
		}

		public static event ViewportHasPointerChanged hasPointerChanged;

		protected static void triggerHasPointerChanged(bool oldHasPointer, bool newHasPointer)
		{
			if (Viewport.hasPointerChanged != null)
			{
				Viewport.hasPointerChanged(oldHasPointer, newHasPointer);
			}
		}

		public event ViewportDimensionsChangedHandler dimensionsChanged;

		protected void triggerDimensionsChanged()
		{
			if (this.dimensionsChanged != null)
			{
				this.dimensionsChanged(this);
			}
		}

		protected virtual void updateScreenRect()
		{
			Vector2 vector = Vector2.Scale(this.rectTransform.rect.size, this.rectTransform.lossyScale);
			Viewport.screenRect = new Rect(this.rectTransform.position.x - this.rectTransform.pivot.x * vector.x, this.rectTransform.position.y - this.rectTransform.pivot.y * vector.y, vector.x, vector.y);
		}

		protected override void OnRectTransformDimensionsChange()
		{
			this.updateScreenRect();
			int num = (int)Viewport.screenRect.width;
			int num2 = (int)Viewport.screenRect.height;
			if (this.width != num || this.height != num2)
			{
				this.width = num;
				this.height = num2;
				this.triggerDimensionsChanged();
			}
		}

		protected override void Awake()
		{
			this.rectTransform = (base.transform as RectTransform);
			this.updateScreenRect();
			this.width = (int)Viewport.screenRect.width;
			this.height = (int)Viewport.screenRect.height;
			base.Awake();
		}

		protected override void OnDisable()
		{
			if (this.containsPointer)
			{
				this.containsPointer = false;
				Viewport.hasPointer = false;
			}
			base.OnDisable();
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			if (!this.containsPointer)
			{
				this.containsPointer = true;
				Viewport.hasPointer = true;
			}
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			if (this.containsPointer)
			{
				this.containsPointer = false;
				Viewport.hasPointer = false;
			}
		}

		protected static bool _hasPointer;

		protected bool containsPointer;

		protected RectTransform rectTransform;

		protected int width;

		protected int height;
	}
}
