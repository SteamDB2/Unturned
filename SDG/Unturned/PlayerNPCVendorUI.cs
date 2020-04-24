using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace SDG.Unturned
{
	public class PlayerNPCVendorUI
	{
		public PlayerNPCVendorUI()
		{
			PlayerNPCVendorUI.localization = Localization.read("/Player/PlayerNPCVendor.dat");
			PlayerNPCVendorUI.container = new Sleek();
			PlayerNPCVendorUI.container.positionScale_Y = 1f;
			PlayerNPCVendorUI.container.positionOffset_X = 10;
			PlayerNPCVendorUI.container.positionOffset_Y = 10;
			PlayerNPCVendorUI.container.sizeOffset_X = -20;
			PlayerNPCVendorUI.container.sizeOffset_Y = -20;
			PlayerNPCVendorUI.container.sizeScale_X = 1f;
			PlayerNPCVendorUI.container.sizeScale_Y = 1f;
			PlayerUI.container.add(PlayerNPCVendorUI.container);
			PlayerNPCVendorUI.active = false;
			PlayerNPCVendorUI.vendorBox = new SleekBox();
			PlayerNPCVendorUI.vendorBox.sizeOffset_Y = -60;
			PlayerNPCVendorUI.vendorBox.sizeScale_X = 1f;
			PlayerNPCVendorUI.vendorBox.sizeScale_Y = 1f;
			PlayerNPCVendorUI.container.add(PlayerNPCVendorUI.vendorBox);
			PlayerNPCVendorUI.nameLabel = new SleekLabel();
			PlayerNPCVendorUI.nameLabel.positionOffset_X = 5;
			PlayerNPCVendorUI.nameLabel.positionOffset_Y = 5;
			PlayerNPCVendorUI.nameLabel.sizeOffset_X = -10;
			PlayerNPCVendorUI.nameLabel.sizeOffset_Y = 40;
			PlayerNPCVendorUI.nameLabel.sizeScale_X = 1f;
			PlayerNPCVendorUI.nameLabel.foregroundTint = ESleekTint.NONE;
			PlayerNPCVendorUI.nameLabel.isRich = true;
			PlayerNPCVendorUI.nameLabel.fontSize = 24;
			PlayerNPCVendorUI.vendorBox.add(PlayerNPCVendorUI.nameLabel);
			PlayerNPCVendorUI.descriptionLabel = new SleekLabel();
			PlayerNPCVendorUI.descriptionLabel.positionOffset_X = 5;
			PlayerNPCVendorUI.descriptionLabel.positionOffset_Y = 40;
			PlayerNPCVendorUI.descriptionLabel.sizeOffset_X = -10;
			PlayerNPCVendorUI.descriptionLabel.sizeOffset_Y = 40;
			PlayerNPCVendorUI.descriptionLabel.sizeScale_X = 1f;
			PlayerNPCVendorUI.descriptionLabel.foregroundTint = ESleekTint.NONE;
			PlayerNPCVendorUI.descriptionLabel.isRich = true;
			PlayerNPCVendorUI.vendorBox.add(PlayerNPCVendorUI.descriptionLabel);
			PlayerNPCVendorUI.buyingLabel = new SleekLabel();
			PlayerNPCVendorUI.buyingLabel.positionOffset_X = 5;
			PlayerNPCVendorUI.buyingLabel.positionOffset_Y = 80;
			PlayerNPCVendorUI.buyingLabel.sizeOffset_X = -40;
			PlayerNPCVendorUI.buyingLabel.sizeOffset_Y = 30;
			PlayerNPCVendorUI.buyingLabel.sizeScale_X = 0.5f;
			PlayerNPCVendorUI.buyingLabel.fontSize = 14;
			PlayerNPCVendorUI.buyingLabel.text = PlayerNPCVendorUI.localization.format("Buying");
			PlayerNPCVendorUI.vendorBox.add(PlayerNPCVendorUI.buyingLabel);
			PlayerNPCVendorUI.buyingBox = new SleekScrollBox();
			PlayerNPCVendorUI.buyingBox.positionOffset_X = 5;
			PlayerNPCVendorUI.buyingBox.positionOffset_Y = 115;
			PlayerNPCVendorUI.buyingBox.sizeOffset_X = -10;
			PlayerNPCVendorUI.buyingBox.sizeOffset_Y = -120;
			PlayerNPCVendorUI.buyingBox.sizeScale_X = 0.5f;
			PlayerNPCVendorUI.buyingBox.sizeScale_Y = 1f;
			PlayerNPCVendorUI.buyingBox.area = new Rect(0f, 0f, 5f, 1024f);
			PlayerNPCVendorUI.vendorBox.add(PlayerNPCVendorUI.buyingBox);
			PlayerNPCVendorUI.sellingLabel = new SleekLabel();
			PlayerNPCVendorUI.sellingLabel.positionOffset_X = 5;
			PlayerNPCVendorUI.sellingLabel.positionOffset_Y = 80;
			PlayerNPCVendorUI.sellingLabel.positionScale_X = 0.5f;
			PlayerNPCVendorUI.sellingLabel.sizeOffset_X = -40;
			PlayerNPCVendorUI.sellingLabel.sizeOffset_Y = 30;
			PlayerNPCVendorUI.sellingLabel.sizeScale_X = 0.5f;
			PlayerNPCVendorUI.sellingLabel.fontSize = 14;
			PlayerNPCVendorUI.sellingLabel.text = PlayerNPCVendorUI.localization.format("Selling");
			PlayerNPCVendorUI.vendorBox.add(PlayerNPCVendorUI.sellingLabel);
			PlayerNPCVendorUI.sellingBox = new SleekScrollBox();
			PlayerNPCVendorUI.sellingBox.positionOffset_X = 5;
			PlayerNPCVendorUI.sellingBox.positionOffset_Y = 115;
			PlayerNPCVendorUI.sellingBox.positionScale_X = 0.5f;
			PlayerNPCVendorUI.sellingBox.sizeOffset_X = -10;
			PlayerNPCVendorUI.sellingBox.sizeOffset_Y = -120;
			PlayerNPCVendorUI.sellingBox.sizeScale_X = 0.5f;
			PlayerNPCVendorUI.sellingBox.sizeScale_Y = 1f;
			PlayerNPCVendorUI.sellingBox.area = new Rect(0f, 0f, 5f, 1024f);
			PlayerNPCVendorUI.vendorBox.add(PlayerNPCVendorUI.sellingBox);
			PlayerNPCVendorUI.experienceBox = new SleekBox();
			PlayerNPCVendorUI.experienceBox.positionOffset_Y = 10;
			PlayerNPCVendorUI.experienceBox.positionScale_Y = 1f;
			PlayerNPCVendorUI.experienceBox.sizeOffset_X = -5;
			PlayerNPCVendorUI.experienceBox.sizeOffset_Y = 50;
			PlayerNPCVendorUI.experienceBox.sizeScale_X = 0.5f;
			PlayerNPCVendorUI.experienceBox.fontSize = 14;
			PlayerNPCVendorUI.experienceBox.foregroundColor = Palette.COLOR_Y;
			PlayerNPCVendorUI.experienceBox.foregroundTint = ESleekTint.NONE;
			PlayerNPCVendorUI.vendorBox.add(PlayerNPCVendorUI.experienceBox);
			PlayerNPCVendorUI.returnButton = new SleekButton();
			PlayerNPCVendorUI.returnButton.positionOffset_X = 5;
			PlayerNPCVendorUI.returnButton.positionOffset_Y = 10;
			PlayerNPCVendorUI.returnButton.positionScale_X = 0.5f;
			PlayerNPCVendorUI.returnButton.positionScale_Y = 1f;
			PlayerNPCVendorUI.returnButton.sizeOffset_X = -5;
			PlayerNPCVendorUI.returnButton.sizeOffset_Y = 50;
			PlayerNPCVendorUI.returnButton.sizeScale_X = 0.5f;
			PlayerNPCVendorUI.returnButton.fontSize = 14;
			PlayerNPCVendorUI.returnButton.text = PlayerNPCVendorUI.localization.format("Return");
			PlayerNPCVendorUI.returnButton.tooltip = PlayerNPCVendorUI.localization.format("Return_Tooltip");
			SleekButton sleekButton = PlayerNPCVendorUI.returnButton;
			if (PlayerNPCVendorUI.<>f__mg$cache2 == null)
			{
				PlayerNPCVendorUI.<>f__mg$cache2 = new ClickedButton(PlayerNPCVendorUI.onClickedReturnButton);
			}
			sleekButton.onClickedButton = PlayerNPCVendorUI.<>f__mg$cache2;
			PlayerNPCVendorUI.vendorBox.add(PlayerNPCVendorUI.returnButton);
			PlayerSkills skills = Player.player.skills;
			Delegate onExperienceUpdated = skills.onExperienceUpdated;
			if (PlayerNPCVendorUI.<>f__mg$cache3 == null)
			{
				PlayerNPCVendorUI.<>f__mg$cache3 = new ExperienceUpdated(PlayerNPCVendorUI.onExperienceUpdated);
			}
			skills.onExperienceUpdated = (ExperienceUpdated)Delegate.Combine(onExperienceUpdated, PlayerNPCVendorUI.<>f__mg$cache3);
		}

		public static void open(VendorAsset newVendor, DialogueResponse newResponse, DialogueAsset newDialogue, DialogueAsset newPrevDialogue)
		{
			if (PlayerNPCVendorUI.active)
			{
				return;
			}
			PlayerNPCVendorUI.active = true;
			PlayerNPCVendorUI.updateVendor(newVendor, newResponse, newDialogue, newPrevDialogue);
			PlayerNPCVendorUI.container.lerpPositionScale(0f, 0f, ESleekLerp.EXPONENTIAL, 20f);
		}

		public static void close()
		{
			if (!PlayerNPCVendorUI.active)
			{
				return;
			}
			PlayerNPCVendorUI.active = false;
			PlayerNPCVendorUI.container.lerpPositionScale(0f, 1f, ESleekLerp.EXPONENTIAL, 20f);
		}

		public static void closeNicely()
		{
			PlayerNPCVendorUI.close();
			PlayerNPCDialogueUI.open(PlayerNPCVendorUI.prevDialogue, null);
		}

		private static void updateVendor(VendorAsset newVendor, DialogueResponse newResponse, DialogueAsset newDialogue, DialogueAsset newPrevDialogue)
		{
			PlayerNPCVendorUI.vendor = newVendor;
			PlayerNPCVendorUI.response = newResponse;
			PlayerNPCVendorUI.dialogue = newDialogue;
			PlayerNPCVendorUI.prevDialogue = newPrevDialogue;
			if (PlayerNPCVendorUI.vendor == null)
			{
				return;
			}
			PlayerNPCVendorUI.nameLabel.text = PlayerNPCVendorUI.vendor.vendorName;
			string text = PlayerNPCVendorUI.vendor.vendorDescription;
			text = text.Replace("<br>", "\n");
			PlayerNPCVendorUI.descriptionLabel.text = text;
			PlayerNPCVendorUI.buttons.Clear();
			PlayerNPCVendorUI.buying.Clear();
			byte b = 0;
			while ((int)b < PlayerNPCVendorUI.vendor.buying.Length)
			{
				VendorBuying vendorBuying = PlayerNPCVendorUI.vendor.buying[(int)b];
				if (vendorBuying.areConditionsMet(Player.player))
				{
					PlayerNPCVendorUI.buying.Add(vendorBuying);
				}
				b += 1;
			}
			PlayerNPCVendorUI.buying.Sort(PlayerNPCVendorUI.buyingComparator);
			PlayerNPCVendorUI.buyingBox.isVisible = (PlayerNPCVendorUI.buying.Count > 0);
			PlayerNPCVendorUI.buyingBox.remove();
			int num = 0;
			byte b2 = 0;
			while ((int)b2 < PlayerNPCVendorUI.buying.Count)
			{
				VendorBuying newElement = PlayerNPCVendorUI.buying[(int)b2];
				SleekVendor sleekVendor = new SleekVendor(newElement);
				sleekVendor.positionOffset_Y = num;
				sleekVendor.sizeOffset_X = -30;
				sleekVendor.sizeScale_X = 1f;
				SleekButton sleekButton = sleekVendor;
				if (PlayerNPCVendorUI.<>f__mg$cache0 == null)
				{
					PlayerNPCVendorUI.<>f__mg$cache0 = new ClickedButton(PlayerNPCVendorUI.onClickedBuyingButton);
				}
				sleekButton.onClickedButton = PlayerNPCVendorUI.<>f__mg$cache0;
				PlayerNPCVendorUI.buyingBox.add(sleekVendor);
				num += sleekVendor.sizeOffset_Y;
				PlayerNPCVendorUI.buttons.Add(sleekVendor);
				b2 += 1;
			}
			PlayerNPCVendorUI.buyingBox.area = new Rect(0f, 0f, 5f, (float)num);
			PlayerNPCVendorUI.selling.Clear();
			byte b3 = 0;
			while ((int)b3 < PlayerNPCVendorUI.vendor.selling.Length)
			{
				VendorSelling vendorSelling = PlayerNPCVendorUI.vendor.selling[(int)b3];
				if (vendorSelling.areConditionsMet(Player.player))
				{
					PlayerNPCVendorUI.selling.Add(vendorSelling);
				}
				b3 += 1;
			}
			PlayerNPCVendorUI.selling.Sort(PlayerNPCVendorUI.sellingComparator);
			PlayerNPCVendorUI.sellingBox.isVisible = (PlayerNPCVendorUI.selling.Count > 0);
			PlayerNPCVendorUI.sellingBox.remove();
			int num2 = 0;
			byte b4 = 0;
			while ((int)b4 < PlayerNPCVendorUI.selling.Count)
			{
				VendorSelling newElement2 = PlayerNPCVendorUI.selling[(int)b4];
				SleekVendor sleekVendor2 = new SleekVendor(newElement2);
				sleekVendor2.positionOffset_Y = num2;
				sleekVendor2.sizeOffset_X = -30;
				sleekVendor2.sizeScale_X = 1f;
				SleekButton sleekButton2 = sleekVendor2;
				if (PlayerNPCVendorUI.<>f__mg$cache1 == null)
				{
					PlayerNPCVendorUI.<>f__mg$cache1 = new ClickedButton(PlayerNPCVendorUI.onClickedSellingButton);
				}
				sleekButton2.onClickedButton = PlayerNPCVendorUI.<>f__mg$cache1;
				PlayerNPCVendorUI.sellingBox.add(sleekVendor2);
				num2 += sleekVendor2.sizeOffset_Y;
				PlayerNPCVendorUI.buttons.Add(sleekVendor2);
				b4 += 1;
			}
			PlayerNPCVendorUI.sellingBox.area = new Rect(0f, 0f, 5f, (float)num2);
		}

		private static void onExperienceUpdated(uint newExperience)
		{
			PlayerNPCVendorUI.experienceBox.text = PlayerNPCVendorUI.localization.format("Experience", new object[]
			{
				newExperience.ToString()
			});
			for (int i = 0; i < PlayerNPCVendorUI.buttons.Count; i++)
			{
				PlayerNPCVendorUI.buttons[i].updateAmount();
			}
		}

		private static void onClickedBuyingButton(SleekButton button)
		{
			byte index = (byte)PlayerNPCVendorUI.buyingBox.search(button);
			VendorBuying vendorBuying = PlayerNPCVendorUI.buying[(int)index];
			if (!vendorBuying.canSell(Player.player))
			{
				return;
			}
			Player.player.quests.sendSellToVendor(PlayerNPCVendorUI.vendor.id, vendorBuying.index);
		}

		private static void onClickedSellingButton(SleekButton button)
		{
			byte index = (byte)PlayerNPCVendorUI.sellingBox.search(button);
			VendorSelling vendorSelling = PlayerNPCVendorUI.selling[(int)index];
			if (!vendorSelling.canBuy(Player.player))
			{
				return;
			}
			Player.player.quests.sendBuyFromVendor(PlayerNPCVendorUI.vendor.id, vendorSelling.index);
		}

		private static void onClickedReturnButton(SleekButton button)
		{
			PlayerNPCVendorUI.close();
			PlayerNPCDialogueUI.registerResponse(PlayerNPCVendorUI.dialogue, PlayerNPCVendorUI.response);
			PlayerNPCDialogueUI.open(PlayerNPCVendorUI.dialogue, PlayerNPCVendorUI.prevDialogue);
		}

		private static Sleek container;

		public static Local localization;

		public static bool active;

		private static VendorAsset vendor;

		private static DialogueResponse response;

		private static DialogueAsset dialogue;

		private static DialogueAsset prevDialogue;

		private static List<VendorBuying> buying = new List<VendorBuying>();

		private static List<VendorSelling> selling = new List<VendorSelling>();

		private static List<SleekVendor> buttons = new List<SleekVendor>();

		private static VendorBuyingNameAscendingComparator buyingComparator = new VendorBuyingNameAscendingComparator();

		private static VendorSellingNameAscendingComparator sellingComparator = new VendorSellingNameAscendingComparator();

		private static SleekBox vendorBox;

		private static SleekLabel nameLabel;

		private static SleekLabel descriptionLabel;

		private static SleekLabel sellingLabel;

		private static SleekScrollBox sellingBox;

		private static SleekLabel buyingLabel;

		private static SleekScrollBox buyingBox;

		private static SleekBox experienceBox;

		private static SleekButton returnButton;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache0;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache1;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache2;

		[CompilerGenerated]
		private static ExperienceUpdated <>f__mg$cache3;
	}
}
