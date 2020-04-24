using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace UnityEditor.PostProcessing
{
	public static class ReflectionUtils
	{
		public static FieldInfo GetFieldInfoFromPath(object source, string path)
		{
			FieldInfo fieldInfo = null;
			KeyValuePair<object, string> key = new KeyValuePair<object, string>(source, path);
			if (!ReflectionUtils.s_FieldInfoFromPaths.TryGetValue(key, out fieldInfo))
			{
				string[] array = path.Split(new char[]
				{
					'.'
				});
				Type type = source.GetType();
				foreach (string name in array)
				{
					fieldInfo = type.GetField(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					if (fieldInfo == null)
					{
						break;
					}
					type = fieldInfo.FieldType;
				}
				ReflectionUtils.s_FieldInfoFromPaths.Add(key, fieldInfo);
			}
			return fieldInfo;
		}

		public static string GetFieldPath<T, TValue>(Expression<Func<T, TValue>> expr)
		{
			ExpressionType nodeType = expr.Body.NodeType;
			MemberExpression memberExpression;
			if (nodeType != ExpressionType.Convert && nodeType != ExpressionType.ConvertChecked)
			{
				memberExpression = (expr.Body as MemberExpression);
			}
			else
			{
				UnaryExpression unaryExpression = expr.Body as UnaryExpression;
				memberExpression = (((unaryExpression == null) ? null : unaryExpression.Operand) as MemberExpression);
			}
			List<string> list = new List<string>();
			while (memberExpression != null)
			{
				list.Add(memberExpression.Member.Name);
				memberExpression = (memberExpression.Expression as MemberExpression);
			}
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = list.Count - 1; i >= 0; i--)
			{
				stringBuilder.Append(list[i]);
				if (i > 0)
				{
					stringBuilder.Append('.');
				}
			}
			return stringBuilder.ToString();
		}

		public static object GetFieldValue(object source, string name)
		{
			for (Type type = source.GetType(); type != null; type = type.BaseType)
			{
				FieldInfo field = type.GetField(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				if (field != null)
				{
					return field.GetValue(source);
				}
			}
			return null;
		}

		public static object GetFieldValueFromPath(object source, ref Type baseType, string path)
		{
			string[] array = path.Split(new char[]
			{
				'.'
			});
			object obj = source;
			foreach (string name in array)
			{
				FieldInfo field = baseType.GetField(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				if (field == null)
				{
					baseType = null;
					break;
				}
				baseType = field.FieldType;
				obj = ReflectionUtils.GetFieldValue(obj, name);
			}
			return (baseType != null) ? obj : null;
		}

		public static object GetParentObject(string path, object obj)
		{
			string[] array = path.Split(new char[]
			{
				'.'
			});
			if (array.Length == 1)
			{
				return obj;
			}
			FieldInfo field = obj.GetType().GetField(array[0], BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			obj = field.GetValue(obj);
			return ReflectionUtils.GetParentObject(string.Join(".", array, 1, array.Length - 1), obj);
		}

		private static Dictionary<KeyValuePair<object, string>, FieldInfo> s_FieldInfoFromPaths = new Dictionary<KeyValuePair<object, string>, FieldInfo>();
	}
}
