using System;
using System.Collections.Generic;
using SDG.Framework.Debug;
using SDG.Framework.Devkit.Tools;
using SDG.Framework.Translations;
using SDG.Framework.UI.Sleek2;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.UI.Devkit.ObjectBrowserUI
{
	public class ObjectBrowserWindow : Sleek2Window
	{
		public ObjectBrowserWindow()
		{
			base.gameObject.name = "Object_Browser";
			this.searchLength = -1;
			this.searchResults = new List<ObjectAsset>();
			base.tab.label.translation = new TranslatedText(new TranslationReference("SDG", "Devkit.Window.Object_Browser.Title"));
			base.tab.label.translation.format();
			this.searchField = new Sleek2Field();
			this.searchField.transform.anchorMin = new Vector2(0f, 1f);
			this.searchField.transform.anchorMax = new Vector2(1f, 1f);
			this.searchField.transform.pivot = new Vector2(0.5f, 1f);
			this.searchField.transform.offsetMin = new Vector2(0f, (float)(-(float)Sleek2Config.bodyHeight));
			this.searchField.transform.offsetMax = new Vector2(0f, 0f);
			this.searchField.typed += this.handleSearchFieldTyped;
			base.safePanel.addElement(this.searchField);
			this.itemsView = new Sleek2Scrollview();
			this.itemsView.transform.anchorMin = new Vector2(0f, 0f);
			this.itemsView.transform.anchorMax = new Vector2(1f, 1f);
			this.itemsView.transform.pivot = new Vector2(0f, 1f);
			this.itemsView.transform.offsetMin = new Vector2(0f, 0f);
			this.itemsView.transform.offsetMax = new Vector2(0f, (float)(-(float)Sleek2Config.bodyHeight - 5));
			this.itemsView.vertical = true;
			this.itemsPanel = new Sleek2VerticalScrollviewContents();
			this.itemsPanel.name = "Panel";
			this.itemsView.panel = this.itemsPanel;
			base.safePanel.addElement(this.itemsView);
		}

		[TerminalCommandProperty("object_browser.show_official_objects", "include objects from vanilla game", true)]
		public static bool showOfficialObjects
		{
			get
			{
				return ObjectBrowserWindow._showOfficialObjects;
			}
			set
			{
				ObjectBrowserWindow._showOfficialObjects = value;
				TerminalUtility.printCommandPass("Set show_official_objects to: " + ObjectBrowserWindow.showOfficialObjects);
			}
		}

		[TerminalCommandProperty("object_browser.show_curated_objects", "include objects from curated maps", true)]
		public static bool showCuratedObjects
		{
			get
			{
				return ObjectBrowserWindow._showCuratedObjects;
			}
			set
			{
				ObjectBrowserWindow._showCuratedObjects = value;
				TerminalUtility.printCommandPass("Set show_curated_objects to: " + ObjectBrowserWindow.showCuratedObjects);
			}
		}

		[TerminalCommandProperty("object_browser.show_workshop_objects", "include objects from workshop downloads", true)]
		public static bool showWorkshopObjects
		{
			get
			{
				return ObjectBrowserWindow._showWorkshopObjects;
			}
			set
			{
				ObjectBrowserWindow._showWorkshopObjects = value;
				TerminalUtility.printCommandPass("Set show_workshop_objects to: " + ObjectBrowserWindow.showWorkshopObjects);
			}
		}

		[TerminalCommandProperty("object_browser.show_misc_objects", "include objects from other origins", true)]
		public static bool showMiscObjects
		{
			get
			{
				return ObjectBrowserWindow._showMiscObjects;
			}
			set
			{
				ObjectBrowserWindow._showMiscObjects = value;
				TerminalUtility.printCommandPass("Set show_misc_objects to: " + ObjectBrowserWindow.showMiscObjects);
			}
		}

		protected virtual void handleSearchFieldTyped(Sleek2Field field, string value)
		{
			if (this.searchLength == -1 || value.Length < this.searchLength)
			{
				this.searchResults.Clear();
				Assets.find<ObjectAsset>(this.searchResults);
			}
			this.searchLength = value.Length;
			this.itemsPanel.clearElements();
			this.itemsPanel.transform.offsetMin = new Vector2(0f, 0f);
			this.itemsPanel.transform.offsetMax = new Vector2(0f, 0f);
			if (value.Length > 0)
			{
				string[] array = value.Split(new char[]
				{
					' '
				});
				for (int i = this.searchResults.Count - 1; i >= 0; i--)
				{
					ObjectAsset objectAsset = this.searchResults[i];
					bool flag = true;
					switch (objectAsset.assetOrigin)
					{
					case EAssetOrigin.OFFICIAL:
						flag &= ObjectBrowserWindow.showOfficialObjects;
						break;
					case EAssetOrigin.CURATED:
						flag &= ObjectBrowserWindow.showCuratedObjects;
						break;
					case EAssetOrigin.WORKSHOP:
						flag &= ObjectBrowserWindow.showWorkshopObjects;
						break;
					case EAssetOrigin.MISC:
						flag &= ObjectBrowserWindow.showMiscObjects;
						break;
					}
					if (flag)
					{
						foreach (string value2 in array)
						{
							if (objectAsset.objectName.IndexOf(value2, StringComparison.InvariantCultureIgnoreCase) == -1 && objectAsset.name.IndexOf(value2, StringComparison.InvariantCultureIgnoreCase) == -1)
							{
								flag = false;
								break;
							}
						}
					}
					if (!flag)
					{
						this.searchResults.RemoveAtFast(i);
					}
				}
				if (this.searchResults.Count <= 64)
				{
					this.searchResults.Sort(new ObjectBrowserWindow.ObjectBrowserAssetComparer());
					foreach (ObjectAsset newAsset in this.searchResults)
					{
						ObjectBrowserAssetButton objectBrowserAssetButton = new ObjectBrowserAssetButton(newAsset);
						objectBrowserAssetButton.clicked += this.handleAssetButtonClicked;
						this.itemsPanel.addElement(objectBrowserAssetButton);
					}
				}
			}
		}

		protected void handleAssetButtonClicked(Sleek2ImageButton button)
		{
			ObjectAsset asset = (button as ObjectBrowserAssetButton).asset;
			if (asset == null)
			{
				return;
			}
			DevkitSelectionToolObjectInstantiationInfo devkitSelectionToolObjectInstantiationInfo = new DevkitSelectionToolObjectInstantiationInfo();
			devkitSelectionToolObjectInstantiationInfo.asset = asset;
			devkitSelectionToolObjectInstantiationInfo.rotation = Quaternion.Euler(-90f, 0f, 0f);
			DevkitSelectionToolOptions.instance.instantiationInfo = devkitSelectionToolObjectInstantiationInfo;
		}

		private static bool _showOfficialObjects = true;

		private static bool _showCuratedObjects = true;

		private static bool _showWorkshopObjects = true;

		private static bool _showMiscObjects = true;

		protected int searchLength;

		protected List<ObjectAsset> searchResults;

		protected Sleek2Field searchField;

		protected Sleek2Element itemsPanel;

		protected Sleek2Scrollview itemsView;

		private class ObjectBrowserAssetComparer : IComparer<ObjectAsset>
		{
			public int Compare(ObjectAsset x, ObjectAsset y)
			{
				return x.objectName.CompareTo(y.objectName);
			}
		}
	}
}
