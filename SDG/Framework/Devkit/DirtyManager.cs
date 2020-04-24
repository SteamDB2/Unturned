using System;
using System.Collections.Generic;

namespace SDG.Framework.Devkit
{
	public class DirtyManager
	{
		public static List<IDirtyable> dirty
		{
			get
			{
				return DirtyManager._dirty;
			}
		}

		public static HashSet<IDirtyable> notSaveable
		{
			get
			{
				return DirtyManager._notSaveable;
			}
		}

		public static event MarkedDirtyHandler markedDirty;

		public static event MarkedCleanHandler markedClean;

		public static event SaveableChangedHandler saveableChanged;

		public static event DirtySaved saved;

		public static void markDirty(IDirtyable item)
		{
			DirtyManager.dirty.Add(item);
			DirtyManager.triggerMarkedDirty(item);
		}

		public static void markClean(IDirtyable item)
		{
			if (DirtyManager.isSaving)
			{
				return;
			}
			DirtyManager.dirty.Remove(item);
			DirtyManager.triggerMarkedClean(item);
		}

		public static bool checkSaveable(IDirtyable item)
		{
			return !DirtyManager.notSaveable.Contains(item);
		}

		public static void toggleSaveable(IDirtyable item)
		{
			if (!DirtyManager.notSaveable.Remove(item))
			{
				DirtyManager.notSaveable.Add(item);
				DirtyManager.triggerSaveableChanged(item, true);
			}
			else
			{
				DirtyManager.triggerSaveableChanged(item, false);
			}
		}

		public static void save()
		{
			DirtyManager.isSaving = true;
			for (int i = DirtyManager.dirty.Count - 1; i >= 0; i--)
			{
				IDirtyable dirtyable = DirtyManager.dirty[i];
				if (!DirtyManager.notSaveable.Contains(dirtyable))
				{
					dirtyable.save();
					dirtyable.isDirty = false;
					DirtyManager.dirty.RemoveAt(i);
				}
			}
			DirtyManager.isSaving = false;
			DirtyManager.triggerSaved();
		}

		protected static void triggerMarkedDirty(IDirtyable item)
		{
			if (DirtyManager.markedDirty != null)
			{
				DirtyManager.markedDirty(item);
			}
		}

		protected static void triggerMarkedClean(IDirtyable item)
		{
			if (DirtyManager.markedClean != null)
			{
				DirtyManager.markedClean(item);
			}
		}

		protected static void triggerSaveableChanged(IDirtyable item, bool isSaveable)
		{
			if (DirtyManager.saveableChanged != null)
			{
				DirtyManager.saveableChanged(item, isSaveable);
			}
		}

		protected static void triggerSaved()
		{
			if (DirtyManager.saved != null)
			{
				DirtyManager.saved();
			}
		}

		protected static List<IDirtyable> _dirty = new List<IDirtyable>();

		public static HashSet<IDirtyable> _notSaveable = new HashSet<IDirtyable>();

		protected static bool isSaving;
	}
}
