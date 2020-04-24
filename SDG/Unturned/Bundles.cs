using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class Bundles : MonoBehaviour
	{
		public static bool isInitialized
		{
			get
			{
				return Bundles._isInitialized;
			}
		}

		public static Bundle getBundle(string path)
		{
			return Bundles.getBundle(path, true);
		}

		public static Bundle getBundle(string path, bool usePath)
		{
			return Bundles.getBundle(path, usePath, false);
		}

		public static Bundle getBundle(string path, bool usePath, bool loadFromResources)
		{
			return new Bundle(path, usePath, loadFromResources);
		}

		private void Awake()
		{
			if (Bundles.isInitialized)
			{
				Object.Destroy(base.gameObject);
				return;
			}
			Bundles._isInitialized = true;
			Object.DontDestroyOnLoad(base.gameObject);
		}

		private static bool _isInitialized;
	}
}
