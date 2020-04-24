using System;
using SDG.Framework.UI.Devkit;
using SDG.Framework.Water;
using UnityEngine;

namespace SDG.Unturned
{
	public class PlayerUI : MonoBehaviour
	{
		public static bool isBlindfolded
		{
			get
			{
				return PlayerUI._isBlindfolded;
			}
			set
			{
				if (PlayerUI.isBlindfolded == value)
				{
					return;
				}
				PlayerUI._isBlindfolded = value;
				PlayerUI.isBlindfoldedChanged();
			}
		}

		public static event IsBlindfoldedChangedHandler isBlindfoldedChanged;

		public static EChatMode chat
		{
			get
			{
				return PlayerUI._chat;
			}
		}

		public static void rebuild()
		{
			PlayerUI.ui.Invoke("init", 0.1f);
		}

		public void build()
		{
			if (PlayerUI.window != null)
			{
				PlayerUI.window.build();
			}
			LoadingUI.rebuild();
		}

		public void init()
		{
			GraphicsSettings.resize();
			base.Invoke("build", 0.1f);
		}

		public static void stun(float amount)
		{
			PlayerUI.stunColor.a = amount * 5f;
			MainCamera.instance.GetComponent<AudioSource>().PlayOneShot((AudioClip)Resources.Load("Sounds/General/Stun"), amount);
		}

		public static void pain(float amount)
		{
			Color backgroundColor = PlayerLifeUI.painImage.backgroundColor;
			backgroundColor.a = amount * 0.75f;
			PlayerLifeUI.painImage.backgroundColor = backgroundColor;
		}

		public static void hitmark(int index, Vector3 point, bool worldspace, EPlayerHit newHit)
		{
			if (!PlayerUI.window.isEnabled || (PlayerUI.isOverlayed && !PlayerUI.isReverting))
			{
				return;
			}
			if (index < 0 || index >= PlayerLifeUI.hitmarkers.Length)
			{
				return;
			}
			if (!Provider.modeConfigData.Gameplay.Hitmarkers)
			{
				return;
			}
			HitmarkerInfo hitmarkerInfo = PlayerLifeUI.hitmarkers[index];
			hitmarkerInfo.lastHit = Time.realtimeSinceStartup;
			hitmarkerInfo.hit = newHit;
			hitmarkerInfo.point = point;
			hitmarkerInfo.worldspace = (worldspace || OptionsSettings.hitmarker);
			if (newHit == EPlayerHit.CRITICAL)
			{
				MainCamera.instance.GetComponent<AudioSource>().PlayOneShot((AudioClip)Resources.Load("Sounds/General/Hit"), 0.5f);
			}
		}

		public static void enableDot()
		{
			PlayerLifeUI.dotImage.isVisible = true;
		}

		public static void disableDot()
		{
			PlayerLifeUI.dotImage.isVisible = false;
		}

		public static void updateScope(bool isScoped)
		{
			PlayerLifeUI.scopeOverlay.isVisible = isScoped;
			PlayerUI.container.isVisible = !isScoped;
			PlayerUI.isOverlayed = isScoped;
			if (PlayerUI.isOverlayed)
			{
				PlayerUI.isReverting = PlayerUI.window.isEnabled;
				PlayerUI.wasOverlayed = true;
				PlayerUI.window.isEnabled = true;
			}
			else if (PlayerUI.wasOverlayed)
			{
				PlayerUI.window.isEnabled = PlayerUI.isReverting;
				PlayerUI.wasOverlayed = false;
			}
		}

		public static void updateBinoculars(bool isBinoculars)
		{
			PlayerLifeUI.binocularsOverlay.isVisible = isBinoculars;
			PlayerUI.container.isVisible = !isBinoculars;
			PlayerUI.isOverlayed = isBinoculars;
			if (PlayerUI.isOverlayed)
			{
				PlayerUI.isReverting = PlayerUI.window.isEnabled;
				PlayerUI.wasOverlayed = true;
				PlayerUI.window.isEnabled = true;
			}
			else if (PlayerUI.wasOverlayed)
			{
				PlayerUI.window.isEnabled = PlayerUI.isReverting;
				PlayerUI.wasOverlayed = false;
			}
		}

		public static void resetCrosshair()
		{
			if (Provider.modeConfigData.Gameplay.Crosshair)
			{
				PlayerLifeUI.crosshairLeftImage.positionOffset_X = -4;
				PlayerLifeUI.crosshairLeftImage.positionOffset_Y = -4;
				PlayerLifeUI.crosshairRightImage.positionOffset_X = -4;
				PlayerLifeUI.crosshairRightImage.positionOffset_Y = -4;
				PlayerLifeUI.crosshairDownImage.positionOffset_X = -4;
				PlayerLifeUI.crosshairDownImage.positionOffset_Y = -4;
				PlayerLifeUI.crosshairUpImage.positionOffset_X = -4;
				PlayerLifeUI.crosshairUpImage.positionOffset_Y = -4;
			}
		}

		public static void updateCrosshair(float spread)
		{
			if (Provider.modeConfigData.Gameplay.Crosshair)
			{
				PlayerLifeUI.crosshairLeftImage.lerpPositionOffset((int)(-spread * 400f) - 4, -4, ESleekLerp.EXPONENTIAL, 10f);
				PlayerLifeUI.crosshairRightImage.lerpPositionOffset((int)(spread * 400f) - 4, -4, ESleekLerp.EXPONENTIAL, 10f);
				PlayerLifeUI.crosshairDownImage.lerpPositionOffset(-4, (int)(spread * 400f) - 4, ESleekLerp.EXPONENTIAL, 10f);
				PlayerLifeUI.crosshairUpImage.lerpPositionOffset(-4, (int)(-spread * 400f) - 4, ESleekLerp.EXPONENTIAL, 10f);
			}
		}

		public static void enableCrosshair()
		{
			if (Provider.modeConfigData.Gameplay.Crosshair)
			{
				PlayerLifeUI.crosshairLeftImage.isVisible = true;
				PlayerLifeUI.crosshairRightImage.isVisible = true;
				PlayerLifeUI.crosshairDownImage.isVisible = true;
				PlayerLifeUI.crosshairUpImage.isVisible = true;
			}
		}

		public static void disableCrosshair()
		{
			if (Provider.modeConfigData.Gameplay.Crosshair)
			{
				PlayerLifeUI.crosshairLeftImage.isVisible = false;
				PlayerLifeUI.crosshairRightImage.isVisible = false;
				PlayerLifeUI.crosshairDownImage.isVisible = false;
				PlayerLifeUI.crosshairUpImage.isVisible = false;
			}
		}

		public static void hint(Transform transform, EPlayerMessage message)
		{
			PlayerUI.hint(transform, message, string.Empty, Color.white, new object[0]);
		}

