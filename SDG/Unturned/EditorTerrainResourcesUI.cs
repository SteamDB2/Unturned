using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace SDG.Unturned
{
	public class EditorTerrainResourcesUI
	{
		public EditorTerrainResourcesUI()
		{
			Local local = Localization.read("/Editor/EditorTerrainResources.dat");
			Bundle bundle = Bundles.getBundle("/Bundles/Textures/Edit/Icons/EditorTerrainResources/EditorTerrainResources.unity3d");
			EditorTerrainResourcesUI.container = new Sleek();
			EditorTerrainResourcesUI.container.positionOffset_X = 10;
			EditorTerrainResourcesUI.container.positionOffset_Y = 10;
			EditorTerrainResourcesUI.container.positionScale_X = 1f;
			EditorTerrainResourcesUI.container.sizeOffset_X = -20;
			EditorTerrainResourcesUI.container.sizeOffset_Y = -20;
			EditorTerrainResourcesUI.container.sizeScale_X = 1f;
			EditorTerrainResourcesUI.container.sizeScale_Y = 1f;
			EditorUI.window.add(EditorTerrainResourcesUI.container);
			EditorTerrainResourcesUI.active = false;
			EditorTerrainResourcesUI.resourcesScrollBox = new SleekScrollBox();
			EditorTerrainResourcesUI.resourcesScrollBox.positionOffset_Y = 120;
			EditorTerrainResourcesUI.resourcesScrollBox.positionOffset_X = -400;
			EditorTerrainResourcesUI.resourcesScrollBox.positionScale_X = 1f;
			EditorTerrainResourcesUI.resourcesScrollBox.sizeOffset_X = 400;
			EditorTerrainResourcesUI.resourcesScrollBox.sizeOffset_Y = -240;
			EditorTerrainResourcesUI.resourcesScrollBox.sizeScale_Y = 1f;
			EditorTerrainResourcesUI.resourcesScrollBox.area = new Rect(0f, 0f, 5f, (float)(LevelGround.resources.Length * 40 + 400));
			EditorTerrainResourcesUI.container.add(EditorTerrainResourcesUI.resourcesScrollBox);
			for (int i = 0; i < LevelGround.resources.Length; i++)
			{
				ResourceAsset resourceAsset = (ResourceAsset)Assets.find(EAssetType.RESOURCE, LevelGround.resources[i].id);
				SleekButton sleekButton = new SleekButton();
				sleekButton.positionOffset_X = 200;
				sleekButton.positionOffset_Y = i * 40;
				sleekButton.sizeOffset_X = 170;
				sleekButton.sizeOffset_Y = 30;
				if (resourceAsset != null)
				{
					sleekButton.text = resourceAsset.resourceName;
				}
				SleekButton sleekButton2 = sleekButton;
				if (EditorTerrainResourcesUI.<>f__mg$cache0 == null)
				{
					EditorTerrainResourcesUI.<>f__mg$cache0 = new ClickedButton(EditorTerrainResourcesUI.onClickedResourceButton);
				}
				sleekButton2.onClickedButton = EditorTerrainResourcesUI.<>f__mg$cache0;
				EditorTerrainResourcesUI.resourcesScrollBox.add(sleekButton);
			}
			EditorTerrainResourcesUI.densitySlider = new SleekSlider();
			EditorTerrainResourcesUI.densitySlider.positionOffset_X = 200;
			EditorTerrainResourcesUI.densitySlider.positionOffset_Y = LevelGround.resources.Length * 40;
			EditorTerrainResourcesUI.densitySlider.sizeOffset_X = 170;
			EditorTerrainResourcesUI.densitySlider.sizeOffset_Y = 20;
			EditorTerrainResourcesUI.densitySlider.orientation = ESleekOrientation.HORIZONTAL;
			EditorTerrainResourcesUI.densitySlider.addLabel(local.format("DensitySliderLabelText"), ESleekSide.LEFT);
			SleekSlider sleekSlider = EditorTerrainResourcesUI.densitySlider;
			if (EditorTerrainResourcesUI.<>f__mg$cache1 == null)
			{
				EditorTerrainResourcesUI.<>f__mg$cache1 = new Dragged(EditorTerrainResourcesUI.onDraggedDensitySlider);
			}
			sleekSlider.onDragged = EditorTerrainResourcesUI.<>f__mg$cache1;
			EditorTerrainResourcesUI.resourcesScrollBox.add(EditorTerrainResourcesUI.densitySlider);
			EditorTerrainResourcesUI.chanceSlider = new SleekSlider();
			EditorTerrainResourcesUI.chanceSlider.positionOffset_X = 200;
			EditorTerrainResourcesUI.chanceSlider.positionOffset_Y = LevelGround.resources.Length * 40 + 30;
			EditorTerrainResourcesUI.chanceSlider.sizeOffset_X = 170;
			EditorTerrainResourcesUI.chanceSlider.sizeOffset_Y = 20;
			EditorTerrainResourcesUI.chanceSlider.orientation = ESleekOrientation.HORIZONTAL;
			EditorTerrainResourcesUI.chanceSlider.addLabel(local.format("ChanceSliderLabelText"), ESleekSide.LEFT);
			SleekSlider sleekSlider2 = EditorTerrainResourcesUI.chanceSlider;
			if (EditorTerrainResourcesUI.<>f__mg$cache2 == null)
			{
				EditorTerrainResourcesUI.<>f__mg$cache2 = new Dragged(EditorTerrainResourcesUI.onDraggedChanceSlider);
			}
			sleekSlider2.onDragged = EditorTerrainResourcesUI.<>f__mg$cache2;
			EditorTerrainResourcesUI.resourcesScrollBox.add(EditorTerrainResourcesUI.chanceSlider);
			EditorTerrainResourcesUI.tree_0_Toggle = new SleekToggle();
			EditorTerrainResourcesUI.tree_0_Toggle.positionOffset_X = 200;
			EditorTerrainResourcesUI.tree_0_Toggle.positionOffset_Y = LevelGround.resources.Length * 40 + 60;
			EditorTerrainResourcesUI.tree_0_Toggle.sizeOffset_X = 40;
			EditorTerrainResourcesUI.tree_0_Toggle.sizeOffset_Y = 40;
			EditorTerrainResourcesUI.tree_0_Toggle.addLabel(local.format("Tree_0_ToggleLabelText"), ESleekSide.RIGHT);
			SleekToggle sleekToggle = EditorTerrainResourcesUI.tree_0_Toggle;
			if (EditorTerrainResourcesUI.<>f__mg$cache3 == null)
			{
				EditorTerrainResourcesUI.<>f__mg$cache3 = new Toggled(EditorTerrainResourcesUI.onToggledTree_0_Toggle);
			}
			sleekToggle.onToggled = EditorTerrainResourcesUI.<>f__mg$cache3;
			EditorTerrainResourcesUI.resourcesScrollBox.add(EditorTerrainResourcesUI.tree_0_Toggle);
			EditorTerrainResourcesUI.tree_1_Toggle = new SleekToggle();
			EditorTerrainResourcesUI.tree_1_Toggle.positionOffset_X = 200;
			EditorTerrainResourcesUI.tree_1_Toggle.positionOffset_Y = LevelGround.resources.Length * 40 + 110;
			EditorTerrainResourcesUI.tree_1_Toggle.sizeOffset_X = 40;
			EditorTerrainResourcesUI.tree_1_Toggle.sizeOffset_Y = 40;
			EditorTerrainResourcesUI.tree_1_Toggle.addLabel(local.format("Tree_1_ToggleLabelText"), ESleekSide.RIGHT);
			SleekToggle sleekToggle2 = EditorTerrainResourcesUI.tree_1_Toggle;
			if (EditorTerrainResourcesUI.<>f__mg$cache4 == null)
			{
				EditorTerrainResourcesUI.<>f__mg$cache4 = new Toggled(EditorTerrainResourcesUI.onToggledTree_1_Toggle);
			}
			sleekToggle2.onToggled = EditorTerrainResourcesUI.<>f__mg$cache4;
			EditorTerrainResourcesUI.resourcesScrollBox.add(EditorTerrainResourcesUI.tree_1_Toggle);
			EditorTerrainResourcesUI.flower_0_Toggle = new SleekToggle();
			EditorTerrainResourcesUI.flower_0_Toggle.positionOffset_X = 200;
			EditorTerrainResourcesUI.flower_0_Toggle.positionOffset_Y = LevelGround.resources.Length * 40 + 160;
			EditorTerrainResourcesUI.flower_0_Toggle.sizeOffset_X = 40;
			EditorTerrainResourcesUI.flower_0_Toggle.sizeOffset_Y = 40;
			EditorTerrainResourcesUI.flower_0_Toggle.addLabel(local.format("Flower_0_ToggleLabelText"), ESleekSide.RIGHT);
			SleekToggle sleekToggle3 = EditorTerrainResourcesUI.flower_0_Toggle;
			if (EditorTerrainResourcesUI.<>f__mg$cache5 == null)
			{
				EditorTerrainResourcesUI.<>f__mg$cache5 = new Toggled(EditorTerrainResourcesUI.onToggledFlower_0_Toggle);
			}
			sleekToggle3.onToggled = EditorTerrainResourcesUI.<>f__mg$cache5;
			EditorTerrainResourcesUI.resourcesScrollBox.add(EditorTerrainResourcesUI.flower_0_Toggle);
			EditorTerrainResourcesUI.flower_1_Toggle = new SleekToggle();
			EditorTerrainResourcesUI.flower_1_Toggle.positionOffset_X = 200;
			EditorTerrainResourcesUI.flower_1_Toggle.positionOffset_Y = LevelGround.resources.Length * 40 + 210;
			EditorTerrainResourcesUI.flower_1_Toggle.sizeOffset_X = 40;
			EditorTerrainResourcesUI.flower_1_Toggle.sizeOffset_Y = 40;
			EditorTerrainResourcesUI.flower_1_Toggle.addLabel(local.format("Flower_1_ToggleLabelText"), ESleekSide.RIGHT);
			SleekToggle sleekToggle4 = EditorTerrainResourcesUI.flower_1_Toggle;
			if (EditorTerrainResourcesUI.<>f__mg$cache6 == null)
			{
				EditorTerrainResourcesUI.<>f__mg$cache6 = new Toggled(EditorTerrainResourcesUI.onToggledFlower_1_Toggle);
			}
			sleekToggle4.onToggled = EditorTerrainResourcesUI.<>f__mg$cache6;
			EditorTerrainResourcesUI.resourcesScrollBox.add(EditorTerrainResourcesUI.flower_1_Toggle);
			EditorTerrainResourcesUI.rockToggle = new SleekToggle();
			EditorTerrainResourcesUI.rockToggle.positionOffset_X = 200;
			EditorTerrainResourcesUI.rockToggle.positionOffset_Y = LevelGround.resources.Length * 40 + 260;
			EditorTerrainResourcesUI.rockToggle.sizeOffset_X = 40;
			EditorTerrainResourcesUI.rockToggle.sizeOffset_Y = 40;
			EditorTerrainResourcesUI.rockToggle.addLabel(local.format("RockToggleLabelText"), ESleekSide.RIGHT);
			SleekToggle sleekToggle5 = EditorTerrainResourcesUI.rockToggle;
			if (EditorTerrainResourcesUI.<>f__mg$cache7 == null)
			{
				EditorTerrainResourcesUI.<>f__mg$cache7 = new Toggled(EditorTerrainResourcesUI.onToggledRockToggle);
			}
			sleekToggle5.onToggled = EditorTerrainResourcesUI.<>f__mg$cache7;
			EditorTerrainResourcesUI.resourcesScrollBox.add(EditorTerrainResourcesUI.rockToggle);
			EditorTerrainResourcesUI.roadToggle = new SleekToggle();
			EditorTerrainResourcesUI.roadToggle.positionOffset_X = 200;
			EditorTerrainResourcesUI.roadToggle.positionOffset_Y = LevelGround.resources.Length * 40 + 310;
			EditorTerrainResourcesUI.roadToggle.sizeOffset_X = 40;
			EditorTerrainResourcesUI.roadToggle.sizeOffset_Y = 40;
			EditorTerrainResourcesUI.roadToggle.addLabel(local.format("RoadToggleLabelText"), ESleekSide.RIGHT);
			SleekToggle sleekToggle6 = EditorTerrainResourcesUI.roadToggle;
			if (EditorTerrainResourcesUI.<>f__mg$cache8 == null)
			{
				EditorTerrainResourcesUI.<>f__mg$cache8 = new Toggled(EditorTerrainResourcesUI.onToggledRoadToggle);
			}
			sleekToggle6.onToggled = EditorTerrainResourcesUI.<>f__mg$cache8;
			EditorTerrainResourcesUI.resourcesScrollBox.add(EditorTerrainResourcesUI.roadToggle);
			EditorTerrainResourcesUI.snowToggle = new SleekToggle();
			EditorTerrainResourcesUI.snowToggle.positionOffset_X = 200;
			EditorTerrainResourcesUI.snowToggle.positionOffset_Y = LevelGround.resources.Length * 40 + 360;
			EditorTerrainResourcesUI.snowToggle.sizeOffset_X = 40;
			EditorTerrainResourcesUI.snowToggle.sizeOffset_Y = 40;
			EditorTerrainResourcesUI.snowToggle.addLabel(local.format("SnowToggleLabelText"), ESleekSide.RIGHT);
			SleekToggle sleekToggle7 = EditorTerrainResourcesUI.snowToggle;
			if (EditorTerrainResourcesUI.<>f__mg$cache9 == null)
			{
				EditorTerrainResourcesUI.<>f__mg$cache9 = new Toggled(EditorTerrainResourcesUI.onToggledSnowToggle);
			}
			sleekToggle7.onToggled = EditorTerrainResourcesUI.<>f__mg$cache9;
			EditorTerrainResourcesUI.resourcesScrollBox.add(EditorTerrainResourcesUI.snowToggle);
			EditorTerrainResourcesUI.selectedBox = new SleekBox();
			EditorTerrainResourcesUI.selectedBox.positionOffset_X = -200;
			EditorTerrainResourcesUI.selectedBox.positionOffset_Y = 80;
			EditorTerrainResourcesUI.selectedBox.positionScale_X = 1f;
			EditorTerrainResourcesUI.selectedBox.sizeOffset_X = 200;
			EditorTerrainResourcesUI.selectedBox.sizeOffset_Y = 30;
			EditorTerrainResourcesUI.selectedBox.addLabel(local.format("SelectionBoxLabelText"), ESleekSide.LEFT);
			EditorTerrainResourcesUI.container.add(EditorTerrainResourcesUI.selectedBox);
			EditorTerrainResourcesUI.updateSelection();
			EditorTerrainResourcesUI.bakeGlobalResourcesButton = new SleekButtonIcon((Texture2D)bundle.load("Resources"));
			EditorTerrainResourcesUI.bakeGlobalResourcesButton.positionOffset_X = -200;
			EditorTerrainResourcesUI.bakeGlobalResourcesButton.positionOffset_Y = -110;
			EditorTerrainResourcesUI.bakeGlobalResourcesButton.positionScale_X = 1f;
			EditorTerrainResourcesUI.bakeGlobalResourcesButton.positionScale_Y = 1f;
			EditorTerrainResourcesUI.bakeGlobalResourcesButton.sizeOffset_X = 200;
			EditorTerrainResourcesUI.bakeGlobalResourcesButton.sizeOffset_Y = 30;
			EditorTerrainResourcesUI.bakeGlobalResourcesButton.text = local.format("BakeGlobalResourcesButtonText");
			EditorTerrainResourcesUI.bakeGlobalResourcesButton.tooltip = local.format("BakeGlobalResourcesButtonTooltip");
			SleekButton sleekButton3 = EditorTerrainResourcesUI.bakeGlobalResourcesButton;
			if (EditorTerrainResourcesUI.<>f__mg$cacheA == null)
			{
				EditorTerrainResourcesUI.<>f__mg$cacheA = new ClickedButton(EditorTerrainResourcesUI.onClickedBakeGlobalResourcesButton);
			}
			sleekButton3.onClickedButton = EditorTerrainResourcesUI.<>f__mg$cacheA;
			EditorTerrainResourcesUI.container.add(EditorTerrainResourcesUI.bakeGlobalResourcesButton);
			EditorTerrainResourcesUI.bakeLocalResourcesButton = new SleekButtonIcon((Texture2D)bundle.load("Resources"));
			EditorTerrainResourcesUI.bakeLocalResourcesButton.positionOffset_X = -200;
			EditorTerrainResourcesUI.bakeLocalResourcesButton.positionOffset_Y = -70;
			EditorTerrainResourcesUI.bakeLocalResourcesButton.positionScale_X = 1f;
			EditorTerrainResourcesUI.bakeLocalResourcesButton.positionScale_Y = 1f;
			EditorTerrainResourcesUI.bakeLocalResourcesButton.sizeOffset_X = 200;
			EditorTerrainResourcesUI.bakeLocalResourcesButton.sizeOffset_Y = 30;
			EditorTerrainResourcesUI.bakeLocalResourcesButton.text = local.format("BakeLocalResourcesButtonText");
			EditorTerrainResourcesUI.bakeLocalResourcesButton.tooltip = local.format("BakeLocalResourcesButtonTooltip");
			SleekButton sleekButton4 = EditorTerrainResourcesUI.bakeLocalResourcesButton;
			if (EditorTerrainResourcesUI.<>f__mg$cacheB == null)
			{
				EditorTerrainResourcesUI.<>f__mg$cacheB = new ClickedButton(EditorTerrainResourcesUI.onClickedBakeLocalResourcesButton);
			}
			sleekButton4.onClickedButton = EditorTerrainResourcesUI.<>f__mg$cacheB;
			EditorTerrainResourcesUI.container.add(EditorTerrainResourcesUI.bakeLocalResourcesButton);
			EditorTerrainResourcesUI.bakeSkyboxResourcesButton = new SleekButtonIcon((Texture2D)bundle.load("Resources"));
			EditorTerrainResourcesUI.bakeSkyboxResourcesButton.positionOffset_X = -200;
			EditorTerrainResourcesUI.bakeSkyboxResourcesButton.positionOffset_Y = -30;
			EditorTerrainResourcesUI.bakeSkyboxResourcesButton.positionScale_X = 1f;
			EditorTerrainResourcesUI.bakeSkyboxResourcesButton.positionScale_Y = 1f;
			EditorTerrainResourcesUI.bakeSkyboxResourcesButton.sizeOffset_X = 200;
			EditorTerrainResourcesUI.bakeSkyboxResourcesButton.sizeOffset_Y = 30;
			EditorTerrainResourcesUI.bakeSkyboxResourcesButton.text = local.format("BakeSkyboxResourcesButtonText");
			EditorTerrainResourcesUI.bakeSkyboxResourcesButton.tooltip = local.format("BakeSkyboxResourcesButtonTooltip");
			SleekButton sleekButton5 = EditorTerrainResourcesUI.bakeSkyboxResourcesButton;
			if (EditorTerrainResourcesUI.<>f__mg$cacheC == null)
			{
				EditorTerrainResourcesUI.<>f__mg$cacheC = new ClickedButton(EditorTerrainResourcesUI.onClickedBakeSkyboxResourcesButton);
			}
			sleekButton5.onClickedButton = EditorTerrainResourcesUI.<>f__mg$cacheC;
			EditorTerrainResourcesUI.container.add(EditorTerrainResourcesUI.bakeSkyboxResourcesButton);
			EditorTerrainResourcesUI.radiusSlider = new SleekSlider();
			EditorTerrainResourcesUI.radiusSlider.positionOffset_Y = -100;
			EditorTerrainResourcesUI.radiusSlider.positionScale_Y = 1f;
			EditorTerrainResourcesUI.radiusSlider.sizeOffset_X = 200;
			EditorTerrainResourcesUI.radiusSlider.sizeOffset_Y = 20;
			EditorTerrainResourcesUI.radiusSlider.state = (float)(EditorSpawns.radius - EditorSpawns.MIN_REMOVE_SIZE) / (float)EditorSpawns.MAX_REMOVE_SIZE;
			EditorTerrainResourcesUI.radiusSlider.orientation = ESleekOrientation.HORIZONTAL;
			EditorTerrainResourcesUI.radiusSlider.addLabel(local.format("RadiusSliderLabelText"), ESleekSide.RIGHT);
			SleekSlider sleekSlider3 = EditorTerrainResourcesUI.radiusSlider;
			if (EditorTerrainResourcesUI.<>f__mg$cacheD == null)
			{
				EditorTerrainResourcesUI.<>f__mg$cacheD = new Dragged(EditorTerrainResourcesUI.onDraggedRadiusSlider);
			}
			sleekSlider3.onDragged = EditorTerrainResourcesUI.<>f__mg$cacheD;
			EditorTerrainResourcesUI.container.add(EditorTerrainResourcesUI.radiusSlider);
			EditorTerrainResourcesUI.addButton = new SleekButtonIcon((Texture2D)bundle.load("Add"));
			EditorTerrainResourcesUI.addButton.positionOffset_Y = -70;
			EditorTerrainResourcesUI.addButton.positionScale_Y = 1f;
			EditorTerrainResourcesUI.addButton.sizeOffset_X = 200;
			EditorTerrainResourcesUI.addButton.sizeOffset_Y = 30;
			EditorTerrainResourcesUI.addButton.text = local.format("AddButtonText", new object[]
			{
				ControlsSettings.tool_0
			});
			EditorTerrainResourcesUI.addButton.tooltip = local.format("AddButtonTooltip");
			SleekButton sleekButton6 = EditorTerrainResourcesUI.addButton;
			if (EditorTerrainResourcesUI.<>f__mg$cacheE == null)
			{
				EditorTerrainResourcesUI.<>f__mg$cacheE = new ClickedButton(EditorTerrainResourcesUI.onClickedAddButton);
			}
			sleekButton6.onClickedButton = EditorTerrainResourcesUI.<>f__mg$cacheE;
			EditorTerrainResourcesUI.container.add(EditorTerrainResourcesUI.addButton);
			EditorTerrainResourcesUI.removeButton = new SleekButtonIcon((Texture2D)bundle.load("Remove"));
			EditorTerrainResourcesUI.removeButton.positionOffset_Y = -30;
			EditorTerrainResourcesUI.removeButton.positionScale_Y = 1f;
			EditorTerrainResourcesUI.removeButton.sizeOffset_X = 200;
			EditorTerrainResourcesUI.removeButton.sizeOffset_Y = 30;
			EditorTerrainResourcesUI.removeButton.text = local.format("RemoveButtonText", new object[]
			{
				ControlsSettings.tool_1
			});
			EditorTerrainResourcesUI.removeButton.tooltip = local.format("RemoveButtonTooltip");
			SleekButton sleekButton7 = EditorTerrainResourcesUI.removeButton;
			if (EditorTerrainResourcesUI.<>f__mg$cacheF == null)
			{
				EditorTerrainResourcesUI.<>f__mg$cacheF = new ClickedButton(EditorTerrainResourcesUI.onClickedRemoveButton);
			}
			sleekButton7.onClickedButton = EditorTerrainResourcesUI.<>f__mg$cacheF;
			EditorTerrainResourcesUI.container.add(EditorTerrainResourcesUI.removeButton);
			bundle.unload();
		}

		public static void open()
		{
			if (EditorTerrainResourcesUI.active)
			{
				return;
			}
			EditorTerrainResourcesUI.active = true;
			EditorSpawns.isSpawning = true;
			EditorSpawns.spawnMode = ESpawnMode.ADD_RESOURCE;
			EditorTerrainResourcesUI.container.lerpPositionScale(0f, 0f, ESleekLerp.EXPONENTIAL, 20f);
		}

		public static void close()
		{
			if (!EditorTerrainResourcesUI.active)
			{
				return;
			}
			EditorTerrainResourcesUI.active = false;
			EditorSpawns.isSpawning = false;
			EditorTerrainResourcesUI.container.lerpPositionScale(1f, 0f, ESleekLerp.EXPONENTIAL, 20f);
		}

		private static void updateSelection()
		{
			if ((int)EditorSpawns.selectedResource < LevelGround.resources.Length)
			{
				GroundResource groundResource = LevelGround.resources[(int)EditorSpawns.selectedResource];
				ResourceAsset resourceAsset = (ResourceAsset)Assets.find(EAssetType.RESOURCE, groundResource.id);
				EditorTerrainResourcesUI.selectedBox.text = resourceAsset.resourceName;
				EditorTerrainResourcesUI.densitySlider.state = groundResource.density;
				EditorTerrainResourcesUI.chanceSlider.state = groundResource.chance;
				EditorTerrainResourcesUI.tree_0_Toggle.state = groundResource.isTree_0;
				EditorTerrainResourcesUI.tree_1_Toggle.state = groundResource.isTree_1;
				EditorTerrainResourcesUI.flower_0_Toggle.state = groundResource.isFlower_0;
				EditorTerrainResourcesUI.flower_1_Toggle.state = groundResource.isFlower_1;
				EditorTerrainResourcesUI.rockToggle.state = groundResource.isRock;
				EditorTerrainResourcesUI.roadToggle.state = groundResource.isRoad;
				EditorTerrainResourcesUI.snowToggle.state = groundResource.isSnow;
			}
		}

		private static void onClickedResourceButton(SleekButton button)
		{
			EditorSpawns.selectedResource = (byte)(button.positionOffset_Y / 40);
			EditorTerrainResourcesUI.updateSelection();
		}

		private static void onDraggedDensitySlider(SleekSlider slider, float state)
		{
			LevelGround.resources[(int)EditorSpawns.selectedResource].density = state;
		}

		private static void onDraggedChanceSlider(SleekSlider slider, float state)
		{
			LevelGround.resources[(int)EditorSpawns.selectedResource].chance = state;
		}

		private static void onToggledTree_0_Toggle(SleekToggle toggle, bool state)
		{
			LevelGround.resources[(int)EditorSpawns.selectedResource].isTree_0 = state;
		}

		private static void onToggledTree_1_Toggle(SleekToggle toggle, bool state)
		{
			LevelGround.resources[(int)EditorSpawns.selectedResource].isTree_1 = state;
		}

		private static void onToggledFlower_0_Toggle(SleekToggle toggle, bool state)
		{
			LevelGround.resources[(int)EditorSpawns.selectedResource].isFlower_0 = state;
		}

		private static void onToggledFlower_1_Toggle(SleekToggle toggle, bool state)
		{
			LevelGround.resources[(int)EditorSpawns.selectedResource].isFlower_1 = state;
		}

		private static void onToggledRockToggle(SleekToggle toggle, bool state)
		{
			LevelGround.resources[(int)EditorSpawns.selectedResource].isRock = state;
		}

		private static void onToggledRoadToggle(SleekToggle toggle, bool state)
		{
			LevelGround.resources[(int)EditorSpawns.selectedResource].isRoad = state;
		}

		private static void onToggledSnowToggle(SleekToggle toggle, bool state)
		{
			LevelGround.resources[(int)EditorSpawns.selectedResource].isSnow = state;
		}

		private static void onClickedBakeGlobalResourcesButton(SleekButton button)
		{
			LevelGround.bakeGlobalResources();
		}

		private static void onClickedBakeLocalResourcesButton(SleekButton button)
		{
			LevelGround.bakeLocalResources();
		}

		private static void onClickedBakeSkyboxResourcesButton(SleekButton button)
		{
			LevelGround.bakeSkyboxResources();
		}

		private static void onDraggedRadiusSlider(SleekSlider slider, float state)
		{
			EditorSpawns.radius = (byte)((float)EditorSpawns.MIN_REMOVE_SIZE + state * (float)EditorSpawns.MAX_REMOVE_SIZE);
		}

		private static void onClickedAddButton(SleekButton button)
		{
			EditorSpawns.spawnMode = ESpawnMode.ADD_RESOURCE;
		}

		private static void onClickedRemoveButton(SleekButton button)
		{
			EditorSpawns.spawnMode = ESpawnMode.REMOVE_RESOURCE;
		}

		private static Sleek container;

		public static bool active;

		private static SleekScrollBox resourcesScrollBox;

		private static SleekButtonIcon bakeGlobalResourcesButton;

		private static SleekButtonIcon bakeLocalResourcesButton;

		private static SleekButtonIcon bakeSkyboxResourcesButton;

		private static SleekBox selectedBox;

		private static SleekSlider densitySlider;

		private static SleekSlider chanceSlider;

		private static SleekToggle tree_0_Toggle;

		private static SleekToggle tree_1_Toggle;

		private static SleekToggle flower_0_Toggle;

		private static SleekToggle flower_1_Toggle;

		private static SleekToggle rockToggle;

		private static SleekToggle roadToggle;

		private static SleekToggle snowToggle;

		private static SleekSlider radiusSlider;

		private static SleekButtonIcon addButton;

		private static SleekButtonIcon removeButton;

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

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cacheB;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cacheC;

		[CompilerGenerated]
		private static Dragged <>f__mg$cacheD;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cacheE;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cacheF;
	}
}
