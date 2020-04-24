using System;
using SDG.Framework.Devkit.Transactions;
using SDG.Framework.Translations;
using SDG.Framework.UI.Sleek2;
using UnityEngine;
using UnityEngine.UI;

namespace SDG.Framework.UI.Devkit.TranslationUI
{
	public class TranslationLeafField : Sleek2Element
	{
		public TranslationLeafField(TranslationLeaf newLeaf)
		{
			base.name = "Leaf";
			this.leaf = newLeaf;
			if (this.leaf != null)
			{
				this.leaf.branch.keyChanged += this.handleKeyChanged;
				this.leaf.versionChanged += this.handleVersionChanged;
			}
			this.climbButton = new Sleek2ImageLabelButton();
			this.climbButton.transform.anchorMin = new Vector2(0f, 0.5f);
			this.climbButton.transform.anchorMax = new Vector2(0.5f, 1f);
			this.climbButton.transform.offsetMin = new Vector2(0f, 0f);
			this.climbButton.transform.offsetMax = new Vector2(0f, 0f);
			this.addElement(this.climbButton);
			this.keyField = new Sleek2Field();
			this.keyField.transform.anchorMin = new Vector2(0.5f, 0.5f);
			this.keyField.transform.anchorMax = new Vector2(1f, 1f);
			this.keyField.transform.offsetMin = new Vector2(0f, 0f);
			this.keyField.transform.offsetMax = new Vector2(0f, 0f);
			this.keyField.submitted += this.handleKeySubmitted;
			this.addElement(this.keyField);
			this.textField = new Sleek2Field();
			this.textField.transform.anchorMin = new Vector2(0f, 0f);
			this.textField.transform.anchorMax = new Vector2(1f, 0.5f);
			this.textField.transform.offsetMin = new Vector2(0f, 0f);
			this.textField.transform.offsetMax = new Vector2(-40f, 0f);
			this.textField.submitted += this.handleTextSubmitted;
			this.addElement(this.textField);
			this.versionField = new Sleek2IntField();
			this.versionField.transform.anchorMin = new Vector2(1f, 0f);
			this.versionField.transform.anchorMax = new Vector2(1f, 0.5f);
			this.versionField.transform.offsetMin = new Vector2(-40f, 0f);
			this.versionField.transform.offsetMax = new Vector2(0f, 0f);
			this.versionField.fieldComponent.interactable = false;
			this.addElement(this.versionField);
			this.refreshKey();
			this.refreshFields();
			this.refreshVersion();
			this.layoutComponent = base.gameObject.AddComponent<LayoutElement>();
			this.layoutComponent.preferredHeight = (float)(Sleek2Config.bodyHeight * 2);
		}

		public TranslationLeaf leaf { get; protected set; }

		public Sleek2ImageLabelButton climbButton { get; protected set; }

		public Sleek2Field keyField { get; protected set; }

		public Sleek2Field textField { get; protected set; }

		public Sleek2IntField versionField { get; protected set; }

		public LayoutElement layoutComponent { get; protected set; }

		protected virtual void refreshKey()
		{
			if (this.leaf != null)
			{
				this.climbButton.label.textComponent.text = this.leaf.getReferenceTo().ToString();
			}
		}

		protected virtual void refreshFields()
		{
			if (this.leaf != null)
			{
				this.keyField.text = this.leaf.branch.key;
				this.textField.text = this.leaf.text;
			}
		}

		protected virtual void refreshVersion()
		{
			if (this.leaf != null)
			{
				this.versionField.value = this.leaf.version;
			}
		}

		protected virtual void handleKeyChanged(TranslationBranch branch, string oldKey, string newKey)
		{
			this.refreshKey();
		}

		protected virtual void handleVersionChanged(TranslationLeaf leaf, int oldVersion, int newVersion)
		{
			this.refreshVersion();
		}

		protected virtual void handleKeySubmitted(Sleek2Field field, string value)
		{
			if (this.leaf != null)
			{
				this.leaf.branch.key = value;
				this.leaf.translation.isDirty = true;
			}
		}

		protected virtual void handleTextSubmitted(Sleek2Field field, string value)
		{
			if (this.leaf != null)
			{
				DevkitTransactionManager.beginTransaction(new TranslatedText(new TranslationReference("#SDG::Devkit.Transactions.Translation")));
				DevkitTransactionUtility.recordObjectDelta(this.leaf);
				DevkitTransactionUtility.recordObjectDelta(this.leaf.translation);
				this.leaf.text = value;
				this.leaf.version++;
				this.leaf.translation.isDirty = true;
				DevkitTransactionManager.endTransaction();
			}
		}

		protected override void triggerDestroyed()
		{
			if (this.leaf != null)
			{
				this.leaf.branch.keyChanged -= this.handleKeyChanged;
				this.leaf.versionChanged -= this.handleVersionChanged;
			}
			base.triggerDestroyed();
		}
	}
}
