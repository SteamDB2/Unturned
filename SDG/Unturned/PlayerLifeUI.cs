using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace SDG.Unturned
{
	public class PlayerLifeUI
	{
		public PlayerLifeUI()
		{
			if (PlayerLifeUI.icons != null)
			{
				PlayerLifeUI.icons.unload();
			}
			PlayerLifeUI.localization = Localization.read("/Player/PlayerLife.dat");
			PlayerLifeUI.icons = Bundles.getBundle("/Bundles/Textures/Player/Icons/PlayerLife/PlayerLife.unity3d");
			PlayerLifeUI._container = new Sleek();
			PlayerLifeUI.container.positionOffset_X = 10;
			PlayerLifeUI.container.positionOffset_Y = 10;
			PlayerLifeUI.container.sizeOffset_X = -20;
			PlayerLifeUI.container.sizeOffset_Y = -20;
			PlayerLifeUI.container.sizeScale_X = 1f;
			PlayerLifeUI.container.sizeScale_Y = 1f;
			PlayerUI.container.add(PlayerLifeUI.container);
			PlayerLifeUI.active = true;
			PlayerLifeUI.chatting = false;
			PlayerLifeUI.chatBox = new SleekScrollBox();
			PlayerLifeUI.chatBox.sizeOffset_X = 504;
			PlayerLifeUI.chatBox.sizeOffset_Y = 160;
			PlayerLifeUI.chatBox.area = new Rect(0f, 0f, 5f, (float)(ChatManager.chat.Length * 40));
			PlayerLifeUI.chatBox.state = new Vector3(0f, float.MaxValue);
			PlayerLifeUI.container.add(PlayerLifeUI.chatBox);
			PlayerLifeUI.chatBox.isVisible = false;
			PlayerLifeUI.chatLabel = new SleekChat[ChatManager.chat.Length];
			for (int i = 0; i < PlayerLifeUI.chatLabel.Length; i++)
			{
				SleekChat sleekChat = new SleekChat();
				sleekChat.positionOffset_Y = PlayerLifeUI.chatLabel.Length * 40 - 40 - i * 40;
				sleekChat.sizeOffset_X = 474;
				sleekChat.sizeOffset_Y = 40;
				PlayerLifeUI.chatBox.add(sleekChat);
				PlayerLifeUI.chatLabel[i] = sleekChat;
			}
			PlayerLifeUI.gameLabel = new SleekChat[4];
			for (int j = 0; j < PlayerLifeUI.gameLabel.Length; j++)
			{
				SleekChat sleekChat2 = new SleekChat();
				sleekChat2.positionOffset_Y = 120 - j * 40;
				sleekChat2.sizeOffset_X = 474;
				sleekChat2.sizeOffset_Y = 40;
				PlayerLifeUI.container.add(sleekChat2);
				PlayerLifeUI.gameLabel[j] = sleekChat2;
			}
			PlayerLifeUI.chatField = new SleekField();
			PlayerLifeUI.chatField.positionOffset_X = -424;
			PlayerLifeUI.chatField.positionOffset_Y = 170;
			PlayerLifeUI.chatField.sizeOffset_X = 404;
			PlayerLifeUI.chatField.sizeOffset_Y = 30;
			PlayerLifeUI.chatField.fontAlignment = 3;
			PlayerLifeUI.chatField.maxLength = ChatManager.LENGTH;
			PlayerLifeUI.chatField.foregroundTint = ESleekTint.NONE;
			PlayerLifeUI.container.add(PlayerLifeUI.chatField);
			PlayerLifeUI.modeBox = new SleekBox();
			PlayerLifeUI.modeBox.positionOffset_X = -100;
			PlayerLifeUI.modeBox.sizeOffset_X = 90;
			PlayerLifeUI.modeBox.sizeOffset_Y = 30;
			PlayerLifeUI.modeBox.fontAlignment = 4;
			PlayerLifeUI.modeBox.foregroundTint = ESleekTint.NONE;
			PlayerLifeUI.chatField.add(PlayerLifeUI.modeBox);
			PlayerLifeUI.voteBox = new SleekBox();
			PlayerLifeUI.voteBox.positionOffset_X = -430;
			PlayerLifeUI.voteBox.positionScale_X = 1f;
			PlayerLifeUI.voteBox.sizeOffset_X = 430;
			PlayerLifeUI.voteBox.sizeOffset_Y = 90;
			PlayerLifeUI.container.add(PlayerLifeUI.voteBox);
			PlayerLifeUI.voteBox.isVisible = false;
			PlayerLifeUI.voteInfoLabel = new SleekLabel();
			PlayerLifeUI.voteInfoLabel.sizeOffset_Y = 30;
			PlayerLifeUI.voteInfoLabel.sizeScale_X = 1f;
			PlayerLifeUI.voteBox.add(PlayerLifeUI.voteInfoLabel);
			PlayerLifeUI.votesNeededLabel = new SleekLabel();
			PlayerLifeUI.votesNeededLabel.positionOffset_Y = 30;
			PlayerLifeUI.votesNeededLabel.sizeOffset_Y = 30;
			PlayerLifeUI.votesNeededLabel.sizeScale_X = 1f;
			PlayerLifeUI.voteBox.add(PlayerLifeUI.votesNeededLabel);
			PlayerLifeUI.voteYesLabel = new SleekLabel();
			PlayerLifeUI.voteYesLabel.positionOffset_Y = 60;
			PlayerLifeUI.voteYesLabel.sizeOffset_Y = 30;
			PlayerLifeUI.voteYesLabel.sizeScale_X = 0.5f;
			PlayerLifeUI.voteBox.add(PlayerLifeUI.voteYesLabel);
			PlayerLifeUI.voteNoLabel = new SleekLabel();
			PlayerLifeUI.voteNoLabel.positionOffset_Y = 60;
			PlayerLifeUI.voteNoLabel.positionScale_X = 0.5f;
			PlayerLifeUI.voteNoLabel.sizeOffset_Y = 30;
			PlayerLifeUI.voteNoLabel.sizeScale_X = 0.5f;
			PlayerLifeUI.voteBox.add(PlayerLifeUI.voteNoLabel);
			PlayerLifeUI.voiceBox = new SleekBoxIcon((Texture2D)PlayerLifeUI.icons.load("Voice"));
			PlayerLifeUI.voiceBox.positionOffset_Y = 210;
			PlayerLifeUI.voiceBox.sizeOffset_X = 50;
			PlayerLifeUI.voiceBox.sizeOffset_Y = 50;
			PlayerLifeUI.voiceBox.iconImage.backgroundTint = ESleekTint.FOREGROUND;
			PlayerLifeUI.container.add(PlayerLifeUI.voiceBox);
			PlayerLifeUI.voiceBox.isVisible = false;
			PlayerLifeUI.trackedQuestTitle = new SleekLabel();
			PlayerLifeUI.trackedQuestTitle.positionOffset_X = -500;
			PlayerLifeUI.trackedQuestTitle.positionOffset_Y = 215;
			PlayerLifeUI.trackedQuestTitle.positionScale_X = 1f;
			PlayerLifeUI.trackedQuestTitle.sizeOffset_X = 500;
			PlayerLifeUI.trackedQuestTitle.sizeOffset_Y = 25;
			PlayerLifeUI.trackedQuestTitle.isRich = true;
			PlayerLifeUI.trackedQuestTitle.fontSize = 14;
			PlayerLifeUI.trackedQuestTitle.fontAlignment = 5;
			PlayerLifeUI.container.add(PlayerLifeUI.trackedQuestTitle);
			PlayerLifeUI.trackedQuestBar = new SleekImageTexture();
			PlayerLifeUI.trackedQuestBar.positionOffset_X = -200;
			PlayerLifeUI.trackedQuestBar.positionOffset_Y = 240;
			PlayerLifeUI.trackedQuestBar.positionScale_X = 1f;
			PlayerLifeUI.trackedQuestBar.sizeOffset_X = 200;
			PlayerLifeUI.trackedQuestBar.sizeOffset_Y = 3;
			PlayerLifeUI.trackedQuestBar.texture = (Texture2D)Resources.Load("Materials/Pixel");
			PlayerLifeUI.trackedQuestBar.backgroundTint = ESleekTint.FOREGROUND;
			PlayerLifeUI.container.add(PlayerLifeUI.trackedQuestBar);
			PlayerLifeUI.levelTextBox = new SleekBox();
			PlayerLifeUI.levelTextBox.positionOffset_X = -180;
			PlayerLifeUI.levelTextBox.positionScale_X = 0.5f;
			PlayerLifeUI.levelTextBox.sizeOffset_X = 300;
			PlayerLifeUI.levelTextBox.sizeOffset_Y = 50;
			PlayerLifeUI.levelTextBox.fontSize = 14;
			PlayerLifeUI.container.add(PlayerLifeUI.levelTextBox);
			PlayerLifeUI.levelTextBox.isVisible = false;
			PlayerLifeUI.levelNumberBox = new SleekBox();
			PlayerLifeUI.levelNumberBox.positionOffset_X = 130;
			PlayerLifeUI.levelNumberBox.positionScale_X = 0.5f;
			PlayerLifeUI.levelNumberBox.sizeOffset_X = 50;
			PlayerLifeUI.levelNumberBox.sizeOffset_Y = 50;
			PlayerLifeUI.levelNumberBox.fontSize = 14;
			PlayerLifeUI.container.add(PlayerLifeUI.levelNumberBox);
			PlayerLifeUI.levelNumberBox.isVisible = false;
			PlayerLifeUI.painImage = new SleekImageTexture();
			PlayerLifeUI.painImage.sizeScale_X = 1f;
			PlayerLifeUI.painImage.sizeScale_Y = 1f;
			Color color_R = Palette.COLOR_R;
			color_R.a = 0f;
			PlayerLifeUI.painImage.backgroundColor = color_R;
			PlayerLifeUI.painImage.texture = (Texture2D)Resources.Load("Materials/Pixel");
			PlayerUI.window.add(PlayerLifeUI.painImage);
			PlayerLifeUI.scopeOverlay = new SleekImageTexture((Texture2D)Resources.Load("Overlay/Scope"));
			PlayerLifeUI.scopeOverlay.positionScale_X = 0.1f;
			PlayerLifeUI.scopeOverlay.positionScale_Y = 0.1f;
			PlayerLifeUI.scopeOverlay.sizeScale_X = 0.8f;
			PlayerLifeUI.scopeOverlay.sizeScale_Y = 0.8f;
			PlayerLifeUI.scopeOverlay.constraint = ESleekConstraint.X;
			PlayerUI.window.add(PlayerLifeUI.scopeOverlay);
			PlayerLifeUI.scopeOverlay.isVisible = false;
			PlayerLifeUI.scopeLeftOverlay = new SleekImageTexture((Texture2D)Resources.Load("Materials/Pixel"));
			PlayerLifeUI.scopeLeftOverlay.positionScale_X = -10f;
			PlayerLifeUI.scopeLeftOverlay.sizeScale_X = 10f;
			PlayerLifeUI.scopeLeftOverlay.sizeScale_Y = 1f;
			PlayerLifeUI.scopeLeftOverlay.backgroundColor = Color.black;
			PlayerLifeUI.scopeOverlay.add(PlayerLifeUI.scopeLeftOverlay);
			PlayerLifeUI.scopeRightOverlay = new SleekImageTexture((Texture2D)Resources.Load("Materials/Pixel"));
			PlayerLifeUI.scopeRightOverlay.positionScale_X = 1f;
			PlayerLifeUI.scopeRightOverlay.sizeScale_X = 10f;
			PlayerLifeUI.scopeRightOverlay.sizeScale_Y = 1f;
			PlayerLifeUI.scopeRightOverlay.backgroundColor = Color.black;
			PlayerLifeUI.scopeOverlay.add(PlayerLifeUI.scopeRightOverlay);
			PlayerLifeUI.scopeUpOverlay = new SleekImageTexture((Texture2D)Resources.Load("Materials/Pixel"));
			PlayerLifeUI.scopeUpOverlay.positionScale_X = -10f;
			PlayerLifeUI.scopeUpOverlay.positionScale_Y = -10f;
			PlayerLifeUI.scopeUpOverlay.sizeScale_X = 21f;
			PlayerLifeUI.scopeUpOverlay.sizeScale_Y = 10f;
			PlayerLifeUI.scopeUpOverlay.backgroundColor = Color.black;
			PlayerLifeUI.scopeOverlay.add(PlayerLifeUI.scopeUpOverlay);
			PlayerLifeUI.scopeDownOverlay = new SleekImageTexture((Texture2D)Resources.Load("Materials/Pixel"));
			PlayerLifeUI.scopeDownOverlay.positionScale_X = -10f;
			PlayerLifeUI.scopeDownOverlay.positionScale_Y = 1f;
			PlayerLifeUI.scopeDownOverlay.sizeScale_X = 21f;
			PlayerLifeUI.scopeDownOverlay.sizeScale_Y = 10f;
			PlayerLifeUI.scopeDownOverlay.backgroundColor = Color.black;
			PlayerLifeUI.scopeOverlay.add(PlayerLifeUI.scopeDownOverlay);
			PlayerLifeUI.scopeImage = new SleekImageTexture();
			PlayerLifeUI.scopeImage.sizeScale_X = 1f;
			PlayerLifeUI.scopeImage.sizeScale_Y = 1f;
			PlayerLifeUI.scopeOverlay.add(PlayerLifeUI.scopeImage);
			PlayerLifeUI.binocularsOverlay = new SleekImageTexture((Texture2D)Resources.Load("Overlay/Binoculars"));
			PlayerLifeUI.binocularsOverlay.sizeScale_X = 1f;
			PlayerLifeUI.binocularsOverlay.sizeScale_Y = 1f;
			PlayerUI.window.add(PlayerLifeUI.binocularsOverlay);
			PlayerLifeUI.binocularsOverlay.isVisible = false;
			PlayerLifeUI.faceButtons = new SleekButton[(int)(Customization.FACES_FREE + Customization.FACES_PRO)];
			for (int k = 0; k < PlayerLifeUI.faceButtons.Length; k++)
			{
				float num = 12.566371f * ((float)k / (float)PlayerLifeUI.faceButtons.Length);
				float num2 = 185f;
				if (k >= PlayerLifeUI.faceButtons.Length / 2)
				{
					num += 3.14159274f / (float)(PlayerLifeUI.faceButtons.Length / 2);
					num2 += 30f;
				}
				SleekButton sleekButton = new SleekButton();
				sleekButton.positionOffset_X = (int)(Mathf.Cos(num) * num2) - 20;
				sleekButton.positionOffset_Y = (int)(Mathf.Sin(num) * num2) - 20;
				sleekButton.positionScale_X = 0.5f;
				sleekButton.positionScale_Y = 0.5f;
				sleekButton.sizeOffset_X = 40;
				sleekButton.sizeOffset_Y = 40;
				PlayerLifeUI.container.add(sleekButton);
				sleekButton.isVisible = false;
				SleekImageTexture sleekImageTexture = new SleekImageTexture();
				sleekImageTexture.positionOffset_X = 10;
				sleekImageTexture.positionOffset_Y = 10;
				sleekImageTexture.sizeOffset_X = 20;
				sleekImageTexture.sizeOffset_Y = 20;
				sleekImageTexture.texture = (Texture2D)Resources.Load("Materials/Pixel");
				sleekImageTexture.backgroundColor = Characters.active.skin;
				sleekButton.add(sleekImageTexture);
				sleekImageTexture.add(new SleekImageTexture
				{
					positionOffset_X = 2,
					positionOffset_Y = 2,
					sizeOffset_X = 16,
					sizeOffset_Y = 16,
					texture = (Texture2D)Resources.Load("Faces/" + k + "/Texture")
				});
				if (k >= (int)Customization.FACES_FREE)
				{
					if (Provider.isPro)
					{
						SleekButton sleekButton2 = sleekButton;
						if (PlayerLifeUI.<>f__mg$cache0 == null)
						{
							PlayerLifeUI.<>f__mg$cache0 = new ClickedButton(PlayerLifeUI.onClickedFaceButton);
						}
						sleekButton2.onClickedButton = PlayerLifeUI.<>f__mg$cache0;
					}
					else
					{
						sleekButton.backgroundColor = Palette.PRO;
						Bundle bundle = Bundles.getBundle("/Bundles/Textures/Menu/Icons/Pro/Pro.unity3d");
						sleekButton.add(new SleekImageTexture
						{
							positionOffset_X = -10,
							positionOffset_Y = -10,
							positionScale_X = 0.5f,
							positionScale_Y = 0.5f,
							sizeOffset_X = 20,
							sizeOffset_Y = 20,
							texture = (Texture2D)bundle.load("Lock_Small")
						});
						bundle.unload();
					}
				}
				else
				{
					SleekButton sleekButton3 = sleekButton;
					if (PlayerLifeUI.<>f__mg$cache1 == null)
					{
						PlayerLifeUI.<>f__mg$cache1 = new ClickedButton(PlayerLifeUI.onClickedFaceButton);
					}
					sleekButton3.onClickedButton = PlayerLifeUI.<>f__mg$cache1;
				}
				PlayerLifeUI.faceButtons[k] = sleekButton;
			}
			PlayerLifeUI.surrenderButton = new SleekButton();
			PlayerLifeUI.surrenderButton.positionOffset_X = -110;
			PlayerLifeUI.surrenderButton.positionOffset_Y = -15;
			PlayerLifeUI.surrenderButton.positionScale_X = 0.5f;
			PlayerLifeUI.surrenderButton.positionScale_Y = 0.5f;
			PlayerLifeUI.surrenderButton.sizeOffset_X = 100;
			PlayerLifeUI.surrenderButton.sizeOffset_Y = 30;
			PlayerLifeUI.surrenderButton.text = PlayerLifeUI.localization.format("Surrender");
			SleekButton sleekButton4 = PlayerLifeUI.surrenderButton;
			if (PlayerLifeUI.<>f__mg$cache2 == null)
			{
				PlayerLifeUI.<>f__mg$cache2 = new ClickedButton(PlayerLifeUI.onClickedSurrenderButton);
			}
			sleekButton4.onClickedButton = PlayerLifeUI.<>f__mg$cache2;
			PlayerLifeUI.container.add(PlayerLifeUI.surrenderButton);
			PlayerLifeUI.surrenderButton.isVisible = false;
			PlayerLifeUI.pointButton = new SleekButton();
			PlayerLifeUI.pointButton.positionOffset_X = 10;
			PlayerLifeUI.pointButton.positionOffset_Y = -15;
			PlayerLifeUI.pointButton.positionScale_X = 0.5f;
			PlayerLifeUI.pointButton.positionScale_Y = 0.5f;
			PlayerLifeUI.pointButton.sizeOffset_X = 100;
			PlayerLifeUI.pointButton.sizeOffset_Y = 30;
			PlayerLifeUI.pointButton.text = PlayerLifeUI.localization.format("Point");
			SleekButton sleekButton5 = PlayerLifeUI.pointButton;
			if (PlayerLifeUI.<>f__mg$cache3 == null)
			{
				PlayerLifeUI.<>f__mg$cache3 = new ClickedButton(PlayerLifeUI.onClickedPointButton);
			}
			sleekButton5.onClickedButton = PlayerLifeUI.<>f__mg$cache3;
			PlayerLifeUI.container.add(PlayerLifeUI.pointButton);
			PlayerLifeUI.pointButton.isVisible = false;
			PlayerLifeUI.waveButton = new SleekButton();
			PlayerLifeUI.waveButton.positionOffset_X = -50;
			PlayerLifeUI.waveButton.positionOffset_Y = -55;
			PlayerLifeUI.waveButton.positionScale_X = 0.5f;
			PlayerLifeUI.waveButton.positionScale_Y = 0.5f;
			PlayerLifeUI.waveButton.sizeOffset_X = 100;
			PlayerLifeUI.waveButton.sizeOffset_Y = 30;
			PlayerLifeUI.waveButton.text = PlayerLifeUI.localization.format("Wave");
			SleekButton sleekButton6 = PlayerLifeUI.waveButton;
			if (PlayerLifeUI.<>f__mg$cache4 == null)
			{
				PlayerLifeUI.<>f__mg$cache4 = new ClickedButton(PlayerLifeUI.onClickedWaveButton);
			}
			sleekButton6.onClickedButton = PlayerLifeUI.<>f__mg$cache4;
			PlayerLifeUI.container.add(PlayerLifeUI.waveButton);
			PlayerLifeUI.waveButton.isVisible = false;
			PlayerLifeUI.saluteButton = new SleekButton();
			PlayerLifeUI.saluteButton.positionOffset_X = -50;
			PlayerLifeUI.saluteButton.positionOffset_Y = 25;
			PlayerLifeUI.saluteButton.positionScale_X = 0.5f;
			PlayerLifeUI.saluteButton.positionScale_Y = 0.5f;
			PlayerLifeUI.saluteButton.sizeOffset_X = 100;
			PlayerLifeUI.saluteButton.sizeOffset_Y = 30;
			PlayerLifeUI.saluteButton.text = PlayerLifeUI.localization.format("Salute");
			SleekButton sleekButton7 = PlayerLifeUI.saluteButton;
			if (PlayerLifeUI.<>f__mg$cache5 == null)
			{
				PlayerLifeUI.<>f__mg$cache5 = new ClickedButton(PlayerLifeUI.onClickedSaluteButton);
			}
			sleekButton7.onClickedButton = PlayerLifeUI.<>f__mg$cache5;
			PlayerLifeUI.container.add(PlayerLifeUI.saluteButton);
			PlayerLifeUI.saluteButton.isVisible = false;
			PlayerLifeUI.restButton = new SleekButton();
			PlayerLifeUI.restButton.positionOffset_X = -110;
			PlayerLifeUI.restButton.positionOffset_Y = 65;
			PlayerLifeUI.restButton.positionScale_X = 0.5f;
			PlayerLifeUI.restButton.positionScale_Y = 0.5f;
			PlayerLifeUI.restButton.sizeOffset_X = 100;
			PlayerLifeUI.restButton.sizeOffset_Y = 30;
			PlayerLifeUI.restButton.text = PlayerLifeUI.localization.format("Rest");
			SleekButton sleekButton8 = PlayerLifeUI.restButton;
			if (PlayerLifeUI.<>f__mg$cache6 == null)
			{
				PlayerLifeUI.<>f__mg$cache6 = new ClickedButton(PlayerLifeUI.onClickedRestButton);
			}
			sleekButton8.onClickedButton = PlayerLifeUI.<>f__mg$cache6;
			PlayerLifeUI.container.add(PlayerLifeUI.restButton);
			PlayerLifeUI.restButton.isVisible = false;
			PlayerLifeUI.facepalmButton = new SleekButton();
			PlayerLifeUI.facepalmButton.positionOffset_X = 10;
			PlayerLifeUI.facepalmButton.positionOffset_Y = -95;
			PlayerLifeUI.facepalmButton.positionScale_X = 0.5f;
			PlayerLifeUI.facepalmButton.positionScale_Y = 0.5f;
			PlayerLifeUI.facepalmButton.sizeOffset_X = 100;
			PlayerLifeUI.facepalmButton.sizeOffset_Y = 30;
			PlayerLifeUI.facepalmButton.text = PlayerLifeUI.localization.format("Facepalm");
			SleekButton sleekButton9 = PlayerLifeUI.facepalmButton;
			if (PlayerLifeUI.<>f__mg$cache7 == null)
			{
				PlayerLifeUI.<>f__mg$cache7 = new ClickedButton(PlayerLifeUI.onClickedFacepalmButton);
			}
			sleekButton9.onClickedButton = PlayerLifeUI.<>f__mg$cache7;
			PlayerLifeUI.container.add(PlayerLifeUI.facepalmButton);
			PlayerLifeUI.facepalmButton.isVisible = false;
			PlayerLifeUI.hitmarkers = new HitmarkerInfo[16];
			for (int l = 0; l < PlayerLifeUI.hitmarkers.Length; l++)
			{
				SleekImageTexture sleekImageTexture2 = new SleekImageTexture();
				sleekImageTexture2.positionOffset_X = -16;
				sleekImageTexture2.positionOffset_Y = -16;
				sleekImageTexture2.sizeOffset_X = 32;
				sleekImageTexture2.sizeOffset_Y = 32;
				sleekImageTexture2.texture = (Texture)PlayerLifeUI.icons.load("Hit_Entity");
				sleekImageTexture2.backgroundColor = OptionsSettings.hitmarkerColor;
				PlayerUI.window.add(sleekImageTexture2);
				sleekImageTexture2.isVisible = false;
				SleekImageTexture sleekImageTexture3 = new SleekImageTexture();
				sleekImageTexture3.positionOffset_X = -16;
				sleekImageTexture3.positionOffset_Y = -16;
				sleekImageTexture3.sizeOffset_X = 32;
				sleekImageTexture3.sizeOffset_Y = 32;
				sleekImageTexture3.texture = (Texture)PlayerLifeUI.icons.load("Hit_Critical");
				sleekImageTexture3.backgroundColor = OptionsSettings.criticalHitmarkerColor;
				PlayerUI.window.add(sleekImageTexture3);
				sleekImageTexture3.isVisible = false;
				SleekImageTexture sleekImageTexture4 = new SleekImageTexture();
				sleekImageTexture4.positionOffset_X = -16;
				sleekImageTexture4.positionOffset_Y = -16;
				sleekImageTexture4.sizeOffset_X = 32;
				sleekImageTexture4.sizeOffset_Y = 32;
				sleekImageTexture4.texture = (Texture)PlayerLifeUI.icons.load("Hit_Build");
				sleekImageTexture4.backgroundColor = OptionsSettings.crosshairColor;
				PlayerUI.window.add(sleekImageTexture4);
				sleekImageTexture4.isVisible = false;
				HitmarkerInfo hitmarkerInfo = new HitmarkerInfo();
				hitmarkerInfo.hit = EPlayerHit.NONE;
				hitmarkerInfo.hitEntitiyImage = sleekImageTexture2;
				hitmarkerInfo.hitCriticalImage = sleekImageTexture3;
				hitmarkerInfo.hitBuildImage = sleekImageTexture4;
				PlayerLifeUI.hitmarkers[l] = hitmarkerInfo;
			}
			PlayerLifeUI.dotImage = new SleekImageTexture();
			PlayerLifeUI.dotImage.positionOffset_X = -4;
			PlayerLifeUI.dotImage.positionOffset_Y = -4;
			PlayerLifeUI.dotImage.positionScale_X = 0.5f;
			PlayerLifeUI.dotImage.positionScale_Y = 0.5f;
			PlayerLifeUI.dotImage.sizeOffset_X = 8;
			PlayerLifeUI.dotImage.sizeOffset_Y = 8;
			PlayerLifeUI.dotImage.texture = (Texture)PlayerLifeUI.icons.load("Dot");
			PlayerLifeUI.dotImage.backgroundColor = OptionsSettings.crosshairColor;
			PlayerLifeUI.container.add(PlayerLifeUI.dotImage);
			PlayerLifeUI.crosshairLeftImage = new SleekImageTexture();
			PlayerLifeUI.crosshairLeftImage.positionOffset_X = -4;
			PlayerLifeUI.crosshairLeftImage.positionOffset_Y = -4;
			PlayerLifeUI.crosshairLeftImage.positionScale_X = 0.5f;
			PlayerLifeUI.crosshairLeftImage.positionScale_Y = 0.5f;
			PlayerLifeUI.crosshairLeftImage.sizeOffset_X = 8;
			PlayerLifeUI.crosshairLeftImage.sizeOffset_Y = 8;
			PlayerLifeUI.crosshairLeftImage.texture = (Texture)PlayerLifeUI.icons.load("Crosshair_Right");
			PlayerLifeUI.crosshairLeftImage.backgroundColor = OptionsSettings.crosshairColor;
			PlayerLifeUI.container.add(PlayerLifeUI.crosshairLeftImage);
			PlayerLifeUI.crosshairLeftImage.isVisible = false;
			PlayerLifeUI.crosshairRightImage = new SleekImageTexture();
			PlayerLifeUI.crosshairRightImage.positionOffset_X = -4;
			PlayerLifeUI.crosshairRightImage.positionOffset_Y = -4;
			PlayerLifeUI.crosshairRightImage.positionScale_X = 0.5f;
			PlayerLifeUI.crosshairRightImage.positionScale_Y = 0.5f;
			PlayerLifeUI.crosshairRightImage.sizeOffset_X = 8;
			PlayerLifeUI.crosshairRightImage.sizeOffset_Y = 8;
			PlayerLifeUI.crosshairRightImage.texture = (Texture)PlayerLifeUI.icons.load("Crosshair_Left");
			PlayerLifeUI.crosshairRightImage.backgroundColor = OptionsSettings.crosshairColor;
			PlayerLifeUI.container.add(PlayerLifeUI.crosshairRightImage);
			PlayerLifeUI.crosshairRightImage.isVisible = false;
			PlayerLifeUI.crosshairDownImage = new SleekImageTexture();
			PlayerLifeUI.crosshairDownImage.positionOffset_X = -4;
			PlayerLifeUI.crosshairDownImage.positionOffset_Y = -4;
			PlayerLifeUI.crosshairDownImage.positionScale_X = 0.5f;
			PlayerLifeUI.crosshairDownImage.positionScale_Y = 0.5f;
			PlayerLifeUI.crosshairDownImage.sizeOffset_X = 8;
			PlayerLifeUI.crosshairDownImage.sizeOffset_Y = 8;
			PlayerLifeUI.crosshairDownImage.texture = (Texture)PlayerLifeUI.icons.load("Crosshair_Up");
			PlayerLifeUI.crosshairDownImage.backgroundColor = OptionsSettings.crosshairColor;
			PlayerLifeUI.container.add(PlayerLifeUI.crosshairDownImage);
			PlayerLifeUI.crosshairDownImage.isVisible = false;
			PlayerLifeUI.crosshairUpImage = new SleekImageTexture();
			PlayerLifeUI.crosshairUpImage.positionOffset_X = -4;
			PlayerLifeUI.crosshairUpImage.positionOffset_Y = -4;
			PlayerLifeUI.crosshairUpImage.positionScale_X = 0.5f;
			PlayerLifeUI.crosshairUpImage.positionScale_Y = 0.5f;
			PlayerLifeUI.crosshairUpImage.sizeOffset_X = 8;
			PlayerLifeUI.crosshairUpImage.sizeOffset_Y = 8;
			PlayerLifeUI.crosshairUpImage.texture = (Texture)PlayerLifeUI.icons.load("Crosshair_Down");
			PlayerLifeUI.crosshairUpImage.backgroundColor = OptionsSettings.crosshairColor;
			PlayerLifeUI.container.add(PlayerLifeUI.crosshairUpImage);
			PlayerLifeUI.crosshairUpImage.isVisible = false;
			PlayerLifeUI.lifeBox = new SleekBox();
			PlayerLifeUI.lifeBox.positionOffset_Y = -180;
			PlayerLifeUI.lifeBox.positionScale_Y = 1f;
			PlayerLifeUI.lifeBox.sizeOffset_Y = 180;
			PlayerLifeUI.lifeBox.sizeScale_X = 0.2f;
			PlayerLifeUI.container.add(PlayerLifeUI.lifeBox);
			PlayerLifeUI.healthIcon = new SleekImageTexture();
			PlayerLifeUI.healthIcon.positionOffset_X = 5;
			PlayerLifeUI.healthIcon.positionOffset_Y = 5;
			PlayerLifeUI.healthIcon.sizeOffset_X = 20;
			PlayerLifeUI.healthIcon.sizeOffset_Y = 20;
			PlayerLifeUI.healthIcon.texture = (Texture2D)PlayerLifeUI.icons.load("Health");
			PlayerLifeUI.lifeBox.add(PlayerLifeUI.healthIcon);
			PlayerLifeUI.healthProgress = new SleekProgress(string.Empty);
			PlayerLifeUI.healthProgress.positionOffset_X = 30;
			PlayerLifeUI.healthProgress.positionOffset_Y = 10;
			PlayerLifeUI.healthProgress.sizeOffset_X = -40;
			PlayerLifeUI.healthProgress.sizeOffset_Y = 10;
			PlayerLifeUI.healthProgress.sizeScale_X = 1f;
			PlayerLifeUI.healthProgress.color = Palette.COLOR_R;
			PlayerLifeUI.lifeBox.add(PlayerLifeUI.healthProgress);
			PlayerLifeUI.foodIcon = new SleekImageTexture();
			PlayerLifeUI.foodIcon.positionOffset_X = 5;
			PlayerLifeUI.foodIcon.positionOffset_Y = 35;
			PlayerLifeUI.foodIcon.sizeOffset_X = 20;
			PlayerLifeUI.foodIcon.sizeOffset_Y = 20;
			PlayerLifeUI.foodIcon.texture = (Texture2D)PlayerLifeUI.icons.load("Food");
			PlayerLifeUI.lifeBox.add(PlayerLifeUI.foodIcon);
			PlayerLifeUI.foodProgress = new SleekProgress(string.Empty);
			PlayerLifeUI.foodProgress.positionOffset_X = 30;
			PlayerLifeUI.foodProgress.positionOffset_Y = 40;
			PlayerLifeUI.foodProgress.sizeOffset_X = -40;
			PlayerLifeUI.foodProgress.sizeOffset_Y = 10;
			PlayerLifeUI.foodProgress.sizeScale_X = 1f;
			PlayerLifeUI.foodProgress.color = Palette.COLOR_O;
			PlayerLifeUI.lifeBox.add(PlayerLifeUI.foodProgress);
			PlayerLifeUI.waterIcon = new SleekImageTexture();
			PlayerLifeUI.waterIcon.positionOffset_X = 5;
			PlayerLifeUI.waterIcon.positionOffset_Y = 65;
			PlayerLifeUI.waterIcon.sizeOffset_X = 20;
			PlayerLifeUI.waterIcon.sizeOffset_Y = 20;
			PlayerLifeUI.waterIcon.texture = (Texture2D)PlayerLifeUI.icons.load("Water");
			PlayerLifeUI.lifeBox.add(PlayerLifeUI.waterIcon);
			PlayerLifeUI.waterProgress = new SleekProgress(string.Empty);
			PlayerLifeUI.waterProgress.positionOffset_X = 30;
			PlayerLifeUI.waterProgress.positionOffset_Y = 70;
			PlayerLifeUI.waterProgress.sizeOffset_X = -40;
			PlayerLifeUI.waterProgress.sizeOffset_Y = 10;
			PlayerLifeUI.waterProgress.sizeScale_X = 1f;
			PlayerLifeUI.waterProgress.color = Palette.COLOR_B;
			PlayerLifeUI.lifeBox.add(PlayerLifeUI.waterProgress);
			PlayerLifeUI.virusIcon = new SleekImageTexture();
			PlayerLifeUI.virusIcon.positionOffset_X = 5;
			PlayerLifeUI.virusIcon.positionOffset_Y = 95;
			PlayerLifeUI.virusIcon.sizeOffset_X = 20;
			PlayerLifeUI.virusIcon.sizeOffset_Y = 20;
			PlayerLifeUI.virusIcon.texture = (Texture2D)PlayerLifeUI.icons.load("Virus");
			PlayerLifeUI.lifeBox.add(PlayerLifeUI.virusIcon);
			PlayerLifeUI.virusProgress = new SleekProgress(string.Empty);
			PlayerLifeUI.virusProgress.positionOffset_X = 30;
			PlayerLifeUI.virusProgress.positionOffset_Y = 100;
			PlayerLifeUI.virusProgress.sizeOffset_X = -40;
			PlayerLifeUI.virusProgress.sizeOffset_Y = 10;
			PlayerLifeUI.virusProgress.sizeScale_X = 1f;
			PlayerLifeUI.virusProgress.color = Palette.COLOR_G;
			PlayerLifeUI.lifeBox.add(PlayerLifeUI.virusProgress);
			PlayerLifeUI.staminaIcon = new SleekImageTexture();
			PlayerLifeUI.staminaIcon.positionOffset_X = 5;
			PlayerLifeUI.staminaIcon.positionOffset_Y = 125;
			PlayerLifeUI.staminaIcon.sizeOffset_X = 20;
			PlayerLifeUI.staminaIcon.sizeOffset_Y = 20;
			PlayerLifeUI.staminaIcon.texture = (Texture2D)PlayerLifeUI.icons.load("Stamina");
			PlayerLifeUI.lifeBox.add(PlayerLifeUI.staminaIcon);
			PlayerLifeUI.staminaProgress = new SleekProgress(string.Empty);
			PlayerLifeUI.staminaProgress.positionOffset_X = 30;
			PlayerLifeUI.staminaProgress.positionOffset_Y = 130;
			PlayerLifeUI.staminaProgress.sizeOffset_X = -40;
			PlayerLifeUI.staminaProgress.sizeOffset_Y = 10;
			PlayerLifeUI.staminaProgress.sizeScale_X = 1f;
			PlayerLifeUI.staminaProgress.color = Palette.COLOR_Y;
			PlayerLifeUI.lifeBox.add(PlayerLifeUI.staminaProgress);
			PlayerLifeUI.waveLabel = new SleekLabel();
			PlayerLifeUI.waveLabel.positionOffset_Y = 60;
			PlayerLifeUI.waveLabel.sizeOffset_Y = 30;
			PlayerLifeUI.waveLabel.sizeScale_X = 0.5f;
			PlayerLifeUI.lifeBox.add(PlayerLifeUI.waveLabel);
			PlayerLifeUI.waveLabel.isVisible = false;
			PlayerLifeUI.scoreLabel = new SleekLabel();
			PlayerLifeUI.scoreLabel.positionOffset_Y = 60;
			PlayerLifeUI.scoreLabel.positionScale_X = 0.5f;
			PlayerLifeUI.scoreLabel.sizeOffset_Y = 30;
			PlayerLifeUI.scoreLabel.sizeScale_X = 0.5f;
			PlayerLifeUI.lifeBox.add(PlayerLifeUI.scoreLabel);
			PlayerLifeUI.scoreLabel.isVisible = false;
			PlayerLifeUI.oxygenIcon = new SleekImageTexture();
			PlayerLifeUI.oxygenIcon.positionOffset_X = 5;
			PlayerLifeUI.oxygenIcon.positionOffset_Y = 155;
			PlayerLifeUI.oxygenIcon.sizeOffset_X = 20;
			PlayerLifeUI.oxygenIcon.sizeOffset_Y = 20;
			PlayerLifeUI.oxygenIcon.texture = (Texture2D)PlayerLifeUI.icons.load("Oxygen");
			PlayerLifeUI.lifeBox.add(PlayerLifeUI.oxygenIcon);
			PlayerLifeUI.oxygenProgress = new SleekProgress(string.Empty);
			PlayerLifeUI.oxygenProgress.positionOffset_X = 30;
			PlayerLifeUI.oxygenProgress.positionOffset_Y = 160;
			PlayerLifeUI.oxygenProgress.sizeOffset_X = -40;
			PlayerLifeUI.oxygenProgress.sizeOffset_Y = 10;
			PlayerLifeUI.oxygenProgress.sizeScale_X = 1f;
			PlayerLifeUI.oxygenProgress.color = Color.white;
			PlayerLifeUI.lifeBox.add(PlayerLifeUI.oxygenProgress);
			PlayerLifeUI.vehicleBox = new SleekBox();
			PlayerLifeUI.vehicleBox.positionOffset_Y = -120;
			PlayerLifeUI.vehicleBox.positionScale_X = 0.8f;
			PlayerLifeUI.vehicleBox.positionScale_Y = 1f;
			PlayerLifeUI.vehicleBox.sizeOffset_Y = 120;
			PlayerLifeUI.vehicleBox.sizeScale_X = 0.2f;
			PlayerLifeUI.container.add(PlayerLifeUI.vehicleBox);
			PlayerLifeUI.vehicleBox.isVisible = false;
			PlayerLifeUI.fuelIcon = new SleekImageTexture();
			PlayerLifeUI.fuelIcon.positionOffset_X = 5;
			PlayerLifeUI.fuelIcon.positionOffset_Y = 35;
			PlayerLifeUI.fuelIcon.sizeOffset_X = 20;
			PlayerLifeUI.fuelIcon.sizeOffset_Y = 20;
			PlayerLifeUI.fuelIcon.texture = (Texture2D)PlayerLifeUI.icons.load("Fuel");
			PlayerLifeUI.vehicleBox.add(PlayerLifeUI.fuelIcon);
			PlayerLifeUI.fuelProgress = new SleekProgress(string.Empty);
			PlayerLifeUI.fuelProgress.positionOffset_X = 30;
			PlayerLifeUI.fuelProgress.positionOffset_Y = 40;
			PlayerLifeUI.fuelProgress.sizeOffset_X = -40;
			PlayerLifeUI.fuelProgress.sizeOffset_Y = 10;
			PlayerLifeUI.fuelProgress.sizeScale_X = 1f;
			PlayerLifeUI.fuelProgress.color = Palette.COLOR_Y;
			PlayerLifeUI.vehicleBox.add(PlayerLifeUI.fuelProgress);
			PlayerLifeUI.speedIcon = new SleekImageTexture();
			PlayerLifeUI.speedIcon.positionOffset_X = 5;
			PlayerLifeUI.speedIcon.positionOffset_Y = 65;
			PlayerLifeUI.speedIcon.sizeOffset_X = 20;
			PlayerLifeUI.speedIcon.sizeOffset_Y = 20;
			PlayerLifeUI.speedIcon.texture = (Texture2D)PlayerLifeUI.icons.load("Speed");
			PlayerLifeUI.vehicleBox.add(PlayerLifeUI.speedIcon);
			PlayerLifeUI.speedProgress = new SleekProgress((!OptionsSettings.metric) ? " mph" : " kph");
			PlayerLifeUI.speedProgress.positionOffset_X = 30;
			PlayerLifeUI.speedProgress.positionOffset_Y = 70;
			PlayerLifeUI.speedProgress.sizeOffset_X = -40;
			PlayerLifeUI.speedProgress.sizeOffset_Y = 10;
			PlayerLifeUI.speedProgress.sizeScale_X = 1f;
			PlayerLifeUI.speedProgress.color = Palette.COLOR_P;
			PlayerLifeUI.vehicleBox.add(PlayerLifeUI.speedProgress);
			PlayerLifeUI.batteryChargeIcon = new SleekImageTexture();
			PlayerLifeUI.batteryChargeIcon.positionOffset_X = 5;
			PlayerLifeUI.batteryChargeIcon.positionOffset_Y = 5;
			PlayerLifeUI.batteryChargeIcon.sizeOffset_X = 20;
			PlayerLifeUI.batteryChargeIcon.sizeOffset_Y = 20;
			PlayerLifeUI.batteryChargeIcon.texture = (Texture2D)PlayerLifeUI.icons.load("Stamina");
			PlayerLifeUI.vehicleBox.add(PlayerLifeUI.batteryChargeIcon);
			PlayerLifeUI.batteryChargeProgress = new SleekProgress(string.Empty);
			PlayerLifeUI.batteryChargeProgress.positionOffset_X = 30;
			PlayerLifeUI.batteryChargeProgress.positionOffset_Y = 10;
			PlayerLifeUI.batteryChargeProgress.sizeOffset_X = -40;
			PlayerLifeUI.batteryChargeProgress.sizeOffset_Y = 10;
			PlayerLifeUI.batteryChargeProgress.sizeScale_X = 1f;
			PlayerLifeUI.batteryChargeProgress.color = Palette.COLOR_Y;
			PlayerLifeUI.vehicleBox.add(PlayerLifeUI.batteryChargeProgress);
			PlayerLifeUI.hpIcon = new SleekImageTexture();
			PlayerLifeUI.hpIcon.positionOffset_X = 5;
			PlayerLifeUI.hpIcon.positionOffset_Y = 95;
			PlayerLifeUI.hpIcon.sizeOffset_X = 20;
			PlayerLifeUI.hpIcon.sizeOffset_Y = 20;
			PlayerLifeUI.hpIcon.texture = (Texture2D)PlayerLifeUI.icons.load("Health");
			PlayerLifeUI.vehicleBox.add(PlayerLifeUI.hpIcon);
			PlayerLifeUI.hpProgress = new SleekProgress(string.Empty);
			PlayerLifeUI.hpProgress.positionOffset_X = 30;
			PlayerLifeUI.hpProgress.positionOffset_Y = 100;
			PlayerLifeUI.hpProgress.sizeOffset_X = -40;
			PlayerLifeUI.hpProgress.sizeOffset_Y = 10;
			PlayerLifeUI.hpProgress.sizeScale_X = 1f;
			PlayerLifeUI.hpProgress.color = Palette.COLOR_R;
			PlayerLifeUI.vehicleBox.add(PlayerLifeUI.hpProgress);
			PlayerLifeUI.gasmaskBox = new SleekBox();
			PlayerLifeUI.gasmaskBox.positionOffset_X = -200;
			PlayerLifeUI.gasmaskBox.positionOffset_Y = -60;
			PlayerLifeUI.gasmaskBox.positionScale_X = 0.5f;
			PlayerLifeUI.gasmaskBox.positionScale_Y = 1f;
			PlayerLifeUI.gasmaskBox.sizeOffset_X = 400;
			PlayerLifeUI.gasmaskBox.sizeOffset_Y = 60;
			PlayerLifeUI.container.add(PlayerLifeUI.gasmaskBox);
			PlayerLifeUI.gasmaskBox.isVisible = false;
			PlayerLifeUI.gasmaskIcon = new SleekImageTexture();
			PlayerLifeUI.gasmaskIcon.positionOffset_X = 5;
			PlayerLifeUI.gasmaskIcon.positionOffset_Y = 5;
			PlayerLifeUI.gasmaskIcon.sizeOffset_X = 50;
			PlayerLifeUI.gasmaskIcon.sizeOffset_Y = 50;
			PlayerLifeUI.gasmaskBox.add(PlayerLifeUI.gasmaskIcon);
			PlayerLifeUI.gasmaskProgress = new SleekProgress(string.Empty);
			PlayerLifeUI.gasmaskProgress.positionOffset_X = 60;
			PlayerLifeUI.gasmaskProgress.positionOffset_Y = 10;
			PlayerLifeUI.gasmaskProgress.sizeOffset_X = -70;
			PlayerLifeUI.gasmaskProgress.sizeOffset_Y = 40;
			PlayerLifeUI.gasmaskProgress.sizeScale_X = 1f;
			PlayerLifeUI.gasmaskBox.add(PlayerLifeUI.gasmaskProgress);
			PlayerLifeUI.bleedingBox = new SleekBoxIcon((Texture2D)PlayerLifeUI.icons.load("Bleeding"));
			PlayerLifeUI.bleedingBox.positionOffset_Y = -60;
			PlayerLifeUI.bleedingBox.sizeOffset_X = 50;
			PlayerLifeUI.bleedingBox.sizeOffset_Y = 50;
			PlayerLifeUI.lifeBox.add(PlayerLifeUI.bleedingBox);
			PlayerLifeUI.bleedingBox.isVisible = false;
			PlayerLifeUI.brokenBox = new SleekBoxIcon((Texture2D)PlayerLifeUI.icons.load("Broken"));
			PlayerLifeUI.brokenBox.positionOffset_Y = -60;
			PlayerLifeUI.brokenBox.sizeOffset_X = 50;
			PlayerLifeUI.brokenBox.sizeOffset_Y = 50;
			PlayerLifeUI.lifeBox.add(PlayerLifeUI.brokenBox);
			PlayerLifeUI.brokenBox.isVisible = false;
			PlayerLifeUI.temperatureBox = new SleekBoxIcon(null);
			PlayerLifeUI.temperatureBox.positionOffset_Y = -60;
			PlayerLifeUI.temperatureBox.sizeOffset_X = 50;
			PlayerLifeUI.temperatureBox.sizeOffset_Y = 50;
			PlayerLifeUI.lifeBox.add(PlayerLifeUI.temperatureBox);
			PlayerLifeUI.temperatureBox.isVisible = false;
			PlayerLifeUI.starvedBox = new SleekBoxIcon((Texture2D)PlayerLifeUI.icons.load("Starved"));
			PlayerLifeUI.starvedBox.positionOffset_Y = -60;
			PlayerLifeUI.starvedBox.sizeOffset_X = 50;
			PlayerLifeUI.starvedBox.sizeOffset_Y = 50;
			PlayerLifeUI.lifeBox.add(PlayerLifeUI.starvedBox);
			PlayerLifeUI.starvedBox.isVisible = false;
			PlayerLifeUI.dehydratedBox = new SleekBoxIcon((Texture2D)PlayerLifeUI.icons.load("Dehydrated"));
			PlayerLifeUI.dehydratedBox.positionOffset_Y = -60;
			PlayerLifeUI.dehydratedBox.sizeOffset_X = 50;
			PlayerLifeUI.dehydratedBox.sizeOffset_Y = 50;
			PlayerLifeUI.lifeBox.add(PlayerLifeUI.dehydratedBox);
			PlayerLifeUI.dehydratedBox.isVisible = false;
			PlayerLifeUI.infectedBox = new SleekBoxIcon((Texture2D)PlayerLifeUI.icons.load("Infected"));
			PlayerLifeUI.infectedBox.positionOffset_Y = -60;
			PlayerLifeUI.infectedBox.sizeOffset_X = 50;
			PlayerLifeUI.infectedBox.sizeOffset_Y = 50;
			PlayerLifeUI.lifeBox.add(PlayerLifeUI.infectedBox);
			PlayerLifeUI.infectedBox.isVisible = false;
			PlayerLifeUI.drownedBox = new SleekBoxIcon((Texture2D)PlayerLifeUI.icons.load("Drowned"));
			PlayerLifeUI.drownedBox.positionOffset_Y = -60;
			PlayerLifeUI.drownedBox.sizeOffset_X = 50;
			PlayerLifeUI.drownedBox.sizeOffset_Y = 50;
			PlayerLifeUI.lifeBox.add(PlayerLifeUI.drownedBox);
			PlayerLifeUI.drownedBox.isVisible = false;
			PlayerLifeUI.moonBox = new SleekBoxIcon((Texture2D)PlayerLifeUI.icons.load("Moon"));
			PlayerLifeUI.moonBox.positionOffset_Y = -60;
			PlayerLifeUI.moonBox.sizeOffset_X = 50;
			PlayerLifeUI.moonBox.sizeOffset_Y = 50;
			PlayerLifeUI.lifeBox.add(PlayerLifeUI.moonBox);
			PlayerLifeUI.moonBox.isVisible = false;
			PlayerLifeUI.radiationBox = new SleekBoxIcon((Texture2D)PlayerLifeUI.icons.load("Deadzone"));
			PlayerLifeUI.radiationBox.positionOffset_Y = -60;
			PlayerLifeUI.radiationBox.sizeOffset_X = 50;
			PlayerLifeUI.radiationBox.sizeOffset_Y = 50;
			PlayerLifeUI.lifeBox.add(PlayerLifeUI.radiationBox);
			PlayerLifeUI.radiationBox.isVisible = false;
			PlayerLifeUI.safeBox = new SleekBoxIcon((Texture2D)PlayerLifeUI.icons.load("Safe"));
			PlayerLifeUI.safeBox.positionOffset_Y = -60;
			PlayerLifeUI.safeBox.sizeOffset_X = 50;
			PlayerLifeUI.safeBox.sizeOffset_Y = 50;
			PlayerLifeUI.lifeBox.add(PlayerLifeUI.safeBox);
			PlayerLifeUI.safeBox.isVisible = false;
			PlayerLifeUI.arrestBox = new SleekBoxIcon((Texture2D)PlayerLifeUI.icons.load("Arrest"));
			PlayerLifeUI.arrestBox.positionOffset_Y = -60;
			PlayerLifeUI.arrestBox.sizeOffset_X = 50;
			PlayerLifeUI.arrestBox.sizeOffset_Y = 50;
			PlayerLifeUI.lifeBox.add(PlayerLifeUI.arrestBox);
			PlayerLifeUI.arrestBox.isVisible = false;
			if (Level.info != null)
			{
				if (Level.info.type == ELevelType.ARENA)
				{
					PlayerLifeUI.levelTextBox.isVisible = true;
					PlayerLifeUI.levelNumberBox.isVisible = true;
				}
				if (Level.info.type != ELevelType.SURVIVAL)
				{
					PlayerLifeUI.foodIcon.isVisible = false;
					PlayerLifeUI.foodProgress.isVisible = false;
					PlayerLifeUI.waterIcon.isVisible = false;
					PlayerLifeUI.waterProgress.isVisible = false;
					PlayerLifeUI.virusIcon.isVisible = false;
					PlayerLifeUI.virusProgress.isVisible = false;
					if (Level.info.type == ELevelType.HORDE)
					{
						PlayerLifeUI.oxygenIcon.isVisible = false;
						PlayerLifeUI.oxygenProgress.isVisible = false;
						PlayerLifeUI.waveLabel.isVisible = true;
						PlayerLifeUI.scoreLabel.isVisible = true;
						PlayerLifeUI.staminaIcon.positionOffset_Y = 35;
						PlayerLifeUI.staminaProgress.positionOffset_Y = 40;
						PlayerLifeUI.lifeBox.positionOffset_Y = -90;
						PlayerLifeUI.lifeBox.sizeOffset_Y = 90;
					}
					else
					{
						PlayerLifeUI.staminaIcon.positionOffset_Y = 35;
						PlayerLifeUI.staminaProgress.positionOffset_Y = 40;
						PlayerLifeUI.oxygenIcon.positionOffset_Y = 65;
						PlayerLifeUI.oxygenProgress.positionOffset_Y = 70;
						PlayerLifeUI.lifeBox.positionOffset_Y = -90;
						PlayerLifeUI.lifeBox.sizeOffset_Y = 90;
					}
				}
				else
				{
					PlayerLifeUI.moonBox.isVisible = LightingManager.isFullMoon;
					PlayerLifeUI.updateIcons();
				}
			}
			PlayerLife life = Player.player.life;
			Delegate onDamaged = life.onDamaged;
			if (PlayerLifeUI.<>f__mg$cache8 == null)
			{
				PlayerLifeUI.<>f__mg$cache8 = new Damaged(PlayerLifeUI.onDamaged);
			}
			life.onDamaged = (Damaged)Delegate.Combine(onDamaged, PlayerLifeUI.<>f__mg$cache8);
			PlayerLife life2 = Player.player.life;
			if (PlayerLifeUI.<>f__mg$cache9 == null)
			{
				PlayerLifeUI.<>f__mg$cache9 = new HealthUpdated(PlayerLifeUI.onHealthUpdated);
			}
			life2.onHealthUpdated = PlayerLifeUI.<>f__mg$cache9;
			PlayerLife life3 = Player.player.life;
			if (PlayerLifeUI.<>f__mg$cacheA == null)
			{
				PlayerLifeUI.<>f__mg$cacheA = new FoodUpdated(PlayerLifeUI.onFoodUpdated);
			}
			life3.onFoodUpdated = PlayerLifeUI.<>f__mg$cacheA;
			PlayerLife life4 = Player.player.life;
			if (PlayerLifeUI.<>f__mg$cacheB == null)
			{
				PlayerLifeUI.<>f__mg$cacheB = new WaterUpdated(PlayerLifeUI.onWaterUpdated);
			}
			life4.onWaterUpdated = PlayerLifeUI.<>f__mg$cacheB;
			PlayerLife life5 = Player.player.life;
			if (PlayerLifeUI.<>f__mg$cacheC == null)
			{
				PlayerLifeUI.<>f__mg$cacheC = new VirusUpdated(PlayerLifeUI.onVirusUpdated);
			}
			life5.onVirusUpdated = PlayerLifeUI.<>f__mg$cacheC;
			PlayerLife life6 = Player.player.life;
			if (PlayerLifeUI.<>f__mg$cacheD == null)
			{
				PlayerLifeUI.<>f__mg$cacheD = new StaminaUpdated(PlayerLifeUI.onStaminaUpdated);
			}
			life6.onStaminaUpdated = PlayerLifeUI.<>f__mg$cacheD;
			PlayerLife life7 = Player.player.life;
			if (PlayerLifeUI.<>f__mg$cacheE == null)
			{
				PlayerLifeUI.<>f__mg$cacheE = new OxygenUpdated(PlayerLifeUI.onOxygenUpdated);
			}
			life7.onOxygenUpdated = PlayerLifeUI.<>f__mg$cacheE;
			PlayerLife life8 = Player.player.life;
			if (PlayerLifeUI.<>f__mg$cacheF == null)
			{
				PlayerLifeUI.<>f__mg$cacheF = new BleedingUpdated(PlayerLifeUI.onBleedingUpdated);
			}
			life8.onBleedingUpdated = PlayerLifeUI.<>f__mg$cacheF;
			PlayerLife life9 = Player.player.life;
			if (PlayerLifeUI.<>f__mg$cache10 == null)
			{
				PlayerLifeUI.<>f__mg$cache10 = new BrokenUpdated(PlayerLifeUI.onBrokenUpdated);
			}
			life9.onBrokenUpdated = PlayerLifeUI.<>f__mg$cache10;
			PlayerLife life10 = Player.player.life;
			if (PlayerLifeUI.<>f__mg$cache11 == null)
			{
				PlayerLifeUI.<>f__mg$cache11 = new TemperatureUpdated(PlayerLifeUI.onTemperatureUpdated);
			}
			life10.onTemperatureUpdated = PlayerLifeUI.<>f__mg$cache11;
			PlayerLook look = Player.player.look;
			Delegate onPerspectiveUpdated = look.onPerspectiveUpdated;
			if (PlayerLifeUI.<>f__mg$cache12 == null)
			{
				PlayerLifeUI.<>f__mg$cache12 = new PerspectiveUpdated(PlayerLifeUI.onPerspectiveUpdated);
			}
			look.onPerspectiveUpdated = (PerspectiveUpdated)Delegate.Combine(onPerspectiveUpdated, PlayerLifeUI.<>f__mg$cache12);
			PlayerMovement movement = Player.player.movement;
			Delegate onSeated = movement.onSeated;
			if (PlayerLifeUI.<>f__mg$cache13 == null)
			{
				PlayerLifeUI.<>f__mg$cache13 = new Seated(PlayerLifeUI.onSeated);
			}
			movement.onSeated = (Seated)Delegate.Combine(onSeated, PlayerLifeUI.<>f__mg$cache13);
			PlayerMovement movement2 = Player.player.movement;
			Delegate onVehicleUpdated = movement2.onVehicleUpdated;
			if (PlayerLifeUI.<>f__mg$cache14 == null)
			{
				PlayerLifeUI.<>f__mg$cache14 = new VehicleUpdated(PlayerLifeUI.onVehicleUpdated);
			}
			movement2.onVehicleUpdated = (VehicleUpdated)Delegate.Combine(onVehicleUpdated, PlayerLifeUI.<>f__mg$cache14);
			PlayerMovement movement3 = Player.player.movement;
			Delegate onSafetyUpdated = movement3.onSafetyUpdated;
			if (PlayerLifeUI.<>f__mg$cache15 == null)
			{
				PlayerLifeUI.<>f__mg$cache15 = new SafetyUpdated(PlayerLifeUI.onSafetyUpdated);
			}
			movement3.onSafetyUpdated = (SafetyUpdated)Delegate.Combine(onSafetyUpdated, PlayerLifeUI.<>f__mg$cache15);
			PlayerMovement movement4 = Player.player.movement;
			Delegate onRadiationUpdated = movement4.onRadiationUpdated;
			if (PlayerLifeUI.<>f__mg$cache16 == null)
			{
				PlayerLifeUI.<>f__mg$cache16 = new RadiationUpdated(PlayerLifeUI.onRadiationUpdated);
			}
			movement4.onRadiationUpdated = (RadiationUpdated)Delegate.Combine(onRadiationUpdated, PlayerLifeUI.<>f__mg$cache16);
			PlayerAnimator animator = Player.player.animator;
			Delegate onGestureUpdated = animator.onGestureUpdated;
			if (PlayerLifeUI.<>f__mg$cache17 == null)
			{
				PlayerLifeUI.<>f__mg$cache17 = new GestureUpdated(PlayerLifeUI.onGestureUpdated);
			}
			animator.onGestureUpdated = (GestureUpdated)Delegate.Combine(onGestureUpdated, PlayerLifeUI.<>f__mg$cache17);
			PlayerVoice voice = Player.player.voice;
			Delegate onTalked = voice.onTalked;
			if (PlayerLifeUI.<>f__mg$cache18 == null)
			{
				PlayerLifeUI.<>f__mg$cache18 = new Talked(PlayerLifeUI.onTalked);
			}
			voice.onTalked = (Talked)Delegate.Combine(onTalked, PlayerLifeUI.<>f__mg$cache18);
			PlayerQuests quests = Player.player.quests;
			if (PlayerLifeUI.<>f__mg$cache19 == null)
			{
				PlayerLifeUI.<>f__mg$cache19 = new TrackedQuestUpdated(PlayerLifeUI.OnTrackedQuestUpdated);
			}
			quests.TrackedQuestUpdated += PlayerLifeUI.<>f__mg$cache19;
			PlayerSkills skills = Player.player.skills;
			Delegate onExperienceUpdated = skills.onExperienceUpdated;
			if (PlayerLifeUI.<>f__mg$cache1A == null)
			{
				PlayerLifeUI.<>f__mg$cache1A = new ExperienceUpdated(PlayerLifeUI.onExperienceUpdated);
			}
			skills.onExperienceUpdated = (ExperienceUpdated)Delegate.Combine(onExperienceUpdated, PlayerLifeUI.<>f__mg$cache1A);
			Delegate onMoonUpdated = LightingManager.onMoonUpdated;
			if (PlayerLifeUI.<>f__mg$cache1B == null)
			{
				PlayerLifeUI.<>f__mg$cache1B = new MoonUpdated(PlayerLifeUI.onMoonUpdated);
			}
			LightingManager.onMoonUpdated = (MoonUpdated)Delegate.Combine(onMoonUpdated, PlayerLifeUI.<>f__mg$cache1B);
			Delegate onWaveUpdated = ZombieManager.onWaveUpdated;
			if (PlayerLifeUI.<>f__mg$cache1C == null)
			{
				PlayerLifeUI.<>f__mg$cache1C = new WaveUpdated(PlayerLifeUI.onWaveUpdated);
			}
			ZombieManager.onWaveUpdated = (WaveUpdated)Delegate.Combine(onWaveUpdated, PlayerLifeUI.<>f__mg$cache1C);
			PlayerClothing clothing = Player.player.clothing;
			Delegate onMaskUpdated = clothing.onMaskUpdated;
			if (PlayerLifeUI.<>f__mg$cache1D == null)
			{
				PlayerLifeUI.<>f__mg$cache1D = new MaskUpdated(PlayerLifeUI.onMaskUpdated);
			}
			clothing.onMaskUpdated = (MaskUpdated)Delegate.Combine(onMaskUpdated, PlayerLifeUI.<>f__mg$cache1D);
			PlayerLifeUI.onListed();
			Delegate onListed = ChatManager.onListed;
			if (PlayerLifeUI.<>f__mg$cache1E == null)
			{
				PlayerLifeUI.<>f__mg$cache1E = new Listed(PlayerLifeUI.onListed);
			}
			ChatManager.onListed = (Listed)Delegate.Combine(onListed, PlayerLifeUI.<>f__mg$cache1E);
			Delegate onVotingStart = ChatManager.onVotingStart;
			if (PlayerLifeUI.<>f__mg$cache1F == null)
			{
				PlayerLifeUI.<>f__mg$cache1F = new VotingStart(PlayerLifeUI.onVotingStart);
			}
			ChatManager.onVotingStart = (VotingStart)Delegate.Combine(onVotingStart, PlayerLifeUI.<>f__mg$cache1F);
			Delegate onVotingUpdate = ChatManager.onVotingUpdate;
			if (PlayerLifeUI.<>f__mg$cache20 == null)
			{
				PlayerLifeUI.<>f__mg$cache20 = new VotingUpdate(PlayerLifeUI.onVotingUpdate);
			}
			ChatManager.onVotingUpdate = (VotingUpdate)Delegate.Combine(onVotingUpdate, PlayerLifeUI.<>f__mg$cache20);
			Delegate onVotingStop = ChatManager.onVotingStop;
			if (PlayerLifeUI.<>f__mg$cache21 == null)
			{
				PlayerLifeUI.<>f__mg$cache21 = new VotingStop(PlayerLifeUI.onVotingStop);
			}
			ChatManager.onVotingStop = (VotingStop)Delegate.Combine(onVotingStop, PlayerLifeUI.<>f__mg$cache21);
			Delegate onVotingMessage = ChatManager.onVotingMessage;
			if (PlayerLifeUI.<>f__mg$cache22 == null)
			{
				PlayerLifeUI.<>f__mg$cache22 = new VotingMessage(PlayerLifeUI.onVotingMessage);
			}
			ChatManager.onVotingMessage = (VotingMessage)Delegate.Combine(onVotingMessage, PlayerLifeUI.<>f__mg$cache22);
			Delegate onArenaMessageUpdated = LevelManager.onArenaMessageUpdated;
			if (PlayerLifeUI.<>f__mg$cache23 == null)
			{
				PlayerLifeUI.<>f__mg$cache23 = new ArenaMessageUpdated(PlayerLifeUI.onArenaMessageUpdated);
			}
			LevelManager.onArenaMessageUpdated = (ArenaMessageUpdated)Delegate.Combine(onArenaMessageUpdated, PlayerLifeUI.<>f__mg$cache23);
			Delegate onArenaPlayerUpdated = LevelManager.onArenaPlayerUpdated;
			if (PlayerLifeUI.<>f__mg$cache24 == null)
			{
				PlayerLifeUI.<>f__mg$cache24 = new ArenaPlayerUpdated(PlayerLifeUI.onArenaPlayerUpdated);
			}
			LevelManager.onArenaPlayerUpdated = (ArenaPlayerUpdated)Delegate.Combine(onArenaPlayerUpdated, PlayerLifeUI.<>f__mg$cache24);
			Delegate onLevelNumberUpdated = LevelManager.onLevelNumberUpdated;
			if (PlayerLifeUI.<>f__mg$cache25 == null)
			{
				PlayerLifeUI.<>f__mg$cache25 = new LevelNumberUpdated(PlayerLifeUI.onLevelNumberUpdated);
			}
			LevelManager.onLevelNumberUpdated = (LevelNumberUpdated)Delegate.Combine(onLevelNumberUpdated, PlayerLifeUI.<>f__mg$cache25);
		}

		public static Sleek container
		{
			get
			{
				return PlayerLifeUI._container;
			}
		}

		public static void open()
		{
			if (PlayerLifeUI.active)
			{
				return;
			}
			PlayerLifeUI.active = true;
			if (PlayerLifeUI.npc != null)
			{
				PlayerLifeUI.npc.isLookingAtPlayer = false;
				PlayerLifeUI.npc = null;
			}
			PlayerLifeUI.container.lerpPositionScale(0f, 0f, ESleekLerp.EXPONENTIAL, 20f);
		}

		public static void close()
		{
			if (!PlayerLifeUI.active)
			{
				return;
			}
			PlayerLifeUI.active = false;
			PlayerLifeUI.closeChat();
			PlayerLifeUI.closeGestures();
			PlayerLifeUI.container.lerpPositionScale(0f, 1f, ESleekLerp.EXPONENTIAL, 20f);
		}

		public static void openChat()
		{
			if (PlayerLifeUI.chatting)
			{
				return;
			}
			PlayerLifeUI.chatting = true;
			PlayerLifeUI.chatField.text = string.Empty;
			PlayerLifeUI.chatField.lerpPositionOffset(100, 170, ESleekLerp.EXPONENTIAL, 20f);
			if (PlayerUI.chat == EChatMode.GLOBAL)
			{
				PlayerLifeUI.modeBox.text = PlayerLifeUI.localization.format("Mode_Global");
			}
			else if (PlayerUI.chat == EChatMode.LOCAL)
			{
				PlayerLifeUI.modeBox.text = PlayerLifeUI.localization.format("Mode_Local");
			}
			else if (PlayerUI.chat == EChatMode.GROUP)
			{
				PlayerLifeUI.modeBox.text = PlayerLifeUI.localization.format("Mode_Group");
			}
			else
			{
				PlayerLifeUI.modeBox.text = "?";
			}
			if (Player.player.channel.owner.isAdmin && !Provider.isServer)
			{
				PlayerLifeUI.modeBox.backgroundColor = Palette.ADMIN;
				PlayerLifeUI.modeBox.foregroundColor = Palette.ADMIN;
				PlayerLifeUI.chatField.backgroundColor = Palette.ADMIN;
				PlayerLifeUI.chatField.foregroundColor = Palette.ADMIN;
			}
			else if (Provider.isPro)
			{
				PlayerLifeUI.modeBox.backgroundColor = Palette.PRO;
				PlayerLifeUI.modeBox.foregroundColor = Palette.PRO;
				PlayerLifeUI.chatField.backgroundColor = Palette.PRO;
				PlayerLifeUI.chatField.foregroundColor = Palette.PRO;
			}
			else
			{
				PlayerLifeUI.modeBox.backgroundColor = Color.white;
				PlayerLifeUI.modeBox.foregroundColor = Color.white;
				PlayerLifeUI.chatField.backgroundColor = Color.white;
				PlayerLifeUI.chatField.foregroundColor = Color.white;
			}
			PlayerLifeUI.chatBox.state = new Vector2(0f, float.MaxValue);
			PlayerLifeUI.chatBox.isVisible = true;
			for (int i = 0; i < 4; i++)
			{
				PlayerLifeUI.gameLabel[i].isVisible = false;
			}
		}

		public static void closeChat()
		{
			if (!PlayerLifeUI.chatting)
			{
				return;
			}
			PlayerLifeUI.chatting = false;
			PlayerLifeUI.chatField.lerpPositionOffset(-424, 170, ESleekLerp.EXPONENTIAL, 20f);
			PlayerLifeUI.chatBox.isVisible = false;
			for (int i = 0; i < 4; i++)
			{
				PlayerLifeUI.gameLabel[i].isVisible = true;
			}
		}

		public static void openGestures()
		{
			if (PlayerLifeUI.gesturing)
			{
				return;
			}
			PlayerLifeUI.gesturing = true;
			for (int i = 0; i < PlayerLifeUI.faceButtons.Length; i++)
			{
				PlayerLifeUI.faceButtons[i].isVisible = true;
			}
			bool isVisible = !Player.player.equipment.isSelected && Player.player.stance.stance != EPlayerStance.PRONE && Player.player.stance.stance != EPlayerStance.DRIVING && Player.player.stance.stance != EPlayerStance.SITTING;
			PlayerLifeUI.surrenderButton.isVisible = isVisible;
			PlayerLifeUI.pointButton.isVisible = isVisible;
			PlayerLifeUI.waveButton.isVisible = isVisible;
			PlayerLifeUI.saluteButton.isVisible = isVisible;
			PlayerLifeUI.restButton.isVisible = isVisible;
			PlayerLifeUI.facepalmButton.isVisible = isVisible;
		}

		public static void closeGestures()
		{
			if (!PlayerLifeUI.gesturing)
			{
				return;
			}
			PlayerLifeUI.gesturing = false;
			for (int i = 0; i < PlayerLifeUI.faceButtons.Length; i++)
			{
				PlayerLifeUI.faceButtons[i].isVisible = false;
			}
			PlayerLifeUI.surrenderButton.isVisible = false;
			PlayerLifeUI.pointButton.isVisible = false;
			PlayerLifeUI.waveButton.isVisible = false;
			PlayerLifeUI.saluteButton.isVisible = false;
			PlayerLifeUI.restButton.isVisible = false;
			PlayerLifeUI.facepalmButton.isVisible = false;
		}

		private static void onDamaged(byte damage)
		{
			if (damage > 5)
			{
				PlayerUI.pain(Mathf.Clamp((float)damage / 40f, 0f, 1f));
			}
		}

		public static void updateGrayscale()
		{
			GrayscaleEffect component = Player.player.animator.view.GetComponent<GrayscaleEffect>();
			GrayscaleEffect component2 = MainCamera.instance.GetComponent<GrayscaleEffect>();
			GrayscaleEffect component3 = Player.player.look.characterCamera.GetComponent<GrayscaleEffect>();
			if (Player.player.look.perspective == EPlayerPerspective.FIRST)
			{
				component.enabled = true;
				component2.enabled = false;
			}
			else
			{
				component.enabled = false;
				component2.enabled = true;
			}
			if (LevelLighting.vision == ELightingVision.CIVILIAN)
			{
				component.blend = 1f;
			}
			else if (Player.player.life.health < 50)
			{
				component.blend = (1f - (float)Player.player.life.health / 50f) * (1f - Player.player.skills.mastery(1, 3) * 0.75f);
			}
			else
			{
				component.blend = 0f;
			}
			component2.blend = component.blend;
			component3.blend = component.blend;
		}

		private static void onPerspectiveUpdated(EPlayerPerspective newPerspective)
		{
			PlayerLifeUI.updateGrayscale();
		}

		private static void onHealthUpdated(byte newHealth)
		{
			PlayerLifeUI.healthProgress.state = (float)newHealth / 100f;
			PlayerLifeUI.onPerspectiveUpdated(Player.player.look.perspective);
		}

		private static void onFoodUpdated(byte newFood)
		{
			if (newFood == 0 != PlayerLifeUI.starvedBox.isVisible)
			{
				PlayerLifeUI.starvedBox.isVisible = (newFood == 0);
				PlayerLifeUI.updateIcons();
			}
			PlayerLifeUI.foodProgress.state = (float)newFood / 100f;
		}

		private static void onWaterUpdated(byte newWater)
		{
			if (newWater == 0 != PlayerLifeUI.dehydratedBox.isVisible)
			{
				PlayerLifeUI.dehydratedBox.isVisible = (newWater == 0);
				PlayerLifeUI.updateIcons();
			}
			PlayerLifeUI.waterProgress.state = (float)newWater / 100f;
		}

		private static void onVirusUpdated(byte newVirus)
		{
			if (newVirus == 0 != PlayerLifeUI.infectedBox.isVisible)
			{
				PlayerLifeUI.infectedBox.isVisible = (newVirus == 0);
				PlayerLifeUI.updateIcons();
			}
			PlayerLifeUI.virusProgress.state = (float)newVirus / 100f;
		}

		private static void onStaminaUpdated(byte newStamina)
		{
			PlayerLifeUI.staminaProgress.state = (float)newStamina / 100f;
		}

		private static void onOxygenUpdated(byte newOxygen)
		{
			if (newOxygen == 0 != PlayerLifeUI.drownedBox.isVisible)
			{
				PlayerLifeUI.drownedBox.isVisible = (newOxygen == 0);
				PlayerLifeUI.updateIcons();
			}
			PlayerLifeUI.oxygenProgress.state = (float)newOxygen / 100f;
		}

		private static void updateIcons()
		{
			int num = 0;
			if (PlayerLifeUI.bleedingBox.isVisible)
			{
				num += 60;
			}
			PlayerLifeUI.brokenBox.positionOffset_X = num;
			if (PlayerLifeUI.brokenBox.isVisible)
			{
				num += 60;
			}
			PlayerLifeUI.temperatureBox.positionOffset_X = num;
			if (PlayerLifeUI.temperatureBox.isVisible)
			{
				num += 60;
			}
			PlayerLifeUI.starvedBox.positionOffset_X = num;
			if (PlayerLifeUI.starvedBox.isVisible)
			{
				num += 60;
			}
			PlayerLifeUI.dehydratedBox.positionOffset_X = num;
			if (PlayerLifeUI.dehydratedBox.isVisible)
			{
				num += 60;
			}
			PlayerLifeUI.infectedBox.positionOffset_X = num;
			if (PlayerLifeUI.infectedBox.isVisible)
			{
				num += 60;
			}
			PlayerLifeUI.drownedBox.positionOffset_X = num;
			if (PlayerLifeUI.drownedBox.isVisible)
			{
				num += 60;
			}
			PlayerLifeUI.moonBox.positionOffset_X = num;
			if (PlayerLifeUI.moonBox.isVisible)
			{
				num += 60;
			}
			PlayerLifeUI.radiationBox.positionOffset_X = num;
			if (PlayerLifeUI.radiationBox.isVisible)
			{
				num += 60;
			}
			PlayerLifeUI.safeBox.positionOffset_X = num;
			if (PlayerLifeUI.safeBox.isVisible)
			{
				num += 60;
			}
			PlayerLifeUI.arrestBox.positionOffset_X = num;
		}

		private static void onBleedingUpdated(bool newBleeding)
		{
			PlayerLifeUI.bleedingBox.isVisible = newBleeding;
			PlayerLifeUI.updateIcons();
		}

		private static void onBrokenUpdated(bool newBroken)
		{
			PlayerLifeUI.brokenBox.isVisible = newBroken;
			PlayerLifeUI.updateIcons();
		}

		private static void onTemperatureUpdated(EPlayerTemperature newTemperature)
		{
			PlayerLifeUI.temperatureBox.isVisible = (newTemperature != EPlayerTemperature.NONE);
			switch (newTemperature)
			{
			case EPlayerTemperature.FREEZING:
				PlayerLifeUI.temperatureBox.icon = (Texture2D)PlayerLifeUI.icons.load("Freezing");
				goto IL_11A;
			case EPlayerTemperature.COLD:
				PlayerLifeUI.temperatureBox.icon = (Texture2D)PlayerLifeUI.icons.load("Cold");
				goto IL_11A;
			case EPlayerTemperature.WARM:
				PlayerLifeUI.temperatureBox.icon = (Texture2D)PlayerLifeUI.icons.load("Warm");
				goto IL_11A;
			case EPlayerTemperature.BURNING:
				PlayerLifeUI.temperatureBox.icon = (Texture2D)PlayerLifeUI.icons.load("Burning");
				goto IL_11A;
			case EPlayerTemperature.COVERED:
				PlayerLifeUI.temperatureBox.icon = (Texture2D)PlayerLifeUI.icons.load("Covered");
				goto IL_11A;
			case EPlayerTemperature.ACID:
				PlayerLifeUI.temperatureBox.icon = (Texture2D)PlayerLifeUI.icons.load("Acid");
				goto IL_11A;
			}
			PlayerLifeUI.temperatureBox.icon = null;
			IL_11A:
			PlayerLifeUI.updateIcons();
		}

		private static void onMoonUpdated(bool isFullMoon)
		{
			PlayerLifeUI.moonBox.isVisible = isFullMoon;
			PlayerLifeUI.updateIcons();
		}

		private static void onExperienceUpdated(uint newExperience)
		{
			PlayerLifeUI.scoreLabel.text = PlayerLifeUI.localization.format("Score", new object[]
			{
				newExperience.ToString()
			});
		}

		private static void onWaveUpdated(bool newWaveReady, int newWaveIndex)
		{
			PlayerLifeUI.waveLabel.text = PlayerLifeUI.localization.format("Round", new object[]
			{
				newWaveIndex
			});
			if (newWaveReady)
			{
				PlayerUI.message(EPlayerMessage.WAVE_ON, string.Empty);
			}
			else
			{
				PlayerUI.message(EPlayerMessage.WAVE_OFF, string.Empty);
			}
		}

		private static void onSeated(bool isDriver, bool inVehicle, bool wasVehicle, InteractableVehicle oldVehicle, InteractableVehicle newVehicle)
		{
			if (isDriver && inVehicle && newVehicle.passengers[(int)Player.player.movement.getSeat()].turret != null)
			{
				PlayerLifeUI.vehicleBox.positionOffset_Y = -200;
			}
			else
			{
				PlayerLifeUI.vehicleBox.positionOffset_Y = -120;
			}
			PlayerLifeUI.vehicleBox.isVisible = (isDriver && inVehicle);
		}

		private static void onVehicleUpdated(bool isDriveable, ushort newFuel, ushort maxFuel, float newSpeed, float minSpeed, float maxSpeed, ushort newHealth, ushort maxHealth, ushort newBatteryCharge)
		{
			if (isDriveable)
			{
				PlayerLifeUI.fuelProgress.state = (float)newFuel / (float)maxFuel;
				float num = Mathf.Clamp(newSpeed, minSpeed, maxSpeed);
				if (num > 0f)
				{
					num /= maxSpeed;
				}
				else
				{
					num /= minSpeed;
				}
				PlayerLifeUI.speedProgress.state = num;
				if (OptionsSettings.metric)
				{
					PlayerLifeUI.speedProgress.measure = (int)MeasurementTool.speedToKPH(Mathf.Abs(newSpeed));
				}
				else
				{
					PlayerLifeUI.speedProgress.measure = (int)MeasurementTool.KPHToMPH(MeasurementTool.speedToKPH(Mathf.Abs(newSpeed)));
				}
				PlayerLifeUI.batteryChargeProgress.state = (float)newBatteryCharge / 10000f;
				PlayerLifeUI.hpProgress.state = (float)newHealth / (float)maxHealth;
			}
			PlayerLifeUI.vehicleBox.isVisible = isDriveable;
		}

		private static void updateGasmask()
		{
			if (Player.player.movement.isRadiated)
			{
				ItemMaskAsset itemMaskAsset = (ItemMaskAsset)Assets.find(EAssetType.ITEM, Player.player.clothing.mask);
				if (itemMaskAsset != null && itemMaskAsset.proofRadiation)
				{
					ItemTool.getIcon(Player.player.clothing.mask, Player.player.clothing.maskQuality, Player.player.clothing.maskState, new ItemIconReady(PlayerLifeUI.gasmaskIcon.updateTexture));
					PlayerLifeUI.gasmaskProgress.state = (float)Player.player.clothing.maskQuality / 100f;
					PlayerLifeUI.gasmaskProgress.color = ItemTool.getQualityColor((float)Player.player.clothing.maskQuality / 100f);
					PlayerLifeUI.gasmaskBox.isVisible = true;
				}
				else
				{
					PlayerLifeUI.gasmaskBox.isVisible = false;
				}
			}
			else
			{
				PlayerLifeUI.gasmaskBox.isVisible = false;
			}
		}

		private static void onMaskUpdated(ushort newMask, byte newMaskQuality, byte[] newMaskState)
		{
			PlayerLifeUI.updateGasmask();
		}

		private static void onSafetyUpdated(bool isSafe)
		{
			PlayerLifeUI.safeBox.isVisible = isSafe;
			PlayerLifeUI.updateIcons();
			if (isSafe)
			{
				PlayerUI.message(EPlayerMessage.SAFEZONE_ON, string.Empty);
			}
			else
			{
				PlayerUI.message(EPlayerMessage.SAFEZONE_OFF, string.Empty);
			}
		}

		private static void onRadiationUpdated(bool isRadiated)
		{
			PlayerLifeUI.radiationBox.isVisible = isRadiated;
			PlayerLifeUI.updateIcons();
			if (isRadiated)
			{
				PlayerUI.message(EPlayerMessage.DEADZONE_ON, string.Empty);
			}
			else
			{
				PlayerUI.message(EPlayerMessage.DEADZONE_OFF, string.Empty);
			}
			PlayerLifeUI.updateGasmask();
		}

		private static void onGestureUpdated(EPlayerGesture gesture)
		{
			PlayerLifeUI.arrestBox.isVisible = (gesture == EPlayerGesture.ARREST_START);
			PlayerLifeUI.updateIcons();
		}

		private static void onTalked(bool isTalking)
		{
			PlayerLifeUI.voiceBox.isVisible = isTalking;
		}

		private static void UpdateTrackedQuest()
		{
			QuestAsset questAsset = Assets.find(EAssetType.NPC, Player.player.quests.TrackedQuestID) as QuestAsset;
			if (questAsset == null)
			{
				PlayerLifeUI.trackedQuestTitle.isVisible = false;
				PlayerLifeUI.trackedQuestBar.isVisible = false;
				return;
			}
			PlayerLifeUI.trackedQuestTitle.text = questAsset.questName;
			bool flag = true;
			if (questAsset.conditions != null)
			{
				PlayerLifeUI.trackedQuestBar.remove();
				int num = 5;
				for (int i = 0; i < questAsset.conditions.Length; i++)
				{
					INPCCondition inpccondition = questAsset.conditions[i];
					if (inpccondition != null && !inpccondition.isConditionMet(Player.player))
					{
						string text = inpccondition.formatCondition(Player.player);
						if (!string.IsNullOrEmpty(text))
						{
							SleekLabel sleekLabel = new SleekLabel();
							sleekLabel.positionOffset_X = -300;
							sleekLabel.positionOffset_Y = num;
							sleekLabel.sizeOffset_X = 500;
							sleekLabel.sizeOffset_Y = 20;
							sleekLabel.isRich = true;
							sleekLabel.fontAlignment = 5;
							sleekLabel.text = text;
							PlayerLifeUI.trackedQuestBar.add(sleekLabel);
							num += 20;
							flag = false;
						}
					}
				}
			}
			PlayerLifeUI.trackedQuestTitle.isVisible = !flag;
			PlayerLifeUI.trackedQuestBar.isVisible = PlayerLifeUI.trackedQuestTitle.isVisible;
		}

		private static void OnTrackedQuestUpdated(PlayerQuests quests)
		{
			PlayerLifeUI.UpdateTrackedQuest();
		}

		private static void onListed()
		{
			for (int i = ChatManager.chat.Length - 1; i >= 0; i--)
			{
				Chat chat = ChatManager.chat[i];
				if (chat != null)
				{
					if (i == 0)
					{
						if (chat.player != null)
						{
							Texture2D texture;
							if (OptionsSettings.streamer)
							{
								texture = null;
							}
							else
							{
								texture = Provider.provider.communityService.getIcon(chat.player.playerID.steamID);
							}
							PlayerLifeUI.chatLabel[i].avatarImage.texture = texture;
							if (chat.player.player != null && chat.player.player.skills != null)
							{
								PlayerLifeUI.chatLabel[i].repImage.texture = PlayerTool.getRepTexture(chat.player.player.skills.reputation);
								PlayerLifeUI.chatLabel[i].repImage.backgroundColor = PlayerTool.getRepColor(chat.player.player.skills.reputation);
							}
							else
							{
								PlayerLifeUI.chatLabel[i].repImage.texture = null;
							}
						}
						else
						{
							PlayerLifeUI.chatLabel[i].avatarImage.texture = null;
							PlayerLifeUI.chatLabel[i].repImage.texture = null;
						}
					}
					else
					{
						if (i == ChatManager.chat.Length - 1 && PlayerLifeUI.chatLabel[i].avatarImage.texture != null)
						{
							Object.DestroyImmediate(PlayerLifeUI.chatLabel[i].avatarImage.texture);
						}
						PlayerLifeUI.chatLabel[i].avatarImage.texture = PlayerLifeUI.chatLabel[i - 1].avatarImage.texture;
						PlayerLifeUI.chatLabel[i].repImage.texture = PlayerLifeUI.chatLabel[i - 1].repImage.texture;
						PlayerLifeUI.chatLabel[i].repImage.backgroundColor = PlayerLifeUI.chatLabel[i - 1].repImage.backgroundColor;
					}
					PlayerLifeUI.chatLabel[i].msg.foregroundColor = chat.color;
					if (chat.mode == EChatMode.GLOBAL)
					{
						PlayerLifeUI.chatLabel[i].msg.text = PlayerLifeUI.localization.format("Chat_Global", new object[]
						{
							chat.speaker,
							chat.text
						});
					}
					else if (chat.mode == EChatMode.LOCAL)
					{
						PlayerLifeUI.chatLabel[i].msg.text = PlayerLifeUI.localization.format("Chat_Local", new object[]
						{
							chat.speaker,
							chat.text
						});
					}
					else if (chat.mode == EChatMode.GROUP)
					{
						PlayerLifeUI.chatLabel[i].msg.text = PlayerLifeUI.localization.format("Chat_Group", new object[]
						{
							chat.speaker,
							chat.text
						});
					}
					else if (chat.mode == EChatMode.WELCOME)
					{
						PlayerLifeUI.chatLabel[i].msg.text = PlayerLifeUI.localization.format("Chat_Welcome", new object[]
						{
							chat.speaker,
							chat.text
						});
					}
					else if (chat.mode == EChatMode.SAY)
					{
						PlayerLifeUI.chatLabel[i].msg.text = PlayerLifeUI.localization.format("Chat_Say", new object[]
						{
							chat.speaker,
							chat.text
						});
					}
					else
					{
						PlayerLifeUI.chatLabel[i].msg.text = "?";
					}
				}
			}
			for (int j = 0; j < 4; j++)
			{
				PlayerLifeUI.gameLabel[j].avatarImage.texture = PlayerLifeUI.chatLabel[j].avatarImage.texture;
				PlayerLifeUI.gameLabel[j].repImage.texture = PlayerLifeUI.chatLabel[j].repImage.texture;
				PlayerLifeUI.gameLabel[j].repImage.backgroundColor = PlayerLifeUI.chatLabel[j].repImage.backgroundColor;
				PlayerLifeUI.gameLabel[j].msg.foregroundColor = PlayerLifeUI.chatLabel[j].msg.foregroundColor;
				PlayerLifeUI.gameLabel[j].msg.text = PlayerLifeUI.chatLabel[j].msg.text;
			}
		}

		private static void onVotingStart(SteamPlayer origin, SteamPlayer target, byte votesNeeded)
		{
			PlayerLifeUI.isVoteMessaged = false;
			PlayerLifeUI.voteBox.text = string.Empty;
			PlayerLifeUI.voteBox.isVisible = true;
			PlayerLifeUI.voteInfoLabel.isVisible = true;
			PlayerLifeUI.votesNeededLabel.isVisible = true;
			PlayerLifeUI.voteYesLabel.isVisible = true;
			PlayerLifeUI.voteNoLabel.isVisible = true;
			PlayerLifeUI.voteInfoLabel.text = PlayerLifeUI.localization.format("Vote_Kick", new object[]
			{
				origin.playerID.characterName,
				origin.playerID.playerName,
				target.playerID.characterName,
				target.playerID.playerName
			});
			PlayerLifeUI.votesNeededLabel.text = PlayerLifeUI.localization.format("Votes_Needed", new object[]
			{
				votesNeeded
			});
			PlayerLifeUI.voteYesLabel.text = PlayerLifeUI.localization.format("Vote_Yes", new object[]
			{
				282,
				0
			});
			PlayerLifeUI.voteNoLabel.text = PlayerLifeUI.localization.format("Vote_No", new object[]
			{
				283,
				0
			});
		}

		private static void onVotingUpdate(byte voteYes, byte voteNo)
		{
			PlayerLifeUI.voteYesLabel.text = PlayerLifeUI.localization.format("Vote_Yes", new object[]
			{
				282,
				voteYes
			});
			PlayerLifeUI.voteNoLabel.text = PlayerLifeUI.localization.format("Vote_No", new object[]
			{
				283,
				voteNo
			});
		}

		private static void onVotingStop(EVotingMessage message)
		{
			PlayerLifeUI.voteInfoLabel.isVisible = false;
			PlayerLifeUI.votesNeededLabel.isVisible = false;
			PlayerLifeUI.voteYesLabel.isVisible = false;
			PlayerLifeUI.voteNoLabel.isVisible = false;
			if (message == EVotingMessage.PASS)
			{
				PlayerLifeUI.voteBox.text = PlayerLifeUI.localization.format("Vote_Pass");
			}
			else if (message == EVotingMessage.FAIL)
			{
				PlayerLifeUI.voteBox.text = PlayerLifeUI.localization.format("Vote_Fail");
			}
			PlayerLifeUI.isVoteMessaged = true;
			PlayerLifeUI.lastVoteMessage = Time.realtimeSinceStartup;
		}

		private static void onVotingMessage(EVotingMessage message)
		{
			PlayerLifeUI.voteBox.isVisible = true;
			PlayerLifeUI.voteInfoLabel.isVisible = false;
			PlayerLifeUI.votesNeededLabel.isVisible = false;
			PlayerLifeUI.voteYesLabel.isVisible = false;
			PlayerLifeUI.voteNoLabel.isVisible = false;
			if (message == EVotingMessage.OFF)
			{
				PlayerLifeUI.voteBox.text = PlayerLifeUI.localization.format("Vote_Off");
			}
			else if (message == EVotingMessage.DELAY)
			{
				PlayerLifeUI.voteBox.text = PlayerLifeUI.localization.format("Vote_Delay");
			}
			else if (message == EVotingMessage.PLAYERS)
			{
				PlayerLifeUI.voteBox.text = PlayerLifeUI.localization.format("Vote_Players");
			}
			PlayerLifeUI.isVoteMessaged = true;
			PlayerLifeUI.lastVoteMessage = Time.realtimeSinceStartup;
		}

		private static void onArenaMessageUpdated(EArenaMessage newArenaMessage)
		{
			switch (newArenaMessage)
			{
			case EArenaMessage.LOBBY:
				PlayerLifeUI.levelTextBox.text = PlayerLifeUI.localization.format("Arena_Lobby");
				return;
			case EArenaMessage.WARMUP:
				PlayerLifeUI.levelTextBox.text = PlayerLifeUI.localization.format("Arena_Warm_Up");
				return;
			case EArenaMessage.PLAY:
				PlayerLifeUI.levelTextBox.text = PlayerLifeUI.localization.format("Arena_Play");
				return;
			case EArenaMessage.LOSE:
				PlayerLifeUI.levelTextBox.text = PlayerLifeUI.localization.format("Arena_Lose");
				return;
			case EArenaMessage.INTERMISSION:
				PlayerLifeUI.levelTextBox.text = PlayerLifeUI.localization.format("Arena_Intermission");
				return;
			}
		}

		private static void onArenaPlayerUpdated(ulong[] playerIDs, EArenaMessage newArenaMessage)
		{
			List<SteamPlayer> list = new List<SteamPlayer>();
			for (int i = 0; i < playerIDs.Length; i++)
			{
				SteamPlayer steamPlayer = PlayerTool.getSteamPlayer(playerIDs[i]);
				if (steamPlayer != null)
				{
					list.Add(steamPlayer);
				}
			}
			if (list.Count == 0)
			{
				return;
			}
			string text = string.Empty;
			for (int j = 0; j < list.Count; j++)
			{
				SteamPlayer steamPlayer2 = list[j];
				if (j == 0)
				{
					text += steamPlayer2.playerID.characterName;
				}
				else if (j == list.Count - 1)
				{
					text = text + PlayerLifeUI.localization.format("List_Joint_1") + steamPlayer2.playerID.characterName;
				}
				else
				{
					text = text + PlayerLifeUI.localization.format("List_Joint_0") + steamPlayer2.playerID.characterName;
				}
			}
			if (newArenaMessage == EArenaMessage.DIED)
			{
				PlayerLifeUI.levelTextBox.text = PlayerLifeUI.localization.format("Arena_Died", new object[]
				{
					text
				});
				return;
			}
			if (newArenaMessage == EArenaMessage.ABANDONED)
			{
				PlayerLifeUI.levelTextBox.text = PlayerLifeUI.localization.format("Arena_Abandoned", new object[]
				{
					text
				});
				return;
			}
			if (newArenaMessage != EArenaMessage.WIN)
			{
				return;
			}
			PlayerLifeUI.levelTextBox.text = PlayerLifeUI.localization.format("Arena_Win", new object[]
			{
				text
			});
		}

		private static void onLevelNumberUpdated(int newLevelNumber)
		{
			PlayerLifeUI.levelNumberBox.text = newLevelNumber.ToString();
		}

		private static void onClickedFaceButton(SleekButton button)
		{
			byte b = 0;
			while ((int)b < PlayerLifeUI.faceButtons.Length)
			{
				if (PlayerLifeUI.faceButtons[(int)b] == button)
				{
					break;
				}
				b += 1;
			}
			Player.player.clothing.sendSwapFace(b);
			PlayerLifeUI.closeGestures();
		}

		private static void onClickedSurrenderButton(SleekButton button)
		{
			if (Player.player.animator.gesture == EPlayerGesture.SURRENDER_START)
			{
				Player.player.animator.sendGesture(EPlayerGesture.SURRENDER_STOP, true);
			}
			else
			{
				Player.player.animator.sendGesture(EPlayerGesture.SURRENDER_START, true);
			}
			PlayerLifeUI.closeGestures();
		}

		private static void onClickedPointButton(SleekButton button)
		{
			Player.player.animator.sendGesture(EPlayerGesture.POINT, true);
			PlayerLifeUI.closeGestures();
		}

		private static void onClickedWaveButton(SleekButton button)
		{
			Player.player.animator.sendGesture(EPlayerGesture.WAVE, true);
			PlayerLifeUI.closeGestures();
		}

		private static void onClickedSaluteButton(SleekButton button)
		{
			Player.player.animator.sendGesture(EPlayerGesture.SALUTE, true);
			PlayerLifeUI.closeGestures();
		}

		private static void onClickedRestButton(SleekButton button)
		{
			if (Player.player.animator.gesture == EPlayerGesture.REST_START)
			{
				Player.player.animator.sendGesture(EPlayerGesture.REST_STOP, true);
			}
			else
			{
				Player.player.animator.sendGesture(EPlayerGesture.REST_START, true);
			}
			PlayerLifeUI.closeGestures();
		}

		private static void onClickedFacepalmButton(SleekButton button)
		{
			Player.player.animator.sendGesture(EPlayerGesture.FACEPALM, true);
			PlayerLifeUI.closeGestures();
		}

		public static Local localization;

		public static Bundle icons;

		private static Sleek _container;

		public static bool active;

		public static bool chatting;

		public static bool gesturing;

		public static InteractableObjectNPC npc;

		public static bool isVoteMessaged;

		public static float lastVoteMessage;

		private static SleekScrollBox chatBox;

		private static SleekChat[] chatLabel;

		private static SleekChat[] gameLabel;

		public static SleekField chatField;

		private static SleekBox modeBox;

		public static SleekBox voteBox;

		private static SleekLabel voteInfoLabel;

		private static SleekLabel votesNeededLabel;

		private static SleekLabel voteYesLabel;

		private static SleekLabel voteNoLabel;

		private static SleekBoxIcon voiceBox;

		private static SleekLabel trackedQuestTitle;

		private static SleekImageTexture trackedQuestBar;

		private static SleekBox levelTextBox;

		private static SleekBox levelNumberBox;

		private static SleekButton[] faceButtons;

		private static SleekButton surrenderButton;

		private static SleekButton pointButton;

		private static SleekButton waveButton;

		private static SleekButton saluteButton;

		private static SleekButton restButton;

		private static SleekButton facepalmButton;

		public static SleekImageTexture painImage;

		public static SleekImageTexture scopeImage;

		public static SleekImageTexture scopeOverlay;

		public static SleekImageTexture scopeLeftOverlay;

		public static SleekImageTexture scopeRightOverlay;

		public static SleekImageTexture scopeUpOverlay;

		public static SleekImageTexture scopeDownOverlay;

		public static SleekImageTexture binocularsOverlay;

		public static HitmarkerInfo[] hitmarkers;

		public static SleekImageTexture crosshairLeftImage;

		public static SleekImageTexture crosshairRightImage;

		public static SleekImageTexture crosshairDownImage;

		public static SleekImageTexture crosshairUpImage;

		public static SleekImageTexture dotImage;

		private static SleekBox lifeBox;

		private static SleekImageTexture healthIcon;

		private static SleekProgress healthProgress;

		private static SleekImageTexture foodIcon;

		private static SleekProgress foodProgress;

		private static SleekImageTexture waterIcon;

		private static SleekProgress waterProgress;

		private static SleekImageTexture virusIcon;

		private static SleekProgress virusProgress;

		private static SleekImageTexture staminaIcon;

		private static SleekProgress staminaProgress;

		private static SleekLabel waveLabel;

		private static SleekLabel scoreLabel;

		private static SleekImageTexture oxygenIcon;

		private static SleekProgress oxygenProgress;

		private static SleekBox vehicleBox;

		private static SleekImageTexture fuelIcon;

		private static SleekProgress fuelProgress;

		private static SleekBox gasmaskBox;

		private static SleekImageTexture gasmaskIcon;

		private static SleekProgress gasmaskProgress;

		private static SleekImageTexture speedIcon;

		private static SleekProgress speedProgress;

		private static SleekImageTexture batteryChargeIcon;

		private static SleekProgress batteryChargeProgress;

		private static SleekImageTexture hpIcon;

		private static SleekProgress hpProgress;

		private static SleekBoxIcon bleedingBox;

		private static SleekBoxIcon brokenBox;

		private static SleekBoxIcon temperatureBox;

		private static SleekBoxIcon starvedBox;

		private static SleekBoxIcon dehydratedBox;

		private static SleekBoxIcon infectedBox;

		private static SleekBoxIcon drownedBox;

		private static SleekBoxIcon moonBox;

		private static SleekBoxIcon radiationBox;

		private static SleekBoxIcon safeBox;

		private static SleekBoxIcon arrestBox;

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
		private static Damaged <>f__mg$cache8;

		[CompilerGenerated]
		private static HealthUpdated <>f__mg$cache9;

		[CompilerGenerated]
		private static FoodUpdated <>f__mg$cacheA;

		[CompilerGenerated]
		private static WaterUpdated <>f__mg$cacheB;

		[CompilerGenerated]
		private static VirusUpdated <>f__mg$cacheC;

		[CompilerGenerated]
		private static StaminaUpdated <>f__mg$cacheD;

		[CompilerGenerated]
		private static OxygenUpdated <>f__mg$cacheE;

		[CompilerGenerated]
		private static BleedingUpdated <>f__mg$cacheF;

		[CompilerGenerated]
		private static BrokenUpdated <>f__mg$cache10;

		[CompilerGenerated]
		private static TemperatureUpdated <>f__mg$cache11;

		[CompilerGenerated]
		private static PerspectiveUpdated <>f__mg$cache12;

		[CompilerGenerated]
		private static Seated <>f__mg$cache13;

		[CompilerGenerated]
		private static VehicleUpdated <>f__mg$cache14;

		[CompilerGenerated]
		private static SafetyUpdated <>f__mg$cache15;

		[CompilerGenerated]
		private static RadiationUpdated <>f__mg$cache16;

		[CompilerGenerated]
		private static GestureUpdated <>f__mg$cache17;

		[CompilerGenerated]
		private static Talked <>f__mg$cache18;

		[CompilerGenerated]
		private static TrackedQuestUpdated <>f__mg$cache19;

		[CompilerGenerated]
		private static ExperienceUpdated <>f__mg$cache1A;

		[CompilerGenerated]
		private static MoonUpdated <>f__mg$cache1B;

		[CompilerGenerated]
		private static WaveUpdated <>f__mg$cache1C;

		[CompilerGenerated]
		private static MaskUpdated <>f__mg$cache1D;

		[CompilerGenerated]
		private static Listed <>f__mg$cache1E;

		[CompilerGenerated]
		private static VotingStart <>f__mg$cache1F;

		[CompilerGenerated]
		private static VotingUpdate <>f__mg$cache20;

		[CompilerGenerated]
		private static VotingStop <>f__mg$cache21;

		[CompilerGenerated]
		private static VotingMessage <>f__mg$cache22;

		[CompilerGenerated]
		private static ArenaMessageUpdated <>f__mg$cache23;

		[CompilerGenerated]
		private static ArenaPlayerUpdated <>f__mg$cache24;

		[CompilerGenerated]
		private static LevelNumberUpdated <>f__mg$cache25;
	}
}
