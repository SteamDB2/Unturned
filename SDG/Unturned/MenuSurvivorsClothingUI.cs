using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SDG.Provider;
using SDG.SteamworksProvider.Services.Economy;
using Steamworks;
using UnityEngine;
using UnityEngine.Analytics;

namespace SDG.Unturned
{
	public class MenuSurvivorsClothingUI
	{
		public MenuSurvivorsClothingUI()
		{
			MenuSurvivorsClothingUI.localization = Localization.read("/Menu/Survivors/MenuSurvivorsClothing.dat");
			if (MenuSurvivorsClothingUI.icons != null)
			{
				MenuSurvivorsClothingUI.icons.unload();
				MenuSurvivorsClothingUI.icons = null;
			}
			MenuSurvivorsClothingUI.icons = Bundles.getBundle("/Bundles/Textures/Menu/Icons/Survivors/MenuSurvivorsClothing/MenuSurvivorsClothing.unity3d");
			MenuSurvivorsClothingUI.container = new Sleek();
			MenuSurvivorsClothingUI.container.positionOffset_X = 10;
			MenuSurvivorsClothingUI.container.positionOffset_Y = 10;
			MenuSurvivorsClothingUI.container.positionScale_Y = 1f;
			MenuSurvivorsClothingUI.container.sizeOffset_X = -20;
			MenuSurvivorsClothingUI.container.sizeOffset_Y = -20;
			MenuSurvivorsClothingUI.container.sizeScale_X = 1f;
			MenuSurvivorsClothingUI.container.sizeScale_Y = 1f;
			MenuUI.container.add(MenuSurvivorsClothingUI.container);
			MenuSurvivorsClothingUI.active = false;
			MenuSurvivorsClothingUI.page = 0;
			MenuSurvivorsClothingUI.inventory = new Sleek();
			MenuSurvivorsClothingUI.inventory.positionOffset_Y = 40;
			MenuSurvivorsClothingUI.inventory.positionScale_X = 0.5f;
			MenuSurvivorsClothingUI.inventory.sizeScale_X = 0.5f;
			MenuSurvivorsClothingUI.inventory.sizeScale_Y = 1f;
			MenuSurvivorsClothingUI.inventory.sizeOffset_Y = -80;
			MenuSurvivorsClothingUI.inventory.constraint = ESleekConstraint.XY;
			MenuSurvivorsClothingUI.container.add(MenuSurvivorsClothingUI.inventory);
			MenuSurvivorsClothingUI.packageButtons = new SleekInventory[25];
			for (int i = 0; i < MenuSurvivorsClothingUI.packageButtons.Length; i++)
			{
				SleekInventory sleekInventory = new SleekInventory();
				sleekInventory.positionOffset_X = 5;
				sleekInventory.positionOffset_Y = 5;
				sleekInventory.positionScale_X = (float)(i % 5) * 0.2f;
				sleekInventory.positionScale_Y = (float)Mathf.FloorToInt((float)i / 5f) * 0.2f;
				sleekInventory.sizeOffset_X = -10;
				sleekInventory.sizeOffset_Y = -10;
				sleekInventory.sizeScale_X = 0.2f;
				sleekInventory.sizeScale_Y = 0.2f;
				SleekInventory sleekInventory2 = sleekInventory;
				if (MenuSurvivorsClothingUI.<>f__mg$cache0 == null)
				{
					MenuSurvivorsClothingUI.<>f__mg$cache0 = new ClickedInventory(MenuSurvivorsClothingUI.onClickedInventory);
				}
				sleekInventory2.onClickedInventory = MenuSurvivorsClothingUI.<>f__mg$cache0;
				MenuSurvivorsClothingUI.inventory.add(sleekInventory);
				MenuSurvivorsClothingUI.packageButtons[i] = sleekInventory;
			}
			MenuSurvivorsClothingUI.searchField = new SleekField();
			MenuSurvivorsClothingUI.searchField.positionOffset_X = 5;
			MenuSurvivorsClothingUI.searchField.positionOffset_Y = -35;
			MenuSurvivorsClothingUI.searchField.sizeOffset_X = -120;
			MenuSurvivorsClothingUI.searchField.sizeOffset_Y = 30;
			MenuSurvivorsClothingUI.searchField.sizeScale_X = 1f;
			MenuSurvivorsClothingUI.searchField.hint = MenuSurvivorsClothingUI.localization.format("Search_Field_Hint");
			MenuSurvivorsClothingUI.searchField.control = "Search";
			SleekField sleekField = MenuSurvivorsClothingUI.searchField;
			Delegate onEntered = sleekField.onEntered;
			if (MenuSurvivorsClothingUI.<>f__mg$cache1 == null)
			{
				MenuSurvivorsClothingUI.<>f__mg$cache1 = new Entered(MenuSurvivorsClothingUI.onEnteredSearchField);
			}
			sleekField.onEntered = (Entered)Delegate.Combine(onEntered, MenuSurvivorsClothingUI.<>f__mg$cache1);
			MenuSurvivorsClothingUI.inventory.add(MenuSurvivorsClothingUI.searchField);
			MenuSurvivorsClothingUI.searchButton = new SleekButton();
			MenuSurvivorsClothingUI.searchButton.positionOffset_X = -105;
			MenuSurvivorsClothingUI.searchButton.positionOffset_Y = -35;
			MenuSurvivorsClothingUI.searchButton.positionScale_X = 1f;
			MenuSurvivorsClothingUI.searchButton.sizeOffset_X = 100;
			MenuSurvivorsClothingUI.searchButton.sizeOffset_Y = 30;
			MenuSurvivorsClothingUI.searchButton.text = MenuSurvivorsClothingUI.localization.format("Search");
			MenuSurvivorsClothingUI.searchButton.tooltip = MenuSurvivorsClothingUI.localization.format("Search_Tooltip");
			SleekButton sleekButton = MenuSurvivorsClothingUI.searchButton;
			if (MenuSurvivorsClothingUI.<>f__mg$cache2 == null)
			{
				MenuSurvivorsClothingUI.<>f__mg$cache2 = new ClickedButton(MenuSurvivorsClothingUI.onClickedSearchButton);
			}
			sleekButton.onClickedButton = MenuSurvivorsClothingUI.<>f__mg$cache2;
			MenuSurvivorsClothingUI.inventory.add(MenuSurvivorsClothingUI.searchButton);
			MenuSurvivorsClothingUI.pageBox = new SleekBox();
			MenuSurvivorsClothingUI.pageBox.positionOffset_X = -145;
			MenuSurvivorsClothingUI.pageBox.positionOffset_Y = 5;
			MenuSurvivorsClothingUI.pageBox.positionScale_X = 1f;
			MenuSurvivorsClothingUI.pageBox.positionScale_Y = 1f;
			MenuSurvivorsClothingUI.pageBox.sizeOffset_X = 100;
			MenuSurvivorsClothingUI.pageBox.sizeOffset_Y = 30;
			MenuSurvivorsClothingUI.pageBox.fontSize = 14;
			MenuSurvivorsClothingUI.inventory.add(MenuSurvivorsClothingUI.pageBox);
			MenuSurvivorsClothingUI.infoBox = new SleekBox();
			MenuSurvivorsClothingUI.infoBox.positionOffset_X = 5;
			MenuSurvivorsClothingUI.infoBox.positionOffset_Y = -25;
			MenuSurvivorsClothingUI.infoBox.positionScale_Y = 0.5f;
			MenuSurvivorsClothingUI.infoBox.sizeScale_X = 1f;
			MenuSurvivorsClothingUI.infoBox.sizeOffset_X = -10;
			MenuSurvivorsClothingUI.infoBox.sizeOffset_Y = 50;
			MenuSurvivorsClothingUI.infoBox.text = MenuSurvivorsClothingUI.localization.format("No_Items");
			MenuSurvivorsClothingUI.infoBox.fontSize = 14;
			MenuSurvivorsClothingUI.inventory.add(MenuSurvivorsClothingUI.infoBox);
			MenuSurvivorsClothingUI.infoBox.isVisible = !Provider.provider.economyService.isInventoryAvailable;
			MenuSurvivorsClothingUI.leftButton = new SleekButtonIcon((Texture2D)MenuSurvivorsClothingUI.icons.load("Left"));
			MenuSurvivorsClothingUI.leftButton.positionOffset_X = -185;
			MenuSurvivorsClothingUI.leftButton.positionOffset_Y = 5;
			MenuSurvivorsClothingUI.leftButton.positionScale_X = 1f;
			MenuSurvivorsClothingUI.leftButton.positionScale_Y = 1f;
			MenuSurvivorsClothingUI.leftButton.sizeOffset_X = 30;
			MenuSurvivorsClothingUI.leftButton.sizeOffset_Y = 30;
			MenuSurvivorsClothingUI.leftButton.tooltip = MenuSurvivorsClothingUI.localization.format("Left_Tooltip");
			MenuSurvivorsClothingUI.leftButton.iconImage.backgroundTint = ESleekTint.FOREGROUND;
			SleekButton sleekButton2 = MenuSurvivorsClothingUI.leftButton;
			if (MenuSurvivorsClothingUI.<>f__mg$cache3 == null)
			{
				MenuSurvivorsClothingUI.<>f__mg$cache3 = new ClickedButton(MenuSurvivorsClothingUI.onClickedLeftButton);
			}
			sleekButton2.onClickedButton = MenuSurvivorsClothingUI.<>f__mg$cache3;
			MenuSurvivorsClothingUI.inventory.add(MenuSurvivorsClothingUI.leftButton);
			MenuSurvivorsClothingUI.rightButton = new SleekButtonIcon((Texture2D)MenuSurvivorsClothingUI.icons.load("Right"));
			MenuSurvivorsClothingUI.rightButton.positionOffset_X = -35;
			MenuSurvivorsClothingUI.rightButton.positionOffset_Y = 5;
			MenuSurvivorsClothingUI.rightButton.positionScale_X = 1f;
			MenuSurvivorsClothingUI.rightButton.positionScale_Y = 1f;
			MenuSurvivorsClothingUI.rightButton.sizeOffset_X = 30;
			MenuSurvivorsClothingUI.rightButton.sizeOffset_Y = 30;
			MenuSurvivorsClothingUI.rightButton.tooltip = MenuSurvivorsClothingUI.localization.format("Right_Tooltip");
			MenuSurvivorsClothingUI.rightButton.iconImage.backgroundTint = ESleekTint.FOREGROUND;
			SleekButton sleekButton3 = MenuSurvivorsClothingUI.rightButton;
			if (MenuSurvivorsClothingUI.<>f__mg$cache4 == null)
			{
				MenuSurvivorsClothingUI.<>f__mg$cache4 = new ClickedButton(MenuSurvivorsClothingUI.onClickedRightButton);
			}
			sleekButton3.onClickedButton = MenuSurvivorsClothingUI.<>f__mg$cache4;
			MenuSurvivorsClothingUI.inventory.add(MenuSurvivorsClothingUI.rightButton);
			MenuSurvivorsClothingUI.refreshButton = new SleekButtonIcon((Texture2D)MenuSurvivorsClothingUI.icons.load("Refresh"));
			MenuSurvivorsClothingUI.refreshButton.positionOffset_X = 5;
			MenuSurvivorsClothingUI.refreshButton.positionOffset_Y = 5;
			MenuSurvivorsClothingUI.refreshButton.positionScale_Y = 1f;
			MenuSurvivorsClothingUI.refreshButton.sizeOffset_X = 30;
			MenuSurvivorsClothingUI.refreshButton.sizeOffset_Y = 30;
			MenuSurvivorsClothingUI.refreshButton.tooltip = MenuSurvivorsClothingUI.localization.format("Refresh_Tooltip");
			MenuSurvivorsClothingUI.refreshButton.iconImage.backgroundTint = ESleekTint.FOREGROUND;
			SleekButton sleekButton4 = MenuSurvivorsClothingUI.refreshButton;
			if (MenuSurvivorsClothingUI.<>f__mg$cache5 == null)
			{
				MenuSurvivorsClothingUI.<>f__mg$cache5 = new ClickedButton(MenuSurvivorsClothingUI.onClickedRefreshButton);
			}
			sleekButton4.onClickedButton = MenuSurvivorsClothingUI.<>f__mg$cache5;
			MenuSurvivorsClothingUI.inventory.add(MenuSurvivorsClothingUI.refreshButton);
			MenuSurvivorsClothingUI.characterSlider = new SleekSlider();
			MenuSurvivorsClothingUI.characterSlider.positionOffset_X = 45;
			MenuSurvivorsClothingUI.characterSlider.positionOffset_Y = 10;
			MenuSurvivorsClothingUI.characterSlider.positionScale_Y = 1f;
			MenuSurvivorsClothingUI.characterSlider.sizeOffset_X = -240;
			MenuSurvivorsClothingUI.characterSlider.sizeOffset_Y = 20;
			MenuSurvivorsClothingUI.characterSlider.sizeScale_X = 1f;
			MenuSurvivorsClothingUI.characterSlider.orientation = ESleekOrientation.HORIZONTAL;
			SleekSlider sleekSlider = MenuSurvivorsClothingUI.characterSlider;
			if (MenuSurvivorsClothingUI.<>f__mg$cache6 == null)
			{
				MenuSurvivorsClothingUI.<>f__mg$cache6 = new Dragged(MenuSurvivorsClothingUI.onDraggedCharacterSlider);
			}
			sleekSlider.onDragged = MenuSurvivorsClothingUI.<>f__mg$cache6;
			MenuSurvivorsClothingUI.inventory.add(MenuSurvivorsClothingUI.characterSlider);
			MenuSurvivorsClothingUI.backButton = new SleekButtonIcon((Texture2D)MenuDashboardUI.icons.load("Exit"));
			MenuSurvivorsClothingUI.backButton.positionOffset_Y = -50;
			MenuSurvivorsClothingUI.backButton.positionScale_Y = 1f;
			MenuSurvivorsClothingUI.backButton.sizeOffset_X = 200;
			MenuSurvivorsClothingUI.backButton.sizeOffset_Y = 50;
			MenuSurvivorsClothingUI.backButton.text = MenuDashboardUI.localization.format("BackButtonText");
			MenuSurvivorsClothingUI.backButton.tooltip = MenuDashboardUI.localization.format("BackButtonTooltip");
			SleekButton sleekButton5 = MenuSurvivorsClothingUI.backButton;
			if (MenuSurvivorsClothingUI.<>f__mg$cache7 == null)
			{
				MenuSurvivorsClothingUI.<>f__mg$cache7 = new ClickedButton(MenuSurvivorsClothingUI.onClickedBackButton);
			}
			sleekButton5.onClickedButton = MenuSurvivorsClothingUI.<>f__mg$cache7;
			MenuSurvivorsClothingUI.backButton.fontSize = 14;
			MenuSurvivorsClothingUI.backButton.iconImage.backgroundTint = ESleekTint.FOREGROUND;
			MenuSurvivorsClothingUI.container.add(MenuSurvivorsClothingUI.backButton);
			MenuSurvivorsClothingUI.itemstoreButton = new SleekButton();
			MenuSurvivorsClothingUI.itemstoreButton.positionOffset_Y = -110;
			MenuSurvivorsClothingUI.itemstoreButton.positionScale_Y = 1f;
			MenuSurvivorsClothingUI.itemstoreButton.sizeOffset_X = 200;
			MenuSurvivorsClothingUI.itemstoreButton.sizeOffset_Y = 50;
			MenuSurvivorsClothingUI.itemstoreButton.text = MenuSurvivorsClothingUI.localization.format("Itemstore");
			MenuSurvivorsClothingUI.itemstoreButton.tooltip = MenuSurvivorsClothingUI.localization.format("Itemstore_Tooltip");
			SleekButton sleekButton6 = MenuSurvivorsClothingUI.itemstoreButton;
			if (MenuSurvivorsClothingUI.<>f__mg$cache8 == null)
			{
				MenuSurvivorsClothingUI.<>f__mg$cache8 = new ClickedButton(MenuSurvivorsClothingUI.onClickedItemstoreButton);
			}
			sleekButton6.onClickedButton = MenuSurvivorsClothingUI.<>f__mg$cache8;
			MenuSurvivorsClothingUI.itemstoreButton.fontSize = 14;
			MenuSurvivorsClothingUI.container.add(MenuSurvivorsClothingUI.itemstoreButton);
			if (Provider.statusData.Stockpile.Has_New_Items)
			{
				SleekNew sleek = new SleekNew(false);
				MenuSurvivorsClothingUI.itemstoreButton.add(sleek);
			}
			if (Provider.statusData.Stockpile.Featured_Item != 0)
			{
				MenuSurvivorsClothingUI.featuredButton = new SleekButton();
				MenuSurvivorsClothingUI.featuredButton.positionOffset_Y = -170;
				MenuSurvivorsClothingUI.featuredButton.positionScale_Y = 1f;
				MenuSurvivorsClothingUI.featuredButton.sizeOffset_X = 200;
				MenuSurvivorsClothingUI.featuredButton.sizeOffset_Y = 50;
				MenuSurvivorsClothingUI.featuredButton.text = Provider.provider.economyService.getInventoryName(Provider.statusData.Stockpile.Featured_Item);
				MenuSurvivorsClothingUI.featuredButton.tooltip = MenuSurvivorsClothingUI.localization.format("Featured_Tooltip");
				SleekButton sleekButton7 = MenuSurvivorsClothingUI.featuredButton;
				if (MenuSurvivorsClothingUI.<>f__mg$cache9 == null)
				{
					MenuSurvivorsClothingUI.<>f__mg$cache9 = new ClickedButton(MenuSurvivorsClothingUI.onClickedFeaturedButton);
				}
				sleekButton7.onClickedButton = MenuSurvivorsClothingUI.<>f__mg$cache9;
				MenuSurvivorsClothingUI.featuredButton.foregroundTint = ESleekTint.NONE;
				MenuSurvivorsClothingUI.featuredButton.foregroundColor = Provider.provider.economyService.getInventoryColor(Provider.statusData.Stockpile.Featured_Item);
				MenuSurvivorsClothingUI.featuredButton.fontSize = 14;
				MenuSurvivorsClothingUI.container.add(MenuSurvivorsClothingUI.featuredButton);
				SleekNew sleek2 = new SleekNew(false);
				MenuSurvivorsClothingUI.featuredButton.add(sleek2);
			}
			if (!MenuSurvivorsClothingUI.hasLoaded)
			{
				TempSteamworksEconomy economyService = Provider.provider.economyService;
				Delegate onInventoryRefreshed = economyService.onInventoryRefreshed;
				if (MenuSurvivorsClothingUI.<>f__mg$cacheA == null)
				{
					MenuSurvivorsClothingUI.<>f__mg$cacheA = new TempSteamworksEconomy.InventoryRefreshed(MenuSurvivorsClothingUI.onInventoryRefreshed);
				}
				economyService.onInventoryRefreshed = (TempSteamworksEconomy.InventoryRefreshed)Delegate.Combine(onInventoryRefreshed, MenuSurvivorsClothingUI.<>f__mg$cacheA);
				TempSteamworksEconomy economyService2 = Provider.provider.economyService;
				Delegate onInventoryDropped = economyService2.onInventoryDropped;
				if (MenuSurvivorsClothingUI.<>f__mg$cacheB == null)
				{
					MenuSurvivorsClothingUI.<>f__mg$cacheB = new TempSteamworksEconomy.InventoryDropped(MenuSurvivorsClothingUI.onInventoryDropped);
				}
				economyService2.onInventoryDropped = (TempSteamworksEconomy.InventoryDropped)Delegate.Combine(onInventoryDropped, MenuSurvivorsClothingUI.<>f__mg$cacheB);
			}
			Delegate onCharacterUpdated = Characters.onCharacterUpdated;
			if (MenuSurvivorsClothingUI.<>f__mg$cacheC == null)
			{
				MenuSurvivorsClothingUI.<>f__mg$cacheC = new CharacterUpdated(MenuSurvivorsClothingUI.onCharacterUpdated);
			}
			Characters.onCharacterUpdated = (CharacterUpdated)Delegate.Combine(onCharacterUpdated, MenuSurvivorsClothingUI.<>f__mg$cacheC);
			MenuSurvivorsClothingUI.hasLoaded = true;
			MenuSurvivorsClothingUI.updateFilter();
			MenuSurvivorsClothingUI.updatePage();
			new MenuSurvivorsClothingItemUI();
			new MenuSurvivorsClothingInspectUI();
			new MenuSurvivorsClothingDeleteUI();
			new MenuSurvivorsClothingBoxUI();
		}

