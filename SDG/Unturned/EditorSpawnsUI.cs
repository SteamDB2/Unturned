using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace SDG.Unturned
{
	public class EditorSpawnsUI
	{
		public EditorSpawnsUI()
		{
			Local local = Localization.read("/Editor/EditorSpawns.dat");
			Bundle bundle = Bundles.getBundle("/Bundles/Textures/Edit/Icons/EditorSpawns/EditorSpawns.unity3d");
			EditorSpawnsUI.container = new Sleek();
			EditorSpawnsUI.container.positionOffset_X = 10;
			EditorSpawnsUI.container.positionOffset_Y = 10;
			EditorSpawnsUI.container.positionScale_X = 1f;
			EditorSpawnsUI.container.sizeOffset_X = -20;
			EditorSpawnsUI.container.sizeOffset_Y = -20;
			EditorSpawnsUI.container.sizeScale_X = 1f;
			EditorSpawnsUI.container.sizeScale_Y = 1f;
			EditorUI.window.add(EditorSpawnsUI.container);
			EditorSpawnsUI.active = false;
			EditorSpawnsUI.animalsButton = new SleekButtonIcon((Texture2D)bundle.load("Animals"));
			EditorSpawnsUI.animalsButton.positionOffset_Y = 40;
			EditorSpawnsUI.animalsButton.sizeOffset_X = -5;
			EditorSpawnsUI.animalsButton.sizeOffset_Y = 30;
			EditorSpawnsUI.animalsButton.sizeScale_X = 0.25f;
			EditorSpawnsUI.animalsButton.text = local.format("AnimalsButtonText");
			EditorSpawnsUI.animalsButton.tooltip = local.format("AnimalsButtonTooltip");
			SleekButton sleekButton = EditorSpawnsUI.animalsButton;
			if (EditorSpawnsUI.<>f__mg$cache0 == null)
			{
				EditorSpawnsUI.<>f__mg$cache0 = new ClickedButton(EditorSpawnsUI.onClickedAnimalsButton);
			}
			sleekButton.onClickedButton = EditorSpawnsUI.<>f__mg$cache0;
			EditorSpawnsUI.container.add(EditorSpawnsUI.animalsButton);
			EditorSpawnsUI.itemsButton = new SleekButtonIcon((Texture2D)bundle.load("Items"));
			EditorSpawnsUI.itemsButton.positionOffset_X = 5;
			EditorSpawnsUI.itemsButton.positionOffset_Y = 40;
			EditorSpawnsUI.itemsButton.positionScale_X = 0.25f;
			EditorSpawnsUI.itemsButton.sizeOffset_X = -10;
			EditorSpawnsUI.itemsButton.sizeOffset_Y = 30;
			EditorSpawnsUI.itemsButton.sizeScale_X = 0.25f;
			EditorSpawnsUI.itemsButton.text = local.format("ItemsButtonText");
			EditorSpawnsUI.itemsButton.tooltip = local.format("ItemsButtonTooltip");
			SleekButton sleekButton2 = EditorSpawnsUI.itemsButton;
			if (EditorSpawnsUI.<>f__mg$cache1 == null)
			{
				EditorSpawnsUI.<>f__mg$cache1 = new ClickedButton(EditorSpawnsUI.onClickItemsButton);
			}
			sleekButton2.onClickedButton = EditorSpawnsUI.<>f__mg$cache1;
			EditorSpawnsUI.container.add(EditorSpawnsUI.itemsButton);
			EditorSpawnsUI.zombiesButton = new SleekButtonIcon((Texture2D)bundle.load("Zombies"));
			EditorSpawnsUI.zombiesButton.positionOffset_X = 5;
			EditorSpawnsUI.zombiesButton.positionOffset_Y = 40;
			EditorSpawnsUI.zombiesButton.positionScale_X = 0.5f;
			EditorSpawnsUI.zombiesButton.sizeOffset_X = -10;
			EditorSpawnsUI.zombiesButton.sizeOffset_Y = 30;
			EditorSpawnsUI.zombiesButton.sizeScale_X = 0.25f;
			EditorSpawnsUI.zombiesButton.text = local.format("ZombiesButtonText");
			EditorSpawnsUI.zombiesButton.tooltip = local.format("ZombiesButtonTooltip");
			SleekButton sleekButton3 = EditorSpawnsUI.zombiesButton;
			if (EditorSpawnsUI.<>f__mg$cache2 == null)
			{
				EditorSpawnsUI.<>f__mg$cache2 = new ClickedButton(EditorSpawnsUI.onClickedZombiesButton);
			}
			sleekButton3.onClickedButton = EditorSpawnsUI.<>f__mg$cache2;
			EditorSpawnsUI.container.add(EditorSpawnsUI.zombiesButton);
			EditorSpawnsUI.vehiclesButton = new SleekButtonIcon((Texture2D)bundle.load("Vehicles"));
			EditorSpawnsUI.vehiclesButton.positionOffset_X = 5;
			EditorSpawnsUI.vehiclesButton.positionOffset_Y = 40;
			EditorSpawnsUI.vehiclesButton.positionScale_X = 0.75f;
			EditorSpawnsUI.vehiclesButton.sizeOffset_X = -5;
			EditorSpawnsUI.vehiclesButton.sizeOffset_Y = 30;
			EditorSpawnsUI.vehiclesButton.sizeScale_X = 0.25f;
			EditorSpawnsUI.vehiclesButton.text = local.format("VehiclesButtonText");
			EditorSpawnsUI.vehiclesButton.tooltip = local.format("VehiclesButtonTooltip");
			SleekButton sleekButton4 = EditorSpawnsUI.vehiclesButton;
			if (EditorSpawnsUI.<>f__mg$cache3 == null)
			{
				EditorSpawnsUI.<>f__mg$cache3 = new ClickedButton(EditorSpawnsUI.onClickedVehiclesButton);
			}
			sleekButton4.onClickedButton = EditorSpawnsUI.<>f__mg$cache3;
			EditorSpawnsUI.container.add(EditorSpawnsUI.vehiclesButton);
			bundle.unload();
			new EditorSpawnsAnimalsUI();
			new EditorSpawnsItemsUI();
			new EditorSpawnsZombiesUI();
			new EditorSpawnsVehiclesUI();
		}

		public static void open()
		{
			if (EditorSpawnsUI.active)
			{
				return;
			}
			EditorSpawnsUI.active = true;
			EditorSpawnsUI.container.lerpPositionScale(0f, 0f, ESleekLerp.EXPONENTIAL, 20f);
		}

		public static void close()
		{
			if (!EditorSpawnsUI.active)
			{
				return;
			}
			EditorSpawnsUI.active = false;
			EditorSpawnsItemsUI.close();
			EditorSpawnsAnimalsUI.close();
			EditorSpawnsZombiesUI.close();
			EditorSpawnsVehiclesUI.close();
			EditorSpawnsUI.container.lerpPositionScale(1f, 0f, ESleekLerp.EXPONENTIAL, 20f);
		}

		private static void onClickedAnimalsButton(SleekButton button)
		{
			EditorSpawnsItemsUI.close();
			EditorSpawnsZombiesUI.close();
			EditorSpawnsVehiclesUI.close();
			EditorSpawnsAnimalsUI.open();
		}

		private static void onClickItemsButton(SleekButton button)
		{
			EditorSpawnsAnimalsUI.close();
			EditorSpawnsZombiesUI.close();
			EditorSpawnsVehiclesUI.close();
			EditorSpawnsItemsUI.open();
		}

		private static void onClickedZombiesButton(SleekButton button)
		{
			EditorSpawnsAnimalsUI.close();
			EditorSpawnsItemsUI.close();
			EditorSpawnsVehiclesUI.close();
			EditorSpawnsZombiesUI.open();
		}

		private static void onClickedVehiclesButton(SleekButton button)
		{
			EditorSpawnsAnimalsUI.close();
			EditorSpawnsItemsUI.close();
			EditorSpawnsZombiesUI.close();
			EditorSpawnsVehiclesUI.open();
		}

		private static Sleek container;

		public static bool active;

		private static SleekButtonIcon animalsButton;

		private static SleekButtonIcon itemsButton;

		private static SleekButtonIcon zombiesButton;

		private static SleekButtonIcon vehiclesButton;

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
