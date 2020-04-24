using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class NightLight : MonoBehaviour
	{
		private void onDayNightUpdated(bool isDaytime)
		{
			if (this.target != null)
			{
				this.target.gameObject.SetActive(!isDaytime);
			}
			if (this.material != null)
			{
				this.material.SetColor("_EmissionColor", (!isDaytime) ? Color.white : Color.black);
			}
		}

		private void Awake()
		{
			this.material = HighlighterTool.getMaterialInstance(base.transform);
			if (Level.isEditor)
			{
				this.onDayNightUpdated(false);
				return;
			}
			this.onDayNightUpdated(LightingManager.isDaytime);
			if (this.isListening)
			{
				return;
			}
			this.isListening = true;
			LightingManager.onDayNightUpdated = (DayNightUpdated)Delegate.Combine(LightingManager.onDayNightUpdated, new DayNightUpdated(this.onDayNightUpdated));
		}

		private void OnDestroy()
		{
			if (this.material != null)
			{
				Object.DestroyImmediate(this.material);
			}
			if (!this.isListening)
			{
				return;
			}
			this.isListening = false;
			LightingManager.onDayNightUpdated = (DayNightUpdated)Delegate.Remove(LightingManager.onDayNightUpdated, new DayNightUpdated(this.onDayNightUpdated));
		}

		public Light target;

		private Material material;

		private bool isListening;
	}
}
