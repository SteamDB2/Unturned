using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace SDG.Unturned
{
	public class MenuSurvivorsClothingItemUI
	{
		public MenuSurvivorsClothingItemUI()
		{
			MenuSurvivorsClothingItemUI.localization = Localization.read("/Menu/Survivors/MenuSurvivorsClothingItem.dat");
			MenuSurvivorsClothingItemUI.container = new Sleek();
			MenuSurvivorsClothingItemUI.container.positionOffset_X = 10;
			MenuSurvivorsClothingItemUI.container.positionOffset_Y = 10;
			MenuSurvivorsClothingItemUI.container.positionScale_Y = 1f;
			MenuSurvivorsClothingItemUI.container.sizeOffset_X = -20;
			MenuSurvivorsClothingItemUI.container.sizeOffset_Y = -20;
			MenuSurvivorsClothingItemUI.container.sizeScale_X = 1f;
			MenuSurvivorsClothingItemUI.container.sizeScale_Y = 1f;
			MenuUI.container.add(MenuSurvivorsClothingItemUI.container);
			MenuSurvivorsClothingItemUI.active = false;
			MenuSurvivorsClothingItemUI.inventory = new Sleek();
			MenuSurvivorsClothingItemUI.inventory.positionScale_X = 0.5f;
			MenuSurvivorsClothingItemUI.inventory.positionOffset_Y = 10;
			MenuSurvivorsClothingItemUI.inventory.sizeScale_X = 0.5f;
			MenuSurvivorsClothingItemUI.inventory.sizeScale_Y = 1f;
			MenuSurvivorsClothingItemUI.inventory.sizeOffset_Y = -20;
			MenuSurvivorsClothingItemUI.inventory.constraint = ESleekConstraint.XY;
			MenuSurvivorsClothingItemUI.container.add(MenuSurvivorsClothingItemUI.inventory);
			MenuSurvivorsClothingItemUI.packageBox = new SleekInventory();
			MenuSurvivorsClothingItemUI.packageBox.sizeScale_X = 1f;
			MenuSurvivorsClothingItemUI.packageBox.sizeScale_Y = 0.5f;
			MenuSurvivorsClothingItemUI.packageBox.sizeOffset_Y = -5;
			MenuSurvivorsClothingItemUI.packageBox.constraint = ESleekConstraint.XY;
			MenuSurvivorsClothingItemUI.inventory.add(MenuSurvivorsClothingItemUI.packageBox);
			MenuSurvivorsClothingItemUI.descriptionBox = new SleekBox();
			MenuSurvivorsClothingItemUI.descriptionBox.positionOffset_Y = 10;
			MenuSurvivorsClothingItemUI.descriptionBox.positionScale_Y = 1f;
			MenuSurvivorsClothingItemUI.descriptionBox.sizeScale_X = 1f;
			MenuSurvivorsClothingItemUI.descriptionBox.sizeScale_Y = 1f;
			MenuSurvivorsClothingItemUI.descriptionBox.foregroundTint = ESleekTint.NONE;
			MenuSurvivorsClothingItemUI.packageBox.add(MenuSurvivorsClothingItemUI.descriptionBox);
			MenuSurvivorsClothingItemUI.infoLabel = new SleekLabel();
			MenuSurvivorsClothingItemUI.infoLabel.isRich = true;
			MenuSurvivorsClothingItemUI.infoLabel.positionOffset_X = 5;
			MenuSurvivorsClothingItemUI.infoLabel.positionOffset_Y = 5;
			MenuSurvivorsClothingItemUI.infoLabel.sizeScale_X = 1f;
			MenuSurvivorsClothingItemUI.infoLabel.sizeScale_Y = 1f;
			MenuSurvivorsClothingItemUI.infoLabel.sizeOffset_X = -10;
			MenuSurvivorsClothingItemUI.infoLabel.sizeOffset_Y = -10;
			MenuSurvivorsClothingItemUI.infoLabel.fontAlignment = 0;
			MenuSurvivorsClothingItemUI.infoLabel.foregroundTint = ESleekTint.NONE;
			MenuSurvivorsClothingItemUI.descriptionBox.add(MenuSurvivorsClothingItemUI.infoLabel);
			MenuSurvivorsClothingItemUI.useButton = new SleekButton();
			MenuSurvivorsClothingItemUI.useButton.positionScale_Y = 1f;
			MenuSurvivorsClothingItemUI.useButton.sizeOffset_X = -5;
			MenuSurvivorsClothingItemUI.useButton.sizeOffset_Y = 50;
			MenuSurvivorsClothingItemUI.useButton.sizeScale_X = 0.5f;
			SleekButton sleekButton = MenuSurvivorsClothingItemUI.useButton;
			if (MenuSurvivorsClothingItemUI.<>f__mg$cache0 == null)
			{
				MenuSurvivorsClothingItemUI.<>f__mg$cache0 = new ClickedButton(MenuSurvivorsClothingItemUI.onClickedUseButton);
			}
			sleekButton.onClickedButton = MenuSurvivorsClothingItemUI.<>f__mg$cache0;
			MenuSurvivorsClothingItemUI.descriptionBox.add(MenuSurvivorsClothingItemUI.useButton);
			MenuSurvivorsClothingItemUI.useButton.fontSize = 14;
			MenuSurvivorsClothingItemUI.useButton.isVisible = false;
			MenuSurvivorsClothingItemUI.inspectButton = new SleekButton();
			MenuSurvivorsClothingItemUI.inspectButton.positionOffset_X = 5;
			MenuSurvivorsClothingItemUI.inspectButton.positionScale_X = 0.5f;
			MenuSurvivorsClothingItemUI.inspectButton.positionScale_Y = 1f;
			MenuSurvivorsClothingItemUI.inspectButton.sizeOffset_X = -5;
			MenuSurvivorsClothingItemUI.inspectButton.sizeOffset_Y = 50;
			MenuSurvivorsClothingItemUI.inspectButton.sizeScale_X = 0.5f;
			MenuSurvivorsClothingItemUI.inspectButton.text = MenuSurvivorsClothingItemUI.localization.format("Inspect_Text");
			MenuSurvivorsClothingItemUI.inspectButton.tooltip = MenuSurvivorsClothingItemUI.localization.format("Inspect_Tooltip");
			SleekButton sleekButton2 = MenuSurvivorsClothingItemUI.inspectButton;
			if (MenuSurvivorsClothingItemUI.<>f__mg$cache1 == null)
			{
				MenuSurvivorsClothingItemUI.<>f__mg$cache1 = new ClickedButton(MenuSurvivorsClothingItemUI.onClickedInspectButton);
			}
			sleekButton2.onClickedButton = MenuSurvivorsClothingItemUI.<>f__mg$cache1;
			MenuSurvivorsClothingItemUI.descriptionBox.add(MenuSurvivorsClothingItemUI.inspectButton);
			MenuSurvivorsClothingItemUI.inspectButton.fontSize = 14;
			MenuSurvivorsClothingItemUI.inspectButton.isVisible = false;
			MenuSurvivorsClothingItemUI.marketButton = new SleekButton();
			MenuSurvivorsClothingItemUI.marketButton.positionScale_Y = 1f;
			MenuSurvivorsClothingItemUI.marketButton.sizeOffset_X = -5;
			MenuSurvivorsClothingItemUI.marketButton.sizeOffset_Y = 50;
			MenuSurvivorsClothingItemUI.marketButton.sizeScale_X = 0.5f;
			MenuSurvivorsClothingItemUI.marketButton.text = MenuSurvivorsClothingItemUI.localization.format("Market_Text");
			MenuSurvivorsClothingItemUI.marketButton.tooltip = MenuSurvivorsClothingItemUI.localization.format("Market_Tooltip");
			SleekButton sleekButton3 = MenuSurvivorsClothingItemUI.marketButton;
			if (MenuSurvivorsClothingItemUI.<>f__mg$cache2 == null)
			{
				MenuSurvivorsClothingItemUI.<>f__mg$cache2 = new ClickedButton(MenuSurvivorsClothingItemUI.onClickedMarketButton);
			}
			sleekButton3.onClickedButton = MenuSurvivorsClothingItemUI.<>f__mg$cache2;
			MenuSurvivorsClothingItemUI.descriptionBox.add(MenuSurvivorsClothingItemUI.marketButton);
			MenuSurvivorsClothingItemUI.marketButton.fontSize = 14;
			MenuSurvivorsClothingItemUI.marketButton.isVisible = false;
			MenuSurvivorsClothingItemUI.deleteButton = new SleekButton();
			MenuSurvivorsClothingItemUI.deleteButton.positionOffset_X = 5;
			MenuSurvivorsClothingItemUI.deleteButton.positionScale_X = 0.5f;
			MenuSurvivorsClothingItemUI.deleteButton.positionScale_Y = 1f;
			MenuSurvivorsClothingItemUI.deleteButton.sizeOffset_Y = 50;
			MenuSurvivorsClothingItemUI.deleteButton.sizeScale_X = 0.5f;
			MenuSurvivorsClothingItemUI.deleteButton.text = MenuSurvivorsClothingItemUI.localization.format("Delete_Text");
			MenuSurvivorsClothingItemUI.deleteButton.tooltip = MenuSurvivorsClothingItemUI.localization.format("Delete_Tooltip");
			SleekButton sleekButton4 = MenuSurvivorsClothingItemUI.deleteButton;
			if (MenuSurvivorsClothingItemUI.<>f__mg$cache3 == null)
			{
				MenuSurvivorsClothingItemUI.<>f__mg$cache3 = new ClickedButton(MenuSurvivorsClothingItemUI.onClickedDeleteButton);
			}
			sleekButton4.onClickedButton = MenuSurvivorsClothingItemUI.<>f__mg$cache3;
			MenuSurvivorsClothingItemUI.descriptionBox.add(MenuSurvivorsClothingItemUI.deleteButton);
			MenuSurvivorsClothingItemUI.deleteButton.fontSize = 14;
			MenuSurvivorsClothingItemUI.backButton = new SleekButtonIcon((Texture2D)MenuDashboardUI.icons.load("Exit"));
			MenuSurvivorsClothingItemUI.backButton.positionOffset_Y = -50;
			MenuSurvivorsClothingItemUI.backButton.positionScale_Y = 1f;
			MenuSurvivorsClothingItemUI.backButton.sizeOffset_X = 200;
			MenuSurvivorsClothingItemUI.backButton.sizeOffset_Y = 50;
			MenuSurvivorsClothingItemUI.backButton.text = MenuDashboardUI.localization.format("BackButtonText");
			MenuSurvivorsClothingItemUI.backButton.tooltip = MenuDashboardUI.localization.format("BackButtonTooltip");
			SleekButton sleekButton5 = MenuSurvivorsClothingItemUI.backButton;
			if (MenuSurvivorsClothingItemUI.<>f__mg$cache4 == null)
			{
				MenuSurvivorsClothingItemUI.<>f__mg$cache4 = new ClickedButton(MenuSurvivorsClothingItemUI.onClickedBackButton);
			}
			sleekButton5.onClickedButton = MenuSurvivorsClothingItemUI.<>f__mg$cache4;
			MenuSurvivorsClothingItemUI.backButton.fontSize = 14;
			MenuSurvivorsClothingItemUI.backButton.iconImage.backgroundTint = ESleekTint.FOREGROUND;
			MenuSurvivorsClothingItemUI.container.add(MenuSurvivorsClothingItemUI.backButton);
		}

		public static void open()
		{
			if (MenuSurvivorsClothingItemUI.active)
			{
				return;
			}
			MenuSurvivorsClothingItemUI.active = true;
			MenuSurvivorsClothingItemUI.container.lerpPositionScale(0f, 0f, ESleekLerp.EXPONENTIAL, 20f);
		}

		public static void close()
		{
			if (!MenuSurvivorsClothingItemUI.active)
			{
				return;
			}
			MenuSurvivorsClothingItemUI.active = false;
			MenuSurvivorsClothingItemUI.container.lerpPositionScale(0f, 1f, ESleekLerp.EXPONENTIAL, 20f);
		}

		public static void viewItem()
		{
			MenuSurvivorsClothingItemUI.viewItem(MenuSurvivorsClothingItemUI.item, MenuSurvivorsClothingItemUI.quantity, MenuSurvivorsClothingItemUI.instance);
		}

		public static void viewItem(int newItem, ushort newQuantity, ulong newInstance)
		{
			MenuSurvivorsClothingItemUI.item = newItem;
			MenuSurvivorsClothingItemUI.quantity = newQuantity;
			MenuSurvivorsClothingItemUI.instance = newInstance;
			MenuSurvivorsClothingItemUI.packageBox.updateInventory(MenuSurvivorsClothingItemUI.instance, MenuSurvivorsClothingItemUI.item, newQuantity, false, true);
			if (MenuSurvivorsClothingItemUI.packageBox.itemAsset != null)
			{
				if (MenuSurvivorsClothingItemUI.packageBox.itemAsset.type == EItemType.KEY)
				{
					MenuSurvivorsClothingItemUI.useButton.isVisible = false;
					MenuSurvivorsClothingItemUI.inspectButton.isVisible = false;
				}
				else if (MenuSurvivorsClothingItemUI.packageBox.itemAsset.type == EItemType.BOX)
				{
					MenuSurvivorsClothingItemUI.useButton.isVisible = true;
					MenuSurvivorsClothingItemUI.inspectButton.isVisible = false;
					MenuSurvivorsClothingItemUI.useButton.text = MenuSurvivorsClothingItemUI.localization.format("Contents_Text");
					MenuSurvivorsClothingItemUI.useButton.tooltip = MenuSurvivorsClothingItemUI.localization.format("Contents_Tooltip");
				}
				else
				{
					MenuSurvivorsClothingItemUI.useButton.isVisible = true;
					MenuSurvivorsClothingItemUI.inspectButton.isVisible = true;
					bool flag;
					if (MenuSurvivorsClothingItemUI.packageBox.itemAsset.proPath == null || MenuSurvivorsClothingItemUI.packageBox.itemAsset.proPath.Length == 0)
					{
						flag = Characters.isSkinEquipped(MenuSurvivorsClothingItemUI.instance);
					}
					else
					{
						flag = Characters.isCosmeticEquipped(MenuSurvivorsClothingItemUI.instance);
					}
					MenuSurvivorsClothingItemUI.useButton.text = MenuSurvivorsClothingItemUI.localization.format((!flag) ? "Equip_Text" : "Dequip_Text");
					MenuSurvivorsClothingItemUI.useButton.tooltip = MenuSurvivorsClothingItemUI.localization.format((!flag) ? "Equip_Tooltip" : "Dequip_Tooltip");
				}
				MenuSurvivorsClothingItemUI.marketButton.isVisible = Provider.provider.economyService.getInventoryMarketable(MenuSurvivorsClothingItemUI.item);
				MenuSurvivorsClothingItemUI.descriptionBox.sizeOffset_Y = 0;
				if (MenuSurvivorsClothingItemUI.useButton.isVisible || MenuSurvivorsClothingItemUI.inspectButton.isVisible)
				{
					MenuSurvivorsClothingItemUI.descriptionBox.sizeOffset_Y -= 60;
					MenuSurvivorsClothingItemUI.useButton.positionOffset_Y = -MenuSurvivorsClothingItemUI.descriptionBox.sizeOffset_Y - 50;
					MenuSurvivorsClothingItemUI.inspectButton.positionOffset_Y = -MenuSurvivorsClothingItemUI.descriptionBox.sizeOffset_Y - 50;
				}
				if (MenuSurvivorsClothingItemUI.marketButton.isVisible || MenuSurvivorsClothingItemUI.deleteButton.isVisible)
				{
					MenuSurvivorsClothingItemUI.descriptionBox.sizeOffset_Y -= 60;
					MenuSurvivorsClothingItemUI.marketButton.positionOffset_Y = -MenuSurvivorsClothingItemUI.descriptionBox.sizeOffset_Y - 50;
					MenuSurvivorsClothingItemUI.deleteButton.positionOffset_Y = -MenuSurvivorsClothingItemUI.descriptionBox.sizeOffset_Y - 50;
				}
				MenuSurvivorsClothingItemUI.infoLabel.text = string.Concat(new string[]
				{
					"<color=",
					Palette.hex(Provider.provider.economyService.getInventoryColor(MenuSurvivorsClothingItemUI.item)),
					">",
					Provider.provider.economyService.getInventoryType(MenuSurvivorsClothingItemUI.item),
					"</color>\n\n",
					Provider.provider.economyService.getInventoryDescription(MenuSurvivorsClothingItemUI.item)
				});
			}
			else
			{
				MenuSurvivorsClothingItemUI.useButton.isVisible = false;
				MenuSurvivorsClothingItemUI.inspectButton.isVisible = false;
				MenuSurvivorsClothingItemUI.marketButton.isVisible = false;
				MenuSurvivorsClothingItemUI.deleteButton.isVisible = true;
				MenuSurvivorsClothingItemUI.descriptionBox.sizeOffset_Y = -60;
				MenuSurvivorsClothingItemUI.deleteButton.positionOffset_Y = -MenuSurvivorsClothingItemUI.descriptionBox.sizeOffset_Y - 50;
				MenuSurvivorsClothingItemUI.infoLabel.text = MenuSurvivorsClothingItemUI.localization.format("Unknown");
			}
		}

		private static void onClickedUseButton(SleekButton button)
		{
			if (MenuSurvivorsClothingItemUI.packageBox.itemAsset == null)
			{
				return;
			}
			if (MenuSurvivorsClothingItemUI.packageBox.itemAsset.type == EItemType.BOX)
			{
				MenuSurvivorsClothingBoxUI.viewItem(MenuSurvivorsClothingItemUI.item, MenuSurvivorsClothingItemUI.quantity, MenuSurvivorsClothingItemUI.instance);
				MenuSurvivorsClothingBoxUI.open();
				MenuSurvivorsClothingItemUI.close();
			}
			else
			{
				Characters.package(MenuSurvivorsClothingItemUI.instance);
				MenuSurvivorsClothingItemUI.viewItem();
			}
		}

		private static void onClickedInspectButton(SleekButton button)
		{
			MenuSurvivorsClothingInspectUI.viewItem(MenuSurvivorsClothingItemUI.item, MenuSurvivorsClothingItemUI.instance);
			MenuSurvivorsClothingInspectUI.open();
			MenuSurvivorsClothingItemUI.close();
		}

		private static void onClickedMarketButton(SleekButton button)
		{
			if (!Provider.provider.economyService.canOpenInventory)
			{
				MenuUI.alert(MenuSurvivorsClothingItemUI.localization.format("Overlay"));
				return;
			}
			Provider.provider.economyService.open(MenuSurvivorsClothingItemUI.instance);
		}

		private static void onClickedDeleteButton(SleekButton button)
		{
			MenuSurvivorsClothingDeleteUI.viewItem(MenuSurvivorsClothingItemUI.item, MenuSurvivorsClothingItemUI.instance);
			MenuSurvivorsClothingDeleteUI.open();
			MenuSurvivorsClothingItemUI.close();
		}

		private static void onClickedBackButton(SleekButton button)
		{
			MenuSurvivorsClothingUI.open();
			MenuSurvivorsClothingItemUI.close();
		}

		private static Local localization;

		private static Sleek container;

		public static bool active;

		private static SleekButtonIcon backButton;

		private static int item;

		private static ushort quantity;

		private static ulong instance;

		private static Sleek inventory;

		private static SleekInventory packageBox;

		private static SleekBox descriptionBox;

		private static SleekLabel infoLabel;

		private static SleekButton useButton;

		private static SleekButton inspectButton;

		private static SleekButton marketButton;

		private static SleekButton deleteButton;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache0;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache1;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache2;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache3;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache4;
	}
}
