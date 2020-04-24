using System;

namespace SDG.Framework.UI.Devkit.InspectorUI.TypeInspectors
{
	public class Sleek2DropdownEnumToolbarButton : Sleek2ToolbarLabelButton
	{
		public Sleek2DropdownEnumToolbarButton(string newEnumName, int newEnumValue)
		{
			this.enumName = newEnumName;
			this.enumValue = newEnumValue;
		}

		public string enumName { get; protected set; }

		public int enumValue { get; protected set; }

		public void refresh(bool isFlagged)
		{
			base.label.textComponent.text = ((!isFlagged) ? this.enumName : ("[[ " + this.enumName + " ]]"));
		}
	}
}
