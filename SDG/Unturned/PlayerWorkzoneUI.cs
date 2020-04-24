using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace SDG.Unturned
{
	public class PlayerWorkzoneUI
	{
		public PlayerWorkzoneUI()
		{
			Local local = Localization.read("/Editor/EditorLevelObjects.dat");
			Bundle bundle = Bundles.getBundle("/Bundles/Textures/Edit/Icons/EditorLevelObjects/EditorLevelObjects.unity3d");
			PlayerWorkzoneUI.container = new Sleek();
			PlayerWorkzoneUI.container.positionOffset_X = 10;
			PlayerWorkzoneUI.container.positionOffset_Y = 10;
			PlayerWorkzoneUI.container.positionScale_X = 1f;
			PlayerWorkzoneUI.container.sizeOffset_X = -20;
			PlayerWorkzoneUI.container.sizeOffset_Y = -20;
			PlayerWorkzoneUI.container.sizeScale_X = 1f;
			PlayerWorkzoneUI.container.sizeScale_Y = 1f;
			PlayerUI.window.add(PlayerWorkzoneUI.container);
			PlayerWorkzoneUI.active = false;
			PlayerWorkzone workzone = Player.player.workzone;
			if (PlayerWorkzoneUI.<>f__mg$cache0 == null)
			{
				PlayerWorkzoneUI.<>f__mg$cache0 = new DragStarted(PlayerWorkzoneUI.onDragStarted);
			}
			workzone.onDragStarted = PlayerWorkzoneUI.<>f__mg$cache0;
			PlayerWorkzone workzone2 = Player.player.workzone;
			if (PlayerWorkzoneUI.<>f__mg$cache1 == null)
			{
				PlayerWorkzoneUI.<>f__mg$cache1 = new DragStopped(PlayerWorkzoneUI.onDragStopped);
			}
			workzone2.onDragStopped = PlayerWorkzoneUI.<>f__mg$cache1;
			PlayerWorkzoneUI.dragBox = new SleekBox();
			PlayerUI.window.add(PlayerWorkzoneUI.dragBox);
			PlayerWorkzoneUI.dragBox.isVisible = false;
			PlayerWorkzoneUI.snapTransformField = new SleekSingleField();
			PlayerWorkzoneUI.snapTransformField.positionOffset_Y = -190;
			PlayerWorkzoneUI.snapTransformField.positionScale_Y = 1f;
			PlayerWorkzoneUI.snapTransformField.sizeOffset_X = 200;
			PlayerWorkzoneUI.snapTransformField.sizeOffset_Y = 30;
			PlayerWorkzoneUI.snapTransformField.text = (Mathf.Floor(Player.player.workzone.snapTransform * 100f) / 100f).ToString();
			PlayerWorkzoneUI.snapTransformField.addLabel(local.format("SnapTransformLabelText"), ESleekSide.RIGHT);
			SleekSingleField sleekSingleField = PlayerWorkzoneUI.snapTransformField;
			if (PlayerWorkzoneUI.<>f__mg$cache2 == null)
			{
				PlayerWorkzoneUI.<>f__mg$cache2 = new TypedSingle(PlayerWorkzoneUI.onTypedSnapTransformField);
			}
			sleekSingleField.onTypedSingle = PlayerWorkzoneUI.<>f__mg$cache2;
			PlayerWorkzoneUI.container.add(PlayerWorkzoneUI.snapTransformField);
			PlayerWorkzoneUI.snapRotationField = new SleekSingleField();
			PlayerWorkzoneUI.snapRotationField.positionOffset_Y = -150;
			PlayerWorkzoneUI.snapRotationField.positionScale_Y = 1f;
			PlayerWorkzoneUI.snapRotationField.sizeOffset_X = 200;
			PlayerWorkzoneUI.snapRotationField.sizeOffset_Y = 30;
			PlayerWorkzoneUI.snapRotationField.text = (Mathf.Floor(Player.player.workzone.snapRotation * 100f) / 100f).ToString();
			PlayerWorkzoneUI.snapRotationField.addLabel(local.format("SnapRotationLabelText"), ESleekSide.RIGHT);
			SleekSingleField sleekSingleField2 = PlayerWorkzoneUI.snapRotationField;
			if (PlayerWorkzoneUI.<>f__mg$cache3 == null)
			{
				PlayerWorkzoneUI.<>f__mg$cache3 = new TypedSingle(PlayerWorkzoneUI.onTypedSnapRotationField);
			}
			sleekSingleField2.onTypedSingle = PlayerWorkzoneUI.<>f__mg$cache3;
			PlayerWorkzoneUI.container.add(PlayerWorkzoneUI.snapRotationField);
			PlayerWorkzoneUI.transformButton = new SleekButtonIcon((Texture2D)bundle.load("Transform"));
			PlayerWorkzoneUI.transformButton.positionOffset_Y = -110;
			PlayerWorkzoneUI.transformButton.positionScale_Y = 1f;
			PlayerWorkzoneUI.transformButton.sizeOffset_X = 200;
			PlayerWorkzoneUI.transformButton.sizeOffset_Y = 30;
			PlayerWorkzoneUI.transformButton.text = local.format("TransformButtonText", new object[]
			{
				ControlsSettings.tool_0
			});
			PlayerWorkzoneUI.transformButton.tooltip = local.format("TransformButtonTooltip");
			SleekButton sleekButton = PlayerWorkzoneUI.transformButton;
			if (PlayerWorkzoneUI.<>f__mg$cache4 == null)
			{
				PlayerWorkzoneUI.<>f__mg$cache4 = new ClickedButton(PlayerWorkzoneUI.onClickedTransformButton);
			}
			sleekButton.onClickedButton = PlayerWorkzoneUI.<>f__mg$cache4;
			PlayerWorkzoneUI.container.add(PlayerWorkzoneUI.transformButton);
			PlayerWorkzoneUI.rotateButton = new SleekButtonIcon((Texture2D)bundle.load("Rotate"));
			PlayerWorkzoneUI.rotateButton.positionOffset_Y = -70;
			PlayerWorkzoneUI.rotateButton.positionScale_Y = 1f;
			PlayerWorkzoneUI.rotateButton.sizeOffset_X = 200;
			PlayerWorkzoneUI.rotateButton.sizeOffset_Y = 30;
			PlayerWorkzoneUI.rotateButton.text = local.format("RotateButtonText", new object[]
			{
				ControlsSettings.tool_1
			});
			PlayerWorkzoneUI.rotateButton.tooltip = local.format("RotateButtonTooltip");
			SleekButton sleekButton2 = PlayerWorkzoneUI.rotateButton;
			if (PlayerWorkzoneUI.<>f__mg$cache5 == null)
			{
				PlayerWorkzoneUI.<>f__mg$cache5 = new ClickedButton(PlayerWorkzoneUI.onClickedRotateButton);
			}
			sleekButton2.onClickedButton = PlayerWorkzoneUI.<>f__mg$cache5;
			PlayerWorkzoneUI.container.add(PlayerWorkzoneUI.rotateButton);
			PlayerWorkzoneUI.coordinateButton = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(local.format("CoordinateButtonTextGlobal"), (Texture)bundle.load("Global")),
				new GUIContent(local.format("CoordinateButtonTextLocal"), (Texture)bundle.load("Local"))
			});
			PlayerWorkzoneUI.coordinateButton.positionOffset_Y = -30;
			PlayerWorkzoneUI.coordinateButton.positionScale_Y = 1f;
			PlayerWorkzoneUI.coordinateButton.sizeOffset_X = 200;
			PlayerWorkzoneUI.coordinateButton.sizeOffset_Y = 30;
			PlayerWorkzoneUI.coordinateButton.tooltip = local.format("CoordinateButtonTooltip");
			SleekButtonState sleekButtonState = PlayerWorkzoneUI.coordinateButton;
			if (PlayerWorkzoneUI.<>f__mg$cache6 == null)
			{
				PlayerWorkzoneUI.<>f__mg$cache6 = new SwappedState(PlayerWorkzoneUI.onSwappedStateCoordinate);
			}
			sleekButtonState.onSwappedState = PlayerWorkzoneUI.<>f__mg$cache6;
			PlayerWorkzoneUI.container.add(PlayerWorkzoneUI.coordinateButton);
			bundle.unload();
		}

		public static void open()
		{
			if (PlayerWorkzoneUI.active)
			{
				return;
			}
			PlayerWorkzoneUI.active = true;
			Player.player.workzone.isBuilding = true;
			PlayerWorkzoneUI.container.lerpPositionScale(0f, 0f, ESleekLerp.EXPONENTIAL, 20f);
		}

		public static void close()
		{
			if (!PlayerWorkzoneUI.active)
			{
				return;
			}
			PlayerWorkzoneUI.active = false;
			Player.player.workzone.isBuilding = false;
			PlayerWorkzoneUI.container.lerpPositionScale(0f, 1f, ESleekLerp.EXPONENTIAL, 20f);
		}

		private static void onDragStarted(int min_x, int min_y, int max_x, int max_y)
		{
			PlayerWorkzoneUI.dragBox.positionOffset_X = min_x;
			PlayerWorkzoneUI.dragBox.positionOffset_Y = min_y;
			PlayerWorkzoneUI.dragBox.sizeOffset_X = max_x - min_x;
			PlayerWorkzoneUI.dragBox.sizeOffset_Y = max_y - min_y;
			PlayerWorkzoneUI.dragBox.isVisible = true;
		}

		private static void onDragStopped()
		{
			PlayerWorkzoneUI.dragBox.isVisible = false;
		}

		private static void onTypedSnapTransformField(SleekSingleField field, float value)
		{
			Player.player.workzone.snapTransform = value;
		}

		private static void onTypedSnapRotationField(SleekSingleField field, float value)
		{
			Player.player.workzone.snapRotation = value;
		}

		private static void onClickedTransformButton(SleekButton button)
		{
			Player.player.workzone.dragMode = EDragMode.TRANSFORM;
		}

		private static void onClickedRotateButton(SleekButton button)
		{
			Player.player.workzone.dragMode = EDragMode.ROTATE;
		}

		private static void onSwappedStateCoordinate(SleekButtonState button, int index)
		{
			Player.player.workzone.dragCoordinate = (EDragCoordinate)index;
		}

		private static Sleek container;

		public static bool active;

		private static SleekBox dragBox;

		private static SleekSingleField snapTransformField;

		private static SleekSingleField snapRotationField;

		private static SleekButtonIcon transformButton;

		private static SleekButtonIcon rotateButton;

		public static SleekButtonState coordinateButton;

		[CompilerGenerated]
		private static DragStarted <>f__mg$cache0;

		[CompilerGenerated]
		private static DragStopped <>f__mg$cache1;

		[CompilerGenerated]
		private static TypedSingle <>f__mg$cache2;

		[CompilerGenerated]
		private static TypedSingle <>f__mg$cache3;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache4;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache5;

		[CompilerGenerated]
		private static SwappedState <>f__mg$cache6;
	}
}
