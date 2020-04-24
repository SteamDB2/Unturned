using System;
using SDG.Framework.Foliage;
using SDG.Framework.UI.Sleek2;
using UnityEngine;

namespace SDG.Framework.UI.Devkit.FoliageUI
{
	public class FoliageToolFoliageCollectionAssetButton : Sleek2ImageButton
	{
		public FoliageToolFoliageCollectionAssetButton(FoliageInfoCollectionAsset newAsset)
		{
			this.asset = newAsset;
			base.transform.anchorMin = new Vector2(0f, 1f);
			base.transform.anchorMax = new Vector2(1f, 1f);
			base.transform.pivot = new Vector2(0.5f, 1f);
			base.transform.sizeDelta = new Vector2(0f, 30f);
			Sleek2Label sleek2Label = new Sleek2Label();
			sleek2Label.transform.reset();
			sleek2Label.textComponent.text = this.asset.name;
			sleek2Label.textComponent.color = Sleek2Config.darkTextColor;
			this.addElement(sleek2Label);
		}

		public FoliageInfoCollectionAsset asset { get; protected set; }
	}
}
