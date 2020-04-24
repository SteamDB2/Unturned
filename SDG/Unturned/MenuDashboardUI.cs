using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Newtonsoft.Json;
using SDG.Provider.Services.Web;
using SDG.SteamworksProvider.Services.Store;
using Steamworks;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;

namespace SDG.Unturned
{
	public class MenuDashboardUI
	{
		public MenuDashboardUI()
		{
			if (SteamUser.BLoggedOn())
			{
				IWebService webService = Provider.provider.webService;
				string url = "http://api.steampowered.com/ISteamNews/GetNewsForApp/v0002/";
				ERequestType requestType = ERequestType.GET;
				if (MenuDashboardUI.<>f__mg$cache0 == null)
				{
					MenuDashboardUI.<>f__mg$cache0 = new WebRequestReadyCallback(MenuDashboardUI.onWebRequestReady);
				}
				MenuDashboardUI.newsRequestHandle = webService.createRequest(url, requestType, MenuDashboardUI.<>f__mg$cache0);
				Provider.provider.webService.updateRequest(MenuDashboardUI.newsRequestHandle, "appid", "304930");
				Provider.provider.webService.updateRequest(MenuDashboardUI.newsRequestHandle, "count", Provider.statusData.News.Announcements_Count.ToString());
				Provider.provider.webService.updateRequest(MenuDashboardUI.newsRequestHandle, "feeds", "steam_community_announcements");
				Provider.provider.webService.submitRequest(MenuDashboardUI.newsRequestHandle);
				if (MenuDashboardUI.steamUGCQueryCompletedPopular == null)
				{
					if (MenuDashboardUI.<>f__mg$cache1 == null)
					{
						MenuDashboardUI.<>f__mg$cache1 = new CallResult<SteamUGCQueryCompleted_t>.APIDispatchDelegate(MenuDashboardUI.onSteamUGCQueryCompleted);
					}
					MenuDashboardUI.steamUGCQueryCompletedPopular = CallResult<SteamUGCQueryCompleted_t>.Create(MenuDashboardUI.<>f__mg$cache1);
				}
				if (MenuDashboardUI.steamUGCQueryCompletedFeatured == null)
				{
					if (MenuDashboardUI.<>f__mg$cache2 == null)
					{
						MenuDashboardUI.<>f__mg$cache2 = new CallResult<SteamUGCQueryCompleted_t>.APIDispatchDelegate(MenuDashboardUI.onSteamUGCQueryCompleted);
					}
					MenuDashboardUI.steamUGCQueryCompletedFeatured = CallResult<SteamUGCQueryCompleted_t>.Create(MenuDashboardUI.<>f__mg$cache2);
				}
				if (MenuDashboardUI.popularWorkshopHandle != UGCQueryHandle_t.Invalid)
				{
					SteamUGC.ReleaseQueryUGCRequest(MenuDashboardUI.popularWorkshopHandle);
					MenuDashboardUI.popularWorkshopHandle = UGCQueryHandle_t.Invalid;
				}
				if (OptionsSettings.featuredWorkshop)
				{
					if (Provider.statusData.News.Featured_Workshop != 0UL)
					{
						MenuDashboardUI.queryFeaturedItem((PublishedFileId_t)Provider.statusData.News.Featured_Workshop);
					}
					else
					{
						MenuDashboardUI.popularWorkshopHandle = SteamUGC.CreateQueryAllUGCRequest(3, 2, Provider.APP_ID, Provider.APP_ID, 1u);
						SteamUGC.SetRankedByTrendDays(MenuDashboardUI.popularWorkshopHandle, Provider.statusData.News.Popular_Workshop_Trend_Days);
						SteamUGC.SetReturnOnlyIDs(MenuDashboardUI.popularWorkshopHandle, true);
						SteamAPICall_t steamAPICall_t = SteamUGC.SendQueryUGCRequest(MenuDashboardUI.popularWorkshopHandle);
						MenuDashboardUI.steamUGCQueryCompletedPopular.Set(steamAPICall_t, null);
					}
				}
			}
			if (MenuDashboardUI.icons != null)
			{
				MenuDashboardUI.icons.unload();
			}
			MenuDashboardUI.localization = Localization.read("/Menu/MenuDashboard.dat");
			MenuDashboardUI.icons = Bundles.getBundle("/Bundles/Textures/Menu/Icons/MenuDashboard/MenuDashboard.unity3d");
			MenuDashboardUI.container = new Sleek();
			MenuDashboardUI.container.positionOffset_X = 10;
			MenuDashboardUI.container.positionOffset_Y = 10;
			MenuDashboardUI.container.sizeOffset_X = -20;
			MenuDashboardUI.container.sizeOffset_Y = -20;
			MenuDashboardUI.container.sizeScale_X = 1f;
			MenuDashboardUI.container.sizeScale_Y = 1f;
			MenuUI.container.add(MenuDashboardUI.container);
			MenuDashboardUI.active = true;
			MenuDashboardUI.playButton = new SleekButtonIcon((Texture2D)MenuDashboardUI.icons.load("Play"));
			MenuDashboardUI.playButton.positionOffset_Y = 170;
			MenuDashboardUI.playButton.sizeOffset_X = 200;
			MenuDashboardUI.playButton.sizeOffset_Y = 50;
			MenuDashboardUI.playButton.text = MenuDashboardUI.localization.format("PlayButtonText");
			MenuDashboardUI.playButton.tooltip = MenuDashboardUI.localization.format("PlayButtonTooltip");
			SleekButton sleekButton = MenuDashboardUI.playButton;
			if (MenuDashboardUI.<>f__mg$cache3 == null)
			{
				MenuDashboardUI.<>f__mg$cache3 = new ClickedButton(MenuDashboardUI.onClickedPlayButton);
			}
			sleekButton.onClickedButton = MenuDashboardUI.<>f__mg$cache3;
			MenuDashboardUI.playButton.fontSize = 14;
			MenuDashboardUI.playButton.iconImage.backgroundTint = ESleekTint.FOREGROUND;
			MenuDashboardUI.container.add(MenuDashboardUI.playButton);
			MenuDashboardUI.survivorsButton = new SleekButtonIcon((Texture2D)MenuDashboardUI.icons.load("Survivors"));
			MenuDashboardUI.survivorsButton.positionOffset_Y = 230;
			MenuDashboardUI.survivorsButton.sizeOffset_X = 200;
			MenuDashboardUI.survivorsButton.sizeOffset_Y = 50;
			MenuDashboardUI.survivorsButton.text = MenuDashboardUI.localization.format("SurvivorsButtonText");
			MenuDashboardUI.survivorsButton.tooltip = MenuDashboardUI.localization.format("SurvivorsButtonTooltip");
			SleekButton sleekButton2 = MenuDashboardUI.survivorsButton;
			if (MenuDashboardUI.<>f__mg$cache4 == null)
			{
				MenuDashboardUI.<>f__mg$cache4 = new ClickedButton(MenuDashboardUI.onClickedSurvivorsButton);
			}
			sleekButton2.onClickedButton = MenuDashboardUI.<>f__mg$cache4;
			MenuDashboardUI.survivorsButton.fontSize = 14;
			MenuDashboardUI.survivorsButton.iconImage.backgroundTint = ESleekTint.FOREGROUND;
			MenuDashboardUI.container.add(MenuDashboardUI.survivorsButton);
			MenuDashboardUI.configurationButton = new SleekButtonIcon((Texture2D)MenuDashboardUI.icons.load("Configuration"));
			MenuDashboardUI.configurationButton.positionOffset_Y = 290;
			MenuDashboardUI.configurationButton.sizeOffset_X = 200;
			MenuDashboardUI.configurationButton.sizeOffset_Y = 50;
			MenuDashboardUI.configurationButton.text = MenuDashboardUI.localization.format("ConfigurationButtonText");
			MenuDashboardUI.configurationButton.tooltip = MenuDashboardUI.localization.format("ConfigurationButtonTooltip");
			SleekButton sleekButton3 = MenuDashboardUI.configurationButton;
			if (MenuDashboardUI.<>f__mg$cache5 == null)
			{
				MenuDashboardUI.<>f__mg$cache5 = new ClickedButton(MenuDashboardUI.onClickedConfigurationButton);
			}
			sleekButton3.onClickedButton = MenuDashboardUI.<>f__mg$cache5;
			MenuDashboardUI.configurationButton.fontSize = 14;
			MenuDashboardUI.configurationButton.iconImage.backgroundTint = ESleekTint.FOREGROUND;
			MenuDashboardUI.container.add(MenuDashboardUI.configurationButton);
			MenuDashboardUI.workshopButton = new SleekButtonIcon((Texture2D)MenuDashboardUI.icons.load("Workshop"));
			MenuDashboardUI.workshopButton.positionOffset_Y = 350;
			MenuDashboardUI.workshopButton.sizeOffset_X = 200;
			MenuDashboardUI.workshopButton.sizeOffset_Y = 50;
			MenuDashboardUI.workshopButton.text = MenuDashboardUI.localization.format("WorkshopButtonText");
			MenuDashboardUI.workshopButton.tooltip = MenuDashboardUI.localization.format("WorkshopButtonTooltip");
			SleekButton sleekButton4 = MenuDashboardUI.workshopButton;
			if (MenuDashboardUI.<>f__mg$cache6 == null)
			{
				MenuDashboardUI.<>f__mg$cache6 = new ClickedButton(MenuDashboardUI.onClickedWorkshopButton);
			}
			sleekButton4.onClickedButton = MenuDashboardUI.<>f__mg$cache6;
			MenuDashboardUI.workshopButton.fontSize = 14;
			MenuDashboardUI.workshopButton.iconImage.backgroundTint = ESleekTint.FOREGROUND;
			MenuDashboardUI.container.add(MenuDashboardUI.workshopButton);
			MenuDashboardUI.exitButton = new SleekButtonIcon((Texture2D)MenuDashboardUI.icons.load("Exit"));
			MenuDashboardUI.exitButton.positionOffset_Y = -50;
			MenuDashboardUI.exitButton.positionScale_Y = 1f;
			MenuDashboardUI.exitButton.sizeOffset_X = 200;
			MenuDashboardUI.exitButton.sizeOffset_Y = 50;
			MenuDashboardUI.exitButton.text = MenuDashboardUI.localization.format("ExitButtonText");
			MenuDashboardUI.exitButton.tooltip = MenuDashboardUI.localization.format("ExitButtonTooltip");
			SleekButton sleekButton5 = MenuDashboardUI.exitButton;
			if (MenuDashboardUI.<>f__mg$cache7 == null)
			{
				MenuDashboardUI.<>f__mg$cache7 = new ClickedButton(MenuDashboardUI.onClickedExitButton);
			}
			sleekButton5.onClickedButton = MenuDashboardUI.<>f__mg$cache7;
			MenuDashboardUI.exitButton.fontSize = 14;
			MenuDashboardUI.exitButton.iconImage.backgroundTint = ESleekTint.FOREGROUND;
			MenuDashboardUI.container.add(MenuDashboardUI.exitButton);
			MenuDashboardUI.canvas = GameObject.Find("Canvas");
			MenuDashboardUI.templateNews = MenuDashboardUI.canvas.transform.FindChild("View").FindChild("List").FindChild("Template_News").gameObject;
			MenuDashboardUI.templateWorkshop = MenuDashboardUI.canvas.transform.FindChild("View").FindChild("List").FindChild("Template_Workshop").gameObject;
			MenuDashboardUI.templateText = MenuDashboardUI.templateNews.transform.parent.FindChild("Template_Text").gameObject;
			MenuDashboardUI.templateImage = MenuDashboardUI.templateNews.transform.parent.FindChild("Template_Image").gameObject;
			MenuDashboardUI.templateLink = MenuDashboardUI.templateNews.transform.parent.FindChild("Template_Link").gameObject;
			MenuDashboardUI.templateReadMoreLink = MenuDashboardUI.templateNews.transform.parent.FindChild("Template_ReadMore_Link").gameObject;
			MenuDashboardUI.templateReadMoreContent = MenuDashboardUI.templateNews.transform.parent.FindChild("Template_ReadMore_Content").gameObject;
			if (!Provider.isPro)
			{
				MenuDashboardUI.proButton = new SleekButton();
				MenuDashboardUI.proButton.positionOffset_X = 210;
				MenuDashboardUI.proButton.positionOffset_Y = -100;
				MenuDashboardUI.proButton.positionScale_Y = 1f;
				MenuDashboardUI.proButton.sizeOffset_Y = 100;
				MenuDashboardUI.proButton.sizeOffset_X = -210;
				MenuDashboardUI.proButton.sizeScale_X = 1f;
				MenuDashboardUI.proButton.tooltip = MenuDashboardUI.localization.format("Pro_Button_Tooltip");
				MenuDashboardUI.proButton.backgroundColor = Palette.PRO;
				MenuDashboardUI.proButton.foregroundColor = Palette.PRO;
				MenuDashboardUI.proButton.backgroundTint = ESleekTint.NONE;
				MenuDashboardUI.proButton.foregroundTint = ESleekTint.NONE;
				SleekButton sleekButton6 = MenuDashboardUI.proButton;
				if (MenuDashboardUI.<>f__mg$cache8 == null)
				{
					MenuDashboardUI.<>f__mg$cache8 = new ClickedButton(MenuDashboardUI.onClickedProButton);
				}
				sleekButton6.onClickedButton = MenuDashboardUI.<>f__mg$cache8;
				MenuDashboardUI.container.add(MenuDashboardUI.proButton);
				MenuDashboardUI.proLabel = new SleekLabel();
				MenuDashboardUI.proLabel.sizeScale_X = 1f;
				MenuDashboardUI.proLabel.sizeOffset_Y = 50;
				MenuDashboardUI.proLabel.text = MenuDashboardUI.localization.format("Pro_Title");
				MenuDashboardUI.proLabel.foregroundColor = Palette.PRO;
				MenuDashboardUI.proLabel.fontSize = 18;
				MenuDashboardUI.proLabel.foregroundTint = ESleekTint.NONE;
				MenuDashboardUI.proButton.add(MenuDashboardUI.proLabel);
				MenuDashboardUI.featureLabel = new SleekLabel();
				MenuDashboardUI.featureLabel.positionOffset_Y = 50;
				MenuDashboardUI.featureLabel.sizeOffset_Y = -50;
				MenuDashboardUI.featureLabel.sizeScale_X = 1f;
				MenuDashboardUI.featureLabel.sizeScale_Y = 1f;
				MenuDashboardUI.featureLabel.text = MenuDashboardUI.localization.format("Pro_Button");
				MenuDashboardUI.featureLabel.foregroundColor = Palette.PRO;
				MenuDashboardUI.featureLabel.foregroundTint = ESleekTint.NONE;
				MenuDashboardUI.proButton.add(MenuDashboardUI.featureLabel);
				MenuDashboardUI.canvas.transform.FindChild("View").GetComponent<RectTransform>().offsetMin += new Vector2(0f, 110f);
				MenuDashboardUI.canvas.transform.FindChild("Scrollbar").GetComponent<RectTransform>().offsetMin += new Vector2(0f, 110f);
			}
			int num = 0;
			if (!string.IsNullOrEmpty(Provider.statusData.Alert.Header))
			{
				if (!string.IsNullOrEmpty(Provider.statusData.Alert.Link))
				{
					SleekButton sleekButton7 = new SleekButton();
					SleekButton sleekButton8 = sleekButton7;
					if (MenuDashboardUI.<>f__mg$cache9 == null)
					{
						MenuDashboardUI.<>f__mg$cache9 = new ClickedButton(MenuDashboardUI.onClickedAlertButton);
					}
					sleekButton8.onClickedButton = MenuDashboardUI.<>f__mg$cache9;
					MenuDashboardUI.alertBox = sleekButton7;
				}
				else
				{
					MenuDashboardUI.alertBox = new SleekBox();
				}
				MenuDashboardUI.alertBox.positionOffset_X = 210;
				MenuDashboardUI.alertBox.positionOffset_Y = 170 + num;
				MenuDashboardUI.alertBox.sizeOffset_Y = 60;
				MenuDashboardUI.alertBox.sizeOffset_X = -210;
				MenuDashboardUI.alertBox.sizeScale_X = 1f;
				MenuDashboardUI.container.add(MenuDashboardUI.alertBox);
				MenuDashboardUI.alertIcon = new SleekImageTexture();
				MenuDashboardUI.alertIcon.positionOffset_X = 10;
				MenuDashboardUI.alertIcon.positionOffset_Y = 10;
				MenuDashboardUI.alertIcon.sizeOffset_X = 40;
				MenuDashboardUI.alertIcon.sizeOffset_Y = 40;
				MenuDashboardUI.alertIcon.texture = (Texture2D)MenuDashboardUI.icons.load("Alert");
				MenuDashboardUI.alertIcon.backgroundColor = Palette.hex(Provider.statusData.Alert.Color);
				MenuDashboardUI.alertIcon.foregroundTint = ESleekTint.NONE;
				MenuDashboardUI.alertBox.add(MenuDashboardUI.alertIcon);
				MenuDashboardUI.alertHeaderLabel = new SleekLabel();
				MenuDashboardUI.alertHeaderLabel.positionOffset_X = 60;
				MenuDashboardUI.alertHeaderLabel.sizeScale_X = 1f;
				MenuDashboardUI.alertHeaderLabel.sizeOffset_X = -60;
				MenuDashboardUI.alertHeaderLabel.sizeOffset_Y = 30;
				MenuDashboardUI.alertHeaderLabel.text = Provider.statusData.Alert.Header;
				MenuDashboardUI.alertHeaderLabel.foregroundColor = Palette.hex(Provider.statusData.Alert.Color);
				MenuDashboardUI.alertHeaderLabel.fontSize = 14;
				MenuDashboardUI.alertHeaderLabel.foregroundTint = ESleekTint.NONE;
				MenuDashboardUI.alertBox.add(MenuDashboardUI.alertHeaderLabel);
				MenuDashboardUI.alertBodyLabel = new SleekLabel();
				MenuDashboardUI.alertBodyLabel.positionOffset_X = 60;
				MenuDashboardUI.alertBodyLabel.positionOffset_Y = 20;
				MenuDashboardUI.alertBodyLabel.sizeOffset_X = -60;
				MenuDashboardUI.alertBodyLabel.sizeOffset_Y = -20;
				MenuDashboardUI.alertBodyLabel.sizeScale_X = 1f;
				MenuDashboardUI.alertBodyLabel.sizeScale_Y = 1f;
				MenuDashboardUI.alertBodyLabel.text = Provider.statusData.Alert.Body;
				MenuDashboardUI.alertBodyLabel.foregroundTint = ESleekTint.NONE;
				MenuDashboardUI.alertBox.add(MenuDashboardUI.alertBodyLabel);
				MenuDashboardUI.canvas.transform.FindChild("View").GetComponent<RectTransform>().offsetMax -= new Vector2(0f, 70f);
				MenuDashboardUI.canvas.transform.FindChild("Scrollbar").GetComponent<RectTransform>().offsetMax -= new Vector2(0f, 70f);
			}
			new MenuPauseUI();
			new MenuCreditsUI();
			new MenuTitleUI();
			new MenuPlayUI();
			new MenuSurvivorsUI();
			new MenuConfigurationUI();
			new MenuWorkshopUI();
			if (Provider.connectionFailureInfo != ESteamConnectionFailureInfo.NONE)
			{
				ESteamConnectionFailureInfo connectionFailureInfo = Provider.connectionFailureInfo;
				string connectionFailureReason = Provider.connectionFailureReason;
				uint connectionFailureDuration = Provider.connectionFailureDuration;
				Provider.resetConnectionFailure();
				if (connectionFailureInfo == ESteamConnectionFailureInfo.BANNED)
				{
					MenuUI.alert(MenuDashboardUI.localization.format("Banned", new object[]
					{
						connectionFailureDuration,
						connectionFailureReason
					}));
				}
				else if (connectionFailureInfo == ESteamConnectionFailureInfo.KICKED)
				{
					MenuUI.alert(MenuDashboardUI.localization.format("Kicked", new object[]
					{
						connectionFailureReason
					}));
				}
				else if (connectionFailureInfo == ESteamConnectionFailureInfo.WHITELISTED)
				{
					MenuUI.alert(MenuDashboardUI.localization.format("Whitelisted"));
				}
				else if (connectionFailureInfo == ESteamConnectionFailureInfo.PASSWORD)
				{
					MenuUI.alert(MenuDashboardUI.localization.format("Password"));
				}
				else if (connectionFailureInfo == ESteamConnectionFailureInfo.FULL)
				{
					MenuUI.alert(MenuDashboardUI.localization.format("Full"));
				}
				else if (connectionFailureInfo == ESteamConnectionFailureInfo.HASH_LEVEL)
				{
					MenuUI.alert(MenuDashboardUI.localization.format("Hash_Level"));
				}
				else if (connectionFailureInfo == ESteamConnectionFailureInfo.HASH_ASSEMBLY)
				{
					MenuUI.alert(MenuDashboardUI.localization.format("Hash_Assembly"));
				}
				else if (connectionFailureInfo == ESteamConnectionFailureInfo.VERSION)
				{
					MenuUI.alert(MenuDashboardUI.localization.format("Version"));
				}
				else if (connectionFailureInfo == ESteamConnectionFailureInfo.PRO_SERVER)
				{
					MenuUI.alert(MenuDashboardUI.localization.format("Pro_Server"));
				}
				else if (connectionFailureInfo == ESteamConnectionFailureInfo.PRO_CHARACTER)
				{
					MenuUI.alert(MenuDashboardUI.localization.format("Pro_Character"));
				}
				else if (connectionFailureInfo == ESteamConnectionFailureInfo.PRO_DESYNC)
				{
					MenuUI.alert(MenuDashboardUI.localization.format("Pro_Desync"));
				}
				else if (connectionFailureInfo == ESteamConnectionFailureInfo.PRO_APPEARANCE)
				{
					MenuUI.alert(MenuDashboardUI.localization.format("Pro_Appearance"));
				}
				else if (connectionFailureInfo == ESteamConnectionFailureInfo.AUTH_VERIFICATION)
				{
					MenuUI.alert(MenuDashboardUI.localization.format("Auth_Verification"));
				}
				else if (connectionFailureInfo == ESteamConnectionFailureInfo.AUTH_NO_STEAM)
				{
					MenuUI.alert(MenuDashboardUI.localization.format("Auth_No_Steam"));
				}
				else if (connectionFailureInfo == ESteamConnectionFailureInfo.AUTH_LICENSE_EXPIRED)
				{
					MenuUI.alert(MenuDashboardUI.localization.format("Auth_License_Expired"));
				}
				else if (connectionFailureInfo == ESteamConnectionFailureInfo.AUTH_VAC_BAN)
				{
					MenuUI.alert(MenuDashboardUI.localization.format("Auth_VAC_Ban"));
				}
				else if (connectionFailureInfo == ESteamConnectionFailureInfo.AUTH_ELSEWHERE)
				{
					MenuUI.alert(MenuDashboardUI.localization.format("Auth_Elsewhere"));
				}
				else if (connectionFailureInfo == ESteamConnectionFailureInfo.AUTH_TIMED_OUT)
				{
					MenuUI.alert(MenuDashboardUI.localization.format("Auth_Timed_Out"));
				}
				else if (connectionFailureInfo == ESteamConnectionFailureInfo.AUTH_USED)
				{
					MenuUI.alert(MenuDashboardUI.localization.format("Auth_Used"));
				}
				else if (connectionFailureInfo == ESteamConnectionFailureInfo.AUTH_NO_USER)
				{
					MenuUI.alert(MenuDashboardUI.localization.format("Auth_No_User"));
				}
				else if (connectionFailureInfo == ESteamConnectionFailureInfo.AUTH_PUB_BAN)
				{
					MenuUI.alert(MenuDashboardUI.localization.format("Auth_Pub_Ban"));
				}
				else if (connectionFailureInfo == ESteamConnectionFailureInfo.AUTH_ECON_SERIALIZE)
				{
					MenuUI.alert(MenuDashboardUI.localization.format("Auth_Econ_Serialize"));
				}
				else if (connectionFailureInfo == ESteamConnectionFailureInfo.AUTH_ECON_DESERIALIZE)
				{
					MenuUI.alert(MenuDashboardUI.localization.format("Auth_Econ_Deserialize"));
				}
				else if (connectionFailureInfo == ESteamConnectionFailureInfo.AUTH_ECON_VERIFY)
				{
					MenuUI.alert(MenuDashboardUI.localization.format("Auth_Econ_Verify"));
				}
				else if (connectionFailureInfo == ESteamConnectionFailureInfo.AUTH_EMPTY)
				{
					MenuUI.alert(MenuDashboardUI.localization.format("Auth_Empty"));
				}
				else if (connectionFailureInfo == ESteamConnectionFailureInfo.ALREADY_CONNECTED)
				{
					MenuUI.alert(MenuDashboardUI.localization.format("Already_Connected"));
				}
				else if (connectionFailureInfo == ESteamConnectionFailureInfo.ALREADY_PENDING)
				{
					MenuUI.alert(MenuDashboardUI.localization.format("Already_Pending"));
				}
				else if (connectionFailureInfo == ESteamConnectionFailureInfo.LATE_PENDING)
				{
					MenuUI.alert(MenuDashboardUI.localization.format("Late_Pending"));
				}
				else if (connectionFailureInfo == ESteamConnectionFailureInfo.NOT_PENDING)
				{
					MenuUI.alert(MenuDashboardUI.localization.format("Not_Pending"));
				}
				else if (connectionFailureInfo == ESteamConnectionFailureInfo.NAME_PLAYER_SHORT)
				{
					MenuUI.alert(MenuDashboardUI.localization.format("Name_Player_Short"));
				}
				else if (connectionFailureInfo == ESteamConnectionFailureInfo.NAME_PLAYER_LONG)
				{
					MenuUI.alert(MenuDashboardUI.localization.format("Name_Player_Long"));
				}
				else if (connectionFailureInfo == ESteamConnectionFailureInfo.NAME_PLAYER_INVALID)
				{
					MenuUI.alert(MenuDashboardUI.localization.format("Name_Player_Invalid"));
				}
				else if (connectionFailureInfo == ESteamConnectionFailureInfo.NAME_PLAYER_NUMBER)
				{
					MenuUI.alert(MenuDashboardUI.localization.format("Name_Player_Number"));
				}
				else if (connectionFailureInfo == ESteamConnectionFailureInfo.NAME_CHARACTER_SHORT)
				{
					MenuUI.alert(MenuDashboardUI.localization.format("Name_Character_Short"));
				}
				else if (connectionFailureInfo == ESteamConnectionFailureInfo.NAME_CHARACTER_LONG)
				{
					MenuUI.alert(MenuDashboardUI.localization.format("Name_Character_Long"));
				}
				else if (connectionFailureInfo == ESteamConnectionFailureInfo.NAME_CHARACTER_INVALID)
				{
					MenuUI.alert(MenuDashboardUI.localization.format("Name_Character_Invalid"));
				}
				else if (connectionFailureInfo == ESteamConnectionFailureInfo.NAME_CHARACTER_NUMBER)
				{
					MenuUI.alert(MenuDashboardUI.localization.format("Name_Character_Number"));
				}
				else if (connectionFailureInfo == ESteamConnectionFailureInfo.TIMED_OUT)
				{
					MenuUI.alert(MenuDashboardUI.localization.format("Timed_Out"));
				}
				else if (connectionFailureInfo == ESteamConnectionFailureInfo.MAP)
				{
					MenuUI.alert(MenuDashboardUI.localization.format("Map"));
				}
				else if (connectionFailureInfo == ESteamConnectionFailureInfo.SHUTDOWN)
				{
					MenuUI.alert(MenuDashboardUI.localization.format("Shutdown"));
				}
				else if (connectionFailureInfo == ESteamConnectionFailureInfo.PING)
				{
					MenuUI.alert(MenuDashboardUI.localization.format("Ping"));
				}
				else if (connectionFailureInfo == ESteamConnectionFailureInfo.PLUGIN)
				{
					MenuUI.alert(MenuDashboardUI.localization.format("Plugin"));
				}
				else if (connectionFailureInfo == ESteamConnectionFailureInfo.BARRICADE)
				{
					MenuUI.alert(MenuDashboardUI.localization.format("Barricade"));
				}
				else if (connectionFailureInfo == ESteamConnectionFailureInfo.STRUCTURE)
				{
					MenuUI.alert(MenuDashboardUI.localization.format("Structure"));
				}
				else if (connectionFailureInfo == ESteamConnectionFailureInfo.VEHICLE)
				{
					MenuUI.alert(MenuDashboardUI.localization.format("Vehicle"));
				}
				else if (connectionFailureInfo == ESteamConnectionFailureInfo.CLIENT_MODULE_DESYNC)
				{
					MenuUI.alert(MenuDashboardUI.localization.format("Client_Module_Desync"));
				}
				else if (connectionFailureInfo == ESteamConnectionFailureInfo.SERVER_MODULE_DESYNC)
				{
					MenuUI.alert(MenuDashboardUI.localization.format("Server_Module_Desync"));
				}
				else if (connectionFailureInfo == ESteamConnectionFailureInfo.BATTLEYE_BROKEN)
				{
					MenuUI.alert(MenuDashboardUI.localization.format("BattlEye_Broken"), 10f);
				}
				else if (connectionFailureInfo == ESteamConnectionFailureInfo.BATTLEYE_UPDATE)
				{
					MenuUI.alert(MenuDashboardUI.localization.format("BattlEye_Update"));
				}
				else if (connectionFailureInfo == ESteamConnectionFailureInfo.BATTLEYE_UNKNOWN)
				{
					MenuUI.alert(MenuDashboardUI.localization.format("BattlEye_Unknown"));
				}
			}
		}

