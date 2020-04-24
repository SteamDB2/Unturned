using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class HumanAnimator : CharacterAnimator
	{
		public void force()
		{
			this._lean = Mathf.Clamp(this.lean, -1f, 1f);
			this._pitch = Mathf.Clamp(this.pitch, 1f, 179f) - 90f;
			this._offset = this.offset;
		}

		public void apply()
		{
			bool animationPlaying = base.getAnimationPlaying();
			if (animationPlaying)
			{
				this.leftShoulder.parent = this.skull;
				this.rightShoulder.parent = this.skull;
			}
			this.spine.Rotate(0f, this._pitch * 0.5f, this._lean * HumanAnimator.LEAN);
			this.skull.Rotate(0f, this._pitch * 0.5f, 0f);
			this.skull.position += this.skull.forward * this.offset;
			if (animationPlaying)
			{
				this.skull.Rotate(0f, -this.spine.localRotation.eulerAngles.x + this._pitch * 0.5f, 0f);
				this.leftShoulder.parent = this.spine;
				this.rightShoulder.parent = this.spine;
				this.skull.Rotate(0f, this.spine.localRotation.eulerAngles.x - this._pitch * 0.5f, 0f);
			}
		}

		private void LateUpdate()
		{
			this._lean = Mathf.LerpAngle(this._lean, Mathf.Clamp(this.lean, -1f, 1f), 4f * Time.deltaTime);
			this._pitch = Mathf.LerpAngle(this._pitch, Mathf.Clamp(this.pitch, 1f, 179f) - 90f, 8f * Time.deltaTime);
			this._offset = Mathf.Lerp(this._offset, this.offset, 4f * Time.deltaTime);
			this.apply();
		}

		private void Awake()
		{
			base.init();
		}

		public static readonly float LEAN = 20f;

		private float _lean;

		public float lean;

		private float _pitch;

		public float pitch;

		private float _offset;

		public float offset;
	}
}
