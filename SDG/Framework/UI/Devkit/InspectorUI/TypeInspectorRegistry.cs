using System;
using System.Collections.Generic;
using SDG.Framework.Debug;
using SDG.Framework.Translations;
using SDG.Framework.UI.Devkit.InspectorUI.TypeInspectors;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.UI.Devkit.InspectorUI
{
	public static class TypeInspectorRegistry
	{
		static TypeInspectorRegistry()
		{
			TypeInspectorRegistry.add(typeof(bool), typeof(Sleek2BoolInspector));
			TypeInspectorRegistry.add(typeof(byte), typeof(Sleek2ByteInspector));
			TypeInspectorRegistry.add(typeof(double), typeof(Sleek2DoubleInspector));
			TypeInspectorRegistry.add(typeof(float), typeof(Sleek2FloatInspector));
			TypeInspectorRegistry.add(typeof(Guid), typeof(Sleek2GUIDInspector));
			TypeInspectorRegistry.add(typeof(int), typeof(Sleek2IntInspector));
			TypeInspectorRegistry.add(typeof(long), typeof(Sleek2LongInspector));
			TypeInspectorRegistry.add(typeof(sbyte), typeof(Sleek2SByteInspector));
			TypeInspectorRegistry.add(typeof(short), typeof(Sleek2ShortInspector));
			TypeInspectorRegistry.add(typeof(string), typeof(Sleek2StringInspector));
			TypeInspectorRegistry.add(typeof(uint), typeof(Sleek2UIntInspector));
			TypeInspectorRegistry.add(typeof(ulong), typeof(Sleek2ULongInspector));
			TypeInspectorRegistry.add(typeof(ushort), typeof(Sleek2UShortInspector));
			TypeInspectorRegistry.add(typeof(Color), typeof(Sleek2ColorInspector));
			TypeInspectorRegistry.add(typeof(Quaternion), typeof(Sleek2QuaternionInspector));
			TypeInspectorRegistry.add(typeof(Vector3), typeof(Sleek2Vector3Inspector));
			TypeInspectorRegistry.add(typeof(Vector4), typeof(Sleek2Vector4Inspector));
			TypeInspectorRegistry.add(typeof(AssetReference<>), typeof(Sleek2AssetReferenceInspector<>));
			TypeInspectorRegistry.add(typeof(ContentReference<>), typeof(Sleek2ContentReferenceInspector<>));
			TypeInspectorRegistry.add(typeof(InspectableFilePath), typeof(Sleek2FilePathInspector));
			TypeInspectorRegistry.add(typeof(InspectableDirectoryPath), typeof(Sleek2DirectoryPathInspector));
			TypeInspectorRegistry.add(typeof(TranslationReference), typeof(Sleek2TranslationReferenceInspector));
			TypeInspectorRegistry.add(typeof(TypeReference<>), typeof(Sleek2TypeReferenceInspector<>));
		}

		public static Sleek2TypeInspector inspect(Type targetType)
		{
			Type key = targetType;
			if (targetType.IsGenericType)
			{
				key = targetType.GetGenericTypeDefinition();
			}
			Type type;
			if (TypeInspectorRegistry.inspectors.TryGetValue(key, out type))
			{
				if (targetType.IsGenericType)
				{
					type = type.MakeGenericType(targetType.GetGenericArguments());
				}
				return Activator.CreateInstance(type) as Sleek2TypeInspector;
			}
			if (targetType.IsEnum)
			{
				object[] customAttributes = targetType.GetCustomAttributes(typeof(InspectableEnumAttribute), false);
				Sleek2TypeInspector result;
				if (customAttributes.Length > 0)
				{
					switch (((InspectableEnumAttribute)customAttributes[0]).inspectionMode)
					{
					case EEnumInspectionMode.DROPDOWN_SINGLE:
						result = new Sleek2SingleDropdownEnumInspector();
						break;
					case EEnumInspectionMode.DROPDOWN_MULTIPLE:
						result = new Sleek2MultipleDropdownEnumInspector();
						break;
					case EEnumInspectionMode.CYCLE_SINGLE:
						result = new Sleek2SingleCycleEnumInspector();
						break;
					default:
						Debug.LogError("Unknown inspectable enum mode!");
						return null;
					}
				}
				else if (targetType.GetCustomAttributes(typeof(FlagsAttribute), true).Length > 0)
				{
					result = new Sleek2MultipleDropdownEnumInspector();
				}
				else if (Enum.GetValues(targetType).Length > 3)
				{
					result = new Sleek2SingleDropdownEnumInspector();
				}
				else
				{
					result = new Sleek2SingleCycleEnumInspector();
				}
				return result;
			}
			return null;
		}

		public static void add(Type targetType, Type inspectorType)
		{
			if (typeof(Sleek2TypeInspector).IsAssignableFrom(inspectorType))
			{
				TypeInspectorRegistry.inspectors.Add(targetType, inspectorType);
			}
		}

		public static void remove(Type targetType)
		{
			TypeInspectorRegistry.inspectors.Remove(targetType);
		}

		private static Dictionary<Type, Type> inspectors = new Dictionary<Type, Type>();
	}
}
