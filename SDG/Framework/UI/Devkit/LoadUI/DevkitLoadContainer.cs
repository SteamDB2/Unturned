using System;
using SDG.Framework.Translations;
using SDG.Framework.UI.Sleek2;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.UI.Devkit.LoadUI
{
	public class DevkitLoadContainer : Sleek2PopoutContainer
	{
		public DevkitLoadContainer()
		{
			base.name = "Load";
			this.selectedLevelInfo = null;
			base.titlebar.titleLabel.translation = new TranslatedText(new TranslationReference("#SDG::Devkit.Window.Load.Title"));
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
			sleek2TranslatedLabel.translation = new TranslatedText(new TranslationReference("SDG", "Devkit.Window.Load.Cancel"));
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
			sleek2TranslatedLabel2.translation = new TranslatedText(new TranslationReference("SDG", "Devkit.Window.Load.Submit"));
			sleek2TranslatedLabel2.translation.format();
			sleek2TranslatedLabel2.textComponent.color = Sleek2Config.darkTextColor;
			this.submitButton.addElement(sleek2TranslatedLabel2);
			this.updateList();
			Level.onLevelsRefreshed = (LevelsRefreshed)Delegate.Combine(Level.onLevelsRefreshed, new LevelsRefreshed(this.onLevelsRefreshed));
		}

		protected void updateList()
		{
			this.list.clearElements();
			LevelInfo[] levels = Level.getLevels(ESingleplayerMapCategory.EDITABLE);
			foreach (LevelInfo newLevelInfo in levels)
			{
				Sleek2Loadable sleek2Loadable = new Sleek2Loadable(newLevelInfo);
				sleek2Loadable.transform.anchorMin = new Vector2(0f, 1f);
				sleek2Loadable.transform.anchorMax = new Vector2(1f, 1f);
				sleek2Loadable.transform.pivot = new Vector2(0.5f, 1f);
				sleek2Loadable.transform.sizeDelta = new Vector2(0f, (float)Sleek2Config.bodyHeight);
				sleek2Loadable.clicked += this.handleLoadableClicked;
				this.list.addElement(sleek2Loadable);
			}
		}

		protected virtual void handleLoadableClicked(Sleek2ImageButton button)
		{
			this.selectedLevelInfo = (button as Sleek2Loadable).levelInfo;
		}

		protected virtual void handleCancelButtonClicked(Sleek2ImageButton button)
		{
			DevkitWindowManager.removeContainer(this);
		}

		protected virtual void handleSubmitButtonClicked(Sleek2ImageButton button)
		{
			Level.edit(this.selectedLevelInfo, true);
			DevkitWindowManager.removeContainer(this);
		}

		protected virtual void onLevelsRefreshed()
		{
			this.updateList();
		}

		protected override void triggerDestroyed()
		{
			Level.onLevelsRefreshed = (LevelsRefreshed)Delegate.Remove(Level.onLevelsRefreshed, new LevelsRefreshed(this.onLevelsRefreshed));
			base.triggerDestroyed();
		}

		protected Sleek2Element list;

		protected Sleek2Scrollview view;

		protected Sleek2ImageButton cancelButton;

		protected Sleek2ImageButton submitButton;

		protected LevelInfo selectedLevelInfo;
	}
}