		private static int pages
		{
			get
			{
				if (MenuSurvivorsClothingUI.filteredItems.Count == 0)
				{
					return 1;
				}
				return (int)Mathf.Ceil((float)MenuSurvivorsClothingUI.filteredItems.Count / 25f);
			}
		}

		public static void open()
		{
			if (MenuSurvivorsClothingUI.active)
			{
				return;
			}
			MenuSurvivorsClothingUI.active = true;
			Characters.apply(false, true);
			MenuSurvivorsClothingUI.container.lerpPositionScale(0f, 0f, ESleekLerp.EXPONENTIAL, 20f);
		}

		public static void close()
		{
			if (!MenuSurvivorsClothingUI.active)
			{
				return;
			}
			MenuSurvivorsClothingUI.active = false;
			if (!MenuSurvivorsClothingBoxUI.active && !MenuSurvivorsClothingInspectUI.active && !MenuSurvivorsClothingDeleteUI.active && !MenuSurvivorsClothingItemUI.active)
			{
				Characters.apply(true, true);
			}
			MenuSurvivorsClothingUI.container.lerpPositionScale(0f, 1f, ESleekLerp.EXPONENTIAL, 20f);
		}

		public static void viewPage(int newPage)
		{
			MenuSurvivorsClothingUI.page = newPage;
			MenuSurvivorsClothingUI.updatePage();
		}