		public static void setCanvasActive(bool isActive)
		{
			MenuDashboardUI.canvasActive = isActive;
			if (MenuDashboardUI.canvas == null)
			{
				return;
			}
			MenuDashboardUI.canvas.GetComponent<Canvas>().enabled = (MenuDashboardUI.active && MenuDashboardUI.canvasActive);
		}

		public static void open()
		{
			if (MenuDashboardUI.active)
			{
				return;
			}
			MenuDashboardUI.active = true;
			MenuDashboardUI.setCanvasActive(true);
			MenuDashboardUI.container.lerpPositionScale(0f, 0f, ESleekLerp.EXPONENTIAL, 20f);
		}

		public static void close()
		{
			if (!MenuDashboardUI.active)
			{
				return;
			}
			MenuDashboardUI.active = false;
			MenuDashboardUI.setCanvasActive(false);
			MenuDashboardUI.container.lerpPositionScale(0f, 1f, ESleekLerp.EXPONENTIAL, 20f);
		}

		private static void onClickedPlayButton(SleekButton button)
		{
			MenuPlayUI.open();
			MenuDashboardUI.close();
			MenuTitleUI.close();
		}

		private static void onClickedSurvivorsButton(SleekButton button)
		{
			MenuSurvivorsUI.open();
			MenuDashboardUI.close();
			MenuTitleUI.close();
		}

