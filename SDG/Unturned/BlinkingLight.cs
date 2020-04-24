using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class BlinkingLight : MonoBehaviour
	{
		private void Update()
		{
			if (Time.time - this.blinkTime < 1f)
			{
				return;
			}
			this.blinkTime = Time.time;
			this.target.SetActive(!this.target.activeSelf);
		}

		public GameObject target;

		private float blinkTime;
	}
}