		private static void onClickedInventory(SleekInventory button)
		{
			int num = MenuSurvivorsClothingUI.packageButtons.Length * MenuSurvivorsClothingUI.page;
			int num2 = MenuSurvivorsClothingUI.inventory.search(button);
			if (num + num2 < MenuSurvivorsClothingUI.filteredItems.Count)
			{
				if (Input.GetKey(ControlsSettings.other) && MenuSurvivorsClothingUI.packageButtons[num2].itemAsset != null)
				{
					if (MenuSurvivorsClothingUI.packageButtons[num2].itemAsset.type == EItemType.BOX)
					{
						MenuSurvivorsClothingItemUI.viewItem(MenuSurvivorsClothingUI.filteredItems[num + num2].m_iDefinition.m_SteamItemDef, MenuSurvivorsClothingUI.filteredItems[num + num2].m_unQuantity, MenuSurvivorsClothingUI.filteredItems[num + num2].m_itemId.m_SteamItemInstanceID);
						MenuSurvivorsClothingBoxUI.viewItem(MenuSurvivorsClothingUI.filteredItems[num + num2].m_iDefinition.m_SteamItemDef, MenuSurvivorsClothingUI.filteredItems[num + num2].m_unQuantity, MenuSurvivorsClothingUI.filteredItems[num + num2].m_itemId.m_SteamItemInstanceID);
						MenuSurvivorsClothingBoxUI.open();
						MenuSurvivorsClothingUI.close();
					}
					else
					{
						Characters.package(MenuSurvivorsClothingUI.filteredItems[num + num2].m_itemId.m_SteamItemInstanceID);
					}
				}
				else
				{
					MenuSurvivorsClothingItemUI.viewItem(MenuSurvivorsClothingUI.filteredItems[num + num2].m_iDefinition.m_SteamItemDef, MenuSurvivorsClothingUI.filteredItems[num + num2].m_unQuantity, MenuSurvivorsClothingUI.filteredItems[num + num2].m_itemId.m_SteamItemInstanceID);
					MenuSurvivorsClothingItemUI.open();
					MenuSurvivorsClothingUI.close();
				}
			}
		}

