using System;
using SDG.Framework.Translations;
using SDG.Framework.UI.Sleek2;
using UnityEngine;

namespace SDG.Framework.UI.Devkit.TranslationUI
{
	public class TranslationLeafMissField : TranslationLeafField
	{
		public TranslationLeafMissField(TranslationReference newReference) : base(null)
		{
			base.name = "Leaf_Miss";
			this.reference = newReference;
			base.climbButton.isVisible = false;
			base.keyField.isVisible = false;
			base.textField.isVisible = false;
			base.versionField.isVisible = false;
			this.addButton = new Sleek2ImageLabelButton();
			this.addButton.transform.anchorMin = new Vector2(0f, 0f);
			this.addButton.transform.anchorMax = new Vector2(0f, 1f);
			this.addButton.transform.offsetMin = new Vector2(0f, 0f);
			this.addButton.transform.offsetMax = new Vector2((float)(Sleek2Config.bodyHeight * 2), 0f);
			this.addButton.label.textComponent.text = "+";
			this.addButton.clicked += this.handleAddButtonClicked;
			this.addElement(this.addButton);
			this.referenceButton = new Sleek2ImageLabelButton();
			this.referenceButton.transform.anchorMin = new Vector2(0f, 0f);
			this.referenceButton.transform.anchorMax = new Vector2(1f, 1f);
			this.referenceButton.transform.offsetMin = new Vector2((float)(Sleek2Config.bodyHeight * 2), 0f);
			this.referenceButton.transform.offsetMax = new Vector2(0f, 0f);
			this.referenceButton.label.textComponent.text = this.reference.ToString();
			this.addElement(this.referenceButton);
		}

		public TranslationReference reference { get; protected set; }

		public Sleek2ImageLabelButton addButton { get; protected set; }

		public Sleek2ImageLabelButton referenceButton { get; protected set; }

		protected virtual void handleAddButtonClicked(Sleek2ImageButton button)
		{
			base.leaf = Translator.addLeaf(this.reference);
			if (base.leaf == null)
			{
				return;
			}
			base.leaf.branch.keyChanged += this.handleKeyChanged;
			base.leaf.versionChanged += this.handleVersionChanged;
			base.leaf.translation.isDirty = true;
			base.climbButton.isVisible = true;
			base.keyField.isVisible = true;
			base.textField.isVisible = true;
			base.versionField.isVisible = true;
			this.addButton.isVisible = false;
			this.referenceButton.isVisible = false;
			this.refreshKey();
			this.refreshFields();
			this.refreshVersion();
		}
	}
}
