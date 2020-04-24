using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class Messager : MonoBehaviour
	{
		private void OnTriggerStay(Collider other)
		{
			if (!Dedicator.isDedicated && other.transform.CompareTag("Player"))
			{
				this.lastTrigger = Time.realtimeSinceStartup;
			}
		}

		private void Update()
		{
			if (Time.realtimeSinceStartup - this.lastTrigger < 0.5f)
			{
				PlayerUI.hint(null, this.message);
			}
		}

		public EPlayerMessage message;

		private float lastTrigger;
	}
}
