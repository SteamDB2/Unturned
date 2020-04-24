using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class BlueprintItemIconsInfo
	{
		public void onItemIconReady(Texture2D texture)
		{
			if (this.index >= this.textures.Length)
			{
				return;
			}
			this.textures[this.index] = texture;
			this.index++;
			if (this.index == this.textures.Length && this.callback != null)
			{
				this.callback();
			}
		}

		public Texture2D[] textures;

		public BlueprintItemIconsReady callback;

		private int index;
	}
}
