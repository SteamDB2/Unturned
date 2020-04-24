using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace SDG.Unturned
{
	public class EditorLevelVisibilityUI
	{
		public EditorLevelVisibilityUI()
		{
			EditorLevelVisibilityUI.localization = Localization.read("/Editor/EditorLevelVisibility.dat");
			EditorLevelVisibilityUI.container = new Sleek();
			EditorLevelVisibilityUI.container.positionScale_X = 1f;
			EditorLevelVisibilityUI.container.sizeScale_X = 1f;
			EditorLevelVisibilityUI.container.sizeScale_Y = 1f;
			EditorUI.window.add(EditorLevelVisibilityUI.container);
			EditorLevelVisibilityUI.active = false;
			EditorLevelVisibilityUI.roadsToggle = new SleekToggle();
			EditorLevelVisibilityUI.roadsToggle.positionOffset_X = -210;
			EditorLevelVisibilityUI.roadsToggle.positionOffset_Y = 90;
			EditorLevelVisibilityUI.roadsToggle.positionScale_X = 1f;
			EditorLevelVisibilityUI.roadsToggle.sizeOffset_X = 40;
			EditorLevelVisibilityUI.roadsToggle.sizeOffset_Y = 40;
			EditorLevelVisibilityUI.roadsToggle.state = LevelVisibility.roadsVisible;
			EditorLevelVisibilityUI.roadsToggle.addLabel(EditorLevelVisibilityUI.localization.format("Roads_Label"), ESleekSide.RIGHT);
			SleekToggle sleekToggle = EditorLevelVisibilityUI.roadsToggle;
			if (EditorLevelVisibilityUI.<>f__mg$cache0 == null)
			{
				EditorLevelVisibilityUI.<>f__mg$cache0 = new Toggled(EditorLevelVisibilityUI.onToggledRoadsToggle);
			}
			sleekToggle.onToggled = EditorLevelVisibilityUI.<>f__mg$cache0;
			EditorLevelVisibilityUI.container.add(EditorLevelVisibilityUI.roadsToggle);
			EditorLevelVisibilityUI.navigationToggle = new SleekToggle();
			EditorLevelVisibilityUI.navigationToggle.positionOffset_X = -210;
			EditorLevelVisibilityUI.navigationToggle.positionOffset_Y = 140;
			EditorLevelVisibilityUI.navigationToggle.positionScale_X = 1f;
			EditorLevelVisibilityUI.navigationToggle.sizeOffset_X = 40;
			EditorLevelVisibilityUI.navigationToggle.sizeOffset_Y = 40;
			EditorLevelVisibilityUI.navigationToggle.state = LevelVisibility.navigationVisible;
			EditorLevelVisibilityUI.navigationToggle.addLabel(EditorLevelVisibilityUI.localization.format("Navigation_Label"), ESleekSide.RIGHT);
			SleekToggle sleekToggle2 = EditorLevelVisibilityUI.navigationToggle;
			if (EditorLevelVisibilityUI.<>f__mg$cache1 == null)
			{
				EditorLevelVisibilityUI.<>f__mg$cache1 = new Toggled(EditorLevelVisibilityUI.onToggledNavigationToggle);
			}
			sleekToggle2.onToggled = EditorLevelVisibilityUI.<>f__mg$cache1;
			EditorLevelVisibilityUI.container.add(EditorLevelVisibilityUI.navigationToggle);
			EditorLevelVisibilityUI.nodesToggle = new SleekToggle();
			EditorLevelVisibilityUI.nodesToggle.positionOffset_X = -210;
			EditorLevelVisibilityUI.nodesToggle.positionOffset_Y = 190;
			EditorLevelVisibilityUI.nodesToggle.positionScale_X = 1f;
			EditorLevelVisibilityUI.nodesToggle.sizeOffset_X = 40;
			EditorLevelVisibilityUI.nodesToggle.sizeOffset_Y = 40;
			EditorLevelVisibilityUI.nodesToggle.state = LevelVisibility.nodesVisible;
			EditorLevelVisibilityUI.nodesToggle.addLabel(EditorLevelVisibilityUI.localization.format("Nodes_Label"), ESleekSide.RIGHT);
			SleekToggle sleekToggle3 = EditorLevelVisibilityUI.nodesToggle;
			if (EditorLevelVisibilityUI.<>f__mg$cache2 == null)
			{
				EditorLevelVisibilityUI.<>f__mg$cache2 = new Toggled(EditorLevelVisibilityUI.onToggledNodesToggle);
			}
			sleekToggle3.onToggled = EditorLevelVisibilityUI.<>f__mg$cache2;
			EditorLevelVisibilityUI.container.add(EditorLevelVisibilityUI.nodesToggle);
			EditorLevelVisibilityUI.itemsToggle = new SleekToggle();
			EditorLevelVisibilityUI.itemsToggle.positionOffset_X = -210;
			EditorLevelVisibilityUI.itemsToggle.positionOffset_Y = 240;
			EditorLevelVisibilityUI.itemsToggle.positionScale_X = 1f;
			EditorLevelVisibilityUI.itemsToggle.sizeOffset_X = 40;
			EditorLevelVisibilityUI.itemsToggle.sizeOffset_Y = 40;
			EditorLevelVisibilityUI.itemsToggle.state = LevelVisibility.itemsVisible;
			EditorLevelVisibilityUI.itemsToggle.addLabel(EditorLevelVisibilityUI.localization.format("Items_Label"), ESleekSide.RIGHT);
			SleekToggle sleekToggle4 = EditorLevelVisibilityUI.itemsToggle;
			if (EditorLevelVisibilityUI.<>f__mg$cache3 == null)
			{
				EditorLevelVisibilityUI.<>f__mg$cache3 = new Toggled(EditorLevelVisibilityUI.onToggledItemsToggle);
			}
			sleekToggle4.onToggled = EditorLevelVisibilityUI.<>f__mg$cache3;
			EditorLevelVisibilityUI.container.add(EditorLevelVisibilityUI.itemsToggle);
			EditorLevelVisibilityUI.playersToggle = new SleekToggle();
			EditorLevelVisibilityUI.playersToggle.positionOffset_X = -210;
			EditorLevelVisibilityUI.playersToggle.positionOffset_Y = 290;
			EditorLevelVisibilityUI.playersToggle.positionScale_X = 1f;
			EditorLevelVisibilityUI.playersToggle.sizeOffset_X = 40;
			EditorLevelVisibilityUI.playersToggle.sizeOffset_Y = 40;
			EditorLevelVisibilityUI.playersToggle.state = LevelVisibility.playersVisible;
			EditorLevelVisibilityUI.playersToggle.addLabel(EditorLevelVisibilityUI.localization.format("Players_Label"), ESleekSide.RIGHT);
			SleekToggle sleekToggle5 = EditorLevelVisibilityUI.playersToggle;
			if (EditorLevelVisibilityUI.<>f__mg$cache4 == null)
			{
				EditorLevelVisibilityUI.<>f__mg$cache4 = new Toggled(EditorLevelVisibilityUI.onToggledPlayersToggle);
			}
			sleekToggle5.onToggled = EditorLevelVisibilityUI.<>f__mg$cache4;
			EditorLevelVisibilityUI.container.add(EditorLevelVisibilityUI.playersToggle);
			EditorLevelVisibilityUI.zombiesToggle = new SleekToggle();
			EditorLevelVisibilityUI.zombiesToggle.positionOffset_X = -210;
			EditorLevelVisibilityUI.zombiesToggle.positionOffset_Y = 340;
			EditorLevelVisibilityUI.zombiesToggle.positionScale_X = 1f;
			EditorLevelVisibilityUI.zombiesToggle.sizeOffset_X = 40;
			EditorLevelVisibilityUI.zombiesToggle.sizeOffset_Y = 40;
			EditorLevelVisibilityUI.zombiesToggle.state = LevelVisibility.zombiesVisible;
			EditorLevelVisibilityUI.zombiesToggle.addLabel(EditorLevelVisibilityUI.localization.format("Zombies_Label"), ESleekSide.RIGHT);
			SleekToggle sleekToggle6 = EditorLevelVisibilityUI.zombiesToggle;
			if (EditorLevelVisibilityUI.<>f__mg$cache5 == null)
			{
				EditorLevelVisibilityUI.<>f__mg$cache5 = new Toggled(EditorLevelVisibilityUI.onToggledZombiesToggle);
			}
			sleekToggle6.onToggled = EditorLevelVisibilityUI.<>f__mg$cache5;
			EditorLevelVisibilityUI.container.add(EditorLevelVisibilityUI.zombiesToggle);
			EditorLevelVisibilityUI.vehiclesToggle = new SleekToggle();
			EditorLevelVisibilityUI.vehiclesToggle.positionOffset_X = -210;
			EditorLevelVisibilityUI.vehiclesToggle.positionOffset_Y = 390;
			EditorLevelVisibilityUI.vehiclesToggle.positionScale_X = 1f;
			EditorLevelVisibilityUI.vehiclesToggle.sizeOffset_X = 40;
			EditorLevelVisibilityUI.vehiclesToggle.sizeOffset_Y = 40;
			EditorLevelVisibilityUI.vehiclesToggle.state = LevelVisibility.vehiclesVisible;
			EditorLevelVisibilityUI.vehiclesToggle.addLabel(EditorLevelVisibilityUI.localization.format("Vehicles_Label"), ESleekSide.RIGHT);
			SleekToggle sleekToggle7 = EditorLevelVisibilityUI.vehiclesToggle;
			if (EditorLevelVisibilityUI.<>f__mg$cache6 == null)
			{
				EditorLevelVisibilityUI.<>f__mg$cache6 = new Toggled(EditorLevelVisibilityUI.onToggledVehiclesToggle);
			}
			sleekToggle7.onToggled = EditorLevelVisibilityUI.<>f__mg$cache6;
			EditorLevelVisibilityUI.container.add(EditorLevelVisibilityUI.vehiclesToggle);
			EditorLevelVisibilityUI.borderToggle = new SleekToggle();
			EditorLevelVisibilityUI.borderToggle.positionOffset_X = -210;
			EditorLevelVisibilityUI.borderToggle.positionOffset_Y = 440;
			EditorLevelVisibilityUI.borderToggle.positionScale_X = 1f;
			EditorLevelVisibilityUI.borderToggle.sizeOffset_X = 40;
			EditorLevelVisibilityUI.borderToggle.sizeOffset_Y = 40;
			EditorLevelVisibilityUI.borderToggle.state = LevelVisibility.borderVisible;
			EditorLevelVisibilityUI.borderToggle.addLabel(EditorLevelVisibilityUI.localization.format("Border_Label"), ESleekSide.RIGHT);
			SleekToggle sleekToggle8 = EditorLevelVisibilityUI.borderToggle;
			if (EditorLevelVisibilityUI.<>f__mg$cache7 == null)
			{
				EditorLevelVisibilityUI.<>f__mg$cache7 = new Toggled(EditorLevelVisibilityUI.onToggledBorderToggle);
			}
			sleekToggle8.onToggled = EditorLevelVisibilityUI.<>f__mg$cache7;
			EditorLevelVisibilityUI.container.add(EditorLevelVisibilityUI.borderToggle);
			EditorLevelVisibilityUI.animalsToggle = new SleekToggle();
			EditorLevelVisibilityUI.animalsToggle.positionOffset_X = -210;
			EditorLevelVisibilityUI.animalsToggle.positionOffset_Y = 490;
			EditorLevelVisibilityUI.animalsToggle.positionScale_X = 1f;
			EditorLevelVisibilityUI.animalsToggle.sizeOffset_X = 40;
			EditorLevelVisibilityUI.animalsToggle.sizeOffset_Y = 40;
			EditorLevelVisibilityUI.animalsToggle.state = LevelVisibility.animalsVisible;
			EditorLevelVisibilityUI.animalsToggle.addLabel(EditorLevelVisibilityUI.localization.format("Animals_Label"), ESleekSide.RIGHT);
			SleekToggle sleekToggle9 = EditorLevelVisibilityUI.animalsToggle;
			if (EditorLevelVisibilityUI.<>f__mg$cache8 == null)
			{
				EditorLevelVisibilityUI.<>f__mg$cache8 = new Toggled(EditorLevelVisibilityUI.onToggledAnimalsToggle);
			}
			sleekToggle9.onToggled = EditorLevelVisibilityUI.<>f__mg$cache8;
			EditorLevelVisibilityUI.container.add(EditorLevelVisibilityUI.animalsToggle);
			EditorLevelVisibilityUI.regionLabels = new SleekLabel[(int)(EditorLevelVisibilityUI.DEBUG_SIZE * EditorLevelVisibilityUI.DEBUG_SIZE)];
			for (int i = 0; i < EditorLevelVisibilityUI.regionLabels.Length; i++)
			{
				SleekLabel sleekLabel = new SleekLabel();
				sleekLabel.sizeOffset_X = 200;
				sleekLabel.sizeOffset_Y = 50;
				EditorLevelVisibilityUI.regionLabels[i] = sleekLabel;
				sleekLabel.foregroundTint = ESleekTint.NONE;
				EditorLevelVisibilityUI.container.add(sleekLabel);
			}
			EditorArea area = Editor.editor.area;
			Delegate onRegionUpdated = area.onRegionUpdated;
			if (EditorLevelVisibilityUI.<>f__mg$cache9 == null)
			{
				EditorLevelVisibilityUI.<>f__mg$cache9 = new EditorRegionUpdated(EditorLevelVisibilityUI.onRegionUpdated);
			}
			area.onRegionUpdated = (EditorRegionUpdated)Delegate.Combine(onRegionUpdated, EditorLevelVisibilityUI.<>f__mg$cache9);
		}

		public static void open()
		{
			if (EditorLevelVisibilityUI.active)
			{
				return;
			}
			EditorLevelVisibilityUI.active = true;
			EditorLevelVisibilityUI.update((int)Editor.editor.area.region_x, (int)Editor.editor.area.region_y);
			EditorUI.message(EEditorMessage.VISIBILITY);
			EditorLevelVisibilityUI.container.lerpPositionScale(0f, 0f, ESleekLerp.EXPONENTIAL, 20f);
		}

		public static void close()
		{
			if (!EditorLevelVisibilityUI.active)
			{
				return;
			}
			EditorLevelVisibilityUI.active = false;
			for (int i = 0; i < EditorLevelVisibilityUI.regionLabels.Length; i++)
			{
				SleekLabel sleekLabel = EditorLevelVisibilityUI.regionLabels[i];
				sleekLabel.isVisible = false;
			}
			EditorLevelVisibilityUI.container.lerpPositionScale(1f, 0f, ESleekLerp.EXPONENTIAL, 20f);
		}

		private static void onToggledRoadsToggle(SleekToggle toggle, bool state)
		{
			LevelVisibility.roadsVisible = state;
		}

		private static void onToggledNavigationToggle(SleekToggle toggle, bool state)
		{
			LevelVisibility.navigationVisible = state;
		}

		private static void onToggledNodesToggle(SleekToggle toggle, bool state)
		{
			LevelVisibility.nodesVisible = state;
		}

		private static void onToggledItemsToggle(SleekToggle toggle, bool state)
		{
			LevelVisibility.itemsVisible = state;
		}

		private static void onToggledPlayersToggle(SleekToggle toggle, bool state)
		{
			LevelVisibility.playersVisible = state;
		}

		private static void onToggledZombiesToggle(SleekToggle toggle, bool state)
		{
			LevelVisibility.zombiesVisible = state;
		}

		private static void onToggledVehiclesToggle(SleekToggle toggle, bool state)
		{
			LevelVisibility.vehiclesVisible = state;
		}

		private static void onToggledBorderToggle(SleekToggle toggle, bool state)
		{
			LevelVisibility.borderVisible = state;
		}

		private static void onToggledAnimalsToggle(SleekToggle toggle, bool state)
		{
			LevelVisibility.animalsVisible = state;
		}

		private static void onRegionUpdated(byte old_x, byte old_y, byte new_x, byte new_y)
		{
			if (!EditorLevelVisibilityUI.active)
			{
				return;
			}
			EditorLevelVisibilityUI.update((int)new_x, (int)new_y);
		}

		private static void update(int x, int y)
		{
			for (int i = 0; i < (int)EditorLevelVisibilityUI.DEBUG_SIZE; i++)
			{
				for (int j = 0; j < (int)EditorLevelVisibilityUI.DEBUG_SIZE; j++)
				{
					int num = i * (int)EditorLevelVisibilityUI.DEBUG_SIZE + j;
					int num2 = x - (int)(EditorLevelVisibilityUI.DEBUG_SIZE / 2) + i;
					int num3 = y - (int)(EditorLevelVisibilityUI.DEBUG_SIZE / 2) + j;
					SleekLabel sleekLabel = EditorLevelVisibilityUI.regionLabels[num];
					if (Regions.checkSafe(num2, num3))
					{
						int num4 = LevelObjects.objects[num2, num3].Count + LevelGround.trees[num2, num3].Count;
						int num5 = LevelObjects.total + LevelGround.total;
						double num6 = Math.Round((double)num4 / (double)num5 * 1000.0) / 10.0;
						int num7 = 0;
						for (int k = 0; k < LevelObjects.objects[num2, num3].Count; k++)
						{
							LevelObject levelObject = LevelObjects.objects[num2, num3][k];
							if (levelObject.transform)
							{
								levelObject.transform.GetComponents<MeshFilter>(EditorLevelVisibilityUI.meshes);
								if (EditorLevelVisibilityUI.meshes.Count == 0)
								{
									Transform transform = levelObject.transform.FindChild("Model_0");
									if (transform)
									{
										transform.GetComponentsInChildren<MeshFilter>(true, EditorLevelVisibilityUI.meshes);
									}
								}
								if (EditorLevelVisibilityUI.meshes.Count != 0)
								{
									for (int l = 0; l < EditorLevelVisibilityUI.meshes.Count; l++)
									{
										Mesh sharedMesh = EditorLevelVisibilityUI.meshes[l].sharedMesh;
										if (sharedMesh)
										{
											num7 += sharedMesh.triangles.Length;
										}
									}
								}
							}
						}
						for (int m = 0; m < LevelGround.trees[num2, num3].Count; m++)
						{
							ResourceSpawnpoint resourceSpawnpoint = LevelGround.trees[num2, num3][m];
							if (resourceSpawnpoint.model)
							{
								resourceSpawnpoint.model.GetComponents<MeshFilter>(EditorLevelVisibilityUI.meshes);
								if (EditorLevelVisibilityUI.meshes.Count == 0)
								{
									Transform transform2 = resourceSpawnpoint.model.FindChild("Model_0");
									if (transform2)
									{
										transform2.GetComponentsInChildren<MeshFilter>(true, EditorLevelVisibilityUI.meshes);
									}
								}
								if (EditorLevelVisibilityUI.meshes.Count != 0)
								{
									for (int n = 0; n < EditorLevelVisibilityUI.meshes.Count; n++)
									{
										Mesh sharedMesh2 = EditorLevelVisibilityUI.meshes[n].sharedMesh;
										if (sharedMesh2)
										{
											num7 += sharedMesh2.triangles.Length;
										}
									}
								}
							}
						}
						long num8 = (long)num4 * (long)num7;
						float quality = Mathf.Clamp01((float)(1.0 - (double)num8 / 50000000.0));
						sleekLabel.text = EditorLevelVisibilityUI.localization.format("Point", new object[]
						{
							num2,
							num3
						});
						SleekLabel sleekLabel2 = sleekLabel;
						sleekLabel2.text = sleekLabel2.text + "\n" + EditorLevelVisibilityUI.localization.format("Objects", new object[]
						{
							num4,
							num6
						});
						SleekLabel sleekLabel3 = sleekLabel;
						sleekLabel3.text = sleekLabel3.text + "\n" + EditorLevelVisibilityUI.localization.format("Triangles", new object[]
						{
							num7
						});
						if (num4 == 0 && num7 == 0)
						{
							sleekLabel.foregroundColor = Color.white;
						}
						else
						{
							sleekLabel.foregroundColor = ItemTool.getQualityColor(quality);
						}
					}
				}
			}
		}

		public static void update()
		{
			for (int i = 0; i < (int)EditorLevelVisibilityUI.DEBUG_SIZE; i++)
			{
				for (int j = 0; j < (int)EditorLevelVisibilityUI.DEBUG_SIZE; j++)
				{
					int num = i * (int)EditorLevelVisibilityUI.DEBUG_SIZE + j;
					int x = (int)(Editor.editor.area.region_x - EditorLevelVisibilityUI.DEBUG_SIZE / 2) + i;
					int y = (int)(Editor.editor.area.region_y - EditorLevelVisibilityUI.DEBUG_SIZE / 2) + j;
					SleekLabel sleekLabel = EditorLevelVisibilityUI.regionLabels[num];
					Vector3 vector;
					if (Regions.tryGetPoint(x, y, out vector))
					{
						vector = MainCamera.instance.WorldToScreenPoint(vector + new Vector3((float)(Regions.REGION_SIZE / 2), 0f, (float)(Regions.REGION_SIZE / 2)));
						if (vector.z > 0f)
						{
							sleekLabel.positionOffset_X = (int)(vector.x - 100f);
							sleekLabel.positionOffset_Y = (int)((float)Screen.height - vector.y - 25f);
							sleekLabel.isVisible = true;
						}
						else
						{
							sleekLabel.isVisible = false;
						}
					}
					else
					{
						sleekLabel.isVisible = false;
					}
				}
			}
		}

		private static readonly byte DEBUG_SIZE = 7;

		private static Local localization;

		private static Sleek container;

		public static bool active;

		private static List<MeshFilter> meshes = new List<MeshFilter>();

		public static SleekToggle roadsToggle;

		public static SleekToggle navigationToggle;

		public static SleekToggle nodesToggle;

		public static SleekToggle itemsToggle;

		public static SleekToggle playersToggle;

		public static SleekToggle zombiesToggle;

		public static SleekToggle vehiclesToggle;

		public static SleekToggle borderToggle;

		public static SleekToggle animalsToggle;

		private static SleekLabel[] regionLabels;

		[CompilerGenerated]
		private static Toggled <>f__mg$cache0;

		[CompilerGenerated]
		private static Toggled <>f__mg$cache1;

		[CompilerGenerated]
		private static Toggled <>f__mg$cache2;

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
		private static EditorRegionUpdated <>f__mg$cache9;
	}
}
