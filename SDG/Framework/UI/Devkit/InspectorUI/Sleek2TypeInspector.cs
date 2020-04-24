using System;
using SDG.Framework.UI.Sleek2;
using UnityEngine;
using UnityEngine.UI;

namespace SDG.Framework.UI.Devkit.InspectorUI
{
	public abstract class Sleek2TypeInspector : Sleek2Element
	{
		public Sleek2TypeInspector()
		{
			base.transform.anchorMin = new Vector2(0f, 1f);
			base.transform.anchorMax = new Vector2(1f, 1f);
			base.transform.pivot = new Vector2(0f, 1f);
			base.transform.offsetMin = new Vector2(0f, 0f);
			base.transform.offsetMax = new Vector2(0f, 0f);
			this.layoutComponent = base.gameObject.AddComponent<LayoutElement>();
		}

		public ObjectInspectableInfo inspectable { get; protected set; }

		public abstract void split(float value);

		public abstract void inspect(ObjectInspectableInfo newInspectable);

		public abstract void refresh();

		public LayoutElement layoutComponent;
	}
}
