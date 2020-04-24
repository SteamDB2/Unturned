using System;
using System.Collections.Generic;
using SDG.Framework.Translations;

namespace SDG.Framework.Devkit.Transactions
{
	public class DevkitTransactionGroup
	{
		public DevkitTransactionGroup(TranslatedText newName)
		{
			this.name = newName;
			this.transactions = new List<IDevkitTransaction>();
		}

		public TranslatedText name { get; protected set; }

		public List<IDevkitTransaction> transactions { get; protected set; }

		public void record(IDevkitTransaction transaction)
		{
			transaction.begin();
			this.transactions.Add(transaction);
		}

		public bool delta
		{
			get
			{
				for (int i = this.transactions.Count - 1; i >= 0; i--)
				{
					if (!this.transactions[i].delta)
					{
						this.transactions.RemoveAt(i);
					}
				}
				return this.transactions.Count > 0;
			}
		}

		public void undo()
		{
			for (int i = 0; i < this.transactions.Count; i++)
			{
				this.transactions[i].undo();
			}
		}

		public void redo()
		{
			for (int i = 0; i < this.transactions.Count; i++)
			{
				this.transactions[i].redo();
			}
		}

		public void end()
		{
			for (int i = 0; i < this.transactions.Count; i++)
			{
				this.transactions[i].end();
			}
		}

		public void forget()
		{
			for (int i = 0; i < this.transactions.Count; i++)
			{
				this.transactions[i].forget();
			}
		}
	}
}
