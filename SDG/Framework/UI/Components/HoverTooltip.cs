using System;
using SDG.Framework.UI.Sleek2;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SDG.Framework.UI.Components
{
	public class HoverTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IEventSystemHandler
	{
		public event BeginTooltip beginTooltip;

		public event EndTooltip endTooltip;

		public void OnPointerEnter(PointerEventData eventData)
		{
			this.element = this.triggerBeginTooltip();
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			this.triggerEndTooltip(this.element);
		}

		protected virtual Sleek2Element triggerBeginTooltip()
		{
			if (this.beginTooltip != null)
			{
				return this.beginTooltip(this);
			}
			return null;
		}

		protected virtual void triggerEndTooltip(Sleek2Element element)
		{
			if (this.endTooltip != null)
			{
				this.endTooltip(this, element);
			}
		}

		protected Sleek2Element element;
	}
}
