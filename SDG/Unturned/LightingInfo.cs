using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class LightingInfo
	{
		public LightingInfo(Color[] newColors, float[] newSingles)
		{
			this._colors = newColors;
			this._singles = newSingles;
		}

		public Color[] colors
		{
			get
			{
				return this._colors;
			}
		}

		public float[] singles
		{
			get
			{
				return this._singles;
			}
		}

		private Color[] _colors;

		private float[] _singles;
	}
}
