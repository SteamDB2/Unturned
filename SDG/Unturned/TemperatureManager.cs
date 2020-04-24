using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	public class TemperatureManager : MonoBehaviour
	{
		public static EPlayerTemperature checkPointTemperature(Vector3 point, bool proofFire)
		{
			EPlayerTemperature eplayerTemperature = EPlayerTemperature.NONE;
			for (int i = 0; i < TemperatureManager.bubbles.Count; i++)
			{
				TemperatureBubble temperatureBubble = TemperatureManager.bubbles[i];
				if (!(temperatureBubble.origin == null))
				{
					if (!proofFire || temperatureBubble.temperature != EPlayerTemperature.BURNING)
					{
						if ((temperatureBubble.origin.position - point).sqrMagnitude < temperatureBubble.sqrRadius)
						{
							if (temperatureBubble.temperature == EPlayerTemperature.ACID)
							{
								return temperatureBubble.temperature;
							}
							if (temperatureBubble.temperature == EPlayerTemperature.BURNING)
							{
								eplayerTemperature = temperatureBubble.temperature;
							}
							else if (eplayerTemperature != EPlayerTemperature.BURNING)
							{
								eplayerTemperature = temperatureBubble.temperature;
							}
						}
					}
				}
			}
			return eplayerTemperature;
		}

		public static TemperatureBubble registerBubble(Transform origin, float radius, EPlayerTemperature temperature)
		{
			TemperatureBubble temperatureBubble = new TemperatureBubble(origin, radius * radius, temperature);
			TemperatureManager.bubbles.Add(temperatureBubble);
			return temperatureBubble;
		}

		public static void deregisterBubble(TemperatureBubble bubble)
		{
			TemperatureManager.bubbles.Remove(bubble);
		}

		private void onLevelLoaded(int level)
		{
			TemperatureManager.bubbles = new List<TemperatureBubble>();
		}

		private void Start()
		{
			Level.onPrePreLevelLoaded = (PrePreLevelLoaded)Delegate.Combine(Level.onPrePreLevelLoaded, new PrePreLevelLoaded(this.onLevelLoaded));
		}

		private static List<TemperatureBubble> bubbles;
	}
}
