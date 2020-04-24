using System;
using SDG.Framework.Translations;
using SDG.Framework.UI.Sleek2;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.UI.Devkit.WorkshopUI
{
	public class ObjectsUpgradeWizardWindow : Sleek2Window
	{
		public ObjectsUpgradeWizardWindow()
		{
			base.gameObject.name = "Objects_Upgrade_Wizard";
			base.tab.label.translation = new TranslatedTextFallback("Objects Upgrade Wizard");
			base.tab.label.translation.format();
			this.upgradeButton = new Sleek2ImageLabelButton();
			this.upgradeButton.transform.anchorMin = new Vector2(0f, 1f);
			this.upgradeButton.transform.anchorMax = new Vector2(1f, 1f);
			this.upgradeButton.transform.pivot = new Vector2(0.5f, 1f);
			this.upgradeButton.transform.offsetMin = new Vector2(0f, -20f);
			this.upgradeButton.transform.offsetMax = new Vector2(0f, 0f);
			this.upgradeButton.label.textComponent.text = "Upgrade";
			this.upgradeButton.clicked += this.handleUpgradeButtonClicked;
			base.safePanel.addElement(this.upgradeButton);
		}

		protected virtual void handleUpgradeButtonClicked(Sleek2ImageButton button)
		{
			for (byte b = 0; b < Regions.WORLD_SIZE; b += 1)
			{
				for (byte b2 = 0; b2 < Regions.WORLD_SIZE; b2 += 1)
				{
					foreach (LevelObject levelObject in LevelObjects.objects[(int)b, (int)b2])
					{
						if (levelObject.placementOrigin == ELevelObjectPlacementOrigin.MANUAL)
						{
							LevelObjects.addDevkitObject(levelObject.GUID, levelObject.transform.position, levelObject.transform.rotation, levelObject.transform.localScale, levelObject.placementOrigin);
						}
					}
				}
			}
		}

		protected Sleek2ImageLabelButton upgradeButton;
	}
}
