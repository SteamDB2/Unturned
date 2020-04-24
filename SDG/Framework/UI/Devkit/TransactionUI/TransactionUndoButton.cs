using System;
using SDG.Framework.Devkit.Transactions;
using SDG.Framework.UI.Sleek2;
using UnityEngine;

namespace SDG.Framework.UI.Devkit.TransactionUI
{
	public class TransactionUndoButton : Sleek2ImageButton
	{
		public TransactionUndoButton(DevkitTransactionGroup newGroup)
		{
			this.group = newGroup;
			Sleek2TranslatedLabel sleek2TranslatedLabel = new Sleek2TranslatedLabel();
			sleek2TranslatedLabel.transform.reset();
			sleek2TranslatedLabel.translation = this.group.name;
			sleek2TranslatedLabel.translation.format();
			sleek2TranslatedLabel.textComponent.color = Sleek2Config.darkTextColor;
			this.addElement(sleek2TranslatedLabel);
		}

		public DevkitTransactionGroup group;
	}
}
