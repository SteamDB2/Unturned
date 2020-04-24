using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class Scrolling : MonoBehaviour
	{
		private void Update()
		{
			this.material.mainTextureOffset = new Vector2(this.x * Time.time, this.y * Time.time);
		}

		public Material material;

		public float x;

		public float y;
	}
}
