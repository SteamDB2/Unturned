using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	public class OxygenManager : MonoBehaviour
	{
		public static bool checkPointBreathable(Vector3 point)
		{
			for (int i = 0; i < OxygenManager.bubbles.Count; i++)
			{
				OxygenBubble oxygenBubble = OxygenManager.bubbles[i];
				if (!(oxygenBubble.origin == null))
				{
					if ((oxygenBubble.origin.position - point).sqrMagnitude < oxygenBubble.sqrRadius)
					{
						return true;
					}
				}
			}
			return false;
		}

		public static OxygenBubble registerBubble(Transform origin, float radius)
		{
			OxygenBubble oxygenBubble = new OxygenBubble(origin, radius * radius);
			OxygenManager.bubbles.Add(oxygenBubble);
			return oxygenBubble;
		}

		public static void deregisterBubble(OxygenBubble bubble)
		{
			OxygenManager.bubbles.Remove(bubble);
		}

		private void onLevelLoaded(int level)
		{
			OxygenManager.bubbles = new List<OxygenBubble>();
		}

		private void Start()
		{
			Level.onPrePreLevelLoaded = (PrePreLevelLoaded)Delegate.Combine(Level.onPrePreLevelLoaded, new PrePreLevelLoaded(this.onLevelLoaded));
		}

		private static List<OxygenBubble> bubbles;
	}
}
