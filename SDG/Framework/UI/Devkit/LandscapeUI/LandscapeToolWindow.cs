using System;
using System.Collections.Generic;
using SDG.Framework.Debug;
using SDG.Framework.Devkit;
using SDG.Framework.Devkit.Tools;
using SDG.Framework.Landscapes;
using SDG.Framework.Translations;
using SDG.Framework.UI.Devkit.InspectorUI;
using SDG.Framework.UI.Sleek2;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.UI.Devkit.LandscapeUI
{
	public class LandscapeToolWindow : Sleek2Window
	{
		public LandscapeToolWindow()
		{
			base.gameObject.name = "Landscape_Tool";
			this.searchLength = -1;
			this.searchResults = new List<LandscapeMaterialAsset>();
			base.tab.label.translation = new TranslatedText(new TranslationReference("SDG", "Devkit.Window.Landscape_Tool.Title"));
			base.tab.label.translation.format();
			this.heightmapModeButton = new Sleek2ImageTranslatedLabelButton();
			this.heightmapModeButton.transform.anchorMin = new Vector2(0f, 1f);
			this.heightmapModeButton.transform.anchorMax = new Vector2(0.33f, 1f);
			this.heightmapModeButton.transform.pivot = new Vector2(0.5f, 1f);
			this.heightmapModeButton.transform.offsetMin = new Vector2(0f, (float)(-(float)Sleek2Config.bodyHeight));
			this.heightmapModeButton.transform.offsetMax = new Vector2(-5f, 0f);
			this.heightmapModeButton.label.translation = new TranslatedText(new TranslationReference("#SDG::Devkit.Window.Landscape_Tool.Heightmap_Mode_Button.Label"));
			this.heightmapModeButton.label.translation.format();
			this.heightmapModeButton.clicked += this.handleHeightmapModeButtonClicked;
			base.safePanel.addElement(this.heightmapModeButton);
			this.splatmapModeButton = new Sleek2ImageTranslatedLabelButton();
			this.splatmapModeButton.transform.anchorMin = new Vector2(0.33f, 1f);
			this.splatmapModeButton.transform.anchorMax = new Vector2(0.667f, 1f);
			this.splatmapModeButton.transform.pivot = new Vector2(0.5f, 1f);
			this.splatmapModeButton.transform.offsetMin = new Vector2(5f, (float)(-(float)Sleek2Config.bodyHeight));
			this.splatmapModeButton.transform.offsetMax = new Vector2(-10f, 0f);
			this.splatmapModeButton.label.translation = new TranslatedText(new TranslationReference("#SDG::Devkit.Window.Landscape_Tool.Splatmap_Mode_Button.Label"));
			this.splatmapModeButton.label.translation.format();
			this.splatmapModeButton.clicked += this.handleSplatmapModeButtonClicked;
			base.safePanel.addElement(this.splatmapModeButton);
			this.tileModeButton = new Sleek2ImageTranslatedLabelButton();
			this.tileModeButton.transform.anchorMin = new Vector2(0.667f, 1f);
			this.tileModeButton.transform.anchorMax = new Vector2(1f, 1f);
			this.tileModeButton.transform.pivot = new Vector2(0.5f, 1f);
			this.tileModeButton.transform.offsetMin = new Vector2(5f, (float)(-(float)Sleek2Config.bodyHeight));
			this.tileModeButton.transform.offsetMax = new Vector2(-5f, 0f);
			this.tileModeButton.label.translation = new TranslatedText(new TranslationReference("#SDG::Devkit.Window.Landscape_Tool.Tile_Mode_Button.Label"));
			this.tileModeButton.label.translation.format();
			this.tileModeButton.clicked += this.handleTileModeButtonClicked;
			base.safePanel.addElement(this.tileModeButton);
			this.modePanel = new Sleek2Element();
			this.modePanel.transform.anchorMin = new Vector2(0f, 0f);
			this.modePanel.transform.anchorMax = new Vector2(1f, 1f);
			this.modePanel.transform.pivot = new Vector2(0f, 1f);
			this.modePanel.transform.offsetMin = new Vector2(0f, 0f);
			this.modePanel.transform.offsetMax = new Vector2(0f, (float)(-(float)Sleek2Config.bodyHeight - 5));
			base.safePanel.addElement(this.modePanel);
			this.heightmapModePanel = new Sleek2Element();
			this.heightmapModePanel.transform.reset();
			this.modePanel.addElement(this.heightmapModePanel);
			this.splatmapModePanel = new Sleek2Element();
			this.splatmapModePanel.transform.reset();
			this.modePanel.addElement(this.splatmapModePanel);
			this.tileModePanel = new Sleek2Element();
			this.tileModePanel.transform.reset();
			this.modePanel.addElement(this.tileModePanel);
			this.updateMode();
			this.heightmapInspector = new Sleek2Inspector();
			this.heightmapInspector.transform.anchorMin = new Vector2(0f, 0f);
			this.heightmapInspector.transform.anchorMax = new Vector2(1f, 1f);
			this.heightmapInspector.transform.pivot = new Vector2(0f, 1f);
			this.heightmapInspector.transform.offsetMin = new Vector2(0f, 0f);
			this.heightmapInspector.transform.offsetMax = new Vector2(0f, 0f);
			this.heightmapModePanel.addElement(this.heightmapInspector);
			this.heightmapInspector.inspect(DevkitLandscapeToolHeightmapOptions.instance);
			this.splatmapInspector = new Sleek2Inspector();
			this.splatmapInspector.transform.anchorMin = new Vector2(0f, 1f);
			this.splatmapInspector.transform.anchorMax = new Vector2(1f, 1f);
			this.splatmapInspector.transform.pivot = new Vector2(0f, 1f);
			this.splatmapInspector.transform.offsetMin = new Vector2(0f, -235f);
			this.splatmapInspector.transform.offsetMax = new Vector2(0f, 0f);
			this.splatmapModePanel.addElement(this.splatmapInspector);
			this.splatmapInspector.inspect(DevkitLandscapeToolSplatmapOptions.instance);
			this.splatmapSearchField = new Sleek2Field();
			this.splatmapSearchField.transform.anchorMin = new Vector2(0f, 1f);
			this.splatmapSearchField.transform.anchorMax = new Vector2(1f, 1f);
			this.splatmapSearchField.transform.pivot = new Vector2(0.5f, 1f);
			this.splatmapSearchField.transform.offsetMin = new Vector2(0f, (float)(-(float)Sleek2Config.bodyHeight - 245));
			this.splatmapSearchField.transform.offsetMax = new Vector2(0f, -245f);
			this.splatmapSearchField.typed += this.handleSplatmapSearchFieldTyped;
			this.splatmapModePanel.addElement(this.splatmapSearchField);
			this.splatmapMaterialsView = new Sleek2Scrollview();
			this.splatmapMaterialsView.transform.anchorMin = new Vector2(0f, 0f);
			this.splatmapMaterialsView.transform.anchorMax = new Vector2(1f, 1f);
			this.splatmapMaterialsView.transform.pivot = new Vector2(0.5f, 1f);
			this.splatmapMaterialsView.transform.offsetMin = new Vector2(0f, 0f);
			this.splatmapMaterialsView.transform.offsetMax = new Vector2(0f, (float)(-(float)Sleek2Config.bodyHeight - 250));
			this.splatmapMaterialsView.vertical = true;
			this.splatmapMaterialsPanel = new Sleek2VerticalScrollviewContents();
			this.splatmapMaterialsPanel.name = "Panel";
			this.splatmapMaterialsView.panel = this.splatmapMaterialsPanel;
			this.splatmapModePanel.addElement(this.splatmapMaterialsView);
			this.tileInspector = new Sleek2Inspector();
			this.tileInspector.transform.anchorMin = new Vector2(0f, 1f);
			this.tileInspector.transform.anchorMax = new Vector2(1f, 1f);
			this.tileInspector.transform.pivot = new Vector2(0f, 1f);
			this.tileInspector.transform.offsetMin = new Vector2(0f, -320f);
			this.tileInspector.transform.offsetMax = new Vector2(0f, 0f);
			this.tileModePanel.addElement(this.tileInspector);
			this.tileResetHeightmapButton = new Sleek2ImageLabelButton();
			this.tileResetHeightmapButton.transform.anchorMin = new Vector2(0f, 1f);
			this.tileResetHeightmapButton.transform.anchorMax = new Vector2(1f, 1f);
			this.tileResetHeightmapButton.transform.pivot = new Vector2(0.5f, 1f);
			this.tileResetHeightmapButton.transform.offsetMin = new Vector2(0f, (float)(-(float)Sleek2Config.bodyHeight - 330));
			this.tileResetHeightmapButton.transform.offsetMax = new Vector2(0f, -330f);
			this.tileResetHeightmapButton.label.textComponent.text = "Reset Heightmap";
			this.tileResetHeightmapButton.clicked += this.handleTileResetHeightmapButtonClicked;
			this.tileModePanel.addElement(this.tileResetHeightmapButton);
			this.tileResetSplatmapButton = new Sleek2ImageLabelButton();
			this.tileResetSplatmapButton.transform.anchorMin = new Vector2(0f, 1f);
			this.tileResetSplatmapButton.transform.anchorMax = new Vector2(1f, 1f);
			this.tileResetSplatmapButton.transform.pivot = new Vector2(0.5f, 1f);
			this.tileResetSplatmapButton.transform.offsetMin = new Vector2(0f, (float)(-(float)Sleek2Config.bodyHeight * 2 - 330));
			this.tileResetSplatmapButton.transform.offsetMax = new Vector2(0f, (float)(-(float)Sleek2Config.bodyHeight - 330));
			this.tileResetSplatmapButton.label.textComponent.text = "Reset Splatmap";
			this.tileResetSplatmapButton.clicked += this.handleTileResetSplatmapButtonClicked;
			this.tileModePanel.addElement(this.tileResetSplatmapButton);
			this.tileNormalizeSplatmapButton = new Sleek2ImageLabelButton();
			this.tileNormalizeSplatmapButton.transform.anchorMin = new Vector2(0f, 1f);
			this.tileNormalizeSplatmapButton.transform.anchorMax = new Vector2(1f, 1f);
			this.tileNormalizeSplatmapButton.transform.pivot = new Vector2(0.5f, 1f);
			this.tileNormalizeSplatmapButton.transform.offsetMin = new Vector2(0f, (float)(-(float)Sleek2Config.bodyHeight * 3 - 330));
			this.tileNormalizeSplatmapButton.transform.offsetMax = new Vector2(0f, (float)(-(float)Sleek2Config.bodyHeight * 2 - 330));
			this.tileNormalizeSplatmapButton.label.textComponent.text = "Normalize Splatmap";
			this.tileNormalizeSplatmapButton.clicked += this.handleTileNormalizeSplatmapButtonClicked;
			this.tileModePanel.addElement(this.tileNormalizeSplatmapButton);
			DevkitLandscapeTool.toolModeChanged += this.handleToolModeChanged;
			DevkitLandscapeTool.selectedTileChanged += this.handleSelectedTileChanged;
			DevkitHotkeys.registerTool(2, this);
		}

		[TerminalCommandProperty("landscape.tool.show_official_assets", "include assets from vanilla game", true)]
		public static bool showOfficialAssets
		{
			get
			{
				return LandscapeToolWindow._showOfficialAssets;
			}
			set
			{
				LandscapeToolWindow._showOfficialAssets = value;
				TerminalUtility.printCommandPass("Set show_official_assets to: " + LandscapeToolWindow.showOfficialAssets);
			}
		}

		[TerminalCommandProperty("landscape.tool.show_curated_assets", "include assets from curated maps", true)]
		public static bool showCuratedAssets
		{
			get
			{
				return LandscapeToolWindow._showCuratedAssets;
			}
			set
			{
				LandscapeToolWindow._showCuratedAssets = value;
				TerminalUtility.printCommandPass("Set show_curated_assets to: " + LandscapeToolWindow.showCuratedAssets);
			}
		}

		[TerminalCommandProperty("landscape.tool.show_workshop_assets", "include assets from workshop downloads", true)]
		public static bool showWorkshopAssets
		{
			get
			{
				return LandscapeToolWindow._showWorkshopAssets;
			}
			set
			{
				LandscapeToolWindow._showWorkshopAssets = value;
				TerminalUtility.printCommandPass("Set show_workshop_assets to: " + LandscapeToolWindow.showWorkshopAssets);
			}
		}

		[TerminalCommandProperty("landscape.tool.show_misc_assets", "include assets from other origins", true)]
		public static bool showMiscAssets
		{
			get
			{
				return LandscapeToolWindow._showMiscAssets;
			}
			set
			{
				LandscapeToolWindow._showMiscAssets = value;
				TerminalUtility.printCommandPass("Set show_misc_assets to: " + LandscapeToolWindow.showMiscAssets);
			}
		}

		protected virtual void updateMode()
		{
			this.heightmapModePanel.isVisible = (DevkitLandscapeTool.toolMode == DevkitLandscapeTool.EDevkitLandscapeToolMode.HEIGHTMAP);
			this.splatmapModePanel.isVisible = (DevkitLandscapeTool.toolMode == DevkitLandscapeTool.EDevkitLandscapeToolMode.SPLATMAP);
			this.tileModePanel.isVisible = (DevkitLandscapeTool.toolMode == DevkitLandscapeTool.EDevkitLandscapeToolMode.TILE && DevkitLandscapeTool.selectedTile != null);
			if (this.tileModePanel.isVisible)
			{
				this.refreshTile();
			}
		}

		protected virtual void refreshTile()
		{
			this.tileInspector.inspect(DevkitLandscapeTool.selectedTile);
		}

		protected virtual void handleToolModeChanged(DevkitLandscapeTool.EDevkitLandscapeToolMode oldMode, DevkitLandscapeTool.EDevkitLandscapeToolMode newMode)
		{
			this.updateMode();
		}

		protected virtual void handleSelectedTileChanged(LandscapeTile oldSelectedTile, LandscapeTile newSelectedTile)
		{
			this.updateMode();
		}

		protected virtual void handleHeightmapModeButtonClicked(Sleek2ImageButton button)
		{
			DevkitLandscapeTool.toolMode = DevkitLandscapeTool.EDevkitLandscapeToolMode.HEIGHTMAP;
			this.updateMode();
		}

		protected virtual void handleSplatmapModeButtonClicked(Sleek2ImageButton button)
		{
			DevkitLandscapeTool.toolMode = DevkitLandscapeTool.EDevkitLandscapeToolMode.SPLATMAP;
			this.updateMode();
		}

		protected virtual void handleTileModeButtonClicked(Sleek2ImageButton button)
		{
			DevkitLandscapeTool.toolMode = DevkitLandscapeTool.EDevkitLandscapeToolMode.TILE;
			this.updateMode();
		}

		protected virtual void handleHeightmapAdjustButtonClicked(Sleek2ImageButton button)
		{
			DevkitLandscapeTool.heightmapMode = DevkitLandscapeTool.EDevkitLandscapeToolHeightmapMode.ADJUST;
		}

		protected virtual void handleHeightmapFlattenButtonClicked(Sleek2ImageButton button)
		{
			DevkitLandscapeTool.heightmapMode = DevkitLandscapeTool.EDevkitLandscapeToolHeightmapMode.FLATTEN;
		}

		protected virtual void handleHeightmapSmoothButtonClicked(Sleek2ImageButton button)
		{
			DevkitLandscapeTool.heightmapMode = DevkitLandscapeTool.EDevkitLandscapeToolHeightmapMode.SMOOTH;
		}

		protected virtual void handleHeightmapRampButtonClicked(Sleek2ImageButton button)
		{
			DevkitLandscapeTool.heightmapMode = DevkitLandscapeTool.EDevkitLandscapeToolHeightmapMode.RAMP;
		}

		protected virtual void handleMaterialAssetButtonClicked(Sleek2ImageButton button)
		{
			DevkitLandscapeTool.splatmapMaterialTarget = (button as LandscapeToolMaterialAssetButton).asset.getReferenceTo<LandscapeMaterialAsset>();
		}

		protected virtual void handleSplatmapSearchFieldTyped(Sleek2Field field, string value)
		{
			if (this.searchLength == -1 || value.Length < this.searchLength)
			{
				this.searchResults.Clear();
				Assets.find<LandscapeMaterialAsset>(this.searchResults);
			}
			this.searchLength = value.Length;
			this.splatmapMaterialsPanel.clearElements();
			this.splatmapMaterialsPanel.transform.offsetMin = new Vector2(0f, 0f);
			this.splatmapMaterialsPanel.transform.offsetMax = new Vector2(0f, 0f);
			if (value.Length > 0)
			{
				string[] array = value.Split(new char[]
				{
					' '
				});
				for (int i = this.searchResults.Count - 1; i >= 0; i--)
				{
					LandscapeMaterialAsset landscapeMaterialAsset = this.searchResults[i];
					bool flag = true;
					switch (landscapeMaterialAsset.assetOrigin)
					{
					case EAssetOrigin.OFFICIAL:
						flag &= LandscapeToolWindow.showOfficialAssets;
						break;
					case EAssetOrigin.CURATED:
						flag &= LandscapeToolWindow.showCuratedAssets;
						break;
					case EAssetOrigin.WORKSHOP:
						flag &= LandscapeToolWindow.showWorkshopAssets;
						break;
					case EAssetOrigin.MISC:
						flag &= LandscapeToolWindow.showMiscAssets;
						break;
					}
					if (flag)
					{
						foreach (string value2 in array)
						{
							if (landscapeMaterialAsset.name.IndexOf(value2, StringComparison.InvariantCultureIgnoreCase) == -1)
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
					this.searchResults.Sort(new LandscapeToolWindow.LandscapeToolAssetComparer());
					foreach (LandscapeMaterialAsset newAsset in this.searchResults)
					{
						LandscapeToolMaterialAssetButton landscapeToolMaterialAssetButton = new LandscapeToolMaterialAssetButton(newAsset);
						landscapeToolMaterialAssetButton.clicked += this.handleMaterialAssetButtonClicked;
						this.splatmapMaterialsPanel.addElement(landscapeToolMaterialAssetButton);
					}
				}
			}
		}

		protected virtual void handleTileResetHeightmapButtonClicked(Sleek2ImageButton button)
		{
			if (DevkitLandscapeTool.selectedTile == null)
			{
				return;
			}
			DevkitLandscapeTool.selectedTile.resetHeightmap();
		}

		protected virtual void handleTileResetSplatmapButtonClicked(Sleek2ImageButton button)
		{
			if (DevkitLandscapeTool.selectedTile == null)
			{
				return;
			}
			DevkitLandscapeTool.selectedTile.resetSplatmap();
		}

		protected virtual void handleTileNormalizeSplatmapButtonClicked(Sleek2ImageButton button)
		{
			if (DevkitLandscapeTool.selectedTile == null)
			{
				return;
			}
			DevkitLandscapeTool.selectedTile.normalizeSplatmap();
		}

		protected override void triggerFocused()
		{
			if (DevkitEquipment.instance != null)
			{
				if (this.isActive)
				{
					DevkitEquipment.instance.equip(Activator.CreateInstance(typeof(DevkitLandscapeTool)) as IDevkitTool);
				}
				else
				{
					DevkitEquipment.instance.dequip();
				}
			}
			base.triggerFocused();
		}

		protected override void triggerDestroyed()
		{
			DevkitLandscapeTool.toolModeChanged -= this.handleToolModeChanged;
			base.triggerDestroyed();
		}

		private static bool _showOfficialAssets = true;

		private static bool _showCuratedAssets = true;

		private static bool _showWorkshopAssets = true;

		private static bool _showMiscAssets = true;

		protected Sleek2ImageTranslatedLabelButton heightmapModeButton;

		protected Sleek2ImageTranslatedLabelButton splatmapModeButton;

		protected Sleek2ImageTranslatedLabelButton tileModeButton;

		protected Sleek2Element modePanel;

		protected Sleek2Element heightmapModePanel;

		protected Sleek2Element splatmapModePanel;

		protected Sleek2Element tileModePanel;

		protected Sleek2ImageTranslatedLabelButton heightmapAdjustButton;

		protected Sleek2ImageTranslatedLabelButton heightmapFlattenButton;

		protected Sleek2ImageTranslatedLabelButton heightmapSmoothButton;

		protected Sleek2ImageTranslatedLabelButton heightmapRampButton;

		protected Sleek2Inspector heightmapInspector;

		protected Sleek2Inspector splatmapInspector;

		protected Sleek2Field splatmapSearchField;

		protected Sleek2Element splatmapMaterialsPanel;

		protected Sleek2Scrollview splatmapMaterialsView;

		protected Sleek2Inspector tileInspector;

		protected Sleek2ImageLabelButton tileResetHeightmapButton;

		protected Sleek2ImageLabelButton tileResetSplatmapButton;

		protected Sleek2ImageLabelButton tileNormalizeSplatmapButton;

		protected int searchLength;

		protected List<LandscapeMaterialAsset> searchResults;

		private class LandscapeToolAssetComparer : IComparer<LandscapeMaterialAsset>
		{
			public int Compare(LandscapeMaterialAsset x, LandscapeMaterialAsset y)
			{
				return x.name.CompareTo(y.name);
			}
		}
	}
}
