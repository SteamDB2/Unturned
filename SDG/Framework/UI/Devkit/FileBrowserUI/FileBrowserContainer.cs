using System;
using System.IO;
using SDG.Framework.Translations;
using SDG.Framework.UI.Sleek2;
using UnityEngine;
using UnityEngine.UI;

namespace SDG.Framework.UI.Devkit.FileBrowserUI
{
	public class FileBrowserContainer : Sleek2PopoutContainer
	{
		public FileBrowserContainer()
		{
			base.name = "File_Browser";
			base.titlebar.titleLabel.translation = new TranslatedText(new TranslationReference("#SDG::Devkit.Window.File_Browser.Title"));
			base.titlebar.titleLabel.translation.format();
			this.itemsBox = new Sleek2Element();
			this.itemsBox.name = "Items";
			this.itemsBox.transform.reset();
			base.bodyPanel.addElement(this.itemsBox);
			this.pathPanel = new Sleek2HorizontalScrollviewContents();
			this.pathPanel.name = "Path";
			this.pathPanel.transform.anchorMin = new Vector2(0f, 1f);
			this.pathPanel.transform.anchorMax = new Vector2(1f, 1f);
			this.pathPanel.transform.pivot = new Vector2(0f, 1f);
			this.pathPanel.transform.offsetMin = new Vector2(5f, -55f);
			this.pathPanel.transform.offsetMax = new Vector2(5f, -5f);
			this.itemsBox.addElement(this.pathPanel);
			this.itemsView = new Sleek2Scrollview();
			this.itemsView.transform.reset();
			this.itemsView.transform.offsetMin = new Vector2(5f, (float)(Sleek2Config.bodyHeight + 5));
			this.itemsView.transform.offsetMax = new Vector2(-5f, -60f);
			this.itemsView.vertical = true;
			this.itemsPanel = new Sleek2Element();
			this.itemsPanel.name = "Panel";
			GridLayoutGroup gridLayoutGroup = this.itemsPanel.gameObject.AddComponent<GridLayoutGroup>();
			gridLayoutGroup.cellSize = new Vector2(200f, 50f);
			gridLayoutGroup.spacing = new Vector2(5f, 5f);
			ContentSizeFitter contentSizeFitter = this.itemsPanel.gameObject.AddComponent<ContentSizeFitter>();
			contentSizeFitter.verticalFit = 2;
			this.itemsPanel.transform.reset();
			this.itemsPanel.transform.pivot = new Vector2(0f, 1f);
			this.itemsView.panel = this.itemsPanel;
			this.itemsBox.addElement(this.itemsView);
			this.filePathLabel = new Sleek2Label();
			this.filePathLabel.transform.anchorMin = new Vector2(0f, 0f);
			this.filePathLabel.transform.anchorMax = new Vector2(1f, 0f);
			this.filePathLabel.transform.pivot = new Vector2(0f, 0f);
			this.filePathLabel.transform.offsetMin = new Vector2(200f, 0f);
			this.filePathLabel.transform.offsetMax = new Vector2(-200f, (float)Sleek2Config.bodyHeight);
			base.bodyPanel.addElement(this.filePathLabel);
			this.cancelButton = new Sleek2ImageButton();
			this.cancelButton.transform.anchorMin = new Vector2(0f, 0f);
			this.cancelButton.transform.anchorMax = new Vector2(0f, 0f);
			this.cancelButton.transform.pivot = new Vector2(0f, 0f);
			this.cancelButton.transform.sizeDelta = new Vector2(200f, (float)Sleek2Config.bodyHeight);
			this.cancelButton.clicked += this.handleCancelButtonClicked;
			base.bodyPanel.addElement(this.cancelButton);
			Sleek2TranslatedLabel sleek2TranslatedLabel = new Sleek2TranslatedLabel();
			sleek2TranslatedLabel.transform.reset();
			sleek2TranslatedLabel.translation = new TranslatedText(new TranslationReference("SDG", "Devkit.Window.File_Browser.Cancel"));
			sleek2TranslatedLabel.translation.format();
			sleek2TranslatedLabel.textComponent.color = Sleek2Config.darkTextColor;
			this.cancelButton.addElement(sleek2TranslatedLabel);
			this.submitButton = new Sleek2ImageButton();
			this.submitButton.transform.anchorMin = new Vector2(1f, 0f);
			this.submitButton.transform.anchorMax = new Vector2(1f, 0f);
			this.submitButton.transform.pivot = new Vector2(1f, 0f);
			this.submitButton.transform.sizeDelta = new Vector2(200f, (float)Sleek2Config.bodyHeight);
			this.submitButton.clicked += this.handleSubmitButtonClicked;
			base.bodyPanel.addElement(this.submitButton);
			Sleek2TranslatedLabel sleek2TranslatedLabel2 = new Sleek2TranslatedLabel();
			sleek2TranslatedLabel2.transform.reset();
			sleek2TranslatedLabel2.translation = new TranslatedText(new TranslationReference("SDG", "Devkit.Window.File_Browser.Submit"));
			sleek2TranslatedLabel2.translation.format();
			sleek2TranslatedLabel2.textComponent.color = Sleek2Config.darkTextColor;
			this.submitButton.addElement(sleek2TranslatedLabel2);
			this.mode = EFileBrowserMode.FILE;
			this.searchPattern = null;
			this.currentFilePath = null;
			this.browse(FileBrowserContainer.currentDirectoryPath);
		}

