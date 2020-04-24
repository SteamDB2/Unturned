using System;
using SDG.Framework.UI.Sleek2;
using UnityEngine;

namespace SDG.Framework.UI.Devkit.InspectorUI.TypeInspectors
{
	public class Sleek2LongInspector : Sleek2KeyValueInspector
	{
		public Sleek2LongInspector()
		{
			base.name = "Long_Inspector";
			this.field = new Sleek2LongField();
			this.field.transform.reset();
			this.field.longSubmitted += this.handleFieldSubmitted;
			base.valuePanel.addElement(this.field);
		}

		public Sleek2LongField field { get; protected set; }

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
			this.field.value = (long)base.inspectable.value;
		}

		protected void handleFieldSubmitted(Sleek2LongField field, long value)
		{
			base.inspectable.value = value;
		}
	}
}
