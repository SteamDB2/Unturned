using System;
using System.Reflection;
using System.Reflection.Emit;

namespace System.Collections.Generic
{
	public static class ListExtension
	{
		public static void RemoveAtFast<T>(this List<T> list, int index)
		{
			list[index] = list[list.Count - 1];
			list.RemoveAt(list.Count - 1);
		}

		public static void RemoveFast<T>(this List<T> list, T item)
		{
			int num = list.IndexOf(item);
			if (num < 0)
			{
				return;
			}
			list.RemoveAtFast(num);
		}

		public static T[] GetInternalArray<T>(this List<T> list)
		{
			return ListExtension.ListInternalArrayAccessor<T>.Getter(list);
		}

		private static class ListInternalArrayAccessor<T>
		{
			static ListInternalArrayAccessor()
			{
				DynamicMethod dynamicMethod = new DynamicMethod("get", MethodAttributes.FamANDAssem | MethodAttributes.Family | MethodAttributes.Static, CallingConventions.Standard, typeof(T[]), new Type[]
				{
					typeof(List<T>)
				}, typeof(ListExtension.ListInternalArrayAccessor<T>), true);
				ILGenerator ilgenerator = dynamicMethod.GetILGenerator();
				ilgenerator.Emit(OpCodes.Ldarg_0);
				ilgenerator.Emit(OpCodes.Ldfld, typeof(List<T>).GetField("_items", BindingFlags.Instance | BindingFlags.NonPublic));
				ilgenerator.Emit(OpCodes.Ret);
				ListExtension.ListInternalArrayAccessor<T>.Getter = (Func<List<T>, T[]>)dynamicMethod.CreateDelegate(typeof(Func<List<T>, T[]>));
			}

			public static Func<List<T>, T[]> Getter;
		}
	}
}
