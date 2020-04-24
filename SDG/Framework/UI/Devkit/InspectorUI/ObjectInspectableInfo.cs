using System;
using SDG.Framework.Debug;
using SDG.Framework.Devkit;
using SDG.Framework.Translations;

namespace SDG.Framework.UI.Devkit.InspectorUI
{
	public abstract class ObjectInspectableInfo
	{
		public ObjectInspectableInfo(ObjectInspectableInfo newParent, IDirtyable newDirtyable, object newInstance, Type newType, TranslationReference newName, TranslationReference newTooltip)
		{
			this.parent = newParent;
			this.dirtyable = newDirtyable;
			this.instance = newInstance;
			this.type = newType;
			this.name = newName;
			this.tooltip = newTooltip;
		}

		public ObjectInspectableInfo parent { get; protected set; }

		public IDirtyable dirtyable { get; protected set; }

		public object instance { get; protected set; }

		public Type type { get; protected set; }

		public TranslationReference name { get; protected set; }

		public TranslationReference tooltip { get; protected set; }

		public abstract bool canRead { get; }

		public abstract bool canWrite { get; }

		public abstract object value { get; set; }

		public abstract void copyValue(object newValue);

		public virtual void validate()
		{
			if (this.instance == null)
			{
				return;
			}
			if (this.instance is IInspectorValidateable)
			{
				IInspectorValidateable inspectorValidateable = this.instance as IInspectorValidateable;
				inspectorValidateable.inspectorValidate();
			}
		}
	}
}
