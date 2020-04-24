using System;
using SDG.Framework.UI.Sleek2;
using UnityEngine;

namespace SDG.Framework.UI.Devkit.InspectorUI.TypeInspectors
{
	public class Sleek2ULongInspector : Sleek2KeyValueInspector
	{
		public Sleek2ULongInspector()
		{
			base.name = "ULong_Inspector";
			this.field = new Sleek2ULongField();
			this.field.transform.reset();
			this.field.ulongSubmitted += this.handleFieldSubmitted;
			base.valuePanel.addElement(this.field);
		}

		public Sleek2ULongField field { get; protected set; }

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
			this.field.value = (ulong)base.inspectable.value;
		}

		protected void handleFieldSubmitted(Sleek2ULongField field, ulong value)
		{
			base.inspectable.value = value;
		}
	}
}
