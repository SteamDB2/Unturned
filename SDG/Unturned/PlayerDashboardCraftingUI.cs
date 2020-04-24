using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace SDG.Unturned
{
	public class PlayerDashboardCraftingUI
	{
		public PlayerDashboardCraftingUI()
		{
			if (PlayerDashboardCraftingUI.icons != null)
			{
				PlayerDashboardCraftingUI.icons.unload();
			}
			PlayerDashboardCraftingUI.localization = Localization.read("/Player/PlayerDashboardCrafting.dat");
			PlayerDashboardCraftingUI.icons = Bundles.getBundle("/Bundles/Textures/Player/Icons/PlayerDashboardCrafting/PlayerDashboardCrafting.unity3d");
			PlayerDashboardCraftingUI.container = new Sleek();
			PlayerDashboardCraftingUI.container.positionScale_Y = 1f;
			PlayerDashboardCraftingUI.container.positionOffset_X = 10;
			PlayerDashboardCraftingUI.container.positionOffset_Y = 10;
			PlayerDashboardCraftingUI.container.sizeOffset_X = -20;
			PlayerDashboardCraftingUI.container.sizeOffset_Y = -20;
			PlayerDashboardCraftingUI.container.sizeScale_X = 1f;
			PlayerDashboardCraftingUI.container.sizeScale_Y = 1f;
			PlayerUI.container.add(PlayerDashboardCraftingUI.container);
			PlayerDashboardCraftingUI.active = false;
			PlayerDashboardCraftingUI.selectedType = byte.MaxValue;
			PlayerDashboardCraftingUI.hideUncraftable = false;
			PlayerDashboardCraftingUI.searchText = string.Empty;
			PlayerDashboardCraftingUI.backdropBox = new SleekBox();
			PlayerDashboardCraftingUI.backdropBox.positionOffset_Y = 60;
			PlayerDashboardCraftingUI.backdropBox.sizeOffset_Y = -60;
			PlayerDashboardCraftingUI.backdropBox.sizeScale_X = 1f;
			PlayerDashboardCraftingUI.backdropBox.sizeScale_Y = 1f;
			Color white = Color.white;
			white.a = 0.5f;
			PlayerDashboardCraftingUI.backdropBox.backgroundColor = white;
			PlayerDashboardCraftingUI.container.add(PlayerDashboardCraftingUI.backdropBox);
			PlayerDashboardCraftingUI.blueprintsScrollBox = new SleekScrollBox();
			PlayerDashboardCraftingUI.blueprintsScrollBox.positionOffset_X = 10;
			PlayerDashboardCraftingUI.blueprintsScrollBox.positionOffset_Y = 110;
			PlayerDashboardCraftingUI.blueprintsScrollBox.sizeOffset_X = -20;
			PlayerDashboardCraftingUI.blueprintsScrollBox.sizeOffset_Y = -120;
			PlayerDashboardCraftingUI.blueprintsScrollBox.sizeScale_X = 1f;
			PlayerDashboardCraftingUI.blueprintsScrollBox.sizeScale_Y = 1f;
			PlayerDashboardCraftingUI.backdropBox.add(PlayerDashboardCraftingUI.blueprintsScrollBox);
			for (int i = 0; i < PlayerDashboardCraftingUI.TYPES; i++)
			{
				SleekButtonIcon sleekButtonIcon = new SleekButtonIcon((Texture2D)PlayerDashboardCraftingUI.icons.load("Blueprint_" + i));
				sleekButtonIcon.positionOffset_X = PlayerDashboardCraftingUI.TYPES * -30 + 5 + i * 60;
				sleekButtonIcon.positionOffset_Y = 10;
				sleekButtonIcon.positionScale_X = 0.5f;
				sleekButtonIcon.sizeOffset_X = 50;
				sleekButtonIcon.sizeOffset_Y = 50;
				sleekButtonIcon.tooltip = PlayerDashboardCraftingUI.localization.format("Type_" + i + "_Tooltip");
				sleekButtonIcon.iconImage.backgroundTint = ESleekTint.FOREGROUND;
				SleekButton sleekButton = sleekButtonIcon;
				if (PlayerDashboardCraftingUI.<>f__mg$cache1 == null)
				{
					PlayerDashboardCraftingUI.<>f__mg$cache1 = new ClickedButton(PlayerDashboardCraftingUI.onClickedTypeButton);
				}
				sleekButton.onClickedButton = PlayerDashboardCraftingUI.<>f__mg$cache1;
				PlayerDashboardCraftingUI.backdropBox.add(sleekButtonIcon);
			}
			PlayerDashboardCraftingUI.hideUncraftableToggle = new SleekToggle();
			PlayerDashboardCraftingUI.hideUncraftableToggle.positionOffset_X = -80;
			PlayerDashboardCraftingUI.hideUncraftableToggle.positionOffset_Y = 65;
			PlayerDashboardCraftingUI.hideUncraftableToggle.positionScale_X = 1f;
			PlayerDashboardCraftingUI.hideUncraftableToggle.sizeOffset_X = 40;
			PlayerDashboardCraftingUI.hideUncraftableToggle.sizeOffset_Y = 40;
			PlayerDashboardCraftingUI.hideUncraftableToggle.addLabel(PlayerDashboardCraftingUI.localization.format("Hide_Uncraftable_Toggle_Label"), ESleekSide.LEFT);
			PlayerDashboardCraftingUI.hideUncraftableToggle.state = PlayerDashboardCraftingUI.hideUncraftable;
			SleekToggle sleekToggle = PlayerDashboardCraftingUI.hideUncraftableToggle;
			if (PlayerDashboardCraftingUI.<>f__mg$cache2 == null)
			{
				PlayerDashboardCraftingUI.<>f__mg$cache2 = new Toggled(PlayerDashboardCraftingUI.onToggledHideUncraftableToggle);
			}
			sleekToggle.onToggled = PlayerDashboardCraftingUI.<>f__mg$cache2;
			PlayerDashboardCraftingUI.backdropBox.add(PlayerDashboardCraftingUI.hideUncraftableToggle);
			PlayerDashboardCraftingUI.searchField = new SleekField();
			PlayerDashboardCraftingUI.searchField.positionOffset_X = 10;
			PlayerDashboardCraftingUI.searchField.positionOffset_Y = 70;
			PlayerDashboardCraftingUI.searchField.sizeOffset_X = -410;
			PlayerDashboardCraftingUI.searchField.sizeOffset_Y = 30;
			PlayerDashboardCraftingUI.searchField.sizeScale_X = 1f;
			PlayerDashboardCraftingUI.searchField.hint = PlayerDashboardCraftingUI.localization.format("Search_Field_Hint");
			PlayerDashboardCraftingUI.searchField.control = "Search";
			SleekField sleekField = PlayerDashboardCraftingUI.searchField;
			Delegate onEntered = sleekField.onEntered;
			if (PlayerDashboardCraftingUI.<>f__mg$cache3 == null)
			{
				PlayerDashboardCraftingUI.<>f__mg$cache3 = new Entered(PlayerDashboardCraftingUI.onEnteredSearchField);
			}
			sleekField.onEntered = (Entered)Delegate.Combine(onEntered, PlayerDashboardCraftingUI.<>f__mg$cache3);
			PlayerDashboardCraftingUI.backdropBox.add(PlayerDashboardCraftingUI.searchField);
			PlayerDashboardCraftingUI.searchButton = new SleekButton();
			PlayerDashboardCraftingUI.searchButton.positionOffset_X = -390;
			PlayerDashboardCraftingUI.searchButton.positionOffset_Y = 70;
			PlayerDashboardCraftingUI.searchButton.positionScale_X = 1f;
			PlayerDashboardCraftingUI.searchButton.sizeOffset_X = 100;
			PlayerDashboardCraftingUI.searchButton.sizeOffset_Y = 30;
			PlayerDashboardCraftingUI.searchButton.text = PlayerDashboardCraftingUI.localization.format("Search");
			PlayerDashboardCraftingUI.searchButton.tooltip = PlayerDashboardCraftingUI.localization.format("Search_Tooltip");
			SleekButton sleekButton2 = PlayerDashboardCraftingUI.searchButton;
			if (PlayerDashboardCraftingUI.<>f__mg$cache4 == null)
			{
				PlayerDashboardCraftingUI.<>f__mg$cache4 = new ClickedButton(PlayerDashboardCraftingUI.onClickedSearchButton);
			}
			sleekButton2.onClickedButton = PlayerDashboardCraftingUI.<>f__mg$cache4;
			PlayerDashboardCraftingUI.backdropBox.add(PlayerDashboardCraftingUI.searchButton);
			PlayerDashboardCraftingUI.infoBox = new SleekBox();
			PlayerDashboardCraftingUI.infoBox.positionOffset_X = 10;
			PlayerDashboardCraftingUI.infoBox.positionOffset_Y = 110;
			PlayerDashboardCraftingUI.infoBox.sizeOffset_X = -20;
			PlayerDashboardCraftingUI.infoBox.sizeOffset_Y = 50;
			PlayerDashboardCraftingUI.infoBox.sizeScale_X = 1f;
			PlayerDashboardCraftingUI.infoBox.text = PlayerDashboardCraftingUI.localization.format("No_Blueprints");
			PlayerDashboardCraftingUI.infoBox.fontSize = 14;
			PlayerDashboardCraftingUI.backdropBox.add(PlayerDashboardCraftingUI.infoBox);
			PlayerDashboardCraftingUI.infoBox.isVisible = false;
			PlayerDashboardCraftingUI.viewBlueprints = null;
			PlayerDashboardCraftingUI.selectedType = 0;
			PlayerDashboardCraftingUI.hideUncraftable = false;
			PlayerDashboardCraftingUI.searchText = string.Empty;
			PlayerInventory inventory = Player.player.inventory;
			Delegate onInventoryResized = inventory.onInventoryResized;
			if (PlayerDashboardCraftingUI.<>f__mg$cache5 == null)
			{
				PlayerDashboardCraftingUI.<>f__mg$cache5 = new InventoryResized(PlayerDashboardCraftingUI.onInventoryResized);
			}
			inventory.onInventoryResized = (InventoryResized)Delegate.Combine(onInventoryResized, PlayerDashboardCraftingUI.<>f__mg$cache5);
			PlayerCrafting crafting = Player.player.crafting;
			Delegate onCraftingUpdated = crafting.onCraftingUpdated;
			if (PlayerDashboardCraftingUI.<>f__mg$cache6 == null)
			{
				PlayerDashboardCraftingUI.<>f__mg$cache6 = new CraftingUpdated(PlayerDashboardCraftingUI.onCraftingUpdated);
			}
			crafting.onCraftingUpdated = (CraftingUpdated)Delegate.Combine(onCraftingUpdated, PlayerDashboardCraftingUI.<>f__mg$cache6);
		}

		public static void open()
		{
			if (PlayerDashboardCraftingUI.active)
			{
				return;
			}
			PlayerDashboardCraftingUI.active = true;
			PlayerDashboardCraftingUI.updateSelection(PlayerDashboardCraftingUI.viewBlueprints, PlayerDashboardCraftingUI.selectedType, PlayerDashboardCraftingUI.hideUncraftable, PlayerDashboardCraftingUI.searchText);
			PlayerDashboardCraftingUI.container.lerpPositionScale(0f, 0f, ESleekLerp.EXPONENTIAL, 20f);
		}

		public static void close()
		{
			if (!PlayerDashboardCraftingUI.active)
			{
				return;
			}
			PlayerDashboardCraftingUI.active = false;
			PlayerDashboardCraftingUI.viewBlueprints = null;
			PlayerDashboardCraftingUI.container.lerpPositionScale(0f, 1f, ESleekLerp.EXPONENTIAL, 20f);
		}

		private static bool searchBlueprintText(Blueprint blueprint, string text)
		{
			byte b = 0;
			while ((int)b < blueprint.outputs.Length)
			{
				BlueprintOutput blueprintOutput = blueprint.outputs[(int)b];
				ItemAsset itemAsset = (ItemAsset)Assets.find(EAssetType.ITEM, blueprintOutput.id);
				if (itemAsset != null && itemAsset.itemName != null && itemAsset.itemName.IndexOf(text, StringComparison.OrdinalIgnoreCase) != -1)
				{
					return true;
				}
				b += 1;
			}
			if (blueprint.tool != 0)
			{
				ItemAsset itemAsset2 = (ItemAsset)Assets.find(EAssetType.ITEM, blueprint.tool);
				if (itemAsset2 != null && itemAsset2.itemName != null && itemAsset2.itemName.IndexOf(text, StringComparison.OrdinalIgnoreCase) != -1)
				{
					return true;
				}
			}
			byte b2 = 0;
			while ((int)b2 < blueprint.supplies.Length)
			{
				BlueprintSupply blueprintSupply = blueprint.supplies[(int)b2];
				ItemAsset itemAsset3 = (ItemAsset)Assets.find(EAssetType.ITEM, blueprintSupply.id);
				if (itemAsset3 != null && itemAsset3.itemName != null && itemAsset3.itemName.IndexOf(text, StringComparison.OrdinalIgnoreCase) != -1)
				{
					return true;
				}
				b2 += 1;
			}
			return false;
		}

		public static void updateSelection()
		{
			PlayerDashboardCraftingUI.updateSelection(PlayerDashboardCraftingUI.viewBlueprints, PlayerDashboardCraftingUI.selectedType, PlayerDashboardCraftingUI.hideUncraftable, PlayerDashboardCraftingUI.searchText);
		}

		private static void updateSelection(Blueprint[] view, byte typeIndex, bool uncraftable, string search)
		{
			bool flag = PowerTool.checkFires(Player.player.transform.position, 16f);
			List<Blueprint> list;
			if (view == null)
			{
				list = new List<Blueprint>();
				foreach (ItemAsset itemAsset in Assets.find(EAssetType.ITEM))
				{
					if (itemAsset != null)
					{
						for (int j = 0; j < itemAsset.blueprints.Count; j++)
						{
							Blueprint blueprint = itemAsset.blueprints[j];
							if ((search.Length <= 0) ? (blueprint.type == (EBlueprintType)typeIndex) : PlayerDashboardCraftingUI.searchBlueprintText(blueprint, search))
							{
								list.Add(blueprint);
							}
						}
					}
				}
			}
			else
			{
				list = new List<Blueprint>(view);
			}
			List<Blueprint> list2 = new List<Blueprint>();
			for (int k = 0; k < list.Count; k++)
			{
				Blueprint blueprint2 = list[k];
				if (blueprint2.skill != EBlueprintSkill.REPAIR || (uint)blueprint2.level <= Provider.modeConfigData.Gameplay.Repair_Level_Max)
				{
					if (string.IsNullOrEmpty(blueprint2.map) || blueprint2.map.Equals(Level.info.name, StringComparison.InvariantCultureIgnoreCase))
					{
						ItemAsset itemAsset2 = (ItemAsset)Assets.find(EAssetType.ITEM, blueprint2.source);
						ushort num = 0;
						bool flag2 = true;
						blueprint2.hasSupplies = true;
						blueprint2.hasSkills = (blueprint2.skill == EBlueprintSkill.NONE || (blueprint2.skill == EBlueprintSkill.CRAFT && Player.player.skills.skills[2][1].level >= blueprint2.level) || (blueprint2.skill == EBlueprintSkill.COOK && flag && Player.player.skills.skills[2][3].level >= blueprint2.level) || (blueprint2.skill == EBlueprintSkill.REPAIR && Player.player.skills.skills[2][7].level >= blueprint2.level));
						List<InventorySearch>[] array2 = new List<InventorySearch>[blueprint2.supplies.Length];
						byte b = 0;
						while ((int)b < blueprint2.supplies.Length)
						{
							BlueprintSupply blueprintSupply = blueprint2.supplies[(int)b];
							List<InventorySearch> list3 = Player.player.inventory.search(blueprintSupply.id, false, true);
							ushort num2 = 0;
							byte b2 = 0;
							while ((int)b2 < list3.Count)
							{
								num2 += (ushort)list3[(int)b2].jar.item.amount;
								b2 += 1;
							}
							num += num2;
							blueprintSupply.hasAmount = num2;
							if (blueprint2.type == EBlueprintType.AMMO)
							{
								if (blueprintSupply.hasAmount == 0)
								{
									blueprint2.hasSupplies = false;
								}
							}
							else if (blueprintSupply.hasAmount < blueprintSupply.amount)
							{
								blueprint2.hasSupplies = false;
							}
							if (blueprintSupply.hasAmount < blueprintSupply.amount && blueprintSupply.isCritical)
							{
								flag2 = false;
							}
							array2[(int)b] = list3;
							b += 1;
						}
						if (blueprint2.tool != 0)
						{
							InventorySearch inventorySearch = Player.player.inventory.has(blueprint2.tool);
							blueprint2.tools = ((inventorySearch == null) ? 0 : 1);
							blueprint2.hasTool = (inventorySearch != null);
							if (inventorySearch == null && blueprint2.toolCritical)
							{
								flag2 = false;
							}
						}
						else
						{
							blueprint2.tools = 1;
							blueprint2.hasTool = true;
						}
						if (!flag2)
						{
							num = 0;
						}
						if (blueprint2.type == EBlueprintType.REPAIR)
						{
							List<InventorySearch> list4 = Player.player.inventory.search(itemAsset2.id, false, false);
							byte b3 = byte.MaxValue;
							byte b4 = byte.MaxValue;
							byte b5 = 0;
							while ((int)b5 < list4.Count)
							{
								if (list4[(int)b5].jar.item.quality < b3)
								{
									b3 = list4[(int)b5].jar.item.quality;
									b4 = b5;
								}
								b5 += 1;
							}
							if (b4 != 255)
							{
								blueprint2.items = (ushort)list4[(int)b4].jar.item.quality;
								num += 1;
							}
							else
							{
								blueprint2.items = 0;
							}
							blueprint2.hasItem = (b4 != byte.MaxValue);
						}
						else if (blueprint2.type == EBlueprintType.AMMO)
						{
							List<InventorySearch> list5 = Player.player.inventory.search(itemAsset2.id, true, true);
							int num3 = -1;
							byte b6 = byte.MaxValue;
							byte b7 = 0;
							while ((int)b7 < list5.Count)
							{
								if ((int)list5[(int)b7].jar.item.amount > num3 && list5[(int)b7].jar.item.amount < itemAsset2.amount)
								{
									num3 = (int)list5[(int)b7].jar.item.amount;
									b6 = b7;
								}
								b7 += 1;
							}
							if (b6 != 255)
							{
								if (list5[(int)b6].jar.item.id == blueprint2.supplies[0].id)
								{
									BlueprintSupply blueprintSupply2 = blueprint2.supplies[0];
									blueprintSupply2.hasAmount -= (ushort)num3;
								}
								blueprint2.supplies[0].amount = (ushort)((byte)((int)itemAsset2.amount - num3));
								blueprint2.items = (ushort)list5[(int)b6].jar.item.amount;
								num += 1;
							}
							else
							{
								blueprint2.supplies[0].amount = 0;
								blueprint2.items = 0;
							}
							blueprint2.hasItem = (b6 != byte.MaxValue);
							if (b6 == 255)
							{
								blueprint2.products = 0;
							}
							else if (blueprint2.items + blueprint2.supplies[0].hasAmount > (ushort)itemAsset2.amount)
							{
								blueprint2.products = (ushort)itemAsset2.amount;
							}
							else
							{
								blueprint2.products = blueprint2.items + blueprint2.supplies[0].hasAmount;
							}
						}
						else
						{
							blueprint2.hasItem = true;
						}
						if (uncraftable)
						{
							if (blueprint2.hasSupplies && blueprint2.hasTool && blueprint2.hasItem && blueprint2.hasSkills)
							{
								list2.Add(blueprint2);
							}
						}
						else if (view != null)
						{
							if (blueprint2.hasSupplies && blueprint2.hasTool && blueprint2.hasItem && blueprint2.hasSkills)
							{
								list2.Insert(0, blueprint2);
							}
							else
							{
								list2.Add(blueprint2);
							}
						}
						else if (blueprint2.hasSupplies && blueprint2.hasTool && blueprint2.hasItem && blueprint2.hasSkills)
						{
							list2.Insert(0, blueprint2);
						}
						else if ((blueprint2.type == EBlueprintType.AMMO || blueprint2.type == EBlueprintType.REPAIR || num != 0) && blueprint2.hasItem)
						{
							list2.Add(blueprint2);
						}
					}
				}
			}
			PlayerDashboardCraftingUI.viewBlueprints = view;
			PlayerDashboardCraftingUI.selectedType = typeIndex;
			PlayerDashboardCraftingUI.hideUncraftable = uncraftable;
			PlayerDashboardCraftingUI.searchText = search;
			PlayerDashboardCraftingUI.blueprints = list2.ToArray();
			PlayerDashboardCraftingUI.blueprintsScrollBox.remove();
			PlayerDashboardCraftingUI.blueprintsScrollBox.area = new Rect(0f, 0f, 5f, (float)(PlayerDashboardCraftingUI.blueprints.Length * 205 - 10));
			for (int l = 0; l < PlayerDashboardCraftingUI.blueprints.Length; l++)
			{
				Blueprint newBlueprint = PlayerDashboardCraftingUI.blueprints[l];
				SleekBlueprint sleekBlueprint = new SleekBlueprint(newBlueprint);
				sleekBlueprint.positionOffset_Y = l * 205;
				sleekBlueprint.sizeOffset_X = -30;
				sleekBlueprint.sizeOffset_Y = 195;
				sleekBlueprint.sizeScale_X = 1f;
				SleekButton sleekButton = sleekBlueprint;
				if (PlayerDashboardCraftingUI.<>f__mg$cache0 == null)
				{
					PlayerDashboardCraftingUI.<>f__mg$cache0 = new ClickedButton(PlayerDashboardCraftingUI.onClickedBlueprintButton);
				}
				sleekButton.onClickedButton = PlayerDashboardCraftingUI.<>f__mg$cache0;
				PlayerDashboardCraftingUI.blueprintsScrollBox.add(sleekBlueprint);
			}
			PlayerDashboardCraftingUI.infoBox.isVisible = (PlayerDashboardCraftingUI.blueprints.Length == 0);
		}

		private static void onInventoryResized(byte page, byte newWidth, byte newHeight)
		{
			if (PlayerDashboardCraftingUI.active)
			{
				PlayerDashboardCraftingUI.updateSelection();
			}
		}

		private static void onCraftingUpdated()
		{
			if (PlayerDashboardCraftingUI.active)
			{
				PlayerDashboardCraftingUI.updateSelection();
			}
		}

		private static void onClickedTypeButton(SleekButton button)
		{
			byte typeIndex = (byte)((button.positionOffset_X + -(PlayerDashboardCraftingUI.TYPES * -30 + 5)) / 60);
			PlayerDashboardCraftingUI.searchField.text = string.Empty;
			PlayerDashboardCraftingUI.updateSelection(null, typeIndex, PlayerDashboardCraftingUI.hideUncraftable, string.Empty);
		}

		private static void onToggledHideUncraftableToggle(SleekToggle toggle, bool state)
		{
			PlayerDashboardCraftingUI.updateSelection(PlayerDashboardCraftingUI.viewBlueprints, PlayerDashboardCraftingUI.selectedType, state, PlayerDashboardCraftingUI.searchText);
		}

		private static void onEnteredSearchField(SleekField field)
		{
			PlayerDashboardCraftingUI.updateSelection(null, PlayerDashboardCraftingUI.selectedType, PlayerDashboardCraftingUI.hideUncraftable, PlayerDashboardCraftingUI.searchField.text);
		}

		private static void onClickedSearchButton(SleekButton button)
		{
			PlayerDashboardCraftingUI.updateSelection(null, PlayerDashboardCraftingUI.selectedType, PlayerDashboardCraftingUI.hideUncraftable, PlayerDashboardCraftingUI.searchField.text);
		}

		private static void onClickedBlueprintButton(SleekButton button)
		{
			byte b = (byte)(button.positionOffset_Y / 205);
			Blueprint blueprint = PlayerDashboardCraftingUI.blueprints[(int)b];
			if (!blueprint.hasSupplies)
			{
				return;
			}
			if (!blueprint.hasTool)
			{
				return;
			}
			if (!blueprint.hasItem)
			{
				return;
			}
			if (!blueprint.hasSkills)
			{
				return;
			}
			if (Player.player.equipment.isBusy)
			{
				return;
			}
			Player.player.crafting.sendCraft(PlayerDashboardCraftingUI.blueprints[(int)b].source, PlayerDashboardCraftingUI.blueprints[(int)b].id, Input.GetKey(ControlsSettings.other));
		}

		private static readonly int TYPES = 10;

		public static Local localization;

		private static Sleek container;

		public static Bundle icons;

		public static bool active;

		private static SleekBox backdropBox;

		private static SleekField searchField;

		private static SleekButton searchButton;

		private static Blueprint[] blueprints;

		private static SleekScrollBox blueprintsScrollBox;

		private static SleekBox infoBox;

		private static SleekToggle hideUncraftableToggle;

		public static Blueprint[] viewBlueprints;

		private static byte selectedType;

		private static bool hideUncraftable;

		private static string searchText;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache0;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache1;

		[CompilerGenerated]
		private static Toggled <>f__mg$cache2;

		[CompilerGenerated]
		private static Entered <>f__mg$cache3;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache4;

		[CompilerGenerated]
		private static InventoryResized <>f__mg$cache5;

		[CompilerGenerated]
		private static CraftingUpdated <>f__mg$cache6;
	}
}
