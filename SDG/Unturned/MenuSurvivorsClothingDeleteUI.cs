using System;
using System.Runtime.CompilerServices;

namespace SDG.Unturned
{
	public class MenuSurvivorsClothingDeleteUI
	{
		public MenuSurvivorsClothingDeleteUI()
		{
			MenuSurvivorsClothingDeleteUI.localization = Localization.read("/Menu/Survivors/MenuSurvivorsClothingDelete.dat");
			MenuSurvivorsClothingDeleteUI.container = new Sleek();
			MenuSurvivorsClothingDeleteUI.container.positionOffset_X = 10;
			MenuSurvivorsClothingDeleteUI.container.positionOffset_Y = 10;
			MenuSurvivorsClothingDeleteUI.container.positionScale_Y = 1f;
			MenuSurvivorsClothingDeleteUI.container.sizeOffset_X = -20;
			MenuSurvivorsClothingDeleteUI.container.sizeOffset_Y = -20;
			MenuSurvivorsClothingDeleteUI.container.sizeScale_X = 1f;
			MenuSurvivorsClothingDeleteUI.container.sizeScale_Y = 1f;
			MenuUI.container.add(MenuSurvivorsClothingDeleteUI.container);
			MenuSurvivorsClothingDeleteUI.active = false;
			MenuSurvivorsClothingDeleteUI.inventory = new Sleek();
			MenuSurvivorsClothingDeleteUI.inventory.positionScale_X = 0.5f;
			MenuSurvivorsClothingDeleteUI.inventory.positionOffset_Y = 10;
			MenuSurvivorsClothingDeleteUI.inventory.sizeScale_X = 0.5f;
			MenuSurvivorsClothingDeleteUI.inventory.sizeScale_Y = 1f;
			MenuSurvivorsClothingDeleteUI.inventory.sizeOffset_Y = -20;
			MenuSurvivorsClothingDeleteUI.inventory.constraint = ESleekConstraint.XY;
			MenuSurvivorsClothingDeleteUI.container.add(MenuSurvivorsClothingDeleteUI.inventory);
			MenuSurvivorsClothingDeleteUI.deleteBox = new SleekBox();
			MenuSurvivorsClothingDeleteUI.deleteBox.positionOffset_Y = -65;
			MenuSurvivorsClothingDeleteUI.deleteBox.positionScale_Y = 0.5f;
			MenuSurvivorsClothingDeleteUI.deleteBox.sizeOffset_Y = 130;
			MenuSurvivorsClothingDeleteUI.deleteBox.sizeScale_X = 1f;
			MenuSurvivorsClothingDeleteUI.inventory.add(MenuSurvivorsClothingDeleteUI.deleteBox);
			MenuSurvivorsClothingDeleteUI.intentLabel = new SleekLabel();
			MenuSurvivorsClothingDeleteUI.intentLabel.isRich = true;
			MenuSurvivorsClothingDeleteUI.intentLabel.positionOffset_X = 5;
			MenuSurvivorsClothingDeleteUI.intentLabel.positionOffset_Y = 5;
			MenuSurvivorsClothingDeleteUI.intentLabel.sizeOffset_X = -10;
			MenuSurvivorsClothingDeleteUI.intentLabel.sizeOffset_Y = 20;
			MenuSurvivorsClothingDeleteUI.intentLabel.sizeScale_X = 1f;
			MenuSurvivorsClothingDeleteUI.intentLabel.foregroundTint = ESleekTint.NONE;
			MenuSurvivorsClothingDeleteUI.deleteBox.add(MenuSurvivorsClothingDeleteUI.intentLabel);
			MenuSurvivorsClothingDeleteUI.warningLabel = new SleekLabel();
			MenuSurvivorsClothingDeleteUI.warningLabel.positionOffset_X = 5;
			MenuSurvivorsClothingDeleteUI.warningLabel.positionOffset_Y = 25;
			MenuSurvivorsClothingDeleteUI.warningLabel.sizeOffset_X = -10;
			MenuSurvivorsClothingDeleteUI.warningLabel.sizeOffset_Y = 20;
			MenuSurvivorsClothingDeleteUI.warningLabel.sizeScale_X = 1f;
			MenuSurvivorsClothingDeleteUI.warningLabel.text = MenuSurvivorsClothingDeleteUI.localization.format("Warning");
			MenuSurvivorsClothingDeleteUI.warningLabel.foregroundTint = ESleekTint.NONE;
			MenuSurvivorsClothingDeleteUI.warningLabel.foregroundColor = Palette.COLOR_O;
			MenuSurvivorsClothingDeleteUI.deleteBox.add(MenuSurvivorsClothingDeleteUI.warningLabel);
			MenuSurvivorsClothingDeleteUI.confirmLabel = new SleekLabel();
			MenuSurvivorsClothingDeleteUI.confirmLabel.positionOffset_X = 5;
			MenuSurvivorsClothingDeleteUI.confirmLabel.positionOffset_Y = 45;
			MenuSurvivorsClothingDeleteUI.confirmLabel.sizeOffset_X = -10;
			MenuSurvivorsClothingDeleteUI.confirmLabel.sizeOffset_Y = 20;
			MenuSurvivorsClothingDeleteUI.confirmLabel.sizeScale_X = 1f;
			MenuSurvivorsClothingDeleteUI.confirmLabel.text = MenuSurvivorsClothingDeleteUI.localization.format("Confirm", new object[]
			{
				MenuSurvivorsClothingDeleteUI.localization.format("Delete")
			});
			MenuSurvivorsClothingDeleteUI.confirmLabel.foregroundTint = ESleekTint.NONE;
			MenuSurvivorsClothingDeleteUI.deleteBox.add(MenuSurvivorsClothingDeleteUI.confirmLabel);
			MenuSurvivorsClothingDeleteUI.confirmField = new SleekField();
			MenuSurvivorsClothingDeleteUI.confirmField.positionOffset_X = 5;
			MenuSurvivorsClothingDeleteUI.confirmField.positionOffset_Y = 75;
			MenuSurvivorsClothingDeleteUI.confirmField.sizeOffset_X = -150;
			MenuSurvivorsClothingDeleteUI.confirmField.sizeOffset_Y = 50;
			MenuSurvivorsClothingDeleteUI.confirmField.sizeScale_X = 1f;
			MenuSurvivorsClothingDeleteUI.confirmField.fontSize = 14;
			MenuSurvivorsClothingDeleteUI.confirmField.backgroundTint = ESleekTint.NONE;
			MenuSurvivorsClothingDeleteUI.confirmField.foregroundTint = ESleekTint.NONE;
			MenuSurvivorsClothingDeleteUI.confirmField.hint = MenuSurvivorsClothingDeleteUI.localization.format("Delete");
			MenuSurvivorsClothingDeleteUI.deleteBox.add(MenuSurvivorsClothingDeleteUI.confirmField);
			MenuSurvivorsClothingDeleteUI.yesButton = new SleekButton();
			MenuSurvivorsClothingDeleteUI.yesButton.positionOffset_X = -135;
			MenuSurvivorsClothingDeleteUI.yesButton.positionOffset_Y = 75;
			MenuSurvivorsClothingDeleteUI.yesButton.positionScale_X = 1f;
			MenuSurvivorsClothingDeleteUI.yesButton.sizeOffset_X = 60;
			MenuSurvivorsClothingDeleteUI.yesButton.sizeOffset_Y = 50;
			MenuSurvivorsClothingDeleteUI.yesButton.fontSize = 14;
			MenuSurvivorsClothingDeleteUI.yesButton.backgroundTint = ESleekTint.NONE;
			MenuSurvivorsClothingDeleteUI.yesButton.foregroundTint = ESleekTint.NONE;
			MenuSurvivorsClothingDeleteUI.yesButton.text = MenuSurvivorsClothingDeleteUI.localization.format("Yes");
			MenuSurvivorsClothingDeleteUI.yesButton.tooltip = MenuSurvivorsClothingDeleteUI.localization.format("Yes_Tooltip");
			SleekButton sleekButton = MenuSurvivorsClothingDeleteUI.yesButton;
			if (MenuSurvivorsClothingDeleteUI.<>f__mg$cache0 == null)
			{
				MenuSurvivorsClothingDeleteUI.<>f__mg$cache0 = new ClickedButton(MenuSurvivorsClothingDeleteUI.onClickedYesButton);
			}
			sleekButton.onClickedButton = MenuSurvivorsClothingDeleteUI.<>f__mg$cache0;
			MenuSurvivorsClothingDeleteUI.deleteBox.add(MenuSurvivorsClothingDeleteUI.yesButton);
			MenuSurvivorsClothingDeleteUI.noButton = new SleekButton();
			MenuSurvivorsClothingDeleteUI.noButton.positionOffset_X = -65;
			MenuSurvivorsClothingDeleteUI.noButton.positionOffset_Y = 75;
			MenuSurvivorsClothingDeleteUI.noButton.positionScale_X = 1f;
			MenuSurvivorsClothingDeleteUI.noButton.sizeOffset_X = 60;
			MenuSurvivorsClothingDeleteUI.noButton.sizeOffset_Y = 50;
			MenuSurvivorsClothingDeleteUI.noButton.fontSize = 14;
			MenuSurvivorsClothingDeleteUI.noButton.backgroundTint = ESleekTint.NONE;
			MenuSurvivorsClothingDeleteUI.noButton.foregroundTint = ESleekTint.NONE;
			MenuSurvivorsClothingDeleteUI.noButton.text = MenuSurvivorsClothingDeleteUI.localization.format("No");
			MenuSurvivorsClothingDeleteUI.noButton.tooltip = MenuSurvivorsClothingDeleteUI.localization.format("No_Tooltip");
			SleekButton sleekButton2 = MenuSurvivorsClothingDeleteUI.noButton;
			if (MenuSurvivorsClothingDeleteUI.<>f__mg$cache1 == null)
			{
				MenuSurvivorsClothingDeleteUI.<>f__mg$cache1 = new ClickedButton(MenuSurvivorsClothingDeleteUI.onClickedNoButton);
			}
			sleekButton2.onClickedButton = MenuSurvivorsClothingDeleteUI.<>f__mg$cache1;
			MenuSurvivorsClothingDeleteUI.deleteBox.add(MenuSurvivorsClothingDeleteUI.noButton);
		}