		private static void onEnteredSearchField(SleekField field)
		{
			MenuSurvivorsClothingUI.updateFilter();
			if (MenuSurvivorsClothingUI.page >= MenuSurvivorsClothingUI.pages)
			{
				MenuSurvivorsClothingUI.page = MenuSurvivorsClothingUI.pages - 1;
			}
			MenuSurvivorsClothingUI.updatePage();
		}

		private static void onClickedSearchButton(SleekButton button)
		{
			MenuSurvivorsClothingUI.updateFilter();
			if (MenuSurvivorsClothingUI.page >= MenuSurvivorsClothingUI.pages)
			{
				MenuSurvivorsClothingUI.page = MenuSurvivorsClothingUI.pages - 1;
			}
			MenuSurvivorsClothingUI.updatePage();
		}

		private static void onClickedLeftButton(SleekButton button)
		{
			if (MenuSurvivorsClothingUI.page > 0)
			{
				MenuSurvivorsClothingUI.viewPage(MenuSurvivorsClothingUI.page - 1);
			}
		}

		private static void onClickedRightButton(SleekButton button)
		{
			if (MenuSurvivorsClothingUI.page < MenuSurvivorsClothingUI.pages - 1)
			{
				MenuSurvivorsClothingUI.viewPage(MenuSurvivorsClothingUI.page + 1);
			}
		}

