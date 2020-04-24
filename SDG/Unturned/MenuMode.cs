using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class MenuMode : MonoBehaviour
	{
		public void Awake()
		{
			this.desktop.SetActive(!Dedicator.isVR);
			this.virtualReality.SetActive(Dedicator.isVR);
		}

		public GameObject desktop;

		public GameObject virtualReality;
	}
}
