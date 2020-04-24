using System;
using SDG.Framework.Devkit;
using UnityEngine;

namespace SDG.Unturned
{
	public class GameMode : IDevkitHierarchySpawnable
	{
		public GameMode()
		{
			Debug.Log(this);
		}

		public void devkitHierarchySpawn()
		{
		}

		public virtual GameObject getPlayerGameObject(SteamPlayerID playerID)
		{
			if (Dedicator.isDedicated)
			{
				return Object.Instantiate<GameObject>(Resources.Load<GameObject>("Characters/Player_Dedicated"));
			}
			if (playerID.steamID == Provider.client)
			{
				return Object.Instantiate<GameObject>(Resources.Load<GameObject>("Characters/Player_Server"));
			}
			return Object.Instantiate<GameObject>(Resources.Load<GameObject>("Characters/Player_Client"));
		}
	}
}
