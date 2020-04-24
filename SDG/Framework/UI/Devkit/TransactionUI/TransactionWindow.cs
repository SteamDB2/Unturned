using System;
using SDG.Framework.Devkit.Transactions;
using SDG.Framework.Translations;
using SDG.Framework.UI.Sleek2;
using UnityEngine;

namespace SDG.Framework.UI.Devkit.TransactionUI
{
	public class TransactionWindow : Sleek2Window
	{
		public TransactionWindow()
		{
			base.gameObject.name = "Inspector";
			base.tab.label.translation = new TranslatedText(new TranslationReference("SDG", "Devkit.Window.Transaction.Title"));
			base.tab.label.translation.format();
			this.view = new Sleek2Scrollview();
			this.view.transform.reset();
			this.view.transform.offsetMin = new Vector2(5f, 5f);
			this.view.transform.offsetMax = new Vector2(-5f, -5f);
			this.view.vertical = true;
			this.panel = new Sleek2VerticalScrollviewContents();
			this.panel.name = "Panel";
			this.view.panel = this.panel;
			this.addElement(this.view);
			this.updateList();
			DevkitTransactionManager.transactionPerformed += this.handleTransactionPerformed;
			DevkitTransactionManager.transactionsChanged += this.handleTransactionsChanged;
		}

		protected void updateList()
		{
			this.panel.clearElements();
			Sleek2TranslatedLabel sleek2TranslatedLabel = new Sleek2TranslatedLabel();
			sleek2TranslatedLabel.transform.anchorMin = new Vector2(0f, 1f);
			sleek2TranslatedLabel.transform.anchorMax = new Vector2(1f, 1f);
			sleek2TranslatedLabel.transform.pivot = new Vector2(0.5f, 1f);
			sleek2TranslatedLabel.transform.sizeDelta = new Vector2(0f, 50f);
			sleek2TranslatedLabel.translation = new TranslatedText(new TranslationReference("SDG", "Devkit.Window.Transaction.Undo"));
			sleek2TranslatedLabel.translation.format();
			this.panel.addElement(sleek2TranslatedLabel);
			foreach (DevkitTransactionGroup newGroup in DevkitTransactionManager.getUndoable())
			{
				TransactionUndoButton transactionUndoButton = new TransactionUndoButton(newGroup);
				transactionUndoButton.transform.anchorMin = new Vector2(0f, 1f);
				transactionUndoButton.transform.anchorMax = new Vector2(1f, 1f);
				transactionUndoButton.transform.pivot = new Vector2(0.5f, 1f);
				transactionUndoButton.transform.sizeDelta = new Vector2(0f, 30f);
				transactionUndoButton.clicked += this.handleUndoButtonClicked;
				this.panel.addElement(transactionUndoButton);
			}
			Sleek2TranslatedLabel sleek2TranslatedLabel2 = new Sleek2TranslatedLabel();
			sleek2TranslatedLabel2.transform.anchorMin = new Vector2(0f, 1f);
			sleek2TranslatedLabel2.transform.anchorMax = new Vector2(1f, 1f);
			sleek2TranslatedLabel2.transform.pivot = new Vector2(0.5f, 1f);
			sleek2TranslatedLabel2.transform.sizeDelta = new Vector2(0f, 50f);
			sleek2TranslatedLabel2.translation = new TranslatedText(new TranslationReference("SDG", "Devkit.Window.Transaction.Redo"));
			sleek2TranslatedLabel2.translation.format();
			this.panel.addElement(sleek2TranslatedLabel2);
			foreach (DevkitTransactionGroup newGroup2 in DevkitTransactionManager.getRedoable())
			{
				TransactionRedoButton transactionRedoButton = new TransactionRedoButton(newGroup2);
				transactionRedoButton.transform.anchorMin = new Vector2(0f, 1f);
				transactionRedoButton.transform.anchorMax = new Vector2(1f, 1f);
				transactionRedoButton.transform.pivot = new Vector2(0.5f, 1f);
				transactionRedoButton.transform.sizeDelta = new Vector2(0f, 30f);
				transactionRedoButton.clicked += this.handleRedoButtonClicked;
				this.panel.addElement(transactionRedoButton);
			}
		}

		protected void handleUndoButtonClicked(Sleek2ImageButton button)
		{
			DevkitTransactionGroup group = (button as TransactionUndoButton).group;
			for (DevkitTransactionGroup devkitTransactionGroup = null; devkitTransactionGroup != group; devkitTransactionGroup = DevkitTransactionManager.undo())
			{
			}
		}

		protected void handleRedoButtonClicked(Sleek2ImageButton button)
		{
			DevkitTransactionGroup group = (button as TransactionRedoButton).group;
			for (DevkitTransactionGroup devkitTransactionGroup = null; devkitTransactionGroup != group; devkitTransactionGroup = DevkitTransactionManager.redo())
			{
			}
		}

		protected void handleTransactionPerformed(DevkitTransactionGroup group)
		{
			this.updateList();
		}

		protected void handleTransactionsChanged()
		{
			this.updateList();
		}

		protected override void triggerDestroyed()
		{
			DevkitTransactionManager.transactionPerformed -= this.handleTransactionPerformed;
			DevkitTransactionManager.transactionsChanged -= this.handleTransactionsChanged;
			base.triggerDestroyed();
		}

		protected Sleek2Element panel;

		protected Sleek2Scrollview view;
	}
}
