using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace SDG.Unturned
{
	public class MenuConfigurationOptionsUI
	{
		public MenuConfigurationOptionsUI()
		{
			MenuConfigurationOptionsUI.localization = Localization.read("/Menu/Configuration/MenuConfigurationOptions.dat");
			MenuConfigurationOptionsUI.container = new Sleek();
			MenuConfigurationOptionsUI.container.positionOffset_X = 10;
			MenuConfigurationOptionsUI.container.positionOffset_Y = 10;
			MenuConfigurationOptionsUI.container.positionScale_Y = 1f;
			MenuConfigurationOptionsUI.container.sizeOffset_X = -20;
			MenuConfigurationOptionsUI.container.sizeOffset_Y = -20;
			MenuConfigurationOptionsUI.container.sizeScale_X = 1f;
			MenuConfigurationOptionsUI.container.sizeScale_Y = 1f;
			if (Provider.isConnected)
			{
				PlayerUI.container.add(MenuConfigurationOptionsUI.container);
			}
			else
			{
				MenuUI.container.add(MenuConfigurationOptionsUI.container);
			}
			MenuConfigurationOptionsUI.active = false;
			MenuConfigurationOptionsUI.optionsBox = new SleekScrollBox();
			MenuConfigurationOptionsUI.optionsBox.positionOffset_X = -200;
			MenuConfigurationOptionsUI.optionsBox.positionOffset_Y = 100;
			MenuConfigurationOptionsUI.optionsBox.positionScale_X = 0.5f;
			MenuConfigurationOptionsUI.optionsBox.sizeOffset_X = 430;
			MenuConfigurationOptionsUI.optionsBox.sizeOffset_Y = -200;
			MenuConfigurationOptionsUI.optionsBox.sizeScale_Y = 1f;
			MenuConfigurationOptionsUI.optionsBox.area = new Rect(0f, 0f, 5f, 2160f);
			MenuConfigurationOptionsUI.container.add(MenuConfigurationOptionsUI.optionsBox);
			MenuConfigurationOptionsUI.fovSlider = new SleekSlider();
			MenuConfigurationOptionsUI.fovSlider.positionOffset_Y = 730;
			MenuConfigurationOptionsUI.fovSlider.sizeOffset_X = 200;
			MenuConfigurationOptionsUI.fovSlider.sizeOffset_Y = 20;
			MenuConfigurationOptionsUI.fovSlider.orientation = ESleekOrientation.HORIZONTAL;
			MenuConfigurationOptionsUI.fovSlider.addLabel(MenuConfigurationOptionsUI.localization.format("FOV_Slider_Label", new object[]
			{
				(int)OptionsSettings.view
			}), ESleekSide.RIGHT);
			SleekSlider sleekSlider = MenuConfigurationOptionsUI.fovSlider;
			if (MenuConfigurationOptionsUI.<>f__mg$cache0 == null)
			{
				MenuConfigurationOptionsUI.<>f__mg$cache0 = new Dragged(MenuConfigurationOptionsUI.onDraggedFOVSlider);
			}
			sleekSlider.onDragged = MenuConfigurationOptionsUI.<>f__mg$cache0;
			MenuConfigurationOptionsUI.optionsBox.add(MenuConfigurationOptionsUI.fovSlider);
			MenuConfigurationOptionsUI.volumeSlider = new SleekSlider();
			MenuConfigurationOptionsUI.volumeSlider.positionOffset_Y = 760;
			MenuConfigurationOptionsUI.volumeSlider.sizeOffset_X = 200;
			MenuConfigurationOptionsUI.volumeSlider.sizeOffset_Y = 20;
			MenuConfigurationOptionsUI.volumeSlider.orientation = ESleekOrientation.HORIZONTAL;
			MenuConfigurationOptionsUI.volumeSlider.addLabel(MenuConfigurationOptionsUI.localization.format("Volume_Slider_Label", new object[]
			{
				(int)(OptionsSettings.volume * 100f)
			}), ESleekSide.RIGHT);
			SleekSlider sleekSlider2 = MenuConfigurationOptionsUI.volumeSlider;
			if (MenuConfigurationOptionsUI.<>f__mg$cache1 == null)
			{
				MenuConfigurationOptionsUI.<>f__mg$cache1 = new Dragged(MenuConfigurationOptionsUI.onDraggedVolumeSlider);
			}
			sleekSlider2.onDragged = MenuConfigurationOptionsUI.<>f__mg$cache1;
			MenuConfigurationOptionsUI.optionsBox.add(MenuConfigurationOptionsUI.volumeSlider);
			MenuConfigurationOptionsUI.voiceSlider = new SleekSlider();
			MenuConfigurationOptionsUI.voiceSlider.positionOffset_Y = 790;
			MenuConfigurationOptionsUI.voiceSlider.sizeOffset_X = 200;
			MenuConfigurationOptionsUI.voiceSlider.sizeOffset_Y = 20;
			MenuConfigurationOptionsUI.voiceSlider.orientation = ESleekOrientation.HORIZONTAL;
			MenuConfigurationOptionsUI.voiceSlider.addLabel(MenuConfigurationOptionsUI.localization.format("Voice_Slider_Label", new object[]
			{
				(int)(OptionsSettings.voice * 100f)
			}), ESleekSide.RIGHT);
			SleekSlider sleekSlider3 = MenuConfigurationOptionsUI.voiceSlider;
			if (MenuConfigurationOptionsUI.<>f__mg$cache2 == null)
			{
				MenuConfigurationOptionsUI.<>f__mg$cache2 = new Dragged(MenuConfigurationOptionsUI.onDraggedVoiceSlider);
			}
			sleekSlider3.onDragged = MenuConfigurationOptionsUI.<>f__mg$cache2;
			MenuConfigurationOptionsUI.optionsBox.add(MenuConfigurationOptionsUI.voiceSlider);
			MenuConfigurationOptionsUI.debugToggle = new SleekToggle();
			MenuConfigurationOptionsUI.debugToggle.sizeOffset_X = 40;
			MenuConfigurationOptionsUI.debugToggle.sizeOffset_Y = 40;
			MenuConfigurationOptionsUI.debugToggle.addLabel(MenuConfigurationOptionsUI.localization.format("Debug_Toggle_Label"), ESleekSide.RIGHT);
			SleekToggle sleekToggle = MenuConfigurationOptionsUI.debugToggle;
			if (MenuConfigurationOptionsUI.<>f__mg$cache3 == null)
			{
				MenuConfigurationOptionsUI.<>f__mg$cache3 = new Toggled(MenuConfigurationOptionsUI.onToggledDebugToggle);
			}
			sleekToggle.onToggled = MenuConfigurationOptionsUI.<>f__mg$cache3;
			MenuConfigurationOptionsUI.optionsBox.add(MenuConfigurationOptionsUI.debugToggle);
			MenuConfigurationOptionsUI.musicToggle = new SleekToggle();
			MenuConfigurationOptionsUI.musicToggle.positionOffset_Y = 50;
			MenuConfigurationOptionsUI.musicToggle.sizeOffset_X = 40;
			MenuConfigurationOptionsUI.musicToggle.sizeOffset_Y = 40;
			MenuConfigurationOptionsUI.musicToggle.addLabel(MenuConfigurationOptionsUI.localization.format("Music_Toggle_Label"), ESleekSide.RIGHT);
			SleekToggle sleekToggle2 = MenuConfigurationOptionsUI.musicToggle;
			if (MenuConfigurationOptionsUI.<>f__mg$cache4 == null)
			{
				MenuConfigurationOptionsUI.<>f__mg$cache4 = new Toggled(MenuConfigurationOptionsUI.onToggledMusicToggle);
			}
			sleekToggle2.onToggled = MenuConfigurationOptionsUI.<>f__mg$cache4;
			MenuConfigurationOptionsUI.optionsBox.add(MenuConfigurationOptionsUI.musicToggle);
			MenuConfigurationOptionsUI.timerToggle = new SleekToggle();
			MenuConfigurationOptionsUI.timerToggle.positionOffset_Y = 100;
			MenuConfigurationOptionsUI.timerToggle.sizeOffset_X = 40;
			MenuConfigurationOptionsUI.timerToggle.sizeOffset_Y = 40;
			MenuConfigurationOptionsUI.timerToggle.addLabel(MenuConfigurationOptionsUI.localization.format("Timer_Toggle_Label"), ESleekSide.RIGHT);
			SleekToggle sleekToggle3 = MenuConfigurationOptionsUI.timerToggle;
			if (MenuConfigurationOptionsUI.<>f__mg$cache5 == null)
			{
				MenuConfigurationOptionsUI.<>f__mg$cache5 = new Toggled(MenuConfigurationOptionsUI.onToggledTimerToggle);
			}
			sleekToggle3.onToggled = MenuConfigurationOptionsUI.<>f__mg$cache5;
			MenuConfigurationOptionsUI.optionsBox.add(MenuConfigurationOptionsUI.timerToggle);
			MenuConfigurationOptionsUI.goreToggle = new SleekToggle();
			MenuConfigurationOptionsUI.goreToggle.positionOffset_Y = 150;
			MenuConfigurationOptionsUI.goreToggle.sizeOffset_X = 40;
			MenuConfigurationOptionsUI.goreToggle.sizeOffset_Y = 40;
			MenuConfigurationOptionsUI.goreToggle.addLabel(MenuConfigurationOptionsUI.localization.format("Gore_Toggle_Label"), ESleekSide.RIGHT);
			SleekToggle sleekToggle4 = MenuConfigurationOptionsUI.goreToggle;
			if (MenuConfigurationOptionsUI.<>f__mg$cache6 == null)
			{
				MenuConfigurationOptionsUI.<>f__mg$cache6 = new Toggled(MenuConfigurationOptionsUI.onToggledGoreToggle);
			}
			sleekToggle4.onToggled = MenuConfigurationOptionsUI.<>f__mg$cache6;
			MenuConfigurationOptionsUI.optionsBox.add(MenuConfigurationOptionsUI.goreToggle);
			MenuConfigurationOptionsUI.filterToggle = new SleekToggle();
			MenuConfigurationOptionsUI.filterToggle.positionOffset_Y = 200;
			MenuConfigurationOptionsUI.filterToggle.sizeOffset_X = 40;
			MenuConfigurationOptionsUI.filterToggle.sizeOffset_Y = 40;
			MenuConfigurationOptionsUI.filterToggle.addLabel(MenuConfigurationOptionsUI.localization.format("Filter_Toggle_Label"), ESleekSide.RIGHT);
			SleekToggle sleekToggle5 = MenuConfigurationOptionsUI.filterToggle;
			if (MenuConfigurationOptionsUI.<>f__mg$cache7 == null)
			{
				MenuConfigurationOptionsUI.<>f__mg$cache7 = new Toggled(MenuConfigurationOptionsUI.onToggledFilterToggle);
			}
			sleekToggle5.onToggled = MenuConfigurationOptionsUI.<>f__mg$cache7;
			MenuConfigurationOptionsUI.optionsBox.add(MenuConfigurationOptionsUI.filterToggle);
			MenuConfigurationOptionsUI.chatTextToggle = new SleekToggle();
			MenuConfigurationOptionsUI.chatTextToggle.positionOffset_Y = 250;
			MenuConfigurationOptionsUI.chatTextToggle.sizeOffset_X = 40;
			MenuConfigurationOptionsUI.chatTextToggle.sizeOffset_Y = 40;
			MenuConfigurationOptionsUI.chatTextToggle.addLabel(MenuConfigurationOptionsUI.localization.format("Chat_Text_Toggle_Label"), ESleekSide.RIGHT);
			SleekToggle sleekToggle6 = MenuConfigurationOptionsUI.chatTextToggle;
			if (MenuConfigurationOptionsUI.<>f__mg$cache8 == null)
			{
				MenuConfigurationOptionsUI.<>f__mg$cache8 = new Toggled(MenuConfigurationOptionsUI.onToggledChatTextToggle);
			}
			sleekToggle6.onToggled = MenuConfigurationOptionsUI.<>f__mg$cache8;
			MenuConfigurationOptionsUI.optionsBox.add(MenuConfigurationOptionsUI.chatTextToggle);
			MenuConfigurationOptionsUI.chatVoiceInToggle = new SleekToggle();
			MenuConfigurationOptionsUI.chatVoiceInToggle.positionOffset_Y = 300;
			MenuConfigurationOptionsUI.chatVoiceInToggle.sizeOffset_X = 40;
			MenuConfigurationOptionsUI.chatVoiceInToggle.sizeOffset_Y = 40;
			MenuConfigurationOptionsUI.chatVoiceInToggle.addLabel(MenuConfigurationOptionsUI.localization.format("Chat_Voice_In_Toggle_Label"), ESleekSide.RIGHT);
			SleekToggle sleekToggle7 = MenuConfigurationOptionsUI.chatVoiceInToggle;
			if (MenuConfigurationOptionsUI.<>f__mg$cache9 == null)
			{
				MenuConfigurationOptionsUI.<>f__mg$cache9 = new Toggled(MenuConfigurationOptionsUI.onToggledChatVoiceInToggle);
			}
			sleekToggle7.onToggled = MenuConfigurationOptionsUI.<>f__mg$cache9;
			MenuConfigurationOptionsUI.optionsBox.add(MenuConfigurationOptionsUI.chatVoiceInToggle);
			MenuConfigurationOptionsUI.chatVoiceOutToggle = new SleekToggle();
			MenuConfigurationOptionsUI.chatVoiceOutToggle.positionOffset_Y = 350;
			MenuConfigurationOptionsUI.chatVoiceOutToggle.sizeOffset_X = 40;
			MenuConfigurationOptionsUI.chatVoiceOutToggle.sizeOffset_Y = 40;
			MenuConfigurationOptionsUI.chatVoiceOutToggle.addLabel(MenuConfigurationOptionsUI.localization.format("Chat_Voice_Out_Toggle_Label"), ESleekSide.RIGHT);
			SleekToggle sleekToggle8 = MenuConfigurationOptionsUI.chatVoiceOutToggle;
			if (MenuConfigurationOptionsUI.<>f__mg$cacheA == null)
			{
				MenuConfigurationOptionsUI.<>f__mg$cacheA = new Toggled(MenuConfigurationOptionsUI.onToggledChatVoiceOutToggle);
			}
			sleekToggle8.onToggled = MenuConfigurationOptionsUI.<>f__mg$cacheA;
			MenuConfigurationOptionsUI.optionsBox.add(MenuConfigurationOptionsUI.chatVoiceOutToggle);
			MenuConfigurationOptionsUI.hintsToggle = new SleekToggle();
			MenuConfigurationOptionsUI.hintsToggle.positionOffset_Y = 400;
			MenuConfigurationOptionsUI.hintsToggle.sizeOffset_X = 40;
			MenuConfigurationOptionsUI.hintsToggle.sizeOffset_Y = 40;
			MenuConfigurationOptionsUI.hintsToggle.addLabel(MenuConfigurationOptionsUI.localization.format("Hints_Toggle_Label"), ESleekSide.RIGHT);
			SleekToggle sleekToggle9 = MenuConfigurationOptionsUI.hintsToggle;
			if (MenuConfigurationOptionsUI.<>f__mg$cacheB == null)
			{
				MenuConfigurationOptionsUI.<>f__mg$cacheB = new Toggled(MenuConfigurationOptionsUI.onToggledHintsToggle);
			}
			sleekToggle9.onToggled = MenuConfigurationOptionsUI.<>f__mg$cacheB;
			MenuConfigurationOptionsUI.optionsBox.add(MenuConfigurationOptionsUI.hintsToggle);
			MenuConfigurationOptionsUI.ambienceToggle = new SleekToggle();
			MenuConfigurationOptionsUI.ambienceToggle.positionOffset_Y = 450;
			MenuConfigurationOptionsUI.ambienceToggle.sizeOffset_X = 40;
			MenuConfigurationOptionsUI.ambienceToggle.sizeOffset_Y = 40;
			MenuConfigurationOptionsUI.ambienceToggle.addLabel(MenuConfigurationOptionsUI.localization.format("Ambience_Toggle_Label"), ESleekSide.RIGHT);
			SleekToggle sleekToggle10 = MenuConfigurationOptionsUI.ambienceToggle;
			if (MenuConfigurationOptionsUI.<>f__mg$cacheC == null)
			{
				MenuConfigurationOptionsUI.<>f__mg$cacheC = new Toggled(MenuConfigurationOptionsUI.onToggledAmbienceToggle);
			}
			sleekToggle10.onToggled = MenuConfigurationOptionsUI.<>f__mg$cacheC;
			MenuConfigurationOptionsUI.optionsBox.add(MenuConfigurationOptionsUI.ambienceToggle);
			MenuConfigurationOptionsUI.streamerToggle = new SleekToggle();
			MenuConfigurationOptionsUI.streamerToggle.positionOffset_Y = 500;
			MenuConfigurationOptionsUI.streamerToggle.sizeOffset_X = 40;
			MenuConfigurationOptionsUI.streamerToggle.sizeOffset_Y = 40;
			MenuConfigurationOptionsUI.streamerToggle.addLabel(MenuConfigurationOptionsUI.localization.format("Streamer_Toggle_Label"), ESleekSide.RIGHT);
			SleekToggle sleekToggle11 = MenuConfigurationOptionsUI.streamerToggle;
			if (MenuConfigurationOptionsUI.<>f__mg$cacheD == null)
			{
				MenuConfigurationOptionsUI.<>f__mg$cacheD = new Toggled(MenuConfigurationOptionsUI.onToggledStreamerToggle);
			}
			sleekToggle11.onToggled = MenuConfigurationOptionsUI.<>f__mg$cacheD;
			MenuConfigurationOptionsUI.optionsBox.add(MenuConfigurationOptionsUI.streamerToggle);
			MenuConfigurationOptionsUI.featuredWorkshopToggle = new SleekToggle();
			MenuConfigurationOptionsUI.featuredWorkshopToggle.positionOffset_Y = 550;
			MenuConfigurationOptionsUI.featuredWorkshopToggle.sizeOffset_X = 40;
			MenuConfigurationOptionsUI.featuredWorkshopToggle.sizeOffset_Y = 40;
			MenuConfigurationOptionsUI.featuredWorkshopToggle.addLabel(MenuConfigurationOptionsUI.localization.format("Featured_Workshop_Toggle_Label"), ESleekSide.RIGHT);
			SleekToggle sleekToggle12 = MenuConfigurationOptionsUI.featuredWorkshopToggle;
			if (MenuConfigurationOptionsUI.<>f__mg$cacheE == null)
			{
				MenuConfigurationOptionsUI.<>f__mg$cacheE = new Toggled(MenuConfigurationOptionsUI.onToggledFeaturedWorkshopToggle);
			}
			sleekToggle12.onToggled = MenuConfigurationOptionsUI.<>f__mg$cacheE;
			MenuConfigurationOptionsUI.optionsBox.add(MenuConfigurationOptionsUI.featuredWorkshopToggle);
			MenuConfigurationOptionsUI.matchmakingShowAllMapsToggle = new SleekToggle();
			MenuConfigurationOptionsUI.matchmakingShowAllMapsToggle.positionOffset_Y = 600;
			MenuConfigurationOptionsUI.matchmakingShowAllMapsToggle.sizeOffset_X = 40;
			MenuConfigurationOptionsUI.matchmakingShowAllMapsToggle.sizeOffset_Y = 40;
			MenuConfigurationOptionsUI.matchmakingShowAllMapsToggle.addLabel(MenuConfigurationOptionsUI.localization.format("Matchmaking_Show_All_Maps_Toggle_Label"), ESleekSide.RIGHT);
			SleekToggle sleekToggle13 = MenuConfigurationOptionsUI.matchmakingShowAllMapsToggle;
			if (MenuConfigurationOptionsUI.<>f__mg$cacheF == null)
			{
				MenuConfigurationOptionsUI.<>f__mg$cacheF = new Toggled(MenuConfigurationOptionsUI.onToggledMatchmakingShowAllMapsToggle);
			}
			sleekToggle13.onToggled = MenuConfigurationOptionsUI.<>f__mg$cacheF;
			MenuConfigurationOptionsUI.optionsBox.add(MenuConfigurationOptionsUI.matchmakingShowAllMapsToggle);
			MenuConfigurationOptionsUI.minMatchmakingPlayersField = new SleekInt32Field();
			MenuConfigurationOptionsUI.minMatchmakingPlayersField.positionOffset_Y = 650;
			MenuConfigurationOptionsUI.minMatchmakingPlayersField.sizeOffset_X = 200;
			MenuConfigurationOptionsUI.minMatchmakingPlayersField.sizeOffset_Y = 30;
			MenuConfigurationOptionsUI.minMatchmakingPlayersField.addLabel(MenuConfigurationOptionsUI.localization.format("Min_Matchmaking_Players_Field_Label"), ESleekSide.RIGHT);
			SleekInt32Field sleekInt32Field = MenuConfigurationOptionsUI.minMatchmakingPlayersField;
			if (MenuConfigurationOptionsUI.<>f__mg$cache10 == null)
			{
				MenuConfigurationOptionsUI.<>f__mg$cache10 = new TypedInt32(MenuConfigurationOptionsUI.onTypedMinMatchmakingPlayersField);
			}
			sleekInt32Field.onTypedInt = MenuConfigurationOptionsUI.<>f__mg$cache10;
			MenuConfigurationOptionsUI.optionsBox.add(MenuConfigurationOptionsUI.minMatchmakingPlayersField);
			MenuConfigurationOptionsUI.maxMatchmakingPingField = new SleekInt32Field();
			MenuConfigurationOptionsUI.maxMatchmakingPingField.positionOffset_Y = 690;
			MenuConfigurationOptionsUI.maxMatchmakingPingField.sizeOffset_X = 200;
			MenuConfigurationOptionsUI.maxMatchmakingPingField.sizeOffset_Y = 30;
			MenuConfigurationOptionsUI.maxMatchmakingPingField.addLabel(MenuConfigurationOptionsUI.localization.format("Max_Matchmaking_Ping_Field_Label"), ESleekSide.RIGHT);
			SleekInt32Field sleekInt32Field2 = MenuConfigurationOptionsUI.maxMatchmakingPingField;
			if (MenuConfigurationOptionsUI.<>f__mg$cache11 == null)
			{
				MenuConfigurationOptionsUI.<>f__mg$cache11 = new TypedInt32(MenuConfigurationOptionsUI.onTypedMaxMatchmakingPingField);
			}
			sleekInt32Field2.onTypedInt = MenuConfigurationOptionsUI.<>f__mg$cache11;
			MenuConfigurationOptionsUI.optionsBox.add(MenuConfigurationOptionsUI.maxMatchmakingPingField);
			MenuConfigurationOptionsUI.talkButton = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(MenuConfigurationOptionsUI.localization.format("Talk_Off")),
				new GUIContent(MenuConfigurationOptionsUI.localization.format("Talk_On"))
			});
			MenuConfigurationOptionsUI.talkButton.positionOffset_Y = 820;
			MenuConfigurationOptionsUI.talkButton.sizeOffset_X = 200;
			MenuConfigurationOptionsUI.talkButton.sizeOffset_Y = 30;
			MenuConfigurationOptionsUI.talkButton.state = ((!OptionsSettings.talk) ? 0 : 1);
			MenuConfigurationOptionsUI.talkButton.tooltip = MenuConfigurationOptionsUI.localization.format("Talk_Tooltip");
			SleekButtonState sleekButtonState = MenuConfigurationOptionsUI.talkButton;
			if (MenuConfigurationOptionsUI.<>f__mg$cache12 == null)
			{
				MenuConfigurationOptionsUI.<>f__mg$cache12 = new SwappedState(MenuConfigurationOptionsUI.onSwappedTalkState);
			}
			sleekButtonState.onSwappedState = MenuConfigurationOptionsUI.<>f__mg$cache12;
			MenuConfigurationOptionsUI.optionsBox.add(MenuConfigurationOptionsUI.talkButton);
			MenuConfigurationOptionsUI.metricButton = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(MenuConfigurationOptionsUI.localization.format("Metric_Off")),
				new GUIContent(MenuConfigurationOptionsUI.localization.format("Metric_On"))
			});
			MenuConfigurationOptionsUI.metricButton.positionOffset_Y = 860;
			MenuConfigurationOptionsUI.metricButton.sizeOffset_X = 200;
			MenuConfigurationOptionsUI.metricButton.sizeOffset_Y = 30;
			MenuConfigurationOptionsUI.metricButton.state = ((!OptionsSettings.metric) ? 0 : 1);
			MenuConfigurationOptionsUI.metricButton.tooltip = MenuConfigurationOptionsUI.localization.format("Metric_Tooltip");
			SleekButtonState sleekButtonState2 = MenuConfigurationOptionsUI.metricButton;
			if (MenuConfigurationOptionsUI.<>f__mg$cache13 == null)
			{
				MenuConfigurationOptionsUI.<>f__mg$cache13 = new SwappedState(MenuConfigurationOptionsUI.onSwappedMetricState);
			}
			sleekButtonState2.onSwappedState = MenuConfigurationOptionsUI.<>f__mg$cache13;
			MenuConfigurationOptionsUI.optionsBox.add(MenuConfigurationOptionsUI.metricButton);
			MenuConfigurationOptionsUI.uiButton = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(MenuConfigurationOptionsUI.localization.format("UI_Free")),
				new GUIContent(MenuConfigurationOptionsUI.localization.format("UI_Pro"))
			});
			MenuConfigurationOptionsUI.uiButton.positionOffset_Y = 900;
			MenuConfigurationOptionsUI.uiButton.sizeOffset_X = 200;
			MenuConfigurationOptionsUI.uiButton.sizeOffset_Y = 30;
			MenuConfigurationOptionsUI.uiButton.tooltip = MenuConfigurationOptionsUI.localization.format("UI_Tooltip");
			SleekButtonState sleekButtonState3 = MenuConfigurationOptionsUI.uiButton;
			if (MenuConfigurationOptionsUI.<>f__mg$cache14 == null)
			{
				MenuConfigurationOptionsUI.<>f__mg$cache14 = new SwappedState(MenuConfigurationOptionsUI.onSwappedUIState);
			}
			sleekButtonState3.onSwappedState = MenuConfigurationOptionsUI.<>f__mg$cache14;
			MenuConfigurationOptionsUI.optionsBox.add(MenuConfigurationOptionsUI.uiButton);
			MenuConfigurationOptionsUI.hitmarkerButton = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(MenuConfigurationOptionsUI.localization.format("Hitmarker_Static")),
				new GUIContent(MenuConfigurationOptionsUI.localization.format("Hitmarker_Dynamic"))
			});
			MenuConfigurationOptionsUI.hitmarkerButton.positionOffset_Y = 940;
			MenuConfigurationOptionsUI.hitmarkerButton.sizeOffset_X = 200;
			MenuConfigurationOptionsUI.hitmarkerButton.sizeOffset_Y = 30;
			MenuConfigurationOptionsUI.hitmarkerButton.tooltip = MenuConfigurationOptionsUI.localization.format("Hitmarker_Tooltip");
			SleekButtonState sleekButtonState4 = MenuConfigurationOptionsUI.hitmarkerButton;
			if (MenuConfigurationOptionsUI.<>f__mg$cache15 == null)
			{
				MenuConfigurationOptionsUI.<>f__mg$cache15 = new SwappedState(MenuConfigurationOptionsUI.onSwappedHitmarkerState);
			}
			sleekButtonState4.onSwappedState = MenuConfigurationOptionsUI.<>f__mg$cache15;
			MenuConfigurationOptionsUI.optionsBox.add(MenuConfigurationOptionsUI.hitmarkerButton);
			MenuConfigurationOptionsUI.crosshairBox = new SleekBox();
			MenuConfigurationOptionsUI.crosshairBox.positionOffset_Y = 980;
			MenuConfigurationOptionsUI.crosshairBox.sizeOffset_X = 240;
			MenuConfigurationOptionsUI.crosshairBox.sizeOffset_Y = 30;
			MenuConfigurationOptionsUI.crosshairBox.text = MenuConfigurationOptionsUI.localization.format("Crosshair_Box");
			MenuConfigurationOptionsUI.optionsBox.add(MenuConfigurationOptionsUI.crosshairBox);
			MenuConfigurationOptionsUI.crosshairColorPicker = new SleekColorPicker();
			MenuConfigurationOptionsUI.crosshairColorPicker.positionOffset_Y = 1020;
			SleekColorPicker sleekColorPicker = MenuConfigurationOptionsUI.crosshairColorPicker;
			if (MenuConfigurationOptionsUI.<>f__mg$cache16 == null)
			{
				MenuConfigurationOptionsUI.<>f__mg$cache16 = new ColorPicked(MenuConfigurationOptionsUI.onCrosshairColorPicked);
			}
			sleekColorPicker.onColorPicked = MenuConfigurationOptionsUI.<>f__mg$cache16;
			MenuConfigurationOptionsUI.optionsBox.add(MenuConfigurationOptionsUI.crosshairColorPicker);
			MenuConfigurationOptionsUI.hitmarkerBox = new SleekBox();
			MenuConfigurationOptionsUI.hitmarkerBox.positionOffset_Y = 1150;
			MenuConfigurationOptionsUI.hitmarkerBox.sizeOffset_X = 240;
			MenuConfigurationOptionsUI.hitmarkerBox.sizeOffset_Y = 30;
			MenuConfigurationOptionsUI.hitmarkerBox.text = MenuConfigurationOptionsUI.localization.format("Hitmarker_Box");
			MenuConfigurationOptionsUI.optionsBox.add(MenuConfigurationOptionsUI.hitmarkerBox);
			MenuConfigurationOptionsUI.hitmarkerColorPicker = new SleekColorPicker();
			MenuConfigurationOptionsUI.hitmarkerColorPicker.positionOffset_Y = 1190;
			SleekColorPicker sleekColorPicker2 = MenuConfigurationOptionsUI.hitmarkerColorPicker;
			if (MenuConfigurationOptionsUI.<>f__mg$cache17 == null)
			{
				MenuConfigurationOptionsUI.<>f__mg$cache17 = new ColorPicked(MenuConfigurationOptionsUI.onHitmarkerColorPicked);
			}
			sleekColorPicker2.onColorPicked = MenuConfigurationOptionsUI.<>f__mg$cache17;
			MenuConfigurationOptionsUI.optionsBox.add(MenuConfigurationOptionsUI.hitmarkerColorPicker);
			MenuConfigurationOptionsUI.criticalHitmarkerBox = new SleekBox();
			MenuConfigurationOptionsUI.criticalHitmarkerBox.positionOffset_Y = 1320;
			MenuConfigurationOptionsUI.criticalHitmarkerBox.sizeOffset_X = 240;
			MenuConfigurationOptionsUI.criticalHitmarkerBox.sizeOffset_Y = 30;
			MenuConfigurationOptionsUI.criticalHitmarkerBox.text = MenuConfigurationOptionsUI.localization.format("Critical_Hitmarker_Box");
			MenuConfigurationOptionsUI.optionsBox.add(MenuConfigurationOptionsUI.criticalHitmarkerBox);
			MenuConfigurationOptionsUI.criticalHitmarkerColorPicker = new SleekColorPicker();
			MenuConfigurationOptionsUI.criticalHitmarkerColorPicker.positionOffset_Y = 1360;
			SleekColorPicker sleekColorPicker3 = MenuConfigurationOptionsUI.criticalHitmarkerColorPicker;
			if (MenuConfigurationOptionsUI.<>f__mg$cache18 == null)
			{
				MenuConfigurationOptionsUI.<>f__mg$cache18 = new ColorPicked(MenuConfigurationOptionsUI.onCriticalHitmarkerColorPicked);
			}
			sleekColorPicker3.onColorPicked = MenuConfigurationOptionsUI.<>f__mg$cache18;
			MenuConfigurationOptionsUI.optionsBox.add(MenuConfigurationOptionsUI.criticalHitmarkerColorPicker);
			MenuConfigurationOptionsUI.cursorBox = new SleekBox();
			MenuConfigurationOptionsUI.cursorBox.positionOffset_Y = 1490;
			MenuConfigurationOptionsUI.cursorBox.sizeOffset_X = 240;
			MenuConfigurationOptionsUI.cursorBox.sizeOffset_Y = 30;
			MenuConfigurationOptionsUI.cursorBox.text = MenuConfigurationOptionsUI.localization.format("Cursor_Box");
			MenuConfigurationOptionsUI.optionsBox.add(MenuConfigurationOptionsUI.cursorBox);
			MenuConfigurationOptionsUI.cursorColorPicker = new SleekColorPicker();
			MenuConfigurationOptionsUI.cursorColorPicker.positionOffset_Y = 1530;
			SleekColorPicker sleekColorPicker4 = MenuConfigurationOptionsUI.cursorColorPicker;
			if (MenuConfigurationOptionsUI.<>f__mg$cache19 == null)
			{
				MenuConfigurationOptionsUI.<>f__mg$cache19 = new ColorPicked(MenuConfigurationOptionsUI.onCursorColorPicked);
			}
			sleekColorPicker4.onColorPicked = MenuConfigurationOptionsUI.<>f__mg$cache19;
			MenuConfigurationOptionsUI.optionsBox.add(MenuConfigurationOptionsUI.cursorColorPicker);
			MenuConfigurationOptionsUI.backgroundBox = new SleekBox();
			MenuConfigurationOptionsUI.backgroundBox.positionOffset_Y = 1660;
			MenuConfigurationOptionsUI.backgroundBox.sizeOffset_X = 240;
			MenuConfigurationOptionsUI.backgroundBox.sizeOffset_Y = 30;
			MenuConfigurationOptionsUI.backgroundBox.text = MenuConfigurationOptionsUI.localization.format("Background_Box");
			MenuConfigurationOptionsUI.optionsBox.add(MenuConfigurationOptionsUI.backgroundBox);
			MenuConfigurationOptionsUI.backgroundColorPicker = new SleekColorPicker();
			MenuConfigurationOptionsUI.backgroundColorPicker.positionOffset_Y = 1700;
			if (Provider.isPro)
			{
				SleekColorPicker sleekColorPicker5 = MenuConfigurationOptionsUI.backgroundColorPicker;
				if (MenuConfigurationOptionsUI.<>f__mg$cache1A == null)
				{
					MenuConfigurationOptionsUI.<>f__mg$cache1A = new ColorPicked(MenuConfigurationOptionsUI.onBackgroundColorPicked);
				}
				sleekColorPicker5.onColorPicked = MenuConfigurationOptionsUI.<>f__mg$cache1A;
			}
			else
			{
				Bundle bundle = Bundles.getBundle("/Bundles/Textures/Menu/Icons/Pro/Pro.unity3d");
				SleekImageTexture sleekImageTexture = new SleekImageTexture();
				sleekImageTexture.positionOffset_X = -40;
				sleekImageTexture.positionOffset_Y = -40;
				sleekImageTexture.positionScale_X = 0.5f;
				sleekImageTexture.positionScale_Y = 0.5f;
				sleekImageTexture.sizeOffset_X = 80;
				sleekImageTexture.sizeOffset_Y = 80;
				sleekImageTexture.texture = (Texture2D)bundle.load("Lock_Large");
				MenuConfigurationOptionsUI.backgroundColorPicker.add(sleekImageTexture);
				bundle.unload();
			}
			MenuConfigurationOptionsUI.optionsBox.add(MenuConfigurationOptionsUI.backgroundColorPicker);
			MenuConfigurationOptionsUI.foregroundBox = new SleekBox();
			MenuConfigurationOptionsUI.foregroundBox.positionOffset_Y = 1830;
			MenuConfigurationOptionsUI.foregroundBox.sizeOffset_X = 240;
			MenuConfigurationOptionsUI.foregroundBox.sizeOffset_Y = 30;
			MenuConfigurationOptionsUI.foregroundBox.text = MenuConfigurationOptionsUI.localization.format("Foreground_Box");
			MenuConfigurationOptionsUI.optionsBox.add(MenuConfigurationOptionsUI.foregroundBox);
			MenuConfigurationOptionsUI.foregroundColorPicker = new SleekColorPicker();
			MenuConfigurationOptionsUI.foregroundColorPicker.positionOffset_Y = 1870;
			if (Provider.isPro)
			{
				SleekColorPicker sleekColorPicker6 = MenuConfigurationOptionsUI.foregroundColorPicker;
				if (MenuConfigurationOptionsUI.<>f__mg$cache1B == null)
				{
					MenuConfigurationOptionsUI.<>f__mg$cache1B = new ColorPicked(MenuConfigurationOptionsUI.onForegroundColorPicked);
				}
				sleekColorPicker6.onColorPicked = MenuConfigurationOptionsUI.<>f__mg$cache1B;
			}
			else
			{
				Bundle bundle2 = Bundles.getBundle("/Bundles/Textures/Menu/Icons/Pro/Pro.unity3d");
				SleekImageTexture sleekImageTexture2 = new SleekImageTexture();
				sleekImageTexture2.positionOffset_X = -40;
				sleekImageTexture2.positionOffset_Y = -40;
				sleekImageTexture2.positionScale_X = 0.5f;
				sleekImageTexture2.positionScale_Y = 0.5f;
				sleekImageTexture2.sizeOffset_X = 80;
				sleekImageTexture2.sizeOffset_Y = 80;
				sleekImageTexture2.texture = (Texture2D)bundle2.load("Lock_Large");
				MenuConfigurationOptionsUI.foregroundColorPicker.add(sleekImageTexture2);
				bundle2.unload();
			}
			MenuConfigurationOptionsUI.optionsBox.add(MenuConfigurationOptionsUI.foregroundColorPicker);
			MenuConfigurationOptionsUI.fontBox = new SleekBox();
			MenuConfigurationOptionsUI.fontBox.positionOffset_Y = 2000;
			MenuConfigurationOptionsUI.fontBox.sizeOffset_X = 240;
			MenuConfigurationOptionsUI.fontBox.sizeOffset_Y = 30;
			MenuConfigurationOptionsUI.fontBox.text = MenuConfigurationOptionsUI.localization.format("Font_Box");
			MenuConfigurationOptionsUI.optionsBox.add(MenuConfigurationOptionsUI.fontBox);
			MenuConfigurationOptionsUI.fontColorPicker = new SleekColorPicker();
			MenuConfigurationOptionsUI.fontColorPicker.positionOffset_Y = 2040;
			if (Provider.isPro)
			{
				SleekColorPicker sleekColorPicker7 = MenuConfigurationOptionsUI.fontColorPicker;
				if (MenuConfigurationOptionsUI.<>f__mg$cache1C == null)
				{
					MenuConfigurationOptionsUI.<>f__mg$cache1C = new ColorPicked(MenuConfigurationOptionsUI.onFontColorPicked);
				}
				sleekColorPicker7.onColorPicked = MenuConfigurationOptionsUI.<>f__mg$cache1C;
			}
			else
			{
				Bundle bundle3 = Bundles.getBundle("/Bundles/Textures/Menu/Icons/Pro/Pro.unity3d");
				SleekImageTexture sleekImageTexture3 = new SleekImageTexture();
				sleekImageTexture3.positionOffset_X = -40;
				sleekImageTexture3.positionOffset_Y = -40;
				sleekImageTexture3.positionScale_X = 0.5f;
				sleekImageTexture3.positionScale_Y = 0.5f;
				sleekImageTexture3.sizeOffset_X = 80;
				sleekImageTexture3.sizeOffset_Y = 80;
				sleekImageTexture3.texture = (Texture2D)bundle3.load("Lock_Large");
				MenuConfigurationOptionsUI.fontColorPicker.add(sleekImageTexture3);
				bundle3.unload();
			}
			MenuConfigurationOptionsUI.optionsBox.add(MenuConfigurationOptionsUI.fontColorPicker);
			MenuConfigurationOptionsUI.backButton = new SleekButtonIcon((Texture2D)MenuDashboardUI.icons.load("Exit"));
			MenuConfigurationOptionsUI.backButton.positionOffset_Y = -50;
			MenuConfigurationOptionsUI.backButton.positionScale_Y = 1f;
			MenuConfigurationOptionsUI.backButton.sizeOffset_X = 200;
			MenuConfigurationOptionsUI.backButton.sizeOffset_Y = 50;
			MenuConfigurationOptionsUI.backButton.text = MenuDashboardUI.localization.format("BackButtonText");
			MenuConfigurationOptionsUI.backButton.tooltip = MenuDashboardUI.localization.format("BackButtonTooltip");
			SleekButton sleekButton = MenuConfigurationOptionsUI.backButton;
			if (MenuConfigurationOptionsUI.<>f__mg$cache1D == null)
			{
				MenuConfigurationOptionsUI.<>f__mg$cache1D = new ClickedButton(MenuConfigurationOptionsUI.onClickedBackButton);
			}
			sleekButton.onClickedButton = MenuConfigurationOptionsUI.<>f__mg$cache1D;
			MenuConfigurationOptionsUI.backButton.fontSize = 14;
			MenuConfigurationOptionsUI.backButton.iconImage.backgroundTint = ESleekTint.FOREGROUND;
			MenuConfigurationOptionsUI.container.add(MenuConfigurationOptionsUI.backButton);
			MenuConfigurationOptionsUI.defaultButton = new SleekButton();
			MenuConfigurationOptionsUI.defaultButton.positionOffset_X = -200;
			MenuConfigurationOptionsUI.defaultButton.positionOffset_Y = -50;
			MenuConfigurationOptionsUI.defaultButton.positionScale_X = 1f;
			MenuConfigurationOptionsUI.defaultButton.positionScale_Y = 1f;
			MenuConfigurationOptionsUI.defaultButton.sizeOffset_X = 200;
			MenuConfigurationOptionsUI.defaultButton.sizeOffset_Y = 50;
			MenuConfigurationOptionsUI.defaultButton.text = MenuPlayConfigUI.localization.format("Default");
			MenuConfigurationOptionsUI.defaultButton.tooltip = MenuPlayConfigUI.localization.format("Default_Tooltip");
			SleekButton sleekButton2 = MenuConfigurationOptionsUI.defaultButton;
			if (MenuConfigurationOptionsUI.<>f__mg$cache1E == null)
			{
				MenuConfigurationOptionsUI.<>f__mg$cache1E = new ClickedButton(MenuConfigurationOptionsUI.onClickedDefaultButton);
			}
			sleekButton2.onClickedButton = MenuConfigurationOptionsUI.<>f__mg$cache1E;
			MenuConfigurationOptionsUI.defaultButton.fontSize = 14;
			MenuConfigurationOptionsUI.container.add(MenuConfigurationOptionsUI.defaultButton);
			MenuConfigurationOptionsUI.updateAll();
		}

		public static void open()
		{
			if (MenuConfigurationOptionsUI.active)
			{
				return;
			}
			MenuConfigurationOptionsUI.active = true;
			MenuConfigurationOptionsUI.container.lerpPositionScale(0f, 0f, ESleekLerp.EXPONENTIAL, 20f);
		}

		public static void close()
		{
			if (!MenuConfigurationOptionsUI.active)
			{
				return;
			}
			MenuConfigurationOptionsUI.active = false;
			MenuConfigurationOptionsUI.container.lerpPositionScale(0f, 1f, ESleekLerp.EXPONENTIAL, 20f);
		}

		private static void onDraggedFOVSlider(SleekSlider slider, float state)
		{
			OptionsSettings.fov = state;
			OptionsSettings.apply();
			MenuConfigurationOptionsUI.fovSlider.updateLabel(MenuConfigurationOptionsUI.localization.format("FOV_Slider_Label", new object[]
			{
				(int)OptionsSettings.view
			}));
		}

		private static void onDraggedVolumeSlider(SleekSlider slider, float state)
		{
			OptionsSettings.volume = state;
			OptionsSettings.apply();
			MenuConfigurationOptionsUI.volumeSlider.updateLabel(MenuConfigurationOptionsUI.localization.format("Volume_Slider_Label", new object[]
			{
				(int)(OptionsSettings.volume * 100f)
			}));
		}

		private static void onDraggedVoiceSlider(SleekSlider slider, float state)
		{
			OptionsSettings.voice = state * 4f;
			MenuConfigurationOptionsUI.voiceSlider.updateLabel(MenuConfigurationOptionsUI.localization.format("Voice_Slider_Label", new object[]
			{
				(int)(OptionsSettings.voice * 100f)
			}));
		}

		private static void onToggledDebugToggle(SleekToggle toggle, bool state)
		{
			OptionsSettings.debug = state;
		}

		private static void onToggledMusicToggle(SleekToggle toggle, bool state)
		{
			OptionsSettings.music = state;
			OptionsSettings.apply();
		}

		private static void onToggledTimerToggle(SleekToggle toggle, bool state)
		{
			OptionsSettings.timer = state;
			OptionsSettings.apply();
		}

		private static void onToggledGoreToggle(SleekToggle toggle, bool state)
		{
			OptionsSettings.gore = state;
		}

		private static void onToggledFilterToggle(SleekToggle toggle, bool state)
		{
			OptionsSettings.filter = state;
		}

		private static void onToggledChatTextToggle(SleekToggle toggle, bool state)
		{
			OptionsSettings.chatText = state;
		}

		private static void onToggledChatVoiceInToggle(SleekToggle toggle, bool state)
		{
			OptionsSettings.chatVoiceIn = state;
		}

		private static void onToggledChatVoiceOutToggle(SleekToggle toggle, bool state)
		{
			OptionsSettings.chatVoiceOut = state;
		}

		private static void onToggledHintsToggle(SleekToggle toggle, bool state)
		{
			OptionsSettings.hints = state;
		}

		private static void onToggledAmbienceToggle(SleekToggle toggle, bool state)
		{
			OptionsSettings.ambience = state;
			OptionsSettings.apply();
		}

		private static void onToggledStreamerToggle(SleekToggle toggle, bool state)
		{
			OptionsSettings.streamer = state;
		}

		private static void onToggledFeaturedWorkshopToggle(SleekToggle toggle, bool state)
		{
			OptionsSettings.featuredWorkshop = state;
		}

		private static void onToggledMatchmakingShowAllMapsToggle(SleekToggle toggle, bool state)
		{
			OptionsSettings.matchmakingShowAllMaps = state;
		}

		private static void onTypedMinMatchmakingPlayersField(SleekInt32Field field, int state)
		{
			OptionsSettings.minMatchmakingPlayers = state;
		}

		private static void onTypedMaxMatchmakingPingField(SleekInt32Field field, int state)
		{
			OptionsSettings.maxMatchmakingPing = state;
		}

		private static void onSwappedMetricState(SleekButtonState button, int index)
		{
			OptionsSettings.metric = (index == 1);
		}

		private static void onSwappedTalkState(SleekButtonState button, int index)
		{
			OptionsSettings.talk = (index == 1);
		}

		private static void onSwappedUIState(SleekButtonState button, int index)
		{
			OptionsSettings.proUI = (index == 1);
		}

		private static void onSwappedHitmarkerState(SleekButtonState button, int index)
		{
			OptionsSettings.hitmarker = (index == 1);
		}

		private static void onCrosshairColorPicked(SleekColorPicker picker, Color color)
		{
			OptionsSettings.crosshairColor = color;
			if (PlayerLifeUI.dotImage != null)
			{
				PlayerLifeUI.crosshairLeftImage.backgroundColor = color;
				PlayerLifeUI.crosshairRightImage.backgroundColor = color;
				PlayerLifeUI.crosshairDownImage.backgroundColor = color;
				PlayerLifeUI.crosshairUpImage.backgroundColor = color;
				PlayerLifeUI.dotImage.backgroundColor = color;
			}
		}

		private static void onHitmarkerColorPicked(SleekColorPicker picker, Color color)
		{
			OptionsSettings.hitmarkerColor = color;
			if (PlayerLifeUI.hitmarkers != null)
			{
				for (int i = 0; i < PlayerLifeUI.hitmarkers.Length; i++)
				{
					HitmarkerInfo hitmarkerInfo = PlayerLifeUI.hitmarkers[i];
					hitmarkerInfo.hitEntitiyImage.backgroundColor = color;
					hitmarkerInfo.hitBuildImage.backgroundColor = color;
				}
			}
		}

		private static void onCriticalHitmarkerColorPicked(SleekColorPicker picker, Color color)
		{
			OptionsSettings.criticalHitmarkerColor = color;
			if (PlayerLifeUI.hitmarkers != null)
			{
				for (int i = 0; i < PlayerLifeUI.hitmarkers.Length; i++)
				{
					HitmarkerInfo hitmarkerInfo = PlayerLifeUI.hitmarkers[i];
					hitmarkerInfo.hitCriticalImage.backgroundColor = color;
					hitmarkerInfo.hitCriticalImage.backgroundColor = color;
				}
			}
		}

		private static void onCursorColorPicked(SleekColorPicker picker, Color color)
		{
			OptionsSettings.cursorColor = color;
		}

		private static void onBackgroundColorPicked(SleekColorPicker picker, Color color)
		{
			OptionsSettings.backgroundColor = color;
		}

		private static void onForegroundColorPicked(SleekColorPicker picker, Color color)
		{
			OptionsSettings.foregroundColor = color;
		}

		private static void onFontColorPicked(SleekColorPicker picker, Color color)
		{
			OptionsSettings.fontColor = color;
		}

		private static void onClickedBackButton(SleekButton button)
		{
			if (Player.player != null)
			{
				PlayerPauseUI.open();
			}
			else
			{
				MenuConfigurationUI.open();
			}
			MenuConfigurationOptionsUI.close();
		}

		private static void onClickedDefaultButton(SleekButton button)
		{
			OptionsSettings.restoreDefaults();
			MenuConfigurationOptionsUI.updateAll();
		}

		private static void updateAll()
		{
			MenuConfigurationOptionsUI.fovSlider.state = OptionsSettings.fov;
			MenuConfigurationOptionsUI.fovSlider.updateLabel(MenuConfigurationOptionsUI.localization.format("FOV_Slider_Label", new object[]
			{
				(int)OptionsSettings.view
			}));
			MenuConfigurationOptionsUI.volumeSlider.state = OptionsSettings.volume;
			MenuConfigurationOptionsUI.volumeSlider.updateLabel(MenuConfigurationOptionsUI.localization.format("Volume_Slider_Label", new object[]
			{
				(int)(OptionsSettings.volume * 100f)
			}));
			MenuConfigurationOptionsUI.voiceSlider.state = OptionsSettings.voice / 4f;
			MenuConfigurationOptionsUI.voiceSlider.updateLabel(MenuConfigurationOptionsUI.localization.format("Voice_Slider_Label", new object[]
			{
				(int)(OptionsSettings.voice * 100f)
			}));
			MenuConfigurationOptionsUI.debugToggle.state = OptionsSettings.debug;
			MenuConfigurationOptionsUI.musicToggle.state = OptionsSettings.music;
			MenuConfigurationOptionsUI.timerToggle.state = OptionsSettings.timer;
			MenuConfigurationOptionsUI.goreToggle.state = OptionsSettings.gore;
			MenuConfigurationOptionsUI.filterToggle.state = OptionsSettings.filter;
			MenuConfigurationOptionsUI.chatTextToggle.state = OptionsSettings.chatText;
			MenuConfigurationOptionsUI.chatVoiceInToggle.state = OptionsSettings.chatVoiceIn;
			MenuConfigurationOptionsUI.chatVoiceOutToggle.state = OptionsSettings.chatVoiceOut;
			MenuConfigurationOptionsUI.hintsToggle.state = OptionsSettings.hints;
			MenuConfigurationOptionsUI.ambienceToggle.state = OptionsSettings.ambience;
			MenuConfigurationOptionsUI.streamerToggle.state = OptionsSettings.streamer;
			MenuConfigurationOptionsUI.featuredWorkshopToggle.state = OptionsSettings.featuredWorkshop;
			MenuConfigurationOptionsUI.matchmakingShowAllMapsToggle.state = OptionsSettings.matchmakingShowAllMaps;
			MenuConfigurationOptionsUI.minMatchmakingPlayersField.state = OptionsSettings.minMatchmakingPlayers;
			MenuConfigurationOptionsUI.maxMatchmakingPingField.state = OptionsSettings.maxMatchmakingPing;
			MenuConfigurationOptionsUI.metricButton.state = ((!OptionsSettings.metric) ? 0 : 1);
			MenuConfigurationOptionsUI.talkButton.state = ((!OptionsSettings.talk) ? 0 : 1);
			MenuConfigurationOptionsUI.uiButton.state = ((!OptionsSettings.proUI) ? 0 : 1);
			MenuConfigurationOptionsUI.hitmarkerButton.state = ((!OptionsSettings.hitmarker) ? 0 : 1);
			MenuConfigurationOptionsUI.crosshairColorPicker.state = OptionsSettings.crosshairColor;
			MenuConfigurationOptionsUI.hitmarkerColorPicker.state = OptionsSettings.hitmarkerColor;
			MenuConfigurationOptionsUI.criticalHitmarkerColorPicker.state = OptionsSettings.criticalHitmarkerColor;
			MenuConfigurationOptionsUI.cursorColorPicker.state = OptionsSettings.cursorColor;
			MenuConfigurationOptionsUI.backgroundColorPicker.state = OptionsSettings.backgroundColor;
			MenuConfigurationOptionsUI.foregroundColorPicker.state = OptionsSettings.foregroundColor;
			MenuConfigurationOptionsUI.fontColorPicker.state = OptionsSettings.fontColor;
		}

		private static Local localization;

		private static Sleek container;

		public static bool active;

		private static SleekButtonIcon backButton;

		private static SleekButton defaultButton;

		private static SleekScrollBox optionsBox;

		private static SleekSlider fovSlider;

		private static SleekSlider volumeSlider;

		private static SleekSlider voiceSlider;

		private static SleekToggle debugToggle;

		private static SleekToggle musicToggle;

		private static SleekToggle timerToggle;

		private static SleekToggle goreToggle;

		private static SleekToggle filterToggle;

		private static SleekToggle chatTextToggle;

		private static SleekToggle chatVoiceInToggle;

		private static SleekToggle chatVoiceOutToggle;

		private static SleekToggle hintsToggle;

		private static SleekToggle ambienceToggle;

		private static SleekToggle streamerToggle;

		private static SleekToggle featuredWorkshopToggle;

		private static SleekToggle matchmakingShowAllMapsToggle;

		private static SleekInt32Field minMatchmakingPlayersField;

		private static SleekInt32Field maxMatchmakingPingField;

		private static SleekButtonState metricButton;

		private static SleekButtonState talkButton;

		private static SleekButtonState uiButton;

		private static SleekButtonState hitmarkerButton;

		private static SleekBox crosshairBox;

		private static SleekColorPicker crosshairColorPicker;

		private static SleekBox hitmarkerBox;

		private static SleekColorPicker hitmarkerColorPicker;

		private static SleekBox criticalHitmarkerBox;

		private static SleekColorPicker criticalHitmarkerColorPicker;

		private static SleekBox cursorBox;

		private static SleekColorPicker cursorColorPicker;

		private static SleekBox backgroundBox;

		private static SleekColorPicker backgroundColorPicker;

		private static SleekBox foregroundBox;

		private static SleekColorPicker foregroundColorPicker;

		private static SleekBox fontBox;

		private static SleekColorPicker fontColorPicker;

		[CompilerGenerated]
		private static Dragged <>f__mg$cache0;

		[CompilerGenerated]
		private static Dragged <>f__mg$cache1;

		[CompilerGenerated]
		private static Dragged <>f__mg$cache2;

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
		private static Toggled <>f__mg$cache9;

		[CompilerGenerated]
		private static Toggled <>f__mg$cacheA;

		[CompilerGenerated]
		private static Toggled <>f__mg$cacheB;

		[CompilerGenerated]
		private static Toggled <>f__mg$cacheC;

		[CompilerGenerated]
		private static Toggled <>f__mg$cacheD;

		[CompilerGenerated]
		private static Toggled <>f__mg$cacheE;

		[CompilerGenerated]
		private static Toggled <>f__mg$cacheF;

		[CompilerGenerated]
		private static TypedInt32 <>f__mg$cache10;

		[CompilerGenerated]
		private static TypedInt32 <>f__mg$cache11;

		[CompilerGenerated]
		private static SwappedState <>f__mg$cache12;

		[CompilerGenerated]
		private static SwappedState <>f__mg$cache13;

		[CompilerGenerated]
		private static SwappedState <>f__mg$cache14;

		[CompilerGenerated]
		private static SwappedState <>f__mg$cache15;

		[CompilerGenerated]
		private static ColorPicked <>f__mg$cache16;

		[CompilerGenerated]
		private static ColorPicked <>f__mg$cache17;

		[CompilerGenerated]
		private static ColorPicked <>f__mg$cache18;

		[CompilerGenerated]
		private static ColorPicked <>f__mg$cache19;

		[CompilerGenerated]
		private static ColorPicked <>f__mg$cache1A;

		[CompilerGenerated]
		private static ColorPicked <>f__mg$cache1B;

		[CompilerGenerated]
		private static ColorPicked <>f__mg$cache1C;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache1D;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache1E;
	}
}
