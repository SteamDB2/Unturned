using System;
using System.Runtime.CompilerServices;
using Steamworks;
using UnityEngine;
using UnityEngine.Analytics;

namespace SDG.Unturned
{
	public class MenuPauseUI
	{
		public MenuPauseUI()
		{
			MenuPauseUI.localization = Localization.read("/Menu/MenuPause.dat");
			if (MenuPauseUI.icons != null)
			{
				MenuPauseUI.icons.unload();
				MenuPauseUI.icons = null;
			}
			MenuPauseUI.icons = Bundles.getBundle("/Bundles/Textures/Menu/Icons/MenuPause/MenuPause.unity3d");
			MenuPauseUI.container = new Sleek();
			MenuPauseUI.container.positionOffset_X = 10;
			MenuPauseUI.container.positionOffset_Y = 10;
			MenuPauseUI.container.positionScale_Y = -1f;
			MenuPauseUI.container.sizeOffset_X = -20;
			MenuPauseUI.container.sizeOffset_Y = -20;
			MenuPauseUI.container.sizeScale_X = 1f;
			MenuPauseUI.container.sizeScale_Y = 1f;
			MenuUI.container.add(MenuPauseUI.container);
			MenuPauseUI.active = false;
			MenuPauseUI.returnButton = new SleekButtonIcon((Texture2D)MenuPauseUI.icons.load("Return"));
			MenuPauseUI.returnButton.positionOffset_X = -100;
			MenuPauseUI.returnButton.positionOffset_Y = -235;
			MenuPauseUI.returnButton.positionScale_X = 0.5f;
			MenuPauseUI.returnButton.positionScale_Y = 0.5f;
			MenuPauseUI.returnButton.sizeOffset_X = 200;
			MenuPauseUI.returnButton.sizeOffset_Y = 50;
			MenuPauseUI.returnButton.text = MenuPauseUI.localization.format("Return_Button");
			MenuPauseUI.returnButton.tooltip = MenuPauseUI.localization.format("Return_Button_Tooltip");
			SleekButton sleekButton = MenuPauseUI.returnButton;
			if (MenuPauseUI.<>f__mg$cache0 == null)
			{
				MenuPauseUI.<>f__mg$cache0 = new ClickedButton(MenuPauseUI.onClickedReturnButton);
			}
			sleekButton.onClickedButton = MenuPauseUI.<>f__mg$cache0;
			MenuPauseUI.returnButton.fontSize = 14;
			MenuPauseUI.returnButton.iconImage.backgroundTint = ESleekTint.FOREGROUND;
			MenuPauseUI.container.add(MenuPauseUI.returnButton);
			MenuPauseUI.reportButton = new SleekButtonIcon((Texture2D)MenuPauseUI.icons.load("Report"));
			MenuPauseUI.reportButton.positionOffset_X = -100;
			MenuPauseUI.reportButton.positionOffset_Y = -115;
			MenuPauseUI.reportButton.positionScale_X = 0.5f;
			MenuPauseUI.reportButton.positionScale_Y = 0.5f;
			MenuPauseUI.reportButton.sizeOffset_X = 200;
			MenuPauseUI.reportButton.sizeOffset_Y = 50;
			MenuPauseUI.reportButton.text = MenuPauseUI.localization.format("Report_Button");
			MenuPauseUI.reportButton.tooltip = MenuPauseUI.localization.format("Report_Button_Tooltip");
			SleekButton sleekButton2 = MenuPauseUI.reportButton;
			if (MenuPauseUI.<>f__mg$cache1 == null)
			{
				MenuPauseUI.<>f__mg$cache1 = new ClickedButton(MenuPauseUI.onClickedReportButton);
			}
			sleekButton2.onClickedButton = MenuPauseUI.<>f__mg$cache1;
			MenuPauseUI.reportButton.fontSize = 14;
			MenuPauseUI.reportButton.iconImage.backgroundTint = ESleekTint.FOREGROUND;
			MenuPauseUI.container.add(MenuPauseUI.reportButton);
			MenuPauseUI.trelloButton = new SleekButtonIcon((Texture2D)MenuPauseUI.icons.load("Trello"));
			MenuPauseUI.trelloButton.positionOffset_X = -100;
			MenuPauseUI.trelloButton.positionOffset_Y = -85;
			MenuPauseUI.trelloButton.positionScale_X = 0.5f;
			MenuPauseUI.trelloButton.positionScale_Y = 0.5f;
			MenuPauseUI.trelloButton.sizeOffset_X = 200;
			MenuPauseUI.trelloButton.sizeOffset_Y = 50;
			MenuPauseUI.trelloButton.text = MenuPauseUI.localization.format("Trello_Button");
			MenuPauseUI.trelloButton.tooltip = MenuPauseUI.localization.format("Trello_Button_Tooltip");
			SleekButton sleekButton3 = MenuPauseUI.trelloButton;
			if (MenuPauseUI.<>f__mg$cache2 == null)
			{
				MenuPauseUI.<>f__mg$cache2 = new ClickedButton(MenuPauseUI.onClickedTrelloButton);
			}
			sleekButton3.onClickedButton = MenuPauseUI.<>f__mg$cache2;
			MenuPauseUI.trelloButton.fontSize = 14;
			MenuPauseUI.twitterButton = new SleekButtonIcon((Texture2D)MenuPauseUI.icons.load("Twitter"));
			MenuPauseUI.twitterButton.positionOffset_X = -100;
			MenuPauseUI.twitterButton.positionOffset_Y = -55;
			MenuPauseUI.twitterButton.positionScale_X = 0.5f;
			MenuPauseUI.twitterButton.positionScale_Y = 0.5f;
			MenuPauseUI.twitterButton.sizeOffset_X = 200;
			MenuPauseUI.twitterButton.sizeOffset_Y = 50;
			MenuPauseUI.twitterButton.text = MenuPauseUI.localization.format("Twitter_Button");
			MenuPauseUI.twitterButton.tooltip = MenuPauseUI.localization.format("Twitter_Button_Tooltip");
			SleekButton sleekButton4 = MenuPauseUI.twitterButton;
			if (MenuPauseUI.<>f__mg$cache3 == null)
			{
				MenuPauseUI.<>f__mg$cache3 = new ClickedButton(MenuPauseUI.onClickedTwitterButton);
			}
			sleekButton4.onClickedButton = MenuPauseUI.<>f__mg$cache3;
			MenuPauseUI.twitterButton.fontSize = 14;
			MenuPauseUI.container.add(MenuPauseUI.twitterButton);
			MenuPauseUI.steamButton = new SleekButtonIcon((Texture2D)MenuPauseUI.icons.load("Steam"));
			MenuPauseUI.steamButton.positionOffset_X = -100;
			MenuPauseUI.steamButton.positionOffset_Y = 5;
			MenuPauseUI.steamButton.positionScale_X = 0.5f;
			MenuPauseUI.steamButton.positionScale_Y = 0.5f;
			MenuPauseUI.steamButton.sizeOffset_X = 200;
			MenuPauseUI.steamButton.sizeOffset_Y = 50;
			MenuPauseUI.steamButton.text = MenuPauseUI.localization.format("Steam_Button");
			MenuPauseUI.steamButton.tooltip = MenuPauseUI.localization.format("Steam_Button_Tooltip");
			SleekButton sleekButton5 = MenuPauseUI.steamButton;
			if (MenuPauseUI.<>f__mg$cache4 == null)
			{
				MenuPauseUI.<>f__mg$cache4 = new ClickedButton(MenuPauseUI.onClickedSteamButton);
			}
			sleekButton5.onClickedButton = MenuPauseUI.<>f__mg$cache4;
			MenuPauseUI.steamButton.fontSize = 14;
			MenuPauseUI.container.add(MenuPauseUI.steamButton);
			MenuPauseUI.creditsButton = new SleekButtonIcon((Texture2D)MenuPauseUI.icons.load("Credits"));
			MenuPauseUI.creditsButton.positionOffset_X = -100;
			MenuPauseUI.creditsButton.positionOffset_Y = 185;
			MenuPauseUI.creditsButton.positionScale_X = 0.5f;
			MenuPauseUI.creditsButton.positionScale_Y = 0.5f;
			MenuPauseUI.creditsButton.sizeOffset_X = 200;
			MenuPauseUI.creditsButton.sizeOffset_Y = 50;
			MenuPauseUI.creditsButton.text = MenuPauseUI.localization.format("Credits_Button");
			MenuPauseUI.creditsButton.tooltip = MenuPauseUI.localization.format("Credits_Button_Tooltip");
			SleekButton sleekButton6 = MenuPauseUI.creditsButton;
			if (MenuPauseUI.<>f__mg$cache5 == null)
			{
				MenuPauseUI.<>f__mg$cache5 = new ClickedButton(MenuPauseUI.onClickedCreditsButton);
			}
			sleekButton6.onClickedButton = MenuPauseUI.<>f__mg$cache5;
			MenuPauseUI.creditsButton.fontSize = 14;
			MenuPauseUI.container.add(MenuPauseUI.creditsButton);
			MenuPauseUI.forumButton = new SleekButtonIcon((Texture2D)MenuPauseUI.icons.load("Forum"));
			MenuPauseUI.forumButton.positionOffset_X = -100;
			MenuPauseUI.forumButton.positionOffset_Y = 65;
			MenuPauseUI.forumButton.positionScale_X = 0.5f;
			MenuPauseUI.forumButton.positionScale_Y = 0.5f;
			MenuPauseUI.forumButton.sizeOffset_X = 200;
			MenuPauseUI.forumButton.sizeOffset_Y = 50;
			MenuPauseUI.forumButton.text = MenuPauseUI.localization.format("Forum_Button");
			MenuPauseUI.forumButton.tooltip = MenuPauseUI.localization.format("Forum_Button_Tooltip");
			SleekButton sleekButton7 = MenuPauseUI.forumButton;
			if (MenuPauseUI.<>f__mg$cache6 == null)
			{
				MenuPauseUI.<>f__mg$cache6 = new ClickedButton(MenuPauseUI.onClickedForumButton);
			}
			sleekButton7.onClickedButton = MenuPauseUI.<>f__mg$cache6;
			MenuPauseUI.forumButton.fontSize = 14;
			MenuPauseUI.container.add(MenuPauseUI.forumButton);
			MenuPauseUI.blogButton = new SleekButtonIcon((Texture2D)MenuPauseUI.icons.load("Blog"));
			MenuPauseUI.blogButton.positionOffset_X = -100;
			MenuPauseUI.blogButton.positionOffset_Y = 125;
			MenuPauseUI.blogButton.positionScale_X = 0.5f;
			MenuPauseUI.blogButton.positionScale_Y = 0.5f;
			MenuPauseUI.blogButton.sizeOffset_X = 200;
			MenuPauseUI.blogButton.sizeOffset_Y = 50;
			MenuPauseUI.blogButton.text = MenuPauseUI.localization.format("Blog_Button");
			MenuPauseUI.blogButton.tooltip = MenuPauseUI.localization.format("Blog_Button_Tooltip");
			SleekButton sleekButton8 = MenuPauseUI.blogButton;
			if (MenuPauseUI.<>f__mg$cache7 == null)
			{
				MenuPauseUI.<>f__mg$cache7 = new ClickedButton(MenuPauseUI.onClickedBlogButton);
			}
			sleekButton8.onClickedButton = MenuPauseUI.<>f__mg$cache7;
			MenuPauseUI.blogButton.fontSize = 14;
			MenuPauseUI.container.add(MenuPauseUI.blogButton);
			MenuPauseUI.exitButton = new SleekButtonIcon((Texture2D)MenuPauseUI.icons.load("Exit"));
			MenuPauseUI.exitButton.positionOffset_X = -100;
			MenuPauseUI.exitButton.positionOffset_Y = -175;
			MenuPauseUI.exitButton.positionScale_X = 0.5f;
			MenuPauseUI.exitButton.positionScale_Y = 0.5f;
			MenuPauseUI.exitButton.sizeOffset_X = 200;
			MenuPauseUI.exitButton.sizeOffset_Y = 50;
			MenuPauseUI.exitButton.text = MenuPauseUI.localization.format("Exit_Button");
			MenuPauseUI.exitButton.tooltip = MenuPauseUI.localization.format("Exit_Button_Tooltip");
			SleekButton sleekButton9 = MenuPauseUI.exitButton;
			if (MenuPauseUI.<>f__mg$cache8 == null)
			{
				MenuPauseUI.<>f__mg$cache8 = new ClickedButton(MenuPauseUI.onClickedExitButton);
			}
			sleekButton9.onClickedButton = MenuPauseUI.<>f__mg$cache8;
			MenuPauseUI.exitButton.fontSize = 14;
			MenuPauseUI.exitButton.iconImage.backgroundTint = ESleekTint.FOREGROUND;
			MenuPauseUI.container.add(MenuPauseUI.exitButton);
			MenuPauseUI.icons.unload();
		}

