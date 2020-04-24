using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace SDG.Unturned
{
	public class EditorLevelPlayersUI
	{
		public EditorLevelPlayersUI()
		{
			Local local = Localization.read("/Editor/EditorLevelPlayers.dat");
			Bundle bundle = Bundles.getBundle("/Bundles/Textures/Edit/Icons/EditorLevelPlayers/EditorLevelPlayers.unity3d");
			EditorLevelPlayersUI.container = new Sleek();
			EditorLevelPlayersUI.container.positionOffset_X = 10;
			EditorLevelPlayersUI.container.positionOffset_Y = 10;
			EditorLevelPlayersUI.container.positionScale_X = 1f;
			EditorLevelPlayersUI.container.sizeOffset_X = -20;
			EditorLevelPlayersUI.container.sizeOffset_Y = -20;
			EditorLevelPlayersUI.container.sizeScale_X = 1f;
			EditorLevelPlayersUI.container.sizeScale_Y = 1f;
			EditorUI.window.add(EditorLevelPlayersUI.container);
			EditorLevelPlayersUI.active = false;
			EditorLevelPlayersUI.altToggle = new SleekToggle();
			EditorLevelPlayersUI.altToggle.positionOffset_Y = -180;
			EditorLevelPlayersUI.altToggle.positionScale_Y = 1f;
			EditorLevelPlayersUI.altToggle.sizeOffset_X = 40;
			EditorLevelPlayersUI.altToggle.sizeOffset_Y = 40;
			EditorLevelPlayersUI.altToggle.state = EditorSpawns.selectedAlt;
			EditorLevelPlayersUI.altToggle.addLabel(local.format("AltLabelText"), ESleekSide.RIGHT);
			SleekToggle sleekToggle = EditorLevelPlayersUI.altToggle;
			if (EditorLevelPlayersUI.<>f__mg$cache0 == null)
			{
				EditorLevelPlayersUI.<>f__mg$cache0 = new Toggled(EditorLevelPlayersUI.onToggledAltToggle);
			}
			sleekToggle.onToggled = EditorLevelPlayersUI.<>f__mg$cache0;
			EditorLevelPlayersUI.container.add(EditorLevelPlayersUI.altToggle);
			EditorLevelPlayersUI.radiusSlider = new SleekSlider();
			EditorLevelPlayersUI.radiusSlider.positionOffset_Y = -130;
			EditorLevelPlayersUI.radiusSlider.positionScale_Y = 1f;
			EditorLevelPlayersUI.radiusSlider.sizeOffset_X = 200;
			EditorLevelPlayersUI.radiusSlider.sizeOffset_Y = 20;
			EditorLevelPlayersUI.radiusSlider.state = (float)(EditorSpawns.radius - EditorSpawns.MIN_REMOVE_SIZE) / (float)EditorSpawns.MAX_REMOVE_SIZE;
			EditorLevelPlayersUI.radiusSlider.orientation = ESleekOrientation.HORIZONTAL;
			EditorLevelPlayersUI.radiusSlider.addLabel(local.format("RadiusSliderLabelText"), ESleekSide.RIGHT);
			SleekSlider sleekSlider = EditorLevelPlayersUI.radiusSlider;
			if (EditorLevelPlayersUI.<>f__mg$cache1 == null)
			{
				EditorLevelPlayersUI.<>f__mg$cache1 = new Dragged(EditorLevelPlayersUI.onDraggedRadiusSlider);
			}
			sleekSlider.onDragged = EditorLevelPlayersUI.<>f__mg$cache1;
			EditorLevelPlayersUI.container.add(EditorLevelPlayersUI.radiusSlider);
			EditorLevelPlayersUI.rotationSlider = new SleekSlider();
			EditorLevelPlayersUI.rotationSlider.positionOffset_Y = -100;
			EditorLevelPlayersUI.rotationSlider.positionScale_Y = 1f;
			EditorLevelPlayersUI.rotationSlider.sizeOffset_X = 200;
			EditorLevelPlayersUI.rotationSlider.sizeOffset_Y = 20;
			EditorLevelPlayersUI.rotationSlider.state = EditorSpawns.rotation / 360f;
			EditorLevelPlayersUI.rotationSlider.orientation = ESleekOrientation.HORIZONTAL;
			EditorLevelPlayersUI.rotationSlider.addLabel(local.format("RotationSliderLabelText"), ESleekSide.RIGHT);
			SleekSlider sleekSlider2 = EditorLevelPlayersUI.rotationSlider;
			if (EditorLevelPlayersUI.<>f__mg$cache2 == null)
			{
				EditorLevelPlayersUI.<>f__mg$cache2 = new Dragged(EditorLevelPlayersUI.onDraggedRotationSlider);
			}
			sleekSlider2.onDragged = EditorLevelPlayersUI.<>f__mg$cache2;
			EditorLevelPlayersUI.container.add(EditorLevelPlayersUI.rotationSlider);
			EditorLevelPlayersUI.addButton = new SleekButtonIcon((Texture2D)bundle.load("Add"));
			EditorLevelPlayersUI.addButton.positionOffset_Y = -70;
			EditorLevelPlayersUI.addButton.positionScale_Y = 1f;
			EditorLevelPlayersUI.addButton.sizeOffset_X = 200;
			EditorLevelPlayersUI.addButton.sizeOffset_Y = 30;
			EditorLevelPlayersUI.addButton.text = local.format("AddButtonText", new object[]
			{
				ControlsSettings.tool_0
			});
			EditorLevelPlayersUI.addButton.tooltip = local.format("AddButtonTooltip");
			SleekButton sleekButton = EditorLevelPlayersUI.addButton;
			if (EditorLevelPlayersUI.<>f__mg$cache3 == null)
			{
				EditorLevelPlayersUI.<>f__mg$cache3 = new ClickedButton(EditorLevelPlayersUI.onClickedAddButton);
			}
			sleekButton.onClickedButton = EditorLevelPlayersUI.<>f__mg$cache3;
			EditorLevelPlayersUI.container.add(EditorLevelPlayersUI.addButton);
			EditorLevelPlayersUI.removeButton = new SleekButtonIcon((Texture2D)bundle.load("Remove"));
			EditorLevelPlayersUI.removeButton.positionOffset_Y = -30;
			EditorLevelPlayersUI.removeButton.positionScale_Y = 1f;
			EditorLevelPlayersUI.removeButton.sizeOffset_X = 200;
			EditorLevelPlayersUI.removeButton.sizeOffset_Y = 30;
			EditorLevelPlayersUI.removeButton.text = local.format("RemoveButtonText", new object[]
			{
				ControlsSettings.tool_1
			});
			EditorLevelPlayersUI.removeButton.tooltip = local.format("RemoveButtonTooltip");
			SleekButton sleekButton2 = EditorLevelPlayersUI.removeButton;
			if (EditorLevelPlayersUI.<>f__mg$cache4 == null)
			{
				EditorLevelPlayersUI.<>f__mg$cache4 = new ClickedButton(EditorLevelPlayersUI.onClickedRemoveButton);
			}
			sleekButton2.onClickedButton = EditorLevelPlayersUI.<>f__mg$cache4;
			EditorLevelPlayersUI.container.add(EditorLevelPlayersUI.removeButton);
			bundle.unload();
		}

		public static void open()
		{
			if (EditorLevelPlayersUI.active)
			{
				return;
			}
			EditorLevelPlayersUI.active = true;
			EditorSpawns.isSpawning = true;
			EditorSpawns.spawnMode = ESpawnMode.ADD_PLAYER;
			EditorLevelPlayersUI.container.lerpPositionScale(0f, 0f, ESleekLerp.EXPONENTIAL, 20f);
		}

		public static void close()
		{
			if (!EditorLevelPlayersUI.active)
			{
				return;
			}
			EditorLevelPlayersUI.active = false;
			EditorSpawns.isSpawning = false;
			EditorLevelPlayersUI.container.lerpPositionScale(1f, 0f, ESleekLerp.EXPONENTIAL, 20f);
		}

		private static void onToggledAltToggle(SleekToggle toggle, bool state)
		{
			EditorSpawns.selectedAlt = state;
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
			EditorSpawns.spawnMode = ESpawnMode.ADD_PLAYER;
		}

		private static void onClickedRemoveButton(SleekButton button)
		{
			EditorSpawns.spawnMode = ESpawnMode.REMOVE_PLAYER;
		}

		private static Sleek container;

		public static bool active;

		private static SleekToggle altToggle;

		private static SleekSlider radiusSlider;

		private static SleekSlider rotationSlider;

		private static SleekButtonIcon addButton;

		private static SleekButtonIcon removeButton;

		[CompilerGenerated]
		private static Toggled <>f__mg$cache0;

		[CompilerGenerated]
		private static Dragged <>f__mg$cache1;

		[CompilerGenerated]
		private static Dragged <>f__mg$cache2;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache3;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache4;
	}
}