		public static void hint(Transform transform, EPlayerMessage message, string text, Color color, params object[] objects)
		{
			if (PlayerUI.messageBox == null)
			{
				return;
			}
			PlayerUI.lastHinted = true;
			PlayerUI.isHinted = true;
			if (message == EPlayerMessage.ENEMY)
			{
				if (objects.Length == 1)
				{
					SteamPlayer steamPlayer = (SteamPlayer)objects[0];
					if (PlayerUI.messagePlayer != null && PlayerUI.messagePlayer.player != steamPlayer)
					{
						PlayerUI.container.remove(PlayerUI.messagePlayer);
						PlayerUI.messagePlayer = null;
					}
					if (PlayerUI.messagePlayer == null)
					{
						PlayerUI.messagePlayer = new SleekPlayer(steamPlayer, false, SleekPlayer.ESleekPlayerDisplayContext.NONE);
						PlayerUI.messagePlayer.positionOffset_X = -150;
						PlayerUI.messagePlayer.positionOffset_Y = -130;
						PlayerUI.messagePlayer.positionScale_X = 0.5f;
						PlayerUI.messagePlayer.positionScale_Y = 1f;
						PlayerUI.messagePlayer.sizeOffset_X = 300;
						PlayerUI.messagePlayer.sizeOffset_Y = 50;
						PlayerUI.container.add(PlayerUI.messagePlayer);
					}
				}
				PlayerUI.messageBox.isVisible = false;
				PlayerUI.messagePlayer.isVisible = true;
				return;
			}
			PlayerUI.messageBox.isVisible = true;
			if (PlayerUI.messagePlayer != null)
			{
				PlayerUI.messagePlayer.isVisible = false;
			}
			if (message == EPlayerMessage.VEHICLE_ENTER)
			{
				PlayerUI.messageBox.sizeOffset_Y = 130;
				PlayerUI.messageProgress_0.isVisible = true;
				PlayerUI.messageIcon_0.isVisible = true;
				PlayerUI.messageProgress_1.isVisible = true;
				PlayerUI.messageIcon_1.isVisible = true;
				PlayerUI.messageProgress_2.isVisible = true;
				PlayerUI.messageIcon_2.isVisible = true;
				InteractableVehicle interactableVehicle = (InteractableVehicle)PlayerInteract.interactable;
				PlayerUI.messageProgress_0.state = (float)interactableVehicle.batteryCharge / 10000f;
				PlayerUI.messageProgress_0.color = Palette.COLOR_Y;
				PlayerUI.messageIcon_0.texture = (Texture2D)PlayerLifeUI.icons.load("Stamina");
				PlayerUI.messageProgress_1.state = (float)interactableVehicle.fuel / (float)interactableVehicle.asset.fuel;
				PlayerUI.messageProgress_1.color = Palette.COLOR_Y;
				PlayerUI.messageIcon_1.texture = (Texture2D)PlayerLifeUI.icons.load("Fuel");
				PlayerUI.messageProgress_2.state = (float)interactableVehicle.health / (float)interactableVehicle.asset.health;
				PlayerUI.messageProgress_2.color = Palette.COLOR_R;
				PlayerUI.messageIcon_2.texture = (Texture2D)PlayerLifeUI.icons.load("Health");
				PlayerUI.messageQualityImage.isVisible = false;
				PlayerUI.messageAmountLabel.isVisible = false;
			}
			else if (message == EPlayerMessage.GENERATOR_ON || message == EPlayerMessage.GENERATOR_OFF || message == EPlayerMessage.GROW || message == EPlayerMessage.VOLUME_WATER || message == EPlayerMessage.VOLUME_FUEL)
			{
				PlayerUI.messageBox.sizeOffset_Y = 70;
				PlayerUI.messageProgress_0.isVisible = true;
				PlayerUI.messageIcon_0.isVisible = true;
				PlayerUI.messageProgress_1.isVisible = false;
				PlayerUI.messageIcon_1.isVisible = false;
				PlayerUI.messageProgress_2.isVisible = false;
				PlayerUI.messageIcon_2.isVisible = false;
				if (message == EPlayerMessage.GENERATOR_ON || message == EPlayerMessage.GENERATOR_OFF)
				{
					InteractableGenerator interactableGenerator = (InteractableGenerator)PlayerInteract.interactable;
					PlayerUI.messageProgress_0.state = (float)interactableGenerator.fuel / (float)interactableGenerator.capacity;
					PlayerUI.messageIcon_0.texture = (Texture2D)PlayerLifeUI.icons.load("Fuel");
				}
				else if (message == EPlayerMessage.GROW)
				{
					InteractableFarm interactableFarm = (InteractableFarm)PlayerInteract.interactable;
					float num = 0f;
					if (interactableFarm.planted > 0u && Provider.time > interactableFarm.planted)
					{
						num = Provider.time - interactableFarm.planted;
					}
					PlayerUI.messageProgress_0.state = num / interactableFarm.growth;
					PlayerUI.messageIcon_0.texture = (Texture2D)PlayerLifeUI.icons.load("Grow");
				}
				else if (message == EPlayerMessage.VOLUME_WATER)
				{
					if (PlayerInteract.interactable is InteractableObjectResource)
					{
						InteractableObjectResource interactableObjectResource = (InteractableObjectResource)PlayerInteract.interactable;
						PlayerUI.messageProgress_0.state = (float)interactableObjectResource.amount / (float)interactableObjectResource.capacity;
					}
					else if (PlayerInteract.interactable is InteractableTank)
					{
						InteractableTank interactableTank = (InteractableTank)PlayerInteract.interactable;
						PlayerUI.messageProgress_0.state = (float)interactableTank.amount / (float)interactableTank.capacity;
					}
					else if (PlayerInteract.interactable is InteractableRainBarrel)
					{
						InteractableRainBarrel interactableRainBarrel = (InteractableRainBarrel)PlayerInteract.interactable;
						PlayerUI.messageProgress_0.state = ((!interactableRainBarrel.isFull) ? 0f : 1f);
						if (interactableRainBarrel.isFull)
						{
							text = PlayerLifeUI.localization.format("Full");
						}
						else
						{
							text = PlayerLifeUI.localization.format("Empty");
						}
					}
					PlayerUI.messageIcon_0.texture = (Texture2D)PlayerLifeUI.icons.load("Water");
				}
				else if (message == EPlayerMessage.VOLUME_FUEL)
				{
					if (PlayerInteract.interactable is InteractableObjectResource)
					{
						InteractableObjectResource interactableObjectResource2 = (InteractableObjectResource)PlayerInteract.interactable;
						PlayerUI.messageProgress_0.state = (float)interactableObjectResource2.amount / (float)interactableObjectResource2.capacity;
					}
					else if (PlayerInteract.interactable is InteractableTank)
					{
						InteractableTank interactableTank2 = (InteractableTank)PlayerInteract.interactable;
						PlayerUI.messageProgress_0.state = (float)interactableTank2.amount / (float)interactableTank2.capacity;
					}
					else if (PlayerInteract.interactable is InteractableOil)
					{
						InteractableOil interactableOil = (InteractableOil)PlayerInteract.interactable;
						PlayerUI.messageProgress_0.state = (float)interactableOil.fuel / (float)interactableOil.capacity;
					}
					PlayerUI.messageIcon_0.texture = (Texture2D)PlayerLifeUI.icons.load("Fuel");
				}
				if (message == EPlayerMessage.GROW)
				{
					PlayerUI.messageProgress_0.color = Palette.COLOR_G;
				}
				else if (message == EPlayerMessage.VOLUME_WATER)
				{
					PlayerUI.messageProgress_0.color = Palette.COLOR_B;
				}
				else
				{
					PlayerUI.messageProgress_0.color = Palette.COLOR_Y;
				}
				PlayerUI.messageQualityImage.isVisible = false;
				PlayerUI.messageAmountLabel.isVisible = false;
			}
			else if (message == EPlayerMessage.ITEM)
			{
				PlayerUI.messageBox.sizeOffset_Y = 70;
				if (objects.Length == 2)
				{
					if (((ItemAsset)objects[1]).showQuality)
					{
						PlayerUI.messageQualityImage.backgroundColor = ItemTool.getQualityColor((float)((Item)objects[0]).quality / 100f);
						PlayerUI.messageAmountLabel.text = ((Item)objects[0]).quality + "%";
						PlayerUI.messageQualityImage.isVisible = true;
						PlayerUI.messageAmountLabel.isVisible = true;
					}
					else if (((ItemAsset)objects[1]).amount > 1)
					{
						PlayerUI.messageQualityImage.backgroundColor = Color.white;
						PlayerUI.messageAmountLabel.text = "x" + ((Item)objects[0]).amount;
						PlayerUI.messageQualityImage.isVisible = false;
						PlayerUI.messageAmountLabel.isVisible = true;
					}
					else
					{
						PlayerUI.messageQualityImage.isVisible = false;
						PlayerUI.messageAmountLabel.isVisible = false;
					}
				}
				PlayerUI.messageQualityImage.foregroundColor = PlayerUI.messageQualityImage.backgroundColor;
				PlayerUI.messageAmountLabel.backgroundColor = PlayerUI.messageQualityImage.backgroundColor;
				PlayerUI.messageAmountLabel.foregroundColor = PlayerUI.messageQualityImage.backgroundColor;
				PlayerUI.messageProgress_0.isVisible = false;
				PlayerUI.messageIcon_0.isVisible = false;
				PlayerUI.messageProgress_1.isVisible = false;
				PlayerUI.messageIcon_1.isVisible = false;
				PlayerUI.messageProgress_2.isVisible = false;
				PlayerUI.messageIcon_2.isVisible = false;
			}
			else
			{
				PlayerUI.messageBox.sizeOffset_Y = 50;
				PlayerUI.messageQualityImage.isVisible = false;
				PlayerUI.messageAmountLabel.isVisible = false;
				PlayerUI.messageProgress_0.isVisible = false;
				PlayerUI.messageIcon_0.isVisible = false;
				PlayerUI.messageProgress_1.isVisible = false;
				PlayerUI.messageIcon_1.isVisible = false;
				PlayerUI.messageProgress_2.isVisible = false;
				PlayerUI.messageIcon_2.isVisible = false;
			}
			PlayerUI.messageLabel.isRich = (message == EPlayerMessage.CONDITION);
			PlayerUI.messageBox.sizeOffset_X = 200;
			if (message == EPlayerMessage.ITEM)
			{
				PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Item", new object[]
				{
					text,
					MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.interact)
				});
			}
			else if (message == EPlayerMessage.VEHICLE_ENTER)
			{
				PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Vehicle_Enter", new object[]
				{
					text,
					MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.interact)
				});
			}
			else if (message == EPlayerMessage.DOOR_OPEN)
			{
				PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Door_Open", new object[]
				{
					MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.interact)
				});
			}
			else if (message == EPlayerMessage.DOOR_CLOSE)
			{
				PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Door_Close", new object[]
				{
					MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.interact)
				});
			}
			else if (message == EPlayerMessage.LOCKED)
			{
				PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Locked");
			}
			else if (message == EPlayerMessage.BLOCKED)
			{
				PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Blocked");
			}
			else if (message == EPlayerMessage.PILLAR)
			{
				PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Pillar");
			}
			else if (message == EPlayerMessage.POST)
			{
				PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Post");
			}
			else if (message == EPlayerMessage.ROOF)
			{
				PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Roof");
			}
			else if (message == EPlayerMessage.WALL)
			{
				PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Wall");
			}
			else if (message == EPlayerMessage.CORNER)
			{
				PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Corner");
			}
			else if (message == EPlayerMessage.GROUND)
			{
				PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Ground");
			}
			else if (message == EPlayerMessage.DOORWAY)
			{
				PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Doorway");
			}
			else if (message == EPlayerMessage.WINDOW)
			{
				PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Window");
			}
			else if (message == EPlayerMessage.GARAGE)
			{
				PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Garage");
			}
			else if (message == EPlayerMessage.BED_ON)
			{
				PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Bed_On", new object[]
				{
					MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.interact),
					text
				});
			}
			else if (message == EPlayerMessage.BED_OFF)
			{
				PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Bed_Off", new object[]
				{
					MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.interact),
					text
				});
			}
			else if (message == EPlayerMessage.BED_CLAIMED)
			{
				PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Bed_Claimed");
			}
			else if (message == EPlayerMessage.BOUNDS)
			{
				PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Bounds");
			}
			else if (message == EPlayerMessage.STORAGE)
			{
				PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Storage", new object[]
				{
					MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.interact)
				});
			}
			else if (message == EPlayerMessage.FARM)
			{
				PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Farm", new object[]
				{
					MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.interact)
				});
			}
			else if (message == EPlayerMessage.GROW)
			{
				PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Grow");
			}
			else if (message == EPlayerMessage.SOIL)
			{
				PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Soil");
			}
			else if (message == EPlayerMessage.FIRE_ON)
			{
				PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Fire_On", new object[]
				{
					MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.interact)
				});
			}
			else if (message == EPlayerMessage.FIRE_OFF)
			{
				PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Fire_Off", new object[]
				{
					MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.interact)
				});
			}
			else if (message == EPlayerMessage.FORAGE)
			{
				PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Forage", new object[]
				{
					MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.interact)
				});
			}
			else if (message == EPlayerMessage.GENERATOR_ON)
			{
				PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Generator_On", new object[]
				{
					MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.interact)
				});
			}
			else if (message == EPlayerMessage.GENERATOR_OFF)
			{
				PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Generator_Off", new object[]
				{
					MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.interact)
				});
			}
			else if (message == EPlayerMessage.SPOT_ON)
			{
				PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Spot_On", new object[]
				{
					MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.interact)
				});
			}
			else if (message == EPlayerMessage.SPOT_OFF)
			{
				PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Spot_Off", new object[]
				{
					MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.interact)
				});
			}
			else if (message == EPlayerMessage.PURCHASE)
			{
				if (objects.Length == 2)
				{
					PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Purchase", new object[]
					{
						objects[0],
						objects[1],
						MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.interact)
					});
				}
			}
			else if (message == EPlayerMessage.POWER)
			{
				PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Power");
			}
			else if (message == EPlayerMessage.USE)
			{
				PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Use", new object[]
				{
					MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.interact)
				});
			}
			else if (message == EPlayerMessage.TUTORIAL_MOVE)
			{
				PlayerUI.messageBox.sizeOffset_X = 600;
				PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Tutorial_Move", new object[]
				{
					MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.left),
					MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.right),
					MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.up),
					MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.down)
				});
			}
			else if (message == EPlayerMessage.TUTORIAL_LOOK)
			{
				PlayerUI.messageBox.sizeOffset_X = 600;
				PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Tutorial_Look");
			}
			else if (message == EPlayerMessage.TUTORIAL_JUMP)
			{
				PlayerUI.messageBox.sizeOffset_X = 600;
				PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Tutorial_Jump", new object[]
				{
					MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.jump)
				});
			}
			else if (message == EPlayerMessage.TUTORIAL_PERSPECTIVE)
			{
				PlayerUI.messageBox.sizeOffset_X = 600;
				PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Tutorial_Perspective", new object[]
				{
					MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.perspective)
				});
			}
			else if (message == EPlayerMessage.TUTORIAL_RUN)
			{
				PlayerUI.messageBox.sizeOffset_X = 600;
				PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Tutorial_Run", new object[]
				{
					MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.sprint)
				});
			}
			else if (message == EPlayerMessage.TUTORIAL_INVENTORY)
			{
				PlayerUI.messageBox.sizeOffset_X = 600;
				PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Tutorial_Inventory", new object[]
				{
					MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.interact)
				});
			}
			else if (message == EPlayerMessage.TUTORIAL_SURVIVAL)
			{
				PlayerUI.messageBox.sizeOffset_X = 600;
				PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Tutorial_Survival", new object[]
				{
					MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.inventory),
					MenuConfigurationControlsUI.getKeyCodeText(324)
				});
			}
			else if (message == EPlayerMessage.TUTORIAL_GUN)
			{
				PlayerUI.messageBox.sizeOffset_X = 600;
				PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Tutorial_Gun", new object[]
				{
					MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.secondary),
					MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.primary)
				});
			}
			else if (message == EPlayerMessage.TUTORIAL_LADDER)
			{
				PlayerUI.messageBox.sizeOffset_X = 600;
				PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Tutorial_Ladder");
			}
			else if (message == EPlayerMessage.TUTORIAL_CRAFT)
			{
				PlayerUI.messageBox.sizeOffset_X = 600;
				PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Tutorial_Craft", new object[]
				{
					MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.attach),
					MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.crafting)
				});
			}
			else if (message == EPlayerMessage.TUTORIAL_SKILLS)
			{
				PlayerUI.messageBox.sizeOffset_X = 600;
				PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Tutorial_Skills", new object[]
				{
					MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.skills)
				});
			}
			else if (message == EPlayerMessage.TUTORIAL_SWIM)
			{
				PlayerUI.messageBox.sizeOffset_X = 600;
				PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Tutorial_Swim", new object[]
				{
					MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.jump)
				});
			}
			else if (message == EPlayerMessage.TUTORIAL_MEDICAL)
			{
				PlayerUI.messageBox.sizeOffset_X = 600;
				PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Tutorial_Medical", new object[]
				{
					MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.primary)
				});
			}
			else if (message == EPlayerMessage.TUTORIAL_VEHICLE)
			{
				PlayerUI.messageBox.sizeOffset_X = 600;
				PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Tutorial_Vehicle", new object[]
				{
					MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.secondary),
					MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.primary),
					MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.interact)
				});
			}
			else if (message == EPlayerMessage.TUTORIAL_CROUCH)
			{
				PlayerUI.messageBox.sizeOffset_X = 600;
				PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Tutorial_Crouch", new object[]
				{
					MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.crouch)
				});
			}
			else if (message == EPlayerMessage.TUTORIAL_PRONE)
			{
				PlayerUI.messageBox.sizeOffset_X = 600;
				PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Tutorial_Prone", new object[]
				{
					MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.prone)
				});
			}
			else if (message == EPlayerMessage.TUTORIAL_EDUCATED)
			{
				PlayerUI.messageBox.sizeOffset_X = 600;
				PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Tutorial_Educated", new object[]
				{
					MenuConfigurationControlsUI.getKeyCodeText(27)
				});
			}
			else if (message == EPlayerMessage.TUTORIAL_HARVEST)
			{
				PlayerUI.messageBox.sizeOffset_X = 600;
				PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Tutorial_Harvest", new object[]
				{
					MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.interact)
				});
			}
			else if (message == EPlayerMessage.TUTORIAL_FISH)
			{
				PlayerUI.messageBox.sizeOffset_X = 600;
				PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Tutorial_Fish", new object[]
				{
					MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.primary)
				});
			}
			else if (message == EPlayerMessage.TUTORIAL_BUILD)
			{
				PlayerUI.messageBox.sizeOffset_X = 600;
				PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Tutorial_Build");
			}
			else if (message == EPlayerMessage.TUTORIAL_HORN)
			{
				PlayerUI.messageBox.sizeOffset_X = 600;
				PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Tutorial_Horn", new object[]
				{
					MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.primary)
				});
			}
			else if (message == EPlayerMessage.TUTORIAL_LIGHTS)
			{
				PlayerUI.messageBox.sizeOffset_X = 600;
				PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Tutorial_Lights", new object[]
				{
					MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.secondary)
				});
			}
			else if (message == EPlayerMessage.TUTORIAL_SIRENS)
			{
				PlayerUI.messageBox.sizeOffset_X = 600;
				PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Tutorial_Sirens", new object[]
				{
					MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.other)
				});
			}
			else if (message == EPlayerMessage.TUTORIAL_FARM)
			{
				PlayerUI.messageBox.sizeOffset_X = 600;
				PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Tutorial_Farm");
			}
			else if (message == EPlayerMessage.TUTORIAL_POWER)
			{
				PlayerUI.messageBox.sizeOffset_X = 600;
				PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Tutorial_Power");
			}
			else if (message == EPlayerMessage.TUTORIAL_FIRE)
			{
				PlayerUI.messageBox.sizeOffset_X = 600;
				PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Tutorial_Fire", new object[]
				{
					MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.crafting)
				});
			}
			else if (message == EPlayerMessage.CLAIM)
			{
				PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Claim");
			}
			else if (message == EPlayerMessage.UNDERWATER)
			{
				PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Underwater");
			}
			else if (message == EPlayerMessage.NAV)
			{
				PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Nav");
			}
			else if (message == EPlayerMessage.SPAWN)
			{
				PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Spawn");
			}
			else if (message == EPlayerMessage.MOBILE)
			{
				PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Mobile");
			}
			else if (message == EPlayerMessage.OIL)
			{
				PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Oil");
			}
			else if (message == EPlayerMessage.VOLUME_WATER)
			{
				PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Volume_Water", new object[]
				{
					text
				});
			}
			else if (message == EPlayerMessage.VOLUME_FUEL)
			{
				PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Volume_Fuel");
			}
			else if (message == EPlayerMessage.TRAPDOOR)
			{
				PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Trapdoor");
			}
			else if (message == EPlayerMessage.TALK)
			{
				PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Talk", new object[]
				{
					MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.interact)
				});
			}
			else if (message == EPlayerMessage.CONDITION)
			{
				PlayerUI.messageLabel.text = text;
			}
			else if (message == EPlayerMessage.INTERACT)
			{
				PlayerUI.messageLabel.text = string.Format(text, MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.interact));
			}
			else if (message == EPlayerMessage.SAFEZONE)
			{
				PlayerUI.messageLabel.text = PlayerLifeUI.localization.format("Safezone");
			}
			PlayerUI.messageBox.backgroundColor = color;
			PlayerUI.messageBox.foregroundColor = color;
			PlayerUI.messageLabel.backgroundColor = color;
			PlayerUI.messageLabel.foregroundColor = color;
			if (transform != null && MainCamera.instance != null)
			{
				Vector3 vector = MainCamera.instance.WorldToScreenPoint(transform.position);
				PlayerUI.messageBox.positionOffset_X = (int)(vector.x - (float)(PlayerUI.messageBox.sizeOffset_X / 2));
				PlayerUI.messageBox.positionOffset_Y = (int)((float)Screen.height - vector.y + 10f);
				PlayerUI.messageBox.positionScale_X = 0f;
				PlayerUI.messageBox.positionScale_Y = 0f;
			}
			else
			{
				PlayerUI.messageBox.positionOffset_X = -PlayerUI.messageBox.sizeOffset_X / 2;
				if (PlayerUI.messageBox2.isVisible)
				{
					PlayerUI.messageBox.positionOffset_Y = -80 - PlayerUI.messageBox.sizeOffset_Y - 10 - PlayerUI.messageBox2.sizeOffset_Y;
				}
				else
				{
					PlayerUI.messageBox.positionOffset_Y = -80 - PlayerUI.messageBox.sizeOffset_Y;
				}
				PlayerUI.messageBox.positionScale_X = 0.5f;
				PlayerUI.messageBox.positionScale_Y = 1f;
			}
		}

		public static void hint2(EPlayerMessage message, float progress, float data)
		{
			if (!PlayerUI.isMessaged)
			{
				PlayerUI.messageBox2.isVisible = true;
				PlayerUI.lastHinted2 = true;
				PlayerUI.isHinted2 = true;
				if (message == EPlayerMessage.SALVAGE)
				{
					PlayerUI.messageBox2.sizeOffset_Y = 100;
					PlayerUI.messageBox2.positionOffset_Y = -80 - PlayerUI.messageBox2.sizeOffset_Y;
					PlayerUI.messageIcon2.isVisible = true;
					PlayerUI.messageProgress2_0.isVisible = true;
					PlayerUI.messageProgress2_1.isVisible = true;
					PlayerUI.messageIcon2.texture = (Texture2D)PlayerLifeUI.icons.load("Health");
					PlayerUI.messageLabel2.text = PlayerLifeUI.localization.format("Salvage", new object[]
					{
						ControlsSettings.interact
					});
					PlayerUI.messageProgress2_0.state = progress;
					PlayerUI.messageProgress2_0.color = Palette.COLOR_P;
					PlayerUI.messageProgress2_1.state = data;
					PlayerUI.messageProgress2_1.color = Palette.COLOR_R;
				}
			}
		}

		public static void message(EPlayerMessage message, string text)
		{
			if (!OptionsSettings.hints && message != EPlayerMessage.EXPERIENCE && message != EPlayerMessage.MOON_ON && message != EPlayerMessage.MOON_OFF && message != EPlayerMessage.SAFEZONE_ON && message != EPlayerMessage.SAFEZONE_OFF && message != EPlayerMessage.WAVE_ON && message != EPlayerMessage.MOON_OFF && message != EPlayerMessage.DEADZONE_ON && message != EPlayerMessage.DEADZONE_OFF && message != EPlayerMessage.REPUTATION)
			{
				return;
			}
			if (message == EPlayerMessage.NONE)
			{
				PlayerUI.messageBox2.isVisible = false;
				PlayerUI.lastMessage = -999f;
				PlayerUI.isMessaged = false;
			}
			else
			{
				if ((message == EPlayerMessage.EXPERIENCE || message == EPlayerMessage.REPUTATION) && (PlayerNPCDialogueUI.active || PlayerNPCQuestUI.active || PlayerNPCVendorUI.active))
				{
					return;
				}
				PlayerUI.messageBox2.sizeOffset_Y = 50;
				PlayerUI.messageBox2.positionOffset_Y = -80 - PlayerUI.messageBox2.sizeOffset_Y;
				PlayerUI.messageBox2.isVisible = true;
				PlayerUI.messageIcon2.isVisible = false;
				PlayerUI.messageProgress2_0.isVisible = false;
				PlayerUI.messageProgress2_1.isVisible = false;
				PlayerUI.lastMessage = Time.realtimeSinceStartup;
				PlayerUI.isMessaged = true;
				if (message == EPlayerMessage.SPACE)
				{
					PlayerUI.messageLabel2.text = PlayerLifeUI.localization.format("Space");
				}
				if (message == EPlayerMessage.RELOAD)
				{
					PlayerUI.messageLabel2.text = PlayerLifeUI.localization.format("Reload", new object[]
					{
						MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.reload)
					});
				}
				else if (message == EPlayerMessage.SAFETY)
				{
					PlayerUI.messageLabel2.text = PlayerLifeUI.localization.format("Safety", new object[]
					{
						MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.firemode)
					});
				}
				else if (message == EPlayerMessage.VEHICLE_EXIT)
				{
					PlayerUI.messageLabel2.text = PlayerLifeUI.localization.format("Vehicle_Exit", new object[]
					{
						MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.interact)
					});
				}
				else if (message == EPlayerMessage.VEHICLE_SWAP)
				{
					PlayerUI.messageLabel2.text = PlayerLifeUI.localization.format("Vehicle_Swap", new object[]
					{
						Player.player.movement.getVehicle().passengers.Length
					});
				}
				else if (message == EPlayerMessage.LIGHT)
				{
					PlayerUI.messageLabel2.text = PlayerLifeUI.localization.format("Light", new object[]
					{
						MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.tactical)
					});
				}
				else if (message == EPlayerMessage.LASER)
				{
					PlayerUI.messageLabel2.text = PlayerLifeUI.localization.format("Laser", new object[]
					{
						MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.tactical)
					});
				}
				else if (message == EPlayerMessage.LASER)
				{
					PlayerUI.messageLabel2.text = PlayerLifeUI.localization.format("Laser", new object[]
					{
						MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.tactical)
					});
				}
				else if (message == EPlayerMessage.RANGEFINDER)
				{
					PlayerUI.messageLabel2.text = PlayerLifeUI.localization.format("Rangefinder", new object[]
					{
						MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.tactical)
					});
				}
				else if (message == EPlayerMessage.EXPERIENCE)
				{
					PlayerUI.messageLabel2.text = PlayerLifeUI.localization.format("Experience", new object[]
					{
						text
					});
				}
				else if (message == EPlayerMessage.EMPTY)
				{
					PlayerUI.messageLabel2.text = PlayerLifeUI.localization.format("Empty");
				}
				else if (message == EPlayerMessage.FULL)
				{
					PlayerUI.messageLabel2.text = PlayerLifeUI.localization.format("Full");
				}
				else if (message == EPlayerMessage.MOON_ON)
				{
					PlayerUI.messageLabel2.text = PlayerLifeUI.localization.format("Moon_On");
				}
				else if (message == EPlayerMessage.MOON_OFF)
				{
					PlayerUI.messageLabel2.text = PlayerLifeUI.localization.format("Moon_Off");
				}
				else if (message == EPlayerMessage.SAFEZONE_ON)
				{
					PlayerUI.messageLabel2.text = PlayerLifeUI.localization.format("Safezone_On");
				}
				else if (message == EPlayerMessage.SAFEZONE_OFF)
				{
					PlayerUI.messageLabel2.text = PlayerLifeUI.localization.format("Safezone_Off");
				}
				else if (message == EPlayerMessage.WAVE_ON)
				{
					PlayerUI.messageLabel2.text = PlayerLifeUI.localization.format("Wave_On");
				}
				else if (message == EPlayerMessage.WAVE_OFF)
				{
					PlayerUI.messageLabel2.text = PlayerLifeUI.localization.format("Wave_Off");
				}
				else if (message == EPlayerMessage.DEADZONE_ON)
				{
					PlayerUI.messageLabel2.text = PlayerLifeUI.localization.format("Deadzone_On");
				}
				else if (message == EPlayerMessage.DEADZONE_OFF)
				{
					PlayerUI.messageLabel2.text = PlayerLifeUI.localization.format("Deadzone_Off");
				}
				else if (message == EPlayerMessage.BUSY)
				{
					PlayerUI.messageLabel2.text = PlayerLifeUI.localization.format("Busy");
				}
				else if (message == EPlayerMessage.FUEL)
				{
					PlayerUI.messageLabel2.text = PlayerLifeUI.localization.format("Fuel", new object[]
					{
						text
					});
				}
				else if (message == EPlayerMessage.CLEAN)
				{
					PlayerUI.messageLabel2.text = PlayerLifeUI.localization.format("Clean");
				}
				else if (message == EPlayerMessage.SALTY)
				{
					PlayerUI.messageLabel2.text = PlayerLifeUI.localization.format("Salty");
				}
				else if (message == EPlayerMessage.DIRTY)
				{
					PlayerUI.messageLabel2.text = PlayerLifeUI.localization.format("Dirty");
				}
				else if (message == EPlayerMessage.REPUTATION)
				{
					PlayerUI.messageLabel2.text = PlayerLifeUI.localization.format("Reputation", new object[]
					{
						text
					});
				}
				else if (message == EPlayerMessage.BAYONET)
				{
					PlayerUI.messageLabel2.text = PlayerLifeUI.localization.format("Bayonet", new object[]
					{
						MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.tactical)
					});
				}
			}
		}

		private void onVisionUpdated(bool isViewing)
		{
			if (isViewing && (double)Random.value < 0.5)
			{
				float value = Random.value;
				if ((double)value < 0.25)
				{
					this.zone.reverbPreset = 24;
				}
				else if ((double)value < 0.5)
				{
					this.zone.reverbPreset = 26;
				}
				else if ((double)value < 0.75)
				{
					this.zone.reverbPreset = 10;
				}
				else
				{
					this.zone.reverbPreset = 22;
				}
				this.zone.enabled = true;
			}
			else
			{
				this.zone.enabled = false;
			}
			if (isViewing && (double)Random.value < 0.5)
			{
				this.twirlScale = Random.Range(2f, 8f);
				this.twirlSize = Random.Range(2f, 32f);
				this.twirlSpeed = Random.Range(0.25f, 1f);
				this.twirl.enabled = true;
			}
			else
			{
				this.twirl.enabled = false;
				this.twirl.angle = 0f;
			}
			if (isViewing && (double)Random.value < 0.5)
			{
				this.vignetteScale = Random.Range(2f, 8f);
				this.vignetteSize = Random.Range(0f, 16f);
				this.vignetteSpeed = Random.Range(0.25f, 1f);
				this.blurScale = Random.Range(2f, 8f);
				this.blurSize = Random.Range(0f, 64f);
				this.blurSpeed = Random.Range(0.25f, 1f);
				this.spreadScale = Random.Range(2f, 8f);
				this.spreadSize = Random.Range(0f, 2f);
				this.spreadSpeed = Random.Range(0.25f, 1f);
				this.chromaScale = Random.Range(2f, 8f);
				this.chromaSize = Random.Range(0f, 64f);
				this.chromaSpeed = Random.Range(0.25f, 1f);
				this.vignetting.enabled = true;
			}
			else
			{
				this.vignetting.enabled = false;
				this.vignetting.intensity = 0f;
				this.vignetting.blur = 0f;
				this.vignetting.blurSpread = 0f;
				this.vignetting.chromaticAberration = 0f;
			}
			if (isViewing && Random.value < 0.5f)
			{
				this.colors.saturation = Random.Range(1f, 2f);
				float value2 = Random.value;
				if ((double)value2 < 0.25)
				{
					this.colors.redChannel = AnimationCurve.Linear(0f, Random.Range(0f, 1f), 1f, Random.Range(0f, 1f));
					this.colors.greenChannel = AnimationCurve.Linear(0f, 0f, 1f, 1f);
					this.colors.blueChannel = AnimationCurve.Linear(0f, 0f, 1f, 1f);
				}
				else if ((double)value2 < 0.5)
				{
					this.colors.redChannel = AnimationCurve.Linear(0f, 0f, 1f, 1f);
					this.colors.greenChannel = AnimationCurve.Linear(0f, Random.Range(0f, 1f), 1f, Random.Range(0f, 1f));
					this.colors.blueChannel = AnimationCurve.Linear(0f, 0f, 1f, 1f);
				}
				else if ((double)value2 < 0.75)
				{
					this.colors.redChannel = AnimationCurve.Linear(0f, 0f, 1f, 1f);
					this.colors.greenChannel = AnimationCurve.Linear(0f, 0f, 1f, 1f);
					this.colors.blueChannel = AnimationCurve.Linear(0f, Random.Range(0f, 1f), 1f, Random.Range(0f, 1f));
				}
				else
				{
					this.colors.redChannel = AnimationCurve.Linear(0f, Random.Range(0f, 1f), 1f, Random.Range(0f, 1f));
					this.colors.greenChannel = AnimationCurve.Linear(0f, Random.Range(0f, 1f), 1f, Random.Range(0f, 1f));
					this.colors.blueChannel = AnimationCurve.Linear(0f, Random.Range(0f, 1f), 1f, Random.Range(0f, 1f));
				}
				this.colors.UpdateParameters();
				this.colors.enabled = true;
			}
			else
			{
				this.colors.enabled = false;
			}
			if (isViewing && (double)Random.value < 0.5)
			{
				this.fishScale = Random.Range(2f, 8f);
				this.fishSize_X = Random.Range(0.1f, 0.6f);
				this.fishSize_Y = Random.Range(0.1f, 0.6f);
				this.fishSpeed = Random.Range(0.25f, 1f);
				this.fish.enabled = true;
			}
			else
			{
				this.fish.enabled = false;
				this.fish.strengthX = 0f;
				this.fish.strengthY = 0f;
			}
			if (isViewing && (double)Random.value < 0.5)
			{
				this.motionScale = Random.Range(2f, 8f);
				this.motionSize = Random.Range(0.1f, 0.92f);
				this.motionSpeed = Random.Range(0.25f, 1f);
				this.motion.enabled = true;
			}
			else
			{
				this.motion.enabled = false;
				this.motion.blurAmount = 0f;
			}
			if (isViewing && (double)Random.value < 0.5)
			{
				this.contrastScale = Random.Range(2f, 8f);
				this.contrastSize = Random.Range(-3f, 3f);
				this.contrastSpeed = Random.Range(0.25f, 1f);
				this.contrast.enabled = true;
			}
			else
			{
				this.contrast.enabled = false;
				this.contrast.intensity = 0f;
			}
		}

		private void onLifeUpdated(bool isDead)
		{
			PlayerUI.isLocked = false;
			if (isDead)
			{
				PlayerLifeUI.close();
				PlayerDashboardUI.close();
				PlayerBarricadeSignUI.close();
				PlayerBarricadeStereoUI.close();
				PlayerBarricadeLibraryUI.close();
				PlayerBarricadeMannequinUI.close();
				PlayerBrowserRequestUI.close();
				PlayerNPCDialogueUI.close();
				PlayerNPCQuestUI.close();
				PlayerNPCVendorUI.close();
				PlayerWorkzoneUI.close();
				PlayerDeathUI.open();
			}
			else
			{
				PlayerDeathUI.close();
			}
		}

		private void onGlassesUpdated(ushort newGlasses, byte newGlassesQuality, byte[] newGlassesState)
		{
			PlayerUI.isBlindfolded = (Player.player.clothing.glassesAsset != null && Player.player.clothing.glassesAsset.isBlindfold);
		}

		private void onMoonUpdated(bool isFullMoon)
		{
			if (isFullMoon)
			{
				PlayerUI.message(EPlayerMessage.MOON_ON, string.Empty);
			}
			else
			{
				PlayerUI.message(EPlayerMessage.MOON_OFF, string.Empty);
			}
		}

		private void OnGUI()
		{
			if (PlayerUI.window == null)
			{
				return;
			}
			if (PlayerUI.isBlindfolded)
			{
				SleekRender.drawImageTexture(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height), PlayerUI.stunTexture, Color.black);
			}
			if (Event.current.isKey && Event.current.type == 5)
			{
				if (Event.current.keyCode == 27)
				{
					if (PlayerLifeUI.chatting)
					{
						PlayerLifeUI.closeChat();
					}
				}
				else if (Event.current.keyCode == 13)
				{
					if (PlayerLifeUI.chatting)
					{
						if (PlayerLifeUI.chatField.text != string.Empty)
						{
							ChatManager.sendChat(PlayerUI.chat, PlayerLifeUI.chatField.text);
						}
						PlayerLifeUI.closeChat();
					}
					else if (PlayerLifeUI.active && !PlayerUI.window.showCursor)
					{
						PlayerLifeUI.openChat();
					}
				}
				else if (Event.current.keyCode == ControlsSettings.global)
				{
					if (PlayerLifeUI.active && !PlayerUI.window.showCursor && !PlayerLifeUI.chatting)
					{
						PlayerUI._chat = EChatMode.GLOBAL;
						PlayerLifeUI.openChat();
					}
				}
				else if (Event.current.keyCode == ControlsSettings.local)
				{
					if (PlayerLifeUI.active && !PlayerUI.window.showCursor && !PlayerLifeUI.chatting)
					{
						PlayerUI._chat = EChatMode.LOCAL;
						PlayerLifeUI.openChat();
					}
				}
				else if (Event.current.keyCode == ControlsSettings.group && PlayerLifeUI.active && !PlayerUI.window.showCursor && !PlayerLifeUI.chatting)
				{
					PlayerUI._chat = EChatMode.GROUP;
					PlayerLifeUI.openChat();
				}
			}
			if (PlayerLifeUI.chatting)
			{
				GUI.SetNextControlName("Chat");
			}
			PlayerUI.window.draw(false);
			SleekRender.drawImageTexture(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height), PlayerUI.stunTexture, PlayerUI.stunColor);
			if (PlayerLifeUI.chatting && GUI.GetNameOfFocusedControl() != "Chat")
			{
				GUI.FocusControl("Chat");
			}
			PlayerDashboardInventoryUI.update();
			MenuConfigurationControlsUI.bindOnGUI();
		}

		private void Update()
		{
			if (PlayerUI.window == null)
			{
				return;
			}
			MenuConfigurationControlsUI.bindUpdate();
			if (Player.player != null && MainCamera.instance != null && PlayerGroupUI.groups != null && PlayerGroupUI.groups.Count == Provider.clients.Count)
			{
				for (int i = 0; i < PlayerGroupUI.groups.Count; i++)
				{
					SleekLabel sleekLabel = PlayerGroupUI.groups[i];
					SteamPlayer steamPlayer = Provider.clients[i];
					if (sleekLabel != null && steamPlayer != null)
					{
						if (Provider.modeConfigData.Gameplay.Group_HUD && steamPlayer.playerID.steamID != Provider.client && steamPlayer.player.quests.isMemberOfSameGroupAs(Player.player) && steamPlayer.model != null)
						{
							Vector3 vector = MainCamera.instance.WorldToScreenPoint(steamPlayer.model.position + Vector3.up * 3f);
							if (vector.z > 0f && (steamPlayer.model.position - Player.player.transform.position).sqrMagnitude < 262144f)
							{
								sleekLabel.positionOffset_X = (int)(vector.x - 100f);
								sleekLabel.positionOffset_Y = (int)((float)Screen.height - vector.y - 15f);
								sleekLabel.isVisible = true;
							}
							else
							{
								sleekLabel.isVisible = false;
							}
						}
						else
						{
							sleekLabel.isVisible = false;
						}
					}
				}
			}
			if (PlayerLifeUI.painImage != null)
			{
				Color backgroundColor = PlayerLifeUI.painImage.backgroundColor;
				backgroundColor.a = Mathf.Lerp(PlayerLifeUI.painImage.backgroundColor.a, 0f, 2f * Time.deltaTime);
				PlayerLifeUI.painImage.backgroundColor = backgroundColor;
			}
			PlayerUI.stunColor.a = Mathf.Max(0f, PlayerUI.stunColor.a - Time.deltaTime);
			if (PlayerLifeUI.hitmarkers != null && MainCamera.instance != null)
			{
				for (int j = 0; j < PlayerLifeUI.hitmarkers.Length; j++)
				{
					HitmarkerInfo hitmarkerInfo = PlayerLifeUI.hitmarkers[j];
					if (hitmarkerInfo != null)
					{
						if (hitmarkerInfo.hit != EPlayerHit.NONE)
						{
							bool flag = Time.realtimeSinceStartup - hitmarkerInfo.lastHit < PlayerUI.HIT_TIME;
							Vector3 vector2;
							if (hitmarkerInfo.worldspace)
							{
								vector2 = MainCamera.instance.WorldToViewportPoint(hitmarkerInfo.point);
								vector2.y = 1f - vector2.y;
							}
							else
							{
								vector2..ctor(0.5f, 0.5f, 1f);
							}
							hitmarkerInfo.hitEntitiyImage.isVisible = (hitmarkerInfo.hit == EPlayerHit.ENTITIY && flag && vector2.z > 0f);
							hitmarkerInfo.hitCriticalImage.isVisible = (hitmarkerInfo.hit == EPlayerHit.CRITICAL && flag && vector2.z > 0f);
							hitmarkerInfo.hitBuildImage.isVisible = (hitmarkerInfo.hit == EPlayerHit.BUILD && flag && vector2.z > 0f);
							hitmarkerInfo.hitEntitiyImage.positionScale_X = vector2.x;
							hitmarkerInfo.hitEntitiyImage.positionScale_Y = vector2.y;
							hitmarkerInfo.hitCriticalImage.positionScale_X = vector2.x;
							hitmarkerInfo.hitCriticalImage.positionScale_Y = vector2.y;
							hitmarkerInfo.hitBuildImage.positionScale_X = vector2.x;
							hitmarkerInfo.hitBuildImage.positionScale_Y = vector2.y;
							if (!flag)
							{
								hitmarkerInfo.hit = EPlayerHit.NONE;
							}
						}
					}
				}
			}
			if (PlayerUI.isHinted)
			{
				if (!PlayerUI.lastHinted)
				{
					PlayerUI.isHinted = false;
					if (PlayerUI.messageBox != null)
					{
						PlayerUI.messageBox.isVisible = false;
					}
					if (PlayerUI.messagePlayer != null)
					{
						PlayerUI.messagePlayer.isVisible = false;
					}
				}
				PlayerUI.lastHinted = false;
			}
			if (PlayerUI.isMessaged)
			{
				if (Time.realtimeSinceStartup - PlayerUI.lastMessage > PlayerUI.MESSAGE_TIME)
				{
					PlayerUI.isMessaged = false;
					if (!PlayerUI.isHinted2 && PlayerUI.messageBox2 != null)
					{
						PlayerUI.messageBox2.isVisible = false;
					}
				}
			}
			else if (PlayerUI.isHinted2)
			{
				if (!PlayerUI.lastHinted2)
				{
					PlayerUI.isHinted2 = false;
					if (PlayerUI.messageBox2 != null)
					{
						PlayerUI.messageBox2.isVisible = false;
					}
				}
				PlayerUI.lastHinted2 = false;
			}
			if (PlayerLifeUI.isVoteMessaged && Time.realtimeSinceStartup - PlayerLifeUI.lastVoteMessage > 2f)
			{
				PlayerLifeUI.isVoteMessaged = false;
				if (PlayerLifeUI.voteBox != null)
				{
					PlayerLifeUI.voteBox.isVisible = false;
				}
			}
			if (Provider.isServer && (MenuConfigurationOptionsUI.active || MenuConfigurationDisplayUI.active || MenuConfigurationGraphicsUI.active || MenuConfigurationControlsUI.active || PlayerPauseUI.active))
			{
				Time.timeScale = 0f;
			}
			else
			{
				Time.timeScale = 1f;
			}
			if (MenuConfigurationControlsUI.binding == 255)
			{
				if ((Input.GetKeyDown(ControlsSettings.left) || Input.GetKeyDown(ControlsSettings.up) || Input.GetKeyDown(ControlsSettings.right) || Input.GetKeyDown(ControlsSettings.down)) && PlayerDashboardUI.active)
				{
					PlayerDashboardUI.close();
					if (!Player.player.life.isDead)
					{
						PlayerLifeUI.open();
					}
				}
				if (Input.GetKeyDown(27))
				{
					if (PlayerDashboardUI.active)
					{
						PlayerDashboardUI.close();
						if (!Player.player.life.isDead)
						{
							PlayerLifeUI.open();
						}
					}
					else if (!Player.player.life.isDead)
					{
						if (PlayerBarricadeSignUI.active)
						{
							PlayerBarricadeSignUI.close();
							PlayerLifeUI.open();
						}
						else if (PlayerBarricadeStereoUI.active)
						{
							PlayerBarricadeStereoUI.close();
							PlayerLifeUI.open();
						}
						else if (PlayerBarricadeLibraryUI.active)
						{
							PlayerBarricadeLibraryUI.close();
							PlayerLifeUI.open();
						}
						else if (PlayerBarricadeMannequinUI.active)
						{
							PlayerBarricadeMannequinUI.close();
							PlayerLifeUI.open();
						}
						else if (PlayerBrowserRequestUI.active)
						{
							PlayerBrowserRequestUI.close();
							PlayerLifeUI.open();
						}
						else if (PlayerNPCDialogueUI.active)
						{
							PlayerNPCDialogueUI.close();
							PlayerLifeUI.open();
						}
						else if (PlayerNPCQuestUI.active)
						{
							PlayerNPCQuestUI.closeNicely();
						}
						else if (PlayerNPCVendorUI.active)
						{
							PlayerNPCVendorUI.closeNicely();
						}
						else if (PlayerWorkzoneUI.active)
						{
							PlayerWorkzoneUI.close();
							PlayerLifeUI.open();
						}
						else if (MenuConfigurationOptionsUI.active)
						{
							MenuConfigurationOptionsUI.close();
							PlayerPauseUI.open();
						}
						else if (MenuConfigurationDisplayUI.active)
						{
							MenuConfigurationDisplayUI.close();
							PlayerPauseUI.open();
						}
						else if (MenuConfigurationGraphicsUI.active)
						{
							MenuConfigurationGraphicsUI.close();
							PlayerPauseUI.open();
						}
						else if (MenuConfigurationControlsUI.active)
						{
							MenuConfigurationControlsUI.close();
							PlayerPauseUI.open();
						}
						else if (PlayerPauseUI.active)
						{
							PlayerPauseUI.close();
							if (!Player.player.life.isDead)
							{
								PlayerLifeUI.open();
							}
						}
						else if (PlayerLifeUI.chatting)
						{
							PlayerLifeUI.closeChat();
						}
						else
						{
							PlayerLifeUI.close();
							PlayerDashboardUI.close();
							PlayerPauseUI.open();
						}
					}
				}
				if (PlayerDeathUI.active)
				{
					if (PlayerDeathUI.homeButton != null)
					{
						if (!Provider.isServer && Provider.isPvP)
						{
							if (Time.realtimeSinceStartup - Player.player.life.lastDeath < Provider.modeConfigData.Gameplay.Timer_Home)
							{
								PlayerDeathUI.homeButton.text = PlayerDeathUI.localization.format("Home_Button_Timer", new object[]
								{
									Mathf.Ceil(Provider.modeConfigData.Gameplay.Timer_Home - (Time.realtimeSinceStartup - Player.player.life.lastDeath))
								});
							}
							else
							{
								PlayerDeathUI.homeButton.text = PlayerDeathUI.localization.format("Home_Button");
							}
						}
						else if (Time.realtimeSinceStartup - Player.player.life.lastRespawn < Provider.modeConfigData.Gameplay.Timer_Respawn)
						{
							PlayerDeathUI.homeButton.text = PlayerDeathUI.localization.format("Home_Button_Timer", new object[]
							{
								Mathf.Ceil(Provider.modeConfigData.Gameplay.Timer_Respawn - (Time.realtimeSinceStartup - Player.player.life.lastRespawn))
							});
						}
						else
						{
							PlayerDeathUI.homeButton.text = PlayerDeathUI.localization.format("Home_Button");
						}
					}
					if (PlayerDeathUI.respawnButton != null)
					{
						if (Time.realtimeSinceStartup - Player.player.life.lastRespawn < Provider.modeConfigData.Gameplay.Timer_Respawn)
						{
							PlayerDeathUI.respawnButton.text = PlayerDeathUI.localization.format("Respawn_Button_Timer", new object[]
							{
								Mathf.Ceil(Provider.modeConfigData.Gameplay.Timer_Respawn - (Time.realtimeSinceStartup - Player.player.life.lastRespawn))
							});
						}
						else
						{
							PlayerDeathUI.respawnButton.text = PlayerDeathUI.localization.format("Respawn_Button");
						}
					}
				}
				if (PlayerPauseUI.active && PlayerPauseUI.exitButton != null)
				{
					if (!Provider.isServer && Provider.isPvP && Provider.clients.Count > 1 && (!Player.player.movement.isSafe || !Player.player.movement.isSafeInfo.noWeapons) && Time.realtimeSinceStartup - PlayerPauseUI.lastLeave < Provider.modeConfigData.Gameplay.Timer_Exit)
					{
						PlayerPauseUI.exitButton.text = PlayerPauseUI.localization.format("Exit_Button_Timer", new object[]
						{
							Mathf.Ceil(Provider.modeConfigData.Gameplay.Timer_Exit - (Time.realtimeSinceStartup - PlayerPauseUI.lastLeave))
						});
					}
					else
					{
						PlayerPauseUI.exitButton.text = PlayerPauseUI.localization.format("Exit_Button_Text");
					}
				}
				if (PlayerNPCDialogueUI.active)
				{
					PlayerNPCDialogueUI.updateText();
				}
				if (!Player.player.life.isDead)
				{
					if (Input.GetKeyDown(ControlsSettings.dashboard))
					{
						if (PlayerDashboardUI.active)
						{
							PlayerDashboardUI.close();
							PlayerLifeUI.open();
						}
						else if (PlayerBarricadeSignUI.active)
						{
							PlayerBarricadeSignUI.close();
							PlayerLifeUI.open();
						}
						else if (PlayerBarricadeStereoUI.active)
						{
							PlayerBarricadeStereoUI.close();
							PlayerLifeUI.open();
						}
						else if (PlayerBarricadeLibraryUI.active)
						{
							PlayerBarricadeLibraryUI.close();
							PlayerLifeUI.open();
						}
						else if (PlayerBarricadeMannequinUI.active)
						{
							PlayerBarricadeMannequinUI.close();
							PlayerLifeUI.open();
						}
						else if (PlayerNPCDialogueUI.active)
						{
							PlayerNPCDialogueUI.close();
							PlayerLifeUI.open();
						}
						else if (PlayerNPCQuestUI.active)
						{
							PlayerNPCQuestUI.closeNicely();
						}
						else if (PlayerNPCVendorUI.active)
						{
							PlayerNPCVendorUI.closeNicely();
						}
						else if (!PlayerUI.window.showCursor && !PlayerLifeUI.chatting)
						{
							PlayerLifeUI.close();
							PlayerPauseUI.close();
							PlayerDashboardUI.open();
						}
					}
					if (Input.GetKeyDown(ControlsSettings.inventory))
					{
						if (PlayerDashboardUI.active && PlayerDashboardInventoryUI.active)
						{
							PlayerDashboardUI.close();
							PlayerLifeUI.open();
						}
						else if (PlayerDashboardUI.active)
						{
							PlayerDashboardCraftingUI.close();
							PlayerDashboardSkillsUI.close();
							PlayerDashboardInformationUI.close();
							PlayerDashboardInventoryUI.open();
						}
						else if (!PlayerUI.window.showCursor && !PlayerLifeUI.chatting)
						{
							PlayerLifeUI.close();
							PlayerPauseUI.close();
							PlayerDashboardInventoryUI.active = true;
							PlayerDashboardCraftingUI.active = false;
							PlayerDashboardSkillsUI.active = false;
							PlayerDashboardInformationUI.active = false;
							PlayerDashboardUI.open();
						}
					}
					if (Input.GetKeyDown(ControlsSettings.crafting) && Level.info != null && Level.info.type != ELevelType.HORDE)
					{
						if (PlayerDashboardUI.active && PlayerDashboardCraftingUI.active)
						{
							PlayerDashboardUI.close();
							PlayerLifeUI.open();
						}
						else if (PlayerDashboardUI.active)
						{
							PlayerDashboardInventoryUI.close();
							PlayerDashboardSkillsUI.close();
							PlayerDashboardInformationUI.close();
							PlayerDashboardCraftingUI.open();
						}
						else if (!PlayerUI.window.showCursor && !PlayerLifeUI.chatting)
						{
							PlayerLifeUI.close();
							PlayerPauseUI.close();
							PlayerDashboardInventoryUI.active = false;
							PlayerDashboardCraftingUI.active = true;
							PlayerDashboardSkillsUI.active = false;
							PlayerDashboardInformationUI.active = false;
							PlayerDashboardUI.open();
						}
					}
					if (Input.GetKeyDown(ControlsSettings.skills) && Level.info != null && Level.info.type != ELevelType.HORDE)
					{
						if (PlayerDashboardUI.active && PlayerDashboardSkillsUI.active)
						{
							PlayerDashboardUI.close();
							PlayerLifeUI.open();
						}
						else if (PlayerDashboardUI.active)
						{
							PlayerDashboardInventoryUI.close();
							PlayerDashboardCraftingUI.close();
							PlayerDashboardInformationUI.close();
							PlayerDashboardSkillsUI.open();
						}
						else if (!PlayerUI.window.showCursor && !PlayerLifeUI.chatting)
						{
							PlayerLifeUI.close();
							PlayerPauseUI.close();
							PlayerDashboardInventoryUI.active = false;
							PlayerDashboardCraftingUI.active = false;
							PlayerDashboardSkillsUI.active = true;
							PlayerDashboardInformationUI.active = false;
							PlayerDashboardUI.open();
						}
					}
					if (Input.GetKeyDown(ControlsSettings.map) || Input.GetKeyDown(ControlsSettings.quests) || Input.GetKeyDown(ControlsSettings.players))
					{
						if (PlayerDashboardUI.active && PlayerDashboardInformationUI.active)
						{
							PlayerDashboardUI.close();
							PlayerLifeUI.open();
						}
						else
						{
							if (Input.GetKeyDown(ControlsSettings.quests))
							{
								PlayerDashboardInformationUI.openQuests();
							}
							else if (Input.GetKeyDown(ControlsSettings.players))
							{
								PlayerDashboardInformationUI.openPlayers();
							}
							if (PlayerDashboardUI.active)
							{
								PlayerDashboardInventoryUI.close();
								PlayerDashboardCraftingUI.close();
								PlayerDashboardSkillsUI.close();
								PlayerDashboardInformationUI.open();
							}
							else if (!PlayerUI.window.showCursor && !PlayerLifeUI.chatting)
							{
								PlayerLifeUI.close();
								PlayerPauseUI.close();
								PlayerDashboardInventoryUI.active = false;
								PlayerDashboardCraftingUI.active = false;
								PlayerDashboardSkillsUI.active = false;
								PlayerDashboardInformationUI.active = true;
								PlayerDashboardUI.open();
							}
						}
					}
					if (Input.GetKeyDown(ControlsSettings.gesture))
					{
						if (PlayerLifeUI.active && !PlayerUI.window.showCursor && !PlayerLifeUI.chatting)
						{
							PlayerLifeUI.openGestures();
						}
					}
					else if (Input.GetKeyUp(ControlsSettings.gesture) && PlayerLifeUI.active)
					{
						PlayerLifeUI.closeGestures();
					}
				}
				if (PlayerUI.window != null)
				{
					if (Input.GetKeyDown(ControlsSettings.screenshot))
					{
						Provider.takeScreenshot();
					}
					if (Input.GetKeyDown(ControlsSettings.hud))
					{
						DevkitWindowManager.isActive = false;
						PlayerUI.window.isEnabled = !PlayerUI.window.isEnabled;
						PlayerUI.window.drawCursorWhileDisabled = false;
					}
					if (Input.GetKeyDown(ControlsSettings.terminal))
					{
						DevkitWindowManager.isActive = !DevkitWindowManager.isActive;
						PlayerUI.window.isEnabled = !DevkitWindowManager.isActive;
						PlayerUI.window.drawCursorWhileDisabled = DevkitWindowManager.isActive;
					}
				}
				if (Input.GetKeyDown(ControlsSettings.refreshAssets) && Provider.isServer)
				{
					Assets.refresh();
				}
				if (Input.GetKeyDown(ControlsSettings.clipboardDebug))
				{
					string text = string.Empty;
					for (int k = 0; k < Player.player.quests.flagsList.Count; k++)
					{
						if (k > 0)
						{
							text += "\n";
						}
						text += string.Format("{0, 5} {1, 5}", Player.player.quests.flagsList[k].id, Player.player.quests.flagsList[k].value);
					}
					GUIUtility.systemCopyBuffer = text;
				}
			}
			PlayerUI.window.showCursor = (PlayerPauseUI.active || MenuConfigurationOptionsUI.active || MenuConfigurationDisplayUI.active || MenuConfigurationGraphicsUI.active || MenuConfigurationControlsUI.active || PlayerDashboardUI.active || PlayerDeathUI.active || PlayerLifeUI.chatting || PlayerLifeUI.gesturing || PlayerBarricadeSignUI.active || PlayerBarricadeStereoUI.active || PlayerBarricadeLibraryUI.active || PlayerBarricadeMannequinUI.active || PlayerBrowserRequestUI.active || PlayerNPCDialogueUI.active || PlayerNPCQuestUI.active || PlayerNPCVendorUI.active || (PlayerWorkzoneUI.active && !Input.GetKey(ControlsSettings.secondary)) || PlayerUI.isLocked);
			if (this.blur != null)
			{
				if ((PlayerUI.window.showCursor && !MenuConfigurationGraphicsUI.active && !PlayerNPCDialogueUI.active && !PlayerNPCQuestUI.active && !PlayerNPCVendorUI.active && !PlayerWorkzoneUI.active) || (WaterUtility.isPointUnderwater(MainCamera.instance.transform.position) && (Player.player.clothing.glassesAsset == null || !Player.player.clothing.glassesAsset.proofWater)) || (Player.player.look.isScopeActive && GraphicsSettings.scopeQuality != EGraphicQuality.OFF && Player.player.look.perspective == EPlayerPerspective.FIRST && Player.player.equipment.useable != null && ((UseableGun)Player.player.equipment.useable).isAiming))
				{
					if (!this.blur.enabled)
					{
						this.blur.enabled = true;
					}
				}
				else if (this.blur.enabled)
				{
					this.blur.enabled = false;
				}
			}
			if (this.twirl != null && this.twirl.enabled)
			{
				this.twirl.angle = Mathf.Lerp(this.twirl.angle, Mathf.Sin(Time.realtimeSinceStartup / this.twirlScale) * this.twirlSize, Time.deltaTime * this.twirlSpeed);
			}
			if (this.vignetting != null && this.vignetting.enabled)
			{
				this.vignetting.intensity = Mathf.Lerp(this.vignetting.intensity, Mathf.Sin(Time.realtimeSinceStartup / this.vignetteScale) * this.vignetteSize, Time.deltaTime * this.vignetteSpeed);
				this.vignetting.blur = Mathf.Lerp(this.vignetting.blur, Mathf.Abs(Mathf.Sin(Time.realtimeSinceStartup / this.blurScale)) * this.blurSize, Time.deltaTime * this.blurSpeed);
				this.vignetting.blurSpread = Mathf.Lerp(this.vignetting.blurSpread, Mathf.Abs(Mathf.Sin(Time.realtimeSinceStartup / this.spreadScale)) * this.spreadSize, Time.deltaTime * this.spreadSpeed);
				this.vignetting.chromaticAberration = Mathf.Lerp(this.vignetting.chromaticAberration, Mathf.Abs(Mathf.Sin(Time.realtimeSinceStartup / this.chromaScale)) * this.chromaSize, Time.deltaTime * this.chromaSpeed);
			}
			if (this.fish != null && this.fish.enabled)
			{
				this.fish.strengthX = Mathf.Lerp(this.fish.strengthX, 0.4f + Mathf.Sin(Time.realtimeSinceStartup / this.fishScale) * this.fishSize_X, Time.deltaTime * this.fishSpeed);
				this.fish.strengthY = Mathf.Lerp(this.fish.strengthY, 0.4f + Mathf.Cos(Time.realtimeSinceStartup / this.fishScale) * this.fishSize_Y, Time.deltaTime * this.fishSpeed);
			}
			if (this.motion != null && this.motion.enabled)
			{
				this.motion.blurAmount = Mathf.Lerp(this.motion.blurAmount, Mathf.Abs(Mathf.Sin(Time.realtimeSinceStartup / this.motionScale)) * this.motionSize, Time.deltaTime * this.motionSpeed);
			}
			if (this.contrast != null && this.contrast.enabled)
			{
				this.contrast.intensity = Mathf.Lerp(this.contrast.intensity, Mathf.Abs(Mathf.Sin(Time.realtimeSinceStartup / this.contrastScale)) * this.contrastSize, Time.deltaTime * this.contrastSpeed);
			}
			PlayerUI.window.updateDebug();
		}

		private void Awake()
		{
			AudioListener component = LoadingUI.loader.GetComponent<AudioListener>();
			if (component != null)
			{
				Object.Destroy(component);
			}
		}

		private void Start()
		{
			PlayerUI.isLocked = false;
			PlayerUI._chat = EChatMode.GLOBAL;
			PlayerUI.window = new SleekWindow();
			PlayerUI.ui = this;
			PlayerUI.container = new Sleek();
			PlayerUI.container.sizeScale_X = 1f;
			PlayerUI.container.sizeScale_Y = 1f;
			PlayerUI.window.add(PlayerUI.container);
			PlayerUI.isOverlayed = false;
			PlayerUI.wasOverlayed = false;
			PlayerUI.isReverting = true;
			OptionsSettings.apply();
			GraphicsSettings.apply();
			new PlayerGroupUI();
			new PlayerDashboardUI();
			new PlayerPauseUI();
			new PlayerLifeUI();
			new PlayerDeathUI();
			new PlayerBarricadeSignUI();
			new PlayerBarricadeStereoUI();
			new PlayerBarricadeLibraryUI();
			new PlayerBarricadeMannequinUI();
			new PlayerBrowserRequestUI();
			new PlayerNPCDialogueUI();
			new PlayerNPCQuestUI();
			new PlayerNPCVendorUI();
			new PlayerWorkzoneUI();
			PlayerUI.messageBox = new SleekBox();
			PlayerUI.messageBox.positionOffset_X = -200;
			PlayerUI.messageBox.positionScale_X = 0.5f;
			PlayerUI.messageBox.positionScale_Y = 1f;
			PlayerUI.messageBox.sizeOffset_X = 400;
			PlayerUI.messageBox.backgroundTint = ESleekTint.NONE;
			PlayerUI.messageBox.foregroundTint = ESleekTint.NONE;
			PlayerUI.container.add(PlayerUI.messageBox);
			PlayerUI.messageBox.isVisible = false;
			PlayerUI.messageLabel = new SleekLabel();
			PlayerUI.messageLabel.positionOffset_X = 5;
			PlayerUI.messageLabel.positionOffset_Y = 5;
			PlayerUI.messageLabel.sizeOffset_X = -10;
			PlayerUI.messageLabel.sizeOffset_Y = 40;
			PlayerUI.messageLabel.sizeScale_X = 1f;
			PlayerUI.messageLabel.fontSize = 14;
			PlayerUI.messageLabel.foregroundTint = ESleekTint.NONE;
			PlayerUI.messageBox.add(PlayerUI.messageLabel);
			PlayerUI.messageIcon_0 = new SleekImageTexture();
			PlayerUI.messageIcon_0.positionOffset_X = 5;
			PlayerUI.messageIcon_0.positionOffset_Y = 45;
			PlayerUI.messageIcon_0.sizeOffset_X = 20;
			PlayerUI.messageIcon_0.sizeOffset_Y = 20;
			PlayerUI.messageBox.add(PlayerUI.messageIcon_0);
			PlayerUI.messageIcon_0.isVisible = false;
			PlayerUI.messageIcon_1 = new SleekImageTexture();
			PlayerUI.messageIcon_1.positionOffset_X = 5;
			PlayerUI.messageIcon_1.positionOffset_Y = 75;
			PlayerUI.messageIcon_1.sizeOffset_X = 20;
			PlayerUI.messageIcon_1.sizeOffset_Y = 20;
			PlayerUI.messageBox.add(PlayerUI.messageIcon_1);
			PlayerUI.messageIcon_1.isVisible = false;
			PlayerUI.messageIcon_2 = new SleekImageTexture();
			PlayerUI.messageIcon_2.positionOffset_X = 5;
			PlayerUI.messageIcon_2.positionOffset_Y = 105;
			PlayerUI.messageIcon_2.sizeOffset_X = 20;
			PlayerUI.messageIcon_2.sizeOffset_Y = 20;
			PlayerUI.messageBox.add(PlayerUI.messageIcon_2);
			PlayerUI.messageIcon_2.isVisible = false;
			PlayerUI.messageProgress_0 = new SleekProgress(string.Empty);
			PlayerUI.messageProgress_0.positionOffset_X = 30;
			PlayerUI.messageProgress_0.positionOffset_Y = 50;
			PlayerUI.messageProgress_0.sizeOffset_X = -40;
			PlayerUI.messageProgress_0.sizeOffset_Y = 10;
			PlayerUI.messageProgress_0.sizeScale_X = 1f;
			PlayerUI.messageBox.add(PlayerUI.messageProgress_0);
			PlayerUI.messageProgress_0.isVisible = false;
			PlayerUI.messageProgress_1 = new SleekProgress(string.Empty);
			PlayerUI.messageProgress_1.positionOffset_X = 30;
			PlayerUI.messageProgress_1.positionOffset_Y = 80;
			PlayerUI.messageProgress_1.sizeOffset_X = -40;
			PlayerUI.messageProgress_1.sizeOffset_Y = 10;
			PlayerUI.messageProgress_1.sizeScale_X = 1f;
			PlayerUI.messageBox.add(PlayerUI.messageProgress_1);
			PlayerUI.messageProgress_1.isVisible = false;
			PlayerUI.messageProgress_2 = new SleekProgress(string.Empty);
			PlayerUI.messageProgress_2.positionOffset_X = 30;
			PlayerUI.messageProgress_2.positionOffset_Y = 110;
			PlayerUI.messageProgress_2.sizeOffset_X = -40;
			PlayerUI.messageProgress_2.sizeOffset_Y = 10;
			PlayerUI.messageProgress_2.sizeScale_X = 1f;
			PlayerUI.messageBox.add(PlayerUI.messageProgress_2);
			PlayerUI.messageProgress_2.isVisible = false;
			PlayerUI.messageQualityImage = new SleekImageTexture((Texture2D)PlayerDashboardInventoryUI.icons.load("Quality_0"));
			PlayerUI.messageQualityImage.positionOffset_X = -30;
			PlayerUI.messageQualityImage.positionOffset_Y = -30;
			PlayerUI.messageQualityImage.positionScale_X = 1f;
			PlayerUI.messageQualityImage.positionScale_Y = 1f;
			PlayerUI.messageQualityImage.sizeOffset_X = 20;
			PlayerUI.messageQualityImage.sizeOffset_Y = 20;
			PlayerUI.messageBox.add(PlayerUI.messageQualityImage);
			PlayerUI.messageQualityImage.isVisible = false;
			PlayerUI.messageAmountLabel = new SleekLabel();
			PlayerUI.messageAmountLabel.positionOffset_X = 10;
			PlayerUI.messageAmountLabel.positionOffset_Y = -40;
			PlayerUI.messageAmountLabel.positionScale_Y = 1f;
			PlayerUI.messageAmountLabel.sizeOffset_X = -20;
			PlayerUI.messageAmountLabel.sizeOffset_Y = 30;
			PlayerUI.messageAmountLabel.sizeScale_X = 1f;
			PlayerUI.messageAmountLabel.fontAlignment = 6;
			PlayerUI.messageAmountLabel.foregroundTint = ESleekTint.NONE;
			PlayerUI.messageBox.add(PlayerUI.messageAmountLabel);
			PlayerUI.messageAmountLabel.isVisible = false;
			PlayerUI.messageBox2 = new SleekBox();
			PlayerUI.messageBox2.positionOffset_X = -200;
			PlayerUI.messageBox2.positionScale_X = 0.5f;
			PlayerUI.messageBox2.positionScale_Y = 1f;
			PlayerUI.messageBox2.sizeOffset_X = 400;
			PlayerUI.messageBox2.backgroundTint = ESleekTint.NONE;
			PlayerUI.messageBox2.foregroundTint = ESleekTint.NONE;
			PlayerUI.container.add(PlayerUI.messageBox2);
			PlayerUI.messageBox2.isVisible = false;
			PlayerUI.messageLabel2 = new SleekLabel();
			PlayerUI.messageLabel2.positionOffset_X = 5;
			PlayerUI.messageLabel2.positionOffset_Y = 5;
			PlayerUI.messageLabel2.sizeOffset_X = -10;
			PlayerUI.messageLabel2.sizeOffset_Y = 40;
			PlayerUI.messageLabel2.sizeScale_X = 1f;
			PlayerUI.messageLabel2.fontSize = 14;
			PlayerUI.messageLabel2.foregroundTint = ESleekTint.NONE;
			PlayerUI.messageBox2.add(PlayerUI.messageLabel2);
			PlayerUI.messageIcon2 = new SleekImageTexture();
			PlayerUI.messageIcon2.positionOffset_X = 5;
			PlayerUI.messageIcon2.positionOffset_Y = 75;
			PlayerUI.messageIcon2.sizeOffset_X = 20;
			PlayerUI.messageIcon2.sizeOffset_Y = 20;
			PlayerUI.messageBox2.add(PlayerUI.messageIcon2);
			PlayerUI.messageIcon2.isVisible = false;
			PlayerUI.messageProgress2_0 = new SleekProgress(string.Empty);
			PlayerUI.messageProgress2_0.positionOffset_X = 5;
			PlayerUI.messageProgress2_0.positionOffset_Y = 50;
			PlayerUI.messageProgress2_0.sizeOffset_X = -10;
			PlayerUI.messageProgress2_0.sizeOffset_Y = 10;
			PlayerUI.messageProgress2_0.sizeScale_X = 1f;
			PlayerUI.messageBox2.add(PlayerUI.messageProgress2_0);
			PlayerUI.messageProgress2_1 = new SleekProgress(string.Empty);
			PlayerUI.messageProgress2_1.positionOffset_X = 30;
			PlayerUI.messageProgress2_1.positionOffset_Y = 80;
			PlayerUI.messageProgress2_1.sizeOffset_X = -40;
			PlayerUI.messageProgress2_1.sizeOffset_Y = 10;
			PlayerUI.messageProgress2_1.sizeScale_X = 1f;
			PlayerUI.messageBox2.add(PlayerUI.messageProgress2_1);
			PlayerUI.stunTexture = (Texture2D)Resources.Load("Materials/Pixel");
			PlayerUI.stunColor = Color.white;
			PlayerUI.stunColor.a = 0f;
			PlayerUI.isBlindfolded = false;
			PlayerLife life = Player.player.life;
			life.onVisionUpdated = (VisionUpdated)Delegate.Combine(life.onVisionUpdated, new VisionUpdated(this.onVisionUpdated));
			PlayerLife life2 = Player.player.life;
			life2.onLifeUpdated = (LifeUpdated)Delegate.Combine(life2.onLifeUpdated, new LifeUpdated(this.onLifeUpdated));
			PlayerClothing clothing = Player.player.clothing;
			clothing.onGlassesUpdated = (GlassesUpdated)Delegate.Combine(clothing.onGlassesUpdated, new GlassesUpdated(this.onGlassesUpdated));
			LightingManager.onMoonUpdated = (MoonUpdated)Delegate.Combine(LightingManager.onMoonUpdated, new MoonUpdated(this.onMoonUpdated));
			this.blur = base.GetComponent<BlurEffect>();
			this.zone = base.GetComponent<AudioReverbZone>();
			this.twirl = Player.player.animator.view.GetComponent<TwirlEffect>();
			this.vignetting = Player.player.animator.view.GetComponent<Vignetting>();
			this.colors = Player.player.animator.view.GetComponent<ColorCorrectionCurves>();
			this.fish = Player.player.animator.view.GetComponent<Fisheye>();
			this.motion = Player.player.animator.view.GetComponent<MotionBlur>();
			this.contrast = Player.player.animator.view.GetComponent<ContrastEnhance>();
		}

		private void OnDestroy()
		{
			if (PlayerUI.window == null)
			{
				return;
			}
			PlayerUI.window.destroy();
		}

		public static readonly float MESSAGE_TIME = 2f;

		public static readonly float HIT_TIME = 0.33f;

		public static SleekWindow window;

		public static Sleek container;

		private static PlayerUI ui;

		private static SleekPlayer messagePlayer;

		private static SleekBox messageBox;

		private static SleekLabel messageLabel;

		private static SleekProgress messageProgress_0;

		private static SleekProgress messageProgress_1;

		private static SleekProgress messageProgress_2;

		private static SleekImageTexture messageIcon_0;

		private static SleekImageTexture messageIcon_1;

		private static SleekImageTexture messageIcon_2;

		private static SleekImageTexture messageQualityImage;

		private static SleekLabel messageAmountLabel;

		private static SleekBox messageBox2;

		private static SleekLabel messageLabel2;

		private static SleekProgress messageProgress2_0;

		private static SleekProgress messageProgress2_1;

		private static SleekImageTexture messageIcon2;

		private static Texture2D stunTexture;

		private static Color stunColor;

		private static bool _isBlindfolded;

		public static bool isLocked;

		private BlurEffect blur;

		private AudioReverbZone zone;

		private TwirlEffect twirl;

		private Vignetting vignetting;

		private ColorCorrectionCurves colors;

		private Fisheye fish;

		private MotionBlur motion;

		private ContrastEnhance contrast;

		private float twirlScale;

		private float twirlSize;

		private float twirlSpeed;

		private float vignetteScale;

		private float vignetteSize;

		private float vignetteSpeed;

		private float blurScale;

		private float blurSize;

		private float blurSpeed;

		private float spreadScale;

		private float spreadSize;

		private float spreadSpeed;

		private float chromaScale;

		private float chromaSize;

		private float chromaSpeed;

		private float fishScale;

		private float fishSize_X;

		private float fishSize_Y;

		private float fishSpeed;

		private float motionScale;

		private float motionSize;

		private float motionSpeed;

		private float contrastScale;

		private float contrastSize;

		private float contrastSpeed;

		private static float lastMessage;

		private static bool isMessaged;

		private static bool lastHinted;

		private static bool isHinted;

		private static bool lastHinted2;

		private static bool isHinted2;

		private static bool isOverlayed;

		private static bool wasOverlayed;

		private static bool isReverting;

		private static EChatMode _chat;
	}
}
