using System;
using SDG.Framework.UI.Sleek2;
using UnityEngine;

namespace SDG.Framework.UI.Devkit.InspectorUI.TypeInspectors
{
	public class Sleek2UShortInspector : Sleek2KeyValueInspector
	{
		public Sleek2UShortInspector()
		{
			base.name = "UShort_Inspector";
			this.field = new Sleek2UShortField();
			this.field.transform.reset();
			this.field.ushortSubmitted += this.handleFieldSubmitted;
			base.valuePanel.addElement(this.field);
		}

		public Sleek2UShortField field { get; protected set; }

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
			this.field.value = (ushort)base.inspectable.value;
		}

		protected void handleFieldSubmitted(Sleek2UShortField field, ushort value)
		{
			base.inspectable.value = value;
		}
	}
}
