using System;
using System.Reflection;

namespace SDG.Framework.Devkit.Transactions
{
	public struct TransactionPropertyDelta : ITransactionDelta
	{
		public TransactionPropertyDelta(PropertyInfo newProperty)
		{
			this = new TransactionPropertyDelta(newProperty, null, null);
		}

		public TransactionPropertyDelta(PropertyInfo newProperty, object newBefore, object newAfter)
		{
			this.property = newProperty;
			this.before = newBefore;
			this.after = newAfter;
		}

		public void undo(object instance)
		{
			this.property.SetValue(instance, this.before, null);
		}

		public void redo(object instance)
		{
			this.property.SetValue(instance, this.after, null);
		}

		public PropertyInfo property;

		public object before;

		public object after;
	}
}
