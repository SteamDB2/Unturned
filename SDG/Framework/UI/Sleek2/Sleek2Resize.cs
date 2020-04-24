using System;
using SDG.Framework.UI.Components;
using UnityEngine.UI;

namespace SDG.Framework.UI.Sleek2
{
	public class Sleek2Resize : Sleek2Element
	{
		public Sleek2Resize()
		{
			base.gameObject.name = "Resize";
			this.handle = base.gameObject.AddComponent<ResizeHandle>();
			this.image = base.gameObject.AddComponent<Image>();
			base.gameObject.AddComponent<Selectable>();
		}

		public ResizeHandle handle { get; protected set; }

		public Image image { get; protected set; }
	}
}
