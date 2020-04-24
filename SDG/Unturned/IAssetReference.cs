using System;
using SDG.Framework.Debug;

namespace SDG.Unturned
{
	public interface IAssetReference
	{
		[Inspectable("#SDG::Asset.AssetReference.GUID.Name", null)]
		Guid GUID { get; set; }

		bool isValid { get; }
	}
}