		protected string currentFilePath
		{
			get
			{
				return this._currentFilePath;
			}
			set
			{
				this._currentFilePath = value;
				this.filePathLabel.textComponent.text = ((this.currentFilePath == null) ? "---" : this.currentFilePath);
			}
		}

		protected void browse(string path)
		{
			this.pathPanel.clearElements();
			this.itemsPanel.clearElements();
			this.itemsPanel.transform.offsetMin = new Vector2(0f, 0f);
			this.itemsPanel.transform.offsetMax = new Vector2(0f, 0f);
			if (string.IsNullOrEmpty(path))
			{
				return;
			}
			DirectoryInfo directoryInfo = new DirectoryInfo(path);
			DirectoryInfo[] directories = directoryInfo.GetDirectories();
			foreach (DirectoryInfo newDirectory in directories)
			{
				FileBrowserDirectoryButton fileBrowserDirectoryButton = new FileBrowserDirectoryButton(newDirectory);
				fileBrowserDirectoryButton.clicked += this.handleDirectoryButtonClicked;
				this.itemsPanel.addElement(fileBrowserDirectoryButton);
			}
			if (this.mode == EFileBrowserMode.FILE)
			{
				FileInfo[] files;
				if (string.IsNullOrEmpty(this.searchPattern))
				{
					files = directoryInfo.GetFiles();
				}
				else
				{
					files = directoryInfo.GetFiles(this.searchPattern);
				}
				foreach (FileInfo newFile in files)
				{
					FileBrowserFileButton fileBrowserFileButton = new FileBrowserFileButton(newFile);
					fileBrowserFileButton.clicked += this.handleFileButtonClicked;
					this.itemsPanel.addElement(fileBrowserFileButton);
				}
			}
			do
			{
				FileBrowserDirectoryButton fileBrowserDirectoryButton2 = new FileBrowserDirectoryButton(directoryInfo);
				fileBrowserDirectoryButton2.transform.anchorMin = new Vector2(0f, 0f);
				fileBrowserDirectoryButton2.transform.anchorMax = new Vector2(0f, 1f);
				fileBrowserDirectoryButton2.transform.pivot = new Vector2(0f, 1f);
				fileBrowserDirectoryButton2.transform.sizeDelta = new Vector2(150f, 0f);
				fileBrowserDirectoryButton2.clicked += this.handleDirectoryButtonClicked;
				this.pathPanel.addElement(fileBrowserDirectoryButton2, 0);
				if (directoryInfo.Parent != null)
				{
					Sleek2Label sleek2Label = new Sleek2Label();
					sleek2Label.transform.anchorMin = new Vector2(0f, 0f);
					sleek2Label.transform.anchorMax = new Vector2(0f, 1f);
					sleek2Label.transform.pivot = new Vector2(0f, 1f);
					sleek2Label.transform.sizeDelta = new Vector2(30f, 0f);
					sleek2Label.textComponent.text = ">";
					this.pathPanel.addElement(sleek2Label, 0);
				}
			}
			while ((directoryInfo = directoryInfo.Parent) != null);
		}

		protected void handleFileButtonClicked(Sleek2ImageButton button)
		{
			this.currentFilePath = (button as FileBrowserFileButton).file.ToString();
		}

		protected void handleDirectoryButtonClicked(Sleek2ImageButton button)
		{
			string currentFilePath = (button as FileBrowserDirectoryButton).directory.ToString();
			if (this.mode == EFileBrowserMode.DIRECTORY)
			{
				this.currentFilePath = currentFilePath;
			}
			FileBrowserContainer.currentDirectoryPath = currentFilePath;
			this.browse(FileBrowserContainer.currentDirectoryPath);
		}

		protected virtual void handleCancelButtonClicked(Sleek2ImageButton button)
		{
			DevkitWindowManager.removeContainer(this);
		}

		protected virtual void handleSubmitButtonClicked(Sleek2ImageButton button)
		{
			this.triggerSelected(this.currentFilePath);
			DevkitWindowManager.removeContainer(this);
		}

		protected virtual void triggerSelected(string path)
		{
			if (this.selected != null)
			{
				this.selected(this, path);
			}
		}

		private static string currentDirectoryPath = "C:/";

		public EFileBrowserMode mode;

		public string searchPattern;

		public FileBrowserSelectedHandler selected;

		protected string _currentFilePath;

		protected Sleek2HorizontalScrollviewContents pathPanel;

		protected Sleek2Element itemsBox;

		protected Sleek2Element itemsPanel;

		protected Sleek2Scrollview itemsView;

		protected Sleek2Label filePathLabel;

		protected Sleek2ImageButton cancelButton;

		protected Sleek2ImageButton submitButton;
	}
}
