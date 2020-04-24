using System;
using SDG.Framework.UI.Sleek2;
using SDG.Unturned;
using UnityEngine.EventSystems;

namespace SDG.Framework.UI.Components
{
	public class DragableTypeDestination<T> : IDropHandler, IEventSystemHandler
	{
		public event TypeReferenceDockedHandler<T> typeReferenceDocked;

		public void OnDrop(PointerEventData eventData)
		{
			if (!(Sleek2DragManager.item is ITypeReference))
			{
				return;
			}
			if (typeof(T).IsAssignableFrom(Sleek2DragManager.item.GetType().GetGenericArguments()[0]))
			{
				ITypeReference typeReference = (ITypeReference)Sleek2DragManager.item;
				this.triggerTypeReferenceDocked(new TypeReference<T>(typeReference));
			}
		}

		protected virtual void triggerTypeReferenceDocked(TypeReference<T> typeReference)
		{
			if (this.typeReferenceDocked != null)
			{
				this.typeReferenceDocked(typeReference);
			}
		}
	}
}
