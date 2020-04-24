using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace SDG.Unturned
{
	public class MenuSurvivorsGroupUI
	{
		public MenuSurvivorsGroupUI()
		{
			MenuSurvivorsGroupUI.localization = Localization.read("/Menu/Survivors/MenuSurvivorsGroup.dat");
			MenuSurvivorsGroupUI.container = new Sleek();
			MenuSurvivorsGroupUI.container.positionOffset_X = 10;
			MenuSurvivorsGroupUI.container.positionOffset_Y = 10;
			MenuSurvivorsGroupUI.container.positionScale_Y = 1f;
			MenuSurvivorsGroupUI.container.sizeOffset_X = -20;
			MenuSurvivorsGroupUI.container.sizeOffset_Y = -20;
			MenuSurvivorsGroupUI.container.sizeScale_X = 1f;
			MenuSurvivorsGroupUI.container.sizeScale_Y = 1f;
			MenuUI.container.add(MenuSurvivorsGroupUI.container);
			MenuSurvivorsGroupUI.active = false;
			MenuSurvivorsGroupUI.groups = Provider.provider.communityService.getGroups();
			MenuSurvivorsGroupUI.groupButton = new SleekButtonIcon(null, 20);
			MenuSurvivorsGroupUI.groupButton.positionOffset_X = -100;
			MenuSurvivorsGroupUI.groupButton.positionOffset_Y = 100;
			MenuSurvivorsGroupUI.groupButton.positionScale_X = 0.75f;
			MenuSurvivorsGroupUI.groupButton.sizeOffset_X = 200;
			MenuSurvivorsGroupUI.groupButton.sizeOffset_Y = 30;
			MenuSurvivorsGroupUI.groupButton.addLabel(MenuSurvivorsGroupUI.localization.format("Group_Box_Label"), ESleekSide.LEFT);
			SleekButton sleekButton = MenuSurvivorsGroupUI.groupButton;
			if (MenuSurvivorsGroupUI.<>f__mg$cache0 == null)
			{
				MenuSurvivorsGroupUI.<>f__mg$cache0 = new ClickedButton(MenuSurvivorsGroupUI.onClickedUngroupButton);
			}
			sleekButton.onClickedButton = MenuSurvivorsGroupUI.<>f__mg$cache0;
			MenuSurvivorsGroupUI.container.add(MenuSurvivorsGroupUI.groupButton);
			MenuSurvivorsGroupUI.groupsBox = new SleekScrollBox();
			MenuSurvivorsGroupUI.groupsBox.positionOffset_X = -100;
			MenuSurvivorsGroupUI.groupsBox.positionOffset_Y = 140;
			MenuSurvivorsGroupUI.groupsBox.positionScale_X = 0.75f;
			MenuSurvivorsGroupUI.groupsBox.sizeOffset_X = 230;
			MenuSurvivorsGroupUI.groupsBox.sizeOffset_Y = -240;
			MenuSurvivorsGroupUI.groupsBox.sizeScale_Y = 1f;
			MenuSurvivorsGroupUI.groupsBox.area = new Rect(0f, 0f, 5f, (float)(MenuSurvivorsGroupUI.groups.Length * 40 - 10));
			MenuSurvivorsGroupUI.container.add(MenuSurvivorsGroupUI.groupsBox);
			for (int i = 0; i < MenuSurvivorsGroupUI.groups.Length; i++)
			{
				SleekButtonIcon sleekButtonIcon = new SleekButtonIcon(MenuSurvivorsGroupUI.groups[i].icon, 20);
				sleekButtonIcon.positionOffset_Y = i * 40;
				sleekButtonIcon.sizeOffset_X = 200;
				sleekButtonIcon.sizeOffset_Y = 30;
				sleekButtonIcon.text = MenuSurvivorsGroupUI.groups[i].name;
				SleekButton sleekButton2 = sleekButtonIcon;
				if (MenuSurvivorsGroupUI.<>f__mg$cache1 == null)
				{
					MenuSurvivorsGroupUI.<>f__mg$cache1 = new ClickedButton(MenuSurvivorsGroupUI.onClickedGroupButton);
				}
				sleekButton2.onClickedButton = MenuSurvivorsGroupUI.<>f__mg$cache1;
				MenuSurvivorsGroupUI.groupsBox.add(sleekButtonIcon);
			}
			Delegate onCharacterUpdated = Characters.onCharacterUpdated;
			if (MenuSurvivorsGroupUI.<>f__mg$cache2 == null)
			{
				MenuSurvivorsGroupUI.<>f__mg$cache2 = new CharacterUpdated(MenuSurvivorsGroupUI.onCharacterUpdated);
			}
			Characters.onCharacterUpdated = (CharacterUpdated)Delegate.Combine(onCharacterUpdated, MenuSurvivorsGroupUI.<>f__mg$cache2);
			MenuSurvivorsGroupUI.backButton = new SleekButtonIcon((Texture2D)MenuDashboardUI.icons.load("Exit"));
			MenuSurvivorsGroupUI.backButton.positionOffset_Y = -50;
			MenuSurvivorsGroupUI.backButton.positionScale_Y = 1f;
			MenuSurvivorsGroupUI.backButton.sizeOffset_X = 200;
			MenuSurvivorsGroupUI.backButton.sizeOffset_Y = 50;
			MenuSurvivorsGroupUI.backButton.text = MenuDashboardUI.localization.format("BackButtonText");
			MenuSurvivorsGroupUI.backButton.tooltip = MenuDashboardUI.localization.format("BackButtonTooltip");
			SleekButton sleekButton3 = MenuSurvivorsGroupUI.backButton;
			if (MenuSurvivorsGroupUI.<>f__mg$cache3 == null)
			{
				MenuSurvivorsGroupUI.<>f__mg$cache3 = new ClickedButton(MenuSurvivorsGroupUI.onClickedBackButton);
			}
			sleekButton3.onClickedButton = MenuSurvivorsGroupUI.<>f__mg$cache3;
			MenuSurvivorsGroupUI.backButton.fontSize = 14;
			MenuSurvivorsGroupUI.backButton.iconImage.backgroundTint = ESleekTint.FOREGROUND;
			MenuSurvivorsGroupUI.container.add(MenuSurvivorsGroupUI.backButton);
		}

		public static void open()
		{
			if (MenuSurvivorsGroupUI.active)
			{
				return;
			}
			MenuSurvivorsGroupUI.active = true;
			MenuSurvivorsGroupUI.container.lerpPositionScale(0f, 0f, ESleekLerp.EXPONENTIAL, 20f);
		}

		public static void close()
		{
			if (!MenuSurvivorsGroupUI.active)
			{
				return;
			}
			MenuSurvivorsGroupUI.active = false;
			MenuSurvivorsGroupUI.container.lerpPositionScale(0f, 1f, ESleekLerp.EXPONENTIAL, 20f);
		}

		private static void onCharacterUpdated(byte index, Character character)
		{
			if (index == Characters.selected)
			{
				for (int i = 0; i < MenuSurvivorsGroupUI.groups.Length; i++)
				{
					if (MenuSurvivorsGroupUI.groups[i].steamID == character.group)
					{
						MenuSurvivorsGroupUI.groupButton.text = MenuSurvivorsGroupUI.groups[i].name;
						MenuSurvivorsGroupUI.groupButton.icon = MenuSurvivorsGroupUI.groups[i].icon;
						return;
					}
				}
				MenuSurvivorsGroupUI.groupButton.text = MenuSurvivorsGroupUI.localization.format("Group_Box");
				MenuSurvivorsGroupUI.groupButton.icon = null;
			}
		}

		private static void onTypedNickField(SleekField field, string text)
		{
			Characters.renick(text);
		}

		private static void onClickedGroupButton(SleekButton button)
		{
			Characters.group(MenuSurvivorsGroupUI.groups[button.positionOffset_Y / 40].steamID);
		}

		private static void onClickedUngroupButton(SleekButton button)
		{
			Characters.ungroup();
		}

		private static void onClickedBackButton(SleekButton button)
		{
			MenuSurvivorsUI.open();
			MenuSurvivorsGroupUI.close();
		}

		private static Local localization;

		private static Sleek container;

		public static bool active;

		private static SleekButtonIcon backButton;

		private static SteamGroup[] groups;

		private static SleekButtonIcon groupButton;

		private static SleekScrollBox groupsBox;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache0;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache1;

		[CompilerGenerated]
		private static CharacterUpdated <>f__mg$cache2;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache3;
	}
}
