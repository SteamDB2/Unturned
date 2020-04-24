using System;
using SDG.Unturned;

namespace SDG.Framework.UI.Components
{
	public delegate void AssetReferenceDockedHandler<T>(AssetReference<T> assetReference) where T : Asset;
}
