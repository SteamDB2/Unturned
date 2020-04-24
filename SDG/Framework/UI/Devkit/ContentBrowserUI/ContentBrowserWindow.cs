using System;
using System.Collections.Generic;
using SDG.Framework.IO.FormattedFiles;
using SDG.Framework.Translations;
using SDG.Framework.UI.Sleek2;
using SDG.Unturned;
using UnityEngine;
using UnityEngine.UI;

namespace SDG.Framework.UI.Devkit.ContentBrowserUI
{
	public class ContentBrowserWindow : Sleek2Window
	{
		public ContentBrowserWindow()
		{
			base.gameObject.name = "Content_Browser";
			base.tab.label.translation = new TranslatedText(new TranslationReference("SDG", "Devkit.Window.Content_Browser.Title"));
			base.tab.label.translation.format();
			this.rootsBox = new Sleek2Element();
			this.rootsBox.name = "Roots";
			this.addElement(this.rootsBox);
			this.itemsBox = new Sleek2Element();
			this.itemsBox.name = "Items";
			this.addElement(this.itemsBox);
			this.separator = new Sleek2Separator();
			this.separator.handle.value = 0.25f;
			this.separator.handle.a = this.rootsBox.transform;
			this.separator.handle.b = this.itemsBox.transform;
			this.addElement(this.separator);
			this.rootsView = new Sleek2Scrollview();
			this.rootsView.transform.reset();
			this.rootsView.transform.offsetMin = new Vector2(5f, 5f);
			this.rootsView.transform.offsetMax = new Vector2(-5f, -5f);
			this.rootsView.vertical = true;
			this.rootsPanel = new Sleek2VerticalScrollviewContents();
			this.rootsPanel.name = "Panel";
			this.rootsView.panel = this.rootsPanel;
			this.rootsBox.addElement(this.rootsView);
			foreach (KeyValuePair<string, RootContentDirectory> keyValuePair in Assets.rootContentDirectories)
			{
				RootContentDirectory value = keyValuePair.Value;
				ContentBrowserRootDirectoryButton contentBrowserRootDirectoryButton = new ContentBrowserRootDirectoryButton(value);
				contentBrowserRootDirectoryButton.clicked += this.handleRootDirectoryButtonClicked;
				this.rootsPanel.addElement(contentBrowserRootDirectoryButton);
			}
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
			this.itemsView.transform.offsetMin = new Vector2(5f, 5f);
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
			ContentBrowserWindow.browsed += this.handleBrowsed;
			this.handleBrowsed(ContentBrowserWindow.currentDirectory);
		}

		protected static event ContentBrowserWindow.ContentBrowserWindowBrowsedHandler browsed;

		public static void browse(ContentDirectory directory)
		{
			ContentBrowserWindow.currentDirectory = directory;
			if (ContentBrowserWindow.browsed != null)
			{
				ContentBrowserWindow.browsed(ContentBrowserWindow.currentDirectory);
			}
		}

		protected override void readWindow(IFormattedFileReader reader)
		{
			this.separator.handle.value = reader.readValue<float>("Split");
		}

		protected override void writeWindow(IFormattedFileWriter writer)
		{
			writer.writeValue<float>("Split", this.separator.handle.value);
		}

		protected override void triggerDestroyed()
		{
			ContentBrowserWindow.browsed -= this.handleBrowsed;
			base.triggerDestroyed();
		}

		protected void handleBrowsed(ContentDirectory directory)
		{
			this.pathPanel.clearElements();
			this.itemsPanel.clearElements();
			this.itemsPanel.transform.offsetMin = new Vector2(0f, 0f);
			this.itemsPanel.transform.offsetMax = new Vector2(0f, 0f);
			if (directory == null)
			{
				return;
			}
			for (int i = 0; i < directory.files.Count; i++)
			{
				ContentBrowserFileButton element = new ContentBrowserFileButton(directory.files[i]);
				this.itemsPanel.addElement(element);
			}
			foreach (KeyValuePair<string, ContentDirectory> keyValuePair in directory.directories)
			{
				ContentDirectory value = keyValuePair.Value;
				ContentBrowserDirectoryButton contentBrowserDirectoryButton = new ContentBrowserDirectoryButton(value);
				contentBrowserDirectoryButton.clicked += this.handleDirectoryButtonClicked;
				this.itemsPanel.addElement(contentBrowserDirectoryButton);
			}
			do
			{
				ContentBrowserDirectoryButton contentBrowserDirectoryButton2 = new ContentBrowserDirectoryButton(directory);
				contentBrowserDirectoryButton2.transform.anchorMin = new Vector2(0f, 0f);
				contentBrowserDirectoryButton2.transform.anchorMax = new Vector2(0f, 1f);
				contentBrowserDirectoryButton2.transform.pivot = new Vector2(0f, 1f);
				contentBrowserDirectoryButton2.transform.sizeDelta = new Vector2(150f, 0f);
				contentBrowserDirectoryButton2.clicked += this.handleDirectoryButtonClicked;
				this.pathPanel.addElement(contentBrowserDirectoryButton2, 0);
				if (directory.parent != null)
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
			while ((directory = directory.parent) != null);
		}

		protected void handleRootDirectoryButtonClicked(Sleek2ImageButton button)
		{
			ContentBrowserWindow.browse((button as ContentBrowserRootDirectoryButton).directory);
		}

		protected void handleDirectoryButtonClicked(Sleek2ImageButton button)
		{
			ContentBrowserWindow.browse((button as ContentBrowserDirectoryButton).directory);
		}

		protected static ContentDirectory currentDirectory;

		protected Sleek2Element rootsBox;

		protected Sleek2Element itemsBox;

		protected Sleek2Separator separator;

		protected Sleek2Element rootsPanel;

		protected Sleek2Scrollview rootsView;

		protected Sleek2HorizontalScrollviewContents pathPanel;

		protected Sleek2Element itemsPanel;

		protected Sleek2Scrollview itemsView;

		protected Sleek2Scrollbar rootsScrollbar;

		protected delegate void ContentBrowserWindowBrowsedHandler(ContentDirectory directory);
	}
}
