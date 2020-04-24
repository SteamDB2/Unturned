using System;
using SDG.Framework.IO.FormattedFiles;
using SDG.Framework.Translations;
using SDG.Framework.UI.Devkit.InspectorUI;
using SDG.Framework.UI.Sleek2;
using SDG.Unturned;
using UnityEngine;
using UnityEngine.UI;

namespace SDG.Framework.UI.Devkit.AssetBrowserUI
{
	public class AssetBrowserWindow : Sleek2Window
	{
		public AssetBrowserWindow()
		{
			base.gameObject.name = "Asset_Browser";
			base.tab.label.translation = new TranslatedText(new TranslationReference("SDG", "Devkit.Window.Asset_Browser.Title"));
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
			for (int i = 0; i < Assets.rootAssetDirectories.Count; i++)
			{
				RootAssetDirectory newDirectory = Assets.rootAssetDirectories[i];
				AssetBrowserRootDirectoryButton assetBrowserRootDirectoryButton = new AssetBrowserRootDirectoryButton(newDirectory);
				assetBrowserRootDirectoryButton.clicked += this.handleRootDirectoryButtonClicked;
				this.rootsPanel.addElement(assetBrowserRootDirectoryButton);
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
			AssetBrowserWindow.browsed += this.handleBrowsed;
			this.handleBrowsed(AssetBrowserWindow.currentDirectory);
		}

		protected static event AssetBrowserWindow.AssetBrowserWindowBrowsedHandler browsed;

		public static void browse(AssetDirectory directory)
		{
			AssetBrowserWindow.currentDirectory = directory;
			if (AssetBrowserWindow.browsed != null)
			{
				AssetBrowserWindow.browsed(AssetBrowserWindow.currentDirectory);
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
			AssetBrowserWindow.browsed -= this.handleBrowsed;
			base.triggerDestroyed();
		}

		protected void handleBrowsed(AssetDirectory directory)
		{
			this.pathPanel.clearElements();
			this.itemsPanel.clearElements();
			this.itemsPanel.transform.offsetMin = new Vector2(0f, 0f);
			this.itemsPanel.transform.offsetMax = new Vector2(0f, 0f);
			if (directory == null)
			{
				return;
			}
			for (int i = 0; i < directory.assets.Count; i++)
			{
				AssetBrowserAssetButton assetBrowserAssetButton = new AssetBrowserAssetButton(directory.assets[i]);
				assetBrowserAssetButton.clicked += this.handleAssetButtonClicked;
				this.itemsPanel.addElement(assetBrowserAssetButton);
			}
			for (int j = 0; j < directory.directories.Count; j++)
			{
				AssetBrowserDirectoryButton assetBrowserDirectoryButton = new AssetBrowserDirectoryButton(directory.directories[j]);
				assetBrowserDirectoryButton.clicked += this.handleDirectoryButtonClicked;
				this.itemsPanel.addElement(assetBrowserDirectoryButton);
			}
			do
			{
				AssetBrowserDirectoryButton assetBrowserDirectoryButton2 = new AssetBrowserDirectoryButton(directory);
				assetBrowserDirectoryButton2.transform.anchorMin = new Vector2(0f, 0f);
				assetBrowserDirectoryButton2.transform.anchorMax = new Vector2(0f, 1f);
				assetBrowserDirectoryButton2.transform.pivot = new Vector2(0f, 1f);
				assetBrowserDirectoryButton2.transform.sizeDelta = new Vector2(150f, 0f);
				assetBrowserDirectoryButton2.clicked += this.handleDirectoryButtonClicked;
				this.pathPanel.addElement(assetBrowserDirectoryButton2, 0);
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

		protected void handleAssetButtonClicked(Sleek2ImageButton button)
		{
			Asset asset = (button as AssetBrowserAssetButton).asset;
			if (asset != null)
			{
				asset.clearHash();
				InspectorWindow.inspect(asset);
			}
		}

		protected void handleRootDirectoryButtonClicked(Sleek2ImageButton button)
		{
			AssetBrowserWindow.browse((button as AssetBrowserRootDirectoryButton).directory);
		}

		protected void handleDirectoryButtonClicked(Sleek2ImageButton button)
		{
			AssetBrowserWindow.browse((button as AssetBrowserDirectoryButton).directory);
		}

		protected static AssetDirectory currentDirectory;

		protected Sleek2Element rootsBox;

		protected Sleek2Element itemsBox;

		protected Sleek2Separator separator;

		protected Sleek2Element rootsPanel;

		protected Sleek2Scrollview rootsView;

		protected Sleek2HorizontalScrollviewContents pathPanel;

		protected Sleek2Element itemsPanel;

		protected Sleek2Scrollview itemsView;

		protected Sleek2Scrollbar rootsScrollbar;

		protected delegate void AssetBrowserWindowBrowsedHandler(AssetDirectory directory);
	}
}