		private static void onClickedRefreshButton(SleekButton button)
		{
			Provider.provider.economyService.refreshInventory();
		}

		private static void onInventoryRefreshed()
		{
			MenuSurvivorsClothingUI.infoBox.isVisible = false;
			MenuSurvivorsClothingUI.updateFilter();
			if (MenuSurvivorsClothingUI.page >= MenuSurvivorsClothingUI.pages)
			{
				MenuSurvivorsClothingUI.page = MenuSurvivorsClothingUI.pages - 1;
			}
			MenuSurvivorsClothingUI.updatePage();
		}

		public static void onInventoryDropped(int item, ushort quantity, ulong instance)
		{
			MenuUI.closeAll();
			MenuUI.alert(MenuSurvivorsClothingUI.localization.format("Origin_Drop"), instance, item, quantity);
			MenuSurvivorsClothingItemUI.viewItem(item, quantity, instance);
			MenuSurvivorsClothingItemUI.open();
		}

		private static void onCharacterUpdated(byte index, Character character)
		{
			MenuSurvivorsClothingUI.updatePage();
		}

		private static void updateFilter()
		{
			string text = MenuSurvivorsClothingUI.searchField.text;
			if (text == null || text.Length < 1)
			{
				MenuSurvivorsClothingUI.filteredItems = new List<SteamItemDetails_t>(Provider.provider.economyService.inventory);
			}
			else
			{
				MenuSurvivorsClothingUI.filteredItems = new List<SteamItemDetails_t>();
				for (int i = 0; i < Provider.provider.economyService.inventory.Length; i++)
				{
					SteamItemDetails_t item = Provider.provider.economyService.inventory[i];
					string inventoryName = Provider.provider.economyService.getInventoryName(item.m_iDefinition.m_SteamItemDef);
					if (inventoryName.IndexOf(text, StringComparison.OrdinalIgnoreCase) != -1)
					{
						MenuSurvivorsClothingUI.filteredItems.Add(item);
					}
					else
					{
						string inventoryType = Provider.provider.economyService.getInventoryType(item.m_iDefinition.m_SteamItemDef);
						if (inventoryType.IndexOf(text, StringComparison.OrdinalIgnoreCase) != -1)
						{
							MenuSurvivorsClothingUI.filteredItems.Add(item);
						}
					}
				}
			}
		}

