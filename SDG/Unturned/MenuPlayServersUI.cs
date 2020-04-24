using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SDG.Provider;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	public class MenuPlayServersUI
	{
		public MenuPlayServersUI()
		{
			if (MenuPlayServersUI.icons != null)
			{
				MenuPlayServersUI.icons.unload();
			}
			MenuPlayServersUI.localization = Localization.read("/Menu/Play/MenuPlayServers.dat");
			MenuPlayServersUI.icons = Bundles.getBundle("/Bundles/Textures/Menu/Icons/Play/MenuPlayServers/MenuPlayServers.unity3d");
			MenuPlayServersUI.container = new Sleek();
			MenuPlayServersUI.container.positionOffset_X = 10;
			MenuPlayServersUI.container.positionOffset_Y = 10;
			MenuPlayServersUI.container.positionScale_Y = 1f;
			MenuPlayServersUI.container.sizeOffset_X = -20;
			MenuPlayServersUI.container.sizeOffset_Y = -20;
			MenuPlayServersUI.container.sizeScale_X = 1f;
			MenuPlayServersUI.container.sizeScale_Y = 1f;
			MenuUI.container.add(MenuPlayServersUI.container);
			MenuPlayServersUI.active = false;
			MenuPlayServersUI.list = new Sleek();
			MenuPlayServersUI.list.positionOffset_X = 210;
			MenuPlayServersUI.list.sizeOffset_X = -210;
			MenuPlayServersUI.list.sizeScale_X = 1f;
			MenuPlayServersUI.list.sizeScale_Y = 1f;
			MenuPlayServersUI.container.add(MenuPlayServersUI.list);
			MenuPlayServersUI.sortName = new SleekButton();
			MenuPlayServersUI.sortName.sizeOffset_X = -270;
			MenuPlayServersUI.sortName.sizeOffset_Y = 30;
			MenuPlayServersUI.sortName.sizeScale_X = 1f;
			MenuPlayServersUI.sortName.text = MenuPlayServersUI.localization.format("Sort_Name");
			MenuPlayServersUI.sortName.tooltip = MenuPlayServersUI.localization.format("Sort_Name_Tooltip");
			SleekButton sleekButton = MenuPlayServersUI.sortName;
			if (MenuPlayServersUI.<>f__mg$cache2 == null)
			{
				MenuPlayServersUI.<>f__mg$cache2 = new ClickedButton(MenuPlayServersUI.onClickedSortNameButton);
			}
			sleekButton.onClickedButton = MenuPlayServersUI.<>f__mg$cache2;
			MenuPlayServersUI.list.add(MenuPlayServersUI.sortName);
			MenuPlayServersUI.sortMap = new SleekButton();
			MenuPlayServersUI.sortMap.positionOffset_X = -260;
			MenuPlayServersUI.sortMap.positionScale_X = 1f;
			MenuPlayServersUI.sortMap.sizeOffset_X = 100;
			MenuPlayServersUI.sortMap.sizeOffset_Y = 30;
			MenuPlayServersUI.sortMap.text = MenuPlayServersUI.localization.format("Sort_Map");
			MenuPlayServersUI.sortMap.tooltip = MenuPlayServersUI.localization.format("Sort_Map_Tooltip");
			SleekButton sleekButton2 = MenuPlayServersUI.sortMap;
			if (MenuPlayServersUI.<>f__mg$cache3 == null)
			{
				MenuPlayServersUI.<>f__mg$cache3 = new ClickedButton(MenuPlayServersUI.onClickedSortMapButton);
			}
			sleekButton2.onClickedButton = MenuPlayServersUI.<>f__mg$cache3;
			MenuPlayServersUI.list.add(MenuPlayServersUI.sortMap);
			MenuPlayServersUI.sortPlayers = new SleekButton();
			MenuPlayServersUI.sortPlayers.positionOffset_X = -150;
			MenuPlayServersUI.sortPlayers.positionScale_X = 1f;
			MenuPlayServersUI.sortPlayers.sizeOffset_X = 60;
			MenuPlayServersUI.sortPlayers.sizeOffset_Y = 30;
			MenuPlayServersUI.sortPlayers.text = MenuPlayServersUI.localization.format("Sort_Players");
			MenuPlayServersUI.sortPlayers.tooltip = MenuPlayServersUI.localization.format("Sort_Players_Tooltip");
			SleekButton sleekButton3 = MenuPlayServersUI.sortPlayers;
			if (MenuPlayServersUI.<>f__mg$cache4 == null)
			{
				MenuPlayServersUI.<>f__mg$cache4 = new ClickedButton(MenuPlayServersUI.onClickedSortPlayersButton);
			}
			sleekButton3.onClickedButton = MenuPlayServersUI.<>f__mg$cache4;
			MenuPlayServersUI.list.add(MenuPlayServersUI.sortPlayers);
			MenuPlayServersUI.sortPing = new SleekButton();
			MenuPlayServersUI.sortPing.positionOffset_X = -80;
			MenuPlayServersUI.sortPing.positionScale_X = 1f;
			MenuPlayServersUI.sortPing.sizeOffset_X = 50;
			MenuPlayServersUI.sortPing.sizeOffset_Y = 30;
			MenuPlayServersUI.sortPing.text = MenuPlayServersUI.localization.format("Sort_Ping");
			MenuPlayServersUI.sortPing.tooltip = MenuPlayServersUI.localization.format("Sort_Ping_Tooltip");
			SleekButton sleekButton4 = MenuPlayServersUI.sortPing;
			if (MenuPlayServersUI.<>f__mg$cache5 == null)
			{
				MenuPlayServersUI.<>f__mg$cache5 = new ClickedButton(MenuPlayServersUI.onClickedSortPingButton);
			}
			sleekButton4.onClickedButton = MenuPlayServersUI.<>f__mg$cache5;
			MenuPlayServersUI.list.add(MenuPlayServersUI.sortPing);
			MenuPlayServersUI.infoBox = new SleekBox();
			MenuPlayServersUI.infoBox.positionOffset_Y = 40;
			MenuPlayServersUI.infoBox.sizeOffset_X = -30;
			MenuPlayServersUI.infoBox.sizeScale_X = 1f;
			MenuPlayServersUI.infoBox.sizeOffset_Y = 50;
			MenuPlayServersUI.infoBox.text = MenuPlayServersUI.localization.format("No_Servers", new object[]
			{
				Provider.APP_VERSION
			});
			MenuPlayServersUI.infoBox.fontSize = 14;
			MenuPlayServersUI.list.add(MenuPlayServersUI.infoBox);
			MenuPlayServersUI.infoBox.isVisible = false;
			MenuPlayServersUI.serverButtons = new List<SleekServer>();
			TempSteamworksMatchmaking matchmakingService = Provider.provider.matchmakingService;
			if (MenuPlayServersUI.<>f__mg$cache6 == null)
			{
				MenuPlayServersUI.<>f__mg$cache6 = new TempSteamworksMatchmaking.MasterServerAdded(MenuPlayServersUI.onMasterServerAdded);
			}
			matchmakingService.onMasterServerAdded = MenuPlayServersUI.<>f__mg$cache6;
			TempSteamworksMatchmaking matchmakingService2 = Provider.provider.matchmakingService;
			if (MenuPlayServersUI.<>f__mg$cache7 == null)
			{
				MenuPlayServersUI.<>f__mg$cache7 = new TempSteamworksMatchmaking.MasterServerRemoved(MenuPlayServersUI.onMasterServerRemoved);
			}
			matchmakingService2.onMasterServerRemoved = MenuPlayServersUI.<>f__mg$cache7;
			TempSteamworksMatchmaking matchmakingService3 = Provider.provider.matchmakingService;
			if (MenuPlayServersUI.<>f__mg$cache8 == null)
			{
				MenuPlayServersUI.<>f__mg$cache8 = new TempSteamworksMatchmaking.MasterServerResorted(MenuPlayServersUI.onMasterServerResorted);
			}
			matchmakingService3.onMasterServerResorted = MenuPlayServersUI.<>f__mg$cache8;
			TempSteamworksMatchmaking matchmakingService4 = Provider.provider.matchmakingService;
			if (MenuPlayServersUI.<>f__mg$cache9 == null)
			{
				MenuPlayServersUI.<>f__mg$cache9 = new TempSteamworksMatchmaking.MasterServerRefreshed(MenuPlayServersUI.onMasterServerRefreshed);
			}
			matchmakingService4.onMasterServerRefreshed = MenuPlayServersUI.<>f__mg$cache9;
			MenuPlayServersUI.nameField = new SleekField();
			MenuPlayServersUI.nameField.positionOffset_Y = -110;
			MenuPlayServersUI.nameField.positionScale_Y = 1f;
			MenuPlayServersUI.nameField.sizeOffset_X = -5;
			MenuPlayServersUI.nameField.sizeOffset_Y = 30;
			MenuPlayServersUI.nameField.sizeScale_X = 0.4f;
			MenuPlayServersUI.nameField.text = PlaySettings.serversName;
			MenuPlayServersUI.nameField.hint = MenuPlayServersUI.localization.format("Name_Field_Hint");
			SleekField sleekField = MenuPlayServersUI.nameField;
			if (MenuPlayServersUI.<>f__mg$cacheA == null)
			{
				MenuPlayServersUI.<>f__mg$cacheA = new Typed(MenuPlayServersUI.onTypedNameField);
			}
			sleekField.onTyped = MenuPlayServersUI.<>f__mg$cacheA;
			MenuPlayServersUI.list.add(MenuPlayServersUI.nameField);
			MenuPlayServersUI.passwordField = new SleekField();
			MenuPlayServersUI.passwordField.positionOffset_X = 5;
			MenuPlayServersUI.passwordField.positionOffset_Y = -110;
			MenuPlayServersUI.passwordField.positionScale_X = 0.4f;
			MenuPlayServersUI.passwordField.positionScale_Y = 1f;
			MenuPlayServersUI.passwordField.sizeOffset_X = -10;
			MenuPlayServersUI.passwordField.sizeOffset_Y = 30;
			MenuPlayServersUI.passwordField.sizeScale_X = 0.4f;
			MenuPlayServersUI.passwordField.replace = MenuPlayServersUI.localization.format("Password_Field_Replace").ToCharArray()[0];
			MenuPlayServersUI.passwordField.text = PlaySettings.serversPassword;
			MenuPlayServersUI.passwordField.hint = MenuPlayServersUI.localization.format("Password_Field_Hint");
			SleekField sleekField2 = MenuPlayServersUI.passwordField;
			if (MenuPlayServersUI.<>f__mg$cacheB == null)
			{
				MenuPlayServersUI.<>f__mg$cacheB = new Typed(MenuPlayServersUI.onTypedPasswordField);
			}
			sleekField2.onTyped = MenuPlayServersUI.<>f__mg$cacheB;
			MenuPlayServersUI.list.add(MenuPlayServersUI.passwordField);
			MenuPlayServersUI.refreshInternetButton = new SleekButton();
			MenuPlayServersUI.refreshInternetButton.sizeOffset_X = 200;
			MenuPlayServersUI.refreshInternetButton.sizeOffset_Y = 50;
			MenuPlayServersUI.refreshInternetButton.text = MenuPlayServersUI.localization.format("Refresh_Internet_Button");
			MenuPlayServersUI.refreshInternetButton.tooltip = MenuPlayServersUI.localization.format("Refresh_Internet_Button_Tooltip");
			SleekButton sleekButton5 = MenuPlayServersUI.refreshInternetButton;
			if (MenuPlayServersUI.<>f__mg$cacheC == null)
			{
				MenuPlayServersUI.<>f__mg$cacheC = new ClickedButton(MenuPlayServersUI.onClickedRefreshInternetButton);
			}
			sleekButton5.onClickedButton = MenuPlayServersUI.<>f__mg$cacheC;
			MenuPlayServersUI.refreshInternetButton.fontSize = 14;
			MenuPlayServersUI.container.add(MenuPlayServersUI.refreshInternetButton);
			MenuPlayServersUI.refreshLANButton = new SleekButton();
			MenuPlayServersUI.refreshLANButton.positionOffset_Y = 180;
			MenuPlayServersUI.refreshLANButton.sizeOffset_X = 200;
			MenuPlayServersUI.refreshLANButton.sizeOffset_Y = 50;
			MenuPlayServersUI.refreshLANButton.text = MenuPlayServersUI.localization.format("Refresh_LAN_Button");
			MenuPlayServersUI.refreshLANButton.tooltip = MenuPlayServersUI.localization.format("Refresh_LAN_Button_Tooltip");
			SleekButton sleekButton6 = MenuPlayServersUI.refreshLANButton;
			if (MenuPlayServersUI.<>f__mg$cacheD == null)
			{
				MenuPlayServersUI.<>f__mg$cacheD = new ClickedButton(MenuPlayServersUI.onClickedRefreshLANButton);
			}
			sleekButton6.onClickedButton = MenuPlayServersUI.<>f__mg$cacheD;
			MenuPlayServersUI.refreshLANButton.fontSize = 14;
			MenuPlayServersUI.container.add(MenuPlayServersUI.refreshLANButton);
			MenuPlayServersUI.refreshHistoryButton = new SleekButton();
			MenuPlayServersUI.refreshHistoryButton.positionOffset_Y = 120;
			MenuPlayServersUI.refreshHistoryButton.sizeOffset_X = 200;
			MenuPlayServersUI.refreshHistoryButton.sizeOffset_Y = 50;
			MenuPlayServersUI.refreshHistoryButton.text = MenuPlayServersUI.localization.format("Refresh_History_Button");
			MenuPlayServersUI.refreshHistoryButton.tooltip = MenuPlayServersUI.localization.format("Refresh_History_Button_Tooltip");
			SleekButton sleekButton7 = MenuPlayServersUI.refreshHistoryButton;
			if (MenuPlayServersUI.<>f__mg$cacheE == null)
			{
				MenuPlayServersUI.<>f__mg$cacheE = new ClickedButton(MenuPlayServersUI.onClickedRefreshHistoryButton);
			}
			sleekButton7.onClickedButton = MenuPlayServersUI.<>f__mg$cacheE;
			MenuPlayServersUI.refreshHistoryButton.fontSize = 14;
			MenuPlayServersUI.container.add(MenuPlayServersUI.refreshHistoryButton);
			MenuPlayServersUI.refreshFavoritesButton = new SleekButton();
			MenuPlayServersUI.refreshFavoritesButton.positionOffset_Y = 60;
			MenuPlayServersUI.refreshFavoritesButton.sizeOffset_X = 200;
			MenuPlayServersUI.refreshFavoritesButton.sizeOffset_Y = 50;
			MenuPlayServersUI.refreshFavoritesButton.text = MenuPlayServersUI.localization.format("Refresh_Favorites_Button");
			MenuPlayServersUI.refreshFavoritesButton.tooltip = MenuPlayServersUI.localization.format("Refresh_Favorites_Button_Tooltip");
			SleekButton sleekButton8 = MenuPlayServersUI.refreshFavoritesButton;
			if (MenuPlayServersUI.<>f__mg$cacheF == null)
			{
				MenuPlayServersUI.<>f__mg$cacheF = new ClickedButton(MenuPlayServersUI.onClickedRefreshFavoritesButton);
			}
			sleekButton8.onClickedButton = MenuPlayServersUI.<>f__mg$cacheF;
			MenuPlayServersUI.refreshFavoritesButton.fontSize = 14;
			MenuPlayServersUI.container.add(MenuPlayServersUI.refreshFavoritesButton);
			MenuPlayServersUI.refreshFriendsButton = new SleekButton();
			MenuPlayServersUI.refreshFriendsButton.positionOffset_Y = 240;
			MenuPlayServersUI.refreshFriendsButton.sizeOffset_X = 200;
			MenuPlayServersUI.refreshFriendsButton.sizeOffset_Y = 50;
			MenuPlayServersUI.refreshFriendsButton.text = MenuPlayServersUI.localization.format("Refresh_Friends_Button");
			MenuPlayServersUI.refreshFriendsButton.tooltip = MenuPlayServersUI.localization.format("Refresh_Friends_Button_Tooltip");
			SleekButton sleekButton9 = MenuPlayServersUI.refreshFriendsButton;
			if (MenuPlayServersUI.<>f__mg$cache10 == null)
			{
				MenuPlayServersUI.<>f__mg$cache10 = new ClickedButton(MenuPlayServersUI.onClickedRefreshFriendsButton);
			}
			sleekButton9.onClickedButton = MenuPlayServersUI.<>f__mg$cache10;
			MenuPlayServersUI.refreshFriendsButton.fontSize = 14;
			MenuPlayServersUI.container.add(MenuPlayServersUI.refreshFriendsButton);
			if (Provider.isPro)
			{
				MenuPlayServersUI.refreshLANButton.positionOffset_Y += 60;
				MenuPlayServersUI.refreshHistoryButton.positionOffset_Y += 60;
				MenuPlayServersUI.refreshFavoritesButton.positionOffset_Y += 60;
				MenuPlayServersUI.refreshFriendsButton.positionOffset_Y += 60;
				MenuPlayServersUI.refreshGoldButton = new SleekButton();
				MenuPlayServersUI.refreshGoldButton.positionOffset_Y = 60;
				MenuPlayServersUI.refreshGoldButton.sizeOffset_X = 200;
				MenuPlayServersUI.refreshGoldButton.sizeOffset_Y = 50;
				MenuPlayServersUI.refreshGoldButton.text = MenuPlayServersUI.localization.format("Refresh_Gold_Button");
				MenuPlayServersUI.refreshGoldButton.tooltip = MenuPlayServersUI.localization.format("Refresh_Gold_Button_Tooltip");
				SleekButton sleekButton10 = MenuPlayServersUI.refreshGoldButton;
				if (MenuPlayServersUI.<>f__mg$cache11 == null)
				{
					MenuPlayServersUI.<>f__mg$cache11 = new ClickedButton(MenuPlayServersUI.onClickedRefreshGoldButton);
				}
				sleekButton10.onClickedButton = MenuPlayServersUI.<>f__mg$cache11;
				MenuPlayServersUI.refreshGoldButton.fontSize = 14;
				MenuPlayServersUI.refreshGoldButton.backgroundTint = ESleekTint.NONE;
				MenuPlayServersUI.refreshGoldButton.foregroundTint = ESleekTint.NONE;
				MenuPlayServersUI.refreshGoldButton.backgroundColor = Palette.PRO;
				MenuPlayServersUI.refreshGoldButton.foregroundColor = Palette.PRO;
				MenuPlayServersUI.container.add(MenuPlayServersUI.refreshGoldButton);
			}
			MenuPlayServersUI.mapButtonState = new SleekButtonState(new GUIContent[0]);
			MenuPlayServersUI.mapButtonState.positionOffset_X = 5;
			MenuPlayServersUI.mapButtonState.positionOffset_Y = -70;
			MenuPlayServersUI.mapButtonState.positionScale_X = 0.2f;
			MenuPlayServersUI.mapButtonState.positionScale_Y = 1f;
			MenuPlayServersUI.mapButtonState.sizeOffset_X = -10;
			MenuPlayServersUI.mapButtonState.sizeOffset_Y = 30;
			MenuPlayServersUI.mapButtonState.sizeScale_X = 0.2f;
			SleekButtonState sleekButtonState = MenuPlayServersUI.mapButtonState;
			if (MenuPlayServersUI.<>f__mg$cache12 == null)
			{
				MenuPlayServersUI.<>f__mg$cache12 = new SwappedState(MenuPlayServersUI.onSwappedMapState);
			}
			sleekButtonState.onSwappedState = MenuPlayServersUI.<>f__mg$cache12;
			MenuPlayServersUI.list.add(MenuPlayServersUI.mapButtonState);
			MenuPlayServersUI.passwordButtonState = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(MenuPlayServersUI.localization.format("No_Password_Button")),
				new GUIContent(MenuPlayServersUI.localization.format("Yes_Password_Button")),
				new GUIContent(MenuPlayServersUI.localization.format("Any_Password_Button"))
			});
			MenuPlayServersUI.passwordButtonState.positionOffset_X = 5;
			MenuPlayServersUI.passwordButtonState.positionOffset_Y = -110;
			MenuPlayServersUI.passwordButtonState.positionScale_X = 0.8f;
			MenuPlayServersUI.passwordButtonState.positionScale_Y = 1f;
			MenuPlayServersUI.passwordButtonState.sizeOffset_X = -5;
			MenuPlayServersUI.passwordButtonState.sizeOffset_Y = 30;
			MenuPlayServersUI.passwordButtonState.sizeScale_X = 0.2f;
			MenuPlayServersUI.passwordButtonState.state = (int)FilterSettings.filterPassword;
			SleekButtonState sleekButtonState2 = MenuPlayServersUI.passwordButtonState;
			if (MenuPlayServersUI.<>f__mg$cache13 == null)
			{
				MenuPlayServersUI.<>f__mg$cache13 = new SwappedState(MenuPlayServersUI.onSwappedPasswordState);
			}
			sleekButtonState2.onSwappedState = MenuPlayServersUI.<>f__mg$cache13;
			MenuPlayServersUI.list.add(MenuPlayServersUI.passwordButtonState);
			MenuPlayServersUI.workshopButtonState = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(MenuPlayServersUI.localization.format("No_Workshop_Button")),
				new GUIContent(MenuPlayServersUI.localization.format("Yes_Workshop_Button")),
				new GUIContent(MenuPlayServersUI.localization.format("Any_Workshop_Button"))
			});
			MenuPlayServersUI.workshopButtonState.positionOffset_Y = -30;
			MenuPlayServersUI.workshopButtonState.positionScale_Y = 1f;
			MenuPlayServersUI.workshopButtonState.sizeOffset_X = -5;
			MenuPlayServersUI.workshopButtonState.sizeOffset_Y = 30;
			MenuPlayServersUI.workshopButtonState.sizeScale_X = 0.2f;
			MenuPlayServersUI.workshopButtonState.state = (int)FilterSettings.filterWorkshop;
			SleekButtonState sleekButtonState3 = MenuPlayServersUI.workshopButtonState;
			if (MenuPlayServersUI.<>f__mg$cache14 == null)
			{
				MenuPlayServersUI.<>f__mg$cache14 = new SwappedState(MenuPlayServersUI.onSwappedWorkshopState);
			}
			sleekButtonState3.onSwappedState = MenuPlayServersUI.<>f__mg$cache14;
			MenuPlayServersUI.list.add(MenuPlayServersUI.workshopButtonState);
			MenuPlayServersUI.pluginsButtonState = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(MenuPlayServersUI.localization.format("No_Plugins_Button")),
				new GUIContent(MenuPlayServersUI.localization.format("Yes_Plugins_Button")),
				new GUIContent(MenuPlayServersUI.localization.format("Any_Plugins_Button"))
			});
			MenuPlayServersUI.pluginsButtonState.positionOffset_X = 5;
			MenuPlayServersUI.pluginsButtonState.positionOffset_Y = -70;
			MenuPlayServersUI.pluginsButtonState.positionScale_X = 0.8f;
			MenuPlayServersUI.pluginsButtonState.positionScale_Y = 1f;
			MenuPlayServersUI.pluginsButtonState.sizeOffset_X = -5;
			MenuPlayServersUI.pluginsButtonState.sizeOffset_Y = 30;
			MenuPlayServersUI.pluginsButtonState.sizeScale_X = 0.2f;
			MenuPlayServersUI.pluginsButtonState.state = (int)FilterSettings.filterPlugins;
			SleekButtonState sleekButtonState4 = MenuPlayServersUI.pluginsButtonState;
			if (MenuPlayServersUI.<>f__mg$cache15 == null)
			{
				MenuPlayServersUI.<>f__mg$cache15 = new SwappedState(MenuPlayServersUI.onSwappedPluginsState);
			}
			sleekButtonState4.onSwappedState = MenuPlayServersUI.<>f__mg$cache15;
			MenuPlayServersUI.list.add(MenuPlayServersUI.pluginsButtonState);
			MenuPlayServersUI.cheatsButtonState = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(MenuPlayServersUI.localization.format("No_Cheats_Button")),
				new GUIContent(MenuPlayServersUI.localization.format("Yes_Cheats_Button")),
				new GUIContent(MenuPlayServersUI.localization.format("Any_Cheats_Button"))
			});
			MenuPlayServersUI.cheatsButtonState.positionOffset_X = 5;
			MenuPlayServersUI.cheatsButtonState.positionOffset_Y = -30;
			MenuPlayServersUI.cheatsButtonState.positionScale_X = 0.8f;
			MenuPlayServersUI.cheatsButtonState.positionScale_Y = 1f;
			MenuPlayServersUI.cheatsButtonState.sizeOffset_X = -5;
			MenuPlayServersUI.cheatsButtonState.sizeOffset_Y = 30;
			MenuPlayServersUI.cheatsButtonState.sizeScale_X = 0.2f;
			MenuPlayServersUI.cheatsButtonState.state = (int)FilterSettings.filterCheats;
			SleekButtonState sleekButtonState5 = MenuPlayServersUI.cheatsButtonState;
			if (MenuPlayServersUI.<>f__mg$cache16 == null)
			{
				MenuPlayServersUI.<>f__mg$cache16 = new SwappedState(MenuPlayServersUI.onSwappedCheatsState);
			}
			sleekButtonState5.onSwappedState = MenuPlayServersUI.<>f__mg$cache16;
			MenuPlayServersUI.list.add(MenuPlayServersUI.cheatsButtonState);
			MenuPlayServersUI.attendanceButtonState = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(MenuPlayServersUI.localization.format("Empty_Button"), (Texture)MenuPlayServersUI.icons.load("Empty")),
				new GUIContent(MenuPlayServersUI.localization.format("Space_Button"), (Texture)MenuPlayServersUI.icons.load("Space")),
				new GUIContent(MenuPlayServersUI.localization.format("Any_Attendance_Button"))
			});
			MenuPlayServersUI.attendanceButtonState.positionOffset_X = 5;
			MenuPlayServersUI.attendanceButtonState.positionOffset_Y = -30;
			MenuPlayServersUI.attendanceButtonState.positionScale_X = 0.4f;
			MenuPlayServersUI.attendanceButtonState.positionScale_Y = 1f;
			MenuPlayServersUI.attendanceButtonState.sizeOffset_X = -10;
			MenuPlayServersUI.attendanceButtonState.sizeOffset_Y = 30;
			MenuPlayServersUI.attendanceButtonState.sizeScale_X = 0.2f;
			MenuPlayServersUI.attendanceButtonState.state = (int)FilterSettings.filterAttendance;
			SleekButtonState sleekButtonState6 = MenuPlayServersUI.attendanceButtonState;
			if (MenuPlayServersUI.<>f__mg$cache17 == null)
			{
				MenuPlayServersUI.<>f__mg$cache17 = new SwappedState(MenuPlayServersUI.onSwappedAttendanceState);
			}
			sleekButtonState6.onSwappedState = MenuPlayServersUI.<>f__mg$cache17;
			MenuPlayServersUI.list.add(MenuPlayServersUI.attendanceButtonState);
			MenuPlayServersUI.VACProtectionButtonState = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(MenuPlayServersUI.localization.format("VAC_Secure_Button"), (Texture)MenuPlayServersUI.icons.load("VAC")),
				new GUIContent(MenuPlayServersUI.localization.format("VAC_Insecure_Button")),
				new GUIContent(MenuPlayServersUI.localization.format("VAC_Any_Button"))
			});
			MenuPlayServersUI.VACProtectionButtonState.positionOffset_X = 5;
			MenuPlayServersUI.VACProtectionButtonState.positionOffset_Y = -70;
			MenuPlayServersUI.VACProtectionButtonState.positionScale_X = 0.4f;
			MenuPlayServersUI.VACProtectionButtonState.positionScale_Y = 1f;
			MenuPlayServersUI.VACProtectionButtonState.sizeOffset_X = -10;
			MenuPlayServersUI.VACProtectionButtonState.sizeOffset_Y = 30;
			MenuPlayServersUI.VACProtectionButtonState.sizeScale_X = 0.2f;
			MenuPlayServersUI.VACProtectionButtonState.state = (int)FilterSettings.filterVACProtection;
			SleekButtonState vacprotectionButtonState = MenuPlayServersUI.VACProtectionButtonState;
			if (MenuPlayServersUI.<>f__mg$cache18 == null)
			{
				MenuPlayServersUI.<>f__mg$cache18 = new SwappedState(MenuPlayServersUI.onSwappedVACProtectionState);
			}
			vacprotectionButtonState.onSwappedState = MenuPlayServersUI.<>f__mg$cache18;
			MenuPlayServersUI.list.add(MenuPlayServersUI.VACProtectionButtonState);
			MenuPlayServersUI.battlEyeProtectionButtonState = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(MenuPlayServersUI.localization.format("BattlEye_Secure_Button"), (Texture)MenuPlayServersUI.icons.load("BattlEye")),
				new GUIContent(MenuPlayServersUI.localization.format("BattlEye_Insecure_Button")),
				new GUIContent(MenuPlayServersUI.localization.format("BattlEye_Any_Button"))
			});
			MenuPlayServersUI.battlEyeProtectionButtonState.positionOffset_X = 5;
			MenuPlayServersUI.battlEyeProtectionButtonState.positionOffset_Y = -70;
			MenuPlayServersUI.battlEyeProtectionButtonState.positionScale_X = 0.6f;
			MenuPlayServersUI.battlEyeProtectionButtonState.positionScale_Y = 1f;
			MenuPlayServersUI.battlEyeProtectionButtonState.sizeOffset_X = -10;
			MenuPlayServersUI.battlEyeProtectionButtonState.sizeOffset_Y = 30;
			MenuPlayServersUI.battlEyeProtectionButtonState.sizeScale_X = 0.2f;
			MenuPlayServersUI.battlEyeProtectionButtonState.state = (int)FilterSettings.filterBattlEyeProtection;
			SleekButtonState sleekButtonState7 = MenuPlayServersUI.battlEyeProtectionButtonState;
			if (MenuPlayServersUI.<>f__mg$cache19 == null)
			{
				MenuPlayServersUI.<>f__mg$cache19 = new SwappedState(MenuPlayServersUI.onSwappedBattlEyeProtectionState);
			}
			sleekButtonState7.onSwappedState = MenuPlayServersUI.<>f__mg$cache19;
			MenuPlayServersUI.list.add(MenuPlayServersUI.battlEyeProtectionButtonState);
			MenuPlayServersUI.combatButtonState = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(MenuPlayServersUI.localization.format("PvP_Button"), (Texture)MenuPlayServersUI.icons.load("PvP")),
				new GUIContent(MenuPlayServersUI.localization.format("PvE_Button"), (Texture)MenuPlayServersUI.icons.load("PvE")),
				new GUIContent(MenuPlayServersUI.localization.format("Any_Combat_Button"))
			});
			MenuPlayServersUI.combatButtonState.positionOffset_Y = -70;
			MenuPlayServersUI.combatButtonState.positionScale_Y = 1f;
			MenuPlayServersUI.combatButtonState.sizeOffset_X = -5;
			MenuPlayServersUI.combatButtonState.sizeOffset_Y = 30;
			MenuPlayServersUI.combatButtonState.sizeScale_X = 0.2f;
			MenuPlayServersUI.combatButtonState.state = (int)FilterSettings.filterCombat;
			SleekButtonState sleekButtonState8 = MenuPlayServersUI.combatButtonState;
			if (MenuPlayServersUI.<>f__mg$cache1A == null)
			{
				MenuPlayServersUI.<>f__mg$cache1A = new SwappedState(MenuPlayServersUI.onSwappedCombatState);
			}
			sleekButtonState8.onSwappedState = MenuPlayServersUI.<>f__mg$cache1A;
			MenuPlayServersUI.list.add(MenuPlayServersUI.combatButtonState);
			MenuPlayServersUI.modeButtonState = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(MenuPlayServersUI.localization.format("Easy_Button"), (Texture)MenuPlayServersUI.icons.load("Easy")),
				new GUIContent(MenuPlayServersUI.localization.format("Normal_Button"), (Texture)MenuPlayServersUI.icons.load("Normal")),
				new GUIContent(MenuPlayServersUI.localization.format("Hard_Button"), (Texture)MenuPlayServersUI.icons.load("Hard")),
				new GUIContent(MenuPlayServersUI.localization.format("Any_Mode_Button"))
			});
			MenuPlayServersUI.modeButtonState.positionOffset_X = 5;
			MenuPlayServersUI.modeButtonState.positionOffset_Y = -30;
			MenuPlayServersUI.modeButtonState.positionScale_X = 0.6f;
			MenuPlayServersUI.modeButtonState.positionScale_Y = 1f;
			MenuPlayServersUI.modeButtonState.sizeOffset_X = -10;
			MenuPlayServersUI.modeButtonState.sizeOffset_Y = 30;
			MenuPlayServersUI.modeButtonState.sizeScale_X = 0.2f;
			MenuPlayServersUI.modeButtonState.state = (int)FilterSettings.filterMode;
			SleekButtonState sleekButtonState9 = MenuPlayServersUI.modeButtonState;
			if (MenuPlayServersUI.<>f__mg$cache1B == null)
			{
				MenuPlayServersUI.<>f__mg$cache1B = new SwappedState(MenuPlayServersUI.onSwappedModeState);
			}
			sleekButtonState9.onSwappedState = MenuPlayServersUI.<>f__mg$cache1B;
			MenuPlayServersUI.list.add(MenuPlayServersUI.modeButtonState);
			MenuPlayServersUI.cameraButtonState = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(MenuPlayServersUI.localization.format("First_Button"), (Texture)MenuPlayServersUI.icons.load("First")),
				new GUIContent(MenuPlayServersUI.localization.format("Third_Button"), (Texture)MenuPlayServersUI.icons.load("Third")),
				new GUIContent(MenuPlayServersUI.localization.format("Both_Button"), (Texture)MenuPlayServersUI.icons.load("Both")),
				new GUIContent(MenuPlayServersUI.localization.format("Vehicle_Button"), (Texture)MenuPlayServersUI.icons.load("Vehicle")),
				new GUIContent(MenuPlayServersUI.localization.format("Any_Camera_Button"))
			});
			MenuPlayServersUI.cameraButtonState.positionOffset_X = 5;
			MenuPlayServersUI.cameraButtonState.positionOffset_Y = -30;
			MenuPlayServersUI.cameraButtonState.positionScale_X = 0.2f;
			MenuPlayServersUI.cameraButtonState.positionScale_Y = 1f;
			MenuPlayServersUI.cameraButtonState.sizeOffset_X = -10;
			MenuPlayServersUI.cameraButtonState.sizeOffset_Y = 30;
			MenuPlayServersUI.cameraButtonState.sizeScale_X = 0.2f;
			MenuPlayServersUI.cameraButtonState.state = (int)FilterSettings.filterCamera;
			SleekButtonState sleekButtonState10 = MenuPlayServersUI.cameraButtonState;
			if (MenuPlayServersUI.<>f__mg$cache1C == null)
			{
				MenuPlayServersUI.<>f__mg$cache1C = new SwappedState(MenuPlayServersUI.onSwappedCameraState);
			}
			sleekButtonState10.onSwappedState = MenuPlayServersUI.<>f__mg$cache1C;
			MenuPlayServersUI.list.add(MenuPlayServersUI.cameraButtonState);
			MenuPlayServersUI.serverBox = new SleekScrollBox();
			MenuPlayServersUI.serverBox.positionOffset_Y = 40;
			MenuPlayServersUI.serverBox.sizeOffset_Y = -160;
			MenuPlayServersUI.serverBox.sizeScale_X = 1f;
			MenuPlayServersUI.serverBox.sizeScale_Y = 1f;
			MenuPlayServersUI.serverBox.area = new Rect(0f, 0f, 5f, 0f);
			MenuPlayServersUI.list.add(MenuPlayServersUI.serverBox);
			MenuPlayServersUI.onLevelsRefreshed();
			Delegate onLevelsRefreshed = Level.onLevelsRefreshed;
			if (MenuPlayServersUI.<>f__mg$cache1D == null)
			{
				MenuPlayServersUI.<>f__mg$cache1D = new LevelsRefreshed(MenuPlayServersUI.onLevelsRefreshed);
			}
			Level.onLevelsRefreshed = (LevelsRefreshed)Delegate.Combine(onLevelsRefreshed, MenuPlayServersUI.<>f__mg$cache1D);
			MenuPlayServersUI.backButton = new SleekButtonIcon((Texture2D)MenuDashboardUI.icons.load("Exit"));
			MenuPlayServersUI.backButton.positionOffset_Y = -50;
			MenuPlayServersUI.backButton.positionScale_Y = 1f;
			MenuPlayServersUI.backButton.sizeOffset_X = 200;
			MenuPlayServersUI.backButton.sizeOffset_Y = 50;
			MenuPlayServersUI.backButton.text = MenuDashboardUI.localization.format("BackButtonText");
			MenuPlayServersUI.backButton.tooltip = MenuDashboardUI.localization.format("BackButtonTooltip");
			SleekButton sleekButton11 = MenuPlayServersUI.backButton;
			if (MenuPlayServersUI.<>f__mg$cache1E == null)
			{
				MenuPlayServersUI.<>f__mg$cache1E = new ClickedButton(MenuPlayServersUI.onClickedBackButton);
			}
			sleekButton11.onClickedButton = MenuPlayServersUI.<>f__mg$cache1E;
			MenuPlayServersUI.backButton.fontSize = 14;
			MenuPlayServersUI.backButton.iconImage.backgroundTint = ESleekTint.FOREGROUND;
			MenuPlayServersUI.container.add(MenuPlayServersUI.backButton);
		}

		public static void open()
		{
			if (MenuPlayServersUI.active)
			{
				return;
			}
			MenuPlayServersUI.active = true;
			MenuPlayServersUI.container.lerpPositionScale(0f, 0f, ESleekLerp.EXPONENTIAL, 20f);
		}

		public static void close()
		{
			if (!MenuPlayServersUI.active)
			{
				return;
			}
			MenuPlayServersUI.active = false;
			MenuSettings.save();
			MenuPlayServersUI.container.lerpPositionScale(0f, 1f, ESleekLerp.EXPONENTIAL, 20f);
		}

		private static void onClickedServer(SleekServer server, SteamServerInfo info)
		{
			if (info.isPro && !Provider.isPro)
			{
				return;
			}
			if (info.isPassworded && MenuPlayServersUI.passwordField.text == string.Empty)
			{
				return;
			}
			MenuSettings.save();
			MenuPlayServerInfoUI.open(info, MenuPlayServersUI.passwordField.text, MenuPlayServerInfoUI.EServerInfoOpenContext.SERVERS);
			MenuPlayServersUI.close();
		}

		private static void onMasterServerAdded(int insert, SteamServerInfo info)
		{
			if (insert > MenuPlayServersUI.serverButtons.Count)
			{
				return;
			}
			SleekServer sleekServer = new SleekServer(Provider.provider.matchmakingService.currentList, info);
			sleekServer.positionOffset_Y = insert * 40;
			sleekServer.sizeOffset_X = -30;
			sleekServer.sizeOffset_Y = 30;
			sleekServer.sizeScale_X = 1f;
			SleekServer sleekServer2 = sleekServer;
			if (MenuPlayServersUI.<>f__mg$cache0 == null)
			{
				MenuPlayServersUI.<>f__mg$cache0 = new ClickedServer(MenuPlayServersUI.onClickedServer);
			}
			sleekServer2.onClickedServer = MenuPlayServersUI.<>f__mg$cache0;
			MenuPlayServersUI.serverBox.add(sleekServer);
			for (int i = insert; i < MenuPlayServersUI.serverButtons.Count; i++)
			{
				MenuPlayServersUI.serverButtons[i].positionOffset_Y += 40;
			}
			MenuPlayServersUI.serverButtons.Insert(insert, sleekServer);
			MenuPlayServersUI.serverBox.area = new Rect(0f, 0f, 5f, (float)(MenuPlayServersUI.serverButtons.Count * 40 - 10));
		}

		private static void onMasterServerRemoved()
		{
			MenuPlayServersUI.infoBox.isVisible = false;
			MenuPlayServersUI.serverBox.remove();
			MenuPlayServersUI.serverButtons.Clear();
		}

		private static void onMasterServerResorted()
		{
			MenuPlayServersUI.infoBox.isVisible = false;
			MenuPlayServersUI.serverBox.remove();
			MenuPlayServersUI.serverButtons.Clear();
			for (int i = 0; i < Provider.provider.matchmakingService.serverList.Count; i++)
			{
				SteamServerInfo newInfo = Provider.provider.matchmakingService.serverList[i];
				SleekServer sleekServer = new SleekServer(Provider.provider.matchmakingService.currentList, newInfo);
				sleekServer.positionOffset_Y = i * 40;
				sleekServer.sizeOffset_X = -30;
				sleekServer.sizeOffset_Y = 30;
				sleekServer.sizeScale_X = 1f;
				SleekServer sleekServer2 = sleekServer;
				if (MenuPlayServersUI.<>f__mg$cache1 == null)
				{
					MenuPlayServersUI.<>f__mg$cache1 = new ClickedServer(MenuPlayServersUI.onClickedServer);
				}
				sleekServer2.onClickedServer = MenuPlayServersUI.<>f__mg$cache1;
				MenuPlayServersUI.serverBox.add(sleekServer);
				MenuPlayServersUI.serverButtons.Add(sleekServer);
			}
			MenuPlayServersUI.serverBox.area = new Rect(0f, 0f, 5f, (float)(Provider.provider.matchmakingService.serverList.Count * 40 - 10));
		}

		private static void onMasterServerRefreshed(EMatchMakingServerResponse response)
		{
			if (MenuPlayServersUI.serverButtons.Count == 0)
			{
				MenuPlayServersUI.infoBox.isVisible = true;
			}
		}

		private static void onClickedSortNameButton(SleekButton button)
		{
			if (Provider.provider.matchmakingService.serverInfoComparer is SteamServerInfoNameAscendingComparator)
			{
				Provider.provider.matchmakingService.sortMasterServer(new SteamServerInfoNameDescendingComparator());
			}
			else
			{
				Provider.provider.matchmakingService.sortMasterServer(new SteamServerInfoNameAscendingComparator());
			}
		}

		private static void onClickedSortMapButton(SleekButton button)
		{
			if (Provider.provider.matchmakingService.serverInfoComparer is SteamServerInfoMapAscendingComparator)
			{
				Provider.provider.matchmakingService.sortMasterServer(new SteamServerInfoMapDescendingComparator());
			}
			else
			{
				Provider.provider.matchmakingService.sortMasterServer(new SteamServerInfoMapAscendingComparator());
			}
		}

		private static void onClickedSortPlayersButton(SleekButton button)
		{
			if (Provider.provider.matchmakingService.serverInfoComparer is SteamServerInfoPlayersAscendingComparator)
			{
				Provider.provider.matchmakingService.sortMasterServer(new SteamServerInfoPlayersDescendingComparator());
			}
			else
			{
				Provider.provider.matchmakingService.sortMasterServer(new SteamServerInfoPlayersAscendingComparator());
			}
		}

		private static void onClickedSortPingButton(SleekButton button)
		{
			if (Provider.provider.matchmakingService.serverInfoComparer is SteamServerInfoPingAscendingComparator)
			{
				Provider.provider.matchmakingService.sortMasterServer(new SteamServerInfoPingDescendingComparator());
			}
			else
			{
				Provider.provider.matchmakingService.sortMasterServer(new SteamServerInfoPingAscendingComparator());
			}
		}

		private static void onClickedRefreshInternetButton(SleekButton button)
		{
			Provider.provider.matchmakingService.refreshMasterServer(ESteamServerList.INTERNET, FilterSettings.filterMap, FilterSettings.filterPassword, FilterSettings.filterWorkshop, FilterSettings.filterPlugins, FilterSettings.filterAttendance, FilterSettings.filterVACProtection, FilterSettings.filterBattlEyeProtection, false, FilterSettings.filterCombat, FilterSettings.filterCheats, FilterSettings.filterMode, FilterSettings.filterCamera);
		}

		private static void onClickedRefreshGoldButton(SleekButton button)
		{
			Provider.provider.matchmakingService.refreshMasterServer(ESteamServerList.INTERNET, FilterSettings.filterMap, FilterSettings.filterPassword, FilterSettings.filterWorkshop, FilterSettings.filterPlugins, FilterSettings.filterAttendance, FilterSettings.filterVACProtection, FilterSettings.filterBattlEyeProtection, true, FilterSettings.filterCombat, FilterSettings.filterCheats, FilterSettings.filterMode, FilterSettings.filterCamera);
		}

		private static void onClickedRefreshLANButton(SleekButton button)
		{
			Provider.provider.matchmakingService.refreshMasterServer(ESteamServerList.LAN, FilterSettings.filterMap, FilterSettings.filterPassword, FilterSettings.filterWorkshop, FilterSettings.filterPlugins, FilterSettings.filterAttendance, FilterSettings.filterVACProtection, FilterSettings.filterBattlEyeProtection, false, FilterSettings.filterCombat, FilterSettings.filterCheats, FilterSettings.filterMode, FilterSettings.filterCamera);
		}

		private static void onClickedRefreshHistoryButton(SleekButton button)
		{
			Provider.provider.matchmakingService.refreshMasterServer(ESteamServerList.HISTORY, FilterSettings.filterMap, FilterSettings.filterPassword, FilterSettings.filterWorkshop, FilterSettings.filterPlugins, FilterSettings.filterAttendance, FilterSettings.filterVACProtection, FilterSettings.filterBattlEyeProtection, false, FilterSettings.filterCombat, FilterSettings.filterCheats, FilterSettings.filterMode, FilterSettings.filterCamera);
		}

		private static void onClickedRefreshFavoritesButton(SleekButton button)
		{
			Provider.provider.matchmakingService.refreshMasterServer(ESteamServerList.FAVORITES, FilterSettings.filterMap, FilterSettings.filterPassword, FilterSettings.filterWorkshop, FilterSettings.filterPlugins, FilterSettings.filterAttendance, FilterSettings.filterVACProtection, FilterSettings.filterBattlEyeProtection, false, FilterSettings.filterCombat, FilterSettings.filterCheats, FilterSettings.filterMode, FilterSettings.filterCamera);
		}

		private static void onClickedRefreshFriendsButton(SleekButton button)
		{
			Provider.provider.matchmakingService.refreshMasterServer(ESteamServerList.FRIENDS, FilterSettings.filterMap, FilterSettings.filterPassword, FilterSettings.filterWorkshop, FilterSettings.filterPlugins, FilterSettings.filterAttendance, FilterSettings.filterVACProtection, FilterSettings.filterBattlEyeProtection, false, FilterSettings.filterCombat, FilterSettings.filterCheats, FilterSettings.filterMode, FilterSettings.filterCamera);
		}

		private static void onTypedNameField(SleekField field, string text)
		{
			PlaySettings.serversName = text;
		}

		private static void onTypedPasswordField(SleekField field, string text)
		{
			PlaySettings.serversPassword = text;
		}

		private static void onSwappedMapState(SleekButtonState button, int index)
		{
			if (index > 0)
			{
				FilterSettings.filterMap = MenuPlayServersUI.levels[index - 1].name;
			}
			else
			{
				FilterSettings.filterMap = string.Empty;
			}
		}

		private static void onSwappedPasswordState(SleekButtonState button, int index)
		{
			FilterSettings.filterPassword = (EPassword)index;
		}

		private static void onSwappedWorkshopState(SleekButtonState button, int index)
		{
			FilterSettings.filterWorkshop = (EWorkshop)index;
		}

		private static void onSwappedPluginsState(SleekButtonState button, int index)
		{
			FilterSettings.filterPlugins = (EPlugins)index;
		}

		private static void onSwappedCheatsState(SleekButtonState button, int index)
		{
			FilterSettings.filterCheats = (ECheats)index;
		}

		private static void onSwappedAttendanceState(SleekButtonState button, int index)
		{
			FilterSettings.filterAttendance = (EAttendance)index;
		}

		private static void onSwappedVACProtectionState(SleekButtonState button, int index)
		{
			FilterSettings.filterVACProtection = (EVACProtectionFilter)index;
		}

		private static void onSwappedBattlEyeProtectionState(SleekButtonState button, int index)
		{
			FilterSettings.filterBattlEyeProtection = (EBattlEyeProtectionFilter)index;
		}

		private static void onSwappedCombatState(SleekButtonState button, int index)
		{
			FilterSettings.filterCombat = (ECombat)index;
		}

		private static void onSwappedModeState(SleekButtonState button, int index)
		{
			FilterSettings.filterMode = (EGameMode)index;
		}

		private static void onSwappedCameraState(SleekButtonState button, int index)
		{
			FilterSettings.filterCamera = (ECameraMode)index;
		}

		private static void onLevelsRefreshed()
		{
			MenuPlayServersUI.levels = Level.getLevels(ESingleplayerMapCategory.ALL);
			GUIContent[] array = new GUIContent[MenuPlayServersUI.levels.Length + 1];
			array[0] = new GUIContent(MenuPlayServersUI.localization.format("Any_Map"));
			for (int i = 0; i < MenuPlayServersUI.levels.Length; i++)
			{
				LevelInfo levelInfo = MenuPlayServersUI.levels[i];
				if (levelInfo != null)
				{
					Local local = Localization.tryRead(levelInfo.path, false);
					if (local != null && local.has("Name"))
					{
						array[i + 1] = new GUIContent(local.format("Name"));
					}
					else
					{
						array[i + 1] = new GUIContent(levelInfo.name);
					}
				}
			}
			int num = -1;
			for (int j = 0; j < MenuPlayServersUI.levels.Length; j++)
			{
				LevelInfo levelInfo2 = MenuPlayServersUI.levels[j];
				if (levelInfo2 != null && levelInfo2.name == FilterSettings.filterMap)
				{
					num = j;
					break;
				}
			}
			if (num != -1 && MenuPlayServersUI.levels[num] != null)
			{
				FilterSettings.filterMap = MenuPlayServersUI.levels[num].name;
				num++;
			}
			else
			{
				FilterSettings.filterMap = string.Empty;
				num = 0;
			}
			MenuPlayServersUI.mapButtonState.setContent(array);
			MenuPlayServersUI.mapButtonState.state = num;
		}

		private static void onClickedBackButton(SleekButton button)
		{
			MenuPlayUI.open();
			MenuPlayServersUI.close();
		}

		public static Local localization;

		public static Bundle icons;

		private static Sleek container;

		private static Sleek list;

		public static bool active;

		private static SleekButtonIcon backButton;

		private static LevelInfo[] levels;

		private static SleekScrollBox serverBox;

		private static SleekBox infoBox;

		private static List<SleekServer> serverButtons;

		private static SleekButton sortName;

		private static SleekButton sortMap;

		private static SleekButton sortPlayers;

		private static SleekButton sortPing;

		private static SleekField nameField;

		private static SleekField passwordField;

		private static SleekButton refreshInternetButton;

		private static SleekButton refreshGoldButton;

		private static SleekButton refreshLANButton;

		private static SleekButton refreshHistoryButton;

		private static SleekButton refreshFavoritesButton;

		private static SleekButton refreshFriendsButton;

		private static SleekButtonState mapButtonState;

		private static SleekButtonState passwordButtonState;

		private static SleekButtonState workshopButtonState;

		private static SleekButtonState pluginsButtonState;

		private static SleekButtonState cheatsButtonState;

		private static SleekButtonState attendanceButtonState;

		private static SleekButtonState VACProtectionButtonState;

		private static SleekButtonState battlEyeProtectionButtonState;

		private static SleekButtonState combatButtonState;

		private static SleekButtonState modeButtonState;

		private static SleekButtonState cameraButtonState;

		[CompilerGenerated]
		private static ClickedServer <>f__mg$cache0;

		[CompilerGenerated]
		private static ClickedServer <>f__mg$cache1;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache2;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache3;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache4;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache5;

		[CompilerGenerated]
		private static TempSteamworksMatchmaking.MasterServerAdded <>f__mg$cache6;

		[CompilerGenerated]
		private static TempSteamworksMatchmaking.MasterServerRemoved <>f__mg$cache7;

		[CompilerGenerated]
		private static TempSteamworksMatchmaking.MasterServerResorted <>f__mg$cache8;

		[CompilerGenerated]
		private static TempSteamworksMatchmaking.MasterServerRefreshed <>f__mg$cache9;

		[CompilerGenerated]
		private static Typed <>f__mg$cacheA;

		[CompilerGenerated]
		private static Typed <>f__mg$cacheB;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cacheC;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cacheD;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cacheE;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cacheF;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache10;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache11;

		[CompilerGenerated]
		private static SwappedState <>f__mg$cache12;

		[CompilerGenerated]
		private static SwappedState <>f__mg$cache13;

		[CompilerGenerated]
		private static SwappedState <>f__mg$cache14;

		[CompilerGenerated]
		private static SwappedState <>f__mg$cache15;

		[CompilerGenerated]
		private static SwappedState <>f__mg$cache16;

		[CompilerGenerated]
		private static SwappedState <>f__mg$cache17;

		[CompilerGenerated]
		private static SwappedState <>f__mg$cache18;

		[CompilerGenerated]
		private static SwappedState <>f__mg$cache19;

		[CompilerGenerated]
		private static SwappedState <>f__mg$cache1A;

		[CompilerGenerated]
		private static SwappedState <>f__mg$cache1B;

		[CompilerGenerated]
		private static SwappedState <>f__mg$cache1C;

		[CompilerGenerated]
		private static LevelsRefreshed <>f__mg$cache1D;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache1E;
	}
}
