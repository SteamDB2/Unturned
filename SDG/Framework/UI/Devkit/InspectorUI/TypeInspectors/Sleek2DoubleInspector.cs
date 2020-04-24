using System;
using SDG.Framework.UI.Sleek2;
using UnityEngine;

namespace SDG.Framework.UI.Devkit.InspectorUI.TypeInspectors
{
	public class Sleek2DoubleInspector : Sleek2KeyValueInspector
	{
		public Sleek2DoubleInspector()
		{
			base.name = "Double_Inspector";
			this.field = new Sleek2DoubleField();
			this.field.transform.reset();
			this.field.doubleSubmitted += this.handleFieldSubmitted;
			base.valuePanel.addElement(this.field);
		}

		public Sleek2DoubleField field { get; protected set; }

		public override void inspect(ObjectInspectableInfo newInspectable)
		{
			base.inspect(newInspectable);
			if (base.inspectable == null)
			{
				return;
			}
			this.field.fieldComponent.interactable = base.inspectable.canWrite;
		}

		public override void refresh()
		{
			if (base.inspectable == null || !base.inspectable.canRead)
			{
				return;
			}
			if (this.field.fieldComponent.isFocused)
			{
				return;
			}
			this.field.value = (double)base.inspectable.value;
		}

		protected void handleFieldSubmitted(Sleek2DoubleField field, double value)
		{
			base.inspectable.value = value;
		}
	}
}
