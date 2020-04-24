using System;
using UnityEngine.UI;

namespace SDG.Framework.UI.Sleek2
{
	public class Sleek2Mask : Sleek2Element
	{
		public Sleek2Mask()
		{
			base.gameObject.name = "Mask";
			this.maskComponent = base.gameObject.AddComponent<Mask>();
			this.maskComponent.showMaskGraphic = false;
			this.imageComponent = base.gameObject.AddComponent<Image>();
		}

		public Mask maskComponent { get; protected set; }

		public Image imageComponent { get; protected set; }
	}
}
