using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class Tools : MonoBehaviour
	{
		public static bool isInitialized
		{
			get
			{
				return Tools._isInitialized;
			}
		}

		private void Awake()
		{
			if (Tools.isInitialized)
			{
				Object.Destroy(base.gameObject);
				return;
			}
			Tools._isInitialized = true;
			Object.DontDestroyOnLoad(base.gameObject);
		}

		private static bool _isInitialized;
	}
}
