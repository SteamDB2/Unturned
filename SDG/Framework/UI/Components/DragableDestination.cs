using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SDG.Framework.UI.Components
{
	public class DragableDestination : MonoBehaviour, IDropHandler, IEventSystemHandler
	{
		public void OnDrop(PointerEventData eventData)
		{
			this.dropHandler.OnDrop(eventData);
		}

		public IDropHandler dropHandler;
	}
}