		private static void onClickedConfigurationButton(SleekButton button)
		{
			MenuConfigurationUI.open();
			MenuDashboardUI.close();
			MenuTitleUI.close();
		}

		private static void onClickedWorkshopButton(SleekButton button)
		{
			MenuWorkshopUI.open();
			MenuDashboardUI.close();
			MenuTitleUI.close();
		}

		private static void onClickedExitButton(SleekButton button)
		{
			MenuPauseUI.open();
			MenuDashboardUI.close();
			MenuTitleUI.close();
		}

		private static void onClickedProButton(SleekButton button)
		{
			if (!Provider.provider.storeService.canOpenStore)
			{
				MenuUI.alert(MenuSurvivorsCharacterUI.localization.format("Overlay"));
				return;
			}
			Provider.provider.storeService.open(new SteamworksStorePackageID(Provider.PRO_ID.m_AppId));
			Analytics.CustomEvent("Link_Gold", null);
		}

		private static void onClickedAlertButton(SleekButton button)
		{
			if (!Provider.provider.browserService.canOpenBrowser)
			{
				MenuUI.alert(MenuSurvivorsCharacterUI.localization.format("Overlay"));
				return;
			}
			Provider.provider.browserService.open(Provider.statusData.Alert.Link);
			Analytics.CustomEvent("Link_Alert", null);
		}

