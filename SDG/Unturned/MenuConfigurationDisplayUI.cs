using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace SDG.Unturned
{
	public class MenuConfigurationDisplayUI
	{
		public MenuConfigurationDisplayUI()
		{
			MenuConfigurationDisplayUI.localization = Localization.read("/Menu/Configuration/MenuConfigurationDisplay.dat");
			MenuConfigurationDisplayUI.container = new Sleek();
			MenuConfigurationDisplayUI.container.positionOffset_X = 10;
			MenuConfigurationDisplayUI.container.positionOffset_Y = 10;
			MenuConfigurationDisplayUI.container.positionScale_Y = 1f;
			MenuConfigurationDisplayUI.container.sizeOffset_X = -20;
			MenuConfigurationDisplayUI.container.sizeOffset_Y = -20;
			MenuConfigurationDisplayUI.container.sizeScale_X = 1f;
			MenuConfigurationDisplayUI.container.sizeScale_Y = 1f;
			MenuConfigurationDisplayUI.resolutions = new List<Resolution>();
			byte b = 0;
			while ((int)b < Screen.resolutions.Length)
			{
				Resolution item = Screen.resolutions[(int)b];
				if (item.width >= 640 && item.height >= 480)
				{
					MenuConfigurationDisplayUI.resolutions.Add(item);
				}
				b += 1;
			}
			if (Provider.isConnected)
			{
				PlayerUI.container.add(MenuConfigurationDisplayUI.container);
			}
			else
			{
				MenuUI.container.add(MenuConfigurationDisplayUI.container);
			}
			MenuConfigurationDisplayUI.active = false;
			MenuConfigurationDisplayUI.resolutionsBox = new SleekScrollBox();
			MenuConfigurationDisplayUI.resolutionsBox.positionOffset_X = -200;
			MenuConfigurationDisplayUI.resolutionsBox.positionOffset_Y = 100;
			MenuConfigurationDisplayUI.resolutionsBox.positionScale_X = 0.5f;
			MenuConfigurationDisplayUI.resolutionsBox.sizeOffset_X = 430;
			MenuConfigurationDisplayUI.resolutionsBox.sizeOffset_Y = -200;
			MenuConfigurationDisplayUI.resolutionsBox.sizeScale_Y = 1f;
			MenuConfigurationDisplayUI.resolutionsBox.area = new Rect(0f, 0f, 5f, (float)(100 + MenuConfigurationDisplayUI.resolutions.Count * 40 - 10));
			MenuConfigurationDisplayUI.container.add(MenuConfigurationDisplayUI.resolutionsBox);
			MenuConfigurationDisplayUI.buttons = new SleekButton[MenuConfigurationDisplayUI.resolutions.Count];
			byte b2 = 0;
			while ((int)b2 < MenuConfigurationDisplayUI.buttons.Length)
			{
				Resolution resolution = MenuConfigurationDisplayUI.resolutions[(int)b2];
				SleekButton sleekButton = new SleekButton();
				sleekButton.positionOffset_Y = (int)(100 + b2 * 40);
				sleekButton.sizeOffset_X = -30;
				sleekButton.sizeOffset_Y = 30;
				sleekButton.sizeScale_X = 1f;
				SleekButton sleekButton2 = sleekButton;
				if (MenuConfigurationDisplayUI.<>f__mg$cache0 == null)
				{
					MenuConfigurationDisplayUI.<>f__mg$cache0 = new ClickedButton(MenuConfigurationDisplayUI.onClickedResolutionButton);
				}
				sleekButton2.onClickedButton = MenuConfigurationDisplayUI.<>f__mg$cache0;
				sleekButton.text = string.Concat(new object[]
				{
					resolution.width,
					" x ",
					resolution.height,
					" [",
					resolution.refreshRate,
					"Hz]"
				});
				MenuConfigurationDisplayUI.resolutionsBox.add(sleekButton);
				MenuConfigurationDisplayUI.buttons[(int)b2] = sleekButton;
				b2 += 1;
			}
			MenuConfigurationDisplayUI.fullscreenToggle = new SleekToggle();
			MenuConfigurationDisplayUI.fullscreenToggle.sizeOffset_X = 40;
			MenuConfigurationDisplayUI.fullscreenToggle.sizeOffset_Y = 40;
			MenuConfigurationDisplayUI.fullscreenToggle.addLabel(MenuConfigurationDisplayUI.localization.format("Fullscreen_Toggle_Label"), ESleekSide.RIGHT);
			MenuConfigurationDisplayUI.fullscreenToggle.state = GraphicsSettings.fullscreen;
			SleekToggle sleekToggle = MenuConfigurationDisplayUI.fullscreenToggle;
			if (MenuConfigurationDisplayUI.<>f__mg$cache1 == null)
			{
				MenuConfigurationDisplayUI.<>f__mg$cache1 = new Toggled(MenuConfigurationDisplayUI.onToggledFullscreenToggle);
			}
			sleekToggle.onToggled = MenuConfigurationDisplayUI.<>f__mg$cache1;
			MenuConfigurationDisplayUI.resolutionsBox.add(MenuConfigurationDisplayUI.fullscreenToggle);
			MenuConfigurationDisplayUI.bufferToggle = new SleekToggle();
			MenuConfigurationDisplayUI.bufferToggle.positionOffset_Y = 50;
			MenuConfigurationDisplayUI.bufferToggle.sizeOffset_X = 40;
			MenuConfigurationDisplayUI.bufferToggle.sizeOffset_Y = 40;
			MenuConfigurationDisplayUI.bufferToggle.addLabel(MenuConfigurationDisplayUI.localization.format("Buffer_Toggle_Label"), ESleekSide.RIGHT);
			MenuConfigurationDisplayUI.bufferToggle.state = GraphicsSettings.buffer;
			SleekToggle sleekToggle2 = MenuConfigurationDisplayUI.bufferToggle;
			if (MenuConfigurationDisplayUI.<>f__mg$cache2 == null)
			{
				MenuConfigurationDisplayUI.<>f__mg$cache2 = new Toggled(MenuConfigurationDisplayUI.onToggledBufferToggle);
			}
			sleekToggle2.onToggled = MenuConfigurationDisplayUI.<>f__mg$cache2;
			MenuConfigurationDisplayUI.resolutionsBox.add(MenuConfigurationDisplayUI.bufferToggle);
			MenuConfigurationDisplayUI.backButton = new SleekButtonIcon((Texture2D)MenuDashboardUI.icons.load("Exit"));
			MenuConfigurationDisplayUI.backButton.positionOffset_Y = -50;
			MenuConfigurationDisplayUI.backButton.positionScale_Y = 1f;
			MenuConfigurationDisplayUI.backButton.sizeOffset_X = 200;
			MenuConfigurationDisplayUI.backButton.sizeOffset_Y = 50;
			MenuConfigurationDisplayUI.backButton.text = MenuDashboardUI.localization.format("BackButtonText");
			MenuConfigurationDisplayUI.backButton.tooltip = MenuDashboardUI.localization.format("BackButtonTooltip");
			SleekButton sleekButton3 = MenuConfigurationDisplayUI.backButton;
			if (MenuConfigurationDisplayUI.<>f__mg$cache3 == null)
			{
				MenuConfigurationDisplayUI.<>f__mg$cache3 = new ClickedButton(MenuConfigurationDisplayUI.onClickedBackButton);
			}
			sleekButton3.onClickedButton = MenuConfigurationDisplayUI.<>f__mg$cache3;
			MenuConfigurationDisplayUI.backButton.fontSize = 14;
			MenuConfigurationDisplayUI.backButton.iconImage.backgroundTint = ESleekTint.FOREGROUND;
			MenuConfigurationDisplayUI.container.add(MenuConfigurationDisplayUI.backButton);
		}

		public static void open()
		{
			if (MenuConfigurationDisplayUI.active)
			{
				return;
			}
			MenuConfigurationDisplayUI.active = true;
			MenuConfigurationDisplayUI.container.lerpPositionScale(0f, 0f, ESleekLerp.EXPONENTIAL, 20f);
		}

		public static void close()
		{
			if (!MenuConfigurationDisplayUI.active)
			{
				return;
			}
			MenuConfigurationDisplayUI.active = false;
			MenuConfigurationDisplayUI.container.lerpPositionScale(0f, 1f, ESleekLerp.EXPONENTIAL, 20f);
		}

		private static void onClickedResolutionButton(SleekButton button)
		{
			int index = (button.positionOffset_Y - 100) / 40;
			GraphicsSettings.resolution = new GraphicsSettingsResolution(MenuConfigurationDisplayUI.resolutions[index]);
			GraphicsSettings.apply();
		}

		private static void onToggledFullscreenToggle(SleekToggle toggle, bool state)
		{
			GraphicsSettings.fullscreen = state;
			GraphicsSettings.apply();
		}

		private static void onToggledBufferToggle(SleekToggle toggle, bool state)
		{
			GraphicsSettings.buffer = state;
			GraphicsSettings.apply();
		}

		private static void onClickedBackButton(SleekButton button)
		{
			if (Player.player != null)
			{
				PlayerPauseUI.open();
			}
			else
			{
				MenuConfigurationUI.open();
			}
			MenuConfigurationDisplayUI.close();
		}

		private static Local localization;

		private static Sleek container;

		public static bool active;

		private static SleekButtonIcon backButton;

		private static SleekScrollBox resolutionsBox;

		private static SleekButton[] buttons;

		private static SleekToggle fullscreenToggle;

		private static SleekToggle bufferToggle;

		private static List<Resolution> resolutions;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache0;

		[CompilerGenerated]
		private static Toggled <>f__mg$cache1;

		[CompilerGenerated]
		private static Toggled <>f__mg$cache2;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache3;
	}
}