		public static void updatePage()
		{
			MenuSurvivorsClothingUI.pageBox.text = MenuSurvivorsClothingUI.localization.format("Page", new object[]
			{
				MenuSurvivorsClothingUI.page + 1,
				MenuSurvivorsClothingUI.pages
			});
			if (MenuSurvivorsClothingUI.packageButtons == null)
			{
				return;
			}
			int num = MenuSurvivorsClothingUI.packageButtons.Length * MenuSurvivorsClothingUI.page;
			for (int i = 0; i < MenuSurvivorsClothingUI.packageButtons.Length; i++)
			{
				if (num + i < MenuSurvivorsClothingUI.filteredItems.Count)
				{
					MenuSurvivorsClothingUI.packageButtons[i].updateInventory(MenuSurvivorsClothingUI.filteredItems[num + i].m_itemId.m_SteamItemInstanceID, MenuSurvivorsClothingUI.filteredItems[num + i].m_iDefinition.m_SteamItemDef, MenuSurvivorsClothingUI.filteredItems[num + i].m_unQuantity, true, false);
				}
				else
				{
					MenuSurvivorsClothingUI.packageButtons[i].updateInventory(0UL, 0, 0, false, false);
				}
			}
		}

		private static void onDraggedCharacterSlider(SleekSlider slider, float state)
		{
			Characters.characterYaw = state * 360f;
		}