		private static void filterContent(string header, string source, ref string contents, ref int lines)
		{
			int num = source.IndexOf("[b]" + header + ":[/b]");
			if (num != -1)
			{
				contents = contents + "<i>" + header + "</i>:\n";
				lines += 2;
				int num2 = source.IndexOf("[list]", num);
				int num3 = source.IndexOf("[/list]", num2);
				string text = source.Substring(num2 + 6, num3 - (num2 + 6));
				string[] array = text.Split(new string[]
				{
					"[*]"
				}, StringSplitOptions.RemoveEmptyEntries);
				foreach (string text2 in array)
				{
					if (text2.Length > 0)
					{
						contents += text2;
						lines++;
					}
				}
				contents += "\n";
				lines++;
			}
		}

		private static void insertContent(GameObject news, string contents)
		{
			int num = 0;
			for (;;)
			{
				int num2 = contents.IndexOf("[h1]", num + 1);
				int num3 = contents.IndexOf("[b]", num + 1);
				int num4;
				if (num2 == -1 && num3 == -1)
				{
					num4 = -1;
				}
				else if (num2 == -1)
				{
					num4 = num3;
				}
				else if (num3 == -1)
				{
					num4 = num2;
				}
				else if (num2 < num3)
				{
					num4 = num2;
				}
				else
				{
					num4 = num3;
				}
				string text;
				if (num4 == -1)
				{
					text = contents.Substring(num);
				}
				else
				{
					text = contents.Substring(num, num4 - num);
				}
				List<SubcontentInfo> list = new List<SubcontentInfo>();
				int num5 = 0;
				for (;;)
				{
					int num6 = text.IndexOf("[img]", num5);
					int num7 = text.IndexOf("[url=", num5);
					if (num6 == -1 && num7 == -1)
					{
						break;
					}
					int num8;
					bool flag;
					if (num6 == -1)
					{
						num8 = num7;
						flag = false;
					}
					else if (num7 == -1)
					{
						num8 = num6;
						flag = true;
					}
					else if (num6 < num7)
					{
						num8 = num6;
						flag = true;
					}
					else
					{
						num8 = num7;
						flag = false;
					}
					list.Add(new SubcontentInfo
					{
						content = text.Substring(num5, num8 - num5)
					});
					int num10;
					if (flag)
					{
						int num9 = text.IndexOf("[/img]", num6);
						string url = text.Substring(num6 + 5, num9 - num6 - 5);
						list.Add(new SubcontentInfo
						{
							url = url,
							isImage = true
						});
						num10 = num9;
					}
					else
					{
						int num11 = text.IndexOf("[/url]", num7);
						int num12 = text.IndexOf("]", num7);
						string url2 = text.Substring(num7 + 5, num12 - num7 - 5);
						string content = text.Substring(num12 + 1, num11 - num12 - 1);
						list.Add(new SubcontentInfo
						{
							content = content,
							url = url2,
							isLink = true
						});
						num10 = num11;
					}
					num5 = num10 + 6;
				}
				list.Add(new SubcontentInfo
				{
					content = text.Substring(num5)
				});
				for (int i = 0; i < list.Count; i++)
				{
					SubcontentInfo subcontentInfo = list[i];
					if (subcontentInfo.isImage)
					{
						GameObject gameObject = Object.Instantiate<GameObject>(MenuDashboardUI.templateImage);
						gameObject.name = "Image";
						gameObject.GetComponent<RectTransform>().SetParent(news.transform, true);
						gameObject.SetActive(true);
						gameObject.GetComponent<WebImage>().url = subcontentInfo.url;
					}
					else if (subcontentInfo.isLink)
					{
						GameObject gameObject2 = Object.Instantiate<GameObject>(MenuDashboardUI.templateLink);
						gameObject2.name = "Link";
						gameObject2.GetComponent<RectTransform>().SetParent(news.transform, true);
						gameObject2.SetActive(true);
						gameObject2.GetComponent<Text>().text = subcontentInfo.content;
						gameObject2.GetComponent<WebLink>().url = subcontentInfo.url;
					}
					else
					{
						subcontentInfo.content = subcontentInfo.content.TrimStart(new char[]
						{
							'\r',
							'\n'
						});
						subcontentInfo.content = subcontentInfo.content.Replace("[b]", "<b>");
						subcontentInfo.content = subcontentInfo.content.Replace("[/b]", "</b>");
						subcontentInfo.content = subcontentInfo.content.Replace("[i]", "<i>");
						subcontentInfo.content = subcontentInfo.content.Replace("[/i]", "</i>");
						subcontentInfo.content = subcontentInfo.content.Replace("[list]", string.Empty);
						subcontentInfo.content = subcontentInfo.content.Replace("[/list]", string.Empty);
						subcontentInfo.content = subcontentInfo.content.Replace("[*]", "- ");
						subcontentInfo.content = subcontentInfo.content.Replace("[h1]", "<color=#5aa9d6><size=18>");
						subcontentInfo.content = subcontentInfo.content.Replace("[/h1]", "</size></color>");
						subcontentInfo.content = subcontentInfo.content.TrimEnd(new char[]
						{
							'\r',
							'\n'
						});
						if (!string.IsNullOrEmpty(subcontentInfo.content))
						{
							string[] array = subcontentInfo.content.Split(new char[]
							{
								'\r',
								'\n'
							});
							string text2 = string.Empty;
							foreach (string text3 in array)
							{
								string text4 = text3.Trim();
								if (!string.IsNullOrEmpty(text4))
								{
									if (text4.StartsWith("- "))
									{
										if (!string.IsNullOrEmpty(text2))
										{
											text2 += '\n';
										}
										text2 += text4;
									}
									else
									{
										if (!string.IsNullOrEmpty(text2))
										{
											GameObject gameObject3 = Object.Instantiate<GameObject>(MenuDashboardUI.templateText);
											gameObject3.name = "Text";
											gameObject3.GetComponent<RectTransform>().SetParent(news.transform, true);
											gameObject3.SetActive(true);
											gameObject3.GetComponent<Text>().text = text2;
										}
										text2 = text4;
										GameObject gameObject4 = Object.Instantiate<GameObject>(MenuDashboardUI.templateText);
										gameObject4.name = "Text";
										gameObject4.GetComponent<RectTransform>().SetParent(news.transform, true);
										gameObject4.SetActive(true);
										gameObject4.GetComponent<Text>().text = text2;
										text2 = string.Empty;
									}
								}
							}
							if (!string.IsNullOrEmpty(text2))
							{
								GameObject gameObject5 = Object.Instantiate<GameObject>(MenuDashboardUI.templateText);
								gameObject5.name = "Text";
								gameObject5.GetComponent<RectTransform>().SetParent(news.transform, true);
								gameObject5.SetActive(true);
								gameObject5.GetComponent<Text>().text = text2;
							}
						}
					}
				}
				if (num4 == -1)
				{
					break;
				}
				num = num4;
			}
		}

