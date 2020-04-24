using System;
using SDG.Framework.Translations;
using SDG.Framework.UI.Sleek2;
using UnityEngine;

namespace SDG.Framework.UI.Devkit.LayoutUI
{
	public class LayoutWindow : Sleek2Window
	{
		public LayoutWindow()
		{
			base.name = "Layouts";
			base.tab.label.translation = new TranslatedText(new TranslationReference("SDG", "Devkit.Window.Layout.Title"));
			base.tab.label.translation.format();
			this.nameField = new Sleek2Field();
			this.nameField.transform.anchorMin = new Vector2(0f, 1f);
			this.nameField.transform.anchorMax = new Vector2(1f, 1f);
			this.nameField.transform.pivot = new Vector2(0.5f, 1f);
			this.nameField.transform.offsetMin = new Vector2(5f, -35f);
			this.nameField.transform.offsetMax = new Vector2(-5f, -5f);
			this.addElement(this.nameField);
			this.loadButton = new Sleek2ImageButton();
			this.loadButton.transform.anchorMin = new Vector2(0f, 1f);
			this.loadButton.transform.anchorMax = new Vector2(0.5f, 1f);
			this.loadButton.transform.pivot = new Vector2(0.5f, 1f);
			this.loadButton.transform.offsetMin = new Vector2(5f, -75f);
			this.loadButton.transform.offsetMax = new Vector2(-5f, -45f);
			this.loadButton.clicked += this.handleLoadClicked;
			this.addElement(this.loadButton);
			Sleek2TranslatedLabel sleek2TranslatedLabel = new Sleek2TranslatedLabel();
			sleek2TranslatedLabel.transform.reset();
			sleek2TranslatedLabel.textComponent.color = Sleek2Config.darkTextColor;
			sleek2TranslatedLabel.translation = new TranslatedText(new TranslationReference("SDG", "Devkit.Window.Layout.Load"));
			sleek2TranslatedLabel.translation.format();
			this.loadButton.addElement(sleek2TranslatedLabel);
			this.saveButton = new Sleek2ImageButton();
			this.saveButton.transform.anchorMin = new Vector2(0.5f, 1f);
			this.saveButton.transform.anchorMax = new Vector2(1f, 1f);
			this.saveButton.transform.pivot = new Vector2(0.5f, 1f);
			this.saveButton.transform.offsetMin = new Vector2(5f, -75f);
			this.saveButton.transform.offsetMax = new Vector2(-5f, -45f);
			this.saveButton.clicked += this.handleSaveClicked;
			this.addElement(this.saveButton);
			Sleek2TranslatedLabel sleek2TranslatedLabel2 = new Sleek2TranslatedLabel();
			sleek2TranslatedLabel2.transform.reset();
			sleek2TranslatedLabel2.textComponent.color = Sleek2Config.darkTextColor;
			sleek2TranslatedLabel2.translation = new TranslatedText(new TranslationReference("SDG", "Devkit.Window.Layout.Save"));
			sleek2TranslatedLabel2.translation.format();
			this.saveButton.addElement(sleek2TranslatedLabel2);
		}

		public Sleek2Field nameField { get; protected set; }

		public Sleek2ImageButton loadButton { get; protected set; }

		public Sleek2ImageButton saveButton { get; protected set; }

		protected virtual void handleLoadClicked(Sleek2ImageButton button)
		{
			DevkitWindowLayout.load(this.nameField.fieldComponent.text);
		}

		protected virtual void handleSaveClicked(Sleek2ImageButton button)
		{
			DevkitWindowLayout.save(this.nameField.fieldComponent.text);
		}
	}
}
