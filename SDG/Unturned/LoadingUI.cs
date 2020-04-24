using System;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace SDG.Unturned
{
	public class LoadingUI : MonoBehaviour
	{
		public static bool isInitialized
		{
			get
			{
				return LoadingUI._isInitialized;
			}
		}

		public static GameObject loader { get; private set; }

		public static bool isBlocked
		{
			get
			{
				return Time.realtimeSinceStartup - LoadingUI.lastLoading < 0.1f;
			}
		}

		public static void updateKey(string key)
		{
			if (!Dedicator.isDedicated)
			{
				if (LoadingUI.loadingLabel == null)
				{
					return;
				}
				LoadingUI.loadingLabel.text = LoadingUI.localization.format(key);
				if (LoadingUI.loadingImage == null)
				{
					return;
				}
				LoadingUI.loadingImage.sizeScale_X = 1f;
				LoadingUI.loadingImage.sizeOffset_X = -20;
			}
			else
			{
				CommandWindow.Log(LoadingUI.localization.format(key));
			}
		}

		public static void updateProgress(float progress)
		{
			if (!Dedicator.isDedicated)
			{
				if (LoadingUI.loadingImage == null)
				{
					return;
				}
				LoadingUI.loadingImage.sizeScale_X = progress;
				LoadingUI.loadingImage.sizeOffset_X = (int)(-20f * progress);
			}
			else
			{
				CommandWindow.Log(LoadingUI.localization.format("Level_Load", new object[]
				{
					(int)(progress * 100f)
				}));
			}
		}

		public static void assetsLoad(string key, int count, float progress, float step)
		{
			LoadingUI.assetsLoadCount = LoadingUI.assetsScanCount - count;
			if (!Dedicator.isDedicated)
			{
				if (LoadingUI.loadingLabel == null)
				{
					return;
				}
				LoadingUI.loadingLabel.text = LoadingUI.localization.format("Assets_Load", new object[]
				{
					LoadingUI.localization.format(key),
					LoadingUI.assetsLoadCount,
					LoadingUI.assetsScanCount
				});
				if (LoadingUI.loadingImage == null)
				{
					return;
				}
				progress += (float)LoadingUI.assetsLoadCount / (float)LoadingUI.assetsScanCount * step;
				LoadingUI.loadingImage.sizeScale_X = progress;
				LoadingUI.loadingImage.sizeOffset_X = (int)(-20f * progress);
			}
			else
			{
				CommandWindow.Log(LoadingUI.localization.format("Assets_Load", new object[]
				{
					LoadingUI.localization.format(key),
					LoadingUI.assetsLoadCount,
					LoadingUI.assetsScanCount
				}));
			}
		}

		public static void assetsScan(string key, int count)
		{
			LoadingUI.assetsScanCount = count;
			if (!Dedicator.isDedicated)
			{
				if (LoadingUI.loadingLabel == null)
				{
					return;
				}
				LoadingUI.loadingLabel.text = LoadingUI.localization.format("Assets_Scan", new object[]
				{
					LoadingUI.localization.format(key),
					LoadingUI.assetsScanCount
				});
			}
			else
			{
				CommandWindow.Log(LoadingUI.localization.format("Assets_Scan", new object[]
				{
					LoadingUI.localization.format(key),
					LoadingUI.assetsScanCount
				}));
			}
		}

		private static bool loadBackgroundImage(string path)
		{
			if (LoadingUI.backgroundImage.texture != null && LoadingUI.backgroundImage.shouldDestroyTexture)
			{
				Object.Destroy(LoadingUI.backgroundImage.texture);
				LoadingUI.backgroundImage.texture = null;
			}
			if (string.IsNullOrEmpty(path))
			{
				return false;
			}
			if (!File.Exists(path))
			{
				return false;
			}
			byte[] array = File.ReadAllBytes(path);
			Texture2D texture2D = new Texture2D(3840, 2160, 5, false, true);
			texture2D.name = "Loading_Texture";
			texture2D.filterMode = 2;
			texture2D.hideFlags = 61;
			texture2D.LoadImage(array);
			LoadingUI.backgroundImage.texture = texture2D;
			LoadingUI.backgroundImage.shouldDestroyTexture = true;
			return true;
		}

		private static bool pickBackgroundImage(string path)
		{
			if (!Directory.Exists(path))
			{
				LoadingUI.loadBackgroundImage(null);
				return false;
			}
			string[] files = Directory.GetFiles(path, "*.png");
			string[] files2 = Directory.GetFiles(path, "*.jpg");
			string[] array = new string[files.Length + files2.Length];
			int num = 0;
			for (int i = 0; i < files.Length; i++)
			{
				array[num] = files[i];
				num++;
			}
			for (int j = 0; j < files2.Length; j++)
			{
				array[num] = files2[j];
				num++;
			}
			if (array.Length == 0)
			{
				LoadingUI.loadBackgroundImage(null);
			}
			else
			{
				int num2 = Random.Range(0, array.Length);
				string path2 = array[num2];
				LoadingUI.loadBackgroundImage(path2);
			}
			return true;
		}

		public static void updateScene()
		{
			if (!Dedicator.isDedicated)
			{
				if (LoadingUI.backgroundImage == null)
				{
					return;
				}
				if (LoadingUI.loadingImage == null)
				{
					return;
				}
				LoadingUI.updateProgress(0f);
				Local local = Localization.read("/Menu/MenuTips.dat");
				byte b;
				do
				{
					b = (byte)Random.Range(1, (int)(LoadingUI.TIP_COUNT + 1));
				}
				while (b == (byte)LoadingUI.tip);
				LoadingUI.tip = (ELoadingTip)b;
				string text;
				if (OptionsSettings.streamer && Provider.streamerNames != null && Provider.streamerNames.Count > 0 && Provider.streamerNames[0] == "Nelson AI")
				{
					text = local.format("Streamer");
				}
				else
				{
					switch (LoadingUI.tip)
					{
					case ELoadingTip.HOTKEY:
						text = local.format("Hotkey");
						break;
					case ELoadingTip.EQUIP:
						text = local.format("Equip", new object[]
						{
							MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.other)
						});
						break;
					case ELoadingTip.DROP:
						text = local.format("Drop", new object[]
						{
							MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.other)
						});
						break;
					case ELoadingTip.SIRENS:
						text = local.format("Sirens", new object[]
						{
							MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.other)
						});
						break;
					case ELoadingTip.TRANSFORM:
						text = local.format("Transform");
						break;
					case ELoadingTip.QUALITY:
						text = local.format("Quality");
						break;
					case ELoadingTip.UMBRELLA:
						text = local.format("Umbrella");
						break;
					case ELoadingTip.HEAL:
						text = local.format("Heal");
						break;
					case ELoadingTip.ROTATE:
						text = local.format("Rotate");
						break;
					case ELoadingTip.BASE:
						text = local.format("Base");
						break;
					case ELoadingTip.DEQUIP:
						text = local.format("Dequip", new object[]
						{
							MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.dequip)
						});
						break;
					case ELoadingTip.NIGHTVISION:
						text = local.format("Nightvision", new object[]
						{
							MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.vision)
						});
						break;
					case ELoadingTip.TRANSFER:
						text = local.format("Transfer", new object[]
						{
							MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.other)
						});
						break;
					case ELoadingTip.SURFACE:
						text = local.format("Surface", new object[]
						{
							MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.jump)
						});
						break;
					case ELoadingTip.ARREST:
						text = local.format("Arrest", new object[]
						{
							MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.leanLeft),
							MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.leanRight)
						});
						break;
					case ELoadingTip.SAFEZONE:
						text = local.format("Safezone");
						break;
					case ELoadingTip.CLAIM:
						text = local.format("Claim");
						break;
					case ELoadingTip.GROUP:
						text = local.format("Group");
						break;
					case ELoadingTip.MAP:
						text = local.format("Map", new object[]
						{
							MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.map)
						});
						break;
					case ELoadingTip.BEACON:
						text = local.format("Beacon");
						break;
					case ELoadingTip.HORN:
						text = local.format("Horn", new object[]
						{
							MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.primary)
						});
						break;
					case ELoadingTip.LIGHTS:
						text = local.format("Lights", new object[]
						{
							MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.secondary)
						});
						break;
					case ELoadingTip.SNAP:
						text = local.format("Snap", new object[]
						{
							MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.snap)
						});
						break;
					case ELoadingTip.UPGRADE:
						text = local.format("Upgrade", new object[]
						{
							MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.other)
						});
						break;
					case ELoadingTip.GRAB:
						text = local.format("Grab", new object[]
						{
							MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.other)
						});
						break;
					case ELoadingTip.SKYCRANE:
						text = local.format("Skycrane", new object[]
						{
							MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.other)
						});
						break;
					case ELoadingTip.SEAT:
						text = local.format("Seat");
						break;
					case ELoadingTip.RARITY:
						text = local.format("Rarity");
						break;
					case ELoadingTip.ORIENTATION:
						text = local.format("Orientation", new object[]
						{
							MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.rotate)
						});
						break;
					case ELoadingTip.RED:
						text = local.format("Red");
						break;
					case ELoadingTip.STEADY:
						text = local.format("Steady", new object[]
						{
							MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.sprint)
						});
						break;
					default:
						text = "#" + LoadingUI.tip.ToString();
						break;
					}
				}
				LoadingUI.tipBox.text = ItemTool.filterRarityRichText(local.format("Tip", new object[]
				{
					text
				}));
				if (Level.info != null)
				{
					if (!LoadingUI.pickBackgroundImage(Level.info.path + "/Screenshots") && !LoadingUI.loadBackgroundImage(Level.info.path + "/Level.png"))
					{
						LoadingUI.pickBackgroundImage(ReadWrite.PATH + "/Screenshots");
					}
					Local local2 = Localization.tryRead(Level.info.path, false);
					if (local2 != null)
					{
						if (Provider.isConnected)
						{
							string text2;
							if (Provider.isServer)
							{
								text2 = LoadingUI.localization.format("Offline");
							}
							else
							{
								if (Provider.currentServerInfo.IsVACSecure)
								{
									text2 = LoadingUI.localization.format("VAC_Secure");
								}
								else
								{
									text2 = LoadingUI.localization.format("VAC_Insecure");
								}
								if (Provider.currentServerInfo.IsBattlEyeSecure)
								{
									text2 = text2 + " + " + LoadingUI.localization.format("BattlEye_Secure");
								}
								else
								{
									text2 = text2 + " + " + LoadingUI.localization.format("BattlEye_Insecure");
								}
							}
							LoadingUI.loadingLabel.text = local2.format("Loading_Server", new object[]
							{
								(!OptionsSettings.streamer) ? Provider.currentServerInfo.name : LoadingUI.localization.format("Streamer"),
								text2
							});
							if (Provider.mode == EGameMode.EASY)
							{
								LoadingUI.loadingImage.backgroundColor = Palette.COLOR_G;
								LoadingUI.loadingImage.foregroundColor = Palette.COLOR_G;
							}
							else if (Provider.mode == EGameMode.HARD)
							{
								LoadingUI.loadingImage.backgroundColor = Palette.COLOR_R;
								LoadingUI.loadingImage.foregroundColor = Palette.COLOR_R;
							}
							else
							{
								LoadingUI.loadingImage.backgroundColor = Color.white;
								LoadingUI.loadingImage.foregroundColor = Color.white;
							}
						}
						else
						{
							LoadingUI.loadingLabel.text = local2.format("Loading_Editor");
							LoadingUI.loadingImage.backgroundColor = Color.white;
							LoadingUI.loadingImage.foregroundColor = Color.white;
						}
					}
					else
					{
						LoadingUI.loadingLabel.text = string.Empty;
						LoadingUI.loadingImage.backgroundColor = Color.white;
						LoadingUI.loadingImage.foregroundColor = Color.white;
					}
					if (Level.info.configData.Creators.Length > 0 || Level.info.configData.Collaborators.Length > 0 || Level.info.configData.Thanks.Length > 0)
					{
						int num = 0;
						string text3 = string.Empty;
						if (Level.info.configData.Creators.Length > 0)
						{
							text3 += LoadingUI.localization.format("Creators");
							num += 15;
							for (int i = 0; i < Level.info.configData.Creators.Length; i++)
							{
								text3 = text3 + "\n" + Level.info.configData.Creators[i];
								num += 15;
							}
						}
						if (Level.info.configData.Collaborators.Length > 0)
						{
							if (text3.Length > 0)
							{
								text3 += "\n\n";
								num += 30;
							}
							text3 += LoadingUI.localization.format("Collaborators");
							num += 15;
							for (int j = 0; j < Level.info.configData.Collaborators.Length; j++)
							{
								text3 = text3 + "\n" + Level.info.configData.Collaborators[j];
								num += 15;
							}
						}
						if (Level.info.configData.Thanks.Length > 0)
						{
							if (text3.Length > 0)
							{
								text3 += "\n\n";
								num += 30;
							}
							text3 += LoadingUI.localization.format("Thanks");
							num += 15;
							for (int k = 0; k < Level.info.configData.Thanks.Length; k++)
							{
								text3 = text3 + "\n" + Level.info.configData.Thanks[k];
								num += 15;
							}
						}
						LoadingUI.creditsBox.positionOffset_Y = -num / 2;
						LoadingUI.creditsBox.sizeOffset_Y = num;
						LoadingUI.creditsBox.text = text3;
						LoadingUI.creditsBox.isVisible = true;
					}
					else
					{
						LoadingUI.creditsBox.isVisible = false;
					}
				}
				else
				{
					LoadingUI.pickBackgroundImage(ReadWrite.PATH + "/Screenshots");
					LoadingUI.loadingLabel.text = LoadingUI.localization.format("Loading");
					LoadingUI.loadingImage.backgroundColor = Color.white;
					LoadingUI.loadingImage.foregroundColor = Color.white;
					LoadingUI.creditsBox.isVisible = false;
				}
				LoadingUI.loadingBox.sizeOffset_X = -20;
				LoadingUI.cancelButton.isVisible = false;
			}
		}

		public static void rebuild()
		{
			if (LoadingUI.window != null)
			{
				LoadingUI.window.build();
			}
		}

		private static void onQueuePositionUpdated()
		{
			LoadingUI.loadingLabel.text = LoadingUI.localization.format("Queue_Position", new object[]
			{
				(int)(Provider.queuePosition + 1)
			});
			LoadingUI.loadingBox.sizeOffset_X = -130;
			LoadingUI.cancelButton.isVisible = true;
		}

		private static void onClickedCancelButton(SleekButton button)
		{
			Provider.disconnect();
		}

		private void OnGUI()
		{
			if (!Dedicator.isDedicated && (Assets.isLoading || Provider.isLoading || Level.isLoading || Player.isLoading))
			{
				LoadingUI.lastLoading = Time.realtimeSinceStartup;
			}
			if (LoadingUI.isBlocked)
			{
				LoadingUI.window.draw(false);
			}
		}

		private void Awake()
		{
			if (LoadingUI.isInitialized)
			{
				Object.Destroy(base.gameObject);
				return;
			}
			LoadingUI._isInitialized = true;
			Object.DontDestroyOnLoad(base.gameObject);
		}

		private void Start()
		{
			LoadingUI.localization = Localization.read("/Menu/MenuLoading.dat");
			LoadingUI.loader = base.gameObject;
			if (!Dedicator.isDedicated)
			{
				LoadingUI.window = new SleekWindow();
				LoadingUI.backgroundImage = new SleekImageTexture();
				LoadingUI.backgroundImage.sizeScale_X = 1f;
				LoadingUI.backgroundImage.sizeScale_Y = 1f;
				LoadingUI.window.add(LoadingUI.backgroundImage);
				LoadingUI.tipBox = new SleekBox();
				LoadingUI.tipBox.isRich = true;
				LoadingUI.tipBox.positionOffset_X = 10;
				LoadingUI.tipBox.positionOffset_Y = -100;
				LoadingUI.tipBox.positionScale_Y = 1f;
				LoadingUI.tipBox.sizeOffset_X = -20;
				LoadingUI.tipBox.sizeOffset_Y = 30;
				LoadingUI.tipBox.sizeScale_X = 1f;
				LoadingUI.tipBox.backgroundTint = ESleekTint.NONE;
				LoadingUI.tipBox.foregroundTint = ESleekTint.NONE;
				LoadingUI.window.add(LoadingUI.tipBox);
				LoadingUI.loadingBox = new SleekBox();
				LoadingUI.loadingBox.positionOffset_X = 10;
				LoadingUI.loadingBox.positionOffset_Y = -60;
				LoadingUI.loadingBox.positionScale_Y = 1f;
				LoadingUI.loadingBox.sizeOffset_X = -20;
				LoadingUI.loadingBox.sizeOffset_Y = 50;
				LoadingUI.loadingBox.sizeScale_X = 1f;
				LoadingUI.window.add(LoadingUI.loadingBox);
				LoadingUI.loadingImage = new SleekImageTexture();
				LoadingUI.loadingImage.positionOffset_X = 10;
				LoadingUI.loadingImage.positionOffset_Y = 10;
				LoadingUI.loadingImage.sizeOffset_X = -20;
				LoadingUI.loadingImage.sizeOffset_Y = -20;
				LoadingUI.loadingImage.sizeScale_X = 1f;
				LoadingUI.loadingImage.sizeScale_Y = 1f;
				LoadingUI.loadingImage.texture = (Texture2D)Resources.Load("Materials/Pixel");
				LoadingUI.loadingBox.add(LoadingUI.loadingImage);
				LoadingUI.loadingLabel = new SleekLabel();
				LoadingUI.loadingLabel.positionOffset_X = 10;
				LoadingUI.loadingLabel.positionOffset_Y = -15;
				LoadingUI.loadingLabel.positionScale_Y = 0.5f;
				LoadingUI.loadingLabel.sizeOffset_X = -20;
				LoadingUI.loadingLabel.sizeOffset_Y = 30;
				LoadingUI.loadingLabel.sizeScale_X = 1f;
				LoadingUI.loadingLabel.fontSize = 14;
				LoadingUI.loadingBox.add(LoadingUI.loadingLabel);
				LoadingUI.creditsBox = new SleekBox();
				LoadingUI.creditsBox.positionOffset_X = -125;
				LoadingUI.creditsBox.positionScale_X = 0.75f;
				LoadingUI.creditsBox.positionScale_Y = 0.5f;
				LoadingUI.creditsBox.sizeOffset_X = 250;
				LoadingUI.window.add(LoadingUI.creditsBox);
				LoadingUI.creditsBox.isVisible = false;
				LoadingUI.cancelButton = new SleekButton();
				LoadingUI.cancelButton.positionOffset_X = -110;
				LoadingUI.cancelButton.positionOffset_Y = -60;
				LoadingUI.cancelButton.positionScale_X = 1f;
				LoadingUI.cancelButton.positionScale_Y = 1f;
				LoadingUI.cancelButton.sizeOffset_X = 100;
				LoadingUI.cancelButton.sizeOffset_Y = 50;
				LoadingUI.cancelButton.fontSize = 14;
				LoadingUI.cancelButton.text = LoadingUI.localization.format("Queue_Cancel");
				LoadingUI.cancelButton.tooltip = LoadingUI.localization.format("Queue_Cancel_Tooltip");
				SleekButton sleekButton = LoadingUI.cancelButton;
				if (LoadingUI.<>f__mg$cache0 == null)
				{
					LoadingUI.<>f__mg$cache0 = new ClickedButton(LoadingUI.onClickedCancelButton);
				}
				sleekButton.onClickedButton = LoadingUI.<>f__mg$cache0;
				LoadingUI.cancelButton.isVisible = false;
				LoadingUI.window.add(LoadingUI.cancelButton);
				LoadingUI.tip = ELoadingTip.NONE;
				Delegate onQueuePositionUpdated = Provider.onQueuePositionUpdated;
				if (LoadingUI.<>f__mg$cache1 == null)
				{
					LoadingUI.<>f__mg$cache1 = new Provider.QueuePositionUpdated(LoadingUI.onQueuePositionUpdated);
				}
				Provider.onQueuePositionUpdated = (Provider.QueuePositionUpdated)Delegate.Combine(onQueuePositionUpdated, LoadingUI.<>f__mg$cache1);
			}
			else
			{
				Object.Destroy(base.gameObject);
			}
		}

		private void OnDestroy()
		{
			if (LoadingUI.window == null)
			{
				return;
			}
			LoadingUI.window.destroy();
		}

		private static readonly byte TIP_COUNT = 31;

		private static bool _isInitialized;

		public static SleekWindow window;

		private static Local localization;

		private static SleekImageTexture backgroundImage;

		private static SleekBox tipBox;

		private static SleekBox loadingBox;

		private static SleekImageTexture loadingImage;

		private static SleekLabel loadingLabel;

		private static SleekButton cancelButton;

		private static SleekBox creditsBox;

		private static float lastLoading;

		private static ELoadingTip tip;

		private static int assetsLoadCount;

		private static int assetsScanCount;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache0;

		[CompilerGenerated]
		private static Provider.QueuePositionUpdated <>f__mg$cache1;
	}
}
