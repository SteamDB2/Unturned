using System;
using SDG.Framework.UI.Sleek2;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.UI.Devkit.ObjectBrowserUI
{
	public class ObjectBrowserAssetButton : Sleek2ImageLabelButton
	{
		public ObjectBrowserAssetButton(ObjectAsset newAsset)
		{
			this.asset = newAsset;
			base.transform.anchorMin = new Vector2(0f, 1f);
			base.transform.anchorMax = new Vector2(1f, 1f);
			base.transform.pivot = new Vector2(0.5f, 1f);
			base.transform.sizeDelta = new Vector2(0f, 30f);
			base.label.textComponent.text = this.asset.objectName;
		}

		public ObjectAsset asset { get; protected set; }
	}
}
