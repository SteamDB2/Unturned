using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace SDG.Unturned
{
	public class PlayerDashboardInventoryUI
	{
		public PlayerDashboardInventoryUI()
		{
			if (PlayerDashboardInventoryUI.icons != null)
			{
				PlayerDashboardInventoryUI.icons.unload();
			}
			PlayerDashboardInventoryUI.localization = Localization.read("/Player/PlayerDashboardInventory.dat");
			PlayerDashboardInventoryUI.icons = Bundles.getBundle("/Bundles/Textures/Player/Icons/PlayerDashboardInventory/PlayerDashboardInventory.unity3d");
			PlayerDashboardInventoryUI._selectedPage = byte.MaxValue;
			PlayerDashboardInventoryUI._selected_x = byte.MaxValue;
			PlayerDashboardInventoryUI._selected_y = byte.MaxValue;
			PlayerDashboardInventoryUI.container = new Sleek();
			PlayerDashboardInventoryUI.container.positionScale_Y = 1f;
			PlayerDashboardInventoryUI.container.positionOffset_X = 10;
			PlayerDashboardInventoryUI.container.positionOffset_Y = 10;
			PlayerDashboardInventoryUI.container.sizeOffset_X = -20;
			PlayerDashboardInventoryUI.container.sizeOffset_Y = -20;
			PlayerDashboardInventoryUI.container.sizeScale_X = 1f;
			PlayerDashboardInventoryUI.container.sizeScale_Y = 1f;
			PlayerUI.container.add(PlayerDashboardInventoryUI.container);
			PlayerDashboardInventoryUI.active = true;
			PlayerDashboardInventoryUI.backdropBox = new SleekBox();
			PlayerDashboardInventoryUI.backdropBox.positionOffset_Y = 60;
			PlayerDashboardInventoryUI.backdropBox.sizeOffset_Y = -60;
			PlayerDashboardInventoryUI.backdropBox.sizeScale_X = 1f;
			PlayerDashboardInventoryUI.backdropBox.sizeScale_Y = 1f;
			Color white = Color.white;
			white.a = 0.5f;
			PlayerDashboardInventoryUI.backdropBox.backgroundColor = white;
			PlayerDashboardInventoryUI.container.add(PlayerDashboardInventoryUI.backdropBox);
			PlayerDashboardInventoryUI.character = new SleekInspect("RenderTextures/Character");
			PlayerDashboardInventoryUI.character.positionOffset_X = 10;
			PlayerDashboardInventoryUI.character.positionOffset_Y = 70;
			PlayerDashboardInventoryUI.character.sizeOffset_X = 410;
			PlayerDashboardInventoryUI.character.sizeOffset_Y = -280;
			PlayerDashboardInventoryUI.character.sizeScale_Y = 1f;
			PlayerDashboardInventoryUI.backdropBox.add(PlayerDashboardInventoryUI.character);
			PlayerDashboardInventoryUI.slots = new SleekSlot[(int)PlayerInventory.SLOTS];
			byte b = 0;
			while ((int)b < PlayerDashboardInventoryUI.slots.Length)
			{
				PlayerDashboardInventoryUI.slots[(int)b] = new SleekSlot(b);
				SleekSlot sleekSlot = PlayerDashboardInventoryUI.slots[(int)b];
				if (PlayerDashboardInventoryUI.<>f__mg$cache16 == null)
				{
					PlayerDashboardInventoryUI.<>f__mg$cache16 = new SelectedItem(PlayerDashboardInventoryUI.onSelectedItem);
				}
				sleekSlot.onSelectedItem = PlayerDashboardInventoryUI.<>f__mg$cache16;
				SleekSlot sleekSlot2 = PlayerDashboardInventoryUI.slots[(int)b];
				if (PlayerDashboardInventoryUI.<>f__mg$cache17 == null)
				{
					PlayerDashboardInventoryUI.<>f__mg$cache17 = new GrabbedItem(PlayerDashboardInventoryUI.onGrabbedItem);
				}
				sleekSlot2.onGrabbedItem = PlayerDashboardInventoryUI.<>f__mg$cache17;
				SleekSlot sleekSlot3 = PlayerDashboardInventoryUI.slots[(int)b];
				if (PlayerDashboardInventoryUI.<>f__mg$cache18 == null)
				{
					PlayerDashboardInventoryUI.<>f__mg$cache18 = new PlacedItem(PlayerDashboardInventoryUI.onPlacedItem);
				}
				sleekSlot3.onPlacedItem = PlayerDashboardInventoryUI.<>f__mg$cache18;
				PlayerDashboardInventoryUI.backdropBox.add(PlayerDashboardInventoryUI.slots[(int)b]);
				b += 1;
			}
			PlayerDashboardInventoryUI.slots[0].positionOffset_X = 10;
			PlayerDashboardInventoryUI.slots[0].positionOffset_Y = -160;
			PlayerDashboardInventoryUI.slots[0].positionScale_Y = 1f;
			PlayerDashboardInventoryUI.slots[1].positionOffset_X = 270;
			PlayerDashboardInventoryUI.slots[1].positionOffset_Y = -160;
			PlayerDashboardInventoryUI.slots[1].positionScale_Y = 1f;
			PlayerDashboardInventoryUI.slots[1].sizeOffset_X = 150;
			PlayerDashboardInventoryUI.characterSlider = new SleekSlider();
			PlayerDashboardInventoryUI.characterSlider.sizeOffset_Y = 20;
			PlayerDashboardInventoryUI.characterSlider.sizeScale_X = 1f;
			PlayerDashboardInventoryUI.characterSlider.sizeOffset_X = -120;
			PlayerDashboardInventoryUI.characterSlider.positionOffset_X = 120;
			PlayerDashboardInventoryUI.characterSlider.positionOffset_Y = 15;
			PlayerDashboardInventoryUI.characterSlider.positionScale_Y = 1f;
			PlayerDashboardInventoryUI.characterSlider.orientation = ESleekOrientation.HORIZONTAL;
			SleekSlider sleekSlider = PlayerDashboardInventoryUI.characterSlider;
			if (PlayerDashboardInventoryUI.<>f__mg$cache19 == null)
			{
				PlayerDashboardInventoryUI.<>f__mg$cache19 = new Dragged(PlayerDashboardInventoryUI.onDraggedCharacterSlider);
			}
			sleekSlider.onDragged = PlayerDashboardInventoryUI.<>f__mg$cache19;
			PlayerDashboardInventoryUI.character.add(PlayerDashboardInventoryUI.characterSlider);
			PlayerDashboardInventoryUI.swapCosmeticsButton = new SleekButtonIcon((Texture2D)PlayerDashboardInventoryUI.icons.load("Swap_Cosmetics"));
			PlayerDashboardInventoryUI.swapCosmeticsButton.positionOffset_Y = 10;
			PlayerDashboardInventoryUI.swapCosmeticsButton.positionScale_Y = 1f;
			PlayerDashboardInventoryUI.swapCosmeticsButton.sizeOffset_X = 30;
			PlayerDashboardInventoryUI.swapCosmeticsButton.sizeOffset_Y = 30;
			PlayerDashboardInventoryUI.swapCosmeticsButton.tooltip = PlayerDashboardInventoryUI.localization.format("Swap_Cosmetics_Tooltip");
			PlayerDashboardInventoryUI.swapCosmeticsButton.iconImage.backgroundTint = ESleekTint.FOREGROUND;
			SleekButton sleekButton = PlayerDashboardInventoryUI.swapCosmeticsButton;
			if (PlayerDashboardInventoryUI.<>f__mg$cache1A == null)
			{
				PlayerDashboardInventoryUI.<>f__mg$cache1A = new ClickedButton(PlayerDashboardInventoryUI.onClickedSwapCosmeticsButton);
			}
			sleekButton.onClickedButton = PlayerDashboardInventoryUI.<>f__mg$cache1A;
			PlayerDashboardInventoryUI.character.add(PlayerDashboardInventoryUI.swapCosmeticsButton);
			PlayerDashboardInventoryUI.swapSkinsButton = new SleekButtonIcon((Texture2D)PlayerDashboardInventoryUI.icons.load("Swap_Skins"));
			PlayerDashboardInventoryUI.swapSkinsButton.positionOffset_X = 40;
			PlayerDashboardInventoryUI.swapSkinsButton.positionOffset_Y = 10;
			PlayerDashboardInventoryUI.swapSkinsButton.positionScale_Y = 1f;
			PlayerDashboardInventoryUI.swapSkinsButton.sizeOffset_X = 30;
			PlayerDashboardInventoryUI.swapSkinsButton.sizeOffset_Y = 30;
			PlayerDashboardInventoryUI.swapSkinsButton.tooltip = PlayerDashboardInventoryUI.localization.format("Swap_Skins_Tooltip");
			PlayerDashboardInventoryUI.swapSkinsButton.iconImage.backgroundTint = ESleekTint.FOREGROUND;
			SleekButton sleekButton2 = PlayerDashboardInventoryUI.swapSkinsButton;
			if (PlayerDashboardInventoryUI.<>f__mg$cache1B == null)
			{
				PlayerDashboardInventoryUI.<>f__mg$cache1B = new ClickedButton(PlayerDashboardInventoryUI.onClickedSwapSkinsButton);
			}
			sleekButton2.onClickedButton = PlayerDashboardInventoryUI.<>f__mg$cache1B;
			PlayerDashboardInventoryUI.character.add(PlayerDashboardInventoryUI.swapSkinsButton);
			PlayerDashboardInventoryUI.swapMythicsButton = new SleekButtonIcon((Texture2D)PlayerDashboardInventoryUI.icons.load("Swap_Mythics"));
			PlayerDashboardInventoryUI.swapMythicsButton.positionOffset_X = 80;
			PlayerDashboardInventoryUI.swapMythicsButton.positionOffset_Y = 10;
			PlayerDashboardInventoryUI.swapMythicsButton.positionScale_Y = 1f;
			PlayerDashboardInventoryUI.swapMythicsButton.sizeOffset_X = 30;
			PlayerDashboardInventoryUI.swapMythicsButton.sizeOffset_Y = 30;
			PlayerDashboardInventoryUI.swapMythicsButton.tooltip = PlayerDashboardInventoryUI.localization.format("Swap_Mythics_Tooltip");
			PlayerDashboardInventoryUI.swapMythicsButton.iconImage.backgroundTint = ESleekTint.FOREGROUND;
			SleekButton sleekButton3 = PlayerDashboardInventoryUI.swapMythicsButton;
			if (PlayerDashboardInventoryUI.<>f__mg$cache1C == null)
			{
				PlayerDashboardInventoryUI.<>f__mg$cache1C = new ClickedButton(PlayerDashboardInventoryUI.onClickedSwapMythicsButton);
			}
			sleekButton3.onClickedButton = PlayerDashboardInventoryUI.<>f__mg$cache1C;
			PlayerDashboardInventoryUI.character.add(PlayerDashboardInventoryUI.swapMythicsButton);
			PlayerDashboardInventoryUI.box = new Sleek();
			PlayerDashboardInventoryUI.box.positionOffset_X = 430;
			PlayerDashboardInventoryUI.box.positionOffset_Y = 10;
			PlayerDashboardInventoryUI.box.sizeOffset_X = -440;
			PlayerDashboardInventoryUI.box.sizeOffset_Y = -20;
			PlayerDashboardInventoryUI.box.sizeScale_X = 1f;
			PlayerDashboardInventoryUI.box.sizeScale_Y = 1f;
			PlayerDashboardInventoryUI.backdropBox.add(PlayerDashboardInventoryUI.box);
			PlayerDashboardInventoryUI.clothingBox = new SleekScrollBox();
			PlayerDashboardInventoryUI.clothingBox.positionOffset_X = -100;
			PlayerDashboardInventoryUI.clothingBox.sizeScale_X = 1f;
			PlayerDashboardInventoryUI.clothingBox.sizeScale_Y = 1f;
			PlayerDashboardInventoryUI.clothingBox.area = new Rect(0f, 0f, 5f, 1000f);
			PlayerDashboardInventoryUI.box.add(PlayerDashboardInventoryUI.clothingBox);
			PlayerDashboardInventoryUI.areaBox = new SleekScrollBox();
			PlayerDashboardInventoryUI.areaBox.positionOffset_X = -95;
			PlayerDashboardInventoryUI.areaBox.positionScale_X = 0.5f;
			PlayerDashboardInventoryUI.areaBox.sizeOffset_X = 95;
			PlayerDashboardInventoryUI.areaBox.sizeScale_X = 0.5f;
			PlayerDashboardInventoryUI.areaBox.sizeScale_Y = 1f;
			PlayerDashboardInventoryUI.areaBox.area = new Rect(0f, 0f, 5f, 1000f);
			PlayerDashboardInventoryUI.box.add(PlayerDashboardInventoryUI.areaBox);
			PlayerDashboardInventoryUI.headers = new SleekButton[(int)(PlayerInventory.PAGES - PlayerInventory.SLOTS + 3)];
			byte b2 = 0;
			while ((int)b2 < PlayerDashboardInventoryUI.headers.Length)
			{
				PlayerDashboardInventoryUI.headers[(int)b2] = new SleekButton();
				PlayerDashboardInventoryUI.headers[(int)b2].positionOffset_X = 100;
				PlayerDashboardInventoryUI.headers[(int)b2].sizeOffset_X = -130;
				PlayerDashboardInventoryUI.headers[(int)b2].sizeOffset_Y = 60;
				PlayerDashboardInventoryUI.headers[(int)b2].sizeScale_X = 1f;
				PlayerDashboardInventoryUI.headers[(int)b2].fontSize = 14;
				SleekButton sleekButton4 = PlayerDashboardInventoryUI.headers[(int)b2];
				if (PlayerDashboardInventoryUI.<>f__mg$cache1D == null)
				{
					PlayerDashboardInventoryUI.<>f__mg$cache1D = new ClickedButton(PlayerDashboardInventoryUI.onClickedHeader);
				}
				sleekButton4.onClickedButton = PlayerDashboardInventoryUI.<>f__mg$cache1D;
				PlayerDashboardInventoryUI.headers[(int)b2].backgroundTint = ESleekTint.NONE;
				PlayerDashboardInventoryUI.headers[(int)b2].foregroundTint = ESleekTint.NONE;
				PlayerDashboardInventoryUI.clothingBox.add(PlayerDashboardInventoryUI.headers[(int)b2]);
				PlayerDashboardInventoryUI.headers[(int)b2].isVisible = false;
				b2 += 1;
			}
			PlayerDashboardInventoryUI.headers[0].isVisible = true;
			PlayerDashboardInventoryUI.headers[(int)(PlayerInventory.AREA - PlayerInventory.SLOTS)].isVisible = true;
			byte b3 = 1;
			while ((int)b3 < PlayerDashboardInventoryUI.headers.Length)
			{
				if (b3 != PlayerInventory.STORAGE - PlayerInventory.SLOTS && b3 != PlayerInventory.AREA - PlayerInventory.SLOTS)
				{
					SleekImageTexture sleekImageTexture = new SleekImageTexture();
					sleekImageTexture.positionOffset_X = 5;
					sleekImageTexture.positionScale_Y = 0.5f;
					PlayerDashboardInventoryUI.headers[(int)b3].add(sleekImageTexture);
					SleekImageTexture sleekImageTexture2 = new SleekImageTexture();
					sleekImageTexture2.positionOffset_X = -25;
					sleekImageTexture2.positionOffset_Y = -25;
					sleekImageTexture2.positionScale_X = 1f;
					sleekImageTexture2.positionScale_Y = 1f;
					sleekImageTexture2.sizeOffset_X = 20;
					sleekImageTexture2.sizeOffset_Y = 20;
					sleekImageTexture2.texture = (Texture2D)PlayerDashboardInventoryUI.icons.load("Quality_0");
					sleekImageTexture2.backgroundTint = ESleekTint.NONE;
					PlayerDashboardInventoryUI.headers[(int)b3].add(sleekImageTexture2);
					SleekLabel sleekLabel = new SleekLabel();
					sleekLabel.positionOffset_X = -110;
					sleekLabel.positionOffset_Y = 10;
					sleekLabel.positionScale_X = 1f;
					sleekLabel.sizeOffset_X = 100;
					sleekLabel.sizeOffset_Y = 40;
					sleekLabel.foregroundTint = ESleekTint.NONE;
					PlayerDashboardInventoryUI.headers[(int)b3].add(sleekLabel);
				}
				b3 += 1;
			}
			PlayerDashboardInventoryUI.headers[0].text = PlayerDashboardInventoryUI.localization.format("Hands");
			PlayerDashboardInventoryUI.headers[(int)(PlayerInventory.STORAGE - PlayerInventory.SLOTS)].text = PlayerDashboardInventoryUI.localization.format("Storage");
			PlayerDashboardInventoryUI.headers[(int)(PlayerInventory.AREA - PlayerInventory.SLOTS)].text = PlayerDashboardInventoryUI.localization.format("Area");
			PlayerDashboardInventoryUI.onShirtUpdated(Player.player.clothing.shirt, Player.player.clothing.shirtQuality, Player.player.clothing.shirtState);
			PlayerDashboardInventoryUI.onPantsUpdated(Player.player.clothing.pants, Player.player.clothing.pantsQuality, Player.player.clothing.pantsState);
			PlayerDashboardInventoryUI.onBackpackUpdated(Player.player.clothing.backpack, Player.player.clothing.backpackQuality, Player.player.clothing.backpackState);
			PlayerDashboardInventoryUI.onVestUpdated(Player.player.clothing.vest, Player.player.clothing.vestQuality, Player.player.clothing.vestState);
			PlayerDashboardInventoryUI.items = new SleekItems[(int)(PlayerInventory.PAGES - PlayerInventory.SLOTS)];
			byte b4 = 0;
			while ((int)b4 < PlayerDashboardInventoryUI.items.Length)
			{
				PlayerDashboardInventoryUI.items[(int)b4] = new SleekItems(PlayerInventory.SLOTS + b4);
				PlayerDashboardInventoryUI.items[(int)b4].positionOffset_X = 100;
				PlayerDashboardInventoryUI.items[(int)b4].sizeOffset_X = -130;
				SleekItems sleekItems = PlayerDashboardInventoryUI.items[(int)b4];
				if (PlayerDashboardInventoryUI.<>f__mg$cache1E == null)
				{
					PlayerDashboardInventoryUI.<>f__mg$cache1E = new SelectedItem(PlayerDashboardInventoryUI.onSelectedItem);
				}
				sleekItems.onSelectedItem = PlayerDashboardInventoryUI.<>f__mg$cache1E;
				SleekItems sleekItems2 = PlayerDashboardInventoryUI.items[(int)b4];
				if (PlayerDashboardInventoryUI.<>f__mg$cache1F == null)
				{
					PlayerDashboardInventoryUI.<>f__mg$cache1F = new GrabbedItem(PlayerDashboardInventoryUI.onGrabbedItem);
				}
				sleekItems2.onGrabbedItem = PlayerDashboardInventoryUI.<>f__mg$cache1F;
				SleekItems sleekItems3 = PlayerDashboardInventoryUI.items[(int)b4];
				if (PlayerDashboardInventoryUI.<>f__mg$cache20 == null)
				{
					PlayerDashboardInventoryUI.<>f__mg$cache20 = new PlacedItem(PlayerDashboardInventoryUI.onPlacedItem);
				}
				sleekItems3.onPlacedItem = PlayerDashboardInventoryUI.<>f__mg$cache20;
				PlayerDashboardInventoryUI.clothingBox.add(PlayerDashboardInventoryUI.items[(int)b4]);
				b4 += 1;
			}
			PlayerDashboardInventoryUI.areaItems = new Items(PlayerInventory.AREA);
			PlayerDashboardInventoryUI.actions = new List<Action>();
			PlayerDashboardInventoryUI.selectionBackdropBox = new SleekBox();
			PlayerDashboardInventoryUI.selectionBackdropBox.sizeOffset_X = 530;
			PlayerDashboardInventoryUI.selectionBackdropBox.sizeOffset_Y = 440;
			PlayerDashboardInventoryUI.backdropBox.backgroundColor = white;
			PlayerDashboardInventoryUI.container.add(PlayerDashboardInventoryUI.selectionBackdropBox);
			PlayerDashboardInventoryUI.selectionBackdropBox.isVisible = false;
			PlayerDashboardInventoryUI.selectionBackdropBox.hideTooltip = true;
			PlayerDashboardInventoryUI.selectionIconBox = new SleekBox();
			PlayerDashboardInventoryUI.selectionIconBox.positionOffset_X = 10;
			PlayerDashboardInventoryUI.selectionIconBox.positionOffset_Y = 10;
			PlayerDashboardInventoryUI.selectionIconBox.sizeOffset_X = 510;
			PlayerDashboardInventoryUI.selectionIconBox.sizeOffset_Y = 310;
			PlayerDashboardInventoryUI.selectionBackdropBox.add(PlayerDashboardInventoryUI.selectionIconBox);
			PlayerDashboardInventoryUI.selectionIconImage = new SleekImageTexture();
			PlayerDashboardInventoryUI.selectionIconImage.positionScale_X = 0.5f;
			PlayerDashboardInventoryUI.selectionIconImage.positionScale_Y = 0.5f;
			PlayerDashboardInventoryUI.selectionIconBox.add(PlayerDashboardInventoryUI.selectionIconImage);
			PlayerDashboardInventoryUI.selectionIconImage.isVisible = false;
			PlayerDashboardInventoryUI.selectionDescriptionBox = new SleekBox();
			PlayerDashboardInventoryUI.selectionDescriptionBox.positionOffset_X = 10;
			PlayerDashboardInventoryUI.selectionDescriptionBox.positionOffset_Y = 330;
			PlayerDashboardInventoryUI.selectionDescriptionBox.sizeOffset_X = 250;
			PlayerDashboardInventoryUI.selectionDescriptionBox.sizeOffset_Y = 100;
			PlayerDashboardInventoryUI.selectionDescriptionBox.backgroundTint = ESleekTint.NONE;
			PlayerDashboardInventoryUI.selectionDescriptionBox.foregroundTint = ESleekTint.NONE;
			PlayerDashboardInventoryUI.selectionBackdropBox.add(PlayerDashboardInventoryUI.selectionDescriptionBox);
			PlayerDashboardInventoryUI.selectionDescriptionLabel = new SleekLabel();
			PlayerDashboardInventoryUI.selectionDescriptionLabel.isRich = true;
			PlayerDashboardInventoryUI.selectionDescriptionLabel.positionOffset_X = 5;
			PlayerDashboardInventoryUI.selectionDescriptionLabel.positionOffset_Y = 5;
			PlayerDashboardInventoryUI.selectionDescriptionLabel.sizeOffset_X = -10;
			PlayerDashboardInventoryUI.selectionDescriptionLabel.sizeOffset_Y = -10;
			PlayerDashboardInventoryUI.selectionDescriptionLabel.sizeScale_X = 1f;
			PlayerDashboardInventoryUI.selectionDescriptionLabel.sizeScale_Y = 1f;
			PlayerDashboardInventoryUI.selectionDescriptionLabel.fontAlignment = 0;
			PlayerDashboardInventoryUI.selectionDescriptionLabel.foregroundTint = ESleekTint.NONE;
			PlayerDashboardInventoryUI.selectionDescriptionBox.add(PlayerDashboardInventoryUI.selectionDescriptionLabel);
			PlayerDashboardInventoryUI.selectionNameLabel = new SleekLabel();
			PlayerDashboardInventoryUI.selectionNameLabel.positionOffset_Y = -70;
			PlayerDashboardInventoryUI.selectionNameLabel.positionScale_Y = 1f;
			PlayerDashboardInventoryUI.selectionNameLabel.sizeOffset_Y = 70;
			PlayerDashboardInventoryUI.selectionNameLabel.sizeScale_X = 1f;
			PlayerDashboardInventoryUI.selectionNameLabel.fontSize = 18;
			PlayerDashboardInventoryUI.selectionNameLabel.foregroundTint = ESleekTint.NONE;
			PlayerDashboardInventoryUI.selectionIconBox.add(PlayerDashboardInventoryUI.selectionNameLabel);
			PlayerDashboardInventoryUI.selectionHotkeyLabel = new SleekLabel();
			PlayerDashboardInventoryUI.selectionHotkeyLabel.positionOffset_X = 5;
			PlayerDashboardInventoryUI.selectionHotkeyLabel.positionOffset_Y = 5;
			PlayerDashboardInventoryUI.selectionHotkeyLabel.sizeOffset_X = -10;
			PlayerDashboardInventoryUI.selectionHotkeyLabel.sizeOffset_Y = 30;
			PlayerDashboardInventoryUI.selectionHotkeyLabel.sizeScale_X = 1f;
			PlayerDashboardInventoryUI.selectionHotkeyLabel.fontAlignment = 2;
			PlayerDashboardInventoryUI.selectionIconBox.add(PlayerDashboardInventoryUI.selectionHotkeyLabel);
			PlayerDashboardInventoryUI.selectionActionsBox = new SleekScrollBox();
			PlayerDashboardInventoryUI.selectionActionsBox.positionOffset_X = 270;
			PlayerDashboardInventoryUI.selectionActionsBox.positionOffset_Y = 330;
			PlayerDashboardInventoryUI.selectionActionsBox.sizeOffset_X = 280;
			PlayerDashboardInventoryUI.selectionActionsBox.sizeOffset_Y = 100;
			PlayerDashboardInventoryUI.selectionBackdropBox.add(PlayerDashboardInventoryUI.selectionActionsBox);
			PlayerDashboardInventoryUI.selectionEquipButton = new SleekButton();
			PlayerDashboardInventoryUI.selectionEquipButton.sizeOffset_X = -30;
			PlayerDashboardInventoryUI.selectionEquipButton.sizeScale_X = 1f;
			PlayerDashboardInventoryUI.selectionEquipButton.sizeOffset_Y = 30;
			SleekButton sleekButton5 = PlayerDashboardInventoryUI.selectionEquipButton;
			if (PlayerDashboardInventoryUI.<>f__mg$cache21 == null)
			{
				PlayerDashboardInventoryUI.<>f__mg$cache21 = new ClickedButton(PlayerDashboardInventoryUI.onClickedEquip);
			}
			sleekButton5.onClickedButton = PlayerDashboardInventoryUI.<>f__mg$cache21;
			PlayerDashboardInventoryUI.selectionActionsBox.add(PlayerDashboardInventoryUI.selectionEquipButton);
			PlayerDashboardInventoryUI.selectionContextButton = new SleekButton();
			PlayerDashboardInventoryUI.selectionContextButton.sizeOffset_X = -30;
			PlayerDashboardInventoryUI.selectionContextButton.sizeScale_X = 1f;
			PlayerDashboardInventoryUI.selectionContextButton.sizeOffset_Y = 30;
			SleekButton sleekButton6 = PlayerDashboardInventoryUI.selectionContextButton;
			if (PlayerDashboardInventoryUI.<>f__mg$cache22 == null)
			{
				PlayerDashboardInventoryUI.<>f__mg$cache22 = new ClickedButton(PlayerDashboardInventoryUI.onClickedContext);
			}
			sleekButton6.onClickedButton = PlayerDashboardInventoryUI.<>f__mg$cache22;
			PlayerDashboardInventoryUI.selectionActionsBox.add(PlayerDashboardInventoryUI.selectionContextButton);
			PlayerDashboardInventoryUI.selectionDropButton = new SleekButton();
			PlayerDashboardInventoryUI.selectionDropButton.sizeOffset_X = -30;
			PlayerDashboardInventoryUI.selectionDropButton.sizeScale_X = 1f;
			PlayerDashboardInventoryUI.selectionDropButton.sizeOffset_Y = 30;
			SleekButton sleekButton7 = PlayerDashboardInventoryUI.selectionDropButton;
			if (PlayerDashboardInventoryUI.<>f__mg$cache23 == null)
			{
				PlayerDashboardInventoryUI.<>f__mg$cache23 = new ClickedButton(PlayerDashboardInventoryUI.onClickedDrop);
			}
			sleekButton7.onClickedButton = PlayerDashboardInventoryUI.<>f__mg$cache23;
			PlayerDashboardInventoryUI.selectionActionsBox.add(PlayerDashboardInventoryUI.selectionDropButton);
			PlayerDashboardInventoryUI.selectionStorageButton = new SleekButton();
			PlayerDashboardInventoryUI.selectionStorageButton.sizeOffset_X = -30;
			PlayerDashboardInventoryUI.selectionStorageButton.sizeScale_X = 1f;
			PlayerDashboardInventoryUI.selectionStorageButton.sizeOffset_Y = 30;
			SleekButton sleekButton8 = PlayerDashboardInventoryUI.selectionStorageButton;
			if (PlayerDashboardInventoryUI.<>f__mg$cache24 == null)
			{
				PlayerDashboardInventoryUI.<>f__mg$cache24 = new ClickedButton(PlayerDashboardInventoryUI.onClickedStore);
			}
			sleekButton8.onClickedButton = PlayerDashboardInventoryUI.<>f__mg$cache24;
			PlayerDashboardInventoryUI.selectionActionsBox.add(PlayerDashboardInventoryUI.selectionStorageButton);
			PlayerDashboardInventoryUI.selectionExtraActionsBox = new Sleek();
			PlayerDashboardInventoryUI.selectionExtraActionsBox.sizeScale_X = 1f;
			PlayerDashboardInventoryUI.selectionActionsBox.add(PlayerDashboardInventoryUI.selectionExtraActionsBox);
			PlayerDashboardInventoryUI.vehicleBox = new SleekBox();
			PlayerDashboardInventoryUI.vehicleBox.positionOffset_X = 100;
			PlayerDashboardInventoryUI.vehicleBox.sizeOffset_X = -130;
			PlayerDashboardInventoryUI.vehicleBox.sizeScale_X = 1f;
			PlayerDashboardInventoryUI.clothingBox.add(PlayerDashboardInventoryUI.vehicleBox);
			PlayerDashboardInventoryUI.vehicleBox.hideTooltip = true;
			PlayerDashboardInventoryUI.vehicleNameLabel = new SleekLabel();
			PlayerDashboardInventoryUI.vehicleNameLabel.sizeOffset_Y = 60;
			PlayerDashboardInventoryUI.vehicleNameLabel.sizeScale_X = 1f;
			PlayerDashboardInventoryUI.vehicleNameLabel.fontSize = 14;
			PlayerDashboardInventoryUI.vehicleNameLabel.foregroundTint = ESleekTint.NONE;
			PlayerDashboardInventoryUI.vehicleBox.add(PlayerDashboardInventoryUI.vehicleNameLabel);
			PlayerDashboardInventoryUI.vehicleActionsBox = new Sleek();
			PlayerDashboardInventoryUI.vehicleActionsBox.positionOffset_X = 10;
			PlayerDashboardInventoryUI.vehicleActionsBox.positionOffset_Y = 60;
			PlayerDashboardInventoryUI.vehicleActionsBox.sizeOffset_X = 250;
			PlayerDashboardInventoryUI.vehicleBox.add(PlayerDashboardInventoryUI.vehicleActionsBox);
			PlayerDashboardInventoryUI.vehicleLockButton = new SleekButton();
			PlayerDashboardInventoryUI.vehicleLockButton.sizeOffset_Y = 30;
			PlayerDashboardInventoryUI.vehicleLockButton.sizeScale_X = 1f;
			SleekButton sleekButton9 = PlayerDashboardInventoryUI.vehicleLockButton;
			if (PlayerDashboardInventoryUI.<>f__mg$cache25 == null)
			{
				PlayerDashboardInventoryUI.<>f__mg$cache25 = new ClickedButton(PlayerDashboardInventoryUI.onClickedVehicleLockButton);
			}
			sleekButton9.onClickedButton = PlayerDashboardInventoryUI.<>f__mg$cache25;
			PlayerDashboardInventoryUI.vehicleActionsBox.add(PlayerDashboardInventoryUI.vehicleLockButton);
			PlayerDashboardInventoryUI.vehicleLockButton.isVisible = false;
			PlayerDashboardInventoryUI.vehicleHornButton = new SleekButton();
			PlayerDashboardInventoryUI.vehicleHornButton.sizeOffset_Y = 30;
			PlayerDashboardInventoryUI.vehicleHornButton.sizeScale_X = 1f;
			SleekButton sleekButton10 = PlayerDashboardInventoryUI.vehicleHornButton;
			if (PlayerDashboardInventoryUI.<>f__mg$cache26 == null)
			{
				PlayerDashboardInventoryUI.<>f__mg$cache26 = new ClickedButton(PlayerDashboardInventoryUI.onClickedVehicleHornButton);
			}
			sleekButton10.onClickedButton = PlayerDashboardInventoryUI.<>f__mg$cache26;
			PlayerDashboardInventoryUI.vehicleActionsBox.add(PlayerDashboardInventoryUI.vehicleHornButton);
			PlayerDashboardInventoryUI.vehicleHornButton.isVisible = false;
			PlayerDashboardInventoryUI.vehicleHeadlightsButton = new SleekButton();
			PlayerDashboardInventoryUI.vehicleHeadlightsButton.sizeOffset_Y = 30;
			PlayerDashboardInventoryUI.vehicleHeadlightsButton.sizeScale_X = 1f;
			SleekButton sleekButton11 = PlayerDashboardInventoryUI.vehicleHeadlightsButton;
			if (PlayerDashboardInventoryUI.<>f__mg$cache27 == null)
			{
				PlayerDashboardInventoryUI.<>f__mg$cache27 = new ClickedButton(PlayerDashboardInventoryUI.onClickedVehicleHeadlightsButton);
			}
			sleekButton11.onClickedButton = PlayerDashboardInventoryUI.<>f__mg$cache27;
			PlayerDashboardInventoryUI.vehicleActionsBox.add(PlayerDashboardInventoryUI.vehicleHeadlightsButton);
			PlayerDashboardInventoryUI.vehicleHeadlightsButton.isVisible = false;
			PlayerDashboardInventoryUI.vehicleSirensButton = new SleekButton();
			PlayerDashboardInventoryUI.vehicleSirensButton.sizeOffset_Y = 30;
			PlayerDashboardInventoryUI.vehicleSirensButton.sizeScale_X = 1f;
			SleekButton sleekButton12 = PlayerDashboardInventoryUI.vehicleSirensButton;
			if (PlayerDashboardInventoryUI.<>f__mg$cache28 == null)
			{
				PlayerDashboardInventoryUI.<>f__mg$cache28 = new ClickedButton(PlayerDashboardInventoryUI.onClickedVehicleSirensButton);
			}
			sleekButton12.onClickedButton = PlayerDashboardInventoryUI.<>f__mg$cache28;
			PlayerDashboardInventoryUI.vehicleActionsBox.add(PlayerDashboardInventoryUI.vehicleSirensButton);
			PlayerDashboardInventoryUI.vehicleSirensButton.isVisible = false;
			PlayerDashboardInventoryUI.vehicleHookButton = new SleekButton();
			PlayerDashboardInventoryUI.vehicleHookButton.sizeOffset_Y = 30;
			PlayerDashboardInventoryUI.vehicleHookButton.sizeScale_X = 1f;
			SleekButton sleekButton13 = PlayerDashboardInventoryUI.vehicleHookButton;
			if (PlayerDashboardInventoryUI.<>f__mg$cache29 == null)
			{
				PlayerDashboardInventoryUI.<>f__mg$cache29 = new ClickedButton(PlayerDashboardInventoryUI.onClickedVehicleHookButton);
			}
			sleekButton13.onClickedButton = PlayerDashboardInventoryUI.<>f__mg$cache29;
			PlayerDashboardInventoryUI.vehicleActionsBox.add(PlayerDashboardInventoryUI.vehicleHookButton);
			PlayerDashboardInventoryUI.vehicleHookButton.isVisible = false;
			PlayerDashboardInventoryUI.vehicleStealBatteryButton = new SleekButton();
			PlayerDashboardInventoryUI.vehicleStealBatteryButton.sizeOffset_Y = 30;
			PlayerDashboardInventoryUI.vehicleStealBatteryButton.sizeScale_X = 1f;
			SleekButton sleekButton14 = PlayerDashboardInventoryUI.vehicleStealBatteryButton;
			if (PlayerDashboardInventoryUI.<>f__mg$cache2A == null)
			{
				PlayerDashboardInventoryUI.<>f__mg$cache2A = new ClickedButton(PlayerDashboardInventoryUI.onClickedVehicleStealBatteryButton);
			}
			sleekButton14.onClickedButton = PlayerDashboardInventoryUI.<>f__mg$cache2A;
			PlayerDashboardInventoryUI.vehicleActionsBox.add(PlayerDashboardInventoryUI.vehicleStealBatteryButton);
			PlayerDashboardInventoryUI.vehicleStealBatteryButton.isVisible = false;
			PlayerDashboardInventoryUI.vehiclePassengersBox = new Sleek();
			PlayerDashboardInventoryUI.vehiclePassengersBox.positionOffset_Y = 60;
			PlayerDashboardInventoryUI.vehiclePassengersBox.sizeScale_X = 1f;
			PlayerDashboardInventoryUI.vehicleBox.add(PlayerDashboardInventoryUI.vehiclePassengersBox);
			PlayerDashboardInventoryUI.rot_xButton = new SleekButton();
			PlayerDashboardInventoryUI.rot_xButton.positionScale_X = 1f;
			PlayerDashboardInventoryUI.rot_xButton.sizeOffset_X = 60;
			PlayerDashboardInventoryUI.rot_xButton.sizeOffset_Y = 60;
			SleekButton sleekButton15 = PlayerDashboardInventoryUI.rot_xButton;
			if (PlayerDashboardInventoryUI.<>f__mg$cache2B == null)
			{
				PlayerDashboardInventoryUI.<>f__mg$cache2B = new ClickedButton(PlayerDashboardInventoryUI.onClickedRot_XButton);
			}
			sleekButton15.onClickedButton = PlayerDashboardInventoryUI.<>f__mg$cache2B;
			PlayerDashboardInventoryUI.rot_xButton.text = PlayerDashboardInventoryUI.localization.format("Rot_X");
			PlayerDashboardInventoryUI.headers[(int)(PlayerInventory.STORAGE - PlayerInventory.SLOTS)].add(PlayerDashboardInventoryUI.rot_xButton);
			PlayerDashboardInventoryUI.rot_xButton.isVisible = false;
			PlayerDashboardInventoryUI.rot_yButton = new SleekButton();
			PlayerDashboardInventoryUI.rot_yButton.positionScale_X = 1f;
			PlayerDashboardInventoryUI.rot_yButton.positionOffset_X = 60;
			PlayerDashboardInventoryUI.rot_yButton.sizeOffset_X = 60;
			PlayerDashboardInventoryUI.rot_yButton.sizeOffset_Y = 60;
			SleekButton sleekButton16 = PlayerDashboardInventoryUI.rot_yButton;
			if (PlayerDashboardInventoryUI.<>f__mg$cache2C == null)
			{
				PlayerDashboardInventoryUI.<>f__mg$cache2C = new ClickedButton(PlayerDashboardInventoryUI.onClickedRot_YButton);
			}
			sleekButton16.onClickedButton = PlayerDashboardInventoryUI.<>f__mg$cache2C;
			PlayerDashboardInventoryUI.rot_yButton.text = PlayerDashboardInventoryUI.localization.format("Rot_Y");
			PlayerDashboardInventoryUI.headers[(int)(PlayerInventory.STORAGE - PlayerInventory.SLOTS)].add(PlayerDashboardInventoryUI.rot_yButton);
			PlayerDashboardInventoryUI.rot_yButton.isVisible = false;
			PlayerDashboardInventoryUI.rot_zButton = new SleekButton();
			PlayerDashboardInventoryUI.rot_zButton.positionScale_X = 1f;
			PlayerDashboardInventoryUI.rot_zButton.positionOffset_X = 120;
			PlayerDashboardInventoryUI.rot_zButton.sizeOffset_X = 60;
			PlayerDashboardInventoryUI.rot_zButton.sizeOffset_Y = 60;
			SleekButton sleekButton17 = PlayerDashboardInventoryUI.rot_zButton;
			if (PlayerDashboardInventoryUI.<>f__mg$cache2D == null)
			{
				PlayerDashboardInventoryUI.<>f__mg$cache2D = new ClickedButton(PlayerDashboardInventoryUI.onClickedRot_ZButton);
			}
			sleekButton17.onClickedButton = PlayerDashboardInventoryUI.<>f__mg$cache2D;
			PlayerDashboardInventoryUI.rot_zButton.text = PlayerDashboardInventoryUI.localization.format("Rot_Z");
			PlayerDashboardInventoryUI.headers[(int)(PlayerInventory.STORAGE - PlayerInventory.SLOTS)].add(PlayerDashboardInventoryUI.rot_zButton);
			PlayerDashboardInventoryUI.rot_zButton.isVisible = false;
			PlayerDashboardInventoryUI.dragItem = new SleekItem();
			PlayerUI.container.add(PlayerDashboardInventoryUI.dragItem);
			PlayerDashboardInventoryUI.dragItem.isVisible = false;
			PlayerDashboardInventoryUI.dragItem.hideTooltip = true;
			PlayerDashboardInventoryUI.dragOffset = Vector2.zero;
			PlayerDashboardInventoryUI.dragPivot = Vector2.zero;
			PlayerDashboardInventoryUI.dragPage = byte.MaxValue;
			PlayerDashboardInventoryUI.drag_x = byte.MaxValue;
			PlayerDashboardInventoryUI.drag_y = byte.MaxValue;
			PlayerDashboardInventoryUI.dragRot = 0;
			PlayerInventory inventory = Player.player.inventory;
			Delegate onInventoryResized = inventory.onInventoryResized;
			if (PlayerDashboardInventoryUI.<>f__mg$cache2E == null)
			{
				PlayerDashboardInventoryUI.<>f__mg$cache2E = new InventoryResized(PlayerDashboardInventoryUI.onInventoryResized);
			}
			inventory.onInventoryResized = (InventoryResized)Delegate.Combine(onInventoryResized, PlayerDashboardInventoryUI.<>f__mg$cache2E);
			PlayerInventory inventory2 = Player.player.inventory;
			Delegate onInventoryUpdated = inventory2.onInventoryUpdated;
			if (PlayerDashboardInventoryUI.<>f__mg$cache2F == null)
			{
				PlayerDashboardInventoryUI.<>f__mg$cache2F = new InventoryUpdated(PlayerDashboardInventoryUI.onInventoryUpdated);
			}
			inventory2.onInventoryUpdated = (InventoryUpdated)Delegate.Combine(onInventoryUpdated, PlayerDashboardInventoryUI.<>f__mg$cache2F);
			PlayerInventory inventory3 = Player.player.inventory;
			Delegate onInventoryAdded = inventory3.onInventoryAdded;
			if (PlayerDashboardInventoryUI.<>f__mg$cache30 == null)
			{
				PlayerDashboardInventoryUI.<>f__mg$cache30 = new InventoryAdded(PlayerDashboardInventoryUI.onInventoryAdded);
			}
			inventory3.onInventoryAdded = (InventoryAdded)Delegate.Combine(onInventoryAdded, PlayerDashboardInventoryUI.<>f__mg$cache30);
			PlayerInventory inventory4 = Player.player.inventory;
			Delegate onInventoryRemoved = inventory4.onInventoryRemoved;
			if (PlayerDashboardInventoryUI.<>f__mg$cache31 == null)
			{
				PlayerDashboardInventoryUI.<>f__mg$cache31 = new InventoryRemoved(PlayerDashboardInventoryUI.onInventoryRemoved);
			}
			inventory4.onInventoryRemoved = (InventoryRemoved)Delegate.Combine(onInventoryRemoved, PlayerDashboardInventoryUI.<>f__mg$cache31);
			PlayerInventory inventory5 = Player.player.inventory;
			Delegate onInventoryStored = inventory5.onInventoryStored;
			if (PlayerDashboardInventoryUI.<>f__mg$cache32 == null)
			{
				PlayerDashboardInventoryUI.<>f__mg$cache32 = new InventoryStored(PlayerDashboardInventoryUI.onInventoryStored);
			}
			inventory5.onInventoryStored = (InventoryStored)Delegate.Combine(onInventoryStored, PlayerDashboardInventoryUI.<>f__mg$cache32);
			PlayerEquipment equipment = Player.player.equipment;
			Delegate onHotkeysUpdated = equipment.onHotkeysUpdated;
			if (PlayerDashboardInventoryUI.<>f__mg$cache33 == null)
			{
				PlayerDashboardInventoryUI.<>f__mg$cache33 = new HotkeysUpdated(PlayerDashboardInventoryUI.onHotkeysUpdated);
			}
			equipment.onHotkeysUpdated = (HotkeysUpdated)Delegate.Combine(onHotkeysUpdated, PlayerDashboardInventoryUI.<>f__mg$cache33);
			if (PlayerDashboardInventoryUI.<>f__mg$cache34 == null)
			{
				PlayerDashboardInventoryUI.<>f__mg$cache34 = new ItemDropAdded(PlayerDashboardInventoryUI.onItemDropAdded);
			}
			ItemManager.onItemDropAdded = PlayerDashboardInventoryUI.<>f__mg$cache34;
			if (PlayerDashboardInventoryUI.<>f__mg$cache35 == null)
			{
				PlayerDashboardInventoryUI.<>f__mg$cache35 = new ItemDropRemoved(PlayerDashboardInventoryUI.onItemDropRemoved);
			}
			ItemManager.onItemDropRemoved = PlayerDashboardInventoryUI.<>f__mg$cache35;
			PlayerMovement movement = Player.player.movement;
			Delegate onSeated = movement.onSeated;
			if (PlayerDashboardInventoryUI.<>f__mg$cache36 == null)
			{
				PlayerDashboardInventoryUI.<>f__mg$cache36 = new Seated(PlayerDashboardInventoryUI.onSeated);
			}
			movement.onSeated = (Seated)Delegate.Combine(onSeated, PlayerDashboardInventoryUI.<>f__mg$cache36);
			PlayerClothing clothing = Player.player.clothing;
			Delegate onShirtUpdated = clothing.onShirtUpdated;
			if (PlayerDashboardInventoryUI.<>f__mg$cache37 == null)
			{
				PlayerDashboardInventoryUI.<>f__mg$cache37 = new ShirtUpdated(PlayerDashboardInventoryUI.onShirtUpdated);
			}
			clothing.onShirtUpdated = (ShirtUpdated)Delegate.Combine(onShirtUpdated, PlayerDashboardInventoryUI.<>f__mg$cache37);
			PlayerClothing clothing2 = Player.player.clothing;
			Delegate onPantsUpdated = clothing2.onPantsUpdated;
			if (PlayerDashboardInventoryUI.<>f__mg$cache38 == null)
			{
				PlayerDashboardInventoryUI.<>f__mg$cache38 = new PantsUpdated(PlayerDashboardInventoryUI.onPantsUpdated);
			}
			clothing2.onPantsUpdated = (PantsUpdated)Delegate.Combine(onPantsUpdated, PlayerDashboardInventoryUI.<>f__mg$cache38);
			PlayerClothing clothing3 = Player.player.clothing;
			Delegate onHatUpdated = clothing3.onHatUpdated;
			if (PlayerDashboardInventoryUI.<>f__mg$cache39 == null)
			{
				PlayerDashboardInventoryUI.<>f__mg$cache39 = new HatUpdated(PlayerDashboardInventoryUI.onHatUpdated);
			}
			clothing3.onHatUpdated = (HatUpdated)Delegate.Combine(onHatUpdated, PlayerDashboardInventoryUI.<>f__mg$cache39);
			PlayerClothing clothing4 = Player.player.clothing;
			Delegate onBackpackUpdated = clothing4.onBackpackUpdated;
			if (PlayerDashboardInventoryUI.<>f__mg$cache3A == null)
			{
				PlayerDashboardInventoryUI.<>f__mg$cache3A = new BackpackUpdated(PlayerDashboardInventoryUI.onBackpackUpdated);
			}
			clothing4.onBackpackUpdated = (BackpackUpdated)Delegate.Combine(onBackpackUpdated, PlayerDashboardInventoryUI.<>f__mg$cache3A);
			PlayerClothing clothing5 = Player.player.clothing;
			Delegate onVestUpdated = clothing5.onVestUpdated;
			if (PlayerDashboardInventoryUI.<>f__mg$cache3B == null)
			{
				PlayerDashboardInventoryUI.<>f__mg$cache3B = new VestUpdated(PlayerDashboardInventoryUI.onVestUpdated);
			}
			clothing5.onVestUpdated = (VestUpdated)Delegate.Combine(onVestUpdated, PlayerDashboardInventoryUI.<>f__mg$cache3B);
			PlayerClothing clothing6 = Player.player.clothing;
			Delegate onMaskUpdated = clothing6.onMaskUpdated;
			if (PlayerDashboardInventoryUI.<>f__mg$cache3C == null)
			{
				PlayerDashboardInventoryUI.<>f__mg$cache3C = new MaskUpdated(PlayerDashboardInventoryUI.onMaskUpdated);
			}
			clothing6.onMaskUpdated = (MaskUpdated)Delegate.Combine(onMaskUpdated, PlayerDashboardInventoryUI.<>f__mg$cache3C);
			PlayerClothing clothing7 = Player.player.clothing;
			Delegate onGlassesUpdated = clothing7.onGlassesUpdated;
			if (PlayerDashboardInventoryUI.<>f__mg$cache3D == null)
			{
				PlayerDashboardInventoryUI.<>f__mg$cache3D = new GlassesUpdated(PlayerDashboardInventoryUI.onGlassesUpdated);
			}
			clothing7.onGlassesUpdated = (GlassesUpdated)Delegate.Combine(onGlassesUpdated, PlayerDashboardInventoryUI.<>f__mg$cache3D);
			SleekWindow window = PlayerUI.window;
			Delegate onClickedMouse = window.onClickedMouse;
			if (PlayerDashboardInventoryUI.<>f__mg$cache3E == null)
			{
				PlayerDashboardInventoryUI.<>f__mg$cache3E = new ClickedMouse(PlayerDashboardInventoryUI.onClickedMouse);
			}
			window.onClickedMouse = (ClickedMouse)Delegate.Combine(onClickedMouse, PlayerDashboardInventoryUI.<>f__mg$cache3E);
			SleekWindow window2 = PlayerUI.window;
			Delegate onMovedMouse = window2.onMovedMouse;
			if (PlayerDashboardInventoryUI.<>f__mg$cache3F == null)
			{
				PlayerDashboardInventoryUI.<>f__mg$cache3F = new MovedMouse(PlayerDashboardInventoryUI.onMovedMouse);
			}
			window2.onMovedMouse = (MovedMouse)Delegate.Combine(onMovedMouse, PlayerDashboardInventoryUI.<>f__mg$cache3F);
		}

		public static byte selectedPage
		{
			get
			{
				return PlayerDashboardInventoryUI._selectedPage;
			}
		}

		public static byte selected_x
		{
			get
			{
				return PlayerDashboardInventoryUI._selected_x;
			}
		}

		public static byte selected_y
		{
			get
			{
				return PlayerDashboardInventoryUI._selected_y;
			}
		}

		public static ItemJar selectedJar
		{
			get
			{
				return PlayerDashboardInventoryUI._selectedJar;
			}
		}

		public static ItemAsset selectedAsset
		{
			get
			{
				return PlayerDashboardInventoryUI._selectedAsset;
			}
		}

		private static bool isSplitClothingArea
		{
			get
			{
				return Screen.width >= 1350;
			}
		}

		public static void open()
		{
			if (PlayerDashboardInventoryUI.active)
			{
				return;
			}
			PlayerDashboardInventoryUI.active = true;
			Player.player.animator.sendGesture(EPlayerGesture.INVENTORY_START, false);
			Player.player.character.FindChild("Camera").gameObject.SetActive(true);
			if (PlayerDashboardInventoryUI.isSplitClothingArea)
			{
				PlayerDashboardInventoryUI.clothingBox.sizeOffset_X = 95;
				PlayerDashboardInventoryUI.clothingBox.sizeScale_X = 0.5f;
				PlayerDashboardInventoryUI.areaBox.isVisible = true;
			}
			else
			{
				PlayerDashboardInventoryUI.clothingBox.sizeOffset_X = 100;
				PlayerDashboardInventoryUI.clothingBox.sizeScale_X = 1f;
				PlayerDashboardInventoryUI.areaBox.isVisible = false;
			}
			PlayerDashboardInventoryUI.updateVehicle();
			PlayerDashboardInventoryUI.updateNearbyDrops();
			PlayerDashboardInventoryUI.updateHotkeys();
			if (PlayerDashboardInventoryUI.characterPlayer != null)
			{
				PlayerDashboardInventoryUI.backdropBox.remove(PlayerDashboardInventoryUI.characterPlayer);
				PlayerDashboardInventoryUI.characterPlayer = null;
			}
			if (Player.player != null)
			{
				PlayerDashboardInventoryUI.characterPlayer = new SleekPlayer(Player.player.channel.owner, true, SleekPlayer.ESleekPlayerDisplayContext.NONE);
				PlayerDashboardInventoryUI.characterPlayer.positionOffset_X = 10;
				PlayerDashboardInventoryUI.characterPlayer.positionOffset_Y = 10;
				PlayerDashboardInventoryUI.characterPlayer.sizeOffset_X = 410;
				PlayerDashboardInventoryUI.characterPlayer.sizeOffset_Y = 50;
				PlayerDashboardInventoryUI.backdropBox.add(PlayerDashboardInventoryUI.characterPlayer);
			}
			PlayerDashboardInventoryUI.container.lerpPositionScale(0f, 0f, ESleekLerp.EXPONENTIAL, 20f);
		}

		public static void close()
		{
			if (!PlayerDashboardInventoryUI.active)
			{
				return;
			}
			PlayerDashboardInventoryUI.active = false;
			Player.player.animator.sendGesture(EPlayerGesture.INVENTORY_STOP, false);
			Player.player.character.FindChild("Camera").gameObject.SetActive(false);
			PlayerDashboardInventoryUI.stopDrag();
			PlayerDashboardInventoryUI.closeSelection();
			PlayerDashboardInventoryUI.container.lerpPositionScale(0f, 1f, ESleekLerp.EXPONENTIAL, 20f);
		}

		private static void startDrag()
		{
			if (PlayerDashboardInventoryUI.isDragging)
			{
				return;
			}
			PlayerDashboardInventoryUI.isDragging = true;
			PlayerDashboardInventoryUI.dragSource.disable();
			PlayerDashboardInventoryUI.dragItem.isVisible = true;
			SleekRender.allowInput = false;
		}

		private static void stopDrag()
		{
			if (!PlayerDashboardInventoryUI.isDragging)
			{
				return;
			}
			PlayerDashboardInventoryUI.isDragging = false;
			PlayerDashboardInventoryUI.dragJar.rot = PlayerDashboardInventoryUI.dragRot;
			PlayerDashboardInventoryUI.dragSource.enable();
			PlayerDashboardInventoryUI.dragItem.isVisible = false;
			SleekRender.allowInput = true;
		}

		private static void onDraggedCharacterSlider(SleekSlider slider, float state)
		{
			PlayerLook.characterYaw = state * 360f;
		}

		private static void onClickedSwapCosmeticsButton(SleekButton button)
		{
			Player.player.clothing.sendVisualToggle(EVisualToggleType.COSMETIC);
		}

		private static void onClickedSwapSkinsButton(SleekButton button)
		{
			Player.player.clothing.sendVisualToggle(EVisualToggleType.SKIN);
		}

		private static void onClickedSwapMythicsButton(SleekButton button)
		{
			Player.player.clothing.sendVisualToggle(EVisualToggleType.MYTHIC);
		}

		private static void onClickedVehicleLockButton(SleekButton button)
		{
			VehicleManager.sendVehicleLock();
		}

		private static void onClickedVehicleHornButton(SleekButton button)
		{
			VehicleManager.sendVehicleHorn();
		}

		private static void onClickedVehicleHeadlightsButton(SleekButton button)
		{
			VehicleManager.sendVehicleHeadlights();
		}

		private static void onClickedVehicleSirensButton(SleekButton button)
		{
			VehicleManager.sendVehicleBonus();
		}

		private static void onClickedVehicleHookButton(SleekButton button)
		{
			VehicleManager.sendVehicleBonus();
		}

		private static void onClickedVehicleStealBatteryButton(SleekButton button)
		{
			VehicleManager.sendVehicleStealBattery();
		}

		private static void onClickedVehiclePassengerButton(SleekButton button)
		{
			int num = PlayerDashboardInventoryUI.vehiclePassengersBox.search(button);
			if (num < 0)
			{
				return;
			}
			VehicleManager.swapVehicle((byte)num);
		}

		private static void onClickedEquip(SleekButton button)
		{
			if (PlayerDashboardInventoryUI.selectedPage != 255)
			{
				PlayerDashboardInventoryUI.checkEquip(PlayerDashboardInventoryUI.selectedPage, PlayerDashboardInventoryUI.selected_x, PlayerDashboardInventoryUI.selected_y, Player.player.inventory.getItem(PlayerDashboardInventoryUI.selectedPage, Player.player.inventory.getIndex(PlayerDashboardInventoryUI.selectedPage, PlayerDashboardInventoryUI.selected_x, PlayerDashboardInventoryUI.selected_y)), byte.MaxValue);
				Event.current.Use();
			}
		}

		private static void onClickedContext(SleekButton button)
		{
			if (PlayerDashboardInventoryUI.selectedPage != 255)
			{
				if (PlayerDashboardInventoryUI.selectedAsset.type == EItemType.GUN)
				{
					Player.player.crafting.sendStripAttachments(PlayerDashboardInventoryUI.selectedPage, PlayerDashboardInventoryUI.selected_x, PlayerDashboardInventoryUI.selected_y);
				}
				Event.current.Use();
				PlayerDashboardInventoryUI.closeSelection();
			}
		}

		private static void onClickedDrop(SleekButton button)
		{
			if (PlayerDashboardInventoryUI.selectedPage != 255)
			{
				if (PlayerDashboardInventoryUI.selectedPage == PlayerInventory.AREA)
				{
					ItemManager.takeItem(PlayerDashboardInventoryUI.selectedJar.interactableItem.transform.parent, byte.MaxValue, byte.MaxValue, 0, byte.MaxValue);
					PlayerDashboardInventoryUI.closeSelection();
				}
				else
				{
					Player.player.inventory.sendDropItem(PlayerDashboardInventoryUI.selectedPage, PlayerDashboardInventoryUI.selected_x, PlayerDashboardInventoryUI.selected_y);
				}
				Event.current.Use();
			}
		}

		private static void onClickedStore(SleekButton button)
		{
			if (PlayerDashboardInventoryUI.selectedPage != 255)
			{
				byte x_2;
				byte y_2;
				byte rot_2;
				if (PlayerDashboardInventoryUI.selectedPage == PlayerInventory.AREA)
				{
					ItemManager.takeItem(PlayerDashboardInventoryUI.selectedJar.interactableItem.transform.parent, byte.MaxValue, byte.MaxValue, 0, PlayerInventory.STORAGE);
					PlayerDashboardInventoryUI.closeSelection();
				}
				else if (PlayerDashboardInventoryUI.selectedPage == PlayerInventory.STORAGE)
				{
					byte page_;
					byte x_;
					byte y_;
					byte rot_;
					if (Player.player.inventory.tryFindSpace(PlayerDashboardInventoryUI.selectedJar.size_x, PlayerDashboardInventoryUI.selectedJar.size_y, out page_, out x_, out y_, out rot_))
					{
						Player.player.inventory.sendDragItem(PlayerDashboardInventoryUI.selectedPage, PlayerDashboardInventoryUI.selected_x, PlayerDashboardInventoryUI.selected_y, page_, x_, y_, rot_);
					}
				}
				else if (Player.player.inventory.tryFindSpace(PlayerInventory.STORAGE, PlayerDashboardInventoryUI.selectedJar.size_x, PlayerDashboardInventoryUI.selectedJar.size_y, out x_2, out y_2, out rot_2))
				{
					Player.player.inventory.sendDragItem(PlayerDashboardInventoryUI.selectedPage, PlayerDashboardInventoryUI.selected_x, PlayerDashboardInventoryUI.selected_y, PlayerInventory.STORAGE, x_2, y_2, rot_2);
				}
				Event.current.Use();
			}
		}

		private static void onClickedAction(SleekButton button)
		{
			int index = PlayerDashboardInventoryUI.selectionExtraActionsBox.search(button);
			Action action = PlayerDashboardInventoryUI.actions[index];
			ItemAsset itemAsset = (ItemAsset)Assets.find(EAssetType.ITEM, action.source);
			if (itemAsset == null)
			{
				return;
			}
			Blueprint[] array = new Blueprint[action.blueprints.Length];
			bool flag = false;
			byte b = 0;
			while ((int)b < array.Length)
			{
				array[(int)b] = itemAsset.blueprints[(int)action.blueprints[(int)b].id];
				if (action.blueprints[(int)b].isLink)
				{
					flag = true;
				}
				b += 1;
			}
			PlayerDashboardCraftingUI.viewBlueprints = array;
			if (!flag)
			{
				PlayerDashboardCraftingUI.updateSelection();
				foreach (Blueprint blueprint in array)
				{
					if (!blueprint.hasSupplies)
					{
						flag = true;
						break;
					}
					if (!blueprint.hasTool)
					{
						flag = true;
						break;
					}
					if (!blueprint.hasItem)
					{
						flag = true;
						break;
					}
					if (!blueprint.hasSkills)
					{
						flag = true;
						break;
					}
					if (Player.player.equipment.isBusy)
					{
						flag = true;
						break;
					}
				}
			}
			if (flag)
			{
				PlayerDashboardInventoryUI.close();
				PlayerDashboardCraftingUI.open();
			}
			else
			{
				foreach (Blueprint blueprint2 in array)
				{
					Player.player.crafting.sendCraft(blueprint2.source, blueprint2.id, Input.GetKey(ControlsSettings.other));
				}
				PlayerDashboardCraftingUI.viewBlueprints = null;
				PlayerDashboardInventoryUI.closeSelection();
			}
		}

		private static void onClickedRot_XButton(SleekButton button)
		{
			InteractableStorage interactableStorage = PlayerInteract.interactable as InteractableStorage;
			if (interactableStorage == null || !interactableStorage.isDisplay)
			{
				return;
			}
			byte b = interactableStorage.rot_x;
			b += 1;
			if (b > 3)
			{
				b = 0;
			}
			byte rotation = interactableStorage.getRotation(b, interactableStorage.rot_y, interactableStorage.rot_z);
			BarricadeManager.rotDisplay(interactableStorage.transform, rotation);
		}

		private static void onClickedRot_YButton(SleekButton button)
		{
			InteractableStorage interactableStorage = PlayerInteract.interactable as InteractableStorage;
			if (interactableStorage == null || !interactableStorage.isDisplay)
			{
				return;
			}
			byte b = interactableStorage.rot_y;
			b += 1;
			if (b > 3)
			{
				b = 0;
			}
			byte rotation = interactableStorage.getRotation(interactableStorage.rot_x, b, interactableStorage.rot_z);
			BarricadeManager.rotDisplay(interactableStorage.transform, rotation);
		}

		private static void onClickedRot_ZButton(SleekButton button)
		{
			InteractableStorage interactableStorage = PlayerInteract.interactable as InteractableStorage;
			if (interactableStorage == null || !interactableStorage.isDisplay)
			{
				return;
			}
			InteractableStorage interactableStorage2 = interactableStorage;
			byte rot_z;
			interactableStorage2.rot_z = (rot_z = interactableStorage2.rot_z) + 1;
			byte b = rot_z;
			b += 1;
			if (b > 3)
			{
				b = 0;
			}
			byte rotation = interactableStorage.getRotation(interactableStorage.rot_x, interactableStorage.rot_y, b);
			BarricadeManager.rotDisplay(interactableStorage.transform, rotation);
		}

		private static void onSelectionIconReady(Texture2D texture)
		{
			PlayerDashboardInventoryUI.selectionIconImage.positionOffset_X = -texture.width / 2;
			PlayerDashboardInventoryUI.selectionIconImage.positionOffset_Y = -texture.height / 2;
			PlayerDashboardInventoryUI.selectionIconImage.sizeOffset_X = texture.width;
			PlayerDashboardInventoryUI.selectionIconImage.sizeOffset_Y = texture.height;
			PlayerDashboardInventoryUI.selectionIconImage.texture = texture;
			PlayerDashboardInventoryUI.selectionIconImage.isVisible = true;
		}

		private static void openSelection(byte page, byte x, byte y)
		{
			PlayerDashboardInventoryUI._selectedPage = page;
			PlayerDashboardInventoryUI._selected_x = x;
			PlayerDashboardInventoryUI._selected_y = y;
			PlayerDashboardInventoryUI.clothingBox.isInputable = false;
			PlayerDashboardInventoryUI.areaBox.isInputable = false;
			for (int i = 0; i < PlayerDashboardInventoryUI.slots.Length; i++)
			{
				PlayerDashboardInventoryUI.slots[i].isInputable = false;
			}
			PlayerDashboardInventoryUI.selectionBackdropBox.isVisible = true;
			PlayerDashboardInventoryUI._selectedJar = Player.player.inventory.getItem(page, Player.player.inventory.getIndex(page, x, y));
			if (PlayerDashboardInventoryUI.selectedJar == null)
			{
				return;
			}
			PlayerDashboardInventoryUI._selectedAsset = (ItemAsset)Assets.find(EAssetType.ITEM, PlayerDashboardInventoryUI.selectedJar.item.id);
			if (PlayerDashboardInventoryUI.selectedAsset != null)
			{
				if (PlayerDashboardInventoryUI.selectedAsset.size_x <= PlayerDashboardInventoryUI.selectedAsset.size_y)
				{
					PlayerDashboardInventoryUI.selectionBackdropBox.sizeOffset_X = 490;
					PlayerDashboardInventoryUI.selectionBackdropBox.sizeOffset_Y = 330;
					PlayerDashboardInventoryUI.selectionIconBox.sizeOffset_X = 210;
					PlayerDashboardInventoryUI.selectionIconBox.sizeOffset_Y = 310;
					PlayerDashboardInventoryUI.selectionDescriptionBox.positionOffset_X = 230;
					PlayerDashboardInventoryUI.selectionDescriptionBox.positionOffset_Y = 10;
					PlayerDashboardInventoryUI.selectionDescriptionBox.sizeOffset_X = 250;
					PlayerDashboardInventoryUI.selectionDescriptionBox.sizeOffset_Y = 150;
					PlayerDashboardInventoryUI.selectionActionsBox.positionOffset_X = 230;
					PlayerDashboardInventoryUI.selectionActionsBox.positionOffset_Y = 170;
					PlayerDashboardInventoryUI.selectionActionsBox.sizeOffset_X = 280;
					PlayerDashboardInventoryUI.selectionActionsBox.sizeOffset_Y = 150;
					PlayerDashboardInventoryUI.selectionBackdropBox.positionOffset_X = (int)Mathf.Min(Mathf.Max(PlayerUI.window.mouse_x - 10f - 225f, 0f), PlayerDashboardInventoryUI.container.frame.width - (float)PlayerDashboardInventoryUI.selectionBackdropBox.sizeOffset_X);
					PlayerDashboardInventoryUI.selectionBackdropBox.positionOffset_Y = (int)Mathf.Min(Mathf.Max(PlayerUI.window.mouse_y - 10f - 165f, 60f), PlayerDashboardInventoryUI.container.frame.height - (float)PlayerDashboardInventoryUI.selectionBackdropBox.sizeOffset_Y);
					if (PlayerDashboardInventoryUI.selectedAsset.size_x == PlayerDashboardInventoryUI.selectedAsset.size_y)
					{
						ushort id = PlayerDashboardInventoryUI.selectedJar.item.id;
						byte quality = PlayerDashboardInventoryUI.selectedJar.item.quality;
						byte[] state = PlayerDashboardInventoryUI.selectedJar.item.state;
						ItemAsset selectedAsset = PlayerDashboardInventoryUI.selectedAsset;
						int x2 = 200;
						int y2 = 200;
						if (PlayerDashboardInventoryUI.<>f__mg$cache0 == null)
						{
							PlayerDashboardInventoryUI.<>f__mg$cache0 = new ItemIconReady(PlayerDashboardInventoryUI.onSelectionIconReady);
						}
						ItemTool.getIcon(id, quality, state, selectedAsset, x2, y2, PlayerDashboardInventoryUI.<>f__mg$cache0);
					}
					else
					{
						ushort id2 = PlayerDashboardInventoryUI.selectedJar.item.id;
						byte quality2 = PlayerDashboardInventoryUI.selectedJar.item.quality;
						byte[] state2 = PlayerDashboardInventoryUI.selectedJar.item.state;
						ItemAsset selectedAsset2 = PlayerDashboardInventoryUI.selectedAsset;
						int x3 = 200;
						int y3 = 300;
						if (PlayerDashboardInventoryUI.<>f__mg$cache1 == null)
						{
							PlayerDashboardInventoryUI.<>f__mg$cache1 = new ItemIconReady(PlayerDashboardInventoryUI.onSelectionIconReady);
						}
						ItemTool.getIcon(id2, quality2, state2, selectedAsset2, x3, y3, PlayerDashboardInventoryUI.<>f__mg$cache1);
					}
				}
				else
				{
					PlayerDashboardInventoryUI.selectionBackdropBox.sizeOffset_X = 530;
					PlayerDashboardInventoryUI.selectionBackdropBox.sizeOffset_Y = 390;
					PlayerDashboardInventoryUI.selectionIconBox.sizeOffset_X = 510;
					PlayerDashboardInventoryUI.selectionIconBox.sizeOffset_Y = 210;
					PlayerDashboardInventoryUI.selectionDescriptionBox.positionOffset_X = 10;
					PlayerDashboardInventoryUI.selectionDescriptionBox.positionOffset_Y = 230;
					PlayerDashboardInventoryUI.selectionDescriptionBox.sizeOffset_X = 250;
					PlayerDashboardInventoryUI.selectionDescriptionBox.sizeOffset_Y = 150;
					PlayerDashboardInventoryUI.selectionActionsBox.positionOffset_X = 270;
					PlayerDashboardInventoryUI.selectionActionsBox.positionOffset_Y = 230;
					PlayerDashboardInventoryUI.selectionActionsBox.sizeOffset_X = 280;
					PlayerDashboardInventoryUI.selectionActionsBox.sizeOffset_Y = 150;
					PlayerDashboardInventoryUI.selectionBackdropBox.positionOffset_X = (int)Mathf.Min(Mathf.Max(PlayerUI.window.mouse_x - 10f - 265f, 0f), PlayerDashboardInventoryUI.container.frame.width - (float)PlayerDashboardInventoryUI.selectionBackdropBox.sizeOffset_X);
					PlayerDashboardInventoryUI.selectionBackdropBox.positionOffset_Y = (int)Mathf.Min(Mathf.Max(PlayerUI.window.mouse_y - 10f - 225f, 60f), PlayerDashboardInventoryUI.container.frame.height - (float)PlayerDashboardInventoryUI.selectionBackdropBox.sizeOffset_Y);
					ushort id3 = PlayerDashboardInventoryUI.selectedJar.item.id;
					byte quality3 = PlayerDashboardInventoryUI.selectedJar.item.quality;
					byte[] state3 = PlayerDashboardInventoryUI.selectedJar.item.state;
					ItemAsset selectedAsset3 = PlayerDashboardInventoryUI.selectedAsset;
					int x4 = 500;
					int y4 = 200;
					if (PlayerDashboardInventoryUI.<>f__mg$cache2 == null)
					{
						PlayerDashboardInventoryUI.<>f__mg$cache2 = new ItemIconReady(PlayerDashboardInventoryUI.onSelectionIconReady);
					}
					ItemTool.getIcon(id3, quality3, state3, selectedAsset3, x4, y4, PlayerDashboardInventoryUI.<>f__mg$cache2);
				}
				PlayerDashboardInventoryUI.selectionIconImage.isVisible = false;
				PlayerDashboardInventoryUI.selectionDescriptionLabel.text = string.Concat(new string[]
				{
					"<color=",
					Palette.hex(ItemTool.getRarityColorUI(PlayerDashboardInventoryUI.selectedAsset.rarity)),
					">",
					PlayerDashboardInventoryUI.localization.format("Rarity_" + (int)PlayerDashboardInventoryUI.selectedAsset.rarity),
					" ",
					PlayerDashboardInventoryUI.localization.format("Type_" + (int)PlayerDashboardInventoryUI.selectedAsset.type),
					"</color>\n\n"
				});
				if (PlayerDashboardInventoryUI.selectedAsset.showQuality)
				{
					Color32 color = ItemTool.getQualityColor((float)PlayerDashboardInventoryUI.selectedJar.item.quality / 100f);
					SleekLabel sleekLabel = PlayerDashboardInventoryUI.selectionDescriptionLabel;
					string text = sleekLabel.text;
					sleekLabel.text = string.Concat(new string[]
					{
						text,
						"<color=",
						Palette.hex(color),
						">",
						PlayerDashboardInventoryUI.localization.format("Quality", new object[]
						{
							PlayerDashboardInventoryUI.selectedJar.item.quality
						}),
						"</color>\n\n"
					});
				}
				if (PlayerDashboardInventoryUI.selectedAsset.amount > 1)
				{
					SleekLabel sleekLabel2 = PlayerDashboardInventoryUI.selectionDescriptionLabel;
					sleekLabel2.text = sleekLabel2.text + PlayerDashboardInventoryUI.localization.format("Amount", new object[]
					{
						PlayerDashboardInventoryUI.selectedJar.item.amount
					}) + "\n\n";
				}
				PlayerDashboardInventoryUI.selectionDescriptionLabel.text = PlayerDashboardInventoryUI.selectedAsset.getContext(PlayerDashboardInventoryUI.selectionDescriptionLabel.text, PlayerDashboardInventoryUI.selectedJar.item.state);
				SleekLabel sleekLabel3 = PlayerDashboardInventoryUI.selectionDescriptionLabel;
				sleekLabel3.text += PlayerDashboardInventoryUI.selectedAsset.itemDescription;
				PlayerDashboardInventoryUI.selectionNameLabel.text = PlayerDashboardInventoryUI.selectedAsset.itemName;
				if (PlayerDashboardInventoryUI.selectedPage < PlayerInventory.SLOTS)
				{
					PlayerDashboardInventoryUI.selectionHotkeyLabel.text = PlayerDashboardInventoryUI.localization.format("Hotkey_Set", new object[]
					{
						(int)(PlayerDashboardInventoryUI.selectedPage + 1)
					});
					PlayerDashboardInventoryUI.selectionHotkeyLabel.isVisible = true;
				}
				else if (PlayerDashboardInventoryUI.selectedPage < PlayerInventory.STORAGE)
				{
					PlayerDashboardInventoryUI.selectionHotkeyLabel.text = PlayerDashboardInventoryUI.localization.format("Hotkey_Unset");
					PlayerDashboardInventoryUI.selectionHotkeyLabel.isVisible = true;
					byte b = 0;
					while ((int)b < Player.player.equipment.hotkeys.Length)
					{
						HotkeyInfo hotkeyInfo = Player.player.equipment.hotkeys[(int)b];
						if (hotkeyInfo.page == PlayerDashboardInventoryUI.selectedPage && hotkeyInfo.x == PlayerDashboardInventoryUI.selected_x && hotkeyInfo.y == PlayerDashboardInventoryUI.selected_y)
						{
							PlayerDashboardInventoryUI.selectionHotkeyLabel.text = PlayerDashboardInventoryUI.localization.format("Hotkey_Set", new object[]
							{
								(int)(b + 3)
							});
							break;
						}
						b += 1;
					}
				}
				else
				{
					PlayerDashboardInventoryUI.selectionHotkeyLabel.isVisible = false;
				}
				if (Player.player.equipment.checkSelection(page, x, y))
				{
					PlayerDashboardInventoryUI.selectionEquipButton.text = PlayerDashboardInventoryUI.localization.format("Dequip_Button");
					PlayerDashboardInventoryUI.selectionEquipButton.tooltip = PlayerDashboardInventoryUI.localization.format("Dequip_Button_Tooltip");
				}
				else
				{
					PlayerDashboardInventoryUI.selectionEquipButton.text = PlayerDashboardInventoryUI.localization.format("Equip_Button");
					PlayerDashboardInventoryUI.selectionEquipButton.tooltip = PlayerDashboardInventoryUI.localization.format("Equip_Button_Tooltip");
				}
				if (PlayerDashboardInventoryUI.selectedAsset.type == EItemType.GUN)
				{
					PlayerDashboardInventoryUI.selectionContextButton.text = PlayerDashboardInventoryUI.localization.format("Attachments_Button");
					PlayerDashboardInventoryUI.selectionContextButton.tooltip = PlayerDashboardInventoryUI.localization.format("Attachments_Button_Tooltip");
					PlayerDashboardInventoryUI.selectionContextButton.isVisible = (PlayerDashboardInventoryUI.selectedPage >= PlayerInventory.SLOTS && PlayerDashboardInventoryUI.selectedPage < PlayerInventory.AREA);
				}
				else
				{
					PlayerDashboardInventoryUI.selectionContextButton.isVisible = false;
				}
				if (page == PlayerInventory.AREA)
				{
					PlayerDashboardInventoryUI.selectionDropButton.text = PlayerDashboardInventoryUI.localization.format("Pickup_Button");
					PlayerDashboardInventoryUI.selectionDropButton.tooltip = PlayerDashboardInventoryUI.localization.format("Pickup_Button_Tooltip");
				}
				else
				{
					PlayerDashboardInventoryUI.selectionDropButton.text = PlayerDashboardInventoryUI.localization.format("Drop_Button");
					PlayerDashboardInventoryUI.selectionDropButton.tooltip = PlayerDashboardInventoryUI.localization.format("Drop_Button_Tooltip");
				}
				if (page == PlayerInventory.STORAGE)
				{
					PlayerDashboardInventoryUI.selectionStorageButton.text = PlayerDashboardInventoryUI.localization.format("Take_Button");
					PlayerDashboardInventoryUI.selectionStorageButton.tooltip = PlayerDashboardInventoryUI.localization.format("Take_Button_Tooltip");
				}
				else
				{
					PlayerDashboardInventoryUI.selectionStorageButton.text = PlayerDashboardInventoryUI.localization.format("Store_Button");
					PlayerDashboardInventoryUI.selectionStorageButton.tooltip = PlayerDashboardInventoryUI.localization.format("Store_Button_Tooltip");
				}
				PlayerDashboardInventoryUI.selectionEquipButton.isVisible = (PlayerDashboardInventoryUI.selectedAsset.isUseable && page < PlayerInventory.PAGES - 2);
				PlayerDashboardInventoryUI.selectionDropButton.isVisible = true;
				PlayerDashboardInventoryUI.selectionStorageButton.isVisible = Player.player.inventory.isStoring;
				int num = 0;
				if (PlayerDashboardInventoryUI.selectionEquipButton.isVisible)
				{
					PlayerDashboardInventoryUI.selectionEquipButton.positionOffset_Y = num;
					num += 40;
				}
				if (PlayerDashboardInventoryUI.selectionContextButton.isVisible)
				{
					PlayerDashboardInventoryUI.selectionContextButton.positionOffset_Y = num;
					num += 40;
				}
				if (PlayerDashboardInventoryUI.selectionDropButton.isVisible)
				{
					PlayerDashboardInventoryUI.selectionDropButton.positionOffset_Y = num;
					num += 40;
				}
				if (PlayerDashboardInventoryUI.selectionStorageButton.isVisible)
				{
					PlayerDashboardInventoryUI.selectionStorageButton.positionOffset_Y = num;
					num += 40;
				}
				PlayerDashboardInventoryUI.selectionExtraActionsBox.remove();
				PlayerDashboardInventoryUI.selectionExtraActionsBox.positionOffset_Y = num;
				int num2 = 0;
				if (page != PlayerInventory.AREA)
				{
					PlayerDashboardInventoryUI.actions.Clear();
					int j = 0;
					while (j < PlayerDashboardInventoryUI.selectedAsset.actions.Count)
					{
						Action action = PlayerDashboardInventoryUI.selectedAsset.actions[j];
						if (action.type != EActionType.BLUEPRINT)
						{
							goto IL_B86;
						}
						if (page >= PlayerInventory.SLOTS)
						{
							if (page < PlayerInventory.STORAGE)
							{
								ItemAsset itemAsset = (ItemAsset)Assets.find(EAssetType.ITEM, action.source);
								Blueprint blueprint = itemAsset.blueprints[(int)action.blueprints[0].id];
								if (blueprint.skill != EBlueprintSkill.REPAIR || (uint)blueprint.level <= Provider.modeConfigData.Gameplay.Repair_Level_Max)
								{
									if (blueprint.type != EBlueprintType.REPAIR || PlayerDashboardInventoryUI.selectedJar.item.quality != 100)
									{
										goto IL_B86;
									}
								}
							}
						}
						IL_C75:
						j++;
						continue;
						IL_B86:
						PlayerDashboardInventoryUI.actions.Add(action);
						SleekButton sleekButton = new SleekButton();
						sleekButton.positionOffset_Y = num2;
						sleekButton.sizeOffset_X = -30;
						sleekButton.sizeScale_X = 1f;
						sleekButton.sizeOffset_Y = 30;
						if (action.key.Length > 0)
						{
							sleekButton.text = PlayerDashboardInventoryUI.localization.format(action.key + "_Button");
							sleekButton.tooltip = PlayerDashboardInventoryUI.localization.format(action.key + "_Button_Tooltip");
						}
						else
						{
							sleekButton.text = action.text;
							sleekButton.tooltip = action.tooltip;
						}
						SleekButton sleekButton2 = sleekButton;
						if (PlayerDashboardInventoryUI.<>f__mg$cache3 == null)
						{
							PlayerDashboardInventoryUI.<>f__mg$cache3 = new ClickedButton(PlayerDashboardInventoryUI.onClickedAction);
						}
						sleekButton2.onClickedButton = PlayerDashboardInventoryUI.<>f__mg$cache3;
						PlayerDashboardInventoryUI.selectionExtraActionsBox.add(sleekButton);
						num2 += 40;
						num += 40;
						goto IL_C75;
					}
				}
				PlayerDashboardInventoryUI.selectionExtraActionsBox.sizeOffset_Y = num2 - 10;
				PlayerDashboardInventoryUI.selectionActionsBox.area = new Rect(0f, 0f, 5f, (float)(num - 10));
				PlayerDashboardInventoryUI.selectionDescriptionBox.backgroundColor = ItemTool.getRarityColorUI(PlayerDashboardInventoryUI.selectedAsset.rarity);
				PlayerDashboardInventoryUI.selectionDescriptionBox.foregroundColor = PlayerDashboardInventoryUI.selectionDescriptionBox.backgroundColor;
				PlayerDashboardInventoryUI.selectionNameLabel.backgroundColor = PlayerDashboardInventoryUI.selectionDescriptionBox.backgroundColor;
				PlayerDashboardInventoryUI.selectionNameLabel.foregroundColor = PlayerDashboardInventoryUI.selectionDescriptionBox.backgroundColor;
			}
		}

		public static void closeSelection()
		{
			if (PlayerDashboardInventoryUI.selectedPage == 255)
			{
				return;
			}
			PlayerDashboardInventoryUI._selectedPage = byte.MaxValue;
			PlayerDashboardInventoryUI._selected_x = byte.MaxValue;
			PlayerDashboardInventoryUI._selected_y = byte.MaxValue;
			PlayerDashboardInventoryUI.clothingBox.isInputable = true;
			PlayerDashboardInventoryUI.areaBox.isInputable = true;
			for (int i = 0; i < PlayerDashboardInventoryUI.slots.Length; i++)
			{
				PlayerDashboardInventoryUI.slots[i].isInputable = true;
			}
			PlayerDashboardInventoryUI.selectionBackdropBox.isVisible = false;
		}

		private static void onSelectedItem(byte page, byte x, byte y)
		{
			if (Time.realtimeSinceStartup - PlayerDashboardInventoryUI.lastDrag > 0.5f && !PlayerDashboardInventoryUI.isDragging)
			{
				if (page == 255 || (page == PlayerDashboardInventoryUI.selectedPage && x == PlayerDashboardInventoryUI.selected_x && y == PlayerDashboardInventoryUI.selected_y))
				{
					PlayerDashboardInventoryUI.closeSelection();
				}
				else if (Input.GetKey(ControlsSettings.other))
				{
					ItemJar item = Player.player.inventory.getItem(page, Player.player.inventory.getIndex(page, x, y));
					if (Player.player.inventory.isStoring)
					{
						byte x_2;
						byte y_2;
						byte rot_2;
						if (page == PlayerInventory.AREA)
						{
							ItemManager.takeItem(item.interactableItem.transform.parent, byte.MaxValue, byte.MaxValue, 0, PlayerInventory.STORAGE);
						}
						else if (page == PlayerInventory.STORAGE)
						{
							byte page_;
							byte x_;
							byte y_;
							byte rot_;
							if (Player.player.inventory.tryFindSpace(item.size_x, item.size_y, out page_, out x_, out y_, out rot_))
							{
								Player.player.inventory.sendDragItem(page, x, y, page_, x_, y_, rot_);
							}
						}
						else if (Player.player.inventory.tryFindSpace(PlayerInventory.STORAGE, item.size_x, item.size_y, out x_2, out y_2, out rot_2))
						{
							Player.player.inventory.sendDragItem(page, x, y, PlayerInventory.STORAGE, x_2, y_2, rot_2);
						}
					}
					else
					{
						PlayerDashboardInventoryUI.checkAction(page, x, y, item);
					}
				}
				else
				{
					PlayerDashboardInventoryUI.openSelection(page, x, y);
				}
			}
		}

		private static bool checkSlot(byte page, byte x, byte y, ItemJar jar, byte slot)
		{
			if (Player.player.inventory.checkSpaceEmpty(slot, 255, 255, 0, 0, 0))
			{
				Player.player.inventory.sendDragItem(page, x, y, slot, 0, 0, 0);
				Player.player.equipment.equip(slot, 0, 0, jar.item.id);
				PlayerDashboardUI.close();
				PlayerLifeUI.open();
				return true;
			}
			ItemJar item = Player.player.inventory.getItem(slot, 0);
			if (Player.player.inventory.checkSpaceSwap(page, x, y, jar.size_x, jar.size_y, jar.rot, item.size_x, item.size_y, jar.rot))
			{
				Player.player.inventory.sendSwapItem(page, x, y, slot, 0, 0);
				Player.player.equipment.equip(slot, 0, 0, jar.item.id);
				PlayerDashboardUI.close();
				PlayerLifeUI.open();
				return true;
			}
			return false;
		}

		private static void checkEquip(byte page, byte x, byte y, ItemJar jar, byte slot)
		{
			if (page == PlayerInventory.AREA)
			{
				if (page == PlayerDashboardInventoryUI.selectedPage && x == PlayerDashboardInventoryUI.selected_x && y == PlayerDashboardInventoryUI.selected_y)
				{
					PlayerDashboardInventoryUI.closeSelection();
				}
				ItemManager.takeItem(jar.interactableItem.transform.parent, byte.MaxValue, byte.MaxValue, 0, byte.MaxValue);
				return;
			}
			if (!Player.player.equipment.checkSelection(page, x, y))
			{
				ItemAsset itemAsset = (ItemAsset)Assets.find(EAssetType.ITEM, jar.item.id);
				if (itemAsset != null)
				{
					if (itemAsset.slot == ESlotType.NONE || page < PlayerInventory.SLOTS)
					{
						Player.player.equipment.equip(page, x, y, jar.item.id);
						PlayerDashboardUI.close();
						PlayerLifeUI.open();
					}
					else if (itemAsset.slot == ESlotType.PRIMARY)
					{
						if (!PlayerDashboardInventoryUI.checkSlot(page, x, y, jar, 0))
						{
							return;
						}
					}
					else if (itemAsset.slot == ESlotType.SECONDARY)
					{
						if (slot == 255)
						{
							if (!PlayerDashboardInventoryUI.checkSlot(page, x, y, jar, 1) && !PlayerDashboardInventoryUI.checkSlot(page, x, y, jar, 0))
							{
								return;
							}
						}
						else if (!PlayerDashboardInventoryUI.checkSlot(page, x, y, jar, slot))
						{
							return;
						}
					}
				}
			}
			else if (Player.player.equipment.isSelected && !Player.player.equipment.isBusy && Player.player.equipment.isEquipped)
			{
				Player.player.equipment.dequip();
				if (page == PlayerDashboardInventoryUI.selectedPage && x == PlayerDashboardInventoryUI.selected_x && y == PlayerDashboardInventoryUI.selected_y)
				{
					PlayerDashboardInventoryUI.closeSelection();
				}
			}
		}

		private static void checkAction(byte page, byte x, byte y, ItemJar jar)
		{
			if (page == PlayerInventory.AREA)
			{
				ItemManager.takeItem(jar.interactableItem.transform.parent, byte.MaxValue, byte.MaxValue, 0, byte.MaxValue);
				return;
			}
			ItemAsset itemAsset = (ItemAsset)Assets.find(EAssetType.ITEM, jar.item.id);
			if (itemAsset != null)
			{
				if (itemAsset.type == EItemType.HAT)
				{
					Player.player.clothing.sendSwapHat(page, x, y);
				}
				else if (itemAsset.type == EItemType.SHIRT)
				{
					Player.player.clothing.sendSwapShirt(page, x, y);
				}
				else if (itemAsset.type == EItemType.PANTS)
				{
					Player.player.clothing.sendSwapPants(page, x, y);
				}
				else if (itemAsset.type == EItemType.BACKPACK)
				{
					Player.player.clothing.sendSwapBackpack(page, x, y);
				}
				else if (itemAsset.type == EItemType.VEST)
				{
					Player.player.clothing.sendSwapVest(page, x, y);
				}
				else if (itemAsset.type == EItemType.MASK)
				{
					Player.player.clothing.sendSwapMask(page, x, y);
				}
				else if (itemAsset.type == EItemType.GLASSES)
				{
					Player.player.clothing.sendSwapGlasses(page, x, y);
				}
				else if (itemAsset.isUseable)
				{
					PlayerDashboardInventoryUI.checkEquip(page, x, y, jar, byte.MaxValue);
				}
			}
		}

		private static void onGrabbedItem(byte page, byte x, byte y, SleekItem item)
		{
			if (Time.realtimeSinceStartup - PlayerDashboardInventoryUI.lastDrag > 0.5f && !PlayerDashboardInventoryUI.isDragging)
			{
				if (Input.GetKey(ControlsSettings.other))
				{
					if (page == PlayerInventory.AREA)
					{
						ItemManager.takeItem(item.jar.interactableItem.transform.parent, byte.MaxValue, byte.MaxValue, 0, byte.MaxValue);
					}
					else
					{
						Player.player.inventory.sendDropItem(page, x, y);
					}
				}
				else
				{
					PlayerDashboardInventoryUI.dragJar = Player.player.inventory.getItem(page, Player.player.inventory.getIndex(page, x, y));
					PlayerDashboardInventoryUI.dragSource = item;
					PlayerDashboardInventoryUI.dragPage = page;
					PlayerDashboardInventoryUI.drag_x = x;
					PlayerDashboardInventoryUI.drag_y = y;
					PlayerDashboardInventoryUI.dragRot = PlayerDashboardInventoryUI.dragJar.rot;
					if (page < PlayerInventory.SLOTS)
					{
						PlayerDashboardInventoryUI.dragOffset.x = item.frame.x - (PlayerUI.window.mouse_x + PlayerUI.window.frame.x);
						PlayerDashboardInventoryUI.dragOffset.y = item.frame.y - (PlayerUI.window.mouse_y + PlayerUI.window.frame.y);
					}
					else
					{
						PlayerDashboardInventoryUI.dragOffset.x = item.frame.x + item.parent.parent.frame.x - ((SleekScrollBox)item.parent.parent).state.x - (PlayerUI.window.mouse_x + PlayerUI.window.frame.x);
						PlayerDashboardInventoryUI.dragOffset.y = item.frame.y + item.parent.parent.frame.y - ((SleekScrollBox)item.parent.parent).state.y - (PlayerUI.window.mouse_y + PlayerUI.window.frame.y);
					}
					if (PlayerDashboardInventoryUI.dragJar.rot == 1)
					{
						float x2 = PlayerDashboardInventoryUI.dragOffset.x;
						PlayerDashboardInventoryUI.dragOffset.x = PlayerDashboardInventoryUI.dragOffset.y;
						PlayerDashboardInventoryUI.dragOffset.y = -((float)(PlayerDashboardInventoryUI.dragJar.size_y * 50) + x2);
					}
					else if (PlayerDashboardInventoryUI.dragJar.rot == 2)
					{
						PlayerDashboardInventoryUI.dragOffset.x = -((float)(PlayerDashboardInventoryUI.dragJar.size_x * 50) + PlayerDashboardInventoryUI.dragOffset.x);
						PlayerDashboardInventoryUI.dragOffset.y = -((float)(PlayerDashboardInventoryUI.dragJar.size_y * 50) + PlayerDashboardInventoryUI.dragOffset.y);
					}
					else if (PlayerDashboardInventoryUI.dragJar.rot == 3)
					{
						float x3 = PlayerDashboardInventoryUI.dragOffset.x;
						PlayerDashboardInventoryUI.dragOffset.x = -((float)(PlayerDashboardInventoryUI.dragJar.size_x * 50) + PlayerDashboardInventoryUI.dragOffset.y);
						PlayerDashboardInventoryUI.dragOffset.y = x3;
					}
					PlayerDashboardInventoryUI.updatePivot();
					PlayerDashboardInventoryUI.dragItem.updateItem(PlayerDashboardInventoryUI.dragJar);
					PlayerDashboardInventoryUI.dragItem.positionOffset_X = (int)(PlayerUI.window.mouse_x + PlayerDashboardInventoryUI.dragPivot.x);
					PlayerDashboardInventoryUI.dragItem.positionOffset_Y = (int)(PlayerUI.window.mouse_y + PlayerDashboardInventoryUI.dragPivot.y);
					PlayerDashboardInventoryUI.startDrag();
				}
			}
		}

		private static void onPlacedItem(byte page, byte x, byte y)
		{
			if (PlayerDashboardInventoryUI.dragSource != null && PlayerDashboardInventoryUI.isDragging)
			{
				PlayerDashboardInventoryUI.lastDrag = Time.realtimeSinceStartup;
				if (page >= PlayerInventory.SLOTS)
				{
					int num = (int)x + (int)(PlayerDashboardInventoryUI.dragPivot.x / 50f);
					int num2 = (int)y + (int)(PlayerDashboardInventoryUI.dragPivot.y / 50f);
					if (num < 0)
					{
						num = 0;
					}
					if (num2 < 0)
					{
						num2 = 0;
					}
					byte b = PlayerDashboardInventoryUI.dragJar.size_x;
					byte b2 = PlayerDashboardInventoryUI.dragJar.size_y;
					if (PlayerDashboardInventoryUI.dragJar.rot % 2 == 1)
					{
						b = PlayerDashboardInventoryUI.dragJar.size_y;
						b2 = PlayerDashboardInventoryUI.dragJar.size_x;
					}
					if (num >= (int)(Player.player.inventory.getWidth(page) - b))
					{
						num = (int)(Player.player.inventory.getWidth(page) - b);
					}
					if (num2 >= (int)(Player.player.inventory.getHeight(page) - b2))
					{
						num2 = (int)(Player.player.inventory.getHeight(page) - b2);
					}
					x = (byte)num;
					y = (byte)num2;
				}
				ItemAsset itemAsset = (ItemAsset)Assets.find(EAssetType.ITEM, PlayerDashboardInventoryUI.dragJar.item.id);
				if (page < PlayerInventory.SLOTS)
				{
					if (itemAsset != null && itemAsset.slot == ESlotType.NONE)
					{
						return;
					}
					if (itemAsset != null && page == 1 && itemAsset.slot != ESlotType.SECONDARY)
					{
						return;
					}
				}
				if (PlayerDashboardInventoryUI.dragPage == page && PlayerDashboardInventoryUI.drag_x == x && PlayerDashboardInventoryUI.drag_y == y && PlayerDashboardInventoryUI.dragRot == PlayerDashboardInventoryUI.dragJar.rot)
				{
					PlayerDashboardInventoryUI.stopDrag();
					return;
				}
				if (page == PlayerInventory.AREA)
				{
					PlayerDashboardInventoryUI.stopDrag();
					if (page != PlayerDashboardInventoryUI.dragPage)
					{
						Player.player.inventory.sendDropItem(PlayerDashboardInventoryUI.dragPage, PlayerDashboardInventoryUI.drag_x, PlayerDashboardInventoryUI.drag_y);
					}
					return;
				}
				if (PlayerDashboardInventoryUI.dragPage == PlayerInventory.AREA)
				{
					byte rot = PlayerDashboardInventoryUI.dragJar.rot;
					PlayerDashboardInventoryUI.stopDrag();
					if (page != PlayerDashboardInventoryUI.dragPage && Player.player.inventory.checkSpaceEmpty(page, x, y, PlayerDashboardInventoryUI.dragJar.size_x, PlayerDashboardInventoryUI.dragJar.size_y, rot))
					{
						ItemManager.takeItem(PlayerDashboardInventoryUI.dragItem.jar.interactableItem.transform.parent, x, y, rot, page);
					}
					return;
				}
				if (Player.player.inventory.checkSpaceDrag(page, PlayerDashboardInventoryUI.drag_x, PlayerDashboardInventoryUI.drag_y, PlayerDashboardInventoryUI.dragRot, x, y, PlayerDashboardInventoryUI.dragJar.rot, PlayerDashboardInventoryUI.dragJar.size_x, PlayerDashboardInventoryUI.dragJar.size_y, page == PlayerDashboardInventoryUI.dragPage))
				{
					byte rot2 = PlayerDashboardInventoryUI.dragJar.rot;
					PlayerDashboardInventoryUI.stopDrag();
					Player.player.inventory.sendDragItem(PlayerDashboardInventoryUI.dragPage, PlayerDashboardInventoryUI.drag_x, PlayerDashboardInventoryUI.drag_y, page, x, y, rot2);
					if (page < PlayerInventory.SLOTS)
					{
						Player.player.equipment.equip(page, 0, 0, PlayerDashboardInventoryUI.dragJar.item.id);
						PlayerDashboardUI.close();
						PlayerLifeUI.open();
					}
				}
				else if (page < PlayerInventory.SLOTS)
				{
					PlayerDashboardInventoryUI.stopDrag();
					PlayerDashboardInventoryUI.checkEquip(PlayerDashboardInventoryUI.dragPage, PlayerDashboardInventoryUI.drag_x, PlayerDashboardInventoryUI.drag_y, PlayerDashboardInventoryUI.dragJar, page);
				}
				else
				{
					byte rot3 = PlayerDashboardInventoryUI.dragJar.rot;
					PlayerDashboardInventoryUI.dragJar.rot = PlayerDashboardInventoryUI.dragRot;
					byte b4;
					byte b5;
					byte b3 = Player.player.inventory.findIndex(page, x, y, out b4, out b5);
					PlayerDashboardInventoryUI.dragJar.rot = rot3;
					if (b3 == 255)
					{
						return;
					}
					if (PlayerDashboardInventoryUI.dragPage == page && PlayerDashboardInventoryUI.drag_x == b4 && PlayerDashboardInventoryUI.drag_y == b5)
					{
						PlayerDashboardInventoryUI.stopDrag();
						return;
					}
					ItemJar item = Player.player.inventory.getItem(page, b3);
					if (Player.player.inventory.checkSpaceSwap(PlayerDashboardInventoryUI.dragPage, PlayerDashboardInventoryUI.drag_x, PlayerDashboardInventoryUI.drag_y, PlayerDashboardInventoryUI.dragJar.size_x, PlayerDashboardInventoryUI.dragJar.size_y, PlayerDashboardInventoryUI.dragRot, item.size_x, item.size_y, PlayerDashboardInventoryUI.dragRot) && Player.player.inventory.checkSpaceSwap(page, b4, b5, item.size_x, item.size_y, item.rot, PlayerDashboardInventoryUI.dragJar.size_x, PlayerDashboardInventoryUI.dragJar.size_y, item.rot))
					{
						ItemAsset itemAsset2 = (ItemAsset)Assets.find(EAssetType.ITEM, item.item.id);
						if (itemAsset2 != null && itemAsset2.slot == itemAsset.slot)
						{
							PlayerDashboardInventoryUI.stopDrag();
							Player.player.inventory.sendSwapItem(page, b4, b5, PlayerDashboardInventoryUI.dragPage, PlayerDashboardInventoryUI.drag_x, PlayerDashboardInventoryUI.drag_y);
							if (PlayerDashboardInventoryUI.dragPage < PlayerInventory.SLOTS)
							{
								PlayerDashboardInventoryUI.checkEquip(PlayerDashboardInventoryUI.dragPage, PlayerDashboardInventoryUI.drag_x, PlayerDashboardInventoryUI.drag_y, PlayerDashboardInventoryUI.dragJar, page);
							}
						}
					}
				}
			}
		}

		private static void onClickedMouse()
		{
			PlayerDashboardInventoryUI.closeSelection();
			if ((double)(Time.realtimeSinceStartup - PlayerDashboardInventoryUI.lastDrag) < 0.05)
			{
				return;
			}
			if (PlayerUI.window.mouse_x + PlayerUI.window.frame.x > Mathf.Max(PlayerDashboardInventoryUI.character.renderImage.frame.xMin, PlayerDashboardInventoryUI.character.frame.xMin) && PlayerUI.window.mouse_x + PlayerUI.window.frame.x < Mathf.Min(PlayerDashboardInventoryUI.character.renderImage.frame.xMax, PlayerDashboardInventoryUI.character.frame.xMax) && PlayerUI.window.mouse_y + PlayerUI.window.frame.y > Mathf.Max(PlayerDashboardInventoryUI.character.renderImage.frame.yMin, PlayerDashboardInventoryUI.character.frame.yMin) && PlayerUI.window.mouse_y + PlayerUI.window.frame.y < Mathf.Min(PlayerDashboardInventoryUI.character.renderImage.frame.yMax, PlayerDashboardInventoryUI.character.frame.yMax))
			{
				if (PlayerDashboardInventoryUI.dragSource != null && PlayerDashboardInventoryUI.isDragging)
				{
					PlayerDashboardInventoryUI.lastDrag = Time.realtimeSinceStartup;
					byte page = PlayerDashboardInventoryUI.dragPage;
					byte x = PlayerDashboardInventoryUI.drag_x;
					byte y = PlayerDashboardInventoryUI.drag_y;
					ItemJar jar = PlayerDashboardInventoryUI.dragJar;
					PlayerDashboardInventoryUI.stopDrag();
					PlayerDashboardInventoryUI.checkAction(page, x, y, jar);
				}
				else
				{
					Ray ray = Player.player.look.characterCamera.ScreenPointToRay(new Vector3((PlayerUI.window.mouse_x + PlayerUI.window.frame.x - PlayerDashboardInventoryUI.character.renderImage.frame.xMin) / PlayerDashboardInventoryUI.character.renderImage.frame.width * 1024f, (PlayerDashboardInventoryUI.character.renderImage.frame.yMax - (PlayerUI.window.mouse_y + PlayerUI.window.frame.y)) / PlayerDashboardInventoryUI.character.renderImage.frame.height * 1024f, 0f));
					RaycastHit raycastHit;
					Physics.Raycast(ray, ref raycastHit, 8f, RayMasks.CLOTHING_INTERACT);
					if (raycastHit.transform != null)
					{
						if (raycastHit.transform.CompareTag("Player"))
						{
							ELimb limb = DamageTool.getLimb(raycastHit.transform);
							if (limb == ELimb.LEFT_FOOT || limb == ELimb.LEFT_LEG || limb == ELimb.RIGHT_FOOT || limb == ELimb.RIGHT_LEG)
							{
								Player.player.clothing.sendSwapPants(byte.MaxValue, byte.MaxValue, byte.MaxValue);
							}
							else if (limb == ELimb.LEFT_HAND || limb == ELimb.LEFT_ARM || limb == ELimb.RIGHT_HAND || limb == ELimb.RIGHT_ARM || limb == ELimb.SPINE)
							{
								Player.player.clothing.sendSwapShirt(byte.MaxValue, byte.MaxValue, byte.MaxValue);
							}
						}
						else if (raycastHit.transform.CompareTag("Enemy"))
						{
							if (raycastHit.transform.name == "Hat")
							{
								Player.player.clothing.sendSwapHat(byte.MaxValue, byte.MaxValue, byte.MaxValue);
							}
							else if (raycastHit.transform.name == "Glasses")
							{
								Player.player.clothing.sendSwapGlasses(byte.MaxValue, byte.MaxValue, byte.MaxValue);
							}
							else if (raycastHit.transform.name == "Mask")
							{
								Player.player.clothing.sendSwapMask(byte.MaxValue, byte.MaxValue, byte.MaxValue);
							}
							else if (raycastHit.transform.name == "Vest")
							{
								Player.player.clothing.sendSwapVest(byte.MaxValue, byte.MaxValue, byte.MaxValue);
							}
							else if (raycastHit.transform.name == "Backpack")
							{
								Player.player.clothing.sendSwapBackpack(byte.MaxValue, byte.MaxValue, byte.MaxValue);
							}
						}
						else if (raycastHit.transform.CompareTag("Item"))
						{
							Player.player.equipment.dequip();
						}
					}
				}
				return;
			}
			for (int i = 0; i < PlayerDashboardInventoryUI.slots.Length; i++)
			{
				if (PlayerUI.window.mouse_x + PlayerUI.window.frame.x > PlayerDashboardInventoryUI.slots[i].frame.xMin && PlayerUI.window.mouse_x + PlayerUI.window.frame.x < PlayerDashboardInventoryUI.slots[i].frame.xMax && PlayerUI.window.mouse_y + PlayerUI.window.frame.y > PlayerDashboardInventoryUI.slots[i].frame.yMin && PlayerUI.window.mouse_y + PlayerUI.window.frame.y < PlayerDashboardInventoryUI.slots[i].frame.yMax)
				{
					PlayerDashboardInventoryUI.slots[i].select();
					return;
				}
			}
			if (PlayerDashboardInventoryUI.dragSource != null && PlayerDashboardInventoryUI.isDragging)
			{
				PlayerDashboardInventoryUI.lastDrag = Time.realtimeSinceStartup;
				byte b = PlayerDashboardInventoryUI.dragPage;
				byte x2 = PlayerDashboardInventoryUI.drag_x;
				byte y2 = PlayerDashboardInventoryUI.drag_y;
				PlayerDashboardInventoryUI.stopDrag();
				if (b != PlayerInventory.AREA)
				{
					Player.player.inventory.sendDropItem(b, x2, y2);
				}
				return;
			}
		}

		private static void onMovedMouse(float x, float y)
		{
			if (PlayerDashboardInventoryUI.isDragging)
			{
				PlayerDashboardInventoryUI.dragItem.positionOffset_X = (int)(x + PlayerDashboardInventoryUI.dragPivot.x);
				PlayerDashboardInventoryUI.dragItem.positionOffset_Y = (int)(y + PlayerDashboardInventoryUI.dragPivot.y);
			}
		}

		private static void onItemDropAdded(Transform model, InteractableItem interactableItem)
		{
			if (!PlayerDashboardInventoryUI.active || !PlayerDashboardUI.active)
			{
				return;
			}
			if (Player.player == null)
			{
				return;
			}
			if ((model.position - Player.player.transform.position).sqrMagnitude > 16f)
			{
				return;
			}
			if (PlayerDashboardInventoryUI.areaItems.getItemCount() >= 200)
			{
				return;
			}
			Renderer componentInChildren = model.GetComponentInChildren<Renderer>();
			if (componentInChildren == null)
			{
				return;
			}
			if (Physics.Linecast(Player.player.look.aim.position, componentInChildren.bounds.center, RayMasks.BLOCK_PICKUP, 1))
			{
				return;
			}
			Item item = interactableItem.item;
			if (item == null)
			{
				return;
			}
			while (!PlayerDashboardInventoryUI.areaItems.tryAddItem(item))
			{
				if (PlayerDashboardInventoryUI.areaItems.height >= 200)
				{
					return;
				}
				PlayerDashboardInventoryUI.areaItems.resize(PlayerDashboardInventoryUI.areaItems.width, PlayerDashboardInventoryUI.areaItems.height + 1);
			}
			ItemJar item2 = PlayerDashboardInventoryUI.areaItems.getItem(PlayerDashboardInventoryUI.areaItems.getItemCount() - 1);
			item2.interactableItem = interactableItem;
			interactableItem.jar = item2;
			byte b = PlayerDashboardInventoryUI.areaItems.height - (item2.y + ((item2.rot % 2 != 0) ? item2.size_x : item2.size_y));
			if (b < 3 && PlayerDashboardInventoryUI.areaItems.height + b <= 200)
			{
				PlayerDashboardInventoryUI.areaItems.resize(PlayerDashboardInventoryUI.areaItems.width, PlayerDashboardInventoryUI.areaItems.height + (3 - b));
			}
			PlayerDashboardInventoryUI.items[(int)(PlayerInventory.AREA - PlayerInventory.SLOTS)].resize(PlayerDashboardInventoryUI.areaItems.width, PlayerDashboardInventoryUI.areaItems.height);
			PlayerDashboardInventoryUI.items[(int)(PlayerInventory.AREA - PlayerInventory.SLOTS)].addItem(item2);
			if (!PlayerDashboardInventoryUI.isSplitClothingArea)
			{
				PlayerDashboardInventoryUI.updateBoxAreas();
			}
		}

		private static void onItemDropRemoved(Transform model, InteractableItem interactableItem)
		{
			if (!PlayerDashboardInventoryUI.active || !PlayerDashboardUI.active)
			{
				return;
			}
			if (Player.player == null)
			{
				return;
			}
			if ((model.position - Player.player.transform.position).sqrMagnitude > 16f)
			{
				return;
			}
			ItemJar jar = interactableItem.jar;
			if (jar == null)
			{
				return;
			}
			byte index = PlayerDashboardInventoryUI.areaItems.getIndex(jar.x, jar.y);
			if (index == 255)
			{
				return;
			}
			PlayerDashboardInventoryUI.areaItems.removeItem(index);
			PlayerDashboardInventoryUI.items[(int)(PlayerInventory.AREA - PlayerInventory.SLOTS)].removeItem(jar);
		}

		private static void onSeated(bool isDriver, bool inVehicle, bool wasVehicle, InteractableVehicle oldVehicle, InteractableVehicle newVehicle)
		{
			if (oldVehicle != null)
			{
				if (PlayerDashboardInventoryUI.<>f__mg$cache4 == null)
				{
					PlayerDashboardInventoryUI.<>f__mg$cache4 = new VehiclePassengersUpdated(PlayerDashboardInventoryUI.updateVehicle);
				}
				oldVehicle.onPassengersUpdated -= PlayerDashboardInventoryUI.<>f__mg$cache4;
				if (PlayerDashboardInventoryUI.<>f__mg$cache5 == null)
				{
					PlayerDashboardInventoryUI.<>f__mg$cache5 = new VehicleLockUpdated(PlayerDashboardInventoryUI.updateVehicle);
				}
				oldVehicle.onLockUpdated -= PlayerDashboardInventoryUI.<>f__mg$cache5;
				if (PlayerDashboardInventoryUI.<>f__mg$cache6 == null)
				{
					PlayerDashboardInventoryUI.<>f__mg$cache6 = new VehicleHeadlightsUpdated(PlayerDashboardInventoryUI.updateVehicle);
				}
				oldVehicle.onHeadlightsUpdated -= PlayerDashboardInventoryUI.<>f__mg$cache6;
				if (PlayerDashboardInventoryUI.<>f__mg$cache7 == null)
				{
					PlayerDashboardInventoryUI.<>f__mg$cache7 = new VehicleSirensUpdated(PlayerDashboardInventoryUI.updateVehicle);
				}
				oldVehicle.onSirensUpdated -= PlayerDashboardInventoryUI.<>f__mg$cache7;
				if (PlayerDashboardInventoryUI.<>f__mg$cache8 == null)
				{
					PlayerDashboardInventoryUI.<>f__mg$cache8 = new VehicleBatteryChangedHandler(PlayerDashboardInventoryUI.updateVehicle);
				}
				oldVehicle.batteryChanged -= PlayerDashboardInventoryUI.<>f__mg$cache8;
			}
			if (newVehicle != null)
			{
				if (PlayerDashboardInventoryUI.<>f__mg$cache9 == null)
				{
					PlayerDashboardInventoryUI.<>f__mg$cache9 = new VehiclePassengersUpdated(PlayerDashboardInventoryUI.updateVehicle);
				}
				newVehicle.onPassengersUpdated += PlayerDashboardInventoryUI.<>f__mg$cache9;
				if (PlayerDashboardInventoryUI.<>f__mg$cacheA == null)
				{
					PlayerDashboardInventoryUI.<>f__mg$cacheA = new VehicleLockUpdated(PlayerDashboardInventoryUI.updateVehicle);
				}
				newVehicle.onLockUpdated += PlayerDashboardInventoryUI.<>f__mg$cacheA;
				if (PlayerDashboardInventoryUI.<>f__mg$cacheB == null)
				{
					PlayerDashboardInventoryUI.<>f__mg$cacheB = new VehicleHeadlightsUpdated(PlayerDashboardInventoryUI.updateVehicle);
				}
				newVehicle.onHeadlightsUpdated += PlayerDashboardInventoryUI.<>f__mg$cacheB;
				if (PlayerDashboardInventoryUI.<>f__mg$cacheC == null)
				{
					PlayerDashboardInventoryUI.<>f__mg$cacheC = new VehicleSirensUpdated(PlayerDashboardInventoryUI.updateVehicle);
				}
				newVehicle.onSirensUpdated += PlayerDashboardInventoryUI.<>f__mg$cacheC;
				if (PlayerDashboardInventoryUI.<>f__mg$cacheD == null)
				{
					PlayerDashboardInventoryUI.<>f__mg$cacheD = new VehicleBatteryChangedHandler(PlayerDashboardInventoryUI.updateVehicle);
				}
				newVehicle.batteryChanged += PlayerDashboardInventoryUI.<>f__mg$cacheD;
			}
			PlayerDashboardInventoryUI.updateVehicle();
		}

		private static void updateVehicle()
		{
			if (!PlayerDashboardInventoryUI.active)
			{
				return;
			}
			InteractableVehicle vehicle = Player.player.movement.getVehicle();
			if (vehicle != null && vehicle.asset != null)
			{
				VehicleAsset asset = vehicle.asset;
				PlayerDashboardInventoryUI.vehicleNameLabel.text = asset.vehicleName;
				PlayerDashboardInventoryUI.vehicleNameLabel.foregroundColor = ItemTool.getRarityColorUI(asset.rarity);
				int num = 0;
				int num2 = 0;
				PlayerDashboardInventoryUI.vehicleLockButton.text = PlayerDashboardInventoryUI.localization.format((!vehicle.isLocked) ? "Vehicle_Lock_On" : "Vehicle_Lock_Off", new object[]
				{
					MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.locker)
				});
				PlayerDashboardInventoryUI.vehicleLockButton.tooltip = PlayerDashboardInventoryUI.localization.format("Vehicle_Lock_Tooltip");
				PlayerDashboardInventoryUI.vehicleLockButton.isVisible = true;
				PlayerDashboardInventoryUI.vehicleLockButton.positionOffset_Y = num;
				num += 40;
				if (asset.horn != null)
				{
					PlayerDashboardInventoryUI.vehicleHornButton.text = PlayerDashboardInventoryUI.localization.format("Vehicle_Horn", new object[]
					{
						MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.primary)
					});
					PlayerDashboardInventoryUI.vehicleHornButton.tooltip = PlayerDashboardInventoryUI.localization.format("Vehicle_Horn_Tooltip");
					PlayerDashboardInventoryUI.vehicleHornButton.isVisible = true;
					PlayerDashboardInventoryUI.vehicleHornButton.positionOffset_Y = num;
					num += 40;
				}
				else
				{
					PlayerDashboardInventoryUI.vehicleHornButton.isVisible = false;
				}
				if (asset.hasHeadlights)
				{
					PlayerDashboardInventoryUI.vehicleHeadlightsButton.text = PlayerDashboardInventoryUI.localization.format((!vehicle.headlightsOn) ? "Vehicle_Headlights_On" : "Vehicle_Headlights_Off", new object[]
					{
						MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.secondary)
					});
					PlayerDashboardInventoryUI.vehicleHeadlightsButton.tooltip = PlayerDashboardInventoryUI.localization.format("Vehicle_Headlights_Tooltip");
					PlayerDashboardInventoryUI.vehicleHeadlightsButton.isVisible = true;
					PlayerDashboardInventoryUI.vehicleHeadlightsButton.positionOffset_Y = num;
					num += 40;
				}
				else
				{
					PlayerDashboardInventoryUI.vehicleHeadlightsButton.isVisible = false;
				}
				if (asset.hasSirens)
				{
					PlayerDashboardInventoryUI.vehicleSirensButton.text = PlayerDashboardInventoryUI.localization.format((!vehicle.sirensOn) ? "Vehicle_Sirens_On" : "Vehicle_Sirens_Off", new object[]
					{
						MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.other)
					});
					PlayerDashboardInventoryUI.vehicleSirensButton.tooltip = PlayerDashboardInventoryUI.localization.format("Vehicle_Sirens_Tooltip");
					PlayerDashboardInventoryUI.vehicleSirensButton.isVisible = true;
					PlayerDashboardInventoryUI.vehicleSirensButton.positionOffset_Y = num;
					num += 40;
				}
				else
				{
					PlayerDashboardInventoryUI.vehicleSirensButton.isVisible = false;
				}
				if (asset.hasHook)
				{
					PlayerDashboardInventoryUI.vehicleHookButton.text = PlayerDashboardInventoryUI.localization.format("Vehicle_Hook", new object[]
					{
						MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.other)
					});
					PlayerDashboardInventoryUI.vehicleHookButton.tooltip = PlayerDashboardInventoryUI.localization.format("Vehicle_Hook_Tooltip");
					PlayerDashboardInventoryUI.vehicleHookButton.isVisible = true;
					PlayerDashboardInventoryUI.vehicleHookButton.positionOffset_Y = num;
					num += 40;
				}
				else
				{
					PlayerDashboardInventoryUI.vehicleHookButton.isVisible = false;
				}
				if (vehicle.hasBattery)
				{
					PlayerDashboardInventoryUI.vehicleStealBatteryButton.text = PlayerDashboardInventoryUI.localization.format("Vehicle_Steal_Battery");
					PlayerDashboardInventoryUI.vehicleStealBatteryButton.tooltip = PlayerDashboardInventoryUI.localization.format("Vehicle_Steal_Battery_Tooltip");
					PlayerDashboardInventoryUI.vehicleStealBatteryButton.isVisible = true;
					PlayerDashboardInventoryUI.vehicleStealBatteryButton.positionOffset_Y = num;
					num += 40;
				}
				else
				{
					PlayerDashboardInventoryUI.vehicleStealBatteryButton.isVisible = false;
				}
				if (Player.player.stance.stance == EPlayerStance.DRIVING)
				{
					PlayerDashboardInventoryUI.vehiclePassengersBox.positionOffset_X = 270;
					PlayerDashboardInventoryUI.vehiclePassengersBox.sizeOffset_X = -280;
					PlayerDashboardInventoryUI.vehicleActionsBox.isVisible = true;
				}
				else
				{
					PlayerDashboardInventoryUI.vehiclePassengersBox.positionOffset_X = 10;
					PlayerDashboardInventoryUI.vehiclePassengersBox.sizeOffset_X = -20;
					PlayerDashboardInventoryUI.vehicleActionsBox.isVisible = false;
				}
				PlayerDashboardInventoryUI.vehiclePassengersBox.remove();
				for (int i = 0; i < vehicle.passengers.Length; i++)
				{
					Passenger passenger = vehicle.passengers[i];
					SleekButton sleekButton = new SleekButton();
					sleekButton.positionOffset_Y = num2;
					sleekButton.sizeOffset_Y = 30;
					sleekButton.sizeScale_X = 1f;
					SleekButton sleekButton2 = sleekButton;
					if (PlayerDashboardInventoryUI.<>f__mg$cacheE == null)
					{
						PlayerDashboardInventoryUI.<>f__mg$cacheE = new ClickedButton(PlayerDashboardInventoryUI.onClickedVehiclePassengerButton);
					}
					sleekButton2.onClickedButton = PlayerDashboardInventoryUI.<>f__mg$cacheE;
					PlayerDashboardInventoryUI.vehiclePassengersBox.add(sleekButton);
					sleekButton.backgroundTint = ESleekTint.NONE;
					sleekButton.foregroundTint = ESleekTint.NONE;
					if (passenger.player != null)
					{
						string text;
						if (passenger.player.player.quests.isMemberOfSameGroupAs(Player.player))
						{
							if (passenger.player.playerID.nickName != string.Empty && passenger.player.playerID.steamID != Provider.client)
							{
								text = passenger.player.playerID.nickName;
							}
							else
							{
								text = passenger.player.playerID.characterName;
							}
						}
						else
						{
							text = passenger.player.playerID.characterName;
						}
						if (i < 12)
						{
							sleekButton.text = PlayerDashboardInventoryUI.localization.format("Vehicle_Seat_Slot", new object[]
							{
								text,
								"F" + (i + 1)
							});
						}
						else
						{
							sleekButton.text = text;
						}
						if (passenger.player.isAdmin && !Provider.isServer)
						{
							sleekButton.backgroundColor = Palette.ADMIN;
							sleekButton.foregroundColor = Palette.ADMIN;
						}
						else if (passenger.player.isPro)
						{
							sleekButton.backgroundColor = Palette.PRO;
							sleekButton.foregroundColor = Palette.PRO;
						}
					}
					else if (i < 12)
					{
						sleekButton.text = PlayerDashboardInventoryUI.localization.format("Vehicle_Seat_Slot", new object[]
						{
							PlayerDashboardInventoryUI.localization.format("Vehicle_Seat_Empty"),
							"F" + (i + 1)
						});
					}
					else
					{
						sleekButton.text = PlayerDashboardInventoryUI.localization.format("Vehicle_Seat_Empty");
					}
					num2 += 40;
				}
				PlayerDashboardInventoryUI.vehicleActionsBox.sizeOffset_Y = num - 10;
				PlayerDashboardInventoryUI.vehiclePassengersBox.sizeOffset_Y = num2 - 10;
				PlayerDashboardInventoryUI.vehicleBox.isVisible = true;
				int num3 = Mathf.Max(num, num2);
				PlayerDashboardInventoryUI.vehicleBox.sizeOffset_Y = 60 + num3;
			}
			else
			{
				PlayerDashboardInventoryUI.vehicleBox.isVisible = false;
			}
		}

		private static void updateNearbyDrops()
		{
			PlayerDashboardInventoryUI.areaItems.clear();
			PlayerDashboardInventoryUI.regionsInRadius.Clear();
			Regions.getRegionsInRadius(Player.player.look.aim.position, 4f, PlayerDashboardInventoryUI.regionsInRadius);
			PlayerDashboardInventoryUI.itemsInRadius.Clear();
			ItemManager.getItemsInRadius(Player.player.look.aim.position, 16f, PlayerDashboardInventoryUI.regionsInRadius, PlayerDashboardInventoryUI.itemsInRadius);
			for (int i = PlayerDashboardInventoryUI.itemsInRadius.Count - 1; i >= 0; i--)
			{
				Renderer componentInChildren = PlayerDashboardInventoryUI.itemsInRadius[i].GetComponentInChildren<Renderer>();
				if (componentInChildren == null)
				{
					PlayerDashboardInventoryUI.itemsInRadius.RemoveAt(i);
				}
				else if (Physics.Linecast(Player.player.look.aim.position, componentInChildren.bounds.center, RayMasks.BLOCK_PICKUP, 1))
				{
					PlayerDashboardInventoryUI.itemsInRadius.RemoveAt(i);
				}
			}
			if (PlayerDashboardInventoryUI.itemsInRadius.Count > 0)
			{
				PlayerDashboardInventoryUI.areaItems.resize(8, 0);
				byte b = 0;
				while ((int)b < PlayerDashboardInventoryUI.itemsInRadius.Count)
				{
					if (PlayerDashboardInventoryUI.areaItems.getItemCount() >= 200)
					{
						break;
					}
					InteractableItem interactableItem = PlayerDashboardInventoryUI.itemsInRadius[(int)b];
					if (!(interactableItem == null))
					{
						Item item = interactableItem.item;
						if (item != null)
						{
							bool flag;
							while (!PlayerDashboardInventoryUI.areaItems.tryAddItem(item))
							{
								if (PlayerDashboardInventoryUI.areaItems.height < 200)
								{
									PlayerDashboardInventoryUI.areaItems.resize(PlayerDashboardInventoryUI.areaItems.width, PlayerDashboardInventoryUI.areaItems.height + 1);
								}
								else
								{
									flag = false;
									IL_1C8:
									if (flag)
									{
										ItemJar item2 = PlayerDashboardInventoryUI.areaItems.getItem(PlayerDashboardInventoryUI.areaItems.getItemCount() - 1);
										item2.interactableItem = interactableItem;
										interactableItem.jar = item2;
										goto IL_204;
									}
									goto IL_219;
								}
							}
							flag = true;
							goto IL_1C8;
						}
					}
					IL_204:
					b += 1;
				}
				IL_219:
				if (PlayerDashboardInventoryUI.areaItems.height + 3 <= 200)
				{
					PlayerDashboardInventoryUI.areaItems.resize(PlayerDashboardInventoryUI.areaItems.width, PlayerDashboardInventoryUI.areaItems.height + 3);
				}
			}
			else
			{
				PlayerDashboardInventoryUI.areaItems.resize(8, 3);
			}
			Player.player.inventory.replaceItems(PlayerInventory.AREA, PlayerDashboardInventoryUI.areaItems);
			PlayerDashboardInventoryUI.items[(int)(PlayerInventory.AREA - PlayerInventory.SLOTS)].clear();
			PlayerDashboardInventoryUI.items[(int)(PlayerInventory.AREA - PlayerInventory.SLOTS)].resize(PlayerDashboardInventoryUI.areaItems.width, PlayerDashboardInventoryUI.areaItems.height);
			for (int j = 0; j < (int)PlayerDashboardInventoryUI.areaItems.getItemCount(); j++)
			{
				PlayerDashboardInventoryUI.items[(int)(PlayerInventory.AREA - PlayerInventory.SLOTS)].addItem(PlayerDashboardInventoryUI.areaItems.getItem((byte)j));
			}
			PlayerDashboardInventoryUI.updateBoxAreas();
		}

		private static void onInventoryResized(byte page, byte newWidth, byte newHeight)
		{
			if (page < PlayerInventory.SLOTS)
			{
				return;
			}
			page -= PlayerInventory.SLOTS;
			PlayerDashboardInventoryUI.items[(int)page].resize(newWidth, newHeight);
			if (page > 0)
			{
				PlayerDashboardInventoryUI.headers[(int)page].isVisible = (newHeight > 0);
			}
			PlayerDashboardInventoryUI.items[(int)page].isVisible = (newHeight > 0);
			if (page == PlayerInventory.STORAGE - PlayerInventory.SLOTS && newHeight == 0)
			{
				PlayerDashboardInventoryUI.items[(int)page].clear();
			}
			PlayerDashboardInventoryUI.updateBoxAreas();
		}

		private static void updateBoxAreas()
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			if (PlayerDashboardInventoryUI.vehicleBox.isVisible)
			{
				if (PlayerDashboardInventoryUI.isSplitClothingArea)
				{
					if (PlayerDashboardInventoryUI.vehicleBox.parent != PlayerDashboardInventoryUI.areaBox)
					{
						PlayerDashboardInventoryUI.clothingBox.remove(PlayerDashboardInventoryUI.vehicleBox);
						PlayerDashboardInventoryUI.areaBox.add(PlayerDashboardInventoryUI.vehicleBox);
					}
					PlayerDashboardInventoryUI.vehicleBox.positionOffset_Y = num4;
					num4 += PlayerDashboardInventoryUI.vehicleBox.sizeOffset_Y + 10;
				}
				else
				{
					if (PlayerDashboardInventoryUI.vehicleBox.parent != PlayerDashboardInventoryUI.clothingBox)
					{
						PlayerDashboardInventoryUI.areaBox.remove(PlayerDashboardInventoryUI.vehicleBox);
						PlayerDashboardInventoryUI.clothingBox.add(PlayerDashboardInventoryUI.vehicleBox);
					}
					PlayerDashboardInventoryUI.vehicleBox.positionOffset_Y = num2;
					num2 += PlayerDashboardInventoryUI.vehicleBox.sizeOffset_Y + 10;
				}
			}
			byte b = 0;
			while ((int)b < PlayerDashboardInventoryUI.items.Length)
			{
				if (PlayerDashboardInventoryUI.headers[(int)b].isVisible)
				{
					if (PlayerDashboardInventoryUI.isSplitClothingArea && (b == PlayerInventory.STORAGE - PlayerInventory.SLOTS || b == PlayerInventory.AREA - PlayerInventory.SLOTS))
					{
						if (PlayerDashboardInventoryUI.headers[(int)b].parent != PlayerDashboardInventoryUI.areaBox)
						{
							PlayerDashboardInventoryUI.clothingBox.remove(PlayerDashboardInventoryUI.headers[(int)b]);
							PlayerDashboardInventoryUI.areaBox.add(PlayerDashboardInventoryUI.headers[(int)b]);
						}
						if (PlayerDashboardInventoryUI.items[(int)b].parent != PlayerDashboardInventoryUI.areaBox)
						{
							PlayerDashboardInventoryUI.clothingBox.remove(PlayerDashboardInventoryUI.items[(int)b]);
							PlayerDashboardInventoryUI.areaBox.add(PlayerDashboardInventoryUI.items[(int)b]);
						}
						PlayerDashboardInventoryUI.headers[(int)b].positionOffset_Y = num4;
						PlayerDashboardInventoryUI.items[(int)b].positionOffset_Y = num4 + 70;
						if (PlayerDashboardInventoryUI.items[(int)b].sizeOffset_X > num3)
						{
							num3 = PlayerDashboardInventoryUI.items[(int)b].sizeOffset_X;
						}
						num4 += PlayerDashboardInventoryUI.items[(int)b].sizeOffset_Y + 80;
					}
					else
					{
						if (PlayerDashboardInventoryUI.headers[(int)b].parent != PlayerDashboardInventoryUI.clothingBox)
						{
							PlayerDashboardInventoryUI.areaBox.remove(PlayerDashboardInventoryUI.headers[(int)b]);
							PlayerDashboardInventoryUI.clothingBox.add(PlayerDashboardInventoryUI.headers[(int)b]);
						}
						if (PlayerDashboardInventoryUI.items[(int)b].parent != PlayerDashboardInventoryUI.clothingBox)
						{
							PlayerDashboardInventoryUI.areaBox.remove(PlayerDashboardInventoryUI.items[(int)b]);
							PlayerDashboardInventoryUI.clothingBox.add(PlayerDashboardInventoryUI.items[(int)b]);
						}
						PlayerDashboardInventoryUI.headers[(int)b].positionOffset_Y = num2;
						PlayerDashboardInventoryUI.items[(int)b].positionOffset_Y = num2 + 70;
						if (PlayerDashboardInventoryUI.items[(int)b].sizeOffset_X > num)
						{
							num = PlayerDashboardInventoryUI.items[(int)b].sizeOffset_X;
						}
						num2 += PlayerDashboardInventoryUI.items[(int)b].sizeOffset_Y + 80;
					}
				}
				b += 1;
			}
			PlayerDashboardInventoryUI.headers[7].isVisible = (Player.player.clothing.hat != 0);
			if (PlayerDashboardInventoryUI.headers[7].isVisible)
			{
				PlayerDashboardInventoryUI.headers[7].positionOffset_Y = num2;
				num2 += 70;
			}
			PlayerDashboardInventoryUI.headers[8].isVisible = (Player.player.clothing.mask != 0);
			if (PlayerDashboardInventoryUI.headers[8].isVisible)
			{
				PlayerDashboardInventoryUI.headers[8].positionOffset_Y = num2;
				num2 += 70;
			}
			PlayerDashboardInventoryUI.headers[9].isVisible = (Player.player.clothing.glasses != 0);
			if (PlayerDashboardInventoryUI.headers[9].isVisible)
			{
				PlayerDashboardInventoryUI.headers[9].positionOffset_Y = num2;
				num2 += 70;
			}
			PlayerDashboardInventoryUI.clothingBox.area = new Rect(0f, 0f, (float)(num + 100), (float)(num2 - 10));
			PlayerDashboardInventoryUI.areaBox.area = new Rect(0f, 0f, (float)(num3 + 100), (float)(num4 - 10));
			InteractableStorage interactableStorage = PlayerInteract.interactable as InteractableStorage;
			if (interactableStorage != null && interactableStorage.isDisplay)
			{
				PlayerDashboardInventoryUI.headers[(int)(PlayerInventory.STORAGE - PlayerInventory.SLOTS)].sizeOffset_X = -310;
				PlayerDashboardInventoryUI.rot_xButton.isVisible = true;
				PlayerDashboardInventoryUI.rot_yButton.isVisible = true;
				PlayerDashboardInventoryUI.rot_zButton.isVisible = true;
			}
			else
			{
				PlayerDashboardInventoryUI.headers[(int)(PlayerInventory.STORAGE - PlayerInventory.SLOTS)].sizeOffset_X = -130;
				PlayerDashboardInventoryUI.rot_xButton.isVisible = false;
				PlayerDashboardInventoryUI.rot_yButton.isVisible = false;
				PlayerDashboardInventoryUI.rot_zButton.isVisible = false;
			}
		}

		private static void updateHotkeys()
		{
			for (byte b = 0; b < PlayerInventory.STORAGE - PlayerInventory.SLOTS; b += 1)
			{
				SleekItems sleekItems = PlayerDashboardInventoryUI.items[(int)b];
				byte b2 = 0;
				while ((int)b2 < sleekItems.items.Count)
				{
					SleekItem sleekItem = sleekItems.items[(int)b2];
					if (sleekItem.hotkey != 255)
					{
						sleekItem.updateHotkey(byte.MaxValue);
					}
					b2 += 1;
				}
			}
			byte b3 = 0;
			while ((int)b3 < Player.player.equipment.hotkeys.Length)
			{
				HotkeyInfo hotkeyInfo = Player.player.equipment.hotkeys[(int)b3];
				byte index = b3 + 2;
				byte b4 = hotkeyInfo.page - 2;
				if (hotkeyInfo.id != 0)
				{
					byte index2 = Player.player.inventory.getIndex(hotkeyInfo.page, hotkeyInfo.x, hotkeyInfo.y);
					ItemJar item = Player.player.inventory.getItem(hotkeyInfo.page, index2);
					if (item == null || item.item.id != hotkeyInfo.id)
					{
						hotkeyInfo.id = 0;
						hotkeyInfo.page = byte.MaxValue;
						hotkeyInfo.x = byte.MaxValue;
						hotkeyInfo.y = byte.MaxValue;
					}
					else if ((int)index2 < PlayerDashboardInventoryUI.items[(int)b4].items.Count)
					{
						PlayerDashboardInventoryUI.items[(int)b4].items[(int)index2].updateHotkey(index);
					}
				}
				b3 += 1;
			}
		}

		private static void onHotkeysUpdated()
		{
			PlayerDashboardInventoryUI.updateHotkeys();
		}

		private static void onInventoryUpdated(byte page, byte index, ItemJar jar)
		{
			if (page < PlayerInventory.SLOTS)
			{
				PlayerDashboardInventoryUI.slots[(int)page].updateItem(jar);
			}
			else
			{
				page -= PlayerInventory.SLOTS;
				PlayerDashboardInventoryUI.items[(int)page].updateItem(index, jar);
			}
		}

		private static void onInventoryAdded(byte page, byte index, ItemJar jar)
		{
			if (page < PlayerInventory.SLOTS)
			{
				PlayerDashboardInventoryUI.slots[(int)page].applyItem(jar);
			}
			else
			{
				page -= PlayerInventory.SLOTS;
				PlayerDashboardInventoryUI.items[(int)page].addItem(jar);
			}
		}

		private static void onInventoryRemoved(byte page, byte index, ItemJar jar)
		{
			if (page == PlayerDashboardInventoryUI.selectedPage && jar.x == PlayerDashboardInventoryUI.selected_x && jar.y == PlayerDashboardInventoryUI.selected_y)
			{
				PlayerDashboardInventoryUI.closeSelection();
			}
			if (page < PlayerInventory.SLOTS)
			{
				PlayerDashboardInventoryUI.slots[(int)page].applyItem(null);
			}
			else
			{
				page -= PlayerInventory.SLOTS;
				PlayerDashboardInventoryUI.items[(int)page].removeItem(jar);
			}
		}

		private static void onInventoryStored()
		{
			PlayerLifeUI.close();
			PlayerPauseUI.close();
			if (PlayerDashboardUI.active)
			{
				PlayerDashboardCraftingUI.close();
				PlayerDashboardSkillsUI.close();
				PlayerDashboardInformationUI.close();
				PlayerDashboardInventoryUI.open();
			}
			else
			{
				PlayerDashboardInventoryUI.active = true;
				PlayerDashboardCraftingUI.active = false;
				PlayerDashboardSkillsUI.active = false;
				PlayerDashboardInformationUI.active = false;
				PlayerDashboardUI.open();
			}
			if (!PlayerDashboardInventoryUI.isSplitClothingArea)
			{
				PlayerDashboardInventoryUI.clothingBox.state = new Vector2(0f, (float)PlayerDashboardInventoryUI.headers[(int)(PlayerInventory.STORAGE - PlayerInventory.SLOTS)].positionOffset_Y);
			}
		}

		private static void onShirtIconReady(Texture2D texture)
		{
			((SleekImageTexture)PlayerDashboardInventoryUI.headers[3].children[0]).texture = texture;
		}

		private static void onShirtUpdated(ushort newShirt, byte newShirtQuality, byte[] newShirtState)
		{
			ItemAsset itemAsset = (ItemAsset)Assets.find(EAssetType.ITEM, newShirt);
			if (itemAsset != null)
			{
				PlayerDashboardInventoryUI.headers[3].text = itemAsset.itemName;
				PlayerDashboardInventoryUI.headers[3].children[0].sizeOffset_X = (int)(itemAsset.size_x * 25);
				PlayerDashboardInventoryUI.headers[3].children[0].sizeOffset_Y = (int)(itemAsset.size_y * 25);
				PlayerDashboardInventoryUI.headers[3].children[0].positionOffset_Y = -PlayerDashboardInventoryUI.headers[3].children[0].sizeOffset_Y / 2;
				if (PlayerDashboardInventoryUI.<>f__mg$cacheF == null)
				{
					PlayerDashboardInventoryUI.<>f__mg$cacheF = new ItemIconReady(PlayerDashboardInventoryUI.onShirtIconReady);
				}
				ItemTool.getIcon(newShirt, newShirtQuality, newShirtState, PlayerDashboardInventoryUI.<>f__mg$cacheF);
				((SleekLabel)PlayerDashboardInventoryUI.headers[3].children[2]).text = newShirtQuality + "%";
				PlayerDashboardInventoryUI.headers[3].backgroundColor = ItemTool.getRarityColorUI(itemAsset.rarity);
				PlayerDashboardInventoryUI.headers[3].foregroundColor = PlayerDashboardInventoryUI.headers[3].backgroundColor;
				PlayerDashboardInventoryUI.headers[3].children[1].backgroundColor = ItemTool.getQualityColor((float)newShirtQuality / 100f);
				PlayerDashboardInventoryUI.headers[3].children[1].foregroundColor = PlayerDashboardInventoryUI.headers[3].children[1].backgroundColor;
				PlayerDashboardInventoryUI.headers[3].children[2].backgroundColor = PlayerDashboardInventoryUI.headers[3].children[1].backgroundColor;
				PlayerDashboardInventoryUI.headers[3].children[2].foregroundColor = PlayerDashboardInventoryUI.headers[3].children[1].backgroundColor;
			}
		}

		private static void onPantsIconReady(Texture2D texture)
		{
			((SleekImageTexture)PlayerDashboardInventoryUI.headers[4].children[0]).texture = texture;
		}

		private static void onPantsUpdated(ushort newPants, byte newPantsQuality, byte[] newPantsState)
		{
			if (PlayerDashboardInventoryUI.headers != null)
			{
				ItemAsset itemAsset = (ItemAsset)Assets.find(EAssetType.ITEM, newPants);
				if (itemAsset != null)
				{
					PlayerDashboardInventoryUI.headers[4].text = itemAsset.itemName;
					PlayerDashboardInventoryUI.headers[4].children[0].sizeOffset_X = (int)(itemAsset.size_x * 25);
					PlayerDashboardInventoryUI.headers[4].children[0].sizeOffset_Y = (int)(itemAsset.size_y * 25);
					PlayerDashboardInventoryUI.headers[4].children[0].positionOffset_Y = -PlayerDashboardInventoryUI.headers[4].children[0].sizeOffset_Y / 2;
					if (PlayerDashboardInventoryUI.<>f__mg$cache10 == null)
					{
						PlayerDashboardInventoryUI.<>f__mg$cache10 = new ItemIconReady(PlayerDashboardInventoryUI.onPantsIconReady);
					}
					ItemTool.getIcon(newPants, newPantsQuality, newPantsState, PlayerDashboardInventoryUI.<>f__mg$cache10);
					((SleekLabel)PlayerDashboardInventoryUI.headers[4].children[2]).text = newPantsQuality + "%";
					PlayerDashboardInventoryUI.headers[4].backgroundColor = ItemTool.getRarityColorUI(itemAsset.rarity);
					PlayerDashboardInventoryUI.headers[4].foregroundColor = PlayerDashboardInventoryUI.headers[4].backgroundColor;
					PlayerDashboardInventoryUI.headers[4].children[1].backgroundColor = ItemTool.getQualityColor((float)newPantsQuality / 100f);
					PlayerDashboardInventoryUI.headers[4].children[1].foregroundColor = PlayerDashboardInventoryUI.headers[4].children[1].backgroundColor;
					PlayerDashboardInventoryUI.headers[4].children[2].backgroundColor = PlayerDashboardInventoryUI.headers[4].children[1].backgroundColor;
					PlayerDashboardInventoryUI.headers[4].children[2].foregroundColor = PlayerDashboardInventoryUI.headers[4].children[1].backgroundColor;
				}
			}
		}

		private static void onHatIconReady(Texture2D texture)
		{
			((SleekImageTexture)PlayerDashboardInventoryUI.headers[7].children[0]).texture = texture;
		}

		private static void onHatUpdated(ushort newHat, byte newHatQuality, byte[] newHatState)
		{
			if (PlayerDashboardInventoryUI.headers != null)
			{
				ItemAsset itemAsset = (ItemAsset)Assets.find(EAssetType.ITEM, newHat);
				if (itemAsset != null)
				{
					PlayerDashboardInventoryUI.headers[7].text = itemAsset.itemName;
					PlayerDashboardInventoryUI.headers[7].children[0].sizeOffset_X = (int)(itemAsset.size_x * 25);
					PlayerDashboardInventoryUI.headers[7].children[0].sizeOffset_Y = (int)(itemAsset.size_y * 25);
					PlayerDashboardInventoryUI.headers[7].children[0].positionOffset_Y = -PlayerDashboardInventoryUI.headers[7].children[0].sizeOffset_Y / 2;
					if (PlayerDashboardInventoryUI.<>f__mg$cache11 == null)
					{
						PlayerDashboardInventoryUI.<>f__mg$cache11 = new ItemIconReady(PlayerDashboardInventoryUI.onHatIconReady);
					}
					ItemTool.getIcon(newHat, newHatQuality, newHatState, PlayerDashboardInventoryUI.<>f__mg$cache11);
					((SleekLabel)PlayerDashboardInventoryUI.headers[7].children[2]).text = newHatQuality + "%";
					PlayerDashboardInventoryUI.headers[7].backgroundColor = ItemTool.getRarityColorUI(itemAsset.rarity);
					PlayerDashboardInventoryUI.headers[7].foregroundColor = PlayerDashboardInventoryUI.headers[7].backgroundColor;
					PlayerDashboardInventoryUI.headers[7].children[1].backgroundColor = ItemTool.getQualityColor((float)newHatQuality / 100f);
					PlayerDashboardInventoryUI.headers[7].children[1].foregroundColor = PlayerDashboardInventoryUI.headers[7].children[1].backgroundColor;
					PlayerDashboardInventoryUI.headers[7].children[2].backgroundColor = PlayerDashboardInventoryUI.headers[7].children[1].backgroundColor;
					PlayerDashboardInventoryUI.headers[7].children[2].foregroundColor = PlayerDashboardInventoryUI.headers[7].children[1].backgroundColor;
				}
				PlayerDashboardInventoryUI.headers[7].isVisible = (newHat != 0);
				PlayerDashboardInventoryUI.updateBoxAreas();
			}
		}

		private static void onBackpackIconReady(Texture2D texture)
		{
			((SleekImageTexture)PlayerDashboardInventoryUI.headers[1].children[0]).texture = texture;
		}

		private static void onBackpackUpdated(ushort newBackpack, byte newBackpackQuality, byte[] newBackpackState)
		{
			ItemAsset itemAsset = (ItemAsset)Assets.find(EAssetType.ITEM, newBackpack);
			if (itemAsset != null)
			{
				PlayerDashboardInventoryUI.headers[1].text = itemAsset.itemName;
				PlayerDashboardInventoryUI.headers[1].children[0].sizeOffset_X = (int)(itemAsset.size_x * 25);
				PlayerDashboardInventoryUI.headers[1].children[0].sizeOffset_Y = (int)(itemAsset.size_y * 25);
				PlayerDashboardInventoryUI.headers[1].children[0].positionOffset_Y = -PlayerDashboardInventoryUI.headers[1].children[0].sizeOffset_Y / 2;
				if (PlayerDashboardInventoryUI.<>f__mg$cache12 == null)
				{
					PlayerDashboardInventoryUI.<>f__mg$cache12 = new ItemIconReady(PlayerDashboardInventoryUI.onBackpackIconReady);
				}
				ItemTool.getIcon(newBackpack, newBackpackQuality, newBackpackState, PlayerDashboardInventoryUI.<>f__mg$cache12);
				((SleekLabel)PlayerDashboardInventoryUI.headers[1].children[2]).text = newBackpackQuality + "%";
				PlayerDashboardInventoryUI.headers[1].backgroundColor = ItemTool.getRarityColorUI(itemAsset.rarity);
				PlayerDashboardInventoryUI.headers[1].foregroundColor = PlayerDashboardInventoryUI.headers[1].backgroundColor;
				PlayerDashboardInventoryUI.headers[1].children[1].backgroundColor = ItemTool.getQualityColor((float)newBackpackQuality / 100f);
				PlayerDashboardInventoryUI.headers[1].children[1].foregroundColor = PlayerDashboardInventoryUI.headers[1].children[1].backgroundColor;
				PlayerDashboardInventoryUI.headers[1].children[2].backgroundColor = PlayerDashboardInventoryUI.headers[1].children[1].backgroundColor;
				PlayerDashboardInventoryUI.headers[1].children[2].foregroundColor = PlayerDashboardInventoryUI.headers[1].children[1].backgroundColor;
			}
		}

		private static void onVestIconReady(Texture2D texture)
		{
			((SleekImageTexture)PlayerDashboardInventoryUI.headers[2].children[0]).texture = texture;
		}

		private static void onVestUpdated(ushort newVest, byte newVestQuality, byte[] newVestState)
		{
			ItemAsset itemAsset = (ItemAsset)Assets.find(EAssetType.ITEM, newVest);
			if (itemAsset != null)
			{
				PlayerDashboardInventoryUI.headers[2].text = itemAsset.itemName;
				PlayerDashboardInventoryUI.headers[2].children[0].sizeOffset_X = (int)(itemAsset.size_x * 25);
				PlayerDashboardInventoryUI.headers[2].children[0].sizeOffset_Y = (int)(itemAsset.size_y * 25);
				PlayerDashboardInventoryUI.headers[2].children[0].positionOffset_Y = -PlayerDashboardInventoryUI.headers[2].children[0].sizeOffset_Y / 2;
				if (PlayerDashboardInventoryUI.<>f__mg$cache13 == null)
				{
					PlayerDashboardInventoryUI.<>f__mg$cache13 = new ItemIconReady(PlayerDashboardInventoryUI.onVestIconReady);
				}
				ItemTool.getIcon(newVest, newVestQuality, newVestState, PlayerDashboardInventoryUI.<>f__mg$cache13);
				((SleekLabel)PlayerDashboardInventoryUI.headers[2].children[2]).text = newVestQuality + "%";
				PlayerDashboardInventoryUI.headers[2].backgroundColor = ItemTool.getRarityColorUI(itemAsset.rarity);
				PlayerDashboardInventoryUI.headers[2].foregroundColor = PlayerDashboardInventoryUI.headers[2].backgroundColor;
				PlayerDashboardInventoryUI.headers[2].children[1].backgroundColor = ItemTool.getQualityColor((float)newVestQuality / 100f);
				PlayerDashboardInventoryUI.headers[2].children[1].foregroundColor = PlayerDashboardInventoryUI.headers[2].children[1].backgroundColor;
				PlayerDashboardInventoryUI.headers[2].children[2].backgroundColor = PlayerDashboardInventoryUI.headers[2].children[1].backgroundColor;
				PlayerDashboardInventoryUI.headers[2].children[2].foregroundColor = PlayerDashboardInventoryUI.headers[2].children[1].backgroundColor;
			}
		}

		private static void onMaskIconReady(Texture2D texture)
		{
			((SleekImageTexture)PlayerDashboardInventoryUI.headers[8].children[0]).texture = texture;
		}

		private static void onMaskUpdated(ushort newMask, byte newMaskQuality, byte[] newMaskState)
		{
			if (PlayerDashboardInventoryUI.headers != null)
			{
				ItemAsset itemAsset = (ItemAsset)Assets.find(EAssetType.ITEM, newMask);
				if (itemAsset != null)
				{
					PlayerDashboardInventoryUI.headers[8].text = itemAsset.itemName;
					PlayerDashboardInventoryUI.headers[8].children[0].sizeOffset_X = (int)(itemAsset.size_x * 25);
					PlayerDashboardInventoryUI.headers[8].children[0].sizeOffset_Y = (int)(itemAsset.size_y * 25);
					PlayerDashboardInventoryUI.headers[8].children[0].positionOffset_Y = -PlayerDashboardInventoryUI.headers[8].children[0].sizeOffset_Y / 2;
					if (PlayerDashboardInventoryUI.<>f__mg$cache14 == null)
					{
						PlayerDashboardInventoryUI.<>f__mg$cache14 = new ItemIconReady(PlayerDashboardInventoryUI.onMaskIconReady);
					}
					ItemTool.getIcon(newMask, newMaskQuality, newMaskState, PlayerDashboardInventoryUI.<>f__mg$cache14);
					((SleekLabel)PlayerDashboardInventoryUI.headers[8].children[2]).text = newMaskQuality + "%";
					PlayerDashboardInventoryUI.headers[8].backgroundColor = ItemTool.getRarityColorUI(itemAsset.rarity);
					PlayerDashboardInventoryUI.headers[8].foregroundColor = PlayerDashboardInventoryUI.headers[8].backgroundColor;
					PlayerDashboardInventoryUI.headers[8].children[1].backgroundColor = ItemTool.getQualityColor((float)newMaskQuality / 100f);
					PlayerDashboardInventoryUI.headers[8].children[1].foregroundColor = PlayerDashboardInventoryUI.headers[8].children[1].backgroundColor;
					PlayerDashboardInventoryUI.headers[8].children[2].backgroundColor = PlayerDashboardInventoryUI.headers[8].children[1].backgroundColor;
					PlayerDashboardInventoryUI.headers[8].children[2].foregroundColor = PlayerDashboardInventoryUI.headers[8].children[1].backgroundColor;
				}
				PlayerDashboardInventoryUI.headers[8].isVisible = (newMask != 0);
				PlayerDashboardInventoryUI.updateBoxAreas();
			}
		}

		private static void onGlassesIconReady(Texture2D texture)
		{
			((SleekImageTexture)PlayerDashboardInventoryUI.headers[9].children[0]).texture = texture;
		}

		private static void onGlassesUpdated(ushort newGlasses, byte newGlassesQuality, byte[] newGlassesState)
		{
			if (PlayerDashboardInventoryUI.headers != null)
			{
				ItemAsset itemAsset = (ItemAsset)Assets.find(EAssetType.ITEM, newGlasses);
				if (itemAsset != null)
				{
					PlayerDashboardInventoryUI.headers[9].text = itemAsset.itemName;
					PlayerDashboardInventoryUI.headers[9].children[0].sizeOffset_X = (int)(itemAsset.size_x * 25);
					PlayerDashboardInventoryUI.headers[9].children[0].sizeOffset_Y = (int)(itemAsset.size_y * 25);
					PlayerDashboardInventoryUI.headers[9].children[0].positionOffset_Y = -PlayerDashboardInventoryUI.headers[9].children[0].sizeOffset_Y / 2;
					if (PlayerDashboardInventoryUI.<>f__mg$cache15 == null)
					{
						PlayerDashboardInventoryUI.<>f__mg$cache15 = new ItemIconReady(PlayerDashboardInventoryUI.onGlassesIconReady);
					}
					ItemTool.getIcon(newGlasses, newGlassesQuality, newGlassesState, PlayerDashboardInventoryUI.<>f__mg$cache15);
					((SleekLabel)PlayerDashboardInventoryUI.headers[9].children[2]).text = newGlassesQuality + "%";
					PlayerDashboardInventoryUI.headers[9].backgroundColor = ItemTool.getRarityColorUI(itemAsset.rarity);
					PlayerDashboardInventoryUI.headers[9].foregroundColor = PlayerDashboardInventoryUI.headers[9].backgroundColor;
					PlayerDashboardInventoryUI.headers[9].children[1].backgroundColor = ItemTool.getQualityColor((float)newGlassesQuality / 100f);
					PlayerDashboardInventoryUI.headers[9].children[1].foregroundColor = PlayerDashboardInventoryUI.headers[9].children[1].backgroundColor;
					PlayerDashboardInventoryUI.headers[9].children[2].backgroundColor = PlayerDashboardInventoryUI.headers[9].children[1].backgroundColor;
					PlayerDashboardInventoryUI.headers[9].children[2].foregroundColor = PlayerDashboardInventoryUI.headers[9].children[1].backgroundColor;
				}
				PlayerDashboardInventoryUI.headers[9].isVisible = (newGlasses != 0);
				PlayerDashboardInventoryUI.updateBoxAreas();
			}
		}

		private static void onClickedHeader(SleekButton button)
		{
			int i;
			for (i = 0; i < PlayerDashboardInventoryUI.headers.Length; i++)
			{
				if (PlayerDashboardInventoryUI.headers[i] == button)
				{
					break;
				}
			}
			switch (i)
			{
			case 0:
				if (Player.player.equipment.isSelected && !Player.player.equipment.isBusy && Player.player.equipment.isEquipped)
				{
					Player.player.equipment.dequip();
				}
				return;
			case 1:
				Player.player.clothing.sendSwapBackpack(byte.MaxValue, byte.MaxValue, byte.MaxValue);
				return;
			case 2:
				Player.player.clothing.sendSwapVest(byte.MaxValue, byte.MaxValue, byte.MaxValue);
				return;
			case 3:
				Player.player.clothing.sendSwapShirt(byte.MaxValue, byte.MaxValue, byte.MaxValue);
				return;
			case 4:
				Player.player.clothing.sendSwapPants(byte.MaxValue, byte.MaxValue, byte.MaxValue);
				return;
			case 5:
				PlayerDashboardUI.close();
				PlayerLifeUI.open();
				return;
			case 6:
				PlayerDashboardUI.close();
				PlayerLifeUI.open();
				return;
			case 7:
				Player.player.clothing.sendSwapHat(byte.MaxValue, byte.MaxValue, byte.MaxValue);
				return;
			case 8:
				Player.player.clothing.sendSwapMask(byte.MaxValue, byte.MaxValue, byte.MaxValue);
				return;
			case 9:
				Player.player.clothing.sendSwapGlasses(byte.MaxValue, byte.MaxValue, byte.MaxValue);
				return;
			default:
				return;
			}
		}

		private static void updatePivot()
		{
			if (PlayerDashboardInventoryUI.dragJar.rot == 0)
			{
				PlayerDashboardInventoryUI.dragPivot.x = PlayerDashboardInventoryUI.dragOffset.x;
				PlayerDashboardInventoryUI.dragPivot.y = PlayerDashboardInventoryUI.dragOffset.y;
			}
			else if (PlayerDashboardInventoryUI.dragJar.rot == 1)
			{
				PlayerDashboardInventoryUI.dragPivot.x = -((float)(PlayerDashboardInventoryUI.dragJar.size_y * 50) + PlayerDashboardInventoryUI.dragOffset.y);
				PlayerDashboardInventoryUI.dragPivot.y = PlayerDashboardInventoryUI.dragOffset.x;
			}
			else if (PlayerDashboardInventoryUI.dragJar.rot == 2)
			{
				PlayerDashboardInventoryUI.dragPivot.x = -((float)(PlayerDashboardInventoryUI.dragJar.size_x * 50) + PlayerDashboardInventoryUI.dragOffset.x);
				PlayerDashboardInventoryUI.dragPivot.y = -((float)(PlayerDashboardInventoryUI.dragJar.size_y * 50) + PlayerDashboardInventoryUI.dragOffset.y);
			}
			else if (PlayerDashboardInventoryUI.dragJar.rot == 3)
			{
				PlayerDashboardInventoryUI.dragPivot.x = PlayerDashboardInventoryUI.dragOffset.y;
				PlayerDashboardInventoryUI.dragPivot.y = -((float)(PlayerDashboardInventoryUI.dragJar.size_x * 50) + PlayerDashboardInventoryUI.dragOffset.x);
			}
		}

		public static void update()
		{
			if (PlayerDashboardInventoryUI.dragPage == 255)
			{
				return;
			}
			if (Event.current.type == 4 && Event.current.keyCode == ControlsSettings.rotate)
			{
				ItemJar itemJar = PlayerDashboardInventoryUI.dragJar;
				itemJar.rot += 1;
				ItemJar itemJar2 = PlayerDashboardInventoryUI.dragJar;
				itemJar2.rot %= 4;
				PlayerDashboardInventoryUI.updatePivot();
				PlayerDashboardInventoryUI.dragItem.updateItem(PlayerDashboardInventoryUI.dragJar);
				PlayerDashboardInventoryUI.dragItem.positionOffset_X = (int)(PlayerUI.window.mouse_x + PlayerDashboardInventoryUI.dragPivot.x);
				PlayerDashboardInventoryUI.dragItem.positionOffset_Y = (int)(PlayerUI.window.mouse_y + PlayerDashboardInventoryUI.dragPivot.y);
			}
		}

		private static List<RegionCoordinate> regionsInRadius = new List<RegionCoordinate>(4);

		private static List<InteractableItem> itemsInRadius = new List<InteractableItem>();

		private static Sleek container;

		public static Local localization;

		public static Bundle icons;

		public static bool active;

		private static SleekBox backdropBox;

		private static bool isDragging;

		private static ItemJar dragJar;

		private static SleekItem dragSource;

		private static SleekItem dragItem;

		private static Vector2 dragOffset;

		private static Vector2 dragPivot;

		private static float lastDrag;

		private static byte dragPage;

		private static byte drag_x;

		private static byte drag_y;

		private static byte dragRot;

		private static SleekInspect character;

		private static SleekSlider characterSlider;

		private static SleekButtonIcon swapCosmeticsButton;

		private static SleekButtonIcon swapSkinsButton;

		private static SleekButtonIcon swapMythicsButton;

		private static SleekPlayer characterPlayer;

		private static SleekSlot[] slots;

		private static Sleek box;

		private static SleekScrollBox clothingBox;

		private static SleekScrollBox areaBox;

		private static SleekButton[] headers;

		private static SleekItems[] items;

		private static SleekBox selectionBackdropBox;

		private static SleekBox selectionIconBox;

		private static SleekImageTexture selectionIconImage;

		private static SleekBox selectionDescriptionBox;

		private static SleekLabel selectionDescriptionLabel;

		private static SleekLabel selectionNameLabel;

		private static SleekLabel selectionHotkeyLabel;

		private static SleekBox vehicleBox;

		private static SleekLabel vehicleNameLabel;

		private static Sleek vehicleActionsBox;

		private static Sleek vehiclePassengersBox;

		private static SleekButton vehicleLockButton;

		private static SleekButton vehicleHornButton;

		private static SleekButton vehicleHeadlightsButton;

		private static SleekButton vehicleSirensButton;

		private static SleekButton vehicleHookButton;

		private static SleekButton vehicleStealBatteryButton;

		private static SleekScrollBox selectionActionsBox;

		private static SleekButton selectionEquipButton;

		private static SleekButton selectionContextButton;

		private static SleekButton selectionDropButton;

		private static SleekButton selectionStorageButton;

		private static Sleek selectionExtraActionsBox;

		private static SleekButton rot_xButton;

		private static SleekButton rot_yButton;

		private static SleekButton rot_zButton;

		private static byte _selectedPage;

		private static byte _selected_x;

		private static byte _selected_y;

		private static ItemJar _selectedJar;

		private static ItemAsset _selectedAsset;

		private static Items areaItems;

		private static List<Action> actions;

		[CompilerGenerated]
		private static ItemIconReady <>f__mg$cache0;

		[CompilerGenerated]
		private static ItemIconReady <>f__mg$cache1;

		[CompilerGenerated]
		private static ItemIconReady <>f__mg$cache2;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache3;

		[CompilerGenerated]
		private static VehiclePassengersUpdated <>f__mg$cache4;

		[CompilerGenerated]
		private static VehicleLockUpdated <>f__mg$cache5;

		[CompilerGenerated]
		private static VehicleHeadlightsUpdated <>f__mg$cache6;

		[CompilerGenerated]
		private static VehicleSirensUpdated <>f__mg$cache7;

		[CompilerGenerated]
		private static VehicleBatteryChangedHandler <>f__mg$cache8;

		[CompilerGenerated]
		private static VehiclePassengersUpdated <>f__mg$cache9;

		[CompilerGenerated]
		private static VehicleLockUpdated <>f__mg$cacheA;

		[CompilerGenerated]
		private static VehicleHeadlightsUpdated <>f__mg$cacheB;

		[CompilerGenerated]
		private static VehicleSirensUpdated <>f__mg$cacheC;

		[CompilerGenerated]
		private static VehicleBatteryChangedHandler <>f__mg$cacheD;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cacheE;

		[CompilerGenerated]
		private static ItemIconReady <>f__mg$cacheF;

		[CompilerGenerated]
		private static ItemIconReady <>f__mg$cache10;

		[CompilerGenerated]
		private static ItemIconReady <>f__mg$cache11;

		[CompilerGenerated]
		private static ItemIconReady <>f__mg$cache12;

		[CompilerGenerated]
		private static ItemIconReady <>f__mg$cache13;

		[CompilerGenerated]
		private static ItemIconReady <>f__mg$cache14;

		[CompilerGenerated]
		private static ItemIconReady <>f__mg$cache15;

		[CompilerGenerated]
		private static SelectedItem <>f__mg$cache16;

		[CompilerGenerated]
		private static GrabbedItem <>f__mg$cache17;

		[CompilerGenerated]
		private static PlacedItem <>f__mg$cache18;

		[CompilerGenerated]
		private static Dragged <>f__mg$cache19;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache1A;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache1B;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache1C;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache1D;

		[CompilerGenerated]
		private static SelectedItem <>f__mg$cache1E;

		[CompilerGenerated]
		private static GrabbedItem <>f__mg$cache1F;

		[CompilerGenerated]
		private static PlacedItem <>f__mg$cache20;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache21;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache22;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache23;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache24;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache25;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache26;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache27;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache28;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache29;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache2A;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache2B;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache2C;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache2D;

		[CompilerGenerated]
		private static InventoryResized <>f__mg$cache2E;

		[CompilerGenerated]
		private static InventoryUpdated <>f__mg$cache2F;

		[CompilerGenerated]
		private static InventoryAdded <>f__mg$cache30;

		[CompilerGenerated]
		private static InventoryRemoved <>f__mg$cache31;

		[CompilerGenerated]
		private static InventoryStored <>f__mg$cache32;

		[CompilerGenerated]
		private static HotkeysUpdated <>f__mg$cache33;

		[CompilerGenerated]
		private static ItemDropAdded <>f__mg$cache34;

		[CompilerGenerated]
		private static ItemDropRemoved <>f__mg$cache35;

		[CompilerGenerated]
		private static Seated <>f__mg$cache36;

		[CompilerGenerated]
		private static ShirtUpdated <>f__mg$cache37;

		[CompilerGenerated]
		private static PantsUpdated <>f__mg$cache38;

		[CompilerGenerated]
		private static HatUpdated <>f__mg$cache39;

		[CompilerGenerated]
		private static BackpackUpdated <>f__mg$cache3A;

		[CompilerGenerated]
		private static VestUpdated <>f__mg$cache3B;

		[CompilerGenerated]
		private static MaskUpdated <>f__mg$cache3C;

		[CompilerGenerated]
		private static GlassesUpdated <>f__mg$cache3D;

		[CompilerGenerated]
		private static ClickedMouse <>f__mg$cache3E;

		[CompilerGenerated]
		private static MovedMouse <>f__mg$cache3F;
	}
}