		public static void open()
		{
			if (MenuPauseUI.active)
			{
				return;
			}
			MenuPauseUI.active = true;
			MenuPauseUI.container.lerpPositionScale(0f, 0f, ESleekLerp.EXPONENTIAL, 20f);
		}

		public static void close()
		{
			if (!MenuPauseUI.active)
			{
				return;
			}
			MenuPauseUI.active = false;
			MenuPauseUI.container.lerpPositionScale(0f, -1f, ESleekLerp.EXPONENTIAL, 20f);
		}

		private static void onClickedReturnButton(SleekButton button)
		{
			MenuPauseUI.close();
			MenuDashboardUI.open();
			MenuTitleUI.open();
		}

		private static void onClickedExitButton(SleekButton button)
		{
			Application.Quit();
		}

		private static void onClickedReportButton(SleekButton button)
		{
			if (!Provider.provider.browserService.canOpenBrowser)
			{
				MenuUI.alert(MenuPauseUI.localization.format("Overlay"));
				return;
			}
			Provider.provider.browserService.open("http://steamcommunity.com/app/" + SteamUtils.GetAppID() + "/discussions/9/613936673439628788/");
		}

		private static void onClickedTrelloButton(SleekButton button)
		{
			if (!Provider.provider.browserService.canOpenBrowser)
			{
				MenuUI.alert(MenuPauseUI.localization.format("Overlay"));
				return;
			}
			Provider.provider.browserService.open("https://trello.com/b/ezUtMJif");
			Analytics.CustomEvent("Link_Trello", null);
		}

