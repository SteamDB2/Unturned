using System;
using SDG.Framework.Devkit.Transactions;
using SDG.Framework.Translations;
using SDG.Framework.UI.Sleek2;
using UnityEngine;
using UnityEngine.UI;

namespace SDG.Framework.UI.Devkit.TranslationUI
{
	public class TranslationLeafUpdateField : Sleek2Element
	{
		public TranslationLeafUpdateField(TranslationLeaf newOriginLeaf, TranslationLeaf newTranslationLeaf)
		{
			base.name = "Leaf_Update";
			this.originLeaf = newOriginLeaf;
			this.translationLeaf = newTranslationLeaf;
			this.climbButton = new Sleek2ImageLabelButton();
			this.climbButton.transform.anchorMin = new Vector2(0f, 0f);
			this.climbButton.transform.anchorMax = new Vector2(1f, 0f);
			this.climbButton.transform.pivot = new Vector2(0f, 1f);
			this.climbButton.transform.offsetMin = new Vector2(0f, (float)(Sleek2Config.bodyHeight * 2));
			this.climbButton.transform.offsetMax = new Vector2(0f, (float)(Sleek2Config.bodyHeight * 3));
			this.addElement(this.climbButton);
			this.originField = new Sleek2Field();
			this.originField.transform.anchorMin = new Vector2(0f, 0f);
			this.originField.transform.anchorMax = new Vector2(1f, 0f);
			this.originField.transform.pivot = new Vector2(0f, 1f);
			this.originField.transform.offsetMin = new Vector2(0f, (float)Sleek2Config.bodyHeight);
			this.originField.transform.offsetMax = new Vector2(0f, (float)(Sleek2Config.bodyHeight * 2));
			this.originField.fieldComponent.interactable = false;
			this.addElement(this.originField);
			this.translationField = new Sleek2Field();
			this.translationField.transform.anchorMin = new Vector2(0f, 0f);
			this.translationField.transform.anchorMax = new Vector2(1f, 0f);
			this.translationField.transform.pivot = new Vector2(0f, 1f);
			this.translationField.transform.offsetMin = new Vector2(0f, 0f);
			this.translationField.transform.offsetMax = new Vector2(0f, (float)Sleek2Config.bodyHeight);
			this.translationField.submitted += this.handleTextSubmitted;
			this.addElement(this.translationField);
			this.refreshFields();
			this.layoutComponent = base.gameObject.AddComponent<LayoutElement>();
			this.layoutComponent.preferredHeight = (float)(Sleek2Config.bodyHeight * 3);
		}

		public TranslationLeaf originLeaf { get; protected set; }

		public TranslationLeaf translationLeaf { get; protected set; }

		public Sleek2ImageLabelButton climbButton { get; protected set; }

		public Sleek2Field originField { get; protected set; }

		public Sleek2Field translationField { get; protected set; }

		public LayoutElement layoutComponent { get; protected set; }

		protected virtual void refreshFields()
		{
			if (this.originLeaf != null)
			{
				this.climbButton.label.textComponent.text = this.originLeaf.getReferenceTo().ToString();
				this.originField.text = this.originLeaf.text;
			}
			if (this.translationLeaf != null)
			{
				this.translationField.text = this.translationLeaf.text;
			}
		}

		protected virtual void handleTextSubmitted(Sleek2Field field, string value)
		{
			if (this.originLeaf != null && this.translationLeaf != null)
			{
				DevkitTransactionManager.beginTransaction(new TranslatedText(new TranslationReference("#SDG::Devkit.Transactions.Translation")));
				DevkitTransactionUtility.recordObjectDelta(this.translationLeaf);
				DevkitTransactionUtility.recordObjectDelta(this.translationLeaf.translation);
				this.translationLeaf.text = value;
				this.translationLeaf.version = this.originLeaf.version;
				this.translationLeaf.translation.isDirty = true;
				DevkitTransactionManager.endTransaction();
			}
		}
	}
}
