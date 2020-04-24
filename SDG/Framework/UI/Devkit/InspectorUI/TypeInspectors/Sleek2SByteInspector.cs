using System;
using SDG.Framework.UI.Sleek2;
using UnityEngine;

namespace SDG.Framework.UI.Devkit.InspectorUI.TypeInspectors
{
	public class Sleek2SByteInspector : Sleek2KeyValueInspector
	{
		public Sleek2SByteInspector()
		{
			base.name = "SByte_Inspector";
			this.field = new Sleek2SByteField();
			this.field.transform.reset();
			this.field.sbyteSubmitted += this.handleFieldSubmitted;
			base.valuePanel.addElement(this.field);
		}

		public Sleek2SByteField field { get; protected set; }

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
			this.field.value = (sbyte)base.inspectable.value;
		}

		protected void handleFieldSubmitted(Sleek2SByteField field, sbyte value)
		{
			base.inspectable.value = value;
		}
	}
}
