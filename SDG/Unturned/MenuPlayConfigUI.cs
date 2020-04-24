using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace SDG.Unturned
{
	public class MenuPlayConfigUI
	{
		public MenuPlayConfigUI()
		{
			MenuPlayConfigUI.localization = Localization.read("/Menu/Play/MenuPlayConfig.dat");
			MenuPlayConfigUI.container = new Sleek();
			MenuPlayConfigUI.container.positionOffset_X = 10;
			MenuPlayConfigUI.container.positionOffset_Y = 10;
			MenuPlayConfigUI.container.positionScale_Y = 1f;
			MenuPlayConfigUI.container.sizeOffset_X = -20;
			MenuPlayConfigUI.container.sizeOffset_Y = -20;
			MenuPlayConfigUI.container.sizeScale_X = 1f;
			MenuPlayConfigUI.container.sizeScale_Y = 1f;
			MenuUI.container.add(MenuPlayConfigUI.container);
			MenuPlayConfigUI.active = false;
			MenuPlayConfigUI.configBox = new SleekScrollBox();
			MenuPlayConfigUI.configBox.positionOffset_X = -200;
			MenuPlayConfigUI.configBox.positionOffset_Y = 100;
			MenuPlayConfigUI.configBox.positionScale_X = 0.5f;
			MenuPlayConfigUI.configBox.sizeOffset_X = 430;
			MenuPlayConfigUI.configBox.sizeOffset_Y = -200;
			MenuPlayConfigUI.configBox.sizeScale_Y = 1f;
			MenuPlayConfigUI.container.add(MenuPlayConfigUI.configBox);
			MenuPlayConfigUI.backButton = new SleekButtonIcon((Texture2D)MenuDashboardUI.icons.load("Exit"));
			MenuPlayConfigUI.backButton.positionOffset_Y = -50;
			MenuPlayConfigUI.backButton.positionScale_Y = 1f;
			MenuPlayConfigUI.backButton.sizeOffset_X = 200;
			MenuPlayConfigUI.backButton.sizeOffset_Y = 50;
			MenuPlayConfigUI.backButton.text = MenuDashboardUI.localization.format("BackButtonText");
			MenuPlayConfigUI.backButton.tooltip = MenuDashboardUI.localization.format("BackButtonTooltip");
			SleekButton sleekButton = MenuPlayConfigUI.backButton;
			if (MenuPlayConfigUI.<>f__mg$cache3 == null)
			{
				MenuPlayConfigUI.<>f__mg$cache3 = new ClickedButton(MenuPlayConfigUI.onClickedBackButton);
			}
			sleekButton.onClickedButton = MenuPlayConfigUI.<>f__mg$cache3;
			MenuPlayConfigUI.backButton.fontSize = 14;
			MenuPlayConfigUI.backButton.iconImage.backgroundTint = ESleekTint.FOREGROUND;
			MenuPlayConfigUI.container.add(MenuPlayConfigUI.backButton);
			MenuPlayConfigUI.defaultButton = new SleekButton();
			MenuPlayConfigUI.defaultButton.positionOffset_X = -200;
			MenuPlayConfigUI.defaultButton.positionOffset_Y = -50;
			MenuPlayConfigUI.defaultButton.positionScale_X = 1f;
			MenuPlayConfigUI.defaultButton.positionScale_Y = 1f;
			MenuPlayConfigUI.defaultButton.sizeOffset_X = 200;
			MenuPlayConfigUI.defaultButton.sizeOffset_Y = 50;
			MenuPlayConfigUI.defaultButton.text = MenuPlayConfigUI.localization.format("Default");
			MenuPlayConfigUI.defaultButton.tooltip = MenuPlayConfigUI.localization.format("Default_Tooltip");
			SleekButton sleekButton2 = MenuPlayConfigUI.defaultButton;
			if (MenuPlayConfigUI.<>f__mg$cache4 == null)
			{
				MenuPlayConfigUI.<>f__mg$cache4 = new ClickedButton(MenuPlayConfigUI.onClickedDefaultButton);
			}
			sleekButton2.onClickedButton = MenuPlayConfigUI.<>f__mg$cache4;
			MenuPlayConfigUI.defaultButton.fontSize = 14;
			MenuPlayConfigUI.container.add(MenuPlayConfigUI.defaultButton);
			MenuPlayConfigUI.configGroups = new List<object>();
		}

		public static void open()
		{
			if (MenuPlayConfigUI.active)
			{
				return;
			}
			MenuPlayConfigUI.active = true;
			if (ReadWrite.fileExists("/Worlds/Singleplayer_" + Characters.selected + "/Config.json", false))
			{
				try
				{
					MenuPlayConfigUI.configData = ReadWrite.deserializeJSON<ConfigData>("/Worlds/Singleplayer_" + Characters.selected + "/Config.json", false);
				}
				catch
				{
					MenuPlayConfigUI.configData = null;
				}
				if (MenuPlayConfigUI.configData == null)
				{
					MenuPlayConfigUI.configData = new ConfigData();
				}
			}
			else
			{
				MenuPlayConfigUI.configData = new ConfigData();
			}
			EGameMode singleplayerMode = PlaySettings.singleplayerMode;
			if (singleplayerMode != EGameMode.EASY)
			{
				if (singleplayerMode != EGameMode.NORMAL)
				{
					if (singleplayerMode == EGameMode.HARD)
					{
						MenuPlayConfigUI.modeConfigData = MenuPlayConfigUI.configData.Hard;
					}
				}
				else
				{
					MenuPlayConfigUI.modeConfigData = MenuPlayConfigUI.configData.Normal;
				}
			}
			else
			{
				MenuPlayConfigUI.modeConfigData = MenuPlayConfigUI.configData.Easy;
			}
			MenuPlayConfigUI.refreshConfig();
			MenuPlayConfigUI.container.lerpPositionScale(0f, 0f, ESleekLerp.EXPONENTIAL, 20f);
		}

		public static void close()
		{
			if (!MenuPlayConfigUI.active)
			{
				return;
			}
			MenuPlayConfigUI.active = false;
			ReadWrite.serializeJSON<ConfigData>("/Worlds/Singleplayer_" + Characters.selected + "/Config.json", false, MenuPlayConfigUI.configData);
			MenuPlayConfigUI.container.lerpPositionScale(0f, 1f, ESleekLerp.EXPONENTIAL, 20f);
		}

		private static void refreshConfig()
		{
			MenuPlayConfigUI.configBox.remove();
			MenuPlayConfigUI.configOffset = 0;
			MenuPlayConfigUI.configGroups.Clear();
			Type type = MenuPlayConfigUI.modeConfigData.GetType();
			foreach (FieldInfo fieldInfo in type.GetFields())
			{
				SleekBox sleekBox = new SleekBox();
				sleekBox.positionOffset_Y = MenuPlayConfigUI.configOffset;
				sleekBox.sizeOffset_X = -30;
				sleekBox.sizeOffset_Y = 30;
				sleekBox.sizeScale_X = 1f;
				sleekBox.text = MenuPlayConfigUI.localization.format(fieldInfo.Name);
				MenuPlayConfigUI.configBox.add(sleekBox);
				int num = 40;
				MenuPlayConfigUI.configOffset += 40;
				object value = fieldInfo.GetValue(MenuPlayConfigUI.modeConfigData);
				Type type2 = value.GetType();
				foreach (FieldInfo fieldInfo2 in type2.GetFields())
				{
					object value2 = fieldInfo2.GetValue(value);
					Type type3 = value2.GetType();
					if (type3 == typeof(uint))
					{
						SleekUInt32Field sleekUInt32Field = new SleekUInt32Field();
						sleekUInt32Field.positionOffset_Y = num;
						sleekUInt32Field.sizeOffset_X = 200;
						sleekUInt32Field.sizeOffset_Y = 30;
						sleekUInt32Field.state = (uint)value2;
						sleekUInt32Field.addLabel(MenuPlayConfigUI.localization.format(fieldInfo2.Name), ESleekSide.RIGHT);
						SleekUInt32Field sleekUInt32Field2 = sleekUInt32Field;
						Delegate onTypedUInt = sleekUInt32Field2.onTypedUInt32;
						if (MenuPlayConfigUI.<>f__mg$cache0 == null)
						{
							MenuPlayConfigUI.<>f__mg$cache0 = new TypedUInt32(MenuPlayConfigUI.onTypedUInt32);
						}
						sleekUInt32Field2.onTypedUInt32 = (TypedUInt32)Delegate.Combine(onTypedUInt, MenuPlayConfigUI.<>f__mg$cache0);
						sleekBox.add(sleekUInt32Field);
						num += 40;
						MenuPlayConfigUI.configOffset += 40;
					}
					else if (type3 == typeof(float))
					{
						SleekSingleField sleekSingleField = new SleekSingleField();
						sleekSingleField.positionOffset_Y = num;
						sleekSingleField.sizeOffset_X = 200;
						sleekSingleField.sizeOffset_Y = 30;
						sleekSingleField.state = (float)value2;
						sleekSingleField.addLabel(MenuPlayConfigUI.localization.format(fieldInfo2.Name), ESleekSide.RIGHT);
						SleekSingleField sleekSingleField2 = sleekSingleField;
						Delegate onTypedSingle = sleekSingleField2.onTypedSingle;
						if (MenuPlayConfigUI.<>f__mg$cache1 == null)
						{
							MenuPlayConfigUI.<>f__mg$cache1 = new TypedSingle(MenuPlayConfigUI.onTypedSingle);
						}
						sleekSingleField2.onTypedSingle = (TypedSingle)Delegate.Combine(onTypedSingle, MenuPlayConfigUI.<>f__mg$cache1);
						sleekBox.add(sleekSingleField);
						num += 40;
						MenuPlayConfigUI.configOffset += 40;
					}
					else if (type3 == typeof(bool))
					{
						SleekToggle sleekToggle = new SleekToggle();
						sleekToggle.positionOffset_Y = num;
						sleekToggle.sizeOffset_X = 40;
						sleekToggle.sizeOffset_Y = 40;
						sleekToggle.state = (bool)value2;
						sleekToggle.addLabel(MenuPlayConfigUI.localization.format(fieldInfo2.Name), ESleekSide.RIGHT);
						SleekToggle sleekToggle2 = sleekToggle;
						Delegate onToggled = sleekToggle2.onToggled;
						if (MenuPlayConfigUI.<>f__mg$cache2 == null)
						{
							MenuPlayConfigUI.<>f__mg$cache2 = new Toggled(MenuPlayConfigUI.onToggled);
						}
						sleekToggle2.onToggled = (Toggled)Delegate.Combine(onToggled, MenuPlayConfigUI.<>f__mg$cache2);
						sleekBox.add(sleekToggle);
						num += 50;
						MenuPlayConfigUI.configOffset += 50;
					}
				}
				MenuPlayConfigUI.configOffset += 40;
				MenuPlayConfigUI.configGroups.Add(value);
			}
			MenuPlayConfigUI.configBox.area = new Rect(0f, 0f, 5f, (float)(MenuPlayConfigUI.configOffset - 50));
		}

		private static void updateValue(Sleek sleek, object state)
		{
			int index = MenuPlayConfigUI.configBox.search(sleek.parent);
			object obj = MenuPlayConfigUI.configGroups[index];
			Type type = obj.GetType();
			FieldInfo[] fields = type.GetFields();
			int num = sleek.parent.search(sleek);
			FieldInfo fieldInfo = fields[num];
			fieldInfo.SetValue(obj, state);
		}

		private static void onTypedUInt32(SleekUInt32Field uint32Field, uint state)
		{
			MenuPlayConfigUI.updateValue(uint32Field, state);
		}

		private static void onTypedSingle(SleekSingleField singleField, float state)
		{
			MenuPlayConfigUI.updateValue(singleField, state);
		}

		private static void onToggled(SleekToggle toggle, bool state)
		{
			MenuPlayConfigUI.updateValue(toggle, state);
		}

		private static void onClickedBackButton(SleekButton button)
		{
			MenuPlaySingleplayerUI.open();
			MenuPlayConfigUI.close();
		}

		private static void onClickedDefaultButton(SleekButton button)
		{
			MenuPlayConfigUI.modeConfigData = new ModeConfigData(PlaySettings.singleplayerMode);
			EGameMode singleplayerMode = PlaySettings.singleplayerMode;
			if (singleplayerMode != EGameMode.EASY)
			{
				if (singleplayerMode != EGameMode.NORMAL)
				{
					if (singleplayerMode == EGameMode.HARD)
					{
						MenuPlayConfigUI.configData.Hard = MenuPlayConfigUI.modeConfigData;
					}
				}
				else
				{
					MenuPlayConfigUI.configData.Normal = MenuPlayConfigUI.modeConfigData;
				}
			}
			else
			{
				MenuPlayConfigUI.configData.Easy = MenuPlayConfigUI.modeConfigData;
			}
			MenuPlayConfigUI.refreshConfig();
		}

		public static Local localization;

		private static Sleek container;

		public static bool active;

		private static SleekButtonIcon backButton;

		private static SleekButton defaultButton;

		private static SleekScrollBox configBox;

		private static ConfigData configData;

		private static ModeConfigData modeConfigData;

		private static int configOffset;

		private static List<object> configGroups;

		[CompilerGenerated]
		private static TypedUInt32 <>f__mg$cache0;

		[CompilerGenerated]
		private static TypedSingle <>f__mg$cache1;

		[CompilerGenerated]
		private static Toggled <>f__mg$cache2;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache3;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache4;
	}
}
