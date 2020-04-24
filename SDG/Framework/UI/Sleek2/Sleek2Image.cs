using System;
using UnityEngine.UI;

namespace SDG.Framework.UI.Sleek2
{
	public class Sleek2Image : Sleek2Element
	{
		public Sleek2Image()
		{
			base.name = "Image";
			this.imageComponent = base.gameObject.AddComponent<Image>();
		}

		public Image imageComponent { get; protected set; }
	}
}
