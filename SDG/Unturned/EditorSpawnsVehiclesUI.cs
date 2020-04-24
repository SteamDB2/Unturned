using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace SDG.Unturned
{
	public class EditorSpawnsVehiclesUI
	{
		public EditorSpawnsVehiclesUI()
		{
			Local local = Localization.read("/Editor/EditorSpawnsVehicles.dat");
			Bundle bundle = Bundles.getBundle("/Bundles/Textures/Edit/Icons/EditorSpawnsVehicles/EditorSpawnsVehicles.unity3d");
			EditorSpawnsVehiclesUI.container = new Sleek();
			EditorSpawnsVehiclesUI.container.positionOffset_X = 10;
			EditorSpawnsVehiclesUI.container.positionOffset_Y = 10;
			EditorSpawnsVehiclesUI.container.positionScale_X = 1f;
			EditorSpawnsVehiclesUI.container.sizeOffset_X = -20;
			EditorSpawnsVehiclesUI.container.sizeOffset_Y = -20;
			EditorSpawnsVehiclesUI.container.sizeScale_X = 1f;
			EditorSpawnsVehiclesUI.container.sizeScale_Y = 1f;
			EditorUI.window.add(EditorSpawnsVehiclesUI.container);
			EditorSpawnsVehiclesUI.active = false;
			EditorSpawnsVehiclesUI.tableScrollBox = new SleekScrollBox();
			EditorSpawnsVehiclesUI.tableScrollBox.positionOffset_X = -470;
			EditorSpawnsVehiclesUI.tableScrollBox.positionOffset_Y = 120;
			EditorSpawnsVehiclesUI.tableScrollBox.positionScale_X = 1f;
			EditorSpawnsVehiclesUI.tableScrollBox.sizeOffset_X = 470;
			EditorSpawnsVehiclesUI.tableScrollBox.sizeOffset_Y = 200;
			EditorSpawnsVehiclesUI.container.add(EditorSpawnsVehiclesUI.tableScrollBox);
			EditorSpawnsVehiclesUI.tableNameField = new SleekField();
			EditorSpawnsVehiclesUI.tableNameField.positionOffset_X = -230;
			EditorSpawnsVehiclesUI.tableNameField.positionOffset_Y = 330;
			EditorSpawnsVehiclesUI.tableNameField.positionScale_X = 1f;
			EditorSpawnsVehiclesUI.tableNameField.sizeOffset_X = 230;
			EditorSpawnsVehiclesUI.tableNameField.sizeOffset_Y = 30;
			EditorSpawnsVehiclesUI.tableNameField.maxLength = 64;
			EditorSpawnsVehiclesUI.tableNameField.addLabel(local.format("TableNameFieldLabelText"), ESleekSide.LEFT);
			SleekField sleekField = EditorSpawnsVehiclesUI.tableNameField;
			Delegate onTyped = sleekField.onTyped;
			if (EditorSpawnsVehiclesUI.<>f__mg$cache4 == null)
			{
				EditorSpawnsVehiclesUI.<>f__mg$cache4 = new Typed(EditorSpawnsVehiclesUI.onTypedNameField);
			}
			sleekField.onTyped = (Typed)Delegate.Combine(onTyped, EditorSpawnsVehiclesUI.<>f__mg$cache4);
			EditorSpawnsVehiclesUI.container.add(EditorSpawnsVehiclesUI.tableNameField);
			EditorSpawnsVehiclesUI.addTableButton = new SleekButtonIcon((Texture2D)bundle.load("Add"));
			EditorSpawnsVehiclesUI.addTableButton.positionOffset_X = -230;
			EditorSpawnsVehiclesUI.addTableButton.positionOffset_Y = 370;
			EditorSpawnsVehiclesUI.addTableButton.positionScale_X = 1f;
			EditorSpawnsVehiclesUI.addTableButton.sizeOffset_X = 110;
			EditorSpawnsVehiclesUI.addTableButton.sizeOffset_Y = 30;
			EditorSpawnsVehiclesUI.addTableButton.text = local.format("AddTableButtonText");
			EditorSpawnsVehiclesUI.addTableButton.tooltip = local.format("AddTableButtonTooltip");
			SleekButton sleekButton = EditorSpawnsVehiclesUI.addTableButton;
			if (EditorSpawnsVehiclesUI.<>f__mg$cache5 == null)
			{
				EditorSpawnsVehiclesUI.<>f__mg$cache5 = new ClickedButton(EditorSpawnsVehiclesUI.onClickedAddTableButton);
			}
			sleekButton.onClickedButton = EditorSpawnsVehiclesUI.<>f__mg$cache5;
			EditorSpawnsVehiclesUI.container.add(EditorSpawnsVehiclesUI.addTableButton);
			EditorSpawnsVehiclesUI.removeTableButton = new SleekButtonIcon((Texture2D)bundle.load("Remove"));
			EditorSpawnsVehiclesUI.removeTableButton.positionOffset_X = -110;
			EditorSpawnsVehiclesUI.removeTableButton.positionOffset_Y = 370;
			EditorSpawnsVehiclesUI.removeTableButton.positionScale_X = 1f;
			EditorSpawnsVehiclesUI.removeTableButton.sizeOffset_X = 110;
			EditorSpawnsVehiclesUI.removeTableButton.sizeOffset_Y = 30;
			EditorSpawnsVehiclesUI.removeTableButton.text = local.format("RemoveTableButtonText");
			EditorSpawnsVehiclesUI.removeTableButton.tooltip = local.format("RemoveTableButtonTooltip");
			SleekButton sleekButton2 = EditorSpawnsVehiclesUI.removeTableButton;
			if (EditorSpawnsVehiclesUI.<>f__mg$cache6 == null)
			{
				EditorSpawnsVehiclesUI.<>f__mg$cache6 = new ClickedButton(EditorSpawnsVehiclesUI.onClickedRemoveTableButton);
			}
			sleekButton2.onClickedButton = EditorSpawnsVehiclesUI.<>f__mg$cache6;
			EditorSpawnsVehiclesUI.container.add(EditorSpawnsVehiclesUI.removeTableButton);
			EditorSpawnsVehiclesUI.updateTables();
			EditorSpawnsVehiclesUI.spawnsScrollBox = new SleekScrollBox();
			EditorSpawnsVehiclesUI.spawnsScrollBox.positionOffset_X = -470;
			EditorSpawnsVehiclesUI.spawnsScrollBox.positionOffset_Y = 410;
			EditorSpawnsVehiclesUI.spawnsScrollBox.positionScale_X = 1f;
			EditorSpawnsVehiclesUI.spawnsScrollBox.sizeOffset_X = 470;
			EditorSpawnsVehiclesUI.spawnsScrollBox.sizeOffset_Y = -410;
			EditorSpawnsVehiclesUI.spawnsScrollBox.sizeScale_Y = 1f;
			EditorSpawnsVehiclesUI.spawnsScrollBox.area = new Rect(0f, 0f, 5f, 1000f);
			EditorSpawnsVehiclesUI.container.add(EditorSpawnsVehiclesUI.spawnsScrollBox);
			EditorSpawnsVehiclesUI.tableColorPicker = new SleekColorPicker();
			EditorSpawnsVehiclesUI.tableColorPicker.positionOffset_X = 200;
			SleekColorPicker sleekColorPicker = EditorSpawnsVehiclesUI.tableColorPicker;
			if (EditorSpawnsVehiclesUI.<>f__mg$cache7 == null)
			{
				EditorSpawnsVehiclesUI.<>f__mg$cache7 = new ColorPicked(EditorSpawnsVehiclesUI.onVehicleColorPicked);
			}
			sleekColorPicker.onColorPicked = EditorSpawnsVehiclesUI.<>f__mg$cache7;
			EditorSpawnsVehiclesUI.spawnsScrollBox.add(EditorSpawnsVehiclesUI.tableColorPicker);
			EditorSpawnsVehiclesUI.tableIDField = new SleekUInt16Field();
			EditorSpawnsVehiclesUI.tableIDField.positionOffset_X = 240;
			EditorSpawnsVehiclesUI.tableIDField.positionOffset_Y = 130;
			EditorSpawnsVehiclesUI.tableIDField.sizeOffset_X = 200;
			EditorSpawnsVehiclesUI.tableIDField.sizeOffset_Y = 30;
			SleekUInt16Field sleekUInt16Field = EditorSpawnsVehiclesUI.tableIDField;
			if (EditorSpawnsVehiclesUI.<>f__mg$cache8 == null)
			{
				EditorSpawnsVehiclesUI.<>f__mg$cache8 = new TypedUInt16(EditorSpawnsVehiclesUI.onTableIDFieldTyped);
			}
			sleekUInt16Field.onTypedUInt16 = EditorSpawnsVehiclesUI.<>f__mg$cache8;
			EditorSpawnsVehiclesUI.tableIDField.addLabel(local.format("TableIDFieldLabelText"), ESleekSide.LEFT);
			EditorSpawnsVehiclesUI.spawnsScrollBox.add(EditorSpawnsVehiclesUI.tableIDField);
			EditorSpawnsVehiclesUI.tierNameField = new SleekField();
			EditorSpawnsVehiclesUI.tierNameField.positionOffset_X = 240;
			EditorSpawnsVehiclesUI.tierNameField.sizeOffset_X = 200;
			EditorSpawnsVehiclesUI.tierNameField.sizeOffset_Y = 30;
			EditorSpawnsVehiclesUI.tierNameField.maxLength = 64;
			EditorSpawnsVehiclesUI.tierNameField.addLabel(local.format("TierNameFieldLabelText"), ESleekSide.LEFT);
			SleekField sleekField2 = EditorSpawnsVehiclesUI.tierNameField;
			if (EditorSpawnsVehiclesUI.<>f__mg$cache9 == null)
			{
				EditorSpawnsVehiclesUI.<>f__mg$cache9 = new Typed(EditorSpawnsVehiclesUI.onTypedTierNameField);
			}
			sleekField2.onTyped = EditorSpawnsVehiclesUI.<>f__mg$cache9;
			EditorSpawnsVehiclesUI.spawnsScrollBox.add(EditorSpawnsVehiclesUI.tierNameField);
			EditorSpawnsVehiclesUI.addTierButton = new SleekButtonIcon((Texture2D)bundle.load("Add"));
			EditorSpawnsVehiclesUI.addTierButton.positionOffset_X = 240;
			EditorSpawnsVehiclesUI.addTierButton.sizeOffset_X = 95;
			EditorSpawnsVehiclesUI.addTierButton.sizeOffset_Y = 30;
			EditorSpawnsVehiclesUI.addTierButton.text = local.format("AddTierButtonText");
			EditorSpawnsVehiclesUI.addTierButton.tooltip = local.format("AddTierButtonTooltip");
			SleekButton sleekButton3 = EditorSpawnsVehiclesUI.addTierButton;
			if (EditorSpawnsVehiclesUI.<>f__mg$cacheA == null)
			{
				EditorSpawnsVehiclesUI.<>f__mg$cacheA = new ClickedButton(EditorSpawnsVehiclesUI.onClickedAddTierButton);
			}
			sleekButton3.onClickedButton = EditorSpawnsVehiclesUI.<>f__mg$cacheA;
			EditorSpawnsVehiclesUI.spawnsScrollBox.add(EditorSpawnsVehiclesUI.addTierButton);
			EditorSpawnsVehiclesUI.removeTierButton = new SleekButtonIcon((Texture2D)bundle.load("Remove"));
			EditorSpawnsVehiclesUI.removeTierButton.positionOffset_X = 345;
			EditorSpawnsVehiclesUI.removeTierButton.sizeOffset_X = 95;
			EditorSpawnsVehiclesUI.removeTierButton.sizeOffset_Y = 30;
			EditorSpawnsVehiclesUI.removeTierButton.text = local.format("RemoveTierButtonText");
			EditorSpawnsVehiclesUI.removeTierButton.tooltip = local.format("RemoveTierButtonTooltip");
			SleekButton sleekButton4 = EditorSpawnsVehiclesUI.removeTierButton;
			if (EditorSpawnsVehiclesUI.<>f__mg$cacheB == null)
			{
				EditorSpawnsVehiclesUI.<>f__mg$cacheB = new ClickedButton(EditorSpawnsVehiclesUI.onClickedRemoveTierButton);
			}
			sleekButton4.onClickedButton = EditorSpawnsVehiclesUI.<>f__mg$cacheB;
			EditorSpawnsVehiclesUI.spawnsScrollBox.add(EditorSpawnsVehiclesUI.removeTierButton);
			EditorSpawnsVehiclesUI.vehicleIDField = new SleekUInt16Field();
			EditorSpawnsVehiclesUI.vehicleIDField.positionOffset_X = 240;
			EditorSpawnsVehiclesUI.vehicleIDField.sizeOffset_X = 200;
			EditorSpawnsVehiclesUI.vehicleIDField.sizeOffset_Y = 30;
			EditorSpawnsVehiclesUI.vehicleIDField.addLabel(local.format("VehicleIDFieldLabelText"), ESleekSide.LEFT);
			EditorSpawnsVehiclesUI.spawnsScrollBox.add(EditorSpawnsVehiclesUI.vehicleIDField);
			EditorSpawnsVehiclesUI.addVehicleButton = new SleekButtonIcon((Texture2D)bundle.load("Add"));
			EditorSpawnsVehiclesUI.addVehicleButton.positionOffset_X = 240;
			EditorSpawnsVehiclesUI.addVehicleButton.sizeOffset_X = 95;
			EditorSpawnsVehiclesUI.addVehicleButton.sizeOffset_Y = 30;
			EditorSpawnsVehiclesUI.addVehicleButton.text = local.format("AddVehicleButtonText");
			EditorSpawnsVehiclesUI.addVehicleButton.tooltip = local.format("AddVehicleButtonTooltip");
			SleekButton sleekButton5 = EditorSpawnsVehiclesUI.addVehicleButton;
			if (EditorSpawnsVehiclesUI.<>f__mg$cacheC == null)
			{
				EditorSpawnsVehiclesUI.<>f__mg$cacheC = new ClickedButton(EditorSpawnsVehiclesUI.onClickedAddVehicleButton);
			}
			sleekButton5.onClickedButton = EditorSpawnsVehiclesUI.<>f__mg$cacheC;
			EditorSpawnsVehiclesUI.spawnsScrollBox.add(EditorSpawnsVehiclesUI.addVehicleButton);
			EditorSpawnsVehiclesUI.removeVehicleButton = new SleekButtonIcon((Texture2D)bundle.load("Remove"));
			EditorSpawnsVehiclesUI.removeVehicleButton.positionOffset_X = 345;
			EditorSpawnsVehiclesUI.removeVehicleButton.sizeOffset_X = 95;
			EditorSpawnsVehiclesUI.removeVehicleButton.sizeOffset_Y = 30;
			EditorSpawnsVehiclesUI.removeVehicleButton.text = local.format("RemoveVehicleButtonText");
			EditorSpawnsVehiclesUI.removeVehicleButton.tooltip = local.format("RemoveVehicleButtonTooltip");
			SleekButton sleekButton6 = EditorSpawnsVehiclesUI.removeVehicleButton;
			if (EditorSpawnsVehiclesUI.<>f__mg$cacheD == null)
			{
				EditorSpawnsVehiclesUI.<>f__mg$cacheD = new ClickedButton(EditorSpawnsVehiclesUI.onClickedRemoveVehicleButton);
			}
			sleekButton6.onClickedButton = EditorSpawnsVehiclesUI.<>f__mg$cacheD;
			EditorSpawnsVehiclesUI.spawnsScrollBox.add(EditorSpawnsVehiclesUI.removeVehicleButton);
			EditorSpawnsVehiclesUI.selectedBox = new SleekBox();
			EditorSpawnsVehiclesUI.selectedBox.positionOffset_X = -230;
			EditorSpawnsVehiclesUI.selectedBox.positionOffset_Y = 80;
			EditorSpawnsVehiclesUI.selectedBox.positionScale_X = 1f;
			EditorSpawnsVehiclesUI.selectedBox.sizeOffset_X = 230;
			EditorSpawnsVehiclesUI.selectedBox.sizeOffset_Y = 30;
			EditorSpawnsVehiclesUI.selectedBox.addLabel(local.format("SelectionBoxLabelText"), ESleekSide.LEFT);
			EditorSpawnsVehiclesUI.container.add(EditorSpawnsVehiclesUI.selectedBox);
			EditorSpawnsVehiclesUI.updateSelection();
			EditorSpawnsVehiclesUI.radiusSlider = new SleekSlider();
			EditorSpawnsVehiclesUI.radiusSlider.positionOffset_Y = -130;
			EditorSpawnsVehiclesUI.radiusSlider.positionScale_Y = 1f;
			EditorSpawnsVehiclesUI.radiusSlider.sizeOffset_X = 200;
			EditorSpawnsVehiclesUI.radiusSlider.sizeOffset_Y = 20;
			EditorSpawnsVehiclesUI.radiusSlider.state = (float)(EditorSpawns.radius - EditorSpawns.MIN_REMOVE_SIZE) / (float)EditorSpawns.MAX_REMOVE_SIZE;
			EditorSpawnsVehiclesUI.radiusSlider.orientation = ESleekOrientation.HORIZONTAL;
			EditorSpawnsVehiclesUI.radiusSlider.addLabel(local.format("RadiusSliderLabelText"), ESleekSide.RIGHT);
			SleekSlider sleekSlider = EditorSpawnsVehiclesUI.radiusSlider;
			if (EditorSpawnsVehiclesUI.<>f__mg$cacheE == null)
			{
				EditorSpawnsVehiclesUI.<>f__mg$cacheE = new Dragged(EditorSpawnsVehiclesUI.onDraggedRadiusSlider);
			}
			sleekSlider.onDragged = EditorSpawnsVehiclesUI.<>f__mg$cacheE;
			EditorSpawnsVehiclesUI.container.add(EditorSpawnsVehiclesUI.radiusSlider);
			EditorSpawnsVehiclesUI.rotationSlider = new SleekSlider();
			EditorSpawnsVehiclesUI.rotationSlider.positionOffset_Y = -100;
			EditorSpawnsVehiclesUI.rotationSlider.positionScale_Y = 1f;
			EditorSpawnsVehiclesUI.rotationSlider.sizeOffset_X = 200;
			EditorSpawnsVehiclesUI.rotationSlider.sizeOffset_Y = 20;
			EditorSpawnsVehiclesUI.rotationSlider.state = EditorSpawns.rotation / 360f;
			EditorSpawnsVehiclesUI.rotationSlider.orientation = ESleekOrientation.HORIZONTAL;
			EditorSpawnsVehiclesUI.rotationSlider.addLabel(local.format("RotationSliderLabelText"), ESleekSide.RIGHT);
			SleekSlider sleekSlider2 = EditorSpawnsVehiclesUI.rotationSlider;
			if (EditorSpawnsVehiclesUI.<>f__mg$cacheF == null)
			{
				EditorSpawnsVehiclesUI.<>f__mg$cacheF = new Dragged(EditorSpawnsVehiclesUI.onDraggedRotationSlider);
			}
			sleekSlider2.onDragged = EditorSpawnsVehiclesUI.<>f__mg$cacheF;
			EditorSpawnsVehiclesUI.container.add(EditorSpawnsVehiclesUI.rotationSlider);
			EditorSpawnsVehiclesUI.addButton = new SleekButtonIcon((Texture2D)bundle.load("Add"));
			EditorSpawnsVehiclesUI.addButton.positionOffset_Y = -70;
			EditorSpawnsVehiclesUI.addButton.positionScale_Y = 1f;
			EditorSpawnsVehiclesUI.addButton.sizeOffset_X = 200;
			EditorSpawnsVehiclesUI.addButton.sizeOffset_Y = 30;
			EditorSpawnsVehiclesUI.addButton.text = local.format("AddButtonText", new object[]
			{
				ControlsSettings.tool_0
			});
			EditorSpawnsVehiclesUI.addButton.tooltip = local.format("AddButtonTooltip");
			SleekButton sleekButton7 = EditorSpawnsVehiclesUI.addButton;
			if (EditorSpawnsVehiclesUI.<>f__mg$cache10 == null)
			{
				EditorSpawnsVehiclesUI.<>f__mg$cache10 = new ClickedButton(EditorSpawnsVehiclesUI.onClickedAddButton);
			}
			sleekButton7.onClickedButton = EditorSpawnsVehiclesUI.<>f__mg$cache10;
			EditorSpawnsVehiclesUI.container.add(EditorSpawnsVehiclesUI.addButton);
			EditorSpawnsVehiclesUI.removeButton = new SleekButtonIcon((Texture2D)bundle.load("Remove"));
			EditorSpawnsVehiclesUI.removeButton.positionOffset_Y = -30;
			EditorSpawnsVehiclesUI.removeButton.positionScale_Y = 1f;
			EditorSpawnsVehiclesUI.removeButton.sizeOffset_X = 200;
			EditorSpawnsVehiclesUI.removeButton.sizeOffset_Y = 30;
			EditorSpawnsVehiclesUI.removeButton.text = local.format("RemoveButtonText", new object[]
			{
				ControlsSettings.tool_1
			});
			EditorSpawnsVehiclesUI.removeButton.tooltip = local.format("RemoveButtonTooltip");
			SleekButton sleekButton8 = EditorSpawnsVehiclesUI.removeButton;
			if (EditorSpawnsVehiclesUI.<>f__mg$cache11 == null)
			{
				EditorSpawnsVehiclesUI.<>f__mg$cache11 = new ClickedButton(EditorSpawnsVehiclesUI.onClickedRemoveButton);
			}
			sleekButton8.onClickedButton = EditorSpawnsVehiclesUI.<>f__mg$cache11;
			EditorSpawnsVehiclesUI.container.add(EditorSpawnsVehiclesUI.removeButton);
			bundle.unload();
		}

		public static void open()
		{
			if (EditorSpawnsVehiclesUI.active)
			{
				return;
			}
			EditorSpawnsVehiclesUI.active = true;
			EditorSpawns.isSpawning = true;
			EditorSpawns.spawnMode = ESpawnMode.ADD_VEHICLE;
			EditorSpawnsVehiclesUI.container.lerpPositionScale(0f, 0f, ESleekLerp.EXPONENTIAL, 20f);
		}

		public static void close()
		{
			if (!EditorSpawnsVehiclesUI.active)
			{
				return;
			}
			EditorSpawnsVehiclesUI.active = false;
			EditorSpawns.isSpawning = false;
			EditorSpawnsVehiclesUI.container.lerpPositionScale(1f, 0f, ESleekLerp.EXPONENTIAL, 20f);
		}

		public static void updateTables()
		{
			if (EditorSpawnsVehiclesUI.tableButtons != null)
			{
				for (int i = 0; i < EditorSpawnsVehiclesUI.tableButtons.Length; i++)
				{
					EditorSpawnsVehiclesUI.tableScrollBox.remove(EditorSpawnsVehiclesUI.tableButtons[i]);
				}
			}
			EditorSpawnsVehiclesUI.tableButtons = new SleekButton[LevelVehicles.tables.Count];
			EditorSpawnsVehiclesUI.tableScrollBox.area = new Rect(0f, 0f, 5f, (float)(EditorSpawnsVehiclesUI.tableButtons.Length * 40 - 10));
			for (int j = 0; j < EditorSpawnsVehiclesUI.tableButtons.Length; j++)
			{
				SleekButton sleekButton = new SleekButton();
				sleekButton.positionOffset_X = 240;
				sleekButton.positionOffset_Y = j * 40;
				sleekButton.sizeOffset_X = 200;
				sleekButton.sizeOffset_Y = 30;
				sleekButton.text = j + " " + LevelVehicles.tables[j].name;
				SleekButton sleekButton2 = sleekButton;
				if (EditorSpawnsVehiclesUI.<>f__mg$cache0 == null)
				{
					EditorSpawnsVehiclesUI.<>f__mg$cache0 = new ClickedButton(EditorSpawnsVehiclesUI.onClickedTableButton);
				}
				sleekButton2.onClickedButton = EditorSpawnsVehiclesUI.<>f__mg$cache0;
				EditorSpawnsVehiclesUI.tableScrollBox.add(sleekButton);
				EditorSpawnsVehiclesUI.tableButtons[j] = sleekButton;
			}
		}

		public static void updateSelection()
		{
			if ((int)EditorSpawns.selectedVehicle < LevelVehicles.tables.Count)
			{
				VehicleTable vehicleTable = LevelVehicles.tables[(int)EditorSpawns.selectedVehicle];
				EditorSpawnsVehiclesUI.selectedBox.text = vehicleTable.name;
				EditorSpawnsVehiclesUI.tableNameField.text = vehicleTable.name;
				EditorSpawnsVehiclesUI.tableIDField.state = vehicleTable.tableID;
				EditorSpawnsVehiclesUI.tableColorPicker.state = vehicleTable.color;
				if (EditorSpawnsVehiclesUI.tierButtons != null)
				{
					for (int i = 0; i < EditorSpawnsVehiclesUI.tierButtons.Length; i++)
					{
						EditorSpawnsVehiclesUI.spawnsScrollBox.remove(EditorSpawnsVehiclesUI.tierButtons[i]);
					}
				}
				EditorSpawnsVehiclesUI.tierButtons = new SleekButton[vehicleTable.tiers.Count];
				for (int j = 0; j < EditorSpawnsVehiclesUI.tierButtons.Length; j++)
				{
					VehicleTier vehicleTier = vehicleTable.tiers[j];
					SleekButton sleekButton = new SleekButton();
					sleekButton.positionOffset_X = 240;
					sleekButton.positionOffset_Y = 170 + j * 70;
					sleekButton.sizeOffset_X = 200;
					sleekButton.sizeOffset_Y = 30;
					sleekButton.text = vehicleTier.name;
					SleekButton sleekButton2 = sleekButton;
					if (EditorSpawnsVehiclesUI.<>f__mg$cache1 == null)
					{
						EditorSpawnsVehiclesUI.<>f__mg$cache1 = new ClickedButton(EditorSpawnsVehiclesUI.onClickedTierButton);
					}
					sleekButton2.onClickedButton = EditorSpawnsVehiclesUI.<>f__mg$cache1;
					EditorSpawnsVehiclesUI.spawnsScrollBox.add(sleekButton);
					SleekSlider sleekSlider = new SleekSlider();
					sleekSlider.positionOffset_Y = 40;
					sleekSlider.sizeOffset_X = 200;
					sleekSlider.sizeOffset_Y = 20;
					sleekSlider.orientation = ESleekOrientation.HORIZONTAL;
					sleekSlider.state = vehicleTier.chance;
					sleekSlider.addLabel(Mathf.RoundToInt(vehicleTier.chance * 100f) + "%", ESleekSide.LEFT);
					SleekSlider sleekSlider2 = sleekSlider;
					if (EditorSpawnsVehiclesUI.<>f__mg$cache2 == null)
					{
						EditorSpawnsVehiclesUI.<>f__mg$cache2 = new Dragged(EditorSpawnsVehiclesUI.onDraggedChanceSlider);
					}
					sleekSlider2.onDragged = EditorSpawnsVehiclesUI.<>f__mg$cache2;
					sleekButton.add(sleekSlider);
					EditorSpawnsVehiclesUI.tierButtons[j] = sleekButton;
				}
				EditorSpawnsVehiclesUI.tierNameField.positionOffset_Y = 170 + EditorSpawnsVehiclesUI.tierButtons.Length * 70;
				EditorSpawnsVehiclesUI.addTierButton.positionOffset_Y = 170 + EditorSpawnsVehiclesUI.tierButtons.Length * 70 + 40;
				EditorSpawnsVehiclesUI.removeTierButton.positionOffset_Y = 170 + EditorSpawnsVehiclesUI.tierButtons.Length * 70 + 40;
				if (EditorSpawnsVehiclesUI.vehicleButtons != null)
				{
					for (int k = 0; k < EditorSpawnsVehiclesUI.vehicleButtons.Length; k++)
					{
						EditorSpawnsVehiclesUI.spawnsScrollBox.remove(EditorSpawnsVehiclesUI.vehicleButtons[k]);
					}
				}
				if ((int)EditorSpawnsVehiclesUI.selectedTier < vehicleTable.tiers.Count)
				{
					EditorSpawnsVehiclesUI.tierNameField.text = vehicleTable.tiers[(int)EditorSpawnsVehiclesUI.selectedTier].name;
					EditorSpawnsVehiclesUI.vehicleButtons = new SleekButton[vehicleTable.tiers[(int)EditorSpawnsVehiclesUI.selectedTier].table.Count];
					for (int l = 0; l < EditorSpawnsVehiclesUI.vehicleButtons.Length; l++)
					{
						SleekButton sleekButton3 = new SleekButton();
						sleekButton3.positionOffset_X = 240;
						sleekButton3.positionOffset_Y = 170 + EditorSpawnsVehiclesUI.tierButtons.Length * 70 + 80 + l * 40;
						sleekButton3.sizeOffset_X = 200;
						sleekButton3.sizeOffset_Y = 30;
						VehicleAsset vehicleAsset = (VehicleAsset)Assets.find(EAssetType.VEHICLE, vehicleTable.tiers[(int)EditorSpawnsVehiclesUI.selectedTier].table[l].vehicle);
						string str = "?";
						if (vehicleAsset != null)
						{
							if (string.IsNullOrEmpty(vehicleAsset.vehicleName))
							{
								str = vehicleAsset.name;
							}
							else
							{
								str = vehicleAsset.vehicleName;
							}
						}
						sleekButton3.text = vehicleTable.tiers[(int)EditorSpawnsVehiclesUI.selectedTier].table[l].vehicle.ToString() + " " + str;
						SleekButton sleekButton4 = sleekButton3;
						if (EditorSpawnsVehiclesUI.<>f__mg$cache3 == null)
						{
							EditorSpawnsVehiclesUI.<>f__mg$cache3 = new ClickedButton(EditorSpawnsVehiclesUI.onClickVehicleButton);
						}
						sleekButton4.onClickedButton = EditorSpawnsVehiclesUI.<>f__mg$cache3;
						EditorSpawnsVehiclesUI.spawnsScrollBox.add(sleekButton3);
						EditorSpawnsVehiclesUI.vehicleButtons[l] = sleekButton3;
					}
				}
				else
				{
					EditorSpawnsVehiclesUI.tierNameField.text = string.Empty;
					EditorSpawnsVehiclesUI.vehicleButtons = new SleekButton[0];
				}
				EditorSpawnsVehiclesUI.vehicleIDField.positionOffset_Y = 170 + EditorSpawnsVehiclesUI.tierButtons.Length * 70 + 80 + EditorSpawnsVehiclesUI.vehicleButtons.Length * 40;
				EditorSpawnsVehiclesUI.addVehicleButton.positionOffset_Y = 170 + EditorSpawnsVehiclesUI.tierButtons.Length * 70 + 80 + EditorSpawnsVehiclesUI.vehicleButtons.Length * 40 + 40;
				EditorSpawnsVehiclesUI.removeVehicleButton.positionOffset_Y = 170 + EditorSpawnsVehiclesUI.tierButtons.Length * 70 + 80 + EditorSpawnsVehiclesUI.vehicleButtons.Length * 40 + 40;
				EditorSpawnsVehiclesUI.spawnsScrollBox.area = new Rect(0f, 0f, 5f, (float)(170 + EditorSpawnsVehiclesUI.tierButtons.Length * 70 + 80 + EditorSpawnsVehiclesUI.vehicleButtons.Length * 40 + 70));
			}
			else
			{
				EditorSpawnsVehiclesUI.selectedBox.text = string.Empty;
				EditorSpawnsVehiclesUI.tableNameField.text = string.Empty;
				EditorSpawnsVehiclesUI.tableIDField.state = 0;
				EditorSpawnsVehiclesUI.tableColorPicker.state = Color.white;
				if (EditorSpawnsVehiclesUI.tierButtons != null)
				{
					for (int m = 0; m < EditorSpawnsVehiclesUI.tierButtons.Length; m++)
					{
						EditorSpawnsVehiclesUI.spawnsScrollBox.remove(EditorSpawnsVehiclesUI.tierButtons[m]);
					}
				}
				EditorSpawnsVehiclesUI.tierButtons = null;
				EditorSpawnsVehiclesUI.tierNameField.text = string.Empty;
				EditorSpawnsVehiclesUI.tierNameField.positionOffset_Y = 170;
				EditorSpawnsVehiclesUI.addTierButton.positionOffset_Y = 210;
				EditorSpawnsVehiclesUI.removeTierButton.positionOffset_Y = 210;
				if (EditorSpawnsVehiclesUI.vehicleButtons != null)
				{
					for (int n = 0; n < EditorSpawnsVehiclesUI.vehicleButtons.Length; n++)
					{
						EditorSpawnsVehiclesUI.spawnsScrollBox.remove(EditorSpawnsVehiclesUI.vehicleButtons[n]);
					}
				}
				EditorSpawnsVehiclesUI.vehicleButtons = null;
				EditorSpawnsVehiclesUI.vehicleIDField.positionOffset_Y = 250;
				EditorSpawnsVehiclesUI.addVehicleButton.positionOffset_Y = 290;
				EditorSpawnsVehiclesUI.removeVehicleButton.positionOffset_Y = 290;
				EditorSpawnsVehiclesUI.spawnsScrollBox.area = new Rect(0f, 0f, 5f, 320f);
			}
		}

		private static void onClickedTableButton(SleekButton button)
		{
			if (EditorSpawns.selectedVehicle != (byte)(button.positionOffset_Y / 40))
			{
				EditorSpawns.selectedVehicle = (byte)(button.positionOffset_Y / 40);
				EditorSpawns.vehicleSpawn.GetComponent<Renderer>().material.color = LevelVehicles.tables[(int)EditorSpawns.selectedVehicle].color;
				EditorSpawns.vehicleSpawn.FindChild("Arrow").GetComponent<Renderer>().material.color = LevelVehicles.tables[(int)EditorSpawns.selectedVehicle].color;
			}
			else
			{
				EditorSpawns.selectedVehicle = byte.MaxValue;
				EditorSpawns.vehicleSpawn.GetComponent<Renderer>().material.color = Color.white;
				EditorSpawns.vehicleSpawn.FindChild("Arrow").GetComponent<Renderer>().material.color = Color.white;
			}
			EditorSpawnsVehiclesUI.updateSelection();
		}

		private static void onVehicleColorPicked(SleekColorPicker picker, Color color)
		{
			if ((int)EditorSpawns.selectedVehicle < LevelVehicles.tables.Count)
			{
				LevelVehicles.tables[(int)EditorSpawns.selectedVehicle].color = color;
			}
		}

		private static void onTableIDFieldTyped(SleekUInt16Field field, ushort state)
		{
			if ((int)EditorSpawns.selectedVehicle < LevelVehicles.tables.Count)
			{
				LevelVehicles.tables[(int)EditorSpawns.selectedVehicle].tableID = state;
			}
		}

		private static void onClickedTierButton(SleekButton button)
		{
			if ((int)EditorSpawns.selectedVehicle < LevelVehicles.tables.Count)
			{
				if (EditorSpawnsVehiclesUI.selectedTier != (byte)((button.positionOffset_Y - 170) / 70))
				{
					EditorSpawnsVehiclesUI.selectedTier = (byte)((button.positionOffset_Y - 170) / 70);
				}
				else
				{
					EditorSpawnsVehiclesUI.selectedTier = byte.MaxValue;
				}
				EditorSpawnsVehiclesUI.updateSelection();
			}
		}

		private static void onClickVehicleButton(SleekButton button)
		{
			if ((int)EditorSpawns.selectedVehicle < LevelVehicles.tables.Count)
			{
				EditorSpawnsVehiclesUI.selectVehicle = (byte)((button.positionOffset_Y - 170 - EditorSpawnsVehiclesUI.tierButtons.Length * 70 - 80) / 40);
				EditorSpawnsVehiclesUI.updateSelection();
			}
		}

		private static void onDraggedChanceSlider(SleekSlider slider, float state)
		{
			if ((int)EditorSpawns.selectedVehicle < LevelVehicles.tables.Count)
			{
				int num = (slider.parent.positionOffset_Y - 170) / 70;
				LevelVehicles.tables[(int)EditorSpawns.selectedVehicle].updateChance(num, state);
				for (int i = 0; i < LevelVehicles.tables[(int)EditorSpawns.selectedVehicle].tiers.Count; i++)
				{
					VehicleTier vehicleTier = LevelVehicles.tables[(int)EditorSpawns.selectedVehicle].tiers[i];
					SleekSlider sleekSlider = (SleekSlider)EditorSpawnsVehiclesUI.tierButtons[i].children[0];
					if (i != num)
					{
						sleekSlider.state = vehicleTier.chance;
					}
					sleekSlider.updateLabel(Mathf.RoundToInt(vehicleTier.chance * 100f) + "%");
				}
			}
		}

		private static void onTypedNameField(SleekField field, string state)
		{
			if ((int)EditorSpawns.selectedVehicle < LevelVehicles.tables.Count)
			{
				EditorSpawnsVehiclesUI.selectedBox.text = state;
				LevelVehicles.tables[(int)EditorSpawns.selectedVehicle].name = state;
				EditorSpawnsVehiclesUI.tableButtons[(int)EditorSpawns.selectedVehicle].text = EditorSpawns.selectedVehicle + " " + state;
			}
		}

		private static void onClickedAddTableButton(SleekButton button)
		{
			if (EditorSpawnsVehiclesUI.tableNameField.text != string.Empty)
			{
				LevelVehicles.addTable(EditorSpawnsVehiclesUI.tableNameField.text);
				EditorSpawnsVehiclesUI.tableNameField.text = string.Empty;
				EditorSpawnsVehiclesUI.updateTables();
				EditorSpawnsVehiclesUI.tableScrollBox.state = new Vector2(0f, float.MaxValue);
			}
		}

		private static void onClickedRemoveTableButton(SleekButton button)
		{
			if ((int)EditorSpawns.selectedVehicle < LevelVehicles.tables.Count)
			{
				LevelVehicles.removeTable();
				EditorSpawnsVehiclesUI.updateTables();
				EditorSpawnsVehiclesUI.updateSelection();
				EditorSpawnsVehiclesUI.tableScrollBox.state = new Vector2(0f, float.MaxValue);
			}
		}

		private static void onTypedTierNameField(SleekField field, string state)
		{
			if ((int)EditorSpawns.selectedVehicle < LevelVehicles.tables.Count && (int)EditorSpawnsVehiclesUI.selectedTier < LevelVehicles.tables[(int)EditorSpawns.selectedVehicle].tiers.Count)
			{
				LevelVehicles.tables[(int)EditorSpawns.selectedVehicle].tiers[(int)EditorSpawnsVehiclesUI.selectedTier].name = state;
				EditorSpawnsVehiclesUI.tierButtons[(int)EditorSpawnsVehiclesUI.selectedTier].text = state;
			}
		}

		private static void onClickedAddTierButton(SleekButton button)
		{
			if ((int)EditorSpawns.selectedVehicle < LevelVehicles.tables.Count && EditorSpawnsVehiclesUI.tierNameField.text != string.Empty)
			{
				LevelVehicles.tables[(int)EditorSpawns.selectedVehicle].addTier(EditorSpawnsVehiclesUI.tierNameField.text);
				EditorSpawnsVehiclesUI.tierNameField.text = string.Empty;
				EditorSpawnsVehiclesUI.updateSelection();
			}
		}

		private static void onClickedRemoveTierButton(SleekButton button)
		{
			if ((int)EditorSpawns.selectedVehicle < LevelVehicles.tables.Count && (int)EditorSpawnsVehiclesUI.selectedTier < LevelVehicles.tables[(int)EditorSpawns.selectedVehicle].tiers.Count)
			{
				LevelVehicles.tables[(int)EditorSpawns.selectedVehicle].removeTier((int)EditorSpawnsVehiclesUI.selectedTier);
				EditorSpawnsVehiclesUI.updateSelection();
			}
		}

		private static void onClickedAddVehicleButton(SleekButton button)
		{
			if ((int)EditorSpawns.selectedVehicle < LevelVehicles.tables.Count && (int)EditorSpawnsVehiclesUI.selectedTier < LevelVehicles.tables[(int)EditorSpawns.selectedVehicle].tiers.Count)
			{
				VehicleAsset vehicleAsset = (VehicleAsset)Assets.find(EAssetType.VEHICLE, EditorSpawnsVehiclesUI.vehicleIDField.state);
				if (vehicleAsset != null)
				{
					LevelVehicles.tables[(int)EditorSpawns.selectedVehicle].addVehicle(EditorSpawnsVehiclesUI.selectedTier, EditorSpawnsVehiclesUI.vehicleIDField.state);
					EditorSpawnsVehiclesUI.updateSelection();
					EditorSpawnsVehiclesUI.spawnsScrollBox.state = new Vector2(0f, float.MaxValue);
				}
				EditorSpawnsVehiclesUI.vehicleIDField.state = 0;
			}
		}

		private static void onClickedRemoveVehicleButton(SleekButton button)
		{
			if ((int)EditorSpawns.selectedVehicle < LevelVehicles.tables.Count && (int)EditorSpawnsVehiclesUI.selectedTier < LevelVehicles.tables[(int)EditorSpawns.selectedVehicle].tiers.Count && (int)EditorSpawnsVehiclesUI.selectVehicle < LevelVehicles.tables[(int)EditorSpawns.selectedVehicle].tiers[(int)EditorSpawnsVehiclesUI.selectedTier].table.Count)
			{
				LevelVehicles.tables[(int)EditorSpawns.selectedVehicle].removeVehicle(EditorSpawnsVehiclesUI.selectedTier, EditorSpawnsVehiclesUI.selectVehicle);
				EditorSpawnsVehiclesUI.updateSelection();
				EditorSpawnsVehiclesUI.spawnsScrollBox.state = new Vector2(0f, float.MaxValue);
			}
		}

		private static void onDraggedRadiusSlider(SleekSlider slider, float state)
		{
			EditorSpawns.radius = (byte)((float)EditorSpawns.MIN_REMOVE_SIZE + state * (float)EditorSpawns.MAX_REMOVE_SIZE);
		}

		private static void onDraggedRotationSlider(SleekSlider slider, float state)
		{
			EditorSpawns.rotation = state * 360f;
		}

		private static void onClickedAddButton(SleekButton button)
		{
			EditorSpawns.spawnMode = ESpawnMode.ADD_VEHICLE;
		}

		private static void onClickedRemoveButton(SleekButton button)
		{
			EditorSpawns.spawnMode = ESpawnMode.REMOVE_VEHICLE;
		}

		private static Sleek container;

		public static bool active;

		private static SleekScrollBox tableScrollBox;

		private static SleekScrollBox spawnsScrollBox;

		private static SleekButton[] tableButtons;

		private static SleekButton[] tierButtons;

		private static SleekButton[] vehicleButtons;

		private static SleekColorPicker tableColorPicker;

		private static SleekUInt16Field tableIDField;

		private static SleekField tierNameField;

		private static SleekButtonIcon addTierButton;

		private static SleekButtonIcon removeTierButton;

		private static SleekUInt16Field vehicleIDField;

		private static SleekButtonIcon addVehicleButton;

		private static SleekButtonIcon removeVehicleButton;

		private static SleekBox selectedBox;

		private static SleekField tableNameField;

		private static SleekButtonIcon addTableButton;

		private static SleekButtonIcon removeTableButton;

		private static SleekSlider radiusSlider;

		private static SleekSlider rotationSlider;

		private static SleekButtonIcon addButton;

		private static SleekButtonIcon removeButton;

		private static byte selectedTier;

		private static byte selectVehicle;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache0;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache1;

		[CompilerGenerated]
		private static Dragged <>f__mg$cache2;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache3;

		[CompilerGenerated]
		private static Typed <>f__mg$cache4;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache5;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache6;

		[CompilerGenerated]
		private static ColorPicked <>f__mg$cache7;

		[CompilerGenerated]
		private static TypedUInt16 <>f__mg$cache8;

		[CompilerGenerated]
		private static Typed <>f__mg$cache9;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cacheA;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cacheB;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cacheC;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cacheD;

		[CompilerGenerated]
		private static Dragged <>f__mg$cacheE;

		[CompilerGenerated]
		private static Dragged <>f__mg$cacheF;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache10;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache11;
	}
}
