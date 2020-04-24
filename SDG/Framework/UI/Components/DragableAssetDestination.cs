using System;
using SDG.Framework.UI.Sleek2;
using SDG.Unturned;
using UnityEngine.EventSystems;

namespace SDG.Framework.UI.Components
{
	public class DragableAssetDestination<T> : IDropHandler, IEventSystemHandler where T : Asset
	{
		public event AssetReferenceDockedHandler<T> assetReferenceDocked;

		public void OnDrop(PointerEventData eventData)
		{
			if (!(Sleek2DragManager.item is IAssetReference))
			{
				return;
			}
			if (typeof(T).IsAssignableFrom(Sleek2DragManager.item.GetType().GetGenericArguments()[0]))
			{
				IAssetReference assetReference = (IAssetReference)Sleek2DragManager.item;
				this.triggerAssetReferenceDocked(new AssetReference<T>(assetReference));
			}
		}

		protected virtual void triggerAssetReferenceDocked(AssetReference<T> assetReference)
		{
			if (this.assetReferenceDocked != null)
			{
				this.assetReferenceDocked(assetReference);
			}
		}
	}
}
