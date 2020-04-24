using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace SDG.Unturned
{
	public class EditorTerrainUI
	{
		public EditorTerrainUI()
		{
			Local local = Localization.read("/Editor/EditorTerrain.dat");
			Bundle bundle = Bundles.getBundle("/Bundles/Textures/Edit/Icons/EditorTerrain/EditorTerrain.unity3d");
			EditorTerrainUI.container = new Sleek();
			EditorTerrainUI.container.positionOffset_X = 10;
			EditorTerrainUI.container.positionOffset_Y = 10;
			EditorTerrainUI.container.positionScale_X = 1f;
			EditorTerrainUI.container.sizeOffset_X = -20;
			EditorTerrainUI.container.sizeOffset_Y = -20;
			EditorTerrainUI.container.sizeScale_X = 1f;
			EditorTerrainUI.container.sizeScale_Y = 1f;
			EditorUI.window.add(EditorTerrainUI.container);
			EditorTerrainUI.active = false;
			EditorTerrainUI.heightButton = new SleekButtonIcon((Texture2D)bundle.load("Height"));
			EditorTerrainUI.heightButton.positionOffset_Y = 40;
			EditorTerrainUI.heightButton.sizeOffset_X = -5;
			EditorTerrainUI.heightButton.sizeOffset_Y = 30;
			EditorTerrainUI.heightButton.sizeScale_X = 0.25f;
			EditorTerrainUI.heightButton.text = local.format("HeightButtonText");
			EditorTerrainUI.heightButton.tooltip = local.format("HeightButtonTooltip");
			SleekButton sleekButton = EditorTerrainUI.heightButton;
			if (EditorTerrainUI.<>f__mg$cache0 == null)
			{
				EditorTerrainUI.<>f__mg$cache0 = new ClickedButton(EditorTerrainUI.onClickedHeightButton);
			}
			sleekButton.onClickedButton = EditorTerrainUI.<>f__mg$cache0;
			EditorTerrainUI.container.add(EditorTerrainUI.heightButton);
			EditorTerrainUI.materialsButton = new SleekButtonIcon((Texture2D)bundle.load("Materials"));
			EditorTerrainUI.materialsButton.positionOffset_X = 5;
			EditorTerrainUI.materialsButton.positionOffset_Y = 40;
			EditorTerrainUI.materialsButton.positionScale_X = 0.25f;
			EditorTerrainUI.materialsButton.sizeOffset_X = -10;
			EditorTerrainUI.materialsButton.sizeOffset_Y = 30;
			EditorTerrainUI.materialsButton.sizeScale_X = 0.25f;
			EditorTerrainUI.materialsButton.text = local.format("MaterialsButtonText");
			EditorTerrainUI.materialsButton.tooltip = local.format("MaterialsButtonTooltip");
			SleekButton sleekButton2 = EditorTerrainUI.materialsButton;
			if (EditorTerrainUI.<>f__mg$cache1 == null)
			{
				EditorTerrainUI.<>f__mg$cache1 = new ClickedButton(EditorTerrainUI.onClickedMaterialsButton);
			}
			sleekButton2.onClickedButton = EditorTerrainUI.<>f__mg$cache1;
			EditorTerrainUI.container.add(EditorTerrainUI.materialsButton);
			EditorTerrainUI.detailsButton = new SleekButtonIcon((Texture2D)bundle.load("Details"));
			EditorTerrainUI.detailsButton.positionOffset_X = 5;
			EditorTerrainUI.detailsButton.positionOffset_Y = 40;
			EditorTerrainUI.detailsButton.positionScale_X = 0.5f;
			EditorTerrainUI.detailsButton.sizeOffset_X = -10;
			EditorTerrainUI.detailsButton.sizeOffset_Y = 30;
			EditorTerrainUI.detailsButton.sizeScale_X = 0.25f;
			EditorTerrainUI.detailsButton.text = local.format("DetailsButtonText");
			EditorTerrainUI.detailsButton.tooltip = local.format("DetailsButtonTooltip");
			SleekButton sleekButton3 = EditorTerrainUI.detailsButton;
			if (EditorTerrainUI.<>f__mg$cache2 == null)
			{
				EditorTerrainUI.<>f__mg$cache2 = new ClickedButton(EditorTerrainUI.onClickedDetailsButton);
			}
			sleekButton3.onClickedButton = EditorTerrainUI.<>f__mg$cache2;
			EditorTerrainUI.container.add(EditorTerrainUI.detailsButton);
			EditorTerrainUI.resourcesButton = new SleekButtonIcon((Texture2D)bundle.load("Resources"));
			EditorTerrainUI.resourcesButton.positionOffset_X = 5;
			EditorTerrainUI.resourcesButton.positionOffset_Y = 40;
			EditorTerrainUI.resourcesButton.positionScale_X = 0.75f;
			EditorTerrainUI.resourcesButton.sizeOffset_X = -5;
			EditorTerrainUI.resourcesButton.sizeOffset_Y = 30;
			EditorTerrainUI.resourcesButton.sizeScale_X = 0.25f;
			EditorTerrainUI.resourcesButton.text = local.format("ResourcesButtonText");
			EditorTerrainUI.resourcesButton.tooltip = local.format("ResourcesButtonTooltip");
			SleekButton sleekButton4 = EditorTerrainUI.resourcesButton;
			if (EditorTerrainUI.<>f__mg$cache3 == null)
			{
				EditorTerrainUI.<>f__mg$cache3 = new ClickedButton(EditorTerrainUI.onClickedResourcesButton);
			}
			sleekButton4.onClickedButton = EditorTerrainUI.<>f__mg$cache3;
			EditorTerrainUI.container.add(EditorTerrainUI.resourcesButton);
			bundle.unload();
			new EditorTerrainHeightUI();
			new EditorTerrainMaterialsUI();
			new EditorTerrainDetailsUI();
			new EditorTerrainResourcesUI();
		}

		public static void open()
		{
			if (EditorTerrainUI.active)
			{
				return;
			}
			EditorTerrainUI.active = true;
			EditorTerrainUI.container.lerpPositionScale(0f, 0f, ESleekLerp.EXPONENTIAL, 20f);
		}

		public static void close()
		{
			if (!EditorTerrainUI.active)
			{
				return;
			}
			EditorTerrainUI.active = false;
			EditorTerrainHeightUI.close();
			EditorTerrainMaterialsUI.close();
			EditorTerrainDetailsUI.close();
			EditorTerrainResourcesUI.close();
			EditorTerrainUI.container.lerpPositionScale(1f, 0f, ESleekLerp.EXPONENTIAL, 20f);
		}

		private static void onClickedHeightButton(SleekButton button)
		{
			EditorTerrainMaterialsUI.close();
			EditorTerrainDetailsUI.close();
			EditorTerrainResourcesUI.close();
			EditorTerrainHeightUI.open();
		}

		private static void onClickedMaterialsButton(SleekButton button)
		{
			EditorTerrainHeightUI.close();
			EditorTerrainDetailsUI.close();
			EditorTerrainResourcesUI.close();
			EditorTerrainMaterialsUI.open();
		}

		private static void onClickedDetailsButton(SleekButton button)
		{
			EditorTerrainHeightUI.close();
			EditorTerrainMaterialsUI.close();
			EditorTerrainResourcesUI.close();
			EditorTerrainDetailsUI.open();
		}

		private static void onClickedResourcesButton(SleekButton button)
		{
			EditorTerrainHeightUI.close();
			EditorTerrainMaterialsUI.close();
			EditorTerrainDetailsUI.close();
			EditorTerrainResourcesUI.open();
		}

		private static Sleek container;

		public static bool active;

		private static SleekButtonIcon heightButton;

		private static SleekButtonIcon materialsButton;

		private static SleekButtonIcon detailsButton;

		private static SleekButtonIcon resourcesButton;

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
