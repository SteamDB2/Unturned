using System;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace SDG.Unturned
{
	public class MenuWorkshopSpawnsUI
	{
		public MenuWorkshopSpawnsUI()
		{
			MenuWorkshopSpawnsUI.localization = Localization.read("/Menu/Workshop/MenuWorkshopSpawns.dat");
			MenuWorkshopSpawnsUI.container = new Sleek();
			MenuWorkshopSpawnsUI.container.positionOffset_X = 10;
			MenuWorkshopSpawnsUI.container.positionOffset_Y = 10;
			MenuWorkshopSpawnsUI.container.positionScale_Y = 1f;
			MenuWorkshopSpawnsUI.container.sizeOffset_X = -20;
			MenuWorkshopSpawnsUI.container.sizeOffset_Y = -20;
			MenuWorkshopSpawnsUI.container.sizeScale_X = 1f;
			MenuWorkshopSpawnsUI.container.sizeScale_Y = 1f;
			MenuUI.container.add(MenuWorkshopSpawnsUI.container);
			MenuWorkshopSpawnsUI.active = false;
			MenuWorkshopSpawnsUI.spawnsBox = new SleekScrollBox();
			MenuWorkshopSpawnsUI.spawnsBox.positionOffset_X = -315;
			MenuWorkshopSpawnsUI.spawnsBox.positionOffset_Y = 100;
			MenuWorkshopSpawnsUI.spawnsBox.positionScale_X = 0.5f;
			MenuWorkshopSpawnsUI.spawnsBox.sizeOffset_X = 630;
			MenuWorkshopSpawnsUI.spawnsBox.sizeOffset_Y = -200;
			MenuWorkshopSpawnsUI.spawnsBox.sizeScale_Y = 1f;
			MenuWorkshopSpawnsUI.container.add(MenuWorkshopSpawnsUI.spawnsBox);
			MenuWorkshopSpawnsUI.typeButton = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(MenuWorkshopSpawnsUI.localization.format("Type_Item")),
				new GUIContent(MenuWorkshopSpawnsUI.localization.format("Type_Vehicle")),
				new GUIContent(MenuWorkshopSpawnsUI.localization.format("Type_Animal"))
			});
			MenuWorkshopSpawnsUI.typeButton.sizeOffset_X = 600;
			MenuWorkshopSpawnsUI.typeButton.sizeOffset_Y = 30;
			MenuWorkshopSpawnsUI.typeButton.tooltip = MenuWorkshopSpawnsUI.localization.format("Type_Tooltip");
			MenuWorkshopSpawnsUI.spawnsBox.add(MenuWorkshopSpawnsUI.typeButton);
			MenuWorkshopSpawnsUI.viewIDField = new SleekUInt16Field();
			MenuWorkshopSpawnsUI.viewIDField.positionOffset_Y = 40;
			MenuWorkshopSpawnsUI.viewIDField.sizeOffset_X = 160;
			MenuWorkshopSpawnsUI.viewIDField.sizeOffset_Y = 30;
			MenuWorkshopSpawnsUI.spawnsBox.add(MenuWorkshopSpawnsUI.viewIDField);
			MenuWorkshopSpawnsUI.viewButton = new SleekButton();
			MenuWorkshopSpawnsUI.viewButton.positionOffset_X = 170;
			MenuWorkshopSpawnsUI.viewButton.positionOffset_Y = 40;
			MenuWorkshopSpawnsUI.viewButton.sizeOffset_X = 100;
			MenuWorkshopSpawnsUI.viewButton.sizeOffset_Y = 30;
			MenuWorkshopSpawnsUI.viewButton.text = MenuWorkshopSpawnsUI.localization.format("View_Button");
			MenuWorkshopSpawnsUI.viewButton.tooltip = MenuWorkshopSpawnsUI.localization.format("View_Button_Tooltip");
			SleekButton sleekButton = MenuWorkshopSpawnsUI.viewButton;
			if (MenuWorkshopSpawnsUI.<>f__mg$cache6 == null)
			{
				MenuWorkshopSpawnsUI.<>f__mg$cache6 = new ClickedButton(MenuWorkshopSpawnsUI.onClickedViewButton);
			}
			sleekButton.onClickedButton = MenuWorkshopSpawnsUI.<>f__mg$cache6;
			MenuWorkshopSpawnsUI.spawnsBox.add(MenuWorkshopSpawnsUI.viewButton);
			MenuWorkshopSpawnsUI.rawButton = new SleekButton();
			MenuWorkshopSpawnsUI.rawButton.positionOffset_X = 280;
			MenuWorkshopSpawnsUI.rawButton.positionOffset_Y = 40;
			MenuWorkshopSpawnsUI.rawButton.sizeOffset_X = 100;
			MenuWorkshopSpawnsUI.rawButton.sizeOffset_Y = 30;
			MenuWorkshopSpawnsUI.rawButton.text = MenuWorkshopSpawnsUI.localization.format("Raw_Button");
			MenuWorkshopSpawnsUI.rawButton.tooltip = MenuWorkshopSpawnsUI.localization.format("Raw_Button_Tooltip");
			SleekButton sleekButton2 = MenuWorkshopSpawnsUI.rawButton;
			if (MenuWorkshopSpawnsUI.<>f__mg$cache7 == null)
			{
				MenuWorkshopSpawnsUI.<>f__mg$cache7 = new ClickedButton(MenuWorkshopSpawnsUI.onClickedRawButton);
			}
			sleekButton2.onClickedButton = MenuWorkshopSpawnsUI.<>f__mg$cache7;
			MenuWorkshopSpawnsUI.spawnsBox.add(MenuWorkshopSpawnsUI.rawButton);
			MenuWorkshopSpawnsUI.newButton = new SleekButton();
			MenuWorkshopSpawnsUI.newButton.positionOffset_X = 390;
			MenuWorkshopSpawnsUI.newButton.positionOffset_Y = 40;
			MenuWorkshopSpawnsUI.newButton.sizeOffset_X = 100;
			MenuWorkshopSpawnsUI.newButton.sizeOffset_Y = 30;
			MenuWorkshopSpawnsUI.newButton.text = MenuWorkshopSpawnsUI.localization.format("New_Button");
			MenuWorkshopSpawnsUI.newButton.tooltip = MenuWorkshopSpawnsUI.localization.format("New_Button_Tooltip");
			SleekButton sleekButton3 = MenuWorkshopSpawnsUI.newButton;
			if (MenuWorkshopSpawnsUI.<>f__mg$cache8 == null)
			{
				MenuWorkshopSpawnsUI.<>f__mg$cache8 = new ClickedButton(MenuWorkshopSpawnsUI.onClickedNewButton);
			}
			sleekButton3.onClickedButton = MenuWorkshopSpawnsUI.<>f__mg$cache8;
			MenuWorkshopSpawnsUI.spawnsBox.add(MenuWorkshopSpawnsUI.newButton);
			MenuWorkshopSpawnsUI.writeButton = new SleekButton();
			MenuWorkshopSpawnsUI.writeButton.positionOffset_X = 500;
			MenuWorkshopSpawnsUI.writeButton.positionOffset_Y = 40;
			MenuWorkshopSpawnsUI.writeButton.sizeOffset_X = 100;
			MenuWorkshopSpawnsUI.writeButton.sizeOffset_Y = 30;
			MenuWorkshopSpawnsUI.writeButton.text = MenuWorkshopSpawnsUI.localization.format("Write_Button");
			MenuWorkshopSpawnsUI.writeButton.tooltip = MenuWorkshopSpawnsUI.localization.format("Write_Button_Tooltip");
			SleekButton sleekButton4 = MenuWorkshopSpawnsUI.writeButton;
			if (MenuWorkshopSpawnsUI.<>f__mg$cache9 == null)
			{
				MenuWorkshopSpawnsUI.<>f__mg$cache9 = new ClickedButton(MenuWorkshopSpawnsUI.onClickedWriteButton);
			}
			sleekButton4.onClickedButton = MenuWorkshopSpawnsUI.<>f__mg$cache9;
			MenuWorkshopSpawnsUI.spawnsBox.add(MenuWorkshopSpawnsUI.writeButton);
			MenuWorkshopSpawnsUI.addRootIDField = new SleekUInt16Field();
			MenuWorkshopSpawnsUI.addRootIDField.sizeOffset_X = 470;
			MenuWorkshopSpawnsUI.addRootIDField.sizeOffset_Y = 30;
			MenuWorkshopSpawnsUI.spawnsBox.add(MenuWorkshopSpawnsUI.addRootIDField);
			MenuWorkshopSpawnsUI.addRootSpawnButton = new SleekButtonIcon((Texture2D)MenuWorkshopEditorUI.icons.load("Add"));
			MenuWorkshopSpawnsUI.addRootSpawnButton.positionOffset_X = 480;
			MenuWorkshopSpawnsUI.addRootSpawnButton.sizeOffset_X = 120;
			MenuWorkshopSpawnsUI.addRootSpawnButton.sizeOffset_Y = 30;
			MenuWorkshopSpawnsUI.addRootSpawnButton.text = MenuWorkshopSpawnsUI.localization.format("Add_Root_Spawn_Button");
			MenuWorkshopSpawnsUI.addRootSpawnButton.tooltip = MenuWorkshopSpawnsUI.localization.format("Add_Root_Spawn_Button_Tooltip");
			SleekButton sleekButton5 = MenuWorkshopSpawnsUI.addRootSpawnButton;
			if (MenuWorkshopSpawnsUI.<>f__mg$cacheA == null)
			{
				MenuWorkshopSpawnsUI.<>f__mg$cacheA = new ClickedButton(MenuWorkshopSpawnsUI.onClickedAddRootSpawnButton);
			}
			sleekButton5.onClickedButton = MenuWorkshopSpawnsUI.<>f__mg$cacheA;
			MenuWorkshopSpawnsUI.spawnsBox.add(MenuWorkshopSpawnsUI.addRootSpawnButton);
			MenuWorkshopSpawnsUI.addTableIDField = new SleekUInt16Field();
			MenuWorkshopSpawnsUI.addTableIDField.sizeOffset_X = 340;
			MenuWorkshopSpawnsUI.addTableIDField.sizeOffset_Y = 30;
			MenuWorkshopSpawnsUI.spawnsBox.add(MenuWorkshopSpawnsUI.addTableIDField);
			MenuWorkshopSpawnsUI.addTableAssetButton = new SleekButtonIcon((Texture2D)MenuWorkshopEditorUI.icons.load("Add"));
			MenuWorkshopSpawnsUI.addTableAssetButton.positionOffset_X = 350;
			MenuWorkshopSpawnsUI.addTableAssetButton.sizeOffset_X = 120;
			MenuWorkshopSpawnsUI.addTableAssetButton.sizeOffset_Y = 30;
			MenuWorkshopSpawnsUI.addTableAssetButton.text = MenuWorkshopSpawnsUI.localization.format("Add_Table_Asset_Button");
			MenuWorkshopSpawnsUI.addTableAssetButton.tooltip = MenuWorkshopSpawnsUI.localization.format("Add_Table_Asset_Button_Tooltip");
			SleekButton sleekButton6 = MenuWorkshopSpawnsUI.addTableAssetButton;
			if (MenuWorkshopSpawnsUI.<>f__mg$cacheB == null)
			{
				MenuWorkshopSpawnsUI.<>f__mg$cacheB = new ClickedButton(MenuWorkshopSpawnsUI.onClickedAddTableAssetButton);
			}
			sleekButton6.onClickedButton = MenuWorkshopSpawnsUI.<>f__mg$cacheB;
			MenuWorkshopSpawnsUI.spawnsBox.add(MenuWorkshopSpawnsUI.addTableAssetButton);
			MenuWorkshopSpawnsUI.addTableSpawnButton = new SleekButtonIcon((Texture2D)MenuWorkshopEditorUI.icons.load("Add"));
			MenuWorkshopSpawnsUI.addTableSpawnButton.positionOffset_X = 480;
			MenuWorkshopSpawnsUI.addTableSpawnButton.sizeOffset_X = 120;
			MenuWorkshopSpawnsUI.addTableSpawnButton.sizeOffset_Y = 30;
			MenuWorkshopSpawnsUI.addTableSpawnButton.text = MenuWorkshopSpawnsUI.localization.format("Add_Table_Spawn_Button");
			MenuWorkshopSpawnsUI.addTableSpawnButton.tooltip = MenuWorkshopSpawnsUI.localization.format("Add_Table_Spawn_Button_Tooltip");
			SleekButton sleekButton7 = MenuWorkshopSpawnsUI.addTableSpawnButton;
			if (MenuWorkshopSpawnsUI.<>f__mg$cacheC == null)
			{
				MenuWorkshopSpawnsUI.<>f__mg$cacheC = new ClickedButton(MenuWorkshopSpawnsUI.onClickedAddTableSpawnButton);
			}
			sleekButton7.onClickedButton = MenuWorkshopSpawnsUI.<>f__mg$cacheC;
			MenuWorkshopSpawnsUI.spawnsBox.add(MenuWorkshopSpawnsUI.addTableSpawnButton);
			MenuWorkshopSpawnsUI.applyWeightsButton = new SleekButton();
			MenuWorkshopSpawnsUI.applyWeightsButton.sizeOffset_X = 600;
			MenuWorkshopSpawnsUI.applyWeightsButton.sizeOffset_Y = 30;
			MenuWorkshopSpawnsUI.applyWeightsButton.text = MenuWorkshopSpawnsUI.localization.format("Apply_Weights_Button");
			MenuWorkshopSpawnsUI.applyWeightsButton.tooltip = MenuWorkshopSpawnsUI.localization.format("Apply_Weights_Button_Tooltip");
			SleekButton sleekButton8 = MenuWorkshopSpawnsUI.applyWeightsButton;
			if (MenuWorkshopSpawnsUI.<>f__mg$cacheD == null)
			{
				MenuWorkshopSpawnsUI.<>f__mg$cacheD = new ClickedButton(MenuWorkshopSpawnsUI.onClickedApplyWeightsButton);
			}
			sleekButton8.onClickedButton = MenuWorkshopSpawnsUI.<>f__mg$cacheD;
			MenuWorkshopSpawnsUI.spawnsBox.add(MenuWorkshopSpawnsUI.applyWeightsButton);
			MenuWorkshopSpawnsUI.rootsBox = new SleekBox();
			MenuWorkshopSpawnsUI.rootsBox.positionOffset_Y = 40;
			MenuWorkshopSpawnsUI.rootsBox.sizeOffset_X = 600;
			MenuWorkshopSpawnsUI.rootsBox.sizeOffset_Y = 30;
			MenuWorkshopSpawnsUI.rootsBox.tooltip = MenuWorkshopSpawnsUI.localization.format("Roots_Box_Tooltip");
			MenuWorkshopSpawnsUI.spawnsBox.add(MenuWorkshopSpawnsUI.rootsBox);
			MenuWorkshopSpawnsUI.tablesBox = new SleekBox();
			MenuWorkshopSpawnsUI.tablesBox.positionOffset_Y = 80;
			MenuWorkshopSpawnsUI.tablesBox.sizeOffset_X = 600;
			MenuWorkshopSpawnsUI.tablesBox.sizeOffset_Y = 30;
			MenuWorkshopSpawnsUI.tablesBox.tooltip = MenuWorkshopSpawnsUI.localization.format("Tables_Box_Tooltip");
			MenuWorkshopSpawnsUI.spawnsBox.add(MenuWorkshopSpawnsUI.tablesBox);
			MenuWorkshopSpawnsUI.rawField = new SleekField();
			MenuWorkshopSpawnsUI.rawField.positionOffset_Y = 80;
			MenuWorkshopSpawnsUI.rawField.sizeOffset_X = 600;
			MenuWorkshopSpawnsUI.rawField.sizeOffset_Y = 1000;
			MenuWorkshopSpawnsUI.rawField.multiline = true;
			MenuWorkshopSpawnsUI.rawField.maxLength = 4096;
			MenuWorkshopSpawnsUI.rawField.fontAlignment = 0;
			MenuWorkshopSpawnsUI.spawnsBox.add(MenuWorkshopSpawnsUI.rawField);
			MenuWorkshopSpawnsUI.backButton = new SleekButtonIcon((Texture2D)MenuDashboardUI.icons.load("Exit"));
			MenuWorkshopSpawnsUI.backButton.positionOffset_Y = -50;
			MenuWorkshopSpawnsUI.backButton.positionScale_Y = 1f;
			MenuWorkshopSpawnsUI.backButton.sizeOffset_X = 200;
			MenuWorkshopSpawnsUI.backButton.sizeOffset_Y = 50;
			MenuWorkshopSpawnsUI.backButton.text = MenuDashboardUI.localization.format("BackButtonText");
			MenuWorkshopSpawnsUI.backButton.tooltip = MenuDashboardUI.localization.format("BackButtonTooltip");
			SleekButton sleekButton9 = MenuWorkshopSpawnsUI.backButton;
			if (MenuWorkshopSpawnsUI.<>f__mg$cacheE == null)
			{
				MenuWorkshopSpawnsUI.<>f__mg$cacheE = new ClickedButton(MenuWorkshopSpawnsUI.onClickedBackButton);
			}
			sleekButton9.onClickedButton = MenuWorkshopSpawnsUI.<>f__mg$cacheE;
			MenuWorkshopSpawnsUI.backButton.fontSize = 14;
			MenuWorkshopSpawnsUI.backButton.iconImage.backgroundTint = ESleekTint.FOREGROUND;
			MenuWorkshopSpawnsUI.container.add(MenuWorkshopSpawnsUI.backButton);
		}

		public static void open()
		{
			if (MenuWorkshopSpawnsUI.active)
			{
				return;
			}
			MenuWorkshopSpawnsUI.active = true;
			Localization.refresh();
			MenuWorkshopSpawnsUI.refresh();
			MenuWorkshopSpawnsUI.container.lerpPositionScale(0f, 0f, ESleekLerp.EXPONENTIAL, 20f);
		}

		public static void close()
		{
			if (!MenuWorkshopSpawnsUI.active)
			{
				return;
			}
			MenuWorkshopSpawnsUI.active = false;
			MenuWorkshopSpawnsUI.container.lerpPositionScale(0f, 1f, ESleekLerp.EXPONENTIAL, 20f);
		}

		private static void refresh()
		{
			MenuWorkshopSpawnsUI.rawField.isVisible = false;
			MenuWorkshopSpawnsUI.rootsBox.isVisible = true;
			MenuWorkshopSpawnsUI.tablesBox.isVisible = true;
			MenuWorkshopSpawnsUI.rootsBox.remove();
			MenuWorkshopSpawnsUI.tablesBox.remove();
			MenuWorkshopSpawnsUI.asset = (SpawnAsset)Assets.find(EAssetType.SPAWN, MenuWorkshopSpawnsUI.viewIDField.state);
			switch (MenuWorkshopSpawnsUI.typeButton.state)
			{
			case 0:
				MenuWorkshopSpawnsUI.type = EAssetType.ITEM;
				break;
			case 1:
				MenuWorkshopSpawnsUI.type = EAssetType.VEHICLE;
				break;
			case 2:
				MenuWorkshopSpawnsUI.type = EAssetType.ANIMAL;
				break;
			default:
				MenuWorkshopSpawnsUI.type = EAssetType.NONE;
				return;
			}
			int num = 120;
			MenuWorkshopSpawnsUI.rootsBox.positionOffset_Y = num;
			num += 40;
			if (MenuWorkshopSpawnsUI.asset != null)
			{
				MenuWorkshopSpawnsUI.rootsBox.text = MenuWorkshopSpawnsUI.localization.format("Roots_Box", new object[]
				{
					MenuWorkshopSpawnsUI.viewIDField.state + " " + MenuWorkshopSpawnsUI.asset.name
				});
				for (int i = 0; i < MenuWorkshopSpawnsUI.asset.roots.Count; i++)
				{
					SpawnTable spawnTable = MenuWorkshopSpawnsUI.asset.roots[i];
					if (spawnTable.spawnID != 0)
					{
						SleekButton sleekButton = new SleekButton();
						sleekButton.positionOffset_Y = 40 + i * 40;
						sleekButton.sizeOffset_X = -260;
						sleekButton.sizeScale_X = 1f;
						sleekButton.sizeOffset_Y = 30;
						SleekButton sleekButton2 = sleekButton;
						if (MenuWorkshopSpawnsUI.<>f__mg$cache0 == null)
						{
							MenuWorkshopSpawnsUI.<>f__mg$cache0 = new ClickedButton(MenuWorkshopSpawnsUI.onClickedRootButton);
						}
						sleekButton2.onClickedButton = MenuWorkshopSpawnsUI.<>f__mg$cache0;
						MenuWorkshopSpawnsUI.rootsBox.add(sleekButton);
						num += 40;
						SpawnAsset spawnAsset = (SpawnAsset)Assets.find(EAssetType.SPAWN, spawnTable.spawnID);
						if (spawnAsset != null)
						{
							sleekButton.text = spawnTable.spawnID + " " + spawnAsset.name;
						}
						else
						{
							sleekButton.text = spawnTable.spawnID + " ?";
						}
						SleekInt32Field sleekInt32Field = new SleekInt32Field();
						sleekInt32Field.positionOffset_X = 10;
						sleekInt32Field.positionScale_X = 1f;
						sleekInt32Field.sizeOffset_X = 55;
						sleekInt32Field.sizeOffset_Y = 30;
						sleekInt32Field.state = spawnTable.weight;
						sleekInt32Field.tooltip = MenuWorkshopSpawnsUI.localization.format("Weight_Tooltip");
						SleekInt32Field sleekInt32Field2 = sleekInt32Field;
						Delegate onTypedInt = sleekInt32Field2.onTypedInt;
						if (MenuWorkshopSpawnsUI.<>f__mg$cache1 == null)
						{
							MenuWorkshopSpawnsUI.<>f__mg$cache1 = new TypedInt32(MenuWorkshopSpawnsUI.onTypedRootWeightField);
						}
						sleekInt32Field2.onTypedInt = (TypedInt32)Delegate.Combine(onTypedInt, MenuWorkshopSpawnsUI.<>f__mg$cache1);
						sleekButton.add(sleekInt32Field);
						sleekButton.add(new SleekBox
						{
							positionOffset_X = 75,
							positionScale_X = 1f,
							sizeOffset_X = 55,
							sizeOffset_Y = 30,
							text = (Math.Round((double)(spawnTable.chance * 1000f)) / 10.0).ToString() + "%",
							tooltip = MenuWorkshopSpawnsUI.localization.format("Chance_Tooltip")
						});
						SleekButtonIcon sleekButtonIcon = new SleekButtonIcon((Texture2D)MenuWorkshopEditorUI.icons.load("Remove"));
						sleekButtonIcon.positionOffset_X = 140;
						sleekButtonIcon.positionScale_X = 1f;
						sleekButtonIcon.sizeOffset_X = 120;
						sleekButtonIcon.sizeOffset_Y = 30;
						sleekButtonIcon.text = MenuWorkshopSpawnsUI.localization.format("Remove_Root_Button");
						sleekButtonIcon.tooltip = MenuWorkshopSpawnsUI.localization.format("Remove_Root_Button_Tooltip");
						SleekButton sleekButton3 = sleekButtonIcon;
						if (MenuWorkshopSpawnsUI.<>f__mg$cache2 == null)
						{
							MenuWorkshopSpawnsUI.<>f__mg$cache2 = new ClickedButton(MenuWorkshopSpawnsUI.onClickedRemoveRootButton);
						}
						sleekButton3.onClickedButton = MenuWorkshopSpawnsUI.<>f__mg$cache2;
						sleekButton.add(sleekButtonIcon);
					}
				}
				MenuWorkshopSpawnsUI.addRootIDField.positionOffset_Y = num;
				MenuWorkshopSpawnsUI.addRootSpawnButton.positionOffset_Y = num;
				num += 40;
				MenuWorkshopSpawnsUI.addRootIDField.isVisible = true;
				MenuWorkshopSpawnsUI.addRootSpawnButton.isVisible = true;
			}
			else
			{
				MenuWorkshopSpawnsUI.rootsBox.text = MenuWorkshopSpawnsUI.localization.format("Roots_Box", new object[]
				{
					MenuWorkshopSpawnsUI.viewIDField.state + " ?"
				});
				MenuWorkshopSpawnsUI.addRootIDField.isVisible = false;
				MenuWorkshopSpawnsUI.addRootSpawnButton.isVisible = false;
			}
			num += 40;
			MenuWorkshopSpawnsUI.tablesBox.positionOffset_Y = num;
			num += 40;
			if (MenuWorkshopSpawnsUI.asset != null)
			{
				MenuWorkshopSpawnsUI.tablesBox.text = MenuWorkshopSpawnsUI.localization.format("Tables_Box", new object[]
				{
					MenuWorkshopSpawnsUI.viewIDField.state + " " + MenuWorkshopSpawnsUI.asset.name
				});
				for (int j = 0; j < MenuWorkshopSpawnsUI.asset.tables.Count; j++)
				{
					SpawnTable spawnTable2 = MenuWorkshopSpawnsUI.asset.tables[j];
					Sleek sleek = null;
					if (spawnTable2.spawnID != 0)
					{
						SleekButton sleekButton4 = new SleekButton();
						sleekButton4.positionOffset_Y = 40 + j * 40;
						sleekButton4.sizeOffset_X = -260;
						sleekButton4.sizeScale_X = 1f;
						sleekButton4.sizeOffset_Y = 30;
						SleekButton sleekButton5 = sleekButton4;
						if (MenuWorkshopSpawnsUI.<>f__mg$cache3 == null)
						{
							MenuWorkshopSpawnsUI.<>f__mg$cache3 = new ClickedButton(MenuWorkshopSpawnsUI.onClickedTableButton);
						}
						sleekButton5.onClickedButton = MenuWorkshopSpawnsUI.<>f__mg$cache3;
						MenuWorkshopSpawnsUI.tablesBox.add(sleekButton4);
						sleek = sleekButton4;
						num += 40;
						SpawnAsset spawnAsset2 = (SpawnAsset)Assets.find(EAssetType.SPAWN, spawnTable2.spawnID);
						if (spawnAsset2 != null)
						{
							sleekButton4.text = spawnTable2.spawnID + " " + spawnAsset2.name;
						}
						else
						{
							sleekButton4.text = spawnTable2.spawnID + " ?";
						}
					}
					else if (spawnTable2.assetID != 0)
					{
						SleekBox sleekBox = new SleekBox();
						sleekBox.positionOffset_Y = 40 + j * 40;
						sleekBox.sizeOffset_X = -260;
						sleekBox.sizeScale_X = 1f;
						sleekBox.sizeOffset_Y = 30;
						MenuWorkshopSpawnsUI.tablesBox.add(sleekBox);
						sleek = sleekBox;
						num += 40;
						Asset asset = Assets.find(MenuWorkshopSpawnsUI.type, spawnTable2.assetID);
						if (asset != null)
						{
							sleekBox.text = spawnTable2.assetID + " " + asset.name;
							if (MenuWorkshopSpawnsUI.type == EAssetType.ITEM)
							{
								ItemAsset itemAsset = asset as ItemAsset;
								if (MenuWorkshopSpawnsUI.asset != null)
								{
									sleekBox.foregroundTint = ESleekTint.NONE;
									sleekBox.foregroundColor = ItemTool.getRarityColorUI(itemAsset.rarity);
								}
							}
							else if (MenuWorkshopSpawnsUI.type == EAssetType.VEHICLE)
							{
								VehicleAsset vehicleAsset = asset as VehicleAsset;
								if (MenuWorkshopSpawnsUI.asset != null)
								{
									sleekBox.foregroundTint = ESleekTint.NONE;
									sleekBox.foregroundColor = ItemTool.getRarityColorUI(vehicleAsset.rarity);
								}
							}
						}
						else
						{
							sleekBox.text = spawnTable2.assetID + " ?";
						}
					}
					if (sleek != null)
					{
						SleekInt32Field sleekInt32Field3 = new SleekInt32Field();
						sleekInt32Field3.positionOffset_X = 10;
						sleekInt32Field3.positionScale_X = 1f;
						sleekInt32Field3.sizeOffset_X = 55;
						sleekInt32Field3.sizeOffset_Y = 30;
						sleekInt32Field3.state = spawnTable2.weight;
						sleekInt32Field3.tooltip = MenuWorkshopSpawnsUI.localization.format("Weight_Tooltip");
						SleekInt32Field sleekInt32Field4 = sleekInt32Field3;
						Delegate onTypedInt2 = sleekInt32Field4.onTypedInt;
						if (MenuWorkshopSpawnsUI.<>f__mg$cache4 == null)
						{
							MenuWorkshopSpawnsUI.<>f__mg$cache4 = new TypedInt32(MenuWorkshopSpawnsUI.onTypedTableWeightField);
						}
						sleekInt32Field4.onTypedInt = (TypedInt32)Delegate.Combine(onTypedInt2, MenuWorkshopSpawnsUI.<>f__mg$cache4);
						sleek.add(sleekInt32Field3);
						float num2 = spawnTable2.chance;
						if (j > 0)
						{
							num2 -= MenuWorkshopSpawnsUI.asset.tables[j - 1].chance;
						}
						sleek.add(new SleekBox
						{
							positionOffset_X = 75,
							positionScale_X = 1f,
							sizeOffset_X = 55,
							sizeOffset_Y = 30,
							text = (Math.Round((double)(num2 * 1000f)) / 10.0).ToString() + "%",
							tooltip = MenuWorkshopSpawnsUI.localization.format("Chance_Tooltip")
						});
						SleekButtonIcon sleekButtonIcon2 = new SleekButtonIcon((Texture2D)MenuWorkshopEditorUI.icons.load("Remove"));
						sleekButtonIcon2.positionOffset_X = 140;
						sleekButtonIcon2.positionScale_X = 1f;
						sleekButtonIcon2.sizeOffset_X = 120;
						sleekButtonIcon2.sizeOffset_Y = 30;
						sleekButtonIcon2.text = MenuWorkshopSpawnsUI.localization.format("Remove_Table_Button");
						sleekButtonIcon2.tooltip = MenuWorkshopSpawnsUI.localization.format("Remove_Table_Button_Tooltip");
						SleekButton sleekButton6 = sleekButtonIcon2;
						if (MenuWorkshopSpawnsUI.<>f__mg$cache5 == null)
						{
							MenuWorkshopSpawnsUI.<>f__mg$cache5 = new ClickedButton(MenuWorkshopSpawnsUI.onClickedRemoveTableButton);
						}
						sleekButton6.onClickedButton = MenuWorkshopSpawnsUI.<>f__mg$cache5;
						sleek.add(sleekButtonIcon2);
					}
				}
				MenuWorkshopSpawnsUI.addTableIDField.positionOffset_Y = num;
				MenuWorkshopSpawnsUI.addTableAssetButton.positionOffset_Y = num;
				MenuWorkshopSpawnsUI.addTableSpawnButton.positionOffset_Y = num;
				num += 40;
				MenuWorkshopSpawnsUI.addTableIDField.isVisible = true;
				MenuWorkshopSpawnsUI.addTableAssetButton.isVisible = true;
				MenuWorkshopSpawnsUI.addTableSpawnButton.isVisible = true;
			}
			else
			{
				MenuWorkshopSpawnsUI.tablesBox.text = MenuWorkshopSpawnsUI.localization.format("Tables_Box", new object[]
				{
					MenuWorkshopSpawnsUI.viewIDField.state + " ?"
				});
				MenuWorkshopSpawnsUI.addTableIDField.isVisible = false;
				MenuWorkshopSpawnsUI.addTableAssetButton.isVisible = false;
				MenuWorkshopSpawnsUI.addTableSpawnButton.isVisible = false;
			}
			if (MenuWorkshopSpawnsUI.asset != null)
			{
				MenuWorkshopSpawnsUI.applyWeightsButton.positionOffset_Y = num;
				num += 40;
				MenuWorkshopSpawnsUI.applyWeightsButton.isVisible = true;
			}
			else
			{
				MenuWorkshopSpawnsUI.applyWeightsButton.isVisible = false;
			}
			MenuWorkshopSpawnsUI.spawnsBox.area = new Rect(0f, 0f, 5f, (float)(num - 10));
		}

		private static string getRaw(SpawnAsset asset)
		{
			string text = "Type Spawn";
			text = text + "\nID " + asset.id;
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < asset.roots.Count; i++)
			{
				SpawnTable spawnTable = asset.roots[i];
				if (spawnTable.isLink && spawnTable.weight > 0)
				{
					num++;
				}
			}
			if (num > 0)
			{
				text += "\n";
				text = text + "\nRoots " + num;
				for (int j = 0; j < asset.roots.Count; j++)
				{
					SpawnTable spawnTable2 = asset.roots[j];
					if (spawnTable2.isLink && spawnTable2.weight > 0)
					{
						string text2 = text;
						text = string.Concat(new object[]
						{
							text2,
							"\nRoot_",
							num2,
							"_Spawn_ID ",
							spawnTable2.spawnID
						});
						text2 = text;
						text = string.Concat(new object[]
						{
							text2,
							"\nRoot_",
							num2,
							"_Weight ",
							spawnTable2.weight
						});
						num2++;
					}
				}
			}
			int num3 = 0;
			int num4 = 0;
			for (int k = 0; k < asset.tables.Count; k++)
			{
				SpawnTable spawnTable3 = asset.tables[k];
				if (!spawnTable3.isLink && spawnTable3.weight > 0)
				{
					num3++;
				}
			}
			if (num3 > 0)
			{
				text += "\n";
				text = text + "\nTables " + asset.tables.Count;
				for (int l = 0; l < asset.tables.Count; l++)
				{
					SpawnTable spawnTable4 = asset.tables[l];
					if (!spawnTable4.isLink && spawnTable4.weight > 0)
					{
						string text2;
						if (spawnTable4.assetID != 0)
						{
							text2 = text;
							text = string.Concat(new object[]
							{
								text2,
								"\nTable_",
								num4,
								"_Asset_ID ",
								spawnTable4.assetID
							});
						}
						else
						{
							text2 = text;
							text = string.Concat(new object[]
							{
								text2,
								"\nTable_",
								num4,
								"_Spawn_ID ",
								spawnTable4.spawnID
							});
						}
						text2 = text;
						text = string.Concat(new object[]
						{
							text2,
							"\nTable_",
							num4,
							"_Weight ",
							spawnTable4.weight
						});
						num4++;
					}
				}
			}
			return text;
		}

		private static void raw()
		{
			MenuWorkshopSpawnsUI.rawField.isVisible = true;
			MenuWorkshopSpawnsUI.rootsBox.isVisible = false;
			MenuWorkshopSpawnsUI.tablesBox.isVisible = false;
			MenuWorkshopSpawnsUI.addRootIDField.isVisible = false;
			MenuWorkshopSpawnsUI.addRootSpawnButton.isVisible = false;
			MenuWorkshopSpawnsUI.addTableIDField.isVisible = false;
			MenuWorkshopSpawnsUI.addTableAssetButton.isVisible = false;
			MenuWorkshopSpawnsUI.addTableSpawnButton.isVisible = false;
			MenuWorkshopSpawnsUI.applyWeightsButton.isVisible = false;
			MenuWorkshopSpawnsUI.asset = (SpawnAsset)Assets.find(EAssetType.SPAWN, MenuWorkshopSpawnsUI.viewIDField.state);
			string text;
			if (MenuWorkshopSpawnsUI.asset != null)
			{
				text = MenuWorkshopSpawnsUI.getRaw(MenuWorkshopSpawnsUI.asset);
			}
			else
			{
				text = "?";
			}
			MenuWorkshopSpawnsUI.rawField.text = text;
			GUIUtility.systemCopyBuffer = text;
			MenuWorkshopSpawnsUI.spawnsBox.area = new Rect(0f, 0f, 5f, 1080f);
		}

		private static void write()
		{
			MenuWorkshopSpawnsUI.asset = (SpawnAsset)Assets.find(EAssetType.SPAWN, MenuWorkshopSpawnsUI.viewIDField.state);
			if (MenuWorkshopSpawnsUI.asset == null)
			{
				return;
			}
			if (string.IsNullOrEmpty(MenuWorkshopSpawnsUI.asset.absoluteOriginFilePath) || !File.Exists(MenuWorkshopSpawnsUI.asset.absoluteOriginFilePath))
			{
				return;
			}
			string raw = MenuWorkshopSpawnsUI.getRaw(MenuWorkshopSpawnsUI.asset);
			File.WriteAllText(MenuWorkshopSpawnsUI.asset.absoluteOriginFilePath, raw);
		}

		private static void onClickedBackButton(SleekButton button)
		{
			MenuWorkshopUI.open();
			MenuWorkshopSpawnsUI.close();
		}

		private static void onClickedViewButton(SleekButton button)
		{
			MenuWorkshopSpawnsUI.refresh();
		}

		private static void onClickedRawButton(SleekButton button)
		{
			MenuWorkshopSpawnsUI.raw();
		}

		private static void onClickedNewButton(SleekButton button)
		{
			SpawnAsset spawnAsset = new SpawnAsset(MenuWorkshopSpawnsUI.viewIDField.state);
			Assets.add(spawnAsset);
			MenuWorkshopSpawnsUI.refresh();
		}

		private static void onClickedWriteButton(SleekButton button)
		{
			MenuWorkshopSpawnsUI.write();
		}

		private static void onClickedRootButton(SleekButton button)
		{
			int index = MenuWorkshopSpawnsUI.rootsBox.search(button);
			ushort spawnID = MenuWorkshopSpawnsUI.asset.roots[index].spawnID;
			MenuWorkshopSpawnsUI.viewIDField.state = spawnID;
			MenuWorkshopSpawnsUI.refresh();
		}

		private static void onClickedTableButton(SleekButton button)
		{
			int index = MenuWorkshopSpawnsUI.tablesBox.search(button);
			ushort spawnID = MenuWorkshopSpawnsUI.asset.tables[index].spawnID;
			MenuWorkshopSpawnsUI.viewIDField.state = spawnID;
			MenuWorkshopSpawnsUI.refresh();
		}

		private static void onTypedRootWeightField(SleekInt32Field field, int state)
		{
			int index = MenuWorkshopSpawnsUI.rootsBox.search(field.parent);
			MenuWorkshopSpawnsUI.asset.roots[index].weight = state;
		}

		private static void onClickedAddRootSpawnButton(SleekButton button)
		{
			if (MenuWorkshopSpawnsUI.addRootIDField.state == 0)
			{
				return;
			}
			for (int i = 0; i < MenuWorkshopSpawnsUI.asset.roots.Count; i++)
			{
				if (MenuWorkshopSpawnsUI.asset.roots[i].spawnID == MenuWorkshopSpawnsUI.addRootIDField.state)
				{
					return;
				}
			}
			SpawnAsset spawnAsset = (SpawnAsset)Assets.find(EAssetType.SPAWN, MenuWorkshopSpawnsUI.addRootIDField.state);
			if (spawnAsset == null)
			{
				return;
			}
			SpawnTable spawnTable = new SpawnTable();
			spawnTable.spawnID = MenuWorkshopSpawnsUI.addRootIDField.state;
			spawnTable.isLink = true;
			MenuWorkshopSpawnsUI.asset.roots.Add(spawnTable);
			SpawnTable spawnTable2 = new SpawnTable();
			spawnTable2.spawnID = MenuWorkshopSpawnsUI.asset.id;
			spawnTable2.isLink = true;
			spawnAsset.tables.Add(spawnTable2);
			MenuWorkshopSpawnsUI.addRootIDField.state = 0;
			MenuWorkshopSpawnsUI.refresh();
		}

		private static void onClickedRemoveRootButton(SleekButton button)
		{
			int index = MenuWorkshopSpawnsUI.rootsBox.search(button.parent);
			SpawnTable spawnTable = MenuWorkshopSpawnsUI.asset.roots[index];
			if (spawnTable.spawnID != 0)
			{
				SpawnAsset spawnAsset = (SpawnAsset)Assets.find(EAssetType.SPAWN, spawnTable.spawnID);
				if (spawnAsset != null)
				{
					for (int i = 0; i < spawnAsset.tables.Count; i++)
					{
						if (spawnAsset.tables[i].spawnID == MenuWorkshopSpawnsUI.asset.id)
						{
							spawnAsset.tables.RemoveAt(i);
							break;
						}
					}
				}
			}
			MenuWorkshopSpawnsUI.asset.roots.RemoveAt(index);
			MenuWorkshopSpawnsUI.refresh();
		}

		private static void onTypedTableWeightField(SleekInt32Field field, int state)
		{
			int index = MenuWorkshopSpawnsUI.tablesBox.search(field.parent);
			MenuWorkshopSpawnsUI.asset.tables[index].weight = state;
		}

		private static void onClickedAddTableAssetButton(SleekButton button)
		{
			if (MenuWorkshopSpawnsUI.addTableIDField.state == 0)
			{
				return;
			}
			for (int i = 0; i < MenuWorkshopSpawnsUI.asset.tables.Count; i++)
			{
				if (MenuWorkshopSpawnsUI.asset.tables[i].assetID == MenuWorkshopSpawnsUI.addTableIDField.state)
				{
					return;
				}
			}
			SpawnTable spawnTable = new SpawnTable();
			spawnTable.assetID = MenuWorkshopSpawnsUI.addTableIDField.state;
			MenuWorkshopSpawnsUI.asset.tables.Add(spawnTable);
			MenuWorkshopSpawnsUI.addTableIDField.state = 0;
			MenuWorkshopSpawnsUI.refresh();
		}

		private static void onClickedAddTableSpawnButton(SleekButton button)
		{
			if (MenuWorkshopSpawnsUI.addTableIDField.state == 0)
			{
				return;
			}
			for (int i = 0; i < MenuWorkshopSpawnsUI.asset.tables.Count; i++)
			{
				if (MenuWorkshopSpawnsUI.asset.tables[i].spawnID == MenuWorkshopSpawnsUI.addTableIDField.state)
				{
					return;
				}
			}
			SpawnAsset spawnAsset = (SpawnAsset)Assets.find(EAssetType.SPAWN, MenuWorkshopSpawnsUI.addTableIDField.state);
			if (spawnAsset == null)
			{
				return;
			}
			SpawnTable spawnTable = new SpawnTable();
			spawnTable.spawnID = MenuWorkshopSpawnsUI.asset.id;
			spawnAsset.roots.Add(spawnTable);
			SpawnTable spawnTable2 = new SpawnTable();
			spawnTable2.spawnID = MenuWorkshopSpawnsUI.addTableIDField.state;
			MenuWorkshopSpawnsUI.asset.tables.Add(spawnTable2);
			MenuWorkshopSpawnsUI.addTableIDField.state = 0;
			MenuWorkshopSpawnsUI.refresh();
		}

		private static void onClickedRemoveTableButton(SleekButton button)
		{
			int index = MenuWorkshopSpawnsUI.tablesBox.search(button.parent);
			SpawnTable spawnTable = MenuWorkshopSpawnsUI.asset.tables[index];
			if (spawnTable.spawnID != 0)
			{
				SpawnAsset spawnAsset = (SpawnAsset)Assets.find(EAssetType.SPAWN, spawnTable.spawnID);
				if (spawnAsset != null)
				{
					for (int i = 0; i < spawnAsset.roots.Count; i++)
					{
						if (spawnAsset.roots[i].spawnID == MenuWorkshopSpawnsUI.asset.id)
						{
							spawnAsset.roots.RemoveAt(i);
							break;
						}
					}
				}
			}
			MenuWorkshopSpawnsUI.asset.tables.RemoveAt(index);
			MenuWorkshopSpawnsUI.refresh();
		}

		private static void onClickedApplyWeightsButton(SleekButton button)
		{
			MenuWorkshopSpawnsUI.asset.prepare();
			MenuWorkshopSpawnsUI.refresh();
		}

		private static Local localization;

		private static Sleek container;

		public static bool active;

		private static SleekButtonIcon backButton;

		private static SleekScrollBox spawnsBox;

		private static SleekButtonState typeButton;

		private static SleekUInt16Field viewIDField;

		private static SleekButton viewButton;

		private static SleekButton rawButton;

		private static SleekButton newButton;

		private static SleekButton writeButton;

		private static SleekBox rootsBox;

		private static SleekBox tablesBox;

		private static SleekField rawField;

		private static SleekUInt16Field addRootIDField;

		private static SleekButtonIcon addRootSpawnButton;

		private static SleekUInt16Field addTableIDField;

		private static SleekButtonIcon addTableAssetButton;

		private static SleekButtonIcon addTableSpawnButton;

		private static SleekButton applyWeightsButton;

		private static SpawnAsset asset;

		private static EAssetType type;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache0;

		[CompilerGenerated]
		private static TypedInt32 <>f__mg$cache1;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache2;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache3;

		[CompilerGenerated]
		private static TypedInt32 <>f__mg$cache4;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache5;

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
		private static ClickedButton <>f__mg$cacheB;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cacheC;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cacheD;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cacheE;
	}
}
