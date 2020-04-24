using System;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	public class ClaimManager : MonoBehaviour
	{
		public static bool checkCanBuild(Vector3 point, CSteamID owner, CSteamID group, bool isClaim)
		{
			for (int i = 0; i < ClaimManager.bubbles.Count; i++)
			{
				ClaimBubble claimBubble = ClaimManager.bubbles[i];
				if (((!isClaim) ? ((claimBubble.origin - point).sqrMagnitude < claimBubble.sqrRadius) : ((claimBubble.origin - point).sqrMagnitude < 4f * claimBubble.sqrRadius)) && ((!Dedicator.isDedicated) ? (!claimBubble.hasOwnership) : (!OwnershipTool.checkToggle(owner, claimBubble.owner, group, claimBubble.group))))
				{
					return false;
				}
			}
			return true;
		}

		public static ClaimBubble registerBubble(Vector3 origin, float radius, ulong owner, ulong group)
		{
			ClaimBubble claimBubble = new ClaimBubble(origin, radius * radius, owner, group);
			ClaimManager.bubbles.Add(claimBubble);
			return claimBubble;
		}

		public static void deregisterBubble(ClaimBubble bubble)
		{
			ClaimManager.bubbles.Remove(bubble);
		}

		private void onLevelLoaded(int level)
		{
			ClaimManager.bubbles = new List<ClaimBubble>();
		}

		private void Start()
		{
			Level.onPrePreLevelLoaded = (PrePreLevelLoaded)Delegate.Combine(Level.onPrePreLevelLoaded, new PrePreLevelLoaded(this.onLevelLoaded));
		}

		private static List<ClaimBubble> bubbles;
	}
}
