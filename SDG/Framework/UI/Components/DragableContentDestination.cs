using System;
using SDG.Framework.UI.Sleek2;
using SDG.Unturned;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SDG.Framework.UI.Components
{
	public class DragableContentDestination<T> : IDropHandler, IEventSystemHandler where T : Object
	{
		public event ContentReferenceDockedHandler<T> contentReferenceDocked;

		public void OnDrop(PointerEventData eventData)
		{
			if (!(Sleek2DragManager.item is IContentReference))
			{
				return;
			}
			if (typeof(T).IsAssignableFrom(Sleek2DragManager.item.GetType().GetGenericArguments()[0]))
			{
				IContentReference contentReference = (IContentReference)Sleek2DragManager.item;
				this.triggerContentReferenceDocked(new ContentReference<T>(contentReference));
			}
		}

		protected virtual void triggerContentReferenceDocked(ContentReference<T> contentReference)
		{
			if (this.contentReferenceDocked != null)
			{
				this.contentReferenceDocked(contentReference);
			}
		}
	}
}