		private static void onClickedBackButton(SleekButton button)
		{
			MenuSurvivorsUI.open();
			MenuSurvivorsClothingUI.close();
		}

		private static void onClickedItemstoreButton(SleekButton button)
		{
			if (!Provider.provider.storeService.canOpenStore)
			{
				MenuUI.alert(MenuSurvivorsClothingUI.localization.format("Overlay"));
				return;
			}
			Provider.provider.storeService.open();
			Analytics.CustomEvent("Link_Stockpile", null);
		}

		private static void onClickedFeaturedButton(SleekButton button)
		{
			if (!Provider.provider.storeService.canOpenStore)
			{
				MenuUI.alert(MenuSurvivorsClothingUI.localization.format("Overlay"));
				return;
			}
			Provider.provider.storeService.open(new SteamworksEconomyItemDefinition((SteamItemDef_t)Provider.statusData.Stockpile.Featured_Item));
		}

		public static Local localization;

		public static Bundle icons;

		private static Sleek container;

		public static bool active;

		private static SleekButtonIcon backButton;

		private static SleekButton itemstoreButton;

		private static SleekButton featuredButton;

		private static List<SteamItemDetails_t> filteredItems;

		private static Sleek inventory;

		private static SleekInventory[] packageButtons;

		private static SleekBox pageBox;

		private static SleekBox infoBox;

		private static SleekField searchField;

		private static SleekButton searchButton;

		private static SleekButtonIcon leftButton;

		private static SleekButtonIcon rightButton;

		private static SleekButtonIcon refreshButton;

		private static SleekSlider characterSlider;

		private static int page;

		private static bool hasLoaded;

		[CompilerGenerated]
		private static ClickedInventory <>f__mg$cache0;

		[CompilerGenerated]
		private static Entered <>f__mg$cache1;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache2;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache3;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache4;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache5;

		[CompilerGenerated]
		private static Dragged <>f__mg$cache6;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache7;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache8;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache9;

		[CompilerGenerated]
		private static TempSteamworksEconomy.InventoryRefreshed <>f__mg$cacheA;

		[CompilerGenerated]
		private static TempSteamworksEconomy.InventoryDropped <>f__mg$cacheB;

		[CompilerGenerated]
		private static CharacterUpdated <>f__mg$cacheC;
	}
}
