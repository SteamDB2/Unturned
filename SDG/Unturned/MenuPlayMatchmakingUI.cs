using System;
using System.Runtime.CompilerServices;
using SDG.Provider;
using UnityEngine;

namespace SDG.Unturned
{
	public class MenuPlayMatchmakingUI
	{
		public MenuPlayMatchmakingUI()
		{
			MenuPlayMatchmakingUI.localization = Localization.read("/Menu/Play/MenuPlayMatchmaking.dat");
			Bundle bundle = Bundles.getBundle("/Bundles/Textures/Menu/Icons/Play/MenuPlayMatchmaking/MenuPlayMatchmaking.unity3d");
			MenuPlayMatchmakingUI.container = new Sleek();
			MenuPlayMatchmakingUI.container.positionOffset_X = 10;
			MenuPlayMatchmakingUI.container.positionOffset_Y = 10;
			MenuPlayMatchmakingUI.container.positionScale_Y = 1f;
			MenuPlayMatchmakingUI.container.sizeOffset_X = -20;
			MenuPlayMatchmakingUI.container.sizeOffset_Y = -20;
			MenuPlayMatchmakingUI.container.sizeScale_X = 1f;
			MenuPlayMatchmakingUI.container.sizeScale_Y = 1f;
			MenuUI.container.add(MenuPlayMatchmakingUI.container);
			MenuPlayMatchmakingUI.active = false;
			MenuPlayMatchmakingUI.levelScrollBox = new SleekScrollBox();
			MenuPlayMatchmakingUI.levelScrollBox.positionOffset_X = -200;
			MenuPlayMatchmakingUI.levelScrollBox.positionOffset_Y = 280;
			MenuPlayMatchmakingUI.levelScrollBox.positionScale_X = 0.5f;
			MenuPlayMatchmakingUI.levelScrollBox.sizeOffset_X = 430;
			MenuPlayMatchmakingUI.levelScrollBox.sizeOffset_Y = -380;
			MenuPlayMatchmakingUI.levelScrollBox.sizeScale_Y = 1f;
			MenuPlayMatchmakingUI.levelScrollBox.area = new Rect(0f, 0f, 5f, 0f);
			MenuPlayMatchmakingUI.container.add(MenuPlayMatchmakingUI.levelScrollBox);
			MenuPlayMatchmakingUI.searchButton = new SleekButtonIcon((Texture2D)bundle.load("Search"));
			MenuPlayMatchmakingUI.searchButton.positionOffset_X = -200;
			MenuPlayMatchmakingUI.searchButton.positionOffset_Y = 100;
			MenuPlayMatchmakingUI.searchButton.positionScale_X = 0.5f;
			MenuPlayMatchmakingUI.searchButton.sizeOffset_X = 400;
			MenuPlayMatchmakingUI.searchButton.sizeOffset_Y = 30;
			MenuPlayMatchmakingUI.searchButton.text = MenuPlayMatchmakingUI.localization.format("Search_Button");
			MenuPlayMatchmakingUI.searchButton.tooltip = MenuPlayMatchmakingUI.localization.format("Search_Button_Tooltip");
			MenuPlayMatchmakingUI.searchButton.iconImage.backgroundTint = ESleekTint.FOREGROUND;
			SleekButton sleekButton = MenuPlayMatchmakingUI.searchButton;
			if (MenuPlayMatchmakingUI.<>f__mg$cache1 == null)
			{
				MenuPlayMatchmakingUI.<>f__mg$cache1 = new ClickedButton(MenuPlayMatchmakingUI.onClickedSearchButton);
			}
			sleekButton.onClickedButton = MenuPlayMatchmakingUI.<>f__mg$cache1;
			MenuPlayMatchmakingUI.container.add(MenuPlayMatchmakingUI.searchButton);
			MenuPlayMatchmakingUI.selectedBox = new SleekBox();
			MenuPlayMatchmakingUI.selectedBox.positionOffset_X = -200;
			MenuPlayMatchmakingUI.selectedBox.positionOffset_Y = 250;
			MenuPlayMatchmakingUI.selectedBox.positionScale_X = 0.5f;
			MenuPlayMatchmakingUI.selectedBox.sizeOffset_X = 400;
			MenuPlayMatchmakingUI.selectedBox.sizeOffset_Y = 30;
			MenuPlayMatchmakingUI.container.add(MenuPlayMatchmakingUI.selectedBox);
			MenuPlayMatchmakingUI.progressBox = new SleekBox();
			MenuPlayMatchmakingUI.progressBox.positionOffset_X = -200;
			MenuPlayMatchmakingUI.progressBox.positionOffset_Y = 130;
			MenuPlayMatchmakingUI.progressBox.positionScale_X = 0.5f;
			MenuPlayMatchmakingUI.progressBox.sizeOffset_X = 400;
			MenuPlayMatchmakingUI.progressBox.sizeOffset_Y = 50;
			MenuPlayMatchmakingUI.container.add(MenuPlayMatchmakingUI.progressBox);
			MenuPlayMatchmakingUI.infoButton = new SleekButtonIcon((Texture2D)bundle.load("Info"));
			MenuPlayMatchmakingUI.infoButton.positionOffset_X = -200;
			MenuPlayMatchmakingUI.infoButton.positionOffset_Y = 180;
			MenuPlayMatchmakingUI.infoButton.positionScale_X = 0.5f;
			MenuPlayMatchmakingUI.infoButton.sizeOffset_X = 195;
			MenuPlayMatchmakingUI.infoButton.sizeOffset_Y = 30;
			MenuPlayMatchmakingUI.infoButton.text = MenuPlayMatchmakingUI.localization.format("Info_Button");
			MenuPlayMatchmakingUI.infoButton.tooltip = MenuPlayMatchmakingUI.localization.format("Info_Button_Tooltip");
			MenuPlayMatchmakingUI.infoButton.iconImage.backgroundTint = ESleekTint.FOREGROUND;
			SleekButton sleekButton2 = MenuPlayMatchmakingUI.infoButton;
			if (MenuPlayMatchmakingUI.<>f__mg$cache2 == null)
			{
				MenuPlayMatchmakingUI.<>f__mg$cache2 = new ClickedButton(MenuPlayMatchmakingUI.onClickedInfoButton);
			}
			sleekButton2.onClickedButton = MenuPlayMatchmakingUI.<>f__mg$cache2;
			MenuPlayMatchmakingUI.container.add(MenuPlayMatchmakingUI.infoButton);
			MenuPlayMatchmakingUI.luckyButton = new SleekButtonIcon((Texture2D)bundle.load("Lucky"));
			MenuPlayMatchmakingUI.luckyButton.positionOffset_X = 5;
			MenuPlayMatchmakingUI.luckyButton.positionOffset_Y = 180;
			MenuPlayMatchmakingUI.luckyButton.positionScale_X = 0.5f;
			MenuPlayMatchmakingUI.luckyButton.sizeOffset_X = 195;
			MenuPlayMatchmakingUI.luckyButton.sizeOffset_Y = 30;
			MenuPlayMatchmakingUI.luckyButton.text = MenuPlayMatchmakingUI.localization.format("Lucky_Button");
			MenuPlayMatchmakingUI.luckyButton.tooltip = MenuPlayMatchmakingUI.localization.format("Lucky_Button_Tooltip");
			MenuPlayMatchmakingUI.luckyButton.iconImage.backgroundTint = ESleekTint.FOREGROUND;
			SleekButton sleekButton3 = MenuPlayMatchmakingUI.luckyButton;
			if (MenuPlayMatchmakingUI.<>f__mg$cache3 == null)
			{
				MenuPlayMatchmakingUI.<>f__mg$cache3 = new ClickedButton(MenuPlayMatchmakingUI.onClickedLuckyButton);
			}
			sleekButton3.onClickedButton = MenuPlayMatchmakingUI.<>f__mg$cache3;
			MenuPlayMatchmakingUI.container.add(MenuPlayMatchmakingUI.luckyButton);
			MenuPlayMatchmakingUI.updateProgressDisplay();
			MenuPlayMatchmakingUI.updateMatchDisplay();
			MenuPlayMatchmakingUI.modeButtonState = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(MenuPlayMatchmakingUI.localization.format("Easy_Button"), (Texture)bundle.load("Easy")),
				new GUIContent(MenuPlayMatchmakingUI.localization.format("Normal_Button"), (Texture)bundle.load("Normal")),
				new GUIContent(MenuPlayMatchmakingUI.localization.format("Hard_Button"), (Texture)bundle.load("Hard"))
			});
			MenuPlayMatchmakingUI.modeButtonState.positionOffset_X = -200;
			MenuPlayMatchmakingUI.modeButtonState.positionOffset_Y = 220;
			MenuPlayMatchmakingUI.modeButtonState.positionScale_X = 0.5f;
			MenuPlayMatchmakingUI.modeButtonState.sizeOffset_X = 400;
			MenuPlayMatchmakingUI.modeButtonState.sizeOffset_Y = 30;
			MenuPlayMatchmakingUI.modeButtonState.state = (int)PlaySettings.matchmakingMode;
			SleekButtonState sleekButtonState = MenuPlayMatchmakingUI.modeButtonState;
			if (MenuPlayMatchmakingUI.<>f__mg$cache4 == null)
			{
				MenuPlayMatchmakingUI.<>f__mg$cache4 = new SwappedState(MenuPlayMatchmakingUI.onSwappedModeState);
			}
			sleekButtonState.onSwappedState = MenuPlayMatchmakingUI.<>f__mg$cache4;
			MenuPlayMatchmakingUI.container.add(MenuPlayMatchmakingUI.modeButtonState);
			bundle.unload();
			MenuPlayMatchmakingUI.refreshLevels();
			Delegate onLevelsRefreshed = Level.onLevelsRefreshed;
			if (MenuPlayMatchmakingUI.<>f__mg$cache5 == null)
			{
				MenuPlayMatchmakingUI.<>f__mg$cache5 = new LevelsRefreshed(MenuPlayMatchmakingUI.onLevelsRefreshed);
			}
			Level.onLevelsRefreshed = (LevelsRefreshed)Delegate.Combine(onLevelsRefreshed, MenuPlayMatchmakingUI.<>f__mg$cache5);
			MenuPlayMatchmakingUI.backButton = new SleekButtonIcon((Texture2D)MenuDashboardUI.icons.load("Exit"));
			MenuPlayMatchmakingUI.backButton.positionOffset_Y = -50;
			MenuPlayMatchmakingUI.backButton.positionScale_Y = 1f;
			MenuPlayMatchmakingUI.backButton.sizeOffset_X = 200;
			MenuPlayMatchmakingUI.backButton.sizeOffset_Y = 50;
			MenuPlayMatchmakingUI.backButton.text = MenuDashboardUI.localization.format("BackButtonText");
			MenuPlayMatchmakingUI.backButton.tooltip = MenuDashboardUI.localization.format("BackButtonTooltip");
			SleekButton sleekButton4 = MenuPlayMatchmakingUI.backButton;
			if (MenuPlayMatchmakingUI.<>f__mg$cache6 == null)
			{
				MenuPlayMatchmakingUI.<>f__mg$cache6 = new ClickedButton(MenuPlayMatchmakingUI.onClickedBackButton);
			}
			sleekButton4.onClickedButton = MenuPlayMatchmakingUI.<>f__mg$cache6;
			MenuPlayMatchmakingUI.backButton.fontSize = 14;
			MenuPlayMatchmakingUI.backButton.iconImage.backgroundTint = ESleekTint.FOREGROUND;
			MenuPlayMatchmakingUI.container.add(MenuPlayMatchmakingUI.backButton);
			TempSteamworksMatchmaking matchmakingService = Provider.provider.matchmakingService;
			if (MenuPlayMatchmakingUI.<>f__mg$cache7 == null)
			{
				MenuPlayMatchmakingUI.<>f__mg$cache7 = new TempSteamworksMatchmaking.MatchmakingProgressedHandler(MenuPlayMatchmakingUI.handleMatchmakingProgressed);
			}
			matchmakingService.matchmakingProgressed = MenuPlayMatchmakingUI.<>f__mg$cache7;
			TempSteamworksMatchmaking matchmakingService2 = Provider.provider.matchmakingService;
			if (MenuPlayMatchmakingUI.<>f__mg$cache8 == null)
			{
				MenuPlayMatchmakingUI.<>f__mg$cache8 = new TempSteamworksMatchmaking.MatchmakingFinishedHandler(MenuPlayMatchmakingUI.handleMatchmakingFinished);
			}
			matchmakingService2.matchmakingFinished = MenuPlayMatchmakingUI.<>f__mg$cache8;
		}

		public static void open()
		{
			if (MenuPlayMatchmakingUI.active)
			{
				return;
			}
			MenuPlayMatchmakingUI.active = true;
			MenuPlayMatchmakingUI.container.lerpPositionScale(0f, 0f, ESleekLerp.EXPONENTIAL, 20f);
		}

		public static void close()
		{
			if (!MenuPlayMatchmakingUI.active)
			{
				return;
			}
			MenuPlayMatchmakingUI.active = false;
			MenuSettings.save();
			MenuPlayMatchmakingUI.container.lerpPositionScale(0f, 1f, ESleekLerp.EXPONENTIAL, 20f);
		}

		private static void updateSelection()
		{
			if (PlaySettings.matchmakingMap == null || PlaySettings.matchmakingMap.Length == 0)
			{
				return;
			}
			LevelInfo level = Level.getLevel(PlaySettings.matchmakingMap);
			if (level == null)
			{
				return;
			}
			Local local = Localization.tryRead(level.path, false);
			if (local != null && local.has("Name"))
			{
				MenuPlayMatchmakingUI.selectedBox.text = local.format("Name");
			}
			else
			{
				MenuPlayMatchmakingUI.selectedBox.text = PlaySettings.matchmakingMap;
			}
		}

		private static void onClickedLevel(SleekLevel level, byte index)
		{
			if ((int)index < MenuPlayMatchmakingUI.levels.Length && MenuPlayMatchmakingUI.levels[(int)index] != null)
			{
				PlaySettings.matchmakingMap = MenuPlayMatchmakingUI.levels[(int)index].name;
				MenuPlayMatchmakingUI.updateSelection();
			}
		}

		private static void onClickedSearchButton(SleekButton button)
		{
			if (PlaySettings.matchmakingMap == null || PlaySettings.matchmakingMap.Length == 0)
			{
				return;
			}
			MenuSettings.save();
			Provider.provider.matchmakingService.initializeMatchmaking();
			Provider.provider.matchmakingService.refreshMasterServer(ESteamServerList.INTERNET, PlaySettings.matchmakingMap, EPassword.NO, EWorkshop.NO, EPlugins.ANY, EAttendance.SPACE, EVACProtectionFilter.Secure, EBattlEyeProtectionFilter.Secure, false, ECombat.PVP, ECheats.NO, PlaySettings.matchmakingMode, ECameraMode.BOTH);
			Provider.provider.matchmakingService.sortMasterServer(new SteamServerInfoMatchmakingComparator());
			MenuPlayMatchmakingUI.updateProgressDisplay();
			MenuPlayMatchmakingUI.updateMatchDisplay();
		}

		private static void onClickedInfoButton(SleekButton button)
		{
			MenuPlayServerInfoUI.open(Provider.provider.matchmakingService.matchmakingBestServer, string.Empty, MenuPlayServerInfoUI.EServerInfoOpenContext.MATCHMAKING);
			MenuPlayMatchmakingUI.close();
		}

		private static void onClickedLuckyButton(SleekButton button)
		{
			Provider.connect(Provider.provider.matchmakingService.matchmakingBestServer, string.Empty);
		}

		private static void onSwappedModeState(SleekButtonState button, int index)
		{
			PlaySettings.matchmakingMode = (EGameMode)index;
		}

		private static void refreshLevels()
		{
			MenuPlayMatchmakingUI.levelScrollBox.remove();
			MenuPlayMatchmakingUI.levels = Level.getLevels((!OptionsSettings.matchmakingShowAllMaps) ? ESingleplayerMapCategory.MATCHMAKING : ESingleplayerMapCategory.ALL);
			bool flag = false;
			MenuPlayMatchmakingUI.levelButtons = new SleekLevel[MenuPlayMatchmakingUI.levels.Length];
			for (int i = 0; i < MenuPlayMatchmakingUI.levels.Length; i++)
			{
				if (MenuPlayMatchmakingUI.levels[i] != null)
				{
					SleekLevel sleekLevel = new SleekLevel(MenuPlayMatchmakingUI.levels[i], false);
					sleekLevel.positionOffset_Y = i * 110;
					SleekLevel sleekLevel2 = sleekLevel;
					if (MenuPlayMatchmakingUI.<>f__mg$cache0 == null)
					{
						MenuPlayMatchmakingUI.<>f__mg$cache0 = new ClickedLevel(MenuPlayMatchmakingUI.onClickedLevel);
					}
					sleekLevel2.onClickedLevel = MenuPlayMatchmakingUI.<>f__mg$cache0;
					MenuPlayMatchmakingUI.levelScrollBox.add(sleekLevel);
					MenuPlayMatchmakingUI.levelButtons[i] = sleekLevel;
					if (!flag && MenuPlayMatchmakingUI.levels[i].name == PlaySettings.matchmakingMap)
					{
						flag = true;
					}
				}
			}
			if (MenuPlayMatchmakingUI.levels.Length == 0)
			{
				PlaySettings.matchmakingMap = string.Empty;
			}
			else if (!flag || PlaySettings.matchmakingMap == null || PlaySettings.matchmakingMap.Length == 0)
			{
				PlaySettings.matchmakingMap = MenuPlayMatchmakingUI.levels[0].name;
			}
			MenuPlayMatchmakingUI.updateSelection();
			MenuPlayMatchmakingUI.levelScrollBox.area = new Rect(0f, 0f, 5f, (float)(MenuPlayMatchmakingUI.levels.Length * 110 - 10));
		}

		private static void onLevelsRefreshed()
		{
			MenuPlayMatchmakingUI.refreshLevels();
		}

		private static void updateProgressDisplay()
		{
			if (MenuPlayMatchmakingUI.progressBox == null)
			{
				return;
			}
			MenuPlayMatchmakingUI.progressBox.text = MenuPlayMatchmakingUI.localization.format("Progress_Matches", new object[]
			{
				(Provider.provider.matchmakingService.serverList == null) ? 0 : Provider.provider.matchmakingService.serverList.Count
			});
			SleekBox sleekBox = MenuPlayMatchmakingUI.progressBox;
			sleekBox.text += "\n";
			if (Provider.provider.matchmakingService.matchmakingBestServer != null)
			{
				SleekBox sleekBox2 = MenuPlayMatchmakingUI.progressBox;
				sleekBox2.text += MenuPlayMatchmakingUI.localization.format("Progress_Best", new object[]
				{
					MenuPlayMatchmakingUI.localization.format("Match_Yes", new object[]
					{
						Provider.provider.matchmakingService.matchmakingBestServer.players,
						Provider.provider.matchmakingService.matchmakingBestServer.maxPlayers,
						Provider.provider.matchmakingService.matchmakingBestServer.ping
					})
				});
			}
			else
			{
				SleekBox sleekBox3 = MenuPlayMatchmakingUI.progressBox;
				sleekBox3.text += MenuPlayMatchmakingUI.localization.format("Progress_Best", new object[]
				{
					MenuPlayMatchmakingUI.localization.format("Match_No")
				});
			}
		}

		private static void updateMatchDisplay()
		{
			if (MenuPlayMatchmakingUI.infoButton == null || MenuPlayMatchmakingUI.luckyButton == null)
			{
				return;
			}
			MenuPlayMatchmakingUI.infoButton.isVisible = (Provider.provider.matchmakingService.matchmakingBestServer != null);
			MenuPlayMatchmakingUI.luckyButton.isVisible = (Provider.provider.matchmakingService.matchmakingBestServer != null);
		}

		private static void handleMatchmakingProgressed()
		{
			MenuPlayMatchmakingUI.updateProgressDisplay();
		}

		private static void handleMatchmakingFinished()
		{
			MenuPlayMatchmakingUI.updateMatchDisplay();
		}

		private static void onClickedBackButton(SleekButton button)
		{
			MenuPlayUI.open();
			MenuPlayMatchmakingUI.close();
		}

		public static Local localization;

		private static Sleek container;

		public static bool active;

		private static SleekButtonIcon backButton;

		private static LevelInfo[] levels;

		private static SleekScrollBox levelScrollBox;

		private static SleekLevel[] levelButtons;

		private static SleekButtonIcon searchButton;

		private static SleekButtonState modeButtonState;

		private static SleekBox selectedBox;

		private static SleekBox progressBox;

		private static SleekButtonIcon infoButton;

		private static SleekButtonIcon luckyButton;

		[CompilerGenerated]
		private static ClickedLevel <>f__mg$cache0;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache1;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache2;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache3;

		[CompilerGenerated]
		private static SwappedState <>f__mg$cache4;

		[CompilerGenerated]
		private static LevelsRefreshed <>f__mg$cache5;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache6;

		[CompilerGenerated]
		private static TempSteamworksMatchmaking.MatchmakingProgressedHandler <>f__mg$cache7;

		[CompilerGenerated]
		private static TempSteamworksMatchmaking.MatchmakingFinishedHandler <>f__mg$cache8;
	}
}
