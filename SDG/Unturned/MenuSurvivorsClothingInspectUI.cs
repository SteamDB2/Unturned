using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace SDG.Unturned
{
	public class MenuSurvivorsClothingInspectUI
	{
		public MenuSurvivorsClothingInspectUI()
		{
			MenuSurvivorsClothingInspectUI.container = new Sleek();
			MenuSurvivorsClothingInspectUI.container.positionOffset_X = 10;
			MenuSurvivorsClothingInspectUI.container.positionOffset_Y = 10;
			MenuSurvivorsClothingInspectUI.container.positionScale_Y = 1f;
			MenuSurvivorsClothingInspectUI.container.sizeOffset_X = -20;
			MenuSurvivorsClothingInspectUI.container.sizeOffset_Y = -20;
			MenuSurvivorsClothingInspectUI.container.sizeScale_X = 1f;
			MenuSurvivorsClothingInspectUI.container.sizeScale_Y = 1f;
			MenuUI.container.add(MenuSurvivorsClothingInspectUI.container);
			MenuSurvivorsClothingInspectUI.active = false;
			MenuSurvivorsClothingInspectUI.inventory = new Sleek();
			MenuSurvivorsClothingInspectUI.inventory.positionScale_X = 0.5f;
			MenuSurvivorsClothingInspectUI.inventory.positionOffset_Y = 10;
			MenuSurvivorsClothingInspectUI.inventory.sizeScale_X = 0.5f;
			MenuSurvivorsClothingInspectUI.inventory.sizeScale_Y = 1f;
			MenuSurvivorsClothingInspectUI.inventory.sizeOffset_Y = -20;
			MenuSurvivorsClothingInspectUI.inventory.constraint = ESleekConstraint.XY;
			MenuSurvivorsClothingInspectUI.container.add(MenuSurvivorsClothingInspectUI.inventory);
			MenuSurvivorsClothingInspectUI.image = new SleekInspect("RenderTextures/Item");
			MenuSurvivorsClothingInspectUI.image.positionScale_Y = 0.125f;
			MenuSurvivorsClothingInspectUI.image.sizeScale_X = 1f;
			MenuSurvivorsClothingInspectUI.image.sizeScale_Y = 0.75f;
			MenuSurvivorsClothingInspectUI.image.constraint = ESleekConstraint.XY;
			MenuSurvivorsClothingInspectUI.inventory.add(MenuSurvivorsClothingInspectUI.image);
			MenuSurvivorsClothingInspectUI.slider = new SleekSlider();
			MenuSurvivorsClothingInspectUI.slider.positionOffset_Y = 10;
			MenuSurvivorsClothingInspectUI.slider.positionScale_Y = 1f;
			MenuSurvivorsClothingInspectUI.slider.sizeOffset_Y = 20;
			MenuSurvivorsClothingInspectUI.slider.sizeScale_X = 1f;
			MenuSurvivorsClothingInspectUI.slider.orientation = ESleekOrientation.HORIZONTAL;
			SleekSlider sleekSlider = MenuSurvivorsClothingInspectUI.slider;
			if (MenuSurvivorsClothingInspectUI.<>f__mg$cache0 == null)
			{
				MenuSurvivorsClothingInspectUI.<>f__mg$cache0 = new Dragged(MenuSurvivorsClothingInspectUI.onDraggedSlider);
			}
			sleekSlider.onDragged = MenuSurvivorsClothingInspectUI.<>f__mg$cache0;
			MenuSurvivorsClothingInspectUI.image.add(MenuSurvivorsClothingInspectUI.slider);
			MenuSurvivorsClothingInspectUI.inspect = GameObject.Find("Inspect").transform;
			MenuSurvivorsClothingInspectUI.look = MenuSurvivorsClothingInspectUI.inspect.GetComponent<ItemLook>();
			MenuSurvivorsClothingInspectUI.camera = MenuSurvivorsClothingInspectUI.look.inspectCamera;
			MenuSurvivorsClothingInspectUI.backButton = new SleekButtonIcon((Texture2D)MenuDashboardUI.icons.load("Exit"));
			MenuSurvivorsClothingInspectUI.backButton.positionOffset_Y = -50;
			MenuSurvivorsClothingInspectUI.backButton.positionScale_Y = 1f;
			MenuSurvivorsClothingInspectUI.backButton.sizeOffset_X = 200;
			MenuSurvivorsClothingInspectUI.backButton.sizeOffset_Y = 50;
			MenuSurvivorsClothingInspectUI.backButton.text = MenuDashboardUI.localization.format("BackButtonText");
			MenuSurvivorsClothingInspectUI.backButton.tooltip = MenuDashboardUI.localization.format("BackButtonTooltip");
			SleekButton sleekButton = MenuSurvivorsClothingInspectUI.backButton;
			if (MenuSurvivorsClothingInspectUI.<>f__mg$cache1 == null)
			{
				MenuSurvivorsClothingInspectUI.<>f__mg$cache1 = new ClickedButton(MenuSurvivorsClothingInspectUI.onClickedBackButton);
			}
			sleekButton.onClickedButton = MenuSurvivorsClothingInspectUI.<>f__mg$cache1;
			MenuSurvivorsClothingInspectUI.backButton.fontSize = 14;
			MenuSurvivorsClothingInspectUI.backButton.iconImage.backgroundTint = ESleekTint.FOREGROUND;
			MenuSurvivorsClothingInspectUI.container.add(MenuSurvivorsClothingInspectUI.backButton);
		}

		public static void open()
		{
			if (MenuSurvivorsClothingInspectUI.active)
			{
				return;
			}
			MenuSurvivorsClothingInspectUI.active = true;
			MenuSurvivorsClothingInspectUI.camera.gameObject.SetActive(true);
			MenuSurvivorsClothingInspectUI.look._yaw = 0f;
			MenuSurvivorsClothingInspectUI.look.yaw = 0f;
			MenuSurvivorsClothingInspectUI.slider.state = 0f;
			MenuSurvivorsClothingInspectUI.container.lerpPositionScale(0f, 0f, ESleekLerp.EXPONENTIAL, 20f);
		}

		public static void close()
		{
			if (!MenuSurvivorsClothingInspectUI.active)
			{
				return;
			}
			MenuSurvivorsClothingInspectUI.active = false;
			MenuSurvivorsClothingInspectUI.camera.gameObject.SetActive(false);
			MenuSurvivorsClothingInspectUI.container.lerpPositionScale(0f, 1f, ESleekLerp.EXPONENTIAL, 20f);
		}

		public static void viewItem(int newItem, ulong newInstance)
		{
			MenuSurvivorsClothingInspectUI.item = newItem;
			if (MenuSurvivorsClothingInspectUI.model != null)
			{
				Object.Destroy(MenuSurvivorsClothingInspectUI.model.gameObject);
			}
			ushort inventoryItemID = Provider.provider.economyService.getInventoryItemID(MenuSurvivorsClothingInspectUI.item);
			ushort inventorySkinID = Provider.provider.economyService.getInventorySkinID(MenuSurvivorsClothingInspectUI.item);
			ushort inventoryMythicID = Provider.provider.economyService.getInventoryMythicID(MenuSurvivorsClothingInspectUI.item);
			ItemAsset itemAsset = (ItemAsset)Assets.find(EAssetType.ITEM, inventoryItemID);
			if (inventorySkinID != 0)
			{
				SkinAsset skinAsset = (SkinAsset)Assets.find(EAssetType.SKIN, inventorySkinID);
				MenuSurvivorsClothingInspectUI.model = ItemTool.getItem(itemAsset.id, inventorySkinID, 100, itemAsset.getState(), false, itemAsset, skinAsset);
				if (inventoryMythicID != 0)
				{
					ItemTool.applyEffect(MenuSurvivorsClothingInspectUI.model, inventoryMythicID, EEffectType.THIRD);
				}
			}
			else
			{
				MenuSurvivorsClothingInspectUI.model = ItemTool.getItem(itemAsset.id, 0, 100, itemAsset.getState(), false, itemAsset);
				if (inventoryMythicID != 0)
				{
					ItemTool.applyEffect(MenuSurvivorsClothingInspectUI.model, inventoryMythicID, EEffectType.HOOK);
				}
			}
			MenuSurvivorsClothingInspectUI.model.parent = MenuSurvivorsClothingInspectUI.inspect;
			MenuSurvivorsClothingInspectUI.model.localPosition = Vector3.zero;
			if (itemAsset.type == EItemType.MELEE)
			{
				MenuSurvivorsClothingInspectUI.model.localRotation = Quaternion.Euler(0f, -90f, 90f);
			}
			else
			{
				MenuSurvivorsClothingInspectUI.model.localRotation = Quaternion.Euler(-90f, 0f, 0f);
			}
			if (MenuSurvivorsClothingInspectUI.model.GetComponent<Renderer>() != null)
			{
				MenuSurvivorsClothingInspectUI.look.pos = MenuSurvivorsClothingInspectUI.model.GetComponent<Renderer>().bounds.center;
			}
			else if (MenuSurvivorsClothingInspectUI.model.GetComponent<LODGroup>() != null)
			{
				for (int i = 0; i < 4; i++)
				{
					Transform transform = MenuSurvivorsClothingInspectUI.model.FindChild("Model_" + i);
					if (!(transform == null))
					{
						if (transform.GetComponent<Renderer>() != null)
						{
							MenuSurvivorsClothingInspectUI.look.pos = transform.GetComponent<Renderer>().bounds.center;
							break;
						}
					}
				}
			}
			MenuSurvivorsClothingInspectUI.look.pos = MenuSurvivorsClothingInspectUI.model.position + MenuSurvivorsClothingInspectUI.model.rotation * MenuSurvivorsClothingInspectUI.model.GetComponent<BoxCollider>().center;
		}

		private static void onDraggedSlider(SleekSlider slider, float state)
		{
			MenuSurvivorsClothingInspectUI.look.yaw = state * 360f;
		}

		private static void onClickedBackButton(SleekButton button)
		{
			MenuSurvivorsClothingItemUI.open();
			MenuSurvivorsClothingInspectUI.close();
		}

		private static Sleek container;

		public static bool active;

		private static SleekButtonIcon backButton;

		private static Sleek inventory;

		private static SleekInspect image;

		private static SleekSlider slider;

		private static int item;

		private static Transform inspect;

		private static Transform model;

		private static ItemLook look;

		private static Camera camera;

		[CompilerGenerated]
		private static Dragged <>f__mg$cache0;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache1;
	}
}
