using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class FlickeringLight : MonoBehaviour
	{
		private void Update()
		{
			float num = Random.Range(0.9f, 1f);
			if (Time.time - this.blackoutTime < 0.15f)
			{
				num = 0.15f;
			}
			else if (Time.time - this.blackoutTime > this.blackoutDelay)
			{
				this.blackoutTime = Time.time;
				this.blackoutDelay = Random.Range(7.3f, 13.2f);
			}
			if (this.target != null)
			{
				this.target.intensity = num;
			}
			if (this.material != null)
			{
				this.material.SetColor("_EmissionColor", new Color(num, num, num));
			}
		}

		private void Awake()
		{
			this.material = HighlighterTool.getMaterialInstance(base.transform);
			this.blackoutTime = Time.time;
			this.blackoutDelay = Random.Range(0f, 13.2f);
		}

		private void OnDestroy()
		{
			if (this.material != null)
			{
				Object.DestroyImmediate(this.material);
			}
		}

		public Light target;

		private Material material;

		private float blackoutTime;

		private float blackoutDelay;
	}
}
