using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class InteractableStereo : Interactable
	{
		public float volume
		{
			get
			{
				return this._volume;
			}
			set
			{
				this._volume = value;
				if (this.audioSource != null)
				{
					this.audioSource.volume = this.volume;
				}
			}
		}

		public byte compressedVolume
		{
			get
			{
				return (byte)Mathf.RoundToInt(this.volume * 100f);
			}
			set
			{
				this.volume = Mathf.Clamp01((float)value / 100f);
			}
		}

		public void updateTrack(Guid newTrack)
		{
			this.track.GUID = newTrack;
			if (this.audioSource != null)
			{
				StereoSongAsset stereoSongAsset = Assets.find<StereoSongAsset>(this.track);
				if (stereoSongAsset != null)
				{
					this.audioSource.clip = Assets.load<AudioClip>(stereoSongAsset.song);
				}
				else
				{
					this.audioSource.clip = null;
				}
				if (this.audioSource.clip != null)
				{
					this.audioSource.Play();
				}
				else
				{
					this.audioSource.Stop();
				}
			}
		}

		public override void updateState(Asset asset, byte[] state)
		{
			if (!Dedicator.isDedicated)
			{
				this.audioSource = base.transform.FindChild("Audio").GetComponent<AudioSource>();
			}
			GuidBuffer guidBuffer = default(GuidBuffer);
			guidBuffer.Read(state, 0);
			this.updateTrack(guidBuffer.GUID);
			this.compressedVolume = state[16];
		}

		public override void use()
		{
			PlayerBarricadeStereoUI.open(this);
			PlayerLifeUI.close();
		}

		public override bool checkHint(out EPlayerMessage message, out string text, out Color color)
		{
			message = EPlayerMessage.USE;
			text = string.Empty;
			color = Color.white;
			return !PlayerUI.window.showCursor;
		}

		protected float _volume;

		public AssetReference<StereoSongAsset> track;

		public AudioSource audioSource;
	}
}
