using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace SDG.Unturned
{
	public class MenuCreditsUI
	{
		public MenuCreditsUI()
		{
			MenuCreditsUI.container = new Sleek();
			MenuCreditsUI.container.positionOffset_X = 10;
			MenuCreditsUI.container.positionOffset_Y = 10;
			MenuCreditsUI.container.positionScale_Y = -1f;
			MenuCreditsUI.container.sizeOffset_X = -20;
			MenuCreditsUI.container.sizeOffset_Y = -20;
			MenuCreditsUI.container.sizeScale_X = 1f;
			MenuCreditsUI.container.sizeScale_Y = 1f;
			MenuUI.container.add(MenuCreditsUI.container);
			MenuCreditsUI.active = false;
			MenuCreditsUI.returnButton = new SleekButtonIcon((Texture2D)MenuPauseUI.icons.load("Exit"));
			MenuCreditsUI.returnButton.positionOffset_X = -250;
			MenuCreditsUI.returnButton.positionOffset_Y = 100;
			MenuCreditsUI.returnButton.positionScale_X = 0.5f;
			MenuCreditsUI.returnButton.sizeOffset_X = 500;
			MenuCreditsUI.returnButton.sizeOffset_Y = 50;
			MenuCreditsUI.returnButton.text = MenuPauseUI.localization.format("Return_Button");
			MenuCreditsUI.returnButton.tooltip = MenuPauseUI.localization.format("Return_Button_Tooltip");
			SleekButton sleekButton = MenuCreditsUI.returnButton;
			if (MenuCreditsUI.<>f__mg$cache0 == null)
			{
				MenuCreditsUI.<>f__mg$cache0 = new ClickedButton(MenuCreditsUI.onClickedReturnButton);
			}
			sleekButton.onClickedButton = MenuCreditsUI.<>f__mg$cache0;
			MenuCreditsUI.returnButton.fontSize = 14;
			MenuCreditsUI.returnButton.iconImage.backgroundTint = ESleekTint.FOREGROUND;
			MenuCreditsUI.container.add(MenuCreditsUI.returnButton);
			MenuCreditsUI.creditsBox = new SleekBox();
			MenuCreditsUI.creditsBox.positionOffset_X = -250;
			MenuCreditsUI.creditsBox.positionOffset_Y = 160;
			MenuCreditsUI.creditsBox.positionScale_X = 0.5f;
			MenuCreditsUI.creditsBox.sizeOffset_X = 500;
			MenuCreditsUI.creditsBox.sizeOffset_Y = -260;
			MenuCreditsUI.creditsBox.sizeScale_Y = 1f;
			MenuCreditsUI.creditsBox.fontSize = 14;
			MenuCreditsUI.container.add(MenuCreditsUI.creditsBox);
			MenuCreditsUI.scrollBox = new SleekScrollBox();
			MenuCreditsUI.scrollBox.positionOffset_X = 5;
			MenuCreditsUI.scrollBox.positionOffset_Y = 5;
			MenuCreditsUI.scrollBox.sizeOffset_X = 25;
			MenuCreditsUI.scrollBox.sizeOffset_Y = -5;
			MenuCreditsUI.scrollBox.sizeScale_X = 1f;
			MenuCreditsUI.scrollBox.sizeScale_Y = 1f;
			MenuCreditsUI.scrollBox.area = new Rect(0f, 0f, 5f, (float)(MenuCreditsUI.credits.Length * 30));
			MenuCreditsUI.creditsBox.add(MenuCreditsUI.scrollBox);
			for (int i = 0; i < MenuCreditsUI.credits.Length; i++)
			{
				CreditsContributorContributionPair creditsContributorContributionPair = MenuCreditsUI.credits[i];
				SleekLabel sleekLabel = new SleekLabel();
				sleekLabel.positionOffset_X = 0;
				sleekLabel.positionOffset_Y = i * 30;
				sleekLabel.sizeOffset_X = -35;
				sleekLabel.sizeOffset_Y = 30;
				sleekLabel.sizeScale_X = 1f;
				sleekLabel.fontAlignment = 3;
				sleekLabel.fontSize = 14;
				sleekLabel.text = creditsContributorContributionPair.contributor;
				MenuCreditsUI.scrollBox.add(sleekLabel);
				SleekLabel sleekLabel2 = new SleekLabel();
				sleekLabel2.positionOffset_X = 0;
				sleekLabel2.positionOffset_Y = i * 30;
				sleekLabel2.sizeOffset_X = -35;
				sleekLabel2.sizeOffset_Y = 30;
				sleekLabel2.sizeScale_X = 1f;
				sleekLabel2.fontAlignment = 5;
				sleekLabel2.fontSize = 14;
				sleekLabel2.text = creditsContributorContributionPair.contribution;
				MenuCreditsUI.scrollBox.add(sleekLabel2);
			}
		}

		public static void open()
		{
			if (MenuCreditsUI.active)
			{
				return;
			}
			MenuCreditsUI.active = true;
			MenuCreditsUI.container.lerpPositionScale(0f, 0f, ESleekLerp.EXPONENTIAL, 20f);
		}

		public static void close()
		{
			if (!MenuCreditsUI.active)
			{
				return;
			}
			MenuCreditsUI.active = false;
			MenuCreditsUI.container.lerpPositionScale(0f, -1f, ESleekLerp.EXPONENTIAL, 20f);
		}

		private static void onClickedReturnButton(SleekButton button)
		{
			MenuCreditsUI.close();
			MenuPauseUI.open();
		}

		private static Sleek container;

		public static bool active;

		private static SleekButtonIcon returnButton;

		private static SleekBox creditsBox;

		private static SleekScrollBox scrollBox;

		private static readonly CreditsContributorContributionPair[] credits = new CreditsContributorContributionPair[]
		{
			new CreditsContributorContributionPair("Nelson Sexton", "Unturned"),
			new CreditsContributorContributionPair("Sven Mawby", "RocketMod"),
			new CreditsContributorContributionPair("Riley Labrecque", "Steamworks .NET"),
			new CreditsContributorContributionPair("Stephen McKamey", "A* Pathfinding Project"),
			new CreditsContributorContributionPair("James Newton-King", "Json .NET"),
			new CreditsContributorContributionPair("Justin \"Gamez2much\" Morton", "Russia Collaborator"),
			new CreditsContributorContributionPair("Mitch \"Sketches\" Wheaton", "Russia Collaborator"),
			new CreditsContributorContributionPair("Alex \"Rainz2much\" Stoyanov", "Russia Collaborator"),
			new CreditsContributorContributionPair("Amanda \"Mooki2much\" Hubler", "Russia Collaborator"),
			new CreditsContributorContributionPair("Justin \"Gamez2much\" Morton", "Hawaii Creator"),
			new CreditsContributorContributionPair("Terran \"Spyjack\" Orion", "Hawaii Creator"),
			new CreditsContributorContributionPair("Alex \"Rainz2much\" Stoyanov", "Hawaii Creator"),
			new CreditsContributorContributionPair("Amanda \"Mooki2much\" Hubler", "Hawaii Creator"),
			new CreditsContributorContributionPair("Fran-war", "Steam Forum Moderator"),
			new CreditsContributorContributionPair("SongPhoenix", "Steam Forum Moderator"),
			new CreditsContributorContributionPair("Lu", "Steam Forum Moderator"),
			new CreditsContributorContributionPair("Morkva", "Steam Forum Moderator"),
			new CreditsContributorContributionPair("Reaver", "Steam Forum Moderator"),
			new CreditsContributorContributionPair("Shadow", "Steam Forum Moderator")
		};

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache0;
	}
}
