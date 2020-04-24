using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace SDG.Unturned
{
	public class PlayerDashboardUI
	{
		public PlayerDashboardUI()
		{
			Local local = Localization.read("/Player/PlayerDashboard.dat");
			Bundle bundle = Bundles.getBundle("/Bundles/Textures/Player/Icons/PlayerDashboard/PlayerDashboard.unity3d");
			PlayerDashboardUI.container = new Sleek();
			PlayerDashboardUI.container.positionScale_Y = -1f;
			PlayerDashboardUI.container.positionOffset_X = 10;
			PlayerDashboardUI.container.positionOffset_Y = 10;
			PlayerDashboardUI.container.sizeOffset_X = -20;
			PlayerDashboardUI.container.sizeOffset_Y = -20;
			PlayerDashboardUI.container.sizeScale_X = 1f;
			PlayerDashboardUI.container.sizeScale_Y = 1f;
			PlayerUI.container.add(PlayerDashboardUI.container);
			PlayerDashboardUI.active = false;
			PlayerDashboardUI.inventoryButton = new SleekButtonIcon((Texture2D)bundle.load("Inventory"));
			PlayerDashboardUI.inventoryButton.sizeOffset_X = -5;
			PlayerDashboardUI.inventoryButton.sizeOffset_Y = 50;
			PlayerDashboardUI.inventoryButton.sizeScale_X = 0.25f;
			PlayerDashboardUI.inventoryButton.text = local.format("Inventory", new object[]
			{
				ControlsSettings.inventory
			});
			PlayerDashboardUI.inventoryButton.tooltip = local.format("Inventory_Tooltip");
			SleekButton sleekButton = PlayerDashboardUI.inventoryButton;
			if (PlayerDashboardUI.<>f__mg$cache0 == null)
			{
				PlayerDashboardUI.<>f__mg$cache0 = new ClickedButton(PlayerDashboardUI.onClickedInventoryButton);
			}
			sleekButton.onClickedButton = PlayerDashboardUI.<>f__mg$cache0;
			PlayerDashboardUI.inventoryButton.fontSize = 14;
			PlayerDashboardUI.inventoryButton.iconImage.backgroundTint = ESleekTint.FOREGROUND;
			PlayerDashboardUI.container.add(PlayerDashboardUI.inventoryButton);
			PlayerDashboardUI.craftingButton = new SleekButtonIcon((Texture2D)bundle.load("Crafting"));
			PlayerDashboardUI.craftingButton.positionOffset_X = 5;
			PlayerDashboardUI.craftingButton.positionScale_X = 0.25f;
			PlayerDashboardUI.craftingButton.sizeOffset_X = -10;
			PlayerDashboardUI.craftingButton.sizeOffset_Y = 50;
			PlayerDashboardUI.craftingButton.sizeScale_X = 0.25f;
			PlayerDashboardUI.craftingButton.text = local.format("Crafting", new object[]
			{
				ControlsSettings.crafting
			});
			PlayerDashboardUI.craftingButton.tooltip = local.format("Crafting_Tooltip");
			SleekButton sleekButton2 = PlayerDashboardUI.craftingButton;
			if (PlayerDashboardUI.<>f__mg$cache1 == null)
			{
				PlayerDashboardUI.<>f__mg$cache1 = new ClickedButton(PlayerDashboardUI.onClickedCraftingButton);
			}
			sleekButton2.onClickedButton = PlayerDashboardUI.<>f__mg$cache1;
			PlayerDashboardUI.craftingButton.iconImage.backgroundTint = ESleekTint.FOREGROUND;
			PlayerDashboardUI.craftingButton.fontSize = 14;
			PlayerDashboardUI.container.add(PlayerDashboardUI.craftingButton);
			PlayerDashboardUI.skillsButton = new SleekButtonIcon((Texture2D)bundle.load("Skills"));
			PlayerDashboardUI.skillsButton.positionOffset_X = 5;
			PlayerDashboardUI.skillsButton.positionScale_X = 0.5f;
			PlayerDashboardUI.skillsButton.sizeOffset_X = -10;
			PlayerDashboardUI.skillsButton.sizeOffset_Y = 50;
			PlayerDashboardUI.skillsButton.sizeScale_X = 0.25f;
			PlayerDashboardUI.skillsButton.text = local.format("Skills", new object[]
			{
				ControlsSettings.skills
			});
			PlayerDashboardUI.skillsButton.tooltip = local.format("Skills_Tooltip");
			PlayerDashboardUI.skillsButton.iconImage.backgroundTint = ESleekTint.FOREGROUND;
			SleekButton sleekButton3 = PlayerDashboardUI.skillsButton;
			if (PlayerDashboardUI.<>f__mg$cache2 == null)
			{
				PlayerDashboardUI.<>f__mg$cache2 = new ClickedButton(PlayerDashboardUI.onClickedSkillsButton);
			}
			sleekButton3.onClickedButton = PlayerDashboardUI.<>f__mg$cache2;
			PlayerDashboardUI.skillsButton.fontSize = 14;
			PlayerDashboardUI.container.add(PlayerDashboardUI.skillsButton);
			PlayerDashboardUI.informationButton = new SleekButtonIcon((Texture2D)bundle.load("Information"));
			PlayerDashboardUI.informationButton.positionOffset_X = 5;
			PlayerDashboardUI.informationButton.positionScale_X = 0.75f;
			PlayerDashboardUI.informationButton.sizeOffset_X = -5;
			PlayerDashboardUI.informationButton.sizeOffset_Y = 50;
			PlayerDashboardUI.informationButton.sizeScale_X = 0.25f;
			PlayerDashboardUI.informationButton.text = local.format("Information", new object[]
			{
				ControlsSettings.map
			});
			PlayerDashboardUI.informationButton.tooltip = local.format("Information_Tooltip");
			PlayerDashboardUI.informationButton.iconImage.backgroundTint = ESleekTint.FOREGROUND;
			SleekButton sleekButton4 = PlayerDashboardUI.informationButton;
			if (PlayerDashboardUI.<>f__mg$cache3 == null)
			{
				PlayerDashboardUI.<>f__mg$cache3 = new ClickedButton(PlayerDashboardUI.onClickedInformationButton);
			}
			sleekButton4.onClickedButton = PlayerDashboardUI.<>f__mg$cache3;
			PlayerDashboardUI.informationButton.fontSize = 14;
			PlayerDashboardUI.container.add(PlayerDashboardUI.informationButton);
			if (Level.info != null && Level.info.type == ELevelType.HORDE)
			{
				PlayerDashboardUI.inventoryButton.sizeScale_X = 0.5f;
				PlayerDashboardUI.craftingButton.isVisible = false;
				PlayerDashboardUI.skillsButton.isVisible = false;
				PlayerDashboardUI.informationButton.positionScale_X = 0.5f;
				PlayerDashboardUI.informationButton.sizeScale_X = 0.5f;
			}
			bundle.unload();
			new PlayerDashboardInventoryUI();
			new PlayerDashboardCraftingUI();
			new PlayerDashboardSkillsUI();
			new PlayerDashboardInformationUI();
		}

		public static void open()
		{
			if (PlayerDashboardUI.active)
			{
				return;
			}
			PlayerDashboardUI.active = true;
			if (PlayerDashboardInventoryUI.active)
			{
				PlayerDashboardInventoryUI.active = false;
				PlayerDashboardInventoryUI.open();
			}
			else if (PlayerDashboardCraftingUI.active)
			{
				PlayerDashboardCraftingUI.active = false;
				PlayerDashboardCraftingUI.open();
			}
			else if (PlayerDashboardSkillsUI.active)
			{
				PlayerDashboardSkillsUI.active = false;
				PlayerDashboardSkillsUI.open();
			}
			else if (PlayerDashboardInformationUI.active)
			{
				PlayerDashboardInformationUI.active = false;
				PlayerDashboardInformationUI.open();
			}
			PlayerDashboardUI.container.lerpPositionScale(0f, 0f, ESleekLerp.EXPONENTIAL, 20f);
		}

		public static void close()
		{
			if (!PlayerDashboardUI.active)
			{
				return;
			}
			PlayerDashboardUI.active = false;
			if (PlayerDashboardInventoryUI.active)
			{
				PlayerDashboardInventoryUI.close();
				PlayerDashboardInventoryUI.active = true;
			}
			else if (PlayerDashboardCraftingUI.active)
			{
				PlayerDashboardCraftingUI.close();
				PlayerDashboardCraftingUI.active = true;
			}
			else if (PlayerDashboardSkillsUI.active)
			{
				PlayerDashboardSkillsUI.close();
				PlayerDashboardSkillsUI.active = true;
			}
			else if (PlayerDashboardInformationUI.active)
			{
				PlayerDashboardInformationUI.close();
				PlayerDashboardInformationUI.active = true;
			}
			PlayerDashboardUI.container.lerpPositionScale(0f, -1f, ESleekLerp.EXPONENTIAL, 20f);
		}

		private static void onClickedInventoryButton(SleekButton button)
		{
			PlayerDashboardCraftingUI.close();
			PlayerDashboardSkillsUI.close();
			PlayerDashboardInformationUI.close();
			if (PlayerDashboardInventoryUI.active)
			{
				PlayerDashboardUI.close();
				PlayerLifeUI.open();
			}
			else
			{
				PlayerDashboardInventoryUI.open();
			}
		}

		private static void onClickedCraftingButton(SleekButton button)
		{
			PlayerDashboardInventoryUI.close();
			PlayerDashboardSkillsUI.close();
			PlayerDashboardInformationUI.close();
			if (PlayerDashboardCraftingUI.active)
			{
				PlayerDashboardUI.close();
				PlayerLifeUI.open();
			}
			else
			{
				PlayerDashboardCraftingUI.open();
			}
		}

		private static void onClickedSkillsButton(SleekButton button)
		{
			PlayerDashboardInventoryUI.close();
			PlayerDashboardCraftingUI.close();
			PlayerDashboardInformationUI.close();
			if (PlayerDashboardSkillsUI.active)
			{
				PlayerDashboardUI.close();
				PlayerLifeUI.open();
			}
			else
			{
				PlayerDashboardSkillsUI.open();
			}
		}

		private static void onClickedInformationButton(SleekButton button)
		{
			PlayerDashboardInventoryUI.close();
			PlayerDashboardCraftingUI.close();
			PlayerDashboardSkillsUI.close();
			if (PlayerDashboardInformationUI.active)
			{
				PlayerDashboardUI.close();
				PlayerLifeUI.open();
			}
			else
			{
				PlayerDashboardInformationUI.open();
			}
		}

		public static Sleek container;

		public static bool active;

		private static SleekButtonIcon inventoryButton;

		private static SleekButtonIcon craftingButton;

		private static SleekButtonIcon skillsButton;

		private static SleekButtonIcon informationButton;

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