		public static void open()
		{
			if (MenuSurvivorsClothingDeleteUI.active)
			{
				return;
			}
			MenuSurvivorsClothingDeleteUI.active = true;
			MenuSurvivorsClothingDeleteUI.container.lerpPositionScale(0f, 0f, ESleekLerp.EXPONENTIAL, 20f);
		}

		public static void close()
		{
			if (!MenuSurvivorsClothingDeleteUI.active)
			{
				return;
			}
			MenuSurvivorsClothingDeleteUI.active = false;
			MenuSurvivorsClothingDeleteUI.container.lerpPositionScale(0f, 1f, ESleekLerp.EXPONENTIAL, 20f);
		}

		public static void viewItem(int newItem, ulong newInstance)
		{
			MenuSurvivorsClothingDeleteUI.item = newItem;
			MenuSurvivorsClothingDeleteUI.instance = newInstance;
			MenuSurvivorsClothingDeleteUI.intentLabel.text = MenuSurvivorsClothingDeleteUI.localization.format("Intent", new object[]
			{
				string.Concat(new string[]
				{
					"<color=",
					Palette.hex(Provider.provider.economyService.getInventoryColor(MenuSurvivorsClothingDeleteUI.item)),
					">",
					Provider.provider.economyService.getInventoryName(MenuSurvivorsClothingDeleteUI.item),
					"</color>"
				})
			});
			MenuSurvivorsClothingDeleteUI.confirmField.text = string.Empty;
		}

		private static void onClickedYesButton(SleekButton button)
		{
			if (MenuSurvivorsClothingDeleteUI.confirmField.text != MenuSurvivorsClothingDeleteUI.localization.format("Delete"))
			{
				return;
			}
			Provider.provider.economyService.consumeItem(MenuSurvivorsClothingDeleteUI.instance);
			Provider.provider.economyService.refreshInventory();
			MenuSurvivorsClothingUI.open();
			MenuSurvivorsClothingDeleteUI.close();
		}

		private static void onClickedNoButton(SleekButton button)
		{
			MenuSurvivorsClothingItemUI.open();
			MenuSurvivorsClothingDeleteUI.close();
		}

		private static Local localization;

		private static Sleek container;

		public static bool active;

		private static int item;

		private static ulong instance;

		private static Sleek inventory;

		private static SleekBox deleteBox;

		private static SleekLabel intentLabel;

		private static SleekLabel warningLabel;

		private static SleekLabel confirmLabel;

		private static SleekField confirmField;

		private static SleekButton yesButton;

		private static SleekButton noButton;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache0;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache1;
	}
}
