using System;
using SDG.Framework.UI.Sleek2;
using UnityEngine;

namespace SDG.Framework.UI.Devkit.InspectorUI.TypeInspectors
{
	public class Sleek2UIntInspector : Sleek2KeyValueInspector
	{
		public Sleek2UIntInspector()
		{
			base.name = "UInt_Inspector";
			this.field = new Sleek2UIntField();
			this.field.transform.reset();
			this.field.uintSubmitted += this.handleFieldSubmitted;
			base.valuePanel.addElement(this.field);
		}

		public Sleek2UIntField field { get; protected set; }

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
			this.field.value = (uint)base.inspectable.value;
		}

		protected void handleFieldSubmitted(Sleek2UIntField field, uint value)
		{
			base.inspectable.value = value;
		}
	}
}
