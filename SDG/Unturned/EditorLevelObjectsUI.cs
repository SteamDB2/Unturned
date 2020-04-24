using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace SDG.Unturned
{
	public class EditorLevelObjectsUI
	{
		public EditorLevelObjectsUI()
		{
			Local local = Localization.read("/Editor/EditorLevelObjects.dat");
			Bundle bundle = Bundles.getBundle("/Bundles/Textures/Edit/Icons/EditorLevelObjects/EditorLevelObjects.unity3d");
			EditorLevelObjectsUI.container = new Sleek();
			EditorLevelObjectsUI.container.positionOffset_X = 10;
			EditorLevelObjectsUI.container.positionOffset_Y = 10;
			EditorLevelObjectsUI.container.positionScale_X = 1f;
			EditorLevelObjectsUI.container.sizeOffset_X = -20;
			EditorLevelObjectsUI.container.sizeOffset_Y = -20;
			EditorLevelObjectsUI.container.sizeScale_X = 1f;
			EditorLevelObjectsUI.container.sizeScale_Y = 1f;
			EditorUI.window.add(EditorLevelObjectsUI.container);
			EditorLevelObjectsUI.active = false;
			EditorLevelObjectsUI.assets = new List<Asset>();
			EditorLevelObjectsUI.selectedBox = new SleekBox();
			EditorLevelObjectsUI.selectedBox.positionOffset_X = -230;
			EditorLevelObjectsUI.selectedBox.positionOffset_Y = 80;
			EditorLevelObjectsUI.selectedBox.positionScale_X = 1f;
			EditorLevelObjectsUI.selectedBox.sizeOffset_X = 230;
			EditorLevelObjectsUI.selectedBox.sizeOffset_Y = 30;
			EditorLevelObjectsUI.selectedBox.addLabel(local.format("SelectionBoxLabelText"), ESleekSide.LEFT);
			EditorLevelObjectsUI.container.add(EditorLevelObjectsUI.selectedBox);
			EditorLevelObjectsUI.searchField = new SleekField();
			EditorLevelObjectsUI.searchField.positionOffset_X = -230;
			EditorLevelObjectsUI.searchField.positionOffset_Y = 120;
			EditorLevelObjectsUI.searchField.positionScale_X = 1f;
			EditorLevelObjectsUI.searchField.sizeOffset_X = 160;
			EditorLevelObjectsUI.searchField.sizeOffset_Y = 30;
			EditorLevelObjectsUI.searchField.hint = local.format("Search_Field_Hint");
			EditorLevelObjectsUI.searchField.control = "Search";
			SleekField sleekField = EditorLevelObjectsUI.searchField;
			if (EditorLevelObjectsUI.<>f__mg$cache1 == null)
			{
				EditorLevelObjectsUI.<>f__mg$cache1 = new Entered(EditorLevelObjectsUI.onEnteredSearchField);
			}
			sleekField.onEntered = EditorLevelObjectsUI.<>f__mg$cache1;
			EditorLevelObjectsUI.container.add(EditorLevelObjectsUI.searchField);
			EditorLevelObjectsUI.searchButton = new SleekButton();
			EditorLevelObjectsUI.searchButton.positionOffset_X = -60;
			EditorLevelObjectsUI.searchButton.positionOffset_Y = 120;
			EditorLevelObjectsUI.searchButton.positionScale_X = 1f;
			EditorLevelObjectsUI.searchButton.sizeOffset_X = 60;
			EditorLevelObjectsUI.searchButton.sizeOffset_Y = 30;
			EditorLevelObjectsUI.searchButton.text = local.format("Search");
			EditorLevelObjectsUI.searchButton.tooltip = local.format("Search_Tooltip");
			SleekButton sleekButton = EditorLevelObjectsUI.searchButton;
			if (EditorLevelObjectsUI.<>f__mg$cache2 == null)
			{
				EditorLevelObjectsUI.<>f__mg$cache2 = new ClickedButton(EditorLevelObjectsUI.onClickedSearchButton);
			}
			sleekButton.onClickedButton = EditorLevelObjectsUI.<>f__mg$cache2;
			EditorLevelObjectsUI.container.add(EditorLevelObjectsUI.searchButton);
			EditorLevelObjectsUI.largeToggle = new SleekToggle();
			EditorLevelObjectsUI.largeToggle.positionOffset_X = -230;
			EditorLevelObjectsUI.largeToggle.positionOffset_Y = 160;
			EditorLevelObjectsUI.largeToggle.positionScale_X = 1f;
			EditorLevelObjectsUI.largeToggle.sizeOffset_X = 40;
			EditorLevelObjectsUI.largeToggle.sizeOffset_Y = 40;
			EditorLevelObjectsUI.largeToggle.addLabel(local.format("LargeLabel"), ESleekSide.RIGHT);
			EditorLevelObjectsUI.largeToggle.state = true;
			SleekToggle sleekToggle = EditorLevelObjectsUI.largeToggle;
			if (EditorLevelObjectsUI.<>f__mg$cache3 == null)
			{
				EditorLevelObjectsUI.<>f__mg$cache3 = new Toggled(EditorLevelObjectsUI.onToggledLargeToggle);
			}
			sleekToggle.onToggled = EditorLevelObjectsUI.<>f__mg$cache3;
			EditorLevelObjectsUI.container.add(EditorLevelObjectsUI.largeToggle);
			EditorLevelObjectsUI.mediumToggle = new SleekToggle();
			EditorLevelObjectsUI.mediumToggle.positionOffset_X = -230;
			EditorLevelObjectsUI.mediumToggle.positionOffset_Y = 210;
			EditorLevelObjectsUI.mediumToggle.positionScale_X = 1f;
			EditorLevelObjectsUI.mediumToggle.sizeOffset_X = 40;
			EditorLevelObjectsUI.mediumToggle.sizeOffset_Y = 40;
			EditorLevelObjectsUI.mediumToggle.addLabel(local.format("MediumLabel"), ESleekSide.RIGHT);
			EditorLevelObjectsUI.mediumToggle.state = true;
			SleekToggle sleekToggle2 = EditorLevelObjectsUI.mediumToggle;
			if (EditorLevelObjectsUI.<>f__mg$cache4 == null)
			{
				EditorLevelObjectsUI.<>f__mg$cache4 = new Toggled(EditorLevelObjectsUI.onToggledMediumToggle);
			}
			sleekToggle2.onToggled = EditorLevelObjectsUI.<>f__mg$cache4;
			EditorLevelObjectsUI.container.add(EditorLevelObjectsUI.mediumToggle);
			EditorLevelObjectsUI.smallToggle = new SleekToggle();
			EditorLevelObjectsUI.smallToggle.positionOffset_X = -230;
			EditorLevelObjectsUI.smallToggle.positionOffset_Y = 260;
			EditorLevelObjectsUI.smallToggle.positionScale_X = 1f;
			EditorLevelObjectsUI.smallToggle.sizeOffset_X = 40;
			EditorLevelObjectsUI.smallToggle.sizeOffset_Y = 40;
			EditorLevelObjectsUI.smallToggle.addLabel(local.format("SmallLabel"), ESleekSide.RIGHT);
			EditorLevelObjectsUI.smallToggle.state = true;
			SleekToggle sleekToggle3 = EditorLevelObjectsUI.smallToggle;
			if (EditorLevelObjectsUI.<>f__mg$cache5 == null)
			{
				EditorLevelObjectsUI.<>f__mg$cache5 = new Toggled(EditorLevelObjectsUI.onToggledSmallToggle);
			}
			sleekToggle3.onToggled = EditorLevelObjectsUI.<>f__mg$cache5;
			EditorLevelObjectsUI.container.add(EditorLevelObjectsUI.smallToggle);
			EditorLevelObjectsUI.barricadesToggle = new SleekToggle();
			EditorLevelObjectsUI.barricadesToggle.positionOffset_X = -130;
			EditorLevelObjectsUI.barricadesToggle.positionOffset_Y = 160;
			EditorLevelObjectsUI.barricadesToggle.positionScale_X = 1f;
			EditorLevelObjectsUI.barricadesToggle.sizeOffset_X = 40;
			EditorLevelObjectsUI.barricadesToggle.sizeOffset_Y = 40;
			EditorLevelObjectsUI.barricadesToggle.addLabel(local.format("BarricadesLabel"), ESleekSide.RIGHT);
			EditorLevelObjectsUI.barricadesToggle.state = true;
			SleekToggle sleekToggle4 = EditorLevelObjectsUI.barricadesToggle;
			if (EditorLevelObjectsUI.<>f__mg$cache6 == null)
			{
				EditorLevelObjectsUI.<>f__mg$cache6 = new Toggled(EditorLevelObjectsUI.onToggledBarricadesToggle);
			}
			sleekToggle4.onToggled = EditorLevelObjectsUI.<>f__mg$cache6;
			EditorLevelObjectsUI.container.add(EditorLevelObjectsUI.barricadesToggle);
			EditorLevelObjectsUI.structuresToggle = new SleekToggle();
			EditorLevelObjectsUI.structuresToggle.positionOffset_X = -130;
			EditorLevelObjectsUI.structuresToggle.positionOffset_Y = 210;
			EditorLevelObjectsUI.structuresToggle.positionScale_X = 1f;
			EditorLevelObjectsUI.structuresToggle.sizeOffset_X = 40;
			EditorLevelObjectsUI.structuresToggle.sizeOffset_Y = 40;
			EditorLevelObjectsUI.structuresToggle.addLabel(local.format("StructuresLabel"), ESleekSide.RIGHT);
			EditorLevelObjectsUI.structuresToggle.state = true;
			SleekToggle sleekToggle5 = EditorLevelObjectsUI.structuresToggle;
			if (EditorLevelObjectsUI.<>f__mg$cache7 == null)
			{
				EditorLevelObjectsUI.<>f__mg$cache7 = new Toggled(EditorLevelObjectsUI.onToggledStructuresToggle);
			}
			sleekToggle5.onToggled = EditorLevelObjectsUI.<>f__mg$cache7;
			EditorLevelObjectsUI.container.add(EditorLevelObjectsUI.structuresToggle);
			EditorLevelObjectsUI.npcsToggle = new SleekToggle();
			EditorLevelObjectsUI.npcsToggle.positionOffset_X = -130;
			EditorLevelObjectsUI.npcsToggle.positionOffset_Y = 260;
			EditorLevelObjectsUI.npcsToggle.positionScale_X = 1f;
			EditorLevelObjectsUI.npcsToggle.sizeOffset_X = 40;
			EditorLevelObjectsUI.npcsToggle.sizeOffset_Y = 40;
			EditorLevelObjectsUI.npcsToggle.addLabel(local.format("NPCsLabel"), ESleekSide.RIGHT);
			EditorLevelObjectsUI.npcsToggle.state = true;
			SleekToggle sleekToggle6 = EditorLevelObjectsUI.npcsToggle;
			if (EditorLevelObjectsUI.<>f__mg$cache8 == null)
			{
				EditorLevelObjectsUI.<>f__mg$cache8 = new Toggled(EditorLevelObjectsUI.onToggledNPCsToggle);
			}
			sleekToggle6.onToggled = EditorLevelObjectsUI.<>f__mg$cache8;
			EditorLevelObjectsUI.container.add(EditorLevelObjectsUI.npcsToggle);
			EditorLevelObjectsUI.assetsScrollBox = new SleekScrollBox();
			EditorLevelObjectsUI.assetsScrollBox.positionOffset_X = -230;
			EditorLevelObjectsUI.assetsScrollBox.positionOffset_Y = 310;
			EditorLevelObjectsUI.assetsScrollBox.positionScale_X = 1f;
			EditorLevelObjectsUI.assetsScrollBox.sizeOffset_X = 230;
			EditorLevelObjectsUI.assetsScrollBox.sizeOffset_Y = -310;
			EditorLevelObjectsUI.assetsScrollBox.sizeScale_Y = 1f;
			EditorLevelObjectsUI.container.add(EditorLevelObjectsUI.assetsScrollBox);
			EditorObjects.selectedObjectAsset = null;
			EditorObjects.selectedItemAsset = null;
			if (EditorLevelObjectsUI.<>f__mg$cache9 == null)
			{
				EditorLevelObjectsUI.<>f__mg$cache9 = new DragStarted(EditorLevelObjectsUI.onDragStarted);
			}
			EditorObjects.onDragStarted = EditorLevelObjectsUI.<>f__mg$cache9;
			if (EditorLevelObjectsUI.<>f__mg$cacheA == null)
			{
				EditorLevelObjectsUI.<>f__mg$cacheA = new DragStopped(EditorLevelObjectsUI.onDragStopped);
			}
			EditorObjects.onDragStopped = EditorLevelObjectsUI.<>f__mg$cacheA;
			EditorLevelObjectsUI.dragBox = new SleekBox();
			EditorUI.window.add(EditorLevelObjectsUI.dragBox);
			EditorLevelObjectsUI.dragBox.isVisible = false;
			EditorLevelObjectsUI.snapTransformField = new SleekSingleField();
			EditorLevelObjectsUI.snapTransformField.positionOffset_Y = -230;
			EditorLevelObjectsUI.snapTransformField.positionScale_Y = 1f;
			EditorLevelObjectsUI.snapTransformField.sizeOffset_X = 200;
			EditorLevelObjectsUI.snapTransformField.sizeOffset_Y = 30;
			EditorLevelObjectsUI.snapTransformField.text = (Mathf.Floor(EditorObjects.snapTransform * 100f) / 100f).ToString();
			EditorLevelObjectsUI.snapTransformField.addLabel(local.format("SnapTransformLabelText"), ESleekSide.RIGHT);
			SleekSingleField sleekSingleField = EditorLevelObjectsUI.snapTransformField;
			if (EditorLevelObjectsUI.<>f__mg$cacheB == null)
			{
				EditorLevelObjectsUI.<>f__mg$cacheB = new TypedSingle(EditorLevelObjectsUI.onTypedSnapTransformField);
			}
			sleekSingleField.onTypedSingle = EditorLevelObjectsUI.<>f__mg$cacheB;
			EditorLevelObjectsUI.container.add(EditorLevelObjectsUI.snapTransformField);
			EditorLevelObjectsUI.snapRotationField = new SleekSingleField();
			EditorLevelObjectsUI.snapRotationField.positionOffset_Y = -190;
			EditorLevelObjectsUI.snapRotationField.positionScale_Y = 1f;
			EditorLevelObjectsUI.snapRotationField.sizeOffset_X = 200;
			EditorLevelObjectsUI.snapRotationField.sizeOffset_Y = 30;
			EditorLevelObjectsUI.snapRotationField.text = (Mathf.Floor(EditorObjects.snapRotation * 100f) / 100f).ToString();
			EditorLevelObjectsUI.snapRotationField.addLabel(local.format("SnapRotationLabelText"), ESleekSide.RIGHT);
			SleekSingleField sleekSingleField2 = EditorLevelObjectsUI.snapRotationField;
			if (EditorLevelObjectsUI.<>f__mg$cacheC == null)
			{
				EditorLevelObjectsUI.<>f__mg$cacheC = new TypedSingle(EditorLevelObjectsUI.onTypedSnapRotationField);
			}
			sleekSingleField2.onTypedSingle = EditorLevelObjectsUI.<>f__mg$cacheC;
			EditorLevelObjectsUI.container.add(EditorLevelObjectsUI.snapRotationField);
			EditorLevelObjectsUI.transformButton = new SleekButtonIcon((Texture2D)bundle.load("Transform"));
			EditorLevelObjectsUI.transformButton.positionOffset_Y = -150;
			EditorLevelObjectsUI.transformButton.positionScale_Y = 1f;
			EditorLevelObjectsUI.transformButton.sizeOffset_X = 200;
			EditorLevelObjectsUI.transformButton.sizeOffset_Y = 30;
			EditorLevelObjectsUI.transformButton.text = local.format("TransformButtonText", new object[]
			{
				ControlsSettings.tool_0
			});
			EditorLevelObjectsUI.transformButton.tooltip = local.format("TransformButtonTooltip");
			SleekButton sleekButton2 = EditorLevelObjectsUI.transformButton;
			if (EditorLevelObjectsUI.<>f__mg$cacheD == null)
			{
				EditorLevelObjectsUI.<>f__mg$cacheD = new ClickedButton(EditorLevelObjectsUI.onClickedTransformButton);
			}
			sleekButton2.onClickedButton = EditorLevelObjectsUI.<>f__mg$cacheD;
			EditorLevelObjectsUI.container.add(EditorLevelObjectsUI.transformButton);
			EditorLevelObjectsUI.rotateButton = new SleekButtonIcon((Texture2D)bundle.load("Rotate"));
			EditorLevelObjectsUI.rotateButton.positionOffset_Y = -110;
			EditorLevelObjectsUI.rotateButton.positionScale_Y = 1f;
			EditorLevelObjectsUI.rotateButton.sizeOffset_X = 200;
			EditorLevelObjectsUI.rotateButton.sizeOffset_Y = 30;
			EditorLevelObjectsUI.rotateButton.text = local.format("RotateButtonText", new object[]
			{
				ControlsSettings.tool_1
			});
			EditorLevelObjectsUI.rotateButton.tooltip = local.format("RotateButtonTooltip");
			SleekButton sleekButton3 = EditorLevelObjectsUI.rotateButton;
			if (EditorLevelObjectsUI.<>f__mg$cacheE == null)
			{
				EditorLevelObjectsUI.<>f__mg$cacheE = new ClickedButton(EditorLevelObjectsUI.onClickedRotateButton);
			}
			sleekButton3.onClickedButton = EditorLevelObjectsUI.<>f__mg$cacheE;
			EditorLevelObjectsUI.container.add(EditorLevelObjectsUI.rotateButton);
			EditorLevelObjectsUI.scaleButton = new SleekButtonIcon((Texture2D)bundle.load("Scale"));
			EditorLevelObjectsUI.scaleButton.positionOffset_Y = -70;
			EditorLevelObjectsUI.scaleButton.positionScale_Y = 1f;
			EditorLevelObjectsUI.scaleButton.sizeOffset_X = 200;
			EditorLevelObjectsUI.scaleButton.sizeOffset_Y = 30;
			EditorLevelObjectsUI.scaleButton.text = local.format("ScaleButtonText", new object[]
			{
				ControlsSettings.tool_3
			});
			EditorLevelObjectsUI.scaleButton.tooltip = local.format("ScaleButtonTooltip");
			SleekButton sleekButton4 = EditorLevelObjectsUI.scaleButton;
			if (EditorLevelObjectsUI.<>f__mg$cacheF == null)
			{
				EditorLevelObjectsUI.<>f__mg$cacheF = new ClickedButton(EditorLevelObjectsUI.onClickedScaleButton);
			}
			sleekButton4.onClickedButton = EditorLevelObjectsUI.<>f__mg$cacheF;
			EditorLevelObjectsUI.container.add(EditorLevelObjectsUI.scaleButton);
			EditorLevelObjectsUI.coordinateButton = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(local.format("CoordinateButtonTextGlobal"), (Texture)bundle.load("Global")),
				new GUIContent(local.format("CoordinateButtonTextLocal"), (Texture)bundle.load("Local"))
			});
			EditorLevelObjectsUI.coordinateButton.positionOffset_Y = -30;
			EditorLevelObjectsUI.coordinateButton.positionScale_Y = 1f;
			EditorLevelObjectsUI.coordinateButton.sizeOffset_X = 200;
			EditorLevelObjectsUI.coordinateButton.sizeOffset_Y = 30;
			EditorLevelObjectsUI.coordinateButton.tooltip = local.format("CoordinateButtonTooltip");
			SleekButtonState sleekButtonState = EditorLevelObjectsUI.coordinateButton;
			if (EditorLevelObjectsUI.<>f__mg$cache10 == null)
			{
				EditorLevelObjectsUI.<>f__mg$cache10 = new SwappedState(EditorLevelObjectsUI.onSwappedStateCoordinate);
			}
			sleekButtonState.onSwappedState = EditorLevelObjectsUI.<>f__mg$cache10;
			EditorLevelObjectsUI.container.add(EditorLevelObjectsUI.coordinateButton);
			bundle.unload();
			EditorLevelObjectsUI.onAssetsRefreshed();
			if (EditorLevelObjectsUI.<>f__mg$cache11 == null)
			{
				EditorLevelObjectsUI.<>f__mg$cache11 = new AssetsRefreshed(EditorLevelObjectsUI.onAssetsRefreshed);
			}
			Assets.onAssetsRefreshed = EditorLevelObjectsUI.<>f__mg$cache11;
		}

		public static void open()
		{
			if (EditorLevelObjectsUI.active)
			{
				return;
			}
			EditorLevelObjectsUI.active = true;
			EditorObjects.isBuilding = true;
			EditorUI.message(EEditorMessage.OBJECTS);
			EditorLevelObjectsUI.container.lerpPositionScale(0f, 0f, ESleekLerp.EXPONENTIAL, 20f);
		}

		public static void close()
		{
			if (!EditorLevelObjectsUI.active)
			{
				return;
			}
			EditorLevelObjectsUI.active = false;
			EditorObjects.isBuilding = false;
			EditorLevelObjectsUI.container.lerpPositionScale(1f, 0f, ESleekLerp.EXPONENTIAL, 20f);
		}

		private static void updateSelection(string search, bool large, bool medium, bool small, bool barricades, bool structures, bool npcs)
		{
			if (EditorLevelObjectsUI.assets == null)
			{
				return;
			}
			EditorLevelObjectsUI.assets.Clear();
			if (large || medium || small || npcs)
			{
				Asset[] array = Assets.find(EAssetType.OBJECT);
				for (int i = 0; i < array.Length; i++)
				{
					ObjectAsset objectAsset = array[i] as ObjectAsset;
					if (large || objectAsset.type != EObjectType.LARGE)
					{
						if (medium || objectAsset.type != EObjectType.MEDIUM)
						{
							if (small || objectAsset.type != EObjectType.SMALL)
							{
								if (npcs || objectAsset.type != EObjectType.NPC)
								{
									if (search == null || search.Length <= 0 || objectAsset.objectName.IndexOf(search, StringComparison.OrdinalIgnoreCase) != -1)
									{
										EditorLevelObjectsUI.assets.Add(objectAsset);
									}
								}
							}
						}
					}
				}
			}
			if (barricades || structures)
			{
				Asset[] array2 = Assets.find(EAssetType.ITEM);
				int j = 0;
				while (j < array2.Length)
				{
					ItemAsset itemAsset = array2[j] as ItemAsset;
					if (itemAsset.type == EItemType.BARRICADE)
					{
						if (barricades)
						{
							goto IL_14E;
						}
					}
					else if (itemAsset.type == EItemType.STRUCTURE)
					{
						if (structures)
						{
							goto IL_14E;
						}
					}
					IL_185:
					j++;
					continue;
					IL_14E:
					if (search != null && search.Length > 0 && itemAsset.itemName.IndexOf(search, StringComparison.OrdinalIgnoreCase) == -1)
					{
						goto IL_185;
					}
					EditorLevelObjectsUI.assets.Add(itemAsset);
					goto IL_185;
				}
			}
			EditorLevelObjectsUI.assets.Sort(EditorLevelObjectsUI.comparator);
			EditorLevelObjectsUI.assetsScrollBox.remove();
			EditorLevelObjectsUI.assetsScrollBox.area = new Rect(0f, 0f, 5f, (float)(EditorLevelObjectsUI.assets.Count * 40 - 10));
			for (int k = 0; k < EditorLevelObjectsUI.assets.Count; k++)
			{
				string text = string.Empty;
				ObjectAsset objectAsset2 = EditorLevelObjectsUI.assets[k] as ObjectAsset;
				ItemAsset itemAsset2 = EditorLevelObjectsUI.assets[k] as ItemAsset;
				if (objectAsset2 != null)
				{
					text = objectAsset2.objectName;
				}
				else if (itemAsset2 != null)
				{
					text = itemAsset2.itemName;
				}
				SleekButton sleekButton = new SleekButton();
				sleekButton.positionOffset_Y = k * 40;
				sleekButton.sizeOffset_X = 200;
				sleekButton.sizeOffset_Y = 30;
				sleekButton.text = text;
				SleekButton sleekButton2 = sleekButton;
				if (EditorLevelObjectsUI.<>f__mg$cache0 == null)
				{
					EditorLevelObjectsUI.<>f__mg$cache0 = new ClickedButton(EditorLevelObjectsUI.onClickedAssetButton);
				}
				sleekButton2.onClickedButton = EditorLevelObjectsUI.<>f__mg$cache0;
				EditorLevelObjectsUI.assetsScrollBox.add(sleekButton);
			}
		}

		private static void onAssetsRefreshed()
		{
			EditorLevelObjectsUI.updateSelection(EditorLevelObjectsUI.searchField.text, EditorLevelObjectsUI.largeToggle.state, EditorLevelObjectsUI.mediumToggle.state, EditorLevelObjectsUI.smallToggle.state, EditorLevelObjectsUI.barricadesToggle.state, EditorLevelObjectsUI.structuresToggle.state, EditorLevelObjectsUI.npcsToggle.state);
		}

		private static void onClickedAssetButton(SleekButton button)
		{
			int index = button.positionOffset_Y / 40;
			EditorObjects.selectedObjectAsset = (EditorLevelObjectsUI.assets[index] as ObjectAsset);
			EditorObjects.selectedItemAsset = (EditorLevelObjectsUI.assets[index] as ItemAsset);
			if (EditorObjects.selectedObjectAsset != null)
			{
				EditorLevelObjectsUI.selectedBox.text = EditorObjects.selectedObjectAsset.objectName;
			}
			else if (EditorObjects.selectedItemAsset != null)
			{
				EditorLevelObjectsUI.selectedBox.text = EditorObjects.selectedItemAsset.itemName;
			}
		}

		private static void onDragStarted(int min_x, int min_y, int max_x, int max_y)
		{
			EditorLevelObjectsUI.dragBox.positionOffset_X = min_x;
			EditorLevelObjectsUI.dragBox.positionOffset_Y = min_y;
			EditorLevelObjectsUI.dragBox.sizeOffset_X = max_x - min_x;
			EditorLevelObjectsUI.dragBox.sizeOffset_Y = max_y - min_y;
			EditorLevelObjectsUI.dragBox.isVisible = true;
		}

		private static void onDragStopped()
		{
			EditorLevelObjectsUI.dragBox.isVisible = false;
		}

		private static void onEnteredSearchField(SleekField field)
		{
			EditorLevelObjectsUI.updateSelection(EditorLevelObjectsUI.searchField.text, EditorLevelObjectsUI.largeToggle.state, EditorLevelObjectsUI.mediumToggle.state, EditorLevelObjectsUI.smallToggle.state, EditorLevelObjectsUI.barricadesToggle.state, EditorLevelObjectsUI.structuresToggle.state, EditorLevelObjectsUI.npcsToggle.state);
		}

		private static void onClickedSearchButton(SleekButton button)
		{
			EditorLevelObjectsUI.updateSelection(EditorLevelObjectsUI.searchField.text, EditorLevelObjectsUI.largeToggle.state, EditorLevelObjectsUI.mediumToggle.state, EditorLevelObjectsUI.smallToggle.state, EditorLevelObjectsUI.barricadesToggle.state, EditorLevelObjectsUI.structuresToggle.state, EditorLevelObjectsUI.npcsToggle.state);
		}

		private static void onToggledLargeToggle(SleekToggle toggle, bool state)
		{
			EditorLevelObjectsUI.updateSelection(EditorLevelObjectsUI.searchField.text, state, EditorLevelObjectsUI.mediumToggle.state, EditorLevelObjectsUI.smallToggle.state, EditorLevelObjectsUI.barricadesToggle.state, EditorLevelObjectsUI.structuresToggle.state, EditorLevelObjectsUI.npcsToggle.state);
		}

		private static void onToggledMediumToggle(SleekToggle toggle, bool state)
		{
			EditorLevelObjectsUI.updateSelection(EditorLevelObjectsUI.searchField.text, EditorLevelObjectsUI.largeToggle.state, state, EditorLevelObjectsUI.smallToggle.state, EditorLevelObjectsUI.barricadesToggle.state, EditorLevelObjectsUI.structuresToggle.state, EditorLevelObjectsUI.npcsToggle.state);
		}

		private static void onToggledSmallToggle(SleekToggle toggle, bool state)
		{
			EditorLevelObjectsUI.updateSelection(EditorLevelObjectsUI.searchField.text, EditorLevelObjectsUI.largeToggle.state, EditorLevelObjectsUI.mediumToggle.state, state, EditorLevelObjectsUI.barricadesToggle.state, EditorLevelObjectsUI.structuresToggle.state, EditorLevelObjectsUI.npcsToggle.state);
		}

		private static void onToggledBarricadesToggle(SleekToggle toggle, bool state)
		{
			EditorLevelObjectsUI.updateSelection(EditorLevelObjectsUI.searchField.text, EditorLevelObjectsUI.largeToggle.state, EditorLevelObjectsUI.mediumToggle.state, EditorLevelObjectsUI.smallToggle.state, state, EditorLevelObjectsUI.structuresToggle.state, EditorLevelObjectsUI.npcsToggle.state);
		}

		private static void onToggledStructuresToggle(SleekToggle toggle, bool state)
		{
			EditorLevelObjectsUI.updateSelection(EditorLevelObjectsUI.searchField.text, EditorLevelObjectsUI.largeToggle.state, EditorLevelObjectsUI.mediumToggle.state, EditorLevelObjectsUI.smallToggle.state, EditorLevelObjectsUI.barricadesToggle.state, state, EditorLevelObjectsUI.npcsToggle.state);
		}

		private static void onToggledNPCsToggle(SleekToggle toggle, bool state)
		{
			EditorLevelObjectsUI.updateSelection(EditorLevelObjectsUI.searchField.text, EditorLevelObjectsUI.largeToggle.state, EditorLevelObjectsUI.mediumToggle.state, EditorLevelObjectsUI.smallToggle.state, EditorLevelObjectsUI.barricadesToggle.state, EditorLevelObjectsUI.structuresToggle.state, state);
		}

		private static void onTypedSnapTransformField(SleekSingleField field, float value)
		{
			EditorObjects.snapTransform = value;
		}

		private static void onTypedSnapRotationField(SleekSingleField field, float value)
		{
			EditorObjects.snapRotation = value;
		}

		private static void onClickedTransformButton(SleekButton button)
		{
			EditorObjects.dragMode = EDragMode.TRANSFORM;
		}

		private static void onClickedRotateButton(SleekButton button)
		{
			EditorObjects.dragMode = EDragMode.ROTATE;
		}

		private static void onClickedScaleButton(SleekButton button)
		{
			EditorObjects.dragMode = EDragMode.SCALE;
		}

		private static void onSwappedStateCoordinate(SleekButtonState button, int index)
		{
			EditorObjects.dragCoordinate = (EDragCoordinate)index;
		}

		private static Sleek container;

		public static bool active;

		private static List<Asset> assets;

		private static AssetNameAscendingComparator comparator = new AssetNameAscendingComparator();

		private static SleekScrollBox assetsScrollBox;

		private static SleekBox selectedBox;

		private static SleekField searchField;

		private static SleekButton searchButton;

		private static SleekToggle largeToggle;

		private static SleekToggle mediumToggle;

		private static SleekToggle smallToggle;

		private static SleekToggle barricadesToggle;

		private static SleekToggle structuresToggle;

		private static SleekToggle npcsToggle;

		private static SleekBox dragBox;

		private static SleekSingleField snapTransformField;

		private static SleekSingleField snapRotationField;

		private static SleekButtonIcon transformButton;

		private static SleekButtonIcon rotateButton;

		private static SleekButtonIcon scaleButton;

		public static SleekButtonState coordinateButton;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache0;

		[CompilerGenerated]
		private static Entered <>f__mg$cache1;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache2;

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
		private static DragStarted <>f__mg$cache9;

		[CompilerGenerated]
		private static DragStopped <>f__mg$cacheA;

		[CompilerGenerated]
		private static TypedSingle <>f__mg$cacheB;

		[CompilerGenerated]
		private static TypedSingle <>f__mg$cacheC;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cacheD;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cacheE;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cacheF;

		[CompilerGenerated]
		private static SwappedState <>f__mg$cache10;

		[CompilerGenerated]
		private static AssetsRefreshed <>f__mg$cache11;
	}
}
