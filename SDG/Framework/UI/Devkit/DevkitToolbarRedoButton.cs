using System;
using SDG.Framework.Devkit.Transactions;
using SDG.Framework.Translations;
using SDG.Framework.UI.Sleek2;
using UnityEngine;

namespace SDG.Framework.UI.Devkit
{
	public class DevkitToolbarRedoButton : Sleek2ImageButton
	{
		public DevkitToolbarRedoButton()
		{
			base.transform.sizeDelta = new Vector2(0f, (float)Sleek2Config.bodyHeight);
			base.imageComponent.sprite = Resources.Load<Sprite>("Sprites/UI/Hover_Background");
			this.label = new Sleek2TranslatedLabel();
			this.label.transform.reset();
			this.label.translation = new TranslatedText(new TranslationReference("SDG", "Devkit.Toolbar.Edit.Redo"));
			this.label.translation.format();
			this.addElement(this.label);
		}

		public Sleek2TranslatedLabel label { get; protected set; }

		protected override void handleButtonClick()
		{
			DevkitTransactionManager.redo();
			base.handleButtonClick();
		}
	}
}
