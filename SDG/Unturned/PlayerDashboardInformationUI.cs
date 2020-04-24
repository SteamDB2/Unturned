using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	public class PlayerDashboardInformationUI
	{
		public PlayerDashboardInformationUI()
		{
			if (PlayerDashboardInformationUI.icons != null)
			{
				PlayerDashboardInformationUI.icons.unload();
			}
			PlayerDashboardInformationUI.localization = Localization.read("/Player/PlayerDashboardInformation.dat");
			PlayerDashboardInformationUI.icons = Bundles.getBundle("/Bundles/Textures/Player/Icons/PlayerDashboardInformation/PlayerDashboardInformation.unity3d");
			PlayerDashboardInformationUI.container = new Sleek();
			PlayerDashboardInformationUI.container.positionScale_Y = 1f;
			PlayerDashboardInformationUI.container.positionOffset_X = 10;
			PlayerDashboardInformationUI.container.positionOffset_Y = 10;
			PlayerDashboardInformationUI.container.sizeOffset_X = -20;
			PlayerDashboardInformationUI.container.sizeOffset_Y = -20;
			PlayerDashboardInformationUI.container.sizeScale_X = 1f;
			PlayerDashboardInformationUI.container.sizeScale_Y = 1f;
			PlayerUI.container.add(PlayerDashboardInformationUI.container);
			PlayerDashboardInformationUI.active = false;
			PlayerDashboardInformationUI.zoom = 1;
			PlayerDashboardInformationUI.tab = PlayerDashboardInformationUI.EInfoTab.PLAYERS;
			PlayerDashboardInformationUI.isDragging = false;
			PlayerDashboardInformationUI.dragOrigin = Vector2.zero;
			PlayerDashboardInformationUI.dragOffset = Vector2.zero;
			PlayerDashboardInformationUI.backdropBox = new SleekBox();
			PlayerDashboardInformationUI.backdropBox.positionOffset_Y = 60;
			PlayerDashboardInformationUI.backdropBox.sizeOffset_Y = -60;
			PlayerDashboardInformationUI.backdropBox.sizeScale_X = 1f;
			PlayerDashboardInformationUI.backdropBox.sizeScale_Y = 1f;
			Color white = Color.white;
			white.a = 0.5f;
			PlayerDashboardInformationUI.backdropBox.backgroundColor = white;
			PlayerDashboardInformationUI.container.add(PlayerDashboardInformationUI.backdropBox);
			PlayerDashboardInformationUI.mapInspect = new Sleek();
			PlayerDashboardInformationUI.mapInspect.positionOffset_X = 10;
			PlayerDashboardInformationUI.mapInspect.positionOffset_Y = 10;
			PlayerDashboardInformationUI.mapInspect.sizeOffset_X = -15;
			PlayerDashboardInformationUI.mapInspect.sizeOffset_Y = -20;
			PlayerDashboardInformationUI.mapInspect.sizeScale_X = 0.6f;
			PlayerDashboardInformationUI.mapInspect.sizeScale_Y = 1f;
			PlayerDashboardInformationUI.backdropBox.add(PlayerDashboardInformationUI.mapInspect);
			PlayerDashboardInformationUI.mapBox = new SleekViewBox();
			PlayerDashboardInformationUI.mapBox.sizeOffset_Y = -40;
			PlayerDashboardInformationUI.mapBox.sizeScale_X = 1f;
			PlayerDashboardInformationUI.mapBox.sizeScale_Y = 1f;
			PlayerDashboardInformationUI.mapBox.constraint = ESleekConstraint.XY;
			PlayerDashboardInformationUI.mapInspect.add(PlayerDashboardInformationUI.mapBox);
			PlayerDashboardInformationUI.mapImage = new SleekImageTexture();
			PlayerDashboardInformationUI.mapBox.add(PlayerDashboardInformationUI.mapImage);
			PlayerDashboardInformationUI.noLabel = new SleekLabel();
			PlayerDashboardInformationUI.noLabel.sizeOffset_Y = -40;
			PlayerDashboardInformationUI.noLabel.sizeScale_X = 1f;
			PlayerDashboardInformationUI.noLabel.sizeScale_Y = 1f;
			PlayerDashboardInformationUI.noLabel.foregroundColor = Palette.COLOR_R;
			PlayerDashboardInformationUI.noLabel.foregroundTint = ESleekTint.NONE;
			PlayerDashboardInformationUI.noLabel.fontSize = 24;
			PlayerDashboardInformationUI.mapInspect.add(PlayerDashboardInformationUI.noLabel);
			PlayerDashboardInformationUI.noLabel.isVisible = false;
			PlayerDashboardInformationUI.updateZoom();
			PlayerDashboardInformationUI.zoomInButton = new SleekButtonIcon((Texture2D)PlayerDashboardInformationUI.icons.load("Zoom_In"));
			PlayerDashboardInformationUI.zoomInButton.positionOffset_Y = -30;
			PlayerDashboardInformationUI.zoomInButton.positionScale_Y = 1f;
			PlayerDashboardInformationUI.zoomInButton.sizeOffset_X = -5;
			PlayerDashboardInformationUI.zoomInButton.sizeOffset_Y = 30;
			PlayerDashboardInformationUI.zoomInButton.sizeScale_X = 0.25f;
			PlayerDashboardInformationUI.zoomInButton.text = PlayerDashboardInformationUI.localization.format("Zoom_In_Button");
			PlayerDashboardInformationUI.zoomInButton.tooltip = PlayerDashboardInformationUI.localization.format("Zoom_In_Button_Tooltip");
			PlayerDashboardInformationUI.zoomInButton.iconImage.backgroundTint = ESleekTint.FOREGROUND;
			SleekButton sleekButton = PlayerDashboardInformationUI.zoomInButton;
			if (PlayerDashboardInformationUI.<>f__mg$cache8 == null)
			{
				PlayerDashboardInformationUI.<>f__mg$cache8 = new ClickedButton(PlayerDashboardInformationUI.onClickedZoomInButton);
			}
			sleekButton.onClickedButton = PlayerDashboardInformationUI.<>f__mg$cache8;
			PlayerDashboardInformationUI.mapInspect.add(PlayerDashboardInformationUI.zoomInButton);
			PlayerDashboardInformationUI.zoomOutButton = new SleekButtonIcon((Texture2D)PlayerDashboardInformationUI.icons.load("Zoom_Out"));
			PlayerDashboardInformationUI.zoomOutButton.positionOffset_X = 5;
			PlayerDashboardInformationUI.zoomOutButton.positionOffset_Y = -30;
			PlayerDashboardInformationUI.zoomOutButton.positionScale_X = 0.25f;
			PlayerDashboardInformationUI.zoomOutButton.positionScale_Y = 1f;
			PlayerDashboardInformationUI.zoomOutButton.sizeOffset_X = -10;
			PlayerDashboardInformationUI.zoomOutButton.sizeOffset_Y = 30;
			PlayerDashboardInformationUI.zoomOutButton.sizeScale_X = 0.25f;
			PlayerDashboardInformationUI.zoomOutButton.text = PlayerDashboardInformationUI.localization.format("Zoom_Out_Button");
			PlayerDashboardInformationUI.zoomOutButton.tooltip = PlayerDashboardInformationUI.localization.format("Zoom_Out_Button_Tooltip");
			PlayerDashboardInformationUI.zoomOutButton.iconImage.backgroundTint = ESleekTint.FOREGROUND;
			SleekButton sleekButton2 = PlayerDashboardInformationUI.zoomOutButton;
			if (PlayerDashboardInformationUI.<>f__mg$cache9 == null)
			{
				PlayerDashboardInformationUI.<>f__mg$cache9 = new ClickedButton(PlayerDashboardInformationUI.onClickedZoomOutButton);
			}
			sleekButton2.onClickedButton = PlayerDashboardInformationUI.<>f__mg$cache9;
			PlayerDashboardInformationUI.mapInspect.add(PlayerDashboardInformationUI.zoomOutButton);
			PlayerDashboardInformationUI.centerButton = new SleekButtonIcon((Texture2D)PlayerDashboardInformationUI.icons.load("Center"));
			PlayerDashboardInformationUI.centerButton.positionOffset_X = 5;
			PlayerDashboardInformationUI.centerButton.positionOffset_Y = -30;
			PlayerDashboardInformationUI.centerButton.positionScale_X = 0.5f;
			PlayerDashboardInformationUI.centerButton.positionScale_Y = 1f;
			PlayerDashboardInformationUI.centerButton.sizeOffset_X = -10;
			PlayerDashboardInformationUI.centerButton.sizeOffset_Y = 30;
			PlayerDashboardInformationUI.centerButton.sizeScale_X = 0.25f;
			PlayerDashboardInformationUI.centerButton.text = PlayerDashboardInformationUI.localization.format("Center_Button");
			PlayerDashboardInformationUI.centerButton.tooltip = PlayerDashboardInformationUI.localization.format("Center_Button_Tooltip");
			PlayerDashboardInformationUI.centerButton.iconImage.backgroundTint = ESleekTint.FOREGROUND;
			SleekButton sleekButton3 = PlayerDashboardInformationUI.centerButton;
			if (PlayerDashboardInformationUI.<>f__mg$cacheA == null)
			{
				PlayerDashboardInformationUI.<>f__mg$cacheA = new ClickedButton(PlayerDashboardInformationUI.onClickedCenterButton);
			}
			sleekButton3.onClickedButton = PlayerDashboardInformationUI.<>f__mg$cacheA;
			PlayerDashboardInformationUI.mapInspect.add(PlayerDashboardInformationUI.centerButton);
			PlayerDashboardInformationUI.mapButtonState = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(PlayerDashboardInformationUI.localization.format("Chart")),
				new GUIContent(PlayerDashboardInformationUI.localization.format("Satellite"))
			});
			PlayerDashboardInformationUI.mapButtonState.positionOffset_X = 5;
			PlayerDashboardInformationUI.mapButtonState.positionOffset_Y = -30;
			PlayerDashboardInformationUI.mapButtonState.positionScale_X = 0.75f;
			PlayerDashboardInformationUI.mapButtonState.positionScale_Y = 1f;
			PlayerDashboardInformationUI.mapButtonState.sizeOffset_X = -5;
			PlayerDashboardInformationUI.mapButtonState.sizeOffset_Y = 30;
			PlayerDashboardInformationUI.mapButtonState.sizeScale_X = 0.25f;
			SleekButtonState sleekButtonState = PlayerDashboardInformationUI.mapButtonState;
			if (PlayerDashboardInformationUI.<>f__mg$cacheB == null)
			{
				PlayerDashboardInformationUI.<>f__mg$cacheB = new SwappedState(PlayerDashboardInformationUI.onSwappedMapState);
			}
			sleekButtonState.onSwappedState = PlayerDashboardInformationUI.<>f__mg$cacheB;
			PlayerDashboardInformationUI.mapInspect.add(PlayerDashboardInformationUI.mapButtonState);
			PlayerDashboardInformationUI.headerButtonsContainer = new Sleek();
			PlayerDashboardInformationUI.headerButtonsContainer.positionOffset_X = 5;
			PlayerDashboardInformationUI.headerButtonsContainer.positionOffset_Y = 10;
			PlayerDashboardInformationUI.headerButtonsContainer.positionScale_X = 0.6f;
			PlayerDashboardInformationUI.headerButtonsContainer.sizeOffset_X = -15;
			PlayerDashboardInformationUI.headerButtonsContainer.sizeOffset_Y = 50;
			PlayerDashboardInformationUI.headerButtonsContainer.sizeScale_X = 0.4f;
			PlayerDashboardInformationUI.backdropBox.add(PlayerDashboardInformationUI.headerButtonsContainer);
			PlayerDashboardInformationUI.questsButton = new SleekButtonIcon((Texture2D)PlayerDashboardInformationUI.icons.load("Quests"));
			PlayerDashboardInformationUI.questsButton.sizeOffset_X = -5;
			PlayerDashboardInformationUI.questsButton.sizeScale_X = 0.333f;
			PlayerDashboardInformationUI.questsButton.sizeScale_Y = 1f;
			PlayerDashboardInformationUI.questsButton.fontSize = 14;
			PlayerDashboardInformationUI.questsButton.tooltip = PlayerDashboardInformationUI.localization.format("Quests_Tooltip");
			SleekButton sleekButton4 = PlayerDashboardInformationUI.questsButton;
			if (PlayerDashboardInformationUI.<>f__mg$cacheC == null)
			{
				PlayerDashboardInformationUI.<>f__mg$cacheC = new ClickedButton(PlayerDashboardInformationUI.onClickedQuestsButton);
			}
			sleekButton4.onClickedButton = PlayerDashboardInformationUI.<>f__mg$cacheC;
			PlayerDashboardInformationUI.headerButtonsContainer.add(PlayerDashboardInformationUI.questsButton);
			PlayerDashboardInformationUI.groupsButton = new SleekButtonIcon((Texture2D)PlayerDashboardInformationUI.icons.load("Groups"));
			PlayerDashboardInformationUI.groupsButton.positionOffset_X = 5;
			PlayerDashboardInformationUI.groupsButton.positionScale_X = 0.333f;
			PlayerDashboardInformationUI.groupsButton.sizeOffset_X = -10;
			PlayerDashboardInformationUI.groupsButton.sizeScale_X = 0.334f;
			PlayerDashboardInformationUI.groupsButton.sizeScale_Y = 1f;
			PlayerDashboardInformationUI.groupsButton.fontSize = 14;
			PlayerDashboardInformationUI.groupsButton.text = PlayerDashboardInformationUI.localization.format("Groups");
			PlayerDashboardInformationUI.groupsButton.tooltip = PlayerDashboardInformationUI.localization.format("Groups_Tooltip");
			SleekButton sleekButton5 = PlayerDashboardInformationUI.groupsButton;
			if (PlayerDashboardInformationUI.<>f__mg$cacheD == null)
			{
				PlayerDashboardInformationUI.<>f__mg$cacheD = new ClickedButton(PlayerDashboardInformationUI.onClickedGroupsButton);
			}
			sleekButton5.onClickedButton = PlayerDashboardInformationUI.<>f__mg$cacheD;
			PlayerDashboardInformationUI.headerButtonsContainer.add(PlayerDashboardInformationUI.groupsButton);
			PlayerDashboardInformationUI.playersButton = new SleekButtonIcon((Texture2D)PlayerDashboardInformationUI.icons.load("Players"));
			PlayerDashboardInformationUI.playersButton.positionOffset_X = 5;
			PlayerDashboardInformationUI.playersButton.positionScale_X = 0.667f;
			PlayerDashboardInformationUI.playersButton.sizeOffset_X = -5;
			PlayerDashboardInformationUI.playersButton.sizeScale_X = 0.333f;
			PlayerDashboardInformationUI.playersButton.sizeScale_Y = 1f;
			PlayerDashboardInformationUI.playersButton.fontSize = 14;
			PlayerDashboardInformationUI.playersButton.tooltip = PlayerDashboardInformationUI.localization.format("Players_Tooltip");
			SleekButton sleekButton6 = PlayerDashboardInformationUI.playersButton;
			if (PlayerDashboardInformationUI.<>f__mg$cacheE == null)
			{
				PlayerDashboardInformationUI.<>f__mg$cacheE = new ClickedButton(PlayerDashboardInformationUI.onClickedPlayersButton);
			}
			sleekButton6.onClickedButton = PlayerDashboardInformationUI.<>f__mg$cacheE;
			PlayerDashboardInformationUI.headerButtonsContainer.add(PlayerDashboardInformationUI.playersButton);
			PlayerDashboardInformationUI.questsBox = new SleekScrollBox();
			PlayerDashboardInformationUI.questsBox.positionOffset_X = 5;
			PlayerDashboardInformationUI.questsBox.positionOffset_Y = 70;
			PlayerDashboardInformationUI.questsBox.positionScale_X = 0.6f;
			PlayerDashboardInformationUI.questsBox.sizeOffset_X = -15;
			PlayerDashboardInformationUI.questsBox.sizeOffset_Y = -80;
			PlayerDashboardInformationUI.questsBox.sizeScale_X = 0.4f;
			PlayerDashboardInformationUI.questsBox.sizeScale_Y = 1f;
			PlayerDashboardInformationUI.backdropBox.add(PlayerDashboardInformationUI.questsBox);
			PlayerDashboardInformationUI.questsBox.isVisible = false;
			PlayerDashboardInformationUI.groupsBox = new SleekScrollBox();
			PlayerDashboardInformationUI.groupsBox.positionOffset_X = 5;
			PlayerDashboardInformationUI.groupsBox.positionOffset_Y = 70;
			PlayerDashboardInformationUI.groupsBox.positionScale_X = 0.6f;
			PlayerDashboardInformationUI.groupsBox.sizeOffset_X = -15;
			PlayerDashboardInformationUI.groupsBox.sizeOffset_Y = -80;
			PlayerDashboardInformationUI.groupsBox.sizeScale_X = 0.4f;
			PlayerDashboardInformationUI.groupsBox.sizeScale_Y = 1f;
			PlayerDashboardInformationUI.backdropBox.add(PlayerDashboardInformationUI.groupsBox);
			PlayerDashboardInformationUI.groupsBox.isVisible = false;
			PlayerDashboardInformationUI.playersBox = new SleekScrollBox();
			PlayerDashboardInformationUI.playersBox.positionOffset_X = 5;
			PlayerDashboardInformationUI.playersBox.positionOffset_Y = 70;
			PlayerDashboardInformationUI.playersBox.positionScale_X = 0.6f;
			PlayerDashboardInformationUI.playersBox.sizeOffset_X = -15;
			PlayerDashboardInformationUI.playersBox.sizeOffset_Y = -80;
			PlayerDashboardInformationUI.playersBox.sizeScale_X = 0.4f;
			PlayerDashboardInformationUI.playersBox.sizeScale_Y = 1f;
			PlayerDashboardInformationUI.backdropBox.add(PlayerDashboardInformationUI.playersBox);
			PlayerDashboardInformationUI.playersBox.isVisible = true;
			SleekWindow window = PlayerUI.window;
			Delegate onClickedMouseStarted = window.onClickedMouseStarted;
			if (PlayerDashboardInformationUI.<>f__mg$cacheF == null)
			{
				PlayerDashboardInformationUI.<>f__mg$cacheF = new ClickedMouseStarted(PlayerDashboardInformationUI.onClickedMouseStarted);
			}
			window.onClickedMouseStarted = (ClickedMouseStarted)Delegate.Combine(onClickedMouseStarted, PlayerDashboardInformationUI.<>f__mg$cacheF);
			SleekWindow window2 = PlayerUI.window;
			Delegate onClickedMouseStopped = window2.onClickedMouseStopped;
			if (PlayerDashboardInformationUI.<>f__mg$cache10 == null)
			{
				PlayerDashboardInformationUI.<>f__mg$cache10 = new ClickedMouseStopped(PlayerDashboardInformationUI.onClickedMouseStopped);
			}
			window2.onClickedMouseStopped = (ClickedMouseStopped)Delegate.Combine(onClickedMouseStopped, PlayerDashboardInformationUI.<>f__mg$cache10);
			SleekWindow window3 = PlayerUI.window;
			Delegate onMovedMouse = window3.onMovedMouse;
			if (PlayerDashboardInformationUI.<>f__mg$cache11 == null)
			{
				PlayerDashboardInformationUI.<>f__mg$cache11 = new MovedMouse(PlayerDashboardInformationUI.onMovedMouse);
			}
			window3.onMovedMouse = (MovedMouse)Delegate.Combine(onMovedMouse, PlayerDashboardInformationUI.<>f__mg$cache11);
			if (PlayerDashboardInformationUI.<>f__mg$cache12 == null)
			{
				PlayerDashboardInformationUI.<>f__mg$cache12 = new IsBlindfoldedChangedHandler(PlayerDashboardInformationUI.handleIsBlindfoldedChanged);
			}
			PlayerUI.isBlindfoldedChanged += PlayerDashboardInformationUI.<>f__mg$cache12;
			Player player = Player.player;
			Delegate onPlayerTeleported = player.onPlayerTeleported;
			if (PlayerDashboardInformationUI.<>f__mg$cache13 == null)
			{
				PlayerDashboardInformationUI.<>f__mg$cache13 = new PlayerTeleported(PlayerDashboardInformationUI.onPlayerTeleported);
			}
			player.onPlayerTeleported = (PlayerTeleported)Delegate.Combine(onPlayerTeleported, PlayerDashboardInformationUI.<>f__mg$cache13);
			if (PlayerDashboardInformationUI.<>f__mg$cache14 == null)
			{
				PlayerDashboardInformationUI.<>f__mg$cache14 = new GroupUpdatedHandler(PlayerDashboardInformationUI.handleGroupUpdated);
			}
			PlayerQuests.groupUpdated = PlayerDashboardInformationUI.<>f__mg$cache14;
			if (PlayerDashboardInformationUI.<>f__mg$cache15 == null)
			{
				PlayerDashboardInformationUI.<>f__mg$cache15 = new GroupInfoReadyHandler(PlayerDashboardInformationUI.handleGroupInfoReady);
			}
			GroupManager.groupInfoReady += PlayerDashboardInformationUI.<>f__mg$cache15;
			PlayerDashboardInformationUI.onPlayerTeleported(Player.player, Player.player.transform.position);
			if (ReadWrite.fileExists(Level.info.path + "/Chart.png", false, false))
			{
				byte[] array = ReadWrite.readBytes(Level.info.path + "/Chart.png", false, false);
				PlayerDashboardInformationUI.chartTexture = new Texture2D((int)Level.size, (int)Level.size, 5, false, true);
				PlayerDashboardInformationUI.chartTexture.name = "Chart_" + Level.info.name;
				PlayerDashboardInformationUI.chartTexture.filterMode = 2;
				PlayerDashboardInformationUI.chartTexture.LoadImage(array);
			}
			else
			{
				PlayerDashboardInformationUI.chartTexture = null;
			}
			if (ReadWrite.fileExists(Level.info.path + "/Map.png", false, false))
			{
				byte[] array2 = ReadWrite.readBytes(Level.info.path + "/Map.png", false, false);
				PlayerDashboardInformationUI.mapTexture = new Texture2D((int)Level.size, (int)Level.size, 5, false, true);
				PlayerDashboardInformationUI.mapTexture.name = "Satellite_" + Level.info.name;
				PlayerDashboardInformationUI.mapTexture.filterMode = 2;
				PlayerDashboardInformationUI.mapTexture.LoadImage(array2);
			}
			else
			{
				PlayerDashboardInformationUI.mapTexture = null;
			}
			PlayerDashboardInformationUI.staticTexture = (Texture2D)Resources.Load("Level/Map");
		}

		private static void refreshMap(int view)
		{
			PlayerDashboardInformationUI.mapImage.remove();
			if (PlayerDashboardInformationUI.mapImage.texture != null && PlayerDashboardInformationUI.mapImage.shouldDestroyTexture)
			{
				Object.Destroy(PlayerDashboardInformationUI.mapImage.texture);
				PlayerDashboardInformationUI.mapImage.texture = null;
			}
			if (view == 0)
			{
				if (PlayerDashboardInformationUI.chartTexture != null && !PlayerUI.isBlindfolded && (PlayerDashboardInformationUI.hasChart || Provider.modeConfigData.Gameplay.Chart || Level.info.type != ELevelType.SURVIVAL))
				{
					PlayerDashboardInformationUI.mapImage.texture = PlayerDashboardInformationUI.chartTexture;
					PlayerDashboardInformationUI.noLabel.isVisible = false;
				}
				else
				{
					PlayerDashboardInformationUI.mapImage.texture = PlayerDashboardInformationUI.staticTexture;
					PlayerDashboardInformationUI.noLabel.text = PlayerDashboardInformationUI.localization.format("No_Chart");
					PlayerDashboardInformationUI.noLabel.isVisible = true;
				}
			}
			else if (PlayerDashboardInformationUI.mapTexture != null && !PlayerUI.isBlindfolded && (PlayerDashboardInformationUI.hasGPS || Level.info.type != ELevelType.SURVIVAL))
			{
				PlayerDashboardInformationUI.mapImage.texture = PlayerDashboardInformationUI.mapTexture;
				PlayerDashboardInformationUI.noLabel.isVisible = false;
			}
			else
			{
				PlayerDashboardInformationUI.mapImage.texture = PlayerDashboardInformationUI.staticTexture;
				PlayerDashboardInformationUI.noLabel.text = PlayerDashboardInformationUI.localization.format("No_GPS");
				PlayerDashboardInformationUI.noLabel.isVisible = true;
			}
			if (!PlayerDashboardInformationUI.noLabel.isVisible)
			{
				for (int i = 0; i < LevelNodes.nodes.Count; i++)
				{
					Node node = LevelNodes.nodes[i];
					if (node.type == ENodeType.LOCATION)
					{
						SleekLabel sleekLabel = new SleekLabel();
						sleekLabel.positionOffset_X = -200;
						sleekLabel.positionOffset_Y = -30;
						sleekLabel.positionScale_X = node.point.x / (float)(Level.size - Level.border * 2) + 0.5f;
						sleekLabel.positionScale_Y = 0.5f - node.point.z / (float)(Level.size - Level.border * 2);
						sleekLabel.sizeOffset_X = 400;
						sleekLabel.sizeOffset_Y = 60;
						sleekLabel.text = ((LocationNode)node).name;
						sleekLabel.foregroundTint = ESleekTint.FONT;
						PlayerDashboardInformationUI.mapImage.add(sleekLabel);
					}
				}
				if (Provider.modeConfigData.Gameplay.Group_Map)
				{
					if (LevelManager.levelType == ELevelType.ARENA)
					{
						SleekImageTexture sleekImageTexture = new SleekImageTexture((Texture2D)PlayerDashboardInformationUI.icons.load("Area"));
						sleekImageTexture.positionScale_X = LevelManager.arenaCenter.x / (float)(Level.size - Level.border * 2) + 0.5f - LevelManager.arenaRadius / (float)(Level.size - Level.border * 2);
						sleekImageTexture.positionScale_Y = 0.5f - LevelManager.arenaCenter.z / (float)(Level.size - Level.border * 2) - LevelManager.arenaRadius / (float)(Level.size - Level.border * 2);
						sleekImageTexture.sizeScale_X = LevelManager.arenaRadius * 2f / (float)(Level.size - Level.border * 2);
						sleekImageTexture.sizeScale_Y = LevelManager.arenaRadius * 2f / (float)(Level.size - Level.border * 2);
						PlayerDashboardInformationUI.mapImage.add(sleekImageTexture);
						SleekImageTexture sleekImageTexture2 = new SleekImageTexture((Texture2D)Resources.Load("Materials/Pixel"));
						sleekImageTexture2.positionScale_Y = sleekImageTexture.positionScale_Y;
						sleekImageTexture2.sizeScale_X = sleekImageTexture.positionScale_X;
						sleekImageTexture2.sizeScale_Y = sleekImageTexture.sizeScale_Y;
						PlayerDashboardInformationUI.mapImage.add(sleekImageTexture2);
						SleekImageTexture sleekImageTexture3 = new SleekImageTexture((Texture2D)Resources.Load("Materials/Pixel"));
						sleekImageTexture3.positionScale_X = sleekImageTexture.positionScale_X + sleekImageTexture.sizeScale_X;
						sleekImageTexture3.positionScale_Y = sleekImageTexture.positionScale_Y;
						sleekImageTexture3.sizeScale_X = 1f - sleekImageTexture.positionScale_X - sleekImageTexture.sizeScale_X;
						sleekImageTexture3.sizeScale_Y = sleekImageTexture.sizeScale_Y;
						PlayerDashboardInformationUI.mapImage.add(sleekImageTexture3);
						SleekImageTexture sleekImageTexture4 = new SleekImageTexture((Texture2D)Resources.Load("Materials/Pixel"));
						sleekImageTexture4.sizeScale_X = 1f;
						sleekImageTexture4.sizeScale_Y = sleekImageTexture.positionScale_Y;
						PlayerDashboardInformationUI.mapImage.add(sleekImageTexture4);
						SleekImageTexture sleekImageTexture5 = new SleekImageTexture((Texture2D)Resources.Load("Materials/Pixel"));
						sleekImageTexture5.positionScale_Y = sleekImageTexture.positionScale_Y + sleekImageTexture.sizeScale_Y;
						sleekImageTexture5.sizeScale_X = 1f;
						sleekImageTexture5.sizeScale_Y = 1f - sleekImageTexture.positionScale_Y - sleekImageTexture.sizeScale_Y;
						PlayerDashboardInformationUI.mapImage.add(sleekImageTexture5);
					}
					for (int j = 0; j < Provider.clients.Count; j++)
					{
						SteamPlayer steamPlayer = Provider.clients[j];
						if (steamPlayer.playerID.steamID != Provider.client && steamPlayer.model != null && steamPlayer.player.quests.isMemberOfSameGroupAs(Player.player))
						{
							SleekImageTexture sleekImageTexture6 = new SleekImageTexture();
							sleekImageTexture6.positionOffset_X = -10;
							sleekImageTexture6.positionOffset_Y = -10;
							sleekImageTexture6.positionScale_X = steamPlayer.player.transform.position.x / (float)(Level.size - Level.border * 2) + 0.5f;
							sleekImageTexture6.positionScale_Y = 0.5f - steamPlayer.player.transform.position.z / (float)(Level.size - Level.border * 2);
							sleekImageTexture6.sizeOffset_X = 20;
							sleekImageTexture6.sizeOffset_Y = 20;
							if (!OptionsSettings.streamer)
							{
								sleekImageTexture6.texture = Provider.provider.communityService.getIcon(steamPlayer.playerID.steamID);
							}
							if (string.IsNullOrEmpty(steamPlayer.playerID.nickName))
							{
								sleekImageTexture6.addLabel(steamPlayer.playerID.characterName, ESleekSide.RIGHT);
							}
							else
							{
								sleekImageTexture6.addLabel(steamPlayer.playerID.nickName, ESleekSide.RIGHT);
							}
							sleekImageTexture6.shouldDestroyTexture = true;
							PlayerDashboardInformationUI.mapImage.add(sleekImageTexture6);
						}
					}
					if (Player.player != null)
					{
						SleekImageTexture sleekImageTexture7 = new SleekImageTexture();
						sleekImageTexture7.positionOffset_X = -10;
						sleekImageTexture7.positionOffset_Y = -10;
						sleekImageTexture7.positionScale_X = Player.player.transform.position.x / (float)(Level.size - Level.border * 2) + 0.5f;
						sleekImageTexture7.positionScale_Y = 0.5f - Player.player.transform.position.z / (float)(Level.size - Level.border * 2);
						sleekImageTexture7.sizeOffset_X = 20;
						sleekImageTexture7.sizeOffset_Y = 20;
						sleekImageTexture7.isAngled = true;
						sleekImageTexture7.angle = Player.player.transform.rotation.eulerAngles.y;
						sleekImageTexture7.texture = (Texture2D)PlayerDashboardInformationUI.icons.load("Player");
						sleekImageTexture7.backgroundTint = ESleekTint.FOREGROUND;
						if (string.IsNullOrEmpty(Characters.active.nick))
						{
							sleekImageTexture7.addLabel(Characters.active.name, ESleekSide.RIGHT);
						}
						else
						{
							sleekImageTexture7.addLabel(Characters.active.nick, ESleekSide.RIGHT);
						}
						PlayerDashboardInformationUI.mapImage.add(sleekImageTexture7);
					}
				}
			}
		}

		public static void open()
		{
			if (PlayerDashboardInformationUI.active)
			{
				return;
			}
			PlayerDashboardInformationUI.active = true;
			InventorySearch inventorySearch = Player.player.inventory.has(1175);
			PlayerDashboardInformationUI.hasChart = (inventorySearch != null);
			InventorySearch inventorySearch2 = Player.player.inventory.has(1176);
			PlayerDashboardInformationUI.hasGPS = (inventorySearch2 != null);
			if (PlayerDashboardInformationUI.hasChart && !PlayerDashboardInformationUI.hasGPS)
			{
				PlayerDashboardInformationUI.mapButtonState.state = 0;
			}
			if (PlayerDashboardInformationUI.hasGPS && !PlayerDashboardInformationUI.hasChart)
			{
				PlayerDashboardInformationUI.mapButtonState.state = 1;
			}
			PlayerDashboardInformationUI.refreshMap(PlayerDashboardInformationUI.mapButtonState.state);
			PlayerDashboardInformationUI.questsButton.text = PlayerDashboardInformationUI.localization.format("Quests", new object[]
			{
				Player.player.quests.questsList.Count
			});
			if (OptionsSettings.streamer)
			{
				PlayerDashboardInformationUI.playersButton.text = PlayerDashboardInformationUI.localization.format("Streamer");
			}
			else
			{
				PlayerDashboardInformationUI.playersButton.text = PlayerDashboardInformationUI.localization.format("Players", new object[]
				{
					Provider.clients.Count,
					Provider.maxPlayers
				});
			}
			PlayerDashboardInformationUI.EInfoTab einfoTab = PlayerDashboardInformationUI.tab;
			if (einfoTab != PlayerDashboardInformationUI.EInfoTab.GROUPS)
			{
				if (einfoTab != PlayerDashboardInformationUI.EInfoTab.QUESTS)
				{
					if (einfoTab == PlayerDashboardInformationUI.EInfoTab.PLAYERS)
					{
						PlayerDashboardInformationUI.openPlayers();
					}
				}
				else
				{
					PlayerDashboardInformationUI.openQuests();
				}
			}
			else
			{
				PlayerDashboardInformationUI.openGroups();
			}
			PlayerDashboardInformationUI.container.lerpPositionScale(0f, 0f, ESleekLerp.EXPONENTIAL, 20f);
		}

		public static void close()
		{
			if (!PlayerDashboardInformationUI.active)
			{
				return;
			}
			PlayerDashboardInformationUI.active = false;
			PlayerDashboardInformationUI.isDragging = false;
			PlayerDashboardInformationUI.container.lerpPositionScale(0f, 1f, ESleekLerp.EXPONENTIAL, 20f);
		}

		public static void openQuests()
		{
			PlayerDashboardInformationUI.tab = PlayerDashboardInformationUI.EInfoTab.QUESTS;
			PlayerDashboardInformationUI.questsBox.remove();
			for (int i = 0; i < Player.player.quests.questsList.Count; i++)
			{
				PlayerQuest playerQuest = Player.player.quests.questsList[i];
				if (playerQuest != null && playerQuest.asset != null)
				{
					bool flag = playerQuest.asset.areConditionsMet(Player.player);
					SleekButton sleekButton = new SleekButton();
					sleekButton.positionOffset_Y = i * 60;
					sleekButton.sizeOffset_X = -30;
					sleekButton.sizeOffset_Y = 50;
					sleekButton.sizeScale_X = 1f;
					SleekButton sleekButton2 = sleekButton;
					if (PlayerDashboardInformationUI.<>f__mg$cache0 == null)
					{
						PlayerDashboardInformationUI.<>f__mg$cache0 = new ClickedButton(PlayerDashboardInformationUI.onClickedQuestButton);
					}
					sleekButton2.onClickedButton = PlayerDashboardInformationUI.<>f__mg$cache0;
					PlayerDashboardInformationUI.questsBox.add(sleekButton);
					sleekButton.add(new SleekImageTexture((Texture2D)PlayerDashboardInformationUI.icons.load((!flag) ? "Incomplete" : "Complete"))
					{
						positionOffset_X = 5,
						positionOffset_Y = 5,
						sizeOffset_X = 40,
						sizeOffset_Y = 40
					});
					sleekButton.add(new SleekLabel
					{
						positionOffset_X = 50,
						sizeOffset_X = -55,
						sizeScale_X = 1f,
						sizeScale_Y = 1f,
						fontAlignment = 3,
						foregroundTint = ESleekTint.NONE,
						isRich = true,
						fontSize = 14,
						text = playerQuest.asset.questName
					});
				}
			}
			PlayerDashboardInformationUI.questsBox.area = new Rect(0f, 0f, 5f, (float)(Player.player.quests.questsList.Count * 60 - 10));
			PlayerDashboardInformationUI.updateTabs();
		}

		private static void onClickedTuneButton(SleekButton button)
		{
			uint num = (uint)(PlayerDashboardInformationUI.radioFrequencyField.state * 1000.0);
			if (num < 300000u)
			{
				num = 300000u;
			}
			else if (num > 900000u)
			{
				num = 900000u;
			}
			PlayerDashboardInformationUI.radioFrequencyField.state = num / 1000.0;
			Player.player.quests.sendSetRadioFrequency(num);
		}

		private static void onClickedResetButton(SleekButton button)
		{
			PlayerDashboardInformationUI.radioFrequencyField.state = PlayerQuests.DEFAULT_RADIO_FREQUENCY / 1000.0;
			PlayerDashboardInformationUI.onClickedTuneButton(button);
		}

		private static void onClickedRenameButton(SleekButton button)
		{
			Player.player.quests.sendRenameGroup(PlayerDashboardInformationUI.groupNameField.text);
		}

		private static void onClickedMainGroupButton(SleekButton button)
		{
			Player.player.quests.sendJoinGroupInvite(Characters.active.group);
		}

		private static void onClickedLeaveGroupButton(SleekButton button)
		{
			Player.player.quests.sendLeaveGroup();
		}

		private static void onClickedDeleteGroupButton(SleekButton button)
		{
			Player.player.quests.sendDeleteGroup();
		}

		private static void onClickedCreateGroupButton(SleekButton button)
		{
			Player.player.quests.sendCreateGroup();
		}

		private static void refreshGroups()
		{
			if (!PlayerDashboardInformationUI.active)
			{
				return;
			}
			PlayerDashboardInformationUI.groupsBox.remove();
			int num = 0;
			PlayerDashboardInformationUI.radioFrequencyField = new SleekDoubleField();
			PlayerDashboardInformationUI.radioFrequencyField.positionOffset_X = 125;
			PlayerDashboardInformationUI.radioFrequencyField.sizeOffset_X = -255;
			PlayerDashboardInformationUI.radioFrequencyField.positionOffset_Y = num;
			PlayerDashboardInformationUI.radioFrequencyField.sizeOffset_Y = 30;
			PlayerDashboardInformationUI.radioFrequencyField.sizeScale_X = 1f;
			PlayerDashboardInformationUI.radioFrequencyField.state = Player.player.quests.radioFrequency / 1000.0;
			PlayerDashboardInformationUI.groupsBox.add(PlayerDashboardInformationUI.radioFrequencyField);
			num += 30;
			SleekBox sleekBox = new SleekBox();
			sleekBox.positionOffset_X = -125;
			sleekBox.sizeOffset_X = 125;
			sleekBox.sizeScale_Y = 1f;
			sleekBox.text = PlayerDashboardInformationUI.localization.format("Radio_Frequency_Label");
			PlayerDashboardInformationUI.radioFrequencyField.add(sleekBox);
			SleekButton sleekButton = new SleekButton();
			sleekButton.positionScale_X = 1f;
			sleekButton.sizeOffset_X = 50;
			sleekButton.sizeScale_Y = 1f;
			sleekButton.text = PlayerDashboardInformationUI.localization.format("Radio_Frequency_Tune");
			sleekButton.tooltip = PlayerDashboardInformationUI.localization.format("Radio_Frequency_Tune_Tooltip");
			SleekButton sleekButton2 = sleekButton;
			Delegate onClickedButton = sleekButton2.onClickedButton;
			if (PlayerDashboardInformationUI.<>f__mg$cache1 == null)
			{
				PlayerDashboardInformationUI.<>f__mg$cache1 = new ClickedButton(PlayerDashboardInformationUI.onClickedTuneButton);
			}
			sleekButton2.onClickedButton = (ClickedButton)Delegate.Combine(onClickedButton, PlayerDashboardInformationUI.<>f__mg$cache1);
			PlayerDashboardInformationUI.radioFrequencyField.add(sleekButton);
			SleekButton sleekButton3 = new SleekButton();
			sleekButton3.positionOffset_X = 50;
			sleekButton3.positionScale_X = 1f;
			sleekButton3.sizeOffset_X = 50;
			sleekButton3.sizeScale_Y = 1f;
			sleekButton3.text = PlayerDashboardInformationUI.localization.format("Radio_Frequency_Reset");
			sleekButton3.tooltip = PlayerDashboardInformationUI.localization.format("Radio_Frequency_Reset_Tooltip");
			SleekButton sleekButton4 = sleekButton3;
			Delegate onClickedButton2 = sleekButton4.onClickedButton;
			if (PlayerDashboardInformationUI.<>f__mg$cache2 == null)
			{
				PlayerDashboardInformationUI.<>f__mg$cache2 = new ClickedButton(PlayerDashboardInformationUI.onClickedResetButton);
			}
			sleekButton4.onClickedButton = (ClickedButton)Delegate.Combine(onClickedButton2, PlayerDashboardInformationUI.<>f__mg$cache2);
			PlayerDashboardInformationUI.radioFrequencyField.add(sleekButton3);
			PlayerQuests quests = Player.player.quests;
			if (quests.isMemberOfAGroup)
			{
				if (Characters.active.group == quests.groupID)
				{
					SteamGroup cachedGroup = Provider.provider.communityService.getCachedGroup(Characters.active.group);
					if (cachedGroup != null)
					{
						SleekBoxIcon sleekBoxIcon = new SleekBoxIcon(cachedGroup.icon, 40);
						sleekBoxIcon.positionOffset_Y = num;
						sleekBoxIcon.sizeOffset_X = -30;
						sleekBoxIcon.sizeOffset_Y = 50;
						sleekBoxIcon.sizeScale_X = 1f;
						sleekBoxIcon.text = cachedGroup.name;
						PlayerDashboardInformationUI.groupsBox.add(sleekBoxIcon);
						num += 50;
					}
				}
				else
				{
					GroupInfo groupInfo = GroupManager.getGroupInfo(quests.groupID);
					string text = (groupInfo == null) ? quests.groupID.ToString() : groupInfo.name;
					Sleek sleek;
					if (quests.groupRank == EPlayerGroupRank.OWNER)
					{
						PlayerDashboardInformationUI.groupNameField = new SleekField();
						PlayerDashboardInformationUI.groupNameField.maxLength = 32;
						PlayerDashboardInformationUI.groupNameField.text = text;
						sleek = PlayerDashboardInformationUI.groupNameField;
						sleek.sizeOffset_X = -130;
						SleekButton sleekButton5 = new SleekButton();
						sleekButton5.positionScale_X = 1f;
						sleekButton5.sizeOffset_X = 100;
						sleekButton5.sizeScale_Y = 1f;
						sleekButton5.text = PlayerDashboardInformationUI.localization.format("Group_Rename");
						sleekButton5.tooltip = PlayerDashboardInformationUI.localization.format("Group_Rename_Tooltip");
						SleekButton sleekButton6 = sleekButton5;
						Delegate onClickedButton3 = sleekButton6.onClickedButton;
						if (PlayerDashboardInformationUI.<>f__mg$cache3 == null)
						{
							PlayerDashboardInformationUI.<>f__mg$cache3 = new ClickedButton(PlayerDashboardInformationUI.onClickedRenameButton);
						}
						sleekButton6.onClickedButton = (ClickedButton)Delegate.Combine(onClickedButton3, PlayerDashboardInformationUI.<>f__mg$cache3);
						PlayerDashboardInformationUI.groupNameField.add(sleekButton5);
					}
					else
					{
						sleek = new SleekBox
						{
							text = text
						};
						sleek.sizeOffset_X = -30;
					}
					sleek.positionOffset_Y = num;
					sleek.sizeOffset_Y = 30;
					sleek.sizeScale_X = 1f;
					PlayerDashboardInformationUI.groupsBox.add(sleek);
					num += 30;
				}
				if (quests.hasPermissionToLeaveGroup)
				{
					SleekButtonIcon sleekButtonIcon = new SleekButtonIcon((Texture2D)MenuWorkshopEditorUI.icons.load("Remove"));
					sleekButtonIcon.positionOffset_Y = num;
					sleekButtonIcon.sizeOffset_X = -30;
					sleekButtonIcon.sizeOffset_Y = 30;
					sleekButtonIcon.sizeScale_X = 1f;
					sleekButtonIcon.text = PlayerDashboardInformationUI.localization.format("Group_Leave");
					sleekButtonIcon.tooltip = PlayerDashboardInformationUI.localization.format("Group_Leave_Tooltip");
					SleekButtonIcon sleekButtonIcon2 = sleekButtonIcon;
					Delegate onClickedButton4 = sleekButtonIcon2.onClickedButton;
					if (PlayerDashboardInformationUI.<>f__mg$cache4 == null)
					{
						PlayerDashboardInformationUI.<>f__mg$cache4 = new ClickedButton(PlayerDashboardInformationUI.onClickedLeaveGroupButton);
					}
					sleekButtonIcon2.onClickedButton = (ClickedButton)Delegate.Combine(onClickedButton4, PlayerDashboardInformationUI.<>f__mg$cache4);
					PlayerDashboardInformationUI.groupsBox.add(sleekButtonIcon);
					num += 30;
				}
				if (quests.hasPermissionToDeleteGroup)
				{
					SleekButtonIconConfirm sleekButtonIconConfirm = new SleekButtonIconConfirm((Texture2D)MenuWorkshopEditorUI.icons.load("Remove"), PlayerDashboardInformationUI.localization.format("Group_Delete_Confirm"), PlayerDashboardInformationUI.localization.format("Group_Delete_Confirm_Tooltip"), PlayerDashboardInformationUI.localization.format("Group_Delete_Deny"), PlayerDashboardInformationUI.localization.format("Group_Delete_Deny_Tooltip"));
					sleekButtonIconConfirm.positionOffset_Y = num;
					sleekButtonIconConfirm.sizeOffset_X = -30;
					sleekButtonIconConfirm.sizeOffset_Y = 30;
					sleekButtonIconConfirm.sizeScale_X = 1f;
					sleekButtonIconConfirm.text = PlayerDashboardInformationUI.localization.format("Group_Delete");
					sleekButtonIconConfirm.tooltip = PlayerDashboardInformationUI.localization.format("Group_Delete_Tooltip");
					SleekButtonIconConfirm sleekButtonIconConfirm2 = sleekButtonIconConfirm;
					Delegate onConfirmed = sleekButtonIconConfirm2.onConfirmed;
					if (PlayerDashboardInformationUI.<>f__mg$cache5 == null)
					{
						PlayerDashboardInformationUI.<>f__mg$cache5 = new Confirm(PlayerDashboardInformationUI.onClickedDeleteGroupButton);
					}
					sleekButtonIconConfirm2.onConfirmed = (Confirm)Delegate.Combine(onConfirmed, PlayerDashboardInformationUI.<>f__mg$cache5);
					PlayerDashboardInformationUI.groupsBox.add(sleekButtonIconConfirm);
					num += 30;
				}
				foreach (SteamPlayer steamPlayer in Provider.clients)
				{
					if (!(steamPlayer.player == null) && steamPlayer.player.quests.isMemberOfSameGroupAs(Player.player))
					{
						SleekPlayer sleekPlayer = new SleekPlayer(steamPlayer, true, SleekPlayer.ESleekPlayerDisplayContext.GROUP_ROSTER);
						sleekPlayer.positionOffset_Y = num;
						sleekPlayer.sizeOffset_X = -30;
						sleekPlayer.sizeOffset_Y = 50;
						sleekPlayer.sizeScale_X = 1f;
						PlayerDashboardInformationUI.groupsBox.add(sleekPlayer);
						num += 50;
					}
				}
			}
			else
			{
				if (Characters.active.group != CSteamID.Nil)
				{
					SteamGroup cachedGroup2 = Provider.provider.communityService.getCachedGroup(Characters.active.group);
					if (cachedGroup2 != null)
					{
						SleekButtonIcon sleekButtonIcon3 = new SleekButtonIcon(cachedGroup2.icon, 40);
						sleekButtonIcon3.positionOffset_Y = num;
						sleekButtonIcon3.sizeOffset_X = -30;
						sleekButtonIcon3.sizeOffset_Y = 50;
						sleekButtonIcon3.sizeScale_X = 1f;
						sleekButtonIcon3.text = cachedGroup2.name;
						SleekButton sleekButton7 = sleekButtonIcon3;
						if (PlayerDashboardInformationUI.<>f__mg$cache6 == null)
						{
							PlayerDashboardInformationUI.<>f__mg$cache6 = new ClickedButton(PlayerDashboardInformationUI.onClickedMainGroupButton);
						}
						sleekButton7.onClickedButton = PlayerDashboardInformationUI.<>f__mg$cache6;
						PlayerDashboardInformationUI.groupsBox.add(sleekButtonIcon3);
						num += 50;
					}
				}
				foreach (CSteamID newGroupID in quests.groupInvites)
				{
					PlayerDashboardInformationUI.SleekInviteButton sleekInviteButton = new PlayerDashboardInformationUI.SleekInviteButton(newGroupID);
					sleekInviteButton.positionOffset_Y = num;
					sleekInviteButton.sizeOffset_X = -30;
					sleekInviteButton.sizeOffset_Y = 30;
					sleekInviteButton.sizeScale_X = 1f;
					PlayerDashboardInformationUI.groupsBox.add(sleekInviteButton);
					num += 30;
				}
				SleekButtonIcon sleekButtonIcon4 = new SleekButtonIcon((Texture2D)MenuWorkshopEditorUI.icons.load("Add"));
				sleekButtonIcon4.positionOffset_Y = num;
				sleekButtonIcon4.sizeOffset_X = -30;
				sleekButtonIcon4.sizeOffset_Y = 30;
				sleekButtonIcon4.sizeScale_X = 1f;
				sleekButtonIcon4.text = PlayerDashboardInformationUI.localization.format("Group_Create");
				sleekButtonIcon4.tooltip = PlayerDashboardInformationUI.localization.format("Group_Create_Tooltip");
				SleekButtonIcon sleekButtonIcon5 = sleekButtonIcon4;
				Delegate onClickedButton5 = sleekButtonIcon5.onClickedButton;
				if (PlayerDashboardInformationUI.<>f__mg$cache7 == null)
				{
					PlayerDashboardInformationUI.<>f__mg$cache7 = new ClickedButton(PlayerDashboardInformationUI.onClickedCreateGroupButton);
				}
				sleekButtonIcon5.onClickedButton = (ClickedButton)Delegate.Combine(onClickedButton5, PlayerDashboardInformationUI.<>f__mg$cache7);
				PlayerDashboardInformationUI.groupsBox.add(sleekButtonIcon4);
				num += 30;
			}
			PlayerDashboardInformationUI.groupsBox.area = new Rect(0f, 0f, 5f, (float)num);
		}

		private static void handleGroupUpdated(PlayerQuests sender)
		{
			PlayerDashboardInformationUI.refreshGroups();
		}

		private static void handleGroupInfoReady(GroupInfo group)
		{
			PlayerDashboardInformationUI.refreshGroups();
		}

		public static void openGroups()
		{
			PlayerDashboardInformationUI.tab = PlayerDashboardInformationUI.EInfoTab.GROUPS;
			PlayerDashboardInformationUI.refreshGroups();
			PlayerDashboardInformationUI.updateTabs();
		}

		public static void openPlayers()
		{
			PlayerDashboardInformationUI.tab = PlayerDashboardInformationUI.EInfoTab.PLAYERS;
			if (OptionsSettings.streamer)
			{
				PlayerDashboardInformationUI.playersBox.remove();
				PlayerDashboardInformationUI.playersBox.area = new Rect(0f, 0f, 5f, 0f);
			}
			else
			{
				PlayerDashboardInformationUI.playersBox.remove();
				PlayerDashboardInformationUI.SORTED_CLIENTS.Clear();
				for (int i = 0; i < Provider.clients.Count; i++)
				{
					int num = PlayerDashboardInformationUI.SORTED_CLIENTS.BinarySearch(Provider.clients[i], PlayerDashboardInformationUI.GROUP_COMPARATOR);
					if (num < 0)
					{
						num = ~num;
					}
					PlayerDashboardInformationUI.SORTED_CLIENTS.Insert(num, Provider.clients[i]);
				}
				if (PlayerDashboardInformationUI.SORTED_CLIENTS.Count > 1)
				{
					for (int j = 1; j < PlayerDashboardInformationUI.SORTED_CLIENTS.Count; j++)
					{
						if (PlayerDashboardInformationUI.SORTED_CLIENTS[j - 1].player.quests.isMemberOfSameGroupAs(PlayerDashboardInformationUI.SORTED_CLIENTS[j].player))
						{
							SleekImageTexture sleekImageTexture = new SleekImageTexture((Texture2D)PlayerDashboardInformationUI.icons.load("Group"));
							sleekImageTexture.positionOffset_X = 21;
							sleekImageTexture.positionOffset_Y = j * 60 - 13;
							sleekImageTexture.sizeOffset_X = 8;
							sleekImageTexture.sizeOffset_Y = 16;
							sleekImageTexture.backgroundTint = ESleekTint.FOREGROUND;
							PlayerDashboardInformationUI.playersBox.add(sleekImageTexture);
						}
					}
				}
				for (int k = 0; k < PlayerDashboardInformationUI.SORTED_CLIENTS.Count; k++)
				{
					SteamPlayer newPlayer = PlayerDashboardInformationUI.SORTED_CLIENTS[k];
					SleekPlayer sleekPlayer = new SleekPlayer(newPlayer, true, SleekPlayer.ESleekPlayerDisplayContext.PLAYER_LIST);
					sleekPlayer.positionOffset_Y = k * 60;
					sleekPlayer.sizeOffset_X = -30;
					sleekPlayer.sizeOffset_Y = 50;
					sleekPlayer.sizeScale_X = 1f;
					PlayerDashboardInformationUI.playersBox.add(sleekPlayer);
				}
				PlayerDashboardInformationUI.playersBox.area = new Rect(0f, 0f, 5f, (float)(PlayerDashboardInformationUI.SORTED_CLIENTS.Count * 60 - 10));
			}
			PlayerDashboardInformationUI.updateTabs();
		}

		private static void updateTabs()
		{
			PlayerDashboardInformationUI.questsBox.isVisible = (PlayerDashboardInformationUI.tab == PlayerDashboardInformationUI.EInfoTab.QUESTS);
			PlayerDashboardInformationUI.groupsBox.isVisible = (PlayerDashboardInformationUI.tab == PlayerDashboardInformationUI.EInfoTab.GROUPS);
			PlayerDashboardInformationUI.playersBox.isVisible = (PlayerDashboardInformationUI.tab == PlayerDashboardInformationUI.EInfoTab.PLAYERS);
		}

		private static void updateZoom()
		{
			if (PlayerDashboardInformationUI.zoom == 0 || (float)(1024 * (int)PlayerDashboardInformationUI.zoom) < PlayerDashboardInformationUI.mapBox.frame.height)
			{
				PlayerDashboardInformationUI.mapBox.area = new Rect(0f, 0f, PlayerDashboardInformationUI.mapBox.frame.width, PlayerDashboardInformationUI.mapBox.frame.height);
				PlayerDashboardInformationUI.mapImage.sizeOffset_X = (int)PlayerDashboardInformationUI.mapBox.frame.width;
				PlayerDashboardInformationUI.mapImage.sizeOffset_Y = (int)PlayerDashboardInformationUI.mapBox.frame.height;
			}
			else
			{
				PlayerDashboardInformationUI.mapBox.area = new Rect(0f, 0f, (float)(1024 * (int)PlayerDashboardInformationUI.zoom), (float)(1024 * (int)PlayerDashboardInformationUI.zoom));
				PlayerDashboardInformationUI.mapImage.sizeOffset_X = 1024 * (int)PlayerDashboardInformationUI.zoom;
				PlayerDashboardInformationUI.mapImage.sizeOffset_Y = 1024 * (int)PlayerDashboardInformationUI.zoom;
			}
		}

		public static void focusPoint(Vector3 point)
		{
			PlayerDashboardInformationUI.mapBox.state = new Vector2((point.x / (float)(Level.size - Level.border * 2) + 0.5f) * PlayerDashboardInformationUI.mapBox.area.width, (0.5f - point.z / (float)(Level.size - Level.border * 2)) * PlayerDashboardInformationUI.mapBox.area.height);
			PlayerDashboardInformationUI.mapBox.state -= new Vector2(PlayerDashboardInformationUI.mapBox.frame.width / 2f, PlayerDashboardInformationUI.mapBox.frame.width / 2f);
		}

		private static void onClickedMouseStarted()
		{
			if (PlayerUI.window.mouse_x > PlayerDashboardInformationUI.mapBox.frame.xMin && PlayerUI.window.mouse_x < PlayerDashboardInformationUI.mapBox.frame.xMax && PlayerUI.window.mouse_y > PlayerDashboardInformationUI.mapBox.frame.yMin && PlayerUI.window.mouse_y < PlayerDashboardInformationUI.mapBox.frame.yMax)
			{
				PlayerDashboardInformationUI.isDragging = true;
				PlayerDashboardInformationUI.dragOrigin.x = PlayerUI.window.mouse_x;
				PlayerDashboardInformationUI.dragOrigin.y = PlayerUI.window.mouse_y;
				PlayerDashboardInformationUI.dragOffset.x = PlayerDashboardInformationUI.mapBox.state.x;
				PlayerDashboardInformationUI.dragOffset.y = PlayerDashboardInformationUI.mapBox.state.y;
			}
		}

		private static void onClickedMouseStopped()
		{
			if (PlayerDashboardInformationUI.isDragging)
			{
				PlayerDashboardInformationUI.isDragging = false;
				PlayerDashboardInformationUI.dragOrigin = Vector2.zero;
				PlayerDashboardInformationUI.dragOffset = Vector2.zero;
			}
		}

		private static void onMovedMouse(float x, float y)
		{
			if (PlayerDashboardInformationUI.isDragging)
			{
				PlayerDashboardInformationUI.mapBox.state.x = PlayerDashboardInformationUI.dragOffset.x - x + PlayerDashboardInformationUI.dragOrigin.x;
				PlayerDashboardInformationUI.mapBox.state.y = PlayerDashboardInformationUI.dragOffset.y - y + PlayerDashboardInformationUI.dragOrigin.y;
			}
		}

		private static void onClickedZoomInButton(SleekButton button)
		{
			if ((ushort)PlayerDashboardInformationUI.zoom < Level.size / 1024)
			{
				PlayerDashboardInformationUI.zoom += 1;
				Vector2 vector = PlayerDashboardInformationUI.mapBox.state + new Vector2(PlayerDashboardInformationUI.mapBox.frame.width / 2f, PlayerDashboardInformationUI.mapBox.frame.width / 2f);
				Vector2 state;
				state..ctor(vector.x / PlayerDashboardInformationUI.mapBox.area.width, vector.y / PlayerDashboardInformationUI.mapBox.area.height);
				PlayerDashboardInformationUI.updateZoom();
				state..ctor(state.x * PlayerDashboardInformationUI.mapBox.area.width, state.y * PlayerDashboardInformationUI.mapBox.area.height);
				PlayerDashboardInformationUI.mapBox.state = state;
				PlayerDashboardInformationUI.mapBox.state -= new Vector2(PlayerDashboardInformationUI.mapBox.frame.width / 2f, PlayerDashboardInformationUI.mapBox.frame.width / 2f);
				PlayerDashboardInformationUI.isDragging = false;
				PlayerDashboardInformationUI.dragOrigin = Vector2.zero;
				PlayerDashboardInformationUI.dragOffset = Vector2.zero;
			}
		}

		private static void onClickedZoomOutButton(SleekButton button)
		{
			if (PlayerDashboardInformationUI.zoom > 0)
			{
				PlayerDashboardInformationUI.zoom -= 1;
				Vector2 vector = PlayerDashboardInformationUI.mapBox.state + new Vector2(PlayerDashboardInformationUI.mapBox.frame.width / 2f, PlayerDashboardInformationUI.mapBox.frame.width / 2f);
				Vector2 state;
				state..ctor(vector.x / PlayerDashboardInformationUI.mapBox.area.width, vector.y / PlayerDashboardInformationUI.mapBox.area.height);
				PlayerDashboardInformationUI.updateZoom();
				state..ctor(state.x * PlayerDashboardInformationUI.mapBox.area.width, state.y * PlayerDashboardInformationUI.mapBox.area.height);
				PlayerDashboardInformationUI.mapBox.state = state;
				PlayerDashboardInformationUI.mapBox.state -= new Vector2(PlayerDashboardInformationUI.mapBox.frame.width / 2f, PlayerDashboardInformationUI.mapBox.frame.width / 2f);
				PlayerDashboardInformationUI.isDragging = false;
				PlayerDashboardInformationUI.dragOrigin = Vector2.zero;
				PlayerDashboardInformationUI.dragOffset = Vector2.zero;
			}
		}

		private static void onClickedCenterButton(SleekButton button)
		{
			PlayerDashboardInformationUI.focusPoint(Player.player.transform.position);
		}

		private static void onSwappedMapState(SleekButtonState button, int index)
		{
			PlayerDashboardInformationUI.refreshMap(index);
		}

		private static void onClickedQuestButton(SleekButton button)
		{
			int index = PlayerDashboardInformationUI.questsBox.search(button);
			PlayerQuest playerQuest = Player.player.quests.questsList[index];
			PlayerDashboardUI.close();
			PlayerNPCQuestUI.open(playerQuest.asset, null, null, null, EQuestViewMode.DETAILS);
		}

		private static void onClickedQuestsButton(SleekButton button)
		{
			PlayerDashboardInformationUI.openQuests();
		}

		private static void onClickedGroupsButton(SleekButton button)
		{
			PlayerDashboardInformationUI.openGroups();
		}

		private static void onClickedPlayersButton(SleekButton button)
		{
			PlayerDashboardInformationUI.openPlayers();
		}

		private static void handleIsBlindfoldedChanged()
		{
			PlayerDashboardInformationUI.refreshMap(PlayerDashboardInformationUI.mapButtonState.state);
		}

		private static void onPlayerTeleported(Player player, Vector3 point)
		{
			PlayerDashboardInformationUI.focusPoint(point);
		}

		private static readonly List<SteamPlayer> SORTED_CLIENTS = new List<SteamPlayer>();

		private static readonly SteamPlayerGroupAscendingComparator GROUP_COMPARATOR = new SteamPlayerGroupAscendingComparator();

		public static Local localization;

		public static Bundle icons;

		private static Sleek container;

		public static bool active;

		private static byte zoom;

		private static SleekBox backdropBox;

		private static bool isDragging;

		private static Vector2 dragOrigin;

		private static Vector2 dragOffset;

		private static Sleek mapInspect;

		private static SleekViewBox mapBox;

		private static SleekImageTexture mapImage;

		private static SleekButtonIcon zoomInButton;

		private static SleekButtonIcon zoomOutButton;

		private static SleekButtonIcon centerButton;

		private static SleekButtonState mapButtonState;

		private static SleekLabel noLabel;

		private static Sleek headerButtonsContainer;

		private static SleekButtonIcon questsButton;

		private static SleekButtonIcon groupsButton;

		private static SleekButtonIcon playersButton;

		private static SleekScrollBox questsBox;

		private static SleekScrollBox groupsBox;

		private static SleekScrollBox playersBox;

		private static SleekDoubleField radioFrequencyField;

		private static SleekField groupNameField;

		private static bool hasChart;

		private static bool hasGPS;

		private static PlayerDashboardInformationUI.EInfoTab tab;

		private static Texture2D mapTexture;

		private static Texture2D chartTexture;

		private static Texture2D staticTexture;

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
		private static Confirm <>f__mg$cache5;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache6;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache7;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache8;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache9;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cacheA;

		[CompilerGenerated]
		private static SwappedState <>f__mg$cacheB;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cacheC;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cacheD;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cacheE;

		[CompilerGenerated]
		private static ClickedMouseStarted <>f__mg$cacheF;

		[CompilerGenerated]
		private static ClickedMouseStopped <>f__mg$cache10;

		[CompilerGenerated]
		private static MovedMouse <>f__mg$cache11;

		[CompilerGenerated]
		private static IsBlindfoldedChangedHandler <>f__mg$cache12;

		[CompilerGenerated]
		private static PlayerTeleported <>f__mg$cache13;

		[CompilerGenerated]
		private static GroupUpdatedHandler <>f__mg$cache14;

		[CompilerGenerated]
		private static GroupInfoReadyHandler <>f__mg$cache15;

		private class SleekInviteButton : Sleek
		{
			public SleekInviteButton(CSteamID newGroupID)
			{
				this.groupID = newGroupID;
				GroupInfo groupInfo = GroupManager.getGroupInfo(this.groupID);
				string text = (groupInfo == null) ? this.groupID.ToString() : groupInfo.name;
				SleekBox sleekBox = new SleekBox();
				sleekBox.sizeOffset_X = -140;
				sleekBox.sizeScale_X = 1f;
				sleekBox.sizeScale_Y = 1f;
				sleekBox.text = text;
				base.add(sleekBox);
				sleekBox.add(new SleekButton
				{
					positionScale_X = 1f,
					sizeOffset_X = 60,
					sizeScale_Y = 1f,
					text = PlayerDashboardInformationUI.localization.format("Group_Join"),
					tooltip = PlayerDashboardInformationUI.localization.format("Group_Join_Tooltip"),
					onClickedButton = new ClickedButton(this.handleJoinButtonClicked)
				});
				sleekBox.add(new SleekButton
				{
					positionOffset_X = 60,
					positionScale_X = 1f,
					sizeOffset_X = 80,
					sizeScale_Y = 1f,
					text = PlayerDashboardInformationUI.localization.format("Group_Ignore"),
					tooltip = PlayerDashboardInformationUI.localization.format("Group_Ignore_Tooltip"),
					onClickedButton = new ClickedButton(this.handleIgnoreButtonClicked)
				});
			}

			private CSteamID groupID { get; }

			private void handleJoinButtonClicked(SleekButton button)
			{
				Player.player.quests.sendJoinGroupInvite(this.groupID);
			}

			private void handleIgnoreButtonClicked(SleekButton button)
			{
				Player.player.quests.sendIgnoreGroupInvite(this.groupID);
			}
		}

		private enum EInfoTab
		{
			QUESTS,
			GROUPS,
			PLAYERS
		}
	}
}
