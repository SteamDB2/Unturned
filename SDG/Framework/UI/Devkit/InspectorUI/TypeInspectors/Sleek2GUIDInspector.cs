using System;
using SDG.Framework.UI.Sleek2;
using UnityEngine;

namespace SDG.Framework.UI.Devkit.InspectorUI.TypeInspectors
{
	public class Sleek2GUIDInspector : Sleek2KeyValueInspector
	{
		public Sleek2GUIDInspector()
		{
			base.name = "GUID_Inspector";
			this.field = new Sleek2Field();
			this.field.transform.reset();
			this.field.submitted += this.handleFieldSubmitted;
			base.valuePanel.addElement(this.field);
		}

		public Sleek2Field field { get; protected set; }

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
			this.field.fieldComponent.text = ((Guid)base.inspectable.value).ToString("N");
		}

		protected void handleFieldSubmitted(Sleek2Field field, string value)
		{
			base.inspectable.value = new Guid(value);
		}
	}
}
