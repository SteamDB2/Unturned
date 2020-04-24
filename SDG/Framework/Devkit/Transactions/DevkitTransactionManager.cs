using System;
using System.Collections.Generic;
using SDG.Framework.Debug;
using SDG.Framework.Translations;

namespace SDG.Framework.Devkit.Transactions
{
	public class DevkitTransactionManager
	{
		[TerminalCommandProperty("transactions.history_length", "how many transactions to remember before forgetting", 25)]
		public static uint historyLength
		{
			get
			{
				return DevkitTransactionManager._historyLength;
			}
			set
			{
				DevkitTransactionManager._historyLength = value;
				TerminalUtility.printCommandPass("Set history_length to: " + DevkitTransactionManager.historyLength);
			}
		}

		public static event DevkitTransactionPerformedHandler transactionPerformed;

		protected static void triggerTransactionPerformed(DevkitTransactionGroup group)
		{
			if (DevkitTransactionManager.transactionPerformed != null)
			{
				DevkitTransactionManager.transactionPerformed(group);
			}
		}

		public static event DevkitTransactionsChangedHandler transactionsChanged;

		protected static void triggerTransactionsChanged()
		{
			if (DevkitTransactionManager.transactionsChanged != null)
			{
				DevkitTransactionManager.transactionsChanged();
			}
		}

		public static bool canUndo
		{
			get
			{
				return DevkitTransactionManager.undoable.Count > 0;
			}
		}

		public static bool canRedo
		{
			get
			{
				return DevkitTransactionManager.redoable.Count > 0;
			}
		}

		public static IEnumerable<DevkitTransactionGroup> getUndoable()
		{
			return DevkitTransactionManager.undoable;
		}

		public static IEnumerable<DevkitTransactionGroup> getRedoable()
		{
			return DevkitTransactionManager.redoable;
		}

		public static DevkitTransactionGroup undo()
		{
			if (!DevkitTransactionManager.canUndo)
			{
				return null;
			}
			DevkitTransactionGroup devkitTransactionGroup = DevkitTransactionManager.popUndo();
			devkitTransactionGroup.undo();
			DevkitTransactionManager.pushRedo(devkitTransactionGroup);
			DevkitTransactionManager.triggerTransactionPerformed(devkitTransactionGroup);
			return devkitTransactionGroup;
		}

		public static DevkitTransactionGroup redo()
		{
			if (!DevkitTransactionManager.canRedo)
			{
				return null;
			}
			DevkitTransactionGroup devkitTransactionGroup = DevkitTransactionManager.popRedo();
			devkitTransactionGroup.redo();
			DevkitTransactionManager.pushUndo(devkitTransactionGroup);
			DevkitTransactionManager.triggerTransactionPerformed(devkitTransactionGroup);
			return devkitTransactionGroup;
		}

		public static void beginTransaction(TranslatedText name)
		{
			if (DevkitTransactionManager.transactionDepth == 0)
			{
				DevkitTransactionManager.clearRedo();
				DevkitTransactionManager.pendingGroup = new DevkitTransactionGroup(name);
			}
			DevkitTransactionManager.transactionDepth++;
		}

		public static void recordTransaction(IDevkitTransaction transaction)
		{
			if (DevkitTransactionManager.pendingGroup == null)
			{
				return;
			}
			DevkitTransactionManager.pendingGroup.record(transaction);
		}

		public static void endTransaction()
		{
			if (DevkitTransactionManager.transactionDepth == 0)
			{
				return;
			}
			DevkitTransactionManager.transactionDepth--;
			if (DevkitTransactionManager.transactionDepth == 0)
			{
				DevkitTransactionManager.pendingGroup.end();
				if (DevkitTransactionManager.pendingGroup.delta)
				{
					DevkitTransactionManager.pushUndo(DevkitTransactionManager.pendingGroup);
				}
				else
				{
					DevkitTransactionManager.pendingGroup.forget();
				}
				DevkitTransactionManager.pendingGroup = null;
				DevkitTransactionManager.triggerTransactionsChanged();
			}
		}

		public static void resetTransactions()
		{
			DevkitTransactionManager.clearUndo();
			DevkitTransactionManager.clearRedo();
			DevkitTransactionManager.pendingGroup = null;
			DevkitTransactionManager.transactionDepth = 0;
		}

		protected static void pushUndo(DevkitTransactionGroup group)
		{
			if ((long)DevkitTransactionManager.undoable.Count >= (long)((ulong)DevkitTransactionManager.historyLength))
			{
				DevkitTransactionManager.undoable.First.Value.forget();
				DevkitTransactionManager.undoable.RemoveFirst();
			}
			DevkitTransactionManager.undoable.AddLast(group);
		}

		protected static DevkitTransactionGroup popUndo()
		{
			DevkitTransactionGroup value = DevkitTransactionManager.undoable.Last.Value;
			DevkitTransactionManager.undoable.RemoveLast();
			return value;
		}

		protected static void clearUndo()
		{
			while (DevkitTransactionManager.undoable.Count > 0)
			{
				DevkitTransactionGroup value = DevkitTransactionManager.undoable.Last.Value;
				DevkitTransactionManager.undoable.RemoveLast();
				value.forget();
			}
			DevkitTransactionManager.undoable.Clear();
		}

		protected static void pushRedo(DevkitTransactionGroup group)
		{
			DevkitTransactionManager.redoable.Push(group);
		}

		protected static DevkitTransactionGroup popRedo()
		{
			return DevkitTransactionManager.redoable.Pop();
		}

		protected static void clearRedo()
		{
			while (DevkitTransactionManager.redoable.Count > 0)
			{
				DevkitTransactionGroup devkitTransactionGroup = DevkitTransactionManager.redoable.Pop();
				devkitTransactionGroup.forget();
			}
			DevkitTransactionManager.redoable.Clear();
		}

		private static uint _historyLength = 25u;

		protected static LinkedList<DevkitTransactionGroup> undoable = new LinkedList<DevkitTransactionGroup>();

		protected static Stack<DevkitTransactionGroup> redoable = new Stack<DevkitTransactionGroup>();

		protected static DevkitTransactionGroup pendingGroup;

		protected static int transactionDepth;
	}
}
