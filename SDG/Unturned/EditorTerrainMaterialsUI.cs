using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace SDG.Unturned
{
	public class EditorTerrainMaterialsUI
	{
		public EditorTerrainMaterialsUI()
		{
			Local local = Localization.read("/Editor/EditorTerrainMaterials.dat");
			Bundle bundle = Bundles.getBundle("/Bundles/Textures/Edit/Icons/EditorTerrainMaterials/EditorTerrainMaterials.unity3d");
			EditorTerrainMaterialsUI.container = new Sleek();
			EditorTerrainMaterialsUI.container.positionOffset_X = 10;
			EditorTerrainMaterialsUI.container.positionOffset_Y = 10;
			EditorTerrainMaterialsUI.container.positionScale_X = 1f;
			EditorTerrainMaterialsUI.container.sizeOffset_X = -20;
			EditorTerrainMaterialsUI.container.sizeOffset_Y = -20;
			EditorTerrainMaterialsUI.container.sizeScale_X = 1f;
			EditorTerrainMaterialsUI.container.sizeScale_Y = 1f;
			EditorUI.window.add(EditorTerrainMaterialsUI.container);
			EditorTerrainMaterialsUI.active = false;
			EditorTerrainMaterialsUI.wasOpened = false;
			if (LevelGround.materials == null)
			{
				return;
			}
			EditorTerrainMaterialsUI.materialScrollBox = new SleekScrollBox();
			EditorTerrainMaterialsUI.materialScrollBox.positionOffset_Y = 120;
			EditorTerrainMaterialsUI.materialScrollBox.positionOffset_X = -400;
			EditorTerrainMaterialsUI.materialScrollBox.positionScale_X = 1f;
			EditorTerrainMaterialsUI.materialScrollBox.sizeOffset_X = 400;
			EditorTerrainMaterialsUI.materialScrollBox.sizeOffset_Y = -200;
			EditorTerrainMaterialsUI.materialScrollBox.sizeScale_Y = 1f;
			EditorTerrainMaterialsUI.materialScrollBox.area = new Rect(0f, 0f, 5f, (float)(LevelGround.materials.Length * 70 + 750));
			EditorTerrainMaterialsUI.container.add(EditorTerrainMaterialsUI.materialScrollBox);
			for (int i = 0; i < LevelGround.materials.Length; i++)
			{
				SleekImageTexture sleekImageTexture = new SleekImageTexture();
				sleekImageTexture.positionOffset_X = 200;
				sleekImageTexture.positionOffset_Y = i * 70;
				sleekImageTexture.sizeOffset_X = 64;
				sleekImageTexture.sizeOffset_Y = 64;
				sleekImageTexture.texture = LevelGround.materials[i].prototype.texture;
				EditorTerrainMaterialsUI.materialScrollBox.add(sleekImageTexture);
				SleekButton sleekButton = new SleekButton();
				sleekButton.positionOffset_X = 70;
				sleekButton.sizeOffset_X = 100;
				sleekButton.sizeOffset_Y = 64;
				sleekButton.text = i + " " + LevelGround.materials[i].prototype.texture.name;
				SleekButton sleekButton2 = sleekButton;
				if (EditorTerrainMaterialsUI.<>f__mg$cache0 == null)
				{
					EditorTerrainMaterialsUI.<>f__mg$cache0 = new ClickedButton(EditorTerrainMaterialsUI.onClickedMaterialButton);
				}
				sleekButton2.onClickedButton = EditorTerrainMaterialsUI.<>f__mg$cache0;
				sleekImageTexture.add(sleekButton);
			}
			EditorTerrainMaterialsUI.overgrowthSlider = new SleekSlider();
			EditorTerrainMaterialsUI.overgrowthSlider.positionOffset_X = 200;
			EditorTerrainMaterialsUI.overgrowthSlider.positionOffset_Y = LevelGround.materials.Length * 70;
			EditorTerrainMaterialsUI.overgrowthSlider.sizeOffset_X = 170;
			EditorTerrainMaterialsUI.overgrowthSlider.sizeOffset_Y = 20;
			EditorTerrainMaterialsUI.overgrowthSlider.orientation = ESleekOrientation.HORIZONTAL;
			EditorTerrainMaterialsUI.overgrowthSlider.addLabel(local.format("OvergrowthSliderLabelText"), ESleekSide.LEFT);
			SleekSlider sleekSlider = EditorTerrainMaterialsUI.overgrowthSlider;
			if (EditorTerrainMaterialsUI.<>f__mg$cache1 == null)
			{
				EditorTerrainMaterialsUI.<>f__mg$cache1 = new Dragged(EditorTerrainMaterialsUI.onDraggedOvergrowthSlider);
			}
			sleekSlider.onDragged = EditorTerrainMaterialsUI.<>f__mg$cache1;
			EditorTerrainMaterialsUI.materialScrollBox.add(EditorTerrainMaterialsUI.overgrowthSlider);
			EditorTerrainMaterialsUI.chanceSlider = new SleekSlider();
			EditorTerrainMaterialsUI.chanceSlider.positionOffset_X = 200;
			EditorTerrainMaterialsUI.chanceSlider.positionOffset_Y = LevelGround.materials.Length * 70 + 30;
			EditorTerrainMaterialsUI.chanceSlider.sizeOffset_X = 170;
			EditorTerrainMaterialsUI.chanceSlider.sizeOffset_Y = 20;
			EditorTerrainMaterialsUI.chanceSlider.orientation = ESleekOrientation.HORIZONTAL;
			EditorTerrainMaterialsUI.chanceSlider.addLabel(local.format("ChanceSliderLabelText"), ESleekSide.LEFT);
			SleekSlider sleekSlider2 = EditorTerrainMaterialsUI.chanceSlider;
			if (EditorTerrainMaterialsUI.<>f__mg$cache2 == null)
			{
				EditorTerrainMaterialsUI.<>f__mg$cache2 = new Dragged(EditorTerrainMaterialsUI.onDraggedChanceSlider);
			}
			sleekSlider2.onDragged = EditorTerrainMaterialsUI.<>f__mg$cache2;
			EditorTerrainMaterialsUI.materialScrollBox.add(EditorTerrainMaterialsUI.chanceSlider);
			EditorTerrainMaterialsUI.steepnessSlider = new SleekSlider();
			EditorTerrainMaterialsUI.steepnessSlider.positionOffset_X = 200;
			EditorTerrainMaterialsUI.steepnessSlider.positionOffset_Y = LevelGround.materials.Length * 70 + 60;
			EditorTerrainMaterialsUI.steepnessSlider.sizeOffset_X = 170;
			EditorTerrainMaterialsUI.steepnessSlider.sizeOffset_Y = 20;
			EditorTerrainMaterialsUI.steepnessSlider.orientation = ESleekOrientation.HORIZONTAL;
			EditorTerrainMaterialsUI.steepnessSlider.addLabel(local.format("SteepnessFieldLabelText"), ESleekSide.LEFT);
			SleekSlider sleekSlider3 = EditorTerrainMaterialsUI.steepnessSlider;
			if (EditorTerrainMaterialsUI.<>f__mg$cache3 == null)
			{
				EditorTerrainMaterialsUI.<>f__mg$cache3 = new Dragged(EditorTerrainMaterialsUI.onDraggedSteepnessSlider);
			}
			sleekSlider3.onDragged = EditorTerrainMaterialsUI.<>f__mg$cache3;
			EditorTerrainMaterialsUI.materialScrollBox.add(EditorTerrainMaterialsUI.steepnessSlider);
			EditorTerrainMaterialsUI.transitionSlider = new SleekSlider();
			EditorTerrainMaterialsUI.transitionSlider.positionOffset_X = 200;
			EditorTerrainMaterialsUI.transitionSlider.positionOffset_Y = LevelGround.materials.Length * 70 + 90;
			EditorTerrainMaterialsUI.transitionSlider.sizeOffset_X = 170;
			EditorTerrainMaterialsUI.transitionSlider.sizeOffset_Y = 20;
			EditorTerrainMaterialsUI.transitionSlider.orientation = ESleekOrientation.HORIZONTAL;
			EditorTerrainMaterialsUI.transitionSlider.addLabel(local.format("TransitionSliderLabelText"), ESleekSide.LEFT);
			SleekSlider sleekSlider4 = EditorTerrainMaterialsUI.transitionSlider;
			if (EditorTerrainMaterialsUI.<>f__mg$cache4 == null)
			{
				EditorTerrainMaterialsUI.<>f__mg$cache4 = new Dragged(EditorTerrainMaterialsUI.onDraggedTransitionSlider);
			}
			sleekSlider4.onDragged = EditorTerrainMaterialsUI.<>f__mg$cache4;
			EditorTerrainMaterialsUI.materialScrollBox.add(EditorTerrainMaterialsUI.transitionSlider);
			EditorTerrainMaterialsUI.heightValue = new SleekValue();
			EditorTerrainMaterialsUI.heightValue.positionOffset_X = 200;
			EditorTerrainMaterialsUI.heightValue.positionOffset_Y = LevelGround.materials.Length * 70 + 120;
			EditorTerrainMaterialsUI.heightValue.sizeOffset_X = 170;
			EditorTerrainMaterialsUI.heightValue.sizeOffset_Y = 30;
			EditorTerrainMaterialsUI.heightValue.addLabel(local.format("HeightValueLabelText"), ESleekSide.LEFT);
			SleekValue sleekValue = EditorTerrainMaterialsUI.heightValue;
			if (EditorTerrainMaterialsUI.<>f__mg$cache5 == null)
			{
				EditorTerrainMaterialsUI.<>f__mg$cache5 = new Valued(EditorTerrainMaterialsUI.onValuedHeightValue);
			}
			sleekValue.onValued = EditorTerrainMaterialsUI.<>f__mg$cache5;
			EditorTerrainMaterialsUI.materialScrollBox.add(EditorTerrainMaterialsUI.heightValue);
			EditorTerrainMaterialsUI.grassy_0_Toggle = new SleekToggle();
			EditorTerrainMaterialsUI.grassy_0_Toggle.positionOffset_X = 200;
			EditorTerrainMaterialsUI.grassy_0_Toggle.positionOffset_Y = LevelGround.materials.Length * 70 + 160;
			EditorTerrainMaterialsUI.grassy_0_Toggle.sizeOffset_X = 40;
			EditorTerrainMaterialsUI.grassy_0_Toggle.sizeOffset_Y = 40;
			EditorTerrainMaterialsUI.grassy_0_Toggle.addLabel(local.format("Grassy_0_ToggleLabelText"), ESleekSide.RIGHT);
			SleekToggle sleekToggle = EditorTerrainMaterialsUI.grassy_0_Toggle;
			if (EditorTerrainMaterialsUI.<>f__mg$cache6 == null)
			{
				EditorTerrainMaterialsUI.<>f__mg$cache6 = new Toggled(EditorTerrainMaterialsUI.onToggledGrassy_0_Toggle);
			}
			sleekToggle.onToggled = EditorTerrainMaterialsUI.<>f__mg$cache6;
			EditorTerrainMaterialsUI.materialScrollBox.add(EditorTerrainMaterialsUI.grassy_0_Toggle);
			EditorTerrainMaterialsUI.grassy_1_Toggle = new SleekToggle();
			EditorTerrainMaterialsUI.grassy_1_Toggle.positionOffset_X = 200;
			EditorTerrainMaterialsUI.grassy_1_Toggle.positionOffset_Y = LevelGround.materials.Length * 70 + 210;
			EditorTerrainMaterialsUI.grassy_1_Toggle.sizeOffset_X = 40;
			EditorTerrainMaterialsUI.grassy_1_Toggle.sizeOffset_Y = 40;
			EditorTerrainMaterialsUI.grassy_1_Toggle.addLabel(local.format("Grassy_1_ToggleLabelText"), ESleekSide.RIGHT);
			SleekToggle sleekToggle2 = EditorTerrainMaterialsUI.grassy_1_Toggle;
			if (EditorTerrainMaterialsUI.<>f__mg$cache7 == null)
			{
				EditorTerrainMaterialsUI.<>f__mg$cache7 = new Toggled(EditorTerrainMaterialsUI.onToggledGrassy_1_Toggle);
			}
			sleekToggle2.onToggled = EditorTerrainMaterialsUI.<>f__mg$cache7;
			EditorTerrainMaterialsUI.materialScrollBox.add(EditorTerrainMaterialsUI.grassy_1_Toggle);
			EditorTerrainMaterialsUI.flowery_0_Toggle = new SleekToggle();
			EditorTerrainMaterialsUI.flowery_0_Toggle.positionOffset_X = 200;
			EditorTerrainMaterialsUI.flowery_0_Toggle.positionOffset_Y = LevelGround.materials.Length * 70 + 260;
			EditorTerrainMaterialsUI.flowery_0_Toggle.sizeOffset_X = 40;
			EditorTerrainMaterialsUI.flowery_0_Toggle.sizeOffset_Y = 40;
			EditorTerrainMaterialsUI.flowery_0_Toggle.addLabel(local.format("Flowery_0_ToggleLabelText"), ESleekSide.RIGHT);
			SleekToggle sleekToggle3 = EditorTerrainMaterialsUI.flowery_0_Toggle;
			if (EditorTerrainMaterialsUI.<>f__mg$cache8 == null)
			{
				EditorTerrainMaterialsUI.<>f__mg$cache8 = new Toggled(EditorTerrainMaterialsUI.onToggledFlowery_0_Toggle);
			}
			sleekToggle3.onToggled = EditorTerrainMaterialsUI.<>f__mg$cache8;
			EditorTerrainMaterialsUI.materialScrollBox.add(EditorTerrainMaterialsUI.flowery_0_Toggle);
			EditorTerrainMaterialsUI.flowery_1_Toggle = new SleekToggle();
			EditorTerrainMaterialsUI.flowery_1_Toggle.positionOffset_X = 200;
			EditorTerrainMaterialsUI.flowery_1_Toggle.positionOffset_Y = LevelGround.materials.Length * 70 + 310;
			EditorTerrainMaterialsUI.flowery_1_Toggle.sizeOffset_X = 40;
			EditorTerrainMaterialsUI.flowery_1_Toggle.sizeOffset_Y = 40;
			EditorTerrainMaterialsUI.flowery_1_Toggle.addLabel(local.format("Flowery_1_ToggleLabelText"), ESleekSide.RIGHT);
			SleekToggle sleekToggle4 = EditorTerrainMaterialsUI.flowery_1_Toggle;
			if (EditorTerrainMaterialsUI.<>f__mg$cache9 == null)
			{
				EditorTerrainMaterialsUI.<>f__mg$cache9 = new Toggled(EditorTerrainMaterialsUI.onToggledFlowery_1_Toggle);
			}
			sleekToggle4.onToggled = EditorTerrainMaterialsUI.<>f__mg$cache9;
			EditorTerrainMaterialsUI.materialScrollBox.add(EditorTerrainMaterialsUI.flowery_1_Toggle);
			EditorTerrainMaterialsUI.rockyToggle = new SleekToggle();
			EditorTerrainMaterialsUI.rockyToggle.positionOffset_X = 200;
			EditorTerrainMaterialsUI.rockyToggle.positionOffset_Y = LevelGround.materials.Length * 70 + 360;
			EditorTerrainMaterialsUI.rockyToggle.sizeOffset_X = 40;
			EditorTerrainMaterialsUI.rockyToggle.sizeOffset_Y = 40;
			EditorTerrainMaterialsUI.rockyToggle.addLabel(local.format("RockyToggleLabelText"), ESleekSide.RIGHT);
			SleekToggle sleekToggle5 = EditorTerrainMaterialsUI.rockyToggle;
			if (EditorTerrainMaterialsUI.<>f__mg$cacheA == null)
			{
				EditorTerrainMaterialsUI.<>f__mg$cacheA = new Toggled(EditorTerrainMaterialsUI.onToggledRockyToggle);
			}
			sleekToggle5.onToggled = EditorTerrainMaterialsUI.<>f__mg$cacheA;
			EditorTerrainMaterialsUI.materialScrollBox.add(EditorTerrainMaterialsUI.rockyToggle);
			EditorTerrainMaterialsUI.roadToggle = new SleekToggle();
			EditorTerrainMaterialsUI.roadToggle.positionOffset_X = 200;
			EditorTerrainMaterialsUI.roadToggle.positionOffset_Y = LevelGround.materials.Length * 70 + 410;
			EditorTerrainMaterialsUI.roadToggle.sizeOffset_X = 40;
			EditorTerrainMaterialsUI.roadToggle.sizeOffset_Y = 40;
			EditorTerrainMaterialsUI.roadToggle.addLabel(local.format("RoadToggleLabelText"), ESleekSide.RIGHT);
			SleekToggle sleekToggle6 = EditorTerrainMaterialsUI.roadToggle;
			if (EditorTerrainMaterialsUI.<>f__mg$cacheB == null)
			{
				EditorTerrainMaterialsUI.<>f__mg$cacheB = new Toggled(EditorTerrainMaterialsUI.onToggledRoadToggle);
			}
			sleekToggle6.onToggled = EditorTerrainMaterialsUI.<>f__mg$cacheB;
			EditorTerrainMaterialsUI.materialScrollBox.add(EditorTerrainMaterialsUI.roadToggle);
			EditorTerrainMaterialsUI.snowyToggle = new SleekToggle();
			EditorTerrainMaterialsUI.snowyToggle.positionOffset_X = 200;
			EditorTerrainMaterialsUI.snowyToggle.positionOffset_Y = LevelGround.materials.Length * 70 + 460;
			EditorTerrainMaterialsUI.snowyToggle.sizeOffset_X = 40;
			EditorTerrainMaterialsUI.snowyToggle.sizeOffset_Y = 40;
			EditorTerrainMaterialsUI.snowyToggle.addLabel(local.format("SnowyToggleLabelText"), ESleekSide.RIGHT);
			SleekToggle sleekToggle7 = EditorTerrainMaterialsUI.snowyToggle;
			if (EditorTerrainMaterialsUI.<>f__mg$cacheC == null)
			{
				EditorTerrainMaterialsUI.<>f__mg$cacheC = new Toggled(EditorTerrainMaterialsUI.onToggledSnowyToggle);
			}
			sleekToggle7.onToggled = EditorTerrainMaterialsUI.<>f__mg$cacheC;
			EditorTerrainMaterialsUI.materialScrollBox.add(EditorTerrainMaterialsUI.snowyToggle);
			EditorTerrainMaterialsUI.foundationToggle = new SleekToggle();
			EditorTerrainMaterialsUI.foundationToggle.positionOffset_X = 200;
			EditorTerrainMaterialsUI.foundationToggle.positionOffset_Y = LevelGround.materials.Length * 70 + 510;
			EditorTerrainMaterialsUI.foundationToggle.sizeOffset_X = 40;
			EditorTerrainMaterialsUI.foundationToggle.sizeOffset_Y = 40;
			EditorTerrainMaterialsUI.foundationToggle.addLabel(local.format("FoundationToggleLabelText"), ESleekSide.RIGHT);
			SleekToggle sleekToggle8 = EditorTerrainMaterialsUI.foundationToggle;
			if (EditorTerrainMaterialsUI.<>f__mg$cacheD == null)
			{
				EditorTerrainMaterialsUI.<>f__mg$cacheD = new Toggled(EditorTerrainMaterialsUI.onToggledFoundationToggle);
			}
			sleekToggle8.onToggled = EditorTerrainMaterialsUI.<>f__mg$cacheD;
			EditorTerrainMaterialsUI.materialScrollBox.add(EditorTerrainMaterialsUI.foundationToggle);
			EditorTerrainMaterialsUI.manualToggle = new SleekToggle();
			EditorTerrainMaterialsUI.manualToggle.positionOffset_X = 200;
			EditorTerrainMaterialsUI.manualToggle.positionOffset_Y = LevelGround.materials.Length * 70 + 560;
			EditorTerrainMaterialsUI.manualToggle.sizeOffset_X = 40;
			EditorTerrainMaterialsUI.manualToggle.sizeOffset_Y = 40;
			EditorTerrainMaterialsUI.manualToggle.addLabel(local.format("ManualToggleLabelText"), ESleekSide.RIGHT);
			SleekToggle sleekToggle9 = EditorTerrainMaterialsUI.manualToggle;
			if (EditorTerrainMaterialsUI.<>f__mg$cacheE == null)
			{
				EditorTerrainMaterialsUI.<>f__mg$cacheE = new Toggled(EditorTerrainMaterialsUI.onToggledManualToggle);
			}
			sleekToggle9.onToggled = EditorTerrainMaterialsUI.<>f__mg$cacheE;
			EditorTerrainMaterialsUI.materialScrollBox.add(EditorTerrainMaterialsUI.manualToggle);
			EditorTerrainMaterialsUI.steepnessToggle = new SleekToggle();
			EditorTerrainMaterialsUI.steepnessToggle.positionOffset_X = 200;
			EditorTerrainMaterialsUI.steepnessToggle.positionOffset_Y = LevelGround.materials.Length * 70 + 610;
			EditorTerrainMaterialsUI.steepnessToggle.sizeOffset_X = 40;
			EditorTerrainMaterialsUI.steepnessToggle.sizeOffset_Y = 40;
			EditorTerrainMaterialsUI.steepnessToggle.addLabel(local.format("SteepnessToggleLabelText"), ESleekSide.RIGHT);
			SleekToggle sleekToggle10 = EditorTerrainMaterialsUI.steepnessToggle;
			if (EditorTerrainMaterialsUI.<>f__mg$cacheF == null)
			{
				EditorTerrainMaterialsUI.<>f__mg$cacheF = new Toggled(EditorTerrainMaterialsUI.onToggledSteepnessToggle);
			}
			sleekToggle10.onToggled = EditorTerrainMaterialsUI.<>f__mg$cacheF;
			EditorTerrainMaterialsUI.materialScrollBox.add(EditorTerrainMaterialsUI.steepnessToggle);
			EditorTerrainMaterialsUI.heightToggle = new SleekToggle();
			EditorTerrainMaterialsUI.heightToggle.positionOffset_X = 200;
			EditorTerrainMaterialsUI.heightToggle.positionOffset_Y = LevelGround.materials.Length * 70 + 660;
			EditorTerrainMaterialsUI.heightToggle.sizeOffset_X = 40;
			EditorTerrainMaterialsUI.heightToggle.sizeOffset_Y = 40;
			EditorTerrainMaterialsUI.heightToggle.addLabel(local.format("HeightToggleLabelText"), ESleekSide.RIGHT);
			SleekToggle sleekToggle11 = EditorTerrainMaterialsUI.heightToggle;
			if (EditorTerrainMaterialsUI.<>f__mg$cache10 == null)
			{
				EditorTerrainMaterialsUI.<>f__mg$cache10 = new Toggled(EditorTerrainMaterialsUI.onToggledHeightToggle);
			}
			sleekToggle11.onToggled = EditorTerrainMaterialsUI.<>f__mg$cache10;
			EditorTerrainMaterialsUI.materialScrollBox.add(EditorTerrainMaterialsUI.heightToggle);
			EditorTerrainMaterialsUI.footprintToggle = new SleekToggle();
			EditorTerrainMaterialsUI.footprintToggle.positionOffset_X = 200;
			EditorTerrainMaterialsUI.footprintToggle.positionOffset_Y = LevelGround.materials.Length * 70 + 710;
			EditorTerrainMaterialsUI.footprintToggle.sizeOffset_X = 40;
			EditorTerrainMaterialsUI.footprintToggle.sizeOffset_Y = 40;
			EditorTerrainMaterialsUI.footprintToggle.addLabel(local.format("FootprintToggleLabelText"), ESleekSide.RIGHT);
			SleekToggle sleekToggle12 = EditorTerrainMaterialsUI.footprintToggle;
			if (EditorTerrainMaterialsUI.<>f__mg$cache11 == null)
			{
				EditorTerrainMaterialsUI.<>f__mg$cache11 = new Toggled(EditorTerrainMaterialsUI.onToggledFootprintToggle);
			}
			sleekToggle12.onToggled = EditorTerrainMaterialsUI.<>f__mg$cache11;
			EditorTerrainMaterialsUI.materialScrollBox.add(EditorTerrainMaterialsUI.footprintToggle);
			EditorTerrainMaterialsUI.selectedBox = new SleekBox();
			EditorTerrainMaterialsUI.selectedBox.positionOffset_X = -200;
			EditorTerrainMaterialsUI.selectedBox.positionOffset_Y = 80;
			EditorTerrainMaterialsUI.selectedBox.positionScale_X = 1f;
			EditorTerrainMaterialsUI.selectedBox.sizeOffset_X = 200;
			EditorTerrainMaterialsUI.selectedBox.sizeOffset_Y = 30;
			EditorTerrainMaterialsUI.selectedBox.addLabel(local.format("SelectionBoxLabelText"), ESleekSide.LEFT);
			EditorTerrainMaterialsUI.container.add(EditorTerrainMaterialsUI.selectedBox);
			EditorTerrainMaterialsUI.updateSelection();
			EditorTerrainMaterialsUI.bakeGlobalMaterialsButton = new SleekButtonIcon((Texture2D)bundle.load("Materials"));
			EditorTerrainMaterialsUI.bakeGlobalMaterialsButton.positionOffset_X = -200;
			EditorTerrainMaterialsUI.bakeGlobalMaterialsButton.positionOffset_Y = -70;
			EditorTerrainMaterialsUI.bakeGlobalMaterialsButton.positionScale_X = 1f;
			EditorTerrainMaterialsUI.bakeGlobalMaterialsButton.positionScale_Y = 1f;
			EditorTerrainMaterialsUI.bakeGlobalMaterialsButton.sizeOffset_X = 200;
			EditorTerrainMaterialsUI.bakeGlobalMaterialsButton.sizeOffset_Y = 30;
			EditorTerrainMaterialsUI.bakeGlobalMaterialsButton.text = local.format("BakeGlobalMaterialsButtonText");
			EditorTerrainMaterialsUI.bakeGlobalMaterialsButton.tooltip = local.format("BakeGlobalMaterialsButtonTooltip");
			SleekButton sleekButton3 = EditorTerrainMaterialsUI.bakeGlobalMaterialsButton;
			if (EditorTerrainMaterialsUI.<>f__mg$cache12 == null)
			{
				EditorTerrainMaterialsUI.<>f__mg$cache12 = new ClickedButton(EditorTerrainMaterialsUI.onClickedBakeGlobalMaterialsButton);
			}
			sleekButton3.onClickedButton = EditorTerrainMaterialsUI.<>f__mg$cache12;
			EditorTerrainMaterialsUI.container.add(EditorTerrainMaterialsUI.bakeGlobalMaterialsButton);
			EditorTerrainMaterialsUI.bakeLocalMaterialsButton = new SleekButtonIcon((Texture2D)bundle.load("Materials"));
			EditorTerrainMaterialsUI.bakeLocalMaterialsButton.positionOffset_X = -200;
			EditorTerrainMaterialsUI.bakeLocalMaterialsButton.positionOffset_Y = -30;
			EditorTerrainMaterialsUI.bakeLocalMaterialsButton.positionScale_X = 1f;
			EditorTerrainMaterialsUI.bakeLocalMaterialsButton.positionScale_Y = 1f;
			EditorTerrainMaterialsUI.bakeLocalMaterialsButton.sizeOffset_X = 200;
			EditorTerrainMaterialsUI.bakeLocalMaterialsButton.sizeOffset_Y = 30;
			EditorTerrainMaterialsUI.bakeLocalMaterialsButton.text = local.format("BakeLocalMaterialsButtonText");
			EditorTerrainMaterialsUI.bakeLocalMaterialsButton.tooltip = local.format("BakeLocalMaterialsButtonTooltip");
			SleekButton sleekButton4 = EditorTerrainMaterialsUI.bakeLocalMaterialsButton;
			if (EditorTerrainMaterialsUI.<>f__mg$cache13 == null)
			{
				EditorTerrainMaterialsUI.<>f__mg$cache13 = new ClickedButton(EditorTerrainMaterialsUI.onClickedBakeLocalMaterialsButton);
			}
			sleekButton4.onClickedButton = EditorTerrainMaterialsUI.<>f__mg$cache13;
			EditorTerrainMaterialsUI.container.add(EditorTerrainMaterialsUI.bakeLocalMaterialsButton);
			EditorTerrainMaterialsUI.map2Button = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(local.format("Map2ButtonText1")),
				new GUIContent(local.format("Map2ButtonText2"))
			});
			EditorTerrainMaterialsUI.map2Button.positionOffset_Y = -30;
			EditorTerrainMaterialsUI.map2Button.positionScale_Y = 1f;
			EditorTerrainMaterialsUI.map2Button.sizeOffset_X = 200;
			EditorTerrainMaterialsUI.map2Button.sizeOffset_Y = 30;
			EditorTerrainMaterialsUI.map2Button.tooltip = local.format("Map2ButtonTooltip");
			SleekButtonState sleekButtonState = EditorTerrainMaterialsUI.map2Button;
			if (EditorTerrainMaterialsUI.<>f__mg$cache14 == null)
			{
				EditorTerrainMaterialsUI.<>f__mg$cache14 = new SwappedState(EditorTerrainMaterialsUI.onSwappedMap2);
			}
			sleekButtonState.onSwappedState = EditorTerrainMaterialsUI.<>f__mg$cache14;
			EditorTerrainMaterialsUI.container.add(EditorTerrainMaterialsUI.map2Button);
			EditorTerrainMaterialsUI.sizeSlider = new SleekSlider();
			EditorTerrainMaterialsUI.sizeSlider.positionOffset_Y = -60;
			EditorTerrainMaterialsUI.sizeSlider.positionScale_Y = 1f;
			EditorTerrainMaterialsUI.sizeSlider.sizeOffset_X = 200;
			EditorTerrainMaterialsUI.sizeSlider.sizeOffset_Y = 20;
			EditorTerrainMaterialsUI.sizeSlider.orientation = ESleekOrientation.HORIZONTAL;
			EditorTerrainMaterialsUI.sizeSlider.state = (float)(EditorTerrainMaterials.brushSize - EditorTerrainMaterials.MIN_BRUSH_SIZE) / (float)EditorTerrainMaterials.MAX_BRUSH_SIZE;
			EditorTerrainMaterialsUI.sizeSlider.addLabel(local.format("SizeSliderLabelText"), ESleekSide.RIGHT);
			SleekSlider sleekSlider5 = EditorTerrainMaterialsUI.sizeSlider;
			if (EditorTerrainMaterialsUI.<>f__mg$cache15 == null)
			{
				EditorTerrainMaterialsUI.<>f__mg$cache15 = new Dragged(EditorTerrainMaterialsUI.onDraggedSizeSlider);
			}
			sleekSlider5.onDragged = EditorTerrainMaterialsUI.<>f__mg$cache15;
			EditorTerrainMaterialsUI.container.add(EditorTerrainMaterialsUI.sizeSlider);
			EditorTerrainMaterialsUI.noiseSlider = new SleekSlider();
			EditorTerrainMaterialsUI.noiseSlider.positionOffset_Y = -90;
			EditorTerrainMaterialsUI.noiseSlider.positionScale_Y = 1f;
			EditorTerrainMaterialsUI.noiseSlider.sizeOffset_X = 200;
			EditorTerrainMaterialsUI.noiseSlider.sizeOffset_Y = 20;
			EditorTerrainMaterialsUI.noiseSlider.orientation = ESleekOrientation.HORIZONTAL;
			EditorTerrainMaterialsUI.noiseSlider.state = EditorTerrainMaterials.brushNoise;
			EditorTerrainMaterialsUI.noiseSlider.addLabel(local.format("NoiseSliderLabelText"), ESleekSide.RIGHT);
			SleekSlider sleekSlider6 = EditorTerrainMaterialsUI.noiseSlider;
			if (EditorTerrainMaterialsUI.<>f__mg$cache16 == null)
			{
				EditorTerrainMaterialsUI.<>f__mg$cache16 = new Dragged(EditorTerrainMaterialsUI.onDraggedNoiseSlider);
			}
			sleekSlider6.onDragged = EditorTerrainMaterialsUI.<>f__mg$cache16;
			EditorTerrainMaterialsUI.container.add(EditorTerrainMaterialsUI.noiseSlider);
			EditorTerrainMaterialsUI.previewToggle = new SleekToggle();
			EditorTerrainMaterialsUI.previewToggle.positionOffset_Y = -140;
			EditorTerrainMaterialsUI.previewToggle.positionScale_Y = 1f;
			EditorTerrainMaterialsUI.previewToggle.sizeOffset_X = 40;
			EditorTerrainMaterialsUI.previewToggle.sizeOffset_Y = 40;
			EditorTerrainMaterialsUI.previewToggle.addLabel(local.format("PreviewToggleLabelText"), ESleekSide.RIGHT);
			SleekToggle sleekToggle13 = EditorTerrainMaterialsUI.previewToggle;
			if (EditorTerrainMaterialsUI.<>f__mg$cache17 == null)
			{
				EditorTerrainMaterialsUI.<>f__mg$cache17 = new Toggled(EditorTerrainMaterialsUI.onToggledPreviewToggle);
			}
			sleekToggle13.onToggled = EditorTerrainMaterialsUI.<>f__mg$cache17;
			EditorTerrainMaterialsUI.container.add(EditorTerrainMaterialsUI.previewToggle);
			bundle.unload();
		}

		public static void open()
		{
			if (EditorTerrainMaterialsUI.active)
			{
				return;
			}
			EditorTerrainMaterialsUI.active = true;
			if (LevelGround.materials == null)
			{
				return;
			}
			EditorTerrainMaterials.isPainting = true;
			if (!EditorTerrainMaterialsUI.wasOpened)
			{
				LevelGround.previewHQ = false;
				EditorTerrainMaterialsUI.wasOpened = true;
			}
			LevelGround.data.baseMapResolution = 32;
			LevelGround.data.baseMapResolution = 16;
			LevelGround.terrain.terrainData = LevelGround.data;
			LevelGround.data2.baseMapResolution = 32;
			LevelGround.data2.baseMapResolution = 16;
			LevelGround.terrain2.terrainData = LevelGround.data2;
			EditorTerrainMaterialsUI.container.lerpPositionScale(0f, 0f, ESleekLerp.EXPONENTIAL, 20f);
		}

		public static void close()
		{
			if (!EditorTerrainMaterialsUI.active)
			{
				return;
			}
			EditorTerrainMaterialsUI.active = false;
			if (LevelGround.materials == null)
			{
				return;
			}
			EditorTerrainMaterials.isPainting = false;
			LevelGround.data.baseMapResolution = (int)(Level.size / 8);
			LevelGround.data.baseMapResolution = (int)(Level.size / 4);
			LevelGround.terrain.terrainData = LevelGround.data;
			LevelGround.data2.baseMapResolution = (int)(Level.size / 8);
			LevelGround.data2.baseMapResolution = (int)(Level.size / 4);
			LevelGround.terrain2.terrainData = LevelGround.data2;
			EditorTerrainMaterialsUI.container.lerpPositionScale(1f, 0f, ESleekLerp.EXPONENTIAL, 20f);
		}

		private static void updateSelection()
		{
			if (LevelGround.materials == null)
			{
				return;
			}
			if ((int)EditorTerrainMaterials.selected < LevelGround.materials.Length)
			{
				GroundMaterial groundMaterial = LevelGround.materials[(int)EditorTerrainMaterials.selected];
				EditorTerrainMaterialsUI.selectedBox.text = groundMaterial.prototype.texture.name;
				EditorTerrainMaterialsUI.overgrowthSlider.state = groundMaterial.overgrowth;
				EditorTerrainMaterialsUI.chanceSlider.state = groundMaterial.chance;
				EditorTerrainMaterialsUI.steepnessSlider.state = groundMaterial.steepness;
				EditorTerrainMaterialsUI.heightValue.state = groundMaterial.height;
				EditorTerrainMaterialsUI.transitionSlider.state = groundMaterial.transition;
				EditorTerrainMaterialsUI.grassy_0_Toggle.state = groundMaterial.isGrassy_0;
				EditorTerrainMaterialsUI.grassy_1_Toggle.state = groundMaterial.isGrassy_1;
				EditorTerrainMaterialsUI.flowery_0_Toggle.state = groundMaterial.isFlowery_0;
				EditorTerrainMaterialsUI.flowery_1_Toggle.state = groundMaterial.isFlowery_1;
				EditorTerrainMaterialsUI.rockyToggle.state = groundMaterial.isRocky;
				EditorTerrainMaterialsUI.roadToggle.state = groundMaterial.isRoad;
				EditorTerrainMaterialsUI.snowyToggle.state = groundMaterial.isSnowy;
				EditorTerrainMaterialsUI.foundationToggle.state = groundMaterial.isFoundation;
				EditorTerrainMaterialsUI.manualToggle.state = groundMaterial.isManual;
				EditorTerrainMaterialsUI.steepnessToggle.state = groundMaterial.ignoreSteepness;
				EditorTerrainMaterialsUI.heightToggle.state = groundMaterial.ignoreHeight;
				EditorTerrainMaterialsUI.footprintToggle.state = groundMaterial.ignoreFootprint;
			}
		}

		private static void onClickedMaterialButton(SleekButton button)
		{
			EditorTerrainMaterials.selected = (byte)(button.parent.positionOffset_Y / 70);
			EditorTerrainMaterialsUI.updateSelection();
		}

		private static void onDraggedOvergrowthSlider(SleekSlider slider, float state)
		{
			LevelGround.materials[(int)EditorTerrainMaterials.selected].overgrowth = state;
		}

		private static void onDraggedChanceSlider(SleekSlider slider, float state)
		{
			LevelGround.materials[(int)EditorTerrainMaterials.selected].chance = state;
		}

		private static void onDraggedSteepnessSlider(SleekSlider slider, float state)
		{
			LevelGround.materials[(int)EditorTerrainMaterials.selected].steepness = state;
		}

		private static void onValuedHeightValue(SleekValue value, float state)
		{
			LevelGround.materials[(int)EditorTerrainMaterials.selected].height = state;
		}

		private static void onDraggedTransitionSlider(SleekSlider slider, float state)
		{
			LevelGround.materials[(int)EditorTerrainMaterials.selected].transition = state;
		}

		private static void onToggledGrassy_0_Toggle(SleekToggle toggle, bool state)
		{
			LevelGround.materials[(int)EditorTerrainMaterials.selected].isGrassy_0 = state;
		}

		private static void onToggledGrassy_1_Toggle(SleekToggle toggle, bool state)
		{
			LevelGround.materials[(int)EditorTerrainMaterials.selected].isGrassy_1 = state;
		}

		private static void onToggledFlowery_0_Toggle(SleekToggle toggle, bool state)
		{
			LevelGround.materials[(int)EditorTerrainMaterials.selected].isFlowery_0 = state;
		}

		private static void onToggledFlowery_1_Toggle(SleekToggle toggle, bool state)
		{
			LevelGround.materials[(int)EditorTerrainMaterials.selected].isFlowery_1 = state;
		}

		private static void onToggledRockyToggle(SleekToggle toggle, bool state)
		{
			LevelGround.materials[(int)EditorTerrainMaterials.selected].isRocky = state;
		}

		private static void onToggledRoadToggle(SleekToggle toggle, bool state)
		{
			LevelGround.materials[(int)EditorTerrainMaterials.selected].isRoad = state;
		}

		private static void onToggledSnowyToggle(SleekToggle toggle, bool state)
		{
			LevelGround.materials[(int)EditorTerrainMaterials.selected].isSnowy = state;
		}

		private static void onToggledFoundationToggle(SleekToggle toggle, bool state)
		{
			LevelGround.materials[(int)EditorTerrainMaterials.selected].isFoundation = state;
		}

		private static void onToggledManualToggle(SleekToggle toggle, bool state)
		{
			LevelGround.materials[(int)EditorTerrainMaterials.selected].isManual = state;
		}

		private static void onToggledSteepnessToggle(SleekToggle toggle, bool state)
		{
			LevelGround.materials[(int)EditorTerrainMaterials.selected].ignoreSteepness = state;
		}

		private static void onToggledHeightToggle(SleekToggle toggle, bool state)
		{
			LevelGround.materials[(int)EditorTerrainMaterials.selected].ignoreHeight = state;
		}

		private static void onToggledFootprintToggle(SleekToggle toggle, bool state)
		{
			LevelGround.materials[(int)EditorTerrainMaterials.selected].ignoreFootprint = state;
		}

		private static void onClickedBakeGlobalMaterialsButton(SleekButton button)
		{
			LevelGround.bakeMaterials(true);
		}

		private static void onClickedBakeLocalMaterialsButton(SleekButton button)
		{
			LevelGround.bakeMaterials(false);
		}

		private static void onDraggedSizeSlider(Sleek field, float state)
		{
			EditorTerrainMaterials.brushSize = (byte)((float)EditorTerrainMaterials.MIN_BRUSH_SIZE + state * (float)EditorTerrainMaterials.MAX_BRUSH_SIZE);
		}

		private static void onDraggedNoiseSlider(Sleek field, float state)
		{
			EditorTerrainMaterials.brushNoise = state;
		}

		private static void onToggledPreviewToggle(SleekToggle toggle, bool state)
		{
			LevelGround.previewHQ = state;
		}

		private static void onSwappedMap2(SleekButtonState button, int state)
		{
			EditorTerrainMaterials.map2 = (state == 1);
		}

		private static Sleek container;

		public static bool active;

		private static SleekScrollBox materialScrollBox;

		private static SleekButtonIcon bakeGlobalMaterialsButton;

		private static SleekButtonIcon bakeLocalMaterialsButton;

		private static SleekButtonState map2Button;

		private static SleekBox selectedBox;

		private static SleekSlider overgrowthSlider;

		private static SleekSlider chanceSlider;

		private static SleekSlider steepnessSlider;

		private static SleekValue heightValue;

		private static SleekSlider transitionSlider;

		private static SleekToggle grassy_0_Toggle;

		private static SleekToggle grassy_1_Toggle;

		private static SleekToggle flowery_0_Toggle;

		private static SleekToggle flowery_1_Toggle;

		private static SleekToggle rockyToggle;

		private static SleekToggle roadToggle;

		private static SleekToggle snowyToggle;

		private static SleekToggle foundationToggle;

		private static SleekToggle manualToggle;

		private static SleekToggle steepnessToggle;

		private static SleekToggle heightToggle;

		private static SleekToggle footprintToggle;

		private static SleekSlider sizeSlider;

		private static SleekSlider noiseSlider;

		public static SleekToggle previewToggle;

		private static bool wasOpened;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache0;

		[CompilerGenerated]
		private static Dragged <>f__mg$cache1;

		[CompilerGenerated]
		private static Dragged <>f__mg$cache2;

		[CompilerGenerated]
		private static Dragged <>f__mg$cache3;

		[CompilerGenerated]
		private static Dragged <>f__mg$cache4;

		[CompilerGenerated]
		private static Valued <>f__mg$cache5;

		[CompilerGenerated]
		private static Toggled <>f__mg$cache6;

		[CompilerGenerated]
		private static Toggled <>f__mg$cache7;

		[CompilerGenerated]
		private static Toggled <>f__mg$cache8;

		[CompilerGenerated]
		private static Toggled <>f__mg$cache9;

		[CompilerGenerated]
		private static Toggled <>f__mg$cacheA;

		[CompilerGenerated]
		private static Toggled <>f__mg$cacheB;

		[CompilerGenerated]
		private static Toggled <>f__mg$cacheC;

		[CompilerGenerated]
		private static Toggled <>f__mg$cacheD;

		[CompilerGenerated]
		private static Toggled <>f__mg$cacheE;

		[CompilerGenerated]
		private static Toggled <>f__mg$cacheF;

		[CompilerGenerated]
		private static Toggled <>f__mg$cache10;

		[CompilerGenerated]
		private static Toggled <>f__mg$cache11;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache12;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache13;

		[CompilerGenerated]
		private static SwappedState <>f__mg$cache14;

		[CompilerGenerated]
		private static Dragged <>f__mg$cache15;

		[CompilerGenerated]
		private static Dragged <>f__mg$cache16;

		[CompilerGenerated]
		private static Toggled <>f__mg$cache17;
	}
}