		private static void onWebRequestReady(IWebRequestHandle webRequestHandle)
		{
			if (webRequestHandle == MenuDashboardUI.newsRequestHandle)
			{
				uint responseBodySize = Provider.provider.webService.getResponseBodySize(MenuDashboardUI.newsRequestHandle);
				byte[] array = new byte[responseBodySize];
				Provider.provider.webService.getResponseBodyData(MenuDashboardUI.newsRequestHandle, array, responseBodySize);
				string @string = Encoding.UTF8.GetString(array);
				MenuDashboardUI.newsResponse = JsonConvert.DeserializeObject<NewsResponse>(@string);
				for (int i = 0; i < MenuDashboardUI.newsResponse.AppNews.NewsItems.Length; i++)
				{
					NewsItem newsItem = MenuDashboardUI.newsResponse.AppNews.NewsItems[i];
					GameObject gameObject = Object.Instantiate<GameObject>(MenuDashboardUI.templateNews);
					gameObject.name = "News_" + i;
					gameObject.GetComponent<RectTransform>().SetParent(MenuDashboardUI.templateNews.transform.parent, true);
					gameObject.SetActive(true);
					Transform transform = gameObject.transform.FindChild("Title");
					transform.GetComponent<Text>().text = newsItem.Title;
					DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
					dateTime = dateTime.AddSeconds((double)newsItem.Date).ToLocalTime();
					Transform transform2 = gameObject.transform.FindChild("Author");
					transform2.GetComponent<Text>().text = MenuDashboardUI.localization.format("News_Author", new object[]
					{
						dateTime,
						newsItem.Author
					});
					MenuDashboardUI.insertContent(gameObject, newsItem.Contents);
					GameObject gameObject2 = Object.Instantiate<GameObject>(MenuDashboardUI.templateLink);
					gameObject2.name = "Comments";
					gameObject2.GetComponent<RectTransform>().SetParent(gameObject.transform, true);
					gameObject2.SetActive(true);
					gameObject2.GetComponent<Text>().text = MenuDashboardUI.localization.format("News_Comments_Link");
					gameObject2.GetComponent<WebLink>().url = newsItem.URL;
				}
				Provider.provider.webService.releaseRequest(MenuDashboardUI.newsRequestHandle);
			}
		}

