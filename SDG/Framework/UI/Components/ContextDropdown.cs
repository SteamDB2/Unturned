using System;
using SDG.Framework.UI.Sleek2;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SDG.Framework.UI.Components
{
	public class ContextDropdown : MonoBehaviour, IPointerExitHandler, IEventSystemHandler
	{
		public void OnPointerExit(PointerEventData eventData)
		{
			this.element.destroy();
		}

		protected void Awake()
		{
			this.canvasOverride = base.gameObject.getOrAddComponent<Canvas>();
			this.canvasOverride.overrideSorting = true;
			this.canvasOverride.sortingOrder = 30000;
			this.raycasterOverride = base.gameObject.getOrAddComponent<GraphicRaycaster>();
		}

		public Sleek2Element element;

		protected Canvas canvasOverride;

		protected GraphicRaycaster raycasterOverride;
	}
}
