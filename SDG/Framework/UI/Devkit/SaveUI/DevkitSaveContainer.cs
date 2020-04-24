using System;
using SDG.Framework.Devkit;
using SDG.Framework.Translations;
using SDG.Framework.UI.Sleek2;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.UI.Devkit.SaveUI
{
	public class DevkitSaveContainer : Sleek2PopoutContainer
	{
		public DevkitSaveContainer()
		{
			base.name = "Save";
			base.titlebar.titleLabel.translation = new TranslatedText(new TranslationReference("#SDG::Devkit.Window.Save.Title"));
			base.titlebar.titleLabel.translation.format();
			this.view = new Sleek2Scrollview();
			this.view.transform.reset();
			this.view.transform.offsetMin = new Vector2(5f, 5f);
			this.view.transform.offsetMax = new Vector2(-5f, -5f);
			this.view.vertical = true;
			this.list = new Sleek2VerticalScrollviewContents();
			this.list.name = "Panel";
			this.view.panel = this.list;
			base.bodyPanel.addElement(this.view);
			this.cancelButton = new Sleek2ImageButton();
			this.cancelButton.transform.anchorMin = new Vector2(0f, 0f);
			this.cancelButton.transform.anchorMax = new Vector2(0f, 0f);
			this.cancelButton.transform.pivot = new Vector2(0f, 0f);
			this.cancelButton.transform.sizeDelta = new Vector2(200f, (float)Sleek2Config.bodyHeight);
			this.cancelButton.clicked += this.handleCancelButtonClicked;
			base.bodyPanel.addElement(this.cancelButton);
			Sleek2TranslatedLabel sleek2TranslatedLabel = new Sleek2TranslatedLabel();
			sleek2TranslatedLabel.transform.reset();
			sleek2TranslatedLabel.translation = new TranslatedText(new TranslationReference("SDG", "Devkit.Window.Save.Cancel"));
			sleek2TranslatedLabel.translation.format();
			sleek2TranslatedLabel.textComponent.color = Sleek2Config.darkTextColor;
			this.cancelButton.addElement(sleek2TranslatedLabel);
			this.submitButton = new Sleek2ImageButton();
			this.submitButton.transform.anchorMin = new Vector2(1f, 0f);
			this.submitButton.transform.anchorMax = new Vector2(1f, 0f);
			this.submitButton.transform.pivot = new Vector2(1f, 0f);
			this.submitButton.transform.sizeDelta = new Vector2(200f, (float)Sleek2Config.bodyHeight);
			this.submitButton.clicked += this.handleSubmitButtonClicked;
			base.bodyPanel.addElement(this.submitButton);
			Sleek2TranslatedLabel sleek2TranslatedLabel2 = new Sleek2TranslatedLabel();
			sleek2TranslatedLabel2.transform.reset();
			sleek2TranslatedLabel2.translation = new TranslatedText(new TranslationReference("SDG", "Devkit.Window.Save.Submit"));
			sleek2TranslatedLabel2.translation.format();
			sleek2TranslatedLabel2.textComponent.color = Sleek2Config.darkTextColor;
			this.submitButton.addElement(sleek2TranslatedLabel2);
			this.updateList();
			DirtyManager.markedDirty += this.handleMarkedDirty;
			DirtyManager.markedClean += this.handleMarkedClean;
			DirtyManager.saved += this.handleSaved;
		}

		protected void updateList()
		{
			this.list.clearElements();
			foreach (IDirtyable newDirtyable in DirtyManager.dirty)
			{
				Sleek2Saveable sleek2Saveable = new Sleek2Saveable(newDirtyable);
				sleek2Saveable.transform.anchorMin = new Vector2(0f, 1f);
				sleek2Saveable.transform.anchorMax = new Vector2(1f, 1f);
				sleek2Saveable.transform.pivot = new Vector2(0.5f, 1f);
				sleek2Saveable.transform.sizeDelta = new Vector2(0f, (float)Sleek2Config.bodyHeight);
				this.list.addElement(sleek2Saveable);
			}
		}

		protected void handleCancelButtonClicked(Sleek2ImageButton button)
		{
			DevkitWindowManager.removeContainer(this);
		}

		protected void handleSubmitButtonClicked(Sleek2ImageButton button)
		{
			DirtyManager.save();
			DevkitWindowManager.removeContainer(this);
			if (Level.isLoaded && Level.isEditor)
			{
				Level.save();
			}
		}

		protected void handleMarkedDirty(IDirtyable item)
		{
			this.updateList();
		}

		protected void handleMarkedClean(IDirtyable item)
		{
			this.updateList();
		}

		protected void handleSaved()
		{
			this.updateList();
		}

		protected override void triggerDestroyed()
		{
			DirtyManager.markedDirty -= this.handleMarkedDirty;
			DirtyManager.markedClean -= this.handleMarkedClean;
			base.triggerDestroyed();
		}

		protected Sleek2Element list;

		protected Sleek2Scrollview view;

		protected Sleek2ImageButton cancelButton;

		protected Sleek2ImageButton submitButton;
	}
}
