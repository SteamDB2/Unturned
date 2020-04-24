using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SDG.Framework.UI.Components
{
	public class HoverDropdownButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IEventSystemHandler
	{
		public void OnPointerEnter(PointerEventData eventData)
		{
			if (this.dropdown == null)
			{
				return;
			}
			bool flag = true;
			if (base.transform.parent != null && base.transform.parent.parent != null && base.transform.parent.parent.GetComponent<HoverDropdownButton>() == null)
			{
				this.canvasOverride = base.gameObject.getOrAddComponent<Canvas>();
				this.canvasOverride.overrideSorting = true;
				this.canvasOverride.sortingOrder = 30000;
				this.raycasterOverride = base.gameObject.getOrAddComponent<GraphicRaycaster>();
				flag = false;
			}
			this.tracker.Clear();
			this.tracker.Add(this, this.dropdown, 52992);
			if (flag)
			{
				this.dropdown.anchorMin = new Vector2(1f, 1f);
				this.dropdown.anchorMax = new Vector2(2f, 1f);
			}
			else
			{
				this.dropdown.anchorMin = Vector2.zero;
				this.dropdown.anchorMax = new Vector2(1f, 0f);
			}
			this.dropdown.pivot = new Vector2(0.5f, 1f);
			this.dropdown.gameObject.SetActive(true);
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			if (this.dropdown == null)
			{
				return;
			}
			this.dropdown.gameObject.SetActive(false);
			if (this.raycasterOverride != null)
			{
				Object.Destroy(this.raycasterOverride);
			}
			if (this.canvasOverride != null)
			{
				Object.Destroy(this.canvasOverride);
			}
			base.GetComponentsInChildren<IPointerExitHandler>(true, HoverDropdownButton.handlers);
			for (int i = 0; i < HoverDropdownButton.handlers.Count; i++)
			{
				if (HoverDropdownButton.handlers[i] != this)
				{
					HoverDropdownButton.handlers[i].OnPointerExit(eventData);
				}
			}
		}

		protected void OnDisable()
		{
			this.tracker.Clear();
		}

		protected static List<IPointerExitHandler> handlers = new List<IPointerExitHandler>();

		public RectTransform dropdown;

		protected Canvas canvasOverride;

		protected GraphicRaycaster raycasterOverride;

		protected DrivenRectTransformTracker tracker;
	}
}
