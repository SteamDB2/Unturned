using System;
using SDG.Framework.Modules;
using UnityEngine;

namespace SDG.Unturned
{
	public class Setup : MonoBehaviour
	{
		private void Awake()
		{
			base.GetComponent<Dedicator>().awake();
			base.GetComponent<Logs>().awake();
			base.GetComponent<ModuleHook>().awake();
			base.GetComponent<Provider>().awake();
			base.GetComponent<ModuleHook>().start();
			base.GetComponent<Provider>().start();
		}

		private void Start()
		{
			if (!Dedicator.isDedicated)
			{
				MenuSettings.load();
				GraphicsSettings.resize();
				LoadingUI.updateScene();
			}
		}
	}
}
