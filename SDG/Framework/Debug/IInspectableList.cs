using System;

namespace SDG.Framework.Debug
{
	public interface IInspectableList
	{
		event InspectableListAddedHandler inspectorAdded;

		event InspectableListRemovedHandler inspectorRemoved;

		event InspectableListChangedHandler inspectorChanged;

		void inspectorAdd(object instance);

		void inspectorRemove(object instance);

		void inspectorSet(int index);

		bool canInspectorAdd { get; set; }

		bool canInspectorRemove { get; set; }
	}
}
