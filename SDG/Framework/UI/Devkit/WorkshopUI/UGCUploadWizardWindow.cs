using System;
using SDG.Framework.Debug;
using SDG.Framework.Translations;
using SDG.Framework.UI.Devkit.InspectorUI;
using SDG.Framework.UI.Sleek2;
using SDG.Provider;
using SDG.Unturned;
using UnityEngine;
using UnityEngine.UI;

namespace SDG.Framework.UI.Devkit.WorkshopUI
{
	public class UGCUploadWizardWindow : Sleek2Window
	{
		public UGCUploadWizardWindow()
		{
			base.gameObject.name = "UGC_Upload_Wizard";
			base.tab.label.translation = new TranslatedText(new TranslationReference("SDG", "Devkit.Window.UGC_Upload_Wizard.Title"));
			base.tab.label.translation.format();
			this.inspector = new Sleek2Inspector();
			this.inspector.transform.anchorMin = new Vector2(0f, 1f);
			this.inspector.transform.anchorMax = new Vector2(1f, 1f);
			this.inspector.transform.pivot = new Vector2(0f, 1f);
			this.inspector.transform.offsetMin = new Vector2(0f, -420f);
			this.inspector.transform.offsetMax = new Vector2(0f, 0f);
			this.inspector.inspect(this);
			base.safePanel.addElement(this.inspector);
			this.publishedFilesView = new Sleek2Scrollview();
			this.publishedFilesView.transform.anchorMin = new Vector2(0f, 1f);
			this.publishedFilesView.transform.anchorMax = new Vector2(1f, 1f);
			this.publishedFilesView.transform.pivot = new Vector2(0.5f, 1f);
			this.publishedFilesView.transform.offsetMin = new Vector2(0f, -560f);
			this.publishedFilesView.transform.offsetMax = new Vector2(0f, -460f);
			this.publishedFilesView.vertical = true;
			this.publishedFilesPanel = new Sleek2Element();
			this.publishedFilesPanel.name = "Panel";
			GridLayoutGroup gridLayoutGroup = this.publishedFilesPanel.gameObject.AddComponent<GridLayoutGroup>();
			gridLayoutGroup.cellSize = new Vector2(200f, 50f);
			gridLayoutGroup.spacing = new Vector2(5f, 5f);
			ContentSizeFitter contentSizeFitter = this.publishedFilesPanel.gameObject.AddComponent<ContentSizeFitter>();
			contentSizeFitter.verticalFit = 2;
			this.publishedFilesPanel.transform.reset();
			this.publishedFilesPanel.transform.pivot = new Vector2(0f, 1f);
			this.publishedFilesView.panel = this.publishedFilesPanel;
			base.safePanel.addElement(this.publishedFilesView);
			this.legalButton = new Sleek2ImageTranslatedLabelButton();
			this.legalButton.transform.anchorMin = new Vector2(0f, 1f);
			this.legalButton.transform.anchorMax = new Vector2(1f, 1f);
			this.legalButton.transform.pivot = new Vector2(0.5f, 1f);
			this.legalButton.transform.offsetMin = new Vector2(0f, -440f);
			this.legalButton.transform.offsetMax = new Vector2(0f, -420f);
			this.legalButton.label.translation = new TranslatedText(new TranslationReference("#SDG::Devkit.Window.UGC_Upload_Wizard.Legal.Label"));
			this.legalButton.label.translation.format();
			this.legalButton.clicked += this.handleLegalButtonClicked;
			base.safePanel.addElement(this.legalButton);
			this.uploadButton = new Sleek2ImageTranslatedLabelButton();
			this.uploadButton.transform.anchorMin = new Vector2(0f, 1f);
			this.uploadButton.transform.anchorMax = new Vector2(1f, 1f);
			this.uploadButton.transform.pivot = new Vector2(0.5f, 1f);
			this.uploadButton.transform.offsetMin = new Vector2(0f, -460f);
			this.uploadButton.transform.offsetMax = new Vector2(0f, -440f);
			this.uploadButton.label.translation = new TranslatedText(new TranslationReference("#SDG::Devkit.Window.UGC_Upload_Wizard.Upload.Label"));
			this.uploadButton.label.translation.format();
			this.uploadButton.clicked += this.handleUploadButtonClicked;
			base.safePanel.addElement(this.uploadButton);
			TempSteamworksWorkshop workshopService = Provider.provider.workshopService;
			workshopService.onPublishedAdded = (TempSteamworksWorkshop.PublishedAdded)Delegate.Combine(workshopService.onPublishedAdded, new TempSteamworksWorkshop.PublishedAdded(this.onPublishedAdded));
			TempSteamworksWorkshop workshopService2 = Provider.provider.workshopService;
			workshopService2.onPublishedRemoved = (TempSteamworksWorkshop.PublishedRemoved)Delegate.Combine(workshopService2.onPublishedRemoved, new TempSteamworksWorkshop.PublishedRemoved(this.onPublishedRemoved));
			this.onPublishedAdded();
		}

