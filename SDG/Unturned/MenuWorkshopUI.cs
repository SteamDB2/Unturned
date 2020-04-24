using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace SDG.Unturned
{
	public class MenuWorkshopUI
	{
		public MenuWorkshopUI()
		{
			MenuWorkshopUI.localization = Localization.read("/Menu/Workshop/MenuWorkshop.dat");
			Bundle bundle = Bundles.getBundle("/Bundles/Textures/Menu/Icons/Workshop/MenuWorkshop/MenuWorkshop.unity3d");
			MenuWorkshopUI.container = new Sleek();
			MenuWorkshopUI.container.positionOffset_X = 10;
			MenuWorkshopUI.container.positionOffset_Y = 10;
			MenuWorkshopUI.container.positionScale_Y = -1f;
			MenuWorkshopUI.container.sizeOffset_X = -20;
			MenuWorkshopUI.container.sizeOffset_Y = -20;
			MenuWorkshopUI.container.sizeScale_X = 1f;
			MenuWorkshopUI.container.sizeScale_Y = 1f;
			MenuUI.container.add(MenuWorkshopUI.container);
			MenuWorkshopUI.active = false;
			MenuWorkshopUI.browseButton = new SleekButtonIcon((Texture2D)bundle.load("Browse"));
			MenuWorkshopUI.browseButton.positionOffset_X = -100;
			MenuWorkshopUI.browseButton.positionOffset_Y = -235;
			MenuWorkshopUI.browseButton.positionScale_X = 0.5f;
			MenuWorkshopUI.browseButton.positionScale_Y = 0.5f;
			MenuWorkshopUI.browseButton.sizeOffset_X = 200;
			MenuWorkshopUI.browseButton.sizeOffset_Y = 50;
			MenuWorkshopUI.browseButton.text = MenuWorkshopUI.localization.format("BrowseButtonText");
			MenuWorkshopUI.browseButton.tooltip = MenuWorkshopUI.localization.format("BrowseButtonTooltip");
			SleekButton sleekButton = MenuWorkshopUI.browseButton;
			if (MenuWorkshopUI.<>f__mg$cache0 == null)
			{
				MenuWorkshopUI.<>f__mg$cache0 = new ClickedButton(MenuWorkshopUI.onClickedBrowseButton);
			}
			sleekButton.onClickedButton = MenuWorkshopUI.<>f__mg$cache0;
			MenuWorkshopUI.browseButton.fontSize = 14;
			MenuWorkshopUI.browseButton.iconImage.backgroundTint = ESleekTint.FOREGROUND;
			MenuWorkshopUI.container.add(MenuWorkshopUI.browseButton);
			MenuWorkshopUI.submitButton = new SleekButtonIcon((Texture2D)bundle.load("Submit"));
			MenuWorkshopUI.submitButton.positionOffset_X = -100;
			MenuWorkshopUI.submitButton.positionOffset_Y = -175;
			MenuWorkshopUI.submitButton.positionScale_X = 0.5f;
			MenuWorkshopUI.submitButton.positionScale_Y = 0.5f;
			MenuWorkshopUI.submitButton.sizeOffset_X = 200;
			MenuWorkshopUI.submitButton.sizeOffset_Y = 50;
			MenuWorkshopUI.submitButton.text = MenuWorkshopUI.localization.format("SubmitButtonText");
			MenuWorkshopUI.submitButton.tooltip = MenuWorkshopUI.localization.format("SubmitButtonTooltip");
			SleekButton sleekButton2 = MenuWorkshopUI.submitButton;
			if (MenuWorkshopUI.<>f__mg$cache1 == null)
			{
				MenuWorkshopUI.<>f__mg$cache1 = new ClickedButton(MenuWorkshopUI.onClickedSubmitButton);
			}
			sleekButton2.onClickedButton = MenuWorkshopUI.<>f__mg$cache1;
			MenuWorkshopUI.submitButton.fontSize = 14;
			MenuWorkshopUI.submitButton.iconImage.backgroundTint = ESleekTint.FOREGROUND;
			MenuWorkshopUI.container.add(MenuWorkshopUI.submitButton);
			MenuWorkshopUI.editorButton = new SleekButtonIcon((Texture2D)bundle.load("Editor"));
			MenuWorkshopUI.editorButton.positionOffset_X = -100;
			MenuWorkshopUI.editorButton.positionOffset_Y = -115;
			MenuWorkshopUI.editorButton.positionScale_X = 0.5f;
			MenuWorkshopUI.editorButton.positionScale_Y = 0.5f;
			MenuWorkshopUI.editorButton.sizeOffset_X = 200;
			MenuWorkshopUI.editorButton.sizeOffset_Y = 50;
			MenuWorkshopUI.editorButton.text = MenuWorkshopUI.localization.format("EditorButtonText");
			MenuWorkshopUI.editorButton.tooltip = MenuWorkshopUI.localization.format("EditorButtonTooltip");
			SleekButton sleekButton3 = MenuWorkshopUI.editorButton;
			if (MenuWorkshopUI.<>f__mg$cache2 == null)
			{
				MenuWorkshopUI.<>f__mg$cache2 = new ClickedButton(MenuWorkshopUI.onClickedEditorButton);
			}
			sleekButton3.onClickedButton = MenuWorkshopUI.<>f__mg$cache2;
			MenuWorkshopUI.editorButton.fontSize = 14;
			MenuWorkshopUI.editorButton.iconImage.backgroundTint = ESleekTint.FOREGROUND;
			MenuWorkshopUI.container.add(MenuWorkshopUI.editorButton);
			MenuWorkshopUI.errorButton = new SleekButtonIcon((Texture2D)bundle.load("Error"));
			MenuWorkshopUI.errorButton.positionOffset_X = -100;
			MenuWorkshopUI.errorButton.positionOffset_Y = -55;
			MenuWorkshopUI.errorButton.positionScale_X = 0.5f;
			MenuWorkshopUI.errorButton.positionScale_Y = 0.5f;
			MenuWorkshopUI.errorButton.sizeOffset_X = 200;
			MenuWorkshopUI.errorButton.sizeOffset_Y = 50;
			MenuWorkshopUI.errorButton.text = MenuWorkshopUI.localization.format("ErrorButtonText");
			MenuWorkshopUI.errorButton.tooltip = MenuWorkshopUI.localization.format("ErrorButtonTooltip");
			SleekButton sleekButton4 = MenuWorkshopUI.errorButton;
			if (MenuWorkshopUI.<>f__mg$cache3 == null)
			{
				MenuWorkshopUI.<>f__mg$cache3 = new ClickedButton(MenuWorkshopUI.onClickedErrorButton);
			}
			sleekButton4.onClickedButton = MenuWorkshopUI.<>f__mg$cache3;
			MenuWorkshopUI.errorButton.fontSize = 14;
			MenuWorkshopUI.errorButton.iconImage.backgroundTint = ESleekTint.FOREGROUND;
			MenuWorkshopUI.container.add(MenuWorkshopUI.errorButton);
			MenuWorkshopUI.localizationButton = new SleekButtonIcon((Texture2D)bundle.load("Localization"));
			MenuWorkshopUI.localizationButton.positionOffset_X = -100;
			MenuWorkshopUI.localizationButton.positionOffset_Y = 5;
			MenuWorkshopUI.localizationButton.positionScale_X = 0.5f;
			MenuWorkshopUI.localizationButton.positionScale_Y = 0.5f;
			MenuWorkshopUI.localizationButton.sizeOffset_X = 200;
			MenuWorkshopUI.localizationButton.sizeOffset_Y = 50;
			MenuWorkshopUI.localizationButton.text = MenuWorkshopUI.localization.format("LocalizationButtonText");
			MenuWorkshopUI.localizationButton.tooltip = MenuWorkshopUI.localization.format("LocalizationButtonTooltip");
			SleekButton sleekButton5 = MenuWorkshopUI.localizationButton;
			if (MenuWorkshopUI.<>f__mg$cache4 == null)
			{
				MenuWorkshopUI.<>f__mg$cache4 = new ClickedButton(MenuWorkshopUI.onClickedLocalizationButton);
			}
			sleekButton5.onClickedButton = MenuWorkshopUI.<>f__mg$cache4;
			MenuWorkshopUI.localizationButton.fontSize = 14;
			MenuWorkshopUI.localizationButton.iconImage.backgroundTint = ESleekTint.FOREGROUND;
			MenuWorkshopUI.container.add(MenuWorkshopUI.localizationButton);
			MenuWorkshopUI.spawnsButton = new SleekButtonIcon((Texture2D)bundle.load("Spawns"));
			MenuWorkshopUI.spawnsButton.positionOffset_X = -100;
			MenuWorkshopUI.spawnsButton.positionOffset_Y = 65;
			MenuWorkshopUI.spawnsButton.positionScale_X = 0.5f;
			MenuWorkshopUI.spawnsButton.positionScale_Y = 0.5f;
			MenuWorkshopUI.spawnsButton.sizeOffset_X = 200;
			MenuWorkshopUI.spawnsButton.sizeOffset_Y = 50;
			MenuWorkshopUI.spawnsButton.text = MenuWorkshopUI.localization.format("SpawnsButtonText");
			MenuWorkshopUI.spawnsButton.tooltip = MenuWorkshopUI.localization.format("SpawnsButtonTooltip");
			SleekButton sleekButton6 = MenuWorkshopUI.spawnsButton;
			if (MenuWorkshopUI.<>f__mg$cache5 == null)
			{
				MenuWorkshopUI.<>f__mg$cache5 = new ClickedButton(MenuWorkshopUI.onClickedSpawnsButton);
			}
			sleekButton6.onClickedButton = MenuWorkshopUI.<>f__mg$cache5;
			MenuWorkshopUI.spawnsButton.fontSize = 14;
			MenuWorkshopUI.spawnsButton.iconImage.backgroundTint = ESleekTint.FOREGROUND;
			MenuWorkshopUI.container.add(MenuWorkshopUI.spawnsButton);
			MenuWorkshopUI.modulesButton = new SleekButtonIcon((Texture2D)bundle.load("Modules"));
			MenuWorkshopUI.modulesButton.positionOffset_X = -100;
			MenuWorkshopUI.modulesButton.positionOffset_Y = 125;
			MenuWorkshopUI.modulesButton.positionScale_X = 0.5f;
			MenuWorkshopUI.modulesButton.positionScale_Y = 0.5f;
			MenuWorkshopUI.modulesButton.sizeOffset_X = 200;
			MenuWorkshopUI.modulesButton.sizeOffset_Y = 50;
			MenuWorkshopUI.modulesButton.text = MenuWorkshopUI.localization.format("ModulesButtonText");
			MenuWorkshopUI.modulesButton.tooltip = MenuWorkshopUI.localization.format("ModulesButtonTooltip");
			SleekButton sleekButton7 = MenuWorkshopUI.modulesButton;
			if (MenuWorkshopUI.<>f__mg$cache6 == null)
			{
				MenuWorkshopUI.<>f__mg$cache6 = new ClickedButton(MenuWorkshopUI.onClickedModulesButton);
			}
			sleekButton7.onClickedButton = MenuWorkshopUI.<>f__mg$cache6;
			MenuWorkshopUI.modulesButton.fontSize = 14;
			MenuWorkshopUI.modulesButton.iconImage.backgroundTint = ESleekTint.FOREGROUND;
			MenuWorkshopUI.container.add(MenuWorkshopUI.modulesButton);
			MenuWorkshopUI.backButton = new SleekButtonIcon((Texture2D)MenuDashboardUI.icons.load("Exit"));
			MenuWorkshopUI.backButton.positionOffset_X = -100;
			MenuWorkshopUI.backButton.positionOffset_Y = 185;
			MenuWorkshopUI.backButton.positionScale_X = 0.5f;
			MenuWorkshopUI.backButton.positionScale_Y = 0.5f;
			MenuWorkshopUI.backButton.sizeOffset_X = 200;
			MenuWorkshopUI.backButton.sizeOffset_Y = 50;
			MenuWorkshopUI.backButton.text = MenuDashboardUI.localization.format("BackButtonText");
			MenuWorkshopUI.backButton.tooltip = MenuDashboardUI.localization.format("BackButtonTooltip");
			SleekButton sleekButton8 = MenuWorkshopUI.backButton;
			if (MenuWorkshopUI.<>f__mg$cache7 == null)
			{
				MenuWorkshopUI.<>f__mg$cache7 = new ClickedButton(MenuWorkshopUI.onClickedBackButton);
			}
			sleekButton8.onClickedButton = MenuWorkshopUI.<>f__mg$cache7;
			MenuWorkshopUI.backButton.fontSize = 14;
			MenuWorkshopUI.backButton.iconImage.backgroundTint = ESleekTint.FOREGROUND;
			MenuWorkshopUI.container.add(MenuWorkshopUI.backButton);
			bundle.unload();
			new MenuWorkshopSubmitUI();
			new MenuWorkshopEditorUI();
			new MenuWorkshopErrorUI();
			new MenuWorkshopLocalizationUI();
			new MenuWorkshopSpawnsUI();
			new MenuWorkshopModulesUI();
		}

		public static void open()
		{
			if (MenuWorkshopUI.active)
			{
				return;
			}
			MenuWorkshopUI.active = true;
			MenuWorkshopUI.container.lerpPositionScale(0f, 0f, ESleekLerp.EXPONENTIAL, 20f);
		}

		public static void close()
		{
			if (!MenuWorkshopUI.active)
			{
				return;
			}
			MenuWorkshopUI.active = false;
			MenuWorkshopUI.container.lerpPositionScale(0f, -1f, ESleekLerp.EXPONENTIAL, 20f);
		}

		private static void onClickedBrowseButton(SleekButton button)
		{
			if (!Provider.provider.browserService.canOpenBrowser)
			{
				MenuUI.alert(MenuWorkshopUI.localization.format("Overlay"));
				return;
			}
			Provider.provider.browserService.open("http://steamcommunity.com/app/304930/workshop/");
		}

		private static void onClickedSubmitButton(SleekButton button)
		{
			MenuWorkshopSubmitUI.open();
			MenuWorkshopUI.close();
		}

		private static void onClickedEditorButton(SleekButton button)
		{
			MenuWorkshopEditorUI.open();
			MenuWorkshopUI.close();
		}

		private static void onClickedErrorButton(SleekButton button)
		{
			MenuWorkshopErrorUI.open();
			MenuWorkshopUI.close();
		}

		private static void onClickedLocalizationButton(SleekButton button)
		{
			MenuWorkshopLocalizationUI.open();
			MenuWorkshopUI.close();
		}

		private static void onClickedSpawnsButton(SleekButton button)
		{
			MenuWorkshopSpawnsUI.open();
			MenuWorkshopUI.close();
		}

		private static void onClickedModulesButton(SleekButton button)
		{
			MenuWorkshopModulesUI.open();
			MenuWorkshopUI.close();
		}

		private static void onClickedBackButton(SleekButton button)
		{
			MenuDashboardUI.open();
			MenuTitleUI.open();
			MenuWorkshopUI.close();
		}

		private static Sleek container;

		private static Local localization;

		public static bool active;

		private static SleekButtonIcon browseButton;

		private static SleekButtonIcon submitButton;

		private static SleekButtonIcon editorButton;

		private static SleekButtonIcon errorButton;

		private static SleekButtonIcon localizationButton;

		private static SleekButtonIcon spawnsButton;

		private static SleekButtonIcon modulesButton;

		private static SleekButtonIcon backButton;

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

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache5;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache6;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache7;
	}
}
