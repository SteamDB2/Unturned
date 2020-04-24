using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class CharacterAnimator : MonoBehaviour
	{
		public void sample()
		{
			base.GetComponent<Animation>().Sample();
		}

		public void mixAnimation(string name)
		{
			base.GetComponent<Animation>()[name].layer = 1;
		}

		public void mixAnimation(string name, bool mixLeftShoulder, bool mixRightShoulder)
		{
			this.mixAnimation(name, mixLeftShoulder, mixRightShoulder, false);
		}

		public void mixAnimation(string name, bool mixLeftShoulder, bool mixRightShoulder, bool mixSkull)
		{
			if (mixLeftShoulder)
			{
				this.anim[name].AddMixingTransform(this.leftShoulder, true);
			}
			if (mixRightShoulder)
			{
				this.anim[name].AddMixingTransform(this.rightShoulder, true);
			}
			if (mixSkull)
			{
				this.anim[name].AddMixingTransform(this.skull, true);
			}
			base.GetComponent<Animation>()[name].layer = 1;
		}

		public void addAnimation(AnimationClip clip)
		{
			this.anim.AddClip(clip, clip.name);
			this.mixAnimation(clip.name, true, true);
		}

		public void removeAnimation(AnimationClip clip)
		{
			if (this.anim[clip.name] != null)
			{
				this.anim.RemoveClip(clip);
			}
		}

		public void setAnimationSpeed(string name, float speed)
		{
			if (this.anim[name] != null)
			{
				this.anim[name].speed = speed;
			}
		}

		public float getAnimationLength(string name)
		{
			if (this.anim[name] != null)
			{
				return this.anim[name].clip.length / base.GetComponent<Animation>()[name].speed;
			}
			return 0f;
		}

		public void getAnimationSample(string name, float point)
		{
			if (this.anim[name] != null)
			{
				this.anim[name].clip.SampleAnimation(base.gameObject, point);
			}
		}

		public bool getAnimationPlaying()
		{
			return !string.IsNullOrEmpty(base.name) && this.anim.IsPlaying(this.clip);
		}

		public void state(string name)
		{
			if (this.anim[name] == null)
			{
				return;
			}
			this.anim.CrossFade(name, CharacterAnimator.BLEND);
		}

		public bool checkExists(string name)
		{
			return this.anim[name] != null;
		}

		public void play(string name, bool smooth)
		{
			if (this.anim[name] == null)
			{
				return;
			}
			if (this.clip != string.Empty)
			{
				this.anim.Stop(this.clip);
			}
			this.clip = name;
			if (smooth)
			{
				this.anim.CrossFade(name, CharacterAnimator.BLEND);
			}
			else
			{
				this.anim.Play(name);
			}
		}

		public void stop(string name)
		{
			if (this.anim[name] == null)
			{
				return;
			}
			if (name == this.clip)
			{
				this.anim.Stop(name);
				this.clip = string.Empty;
			}
		}

		protected void init()
		{
			this.clip = string.Empty;
			this.anim = base.GetComponent<Animation>();
			this.spine = base.transform.FindChild("Skeleton").FindChild("Spine");
			this.skull = this.spine.FindChild("Skull");
			this.leftShoulder = this.spine.FindChild("Left_Shoulder");
			this.rightShoulder = this.spine.FindChild("Right_Shoulder");
		}

		private void Awake()
		{
			this.init();
		}

		public static readonly float BLEND = 0.25f;

		protected Animation anim;

		protected Transform spine;

		protected Transform skull;

		protected Transform leftShoulder;

		protected Transform rightShoulder;

		protected string clip;
	}
}
