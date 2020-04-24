using System;
using System.IO;
using SDG.Framework.Debug;
using SDG.Framework.IO.Serialization;
using SDG.Framework.Translations;
using SDG.Framework.UI.Devkit.InspectorUI;
using SDG.Framework.UI.Sleek2;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.UI.Devkit.WorkshopUI
{
	public class SkinEconBundleWizardWindow : Sleek2Window
	{
		public SkinEconBundleWizardWindow()
		{
			base.gameObject.name = "UGC_Skin_Econ_Bundle_Wizard";
			base.tab.label.translation = new TranslatedText(new TranslationReference("SDG", "Devkit.Window.UGC_Skin_Econ_Bundle_Wizard.Title"));
			base.tab.label.translation.format();
			this.inspector = new Sleek2Inspector();
			this.inspector.transform.anchorMin = new Vector2(0f, 0f);
			this.inspector.transform.anchorMax = new Vector2(1f, 1f);
			this.inspector.transform.pivot = new Vector2(0f, 1f);
			this.inspector.transform.offsetMin = new Vector2(0f, 20f);
			this.inspector.transform.offsetMax = new Vector2(0f, 0f);
			this.inspector.inspect(this);
			base.safePanel.addElement(this.inspector);
			this.setupButton = new Sleek2ImageTranslatedLabelButton();
			this.setupButton.transform.anchorMin = new Vector2(0f, 0f);
			this.setupButton.transform.anchorMax = new Vector2(1f, 0f);
			this.setupButton.transform.pivot = new Vector2(0.5f, 1f);
			this.setupButton.transform.offsetMin = new Vector2(0f, 0f);
			this.setupButton.transform.offsetMax = new Vector2(0f, 20f);
			this.setupButton.label.translation = new TranslatedText(new TranslationReference("#SDG::Devkit.Window.UGC_Skin_Econ_Bundle_Wizard.Input.Label"));
			this.setupButton.label.translation.format();
			this.setupButton.clicked += this.handleSetupButtonClicked;
			base.safePanel.addElement(this.setupButton);
		}

		public static void setupEcon(EconItemDefinition econ, out int econID)
		{
			econID = 0;
			ItemAsset itemAsset = Assets.find(EAssetType.ITEM, econ.ItemID) as ItemAsset;
			if (itemAsset == null)
			{
				return;
			}
			econ.ItemName = new EconName(itemAsset.itemName, itemAsset.name);
			econ.Type = 8;
			econ.DefinitionID = econID;
			string text = "C:\\Unturned\\Economy\\ItemSchema\\Input\\Skins\\" + itemAsset.name + "/" + econ.SkinName.Private;
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
			}
			string path = text + "/ItemDefinitions.json";
			EconContainer econContainer = new EconContainer();
			econContainer.Items = new EconItemDefinition[]
			{
				econ
			};
			ISerializer serializer = new JSONSerializer();
			serializer.serialize<EconContainer>(econContainer, path, true);
		}

		public static void setupBundle(SkinCreatorOutput bundle, string patternID, out ushort skinID)
		{
			skinID = 0;
			ItemAsset itemAsset = Assets.find(EAssetType.ITEM, bundle.itemID) as ItemAsset;
			if (itemAsset == null)
			{
				return;
			}
			string text = "C:\\Unturned\\Builds\\Shared\\Bundles\\Skins\\" + itemAsset.name + "/" + patternID;
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
			}
			string path = text + "/" + patternID + ".dat";
			string text2 = "Type Skin\nID " + skinID + "\n\nSight\nTactical\nGrip\nBarrel\nMagazine";
			if (bundle.secondarySkins.Count > 0)
			{
				text2 = text2 + "\n\nSecondary_Skins " + bundle.secondarySkins.Count;
				for (int i = 0; i < bundle.secondarySkins.Count; i++)
				{
					SecondarySkinInfo secondarySkinInfo = bundle.secondarySkins[i];
					string text3 = text2;
					text2 = string.Concat(new object[]
					{
						text3,
						"\nSecondary_",
						i,
						" ",
						secondarySkinInfo.itemID
					});
				}
			}
			File.WriteAllText(path, text2);
		}

		protected virtual void handleSetupButtonClicked(Sleek2ImageButton button)
		{
		}

		[Inspectable("#SDG::Devkit.Window.UGC_Skin_Econ_Bundle_Wizard.Workshop_ID.Name", null)]
		public ulong workshopID;

		[Inspectable("#SDG::Devkit.Window.UGC_Skin_Econ_Bundle_Wizard.Pattern_ID.Name", null)]
		public string patternID;

		protected Sleek2Inspector inspector;

		protected Sleek2ImageTranslatedLabelButton setupButton;
	}
}
