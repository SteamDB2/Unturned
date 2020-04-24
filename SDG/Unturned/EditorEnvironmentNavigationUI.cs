using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace SDG.Unturned
{
	public class EditorEnvironmentNavigationUI
	{
		public EditorEnvironmentNavigationUI()
		{
			Local local = Localization.read("/Editor/EditorEnvironmentNavigation.dat");
			Bundle bundle = Bundles.getBundle("/Bundles/Textures/Edit/Icons/EditorEnvironmentNavigation/EditorEnvironmentNavigation.unity3d");
			EditorEnvironmentNavigationUI.container = new Sleek();
			EditorEnvironmentNavigationUI.container.positionOffset_X = 10;
			EditorEnvironmentNavigationUI.container.positionOffset_Y = 10;
			EditorEnvironmentNavigationUI.container.positionScale_X = 1f;
			EditorEnvironmentNavigationUI.container.sizeOffset_X = -20;
			EditorEnvironmentNavigationUI.container.sizeOffset_Y = -20;
			EditorEnvironmentNavigationUI.container.sizeScale_X = 1f;
			EditorEnvironmentNavigationUI.container.sizeScale_Y = 1f;
			EditorUI.window.add(EditorEnvironmentNavigationUI.container);
			EditorEnvironmentNavigationUI.active = false;
			EditorEnvironmentNavigationUI.widthSlider = new SleekSlider();
			EditorEnvironmentNavigationUI.widthSlider.positionOffset_X = -200;
			EditorEnvironmentNavigationUI.widthSlider.positionOffset_Y = 80;
			EditorEnvironmentNavigationUI.widthSlider.positionScale_X = 1f;
			EditorEnvironmentNavigationUI.widthSlider.sizeOffset_X = 200;
			EditorEnvironmentNavigationUI.widthSlider.sizeOffset_Y = 20;
			EditorEnvironmentNavigationUI.widthSlider.orientation = ESleekOrientation.HORIZONTAL;
			EditorEnvironmentNavigationUI.widthSlider.addLabel(local.format("Width_Label"), ESleekSide.LEFT);
			SleekSlider sleekSlider = EditorEnvironmentNavigationUI.widthSlider;
			if (EditorEnvironmentNavigationUI.<>f__mg$cache0 == null)
			{
				EditorEnvironmentNavigationUI.<>f__mg$cache0 = new Dragged(EditorEnvironmentNavigationUI.onDraggedWidthSlider);
			}
			sleekSlider.onDragged = EditorEnvironmentNavigationUI.<>f__mg$cache0;
			EditorEnvironmentNavigationUI.container.add(EditorEnvironmentNavigationUI.widthSlider);
			EditorEnvironmentNavigationUI.widthSlider.isVisible = false;
			EditorEnvironmentNavigationUI.heightSlider = new SleekSlider();
			EditorEnvironmentNavigationUI.heightSlider.positionOffset_X = -200;
			EditorEnvironmentNavigationUI.heightSlider.positionOffset_Y = 110;
			EditorEnvironmentNavigationUI.heightSlider.positionScale_X = 1f;
			EditorEnvironmentNavigationUI.heightSlider.sizeOffset_X = 200;
			EditorEnvironmentNavigationUI.heightSlider.sizeOffset_Y = 20;
			EditorEnvironmentNavigationUI.heightSlider.orientation = ESleekOrientation.HORIZONTAL;
			EditorEnvironmentNavigationUI.heightSlider.addLabel(local.format("Height_Label"), ESleekSide.LEFT);
			SleekSlider sleekSlider2 = EditorEnvironmentNavigationUI.heightSlider;
			if (EditorEnvironmentNavigationUI.<>f__mg$cache1 == null)
			{
				EditorEnvironmentNavigationUI.<>f__mg$cache1 = new Dragged(EditorEnvironmentNavigationUI.onDraggedHeightSlider);
			}
			sleekSlider2.onDragged = EditorEnvironmentNavigationUI.<>f__mg$cache1;
			EditorEnvironmentNavigationUI.container.add(EditorEnvironmentNavigationUI.heightSlider);
			EditorEnvironmentNavigationUI.heightSlider.isVisible = false;
			EditorEnvironmentNavigationUI.navBox = new SleekBox();
			EditorEnvironmentNavigationUI.navBox.positionOffset_X = -200;
			EditorEnvironmentNavigationUI.navBox.positionOffset_Y = 140;
			EditorEnvironmentNavigationUI.navBox.positionScale_X = 1f;
			EditorEnvironmentNavigationUI.navBox.sizeOffset_X = 200;
			EditorEnvironmentNavigationUI.navBox.sizeOffset_Y = 30;
			EditorEnvironmentNavigationUI.navBox.addLabel(local.format("Nav_Label"), ESleekSide.LEFT);
			EditorEnvironmentNavigationUI.container.add(EditorEnvironmentNavigationUI.navBox);
			EditorEnvironmentNavigationUI.navBox.isVisible = false;
			EditorEnvironmentNavigationUI.difficultyGUIDField = new SleekField();
			EditorEnvironmentNavigationUI.difficultyGUIDField.positionOffset_X = -200;
			EditorEnvironmentNavigationUI.difficultyGUIDField.positionOffset_Y = 180;
			EditorEnvironmentNavigationUI.difficultyGUIDField.positionScale_X = 1f;
			EditorEnvironmentNavigationUI.difficultyGUIDField.sizeOffset_X = 200;
			EditorEnvironmentNavigationUI.difficultyGUIDField.sizeOffset_Y = 30;
			EditorEnvironmentNavigationUI.difficultyGUIDField.maxLength = 32;
			SleekField sleekField = EditorEnvironmentNavigationUI.difficultyGUIDField;
			if (EditorEnvironmentNavigationUI.<>f__mg$cache2 == null)
			{
				EditorEnvironmentNavigationUI.<>f__mg$cache2 = new Typed(EditorEnvironmentNavigationUI.onDifficultyGUIDFieldTyped);
			}
			sleekField.onTyped = EditorEnvironmentNavigationUI.<>f__mg$cache2;
			EditorEnvironmentNavigationUI.difficultyGUIDField.addLabel(local.format("Difficulty_GUID_Field_Label"), ESleekSide.LEFT);
			EditorEnvironmentNavigationUI.container.add(EditorEnvironmentNavigationUI.difficultyGUIDField);
			EditorEnvironmentNavigationUI.difficultyGUIDField.isVisible = false;
			EditorEnvironmentNavigationUI.maxZombiesField = new SleekByteField();
			EditorEnvironmentNavigationUI.maxZombiesField.positionOffset_X = -200;
			EditorEnvironmentNavigationUI.maxZombiesField.positionOffset_Y = 220;
			EditorEnvironmentNavigationUI.maxZombiesField.positionScale_X = 1f;
			EditorEnvironmentNavigationUI.maxZombiesField.sizeOffset_X = 200;
			EditorEnvironmentNavigationUI.maxZombiesField.sizeOffset_Y = 30;
			SleekByteField sleekByteField = EditorEnvironmentNavigationUI.maxZombiesField;
			if (EditorEnvironmentNavigationUI.<>f__mg$cache3 == null)
			{
				EditorEnvironmentNavigationUI.<>f__mg$cache3 = new TypedByte(EditorEnvironmentNavigationUI.onMaxZombiesFieldTyped);
			}
			sleekByteField.onTypedByte = EditorEnvironmentNavigationUI.<>f__mg$cache3;
			EditorEnvironmentNavigationUI.maxZombiesField.addLabel(local.format("Max_Zombies_Field_Label"), ESleekSide.LEFT);
			EditorEnvironmentNavigationUI.container.add(EditorEnvironmentNavigationUI.maxZombiesField);
			EditorEnvironmentNavigationUI.maxZombiesField.isVisible = false;
			EditorEnvironmentNavigationUI.spawnZombiesToggle = new SleekToggle();
			EditorEnvironmentNavigationUI.spawnZombiesToggle.positionOffset_X = -200;
			EditorEnvironmentNavigationUI.spawnZombiesToggle.positionOffset_Y = 260;
			EditorEnvironmentNavigationUI.spawnZombiesToggle.positionScale_X = 1f;
			EditorEnvironmentNavigationUI.spawnZombiesToggle.sizeOffset_X = 40;
			EditorEnvironmentNavigationUI.spawnZombiesToggle.sizeOffset_Y = 40;
			SleekToggle sleekToggle = EditorEnvironmentNavigationUI.spawnZombiesToggle;
			if (EditorEnvironmentNavigationUI.<>f__mg$cache4 == null)
			{
				EditorEnvironmentNavigationUI.<>f__mg$cache4 = new Toggled(EditorEnvironmentNavigationUI.onToggledSpawnZombiesToggle);
			}
			sleekToggle.onToggled = EditorEnvironmentNavigationUI.<>f__mg$cache4;
			EditorEnvironmentNavigationUI.spawnZombiesToggle.addLabel(local.format("Spawn_Zombies_Toggle_Label"), ESleekSide.RIGHT);
			EditorEnvironmentNavigationUI.container.add(EditorEnvironmentNavigationUI.spawnZombiesToggle);
			EditorEnvironmentNavigationUI.spawnZombiesToggle.isVisible = false;
			EditorEnvironmentNavigationUI.bakeNavigationButton = new SleekButtonIcon((Texture2D)bundle.load("Navigation"));
			EditorEnvironmentNavigationUI.bakeNavigationButton.positionOffset_X = -200;
			EditorEnvironmentNavigationUI.bakeNavigationButton.positionOffset_Y = -30;
			EditorEnvironmentNavigationUI.bakeNavigationButton.positionScale_X = 1f;
			EditorEnvironmentNavigationUI.bakeNavigationButton.positionScale_Y = 1f;
			EditorEnvironmentNavigationUI.bakeNavigationButton.sizeOffset_X = 200;
			EditorEnvironmentNavigationUI.bakeNavigationButton.sizeOffset_Y = 30;
			EditorEnvironmentNavigationUI.bakeNavigationButton.text = local.format("Bake_Navigation");
			EditorEnvironmentNavigationUI.bakeNavigationButton.tooltip = local.format("Bake_Navigation_Tooltip");
			SleekButton sleekButton = EditorEnvironmentNavigationUI.bakeNavigationButton;
			if (EditorEnvironmentNavigationUI.<>f__mg$cache5 == null)
			{
				EditorEnvironmentNavigationUI.<>f__mg$cache5 = new ClickedButton(EditorEnvironmentNavigationUI.onClickedBakeNavigationButton);
			}
			sleekButton.onClickedButton = EditorEnvironmentNavigationUI.<>f__mg$cache5;
			EditorEnvironmentNavigationUI.container.add(EditorEnvironmentNavigationUI.bakeNavigationButton);
			EditorEnvironmentNavigationUI.bakeNavigationButton.isVisible = false;
			bundle.unload();
		}

		public static void open()
		{
			if (EditorEnvironmentNavigationUI.active)
			{
				return;
			}
			EditorEnvironmentNavigationUI.active = true;
			EditorNavigation.isPathfinding = true;
			EditorUI.message(EEditorMessage.NAVIGATION);
			EditorEnvironmentNavigationUI.container.lerpPositionScale(0f, 0f, ESleekLerp.EXPONENTIAL, 20f);
		}

		public static void close()
		{
			if (!EditorEnvironmentNavigationUI.active)
			{
				return;
			}
			EditorEnvironmentNavigationUI.active = false;
			EditorNavigation.isPathfinding = false;
			EditorEnvironmentNavigationUI.container.lerpPositionScale(1f, 0f, ESleekLerp.EXPONENTIAL, 20f);
		}

		public static void updateSelection(Flag flag)
		{
			if (flag != null)
			{
				EditorEnvironmentNavigationUI.widthSlider.state = flag.width;
				EditorEnvironmentNavigationUI.heightSlider.state = flag.height;
				EditorEnvironmentNavigationUI.navBox.text = flag.graph.graphIndex.ToString();
				EditorEnvironmentNavigationUI.difficultyGUIDField.text = flag.data.difficultyGUID;
				EditorEnvironmentNavigationUI.maxZombiesField.state = flag.data.maxZombies;
				EditorEnvironmentNavigationUI.spawnZombiesToggle.state = flag.data.spawnZombies;
			}
			EditorEnvironmentNavigationUI.widthSlider.isVisible = (flag != null);
			EditorEnvironmentNavigationUI.heightSlider.isVisible = (flag != null);
			EditorEnvironmentNavigationUI.navBox.isVisible = (flag != null);
			EditorEnvironmentNavigationUI.difficultyGUIDField.isVisible = (flag != null);
			EditorEnvironmentNavigationUI.maxZombiesField.isVisible = (flag != null);
			EditorEnvironmentNavigationUI.spawnZombiesToggle.isVisible = (flag != null);
			EditorEnvironmentNavigationUI.bakeNavigationButton.isVisible = (flag != null);
		}

		private static void onDraggedWidthSlider(SleekSlider slider, float state)
		{
			if (EditorNavigation.flag != null)
			{
				EditorNavigation.flag.width = state;
				EditorNavigation.flag.buildMesh();
			}
		}

		private static void onDraggedHeightSlider(SleekSlider slider, float state)
		{
			if (EditorNavigation.flag != null)
			{
				EditorNavigation.flag.height = state;
				EditorNavigation.flag.buildMesh();
			}
		}

		private static void onDifficultyGUIDFieldTyped(SleekField field, string state)
		{
			if (EditorNavigation.flag != null)
			{
				EditorNavigation.flag.data.difficultyGUID = state;
			}
		}

		private static void onMaxZombiesFieldTyped(SleekByteField field, byte state)
		{
			if (EditorNavigation.flag != null)
			{
				EditorNavigation.flag.data.maxZombies = state;
			}
		}

		private static void onToggledSpawnZombiesToggle(SleekToggle toggle, bool state)
		{
			if (EditorNavigation.flag != null)
			{
				EditorNavigation.flag.data.spawnZombies = state;
			}
		}

		private static void onClickedBakeNavigationButton(SleekButton button)
		{
			if (EditorNavigation.flag != null)
			{
				EditorNavigation.flag.bakeNavigation();
			}
		}

		private static Sleek container;

		public static bool active;

		private static SleekSlider widthSlider;

		private static SleekSlider heightSlider;

		private static SleekBox navBox;

		private static SleekField difficultyGUIDField;

		private static SleekByteField maxZombiesField;

		private static SleekToggle spawnZombiesToggle;

		private static SleekButtonIcon bakeNavigationButton;

		[CompilerGenerated]
		private static Dragged <>f__mg$cache0;

		[CompilerGenerated]
		private static Dragged <>f__mg$cache1;

		[CompilerGenerated]
		private static Typed <>f__mg$cache2;

		[CompilerGenerated]
		private static TypedByte <>f__mg$cache3;

		[CompilerGenerated]
		private static Toggled <>f__mg$cache4;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache5;
	}
}
