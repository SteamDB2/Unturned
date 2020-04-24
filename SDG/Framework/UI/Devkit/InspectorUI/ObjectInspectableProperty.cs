using System;
using System.Reflection;
using SDG.Framework.Devkit;
using SDG.Framework.Devkit.Transactions;
using SDG.Framework.Translations;

namespace SDG.Framework.UI.Devkit.InspectorUI
{
	public class ObjectInspectableProperty : ObjectInspectableInfo
	{
		public ObjectInspectableProperty(ObjectInspectableInfo newParent, PropertyInfo newProperty, IDirtyable newDirtyable, object newInstance, TranslationReference newName, TranslationReference newTooltip) : base(newParent, newDirtyable, newInstance, newProperty.PropertyType, newName, newTooltip)
		{
			this.property = newProperty;
		}

		public PropertyInfo property { get; protected set; }

		public override bool canRead
		{
			get
			{
				return this.property.GetGetMethod() != null;
			}
		}

		public override bool canWrite
		{
			get
			{
				return this.property.GetSetMethod() != null;
			}
		}

		public override object value
		{
			get
			{
				return this.property.GetValue(base.instance, null);
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
				TranslatedText translatedText = new TranslatedText(new TranslationReference("SDG", "Devkit.Transactions.Property_Delta"));
				translatedText.format(new object[]
				{
					base.instance,
					this.property.Name,
					(value2 == null) ? "nullptr" : value2.ToString(),
					(value == null) ? "nullptr" : value.ToString()
				});
				DevkitTransactionManager.beginTransaction(translatedText);
				DevkitTransactionUtility.recordObjectDelta(base.instance);
				this.property.SetValue(base.instance, value, null);
				if (base.dirtyable != null)
				{
					base.dirtyable.isDirty = true;
				}
				DevkitTransactionManager.endTransaction();
			}
		}

		public override void copyValue(object newValue)
		{
			if (base.type.IsValueType)
			{
				DevkitTransactionUtility.recordObjectDelta(base.instance);
				this.property.SetValue(base.instance, newValue, null);
			}
			if (base.parent != null)
			{
				base.parent.copyValue(base.instance);
			}
		}
	}
}
