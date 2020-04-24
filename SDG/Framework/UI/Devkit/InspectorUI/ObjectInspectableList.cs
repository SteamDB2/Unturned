using System;
using System.Collections;
using SDG.Framework.Debug;
using SDG.Framework.Devkit;
using SDG.Framework.Translations;

namespace SDG.Framework.UI.Devkit.InspectorUI
{
	public class ObjectInspectableList : ObjectInspectableInfo
	{
		public ObjectInspectableList(ObjectInspectableInfo newParent, IDirtyable newDirtyable, IList newList, Type newType, int newIndex, TranslationReference newName, TranslationReference newTooltip) : base(newParent, newDirtyable, newList, newType, newName, newTooltip)
		{
			this.list = newList;
			this.index = newIndex;
		}

		public IList list { get; protected set; }

		public int index { get; protected set; }

		public override bool canRead
		{
			get
			{
				return true;
			}
		}

		public override bool canWrite
		{
			get
			{
				return true;
			}
		}

		public override object value
		{
			get
			{
				return this.list[this.index];
			}
			set
			{
				object value2 = this.value;
				if (value2 == null || value == null)
				{
					if (value2 == value)
					{
						return;
					}
				}
				else if (value2.Equals(value))
				{
					return;
				}
				this.list[this.index] = value;
				if (this.list is IInspectableList)
				{
					IInspectableList inspectableList = this.list as IInspectableList;
					inspectableList.inspectorSet(this.index);
				}
			}
		}

		public override void copyValue(object newValue)
		{
			if (base.parent != null)
			{
				base.parent.copyValue(base.instance);
			}
		}
	}
}
