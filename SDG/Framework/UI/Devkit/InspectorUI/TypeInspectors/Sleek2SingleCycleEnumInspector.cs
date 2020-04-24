using System;
using SDG.Framework.UI.Sleek2;
using UnityEngine;

namespace SDG.Framework.UI.Devkit.InspectorUI.TypeInspectors
{
	public class Sleek2SingleCycleEnumInspector : Sleek2KeyValueInspector
	{
		public Sleek2SingleCycleEnumInspector()
		{
			base.name = "Single_Cycle_Enum_Inspector";
			this.button = new Sleek2ImageButton();
			this.button.transform.reset();
			this.button.clicked += this.handleButtonClicked;
			base.valuePanel.addElement(this.button);
			this.label = new Sleek2Label();
			this.label.transform.reset();
			this.label.textComponent.color = Sleek2Config.darkTextColor;
			this.button.addElement(this.label);
		}

		public Sleek2ImageButton button { get; protected set; }

		public Sleek2Label label { get; protected set; }

		public override void inspect(ObjectInspectableInfo newInspectable)
		{
			base.inspect(newInspectable);
			if (base.inspectable == null)
			{
				return;
			}
			this.button.buttonComponent.interactable = base.inspectable.canWrite;
		}

		public override void refresh()
		{
			if (base.inspectable == null || !base.inspectable.canRead)
			{
				return;
			}
			this.label.textComponent.text = base.inspectable.value.ToString();
		}

		protected void handleButtonClicked(Sleek2ImageButton button)
		{
			int num = (int)base.inspectable.value;
			num++;
			if (num >= Enum.GetValues(base.inspectable.type).Length)
			{
				num = 0;
			}
			base.inspectable.value = num;
		}
	}
}
