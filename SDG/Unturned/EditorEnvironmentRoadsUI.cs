using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace SDG.Unturned
{
	public class EditorEnvironmentRoadsUI
	{
		public EditorEnvironmentRoadsUI()
		{
			Local local = Localization.read("/Editor/EditorEnvironmentRoads.dat");
			Bundle bundle = Bundles.getBundle("/Bundles/Textures/Edit/Icons/EditorEnvironmentRoads/EditorEnvironmentRoads.unity3d");
			EditorEnvironmentRoadsUI.container = new Sleek();
			EditorEnvironmentRoadsUI.container.positionOffset_X = 10;
			EditorEnvironmentRoadsUI.container.positionOffset_Y = 10;
			EditorEnvironmentRoadsUI.container.positionScale_X = 1f;
			EditorEnvironmentRoadsUI.container.sizeOffset_X = -20;
			EditorEnvironmentRoadsUI.container.sizeOffset_Y = -20;
			EditorEnvironmentRoadsUI.container.sizeScale_X = 1f;
			EditorEnvironmentRoadsUI.container.sizeScale_Y = 1f;
			EditorUI.window.add(EditorEnvironmentRoadsUI.container);
			EditorEnvironmentRoadsUI.active = false;
			EditorEnvironmentRoadsUI.roadScrollBox = new SleekScrollBox();
			EditorEnvironmentRoadsUI.roadScrollBox.positionOffset_X = -400;
			EditorEnvironmentRoadsUI.roadScrollBox.positionOffset_Y = 120;
			EditorEnvironmentRoadsUI.roadScrollBox.positionScale_X = 1f;
			EditorEnvironmentRoadsUI.roadScrollBox.sizeOffset_X = 400;
			EditorEnvironmentRoadsUI.roadScrollBox.sizeOffset_Y = -160;
			EditorEnvironmentRoadsUI.roadScrollBox.sizeScale_Y = 1f;
			EditorEnvironmentRoadsUI.roadScrollBox.area = new Rect(0f, 0f, 5f, (float)(LevelRoads.materials.Length * 70 + 160));
			EditorEnvironmentRoadsUI.container.add(EditorEnvironmentRoadsUI.roadScrollBox);
			for (int i = 0; i < LevelRoads.materials.Length; i++)
			{
				SleekImageTexture sleekImageTexture = new SleekImageTexture();
				sleekImageTexture.positionOffset_X = 200;
				sleekImageTexture.positionOffset_Y = i * 70;
				sleekImageTexture.sizeOffset_X = 64;
				sleekImageTexture.sizeOffset_Y = 64;
				sleekImageTexture.texture = LevelRoads.materials[i].material.mainTexture;
				EditorEnvironmentRoadsUI.roadScrollBox.add(sleekImageTexture);
				SleekButton sleekButton = new SleekButton();
				sleekButton.positionOffset_X = 70;
				sleekButton.sizeOffset_X = 100;
				sleekButton.sizeOffset_Y = 64;
				sleekButton.text = LevelRoads.materials[i].material.mainTexture.name;
				SleekButton sleekButton2 = sleekButton;
				if (EditorEnvironmentRoadsUI.<>f__mg$cache0 == null)
				{
					EditorEnvironmentRoadsUI.<>f__mg$cache0 = new ClickedButton(EditorEnvironmentRoadsUI.onClickedRoadButton);
				}
				sleekButton2.onClickedButton = EditorEnvironmentRoadsUI.<>f__mg$cache0;
				sleekImageTexture.add(sleekButton);
			}
			EditorEnvironmentRoadsUI.widthField = new SleekSingleField();
			EditorEnvironmentRoadsUI.widthField.positionOffset_X = 200;
			EditorEnvironmentRoadsUI.widthField.positionOffset_Y = LevelRoads.materials.Length * 70;
			EditorEnvironmentRoadsUI.widthField.sizeOffset_X = 170;
			EditorEnvironmentRoadsUI.widthField.sizeOffset_Y = 30;
			EditorEnvironmentRoadsUI.widthField.addLabel(local.format("WidthFieldLabelText"), ESleekSide.LEFT);
			SleekSingleField sleekSingleField = EditorEnvironmentRoadsUI.widthField;
			if (EditorEnvironmentRoadsUI.<>f__mg$cache1 == null)
			{
				EditorEnvironmentRoadsUI.<>f__mg$cache1 = new TypedSingle(EditorEnvironmentRoadsUI.onTypedWidthField);
			}
			sleekSingleField.onTypedSingle = EditorEnvironmentRoadsUI.<>f__mg$cache1;
			EditorEnvironmentRoadsUI.roadScrollBox.add(EditorEnvironmentRoadsUI.widthField);
			EditorEnvironmentRoadsUI.heightField = new SleekSingleField();
			EditorEnvironmentRoadsUI.heightField.positionOffset_X = 200;
			EditorEnvironmentRoadsUI.heightField.positionOffset_Y = LevelRoads.materials.Length * 70 + 40;
			EditorEnvironmentRoadsUI.heightField.sizeOffset_X = 170;
			EditorEnvironmentRoadsUI.heightField.sizeOffset_Y = 30;
			EditorEnvironmentRoadsUI.heightField.addLabel(local.format("HeightFieldLabelText"), ESleekSide.LEFT);
			SleekSingleField sleekSingleField2 = EditorEnvironmentRoadsUI.heightField;
			if (EditorEnvironmentRoadsUI.<>f__mg$cache2 == null)
			{
				EditorEnvironmentRoadsUI.<>f__mg$cache2 = new TypedSingle(EditorEnvironmentRoadsUI.onTypedHeightField);
			}
			sleekSingleField2.onTypedSingle = EditorEnvironmentRoadsUI.<>f__mg$cache2;
			EditorEnvironmentRoadsUI.roadScrollBox.add(EditorEnvironmentRoadsUI.heightField);
			EditorEnvironmentRoadsUI.depthField = new SleekSingleField();
			EditorEnvironmentRoadsUI.depthField.positionOffset_X = 200;
			EditorEnvironmentRoadsUI.depthField.positionOffset_Y = LevelRoads.materials.Length * 70 + 80;
			EditorEnvironmentRoadsUI.depthField.sizeOffset_X = 170;
			EditorEnvironmentRoadsUI.depthField.sizeOffset_Y = 30;
			EditorEnvironmentRoadsUI.depthField.addLabel(local.format("DepthFieldLabelText"), ESleekSide.LEFT);
			SleekSingleField sleekSingleField3 = EditorEnvironmentRoadsUI.depthField;
			if (EditorEnvironmentRoadsUI.<>f__mg$cache3 == null)
			{
				EditorEnvironmentRoadsUI.<>f__mg$cache3 = new TypedSingle(EditorEnvironmentRoadsUI.onTypedDepthField);
			}
			sleekSingleField3.onTypedSingle = EditorEnvironmentRoadsUI.<>f__mg$cache3;
			EditorEnvironmentRoadsUI.roadScrollBox.add(EditorEnvironmentRoadsUI.depthField);
			EditorEnvironmentRoadsUI.offset2Field = new SleekSingleField();
			EditorEnvironmentRoadsUI.offset2Field.positionOffset_X = 200;
			EditorEnvironmentRoadsUI.offset2Field.positionOffset_Y = LevelRoads.materials.Length * 70 + 120;
			EditorEnvironmentRoadsUI.offset2Field.sizeOffset_X = 170;
			EditorEnvironmentRoadsUI.offset2Field.sizeOffset_Y = 30;
			EditorEnvironmentRoadsUI.offset2Field.addLabel(local.format("OffsetFieldLabelText"), ESleekSide.LEFT);
			SleekSingleField sleekSingleField4 = EditorEnvironmentRoadsUI.offset2Field;
			if (EditorEnvironmentRoadsUI.<>f__mg$cache4 == null)
			{
				EditorEnvironmentRoadsUI.<>f__mg$cache4 = new TypedSingle(EditorEnvironmentRoadsUI.onTypedOffset2Field);
			}
			sleekSingleField4.onTypedSingle = EditorEnvironmentRoadsUI.<>f__mg$cache4;
			EditorEnvironmentRoadsUI.roadScrollBox.add(EditorEnvironmentRoadsUI.offset2Field);
			EditorEnvironmentRoadsUI.concreteToggle = new SleekToggle();
			EditorEnvironmentRoadsUI.concreteToggle.positionOffset_X = 200;
			EditorEnvironmentRoadsUI.concreteToggle.positionOffset_Y = LevelRoads.materials.Length * 70 + 160;
			EditorEnvironmentRoadsUI.concreteToggle.sizeOffset_X = 40;
			EditorEnvironmentRoadsUI.concreteToggle.sizeOffset_Y = 40;
			EditorEnvironmentRoadsUI.concreteToggle.addLabel(local.format("ConcreteToggleLabelText"), ESleekSide.RIGHT);
			SleekToggle sleekToggle = EditorEnvironmentRoadsUI.concreteToggle;
			if (EditorEnvironmentRoadsUI.<>f__mg$cache5 == null)
			{
				EditorEnvironmentRoadsUI.<>f__mg$cache5 = new Toggled(EditorEnvironmentRoadsUI.onToggledConcreteToggle);
			}
			sleekToggle.onToggled = EditorEnvironmentRoadsUI.<>f__mg$cache5;
			EditorEnvironmentRoadsUI.roadScrollBox.add(EditorEnvironmentRoadsUI.concreteToggle);
			EditorEnvironmentRoadsUI.selectedBox = new SleekBox();
			EditorEnvironmentRoadsUI.selectedBox.positionOffset_X = -200;
			EditorEnvironmentRoadsUI.selectedBox.positionOffset_Y = 80;
			EditorEnvironmentRoadsUI.selectedBox.positionScale_X = 1f;
			EditorEnvironmentRoadsUI.selectedBox.sizeOffset_X = 200;
			EditorEnvironmentRoadsUI.selectedBox.sizeOffset_Y = 30;
			EditorEnvironmentRoadsUI.selectedBox.addLabel(local.format("SelectionBoxLabelText"), ESleekSide.LEFT);
			EditorEnvironmentRoadsUI.container.add(EditorEnvironmentRoadsUI.selectedBox);
			EditorEnvironmentRoadsUI.updateSelection();
			EditorEnvironmentRoadsUI.bakeRoadsButton = new SleekButtonIcon((Texture2D)bundle.load("Roads"));
			EditorEnvironmentRoadsUI.bakeRoadsButton.positionOffset_X = -200;
			EditorEnvironmentRoadsUI.bakeRoadsButton.positionOffset_Y = -30;
			EditorEnvironmentRoadsUI.bakeRoadsButton.positionScale_X = 1f;
			EditorEnvironmentRoadsUI.bakeRoadsButton.positionScale_Y = 1f;
			EditorEnvironmentRoadsUI.bakeRoadsButton.sizeOffset_X = 200;
			EditorEnvironmentRoadsUI.bakeRoadsButton.sizeOffset_Y = 30;
			EditorEnvironmentRoadsUI.bakeRoadsButton.text = local.format("BakeRoadsButtonText");
			EditorEnvironmentRoadsUI.bakeRoadsButton.tooltip = local.format("BakeRoadsButtonTooltip");
			SleekButton sleekButton3 = EditorEnvironmentRoadsUI.bakeRoadsButton;
			if (EditorEnvironmentRoadsUI.<>f__mg$cache6 == null)
			{
				EditorEnvironmentRoadsUI.<>f__mg$cache6 = new ClickedButton(EditorEnvironmentRoadsUI.onClickedBakeRoadsButton);
			}
			sleekButton3.onClickedButton = EditorEnvironmentRoadsUI.<>f__mg$cache6;
			EditorEnvironmentRoadsUI.container.add(EditorEnvironmentRoadsUI.bakeRoadsButton);
			EditorEnvironmentRoadsUI.offsetField = new SleekSingleField();
			EditorEnvironmentRoadsUI.offsetField.positionOffset_Y = -170;
			EditorEnvironmentRoadsUI.offsetField.positionScale_Y = 1f;
			EditorEnvironmentRoadsUI.offsetField.sizeOffset_X = 200;
			EditorEnvironmentRoadsUI.offsetField.sizeOffset_Y = 30;
			EditorEnvironmentRoadsUI.offsetField.addLabel(local.format("OffsetFieldLabelText"), ESleekSide.RIGHT);
			SleekSingleField sleekSingleField5 = EditorEnvironmentRoadsUI.offsetField;
			if (EditorEnvironmentRoadsUI.<>f__mg$cache7 == null)
			{
				EditorEnvironmentRoadsUI.<>f__mg$cache7 = new TypedSingle(EditorEnvironmentRoadsUI.onTypedOffsetField);
			}
			sleekSingleField5.onTypedSingle = EditorEnvironmentRoadsUI.<>f__mg$cache7;
			EditorEnvironmentRoadsUI.container.add(EditorEnvironmentRoadsUI.offsetField);
			EditorEnvironmentRoadsUI.offsetField.isVisible = false;
			EditorEnvironmentRoadsUI.loopToggle = new SleekToggle();
			EditorEnvironmentRoadsUI.loopToggle.positionOffset_Y = -130;
			EditorEnvironmentRoadsUI.loopToggle.positionScale_Y = 1f;
			EditorEnvironmentRoadsUI.loopToggle.sizeOffset_X = 40;
			EditorEnvironmentRoadsUI.loopToggle.sizeOffset_Y = 40;
			EditorEnvironmentRoadsUI.loopToggle.addLabel(local.format("LoopToggleLabelText"), ESleekSide.RIGHT);
			SleekToggle sleekToggle2 = EditorEnvironmentRoadsUI.loopToggle;
			if (EditorEnvironmentRoadsUI.<>f__mg$cache8 == null)
			{
				EditorEnvironmentRoadsUI.<>f__mg$cache8 = new Toggled(EditorEnvironmentRoadsUI.onToggledLoopToggle);
			}
			sleekToggle2.onToggled = EditorEnvironmentRoadsUI.<>f__mg$cache8;
			EditorEnvironmentRoadsUI.container.add(EditorEnvironmentRoadsUI.loopToggle);
			EditorEnvironmentRoadsUI.loopToggle.isVisible = false;
			EditorEnvironmentRoadsUI.ignoreTerrainToggle = new SleekToggle();
			EditorEnvironmentRoadsUI.ignoreTerrainToggle.positionOffset_Y = -80;
			EditorEnvironmentRoadsUI.ignoreTerrainToggle.positionScale_Y = 1f;
			EditorEnvironmentRoadsUI.ignoreTerrainToggle.sizeOffset_X = 40;
			EditorEnvironmentRoadsUI.ignoreTerrainToggle.sizeOffset_Y = 40;
			EditorEnvironmentRoadsUI.ignoreTerrainToggle.addLabel(local.format("IgnoreTerrainToggleLabelText"), ESleekSide.RIGHT);
			SleekToggle sleekToggle3 = EditorEnvironmentRoadsUI.ignoreTerrainToggle;
			if (EditorEnvironmentRoadsUI.<>f__mg$cache9 == null)
			{
				EditorEnvironmentRoadsUI.<>f__mg$cache9 = new Toggled(EditorEnvironmentRoadsUI.onToggledIgnoreTerrainToggle);
			}
			sleekToggle3.onToggled = EditorEnvironmentRoadsUI.<>f__mg$cache9;
			EditorEnvironmentRoadsUI.container.add(EditorEnvironmentRoadsUI.ignoreTerrainToggle);
			EditorEnvironmentRoadsUI.ignoreTerrainToggle.isVisible = false;
			EditorEnvironmentRoadsUI.modeButton = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(local.format("Mirror")),
				new GUIContent(local.format("Aligned")),
				new GUIContent(local.format("Free"))
			});
			EditorEnvironmentRoadsUI.modeButton.positionOffset_Y = -30;
			EditorEnvironmentRoadsUI.modeButton.positionScale_Y = 1f;
			EditorEnvironmentRoadsUI.modeButton.sizeOffset_X = 200;
			EditorEnvironmentRoadsUI.modeButton.sizeOffset_Y = 30;
			EditorEnvironmentRoadsUI.modeButton.tooltip = local.format("ModeButtonTooltipText");
			SleekButtonState sleekButtonState = EditorEnvironmentRoadsUI.modeButton;
			if (EditorEnvironmentRoadsUI.<>f__mg$cacheA == null)
			{
				EditorEnvironmentRoadsUI.<>f__mg$cacheA = new SwappedState(EditorEnvironmentRoadsUI.onSwappedStateMode);
			}
			sleekButtonState.onSwappedState = EditorEnvironmentRoadsUI.<>f__mg$cacheA;
			EditorEnvironmentRoadsUI.container.add(EditorEnvironmentRoadsUI.modeButton);
			EditorEnvironmentRoadsUI.modeButton.isVisible = false;
			bundle.unload();
		}

		public static void open()
		{
			if (EditorEnvironmentRoadsUI.active)
			{
				return;
			}
			EditorEnvironmentRoadsUI.active = true;
			EditorRoads.isPaving = true;
			EditorUI.message(EEditorMessage.ROADS);
			EditorEnvironmentRoadsUI.container.lerpPositionScale(0f, 0f, ESleekLerp.EXPONENTIAL, 20f);
		}

		public static void close()
		{
			if (!EditorEnvironmentRoadsUI.active)
			{
				return;
			}
			EditorEnvironmentRoadsUI.active = false;
			EditorRoads.isPaving = false;
			EditorEnvironmentRoadsUI.container.lerpPositionScale(1f, 0f, ESleekLerp.EXPONENTIAL, 20f);
		}

		public static void updateSelection(Road road, RoadJoint joint)
		{
			if (road != null && joint != null)
			{
				EditorEnvironmentRoadsUI.offsetField.state = joint.offset;
				EditorEnvironmentRoadsUI.loopToggle.state = road.isLoop;
				EditorEnvironmentRoadsUI.ignoreTerrainToggle.state = joint.ignoreTerrain;
				EditorEnvironmentRoadsUI.modeButton.state = (int)joint.mode;
			}
			EditorEnvironmentRoadsUI.offsetField.isVisible = (road != null);
			EditorEnvironmentRoadsUI.loopToggle.isVisible = (road != null);
			EditorEnvironmentRoadsUI.ignoreTerrainToggle.isVisible = (road != null);
			EditorEnvironmentRoadsUI.modeButton.isVisible = (road != null);
		}

		private static void updateSelection()
		{
			if ((int)EditorRoads.selected < LevelRoads.materials.Length)
			{
				RoadMaterial roadMaterial = LevelRoads.materials[(int)EditorRoads.selected];
				EditorEnvironmentRoadsUI.selectedBox.text = roadMaterial.material.mainTexture.name;
				EditorEnvironmentRoadsUI.widthField.state = roadMaterial.width;
				EditorEnvironmentRoadsUI.heightField.state = roadMaterial.height;
				EditorEnvironmentRoadsUI.depthField.state = roadMaterial.depth;
				EditorEnvironmentRoadsUI.offset2Field.state = roadMaterial.offset;
				EditorEnvironmentRoadsUI.concreteToggle.state = roadMaterial.isConcrete;
			}
		}

		private static void onClickedRoadButton(SleekButton button)
		{
			EditorRoads.selected = (byte)(button.parent.positionOffset_Y / 70);
			if (EditorRoads.road != null)
			{
				EditorRoads.road.material = EditorRoads.selected;
			}
			EditorEnvironmentRoadsUI.updateSelection();
		}

		private static void onTypedWidthField(SleekSingleField field, float state)
		{
			LevelRoads.materials[(int)EditorRoads.selected].width = state;
		}

		private static void onTypedHeightField(SleekSingleField field, float state)
		{
			LevelRoads.materials[(int)EditorRoads.selected].height = state;
		}

		private static void onTypedDepthField(SleekSingleField field, float state)
		{
			LevelRoads.materials[(int)EditorRoads.selected].depth = state;
		}

		private static void onTypedOffset2Field(SleekSingleField field, float state)
		{
			LevelRoads.materials[(int)EditorRoads.selected].offset = state;
		}

		private static void onToggledConcreteToggle(SleekToggle toggle, bool state)
		{
			LevelRoads.materials[(int)EditorRoads.selected].isConcrete = state;
		}

		private static void onClickedBakeRoadsButton(SleekButton button)
		{
			LevelRoads.bakeRoads();
		}

		private static void onTypedOffsetField(SleekSingleField field, float state)
		{
			EditorRoads.joint.offset = state;
			EditorRoads.road.updatePoints();
		}

		private static void onToggledLoopToggle(SleekToggle toggle, bool state)
		{
			EditorRoads.road.isLoop = state;
		}

		private static void onToggledIgnoreTerrainToggle(SleekToggle toggle, bool state)
		{
			EditorRoads.joint.ignoreTerrain = state;
			EditorRoads.road.updatePoints();
		}

		private static void onSwappedStateMode(SleekButtonState button, int index)
		{
			EditorRoads.joint.mode = (ERoadMode)index;
		}

		private static Sleek container;

		public static bool active;

		private static SleekScrollBox roadScrollBox;

		private static SleekBox selectedBox;

		private static SleekSingleField widthField;

		private static SleekSingleField heightField;

		private static SleekSingleField depthField;

		private static SleekSingleField offset2Field;

		private static SleekToggle concreteToggle;

		private static SleekButtonIcon bakeRoadsButton;

		private static SleekSingleField offsetField;

		private static SleekToggle loopToggle;

		private static SleekToggle ignoreTerrainToggle;

		private static SleekButtonState modeButton;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache0;

		[CompilerGenerated]
		private static TypedSingle <>f__mg$cache1;

		[CompilerGenerated]
		private static TypedSingle <>f__mg$cache2;

		[CompilerGenerated]
		private static TypedSingle <>f__mg$cache3;

		[CompilerGenerated]
		private static TypedSingle <>f__mg$cache4;

		[CompilerGenerated]
		private static Toggled <>f__mg$cache5;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache6;

		[CompilerGenerated]
		private static TypedSingle <>f__mg$cache7;

		[CompilerGenerated]
		private static Toggled <>f__mg$cache8;

		[CompilerGenerated]
		private static Toggled <>f__mg$cache9;

		[CompilerGenerated]
		private static SwappedState <>f__mg$cacheA;
	}
}
