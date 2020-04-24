using System;
using SDG.Framework.UI.Sleek2;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.UI.Devkit.LandscapeUI
{
	public class LandscapeToolRemoveMaterialButton : Sleek2ImageButton
	{
		public LandscapeToolRemoveMaterialButton()
		{
			base.transform.anchorMin = new Vector2(0f, 1f);
			base.transform.anchorMax = new Vector2(1f, 1f);
			base.transform.pivot = new Vector2(0.5f, 1f);
			base.transform.sizeDelta = new Vector2(0f, 30f);
			this.label = new Sleek2Label();
			this.label.transform.reset();
			this.label.textComponent.color = Sleek2Config.darkTextColor;
			this.addElement(this.label);
		}

		public int layer { get; protected set; }

		public void update(int newLayer, AssetReference<LandscapeMaterialAsset> newAsset)
		{
			this.layer = newLayer;
			LandscapeMaterialAsset landscapeMaterialAsset = Assets.find<LandscapeMaterialAsset>(newAsset);
			if (landscapeMaterialAsset != null)
			{
				this.label.textComponent.text = landscapeMaterialAsset.name;
			}
			else
			{
				this.label.textComponent.text = "---";
			}
		}

		protected Sleek2Label label;
	}
}
