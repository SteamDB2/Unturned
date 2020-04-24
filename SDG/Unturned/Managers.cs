using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class Managers : MonoBehaviour
	{
		public static bool isInitialized
		{
			get
			{
				return Managers._isInitialized;
			}
		}

		private void Awake()
		{
			if (Managers.isInitialized)
			{
				Object.Destroy(base.gameObject);
				return;
			}
			Managers._isInitialized = true;
			Object.DontDestroyOnLoad(base.gameObject);
			base.GetComponent<SteamChannel>().setup();
		}

		private static bool _isInitialized;
	}
}
