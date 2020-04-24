using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class TemperatureTrigger : MonoBehaviour
	{
		private void OnEnable()
		{
			if (this.bubble != null)
			{
				return;
			}
			this.bubble = TemperatureManager.registerBubble(base.transform, base.transform.localScale.x, this.temperature);
		}

		private void OnDisable()
		{
			if (this.bubble == null)
			{
				return;
			}
			TemperatureManager.deregisterBubble(this.bubble);
			this.bubble = null;
		}

		public EPlayerTemperature temperature;

		private TemperatureBubble bubble;
	}
}