		private static void onSteamUGCQueryCompleted(SteamUGCQueryCompleted_t callback, bool io)
		{
			if (callback.m_handle == MenuDashboardUI.popularWorkshopHandle)
			{
				uint num = (uint)Random.Range(0, Provider.statusData.News.Popular_Workshop_Carousel_Items);
				SteamUGCDetails_t steamUGCDetails_t;
				SteamUGC.GetQueryUGCResult(MenuDashboardUI.popularWorkshopHandle, num, ref steamUGCDetails_t);
				SteamUGC.ReleaseQueryUGCRequest(MenuDashboardUI.popularWorkshopHandle);
				MenuDashboardUI.popularWorkshopHandle = UGCQueryHandle_t.Invalid;
				MenuDashboardUI.queryFeaturedItem(steamUGCDetails_t.m_nPublishedFileId);
			}
			else if (callback.m_handle == MenuDashboardUI.featuredWorkshopHandle)
			{
				GameObject gameObject = Object.Instantiate<GameObject>(MenuDashboardUI.templateWorkshop);
				gameObject.name = "Workshop";
				gameObject.GetComponent<RectTransform>().SetParent(MenuDashboardUI.templateNews.transform.parent, true);
				gameObject.transform.SetAsFirstSibling();
				gameObject.SetActive(true);
				SteamUGCDetails_t steamUGCDetails_t2;
				if (SteamUGC.GetQueryUGCResult(MenuDashboardUI.featuredWorkshopHandle, 0u, ref steamUGCDetails_t2))
				{
					Transform transform = gameObject.transform.FindChild("Title");
					transform.GetComponent<Text>().text = MenuDashboardUI.localization.format("Featured_Workshop_Title", new object[]
					{
						steamUGCDetails_t2.m_rgchTitle
					});
					string url;
					if (SteamUGC.GetQueryUGCPreviewURL(MenuDashboardUI.featuredWorkshopHandle, 0u, ref url, 1024u))
					{
						GameObject gameObject2 = Object.Instantiate<GameObject>(MenuDashboardUI.templateImage);
						gameObject2.name = "Preview";
						gameObject2.GetComponent<RectTransform>().SetParent(gameObject.transform, true);
						gameObject2.SetActive(true);
						gameObject2.GetComponent<WebImage>().url = url;
					}
					GameObject gameObject3 = Object.Instantiate<GameObject>(MenuDashboardUI.templateReadMoreLink);
					gameObject3.name = "ReadMore_Link";
					gameObject3.GetComponent<RectTransform>().SetParent(gameObject.transform, true);
					gameObject3.SetActive(true);
					GameObject gameObject4 = Object.Instantiate<GameObject>(MenuDashboardUI.templateReadMoreContent);
					gameObject4.name = "ReadMore_Content";
					gameObject4.GetComponent<RectTransform>().SetParent(gameObject.transform, true);
					MenuDashboardUI.insertContent(gameObject4, steamUGCDetails_t2.m_rgchDescription);
					gameObject3.GetComponent<ReadMore>().targetContent = gameObject4;
					gameObject3.GetComponent<ReadMore>().onText = MenuDashboardUI.localization.format("ReadMore_Link_On");
					gameObject3.GetComponent<ReadMore>().offText = MenuDashboardUI.localization.format("ReadMore_Link_Off");
					gameObject3.GetComponent<ReadMore>().Refresh();
					GameObject gameObject5 = Object.Instantiate<GameObject>(MenuDashboardUI.templateLink);
					gameObject5.name = "View";
					gameObject5.GetComponent<RectTransform>().SetParent(gameObject.transform, true);
					gameObject5.SetActive(true);
					gameObject5.GetComponent<Text>().text = MenuDashboardUI.localization.format("Featured_Workshop_Link");
					gameObject5.GetComponent<WebLink>().url = "http://steamcommunity.com/sharedfiles/filedetails/?id=" + steamUGCDetails_t2.m_nPublishedFileId;
				}
				SteamUGC.ReleaseQueryUGCRequest(MenuDashboardUI.featuredWorkshopHandle);
				MenuDashboardUI.featuredWorkshopHandle = UGCQueryHandle_t.Invalid;
			}
		}

