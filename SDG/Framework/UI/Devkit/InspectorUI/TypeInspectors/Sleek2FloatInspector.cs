using System;
using SDG.Framework.UI.Sleek2;
using UnityEngine;

namespace SDG.Framework.UI.Devkit.InspectorUI.TypeInspectors
{
	public class Sleek2FloatInspector : Sleek2KeyValueInspector
	{
		public Sleek2FloatInspector()
		{
			base.name = "Float_Inspector";
			this.field = new Sleek2FloatField();
			this.field.transform.reset();
			this.field.floatSubmitted += this.handleFieldSubmitted;
			base.valuePanel.addElement(this.field);
		}

		public Sleek2FloatField field { get; protected set; }

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
			this.field.value = (float)base.inspectable.value;
		}

		protected void handleFieldSubmitted(Sleek2FloatField field, float value)
		{
			base.inspectable.value = value;
		}
	}
}
