using System;
using System.Runtime.CompilerServices;
using SDG.Provider;
using SDG.SteamworksProvider.Services.Economy;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	public class MenuSurvivorsClothingBoxUI
	{
		public MenuSurvivorsClothingBoxUI()
		{
			MenuSurvivorsClothingBoxUI.localization = Localization.read("/Menu/Survivors/MenuSurvivorsClothingBox.dat");
			if (MenuSurvivorsClothingBoxUI.icons != null)
			{
				MenuSurvivorsClothingBoxUI.icons.unload();
			}
			MenuSurvivorsClothingBoxUI.icons = Bundles.getBundle("/Bundles/Textures/Menu/Icons/Survivors/MenuSurvivorsClothingBox/MenuSurvivorsClothingBox.unity3d");
			MenuSurvivorsClothingBoxUI.container = new Sleek();
			MenuSurvivorsClothingBoxUI.container.positionOffset_X = 10;
			MenuSurvivorsClothingBoxUI.container.positionOffset_Y = 10;
			MenuSurvivorsClothingBoxUI.container.positionScale_Y = 1f;
			MenuSurvivorsClothingBoxUI.container.sizeOffset_X = -20;
			MenuSurvivorsClothingBoxUI.container.sizeOffset_Y = -20;
			MenuSurvivorsClothingBoxUI.container.sizeScale_X = 1f;
			MenuSurvivorsClothingBoxUI.container.sizeScale_Y = 1f;
			MenuUI.container.add(MenuSurvivorsClothingBoxUI.container);
			MenuSurvivorsClothingBoxUI.active = false;
			MenuSurvivorsClothingBoxUI.inventory = new Sleek();
			MenuSurvivorsClothingBoxUI.inventory.positionScale_X = 0.5f;
			MenuSurvivorsClothingBoxUI.inventory.positionOffset_Y = 10;
			MenuSurvivorsClothingBoxUI.inventory.sizeScale_X = 0.5f;
			MenuSurvivorsClothingBoxUI.inventory.sizeScale_Y = 1f;
			MenuSurvivorsClothingBoxUI.inventory.sizeOffset_Y = -20;
			MenuSurvivorsClothingBoxUI.inventory.constraint = ESleekConstraint.XY;
			MenuSurvivorsClothingBoxUI.container.add(MenuSurvivorsClothingBoxUI.inventory);
			MenuSurvivorsClothingBoxUI.finalBox = new SleekBox();
			MenuSurvivorsClothingBoxUI.finalBox.positionOffset_X = -10;
			MenuSurvivorsClothingBoxUI.finalBox.positionOffset_Y = -10;
			MenuSurvivorsClothingBoxUI.finalBox.sizeOffset_X = 20;
			MenuSurvivorsClothingBoxUI.finalBox.sizeOffset_Y = 20;
			MenuSurvivorsClothingBoxUI.inventory.add(MenuSurvivorsClothingBoxUI.finalBox);
			MenuSurvivorsClothingBoxUI.boxButton = new SleekInventory();
			MenuSurvivorsClothingBoxUI.boxButton.positionOffset_Y = -30;
			MenuSurvivorsClothingBoxUI.boxButton.positionScale_X = 0.3f;
			MenuSurvivorsClothingBoxUI.boxButton.positionScale_Y = 0.3f;
			MenuSurvivorsClothingBoxUI.boxButton.sizeScale_X = 0.4f;
			MenuSurvivorsClothingBoxUI.boxButton.sizeScale_Y = 0.4f;
			MenuSurvivorsClothingBoxUI.inventory.add(MenuSurvivorsClothingBoxUI.boxButton);
			MenuSurvivorsClothingBoxUI.keyButton = new SleekButtonIcon(null, 40);
			MenuSurvivorsClothingBoxUI.keyButton.positionOffset_Y = -20;
			MenuSurvivorsClothingBoxUI.keyButton.positionScale_X = 0.3f;
			MenuSurvivorsClothingBoxUI.keyButton.positionScale_Y = 0.7f;
			MenuSurvivorsClothingBoxUI.keyButton.sizeOffset_X = -5;
			MenuSurvivorsClothingBoxUI.keyButton.sizeOffset_Y = 50;
			MenuSurvivorsClothingBoxUI.keyButton.sizeScale_X = 0.2f;
			MenuSurvivorsClothingBoxUI.keyButton.text = MenuSurvivorsClothingBoxUI.localization.format("Key_Text");
			MenuSurvivorsClothingBoxUI.keyButton.tooltip = MenuSurvivorsClothingBoxUI.localization.format("Key_Tooltip");
			SleekButton sleekButton = MenuSurvivorsClothingBoxUI.keyButton;
			if (MenuSurvivorsClothingBoxUI.<>f__mg$cache0 == null)
			{
				MenuSurvivorsClothingBoxUI.<>f__mg$cache0 = new ClickedButton(MenuSurvivorsClothingBoxUI.onClickedKeyButton);
			}
			sleekButton.onClickedButton = MenuSurvivorsClothingBoxUI.<>f__mg$cache0;
			MenuSurvivorsClothingBoxUI.keyButton.fontSize = 14;
			MenuSurvivorsClothingBoxUI.inventory.add(MenuSurvivorsClothingBoxUI.keyButton);
			MenuSurvivorsClothingBoxUI.keyButton.isVisible = false;
			MenuSurvivorsClothingBoxUI.unboxButton = new SleekButtonIcon(null);
			MenuSurvivorsClothingBoxUI.unboxButton.positionOffset_X = 5;
			MenuSurvivorsClothingBoxUI.unboxButton.positionOffset_Y = -20;
			MenuSurvivorsClothingBoxUI.unboxButton.positionScale_X = 0.5f;
			MenuSurvivorsClothingBoxUI.unboxButton.positionScale_Y = 0.7f;
			MenuSurvivorsClothingBoxUI.unboxButton.sizeOffset_X = -5;
			MenuSurvivorsClothingBoxUI.unboxButton.sizeOffset_Y = 50;
			MenuSurvivorsClothingBoxUI.unboxButton.sizeScale_X = 0.2f;
			MenuSurvivorsClothingBoxUI.unboxButton.text = MenuSurvivorsClothingBoxUI.localization.format("Unbox_Text");
			MenuSurvivorsClothingBoxUI.unboxButton.tooltip = MenuSurvivorsClothingBoxUI.localization.format("Unbox_Tooltip");
			SleekButton sleekButton2 = MenuSurvivorsClothingBoxUI.unboxButton;
			if (MenuSurvivorsClothingBoxUI.<>f__mg$cache1 == null)
			{
				MenuSurvivorsClothingBoxUI.<>f__mg$cache1 = new ClickedButton(MenuSurvivorsClothingBoxUI.onClickedUnboxButton);
			}
			sleekButton2.onClickedButton = MenuSurvivorsClothingBoxUI.<>f__mg$cache1;
			MenuSurvivorsClothingBoxUI.unboxButton.fontSize = 14;
			MenuSurvivorsClothingBoxUI.inventory.add(MenuSurvivorsClothingBoxUI.unboxButton);
			MenuSurvivorsClothingBoxUI.unboxButton.isVisible = false;
			if (!MenuSurvivorsClothingBoxUI.hasLoaded)
			{
				TempSteamworksEconomy economyService = Provider.provider.economyService;
				Delegate onInventoryExchanged = economyService.onInventoryExchanged;
				if (MenuSurvivorsClothingBoxUI.<>f__mg$cache2 == null)
				{
					MenuSurvivorsClothingBoxUI.<>f__mg$cache2 = new TempSteamworksEconomy.InventoryExchanged(MenuSurvivorsClothingBoxUI.onInventoryExchanged);
				}
				economyService.onInventoryExchanged = (TempSteamworksEconomy.InventoryExchanged)Delegate.Combine(onInventoryExchanged, MenuSurvivorsClothingBoxUI.<>f__mg$cache2);
			}
			MenuSurvivorsClothingBoxUI.hasLoaded = true;
			MenuSurvivorsClothingBoxUI.backButton = new SleekButtonIcon((Texture2D)MenuDashboardUI.icons.load("Exit"));
			MenuSurvivorsClothingBoxUI.backButton.positionOffset_Y = -50;
			MenuSurvivorsClothingBoxUI.backButton.positionScale_Y = 1f;
			MenuSurvivorsClothingBoxUI.backButton.sizeOffset_X = 200;
			MenuSurvivorsClothingBoxUI.backButton.sizeOffset_Y = 50;
			MenuSurvivorsClothingBoxUI.backButton.text = MenuDashboardUI.localization.format("BackButtonText");
			MenuSurvivorsClothingBoxUI.backButton.tooltip = MenuDashboardUI.localization.format("BackButtonTooltip");
			SleekButton sleekButton3 = MenuSurvivorsClothingBoxUI.backButton;
			if (MenuSurvivorsClothingBoxUI.<>f__mg$cache3 == null)
			{
				MenuSurvivorsClothingBoxUI.<>f__mg$cache3 = new ClickedButton(MenuSurvivorsClothingBoxUI.onClickedBackButton);
			}
			sleekButton3.onClickedButton = MenuSurvivorsClothingBoxUI.<>f__mg$cache3;
			MenuSurvivorsClothingBoxUI.backButton.fontSize = 14;
			MenuSurvivorsClothingBoxUI.backButton.iconImage.backgroundTint = ESleekTint.FOREGROUND;
			MenuSurvivorsClothingBoxUI.container.add(MenuSurvivorsClothingBoxUI.backButton);
		}

		public static void open()
		{
			if (MenuSurvivorsClothingBoxUI.active)
			{
				return;
			}
			MenuSurvivorsClothingBoxUI.active = true;
			MenuSurvivorsClothingBoxUI.container.lerpPositionScale(0f, 0f, ESleekLerp.EXPONENTIAL, 20f);
		}

		public static void close()
		{
			if (!MenuSurvivorsClothingBoxUI.active)
			{
				return;
			}
			MenuSurvivorsClothingBoxUI.active = false;
			MenuSurvivorsClothingBoxUI.container.lerpPositionScale(0f, 1f, ESleekLerp.EXPONENTIAL, 20f);
		}

		public static void viewItem(int newItem, ushort newQuantity, ulong newInstance)
		{
			MenuSurvivorsClothingBoxUI.item = newItem;
			MenuSurvivorsClothingBoxUI.instance = newInstance;
			MenuSurvivorsClothingBoxUI.drop = -1;
			MenuSurvivorsClothingBoxUI.isMythical = false;
			MenuSurvivorsClothingBoxUI.angle = 0f;
			MenuSurvivorsClothingBoxUI.lastRotation = 0;
			MenuSurvivorsClothingBoxUI.rotation = 0;
			MenuSurvivorsClothingBoxUI.target = -1;
			MenuSurvivorsClothingBoxUI.keyButton.isVisible = true;
			MenuSurvivorsClothingBoxUI.unboxButton.isVisible = true;
			MenuSurvivorsClothingBoxUI.boxButton.updateInventory(MenuSurvivorsClothingBoxUI.instance, MenuSurvivorsClothingBoxUI.item, newQuantity, false, true);
			MenuSurvivorsClothingBoxUI.boxAsset = (ItemBoxAsset)Assets.find(EAssetType.ITEM, Provider.provider.economyService.getInventoryItemID(MenuSurvivorsClothingBoxUI.item));
			if (MenuSurvivorsClothingBoxUI.boxAsset != null)
			{
				if (MenuSurvivorsClothingBoxUI.boxAsset.destroy == 0)
				{
					MenuSurvivorsClothingBoxUI.keyButton.isVisible = false;
					MenuSurvivorsClothingBoxUI.unboxButton.icon = null;
					MenuSurvivorsClothingBoxUI.unboxButton.positionOffset_X = 0;
					MenuSurvivorsClothingBoxUI.unboxButton.positionScale_X = 0.3f;
					MenuSurvivorsClothingBoxUI.unboxButton.sizeOffset_X = 0;
					MenuSurvivorsClothingBoxUI.unboxButton.sizeScale_X = 0.4f;
					MenuSurvivorsClothingBoxUI.unboxButton.text = MenuSurvivorsClothingBoxUI.localization.format("Unwrap_Text");
					MenuSurvivorsClothingBoxUI.unboxButton.tooltip = MenuSurvivorsClothingBoxUI.localization.format("Unwrap_Tooltip");
					MenuSurvivorsClothingBoxUI.unboxButton.isVisible = true;
					MenuSurvivorsClothingBoxUI.keyAsset = null;
				}
				else
				{
					MenuSurvivorsClothingBoxUI.keyButton.isVisible = true;
					MenuSurvivorsClothingBoxUI.unboxButton.icon = (Texture2D)MenuSurvivorsClothingBoxUI.icons.load("Unbox");
					MenuSurvivorsClothingBoxUI.unboxButton.positionOffset_X = 5;
					MenuSurvivorsClothingBoxUI.unboxButton.positionScale_X = 0.5f;
					MenuSurvivorsClothingBoxUI.unboxButton.sizeOffset_X = -5;
					MenuSurvivorsClothingBoxUI.unboxButton.sizeScale_X = 0.2f;
					MenuSurvivorsClothingBoxUI.unboxButton.text = MenuSurvivorsClothingBoxUI.localization.format("Unbox_Text");
					MenuSurvivorsClothingBoxUI.unboxButton.tooltip = MenuSurvivorsClothingBoxUI.localization.format("Unbox_Tooltip");
					MenuSurvivorsClothingBoxUI.unboxButton.isVisible = true;
					MenuSurvivorsClothingBoxUI.keyAsset = (ItemKeyAsset)Assets.find(EAssetType.ITEM, Provider.provider.economyService.getInventoryItemID(MenuSurvivorsClothingBoxUI.boxAsset.destroy));
					if (MenuSurvivorsClothingBoxUI.keyAsset != null)
					{
						MenuSurvivorsClothingBoxUI.keyButton.icon = (Texture2D)Resources.Load("Economy" + MenuSurvivorsClothingBoxUI.keyAsset.proPath + "/Icon_Small");
					}
				}
				MenuSurvivorsClothingBoxUI.size = 6.28318548f / (float)MenuSurvivorsClothingBoxUI.boxAsset.drops.Length / 2.75f;
				MenuSurvivorsClothingBoxUI.finalBox.positionScale_Y = 0.5f - MenuSurvivorsClothingBoxUI.size / 2f;
				MenuSurvivorsClothingBoxUI.finalBox.sizeScale_X = MenuSurvivorsClothingBoxUI.size;
				MenuSurvivorsClothingBoxUI.finalBox.sizeScale_Y = MenuSurvivorsClothingBoxUI.size;
				if (MenuSurvivorsClothingBoxUI.dropButtons != null)
				{
					for (int i = 0; i < MenuSurvivorsClothingBoxUI.dropButtons.Length; i++)
					{
						MenuSurvivorsClothingBoxUI.inventory.remove(MenuSurvivorsClothingBoxUI.dropButtons[i]);
					}
				}
				MenuSurvivorsClothingBoxUI.dropButtons = new SleekInventory[MenuSurvivorsClothingBoxUI.boxAsset.drops.Length];
				for (int j = 0; j < MenuSurvivorsClothingBoxUI.boxAsset.drops.Length; j++)
				{
					float num = 6.28318548f * (float)j / (float)MenuSurvivorsClothingBoxUI.boxAsset.drops.Length + 3.14159274f;
					SleekInventory sleekInventory = new SleekInventory();
					sleekInventory.positionScale_X = 0.5f + Mathf.Cos(-num) * (0.5f - MenuSurvivorsClothingBoxUI.size / 2f) - MenuSurvivorsClothingBoxUI.size / 2f;
					sleekInventory.positionScale_Y = 0.5f + Mathf.Sin(-num) * (0.5f - MenuSurvivorsClothingBoxUI.size / 2f) - MenuSurvivorsClothingBoxUI.size / 2f;
					sleekInventory.sizeScale_X = MenuSurvivorsClothingBoxUI.size;
					sleekInventory.sizeScale_Y = MenuSurvivorsClothingBoxUI.size;
					sleekInventory.updateInventory(0UL, MenuSurvivorsClothingBoxUI.boxAsset.drops[j], 1, false, false);
					MenuSurvivorsClothingBoxUI.inventory.add(sleekInventory);
					MenuSurvivorsClothingBoxUI.dropButtons[j] = sleekInventory;
				}
			}
			MenuSurvivorsClothingBoxUI.keyButton.backgroundColor = Provider.provider.economyService.getInventoryColor(MenuSurvivorsClothingBoxUI.item);
			MenuSurvivorsClothingBoxUI.keyButton.foregroundColor = MenuSurvivorsClothingBoxUI.keyButton.backgroundColor;
			MenuSurvivorsClothingBoxUI.unboxButton.backgroundColor = MenuSurvivorsClothingBoxUI.keyButton.backgroundColor;
			MenuSurvivorsClothingBoxUI.unboxButton.foregroundColor = MenuSurvivorsClothingBoxUI.keyButton.backgroundColor;
		}

		private static void onClickedKeyButton(SleekButton button)
		{
			if (!Provider.provider.storeService.canOpenStore)
			{
				MenuUI.alert(MenuSurvivorsClothingBoxUI.localization.format("Overlay"));
				return;
			}
			Provider.provider.storeService.open(new SteamworksEconomyItemDefinition((SteamItemDef_t)MenuSurvivorsClothingBoxUI.boxAsset.destroy));
		}

		private static void onClickedUnboxButton(SleekButton button)
		{
			if (MenuSurvivorsClothingBoxUI.boxAsset.destroy == 0)
			{
				Provider.provider.economyService.exchangeInventory(MenuSurvivorsClothingBoxUI.boxAsset.generate, new ulong[]
				{
					MenuSurvivorsClothingBoxUI.instance
				});
			}
			else
			{
				ulong inventoryPackage = Provider.provider.economyService.getInventoryPackage(MenuSurvivorsClothingBoxUI.boxAsset.destroy);
				if (inventoryPackage == 0UL)
				{
					return;
				}
				Provider.provider.economyService.exchangeInventory(MenuSurvivorsClothingBoxUI.boxAsset.generate, new ulong[]
				{
					MenuSurvivorsClothingBoxUI.instance,
					inventoryPackage
				});
			}
			MenuSurvivorsClothingBoxUI.isUnboxing = true;
			MenuSurvivorsClothingBoxUI.backButton.isVisible = false;
			MenuSurvivorsClothingBoxUI.lastUnbox = Time.realtimeSinceStartup;
			MenuSurvivorsClothingBoxUI.lastAngle = Time.realtimeSinceStartup;
			MenuSurvivorsClothingBoxUI.keyButton.isVisible = false;
			MenuSurvivorsClothingBoxUI.unboxButton.isVisible = false;
		}

		private static void onInventoryExchanged(int newItem, ushort newQuantity, ulong newInstance)
		{
			MenuSurvivorsClothingBoxUI.drop = newItem;
			MenuSurvivorsClothingBoxUI.got = newInstance;
			ushort inventoryItemID = Provider.provider.economyService.getInventoryItemID(MenuSurvivorsClothingBoxUI.drop);
			if ((ItemAsset)Assets.find(EAssetType.ITEM, inventoryItemID) == null)
			{
				MenuSurvivorsClothingBoxUI.isUnboxing = false;
				MenuSurvivorsClothingBoxUI.backButton.isVisible = true;
				MenuUI.alert(MenuSurvivorsClothingBoxUI.localization.format("Exchange_Unknown"));
				MenuSurvivorsClothingUI.open();
				MenuSurvivorsClothingBoxUI.close();
				return;
			}
			MenuSurvivorsClothingUI.updatePage();
			int num = 0;
			MenuSurvivorsClothingBoxUI.isMythical = true;
			for (int i = 1; i < MenuSurvivorsClothingBoxUI.boxAsset.drops.Length; i++)
			{
				if (MenuSurvivorsClothingBoxUI.drop == MenuSurvivorsClothingBoxUI.boxAsset.drops[i])
				{
					num = i;
					MenuSurvivorsClothingBoxUI.isMythical = false;
					break;
				}
			}
			if (MenuSurvivorsClothingBoxUI.isMythical && Provider.provider.economyService.getInventoryMythicID(MenuSurvivorsClothingBoxUI.drop) == 0)
			{
				MenuSurvivorsClothingBoxUI.isUnboxing = false;
				MenuSurvivorsClothingBoxUI.backButton.isVisible = true;
				MenuUI.alert(MenuSurvivorsClothingBoxUI.localization.format("Exchange_Mythic"));
				MenuSurvivorsClothingUI.open();
				MenuSurvivorsClothingBoxUI.close();
				return;
			}
			if (MenuSurvivorsClothingBoxUI.rotation < MenuSurvivorsClothingBoxUI.boxAsset.drops.Length * 2)
			{
				MenuSurvivorsClothingBoxUI.target = MenuSurvivorsClothingBoxUI.boxAsset.drops.Length * 3 + num;
			}
			else
			{
				MenuSurvivorsClothingBoxUI.target = ((int)((float)MenuSurvivorsClothingBoxUI.rotation / (float)MenuSurvivorsClothingBoxUI.boxAsset.drops.Length) + 2) * MenuSurvivorsClothingBoxUI.boxAsset.drops.Length + num;
			}
		}

		public static void update()
		{
			if (!MenuSurvivorsClothingBoxUI.isUnboxing)
			{
				return;
			}
			if (Time.realtimeSinceStartup - MenuSurvivorsClothingBoxUI.lastUnbox > (float)Provider.CLIENT_TIMEOUT)
			{
				MenuSurvivorsClothingBoxUI.isUnboxing = false;
				MenuSurvivorsClothingBoxUI.backButton.isVisible = true;
				MenuUI.alert(MenuSurvivorsClothingBoxUI.localization.format("Exchange_Timed_Out"));
				MenuSurvivorsClothingUI.open();
				MenuSurvivorsClothingBoxUI.close();
				return;
			}
			if (MenuSurvivorsClothingBoxUI.rotation == MenuSurvivorsClothingBoxUI.target)
			{
				if (Time.realtimeSinceStartup - MenuSurvivorsClothingBoxUI.lastAngle > 0.5f)
				{
					MenuSurvivorsClothingBoxUI.isUnboxing = false;
					MenuSurvivorsClothingBoxUI.backButton.isVisible = true;
					if (MenuSurvivorsClothingBoxUI.boxAsset.destroy == 0)
					{
						MenuUI.alert(MenuSurvivorsClothingBoxUI.localization.format("Origin_Unwrap"), MenuSurvivorsClothingBoxUI.got, MenuSurvivorsClothingBoxUI.drop, 1);
					}
					else
					{
						MenuUI.alert(MenuSurvivorsClothingBoxUI.localization.format("Origin_Unbox"), MenuSurvivorsClothingBoxUI.got, MenuSurvivorsClothingBoxUI.drop, 1);
					}
					MenuSurvivorsClothingItemUI.viewItem(MenuSurvivorsClothingBoxUI.drop, 1, MenuSurvivorsClothingBoxUI.got);
					MenuSurvivorsClothingItemUI.open();
					MenuSurvivorsClothingBoxUI.close();
					if (MenuSurvivorsClothingBoxUI.isMythical)
					{
						MainCamera.instance.GetComponent<AudioSource>().PlayOneShot((AudioClip)Resources.Load("Economy/Sounds/Mythical"), 0.66f);
					}
					else
					{
						MainCamera.instance.GetComponent<AudioSource>().PlayOneShot((AudioClip)Resources.Load("Economy/Sounds/Unbox"), 0.66f);
					}
				}
			}
			else
			{
				if (MenuSurvivorsClothingBoxUI.rotation < MenuSurvivorsClothingBoxUI.target - MenuSurvivorsClothingBoxUI.boxAsset.drops.Length || MenuSurvivorsClothingBoxUI.target == -1)
				{
					if (MenuSurvivorsClothingBoxUI.angle < 12.566371f)
					{
						MenuSurvivorsClothingBoxUI.angle += (Time.realtimeSinceStartup - MenuSurvivorsClothingBoxUI.lastAngle) * MenuSurvivorsClothingBoxUI.size * Mathf.Lerp(80f, 20f, MenuSurvivorsClothingBoxUI.angle / 12.566371f);
					}
					else
					{
						MenuSurvivorsClothingBoxUI.angle += (Time.realtimeSinceStartup - MenuSurvivorsClothingBoxUI.lastAngle) * MenuSurvivorsClothingBoxUI.size * 20f;
					}
				}
				else
				{
					MenuSurvivorsClothingBoxUI.angle += (Time.realtimeSinceStartup - MenuSurvivorsClothingBoxUI.lastAngle) * Mathf.Max(((float)MenuSurvivorsClothingBoxUI.target - MenuSurvivorsClothingBoxUI.angle / (6.28318548f / (float)MenuSurvivorsClothingBoxUI.boxAsset.drops.Length)) / (float)MenuSurvivorsClothingBoxUI.boxAsset.drops.Length, 0.05f) * MenuSurvivorsClothingBoxUI.size * 20f;
				}
				MenuSurvivorsClothingBoxUI.lastAngle = Time.realtimeSinceStartup;
				MenuSurvivorsClothingBoxUI.rotation = (int)(MenuSurvivorsClothingBoxUI.angle / (6.28318548f / (float)MenuSurvivorsClothingBoxUI.boxAsset.drops.Length));
				if (MenuSurvivorsClothingBoxUI.rotation == MenuSurvivorsClothingBoxUI.target)
				{
					MenuSurvivorsClothingBoxUI.angle = (float)MenuSurvivorsClothingBoxUI.rotation * (6.28318548f / (float)MenuSurvivorsClothingBoxUI.boxAsset.drops.Length);
				}
				for (int i = 0; i < MenuSurvivorsClothingBoxUI.boxAsset.drops.Length; i++)
				{
					float num = 6.28318548f * (float)i / (float)MenuSurvivorsClothingBoxUI.boxAsset.drops.Length + 3.14159274f;
					MenuSurvivorsClothingBoxUI.dropButtons[i].positionScale_X = 0.5f + Mathf.Cos(MenuSurvivorsClothingBoxUI.angle - num) * (0.5f - MenuSurvivorsClothingBoxUI.size / 2f) - MenuSurvivorsClothingBoxUI.size / 2f;
					MenuSurvivorsClothingBoxUI.dropButtons[i].positionScale_Y = 0.5f + Mathf.Sin(MenuSurvivorsClothingBoxUI.angle - num) * (0.5f - MenuSurvivorsClothingBoxUI.size / 2f) - MenuSurvivorsClothingBoxUI.size / 2f;
				}
				if (MenuSurvivorsClothingBoxUI.rotation != MenuSurvivorsClothingBoxUI.lastRotation)
				{
					MenuSurvivorsClothingBoxUI.lastRotation = MenuSurvivorsClothingBoxUI.rotation;
					MenuSurvivorsClothingBoxUI.boxButton.positionScale_Y = 0.25f;
					MenuSurvivorsClothingBoxUI.boxButton.lerpPositionScale(0.3f, 0.3f, ESleekLerp.EXPONENTIAL, 20f);
					MenuSurvivorsClothingBoxUI.boxButton.updateInventory(0UL, MenuSurvivorsClothingBoxUI.boxAsset.drops[MenuSurvivorsClothingBoxUI.rotation % MenuSurvivorsClothingBoxUI.boxAsset.drops.Length], 1, false, true);
					if (MenuSurvivorsClothingBoxUI.rotation == MenuSurvivorsClothingBoxUI.target)
					{
						MainCamera.instance.GetComponent<AudioSource>().PlayOneShot((AudioClip)Resources.Load("Economy/Sounds/Drop"), 0.33f);
					}
					else
					{
						MainCamera.instance.GetComponent<AudioSource>().PlayOneShot((AudioClip)Resources.Load("Economy/Sounds/Tick"), 0.33f);
					}
				}
			}
		}

		private static void onClickedBackButton(SleekButton button)
		{
			MenuSurvivorsClothingItemUI.open();
			MenuSurvivorsClothingBoxUI.close();
		}

		private static Bundle icons;

		private static Local localization;

		private static Sleek container;

		public static bool active;

		private static SleekButtonIcon backButton;

		public static bool isUnboxing;

		private static float lastUnbox;

		private static float lastAngle;

		private static float angle;

		private static int lastRotation;

		private static int rotation;

		private static int target;

		private static bool hasLoaded;

		private static int item;

		private static ulong instance;

		private static int drop;

		private static bool isMythical;

		private static ulong got;

		private static ItemBoxAsset boxAsset;

		private static ItemKeyAsset keyAsset;

		private static float size;

		private static Sleek inventory;

		private static SleekBox finalBox;

		private static SleekInventory boxButton;

		private static SleekButtonIcon keyButton;

		private static SleekButtonIcon unboxButton;

		private static SleekInventory[] dropButtons;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache0;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache1;

		[CompilerGenerated]
		private static TempSteamworksEconomy.InventoryExchanged <>f__mg$cache2;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache3;
	}
}
