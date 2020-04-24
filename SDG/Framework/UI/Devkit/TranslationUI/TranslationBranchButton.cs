using System;
using SDG.Framework.Translations;
using SDG.Framework.UI.Sleek2;
using UnityEngine;
using UnityEngine.UI;

namespace SDG.Framework.UI.Devkit.TranslationUI
{
	public class TranslationBranchButton : Sleek2Element
	{
		public TranslationBranchButton(TranslationBranch newBranch)
		{
			base.name = "Branch";
			this.branch = newBranch;
			this.branch.keyChanged += this.handleKeyChanged;
			this.climbButton = new Sleek2ImageLabelButton();
			this.climbButton.transform.anchorMin = new Vector2(0f, 0f);
			this.climbButton.transform.anchorMax = new Vector2(0.5f, 1f);
			this.climbButton.transform.offsetMin = new Vector2(0f, 0f);
			this.climbButton.transform.offsetMax = new Vector2(0f, 0f);
			this.addElement(this.climbButton);
			this.refreshClimbButton();
			this.keyField = new Sleek2Field();
			this.keyField.transform.anchorMin = new Vector2(0.5f, 0f);
			this.keyField.transform.anchorMax = new Vector2(1f, 1f);
			this.keyField.transform.offsetMin = new Vector2(0f, 0f);
			this.keyField.transform.offsetMax = new Vector2(0f, 0f);
			this.keyField.text = this.branch.key;
			this.keyField.submitted += this.handleKeySubmitted;
			this.addElement(this.keyField);
			this.layoutComponent = base.gameObject.AddComponent<LayoutElement>();
			this.layoutComponent.preferredHeight = (float)Sleek2Config.bodyHeight;
		}

		public TranslationBranch branch { get; protected set; }

		public LayoutElement layoutComponent { get; protected set; }

		public Sleek2ImageLabelButton climbButton { get; protected set; }

		public Sleek2Field keyField { get; protected set; }

		protected virtual void refreshClimbButton()
		{
			this.climbButton.label.textComponent.text = this.branch.getReferenceTo().ToString();
		}

		protected virtual void handleKeyChanged(TranslationBranch branch, string oldKey, string newKey)
		{
			this.refreshClimbButton();
		}

		protected virtual void handleKeySubmitted(Sleek2Field field, string value)
		{
			this.branch.key = value;
		}

		protected override void triggerDestroyed()
		{
			this.branch.keyChanged -= this.handleKeyChanged;
			base.triggerDestroyed();
		}
	}
}
