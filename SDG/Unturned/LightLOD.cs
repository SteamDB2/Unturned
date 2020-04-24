using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class LightLOD : MonoBehaviour
	{
		private void apply()
		{
			if (this.targetLight == null || this.targetLight.type == 3 || this.targetLight.type == 1)
			{
				return;
			}
			if (MainCamera.instance == null)
			{
				return;
			}
			Vector3 vector = base.transform.position - MainCamera.instance.transform.position;
			float sqrMagnitude = vector.sqrMagnitude;
			if (sqrMagnitude < this.sqrTransitionStart)
			{
				if (!this.targetLight.enabled)
				{
					this.targetLight.intensity = this.intensityStart;
					this.targetLight.enabled = true;
				}
			}
			else if (sqrMagnitude > this.sqrTransitionEnd)
			{
				if (this.targetLight.enabled)
				{
					this.targetLight.intensity = this.intensityEnd;
					this.targetLight.enabled = false;
				}
			}
			else
			{
				float magnitude = vector.magnitude;
				float num = (magnitude - this.transitionStart) / this.transitionMagnitude;
				this.targetLight.intensity = Mathf.Lerp(this.intensityStart, this.intensityEnd, num);
				if (!this.targetLight.enabled)
				{
					this.targetLight.enabled = true;
				}
			}
		}

		private void Update()
		{
			this.apply();
		}

		private void Start()
		{
			if (this.targetLight == null || this.targetLight.type == 3 || this.targetLight.type == 1)
			{
				base.enabled = false;
				return;
			}
			this.intensityStart = this.targetLight.intensity;
			this.intensityEnd = 0f;
			if (this.targetLight.type == 2)
			{
				this.transitionStart = this.targetLight.range * 13f;
				this.transitionEnd = this.targetLight.range * 15f;
			}
			else if (this.targetLight.type == null)
			{
				this.transitionStart = Mathf.Max(64f, this.targetLight.range) * 1.75f;
				this.transitionEnd = Mathf.Max(64f, this.targetLight.range) * 2f;
			}
			this.transitionMagnitude = this.transitionEnd - this.transitionStart;
			this.sqrTransitionStart = this.transitionStart * this.transitionStart;
			this.sqrTransitionEnd = this.transitionEnd * this.transitionEnd;
			this.apply();
		}

		public Light targetLight;

		private float intensityStart;

		private float intensityEnd;

		private float transitionStart;

		private float transitionEnd;

		private float transitionMagnitude;

		private float sqrTransitionStart;

		private float sqrTransitionEnd;
	}
}