		protected virtual void handleLegalButtonClicked(Sleek2ImageButton button)
		{
			Provider.provider.browserService.open("http://steamcommunity.com/sharedfiles/workshoplegalagreement/?appid=304930");
		}

		protected virtual void handleUploadButtonClicked(Sleek2ImageButton button)
		{
			Provider.provider.workshopService.prepareUGC(this.itemName, this.itemDesc, this.contentPath.absolutePath, this.previewImagePath.absolutePath, this.changeNote, this.itemType, this.itemTag, this.visibility);
			Provider.provider.workshopService.createUGC(this.curated);
		}

		protected virtual void handlePublishedFileButtonClicked(Sleek2ImageButton button)
		{
			SteamPublished file = (button as UGCUploadPublishedFileButton).file;
			Provider.provider.workshopService.prepareUGC(this.itemName, this.itemDesc, this.contentPath.absolutePath, this.previewImagePath.absolutePath, this.changeNote, this.itemType, this.itemTag, this.visibility);
			Provider.provider.workshopService.prepareUGC(file.id);
			Provider.provider.workshopService.updateUGC();
		}

		protected virtual void onPublishedAdded()
		{
			for (int i = 0; i < Provider.provider.workshopService.published.Count; i++)
			{
				SteamPublished newFile = Provider.provider.workshopService.published[i];
				UGCUploadPublishedFileButton ugcuploadPublishedFileButton = new UGCUploadPublishedFileButton(newFile);
				ugcuploadPublishedFileButton.clicked += this.handlePublishedFileButtonClicked;
				this.publishedFilesPanel.addElement(ugcuploadPublishedFileButton);
			}
		}

		protected virtual void onPublishedRemoved()
		{
			this.publishedFilesPanel.clearElements();
		}

		public override void destroy()
		{
			TempSteamworksWorkshop workshopService = Provider.provider.workshopService;
			workshopService.onPublishedAdded = (TempSteamworksWorkshop.PublishedAdded)Delegate.Remove(workshopService.onPublishedAdded, new TempSteamworksWorkshop.PublishedAdded(this.onPublishedAdded));
			TempSteamworksWorkshop workshopService2 = Provider.provider.workshopService;
			workshopService2.onPublishedRemoved = (TempSteamworksWorkshop.PublishedRemoved)Delegate.Remove(workshopService2.onPublishedRemoved, new TempSteamworksWorkshop.PublishedRemoved(this.onPublishedRemoved));
			base.destroy();
		}

		[Inspectable("#SDG::Devkit.Window.UGC_Upload_Wizard.Item_Name.Name", null)]
		public string itemName;

		[Inspectable("#SDG::Devkit.Window.UGC_Upload_Wizard.Item_Desc.Name", null)]
		public string itemDesc;

		[Inspectable("#SDG::Devkit.Window.UGC_Upload_Wizard.Content_Path.Name", null)]
		public InspectableDirectoryPath contentPath;

		[Inspectable("#SDG::Devkit.Window.UGC_Upload_Wizard.Preview_Image_Path.Name", null)]
		public InspectableFilePath previewImagePath = new InspectableFilePath("*.png");

		[Inspectable("#SDG::Devkit.Window.UGC_Upload_Wizard.Change_Note.Name", null)]
		public string changeNote;

		[Inspectable("#SDG::Devkit.Window.UGC_Upload_Wizard.Item_Type.Name", null)]
		public ESteamUGCType itemType;

		[Inspectable("#SDG::Devkit.Window.UGC_Upload_Wizard.Item_Tag.Name", null)]
		public string itemTag;

		[Inspectable("#SDG::Devkit.Window.UGC_Upload_Wizard.Visibility.Name", null)]
		public ESteamUGCVisibility visibility;

		[Inspectable("#SDG::Devkit.Window.UGC_Upload_Wizard.Curated.Name", null)]
		public bool curated;

		protected Sleek2Inspector inspector;

		protected Sleek2Element publishedFilesPanel;

		protected Sleek2Scrollview publishedFilesView;

		protected Sleek2ImageTranslatedLabelButton legalButton;

		protected Sleek2ImageTranslatedLabelButton uploadButton;
	}
}
