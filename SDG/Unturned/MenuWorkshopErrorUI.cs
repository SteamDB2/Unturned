using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace SDG.Unturned
{
	public class MenuWorkshopErrorUI
	{
		public MenuWorkshopErrorUI()
		{
			MenuWorkshopErrorUI.localization = Localization.read("/Menu/Workshop/MenuWorkshopError.dat");
			MenuWorkshopErrorUI.container = new Sleek();
			MenuWorkshopErrorUI.container.positionOffset_X = 10;
			MenuWorkshopErrorUI.container.positionOffset_Y = 10;
			MenuWorkshopErrorUI.container.positionScale_Y = 1f;
			MenuWorkshopErrorUI.container.sizeOffset_X = -20;
			MenuWorkshopErrorUI.container.sizeOffset_Y = -20;
			MenuWorkshopErrorUI.container.sizeScale_X = 1f;
			MenuWorkshopErrorUI.container.sizeScale_Y = 1f;
			MenuUI.container.add(MenuWorkshopErrorUI.container);
			MenuWorkshopErrorUI.active = false;
			MenuWorkshopErrorUI.headerBox = new SleekBox();
			MenuWorkshopErrorUI.headerBox.sizeOffset_Y = 50;
			MenuWorkshopErrorUI.headerBox.sizeScale_X = 1f;
			MenuWorkshopErrorUI.headerBox.fontSize = 14;
			MenuWorkshopErrorUI.headerBox.text = MenuWorkshopErrorUI.localization.format("Header");
			MenuWorkshopErrorUI.container.add(MenuWorkshopErrorUI.headerBox);
			MenuWorkshopErrorUI.infoBox = new SleekBox();
			MenuWorkshopErrorUI.infoBox.positionOffset_Y = 60;
			MenuWorkshopErrorUI.infoBox.sizeOffset_X = -30;
			MenuWorkshopErrorUI.infoBox.sizeOffset_Y = 50;
			MenuWorkshopErrorUI.infoBox.sizeScale_X = 1f;
			MenuWorkshopErrorUI.infoBox.fontSize = 14;
			MenuWorkshopErrorUI.infoBox.text = MenuWorkshopErrorUI.localization.format("No_Errors");
			MenuWorkshopErrorUI.container.add(MenuWorkshopErrorUI.infoBox);
			MenuWorkshopErrorUI.infoBox.isVisible = false;
			MenuWorkshopErrorUI.errorBox = new SleekScrollBox();
			MenuWorkshopErrorUI.errorBox.positionOffset_Y = 60;
			MenuWorkshopErrorUI.errorBox.sizeOffset_Y = -120;
			MenuWorkshopErrorUI.errorBox.sizeScale_X = 1f;
			MenuWorkshopErrorUI.errorBox.sizeScale_Y = 1f;
			MenuWorkshopErrorUI.errorBox.area = new Rect(0f, 0f, 5f, 0f);
			MenuWorkshopErrorUI.container.add(MenuWorkshopErrorUI.errorBox);
			MenuWorkshopErrorUI.backButton = new SleekButtonIcon((Texture2D)MenuDashboardUI.icons.load("Exit"));
			MenuWorkshopErrorUI.backButton.positionOffset_Y = -50;
			MenuWorkshopErrorUI.backButton.positionScale_Y = 1f;
			MenuWorkshopErrorUI.backButton.sizeOffset_X = 200;
			MenuWorkshopErrorUI.backButton.sizeOffset_Y = 50;
			MenuWorkshopErrorUI.backButton.text = MenuDashboardUI.localization.format("BackButtonText");
			MenuWorkshopErrorUI.backButton.tooltip = MenuDashboardUI.localization.format("BackButtonTooltip");
			SleekButton sleekButton = MenuWorkshopErrorUI.backButton;
			if (MenuWorkshopErrorUI.<>f__mg$cache0 == null)
			{
				MenuWorkshopErrorUI.<>f__mg$cache0 = new ClickedButton(MenuWorkshopErrorUI.onClickedBackButton);
			}
			sleekButton.onClickedButton = MenuWorkshopErrorUI.<>f__mg$cache0;
			MenuWorkshopErrorUI.backButton.fontSize = 14;
			MenuWorkshopErrorUI.backButton.iconImage.backgroundTint = ESleekTint.FOREGROUND;
			MenuWorkshopErrorUI.container.add(MenuWorkshopErrorUI.backButton);
			MenuWorkshopErrorUI.refreshButton = new SleekButton();
			MenuWorkshopErrorUI.refreshButton.positionOffset_X = -200;
			MenuWorkshopErrorUI.refreshButton.positionOffset_Y = -50;
			MenuWorkshopErrorUI.refreshButton.positionScale_X = 1f;
			MenuWorkshopErrorUI.refreshButton.positionScale_Y = 1f;
			MenuWorkshopErrorUI.refreshButton.sizeOffset_X = 200;
			MenuWorkshopErrorUI.refreshButton.sizeOffset_Y = 50;
			MenuWorkshopErrorUI.refreshButton.text = MenuWorkshopErrorUI.localization.format("Refresh");
			MenuWorkshopErrorUI.refreshButton.tooltip = MenuWorkshopErrorUI.localization.format("Refresh_Tooltip");
			SleekButton sleekButton2 = MenuWorkshopErrorUI.refreshButton;
			if (MenuWorkshopErrorUI.<>f__mg$cache1 == null)
			{
				MenuWorkshopErrorUI.<>f__mg$cache1 = new ClickedButton(MenuWorkshopErrorUI.onClickedRefreshButton);
			}
			sleekButton2.onClickedButton = MenuWorkshopErrorUI.<>f__mg$cache1;
			MenuWorkshopErrorUI.refreshButton.fontSize = 14;
			MenuWorkshopErrorUI.container.add(MenuWorkshopErrorUI.refreshButton);
		}

		public static void open()
		{
			if (MenuWorkshopErrorUI.active)
			{
				return;
			}
			MenuWorkshopErrorUI.active = true;
			MenuWorkshopErrorUI.refresh();
			MenuWorkshopErrorUI.container.lerpPositionScale(0f, 0f, ESleekLerp.EXPONENTIAL, 20f);
		}

		public static void close()
		{
			if (!MenuWorkshopErrorUI.active)
			{
				return;
			}
			MenuWorkshopErrorUI.active = false;
			MenuWorkshopErrorUI.container.lerpPositionScale(0f, 1f, ESleekLerp.EXPONENTIAL, 20f);
		}

		private static void refresh()
		{
			MenuWorkshopErrorUI.errorBox.remove();
			for (int i = 0; i < Assets.errors.Count; i++)
			{
				SleekBox sleekBox = new SleekBox();
				sleekBox.positionOffset_Y = i * 60;
				sleekBox.sizeOffset_X = -30;
				sleekBox.sizeOffset_Y = 50;
				sleekBox.sizeScale_X = 1f;
				sleekBox.text = Assets.errors[i];
				MenuWorkshopErrorUI.errorBox.add(sleekBox);
			}
			MenuWorkshopErrorUI.errorBox.area = new Rect(0f, 0f, 5f, (float)(Assets.errors.Count * 60 - 10));
			MenuWorkshopErrorUI.infoBox.isVisible = (Assets.errors.Count == 0);
		}

		private static void onClickedBackButton(SleekButton button)
		{
			MenuWorkshopUI.open();
			MenuWorkshopErrorUI.close();
		}

		private static void onClickedRefreshButton(SleekButton button)
		{
			MenuWorkshopErrorUI.refresh();
		}

		private static Local localization;

		private static Sleek container;

		public static bool active;

		private static SleekButtonIcon backButton;

		private static SleekButton refreshButton;

		private static SleekBox headerBox;

		private static SleekBox infoBox;

		private static SleekScrollBox errorBox;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache0;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache1;
	}
}