		private static void onClickedTwitterButton(SleekButton button)
		{
			if (!Provider.provider.browserService.canOpenBrowser)
			{
				MenuUI.alert(MenuPauseUI.localization.format("Overlay"));
				return;
			}
			Provider.provider.browserService.open("https://twitter.com/SDGNelson");
			Analytics.CustomEvent("Link_Twitter", null);
		}

		private static void onClickedSteamButton(SleekButton button)
		{
			if (!Provider.provider.browserService.canOpenBrowser)
			{
				MenuUI.alert(MenuPauseUI.localization.format("Overlay"));
				return;
			}
			Provider.provider.browserService.open("http://steamcommunity.com/app/304930/announcements/");
			Analytics.CustomEvent("Link_News", null);
		}

		private static void onClickedCreditsButton(SleekButton button)
		{
			MenuPauseUI.close();
			MenuCreditsUI.open();
		}

		private static void onClickedForumButton(SleekButton button)
		{
			if (!Provider.provider.browserService.canOpenBrowser)
			{
				MenuUI.alert(MenuPauseUI.localization.format("Overlay"));
				return;
			}
			Provider.provider.browserService.open("https://forum.smartlydressedgames.com/");
			Analytics.CustomEvent("Link_Forum", null);
		}

		private static void onClickedBlogButton(SleekButton button)
		{
			if (!Provider.provider.browserService.canOpenBrowser)
			{
				MenuUI.alert(MenuPauseUI.localization.format("Overlay"));
				return;
			}
			Provider.provider.browserService.open("http://blog.smartlydressedgames.com/");
			Analytics.CustomEvent("Link_Blog", null);
		}

		public static Local localization;

		public static Bundle icons;

		private static Sleek container;

		public static bool active;

		private static SleekButtonIcon returnButton;

		private static SleekButtonIcon exitButton;

		private static SleekButtonIcon reportButton;

		private static SleekButtonIcon trelloButton;

		private static SleekButtonIcon twitterButton;

		private static SleekButtonIcon steamButton;

		private static SleekButtonIcon creditsButton;

		private static SleekButtonIcon forumButton;

		private static SleekButtonIcon blogButton;

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

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache8;
	}
}
