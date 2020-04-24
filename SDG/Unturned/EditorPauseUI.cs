using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace SDG.Unturned
{
	public class EditorPauseUI
	{
		public EditorPauseUI()
		{
			Local local = Localization.read("/Editor/EditorPause.dat");
			Bundle bundle = Bundles.getBundle("/Bundles/Textures/Edit/Icons/EditorPause/EditorPause.unity3d");
			EditorPauseUI.container = new Sleek();
			EditorPauseUI.container.positionOffset_X = 10;
			EditorPauseUI.container.positionOffset_Y = 10;
			EditorPauseUI.container.positionScale_X = 1f;
			EditorPauseUI.container.sizeOffset_X = -20;
			EditorPauseUI.container.sizeOffset_Y = -20;
			EditorPauseUI.container.sizeScale_X = 1f;
			EditorPauseUI.container.sizeScale_Y = 1f;
			EditorUI.window.add(EditorPauseUI.container);
			EditorPauseUI.active = false;
			EditorPauseUI.saveButton = new SleekButtonIcon((Texture2D)bundle.load("Save"));
			EditorPauseUI.saveButton.positionOffset_X = -100;
			EditorPauseUI.saveButton.positionOffset_Y = -115;
			EditorPauseUI.saveButton.positionScale_X = 0.5f;
			EditorPauseUI.saveButton.positionScale_Y = 0.5f;
			EditorPauseUI.saveButton.sizeOffset_X = 200;
			EditorPauseUI.saveButton.sizeOffset_Y = 30;
			EditorPauseUI.saveButton.text = local.format("Save_Button");
			EditorPauseUI.saveButton.tooltip = local.format("Save_Button_Tooltip");
			SleekButton sleekButton = EditorPauseUI.saveButton;
			if (EditorPauseUI.<>f__mg$cache0 == null)
			{
				EditorPauseUI.<>f__mg$cache0 = new ClickedButton(EditorPauseUI.onClickedSaveButton);
			}
			sleekButton.onClickedButton = EditorPauseUI.<>f__mg$cache0;
			EditorPauseUI.container.add(EditorPauseUI.saveButton);
			EditorPauseUI.mapButton = new SleekButtonIcon((Texture2D)bundle.load("Map"));
			EditorPauseUI.mapButton.positionOffset_X = -100;
			EditorPauseUI.mapButton.positionOffset_Y = -75;
			EditorPauseUI.mapButton.positionScale_X = 0.5f;
			EditorPauseUI.mapButton.positionScale_Y = 0.5f;
			EditorPauseUI.mapButton.sizeOffset_X = 200;
			EditorPauseUI.mapButton.sizeOffset_Y = 30;
			EditorPauseUI.mapButton.text = local.format("Map_Button");
			EditorPauseUI.mapButton.tooltip = local.format("Map_Button_Tooltip");
			SleekButton sleekButton2 = EditorPauseUI.mapButton;
			if (EditorPauseUI.<>f__mg$cache1 == null)
			{
				EditorPauseUI.<>f__mg$cache1 = new ClickedButton(EditorPauseUI.onClickedMapButton);
			}
			sleekButton2.onClickedButton = EditorPauseUI.<>f__mg$cache1;
			EditorPauseUI.container.add(EditorPauseUI.mapButton);
			EditorPauseUI.chartButton = new SleekButtonIcon((Texture2D)bundle.load("Chart"));
			EditorPauseUI.chartButton.positionOffset_X = -100;
			EditorPauseUI.chartButton.positionOffset_Y = -35;
			EditorPauseUI.chartButton.positionScale_X = 0.5f;
			EditorPauseUI.chartButton.positionScale_Y = 0.5f;
			EditorPauseUI.chartButton.sizeOffset_X = 200;
			EditorPauseUI.chartButton.sizeOffset_Y = 30;
			EditorPauseUI.chartButton.text = local.format("Chart_Button");
			EditorPauseUI.chartButton.tooltip = local.format("Chart_Button_Tooltip");
			SleekButton sleekButton3 = EditorPauseUI.chartButton;
			if (EditorPauseUI.<>f__mg$cache2 == null)
			{
				EditorPauseUI.<>f__mg$cache2 = new ClickedButton(EditorPauseUI.onClickedChartButton);
			}
			sleekButton3.onClickedButton = EditorPauseUI.<>f__mg$cache2;
			EditorPauseUI.container.add(EditorPauseUI.chartButton);
			EditorPauseUI.legacyIDField = new SleekUInt16Field();
			EditorPauseUI.legacyIDField.positionOffset_X = -100;
			EditorPauseUI.legacyIDField.positionOffset_Y = 5;
			EditorPauseUI.legacyIDField.positionScale_X = 0.5f;
			EditorPauseUI.legacyIDField.positionScale_Y = 0.5f;
			EditorPauseUI.legacyIDField.sizeOffset_X = 50;
			EditorPauseUI.legacyIDField.sizeOffset_Y = 30;
			EditorPauseUI.container.add(EditorPauseUI.legacyIDField);
			EditorPauseUI.legacyButton = new SleekButton();
			EditorPauseUI.legacyButton.positionOffset_X = -40;
			EditorPauseUI.legacyButton.positionOffset_Y = 5;
			EditorPauseUI.legacyButton.positionScale_X = 0.5f;
			EditorPauseUI.legacyButton.positionScale_Y = 0.5f;
			EditorPauseUI.legacyButton.sizeOffset_X = 140;
			EditorPauseUI.legacyButton.sizeOffset_Y = 30;
			EditorPauseUI.legacyButton.text = local.format("Legacy_Spawns");
			EditorPauseUI.legacyButton.tooltip = local.format("Legacy_Spawns_Tooltip");
			SleekButton sleekButton4 = EditorPauseUI.legacyButton;
			if (EditorPauseUI.<>f__mg$cache3 == null)
			{
				EditorPauseUI.<>f__mg$cache3 = new ClickedButton(EditorPauseUI.onClickedLegacyButton);
			}
			sleekButton4.onClickedButton = EditorPauseUI.<>f__mg$cache3;
			EditorPauseUI.container.add(EditorPauseUI.legacyButton);
			EditorPauseUI.proxyIDField = new SleekUInt16Field();
			EditorPauseUI.proxyIDField.positionOffset_X = -100;
			EditorPauseUI.proxyIDField.positionOffset_Y = 45;
			EditorPauseUI.proxyIDField.positionScale_X = 0.5f;
			EditorPauseUI.proxyIDField.positionScale_Y = 0.5f;
			EditorPauseUI.proxyIDField.sizeOffset_X = 50;
			EditorPauseUI.proxyIDField.sizeOffset_Y = 30;
			EditorPauseUI.container.add(EditorPauseUI.proxyIDField);
			EditorPauseUI.proxyButton = new SleekButton();
			EditorPauseUI.proxyButton.positionOffset_X = -40;
			EditorPauseUI.proxyButton.positionOffset_Y = 45;
			EditorPauseUI.proxyButton.positionScale_X = 0.5f;
			EditorPauseUI.proxyButton.positionScale_Y = 0.5f;
			EditorPauseUI.proxyButton.sizeOffset_X = 140;
			EditorPauseUI.proxyButton.sizeOffset_Y = 30;
			EditorPauseUI.proxyButton.text = local.format("Proxy_Spawns");
			EditorPauseUI.proxyButton.tooltip = local.format("Proxy_Spawns_Tooltip");
			SleekButton sleekButton5 = EditorPauseUI.proxyButton;
			if (EditorPauseUI.<>f__mg$cache4 == null)
			{
				EditorPauseUI.<>f__mg$cache4 = new ClickedButton(EditorPauseUI.onClickedProxyButton);
			}
			sleekButton5.onClickedButton = EditorPauseUI.<>f__mg$cache4;
			EditorPauseUI.container.add(EditorPauseUI.proxyButton);
			EditorPauseUI.exitButton = new SleekButtonIcon((Texture2D)bundle.load("Exit"));
			EditorPauseUI.exitButton.positionOffset_X = -100;
			EditorPauseUI.exitButton.positionOffset_Y = 85;
			EditorPauseUI.exitButton.positionScale_X = 0.5f;
			EditorPauseUI.exitButton.positionScale_Y = 0.5f;
			EditorPauseUI.exitButton.sizeOffset_X = 200;
			EditorPauseUI.exitButton.sizeOffset_Y = 30;
			EditorPauseUI.exitButton.text = local.format("Exit_Button");
			EditorPauseUI.exitButton.tooltip = local.format("Exit_Button_Tooltip");
			SleekButton sleekButton6 = EditorPauseUI.exitButton;
			if (EditorPauseUI.<>f__mg$cache5 == null)
			{
				EditorPauseUI.<>f__mg$cache5 = new ClickedButton(EditorPauseUI.onClickedExitButton);
			}
			sleekButton6.onClickedButton = EditorPauseUI.<>f__mg$cache5;
			EditorPauseUI.container.add(EditorPauseUI.exitButton);
			bundle.unload();
		}

		public static void open()
		{
			if (EditorPauseUI.active)
			{
				return;
			}
			EditorPauseUI.active = true;
			EditorPauseUI.container.lerpPositionScale(0f, 0f, ESleekLerp.EXPONENTIAL, 20f);
		}

		public static void close()
		{
			if (!EditorPauseUI.active)
			{
				return;
			}
			EditorPauseUI.active = false;
			EditorPauseUI.container.lerpPositionScale(1f, 0f, ESleekLerp.EXPONENTIAL, 20f);
		}

		private static void onClickedSaveButton(SleekButton button)
		{
			Level.save();
		}

		private static void onClickedMapButton(SleekButton button)
		{
			Level.mapify();
		}

		private static void onClickedChartButton(SleekButton button)
		{
			Level.chartify();
		}

		private static void onClickedLegacyButton(SleekButton button)
		{
			ushort state = EditorPauseUI.legacyIDField.state;
			if (state == 0)
			{
				return;
			}
			SpawnTableTool.export(state, true);
		}

		private static void onClickedProxyButton(SleekButton button)
		{
			ushort state = EditorPauseUI.proxyIDField.state;
			if (state == 0)
			{
				return;
			}
			SpawnTableTool.export(state, false);
		}

		private static void onClickedExitButton(SleekButton button)
		{
			Level.exit();
		}

		private static Sleek container;

		public static bool active;

		private static SleekButtonIcon saveButton;

		private static SleekButtonIcon mapButton;

		private static SleekButtonIcon exitButton;

		private static SleekUInt16Field legacyIDField;

		private static SleekButton legacyButton;

		private static SleekUInt16Field proxyIDField;

		private static SleekButton proxyButton;

		private static SleekButtonIcon chartButton;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache0;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache1;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache2;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache3;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache4;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache5;
	}
}
