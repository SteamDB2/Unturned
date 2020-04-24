using System;
using System.Collections.Generic;
using System.Reflection;
using SDG.Framework.Utilities;
using UnityEngine;

namespace SDG.Framework.Devkit.Transactions
{
	public class DevkitObjectDeltaTransaction : IDevkitTransaction
	{
		public DevkitObjectDeltaTransaction(object newInstance)
		{
			this.instance = newInstance;
		}

		public bool delta
		{
			get
			{
				return this.deltas.Count > 0;
			}
		}

		public void undo()
		{
			for (int i = 0; i < this.deltas.Count; i++)
			{
				this.deltas[i].undo(this.instance);
			}
		}

		public void redo()
		{
			for (int i = 0; i < this.deltas.Count; i++)
			{
				this.deltas[i].redo(this.instance);
			}
		}

		public void begin()
		{
			this.tempFields = ListPool<object>.claim();
			this.tempProperties = ListPool<object>.claim();
			Type type = this.instance.GetType();
			FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public);
			for (int i = 0; i < fields.Length; i++)
			{
				try
				{
					FieldInfo fieldInfo = fields[i];
					object value = fieldInfo.GetValue(this.instance);
					this.tempFields.Add(value);
				}
				catch
				{
					this.tempFields.Add(null);
				}
			}
			PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
			for (int j = 0; j < properties.Length; j++)
			{
				try
				{
					PropertyInfo propertyInfo = properties[j];
					if (propertyInfo.CanRead && propertyInfo.CanWrite)
					{
						object value2 = propertyInfo.GetValue(this.instance, null);
						this.tempProperties.Add(value2);
					}
					else
					{
						this.tempProperties.Add(null);
					}
				}
				catch
				{
					this.tempProperties.Add(null);
				}
			}
		}

		public void end()
		{
			this.deltas = ListPool<ITransactionDelta>.claim();
			Type type = this.instance.GetType();
			FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public);
			for (int i = 0; i < fields.Length; i++)
			{
				try
				{
					FieldInfo fieldInfo = fields[i];
					object value = fieldInfo.GetValue(this.instance);
					if (this.changed(this.tempFields[i], value))
					{
						this.deltas.Add(new TransactionFieldDelta(fieldInfo, this.tempFields[i], value));
					}
				}
				catch (Exception ex)
				{
					Debug.LogException(ex);
				}
			}
			PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
			for (int j = 0; j < properties.Length; j++)
			{
				try
				{
					PropertyInfo propertyInfo = properties[j];
					if (propertyInfo.CanRead && propertyInfo.CanWrite)
					{
						object value2 = propertyInfo.GetValue(this.instance, null);
						if (this.changed(this.tempProperties[j], value2))
						{
							this.deltas.Add(new TransactionPropertyDelta(propertyInfo, this.tempProperties[j], value2));
						}
					}
				}
				catch (Exception ex2)
				{
					Debug.LogException(ex2);
				}
			}
			ListPool<object>.release(this.tempFields);
			ListPool<object>.release(this.tempProperties);
		}

		public void forget()
		{
			if (this.deltas != null)
			{
				ListPool<ITransactionDelta>.release(this.deltas);
				this.deltas = null;
			}
		}

		protected bool changed(object before, object after)
		{
			if (before == null || after == null)
			{
				return before != after;
			}
			return !before.Equals(after);
		}

		protected object instance;

		protected List<object> tempFields;

		protected List<object> tempProperties;

		protected List<ITransactionDelta> deltas;
	}
}
