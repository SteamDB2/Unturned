using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace SDG.Unturned
{
	public class EditorEnvironmentUI
	{
		public EditorEnvironmentUI()
		{
			Local local = Localization.read("/Editor/EditorEnvironment.dat");
			Bundle bundle = Bundles.getBundle("/Bundles/Textures/Edit/Icons/EditorEnvironment/EditorEnvironment.unity3d");
			EditorEnvironmentUI.container = new Sleek();
			EditorEnvironmentUI.container.positionOffset_X = 10;
			EditorEnvironmentUI.container.positionOffset_Y = 10;
			EditorEnvironmentUI.container.positionScale_X = 1f;
			EditorEnvironmentUI.container.sizeOffset_X = -20;
			EditorEnvironmentUI.container.sizeOffset_Y = -20;
			EditorEnvironmentUI.container.sizeScale_X = 1f;
			EditorEnvironmentUI.container.sizeScale_Y = 1f;
			EditorUI.window.add(EditorEnvironmentUI.container);
			EditorEnvironmentUI.active = false;
			EditorEnvironmentUI.lightingButton = new SleekButtonIcon((Texture2D)bundle.load("Lighting"));
			EditorEnvironmentUI.lightingButton.positionOffset_Y = 40;
			EditorEnvironmentUI.lightingButton.sizeOffset_X = -5;
			EditorEnvironmentUI.lightingButton.sizeOffset_Y = 30;
			EditorEnvironmentUI.lightingButton.sizeScale_X = 0.25f;
			EditorEnvironmentUI.lightingButton.text = local.format("LightingButtonText");
			EditorEnvironmentUI.lightingButton.tooltip = local.format("LightingButtonTooltip");
			SleekButton sleekButton = EditorEnvironmentUI.lightingButton;
			if (EditorEnvironmentUI.<>f__mg$cache0 == null)
			{
				EditorEnvironmentUI.<>f__mg$cache0 = new ClickedButton(EditorEnvironmentUI.onClickedLightingButton);
			}
			sleekButton.onClickedButton = EditorEnvironmentUI.<>f__mg$cache0;
			EditorEnvironmentUI.container.add(EditorEnvironmentUI.lightingButton);
			EditorEnvironmentUI.roadsButton = new SleekButtonIcon((Texture2D)bundle.load("Roads"));
			EditorEnvironmentUI.roadsButton.positionOffset_X = 5;
			EditorEnvironmentUI.roadsButton.positionOffset_Y = 40;
			EditorEnvironmentUI.roadsButton.positionScale_X = 0.25f;
			EditorEnvironmentUI.roadsButton.sizeOffset_X = -10;
			EditorEnvironmentUI.roadsButton.sizeOffset_Y = 30;
			EditorEnvironmentUI.roadsButton.sizeScale_X = 0.25f;
			EditorEnvironmentUI.roadsButton.text = local.format("RoadsButtonText");
			EditorEnvironmentUI.roadsButton.tooltip = local.format("RoadsButtonTooltip");
			SleekButton sleekButton2 = EditorEnvironmentUI.roadsButton;
			if (EditorEnvironmentUI.<>f__mg$cache1 == null)
			{
				EditorEnvironmentUI.<>f__mg$cache1 = new ClickedButton(EditorEnvironmentUI.onClickedRoadsButton);
			}
			sleekButton2.onClickedButton = EditorEnvironmentUI.<>f__mg$cache1;
			EditorEnvironmentUI.container.add(EditorEnvironmentUI.roadsButton);
			EditorEnvironmentUI.navigationButton = new SleekButtonIcon((Texture2D)bundle.load("Navigation"));
			EditorEnvironmentUI.navigationButton.positionOffset_X = 5;
			EditorEnvironmentUI.navigationButton.positionOffset_Y = 40;
			EditorEnvironmentUI.navigationButton.positionScale_X = 0.5f;
			EditorEnvironmentUI.navigationButton.sizeOffset_X = -10;
			EditorEnvironmentUI.navigationButton.sizeOffset_Y = 30;
			EditorEnvironmentUI.navigationButton.sizeScale_X = 0.25f;
			EditorEnvironmentUI.navigationButton.text = local.format("NavigationButtonText");
			EditorEnvironmentUI.navigationButton.tooltip = local.format("NavigationButtonTooltip");
			SleekButton sleekButton3 = EditorEnvironmentUI.navigationButton;
			if (EditorEnvironmentUI.<>f__mg$cache2 == null)
			{
				EditorEnvironmentUI.<>f__mg$cache2 = new ClickedButton(EditorEnvironmentUI.onClickedNavigationButton);
			}
			sleekButton3.onClickedButton = EditorEnvironmentUI.<>f__mg$cache2;
			EditorEnvironmentUI.container.add(EditorEnvironmentUI.navigationButton);
			EditorEnvironmentUI.nodesButton = new SleekButtonIcon((Texture2D)bundle.load("Nodes"));
			EditorEnvironmentUI.nodesButton.positionOffset_X = 5;
			EditorEnvironmentUI.nodesButton.positionOffset_Y = 40;
			EditorEnvironmentUI.nodesButton.positionScale_X = 0.75f;
			EditorEnvironmentUI.nodesButton.sizeOffset_X = -5;
			EditorEnvironmentUI.nodesButton.sizeOffset_Y = 30;
			EditorEnvironmentUI.nodesButton.sizeScale_X = 0.25f;
			EditorEnvironmentUI.nodesButton.text = local.format("NodesButtonText");
			EditorEnvironmentUI.nodesButton.tooltip = local.format("NodesButtonTooltip");
			SleekButton sleekButton4 = EditorEnvironmentUI.nodesButton;
			if (EditorEnvironmentUI.<>f__mg$cache3 == null)
			{
				EditorEnvironmentUI.<>f__mg$cache3 = new ClickedButton(EditorEnvironmentUI.onClickedNodesButton);
			}
			sleekButton4.onClickedButton = EditorEnvironmentUI.<>f__mg$cache3;
			EditorEnvironmentUI.container.add(EditorEnvironmentUI.nodesButton);
			bundle.unload();
			new EditorEnvironmentLightingUI();
			new EditorEnvironmentRoadsUI();
			new EditorEnvironmentNavigationUI();
			new EditorEnvironmentNodesUI();
		}

		public static void open()
		{
			if (EditorEnvironmentUI.active)
			{
				return;
			}
			EditorEnvironmentUI.active = true;
			EditorEnvironmentUI.container.lerpPositionScale(0f, 0f, ESleekLerp.EXPONENTIAL, 20f);
		}

		public static void close()
		{
			if (!EditorEnvironmentUI.active)
			{
				return;
			}
			EditorEnvironmentUI.active = false;
			EditorEnvironmentLightingUI.close();
			EditorEnvironmentRoadsUI.close();
			EditorEnvironmentNavigationUI.close();
			EditorEnvironmentNodesUI.close();
			EditorEnvironmentUI.container.lerpPositionScale(1f, 0f, ESleekLerp.EXPONENTIAL, 20f);
		}

		private static void onClickedLightingButton(SleekButton button)
		{
			EditorEnvironmentRoadsUI.close();
			EditorEnvironmentNavigationUI.close();
			EditorEnvironmentNodesUI.close();
			EditorEnvironmentLightingUI.open();
		}

		private static void onClickedRoadsButton(SleekButton button)
		{
			EditorEnvironmentLightingUI.close();
			EditorEnvironmentNavigationUI.close();
			EditorEnvironmentNodesUI.close();
			EditorEnvironmentRoadsUI.open();
		}

		private static void onClickedNavigationButton(SleekButton button)
		{
			EditorEnvironmentLightingUI.close();
			EditorEnvironmentRoadsUI.close();
			EditorEnvironmentNodesUI.close();
			EditorEnvironmentNavigationUI.open();
		}

		private static void onClickedNodesButton(SleekButton button)
		{
			EditorEnvironmentLightingUI.close();
			EditorEnvironmentRoadsUI.close();
			EditorEnvironmentNavigationUI.close();
			EditorEnvironmentNodesUI.open();
		}

		private static Sleek container;

		public static bool active;

		private static SleekButtonIcon lightingButton;

		private static SleekButtonIcon roadsButton;

		private static SleekButtonIcon navigationButton;

		private static SleekButtonIcon nodesButton;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache0;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache1;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache2;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache3;
	}
}
