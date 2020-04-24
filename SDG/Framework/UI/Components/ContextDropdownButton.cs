using System;
using SDG.Framework.UI.Sleek2;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SDG.Framework.UI.Components
{
	public class ContextDropdownButton : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		public event ContextDropdownOpenedHandler opened;

		public void OnPointerClick(PointerEventData eventData)
		{
			if (eventData.button != 1)
			{
				return;
			}
			Sleek2HoverDropdown sleek2HoverDropdown = new Sleek2HoverDropdown();
			sleek2HoverDropdown.name = "Context";
			this.element.addElement(sleek2HoverDropdown);
			sleek2HoverDropdown.transform.anchorMin = new Vector2(0.5f, 1f);
			sleek2HoverDropdown.transform.anchorMax = new Vector2(0.5f, 1f);
			sleek2HoverDropdown.transform.offsetMin = new Vector2(-100f, 0f);
			sleek2HoverDropdown.transform.offsetMax = new Vector2(100f, 0f);
			sleek2HoverDropdown.transform.pivot = new Vector2(0.5f, 1f);
			sleek2HoverDropdown.transform.position = eventData.position;
			sleek2HoverDropdown.transform.anchoredPosition += new Vector2(0f, (float)(Sleek2Config.bodyHeight / 2));
			sleek2HoverDropdown.transform.sizeDelta = new Vector2((float)Sleek2Config.tabWidth, 0f);
			sleek2HoverDropdown.transform.gameObject.AddComponent<ContextDropdown>().element = sleek2HoverDropdown;
			this.triggerOpened(sleek2HoverDropdown);
		}

		protected void triggerOpened(Sleek2HoverDropdown dropdown)
		{
			if (this.opened != null)
			{
				this.opened(this, dropdown);
			}
		}

		public Sleek2Element element;
	}
}
