using System;
using System.Reflection;

namespace SDG.Framework.Devkit.Transactions
{
	public struct TransactionFieldDelta : ITransactionDelta
	{
		public TransactionFieldDelta(FieldInfo newField)
		{
			this = new TransactionFieldDelta(newField, null, null);
		}

		public TransactionFieldDelta(FieldInfo newField, object newBefore, object newAfter)
		{
			this.field = newField;
			this.before = newBefore;
			this.after = newAfter;
		}

		public void undo(object instance)
		{
			this.field.SetValue(instance, this.before);
		}

		public void redo(object instance)
		{
			this.field.SetValue(instance, this.after);
		}

		public FieldInfo field;

		public object before;

		public object after;
	}
}
