using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace SDG.Unturned
{
	public class EditorTerrainHeightUI
	{
		public EditorTerrainHeightUI()
		{
			Local local = Localization.read("/Editor/EditorTerrainHeight.dat");
			Bundle bundle = Bundles.getBundle("/Bundles/Textures/Edit/Icons/EditorTerrainHeight/EditorTerrainHeight.unity3d");
			EditorTerrainHeightUI.container = new Sleek();
			EditorTerrainHeightUI.container.positionOffset_X = 10;
			EditorTerrainHeightUI.container.positionOffset_Y = 10;
			EditorTerrainHeightUI.container.positionScale_X = 1f;
			EditorTerrainHeightUI.container.sizeOffset_X = -20;
			EditorTerrainHeightUI.container.sizeOffset_Y = -20;
			EditorTerrainHeightUI.container.sizeScale_X = 1f;
			EditorTerrainHeightUI.container.sizeScale_Y = 1f;
			EditorUI.window.add(EditorTerrainHeightUI.container);
			EditorTerrainHeightUI.active = false;
			EditorTerrainHeightUI.adjustUpButton = new SleekButtonIcon((Texture2D)bundle.load("Adjust_Up"));
			EditorTerrainHeightUI.adjustUpButton.positionOffset_Y = -190;
			EditorTerrainHeightUI.adjustUpButton.positionScale_Y = 1f;
			EditorTerrainHeightUI.adjustUpButton.sizeOffset_X = 200;
			EditorTerrainHeightUI.adjustUpButton.sizeOffset_Y = 30;
			EditorTerrainHeightUI.adjustUpButton.text = local.format("AdjustUpButtonText", new object[]
			{
				ControlsSettings.tool_0
			});
			EditorTerrainHeightUI.adjustUpButton.tooltip = local.format("AdjustUpButtonTooltip");
			SleekButton sleekButton = EditorTerrainHeightUI.adjustUpButton;
			if (EditorTerrainHeightUI.<>f__mg$cache0 == null)
			{
				EditorTerrainHeightUI.<>f__mg$cache0 = new ClickedButton(EditorTerrainHeightUI.onClickedAdjustUpButton);
			}
			sleekButton.onClickedButton = EditorTerrainHeightUI.<>f__mg$cache0;
			EditorTerrainHeightUI.container.add(EditorTerrainHeightUI.adjustUpButton);
			EditorTerrainHeightUI.adjustDownButton = new SleekButtonIcon((Texture2D)bundle.load("Adjust_Down"));
			EditorTerrainHeightUI.adjustDownButton.positionOffset_Y = -150;
			EditorTerrainHeightUI.adjustDownButton.positionScale_Y = 1f;
			EditorTerrainHeightUI.adjustDownButton.sizeOffset_X = 200;
			EditorTerrainHeightUI.adjustDownButton.sizeOffset_Y = 30;
			EditorTerrainHeightUI.adjustDownButton.text = local.format("AdjustDownButtonText", new object[]
			{
				ControlsSettings.tool_0
			});
			EditorTerrainHeightUI.adjustDownButton.tooltip = local.format("AdjustDownButtonTooltip");
			SleekButton sleekButton2 = EditorTerrainHeightUI.adjustDownButton;
			if (EditorTerrainHeightUI.<>f__mg$cache1 == null)
			{
				EditorTerrainHeightUI.<>f__mg$cache1 = new ClickedButton(EditorTerrainHeightUI.onClickedAdjustDownButton);
			}
			sleekButton2.onClickedButton = EditorTerrainHeightUI.<>f__mg$cache1;
			EditorTerrainHeightUI.container.add(EditorTerrainHeightUI.adjustDownButton);
			EditorTerrainHeightUI.smoothButton = new SleekButtonIcon((Texture2D)bundle.load("Smooth"));
			EditorTerrainHeightUI.smoothButton.positionOffset_Y = -110;
			EditorTerrainHeightUI.smoothButton.positionScale_Y = 1f;
			EditorTerrainHeightUI.smoothButton.sizeOffset_X = 200;
			EditorTerrainHeightUI.smoothButton.sizeOffset_Y = 30;
			EditorTerrainHeightUI.smoothButton.text = local.format("SmoothButtonText", new object[]
			{
				ControlsSettings.tool_1
			});
			EditorTerrainHeightUI.smoothButton.tooltip = local.format("SmoothButtonTooltip");
			SleekButton sleekButton3 = EditorTerrainHeightUI.smoothButton;
			if (EditorTerrainHeightUI.<>f__mg$cache2 == null)
			{
				EditorTerrainHeightUI.<>f__mg$cache2 = new ClickedButton(EditorTerrainHeightUI.onClickedSmoothButton);
			}
			sleekButton3.onClickedButton = EditorTerrainHeightUI.<>f__mg$cache2;
			EditorTerrainHeightUI.container.add(EditorTerrainHeightUI.smoothButton);
			EditorTerrainHeightUI.flattenButton = new SleekButtonIcon((Texture2D)bundle.load("Flatten"));
			EditorTerrainHeightUI.flattenButton.positionOffset_Y = -70;
			EditorTerrainHeightUI.flattenButton.positionScale_Y = 1f;
			EditorTerrainHeightUI.flattenButton.sizeOffset_X = 200;
			EditorTerrainHeightUI.flattenButton.sizeOffset_Y = 30;
			EditorTerrainHeightUI.flattenButton.text = local.format("FlattenButtonText", new object[]
			{
				ControlsSettings.tool_2
			});
			EditorTerrainHeightUI.flattenButton.tooltip = local.format("FlattenButtonTooltip");
			SleekButton sleekButton4 = EditorTerrainHeightUI.flattenButton;
			if (EditorTerrainHeightUI.<>f__mg$cache3 == null)
			{
				EditorTerrainHeightUI.<>f__mg$cache3 = new ClickedButton(EditorTerrainHeightUI.onClickedFlattenButton);
			}
			sleekButton4.onClickedButton = EditorTerrainHeightUI.<>f__mg$cache3;
			EditorTerrainHeightUI.container.add(EditorTerrainHeightUI.flattenButton);
			EditorTerrainHeightUI.map2Button = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(local.format("Map2ButtonText1")),
				new GUIContent(local.format("Map2ButtonText2"))
			});
			EditorTerrainHeightUI.map2Button.positionOffset_Y = -30;
			EditorTerrainHeightUI.map2Button.positionScale_Y = 1f;
			EditorTerrainHeightUI.map2Button.sizeOffset_X = 200;
			EditorTerrainHeightUI.map2Button.sizeOffset_Y = 30;
			EditorTerrainHeightUI.map2Button.tooltip = local.format("Map2ButtonTooltip");
			SleekButtonState sleekButtonState = EditorTerrainHeightUI.map2Button;
			if (EditorTerrainHeightUI.<>f__mg$cache4 == null)
			{
				EditorTerrainHeightUI.<>f__mg$cache4 = new SwappedState(EditorTerrainHeightUI.onSwappedMap2);
			}
			sleekButtonState.onSwappedState = EditorTerrainHeightUI.<>f__mg$cache4;
			EditorTerrainHeightUI.container.add(EditorTerrainHeightUI.map2Button);
			EditorTerrainHeightUI.noiseSlider = new SleekSlider();
			EditorTerrainHeightUI.noiseSlider.positionOffset_Y = -320;
			EditorTerrainHeightUI.noiseSlider.positionScale_Y = 1f;
			EditorTerrainHeightUI.noiseSlider.sizeOffset_X = 200;
			EditorTerrainHeightUI.noiseSlider.sizeOffset_Y = 20;
			EditorTerrainHeightUI.noiseSlider.orientation = ESleekOrientation.HORIZONTAL;
			EditorTerrainHeightUI.noiseSlider.state = EditorTerrainHeight.brushNoise;
			EditorTerrainHeightUI.noiseSlider.addLabel(local.format("NoiseSliderLabelText"), ESleekSide.RIGHT);
			SleekSlider sleekSlider = EditorTerrainHeightUI.noiseSlider;
			if (EditorTerrainHeightUI.<>f__mg$cache5 == null)
			{
				EditorTerrainHeightUI.<>f__mg$cache5 = new Dragged(EditorTerrainHeightUI.onDraggedNoiseSlider);
			}
			sleekSlider.onDragged = EditorTerrainHeightUI.<>f__mg$cache5;
			EditorTerrainHeightUI.container.add(EditorTerrainHeightUI.noiseSlider);
			EditorTerrainHeightUI.sizeSlider = new SleekSlider();
			EditorTerrainHeightUI.sizeSlider.positionOffset_Y = -290;
			EditorTerrainHeightUI.sizeSlider.positionScale_Y = 1f;
			EditorTerrainHeightUI.sizeSlider.sizeOffset_X = 200;
			EditorTerrainHeightUI.sizeSlider.sizeOffset_Y = 20;
			EditorTerrainHeightUI.sizeSlider.orientation = ESleekOrientation.HORIZONTAL;
			EditorTerrainHeightUI.sizeSlider.state = (float)(EditorTerrainHeight.brushSize - EditorTerrainHeight.MIN_BRUSH_SIZE) / (float)EditorTerrainHeight.MAX_BRUSH_SIZE;
			EditorTerrainHeightUI.sizeSlider.addLabel(local.format("SizeSliderLabelText"), ESleekSide.RIGHT);
			SleekSlider sleekSlider2 = EditorTerrainHeightUI.sizeSlider;
			if (EditorTerrainHeightUI.<>f__mg$cache6 == null)
			{
				EditorTerrainHeightUI.<>f__mg$cache6 = new Dragged(EditorTerrainHeightUI.onDraggedSizeSlider);
			}
			sleekSlider2.onDragged = EditorTerrainHeightUI.<>f__mg$cache6;
			EditorTerrainHeightUI.container.add(EditorTerrainHeightUI.sizeSlider);
			EditorTerrainHeightUI.strengthSlider = new SleekSlider();
			EditorTerrainHeightUI.strengthSlider.positionOffset_Y = -260;
			EditorTerrainHeightUI.strengthSlider.positionScale_Y = 1f;
			EditorTerrainHeightUI.strengthSlider.sizeOffset_X = 200;
			EditorTerrainHeightUI.strengthSlider.sizeOffset_Y = 20;
			EditorTerrainHeightUI.strengthSlider.orientation = ESleekOrientation.HORIZONTAL;
			EditorTerrainHeightUI.strengthSlider.addLabel(local.format("StrengthSliderLabelText"), ESleekSide.RIGHT);
			EditorTerrainHeightUI.strengthSlider.state = EditorTerrainHeight.brushStrength;
			SleekSlider sleekSlider3 = EditorTerrainHeightUI.strengthSlider;
			if (EditorTerrainHeightUI.<>f__mg$cache7 == null)
			{
				EditorTerrainHeightUI.<>f__mg$cache7 = new Dragged(EditorTerrainHeightUI.onDraggedStrengthSlider);
			}
			sleekSlider3.onDragged = EditorTerrainHeightUI.<>f__mg$cache7;
			EditorTerrainHeightUI.container.add(EditorTerrainHeightUI.strengthSlider);
			EditorTerrainHeightUI.heightValue = new SleekValue();
			EditorTerrainHeightUI.heightValue.positionOffset_Y = -230;
			EditorTerrainHeightUI.heightValue.positionScale_Y = 1f;
			EditorTerrainHeightUI.heightValue.sizeOffset_X = 200;
			EditorTerrainHeightUI.heightValue.sizeOffset_Y = 30;
			EditorTerrainHeightUI.heightValue.addLabel(local.format("HeightValueLabelText"), ESleekSide.RIGHT);
			EditorTerrainHeightUI.heightValue.state = EditorTerrainHeight.brushHeight;
			SleekValue sleekValue = EditorTerrainHeightUI.heightValue;
			if (EditorTerrainHeightUI.<>f__mg$cache8 == null)
			{
				EditorTerrainHeightUI.<>f__mg$cache8 = new Valued(EditorTerrainHeightUI.onValuedHeightValue);
			}
			sleekValue.onValued = EditorTerrainHeightUI.<>f__mg$cache8;
			EditorTerrainHeightUI.container.add(EditorTerrainHeightUI.heightValue);
			bundle.unload();
		}

		public static void open()
		{
			if (EditorTerrainHeightUI.active)
			{
				return;
			}
			EditorTerrainHeightUI.active = true;
			if (LevelGround.materials == null)
			{
				return;
			}
			EditorTerrainHeight.isTerraforming = true;
			EditorUI.message(EEditorMessage.HEIGHTS);
			EditorTerrainHeightUI.container.lerpPositionScale(0f, 0f, ESleekLerp.EXPONENTIAL, 20f);
		}

		public static void close()
		{
			if (!EditorTerrainHeightUI.active)
			{
				return;
			}
			EditorTerrainHeightUI.active = false;
			if (LevelGround.materials == null)
			{
				return;
			}
			EditorTerrainHeight.isTerraforming = false;
			EditorTerrainHeightUI.container.lerpPositionScale(1f, 0f, ESleekLerp.EXPONENTIAL, 20f);
		}

		private static void onClickedAdjustUpButton(SleekButton button)
		{
			EditorTerrainHeight.brushMode = EPaintMode.ADJUST_UP;
		}

		private static void onClickedAdjustDownButton(SleekButton button)
		{
			EditorTerrainHeight.brushMode = EPaintMode.ADJUST_DOWN;
		}

		private static void onClickedSmoothButton(SleekButton button)
		{
			EditorTerrainHeight.brushMode = EPaintMode.SMOOTH;
		}

		private static void onClickedFlattenButton(SleekButton button)
		{
			EditorTerrainHeight.brushMode = EPaintMode.FLATTEN;
		}

		private static void onSwappedMap2(SleekButtonState button, int state)
		{
			EditorTerrainHeight.map2 = (state == 1);
		}

		private static void onDraggedSizeSlider(SleekSlider slider, float state)
		{
			EditorTerrainHeight.brushSize = (byte)((float)EditorTerrainHeight.MIN_BRUSH_SIZE + state * (float)EditorTerrainHeight.MAX_BRUSH_SIZE);
		}

		private static void onDraggedNoiseSlider(SleekSlider slider, float state)
		{
			EditorTerrainHeight.brushNoise = state;
		}

		private static void onDraggedStrengthSlider(SleekSlider slider, float state)
		{
			EditorTerrainHeight.brushStrength = state;
		}

		private static void onValuedHeightValue(SleekValue value, float state)
		{
			EditorTerrainHeight.brushHeight = state;
		}

		private static Sleek container;

		public static bool active;

		private static SleekButtonIcon adjustUpButton;

		private static SleekButtonIcon adjustDownButton;

		private static SleekButtonIcon smoothButton;

		private static SleekButtonIcon flattenButton;

		private static SleekButtonState map2Button;

		private static SleekSlider sizeSlider;

		private static SleekSlider noiseSlider;

		private static SleekSlider strengthSlider;

		public static SleekValue heightValue;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache0;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache1;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache2;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache3;

		[CompilerGenerated]
		private static SwappedState <>f__mg$cache4;

		[CompilerGenerated]
		private static Dragged <>f__mg$cache5;

		[CompilerGenerated]
		private static Dragged <>f__mg$cache6;

		[CompilerGenerated]
		private static Dragged <>f__mg$cache7;

		[CompilerGenerated]
		private static Valued <>f__mg$cache8;
	}
}
