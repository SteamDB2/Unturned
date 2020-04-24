using System;
using SDG.Framework.UI.Sleek2;
using UnityEngine;

namespace SDG.Framework.UI.Devkit.InspectorUI.TypeInspectors
{
	public class Sleek2ShortInspector : Sleek2KeyValueInspector
	{
		public Sleek2ShortInspector()
		{
			base.name = "Short_Inspector";
			this.field = new Sleek2ShortField();
			this.field.transform.reset();
			this.field.shortSubmitted += this.handleFieldSubmitted;
			base.valuePanel.addElement(this.field);
		}

		public Sleek2ShortField field { get; protected set; }

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
			this.field.value = (short)base.inspectable.value;
		}

		protected void handleFieldSubmitted(Sleek2ShortField field, short value)
		{
			base.inspectable.value = value;
		}
	}
}
