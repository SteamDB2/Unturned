using System;
using SDG.Framework.Water;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	public class UseableFisher : Useable
	{
		private bool isCastable
		{
			get
			{
				return Time.realtimeSinceStartup - this.startedCast > this.castTime;
			}
		}

		private bool isReelable
		{
			get
			{
				return Time.realtimeSinceStartup - this.startedReel > this.reelTime;
			}
		}

		private bool isBobable
		{
			get
			{
				return (!this.isCasting) ? (Time.realtimeSinceStartup - this.startedReel > this.reelTime * 0.75f) : (Time.realtimeSinceStartup - this.startedCast > this.castTime * 0.45f);
			}
		}

		private void reel()
		{
			if (!Dedicator.isDedicated)
			{
				base.player.playSound(((ItemFisherAsset)base.player.equipment.asset).reel);
			}
			base.player.animator.play("Reel", false);
		}

		private void startStrength()
		{
			PlayerLifeUI.close();
			if (this.castStrengthBox != null)
			{
				this.castStrengthBox.isVisible = true;
			}
		}

		private void stopStrength()
		{
			PlayerLifeUI.open();
			if (this.castStrengthBox != null)
			{
				this.castStrengthBox.isVisible = false;
			}
		}

		[SteamCall]
		public void askCatch(CSteamID steamID)
		{
			if (base.channel.checkOwner(steamID) && (Time.realtimeSinceStartup - this.lastLuck > this.luckTime - 2.4f || (this.hasLuckReset && Time.realtimeSinceStartup - this.lastLuck < 1f)))
			{
				this.isCatch = true;
			}
		}

		[SteamCall]
		public void askReel(CSteamID steamID)
		{
			if (base.channel.checkServer(steamID) && base.player.equipment.isEquipped)
			{
				this.reel();
			}
		}

		private void cast()
		{
			if (!Dedicator.isDedicated)
			{
				base.player.playSound(((ItemFisherAsset)base.player.equipment.asset).cast);
			}
			base.player.animator.play("Cast", false);
		}

		[SteamCall]
		public void askCast(CSteamID steamID)
		{
			if (base.channel.checkServer(steamID) && base.player.equipment.isEquipped)
			{
				this.cast();
			}
		}

		public override void startPrimary()
		{
			if (base.player.equipment.isBusy)
			{
				return;
			}
			if (this.isFishing)
			{
				this.isFishing = false;
				base.player.equipment.isBusy = true;
				this.startedReel = Time.realtimeSinceStartup;
				this.isReeling = true;
				if (base.channel.isOwner)
				{
					this.isBobbing = true;
					if (Time.realtimeSinceStartup - this.lastLuck > this.luckTime - 1.4f && Time.realtimeSinceStartup - this.lastLuck < this.luckTime)
					{
						base.channel.send("askCatch", ESteamCall.SERVER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[0]);
					}
				}
				this.reel();
				if (Provider.isServer)
				{
					if (this.isCatch)
					{
						this.isCatch = false;
						ushort num = SpawnTableTool.resolve(((ItemFisherAsset)base.player.equipment.asset).rewardID);
						if (num != 0)
						{
							base.player.inventory.forceAddItem(new Item(num, EItemOrigin.NATURE), false);
						}
						base.player.sendStat(EPlayerStat.FOUND_FISHES);
						base.player.skills.askPay(3u);
					}
					base.channel.send("askReel", ESteamCall.NOT_OWNER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[0]);
					AlertTool.alert(base.transform.position, 8f);
				}
			}
			else
			{
				this.isStrengthening = true;
				this.strengthTime = 0u;
				this.strengthMultiplier = 0f;
				if (base.channel.isOwner)
				{
					this.startStrength();
				}
			}
		}

		public override void stopPrimary()
		{
			if (base.player.equipment.isBusy)
			{
				return;
			}
			if (!this.isStrengthening)
			{
				return;
			}
			this.isStrengthening = false;
			if (base.channel.isOwner)
			{
				this.stopStrength();
			}
			this.isFishing = true;
			base.player.equipment.isBusy = true;
			this.startedCast = Time.realtimeSinceStartup;
			this.isCasting = true;
			if (base.channel.isOwner)
			{
				this.isBobbing = true;
			}
			this.resetLuck();
			this.hasLuckReset = false;
			this.cast();
			if (Provider.isServer)
			{
				base.channel.send("askCast", ESteamCall.NOT_OWNER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[0]);
				AlertTool.alert(base.transform.position, 8f);
			}
		}

		public override void equip()
		{
			base.player.animator.play("Equip", true);
			this.castTime = base.player.animator.getAnimationLength("Cast");
			this.reelTime = base.player.animator.getAnimationLength("Reel");
			if (base.channel.isOwner)
			{
				this.firstHook = base.player.equipment.firstModel.FindChild("Hook");
				this.thirdHook = base.player.equipment.thirdModel.FindChild("Hook");
				this.firstLine = (LineRenderer)base.player.equipment.firstModel.FindChild("Line").GetComponent<Renderer>();
				this.firstLine.tag = "Viewmodel";
				this.firstLine.gameObject.layer = LayerMasks.VIEWMODEL;
				this.firstLine.gameObject.SetActive(true);
				this.thirdLine = (LineRenderer)base.player.equipment.thirdModel.FindChild("Line").GetComponent<Renderer>();
				this.thirdLine.gameObject.SetActive(true);
				this.castStrengthBox = new SleekBox();
				this.castStrengthBox.positionOffset_X = -20;
				this.castStrengthBox.positionOffset_Y = -110;
				this.castStrengthBox.positionScale_X = 0.5f;
				this.castStrengthBox.positionScale_Y = 0.5f;
				this.castStrengthBox.sizeOffset_X = 40;
				this.castStrengthBox.sizeOffset_Y = 220;
				PlayerUI.container.add(this.castStrengthBox);
				this.castStrengthBox.isVisible = false;
				this.castStrengthArea = new Sleek();
				this.castStrengthArea.positionOffset_X = 10;
				this.castStrengthArea.positionOffset_Y = 10;
				this.castStrengthArea.sizeOffset_X = -20;
				this.castStrengthArea.sizeOffset_Y = -20;
				this.castStrengthArea.sizeScale_X = 1f;
				this.castStrengthArea.sizeScale_Y = 1f;
				this.castStrengthBox.add(this.castStrengthArea);
				this.castStrengthBar = new SleekImageTexture();
				this.castStrengthBar.sizeScale_X = 1f;
				this.castStrengthBar.sizeScale_Y = 1f;
				this.castStrengthBar.texture = (Texture2D)Resources.Load("Materials/Pixel");
				this.castStrengthArea.add(this.castStrengthBar);
			}
		}

		public override void dequip()
		{
			if (base.channel.isOwner)
			{
				if (this.bob != null)
				{
					Object.Destroy(this.bob.gameObject);
				}
				if (this.castStrengthBox != null)
				{
					PlayerUI.container.remove(this.castStrengthBox);
				}
				if (this.isStrengthening)
				{
					PlayerLifeUI.open();
				}
			}
		}

		public override void tock(uint clock)
		{
			if (!this.isStrengthening)
			{
				return;
			}
			this.strengthTime += 1u;
			uint num = (uint)(100 + base.player.skills.skills[2][4].level * 20);
			this.strengthMultiplier = 1f - Mathf.Abs(Mathf.Sin((this.strengthTime + num / 2u) % num / num * 3.14159274f));
			this.strengthMultiplier *= this.strengthMultiplier;
			if (base.channel.isOwner && this.castStrengthBar != null)
			{
				this.castStrengthBar.positionScale_Y = 1f - this.strengthMultiplier;
				this.castStrengthBar.sizeScale_Y = this.strengthMultiplier;
				this.castStrengthBar.backgroundColor = ItemTool.getQualityColor(this.strengthMultiplier);
			}
		}

		public override void tick()
		{
			if (!base.player.equipment.isEquipped)
			{
				return;
			}
			if (base.channel.isOwner)
			{
				if (this.isBobable && this.isBobbing)
				{
					if (this.isCasting)
					{
						this.bob = ((GameObject)Object.Instantiate(Resources.Load("Fishers/Bob"))).transform;
						this.bob.name = "Bob";
						this.bob.parent = Level.effects;
						this.bob.position = base.player.look.aim.position + base.player.look.aim.forward;
						this.bob.GetComponent<Rigidbody>().AddForce(base.player.look.aim.forward * Mathf.Lerp(500f, 1000f, this.strengthMultiplier));
						this.isBobbing = false;
						this.isLuring = true;
					}
					else if (this.isReeling && this.bob != null)
					{
						Object.Destroy(this.bob.gameObject);
					}
				}
				if (this.bob != null)
				{
					if (base.player.look.perspective == EPlayerPerspective.FIRST)
					{
						Vector3 vector = MainCamera.instance.WorldToViewportPoint(this.bob.position);
						Vector3 vector2 = base.player.animator.view.GetComponent<Camera>().ViewportToWorldPoint(vector);
						this.firstLine.SetPosition(0, this.firstHook.position);
						this.firstLine.SetPosition(1, vector2);
					}
					else
					{
						this.thirdLine.SetPosition(0, this.thirdHook.position);
						this.thirdLine.SetPosition(1, this.bob.position);
					}
				}
				else if (base.player.look.perspective == EPlayerPerspective.FIRST)
				{
					this.firstLine.SetPosition(0, Vector3.zero);
					this.firstLine.SetPosition(1, Vector3.zero);
				}
				else
				{
					this.thirdLine.SetPosition(0, Vector3.zero);
					this.thirdLine.SetPosition(1, Vector3.zero);
				}
			}
		}

		public override void simulate(uint simulation, bool inputSteady)
		{
			if (this.isCasting && this.isCastable)
			{
				base.player.equipment.isBusy = false;
				this.isCasting = false;
			}
			else if (this.isReeling && this.isReelable)
			{
				base.player.equipment.isBusy = false;
				this.isReeling = false;
			}
			if (!base.channel.isOwner && Time.realtimeSinceStartup - this.lastLuck > this.luckTime && !this.isReeling)
			{
				this.resetLuck();
				this.hasLuckReset = true;
			}
		}

		private void resetLuck()
		{
			this.lastLuck = Time.realtimeSinceStartup;
			this.luckTime = 54.2f - this.strengthMultiplier * 33.5f;
			this.hasSplashed = false;
			this.hasTugged = false;
		}

		private void Update()
		{
			if (this.bob != null)
			{
				if (this.isLuring)
				{
					bool flag;
					float num;
					WaterUtility.getUnderwaterInfo(this.bob.position, out flag, out num);
					if (flag && this.bob.position.y < num - 4f)
					{
						this.bob.GetComponent<Rigidbody>().useGravity = false;
						this.bob.GetComponent<Rigidbody>().isKinematic = true;
						this.water = this.bob.position;
						this.water.y = num;
						this.isLuring = false;
					}
				}
				else
				{
					if (Time.realtimeSinceStartup - this.lastLuck > this.luckTime)
					{
						if (!this.isReeling)
						{
							this.resetLuck();
							this.hasLuckReset = true;
						}
					}
					else if (Time.realtimeSinceStartup - this.lastLuck > this.luckTime - 1.4f)
					{
						if (!this.hasTugged)
						{
							this.hasTugged = true;
							base.player.playSound(((ItemFisherAsset)base.player.equipment.asset).tug);
							base.player.animator.play("Tug", false);
						}
					}
					else if (Time.realtimeSinceStartup - this.lastLuck > this.luckTime - 2.4f && !this.hasSplashed)
					{
						this.hasSplashed = true;
						Transform transform = ((GameObject)Object.Instantiate(Resources.Load("Fishers/Splash"))).transform;
						transform.name = "Splash";
						transform.parent = Level.effects;
						transform.position = this.water;
						transform.rotation = Quaternion.Euler(-90f, Random.Range(0f, 360f), 0f);
						Object.Destroy(transform.gameObject, 8f);
					}
					if (Time.realtimeSinceStartup - this.lastLuck > this.luckTime - 1.4f)
					{
						this.bob.GetComponent<Rigidbody>().MovePosition(Vector3.Lerp(this.bob.position, this.water + Vector3.down * 4f + Vector3.left * Random.Range(-4f, 4f) + Vector3.forward * Random.Range(-4f, 4f), 4f * Time.deltaTime));
					}
					else
					{
						this.bob.GetComponent<Rigidbody>().MovePosition(Vector3.Lerp(this.bob.position, this.water + Vector3.up * Mathf.Sin(Time.time) * 0.25f, 4f * Time.deltaTime));
					}
				}
			}
		}

		private float startedCast;

		private float startedReel;

		private float castTime;

		private float reelTime;

		private bool isStrengthening;

		private bool isCasting;

		private bool isReeling;

		private bool isFishing;

		private bool isBobbing;

		private bool isLuring;

		private bool isCatch;

		private Transform bob;

		private Transform firstHook;

		private Transform thirdHook;

		private LineRenderer firstLine;

		private LineRenderer thirdLine;

		private Vector3 water;

		private uint strengthTime;

		private float strengthMultiplier;

		private float lastLuck;

		private float luckTime;

		private bool hasLuckReset;

		private bool hasSplashed;

		private bool hasTugged;

		private SleekBox castStrengthBox;

		private Sleek castStrengthArea;

		private SleekImageTexture castStrengthBar;
	}
}
