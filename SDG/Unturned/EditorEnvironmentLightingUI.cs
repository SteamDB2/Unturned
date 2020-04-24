using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace SDG.Unturned
{
	public class EditorEnvironmentLightingUI
	{
		public EditorEnvironmentLightingUI()
		{
			Local local = Localization.read("/Editor/EditorEnvironmentLighting.dat");
			EditorEnvironmentLightingUI.container = new Sleek();
			EditorEnvironmentLightingUI.container.positionOffset_X = 10;
			EditorEnvironmentLightingUI.container.positionOffset_Y = 10;
			EditorEnvironmentLightingUI.container.positionScale_X = 1f;
			EditorEnvironmentLightingUI.container.sizeOffset_X = -20;
			EditorEnvironmentLightingUI.container.sizeOffset_Y = -20;
			EditorEnvironmentLightingUI.container.sizeScale_X = 1f;
			EditorEnvironmentLightingUI.container.sizeScale_Y = 1f;
			EditorUI.window.add(EditorEnvironmentLightingUI.container);
			EditorEnvironmentLightingUI.active = false;
			EditorEnvironmentLightingUI.selectedTime = ELightingTime.DAWN;
			EditorEnvironmentLightingUI.azimuthSlider = new SleekSlider();
			EditorEnvironmentLightingUI.azimuthSlider.positionOffset_X = -230;
			EditorEnvironmentLightingUI.azimuthSlider.positionOffset_Y = 80;
			EditorEnvironmentLightingUI.azimuthSlider.positionScale_X = 1f;
			EditorEnvironmentLightingUI.azimuthSlider.sizeOffset_X = 230;
			EditorEnvironmentLightingUI.azimuthSlider.sizeOffset_Y = 20;
			EditorEnvironmentLightingUI.azimuthSlider.state = LevelLighting.azimuth / 360f;
			EditorEnvironmentLightingUI.azimuthSlider.orientation = ESleekOrientation.HORIZONTAL;
			EditorEnvironmentLightingUI.azimuthSlider.addLabel(local.format("AzimuthSliderLabelText"), ESleekSide.LEFT);
			SleekSlider sleekSlider = EditorEnvironmentLightingUI.azimuthSlider;
			if (EditorEnvironmentLightingUI.<>f__mg$cache0 == null)
			{
				EditorEnvironmentLightingUI.<>f__mg$cache0 = new Dragged(EditorEnvironmentLightingUI.onDraggedAzimuthSlider);
			}
			sleekSlider.onDragged = EditorEnvironmentLightingUI.<>f__mg$cache0;
			EditorEnvironmentLightingUI.container.add(EditorEnvironmentLightingUI.azimuthSlider);
			EditorEnvironmentLightingUI.biasSlider = new SleekSlider();
			EditorEnvironmentLightingUI.biasSlider.positionOffset_X = -230;
			EditorEnvironmentLightingUI.biasSlider.positionOffset_Y = 110;
			EditorEnvironmentLightingUI.biasSlider.positionScale_X = 1f;
			EditorEnvironmentLightingUI.biasSlider.sizeOffset_X = 230;
			EditorEnvironmentLightingUI.biasSlider.sizeOffset_Y = 20;
			EditorEnvironmentLightingUI.biasSlider.state = LevelLighting.bias;
			EditorEnvironmentLightingUI.biasSlider.orientation = ESleekOrientation.HORIZONTAL;
			EditorEnvironmentLightingUI.biasSlider.addLabel(local.format("BiasSliderLabelText"), ESleekSide.LEFT);
			SleekSlider sleekSlider2 = EditorEnvironmentLightingUI.biasSlider;
			if (EditorEnvironmentLightingUI.<>f__mg$cache1 == null)
			{
				EditorEnvironmentLightingUI.<>f__mg$cache1 = new Dragged(EditorEnvironmentLightingUI.onDraggedBiasSlider);
			}
			sleekSlider2.onDragged = EditorEnvironmentLightingUI.<>f__mg$cache1;
			EditorEnvironmentLightingUI.container.add(EditorEnvironmentLightingUI.biasSlider);
			EditorEnvironmentLightingUI.fadeSlider = new SleekSlider();
			EditorEnvironmentLightingUI.fadeSlider.positionOffset_X = -230;
			EditorEnvironmentLightingUI.fadeSlider.positionOffset_Y = 140;
			EditorEnvironmentLightingUI.fadeSlider.positionScale_X = 1f;
			EditorEnvironmentLightingUI.fadeSlider.sizeOffset_X = 230;
			EditorEnvironmentLightingUI.fadeSlider.sizeOffset_Y = 20;
			EditorEnvironmentLightingUI.fadeSlider.state = LevelLighting.fade;
			EditorEnvironmentLightingUI.fadeSlider.orientation = ESleekOrientation.HORIZONTAL;
			EditorEnvironmentLightingUI.fadeSlider.addLabel(local.format("FadeSliderLabelText"), ESleekSide.LEFT);
			SleekSlider sleekSlider3 = EditorEnvironmentLightingUI.fadeSlider;
			if (EditorEnvironmentLightingUI.<>f__mg$cache2 == null)
			{
				EditorEnvironmentLightingUI.<>f__mg$cache2 = new Dragged(EditorEnvironmentLightingUI.onDraggedFadeSlider);
			}
			sleekSlider3.onDragged = EditorEnvironmentLightingUI.<>f__mg$cache2;
			EditorEnvironmentLightingUI.container.add(EditorEnvironmentLightingUI.fadeSlider);
			EditorEnvironmentLightingUI.lightingScrollBox = new SleekScrollBox();
			EditorEnvironmentLightingUI.lightingScrollBox.positionOffset_X = -470;
			EditorEnvironmentLightingUI.lightingScrollBox.positionOffset_Y = 170;
			EditorEnvironmentLightingUI.lightingScrollBox.positionScale_X = 1f;
			EditorEnvironmentLightingUI.lightingScrollBox.sizeOffset_X = 470;
			EditorEnvironmentLightingUI.lightingScrollBox.sizeOffset_Y = -170;
			EditorEnvironmentLightingUI.lightingScrollBox.sizeScale_Y = 1f;
			EditorEnvironmentLightingUI.container.add(EditorEnvironmentLightingUI.lightingScrollBox);
			EditorEnvironmentLightingUI.seaLevelSlider = new SleekValue();
			EditorEnvironmentLightingUI.seaLevelSlider.positionOffset_Y = -130;
			EditorEnvironmentLightingUI.seaLevelSlider.positionScale_Y = 1f;
			EditorEnvironmentLightingUI.seaLevelSlider.sizeOffset_X = 200;
			EditorEnvironmentLightingUI.seaLevelSlider.sizeOffset_Y = 30;
			EditorEnvironmentLightingUI.seaLevelSlider.state = LevelLighting.seaLevel;
			EditorEnvironmentLightingUI.seaLevelSlider.addLabel(local.format("Sea_Level_Slider_Label"), ESleekSide.RIGHT);
			SleekValue sleekValue = EditorEnvironmentLightingUI.seaLevelSlider;
			if (EditorEnvironmentLightingUI.<>f__mg$cache3 == null)
			{
				EditorEnvironmentLightingUI.<>f__mg$cache3 = new Valued(EditorEnvironmentLightingUI.onValuedSeaLevelSlider);
			}
			sleekValue.onValued = EditorEnvironmentLightingUI.<>f__mg$cache3;
			EditorEnvironmentLightingUI.container.add(EditorEnvironmentLightingUI.seaLevelSlider);
			EditorEnvironmentLightingUI.snowLevelSlider = new SleekValue();
			EditorEnvironmentLightingUI.snowLevelSlider.positionOffset_Y = -90;
			EditorEnvironmentLightingUI.snowLevelSlider.positionScale_Y = 1f;
			EditorEnvironmentLightingUI.snowLevelSlider.sizeOffset_X = 200;
			EditorEnvironmentLightingUI.snowLevelSlider.sizeOffset_Y = 30;
			EditorEnvironmentLightingUI.snowLevelSlider.state = LevelLighting.snowLevel;
			EditorEnvironmentLightingUI.snowLevelSlider.addLabel(local.format("Snow_Level_Slider_Label"), ESleekSide.RIGHT);
			SleekValue sleekValue2 = EditorEnvironmentLightingUI.snowLevelSlider;
			if (EditorEnvironmentLightingUI.<>f__mg$cache4 == null)
			{
				EditorEnvironmentLightingUI.<>f__mg$cache4 = new Valued(EditorEnvironmentLightingUI.onValuedSnowLevelSlider);
			}
			sleekValue2.onValued = EditorEnvironmentLightingUI.<>f__mg$cache4;
			EditorEnvironmentLightingUI.container.add(EditorEnvironmentLightingUI.snowLevelSlider);
			EditorEnvironmentLightingUI.rainFreqField = new SleekSingleField();
			EditorEnvironmentLightingUI.rainFreqField.positionOffset_Y = -370;
			EditorEnvironmentLightingUI.rainFreqField.positionScale_Y = 1f;
			EditorEnvironmentLightingUI.rainFreqField.sizeOffset_X = 100;
			EditorEnvironmentLightingUI.rainFreqField.sizeOffset_Y = 30;
			EditorEnvironmentLightingUI.rainFreqField.state = LevelLighting.rainFreq;
			EditorEnvironmentLightingUI.rainFreqField.addLabel(local.format("Rain_Freq_Label"), ESleekSide.RIGHT);
			SleekSingleField sleekSingleField = EditorEnvironmentLightingUI.rainFreqField;
			if (EditorEnvironmentLightingUI.<>f__mg$cache5 == null)
			{
				EditorEnvironmentLightingUI.<>f__mg$cache5 = new TypedSingle(EditorEnvironmentLightingUI.onTypedRainFreqField);
			}
			sleekSingleField.onTypedSingle = EditorEnvironmentLightingUI.<>f__mg$cache5;
			EditorEnvironmentLightingUI.container.add(EditorEnvironmentLightingUI.rainFreqField);
			EditorEnvironmentLightingUI.rainDurField = new SleekSingleField();
			EditorEnvironmentLightingUI.rainDurField.positionOffset_Y = -330;
			EditorEnvironmentLightingUI.rainDurField.positionScale_Y = 1f;
			EditorEnvironmentLightingUI.rainDurField.sizeOffset_X = 100;
			EditorEnvironmentLightingUI.rainDurField.sizeOffset_Y = 30;
			EditorEnvironmentLightingUI.rainDurField.state = LevelLighting.rainDur;
			EditorEnvironmentLightingUI.rainDurField.addLabel(local.format("Rain_Dur_Label"), ESleekSide.RIGHT);
			SleekSingleField sleekSingleField2 = EditorEnvironmentLightingUI.rainDurField;
			if (EditorEnvironmentLightingUI.<>f__mg$cache6 == null)
			{
				EditorEnvironmentLightingUI.<>f__mg$cache6 = new TypedSingle(EditorEnvironmentLightingUI.onTypedRainDurField);
			}
			sleekSingleField2.onTypedSingle = EditorEnvironmentLightingUI.<>f__mg$cache6;
			EditorEnvironmentLightingUI.container.add(EditorEnvironmentLightingUI.rainDurField);
			EditorEnvironmentLightingUI.snowFreqField = new SleekSingleField();
			EditorEnvironmentLightingUI.snowFreqField.positionOffset_Y = -290;
			EditorEnvironmentLightingUI.snowFreqField.positionScale_Y = 1f;
			EditorEnvironmentLightingUI.snowFreqField.sizeOffset_X = 100;
			EditorEnvironmentLightingUI.snowFreqField.sizeOffset_Y = 30;
			EditorEnvironmentLightingUI.snowFreqField.state = LevelLighting.snowFreq;
			EditorEnvironmentLightingUI.snowFreqField.addLabel(local.format("Snow_Freq_Label"), ESleekSide.RIGHT);
			SleekSingleField sleekSingleField3 = EditorEnvironmentLightingUI.snowFreqField;
			if (EditorEnvironmentLightingUI.<>f__mg$cache7 == null)
			{
				EditorEnvironmentLightingUI.<>f__mg$cache7 = new TypedSingle(EditorEnvironmentLightingUI.onTypedSnowFreqField);
			}
			sleekSingleField3.onTypedSingle = EditorEnvironmentLightingUI.<>f__mg$cache7;
			EditorEnvironmentLightingUI.container.add(EditorEnvironmentLightingUI.snowFreqField);
			EditorEnvironmentLightingUI.snowDurField = new SleekSingleField();
			EditorEnvironmentLightingUI.snowDurField.positionOffset_Y = -250;
			EditorEnvironmentLightingUI.snowDurField.positionScale_Y = 1f;
			EditorEnvironmentLightingUI.snowDurField.sizeOffset_X = 100;
			EditorEnvironmentLightingUI.snowDurField.sizeOffset_Y = 30;
			EditorEnvironmentLightingUI.snowDurField.state = LevelLighting.snowDur;
			EditorEnvironmentLightingUI.snowDurField.addLabel(local.format("Snow_Dur_Label"), ESleekSide.RIGHT);
			SleekSingleField sleekSingleField4 = EditorEnvironmentLightingUI.snowDurField;
			if (EditorEnvironmentLightingUI.<>f__mg$cache8 == null)
			{
				EditorEnvironmentLightingUI.<>f__mg$cache8 = new TypedSingle(EditorEnvironmentLightingUI.onTypedSnowDurField);
			}
			sleekSingleField4.onTypedSingle = EditorEnvironmentLightingUI.<>f__mg$cache8;
			EditorEnvironmentLightingUI.container.add(EditorEnvironmentLightingUI.snowDurField);
			EditorEnvironmentLightingUI.stormButton = new SleekButton();
			EditorEnvironmentLightingUI.stormButton.positionOffset_Y = -210;
			EditorEnvironmentLightingUI.stormButton.positionScale_Y = 1f;
			EditorEnvironmentLightingUI.stormButton.sizeOffset_X = 100;
			EditorEnvironmentLightingUI.stormButton.sizeOffset_Y = 30;
			EditorEnvironmentLightingUI.stormButton.text = local.format("Storm");
			EditorEnvironmentLightingUI.stormButton.tooltip = local.format("Storm_Tooltip");
			SleekButton sleekButton = EditorEnvironmentLightingUI.stormButton;
			if (EditorEnvironmentLightingUI.<>f__mg$cache9 == null)
			{
				EditorEnvironmentLightingUI.<>f__mg$cache9 = new ClickedButton(EditorEnvironmentLightingUI.onClickedStormButton);
			}
			sleekButton.onClickedButton = EditorEnvironmentLightingUI.<>f__mg$cache9;
			EditorEnvironmentLightingUI.container.add(EditorEnvironmentLightingUI.stormButton);
			EditorEnvironmentLightingUI.rainToggle = new SleekToggle();
			EditorEnvironmentLightingUI.rainToggle.positionOffset_X = 110;
			EditorEnvironmentLightingUI.rainToggle.positionOffset_Y = -215;
			EditorEnvironmentLightingUI.rainToggle.positionScale_Y = 1f;
			EditorEnvironmentLightingUI.rainToggle.sizeOffset_X = 40;
			EditorEnvironmentLightingUI.rainToggle.sizeOffset_Y = 40;
			EditorEnvironmentLightingUI.rainToggle.state = LevelLighting.canRain;
			EditorEnvironmentLightingUI.rainToggle.addLabel(local.format("Rain_Toggle_Label"), ESleekSide.RIGHT);
			SleekToggle sleekToggle = EditorEnvironmentLightingUI.rainToggle;
			if (EditorEnvironmentLightingUI.<>f__mg$cacheA == null)
			{
				EditorEnvironmentLightingUI.<>f__mg$cacheA = new Toggled(EditorEnvironmentLightingUI.onToggledRainToggle);
			}
			sleekToggle.onToggled = EditorEnvironmentLightingUI.<>f__mg$cacheA;
			EditorEnvironmentLightingUI.container.add(EditorEnvironmentLightingUI.rainToggle);
			EditorEnvironmentLightingUI.blizzardButton = new SleekButton();
			EditorEnvironmentLightingUI.blizzardButton.positionOffset_Y = -170;
			EditorEnvironmentLightingUI.blizzardButton.positionScale_Y = 1f;
			EditorEnvironmentLightingUI.blizzardButton.sizeOffset_X = 100;
			EditorEnvironmentLightingUI.blizzardButton.sizeOffset_Y = 30;
			EditorEnvironmentLightingUI.blizzardButton.text = local.format("Blizzard");
			EditorEnvironmentLightingUI.blizzardButton.tooltip = local.format("Blizzard_Tooltip");
			SleekButton sleekButton2 = EditorEnvironmentLightingUI.blizzardButton;
			if (EditorEnvironmentLightingUI.<>f__mg$cacheB == null)
			{
				EditorEnvironmentLightingUI.<>f__mg$cacheB = new ClickedButton(EditorEnvironmentLightingUI.onClickedBlizzardButton);
			}
			sleekButton2.onClickedButton = EditorEnvironmentLightingUI.<>f__mg$cacheB;
			EditorEnvironmentLightingUI.container.add(EditorEnvironmentLightingUI.blizzardButton);
			EditorEnvironmentLightingUI.snowToggle = new SleekToggle();
			EditorEnvironmentLightingUI.snowToggle.positionOffset_X = 110;
			EditorEnvironmentLightingUI.snowToggle.positionOffset_Y = -175;
			EditorEnvironmentLightingUI.snowToggle.positionScale_Y = 1f;
			EditorEnvironmentLightingUI.snowToggle.sizeOffset_X = 40;
			EditorEnvironmentLightingUI.snowToggle.sizeOffset_Y = 40;
			EditorEnvironmentLightingUI.snowToggle.state = LevelLighting.canSnow;
			EditorEnvironmentLightingUI.snowToggle.addLabel(local.format("Snow_Toggle_Label"), ESleekSide.RIGHT);
			SleekToggle sleekToggle2 = EditorEnvironmentLightingUI.snowToggle;
			if (EditorEnvironmentLightingUI.<>f__mg$cacheC == null)
			{
				EditorEnvironmentLightingUI.<>f__mg$cacheC = new Toggled(EditorEnvironmentLightingUI.onToggledSnowToggle);
			}
			sleekToggle2.onToggled = EditorEnvironmentLightingUI.<>f__mg$cacheC;
			EditorEnvironmentLightingUI.container.add(EditorEnvironmentLightingUI.snowToggle);
			EditorEnvironmentLightingUI.moonSlider = new SleekSlider();
			EditorEnvironmentLightingUI.moonSlider.positionOffset_Y = -50;
			EditorEnvironmentLightingUI.moonSlider.positionScale_Y = 1f;
			EditorEnvironmentLightingUI.moonSlider.sizeOffset_X = 200;
			EditorEnvironmentLightingUI.moonSlider.sizeOffset_Y = 20;
			EditorEnvironmentLightingUI.moonSlider.state = (float)LevelLighting.moon / (float)LevelLighting.MOON_CYCLES;
			EditorEnvironmentLightingUI.moonSlider.orientation = ESleekOrientation.HORIZONTAL;
			EditorEnvironmentLightingUI.moonSlider.addLabel(local.format("MoonSliderLabelText"), ESleekSide.RIGHT);
			SleekSlider sleekSlider4 = EditorEnvironmentLightingUI.moonSlider;
			if (EditorEnvironmentLightingUI.<>f__mg$cacheD == null)
			{
				EditorEnvironmentLightingUI.<>f__mg$cacheD = new Dragged(EditorEnvironmentLightingUI.onDraggedMoonSlider);
			}
			sleekSlider4.onDragged = EditorEnvironmentLightingUI.<>f__mg$cacheD;
			EditorEnvironmentLightingUI.container.add(EditorEnvironmentLightingUI.moonSlider);
			EditorEnvironmentLightingUI.timeSlider = new SleekSlider();
			EditorEnvironmentLightingUI.timeSlider.positionOffset_Y = -20;
			EditorEnvironmentLightingUI.timeSlider.positionScale_Y = 1f;
			EditorEnvironmentLightingUI.timeSlider.sizeOffset_X = 200;
			EditorEnvironmentLightingUI.timeSlider.sizeOffset_Y = 20;
			EditorEnvironmentLightingUI.timeSlider.state = LevelLighting.time;
			EditorEnvironmentLightingUI.timeSlider.orientation = ESleekOrientation.HORIZONTAL;
			EditorEnvironmentLightingUI.timeSlider.addLabel(local.format("TimeSliderLabelText"), ESleekSide.RIGHT);
			SleekSlider sleekSlider5 = EditorEnvironmentLightingUI.timeSlider;
			if (EditorEnvironmentLightingUI.<>f__mg$cacheE == null)
			{
				EditorEnvironmentLightingUI.<>f__mg$cacheE = new Dragged(EditorEnvironmentLightingUI.onDraggedTimeSlider);
			}
			sleekSlider5.onDragged = EditorEnvironmentLightingUI.<>f__mg$cacheE;
			EditorEnvironmentLightingUI.container.add(EditorEnvironmentLightingUI.timeSlider);
			EditorEnvironmentLightingUI.timeButtons = new SleekButton[4];
			for (int i = 0; i < EditorEnvironmentLightingUI.timeButtons.Length; i++)
			{
				SleekButton sleekButton3 = new SleekButton();
				sleekButton3.positionOffset_X = 240;
				sleekButton3.positionOffset_Y = i * 40;
				sleekButton3.sizeOffset_X = 200;
				sleekButton3.sizeOffset_Y = 30;
				sleekButton3.text = local.format("Time_" + i);
				SleekButton sleekButton4 = sleekButton3;
				Delegate onClickedButton = sleekButton4.onClickedButton;
				if (EditorEnvironmentLightingUI.<>f__mg$cacheF == null)
				{
					EditorEnvironmentLightingUI.<>f__mg$cacheF = new ClickedButton(EditorEnvironmentLightingUI.onClickedTimeButton);
				}
				sleekButton4.onClickedButton = (ClickedButton)Delegate.Combine(onClickedButton, EditorEnvironmentLightingUI.<>f__mg$cacheF);
				EditorEnvironmentLightingUI.lightingScrollBox.add(sleekButton3);
				EditorEnvironmentLightingUI.timeButtons[i] = sleekButton3;
			}
			EditorEnvironmentLightingUI.infoBoxes = new SleekBox[12];
			EditorEnvironmentLightingUI.colorPickers = new SleekColorPicker[EditorEnvironmentLightingUI.infoBoxes.Length];
			EditorEnvironmentLightingUI.singleSliders = new SleekSlider[5];
			for (int j = 0; j < EditorEnvironmentLightingUI.colorPickers.Length; j++)
			{
				SleekBox sleekBox = new SleekBox();
				sleekBox.positionOffset_X = 240;
				sleekBox.positionOffset_Y = EditorEnvironmentLightingUI.timeButtons.Length * 40 + j * 170;
				sleekBox.sizeOffset_X = 200;
				sleekBox.sizeOffset_Y = 30;
				sleekBox.text = local.format("Color_" + j);
				EditorEnvironmentLightingUI.lightingScrollBox.add(sleekBox);
				EditorEnvironmentLightingUI.infoBoxes[j] = sleekBox;
				SleekColorPicker sleekColorPicker = new SleekColorPicker();
				sleekColorPicker.positionOffset_X = 200;
				sleekColorPicker.positionOffset_Y = EditorEnvironmentLightingUI.timeButtons.Length * 40 + j * 170 + 40;
				SleekColorPicker sleekColorPicker2 = sleekColorPicker;
				Delegate onColorPicked = sleekColorPicker2.onColorPicked;
				if (EditorEnvironmentLightingUI.<>f__mg$cache10 == null)
				{
					EditorEnvironmentLightingUI.<>f__mg$cache10 = new ColorPicked(EditorEnvironmentLightingUI.onPickedColorPicker);
				}
				sleekColorPicker2.onColorPicked = (ColorPicked)Delegate.Combine(onColorPicked, EditorEnvironmentLightingUI.<>f__mg$cache10);
				EditorEnvironmentLightingUI.lightingScrollBox.add(sleekColorPicker);
				EditorEnvironmentLightingUI.colorPickers[j] = sleekColorPicker;
			}
			for (int k = 0; k < EditorEnvironmentLightingUI.singleSliders.Length; k++)
			{
				SleekSlider sleekSlider6 = new SleekSlider();
				sleekSlider6.positionOffset_X = 240;
				sleekSlider6.positionOffset_Y = EditorEnvironmentLightingUI.timeButtons.Length * 40 + EditorEnvironmentLightingUI.colorPickers.Length * 170 + k * 30;
				sleekSlider6.sizeOffset_X = 200;
				sleekSlider6.sizeOffset_Y = 20;
				sleekSlider6.orientation = ESleekOrientation.HORIZONTAL;
				sleekSlider6.addLabel(local.format("Single_" + k), ESleekSide.LEFT);
				SleekSlider sleekSlider7 = sleekSlider6;
				Delegate onDragged = sleekSlider7.onDragged;
				if (EditorEnvironmentLightingUI.<>f__mg$cache11 == null)
				{
					EditorEnvironmentLightingUI.<>f__mg$cache11 = new Dragged(EditorEnvironmentLightingUI.onDraggedSingleSlider);
				}
				sleekSlider7.onDragged = (Dragged)Delegate.Combine(onDragged, EditorEnvironmentLightingUI.<>f__mg$cache11);
				EditorEnvironmentLightingUI.lightingScrollBox.add(sleekSlider6);
				EditorEnvironmentLightingUI.singleSliders[k] = sleekSlider6;
			}
			EditorEnvironmentLightingUI.lightingScrollBox.area = new Rect(0f, 0f, 5f, (float)(EditorEnvironmentLightingUI.timeButtons.Length * 40 + EditorEnvironmentLightingUI.colorPickers.Length * 170 + EditorEnvironmentLightingUI.singleSliders.Length * 30 - 10));
			EditorEnvironmentLightingUI.updateSelection();
		}

		public static void open()
		{
			if (EditorEnvironmentLightingUI.active)
			{
				return;
			}
			EditorEnvironmentLightingUI.active = true;
			EditorEnvironmentLightingUI.container.lerpPositionScale(0f, 0f, ESleekLerp.EXPONENTIAL, 20f);
		}

		public static void close()
		{
			if (!EditorEnvironmentLightingUI.active)
			{
				return;
			}
			EditorEnvironmentLightingUI.active = false;
			EditorEnvironmentLightingUI.container.lerpPositionScale(1f, 0f, ESleekLerp.EXPONENTIAL, 20f);
		}

		private static void onDraggedAzimuthSlider(SleekSlider slider, float state)
		{
			LevelLighting.azimuth = state * 360f;
		}

		private static void onDraggedBiasSlider(SleekSlider slider, float state)
		{
			LevelLighting.bias = state;
		}

		private static void onDraggedFadeSlider(SleekSlider slider, float state)
		{
			LevelLighting.fade = state;
		}

		private static void onValuedSeaLevelSlider(SleekValue slider, float state)
		{
			LevelLighting.seaLevel = state;
		}

		private static void onValuedSnowLevelSlider(SleekValue slider, float state)
		{
			LevelLighting.snowLevel = state;
		}

		private static void onToggledRainToggle(SleekToggle toggle, bool state)
		{
			LevelLighting.canRain = state;
		}

		private static void onToggledSnowToggle(SleekToggle toggle, bool state)
		{
			LevelLighting.canSnow = state;
		}

		private static void onTypedRainFreqField(SleekSingleField field, float state)
		{
			LevelLighting.rainFreq = state;
		}

		private static void onTypedRainDurField(SleekSingleField field, float state)
		{
			LevelLighting.rainDur = state;
		}

		private static void onTypedSnowFreqField(SleekSingleField field, float state)
		{
			LevelLighting.snowFreq = state;
		}

		private static void onTypedSnowDurField(SleekSingleField field, float state)
		{
			LevelLighting.snowDur = state;
		}

		private static void onDraggedMoonSlider(SleekSlider slider, float state)
		{
			byte b = (byte)(state * (float)LevelLighting.MOON_CYCLES);
			if (b >= LevelLighting.MOON_CYCLES)
			{
				b = LevelLighting.MOON_CYCLES - 1;
			}
			LevelLighting.moon = b;
		}

		private static void onDraggedTimeSlider(SleekSlider slider, float state)
		{
			LevelLighting.time = state;
		}

		private static void onClickedTimeButton(SleekButton button)
		{
			int i;
			for (i = 0; i < EditorEnvironmentLightingUI.timeButtons.Length; i++)
			{
				if (EditorEnvironmentLightingUI.timeButtons[i] == button)
				{
					break;
				}
			}
			EditorEnvironmentLightingUI.selectedTime = (ELightingTime)i;
			EditorEnvironmentLightingUI.updateSelection();
			switch (EditorEnvironmentLightingUI.selectedTime)
			{
			case ELightingTime.DAWN:
				LevelLighting.time = 0f;
				break;
			case ELightingTime.MIDDAY:
				LevelLighting.time = LevelLighting.bias / 2f;
				break;
			case ELightingTime.DUSK:
				LevelLighting.time = LevelLighting.bias;
				break;
			case ELightingTime.MIDNIGHT:
				LevelLighting.time = 1f - (1f - LevelLighting.bias) / 2f;
				break;
			}
			LevelLighting.updateClouds();
			EditorEnvironmentLightingUI.timeSlider.state = LevelLighting.time;
		}

		private static void onClickedStormButton(SleekButton button)
		{
			if (LevelLighting.rainyness == ELightingRain.NONE)
			{
				LevelLighting.rainyness = ELightingRain.DRIZZLE;
			}
			else
			{
				LevelLighting.rainyness = ELightingRain.NONE;
			}
		}

		private static void onClickedBlizzardButton(SleekButton button)
		{
			if (LevelLighting.snowyness == ELightingSnow.NONE)
			{
				LevelLighting.snowyness = ELightingSnow.BLIZZARD;
			}
			else
			{
				LevelLighting.snowyness = ELightingSnow.NONE;
			}
		}

		private static void onPickedColorPicker(SleekColorPicker picker, Color state)
		{
			int i;
			for (i = 0; i < EditorEnvironmentLightingUI.colorPickers.Length; i++)
			{
				if (EditorEnvironmentLightingUI.colorPickers[i] == picker)
				{
					break;
				}
			}
			LevelLighting.times[(int)EditorEnvironmentLightingUI.selectedTime].colors[i] = state;
			LevelLighting.updateLighting();
		}

		private static void onDraggedSingleSlider(SleekSlider slider, float state)
		{
			int i;
			for (i = 0; i < EditorEnvironmentLightingUI.singleSliders.Length; i++)
			{
				if (EditorEnvironmentLightingUI.singleSliders[i] == slider)
				{
					break;
				}
			}
			LevelLighting.times[(int)EditorEnvironmentLightingUI.selectedTime].singles[i] = state;
			LevelLighting.updateLighting();
			if (i == 2)
			{
				LevelLighting.updateClouds();
			}
		}

		private static void updateSelection()
		{
			for (int i = 0; i < EditorEnvironmentLightingUI.colorPickers.Length; i++)
			{
				EditorEnvironmentLightingUI.colorPickers[i].state = LevelLighting.times[(int)EditorEnvironmentLightingUI.selectedTime].colors[i];
			}
			for (int j = 0; j < EditorEnvironmentLightingUI.singleSliders.Length; j++)
			{
				EditorEnvironmentLightingUI.singleSliders[j].state = LevelLighting.times[(int)EditorEnvironmentLightingUI.selectedTime].singles[j];
			}
		}

		private static Sleek container;

		public static bool active;

		private static SleekScrollBox lightingScrollBox;

		private static SleekSlider azimuthSlider;

		private static SleekSlider biasSlider;

		private static SleekSlider fadeSlider;

		private static SleekButton[] timeButtons;

		private static SleekBox[] infoBoxes;

		private static SleekColorPicker[] colorPickers;

		private static SleekSlider[] singleSliders;

		private static ELightingTime selectedTime;

		private static SleekValue seaLevelSlider;

		private static SleekValue snowLevelSlider;

		private static SleekSingleField rainFreqField;

		private static SleekSingleField rainDurField;

		private static SleekSingleField snowFreqField;

		private static SleekSingleField snowDurField;

		private static SleekButton stormButton;

		private static SleekToggle rainToggle;

		private static SleekButton blizzardButton;

		private static SleekToggle snowToggle;

		private static SleekSlider moonSlider;

		private static SleekSlider timeSlider;

		[CompilerGenerated]
		private static Dragged <>f__mg$cache0;

		[CompilerGenerated]
		private static Dragged <>f__mg$cache1;

		[CompilerGenerated]
		private static Dragged <>f__mg$cache2;

		[CompilerGenerated]
		private static Valued <>f__mg$cache3;

		[CompilerGenerated]
		private static Valued <>f__mg$cache4;

		[CompilerGenerated]
		private static TypedSingle <>f__mg$cache5;

		[CompilerGenerated]
		private static TypedSingle <>f__mg$cache6;

		[CompilerGenerated]
		private static TypedSingle <>f__mg$cache7;

		[CompilerGenerated]
		private static TypedSingle <>f__mg$cache8;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache9;

		[CompilerGenerated]
		private static Toggled <>f__mg$cacheA;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cacheB;

		[CompilerGenerated]
		private static Toggled <>f__mg$cacheC;

		[CompilerGenerated]
		private static Dragged <>f__mg$cacheD;

		[CompilerGenerated]
		private static Dragged <>f__mg$cacheE;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cacheF;

		[CompilerGenerated]
		private static ColorPicked <>f__mg$cache10;

		[CompilerGenerated]
		private static Dragged <>f__mg$cache11;
	}
}
