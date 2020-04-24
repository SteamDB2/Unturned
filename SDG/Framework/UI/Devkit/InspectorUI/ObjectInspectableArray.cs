using System;
using SDG.Framework.Devkit;
using SDG.Framework.Translations;

namespace SDG.Framework.UI.Devkit.InspectorUI
{
	public class ObjectInspectableArray : ObjectInspectableInfo
	{
		public ObjectInspectableArray(ObjectInspectableInfo newParent, IDirtyable newDirtyable, Array newArray, Type newType, int newIndex, TranslationReference newName, TranslationReference newTooltip) : base(newParent, newDirtyable, newArray, newType, newName, newTooltip)
		{
			this.array = newArray;
			this.index = newIndex;
		}

		public Array array { get; protected set; }

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
				return this.array.GetValue(this.index);
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
				this.array.SetValue(value, this.index);
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
