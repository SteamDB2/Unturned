using System;
using SDG.Framework.UI.Sleek2;
using UnityEngine;

namespace SDG.Framework.UI.Devkit.InspectorUI.TypeInspectors
{
	public class Sleek2IntInspector : Sleek2KeyValueInspector
	{
		public Sleek2IntInspector()
		{
			base.name = "Int_Inspector";
			this.field = new Sleek2IntField();
			this.field.transform.reset();
			this.field.intSubmitted += this.handleFieldSubmitted;
			base.valuePanel.addElement(this.field);
		}

		public Sleek2IntField field { get; protected set; }

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
			this.field.value = (int)base.inspectable.value;
		}

		protected void handleFieldSubmitted(Sleek2IntField field, int value)
		{
			base.inspectable.value = value;
		}
	}
}