		protected static void queryFeaturedItem(PublishedFileId_t publishedFileID)
		{
			if (MenuDashboardUI.featuredWorkshopHandle != UGCQueryHandle_t.Invalid)
			{
				SteamUGC.ReleaseQueryUGCRequest(MenuDashboardUI.featuredWorkshopHandle);
				MenuDashboardUI.featuredWorkshopHandle = UGCQueryHandle_t.Invalid;
			}
			MenuDashboardUI.featuredWorkshopHandle = SteamUGC.CreateQueryUGCDetailsRequest(new PublishedFileId_t[]
			{
				publishedFileID
			}, 1u);
			SteamUGC.SetReturnLongDescription(MenuDashboardUI.featuredWorkshopHandle, true);
			SteamAPICall_t steamAPICall_t = SteamUGC.SendQueryUGCRequest(MenuDashboardUI.featuredWorkshopHandle);
			MenuDashboardUI.steamUGCQueryCompletedFeatured.Set(steamAPICall_t, null);
		}

		public static Local localization;

		public static Bundle icons;

		private static Sleek container;

		public static bool active;

		private static bool canvasActive;

		private static SleekButtonIcon playButton;

		private static SleekButtonIcon survivorsButton;

		private static SleekButtonIcon configurationButton;

		private static SleekButtonIcon workshopButton;

