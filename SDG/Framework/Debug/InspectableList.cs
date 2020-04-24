using System;
using System.Collections.Generic;

namespace SDG.Framework.Debug
{
	public class InspectableList<T> : List<T>, IInspectableList
	{
		public InspectableList()
		{
			this.canInspectorAdd = true;
			this.canInspectorRemove = true;
		}

		public InspectableList(int capacity) : base(capacity)
		{
			this.canInspectorAdd = false;
			this.canInspectorRemove = false;
		}

		public InspectableList(IEnumerable<T> collection) : base(collection)
		{
			this.canInspectorAdd = true;
			this.canInspectorRemove = true;
		}

		public event InspectableListAddedHandler inspectorAdded;

		public event InspectableListRemovedHandler inspectorRemoved;

		public event InspectableListChangedHandler inspectorChanged;

		public new void Add(T item)
		{
			base.Add(item);
			this.triggerChanged();
		}

		public new bool Remove(T item)
		{
			bool result = base.Remove(item);
			this.triggerChanged();
			return result;
		}

		public new void RemoveAt(int index)
		{
			base.RemoveAt(index);
			this.triggerChanged();
		}

		public virtual void inspectorAdd(object instance)
		{
			this.triggerAdded(instance);
			this.triggerChanged();
		}

		public virtual void inspectorRemove(object instance)
		{
			this.triggerRemoved(instance);
			this.triggerChanged();
		}

		public virtual void inspectorSet(int index)
		{
			this.triggerChanged();
		}

		public virtual bool canInspectorAdd { get; set; }

		public virtual bool canInspectorRemove { get; set; }

		protected virtual void triggerAdded(object instance)
		{
			InspectableListAddedHandler inspectableListAddedHandler = this.inspectorAdded;
			if (inspectableListAddedHandler != null)
			{
				inspectableListAddedHandler(this, instance);
			}
		}

		protected virtual void triggerRemoved(object instance)
		{
			InspectableListRemovedHandler inspectableListRemovedHandler = this.inspectorRemoved;
			if (inspectableListRemovedHandler != null)
			{
				inspectableListRemovedHandler(this, instance);
			}
		}

		protected virtual void triggerChanged()
		{
			InspectableListChangedHandler inspectableListChangedHandler = this.inspectorChanged;
			if (inspectableListChangedHandler != null)
			{
				inspectableListChangedHandler(this);
			}
		}
	}
}
