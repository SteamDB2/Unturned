using System;
using System.Collections;
using System.Collections.Generic;
using SDG.Framework.Debug;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	public class Player : MonoBehaviour
	{
		public static bool isLoading
		{
			get
			{
				return Player.isLoadingLife || Player.isLoadingInventory || Player.isLoadingClothing;
			}
		}

		public static Player player
		{
			get
			{
				return Player._player;
			}
		}

		public static Player instance
		{
			get
			{
				return Player.player;
			}
		}

		public SteamChannel channel
		{
			get
			{
				return this._channel;
			}
		}

		public PlayerAnimator animator
		{
			get
			{
				return this._animator;
			}
		}

		public PlayerClothing clothing
		{
			get
			{
				return this._clothing;
			}
		}

		public PlayerInventory inventory
		{
			get
			{
				return this._inventory;
			}
		}

		public PlayerEquipment equipment
		{
			get
			{
				return this._equipment;
			}
		}

		public PlayerLife life
		{
			get
			{
				return this._life;
			}
		}

		public PlayerCrafting crafting
		{
			get
			{
				return this._crafting;
			}
		}

		public PlayerSkills skills
		{
			get
			{
				return this._skills;
			}
		}

		public PlayerMovement movement
		{
			get
			{
				return this._movement;
			}
		}

		public PlayerLook look
		{
			get
			{
				return this._look;
			}
		}

		public PlayerStance stance
		{
			get
			{
				return this._stance;
			}
		}

		public PlayerInput input
		{
			get
			{
				return this._input;
			}
		}

		public PlayerVoice voice
		{
			get
			{
				return this._voice;
			}
		}

		public PlayerInteract interact
		{
			get
			{
				return this._interact;
			}
		}

		public PlayerWorkzone workzone
		{
			get
			{
				return this._workzone;
			}
		}

		public PlayerQuests quests
		{
			get
			{
				return this._quests;
			}
		}

		public Transform first
		{
			get
			{
				return this._first;
			}
		}

		public Transform third
		{
			get
			{
				return this._third;
			}
		}

		public Transform character
		{
			get
			{
				return this._character;
			}
		}

		public bool isSpotOn
		{
			get
			{
				return this.itemOn || this.headlampOn;
			}
		}

		public void playSound(AudioClip clip, float volume, float pitch, float deviation)
		{
			if (clip == null || Dedicator.isDedicated)
			{
				return;
			}
			this.sound.spatialBlend = 1f;
			deviation = Mathf.Clamp01(deviation);
			this.sound.pitch = Random.Range(pitch * (1f - deviation), pitch * (1f + deviation));
			this.sound.PlayOneShot(clip, volume);
		}

		public void playSound(AudioClip clip, float pitch, float deviation)
		{
			this.playSound(clip, 1f, pitch, deviation);
		}

		public void playSound(AudioClip clip, float volume)
		{
			this.playSound(clip, volume, 1f, 0.1f);
		}

		public void playSound(AudioClip clip)
		{
			this.playSound(clip, 1f, 1f, 0.1f);
		}

		[SteamCall]
		public void tellScreenshotDestination(CSteamID steamID)
		{
			if (this.channel.checkServer(steamID))
			{
				this.channel.longBinaryData = true;
				byte[] array = (byte[])this.channel.read(Types.BYTE_ARRAY_TYPE);
				this.channel.longBinaryData = false;
				if (array.Length > 0)
				{
					if (Dedicator.isDedicated)
					{
						ReadWrite.writeBytes(string.Concat(new string[]
						{
							ReadWrite.PATH,
							ServerSavedata.directory,
							"/",
							Provider.serverID,
							"/Spy.jpg"
						}), false, false, array);
						ReadWrite.writeBytes(string.Concat(new object[]
						{
							ReadWrite.PATH,
							ServerSavedata.directory,
							"/",
							Provider.serverID,
							"/Spy/",
							this.channel.owner.playerID.steamID.m_SteamID,
							".jpg"
						}), false, false, array);
						if (this.onPlayerSpyReady != null)
						{
							this.onPlayerSpyReady(this.channel.owner.playerID.steamID, array);
						}
						PlayerSpyReady playerSpyReady = this.screenshotsCallbacks.Dequeue();
						if (playerSpyReady != null)
						{
							playerSpyReady(this.channel.owner.playerID.steamID, array);
						}
					}
					else
					{
						ReadWrite.writeBytes("/Spy.jpg", false, true, array);
						if (Player.onSpyReady != null)
						{
							Player.onSpyReady(this.channel.owner.playerID.steamID, array);
						}
					}
				}
			}
		}

		[SteamCall]
		public void tellScreenshotRelay(CSteamID steamID)
		{
			if (this.channel.checkOwner(steamID) && this.screenshotsExpected > 0)
			{
				this.screenshotsExpected--;
				if (this.screenshotsDestination == CSteamID.Nil)
				{
					this.tellScreenshotDestination(Provider.server);
				}
				else
				{
					this.channel.longBinaryData = true;
					byte[] array = (byte[])this.channel.read(Types.BYTE_ARRAY_TYPE);
					if (array.Length > 0)
					{
						this.channel.openWrite();
						this.channel.write(array);
						this.channel.closeWrite("tellScreenshotDestination", this.screenshotsDestination, ESteamPacket.UPDATE_RELIABLE_CHUNK_BUFFER);
					}
					this.channel.longBinaryData = false;
					ReadWrite.writeBytes(string.Concat(new object[]
					{
						ReadWrite.PATH,
						ServerSavedata.directory,
						"/",
						Provider.serverID,
						"/Spy/",
						this.channel.owner.playerID.steamID.m_SteamID,
						".jpg"
					}), false, false, array);
					if (this.onPlayerSpyReady != null)
					{
						this.onPlayerSpyReady(this.channel.owner.playerID.steamID, array);
					}
					PlayerSpyReady playerSpyReady = this.screenshotsCallbacks.Dequeue();
					if (playerSpyReady != null)
					{
						playerSpyReady(this.channel.owner.playerID.steamID, array);
					}
				}
			}
		}

		private IEnumerator takeScreenshot()
		{
			yield return new WaitForEndOfFrame();
			if (this.screenshotRaw != null && (this.screenshotRaw.width != Screen.width || this.screenshotRaw.height != Screen.height))
			{
				Object.DestroyImmediate(this.screenshotRaw);
				this.screenshotRaw = null;
			}
			if (this.screenshotRaw == null)
			{
				this.screenshotRaw = new Texture2D(Screen.width, Screen.height, 3, false);
				this.screenshotRaw.name = "Screenshot_Raw";
				this.screenshotRaw.hideFlags = 61;
			}
			this.screenshotRaw.ReadPixels(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height), 0, 0, false);
			if (this.screenshotFinal == null)
			{
				this.screenshotFinal = new Texture2D(640, 480, 3, false);
				this.screenshotFinal.name = "Screenshot_Final";
				this.screenshotFinal.hideFlags = 61;
			}
			Color[] oldColors = this.screenshotRaw.GetPixels();
			Color[] newColors = new Color[this.screenshotFinal.width * this.screenshotFinal.height];
			float widthRatio = (float)this.screenshotRaw.width / (float)this.screenshotFinal.width;
			float heightRatio = (float)this.screenshotRaw.height / (float)this.screenshotFinal.height;
			for (int i = 0; i < this.screenshotFinal.height; i++)
			{
				int num = (int)((float)i * heightRatio) * this.screenshotRaw.width;
				int num2 = i * this.screenshotFinal.width;
				for (int j = 0; j < this.screenshotFinal.width; j++)
				{
					int num3 = (int)((float)j * widthRatio);
					newColors[num2 + j] = oldColors[num + num3];
				}
			}
			this.screenshotFinal.SetPixels(newColors);
			byte[] data = this.screenshotFinal.EncodeToJPG(33);
			if (data.Length < 30000)
			{
				this.channel.longBinaryData = true;
				this.channel.openWrite();
				this.channel.write(data);
				this.channel.closeWrite("tellScreenshotRelay", ESteamCall.SERVER, ESteamPacket.UPDATE_RELIABLE_CHUNK_BUFFER);
				this.channel.longBinaryData = false;
			}
			yield break;
		}

		[SteamCall]
		public void askScreenshot(CSteamID steamID)
		{
			if (this.channel.checkServer(steamID))
			{
				base.StartCoroutine(this.takeScreenshot());
			}
		}

		public void sendScreenshot(CSteamID destination, PlayerSpyReady callback = null)
		{
			this.screenshotsExpected++;
			this.screenshotsDestination = destination;
			this.screenshotsCallbacks.Enqueue(callback);
			this.channel.send("askScreenshot", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[0]);
		}

		[SteamCall]
		public void askBrowserRequest(CSteamID steamID, string msg, string url)
		{
			if (this.channel.checkServer(steamID))
			{
				PlayerBrowserRequestUI.open(msg, url);
				PlayerLifeUI.close();
			}
		}

		public void sendBrowserRequest(string msg, string url)
		{
			this.channel.send("askBrowserRequest", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
			{
				msg,
				url
			});
		}

		public bool wantsBattlEyeLogs { get; protected set; }

		[SteamCall]
		public void askRequestBattlEyeLogs(CSteamID steamID)
		{
			if (this.channel.checkOwner(steamID))
			{
				if (!this.channel.owner.isAdmin)
				{
					return;
				}
				this.wantsBattlEyeLogs = !this.wantsBattlEyeLogs;
			}
		}

		[TerminalCommandMethod("sv.request_battleye_logs", "if admin toggle relaying serverside battleye logs")]
		public static void requestBattlEyeLogs()
		{
			TerminalUtility.printCommandPass("Sent BattlEye logs request");
			if (Player.player == null)
			{
				return;
			}
			Player.player.channel.send("askRequestBattlEyeLogs", ESteamCall.SERVER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[0]);
		}

		[SteamCall]
		public void tellTerminalRelay(CSteamID steamID, string internalMessage, string internalCategory, string displayCategory)
		{
			if (this.channel.checkServer(steamID))
			{
				Terminal.print(internalMessage, null, internalCategory, displayCategory, true);
			}
		}

		public void sendTerminalRelay(string internalMessage, string internalCategory, string displayCategory)
		{
			this.channel.send("tellTerminalRelay", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
			{
				internalMessage,
				internalCategory,
				displayCategory
			});
		}

		[SteamCall]
		public void askTeleport(CSteamID steamID, Vector3 position, byte angle)
		{
			if (this.channel.checkServer(steamID))
			{
				base.transform.position = position + new Vector3(0f, 0.5f, 0f);
				base.transform.rotation = Quaternion.Euler(0f, (float)(angle * 2), 0f);
				this.look.updateLook();
				this.movement.updateMovement();
				this.movement.isAllowed = true;
				if (this.onPlayerTeleported != null)
				{
					this.onPlayerTeleported(this, position);
				}
			}
		}

		public void sendTeleport(Vector3 position, byte angle)
		{
			this.channel.send("askTeleport", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
			{
				position,
				angle
			});
		}

		[SteamCall]
		public void tellStat(CSteamID steamID, byte newStat)
		{
			if (this.channel.checkServer(steamID))
			{
				if (newStat == 0)
				{
					return;
				}
				int num11;
				if (newStat == 2)
				{
					int num;
					if (Provider.provider.statisticsService.userStatisticsService.getStatistic("Kills_Players", out num))
					{
						Provider.provider.statisticsService.userStatisticsService.setStatistic("Kills_Players", num + 1);
					}
				}
				else if (newStat == 1)
				{
					int num2;
					if (Provider.provider.statisticsService.userStatisticsService.getStatistic("Kills_Zombies_Normal", out num2))
					{
						Provider.provider.statisticsService.userStatisticsService.setStatistic("Kills_Zombies_Normal", num2 + 1);
					}
				}
				else if (newStat == 6)
				{
					int num3;
					if (Provider.provider.statisticsService.userStatisticsService.getStatistic("Kills_Zombies_Mega", out num3))
					{
						Provider.provider.statisticsService.userStatisticsService.setStatistic("Kills_Zombies_Mega", num3 + 1);
					}
				}
				else if (newStat == 3)
				{
					int num4;
					if (Provider.provider.statisticsService.userStatisticsService.getStatistic("Found_Items", out num4))
					{
						Provider.provider.statisticsService.userStatisticsService.setStatistic("Found_Items", num4 + 1);
					}
				}
				else if (newStat == 4)
				{
					int num5;
					if (Provider.provider.statisticsService.userStatisticsService.getStatistic("Found_Resources", out num5))
					{
						Provider.provider.statisticsService.userStatisticsService.setStatistic("Found_Resources", num5 + 1);
					}
				}
				else if (newStat == 8)
				{
					int num6;
					if (Provider.provider.statisticsService.userStatisticsService.getStatistic("Kills_Animals", out num6))
					{
						Provider.provider.statisticsService.userStatisticsService.setStatistic("Kills_Animals", num6 + 1);
					}
				}
				else if (newStat == 9)
				{
					int num7;
					if (Provider.provider.statisticsService.userStatisticsService.getStatistic("Found_Crafts", out num7))
					{
						Provider.provider.statisticsService.userStatisticsService.setStatistic("Found_Crafts", num7 + 1);
					}
				}
				else if (newStat == 10)
				{
					int num8;
					if (Provider.provider.statisticsService.userStatisticsService.getStatistic("Found_Fishes", out num8))
					{
						Provider.provider.statisticsService.userStatisticsService.setStatistic("Found_Fishes", num8 + 1);
					}
				}
				else if (newStat == 11)
				{
					int num9;
					if (Provider.provider.statisticsService.userStatisticsService.getStatistic("Found_Plants", out num9))
					{
						Provider.provider.statisticsService.userStatisticsService.setStatistic("Found_Plants", num9 + 1);
					}
				}
				else if (newStat == 16)
				{
					int num10;
					if (Provider.provider.statisticsService.userStatisticsService.getStatistic("Arena_Wins", out num10))
					{
						Provider.provider.statisticsService.userStatisticsService.setStatistic("Arena_Wins", num10 + 1);
					}
				}
				else if (newStat == 17 && Provider.provider.statisticsService.userStatisticsService.getStatistic("Found_Buildables", out num11))
				{
					Provider.provider.statisticsService.userStatisticsService.setStatistic("Found_Buildables", num11 + 1);
				}
			}
		}

		public void sendStat(EPlayerStat stat)
		{
			this.channel.send("tellStat", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
			{
				(byte)stat
			});
		}

		[SteamCall]
		public void askMessage(CSteamID steamID, byte message)
		{
			if (this.channel.checkServer(steamID))
			{
				PlayerUI.message((EPlayerMessage)message, string.Empty);
			}
		}

		public void sendMessage(EPlayerMessage message)
		{
			this.channel.send("askMessage", ESteamCall.OWNER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[]
			{
				(byte)message
			});
		}

		public void updateSpot(bool on)
		{
			this.itemOn = on;
			this.updateLights();
		}

		public void updateGlassesLights(bool on)
		{
			if (this.clothing.firstClothes != null && this.clothing.firstClothes.glassesModel != null)
			{
				Transform transform = this.clothing.firstClothes.glassesModel.FindChild("Model_0");
				if (transform != null)
				{
					Transform transform2 = transform.FindChild("Light");
					if (transform2 != null)
					{
						transform2.gameObject.SetActive(on);
					}
				}
			}
			if (this.clothing.thirdClothes != null && this.clothing.thirdClothes.glassesModel != null)
			{
				Transform transform3 = this.clothing.thirdClothes.glassesModel.FindChild("Model_0");
				if (transform3 != null)
				{
					Transform transform4 = transform3.FindChild("Light");
					if (transform4 != null)
					{
						transform4.gameObject.SetActive(on);
					}
				}
			}
			if (this.clothing.characterClothes != null && this.clothing.characterClothes.glassesModel != null)
			{
				Transform transform5 = this.clothing.characterClothes.glassesModel.FindChild("Model_0");
				if (transform5 != null)
				{
					Transform transform6 = transform5.FindChild("Light");
					if (transform6 != null)
					{
						transform6.gameObject.SetActive(on);
					}
				}
			}
		}

		public void updateHeadlamp(bool on)
		{
			this.headlampOn = on;
			this.updateLights();
		}

		private void updateLights()
		{
			if (!Dedicator.isDedicated)
			{
				if (this.channel.isOwner)
				{
					this.firstSpot.gameObject.SetActive(this.isSpotOn && Player.player.look.perspective == EPlayerPerspective.FIRST);
					this.thirdSpot.gameObject.SetActive(this.isSpotOn && Player.player.look.perspective == EPlayerPerspective.THIRD);
				}
				else
				{
					this.thirdSpot.gameObject.SetActive(this.isSpotOn);
				}
			}
		}

		private void onPerspectiveUpdated(EPlayerPerspective newPerspective)
		{
			if (this.isSpotOn)
			{
				this.updateLights();
			}
		}

		private void Start()
		{
			if (this.channel.isOwner)
			{
				Player._player = this;
				this._first = base.transform.FindChild("First");
				this._third = base.transform.FindChild("Third");
				this.first.gameObject.SetActive(true);
				this.third.gameObject.SetActive(true);
				this._character = ((GameObject)Object.Instantiate(Resources.Load("Characters/Inspect"))).transform;
				this.character.name = "Inspect";
				this.character.transform.position = new Vector3(256f, -256f, 0f);
				this.character.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
				this.firstSpot = MainCamera.instance.transform.FindChild("Spot");
				Player.isLoadingInventory = true;
				Player.isLoadingLife = true;
				Player.isLoadingClothing = true;
				PlayerLook look = this.look;
				look.onPerspectiveUpdated = (PerspectiveUpdated)Delegate.Combine(look.onPerspectiveUpdated, new PerspectiveUpdated(this.onPerspectiveUpdated));
			}
			else
			{
				this._first = null;
				this._third = base.transform.FindChild("Third");
				this.third.gameObject.SetActive(true);
			}
			this.thirdSpot = this.third.FindChild("Skeleton").FindChild("Spine").FindChild("Skull").FindChild("Spot");
		}

		private void Awake()
		{
			this._channel = base.GetComponent<SteamChannel>();
			this.agro = 0;
			this._animator = base.GetComponent<PlayerAnimator>();
			this._clothing = base.GetComponent<PlayerClothing>();
			this._inventory = base.GetComponent<PlayerInventory>();
			this._equipment = base.GetComponent<PlayerEquipment>();
			this._life = base.GetComponent<PlayerLife>();
			this._crafting = base.GetComponent<PlayerCrafting>();
			this._skills = base.GetComponent<PlayerSkills>();
			this._movement = base.GetComponent<PlayerMovement>();
			this._look = base.GetComponent<PlayerLook>();
			this._stance = base.GetComponent<PlayerStance>();
			this._input = base.GetComponent<PlayerInput>();
			this._voice = base.GetComponent<PlayerVoice>();
			this._interact = base.GetComponent<PlayerInteract>();
			this._workzone = base.GetComponent<PlayerWorkzone>();
			this._quests = base.GetComponent<PlayerQuests>();
			Transform transform = base.transform.FindChild("Sound");
			if (transform != null)
			{
				this.sound = transform.GetComponent<AudioSource>();
			}
		}

		private void OnDestroy()
		{
			if (this.screenshotRaw != null)
			{
				Object.DestroyImmediate(this.screenshotRaw);
				this.screenshotRaw = null;
			}
			if (this.screenshotFinal != null)
			{
				Object.DestroyImmediate(this.screenshotFinal);
				this.screenshotFinal = null;
			}
			if (this.channel != null && this.channel.isOwner)
			{
				Player.isLoadingInventory = false;
				Player.isLoadingLife = false;
				Player.isLoadingClothing = false;
			}
		}

		public void save()
		{
			if (this.life.isDead)
			{
				if (PlayerSavedata.fileExists(this.channel.owner.playerID, "/Player/Player.dat"))
				{
					PlayerSavedata.deleteFile(this.channel.owner.playerID, "/Player/Player.dat");
				}
			}
			else
			{
				Block block = new Block();
				block.writeByte(Player.SAVEDATA_VERSION);
				block.writeSingleVector3(base.transform.position);
				block.writeByte((byte)(base.transform.rotation.eulerAngles.y / 2f));
				PlayerSavedata.writeBlock(this.channel.owner.playerID, "/Player/Player.dat", block);
			}
			this.clothing.save();
			this.inventory.save();
			this.life.save();
			this.skills.save();
			this.animator.save();
			this.quests.save();
		}

		public static readonly byte SAVEDATA_VERSION = 1;

		public static PlayerCreated onPlayerCreated;

		public PlayerTeleported onPlayerTeleported;

		public PlayerSpyReady onPlayerSpyReady;

		public static PlayerSpyReady onSpyReady;

		public static bool isLoadingInventory;

		public static bool isLoadingLife;

		public static bool isLoadingClothing;

		public int agro;

		private static Player _player;

		protected SteamChannel _channel;

		private PlayerAnimator _animator;

		private PlayerClothing _clothing;

		private PlayerInventory _inventory;

		private PlayerEquipment _equipment;

		private PlayerLife _life;

		private PlayerCrafting _crafting;

		private PlayerSkills _skills;

		private PlayerMovement _movement;

		private PlayerLook _look;

		private PlayerStance _stance;

		private PlayerInput _input;

		private PlayerVoice _voice;

		private PlayerInteract _interact;

		private PlayerWorkzone _workzone;

		private PlayerQuests _quests;

		private Transform _first;

		private Transform _third;

		private Transform _character;

		private Transform firstSpot;

		private Transform thirdSpot;

		private bool itemOn;

		private bool headlampOn;

		private AudioSource sound;

		private int screenshotsExpected;

		private CSteamID screenshotsDestination;

		private Queue<PlayerSpyReady> screenshotsCallbacks = new Queue<PlayerSpyReady>();

		private Texture2D screenshotRaw;

		private Texture2D screenshotFinal;
	}
}