		private static SleekButtonIcon exitButton;

		private static SleekButton proButton;

		private static SleekLabel proLabel;

		private static SleekLabel featureLabel;

		private static SleekBox battlEyeBox;

		private static SleekImageTexture battlEyeIcon;

		private static SleekLabel battlEyeHeaderLabel;

		private static SleekLabel battlEyeBodyLabel;

		private static SleekLabel alertBox;

		private static SleekImageTexture alertIcon;

		private static SleekLabel alertHeaderLabel;

		private static SleekLabel alertBodyLabel;

		private static GameObject canvas;

		private static GameObject templateNews;

		private static GameObject templateWorkshop;

		private static GameObject templateText;

		private static GameObject templateImage;

		private static GameObject templateLink;

		private static GameObject templateReadMoreLink;

		private static GameObject templateReadMoreContent;

		private static NewsResponse newsResponse;

		private static IWebRequestHandle newsRequestHandle;

		private static UGCQueryHandle_t popularWorkshopHandle = UGCQueryHandle_t.Invalid;

		private static UGCQueryHandle_t featuredWorkshopHandle = UGCQueryHandle_t.Invalid;

		private static CallResult<SteamUGCQueryCompleted_t> steamUGCQueryCompletedPopular;

		private static CallResult<SteamUGCQueryCompleted_t> steamUGCQueryCompletedFeatured;

		[CompilerGenerated]
		private static WebRequestReadyCallback <>f__mg$cache0;

		[CompilerGenerated]
		private static CallResult<SteamUGCQueryCompleted_t>.APIDispatchDelegate <>f__mg$cache1;

		[CompilerGenerated]
		private static CallResult<SteamUGCQueryCompleted_t>.APIDispatchDelegate <>f__mg$cache2;

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

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache8;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache9;
	}
}
