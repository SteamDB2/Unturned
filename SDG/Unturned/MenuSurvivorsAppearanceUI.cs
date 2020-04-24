using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace SDG.Unturned
{
	public class MenuSurvivorsAppearanceUI
	{
		public MenuSurvivorsAppearanceUI()
		{
			MenuSurvivorsAppearanceUI.localization = Localization.read("/Menu/Survivors/MenuSurvivorsAppearance.dat");
			MenuSurvivorsAppearanceUI.container = new Sleek();
			MenuSurvivorsAppearanceUI.container.positionOffset_X = 10;
			MenuSurvivorsAppearanceUI.container.positionOffset_Y = 10;
			MenuSurvivorsAppearanceUI.container.positionScale_Y = 1f;
			MenuSurvivorsAppearanceUI.container.sizeOffset_X = -20;
			MenuSurvivorsAppearanceUI.container.sizeOffset_Y = -20;
			MenuSurvivorsAppearanceUI.container.sizeScale_X = 1f;
			MenuSurvivorsAppearanceUI.container.sizeScale_Y = 1f;
			MenuUI.container.add(MenuSurvivorsAppearanceUI.container);
			MenuSurvivorsAppearanceUI.active = false;
			MenuSurvivorsAppearanceUI.customizationBox = new SleekScrollBox();
			MenuSurvivorsAppearanceUI.customizationBox.positionOffset_X = -140;
			MenuSurvivorsAppearanceUI.customizationBox.positionOffset_Y = 100;
			MenuSurvivorsAppearanceUI.customizationBox.positionScale_X = 0.75f;
			MenuSurvivorsAppearanceUI.customizationBox.sizeOffset_X = 270;
			MenuSurvivorsAppearanceUI.customizationBox.sizeOffset_Y = -270;
			MenuSurvivorsAppearanceUI.customizationBox.sizeScale_Y = 1f;
			MenuSurvivorsAppearanceUI.container.add(MenuSurvivorsAppearanceUI.customizationBox);
			MenuSurvivorsAppearanceUI.faceBox = new SleekBox();
			MenuSurvivorsAppearanceUI.faceBox.sizeOffset_X = 240;
			MenuSurvivorsAppearanceUI.faceBox.sizeOffset_Y = 30;
			MenuSurvivorsAppearanceUI.faceBox.text = MenuSurvivorsAppearanceUI.localization.format("Face_Box");
			MenuSurvivorsAppearanceUI.faceBox.tooltip = MenuSurvivorsAppearanceUI.localization.format("Face_Box_Tooltip");
			MenuSurvivorsAppearanceUI.customizationBox.add(MenuSurvivorsAppearanceUI.faceBox);
			MenuSurvivorsAppearanceUI.faceButtons = new SleekButton[(int)(Customization.FACES_FREE + Customization.FACES_PRO)];
			for (int i = 0; i < MenuSurvivorsAppearanceUI.faceButtons.Length; i++)
			{
				SleekButton sleekButton = new SleekButton();
				sleekButton.positionOffset_X = i % 5 * 50;
				sleekButton.positionOffset_Y = 40 + Mathf.FloorToInt((float)i / 5f) * 50;
				sleekButton.sizeOffset_X = 40;
				sleekButton.sizeOffset_Y = 40;
				MenuSurvivorsAppearanceUI.faceBox.add(sleekButton);
				SleekImageTexture sleekImageTexture = new SleekImageTexture();
				sleekImageTexture.positionOffset_X = 10;
				sleekImageTexture.positionOffset_Y = 10;
				sleekImageTexture.sizeOffset_X = 20;
				sleekImageTexture.sizeOffset_Y = 20;
				sleekImageTexture.texture = (Texture2D)Resources.Load("Materials/Pixel");
				sleekButton.add(sleekImageTexture);
				sleekImageTexture.add(new SleekImageTexture
				{
					positionOffset_X = 2,
					positionOffset_Y = 2,
					sizeOffset_X = 16,
					sizeOffset_Y = 16,
					texture = (Texture2D)Resources.Load("Faces/" + i + "/Texture")
				});
				if (i >= (int)Customization.FACES_FREE)
				{
					if (Provider.isPro)
					{
						SleekButton sleekButton2 = sleekButton;
						if (MenuSurvivorsAppearanceUI.<>f__mg$cache0 == null)
						{
							MenuSurvivorsAppearanceUI.<>f__mg$cache0 = new ClickedButton(MenuSurvivorsAppearanceUI.onClickedFaceButton);
						}
						sleekButton2.onClickedButton = MenuSurvivorsAppearanceUI.<>f__mg$cache0;
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
					if (MenuSurvivorsAppearanceUI.<>f__mg$cache1 == null)
					{
						MenuSurvivorsAppearanceUI.<>f__mg$cache1 = new ClickedButton(MenuSurvivorsAppearanceUI.onClickedFaceButton);
					}
					sleekButton3.onClickedButton = MenuSurvivorsAppearanceUI.<>f__mg$cache1;
				}
				MenuSurvivorsAppearanceUI.faceButtons[i] = sleekButton;
			}
			MenuSurvivorsAppearanceUI.hairBox = new SleekBox();
			MenuSurvivorsAppearanceUI.hairBox.positionOffset_Y = 80 + Mathf.CeilToInt((float)MenuSurvivorsAppearanceUI.faceButtons.Length / 5f) * 50;
			MenuSurvivorsAppearanceUI.hairBox.sizeOffset_X = 240;
			MenuSurvivorsAppearanceUI.hairBox.sizeOffset_Y = 30;
			MenuSurvivorsAppearanceUI.hairBox.text = MenuSurvivorsAppearanceUI.localization.format("Hair_Box");
			MenuSurvivorsAppearanceUI.hairBox.tooltip = MenuSurvivorsAppearanceUI.localization.format("Hair_Box_Tooltip");
			MenuSurvivorsAppearanceUI.customizationBox.add(MenuSurvivorsAppearanceUI.hairBox);
			MenuSurvivorsAppearanceUI.hairButtons = new SleekButton[(int)(Customization.HAIRS_FREE + Customization.HAIRS_PRO)];
			for (int j = 0; j < MenuSurvivorsAppearanceUI.hairButtons.Length; j++)
			{
				SleekButton sleekButton4 = new SleekButton();
				sleekButton4.positionOffset_X = j % 5 * 50;
				sleekButton4.positionOffset_Y = 40 + Mathf.FloorToInt((float)j / 5f) * 50;
				sleekButton4.sizeOffset_X = 40;
				sleekButton4.sizeOffset_Y = 40;
				MenuSurvivorsAppearanceUI.hairBox.add(sleekButton4);
				sleekButton4.add(new SleekImageTexture
				{
					positionOffset_X = 10,
					positionOffset_Y = 10,
					sizeOffset_X = 20,
					sizeOffset_Y = 20,
					texture = (Texture2D)Resources.Load("Hairs/" + j + "/Texture")
				});
				if (j >= (int)Customization.HAIRS_FREE)
				{
					if (Provider.isPro)
					{
						SleekButton sleekButton5 = sleekButton4;
						if (MenuSurvivorsAppearanceUI.<>f__mg$cache2 == null)
						{
							MenuSurvivorsAppearanceUI.<>f__mg$cache2 = new ClickedButton(MenuSurvivorsAppearanceUI.onClickedHairButton);
						}
						sleekButton5.onClickedButton = MenuSurvivorsAppearanceUI.<>f__mg$cache2;
					}
					else
					{
						sleekButton4.backgroundColor = Palette.PRO;
						Bundle bundle2 = Bundles.getBundle("/Bundles/Textures/Menu/Icons/Pro/Pro.unity3d");
						sleekButton4.add(new SleekImageTexture
						{
							positionOffset_X = -10,
							positionOffset_Y = -10,
							positionScale_X = 0.5f,
							positionScale_Y = 0.5f,
							sizeOffset_X = 20,
							sizeOffset_Y = 20,
							texture = (Texture2D)bundle2.load("Lock_Small")
						});
						bundle2.unload();
					}
				}
				else
				{
					SleekButton sleekButton6 = sleekButton4;
					if (MenuSurvivorsAppearanceUI.<>f__mg$cache3 == null)
					{
						MenuSurvivorsAppearanceUI.<>f__mg$cache3 = new ClickedButton(MenuSurvivorsAppearanceUI.onClickedHairButton);
					}
					sleekButton6.onClickedButton = MenuSurvivorsAppearanceUI.<>f__mg$cache3;
				}
				MenuSurvivorsAppearanceUI.hairButtons[j] = sleekButton4;
			}
			MenuSurvivorsAppearanceUI.beardBox = new SleekBox();
			MenuSurvivorsAppearanceUI.beardBox.positionOffset_Y = 160 + Mathf.CeilToInt((float)MenuSurvivorsAppearanceUI.faceButtons.Length / 5f) * 50 + Mathf.CeilToInt((float)MenuSurvivorsAppearanceUI.hairButtons.Length / 5f) * 50;
			MenuSurvivorsAppearanceUI.beardBox.sizeOffset_X = 240;
			MenuSurvivorsAppearanceUI.beardBox.sizeOffset_Y = 30;
			MenuSurvivorsAppearanceUI.beardBox.text = MenuSurvivorsAppearanceUI.localization.format("Beard_Box");
			MenuSurvivorsAppearanceUI.beardBox.tooltip = MenuSurvivorsAppearanceUI.localization.format("Beard_Box_Tooltip");
			MenuSurvivorsAppearanceUI.customizationBox.add(MenuSurvivorsAppearanceUI.beardBox);
			MenuSurvivorsAppearanceUI.beardButtons = new SleekButton[(int)(Customization.BEARDS_FREE + Customization.BEARDS_PRO)];
			for (int k = 0; k < MenuSurvivorsAppearanceUI.beardButtons.Length; k++)
			{
				SleekButton sleekButton7 = new SleekButton();
				sleekButton7.positionOffset_X = k % 5 * 50;
				sleekButton7.positionOffset_Y = 40 + Mathf.FloorToInt((float)k / 5f) * 50;
				sleekButton7.sizeOffset_X = 40;
				sleekButton7.sizeOffset_Y = 40;
				MenuSurvivorsAppearanceUI.beardBox.add(sleekButton7);
				sleekButton7.add(new SleekImageTexture
				{
					positionOffset_X = 10,
					positionOffset_Y = 10,
					sizeOffset_X = 20,
					sizeOffset_Y = 20,
					texture = (Texture2D)Resources.Load("Beards/" + k + "/Texture")
				});
				if (k >= (int)Customization.BEARDS_FREE)
				{
					if (Provider.isPro)
					{
						SleekButton sleekButton8 = sleekButton7;
						if (MenuSurvivorsAppearanceUI.<>f__mg$cache4 == null)
						{
							MenuSurvivorsAppearanceUI.<>f__mg$cache4 = new ClickedButton(MenuSurvivorsAppearanceUI.onClickedBeardButton);
						}
						sleekButton8.onClickedButton = MenuSurvivorsAppearanceUI.<>f__mg$cache4;
					}
					else
					{
						sleekButton7.backgroundColor = Palette.PRO;
						Bundle bundle3 = Bundles.getBundle("/Bundles/Textures/Menu/Icons/Pro/Pro.unity3d");
						sleekButton7.add(new SleekImageTexture
						{
							positionOffset_X = -10,
							positionOffset_Y = -10,
							positionScale_X = 0.5f,
							positionScale_Y = 0.5f,
							sizeOffset_X = 20,
							sizeOffset_Y = 20,
							texture = (Texture2D)bundle3.load("Lock_Small")
						});
						bundle3.unload();
					}
				}
				else
				{
					SleekButton sleekButton9 = sleekButton7;
					if (MenuSurvivorsAppearanceUI.<>f__mg$cache5 == null)
					{
						MenuSurvivorsAppearanceUI.<>f__mg$cache5 = new ClickedButton(MenuSurvivorsAppearanceUI.onClickedBeardButton);
					}
					sleekButton9.onClickedButton = MenuSurvivorsAppearanceUI.<>f__mg$cache5;
				}
				MenuSurvivorsAppearanceUI.beardButtons[k] = sleekButton7;
			}
			MenuSurvivorsAppearanceUI.skinBox = new SleekBox();
			MenuSurvivorsAppearanceUI.skinBox.positionOffset_Y = 240 + Mathf.CeilToInt((float)MenuSurvivorsAppearanceUI.faceButtons.Length / 5f) * 50 + Mathf.CeilToInt((float)MenuSurvivorsAppearanceUI.hairButtons.Length / 5f) * 50 + Mathf.CeilToInt((float)MenuSurvivorsAppearanceUI.beardButtons.Length / 5f) * 50;
			MenuSurvivorsAppearanceUI.skinBox.sizeOffset_X = 240;
			MenuSurvivorsAppearanceUI.skinBox.sizeOffset_Y = 30;
			MenuSurvivorsAppearanceUI.skinBox.text = MenuSurvivorsAppearanceUI.localization.format("Skin_Box");
			MenuSurvivorsAppearanceUI.skinBox.tooltip = MenuSurvivorsAppearanceUI.localization.format("Skin_Box_Tooltip");
			MenuSurvivorsAppearanceUI.customizationBox.add(MenuSurvivorsAppearanceUI.skinBox);
			MenuSurvivorsAppearanceUI.skinButtons = new SleekButton[Customization.SKINS.Length];
			for (int l = 0; l < MenuSurvivorsAppearanceUI.skinButtons.Length; l++)
			{
				SleekButton sleekButton10 = new SleekButton();
				sleekButton10.positionOffset_X = l % 5 * 50;
				sleekButton10.positionOffset_Y = 40 + Mathf.FloorToInt((float)l / 5f) * 50;
				sleekButton10.sizeOffset_X = 40;
				sleekButton10.sizeOffset_Y = 40;
				SleekButton sleekButton11 = sleekButton10;
				if (MenuSurvivorsAppearanceUI.<>f__mg$cache6 == null)
				{
					MenuSurvivorsAppearanceUI.<>f__mg$cache6 = new ClickedButton(MenuSurvivorsAppearanceUI.onClickedSkinButton);
				}
				sleekButton11.onClickedButton = MenuSurvivorsAppearanceUI.<>f__mg$cache6;
				MenuSurvivorsAppearanceUI.skinBox.add(sleekButton10);
				sleekButton10.add(new SleekImageTexture
				{
					positionOffset_X = 10,
					positionOffset_Y = 10,
					sizeOffset_X = 20,
					sizeOffset_Y = 20,
					texture = (Texture2D)Resources.Load("Materials/Pixel"),
					backgroundColor = Customization.SKINS[l]
				});
				MenuSurvivorsAppearanceUI.skinButtons[l] = sleekButton10;
			}
			MenuSurvivorsAppearanceUI.skinColorPicker = new SleekColorPicker();
			MenuSurvivorsAppearanceUI.skinColorPicker.positionOffset_Y = 280 + Mathf.CeilToInt((float)MenuSurvivorsAppearanceUI.faceButtons.Length / 5f) * 50 + Mathf.CeilToInt((float)MenuSurvivorsAppearanceUI.hairButtons.Length / 5f) * 50 + Mathf.CeilToInt((float)MenuSurvivorsAppearanceUI.beardButtons.Length / 5f) * 50 + Mathf.CeilToInt((float)MenuSurvivorsAppearanceUI.skinButtons.Length / 5f) * 50;
			MenuSurvivorsAppearanceUI.customizationBox.add(MenuSurvivorsAppearanceUI.skinColorPicker);
			if (Provider.isPro)
			{
				SleekColorPicker sleekColorPicker = MenuSurvivorsAppearanceUI.skinColorPicker;
				if (MenuSurvivorsAppearanceUI.<>f__mg$cache7 == null)
				{
					MenuSurvivorsAppearanceUI.<>f__mg$cache7 = new ColorPicked(MenuSurvivorsAppearanceUI.onSkinColorPicked);
				}
				sleekColorPicker.onColorPicked = MenuSurvivorsAppearanceUI.<>f__mg$cache7;
			}
			else
			{
				Bundle bundle4 = Bundles.getBundle("/Bundles/Textures/Menu/Icons/Pro/Pro.unity3d");
				SleekImageTexture sleekImageTexture2 = new SleekImageTexture();
				sleekImageTexture2.positionOffset_X = -40;
				sleekImageTexture2.positionOffset_Y = -40;
				sleekImageTexture2.positionScale_X = 0.5f;
				sleekImageTexture2.positionScale_Y = 0.5f;
				sleekImageTexture2.sizeOffset_X = 80;
				sleekImageTexture2.sizeOffset_Y = 80;
				sleekImageTexture2.texture = (Texture2D)bundle4.load("Lock_Large");
				MenuSurvivorsAppearanceUI.skinColorPicker.add(sleekImageTexture2);
				bundle4.unload();
			}
			MenuSurvivorsAppearanceUI.colorBox = new SleekBox();
			MenuSurvivorsAppearanceUI.colorBox.positionOffset_Y = 440 + Mathf.CeilToInt((float)MenuSurvivorsAppearanceUI.faceButtons.Length / 5f) * 50 + Mathf.CeilToInt((float)MenuSurvivorsAppearanceUI.hairButtons.Length / 5f) * 50 + Mathf.CeilToInt((float)MenuSurvivorsAppearanceUI.beardButtons.Length / 5f) * 50 + Mathf.CeilToInt((float)MenuSurvivorsAppearanceUI.skinButtons.Length / 5f) * 50;
			MenuSurvivorsAppearanceUI.colorBox.sizeOffset_X = 240;
			MenuSurvivorsAppearanceUI.colorBox.sizeOffset_Y = 30;
			MenuSurvivorsAppearanceUI.colorBox.text = MenuSurvivorsAppearanceUI.localization.format("Color_Box");
			MenuSurvivorsAppearanceUI.colorBox.tooltip = MenuSurvivorsAppearanceUI.localization.format("Color_Box_Tooltip");
			MenuSurvivorsAppearanceUI.customizationBox.add(MenuSurvivorsAppearanceUI.colorBox);
			MenuSurvivorsAppearanceUI.colorButtons = new SleekButton[Customization.COLORS.Length];
			for (int m = 0; m < MenuSurvivorsAppearanceUI.colorButtons.Length; m++)
			{
				SleekButton sleekButton12 = new SleekButton();
				sleekButton12.positionOffset_X = m % 5 * 50;
				sleekButton12.positionOffset_Y = 40 + Mathf.FloorToInt((float)m / 5f) * 50;
				sleekButton12.sizeOffset_X = 40;
				sleekButton12.sizeOffset_Y = 40;
				SleekButton sleekButton13 = sleekButton12;
				if (MenuSurvivorsAppearanceUI.<>f__mg$cache8 == null)
				{
					MenuSurvivorsAppearanceUI.<>f__mg$cache8 = new ClickedButton(MenuSurvivorsAppearanceUI.onClickedColorButton);
				}
				sleekButton13.onClickedButton = MenuSurvivorsAppearanceUI.<>f__mg$cache8;
				MenuSurvivorsAppearanceUI.colorBox.add(sleekButton12);
				sleekButton12.add(new SleekImageTexture
				{
					positionOffset_X = 10,
					positionOffset_Y = 10,
					sizeOffset_X = 20,
					sizeOffset_Y = 20,
					texture = (Texture2D)Resources.Load("Materials/Pixel"),
					backgroundColor = Customization.COLORS[m]
				});
				MenuSurvivorsAppearanceUI.colorButtons[m] = sleekButton12;
			}
			MenuSurvivorsAppearanceUI.colorColorPicker = new SleekColorPicker();
			MenuSurvivorsAppearanceUI.colorColorPicker.positionOffset_Y = 480 + Mathf.CeilToInt((float)MenuSurvivorsAppearanceUI.faceButtons.Length / 5f) * 50 + Mathf.CeilToInt((float)MenuSurvivorsAppearanceUI.hairButtons.Length / 5f) * 50 + Mathf.CeilToInt((float)MenuSurvivorsAppearanceUI.beardButtons.Length / 5f) * 50 + Mathf.CeilToInt((float)MenuSurvivorsAppearanceUI.skinButtons.Length / 5f) * 50 + Mathf.CeilToInt((float)MenuSurvivorsAppearanceUI.colorButtons.Length / 5f) * 50;
			MenuSurvivorsAppearanceUI.customizationBox.add(MenuSurvivorsAppearanceUI.colorColorPicker);
			if (Provider.isPro)
			{
				SleekColorPicker sleekColorPicker2 = MenuSurvivorsAppearanceUI.colorColorPicker;
				if (MenuSurvivorsAppearanceUI.<>f__mg$cache9 == null)
				{
					MenuSurvivorsAppearanceUI.<>f__mg$cache9 = new ColorPicked(MenuSurvivorsAppearanceUI.onColorColorPicked);
				}
				sleekColorPicker2.onColorPicked = MenuSurvivorsAppearanceUI.<>f__mg$cache9;
			}
			else
			{
				Bundle bundle5 = Bundles.getBundle("/Bundles/Textures/Menu/Icons/Pro/Pro.unity3d");
				SleekImageTexture sleekImageTexture3 = new SleekImageTexture();
				sleekImageTexture3.positionOffset_X = -40;
				sleekImageTexture3.positionOffset_Y = -40;
				sleekImageTexture3.positionScale_X = 0.5f;
				sleekImageTexture3.positionScale_Y = 0.5f;
				sleekImageTexture3.sizeOffset_X = 80;
				sleekImageTexture3.sizeOffset_Y = 80;
				sleekImageTexture3.texture = (Texture2D)bundle5.load("Lock_Large");
				MenuSurvivorsAppearanceUI.colorColorPicker.add(sleekImageTexture3);
				bundle5.unload();
			}
			MenuSurvivorsAppearanceUI.customizationBox.area = new Rect(0f, 0f, 5f, (float)(600 + Mathf.CeilToInt((float)MenuSurvivorsAppearanceUI.faceButtons.Length / 5f) * 50 + Mathf.CeilToInt((float)MenuSurvivorsAppearanceUI.hairButtons.Length / 5f) * 50 + Mathf.CeilToInt((float)MenuSurvivorsAppearanceUI.beardButtons.Length / 5f) * 50 + Mathf.CeilToInt((float)MenuSurvivorsAppearanceUI.skinButtons.Length / 5f) * 50 + Mathf.CeilToInt((float)MenuSurvivorsAppearanceUI.colorButtons.Length / 5f) * 50));
			Delegate onCharacterUpdated = Characters.onCharacterUpdated;
			if (MenuSurvivorsAppearanceUI.<>f__mg$cacheA == null)
			{
				MenuSurvivorsAppearanceUI.<>f__mg$cacheA = new CharacterUpdated(MenuSurvivorsAppearanceUI.onCharacterUpdated);
			}
			Characters.onCharacterUpdated = (CharacterUpdated)Delegate.Combine(onCharacterUpdated, MenuSurvivorsAppearanceUI.<>f__mg$cacheA);
			MenuSurvivorsAppearanceUI.handState = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(MenuSurvivorsAppearanceUI.localization.format("Right")),
				new GUIContent(MenuSurvivorsAppearanceUI.localization.format("Left"))
			});
			MenuSurvivorsAppearanceUI.handState.positionOffset_X = -140;
			MenuSurvivorsAppearanceUI.handState.positionOffset_Y = -160;
			MenuSurvivorsAppearanceUI.handState.positionScale_X = 0.75f;
			MenuSurvivorsAppearanceUI.handState.positionScale_Y = 1f;
			MenuSurvivorsAppearanceUI.handState.sizeOffset_X = 240;
			MenuSurvivorsAppearanceUI.handState.sizeOffset_Y = 30;
			SleekButtonState sleekButtonState = MenuSurvivorsAppearanceUI.handState;
			if (MenuSurvivorsAppearanceUI.<>f__mg$cacheB == null)
			{
				MenuSurvivorsAppearanceUI.<>f__mg$cacheB = new SwappedState(MenuSurvivorsAppearanceUI.onSwappedHandState);
			}
			sleekButtonState.onSwappedState = MenuSurvivorsAppearanceUI.<>f__mg$cacheB;
			MenuSurvivorsAppearanceUI.container.add(MenuSurvivorsAppearanceUI.handState);
			MenuSurvivorsAppearanceUI.characterSlider = new SleekSlider();
			MenuSurvivorsAppearanceUI.characterSlider.positionOffset_X = -140;
			MenuSurvivorsAppearanceUI.characterSlider.positionOffset_Y = -120;
			MenuSurvivorsAppearanceUI.characterSlider.positionScale_X = 0.75f;
			MenuSurvivorsAppearanceUI.characterSlider.positionScale_Y = 1f;
			MenuSurvivorsAppearanceUI.characterSlider.sizeOffset_X = 240;
			MenuSurvivorsAppearanceUI.characterSlider.sizeOffset_Y = 20;
			MenuSurvivorsAppearanceUI.characterSlider.orientation = ESleekOrientation.HORIZONTAL;
			SleekSlider sleekSlider = MenuSurvivorsAppearanceUI.characterSlider;
			if (MenuSurvivorsAppearanceUI.<>f__mg$cacheC == null)
			{
				MenuSurvivorsAppearanceUI.<>f__mg$cacheC = new Dragged(MenuSurvivorsAppearanceUI.onDraggedCharacterSlider);
			}
			sleekSlider.onDragged = MenuSurvivorsAppearanceUI.<>f__mg$cacheC;
			MenuSurvivorsAppearanceUI.container.add(MenuSurvivorsAppearanceUI.characterSlider);
			MenuSurvivorsAppearanceUI.backButton = new SleekButtonIcon((Texture2D)MenuDashboardUI.icons.load("Exit"));
			MenuSurvivorsAppearanceUI.backButton.positionOffset_Y = -50;
			MenuSurvivorsAppearanceUI.backButton.positionScale_Y = 1f;
			MenuSurvivorsAppearanceUI.backButton.sizeOffset_X = 200;
			MenuSurvivorsAppearanceUI.backButton.sizeOffset_Y = 50;
			MenuSurvivorsAppearanceUI.backButton.text = MenuDashboardUI.localization.format("BackButtonText");
			MenuSurvivorsAppearanceUI.backButton.tooltip = MenuDashboardUI.localization.format("BackButtonTooltip");
			SleekButton sleekButton14 = MenuSurvivorsAppearanceUI.backButton;
			if (MenuSurvivorsAppearanceUI.<>f__mg$cacheD == null)
			{
				MenuSurvivorsAppearanceUI.<>f__mg$cacheD = new ClickedButton(MenuSurvivorsAppearanceUI.onClickedBackButton);
			}
			sleekButton14.onClickedButton = MenuSurvivorsAppearanceUI.<>f__mg$cacheD;
			MenuSurvivorsAppearanceUI.backButton.fontSize = 14;
			MenuSurvivorsAppearanceUI.backButton.iconImage.backgroundTint = ESleekTint.FOREGROUND;
			MenuSurvivorsAppearanceUI.container.add(MenuSurvivorsAppearanceUI.backButton);
		}

		public static void open()
		{
			if (MenuSurvivorsAppearanceUI.active)
			{
				return;
			}
			MenuSurvivorsAppearanceUI.active = true;
			Characters.apply(false, false);
			MenuSurvivorsAppearanceUI.container.lerpPositionScale(0f, 0f, ESleekLerp.EXPONENTIAL, 20f);
		}

		public static void close()
		{
			if (!MenuSurvivorsAppearanceUI.active)
			{
				return;
			}
			MenuSurvivorsAppearanceUI.active = false;
			Characters.apply(true, true);
			MenuSurvivorsAppearanceUI.container.lerpPositionScale(0f, 1f, ESleekLerp.EXPONENTIAL, 20f);
		}

		private static void updateFaces(Color color)
		{
			for (int i = 0; i < MenuSurvivorsAppearanceUI.faceButtons.Length; i++)
			{
				MenuSurvivorsAppearanceUI.faceButtons[i].children[0].backgroundColor = color;
			}
		}

		private static void updateColors(Color color)
		{
			for (int i = 1; i < MenuSurvivorsAppearanceUI.hairButtons.Length; i++)
			{
				MenuSurvivorsAppearanceUI.hairButtons[i].children[0].backgroundColor = color;
			}
			for (int j = 1; j < MenuSurvivorsAppearanceUI.beardButtons.Length; j++)
			{
				MenuSurvivorsAppearanceUI.beardButtons[j].children[0].backgroundColor = color;
			}
		}

		private static void onCharacterUpdated(byte index, Character character)
		{
			if (index == Characters.selected)
			{
				MenuSurvivorsAppearanceUI.skinColorPicker.state = character.skin;
				MenuSurvivorsAppearanceUI.colorColorPicker.state = character.color;
				MenuSurvivorsAppearanceUI.updateFaces(character.skin);
				MenuSurvivorsAppearanceUI.updateColors(character.color);
				MenuSurvivorsAppearanceUI.handState.state = ((!character.hand) ? 0 : 1);
			}
		}

		private static void onClickedFaceButton(SleekButton button)
		{
			int num = button.positionOffset_X / 50 + (button.positionOffset_Y - 40) / 50 * 5;
			Characters.growFace((byte)num);
		}

		private static void onClickedHairButton(SleekButton button)
		{
			int num = button.positionOffset_X / 50 + (button.positionOffset_Y - 40) / 50 * 5;
			Characters.growHair((byte)num);
		}

		private static void onClickedBeardButton(SleekButton button)
		{
			int num = button.positionOffset_X / 50 + (button.positionOffset_Y - 40) / 50 * 5;
			Characters.growBeard((byte)num);
		}

		private static void onClickedSkinButton(SleekButton button)
		{
			int num = button.positionOffset_X / 50 + (button.positionOffset_Y - 40) / 50 * 5;
			Color color = Customization.SKINS[num];
			Characters.paintSkin(color);
			MenuSurvivorsAppearanceUI.skinColorPicker.state = color;
			MenuSurvivorsAppearanceUI.updateFaces(color);
		}

		private static void onSkinColorPicked(SleekColorPicker picker, Color color)
		{
			Characters.paintSkin(color);
			MenuSurvivorsAppearanceUI.updateFaces(color);
		}

		private static void onClickedColorButton(SleekButton button)
		{
			int num = button.positionOffset_X / 50 + (button.positionOffset_Y - 40) / 50 * 5;
			Color color = Customization.COLORS[num];
			Characters.paintColor(color);
			MenuSurvivorsAppearanceUI.colorColorPicker.state = color;
			MenuSurvivorsAppearanceUI.updateColors(color);
		}

		private static void onColorColorPicked(SleekColorPicker picker, Color color)
		{
			Characters.paintColor(color);
			MenuSurvivorsAppearanceUI.updateColors(color);
		}

		private static void onSwappedHandState(SleekButtonState button, int index)
		{
			Characters.hand(index != 0);
		}

		private static void onDraggedCharacterSlider(SleekSlider slider, float state)
		{
			Characters.characterYaw = state * 360f;
		}

		private static void onClickedBackButton(SleekButton button)
		{
			MenuSurvivorsUI.open();
			MenuSurvivorsAppearanceUI.close();
		}

		private static Local localization;

		private static Sleek container;

		public static bool active;

		private static SleekButtonIcon backButton;

		private static SleekScrollBox customizationBox;

		private static SleekBox faceBox;

		private static SleekBox hairBox;

		private static SleekBox beardBox;

		private static SleekButton[] faceButtons;

		private static SleekButton[] hairButtons;

		private static SleekButton[] beardButtons;

		private static SleekBox skinBox;

		private static SleekBox colorBox;

		private static SleekButton[] skinButtons;

		private static SleekButton[] colorButtons;

		private static SleekColorPicker skinColorPicker;

		private static SleekColorPicker colorColorPicker;

		private static SleekButtonState handState;

		private static SleekSlider characterSlider;

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
		private static ColorPicked <>f__mg$cache7;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache8;

		[CompilerGenerated]
		private static ColorPicked <>f__mg$cache9;

		[CompilerGenerated]
		private static CharacterUpdated <>f__mg$cacheA;

		[CompilerGenerated]
		private static SwappedState <>f__mg$cacheB;

		[CompilerGenerated]
		private static Dragged <>f__mg$cacheC;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cacheD;
	}
}
