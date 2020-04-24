using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace SDG.Unturned
{
	public class EditorTerrainDetailsUI
	{
		public EditorTerrainDetailsUI()
		{
			Local local = Localization.read("/Editor/EditorTerrainDetails.dat");
			Bundle bundle = Bundles.getBundle("/Bundles/Textures/Edit/Icons/EditorTerrainDetails/EditorTerrainDetails.unity3d");
			EditorTerrainDetailsUI.container = new Sleek();
			EditorTerrainDetailsUI.container.positionOffset_X = 10;
			EditorTerrainDetailsUI.container.positionOffset_Y = 10;
			EditorTerrainDetailsUI.container.positionScale_X = 1f;
			EditorTerrainDetailsUI.container.sizeOffset_X = -20;
			EditorTerrainDetailsUI.container.sizeOffset_Y = -20;
			EditorTerrainDetailsUI.container.sizeScale_X = 1f;
			EditorTerrainDetailsUI.container.sizeScale_Y = 1f;
			EditorUI.window.add(EditorTerrainDetailsUI.container);
			EditorTerrainDetailsUI.active = false;
			if (LevelGround.details == null)
			{
				return;
			}
			EditorTerrainDetailsUI.detailsScrollBox = new SleekScrollBox();
			EditorTerrainDetailsUI.detailsScrollBox.positionOffset_Y = 120;
			EditorTerrainDetailsUI.detailsScrollBox.positionOffset_X = -400;
			EditorTerrainDetailsUI.detailsScrollBox.positionScale_X = 1f;
			EditorTerrainDetailsUI.detailsScrollBox.sizeOffset_X = 400;
			EditorTerrainDetailsUI.detailsScrollBox.sizeOffset_Y = -160;
			EditorTerrainDetailsUI.detailsScrollBox.sizeScale_Y = 1f;
			EditorTerrainDetailsUI.detailsScrollBox.area = new Rect(0f, 0f, 5f, (float)(LevelGround.details.Length * 70 + 400));
			EditorTerrainDetailsUI.container.add(EditorTerrainDetailsUI.detailsScrollBox);
			for (int i = 0; i < LevelGround.details.Length; i++)
			{
				GroundDetail groundDetail = LevelGround.details[i];
				SleekImageTexture sleekImageTexture = new SleekImageTexture();
				sleekImageTexture.positionOffset_X = 200;
				sleekImageTexture.positionOffset_Y = i * 70;
				sleekImageTexture.sizeOffset_X = 64;
				sleekImageTexture.sizeOffset_Y = 64;
				sleekImageTexture.texture = groundDetail.prototype.prototypeTexture;
				EditorTerrainDetailsUI.detailsScrollBox.add(sleekImageTexture);
				SleekButton sleekButton = new SleekButton();
				sleekButton.sizeOffset_Y = 64;
				if (groundDetail.prototype.prototypeTexture != null)
				{
					sleekButton.positionOffset_X = 70;
					sleekButton.sizeOffset_X = 100;
					sleekButton.text = LevelGround.details[i].prototype.prototypeTexture.name;
				}
				else
				{
					sleekButton.sizeOffset_X = 170;
					sleekButton.text = LevelGround.details[i].prototype.prototype.name;
				}
				SleekButton sleekButton2 = sleekButton;
				if (EditorTerrainDetailsUI.<>f__mg$cache0 == null)
				{
					EditorTerrainDetailsUI.<>f__mg$cache0 = new ClickedButton(EditorTerrainDetailsUI.onClickedDetailButton);
				}
				sleekButton2.onClickedButton = EditorTerrainDetailsUI.<>f__mg$cache0;
				sleekImageTexture.add(sleekButton);
			}
			EditorTerrainDetailsUI.densitySlider = new SleekSlider();
			EditorTerrainDetailsUI.densitySlider.positionOffset_X = 200;
			EditorTerrainDetailsUI.densitySlider.positionOffset_Y = LevelGround.details.Length * 70;
			EditorTerrainDetailsUI.densitySlider.sizeOffset_X = 170;
			EditorTerrainDetailsUI.densitySlider.sizeOffset_Y = 20;
			EditorTerrainDetailsUI.densitySlider.orientation = ESleekOrientation.HORIZONTAL;
			EditorTerrainDetailsUI.densitySlider.addLabel(local.format("DensitySliderLabelText"), ESleekSide.LEFT);
			SleekSlider sleekSlider = EditorTerrainDetailsUI.densitySlider;
			if (EditorTerrainDetailsUI.<>f__mg$cache1 == null)
			{
				EditorTerrainDetailsUI.<>f__mg$cache1 = new Dragged(EditorTerrainDetailsUI.onDraggedDensitySlider);
			}
			sleekSlider.onDragged = EditorTerrainDetailsUI.<>f__mg$cache1;
			EditorTerrainDetailsUI.detailsScrollBox.add(EditorTerrainDetailsUI.densitySlider);
			EditorTerrainDetailsUI.chanceSlider = new SleekSlider();
			EditorTerrainDetailsUI.chanceSlider.positionOffset_X = 200;
			EditorTerrainDetailsUI.chanceSlider.positionOffset_Y = LevelGround.details.Length * 70 + 30;
			EditorTerrainDetailsUI.chanceSlider.sizeOffset_X = 170;
			EditorTerrainDetailsUI.chanceSlider.sizeOffset_Y = 20;
			EditorTerrainDetailsUI.chanceSlider.orientation = ESleekOrientation.HORIZONTAL;
			EditorTerrainDetailsUI.chanceSlider.addLabel(local.format("ChanceSliderLabelText"), ESleekSide.LEFT);
			SleekSlider sleekSlider2 = EditorTerrainDetailsUI.chanceSlider;
			if (EditorTerrainDetailsUI.<>f__mg$cache2 == null)
			{
				EditorTerrainDetailsUI.<>f__mg$cache2 = new Dragged(EditorTerrainDetailsUI.onDraggedChanceSlider);
			}
			sleekSlider2.onDragged = EditorTerrainDetailsUI.<>f__mg$cache2;
			EditorTerrainDetailsUI.detailsScrollBox.add(EditorTerrainDetailsUI.chanceSlider);
			EditorTerrainDetailsUI.grass_0_Toggle = new SleekToggle();
			EditorTerrainDetailsUI.grass_0_Toggle.positionOffset_X = 200;
			EditorTerrainDetailsUI.grass_0_Toggle.positionOffset_Y = LevelGround.details.Length * 70 + 60;
			EditorTerrainDetailsUI.grass_0_Toggle.sizeOffset_X = 40;
			EditorTerrainDetailsUI.grass_0_Toggle.sizeOffset_Y = 40;
			EditorTerrainDetailsUI.grass_0_Toggle.addLabel(local.format("Grass_0_ToggleLabelText"), ESleekSide.RIGHT);
			SleekToggle sleekToggle = EditorTerrainDetailsUI.grass_0_Toggle;
			if (EditorTerrainDetailsUI.<>f__mg$cache3 == null)
			{
				EditorTerrainDetailsUI.<>f__mg$cache3 = new Toggled(EditorTerrainDetailsUI.onToggledGrass_0_Toggle);
			}
			sleekToggle.onToggled = EditorTerrainDetailsUI.<>f__mg$cache3;
			EditorTerrainDetailsUI.detailsScrollBox.add(EditorTerrainDetailsUI.grass_0_Toggle);
			EditorTerrainDetailsUI.grass_1_Toggle = new SleekToggle();
			EditorTerrainDetailsUI.grass_1_Toggle.positionOffset_X = 200;
			EditorTerrainDetailsUI.grass_1_Toggle.positionOffset_Y = LevelGround.details.Length * 70 + 110;
			EditorTerrainDetailsUI.grass_1_Toggle.sizeOffset_X = 40;
			EditorTerrainDetailsUI.grass_1_Toggle.sizeOffset_Y = 40;
			EditorTerrainDetailsUI.grass_1_Toggle.addLabel(local.format("Grass_1_ToggleLabelText"), ESleekSide.RIGHT);
			SleekToggle sleekToggle2 = EditorTerrainDetailsUI.grass_1_Toggle;
			if (EditorTerrainDetailsUI.<>f__mg$cache4 == null)
			{
				EditorTerrainDetailsUI.<>f__mg$cache4 = new Toggled(EditorTerrainDetailsUI.onToggledGrass_1_Toggle);
			}
			sleekToggle2.onToggled = EditorTerrainDetailsUI.<>f__mg$cache4;
			EditorTerrainDetailsUI.detailsScrollBox.add(EditorTerrainDetailsUI.grass_1_Toggle);
			EditorTerrainDetailsUI.flower_0_Toggle = new SleekToggle();
			EditorTerrainDetailsUI.flower_0_Toggle.positionOffset_X = 200;
			EditorTerrainDetailsUI.flower_0_Toggle.positionOffset_Y = LevelGround.details.Length * 70 + 160;
			EditorTerrainDetailsUI.flower_0_Toggle.sizeOffset_X = 40;
			EditorTerrainDetailsUI.flower_0_Toggle.sizeOffset_Y = 40;
			EditorTerrainDetailsUI.flower_0_Toggle.addLabel(local.format("Flower_0_ToggleLabelText"), ESleekSide.RIGHT);
			SleekToggle sleekToggle3 = EditorTerrainDetailsUI.flower_0_Toggle;
			if (EditorTerrainDetailsUI.<>f__mg$cache5 == null)
			{
				EditorTerrainDetailsUI.<>f__mg$cache5 = new Toggled(EditorTerrainDetailsUI.onToggledFlower_0_Toggle);
			}
			sleekToggle3.onToggled = EditorTerrainDetailsUI.<>f__mg$cache5;
			EditorTerrainDetailsUI.detailsScrollBox.add(EditorTerrainDetailsUI.flower_0_Toggle);
			EditorTerrainDetailsUI.flower_1_Toggle = new SleekToggle();
			EditorTerrainDetailsUI.flower_1_Toggle.positionOffset_X = 200;
			EditorTerrainDetailsUI.flower_1_Toggle.positionOffset_Y = LevelGround.details.Length * 70 + 210;
			EditorTerrainDetailsUI.flower_1_Toggle.sizeOffset_X = 40;
			EditorTerrainDetailsUI.flower_1_Toggle.sizeOffset_Y = 40;
			EditorTerrainDetailsUI.flower_1_Toggle.addLabel(local.format("Flower_0_ToggleLabelText"), ESleekSide.RIGHT);
			SleekToggle sleekToggle4 = EditorTerrainDetailsUI.flower_1_Toggle;
			if (EditorTerrainDetailsUI.<>f__mg$cache6 == null)
			{
				EditorTerrainDetailsUI.<>f__mg$cache6 = new Toggled(EditorTerrainDetailsUI.onToggledFlower_1_Toggle);
			}
			sleekToggle4.onToggled = EditorTerrainDetailsUI.<>f__mg$cache6;
			EditorTerrainDetailsUI.detailsScrollBox.add(EditorTerrainDetailsUI.flower_1_Toggle);
			EditorTerrainDetailsUI.rockToggle = new SleekToggle();
			EditorTerrainDetailsUI.rockToggle.positionOffset_X = 200;
			EditorTerrainDetailsUI.rockToggle.positionOffset_Y = LevelGround.details.Length * 70 + 260;
			EditorTerrainDetailsUI.rockToggle.sizeOffset_X = 40;
			EditorTerrainDetailsUI.rockToggle.sizeOffset_Y = 40;
			EditorTerrainDetailsUI.rockToggle.addLabel(local.format("RockToggleLabelText"), ESleekSide.RIGHT);
			SleekToggle sleekToggle5 = EditorTerrainDetailsUI.rockToggle;
			if (EditorTerrainDetailsUI.<>f__mg$cache7 == null)
			{
				EditorTerrainDetailsUI.<>f__mg$cache7 = new Toggled(EditorTerrainDetailsUI.onToggledRockToggle);
			}
			sleekToggle5.onToggled = EditorTerrainDetailsUI.<>f__mg$cache7;
			EditorTerrainDetailsUI.detailsScrollBox.add(EditorTerrainDetailsUI.rockToggle);
			EditorTerrainDetailsUI.roadToggle = new SleekToggle();
			EditorTerrainDetailsUI.roadToggle.positionOffset_X = 200;
			EditorTerrainDetailsUI.roadToggle.positionOffset_Y = LevelGround.details.Length * 70 + 310;
			EditorTerrainDetailsUI.roadToggle.sizeOffset_X = 40;
			EditorTerrainDetailsUI.roadToggle.sizeOffset_Y = 40;
			EditorTerrainDetailsUI.roadToggle.addLabel(local.format("RoadToggleLabelText"), ESleekSide.RIGHT);
			SleekToggle sleekToggle6 = EditorTerrainDetailsUI.roadToggle;
			if (EditorTerrainDetailsUI.<>f__mg$cache8 == null)
			{
				EditorTerrainDetailsUI.<>f__mg$cache8 = new Toggled(EditorTerrainDetailsUI.onToggledRoadToggle);
			}
			sleekToggle6.onToggled = EditorTerrainDetailsUI.<>f__mg$cache8;
			EditorTerrainDetailsUI.detailsScrollBox.add(EditorTerrainDetailsUI.roadToggle);
			EditorTerrainDetailsUI.snowToggle = new SleekToggle();
			EditorTerrainDetailsUI.snowToggle.positionOffset_X = 200;
			EditorTerrainDetailsUI.snowToggle.positionOffset_Y = LevelGround.details.Length * 70 + 360;
			EditorTerrainDetailsUI.snowToggle.sizeOffset_X = 40;
			EditorTerrainDetailsUI.snowToggle.sizeOffset_Y = 40;
			EditorTerrainDetailsUI.snowToggle.addLabel(local.format("SnowToggleLabelText"), ESleekSide.RIGHT);
			SleekToggle sleekToggle7 = EditorTerrainDetailsUI.snowToggle;
			if (EditorTerrainDetailsUI.<>f__mg$cache9 == null)
			{
				EditorTerrainDetailsUI.<>f__mg$cache9 = new Toggled(EditorTerrainDetailsUI.onToggledSnowToggle);
			}
			sleekToggle7.onToggled = EditorTerrainDetailsUI.<>f__mg$cache9;
			EditorTerrainDetailsUI.detailsScrollBox.add(EditorTerrainDetailsUI.snowToggle);
			EditorTerrainDetailsUI.selectedBox = new SleekBox();
			EditorTerrainDetailsUI.selectedBox.positionOffset_X = -200;
			EditorTerrainDetailsUI.selectedBox.positionOffset_Y = 80;
			EditorTerrainDetailsUI.selectedBox.positionScale_X = 1f;
			EditorTerrainDetailsUI.selectedBox.sizeOffset_X = 200;
			EditorTerrainDetailsUI.selectedBox.sizeOffset_Y = 30;
			EditorTerrainDetailsUI.selectedBox.addLabel(local.format("SelectionBoxLabelText"), ESleekSide.LEFT);
			EditorTerrainDetailsUI.container.add(EditorTerrainDetailsUI.selectedBox);
			EditorTerrainDetailsUI.updateSelection();
			EditorTerrainDetailsUI.bakeDetailsButton = new SleekButtonIcon((Texture2D)bundle.load("Details"));
			EditorTerrainDetailsUI.bakeDetailsButton.positionOffset_X = -200;
			EditorTerrainDetailsUI.bakeDetailsButton.positionOffset_Y = -30;
			EditorTerrainDetailsUI.bakeDetailsButton.positionScale_X = 1f;
			EditorTerrainDetailsUI.bakeDetailsButton.positionScale_Y = 1f;
			EditorTerrainDetailsUI.bakeDetailsButton.sizeOffset_X = 200;
			EditorTerrainDetailsUI.bakeDetailsButton.sizeOffset_Y = 30;
			EditorTerrainDetailsUI.bakeDetailsButton.text = local.format("BakeDetailsButtonText");
			EditorTerrainDetailsUI.bakeDetailsButton.tooltip = local.format("BakeDetailsButtonTooltip");
			SleekButton sleekButton3 = EditorTerrainDetailsUI.bakeDetailsButton;
			if (EditorTerrainDetailsUI.<>f__mg$cacheA == null)
			{
				EditorTerrainDetailsUI.<>f__mg$cacheA = new ClickedButton(EditorTerrainDetailsUI.onClickedBakeDetailsButton);
			}
			sleekButton3.onClickedButton = EditorTerrainDetailsUI.<>f__mg$cacheA;
			EditorTerrainDetailsUI.container.add(EditorTerrainDetailsUI.bakeDetailsButton);
			bundle.unload();
		}

		public static void open()
		{
			if (EditorTerrainDetailsUI.active)
			{
				return;
			}
			EditorTerrainDetailsUI.active = true;
			if (LevelGround.materials == null)
			{
				return;
			}
			EditorTerrainDetailsUI.container.lerpPositionScale(0f, 0f, ESleekLerp.EXPONENTIAL, 20f);
		}

		public static void close()
		{
			if (!EditorTerrainDetailsUI.active)
			{
				return;
			}
			EditorTerrainDetailsUI.active = false;
			EditorTerrainDetailsUI.container.lerpPositionScale(1f, 0f, ESleekLerp.EXPONENTIAL, 20f);
		}

		private static void updateSelection()
		{
			if (LevelGround.details == null)
			{
				return;
			}
			if ((int)EditorTerrainDetailsUI.selected < LevelGround.details.Length)
			{
				GroundDetail groundDetail = LevelGround.details[(int)EditorTerrainDetailsUI.selected];
				if (groundDetail.prototype.prototypeTexture != null)
				{
					EditorTerrainDetailsUI.selectedBox.text = groundDetail.prototype.prototypeTexture.name;
				}
				else
				{
					EditorTerrainDetailsUI.selectedBox.text = groundDetail.prototype.prototype.name;
				}
				EditorTerrainDetailsUI.densitySlider.state = groundDetail.density;
				EditorTerrainDetailsUI.chanceSlider.state = groundDetail.chance;
				EditorTerrainDetailsUI.grass_0_Toggle.state = groundDetail.isGrass_0;
				EditorTerrainDetailsUI.grass_1_Toggle.state = groundDetail.isGrass_1;
				EditorTerrainDetailsUI.flower_0_Toggle.state = groundDetail.isFlower_0;
				EditorTerrainDetailsUI.flower_1_Toggle.state = groundDetail.isFlower_1;
				EditorTerrainDetailsUI.rockToggle.state = groundDetail.isRock;
				EditorTerrainDetailsUI.roadToggle.state = groundDetail.isRoad;
				EditorTerrainDetailsUI.snowToggle.state = groundDetail.isSnow;
			}
		}

		private static void onClickedDetailButton(SleekButton button)
		{
			EditorTerrainDetailsUI.selected = (byte)(button.parent.positionOffset_Y / 70);
			EditorTerrainDetailsUI.updateSelection();
		}

		private static void onDraggedDensitySlider(SleekSlider slider, float state)
		{
			LevelGround.details[(int)EditorTerrainDetailsUI.selected].density = state;
		}

		private static void onDraggedChanceSlider(SleekSlider slider, float state)
		{
			LevelGround.details[(int)EditorTerrainDetailsUI.selected].chance = state;
		}

		private static void onToggledGrass_0_Toggle(SleekToggle toggle, bool state)
		{
			LevelGround.details[(int)EditorTerrainDetailsUI.selected].isGrass_0 = state;
		}

		private static void onToggledGrass_1_Toggle(SleekToggle toggle, bool state)
		{
			LevelGround.details[(int)EditorTerrainDetailsUI.selected].isGrass_1 = state;
		}

		private static void onToggledFlower_0_Toggle(SleekToggle toggle, bool state)
		{
			LevelGround.details[(int)EditorTerrainDetailsUI.selected].isFlower_0 = state;
		}

		private static void onToggledFlower_1_Toggle(SleekToggle toggle, bool state)
		{
			LevelGround.details[(int)EditorTerrainDetailsUI.selected].isFlower_1 = state;
		}

		private static void onToggledRockToggle(SleekToggle toggle, bool state)
		{
			LevelGround.details[(int)EditorTerrainDetailsUI.selected].isRock = state;
		}

		private static void onToggledRoadToggle(SleekToggle toggle, bool state)
		{
			LevelGround.details[(int)EditorTerrainDetailsUI.selected].isRoad = state;
		}

		private static void onToggledSnowToggle(SleekToggle toggle, bool state)
		{
			LevelGround.details[(int)EditorTerrainDetailsUI.selected].isSnow = state;
		}

		private static void onClickedBakeDetailsButton(SleekButton button)
		{
			LevelGround.bakeDetails();
		}

		private static Sleek container;

		public static bool active;

		private static SleekScrollBox detailsScrollBox;

		private static SleekBox selectedBox;

		private static SleekSlider densitySlider;

		private static SleekSlider chanceSlider;

		private static SleekToggle grass_0_Toggle;

		private static SleekToggle grass_1_Toggle;

		private static SleekToggle flower_0_Toggle;

		private static SleekToggle flower_1_Toggle;

		private static SleekToggle rockToggle;

		private static SleekToggle roadToggle;

		private static SleekToggle snowToggle;

		private static SleekButtonIcon bakeDetailsButton;

		private static byte selected;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache0;

		[CompilerGenerated]
		private static Dragged <>f__mg$cache1;

		[CompilerGenerated]
		private static Dragged <>f__mg$cache2;

		[CompilerGenerated]
		private static Toggled <>f__mg$cache3;

		[CompilerGenerated]
		private static Toggled <>f__mg$cache4;

		[CompilerGenerated]
		private static Toggled <>f__mg$cache5;

		[CompilerGenerated]
		private static Toggled <>f__mg$cache6;

		[CompilerGenerated]
		private static Toggled <>f__mg$cache7;

		[CompilerGenerated]
		private static Toggled <>f__mg$cache8;

		[CompilerGenerated]
		private static Toggled <>f__mg$cache9;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cacheA;
	}
}
