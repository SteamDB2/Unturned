using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class Snow : MonoBehaviour
	{
		private void Update()
		{
			if (Dedicator.isDedicated)
			{
				return;
			}
			if (this._Snow_Sparkle_Map != -1)
			{
				Shader.SetGlobalTexture(this._Snow_Sparkle_Map, this.Sparkle_Map);
			}
		}

		private void OnEnable()
		{
			if (Dedicator.isDedicated)
			{
				return;
			}
			if (this._Snow_Sparkle_Map == -1)
			{
				this._Snow_Sparkle_Map = Shader.PropertyToID("_Snow_Sparkle_Map");
			}
		}

		private int _Snow_Sparkle_Map = -1;

		public Texture2D Sparkle_Map;
	}
}
