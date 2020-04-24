using System;
using System.Runtime.CompilerServices;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	public class MenuPlayLobbiesUI
	{
		public MenuPlayLobbiesUI()
		{
			MenuPlayLobbiesUI.localization = Localization.read("/Menu/Play/MenuPlayLobbies.dat");
			Bundle bundle = Bundles.getBundle("/Bundles/Textures/Menu/Icons/Play/MenuPlayLobbies/MenuPlayLobbies.unity3d");
			MenuPlayLobbiesUI.container = new Sleek();
			MenuPlayLobbiesUI.container.positionOffset_X = 10;
			MenuPlayLobbiesUI.container.positionOffset_Y = 10;
			MenuPlayLobbiesUI.container.positionScale_Y = 1f;
			MenuPlayLobbiesUI.container.sizeOffset_X = -20;
			MenuPlayLobbiesUI.container.sizeOffset_Y = -20;
			MenuPlayLobbiesUI.container.sizeScale_X = 1f;
			MenuPlayLobbiesUI.container.sizeScale_Y = 1f;
			MenuUI.container.add(MenuPlayLobbiesUI.container);
			MenuPlayLobbiesUI.active = false;
			bundle.unload();
			MenuPlayLobbiesUI.membersLabel = new SleekLabel();
			MenuPlayLobbiesUI.membersLabel.positionOffset_X = -200;
			MenuPlayLobbiesUI.membersLabel.positionOffset_Y = 100;
			MenuPlayLobbiesUI.membersLabel.positionScale_X = 0.5f;
			MenuPlayLobbiesUI.membersLabel.sizeOffset_X = 400;
			MenuPlayLobbiesUI.membersLabel.sizeOffset_Y = 50;
			MenuPlayLobbiesUI.membersLabel.text = MenuPlayLobbiesUI.localization.format("Members");
			MenuPlayLobbiesUI.membersLabel.fontSize = 14;
			MenuPlayLobbiesUI.container.add(MenuPlayLobbiesUI.membersLabel);
			MenuPlayLobbiesUI.membersBox = new SleekScrollBox();
			MenuPlayLobbiesUI.membersBox.positionOffset_X = -200;
			MenuPlayLobbiesUI.membersBox.positionOffset_Y = 150;
			MenuPlayLobbiesUI.membersBox.positionScale_X = 0.5f;
			MenuPlayLobbiesUI.membersBox.sizeOffset_X = 430;
			MenuPlayLobbiesUI.membersBox.sizeOffset_Y = -300;
			MenuPlayLobbiesUI.membersBox.sizeScale_Y = 1f;
			MenuPlayLobbiesUI.container.add(MenuPlayLobbiesUI.membersBox);
			MenuPlayLobbiesUI.inviteButton = new SleekButtonIcon((Texture2D)MenuDashboardUI.icons.load("Invite"));
			MenuPlayLobbiesUI.inviteButton.positionOffset_X = -200;
			MenuPlayLobbiesUI.inviteButton.positionOffset_Y = -150;
			MenuPlayLobbiesUI.inviteButton.positionScale_X = 0.5f;
			MenuPlayLobbiesUI.inviteButton.positionScale_Y = 1f;
			MenuPlayLobbiesUI.inviteButton.sizeOffset_X = 400;
			MenuPlayLobbiesUI.inviteButton.sizeOffset_Y = 50;
			MenuPlayLobbiesUI.inviteButton.text = MenuPlayLobbiesUI.localization.format("Invite_Button");
			MenuPlayLobbiesUI.inviteButton.tooltip = MenuPlayLobbiesUI.localization.format("Invite_Button_Tooltip");
			SleekButton sleekButton = MenuPlayLobbiesUI.inviteButton;
			if (MenuPlayLobbiesUI.<>f__mg$cache0 == null)
			{
				MenuPlayLobbiesUI.<>f__mg$cache0 = new ClickedButton(MenuPlayLobbiesUI.onClickedInviteButton);
			}
			sleekButton.onClickedButton = MenuPlayLobbiesUI.<>f__mg$cache0;
			MenuPlayLobbiesUI.inviteButton.fontSize = 14;
			MenuPlayLobbiesUI.inviteButton.iconImage.backgroundTint = ESleekTint.FOREGROUND;
			MenuPlayLobbiesUI.container.add(MenuPlayLobbiesUI.inviteButton);
			MenuPlayLobbiesUI.backButton = new SleekButtonIcon((Texture2D)MenuDashboardUI.icons.load("Exit"));
			MenuPlayLobbiesUI.backButton.positionOffset_Y = -50;
			MenuPlayLobbiesUI.backButton.positionScale_Y = 1f;
			MenuPlayLobbiesUI.backButton.sizeOffset_X = 200;
			MenuPlayLobbiesUI.backButton.sizeOffset_Y = 50;
			MenuPlayLobbiesUI.backButton.text = MenuDashboardUI.localization.format("BackButtonText");
			MenuPlayLobbiesUI.backButton.tooltip = MenuDashboardUI.localization.format("BackButtonTooltip");
			SleekButton sleekButton2 = MenuPlayLobbiesUI.backButton;
			if (MenuPlayLobbiesUI.<>f__mg$cache1 == null)
			{
				MenuPlayLobbiesUI.<>f__mg$cache1 = new ClickedButton(MenuPlayLobbiesUI.onClickedBackButton);
			}
			sleekButton2.onClickedButton = MenuPlayLobbiesUI.<>f__mg$cache1;
			MenuPlayLobbiesUI.backButton.fontSize = 14;
			MenuPlayLobbiesUI.backButton.iconImage.backgroundTint = ESleekTint.FOREGROUND;
			MenuPlayLobbiesUI.container.add(MenuPlayLobbiesUI.backButton);
			if (MenuPlayLobbiesUI.<>f__mg$cache2 == null)
			{
				MenuPlayLobbiesUI.<>f__mg$cache2 = new LobbiesRefreshedHandler(MenuPlayLobbiesUI.handleLobbiesRefreshed);
			}
			Lobbies.lobbiesRefreshed = MenuPlayLobbiesUI.<>f__mg$cache2;
			if (MenuPlayLobbiesUI.<>f__mg$cache3 == null)
			{
				MenuPlayLobbiesUI.<>f__mg$cache3 = new LobbiesEnteredHandler(MenuPlayLobbiesUI.handleLobbiesEntered);
			}
			Lobbies.lobbiesEntered = MenuPlayLobbiesUI.<>f__mg$cache3;
		}

		public static void open()
		{
			if (MenuPlayLobbiesUI.active)
			{
				return;
			}
			MenuPlayLobbiesUI.active = true;
			if (Lobbies.inLobby)
			{
				MenuPlayLobbiesUI.refresh();
			}
			else
			{
				Lobbies.createLobby();
			}
			MenuPlayLobbiesUI.container.lerpPositionScale(0f, 0f, ESleekLerp.EXPONENTIAL, 20f);
		}

		public static void close()
		{
			if (!MenuPlayLobbiesUI.active)
			{
				return;
			}
			MenuPlayLobbiesUI.active = false;
			MenuSettings.save();
			MenuPlayLobbiesUI.container.lerpPositionScale(0f, 1f, ESleekLerp.EXPONENTIAL, 20f);
		}

		private static void refresh()
		{
			MenuPlayLobbiesUI.membersBox.remove();
			int lobbyMemberCount = Lobbies.getLobbyMemberCount();
			for (int i = 0; i < lobbyMemberCount; i++)
			{
				CSteamID lobbyMemberByIndex = Lobbies.getLobbyMemberByIndex(i);
				MenuPlayLobbiesUI.SleekLobbyPlayerButton sleekLobbyPlayerButton = new MenuPlayLobbiesUI.SleekLobbyPlayerButton(lobbyMemberByIndex);
				sleekLobbyPlayerButton.positionOffset_Y = i * 50;
				sleekLobbyPlayerButton.sizeOffset_X = -30;
				sleekLobbyPlayerButton.sizeOffset_Y = 50;
				sleekLobbyPlayerButton.sizeScale_X = 1f;
				MenuPlayLobbiesUI.membersBox.add(sleekLobbyPlayerButton);
			}
			MenuPlayLobbiesUI.membersBox.area = new Rect(0f, 0f, 5f, (float)(lobbyMemberCount * 50));
		}

		private static void handleLobbiesRefreshed()
		{
			if (!MenuPlayLobbiesUI.active)
			{
				return;
			}
			MenuPlayLobbiesUI.refresh();
		}

		private static void handleLobbiesEntered()
		{
			if (MenuPlayLobbiesUI.active)
			{
				return;
			}
			MenuUI.closeAll();
			MenuPlayLobbiesUI.open();
		}

		private static void onClickedInviteButton(SleekButton button)
		{
			if (!Lobbies.canOpenInvitations)
			{
				MenuUI.alert(MenuPlayLobbiesUI.localization.format("Overlay"));
				return;
			}
			Lobbies.openInvitations();
		}

		private static void onClickedBackButton(SleekButton button)
		{
			MenuPlayUI.open();
			MenuPlayLobbiesUI.close();
		}

		public static Local localization;

		private static Sleek container;

		public static bool active;

		private static SleekLabel membersLabel;

		private static SleekScrollBox membersBox;

		private static SleekButtonIcon inviteButton;

		private static SleekButtonIcon backButton;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache0;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache1;

		[CompilerGenerated]
		private static LobbiesRefreshedHandler <>f__mg$cache2;

		[CompilerGenerated]
		private static LobbiesEnteredHandler <>f__mg$cache3;

		public class SleekLobbyPlayerButton : Sleek
		{
			public SleekLobbyPlayerButton(CSteamID newSteamID)
			{
				this.steamID = newSteamID;
				base.init();
				this.button = new SleekButton();
				this.button.sizeScale_X = 1f;
				this.button.sizeScale_Y = 1f;
				this.button.onClickedButton = new ClickedButton(this.onClickedPlayerButton);
				base.add(this.button);
				this.avatarImage = new SleekImageTexture();
				this.avatarImage.positionOffset_X = 9;
				this.avatarImage.positionOffset_Y = 9;
				this.avatarImage.sizeOffset_X = 32;
				this.avatarImage.sizeOffset_Y = 32;
				this.avatarImage.texture = Provider.provider.communityService.getIcon(this.steamID);
				this.avatarImage.shouldDestroyTexture = true;
				this.button.add(this.avatarImage);
				this.nameLabel = new SleekLabel();
				this.nameLabel.positionOffset_X = 40;
				this.nameLabel.sizeOffset_X = -40;
				this.nameLabel.sizeScale_X = 1f;
				this.nameLabel.sizeScale_Y = 1f;
				this.nameLabel.text = SteamFriends.GetFriendPersonaName(this.steamID);
				this.nameLabel.fontSize = 14;
				this.button.add(this.nameLabel);
			}

			private void onClickedPlayerButton(SleekButton button)
			{
				Provider.provider.browserService.open("http://steamcommunity.com/profiles/" + this.steamID);
			}

			private CSteamID steamID;

			private SleekButton button;

			private SleekImageTexture avatarImage;

			private SleekLabel nameLabel;
		}
	}
}
