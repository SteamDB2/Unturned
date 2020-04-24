using System;
using SDG.Framework.UI.Sleek2;
using UnityEngine;

namespace SDG.Framework.UI.Devkit.InspectorUI.TypeInspectors
{
	public class Sleek2BoolInspector : Sleek2KeyValueInspector
	{
		public Sleek2BoolInspector()
		{
			base.name = "Bool_Inspector";
			this.toggle = new Sleek2Toggle();
			this.toggle.transform.anchorMin = new Vector2(0f, 0f);
			this.toggle.transform.anchorMax = new Vector2(0f, 0f);
			this.toggle.transform.pivot = new Vector2(0f, 0f);
			this.toggle.transform.sizeDelta = new Vector2(30f, 30f);
			this.toggle.toggled += this.handleToggleToggled;
			base.valuePanel.addElement(this.toggle);
		}

		public Sleek2Toggle toggle { get; protected set; }

		public override void inspect(ObjectInspectableInfo newInspectable)
		{
			base.inspect(newInspectable);
			if (base.inspectable == null)
			{
				return;
			}
			this.toggle.toggleComponent.interactable = base.inspectable.canWrite;
		}

		public override void refresh()
		{
			if (base.inspectable == null || !base.inspectable.canRead)
			{
				return;
			}
			this.toggle.toggleComponent.isOn = (bool)base.inspectable.value;
		}

		protected void handleToggleToggled(Sleek2Toggle toggle, bool isOn)
		{
			base.inspectable.value = isOn;
		}
	}
}
