using System;
using System.Reflection;
using SDG.Framework.Devkit;
using SDG.Framework.Devkit.Transactions;
using SDG.Framework.Translations;

namespace SDG.Framework.UI.Devkit.InspectorUI
{
	public class ObjectInspectableField : ObjectInspectableInfo
	{
		public ObjectInspectableField(ObjectInspectableInfo newParent, FieldInfo newField, IDirtyable newDirtyable, object newInstance, TranslationReference newName, TranslationReference newTooltip) : base(newParent, newDirtyable, newInstance, newField.FieldType, newName, newTooltip)
		{
			this.field = newField;
		}

		public FieldInfo field { get; protected set; }

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
				return this.field.GetValue(base.instance);
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
				TranslatedText translatedText = new TranslatedText(new TranslationReference("SDG", "Devkit.Transactions.Field_Delta"));
				translatedText.format(new object[]
				{
					base.instance,
					this.field.Name,
					(value2 == null) ? "nullptr" : value2.ToString(),
					(value == null) ? "nullptr" : value.ToString()
				});
				DevkitTransactionManager.beginTransaction(translatedText);
				DevkitTransactionUtility.recordObjectDelta(base.instance);
				this.field.SetValue(base.instance, value);
				if (base.dirtyable != null)
				{
					base.dirtyable.isDirty = true;
				}
				if (base.parent != null)
				{
					base.parent.copyValue(base.instance);
				}
				DevkitTransactionManager.endTransaction();
			}
		}

		public override void copyValue(object newValue)
		{
			if (base.type.IsValueType)
			{
				DevkitTransactionUtility.recordObjectDelta(base.instance);
				this.field.SetValue(base.instance, newValue);
			}
			if (base.parent != null)
			{
				base.parent.copyValue(base.instance);
			}
		}
	}
}
