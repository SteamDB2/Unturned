using System;
using UnityEngine.UI;

namespace SDG.Framework.UI.Sleek2
{
	public class Sleek2Partition : Sleek2Element
	{
		public Sleek2Partition()
		{
			base.name = "Partition";
			this.image = base.gameObject.AddComponent<Image>();
			this.mask = base.gameObject.AddComponent<Mask>();
			this.mask.showMaskGraphic = false;
		}

		public Image image { get; protected set; }

		public Mask mask { get; protected set; }
	}
}
