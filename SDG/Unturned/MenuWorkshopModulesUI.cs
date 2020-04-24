using System;
using System.Runtime.CompilerServices;
using SDG.Framework.Modules;
using UnityEngine;

namespace SDG.Unturned
{
	public class MenuWorkshopModulesUI
	{
		public MenuWorkshopModulesUI()
		{
			MenuWorkshopModulesUI.localization = Localization.read("/Menu/Workshop/MenuWorkshopModules.dat");
			MenuWorkshopModulesUI.container = new Sleek();
			MenuWorkshopModulesUI.container.positionOffset_X = 10;
			MenuWorkshopModulesUI.container.positionOffset_Y = 10;
			MenuWorkshopModulesUI.container.positionScale_Y = 1f;
			MenuWorkshopModulesUI.container.sizeOffset_X = -20;
			MenuWorkshopModulesUI.container.sizeOffset_Y = -20;
			MenuWorkshopModulesUI.container.sizeScale_X = 1f;
			MenuWorkshopModulesUI.container.sizeScale_Y = 1f;
			MenuUI.container.add(MenuWorkshopModulesUI.container);
			MenuWorkshopModulesUI.active = false;
			MenuWorkshopModulesUI.headerBox = new SleekBox();
			MenuWorkshopModulesUI.headerBox.sizeOffset_Y = 50;
			MenuWorkshopModulesUI.headerBox.sizeScale_X = 1f;
			MenuWorkshopModulesUI.headerBox.fontSize = 14;
			MenuWorkshopModulesUI.headerBox.text = MenuWorkshopModulesUI.localization.format("Header");
			MenuWorkshopModulesUI.container.add(MenuWorkshopModulesUI.headerBox);
			MenuWorkshopModulesUI.moduleBox = new SleekScrollBox();
			MenuWorkshopModulesUI.moduleBox.positionOffset_Y = 60;
			MenuWorkshopModulesUI.moduleBox.sizeOffset_Y = -120;
			MenuWorkshopModulesUI.moduleBox.sizeScale_X = 1f;
			MenuWorkshopModulesUI.moduleBox.sizeScale_Y = 1f;
			MenuWorkshopModulesUI.moduleBox.area = new Rect(0f, 0f, 5f, 0f);
			MenuWorkshopModulesUI.container.add(MenuWorkshopModulesUI.moduleBox);
			if (ModuleHook.modules.Count == 0)
			{
				SleekBox sleekBox = new SleekBox();
				sleekBox.positionOffset_Y = 60;
				sleekBox.sizeOffset_X = -30;
				sleekBox.sizeOffset_Y = 50;
				sleekBox.sizeScale_X = 1f;
				sleekBox.fontSize = 14;
				sleekBox.text = MenuWorkshopModulesUI.localization.format("No_Modules");
				MenuWorkshopModulesUI.container.add(sleekBox);
			}
			else
			{
				for (int i = 0; i < ModuleHook.modules.Count; i++)
				{
					ModuleConfig config = ModuleHook.modules[i].config;
					Local local = Localization.tryRead(config.DirectoryPath, false);
					SleekBox sleekBox2 = new SleekBox();
					sleekBox2.positionOffset_Y = i * 130;
					sleekBox2.sizeOffset_X = -30;
					sleekBox2.sizeOffset_Y = 120;
					sleekBox2.sizeScale_X = 1f;
					SleekToggle sleekToggle = new SleekToggle();
					sleekToggle.positionOffset_X = 5;
					sleekToggle.positionOffset_Y = -20;
					sleekToggle.positionScale_Y = 0.5f;
					sleekToggle.sizeOffset_X = 40;
					sleekToggle.sizeOffset_Y = 40;
					sleekToggle.state = config.IsEnabled;
					SleekToggle sleekToggle2 = sleekToggle;
					if (MenuWorkshopModulesUI.<>f__mg$cache0 == null)
					{
						MenuWorkshopModulesUI.<>f__mg$cache0 = new Toggled(MenuWorkshopModulesUI.onToggledModuleToggle);
					}
					sleekToggle2.onToggled = MenuWorkshopModulesUI.<>f__mg$cache0;
					sleekBox2.add(sleekToggle);
					SleekLabel sleekLabel = new SleekLabel();
					sleekLabel.positionOffset_X = 50;
					sleekLabel.positionOffset_Y = 5;
					sleekLabel.sizeOffset_X = -55;
					sleekLabel.sizeOffset_Y = 30;
					sleekLabel.sizeScale_X = 1f;
					sleekLabel.fontSize = 14;
					sleekLabel.fontAlignment = 0;
					sleekLabel.text = local.format("Name");
					sleekBox2.add(sleekLabel);
					SleekLabel sleekLabel2 = new SleekLabel();
					sleekLabel2.positionOffset_X = 50;
					sleekLabel2.positionOffset_Y = 30;
					sleekLabel2.sizeOffset_X = -55;
					sleekLabel2.sizeOffset_Y = 25;
					sleekLabel2.sizeScale_X = 1f;
					sleekLabel2.fontAlignment = 0;
					sleekLabel2.text = MenuWorkshopModulesUI.localization.format("Version", new object[]
					{
						config.Version
					});
					sleekBox2.add(sleekLabel2);
					SleekLabel sleekLabel3 = new SleekLabel();
					sleekLabel3.positionOffset_X = 50;
					sleekLabel3.positionOffset_Y = 50;
					sleekLabel3.sizeOffset_X = -55;
					sleekLabel3.sizeOffset_Y = 65;
					sleekLabel3.sizeScale_X = 1f;
					sleekLabel3.fontSize = 12;
					sleekLabel3.fontAlignment = 0;
					sleekLabel3.text = local.format("Description");
					sleekBox2.add(sleekLabel3);
					if (ReadWrite.fileExists(config.DirectoryPath + "/Icon.png", false, false))
					{
						byte[] array = ReadWrite.readBytes(config.DirectoryPath + "/Icon.png", false, false);
						Texture2D texture2D = new Texture2D(100, 100, 5, false, true);
						texture2D.name = "Module_" + config.Name + "_Icon";
						texture2D.hideFlags = 61;
						texture2D.LoadImage(array);
						sleekBox2.add(new SleekImageTexture
						{
							positionOffset_X = 50,
							positionOffset_Y = 10,
							sizeOffset_X = 100,
							sizeOffset_Y = 100,
							texture = texture2D,
							shouldDestroyTexture = true
						});
						sleekLabel.positionOffset_X += 105;
						sleekLabel.sizeOffset_X -= 105;
						sleekLabel2.positionOffset_X += 105;
						sleekLabel2.sizeOffset_X -= 105;
						sleekLabel3.positionOffset_X += 105;
						sleekLabel3.sizeOffset_X -= 105;
					}
					MenuWorkshopModulesUI.moduleBox.add(sleekBox2);
				}
				MenuWorkshopModulesUI.moduleBox.area = new Rect(0f, 0f, 5f, (float)(ModuleHook.modules.Count * 130 - 10));
			}
			MenuWorkshopModulesUI.backButton = new SleekButtonIcon((Texture2D)MenuDashboardUI.icons.load("Exit"));
			MenuWorkshopModulesUI.backButton.positionOffset_Y = -50;
			MenuWorkshopModulesUI.backButton.positionScale_Y = 1f;
			MenuWorkshopModulesUI.backButton.sizeOffset_X = 200;
			MenuWorkshopModulesUI.backButton.sizeOffset_Y = 50;
			MenuWorkshopModulesUI.backButton.text = MenuDashboardUI.localization.format("BackButtonText");
			MenuWorkshopModulesUI.backButton.tooltip = MenuDashboardUI.localization.format("BackButtonTooltip");
			SleekButton sleekButton = MenuWorkshopModulesUI.backButton;
			if (MenuWorkshopModulesUI.<>f__mg$cache1 == null)
			{
				MenuWorkshopModulesUI.<>f__mg$cache1 = new ClickedButton(MenuWorkshopModulesUI.onClickedBackButton);
			}
			sleekButton.onClickedButton = MenuWorkshopModulesUI.<>f__mg$cache1;
			MenuWorkshopModulesUI.backButton.fontSize = 14;
			MenuWorkshopModulesUI.backButton.iconImage.backgroundTint = ESleekTint.FOREGROUND;
			MenuWorkshopModulesUI.container.add(MenuWorkshopModulesUI.backButton);
		}

		public static void open()
		{
			if (MenuWorkshopModulesUI.active)
			{
				return;
			}
			MenuWorkshopModulesUI.active = true;
			MenuWorkshopModulesUI.container.lerpPositionScale(0f, 0f, ESleekLerp.EXPONENTIAL, 20f);
		}

		public static void close()
		{
			if (!MenuWorkshopModulesUI.active)
			{
				return;
			}
			MenuWorkshopModulesUI.active = false;
			MenuWorkshopModulesUI.container.lerpPositionScale(0f, 1f, ESleekLerp.EXPONENTIAL, 20f);
		}

		private static void onToggledModuleToggle(SleekToggle toggle, bool state)
		{
			int index = MenuWorkshopModulesUI.moduleBox.search(toggle.parent);
			ModuleHook.toggleModuleEnabled(index);
		}

		private static void onClickedBackButton(SleekButton button)
		{
			MenuWorkshopUI.open();
			MenuWorkshopModulesUI.close();
		}

		private static Local localization;

		private static Sleek container;

		public static bool active;

		private static SleekButtonIcon backButton;

		private static SleekBox headerBox;

		private static SleekScrollBox moduleBox;

		[CompilerGenerated]
		private static Toggled <>f__mg$cache0;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache1;
	}
}
