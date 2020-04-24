using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class ControlsSettings
	{
		public static float look
		{
			get
			{
				return ControlsSettings.sensitivity * 2f;
			}
		}

		public static ControlBinding[] bindings
		{
			get
			{
				return ControlsSettings._bindings;
			}
		}

		public static KeyCode left
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.LEFT].key;
			}
		}

		public static KeyCode up
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.UP].key;
			}
		}

		public static KeyCode right
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.RIGHT].key;
			}
		}

		public static KeyCode down
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.DOWN].key;
			}
		}

		public static KeyCode jump
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.JUMP].key;
			}
		}

		public static KeyCode leanLeft
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.LEAN_LEFT].key;
			}
		}

		public static KeyCode leanRight
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.LEAN_RIGHT].key;
			}
		}

		public static KeyCode rollLeft
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.ROLL_LEFT].key;
			}
		}

		public static KeyCode rollRight
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.ROLL_RIGHT].key;
			}
		}

		public static KeyCode pitchUp
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.PITCH_UP].key;
			}
		}

		public static KeyCode pitchDown
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.PITCH_DOWN].key;
			}
		}

		public static KeyCode primary
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.PRIMARY].key;
			}
		}

		public static KeyCode yawLeft
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.YAW_LEFT].key;
			}
		}

		public static KeyCode yawRight
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.YAW_RIGHT].key;
			}
		}

		public static KeyCode thrustIncrease
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.THRUST_INCREASE].key;
			}
		}

		public static KeyCode thrustDecrease
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.THRUST_DECREASE].key;
			}
		}

		public static KeyCode locker
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.LOCKER].key;
			}
		}

		public static KeyCode secondary
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.SECONDARY].key;
			}
		}

		public static KeyCode reload
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.RELOAD].key;
			}
		}

		public static KeyCode attach
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.ATTACH].key;
			}
		}

		public static KeyCode firemode
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.FIREMODE].key;
			}
		}

		public static KeyCode dashboard
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.DASHBOARD].key;
			}
		}

		public static KeyCode inventory
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.INVENTORY].key;
			}
		}

		public static KeyCode crafting
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.CRAFTING].key;
			}
		}

		public static KeyCode skills
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.SKILLS].key;
			}
		}

		public static KeyCode map
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.MAP].key;
			}
		}

		public static KeyCode quests
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.QUESTS].key;
			}
		}

		public static KeyCode players
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.PLAYERS].key;
			}
		}

		public static KeyCode voice
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.VOICE].key;
			}
		}

		public static KeyCode interact
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.INTERACT].key;
			}
		}

		public static KeyCode crouch
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.CROUCH].key;
			}
		}

		public static KeyCode prone
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.PRONE].key;
			}
		}

		public static KeyCode stance
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.STANCE].key;
			}
		}

		public static KeyCode sprint
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.SPRINT].key;
			}
		}

		public static KeyCode modify
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.MODIFY].key;
			}
		}

		public static KeyCode snap
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.SNAP].key;
			}
		}

		public static KeyCode focus
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.FOCUS].key;
			}
		}

		public static KeyCode ascend
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.ASCEND].key;
			}
		}

		public static KeyCode descend
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.DESCEND].key;
			}
		}

		public static KeyCode tool_0
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.TOOL_0].key;
			}
		}

		public static KeyCode tool_1
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.TOOL_1].key;
			}
		}

		public static KeyCode tool_2
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.TOOL_2].key;
			}
		}

		public static KeyCode tool_3
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.TOOL_3].key;
			}
		}

		public static KeyCode terminal
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.TERMINAL].key;
			}
		}

		public static KeyCode screenshot
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.SCREENSHOT].key;
			}
		}

		public static KeyCode refreshAssets
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.REFRESH_ASSETS].key;
			}
		}

		public static KeyCode clipboardDebug
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.CLIPBOARD_DEBUG].key;
			}
		}

		public static KeyCode hud
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.HUD].key;
			}
		}

		public static KeyCode other
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.OTHER].key;
			}
		}

		public static KeyCode global
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.GLOBAL].key;
			}
		}

		public static KeyCode local
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.LOCAL].key;
			}
		}

		public static KeyCode group
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.GROUP].key;
			}
		}

		public static KeyCode gesture
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.GESTURE].key;
			}
		}

		public static KeyCode vision
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.VISION].key;
			}
		}

		public static KeyCode tactical
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.TACTICAL].key;
			}
		}

		public static KeyCode perspective
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.PERSPECTIVE].key;
			}
		}

		public static KeyCode dequip
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.DEQUIP].key;
			}
		}

		public static KeyCode inspect
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.INSPECT].key;
			}
		}

		public static KeyCode rotate
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.ROTATE].key;
			}
		}

		private static bool isTooImportantToMessUp(KeyCode key)
		{
			return key == 323 || key == 324;
		}

		public static void bind(byte index, KeyCode key)
		{
			if (index == ControlsSettings.HUD)
			{
				if (ControlsSettings.isTooImportantToMessUp(key))
				{
					key = 278;
				}
			}
			else if (index == ControlsSettings.OTHER)
			{
				if (ControlsSettings.isTooImportantToMessUp(key))
				{
					key = 306;
				}
			}
			else if (index == ControlsSettings.TERMINAL)
			{
				if (ControlsSettings.isTooImportantToMessUp(key))
				{
					key = 96;
				}
			}
			else if (index == ControlsSettings.REFRESH_ASSETS && ControlsSettings.isTooImportantToMessUp(key))
			{
				key = 280;
			}
			if (ControlsSettings.bindings[(int)index] == null)
			{
				ControlsSettings.bindings[(int)index] = new ControlBinding(key);
				return;
			}
			ControlsSettings.bindings[(int)index].key = key;
		}

		public static void restoreDefaults()
		{
			ControlsSettings.bind(ControlsSettings.LEFT, 97);
			ControlsSettings.bind(ControlsSettings.RIGHT, 100);
			ControlsSettings.bind(ControlsSettings.UP, 119);
			ControlsSettings.bind(ControlsSettings.DOWN, 115);
			ControlsSettings.bind(ControlsSettings.JUMP, 32);
			ControlsSettings.bind(ControlsSettings.LEAN_LEFT, 113);
			ControlsSettings.bind(ControlsSettings.LEAN_RIGHT, 101);
			ControlsSettings.bind(ControlsSettings.PRIMARY, 323);
			ControlsSettings.bind(ControlsSettings.SECONDARY, 324);
			ControlsSettings.bind(ControlsSettings.INTERACT, 102);
			ControlsSettings.bind(ControlsSettings.CROUCH, 120);
			ControlsSettings.bind(ControlsSettings.PRONE, 122);
			ControlsSettings.bind(ControlsSettings.STANCE, 111);
			ControlsSettings.bind(ControlsSettings.SPRINT, 304);
			ControlsSettings.bind(ControlsSettings.RELOAD, 114);
			ControlsSettings.bind(ControlsSettings.ATTACH, 116);
			ControlsSettings.bind(ControlsSettings.FIREMODE, 118);
			ControlsSettings.bind(ControlsSettings.DASHBOARD, 9);
			ControlsSettings.bind(ControlsSettings.INVENTORY, 103);
			ControlsSettings.bind(ControlsSettings.CRAFTING, 121);
			ControlsSettings.bind(ControlsSettings.SKILLS, 117);
			ControlsSettings.bind(ControlsSettings.MAP, 109);
			ControlsSettings.bind(ControlsSettings.QUESTS, 105);
			ControlsSettings.bind(ControlsSettings.PLAYERS, 112);
			ControlsSettings.bind(ControlsSettings.VOICE, 308);
			ControlsSettings.bind(ControlsSettings.MODIFY, 304);
			ControlsSettings.bind(ControlsSettings.SNAP, 306);
			ControlsSettings.bind(ControlsSettings.FOCUS, 102);
			ControlsSettings.bind(ControlsSettings.ASCEND, 113);
			ControlsSettings.bind(ControlsSettings.DESCEND, 101);
			ControlsSettings.bind(ControlsSettings.TOOL_0, 113);
			ControlsSettings.bind(ControlsSettings.TOOL_1, 119);
			ControlsSettings.bind(ControlsSettings.TOOL_2, 101);
			ControlsSettings.bind(ControlsSettings.TOOL_3, 114);
			ControlsSettings.bind(ControlsSettings.TERMINAL, 96);
			ControlsSettings.bind(ControlsSettings.SCREENSHOT, 277);
			ControlsSettings.bind(ControlsSettings.REFRESH_ASSETS, 280);
			ControlsSettings.bind(ControlsSettings.CLIPBOARD_DEBUG, 281);
			ControlsSettings.bind(ControlsSettings.HUD, 278);
			ControlsSettings.bind(ControlsSettings.OTHER, 306);
			ControlsSettings.bind(ControlsSettings.GLOBAL, 106);
			ControlsSettings.bind(ControlsSettings.LOCAL, 107);
			ControlsSettings.bind(ControlsSettings.GROUP, 108);
			ControlsSettings.bind(ControlsSettings.GESTURE, 99);
			ControlsSettings.bind(ControlsSettings.VISION, 110);
			ControlsSettings.bind(ControlsSettings.TACTICAL, 98);
			ControlsSettings.bind(ControlsSettings.PERSPECTIVE, 104);
			ControlsSettings.bind(ControlsSettings.DEQUIP, 301);
			ControlsSettings.bind(ControlsSettings.ROLL_LEFT, 276);
			ControlsSettings.bind(ControlsSettings.ROLL_RIGHT, 275);
			ControlsSettings.bind(ControlsSettings.PITCH_UP, 273);
			ControlsSettings.bind(ControlsSettings.PITCH_DOWN, 274);
			ControlsSettings.bind(ControlsSettings.YAW_LEFT, 97);
			ControlsSettings.bind(ControlsSettings.YAW_RIGHT, 100);
			ControlsSettings.bind(ControlsSettings.THRUST_INCREASE, 119);
			ControlsSettings.bind(ControlsSettings.THRUST_DECREASE, 115);
			ControlsSettings.bind(ControlsSettings.LOCKER, 111);
			ControlsSettings.bind(ControlsSettings.INSPECT, 102);
			ControlsSettings.bind(ControlsSettings.ROTATE, 114);
			ControlsSettings.aiming = EControlMode.HOLD;
			ControlsSettings.crouching = EControlMode.TOGGLE;
			ControlsSettings.proning = EControlMode.TOGGLE;
			ControlsSettings.sprinting = EControlMode.HOLD;
			ControlsSettings.leaning = EControlMode.HOLD;
			ControlsSettings.sensitivity = 1f;
			ControlsSettings.invert = false;
			ControlsSettings.invertFlight = false;
		}

		public static void load()
		{
			ControlsSettings.restoreDefaults();
			if (ReadWrite.fileExists("/Controls.dat", true))
			{
				Block block = ReadWrite.readBlock("/Controls.dat", true, 0);
				if (block != null)
				{
					byte b = block.readByte();
					if (b > 10)
					{
						ControlsSettings.sensitivity = block.readSingle();
						if (b < 16)
						{
							ControlsSettings.sensitivity = 1f;
						}
						ControlsSettings.invert = block.readBoolean();
						if (b > 13)
						{
							ControlsSettings.invertFlight = block.readBoolean();
						}
						else
						{
							ControlsSettings.invertFlight = false;
						}
						if (b > 11)
						{
							ControlsSettings.aiming = (EControlMode)block.readByte();
							ControlsSettings.crouching = (EControlMode)block.readByte();
							ControlsSettings.proning = (EControlMode)block.readByte();
						}
						else
						{
							ControlsSettings.aiming = EControlMode.HOLD;
							ControlsSettings.crouching = EControlMode.TOGGLE;
							ControlsSettings.proning = EControlMode.TOGGLE;
						}
						if (b > 12)
						{
							ControlsSettings.sprinting = (EControlMode)block.readByte();
						}
						else
						{
							ControlsSettings.sprinting = EControlMode.HOLD;
						}
						if (b > 14)
						{
							ControlsSettings.leaning = (EControlMode)block.readByte();
						}
						else
						{
							ControlsSettings.leaning = EControlMode.HOLD;
						}
						byte b2 = block.readByte();
						for (byte b3 = 0; b3 < b2; b3 += 1)
						{
							if ((int)b3 >= ControlsSettings.bindings.Length)
							{
								block.readByte();
							}
							else
							{
								ushort key = block.readUInt16();
								ControlsSettings.bind(b3, key);
							}
						}
						if (b < 17)
						{
							ControlsSettings.bind(ControlsSettings.DEQUIP, 301);
						}
					}
				}
			}
		}

		public static void save()
		{
			Block block = new Block();
			block.writeByte(ControlsSettings.SAVEDATA_VERSION);
			block.writeSingle(ControlsSettings.sensitivity);
			block.writeBoolean(ControlsSettings.invert);
			block.writeBoolean(ControlsSettings.invertFlight);
			block.writeByte((byte)ControlsSettings.aiming);
			block.writeByte((byte)ControlsSettings.crouching);
			block.writeByte((byte)ControlsSettings.proning);
			block.writeByte((byte)ControlsSettings.sprinting);
			block.writeByte((byte)ControlsSettings.leaning);
			block.writeByte((byte)ControlsSettings.bindings.Length);
			byte b = 0;
			while ((int)b < ControlsSettings.bindings.Length)
			{
				ControlBinding controlBinding = ControlsSettings.bindings[(int)b];
				block.writeUInt16(controlBinding.key);
				b += 1;
			}
			ReadWrite.writeBlock("/Controls.dat", true, block);
		}

		public static readonly byte SAVEDATA_VERSION = 17;

		public static readonly byte LEFT = 0;

		public static readonly byte RIGHT = 1;

		public static readonly byte UP = 2;

		public static readonly byte DOWN = 3;

		public static readonly byte JUMP = 4;

		public static readonly byte LEAN_LEFT = 5;

		public static readonly byte LEAN_RIGHT = 6;

		public static readonly byte PRIMARY = 7;

		public static readonly byte SECONDARY = 8;

		public static readonly byte INTERACT = 9;

		public static readonly byte CROUCH = 10;

		public static readonly byte PRONE = 11;

		public static readonly byte SPRINT = 12;

		public static readonly byte RELOAD = 13;

		public static readonly byte ATTACH = 14;

		public static readonly byte FIREMODE = 15;

		public static readonly byte DASHBOARD = 16;

		public static readonly byte INVENTORY = 17;

		public static readonly byte CRAFTING = 18;

		public static readonly byte SKILLS = 19;

		public static readonly byte MAP = 20;

		public static readonly byte QUESTS = 54;

		public static readonly byte PLAYERS = 21;

		public static readonly byte VOICE = 22;

		public static readonly byte MODIFY = 23;

		public static readonly byte SNAP = 24;

		public static readonly byte FOCUS = 25;

		public static readonly byte ASCEND = 51;

		public static readonly byte DESCEND = 52;

		public static readonly byte TOOL_0 = 26;

		public static readonly byte TOOL_1 = 27;

		public static readonly byte TOOL_2 = 28;

		public static readonly byte TOOL_3 = 50;

		public static readonly byte TERMINAL = 55;

		public static readonly byte SCREENSHOT = 56;

		public static readonly byte REFRESH_ASSETS = 57;

		public static readonly byte CLIPBOARD_DEBUG = 58;

		public static readonly byte HUD = 29;

		public static readonly byte OTHER = 30;

		public static readonly byte GLOBAL = 31;

		public static readonly byte LOCAL = 32;

		public static readonly byte GROUP = 33;

		public static readonly byte GESTURE = 34;

		public static readonly byte VISION = 35;

		public static readonly byte TACTICAL = 36;

		public static readonly byte PERSPECTIVE = 37;

		public static readonly byte DEQUIP = 38;

		public static readonly byte STANCE = 39;

		public static readonly byte ROLL_LEFT = 40;

		public static readonly byte ROLL_RIGHT = 41;

		public static readonly byte PITCH_UP = 42;

		public static readonly byte PITCH_DOWN = 43;

		public static readonly byte YAW_LEFT = 44;

		public static readonly byte YAW_RIGHT = 45;

		public static readonly byte THRUST_INCREASE = 46;

		public static readonly byte THRUST_DECREASE = 47;

		public static readonly byte LOCKER = 53;

		public static readonly byte INSPECT = 48;

		public static readonly byte ROTATE = 49;

		public static float sensitivity;

		public static bool invert;

		public static bool invertFlight;

		public static EControlMode aiming;

		public static EControlMode crouching;

		public static EControlMode proning;

		public static EControlMode sprinting;

		public static EControlMode leaning;

		private static ControlBinding[] _bindings = new ControlBinding[59];
	}
}
