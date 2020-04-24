using System;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	public class PlayerVoice : PlayerCaller
	{
		public bool canHearRadio
		{
			get
			{
				return this.hasWalkieTalkie || this.hasEarpiece;
			}
		}

		public bool hasEarpiece
		{
			get
			{
				return base.player.clothing != null && base.player.clothing.maskAsset != null && base.player.clothing.maskAsset.isEarpiece;
			}
		}

		public bool isTalking { get; private set; }

		[SteamCall]
		public void tellVoice(CSteamID steamID, byte[] data, int length)
		{
			if (this.bufferReceive == null || this.received == null)
			{
				return;
			}
			if (base.channel.checkOwner(steamID) && !Provider.isServer)
			{
				if (!OptionsSettings.chatVoiceIn || base.channel.owner.isMuted)
				{
					return;
				}
				if (base.player.life.isDead)
				{
					return;
				}
				if (length <= 5)
				{
					return;
				}
				this.usingWalkieTalkie = (data[4] == 1);
				if (this.usingWalkieTalkie)
				{
					if (!this.canHearRadio)
					{
						return;
					}
					if (Player.player != null && Player.player.quests.radioFrequency != base.player.quests.radioFrequency)
					{
						return;
					}
				}
				for (int i = 0; i < length; i++)
				{
					data[i] = data[i + 5];
				}
				uint num;
				if (SteamUser.DecompressVoice(data, (uint)length, this.bufferReceive, (uint)this.bufferReceive.Length, ref num, PlayerVoice.FREQUENCY) == null)
				{
					float num2 = num / 2u / PlayerVoice.FREQUENCY;
					this.playback += num2;
					int num3 = 0;
					while ((long)num3 < (long)((ulong)num))
					{
						this.received[this.write] = (float)BitConverter.ToInt16(this.bufferReceive, num3) / 32767f;
						this.received[this.write] *= OptionsSettings.voice;
						this.write++;
						if ((long)this.write >= (long)((ulong)PlayerVoice.SAMPLES))
						{
							this.write = 0;
						}
						num3 += 2;
					}
					this.audioSource.clip.SetData(this.received, 0);
					if (!this.isPlaying)
					{
						this.needsPlay = true;
						if (this.delayPlay <= 0f)
						{
							this.delayPlay = 0.3f;
						}
					}
				}
			}
		}

		private void Update()
		{
			if (base.channel.isOwner)
			{
				if (OptionsSettings.chatVoiceOut && Input.GetKey(ControlsSettings.voice) && !base.player.life.isDead)
				{
					if (!this.isTalking)
					{
						this.isTalking = true;
						this.wasRecording = true;
						this.lastTalk = Time.realtimeSinceStartup;
						if (this.hasWalkieTalkie)
						{
							this.audioSource.PlayOneShot(Resources.Load<AudioClip>("Sounds/General/Radio"), 0.75f);
						}
						SteamUser.StartVoiceRecording();
						SteamFriends.SetInGameVoiceSpeaking(Provider.user, this.isTalking);
						if (this.onTalked != null)
						{
							this.onTalked(this.isTalking);
						}
					}
				}
				else if ((!OptionsSettings.chatVoiceOut || !Input.GetKey(ControlsSettings.voice) || base.player.life.isDead) && this.isTalking)
				{
					this.isTalking = false;
					if (this.hasWalkieTalkie)
					{
						this.audioSource.PlayOneShot(Resources.Load<AudioClip>("Sounds/General/Radio"), 0.75f);
					}
					SteamUser.StopVoiceRecording();
					SteamFriends.SetInGameVoiceSpeaking(Provider.user, this.isTalking);
					if (this.onTalked != null)
					{
						this.onTalked(this.isTalking);
					}
				}
				if (this.wasRecording && (double)(Time.realtimeSinceStartup - this.lastTalk) > 0.1)
				{
					this.wasRecording = this.isTalking;
					this.lastTalk = Time.realtimeSinceStartup;
					uint num;
					uint num2;
					if (SteamUser.GetAvailableVoice(ref num, ref num2, 0u) == null && num > 0u)
					{
						SteamUser.GetVoice(true, this.bufferSend, num, ref num, false, null, num2, ref num2, PlayerVoice.FREQUENCY);
						if (num > 0u)
						{
							for (int i = (int)(num + 4u); i > 4; i--)
							{
								this.bufferSend[i] = this.bufferSend[i - 5];
							}
							this.bufferSend[4] = ((!this.hasWalkieTalkie) ? 0 : 1);
							if (this.hasWalkieTalkie)
							{
								int call = base.channel.getCall("tellVoice");
								int size;
								byte[] packet;
								base.channel.getPacketVoice(ESteamPacket.UPDATE_VOICE, call, out size, out packet, this.bufferSend, (int)num);
								for (int j = 0; j < Provider.clients.Count; j++)
								{
									if (Provider.clients[j].playerID.steamID != Provider.client && Provider.clients[j].player != null && Provider.clients[j].player.voice.canHearRadio && Provider.clients[j].player.quests.radioFrequency == base.player.quests.radioFrequency)
									{
										Provider.send(Provider.clients[j].playerID.steamID, ESteamPacket.UPDATE_VOICE, packet, size, base.channel.id);
									}
								}
							}
							else
							{
								base.channel.sendVoice("tellVoice", ESteamCall.PEERS, base.transform.position, EffectManager.MEDIUM, ESteamPacket.UPDATE_VOICE, this.bufferSend, (int)num);
							}
						}
					}
				}
			}
			else if (!Provider.isServer)
			{
				if (this.usingWalkieTalkie)
				{
					this.audioSource.spatialBlend = 0f;
				}
				else
				{
					this.audioSource.spatialBlend = 1f;
				}
				if (this.isPlaying)
				{
					if (this.lastPlay > this.audioSource.time)
					{
						this.played += this.audioSource.clip.length;
					}
					this.lastPlay = this.audioSource.time;
					if (this.played + this.audioSource.time >= this.playback)
					{
						this.isPlaying = false;
						this.audioSource.Stop();
						if (this.usingWalkieTalkie)
						{
							this.audioSource.PlayOneShot(Resources.Load<AudioClip>("Sounds/General/Radio"), 0.75f);
						}
						this.audioSource.time = 0f;
						this.write = 0;
						this.playback = 0f;
						this.played = 0f;
						this.lastPlay = 0f;
						this.needsPlay = false;
						this.isTalking = false;
						if (this.onTalked != null)
						{
							this.onTalked(this.isTalking);
						}
					}
				}
				else if (this.needsPlay)
				{
					this.delayPlay -= Time.deltaTime;
					if (this.delayPlay <= 0f)
					{
						this.isPlaying = true;
						this.audioSource.Play();
						if (this.usingWalkieTalkie)
						{
							this.audioSource.PlayOneShot(Resources.Load<AudioClip>("Sounds/General/Radio"), 0.75f);
						}
						this.isTalking = true;
						if (this.onTalked != null)
						{
							this.onTalked(this.isTalking);
						}
					}
				}
			}
		}

		private void Start()
		{
			this.audioSource = base.GetComponent<AudioSource>();
			if (base.channel.isOwner)
			{
				this.audioSource.spatialBlend = 0f;
				this.bufferSend = new byte[8005];
			}
			else if (!Provider.isServer)
			{
				this.audioSource.clip = AudioClip.Create("Voice", (int)PlayerVoice.SAMPLES, 1, (int)PlayerVoice.FREQUENCY, false);
				this.received = new float[PlayerVoice.SAMPLES];
				this.bufferReceive = new byte[22000];
			}
		}

		private void OnDestroy()
		{
			if (base.channel.isOwner && this.isTalking)
			{
				this.isTalking = false;
				SteamUser.StopVoiceRecording();
				SteamFriends.SetInGameVoiceSpeaking(Provider.user, this.isTalking);
			}
		}

		private static readonly uint FREQUENCY = 8000u;

		private static readonly uint LENGTH = 10u;

		private static readonly uint SAMPLES = PlayerVoice.FREQUENCY * PlayerVoice.LENGTH;

		public Talked onTalked;

		private AudioSource audioSource;

		public bool hasWalkieTalkie;

		protected bool usingWalkieTalkie;

		private float[] received;

		private byte[] bufferReceive;

		private byte[] bufferSend;

		private float playback;

		private int write;

		private bool needsPlay;

		private float delayPlay;

		private float lastPlay;

		private float played;

		private float lastTalk;

		private bool isPlaying;

		private bool wasRecording;
	}
}
