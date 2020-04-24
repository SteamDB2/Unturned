using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace SDG.Unturned
{
	public class MenuConfigurationControlsUI
	{
		public MenuConfigurationControlsUI()
		{
			MenuConfigurationControlsUI.localization = Localization.read("/Menu/Configuration/MenuConfigurationControls.dat");
			MenuConfigurationControlsUI.container = new Sleek();
			MenuConfigurationControlsUI.container.positionOffset_X = 10;
			MenuConfigurationControlsUI.container.positionOffset_Y = 10;
			MenuConfigurationControlsUI.container.positionScale_Y = 1f;
			MenuConfigurationControlsUI.container.sizeOffset_X = -20;
			MenuConfigurationControlsUI.container.sizeOffset_Y = -20;
			MenuConfigurationControlsUI.container.sizeScale_X = 1f;
			MenuConfigurationControlsUI.container.sizeScale_Y = 1f;
			if (Provider.isConnected)
			{
				PlayerUI.container.add(MenuConfigurationControlsUI.container);
			}
			else
			{
				MenuUI.container.add(MenuConfigurationControlsUI.container);
			}
			MenuConfigurationControlsUI.active = false;
			MenuConfigurationControlsUI.binding = byte.MaxValue;
			MenuConfigurationControlsUI.controlsBox = new SleekScrollBox();
			MenuConfigurationControlsUI.controlsBox.positionOffset_X = -200;
			MenuConfigurationControlsUI.controlsBox.positionOffset_Y = 100;
			MenuConfigurationControlsUI.controlsBox.positionScale_X = 0.5f;
			MenuConfigurationControlsUI.controlsBox.sizeOffset_X = 430;
			MenuConfigurationControlsUI.controlsBox.sizeOffset_Y = -200;
			MenuConfigurationControlsUI.controlsBox.sizeScale_Y = 1f;
			MenuConfigurationControlsUI.controlsBox.area = new Rect(0f, 0f, 5f, (float)(380 + (ControlsSettings.bindings.Length + (MenuConfigurationControlsUI.layouts.Length - 1) * 2) * 40 - 10));
			MenuConfigurationControlsUI.container.add(MenuConfigurationControlsUI.controlsBox);
			MenuConfigurationControlsUI.sensitivityField = new SleekSingleField();
			MenuConfigurationControlsUI.sensitivityField.positionOffset_Y = 100;
			MenuConfigurationControlsUI.sensitivityField.sizeOffset_X = 200;
			MenuConfigurationControlsUI.sensitivityField.sizeOffset_Y = 30;
			MenuConfigurationControlsUI.sensitivityField.addLabel(MenuConfigurationControlsUI.localization.format("Sensitivity_Field_Label"), ESleekSide.RIGHT);
			SleekSingleField sleekSingleField = MenuConfigurationControlsUI.sensitivityField;
			if (MenuConfigurationControlsUI.<>f__mg$cache0 == null)
			{
				MenuConfigurationControlsUI.<>f__mg$cache0 = new TypedSingle(MenuConfigurationControlsUI.onTypedSensitivityField);
			}
			sleekSingleField.onTypedSingle = MenuConfigurationControlsUI.<>f__mg$cache0;
			MenuConfigurationControlsUI.controlsBox.add(MenuConfigurationControlsUI.sensitivityField);
			MenuConfigurationControlsUI.invertToggle = new SleekToggle();
			MenuConfigurationControlsUI.invertToggle.sizeOffset_X = 40;
			MenuConfigurationControlsUI.invertToggle.sizeOffset_Y = 40;
			MenuConfigurationControlsUI.invertToggle.addLabel(MenuConfigurationControlsUI.localization.format("Invert_Toggle_Label"), ESleekSide.RIGHT);
			SleekToggle sleekToggle = MenuConfigurationControlsUI.invertToggle;
			if (MenuConfigurationControlsUI.<>f__mg$cache1 == null)
			{
				MenuConfigurationControlsUI.<>f__mg$cache1 = new Toggled(MenuConfigurationControlsUI.onToggledInvertToggle);
			}
			sleekToggle.onToggled = MenuConfigurationControlsUI.<>f__mg$cache1;
			MenuConfigurationControlsUI.controlsBox.add(MenuConfigurationControlsUI.invertToggle);
			MenuConfigurationControlsUI.invertFlightToggle = new SleekToggle();
			MenuConfigurationControlsUI.invertFlightToggle.positionOffset_Y = 50;
			MenuConfigurationControlsUI.invertFlightToggle.sizeOffset_X = 40;
			MenuConfigurationControlsUI.invertFlightToggle.sizeOffset_Y = 40;
			MenuConfigurationControlsUI.invertFlightToggle.addLabel(MenuConfigurationControlsUI.localization.format("Invert_Flight_Toggle_Label"), ESleekSide.RIGHT);
			SleekToggle sleekToggle2 = MenuConfigurationControlsUI.invertFlightToggle;
			if (MenuConfigurationControlsUI.<>f__mg$cache2 == null)
			{
				MenuConfigurationControlsUI.<>f__mg$cache2 = new Toggled(MenuConfigurationControlsUI.onToggledInvertFlightToggle);
			}
			sleekToggle2.onToggled = MenuConfigurationControlsUI.<>f__mg$cache2;
			MenuConfigurationControlsUI.controlsBox.add(MenuConfigurationControlsUI.invertFlightToggle);
			MenuConfigurationControlsUI.aimingButton = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(MenuConfigurationControlsUI.localization.format("Hold")),
				new GUIContent(MenuConfigurationControlsUI.localization.format("Toggle"))
			});
			MenuConfigurationControlsUI.aimingButton.positionOffset_Y = 140;
			MenuConfigurationControlsUI.aimingButton.sizeOffset_X = 200;
			MenuConfigurationControlsUI.aimingButton.sizeOffset_Y = 30;
			MenuConfigurationControlsUI.aimingButton.addLabel(MenuConfigurationControlsUI.localization.format("Aiming_Label"), ESleekSide.RIGHT);
			SleekButtonState sleekButtonState = MenuConfigurationControlsUI.aimingButton;
			if (MenuConfigurationControlsUI.<>f__mg$cache3 == null)
			{
				MenuConfigurationControlsUI.<>f__mg$cache3 = new SwappedState(MenuConfigurationControlsUI.onSwappedAimingState);
			}
			sleekButtonState.onSwappedState = MenuConfigurationControlsUI.<>f__mg$cache3;
			MenuConfigurationControlsUI.controlsBox.add(MenuConfigurationControlsUI.aimingButton);
			MenuConfigurationControlsUI.crouchingButton = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(MenuConfigurationControlsUI.localization.format("Hold")),
				new GUIContent(MenuConfigurationControlsUI.localization.format("Toggle"))
			});
			MenuConfigurationControlsUI.crouchingButton.positionOffset_Y = 180;
			MenuConfigurationControlsUI.crouchingButton.sizeOffset_X = 200;
			MenuConfigurationControlsUI.crouchingButton.sizeOffset_Y = 30;
			MenuConfigurationControlsUI.crouchingButton.addLabel(MenuConfigurationControlsUI.localization.format("Crouching_Label"), ESleekSide.RIGHT);
			SleekButtonState sleekButtonState2 = MenuConfigurationControlsUI.crouchingButton;
			if (MenuConfigurationControlsUI.<>f__mg$cache4 == null)
			{
				MenuConfigurationControlsUI.<>f__mg$cache4 = new SwappedState(MenuConfigurationControlsUI.onSwappedCrouchingState);
			}
			sleekButtonState2.onSwappedState = MenuConfigurationControlsUI.<>f__mg$cache4;
			MenuConfigurationControlsUI.controlsBox.add(MenuConfigurationControlsUI.crouchingButton);
			MenuConfigurationControlsUI.proningButton = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(MenuConfigurationControlsUI.localization.format("Hold")),
				new GUIContent(MenuConfigurationControlsUI.localization.format("Toggle"))
			});
			MenuConfigurationControlsUI.proningButton.positionOffset_Y = 220;
			MenuConfigurationControlsUI.proningButton.sizeOffset_X = 200;
			MenuConfigurationControlsUI.proningButton.sizeOffset_Y = 30;
			MenuConfigurationControlsUI.proningButton.addLabel(MenuConfigurationControlsUI.localization.format("Proning_Label"), ESleekSide.RIGHT);
			SleekButtonState sleekButtonState3 = MenuConfigurationControlsUI.proningButton;
			if (MenuConfigurationControlsUI.<>f__mg$cache5 == null)
			{
				MenuConfigurationControlsUI.<>f__mg$cache5 = new SwappedState(MenuConfigurationControlsUI.onSwappedProningState);
			}
			sleekButtonState3.onSwappedState = MenuConfigurationControlsUI.<>f__mg$cache5;
			MenuConfigurationControlsUI.controlsBox.add(MenuConfigurationControlsUI.proningButton);
			MenuConfigurationControlsUI.sprintingButton = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(MenuConfigurationControlsUI.localization.format("Hold")),
				new GUIContent(MenuConfigurationControlsUI.localization.format("Toggle"))
			});
			MenuConfigurationControlsUI.sprintingButton.positionOffset_Y = 260;
			MenuConfigurationControlsUI.sprintingButton.sizeOffset_X = 200;
			MenuConfigurationControlsUI.sprintingButton.sizeOffset_Y = 30;
			MenuConfigurationControlsUI.sprintingButton.addLabel(MenuConfigurationControlsUI.localization.format("Sprinting_Label"), ESleekSide.RIGHT);
			SleekButtonState sleekButtonState4 = MenuConfigurationControlsUI.sprintingButton;
			if (MenuConfigurationControlsUI.<>f__mg$cache6 == null)
			{
				MenuConfigurationControlsUI.<>f__mg$cache6 = new SwappedState(MenuConfigurationControlsUI.onSwappedSprintingState);
			}
			sleekButtonState4.onSwappedState = MenuConfigurationControlsUI.<>f__mg$cache6;
			MenuConfigurationControlsUI.controlsBox.add(MenuConfigurationControlsUI.sprintingButton);
			MenuConfigurationControlsUI.leaningButton = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(MenuConfigurationControlsUI.localization.format("Hold")),
				new GUIContent(MenuConfigurationControlsUI.localization.format("Toggle"))
			});
			MenuConfigurationControlsUI.leaningButton.positionOffset_Y = 300;
			MenuConfigurationControlsUI.leaningButton.sizeOffset_X = 200;
			MenuConfigurationControlsUI.leaningButton.sizeOffset_Y = 30;
			MenuConfigurationControlsUI.leaningButton.addLabel(MenuConfigurationControlsUI.localization.format("Leaning_Label"), ESleekSide.RIGHT);
			SleekButtonState sleekButtonState5 = MenuConfigurationControlsUI.leaningButton;
			if (MenuConfigurationControlsUI.<>f__mg$cache7 == null)
			{
				MenuConfigurationControlsUI.<>f__mg$cache7 = new SwappedState(MenuConfigurationControlsUI.onSwappedLeaningState);
			}
			sleekButtonState5.onSwappedState = MenuConfigurationControlsUI.<>f__mg$cache7;
			MenuConfigurationControlsUI.controlsBox.add(MenuConfigurationControlsUI.leaningButton);
			MenuConfigurationControlsUI.buttons = new SleekButton[ControlsSettings.bindings.Length];
			byte b = 0;
			byte b2 = 0;
			while ((int)b2 < MenuConfigurationControlsUI.layouts.Length)
			{
				SleekBox sleekBox = new SleekBox();
				sleekBox.positionOffset_Y = 340 + (int)((b + b2 * 2) * 40);
				sleekBox.sizeOffset_X = -30;
				sleekBox.sizeOffset_Y = 30;
				sleekBox.sizeScale_X = 1f;
				sleekBox.text = MenuConfigurationControlsUI.localization.format("Layout_" + b2);
				MenuConfigurationControlsUI.controlsBox.add(sleekBox);
				byte b3 = 0;
				while ((int)b3 < MenuConfigurationControlsUI.layouts[(int)b2].Length)
				{
					SleekButton sleekButton = new SleekButton();
					sleekButton.positionOffset_Y = (int)((b3 + 1) * 40);
					sleekButton.sizeOffset_Y = 30;
					sleekButton.sizeScale_X = 1f;
					SleekButton sleekButton2 = sleekButton;
					if (MenuConfigurationControlsUI.<>f__mg$cache8 == null)
					{
						MenuConfigurationControlsUI.<>f__mg$cache8 = new ClickedButton(MenuConfigurationControlsUI.onClickedKeyButton);
					}
					sleekButton2.onClickedButton = MenuConfigurationControlsUI.<>f__mg$cache8;
					sleekBox.add(sleekButton);
					MenuConfigurationControlsUI.buttons[(int)MenuConfigurationControlsUI.layouts[(int)b2][(int)b3]] = sleekButton;
					b += 1;
					b3 += 1;
				}
				b2 += 1;
			}
			MenuConfigurationControlsUI.backButton = new SleekButtonIcon((Texture2D)MenuDashboardUI.icons.load("Exit"));
			MenuConfigurationControlsUI.backButton.positionOffset_Y = -50;
			MenuConfigurationControlsUI.backButton.positionScale_Y = 1f;
			MenuConfigurationControlsUI.backButton.sizeOffset_X = 200;
			MenuConfigurationControlsUI.backButton.sizeOffset_Y = 50;
			MenuConfigurationControlsUI.backButton.text = MenuDashboardUI.localization.format("BackButtonText");
			MenuConfigurationControlsUI.backButton.tooltip = MenuDashboardUI.localization.format("BackButtonTooltip");
			SleekButton sleekButton3 = MenuConfigurationControlsUI.backButton;
			if (MenuConfigurationControlsUI.<>f__mg$cache9 == null)
			{
				MenuConfigurationControlsUI.<>f__mg$cache9 = new ClickedButton(MenuConfigurationControlsUI.onClickedBackButton);
			}
			sleekButton3.onClickedButton = MenuConfigurationControlsUI.<>f__mg$cache9;
			MenuConfigurationControlsUI.backButton.fontSize = 14;
			MenuConfigurationControlsUI.backButton.iconImage.backgroundTint = ESleekTint.FOREGROUND;
			MenuConfigurationControlsUI.container.add(MenuConfigurationControlsUI.backButton);
			MenuConfigurationControlsUI.defaultButton = new SleekButton();
			MenuConfigurationControlsUI.defaultButton.positionOffset_X = -200;
			MenuConfigurationControlsUI.defaultButton.positionOffset_Y = -50;
			MenuConfigurationControlsUI.defaultButton.positionScale_X = 1f;
			MenuConfigurationControlsUI.defaultButton.positionScale_Y = 1f;
			MenuConfigurationControlsUI.defaultButton.sizeOffset_X = 200;
			MenuConfigurationControlsUI.defaultButton.sizeOffset_Y = 50;
			MenuConfigurationControlsUI.defaultButton.text = MenuPlayConfigUI.localization.format("Default");
			MenuConfigurationControlsUI.defaultButton.tooltip = MenuPlayConfigUI.localization.format("Default_Tooltip");
			SleekButton sleekButton4 = MenuConfigurationControlsUI.defaultButton;
			if (MenuConfigurationControlsUI.<>f__mg$cacheA == null)
			{
				MenuConfigurationControlsUI.<>f__mg$cacheA = new ClickedButton(MenuConfigurationControlsUI.onClickedDefaultButton);
			}
			sleekButton4.onClickedButton = MenuConfigurationControlsUI.<>f__mg$cacheA;
			MenuConfigurationControlsUI.defaultButton.fontSize = 14;
			MenuConfigurationControlsUI.container.add(MenuConfigurationControlsUI.defaultButton);
			MenuConfigurationControlsUI.updateAll();
		}

		public static void open()
		{
			if (MenuConfigurationControlsUI.active)
			{
				return;
			}
			MenuConfigurationControlsUI.active = true;
			MenuConfigurationControlsUI.container.lerpPositionScale(0f, 0f, ESleekLerp.EXPONENTIAL, 20f);
		}

		public static void close()
		{
			if (!MenuConfigurationControlsUI.active)
			{
				return;
			}
			MenuConfigurationControlsUI.active = false;
			MenuConfigurationControlsUI.binding = byte.MaxValue;
			MenuConfigurationControlsUI.container.lerpPositionScale(0f, 1f, ESleekLerp.EXPONENTIAL, 20f);
		}

		public static void cancel()
		{
			MenuConfigurationControlsUI.binding = byte.MaxValue;
			SleekRender.allowInput = true;
		}

		public static void bind(KeyCode key)
		{
			ControlsSettings.bind(MenuConfigurationControlsUI.binding, key);
			MenuConfigurationControlsUI.updateButton(MenuConfigurationControlsUI.binding);
			MenuConfigurationControlsUI.cancel();
		}

		public static string getKeyCodeText(KeyCode key)
		{
			if (MenuConfigurationControlsUI.localizationKeyCodes == null)
			{
				MenuConfigurationControlsUI.localizationKeyCodes = Localization.read("/Shared/KeyCodes.dat");
			}
			string text = key.ToString();
			if (MenuConfigurationControlsUI.localizationKeyCodes.has(text))
			{
				text = MenuConfigurationControlsUI.localizationKeyCodes.format(text);
			}
			return text;
		}

		public static void updateButton(byte index)
		{
			KeyCode key = ControlsSettings.bindings[(int)index].key;
			string keyCodeText = MenuConfigurationControlsUI.getKeyCodeText(key);
			MenuConfigurationControlsUI.buttons[(int)index].text = MenuConfigurationControlsUI.localization.format("Key_" + index + "_Button", new object[]
			{
				keyCodeText
			});
		}

		private static void onTypedSensitivityField(SleekSingleField field, float state)
		{
			ControlsSettings.sensitivity = state;
		}

		private static void onToggledInvertToggle(SleekToggle toggle, bool state)
		{
			ControlsSettings.invert = state;
		}

		private static void onToggledInvertFlightToggle(SleekToggle toggle, bool state)
		{
			ControlsSettings.invertFlight = state;
		}

		private static void onSwappedAimingState(SleekButtonState button, int index)
		{
			ControlsSettings.aiming = (EControlMode)index;
		}

		private static void onSwappedCrouchingState(SleekButtonState button, int index)
		{
			ControlsSettings.crouching = (EControlMode)index;
		}

		private static void onSwappedProningState(SleekButtonState button, int index)
		{
			ControlsSettings.proning = (EControlMode)index;
		}

		private static void onSwappedSprintingState(SleekButtonState button, int index)
		{
			ControlsSettings.sprinting = (EControlMode)index;
		}

		private static void onSwappedLeaningState(SleekButtonState button, int index)
		{
			ControlsSettings.leaning = (EControlMode)index;
		}

		private static void onClickedKeyButton(SleekButton button)
		{
			SleekRender.allowInput = false;
			MenuConfigurationControlsUI.binding = 0;
			while ((int)MenuConfigurationControlsUI.binding < MenuConfigurationControlsUI.buttons.Length)
			{
				if (MenuConfigurationControlsUI.buttons[(int)MenuConfigurationControlsUI.binding] == button)
				{
					break;
				}
				MenuConfigurationControlsUI.binding += 1;
			}
			button.text = MenuConfigurationControlsUI.localization.format("Key_" + MenuConfigurationControlsUI.binding + "_Button", new object[]
			{
				"?"
			});
		}

		public static void bindOnGUI()
		{
			if (MenuConfigurationControlsUI.binding != 255)
			{
				if (Event.current.type == 4)
				{
					if (Event.current.keyCode == 8)
					{
						MenuConfigurationControlsUI.updateButton(MenuConfigurationControlsUI.binding);
						MenuConfigurationControlsUI.cancel();
					}
					else if (Event.current.keyCode != 27 && Event.current.keyCode != 277)
					{
						MenuConfigurationControlsUI.bind(Event.current.keyCode);
					}
				}
				else if (Event.current.type == null)
				{
					if (Event.current.button == 0)
					{
						MenuConfigurationControlsUI.bind(323);
					}
					else if (Event.current.button == 1)
					{
						MenuConfigurationControlsUI.bind(324);
					}
					else if (Event.current.button == 2)
					{
						MenuConfigurationControlsUI.bind(325);
					}
					else if (Event.current.button == 3)
					{
						MenuConfigurationControlsUI.bind(326);
					}
					else if (Event.current.button == 4)
					{
						MenuConfigurationControlsUI.bind(327);
					}
					else if (Event.current.button == 5)
					{
						MenuConfigurationControlsUI.bind(328);
					}
					else if (Event.current.button == 6)
					{
						MenuConfigurationControlsUI.bind(329);
					}
				}
				else if (Event.current.shift)
				{
					MenuConfigurationControlsUI.bind(304);
				}
			}
		}

		public static void bindUpdate()
		{
			if (MenuConfigurationControlsUI.binding != 255)
			{
				if (Input.GetKeyDown(326))
				{
					MenuConfigurationControlsUI.bind(326);
				}
				else if (Input.GetKeyDown(327))
				{
					MenuConfigurationControlsUI.bind(327);
				}
				else if (Input.GetKeyDown(328))
				{
					MenuConfigurationControlsUI.bind(328);
				}
				else if (Input.GetKeyDown(329))
				{
					MenuConfigurationControlsUI.bind(329);
				}
			}
		}

		private static void onClickedBackButton(SleekButton button)
		{
			if (Player.player != null)
			{
				PlayerPauseUI.open();
			}
			else
			{
				MenuConfigurationUI.open();
			}
			MenuConfigurationControlsUI.close();
		}

		private static void onClickedDefaultButton(SleekButton button)
		{
			ControlsSettings.restoreDefaults();
			MenuConfigurationControlsUI.updateAll();
		}

		private static void updateAll()
		{
			byte b = 0;
			while ((int)b < MenuConfigurationControlsUI.layouts.Length)
			{
				byte b2 = 0;
				while ((int)b2 < MenuConfigurationControlsUI.layouts[(int)b].Length)
				{
					MenuConfigurationControlsUI.updateButton(MenuConfigurationControlsUI.layouts[(int)b][(int)b2]);
					b2 += 1;
				}
				b += 1;
			}
			MenuConfigurationControlsUI.leaningButton.state = (int)ControlsSettings.leaning;
			MenuConfigurationControlsUI.sprintingButton.state = (int)ControlsSettings.sprinting;
			MenuConfigurationControlsUI.proningButton.state = (int)ControlsSettings.proning;
			MenuConfigurationControlsUI.crouchingButton.state = (int)ControlsSettings.crouching;
			MenuConfigurationControlsUI.aimingButton.state = (int)ControlsSettings.aiming;
			MenuConfigurationControlsUI.sensitivityField.state = ControlsSettings.sensitivity;
			MenuConfigurationControlsUI.invertToggle.state = ControlsSettings.invert;
			MenuConfigurationControlsUI.invertFlightToggle.state = ControlsSettings.invert;
		}

		private static byte[][] layouts = new byte[][]
		{
			new byte[]
			{
				ControlsSettings.UP,
				ControlsSettings.DOWN,
				ControlsSettings.LEFT,
				ControlsSettings.RIGHT,
				ControlsSettings.JUMP,
				ControlsSettings.SPRINT
			},
			new byte[]
			{
				ControlsSettings.CROUCH,
				ControlsSettings.PRONE,
				ControlsSettings.STANCE,
				ControlsSettings.LEAN_LEFT,
				ControlsSettings.LEAN_RIGHT,
				ControlsSettings.PERSPECTIVE,
				ControlsSettings.GESTURE
			},
			new byte[]
			{
				ControlsSettings.INTERACT,
				ControlsSettings.PRIMARY,
				ControlsSettings.SECONDARY
			},
			new byte[]
			{
				ControlsSettings.RELOAD,
				ControlsSettings.ATTACH,
				ControlsSettings.FIREMODE,
				ControlsSettings.TACTICAL,
				ControlsSettings.VISION,
				ControlsSettings.INSPECT,
				ControlsSettings.ROTATE,
				ControlsSettings.DEQUIP
			},
			new byte[]
			{
				ControlsSettings.VOICE,
				ControlsSettings.GLOBAL,
				ControlsSettings.LOCAL,
				ControlsSettings.GROUP
			},
			new byte[]
			{
				ControlsSettings.HUD,
				ControlsSettings.OTHER,
				ControlsSettings.DASHBOARD,
				ControlsSettings.INVENTORY,
				ControlsSettings.CRAFTING,
				ControlsSettings.SKILLS,
				ControlsSettings.MAP,
				ControlsSettings.QUESTS,
				ControlsSettings.PLAYERS
			},
			new byte[]
			{
				ControlsSettings.LOCKER,
				ControlsSettings.ROLL_LEFT,
				ControlsSettings.ROLL_RIGHT,
				ControlsSettings.PITCH_UP,
				ControlsSettings.PITCH_DOWN,
				ControlsSettings.YAW_LEFT,
				ControlsSettings.YAW_RIGHT,
				ControlsSettings.THRUST_INCREASE,
				ControlsSettings.THRUST_DECREASE
			},
			new byte[]
			{
				ControlsSettings.MODIFY,
				ControlsSettings.SNAP,
				ControlsSettings.FOCUS,
				ControlsSettings.ASCEND,
				ControlsSettings.DESCEND,
				ControlsSettings.TOOL_0,
				ControlsSettings.TOOL_1,
				ControlsSettings.TOOL_2,
				ControlsSettings.TOOL_3,
				ControlsSettings.TERMINAL,
				ControlsSettings.SCREENSHOT,
				ControlsSettings.REFRESH_ASSETS,
				ControlsSettings.CLIPBOARD_DEBUG
			}
		};

		private static Local localization;

		private static Local localizationKeyCodes;

		private static Sleek container;

		public static bool active;

		private static SleekButtonIcon backButton;

		private static SleekButton defaultButton;

		private static SleekSingleField sensitivityField;

		private static SleekToggle invertToggle;

		private static SleekToggle invertFlightToggle;

		private static SleekScrollBox controlsBox;

		private static SleekButton[] buttons;

		private static SleekButtonState aimingButton;

		private static SleekButtonState crouchingButton;

		private static SleekButtonState proningButton;

		private static SleekButtonState sprintingButton;

		private static SleekButtonState leaningButton;

		public static byte binding;

		[CompilerGenerated]
		private static TypedSingle <>f__mg$cache0;

		[CompilerGenerated]
		private static Toggled <>f__mg$cache1;

		[CompilerGenerated]
		private static Toggled <>f__mg$cache2;

		[CompilerGenerated]
		private static SwappedState <>f__mg$cache3;

		[CompilerGenerated]
		private static SwappedState <>f__mg$cache4;

		[CompilerGenerated]
		private static SwappedState <>f__mg$cache5;

		[CompilerGenerated]
		private static SwappedState <>f__mg$cache6;

		[CompilerGenerated]
		private static SwappedState <>f__mg$cache7;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache8;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache9;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cacheA;
	}
}
