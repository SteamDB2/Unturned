using System;
using SDG.Framework.UI.Sleek2;
using UnityEngine;

namespace SDG.Framework.UI.Devkit.InspectorUI.TypeInspectors
{
	public class Sleek2ByteInspector : Sleek2KeyValueInspector
	{
		public Sleek2ByteInspector()
		{
			base.name = "Byte_Inspector";
			this.field = new Sleek2ByteField();
			this.field.transform.reset();
			this.field.byteSubmitted += this.handleFieldSubmitted;
			base.valuePanel.addElement(this.field);
		}

		public Sleek2ByteField field { get; protected set; }

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
			this.field.value = (byte)base.inspectable.value;
		}

		protected void handleFieldSubmitted(Sleek2ByteField field, byte value)
		{
			base.inspectable.value = value;
		}
	}
}
