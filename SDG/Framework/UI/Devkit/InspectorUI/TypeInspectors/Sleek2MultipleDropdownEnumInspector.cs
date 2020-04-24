using System;
using SDG.Framework.UI.Sleek2;
using UnityEngine;

namespace SDG.Framework.UI.Devkit.InspectorUI.TypeInspectors
{
	public class Sleek2MultipleDropdownEnumInspector : Sleek2KeyValueInspector
	{
		public Sleek2MultipleDropdownEnumInspector()
		{
			base.name = "Multiple_Dropdown_Enum_Inspector";
			this.button = new Sleek2ToolbarButton();
			this.button.transform.reset();
			base.valuePanel.addElement(this.button);
			this.label = new Sleek2Label();
			this.label.transform.reset();
			this.label.textComponent.color = Sleek2Config.darkTextColor;
			this.button.addElement(this.label);
		}

		public Sleek2ToolbarButton button { get; protected set; }

		public Sleek2Label label { get; protected set; }

		public override void inspect(ObjectInspectableInfo newInspectable)
		{
			base.inspect(newInspectable);
			if (base.inspectable == null)
			{
				return;
			}
			string[] names = Enum.GetNames(base.inspectable.type);
			Array values = Enum.GetValues(base.inspectable.type);
			this.enumButtons = new Sleek2DropdownEnumToolbarButton[names.Length];
			for (int i = 0; i < names.Length; i++)
			{
				Sleek2DropdownEnumToolbarButton sleek2DropdownEnumToolbarButton = new Sleek2DropdownEnumToolbarButton(names[i], (int)values.GetValue(i));
				sleek2DropdownEnumToolbarButton.clicked += this.handleEnumButtonClicked;
				this.button.panel.addElement(sleek2DropdownEnumToolbarButton);
				this.enumButtons[i] = sleek2DropdownEnumToolbarButton;
			}
		}

		public override void refresh()
		{
			if (base.inspectable == null || !base.inspectable.canRead)
			{
				return;
			}
			this.button.label.textComponent.text = base.inspectable.value.ToString();
			if (this.enumButtons == null)
			{
				return;
			}
			int num = (int)base.inspectable.value;
			for (int i = 0; i < this.enumButtons.Length; i++)
			{
				int enumValue = this.enumButtons[i].enumValue;
				this.enumButtons[i].refresh((num & enumValue) == enumValue);
			}
		}

		protected virtual void handleEnumButtonClicked(Sleek2ImageButton button)
		{
			int enumValue = (button as Sleek2DropdownEnumToolbarButton).enumValue;
			int num = (int)base.inspectable.value;
			if ((num & enumValue) == enumValue)
			{
				num &= ~enumValue;
			}
			else
			{
				num |= enumValue;
			}
			base.inspectable.value = num;
		}

		protected Sleek2DropdownEnumToolbarButton[] enumButtons;
	}
}
