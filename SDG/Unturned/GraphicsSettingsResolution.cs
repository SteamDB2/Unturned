using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class GraphicsSettingsResolution
	{
		public GraphicsSettingsResolution(Resolution resolution)
		{
			this.Width = resolution.width;
			this.Height = resolution.height;
		}

		public GraphicsSettingsResolution() : this(Screen.resolutions[Screen.resolutions.Length - 1])
		{
		}

		public int Width { get; set; }

		public int Height { get; set; }
	}
}
